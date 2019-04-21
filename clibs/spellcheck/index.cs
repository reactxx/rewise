﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using W = Microsoft.Office.Interop.Word;

public static class WordSpellCheck {

  public static List<int> Spellcheck(string lang, IEnumerable<string> words) {
    //var fn = Path.GetTempFileName();
    var fn = string.Format("{0}{1}.html", AppDomain.CurrentDomain.BaseDirectory[0] + @":\temp\", lang);
    try {
      using (var wr = new StreamWriter(fn, true))
        wordsToHTML(wr, words);
      var w = new W.Application();
      w.Visible = true;
      try {
        W.Document doc = w.Documents.Open(fn, false, true);
        try {
          var res = new List<int>();
          var lid = (W.WdLanguageID)Langs.nameToMeta[lang].WordSpellCheckLCID;
          doc.Content.LanguageID = lid;
          doc.SpellingChecked = false;
          var parCount = 0;
          foreach (W.Paragraph par in doc.Paragraphs) {
            if (par.Range.SpellingErrors.Count > 0)
              res.Add(parCount);
            parCount++;
          }
          return res;
        } finally {
          object dontSave = W.WdSaveOptions.wdDoNotSaveChanges;
          doc.Close(ref dontSave);
        }
      } finally {
        w.Quit();
      }
    } finally {
      //try { File.Delete(fn); } catch { }
    }
  }

  static void wordsToHTML(StreamWriter wr, IEnumerable<string> words) {
    wr.Write("<html><head><meta charset=\"UTF-8\"></head><body>");
    foreach (var w in words) {
      wr.Write("<p>");
      wr.Write(HttpUtility.HtmlEncode(w));
      wr.Write("</p>");
    }
    wr.Write("</body></html>");
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