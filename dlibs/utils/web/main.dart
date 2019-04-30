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

  Future delay() => Future.delayed(Duration(milliseconds: 200));
  bool isTrans(Element el) => el.children.length > 0;

  var height = document.documentElement.clientHeight,
      lastInPage = 0,
      firstNonTranslated = 0;

  await delay();

  while (true) {
    final col = querySelectorAll('p');
    if (lastInPage == col.length) break;

    // first non transated element
    for (firstNonTranslated = lastInPage;
        firstNonTranslated < col.length;
        firstNonTranslated++) {
      if (!isTrans(col[firstNonTranslated])) break;
    }

    // --- fuctions
    Future scroll(int idx) {
      col[idx].scrollIntoView(ScrollAlignment.TOP);
      return delay();
    }
    bool pageIsTrans() {
      for (var j = firstNonTranslated; j < lastInPage; j++)
        if (!isTrans(col[j])) return false;
      return true;
    }

    // put first non transated to top
    await scroll(firstNonTranslated-1);

    if (firstNonTranslated==col.length) break;

    // find last element on page
    for (lastInPage = firstNonTranslated + 1;
        lastInPage < col.length;
        lastInPage++) {
      final ph = col[lastInPage].getBoundingClientRect().bottom;
      if (ph > height) break;
    }

    // while page is not translated => wait
    do {
      await delay();
    } while (!pageIsTrans());

    print(
        '${firstNonTranslated.toString()} / ${lastInPage.toString()} / ${col.length}');

    // last in page to top
    await scroll(lastInPage - 1);
  }
}
