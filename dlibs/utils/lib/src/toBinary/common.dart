import 'dart:typed_data';
import 'package:rw_utils/toBinary.dart';

abstract class IWriters {
  String dump();
  Uint8List toBytes();
  ByteWriter get writer;
}

abstract class IReaders {
  ByteReader get reader;
}

abstract class IWritable {
  Uint8List writeToBuffer();
  IWritable.fromBuffer(Uint8List buff);
}


