import 'package:rw_low/code.dart' show Linq;
import 'package:rw_utils/dom/to_parsed.dart' as toPars;
import 'package:rw_utils/dom/word_breaking.dart' as wbreak;
import 'parseText.dart';

class ParseBookResult {
  ParseBookResult(this.book, this.brakets, this.errors);
  toPars.ParsedBooks book;
  toPars.BracketBooks brakets;
  Map<String, StringBuffer> errors;
}

ParseBookResult parsebook(toPars.RawBooks rawBooks) {
  // all langs
  final parsedBooks = toPars.ParsedBooks()..name = rawBooks.name;
  final bracketBooks = toPars.BracketBooks()..name = rawBooks.name;
  final errorsBooks = Map<String, StringBuffer>();
  // for each lang
  for (final rawBook in rawBooks.books) {
    // create msg version
    final parsedBook = toPars.ParsedBook()..lang = rawBook.lang;
    final bracketBook = toPars.BracketBook()..lang = rawBook.lang;
    parsedBooks.books.add(parsedBook);
    bracketBooks.books.add(bracketBook);
    final errors = StringBuffer();
    errorsBooks[rawBook.lang] = errors;
    //  for each fact
    for (var idx = 0; idx < rawBook.facts.length; idx++) {
      // create msg version of fact
      devCount++;
      if (devCount & 0x4ff == 0) print(devCount);
      final msgFact = toPars.ParsedFact()
        ..lessonId =
            rawBooks.lessons.length > 0 ? rawBooks.lessons[idx] + 1 : 0;
      parsedBook.facts.add(msgFact);
      // MAIN PROC: parse single source fact text and fill msg by parsed fact
      ParsedFact(rawBook.facts[idx]).toMsg(idx, msgFact, bracketBook, errors);
    }
  }
  return ParseBookResult(parsedBooks, bracketBooks, errorsBooks);
}

var devCount = 0;

Iterable<toPars.ParsedSubFact> forBreaking(toPars.ParsedBook book) =>
    Linq.selectMany(book.facts, (toPars.ParsedFact f) => f.childs);

megreBreaking(toPars.ParsedBook book, wbreak.Response breaks) {
  for (final pair in Linq.zip(forBreaking(book), breaks.facts))
    pair.item1.breaks = pair.item2.breaks;
}

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
      }
      brackets = parseRes.brakets;
      text = parseRes.str;
      toBreakText = parseRes.breakStr;
      // 'empty without brakets' error
      if (toBreakText.trim().isEmpty && text.isNotEmpty)
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
        for (final ch
            in _childDeep(false).where((ch) => ch.delim == Consts.wSyn)) {
          ch.text = ch.childs.map((cc) => cc.text).join(', ');
          ch.wcls = ch.childs[0].wcls;
          ch.toBreakText = ch.childs.map((cc) => cc.toBreakText).join(', ');
          ch.childs = null;
        }
        childs = _childDeep().toList();
        for (final ch in childs)
          if (ch.text == ch.toBreakText) ch.toBreakText = null;
      }
    }
  }
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

const _types = <String>[null, Consts.br, Consts.brSq, Consts.brCur];

// final _factTestRx = RegExp(
//     //r'^(\([^(){}\[\],|^]+\)|\[\.+?\]|{[^(){}\[\],|^]+}|[^(){}\[\],|^]*)+$');
//     r'^(\([^(){}\[\],|^]+\)|\[\.+?\]|{[^(){}\[\],|^]+}|[^({\[]*)+$');
// final _factBracketsRx =
//     RegExp(r'(\([^(){}\[\],|^]+\))|(\[\.+?\])|({[^(){}\[\],|^]+})');
