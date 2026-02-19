using System;
using System.Collections.Generic;
using System.Text;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Mod;

public class SerializableModData : ISerializableGameData
{
	[SerializableGameDataField]
	private readonly Dictionary<string, int> _intValues;

	[SerializableGameDataField]
	private readonly Dictionary<string, float> _floatValues;

	[SerializableGameDataField]
	private readonly Dictionary<string, bool> _boolValues;

	[SerializableGameDataField]
	private readonly Dictionary<string, string> _stringValues;

	private readonly Dictionary<Type, Dictionary<string, ISerializableGameData>> _serializableGameDataValues;

	public SerializableModData()
	{
		_intValues = new Dictionary<string, int>();
		_floatValues = new Dictionary<string, float>();
		_boolValues = new Dictionary<string, bool>();
		_stringValues = new Dictionary<string, string>();
		_serializableGameDataValues = new Dictionary<Type, Dictionary<string, ISerializableGameData>>();
	}

	private unsafe string ReadString(ref byte* pData)
	{
		ushort num = *(ushort*)pData;
		pData += 2;
		if (num > 0)
		{
			int num2 = 2 * num;
			string result = Encoding.Unicode.GetString(pData, num2);
			pData += num2;
			return result;
		}
		return string.Empty;
	}

	private unsafe int WriteString(byte* pData, string target)
	{
		byte* ptr = pData;
		if (!string.IsNullOrEmpty(target))
		{
			int length = target.Length;
			Tester.Assert(length <= 65535);
			*(ushort*)ptr = (ushort)length;
			ptr += 2;
			fixed (char* ptr2 = target)
			{
				for (int i = 0; i < length; i++)
				{
					((short*)ptr)[i] = (short)ptr2[i];
				}
			}
			ptr += 2 * length;
		}
		else
		{
			*(short*)pData = 0;
			ptr += 2;
		}
		return (int)(ptr - pData);
	}

	public bool ContainsKey(string key)
	{
		if (_intValues.ContainsKey(key) || _boolValues.ContainsKey(key) || _stringValues.ContainsKey(key) || _floatValues.ContainsKey(key))
		{
			return true;
		}
		foreach (KeyValuePair<Type, Dictionary<string, ISerializableGameData>> serializableGameDataValue in _serializableGameDataValues)
		{
			if (serializableGameDataValue.Value.ContainsKey(key))
			{
				return true;
			}
		}
		return false;
	}

	public bool Get(string key, out int val)
	{
		return _intValues.TryGetValue(key, out val);
	}

	public bool Get(string key, out float val)
	{
		return _floatValues.TryGetValue(key, out val);
	}

	public bool Get(string key, out bool val)
	{
		return _boolValues.TryGetValue(key, out val);
	}

	public bool Get(string key, out string val)
	{
		return _stringValues.TryGetValue(key, out val);
	}

	public bool Get<T>(string key, out T serializableGameData) where T : ISerializableGameData
	{
		if (_serializableGameDataValues.TryGetValue(typeof(T), out var value) && value.TryGetValue(key, out var value2))
		{
			serializableGameData = (T)(object)value2;
			return true;
		}
		serializableGameData = default(T);
		return false;
	}

	public void Set(string key, int val)
	{
		_intValues[key] = val;
	}

	public void Set(string key, float val)
	{
		_floatValues[key] = val;
	}

	public void Set(string key, bool val)
	{
		_boolValues[key] = val;
	}

	public void Set(string key, string val)
	{
		_stringValues[key] = val;
	}

	public void Set<T>(string key, T serializableGameData) where T : ISerializableGameData
	{
		Type typeFromHandle = typeof(T);
		if (!_serializableGameDataValues.TryGetValue(typeFromHandle, out var value))
		{
			value = new Dictionary<string, ISerializableGameData>();
			_serializableGameDataValues.Add(typeFromHandle, value);
		}
		value[key] = (ISerializableGameData)(object)serializableGameData;
	}

	public void Remove(string key)
	{
		_intValues.Remove(key);
		_floatValues.Remove(key);
		_stringValues.Remove(key);
		_boolValues.Remove(key);
		foreach (KeyValuePair<Type, Dictionary<string, ISerializableGameData>> serializableGameDataValue in _serializableGameDataValues)
		{
			serializableGameDataValue.Value.Remove(key);
		}
	}

	public bool RemoveInt(string key)
	{
		return _intValues.Remove(key);
	}

	public bool RemoveFloat(string key)
	{
		return _floatValues.Remove(key);
	}

	public bool RemoveString(string key)
	{
		return _stringValues.Remove(key);
	}

	public bool RemoveBool(string key)
	{
		return _boolValues.Remove(key);
	}

	public bool RemoveObject(string key)
	{
		bool flag = false;
		foreach (KeyValuePair<Type, Dictionary<string, ISerializableGameData>> serializableGameDataValue in _serializableGameDataValues)
		{
			flag = flag || serializableGameDataValue.Value.Remove(key);
		}
		return flag;
	}

