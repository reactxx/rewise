import 'package:rw_utils/utils.dart' show Linq;

class Parsed {
  int idx;
  int lessonId;
  String text;
  String breakText;
  String wcls;
  List<Bracket> brackets;
  List<ParsedFact> child;
}

Iterable<Parsed> parseFactTextFormat(String str) sync* {
  yield Parsed()..breakText = str;
}

class Bracket {
  String type;
  String value;
}

class Error {
  Error(this.id, this.code);
  String id;
  int code;
}

class ParsedFact {

  String wcls;
  String text;
  String toBreakText;
  List<Bracket> brackets;
  List<ParsedFact> childs;
  List<Error> errors;
  String id;
  String delim;
  getToBreakText() => toBreakText ?? text;

  ParsedFact(this.text,
      [this.errors,
      List<String> delims = const [Consts.wCls, Consts.wMean, Consts.wSyn],
      this.id = '0']) {
    if (_isRoot) errors = List<Error>();
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
    // raw syntax check
    if (_isLeaf && !_factTestRx.hasMatch(text)) {
      brackets = [];
      _addError(Consts.eSyntax);
      return;
    }
    // leaf => parsing brackets
    if (_isLeaf) {
      brackets = List<Bracket>();
      text = _processBrakets(false);
      toBreakText = _processBrakets(true);
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
        errors = null;
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

  _processBrakets(bool forBreakText) {
    return text.replaceAllMapped(_factBracketsRx, (m) {
      final br = _bracketFromGroup(m, 1) ??
          _bracketFromGroup(m, 2) ??
          _bracketFromGroup(m, 3);
      assert(br != null);
      if (!forBreakText) brackets.add(br);
      if (br.type == Consts.brSq) return '';
      return forBreakText ? ''.padRight(m.end - m.start, ' ') : m.group(0);
    });
  }

  _addError(int err) => errors.add(Error(id, err));

  bool get _isLeaf => childs == null;
  bool get _isRoot => id == '0';

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

  Bracket _bracketFromGroup(Match m, int idx) {
    final val = m.group(idx);
    if (val == null) return null;
    return Bracket()
      ..type = _types[idx]
      ..value = m.input.substring(m.start + 1, m.end - 1);
  }

  const _types = <String>[null, Consts.br, Consts.brSq, Consts.brCur];


final _factTestRx = RegExp(
    r'^(\([^(){}\[\],|^]+\)|\[\w+\]|{[^(){}\[\],|^]+}|[^(){}\[\],|^]*)+$');
final _factBracketsRx =
    RegExp(r'(\([^(){}\[\],|^]+\))|(\[\w+\])|({[^(){}\[\],|^]+})');
