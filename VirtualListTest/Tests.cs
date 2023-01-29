using VirtualList;

namespace VirtualListTest;

public class Tests
{
    public class IntReader : IVirtualListReader<int>
    {
        public ISet<int> Pages = new HashSet<int>();

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
        Assert.AreEqual(3, list[3]);
        Assert.AreEqual(13, list[13]);
        Assert.IsTrue(reader.Pages.Contains(0));
        Assert.IsTrue(reader.Pages.Contains(7));
    }

    [Test]
    public void RangeTest()
    {
        var reader = new IntReader();
        var list = new VirtualReadOnlyList<int>(reader, 20, 7);
        var range = list.Skip(10).Take(3).ToArray();
        Assert.AreEqual(10, range[0]);
        Assert.AreEqual(11, range[1]);
        Assert.AreEqual(12, range[2]);
        Assert.AreEqual(1, reader.Pages.Count);
        Assert.AreEqual(7, reader.Pages.Single());
    }
}