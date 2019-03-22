import 'dart:typed_data';
import 'package:rw_utils/toBinary.dart';

abstract class IWriters {
  String dump();
  Uint8List toBytes();
  MemoryWriter get writer;
}

abstract class IReaders {
  Reader get reader;
}

abstract class IWritable {
  Uint8List writeToBuffer();
  IWritable.fromBuffer(Uint8List buff);
}


