using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace bank {
  public class Class1 {
        public void Test() {
            //new Microsoft.EntityFrameworkCore.Infrastructure.e
            Range range = 1..5;
            var array = new[] { 0, 1, 2, 3, 4, 5 };
            var subArray = array[range]; // = { 1, 2, 3, 4 }
        }
  }
}
