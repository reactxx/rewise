using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using W = Microsoft.Office.Interop.Word;

public static class WordSpellCheck {

  public static List<int> Spellcheck(string lang, string html) {

    var lid = (W.WdLanguageID)Langs.nameToMeta[lang].WordSpellCheckLCID;
    if (lid == W.WdLanguageID.wdLanguageNone)
      throw new Exception("Wrong lang: " + lang);

    var fn = Path.GetTempFileName();
    var res = new List<int>();

    try {
      File.WriteAllText(fn, html);

      var w = new W.Application();
      //w.Visible = true;
      try {
        W.Document doc = w.Documents.Open(fn, false, true);
        try {
          doc.Content.LanguageID = lid;
          doc.SpellingChecked = false;
          var parCount = 0;
          foreach (W.Paragraph par in doc.Paragraphs) {
            foreach (var err in par.Range.SpellingErrors) {
              res.Add(parCount);
              break;
            }
            parCount++;
          }
        } finally {
          object dontSave = W.WdSaveOptions.wdDoNotSaveChanges;
          doc.Close(ref dontSave);
        }
      } finally {
        w.Quit();
      }
    } finally {
      File.Delete(fn);
    }

    return res;
  }

  public static void Test() {
    var w = new W.Application();
    w.Visible = true;
    var doc = w.Documents.Add();
    foreach (var meta in Langs.meta.Where(m => m.WordSpellCheckLCID != 0)) {
      var par = doc.Paragraphs.Add();
      par.Range.Text = meta.Id;
      par.Range.InsertParagraphAfter();
      par.Range.LanguageID = (W.WdLanguageID)meta.WordSpellCheckLCID;
    }

    doc.SpellingChecked = false;
    List<string> mispelled = new List<string>();
    foreach (W.Range word in doc.Words)
      foreach (W.Range err in word.SpellingErrors) {
        mispelled.Add(err.Text);
      }

    Console.ReadKey();

    object dontSave = W.WdSaveOptions.wdDoNotSaveChanges;
    doc.Close(ref dontSave);
    w.Quit();

  }
}