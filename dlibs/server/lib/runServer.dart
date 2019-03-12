import 'package:server_dart/utils.dart' show ServerEntryPoint;

main() async {
  await ServerEntryPoint.runServer('localhost', 50053);
}
