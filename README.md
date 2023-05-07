# VirtualList

## Why?

The idea was to have a dynamically filled list, initially to get big password derivation without losing much time at start.  
However it also can be used for paged views.

## How?

```csharp
var list = new VirtualReadOnlyList<int>(
reader, // an implementation of IVirtualListReader 
        // or a simple lambda to provide data parts 
20,     // the total size (as returned by IReadOnlyList<>.Count)
7       // the page size, which the reader fills
);

// then use it, it is a IReadOnlyList
var a = list[12];
var r = list[1..3];
```
