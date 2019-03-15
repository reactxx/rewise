import 'package:rewise_low_utils/rw/to_parsed.dart' as ToParsed;
import 'package:rewise_low_utils/rw/dom.dart' as dom;

Iterable<ToParsed.ParsedFact> parseFactTextFormat (String str, int idx, int lessonId) sync* {
  yield ToParsed.ParsedFact()..fact = dom.Fact();
}