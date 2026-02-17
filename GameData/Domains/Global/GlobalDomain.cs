using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Config;
using Config.Common;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Common.SingleValueCollection;
using GameData.Common.WorkerThread;
using GameData.Dependencies;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.Creation;
using GameData.Domains.Global.Inscription;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.World;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using NLog;
using Redzen.Random;

namespace GameData.Domains.Global
{
	// Token: 0x02000689 RID: 1673
	[GameDataDomain(0, ArchiveAttached = false, CustomArchiveModuleCode = true)]
	public class GlobalDomain : BaseGameDataDomain
	{
		// Token: 0x060054E9 RID: 21737 RVA: 0x002EAE80 File Offset: 0x002E9080
		private void OnInitializedDomainData()
		{
			this.GlobalDataLoaded = false;
		}

		// Token: 0x060054EA RID: 21738 RVA: 0x002EAE8A File Offset: 0x002E908A
		private void InitializeOnInitializeGameDataModule()
		{
		}

		// Token: 0x060054EB RID: 21739 RVA: 0x002EAE8D File Offset: 0x002E908D
		private void InitializeOnEnterNewWorld()
		{
		}

		// Token: 0x060054EC RID: 21740 RVA: 0x002EAE90 File Offset: 0x002E9090
		private void OnLoadedArchiveData()
		{
			this.GlobalDataLoaded = true;
		}

