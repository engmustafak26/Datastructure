using System.Collections.Generic;

namespace Datastructure
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var circularSet = new CircularHashSet<string>(4);

            for (int i = 0; i < 10; i++)
            {
                circularSet.TryAdd(i.ToString());
                Console.WriteLine(i+" : "+ string.Join(", ",circularSet.GetKeys()));

            }
        }


    }
}
