///
//  Generated code. Do not modify.
//  source: google/api/client.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore: UNUSED_SHOWN_NAME
import 'dart:core' show int, bool, double, String, List, Map, override;

import 'package:protobuf/protobuf.dart' as $pb;

class Client {
  static final $pb.Extension methodSignature = new $pb.Extension<String>.repeated('google.protobuf.MethodOptions', 'methodSignature', 1051, $pb.PbFieldType.PS, $pb.getCheckFunction($pb.PbFieldType.PS));
  static final $pb.Extension defaultHost = new $pb.Extension<String>('google.protobuf.ServiceOptions', 'defaultHost', 1049, $pb.PbFieldType.OS);
  static final $pb.Extension oauthScopes = new $pb.Extension<String>('google.protobuf.ServiceOptions', 'oauthScopes', 1050, $pb.PbFieldType.OS);
  static void registerAllExtensions($pb.ExtensionRegistry registry) {
    registry.add(methodSignature);
    registry.add(defaultHost);
    registry.add(oauthScopes);
  }
}

