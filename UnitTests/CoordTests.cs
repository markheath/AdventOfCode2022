using System.Linq;
using AdventOfCode2022;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class CoordTests
    {
        [TestCase(5, 1, 90, 1, -5)]
        [TestCase(5, 1, 180, -5, -1)]
        [TestCase(5, 1, 270, -1, 5)]
        [TestCase(5, 1, 0, 5, 1)]
        public void RotateClockwise(int x, int y, int degrees, int x2, int y2)
        {
            var coord = new Coord(x, y);
            Assert.AreEqual(new Coord(x2,y2), coord.RotateClockwise(degrees));
        }

        [TestCase(5, 1, 90, 1, -5)]
        [TestCase(5, 1, 180, -5, -1)]
        [TestCase(5, 1, 270, -1, 5)]
        [TestCase(5, 1, 0, 5, 1)]
        public void Rotate3DClockwiseAroundZ(int x, int y, int degrees, int x2, int y2)
        {
            var z = 4;
            var coord = new Coord3(x, y, z);
            var actual = coord.RotateClockwiseAroundZ(degrees);
            Assert.AreEqual(new Coord3(x2, y2, z), actual);
        }

        [Test]
        public void GetNeighbours()
        {
            var p = new Coord(4, 5);
            var neighbours = p.Neighbours().ToHashSet();
            Assert.AreEqual(4, neighbours.Count);
            Assert.That(neighbours.Contains(new Coord(3, 5)));
            Assert.That(neighbours.Contains(new Coord(5, 5)));
            Assert.That(neighbours.Contains(new Coord(4, 4)));
            Assert.That(neighbours.Contains(new Coord(4, 6)));
        }

        [Test]
        public void GridNeighbours()
        {
            var g = new Grid<int>(4, 5);
            var neighbours = g.Neighbours((0,1)).ToHashSet();
            Assert.AreEqual(3, neighbours.Count);
            Assert.That(neighbours.Contains(new Coord(0, 0)));
            Assert.That(neighbours.Contains(new Coord(0, 2)));
            Assert.That(neighbours.Contains(new Coord(1, 1)));
        }

        [Test]
        public void LineTo()
        {
            var c = new Coord(3, 3);
            var line = c.LineTo((3,0)).Take(1000).ToList();
            Assert.AreEqual(4, line.Count);
            Assert.AreEqual(new Coord[] { (3, 3), (3, 2), (3, 1), (3, 0) }, line);
        }

        [Test]
        public void RangeCombiner()
        {
            var r = new RangeCombiner();
            r.Add(3, 5);
            r.Add(7, 9);
            r.Add(4, 8);
            Assert.AreEqual(new[] { (3,9) },  r.ToList());

        }
    }
}
