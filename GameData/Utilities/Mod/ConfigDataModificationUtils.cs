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

namespace GameData.Utilities.Mod
{
	// Token: 0x02000018 RID: 24
	public static class ConfigDataModificationUtils
	{
		// Token: 0x06000080 RID: 128 RVA: 0x00050C48 File Offset: 0x0004EE48
		public static void ReplaceConfig<TConfig, TItem>(this TConfig config, int templateId, TItem item) where TConfig : IEnumerable<TItem>, IConfigData
		{
			Type configType = config.GetType();
			FieldInfo field = configType.GetField("_dataArray", BindingFlags.Instance | BindingFlags.NonPublic);
			List<TItem> dataList = (List<TItem>)((field != null) ? field.GetValue(config) : null);
			dataList[templateId] = item;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00050C94 File Offset: 0x0004EE94
		[Obsolete]
		public static void AppendConfig<TConfig, TItem>(this TConfig config, TItem item) where TConfig : IEnumerable<TItem>, IConfigData
		{
			Type configType = config.GetType();
			FieldInfo field = configType.GetField("_dataArray", BindingFlags.Instance | BindingFlags.NonPublic);
			List<TItem> dataList = (List<TItem>)((field != null) ? field.GetValue(config) : null);
			dataList.Add(item);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00050CE0 File Offset: 0x0004EEE0
		[Obsolete]
		public static void AppendConfig<TConfig>(this TConfig config, object item) where TConfig : IConfigData
		{
			Type configType = config.GetType();
			FieldInfo field = configType.GetField("_dataArray", BindingFlags.Instance | BindingFlags.NonPublic);
			IList dataList = (IList)((field != null) ? field.GetValue(config) : null);
			dataList.Add(item);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00050D2C File Offset: 0x0004EF2C
		public static void AppendConfigRange<TConfig, TItem>(this TConfig config, IEnumerable<TItem> items) where TConfig : IEnumerable<TItem>, IConfigData
		{
			Type configType = config.GetType();
			FieldInfo field = configType.GetField("_dataArray", BindingFlags.Instance | BindingFlags.NonPublic);
			List<TItem> dataList = (List<TItem>)((field != null) ? field.GetValue(config) : null);
			dataList.AddRange(items);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00050D78 File Offset: 0x0004EF78
		public static TItem GetConfigItem<TConfig, TItem>(this TConfig config, string refName) where TConfig : IEnumerable<TItem>, IConfigData
		{
			int templateId = config.GetItemId(refName);
			return config.GetConfigItem(templateId);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00050DA0 File Offset: 0x0004EFA0
		public static object GetConfigItem<TConfig>(this TConfig config, string refName) where TConfig : IConfigData
		{
			int templateId = config.GetItemId(refName);
			return config.GetConfigItem(templateId);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00050DC8 File Offset: 0x0004EFC8
		public static TItem GetConfigItem<TConfig, TItem>(this TConfig config, int templateId) where TConfig : IEnumerable<TItem>, IConfigData
		{
			Type configType = config.GetType();
			FieldInfo field = configType.GetField("_dataArray", BindingFlags.Instance | BindingFlags.NonPublic);
			List<TItem> dataList = (List<TItem>)((field != null) ? field.GetValue(config) : null);
			return dataList[templateId];
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00050E14 File Offset: 0x0004F014
		public static object GetConfigItem<TConfig>(this TConfig config, int templateId) where TConfig : IConfigData
		{
			Type configType = config.GetType();
			FieldInfo field = configType.GetField("_dataArray", BindingFlags.Instance | BindingFlags.NonPublic);
			IList dataList = (IList)((field != null) ? field.GetValue(config) : null);
			return dataList[templateId];
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00050E60 File Offset: 0x0004F060
		public static int GetConfigCount<TConfig>(this TConfig config) where TConfig : IConfigData
		{
			IList dataList = (IList)ReflectionExtensions.GetFieldValue<TConfig>(config, "_dataArray");
			return dataList.Count;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00050E8C File Offset: 0x0004F08C
		public static object GetConfigPropertyValue(string typeName, int templateId, string propertyName)
		{
			return ReflectionExtensions.GetFieldValue<object>(ConfigCollection.NameMap[typeName].GetConfigItem(templateId), propertyName);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00050EB8 File Offset: 0x0004F0B8
		public static void ModifyConfigObjectPropertyValue<TItem>(TItem config, string propertyName, string propertyTypeName, object[] parameters)
		{
			Type type = Assembly.GetExecutingAssembly().GetType(propertyTypeName);
			bool flag = type == null;
			if (!flag)
			{
				object propertyVal = Activator.CreateInstance(type, parameters);
				ReflectionExtensions.ModifyField<TItem>(config, propertyName, propertyVal);
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00050EF0 File Offset: 0x0004F0F0
		[Obsolete("Use Config.ConfigCollection.NameMap[string] instead.")]
		public static IConfigData GetConfigData(string configTypeName)
		{
			return ConfigCollection.NameMap[configTypeName];
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00050F10 File Offset: 0x0004F110
		public static object CreateDeepCopy(this object obj)
		{
			int offset = ConfigDataModificationUtils.DataPool.GetWritingOffset();
			ConfigDataModificationUtils.BinaryFormatter.Serialize(ConfigDataModificationUtils.DataPool, obj);
			object result = ConfigDataModificationUtils.BinaryFormatter.Deserialize(ConfigDataModificationUtils.DataPool);
			ConfigDataModificationUtils.DataPool.SetStreamReadingOffset(offset);
			ConfigDataModificationUtils.DataPool.Clear();
			return result;
		}

		// Token: 0x04000071 RID: 113
		private static readonly RawDataPool DataPool = new RawDataPool(1024);

		// Token: 0x04000072 RID: 114
		private static readonly IFormatter BinaryFormatter = new BinaryFormatter
		{
			AssemblyFormat = FormatterAssemblyStyle.Simple
		};
	}
}
