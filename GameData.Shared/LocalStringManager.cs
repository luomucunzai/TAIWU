using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GameData;
using GameData.Utilities;

public class LocalStringManager
{
	public class LanguagePackInfo
	{
		public string PackName;

		public string[] ArrayLanguageData;

		public Dictionary<string, string> MapLanguageData;
	}

	private static readonly Dictionary<string, string> _languageKeyMap = new Dictionary<string, string>
	{
		{ "schinese", "CN" },
		{ "zh-CN", "CN" }
	};

	private const string LanguageFolderPath = "StreamingAssets";

	private const string UiLanguageFileName = "ui_language";

	private static readonly string ConfigArrayDicSeparator = new string('>', 50);

	private static Dictionary<string, LanguagePackInfo> _configLanguageMap;

	private static string[] _localUiLanguageArray;

	private static Func<string, string> _customLanguageHandlerOfKey;

	private static Func<ushort, string> _customLanguageHandlerOfId;

	private static string[] _number2String;

	private static string[] _quantifier;

	private static string _zero;

	private static string _ten;

	private static string _negative;

	private static readonly StringBuilder StringBuilder = new StringBuilder();

	public static bool UiLanguageInitReady => _localUiLanguageArray != null;

	public static bool ConfigLanguageInitReady => _configLanguageMap != null;

	public static IReadOnlyDictionary<string, string> LanguageInfoKeyMap => _languageKeyMap;

	public static string CurLanguageKey => ExternalDataBridge.Context.Language;

	private static void CnNumberConverterInit()
	{
		_number2String = new string[10]
		{
			Get(LanguageKey.LK_Number0),
			Get(LanguageKey.LK_Number1),
			Get(LanguageKey.LK_Number2),
			Get(LanguageKey.LK_Number3),
			Get(LanguageKey.LK_Number4),
			Get(LanguageKey.LK_Number5),
			Get(LanguageKey.LK_Number6),
			Get(LanguageKey.LK_Number7),
			Get(LanguageKey.LK_Number8),
			Get(LanguageKey.LK_Number9)
		};
		_quantifier = new string[19]
		{
			Get(LanguageKey.LK_NumberQuantifier0),
			Get(LanguageKey.LK_NumberQuantifier1),
			Get(LanguageKey.LK_NumberQuantifier2),
			Get(LanguageKey.LK_NumberQuantifier3),
			Get(LanguageKey.LK_NumberQuantifier0),
			Get(LanguageKey.LK_NumberQuantifier1),
			Get(LanguageKey.LK_NumberQuantifier2),
			Get(LanguageKey.LK_NumberQuantifier4),
			Get(LanguageKey.LK_NumberQuantifier0),
			Get(LanguageKey.LK_NumberQuantifier1),
			Get(LanguageKey.LK_NumberQuantifier2),
			Get(LanguageKey.LK_NumberQuantifier3),
			Get(LanguageKey.LK_NumberQuantifier0),
			Get(LanguageKey.LK_NumberQuantifier1),
			Get(LanguageKey.LK_NumberQuantifier2),
			Get(LanguageKey.LK_NumberQuantifier3),
			Get(LanguageKey.LK_NumberQuantifier0),
			Get(LanguageKey.LK_NumberQuantifier1),
			Get(LanguageKey.LK_NumberQuantifier2)
		};
		_zero = Get(LanguageKey.LK_Number0);
		_ten = Get(LanguageKey.LK_NumberQuantifier0);
		_negative = Get(LanguageKey.LK_NumberNegative);
	}

