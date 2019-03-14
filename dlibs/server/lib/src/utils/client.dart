import 'package:rewise_low_utils/utils.dart' show grpcRequest, getHost, MakeRequest;
import 'package:rewise_low_utils/messages.dart' show CSharpMainClient;
import 'package:protobuf/protobuf.dart' as proto;

//import 'package:rewise_low_utils/rw/to_raw.dart' as ToRaw;

typedef RequestGetter<TRepply> = Future<TRepply> Function(
    CSharpMainClient client);

Future<T> makeRequest<T extends proto.GeneratedMessage>(
    RequestGetter<T> getter) async {
  return grpcRequest<T>(
      (channel) => getter(CSharpMainClient(channel)), 50052, 'localhost');
}

// Future<ToRaw.Response> ToRawRun(ToRaw.Request request) => 
//   MakeRequest<ToRaw.Response>(
//       (channel) => ToRaw.CSharpServiceClient(channel).run(request),
//       getHost('ToRaw'));
