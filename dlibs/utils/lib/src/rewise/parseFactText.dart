import 'package:rewise_low_utils/utils.dart' show Linq;
import 'package:tuple/tuple.dart';

class Parsed {
  String text;
  String breakText;
}

Iterable<Parsed> parseFactTextFormat(String str) sync* {
  final match =
      factTestRx.firstMatch(r'a adsfasd(asfd) [t] sdadf {sdf asdf} sdfasdf');
  var mm = List<String>.from(
      Linq.range(1, match.groupCount).map((i) => match.group(i)));

  yield Parsed()..breakText = str;
}

class Bracket {
  int pos;
  int end;
  String type;
  String value;
  static Bracket fromGroup(Match m, int idx) {
    final val = m.group(idx);
    if (val == null) return null;
    return Bracket()
      ..type = _types[idx]
      ..pos = m.start
      ..end = m.end
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
  static const eWClsMissing = 2; // missing wCls for | delimited chids
  static const eWClsOther = 3; // wCls is not in first child's item 
  static const eWClsMore = 5; // more than single wCls in leaf item
  static const eEmpty = 4; // empty without brakets
}

class Item {
  Item(this.text,
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
              Item(s, errors, delims.skip(1).toList(), '$id.${childIdx++}'))
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
      brackets = factBracketsRx
          .allMatches(text)
          .map((m) =>
              Bracket.fromGroup(m, 1) ??
              Bracket.fromGroup(m, 2) ??
              Bracket.fromGroup(m, 3))
          .toList();
      if (brackets.isNotEmpty) {
        // get text for stemming
        _stemmingText = text.replaceAllMapped(
            factBracketsRx, (m) => ''.padRight(m.end - m.start, ' '));
        assert(text.length == _stemmingText.length);
        // 'empty without brakets' error
        if (_stemmingText.trim().isEmpty) _addError(Consts.eEmpty);
      }
    }
    // root: check and assign word class
    if (_isRoot) {
      final toCheck = delim == Consts.wCls ? child : [this];
      for (final toCh in toCheck) {
        for (final ch in toCh.childDeep()) {
          final sqs = ch.brackets.where((b) => b.type == Consts.brSq).toList();
          final isFirstChain =
              _firstChildChainRx.hasMatch(ch.id.substring(toCh.id.length));
          if (sqs.isEmpty) {
            if (isFirstChain && delim == Consts.wCls)
              _addError(Consts.eWClsMissing);
            continue;
          }
          if (sqs.length > 1)
            _addError(Consts.eWClsMore);
          else if (!isFirstChain)
            _addError(Consts.eWClsOther);
          else
            ch.wcls = sqs[0].value;
        }
      }

      //final withCls = childDeep().where((ch) => ch.wcls!=null).toList();
    }
  }
  static final _firstChildChainRx = RegExp(r'^[0.]*$');
  Iterable<Item> childDeep([bool leafOnly = true]) sync* {
    if (child == null || !leafOnly) yield this;
    if (child != null) for (final ch in child) yield* ch.childDeep();
  }

  _addError(int err) {
    errors.add(Tuple2(id, err));
  }

  bool get _isLeaf => delim == null;
  bool get _isRoot => id == '0';

  // String _wordClassDeep() {
  //   bool isFirst = true;
  //   String res = _wordClass(false);
  //   childDeep().map((n) {
  //     if (isFirst) {
  //       final sqs = n.brackets.where((b) => b.type == Consts.brSq).toList();
  //       if (sqs.length == 1)
  //         res = sqs[0].value;
  //       else
  //         _addError(Consts.eWClsFirst);
  //       isFirst = false;
  //       return;
  //     }
  //     if (n.brackets.where((b) => b.type == Consts.brSq).isNotEmpty)
  //       _addError(Consts.eWClsOther);
  //   });
  //   return res;
  // }

  String wcls;
  String delim;
  String id;
  String text;
  String get stemmingText => _stemmingText ?? text;
  String _stemmingText;
  List<Bracket> brackets;
  List<Item> child;
  List<Tuple2<String, int>> errors;
}

Item parseRaw(String str) => str.isEmpty ? null : Item(str);

final factTestRx = RegExp(
    r'^(\([^(){}\[\],|^]+\)|\[\w+\]|{[^(){}\[\],|^]+}|[^(){}\[\],|^]*)+$');
final factBracketsRx =
    RegExp(r'(\([^(){}\[\],|^]+\))|(\[\w+\])|({[^(){}\[\],|^]+})');
