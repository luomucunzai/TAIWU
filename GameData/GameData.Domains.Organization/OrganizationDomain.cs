using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Config;
using Config.ConfigCells.Character;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Common.SingleValueCollection;
using GameData.Dependencies;
using GameData.DomainEvents;
using GameData.Domains.Building;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.Creation;
using GameData.Domains.Character.Display;
using GameData.Domains.Character.Filters;
using GameData.Domains.Character.Relation;
using GameData.Domains.Global.Inscription;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization.Display;
using GameData.Domains.Organization.SettlementPrisonRecord;
using GameData.Domains.Organization.SettlementTreasuryRecord;
using GameData.Domains.Taiwu;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.Taiwu.Profession.SkillsData;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.MonthlyEventActions;
using GameData.Domains.World;
using GameData.Domains.World.Notification;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using NLog;
using Redzen.Random;

namespace GameData.Domains.Organization;

[GameDataDomain(3)]
public class OrganizationDomain : BaseGameDataDomain
{
	public enum ESettlementTreasuryOperationResult
	{
		None,
		Exchange,
		Steal,
		Store
	}

	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
	private readonly Dictionary<short, Sect> _sects;

	[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
	private readonly Dictionary<short, CivilianSettlement> _civilianSettlements;

	[DomainData(DomainDataType.SingleValue, true, false, false, false)]
	private short _nextSettlementId;

	[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
	private readonly Dictionary<int, SectCharacter> _sectCharacters;

	[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
	private readonly Dictionary<int, CivilianSettlementCharacter> _civilianSettlementCharacters;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, true)]
	private readonly Dictionary<int, CharacterSet> _factions;

	[DomainData(DomainDataType.SingleValue, true, false, false, false, ArrayElementsCount = 64)]
	private sbyte[] _largeSectFavorabilities;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private List<MartialArtTournamentPreparationInfo> _martialArtTournamentPreparationInfoList;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private List<short> _previousMartialArtTournamentHosts;

	private Dictionary<Location, Settlement> _locationSettlements;

	private Dictionary<short, Settlement> _settlements;

	private List<Settlement>[] _orgTemplateId2Settlements;

	private Dictionary<int, SettlementCharacter> _settlementCharacters;

	public const sbyte MerchantGrade = 4;

	public bool ParallelUpdateOrganizationMembers = true;

	private SettlementCreatingInfo _settlementCreatingInfo;

	private static Dictionary<sbyte, List<InscribedCharacter>> _orgInscribedCharIdMap;

	private static StringBuilder _stringBuilder;

	private static readonly sbyte[] CreateFactionChance = new sbyte[5] { 30, 10, 40, 20, 50 };

	private static readonly sbyte[] ExpandFactionChance = new sbyte[5] { 20, 40, 50, 10, 30 };

	private static readonly sbyte[] JoinFactionChance = new sbyte[5] { 30, 40, 50, 10, 20 };

	private static readonly sbyte[] JoinFactionFavorabilityBonus = new sbyte[5] { 0, 5, 10, 15, 0 };

	private static readonly sbyte[] JoinFactionFavorabilityReq = new sbyte[6] { 1, 1, 2, 1, 2, 3 };

	private static readonly sbyte[][] JoinFactionPriorities = new sbyte[5][]
	{
		new sbyte[6] { 2, 1, 4, 3, 0, 5 },
		new sbyte[6] { 0, 2, 1, 3, 4, 5 },
		new sbyte[6] { 1, 0, 4, 2, 3, 5 },
		new sbyte[6] { 4, 2, 0, 3, 1, 5 },
		new sbyte[6] { 0, 1, 4, 3, 2, 5 }
	};

	private readonly Dictionary<int, List<sbyte>> _sectFugitives = new Dictionary<int, List<sbyte>>();

	private readonly Dictionary<int, sbyte> _sectPrisoners = new Dictionary<int, sbyte>();

	private readonly List<SettlementBounty> _calculatedBountiesCache = new List<SettlementBounty>();

	[Obsolete]
	private int _prisonGuardCharId = -1;

	private static sbyte[] _allSectOrgTemplateIds;

	private static sbyte[] _maleSectOrgTemplateIds;

	private static sbyte[] _femaleSectOrgTemplateIds;

	private SettlementTreasuryLayers _currentTreasuryLayer;

	private int _firstGuardCharId = -1;

	private List<ItemSourceChange> _itemSourceChanges;

	private Inventory _getTreasuryInventory;

	private Inventory _stealTreasuryInventory;

	private Inventory _exchangeTreasuryInventory;

	private Inventory _storeTreasuryInventory;

	private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[9][];

	private static readonly DataInfluence[][] CacheInfluencesSects = new DataInfluence[20][];

	private readonly ObjectCollectionDataStates _dataStatesSects = new ObjectCollectionDataStates(20, 0);

	public readonly ObjectCollectionHelperData HelperDataSects;

	private static readonly DataInfluence[][] CacheInfluencesCivilianSettlements = new DataInfluence[17][];

	private readonly ObjectCollectionDataStates _dataStatesCivilianSettlements = new ObjectCollectionDataStates(17, 0);

	public readonly ObjectCollectionHelperData HelperDataCivilianSettlements;

	private static readonly DataInfluence[][] CacheInfluencesSectCharacters = new DataInfluence[6][];

	private readonly ObjectCollectionDataStates _dataStatesSectCharacters = new ObjectCollectionDataStates(6, 0);

	public readonly ObjectCollectionHelperData HelperDataSectCharacters;

	private static readonly DataInfluence[][] CacheInfluencesCivilianSettlementCharacters = new DataInfluence[6][];

	private readonly ObjectCollectionDataStates _dataStatesCivilianSettlementCharacters = new ObjectCollectionDataStates(6, 0);

	public readonly ObjectCollectionHelperData HelperDataCivilianSettlementCharacters;

	private SingleValueCollectionModificationCollection<int> _modificationsFactions = SingleValueCollectionModificationCollection<int>.Create();

	private SpinLock _spinLockMartialArtTournamentPreparationInfoList = new SpinLock(enableThreadOwnerTracking: false);

	private Queue<uint> _pendingLoadingOperationIds;

	private void OnInitializedDomainData()
	{
		InitializeSettlementTreasury();
	}

	private void InitializeOnInitializeGameDataModule()
	{
		InitializeSectOrgTemplateIds();
	}

	private void InitializeOnEnterNewWorld()
	{
		InitializeSettlementsCache();
		InitializeSettlementCharactersCache();
		_orgInscribedCharIdMap = new Dictionary<sbyte, List<InscribedCharacter>>();
	}

	private void OnLoadedArchiveData()
	{
		InitializeSettlementsCache();
		InitializeSettlementCharactersCache();
		DataUid uid = new DataUid(0, 1, ulong.MaxValue);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(uid, "InitializeSortedMembersCache", InitializeSortedMembersCache);
	}

	public override void OnCurrWorldArchiveDataReady(DataContext context, bool isNewWorld)
	{
		InitializePrisonCache();
	}

	public override void FixAbnormalDomainArchiveData(DataContext context)
	{
		Settlement settlementByOrgTemplateId = GetSettlementByOrgTemplateId(16);
		if (settlementByOrgTemplateId.GetMaxCulture() < 25)
		{
			Logger.Warn($"Fixing taiwu village's max culture {settlementByOrgTemplateId.GetMaxCulture()} => 25.");
			settlementByOrgTemplateId.SetMaxCulture(25, context);
		}
		bool flag = DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 71, 26);
		if (flag)
		{
			Logger.Warn("Initializing settlement treasuries.");
		}
		foreach (Settlement value in _settlements.Values)
		{
			FixAbnormalSettlementMembers(context, value);
			if (flag)
			{
				FixComplementSettlementTreasuryBuilding(context, value);
			}
		}
		bool flag2 = DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 73, 27);
		if (flag2)
		{
			Logger.Warn("Initializing settlement prisons");
		}
		foreach (Sect value2 in _sects.Values)
		{
			if (flag2)
			{
				FixComplementSectPrisonBuilding(context, value2);
			}
			FixInvalidSectPrisoner(context, value2);
			GameData.Domains.Character.Character leader = value2.GetLeader();
			if (leader != null && leader.AddFeature(context, 405))
			{
				Logger.Info($"Adding sect leader feature to {leader}.");
			}
		}
		if (!DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 79, 21))
		{
			return;
		}
		Logger.Warn("Fixing settlement treasury hobbies.");
		foreach (Settlement value3 in _settlements.Values)
		{
			SettlementTreasury[] settlementTreasuries = value3.Treasuries.SettlementTreasuries;
			int num = settlementTreasuries.Length - 1;
			while (num-- > 0)
			{
				settlementTreasuries[num].LovingItemSubTypes = settlementTreasuries[^1].LovingItemSubTypes.ToList();
				settlementTreasuries[num].HatingItemSubTypes = settlementTreasuries[^1].HatingItemSubTypes.ToList();
			}
			DomainManager.Extra.SetTreasuries(context, value3.GetId(), value3.Treasuries, needUpdateTotalValue: false);
		}
	}

	private void FixComplementSettlementTreasuryBuilding(DataContext context, Settlement settlement)
	{
		if (settlement.GetLocation().AreaId < 45)
		{
			Location location = settlement.GetLocation();
			sbyte orgTemplateId = settlement.GetOrgTemplateId();
			if (1 == 0)
			{
			}
			short num;
			switch (orgTemplateId)
			{
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
			case 13:
			case 14:
			case 15:
				num = (short)(orgTemplateId - 1 + 288);
				break;
			case 21:
			case 22:
			case 23:
			case 24:
			case 25:
			case 26:
			case 27:
			case 28:
			case 29:
			case 30:
			case 31:
			case 32:
			case 33:
			case 34:
			case 35:
				num = 284;
				break;
			case 36:
				num = 286;
				break;
			case 37:
				num = 285;
				break;
			case 38:
				num = 287;
				break;
			default:
				num = -1;
				break;
			}
			if (1 == 0)
			{
			}
			short num2 = num;
			if (num2 >= 0)
			{
				DomainManager.Building.PlaceBuildingAtBlock(context, location.AreaId, location.BlockId, num2, forcePlace: true, isRandom: false);
			}
		}
	}

	private void FixComplementSectPrisonBuilding(DataContext context, Sect sect)
	{
		Location location = sect.GetLocation();
		sbyte orgTemplateId = sect.GetOrgTemplateId();
		sbyte largeSectIndex = GetLargeSectIndex(orgTemplateId);
		if (largeSectIndex >= 0)
		{
			short templateId = (short)(largeSectIndex + 303);
			DomainManager.Building.PlaceBuildingAtBlock(context, location.AreaId, location.BlockId, templateId, forcePlace: true, isRandom: false);
		}
	}

	private void FixInvalidSectPrisoner(DataContext context, Sect sect)
	{
		SettlementPrison prison = sect.Prison;
		bool flag = false;
		for (int num = prison.Prisoners.Count - 1; num >= 0; num--)
		{
			SettlementPrisoner settlementPrisoner = prison.Prisoners[num];
			int charId = settlementPrisoner.CharId;
			if (!DomainManager.Character.TryGetElement_Objects(charId, out var element))
			{
				prison.Prisoners.RemoveAt(num);
				flag = true;
				Logger.Warn($"Fixing dead character {charId} in sect {sect}'s prison.");
			}
			else if (element.IsActiveExternalRelationState(8))
			{
				prison.Prisoners.RemoveAt(num);
				DomainManager.Organization.UnregisterSectPrisoner(settlementPrisoner.CharId);
				element.SetLocation(sect.GetLocation(), context);
				element.DeactivateExternalRelationState(context, 32);
				flag = true;
				Logger.Warn($"Fixing infected character {element} exist in both stone room and sect {sect}'s prison.");
			}
		}
		for (int num2 = prison.Bounties.Count - 1; num2 >= 0; num2--)
		{
			SettlementBounty settlementBounty = prison.Bounties[num2];
			if (settlementBounty.PunishmentType < 0)
			{
				PredefinedLog.Show(33, DomainManager.Character.TryGetElement_Objects(settlementBounty.CharId, out var element2) ? element2.ToString() : settlementBounty.CharId.ToString(), PunishmentSeverity.Instance.GetItem(settlementBounty.PunishmentSeverity)?.Name);
				prison.Bounties.RemoveAt(num2);
			}
		}
		if (flag)
		{
			DomainManager.Extra.SetSettlementPrison(context, sect.GetId(), prison);
		}
	}

	private BuildingBlockData GetAvailableBlockInSettlementBuildingArea(Location location)
	{
		BuildingAreaData element_BuildingAreas = DomainManager.Building.GetElement_BuildingAreas(location);
		int num = element_BuildingAreas.Width * element_BuildingAreas.Width;
		(int, int) tuple = (element_BuildingAreas.Width / 2, element_BuildingAreas.Width / 2);
		BuildingBlockData buildingBlockData = null;
		int num2 = int.MinValue;
		int num3 = int.MaxValue;
		for (short num4 = 0; num4 < num; num4++)
		{
			BuildingBlockKey elementId = new BuildingBlockKey(location.AreaId, location.BlockId, num4);
			if (DomainManager.Building.TryGetElement_BuildingBlocks(elementId, out var value) && value.RootBlockIndex < 0)
			{
				BuildingBlockItem configData = value.ConfigData;
				EBuildingBlockType type = configData.Type;
				if ((uint)(type - 3) > 1u)
				{
					(int, int) blockPos = element_BuildingAreas.GetBlockPos(num4);
					EBuildingBlockType type2 = configData.Type;
					if (1 == 0)
					{
					}
					int num5 = type2 switch
					{
						EBuildingBlockType.SpecialResource => 0, 
						EBuildingBlockType.NormalResource => 1, 
						EBuildingBlockType.UselessResource => 2, 
						EBuildingBlockType.Empty => 3, 
						_ => int.MinValue, 
					};
					if (1 == 0)
					{
					}
					int num6 = num5;
					int manhattanDistance = MathUtils.GetManhattanDistance(tuple.Item1, tuple.Item2, blockPos.Item1, blockPos.Item2);
					if (buildingBlockData == null || num2 > num6 || (num2 == num6 && manhattanDistance < num3))
					{
						buildingBlockData = value;
						num2 = num6;
						num3 = manhattanDistance;
					}
				}
			}
		}
		return buildingBlockData;
	}

	private void FixAbnormalSettlementMembers(DataContext context, Settlement settlement)
	{
		short id = settlement.GetId();
		sbyte orgTemplateId = settlement.GetOrgTemplateId();
		OrgMemberCollection members = DomainManager.Organization.GetSettlement(id).GetMembers();
		List<(int, sbyte)> list = new List<(int, sbyte)>();
		for (sbyte b = 0; b < 9; b++)
		{
			HashSet<int> members2 = members.GetMembers(b);
			foreach (int item3 in members2)
			{
				if (DomainManager.Character.TryGetElement_Objects(item3, out var element) && element.GetOrganizationInfo().OrgTemplateId == 20)
				{
					list.Add((item3, b));
				}
			}
		}
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			(int, sbyte) tuple = list[i];
			int item = tuple.Item1;
			sbyte item2 = tuple.Item2;
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			OrganizationInfo organizationInfo = element_Objects.GetOrganizationInfo();
			organizationInfo.OrgTemplateId = orgTemplateId;
			organizationInfo.Grade = item2;
			organizationInfo.SettlementId = id;
			element_Objects.SetOrganizationInfo(organizationInfo, context);
			Events.RaiseXiangshuInfectionFeatureChanged(context, element_Objects, 218);
			string name = Config.Organization.Instance[organizationInfo.OrgTemplateId].Name;
			string gradeName = GetOrgMemberConfig(organizationInfo).GradeName;
			Logger.Warn($"Removing infected character {element_Objects} from his/her original settlement as {name}{gradeName}.");
		}
	}

	[SingleValueDependency(12, new ushort[] { 1 })]
	[SingleValueDependency(3, new ushort[] { 8 })]
	[ObjectCollectionDependency(3, 0, new ushort[] { 18 })]
	private unsafe void CalcMartialArtTournamentPreparationInfoList(List<MartialArtTournamentPreparationInfo> value)
	{
		value.Clear();
		MonthlyActionKey key = new MonthlyActionKey(2, 0);
		MonthlyActionBase monthlyAction = DomainManager.TaiwuEvent.GetMonthlyAction(key);
		if (monthlyAction == null || monthlyAction.State != 1)
		{
			return;
		}
		long* ptr = stackalloc long[_sects.Count];
		long* ptr2 = stackalloc long[_sects.Count];
		long* ptr3 = stackalloc long[_sects.Count];
		int num = 0;
		int num2 = Math.Max(0, _previousMartialArtTournamentHosts.Count - 3);
		foreach (Sect value5 in _sects.Values)
		{
			short id = value5.GetId();
			int num3 = _previousMartialArtTournamentHosts.LastIndexOf(id);
			if (num3 < num2)
			{
				int[] taiwuInvestmentForMartialArtTournament = value5.GetTaiwuInvestmentForMartialArtTournament();
				int[] martialArtTournamentPreparations = value5.GetMartialArtTournamentPreparations();
				int num4 = martialArtTournamentPreparations[0] + taiwuInvestmentForMartialArtTournament[0];
				int num5 = martialArtTournamentPreparations[1] + taiwuInvestmentForMartialArtTournament[1];
				int num6 = martialArtTournamentPreparations[2] + taiwuInvestmentForMartialArtTournament[2];
				ptr[num] = ((long)num4 << 32) + id;
				ptr2[num] = ((long)num5 << 32) + id;
				ptr3[num] = ((long)num6 << 32) + id;
				value.Add(new MartialArtTournamentPreparationInfo
				{
					SettlementId = id,
					CombatPowerPreparation = num4,
					AuthorityPreparation = num5,
					ResourcePreparation = num6,
					TotalScore = 0
				});
				num++;
			}
		}
		CollectionUtils.Sort(ptr, num);
		CollectionUtils.Sort(ptr2, num);
		CollectionUtils.Sort(ptr3, num);
		stackalloc long[num].Fill(0L);
		for (int i = 0; i < num; i++)
		{
			int num7 = num - i - 1;
			short settlementId = (short)(ptr[num7] & 0xFFFF);
			int num8 = GetScore(ptr, num, num7);
			int index = value.FindIndex((MartialArtTournamentPreparationInfo info) => info.SettlementId == settlementId);
			MartialArtTournamentPreparationInfo value2 = value[index];
			value2.TotalScore += num8;
			value[index] = value2;
			short settlementId2 = (short)(ptr2[num7] & 0xFFFF);
			int num9 = GetScore(ptr2, num, num7);
			int index2 = value.FindIndex((MartialArtTournamentPreparationInfo info) => info.SettlementId == settlementId2);
			MartialArtTournamentPreparationInfo value3 = value[index2];
			value3.TotalScore += num9;
			value[index2] = value3;
			short settlementId3 = (short)(ptr3[num7] & 0xFFFF);
			int num10 = GetScore(ptr3, num, num7);
			int index3 = value.FindIndex((MartialArtTournamentPreparationInfo info) => info.SettlementId == settlementId3);
			MartialArtTournamentPreparationInfo value4 = value[index3];
			value4.TotalScore += num10;
			value[index3] = value4;
		}
		value.Sort();
		unsafe static int GetScore(long* rank, int length, int rankIndex)
		{
			int num11 = rankIndex;
			long num12 = rank[rankIndex] >> 32;
			for (int j = rankIndex + 1; j < length; j++)
			{
				long num13 = rank[j] >> 32;
				if (num13 != num12)
				{
					break;
				}
				num11 = j;
			}
			return 15 - (length - num11 - 1);
		}
	}

	public void MakeNoneOrgCharactersBecomeBeggar(DataContext context)
	{
		List<GameData.Domains.Character.Character> list = new List<GameData.Domains.Character.Character>();
		MapCharacterFilter.ParallelFind((GameData.Domains.Character.Character character) => character.GetOrganizationInfo().OrgTemplateId == 0 && character.IsInteractableAsIntelligentCharacter() && DomainManager.Organization.GetFugitiveBountySect(character.GetId()) < 0 && DomainManager.Organization.GetPrisonerSect(character.GetId()) < 0, list, 0, 135);
		foreach (GameData.Domains.Character.Character item in list)
		{
			JoinNearbyVillageTownAsBeggar(context, item, -1);
		}
	}

	public void UpdateApprovingRateEffectOnAdvanceMonth(DataContext context)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		foreach (KeyValuePair<short, Sect> sect2 in _sects)
		{
			sect2.Deconstruct(out var key, out var value);
			short num4 = key;
			Sect sect = value;
			short num5 = sect.CalcApprovingRate();
			if (num5 >= 200)
			{
				switch (Config.Organization.Instance[sect.GetOrgTemplateId()].Goodness)
				{
				case -1:
					num2 += num5;
					break;
				case 0:
					num3 += num5;
					break;
				case 1:
					num += num5;
					break;
				}
			}
		}
		if (num2 >= 100)
		{
			taiwu.RecordFameAction(context, 72, -1, (short)(num2 / 100));
		}
		if (num >= 100)
		{
			taiwu.RecordFameAction(context, 71, -1, (short)(num / 100));
		}
		if (num3 >= 100)
		{
			taiwu.RecordFameAction(context, 73, -1, (short)(num3 / 100));
			taiwu.RecordFameAction(context, 74, -1, (short)(num3 / 100));
		}
		taiwu.ChangeResource(context, 7, CalcApprovingRateEffectAuthorityGain());
	}

	[DomainMethod]
	public int CalcApprovingRateEffectAuthorityGain()
	{
		int num = 0;
		foreach (KeyValuePair<short, Sect> sect2 in _sects)
		{
			sect2.Deconstruct(out var key, out var value);
			short num2 = key;
			Sect sect = value;
			short num3 = sect.CalcApprovingRate();
			if (num3 >= 300)
			{
				num += num3;
			}
		}
		return num / 10;
	}

	public Settlement GetSettlement(short settlementId)
	{
		return _settlements[settlementId];
	}

	public Settlement GetSettlementByOrgTemplateId(sbyte orgTemplateId)
	{
		List<Settlement> list = _orgTemplateId2Settlements[orgTemplateId];
		bool flag = list == null;
		bool flag2 = flag;
		if (!flag2)
		{
			int count = list.Count;
			bool flag3 = ((count > 1 || count == 0) ? true : false);
			flag2 = flag3;
		}
		if (flag2)
		{
			return null;
		}
		return list[0];
	}

	public Settlement GetSettlementByLocation(Location location)
	{
		if (_locationSettlements.TryGetValue(location, out var value))
		{
			return value;
		}
		return null;
	}

	public void GetAllSettlements(List<Settlement> settlements)
	{
		settlements.Clear();
		settlements.AddRange(_sects.Values);
		settlements.AddRange(_civilianSettlements.Values);
	}

	public void GetAllCivilianSettlements(List<Settlement> settlements)
	{
		settlements.Clear();
		settlements.AddRange(_civilianSettlements.Values);
	}

	public short GetSettlementIdByOrgTemplateId(sbyte orgTemplateId)
	{
		return GetSettlementByOrgTemplateId(orgTemplateId)?.GetId() ?? (-1);
	}

	public SettlementCharacter GetSettlementCharacter(int charId)
	{
		return _settlementCharacters[charId];
	}

	public bool TryGetSettlementCharacter(int charId, out SettlementCharacter settlementChar)
	{
		return _settlementCharacters.TryGetValue(charId, out settlementChar);
	}

	public bool IsInAnySect(int charId)
	{
		return _sectCharacters.ContainsKey(charId);
	}

	public bool IsInAnyCivilianSettlement(int charId)
	{
		return _civilianSettlementCharacters.ContainsKey(charId);
	}

	public void JoinSect(DataContext context, GameData.Domains.Character.Character character, OrganizationInfo destOrgInfo)
	{
		LeaveOrganization(context, character, charIsDead: false);
		JoinOrganization(context, character, destOrgInfo);
		OrganizationInfo organizationInfo = character.GetOrganizationInfo();
		character.SetOrganizationInfo(destOrgInfo, context);
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		int id = character.GetId();
		Location location = character.GetLocation();
		sbyte gender = character.GetGender();
		lifeRecordCollection.AddJoinSectSucceed(id, currDate, location, destOrgInfo.SettlementId, destOrgInfo.OrgTemplateId, destOrgInfo.Grade, orgPrincipal: true, gender);
		Events.RaiseCharacterOrganizationChanged(context, character, organizationInfo, destOrgInfo);
	}

	public void JoinOrganization(DataContext context, GameData.Domains.Character.Character character, OrganizationInfo destOrgInfo)
	{
		if (destOrgInfo.SettlementId < 0)
		{
			return;
		}
		int id = character.GetId();
		SettlementCharacter value;
		OrgMemberCollection members;
		if (IsSect(destOrgInfo.OrgTemplateId))
		{
			SectCharacter sectCharacter = new SectCharacter(id, destOrgInfo.OrgTemplateId, destOrgInfo.SettlementId);
			AddElement_SectCharacters(id, sectCharacter);
			value = sectCharacter;
			Sect sect = _sects[destOrgInfo.SettlementId];
			members = sect.GetMembers();
			CreateRelationWithAllSettlementMembers(context, character, members);
			members.Add(id, destOrgInfo.Grade);
			sect.SetMembers(members, context);
			TryAddSectMemberFeature(context, character, destOrgInfo);
			if (destOrgInfo.Grade == 8 && destOrgInfo.Principal)
			{
				character.AddFeature(context, 405);
			}
			OrganizationMemberItem orgMemberConfig = GetOrgMemberConfig(destOrgInfo);
			short mentorSeniorityId = SetRandomSectMentor(context, id, destOrgInfo, members, orgMemberConfig.TeacherGrade);
			TryBecomeSectMonk(context, character, sect, orgMemberConfig, mentorSeniorityId);
		}
		else
		{
			CivilianSettlementCharacter civilianSettlementCharacter = new CivilianSettlementCharacter(id, destOrgInfo.OrgTemplateId, destOrgInfo.SettlementId);
			AddElement_CivilianSettlementCharacters(id, civilianSettlementCharacter);
			value = civilianSettlementCharacter;
			CivilianSettlement civilianSettlement = _civilianSettlements[destOrgInfo.SettlementId];
			members = civilianSettlement.GetMembers();
			CreateRelationWithAllSettlementMembers(context, character, members);
			members.Add(id, destOrgInfo.Grade);
			civilianSettlement.SetMembers(members, context);
			if (destOrgInfo.OrgTemplateId == 16)
			{
				if (!DomainManager.Taiwu.IsInGroup(id))
				{
					DomainManager.Taiwu.AddTaiwuVillageResident(context, id);
				}
				if (DomainManager.Extra.TryTriggerAddSeniorityPoint(context, 65, id))
				{
					int arg = character.GetMaxMainAttributes().GetSum() + character.GetCombatSkillQualifications().GetSum() + character.GetLifeSkillQualifications().GetSum();
					int baseDelta = ProfessionFormulaImpl.Calculate(66, arg);
					DomainManager.Extra.ChangeProfessionSeniority(context, 10, baseDelta);
				}
			}
		}
		_settlementCharacters.Add(id, value);
		if (destOrgInfo.Principal)
		{
			CheckPrincipalMembersAmount(destOrgInfo.OrgTemplateId, destOrgInfo.Grade, members);
		}
		character.ChangeMerchantType(context, destOrgInfo);
	}

	public void LeaveOrganization(DataContext context, GameData.Domains.Character.Character character, bool charIsDead)
	{
		OrganizationInfo organizationInfo = character.GetOrganizationInfo();
		if (organizationInfo.SettlementId < 0)
		{
			return;
		}
		int id = character.GetId();
		OrganizationItem organizationItem = Config.Organization.Instance[organizationInfo.OrgTemplateId];
		if (IsSect(organizationInfo.OrgTemplateId))
		{
			Sect sect = _sects[organizationInfo.SettlementId];
			if (organizationItem.Hereditary && organizationInfo.Principal && organizationInfo.Grade > 0)
			{
				OrgMemberCollection lackingCoreMembers = sect.GetLackingCoreMembers();
				lackingCoreMembers.Add(id, organizationInfo.Grade);
				sect.SetLackingCoreMembers(lackingCoreMembers, context);
			}
			OrgMemberCollection members = sect.GetMembers();
			members.Remove(id, organizationInfo.Grade);
			sect.SetMembers(members, context);
			RemoveElement_SectCharacters(id);
			if (!charIsDead && organizationInfo.Grade == 8 && organizationInfo.Principal)
			{
				character.RemoveFeature(context, 405);
			}
			int factionId = character.GetFactionId();
			if (factionId == id)
			{
				RemoveFaction(context, character, charIsDead);
			}
			else if (factionId >= 0)
			{
				LeaveFaction(context, character, charIsDead);
			}
			if (!charIsDead)
			{
				TrySecularize(context, character);
			}
			SettlementLayeredTreasuries treasuries = sect.Treasuries;
			if (treasuries.TryRemoveGuard(id, out var _))
			{
				if (!charIsDead)
				{
					character.RemoveFeatureGroup(context, 536);
				}
				Logger.Info($"{character} is no longer guarding sect {sect.GetNameRelatedData().GetName()}");
				DomainManager.Extra.SetTreasuries(context, sect.GetId(), treasuries, needUpdateTotalValue: false);
			}
		}
		else
		{
			CivilianSettlement civilianSettlement = _civilianSettlements[organizationInfo.SettlementId];
			if (organizationItem.Hereditary && organizationInfo.Principal && organizationInfo.Grade > 0)
			{
				OrgMemberCollection lackingCoreMembers2 = civilianSettlement.GetLackingCoreMembers();
				lackingCoreMembers2.Add(id, organizationInfo.Grade);
				civilianSettlement.SetLackingCoreMembers(lackingCoreMembers2, context);
			}
			OrgMemberCollection members2 = civilianSettlement.GetMembers();
			members2.Remove(id, organizationInfo.Grade);
			civilianSettlement.SetMembers(members2, context);
			RemoveElement_CivilianSettlementCharacters(id);
			if (organizationInfo.OrgTemplateId == 16)
			{
				if (charIsDead)
				{
					if (DomainManager.Extra.GetVillagerRole(id) != null)
					{
						DomainManager.World.GetMonthlyNotificationCollection().AddTaiwuVillagerDied(id, organizationInfo.OrgTemplateId, organizationInfo.Grade, organizationInfo.Principal, character.GetGender(), character.GetLocation());
					}
				}
				else
				{
					character.RemoveFeatureGroup(context, 734);
				}
				DomainManager.Taiwu.TryRemoveTaiwuVillageResident(context, id);
				DomainManager.Extra.UnregisterVillagerRole(context, id);
			}
		}
		_settlementCharacters.Remove(id);
		TryDowngradeDeputySpouses(context, id, organizationInfo);
	}

	public void ChangeOrganization(DataContext context, GameData.Domains.Character.Character character, OrganizationInfo destOrgInfo)
	{
		LeaveOrganization(context, character, charIsDead: false);
		JoinOrganization(context, character, destOrgInfo);
		OrganizationInfo organizationInfo = character.GetOrganizationInfo();
		character.SetOrganizationInfo(destOrgInfo, context);
		if (organizationInfo.OrgTemplateId != 20 && destOrgInfo.OrgTemplateId != 20 && organizationInfo.OrgTemplateId != destOrgInfo.OrgTemplateId)
		{
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int id = character.GetId();
			int currDate = DomainManager.World.GetCurrDate();
			sbyte gender = character.GetGender();
			if (organizationInfo.OrgTemplateId == 0)
			{
				lifeRecordCollection.AddJoinOrganization(id, currDate, destOrgInfo.SettlementId, destOrgInfo.OrgTemplateId, destOrgInfo.Grade, destOrgInfo.Principal, gender);
			}
			else if (destOrgInfo.OrgTemplateId == 0)
			{
				lifeRecordCollection.AddBreakAwayOrganization(id, currDate, organizationInfo.SettlementId);
			}
			else
			{
				lifeRecordCollection.AddChangeOrganization(id, currDate, organizationInfo.SettlementId, destOrgInfo.SettlementId, destOrgInfo.OrgTemplateId, destOrgInfo.Grade, destOrgInfo.Principal, gender);
			}
		}
		Events.RaiseCharacterOrganizationChanged(context, character, organizationInfo, destOrgInfo);
	}

	public void ChangeGrade(DataContext context, GameData.Domains.Character.Character character, sbyte destGrade, bool destPrincipal)
	{
		int id = character.GetId();
		OrganizationInfo organizationInfo = character.GetOrganizationInfo();
		OrganizationInfo organizationInfo2 = new OrganizationInfo(organizationInfo.OrgTemplateId, destGrade, destPrincipal, organizationInfo.SettlementId);
		character.SetOrganizationInfo(organizationInfo2, context);
		if (destGrade > organizationInfo.Grade || (destGrade == organizationInfo.Grade && destPrincipal && !organizationInfo.Principal))
		{
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			Location location = character.GetLocation();
			int currDate = DomainManager.World.GetCurrDate();
			sbyte gender = character.GetGender();
			lifeRecordCollection.AddChangeGrade(id, currDate, location, organizationInfo2.OrgTemplateId, destGrade, destPrincipal, gender);
		}
		else if (destGrade < organizationInfo.Grade || (destGrade == organizationInfo.Grade && destPrincipal && !organizationInfo.Principal))
		{
			LifeRecordCollection lifeRecordCollection2 = DomainManager.LifeRecord.GetLifeRecordCollection();
			Location location2 = character.GetLocation();
			int currDate2 = DomainManager.World.GetCurrDate();
			sbyte gender2 = character.GetGender();
			lifeRecordCollection2.AddChangeGradeDrop(id, currDate2, location2, organizationInfo2.OrgTemplateId, destGrade, destPrincipal, gender2);
		}
		if (organizationInfo.SettlementId < 0)
		{
			return;
		}
		Settlement settlement = _settlements[organizationInfo.SettlementId];
		OrganizationItem organizationItem = Config.Organization.Instance[organizationInfo.OrgTemplateId];
		if (organizationItem.Hereditary && organizationInfo.Principal && organizationInfo.Grade > 0)
		{
			OrgMemberCollection lackingCoreMembers = settlement.GetLackingCoreMembers();
			lackingCoreMembers.Add(id, organizationInfo.Grade);
			settlement.SetLackingCoreMembers(lackingCoreMembers, context);
		}
		OrgMemberCollection members = settlement.GetMembers();
		members.OnChangeGrade(id, organizationInfo.Grade, destGrade);
		settlement.SetMembers(members, context);
		if (organizationItem.IsSect)
		{
			TryAddSectMemberFeature(context, character, organizationInfo2);
			if (destGrade == 8 && destPrincipal)
			{
				character.AddFeature(context, 405);
			}
			else if (organizationInfo.Grade == 8 && organizationInfo.Principal)
			{
				character.RemoveFeature(context, 405);
			}
		}
		settlement.RemoveSettlementFeatures(context, character);
		settlement.AddSettlementFeatures(context, character);
		int factionId = character.GetFactionId();
		if (factionId == id)
		{
			RemoveFaction(context, character, leaderIsDead: false);
		}
		else if (factionId >= 0)
		{
			LeaveFaction(context, character, charIsDead: false);
		}
		if (destPrincipal)
		{
			CheckPrincipalMembersAmount(organizationInfo.OrgTemplateId, destGrade, members);
		}
		if (destGrade > organizationInfo.Grade && settlement is Sect)
		{
			OrganizationMemberItem orgMemberConfig = GetOrgMemberConfig(organizationInfo2);
			SetRandomSectMentor(context, id, organizationInfo2, members, orgMemberConfig.TeacherGrade);
		}
		character.ChangeMerchantType(context, organizationInfo2);
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(8);
		AristocratSkillsData skillsData = professionData.GetSkillsData<AristocratSkillsData>();
		if (skillsData.IsCharacterRecommended(id) && destPrincipal)
		{
			short influencePower = DomainManager.Organization.GetSettlementCharacter(id).GetInfluencePower();
			if (destGrade > organizationInfo.Grade)
			{
				int arg = destGrade - organizationInfo.Grade;
				ProfessionFormulaItem formulaCfg = ProfessionFormula.Instance[56];
				int baseDelta = formulaCfg.Calculate(influencePower, arg);
				DomainManager.Extra.ChangeProfessionSeniority(context, 8, baseDelta);
			}
			if (destGrade == 8 && organizationItem.IsSect)
			{
				ProfessionFormulaItem formulaCfg2 = ProfessionFormula.Instance[57];
				int baseDelta2 = formulaCfg2.Calculate(influencePower);
				DomainManager.Extra.ChangeProfessionSeniority(context, 8, baseDelta2);
			}
		}
		if (organizationItem.TemplateId == 16)
		{
			DomainManager.Taiwu.OnTaiwuVillagerGradeChanged(context, character, destGrade);
		}
	}

	public void JoinNearbyVillageTownAsBeggar(DataContext context, GameData.Domains.Character.Character character, short settlementId = -1)
	{
		if (settlementId < 0)
		{
			Location location = character.GetLocation();
			if (!location.IsValid())
			{
				location = character.GetValidLocation();
			}
			if (location.AreaId == 138)
			{
				MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(location.AreaId);
				settlementId = element_Areas.SettlementInfos[0].SettlementId;
			}
			else if (location.AreaId < 135)
			{
				sbyte stateIdByAreaId = DomainManager.Map.GetStateIdByAreaId(location.AreaId);
				List<short> list = ObjectPool<List<short>>.Instance.Get();
				DomainManager.Map.GetStateSettlementIds(stateIdByAreaId, list);
				list.Remove(character.GetOrganizationInfo().SettlementId);
				settlementId = (short)((list.Count <= 0) ? (-1) : list.GetRandom(context.Random));
				ObjectPool<List<short>>.Instance.Return(list);
			}
			else
			{
				settlementId = -1;
			}
		}
		if (settlementId >= 0)
		{
			Settlement settlement = GetSettlement(settlementId);
			OrganizationInfo destOrgInfo = new OrganizationInfo(settlement.GetOrgTemplateId(), 0, principal: true, settlementId);
			ChangeOrganization(context, character, destOrgInfo);
		}
		else
		{
			ChangeOrganization(context, character, OrganizationInfo.None);
		}
	}

	public void UpdateOrganizationAfterMarriage(DataContext context, GameData.Domains.Character.Character selfChar, GameData.Domains.Character.Character targetChar)
	{
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		if (selfChar.GetId() != taiwuCharId && targetChar.GetId() != taiwuCharId)
		{
			OrganizationInfo organizationInfo = selfChar.GetOrganizationInfo();
			bool isSect = Config.Organization.Instance[organizationInfo.OrgTemplateId].IsSect;
			sbyte grade = organizationInfo.Grade;
			int resource = selfChar.GetResource(7);
			OrganizationInfo organizationInfo2 = targetChar.GetOrganizationInfo();
			bool isSect2 = Config.Organization.Instance[organizationInfo2.OrgTemplateId].IsSect;
			sbyte grade2 = organizationInfo2.Grade;
			int resource2 = targetChar.GetResource(7);
			if (isSect && !isSect2)
			{
				DomainManager.Organization.JoinSpouseOrganization(context, targetChar, selfChar);
			}
			else if (!isSect && isSect2)
			{
				DomainManager.Organization.JoinSpouseOrganization(context, selfChar, targetChar);
			}
			else if (grade > grade2)
			{
				DomainManager.Organization.JoinSpouseOrganization(context, targetChar, selfChar);
			}
			else if (grade < grade2)
			{
				DomainManager.Organization.JoinSpouseOrganization(context, selfChar, targetChar);
			}
			else if (resource > resource2)
			{
				DomainManager.Organization.JoinSpouseOrganization(context, targetChar, selfChar);
			}
			else if (resource < resource2)
			{
				DomainManager.Organization.JoinSpouseOrganization(context, selfChar, targetChar);
			}
			else if (selfChar.GetId() < targetChar.GetId())
			{
				DomainManager.Organization.JoinSpouseOrganization(context, targetChar, selfChar);
			}
			else
			{
				DomainManager.Organization.JoinSpouseOrganization(context, selfChar, targetChar);
			}
		}
	}

	public void JoinSpouseOrganization(DataContext context, GameData.Domains.Character.Character selfChar, GameData.Domains.Character.Character spouseChar)
	{
		OrganizationInfo organizationInfo = selfChar.GetOrganizationInfo();
		OrganizationInfo organizationInfo2 = spouseChar.GetOrganizationInfo();
		OrganizationMemberItem orgMemberConfig = GetOrgMemberConfig(organizationInfo2);
		Tester.Assert(organizationInfo2.Principal && orgMemberConfig.ChildGrade >= 0, $"{selfChar} x {spouseChar} Marriage: {organizationInfo2.Principal} && {orgMemberConfig.ChildGrade} >= 0");
		if (organizationInfo.OrgTemplateId == organizationInfo2.OrgTemplateId && organizationInfo.SettlementId == organizationInfo2.SettlementId)
		{
			UpdateGradeAccordingToSpouse(context, selfChar, spouseChar);
		}
		else if (organizationInfo2.OrgTemplateId == 16)
		{
			OrganizationInfo destOrgInfo = new OrganizationInfo(organizationInfo2.OrgTemplateId, 0, principal: true, organizationInfo2.SettlementId);
			DomainManager.Organization.ChangeOrganization(context, selfChar, destOrgInfo);
		}
		else
		{
			OrganizationInfo destOrgInfo2 = new OrganizationInfo(organizationInfo2.OrgTemplateId, (sbyte)((!orgMemberConfig.RestrictPrincipalAmount || orgMemberConfig.DeputySpouseDowngrade >= 0) ? organizationInfo2.Grade : 0), orgMemberConfig.DeputySpouseDowngrade < 0, organizationInfo2.SettlementId);
			DomainManager.Organization.ChangeOrganization(context, selfChar, destOrgInfo2);
		}
	}

	public void UpdateGradeAccordingToSpouse(DataContext context, GameData.Domains.Character.Character selfChar, GameData.Domains.Character.Character spouseChar)
	{
		OrganizationInfo organizationInfo = selfChar.GetOrganizationInfo();
		OrganizationInfo organizationInfo2 = spouseChar.GetOrganizationInfo();
		if (organizationInfo.OrgTemplateId != organizationInfo2.OrgTemplateId || organizationInfo.SettlementId != organizationInfo2.SettlementId || (organizationInfo.Grade >= organizationInfo2.Grade && organizationInfo.Principal) || organizationInfo.OrgTemplateId == 16)
		{
			return;
		}
		OrganizationMemberItem orgMemberConfig = GetOrgMemberConfig(organizationInfo2);
		Tester.Assert(organizationInfo2.Principal);
		if (orgMemberConfig.DeputySpouseDowngrade < 0)
		{
			if (!organizationInfo.Principal && !orgMemberConfig.RestrictPrincipalAmount)
			{
				ChangeGrade(context, selfChar, organizationInfo2.Grade, destPrincipal: true);
			}
		}
		else
		{
			ChangeGrade(context, selfChar, organizationInfo2.Grade, destPrincipal: false);
		}
	}

	private void UpdateAllMentorsAndMenteesInSect(DataContext context, Sect sect)
	{
		OrgMemberCollection members = sect.GetMembers();
		for (sbyte b = 0; b < 8; b++)
		{
			HashSet<int> members2 = members.GetMembers(b);
			foreach (int item in members2)
			{
				if (DomainManager.Character.TryGetElement_Objects(item, out var element))
				{
					OrganizationInfo organizationInfo = element.GetOrganizationInfo();
					OrganizationMemberItem orgMemberConfig = GetOrgMemberConfig(organizationInfo);
					DomainManager.Organization.SetRandomSectMentor(context, item, organizationInfo, members, orgMemberConfig.TeacherGrade);
				}
			}
		}
	}

	public void GetCharactersFromSettlement(short settlementId, sbyte minGrade, sbyte maxGrade, List<GameData.Domains.Character.Character> result)
	{
		GetCharactersFromSettlementWithInfantFilter(settlementId, minGrade, maxGrade, result, includeInfant: true);
	}

	public void GetCharactersFromSettlementWithInfantFilter(short settlementId, sbyte minGrade, sbyte maxGrade, List<GameData.Domains.Character.Character> result, bool includeInfant = false)
	{
		result.Clear();
		Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
		OrgMemberCollection members = settlement.GetMembers();
		for (sbyte b = minGrade; b <= maxGrade; b++)
		{
			IEnumerable<GameData.Domains.Character.Character> enumerable = from memberId in members.GetMembers(b)
				select DomainManager.Character.GetElement_Objects(memberId);
			if (!includeInfant)
			{
				enumerable = DomainManager.Character.ExcludeInfant(enumerable);
			}
			result.AddRange(enumerable);
		}
	}

	public void OnCharacterDead(DataContext context, GameData.Domains.Character.Character character)
	{
		LeaveOrganization(context, character, charIsDead: true);
		int id = character.GetId();
		sbyte fugitiveBountySect = GetFugitiveBountySect(id);
		if (fugitiveBountySect >= 0)
		{
			Sect sect = (Sect)GetSettlementByOrgTemplateId(fugitiveBountySect);
			sect.RemoveBounty(context, id);
		}
		sbyte prisonerSect = GetPrisonerSect(id);
		if (prisonerSect >= 0)
		{
			Sect sect2 = (Sect)GetSettlementByOrgTemplateId(prisonerSect);
			sect2.RemovePrisoner(context, id);
		}
		DomainManager.Building.TryRemoveFeastCustomer(context, id);
	}

	public void OnSectMemberCrimeMadePublic(DataContext context, GameData.Domains.Character.Character character, OrganizationInfo orgInfoOnCommit, sbyte punishmentSeverity, short punishmentType)
	{
		if (punishmentSeverity < 0 || punishmentType < 0 || orgInfoOnCommit.SettlementId != character.GetOrganizationInfo().SettlementId || character.IsCompletelyInfected())
		{
			return;
		}
		if (!DomainManager.Organization.TryGetElement_Sects(orgInfoOnCommit.SettlementId, out var element))
		{
			if (orgInfoOnCommit.SettlementId < 0)
			{
				return;
			}
			Location location = DomainManager.Organization.GetSettlement(orgInfoOnCommit.SettlementId).GetLocation();
			if (!location.IsValid())
			{
				return;
			}
			sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(location.AreaId);
			MapStateItem mapStateItem = MapState.Instance[stateTemplateIdByAreaId];
			if (mapStateItem.SectID < 0)
			{
				return;
			}
			element = (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(mapStateItem.SectID);
		}
		PunishmentSeverityItem punishmentSeverityItem = PunishmentSeverity.Instance[punishmentSeverity];
		sbyte behaviorType = character.GetBehaviorType();
		sbyte percentProb = punishmentSeverityItem.EscapePunishmentChance[behaviorType];
		if (punishmentType == 40)
		{
			percentProb = 100;
		}
		if (character.IsActiveExternalRelationState(32))
		{
			DomainManager.Organization.PunishSectMember(context, element, character, punishmentSeverity, punishmentType, isArrested: true);
			return;
		}
		if (!context.Random.CheckPercentProb(percentProb) && character.IsInteractableAsIntelligentCharacter())
		{
			DomainManager.Character.LeaveGroup(context, character);
			DomainManager.Character.GroupMove(context, character, element.GetLocation());
			DomainManager.Organization.PunishSectMember(context, element, character, punishmentSeverity, punishmentType);
			return;
		}
		OrganizationInfo organizationInfo = character.GetOrganizationInfo();
		if (organizationInfo.OrgTemplateId == orgInfoOnCommit.OrgTemplateId)
		{
			DomainManager.Organization.ChangeOrganization(context, character, new OrganizationInfo(0, organizationInfo.Grade, principal: true, -1));
		}
		element.AddBounty(context, character, punishmentSeverity, punishmentType);
	}

	public void PunishSectMember(DataContext context, Sect sect, GameData.Domains.Character.Character character, sbyte punishmentSeverity = -1, short punishmentType = -1, bool isArrested = false)
	{
		int id = character.GetId();
		OrganizationInfo organizationInfo = character.GetOrganizationInfo();
		sbyte orgTemplateId = sect.GetOrgTemplateId();
		short id2 = sect.GetId();
		PunishmentTypeItem punishmentTypeItem = PunishmentType.Instance[punishmentType];
		if (punishmentTypeItem == null)
		{
			PredefinedLog.Show(33, character, PunishmentSeverity.Instance.GetItem(punishmentSeverity)?.Name);
			return;
		}
		if (punishmentSeverity < 0)
		{
			punishmentSeverity = sect.GetPunishmentTypeSeverity(punishmentTypeItem, includeDefault: true);
		}
		PunishmentSeverityItem punishmentSeverityItem = PunishmentSeverity.Instance[punishmentSeverity];
		if (punishmentSeverityItem.ResourceConfiscation > 0)
		{
			ResourceInts resources = character.GetResources();
			if (punishmentSeverityItem.ResourceConfiscation == 1)
			{
				for (sbyte b = 0; b < 8; b++)
				{
					resources[b] /= 2;
				}
			}
			sect.ConfiscateResources(context, character, ref resources);
		}
		if (punishmentSeverityItem.ItemConfiscation > 0)
		{
			List<ItemKey> list = ObjectPool<List<ItemKey>>.Instance.Get();
			character.GetItemsToLose(list, 0, 8);
			list.Sort(ItemTemplateHelper.ItemGradeComparer);
			if (punishmentSeverityItem.ItemConfiscation == 1)
			{
				int num = list.Count / 2;
				list.RemoveRange(num, list.Count - num);
			}
			sect.ConfiscateItem(context, character, list);
			ObjectPool<List<ItemKey>>.Instance.Return(list);
		}
		if (punishmentSeverityItem.CombatSkillRevoke > 0)
		{
			List<short> list2 = ObjectPool<List<short>>.Instance.Get();
			character.GetLearnedCombatSkillsFromSect(list2, orgTemplateId, 0, 8);
			int num2 = list2.Count;
			if (punishmentSeverityItem.CombatSkillRevoke == 1)
			{
				num2 /= 2;
				list2.Sort(CombatSkillHelper.CombatSkillGradeComparer);
			}
			for (int i = list2.Count - num2; i < list2.Count; i++)
			{
				short skillTemplateId = list2[i];
				DomainManager.Character.RevokeCombatSkill(context, character, skillTemplateId);
			}
			ObjectPool<List<short>>.Instance.Return(list2);
		}
		if (character.GetId() == DomainManager.Taiwu.GetTaiwuCharId())
		{
			return;
		}
		if (punishmentSeverityItem.PrisonTime > 0)
		{
			sect.AddPrisoner(context, character, punishmentSeverity, punishmentType);
			SettlementPrisonRecordCollection settlementPrisonRecordCollection = DomainManager.Organization.GetSettlementPrisonRecordCollection(context, id2);
			int currDate = DomainManager.World.GetCurrDate();
			if (isArrested)
			{
				settlementPrisonRecordCollection.AddImprisonedByArrested(currDate, id2, id, punishmentType);
			}
			else
			{
				settlementPrisonRecordCollection.AddImprisonedVoluntarily(currDate, id2, id, punishmentType);
			}
		}
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		if (punishmentSeverityItem.Expel)
		{
			int num3 = DomainManager.Character.GetAliveSpouse(id);
			GameData.Domains.Character.Character character2 = ((num3 >= 0) ? DomainManager.Character.GetElement_Objects(num3) : null);
			OrganizationInfo organizationInfo2 = default(OrganizationInfo);
			if (character2 != null)
			{
				organizationInfo2 = character2.GetOrganizationInfo();
				if (organizationInfo2.OrgTemplateId != organizationInfo.OrgTemplateId || (organizationInfo2.Principal && organizationInfo.Principal))
				{
					character2 = null;
					num3 = -1;
				}
			}
			lifeRecordCollection.AddSectPunishmentRecord(punishmentTypeItem, punishmentSeverityItem, id2, isArrested, character, num3);
			if (character2 != null)
			{
				if (organizationInfo2.Principal)
				{
					GameData.Domains.Character.Character.ApplySeverHusbandOrWife(context, character, character2, character.GetBehaviorType(), selfIsTaiwuPeople: false, targetIsTaiwuPeople: false);
				}
				else
				{
					PunishSectMember(context, sect, character2, punishmentSeverity, 20);
				}
			}
			return;
		}
		lifeRecordCollection.AddSectPunishmentRecord(punishmentTypeItem, punishmentSeverityItem, id2, isArrested, character, -1);
		if (!punishmentSeverityItem.Expel && organizationInfo.OrgTemplateId == 0)
		{
			sbyte stateIdByAreaId = DomainManager.Map.GetStateIdByAreaId(sect.GetLocation().AreaId);
			short num4 = DomainManager.Map.GetRandomStateSettlementId(context.Random, stateIdByAreaId, containsMainCity: true);
			if (num4 < 0)
			{
				num4 = id2;
			}
			Settlement settlement = DomainManager.Organization.GetSettlement(num4);
			OrganizationMemberItem orgMemberConfig = GetOrgMemberConfig(settlement.GetOrgTemplateId(), organizationInfo.Grade);
			sbyte rejoinGrade = orgMemberConfig.GetRejoinGrade();
			OrganizationInfo destOrgInfo = new OrganizationInfo(settlement.GetOrgTemplateId(), rejoinGrade, principal: true, num4);
			DomainManager.Organization.ChangeOrganization(context, character, destOrgInfo);
		}
	}

	public void TryAddSectMemberFeature(DataContext context, GameData.Domains.Character.Character character, OrganizationInfo dstOrgInfo)
	{
		if (character.GetAgeGroup() == 2)
		{
			short memberFeature = Config.Organization.Instance[dstOrgInfo.OrgTemplateId].MemberFeature;
			if (memberFeature >= 0 && dstOrgInfo.Grade >= GlobalConfig.Instance.AddMemberFeatureMinGrade)
			{
				character.AddFeature(context, memberFeature);
			}
		}
	}

	public static short GetApprovingRateUpperLimit()
	{
		int num = Math.Clamp(DomainManager.World.GetXiangshuLevel(), 0, GlobalConfig.Instance.SectApprovingRateUpperLimits.Length);
		int val = GlobalConfig.Instance.SectApprovingRateUpperLimits[num] * 10;
		return (short)Math.Min(val, 1000);
	}

	public static sbyte GetHighestGradeOfTeachableCombatSkill(short approvingRate)
	{
		if (approvingRate < 300)
		{
			return 1;
		}
		int value = 2 + (approvingRate - 300) / 100;
		return (sbyte)Math.Clamp(value, 0, 8);
	}

	public static bool IsLargeSect(short orgTemplateId)
	{
		return orgTemplateId >= 1 && orgTemplateId <= 15;
	}

	public static sbyte GetLargeSectIndex(sbyte orgTemplateId)
	{
		return (sbyte)((orgTemplateId >= 1 && orgTemplateId <= 15) ? ((sbyte)(orgTemplateId - 1)) : (-1));
	}

	public static sbyte GetLargeSectTemplateId(sbyte index)
	{
		return (sbyte)(index + 1);
	}

	public void UpdateSectPrisonersOnAdvanceMonth(DataContext context)
	{
		foreach (var (_, sect2) in _sects)
		{
			sect2.UpdatePrisonOnAdvanceMonth(context);
		}
	}

	public void UpdateFugitiveGroupsOnAdvanceMonth(DataContext context)
	{
		Dictionary<IntPair, List<GameData.Domains.Character.Character>> dictionary = new Dictionary<IntPair, List<GameData.Domains.Character.Character>>();
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		foreach (var (_, sect2) in _sects)
		{
			dictionary.Clear();
			List<SettlementBounty> bounties = sect2.Prison.Bounties;
			foreach (SettlementBounty item in bounties)
			{
				if (!DomainManager.Character.TryGetElement_Objects(item.CharId, out var element) || !element.IsInteractableAsIntelligentCharacter() || element == taiwu)
				{
					continue;
				}
				Location location = element.GetLocation();
				if (location.AreaId < 0 || !DomainManager.Character.TryGetCharacterPrioritizedAction(item.CharId, out var action) || (action.ActionType != 18 && action.ActionType != 19))
				{
					continue;
				}
				int leaderId = element.GetLeaderId();
				if (leaderId >= 0)
				{
					continue;
				}
				PrioritizedActionsItem prioritizedActionsItem = PrioritizedActions.Instance[action.ActionType];
				sbyte behaviorType = element.GetBehaviorType();
				if (context.Random.CheckPercentProb(prioritizedActionsItem.ActionJointChance[behaviorType]))
				{
					IntPair key = new IntPair(location.AreaId, action.Target.GetRealTargetLocation().AreaId);
					if (!dictionary.TryGetValue(key, out var value))
					{
						value = new List<GameData.Domains.Character.Character>();
						dictionary.Add(key, value);
					}
					value.Add(element);
				}
			}
			foreach (var (intPair2, list2) in dictionary)
			{
				if (list2.Count <= 1)
				{
					continue;
				}
				GameData.Domains.Character.Character random = list2.GetRandom(context.Random);
				foreach (GameData.Domains.Character.Character item2 in list2)
				{
					if (random != item2)
					{
						DomainManager.Character.JoinGroup(context, item2, random);
					}
				}
			}
		}
	}

	public void UpdateOrganizationMembers(DataContext context)
	{
		int currDate = DomainManager.World.GetCurrDate();
		Dictionary<int, (GameData.Domains.Character.Character, short)> baseInfluencePowers = new Dictionary<int, (GameData.Domains.Character.Character, short)>();
		HashSet<int> relatedCharIds = new HashSet<int>();
		short key;
		foreach (KeyValuePair<short, Sect> sect2 in _sects)
		{
			sect2.Deconstruct(out key, out var value);
			Sect sect = value;
			sbyte orgTemplateId = sect.GetOrgTemplateId();
			sect.UpdateMemberGrades(context);
			short influencePowerUpdateInterval = Config.Organization.Instance[orgTemplateId].InfluencePowerUpdateInterval;
			int influencePowerUpdateDate = sect.GetInfluencePowerUpdateDate();
			if (influencePowerUpdateInterval > 0 && currDate >= influencePowerUpdateDate)
			{
				sect.UpdateInfluencePowers(context, baseInfluencePowers, relatedCharIds);
				sect.SetInfluencePowerUpdateDate(currDate + influencePowerUpdateInterval, context);
				UpdateFactions(context, sect);
			}
			if (currDate % 3 == 0)
			{
				sect.UpdateApprovalOfTaiwu(context);
			}
			UpdateAllMentorsAndMenteesInSect(context, sect);
			sect.UpdateTreasuryOnAdvanceMonth(context);
			sect.PrisonEnteredStatus = 0;
		}
		foreach (KeyValuePair<short, CivilianSettlement> civilianSettlement2 in _civilianSettlements)
		{
			civilianSettlement2.Deconstruct(out key, out var value2);
			CivilianSettlement civilianSettlement = value2;
			sbyte orgTemplateId2 = civilianSettlement.GetOrgTemplateId();
			civilianSettlement.UpdateMemberGrades(context);
			short influencePowerUpdateInterval2 = Config.Organization.Instance[orgTemplateId2].InfluencePowerUpdateInterval;
			int influencePowerUpdateDate2 = civilianSettlement.GetInfluencePowerUpdateDate();
			if (influencePowerUpdateInterval2 > 0 && currDate >= influencePowerUpdateDate2)
			{
				civilianSettlement.UpdateInfluencePowers(context, baseInfluencePowers, relatedCharIds);
				civilianSettlement.SetInfluencePowerUpdateDate(currDate + influencePowerUpdateInterval2, context);
			}
			civilianSettlement.UpdateTreasuryOnAdvanceMonth(context);
		}
		short taiwuVillageSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
		Settlement settlement = DomainManager.Organization.GetSettlement(taiwuVillageSettlementId);
		sbyte orgTemplateId3 = settlement.GetOrgTemplateId();
		short influencePowerUpdateInterval3 = Config.Organization.Instance[orgTemplateId3].InfluencePowerUpdateInterval;
		int influencePowerUpdateDate3 = settlement.GetInfluencePowerUpdateDate();
		if (influencePowerUpdateInterval3 > 0 && currDate >= influencePowerUpdateDate3)
		{
			settlement.UpdateTaiwuVillagerInfluencePowers(context, baseInfluencePowers, relatedCharIds);
			settlement.SetInfluencePowerUpdateDate(currDate + influencePowerUpdateInterval3, context);
		}
		UpdateSettlementCacheInfo();
	}

	[DomainMethod]
	public void ForceUpdateTaiwuVillager(DataContext context)
	{
		Dictionary<int, (GameData.Domains.Character.Character, short)> baseInfluencePowers = new Dictionary<int, (GameData.Domains.Character.Character, short)>();
		HashSet<int> relatedCharIds = new HashSet<int>();
		short taiwuVillageSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
		Settlement settlement = DomainManager.Organization.GetSettlement(taiwuVillageSettlementId);
		settlement.UpdateTaiwuVillagerInfluencePowers(context, baseInfluencePowers, relatedCharIds);
	}

	private void UpdateSettlementCacheInfo()
	{
		MonthlyActionKey key = MonthlyEventActionsManager.PredefinedKeys["MartialArtTournamentDefault"];
		MonthlyActionBase monthlyAction = DomainManager.TaiwuEvent.GetMonthlyAction(key);
		bool isPreparing = monthlyAction.State != 0;
		if (ParallelUpdateOrganizationMembers)
		{
			Parallel.ForEach(_settlements, delegate(KeyValuePair<short, Settlement> pair)
			{
				Settlement value2 = pair.Value;
				value2.SortMembersByCombatPower();
				if (isPreparing && value2 is Sect sect2)
				{
					sect2.UpdateMartialArtTournamentPreparations();
				}
			});
		}
		else
		{
			foreach (KeyValuePair<short, Settlement> settlement in _settlements)
			{
				Settlement value = settlement.Value;
				value.SortMembersByCombatPower();
				if (isPreparing && value is Sect sect)
				{
					sect.UpdateMartialArtTournamentPreparations();
				}
			}
		}
		DomainManager.Character.UpdateTopThousandCharRanking();
	}

	public void ForceUpdateInfluencePowers(DataContext context)
	{
		Dictionary<int, (GameData.Domains.Character.Character, short)> baseInfluencePowers = new Dictionary<int, (GameData.Domains.Character.Character, short)>();
		HashSet<int> relatedCharIds = new HashSet<int>();
		int currDate = DomainManager.World.GetCurrDate();
		DomainManager.Extra.InitTreasurySupplies();
		foreach (KeyValuePair<short, Settlement> settlement2 in _settlements)
		{
			Settlement value = settlement2.Value;
			short influencePowerUpdateInterval = Config.Organization.Instance[value.GetOrgTemplateId()].InfluencePowerUpdateInterval;
			value.UpdateInfluencePowers(context, baseInfluencePowers, relatedCharIds);
			if (influencePowerUpdateInterval > 0)
			{
				value.SetInfluencePowerUpdateDate(currDate + influencePowerUpdateInterval, context);
			}
			if (value is Sect settlement)
			{
				UpdateFactions(context, settlement);
			}
		}
	}

	public void ResetSectExploreStatuses(DataContext context)
	{
		foreach (var (_, sect2) in _sects)
		{
			sect2.SetTaiwuExploreStatus(0, context);
			sect2.SetSpiritualDebtInteractionOccurred(spiritualDebtInteractionOccurred: false, context);
		}
	}

	public void BecomeSectMonk(DataContext context, GameData.Domains.Character.Character character, short mentorSeniorityId)
	{
		OrganizationInfo organizationInfo = character.GetOrganizationInfo();
		OrganizationMemberItem orgMemberConfig = GetOrgMemberConfig(organizationInfo);
		Sect sect = _sects[organizationInfo.SettlementId];
		BecomeSectMonkInternal(context, character, sect, orgMemberConfig, mentorSeniorityId);
	}

	public static void TryBecomeSectMonk(DataContext context, GameData.Domains.Character.Character character, Sect sect, OrganizationMemberItem orgMemberCfg, short mentorSeniorityId)
	{
		if (CheckConditionOfBecomingSectMonk(context, character, orgMemberCfg) && orgMemberCfg.ProbOfBecomingMonk > 0 && context.Random.CheckPercentProb(orgMemberCfg.ProbOfBecomingMonk))
		{
			BecomeSectMonkInternal(context, character, sect, orgMemberCfg, mentorSeniorityId);
		}
	}

	public void SelectRandomSectCharacterToApproveTaiwu(DataContext context, sbyte orgTemplateId, sbyte grade)
	{
		Tester.Assert(IsSect(orgTemplateId));
		Settlement settlementByOrgTemplateId = GetSettlementByOrgTemplateId(orgTemplateId);
		OrgMemberCollection members = settlementByOrgTemplateId.GetMembers();
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		for (sbyte b = grade; b >= 0; b--)
		{
			IEnumerable<int> enumerable = DomainManager.Character.ExcludeInfant(members.GetMembers(b));
			list.Clear();
			foreach (int item in enumerable)
			{
				SectCharacter sectCharacter = _sectCharacters[item];
				if (!sectCharacter.GetApprovedTaiwu())
				{
					list.Add(item);
				}
			}
			if (list.Count > 0)
			{
				break;
			}
		}
		if (list.Count > 0)
		{
			int random = list.GetRandom(context.Random);
			SectCharacter sectCharacter2 = _sectCharacters[random];
			sectCharacter2.SetApprovedTaiwu(context, approve: true);
		}
		ObjectPool<List<int>>.Instance.Return(list);
	}

	public void ShowCharactersStats()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		List<int> list = new List<int>();
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Organization characters:");
		short key;
		StringBuilder stringBuilder2;
		StringBuilder.AppendInterpolatedStringHandler handler;
		foreach (KeyValuePair<short, Sect> sect2 in _sects)
		{
			sect2.Deconstruct(out key, out var value);
			Sect sect = value;
			MapBlockData rootBlock = DomainManager.Map.GetBlock(sect.GetLocation()).GetRootBlock();
			string name = MapBlock.Instance[rootBlock.TemplateId].Name;
			(int, int, int) ageStats = GetAgeStats(sect.GetMembers(), list);
			int item = ageStats.Item1;
			int item2 = ageStats.Item2;
			int item3 = ageStats.Item3;
			num += item;
			num2 += item2;
			num3 += item3;
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder3 = stringBuilder2;
			handler = new StringBuilder.AppendInterpolatedStringHandler(8, 4, stringBuilder2);
			handler.AppendLiteral("  ");
			handler.AppendFormatted<string>(name, 5);
			handler.AppendLiteral(": ");
			handler.AppendFormatted(item, 3);
			handler.AppendLiteral(", ");
			handler.AppendFormatted(item2, 3);
			handler.AppendLiteral(", ");
			handler.AppendFormatted(item3, 3);
			stringBuilder3.AppendLine(ref handler);
		}
		foreach (KeyValuePair<short, CivilianSettlement> civilianSettlement2 in _civilianSettlements)
		{
			civilianSettlement2.Deconstruct(out key, out var value2);
			CivilianSettlement civilianSettlement = value2;
			short randomNameId = civilianSettlement.GetRandomNameId();
			string name2;
			if (randomNameId >= 0)
			{
				name2 = LocalTownNames.Instance.TownNameCore[randomNameId].Name;
			}
			else
			{
				MapBlockData rootBlock2 = DomainManager.Map.GetBlock(civilianSettlement.GetLocation()).GetRootBlock();
				name2 = MapBlock.Instance[rootBlock2.TemplateId].Name;
			}
			(int, int, int) ageStats2 = GetAgeStats(civilianSettlement.GetMembers(), list);
			int item4 = ageStats2.Item1;
			int item5 = ageStats2.Item2;
			int item6 = ageStats2.Item3;
			num += item4;
			num2 += item5;
			num3 += item6;
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder4 = stringBuilder2;
			handler = new StringBuilder.AppendInterpolatedStringHandler(8, 4, stringBuilder2);
			handler.AppendLiteral("  ");
			handler.AppendFormatted<string>(name2, 5);
			handler.AppendLiteral(": ");
			handler.AppendFormatted(item4, 3);
			handler.AppendLiteral(", ");
			handler.AppendFormatted(item5, 3);
			handler.AppendLiteral(", ");
			handler.AppendFormatted(item6, 3);
			stringBuilder4.AppendLine(ref handler);
		}
		int num4 = num + num2 + num3;
		float value3 = (float)num * 100f / (float)num4;
		float value4 = (float)num2 * 100f / (float)num4;
		float value5 = (float)num3 * 100f / (float)num4;
		stringBuilder2 = stringBuilder;
		StringBuilder stringBuilder5 = stringBuilder2;
		handler = new StringBuilder.AppendInterpolatedStringHandler(23, 6, stringBuilder2);
		handler.AppendLiteral("Total: ");
		handler.AppendFormatted(num);
		handler.AppendLiteral(" (");
		handler.AppendFormatted(value3, "N1");
		handler.AppendLiteral("%)");
		handler.AppendLiteral(", ");
		handler.AppendFormatted(num2);
		handler.AppendLiteral(" (");
		handler.AppendFormatted(value4, "N1");
		handler.AppendLiteral("%)");
		handler.AppendLiteral(", ");
		handler.AppendFormatted(num3);
		handler.AppendLiteral(" (");
		handler.AppendFormatted(value5, "N1");
		handler.AppendLiteral("%)");
		stringBuilder5.AppendLine(ref handler);
		Logger.Info<StringBuilder>(stringBuilder);
		Histogram histogram = new Histogram(0, 60, 20);
		histogram.Record(list);
		Logger.Info($"World ages ({list.Count}):\n{histogram.GetTextGraph()}");
	}

	[DomainMethod]
	public short GetOrganizationTemplateIdOfTaiwuLocation()
	{
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		if (!location.IsValid())
		{
			return -1;
		}
		MapBlockData blockData = DomainManager.Map.GetBlockData(location.AreaId, location.BlockId);
		return GetSettlementByLocation(blockData.GetRootBlock().GetLocation())?.GetOrgTemplateId() ?? (-1);
	}

	private void InitializeSettlementsCache()
	{
		_settlements = new Dictionary<short, Settlement>();
		_locationSettlements = new Dictionary<Location, Settlement>();
		int num = CalcOrgTemplateCount();
		_orgTemplateId2Settlements = new List<Settlement>[num];
		short key;
		foreach (KeyValuePair<short, Sect> sect in _sects)
		{
			sect.Deconstruct(out key, out var value);
			Sect settlement = value;
			AddSettlementCache(settlement);
		}
		foreach (KeyValuePair<short, CivilianSettlement> civilianSettlement in _civilianSettlements)
		{
			civilianSettlement.Deconstruct(out key, out var value2);
			CivilianSettlement settlement2 = value2;
			AddSettlementCache(settlement2);
		}
	}

	private void InitializeSortedMembersCache(DataContext context, DataUid dataUid)
	{
		UpdateSettlementCacheInfo();
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(dataUid, "InitializeSortedMembersCache");
	}

	private void CreateRelationWithAllSettlementMembers(DataContext context, GameData.Domains.Character.Character character, OrgMemberCollection members)
	{
		for (sbyte b = 0; b < 9; b++)
		{
			foreach (int member in members.GetMembers(b))
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(member);
				DomainManager.Character.TryCreateGeneralRelation(context, character, element_Objects);
			}
		}
	}

	private static int CalcOrgTemplateCount()
	{
		sbyte b = -1;
		foreach (OrganizationItem item in (IEnumerable<OrganizationItem>)Config.Organization.Instance)
		{
			if (item.TemplateId > b)
			{
				b = item.TemplateId;
			}
		}
		return b + 1;
	}

	private void AddSettlementCache(Settlement settlement)
	{
		_locationSettlements.Add(settlement.GetLocation(), settlement);
		_settlements.Add(settlement.GetId(), settlement);
		sbyte orgTemplateId = settlement.GetOrgTemplateId();
		List<Settlement> list = _orgTemplateId2Settlements[orgTemplateId];
		if (list == null)
		{
			list = new List<Settlement>();
			_orgTemplateId2Settlements[orgTemplateId] = list;
		}
		list.Add(settlement);
	}

	private void RemoveSettlementCache(Settlement settlement)
	{
		_settlements.Remove(settlement.GetId());
		_orgTemplateId2Settlements[settlement.GetOrgTemplateId()]?.Remove(settlement);
	}

	private void InitializeSettlementCharactersCache()
	{
		_settlementCharacters = new Dictionary<int, SettlementCharacter>();
		int key;
		foreach (KeyValuePair<int, SectCharacter> sectCharacter in _sectCharacters)
		{
			sectCharacter.Deconstruct(out key, out var value);
			int key2 = key;
			SectCharacter value2 = value;
			_settlementCharacters.Add(key2, value2);
		}
		foreach (KeyValuePair<int, CivilianSettlementCharacter> civilianSettlementCharacter in _civilianSettlementCharacters)
		{
			civilianSettlementCharacter.Deconstruct(out key, out var value3);
			int key3 = key;
			CivilianSettlementCharacter value4 = value3;
			_settlementCharacters.Add(key3, value4);
		}
	}

	private static void CheckPrincipalMembersAmount(sbyte orgTemplateId, sbyte grade, OrgMemberCollection members)
	{
		OrganizationMemberItem orgMemberConfig = GetOrgMemberConfig(orgTemplateId, grade);
		if (!orgMemberConfig.RestrictPrincipalAmount)
		{
			return;
		}
		int num = 0;
		HashSet<int> members2 = members.GetMembers(grade);
		foreach (int item in members2)
		{
			if (DomainManager.Character.TryGetElement_Objects(item, out var element) && element.GetOrganizationInfo().Principal)
			{
				num++;
			}
		}
		if (num <= orgMemberConfig.Amount)
		{
			return;
		}
		throw new Exception($"The number of principal members exceeds the max limit: {orgMemberConfig.TemplateId}");
	}

	private short SetRandomSectMentor(DataContext context, int charId, OrganizationInfo orgInfo, OrgMemberCollection sectMembers, sbyte mentorGrade)
	{
		if (mentorGrade < 0)
		{
			return -1;
		}
		HashSet<int> relatedCharIds = DomainManager.Character.GetRelatedCharIds(charId, 2048);
		foreach (int item in relatedCharIds)
		{
			if (DomainManager.Character.TryGetElement_Objects(item, out var element))
			{
				OrganizationInfo organizationInfo = element.GetOrganizationInfo();
				if (organizationInfo.SettlementId == orgInfo.SettlementId && organizationInfo.Grade >= mentorGrade)
				{
					return element.GetMonasticTitle().SeniorityId;
				}
			}
		}
		while (mentorGrade < 9)
		{
			HashSet<int> members = sectMembers.GetMembers(mentorGrade);
			if (members.Count > 0)
			{
				short num = short.MinValue;
				int num2 = -1;
				foreach (int item2 in members)
				{
					GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item2);
					if (element_Objects.GetKidnapperId() < 0 && element_Objects.GetAgeGroup() == 2)
					{
						SettlementCharacter settlementCharacter = GetSettlementCharacter(item2);
						short influencePower = settlementCharacter.GetInfluencePower();
						if (num < influencePower)
						{
							num = influencePower;
							num2 = item2;
						}
					}
				}
				if (num2 >= 0 && RelationTypeHelper.AllowAddingMentorRelation(charId, num2))
				{
					DomainManager.Character.AddRelation(context, charId, num2, 2048);
					GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(num2);
					return element_Objects2.GetMonasticTitle().SeniorityId;
				}
			}
			mentorGrade++;
		}
		return -1;
	}

	private static bool CheckConditionOfBecomingSectMonk(DataContext context, GameData.Domains.Character.Character character, OrganizationMemberItem orgMemberCfg)
	{
		byte monkType = character.GetMonkType();
		if (character.GetTemplateId() == 625)
		{
			return false;
		}
		if (monkType == 0)
		{
			return true;
		}
		if ((monkType & 0x80) == 0)
		{
			character.SetMonkType(0, context);
			return true;
		}
		if (monkType != orgMemberCfg.MonkType)
		{
			throw new Exception($"Monk type of character is not compatible with organization: {monkType}, {orgMemberCfg.TemplateId}");
		}
		if (!DomainManager.Character.IsTemporaryIntelligentCharacter(character.GetId()))
		{
			throw new Exception($"Character.Character is not TemporaryIntelligentCharacter: {character.GetTemplateId()}");
		}
		return false;
	}

	private static void BecomeSectMonkInternal(DataContext context, GameData.Domains.Character.Character character, Sect sect, OrganizationMemberItem orgMemberCfg, short mentorSeniorityId)
	{
		if (orgMemberCfg.MonkType == 0)
		{
			throw new Exception($"Sect member {orgMemberCfg.TemplateId} cannot become monk");
		}
		MonasticTitle monasticTitle = CharacterDomain.CreateSectMemberMonasticTitle(context, sect, mentorSeniorityId);
		character.SetMonasticTitle(monasticTitle, context);
		character.SetMonkType(orgMemberCfg.MonkType, context);
		if (orgMemberCfg.MonkType == 130)
		{
			AvatarData avatar = character.GetAvatar();
			avatar.ResetGrowableElementShowingAbility(0);
			character.SetAvatar(avatar, context);
		}
	}

	private static void TrySecularize(DataContext context, GameData.Domains.Character.Character character)
	{
		byte monkType = character.GetMonkType();
		if ((monkType & 0x80) != 0)
		{
			if (character.GetMonkType() == 130)
			{
				AvatarData avatar = character.GetAvatar();
				avatar.SetGrowableElementShowingAbility(0);
				avatar.ResetGrowableElementShowingState(0);
				character.SetAvatar(avatar, context);
				DomainManager.Character.InitializeAvatarElementGrowthProgress(context, character.GetId(), 0);
			}
			character.SetMonkType(0, context);
			character.SetMonasticTitle(new MonasticTitle(-1, -1), context);
		}
	}

	public void TryDowngradeDeputySpouses(DataContext context, int charId, OrganizationInfo orgInfo)
	{
		OrganizationMemberItem orgMemberConfig = GetOrgMemberConfig(orgInfo);
		if (orgMemberConfig.DeputySpouseDowngrade < 0)
		{
			return;
		}
		HashSet<int> relatedCharIds = DomainManager.Character.GetRelatedCharIds(charId, 1024);
		foreach (int item in relatedCharIds)
		{
			if (!DomainManager.Character.TryGetElement_Objects(item, out var element))
			{
				continue;
			}
			OrganizationInfo organizationInfo = element.GetOrganizationInfo();
			if (!organizationInfo.Principal && organizationInfo.SettlementId == orgInfo.SettlementId && organizationInfo.Grade > orgMemberConfig.DeputySpouseDowngrade)
			{
				sbyte b = orgMemberConfig.DeputySpouseDowngrade;
				if (b < 0)
				{
					b = orgMemberConfig.GetRejoinGrade();
				}
				ChangeGrade(context, element, b, destPrincipal: true);
			}
		}
	}

	public void TryDowngradeDeputySpouse(DataContext context, int charId, OrganizationInfo charOrgInfo, int spouseId)
	{
		if (!DomainManager.Character.TryGetElement_Objects(spouseId, out var element))
		{
			return;
		}
		OrganizationInfo organizationInfo = element.GetOrganizationInfo();
		if (organizationInfo.Principal)
		{
			return;
		}
		OrganizationMemberItem orgMemberConfig = GetOrgMemberConfig(charOrgInfo);
		if (organizationInfo.SettlementId == charOrgInfo.SettlementId && organizationInfo.Grade > orgMemberConfig.DeputySpouseDowngrade)
		{
			sbyte b = orgMemberConfig.DeputySpouseDowngrade;
			if (b < 0)
			{
				b = orgMemberConfig.GetRejoinGrade();
			}
			ChangeGrade(context, element, b, destPrincipal: true);
		}
	}

	private unsafe static (int, int, int) GetAgeStats(OrgMemberCollection members, List<int> ages)
	{
		byte* intPtr = stackalloc byte[12];
		// IL initblk instruction
		Unsafe.InitBlock(intPtr, 0, 12);
		int* ptr = (int*)intPtr;
		List<int> list = new List<int>();
		members.GetAllMembers(list);
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			int objectId = list[i];
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(objectId);
			short currAge = element_Objects.GetCurrAge();
			sbyte ageGroup = AgeGroup.GetAgeGroup(currAge);
			ptr[ageGroup]++;
			ages.Add(currAge);
		}
		return (*ptr, ptr[1], ptr[2]);
	}

	public void BeginCreatingSettlements(IRandomSource randomSource)
	{
		List<short> list = new List<short>();
		List<short> list2 = new List<short>();
		List<short> list3 = new List<short>();
		LocalTownNames instance = LocalTownNames.Instance;
		for (short num = instance.VillageStart; num <= instance.VillageEnd; num++)
		{
			list.Add(instance.TownNameCore[num].TemplateId);
		}
		for (short num2 = instance.TownStart; num2 <= instance.TownEnd; num2++)
		{
			list2.Add(instance.TownNameCore[num2].TemplateId);
		}
		for (short num3 = instance.WalledTownStart; num3 <= instance.WalledTownEnd; num3++)
		{
			list3.Add(instance.TownNameCore[num3].TemplateId);
		}
		CollectionUtils.Shuffle(randomSource, list);
		CollectionUtils.Shuffle(randomSource, list2);
		CollectionUtils.Shuffle(randomSource, list3);
		_settlementCreatingInfo = new SettlementCreatingInfo(list, list2, list3);
	}

	public void EndCreatingSettlements(DataContext context)
	{
		ForceUpdateInfluencePowers(context);
		RecordSettlementStandardPopulations(context);
		DomainManager.World.RecordWorldStandardPopulation(context);
		DomainManager.World.UpdatePopulationRelatedData();
		_settlementCreatingInfo = null;
		_orgInscribedCharIdMap = null;
	}

	public bool IsCreatingSettlements()
	{
		return _settlementCreatingInfo != null;
	}

	public void CreateEmptySects(DataContext context)
	{
		short num = 0;
		foreach (OrganizationItem item in (IEnumerable<OrganizationItem>)Config.Organization.Instance)
		{
			if (item.IsSect)
			{
				short num2 = GenerateNextSettlementId(context);
				Sect sect = new Sect(num2, new Location(-1, num), item.TemplateId, context.Random);
				AddElement_Sects(num2, sect);
				AddSettlementCache(sect);
				num++;
			}
		}
	}

	public short CreateSettlement(DataContext context, Location location, sbyte orgTemplateId)
	{
		short num = GenerateNextSettlementId(context);
		if (IsSect(orgTemplateId))
		{
			Sect sect = new Sect(num, location, orgTemplateId, context.Random);
			AddElement_Sects(num, sect);
			SetLargeSectFavorabilities(_largeSectFavorabilities, context);
			AddSettlementCache(sect);
			CreateSettlementMembers(context, sect);
		}
		else
		{
			CivilianSettlement civilianSettlement = new CivilianSettlement(num, location, orgTemplateId, _settlementCreatingInfo, context.Random);
			AddElement_CivilianSettlements(num, civilianSettlement);
			AddSettlementCache(civilianSettlement);
			CreateSettlementMembers(context, civilianSettlement);
		}
		return num;
	}

	[DomainMethod]
	public void SetInscribedCharactersForCreation(DataContext context, List<InscribedCharacterKey> inscribedCharList)
	{
		_orgInscribedCharIdMap = new Dictionary<sbyte, List<InscribedCharacter>>();
		if (inscribedCharList != null && inscribedCharList.Count > 0)
		{
			IRandomSource random = context.Random;
			int count = inscribedCharList.Count;
			if (1 == 0)
			{
			}
			int num = count switch
			{
				1 => random.Next(2), 
				2 => 1, 
				_ => count / 3, 
			};
			if (1 == 0)
			{
			}
			int num2 = num;
			CollectionUtils.Shuffle(random, inscribedCharList);
			Logger.Info($"Inscribed Character to settlements: {num2} civilians / {count - num2} sect members");
			for (int i = 0; i < num2; i++)
			{
				InscribedCharacterKey elementId = inscribedCharList[i];
				InscribedCharacter element_InscribedCharacters = DomainManager.Global.GetElement_InscribedCharacters(elementId);
				sbyte orgTemplateId = (sbyte)(21 + random.Next(15));
				AddOrgInscribedCharacter(orgTemplateId, element_InscribedCharacters);
			}
			for (int j = num2; j < count; j++)
			{
				InscribedCharacterKey elementId2 = inscribedCharList[j];
				InscribedCharacter element_InscribedCharacters2 = DomainManager.Global.GetElement_InscribedCharacters(elementId2);
				sbyte bestMatchingOrgTemplateId = GetBestMatchingOrgTemplateId(random, element_InscribedCharacters2.Gender, element_InscribedCharacters2.BaseCombatSkillQualifications, element_InscribedCharacters2.BaseLifeSkillQualifications);
				AddOrgInscribedCharacter(bestMatchingOrgTemplateId, element_InscribedCharacters2);
			}
		}
		static void AddOrgInscribedCharacter(sbyte key, InscribedCharacter inscribedCharacter)
		{
			if (!_orgInscribedCharIdMap.TryGetValue(key, out var value))
			{
				value = new List<InscribedCharacter>();
				_orgInscribedCharIdMap.Add(key, value);
			}
			value.Add(inscribedCharacter);
		}
	}

	private sbyte GetBestMatchingOrgTemplateId(IRandomSource random, sbyte gender, CombatSkillShorts combatSkillQualifications, LifeSkillShorts lifeSkillQualifications)
	{
		int num = int.MinValue;
		Span<sbyte> span = stackalloc sbyte[15];
		SpanList<sbyte> spanList = span;
		foreach (OrganizationItem item in (IEnumerable<OrganizationItem>)Config.Organization.Instance)
		{
			if (!item.IsSect || (item.GenderRestriction != -1 && item.GenderRestriction != gender))
			{
				continue;
			}
			OrganizationMemberItem orgMemberConfig = GetOrgMemberConfig(item.TemplateId, 8);
			int num2 = 0;
			for (sbyte b = 0; b < 14; b++)
			{
				short num3 = orgMemberConfig.CombatSkillsAdjust[b];
				if (num3 >= 0)
				{
					num2 += combatSkillQualifications[b] * num3 * 3;
				}
			}
			for (sbyte b2 = 0; b2 < 16; b2++)
			{
				short num4 = orgMemberConfig.LifeSkillsAdjust[b2];
				if (num4 >= 0)
				{
					num2 += lifeSkillQualifications[b2] * num4;
				}
			}
			if (num2 > num)
			{
				spanList.Clear();
				spanList.Add(item.TemplateId);
				num = num2;
			}
			else if (num2 == num)
			{
				spanList.Add(item.TemplateId);
			}
		}
		return spanList.GetRandom(random);
	}

	private short GenerateNextSettlementId(DataContext context)
	{
		short nextSettlementId = _nextSettlementId;
		_nextSettlementId++;
		if ((ushort)_nextSettlementId > 32767)
		{
			_nextSettlementId = 0;
		}
		SetNextSettlementId(_nextSettlementId, context);
		return nextSettlementId;
	}

	private static void CreateSettlementMembers(DataContext context, Settlement settlement)
	{
		sbyte orgTemplateId = settlement.GetOrgTemplateId();
		short id = settlement.GetId();
		Location location = settlement.GetLocation();
		OrgMemberCollection members = settlement.GetMembers();
		if (_orgInscribedCharIdMap.TryGetValue(orgTemplateId, out var value))
		{
			if (_stringBuilder == null)
			{
				_stringBuilder = new StringBuilder();
			}
			_stringBuilder.Clear();
			_stringBuilder.Append("Creating inscribed characters at ");
			_stringBuilder.AppendLine(settlement.GetNameRelatedData().GetName());
			foreach (InscribedCharacter item in value)
			{
				OrganizationInfo inscribedCharTargetOrgInfo = GetInscribedCharTargetOrgInfo(context.Random, item, settlement);
				GameData.Domains.Character.Character character = DomainManager.Character.CreateIntelligentCharacterFromInscription(context, item, inscribedCharTargetOrgInfo);
				DomainManager.Character.CompleteCreatingCharacter(character.GetId());
				_stringBuilder.Append('\t');
				_stringBuilder.AppendLine(character.ToString());
			}
			Logger.Info(_stringBuilder.ToString());
		}
		OrganizationItem organizationItem = Config.Organization.Instance[orgTemplateId];
		if (organizationItem.Population <= 0)
		{
			return;
		}
		sbyte b = DomainManager.Map.GetStateTemplateIdByAreaId(location.AreaId);
		if (b <= 0)
		{
			b = DomainManager.World.GetTaiwuVillageStateTemplateId();
		}
		List<short> blockIds = new List<short>();
		DomainManager.Map.GetSettlementBlocks(location.AreaId, location.BlockId, blockIds);
		List<short> list = new List<short>();
		DomainManager.Map.GetSettlementBlocksAndAffiliatedBlocks(location.AreaId, location.BlockId, list);
		SettlementMembersCreationInfo settlementMembersCreationInfo = new SettlementMembersCreationInfo(orgTemplateId, id, b, location.AreaId, blockIds, list);
		int worldPopulationFactor = DomainManager.World.GetWorldPopulationFactor();
		sbyte b2 = (sbyte)((orgTemplateId != 16) ? 8 : 7);
		for (sbyte b3 = b2; b3 >= 0; b3--)
		{
			short index = organizationItem.Members[b3];
			OrganizationMemberItem organizationMemberItem = OrganizationMember.Instance[index];
			if (organizationMemberItem.Amount > 0)
			{
				settlementMembersCreationInfo.CoreMemberConfig = organizationMemberItem;
				HashSet<int> members2 = members.GetMembers(b3);
				int num = organizationMemberItem.Amount;
				if (!organizationMemberItem.RestrictPrincipalAmount)
				{
					num = Math.Max(1, num * worldPopulationFactor / 125);
				}
				else if (members2.Count > 0)
				{
					num -= members2.Count;
				}
				for (int i = 0; i < num; i++)
				{
					CreateCoreCharacter(context, settlementMembersCreationInfo);
					CreateBrothersAndSisters(context, settlementMembersCreationInfo);
					CreateSpouseAndChildren(context, settlementMembersCreationInfo);
					settlementMembersCreationInfo.CompleteCreatingCharacters();
				}
			}
		}
	}

	private static OrganizationInfo GetInscribedCharTargetOrgInfo(IRandomSource random, InscribedCharacter inscribedChar, Settlement settlement)
	{
		OrgMemberCollection members = settlement.GetMembers();
		sbyte orgTemplateId = settlement.GetOrgTemplateId();
		bool flag = settlement is Sect;
		sbyte mainAttributeGrade = CharacterCreation.GetMainAttributeGrade(inscribedChar.BaseMainAttributes.GetSum());
		sbyte combatSkillQualificationGrade = CharacterCreation.GetCombatSkillQualificationGrade(inscribedChar.BaseCombatSkillQualifications.GetSum());
		sbyte lifeSkillQualificationGrade = CharacterCreation.GetLifeSkillQualificationGrade(inscribedChar.BaseLifeSkillQualifications.GetSum());
		sbyte b = (sbyte)Math.Clamp((mainAttributeGrade + combatSkillQualificationGrade + lifeSkillQualificationGrade) / 3, 0, 8);
		if (flag)
		{
			for (b = (sbyte)Math.Clamp(random.Next(b - 2, b + 1), 0, 8); b >= 0; b--)
			{
				OrganizationMemberItem orgMemberConfig = GetOrgMemberConfig(orgTemplateId, b);
				if ((!orgMemberConfig.RestrictPrincipalAmount || members.GetMembers(b).Count < orgMemberConfig.Amount) && (orgMemberConfig.Gender == -1 || orgMemberConfig.Gender == inscribedChar.Gender))
				{
					break;
				}
			}
		}
		else
		{
			OrganizationMemberItem orgMemberConfig2 = GetOrgMemberConfig(orgTemplateId, b);
			if (orgMemberConfig2.RestrictPrincipalAmount && members.GetMembers(b).Count >= orgMemberConfig2.Amount)
			{
				b = Math.Max(0, orgMemberConfig2.ChildGrade);
			}
			else if (orgMemberConfig2.Gender != -1 && orgMemberConfig2.Gender != inscribedChar.Gender)
			{
				b = 0;
			}
		}
		return new OrganizationInfo(orgTemplateId, b, principal: true, settlement.GetId());
	}

	public static void CreateCoreCharacter(DataContext context, SettlementMembersCreationInfo info)
	{
		IRandomSource random = context.Random;
		Genome.CreateRandom(random, ref info.CoreMotherGenome);
		Genome.CreateRandom(random, ref info.CoreFatherGenome);
		sbyte gender = ((info.CoreMemberConfig.Gender == -1) ? Gender.GetRandom(random) : info.CoreMemberConfig.Gender);
		short characterTemplateId = GetCharacterTemplateId(info.OrgTemplateId, info.MapStateTemplateId, gender);
		short initialAge = GetInitialAge(info.CoreMemberConfig);
		short num2;
		if (initialAge >= 0)
		{
			int num = initialAge / 4;
			num2 = (short)(initialAge + random.Next(-num, num + 1));
		}
		else
		{
			num2 = GameData.Domains.Character.Character.GenerateRandomAge(random);
		}
		short blockId = (info.CoreMemberConfig.CanStroll ? info.NearbyBlockIds[random.Next(info.NearbyBlockIds.Count)] : info.BlockIds[random.Next(info.BlockIds.Count)]);
		sbyte grade = (sbyte)((info.OrgTemplateId != 16) ? info.CoreMemberConfig.Grade : 0);
		IntelligentCharacterCreationInfo intelligentCharacterCreationInfo = new IntelligentCharacterCreationInfo(orgInfo: new OrganizationInfo(info.OrgTemplateId, grade, principal: true, info.SettlementId), location: new Location(info.AreaId, blockId), charTemplateId: characterTemplateId);
		intelligentCharacterCreationInfo.Age = num2;
		intelligentCharacterCreationInfo.SpecifyGenome = true;
		IntelligentCharacterCreationInfo info2 = intelligentCharacterCreationInfo;
		Genome.Inherit(random, ref info.CoreMotherGenome, ref info.CoreFatherGenome, ref info2.Genome);
		GameData.Domains.Character.Character character = DomainManager.Character.CreateIntelligentCharacter(context, ref info2);
		int id = character.GetId();
		info.CreatedCharIds.Add(id);
		bool flag = character.GetFertility() <= 0;
		if (num2 >= 16 && !flag && random.CheckPercentProb(10))
		{
			character.LoseVirginity(context);
		}
		info.CoreCharId = id;
		info.CoreChar = character;
		info.IsCoreCharInfertile = flag;
		if (character.GetGender() == 0)
		{
			info.CoreMotherAvatar = character.GetAvatar();
			info.CoreFatherAvatar = new AvatarData(info.CoreMotherAvatar);
			info.CoreFatherAvatar.ChangeGender(1);
			info.CoreFatherAvatar.ChangeBodyType(BodyType.GetRandom(random));
		}
		else
		{
			info.CoreFatherAvatar = character.GetAvatar();
			info.CoreMotherAvatar = new AvatarData(info.CoreFatherAvatar);
			info.CoreMotherAvatar.ChangeGender(0);
			info.CoreMotherAvatar.ChangeBodyType(BodyType.GetRandom(random));
		}
	}

	private unsafe static void CreateBrothersAndSisters(DataContext context, SettlementMembersCreationInfo info)
	{
		IRandomSource random = context.Random;
		int num = RedzenHelper.NormalDistribute(random, 1f, 1f, 1, 3);
		if (info.OrgTemplateId == 16)
		{
			num = Math.Min(num, 2);
		}
		sbyte brotherGrade = info.CoreMemberConfig.BrotherGrade;
		short index = Config.Organization.Instance[info.OrgTemplateId].Members[brotherGrade];
		OrganizationMemberItem organizationMemberItem = OrganizationMember.Instance[index];
		(int, int, ushort)* ptr = stackalloc(int, int, ushort)[num];
		Unsafe.Write(ptr, (info.CoreCharId, info.CoreChar.GetBirthDate(), (ushort)4));
		FullName fullName = info.CoreChar.GetFullName();
		short num2 = info.CoreChar.GetCurrAge();
		for (int i = 1; i < num; i++)
		{
			num2 += (short)(1 + random.Next(2));
			sbyte gender = ((organizationMemberItem.Gender == -1) ? Gender.GetRandom(random) : organizationMemberItem.Gender);
			short characterTemplateId = GetCharacterTemplateId(info.OrgTemplateId, info.MapStateTemplateId, gender);
			ushort num3 = (ushort)(random.CheckPercentProb(75) ? 4 : 512);
			short blockId = (organizationMemberItem.CanStroll ? info.NearbyBlockIds[random.Next(info.NearbyBlockIds.Count)] : info.BlockIds[random.Next(info.BlockIds.Count)]);
			IntelligentCharacterCreationInfo intelligentCharacterCreationInfo = new IntelligentCharacterCreationInfo(orgInfo: new OrganizationInfo(info.OrgTemplateId, brotherGrade, principal: true, info.SettlementId), location: new Location(info.AreaId, blockId), charTemplateId: characterTemplateId);
			intelligentCharacterCreationInfo.Age = num2;
			IntelligentCharacterCreationInfo info2 = intelligentCharacterCreationInfo;
			if (num3 == 4)
			{
				info2.Avatar = AvatarManager.Instance.GetRandomAvatar(random, gender, transgender: false, -1, info.CoreFatherAvatar, info.CoreMotherAvatar);
				info2.BaseAttraction = info2.Avatar.GetBaseCharm();
				info2.SpecifyGenome = true;
				Genome.Inherit(random, ref info.CoreMotherGenome, ref info.CoreFatherGenome, ref info2.Genome);
				info2.ReferenceFullName = fullName;
			}
			GameData.Domains.Character.Character character = DomainManager.Character.CreateIntelligentCharacter(context, ref info2);
			int id = character.GetId();
			info.CreatedCharIds.Add(id);
			if (num2 >= 16 && random.CheckPercentProb(10) && character.GetFertility() > 0)
			{
				character.LoseVirginity(context);
			}
			Unsafe.Write(ptr + i, (id, character.GetBirthDate(), num3));
		}
		for (int j = 0; j < num; j++)
		{
			(int, int, ushort) tuple = ptr[j];
			int item = tuple.Item1;
			int item2 = tuple.Item2;
			ushort item3 = tuple.Item3;
			for (int k = j + 1; k < num; k++)
			{
				(int, int, ushort) tuple2 = ptr[k];
				int item4 = tuple2.Item1;
				ushort item5 = tuple2.Item3;
				ushort addingType = (ushort)((item3 == 4 && item5 == 4) ? 4 : 512);
				DomainManager.Character.AddRelation(context, item, item4, addingType, item2);
			}
		}
	}

	private static void CreateSpouseAndChildren(DataContext context, SettlementMembersCreationInfo info)
	{
		if (info.CoreMemberConfig.ChildGrade < 0 || info.CoreChar.GetMonkType() != 0)
		{
			return;
		}
		IRandomSource random = context.Random;
		int percentProb = Math.Min((info.CoreChar.GetCurrAge() - 20) * 10, 90);
		if (random.CheckPercentProb(percentProb))
		{
			bool flag = info.OrgTemplateId == 16;
			CreateSpouse(context, info);
			if (!flag && random.CheckPercentProb(10))
			{
				CreateLover(context, info);
			}
			short fertility = info.CoreChar.GetFertility();
			short fertility2 = info.SpouseChar.GetFertility();
			if (fertility > 0 && fertility2 > 0 && fertility * fertility2 > random.Next(20000))
			{
				info.CoreChar.LoseVirginity(context);
				info.SpouseChar.LoseVirginity(context);
				CreateChildren(context, info, isBloodChildren: true);
			}
			else if (!flag && random.CheckPercentProb((info.CoreChar.GetCurrAge() - 40) * 2))
			{
				CreateChildren(context, info, isBloodChildren: false);
			}
		}
		else if (random.CheckPercentProb(25))
		{
			CreateLover(context, info);
		}
	}

	private static void CreateSpouse(DataContext context, SettlementMembersCreationInfo info)
	{
		IRandomSource random = context.Random;
		sbyte grade = (sbyte)((info.OrgTemplateId != 16) ? info.CoreChar.GetOrganizationInfo().Grade : 0);
		sbyte b = Gender.Flip(info.CoreChar.GetGender());
		short characterTemplateId = GetCharacterTemplateId(info.OrgTemplateId, info.MapStateTemplateId, b);
		short age = (short)Math.Max(info.CoreChar.GetCurrAge() + ((b != 0) ? 1 : (-1)) * random.Next(16), 16);
		short blockId = (info.CoreMemberConfig.CanStroll ? info.NearbyBlockIds[random.Next(info.NearbyBlockIds.Count)] : info.BlockIds[random.Next(info.BlockIds.Count)]);
		IntelligentCharacterCreationInfo intelligentCharacterCreationInfo = new IntelligentCharacterCreationInfo(orgInfo: new OrganizationInfo(info.OrgTemplateId, grade, info.CoreMemberConfig.DeputySpouseDowngrade < 0, info.SettlementId), location: new Location(info.AreaId, blockId), charTemplateId: characterTemplateId);
		intelligentCharacterCreationInfo.Age = age;
		IntelligentCharacterCreationInfo info2 = intelligentCharacterCreationInfo;
		GameData.Domains.Character.Character character = DomainManager.Character.CreateIntelligentCharacter(context, ref info2);
		int id = character.GetId();
		info.CreatedCharIds.Add(id);
		GameData.Domains.Character.Character character2 = ((b == 0) ? character : info.CoreChar);
		int establishmentDate = character2.GetBirthDate() + 192;
		if (!RelationTypeHelper.AllowAddingHusbandOrWifeRelation(info.CoreCharId, id))
		{
			throw new Exception($"Failed to add husband or wife relation: {info.CoreCharId} - {id}");
		}
		DomainManager.Character.AddRelation(context, info.CoreCharId, id, 1024, establishmentDate);
		if (context.Random.NextBool())
		{
			DomainManager.Character.ChangeRelationType(context, info.CoreCharId, id, 0, 16384);
		}
		if (context.Random.NextBool())
		{
			DomainManager.Character.ChangeRelationType(context, id, info.CoreCharId, 0, 16384);
		}
		if (!info.IsCoreCharInfertile && random.CheckPercentProb(75) && character.GetFertility() > 0)
		{
			info.CoreChar.LoseVirginity(context);
			character.LoseVirginity(context);
			if (random.CheckPercentProb(10))
			{
				GameData.Domains.Character.Character father = ((b == 1) ? character : info.CoreChar);
				character2.AddFeature(context, 197);
				DomainManager.Character.CreatePregnantState(context, character2, father, isRaped: false);
			}
		}
		info.SpouseCharId = id;
		info.SpouseChar = character;
	}

	private static void CreateLover(DataContext context, SettlementMembersCreationInfo info)
	{
		IRandomSource random = context.Random;
		sbyte brotherGrade = info.CoreMemberConfig.BrotherGrade;
		sbyte b = Gender.Flip(info.CoreChar.GetGender());
		short characterTemplateId = GetCharacterTemplateId(info.OrgTemplateId, info.MapStateTemplateId, b);
		short age = (short)Math.Max(info.CoreChar.GetCurrAge() + ((b != 0) ? 1 : (-1)) * random.Next(16), 16);
		short index = Config.Organization.Instance[info.OrgTemplateId].Members[brotherGrade];
		OrganizationMemberItem organizationMemberItem = OrganizationMember.Instance[index];
		short blockId = (organizationMemberItem.CanStroll ? info.NearbyBlockIds[random.Next(info.NearbyBlockIds.Count)] : info.BlockIds[random.Next(info.BlockIds.Count)]);
		IntelligentCharacterCreationInfo intelligentCharacterCreationInfo = new IntelligentCharacterCreationInfo(orgInfo: new OrganizationInfo(info.OrgTemplateId, brotherGrade, principal: true, info.SettlementId), location: new Location(info.AreaId, blockId), charTemplateId: characterTemplateId);
		intelligentCharacterCreationInfo.Age = age;
		IntelligentCharacterCreationInfo info2 = intelligentCharacterCreationInfo;
		GameData.Domains.Character.Character character = DomainManager.Character.CreateIntelligentCharacter(context, ref info2);
		int id = character.GetId();
		info.CreatedCharIds.Add(id);
		if (random.CheckPercentProb(50))
		{
			if (!RelationTypeHelper.AllowAddingAdoredRelation(info.CoreCharId, id))
			{
				throw new Exception($"Failed to add adored relation: {info.CoreCharId} - {id}");
			}
			DomainManager.Character.AddRelation(context, info.CoreCharId, id, 16384);
			if (!RelationTypeHelper.AllowAddingAdoredRelation(id, info.CoreCharId))
			{
				throw new Exception($"Failed to add adored relation: {id} - {info.CoreCharId}");
			}
			DomainManager.Character.AddRelation(context, id, info.CoreCharId, 16384);
			if (!info.IsCoreCharInfertile && random.CheckPercentProb(20) && character.GetFertility() > 0)
			{
				info.CoreChar.LoseVirginity(context);
				character.LoseVirginity(context);
			}
		}
		else
		{
			if (!RelationTypeHelper.AllowAddingAdoredRelation(info.CoreCharId, id))
			{
				throw new Exception($"Failed to add adored relation: {info.CoreCharId} - {id}");
			}
			DomainManager.Character.AddRelation(context, info.CoreCharId, id, 16384);
		}
	}

	private static void CreateChildren(DataContext context, SettlementMembersCreationInfo info, bool isBloodChildren)
	{
		IRandomSource random = context.Random;
		int num;
		GameData.Domains.Character.Character character;
		int num2;
		GameData.Domains.Character.Character character2;
		short currAge;
		if (info.CoreChar.GetGender() == 1)
		{
			num = info.CoreCharId;
			character = info.CoreChar;
			num2 = info.SpouseCharId;
			character2 = info.SpouseChar;
			currAge = info.SpouseChar.GetCurrAge();
		}
		else
		{
			num = info.SpouseCharId;
			character = info.SpouseChar;
			num2 = info.CoreCharId;
			character2 = info.CoreChar;
			currAge = info.CoreChar.GetCurrAge();
		}
		sbyte childGrade = info.CoreMemberConfig.ChildGrade;
		short index = Config.Organization.Instance[info.OrgTemplateId].Members[childGrade];
		OrganizationMemberItem organizationMemberItem = OrganizationMember.Instance[index];
		sbyte gender = organizationMemberItem.Gender;
		int num3 = 17;
		int num4 = Math.Min(GlobalConfig.Instance.MaxAgeOfCreatingChar + 1, currAge);
		if (num3 > num4)
		{
			return;
		}
		int num5 = currAge - random.Next(num3, num4 + 1);
		int num6 = RedzenHelper.NormalDistribute(random, 1f, 1f, 1, 3);
		for (int i = 0; i < num6; i++)
		{
			int num7 = currAge - num5;
			if (num7 >= num3 && num5 >= 0)
			{
				sbyte gender2 = ((gender == -1) ? Gender.GetRandom(random) : gender);
				short characterTemplateId = GetCharacterTemplateId(info.OrgTemplateId, info.MapStateTemplateId, gender2);
				IntelligentCharacterCreationInfo intelligentCharacterCreationInfo = new IntelligentCharacterCreationInfo(orgInfo: new OrganizationInfo(info.OrgTemplateId, childGrade, principal: true, info.SettlementId), location: character2.GetLocation(), charTemplateId: characterTemplateId);
				intelligentCharacterCreationInfo.GrowingSectGrade = info.CoreMemberConfig.Grade;
				intelligentCharacterCreationInfo.Age = (short)num5;
				IntelligentCharacterCreationInfo info2 = intelligentCharacterCreationInfo;
				if (isBloodChildren)
				{
					info2.MotherCharId = num2;
					info2.Mother = character2;
					info2.FatherCharId = num;
					info2.ActualFatherCharId = num;
					info2.Father = character;
					info2.ActualFather = character;
				}
				GameData.Domains.Character.Character character3 = DomainManager.Character.CreateIntelligentCharacter(context, ref info2);
				int id = character3.GetId();
				info.CreatedCharIds.Add(id);
				int birthDate = character3.GetBirthDate();
				if (isBloodChildren)
				{
					if (!RelationTypeHelper.AllowAddingBloodParentRelation(id, num))
					{
						throw new Exception($"Failed to add blood parent relation: {id} - {num}");
					}
					if (!RelationTypeHelper.AllowAddingBloodParentRelation(id, num2))
					{
						throw new Exception($"Failed to add blood parent relation: {id} - {num2}");
					}
					DomainManager.Character.AddBloodParentRelations(context, id, num, birthDate);
					DomainManager.Character.AddBloodParentRelations(context, id, num2, birthDate);
				}
				else
				{
					if (!RelationTypeHelper.AllowAddingAdoptiveParentRelation(id, num))
					{
						throw new Exception($"Failed to add adoptive parent relation: {id} - {num}");
					}
					if (!RelationTypeHelper.AllowAddingAdoptiveParentRelation(id, num2))
					{
						throw new Exception($"Failed to add adoptive parent relation: {id} - {num2}");
					}
					DomainManager.Character.AddAdoptiveParentRelations(context, id, num, birthDate);
					DomainManager.Character.AddAdoptiveParentRelations(context, id, num2, birthDate);
				}
			}
			num5 -= 1 + random.Next(2);
		}
	}

	[DomainMethod]
	public SettlementDisplayData GetDisplayData(short settlementId)
	{
		Settlement settlement = _settlements[settlementId];
		SettlementDisplayData result = default(SettlementDisplayData);
		(int, int) populationInfo = settlement.GetPopulationInfo();
		result.SettlementId = settlementId;
		result.Culture = settlement.GetCulture();
		result.MaxCulture = settlement.GetMaxCulture();
		result.Safety = settlement.GetSafety();
		result.MaxSafety = settlement.GetMaxSafety();
		result.Population = populationInfo.Item1;
		result.MaxPopulation = populationInfo.Item2;
		result.RandomNameId = (short)((settlement is CivilianSettlement civilianSettlement) ? civilianSettlement.GetRandomNameId() : (-1));
		result.OrgTemplateId = settlement.GetOrgTemplateId();
		result.AreaTemplateId = DomainManager.Map.GetElement_Areas(settlement.GetLocation().AreaId).GetTemplateId();
		result.InfluencePowerUpdateDate = settlement.GetInfluencePowerUpdateDate();
		return result;
	}

	[DomainMethod]
	public List<SettlementNameRelatedData> GetSettlementNameRelatedData(List<short> settlementIds)
	{
		int count = settlementIds.Count;
		List<SettlementNameRelatedData> list = new List<SettlementNameRelatedData>(count);
		for (int i = 0; i < count; i++)
		{
			if (settlementIds[i] < 0)
			{
				list.Add(new SettlementNameRelatedData(-1, -1));
				continue;
			}
			Settlement settlement = _settlements[settlementIds[i]];
			list.Add(settlement.GetNameRelatedData());
		}
		return list;
	}

	[DomainMethod]
	public List<CharacterDisplayData> GetSettlementMembers(short settlementId)
	{
		Settlement settlement = _settlements[settlementId];
		List<int> list = new List<int>();
		settlement.GetMembers().GetAllMembers(list);
		return DomainManager.Character.GetCharacterDisplayDataList(list);
	}

	[DomainMethod]
	public OrganizationCombatSkillsDisplayData GetOrganizationCombatSkillsDisplayData(sbyte organizationTemplateId)
	{
		OrganizationCombatSkillsDisplayData organizationCombatSkillsDisplayData = new OrganizationCombatSkillsDisplayData();
		organizationCombatSkillsDisplayData.OrganizationTemplateId = organizationTemplateId;
		Settlement settlementByOrgTemplateId = GetSettlementByOrgTemplateId(organizationTemplateId);
		organizationCombatSkillsDisplayData.ApprovingRate = settlementByOrgTemplateId.CalcApprovingRate();
		organizationCombatSkillsDisplayData.ApprovingRateTotal = settlementByOrgTemplateId.CalcApprovingRateTotal();
		organizationCombatSkillsDisplayData.ApprovingRateUpperLimit = GetApprovingRateUpperLimit();
		organizationCombatSkillsDisplayData.ApprovingRateUpperLimitBonus = (short)(settlementByOrgTemplateId.GetApprovingRateUpperLimitBonus() + settlementByOrgTemplateId.GetApprovingRateUpperLimitTempBonus());
		List<short> learnedCombatSkills = DomainManager.Taiwu.GetTaiwu().GetLearnedCombatSkills();
		List<short> list = new List<short>();
		for (int i = 0; i < learnedCombatSkills.Count; i++)
		{
			CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[learnedCombatSkills[i]];
			if (combatSkillItem != null && combatSkillItem.SectId == organizationTemplateId)
			{
				list.Add(learnedCombatSkills[i]);
			}
		}
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		organizationCombatSkillsDisplayData.LearnedSkills = DomainManager.CombatSkill.GetCombatSkillDisplayData(taiwuCharId, list);
		return organizationCombatSkillsDisplayData;
	}

	[DomainMethod]
	public int[] GetSectPreparationForMartialArtTournament(sbyte orgTemplateId)
	{
		Sect sect = (Sect)GetSettlementByOrgTemplateId(orgTemplateId);
		return sect.GetMartialArtTournamentPreparations();
	}

	[DomainMethod]
	public short GetMartialArtTournamentCurrentHostSettlementId()
	{
		MartialArtTournamentMonthlyAction martialArtTournamentMonthlyAction = (MartialArtTournamentMonthlyAction)DomainManager.TaiwuEvent.GetMonthlyAction(new MonthlyActionKey(2, 0));
		return martialArtTournamentMonthlyAction.CurrentHost;
	}

	[DomainMethod]
	public short GetSettlementIdByAreaIdAndBlockId(short areaId, short blockId)
	{
		return DomainManager.Organization.GetSettlementByLocation(new Location(areaId, blockId)).GetId();
	}

	[DomainMethod]
	public ShortPair GetCultureByAreaIdAndBlockId(short areaId, short blockId)
	{
		short id = DomainManager.Organization.GetSettlementByLocation(new Location(areaId, blockId)).GetId();
		Settlement settlement = _settlements[id];
		return new ShortPair(settlement.GetCulture(), settlement.GetMaxCulture());
	}

	private void UpdateFactions(DataContext context, Settlement settlement)
	{
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		OrgMemberCollection members = settlement.GetMembers();
		List<GameData.Domains.Character.Character> list = new List<GameData.Domains.Character.Character>();
		List<GameData.Domains.Character.Character> list2 = new List<GameData.Domains.Character.Character>();
		for (sbyte b = 0; b < 9; b++)
		{
			OrganizationMemberItem orgMemberConfig = GetOrgMemberConfig(settlement.GetOrgTemplateId(), b);
			if (!orgMemberConfig.RestrictPrincipalAmount || orgMemberConfig.Amount >= 2)
			{
				list2.Clear();
				list.Clear();
				GameData.Domains.Character.Character character = null;
				int num = -1;
				HashSet<int> members2 = members.GetMembers(b);
				foreach (int item in members2)
				{
					GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
					if (!element_Objects.IsInteractableAsIntelligentCharacter())
					{
						continue;
					}
					int factionId = element_Objects.GetFactionId();
					if (factionId < 0)
					{
						list.Add(element_Objects);
					}
					else if (factionId == item)
					{
						list2.Add(element_Objects);
					}
					else
					{
						RelatedCharacter relation = DomainManager.Character.GetRelation(item, factionId);
						sbyte favorabilityType = FavorabilityType.GetFavorabilityType(relation.Favorability);
						sbyte factionLeaderPriorityType = FactionLeaderPriorityType.GetFactionLeaderPriorityType(relation.RelationType);
						sbyte b2 = JoinFactionFavorabilityReq[factionLeaderPriorityType];
						if (favorabilityType < b2)
						{
							LeaveFaction(context, element_Objects, charIsDead: false);
						}
					}
					SettlementCharacter settlementCharacter = DomainManager.Organization.GetSettlementCharacter(item);
					short influencePower = settlementCharacter.GetInfluencePower();
					if (influencePower > num)
					{
						num = influencePower;
						character = element_Objects;
					}
				}
				if (character != null && TryCreateFaction(context, character))
				{
					list.Remove(character);
					list2.Add(character);
				}
				foreach (GameData.Domains.Character.Character item2 in list)
				{
					(int factionId, bool succeed) tuple = OfflineJoinFaction(context, item2, list2);
					var (num2, _) = tuple;
					if (tuple.succeed)
					{
						lifeRecordCollection.AddJoinFaction(item2.GetId(), currDate, num2, item2.GetLocation());
						CharacterSet value = _factions[num2];
						item2.SetFactionId(num2, context);
						value.Add(item2.GetId());
						SetElement_Factions(num2, value, context);
					}
				}
			}
		}
	}

	public bool TryCreateFaction(DataContext context, GameData.Domains.Character.Character factionLeader)
	{
		int id = factionLeader.GetId();
		int factionId = factionLeader.GetFactionId();
		if (id == factionId)
		{
			return false;
		}
		if (!context.Random.CheckPercentProb(CreateFactionChance[factionLeader.GetBehaviorType()]))
		{
			return false;
		}
		if (factionId >= 0)
		{
			LeaveFaction(context, factionLeader, charIsDead: false);
		}
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		monthlyNotificationCollection.AddFactionUpgrade(id, factionLeader.GetOrganizationInfo().SettlementId);
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		lifeRecordCollection.AddCreateFaction(id, currDate, factionLeader.GetLocation());
		AddElement_Factions(id, default(CharacterSet), context);
		factionLeader.SetFactionId(id, context);
		return true;
	}

	public void LeaveFaction(DataContext context, GameData.Domains.Character.Character character, bool charIsDead)
	{
		int factionId = character.GetFactionId();
		int currDate = DomainManager.World.GetCurrDate();
		DomainManager.LifeRecord.GetLifeRecordCollection().AddLeaveFaction(character.GetId(), currDate, factionId, character.GetLocation());
		CharacterSet value = _factions[factionId];
		value.Remove(character.GetId());
		if (!charIsDead)
		{
			character.SetFactionId(-1, context);
		}
		SetElement_Factions(factionId, value, context);
	}

	public void RemoveFaction(DataContext context, GameData.Domains.Character.Character factionLeader, bool leaderIsDead)
	{
		int id = factionLeader.GetId();
		if (!leaderIsDead)
		{
			factionLeader.SetFactionId(-1, context);
		}
		foreach (int item in _factions[id].GetCollection())
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			element_Objects.SetFactionId(-1, context);
		}
		RemoveElement_Factions(id, context);
	}

	public void ExpandAllFactions(DataContext context)
	{
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		foreach (int key in _factions.Keys)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(key);
			if (!element_Objects.IsInteractableAsIntelligentCharacter())
			{
				continue;
			}
			var (num, flag) = OfflineExpandFaction(context, element_Objects);
			if (num >= 0)
			{
				if (flag)
				{
					lifeRecordCollection.AddFactionRecruitSucceed(key, currDate, num, element_Objects.GetLocation());
					CharacterSet value = _factions[key];
					value.Add(num);
					DomainManager.Character.GetElement_Objects(num).SetFactionId(key, context);
					SetElement_Factions(key, value, context);
				}
				else
				{
					lifeRecordCollection.AddFactionRecruitFail(key, currDate, num, element_Objects.GetLocation());
					GameData.Domains.Character.Ai.PersonalNeed personalNeed = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(19, num);
					element_Objects.AddPersonalNeed(context, personalNeed);
				}
			}
		}
	}

	public (int newMemberId, bool succeed) OfflineExpandFaction(DataContext context, GameData.Domains.Character.Character factionLeader)
	{
		int id = factionLeader.GetId();
		OrganizationInfo organizationInfo = factionLeader.GetOrganizationInfo();
		Sect sect = _sects[organizationInfo.SettlementId];
		OrgMemberCollection members = sect.GetMembers();
		HashSet<int> members2 = members.GetMembers(organizationInfo.Grade);
		if (members2.Count == 0)
		{
			return (newMemberId: -1, succeed: false);
		}
		if (!context.Random.CheckPercentProb(ExpandFactionChance[factionLeader.GetBehaviorType()]))
		{
			return (newMemberId: -1, succeed: false);
		}
		int num = int.MaxValue;
		int num2 = int.MinValue;
		int num3 = -1;
		foreach (int item in members2)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			if (element_Objects.GetFactionId() >= 0 || element_Objects.GetAgeGroup() == 0)
			{
				continue;
			}
			sbyte behaviorType = element_Objects.GetBehaviorType();
			RelatedCharacter relation = DomainManager.Character.GetRelation(item, id);
			ushort relationType = relation.RelationType;
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(relation.Favorability);
			sbyte factionLeaderPriorityType = FactionLeaderPriorityType.GetFactionLeaderPriorityType(relationType);
			if (factionLeaderPriorityType == -1)
			{
				continue;
			}
			sbyte b = JoinFactionFavorabilityReq[factionLeaderPriorityType];
			if (favorabilityType < b)
			{
				continue;
			}
			sbyte b2 = JoinFactionPriorities[behaviorType][factionLeaderPriorityType];
			if (b2 <= num)
			{
				int num4 = JoinFactionChance[behaviorType] + JoinFactionFavorabilityBonus[behaviorType] * (favorabilityType - b);
				if (num4 != 0 && (b2 < num || num2 < num4))
				{
					num = b2;
					num2 = num4;
					num3 = item;
				}
			}
		}
		if (num3 >= 0)
		{
			return (newMemberId: num3, succeed: context.Random.CheckPercentProb(num2));
		}
		return (newMemberId: -1, succeed: false);
	}

	public (int factionId, bool succeed) OfflineJoinFaction(DataContext context, GameData.Domains.Character.Character character, List<GameData.Domains.Character.Character> factionLeaders)
	{
		int id = character.GetId();
		sbyte behaviorType = character.GetBehaviorType();
		int num = int.MaxValue;
		int num2 = int.MinValue;
		int num3 = -1;
		foreach (GameData.Domains.Character.Character factionLeader in factionLeaders)
		{
			int id2 = factionLeader.GetId();
			RelatedCharacter relation = DomainManager.Character.GetRelation(id, id2);
			ushort relationType = relation.RelationType;
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(relation.Favorability);
			sbyte factionLeaderPriorityType = FactionLeaderPriorityType.GetFactionLeaderPriorityType(relationType);
			if (factionLeaderPriorityType == -1)
			{
				continue;
			}
			sbyte b = JoinFactionFavorabilityReq[factionLeaderPriorityType];
			if (favorabilityType < b)
			{
				continue;
			}
			sbyte b2 = JoinFactionPriorities[behaviorType][factionLeaderPriorityType];
			if (b2 <= num)
			{
				int num4 = JoinFactionChance[behaviorType] + JoinFactionFavorabilityBonus[behaviorType] * (favorabilityType - b);
				if (num4 != 0 && (b2 < num || num2 < num4))
				{
					num = b2;
					num2 = num4;
					num3 = id2;
				}
			}
		}
		if (num3 >= 0)
		{
			return (factionId: num3, succeed: context.Random.CheckPercentProb(num2));
		}
		return (factionId: -1, succeed: false);
	}

	public void Test_CheckFactions()
	{
		int num = 0;
		foreach (KeyValuePair<int, CharacterSet> faction in _factions)
		{
			faction.Deconstruct(out var key, out var value);
			int num2 = key;
			CharacterSet characterSet = value;
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(num2);
			if (AgeGroup.GetAgeGroup(element_Objects.GetCurrAge()) == 0)
			{
				throw new Exception($"(Test) Faction leader {num2} cannot be a baby");
			}
			OrganizationInfo organizationInfo = element_Objects.GetOrganizationInfo();
			if (!IsSect(organizationInfo.OrgTemplateId))
			{
				throw new Exception($"(Test) Faction {num2} appeared in non-sect organization {Config.Organization.Instance[organizationInfo.OrgTemplateId].Name}");
			}
			foreach (int item in characterSet.GetCollection())
			{
				GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(item);
				if (element_Objects2.GetAgeGroup() == 0)
				{
					throw new Exception($"(Test) Faction member {item} cannot be a baby.");
				}
				OrganizationInfo organizationInfo2 = element_Objects2.GetOrganizationInfo();
				if (organizationInfo2.OrgTemplateId != organizationInfo.OrgTemplateId)
				{
					throw new Exception($"(Test) Faction member {item} is in different faction from the leader {num2}");
				}
				if (organizationInfo2.Grade != organizationInfo.Grade)
				{
					throw new Exception($"(Test) Faction member {item} is in different grade from the leader {num2}");
				}
			}
			num += characterSet.GetCount();
		}
		Logger.Info($"Faction Total Count: {_factions.Count}   Faction Member Total Count: {num}");
	}

	[DomainMethod]
	public void GmCmd_SetAllSettlementInformationVisited(DataContext context)
	{
		TaiwuDomain taiwu = DomainManager.Taiwu;
		HashSet<short> hashSet = taiwu.GetVisitedSettlements().ToHashSet();
		foreach (short key in _sects.Keys)
		{
			hashSet.Add(key);
		}
		foreach (short key2 in _civilianSettlements.Keys)
		{
			hashSet.Add(key2);
		}
		taiwu.SetVisitedSettlements(hashSet.ToList(), context);
	}

	[DomainMethod]
	public List<CharacterDisplayData> GmCmd_GetSettlementPrisoner(DataContext context, int prisonType)
	{
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		if (!location.IsValid())
		{
			return null;
		}
		MapBlockData blockData = DomainManager.Map.GetBlockData(location.AreaId, location.BlockId);
		if (blockData == null)
		{
			return null;
		}
		Settlement settlementByLocation = GetSettlementByLocation(blockData.GetRootBlock().GetLocation());
		if (!(settlementByLocation is Sect sect))
		{
			return null;
		}
		if (sect == null)
		{
			return null;
		}
		return (PrisonType)prisonType switch
		{
			PrisonType.Low => DomainManager.Character.GetCharacterDisplayDataList((from x in sect.Prison.GetPrisonLow()
				select x.CharId).ToList()), 
			PrisonType.Mid => DomainManager.Character.GetCharacterDisplayDataList((from x in sect.Prison.GetPrisonMid()
				select x.CharId).ToList()), 
			PrisonType.High => DomainManager.Character.GetCharacterDisplayDataList((from x in sect.Prison.GetPrisonHigh()
				select x.CharId).ToList()), 
			PrisonType.Infected => DomainManager.Character.GetCharacterDisplayDataList((from x in sect.Prison.GetPrisonInfected()
				select x.CharId).ToList()), 
			_ => null, 
		};
	}

	[DomainMethod]
	public List<List<CharacterDisplayData>> GmCmd_GetAllFactionMembers()
	{
		CharacterDomain character = DomainManager.Character;
		List<int> list = new List<int>();
		List<List<CharacterDisplayData>> list2 = new List<List<CharacterDisplayData>>();
		foreach (KeyValuePair<int, CharacterSet> faction in _factions)
		{
			list.Clear();
			list.Add(faction.Key);
			list.AddRange(faction.Value.GetCollection());
			list2.Add(character.GetCharacterDisplayDataList(list));
		}
		return list2;
	}

	[DomainMethod]
	public void GmCmd_ForceUpdateTreasuryGuards(DataContext context, short settlementId)
	{
		Settlement settlement = GetSettlement(settlementId);
		settlement.ForceUpdateTreasuryGuards(context);
	}

	[DomainMethod]
	public void GmCmd_ForceUpdateInfluencePower(DataContext context, short settlementId)
	{
		Settlement settlement = GetSettlement(settlementId);
		Dictionary<int, (GameData.Domains.Character.Character, short)> baseInfluencePowers = new Dictionary<int, (GameData.Domains.Character.Character, short)>();
		HashSet<int> relatedCharIds = new HashSet<int>();
		settlement.UpdateInfluencePowers(context, baseInfluencePowers, relatedCharIds);
	}

	[DomainMethod]
	public void AddSectBounty(DataContext context, sbyte orgTemplateId, int charId, sbyte punishmentSeverity, short punishmentType, int duration)
	{
		if (!DomainManager.Character.TryGetElement_Objects(charId, out var element) || !IsSect(orgTemplateId))
		{
			return;
		}
		Sect sect = (Sect)GetSettlementByOrgTemplateId(orgTemplateId);
		sect.AddBounty(context, element, punishmentSeverity, punishmentType, duration);
		if (punishmentType != 42)
		{
			return;
		}
		Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(orgTemplateId);
		List<int> list = new List<int>();
		settlementByOrgTemplateId.GetMembers().GetAllMembers(list);
		using List<int>.Enumerator enumerator = list.GetEnumerator();
		if (enumerator.MoveNext())
		{
			int current = enumerator.Current;
			DomainManager.Character.AddRelation(context, charId, current, 32768);
		}
	}

	[DomainMethod]
	public void AddSectPrisoner(DataContext context, sbyte orgTemplateId, int charId, sbyte punishmentSeverity, short punishmentType, int duration)
	{
		if (DomainManager.Character.TryGetElement_Objects(charId, out var element) && IsSect(orgTemplateId))
		{
			Sect sect = (Sect)GetSettlementByOrgTemplateId(orgTemplateId);
			sect.AddPrisoner(context, element, punishmentSeverity, punishmentType);
		}
	}

	private void RecordSettlementStandardPopulations(DataContext context)
	{
		foreach (KeyValuePair<short, Settlement> settlement2 in _settlements)
		{
			settlement2.Deconstruct(out var _, out var value);
			Settlement settlement = value;
			OrgMemberCollection members = settlement.GetMembers();
			int count = members.GetCount();
			settlement.SetStandardOnStagePopulation(count, context);
		}
	}

	public void ChangeSettlementStandardPopulations(DataContext context, byte oriWorldPopulationType)
	{
		int worldPopulationFactor = WorldDomain.GetWorldPopulationFactor(oriWorldPopulationType);
		int worldPopulationFactor2 = DomainManager.World.GetWorldPopulationFactor();
		foreach (KeyValuePair<short, Settlement> settlement2 in _settlements)
		{
			settlement2.Deconstruct(out var _, out var value);
			Settlement settlement = value;
			int standardOnStagePopulation = settlement.GetStandardOnStagePopulation();
			int num = standardOnStagePopulation * 100 / worldPopulationFactor;
			int standardOnStagePopulation2 = num * worldPopulationFactor2 / 100;
			settlement.SetStandardOnStagePopulation(standardOnStagePopulation2, context);
		}
	}

	private void InitializePrisonCache()
	{
		_sectFugitives.Clear();
		_sectPrisoners.Clear();
		foreach (Sect value in _sects.Values)
		{
			sbyte orgTemplateId = value.GetOrgTemplateId();
			SettlementPrison prison = value.Prison;
			foreach (SettlementBounty bounty in prison.Bounties)
			{
				RegisterSectFugitive(bounty.CharId, orgTemplateId);
			}
			foreach (SettlementPrisoner prisoner in prison.Prisoners)
			{
				RegisterSectPrisoner(prisoner.CharId, orgTemplateId);
			}
		}
	}

	internal void RegisterSectFugitive(int charId, sbyte orgTemplateId)
	{
		if (!_sectFugitives.TryGetValue(charId, out var value))
		{
			value = new List<sbyte>(1);
			_sectFugitives.Add(charId, value);
		}
		else if (value.Contains(orgTemplateId))
		{
			return;
		}
		value.Add(orgTemplateId);
	}

	internal void UnregisterSectFugitive(int charId, sbyte orgTemplateId)
	{
		if (_sectFugitives.TryGetValue(charId, out var value))
		{
			value.Remove(orgTemplateId);
			if (value.Count == 0)
			{
				_sectFugitives.Remove(charId);
			}
		}
	}

	public sbyte GetFugitiveBountySect(int charId)
	{
		if (_sectFugitives.TryGetValue(charId, out var value))
		{
			return value[0];
		}
		return -1;
	}

	public IEnumerable<sbyte> GetFugitiveBountySects(int charId)
	{
		if (!_sectFugitives.TryGetValue(charId, out var sects))
		{
			yield break;
		}
		foreach (sbyte item in sects)
		{
			yield return item;
		}
	}

	public List<sbyte> GetTaiwuFugitiveBountySect()
	{
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		if (_sectFugitives.TryGetValue(taiwuCharId, out var value))
		{
			return value;
		}
		return null;
	}

	public bool IsSectFugitive(int charId, sbyte orgTemplateId)
	{
		List<sbyte> value;
		return _sectFugitives.TryGetValue(charId, out value) && value.Contains(orgTemplateId);
	}

	public SettlementBounty GetBounty(int charId, out sbyte sectOrgTemplateId)
	{
		sectOrgTemplateId = GetFugitiveBountySect(charId);
		if (sectOrgTemplateId < 0)
		{
			return null;
		}
		Sect sect = (Sect)GetSettlementByOrgTemplateId(sectOrgTemplateId);
		return sect.Prison.GetBounty(charId);
	}

	public bool TryRemoveBounty(DataContext context, int charId)
	{
		Tester.Assert(charId != DomainManager.Taiwu.GetTaiwuCharId(), "使用TryRemoveTaiwuBounty移除太吾的悬赏");
		sbyte fugitiveBountySect = DomainManager.Organization.GetFugitiveBountySect(charId);
		if (fugitiveBountySect < 0)
		{
			return false;
		}
		short settlementIdByOrgTemplateId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(fugitiveBountySect);
		Sect sect = _sects[settlementIdByOrgTemplateId];
		return sect.RemoveBounty(context, charId);
	}

	public void TryRemoveTaiwuBounty(DataContext context)
	{
		List<sbyte> taiwuFugitiveBountySect = DomainManager.Organization.GetTaiwuFugitiveBountySect();
		if (taiwuFugitiveBountySect != null)
		{
			for (int i = 0; i < taiwuFugitiveBountySect.Count; i++)
			{
				sbyte orgTemplateId = taiwuFugitiveBountySect[i];
				short settlementIdByOrgTemplateId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(orgTemplateId);
				Sect sect = _sects[settlementIdByOrgTemplateId];
				sect.RemoveBounty(context, DomainManager.Taiwu.GetTaiwuCharId());
			}
		}
	}

	public void TryRemoveTaiwuGroupBountyAndPunishment(DataContext context)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int id = taiwu.GetId();
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
		List<short> featureIds = taiwu.GetFeatureIds();
		foreach (int item in collection)
		{
			if (item == id)
			{
				continue;
			}
			sbyte fugitiveBountySect = DomainManager.Organization.GetFugitiveBountySect(item);
			if (fugitiveBountySect >= 0)
			{
				short settlementIdByOrgTemplateId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(fugitiveBountySect);
				Sect sect = _sects[settlementIdByOrgTemplateId];
				if (sect.RemoveBounty(context, item))
				{
					instantNotificationCollection.AddSectPunishmentWarrantRelieved(item, settlementIdByOrgTemplateId);
				}
			}
		}
		List<int> list = new List<int>();
		if (_sectFugitives.TryGetValue(id, out var value))
		{
			foreach (sbyte item2 in value)
			{
				list.Add(item2);
			}
		}
		foreach (int item3 in list)
		{
			short settlementIdByOrgTemplateId2 = DomainManager.Organization.GetSettlementIdByOrgTemplateId((sbyte)item3);
			Sect sect2 = _sects[settlementIdByOrgTemplateId2];
			if (sect2.RemoveBounty(context, id))
			{
				instantNotificationCollection.AddSectPunishmentWarrantRelieved(id, settlementIdByOrgTemplateId2);
			}
		}
		list.Clear();
		for (sbyte b = 1; b <= 15; b++)
		{
			List<short> taiwuPunishementFeature = Config.Organization.Instance[b].TaiwuPunishementFeature;
			if (taiwuPunishementFeature != null)
			{
				foreach (short item4 in taiwuPunishementFeature)
				{
					if (featureIds.Contains(item4))
					{
						short settlementIdByOrgTemplateId3 = DomainManager.Organization.GetSettlementIdByOrgTemplateId(b);
						list.Add(item4);
						instantNotificationCollection.AddSectPunishmentCharacterFeatureRelieved(id, settlementIdByOrgTemplateId3);
						break;
					}
				}
			}
		}
		foreach (int item5 in list)
		{
			taiwu.RemoveFeature(context, (short)item5);
			DomainManager.Extra.UnregisterCharacterTemporaryFeature(context, id, (short)item5);
		}
	}

	internal void RegisterSectPrisoner(int charId, sbyte orgTemplateId)
	{
		if (!_sectPrisoners.TryAdd(charId, orgTemplateId))
		{
			Logger.AppendWarning($"character {charId} is imprisoned by multiple sects.");
		}
	}

	internal void UnregisterSectPrisoner(int charId)
	{
		_sectPrisoners.Remove(charId);
	}

	public sbyte GetPrisonerSect(int charId)
	{
		return _sectPrisoners.GetValueOrDefault<int, sbyte>(charId, -1);
	}

	[Obsolete]
	public void SetSettlementPrisonGuardCharId(int charId)
	{
		_prisonGuardCharId = charId;
	}

	[DomainMethod]
	public SettlementPrisonDisplayData GetSettlementPrisonDisplayData(DataContext context, short settlementId)
	{
		Sect sect = GetSettlement(settlementId) as Sect;
		SettlementPrison prison = sect.Prison;
		CharacterDisplayData[] array = (from prisonGuardCharId in sect.Treasuries.GetGuardIds()
			select DomainManager.Character.GetCharacterDisplayData(prisonGuardCharId)).ToArray();
		SettlementPrisonDisplayData settlementPrisonDisplayData = new SettlementPrisonDisplayData
		{
			OrgTemplateId = sect.GetOrgTemplateId(),
			DebtOrSupport = sect.CalcApprovingRate(),
			GuardianCharacterDisplayDataLow = sect.GetGuardsDisplayData(context, 0),
			GuardianCharacterDisplayDataMid = sect.GetGuardsDisplayData(context, 1),
			GuardianCharacterDisplayDataHigh = sect.GetGuardsDisplayData(context, 2),
			PrisonerCharacterDisplayDataDict = new Dictionary<int, CharacterDisplayDataForSettlementPrisoner>()
		};
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		foreach (SettlementPrisoner prisoner in prison.Prisoners)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(prisoner.CharId);
			CharacterDisplayDataForSettlementPrisoner characterDisplayDataForSettlementPrisoner = new CharacterDisplayDataForSettlementPrisoner();
			characterDisplayDataForSettlementPrisoner.Resistance = sect.CalcKidnappedCharacterResistance(prisoner);
			characterDisplayDataForSettlementPrisoner.EscapeRate = prisoner.CalcEscapeRate(characterDisplayDataForSettlementPrisoner.Resistance, 0);
			characterDisplayDataForSettlementPrisoner.SettlementPrisoner = new SettlementPrisoner(prisoner);
			characterDisplayDataForSettlementPrisoner.CurrAge = element_Objects.GetCurrAge();
			characterDisplayDataForSettlementPrisoner.AvatarRelatedData = element_Objects.GenerateAvatarRelatedData();
			characterDisplayDataForSettlementPrisoner.NameRelatedData = DomainManager.Character.GetNameRelatedData(prisoner.CharId);
			characterDisplayDataForSettlementPrisoner.Gender = element_Objects.GetGender();
			characterDisplayDataForSettlementPrisoner.Health = element_Objects.GetHealth();
			characterDisplayDataForSettlementPrisoner.LeftMaxHealth = element_Objects.GetLeftMaxHealth();
			characterDisplayDataForSettlementPrisoner.OrgInfo = element_Objects.GetOrganizationInfo();
			characterDisplayDataForSettlementPrisoner.CompletelyInfected = element_Objects.IsCompletelyInfected();
			characterDisplayDataForSettlementPrisoner.Happiness = element_Objects.GetHappiness();
			characterDisplayDataForSettlementPrisoner.FavorabilityToTaiwu = DomainManager.Character.GetFavorability(prisoner.CharId, taiwuCharId);
			characterDisplayDataForSettlementPrisoner.RandomNameId = (short)((characterDisplayDataForSettlementPrisoner.OrgInfo.SettlementId >= 0) ? DomainManager.Organization.GetSettlement(characterDisplayDataForSettlementPrisoner.OrgInfo.SettlementId).GetNameRelatedData().RandomNameId : (-1));
			settlementPrisonDisplayData.PrisonerCharacterDisplayDataDict[prisoner.CharId] = characterDisplayDataForSettlementPrisoner;
		}
		return settlementPrisonDisplayData;
	}

	[DomainMethod]
	public SettlementBountyDisplayData GetSettlementBountyDisplayData(short settlementId)
	{
		SettlementBountyDisplayData settlementBountyDisplayData = new SettlementBountyDisplayData();
		Sect sect = (Sect)GetSettlement(settlementId);
		_calculatedBountiesCache.Clear();
		sect.GetEnemyRelationBounties(_calculatedBountiesCache);
		sect.GetEnemySectBounties(_calculatedBountiesCache);
		sect.GetXiangshuInfectedBounties(_calculatedBountiesCache);
		SettlementPrison prison = sect.Prison;
		settlementBountyDisplayData.BountyCharacterDisplayDataDict = new Dictionary<int, CharacterDisplayDataForSettlementBounty>();
		settlementBountyDisplayData.OrgTemplateId = sect.GetOrgTemplateId();
		GetBountyCharacterDisplayDataFromList(settlementBountyDisplayData, prison.Bounties);
		GetBountyCharacterDisplayDataFromList(settlementBountyDisplayData, _calculatedBountiesCache);
		return settlementBountyDisplayData;
	}

	internal void FillBountyCharacterDisplayDataFromInfo(CharacterDisplayDataForSettlementBounty charDisplayData, GameData.Domains.Character.Character character, SettlementBounty bounty)
	{
		charDisplayData.CurrAge = character.GetCurrAge();
		charDisplayData.AvatarRelatedData = character.GenerateAvatarRelatedData();
		charDisplayData.NameRelatedData = DomainManager.Character.GetNameRelatedData(character.GetId());
		charDisplayData.Gender = character.GetGender();
		charDisplayData.Health = character.GetHealth();
		charDisplayData.LeftMaxHealth = character.GetLeftMaxHealth();
		charDisplayData.OrgInfo = character.GetOrganizationInfo();
		charDisplayData.FullBlockName = DomainManager.Map.GetBlockFullName(character.GetLocation());
		charDisplayData.RandomNameId = (short)((charDisplayData.OrgInfo.SettlementId >= 0) ? DomainManager.Organization.GetSettlement(charDisplayData.OrgInfo.SettlementId).GetNameRelatedData().RandomNameId : (-1));
		if (bounty != null)
		{
			charDisplayData.SettlementBounty = new SettlementBounty(bounty);
			charDisplayData.HunterState = GetHunterState(bounty, character);
		}
	}

	private void GetBountyCharacterDisplayDataFromList(SettlementBountyDisplayData data, List<SettlementBounty> source)
	{
		foreach (SettlementBounty item in source)
		{
			if (!data.BountyCharacterDisplayDataDict.ContainsKey(item.CharId))
			{
				CharacterDisplayDataForSettlementBounty characterDisplayDataForSettlementBounty = new CharacterDisplayDataForSettlementBounty();
				FillBountyCharacterDisplayDataFromInfo(characterDisplayDataForSettlementBounty, DomainManager.Character.GetElement_Objects(item.CharId), item);
				data.BountyCharacterDisplayDataDict[item.CharId] = characterDisplayDataForSettlementBounty;
			}
		}
	}

	[DomainMethod]
	public SettlementBountyDisplayData GetBountyCharacterDisplayDataFromCharacterList(List<int> characterIds)
	{
		SettlementBountyDisplayData settlementBountyDisplayData = new SettlementBountyDisplayData
		{
			BountyCharacterDisplayDataDict = new Dictionary<int, CharacterDisplayDataForSettlementBounty>(),
			OrgTemplateId = -1
		};
		OrganizationDomain organization = DomainManager.Organization;
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		List<int> list = new List<int>();
		if (characterIds != null)
		{
			list.AddRange(characterIds);
		}
		for (int i = 0; i < list.Count; i++)
		{
			int num = list[i];
			if (!DomainManager.Character.TryGetElement_Objects(num, out var element))
			{
				continue;
			}
			if (num == taiwuCharId)
			{
				int num2 = -1;
				foreach (OrganizationItem item in (IEnumerable<OrganizationItem>)Config.Organization.Instance)
				{
					if (!item.IsSect || !organization.IsSectFugitive(num, item.TemplateId) || !(organization.GetSettlementByOrgTemplateId(item.TemplateId) is Sect sect))
					{
						continue;
					}
					SettlementBounty bounty = sect.Prison.GetBounty(num);
					if (bounty != null)
					{
						CharacterDisplayDataForSettlementBounty characterDisplayDataForSettlementBounty = new CharacterDisplayDataForSettlementBounty();
						FillBountyCharacterDisplayDataFromInfo(characterDisplayDataForSettlementBounty, element, bounty);
						characterDisplayDataForSettlementBounty.OrgInfo.OrgTemplateId = item.TemplateId;
						if (!list.Contains(bounty.CurrentHunterId))
						{
							list.Add(bounty.CurrentHunterId);
						}
						settlementBountyDisplayData.BountyCharacterDisplayDataDict[num2] = characterDisplayDataForSettlementBounty;
						num2--;
					}
				}
			}
			else
			{
				CharacterDisplayDataForSettlementBounty characterDisplayDataForSettlementBounty2 = new CharacterDisplayDataForSettlementBounty();
				sbyte sectOrgTemplateId;
				SettlementBounty bounty2 = GetBounty(num, out sectOrgTemplateId);
				FillBountyCharacterDisplayDataFromInfo(characterDisplayDataForSettlementBounty2, element, bounty2);
				characterDisplayDataForSettlementBounty2.OrgInfo.OrgTemplateId = sectOrgTemplateId;
				if (bounty2 != null && !list.Contains(bounty2.CurrentHunterId))
				{
					list.Add(bounty2.CurrentHunterId);
				}
				settlementBountyDisplayData.BountyCharacterDisplayDataDict[num] = characterDisplayDataForSettlementBounty2;
			}
		}
		return settlementBountyDisplayData;
	}

	[DomainMethod]
	public SettlementPrisonRecordCollection GetSettlementPrisonRecordCollection(DataContext context, short settlementId)
	{
		return DomainManager.Extra.GetSettlementPrisonRecordCollection(context, settlementId);
	}

	public bool IsCharacterSectFugitive(int charId, sbyte orgTemplateId)
	{
		sbyte fugitiveBountySect = DomainManager.Organization.GetFugitiveBountySect(charId);
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		return fugitiveBountySect == orgTemplateId || element_Objects.IsCompletelyInfected();
	}

	public void SetSettlementPrisonRecordCollection(DataContext context, short settlementId, SettlementPrisonRecordCollection collection)
	{
		DomainManager.Extra.SetSettlementPrisonRecordCollection(context, settlementId, collection);
	}

	private sbyte GetHunterState(SettlementBounty bounty, GameData.Domains.Character.Character character)
	{
		if (bounty.CurrentHunterId < 0)
		{
			return (sbyte)((bounty.RequiredConsummateLevel >= 0) ? 2 : 0);
		}
		return (sbyte)((character.GetKidnapperId() != bounty.CurrentHunterId) ? 1 : 3);
	}

	public (CharacterSet changeFavorCharIds, CharacterSet becomeEnemyCharIds, CharacterSet approveCharIds, CharacterSet disapproveCharIds) ApplySettlementPrisonEventEffect(DataContext context, SettlementPrisonEventEffectItem effectCfg, short settlementId)
	{
		Settlement settlement = GetSettlement(settlementId);
		if (!(settlement is Sect sect))
		{
			throw new Exception($"settlement {settlement} is not Sect");
		}
		SettlementLayeredTreasuries treasuries = settlement.Treasuries;
		OrgMemberCollection members = settlement.GetMembers();
		if (effectCfg.TaiwuBounty >= 0)
		{
			PunishmentTypeItem punishmentTypeItem = PunishmentType.Instance[effectCfg.TaiwuBounty];
			if (punishmentTypeItem != null)
			{
				sect.AddBounty(context, DomainManager.Taiwu.GetTaiwu(), punishmentTypeItem.Severity, effectCfg.TaiwuBounty);
			}
		}
		if (effectCfg.AlterTime > 0)
		{
			sect.SetAlterTime(context, (byte)effectCfg.AlterTime);
		}
		HashSet<int> hashSet = ObjectPool<HashSet<int>>.Instance.Get();
		hashSet.Clear();
		treasuries.GetGuardIds(hashSet);
		SettlementTreasuryEventEffectHelper.EffectArgs args = new SettlementTreasuryEventEffectHelper.EffectArgs(effectCfg, hashSet.Count, isGuard: true);
		ApplySettlementTreasuryEventEffect(context, hashSet, ref args);
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		for (sbyte b = 0; b <= 8; b++)
		{
			IEnumerable<int> enumerable = DomainManager.Character.ExcludeInfant(members.GetMembers(b));
			foreach (int item in enumerable)
			{
				if (!hashSet.Contains(item))
				{
					list.Add(item);
				}
			}
		}
		CollectionUtils.Shuffle(context.Random, list);
		SettlementTreasuryEventEffectHelper.EffectArgs effectArgs = new SettlementTreasuryEventEffectHelper.EffectArgs(effectCfg, list.Count, isGuard: false);
		effectArgs.ChangeFavorCharIds = args.ChangeFavorCharIds;
		effectArgs.ApproveCharIds = args.ApproveCharIds;
		effectArgs.DisapproveCharIds = args.DisapproveCharIds;
		effectArgs.BecomeEnemyCharIds = args.BecomeEnemyCharIds;
		SettlementTreasuryEventEffectHelper.EffectArgs args2 = effectArgs;
		ApplySettlementTreasuryEventEffect(context, list, ref args2);
		ObjectPool<HashSet<int>>.Instance.Return(hashSet);
		ObjectPool<List<int>>.Instance.Return(list);
		return (changeFavorCharIds: args2.ChangeFavorCharIds, becomeEnemyCharIds: args2.BecomeEnemyCharIds, approveCharIds: args2.ApproveCharIds, disapproveCharIds: args2.DisapproveCharIds);
	}

	[DomainMethod]
	public bool IsTaiwuSectFugitive(sbyte orgTemplateId)
	{
		return GetTaiwuFugitiveBountySect()?.Contains(orgTemplateId) ?? false;
	}

	public sbyte GetSectFavorability(sbyte orgTemplateId, sbyte relatedOrgTemplateId)
	{
		sbyte largeSectIndex = GetLargeSectIndex(orgTemplateId);
		sbyte largeSectIndex2 = GetLargeSectIndex(relatedOrgTemplateId);
		if (largeSectIndex >= 0 && largeSectIndex2 >= 0)
		{
			return Config.Organization.Instance[orgTemplateId].LargeSectFavorabilities[largeSectIndex2];
		}
		return 0;
	}

	public void GetSectTemplateIdsByFavorability(sbyte orgTemplateId, sbyte sectFavorability, ref SpanList<sbyte> result)
	{
		for (sbyte b = 0; b < 15; b++)
		{
			sbyte largeSectTemplateId = GetLargeSectTemplateId(b);
			if (GetSectFavorability(orgTemplateId, largeSectTemplateId) == sectFavorability)
			{
				result.Add(largeSectTemplateId);
			}
		}
	}

	[Obsolete]
	public void SetSectFavorability(DataContext context, sbyte orgTemplateId, sbyte relatedOrgTemplateId, sbyte favorability)
	{
		sbyte largeSectIndex = GetLargeSectIndex(orgTemplateId);
		sbyte largeSectIndex2 = GetLargeSectIndex(relatedOrgTemplateId);
		if (largeSectIndex >= 0 && largeSectIndex2 >= 0)
		{
			SetLargeSectFavorability(context, largeSectIndex, largeSectIndex2, favorability);
			return;
		}
		throw new Exception($"Not support favorability of small sects: {orgTemplateId}, {relatedOrgTemplateId}");
	}

	public unsafe void OfflineInitializeLargeSectFavorabilities(sbyte largeSectIndex, sbyte[] sectFavorabilities)
	{
		uint num = 0u;
		for (int i = 0; i < 15; i++)
		{
			uint num2 = (uint)sectFavorabilities[i];
			num |= num2 << i * 2;
		}
		fixed (sbyte* largeSectFavorabilities = _largeSectFavorabilities)
		{
			sbyte* ptr = largeSectFavorabilities + largeSectIndex * 4;
			*(uint*)ptr = num;
		}
	}

	[Obsolete]
	private sbyte GetLargeSectFavorability(sbyte largeSectIndex, sbyte relatedLargeSectIndex)
	{
		int num = largeSectIndex * 4 + relatedLargeSectIndex / 4;
		uint num2 = (uint)_largeSectFavorabilities[num];
		int num3 = relatedLargeSectIndex % 4 * 2;
		return (sbyte)((num2 >> num3) & 3);
	}

	[Obsolete]
	private void SetLargeSectFavorability(DataContext context, sbyte largeSectIndex, sbyte relatedLargeSectIndex, sbyte favorability)
	{
		int num = largeSectIndex * 4 + relatedLargeSectIndex / 4;
		uint num2 = (uint)_largeSectFavorabilities[num];
		int num3 = relatedLargeSectIndex % 4 * 2;
		num2 &= (uint)(~(3 << num3));
		num2 |= (uint)(favorability << num3);
		_largeSectFavorabilities[num] = (sbyte)num2;
		SetLargeSectFavorabilities(_largeSectFavorabilities, context);
	}

	public static sbyte GetRandomSectOrgTemplateId(IRandomSource random, sbyte gender = -1)
	{
		if (1 == 0)
		{
		}
		sbyte result = gender switch
		{
			-1 => _allSectOrgTemplateIds[random.Next(_allSectOrgTemplateIds.Length)], 
			0 => _femaleSectOrgTemplateIds[random.Next(_femaleSectOrgTemplateIds.Length)], 
			1 => _maleSectOrgTemplateIds[random.Next(_maleSectOrgTemplateIds.Length)], 
			_ => throw new Exception($"Unsupported gender {gender}"), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public static short GetRandomOrgMemberClothing(IRandomSource random, OrganizationMemberItem orgMemberConfig)
	{
		PresetEquipmentItem clothing = orgMemberConfig.Clothing;
		if (clothing.TemplateId >= 0)
		{
			return clothing.TemplateId;
		}
		return (short)((!random.NextBool()) ? 9 : 0);
	}

	public static sbyte GetRandomOrgMemberGender(IRandomSource random, sbyte orgTemplateId)
	{
		sbyte genderRestriction = Config.Organization.Instance[orgTemplateId].GenderRestriction;
		return (sbyte)((genderRestriction != -1) ? genderRestriction : (random.NextBool() ? 1 : 0));
	}

	public static bool MeetGenderRestriction(sbyte orgTemplateId, sbyte gender)
	{
		sbyte genderRestriction = Config.Organization.Instance[orgTemplateId].GenderRestriction;
		return genderRestriction == -1 || genderRestriction == gender;
	}

	public static bool IsSect(sbyte orgTemplateId)
	{
		return Config.Organization.Instance[orgTemplateId].IsSect;
	}

	public static short GetMemberId(sbyte orgTemplateId, sbyte grade)
	{
		return Config.Organization.Instance[orgTemplateId].Members[grade];
	}

	public static short[] GetMemberResourcesAdjust(short orgMemberId)
	{
		return OrganizationMember.Instance[orgMemberId].ResourcesAdjust;
	}

	public static short[] GetMemberMainAttributesAdjust(short orgMemberId)
	{
		return OrganizationMember.Instance[orgMemberId].MainAttributesAdjust;
	}

	public static short[] GetMemberLifeSkillsAdjust(short orgMemberId)
	{
		return OrganizationMember.Instance[orgMemberId].LifeSkillsAdjust;
	}

	public static short[] GetMemberCombatSkillsAdjust(short orgMemberId)
	{
		return OrganizationMember.Instance[orgMemberId].CombatSkillsAdjust;
	}

	public static string GetMonasticTitleSuffix(sbyte orgTemplateId, sbyte grade, sbyte gender)
	{
		OrganizationItem organizationItem = Config.Organization.Instance[orgTemplateId];
		short index = organizationItem.Members[grade];
		OrganizationMemberItem organizationMemberItem = OrganizationMember.Instance[index];
		return organizationMemberItem.MonasticTitleSuffixes[gender];
	}

	public static (short first, short last) GetSeniorityRange(sbyte seniorityGroupId)
	{
		LocalMonasticTitles instance = LocalMonasticTitles.Instance;
		if (1 == 0)
		{
		}
		(short, short) result = seniorityGroupId switch
		{
			0 => (instance.SeniorityShaolinStart, instance.SeniorityShaolinEnd), 
			1 => (instance.SeniorityEmeiStart, instance.SeniorityEmeiEnd), 
			2 => (instance.SeniorityWudangStart, instance.SeniorityWudangEnd), 
			3 => (instance.SeniorityRanshanStart, instance.SeniorityRanshanEnd), 
			_ => throw new Exception($"Unsupported seniorityGroupId {seniorityGroupId}"), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public static (short first, short last) GetMonasticTitleSuffixRange(sbyte seniorityGroupId)
	{
		LocalMonasticTitles instance = LocalMonasticTitles.Instance;
		if (1 == 0)
		{
		}
		(short, short) result = seniorityGroupId switch
		{
			0 => (instance.SuffixBuddhistStart, instance.SuffixBuddhistEnd), 
			1 => (instance.SuffixBuddhistStart, instance.SuffixBuddhistEnd), 
			2 => (instance.SuffixTaoistStart, instance.SuffixTaoistEnd), 
			3 => (instance.SuffixTaoistStart, instance.SuffixTaoistEnd), 
			_ => throw new Exception($"Unsupported seniorityGroupId {seniorityGroupId}"), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public static short GetNextSeniorityId(sbyte seniorityGroupId, short currSeniorityId)
	{
		(short first, short last) seniorityRange = GetSeniorityRange(seniorityGroupId);
		short item = seniorityRange.first;
		short item2 = seniorityRange.last;
		short num = (short)(currSeniorityId + 1);
		return (num > item2) ? item : num;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static OrganizationMemberItem GetOrgMemberConfig(OrganizationInfo orgInfo)
	{
		return orgInfo.GetOrgMemberConfig();
	}

	public static OrganizationMemberItem GetOrgMemberConfig(sbyte orgTemplateId, sbyte grade)
	{
		OrganizationItem organizationItem = Config.Organization.Instance[orgTemplateId];
		short index = organizationItem.Members[grade];
		return OrganizationMember.Instance[index];
	}

	public static short GetInitialAge(OrganizationMemberItem orgMemberCfg)
	{
		byte characterLifespanType = DomainManager.World.GetCharacterLifespanType();
		return orgMemberCfg.InitialAges[characterLifespanType];
	}

	public static short GetCharacterTemplateId(sbyte orgTemplateId, sbyte mapStateTemplateId, sbyte gender)
	{
		short num = Config.Organization.Instance[orgTemplateId].CharTemplateIds[gender];
		return (num >= 0) ? num : MapDomain.GetCharacterTemplateId(mapStateTemplateId, gender);
	}

	public static bool CanInteractWithType(GameData.Domains.Character.Character character, sbyte type)
	{
		OrganizationInfo organizationInfo = character.GetOrganizationInfo();
		OrganizationMemberItem orgMemberConfig = GetOrgMemberConfig(organizationInfo);
		if (orgMemberConfig == null)
		{
			return false;
		}
		short currAge = character.GetCurrAge();
		if (currAge < orgMemberConfig.IdentityActiveAge)
		{
			return false;
		}
		return orgMemberConfig.IdentityInteractConfig.Contains(type);
	}

	private static void InitializeSectOrgTemplateIds()
	{
		List<sbyte> list = new List<sbyte>();
		List<sbyte> list2 = new List<sbyte>();
		List<sbyte> list3 = new List<sbyte>();
		foreach (OrganizationItem item in (IEnumerable<OrganizationItem>)Config.Organization.Instance)
		{
			if (item.IsSect)
			{
				list.Add(item.TemplateId);
				switch (item.GenderRestriction)
				{
				case -1:
					list3.Add(item.TemplateId);
					list2.Add(item.TemplateId);
					break;
				case 0:
					list2.Add(item.TemplateId);
					break;
				case 1:
					list3.Add(item.TemplateId);
					break;
				}
			}
		}
		_allSectOrgTemplateIds = list.ToArray();
		_femaleSectOrgTemplateIds = list2.ToArray();
		_maleSectOrgTemplateIds = list3.ToArray();
	}

	public void Test_ContributionInfluencePowerBonus()
	{
		SettlementTreasury settlementTreasury = new SettlementTreasury();
		settlementTreasury.Contributions.Add(0, Config.Accessory.Instance[(short)8].BaseValue);
		settlementTreasury.Contributions.Add(1, Config.Accessory.Instance[(short)8].BaseValue * 10);
		Tester.Assert(settlementTreasury.CalcBonusInfluencePower(0) == 110);
		Tester.Assert(settlementTreasury.CalcBonusInfluencePower(1) == 200);
		Tester.Assert(settlementTreasury.CalcBonusInfluencePower(2) == 100);
	}

	public SettlementTreasury GetTreasury(OrganizationInfo info)
	{
		if (info.OrgTemplateId == 16)
		{
			return DomainManager.Taiwu.GetTaiwuTreasury();
		}
		Settlement settlement = DomainManager.Organization.GetSettlement(info.SettlementId);
		return settlement.GetTreasury(info.Grade);
	}

	public SettlementTreasury GetTreasury(short settlementId, sbyte layerIndex)
	{
		Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
		return settlement.Treasuries.SettlementTreasuries[layerIndex];
	}

	public void StoreItemInTreasury(DataContext context, short settlementId, GameData.Domains.Character.Character character, ItemKey itemKey, int amount, sbyte layerIndex = -1)
	{
		Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
		sbyte orgTemplateId = settlement.GetOrgTemplateId();
		if (orgTemplateId == 16)
		{
			if (character.GetId() == DomainManager.Taiwu.GetTaiwuCharId())
			{
				DomainManager.Taiwu.StoreItemInTreasury(context, itemKey, amount);
			}
			else
			{
				DomainManager.Taiwu.VillagerStoreItemInTreasury(context, character, itemKey, amount);
			}
		}
		else
		{
			settlement.StoreItemInTreasury(context, character, itemKey, amount, layerIndex);
		}
	}

	public void TakeItemFromTreasury(DataContext context, short settlementId, GameData.Domains.Character.Character character, ItemKey itemKey, int amount, bool deleteItem = false)
	{
		Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
		sbyte orgTemplateId = settlement.GetOrgTemplateId();
		if (orgTemplateId == 16)
		{
			if (character.GetId() == DomainManager.Taiwu.GetTaiwuCharId())
			{
				DomainManager.Taiwu.TakeItemFromTreasury(context, itemKey, amount, deleteItem);
			}
			else
			{
				DomainManager.Taiwu.VillagerTakeItemFromTreasury(context, character, itemKey, amount);
			}
		}
		else
		{
			settlement.TakeItemFromTreasury(context, character, itemKey, amount);
		}
	}

	public void StoreResourceInTreasury(DataContext context, short settlementId, GameData.Domains.Character.Character character, sbyte resourceType, int amount, sbyte layerIndex = -1)
	{
		Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
		sbyte orgTemplateId = settlement.GetOrgTemplateId();
		if (orgTemplateId == 16)
		{
			if (character.GetId() == DomainManager.Taiwu.GetTaiwuCharId())
			{
				DomainManager.Taiwu.StoreResourceInTreasury(context, resourceType, amount);
			}
			else
			{
				DomainManager.Taiwu.VillagerStoreResourceInTreasury(context, character, resourceType, amount);
			}
		}
		else
		{
			settlement.StoreResourceInTreasury(context, character, resourceType, amount, layerIndex);
		}
	}

	public void TakeResourceFromTreasury(DataContext context, short settlementId, GameData.Domains.Character.Character character, sbyte resourceType, int amount, sbyte layerIndex = -1)
	{
		Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
		sbyte orgTemplateId = settlement.GetOrgTemplateId();
		if (orgTemplateId == 16)
		{
			if (character.GetId() == DomainManager.Taiwu.GetTaiwuCharId())
			{
				DomainManager.Taiwu.TakeResourceFromTreasury(context, resourceType, amount);
			}
			else
			{
				DomainManager.Taiwu.VillagerTakeResourceFromTreasury(context, character, resourceType, amount);
			}
		}
		else
		{
			settlement.TakeResourceFromTreasury(context, character, resourceType, amount, layerIndex);
		}
	}

	public int CalcResourceContribution(sbyte orgTemplateId, sbyte resourceType, int amount)
	{
		OrganizationMemberItem orgMemberConfig = GetOrgMemberConfig(orgTemplateId, 8);
		return orgMemberConfig.AdjustResourceValue(resourceType, amount) * GlobalConfig.Instance.ResourceContributionPercent / 100;
	}

	public int CalcItemContribution(Settlement settlement, ItemKey itemKey, int amount)
	{
		return (settlement.GetOrgTemplateId() != 16) ? settlement.CalcItemContribution(itemKey, amount) : DomainManager.Taiwu.CalcItemContribution(itemKey, amount);
	}

	public bool IsCharacterTreasuryGuard(int charId)
	{
		if (!DomainManager.Character.TryGetElement_Objects(charId, out var element))
		{
			return false;
		}
		if (element.GetOrganizationInfo().SettlementId < 0)
		{
			return false;
		}
		return GetSettlement(element.GetOrganizationInfo().SettlementId).Treasuries.IsGuard(charId);
	}

	public byte CharacterTreasuryGuardInfo(int charId)
	{
		if (!DomainManager.Character.TryGetElement_Objects(charId, out var element))
		{
			return 0;
		}
		if (element.GetOrganizationInfo().SettlementId < 0)
		{
			return 0;
		}
		byte b = GetSettlement(element.GetOrganizationInfo().SettlementId).Treasuries.GuardLevel(charId);
		if (b != 0 && Settlement.IsGuarding(charId))
		{
			return (byte)(b | 4);
		}
		return b;
	}

	public void InitializeOwnedItems()
	{
		short taiwuVillageSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
		foreach (var (num2, _) in _settlements)
		{
			ItemKey key;
			int value2;
			if (num2 == taiwuVillageSettlementId)
			{
				if (!DomainManager.Extra.TryGetElement_SettlementTreasuries(taiwuVillageSettlementId, out var value))
				{
					continue;
				}
				foreach (KeyValuePair<ItemKey, int> item in value.Inventory.Items)
				{
					item.Deconstruct(out key, out value2);
					ItemKey itemKey = key;
					DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Treasury, num2);
				}
			}
			else
			{
				if (!DomainManager.Extra.TryGetElement_SettlementLayeredTreasuries(num2, out var value3))
				{
					continue;
				}
				SettlementTreasury[] settlementTreasuries = value3.SettlementTreasuries;
				foreach (SettlementTreasury settlementTreasury in settlementTreasuries)
				{
					foreach (KeyValuePair<ItemKey, int> item2 in settlementTreasury.Inventory.Items)
					{
						item2.Deconstruct(out key, out value2);
						ItemKey itemKey2 = key;
						DomainManager.Item.SetOwner(itemKey2, ItemOwnerType.Treasury, num2);
					}
				}
			}
		}
	}

	public void InitializeSettlementTreasury()
	{
		_firstGuardCharId = -1;
		_itemSourceChanges = null;
		_getTreasuryInventory = null;
		_stealTreasuryInventory = null;
		_exchangeTreasuryInventory = null;
		_storeTreasuryInventory = null;
		_currentTreasuryLayer = SettlementTreasuryLayers.Shallow;
	}

	public void SetCurrentTreasuryLayer(sbyte layerIndex)
	{
		_currentTreasuryLayer = (SettlementTreasuryLayers)layerIndex;
	}

	public sbyte GetCurrentTreasuryLayer()
	{
		return (sbyte)_currentTreasuryLayer;
	}

	public void SetSettlementTreasuryFirstGuardChar(int charId)
	{
		_firstGuardCharId = charId;
	}

	public void SetSettlementTreasuryAlterTime(DataContext context, short settlementId, byte time)
	{
		Settlement settlement = _settlements[settlementId];
		settlement.SetAlterTime(context, time);
	}

	public byte GetSettlementTreasuryAlterTime(DataContext context, short settlementId)
	{
		Settlement settlement = _settlements[settlementId];
		return settlement.GetAlterTime(context);
	}

	[DomainMethod]
	public SettlementTreasuryDisplayData GetSettlementTreasuryDisplayData(DataContext context, short settlementId, sbyte layerIndex)
	{
		Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
		sbyte orgTemplateId = settlement.GetOrgTemplateId();
		OrganizationItem organizationItem = Config.Organization.Instance[orgTemplateId];
		SettlementTreasury treasury = settlement.Treasuries.GetTreasury(layerIndex);
		SettlementTreasuryDisplayData result = new SettlementTreasuryDisplayData
		{
			SettlementTreasury = treasury,
			AlertTime = settlement.Treasuries.AlertTime,
			SupplyLevel = settlement.GetSupplyLevel(),
			DebtOrSupport = settlement.CalcApprovingRate(),
			GuardianCharacterDisplayDataLow = settlement.GetGuardsDisplayData(context, 0),
			GuardianCharacterDisplayDataMid = settlement.GetGuardsDisplayData(context, 1),
			GuardianCharacterDisplayDataHigh = settlement.GetGuardsDisplayData(context, 2),
			OrgTemplateId = orgTemplateId,
			SupplyItems = settlement.GetSupplyItems(),
			InfluenceRefreshTime = (byte)Math.Clamp(settlement.GetInfluencePowerUpdateDate() - DomainManager.World.GetCurrDate(), 0, 256),
			SettlementNameRelatedData = settlement.GetNameRelatedData(),
			ResourceStatus = settlement.Treasuries.GetTreasuryResourceStatus()
		};
		if (organizationItem.IsSect)
		{
			sbyte sectMainStoryTaskStatus = DomainManager.World.GetSectMainStoryTaskStatus(orgTemplateId);
			result.SectStoryEnding = sectMainStoryTaskStatus;
			short lastMartialArtTournamentWinner = DomainManager.Extra.GetLastMartialArtTournamentWinner();
			result.MartialArtTournamentResult = orgTemplateId == lastMartialArtTournamentWinner;
		}
		return result;
	}

	[DomainMethod]
	public static bool[] CheckSettlementGuardFavorabilityType(DataContext context, short settlementId)
	{
		Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
		int length = Enum.GetValues(typeof(SettlementTreasuryLayers)).Length;
		bool[] array = new bool[length];
		array[0] = true;
		for (sbyte b = 1; b < length; b++)
		{
			short item = settlement.GetGuardsAndFavors(context, b).First().Favor;
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(item);
			array[b] = favorabilityType >= 4;
		}
		return array;
	}

	[DomainMethod]
	public SettlementTreasuryRecordCollection GetSettlementTreasuryRecordCollection(DataContext context, short settlementId)
	{
		return DomainManager.Extra.GetSettlementTreasuryRecordCollection(context, settlementId);
	}

	public void SetSettlementTreasuryRecordCollection(DataContext context, short settlementId, SettlementTreasuryRecordCollection collection)
	{
		DomainManager.Extra.SetSettlementTreasuryRecordCollection(context, settlementId, collection);
	}

	[DomainMethod]
	public void ConfirmSettlementTreasuryOperation(DataContext context, int needAuthority, List<ItemSourceChange> itemSourceChanges, Inventory getTreasuryInventory, Inventory stealTreasuryInventory, Inventory exchangeTreasuryInventory, Inventory storeTreasuryInventory)
	{
		_itemSourceChanges = itemSourceChanges;
		_getTreasuryInventory = getTreasuryInventory;
		_stealTreasuryInventory = stealTreasuryInventory;
		_exchangeTreasuryInventory = exchangeTreasuryInventory;
		_storeTreasuryInventory = storeTreasuryInventory;
		ESettlementTreasuryOperationResult value;
		ItemKey itemKey;
		int count;
		if (stealTreasuryInventory != null && stealTreasuryInventory.InventoryItemTotalCount > 0)
		{
			value = ESettlementTreasuryOperationResult.Steal;
		}
		else if (getTreasuryInventory == null || getTreasuryInventory.InventoryItemTotalCount <= 0 || exchangeTreasuryInventory == null || exchangeTreasuryInventory.InventoryItemTotalCount <= 0)
		{
			value = ((storeTreasuryInventory != null && storeTreasuryInventory.InventoryItemTotalCount > 0) ? ESettlementTreasuryOperationResult.Store : ESettlementTreasuryOperationResult.None);
		}
		else
		{
			int num = 0;
			foreach (ItemSourceChange itemSourceChange in _itemSourceChanges)
			{
				foreach (ItemKeyAndCount item in itemSourceChange.Items)
				{
					item.Deconstruct(out itemKey, out count);
					ItemKey itemKey2 = itemKey;
					int num2 = count;
					num = ((!ItemTemplateHelper.IsMiscResource(itemKey2.ItemType, itemKey2.TemplateId)) ? (num + num2) : (num + ((num2 >= 0) ? 1 : (-1))));
				}
			}
			if (1 == 0)
			{
			}
			ESettlementTreasuryOperationResult eSettlementTreasuryOperationResult = ((num > 0) ? ESettlementTreasuryOperationResult.Steal : ((num == 0) ? ESettlementTreasuryOperationResult.Exchange : ESettlementTreasuryOperationResult.Store));
			if (1 == 0)
			{
			}
			value = eSettlementTreasuryOperationResult;
		}
		DomainManager.TaiwuEvent.SetListenerEventActionIntArg("ShopActionComplete", "OperationResult", (int)value);
		DomainManager.TaiwuEvent.SetListenerEventActionIntArg("ShopActionComplete", "NeedAuthority", needAuthority);
		DomainManager.TaiwuEvent.SetListenerEventActionISerializableArg("ShopActionComplete", "StealTreasuryInventory", (ISerializableGameData)(object)stealTreasuryInventory);
		DomainManager.TaiwuEvent.SetListenerEventActionISerializableArg("ShopActionComplete", "StoreTreasuryInventory", (ISerializableGameData)(object)storeTreasuryInventory);
		if (itemSourceChanges == null || itemSourceChanges.Count == 0)
		{
			return;
		}
		foreach (ItemSourceChange itemSourceChange2 in _itemSourceChanges)
		{
			foreach (ItemKeyAndCount item2 in itemSourceChange2.Items)
			{
				item2.Deconstruct(out itemKey, out count);
				ItemKey itemKey3 = itemKey;
				int num3 = count;
				if (num3 >= 0)
				{
					continue;
				}
				if (ItemTemplateHelper.IsMiscResource(itemKey3.ItemType, itemKey3.TemplateId))
				{
					sbyte miscResourceType = ItemTemplateHelper.GetMiscResourceType(itemKey3.ItemType, itemKey3.TemplateId);
					if (itemSourceChange2.ItemSourceTypeEnum != ItemSourceType.Treasury)
					{
						DomainManager.Taiwu.GetTaiwu().ChangeResource(context, miscResourceType, num3);
					}
					else
					{
						DomainManager.Taiwu.TakeResourceFromTreasury(context, miscResourceType, -num3);
					}
				}
				else
				{
					DomainManager.Taiwu.RemoveItem(context, itemKey3, -num3, itemSourceChange2.ItemSourceType, deleteItem: false);
				}
			}
		}
	}

	[DomainMethod]
	public List<ItemSourceChange> GetLastSettlementTreasuryOperationData()
	{
		return _itemSourceChanges;
	}

	public (CharacterSet changeFavorCharIds, CharacterSet becomeEnemyCharIds, CharacterSet approveCharIds, CharacterSet disapproveCharIds) ApplySettlementTreasuryEventEffect(DataContext context, SettlementTreasuryEventEffectItem effectCfg, short settlementId, int totalWorth, sbyte layerIndex)
	{
		Settlement settlement = GetSettlement(settlementId);
		OrgMemberCollection members = settlement.GetMembers();
		if (effectCfg.TaiwuBounty >= 0)
		{
			PunishmentTypeItem punishmentTypeItem = PunishmentType.Instance[effectCfg.TaiwuBounty];
			if (punishmentTypeItem != null)
			{
				sbyte sectOrgTemplateIdByStateTemplateId = MapDomain.GetSectOrgTemplateIdByStateTemplateId(DomainManager.Map.GetStateTemplateIdByAreaId(settlement.GetLocation().AreaId));
				Sect sect = DomainManager.Organization.GetSettlementByOrgTemplateId(sectOrgTemplateIdByStateTemplateId) as Sect;
				sect.AddBounty(context, DomainManager.Taiwu.GetTaiwu(), punishmentTypeItem.Severity, effectCfg.TaiwuBounty);
			}
		}
		if (effectCfg.AlterTime > 0)
		{
			settlement.SetAlterTime(context, (byte)effectCfg.AlterTime);
		}
		HashSet<int> collection = settlement.Treasuries.GetTreasury(layerIndex).GuardIds.GetCollection();
		SettlementTreasuryEventEffectHelper.EffectArgs args = new SettlementTreasuryEventEffectHelper.EffectArgs(effectCfg, collection.Count, totalWorth, isGuard: true);
		ApplySettlementTreasuryEventEffect(context, collection, ref args);
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		for (sbyte b = 0; b <= 8; b++)
		{
			IEnumerable<int> enumerable = DomainManager.Character.ExcludeInfant(members.GetMembers(b));
			foreach (int item in enumerable)
			{
				if (!collection.Contains(item))
				{
					list.Add(item);
				}
			}
		}
		CollectionUtils.Shuffle(context.Random, list);
		SettlementTreasuryEventEffectHelper.EffectArgs effectArgs = new SettlementTreasuryEventEffectHelper.EffectArgs(effectCfg, list.Count, totalWorth, isGuard: false);
		effectArgs.ChangeFavorCharIds = args.ChangeFavorCharIds;
		effectArgs.ApproveCharIds = args.ApproveCharIds;
		effectArgs.DisapproveCharIds = args.DisapproveCharIds;
		effectArgs.BecomeEnemyCharIds = args.BecomeEnemyCharIds;
		SettlementTreasuryEventEffectHelper.EffectArgs args2 = effectArgs;
		ApplySettlementTreasuryEventEffect(context, list, ref args2);
		Location location = settlement.GetLocation();
		int num = effectCfg.CalcSpiritualDebtChange(totalWorth);
		if (num != 0)
		{
			DomainManager.Extra.ChangeAreaSpiritualDebt(context, location.AreaId, num);
		}
		ObjectPool<List<int>>.Instance.Return(list);
		_itemSourceChanges?.Clear();
		return (changeFavorCharIds: args2.ChangeFavorCharIds, becomeEnemyCharIds: args2.BecomeEnemyCharIds, approveCharIds: args2.ApproveCharIds, disapproveCharIds: args2.DisapproveCharIds);
	}

	public void ApplySettlementTreasuryEventEffect(DataContext context, IEnumerable<int> charIds, ref SettlementTreasuryEventEffectHelper.EffectArgs args)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int id = taiwu.GetId();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		foreach (int charId in charIds)
		{
			if (num >= args.ChangeFavorCount)
			{
				break;
			}
			if (!DomainManager.Character.TryGetElement_Objects(charId, out var element) || element.GetCreatingType() != 1 || DomainManager.Organization.GetPrisonerSect(charId) >= 0)
			{
				continue;
			}
			short favorability = DomainManager.Character.GetFavorability(element.GetId(), taiwu.GetId());
			if (favorability == short.MinValue)
			{
				DomainManager.Character.TryCreateGeneralRelation(context, element, taiwu);
				favorability = DomainManager.Character.GetFavorability(element.GetId(), taiwu.GetId());
			}
			int num5 = DomainManager.Character.CalcFavorabilityDelta(charId, id, args.FavorChange, -1);
			if (num5 == 0)
			{
				continue;
			}
			DomainManager.Character.DirectlyChangeFavorabilityOptional(context, element, taiwu, args.FavorChange, 3);
			DomainManager.Character.AddFavorabilityChangeInstantNotification(element, taiwu, num5 > 0);
			args.ChangeFavorCharIds.Add(charId);
			if (num4 < args.BecomeEnemyCount && !DomainManager.Character.HasRelation(charId, id, 32768))
			{
				DomainManager.Character.AddRelation(context, charId, id, 32768);
				args.BecomeEnemyCharIds.Add(charId);
				num4++;
			}
			SettlementCharacter settlementCharacter = GetSettlementCharacter(charId);
			if (settlementCharacter.GetApprovedTaiwu())
			{
				if (num2 < args.DisapproveCount)
				{
					settlementCharacter.SetApprovedTaiwu(context, approve: false);
					args.DisapproveCharIds.Add(charId);
					num2++;
				}
			}
			else if (num3 < args.ApproveCount)
			{
				settlementCharacter.SetApprovedTaiwu(context, approve: true);
				args.ApproveCharIds.Add(charId);
				num3++;
			}
			num++;
		}
	}

	public void SettleTreasuryOperate(DataContext context, short settlementId, bool takeEffect, bool startCombat, bool isSect, bool restart)
	{
		if (startCombat)
		{
			DomainManager.Organization.SetSettlementTreasuryAlterTime(context, settlementId, (byte)GlobalConfig.Instance.SettlementAlterTime);
		}
		if (_itemSourceChanges == null)
		{
			InitializeSettlementTreasury();
			return;
		}
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		ItemKey itemKey;
		int count;
		foreach (ItemSourceChange itemSourceChange in _itemSourceChanges)
		{
			foreach (ItemKeyAndCount item in itemSourceChange.Items)
			{
				item.Deconstruct(out itemKey, out count);
				ItemKey itemKey2 = itemKey;
				int num = count;
				if (num >= 0)
				{
					continue;
				}
				if (ItemTemplateHelper.IsMiscResource(itemKey2.ItemType, itemKey2.TemplateId))
				{
					sbyte miscResourceType = ItemTemplateHelper.GetMiscResourceType(itemKey2.ItemType, itemKey2.TemplateId);
					if (itemSourceChange.ItemSourceTypeEnum != ItemSourceType.Treasury)
					{
						DomainManager.Taiwu.GetTaiwu().ChangeResource(context, miscResourceType, -num);
					}
					else
					{
						DomainManager.Taiwu.StoreResourceInTreasury(context, miscResourceType, -num);
					}
				}
				else
				{
					DomainManager.Taiwu.AddItem(context, itemKey2, -num, itemSourceChange.ItemSourceType);
				}
			}
		}
		if (restart)
		{
			return;
		}
		if (takeEffect)
		{
			foreach (ItemSourceChange itemSourceChange2 in _itemSourceChanges)
			{
				foreach (ItemKeyAndCount item2 in itemSourceChange2.Items)
				{
					item2.Deconstruct(out itemKey, out count);
					ItemKey itemKey3 = itemKey;
					int num2 = count;
					if (num2 > 0)
					{
						if (_stealTreasuryInventory.InventoryItemTotalCount > 0)
						{
							SettlementTreasuryRecordCollection settlementTreasuryRecordCollection = DomainManager.Organization.GetSettlementTreasuryRecordCollection(context, settlementId);
							int currDate = DomainManager.World.GetCurrDate();
							if (isSect)
							{
								settlementTreasuryRecordCollection.AddPlunderSectTreasurySuccess(currDate, settlementId, taiwuCharId);
							}
							else
							{
								settlementTreasuryRecordCollection.AddPlunderTownTreasurySuccess(currDate, settlementId, taiwuCharId);
							}
							DomainManager.Organization.SetSettlementTreasuryRecordCollection(context, settlementId, settlementTreasuryRecordCollection);
						}
						if (ItemTemplateHelper.IsMiscResource(itemKey3.ItemType, itemKey3.TemplateId))
						{
							GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
							sbyte miscResourceType2 = ItemTemplateHelper.GetMiscResourceType(itemKey3.ItemType, itemKey3.TemplateId);
							DomainManager.Organization.TakeResourceFromTreasury(context, settlementId, taiwu, miscResourceType2, num2, GetCurrentTreasuryLayer());
							DomainManager.Taiwu.GetTaiwu().ChangeResource(context, miscResourceType2, num2);
						}
						else
						{
							DomainManager.Organization.TakeItemFromTreasury(context, settlementId, DomainManager.Taiwu.GetTaiwu(), itemKey3, num2);
							DomainManager.Taiwu.AddItem(context, itemKey3, num2, itemSourceChange2.ItemSourceType);
						}
					}
					else
					{
						if (num2 >= 0)
						{
							continue;
						}
						if (ItemTemplateHelper.IsMiscResource(itemKey3.ItemType, itemKey3.TemplateId))
						{
							GameData.Domains.Character.Character taiwu2 = DomainManager.Taiwu.GetTaiwu();
							sbyte miscResourceType3 = ItemTemplateHelper.GetMiscResourceType(itemKey3.ItemType, itemKey3.TemplateId);
							if (itemSourceChange2.ItemSourceTypeEnum != ItemSourceType.Treasury)
							{
								DomainManager.Taiwu.GetTaiwu().ChangeResource(context, miscResourceType3, num2);
							}
							else
							{
								TakeResourceFromTreasury(context, DomainManager.Taiwu.GetTaiwu().GetOrganizationInfo().SettlementId, DomainManager.Taiwu.GetTaiwu(), miscResourceType3, -num2, -1);
							}
							DomainManager.Organization.StoreResourceInTreasury(context, settlementId, taiwu2, miscResourceType3, -num2, GetCurrentTreasuryLayer());
						}
						else
						{
							DomainManager.Taiwu.RemoveItem(context, itemKey3, -num2, itemSourceChange2.ItemSourceType, deleteItem: false);
							DomainManager.Organization.StoreItemInTreasury(context, settlementId, DomainManager.Taiwu.GetTaiwu(), itemKey3, -num2, GetCurrentTreasuryLayer());
						}
					}
				}
			}
		}
		else if (startCombat && _stealTreasuryInventory.InventoryItemTotalCount > 0)
		{
			SettlementTreasuryRecordCollection settlementTreasuryRecordCollection2 = DomainManager.Organization.GetSettlementTreasuryRecordCollection(context, settlementId);
			int currDate2 = DomainManager.World.GetCurrDate();
			if (isSect)
			{
				settlementTreasuryRecordCollection2.AddPlunderSectTreasuryFail(currDate2, settlementId, taiwuCharId);
			}
			else
			{
				settlementTreasuryRecordCollection2.AddPlunderTownTreasuryFail(currDate2, settlementId, taiwuCharId);
			}
			DomainManager.Organization.SetSettlementTreasuryRecordCollection(context, settlementId, settlementTreasuryRecordCollection2);
		}
		InitializeSettlementTreasury();
	}

	public sbyte CheckTreasuryLayerItemGradeRange(EventArgBox argBox)
	{
		return 1;
	}

	[DomainMethod]
	public void GmCmd_ClearSettlementTreasuryAlertTime(DataContext context, short settlementId)
	{
		if (settlementId >= 0)
		{
			SettlementLayeredTreasuries treasuries = GetSettlement(settlementId).Treasuries;
			treasuries.AlertTime = 0;
			DomainManager.Extra.SetTreasuries(context, settlementId, treasuries, needUpdateTotalValue: false);
		}
	}

	[DomainMethod]
	public void GmCmd_ClearSettlementTreasuryItemAndResource(DataContext context, short settlementId)
	{
		if (settlementId >= 0)
		{
			SettlementLayeredTreasuries treasuries = GetSettlement(settlementId).Treasuries;
			SettlementTreasury[] settlementTreasuries = treasuries.SettlementTreasuries;
			foreach (SettlementTreasury settlementTreasury in settlementTreasuries)
			{
				settlementTreasury?.Inventory?.Items?.Clear();
				settlementTreasury?.Resources.Initialize();
			}
			DomainManager.Extra.SetTreasuries(context, settlementId, treasuries, needUpdateTotalValue: true);
		}
	}

	[DomainMethod]
	public void GmCmd_UpdateSettlementTreasury(DataContext context, short settlementId)
	{
		if (_settlements.TryGetValue(settlementId, out var value))
		{
			value.UpdateTreasury(context);
		}
	}

	public OrganizationDomain()
		: base(9)
	{
		_sects = new Dictionary<short, Sect>(0);
		_civilianSettlements = new Dictionary<short, CivilianSettlement>(0);
		_nextSettlementId = 0;
		_sectCharacters = new Dictionary<int, SectCharacter>(0);
		_civilianSettlementCharacters = new Dictionary<int, CivilianSettlementCharacter>(0);
		_factions = new Dictionary<int, CharacterSet>(0);
		_largeSectFavorabilities = new sbyte[64];
		_martialArtTournamentPreparationInfoList = new List<MartialArtTournamentPreparationInfo>();
		_previousMartialArtTournamentHosts = new List<short>();
		HelperDataSects = new ObjectCollectionHelperData(3, 0, CacheInfluencesSects, _dataStatesSects, isArchive: true);
		HelperDataCivilianSettlements = new ObjectCollectionHelperData(3, 1, CacheInfluencesCivilianSettlements, _dataStatesCivilianSettlements, isArchive: true);
		HelperDataSectCharacters = new ObjectCollectionHelperData(3, 3, CacheInfluencesSectCharacters, _dataStatesSectCharacters, isArchive: true);
		HelperDataCivilianSettlementCharacters = new ObjectCollectionHelperData(3, 4, CacheInfluencesCivilianSettlementCharacters, _dataStatesCivilianSettlementCharacters, isArchive: true);
		OnInitializedDomainData();
	}

	public Sect GetElement_Sects(short objectId)
	{
		return _sects[objectId];
	}

	public bool TryGetElement_Sects(short objectId, out Sect element)
	{
		return _sects.TryGetValue(objectId, out element);
	}

	private unsafe void AddElement_Sects(short objectId, Sect instance)
	{
		instance.CollectionHelperData = HelperDataSects;
		instance.DataStatesOffset = _dataStatesSects.Create();
		_sects.Add(objectId, instance);
		byte* pData = OperationAdder.DynamicObjectCollection_Add(3, 0, objectId, instance.GetSerializedSize());
		instance.Serialize(pData);
	}

	private void RemoveElement_Sects(short objectId)
	{
		if (_sects.TryGetValue(objectId, out var value))
		{
			_dataStatesSects.Remove(value.DataStatesOffset);
			_sects.Remove(objectId);
			OperationAdder.DynamicObjectCollection_Remove(3, 0, objectId);
		}
	}

	private void ClearSects()
	{
		_dataStatesSects.Clear();
		_sects.Clear();
		OperationAdder.DynamicObjectCollection_Clear(3, 0);
	}

	public int GetElementField_Sects(short objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
	{
		if (!_sects.TryGetValue(objectId, out var value))
		{
			AdaptableLog.TagWarning("GetElementField_Sects", $"Failed to find element {objectId} with field {fieldId}");
			return -1;
		}
		if (resetModified)
		{
			_dataStatesSects.ResetModified(value.DataStatesOffset, fieldId);
		}
		switch (fieldId)
		{
		case 0:
			return GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool);
		case 1:
			return GameData.Serializer.Serializer.Serialize(value.GetOrgTemplateId(), dataPool);
		case 2:
			return GameData.Serializer.Serializer.Serialize(value.GetLocation(), dataPool);
		case 3:
			return GameData.Serializer.Serializer.Serialize(value.GetCulture(), dataPool);
		case 4:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxCulture(), dataPool);
		case 5:
			return GameData.Serializer.Serializer.Serialize(value.GetSafety(), dataPool);
		case 6:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxSafety(), dataPool);
		case 7:
			return GameData.Serializer.Serializer.Serialize(value.GetPopulation(), dataPool);
		case 8:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxPopulation(), dataPool);
		case 9:
			return GameData.Serializer.Serializer.Serialize(value.GetStandardOnStagePopulation(), dataPool);
		case 10:
			return GameData.Serializer.Serializer.Serialize(value.GetMembers(), dataPool);
		case 11:
			return GameData.Serializer.Serializer.Serialize(value.GetLackingCoreMembers(), dataPool);
		case 12:
			return GameData.Serializer.Serializer.Serialize(value.GetApprovingRateUpperLimitBonus(), dataPool);
		case 13:
			return GameData.Serializer.Serializer.Serialize(value.GetInfluencePowerUpdateDate(), dataPool);
		case 14:
			return GameData.Serializer.Serializer.Serialize(value.GetMinSeniorityId(), dataPool);
		case 15:
			return GameData.Serializer.Serializer.Serialize(value.GetAvailableMonasticTitleSuffixIds(), dataPool);
		case 16:
			return GameData.Serializer.Serializer.Serialize(value.GetTaiwuExploreStatus(), dataPool);
		case 17:
			return GameData.Serializer.Serializer.Serialize(value.GetSpiritualDebtInteractionOccurred(), dataPool);
		case 18:
			return GameData.Serializer.Serializer.Serialize(value.GetTaiwuInvestmentForMartialArtTournament(), dataPool);
		case 19:
			return GameData.Serializer.Serializer.Serialize(value.GetApprovingRateUpperLimitTempBonus(), dataPool);
		default:
			if (fieldId >= 20)
			{
				throw new Exception($"Unsupported fieldId {fieldId}");
			}
			throw new Exception($"Not allow to get readonly field data: {fieldId}");
		}
	}

	public void SetElementField_Sects(short objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		if (!_sects.TryGetValue(objectId, out var value))
		{
			throw new Exception($"Failed to find element {objectId} with field {fieldId}");
		}
		switch (fieldId)
		{
		case 0:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 1:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 2:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 3:
		{
			short item = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item);
			value.SetCulture(item, context);
			return;
		}
		case 4:
		{
			short item16 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item16);
			value.SetMaxCulture(item16, context);
			return;
		}
		case 5:
		{
			short item15 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item15);
			value.SetSafety(item15, context);
			return;
		}
		case 6:
		{
			short item14 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item14);
			value.SetMaxSafety(item14, context);
			return;
		}
		case 7:
		{
			int item13 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item13);
			value.SetPopulation(item13, context);
			return;
		}
		case 8:
		{
			int item12 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item12);
			value.SetMaxPopulation(item12, context);
			return;
		}
		case 9:
		{
			int item11 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item11);
			value.SetStandardOnStagePopulation(item11, context);
			return;
		}
		case 10:
		{
			OrgMemberCollection item10 = value.GetMembers();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item10);
			value.SetMembers(item10, context);
			return;
		}
		case 11:
		{
			OrgMemberCollection item9 = value.GetLackingCoreMembers();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item9);
			value.SetLackingCoreMembers(item9, context);
			return;
		}
		case 12:
		{
			short item8 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item8);
			value.SetApprovingRateUpperLimitBonus(item8, context);
			return;
		}
		case 13:
		{
			int item7 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item7);
			value.SetInfluencePowerUpdateDate(item7, context);
			return;
		}
		case 14:
		{
			short item6 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item6);
			value.SetMinSeniorityId(item6, context);
			return;
		}
		case 15:
		{
			List<short> item5 = value.GetAvailableMonasticTitleSuffixIds();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item5);
			value.SetAvailableMonasticTitleSuffixIds(item5, context);
			return;
		}
		case 16:
		{
			byte item4 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item4);
			value.SetTaiwuExploreStatus(item4, context);
			return;
		}
		case 17:
		{
			bool item3 = false;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item3);
			value.SetSpiritualDebtInteractionOccurred(item3, context);
			return;
		}
		case 18:
		{
			int[] item2 = value.GetTaiwuInvestmentForMartialArtTournament();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item2);
			value.SetTaiwuInvestmentForMartialArtTournament(item2, context);
			return;
		}
		}
		if (fieldId >= 20)
		{
			throw new Exception($"Unsupported fieldId {fieldId}");
		}
		if (fieldId >= 20)
		{
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		}
		throw new Exception($"Not allow to set cache field data: {fieldId}");
	}

	private int CheckModified_Sects(short objectId, ushort fieldId, RawDataPool dataPool)
	{
		if (!_sects.TryGetValue(objectId, out var value))
		{
			return -1;
		}
		if (fieldId >= 20)
		{
			throw new Exception($"Not allow to check readonly field data: {fieldId}");
		}
		if (!_dataStatesSects.IsModified(value.DataStatesOffset, fieldId))
		{
			return -1;
		}
		_dataStatesSects.ResetModified(value.DataStatesOffset, fieldId);
		return fieldId switch
		{
			0 => GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool), 
			1 => GameData.Serializer.Serializer.Serialize(value.GetOrgTemplateId(), dataPool), 
			2 => GameData.Serializer.Serializer.Serialize(value.GetLocation(), dataPool), 
			3 => GameData.Serializer.Serializer.Serialize(value.GetCulture(), dataPool), 
			4 => GameData.Serializer.Serializer.Serialize(value.GetMaxCulture(), dataPool), 
			5 => GameData.Serializer.Serializer.Serialize(value.GetSafety(), dataPool), 
			6 => GameData.Serializer.Serializer.Serialize(value.GetMaxSafety(), dataPool), 
			7 => GameData.Serializer.Serializer.Serialize(value.GetPopulation(), dataPool), 
			8 => GameData.Serializer.Serializer.Serialize(value.GetMaxPopulation(), dataPool), 
			9 => GameData.Serializer.Serializer.Serialize(value.GetStandardOnStagePopulation(), dataPool), 
			10 => GameData.Serializer.Serializer.Serialize(value.GetMembers(), dataPool), 
			11 => GameData.Serializer.Serializer.Serialize(value.GetLackingCoreMembers(), dataPool), 
			12 => GameData.Serializer.Serializer.Serialize(value.GetApprovingRateUpperLimitBonus(), dataPool), 
			13 => GameData.Serializer.Serializer.Serialize(value.GetInfluencePowerUpdateDate(), dataPool), 
			14 => GameData.Serializer.Serializer.Serialize(value.GetMinSeniorityId(), dataPool), 
			15 => GameData.Serializer.Serializer.Serialize(value.GetAvailableMonasticTitleSuffixIds(), dataPool), 
			16 => GameData.Serializer.Serializer.Serialize(value.GetTaiwuExploreStatus(), dataPool), 
			17 => GameData.Serializer.Serializer.Serialize(value.GetSpiritualDebtInteractionOccurred(), dataPool), 
			18 => GameData.Serializer.Serializer.Serialize(value.GetTaiwuInvestmentForMartialArtTournament(), dataPool), 
			19 => GameData.Serializer.Serializer.Serialize(value.GetApprovingRateUpperLimitTempBonus(), dataPool), 
			_ => throw new Exception($"Unsupported fieldId {fieldId}"), 
		};
	}

	private void ResetModifiedWrapper_Sects(short objectId, ushort fieldId)
	{
		if (_sects.TryGetValue(objectId, out var value))
		{
			if (fieldId >= 20)
			{
				throw new Exception($"Not allow to reset modification state of readonly field data: {fieldId}");
			}
			if (_dataStatesSects.IsModified(value.DataStatesOffset, fieldId))
			{
				_dataStatesSects.ResetModified(value.DataStatesOffset, fieldId);
			}
		}
	}

	private bool IsModifiedWrapper_Sects(short objectId, ushort fieldId)
	{
		if (!_sects.TryGetValue(objectId, out var value))
		{
			return false;
		}
		if (fieldId >= 20)
		{
			throw new Exception($"Not allow to check modification state of readonly field data: {fieldId}");
		}
		return _dataStatesSects.IsModified(value.DataStatesOffset, fieldId);
	}

	public CivilianSettlement GetElement_CivilianSettlements(short objectId)
	{
		return _civilianSettlements[objectId];
	}

	public bool TryGetElement_CivilianSettlements(short objectId, out CivilianSettlement element)
	{
		return _civilianSettlements.TryGetValue(objectId, out element);
	}

	private unsafe void AddElement_CivilianSettlements(short objectId, CivilianSettlement instance)
	{
		instance.CollectionHelperData = HelperDataCivilianSettlements;
		instance.DataStatesOffset = _dataStatesCivilianSettlements.Create();
		_civilianSettlements.Add(objectId, instance);
		byte* pData = OperationAdder.DynamicObjectCollection_Add(3, 1, objectId, instance.GetSerializedSize());
		instance.Serialize(pData);
	}

	private void RemoveElement_CivilianSettlements(short objectId)
	{
		if (_civilianSettlements.TryGetValue(objectId, out var value))
		{
			_dataStatesCivilianSettlements.Remove(value.DataStatesOffset);
			_civilianSettlements.Remove(objectId);
			OperationAdder.DynamicObjectCollection_Remove(3, 1, objectId);
		}
	}

	private void ClearCivilianSettlements()
	{
		_dataStatesCivilianSettlements.Clear();
		_civilianSettlements.Clear();
		OperationAdder.DynamicObjectCollection_Clear(3, 1);
	}

	public int GetElementField_CivilianSettlements(short objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
	{
		if (!_civilianSettlements.TryGetValue(objectId, out var value))
		{
			AdaptableLog.TagWarning("GetElementField_CivilianSettlements", $"Failed to find element {objectId} with field {fieldId}");
			return -1;
		}
		if (resetModified)
		{
			_dataStatesCivilianSettlements.ResetModified(value.DataStatesOffset, fieldId);
		}
		switch (fieldId)
		{
		case 0:
			return GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool);
		case 1:
			return GameData.Serializer.Serializer.Serialize(value.GetOrgTemplateId(), dataPool);
		case 2:
			return GameData.Serializer.Serializer.Serialize(value.GetLocation(), dataPool);
		case 3:
			return GameData.Serializer.Serializer.Serialize(value.GetCulture(), dataPool);
		case 4:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxCulture(), dataPool);
		case 5:
			return GameData.Serializer.Serializer.Serialize(value.GetSafety(), dataPool);
		case 6:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxSafety(), dataPool);
		case 7:
			return GameData.Serializer.Serializer.Serialize(value.GetPopulation(), dataPool);
		case 8:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxPopulation(), dataPool);
		case 9:
			return GameData.Serializer.Serializer.Serialize(value.GetStandardOnStagePopulation(), dataPool);
		case 10:
			return GameData.Serializer.Serializer.Serialize(value.GetMembers(), dataPool);
		case 11:
			return GameData.Serializer.Serializer.Serialize(value.GetLackingCoreMembers(), dataPool);
		case 12:
			return GameData.Serializer.Serializer.Serialize(value.GetApprovingRateUpperLimitBonus(), dataPool);
		case 13:
			return GameData.Serializer.Serializer.Serialize(value.GetInfluencePowerUpdateDate(), dataPool);
		case 14:
			return GameData.Serializer.Serializer.Serialize(value.GetRandomNameId(), dataPool);
		case 15:
			return GameData.Serializer.Serializer.Serialize(value.GetMainMorality(), dataPool);
		case 16:
			return GameData.Serializer.Serializer.Serialize(value.GetApprovingRateUpperLimitTempBonus(), dataPool);
		default:
			if (fieldId >= 17)
			{
				throw new Exception($"Unsupported fieldId {fieldId}");
			}
			throw new Exception($"Not allow to get readonly field data: {fieldId}");
		}
	}

	public void SetElementField_CivilianSettlements(short objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		if (!_civilianSettlements.TryGetValue(objectId, out var value))
		{
			throw new Exception($"Failed to find element {objectId} with field {fieldId}");
		}
		switch (fieldId)
		{
		case 0:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 1:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 2:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 3:
		{
			short item = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item);
			value.SetCulture(item, context);
			return;
		}
		case 4:
		{
			short item12 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item12);
			value.SetMaxCulture(item12, context);
			return;
		}
		case 5:
		{
			short item11 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item11);
			value.SetSafety(item11, context);
			return;
		}
		case 6:
		{
			short item10 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item10);
			value.SetMaxSafety(item10, context);
			return;
		}
		case 7:
		{
			int item9 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item9);
			value.SetPopulation(item9, context);
			return;
		}
		case 8:
		{
			int item8 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item8);
			value.SetMaxPopulation(item8, context);
			return;
		}
		case 9:
		{
			int item7 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item7);
			value.SetStandardOnStagePopulation(item7, context);
			return;
		}
		case 10:
		{
			OrgMemberCollection item6 = value.GetMembers();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item6);
			value.SetMembers(item6, context);
			return;
		}
		case 11:
		{
			OrgMemberCollection item5 = value.GetLackingCoreMembers();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item5);
			value.SetLackingCoreMembers(item5, context);
			return;
		}
		case 12:
		{
			short item4 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item4);
			value.SetApprovingRateUpperLimitBonus(item4, context);
			return;
		}
		case 13:
		{
			int item3 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item3);
			value.SetInfluencePowerUpdateDate(item3, context);
			return;
		}
		case 14:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 15:
		{
			short item2 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item2);
			value.SetMainMorality(item2, context);
			return;
		}
		}
		if (fieldId >= 17)
		{
			throw new Exception($"Unsupported fieldId {fieldId}");
		}
		if (fieldId >= 17)
		{
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		}
		throw new Exception($"Not allow to set cache field data: {fieldId}");
	}

	private int CheckModified_CivilianSettlements(short objectId, ushort fieldId, RawDataPool dataPool)
	{
		if (!_civilianSettlements.TryGetValue(objectId, out var value))
		{
			return -1;
		}
		if (fieldId >= 17)
		{
			throw new Exception($"Not allow to check readonly field data: {fieldId}");
		}
		if (!_dataStatesCivilianSettlements.IsModified(value.DataStatesOffset, fieldId))
		{
			return -1;
		}
		_dataStatesCivilianSettlements.ResetModified(value.DataStatesOffset, fieldId);
		return fieldId switch
		{
			0 => GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool), 
			1 => GameData.Serializer.Serializer.Serialize(value.GetOrgTemplateId(), dataPool), 
			2 => GameData.Serializer.Serializer.Serialize(value.GetLocation(), dataPool), 
			3 => GameData.Serializer.Serializer.Serialize(value.GetCulture(), dataPool), 
			4 => GameData.Serializer.Serializer.Serialize(value.GetMaxCulture(), dataPool), 
			5 => GameData.Serializer.Serializer.Serialize(value.GetSafety(), dataPool), 
			6 => GameData.Serializer.Serializer.Serialize(value.GetMaxSafety(), dataPool), 
			7 => GameData.Serializer.Serializer.Serialize(value.GetPopulation(), dataPool), 
			8 => GameData.Serializer.Serializer.Serialize(value.GetMaxPopulation(), dataPool), 
			9 => GameData.Serializer.Serializer.Serialize(value.GetStandardOnStagePopulation(), dataPool), 
			10 => GameData.Serializer.Serializer.Serialize(value.GetMembers(), dataPool), 
			11 => GameData.Serializer.Serializer.Serialize(value.GetLackingCoreMembers(), dataPool), 
			12 => GameData.Serializer.Serializer.Serialize(value.GetApprovingRateUpperLimitBonus(), dataPool), 
			13 => GameData.Serializer.Serializer.Serialize(value.GetInfluencePowerUpdateDate(), dataPool), 
			14 => GameData.Serializer.Serializer.Serialize(value.GetRandomNameId(), dataPool), 
			15 => GameData.Serializer.Serializer.Serialize(value.GetMainMorality(), dataPool), 
			16 => GameData.Serializer.Serializer.Serialize(value.GetApprovingRateUpperLimitTempBonus(), dataPool), 
			_ => throw new Exception($"Unsupported fieldId {fieldId}"), 
		};
	}

	private void ResetModifiedWrapper_CivilianSettlements(short objectId, ushort fieldId)
	{
		if (_civilianSettlements.TryGetValue(objectId, out var value))
		{
			if (fieldId >= 17)
			{
				throw new Exception($"Not allow to reset modification state of readonly field data: {fieldId}");
			}
			if (_dataStatesCivilianSettlements.IsModified(value.DataStatesOffset, fieldId))
			{
				_dataStatesCivilianSettlements.ResetModified(value.DataStatesOffset, fieldId);
			}
		}
	}

	private bool IsModifiedWrapper_CivilianSettlements(short objectId, ushort fieldId)
	{
		if (!_civilianSettlements.TryGetValue(objectId, out var value))
		{
			return false;
		}
		if (fieldId >= 17)
		{
			throw new Exception($"Not allow to check modification state of readonly field data: {fieldId}");
		}
		return _dataStatesCivilianSettlements.IsModified(value.DataStatesOffset, fieldId);
	}

	private short GetNextSettlementId()
	{
		return _nextSettlementId;
	}

	private unsafe void SetNextSettlementId(short value, DataContext context)
	{
		_nextSettlementId = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(2, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(3, 2, 2);
		*(short*)ptr = _nextSettlementId;
		ptr += 2;
	}

	public SectCharacter GetElement_SectCharacters(int objectId)
	{
		return _sectCharacters[objectId];
	}

	public bool TryGetElement_SectCharacters(int objectId, out SectCharacter element)
	{
		return _sectCharacters.TryGetValue(objectId, out element);
	}

	private unsafe void AddElement_SectCharacters(int objectId, SectCharacter instance)
	{
		instance.CollectionHelperData = HelperDataSectCharacters;
		instance.DataStatesOffset = _dataStatesSectCharacters.Create();
		_sectCharacters.Add(objectId, instance);
		byte* pData = OperationAdder.FixedObjectCollection_Add(3, 3, objectId, 12);
		instance.Serialize(pData);
	}

	private void RemoveElement_SectCharacters(int objectId)
	{
		if (_sectCharacters.TryGetValue(objectId, out var value))
		{
			_dataStatesSectCharacters.Remove(value.DataStatesOffset);
			_sectCharacters.Remove(objectId);
			OperationAdder.FixedObjectCollection_Remove(3, 3, objectId);
		}
	}

	private void ClearSectCharacters()
	{
		_dataStatesSectCharacters.Clear();
		_sectCharacters.Clear();
		OperationAdder.FixedObjectCollection_Clear(3, 3);
	}

	public int GetElementField_SectCharacters(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
	{
		if (!_sectCharacters.TryGetValue(objectId, out var value))
		{
			AdaptableLog.TagWarning("GetElementField_SectCharacters", $"Failed to find element {objectId} with field {fieldId}");
			return -1;
		}
		if (resetModified)
		{
			_dataStatesSectCharacters.ResetModified(value.DataStatesOffset, fieldId);
		}
		switch (fieldId)
		{
		case 0:
			return GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool);
		case 1:
			return GameData.Serializer.Serializer.Serialize(value.GetOrgTemplateId(), dataPool);
		case 2:
			return GameData.Serializer.Serializer.Serialize(value.GetSettlementId(), dataPool);
		case 3:
			return GameData.Serializer.Serializer.Serialize(value.GetApprovedTaiwu(), dataPool);
		case 4:
			return GameData.Serializer.Serializer.Serialize(value.GetInfluencePower(), dataPool);
		case 5:
			return GameData.Serializer.Serializer.Serialize(value.GetInfluencePowerBonus(), dataPool);
		default:
			if (fieldId >= 6)
			{
				throw new Exception($"Unsupported fieldId {fieldId}");
			}
			throw new Exception($"Not allow to get readonly field data: {fieldId}");
		}
	}

	public void SetElementField_SectCharacters(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		if (!_sectCharacters.TryGetValue(objectId, out var value))
		{
			throw new Exception($"Failed to find element {objectId} with field {fieldId}");
		}
		switch (fieldId)
		{
		case 0:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 1:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 2:
		{
			short item2 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item2);
			value.SetSettlementId(item2, context);
			return;
		}
		case 3:
		{
			bool item = false;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item);
			value.SetApprovedTaiwu(item, context);
			return;
		}
		case 4:
		{
			short item4 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item4);
			value.SetInfluencePower(item4, context);
			return;
		}
		case 5:
		{
			short item3 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item3);
			value.SetInfluencePowerBonus(item3, context);
			return;
		}
		}
		if (fieldId >= 6)
		{
			throw new Exception($"Unsupported fieldId {fieldId}");
		}
		if (fieldId >= 6)
		{
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		}
		throw new Exception($"Not allow to set cache field data: {fieldId}");
	}

	private int CheckModified_SectCharacters(int objectId, ushort fieldId, RawDataPool dataPool)
	{
		if (!_sectCharacters.TryGetValue(objectId, out var value))
		{
			return -1;
		}
		if (fieldId >= 6)
		{
			throw new Exception($"Not allow to check readonly field data: {fieldId}");
		}
		if (!_dataStatesSectCharacters.IsModified(value.DataStatesOffset, fieldId))
		{
			return -1;
		}
		_dataStatesSectCharacters.ResetModified(value.DataStatesOffset, fieldId);
		return fieldId switch
		{
			0 => GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool), 
			1 => GameData.Serializer.Serializer.Serialize(value.GetOrgTemplateId(), dataPool), 
			2 => GameData.Serializer.Serializer.Serialize(value.GetSettlementId(), dataPool), 
			3 => GameData.Serializer.Serializer.Serialize(value.GetApprovedTaiwu(), dataPool), 
			4 => GameData.Serializer.Serializer.Serialize(value.GetInfluencePower(), dataPool), 
			5 => GameData.Serializer.Serializer.Serialize(value.GetInfluencePowerBonus(), dataPool), 
			_ => throw new Exception($"Unsupported fieldId {fieldId}"), 
		};
	}

	private void ResetModifiedWrapper_SectCharacters(int objectId, ushort fieldId)
	{
		if (_sectCharacters.TryGetValue(objectId, out var value))
		{
			if (fieldId >= 6)
			{
				throw new Exception($"Not allow to reset modification state of readonly field data: {fieldId}");
			}
			if (_dataStatesSectCharacters.IsModified(value.DataStatesOffset, fieldId))
			{
				_dataStatesSectCharacters.ResetModified(value.DataStatesOffset, fieldId);
			}
		}
	}

	private bool IsModifiedWrapper_SectCharacters(int objectId, ushort fieldId)
	{
		if (!_sectCharacters.TryGetValue(objectId, out var value))
		{
			return false;
		}
		if (fieldId >= 6)
		{
			throw new Exception($"Not allow to check modification state of readonly field data: {fieldId}");
		}
		return _dataStatesSectCharacters.IsModified(value.DataStatesOffset, fieldId);
	}

	public CivilianSettlementCharacter GetElement_CivilianSettlementCharacters(int objectId)
	{
		return _civilianSettlementCharacters[objectId];
	}

	public bool TryGetElement_CivilianSettlementCharacters(int objectId, out CivilianSettlementCharacter element)
	{
		return _civilianSettlementCharacters.TryGetValue(objectId, out element);
	}

	private unsafe void AddElement_CivilianSettlementCharacters(int objectId, CivilianSettlementCharacter instance)
	{
		instance.CollectionHelperData = HelperDataCivilianSettlementCharacters;
		instance.DataStatesOffset = _dataStatesCivilianSettlementCharacters.Create();
		_civilianSettlementCharacters.Add(objectId, instance);
		byte* pData = OperationAdder.FixedObjectCollection_Add(3, 4, objectId, 12);
		instance.Serialize(pData);
	}

	private void RemoveElement_CivilianSettlementCharacters(int objectId)
	{
		if (_civilianSettlementCharacters.TryGetValue(objectId, out var value))
		{
			_dataStatesCivilianSettlementCharacters.Remove(value.DataStatesOffset);
			_civilianSettlementCharacters.Remove(objectId);
			OperationAdder.FixedObjectCollection_Remove(3, 4, objectId);
		}
	}

	private void ClearCivilianSettlementCharacters()
	{
		_dataStatesCivilianSettlementCharacters.Clear();
		_civilianSettlementCharacters.Clear();
		OperationAdder.FixedObjectCollection_Clear(3, 4);
	}

	public int GetElementField_CivilianSettlementCharacters(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
	{
		if (!_civilianSettlementCharacters.TryGetValue(objectId, out var value))
		{
			AdaptableLog.TagWarning("GetElementField_CivilianSettlementCharacters", $"Failed to find element {objectId} with field {fieldId}");
			return -1;
		}
		if (resetModified)
		{
			_dataStatesCivilianSettlementCharacters.ResetModified(value.DataStatesOffset, fieldId);
		}
		switch (fieldId)
		{
		case 0:
			return GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool);
		case 1:
			return GameData.Serializer.Serializer.Serialize(value.GetOrgTemplateId(), dataPool);
		case 2:
			return GameData.Serializer.Serializer.Serialize(value.GetSettlementId(), dataPool);
		case 3:
			return GameData.Serializer.Serializer.Serialize(value.GetApprovedTaiwu(), dataPool);
		case 4:
			return GameData.Serializer.Serializer.Serialize(value.GetInfluencePower(), dataPool);
		case 5:
			return GameData.Serializer.Serializer.Serialize(value.GetInfluencePowerBonus(), dataPool);
		default:
			if (fieldId >= 6)
			{
				throw new Exception($"Unsupported fieldId {fieldId}");
			}
			throw new Exception($"Not allow to get readonly field data: {fieldId}");
		}
	}

	public void SetElementField_CivilianSettlementCharacters(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		if (!_civilianSettlementCharacters.TryGetValue(objectId, out var value))
		{
			throw new Exception($"Failed to find element {objectId} with field {fieldId}");
		}
		switch (fieldId)
		{
		case 0:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 1:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 2:
		{
			short item2 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item2);
			value.SetSettlementId(item2, context);
			return;
		}
		case 3:
		{
			bool item = false;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item);
			value.SetApprovedTaiwu(item, context);
			return;
		}
		case 4:
		{
			short item4 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item4);
			value.SetInfluencePower(item4, context);
			return;
		}
		case 5:
		{
			short item3 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item3);
			value.SetInfluencePowerBonus(item3, context);
			return;
		}
		}
		if (fieldId >= 6)
		{
			throw new Exception($"Unsupported fieldId {fieldId}");
		}
		if (fieldId >= 6)
		{
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		}
		throw new Exception($"Not allow to set cache field data: {fieldId}");
	}

	private int CheckModified_CivilianSettlementCharacters(int objectId, ushort fieldId, RawDataPool dataPool)
	{
		if (!_civilianSettlementCharacters.TryGetValue(objectId, out var value))
		{
			return -1;
		}
		if (fieldId >= 6)
		{
			throw new Exception($"Not allow to check readonly field data: {fieldId}");
		}
		if (!_dataStatesCivilianSettlementCharacters.IsModified(value.DataStatesOffset, fieldId))
		{
			return -1;
		}
		_dataStatesCivilianSettlementCharacters.ResetModified(value.DataStatesOffset, fieldId);
		return fieldId switch
		{
			0 => GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool), 
			1 => GameData.Serializer.Serializer.Serialize(value.GetOrgTemplateId(), dataPool), 
			2 => GameData.Serializer.Serializer.Serialize(value.GetSettlementId(), dataPool), 
			3 => GameData.Serializer.Serializer.Serialize(value.GetApprovedTaiwu(), dataPool), 
			4 => GameData.Serializer.Serializer.Serialize(value.GetInfluencePower(), dataPool), 
			5 => GameData.Serializer.Serializer.Serialize(value.GetInfluencePowerBonus(), dataPool), 
			_ => throw new Exception($"Unsupported fieldId {fieldId}"), 
		};
	}

	private void ResetModifiedWrapper_CivilianSettlementCharacters(int objectId, ushort fieldId)
	{
		if (_civilianSettlementCharacters.TryGetValue(objectId, out var value))
		{
			if (fieldId >= 6)
			{
				throw new Exception($"Not allow to reset modification state of readonly field data: {fieldId}");
			}
			if (_dataStatesCivilianSettlementCharacters.IsModified(value.DataStatesOffset, fieldId))
			{
				_dataStatesCivilianSettlementCharacters.ResetModified(value.DataStatesOffset, fieldId);
			}
		}
	}

	private bool IsModifiedWrapper_CivilianSettlementCharacters(int objectId, ushort fieldId)
	{
		if (!_civilianSettlementCharacters.TryGetValue(objectId, out var value))
		{
			return false;
		}
		if (fieldId >= 6)
		{
			throw new Exception($"Not allow to check modification state of readonly field data: {fieldId}");
		}
		return _dataStatesCivilianSettlementCharacters.IsModified(value.DataStatesOffset, fieldId);
	}

	public CharacterSet GetElement_Factions(int elementId)
	{
		return _factions[elementId];
	}

	public bool TryGetElement_Factions(int elementId, out CharacterSet value)
	{
		return _factions.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_Factions(int elementId, CharacterSet value, DataContext context)
	{
		_factions.Add(elementId, value);
		_modificationsFactions.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, DataStates, CacheInfluences, context);
		int serializedSize = value.GetSerializedSize();
		byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(3, 5, elementId, serializedSize);
		ptr += value.Serialize(ptr);
	}

	private unsafe void SetElement_Factions(int elementId, CharacterSet value, DataContext context)
	{
		_factions[elementId] = value;
		_modificationsFactions.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, DataStates, CacheInfluences, context);
		int serializedSize = value.GetSerializedSize();
		byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(3, 5, elementId, serializedSize);
		ptr += value.Serialize(ptr);
	}

	private void RemoveElement_Factions(int elementId, DataContext context)
	{
		_factions.Remove(elementId);
		_modificationsFactions.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(3, 5, elementId);
	}

	private void ClearFactions(DataContext context)
	{
		_factions.Clear();
		_modificationsFactions.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(3, 5);
	}

	private sbyte[] GetLargeSectFavorabilities()
	{
		return _largeSectFavorabilities;
	}

	private unsafe void SetLargeSectFavorabilities(sbyte[] value, DataContext context)
	{
		_largeSectFavorabilities = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(3, 6, 64);
		for (int i = 0; i < 64; i++)
		{
			ptr[i] = (byte)_largeSectFavorabilities[i];
		}
		ptr += 64;
	}

	public List<MartialArtTournamentPreparationInfo> GetMartialArtTournamentPreparationInfoList()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 7))
		{
			return _martialArtTournamentPreparationInfoList;
		}
		List<MartialArtTournamentPreparationInfo> list = new List<MartialArtTournamentPreparationInfo>();
		CalcMartialArtTournamentPreparationInfoList(list);
		bool lockTaken = false;
		try
		{
			_spinLockMartialArtTournamentPreparationInfoList.Enter(ref lockTaken);
			_martialArtTournamentPreparationInfoList.Assign(list);
			BaseGameDataDomain.SetCached(DataStates, 7);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockMartialArtTournamentPreparationInfoList.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _martialArtTournamentPreparationInfoList;
	}

	public List<short> GetPreviousMartialArtTournamentHosts()
	{
		return _previousMartialArtTournamentHosts;
	}

	public unsafe void SetPreviousMartialArtTournamentHosts(List<short> value, DataContext context)
	{
		_previousMartialArtTournamentHosts = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, DataStates, CacheInfluences, context);
		int count = _previousMartialArtTournamentHosts.Count;
		int num = 2 * count;
		int valueSize = 2 + num;
		byte* ptr = OperationAdder.DynamicSingleValue_Set(3, 8, valueSize);
		*(ushort*)ptr = (ushort)count;
		ptr += 2;
		for (int i = 0; i < count; i++)
		{
			((short*)ptr)[i] = _previousMartialArtTournamentHosts[i];
		}
		ptr += num;
	}

	public override void OnInitializeGameDataModule()
	{
		InitializeOnInitializeGameDataModule();
	}

	public unsafe override void OnEnterNewWorld()
	{
		InitializeOnEnterNewWorld();
		InitializeInternalDataOfCollections();
		foreach (KeyValuePair<short, Sect> sect in _sects)
		{
			short key = sect.Key;
			Sect value = sect.Value;
			byte* pData = OperationAdder.DynamicObjectCollection_Add(3, 0, key, value.GetSerializedSize());
			value.Serialize(pData);
		}
		foreach (KeyValuePair<short, CivilianSettlement> civilianSettlement in _civilianSettlements)
		{
			short key2 = civilianSettlement.Key;
			CivilianSettlement value2 = civilianSettlement.Value;
			byte* pData2 = OperationAdder.DynamicObjectCollection_Add(3, 1, key2, value2.GetSerializedSize());
			value2.Serialize(pData2);
		}
		byte* ptr = OperationAdder.FixedSingleValue_Set(3, 2, 2);
		*(short*)ptr = _nextSettlementId;
		ptr += 2;
		foreach (KeyValuePair<int, SectCharacter> sectCharacter in _sectCharacters)
		{
			int key3 = sectCharacter.Key;
			SectCharacter value3 = sectCharacter.Value;
			byte* pData3 = OperationAdder.FixedObjectCollection_Add(3, 3, key3, 12);
			value3.Serialize(pData3);
		}
		foreach (KeyValuePair<int, CivilianSettlementCharacter> civilianSettlementCharacter in _civilianSettlementCharacters)
		{
			int key4 = civilianSettlementCharacter.Key;
			CivilianSettlementCharacter value4 = civilianSettlementCharacter.Value;
			byte* pData4 = OperationAdder.FixedObjectCollection_Add(3, 4, key4, 12);
			value4.Serialize(pData4);
		}
		foreach (KeyValuePair<int, CharacterSet> faction in _factions)
		{
			int key5 = faction.Key;
			CharacterSet value5 = faction.Value;
			int serializedSize = value5.GetSerializedSize();
			byte* ptr2 = OperationAdder.DynamicSingleValueCollection_Add(3, 5, key5, serializedSize);
			ptr2 += value5.Serialize(ptr2);
		}
		byte* ptr3 = OperationAdder.FixedSingleValue_Set(3, 6, 64);
		for (int i = 0; i < 64; i++)
		{
			ptr3[i] = (byte)_largeSectFavorabilities[i];
		}
		ptr3 += 64;
		int count = _previousMartialArtTournamentHosts.Count;
		int num = 2 * count;
		int valueSize = 2 + num;
		byte* ptr4 = OperationAdder.DynamicSingleValue_Set(3, 8, valueSize);
		*(ushort*)ptr4 = (ushort)count;
		ptr4 += 2;
		for (int j = 0; j < count; j++)
		{
			((short*)ptr4)[j] = _previousMartialArtTournamentHosts[j];
		}
		ptr4 += num;
	}

	public override void OnLoadWorld()
	{
		_pendingLoadingOperationIds = new Queue<uint>();
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicObjectCollection_GetAllObjects(3, 0));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicObjectCollection_GetAllObjects(3, 1));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(3, 2));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedObjectCollection_GetAllObjects(3, 3));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedObjectCollection_GetAllObjects(3, 4));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(3, 5));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(3, 6));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValue_Get(3, 8));
	}

	public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
	{
		switch (dataId)
		{
		case 0:
			return GetElementField_Sects((short)subId0, (ushort)subId1, dataPool, resetModified);
		case 1:
			return GetElementField_CivilianSettlements((short)subId0, (ushort)subId1, dataPool, resetModified);
		case 2:
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		case 3:
			return GetElementField_SectCharacters((int)subId0, (ushort)subId1, dataPool, resetModified);
		case 4:
			return GetElementField_CivilianSettlementCharacters((int)subId0, (ushort)subId1, dataPool, resetModified);
		case 5:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 5);
				_modificationsFactions.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<int, CharacterSet>)_factions, dataPool);
		case 6:
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		case 7:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 7);
			}
			return GameData.Serializer.Serializer.Serialize(GetMartialArtTournamentPreparationInfoList(), dataPool);
		case 8:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 8);
			}
			return GameData.Serializer.Serializer.Serialize(_previousMartialArtTournamentHosts, dataPool);
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		switch (dataId)
		{
		case 0:
			SetElementField_Sects((short)subId0, (ushort)subId1, valueOffset, dataPool, context);
			break;
		case 1:
			SetElementField_CivilianSettlements((short)subId0, (ushort)subId1, valueOffset, dataPool, context);
			break;
		case 2:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 3:
			SetElementField_SectCharacters((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
			break;
		case 4:
			SetElementField_CivilianSettlementCharacters((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
			break;
		case 5:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 6:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 7:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 8:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _previousMartialArtTournamentHosts);
			SetPreviousMartialArtTournamentHosts(_previousMartialArtTournamentHosts, context);
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
			int argsCount2 = operation.ArgsCount;
			int num2 = argsCount2;
			if (num2 == 1)
			{
				short item3 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item3);
				SettlementDisplayData displayData = GetDisplayData(item3);
				return GameData.Serializer.Serializer.Serialize(displayData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 1:
		{
			int argsCount18 = operation.ArgsCount;
			int num18 = argsCount18;
			if (num18 == 1)
			{
				List<short> item37 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item37);
				List<SettlementNameRelatedData> settlementNameRelatedData = GetSettlementNameRelatedData(item37);
				return GameData.Serializer.Serializer.Serialize(settlementNameRelatedData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 2:
		{
			int argsCount6 = operation.ArgsCount;
			int num6 = argsCount6;
			if (num6 == 1)
			{
				short item17 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item17);
				List<CharacterDisplayData> settlementMembers = GetSettlementMembers(item17);
				return GameData.Serializer.Serializer.Serialize(settlementMembers, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 3:
		{
			int argsCount12 = operation.ArgsCount;
			int num12 = argsCount12;
			if (num12 == 1)
			{
				sbyte item29 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item29);
				OrganizationCombatSkillsDisplayData organizationCombatSkillsDisplayData = GetOrganizationCombatSkillsDisplayData(item29);
				return GameData.Serializer.Serializer.Serialize(organizationCombatSkillsDisplayData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 4:
		{
			int argsCount21 = operation.ArgsCount;
			int num21 = argsCount21;
			if (num21 == 1)
			{
				sbyte item41 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item41);
				int[] sectPreparationForMartialArtTournament = GetSectPreparationForMartialArtTournament(item41);
				return GameData.Serializer.Serializer.Serialize(sectPreparationForMartialArtTournament, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 5:
			if (operation.ArgsCount == 0)
			{
				short martialArtTournamentCurrentHostSettlementId = GetMartialArtTournamentCurrentHostSettlementId();
				return GameData.Serializer.Serializer.Serialize(martialArtTournamentCurrentHostSettlementId, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 6:
			if (operation.ArgsCount == 0)
			{
				GmCmd_SetAllSettlementInformationVisited(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 7:
			if (operation.ArgsCount == 0)
			{
				List<List<CharacterDisplayData>> item34 = GmCmd_GetAllFactionMembers();
				return GameData.Serializer.Serializer.Serialize(item34, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 8:
		{
			int argsCount8 = operation.ArgsCount;
			int num8 = argsCount8;
			if (num8 == 2)
			{
				short item23 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item23);
				short item24 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item24);
				short settlementIdByAreaIdAndBlockId = GetSettlementIdByAreaIdAndBlockId(item23, item24);
				return GameData.Serializer.Serializer.Serialize(settlementIdByAreaIdAndBlockId, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 9:
		{
			int argsCount25 = operation.ArgsCount;
			int num25 = argsCount25;
			if (num25 == 2)
			{
				short item45 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item45);
				short item46 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item46);
				ShortPair cultureByAreaIdAndBlockId = GetCultureByAreaIdAndBlockId(item45, item46);
				return GameData.Serializer.Serializer.Serialize(cultureByAreaIdAndBlockId, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 10:
			if (operation.ArgsCount == 0)
			{
				int item38 = CalcApprovingRateEffectAuthorityGain();
				return GameData.Serializer.Serializer.Serialize(item38, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 11:
		{
			int argsCount14 = operation.ArgsCount;
			int num14 = argsCount14;
			if (num14 == 2)
			{
				short item31 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item31);
				sbyte item32 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item32);
				SettlementTreasuryDisplayData settlementTreasuryDisplayData = GetSettlementTreasuryDisplayData(context, item31, item32);
				return GameData.Serializer.Serializer.Serialize(settlementTreasuryDisplayData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 12:
		{
			int argsCount9 = operation.ArgsCount;
			int num9 = argsCount9;
			if (num9 == 1)
			{
				short item25 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item25);
				SettlementTreasuryRecordCollection settlementTreasuryRecordCollection = GetSettlementTreasuryRecordCollection(context, item25);
				return GameData.Serializer.Serializer.Serialize(settlementTreasuryRecordCollection, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 13:
		{
			int argsCount5 = operation.ArgsCount;
			int num5 = argsCount5;
			if (num5 == 6)
			{
				int item11 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item11);
				List<ItemSourceChange> item12 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item12);
				Inventory item13 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item13);
				Inventory item14 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item14);
				Inventory item15 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item15);
				Inventory item16 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item16);
				ConfirmSettlementTreasuryOperation(context, item11, item12, item13, item14, item15, item16);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 14:
		{
			int argsCount23 = operation.ArgsCount;
			int num23 = argsCount23;
			if (num23 == 1)
			{
				List<InscribedCharacterKey> item43 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item43);
				SetInscribedCharactersForCreation(context, item43);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 15:
		{
			int argsCount20 = operation.ArgsCount;
			int num20 = argsCount20;
			if (num20 == 1)
			{
				short item40 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item40);
				GmCmd_UpdateSettlementTreasury(context, item40);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 16:
		{
			int argsCount16 = operation.ArgsCount;
			int num16 = argsCount16;
			if (num16 == 1)
			{
				short item35 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item35);
				GmCmd_ClearSettlementTreasuryAlertTime(context, item35);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 17:
		{
			int argsCount13 = operation.ArgsCount;
			int num13 = argsCount13;
			if (num13 == 1)
			{
				short item30 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item30);
				GmCmd_ClearSettlementTreasuryItemAndResource(context, item30);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 18:
		{
			int argsCount10 = operation.ArgsCount;
			int num10 = argsCount10;
			if (num10 == 1)
			{
				short item26 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item26);
				GmCmd_ForceUpdateTreasuryGuards(context, item26);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 19:
		{
			int argsCount7 = operation.ArgsCount;
			int num7 = argsCount7;
			if (num7 == 5)
			{
				sbyte item18 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item18);
				int item19 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item19);
				sbyte item20 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item20);
				short item21 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item21);
				int item22 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item22);
				AddSectBounty(context, item18, item19, item20, item21, item22);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 20:
		{
			int argsCount3 = operation.ArgsCount;
			int num3 = argsCount3;
			if (num3 == 5)
			{
				sbyte item4 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item4);
				int item5 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item5);
				sbyte item6 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item6);
				short item7 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item7);
				int item8 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item8);
				AddSectPrisoner(context, item4, item5, item6, item7, item8);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 21:
		{
			int argsCount24 = operation.ArgsCount;
			int num24 = argsCount24;
			if (num24 == 1)
			{
				short item44 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item44);
				SettlementPrisonDisplayData settlementPrisonDisplayData = GetSettlementPrisonDisplayData(context, item44);
				return GameData.Serializer.Serializer.Serialize(settlementPrisonDisplayData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 22:
		{
			int argsCount22 = operation.ArgsCount;
			int num22 = argsCount22;
			if (num22 == 1)
			{
				short item42 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item42);
				SettlementBountyDisplayData settlementBountyDisplayData = GetSettlementBountyDisplayData(item42);
				return GameData.Serializer.Serializer.Serialize(settlementBountyDisplayData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 23:
		{
			int argsCount19 = operation.ArgsCount;
			int num19 = argsCount19;
			if (num19 == 1)
			{
				short item39 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item39);
				SettlementPrisonRecordCollection settlementPrisonRecordCollection = GetSettlementPrisonRecordCollection(context, item39);
				return GameData.Serializer.Serializer.Serialize(settlementPrisonRecordCollection, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 24:
		{
			int argsCount17 = operation.ArgsCount;
			int num17 = argsCount17;
			if (num17 == 1)
			{
				short item36 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item36);
				GmCmd_ForceUpdateInfluencePower(context, item36);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 25:
		{
			int argsCount15 = operation.ArgsCount;
			int num15 = argsCount15;
			if (num15 == 1)
			{
				List<int> item33 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item33);
				SettlementBountyDisplayData bountyCharacterDisplayDataFromCharacterList = GetBountyCharacterDisplayDataFromCharacterList(item33);
				return GameData.Serializer.Serializer.Serialize(bountyCharacterDisplayDataFromCharacterList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 26:
			if (operation.ArgsCount == 0)
			{
				ForceUpdateTaiwuVillager(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 27:
		{
			int argsCount11 = operation.ArgsCount;
			int num11 = argsCount11;
			if (num11 == 1)
			{
				sbyte item27 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item27);
				bool item28 = IsTaiwuSectFugitive(item27);
				return GameData.Serializer.Serializer.Serialize(item28, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 28:
			if (operation.ArgsCount == 0)
			{
				short organizationTemplateIdOfTaiwuLocation = GetOrganizationTemplateIdOfTaiwuLocation();
				return GameData.Serializer.Serializer.Serialize(organizationTemplateIdOfTaiwuLocation, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 29:
			if (operation.ArgsCount == 0)
			{
				List<ItemSourceChange> lastSettlementTreasuryOperationData = GetLastSettlementTreasuryOperationData();
				return GameData.Serializer.Serializer.Serialize(lastSettlementTreasuryOperationData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 30:
		{
			int argsCount4 = operation.ArgsCount;
			int num4 = argsCount4;
			if (num4 == 1)
			{
				int item9 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item9);
				List<CharacterDisplayData> item10 = GmCmd_GetSettlementPrisoner(context, item9);
				return GameData.Serializer.Serializer.Serialize(item10, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 31:
		{
			int argsCount = operation.ArgsCount;
			int num = argsCount;
			if (num == 1)
			{
				short item = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item);
				bool[] item2 = CheckSettlementGuardFavorabilityType(context, item);
				return GameData.Serializer.Serializer.Serialize(item2, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
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
			break;
		case 4:
			break;
		case 5:
			_modificationsFactions.ChangeRecording(monitoring);
			break;
		case 6:
			break;
		case 7:
			break;
		case 8:
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
			return CheckModified_Sects((short)subId0, (ushort)subId1, dataPool);
		case 1:
			return CheckModified_CivilianSettlements((short)subId0, (ushort)subId1, dataPool);
		case 2:
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		case 3:
			return CheckModified_SectCharacters((int)subId0, (ushort)subId1, dataPool);
		case 4:
			return CheckModified_CivilianSettlementCharacters((int)subId0, (ushort)subId1, dataPool);
		case 5:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 5))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 5);
			int result = GameData.Serializer.Serializer.SerializeModifications(_factions, dataPool, _modificationsFactions);
			_modificationsFactions.Reset();
			return result;
		}
		case 6:
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		case 7:
			if (!BaseGameDataDomain.IsModified(DataStates, 7))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 7);
			return GameData.Serializer.Serializer.Serialize(GetMartialArtTournamentPreparationInfoList(), dataPool);
		case 8:
			if (!BaseGameDataDomain.IsModified(DataStates, 8))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 8);
			return GameData.Serializer.Serializer.Serialize(_previousMartialArtTournamentHosts, dataPool);
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
	{
		switch (dataId)
		{
		case 0:
			ResetModifiedWrapper_Sects((short)subId0, (ushort)subId1);
			break;
		case 1:
			ResetModifiedWrapper_CivilianSettlements((short)subId0, (ushort)subId1);
			break;
		case 2:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		case 3:
			ResetModifiedWrapper_SectCharacters((int)subId0, (ushort)subId1);
			break;
		case 4:
			ResetModifiedWrapper_CivilianSettlementCharacters((int)subId0, (ushort)subId1);
			break;
		case 5:
			if (BaseGameDataDomain.IsModified(DataStates, 5))
			{
				BaseGameDataDomain.ResetModified(DataStates, 5);
				_modificationsFactions.Reset();
			}
			break;
		case 6:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		case 7:
			if (BaseGameDataDomain.IsModified(DataStates, 7))
			{
				BaseGameDataDomain.ResetModified(DataStates, 7);
			}
			break;
		case 8:
			if (BaseGameDataDomain.IsModified(DataStates, 8))
			{
				BaseGameDataDomain.ResetModified(DataStates, 8);
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
			0 => IsModifiedWrapper_Sects((short)subId0, (ushort)subId1), 
			1 => IsModifiedWrapper_CivilianSettlements((short)subId0, (ushort)subId1), 
			2 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			3 => IsModifiedWrapper_SectCharacters((int)subId0, (ushort)subId1), 
			4 => IsModifiedWrapper_CivilianSettlementCharacters((int)subId0, (ushort)subId1), 
			5 => BaseGameDataDomain.IsModified(DataStates, 5), 
			6 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			7 => BaseGameDataDomain.IsModified(DataStates, 7), 
			8 => BaseGameDataDomain.IsModified(DataStates, 8), 
			_ => throw new Exception($"Unsupported dataId {dataId}"), 
		};
	}

	public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
	{
		switch (influence.TargetIndicator.DataId)
		{
		case 0:
			if (!unconditionallyInfluenceAll)
			{
				List<BaseGameDataObject> list2 = InfluenceChecker.InfluencedObjectsPool.Get();
				if (!InfluenceChecker.GetScope(context, sourceObject, influence.Scope, _sects, list2))
				{
					int count3 = list2.Count;
					for (int k = 0; k < count3; k++)
					{
						BaseGameDataObject baseGameDataObject2 = list2[k];
						List<DataUid> targetUids2 = influence.TargetUids;
						int count4 = targetUids2.Count;
						for (int l = 0; l < count4; l++)
						{
							baseGameDataObject2.InvalidateSelfAndInfluencedCache((ushort)targetUids2[l].SubId1, context);
						}
					}
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesSects, _dataStatesSects, influence, context);
				}
				list2.Clear();
				InfluenceChecker.InfluencedObjectsPool.Return(list2);
			}
			else
			{
				BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesSects, _dataStatesSects, influence, context);
			}
			break;
		case 1:
			if (!unconditionallyInfluenceAll)
			{
				List<BaseGameDataObject> list4 = InfluenceChecker.InfluencedObjectsPool.Get();
				if (!InfluenceChecker.GetScope(context, sourceObject, influence.Scope, _civilianSettlements, list4))
				{
					int count7 = list4.Count;
					for (int num = 0; num < count7; num++)
					{
						BaseGameDataObject baseGameDataObject4 = list4[num];
						List<DataUid> targetUids4 = influence.TargetUids;
						int count8 = targetUids4.Count;
						for (int num2 = 0; num2 < count8; num2++)
						{
							baseGameDataObject4.InvalidateSelfAndInfluencedCache((ushort)targetUids4[num2].SubId1, context);
						}
					}
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesCivilianSettlements, _dataStatesCivilianSettlements, influence, context);
				}
				list4.Clear();
				InfluenceChecker.InfluencedObjectsPool.Return(list4);
			}
			else
			{
				BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesCivilianSettlements, _dataStatesCivilianSettlements, influence, context);
			}
			break;
		case 3:
			if (!unconditionallyInfluenceAll)
			{
				List<BaseGameDataObject> list3 = InfluenceChecker.InfluencedObjectsPool.Get();
				if (!InfluenceChecker.GetScope(context, sourceObject, influence.Scope, _sectCharacters, list3))
				{
					int count5 = list3.Count;
					for (int m = 0; m < count5; m++)
					{
						BaseGameDataObject baseGameDataObject3 = list3[m];
						List<DataUid> targetUids3 = influence.TargetUids;
						int count6 = targetUids3.Count;
						for (int n = 0; n < count6; n++)
						{
							baseGameDataObject3.InvalidateSelfAndInfluencedCache((ushort)targetUids3[n].SubId1, context);
						}
					}
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesSectCharacters, _dataStatesSectCharacters, influence, context);
				}
				list3.Clear();
				InfluenceChecker.InfluencedObjectsPool.Return(list3);
			}
			else
			{
				BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesSectCharacters, _dataStatesSectCharacters, influence, context);
			}
			break;
		case 4:
			if (!unconditionallyInfluenceAll)
			{
				List<BaseGameDataObject> list = InfluenceChecker.InfluencedObjectsPool.Get();
				if (!InfluenceChecker.GetScope(context, sourceObject, influence.Scope, _civilianSettlementCharacters, list))
				{
					int count = list.Count;
					for (int i = 0; i < count; i++)
					{
						BaseGameDataObject baseGameDataObject = list[i];
						List<DataUid> targetUids = influence.TargetUids;
						int count2 = targetUids.Count;
						for (int j = 0; j < count2; j++)
						{
							baseGameDataObject.InvalidateSelfAndInfluencedCache((ushort)targetUids[j].SubId1, context);
						}
					}
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesCivilianSettlementCharacters, _dataStatesCivilianSettlementCharacters, influence, context);
				}
				list.Clear();
				InfluenceChecker.InfluencedObjectsPool.Return(list);
			}
			else
			{
				BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesCivilianSettlementCharacters, _dataStatesCivilianSettlementCharacters, influence, context);
			}
			break;
		case 7:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(7, DataStates, CacheInfluences, context);
			break;
		default:
			throw new Exception($"Unsupported dataId {influence.TargetIndicator.DataId}");
		case 2:
		case 5:
		case 6:
		case 8:
			throw new Exception($"Cannot invalidate cache state of non-cache data {influence.TargetIndicator.DataId}");
		}
	}

	public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
	{
		uint num;
		switch (operation.DataId)
		{
		case 0:
			ResponseProcessor.ProcessDynamicObjectCollection(operation, pResult, _sects);
			goto IL_00fd;
		case 1:
			ResponseProcessor.ProcessDynamicObjectCollection(operation, pResult, _civilianSettlements);
			goto IL_00fd;
		case 2:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _nextSettlementId);
			goto IL_00fd;
		case 3:
			ResponseProcessor.ProcessFixedObjectCollection(operation, pResult, _sectCharacters);
			goto IL_00fd;
		case 4:
			ResponseProcessor.ProcessFixedObjectCollection(operation, pResult, _civilianSettlementCharacters);
			goto IL_00fd;
		case 5:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Value<int, CharacterSet>(operation, pResult, (IDictionary<int, CharacterSet>)_factions);
			goto IL_00fd;
		case 6:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Array(operation, pResult, _largeSectFavorabilities, 64);
			goto IL_00fd;
		case 8:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_List(operation, pResult, _previousMartialArtTournamentHosts);
			goto IL_00fd;
		default:
			throw new Exception($"Unsupported dataId {operation.DataId}");
		case 7:
			{
				throw new Exception($"Cannot process archive response of non-archive data {operation.DataId}");
			}
			IL_00fd:
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
					DomainManager.Global.CompleteLoading(3);
				}
			}
			break;
		}
	}

	private void InitializeInternalDataOfCollections()
	{
		foreach (KeyValuePair<short, Sect> sect in _sects)
		{
			Sect value = sect.Value;
			value.CollectionHelperData = HelperDataSects;
			value.DataStatesOffset = _dataStatesSects.Create();
		}
		foreach (KeyValuePair<short, CivilianSettlement> civilianSettlement in _civilianSettlements)
		{
			CivilianSettlement value2 = civilianSettlement.Value;
			value2.CollectionHelperData = HelperDataCivilianSettlements;
			value2.DataStatesOffset = _dataStatesCivilianSettlements.Create();
		}
		foreach (KeyValuePair<int, SectCharacter> sectCharacter in _sectCharacters)
		{
			SectCharacter value3 = sectCharacter.Value;
			value3.CollectionHelperData = HelperDataSectCharacters;
			value3.DataStatesOffset = _dataStatesSectCharacters.Create();
		}
		foreach (KeyValuePair<int, CivilianSettlementCharacter> civilianSettlementCharacter in _civilianSettlementCharacters)
		{
			CivilianSettlementCharacter value4 = civilianSettlementCharacter.Value;
			value4.CollectionHelperData = HelperDataCivilianSettlementCharacters;
			value4.DataStatesOffset = _dataStatesCivilianSettlementCharacters.Create();
		}
	}
}
