import 'package:rewise_low_utils/utils.dart' show Linq;
import 'package:tuple/tuple.dart';

class Parsed {
  String text;
  String breakText;

  String wcls;
  String get stemmingText => _stemmingText ?? text;
  String _stemmingText;
  List<Bracket> brackets;
  List<ParsedFact> child;
  List<Tuple2<String, int>> errors;

}

Iterable<Parsed> parseFactTextFormat(String str) sync* {
  yield Parsed()..breakText = str;
}

class Bracket {
  String type;
  String value;
  static Bracket fromGroup(Match m, int idx) {
    final val = m.group(idx);
    if (val == null) return null;
    return Bracket()
      ..type = _types[idx]
      ..value = m.input.substring(m.start + 1, m.end - 1);
  }

  static const _types = <String>[null, Consts.br, Consts.brSq, Consts.brCur];
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

class ParsedFact {
  ParsedFact(this.text,
      [this.errors,
      List<String> delims = const [Consts.wCls, Consts.wMean, Consts.wSyn],
      this.id = '0']) {
    if (_isRoot) errors = List<Tuple2<String, int>>();
    // split fact by to tree by deimiters:|^,
    for (var delim in delims) {
      final d = text.split(delim);
      if (d.length == 1) continue;
      var childIdx = 0;
      child = d
          .map((s) =>
              ParsedFact(s, errors, delims.skip(1).toList(), '$id.${childIdx++}'))
          .toList();
      this.delim = delim;
      break;
    }
    // raw syntax check
    if (_isLeaf && !factTestRx.hasMatch(text)) {
      brackets = [];
      _addError(Consts.eSyntax);
      return;
    }
    // leaf => parsing brackets
    if (_isLeaf) {
      brackets = List<Bracket>();
      text = _processBrakets(false);
      _stemmingText = _processBrakets(true);
      if (_stemmingText == brackets) _stemmingText = null;
      // 'empty without brakets' error
      if (_stemmingText.trim().isEmpty && text.isNotEmpty)
        _addError(Consts.eEmptyWithoutBrackets);
    }
    // root:
    if (_isRoot) {
      //check and assign word class
      final toCheck = delim == Consts.wCls ? child : [this];
      for (final toCh in toCheck) {
        String cls;
        for (final ch in toCh.childDeep()) {
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
        else if (cls != null) for (final ch in toCh.childDeep()) ch.wcls = cls;
      }
      // no error => join synonymous and remove non-leaf
      if (errors.length == 0) {
        errors = null;
        brackets = Linq.selectMany(childDeep(), (ParsedFact ch) => ch.brackets).toList();
        for (final ch
            in childDeep(false).where((ch) => ch.delim == Consts.wSyn)) {
          ch.text = ch.child.map((cc) => cc.text).join(', ');
          ch.wcls = ch.child[0].wcls;
          ch._stemmingText = ch.child.map((cc) => cc.toBreak).join(', ');
          ch.child = null;
          if (ch.text == ch._stemmingText) ch._stemmingText = null;
          assert(ch.text.length == ch.toBreak.length);
        }
        child = childDeep().toList();
      }
    }
  }
  static final _firstChildChainRx = RegExp(r'^[0.]*$');
  Iterable<ParsedFact> childDeep([bool leafOnly = true]) sync* {
    if (child == null || !leafOnly) yield this;
    if (child != null) for (final ch in child) yield* ch.childDeep(leafOnly);
  }

  _processBrakets(bool forBreakText) {
    return text.replaceAllMapped(factBracketsRx, (m) {
      final br = Bracket.fromGroup(m, 1) ??
          Bracket.fromGroup(m, 2) ??
          Bracket.fromGroup(m, 3);
      assert(br != null);
      if (!forBreakText) brackets.add(br);
      if (br.type == Consts.brSq) return '';
      return forBreakText ? ''.padRight(m.end - m.start, ' ') : m.group(0);
    });
  }

  _addError(int err) {
    errors.add(Tuple2(id, err));
  }

  bool get _isLeaf => child == null;
  bool get _isRoot => id == '0';

  String wcls;
  String delim;
  String id;
  String text;
  String get toBreak => _stemmingText ?? text;
  String _stemmingText;
  List<Bracket> brackets;
  List<ParsedFact> child;
  List<Tuple2<String, int>> errors;
}

ParsedFact parseRaw(String str) => str.isEmpty ? null : ParsedFact(str);

final factTestRx = RegExp(
    r'^(\([^(){}\[\],|^]+\)|\[\w+\]|{[^(){}\[\],|^]+}|[^(){}\[\],|^]*)+$');
final factBracketsRx =
    RegExp(r'(\([^(){}\[\],|^]+\))|(\[\w+\])|({[^(){}\[\],|^]+})');
