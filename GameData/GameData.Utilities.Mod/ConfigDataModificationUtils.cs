using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using Config;
using Config.Common;
using TaiwuModdingLib.Core.Utils;

namespace GameData.Utilities.Mod;

public static class ConfigDataModificationUtils
{
	private static readonly RawDataPool DataPool = new RawDataPool(1024);

	private static readonly IFormatter BinaryFormatter = new BinaryFormatter
	{
		AssemblyFormat = FormatterAssemblyStyle.Simple
	};

	public static void ReplaceConfig<TConfig, TItem>(this TConfig config, int templateId, TItem item) where TConfig : IEnumerable<TItem>, IConfigData
	{
		Type type = config.GetType();
		List<TItem> list = (List<TItem>)(type.GetField("_dataArray", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(config));
		list[templateId] = item;
	}

	[Obsolete]
	public static void AppendConfig<TConfig, TItem>(this TConfig config, TItem item) where TConfig : IEnumerable<TItem>, IConfigData
	{
		Type type = config.GetType();
		List<TItem> list = (List<TItem>)(type.GetField("_dataArray", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(config));
		list.Add(item);
	}

	[Obsolete]
	public static void AppendConfig<TConfig>(this TConfig config, object item) where TConfig : IConfigData
	{
		Type type = config.GetType();
		IList list = (IList)(type.GetField("_dataArray", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(config));
		list.Add(item);
	}

	public static void AppendConfigRange<TConfig, TItem>(this TConfig config, IEnumerable<TItem> items) where TConfig : IEnumerable<TItem>, IConfigData
	{
		Type type = config.GetType();
		List<TItem> list = (List<TItem>)(type.GetField("_dataArray", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(config));
		list.AddRange(items);
	}

	public static TItem GetConfigItem<TConfig, TItem>(this TConfig config, string refName) where TConfig : IEnumerable<TItem>, IConfigData
	{
		int itemId = config.GetItemId(refName);
		return config.GetConfigItem<TConfig, TItem>(itemId);
	}

	public static object GetConfigItem<TConfig>(this TConfig config, string refName) where TConfig : IConfigData
	{
		int itemId = config.GetItemId(refName);
		return config.GetConfigItem(itemId);
	}

	public static TItem GetConfigItem<TConfig, TItem>(this TConfig config, int templateId) where TConfig : IEnumerable<TItem>, IConfigData
	{
		Type type = config.GetType();
		List<TItem> list = (List<TItem>)(type.GetField("_dataArray", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(config));
		return list[templateId];
	}

	public static object GetConfigItem<TConfig>(this TConfig config, int templateId) where TConfig : IConfigData
	{
		Type type = config.GetType();
		IList list = (IList)(type.GetField("_dataArray", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(config));
		return list[templateId];
	}

	public static int GetConfigCount<TConfig>(this TConfig config) where TConfig : IConfigData
	{
		IList list = (IList)ReflectionExtensions.GetFieldValue<TConfig>(config, "_dataArray");
		return list.Count;
	}

	public static object GetConfigPropertyValue(string typeName, int templateId, string propertyName)
	{
		return ReflectionExtensions.GetFieldValue<object>(ConfigCollection.NameMap[typeName].GetConfigItem(templateId), propertyName);
	}

	public static void ModifyConfigObjectPropertyValue<TItem>(TItem config, string propertyName, string propertyTypeName, object[] parameters)
	{
		Type type = Assembly.GetExecutingAssembly().GetType(propertyTypeName);
		if (!(type == null))
		{
			object obj = Activator.CreateInstance(type, parameters);
			ReflectionExtensions.ModifyField<TItem>(config, propertyName, obj);
		}
	}

	[Obsolete("Use Config.ConfigCollection.NameMap[string] instead.")]
	public static IConfigData GetConfigData(string configTypeName)
	{
		return ConfigCollection.NameMap[configTypeName];
	}

	public static object CreateDeepCopy(this object obj)
	{
		int writingOffset = DataPool.GetWritingOffset();
		BinaryFormatter.Serialize(DataPool, obj);
		object result = BinaryFormatter.Deserialize(DataPool);
		DataPool.SetStreamReadingOffset(writingOffset);
		DataPool.Clear();
		return result;
	}
}
