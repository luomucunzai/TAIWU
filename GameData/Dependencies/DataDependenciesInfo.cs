using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using GameData.Common;
using GameData.Domains;
using NLog;

namespace GameData.Dependencies
{
	// Token: 0x020008E1 RID: 2273
	public static class DataDependenciesInfo
	{
		// Token: 0x0600819C RID: 33180 RVA: 0x004D65F8 File Offset: 0x004D47F8
		public static void Generate()
		{
			Dictionary<DataUidWithType, List<DataDependency>> dependencies = DataDependenciesInfo.GetDependencies();
			Dictionary<DataUidWithType, HashSet<DataInfluence>> shallowInfluences = DataDependenciesInfo.CalcShallowInfluences(dependencies);
			Dictionary<DataIndicator, DataInfluence[][]> indicator2FieldId2Influences = DataDependenciesInfo.AssignInfluences(shallowInfluences);
		}

		// Token: 0x0600819D RID: 33181 RVA: 0x004D661C File Offset: 0x004D481C
		private static Dictionary<DataUidWithType, List<DataDependency>> GetDependencies()
		{
			Dictionary<DataUidWithType, List<DataDependency>> dependencies = new Dictionary<DataUidWithType, List<DataDependency>>();
			foreach (Type type in DataDependenciesInfo.Assembly.GetTypes())
			{
				bool flag = !type.IsSubclassOf(DataDependenciesInfo.BaseDomainType);
				if (!flag)
				{
					DataDependenciesInfo.GetDomainDataDependencies(type, dependencies);
				}
			}
			return dependencies;
		}

