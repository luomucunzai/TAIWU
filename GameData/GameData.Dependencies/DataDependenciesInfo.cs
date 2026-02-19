using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using GameData.Common;
using GameData.Domains;
using NLog;

namespace GameData.Dependencies;

public static class DataDependenciesInfo
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	private const string DomainNameSuffix = "Domain";

	private const string CalcMethodPrefix = "Calc";

	private const string CacheInfluencesFieldPrefix = "CacheInfluences";

	private const int InfluenceMaxDepth = 16;

	private static readonly Assembly Assembly = Assembly.GetExecutingAssembly();

	private static readonly Assembly SharedAssembly = typeof(ExternalDataBridge).Assembly;

	private static readonly Type BaseDomainType = typeof(BaseGameDataDomain);

	private static readonly Type DomainDataAttrType = typeof(DomainDataAttribute);

	private static readonly Type CollectionObjectFieldAttrType = typeof(CollectionObjectFieldAttribute);

	private static readonly Type DependencyAttrType = typeof(BaseDataDependencyAttribute);

	public static void Generate()
	{
		Dictionary<DataUidWithType, List<DataDependency>> dependencies = GetDependencies();
		Dictionary<DataUidWithType, HashSet<DataInfluence>> influences = CalcShallowInfluences(dependencies);
		Dictionary<DataIndicator, DataInfluence[][]> dictionary = AssignInfluences(influences);
	}

	private static Dictionary<DataUidWithType, List<DataDependency>> GetDependencies()
	{
		Dictionary<DataUidWithType, List<DataDependency>> dictionary = new Dictionary<DataUidWithType, List<DataDependency>>();
		Type[] types = Assembly.GetTypes();
		foreach (Type type in types)
		{
			if (type.IsSubclassOf(BaseDomainType))
			{
				GetDomainDataDependencies(type, dictionary);
			}
		}
		return dictionary;
	}

	private static void GetDomainDataDependencies(Type domainType, Dictionary<DataUidWithType, List<DataDependency>> dependencies)
	{
		string domainName = GetDomainName(domainType);
		ushort domainId = DomainHelper.DomainName2DomainId[domainName];
		string name = domainType.Namespace + "." + domainName + "DomainHelper";
		Type type = SharedAssembly.GetType(name);
		Dictionary<string, ushort> staticFieldValue = GetStaticFieldValue<Dictionary<string, ushort>>(type, "FieldName2DataId");
		FieldInfo[] fields = domainType.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		foreach (FieldInfo fieldInfo in fields)
		{
			object[] customAttributes = fieldInfo.GetCustomAttributes(DomainDataAttrType, inherit: false);
			if (customAttributes.Length != 0)
			{
				DomainDataAttribute domainDataAttribute = (DomainDataAttribute)customAttributes[0];
				if (domainDataAttribute.IsCache)
				{
					string text = ToPublicName(fieldInfo.Name);
					ushort dataId = staticFieldValue[text];
					DataUid dataUid = new DataUid(domainId, dataId, ulong.MaxValue);
					DataUidWithType key = new DataUidWithType(DomainDataType.SingleValue, dataUid);
					List<DataDependency> cacheFieldDependencies = GetCacheFieldDependencies(domainType, text);
					dependencies.Add(key, cacheFieldDependencies);
				}
				else if (domainDataAttribute.DomainDataType == DomainDataType.ObjectCollection)
				{
					string key2 = ToPublicName(fieldInfo.Name);
					ushort dataId2 = staticFieldValue[key2];
					Type objectCollectionElementType = GetObjectCollectionElementType(fieldInfo.FieldType);
					GetCollectionObjectFieldsDependencies(domainId, dataId2, objectCollectionElementType, dependencies);
				}
			}
		}
	}

	private static void GetCollectionObjectFieldsDependencies(ushort domainId, ushort dataId, Type objectType, Dictionary<DataUidWithType, List<DataDependency>> dependencies)
	{
		string name = objectType.Namespace + "." + objectType.Name + "Helper";
		Type type = SharedAssembly.GetType(name);
		Dictionary<string, ushort> staticFieldValue = GetStaticFieldValue<Dictionary<string, ushort>>(type, "FieldName2FieldId");
		FieldInfo[] fields = objectType.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		foreach (FieldInfo fieldInfo in fields)
		{
			object[] customAttributes = fieldInfo.GetCustomAttributes(CollectionObjectFieldAttrType, inherit: false);
			if (customAttributes.Length != 0)
			{
				CollectionObjectFieldAttribute collectionObjectFieldAttribute = (CollectionObjectFieldAttribute)customAttributes[0];
				if (collectionObjectFieldAttribute.IsCache)
				{
					string text = ToPublicName(fieldInfo.Name);
					ushort subId = staticFieldValue[text];
					DataUid dataUid = new DataUid(domainId, dataId, 18446744073709551614uL, subId);
					DataUidWithType key = new DataUidWithType(DomainDataType.ObjectCollection, dataUid);
					List<DataDependency> cacheFieldDependencies = GetCacheFieldDependencies(objectType, text);
					dependencies.Add(key, cacheFieldDependencies);
				}
			}
		}
	}

	private static List<DataDependency> GetCacheFieldDependencies(Type classType, string fieldPublicName)
	{
		string text = "Calc" + fieldPublicName;
		MethodInfo method = classType.GetMethod(text, BindingFlags.Instance | BindingFlags.NonPublic);
		if (method == null)
		{
			throw new Exception("Failed to get method " + classType.FullName + "." + text);
		}
		object[] customAttributes = method.GetCustomAttributes(DependencyAttrType, inherit: false);
		if (customAttributes.Length == 0)
		{
			throw new Exception("Calc-method of cache field must be annotated with derived class of BaseDataDependencyAttribute: " + classType.FullName + "." + text);
		}
		List<DataDependency> list = new List<DataDependency>();
		object[] array = customAttributes;
		for (int i = 0; i < array.Length; i++)
		{
			BaseDataDependencyAttribute baseDataDependencyAttribute = (BaseDataDependencyAttribute)array[i];
			if (baseDataDependencyAttribute.SourceUids.Length == 0)
			{
				throw new Exception($"You have to set at least one DataUid in one dependency ({baseDataDependencyAttribute}): {classType.FullName}.{text}");
			}
			list.Add(new DataDependency(baseDataDependencyAttribute.SourceType, baseDataDependencyAttribute.SourceUids, baseDataDependencyAttribute.Condition, baseDataDependencyAttribute.Scope));
		}
		return list;
	}

	private static Dictionary<DataUidWithType, HashSet<DataInfluence>> CalcShallowInfluences(Dictionary<DataUidWithType, List<DataDependency>> dependencies)
	{
		Dictionary<DataUidWithType, HashSet<DataInfluence>> dictionary = new Dictionary<DataUidWithType, HashSet<DataInfluence>>();
		DataInfluence dataInfluence = new DataInfluence(default(DataIndicator), InfluenceCondition.None, InfluenceScope.All);
		foreach (KeyValuePair<DataUidWithType, List<DataDependency>> dependency in dependencies)
		{
			DataUidWithType key = dependency.Key;
			List<DataDependency> value = dependency.Value;
			DataIndicator targetIndicator = (dataInfluence.TargetIndicator = new DataIndicator(key.Type, key.DataUid.DomainId, key.DataUid.DataId));
			foreach (DataDependency item in value)
			{
				dataInfluence.Condition = item.Condition;
				dataInfluence.Scope = item.Scope;
				DataUid[] sourceUids = item.SourceUids;
				foreach (DataUid dataUid in sourceUids)
				{
					DataUidWithType key2 = new DataUidWithType(item.SourceType, dataUid);
					if (!dictionary.TryGetValue(key2, out var value2))
					{
						value2 = new HashSet<DataInfluence>();
						dictionary.Add(key2, value2);
					}
					if (!value2.TryGetValue(dataInfluence, out var actualValue))
					{
						actualValue = new DataInfluence(targetIndicator, item.Condition, item.Scope);
						value2.Add(actualValue);
					}
					actualValue.TargetUids.Add(key.DataUid);
				}
			}
		}
		return dictionary;
	}

	private static Dictionary<DataIndicator, DataInfluence[][]> AssignInfluences(Dictionary<DataUidWithType, HashSet<DataInfluence>> influences)
	{
		Dictionary<DataIndicator, DataInfluence[][]> dictionary = new Dictionary<DataIndicator, DataInfluence[][]>();
		foreach (KeyValuePair<DataUidWithType, HashSet<DataInfluence>> influence in influences)
		{
			DataUidWithType key = influence.Key;
			HashSet<DataInfluence> value = influence.Value;
			DataIndicator dataIndicator = new DataIndicator(key.Type, key.DataUid.DomainId, key.DataUid.DataId);
			if (!dictionary.TryGetValue(dataIndicator, out var value2))
			{
				switch (dataIndicator.DataType)
				{
				case DomainDataType.SingleValue:
				case DomainDataType.SingleValueCollection:
					value2 = GetCacheInfluencesOfDomain(dataIndicator);
					break;
				case DomainDataType.ElementList:
					value2 = GetCacheInfluencesOfElementList(dataIndicator);
					break;
				default:
					value2 = GetCacheInfluencesOfCollectionObject(dataIndicator);
					break;
				}
				dictionary.Add(dataIndicator, value2);
			}
			DataInfluence[] array = HashSetToArray(value);
			switch (dataIndicator.DataType)
			{
			case DomainDataType.SingleValue:
			case DomainDataType.SingleValueCollection:
				value2[key.DataUid.DataId] = array;
				break;
			case DomainDataType.ElementList:
				value2[(uint)key.DataUid.SubId0] = array;
				break;
			default:
				value2[key.DataUid.SubId1] = array;
				break;
			}
		}
		return dictionary;
	}

	private static DataInfluence[][] GetCacheInfluencesOfDomain(DataIndicator indicator)
	{
		BaseGameDataDomain baseGameDataDomain = DomainManager.Domains[indicator.DomainId];
		Type type = baseGameDataDomain.GetType();
		FieldInfo field = type.GetField("CacheInfluences", BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.NonPublic);
		if (field == null)
		{
			throw new Exception("Cannot find field CacheInfluences on class " + type.FullName);
		}
		return (DataInfluence[][])field.GetValue(null);
	}

	private static DataInfluence[][] GetCacheInfluencesOfElementList(DataIndicator indicator)
	{
		BaseGameDataDomain baseGameDataDomain = DomainManager.Domains[indicator.DomainId];
		Type type = baseGameDataDomain.GetType();
		string text = DomainHelper.DomainId2DataId2FieldName[indicator.DomainId][indicator.DataId];
		string text2 = "CacheInfluences" + text;
		FieldInfo field = type.GetField(text2, BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.NonPublic);
		if (field == null)
		{
			throw new Exception("Cannot find field " + text2 + " on class " + type.FullName);
		}
		return (DataInfluence[][])field.GetValue(null);
	}

	private static DataInfluence[][] GetCacheInfluencesOfCollectionObject(DataIndicator indicator)
	{
		BaseGameDataDomain baseGameDataDomain = DomainManager.Domains[indicator.DomainId];
		Type type = baseGameDataDomain.GetType();
		string text = DomainHelper.DomainId2DataId2FieldName[indicator.DomainId][indicator.DataId];
		string text2 = "CacheInfluences" + text;
		FieldInfo field = type.GetField(text2, BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.NonPublic);
		if (field == null)
		{
			throw new Exception("Cannot find field " + text2 + " on class " + type.FullName);
		}
		return (DataInfluence[][])field.GetValue(null);
	}

	private static Dictionary<DataUid, HashSet<DataUid>> CalcDeepInfluences(Dictionary<DataUidWithType, List<DataDependency>> dependencies)
	{
		Dictionary<DataUid, HashSet<DataUid>> dictionary = new Dictionary<DataUid, HashSet<DataUid>>();
		foreach (KeyValuePair<DataUidWithType, List<DataDependency>> dependency in dependencies)
		{
			DataUid dataUid = dependency.Key.DataUid;
			List<DataDependency> value = dependency.Value;
			foreach (DataDependency item in value)
			{
				DataUid[] sourceUids = item.SourceUids;
				foreach (DataUid key in sourceUids)
				{
					if (!dictionary.TryGetValue(key, out var value2))
					{
						value2 = new HashSet<DataUid>();
						dictionary.Add(key, value2);
					}
					value2.Add(dataUid);
				}
			}
		}
		foreach (KeyValuePair<DataUid, HashSet<DataUid>> item2 in dictionary)
		{
			CalcDeeperInfluences(dictionary, item2.Key, item2.Value, item2.Value, 0);
		}
		return dictionary;
	}

	private static void CalcDeeperInfluences(Dictionary<DataUid, HashSet<DataUid>> influences, DataUid sourceUid, HashSet<DataUid> targetUids, HashSet<DataUid> currTargetUids, int depth)
	{
		if (++depth > 16)
		{
			throw new Exception($"Encountered max depth of influence: {sourceUid}");
		}
		DataUid[] array = HashSetToArray(currTargetUids);
		foreach (DataUid key in array)
		{
			if (influences.TryGetValue(key, out var value))
			{
				targetUids.UnionWith(value);
				CalcDeeperInfluences(influences, sourceUid, targetUids, value, depth);
			}
		}
	}

	private static void LogDependencies(Dictionary<DataUidWithType, List<DataDependency>> dependencies)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Dependencies:");
		foreach (KeyValuePair<DataUidWithType, List<DataDependency>> dependency in dependencies)
		{
			DataUid dataUid = dependency.Key.DataUid;
			List<DataDependency> value = dependency.Value;
			StringBuilder stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder3 = stringBuilder2;
			StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(3, 1, stringBuilder2);
			handler.AppendLiteral("  ");
			handler.AppendFormatted(dataUid.ToString());
			handler.AppendLiteral(":");
			stringBuilder3.AppendLine(ref handler);
			foreach (DataDependency item in value)
			{
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder4 = stringBuilder2;
				handler = new StringBuilder.AppendInterpolatedStringHandler(4, 1, stringBuilder2);
				handler.AppendLiteral("    ");
				handler.AppendFormatted(item);
				stringBuilder4.AppendLine(ref handler);
			}
		}
		Logger.Debug(stringBuilder.ToString());
	}

	private static void LogDeepInfluences(Dictionary<DataUid, HashSet<DataUid>> influences)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Deep influences:");
		foreach (KeyValuePair<DataUid, HashSet<DataUid>> influence in influences)
		{
			DataUid key = influence.Key;
			HashSet<DataUid> value = influence.Value;
			StringBuilder stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder3 = stringBuilder2;
			StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(3, 1, stringBuilder2);
			handler.AppendLiteral("  ");
			handler.AppendFormatted(key.ToString());
			handler.AppendLiteral(":");
			stringBuilder3.AppendLine(ref handler);
			foreach (DataUid item in value)
			{
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder4 = stringBuilder2;
				handler = new StringBuilder.AppendInterpolatedStringHandler(4, 1, stringBuilder2);
				handler.AppendLiteral("    ");
				handler.AppendFormatted(item.ToString());
				stringBuilder4.AppendLine(ref handler);
			}
		}
		Logger.Debug(stringBuilder.ToString());
	}

	private static void LogInfluences(Dictionary<DataIndicator, DataInfluence[][]> indicator2FieldId2Influences)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Influences:");
		foreach (KeyValuePair<DataIndicator, DataInfluence[][]> indicator2FieldId2Influence in indicator2FieldId2Influences)
		{
			DataIndicator key = indicator2FieldId2Influence.Key;
			DataInfluence[][] value = indicator2FieldId2Influence.Value;
			StringBuilder stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder3 = stringBuilder2;
			StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(3, 1, stringBuilder2);
			handler.AppendLiteral("  ");
			handler.AppendFormatted(key.ToString());
			handler.AppendLiteral(":");
			stringBuilder3.AppendLine(ref handler);
			switch (key.DataType)
			{
			case DomainDataType.SingleValue:
			case DomainDataType.SingleValueCollection:
			{
				DataInfluence[] array = value[key.DataId];
				if (array != null)
				{
					string value2 = DomainHelper.DomainId2DataId2FieldName[key.DomainId][key.DataId];
					stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder4 = stringBuilder2;
					handler = new StringBuilder.AppendInterpolatedStringHandler(5, 1, stringBuilder2);
					handler.AppendLiteral("    ");
					handler.AppendFormatted(value2);
					handler.AppendLiteral(":");
					stringBuilder4.AppendLine(ref handler);
					DataInfluence[] array2 = array;
					foreach (DataInfluence value3 in array2)
					{
						stringBuilder2 = stringBuilder;
						StringBuilder stringBuilder5 = stringBuilder2;
						handler = new StringBuilder.AppendInterpolatedStringHandler(6, 1, stringBuilder2);
						handler.AppendLiteral("      ");
						handler.AppendFormatted(value3);
						stringBuilder5.AppendLine(ref handler);
					}
				}
				continue;
			}
			case DomainDataType.ElementList:
			{
				int num = value.Length;
				for (int j = 0; j < num; j++)
				{
					DataInfluence[] array3 = value[j];
					if (array3 != null)
					{
						stringBuilder2 = stringBuilder;
						StringBuilder stringBuilder6 = stringBuilder2;
						handler = new StringBuilder.AppendInterpolatedStringHandler(5, 1, stringBuilder2);
						handler.AppendLiteral("    ");
						handler.AppendFormatted(j.ToString());
						handler.AppendLiteral(":");
						stringBuilder6.AppendLine(ref handler);
						DataInfluence[] array4 = array3;
						foreach (DataInfluence value4 in array4)
						{
							stringBuilder2 = stringBuilder;
							StringBuilder stringBuilder7 = stringBuilder2;
							handler = new StringBuilder.AppendInterpolatedStringHandler(6, 1, stringBuilder2);
							handler.AppendLiteral("      ");
							handler.AppendFormatted(value4);
							stringBuilder7.AppendLine(ref handler);
						}
					}
				}
				continue;
			}
			}
			string[] array5 = DomainHelper.DomainId2DataId2ObjectFieldId2FieldName[key.DomainId][key.DataId];
			int num2 = value.Length;
			for (int l = 0; l < num2; l++)
			{
				DataInfluence[] array6 = value[l];
				if (array6 != null)
				{
					string value5 = array5[l];
					stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder8 = stringBuilder2;
					handler = new StringBuilder.AppendInterpolatedStringHandler(5, 1, stringBuilder2);
					handler.AppendLiteral("    ");
					handler.AppendFormatted(value5);
					handler.AppendLiteral(":");
					stringBuilder8.AppendLine(ref handler);
					DataInfluence[] array7 = array6;
					foreach (DataInfluence value6 in array7)
					{
						stringBuilder2 = stringBuilder;
						StringBuilder stringBuilder9 = stringBuilder2;
						handler = new StringBuilder.AppendInterpolatedStringHandler(6, 1, stringBuilder2);
						handler.AppendLiteral("      ");
						handler.AppendFormatted(value6);
						stringBuilder9.AppendLine(ref handler);
					}
				}
			}
		}
		Logger.Debug(stringBuilder.ToString());
	}

	private static T[] HashSetToArray<T>(HashSet<T> items)
	{
		int count = items.Count;
		T[] array = new T[count];
		int num = 0;
		foreach (T item in items)
		{
			array[num] = item;
			num++;
		}
		return array;
	}

	private static string GetDomainName(Type domainType)
	{
		string name = domainType.Name;
		if (!name.EndsWith("Domain"))
		{
			throw new Exception("Domain name must end with Domain: " + domainType.FullName);
		}
		return name.Substring(0, name.Length - "Domain".Length);
	}

	private static Type GetObjectCollectionElementType(Type collectionType)
	{
		Type type = collectionType.GetInterface("System.Collections.Generic.IDictionary`2");
		if (type == null)
		{
			throw new Exception("The specified type is not implement IDictionary interface: " + collectionType.FullName);
		}
		Type[] genericArguments = type.GetGenericArguments();
		return genericArguments[1];
	}

	private static T GetStaticFieldValue<T>(Type classType, string filedName)
	{
		FieldInfo field = classType.GetField(filedName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		if (field == null)
		{
			throw new Exception("Failed to get field: " + classType.FullName + "." + filedName);
		}
		return (T)field.GetValue(null);
	}

	private static string ToPublicName(string fieldName)
	{
		StringBuilder stringBuilder = new StringBuilder(fieldName);
		if (stringBuilder[0] == '_')
		{
			stringBuilder.Remove(0, 1);
		}
		stringBuilder[0] = char.ToUpper(stringBuilder[0]);
		return stringBuilder.ToString();
	}
}
