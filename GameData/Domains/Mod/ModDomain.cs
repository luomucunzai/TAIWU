using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Dependencies;
using GameData.Domains.TaiwuEvent;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using NLog;
using TaiwuModdingLib.Core.Plugin;

namespace GameData.Domains.Mod
{
	// Token: 0x02000652 RID: 1618
	[GameDataDomain(16)]
	public class ModDomain : BaseGameDataDomain
	{
		// Token: 0x06004878 RID: 18552 RVA: 0x0028D584 File Offset: 0x0028B784
		public void LoadAllMods(ModInfoList modInfoList)
		{
			this.UnloadAllMods();
			ModDomain.ModInfoDict.Clear();
			bool flag = modInfoList.Items == null;
			if (!flag)
			{
				foreach (ModInfo modInfo in modInfoList.Items)
				{
					ModDomain.ModInfoDict.Add(modInfo.ModId.ToString(), modInfo);
					try
					{
						this.LoadMod(modInfo);
					}
					catch (Exception e)
					{
						string path = Path.Combine(Common.ArchiveBaseDir, "ModSettings.Lua");
						bool flag2 = File.Exists(path);
						if (flag2)
						{
							File.Delete(path);
						}
						string str = "Loading - ";
						string title = modInfo.Title;
						string str2 = "\n";
						Exception ex = e;
						throw new Exception(str + title + str2 + ((ex != null) ? ex.ToString() : null));
					}
				}
			}
		}

		// Token: 0x06004879 RID: 18553 RVA: 0x0028D684 File Offset: 0x0028B884
		private void UnloadAllMods()
		{
			for (int i = ModDomain.LoadedMods.Count - 1; i >= 0; i--)
			{
				this.UnloadMod(i);
			}
			ModDomain.LoadedPlugins.Clear();
			ModDomain.LoadedMods.Clear();
			this.ClearDataHandlers();
			this.ClearModMethods();
		}

		// Token: 0x0600487A RID: 18554 RVA: 0x0028D6E0 File Offset: 0x0028B8E0
		public void LoadAllEventPackages()
		{
			foreach (ModId modId in ModDomain.LoadedMods)
			{
				ModInfo modInfo = ModDomain.ModInfoDict[modId.ToString()];
				string eventsDir = Path.Combine(modInfo.DirectoryName, "Events");
				EventPackagePathInfo pathInfo = new EventPackagePathInfo(eventsDir);
				foreach (string eventPackageName in modInfo.EventPackages)
				{
					ModDomain.Logger.Info(" - Loading events from " + eventPackageName);
					string packageName = Path.GetFileNameWithoutExtension(eventPackageName);
					string dllPath = Path.Combine(pathInfo.DllDirPath, eventPackageName);
					DomainManager.TaiwuEvent.LoadEventPackageFromAssembly(packageName, pathInfo, modId.ToString(), dllPath);
				}
			}
		}

		// Token: 0x0600487B RID: 18555 RVA: 0x0028D7F8 File Offset: 0x0028B9F8
		private void LoadMod(ModInfo modInfo)
		{
			ModDomain.Logger.Info("Start loading mod " + modInfo.Title + " for GameData ...");
			bool flag = ModDomain.LoadedPlugins.ContainsKey(modInfo.ModId);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Mod with FileId ");
				defaultInterpolatedStringHandler.AppendFormatted<ModId>(modInfo.ModId);
				defaultInterpolatedStringHandler.AppendLiteral(" is already loaded.");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			string pluginDir = Path.Combine(modInfo.DirectoryName, "Plugins");
			List<TaiwuRemakePlugin> plugins = new List<TaiwuRemakePlugin>();
			ModDomain.LoadedPlugins.Add(modInfo.ModId, plugins);
			foreach (string pluginPath in modInfo.BackendPlugins)
			{
				ModDomain.Logger.Info(" - Loading plugin from " + pluginPath);
				TaiwuRemakePlugin pluginInstance = PluginHelper.LoadPlugin(pluginDir, pluginPath, modInfo.ModId.ToString());
				pluginInstance.OnModSettingUpdate();
				plugins.Add(pluginInstance);
			}
			ModDomain._modConfigDataManager.LoadModConfig(modInfo);
			ModDomain.LoadedMods.Add(modInfo.ModId);
		}

		// Token: 0x0600487C RID: 18556 RVA: 0x0028D948 File Offset: 0x0028BB48
		private void UnloadMod(int index)
		{
			ModInfo modInfo = ModDomain.ModInfoDict[ModDomain.LoadedMods[index].ToString()];
			ModDomain.Logger.Info("Start unloading mod " + modInfo.Title + " for GameData ...");
			List<TaiwuRemakePlugin> plugins = ModDomain.LoadedPlugins[modInfo.ModId];
			for (int i = plugins.Count - 1; i >= 0; i--)
			{
				ModDomain.Logger.Info(" - Unloading plugin from " + modInfo.BackendPlugins[i]);
				plugins[i].Dispose();
				plugins.RemoveAt(i);
			}
			ModDomain.LoadedPlugins.Remove(modInfo.ModId);
			ModDomain.LoadedMods.RemoveAt(index);
		}

		// Token: 0x0600487D RID: 18557 RVA: 0x0028DA1C File Offset: 0x0028BC1C
		public static List<ModId> GetLoadedModIds()
		{
			return ModDomain.LoadedMods.ToList<ModId>();
		}

		// Token: 0x0600487E RID: 18558 RVA: 0x0028DA38 File Offset: 0x0028BC38
		public string GetModDirectory(string modIdStr)
		{
			ModInfo modInfo;
			bool flag = !ModDomain.ModInfoDict.TryGetValue(modIdStr, out modInfo);
			string result;
			if (flag)
			{
				result = string.Empty;
			}
			else
			{
				result = modInfo.DirectoryName;
			}
			return result;
		}

		// Token: 0x0600487F RID: 18559 RVA: 0x0028DA6C File Offset: 0x0028BC6C
		public string GetModTitle(string modIdStr)
		{
			ModInfo modInfo;
			bool flag = !ModDomain.ModInfoDict.TryGetValue(modIdStr, out modInfo);
			string result;
			if (flag)
			{
				result = string.Empty;
			}
			else
			{
				result = modInfo.Title;
			}
			return result;
		}

		// Token: 0x06004880 RID: 18560 RVA: 0x0028DAA0 File Offset: 0x0028BCA0
		private void OnInitializedDomainData()
		{
		}

