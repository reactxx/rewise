import 'package:rw_low/code.dart' show Linq;
import 'package:rw_utils/dom/to_parsed.dart' as toPars;
import 'parseText.dart';

var devCount = 0;

class Consts {
  static const br = "(";
  static const brSq = "[";
  static const brCur = "{";
  static const wCls = "|"; // word class
  static const wMean = "^"; // word meaning
  static const wSyn = ","; // synonymous
  static const eSyntax = 1; // raw syntax errpr
  static const eWClsMissing = 2; // missing wCls for | -delimited chids
  static const eWClsOther = 3; // wCls is not in first child's item
  static const eWClsMore = 5; // more than single wCls in leaf item
  static const eEmptyWithoutBrackets = 4; // empty without brakets
  static const eEmptyBracket = 5; // some empty braket
}

//***************************** PRIVATE ************/

class _Error {
  _Error(this.id, this.code);
  String id;
  int code;
}

// visible for testing
class ParsedFact {
  String wcls;
  String text;
  String toBreakText;
  List<Bracket> brackets;
  List<ParsedFact> childs;
  List<_Error> errors;
  String id;
  String delim;
  getToBreakText() => toBreakText ?? text;
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
        ..breakText = ch.toBreakText ?? ''));
    else
      msg.childs.add(toPars.ParsedSubFact()
        ..wordClass = wcls ?? ''
        ..text = text
        ..breakText = toBreakText ?? '');

    if (errors.length != 0) {
      err.write('FACT: $idx');
      for (final er in errors) err.writeln('  ${er.id}: ${er.code}; ');
    }
  }

  ParsedFact(this.text,
      [this.errors,
      List<String> delims = const [Consts.wCls, Consts.wMean, Consts.wSyn],
      this.id = '0']) {
    if (_isRoot) errors = List<_Error>();
    // split fact by to tree by deimiters:|^,
    for (var delim in delims) {
      final d = text.split(delim);
      if (d.length == 1) continue;
      var childIdx = 0;
      childs = d
          .map((s) => ParsedFact(
              s, errors, delims.skip(1).toList(), '$id.${childIdx++}'))
          .toList();
      this.delim = delim;
      break;
    }
    // leaf => parsing brackets
    if (_isLeaf) {
      final parseRes = parse(text);
      if (!parseRes.valid) {
        brackets = [];
        _addError(Consts.eSyntax);
        return;
      }
      brackets = parseRes.brakets;
      text = parseRes.str;
      toBreakText = parseRes.breakStr;
      // 'empty without brakets' error
      if (toBreakText != null && toBreakText.trim().isEmpty && text.isNotEmpty)
        _addError(Consts.eEmptyWithoutBrackets);
    }
    // root:
    if (_isRoot) {
      //check and assign word class
      final toCheck = delim == Consts.wCls ? childs : [this];
      for (final toCh in toCheck) {
        String cls;
        for (final ch in toCh._childDeep()) {
          final sqs = ch.brackets.where((b) => b.type == Consts.brSq).toList();
          if (sqs.isEmpty) continue;
          if (sqs.length > 1)
            _addError(Consts.eWClsMore);
          else if (!_firstChildChainRx
              .hasMatch(ch.id.substring(toCh.id.length)))
            _addError(Consts.eWClsOther);
          else if (cls != null)
            assert(false);
          else
            cls = sqs[0].value;
        }
        if (delim == Consts.wCls && cls == null)
          _addError(Consts.eWClsMissing);
        else if (cls != null) for (final ch in toCh._childDeep()) ch.wcls = cls;
      }
      // no error => join synonymous and remove non-leaf
      if (errors.length == 0) {
        brackets = Linq.selectMany(_childDeep(), (ParsedFact ch) => ch.brackets)
            .toList();
        if (!brackets.every((b) => b.value.isNotEmpty))
          _addError(Consts.eEmptyBracket);
        for (final ch
            in _childDeep(false).where((ch) => ch.delim == Consts.wSyn)) {
          ch.text = ch.childs.map((cc) => cc.text).join(', ');
          ch.wcls = ch.childs[0].wcls;
          ch.toBreakText =
              ch.childs.map((cc) => cc.getToBreakText()).join(', ');
          ch.childs = null;
        }
        // linearize childs
        if (childs != null) childs = _childDeep().toList();
        // for (final ch in childs)
        //   if (ch.text == ch.toBreakText) ch.toBreakText = null;
      }
    }
  }
  String get devText =>
      childs == null ? text : childs.map((ch) => ch.text).join(', ');
  String get devBreakText => childs == null
      ? getToBreakText()
      : childs.map((ch) => ch.getToBreakText()).join(', ');
  static final _firstChildChainRx = RegExp(r'^[0.]*$');
  Iterable<ParsedFact> _childDeep([bool leafOnly = true]) sync* {
    if (childs == null || !leafOnly) yield this;
    if (childs != null) for (final ch in childs) yield* ch._childDeep(leafOnly);
  }

  // String _processBrakets(bool forBreakText) {
  //   return text.replaceAllMapped(_factBracketsRx, (m) {
  //     final br = _bracketFromGroup(m, 1) ??
  //         _bracketFromGroup(m, 2) ??
  //         _bracketFromGroup(m, 3);
  //     assert(br != null);
  //     if (!forBreakText) brackets.add(br);
  //     if (br.type == Consts.brSq) return '';
  //     return forBreakText ? ''.padRight(m.end - m.start, ' ') : m.group(0);
  //   });
  // }

  _addError(int err) => errors.add(_Error(id, err));

  bool get _isLeaf => childs == null;
  bool get _isRoot => id == '0';
}

//const _types = <String>[null, Consts.br, Consts.brSq, Consts.brCur];

// final _factTestRx = RegExp(
//     //r'^(\([^(){}\[\],|^]+\)|\[\.+?\]|{[^(){}\[\],|^]+}|[^(){}\[\],|^]*)+$');
//     r'^(\([^(){}\[\],|^]+\)|\[\.+?\]|{[^(){}\[\],|^]+}|[^({\[]*)+$');
// final _factBracketsRx =
//     RegExp(r'(\([^(){}\[\],|^]+\))|(\[\.+?\])|({[^(){}\[\],|^]+})');
