using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using W = Microsoft.Office.Interop.Word;

public static class WordSpellCheck {

  public static Rw.Spellcheck.Response Spellcheck(Rw.Spellcheck.Request req) {
    var w = new W.Application();
    //w.Visible = true;
    var doc = w.Documents.Open(req.SourceFile, false, true);
    var res = new Rw.Spellcheck.Response();
    try {
      var lid = (W.WdLanguageID)Langs.nameToMeta[req.Lang].WordSpellCheckLCID;
      doc.Content.LanguageID = lid;
      doc.SpellingChecked = false;
      var parCount = 0;
      foreach (W.Paragraph par in doc.Paragraphs) {
        if (par.Range.SpellingErrors.Count > 0)
          res.WrongIdxs.Add(parCount);
        parCount++;
      }
    } finally {
      object dontSave = W.WdSaveOptions.wdDoNotSaveChanges;
      doc.Close(ref dontSave);
      w.Quit();
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