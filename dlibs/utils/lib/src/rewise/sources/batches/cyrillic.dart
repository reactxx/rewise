import 'dart:collection';
import 'package:rw_utils/utils.dart' show fileSystem, Matrix;
import 'package:rw_utils/langs.dart' show Langs, Unicode;
import '../filer.dart';
import '../consts.dart';
import '../dom.dart';

Future cyrillic({bool doParallel}) async {
  final latins = HashSet<int>();
  final matrix = Matrix(header: ['word','latn', 'file']);
  final uniq = HashSet<String>();
  final cyrlAll = HashSet<int>();
  final allChars = HashSet<int>();
  for (final file in Filer.files
      .where((f) => Langs.nameToMeta[f.dataLang].scriptId == 'Cyrl')
      .map((f) => File.fromFileInfo(f))) {
    for (final word in file.factss.expand((f) => f.facts.expand((ff) => ff.words
        .where((w) =>
            w.text.isNotEmpty &&
            (w.flags & WordFlags.wInBr == 0) &&
            (w.flags & WordFlags.wIsPartOf == 0))))) {
      var cyrl = 0, latn = List<int>(), len = 0, wrong = false;
      for (final ch in word.text.codeUnits) {
        allChars.add(ch);
        len++;
        final uni = Unicode.item(ch);
        if (uni == null || (uni.script != 'Latn' && uni.script != 'Cyrl')) {
          wrong = true;
          break;
        }
        if (uni.script == 'Cyrl') {
          cyrlAll.add(ch);
          cyrl++;
        } else
          latn.add(ch);
      }
      if (wrong || cyrl==len || latn.length==len || latn.length > cyrl) continue;
      latins.addAll(latn);
      matrix.add([word.text, String.fromCharCodes(latn), file.fileName]);
      uniq.add('${latn.length.toString()}-${String.fromCharCodes(latn)}-${word.text}');
    }
  }
  (matrix..sort(0)).save(fileSystem.edits.absolute('cyrillic\\words1.csv'));
  (matrix..sort(1)).save(fileSystem.edits.absolute('cyrillic\\words2.csv'));
  fileSystem.edits.writeAsString('cyrillic\\chars.txt', String.fromCharCodes(List.from(latins)..sort()));
  fileSystem.edits.writeAsLines('cyrillic\\uniq.txt', List.from(uniq)..sort());
  fileSystem.edits.writeAsString('cyrillic\\cyrlAll.txt', String.fromCharCodes(List.from(cyrlAll)..sort()));
  fileSystem.edits.writeAsString('cyrillic\\allChars.txt', String.fromCharCodes(List.from(allChars)..sort()));
}

const latn = 'ABCHMOPTXàáaceijëopòósxyý';
const cyrl = 'АВСНМОРТХaaасеіјёорooѕхуy';


/*
аеклмнопрстуфхцчшщъыьэюяё

АВЕКМНОРСТУЁ
 */