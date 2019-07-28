///
//  Generated code. Do not modify.
//  source: google/cloud/texttospeech/v1/cloud_tts.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore_for_file: UNDEFINED_SHOWN_NAME,UNUSED_SHOWN_NAME
import 'dart:core' show int, dynamic, String, List, Map;
import 'package:protobuf/protobuf.dart' as $pb;

class SsmlVoiceGender extends $pb.ProtobufEnum {
  static const SsmlVoiceGender SSML_VOICE_GENDER_UNSPECIFIED = const SsmlVoiceGender._(0, 'SSML_VOICE_GENDER_UNSPECIFIED');
  static const SsmlVoiceGender MALE = const SsmlVoiceGender._(1, 'MALE');
  static const SsmlVoiceGender FEMALE = const SsmlVoiceGender._(2, 'FEMALE');
  static const SsmlVoiceGender NEUTRAL = const SsmlVoiceGender._(3, 'NEUTRAL');

  static const List<SsmlVoiceGender> values = const <SsmlVoiceGender> [
    SSML_VOICE_GENDER_UNSPECIFIED,
    MALE,
    FEMALE,
    NEUTRAL,
  ];

  static final Map<int, SsmlVoiceGender> _byValue = $pb.ProtobufEnum.initByValue(values);
  static SsmlVoiceGender valueOf(int value) => _byValue[value];

  const SsmlVoiceGender._(int v, String n) : super(v, n);
}

class AudioEncoding extends $pb.ProtobufEnum {
  static const AudioEncoding AUDIO_ENCODING_UNSPECIFIED = const AudioEncoding._(0, 'AUDIO_ENCODING_UNSPECIFIED');
  static const AudioEncoding LINEAR16 = const AudioEncoding._(1, 'LINEAR16');
  static const AudioEncoding MP3 = const AudioEncoding._(2, 'MP3');
  static const AudioEncoding OGG_OPUS = const AudioEncoding._(3, 'OGG_OPUS');

  static const List<AudioEncoding> values = const <AudioEncoding> [
    AUDIO_ENCODING_UNSPECIFIED,
    LINEAR16,
    MP3,
    OGG_OPUS,
  ];

  static final Map<int, AudioEncoding> _byValue = $pb.ProtobufEnum.initByValue(values);
  static AudioEncoding valueOf(int value) => _byValue[value];

  const AudioEncoding._(int v, String n) : super(v, n);
}

