// import 'dart:collection';
// import 'package:test/test.dart' as test;

// import 'package:rw_utils/utils.dart' show fileSystem;
// import 'package:path/path.dart' as p;

// getfileMeta() {
//   String getLang(String lang, String fn) {
//     if (lang.length == 5 && lang.split('_').length == 2) return lang;
//     if (lang=='sr_latn_cs') return lang;
//     final f = lang.split('#');
//     if (f.length == 2) {
//       final l = getLang(f[1], fn);
//       if (l != null) return l;
//     }
//     return fn;
//   }

//   var langs = fileSystem.rawCsv.list(regExp: r'\.csv').expand((fn) {
//     var isAll = fn.indexOf('\\all\\')>=0;
//     if (isAll) return [];
//     var isTempl = fn.startsWith('templates');
//     var isKDict = fn.indexOf('KDictionaries')>=0;
//     final f = p.split(p.withoutExtension(fn));
//     return [isKDict ? '' : getLang(f[f.length - 2], fn), isTempl ? '' : getLang(f.last, fn)];
//   }).where((s) => s.isNotEmpty).toList();
//   fileSystem.rawCsv.writeAsLines('dir.txt', HashSet<String>.from(langs));
//   langs = null;
// }

// main() {
//   test.test('?', () {
//     getfileMeta();
//   });
// }