	private static string CnNumberConverter(long number)
	{
		StringBuilder stringBuilder = StringBuilder;
		stringBuilder.Clear();
		if (number < 0)
		{
			stringBuilder.Append(_negative);
		}
		string numberStr = NumberStr(number);
		for (int i = 0; i < numberStr.Length; i++)
		{
			char c = numberStr[i];
			int num = numberStr.Length - i - 1;
			string text = ((num == 0) ? string.Empty : _quantifier[num - 1]);
			if (c == '1' && i == 0 && text == _ten)
			{
				stringBuilder.Append(text);
				continue;
			}
			bool flag = c == '0' && stringBuilder.Length > 0 && stringBuilder[stringBuilder.Length - 1].ToString() == _number2String[0];
			if (c == '0' && num > 0 && num % 4 == 0 && (num / 4 == (numberStr.Length - 1) / 4 || num > 4))
			{
				if (flag)
				{
					stringBuilder[stringBuilder.Length - 1] = text[0];
				}
				else
				{
					stringBuilder.Append(text);
				}
			}
			else if (!flag)
			{
				stringBuilder.Append(NumberCharToString(i));
				if (c != '0' && !string.IsNullOrEmpty(text))
				{
					stringBuilder.Append(text);
				}
			}
		}
		if (numberStr.Length > 1 && stringBuilder[stringBuilder.Length - 1].ToString() == _zero)
		{
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
		}
		return stringBuilder.ToString();
		string NumberCharToString(int index)
		{
			return _number2String[int.Parse(numberStr[index].ToString())];
		}
	}

	private static string NumberStr(long number)
	{
		if (number >= 0)
		{
			return number.ToString();
		}
		if (number == long.MinValue)
		{
			return 9223372036854775808uL.ToString();
		}
		return (-number).ToString();
	}

	public static void Init(string languageKey)
	{
		List<LanguagePackInfo> list = new List<LanguagePackInfo>();
		string path = Path.Combine(ExternalDataBridge.Context.DataPath, "StreamingAssets", "Language_" + GetLanguageFileKey(languageKey));
		if (Directory.Exists(path))
		{
			string[] files = Directory.GetFiles(path, "*.txt", SearchOption.AllDirectories);
			string[] array = files.ChangeArrType(File.ReadAllText);
			string[] array2 = files.ChangeArrType(Path.GetFileNameWithoutExtension);
			for (int i = 0; i < array.Length; i++)
			{
				string obj = array[i];
				string text = array2[i];
				string[] array3 = obj.Split('\n');
				if (text == "ui_language")
				{
					_localUiLanguageArray = array3.ChangeArrType((string e) => e.Replace("\\n", "\n"));
				}
				LanguagePackInfo item = HandleLanguagePack(text, array3);
				list.Add(item);
			}
		}
		_configLanguageMap = list.ToDictionary((LanguagePackInfo e) => e.PackName);
		CnNumberConverterInit();
	}

	public static string GetLanguageFileKey(string langId)
	{
		if (string.IsNullOrEmpty(langId) || !_languageKeyMap.TryGetValue(langId, out var value))
		{
			return "CN";
		}
		return value;
	}

	private static LanguagePackInfo HandleLanguagePack(string packName, string[] lines)
	{
		if (lines == null || lines.Length <= 0)
		{
			throw new Exception("can not handle null data for language pack " + packName);
		}
		LanguagePackInfo languagePackInfo = new LanguagePackInfo
		{
			PackName = packName,
			ArrayLanguageData = lines
		};
		bool flag = false;
		foreach (string text in lines)
		{
			if (text == ConfigArrayDicSeparator)
			{
				flag = true;
				languagePackInfo.MapLanguageData = new Dictionary<string, string>();
			}
			else
			{
				if (!flag)
				{
					continue;
				}
				int num = text.IndexOf('=');
				if (num >= 0)
				{
					string text2 = text.Substring(0, num);
					string text3 = text;
					int num2 = num + 1;
					string value = text3.Substring(num2, text3.Length - num2);
					if (languagePackInfo.MapLanguageData.ContainsKey(text2))
					{
						AdaptableLog.TagWarning("Config", packName + " has duplicate language key: " + text2 + "\nthis is not allowed!");
					}
					else
					{
						languagePackInfo.MapLanguageData.Add(text2, value);
					}
				}
			}
		}
		return languagePackInfo;
	}

	public static void Release()
	{
		_configLanguageMap.Clear();
		_configLanguageMap = null;
	}

	public static string GetConfig(string packName, int index)
	{
		if (-1 == index)
		{
			return string.Empty;
		}
		if (_configLanguageMap != null && _configLanguageMap.TryGetValue(packName, out var value) && value.ArrayLanguageData.CheckIndex(index))
		{
			return value.ArrayLanguageData[index].Replace("\\n", "\n");
		}
		return $"{packName}_{index}_invalid";
	}

