using System;
using System.Collections;
using System.Collections.Generic;

public sealed class ReadOnlyArrayList<T> : IEnumerable<T>
{
	List<T> list;

	public int Count => list == null ? 0 : list.Count;

	private ReadOnlyArrayList(List<T> list)
	{
		this.list = list;
	}

	public T this[int index]
	{
		get
		{
			if (list == null) throw new System.IndexOutOfRangeException();
			else return list[index];
		}
	}

	public IEnumerator<T> EmptyEnumerator()
	{
		yield break;
	}

	public IEnumerator<T> GetEnumerator()
	{
		return list == null ? EmptyEnumerator() : ((IEnumerable<T>)list).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return list == null ? EmptyEnumerator() : ((IEnumerable)list).GetEnumerator();
	}

	public static implicit operator ReadOnlyArrayList<T>(List<T> list)
	{
		return new ReadOnlyArrayList<T>(list);
	}

	public int IndexOf(T element)
	{
		return list == null ? -1 : list.IndexOf(element);
	}
}

