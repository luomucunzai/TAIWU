using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using Config;
using Config.Common;
using GameData.Utilities;
using GameData.Utilities.Mod;
using MoonSharp.Interpreter;
using TaiwuModdingLib.Core.Utils;

namespace GameData.Domains.Mod
{
	// Token: 0x02000651 RID: 1617
	public class ModConfigDataManager
	{
		// Token: 0x06004873 RID: 18547 RVA: 0x0028D1A4 File Offset: 0x0028B3A4
		public void LoadModConfig(ModInfo modInfo)
		{
			string cfgDirPath = Path.Combine(modInfo.DirectoryName, "Config");
			string modIdentifier = modInfo.Title + "(" + modInfo.ModId.ToString();
			bool flag = Directory.Exists(cfgDirPath);
			if (flag)
			{
				foreach (string cfgPath in Directory.GetFiles(cfgDirPath, "*.lua", SearchOption.AllDirectories))
				{
					string text = File.ReadAllText(cfgPath);
					DynValue value = LuaParser.Parse(text, null);
					Table table = (value.Type == DataType.Table) ? value.Table : null;
					ModConfigDataManager.LoadConfigItem(modIdentifier + "-" + cfgPath, table);
				}
			}
		}

		// Token: 0x06004874 RID: 18548 RVA: 0x0028D258 File Offset: 0x0028B458
		internal static void LoadConfigItem(string modIdentifier, Table luaTable)
		{
			bool flag = !ModConfigDataManager.CheckRequiredField(modIdentifier, "ConfigName", luaTable);
			if (!flag)
			{
				string configName = luaTable.Get("ConfigName").String;
				bool flag2 = !ModConfigDataManager.CheckFieldNull(modIdentifier, "ConfigName", configName);
				if (!flag2)
				{
					bool flag3 = !ConfigCollection.NameMap.ContainsKey(configName);
					if (flag3)
					{
						AdaptableLog.Warning(modIdentifier + ": field ConfigName has an invalid value.", true);
					}
					else
					{
						IConfigData configData = ConfigCollection.NameMap[configName];
						bool flag4 = !ModConfigDataManager.CheckRequiredField(modIdentifier, "DestConfigRefName", luaTable);
						if (!flag4)
						{
							string destConfigRefName = luaTable.Get("DestConfigRefName").String;
							bool flag5 = !ModConfigDataManager.CheckFieldNull(modIdentifier, "DestConfigRefName", destConfigRefName);
							if (!flag5)
							{
								bool flag6 = luaTable.ContainsKey("SrcConfigRefName");
								if (!flag6)
								{
									throw new NotImplementedException();
								}
								string srcConfigRefName = luaTable.Get("SrcConfigRefName").String;
								object srcConfigItem = configData.GetConfigItem(srcConfigRefName);
								bool flag7 = srcConfigRefName == destConfigRefName;
								object destConfigItem;
								if (flag7)
								{
									DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 3);
									defaultInterpolatedStringHandler.AppendFormatted(modIdentifier);
									defaultInterpolatedStringHandler.AppendLiteral(" is replacing ");
									defaultInterpolatedStringHandler.AppendFormatted(configName);
									defaultInterpolatedStringHandler.AppendLiteral(" - ");
									defaultInterpolatedStringHandler.AppendFormatted(srcConfigRefName);
									AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
									destConfigItem = srcConfigItem;
								}
								else
								{
									DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 3);
									defaultInterpolatedStringHandler.AppendFormatted(modIdentifier);
									defaultInterpolatedStringHandler.AppendLiteral(" is appending ");
									defaultInterpolatedStringHandler.AppendFormatted(configName);
									defaultInterpolatedStringHandler.AppendLiteral(" - ");
									defaultInterpolatedStringHandler.AppendFormatted(destConfigRefName);
									AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
									destConfigItem = srcConfigItem.CreateDeepCopy();
									int destTemplateId = luaTable.Get("TemplateId").ToObject<int>();
									ReflectionExtensions.ModifyFieldWithTypeConvert<object>(destConfigItem, "TemplateId", destTemplateId, (BindingFlags)(-1));
									configData.AddExtraItem(modIdentifier, destConfigRefName, destConfigItem);
								}
								Table dataTable = luaTable.Get("Data").Table;
								foreach (DynValue keyDynVal in dataTable.Keys)
								{
									string key = keyDynVal.String;
									FieldInfo fieldInfo = destConfigItem.GetType().GetField(key, (BindingFlags)(-1));
									bool flag8 = fieldInfo == null;
									if (flag8)
									{
										AdaptableLog.Warning(key + " cannot be found as a field of " + configName, false);
									}
									else
									{
										object fieldVal;
										bool flag9 = !dataTable.LoadObject(key, fieldInfo.FieldType, out fieldVal);
										if (!flag9)
										{
											ReflectionExtensions.ModifyFieldWithTypeConvert<object>(destConfigItem, key, fieldVal, (BindingFlags)(-1));
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06004875 RID: 18549 RVA: 0x0028D50C File Offset: 0x0028B70C
		private static bool CheckRequiredField(string modIdentifier, string fieldName, Table luaTable)
		{
			bool flag = !luaTable.ContainsKey(fieldName);
			bool result;
			if (flag)
			{
				AdaptableLog.Warning(modIdentifier + ": cannot find the required field ConfigName.", true);
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06004876 RID: 18550 RVA: 0x0028D544 File Offset: 0x0028B744
		private static bool CheckFieldNull(string modIdentifier, string fieldName, string fieldValue)
		{
			bool flag = string.IsNullOrEmpty(fieldValue);
			bool result;
			if (flag)
			{
				AdaptableLog.Warning(modIdentifier + ": the required field ConfigName is null or empty.", true);
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}
	}
}
