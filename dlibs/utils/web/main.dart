//import 'package:tuple/tuple.dart';
//import 'package:rw_utils/env.dart' as env;
//import 'package:rw_utils/linq.dart' as linq;
import 'package:cooky/cooky.dart' as cookie;
import 'package:rw_utils/utils.dart' show fileSystem;

import 'dart:html';

const hostName = 'reactxx.github.io';

void main() async {
  if (window.location.hostname == hostName) {
    window.open(
        'https://translate.google.cz/translate?hl=cs&sl=ar&tl=en&u=https%3A%2F%2Freactxx.github.io',
        'trans');
    return;
  }

  delay() => Future.delayed(Duration(milliseconds: 200));
  bool isTrans(Element el) => el.children.length > 0;

  var height = document.documentElement.clientHeight;
  var lastPageIdx = 0;

  await delay();

  while (true) {
    final col = querySelectorAll('p');
    if (lastPageIdx == col.length) break;

    for (var firstWrongIdx = lastPageIdx;
        firstWrongIdx < col.length;
        firstWrongIdx++) {
      if (!isTrans(col[firstWrongIdx])) continue;

      col[firstWrongIdx].scrollIntoView(ScrollAlignment.TOP);
      await delay();

      for (lastPageIdx = firstWrongIdx + 1;
          lastPageIdx < col.length;
          lastPageIdx++) {
        final ph = col[lastPageIdx].getBoundingClientRect().bottom;
        if (ph > height) break;
      }

      bool pageIsTrans() {
        for (var j = firstWrongIdx; j < lastPageIdx; j++)
          if (!isTrans(col[j])) return false;
        return true;
      }

      do {
        await delay();
      } while (!pageIsTrans());

      print(
          '${lastPageIdx.toString()}/${firstWrongIdx.toString()}/${col.length}');

      col[lastPageIdx - 1].scrollIntoView(ScrollAlignment.TOP);
      await delay();
      break;
    }
  }
}