	public static string GetConfig(string packName, string key)
	{
		if (_configLanguageMap == null)
		{
			return key;
		}
		if (_configLanguageMap.TryGetValue(packName, out var value) && value.MapLanguageData != null && value.MapLanguageData.ContainsKey(key))
		{
			return value.MapLanguageData[key].Replace("\\n", "\n");
		}
		return key;
	}

	public static string[] ConvertConfigList(string packName, int[] indexArray)
	{
		return indexArray?.ChangeArrType((int e) => GetConfig(packName, e));
	}

	public static string Get(string key)
	{
		if (_customLanguageHandlerOfKey != null)
		{
			string text = _customLanguageHandlerOfKey(key);
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
		}
		if (!Enum.TryParse<LanguageKey>(key, out var result))
		{
			return key;
		}
		return Get(result);
	}

	public static string GetFormat(string key, object arg0)
	{
		return Get(key).GetFormat(arg0);
	}

	public static string GetFormat(string key, object arg0, object arg1)
	{
		return Get(key).GetFormat(arg0, arg1);
	}

	public static string GetFormat(string key, object arg0, object arg1, object arg2)
	{
		return Get(key).GetFormat(arg0, arg1, arg2);
	}

	public static string GetFormat(string key, params object[] args)
	{
		return Get(key).GetFormat(args);
	}

	public static string Get(LanguageKey id)
	{
		if (_customLanguageHandlerOfId == null)
		{
			if (!_localUiLanguageArray.CheckIndex((int)id))
			{
				return $"{id} is not an available language id";
			}
			return _localUiLanguageArray[(int)id];
		}
		string text = _customLanguageHandlerOfId((ushort)id);
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		if (!_localUiLanguageArray.CheckIndex((int)id))
		{
			return $"{id} is not an available language id";
		}
		return _localUiLanguageArray[(int)id];
	}

	public static string GetFormat(LanguageKey id, object arg0)
	{
		return Get(id).GetFormat(arg0);
	}

	public static string GetFormat(LanguageKey id, object arg0, object arg1)
	{
		return Get(id).GetFormat(arg0, arg1);
	}

	public static string GetFormat(LanguageKey id, object arg0, object arg1, object arg2)
	{
		return Get(id).GetFormat(arg0, arg1, arg2);
	}

	public static string GetFormat(LanguageKey id, params object[] args)
	{
		return Get(id).GetFormat(args);
	}

	public static string GetLanguageNumber(long number)
	{
		if (!(CurLanguageKey == _languageKeyMap["schinese"]))
		{
			return number.ToString();
		}
		return CnNumberConverter(number);
	}

	public static void ReplaceStringPackForUI(string[] langArray)
	{
		if (langArray.Length != _localUiLanguageArray.Length)
		{
			AdaptableLog.Warning("wrong replace language array for ui ...");
		}
		else
		{
			_localUiLanguageArray = langArray;
		}
	}

	public static void RegisterGetLanguageCustomHandler(Func<string, string> keyHandler, Func<ushort, string> idHandler)
	{
		if (keyHandler != null)
		{
			_customLanguageHandlerOfKey = (Func<string, string>)Delegate.Remove(_customLanguageHandlerOfKey, keyHandler);
			_customLanguageHandlerOfKey = (Func<string, string>)Delegate.Combine(_customLanguageHandlerOfKey, keyHandler);
		}
		if (idHandler != null)
		{
			_customLanguageHandlerOfId = (Func<ushort, string>)Delegate.Remove(_customLanguageHandlerOfId, idHandler);
			_customLanguageHandlerOfId = (Func<ushort, string>)Delegate.Combine(_customLanguageHandlerOfId, idHandler);
		}
	}

	public static string[] GetLocalUILanguageArray()
	{
		return _localUiLanguageArray;
	}

	public static Dictionary<string, LanguagePackInfo> GetConfigLanguageMap()
	{
		return _configLanguageMap;
	}
}
