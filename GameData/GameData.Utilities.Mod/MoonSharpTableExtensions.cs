using System;
using System.Collections.Generic;
using System.Globalization;
using MoonSharp.Interpreter;

namespace GameData.Utilities.Mod;

public static class MoonSharpTableExtensions
{
	public static void Save<TK, TV>(this Table table, TK saveKey, List<TV> list)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		if (list == null || list.Count <= 0)
		{
			table.Remove((object)saveKey);
			return;
		}
		Table val = new Table((Script)null);
		for (int i = 0; i < list.Count; i++)
		{
			val.Save(i + 1, list[i]);
		}
		table.Save<TK, Table>(saveKey, val);
	}

	public static void Save<TK, TV>(this Table table, TK saveKey, TV[] array)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		if (array == null || array.Length == 0)
		{
			table.Remove((object)saveKey);
			return;
		}
		Table val = new Table((Script)null);
		for (int i = 0; i < array.Length; i++)
		{
			val.Save(i + 1, array[i]);
		}
		table.Save<TK, Table>(saveKey, val);
	}

	public static void Save<TK, TV>(this Table table, TK saveKey, HashSet<TV> hashset)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		if (hashset == null || hashset.Count <= 0)
		{
			table.Remove((object)saveKey);
			return;
		}
		Table val = new Table((Script)null);
		int num = 1;
		foreach (TV item in hashset)
		{
			val.Save(num, item);
			num++;
		}
		table.Save<TK, Table>(saveKey, val);
	}

	public static void Save<TK, TV1, TV2>(this Table table, TK saveKey, (TV1, TV2)[] array)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		if (array == null || array.Length == 0)
		{
			table.Remove((object)saveKey);
			return;
		}
		Table val = new Table((Script)null);
		for (int i = 0; i < array.Length; i++)
		{
			(TV1, TV2) tuple = array[i];
			Table val2 = new Table((Script)null);
			val2.Save(1, tuple.Item1);
			val2.Save(2, tuple.Item2);
			val.Save<int, Table>(i + 1, val2);
		}
		table.Save<TK, Table>(saveKey, val);
	}

	public static void Save<TK, TV1, TV2>(this Table table, TK saveKey, List<(TV1, TV2)> list)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		if (list == null || list.Count <= 0)
		{
			table.Remove((object)saveKey);
			return;
		}
		Table val = new Table((Script)null);
		for (int i = 0; i < list.Count; i++)
		{
			(TV1, TV2) tuple = list[i];
			Table val2 = new Table((Script)null);
			val2.Save(1, tuple.Item1);
			val2.Save(2, tuple.Item2);
			val.Save<int, Table>(i + 1, val2);
		}
		table.Save<TK, Table>(saveKey, val);
	}

	public static void Save<TK, TV1, TV2, TV3>(this Table table, TK saveKey, List<(TV1, TV2, TV3)> list)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		if (list == null || list.Count <= 0)
		{
			table.Remove((object)saveKey);
			return;
		}
		Table val = new Table((Script)null);
		for (int i = 0; i < list.Count; i++)
		{
			(TV1, TV2, TV3) tuple = list[i];
			Table val2 = new Table((Script)null);
			val2.Save(1, tuple.Item1);
			val2.Save(2, tuple.Item2);
			val2.Save(3, tuple.Item3);
			val.Save<int, Table>(i + 1, val2);
		}
		table.Save<TK, Table>(saveKey, val);
	}

	public static void Save<TK, TV1, TV2, TV3, TV4>(this Table table, TK saveKey, List<(TV1, TV2, TV3, TV4)> list)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		if (list == null || list.Count <= 0)
		{
			table.Remove((object)saveKey);
			return;
		}
		Table val = new Table((Script)null);
		for (int i = 0; i < list.Count; i++)
		{
			(TV1, TV2, TV3, TV4) tuple = list[i];
			Table val2 = new Table((Script)null);
			val2.Save(1, tuple.Item1);
			val2.Save(2, tuple.Item2);
			val2.Save(3, tuple.Item3);
			val2.Save(4, tuple.Item4);
			val.Save<int, Table>(i + 1, val2);
		}
		table.Save<TK, Table>(saveKey, val);
	}

	public static void Save<TK, TV>(this Table table, TK saveKey, TV obj)
	{
		DynValue val = DynValue.FromObject((Script)null, (object)obj);
		table.Set(DynValue.FromObject(table.OwnerScript, (object)saveKey), val);
	}

	public static bool LoadObject<TK>(this Table table, TK saveKey, Type type, out object obj)
	{
		if (type.IsEnum)
		{
			obj = Enum.Parse(type, table.Get(DynValue.FromObject(table.OwnerScript, (object)saveKey)).String);
			return true;
		}
		try
		{
			obj = table.Get(DynValue.FromObject(table.OwnerScript, (object)saveKey)).ToObject(type);
			return true;
		}
		catch (Exception)
		{
			obj = null;
			AdaptableLog.Warning($"{saveKey}: object type {type.Name} is not yet supported.");
			return false;
		}
	}

	public static void Load<TK>(this Table table, TK key, out Table val)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		DynValue val2 = table.Get(DynValue.FromObject(table.OwnerScript, (object)key));
		if ((int)val2.Type == 6)
		{
			val = val2.Table;
		}
		else
		{
			val = null;
		}
	}

	public static void Load<TK, TV>(this Table table, TK key, out TV val)
	{
		DynValue val2 = table.Get(DynValue.FromObject(table.OwnerScript, (object)key));
		if (val2.IsNil())
		{
			val = default(TV);
		}
		else
		{
			val = val2.ToObject<TV>();
		}
	}

	public static void Load<TK, TV1, TV2>(this Table table, TK key, out List<(TV1, TV2)> list)
	{
		if (!table.ContainsKey(key))
		{
			list = new List<(TV1, TV2)>();
			return;
		}
		Table table2 = table.Get(DynValue.FromObject(table.OwnerScript, (object)key)).Table;
		list = new List<(TV1, TV2)>(table2.Length);
		for (int i = 0; i < table2.Length; i++)
		{
			table2.Load(i + 1, out var val);
			val.Load(1, out TV1 val2);
			val.Load(2, out TV2 val3);
			list.Add((val2, val3));
		}
	}

	public static void Load<TK, TV1, TV2, TV3>(this Table table, TK saveKey, out List<(TV1, TV2, TV3)> list)
	{
		if (!table.ContainsKey(saveKey))
		{
			list = new List<(TV1, TV2, TV3)>();
			return;
		}
		Table table2 = table.Get(DynValue.FromObject(table.OwnerScript, (object)saveKey)).Table;
		list = new List<(TV1, TV2, TV3)>(table2.Length);
		for (int i = 0; i < table2.Length; i++)
		{
			table2.Load(i + 1, out var val);
			val.Load(1, out TV1 val2);
			val.Load(2, out TV2 val3);
			val.Load(3, out TV3 val4);
			list.Add((val2, val3, val4));
		}
	}

	public static void Load<TK, TV1, TV2, TV3, TV4>(this Table table, TK saveKey, out List<(TV1, TV2, TV3, TV4)> list)
	{
		if (!table.ContainsKey(saveKey))
		{
			list = new List<(TV1, TV2, TV3, TV4)>();
			return;
		}
		Table table2 = table.Get(DynValue.FromObject(table.OwnerScript, (object)saveKey)).Table;
		list = new List<(TV1, TV2, TV3, TV4)>(table2.Length);
		for (int i = 0; i < table2.Length; i++)
		{
			table2.Load(i + 1, out var val);
			val.Load(1, out TV1 val2);
			val.Load(2, out TV2 val3);
			val.Load(3, out TV3 val4);
			val.Load(4, out TV4 val5);
			list.Add((val2, val3, val4, val5));
		}
	}

	public static void Load<TK, TV>(this Table table, TK key, out TV[] array)
	{
		if (!table.ContainsKey(key))
		{
			array = Array.Empty<TV>();
			return;
		}
		table.Load(key, out var val);
		array = new TV[val.Length];
		for (int i = 0; i < val.Length; i++)
		{
			val.Load(i + 1, out array[i]);
		}
	}

	public static void Load<TK, TV1, TV2>(this Table table, TK key, out (TV1, TV2)[] array)
	{
		if (!table.ContainsKey(key))
		{
			array = Array.Empty<(TV1, TV2)>();
			return;
		}
		table.Load(key, out var val);
		array = new(TV1, TV2)[val.Length];
		for (int i = 0; i < val.Length; i++)
		{
			val.Load(i + 1, out var val2);
			val2.Load(1, out TV1 val3);
			val2.Load(2, out TV2 val4);
			array[i] = (val3, val4);
		}
	}

	public static void Load<TK, TV>(this Table table, TK key, out List<TV> list)
	{
		if (!table.ContainsKey(key))
		{
			list = new List<TV>();
			return;
		}
		table.Load(key, out var val);
		list = new List<TV>(val.Length);
		for (int i = 0; i < val.Length; i++)
		{
			val.Load(i + 1, out TV val2);
			list.Add(val2);
		}
	}

	public static bool ContainsKey<TK>(this Table table, TK key)
	{
		return !((object)DynValue.Nil).Equals((object?)table.Get((object)key));
	}

	public static void Get<TKey, TValue>(this Table table, TKey key, out TValue value)
	{
		object obj = table.Get(DynValue.FromObject(table.OwnerScript, (object)key)).ToObject();
		value = (TValue)((obj is IConvertible convertible) ? convertible.ToType(typeof(TValue), CultureInfo.InvariantCulture) : obj);
	}

	public static TValue Get<TKey, TValue>(this Table table, TKey key)
	{
		table.Get<TKey, TValue>(key, out var value);
		return value;
	}

	public static TValue Get<TValue>(this Table table, string key)
	{
		table.Get<string, TValue>(key, out var value);
		return value;
	}

	public static TValue GetOrDefault<TKey, TValue>(this Table table, TKey key, TValue defaultValue)
	{
		if (table.ContainsKey(key))
		{
			table.Get<TKey, TValue>(key, out var value);
			return value;
		}
		return defaultValue;
	}

	public static void Set<TKey, TValue>(this Table table, TKey key, TValue value)
	{
		table.Set(DynValue.FromObject(table.OwnerScript, (object)key), DynValue.FromObject(table.OwnerScript, (object)value));
	}

	private static IEnumerable<DynValue> GetRawKeys<TKey>(this Table table)
	{
		foreach (DynValue key in table.Keys)
		{
			if (CanTranslate<TKey>(key.ToObject()))
			{
				yield return key;
			}
		}
	}

	public static IEnumerable<TKey> GetKeys<TKey>(this Table table)
	{
		foreach (DynValue key in table.GetRawKeys<TKey>())
		{
			yield return key.ToObject<TKey>();
		}
	}

	public static void SetInPath<TV>(this Table table, string path, TV val)
	{
		string[] array = path.Split('.');
		Table val2 = table;
		for (int i = 0; i < array.Length - 1; i++)
		{
			string text = array[i];
			if (!(val2[(object)text] is Table))
			{
				val2[(object)text] = DynValue.NewTable(table.OwnerScript);
			}
			val2 = val2.Get(text).Table;
		}
		val2.Set(array[^1], val);
	}

	public static TV GetInPath<TV>(this Table root, string path)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Invalid comparison between Unknown and I4
		if (root == null)
		{
			throw new ArgumentNullException("root");
		}
		if (string.IsNullOrEmpty(path))
		{
			throw new ArgumentException("path is null or empty");
		}
		string[] array = path.Split('.');
		DynValue val = DynValue.NewTable(root);
		string[] array2 = array;
		foreach (string text in array2)
		{
			if ((int)val.Type != 6)
			{
				return default(TV);
			}
			val = val.Table.Get(text);
			if (val.IsNil())
			{
				return default(TV);
			}
		}
		try
		{
			return val.ToObject<TV>();
		}
		catch (InvalidCastException)
		{
			return default(TV);
		}
	}

	public static void ForEach<TKey, TValue>(this Table table, Action<TKey, TValue> action)
	{
		TValue arg = default(TValue);
		foreach (DynValue rawKey in table.GetRawKeys<TKey>())
		{
			if (CanTranslate<TKey>(rawKey.ToObject()))
			{
				DynValue val = table.Get(rawKey);
				if (CanTranslate<TValue>(val.ToObject()))
				{
					action(rawKey.ToObject<TKey>(), val.ToObject<TValue>());
					continue;
				}
				AdaptableLog.TagWarning("MoonSharpTableExtensions", $"{val} is not compatible with {typeof(TValue)}, using default value");
				action(rawKey.ToObject<TKey>(), arg);
			}
		}
	}

	private static bool CanTranslate<TP>(object val)
	{
		if (val != null)
		{
			DynValue val2 = (DynValue)((val is DynValue) ? val : null);
			if (val2 == null || !val2.IsNil())
			{
				Type typeFromHandle = typeof(TP);
				if (val.GetType().IsValueType && val is IConvertible convertible)
				{
					val = convertible.ToType(typeFromHandle, CultureInfo.InvariantCulture);
				}
				if (!typeFromHandle.IsAssignableFrom(val.GetType()))
				{
					return false;
				}
				return true;
			}
		}
		return false;
	}

	public static Table NewTable(this Script env)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		return new Table(env);
	}
}
