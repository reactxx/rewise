using System;
using System.Collections.Generic;

namespace StemmerBreaker {

  public static class SplitLines {

    // split large text to chunks of "lineCount" lines
    public static IEnumerable<string> Run(string text, int lineCount) {
      int begPos = 0, pos = 0, lfCount = lineCount;
      while (pos < text.Length) {
        if (text[pos] == '\n') {
          lfCount--;
          if (lfCount == 0) {
            yield return text.Substring(begPos, pos - begPos);
            lfCount = lineCount;
            begPos = pos + 1;
          }
        }
        pos++;
      }
      yield return text.Substring(begPos, pos - begPos);

    }

  }
}
