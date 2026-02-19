using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace GameData.Domains.Global;

[GameDataDomain(0, ArchiveAttached = false, CustomArchiveModuleCode = true)]
public class GlobalDomain : BaseGameDataDomain
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	[DomainData(DomainDataType.SingleValue, false, false, false, false)]
	private int _global;

	[DomainData(DomainDataType.SingleValue, false, false, true, false)]
	private bool _loadedAllArchiveData;

	[DomainData(DomainDataType.SingleValue, false, false, true, false)]
	private bool _savingWorld;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, true)]
	private readonly Dictionary<InscribedCharacterKey, InscribedCharacter> _inscribedCharacters;

	private static string _gameVersion;

	public static SharedGlobalSettings Settings = new SharedGlobalSettings
	{
		Language = "CN"
	};

	public bool GlobalDataLoaded;

	private HashSet<ushort> _loadingDomainIds;

	private Stopwatch _timer;

	private Action<bool> _onEndingArchiveInfoLoaded = null;

	private static sbyte _compressionLevel = 6;

	private const sbyte DefaultCompression = 6;

	private const sbyte NoCompression = 0;

	private const sbyte FastCompression = 1;

	private const sbyte BestCompression = 9;

	private CrossArchiveGameData _crossArchiveGameData;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private ulong _globalFlags;

	private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[5][];

	private SingleValueCollectionModificationCollection<InscribedCharacterKey> _modificationsInscribedCharacters = SingleValueCollectionModificationCollection<InscribedCharacterKey>.Create();

	private Queue<uint> _pendingLoadingOperationIds;

	private void OnInitializedDomainData()
	{
		GlobalDataLoaded = false;
	}

	private void InitializeOnInitializeGameDataModule()
	{
	}

	private void InitializeOnEnterNewWorld()
	{
	}

	private void OnLoadedArchiveData()
	{
		GlobalDataLoaded = true;
	}

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
			SetSavingWorld(value: false, null);
			_timer.Stop();
			int currDate = DomainManager.World.GetCurrDate();
			Logger.Info($"[[{currDate}]] Save world: {_timer.Elapsed.TotalMilliseconds:F1}");
			break;
		}
		case 4:
			break;
		case 5:
		{
			ResponseProcessor.Global_GetArchivesInfo(pResult, out var archivesInfo);
			GameData.GameDataBridge.GameDataBridge.TryReturnPassthroughMethod(operation.Id, archivesInfo);
			break;
		}
		case 6:
			break;
		case 8:
			break;
		case 7:
			break;
		case 9:
		{
			ResponseProcessor.Global_GetEndingArchiveInfo(pResult, out var archiveInfo);
			_onEndingArchiveInfoLoaded(archiveInfo.Status == 1);
			break;
		}
		default:
			throw new Exception($"Unsupported GlobalMethodId: {operation.MethodId}");
		}
	}

	public unsafe static void FreeMemory(IntPtr pointer)
	{
		GameData.ArchiveData.Common.FreeMemory((byte*)(void*)pointer);
	}

	[DomainMethod]
	public void EnterNewWorld(sbyte archiveId)
	{
		if (GameData.ArchiveData.Common.IsInWorld())
		{
			throw new Exception("Cannot enter new world when it's already in a world");
		}
		GameData.ArchiveData.Common.SetArchiveId(archiveId);
		SetLoadedAllArchiveData(value: false, null);
		OperationAdder.Global_EnterNewWorld(archiveId);
		DomainManager.ResetArchiveAttachedDomains();
		BaseGameDataDomain[] archiveAttachedDomains = DomainManager.GetArchiveAttachedDomains();
		foreach (BaseGameDataDomain baseGameDataDomain in archiveAttachedDomains)
		{
			baseGameDataDomain.OnEnterNewWorld();
		}
	}

	[DomainMethod]
	public void LoadWorld(sbyte archiveId, long backupTimestamp)
	{
		if (GameData.ArchiveData.Common.IsInWorld())
		{
			throw new Exception("Cannot load world when it's already in a world");
		}
		GameData.ArchiveData.Common.SetArchiveId(archiveId);
		SetLoadedAllArchiveData(value: false, null);
		OperationAdder.Global_LoadWorld(archiveId, backupTimestamp);
		_loadingDomainIds = new HashSet<ushort>(DomainManager.ArchiveAttachedDomainIds);
		DataUid uid = new DataUid(0, 1, ulong.MaxValue);
		AddPostModificationHandler(uid, "RegisterItemOwners", ItemDomain.RegisterItemOwners);
		AddPostModificationHandler(uid, "FixAllAbnormalDomainArchiveData", FixAllAbnormalDomainArchiveData);
		AddPostModificationHandler(uid, "InitBuildingEffect", InitBuildingEffect);
		DomainManager.ResetArchiveAttachedDomains();
		BaseGameDataDomain[] archiveAttachedDomains = DomainManager.GetArchiveAttachedDomains();
		foreach (BaseGameDataDomain baseGameDataDomain in archiveAttachedDomains)
		{
			baseGameDataDomain.OnLoadWorld();
		}
	}

	[DomainMethod]
	public void LoadEnding(sbyte archiveId)
	{
		if (GameData.ArchiveData.Common.IsInWorld())
		{
			throw new Exception("Need to exit the current world first.");
		}
		GameData.ArchiveData.Common.SetArchiveId(archiveId);
		SetLoadedAllArchiveData(value: false, null);
		OperationAdder.Global_LoadEnding(archiveId);
		_loadingDomainIds = new HashSet<ushort>(DomainManager.ArchiveAttachedDomainIds);
		DataUid uid = new DataUid(0, 1, ulong.MaxValue);
		AddPostModificationHandler(uid, "RegisterItemOwners", ItemDomain.RegisterItemOwners);
		AddPostModificationHandler(uid, "UnpackAllCrossArchiveGameData", UnpackAllCrossArchiveGameData);
		AddPostModificationHandler(uid, "FixAllAbnormalDomainArchiveData", FixAllAbnormalDomainArchiveData);
		AddPostModificationHandler(uid, "InitBuildingEffect", InitBuildingEffect);
		DomainManager.ResetArchiveAttachedDomains();
		BaseGameDataDomain[] archiveAttachedDomains = DomainManager.GetArchiveAttachedDomains();
		foreach (BaseGameDataDomain baseGameDataDomain in archiveAttachedDomains)
		{
			baseGameDataDomain.OnLoadWorld();
		}
	}

	public void SaveEnding(DataContext context)
	{
		if (!GameData.ArchiveData.Common.IsInWorld())
		{
			throw new Exception("Cannot save world when it's not in a world");
		}
		_timer = Stopwatch.StartNew();
		WorldDomain.CheckSanity();
		Events.RaiseBeforeSaveWorld(context);
		WorldInfo worldInfo = WorldDomain.GetWorldInfo();
		OperationAdder.Global_SaveEnding(worldInfo, _compressionLevel);
	}

	public void CheckArchiveInfoExist(sbyte archiveId, Action<bool> exists)
	{
		if (!GameData.ArchiveData.Common.IsInWorld())
		{
			throw new Exception("Currently not in a world");
		}
		_timer = Stopwatch.StartNew();
		WorldDomain.CheckSanity();
		_onEndingArchiveInfoLoaded = exists;
		OperationAdder.Global_GetEndingArchiveInfo(archiveId);
	}

	public void CompleteLoading(ushort domainId)
	{
		if (_loadingDomainIds != null)
		{
			_loadingDomainIds.Remove(domainId);
			if (_loadingDomainIds.Count <= 0)
			{
				_loadingDomainIds = null;
				OnCurrWorldArchiveDataReady(DataContextManager.GetCurrentThreadDataContext(), isNewWorld: false);
			}
		}
	}

	public override void OnCurrWorldArchiveDataReady(DataContext context, bool isNewWorld)
	{
		if (_loadedAllArchiveData)
		{
			Logger.Warn("OnCurrWorldArchiveDataReady called when _loadedAllArchiveData already set to true.");
			return;
		}
		BaseGameDataDomain[] archiveAttachedDomains = DomainManager.GetArchiveAttachedDomains();
		foreach (BaseGameDataDomain baseGameDataDomain in archiveAttachedDomains)
		{
			baseGameDataDomain.OnCurrWorldArchiveDataReady(context, isNewWorld);
		}
		SetLoadedAllArchiveData(value: true, context);
	}

	private void FixAllAbnormalDomainArchiveData(DataContext context, DataUid uid)
	{
		if (_loadedAllArchiveData)
		{
			Stopwatch sw = StartTimer();
			BaseGameDataDomain[] archiveAttachedDomains = DomainManager.GetArchiveAttachedDomains();
			foreach (BaseGameDataDomain baseGameDataDomain in archiveAttachedDomains)
			{
				baseGameDataDomain.FixAbnormalDomainArchiveData(context);
			}
			RemovePostModificationHandler(uid, "FixAllAbnormalDomainArchiveData");
			StopTimer(sw, "FixAbnormalDomainArchiveData");
		}
	}

	private void InitBuildingEffect(DataContext context, DataUid uid)
	{
		DomainManager.Building.InitBuildingEffect();
	}

	[DomainMethod]
	public void SaveWorld(DataContext context)
	{
		if (!GameData.ArchiveData.Common.IsInWorld())
		{
			throw new Exception("Cannot save world when it's not in a world");
		}
		DomainManager.Extra.UpdateSavingWorldVersionInfo(context);
		DomainManager.Extra.BackupDreamBackArchiveData(context);
		_timer = Stopwatch.StartNew();
		WorldDomain.CheckSanity();
		Events.RaiseBeforeSaveWorld(context);
		if (CheckDriveSpace(context))
		{
			SetSavingWorld(value: true, null);
			WorldInfo worldInfo = WorldDomain.GetWorldInfo();
			sbyte maxBackupsCount = ShouldMakeBackup();
			OperationAdder.Global_SaveWorld(worldInfo, maxBackupsCount, _compressionLevel);
		}
	}

	[DomainMethod]
	public void LeaveWorld()
	{
		if (!GameData.ArchiveData.Common.IsInWorld())
		{
			throw new Exception("Cannot leave world when it's not in a world");
		}
		DomainManager.ResetArchiveAttachedDomains();
		GameData.GameDataBridge.GameDataBridge.ClearMonitoredData();
		Events.ClearAllHandlers();
		OperationAdder.Global_LeaveWorld();
		ObjectPoolManager.Initialize();
		WorkerThreadManager.ReInitialize();
		GameData.ArchiveData.Common.ResetArchiveId();
		SetLoadedAllArchiveData(value: false, null);
	}

	[DomainMethod(IsPassthrough = true)]
	public void GetArchivesInfo(uint operationId)
	{
		OperationAdder.Global_GetArchivesInfo(isPassthrough: true, operationId);
	}

	[DomainMethod]
	public void DeleteArchive(sbyte archiveId)
	{
		if (!GameData.ArchiveData.Common.CheckArchiveId(archiveId))
		{
			throw new Exception($"Invalid archiveId: {archiveId}");
		}
		OperationAdder.Global_DeleteArchive(archiveId);
	}

	[DomainMethod]
	public void SetGameVersion(string gameVersion)
	{
		_gameVersion = gameVersion;
	}

	[DomainMethod]
	public void SetCompressionLevel(sbyte compressionLevel)
	{
		_compressionLevel = Math.Clamp(compressionLevel, 0, 9);
	}

	public string GetGameVersion()
	{
		return _gameVersion;
	}

	private static sbyte ShouldMakeBackup()
	{
		sbyte archiveFilesBackupInterval = DomainManager.World.GetArchiveFilesBackupInterval();
		if (archiveFilesBackupInterval <= 0)
		{
			return 0;
		}
		int currDate = DomainManager.World.GetCurrDate();
		return (sbyte)((currDate % archiveFilesBackupInterval == 0) ? DomainManager.World.GetArchiveFilesBackupCount() : 0);
	}

	[DomainMethod]
	public bool CheckDriveSpace(DataContext context)
	{
		string archiveBaseDir = GameData.ArchiveData.Common.ArchiveBaseDir;
		string pathRoot = Path.GetPathRoot(archiveBaseDir);
		if (pathRoot == null)
		{
			Logger.Warn("Invalid root directory for path: " + archiveBaseDir);
			return false;
		}
		long num = 1073741824L;
		sbyte currArchiveId = GameData.ArchiveData.Common.GetCurrArchiveId();
		if (currArchiveId >= 0)
		{
			string text = Path.Combine(archiveBaseDir, $"World_{currArchiveId + 1}/local.sav");
			if (File.Exists(text))
			{
				FileInfo fileInfo = new FileInfo(text);
				num = Math.Max(num, fileInfo.Length * 3);
			}
		}
		DriveInfo driveInfo = new DriveInfo(pathRoot);
		Logger.Info($"Required Space: {num} bytes.\nAvailable Space: {driveInfo.AvailableFreeSpace}");
		return driveInfo.AvailableFreeSpace >= num;
	}

	[DomainMethod]
	public void UpdateSharedGlobalSettings(SharedGlobalSettings settings)
	{
		Settings = settings;
	}

	[DomainMethod]
	public void ReloadAllConfigData()
	{
		LocalStringManager.Init(Settings.Language);
		Parallel.ForEach(ConfigCollection.Items, delegate(IConfigData item)
		{
			item.Init();
		});
		RefNameMap.DoQueuedLoadRequests();
	}

	[DomainMethod]
	public void PackAllCrossArchiveGameData()
	{
		Logger.Info("Start packing cross archive game data.");
		CrossArchiveGameData crossArchiveGameData = new CrossArchiveGameData();
		if (_crossArchiveGameData != null)
		{
			crossArchiveGameData.SectZhujianGearMate = _crossArchiveGameData.SectZhujianGearMate;
		}
		_crossArchiveGameData = crossArchiveGameData;
		BaseGameDataDomain[] domains = DomainManager.Domains;
		foreach (BaseGameDataDomain baseGameDataDomain in domains)
		{
			baseGameDataDomain.PackCrossArchiveGameData(_crossArchiveGameData);
		}
	}

	public void UnpackAllCrossArchiveGameData(DataContext context, DataUid uid)
	{
		if (_crossArchiveGameData != null)
		{
			BaseGameDataDomain[] domains = DomainManager.Domains;
			foreach (BaseGameDataDomain baseGameDataDomain in domains)
			{
				baseGameDataDomain.UnpackCrossArchiveGameData(context, _crossArchiveGameData);
			}
			_crossArchiveGameData = null;
			Logger.Info("Finish packing cross archive game data.");
		}
	}

	[DomainMethod]
	public void SetGlobalFlag(DataContext context, sbyte flagType, bool value)
	{
		_globalFlags = BitOperation.SetBit(_globalFlags, (int)flagType, value);
		SetGlobalFlags(_globalFlags, context);
	}

	[DomainMethod]
	public bool GetGlobalFlag(sbyte flagType)
	{
		return BitOperation.GetBit(_globalFlags, (int)flagType);
	}

	[DomainMethod]
	public void InscribeCharacter(DataContext context, int charId)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		InscribedCharacter value = CreateInscribedCharacter(element_Objects);
		uint worldId = DomainManager.World.GetWorldId();
		InscribedCharacterKey elementId = new InscribedCharacterKey(worldId, charId);
		AddElement_InscribedCharacters(elementId, value, context);
	}

	private InscribedCharacter CreateInscribedCharacter(GameData.Domains.Character.Character character)
	{
		InscribedCharacter inscribedCharacter = new InscribedCharacter();
		inscribedCharacter.Timestamp = DateTime.UtcNow.Ticks;
		inscribedCharacter.Gender = character.GetGender();
		inscribedCharacter.ActualAge = character.GetActualAge();
		inscribedCharacter.CurrAge = character.GetCurrAge();
		inscribedCharacter.BaseMaxHealth = character.GetBaseMaxHealth();
		inscribedCharacter.Morality = character.GetMorality();
		inscribedCharacter.OrganizationInfo = character.GetOrganizationInfo();
		inscribedCharacter.Avatar = new AvatarData(character.GetAvatar());
		inscribedCharacter.ClothingDisplayId = character.GetClothingDisplayId();
		inscribedCharacter.BirthMonth = character.GetBirthMonth();
		inscribedCharacter.BaseMainAttributes = character.GetBaseMainAttributes();
		inscribedCharacter.BaseLifeSkillQualifications = character.GetBaseLifeSkillQualifications();
		inscribedCharacter.LifeSkillQualificationGrowthType = character.GetLifeSkillQualificationGrowthType();
		inscribedCharacter.BaseCombatSkillQualifications = character.GetBaseCombatSkillQualifications();
		inscribedCharacter.CombatSkillQualificationGrowthType = character.GetCombatSkillQualificationGrowthType();
		inscribedCharacter.InnateSkillQualificationBonuses = new SkillQualificationBonus[2];
		inscribedCharacter.FeatureIds = new List<short>();
		InscribedCharacter inscribedCharacter2 = inscribedCharacter;
		(inscribedCharacter2.Surname, inscribedCharacter2.GivenName) = CharacterDomain.GetRealName(character);
		character.GetInscribableFeatureIds(inscribedCharacter2.FeatureIds);
		List<SkillQualificationBonus> skillQualificationBonuses = character.GetSkillQualificationBonuses();
		if (skillQualificationBonuses.CheckIndex(0))
		{
			inscribedCharacter2.InnateSkillQualificationBonuses[0] = skillQualificationBonuses[0];
		}
		if (skillQualificationBonuses.CheckIndex(1))
		{
			inscribedCharacter2.InnateSkillQualificationBonuses[1] = skillQualificationBonuses[1];
		}
		return inscribedCharacter2;
	}

	[DomainMethod]
	public void RemoveInscribedCharacter(DataContext context, InscribedCharacterKey key)
	{
		RemoveElement_InscribedCharacters(key, context);
	}

	public bool IsCharacterInscribed(InscribedCharacterKey key)
	{
		return _inscribedCharacters.ContainsKey(key);
	}

	public static Stopwatch StartTimer()
	{
		return Stopwatch.StartNew();
	}

	public static void StopTimer(Stopwatch sw, string label)
	{
		sw.Stop();
		Logger.Info($"{label}: {sw.Elapsed.TotalMilliseconds:N1}");
	}

	public static void ShowMemoryUsage()
	{
		long totalMemory = GC.GetTotalMemory(forceFullCollection: true);
		Logger.Info($"Total managed memory usage: {(double)totalMemory / 1024.0 / 1024.0:N1}MB");
	}

	private void RunTestGetCacheData(DataContext context)
	{
		context.Random.Reinitialise(1uL);
		EnterNewWorld(2);
		GameData.Domains.Character.Character[] characters = RunTestGetCacheData_CreateCharacters(context);
		RunTestGetCacheData_MultiThreads(context, characters);
		LeaveWorld();
	}

	private static GameData.Domains.Character.Character[] RunTestGetCacheData_CreateCharacters(DataContext context)
	{
		Logger.Info($"ProcessorCount: {Environment.ProcessorCount}");
		int num = 100000 * Environment.ProcessorCount;
		GameData.Domains.Character.Character[] array = new GameData.Domains.Character.Character[num];
		IRandomSource random = context.Random;
		for (int i = 0; i < num; i++)
		{
			sbyte random2 = Gender.GetRandom(random);
			sbyte randomSectOrgTemplateId = OrganizationDomain.GetRandomSectOrgTemplateId(random, random2);
			sbyte grade = (sbyte)random.Next(9);
			IntelligentCharacterCreationInfo info = new IntelligentCharacterCreationInfo(new Location((short)random.Next(45), 0), new OrganizationInfo(randomSectOrgTemplateId, grade, principal: true, -1), (short)random.Next(1, 33));
			GameData.Domains.Character.Character character = DomainManager.Character.CreateIntelligentCharacter(context, ref info);
			List<short> featureIds = character.GetFeatureIds();
			featureIds.Clear();
			for (int j = 0; j < 10; j++)
			{
				int num2 = random.Next(171);
				featureIds.Add((short)num2);
			}
			character.SetFeatureIds(featureIds, context);
			array[i] = character;
			DomainManager.Character.CompleteCreatingCharacter(character.GetId());
		}
		return array;
	}

	private static void RunTestGetCacheData_SingleThread(DataContext context, GameData.Domains.Character.Character[] characters)
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		RunTestGetCacheData_GetCacheData(characters, 0, characters.Length);
		stopwatch.Stop();
		Logger.Info($"Calc cache data: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		RunTestGetCacheData_GetCacheData(characters, 0, characters.Length);
		stopwatch.Stop();
		Logger.Info($"Get cached data: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		RunTestGetCacheData_InvalidCache(context, characters);
		stopwatch.Stop();
		Logger.Info($"Invalid cache: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		RunTestGetCacheData_GetCacheData(characters, 0, characters.Length);
		stopwatch.Stop();
		Logger.Info($"Re-calc cache data: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		RunTestGetCacheData_GetCacheData(characters, 0, characters.Length);
		stopwatch.Stop();
		Logger.Info($"Re-get cache data: {stopwatch.Elapsed.TotalMilliseconds:N1}");
	}

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

	private static void RunTestGetCacheData_InvalidCache(DataContext context, GameData.Domains.Character.Character[] characters)
	{
		int i = 0;
		for (int num = characters.Length; i < num; i++)
		{
			GameData.Domains.Character.Character character = characters[i];
			List<short> featureIds = character.GetFeatureIds();
			character.SetFeatureIds(featureIds, context);
		}
	}

	private static void RunTestGetCacheData_MultiThreads(DataContext context, GameData.Domains.Character.Character[] characters)
	{
		Action<GameData.Domains.Character.Character> body = RunTestGetCacheData_GetCacheData;
		Stopwatch stopwatch = Stopwatch.StartNew();
		Parallel.ForEach(characters, body);
		stopwatch.Stop();
		Logger.Info($"Calc cache data: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		Parallel.ForEach(characters, body);
		stopwatch.Stop();
		Logger.Info($"Get cached data: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		RunTestGetCacheData_InvalidCache(context, characters);
		stopwatch.Stop();
		Logger.Info($"Invalid cache: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		Parallel.ForEach(characters, body);
		stopwatch.Stop();
		Logger.Info($"Re-calc cache data: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		Parallel.ForEach(characters, body);
		stopwatch.Stop();
		Logger.Info($"Re-get cache data: {stopwatch.Elapsed.TotalMilliseconds:N1}");
	}

	public void RunTestCompileDll()
	{
	}

	public static void RunTestRandomGenerators()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		Random random = new Random();
		IRandomSource randomSource = RandomDefaults.CreateRandomSource();
		IRandomSource val = new Xoshiro256PlusRandomFactory().Create();
		Stopwatch stopwatch = new Stopwatch();
		int[] array = new int[10000000];
		stopwatch.Restart();
		for (int i = 0; i < 10000000; i++)
		{
			array[i] = RedzenHelper.NormalDistribute(randomSource, 0f, 1f, 0, 2);
		}
		RunTestRandomGenerators_ShowHistogram(stopwatch, array, "", 0, 3, 4);
		stopwatch.Stop();
	}

	private static void RunTestRandomGenerators_ShowHistogram(Stopwatch sw, int[] samples, string label, int min = 0, int max = 100, int bins = 10)
	{
		sw.Stop();
		Logger.Info($"{label}: {sw.Elapsed.TotalMilliseconds:N1}");
		Histogram histogram = new Histogram(min, max, bins);
		histogram.Record(samples);
		Logger.Info("Histogram:\n" + histogram.GetTextGraph());
		sw.Restart();
	}

	public static void RunTestSkewDistribute()
	{
		int[] array = new int[10000];
		IRandomSource randomSource = RandomDefaults.CreateRandomSource();
		for (int i = 0; i < 10000; i++)
		{
			array[i] = RedzenHelper.SkewDistribute(randomSource, 6f, 1.5f, 2f, 2, 12);
		}
		Stopwatch sw = new Stopwatch();
		RunTestRandomGenerators_ShowHistogram(sw, array, "", 0, 13, 13);
	}

	public static void RunTestNormalDistribute()
	{
		int[] array = new int[10000];
		IRandomSource randomSource = RandomDefaults.CreateRandomSource();
		for (int i = 0; i < 10000; i++)
		{
			array[i] = RedzenHelper.NormalDistribute(randomSource, 4f, 1.2f);
		}
		Stopwatch sw = new Stopwatch();
		RunTestRandomGenerators_ShowHistogram(sw, array, "", 0, 13, 13);
	}

	public static void RunTestAvatarGenerating(DataContext context)
	{
		Stopwatch stopwatch = new Stopwatch();
		IRandomSource random = context.Random;
		int[] array = new int[1000000];
		AvatarManager.Instance = new AvatarManager();
		stopwatch.Restart();
		for (int i = 0; i < 1000000; i++)
		{
			short num = (short)random.Next(0, 901);
			AvatarData randomAvatar = AvatarManager.Instance.GetRandomAvatar(random, Gender.GetRandom(random), transgender: false, BodyType.GetRandom(random), num);
			short baseCharm = randomAvatar.GetBaseCharm();
			int num2 = baseCharm - num;
			array[i] = num2;
		}
		stopwatch.Stop();
		Logger.Info($"{stopwatch.Elapsed.TotalMilliseconds:N1}");
		Histogram histogram = new Histogram(-5, 5, 11);
		histogram.Record(array);
		Logger.Info("Histogram:\n" + histogram.GetTextGraph());
		Histogram histogram2 = new Histogram(-50, 50, 20);
		histogram2.Record(array);
		Logger.Info("Histogram:\n" + histogram2.GetTextGraph());
		Histogram histogram3 = new Histogram(-200, 200, 20);
		histogram3.Record(array);
		Logger.Info("Histogram:\n" + histogram3.GetTextGraph());
	}

	public static void RunTestCharacterQualificationGenerating(DataContext context)
	{
		int[][] array = new int[2][]
		{
			new int[1000],
			new int[1000]
		};
		Stopwatch stopwatch = Stopwatch.StartNew();
		for (int i = 0; i < 1000; i++)
		{
			short num = CreateIntelligentCharacterAndGetQualification(context, 6, 11, 8, 4);
			short num2 = CreateIntelligentCharacterAndGetQualification(context, 7, 13, 8, 4);
			array[0][i] = num;
			array[1][i] = num2;
		}
		stopwatch.Stop();
		Logger.Info($"{stopwatch.Elapsed.TotalMilliseconds:N1}");
		Histogram histogram = new Histogram(0, 120, 20);
		histogram.Record(array[0]);
		Logger.Info("Histogram:\n" + histogram.GetTextGraph());
		Histogram histogram2 = new Histogram(0, 120, 20);
		histogram2.Record(array[1]);
		Logger.Info("Histogram:\n" + histogram2.GetTextGraph());
	}

	private unsafe static short CreateIntelligentCharacterAndGetQualification(DataContext context, sbyte orgTemplateId, short charTemplateId, sbyte grade, sbyte lifeSkillType)
	{
		IntelligentCharacterCreationInfo info = new IntelligentCharacterCreationInfo(Location.Invalid, new OrganizationInfo(orgTemplateId, grade, principal: true, -1), charTemplateId);
		GameData.Domains.Character.Character character = DomainManager.Character.CreateIntelligentCharacter(context, ref info);
		DomainManager.Character.CompleteCreatingCharacter(character.GetId());
		return character.GetLifeSkillQualifications().Items[lifeSkillType];
	}

	public static void RunTestCharacterCombatSkillEquipping(DataContext context)
	{
		double[] array = new double[9];
		double[] array2 = new double[9];
		Stopwatch stopwatch = Stopwatch.StartNew();
		for (int i = 0; i < 1000; i++)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.CreateRandomEnemy(context, 306);
			int id = character.GetId();
			DomainManager.Character.CompleteCreatingCharacter(id);
			List<short> learnedCombatSkills = character.GetLearnedCombatSkills();
			foreach (short item in learnedCombatSkills)
			{
				CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[item];
				array[combatSkillItem.Grade] += 1.0;
			}
			CombatSkillEquipment combatSkillEquipment = character.GetCombatSkillEquipment();
			foreach (short item2 in combatSkillEquipment)
			{
				if (item2 >= 0)
				{
					CombatSkillItem combatSkillItem2 = Config.CombatSkill.Instance[item2];
					array2[combatSkillItem2.Grade] += 1.0;
				}
			}
		}
		stopwatch.Stop();
		Logger.Info($"{stopwatch.Elapsed.TotalMilliseconds:N1}");
		for (int j = 0; j < 9; j++)
		{
			array[j] /= 1000.0;
			array2[j] /= 1000.0;
		}
		Logger.Info("Learned skills:" + $"  {array[0]:N1}, {array[1]:N1}, {array[2]:N1};" + $"  {array[3]:N1}, {array[4]:N1}, {array[5]:N1};" + $"  {array[6]:N1}, {array[7]:N1}, {array[8]:N1};");
		Logger.Info("Equipped skills:" + $"  {array2[0]:N1}, {array2[1]:N1}, {array2[2]:N1};" + $"  {array2[3]:N1}, {array2[4]:N1}, {array2[5]:N1};" + $"  {array2[6]:N1}, {array2[7]:N1}, {array2[8]:N1};");
	}

	public static void RunTestNameGenerating(DataContext context)
	{
		IRandomSource random = context.Random;
		int num = LocalSurnames.Instance.SurnameCore.Length;
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			SurnameItem surnameItem = LocalSurnames.Instance.SurnameCore[i];
			if (surnameItem != null)
			{
				num2 += surnameItem.Prob;
			}
		}
		double num3 = 1.0 / (double)num2;
		int[] array = new int[num];
		Stopwatch stopwatch = Stopwatch.StartNew();
		for (int j = 0; j < 1000000; j++)
		{
			sbyte gender = (sbyte)(j % 2);
			short surnameId = CharacterDomain.GenerateRandomHanName(random, -1, -1, gender, -1).SurnameId;
			array[surnameId]++;
		}
		stopwatch.Stop();
		Logger.Info($"{stopwatch.Elapsed.TotalMilliseconds:N1}");
		StringBuilder stringBuilder = new StringBuilder("Ratios:\n");
		for (int k = 0; k < num; k++)
		{
			SurnameItem surnameItem2 = LocalSurnames.Instance.SurnameCore[k];
			if (surnameItem2 != null)
			{
				double num4 = (double)surnameItem2.Prob * num3;
				double num5 = (double)array[k] / 1000000.0;
				double num6 = num5 / num4;
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(2, 2, stringBuilder2);
				handler.AppendFormatted(surnameItem2.Surname);
				handler.AppendLiteral(": ");
				handler.AppendFormatted(num6, "N3");
				stringBuilder2.AppendLine(ref handler);
				if (!(num6 >= 0.9) || !(num6 <= 1.1))
				{
					Logger.Warn($"{surnameItem2.Surname}: {num6:N3}");
				}
			}
			else
			{
				Tester.Assert(array[k] == 0);
			}
		}
		Logger.Info<StringBuilder>(stringBuilder);
	}

	public GlobalDomain()
		: base(5)
	{
		_global = 0;
		_loadedAllArchiveData = false;
		_savingWorld = false;
		_inscribedCharacters = new Dictionary<InscribedCharacterKey, InscribedCharacter>(0);
		_globalFlags = 0uL;
		OnInitializedDomainData();
	}

	private int GetGlobal()
	{
		return _global;
	}

	private void SetGlobal(int value, DataContext context)
	{
		_global = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, DataStates, CacheInfluences, context);
	}

	public bool GetLoadedAllArchiveData()
	{
		return _loadedAllArchiveData;
	}

	private void SetLoadedAllArchiveData(bool value, DataContext context)
	{
		_loadedAllArchiveData = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, DataStates, CacheInfluences, context);
	}

	public bool GetSavingWorld()
	{
		return _savingWorld;
	}

	private void SetSavingWorld(bool value, DataContext context)
	{
		_savingWorld = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(2, DataStates, CacheInfluences, context);
	}

	public InscribedCharacter GetElement_InscribedCharacters(InscribedCharacterKey elementId)
	{
		return _inscribedCharacters[elementId];
	}

	public bool TryGetElement_InscribedCharacters(InscribedCharacterKey elementId, out InscribedCharacter value)
	{
		return _inscribedCharacters.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_InscribedCharacters(InscribedCharacterKey elementId, InscribedCharacter value, DataContext context)
	{
		_inscribedCharacters.Add(elementId, value);
		_modificationsInscribedCharacters.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(0, 3, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(0, 3, elementId, 0);
		}
	}

	private unsafe void SetElement_InscribedCharacters(InscribedCharacterKey elementId, InscribedCharacter value, DataContext context)
	{
		_inscribedCharacters[elementId] = value;
		_modificationsInscribedCharacters.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(0, 3, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(0, 3, elementId, 0);
		}
	}

	private void RemoveElement_InscribedCharacters(InscribedCharacterKey elementId, DataContext context)
	{
		_inscribedCharacters.Remove(elementId);
		_modificationsInscribedCharacters.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(0, 3, elementId);
	}

	private void ClearInscribedCharacters(DataContext context)
	{
		_inscribedCharacters.Clear();
		_modificationsInscribedCharacters.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(0, 3);
	}

	public ulong GetGlobalFlags()
	{
		return _globalFlags;
	}

	public unsafe void SetGlobalFlags(ulong value, DataContext context)
	{
		_globalFlags = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(0, 4, 8);
		*(ulong*)ptr = _globalFlags;
		ptr += 8;
	}

	public override void OnInitializeGameDataModule()
	{
		InitializeOnInitializeGameDataModule();
	}

	public override void OnEnterNewWorld()
	{
		InitializeOnEnterNewWorld();
		InitializeInternalDataOfCollections();
	}

	public override void OnLoadWorld()
	{
		_pendingLoadingOperationIds = new Queue<uint>();
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(0, 3));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(0, 4));
	}

	public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
	{
		switch (dataId)
		{
		case 0:
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		case 1:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 1);
			}
			return GameData.Serializer.Serializer.Serialize(_loadedAllArchiveData, dataPool);
		case 2:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 2);
			}
			return GameData.Serializer.Serializer.Serialize(_savingWorld, dataPool);
		case 3:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 3);
				_modificationsInscribedCharacters.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<InscribedCharacterKey, InscribedCharacter>)_inscribedCharacters, dataPool);
		case 4:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 4);
			}
			return GameData.Serializer.Serializer.Serialize(_globalFlags, dataPool);
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
		case 2:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 3:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 4:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _globalFlags);
			SetGlobalFlags(_globalFlags, context);
			break;
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
			int argsCount6 = operation.ArgsCount;
			int num6 = argsCount6;
			if (num6 == 1)
			{
				sbyte item8 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item8);
				EnterNewWorld(item8);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 1:
		{
			int argsCount = operation.ArgsCount;
			int num = argsCount;
			if (num == 2)
			{
				sbyte item = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item);
				long item2 = 0L;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item2);
				LoadWorld(item, item2);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 2:
		{
			int argsCount8 = operation.ArgsCount;
			int num8 = argsCount8;
			if (num8 == 1)
			{
				sbyte item10 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item10);
				LoadEnding(item10);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 3:
			if (operation.ArgsCount == 0)
			{
				SaveWorld(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 4:
			if (operation.ArgsCount == 0)
			{
				LeaveWorld();
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 5:
			if (operation.ArgsCount == 0)
			{
				uint operationId = GameData.GameDataBridge.GameDataBridge.RecordPassthroughMethod(operation);
				GetArchivesInfo(operationId);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 6:
		{
			int argsCount4 = operation.ArgsCount;
			int num4 = argsCount4;
			if (num4 == 1)
			{
				sbyte item5 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item5);
				DeleteArchive(item5);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 7:
		{
			int argsCount11 = operation.ArgsCount;
			int num11 = argsCount11;
			if (num11 == 1)
			{
				int item14 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item14);
				InscribeCharacter(context, item14);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 8:
		{
			int argsCount9 = operation.ArgsCount;
			int num9 = argsCount9;
			if (num9 == 1)
			{
				InscribedCharacterKey item11 = default(InscribedCharacterKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item11);
				RemoveInscribedCharacter(context, item11);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 9:
		{
			int argsCount5 = operation.ArgsCount;
			int num5 = argsCount5;
			if (num5 == 1)
			{
				string item7 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item7);
				SetGameVersion(item7);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 10:
		{
			int argsCount2 = operation.ArgsCount;
			int num2 = argsCount2;
			if (num2 == 1)
			{
				sbyte item3 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item3);
				SetCompressionLevel(item3);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 11:
			if (operation.ArgsCount == 0)
			{
				PackAllCrossArchiveGameData();
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 12:
		{
			int argsCount10 = operation.ArgsCount;
			int num10 = argsCount10;
			if (num10 == 2)
			{
				sbyte item12 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item12);
				bool item13 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item13);
				SetGlobalFlag(context, item12, item13);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 13:
		{
			int argsCount7 = operation.ArgsCount;
			int num7 = argsCount7;
			if (num7 == 1)
			{
				sbyte item9 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item9);
				bool globalFlag = GetGlobalFlag(item9);
				return GameData.Serializer.Serializer.Serialize(globalFlag, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 14:
			if (operation.ArgsCount == 0)
			{
				bool item6 = CheckDriveSpace(context);
				return GameData.Serializer.Serializer.Serialize(item6, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 15:
		{
			int argsCount3 = operation.ArgsCount;
			int num3 = argsCount3;
			if (num3 == 1)
			{
				SharedGlobalSettings item4 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item4);
				UpdateSharedGlobalSettings(item4);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 16:
			if (operation.ArgsCount == 0)
			{
				ReloadAllConfigData();
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

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
			_modificationsInscribedCharacters.ChangeRecording(monitoring);
			break;
		case 4:
			break;
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
	{
		switch (dataId)
		{
		case 0:
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		case 1:
			if (!BaseGameDataDomain.IsModified(DataStates, 1))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 1);
			return GameData.Serializer.Serializer.Serialize(_loadedAllArchiveData, dataPool);
		case 2:
			if (!BaseGameDataDomain.IsModified(DataStates, 2))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 2);
			return GameData.Serializer.Serializer.Serialize(_savingWorld, dataPool);
		case 3:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 3))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 3);
			int result = GameData.Serializer.Serializer.SerializeModifications(_inscribedCharacters, dataPool, _modificationsInscribedCharacters);
			_modificationsInscribedCharacters.Reset();
			return result;
		}
		case 4:
			if (!BaseGameDataDomain.IsModified(DataStates, 4))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 4);
			return GameData.Serializer.Serializer.Serialize(_globalFlags, dataPool);
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
			if (BaseGameDataDomain.IsModified(DataStates, 1))
			{
				BaseGameDataDomain.ResetModified(DataStates, 1);
			}
			break;
		case 2:
			if (BaseGameDataDomain.IsModified(DataStates, 2))
			{
				BaseGameDataDomain.ResetModified(DataStates, 2);
			}
			break;
		case 3:
			if (BaseGameDataDomain.IsModified(DataStates, 3))
			{
				BaseGameDataDomain.ResetModified(DataStates, 3);
				_modificationsInscribedCharacters.Reset();
			}
			break;
		case 4:
			if (BaseGameDataDomain.IsModified(DataStates, 4))
			{
				BaseGameDataDomain.ResetModified(DataStates, 4);
			}
			break;
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
	{
		return dataId switch
		{
			0 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			1 => BaseGameDataDomain.IsModified(DataStates, 1), 
			2 => BaseGameDataDomain.IsModified(DataStates, 2), 
			3 => BaseGameDataDomain.IsModified(DataStates, 3), 
			4 => BaseGameDataDomain.IsModified(DataStates, 4), 
			_ => throw new Exception($"Unsupported dataId {dataId}"), 
		};
	}

	public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
	{
		switch (influence.TargetIndicator.DataId)
		{
		default:
			throw new Exception($"Unsupported dataId {influence.TargetIndicator.DataId}");
		case 0:
		case 1:
		case 2:
		case 3:
		case 4:
			throw new Exception($"Cannot invalidate cache state of non-cache data {influence.TargetIndicator.DataId}");
		}
	}

	public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
	{
		uint num;
		switch (operation.DataId)
		{
		case 0:
			ProcessGlobalArchiveResponse(operation, pResult);
			goto IL_008f;
		case 3:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _inscribedCharacters);
			goto IL_008f;
		case 4:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _globalFlags);
			goto IL_008f;
		default:
			throw new Exception($"Unsupported dataId {operation.DataId}");
		case 1:
		case 2:
			{
				throw new Exception($"Cannot process archive response of non-archive data {operation.DataId}");
			}
			IL_008f:
			if (_pendingLoadingOperationIds == null)
			{
				break;
			}
			num = _pendingLoadingOperationIds.Peek();
			if (num == operation.Id)
			{
				_pendingLoadingOperationIds.Dequeue();
				if (_pendingLoadingOperationIds.Count <= 0)
				{
					_pendingLoadingOperationIds = null;
					InitializeInternalDataOfCollections();
					OnLoadedArchiveData();
					DomainManager.Global.CompleteLoading(0);
				}
			}
			break;
		}
	}

	private void InitializeInternalDataOfCollections()
	{
	}
}
