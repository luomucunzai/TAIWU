using System.Collections.Generic;
using System.Linq;

namespace GameData.Utilities;

public class StatisticValue
{
	private readonly List<int> _values = new List<int>();

	public double Total { get; private set; }

	public int Count => _values.Count;

	public double Median => GetMedian(_values, 0);

	public double Mode => GetMode(_values, 0);

	public double Average => Total / (double)Count;

	public void Record(int value)
	{
		_values.Add(value);
		Total += value;
	}

	public static int GetAverage(IReadOnlyList<int> numbers, int defaultValue)
	{
		if (numbers.Count == 0)
		{
			return defaultValue;
		}
		int num = 0;
		foreach (int number in numbers)
		{
			num += number;
		}
		return num / numbers.Count;
	}

	public static int GetMedian(IReadOnlyList<int> numbers, int defaultValue)
	{
		if (numbers.Count == 0)
		{
			return defaultValue;
		}
		List<int> list = new List<int>(numbers);
		list.Sort();
		int count = list.Count;
		if (count % 2 != 0)
		{
			return list[count / 2];
		}
		return (list[count / 2 - 1] + list[count / 2]) / 2;
	}

	public static int GetMode(IReadOnlyList<int> numbers, int defaultValue)
	{
		if (numbers.Count == 0)
		{
			return defaultValue;
		}
		IEnumerable<IGrouping<int, int>> enumerable = from n in numbers
			group n by n;
		int num = -1;
		int result = defaultValue;
		foreach (IGrouping<int, int> item in enumerable)
		{
			int num2 = item.Count();
			if (num2 > num)
			{
				result = item.Key;
				num = num2;
			}
		}
		return result;
	}
}
