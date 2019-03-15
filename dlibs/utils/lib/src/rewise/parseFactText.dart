class Parsed {
  String text;
  String breakText;
}

Iterable<Parsed> parseFactTextFormat(
    String str) sync* {
  yield Parsed()
    ..breakText = str;
}
