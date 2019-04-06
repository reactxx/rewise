import 'package:rw_utils/dom/word_breaking.dart' as wbreak;

import 'dom.dart';
import 'lexAnal.dart';
import 'parser.dart';

Facts import(Book book, Breaked breakedLeft, {Breaked breakedRight}) {
  final wordsLeft = _lexAnal(breakedLeft);
}

List<Token> _lexAnal(Breaked breaked) {}

// List<LexFact> _splitToFacts(List<Token> tokens) {
//   var beg = tokens.first;

//   final facts = List<LexFact>();

//   // wordClass management
//   LexFact firstInWordClassGroup = null;
//   var factHasWordClass = false;

//   for (var t in tokens) {
//     if (t.type == '^' || t.type == ',' || t.type == '|') {
//       if (beg == t) /*ERROR*/ continue;
//       final fact = LexFact(beg, t);
//       facts.add(fact);
//       if (t != tokens.last) beg = tokens[t.idx + 1];
//       // wordClass management
//       if (firstInWordClassGroup == null) {
//         firstInWordClassGroup = fact;
//         fact.canHaveWordClass = factHasWordClass ? true : null;
//       }
//       if (t.type == '|') {
//         // facts has wordClass
//         firstInWordClassGroup.canHaveWordClass = true;
//         firstInWordClassGroup == null;
//         factHasWordClass = true;
//       }
//     }
//   }
//   return facts;
// }

