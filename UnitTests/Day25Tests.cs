using System;
using System.Runtime.CompilerServices;
using AdventOfCode2022;
using NUnit.Framework;

namespace UnitTests;

public class Day25Tests
{
    private const string TestInput = @"1=-0-2
12111
2=0=
21
2=01
111
20012
112
1=-1=
1-12
12
1=
122";


    /*
    Decimal SNAFU
        1              1
        2              2
        3             1=
        4             1-
        5             10
        6             11
        7             12
        8             2=
        9             2-
       10             20
       15            1=0
       20            1-0
     2022         1=11-2
    12345        1-0---0
314159265  1121-1110-1=0


         SNAFU Decimal
1=-0-2     1747
 12111      906
  2=0=      198
    21       11
  2=01      201
   111       31
 20012     1257
   112       32
 1=-1=      353
  1-12      107
    12        7
    1=        3
   122       37
    */

    [TestCase(1, "1")]
    [TestCase(2, "2")]
    [TestCase(3, "1=")]
    [TestCase(4, "1-")]
    [TestCase(5, "10")]
    [TestCase(6, "11")]
    [TestCase(7, "12")]
    [TestCase(8, "2=")]
    [TestCase(9, "2-")]
    [TestCase(10, "20")]
    [TestCase(15, "1=0")]
    [TestCase(20, "1-0")]
    [TestCase(2022, "1=11-2")]
    [TestCase(12345, "1-0---0")]
    [TestCase(314159265, "1121-1110-1=0")]



    /*
           15            1=0
           20            1-0
         2022         1=11-2
        12345        1-0---0
    314159265  1121-1110-1=0*/

    public void DecimalToSnafu(long number, string snafu)
    {
        var convertedToSnafu = Day25.DecimalToSnafu(number);
        Assert.AreEqual(snafu, convertedToSnafu);
        var convertedBack = Day25.SnafuToDecimal(snafu);
        Assert.AreEqual(number, convertedBack);
    }

    [Test]
    public void Day25TestInput()
    {
        var solver = new Day25();
        var n = solver.Solve(TestInput.Split("\r\n"));
        Assert.AreEqual(("2=-1=0", ""), n);       
    }

}