	public void Clear()
	{
		_intValues.Clear();
		_floatValues.Clear();
		_boolValues.Clear();
		_stringValues.Clear();
		_serializableGameDataValues.Clear();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 10;
		foreach (KeyValuePair<string, int> intValue in _intValues)
		{
			num += 2 + 2 * intValue.Key.Length + 4;
		}
		foreach (KeyValuePair<string, float> floatValue in _floatValues)
		{
			num += 2 + 2 * floatValue.Key.Length + 4;
		}
		foreach (KeyValuePair<string, bool> boolValue in _boolValues)
		{
			num += 2 + 2 * boolValue.Key.Length + 1;
		}
		foreach (KeyValuePair<string, string> stringValue in _stringValues)
		{
			num += 2 + 2 * stringValue.Key.Length + 2 + 2 * stringValue.Value.Length;
		}
		foreach (KeyValuePair<Type, Dictionary<string, ISerializableGameData>> serializableGameDataValue in _serializableGameDataValues)
		{
			num += 2 + 2 * serializableGameDataValue.Key.FullName.Length + 2;
			foreach (KeyValuePair<string, ISerializableGameData> item in serializableGameDataValue.Value)
			{
				num += 2 + 2 * item.Key.Length + item.Value.GetSerializedSize();
			}
		}
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(ushort*)ptr = (ushort)_intValues.Count;
		ptr += 2;
		foreach (KeyValuePair<string, int> intValue in _intValues)
		{
			ptr += WriteString(ptr, intValue.Key);
			*(int*)ptr = intValue.Value;
			ptr += 4;
		}
		*(ushort*)ptr = (ushort)_floatValues.Count;
		ptr += 2;
		foreach (KeyValuePair<string, float> floatValue in _floatValues)
		{
			ptr += WriteString(ptr, floatValue.Key);
			*(float*)ptr = floatValue.Value;
			ptr += 4;
		}
		*(ushort*)ptr = (ushort)_boolValues.Count;
		ptr += 2;
		foreach (KeyValuePair<string, bool> boolValue in _boolValues)
		{
			ptr += WriteString(ptr, boolValue.Key);
			*ptr = (boolValue.Value ? ((byte)1) : ((byte)0));
			ptr++;
		}
		*(ushort*)ptr = (ushort)_stringValues.Count;
		ptr += 2;
		foreach (KeyValuePair<string, string> stringValue in _stringValues)
		{
			ptr += WriteString(ptr, stringValue.Key);
			ptr += WriteString(ptr, stringValue.Value);
		}
		*(ushort*)ptr = (ushort)_serializableGameDataValues.Count;
		ptr += 2;
		foreach (KeyValuePair<Type, Dictionary<string, ISerializableGameData>> serializableGameDataValue in _serializableGameDataValues)
		{
			ptr += WriteString(ptr, serializableGameDataValue.Key.FullName);
			*(ushort*)ptr = (ushort)serializableGameDataValue.Value.Count;
			ptr += 2;
			foreach (KeyValuePair<string, ISerializableGameData> item in serializableGameDataValue.Value)
			{
				ptr += WriteString(ptr, item.Key);
				ptr += item.Value.Serialize(ptr);
			}
		}
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Deserialize(byte* pData)
	{
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		_intValues.Clear();
		for (int i = 0; i < num; i++)
		{
			string key = ReadString(ref ptr);
			int value = *(int*)ptr;
			ptr += 4;
			_intValues.Add(key, value);
		}
		num = *(ushort*)ptr;
		ptr += 2;
		_floatValues.Clear();
		for (int j = 0; j < num; j++)
		{
			string key2 = ReadString(ref ptr);
			float value2 = *(float*)ptr;
			ptr += 4;
			_floatValues.Add(key2, value2);
		}
		num = *(ushort*)ptr;
		ptr += 2;
		_boolValues.Clear();
		for (int k = 0; k < num; k++)
		{
			string key3 = ReadString(ref ptr);
			bool value3 = *ptr != 0;
			ptr++;
			_boolValues.Add(key3, value3);
		}
		num = *(ushort*)ptr;
		ptr += 2;
		_stringValues.Clear();
		for (int l = 0; l < num; l++)
		{
			string key4 = ReadString(ref ptr);
			string value4 = ReadString(ref ptr);
			_stringValues.Add(key4, value4);
		}
		num = *(ushort*)ptr;
		ptr += 2;
		_serializableGameDataValues.Clear();
		for (int m = 0; m < num; m++)
		{
			string text = ReadString(ref ptr);
			Type type = Type.GetType(text);
			if (type == null)
			{
				throw new Exception("Can't find type " + text + " in any of the loaded assemblies.");
			}
			Dictionary<string, ISerializableGameData> dictionary = new Dictionary<string, ISerializableGameData>();
			_serializableGameDataValues.Add(type, dictionary);
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			for (int n = 0; n < num2; n++)
			{
				string key5 = ReadString(ref ptr);
				ISerializableGameData val = (ISerializableGameData)Activator.CreateInstance(type);
				if (val == null)
				{
					throw new Exception("Can't parse type " + text + " to ISerializableGameData.");
				}
				ptr += val.Deserialize(ptr);
				dictionary.Add(key5, val);
			}
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
