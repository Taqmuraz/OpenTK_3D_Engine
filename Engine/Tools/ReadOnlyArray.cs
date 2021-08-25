using System.Collections;
using System.Collections.Generic;

public sealed class ReadOnlyArray<T> : IEnumerable<T>
{
	T[] array;

	public int Length => array == null ? 0 : array.Length;

	private ReadOnlyArray(T[] array)
	{
		this.array = array;
	}

	public T this[int index]
	{
		get
		{
			if (array == null) throw new System.IndexOutOfRangeException();
			else return array[index];
		}
	}

	public IEnumerator<T> EmptyEnumerator()
	{
		yield break;
	}

	public IEnumerator<T> GetEnumerator()
	{
		return array == null ? EmptyEnumerator() : ((IEnumerable<T>)array).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return array == null ? EmptyEnumerator() : array.GetEnumerator();
	}

	public static implicit operator ReadOnlyArray<T>(T[] array)
	{
		return new ReadOnlyArray<T>(array);
	}
}