		// Token: 0x06004881 RID: 18561 RVA: 0x0028DAA3 File Offset: 0x0028BCA3
		private void InitializeOnInitializeGameDataModule()
		{
		}

		// Token: 0x06004882 RID: 18562 RVA: 0x0028DAA8 File Offset: 0x0028BCA8
		private void InitializeOnEnterNewWorld()
		{
			this._archiveModDataDict.Clear();
			this._nonArchiveModDataDict.Clear();
			foreach (ModId modId in ModDomain.LoadedMods)
			{
				this._archiveModDataDict.Add(modId, new SerializableModData());
				this._nonArchiveModDataDict.Add(modId, new SerializableModData());
				List<TaiwuRemakePlugin> plugins = ModDomain.LoadedPlugins[modId];
				foreach (TaiwuRemakePlugin plugin in plugins)
				{
					plugin.OnEnterNewWorld();
				}
			}
		}

		// Token: 0x06004883 RID: 18563 RVA: 0x0028DB84 File Offset: 0x0028BD84
		private void OnLoadedArchiveData()
		{
			DataContext context = DataContextManager.GetCurrentThreadDataContext();
			foreach (ModId modId in ModDomain.LoadedMods)
			{
				bool flag = !this._archiveModDataDict.ContainsKey(modId);
				if (flag)
				{
					this.AddElement_ArchiveModDataDict(modId, new SerializableModData(), context);
				}
				bool flag2 = !this._nonArchiveModDataDict.ContainsKey(modId);
				if (flag2)
				{
					this.AddElement_NonArchiveModDataDict(modId, new SerializableModData(), context);
				}
				List<TaiwuRemakePlugin> plugins = ModDomain.LoadedPlugins[modId];
				foreach (TaiwuRemakePlugin plugin in plugins)
				{
					plugin.OnLoadedArchiveData();
				}
			}
		}

