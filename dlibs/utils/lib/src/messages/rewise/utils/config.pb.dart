///
//  Generated code. Do not modify.
//  source: rewise/utils/config.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore: UNUSED_SHOWN_NAME
import 'dart:core' show int, bool, double, String, List, Map, override;

import 'package:protobuf/protobuf.dart' as $pb;

class Config extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('Config', package: const $pb.PackageName('rewiseDom'))
    ..m<String, WorkSpace>(1, 'workSpaces', 'Config.WorkSpacesEntry',$pb.PbFieldType.OS, $pb.PbFieldType.OM, WorkSpace.create, null, null , const $pb.PackageName('rewiseDom'))
    ..hasRequiredFields = false
  ;

  Config() : super();
  Config.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  Config.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  Config clone() => new Config()..mergeFromMessage(this);
  Config copyWith(void Function(Config) updates) => super.copyWith((message) => updates(message as Config));
  $pb.BuilderInfo get info_ => _i;
  static Config create() => new Config();
  Config createEmptyInstance() => create();
  static $pb.PbList<Config> createRepeated() => new $pb.PbList<Config>();
  static Config getDefault() => _defaultInstance ??= create()..freeze();
  static Config _defaultInstance;

  Map<String, WorkSpace> get workSpaces => $_getMap(0);
}

class WorkSpace extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('WorkSpace', package: const $pb.PackageName('rewiseDom'))
    ..a<Connection>(1, 'dartServer', $pb.PbFieldType.OM, Connection.getDefault, Connection.create)
    ..a<Connection>(2, 'csharpServer', $pb.PbFieldType.OM, Connection.getDefault, Connection.create)
    ..hasRequiredFields = false
  ;

  WorkSpace() : super();
  WorkSpace.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  WorkSpace.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  WorkSpace clone() => new WorkSpace()..mergeFromMessage(this);
  WorkSpace copyWith(void Function(WorkSpace) updates) => super.copyWith((message) => updates(message as WorkSpace));
  $pb.BuilderInfo get info_ => _i;
  static WorkSpace create() => new WorkSpace();
  WorkSpace createEmptyInstance() => create();
  static $pb.PbList<WorkSpace> createRepeated() => new $pb.PbList<WorkSpace>();
  static WorkSpace getDefault() => _defaultInstance ??= create()..freeze();
  static WorkSpace _defaultInstance;

  Connection get dartServer => $_getN(0);
  set dartServer(Connection v) { setField(1, v); }
  bool hasDartServer() => $_has(0);
  void clearDartServer() => clearField(1);

  Connection get csharpServer => $_getN(1);
  set csharpServer(Connection v) { setField(2, v); }
  bool hasCsharpServer() => $_has(1);
  void clearCsharpServer() => clearField(2);
}

class Connection extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('Connection', package: const $pb.PackageName('rewiseDom'))
    ..aOS(1, 'host')
    ..a<int>(2, 'port', $pb.PbFieldType.O3)
    ..hasRequiredFields = false
  ;

  Connection() : super();
  Connection.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  Connection.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  Connection clone() => new Connection()..mergeFromMessage(this);
  Connection copyWith(void Function(Connection) updates) => super.copyWith((message) => updates(message as Connection));
  $pb.BuilderInfo get info_ => _i;
  static Connection create() => new Connection();
  Connection createEmptyInstance() => create();
  static $pb.PbList<Connection> createRepeated() => new $pb.PbList<Connection>();
  static Connection getDefault() => _defaultInstance ??= create()..freeze();
  static Connection _defaultInstance;

  String get host => $_getS(0, '');
  set host(String v) { $_setString(0, v); }
  bool hasHost() => $_has(0);
  void clearHost() => clearField(1);

  int get port => $_get(1, 0);
  set port(int v) { $_setSignedInt32(1, v); }
  bool hasPort() => $_has(1);
  void clearPort() => clearField(2);
}

