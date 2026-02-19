using System;
using System.IO;
using System.Reflection;
using Config;
using Config.Common;
using GameData.Utilities;
using GameData.Utilities.Mod;
using MoonSharp.Interpreter;
using TaiwuModdingLib.Core.Utils;

namespace GameData.Domains.Mod;

public class ModConfigDataManager
{
	public void LoadModConfig(ModInfo modInfo)
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Invalid comparison between Unknown and I4
		string path = Path.Combine(modInfo.DirectoryName, "Config");
		string text = modInfo.Title + "(" + modInfo.ModId.ToString();
		if (Directory.Exists(path))
		{
			string[] files = Directory.GetFiles(path, "*.lua", SearchOption.AllDirectories);
			foreach (string text2 in files)
			{
				string text3 = File.ReadAllText(text2);
				DynValue val = LuaParser.Parse(text3, (Script)null);
				Table luaTable = (((int)val.Type == 6) ? val.Table : null);
				LoadConfigItem(text + "-" + text2, luaTable);
			}
		}
	}

	internal static void LoadConfigItem(string modIdentifier, Table luaTable)
	{
		if (!CheckRequiredField(modIdentifier, "ConfigName", luaTable))
		{
			return;
		}
		string text = luaTable.Get("ConfigName").String;
		if (!CheckFieldNull(modIdentifier, "ConfigName", text))
		{
			return;
		}
		if (!ConfigCollection.NameMap.ContainsKey(text))
		{
			AdaptableLog.Warning(modIdentifier + ": field ConfigName has an invalid value.", appendWarningMessage: true);
			return;
		}
		IConfigData configData = ConfigCollection.NameMap[text];
		if (!CheckRequiredField(modIdentifier, "DestConfigRefName", luaTable))
		{
			return;
		}
		string text2 = luaTable.Get("DestConfigRefName").String;
		if (!CheckFieldNull(modIdentifier, "DestConfigRefName", text2))
		{
			return;
		}
		if (luaTable.ContainsKey("SrcConfigRefName"))
		{
			string text3 = luaTable.Get("SrcConfigRefName").String;
			object configItem = configData.GetConfigItem(text3);
			object obj;
			if (text3 == text2)
			{
				AdaptableLog.Info($"{modIdentifier} is replacing {text} - {text3}");
				obj = configItem;
			}
			else
			{
				AdaptableLog.Info($"{modIdentifier} is appending {text} - {text2}");
				obj = configItem.CreateDeepCopy();
				int num = luaTable.Get("TemplateId").ToObject<int>();
				ReflectionExtensions.ModifyFieldWithTypeConvert<object>(obj, "TemplateId", (object)num, (BindingFlags)(-1));
				configData.AddExtraItem(modIdentifier, text2, obj);
			}
			Table table = luaTable.Get("Data").Table;
			{
				foreach (DynValue key in table.Keys)
				{
					string text4 = key.String;
					FieldInfo field = obj.GetType().GetField(text4, (BindingFlags)(-1));
					object obj2;
					if (field == null)
					{
						AdaptableLog.Warning(text4 + " cannot be found as a field of " + text);
					}
					else if (table.LoadObject(text4, field.FieldType, out obj2))
					{
						ReflectionExtensions.ModifyFieldWithTypeConvert<object>(obj, text4, obj2, (BindingFlags)(-1));
					}
				}
				return;
			}
		}
		throw new NotImplementedException();
	}

	private static bool CheckRequiredField(string modIdentifier, string fieldName, Table luaTable)
	{
		if (!luaTable.ContainsKey(fieldName))
		{
			AdaptableLog.Warning(modIdentifier + ": cannot find the required field ConfigName.", appendWarningMessage: true);
			return false;
		}
		return true;
	}

	private static bool CheckFieldNull(string modIdentifier, string fieldName, string fieldValue)
	{
		if (string.IsNullOrEmpty(fieldValue))
		{
			AdaptableLog.Warning(modIdentifier + ": the required field ConfigName is null or empty.", appendWarningMessage: true);
			return false;
		}
		return true;
	}
}
