using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtualList;

/// <summary>
/// Allows to show an implementation of <see cref="IReadOnlyList{T}"/> which can be filled partially
/// </summary>
/// <typeparam name="TItem"></typeparam>
public partial class VirtualReadOnlyList<TItem>
{
    private sealed class Page
    {
        public readonly int Index;
        public readonly IReadOnlyList<TItem> Data;

        public Page(int index, IEnumerable<TItem> data)
        {
            Index = index;
            Data = data.ToArray();
        }
    }

    private readonly IVirtualListReader<TItem> _reader;

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
        Count = size;
        _pageSize = pageSize;
    }

    public VirtualReadOnlyList(Func<int, int, IEnumerable<TItem>> get, int size, int pageSize = 256)
        : this(new VirtualListReader<TItem>(get), size, pageSize)
    { }

    private Page GetPage(int index) => TryGetPage(index) ?? LoadPage(index);

    private Page? TryGetPage(int index)
    {
        if (_pages.TryGetValue(index, out var page))
            return page;
        return null;
    }

    private Page LoadPage(int index)
    {
        var page = new Page(index, GetPage(index * PageSize, PageSize).ToArray());
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
