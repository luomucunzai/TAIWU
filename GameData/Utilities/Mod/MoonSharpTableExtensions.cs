using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using MoonSharp.Interpreter;

namespace GameData.Utilities.Mod
{
	// Token: 0x02000019 RID: 25
	public static class MoonSharpTableExtensions
	{
		// Token: 0x0600008E RID: 142 RVA: 0x00050F8C File Offset: 0x0004F18C
		public static void Save<TK, TV>(this Table table, TK saveKey, List<TV> list)
		{
			bool flag = list == null || list.Count <= 0;
			if (flag)
			{
				table.Remove(saveKey);
			}
			else
			{
				Table arrayTable = new Table(null);
				for (int i = 0; i < list.Count; i++)
				{
					arrayTable.Save(i + 1, list[i]);
				}
				table.Save(saveKey, arrayTable);
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00050FF8 File Offset: 0x0004F1F8
		public static void Save<TK, TV>(this Table table, TK saveKey, TV[] array)
		{
			bool flag = array == null || array.Length == 0;
			if (flag)
			{
				table.Remove(saveKey);
			}
			else
			{
				Table arrayTable = new Table(null);
				for (int i = 0; i < array.Length; i++)
				{
					arrayTable.Save(i + 1, array[i]);
				}
				table.Save(saveKey, arrayTable);
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x0005105C File Offset: 0x0004F25C
		public static void Save<TK, TV>(this Table table, TK saveKey, HashSet<TV> hashset)
		{
			bool flag = hashset == null || hashset.Count <= 0;
			if (flag)
			{
				table.Remove(saveKey);
			}
			else
			{
				Table arrayTable = new Table(null);
				int i = 1;
				foreach (TV element in hashset)
				{
					arrayTable.Save(i, element);
					i++;
				}
				table.Save(saveKey, arrayTable);
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000510F0 File Offset: 0x0004F2F0
		public static void Save<TK, TV1, TV2>(this Table table, TK saveKey, ValueTuple<TV1, TV2>[] array)
		{
			bool flag = array == null || array.Length == 0;
			if (flag)
			{
				table.Remove(saveKey);
			}
			else
			{
				Table arrayTable = new Table(null);
				for (int i = 0; i < array.Length; i++)
				{
					ValueTuple<TV1, TV2> tuple = array[i];
					Table cellTable = new Table(null);
					cellTable.Save(1, tuple.Item1);
					cellTable.Save(2, tuple.Item2);
					arrayTable.Save(i + 1, cellTable);
				}
				table.Save(saveKey, arrayTable);
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00051180 File Offset: 0x0004F380
		public static void Save<TK, TV1, TV2>(this Table table, TK saveKey, List<ValueTuple<TV1, TV2>> list)
		{
			bool flag = list == null || list.Count <= 0;
			if (flag)
			{
				table.Remove(saveKey);
			}
			else
			{
				Table arrayTable = new Table(null);
				for (int i = 0; i < list.Count; i++)
				{
					ValueTuple<TV1, TV2> tuple = list[i];
					Table cellTable = new Table(null);
					cellTable.Save(1, tuple.Item1);
					cellTable.Save(2, tuple.Item2);
					arrayTable.Save(i + 1, cellTable);
				}
				table.Save(saveKey, arrayTable);
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00051218 File Offset: 0x0004F418
		public static void Save<TK, TV1, TV2, TV3>(this Table table, TK saveKey, List<ValueTuple<TV1, TV2, TV3>> list)
		{
			bool flag = list == null || list.Count <= 0;
			if (flag)
			{
				table.Remove(saveKey);
			}
			else
			{
				Table arrayTable = new Table(null);
				for (int i = 0; i < list.Count; i++)
				{
					ValueTuple<TV1, TV2, TV3> tuple = list[i];
					Table cellTable = new Table(null);
					cellTable.Save(1, tuple.Item1);
					cellTable.Save(2, tuple.Item2);
					cellTable.Save(3, tuple.Item3);
					arrayTable.Save(i + 1, cellTable);
				}
				table.Save(saveKey, arrayTable);
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000512C0 File Offset: 0x0004F4C0
		public static void Save<TK, TV1, TV2, TV3, TV4>(this Table table, TK saveKey, List<ValueTuple<TV1, TV2, TV3, TV4>> list)
		{
			bool flag = list == null || list.Count <= 0;
			if (flag)
			{
				table.Remove(saveKey);
			}
			else
			{
				Table arrayTable = new Table(null);
				for (int i = 0; i < list.Count; i++)
				{
					ValueTuple<TV1, TV2, TV3, TV4> tuple = list[i];
					Table cellTable = new Table(null);
					cellTable.Save(1, tuple.Item1);
					cellTable.Save(2, tuple.Item2);
					cellTable.Save(3, tuple.Item3);
					cellTable.Save(4, tuple.Item4);
					arrayTable.Save(i + 1, cellTable);
				}
				table.Save(saveKey, arrayTable);
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00051378 File Offset: 0x0004F578
		public static void Save<TK, TV>(this Table table, TK saveKey, TV obj)
		{
			DynValue dynVal = DynValue.FromObject(null, obj);
			table.Set(DynValue.FromObject(table.OwnerScript, saveKey), dynVal);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000513AC File Offset: 0x0004F5AC
		public static bool LoadObject<TK>(this Table table, TK saveKey, Type type, out object obj)
		{
			bool isEnum = type.IsEnum;
			bool result;
			if (isEnum)
			{
				obj = Enum.Parse(type, table.Get(DynValue.FromObject(table.OwnerScript, saveKey)).String);
				result = true;
			}
			else
			{
				try
				{
					obj = table.Get(DynValue.FromObject(table.OwnerScript, saveKey)).ToObject(type);
					result = true;
				}
				catch (Exception)
				{
					obj = null;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 2);
					defaultInterpolatedStringHandler.AppendFormatted<TK>(saveKey);
					defaultInterpolatedStringHandler.AppendLiteral(": object type ");
					defaultInterpolatedStringHandler.AppendFormatted(type.Name);
					defaultInterpolatedStringHandler.AppendLiteral(" is not yet supported.");
					AdaptableLog.Warning(defaultInterpolatedStringHandler.ToStringAndClear(), false);
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00051478 File Offset: 0x0004F678
		public static void Load<TK>(this Table table, TK key, out Table val)
		{
			DynValue dynVal = table.Get(DynValue.FromObject(table.OwnerScript, key));
			bool flag = dynVal.Type == DataType.Table;
			if (flag)
			{
				val = dynVal.Table;
			}
			else
			{
				val = null;
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000514B8 File Offset: 0x0004F6B8
		public static void Load<TK, TV>(this Table table, TK key, out TV val)
		{
			DynValue dynVal = table.Get(DynValue.FromObject(table.OwnerScript, key));
			bool flag = dynVal.IsNil();
			if (flag)
			{
				val = default(TV);
			}
			else
			{
				val = dynVal.ToObject<TV>();
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00051500 File Offset: 0x0004F700
		public static void Load<TK, TV1, TV2>(this Table table, TK key, out List<ValueTuple<TV1, TV2>> list)
		{
			bool flag = !table.ContainsKey(key);
			if (flag)
			{
				list = new List<ValueTuple<TV1, TV2>>();
			}
			else
			{
				Table arrayTable = table.Get(DynValue.FromObject(table.OwnerScript, key)).Table;
				list = new List<ValueTuple<TV1, TV2>>(arrayTable.Length);
				for (int i = 0; i < arrayTable.Length; i++)
				{
					Table cellTable;
					arrayTable.Load(i + 1, out cellTable);
					TV1 t;
					cellTable.Load(1, out t);
					TV2 t2;
					cellTable.Load(2, out t2);
					list.Add(new ValueTuple<TV1, TV2>(t, t2));
				}
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0005159C File Offset: 0x0004F79C
		public static void Load<TK, TV1, TV2, TV3>(this Table table, TK saveKey, out List<ValueTuple<TV1, TV2, TV3>> list)
		{
			bool flag = !table.ContainsKey(saveKey);
			if (flag)
			{
				list = new List<ValueTuple<TV1, TV2, TV3>>();
			}
			else
			{
				Table arrayTable = table.Get(DynValue.FromObject(table.OwnerScript, saveKey)).Table;
				list = new List<ValueTuple<TV1, TV2, TV3>>(arrayTable.Length);
				for (int i = 0; i < arrayTable.Length; i++)
				{
					Table cellTable;
					arrayTable.Load(i + 1, out cellTable);
					TV1 t;
					cellTable.Load(1, out t);
					TV2 t2;
					cellTable.Load(2, out t2);
					TV3 t3;
					cellTable.Load(3, out t3);
					list.Add(new ValueTuple<TV1, TV2, TV3>(t, t2, t3));
				}
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00051648 File Offset: 0x0004F848
		public static void Load<TK, TV1, TV2, TV3, TV4>(this Table table, TK saveKey, out List<ValueTuple<TV1, TV2, TV3, TV4>> list)
		{
			bool flag = !table.ContainsKey(saveKey);
			if (flag)
			{
				list = new List<ValueTuple<TV1, TV2, TV3, TV4>>();
			}
			else
			{
				Table arrayTable = table.Get(DynValue.FromObject(table.OwnerScript, saveKey)).Table;
				list = new List<ValueTuple<TV1, TV2, TV3, TV4>>(arrayTable.Length);
				for (int i = 0; i < arrayTable.Length; i++)
				{
					Table cellTable;
					arrayTable.Load(i + 1, out cellTable);
					TV1 t;
					cellTable.Load(1, out t);
					TV2 t2;
					cellTable.Load(2, out t2);
					TV3 t3;
					cellTable.Load(3, out t3);
					TV4 t4;
					cellTable.Load(4, out t4);
					list.Add(new ValueTuple<TV1, TV2, TV3, TV4>(t, t2, t3, t4));
				}
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00051700 File Offset: 0x0004F900
		public static void Load<TK, TV>(this Table table, TK key, out TV[] array)
		{
			bool flag = !table.ContainsKey(key);
			if (flag)
			{
				array = Array.Empty<TV>();
			}
			else
			{
				Table arrayTable;
				table.Load(key, out arrayTable);
				array = new TV[arrayTable.Length];
				for (int i = 0; i < arrayTable.Length; i++)
				{
					arrayTable.Load(i + 1, out array[i]);
				}
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00051768 File Offset: 0x0004F968
		public static void Load<TK, TV1, TV2>(this Table table, TK key, out ValueTuple<TV1, TV2>[] array)
		{
			bool flag = !table.ContainsKey(key);
			if (flag)
			{
				array = Array.Empty<ValueTuple<TV1, TV2>>();
			}
			else
			{
				Table arrayTable;
				table.Load(key, out arrayTable);
				array = new ValueTuple<TV1, TV2>[arrayTable.Length];
				for (int i = 0; i < arrayTable.Length; i++)
				{
					Table cellTable;
					arrayTable.Load(i + 1, out cellTable);
					TV1 t;
					cellTable.Load(1, out t);
					TV2 t2;
					cellTable.Load(2, out t2);
					array[i] = new ValueTuple<TV1, TV2>(t, t2);
				}
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000517F0 File Offset: 0x0004F9F0
		public static void Load<TK, TV>(this Table table, TK key, out List<TV> list)
		{
			bool flag = !table.ContainsKey(key);
			if (flag)
			{
				list = new List<TV>();
			}
			else
			{
				Table arrayTable;
				table.Load(key, out arrayTable);
				list = new List<TV>(arrayTable.Length);
				for (int i = 0; i < arrayTable.Length; i++)
				{
					TV val;
					arrayTable.Load(i + 1, out val);
					list.Add(val);
				}
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x0005185C File Offset: 0x0004FA5C
		public static bool ContainsKey<TK>(this Table table, TK key)
		{
			return !DynValue.Nil.Equals(table.Get(key));
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00051888 File Offset: 0x0004FA88
		public static void Get<TKey, TValue>(this Table table, TKey key, out TValue value)
		{
			object raw = table.Get(DynValue.FromObject(table.OwnerScript, key)).ToObject();
			IConvertible rawConv = raw as IConvertible;
			value = (TValue)((object)((rawConv != null) ? rawConv.ToType(typeof(TValue), CultureInfo.InvariantCulture) : raw));
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000518E0 File Offset: 0x0004FAE0
		public static TValue Get<TKey, TValue>(this Table table, TKey key)
		{
			TValue value;
			table.Get(key, out value);
			return value;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00051900 File Offset: 0x0004FB00
		public static TValue Get<TValue>(this Table table, string key)
		{
			TValue value;
			table.Get(key, out value);
			return value;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00051920 File Offset: 0x0004FB20
		public static TValue GetOrDefault<TKey, TValue>(this Table table, TKey key, TValue defaultValue)
		{
			bool flag = table.ContainsKey(key);
			TValue value;
			if (flag)
			{
				table.Get(key, out value);
			}
			else
			{
				value = defaultValue;
			}
			return value;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0005194C File Offset: 0x0004FB4C
		public static void Set<TKey, TValue>(this Table table, TKey key, TValue value)
		{
			table.Set(DynValue.FromObject(table.OwnerScript, key), DynValue.FromObject(table.OwnerScript, value));
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00051978 File Offset: 0x0004FB78
		private static IEnumerable<DynValue> GetRawKeys<TKey>(this Table table)
		{
			foreach (DynValue key in table.Keys)
			{
				bool flag = !MoonSharpTableExtensions.CanTranslate<TKey>(key.ToObject());
				if (!flag)
				{
					yield return key;
					key = null;
				}
			}
			IEnumerator<DynValue> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00051988 File Offset: 0x0004FB88
		public static IEnumerable<TKey> GetKeys<TKey>(this Table table)
		{
			foreach (DynValue key in table.GetRawKeys<TKey>())
			{
				yield return key.ToObject<TKey>();
				key = null;
			}
			IEnumerator<DynValue> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00051998 File Offset: 0x0004FB98
		public static void SetInPath<TV>(this Table table, string path, TV val)
		{
			string[] segments = path.Split('.', StringSplitOptions.None);
			Table cur = table;
			for (int i = 0; i < segments.Length - 1; i++)
			{
				string seg = segments[i];
				bool flag = !(cur[seg] is Table);
				if (flag)
				{
					cur[seg] = DynValue.NewTable(table.OwnerScript);
				}
				cur = cur.Get(seg).Table;
			}
			Table table2 = cur;
			string[] array = segments;
			table2.Set(array[array.Length - 1], val);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00051A18 File Offset: 0x0004FC18
		public static TV GetInPath<TV>(this Table root, string path)
		{
			bool flag = root == null;
			if (flag)
			{
				throw new ArgumentNullException("root");
			}
			bool flag2 = string.IsNullOrEmpty(path);
			if (flag2)
			{
				throw new ArgumentException("path is null or empty");
			}
			string[] segments = path.Split('.', StringSplitOptions.None);
			DynValue cur = DynValue.NewTable(root);
			string[] array = segments;
			int i = 0;
			TV result;
			while (i < array.Length)
			{
				string seg = array[i];
				bool flag3 = cur.Type != DataType.Table;
				if (flag3)
				{
					result = default(TV);
				}
				else
				{
					cur = cur.Table.Get(seg);
					bool flag4 = cur.IsNil();
					if (!flag4)
					{
						i++;
						continue;
					}
					result = default(TV);
				}
				return result;
			}
			try
			{
				result = cur.ToObject<TV>();
			}
			catch (InvalidCastException)
			{
				result = default(TV);
			}
			return result;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00051AF8 File Offset: 0x0004FCF8
		public static void ForEach<TKey, TValue>(this Table table, Action<TKey, TValue> action)
		{
			TValue defaultValue = default(TValue);
			foreach (DynValue key in table.GetRawKeys<TKey>())
			{
				bool flag = !MoonSharpTableExtensions.CanTranslate<TKey>(key.ToObject());
				if (!flag)
				{
					DynValue value = table.Get(key);
					bool flag2 = MoonSharpTableExtensions.CanTranslate<TValue>(value.ToObject());
					if (flag2)
					{
						action(key.ToObject<TKey>(), value.ToObject<TValue>());
					}
					else
					{
						string tag = "MoonSharpTableExtensions";
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 2);
						defaultInterpolatedStringHandler.AppendFormatted<DynValue>(value);
						defaultInterpolatedStringHandler.AppendLiteral(" is not compatible with ");
						defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(TValue));
						defaultInterpolatedStringHandler.AppendLiteral(", using default value");
						AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
						action(key.ToObject<TKey>(), defaultValue);
					}
				}
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00051BFC File Offset: 0x0004FDFC
		private static bool CanTranslate<TP>(object val)
		{
			bool flag;
			if (val != null)
			{
				DynValue valDynValue = val as DynValue;
				flag = (valDynValue != null && valDynValue.IsNil());
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			bool result;
			if (flag2)
			{
				result = false;
			}
			else
			{
				Type typeTp = typeof(TP);
				IConvertible valConvertible;
				bool flag3;
				if (val.GetType().IsValueType)
				{
					valConvertible = (val as IConvertible);
					flag3 = (valConvertible != null);
				}
				else
				{
					flag3 = false;
				}
				bool flag4 = flag3;
				if (flag4)
				{
					val = valConvertible.ToType(typeTp, CultureInfo.InvariantCulture);
				}
				bool flag5 = !typeTp.IsAssignableFrom(val.GetType());
				result = !flag5;
			}
			return result;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00051C87 File Offset: 0x0004FE87
		public static Table NewTable(this Script env)
		{
			return new Table(env);
		}
	}
}
