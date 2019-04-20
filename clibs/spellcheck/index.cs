using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using W = Microsoft.Office.Interop.Word;



public static class WordSpellCheck {

  public static Rw.Spellcheck.Response Spellcheck(Rw.Spellcheck.Request request) {
    Rw.Spellcheck.Response res = null;
    var w = new W.Application();
    w.Visible = true;
    var doc = w.Documents.Add();
    try {

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