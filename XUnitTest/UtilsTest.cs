using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UtilsTest {
  public class JsonTest {
    [Fact]
    public void Test1() {
      JsonNew.SerializeEnum(@"c:\temp\pom.json", objs());
      Parallel.ForEach(Enumerable.Range(0, 100), idx => {
        var count = 0;
        JsonNew.DeserializeEnum(typeof(X), @"c:\temp\pom.json", x => {
          count++;
        });
        Assert.Equal(cnt * 2, count);
        count = 0;
      });
    }
    const int cnt = 20000;

    class X {
      public int a { get; set; }
      public string b { get; set; }
    }

    static IEnumerable<X> objs() {
      for (var i = 0; i < cnt; i++) {
        yield return new X { a = 5, b = "xxx" };
        yield return new X { a = 6, b = "yyy" };
      }
    }

  }

}
