﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using W = Microsoft.Office.Interop.Word;

public static class WordSpellCheck {

  static void SpellcheckLow(string lang, int start, List<int> res, string fn) {
    var w = new W.Application();
    w.Visible = true;
    try {
      W.Document doc = w.Documents.Open(fn, false, true);
      try {
        var lid = (W.WdLanguageID)Langs.nameToMeta[lang].WordSpellCheckLCID;
        if (lid == W.WdLanguageID.wdLanguageNone) {
          Console.WriteLine("Wrong lang: " + lang);
          return;
        }
        doc.Content.LanguageID = lid;
        doc.SpellingChecked = false;
        var parCount = start;
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
  }

  const int maxLen = 5000;

  public static List<int> Spellcheck(string lang, IList<string> words) {

    var res = new List<int>();
    var fn = Path.GetTempFileName();
    foreach (var intl in Intervals2.intervals(words.Count, maxLen)) {
      Console.WriteLine(string.Format("{0}: {1}-{2}", lang, intl.start, intl.end));
      if (File.Exists(fn)) File.Delete(fn);
      using (var wr = new StreamWriter(fn, true))
        wordsToHTML2(wr, words, intl.start, intl.end, lang);
      SpellcheckLow(lang, intl.start, res, fn);
    }
    var wrongsFn = string.Format("{0}{1}.html", AppDomain.CurrentDomain.BaseDirectory[0] + @":\temp\", lang);
    using (var wr = new StreamWriter(wrongsFn))
      wordsToHTML(wr, res.Select(i => words[i]), lang);
    return res;
  }

  public static List<int> Spellcheck2(string lang, IList<string> words) {
    //var fn = Path.GetTempFileName();
    var fn = string.Format("{0}{1}.html", AppDomain.CurrentDomain.BaseDirectory[0] + @":\temp\", lang);
    if (File.Exists(fn)) File.Delete(fn);
    try {
      using (var wr = new StreamWriter(fn, true))
        wordsToHTML(wr, words, lang);
      var w = new W.Application();
      w.Visible = true;
      var res = new List<int>();
      try {
        W.Document doc = w.Documents.Open(fn, false, true);
        try {
          var lid = (W.WdLanguageID)Langs.nameToMeta[lang].WordSpellCheckLCID;
          if (lid == W.WdLanguageID.wdLanguageNone) {
            Console.WriteLine("Wrong lang: " + lang);
            return res;
          }
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
          return res;
        } finally {
          object dontSave = W.WdSaveOptions.wdDoNotSaveChanges;
          doc.Close(ref dontSave);
          using (var wr = new StreamWriter(fn))
            wordsToHTML(wr, res.Select(i => words[i]), lang);
        }
      } finally {
        w.Quit();
      }
    } finally {
      //try { File.Delete(fn); } catch { }
    }
  }

  static void wordsToHTML(StreamWriter wr, IEnumerable<string> words, string lang) {
    wr.Write("<html lang=\"");
    wr.Write(lang);
    wr.Write("\"><head><meta charset=\"UTF-8\"></head><body>");
    foreach (var w in words) {
      wr.Write("<p>");
      wr.Write(HttpUtility.HtmlEncode(w));
      wr.Write("</p>");
    }
    wr.Write("</body></html>");
  }

  static void wordsToHTML2(StreamWriter wr, IList<string> words, int beg, int end, string lang) {
    wr.Write("<html lang=\"");
    wr.Write(lang);
    wr.Write("\"><head><meta charset=\"UTF-8\"></head><body>");
    for (var i = beg; i < end; i++) {
      wr.Write("<p>");
      wr.Write(HttpUtility.HtmlEncode(words[i]));
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