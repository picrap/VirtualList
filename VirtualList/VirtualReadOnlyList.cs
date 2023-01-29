using System.Collections.Generic;
using System.Linq;

namespace VirtualList;

public partial class VirtualReadOnlyList<TItem>
{
    private class Page
    {
        public int Index;
        public TItem[] Data;
    }

    private readonly IVirtualListReader<TItem> _reader;
    private readonly int _size;

    private readonly Dictionary<int, Page> _pages = new();

    private int _pageSize;

    public int PageSize
    {
        get { return _pageSize; }
        set
        {
            if (_pageSize != value)
            {
                _pageSize = value;
                _pages.Clear();
            }
        }
    }

    public VirtualReadOnlyList(IVirtualListReader<TItem> reader, int size, int pageSize = 256)
    {
        _reader = reader;
        _size = size;
        _pageSize = pageSize;
    }

    private Page GetPage(int index) => TryGetPage(index) ?? LoadPage(index);

    private Page? TryGetPage(int index)
    {
        if (_pages.TryGetValue(index, out var page))
            return page;
        return null;
    }

    private Page LoadPage(int index)
    {
        var page = new Page { Index = index, Data = GetPage(index * PageSize, PageSize).ToArray() };
        _pages[index] = page;
        return page;
    }

    protected virtual IEnumerable<TItem> GetPage(int offset, int size) => _reader.Get(offset, size);

    private TItem GetAt(int index)
    {
        var page = GetPage(index / PageSize);
        return page.Data[index % PageSize];
    }
}
