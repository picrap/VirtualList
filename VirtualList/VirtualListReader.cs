using System;
using System.Collections.Generic;

namespace VirtualList;

/// <summary>
/// Simple adapter for lambdas
/// </summary>
/// <typeparam name="TItem">The type of the item.</typeparam>
/// <seealso cref="VirtualList.IVirtualListReader&lt;TItem&gt;" />
public class VirtualListReader<TItem> : IVirtualListReader<TItem>
{
    private readonly Func<int, int, IEnumerable<TItem>> _get;

    public VirtualListReader(Func<int, int, IEnumerable<TItem>> get)
    {
        _get = get;
    }

    public IEnumerable<TItem> Get(int offset, int count) => _get(offset, count);
}