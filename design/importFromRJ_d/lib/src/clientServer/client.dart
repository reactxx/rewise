import 'package:rewise_low_utils/utils.dart' show grpcRequest;
import 'package:rewise_low_utils/messages.dart' show CSharpMainClient;
import 'package:protobuf/protobuf.dart' as proto;

typedef RequestGetter<TRepply> = Future<TRepply> Function(CSharpMainClient client);

Future<T> makeRequest<T extends proto.GeneratedMessage>(
    RequestGetter<T> getter) async {
  return grpcRequest<T>(
      (channel) => getter(CSharpMainClient(channel)), 50052, 'localhost');
}
