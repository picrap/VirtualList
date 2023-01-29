
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace VirtualList;

partial class VirtualReadOnlyList<TItem> : IReadOnlyList<TItem>
{
    private class Enumerator : IEnumerator<TItem>
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

        public bool MoveNext() => _currentIndex++ < _list.Count;
        public void Reset() => _currentIndex = -1;
        public void Dispose() { }
    }

    public int Count => _size;

    public IEnumerator<TItem> GetEnumerator() => new Enumerator(this);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public TItem this[int index] => GetAt(index);
}
