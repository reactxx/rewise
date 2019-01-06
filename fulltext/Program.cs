﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.IO;

namespace fulltext
{
  class Program
  {
    static void Main(string[] args)
    {
      VLQ2.Test();
      return;
      //HunspellLib.normalizeHunspellLangs();
      //HunspellLib.extractWordLists();
      //return;
      //LangsLib.Metas.designTimeRebuild();

      //var words = File.ReadAllLines(@"d:\rewise\dictionariesWordLists\cs_CZ.txt");
      //var stemms = Stemming.getStemms(words, LangsLib.langs.cs_cz, 5000);

      //var words = File.ReadAllLines(@"d:\rewise\dictionariesWordLists\el_GR.txt");
      //var stemms = Stemming.getStemms(words, LangsLib.langs.el_gr, 5000);

      //var words = File.ReadAllLines(@"d:\rewise\dictionariesWordLists\de.txt");
      //var stemms = Stemming.getStemms(words, LangsLib.langs.de_de, 5000);

      var words = File.ReadAllLines(@"d:\rewise\dictionariesWordLists\cs-CZ.txt");
      var res = new GetAllStemmsResult();
      StemmingAll.getAllStemms(res, 0, words, LangsLib.langs.cs_cz, 5000);
      StemmingAll.dumpAllStemmsResult(res, @"D:\rewise\fulltext\sqlserver\dumps\cs_cz.xml");

      //var words = File.ReadAllLines(@"d:\rewise\dictionariesWordLists\de-de.txt");
      //var res = new GetAllStemmsResult();
      //StemmingAll.getAllStemms(res, 0, words, LangsLib.langs.de_de, 5000);
      //StemmingAll.dumpAllStemmsResult(res, @"D:\rewise\fulltext\sqlserver\dumps\de_de.xml");

      //var words = File.ReadAllLines(@"d:\rewise\dictionariesWordLists\ru-ru.txt");
      //var res = new GetAllStemmsResult();
      //StemmingAll.getAllStemms(res, 0, words, LangsLib.langs.ru_ru, 5000);
      //StemmingAll.dumpAllStemmsResult(res, @"D:\rewise\fulltext\sqlserver\dumps\ru_ru.xml");

      //var words = File.ReadAllLines(@"d:\rewise\dictionariesWordLists\el-gr.txt");
      //var res = new GetAllStemmsResult();
      //StemmingAll.getAllStemms(res, 0, words, LangsLib.langs.el_gr, 5000);
      //StemmingAll.dumpAllStemmsResult(res, @"D:\rewise\fulltext\sqlserver\dumps\el_gr.xml");

      return;
    }
  }
}
