
using Datastructure;

var set = new CircularHashSet<string>(2);

set.TryAdd("A");
set.TryAdd("B");
set.TryAdd("C");

foreach (var item in set.GetKeysByCreatedTime())
{
    Console.WriteLine(item);
}
// Don't forget to dispose when done
set.Dispose();