		// Token: 0x060054ED RID: 21741 RVA: 0x002EAE9C File Offset: 0x002E909C
		private unsafe void ProcessGlobalArchiveResponse(OperationWrapper operation, byte* pResult)
		{
			switch (operation.MethodId)
			{
			case 0:
				break;
			case 1:
				break;
			case 2:
				break;
			case 3:
			{
				this.SetSavingWorld(false, null);
				this._timer.Stop();
				int date = DomainManager.World.GetCurrDate();
				Logger logger = GlobalDomain.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 2);
				defaultInterpolatedStringHandler.AppendLiteral("[[");
				defaultInterpolatedStringHandler.AppendFormatted<int>(date);
				defaultInterpolatedStringHandler.AppendLiteral("]] Save world: ");
				defaultInterpolatedStringHandler.AppendFormatted<double>(this._timer.Elapsed.TotalMilliseconds, "F1");
				logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
				break;
			}
			case 4:
				break;
			case 5:
			{
				ArchiveInfo[] archivesInfo;
				ResponseProcessor.Global_GetArchivesInfo(pResult, out archivesInfo);
				GameDataBridge.TryReturnPassthroughMethod<ArchiveInfo[]>(operation.Id, archivesInfo);
				break;
			}
			case 6:
				break;
			case 7:
				break;
			case 8:
				break;
			case 9:
			{
				ArchiveInfo archiveInfo;
				ResponseProcessor.Global_GetEndingArchiveInfo(pResult, out archiveInfo);
				this._onEndingArchiveInfoLoaded(archiveInfo.Status == 1);
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported GlobalMethodId: ");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x060054EE RID: 21742 RVA: 0x002EAFEE File Offset: 0x002E91EE
		public unsafe static void FreeMemory(IntPtr pointer)
		{
			Common.FreeMemory((byte*)((void*)pointer));
		}

		// Token: 0x060054EF RID: 21743 RVA: 0x002EB000 File Offset: 0x002E9200
		[DomainMethod]
		public void EnterNewWorld(sbyte archiveId)
		{
			bool flag = Common.IsInWorld();
			if (flag)
			{
				throw new Exception("Cannot enter new world when it's already in a world");
			}
			Common.SetArchiveId(archiveId);
			this.SetLoadedAllArchiveData(false, null);
			OperationAdder.Global_EnterNewWorld(archiveId);
			DomainManager.ResetArchiveAttachedDomains();
			foreach (BaseGameDataDomain domain in DomainManager.GetArchiveAttachedDomains())
			{
				domain.OnEnterNewWorld();
			}
		}

		// Token: 0x060054F0 RID: 21744 RVA: 0x002EB060 File Offset: 0x002E9260
		[DomainMethod]
		public void LoadWorld(sbyte archiveId, long backupTimestamp)
		{
			bool flag = Common.IsInWorld();
			if (flag)
			{
				throw new Exception("Cannot load world when it's already in a world");
			}
			Common.SetArchiveId(archiveId);
			this.SetLoadedAllArchiveData(false, null);
			OperationAdder.Global_LoadWorld(archiveId, backupTimestamp);
			this._loadingDomainIds = new HashSet<ushort>(DomainManager.ArchiveAttachedDomainIds);
			DataUid uid = new DataUid(0, 1, ulong.MaxValue, uint.MaxValue);
			base.AddPostModificationHandler(uid, "RegisterItemOwners", new Action<DataContext, DataUid>(ItemDomain.RegisterItemOwners));
			base.AddPostModificationHandler(uid, "FixAllAbnormalDomainArchiveData", new Action<DataContext, DataUid>(this.FixAllAbnormalDomainArchiveData));
			base.AddPostModificationHandler(uid, "InitBuildingEffect", new Action<DataContext, DataUid>(this.InitBuildingEffect));
			DomainManager.ResetArchiveAttachedDomains();
			foreach (BaseGameDataDomain domain in DomainManager.GetArchiveAttachedDomains())
			{
				domain.OnLoadWorld();
			}
		}

		// Token: 0x060054F1 RID: 21745 RVA: 0x002EB12C File Offset: 0x002E932C
		[DomainMethod]
		public void LoadEnding(sbyte archiveId)
		{
			bool flag = Common.IsInWorld();
			if (flag)
			{
				throw new Exception("Need to exit the current world first.");
			}
			Common.SetArchiveId(archiveId);
			this.SetLoadedAllArchiveData(false, null);
			OperationAdder.Global_LoadEnding(archiveId);
			this._loadingDomainIds = new HashSet<ushort>(DomainManager.ArchiveAttachedDomainIds);
			DataUid uid = new DataUid(0, 1, ulong.MaxValue, uint.MaxValue);
			base.AddPostModificationHandler(uid, "RegisterItemOwners", new Action<DataContext, DataUid>(ItemDomain.RegisterItemOwners));
			base.AddPostModificationHandler(uid, "UnpackAllCrossArchiveGameData", new Action<DataContext, DataUid>(this.UnpackAllCrossArchiveGameData));
			base.AddPostModificationHandler(uid, "FixAllAbnormalDomainArchiveData", new Action<DataContext, DataUid>(this.FixAllAbnormalDomainArchiveData));
			base.AddPostModificationHandler(uid, "InitBuildingEffect", new Action<DataContext, DataUid>(this.InitBuildingEffect));
			DomainManager.ResetArchiveAttachedDomains();
			foreach (BaseGameDataDomain domain in DomainManager.GetArchiveAttachedDomains())
			{
				domain.OnLoadWorld();
			}
		}

		// Token: 0x060054F2 RID: 21746 RVA: 0x002EB210 File Offset: 0x002E9410
		public void SaveEnding(DataContext context)
		{
			bool flag = !Common.IsInWorld();
			if (flag)
			{
				throw new Exception("Cannot save world when it's not in a world");
			}
			this._timer = Stopwatch.StartNew();
			WorldDomain.CheckSanity();
			Events.RaiseBeforeSaveWorld(context);
			WorldInfo worldInfo = WorldDomain.GetWorldInfo();
			OperationAdder.Global_SaveEnding(worldInfo, GlobalDomain._compressionLevel);
		}

		// Token: 0x060054F3 RID: 21747 RVA: 0x002EB260 File Offset: 0x002E9460
		public void CheckArchiveInfoExist(sbyte archiveId, Action<bool> exists)
		{
			bool flag = !Common.IsInWorld();
			if (flag)
			{
				throw new Exception("Currently not in a world");
			}
			this._timer = Stopwatch.StartNew();
			WorldDomain.CheckSanity();
			this._onEndingArchiveInfoLoaded = exists;
			OperationAdder.Global_GetEndingArchiveInfo(archiveId);
		}

		// Token: 0x060054F4 RID: 21748 RVA: 0x002EB2A4 File Offset: 0x002E94A4
		public void CompleteLoading(ushort domainId)
		{
			bool flag = this._loadingDomainIds != null;
			if (flag)
			{
				this._loadingDomainIds.Remove(domainId);
				bool flag2 = this._loadingDomainIds.Count <= 0;
				if (flag2)
				{
					this._loadingDomainIds = null;
					this.OnCurrWorldArchiveDataReady(DataContextManager.GetCurrentThreadDataContext(), false);
				}
			}
		}

		// Token: 0x060054F5 RID: 21749 RVA: 0x002EB2FC File Offset: 0x002E94FC
		public override void OnCurrWorldArchiveDataReady(DataContext context, bool isNewWorld)
		{
			bool loadedAllArchiveData = this._loadedAllArchiveData;
			if (loadedAllArchiveData)
			{
				GlobalDomain.Logger.Warn("OnCurrWorldArchiveDataReady called when _loadedAllArchiveData already set to true.");
			}
			else
			{
				foreach (BaseGameDataDomain domain in DomainManager.GetArchiveAttachedDomains())
				{
					domain.OnCurrWorldArchiveDataReady(context, isNewWorld);
				}
				this.SetLoadedAllArchiveData(true, context);
			}
		}

		// Token: 0x060054F6 RID: 21750 RVA: 0x002EB354 File Offset: 0x002E9554
		private void FixAllAbnormalDomainArchiveData(DataContext context, DataUid uid)
		{
			bool flag = !this._loadedAllArchiveData;
			if (!flag)
			{
				Stopwatch timer = GlobalDomain.StartTimer();
				foreach (BaseGameDataDomain domain in DomainManager.GetArchiveAttachedDomains())
				{
					domain.FixAbnormalDomainArchiveData(context);
				}
				base.RemovePostModificationHandler(uid, "FixAllAbnormalDomainArchiveData");
				GlobalDomain.StopTimer(timer, "FixAbnormalDomainArchiveData");
			}
		}

		// Token: 0x060054F7 RID: 21751 RVA: 0x002EB3B3 File Offset: 0x002E95B3
		private void InitBuildingEffect(DataContext context, DataUid uid)
		{
			DomainManager.Building.InitBuildingEffect();
		}

		// Token: 0x060054F8 RID: 21752 RVA: 0x002EB3C4 File Offset: 0x002E95C4
		[DomainMethod]
		public void SaveWorld(DataContext context)
		{
			bool flag = !Common.IsInWorld();
			if (flag)
			{
				throw new Exception("Cannot save world when it's not in a world");
			}
			DomainManager.Extra.UpdateSavingWorldVersionInfo(context);
			DomainManager.Extra.BackupDreamBackArchiveData(context);
			this._timer = Stopwatch.StartNew();
			WorldDomain.CheckSanity();
			Events.RaiseBeforeSaveWorld(context);
			bool flag2 = !this.CheckDriveSpace(context);
			if (!flag2)
			{
				this.SetSavingWorld(true, null);
				WorldInfo worldInfo = WorldDomain.GetWorldInfo();
				sbyte maxBackupsCount = GlobalDomain.ShouldMakeBackup();
				OperationAdder.Global_SaveWorld(worldInfo, maxBackupsCount, GlobalDomain._compressionLevel);
			}
		}

		// Token: 0x060054F9 RID: 21753 RVA: 0x002EB44C File Offset: 0x002E964C
		[DomainMethod]
		public void LeaveWorld()
		{
			bool flag = !Common.IsInWorld();
			if (flag)
			{
				throw new Exception("Cannot leave world when it's not in a world");
			}
			DomainManager.ResetArchiveAttachedDomains();
			GameDataBridge.ClearMonitoredData();
			Events.ClearAllHandlers();
			OperationAdder.Global_LeaveWorld();
			ObjectPoolManager.Initialize();
			WorkerThreadManager.ReInitialize();
			Common.ResetArchiveId();
			this.SetLoadedAllArchiveData(false, null);
		}

		// Token: 0x060054FA RID: 21754 RVA: 0x002EB4A4 File Offset: 0x002E96A4
		[DomainMethod(IsPassthrough = true)]
		public void GetArchivesInfo(uint operationId)
		{
			OperationAdder.Global_GetArchivesInfo(true, operationId);
		}

		// Token: 0x060054FB RID: 21755 RVA: 0x002EB4B0 File Offset: 0x002E96B0
		[DomainMethod]
		public void DeleteArchive(sbyte archiveId)
		{
			bool flag = !Common.CheckArchiveId(archiveId);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid archiveId: ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(archiveId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			OperationAdder.Global_DeleteArchive(archiveId);
		}

		// Token: 0x060054FC RID: 21756 RVA: 0x002EB4FF File Offset: 0x002E96FF
		[DomainMethod]
		public void SetGameVersion(string gameVersion)
		{
			GlobalDomain._gameVersion = gameVersion;
		}

		// Token: 0x060054FD RID: 21757 RVA: 0x002EB508 File Offset: 0x002E9708
		[DomainMethod]
		public void SetCompressionLevel(sbyte compressionLevel)
		{
			GlobalDomain._compressionLevel = Math.Clamp(compressionLevel, 0, 9);
		}

		// Token: 0x060054FE RID: 21758 RVA: 0x002EB519 File Offset: 0x002E9719
		public string GetGameVersion()
		{
			return GlobalDomain._gameVersion;
		}

		// Token: 0x060054FF RID: 21759 RVA: 0x002EB520 File Offset: 0x002E9720
		private static sbyte ShouldMakeBackup()
		{
			sbyte backupInterval = DomainManager.World.GetArchiveFilesBackupInterval();
			bool flag = backupInterval <= 0;
			sbyte result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int currDate = DomainManager.World.GetCurrDate();
				result = ((currDate % (int)backupInterval == 0) ? DomainManager.World.GetArchiveFilesBackupCount() : 0);
			}
			return result;
		}

		// Token: 0x06005500 RID: 21760 RVA: 0x002EB56C File Offset: 0x002E976C
		[DomainMethod]
		public bool CheckDriveSpace(DataContext context)
		{
			string archiveBaseDir = Common.ArchiveBaseDir;
			string root = Path.GetPathRoot(archiveBaseDir);
			bool flag = root == null;
			bool result;
			if (flag)
			{
				GlobalDomain.Logger.Warn("Invalid root directory for path: " + archiveBaseDir);
				result = false;
			}
			else
			{
				long requiredSpace = 1073741824L;
				sbyte archiveId = Common.GetCurrArchiveId();
				bool flag2 = archiveId >= 0;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (flag2)
				{
					string path = archiveBaseDir;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(16, 1);
					defaultInterpolatedStringHandler.AppendLiteral("World_");
					defaultInterpolatedStringHandler.AppendFormatted<int>((int)(archiveId + 1));
					defaultInterpolatedStringHandler.AppendLiteral("/local.sav");
					string prevArchiveFilePath = Path.Combine(path, defaultInterpolatedStringHandler.ToStringAndClear());
					bool flag3 = File.Exists(prevArchiveFilePath);
					if (flag3)
					{
						FileInfo fileInfo = new FileInfo(prevArchiveFilePath);
						requiredSpace = Math.Max(requiredSpace, fileInfo.Length * 3L);
					}
				}
				DriveInfo driveInfo = new DriveInfo(root);
				Logger logger = GlobalDomain.Logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(41, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Required Space: ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(requiredSpace);
				defaultInterpolatedStringHandler.AppendLiteral(" bytes.\nAvailable Space: ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(driveInfo.AvailableFreeSpace);
				logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
				result = (driveInfo.AvailableFreeSpace >= requiredSpace);
			}
			return result;
		}

		// Token: 0x06005501 RID: 21761 RVA: 0x002EB69C File Offset: 0x002E989C
		[DomainMethod]
		public void UpdateSharedGlobalSettings(SharedGlobalSettings settings)
		{
			GlobalDomain.Settings = settings;
		}

		// Token: 0x06005502 RID: 21762 RVA: 0x002EB6A8 File Offset: 0x002E98A8
		[DomainMethod]
		public void ReloadAllConfigData()
		{
			LocalStringManager.Init(GlobalDomain.Settings.Language);
			Parallel.ForEach<IConfigData>(ConfigCollection.Items, delegate(IConfigData item)
			{
				item.Init();
			});
			RefNameMap.DoQueuedLoadRequests();
		}

		// Token: 0x06005503 RID: 21763 RVA: 0x002EB6F8 File Offset: 0x002E98F8
		[DomainMethod]
		public void PackAllCrossArchiveGameData()
		{
			GlobalDomain.Logger.Info("Start packing cross archive game data.");
			CrossArchiveGameData data = new CrossArchiveGameData();
			bool flag = this._crossArchiveGameData != null;
			if (flag)
			{
				data.SectZhujianGearMate = this._crossArchiveGameData.SectZhujianGearMate;
			}
			this._crossArchiveGameData = data;
			foreach (BaseGameDataDomain domain in DomainManager.Domains)
			{
				domain.PackCrossArchiveGameData(this._crossArchiveGameData);
			}
		}

		// Token: 0x06005504 RID: 21764 RVA: 0x002EB76C File Offset: 0x002E996C
		public void UnpackAllCrossArchiveGameData(DataContext context, DataUid uid)
		{
			bool flag = this._crossArchiveGameData == null;
			if (!flag)
			{
				foreach (BaseGameDataDomain domain in DomainManager.Domains)
				{
					domain.UnpackCrossArchiveGameData(context, this._crossArchiveGameData);
				}
				this._crossArchiveGameData = null;
				GlobalDomain.Logger.Info("Finish packing cross archive game data.");
			}
		}

		// Token: 0x06005505 RID: 21765 RVA: 0x002EB7C7 File Offset: 0x002E99C7
		[DomainMethod]
		public void SetGlobalFlag(DataContext context, sbyte flagType, bool value)
		{
			this._globalFlags = BitOperation.SetBit(this._globalFlags, (int)flagType, value);
			this.SetGlobalFlags(this._globalFlags, context);
		}

		// Token: 0x06005506 RID: 21766 RVA: 0x002EB7EB File Offset: 0x002E99EB
		[DomainMethod]
		public bool GetGlobalFlag(sbyte flagType)
		{
			return BitOperation.GetBit(this._globalFlags, (int)flagType);
		}

		// Token: 0x06005507 RID: 21767 RVA: 0x002EB7FC File Offset: 0x002E99FC
		[DomainMethod]
		public void InscribeCharacter(DataContext context, int charId)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			InscribedCharacter inscribedChar = this.CreateInscribedCharacter(character);
			uint worldId = DomainManager.World.GetWorldId();
			InscribedCharacterKey key = new InscribedCharacterKey(worldId, charId);
			this.AddElement_InscribedCharacters(key, inscribedChar, context);
		}

		// Token: 0x06005508 RID: 21768 RVA: 0x002EB83C File Offset: 0x002E9A3C
		private unsafe InscribedCharacter CreateInscribedCharacter(GameData.Domains.Character.Character character)
		{
			InscribedCharacter inscribedChar = new InscribedCharacter
			{
				Timestamp = DateTime.UtcNow.Ticks,
				Gender = character.GetGender(),
				ActualAge = character.GetActualAge(),
				CurrAge = character.GetCurrAge(),
				BaseMaxHealth = character.GetBaseMaxHealth(),
				Morality = character.GetMorality(),
				OrganizationInfo = character.GetOrganizationInfo(),
				Avatar = new AvatarData(character.GetAvatar()),
				ClothingDisplayId = character.GetClothingDisplayId(),
				BirthMonth = character.GetBirthMonth(),
				BaseMainAttributes = character.GetBaseMainAttributes(),
				BaseLifeSkillQualifications = *character.GetBaseLifeSkillQualifications(),
				LifeSkillQualificationGrowthType = character.GetLifeSkillQualificationGrowthType(),
				BaseCombatSkillQualifications = *character.GetBaseCombatSkillQualifications(),
				CombatSkillQualificationGrowthType = character.GetCombatSkillQualificationGrowthType(),
				InnateSkillQualificationBonuses = new SkillQualificationBonus[2],
				FeatureIds = new List<short>()
			};
			InscribedCharacter inscribedCharacter = inscribedChar;
			InscribedCharacter inscribedCharacter2 = inscribedChar;
			ValueTuple<string, string> realName = CharacterDomain.GetRealName(character);
			inscribedCharacter.Surname = realName.Item1;
			inscribedCharacter2.GivenName = realName.Item2;
			character.GetInscribableFeatureIds(inscribedChar.FeatureIds);
			List<SkillQualificationBonus> skillQualificationBonuses = character.GetSkillQualificationBonuses();
			bool flag = skillQualificationBonuses.CheckIndex(0);
			if (flag)
			{
				inscribedChar.InnateSkillQualificationBonuses[0] = skillQualificationBonuses[0];
			}
			bool flag2 = skillQualificationBonuses.CheckIndex(1);
			if (flag2)
			{
				inscribedChar.InnateSkillQualificationBonuses[1] = skillQualificationBonuses[1];
			}
			return inscribedChar;
		}

		// Token: 0x06005509 RID: 21769 RVA: 0x002EB9B2 File Offset: 0x002E9BB2
		[DomainMethod]
		public void RemoveInscribedCharacter(DataContext context, InscribedCharacterKey key)
		{
			this.RemoveElement_InscribedCharacters(key, context);
		}

		// Token: 0x0600550A RID: 21770 RVA: 0x002EB9C0 File Offset: 0x002E9BC0
		public bool IsCharacterInscribed(InscribedCharacterKey key)
		{
			return this._inscribedCharacters.ContainsKey(key);
		}

		// Token: 0x0600550B RID: 21771 RVA: 0x002EB9E0 File Offset: 0x002E9BE0
		public static Stopwatch StartTimer()
		{
			return Stopwatch.StartNew();
		}

		// Token: 0x0600550C RID: 21772 RVA: 0x002EB9F8 File Offset: 0x002E9BF8
		public static void StopTimer(Stopwatch sw, string label)
		{
			sw.Stop();
			Logger logger = GlobalDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
			defaultInterpolatedStringHandler.AppendFormatted(label);
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600550D RID: 21773 RVA: 0x002EBA5C File Offset: 0x002E9C5C
		public static void ShowMemoryUsage()
		{
			long totalMemory = GC.GetTotalMemory(true);
			Logger logger = GlobalDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Total managed memory usage: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>((double)totalMemory / 1024.0 / 1024.0, "N1");
			defaultInterpolatedStringHandler.AppendLiteral("MB");
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600550E RID: 21774 RVA: 0x002EBACC File Offset: 0x002E9CCC
		private void RunTestGetCacheData(DataContext context)
		{
			context.Random.Reinitialise(1UL);
			this.EnterNewWorld(2);
			GameData.Domains.Character.Character[] characters = GlobalDomain.RunTestGetCacheData_CreateCharacters(context);
			GlobalDomain.RunTestGetCacheData_MultiThreads(context, characters);
			this.LeaveWorld();
		}

		// Token: 0x0600550F RID: 21775 RVA: 0x002EBB08 File Offset: 0x002E9D08
		private static GameData.Domains.Character.Character[] RunTestGetCacheData_CreateCharacters(DataContext context)
		{
			Logger logger = GlobalDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(16, 1);
			defaultInterpolatedStringHandler.AppendLiteral("ProcessorCount: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(Environment.ProcessorCount);
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			int charactersCount = 100000 * Environment.ProcessorCount;
			GameData.Domains.Character.Character[] characters = new GameData.Domains.Character.Character[charactersCount];
			IRandomSource random = context.Random;
			for (int i = 0; i < charactersCount; i++)
			{
				sbyte gender = Gender.GetRandom(random);
				sbyte orgTemplateId = OrganizationDomain.GetRandomSectOrgTemplateId(random, gender);
				sbyte grade = (sbyte)random.Next(9);
				IntelligentCharacterCreationInfo info = new IntelligentCharacterCreationInfo(new Location((short)random.Next(45), 0), new OrganizationInfo(orgTemplateId, grade, true, -1), (short)random.Next(1, 33));
				GameData.Domains.Character.Character character = DomainManager.Character.CreateIntelligentCharacter(context, ref info);
				List<short> featureIds = character.GetFeatureIds();
				featureIds.Clear();
				for (int j = 0; j < 10; j++)
				{
					int featureId = random.Next(171);
					featureIds.Add((short)featureId);
				}
				character.SetFeatureIds(featureIds, context);
				characters[i] = character;
				DomainManager.Character.CompleteCreatingCharacter(character.GetId());
			}
			return characters;
		}

		// Token: 0x06005510 RID: 21776 RVA: 0x002EBC48 File Offset: 0x002E9E48
		private static void RunTestGetCacheData_SingleThread(DataContext context, GameData.Domains.Character.Character[] characters)
		{
			Stopwatch sw = Stopwatch.StartNew();
			GlobalDomain.RunTestGetCacheData_GetCacheData(characters, 0, characters.Length);
			sw.Stop();
			Logger logger = GlobalDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Calc cache data: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			GlobalDomain.RunTestGetCacheData_GetCacheData(characters, 0, characters.Length);
			sw.Stop();
			Logger logger2 = GlobalDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Get cached data: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger2.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			GlobalDomain.RunTestGetCacheData_InvalidCache(context, characters);
			sw.Stop();
			Logger logger3 = GlobalDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Invalid cache: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger3.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			GlobalDomain.RunTestGetCacheData_GetCacheData(characters, 0, characters.Length);
			sw.Stop();
			Logger logger4 = GlobalDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Re-calc cache data: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger4.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			GlobalDomain.RunTestGetCacheData_GetCacheData(characters, 0, characters.Length);
			sw.Stop();
			Logger logger5 = GlobalDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Re-get cache data: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger5.Info(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06005511 RID: 21777 RVA: 0x002EBE24 File Offset: 0x002EA024
		private static void RunTestGetCacheData_GetCacheData(GameData.Domains.Character.Character[] characters, int indexBegin, int indexEnd)
		{
			for (int i = indexBegin; i < indexEnd; i++)
			{
				GameData.Domains.Character.Character character = characters[i];
				character.GetMaxMainAttributes();
				character.GetHitValues();
				character.GetPenetrations();
				character.GetAvoidValues();
				character.GetPenetrationResists();
				character.GetLifeSkillQualifications();
				character.GetCombatSkillQualifications();
				character.GetLifeSkillAttainments();
				character.GetCombatSkillAttainments();
			}
		}

		// Token: 0x06005512 RID: 21778 RVA: 0x002EBE88 File Offset: 0x002EA088
		private static void RunTestGetCacheData_GetCacheData(GameData.Domains.Character.Character character)
		{
			character.GetMaxMainAttributes();
			character.GetHitValues();
			character.GetPenetrations();
			character.GetAvoidValues();
			character.GetPenetrationResists();
			character.GetLifeSkillQualifications();
			character.GetCombatSkillQualifications();
			character.GetLifeSkillAttainments();
			character.GetCombatSkillAttainments();
		}

		// Token: 0x06005513 RID: 21779 RVA: 0x002EBED8 File Offset: 0x002EA0D8
		private static void RunTestGetCacheData_InvalidCache(DataContext context, GameData.Domains.Character.Character[] characters)
		{
			int i = 0;
			int charactersCount = characters.Length;
			while (i < charactersCount)
			{
				GameData.Domains.Character.Character character = characters[i];
				List<short> featureIds = character.GetFeatureIds();
				character.SetFeatureIds(featureIds, context);
				i++;
			}
		}

		// Token: 0x06005514 RID: 21780 RVA: 0x002EBF14 File Offset: 0x002EA114
		private static void RunTestGetCacheData_MultiThreads(DataContext context, GameData.Domains.Character.Character[] characters)
		{
			Action<GameData.Domains.Character.Character> getCacheDataAction = new Action<GameData.Domains.Character.Character>(GlobalDomain.RunTestGetCacheData_GetCacheData);
			Stopwatch sw = Stopwatch.StartNew();
			Parallel.ForEach<GameData.Domains.Character.Character>(characters, getCacheDataAction);
			sw.Stop();
			Logger logger = GlobalDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Calc cache data: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			Parallel.ForEach<GameData.Domains.Character.Character>(characters, getCacheDataAction);
			sw.Stop();
			Logger logger2 = GlobalDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Get cached data: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger2.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			GlobalDomain.RunTestGetCacheData_InvalidCache(context, characters);
			sw.Stop();
			Logger logger3 = GlobalDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Invalid cache: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger3.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			Parallel.ForEach<GameData.Domains.Character.Character>(characters, getCacheDataAction);
			sw.Stop();
			Logger logger4 = GlobalDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Re-calc cache data: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger4.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			Parallel.ForEach<GameData.Domains.Character.Character>(characters, getCacheDataAction);
			sw.Stop();
			Logger logger5 = GlobalDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Re-get cache data: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger5.Info(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06005515 RID: 21781 RVA: 0x002EC0F0 File Offset: 0x002EA2F0
		public void RunTestCompileDll()
		{
		}

		// Token: 0x06005516 RID: 21782 RVA: 0x002EC0F4 File Offset: 0x002EA2F4
		public static void RunTestRandomGenerators()
		{
			Random systemRandomSource = new Random();
			IRandomSource redzenDefaultRandomSource = RandomDefaults.CreateRandomSource();
			IRandomSource redzenFloatRandomSource = new Xoshiro256PlusRandomFactory().Create();
			Stopwatch sw = new Stopwatch();
			int[] samples = new int[10000000];
			sw.Restart();
			for (int i = 0; i < 10000000; i++)
			{
				samples[i] = RedzenHelper.NormalDistribute(redzenDefaultRandomSource, 0f, 1f, 0, 2);
			}
			GlobalDomain.RunTestRandomGenerators_ShowHistogram(sw, samples, "", 0, 3, 4);
			sw.Stop();
		}

		// Token: 0x06005517 RID: 21783 RVA: 0x002EC17C File Offset: 0x002EA37C
		private static void RunTestRandomGenerators_ShowHistogram(Stopwatch sw, int[] samples, string label, int min = 0, int max = 100, int bins = 10)
		{
			sw.Stop();
			Logger logger = GlobalDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
			defaultInterpolatedStringHandler.AppendFormatted(label);
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			Histogram histogram = new Histogram(min, max, bins);
			histogram.Record(samples);
			GlobalDomain.Logger.Info("Histogram:\n" + histogram.GetTextGraph(100));
			sw.Restart();
		}

		// Token: 0x06005518 RID: 21784 RVA: 0x002EC214 File Offset: 0x002EA414
		public static void RunTestSkewDistribute()
		{
			int[] samples = new int[10000];
			IRandomSource redzenDefaultRandomSource = RandomDefaults.CreateRandomSource();
			for (int i = 0; i < 10000; i++)
			{
				samples[i] = RedzenHelper.SkewDistribute(redzenDefaultRandomSource, 6f, 1.5f, 2f, 2, 12);
			}
			Stopwatch sw = new Stopwatch();
			GlobalDomain.RunTestRandomGenerators_ShowHistogram(sw, samples, "", 0, 13, 13);
		}

		// Token: 0x06005519 RID: 21785 RVA: 0x002EC280 File Offset: 0x002EA480
		public static void RunTestNormalDistribute()
		{
			int[] samples = new int[10000];
			IRandomSource redzenDefaultRandomSource = RandomDefaults.CreateRandomSource();
			for (int i = 0; i < 10000; i++)
			{
				samples[i] = RedzenHelper.NormalDistribute(redzenDefaultRandomSource, 4f, 1.2f);
			}
			Stopwatch sw = new Stopwatch();
			GlobalDomain.RunTestRandomGenerators_ShowHistogram(sw, samples, "", 0, 13, 13);
		}

		// Token: 0x0600551A RID: 21786 RVA: 0x002EC2E4 File Offset: 0x002EA4E4
		public static void RunTestAvatarGenerating(DataContext context)
		{
			Stopwatch sw = new Stopwatch();
			IRandomSource random = context.Random;
			int[] samples = new int[1000000];
			AvatarManager.Instance = new AvatarManager();
			sw.Restart();
			for (int i = 0; i < 1000000; i++)
			{
				short attraction = (short)random.Next(0, 901);
				AvatarData avatar = AvatarManager.Instance.GetRandomAvatar(random, Gender.GetRandom(random), false, BodyType.GetRandom(random), attraction);
				short newAttraction = avatar.GetBaseCharm();
				int delta = (int)(newAttraction - attraction);
				samples[i] = delta;
			}
			sw.Stop();
			Logger logger = GlobalDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			Histogram histogram = new Histogram(-5, 5, 11);
			histogram.Record(samples);
			GlobalDomain.Logger.Info("Histogram:\n" + histogram.GetTextGraph(100));
			Histogram histogram2 = new Histogram(-50, 50, 20);
			histogram2.Record(samples);
			GlobalDomain.Logger.Info("Histogram:\n" + histogram2.GetTextGraph(100));
			Histogram histogram3 = new Histogram(-200, 200, 20);
			histogram3.Record(samples);
			GlobalDomain.Logger.Info("Histogram:\n" + histogram3.GetTextGraph(100));
		}

		// Token: 0x0600551B RID: 21787 RVA: 0x002EC458 File Offset: 0x002EA658
		public static void RunTestCharacterQualificationGenerating(DataContext context)
		{
			int[][] samples = new int[][]
			{
				new int[1000],
				new int[1000]
			};
			Stopwatch sw = Stopwatch.StartNew();
			for (int i = 0; i < 1000; i++)
			{
				short qualification0 = GlobalDomain.CreateIntelligentCharacterAndGetQualification(context, 6, 11, 8, 4);
				short qualification = GlobalDomain.CreateIntelligentCharacterAndGetQualification(context, 7, 13, 8, 4);
				samples[0][i] = (int)qualification0;
				samples[1][i] = (int)qualification;
			}
			sw.Stop();
			Logger logger = GlobalDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			Histogram histogram = new Histogram(0, 120, 20);
			histogram.Record(samples[0]);
			GlobalDomain.Logger.Info("Histogram:\n" + histogram.GetTextGraph(100));
			Histogram histogram2 = new Histogram(0, 120, 20);
			histogram2.Record(samples[1]);
			GlobalDomain.Logger.Info("Histogram:\n" + histogram2.GetTextGraph(100));
		}

		// Token: 0x0600551C RID: 21788 RVA: 0x002EC578 File Offset: 0x002EA778
		private unsafe static short CreateIntelligentCharacterAndGetQualification(DataContext context, sbyte orgTemplateId, short charTemplateId, sbyte grade, sbyte lifeSkillType)
		{
			IntelligentCharacterCreationInfo info = new IntelligentCharacterCreationInfo(Location.Invalid, new OrganizationInfo(orgTemplateId, grade, true, -1), charTemplateId);
			GameData.Domains.Character.Character character = DomainManager.Character.CreateIntelligentCharacter(context, ref info);
			DomainManager.Character.CompleteCreatingCharacter(character.GetId());
			ref LifeSkillShorts qualifications = ref character.GetLifeSkillQualifications();
			return *(ref qualifications.Items.FixedElementField + (IntPtr)lifeSkillType * 2);
		}

		// Token: 0x0600551D RID: 21789 RVA: 0x002EC5D8 File Offset: 0x002EA7D8
		public static void RunTestCharacterCombatSkillEquipping(DataContext context)
		{
			double[] learnedSkillsCounts = new double[9];
			double[] equippedSkillsCounts = new double[9];
			Stopwatch sw = Stopwatch.StartNew();
			for (int i = 0; i < 1000; i++)
			{
				GameData.Domains.Character.Character character = DomainManager.Character.CreateRandomEnemy(context, 306, null);
				int charId = character.GetId();
				DomainManager.Character.CompleteCreatingCharacter(charId);
				List<short> learnedCombatSkills = character.GetLearnedCombatSkills();
				foreach (short skillTemplateId in learnedCombatSkills)
				{
					CombatSkillItem config = CombatSkill.Instance[skillTemplateId];
					learnedSkillsCounts[(int)config.Grade] += 1.0;
				}
				CombatSkillEquipment combatSkillEquipment = character.GetCombatSkillEquipment();
				foreach (short skillTemplateId2 in combatSkillEquipment)
				{
					bool flag = skillTemplateId2 < 0;
					if (!flag)
					{
						CombatSkillItem config2 = CombatSkill.Instance[skillTemplateId2];
						equippedSkillsCounts[(int)config2.Grade] += 1.0;
					}
				}
			}
			sw.Stop();
			Logger logger = GlobalDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			for (int j = 0; j < 9; j++)
			{
				learnedSkillsCounts[j] /= 1000.0;
				equippedSkillsCounts[j] /= 1000.0;
			}
			Logger logger2 = GlobalDomain.Logger;
			string str = "Learned skills:";
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 3);
			defaultInterpolatedStringHandler.AppendLiteral("  ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(learnedSkillsCounts[0], "N1");
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(learnedSkillsCounts[1], "N1");
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(learnedSkillsCounts[2], "N1");
			defaultInterpolatedStringHandler.AppendLiteral(";");
			string str2 = defaultInterpolatedStringHandler.ToStringAndClear();
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 3);
			defaultInterpolatedStringHandler.AppendLiteral("  ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(learnedSkillsCounts[3], "N1");
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(learnedSkillsCounts[4], "N1");
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(learnedSkillsCounts[5], "N1");
			defaultInterpolatedStringHandler.AppendLiteral(";");
			string str3 = defaultInterpolatedStringHandler.ToStringAndClear();
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 3);
			defaultInterpolatedStringHandler.AppendLiteral("  ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(learnedSkillsCounts[6], "N1");
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(learnedSkillsCounts[7], "N1");
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(learnedSkillsCounts[8], "N1");
			defaultInterpolatedStringHandler.AppendLiteral(";");
			logger2.Info(str + str2 + str3 + defaultInterpolatedStringHandler.ToStringAndClear());
			Logger logger3 = GlobalDomain.Logger;
			string str4 = "Equipped skills:";
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 3);
			defaultInterpolatedStringHandler.AppendLiteral("  ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(equippedSkillsCounts[0], "N1");
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(equippedSkillsCounts[1], "N1");
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(equippedSkillsCounts[2], "N1");
			defaultInterpolatedStringHandler.AppendLiteral(";");
			string str5 = defaultInterpolatedStringHandler.ToStringAndClear();
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 3);
			defaultInterpolatedStringHandler.AppendLiteral("  ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(equippedSkillsCounts[3], "N1");
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(equippedSkillsCounts[4], "N1");
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(equippedSkillsCounts[5], "N1");
			defaultInterpolatedStringHandler.AppendLiteral(";");
			string str6 = defaultInterpolatedStringHandler.ToStringAndClear();
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 3);
			defaultInterpolatedStringHandler.AppendLiteral("  ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(equippedSkillsCounts[6], "N1");
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(equippedSkillsCounts[7], "N1");
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(equippedSkillsCounts[8], "N1");
			defaultInterpolatedStringHandler.AppendLiteral(";");
			logger3.Info(str4 + str5 + str6 + defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600551E RID: 21790 RVA: 0x002ECA94 File Offset: 0x002EAC94
		public static void RunTestNameGenerating(DataContext context)
		{
			IRandomSource random = context.Random;
			int itemsCount = LocalSurnames.Instance.SurnameCore.Length;
			int totalWeight = 0;
			for (int i = 0; i < itemsCount; i++)
			{
				SurnameItem item = LocalSurnames.Instance.SurnameCore[i];
				bool flag = item != null;
				if (flag)
				{
					totalWeight += (int)item.Prob;
				}
			}
			double unitWeight = 1.0 / (double)totalWeight;
			int[] surnamesCounts = new int[itemsCount];
			Stopwatch sw = Stopwatch.StartNew();
			for (int j = 0; j < 1000000; j++)
			{
				sbyte gender = (sbyte)(j % 2);
				FullName fullName = CharacterDomain.GenerateRandomHanName(random, -1, -1, gender, -1);
				short surnameId = fullName.SurnameId;
				surnamesCounts[(int)surnameId]++;
			}
			sw.Stop();
			Logger logger = GlobalDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			StringBuilder message = new StringBuilder("Ratios:\n");
			for (int k = 0; k < itemsCount; k++)
			{
				SurnameItem item2 = LocalSurnames.Instance.SurnameCore[k];
				bool flag2 = item2 != null;
				if (flag2)
				{
					double idealProb = (double)item2.Prob * unitWeight;
					double realProb = (double)surnamesCounts[k] / 1000000.0;
					double ratio = realProb / idealProb;
					StringBuilder stringBuilder = message;
					StringBuilder stringBuilder2 = stringBuilder;
					StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(2, 2, stringBuilder);
					appendInterpolatedStringHandler.AppendFormatted(item2.Surname);
					appendInterpolatedStringHandler.AppendLiteral(": ");
					appendInterpolatedStringHandler.AppendFormatted<double>(ratio, "N3");
					stringBuilder2.AppendLine(ref appendInterpolatedStringHandler);
					bool flag3 = ratio < 0.9 || ratio > 1.1;
					if (flag3)
					{
						Logger logger2 = GlobalDomain.Logger;
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
						defaultInterpolatedStringHandler.AppendFormatted(item2.Surname);
						defaultInterpolatedStringHandler.AppendLiteral(": ");
						defaultInterpolatedStringHandler.AppendFormatted<double>(ratio, "N3");
						logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
				else
				{
					Tester.Assert(surnamesCounts[k] == 0, "");
				}
			}
			GlobalDomain.Logger.Info<StringBuilder>(message);
		}

		// Token: 0x0600551F RID: 21791 RVA: 0x002ECCD4 File Offset: 0x002EAED4
		public GlobalDomain() : base(5)
		{
			this._global = 0;
			this._loadedAllArchiveData = false;
			this._savingWorld = false;
			this._inscribedCharacters = new Dictionary<InscribedCharacterKey, InscribedCharacter>(0);
			this._globalFlags = 0UL;
			this.OnInitializedDomainData();
		}

		// Token: 0x06005520 RID: 21792 RVA: 0x002ECD2C File Offset: 0x002EAF2C
		private int GetGlobal()
		{
			return this._global;
		}

		// Token: 0x06005521 RID: 21793 RVA: 0x002ECD44 File Offset: 0x002EAF44
		private void SetGlobal(int value, DataContext context)
		{
			this._global = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, this.DataStates, GlobalDomain.CacheInfluences, context);
		}

		// Token: 0x06005522 RID: 21794 RVA: 0x002ECD64 File Offset: 0x002EAF64
		public bool GetLoadedAllArchiveData()
		{
			return this._loadedAllArchiveData;
		}

		// Token: 0x06005523 RID: 21795 RVA: 0x002ECD7C File Offset: 0x002EAF7C
		private void SetLoadedAllArchiveData(bool value, DataContext context)
		{
			this._loadedAllArchiveData = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, this.DataStates, GlobalDomain.CacheInfluences, context);
		}

		// Token: 0x06005524 RID: 21796 RVA: 0x002ECD9C File Offset: 0x002EAF9C
		public bool GetSavingWorld()
		{
			return this._savingWorld;
		}

		// Token: 0x06005525 RID: 21797 RVA: 0x002ECDB4 File Offset: 0x002EAFB4
		private void SetSavingWorld(bool value, DataContext context)
		{
			this._savingWorld = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(2, this.DataStates, GlobalDomain.CacheInfluences, context);
		}

		// Token: 0x06005526 RID: 21798 RVA: 0x002ECDD4 File Offset: 0x002EAFD4
		public InscribedCharacter GetElement_InscribedCharacters(InscribedCharacterKey elementId)
		{
			return this._inscribedCharacters[elementId];
		}

		// Token: 0x06005527 RID: 21799 RVA: 0x002ECDF4 File Offset: 0x002EAFF4
		public bool TryGetElement_InscribedCharacters(InscribedCharacterKey elementId, out InscribedCharacter value)
		{
			return this._inscribedCharacters.TryGetValue(elementId, out value);
		}

		// Token: 0x06005528 RID: 21800 RVA: 0x002ECE14 File Offset: 0x002EB014
		private unsafe void AddElement_InscribedCharacters(InscribedCharacterKey elementId, InscribedCharacter value, DataContext context)
		{
			this._inscribedCharacters.Add(elementId, value);
			this._modificationsInscribedCharacters.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, this.DataStates, GlobalDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<InscribedCharacterKey>(0, 3, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<InscribedCharacterKey>(0, 3, elementId, 0);
			}
		}

		// Token: 0x06005529 RID: 21801 RVA: 0x002ECE84 File Offset: 0x002EB084
		private unsafe void SetElement_InscribedCharacters(InscribedCharacterKey elementId, InscribedCharacter value, DataContext context)
		{
			this._inscribedCharacters[elementId] = value;
			this._modificationsInscribedCharacters.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, this.DataStates, GlobalDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<InscribedCharacterKey>(0, 3, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<InscribedCharacterKey>(0, 3, elementId, 0);
			}
		}

		// Token: 0x0600552A RID: 21802 RVA: 0x002ECEF1 File Offset: 0x002EB0F1
		private void RemoveElement_InscribedCharacters(InscribedCharacterKey elementId, DataContext context)
		{
			this._inscribedCharacters.Remove(elementId);
			this._modificationsInscribedCharacters.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, this.DataStates, GlobalDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<InscribedCharacterKey>(0, 3, elementId);
		}

		// Token: 0x0600552B RID: 21803 RVA: 0x002ECF2A File Offset: 0x002EB12A
		private void ClearInscribedCharacters(DataContext context)
		{
			this._inscribedCharacters.Clear();
			this._modificationsInscribedCharacters.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, this.DataStates, GlobalDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(0, 3);
		}

		// Token: 0x0600552C RID: 21804 RVA: 0x002ECF60 File Offset: 0x002EB160
		public ulong GetGlobalFlags()
		{
			return this._globalFlags;
		}

		// Token: 0x0600552D RID: 21805 RVA: 0x002ECF78 File Offset: 0x002EB178
		public unsafe void SetGlobalFlags(ulong value, DataContext context)
		{
			this._globalFlags = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, this.DataStates, GlobalDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(0, 4, 8);
			*(long*)pData = (long)this._globalFlags;
			pData += 8;
		}

		// Token: 0x0600552E RID: 21806 RVA: 0x002ECFB5 File Offset: 0x002EB1B5
		public override void OnInitializeGameDataModule()
		{
			this.InitializeOnInitializeGameDataModule();
		}

		// Token: 0x0600552F RID: 21807 RVA: 0x002ECFBF File Offset: 0x002EB1BF
		public override void OnEnterNewWorld()
		{
			this.InitializeOnEnterNewWorld();
			this.InitializeInternalDataOfCollections();
		}

		// Token: 0x06005530 RID: 21808 RVA: 0x002ECFD0 File Offset: 0x002EB1D0
		public override void OnLoadWorld()
		{
			this._pendingLoadingOperationIds = new Queue<uint>();
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(0, 3));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(0, 4));
		}

		// Token: 0x06005531 RID: 21809 RVA: 0x002ED004 File Offset: 0x002EB204
		public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
		{
			int result;
			switch (dataId)
			{
			case 0:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to get value of dataId: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 1:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 1);
				}
				result = Serializer.Serialize(this._loadedAllArchiveData, dataPool);
				break;
			case 2:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 2);
				}
				result = Serializer.Serialize(this._savingWorld, dataPool);
				break;
			case 3:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 3);
					this._modificationsInscribedCharacters.Reset();
				}
				result = Serializer.SerializeModifications<InscribedCharacterKey>(this._inscribedCharacters, dataPool);
				break;
			case 4:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 4);
				}
				result = Serializer.Serialize(this._globalFlags, dataPool);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06005532 RID: 21810 RVA: 0x002ED13C File Offset: 0x002EB33C
		public override void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			switch (dataId)
			{
			case 0:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 1:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 3:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 4:
				Serializer.Deserialize(dataPool, valueOffset, ref this._globalFlags);
				this.SetGlobalFlags(this._globalFlags, context);
				return;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06005533 RID: 21811 RVA: 0x002ED270 File Offset: 0x002EB470
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
				if (num != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte archiveId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref archiveId);
				this.EnterNewWorld(archiveId);
				result = -1;
				break;
			}
			case 1:
			{
				int argsCount2 = operation.ArgsCount;
				int num2 = argsCount2;
				if (num2 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte archiveId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref archiveId2);
				long backupTimestamp = 0L;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref backupTimestamp);
				this.LoadWorld(archiveId2, backupTimestamp);
				result = -1;
				break;
			}
			case 2:
			{
				int argsCount3 = operation.ArgsCount;
				int num3 = argsCount3;
				if (num3 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte archiveId3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref archiveId3);
				this.LoadEnding(archiveId3);
				result = -1;
				break;
			}
			case 3:
			{
				int argsCount4 = operation.ArgsCount;
				int num4 = argsCount4;
				if (num4 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.SaveWorld(context);
				result = -1;
				break;
			}
			case 4:
			{
				int argsCount5 = operation.ArgsCount;
				int num5 = argsCount5;
				if (num5 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.LeaveWorld();
				result = -1;
				break;
			}
			case 5:
			{
				int argsCount6 = operation.ArgsCount;
				int num6 = argsCount6;
				if (num6 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				uint operationId = GameDataBridge.RecordPassthroughMethod(operation);
				this.GetArchivesInfo(operationId);
				result = -1;
				break;
			}
			case 6:
			{
				int argsCount7 = operation.ArgsCount;
				int num7 = argsCount7;
				if (num7 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte archiveId4 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref archiveId4);
				this.DeleteArchive(archiveId4);
				result = -1;
				break;
			}
			case 7:
			{
				int argsCount8 = operation.ArgsCount;
				int num8 = argsCount8;
				if (num8 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId);
				this.InscribeCharacter(context, charId);
				result = -1;
				break;
			}
			case 8:
			{
				int argsCount9 = operation.ArgsCount;
				int num9 = argsCount9;
				if (num9 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				InscribedCharacterKey key = default(InscribedCharacterKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key);
				this.RemoveInscribedCharacter(context, key);
				result = -1;
				break;
			}
			case 9:
			{
				int argsCount10 = operation.ArgsCount;
				int num10 = argsCount10;
				if (num10 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string gameVersion = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref gameVersion);
				this.SetGameVersion(gameVersion);
				result = -1;
				break;
			}
			case 10:
			{
				int argsCount11 = operation.ArgsCount;
				int num11 = argsCount11;
				if (num11 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte compressionLevel = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref compressionLevel);
				this.SetCompressionLevel(compressionLevel);
				result = -1;
				break;
			}
			case 11:
			{
				int argsCount12 = operation.ArgsCount;
				int num12 = argsCount12;
				if (num12 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.PackAllCrossArchiveGameData();
				result = -1;
				break;
			}
			case 12:
			{
				int argsCount13 = operation.ArgsCount;
				int num13 = argsCount13;
				if (num13 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte flagType = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref flagType);
				bool value = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref value);
				this.SetGlobalFlag(context, flagType, value);
				result = -1;
				break;
			}
			case 13:
			{
				int argsCount14 = operation.ArgsCount;
				int num14 = argsCount14;
				if (num14 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte flagType2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref flagType2);
				bool returnValue = this.GetGlobalFlag(flagType2);
				result = Serializer.Serialize(returnValue, returnDataPool);
				break;
			}
			case 14:
			{
				int argsCount15 = operation.ArgsCount;
				int num15 = argsCount15;
				if (num15 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool returnValue2 = this.CheckDriveSpace(context);
				result = Serializer.Serialize(returnValue2, returnDataPool);
				break;
			}
			case 15:
			{
				int argsCount16 = operation.ArgsCount;
				int num16 = argsCount16;
				if (num16 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				SharedGlobalSettings settings = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref settings);
				this.UpdateSharedGlobalSettings(settings);
				result = -1;
				break;
			}
			case 16:
			{
				int argsCount17 = operation.ArgsCount;
				int num17 = argsCount17;
				if (num17 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.ReloadAllConfigData();
				result = -1;
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

		// Token: 0x06005534 RID: 21812 RVA: 0x002ED9C4 File Offset: 0x002EBBC4
		public override void OnMonitorData(ushort dataId, ulong subId0, uint subId1, bool monitoring)
		{
			switch (dataId)
			{
			case 0:
				break;
			case 1:
				break;
			case 2:
				break;
			case 3:
				this._modificationsInscribedCharacters.ChangeRecording(monitoring);
				break;
			case 4:
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06005535 RID: 21813 RVA: 0x002EDA38 File Offset: 0x002EBC38
		public override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
		{
			int result;
			switch (dataId)
			{
			case 0:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 1:
			{
				bool flag = !BaseGameDataDomain.IsModified(this.DataStates, 1);
				if (flag)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 1);
					result = Serializer.Serialize(this._loadedAllArchiveData, dataPool);
				}
				break;
			}
			case 2:
			{
				bool flag2 = !BaseGameDataDomain.IsModified(this.DataStates, 2);
				if (flag2)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 2);
					result = Serializer.Serialize(this._savingWorld, dataPool);
				}
				break;
			}
			case 3:
			{
				bool flag3 = !BaseGameDataDomain.IsModified(this.DataStates, 3);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 3);
					int offset = Serializer.SerializeModifications<InscribedCharacterKey>(this._inscribedCharacters, dataPool, this._modificationsInscribedCharacters);
					this._modificationsInscribedCharacters.Reset();
					result = offset;
				}
				break;
			}
			case 4:
			{
				bool flag4 = !BaseGameDataDomain.IsModified(this.DataStates, 4);
				if (flag4)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 4);
					result = Serializer.Serialize(this._globalFlags, dataPool);
				}
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06005536 RID: 21814 RVA: 0x002EDBCC File Offset: 0x002EBDCC
		public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			switch (dataId)
			{
			case 0:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 1:
			{
				bool flag = !BaseGameDataDomain.IsModified(this.DataStates, 1);
				if (!flag)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 1);
				}
				break;
			}
			case 2:
			{
				bool flag2 = !BaseGameDataDomain.IsModified(this.DataStates, 2);
				if (!flag2)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 2);
				}
				break;
			}
			case 3:
			{
				bool flag3 = !BaseGameDataDomain.IsModified(this.DataStates, 3);
				if (!flag3)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 3);
					this._modificationsInscribedCharacters.Reset();
				}
				break;
			}
			case 4:
			{
				bool flag4 = !BaseGameDataDomain.IsModified(this.DataStates, 4);
				if (!flag4)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 4);
				}
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06005537 RID: 21815 RVA: 0x002EDD0C File Offset: 0x002EBF0C
		public override bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			bool result;
			switch (dataId)
			{
			case 0:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to verify modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 1:
				result = BaseGameDataDomain.IsModified(this.DataStates, 1);
				break;
			case 2:
				result = BaseGameDataDomain.IsModified(this.DataStates, 2);
				break;
			case 3:
				result = BaseGameDataDomain.IsModified(this.DataStates, 3);
				break;
			case 4:
				result = BaseGameDataDomain.IsModified(this.DataStates, 4);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06005538 RID: 21816 RVA: 0x002EDDD4 File Offset: 0x002EBFD4
		public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			switch (influence.TargetIndicator.DataId)
			{
			case 0:
				break;
			case 1:
				break;
			case 2:
				break;
			case 3:
				break;
			case 4:
				break;
			default:
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(influence.TargetIndicator.DataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Cannot invalidate cache state of non-cache data ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(influence.TargetIndicator.DataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06005539 RID: 21817 RVA: 0x002EDE84 File Offset: 0x002EC084
		public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			switch (operation.DataId)
			{
			case 0:
				this.ProcessGlobalArchiveResponse(operation, pResult);
				break;
			case 1:
				goto IL_106;
			case 2:
				goto IL_106;
			case 3:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<InscribedCharacterKey, InscribedCharacter>(operation, pResult, this._inscribedCharacters);
				break;
			case 4:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single<ulong>(operation, pResult, ref this._globalFlags);
				break;
			default:
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.DataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
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
						DomainManager.Global.CompleteLoading(0);
					}
				}
			}
			return;
			IL_106:
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Cannot process archive response of non-archive data ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.DataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600553A RID: 21818 RVA: 0x002EDFCB File Offset: 0x002EC1CB
		private void InitializeInternalDataOfCollections()
		{
		}

		// Token: 0x0400172A RID: 5930
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x0400172B RID: 5931
		[DomainData(DomainDataType.SingleValue, false, false, false, false)]
		private int _global;

		// Token: 0x0400172C RID: 5932
		[DomainData(DomainDataType.SingleValue, false, false, true, false)]
		private bool _loadedAllArchiveData;

		// Token: 0x0400172D RID: 5933
		[DomainData(DomainDataType.SingleValue, false, false, true, false)]
		private bool _savingWorld;

		// Token: 0x0400172E RID: 5934
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, true)]
		private readonly Dictionary<InscribedCharacterKey, InscribedCharacter> _inscribedCharacters;

		// Token: 0x0400172F RID: 5935
		private static string _gameVersion;

		// Token: 0x04001730 RID: 5936
		public static SharedGlobalSettings Settings = new SharedGlobalSettings
		{
			Language = "CN"
		};

		// Token: 0x04001731 RID: 5937
		public bool GlobalDataLoaded;

		// Token: 0x04001732 RID: 5938
		private HashSet<ushort> _loadingDomainIds;

		// Token: 0x04001733 RID: 5939
		private Stopwatch _timer;

		// Token: 0x04001734 RID: 5940
		private Action<bool> _onEndingArchiveInfoLoaded = null;

		// Token: 0x04001735 RID: 5941
		private static sbyte _compressionLevel = 6;

		// Token: 0x04001736 RID: 5942
		private const sbyte DefaultCompression = 6;

		// Token: 0x04001737 RID: 5943
		private const sbyte NoCompression = 0;

		// Token: 0x04001738 RID: 5944
		private const sbyte FastCompression = 1;

		// Token: 0x04001739 RID: 5945
		private const sbyte BestCompression = 9;

		// Token: 0x0400173A RID: 5946
		private CrossArchiveGameData _crossArchiveGameData;

		// Token: 0x0400173B RID: 5947
		[DomainData(DomainDataType.SingleValue, true, false, true, true)]
		private ulong _globalFlags;

		// Token: 0x0400173C RID: 5948
		private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[5][];

		// Token: 0x0400173D RID: 5949
		private SingleValueCollectionModificationCollection<InscribedCharacterKey> _modificationsInscribedCharacters = SingleValueCollectionModificationCollection<InscribedCharacterKey>.Create();

		// Token: 0x0400173E RID: 5950
		private Queue<uint> _pendingLoadingOperationIds;
	}
}