		// Token: 0x06004884 RID: 18564 RVA: 0x0028DC74 File Offset: 0x0028BE74
		public static Assembly ResolveTaiwuModdingLibPath(object sender, ResolveEventArgs args)
		{
			AssemblyName assemblyName = new AssemblyName(args.Name);
			Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
			Assembly loadedAssembly = loadedAssemblies.FirstOrDefault((Assembly assembly) => assemblyName.FullName == assembly.FullName);
			bool flag = loadedAssembly != null;
			Assembly result;
			if (flag)
			{
				result = loadedAssembly;
			}
			else
			{
				if (ModDomain._requiredAssemblies == null)
				{
					ModDomain._requiredAssemblies = new HashSet<string>
					{
						"0Harmony",
						"Mono.Cecil",
						"Mono.Cecil.Mdb",
						"Mono.Cecil.Pdb",
						"Mono.Cecil.Rocks",
						"MonoMod.RuntimeDetour",
						"MonoMod.Utils",
						"TaiwuModdingLib",
						"Newtonsoft.Json",
						"Steamworks.NET"
					};
				}
				bool flag2 = !ModDomain._requiredAssemblies.Contains(assemblyName.Name);
				if (flag2)
				{
					result = null;
				}
				else
				{
					string path = Path.Combine(Program.BaseDataDir, "The Scroll of Taiwu_Data\\Managed\\" + assemblyName.Name + ".dll");
					bool flag3 = !File.Exists(path);
					if (flag3)
					{
						ModDomain.Logger.Warn("Failed to resolve assembly because no such file can be found: " + path + ".");
						result = null;
					}
					else
					{
						Assembly assembly2 = Assembly.LoadFile(path);
						AssemblyName loadedAssemblyName = assembly2.GetName();
						bool flag4 = loadedAssemblyName.Name != assemblyName.Name;
						if (flag4)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(55, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Unexpected assembly name info: looking for ");
							defaultInterpolatedStringHandler.AppendFormatted(assemblyName.Name);
							defaultInterpolatedStringHandler.AppendLiteral(" => ");
							defaultInterpolatedStringHandler.AppendFormatted(loadedAssemblyName.Name);
							defaultInterpolatedStringHandler.AppendLiteral(" loaded.");
							throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						bool flag5 = loadedAssemblyName.FullName != assemblyName.FullName;
						if (flag5)
						{
							Logger logger = ModDomain.Logger;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Unexpected assembly Fullname info: looking for ");
							defaultInterpolatedStringHandler.AppendFormatted(assemblyName.FullName);
							defaultInterpolatedStringHandler.AppendLiteral(" => ");
							defaultInterpolatedStringHandler.AppendFormatted(loadedAssemblyName.FullName);
							defaultInterpolatedStringHandler.AppendLiteral(" loaded.");
							logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						result = assembly2;
					}
				}
			}
			return result;
		}

		// Token: 0x06004885 RID: 18565 RVA: 0x0028DEEE File Offset: 0x0028C0EE
		private void ClearDataHandlers()
		{
			ModDomain.IntDataHandlers.Clear();
			ModDomain.BoolDataHandlers.Clear();
			ModDomain.FloatDataHandlers.Clear();
			ModDomain.StringDataHandlers.Clear();
			ModDomain.SerializableModDataHandlers.Clear();
		}

		// Token: 0x06004886 RID: 18566 RVA: 0x0028DF28 File Offset: 0x0028C128
		public void AddOnReceiveDataHandler(string modIdStr, string dataName, Action<int> handler)
		{
			ModDomain.IntDataHandlers.Add(modIdStr + "." + dataName, handler);
		}

		// Token: 0x06004887 RID: 18567 RVA: 0x0028DF43 File Offset: 0x0028C143
		public void AddOnReceiveDataHandler(string modIdStr, string dataName, Action<bool> handler)
		{
			ModDomain.BoolDataHandlers.Add(modIdStr + "." + dataName, handler);
		}

		// Token: 0x06004888 RID: 18568 RVA: 0x0028DF5E File Offset: 0x0028C15E
		public void AddOnReceiveDataHandler(string modIdStr, string dataName, Action<float> handler)
		{
			ModDomain.FloatDataHandlers.Add(modIdStr + "." + dataName, handler);
		}

		// Token: 0x06004889 RID: 18569 RVA: 0x0028DF79 File Offset: 0x0028C179
		public void AddOnReceiveDataHandler(string modIdStr, string dataName, Action<string> handler)
		{
			ModDomain.StringDataHandlers.Add(modIdStr + "." + dataName, handler);
		}

		// Token: 0x0600488A RID: 18570 RVA: 0x0028DF94 File Offset: 0x0028C194
		public void AddOnReceiveDataHandler(string modIdStr, string dataName, Action<SerializableModData> handler)
		{
			ModDomain.SerializableModDataHandlers.Add(modIdStr + "." + dataName, handler);
		}

		// Token: 0x0600488B RID: 18571 RVA: 0x0028DFB0 File Offset: 0x0028C1B0
		private SerializableModData GetModData(string modIdStr, bool isArchive)
		{
			return isArchive ? this._archiveModDataDict[ModDomain.ModInfoDict[modIdStr].ModId] : this._nonArchiveModDataDict[ModDomain.ModInfoDict[modIdStr].ModId];
		}

		// Token: 0x0600488C RID: 18572 RVA: 0x0028DFFD File Offset: 0x0028C1FD
		public void OfflineSetInt(string modIdStr, string dataName, bool isArchive, int val)
		{
			this.GetModData(modIdStr, isArchive).Set(dataName, val);
		}

		// Token: 0x0600488D RID: 18573 RVA: 0x0028E011 File Offset: 0x0028C211
		public void OfflineSetFloat(string modIdStr, string dataName, bool isArchive, float val)
		{
			this.GetModData(modIdStr, isArchive).Set(dataName, val);
		}

		// Token: 0x0600488E RID: 18574 RVA: 0x0028E025 File Offset: 0x0028C225
		public void OfflineSetBool(string modIdStr, string dataName, bool isArchive, bool val)
		{
			this.GetModData(modIdStr, isArchive).Set(dataName, val);
		}

		// Token: 0x0600488F RID: 18575 RVA: 0x0028E039 File Offset: 0x0028C239
		public void OfflineSetString(string modIdStr, string dataName, bool isArchive, string val)
		{
			this.GetModData(modIdStr, isArchive).Set(dataName, val);
		}

		// Token: 0x06004890 RID: 18576 RVA: 0x0028E04D File Offset: 0x0028C24D
		public void OfflineSetSerializableModData(string modIdStr, string dataName, bool isArchive, SerializableModData val)
		{
			this.GetModData(modIdStr, isArchive).Set<SerializableModData>(dataName, val);
		}

		// Token: 0x06004891 RID: 18577 RVA: 0x0028E064 File Offset: 0x0028C264
		public void SetModData(DataContext context, string modIdStr, bool isArchive)
		{
			ModId modId = ModDomain.ModInfoDict[modIdStr].ModId;
			if (isArchive)
			{
				SerializableModData archiveModData = this._archiveModDataDict[modId];
				this.SetElement_ArchiveModDataDict(modId, archiveModData, context);
			}
			else
			{
				SerializableModData nonArchiveModData = this._nonArchiveModDataDict[modId];
				this.SetElement_NonArchiveModDataDict(modId, nonArchiveModData, context);
			}
		}

		// Token: 0x06004892 RID: 18578 RVA: 0x0028E0BC File Offset: 0x0028C2BC
		[DomainMethod]
		public bool SetInt(DataContext context, string modIdStr, string dataName, bool isArchive, int val)
		{
			this.GetModData(modIdStr, isArchive).Set(dataName, val);
			this.SetModData(context, modIdStr, isArchive);
			Action<int> handler;
			bool flag = ModDomain.IntDataHandlers.TryGetValue(modIdStr + "." + dataName, out handler);
			bool result;
			if (flag)
			{
				handler(val);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06004893 RID: 18579 RVA: 0x0028E118 File Offset: 0x0028C318
		[DomainMethod]
		public bool SetBool(DataContext context, string modIdStr, string dataName, bool isArchive, bool val)
		{
			this.GetModData(modIdStr, isArchive).Set(dataName, val);
			this.SetModData(context, modIdStr, isArchive);
			Action<bool> handler;
			bool flag = ModDomain.BoolDataHandlers.TryGetValue(modIdStr + "." + dataName, out handler);
			bool result;
			if (flag)
			{
				handler(val);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06004894 RID: 18580 RVA: 0x0028E174 File Offset: 0x0028C374
		public bool SetFloat(DataContext context, string modIdStr, string dataName, bool isArchive, float val)
		{
			this.GetModData(modIdStr, isArchive).Set(dataName, val);
			this.SetModData(context, modIdStr, isArchive);
			Action<float> handler;
			bool flag = ModDomain.FloatDataHandlers.TryGetValue(modIdStr + "." + dataName, out handler);
			bool result;
			if (flag)
			{
				handler(val);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06004895 RID: 18581 RVA: 0x0028E1D0 File Offset: 0x0028C3D0
		[DomainMethod]
		public bool SetString(DataContext context, string modIdStr, string dataName, bool isArchive, string val)
		{
			this.GetModData(modIdStr, isArchive).Set(dataName, val);
			this.SetModData(context, modIdStr, isArchive);
			Action<string> handler;
			bool flag = ModDomain.StringDataHandlers.TryGetValue(modIdStr + "." + dataName, out handler);
			bool result;
			if (flag)
			{
				handler(val);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06004896 RID: 18582 RVA: 0x0028E22C File Offset: 0x0028C42C
		[DomainMethod]
		public bool SetSerializableModData(DataContext context, string modIdStr, string dataName, bool isArchive, SerializableModData val)
		{
			this.GetModData(modIdStr, isArchive).Set<SerializableModData>(dataName, val);
			this.SetModData(context, modIdStr, isArchive);
			Action<SerializableModData> handler;
			bool flag = ModDomain.SerializableModDataHandlers.TryGetValue(modIdStr + "." + dataName, out handler);
			bool result;
			if (flag)
			{
				handler(val);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06004897 RID: 18583 RVA: 0x0028E288 File Offset: 0x0028C488
		public bool SetISerializableGameData<T>(DataContext context, string modIdStr, string dataName, bool isArchive, T val) where T : ISerializableGameData
		{
			this.GetModData(modIdStr, isArchive).Set<T>(dataName, val);
			this.SetModData(context, modIdStr, isArchive);
			return false;
		}

		// Token: 0x06004898 RID: 18584 RVA: 0x0028E2B8 File Offset: 0x0028C4B8
		[DomainMethod]
		public int GetInt(string modIdStr, string dataName, bool isArchive)
		{
			int val;
			bool flag = this.GetModData(modIdStr, isArchive).Get(dataName, out val);
			if (flag)
			{
				return val;
			}
			throw new ArgumentOutOfRangeException("dataName", "Failed to find mod data " + dataName + " of type int");
		}

		// Token: 0x06004899 RID: 18585 RVA: 0x0028E2FC File Offset: 0x0028C4FC
		[DomainMethod]
		public bool GetBool(string modIdStr, string dataName, bool isArchive)
		{
			bool val;
			bool flag = this.GetModData(modIdStr, isArchive).Get(dataName, out val);
			if (flag)
			{
				return val;
			}
			throw new ArgumentOutOfRangeException("dataName", "Failed to find mod data " + dataName + " of type bool");
		}

		// Token: 0x0600489A RID: 18586 RVA: 0x0028E340 File Offset: 0x0028C540
		[DomainMethod]
		public string GetString(string modIdStr, string dataName, bool isArchive)
		{
			string val;
			bool flag = this.GetModData(modIdStr, isArchive).Get(dataName, out val);
			if (flag)
			{
				return val;
			}
			throw new ArgumentOutOfRangeException("dataName", "Failed to find mod data " + dataName + " of type string");
		}

		// Token: 0x0600489B RID: 18587 RVA: 0x0028E384 File Offset: 0x0028C584
		[DomainMethod]
		public SerializableModData GetSerializableModData(string modIdStr, string dataName, bool isArchive)
		{
			SerializableModData val;
			bool flag = this.GetModData(modIdStr, isArchive).Get<SerializableModData>(dataName, out val);
			if (flag)
			{
				return val;
			}
			throw new ArgumentOutOfRangeException("dataName", "Failed to find SerializableModData with name " + dataName);
		}

		// Token: 0x0600489C RID: 18588 RVA: 0x0028E3C4 File Offset: 0x0028C5C4
		public bool TryGet(string modIdStr, string dataName, bool isArchive, out int val)
		{
			return this.GetModData(modIdStr, isArchive).Get(dataName, out val);
		}

		// Token: 0x0600489D RID: 18589 RVA: 0x0028E3E8 File Offset: 0x0028C5E8
		public bool TryGet(string modIdStr, string dataName, bool isArchive, out bool val)
		{
			return this.GetModData(modIdStr, isArchive).Get(dataName, out val);
		}

		// Token: 0x0600489E RID: 18590 RVA: 0x0028E40C File Offset: 0x0028C60C
		public bool TryGet(string modIdStr, string dataName, bool isArchive, out float val)
		{
			return this.GetModData(modIdStr, isArchive).Get(dataName, out val);
		}

		// Token: 0x0600489F RID: 18591 RVA: 0x0028E430 File Offset: 0x0028C630
		public bool TryGet(string modIdStr, string dataName, bool isArchive, out string val)
		{
			return this.GetModData(modIdStr, isArchive).Get(dataName, out val);
		}

		// Token: 0x060048A0 RID: 18592 RVA: 0x0028E454 File Offset: 0x0028C654
		public bool TryGet(string modIdStr, string dataName, bool isArchive, out SerializableModData val)
		{
			return this.GetModData(modIdStr, isArchive).Get<SerializableModData>(dataName, out val);
		}

		// Token: 0x060048A1 RID: 18593 RVA: 0x0028E478 File Offset: 0x0028C678
		public bool TryGet<T>(string modIdStr, string dataName, bool isArchive, out T val) where T : ISerializableGameData
		{
			return this.GetModData(modIdStr, isArchive).Get<T>(dataName, out val);
		}

		// Token: 0x060048A2 RID: 18594 RVA: 0x0028E49C File Offset: 0x0028C69C
		public bool RemoveInt(DataContext context, string modIdStr, string dataName, bool isArchive)
		{
			bool succeed = this.GetModData(modIdStr, isArchive).RemoveInt(dataName);
			this.SetModData(context, modIdStr, isArchive);
			return succeed;
		}

		// Token: 0x060048A3 RID: 18595 RVA: 0x0028E4CC File Offset: 0x0028C6CC
		public bool RemoveBool(DataContext context, string modIdStr, string dataName, bool isArchive)
		{
			bool succeed = this.GetModData(modIdStr, isArchive).RemoveBool(dataName);
			this.SetModData(context, modIdStr, isArchive);
			return succeed;
		}

		// Token: 0x060048A4 RID: 18596 RVA: 0x0028E4FC File Offset: 0x0028C6FC
		public bool RemoveFloat(DataContext context, string modIdStr, string dataName, bool isArchive)
		{
			bool succeed = this.GetModData(modIdStr, isArchive).RemoveFloat(dataName);
			this.SetModData(context, modIdStr, isArchive);
			return succeed;
		}

		// Token: 0x060048A5 RID: 18597 RVA: 0x0028E52C File Offset: 0x0028C72C
		public bool RemoveString(DataContext context, string modIdStr, string dataName, bool isArchive)
		{
			bool succeed = this.GetModData(modIdStr, isArchive).RemoveString(dataName);
			this.SetModData(context, modIdStr, isArchive);
			return succeed;
		}

		// Token: 0x060048A6 RID: 18598 RVA: 0x0028E55C File Offset: 0x0028C75C
		public bool RemoveSerializableModData(DataContext context, string modIdStr, string dataName, bool isArchive)
		{
			bool succeed = this.GetModData(modIdStr, isArchive).RemoveObject(dataName);
			this.SetModData(context, modIdStr, isArchive);
			return succeed;
		}

		// Token: 0x060048A7 RID: 18599 RVA: 0x0028E58C File Offset: 0x0028C78C
		public bool RemoveISerializableGameData(DataContext context, string modIdStr, string dataName, bool isArchive)
		{
			bool succeed = this.GetModData(modIdStr, isArchive).RemoveObject(dataName);
			this.SetModData(context, modIdStr, isArchive);
			return succeed;
		}

		// Token: 0x060048A8 RID: 18600 RVA: 0x0028E5BA File Offset: 0x0028C7BA
		public void RemoveData(DataContext context, string modIdStr, string dataName)
		{
			this.GetModData(modIdStr, true).Remove(dataName);
			this.SetModData(context, modIdStr, true);
			this.GetModData(modIdStr, false).Remove(dataName);
			this.SetModData(context, modIdStr, false);
		}

		// Token: 0x060048A9 RID: 18601 RVA: 0x0028E5EF File Offset: 0x0028C7EF
		private void ClearModMethods()
		{
			ModDomain.ModMethods.Clear();
			ModDomain.ModMethodsWithParam.Clear();
			ModDomain.ModMethodsWithRet.Clear();
			ModDomain.ModMethodsWithParamAndRet.Clear();
		}

		// Token: 0x060048AA RID: 18602 RVA: 0x0028E620 File Offset: 0x0028C820
		[DomainMethod]
		public void CallModMethod(DataContext context, string modIdStr, string methodName)
		{
			Action<DataContext> method;
			bool flag = ModDomain.ModMethods.TryGetValue(modIdStr + ".Method." + methodName, out method);
			if (flag)
			{
				method(context);
			}
			else
			{
				string modName = this.GetModTitle(modIdStr);
				Logger logger = ModDomain.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Unable to call method ");
				defaultInterpolatedStringHandler.AppendFormatted(methodName);
				defaultInterpolatedStringHandler.AppendLiteral(" of mod ");
				defaultInterpolatedStringHandler.AppendFormatted(modName);
				defaultInterpolatedStringHandler.AppendLiteral("(");
				defaultInterpolatedStringHandler.AppendFormatted(modIdStr);
				defaultInterpolatedStringHandler.AppendLiteral(").");
				logger.AppendWarning(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x060048AB RID: 18603 RVA: 0x0028E6C8 File Offset: 0x0028C8C8
		[DomainMethod]
		public void CallModMethodWithParam(DataContext context, string modIdStr, string methodName, SerializableModData parameter)
		{
			Action<DataContext, SerializableModData> method;
			bool flag = ModDomain.ModMethodsWithParam.TryGetValue(modIdStr + ".Method." + methodName, out method);
			if (flag)
			{
				method(context, parameter);
			}
			else
			{
				string modName = this.GetModTitle(modIdStr);
				Logger logger = ModDomain.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Unable to call method ");
				defaultInterpolatedStringHandler.AppendFormatted(methodName);
				defaultInterpolatedStringHandler.AppendLiteral(" of mod ");
				defaultInterpolatedStringHandler.AppendFormatted(modName);
				defaultInterpolatedStringHandler.AppendLiteral("(");
				defaultInterpolatedStringHandler.AppendFormatted(modIdStr);
				defaultInterpolatedStringHandler.AppendLiteral(") with parameter.");
				logger.AppendWarning(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x060048AC RID: 18604 RVA: 0x0028E774 File Offset: 0x0028C974
		[DomainMethod]
		public SerializableModData CallModMethodWithRet(DataContext context, string modIdStr, string methodName)
		{
			Func<DataContext, SerializableModData> method;
			bool flag = ModDomain.ModMethodsWithRet.TryGetValue(modIdStr + ".Method." + methodName, out method);
			SerializableModData result;
			if (flag)
			{
				result = method(context);
			}
			else
			{
				string modName = this.GetModTitle(modIdStr);
				Logger logger = ModDomain.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Unable to call method ");
				defaultInterpolatedStringHandler.AppendFormatted(methodName);
				defaultInterpolatedStringHandler.AppendLiteral(" of mod ");
				defaultInterpolatedStringHandler.AppendFormatted(modName);
				defaultInterpolatedStringHandler.AppendLiteral("(");
				defaultInterpolatedStringHandler.AppendFormatted(modIdStr);
				defaultInterpolatedStringHandler.AppendLiteral(") with return value.");
				logger.AppendWarning(defaultInterpolatedStringHandler.ToStringAndClear());
				result = new SerializableModData();
			}
			return result;
		}

		// Token: 0x060048AD RID: 18605 RVA: 0x0028E824 File Offset: 0x0028CA24
		[DomainMethod]
		public SerializableModData CallModMethodWithParamAndRet(DataContext context, string modIdStr, string methodName, SerializableModData parameter)
		{
			Func<DataContext, SerializableModData, SerializableModData> method;
			bool flag = ModDomain.ModMethodsWithParamAndRet.TryGetValue(modIdStr + ".Method." + methodName, out method);
			SerializableModData result;
			if (flag)
			{
				result = method(context, parameter);
			}
			else
			{
				string modName = this.GetModTitle(modIdStr);
				Logger logger = ModDomain.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(65, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Unable to call method ");
				defaultInterpolatedStringHandler.AppendFormatted(methodName);
				defaultInterpolatedStringHandler.AppendLiteral(" of mod ");
				defaultInterpolatedStringHandler.AppendFormatted(modName);
				defaultInterpolatedStringHandler.AppendLiteral("(");
				defaultInterpolatedStringHandler.AppendFormatted(modIdStr);
				defaultInterpolatedStringHandler.AppendLiteral(") with parameter and return value.");
				logger.AppendWarning(defaultInterpolatedStringHandler.ToStringAndClear());
				result = new SerializableModData();
			}
			return result;
		}

		// Token: 0x060048AE RID: 18606 RVA: 0x0028E8D6 File Offset: 0x0028CAD6
		public void AddModMethod(string modIdStr, string methodName, Func<DataContext, SerializableModData, SerializableModData> method)
		{
			ModDomain.ModMethodsWithParamAndRet.Add(modIdStr + ".Method." + methodName, method);
		}

		// Token: 0x060048AF RID: 18607 RVA: 0x0028E8F1 File Offset: 0x0028CAF1
		public void AddModMethod(string modIdStr, string methodName, Func<DataContext, SerializableModData> method)
		{
			ModDomain.ModMethodsWithRet.Add(modIdStr + ".Method." + methodName, method);
		}

		// Token: 0x060048B0 RID: 18608 RVA: 0x0028E90C File Offset: 0x0028CB0C
		public void AddModMethod(string modIdStr, string methodName, Action<DataContext, SerializableModData> method)
		{
			ModDomain.ModMethodsWithParam.Add(modIdStr + ".Method." + methodName, method);
		}

		// Token: 0x060048B1 RID: 18609 RVA: 0x0028E927 File Offset: 0x0028CB27
		public void AddModMethod(string modIdStr, string methodName, Action<DataContext> method)
		{
			ModDomain.ModMethods.Add(modIdStr + ".Method." + methodName, method);
		}

		// Token: 0x060048B2 RID: 18610 RVA: 0x0028E942 File Offset: 0x0028CB42
		public void AddModDisplayEvent(string modIdStr, string customData)
		{
			GameDataBridge.AddDisplayEvent<string, string>(DisplayEventType.ModDisplayEvent, modIdStr, customData);
		}

		// Token: 0x060048B3 RID: 18611 RVA: 0x0028E950 File Offset: 0x0028CB50
		[DomainMethod]
		public void UpdateModSettings(DataContext context, ModId modId, SerializableModData modData)
		{
			ModDomain.ModInfoDict[modId.ToString()].ModSettings = modData;
			foreach (TaiwuRemakePlugin plugin in ModDomain.LoadedPlugins[modId])
			{
				plugin.OnModSettingUpdate();
			}
		}

		// Token: 0x060048B4 RID: 18612 RVA: 0x0028E9CC File Offset: 0x0028CBCC
		public bool GetSetting(string modIdStr, string settingName, ref int val)
		{
			ModInfo modInfo;
			return ModDomain.ModInfoDict.TryGetValue(modIdStr, out modInfo) && modInfo.ModSettings.Get(settingName, out val);
		}

		// Token: 0x060048B5 RID: 18613 RVA: 0x0028EA00 File Offset: 0x0028CC00
		public bool GetSetting(string modIdStr, string settingName, ref float val)
		{
			ModInfo modInfo;
			return ModDomain.ModInfoDict.TryGetValue(modIdStr, out modInfo) && modInfo.ModSettings.Get(settingName, out val);
		}

		// Token: 0x060048B6 RID: 18614 RVA: 0x0028EA34 File Offset: 0x0028CC34
		public bool GetSetting(string modIdStr, string settingName, ref bool val)
		{
			ModInfo modInfo;
			return ModDomain.ModInfoDict.TryGetValue(modIdStr, out modInfo) && modInfo.ModSettings.Get(settingName, out val);
		}

		// Token: 0x060048B7 RID: 18615 RVA: 0x0028EA68 File Offset: 0x0028CC68
		public bool GetSetting(string modIdStr, string settingName, ref string val)
		{
			ModInfo modInfo;
			return ModDomain.ModInfoDict.TryGetValue(modIdStr, out modInfo) && modInfo.ModSettings.Get(settingName, out val);
		}

		// Token: 0x060048B8 RID: 18616 RVA: 0x0028EA99 File Offset: 0x0028CC99
		public ModDomain() : base(2)
		{
			this._archiveModDataDict = new Dictionary<ModId, SerializableModData>(0);
			this._nonArchiveModDataDict = new Dictionary<ModId, SerializableModData>(0);
			this.OnInitializedDomainData();
		}

		// Token: 0x060048B9 RID: 18617 RVA: 0x0028EAC4 File Offset: 0x0028CCC4
		private SerializableModData GetElement_ArchiveModDataDict(ModId elementId)
		{
			return this._archiveModDataDict[elementId];
		}

		// Token: 0x060048BA RID: 18618 RVA: 0x0028EAE4 File Offset: 0x0028CCE4
		private bool TryGetElement_ArchiveModDataDict(ModId elementId, out SerializableModData value)
		{
			return this._archiveModDataDict.TryGetValue(elementId, out value);
		}

		// Token: 0x060048BB RID: 18619 RVA: 0x0028EB04 File Offset: 0x0028CD04
		private unsafe void AddElement_ArchiveModDataDict(ModId elementId, SerializableModData value, DataContext context)
		{
			this._archiveModDataDict.Add(elementId, value);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, this.DataStates, ModDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<ModId>(16, 0, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<ModId>(16, 0, elementId, 0);
			}
		}

		// Token: 0x060048BC RID: 18620 RVA: 0x0028EB68 File Offset: 0x0028CD68
		private unsafe void SetElement_ArchiveModDataDict(ModId elementId, SerializableModData value, DataContext context)
		{
			this._archiveModDataDict[elementId] = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, this.DataStates, ModDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<ModId>(16, 0, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<ModId>(16, 0, elementId, 0);
			}
		}

		// Token: 0x060048BD RID: 18621 RVA: 0x0028EBCA File Offset: 0x0028CDCA
		private void RemoveElement_ArchiveModDataDict(ModId elementId, DataContext context)
		{
			this._archiveModDataDict.Remove(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, this.DataStates, ModDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<ModId>(16, 0, elementId);
		}

		// Token: 0x060048BE RID: 18622 RVA: 0x0028EBF7 File Offset: 0x0028CDF7
		private void ClearArchiveModDataDict(DataContext context)
		{
			this._archiveModDataDict.Clear();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, this.DataStates, ModDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(16, 0);
		}

		// Token: 0x060048BF RID: 18623 RVA: 0x0028EC24 File Offset: 0x0028CE24
		private SerializableModData GetElement_NonArchiveModDataDict(ModId elementId)
		{
			return this._nonArchiveModDataDict[elementId];
		}

		// Token: 0x060048C0 RID: 18624 RVA: 0x0028EC44 File Offset: 0x0028CE44
		private bool TryGetElement_NonArchiveModDataDict(ModId elementId, out SerializableModData value)
		{
			return this._nonArchiveModDataDict.TryGetValue(elementId, out value);
		}

		// Token: 0x060048C1 RID: 18625 RVA: 0x0028EC63 File Offset: 0x0028CE63
		private void AddElement_NonArchiveModDataDict(ModId elementId, SerializableModData value, DataContext context)
		{
			this._nonArchiveModDataDict.Add(elementId, value);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, this.DataStates, ModDomain.CacheInfluences, context);
		}

		// Token: 0x060048C2 RID: 18626 RVA: 0x0028EC87 File Offset: 0x0028CE87
		private void SetElement_NonArchiveModDataDict(ModId elementId, SerializableModData value, DataContext context)
		{
			this._nonArchiveModDataDict[elementId] = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, this.DataStates, ModDomain.CacheInfluences, context);
		}

		// Token: 0x060048C3 RID: 18627 RVA: 0x0028ECAB File Offset: 0x0028CEAB
		private void RemoveElement_NonArchiveModDataDict(ModId elementId, DataContext context)
		{
			this._nonArchiveModDataDict.Remove(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, this.DataStates, ModDomain.CacheInfluences, context);
		}

		// Token: 0x060048C4 RID: 18628 RVA: 0x0028ECCE File Offset: 0x0028CECE
		private void ClearNonArchiveModDataDict(DataContext context)
		{
			this._nonArchiveModDataDict.Clear();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, this.DataStates, ModDomain.CacheInfluences, context);
		}

		// Token: 0x060048C5 RID: 18629 RVA: 0x0028ECF0 File Offset: 0x0028CEF0
		public override void OnInitializeGameDataModule()
		{
			this.InitializeOnInitializeGameDataModule();
		}

		// Token: 0x060048C6 RID: 18630 RVA: 0x0028ECFC File Offset: 0x0028CEFC
		public unsafe override void OnEnterNewWorld()
		{
			this.InitializeOnEnterNewWorld();
			this.InitializeInternalDataOfCollections();
			foreach (KeyValuePair<ModId, SerializableModData> entry in this._archiveModDataDict)
			{
				ModId elementId = entry.Key;
				SerializableModData value = entry.Value;
				bool flag = value != null;
				if (flag)
				{
					int contentSize = value.GetSerializedSize();
					byte* pData = OperationAdder.DynamicSingleValueCollection_Add<ModId>(16, 0, elementId, contentSize);
					pData += value.Serialize(pData);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<ModId>(16, 0, elementId, 0);
				}
			}
		}

		// Token: 0x060048C7 RID: 18631 RVA: 0x0028EDA8 File Offset: 0x0028CFA8
		public override void OnLoadWorld()
		{
			this._pendingLoadingOperationIds = new Queue<uint>();
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(16, 0));
		}

		// Token: 0x060048C8 RID: 18632 RVA: 0x0028EDCC File Offset: 0x0028CFCC
		public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (dataId == 0)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to get value of dataId: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (dataId != 1)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Not allow to get value of dataId: ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060048C9 RID: 18633 RVA: 0x0028EE70 File Offset: 0x0028D070
		public override void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (dataId == 0)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (dataId != 1)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060048CA RID: 18634 RVA: 0x0028EF14 File Offset: 0x0028D114
		public override int CallMethod(Operation operation, RawDataPool argDataPool, RawDataPool returnDataPool, DataContext context)
		{
			int argsOffset = operation.ArgsOffset;
			int result;
			switch (operation.MethodId)
			{
			case 0:
			{
				int argsCount = operation.ArgsCount;
				int num = argsCount;
				if (num != 4)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string modIdStr = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref modIdStr);
				string dataName = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref dataName);
				bool isArchive = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isArchive);
				int val = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref val);
				bool returnValue = this.SetInt(context, modIdStr, dataName, isArchive, val);
				result = Serializer.Serialize(returnValue, returnDataPool);
				break;
			}
			case 1:
			{
				int argsCount2 = operation.ArgsCount;
				int num2 = argsCount2;
				if (num2 != 4)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string modIdStr2 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref modIdStr2);
				string dataName2 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref dataName2);
				bool isArchive2 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isArchive2);
				bool val2 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref val2);
				bool returnValue2 = this.SetBool(context, modIdStr2, dataName2, isArchive2, val2);
				result = Serializer.Serialize(returnValue2, returnDataPool);
				break;
			}
			case 2:
			{
				int argsCount3 = operation.ArgsCount;
				int num3 = argsCount3;
				if (num3 != 4)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string modIdStr3 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref modIdStr3);
				string dataName3 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref dataName3);
				bool isArchive3 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isArchive3);
				string val3 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref val3);
				bool returnValue3 = this.SetString(context, modIdStr3, dataName3, isArchive3, val3);
				result = Serializer.Serialize(returnValue3, returnDataPool);
				break;
			}
			case 3:
			{
				int argsCount4 = operation.ArgsCount;
				int num4 = argsCount4;
				if (num4 != 4)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string modIdStr4 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref modIdStr4);
				string dataName4 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref dataName4);
				bool isArchive4 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isArchive4);
				SerializableModData val4 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref val4);
				bool returnValue4 = this.SetSerializableModData(context, modIdStr4, dataName4, isArchive4, val4);
				result = Serializer.Serialize(returnValue4, returnDataPool);
				break;
			}
			case 4:
			{
				int argsCount5 = operation.ArgsCount;
				int num5 = argsCount5;
				if (num5 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string modIdStr5 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref modIdStr5);
				string dataName5 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref dataName5);
				bool isArchive5 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isArchive5);
				int returnValue5 = this.GetInt(modIdStr5, dataName5, isArchive5);
				result = Serializer.Serialize(returnValue5, returnDataPool);
				break;
			}
			case 5:
			{
				int argsCount6 = operation.ArgsCount;
				int num6 = argsCount6;
				if (num6 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string modIdStr6 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref modIdStr6);
				string dataName6 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref dataName6);
				bool isArchive6 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isArchive6);
				bool returnValue6 = this.GetBool(modIdStr6, dataName6, isArchive6);
				result = Serializer.Serialize(returnValue6, returnDataPool);
				break;
			}
			case 6:
			{
				int argsCount7 = operation.ArgsCount;
				int num7 = argsCount7;
				if (num7 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string modIdStr7 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref modIdStr7);
				string dataName7 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref dataName7);
				bool isArchive7 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isArchive7);
				string returnValue7 = this.GetString(modIdStr7, dataName7, isArchive7);
				result = Serializer.Serialize(returnValue7, returnDataPool);
				break;
			}
			case 7:
			{
				int argsCount8 = operation.ArgsCount;
				int num8 = argsCount8;
				if (num8 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string modIdStr8 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref modIdStr8);
				string dataName8 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref dataName8);
				bool isArchive8 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isArchive8);
				SerializableModData returnValue8 = this.GetSerializableModData(modIdStr8, dataName8, isArchive8);
				result = Serializer.Serialize(returnValue8, returnDataPool);
				break;
			}
			case 8:
			{
				int argsCount9 = operation.ArgsCount;
				int num9 = argsCount9;
				if (num9 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ModId modId = default(ModId);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref modId);
				SerializableModData modData = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref modData);
				this.UpdateModSettings(context, modId, modData);
				result = -1;
				break;
			}
			case 9:
			{
				int argsCount10 = operation.ArgsCount;
				int num10 = argsCount10;
				if (num10 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string modIdStr9 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref modIdStr9);
				string methodName = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref methodName);
				this.CallModMethod(context, modIdStr9, methodName);
				result = -1;
				break;
			}
			case 10:
			{
				int argsCount11 = operation.ArgsCount;
				int num11 = argsCount11;
				if (num11 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string modIdStr10 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref modIdStr10);
				string methodName2 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref methodName2);
				SerializableModData parameter = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref parameter);
				this.CallModMethodWithParam(context, modIdStr10, methodName2, parameter);
				result = -1;
				break;
			}
			case 11:
			{
				int argsCount12 = operation.ArgsCount;
				int num12 = argsCount12;
				if (num12 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string modIdStr11 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref modIdStr11);
				string methodName3 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref methodName3);
				SerializableModData returnValue9 = this.CallModMethodWithRet(context, modIdStr11, methodName3);
				result = Serializer.Serialize(returnValue9, returnDataPool);
				break;
			}
			case 12:
			{
				int argsCount13 = operation.ArgsCount;
				int num13 = argsCount13;
				if (num13 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string modIdStr12 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref modIdStr12);
				string methodName4 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref methodName4);
				SerializableModData parameter2 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref parameter2);
				SerializableModData returnValue10 = this.CallModMethodWithParamAndRet(context, modIdStr12, methodName4, parameter2);
				result = Serializer.Serialize(returnValue10, returnDataPool);
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x060048CB RID: 18635 RVA: 0x0028F710 File Offset: 0x0028D910
		public override void OnMonitorData(ushort dataId, ulong subId0, uint subId1, bool monitoring)
		{
			if (dataId != 0)
			{
				if (dataId != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
		}

		// Token: 0x060048CC RID: 18636 RVA: 0x0028F760 File Offset: 0x0028D960
		public override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (dataId == 0)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (dataId != 1)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification of dataId ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060048CD RID: 18637 RVA: 0x0028F804 File Offset: 0x0028DA04
		public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (dataId == 0)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (dataId != 1)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of dataId ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060048CE RID: 18638 RVA: 0x0028F8A8 File Offset: 0x0028DAA8
		public override bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (dataId == 0)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to verify modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (dataId != 1)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Not allow to verify modification state of dataId ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060048CF RID: 18639 RVA: 0x0028F94C File Offset: 0x0028DB4C
		public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
		{
			ushort dataId = influence.TargetIndicator.DataId;
			ushort num = dataId;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (num != 0)
			{
				if (num != 1)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(influence.TargetIndicator.DataId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Cannot invalidate cache state of non-cache data ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(influence.TargetIndicator.DataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060048D0 RID: 18640 RVA: 0x0028F9E8 File Offset: 0x0028DBE8
		public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
		{
			ushort dataId = operation.DataId;
			ushort num = dataId;
			if (num == 0)
			{
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<ModId, SerializableModData>(operation, pResult, this._archiveModDataDict);
				bool flag = this._pendingLoadingOperationIds != null;
				if (flag)
				{
					uint currPendingOperationId = this._pendingLoadingOperationIds.Peek();
					bool flag2 = currPendingOperationId == operation.Id;
					if (flag2)
					{
						this._pendingLoadingOperationIds.Dequeue();
						bool flag3 = this._pendingLoadingOperationIds.Count <= 0;
						if (flag3)
						{
							this._pendingLoadingOperationIds = null;
							this.InitializeInternalDataOfCollections();
							this.OnLoadedArchiveData();
							DomainManager.Global.CompleteLoading(16);
						}
					}
				}
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (num != 1)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.DataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Cannot process archive response of non-archive data ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.DataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060048D1 RID: 18641 RVA: 0x0028FAFF File Offset: 0x0028DCFF
		private void InitializeInternalDataOfCollections()
		{
		}

		// Token: 0x0400152E RID: 5422
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x0400152F RID: 5423
		private static readonly List<ModId> LoadedMods = new List<ModId>();

		// Token: 0x04001530 RID: 5424
		private static readonly Dictionary<string, ModInfo> ModInfoDict = new Dictionary<string, ModInfo>();

		// Token: 0x04001531 RID: 5425
		private static readonly Dictionary<ModId, List<TaiwuRemakePlugin>> LoadedPlugins = new Dictionary<ModId, List<TaiwuRemakePlugin>>();

		// Token: 0x04001532 RID: 5426
		private static readonly ModConfigDataManager _modConfigDataManager = new ModConfigDataManager();

		// Token: 0x04001533 RID: 5427
		private static HashSet<string> _requiredAssemblies;

		// Token: 0x04001534 RID: 5428
		private static readonly Dictionary<string, Action<int>> IntDataHandlers = new Dictionary<string, Action<int>>();

		// Token: 0x04001535 RID: 5429
		private static readonly Dictionary<string, Action<bool>> BoolDataHandlers = new Dictionary<string, Action<bool>>();

		// Token: 0x04001536 RID: 5430
		private static readonly Dictionary<string, Action<float>> FloatDataHandlers = new Dictionary<string, Action<float>>();

		// Token: 0x04001537 RID: 5431
		private static readonly Dictionary<string, Action<string>> StringDataHandlers = new Dictionary<string, Action<string>>();

		// Token: 0x04001538 RID: 5432
		private static readonly Dictionary<string, Action<SerializableModData>> SerializableModDataHandlers = new Dictionary<string, Action<SerializableModData>>();

		// Token: 0x04001539 RID: 5433
		[DomainData(DomainDataType.SingleValueCollection, true, false, false, false)]
		private readonly Dictionary<ModId, SerializableModData> _archiveModDataDict;

		// Token: 0x0400153A RID: 5434
		[DomainData(DomainDataType.SingleValueCollection, false, false, false, false)]
		private readonly Dictionary<ModId, SerializableModData> _nonArchiveModDataDict;

		// Token: 0x0400153B RID: 5435
		private static readonly Dictionary<string, Action<DataContext>> ModMethods = new Dictionary<string, Action<DataContext>>();

		// Token: 0x0400153C RID: 5436
		private static readonly Dictionary<string, Action<DataContext, SerializableModData>> ModMethodsWithParam = new Dictionary<string, Action<DataContext, SerializableModData>>();

		// Token: 0x0400153D RID: 5437
		private static readonly Dictionary<string, Func<DataContext, SerializableModData>> ModMethodsWithRet = new Dictionary<string, Func<DataContext, SerializableModData>>();

		// Token: 0x0400153E RID: 5438
		private static readonly Dictionary<string, Func<DataContext, SerializableModData, SerializableModData>> ModMethodsWithParamAndRet = new Dictionary<string, Func<DataContext, SerializableModData, SerializableModData>>();

		// Token: 0x0400153F RID: 5439
		private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[2][];

		// Token: 0x04001540 RID: 5440
		private Queue<uint> _pendingLoadingOperationIds;
	}
}
