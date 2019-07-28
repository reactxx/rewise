///
//  Generated code. Do not modify.
//  source: google/api/resource.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore: UNUSED_SHOWN_NAME
import 'dart:core' show int, bool, double, String, List, Map, override;

import 'package:protobuf/protobuf.dart' as $pb;

import 'resource.pbenum.dart';

export 'resource.pbenum.dart';

class ResourceDescriptor extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('ResourceDescriptor', package: const $pb.PackageName('google.api'))
    ..aOS(1, 'type')
    ..pPS(2, 'pattern')
    ..aOS(3, 'nameField')
    ..e<ResourceDescriptor_History>(4, 'history', $pb.PbFieldType.OE, ResourceDescriptor_History.HISTORY_UNSPECIFIED, ResourceDescriptor_History.valueOf, ResourceDescriptor_History.values)
    ..hasRequiredFields = false
  ;

  ResourceDescriptor() : super();
  ResourceDescriptor.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  ResourceDescriptor.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  ResourceDescriptor clone() => new ResourceDescriptor()..mergeFromMessage(this);
  ResourceDescriptor copyWith(void Function(ResourceDescriptor) updates) => super.copyWith((message) => updates(message as ResourceDescriptor));
  $pb.BuilderInfo get info_ => _i;
  static ResourceDescriptor create() => new ResourceDescriptor();
  ResourceDescriptor createEmptyInstance() => create();
  static $pb.PbList<ResourceDescriptor> createRepeated() => new $pb.PbList<ResourceDescriptor>();
  static ResourceDescriptor getDefault() => _defaultInstance ??= create()..freeze();
  static ResourceDescriptor _defaultInstance;

  String get type => $_getS(0, '');
  set type(String v) { $_setString(0, v); }
  bool hasType() => $_has(0);
  void clearType() => clearField(1);

  List<String> get pattern => $_getList(1);

  String get nameField => $_getS(2, '');
  set nameField(String v) { $_setString(2, v); }
  bool hasNameField() => $_has(2);
  void clearNameField() => clearField(3);

  ResourceDescriptor_History get history => $_getN(3);
  set history(ResourceDescriptor_History v) { setField(4, v); }
  bool hasHistory() => $_has(3);
  void clearHistory() => clearField(4);
}

class ResourceReference extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('ResourceReference', package: const $pb.PackageName('google.api'))
    ..aOS(1, 'type')
    ..aOS(2, 'childType')
    ..hasRequiredFields = false
  ;

  ResourceReference() : super();
  ResourceReference.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  ResourceReference.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  ResourceReference clone() => new ResourceReference()..mergeFromMessage(this);
  ResourceReference copyWith(void Function(ResourceReference) updates) => super.copyWith((message) => updates(message as ResourceReference));
  $pb.BuilderInfo get info_ => _i;
  static ResourceReference create() => new ResourceReference();
  ResourceReference createEmptyInstance() => create();
  static $pb.PbList<ResourceReference> createRepeated() => new $pb.PbList<ResourceReference>();
  static ResourceReference getDefault() => _defaultInstance ??= create()..freeze();
  static ResourceReference _defaultInstance;

  String get type => $_getS(0, '');
  set type(String v) { $_setString(0, v); }
  bool hasType() => $_has(0);
  void clearType() => clearField(1);

  String get childType => $_getS(1, '');
  set childType(String v) { $_setString(1, v); }
  bool hasChildType() => $_has(1);
  void clearChildType() => clearField(2);
}

class Resource {
  static final $pb.Extension resourceReference = new $pb.Extension<ResourceReference>('google.protobuf.FieldOptions', 'resourceReference', 1055, $pb.PbFieldType.OM, ResourceReference.getDefault, ResourceReference.create);
  static final $pb.Extension resource = new $pb.Extension<ResourceDescriptor>('google.protobuf.MessageOptions', 'resource', 1053, $pb.PbFieldType.OM, ResourceDescriptor.getDefault, ResourceDescriptor.create);
  static void registerAllExtensions($pb.ExtensionRegistry registry) {
    registry.add(resourceReference);
    registry.add(resource);
  }
}

