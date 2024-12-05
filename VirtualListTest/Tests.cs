using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using VirtualList;

namespace VirtualListTest;

#pragma warning disable NUnit2005

public class Tests
{
    public sealed class IntReader : IVirtualListReader<int>
    {
        public readonly HashSet<int> Pages = new();

        public IEnumerable<int> Get(int offset, int count)
        {
            Pages.Add(offset);
            return Enumerable.Range(offset, count);
        }
    }

    [Test]
    public void SimpleLoadTest()
    {
        var reader = new IntReader();
        var list = new VirtualReadOnlyList<int>(reader, 20, 7);
        Assert.That(list[3], Is.EqualTo(3));
        Assert.That(list[13], Is.EqualTo(13));
        Assert.That(reader.Pages.Contains(0));
        Assert.That(reader.Pages.Contains(7));
    }

    [Test]
    public void RangeTest()
    {
        var reader = new IntReader();
        var list = new VirtualReadOnlyList<int>(reader, 20, 7);
        var range = list.Skip(10).Take(3).ToArray();
        Assert.That(range[0], Is.EqualTo(10));
        Assert.That(range[1], Is.EqualTo(11));
        Assert.That(range[2], Is.EqualTo(12));
        Assert.That(reader.Pages.Count, Is.EqualTo(1));
        Assert.That(reader.Pages.Single(), Is.EqualTo(7));
    }

    [Test]
    public void ToArrayTest()
    {
        var reader = new IntReader();
        var list = new VirtualReadOnlyList<int>(reader, 32, 8);
        var range = list.ToArray();
        Assert.That(range.Length, Is.EqualTo(32));
    }
}