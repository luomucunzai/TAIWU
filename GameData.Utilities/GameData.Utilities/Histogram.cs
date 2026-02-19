using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.Utilities;

public class Histogram
{
	private readonly double _min;

	private readonly double _max;

	private readonly int _bins;

	private readonly double[] _nodes;

	private readonly int[] _amounts;

	private bool _isIntegers;

	public Histogram(int min, int max, int bins = 10)
	{
		if (min >= max)
		{
			throw new Exception("min must less than max.");
		}
		if (bins <= 0)
		{
			throw new Exception("bins must greater than zero.");
		}
		if ((max - min) % bins != 0)
		{
			throw new Exception("cannot calculate bin width as integer.");
		}
		_isIntegers = true;
		_min = min;
		_max = max;
		_bins = bins;
		double num = (_max - _min) / (double)_bins;
		int num2 = _bins + 1;
		_nodes = new double[num2];
		for (int i = 0; i < num2; i++)
		{
			_nodes[i] = _min + num * (double)i;
		}
		_nodes[0] = _min;
		_nodes[num2 - 1] = _max;
		_amounts = new int[_bins + 2];
	}

	public Histogram(double min, double max, int bins = 10)
	{
		if (min >= max)
		{
			throw new Exception("min must less than max.");
		}
		if (bins <= 0)
		{
			throw new Exception("bins must greater than zero.");
		}
		_isIntegers = false;
		_min = min;
		_max = max;
		_bins = bins;
		double num = (_max - _min) / (double)_bins;
		int num2 = _bins + 1;
		_nodes = new double[num2];
		for (int i = 0; i < num2; i++)
		{
			_nodes[i] = _min + num * (double)i;
		}
		_nodes[0] = _min;
		_nodes[num2 - 1] = _max;
		_amounts = new int[_bins + 2];
	}

	public void Record(double value)
	{
		int num = _amounts.Length;
		for (int i = 0; i < num; i++)
		{
			if (i == 0)
			{
				if (value < _min)
				{
					_amounts[i]++;
					break;
				}
			}
			else if (i == num - 1)
			{
				if (value > _max)
				{
					_amounts[i]++;
					break;
				}
			}
			else if (i == num - 2)
			{
				double num2 = _nodes[i - 1];
				double num3 = _nodes[i];
				if (value >= num2 && value <= num3)
				{
					_amounts[i]++;
					break;
				}
			}
			else
			{
				double num4 = _nodes[i - 1];
				double num5 = _nodes[i];
				if (value >= num4 && value < num5)
				{
					_amounts[i]++;
					break;
				}
			}
		}
	}

	public void Record(IEnumerable<int> values)
	{
		foreach (int value in values)
		{
			Record(value);
		}
	}

	public void Record(IEnumerable<double> values)
	{
		foreach (double value in values)
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
			if (j == 0)
			{
				stringBuilder.AppendFormat("{0,20} ({1,10}): ", _isIntegers ? $" - {_min:N0}" : $" - {_min:N3}", _amounts[j]);
				AppendBlocks(stringBuilder, _amounts[j], num, maxBlocks);
				continue;
			}
			if (j == num3 - 1)
			{
				stringBuilder.AppendFormat("{0,20} ({1,10}): ", _isIntegers ? $"{_max:N0} - " : $"{_max:N3} - ", _amounts[j]);
				AppendBlocks(stringBuilder, _amounts[j], num, maxBlocks);
				continue;
			}
			double num4 = _nodes[j - 1];
			double num5 = _nodes[j];
			stringBuilder.AppendFormat("{0,20} ({1,10}): ", _isIntegers ? $"{num4:N0} - {num5:N0}" : $"{num4:N3} - {num5:N3}", _amounts[j]);
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
