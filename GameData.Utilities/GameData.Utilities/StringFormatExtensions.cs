using System;

namespace GameData.Utilities;

public static class StringFormatExtensions
{
	public static string GetFormat(this string str, object arg0)
	{
		try
		{
			return string.Format(str, arg0);
		}
		catch (Exception arg1)
		{
			AdaptableLog.Warning($"Failed to format string \"{str}\" with args [{arg0}]\n{arg1}", appendWarningMessage: true);
			return str;
		}
	}

	public static string GetFormat(this string str, object arg0, object arg1)
	{
		try
		{
			return string.Format(str, arg0, arg1);
		}
		catch (Exception ex)
		{
			AdaptableLog.Warning($"Failed to format string \"{str}\" with args [{arg0},{arg1}]\n{ex}", appendWarningMessage: true);
			return str;
		}
	}

	public static string GetFormat(this string str, object arg0, object arg1, object arg2)
	{
		try
		{
			return string.Format(str, arg0, arg1, arg2);
		}
		catch (Exception ex)
		{
			AdaptableLog.Warning($"Failed to format string \"{str}\" with args [{arg0},{arg1},{arg2}]\n{ex}", appendWarningMessage: true);
			return str;
		}
	}

	public static string GetFormat(this string str, params object[] args)
	{
		try
		{
			return string.Format(str, args);
		}
		catch (Exception arg)
		{
			AdaptableLog.Warning($"Failed to format string \"{str}\" with args [{((args == null) ? string.Empty : string.Join(',', args))}].\n{arg}", appendWarningMessage: true);
			return str;
		}
	}

	public static string ReplaceLast(this string input, string oldValue, string newValue)
	{
		if (string.IsNullOrEmpty(input))
		{
			return input;
		}
		int num = input.LastIndexOf(oldValue, StringComparison.Ordinal);
		if (num < 0)
		{
			return input;
		}
		string text = ((num > 0) ? input.Substring(0, num) : string.Empty);
		string text2;
		if (num + oldValue.Length >= input.Length)
		{
			text2 = string.Empty;
		}
		else
		{
			int num2 = num + oldValue.Length;
			text2 = input.Substring(num2, input.Length - num2);
		}
		string text3 = text2;
		string text4 = newValue;
		if (!string.IsNullOrEmpty(text))
		{
			text4 = text + text4;
		}
		if (!string.IsNullOrEmpty(text3))
		{
			text4 += text3;
		}
		return text4;
	}

	public static string FirstCharToLower(this string input)
	{
		if (string.IsNullOrEmpty(input))
		{
			return input;
		}
		if (input.Length == 1)
		{
			return input.ToLower();
		}
		return char.ToLower(input[0]) + input.Substring(1, input.Length - 1);
	}

	public static string FirstCharToUpper(this string input)
	{
		if (string.IsNullOrEmpty(input))
		{
			return input;
		}
		if (input.Length == 1)
		{
			return input.ToUpper();
		}
		return char.ToUpper(input[0]) + input.Substring(1, input.Length - 1);
	}
}
