namespace System.Collections.Generic
{
    public static class DictionaryExtensions
    {
        public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, Func<TValue> nullValue)
        {
            return dic.ContainsKey(key) ? dic[key] : nullValue();
        }

        public static object Get<T, TValue>(this Dictionary<T, TValue> dic, T key)
        {
            return dic.Get(key, () => default(TValue));
        }
    }
}