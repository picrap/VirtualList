using System.Collections.Generic;

namespace VirtualList;

public interface IVirtualListReader<out TItem>
{
    /// <summary>
    /// Gets data at given offset and from requested size (this is aligned to <see cref="VirtualReadOnlyList{TItem}.PageSize"/>)
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public IEnumerable<TItem> Get(int offset, int count);
}