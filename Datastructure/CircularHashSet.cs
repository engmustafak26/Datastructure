namespace Datastructure
{
    public class CircularHashSet<T>
    {
        private readonly int _fixedSize;

        public CircularHashSet(int fixedSize)
        {
            _fixedSize = fixedSize;
            LinkedDictionary = new(_fixedSize);
        }

        private T OldKey = default;
        private T PreviousKey = default;
        private Dictionary<T, T?> LinkedDictionary;

        public bool TryAdd(T key)
        {
            if (LinkedDictionary.Count < _fixedSize)
            {
                return TryAddInternal(key);
            }

            var nextOldKey = LinkedDictionary[OldKey];
            LinkedDictionary.Remove(OldKey);
            OldKey = nextOldKey;
            return TryAddInternal(key);
        }

        public T? Find(T key)
        {
            LinkedDictionary.TryGetValue(key, out T? value);
            return value;
        }

        public T[] GetKeys()
        {
            return LinkedDictionary.Keys.ToArray();

        }

        private bool TryAddInternal(T key)
        {
            bool isSuccess = LinkedDictionary.TryAdd(key, default);
            if (!isSuccess)
            {
                return false;
            }

            if (LinkedDictionary.Count == 1)
            {
                OldKey = key;
            }
            else
            {
                PreviousKey = PreviousKey is null ? OldKey! : PreviousKey;
                LinkedDictionary[PreviousKey!] = key;
                PreviousKey = key;
            }
            return true;
        }

    }
}
