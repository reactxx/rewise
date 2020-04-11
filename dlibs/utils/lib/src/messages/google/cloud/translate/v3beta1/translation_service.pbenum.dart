///
//  Generated code. Do not modify.
//  source: google/cloud/translate/v3beta1/translation_service.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore_for_file: UNDEFINED_SHOWN_NAME,UNUSED_SHOWN_NAME
import 'dart:core' show int, dynamic, String, List, Map;
import 'package:protobuf/protobuf.dart' as $pb;

class BatchTranslateMetadata_State extends $pb.ProtobufEnum {
  static const BatchTranslateMetadata_State STATE_UNSPECIFIED = const BatchTranslateMetadata_State._(0, 'STATE_UNSPECIFIED');
  static const BatchTranslateMetadata_State RUNNING = const BatchTranslateMetadata_State._(1, 'RUNNING');
  static const BatchTranslateMetadata_State SUCCEEDED = const BatchTranslateMetadata_State._(2, 'SUCCEEDED');
  static const BatchTranslateMetadata_State FAILED = const BatchTranslateMetadata_State._(3, 'FAILED');
  static const BatchTranslateMetadata_State CANCELLING = const BatchTranslateMetadata_State._(4, 'CANCELLING');
  static const BatchTranslateMetadata_State CANCELLED = const BatchTranslateMetadata_State._(5, 'CANCELLED');

  static const List<BatchTranslateMetadata_State> values = const <BatchTranslateMetadata_State> [
    STATE_UNSPECIFIED,
    RUNNING,
    SUCCEEDED,
    FAILED,
    CANCELLING,
    CANCELLED,
  ];

  static final Map<int, BatchTranslateMetadata_State> _byValue = $pb.ProtobufEnum.initByValue(values);
  static BatchTranslateMetadata_State valueOf(int value) => _byValue[value];

  const BatchTranslateMetadata_State._(int v, String n) : super(v, n);
}

class CreateGlossaryMetadata_State extends $pb.ProtobufEnum {
  static const CreateGlossaryMetadata_State STATE_UNSPECIFIED = const CreateGlossaryMetadata_State._(0, 'STATE_UNSPECIFIED');
  static const CreateGlossaryMetadata_State RUNNING = const CreateGlossaryMetadata_State._(1, 'RUNNING');
  static const CreateGlossaryMetadata_State SUCCEEDED = const CreateGlossaryMetadata_State._(2, 'SUCCEEDED');
  static const CreateGlossaryMetadata_State FAILED = const CreateGlossaryMetadata_State._(3, 'FAILED');
  static const CreateGlossaryMetadata_State CANCELLING = const CreateGlossaryMetadata_State._(4, 'CANCELLING');
  static const CreateGlossaryMetadata_State CANCELLED = const CreateGlossaryMetadata_State._(5, 'CANCELLED');

  static const List<CreateGlossaryMetadata_State> values = const <CreateGlossaryMetadata_State> [
    STATE_UNSPECIFIED,
    RUNNING,
    SUCCEEDED,
    FAILED,
    CANCELLING,
    CANCELLED,
  ];

  static final Map<int, CreateGlossaryMetadata_State> _byValue = $pb.ProtobufEnum.initByValue(values);
  static CreateGlossaryMetadata_State valueOf(int value) => _byValue[value];

  const CreateGlossaryMetadata_State._(int v, String n) : super(v, n);
}

class DeleteGlossaryMetadata_State extends $pb.ProtobufEnum {
  static const DeleteGlossaryMetadata_State STATE_UNSPECIFIED = const DeleteGlossaryMetadata_State._(0, 'STATE_UNSPECIFIED');
  static const DeleteGlossaryMetadata_State RUNNING = const DeleteGlossaryMetadata_State._(1, 'RUNNING');
  static const DeleteGlossaryMetadata_State SUCCEEDED = const DeleteGlossaryMetadata_State._(2, 'SUCCEEDED');
  static const DeleteGlossaryMetadata_State FAILED = const DeleteGlossaryMetadata_State._(3, 'FAILED');
  static const DeleteGlossaryMetadata_State CANCELLING = const DeleteGlossaryMetadata_State._(4, 'CANCELLING');
  static const DeleteGlossaryMetadata_State CANCELLED = const DeleteGlossaryMetadata_State._(5, 'CANCELLED');

  static const List<DeleteGlossaryMetadata_State> values = const <DeleteGlossaryMetadata_State> [
    STATE_UNSPECIFIED,
    RUNNING,
    SUCCEEDED,
    FAILED,
    CANCELLING,
    CANCELLED,
  ];

  static final Map<int, DeleteGlossaryMetadata_State> _byValue = $pb.ProtobufEnum.initByValue(values);
  static DeleteGlossaryMetadata_State valueOf(int value) => _byValue[value];

  const DeleteGlossaryMetadata_State._(int v, String n) : super(v, n);
}