		// Token: 0x0600819E RID: 33182 RVA: 0x004D6678 File Offset: 0x004D4878
		private static void GetDomainDataDependencies(Type domainType, Dictionary<DataUidWithType, List<DataDependency>> dependencies)
		{
			string domainName = DataDependenciesInfo.GetDomainName(domainType);
			ushort domainId = DomainHelper.DomainName2DomainId[domainName];
			string helperTypeName = domainType.Namespace + "." + domainName + "DomainHelper";
			Type helperType = DataDependenciesInfo.SharedAssembly.GetType(helperTypeName);
			Dictionary<string, ushort> fieldName2DataId = DataDependenciesInfo.GetStaticFieldValue<Dictionary<string, ushort>>(helperType, "FieldName2DataId");
			foreach (FieldInfo fieldInfo in domainType.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
			{
				object[] fieldAttributes = fieldInfo.GetCustomAttributes(DataDependenciesInfo.DomainDataAttrType, false);
				bool flag = fieldAttributes.Length == 0;
				if (!flag)
				{
					DomainDataAttribute fieldAttr = (DomainDataAttribute)fieldAttributes[0];
					bool isCache = fieldAttr.IsCache;
					if (isCache)
					{
						string fieldPublicName = DataDependenciesInfo.ToPublicName(fieldInfo.Name);
						ushort dataId = fieldName2DataId[fieldPublicName];
						DataUid targetUid = new DataUid(domainId, dataId, ulong.MaxValue, uint.MaxValue);
						DataUidWithType targetUidWithType = new DataUidWithType(DomainDataType.SingleValue, targetUid);
						List<DataDependency> fieldDependencies = DataDependenciesInfo.GetCacheFieldDependencies(domainType, fieldPublicName);
						dependencies.Add(targetUidWithType, fieldDependencies);
					}
					else
					{
						bool flag2 = fieldAttr.DomainDataType == DomainDataType.ObjectCollection;
						if (flag2)
						{
							string fieldPublicName2 = DataDependenciesInfo.ToPublicName(fieldInfo.Name);
							ushort dataId2 = fieldName2DataId[fieldPublicName2];
							Type elementType = DataDependenciesInfo.GetObjectCollectionElementType(fieldInfo.FieldType);
							DataDependenciesInfo.GetCollectionObjectFieldsDependencies(domainId, dataId2, elementType, dependencies);
						}
					}
				}
			}
		}

		// Token: 0x0600819F RID: 33183 RVA: 0x004D67BC File Offset: 0x004D49BC
		private static void GetCollectionObjectFieldsDependencies(ushort domainId, ushort dataId, Type objectType, Dictionary<DataUidWithType, List<DataDependency>> dependencies)
		{
			string helperTypeName = objectType.Namespace + "." + objectType.Name + "Helper";
			Type helperType = DataDependenciesInfo.SharedAssembly.GetType(helperTypeName);
			Dictionary<string, ushort> fieldName2FieldId = DataDependenciesInfo.GetStaticFieldValue<Dictionary<string, ushort>>(helperType, "FieldName2FieldId");
			foreach (FieldInfo fieldInfo in objectType.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
			{
				object[] fieldAttributes = fieldInfo.GetCustomAttributes(DataDependenciesInfo.CollectionObjectFieldAttrType, false);
				bool flag = fieldAttributes.Length == 0;
				if (!flag)
				{
					CollectionObjectFieldAttribute fieldAttr = (CollectionObjectFieldAttribute)fieldAttributes[0];
					bool flag2 = !fieldAttr.IsCache;
					if (!flag2)
					{
						string fieldPublicName = DataDependenciesInfo.ToPublicName(fieldInfo.Name);
						ushort fieldId = fieldName2FieldId[fieldPublicName];
						DataUid targetUid = new DataUid(domainId, dataId, 18446744073709551614UL, (uint)fieldId);
						DataUidWithType targetUidWithType = new DataUidWithType(DomainDataType.ObjectCollection, targetUid);
						List<DataDependency> fieldDependencies = DataDependenciesInfo.GetCacheFieldDependencies(objectType, fieldPublicName);
						dependencies.Add(targetUidWithType, fieldDependencies);
					}
				}
			}
		}

		// Token: 0x060081A0 RID: 33184 RVA: 0x004D68A8 File Offset: 0x004D4AA8
		private static List<DataDependency> GetCacheFieldDependencies(Type classType, string fieldPublicName)
		{
			string methodName = "Calc" + fieldPublicName;
			MethodInfo methodInfo = classType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
			bool flag = methodInfo == null;
			if (flag)
			{
				throw new Exception("Failed to get method " + classType.FullName + "." + methodName);
			}
			object[] dependencyAttributes = methodInfo.GetCustomAttributes(DataDependenciesInfo.DependencyAttrType, false);
			bool flag2 = dependencyAttributes.Length == 0;
			if (flag2)
			{
				throw new Exception("Calc-method of cache field must be annotated with derived class of BaseDataDependencyAttribute: " + classType.FullName + "." + methodName);
			}
			List<DataDependency> dependencies = new List<DataDependency>();
			foreach (BaseDataDependencyAttribute attr in dependencyAttributes)
			{
				bool flag3 = attr.SourceUids.Length == 0;
				if (flag3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(60, 3);
					defaultInterpolatedStringHandler.AppendLiteral("You have to set at least one DataUid in one dependency (");
					defaultInterpolatedStringHandler.AppendFormatted<BaseDataDependencyAttribute>(attr);
					defaultInterpolatedStringHandler.AppendLiteral("): ");
					defaultInterpolatedStringHandler.AppendFormatted(classType.FullName);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					defaultInterpolatedStringHandler.AppendFormatted(methodName);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				dependencies.Add(new DataDependency(attr.SourceType, attr.SourceUids, attr.Condition, attr.Scope));
			}
			return dependencies;
		}

		// Token: 0x060081A1 RID: 33185 RVA: 0x004D69FC File Offset: 0x004D4BFC
		private static Dictionary<DataUidWithType, HashSet<DataInfluence>> CalcShallowInfluences(Dictionary<DataUidWithType, List<DataDependency>> dependencies)
		{
			Dictionary<DataUidWithType, HashSet<DataInfluence>> influences = new Dictionary<DataUidWithType, HashSet<DataInfluence>>();
			DataInfluence templateInfluence = new DataInfluence(default(DataIndicator), InfluenceCondition.None, InfluenceScope.All);
			foreach (KeyValuePair<DataUidWithType, List<DataDependency>> entry in dependencies)
			{
				DataUidWithType targetUidWithType = entry.Key;
				List<DataDependency> currDependencies = entry.Value;
				DataIndicator targetIndicator = new DataIndicator(targetUidWithType.Type, targetUidWithType.DataUid.DomainId, targetUidWithType.DataUid.DataId);
				templateInfluence.TargetIndicator = targetIndicator;
				foreach (DataDependency currDependency in currDependencies)
				{
					templateInfluence.Condition = currDependency.Condition;
					templateInfluence.Scope = currDependency.Scope;
					foreach (DataUid sourceUid in currDependency.SourceUids)
					{
						DataUidWithType sourceUidWithType = new DataUidWithType(currDependency.SourceType, sourceUid);
						HashSet<DataInfluence> currInfluences;
						bool flag = !influences.TryGetValue(sourceUidWithType, out currInfluences);
						if (flag)
						{
							currInfluences = new HashSet<DataInfluence>();
							influences.Add(sourceUidWithType, currInfluences);
						}
						DataInfluence currInfluence;
						bool flag2 = !currInfluences.TryGetValue(templateInfluence, out currInfluence);
						if (flag2)
						{
							currInfluence = new DataInfluence(targetIndicator, currDependency.Condition, currDependency.Scope);
							currInfluences.Add(currInfluence);
						}
						currInfluence.TargetUids.Add(targetUidWithType.DataUid);
					}
				}
			}
			return influences;
		}

		// Token: 0x060081A2 RID: 33186 RVA: 0x004D6BD4 File Offset: 0x004D4DD4
		private static Dictionary<DataIndicator, DataInfluence[][]> AssignInfluences(Dictionary<DataUidWithType, HashSet<DataInfluence>> influences)
		{
			Dictionary<DataIndicator, DataInfluence[][]> indicator2FieldId2Influences = new Dictionary<DataIndicator, DataInfluence[][]>();
			foreach (KeyValuePair<DataUidWithType, HashSet<DataInfluence>> entry in influences)
			{
				DataUidWithType sourceUidWithType = entry.Key;
				HashSet<DataInfluence> dataInfluences = entry.Value;
				DataIndicator sourceIndicator = new DataIndicator(sourceUidWithType.Type, sourceUidWithType.DataUid.DomainId, sourceUidWithType.DataUid.DataId);
				DataInfluence[][] fieldId2Influences;
				bool flag = !indicator2FieldId2Influences.TryGetValue(sourceIndicator, out fieldId2Influences);
				if (flag)
				{
					switch (sourceIndicator.DataType)
					{
					case DomainDataType.SingleValue:
					case DomainDataType.SingleValueCollection:
						fieldId2Influences = DataDependenciesInfo.GetCacheInfluencesOfDomain(sourceIndicator);
						break;
					case DomainDataType.ElementList:
						fieldId2Influences = DataDependenciesInfo.GetCacheInfluencesOfElementList(sourceIndicator);
						break;
					case DomainDataType.ObjectCollection:
						goto IL_A1;
					default:
						goto IL_A1;
					}
					IL_AC:
					indicator2FieldId2Influences.Add(sourceIndicator, fieldId2Influences);
					goto IL_B8;
					IL_A1:
					fieldId2Influences = DataDependenciesInfo.GetCacheInfluencesOfCollectionObject(sourceIndicator);
					goto IL_AC;
				}
				IL_B8:
				DataInfluence[] influenceArray = DataDependenciesInfo.HashSetToArray<DataInfluence>(dataInfluences);
				switch (sourceIndicator.DataType)
				{
				case DomainDataType.SingleValue:
				case DomainDataType.SingleValueCollection:
					fieldId2Influences[(int)sourceUidWithType.DataUid.DataId] = influenceArray;
					break;
				case DomainDataType.ElementList:
					fieldId2Influences[(int)sourceUidWithType.DataUid.SubId0] = influenceArray;
					break;
				case DomainDataType.ObjectCollection:
					goto IL_10C;
				default:
					goto IL_10C;
				}
				continue;
				IL_10C:
				fieldId2Influences[(int)sourceUidWithType.DataUid.SubId1] = influenceArray;
			}
			return indicator2FieldId2Influences;
		}

		// Token: 0x060081A3 RID: 33187 RVA: 0x004D6D40 File Offset: 0x004D4F40
		private static DataInfluence[][] GetCacheInfluencesOfDomain(DataIndicator indicator)
		{
			BaseGameDataDomain domainInstance = DomainManager.Domains[(int)indicator.DomainId];
			Type domainType = domainInstance.GetType();
			FieldInfo fieldInfo = domainType.GetField("CacheInfluences", BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.NonPublic);
			bool flag = fieldInfo == null;
			if (flag)
			{
				throw new Exception("Cannot find field CacheInfluences on class " + domainType.FullName);
			}
			return (DataInfluence[][])fieldInfo.GetValue(null);
		}

		// Token: 0x060081A4 RID: 33188 RVA: 0x004D6DA4 File Offset: 0x004D4FA4
		private static DataInfluence[][] GetCacheInfluencesOfElementList(DataIndicator indicator)
		{
			BaseGameDataDomain domainInstance = DomainManager.Domains[(int)indicator.DomainId];
			Type domainType = domainInstance.GetType();
			string dataFieldName = DomainHelper.DomainId2DataId2FieldName[(int)indicator.DomainId][(int)indicator.DataId];
			string fieldName = "CacheInfluences" + dataFieldName;
			FieldInfo fieldInfo = domainType.GetField(fieldName, BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.NonPublic);
			bool flag = fieldInfo == null;
			if (flag)
			{
				throw new Exception("Cannot find field " + fieldName + " on class " + domainType.FullName);
			}
			return (DataInfluence[][])fieldInfo.GetValue(null);
		}

		// Token: 0x060081A5 RID: 33189 RVA: 0x004D6E30 File Offset: 0x004D5030
		private static DataInfluence[][] GetCacheInfluencesOfCollectionObject(DataIndicator indicator)
		{
			BaseGameDataDomain domainInstance = DomainManager.Domains[(int)indicator.DomainId];
			Type domainType = domainInstance.GetType();
			string dataFieldName = DomainHelper.DomainId2DataId2FieldName[(int)indicator.DomainId][(int)indicator.DataId];
			string fieldName = "CacheInfluences" + dataFieldName;
			FieldInfo fieldInfo = domainType.GetField(fieldName, BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.NonPublic);
			bool flag = fieldInfo == null;
			if (flag)
			{
				throw new Exception("Cannot find field " + fieldName + " on class " + domainType.FullName);
			}
			return (DataInfluence[][])fieldInfo.GetValue(null);
		}

		// Token: 0x060081A6 RID: 33190 RVA: 0x004D6EBC File Offset: 0x004D50BC
		private static Dictionary<DataUid, HashSet<DataUid>> CalcDeepInfluences(Dictionary<DataUidWithType, List<DataDependency>> dependencies)
		{
			Dictionary<DataUid, HashSet<DataUid>> influences = new Dictionary<DataUid, HashSet<DataUid>>();
			foreach (KeyValuePair<DataUidWithType, List<DataDependency>> entry in dependencies)
			{
				DataUid targetUid = entry.Key.DataUid;
				List<DataDependency> currDependencies = entry.Value;
				foreach (DataDependency currDependency in currDependencies)
				{
					foreach (DataUid sourceUid in currDependency.SourceUids)
					{
						HashSet<DataUid> targetUids;
						bool flag = !influences.TryGetValue(sourceUid, out targetUids);
						if (flag)
						{
							targetUids = new HashSet<DataUid>();
							influences.Add(sourceUid, targetUids);
						}
						targetUids.Add(targetUid);
					}
				}
			}
			foreach (KeyValuePair<DataUid, HashSet<DataUid>> entry2 in influences)
			{
				DataDependenciesInfo.CalcDeeperInfluences(influences, entry2.Key, entry2.Value, entry2.Value, 0);
			}
			return influences;
		}

		// Token: 0x060081A7 RID: 33191 RVA: 0x004D7020 File Offset: 0x004D5220
		private static void CalcDeeperInfluences(Dictionary<DataUid, HashSet<DataUid>> influences, DataUid sourceUid, HashSet<DataUid> targetUids, HashSet<DataUid> currTargetUids, int depth)
		{
			bool flag = ++depth > 16;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Encountered max depth of influence: ");
				defaultInterpolatedStringHandler.AppendFormatted<DataUid>(sourceUid);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			foreach (DataUid currTargetUid in DataDependenciesInfo.HashSetToArray<DataUid>(currTargetUids))
			{
				HashSet<DataUid> childTargetUids;
				bool flag2 = !influences.TryGetValue(currTargetUid, out childTargetUids);
				if (!flag2)
				{
					targetUids.UnionWith(childTargetUids);
					DataDependenciesInfo.CalcDeeperInfluences(influences, sourceUid, targetUids, childTargetUids, depth);
				}
			}
		}

		// Token: 0x060081A8 RID: 33192 RVA: 0x004D70B8 File Offset: 0x004D52B8
		private static void LogDependencies(Dictionary<DataUidWithType, List<DataDependency>> dependencies)
		{
			StringBuilder message = new StringBuilder();
			message.AppendLine("Dependencies:");
			foreach (KeyValuePair<DataUidWithType, List<DataDependency>> entry in dependencies)
			{
				DataUid sourceUid = entry.Key.DataUid;
				List<DataDependency> currDependencies = entry.Value;
				StringBuilder stringBuilder = message;
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(3, 1, stringBuilder);
				appendInterpolatedStringHandler.AppendLiteral("  ");
				appendInterpolatedStringHandler.AppendFormatted(sourceUid.ToString());
				appendInterpolatedStringHandler.AppendLiteral(":");
				stringBuilder2.AppendLine(ref appendInterpolatedStringHandler);
				foreach (DataDependency dependency in currDependencies)
				{
					stringBuilder = message;
					StringBuilder stringBuilder3 = stringBuilder;
					appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(4, 1, stringBuilder);
					appendInterpolatedStringHandler.AppendLiteral("    ");
					appendInterpolatedStringHandler.AppendFormatted<DataDependency>(dependency);
					stringBuilder3.AppendLine(ref appendInterpolatedStringHandler);
				}
			}
			DataDependenciesInfo.Logger.Debug(message.ToString());
		}

		// Token: 0x060081A9 RID: 33193 RVA: 0x004D71F4 File Offset: 0x004D53F4
		private static void LogDeepInfluences(Dictionary<DataUid, HashSet<DataUid>> influences)
		{
			StringBuilder message = new StringBuilder();
			message.AppendLine("Deep influences:");
			foreach (KeyValuePair<DataUid, HashSet<DataUid>> entry in influences)
			{
				DataUid sourceUid = entry.Key;
				HashSet<DataUid> targetUids = entry.Value;
				StringBuilder stringBuilder = message;
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(3, 1, stringBuilder);
				appendInterpolatedStringHandler.AppendLiteral("  ");
				appendInterpolatedStringHandler.AppendFormatted(sourceUid.ToString());
				appendInterpolatedStringHandler.AppendLiteral(":");
				stringBuilder2.AppendLine(ref appendInterpolatedStringHandler);
				foreach (DataUid targetUid in targetUids)
				{
					stringBuilder = message;
					StringBuilder stringBuilder3 = stringBuilder;
					appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(4, 1, stringBuilder);
					appendInterpolatedStringHandler.AppendLiteral("    ");
					appendInterpolatedStringHandler.AppendFormatted(targetUid.ToString());
					stringBuilder3.AppendLine(ref appendInterpolatedStringHandler);
				}
			}
			DataDependenciesInfo.Logger.Debug(message.ToString());
		}

		// Token: 0x060081AA RID: 33194 RVA: 0x004D7338 File Offset: 0x004D5538
		private static void LogInfluences(Dictionary<DataIndicator, DataInfluence[][]> indicator2FieldId2Influences)
		{
			StringBuilder message = new StringBuilder();
			message.AppendLine("Influences:");
			foreach (KeyValuePair<DataIndicator, DataInfluence[][]> entry in indicator2FieldId2Influences)
			{
				DataIndicator indicator = entry.Key;
				DataInfluence[][] fieldId2Influences = entry.Value;
				StringBuilder stringBuilder = message;
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(3, 1, stringBuilder);
				appendInterpolatedStringHandler.AppendLiteral("  ");
				appendInterpolatedStringHandler.AppendFormatted(indicator.ToString());
				appendInterpolatedStringHandler.AppendLiteral(":");
				stringBuilder2.AppendLine(ref appendInterpolatedStringHandler);
				switch (indicator.DataType)
				{
				case DomainDataType.SingleValue:
				case DomainDataType.SingleValueCollection:
				{
					DataInfluence[] influences = fieldId2Influences[(int)indicator.DataId];
					bool flag = influences == null;
					if (!flag)
					{
						string dataName = DomainHelper.DomainId2DataId2FieldName[(int)indicator.DomainId][(int)indicator.DataId];
						stringBuilder = message;
						StringBuilder stringBuilder3 = stringBuilder;
						appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(5, 1, stringBuilder);
						appendInterpolatedStringHandler.AppendLiteral("    ");
						appendInterpolatedStringHandler.AppendFormatted(dataName);
						appendInterpolatedStringHandler.AppendLiteral(":");
						stringBuilder3.AppendLine(ref appendInterpolatedStringHandler);
						foreach (DataInfluence influence in influences)
						{
							stringBuilder = message;
							StringBuilder stringBuilder4 = stringBuilder;
							appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(6, 1, stringBuilder);
							appendInterpolatedStringHandler.AppendLiteral("      ");
							appendInterpolatedStringHandler.AppendFormatted<DataInfluence>(influence);
							stringBuilder4.AppendLine(ref appendInterpolatedStringHandler);
						}
					}
					break;
				}
				case DomainDataType.ElementList:
				{
					int fieldsCount = fieldId2Influences.Length;
					for (int fieldId = 0; fieldId < fieldsCount; fieldId++)
					{
						DataInfluence[] influences2 = fieldId2Influences[fieldId];
						bool flag2 = influences2 == null;
						if (!flag2)
						{
							stringBuilder = message;
							StringBuilder stringBuilder5 = stringBuilder;
							appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(5, 1, stringBuilder);
							appendInterpolatedStringHandler.AppendLiteral("    ");
							appendInterpolatedStringHandler.AppendFormatted(fieldId.ToString());
							appendInterpolatedStringHandler.AppendLiteral(":");
							stringBuilder5.AppendLine(ref appendInterpolatedStringHandler);
							foreach (DataInfluence influence2 in influences2)
							{
								stringBuilder = message;
								StringBuilder stringBuilder6 = stringBuilder;
								appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(6, 1, stringBuilder);
								appendInterpolatedStringHandler.AppendLiteral("      ");
								appendInterpolatedStringHandler.AppendFormatted<DataInfluence>(influence2);
								stringBuilder6.AppendLine(ref appendInterpolatedStringHandler);
							}
						}
					}
					break;
				}
				case DomainDataType.ObjectCollection:
					goto IL_23A;
				default:
					goto IL_23A;
				}
				continue;
				IL_23A:
				string[] fieldId2FieldName = DomainHelper.DomainId2DataId2ObjectFieldId2FieldName[(int)indicator.DomainId][(int)indicator.DataId];
				int fieldsCount2 = fieldId2Influences.Length;
				for (int fieldId2 = 0; fieldId2 < fieldsCount2; fieldId2++)
				{
					DataInfluence[] influences3 = fieldId2Influences[fieldId2];
					bool flag3 = influences3 == null;
					if (!flag3)
					{
						string fieldName = fieldId2FieldName[fieldId2];
						stringBuilder = message;
						StringBuilder stringBuilder7 = stringBuilder;
						appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(5, 1, stringBuilder);
						appendInterpolatedStringHandler.AppendLiteral("    ");
						appendInterpolatedStringHandler.AppendFormatted(fieldName);
						appendInterpolatedStringHandler.AppendLiteral(":");
						stringBuilder7.AppendLine(ref appendInterpolatedStringHandler);
						foreach (DataInfluence influence3 in influences3)
						{
							stringBuilder = message;
							StringBuilder stringBuilder8 = stringBuilder;
							appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(6, 1, stringBuilder);
							appendInterpolatedStringHandler.AppendLiteral("      ");
							appendInterpolatedStringHandler.AppendFormatted<DataInfluence>(influence3);
							stringBuilder8.AppendLine(ref appendInterpolatedStringHandler);
						}
					}
				}
			}
			DataDependenciesInfo.Logger.Debug(message.ToString());
		}

		// Token: 0x060081AB RID: 33195 RVA: 0x004D76B0 File Offset: 0x004D58B0
		private static T[] HashSetToArray<T>(HashSet<T> items)
		{
			int count = items.Count;
			T[] array = new T[count];
			int id = 0;
			foreach (T item in items)
			{
				array[id] = item;
				id++;
			}
			return array;
		}

		// Token: 0x060081AC RID: 33196 RVA: 0x004D7724 File Offset: 0x004D5924
		private static string GetDomainName(Type domainType)
		{
			string domainTypeShortName = domainType.Name;
			bool flag = !domainTypeShortName.EndsWith("Domain");
			if (flag)
			{
				throw new Exception("Domain name must end with Domain: " + domainType.FullName);
			}
			return domainTypeShortName.Substring(0, domainTypeShortName.Length - "Domain".Length);
		}

		// Token: 0x060081AD RID: 33197 RVA: 0x004D7780 File Offset: 0x004D5980
		private static Type GetObjectCollectionElementType(Type collectionType)
		{
			Type interfaceType = collectionType.GetInterface("System.Collections.Generic.IDictionary`2");
			bool flag = interfaceType == null;
			if (flag)
			{
				throw new Exception("The specified type is not implement IDictionary interface: " + collectionType.FullName);
			}
			Type[] typeArguments = interfaceType.GetGenericArguments();
			return typeArguments[1];
		}

		// Token: 0x060081AE RID: 33198 RVA: 0x004D77CC File Offset: 0x004D59CC
		private static T GetStaticFieldValue<T>(Type classType, string filedName)
		{
			FieldInfo fieldInfo = classType.GetField(filedName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			bool flag = fieldInfo == null;
			if (flag)
			{
				throw new Exception("Failed to get field: " + classType.FullName + "." + filedName);
			}
			return (T)((object)fieldInfo.GetValue(null));
		}

		// Token: 0x060081AF RID: 33199 RVA: 0x004D781C File Offset: 0x004D5A1C
		private static string ToPublicName(string fieldName)
		{
			StringBuilder sb = new StringBuilder(fieldName);
			bool flag = sb[0] == '_';
			if (flag)
			{
				sb.Remove(0, 1);
			}
			sb[0] = char.ToUpper(sb[0]);
			return sb.ToString();
		}

		// Token: 0x040023CD RID: 9165
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x040023CE RID: 9166
		private const string DomainNameSuffix = "Domain";

		// Token: 0x040023CF RID: 9167
		private const string CalcMethodPrefix = "Calc";

		// Token: 0x040023D0 RID: 9168
		private const string CacheInfluencesFieldPrefix = "CacheInfluences";

		// Token: 0x040023D1 RID: 9169
		private const int InfluenceMaxDepth = 16;

		// Token: 0x040023D2 RID: 9170
		private static readonly Assembly Assembly = Assembly.GetExecutingAssembly();

		// Token: 0x040023D3 RID: 9171
		private static readonly Assembly SharedAssembly = typeof(ExternalDataBridge).Assembly;

		// Token: 0x040023D4 RID: 9172
		private static readonly Type BaseDomainType = typeof(BaseGameDataDomain);

		// Token: 0x040023D5 RID: 9173
		private static readonly Type DomainDataAttrType = typeof(DomainDataAttribute);

		// Token: 0x040023D6 RID: 9174
		private static readonly Type CollectionObjectFieldAttrType = typeof(CollectionObjectFieldAttribute);

		// Token: 0x040023D7 RID: 9175
		private static readonly Type DependencyAttrType = typeof(BaseDataDependencyAttribute);
	}
}
