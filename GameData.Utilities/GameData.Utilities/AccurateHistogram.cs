using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.Utilities;

public class AccurateHistogram<T> where T : IEquatable<T>
{
	private readonly int[] _amounts;

	private readonly string[] _names;

	private readonly T[] _values;

	public AccurateHistogram(string[] names, T[] values)
	{
		if (names != null)
		{
			Tester.Assert(names.Length == values.Length);
		}
		_names = names;
		_values = values;
		_amounts = new int[_values.Length];
	}

	public void Record(T value)
	{
		int num = _amounts.Length;
		for (int i = 0; i < num; i++)
		{
			if (value.Equals(_values[i]))
			{
				_amounts[i]++;
			}
		}
	}

	public void Record(IEnumerable<T> values)
	{
		foreach (T value in values)
		{
			Record(value);
		}
	}

	public string GetTextGraph(int maxBlocks = 100)
	{
		long num = 0L;
		int[] amounts = _amounts;
		foreach (int num2 in amounts)
		{
			num += num2;
		}
		StringBuilder stringBuilder = new StringBuilder();
		int num3 = _amounts.Length;
		for (int j = 0; j < num3; j++)
		{
			stringBuilder.AppendFormat("{0,20} ({1,10}): ", (_names != null) ? _names[j] : ((object)_values[j]), _amounts[j]);
			AppendBlocks(stringBuilder, _amounts[j], num, maxBlocks);
		}
		return stringBuilder.ToString();
	}

	private static void AppendBlocks(StringBuilder sb, int amount, long totalAmount, int maxBlocks)
	{
		int num = (int)Math.Round((double)amount / (double)totalAmount * (double)maxBlocks);
		for (int i = 0; i < num; i++)
		{
			sb.Append('█');
		}
		for (int j = 0; j < maxBlocks - num; j++)
		{
			sb.Append('░');
		}
		sb.AppendLine();
	}
}
