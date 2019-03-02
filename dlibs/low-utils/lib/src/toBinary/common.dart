import 'dart:typed_data';
import 'package:rewise_low_utils/toBinary.dart';

abstract class IWriters {
  String dump();
  Uint8List toBytes();
  ByteWriter get writer;
}

abstract class IReaders {
  ByteReader get reader;
}

