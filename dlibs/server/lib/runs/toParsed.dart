import 'package:server_dart/commands.dart';

main() async {
  await toParsed();
  //test();
}

final _factTestRx = RegExp(
    r'^(\([^(){}\[\],|^]+\)|\[\w+\]|{[^(){}\[\],|^]+}|[^(){}\[\],|^]*)+$');

final t = 'రెండు [గంటలు] అయ్యింది';

test() {
  for (var i = 0; i < 1000000; i++) {
    if (!_factTestRx.hasMatch('aas${i}das (asdas) {asd${i}asd} [sdasdas]')) break;
    if (i & 0x4ff == 0) print(i);
  }
}
