import 'package:server_dart/index.dart' show ServerEntryPoint;

main() async {
  await ServerEntryPoint.runServer('localhost', 50053);
}
