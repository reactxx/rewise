import 'dart:collection';
import 'package:rw_utils/dom/to_parsed.dart' as toPars;

import 'fsm.dart' as fsm;

FactState parseMachine(String input) {
  final res = FactState.asRoot(fsm.StateMachine(input));
  res.st.start(res);
  return res;
}

class Error {
  Error(this.pos, this.code);
  int pos;
  ErrorCodes code;
}

enum ErrorCodes {
  missingWordClass,
  wordClassInOthers,
  closeBracketWithoutOpenOne,
  emptyFactText,
  emptyFactBreakText,
  missingCloseBracket,
  emptyBracket,
  moreWordClasses,
}

abstract class IState extends fsm.IState {
  IState._();
  IState(this.caller)
      : st = caller.st,
        root = caller.root;
  fsm.StateMachine st;
  IState caller;
  FactState root;
  addError(ErrorCodes code) {
    root.errors.add(Error(st.pos, code));
  }
}

class FactState extends IState {
  FactState.asRoot(fsm.StateMachine st) : super._() {
    this.st = st;
    root = this;
  }
  final brackets = List<BracketState>();
  final errors = List<Error>();
  final childs = List<SubFactState>();
  var subFactsDelims = '';
  run() {
    if (st.eos) {
      st.pop();
      return;
    }
    childs.add(SubFactState(this));
    st.push(childs.last);
  }

  popped() {
    void checkCls(SubFactState sf, bool mustBe) {
      if ((sf.wcls != null) == mustBe) return;
      addError(mustBe
          ? ErrorCodes.missingWordClass
          : ErrorCodes.wordClassInOthers);
    }

    final types = subFactsDelims.split('|');
    var ignoreFirst = types.length == 1;
    var idx = 0;
    for (final wclsGroup in types) {
      if (!ignoreFirst) checkCls(childs[idx], true);
      if (childs.length > 1)
        for (final f in childs.skip(idx + 1).take(wclsGroup.length))
          checkCls(f, false);
      idx += wclsGroup.length + 1;
      ignoreFirst = false;
    }
  }

  String get devText => childs.map((ch) => ch.text).join(', ');
  String get devBreakText => childs.map((ch) => ch.getToBreakText()).join(', ');

  void toMsg(
      int idx, toPars.ParsedFact msg, toPars.BracketBook br, StringBuffer err) {
    if (brackets != null)
      br.facts.addAll(brackets.map((br) => toPars.Bracket()
        ..value = br.value
        ..type = br.type
        ..factIdx = idx));
    msg.idx = idx;

    if (childs != null)
      msg.childs.addAll(childs.map((ch) => toPars.ParsedSubFact()
        ..wordClass = ch.wcls ?? ''
        ..text = ch.text
        ..breakText = ch.textBreak ?? ''));

    if (errors.length != 0) {
      err.writeln('FACT:$idx ${st.input} ');
      for (final er in errors) err.writeln('  ${er.pos}: ${er.code}; ');
    }
  }
}

class SubFactState extends IState {
  SubFactState(IState caller) : super(caller);
  String text;
  String textBreak;
  String wcls;
  final _text = StringBuffer();
  final _textBreak = StringBuffer();
  run() {
    final readed = _subfactCond.readTo(st);
    addToTexts(readed);
    switch (readed.actChar) {
      case '|':
      case '^':
      case ',':
      case fsm.eosChar:
        root.subFactsDelims += st.actChar;
        st.moveAhead();
        return st.pop();
      case '(':
      case '{':
      case '[':
        final br = BracketState(this, st.actChar);
        st.push(br);
        root.brackets.add(br);
        return st.moveAhead();
      case fsm.errorChar:
        st.moveAhead();
        return addError(ErrorCodes.closeBracketWithoutOpenOne);
      default:
        throw Exception();
    }
  }

  addToTexts(fsm.StrPart subStr, {bool breakWithSpace = false}) {
    if (subStr.start >= subStr.end) return;
    var txt = st.input.substring(subStr.start, subStr.end);
    _text.write(txt);
    if (breakWithSpace) txt = ''.padRight(subStr.end - subStr.start);
    _textBreak.write(txt);
  }

  popped() {
    text = _text.toString();
    textBreak = _textBreak.toString();
    if (text.trim().isEmpty)
      return addError(ErrorCodes.emptyFactText);
    else if (textBreak.trim().isEmpty)
      return addError(ErrorCodes.emptyFactBreakText);
    if (text == textBreak) textBreak = null;
  }

  getToBreakText() => textBreak ?? text;

  static final _subfactCond = _ReadToCondition(to: '({[|^,', error: ')}]');
}

class BracketState extends IState {
  BracketState(IState caller, this.type) : super(caller);
  String type;
  String value;
  run() {
    final readed = _bracketCond[type].readTo(st);
    switch (readed.actChar) {
      case '(':
        final b = BracketState(this, st.actChar);
        st.push(b);
        return st.moveAhead();
      case ')':
      case '}':
      case ']':
        st.moveAhead();
        return st.pop();
      case fsm.errorChar:
      case fsm.eosChar:
        st.pop();
        return addError(ErrorCodes.missingCloseBracket);
      default:
        throw Exception();
    }
  }

  popped() {
    if (caller is! SubFactState) return;
    final clr = caller as SubFactState;
    if (end - start <= 2) {
      addError(ErrorCodes.emptyBracket);
      return;
    }
    value = st.input.substring(start + 1, end - 1);
    switch (type) {
      case '{':
      case '(':
        clr.addToTexts(this, breakWithSpace: true);
        break;
      case '[':
        if (clr.wcls != null) addError(ErrorCodes.moreWordClasses);
        clr.wcls = value;
        break;
    }
  }

  static final _bracketCond = <String, _ReadToCondition>{
    '(': _ReadToCondition(to: ')(', error: '|^'),
    '[': _ReadToCondition(to: ']', error: '|^()[{}'),
    '{': _ReadToCondition(to: '}', error: '|^()[]{'),
  };
}

class _ReadCondResult extends fsm.StrPart {
  String actChar;
}

class _ReadToCondition {
  _ReadToCondition({String to, String error})
      : _to = to == null ? null : HashSet<int>.from(to.codeUnits),
        _error = error == null ? null : HashSet<int>.from(error.codeUnits);
  _ReadCondResult readTo(fsm.StateMachine st) {
    final res = _ReadCondResult()..start = st.pos;
    while (true) {
      if (st.eos) break;
      if (_error != null && _error.contains(st.act))
        return res
          ..end = st.pos
          ..actChar = fsm.errorChar;
      if (_to != null && _to.contains(st.act)) break;
      st.read();
    }
    return res
      ..end = st.pos
      ..actChar = st.eos ? fsm.eosChar : st.actChar;
  }

  final HashSet<int> _to;
  final HashSet<int> _error;
}
