using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Dependencies;
using GameData.Domains.TaiwuEvent;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using NLog;
using TaiwuModdingLib.Core.Plugin;

namespace GameData.Domains.Mod;

[GameDataDomain(16)]
public class ModDomain : BaseGameDataDomain
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	private static readonly List<ModId> LoadedMods = new List<ModId>();

	private static readonly Dictionary<string, ModInfo> ModInfoDict = new Dictionary<string, ModInfo>();

	private static readonly Dictionary<ModId, List<TaiwuRemakePlugin>> LoadedPlugins = new Dictionary<ModId, List<TaiwuRemakePlugin>>();

	private static readonly ModConfigDataManager _modConfigDataManager = new ModConfigDataManager();

	private static HashSet<string> _requiredAssemblies;

	private static readonly Dictionary<string, Action<int>> IntDataHandlers = new Dictionary<string, Action<int>>();

	private static readonly Dictionary<string, Action<bool>> BoolDataHandlers = new Dictionary<string, Action<bool>>();

	private static readonly Dictionary<string, Action<float>> FloatDataHandlers = new Dictionary<string, Action<float>>();

	private static readonly Dictionary<string, Action<string>> StringDataHandlers = new Dictionary<string, Action<string>>();

	private static readonly Dictionary<string, Action<SerializableModData>> SerializableModDataHandlers = new Dictionary<string, Action<SerializableModData>>();

	[DomainData(DomainDataType.SingleValueCollection, true, false, false, false)]
	private readonly Dictionary<ModId, SerializableModData> _archiveModDataDict;

	[DomainData(DomainDataType.SingleValueCollection, false, false, false, false)]
	private readonly Dictionary<ModId, SerializableModData> _nonArchiveModDataDict;

	private static readonly Dictionary<string, Action<DataContext>> ModMethods = new Dictionary<string, Action<DataContext>>();

	private static readonly Dictionary<string, Action<DataContext, SerializableModData>> ModMethodsWithParam = new Dictionary<string, Action<DataContext, SerializableModData>>();

	private static readonly Dictionary<string, Func<DataContext, SerializableModData>> ModMethodsWithRet = new Dictionary<string, Func<DataContext, SerializableModData>>();

	private static readonly Dictionary<string, Func<DataContext, SerializableModData, SerializableModData>> ModMethodsWithParamAndRet = new Dictionary<string, Func<DataContext, SerializableModData, SerializableModData>>();

	private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[2][];

	private Queue<uint> _pendingLoadingOperationIds;

	public void LoadAllMods(ModInfoList modInfoList)
	{
		UnloadAllMods();
		ModInfoDict.Clear();
		if (modInfoList.Items == null)
		{
			return;
		}
		foreach (ModInfo item in modInfoList.Items)
		{
			ModInfoDict.Add(item.ModId.ToString(), item);
			try
			{
				LoadMod(item);
			}
			catch (Exception ex)
			{
				string path = Path.Combine(GameData.ArchiveData.Common.ArchiveBaseDir, "ModSettings.Lua");
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				throw new Exception("Loading - " + item.Title + "\n" + ex);
			}
		}
	}

	private void UnloadAllMods()
	{
		for (int num = LoadedMods.Count - 1; num >= 0; num--)
		{
			UnloadMod(num);
		}
		LoadedPlugins.Clear();
		LoadedMods.Clear();
		ClearDataHandlers();
		ClearModMethods();
	}

	public void LoadAllEventPackages()
	{
		foreach (ModId loadedMod in LoadedMods)
		{
			ModInfo modInfo = ModInfoDict[loadedMod.ToString()];
			string rootPath = Path.Combine(modInfo.DirectoryName, "Events");
			EventPackagePathInfo pathInfo = new EventPackagePathInfo(rootPath);
			foreach (string eventPackage in modInfo.EventPackages)
			{
				Logger.Info(" - Loading events from " + eventPackage);
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(eventPackage);
				string dllFilePath = Path.Combine(pathInfo.DllDirPath, eventPackage);
				DomainManager.TaiwuEvent.LoadEventPackageFromAssembly(fileNameWithoutExtension, pathInfo, loadedMod.ToString(), dllFilePath);
			}
		}
	}

	private void LoadMod(ModInfo modInfo)
	{
		Logger.Info("Start loading mod " + modInfo.Title + " for GameData ...");
		if (LoadedPlugins.ContainsKey(modInfo.ModId))
		{
			throw new Exception($"Mod with FileId {modInfo.ModId} is already loaded.");
		}
		string text = Path.Combine(modInfo.DirectoryName, "Plugins");
		List<TaiwuRemakePlugin> list = new List<TaiwuRemakePlugin>();
		LoadedPlugins.Add(modInfo.ModId, list);
		foreach (string backendPlugin in modInfo.BackendPlugins)
		{
			Logger.Info(" - Loading plugin from " + backendPlugin);
			TaiwuRemakePlugin val = PluginHelper.LoadPlugin(text, backendPlugin, modInfo.ModId.ToString());
			val.OnModSettingUpdate();
			list.Add(val);
		}
		_modConfigDataManager.LoadModConfig(modInfo);
		LoadedMods.Add(modInfo.ModId);
	}

	private void UnloadMod(int index)
	{
		ModInfo modInfo = ModInfoDict[LoadedMods[index].ToString()];
		Logger.Info("Start unloading mod " + modInfo.Title + " for GameData ...");
		List<TaiwuRemakePlugin> list = LoadedPlugins[modInfo.ModId];
		for (int num = list.Count - 1; num >= 0; num--)
		{
			Logger.Info(" - Unloading plugin from " + modInfo.BackendPlugins[num]);
			list[num].Dispose();
			list.RemoveAt(num);
		}
		LoadedPlugins.Remove(modInfo.ModId);
		LoadedMods.RemoveAt(index);
	}

	public static List<ModId> GetLoadedModIds()
	{
		return LoadedMods.ToList();
	}

	public string GetModDirectory(string modIdStr)
	{
		if (!ModInfoDict.TryGetValue(modIdStr, out var value))
		{
			return string.Empty;
		}
		return value.DirectoryName;
	}

	public string GetModTitle(string modIdStr)
	{
		if (!ModInfoDict.TryGetValue(modIdStr, out var value))
		{
			return string.Empty;
		}
		return value.Title;
	}

	private void OnInitializedDomainData()
	{
	}

	private void InitializeOnInitializeGameDataModule()
	{
	}

	private void InitializeOnEnterNewWorld()
	{
		_archiveModDataDict.Clear();
		_nonArchiveModDataDict.Clear();
		foreach (ModId loadedMod in LoadedMods)
		{
			_archiveModDataDict.Add(loadedMod, new SerializableModData());
			_nonArchiveModDataDict.Add(loadedMod, new SerializableModData());
			List<TaiwuRemakePlugin> list = LoadedPlugins[loadedMod];
			foreach (TaiwuRemakePlugin item in list)
			{
				item.OnEnterNewWorld();
			}
		}
	}

	private void OnLoadedArchiveData()
	{
		DataContext currentThreadDataContext = DataContextManager.GetCurrentThreadDataContext();
		foreach (ModId loadedMod in LoadedMods)
		{
			if (!_archiveModDataDict.ContainsKey(loadedMod))
			{
				AddElement_ArchiveModDataDict(loadedMod, new SerializableModData(), currentThreadDataContext);
			}
			if (!_nonArchiveModDataDict.ContainsKey(loadedMod))
			{
				AddElement_NonArchiveModDataDict(loadedMod, new SerializableModData(), currentThreadDataContext);
			}
			List<TaiwuRemakePlugin> list = LoadedPlugins[loadedMod];
			foreach (TaiwuRemakePlugin item in list)
			{
				item.OnLoadedArchiveData();
			}
		}
	}

	public static Assembly ResolveTaiwuModdingLibPath(object sender, ResolveEventArgs args)
	{
		AssemblyName assemblyName = new AssemblyName(args.Name);
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		Assembly assembly = assemblies.FirstOrDefault((Assembly assembly3) => assemblyName.FullName == assembly3.FullName);
		if (assembly != null)
		{
			return assembly;
		}
		if (_requiredAssemblies == null)
		{
			_requiredAssemblies = new HashSet<string> { "0Harmony", "Mono.Cecil", "Mono.Cecil.Mdb", "Mono.Cecil.Pdb", "Mono.Cecil.Rocks", "MonoMod.RuntimeDetour", "MonoMod.Utils", "TaiwuModdingLib", "Newtonsoft.Json", "Steamworks.NET" };
		}
		if (!_requiredAssemblies.Contains(assemblyName.Name))
		{
			return null;
		}
		string text = Path.Combine(Program.BaseDataDir, "The Scroll of Taiwu_Data\\Managed\\" + assemblyName.Name + ".dll");
		if (!File.Exists(text))
		{
			Logger.Warn("Failed to resolve assembly because no such file can be found: " + text + ".");
			return null;
		}
		Assembly assembly2 = Assembly.LoadFile(text);
		AssemblyName name = assembly2.GetName();
		if (name.Name != assemblyName.Name)
		{
			throw new Exception($"Unexpected assembly name info: looking for {assemblyName.Name} => {name.Name} loaded.");
		}
		if (name.FullName != assemblyName.FullName)
		{
			Logger.Warn($"Unexpected assembly Fullname info: looking for {assemblyName.FullName} => {name.FullName} loaded.");
		}
		return assembly2;
	}

	private void ClearDataHandlers()
	{
		IntDataHandlers.Clear();
		BoolDataHandlers.Clear();
		FloatDataHandlers.Clear();
		StringDataHandlers.Clear();
		SerializableModDataHandlers.Clear();
	}

	public void AddOnReceiveDataHandler(string modIdStr, string dataName, Action<int> handler)
	{
		IntDataHandlers.Add(modIdStr + "." + dataName, handler);
	}

	public void AddOnReceiveDataHandler(string modIdStr, string dataName, Action<bool> handler)
	{
		BoolDataHandlers.Add(modIdStr + "." + dataName, handler);
	}

	public void AddOnReceiveDataHandler(string modIdStr, string dataName, Action<float> handler)
	{
		FloatDataHandlers.Add(modIdStr + "." + dataName, handler);
	}

	public void AddOnReceiveDataHandler(string modIdStr, string dataName, Action<string> handler)
	{
		StringDataHandlers.Add(modIdStr + "." + dataName, handler);
	}

	public void AddOnReceiveDataHandler(string modIdStr, string dataName, Action<SerializableModData> handler)
	{
		SerializableModDataHandlers.Add(modIdStr + "." + dataName, handler);
	}

	private SerializableModData GetModData(string modIdStr, bool isArchive)
	{
		return isArchive ? _archiveModDataDict[ModInfoDict[modIdStr].ModId] : _nonArchiveModDataDict[ModInfoDict[modIdStr].ModId];
	}

	public void OfflineSetInt(string modIdStr, string dataName, bool isArchive, int val)
	{
		GetModData(modIdStr, isArchive).Set(dataName, val);
	}

	public void OfflineSetFloat(string modIdStr, string dataName, bool isArchive, float val)
	{
		GetModData(modIdStr, isArchive).Set(dataName, val);
	}

	public void OfflineSetBool(string modIdStr, string dataName, bool isArchive, bool val)
	{
		GetModData(modIdStr, isArchive).Set(dataName, val);
	}

	public void OfflineSetString(string modIdStr, string dataName, bool isArchive, string val)
	{
		GetModData(modIdStr, isArchive).Set(dataName, val);
	}

	public void OfflineSetSerializableModData(string modIdStr, string dataName, bool isArchive, SerializableModData val)
	{
		GetModData(modIdStr, isArchive).Set(dataName, val);
	}

	public void SetModData(DataContext context, string modIdStr, bool isArchive)
	{
		ModId modId = ModInfoDict[modIdStr].ModId;
		if (isArchive)
		{
			SerializableModData value = _archiveModDataDict[modId];
			SetElement_ArchiveModDataDict(modId, value, context);
		}
		else
		{
			SerializableModData value2 = _nonArchiveModDataDict[modId];
			SetElement_NonArchiveModDataDict(modId, value2, context);
		}
	}

	[DomainMethod]
	public bool SetInt(DataContext context, string modIdStr, string dataName, bool isArchive, int val)
	{
		GetModData(modIdStr, isArchive).Set(dataName, val);
		SetModData(context, modIdStr, isArchive);
		if (IntDataHandlers.TryGetValue(modIdStr + "." + dataName, out var value))
		{
			value(val);
			return true;
		}
		return false;
	}

	[DomainMethod]
	public bool SetBool(DataContext context, string modIdStr, string dataName, bool isArchive, bool val)
	{
		GetModData(modIdStr, isArchive).Set(dataName, val);
		SetModData(context, modIdStr, isArchive);
		if (BoolDataHandlers.TryGetValue(modIdStr + "." + dataName, out var value))
		{
			value(val);
			return true;
		}
		return false;
	}

	public bool SetFloat(DataContext context, string modIdStr, string dataName, bool isArchive, float val)
	{
		GetModData(modIdStr, isArchive).Set(dataName, val);
		SetModData(context, modIdStr, isArchive);
		if (FloatDataHandlers.TryGetValue(modIdStr + "." + dataName, out var value))
		{
			value(val);
			return true;
		}
		return false;
	}

	[DomainMethod]
	public bool SetString(DataContext context, string modIdStr, string dataName, bool isArchive, string val)
	{
		GetModData(modIdStr, isArchive).Set(dataName, val);
		SetModData(context, modIdStr, isArchive);
		if (StringDataHandlers.TryGetValue(modIdStr + "." + dataName, out var value))
		{
			value(val);
			return true;
		}
		return false;
	}

	[DomainMethod]
	public bool SetSerializableModData(DataContext context, string modIdStr, string dataName, bool isArchive, SerializableModData val)
	{
		GetModData(modIdStr, isArchive).Set(dataName, val);
		SetModData(context, modIdStr, isArchive);
		if (SerializableModDataHandlers.TryGetValue(modIdStr + "." + dataName, out var value))
		{
			value(val);
			return true;
		}
		return false;
	}

	public bool SetISerializableGameData<T>(DataContext context, string modIdStr, string dataName, bool isArchive, T val) where T : ISerializableGameData
	{
		GetModData(modIdStr, isArchive).Set(dataName, val);
		SetModData(context, modIdStr, isArchive);
		return false;
	}

	[DomainMethod]
	public int GetInt(string modIdStr, string dataName, bool isArchive)
	{
		if (GetModData(modIdStr, isArchive).Get(dataName, out int val))
		{
			return val;
		}
		throw new ArgumentOutOfRangeException("dataName", "Failed to find mod data " + dataName + " of type int");
	}

	[DomainMethod]
	public bool GetBool(string modIdStr, string dataName, bool isArchive)
	{
		if (GetModData(modIdStr, isArchive).Get(dataName, out bool val))
		{
			return val;
		}
		throw new ArgumentOutOfRangeException("dataName", "Failed to find mod data " + dataName + " of type bool");
	}

	[DomainMethod]
	public string GetString(string modIdStr, string dataName, bool isArchive)
	{
		if (GetModData(modIdStr, isArchive).Get(dataName, out string val))
		{
			return val;
		}
		throw new ArgumentOutOfRangeException("dataName", "Failed to find mod data " + dataName + " of type string");
	}

	[DomainMethod]
	public SerializableModData GetSerializableModData(string modIdStr, string dataName, bool isArchive)
	{
		if (GetModData(modIdStr, isArchive).Get(dataName, out SerializableModData serializableGameData))
		{
			return serializableGameData;
		}
		throw new ArgumentOutOfRangeException("dataName", "Failed to find SerializableModData with name " + dataName);
	}

	public bool TryGet(string modIdStr, string dataName, bool isArchive, out int val)
	{
		return GetModData(modIdStr, isArchive).Get(dataName, out val);
	}

	public bool TryGet(string modIdStr, string dataName, bool isArchive, out bool val)
	{
		return GetModData(modIdStr, isArchive).Get(dataName, out val);
	}

	public bool TryGet(string modIdStr, string dataName, bool isArchive, out float val)
	{
		return GetModData(modIdStr, isArchive).Get(dataName, out val);
	}

	public bool TryGet(string modIdStr, string dataName, bool isArchive, out string val)
	{
		return GetModData(modIdStr, isArchive).Get(dataName, out val);
	}

	public bool TryGet(string modIdStr, string dataName, bool isArchive, out SerializableModData val)
	{
		return GetModData(modIdStr, isArchive).Get(dataName, out val);
	}

	public bool TryGet<T>(string modIdStr, string dataName, bool isArchive, out T val) where T : ISerializableGameData
	{
		return GetModData(modIdStr, isArchive).Get(dataName, out val);
	}

	public bool RemoveInt(DataContext context, string modIdStr, string dataName, bool isArchive)
	{
		bool result = GetModData(modIdStr, isArchive).RemoveInt(dataName);
		SetModData(context, modIdStr, isArchive);
		return result;
	}

	public bool RemoveBool(DataContext context, string modIdStr, string dataName, bool isArchive)
	{
		bool result = GetModData(modIdStr, isArchive).RemoveBool(dataName);
		SetModData(context, modIdStr, isArchive);
		return result;
	}

	public bool RemoveFloat(DataContext context, string modIdStr, string dataName, bool isArchive)
	{
		bool result = GetModData(modIdStr, isArchive).RemoveFloat(dataName);
		SetModData(context, modIdStr, isArchive);
		return result;
	}

	public bool RemoveString(DataContext context, string modIdStr, string dataName, bool isArchive)
	{
		bool result = GetModData(modIdStr, isArchive).RemoveString(dataName);
		SetModData(context, modIdStr, isArchive);
		return result;
	}

	public bool RemoveSerializableModData(DataContext context, string modIdStr, string dataName, bool isArchive)
	{
		bool result = GetModData(modIdStr, isArchive).RemoveObject(dataName);
		SetModData(context, modIdStr, isArchive);
		return result;
	}

	public bool RemoveISerializableGameData(DataContext context, string modIdStr, string dataName, bool isArchive)
	{
		bool result = GetModData(modIdStr, isArchive).RemoveObject(dataName);
		SetModData(context, modIdStr, isArchive);
		return result;
	}

	public void RemoveData(DataContext context, string modIdStr, string dataName)
	{
		GetModData(modIdStr, isArchive: true).Remove(dataName);
		SetModData(context, modIdStr, isArchive: true);
		GetModData(modIdStr, isArchive: false).Remove(dataName);
		SetModData(context, modIdStr, isArchive: false);
	}

	private void ClearModMethods()
	{
		ModMethods.Clear();
		ModMethodsWithParam.Clear();
		ModMethodsWithRet.Clear();
		ModMethodsWithParamAndRet.Clear();
	}

	[DomainMethod]
	public void CallModMethod(DataContext context, string modIdStr, string methodName)
	{
		if (ModMethods.TryGetValue(modIdStr + ".Method." + methodName, out var value))
		{
			value(context);
			return;
		}
		string modTitle = GetModTitle(modIdStr);
		Logger.AppendWarning($"Unable to call method {methodName} of mod {modTitle}({modIdStr}).");
	}

	[DomainMethod]
	public void CallModMethodWithParam(DataContext context, string modIdStr, string methodName, SerializableModData parameter)
	{
		if (ModMethodsWithParam.TryGetValue(modIdStr + ".Method." + methodName, out var value))
		{
			value(context, parameter);
			return;
		}
		string modTitle = GetModTitle(modIdStr);
		Logger.AppendWarning($"Unable to call method {methodName} of mod {modTitle}({modIdStr}) with parameter.");
	}

	[DomainMethod]
	public SerializableModData CallModMethodWithRet(DataContext context, string modIdStr, string methodName)
	{
		if (ModMethodsWithRet.TryGetValue(modIdStr + ".Method." + methodName, out var value))
		{
			return value(context);
		}
		string modTitle = GetModTitle(modIdStr);
		Logger.AppendWarning($"Unable to call method {methodName} of mod {modTitle}({modIdStr}) with return value.");
		return new SerializableModData();
	}

	[DomainMethod]
	public SerializableModData CallModMethodWithParamAndRet(DataContext context, string modIdStr, string methodName, SerializableModData parameter)
	{
		if (ModMethodsWithParamAndRet.TryGetValue(modIdStr + ".Method." + methodName, out var value))
		{
			return value(context, parameter);
		}
		string modTitle = GetModTitle(modIdStr);
		Logger.AppendWarning($"Unable to call method {methodName} of mod {modTitle}({modIdStr}) with parameter and return value.");
		return new SerializableModData();
	}

	public void AddModMethod(string modIdStr, string methodName, Func<DataContext, SerializableModData, SerializableModData> method)
	{
		ModMethodsWithParamAndRet.Add(modIdStr + ".Method." + methodName, method);
	}

	public void AddModMethod(string modIdStr, string methodName, Func<DataContext, SerializableModData> method)
	{
		ModMethodsWithRet.Add(modIdStr + ".Method." + methodName, method);
	}

	public void AddModMethod(string modIdStr, string methodName, Action<DataContext, SerializableModData> method)
	{
		ModMethodsWithParam.Add(modIdStr + ".Method." + methodName, method);
	}

	public void AddModMethod(string modIdStr, string methodName, Action<DataContext> method)
	{
		ModMethods.Add(modIdStr + ".Method." + methodName, method);
	}

	public void AddModDisplayEvent(string modIdStr, string customData)
	{
		GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.ModDisplayEvent, modIdStr, customData);
	}

	[DomainMethod]
	public void UpdateModSettings(DataContext context, ModId modId, SerializableModData modData)
	{
		ModInfoDict[modId.ToString()].ModSettings = modData;
		foreach (TaiwuRemakePlugin item in LoadedPlugins[modId])
		{
			item.OnModSettingUpdate();
		}
	}

	public bool GetSetting(string modIdStr, string settingName, ref int val)
	{
		ModInfo value;
		return ModInfoDict.TryGetValue(modIdStr, out value) && value.ModSettings.Get(settingName, out val);
	}

	public bool GetSetting(string modIdStr, string settingName, ref float val)
	{
		ModInfo value;
		return ModInfoDict.TryGetValue(modIdStr, out value) && value.ModSettings.Get(settingName, out val);
	}

	public bool GetSetting(string modIdStr, string settingName, ref bool val)
	{
		ModInfo value;
		return ModInfoDict.TryGetValue(modIdStr, out value) && value.ModSettings.Get(settingName, out val);
	}

	public bool GetSetting(string modIdStr, string settingName, ref string val)
	{
		ModInfo value;
		return ModInfoDict.TryGetValue(modIdStr, out value) && value.ModSettings.Get(settingName, out val);
	}

	public ModDomain()
		: base(2)
	{
		_archiveModDataDict = new Dictionary<ModId, SerializableModData>(0);
		_nonArchiveModDataDict = new Dictionary<ModId, SerializableModData>(0);
		OnInitializedDomainData();
	}

	private SerializableModData GetElement_ArchiveModDataDict(ModId elementId)
	{
		return _archiveModDataDict[elementId];
	}

	private bool TryGetElement_ArchiveModDataDict(ModId elementId, out SerializableModData value)
	{
		return _archiveModDataDict.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_ArchiveModDataDict(ModId elementId, SerializableModData value, DataContext context)
	{
		_archiveModDataDict.Add(elementId, value);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(16, 0, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(16, 0, elementId, 0);
		}
	}

	private unsafe void SetElement_ArchiveModDataDict(ModId elementId, SerializableModData value, DataContext context)
	{
		_archiveModDataDict[elementId] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(16, 0, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(16, 0, elementId, 0);
		}
	}

	private void RemoveElement_ArchiveModDataDict(ModId elementId, DataContext context)
	{
		_archiveModDataDict.Remove(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(16, 0, elementId);
	}

	private void ClearArchiveModDataDict(DataContext context)
	{
		_archiveModDataDict.Clear();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(16, 0);
	}

	private SerializableModData GetElement_NonArchiveModDataDict(ModId elementId)
	{
		return _nonArchiveModDataDict[elementId];
	}

	private bool TryGetElement_NonArchiveModDataDict(ModId elementId, out SerializableModData value)
	{
		return _nonArchiveModDataDict.TryGetValue(elementId, out value);
	}

	private void AddElement_NonArchiveModDataDict(ModId elementId, SerializableModData value, DataContext context)
	{
		_nonArchiveModDataDict.Add(elementId, value);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, DataStates, CacheInfluences, context);
	}

	private void SetElement_NonArchiveModDataDict(ModId elementId, SerializableModData value, DataContext context)
	{
		_nonArchiveModDataDict[elementId] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, DataStates, CacheInfluences, context);
	}

	private void RemoveElement_NonArchiveModDataDict(ModId elementId, DataContext context)
	{
		_nonArchiveModDataDict.Remove(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, DataStates, CacheInfluences, context);
	}

	private void ClearNonArchiveModDataDict(DataContext context)
	{
		_nonArchiveModDataDict.Clear();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, DataStates, CacheInfluences, context);
	}

	public override void OnInitializeGameDataModule()
	{
		InitializeOnInitializeGameDataModule();
	}

	public unsafe override void OnEnterNewWorld()
	{
		InitializeOnEnterNewWorld();
		InitializeInternalDataOfCollections();
		foreach (KeyValuePair<ModId, SerializableModData> item in _archiveModDataDict)
		{
			ModId key = item.Key;
			SerializableModData value = item.Value;
			if (value != null)
			{
				int serializedSize = value.GetSerializedSize();
				byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(16, 0, key, serializedSize);
				ptr += value.Serialize(ptr);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(16, 0, key, 0);
			}
		}
	}

	public override void OnLoadWorld()
	{
		_pendingLoadingOperationIds = new Queue<uint>();
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(16, 0));
	}

	public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
	{
		switch (dataId)
		{
		case 0:
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		case 1:
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		switch (dataId)
		{
		case 0:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 1:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override int CallMethod(Operation operation, RawDataPool argDataPool, RawDataPool returnDataPool, DataContext context)
	{
		int argsOffset = operation.ArgsOffset;
		switch (operation.MethodId)
		{
		case 0:
		{
			int argsCount11 = operation.ArgsCount;
			int num11 = argsCount11;
			if (num11 == 4)
			{
				string item38 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item38);
				string item39 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item39);
				bool item40 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item40);
				int item41 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item41);
				bool item42 = SetInt(context, item38, item39, item40, item41);
				return GameData.Serializer.Serializer.Serialize(item42, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 1:
		{
			int argsCount6 = operation.ArgsCount;
			int num6 = argsCount6;
			if (num6 == 4)
			{
				string item20 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item20);
				string item21 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item21);
				bool item22 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item22);
				bool item23 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item23);
				bool item24 = SetBool(context, item20, item21, item22, item23);
				return GameData.Serializer.Serializer.Serialize(item24, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 2:
		{
			int argsCount12 = operation.ArgsCount;
			int num12 = argsCount12;
			if (num12 == 4)
			{
				string item43 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item43);
				string item44 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item44);
				bool item45 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item45);
				string item46 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item46);
				bool item47 = SetString(context, item43, item44, item45, item46);
				return GameData.Serializer.Serializer.Serialize(item47, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 3:
		{
			int argsCount3 = operation.ArgsCount;
			int num3 = argsCount3;
			if (num3 == 4)
			{
				string item9 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item9);
				string item10 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item10);
				bool item11 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item11);
				SerializableModData item12 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item12);
				bool item13 = SetSerializableModData(context, item9, item10, item11, item12);
				return GameData.Serializer.Serializer.Serialize(item13, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 4:
		{
			int argsCount8 = operation.ArgsCount;
			int num8 = argsCount8;
			if (num8 == 3)
			{
				string item28 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item28);
				string item29 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item29);
				bool item30 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item30);
				int item31 = GetInt(item28, item29, item30);
				return GameData.Serializer.Serializer.Serialize(item31, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 5:
		{
			int argsCount2 = operation.ArgsCount;
			int num2 = argsCount2;
			if (num2 == 3)
			{
				string item5 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item5);
				string item6 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item6);
				bool item7 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item7);
				bool item8 = GetBool(item5, item6, item7);
				return GameData.Serializer.Serializer.Serialize(item8, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 6:
		{
			int argsCount9 = operation.ArgsCount;
			int num9 = argsCount9;
			if (num9 == 3)
			{
				string item32 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item32);
				string item33 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item33);
				bool item34 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item34);
				string item35 = GetString(item32, item33, item34);
				return GameData.Serializer.Serializer.Serialize(item35, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 7:
		{
			int argsCount5 = operation.ArgsCount;
			int num5 = argsCount5;
			if (num5 == 3)
			{
				string item17 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item17);
				string item18 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item18);
				bool item19 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item19);
				SerializableModData serializableModData = GetSerializableModData(item17, item18, item19);
				return GameData.Serializer.Serializer.Serialize(serializableModData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 8:
		{
			int argsCount13 = operation.ArgsCount;
			int num13 = argsCount13;
			if (num13 == 2)
			{
				ModId item48 = default(ModId);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item48);
				SerializableModData item49 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item49);
				UpdateModSettings(context, item48, item49);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 9:
		{
			int argsCount10 = operation.ArgsCount;
			int num10 = argsCount10;
			if (num10 == 2)
			{
				string item36 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item36);
				string item37 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item37);
				CallModMethod(context, item36, item37);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 10:
		{
			int argsCount7 = operation.ArgsCount;
			int num7 = argsCount7;
			if (num7 == 3)
			{
				string item25 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item25);
				string item26 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item26);
				SerializableModData item27 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item27);
				CallModMethodWithParam(context, item25, item26, item27);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 11:
		{
			int argsCount4 = operation.ArgsCount;
			int num4 = argsCount4;
			if (num4 == 2)
			{
				string item14 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item14);
				string item15 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item15);
				SerializableModData item16 = CallModMethodWithRet(context, item14, item15);
				return GameData.Serializer.Serializer.Serialize(item16, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 12:
		{
			int argsCount = operation.ArgsCount;
			int num = argsCount;
			if (num == 3)
			{
				string item = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item);
				string item2 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item2);
				SerializableModData item3 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item3);
				SerializableModData item4 = CallModMethodWithParamAndRet(context, item, item2, item3);
				return GameData.Serializer.Serializer.Serialize(item4, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public override void OnMonitorData(ushort dataId, ulong subId0, uint subId1, bool monitoring)
	{
		ushort num = dataId;
		ushort num2 = num;
		if (num2 == 0 || num2 == 1)
		{
			return;
		}
		throw new Exception($"Unsupported dataId {dataId}");
	}

	public override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
	{
		switch (dataId)
		{
		case 0:
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		case 1:
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
	{
		switch (dataId)
		{
		case 0:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		case 1:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
	{
		switch (dataId)
		{
		case 0:
			throw new Exception($"Not allow to verify modification state of dataId {dataId}");
		case 1:
			throw new Exception($"Not allow to verify modification state of dataId {dataId}");
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
	{
		ushort dataId = influence.TargetIndicator.DataId;
		ushort num = dataId;
		if (num != 0 && num != 1)
		{
			throw new Exception($"Unsupported dataId {influence.TargetIndicator.DataId}");
		}
		throw new Exception($"Cannot invalidate cache state of non-cache data {influence.TargetIndicator.DataId}");
	}

	public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
	{
		switch (operation.DataId)
		{
		case 0:
		{
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _archiveModDataDict);
			if (_pendingLoadingOperationIds == null)
			{
				break;
			}
			uint num = _pendingLoadingOperationIds.Peek();
			if (num == operation.Id)
			{
				_pendingLoadingOperationIds.Dequeue();
				if (_pendingLoadingOperationIds.Count <= 0)
				{
					_pendingLoadingOperationIds = null;
					InitializeInternalDataOfCollections();
					OnLoadedArchiveData();
					DomainManager.Global.CompleteLoading(16);
				}
			}
			break;
		}
		default:
			throw new Exception($"Unsupported dataId {operation.DataId}");
		case 1:
			throw new Exception($"Cannot process archive response of non-archive data {operation.DataId}");
		}
	}

	private void InitializeInternalDataOfCollections()
	{
	}
}
