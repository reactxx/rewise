///
//  Generated code. Do not modify.
//  source: rewise/dom/dom.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore_for_file: UNDEFINED_SHOWN_NAME,UNUSED_SHOWN_NAME
import 'dart:core' show int, dynamic, String, List, Map;
import 'package:protobuf/protobuf.dart' as $pb;

class FileMsg_BookType extends $pb.ProtobufEnum {
  static const FileMsg_BookType KDICT = const FileMsg_BookType._(0, 'KDICT');
  static const FileMsg_BookType DICT = const FileMsg_BookType._(1, 'DICT');
  static const FileMsg_BookType ETALK = const FileMsg_BookType._(2, 'ETALK');
  static const FileMsg_BookType BOOK = const FileMsg_BookType._(3, 'BOOK');

  static const List<FileMsg_BookType> values = const <FileMsg_BookType> [
    KDICT,
    DICT,
    ETALK,
    BOOK,
  ];

  static final Map<int, FileMsg_BookType> _byValue = $pb.ProtobufEnum.initByValue(values);
  static FileMsg_BookType valueOf(int value) => _byValue[value];

  const FileMsg_BookType._(int v, String n) : super(v, n);
}

class FileMsg_FileType extends $pb.ProtobufEnum {
  static const FileMsg_FileType LEFT = const FileMsg_FileType._(0, 'LEFT');
  static const FileMsg_FileType LANGLEFT = const FileMsg_FileType._(1, 'LANGLEFT');
  static const FileMsg_FileType LANG = const FileMsg_FileType._(2, 'LANG');

  static const List<FileMsg_FileType> values = const <FileMsg_FileType> [
    LEFT,
    LANGLEFT,
    LANG,
  ];

  static final Map<int, FileMsg_FileType> _byValue = $pb.ProtobufEnum.initByValue(values);
  static FileMsg_FileType valueOf(int value) => _byValue[value];

  const FileMsg_FileType._(int v, String n) : super(v, n);
}

