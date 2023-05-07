
using System.Collections;
using System.Collections.Generic;

namespace VirtualList;

partial class VirtualReadOnlyList<TItem> : IReadOnlyList<TItem>
{
#pragma warning disable S3881
    private sealed class Enumerator : IEnumerator<TItem>
#pragma warning restore S3881
    {
        private readonly VirtualReadOnlyList<TItem> _list;
        private int _currentIndex;

        public TItem Current => _list[_currentIndex];
        object? IEnumerator.Current => Current;

        public Enumerator(VirtualReadOnlyList<TItem> list)
        {
            _list = list;
            Reset();
        }

        public void Dispose()
        {
            // nothing to do here
        }

        public bool MoveNext() => _currentIndex++ < _list.Count;
        public void Reset() => _currentIndex = -1;
    }

    public int Count { get; }

    public IEnumerator<TItem> GetEnumerator() => new Enumerator(this);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public TItem this[int index] => GetAt(index);
}
