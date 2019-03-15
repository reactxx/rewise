import 'package:rewise_low_utils/rw/to_parsed.dart' as ToParsed;
import 'package:rewise_low_utils/rw/word_breaking.dart' as wbreak;

typedef WordBreakProc = Future<wbreak.Response> Function(
    wbreak.Request request);

Future<List<List<int>>> wordBreak(
    ToParsed.ParsedBook book, String lang, WordBreakProc doBreak) async {
  final res = List<List<int>>();
  res.add([1, 2, 3, 4]);
  return Future.value(res);
}
