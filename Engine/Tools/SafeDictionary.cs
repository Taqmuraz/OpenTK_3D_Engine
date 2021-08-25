using System;
using System.Collections;
using System.Collections.Generic;

public sealed class SafeDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
{
    private Dictionary<TKey, TValue> provider;
	private Func<TValue> defaultInstanceFunc;

	public SafeDictionary(Func<TValue> defaultInstanceFunc = null)
	{
		provider = new Dictionary<TKey, TValue>();
		if (defaultInstanceFunc == null) defaultInstanceFunc = () => default(TValue);
		this.defaultInstanceFunc = defaultInstanceFunc;
	}

	public TValue this[TKey key]
	{
		get
		{
			if (key == null) return defaultInstanceFunc();
			if (!provider.ContainsKey(key))
			{
				provider.Add(key, defaultInstanceFunc());
			}
			return provider[key];
		}
		set
		{
			if (provider.ContainsKey(key)) provider[key] = value;
			else provider.Add(key, value);
		}
	}

	public void Add(TKey key, TValue value)
	{
		this[key] = value;
	}
	public void Remove(TKey key)
	{
		if (provider.ContainsKey(key)) provider.Remove(key);
	}

	public bool ContainsKey(TKey key)
	{
		try { return provider.ContainsKey(key); }
		catch { return false; }
	}

	public void SetDefaultInstanceFunc (Func<TValue> defaultInstanceFunc)
	{
		if (defaultInstanceFunc != null)
			this.defaultInstanceFunc = defaultInstanceFunc;
		else defaultInstanceFunc = () => default(TValue);
	}

	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		return provider.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return provider.GetEnumerator();
	}
}