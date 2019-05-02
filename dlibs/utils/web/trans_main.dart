import 'dart:convert' as conv;
import 'dart:html';
import 'package:http/http.dart' as http;

String _uri(String path, [Map<String, String> queryParameters]) =>
    Uri.http('localhost:8080', '$path', queryParameters).toString();

String _fileUri(String path, [Map<String, String> queryParameters]) =>
    _uri('build/trans_tasks/$path', queryParameters);

String _workerUri(String path) => _uri('worker/$path');

void main() async {
  final url = Uri.parse(window.location.href);
  var file = url.queryParameters['file'];

  if (file == null) {
    var json = await http.read(_workerUri('init'));
    List<String> tasks = conv
        .jsonDecode(json); // list of build/trans_tasks relative HTML file urls
    var count = 0;
    for (var task in tasks) {
      querySelector('#title').text =
          '$task (${count.toString()}/${tasks.length.toString()})';
      await http.read(_workerUri('startfile'));
      var win = window.open(
          _fileUri('build/trans_tasks/$task.html', {'file': task}), task);
      while (true) {
        await Future.delayed(Duration(seconds: 1));
        var isWorking = await http.read(_workerUri('isworking'));
        if (isWorking == 'no') break;
      }
      win.close();
      await Future.delayed(Duration(microseconds: 100));
    }
  }

  querySelector('#title').text = file;

  try {
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
      await scroll(firstNonTranslated < 0
          ? 0
          : (firstNonTranslated == col.length
              ? col.length - 1
              : firstNonTranslated));

      if (firstNonTranslated == col.length) break;

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
  } finally {
    await Future.delayed(Duration(seconds: 1));
    await http.read(_workerUri('endfile'));
  }
}
