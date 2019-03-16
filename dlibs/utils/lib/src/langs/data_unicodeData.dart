import 'dart:convert';
import 'package:rw_utils/dom/utils.dart';

UncBlocks getUnicodeData() {
  if (_unicodeData==null) {
    const res = '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';
    _unicodeData = UncBlocks.fromBuffer(base64.decode(res));
  }
  return _unicodeData;
}
UncBlocks _unicodeData;
