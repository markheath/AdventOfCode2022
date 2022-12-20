using System.Collections;
using System.Diagnostics.CodeAnalysis;
using SuperLinq;

namespace AdventOfCode2022;

class DefaultDictionary<TKey, TValue> : IDictionary<TKey, TValue>
{
    private readonly Dictionary<TKey, TValue> dictionary;
    public DefaultDictionary(TValue defaultValue)
    {
        DefaultValue = defaultValue;
        this.dictionary = new();
    }
    public TValue this[TKey key]
    {
        get
        {
            if (dictionary.TryGetValue(key, out var val))
            {
                return val;
            }
            return DefaultValue;
        }
        set
        {
            dictionary[key] = value;
        }
    }


    public ICollection<TKey> Keys => dictionary.Keys;

    public ICollection<TValue> Values => dictionary.Values;

    public int Count => dictionary.Count;

    public bool IsReadOnly => ((IDictionary<TKey, TValue>)dictionary).IsReadOnly;

    public TValue DefaultValue { get; }

    public void Add(TKey key, TValue value)
    {
        dictionary.Add(key, value);
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        ((IDictionary<TKey,TValue>)dictionary).Add(item);
    }

    public void Clear()
    {
        dictionary.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return ((IDictionary<TKey, TValue>)dictionary).Contains(item);
    }

    public bool ContainsKey(TKey key)
    {
        return dictionary.ContainsKey(key);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        dictionary.CopyTo(array, arrayIndex);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return dictionary.GetEnumerator();
    }

    public bool Remove(TKey key)
    {
        return dictionary.Remove(key);
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        return ((IDictionary<TKey, TValue>)dictionary).Remove(item);
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        return dictionary.TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return dictionary.GetEnumerator();
    }
}
