using Qdbm.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1 {
  class Program {
    static void Main(string[] args) {
      using (var fs = File.Open(@"c:\temp\pom.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite)) {
        var qdbm = new QdbmDatabase(fs);
        for (var i = 0; i < 100000; i++) {
          qdbm.Put(new QdbmKey(i.ToString()), new byte[] { 0, 1, 3 });
        }
      }
    }
  }
}
