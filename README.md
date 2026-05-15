# Datastructure

A small .NET library of custom collection types.

## `CircularHashSet<T>`

`CircularHashSet<T>` is a fixed-capacity set backed by a dictionary. When the set is full, adding a new key evicts the oldest entry in insertion order and inserts the new key.

- **Fixed capacity** — size is set at construction and does not grow.
- **Insertion-order eviction** — the oldest key is removed when capacity is exceeded.
- **Thread-safe** — read/write operations use `ReaderWriterLockSlim`.
- **Duplicate keys** — `TryAdd` returns `false` if the key is already present.

### Install

```bash
dotnet add package Datastructure
```

### Example

```csharp
using Datastructure;

var set = new CircularHashSet<string>(3);

set.TryAdd("a"); // true
set.TryAdd("b"); // true
set.TryAdd("c"); // true
set.TryAdd("d"); // true — "a" is evicted

set.Contains("a"); // false
set.Contains("d"); // true

set.GetKeys();              // current keys (order not guaranteed)
set.GetKeysByCreatedTime(); // keys ordered by insertion time
```

### API

| Member | Description |
|--------|-------------|
| `CircularHashSet(int fixedSize)` | Creates a set with the given maximum capacity. |
| `bool TryAdd(T key)` | Adds a key, or evicts the oldest if full. Returns `false` if the key already exists. |
| `bool Contains(T key)` | Whether the key is in the set. |
| `T[] GetKeys()` | Snapshot of all keys. |
| `T[] GetKeysByCreatedTime()` | Keys ordered by insertion time. |
| `int Count` | Number of keys currently stored. |
| `void Clear()` | Removes all keys. |
| `void Dispose()` | Releases the lock. |

## Requirements

- .NET Standard 2.0 (compatible with .NET Framework 4.6.1+, .NET Core 2.0+, and .NET 5+)
