import 'dart:collection';
import 'package:rw_utils/utils.dart' show fileSystem, Matrix;
import 'package:rw_utils/langs.dart' show Langs, Unicode;
import '../filer.dart';
import '../consts.dart';
import '../dom.dart';

Future cyrilic({bool doParallel}) async {
  final latins = HashSet<int>();
  final matrix = Matrix(header: ['word','latn', 'file']);
  for (final file in Filer.files
      .where((f) => Langs.nameToMeta[f.dataLang].scriptId == 'Cyrl')
      .map((f) => File.fromFileInfo(f))) {
    for (final word in file.factss.expand((f) => f.facts.expand((ff) => ff.words
        .where((w) =>
            w.text.isNotEmpty &&
            (w.flags & Flags.wInBr == 0) &&
            (w.flags & Flags.wIsPartOf == 0))))) {
      var cyrl = 0, latn = List<int>(), len = 0, wrong = false;
      for (final ch in word.text.codeUnits) {
        len++;
        final uni = Unicode.item(ch);
        if (uni == null || (uni.script != 'Latn' && uni.script != 'Cyrl')) {
          wrong = true;
          break;
        }
        if (uni.script == 'Cyrl')
          cyrl++;
        else
          latn.add(ch);
      }
      if (wrong || cyrl==len || latn.length==len || latn.length > cyrl) continue;
      latins.addAll(latn);
      matrix.add([word.text, String.fromCharCodes(latn), file.fileName]);
    }
  }
  matrix.save(fileSystem.edits.absolute('cyrilic\\words.csv'));
  fileSystem.edits.writeAsString('cyrilic\\chars.txt', String.fromCharCodes(latins));
}

