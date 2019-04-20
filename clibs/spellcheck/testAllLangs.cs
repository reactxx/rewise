using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using W = Microsoft.Office.Interop.Word;

public static class TestAllLangs {

  public static void Run() {
    var w = new W.Application();
    w.Visible = true;
    var doc = w.Documents.Add();
    foreach (var lcid in Langs.meta.Where(m => m.WordSpellCheckLCID != 0).Select(m => m.WordSpellCheckLCID)) {
      var par = doc.Paragraphs.Add();
      par.Range.Text = "1234";
      doc.Content.LanguageID = (W.WdLanguageID)lcid;
      doc.SpellingChecked = false;
    }

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