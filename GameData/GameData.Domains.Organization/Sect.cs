using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Character.Ai.GeneralAction.TeachRandom;
using GameData.Domains.Character.Ai.PrioritizedAction;
using GameData.Domains.Character.Filters;
using GameData.Domains.Character.Relation;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Information;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization.Display;
using GameData.Domains.Organization.SettlementPrisonRecord;
using GameData.Domains.SpecialEffect;
using GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect;
using GameData.Domains.World.MonthlyEvent;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Organization;

[SerializableGameData(NotForDisplayModule = true)]
public class Sect : Settlement, ISerializableGameData
{
	internal class FixedFieldInfos
	{
		public const uint Id_Offset = 0u;

		public const int Id_Size = 2;

		public const uint OrgTemplateId_Offset = 2u;

		public const int OrgTemplateId_Size = 1;

		public const uint Location_Offset = 3u;

		public const int Location_Size = 4;

		public const uint Culture_Offset = 7u;

		public const int Culture_Size = 2;

		public const uint MaxCulture_Offset = 9u;

		public const int MaxCulture_Size = 2;

		public const uint Safety_Offset = 11u;

		public const int Safety_Size = 2;

		public const uint MaxSafety_Offset = 13u;

		public const int MaxSafety_Size = 2;

		public const uint Population_Offset = 15u;

		public const int Population_Size = 4;

		public const uint MaxPopulation_Offset = 19u;

		public const int MaxPopulation_Size = 4;

		public const uint StandardOnStagePopulation_Offset = 23u;

		public const int StandardOnStagePopulation_Size = 4;

		public const uint ApprovingRateUpperLimitBonus_Offset = 27u;

		public const int ApprovingRateUpperLimitBonus_Size = 2;

		public const uint InfluencePowerUpdateDate_Offset = 29u;

		public const int InfluencePowerUpdateDate_Size = 4;

		public const uint MinSeniorityId_Offset = 33u;

		public const int MinSeniorityId_Size = 2;

		public const uint TaiwuExploreStatus_Offset = 35u;

		public const int TaiwuExploreStatus_Size = 1;

		public const uint SpiritualDebtInteractionOccurred_Offset = 36u;

		public const int SpiritualDebtInteractionOccurred_Size = 1;

		public const uint TaiwuInvestmentForMartialArtTournament_Offset = 37u;

		public const int TaiwuInvestmentForMartialArtTournament_Size = 12;
	}

	[CollectionObjectField(false, true, false, false, false)]
	private short _minSeniorityId;

	[CollectionObjectField(false, true, false, false, false)]
	private List<short> _availableMonasticTitleSuffixIds;

	[CollectionObjectField(false, true, false, false, false)]
	private byte _taiwuExploreStatus;

	[CollectionObjectField(false, true, false, false, false)]
	private bool _spiritualDebtInteractionOccurred;

	[CollectionObjectField(false, true, false, false, false, ArrayElementsCount = 3)]
	private int[] _taiwuInvestmentForMartialArtTournament;

	private int[] _martialArtTournamentPreparations = new int[3];

	public byte PrisonEnteredStatus;

	private SortedList<long, int> _membersSortedByAuthority = new SortedList<long, int>();

	private SettlementPrison _prison;

	public const int FixedSize = 49;

	public const int DynamicCount = 3;

	private SpinLock _spinLock = new SpinLock(enableThreadOwnerTracking: false);

	public sbyte TaskStatus => DomainManager.World.GetSectMainStoryTaskStatus(OrgTemplateId);

	private bool IsPreviousMartialArtTournamentWinner => OrgTemplateId == DomainManager.Extra.GetLastMartialArtTournamentWinner();

	public SettlementPrison Prison
	{
		get
		{
			if (_prison != null)
			{
				return _prison;
			}
			if (!DomainManager.Extra.TryGetElement_SettlementPrisons(Id, out _prison))
			{
				_prison = new SettlementPrison();
			}
			return _prison;
		}
	}

	public Sect(short id, Location location, sbyte orgTemplateId, IRandomSource random)
		: base(id, location, orgTemplateId, random)
	{
		OrganizationItem organizationItem = Config.Organization.Instance[orgTemplateId];
		if (organizationItem.SeniorityGroupId >= 0)
		{
			(short first, short last) seniorityRange = OrganizationDomain.GetSeniorityRange(organizationItem.SeniorityGroupId);
			short item = seniorityRange.first;
			short item2 = seniorityRange.last;
			_minSeniorityId = (short)random.Next((int)item, item2 + 1);
			(short first, short last) monasticTitleSuffixRange = OrganizationDomain.GetMonasticTitleSuffixRange(organizationItem.SeniorityGroupId);
			short item3 = monasticTitleSuffixRange.first;
			short item4 = monasticTitleSuffixRange.last;
			int capacity = item4 - item3 + 1;
			_availableMonasticTitleSuffixIds = new List<short>(capacity);
			for (short num = item3; num <= item4; num++)
			{
				_availableMonasticTitleSuffixIds.Add(num);
			}
		}
		else
		{
			_minSeniorityId = -1;
			_availableMonasticTitleSuffixIds = new List<short>();
		}
		sbyte largeSectIndex = OrganizationDomain.GetLargeSectIndex(orgTemplateId);
		if (largeSectIndex >= 0)
		{
			DomainManager.Organization.OfflineInitializeLargeSectFavorabilities(largeSectIndex, organizationItem.LargeSectFavorabilities);
		}
		_taiwuExploreStatus = 0;
		_taiwuInvestmentForMartialArtTournament = new int[3];
	}

	public int[] GetMartialArtTournamentPreparations()
	{
		return _martialArtTournamentPreparations;
	}

	public static bool CanBeGuard(int charId)
	{
		GameData.Domains.Character.Character element;
		return DomainManager.Character.TryGetElement_Objects(charId, out element) && element.IsInteractableAsIntelligentCharacter() && element.GetAgeGroup() == 2 && !DomainManager.LegendaryBook.IsCharacterLegendaryBookOwnerOrContest(charId);
	}

	public unsafe void UpdateMartialArtTournamentPreparations()
	{
		_membersSortedByAuthority.Clear();
		for (sbyte b = 0; b < 9; b++)
		{
			HashSet<int> members = Members.GetMembers(b);
			foreach (int item in members)
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
				ResourceInts resources = element_Objects.GetResources();
				int num = resources.Items[7];
				_membersSortedByAuthority.Add(((long)num << 32) + item, item);
			}
		}
		int num2 = 0;
		for (int i = 0; i < 10 && i < _membersSortedByCombatPower.Count; i++)
		{
			int index = _membersSortedByCombatPower.Count - i - 1;
			int num3 = (int)(_membersSortedByCombatPower.Keys[index] >> 32);
			num2 += num3;
		}
		_martialArtTournamentPreparations[0] = num2 / 10000;
		int num4 = 0;
		for (int j = 0; j < 10 && j < _membersSortedByAuthority.Count; j++)
		{
			int index2 = _membersSortedByAuthority.Count - j - 1;
			int num5 = (int)(_membersSortedByAuthority.Keys[index2] >> 32);
			num4 += num5;
		}
		_martialArtTournamentPreparations[1] = num4 * GlobalConfig.ResourcesWorth[7] / 10000;
		SettlementLayeredTreasuries treasuries = base.Treasuries;
		_martialArtTournamentPreparations[2] = 0;
		SettlementTreasury[] settlementTreasuries = treasuries.SettlementTreasuries;
		foreach (SettlementTreasury settlementTreasury in settlementTreasuries)
		{
			_martialArtTournamentPreparations[2] += (settlementTreasury.Inventory.GetTotalValue() + settlementTreasury.Resources.GetTotalWorth()) / GlobalConfig.Instance.MartialArtTournamentPreparationValueDivider;
		}
	}

	public void UpdateApprovalOfTaiwu(DataContext context)
	{
		short num = CalcApprovingRate();
		if (num < 900)
		{
			return;
		}
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		for (sbyte b = 0; b < 9; b++)
		{
			IEnumerable<int> enumerable = DomainManager.Character.ExcludeInfant(Members.GetMembers(b));
			foreach (int item in enumerable)
			{
				SettlementCharacter settlementCharacter = DomainManager.Organization.GetSettlementCharacter(item);
				if (!settlementCharacter.GetApprovedTaiwu())
				{
					list.Add(item);
				}
			}
		}
		if (list.Count <= 0)
		{
			ObjectPool<List<int>>.Instance.Return(list);
			return;
		}
		int random = list.GetRandom(context.Random);
		SectCharacter element_SectCharacters = DomainManager.Organization.GetElement_SectCharacters(random);
		element_SectCharacters.SetApprovedTaiwu(context, approve: true);
		DomainManager.Character.TryCreateRelation(context, random, DomainManager.Taiwu.GetTaiwuCharId());
		ObjectPool<List<int>>.Instance.Return(list);
	}

	protected override void RecruitOrCreateLackingMembers(DataContext context)
	{
		List<(int, short)> list = new List<(int, short)>();
		List<short> list2 = ObjectPool<List<short>>.Instance.Get();
		list2.Clear();
		DomainManager.Map.GetSettlementBlocks(Location.AreaId, Location.BlockId, list2);
		List<short> list3 = ObjectPool<List<short>>.Instance.Get();
		list3.Clear();
		DomainManager.Map.GetSettlementBlocksAndAffiliatedBlocks(Location.AreaId, Location.BlockId, list3);
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(Location.AreaId);
		OrganizationItem organizationItem = Config.Organization.Instance[OrgTemplateId];
		int worldPopulationFactor = DomainManager.World.GetWorldPopulationFactor();
		for (sbyte b = 8; b >= 0; b--)
		{
			OrganizationMemberItem organizationMemberItem = OrganizationMember.Instance[organizationItem.Members[b]];
			OrganizationInfo destOrgInfo = new OrganizationInfo(OrgTemplateId, b, principal: true, Id);
			int principalAmount = GetPrincipalAmount(b);
			int num = GetExpectedCoreMemberAmount(organizationMemberItem);
			if (!organizationMemberItem.RestrictPrincipalAmount)
			{
				num = num * worldPopulationFactor / 100;
			}
			int num2 = num - principalAmount;
			if (num2 > 0)
			{
				GetRecruitableCharacters(list, b);
				for (int i = 0; i < num2; i++)
				{
					if (list.Count > 0)
					{
						int randomIndex = RandomUtils.GetRandomIndex(list, context.Random);
						int item = list[randomIndex].Item1;
						GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
						DomainManager.Organization.JoinSect(context, element_Objects, destOrgInfo);
						CollectionUtils.SwapAndRemove(list, randomIndex);
					}
					else
					{
						SettlementMembersCreationInfo settlementMembersCreationInfo = new SettlementMembersCreationInfo(OrgTemplateId, Id, stateTemplateIdByAreaId, Location.AreaId, list2, list3);
						settlementMembersCreationInfo.CoreMemberConfig = organizationMemberItem;
						OrganizationDomain.CreateCoreCharacter(context, settlementMembersCreationInfo);
						settlementMembersCreationInfo.CompleteCreatingCharacters();
					}
				}
				if (num2 > 0)
				{
					AdaptableLog.TagInfo("RecruitOrCreateLackingMembers", $"Recruited Count for {organizationItem.Name}, grade {b}: {num2}");
				}
			}
		}
		ObjectPool<List<short>>.Instance.Return(list2);
		ObjectPool<List<short>>.Instance.Return(list3);
	}

	private void GetRecruitableCharacters(List<(int, short)> weightTable, sbyte grade = 0)
	{
		weightTable.Clear();
		MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(Location.AreaId);
		SettlementInfo[] settlementInfos = element_Areas.SettlementInfos;
		for (int i = 0; i < settlementInfos.Length; i++)
		{
			SettlementInfo settlementInfo = settlementInfos[i];
			if (settlementInfo.SettlementId < 0 || settlementInfo.SettlementId == Id)
			{
				continue;
			}
			Settlement settlement = DomainManager.Organization.GetSettlement(settlementInfo.SettlementId);
			OrganizationItem organizationItem = Config.Organization.Instance[OrgTemplateId];
			OrgMemberCollection members = settlement.GetMembers();
			int num = 100 + AiHelper.PrioritizedActionConstants.CivilianGradeJoinSectChance[grade];
			if (num <= 0)
			{
				continue;
			}
			HashSet<int> members2 = members.GetMembers(grade);
			OrganizationMemberItem organizationMemberItem = OrganizationMember.Instance[organizationItem.Members[grade]];
			if (organizationMemberItem.Gender == -1)
			{
				foreach (int item in members2)
				{
					weightTable.Add((item, (short)num));
				}
				continue;
			}
			foreach (int item2 in members2)
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item2);
				if (element_Objects.GetGender() != organizationMemberItem.Gender)
				{
					continue;
				}
				if (organizationMemberItem.ChildGrade < 0)
				{
					RelatedCharacters relatedCharacters = DomainManager.Character.GetRelatedCharacters(item2);
					if (relatedCharacters.HusbandsAndWives.GetCount() > 0 || relatedCharacters.AdoptiveChildren.GetCount() > 0 || relatedCharacters.BloodChildren.GetCount() > 0 || relatedCharacters.StepChildren.GetCount() > 0)
					{
						continue;
					}
				}
				weightTable.Add((item2, (short)num));
			}
		}
	}

	protected override void OfflineUpdateTreasuryGuards(DataContext context, SettlementLayeredTreasuries treasuries)
	{
		int treasuryGuardCount = GlobalConfig.Instance.TreasuryGuardCount;
		Span<(int, int)> span = stackalloc(int, int)[treasuryGuardCount];
		SpanList<(int, int)> topK = span;
		HashSet<int> hashSet = ObjectPool<HashSet<int>>.Instance.Get();
		hashSet.Clear();
		SettlementTreasury[] settlementTreasuries = treasuries.SettlementTreasuries;
		foreach (SettlementTreasury settlementTreasury in settlementTreasuries)
		{
			foreach (int item2 in settlementTreasury.GuardIds.GetCollection())
			{
				if (DomainManager.Character.TryGetElement_Objects(item2, out var element))
				{
					element.RemoveFeatureGroup(context, 536);
					element.AddFeature(context, 553);
				}
			}
		}
		SettlementTreasury[] settlementTreasuries2 = treasuries.SettlementTreasuries;
		foreach (SettlementTreasury settlementTreasury2 in settlementTreasuries2)
		{
			sbyte grade = GlobalConfig.Instance.SectTreasuryGuardMaxGrade[settlementTreasury2.LayerIndex];
			topK.Clear();
			HashSet<int> members = Members.GetMembers(grade);
			foreach (int item3 in members)
			{
				if (!hashSet.Contains(item3) && CanBeGuard(item3))
				{
					GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item3);
					int num = element_Objects.GetCombatPower();
					if (element_Objects.GetFeatureIds().Contains(553))
					{
						num = num * 2 / 3;
					}
					topK.TryInsertTopK<int>(treasuryGuardCount, item3, num);
				}
			}
			settlementTreasury2.GuardIds.Clear();
			for (int k = 0; k < topK.Count; k++)
			{
				int item = topK[k].Item1;
				settlementTreasury2.GuardIds.Add(item);
				GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(item);
				element_Objects2.RemoveFeature(context, 553);
				short treasuryGuardFeatureId = GetTreasuryGuardFeatureId(settlementTreasury2.LayerIndex);
				element_Objects2.AddFeature(context, treasuryGuardFeatureId);
				if (!DomainManager.Character.TryGetCharacterPrioritizedAction(item, out var action) || action.ActionType != 15)
				{
					DomainManager.Character.StartCharacterPrioritizedAction(context, element_Objects2, new GuardTreasuryAction
					{
						Target = new NpcTravelTarget(Location, PrioritizedActionTypeHelper.GetMaxDurationByPrioritizedActionTemplateId(15))
					});
				}
				hashSet.Add(item);
			}
		}
		ObjectPool<HashSet<int>>.Instance.Return(hashSet);
	}

	public override int GetSupplyLevel()
	{
		if (DomainManager.Organization.IsCreatingSettlements())
		{
			return 2;
		}
		sbyte taskStatus = TaskStatus;
		bool isPreviousMartialArtTournamentWinner = IsPreviousMartialArtTournamentWinner;
		if (1 == 0)
		{
		}
		int num = taskStatus switch
		{
			0 => (!isPreviousMartialArtTournamentWinner) ? 1 : 2, 
			1 => isPreviousMartialArtTournamentWinner ? 3 : 2, 
			2 => isPreviousMartialArtTournamentWinner ? 1 : 0, 
			_ => 1, 
		};
		if (1 == 0)
		{
		}
		return num + base.Treasuries.SupplyLevelAddOn;
	}

	public override SettlementNameRelatedData GetNameRelatedData()
	{
		MapBlockData rootBlock = DomainManager.Map.GetBlock(Location).GetRootBlock();
		return new SettlementNameRelatedData(-1, rootBlock.TemplateId);
	}

	public int CalcBountyAmount(sbyte grade)
	{
		sbyte goodness = Config.Organization.Instance[OrgTemplateId].Goodness;
		if (1 == 0)
		{
		}
		int result = goodness switch
		{
			-1 => (grade + 1) * (grade + 1) * 1200, 
			1 => (grade + 1) * (grade + 1) * 120, 
			0 => (grade + 1) * (grade + 1) * 600, 
			_ => 0, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public void GetXiangshuInfectedBounties(List<SettlementBounty> bounties)
	{
		List<Predicate<GameData.Domains.Character.Character>> list = ObjectPool<List<Predicate<GameData.Domains.Character.Character>>>.Instance.Get();
		List<GameData.Domains.Character.Character> list2 = ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Get();
		List<short> list3 = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		list2.Clear();
		list3.Clear();
		sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(Location.AreaId);
		sbyte stateId = (sbyte)(stateTemplateIdByAreaId - 1);
		DomainManager.Map.GetAllAreaInState(stateId, list3);
		MapCharacterFilter.ParallelFindInfected(list, list2, list3);
		short num = 39;
		PunishmentTypeItem punishmentTypeCfg = PunishmentType.Instance[num];
		sbyte punishmentTypeSeverity = GetPunishmentTypeSeverity(punishmentTypeCfg, includeDefault: true);
		PunishmentSeverityItem punishmentSeverityItem = PunishmentSeverity.Instance[punishmentTypeSeverity];
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		if (taiwu.IsActiveExternalRelationState(2) && taiwu.GetLocation().AreaId == Location.AreaId)
		{
			KidnappedCharacterList kidnappedCharacters = DomainManager.Character.GetKidnappedCharacters(taiwu.GetId());
			foreach (KidnappedCharacter item in kidnappedCharacters.GetCollection())
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item.CharId);
				if (element_Objects.IsCompletelyInfected())
				{
					bounties.Add(new SettlementBounty
					{
						CharId = item.CharId,
						PunishmentSeverity = punishmentTypeSeverity,
						PunishmentType = num,
						ExpireDate = DomainManager.World.GetCurrDate() + punishmentSeverityItem.BountyDuration,
						BountyAmount = CalcBountyAmount(element_Objects.GetOrganizationInfo().Grade)
					});
				}
			}
		}
		foreach (GameData.Domains.Character.Character item2 in list2)
		{
			int id = item2.GetId();
			if (DomainManager.Organization.GetPrisonerSect(id) < 0)
			{
				bounties.Add(new SettlementBounty
				{
					CharId = id,
					PunishmentSeverity = punishmentTypeSeverity,
					PunishmentType = num,
					ExpireDate = DomainManager.World.GetCurrDate() + punishmentSeverityItem.BountyDuration,
					BountyAmount = CalcBountyAmount(item2.GetOrganizationInfo().Grade)
				});
			}
		}
		ObjectPool<List<Predicate<GameData.Domains.Character.Character>>>.Instance.Return(list);
		ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(list2);
		ObjectPool<List<short>>.Instance.Return(list3);
	}

	public bool HasCriminalBounty(int charId)
	{
		return Prison.GetBounty(charId) != null;
	}

	public bool HasEnemySectBounty(int charId)
	{
		if (!DomainManager.Character.TryGetElement_Objects(charId, out var element))
		{
			return false;
		}
		if (!CanHaveBounty(element))
		{
			return false;
		}
		sbyte orgTemplateId = element.GetOrganizationInfo().OrgTemplateId;
		if (!OrganizationDomain.IsLargeSect(orgTemplateId))
		{
			return false;
		}
		return DomainManager.Organization.GetSectFavorability(OrgTemplateId, orgTemplateId) == -1;
	}

	public bool HasEnemyRelationBounty(int charId)
	{
		if (!DomainManager.Character.TryGetElement_Objects(charId, out var element))
		{
			return false;
		}
		if (!CanHaveBounty(element))
		{
			return false;
		}
		if (element.GetOrganizationInfo().SettlementId == Id)
		{
			return false;
		}
		sbyte stateIdByAreaId = DomainManager.Map.GetStateIdByAreaId(Location.AreaId);
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		DomainManager.Map.GetStateSettlementIds(stateIdByAreaId, list, containsMainCity: true, containsSect: true);
		for (int i = 0; i < list.Count; i++)
		{
			short settlementId = list[i];
			Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
			OrgMemberCollection members = settlement.GetMembers();
			for (sbyte b = 3; b < 9; b++)
			{
				HashSet<int> members2 = members.GetMembers(b);
				foreach (int item in members2)
				{
					if (DomainManager.Organization.GetPrisonerSect(item) != OrgTemplateId)
					{
						RelatedCharacters relatedCharacters = DomainManager.Character.GetRelatedCharacters(item);
						HashSet<int> collection = relatedCharacters.Enemies.GetCollection();
						if (collection.Contains(charId))
						{
							ObjectPool<List<short>>.Instance.Return(list);
							return true;
						}
					}
				}
			}
		}
		ObjectPool<List<short>>.Instance.Return(list);
		return false;
	}

	public void GetEnemySectBounties(List<SettlementBounty> bounties)
	{
		short num = 41;
		PunishmentTypeItem punishmentTypeCfg = PunishmentType.Instance[num];
		sbyte punishmentTypeSeverity = GetPunishmentTypeSeverity(punishmentTypeCfg, includeDefault: true);
		int bountyDuration = PunishmentSeverity.Instance[punishmentTypeSeverity].BountyDuration;
		Span<sbyte> span = stackalloc sbyte[15];
		SpanList<sbyte> result = span;
		DomainManager.Organization.GetSectTemplateIdsByFavorability(OrgTemplateId, -1, ref result);
		SpanList<sbyte>.Enumerator enumerator = result.GetEnumerator();
		while (enumerator.MoveNext())
		{
			sbyte current = enumerator.Current;
			Sect sect = (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(current);
			for (sbyte b = 0; b < 9; b++)
			{
				HashSet<int> members = sect.Members.GetMembers(b);
				foreach (int item in members)
				{
					if (DomainManager.Character.TryGetElement_Objects(item, out var element) && CanHaveBounty(element))
					{
						bounties.Add(new SettlementBounty
						{
							CharId = item,
							PunishmentSeverity = punishmentTypeSeverity,
							PunishmentType = num,
							ExpireDate = DomainManager.World.GetCurrDate() + bountyDuration,
							BountyAmount = CalcBountyAmount(b)
						});
					}
				}
			}
		}
	}

	public void GetEnemyRelationBounties(List<SettlementBounty> bounties)
	{
		short num = 42;
		PunishmentTypeItem punishmentTypeCfg = PunishmentType.Instance[num];
		sbyte punishmentTypeSeverity = GetPunishmentTypeSeverity(punishmentTypeCfg, includeDefault: true);
		int bountyDuration = PunishmentSeverity.Instance[punishmentTypeSeverity].BountyDuration;
		for (sbyte b = 3; b < 9; b++)
		{
			HashSet<int> members = Members.GetMembers(b);
			foreach (int item in members)
			{
				if (DomainManager.Organization.GetPrisonerSect(item) == OrgTemplateId)
				{
					continue;
				}
				RelatedCharacters relatedCharacters = DomainManager.Character.GetRelatedCharacters(item);
				HashSet<int> collection = relatedCharacters.Enemies.GetCollection();
				foreach (int item2 in collection)
				{
					if (DomainManager.Character.TryGetElement_Objects(item2, out var element) && CanHaveBounty(element) && element.GetOrganizationInfo().SettlementId != Id)
					{
						bounties.Add(new SettlementBounty
						{
							CharId = item2,
							PunishmentSeverity = punishmentTypeSeverity,
							PunishmentType = num,
							ExpireDate = DomainManager.World.GetCurrDate() + bountyDuration,
							BountyAmount = CalcBountyAmount(element.GetOrganizationInfo().Grade)
						});
					}
				}
			}
		}
	}

	private bool CanHaveBounty(GameData.Domains.Character.Character character)
	{
		return CharacterMatcher.DefValue.CanHaveBounty.Match(character);
	}

	public void AddBounty(DataContext context, GameData.Domains.Character.Character character, sbyte punishmentSeverity, short punishmentType, int duration = -1)
	{
		if (duration < 0)
		{
			duration = PunishmentSeverity.Instance[punishmentSeverity].BountyDuration;
		}
		AdaptableLog.TagInfo(ToString(), $"add bounty on {character} for {duration} months.");
		if (punishmentType < 0)
		{
			PredefinedLog.Show(33, character, PunishmentSeverity.Instance.GetItem(punishmentSeverity)?.Name);
		}
		int id = character.GetId();
		SettlementPrison prison = Prison;
		sbyte interactionGrade = character.GetInteractionGrade();
		SettlementBounty bounty = prison.GetBounty(id);
		if (bounty != null)
		{
			if (bounty.PunishmentSeverity >= punishmentSeverity)
			{
				return;
			}
			bounty.PunishmentSeverity = punishmentSeverity;
			bounty.PunishmentType = punishmentType;
			bounty.ExpireDate = DomainManager.World.GetCurrDate() + duration;
			if (character.GetId() == DomainManager.Taiwu.GetTaiwuCharId())
			{
				DomainManager.Extra.TaiwuWantedResetSectInteracted(context, OrgTemplateId);
			}
		}
		else
		{
			DomainManager.Organization.RegisterSectFugitive(id, OrgTemplateId);
			prison.Bounties.Add(new SettlementBounty
			{
				CharId = id,
				PunishmentSeverity = punishmentSeverity,
				PunishmentType = punishmentType,
				ExpireDate = DomainManager.World.GetCurrDate() + duration,
				BountyAmount = CalcBountyAmount(interactionGrade)
			});
			if (character.GetId() == DomainManager.Taiwu.GetTaiwuCharId())
			{
				DomainManager.Extra.TaiwuWantedResetSectInteracted(context, OrgTemplateId);
			}
		}
		DomainManager.Extra.SetSettlementPrison(context, Id, prison);
	}

	public bool RemoveBounty(DataContext context, int charId)
	{
		SettlementPrison prison = Prison;
		SettlementBounty settlementBounty = prison.OfflineRemoveBounty(charId);
		if (settlementBounty == null)
		{
			return false;
		}
		OnSettlementBountyRemoved(context, settlementBounty);
		DomainManager.Extra.SetSettlementPrison(context, Id, prison);
		return true;
	}

	public void AddPrisoner(DataContext context, GameData.Domains.Character.Character character, short punishmentType)
	{
		PunishmentTypeItem punishmentTypeCfg = PunishmentType.Instance[punishmentType];
		sbyte punishmentTypeSeverity = GetPunishmentTypeSeverity(punishmentTypeCfg, includeDefault: true);
		PunishmentSeverityItem punishmentSeverityItem = PunishmentSeverity.Instance[punishmentTypeSeverity];
		AddPrisoner(context, character, punishmentTypeSeverity, punishmentType, punishmentSeverityItem.PrisonTime);
	}

	public void AddPrisoner(DataContext context, GameData.Domains.Character.Character character, sbyte punishmentSeverity, short punishmentType, int duration = -1)
	{
		Tester.Assert(character.GetKidnapperId() < 0);
		Tester.Assert(character.GetId() != DomainManager.Taiwu.GetTaiwuCharId());
		int id = character.GetId();
		SettlementPrison prison = Prison;
		if (duration < 0)
		{
			PunishmentSeverityItem punishmentSeverityItem = PunishmentSeverity.Instance[punishmentSeverity];
			duration = punishmentSeverityItem.PrisonTime;
		}
		AdaptableLog.TagInfo(ToString(), $"add prisoner {character} for {duration} months.");
		SettlementPrisoner prisoner = prison.GetPrisoner(id);
		if (prisoner != null)
		{
			prisoner.PunishmentSeverity = punishmentSeverity;
			prisoner.PunishmentType = punishmentType;
			prisoner.Duration = duration;
			prisoner.KidnapBeginDate = DomainManager.World.GetCurrDate();
			prisoner.RopeItemKey = new ItemKey(12, 0, GetPrisonRopeTemplateId(punishmentSeverity), -1);
			prisoner.InitialMorality = character.GetBaseMorality();
			DomainManager.Extra.SetSettlementPrison(context, Id, prison);
			return;
		}
		prison.Prisoners.Add(new SettlementPrisoner
		{
			CharId = id,
			PunishmentType = punishmentType,
			PunishmentSeverity = punishmentSeverity,
			Duration = duration,
			KidnapBeginDate = DomainManager.World.GetCurrDate(),
			RopeItemKey = new ItemKey(12, 0, GetPrisonRopeTemplateId(punishmentSeverity), -1),
			InitialMorality = character.GetBaseMorality(),
			SpouseCharId = DomainManager.Character.GetAliveSpouse(id)
		});
		if (DomainManager.Character.TryGetCharacterPrioritizedAction(id, out var _))
		{
			DomainManager.Character.RemoveCharacterPrioritizedAction(context, id);
		}
		DomainManager.Character.LeaveGroup(context, character, bringWards: false);
		DomainManager.Character.GroupMove(context, character, Location);
		DomainManager.Character.RemoveAllKidnappedChars(context, character, isEscaped: true);
		character.DeactivateExternalRelationState(context, 2);
		character.ActiveExternalRelationState(context, 32);
		if (character.IsCompletelyInfected())
		{
			Events.RaiseInfectedCharacterLocationChanged(context, id, character.GetLocation(), Location.Invalid);
		}
		else
		{
			Events.RaiseCharacterLocationChanged(context, id, character.GetLocation(), Location.Invalid);
		}
		character.SetLocation(Location.Invalid, context);
		DomainManager.Organization.RegisterSectPrisoner(id, OrgTemplateId);
		AdaptableLog.Info($"Prisoner {character} added to {ToString()}");
		if (!RemoveBounty(context, id))
		{
			DomainManager.Extra.SetSettlementPrison(context, Id, prison);
		}
	}

	public bool RemovePrisoner(DataContext context, int charId)
	{
		SettlementPrison prison = Prison;
		SettlementPrisoner settlementPrisoner = prison.OfflineRemovePrisoner(charId);
		if (settlementPrisoner == null)
		{
			return false;
		}
		OnSettlementPrisonerRemoved(context, settlementPrisoner);
		DomainManager.Extra.SetSettlementPrison(context, Id, prison);
		return true;
	}

	private void OnSettlementPrisonerRemoved(DataContext context, SettlementPrisoner prisoner)
	{
		if (!DomainManager.Character.TryGetElement_Objects(prisoner.CharId, out var element))
		{
			DomainManager.Organization.UnregisterSectPrisoner(prisoner.CharId);
			return;
		}
		Location validLocation = element.GetValidLocation();
		element.DeactivateExternalRelationState(context, 32);
		if (element.IsCompletelyInfected())
		{
			Events.RaiseInfectedCharacterLocationChanged(context, prisoner.CharId, Location.Invalid, validLocation);
		}
		else
		{
			Events.RaiseCharacterLocationChanged(context, prisoner.CharId, Location.Invalid, validLocation);
		}
		element.SetLocation(validLocation, context);
		DomainManager.Organization.UnregisterSectPrisoner(prisoner.CharId);
		AdaptableLog.Info($"Prisoner {element} removed from {ToString()}.");
	}

	private void OnSettlementBountyRemoved(DataContext context, SettlementBounty bounty)
	{
		DomainManager.Organization.UnregisterSectFugitive(bounty.CharId, OrgTemplateId);
		if (bounty.CharId == DomainManager.Taiwu.GetTaiwuCharId())
		{
			DomainManager.Extra.TaiwuWantedResetSectInteracted(context, OrgTemplateId);
		}
	}

	public short GetPrisonRopeTemplateId(sbyte punishmentSeverity)
	{
		int num = Math.Clamp((punishmentSeverity - 1) * 2, 0, 8);
		return (short)(73 + num);
	}

	public void UpdatePrisonOnAdvanceMonth(DataContext context)
	{
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		SettlementPrisonRecordCollection settlementPrisonRecordCollection = DomainManager.Organization.GetSettlementPrisonRecordCollection(context, Id);
		int currDate = DomainManager.World.GetCurrDate();
		SettlementPrison prison = Prison;
		bool flag = prison.Prisoners.Count > 0;
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		KidnappedTravelData kidnappedTravelData = DomainManager.Extra.GetKidnappedTravelData();
		for (int num = prison.Bounties.Count - 1; num >= 0; num--)
		{
			SettlementBounty settlementBounty = prison.Bounties[num];
			if (settlementBounty.CharId != taiwuCharId || !kidnappedTravelData.Valid)
			{
				GameData.Domains.Character.Character element;
				if (settlementBounty.ExpireDate <= currDate)
				{
					prison.Bounties.RemoveAt(num);
					OnSettlementBountyRemoved(context, settlementBounty);
					flag = true;
				}
				else if (settlementBounty.CurrentHunterId >= 0 && (!DomainManager.Character.TryGetElement_Objects(settlementBounty.CurrentHunterId, out element) || element.IsActiveExternalRelationState(32) || DomainManager.Organization.GetFugitiveBountySect(settlementBounty.CurrentHunterId) >= 0))
				{
					settlementBounty.CurrentHunterId = -1;
					flag = true;
				}
			}
		}
		for (int num2 = prison.Prisoners.Count - 1; num2 >= 0; num2--)
		{
			SettlementPrisoner settlementPrisoner = prison.Prisoners[num2];
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(settlementPrisoner.CharId);
			if (!element_Objects.IsCompletelyInfected())
			{
				ApplyPrisonerPunishmentOnAdvanceMonth(context, settlementPrisoner);
				if (settlementPrisoner.KidnapBeginDate + settlementPrisoner.Duration <= currDate)
				{
					short punishmentType = settlementPrisoner.PunishmentType;
					if ((uint)(punishmentType - 41) <= 1u)
					{
						MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
						monthlyEventCollection.AddSentenceCompleted(settlementPrisoner.CharId, Id);
						continue;
					}
					prison.Prisoners.RemoveAt(num2);
					OnSettlementPrisonerRemoved(context, settlementPrisoner);
					PunishmentSeverityItem punishmentSeverityItem = PunishmentSeverity.Instance[settlementPrisoner.PunishmentSeverity];
					if (punishmentSeverityItem.Expel)
					{
						ExpelSectMember(context, this, element_Objects, settlementPrisoner.PunishmentType, settlementPrisoner.SpouseCharId);
						continue;
					}
					lifeRecordCollection.AddBeReleasedUponCompletionOfASentence(settlementPrisoner.CharId, currDate, Id);
					settlementPrisonRecordCollection.AddBeReleasedUponCompletionOfASentence(currDate, Id, settlementPrisoner.CharId);
					continue;
				}
			}
			int totalResistance = CalcKidnappedCharacterResistance(settlementPrisoner);
			int percentProb = settlementPrisoner.CalcEscapeRate(totalResistance, 0);
			bool flag2 = context.Random.CheckPercentProb(percentProb);
			string text = (flag2 ? "√成功" : "×失败");
			AdaptableLog.Info("逃跑" + text);
			AdaptableLog.Info("");
			if (flag2)
			{
				prison.Prisoners.RemoveAt(num2);
				OnSettlementPrisonerRemoved(context, settlementPrisoner);
				lifeRecordCollection.AddPrisonBreak(settlementPrisoner.CharId, currDate, Id);
				settlementPrisonRecordCollection.AddPrisonBreak(currDate, Id, settlementPrisoner.CharId);
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int dataOffset = secretInformationCollection.AddPrisonBreak(settlementPrisoner.CharId, Location);
				int metaDataId = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
				DomainManager.Information.MakeSecretInformationBroadcastEffect(context, metaDataId, -1);
			}
		}
		if (flag)
		{
			DomainManager.Extra.SetSettlementPrison(context, Id, prison);
		}
		DomainManager.Organization.SetSettlementPrisonRecordCollection(context, Id, settlementPrisonRecordCollection);
	}

	public int CalcKidnappedCharacterResistance(SettlementPrisoner prisoner)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(prisoner.CharId);
		int num = element_Objects.GetBehaviorType() + 1;
		int num2 = prisoner.PunishmentSeverity + 1;
		sbyte grade = element_Objects.GetOrganizationInfo().Grade;
		int num3 = num * (num2 * 5 - grade);
		int num4 = GetPrisonerRelatedGuards(prisoner).Select((Func<int, int>)((int id) => DomainManager.Character.GetElement_Objects(id).GetConsummateLevel())).Prepend(-1).Max();
		if (num4 == -1)
		{
			num4 = GlobalConfig.Instance.GuardConsummateLevel[Math.Clamp((int)prisoner.GetPrisonType(), 0, 2)];
		}
		int num5 = GlobalConfig.Instance.MaxConsummateLevel + element_Objects.GetConsummateLevel() - num4;
		int currDate = DomainManager.World.GetCurrDate();
		int num6 = currDate - prisoner.KidnapBeginDate;
		int num7 = prisoner.Duration - 2 * num6;
		int num8 = num3 + num5 + num7;
		AdaptableLog.Info("");
		AdaptableLog.Info($"{base.OrganizationConfig.Name}关押{element_Objects}，初始值：立场{num} * (罪行{num2} * 5 - 身份{grade}) = {num3}，精纯影响：精纯上限{GlobalConfig.Instance.MaxConsummateLevel} + 囚犯精纯{element_Objects.GetConsummateLevel()} - 守卫平均精纯{num4} = {num5}，时长影响：总时长{prisoner.Duration} - 2 * 已关押时长{num6} = {num7}，共计：{num8}");
		return num8;
	}

	public void ExpelSectMember(DataContext context, Sect sect, GameData.Domains.Character.Character character, short punishmentType, int spouseCharId)
	{
		int id = character.GetId();
		OrganizationInfo organizationInfo = character.GetOrganizationInfo();
		sbyte orgTemplateId = sect.GetOrgTemplateId();
		short id2 = sect.GetId();
		if (organizationInfo.OrgTemplateId == orgTemplateId)
		{
			HashSet<int> relatedCharIds = DomainManager.Character.GetRelatedCharIds(id, 2048);
			HashSet<int> relatedCharIds2 = DomainManager.Character.GetRelatedCharIds(id, 4096);
			HashSet<int> hashSet = ObjectPool<HashSet<int>>.Instance.Get();
			hashSet.Clear();
			hashSet.UnionWith(relatedCharIds);
			foreach (int item in hashSet)
			{
				DomainManager.Character.ChangeRelationType(context, id, item, 2048, 0);
				DomainManager.Character.ChangeRelationType(context, item, id, 4096, 0);
			}
			hashSet.Clear();
			hashSet.UnionWith(relatedCharIds2);
			foreach (int item2 in hashSet)
			{
				DomainManager.Character.ChangeRelationType(context, id, item2, 4096, 0);
				DomainManager.Character.ChangeRelationType(context, item2, id, 2048, 0);
			}
		}
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		if (punishmentType == 20 && spouseCharId >= 0)
		{
			lifeRecordCollection.AddBeImplicatedSectPunishLevel5Expel(character.GetId(), currDate, spouseCharId, id2);
		}
		else
		{
			lifeRecordCollection.AddSectPunishLevel5Expel(character.GetId(), currDate, punishmentType, id2);
		}
		DomainManager.Organization.JoinNearbyVillageTownAsBeggar(context, character, -1);
	}

	private unsafe void ApplyPrisonerPunishmentOnAdvanceMonth(DataContext context, SettlementPrisoner prisoner)
	{
		int charId = prisoner.CharId;
		if (!DomainManager.Character.TryGetElement_Objects(charId, out var element) || element.GetAgeGroup() != 2)
		{
			return;
		}
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		switch (OrgTemplateId)
		{
		case 1:
		{
			HashSet<int> obj5 = context.AdvanceMonthRelatedData.RelatedCharIds.Occupy();
			DomainManager.Character.GetAllRelatedCharIds(charId, obj5);
			foreach (int item2 in obj5)
			{
				if (DomainManager.Character.TryGetElement_Objects(item2, out var element3))
				{
					if (DomainManager.Character.TryGetRelation(item2, charId, out var relation) && relation.Favorability > 0)
					{
						DomainManager.Character.ChangeFavorabilityOptional(context, element3, element, -1000, 5);
					}
					if (DomainManager.Character.TryGetRelation(charId, item2, out var relation2) && relation2.Favorability > 0)
					{
						DomainManager.Character.ChangeFavorabilityOptional(context, element, element3, -1000, 5);
					}
				}
			}
			context.AdvanceMonthRelatedData.RelatedCharIds.Release(ref obj5);
			lifeRecordCollection.AddImprisonedShaoLin(charId, currDate, Id);
			break;
		}
		case 2:
		{
			List<int> obj2 = context.AdvanceMonthRelatedData.TargetCharIdList.Occupy();
			for (sbyte b4 = 0; b4 < 9; b4++)
			{
				HashSet<int> members = Members.GetMembers(b4);
				foreach (int item3 in members)
				{
					if (DomainManager.Character.TryGetElement_Objects(item3, out var element2) && element.CanTeachCombatSkill(element2))
					{
						obj2.Add(item3);
					}
				}
			}
			int randomOrDefault = obj2.GetRandomOrDefault(context.Random, -1);
			context.AdvanceMonthRelatedData.TargetCharIdList.Release(ref obj2);
			if (randomOrDefault >= 0)
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(randomOrDefault);
				List<(short, short)> obj3 = context.AdvanceMonthRelatedData.WeightTable.Occupy();
				element.GetTeachableCombatSkillBookIds(element_Objects, obj3);
				short randomResult = RandomUtils.GetRandomResult(obj3, context.Random);
				context.AdvanceMonthRelatedData.WeightTable.Release(ref obj3);
				SkillBookItem skillBookItem = Config.SkillBook.Instance[randomResult];
				Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(charId);
				ushort readingState = charCombatSkills[skillBookItem.CombatSkillTemplateId].GetReadingState();
				byte b5 = 0;
				while (b5 < 15 && !CombatSkillStateHelper.IsPageRead(readingState, b5))
				{
					b5++;
				}
				CombatSkillShorts combatSkillAttainments = element_Objects.GetCombatSkillAttainments();
				CombatSkillShorts combatSkillQualifications = element_Objects.GetCombatSkillQualifications();
				Personalities personalities = element_Objects.GetPersonalities();
				int taughtNewSkillSuccessRate = GameData.Domains.Character.Character.GetTaughtNewSkillSuccessRate(skillBookItem.Grade, combatSkillQualifications[skillBookItem.CombatSkillType], combatSkillAttainments[skillBookItem.CombatSkillType], personalities[1]);
				byte generatedPageTypes = CombatSkillStateHelper.GeneratePageTypesFromReadingState(context.Random, readingState);
				TeachCombatSkillAction teachCombatSkillAction = new TeachCombatSkillAction
				{
					GeneratedPageTypes = generatedPageTypes,
					InternalIndex = b5,
					SkillTemplateId = skillBookItem.CombatSkillTemplateId,
					Succeed = context.Random.CheckPercentProb(taughtNewSkillSuccessRate)
				};
				teachCombatSkillAction.ApplyChanges(context, element, element_Objects);
				if (teachCombatSkillAction.Succeed)
				{
					prisoner.Duration = Math.Max(0, prisoner.Duration - 1);
					lifeRecordCollection.AddImprisonedEmei1(charId, currDate, Id);
				}
				else
				{
					prisoner.Duration++;
					lifeRecordCollection.AddImprisonedEmei2(charId, currDate, Id);
				}
			}
			break;
		}
		case 3:
			if (element.GetCurrNeili() > 0)
			{
				element.ChangeCurrNeiliWithoutChecking(context, -element.GetMaxNeili() * 3 / 10);
			}
			else
			{
				NeiliAllocation baseNeiliAllocation = element.GetBaseNeiliAllocation();
				for (int k = 0; k < 30; k++)
				{
					byte maxType = baseNeiliAllocation.GetMaxType();
					if (baseNeiliAllocation[maxType] == 0)
					{
						break;
					}
					baseNeiliAllocation[maxType]--;
				}
				element.SetBaseNeiliAllocation(baseNeiliAllocation, context);
			}
			lifeRecordCollection.AddImprisonedBaihua(charId, currDate, Id);
			break;
		case 4:
			element.ChangeDisorderOfQi(context, 500);
			lifeRecordCollection.AddImprisonedWudang(charId, currDate, Id);
			break;
		case 5:
			element.ChangeXiangshuInfection(context, 10);
			lifeRecordCollection.AddImprisonedYuanshan(charId, currDate, Id);
			break;
		case 6:
		{
			List<int> obj = context.AdvanceMonthRelatedData.IntList.Occupy();
			List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills = element.GetLearnedLifeSkills();
			for (int num2 = learnedLifeSkills.Count - 1; num2 >= 0; num2--)
			{
				if (learnedLifeSkills[num2].ReadingState != 0)
				{
					obj.Add(num2);
				}
			}
			Span<byte> span2 = stackalloc byte[5];
			SpanList<byte> spanList2 = span2;
			for (int j = 0; j < 3; j++)
			{
				if (obj.Count <= 0)
				{
					break;
				}
				int random3 = obj.GetRandom(context.Random);
				GameData.Domains.Character.LifeSkillItem value = learnedLifeSkills[random3];
				bool flag2 = value.IsAllPagesRead();
				spanList2.Clear();
				for (byte b3 = 0; b3 < 5; b3++)
				{
					if (value.IsPageRead(b3))
					{
						spanList2.Add(b3);
					}
				}
				byte random4 = spanList2.GetRandom(context.Random);
				value.SetPageUnread(random4);
				learnedLifeSkills[random3] = value;
				if (value.GetReadPagesCount() == 0)
				{
					obj.Remove(random3);
				}
				if (value.IsAllPagesRead() != flag2 && flag2)
				{
					Config.LifeSkillItem item = LifeSkill.Instance.GetItem(value.SkillTemplateId);
					short informationTemplateId = Config.LifeSkillType.Instance[item.Type].InformationTemplateId;
					DomainManager.Information.DiscardNormalInformation(context, charId, new NormalInformation(informationTemplateId, item.Grade));
				}
			}
			element.SetLearnedLifeSkills(learnedLifeSkills, context);
			context.AdvanceMonthRelatedData.IntList.Release(ref obj);
			lifeRecordCollection.AddImprisonedShingXiang(charId, currDate, Id);
			break;
		}
		case 7:
		{
			if (prisoner.InitialMorality == 0 || element.GetFixedMorality() != short.MaxValue)
			{
				break;
			}
			sbyte behaviorType = GameData.Domains.Character.BehaviorType.GetBehaviorType(prisoner.InitialMorality);
			sbyte behaviorType2 = element.GetBehaviorType();
			switch (behaviorType)
			{
			case 0:
				if (behaviorType2 >= 4)
				{
					return;
				}
				element.ChangeBaseMorality(context, -25);
				break;
			case 1:
				if (behaviorType2 >= 3)
				{
					return;
				}
				element.ChangeBaseMorality(context, -25);
				break;
			case 2:
				if (prisoner.InitialMorality > 0)
				{
					element.ChangeBaseMorality(context, -25);
				}
				else
				{
					element.ChangeBaseMorality(context, 25);
				}
				break;
			case 3:
				if (behaviorType2 <= 1)
				{
					return;
				}
				element.ChangeBaseMorality(context, 25);
				break;
			case 4:
				if (behaviorType2 <= 0)
				{
					return;
				}
				element.ChangeBaseMorality(context, 25);
				break;
			}
			lifeRecordCollection.AddImprisonedRanShan(charId, currDate, Id);
			break;
		}
		case 8:
		{
			if (!DomainManager.World.CheckDateInterval(prisoner.KidnapBeginDate, 6))
			{
				break;
			}
			List<short> featureIds = element.GetFeatureIds();
			List<short> list = ObjectPool<List<short>>.Instance.Get();
			foreach (short item4 in featureIds)
			{
				CharacterFeatureItem config = CharacterFeature.Instance[item4];
				if (config.Degrade() != null)
				{
					list.Add(item4);
				}
			}
			if (list.Count > 0)
			{
				short random = list.GetRandom(context.Random);
				CharacterFeatureItem characterFeatureItem = CharacterFeature.Instance[random].Degrade();
				element.AddFeature(context, characterFeatureItem.TemplateId, removeMutexFeature: true);
				lifeRecordCollection.AddImprisonedXuanNv(charId, currDate, Id);
			}
			ObjectPool<List<short>>.Instance.Return(list);
			break;
		}
		case 9:
			if (DomainManager.World.CheckDateInterval(prisoner.KidnapBeginDate, 3))
			{
				element.ChangeCurrAge(context, 1);
				lifeRecordCollection.AddImprisonedZhuJian(charId, currDate, Id);
			}
			break;
		case 10:
		{
			Span<short> span3 = stackalloc short[6] { 8, 17, 26, 35, 44, 53 };
			sbyte index = (sbyte)context.Random.Next(6);
			short num3 = span3[index];
			element.ApplyEatingItemInstantEffects(context, 8, num3);
			lifeRecordCollection.AddImprisonedKongSang(charId, currDate, Id, 8, num3);
			break;
		}
		case 11:
		{
			NeiliAllocation extraNeiliAllocation = element.GetExtraNeiliAllocation();
			byte maxType2 = extraNeiliAllocation.GetMaxType();
			int num4 = extraNeiliAllocation[maxType2];
			if (num4 > 0)
			{
				short delta = (short)(-Math.Min(num4, 5));
				element.ChangeExtraNeiliAllocation(context, maxType2, delta);
				lifeRecordCollection.AddImprisonedJinGang(charId, currDate, Id);
			}
			break;
		}
		case 12:
		{
			int kidnapBeginDate = prisoner.KidnapBeginDate;
			if ((currDate - kidnapBeginDate) % 6 != 0)
			{
				break;
			}
			EatingItems eatingItems = element.GetEatingItems();
			Span<sbyte> span = stackalloc sbyte[8];
			SpanList<sbyte> spanList = span;
			for (sbyte b2 = 0; b2 < 8; b2++)
			{
				spanList.Add(b2);
			}
			for (int i = 0; i < 9; i++)
			{
				ItemKey itemKey = (ItemKey)eatingItems.ItemKeys[i];
				if (EatingItems.IsWug(itemKey))
				{
					spanList.Remove(Config.Medicine.Instance[itemKey.TemplateId].WugType);
				}
			}
			if (spanList.Count != 0)
			{
				sbyte random2 = spanList.GetRandom(context.Random);
				short wugTemplateId = ItemDomain.GetWugTemplateId(random2, 4);
				element.AddWug(context, wugTemplateId);
				lifeRecordCollection.AddImprisonedWuXian(charId, currDate, Id, 8, wugTemplateId);
			}
			break;
		}
		case 13:
		{
			OrgMemberCollection members2 = GetMembers();
			List<int> obj4 = context.AdvanceMonthRelatedData.TargetCharIdList.Occupy();
			HashSet<int> members3 = members2.GetMembers(element.GetInteractionGrade());
			foreach (int item5 in members3)
			{
				GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(item5);
				if (element_Objects2.IsInteractableAsIntelligentCharacter())
				{
					obj4.Add(item5);
				}
			}
			if (obj4.Count > 0)
			{
				int random5 = obj4.GetRandom(context.Random);
				GameData.Domains.Character.Character element_Objects3 = DomainManager.Character.GetElement_Objects(random5);
				AiHelper.NpcCombatResultType npcCombatResultType = DomainManager.Character.SimulateCharacterCombat(context, element, element_Objects3, CombatType.Beat, isGroupCombat: false);
				if ((uint)npcCombatResultType <= 1u)
				{
					prisoner.Duration = Math.Max(0, prisoner.Duration - 1);
					lifeRecordCollection.AddImprisonedJieQing1(charId, currDate, Id);
				}
				else
				{
					prisoner.Duration++;
					lifeRecordCollection.AddImprisonedJieQing2(charId, currDate, Id);
				}
			}
			context.AdvanceMonthRelatedData.TargetCharIdList.Release(ref obj4);
			break;
		}
		case 14:
			element.ChangeHealth(context, -24);
			lifeRecordCollection.AddImprisonedFuLong(charId, currDate, Id);
			break;
		case 15:
		{
			sbyte b = (sbyte)context.Random.Next(7);
			bool flag = context.Random.NextBool();
			element.ChangeInjury(context, b, flag, 1);
			if (context.Random.NextBool())
			{
				short num = (flag ? BreakFeatureHelper.BodyPart2HurtFeature[b] : BreakFeatureHelper.BodyPart2CrashFeature[b]);
				if (!element.GetFeatureIds().Contains(num))
				{
					element.AddFeature(context, num);
					DomainManager.SpecialEffect.Add(context, charId, SpecialEffectDomain.BreakBodyFeatureEffectClassName[num]);
				}
			}
			lifeRecordCollection.AddImprisonedXueHou(charId, currDate, Id, b);
			break;
		}
		}
	}

	public unsafe override void SetCulture(short culture, DataContext context)
	{
		Culture = culture;
		SetModifiedAndInvalidateInfluencedCache(3, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 7u, 2);
			*(short*)ptr = Culture;
			ptr += 2;
		}
	}

	public unsafe override void SetMaxCulture(short maxCulture, DataContext context)
	{
		MaxCulture = maxCulture;
		SetModifiedAndInvalidateInfluencedCache(4, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 9u, 2);
			*(short*)ptr = MaxCulture;
			ptr += 2;
		}
	}

	public unsafe override void SetSafety(short safety, DataContext context)
	{
		Safety = safety;
		SetModifiedAndInvalidateInfluencedCache(5, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 11u, 2);
			*(short*)ptr = Safety;
			ptr += 2;
		}
	}

	public unsafe override void SetMaxSafety(short maxSafety, DataContext context)
	{
		MaxSafety = maxSafety;
		SetModifiedAndInvalidateInfluencedCache(6, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 13u, 2);
			*(short*)ptr = MaxSafety;
			ptr += 2;
		}
	}

	public unsafe override void SetPopulation(int population, DataContext context)
	{
		Population = population;
		SetModifiedAndInvalidateInfluencedCache(7, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 15u, 4);
			*(int*)ptr = Population;
			ptr += 4;
		}
	}

	public unsafe override void SetMaxPopulation(int maxPopulation, DataContext context)
	{
		MaxPopulation = maxPopulation;
		SetModifiedAndInvalidateInfluencedCache(8, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 19u, 4);
			*(int*)ptr = MaxPopulation;
			ptr += 4;
		}
	}

	public unsafe override void SetStandardOnStagePopulation(int standardOnStagePopulation, DataContext context)
	{
		StandardOnStagePopulation = standardOnStagePopulation;
		SetModifiedAndInvalidateInfluencedCache(9, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 23u, 4);
			*(int*)ptr = StandardOnStagePopulation;
			ptr += 4;
		}
	}

	public unsafe override void SetMembers(OrgMemberCollection members, DataContext context)
	{
		Members = members;
		SetModifiedAndInvalidateInfluencedCache(10, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = Members.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 0, serializedSize);
			ptr += Members.Serialize(ptr);
		}
	}

	public unsafe override void SetLackingCoreMembers(OrgMemberCollection lackingCoreMembers, DataContext context)
	{
		LackingCoreMembers = lackingCoreMembers;
		SetModifiedAndInvalidateInfluencedCache(11, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = LackingCoreMembers.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 1, serializedSize);
			ptr += LackingCoreMembers.Serialize(ptr);
		}
	}

	public unsafe override void SetApprovingRateUpperLimitBonus(short approvingRateUpperLimitBonus, DataContext context)
	{
		ApprovingRateUpperLimitBonus = approvingRateUpperLimitBonus;
		SetModifiedAndInvalidateInfluencedCache(12, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 27u, 2);
			*(short*)ptr = ApprovingRateUpperLimitBonus;
			ptr += 2;
		}
	}

	public unsafe override void SetInfluencePowerUpdateDate(int influencePowerUpdateDate, DataContext context)
	{
		InfluencePowerUpdateDate = influencePowerUpdateDate;
		SetModifiedAndInvalidateInfluencedCache(13, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 29u, 4);
			*(int*)ptr = InfluencePowerUpdateDate;
			ptr += 4;
		}
	}

	public short GetMinSeniorityId()
	{
		return _minSeniorityId;
	}

	public unsafe void SetMinSeniorityId(short minSeniorityId, DataContext context)
	{
		_minSeniorityId = minSeniorityId;
		SetModifiedAndInvalidateInfluencedCache(14, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 33u, 2);
			*(short*)ptr = _minSeniorityId;
			ptr += 2;
		}
	}

	public List<short> GetAvailableMonasticTitleSuffixIds()
	{
		return _availableMonasticTitleSuffixIds;
	}

	public unsafe void SetAvailableMonasticTitleSuffixIds(List<short> availableMonasticTitleSuffixIds, DataContext context)
	{
		_availableMonasticTitleSuffixIds = availableMonasticTitleSuffixIds;
		SetModifiedAndInvalidateInfluencedCache(15, context);
		if (CollectionHelperData.IsArchive)
		{
			int count = _availableMonasticTitleSuffixIds.Count;
			int num = 2 * count;
			int valueSize = 2 + num;
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 2, valueSize);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = _availableMonasticTitleSuffixIds[i];
			}
			ptr += num;
		}
	}

	public byte GetTaiwuExploreStatus()
	{
		return _taiwuExploreStatus;
	}

	public unsafe void SetTaiwuExploreStatus(byte taiwuExploreStatus, DataContext context)
	{
		_taiwuExploreStatus = taiwuExploreStatus;
		SetModifiedAndInvalidateInfluencedCache(16, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 35u, 1);
			*ptr = _taiwuExploreStatus;
			ptr++;
		}
	}

	public bool GetSpiritualDebtInteractionOccurred()
	{
		return _spiritualDebtInteractionOccurred;
	}

	public unsafe void SetSpiritualDebtInteractionOccurred(bool spiritualDebtInteractionOccurred, DataContext context)
	{
		_spiritualDebtInteractionOccurred = spiritualDebtInteractionOccurred;
		SetModifiedAndInvalidateInfluencedCache(17, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 36u, 1);
			*ptr = (_spiritualDebtInteractionOccurred ? ((byte)1) : ((byte)0));
			ptr++;
		}
	}

	public int[] GetTaiwuInvestmentForMartialArtTournament()
	{
		return _taiwuInvestmentForMartialArtTournament;
	}

	public unsafe void SetTaiwuInvestmentForMartialArtTournament(int[] taiwuInvestmentForMartialArtTournament, DataContext context)
	{
		_taiwuInvestmentForMartialArtTournament = taiwuInvestmentForMartialArtTournament;
		SetModifiedAndInvalidateInfluencedCache(18, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 37u, 12);
			for (int i = 0; i < 3; i++)
			{
				((int*)ptr)[i] = _taiwuInvestmentForMartialArtTournament[i];
			}
			ptr += 12;
		}
	}

	public override short GetApprovingRateUpperLimitTempBonus()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 19))
		{
			return ApprovingRateUpperLimitTempBonus;
		}
		short approvingRateUpperLimitTempBonus = CalcApprovingRateUpperLimitTempBonus();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			ApprovingRateUpperLimitTempBonus = approvingRateUpperLimitTempBonus;
			dataStates.SetCached(DataStatesOffset, 19);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return ApprovingRateUpperLimitTempBonus;
	}

	public Sect()
	{
		Members = new OrgMemberCollection();
		LackingCoreMembers = new OrgMemberCollection();
		_availableMonasticTitleSuffixIds = new List<short>();
		_taiwuInvestmentForMartialArtTournament = new int[3];
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 61;
		int serializedSize = Members.GetSerializedSize();
		num += serializedSize;
		int serializedSize2 = LackingCoreMembers.GetSerializedSize();
		num += serializedSize2;
		int count = _availableMonasticTitleSuffixIds.Count;
		int num2 = 2 * count;
		int num3 = 2 + num2;
		return num + num3;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = Id;
		ptr += 2;
		*ptr = (byte)OrgTemplateId;
		ptr++;
		ptr += Location.Serialize(ptr);
		*(short*)ptr = Culture;
		ptr += 2;
		*(short*)ptr = MaxCulture;
		ptr += 2;
		*(short*)ptr = Safety;
		ptr += 2;
		*(short*)ptr = MaxSafety;
		ptr += 2;
		*(int*)ptr = Population;
		ptr += 4;
		*(int*)ptr = MaxPopulation;
		ptr += 4;
		*(int*)ptr = StandardOnStagePopulation;
		ptr += 4;
		*(short*)ptr = ApprovingRateUpperLimitBonus;
		ptr += 2;
		*(int*)ptr = InfluencePowerUpdateDate;
		ptr += 4;
		*(short*)ptr = _minSeniorityId;
		ptr += 2;
		*ptr = _taiwuExploreStatus;
		ptr++;
		*ptr = (_spiritualDebtInteractionOccurred ? ((byte)1) : ((byte)0));
		ptr++;
		if (_taiwuInvestmentForMartialArtTournament.Length != 3)
		{
			throw new Exception("Elements count of field _taiwuInvestmentForMartialArtTournament is not equal to declaration");
		}
		for (int i = 0; i < 3; i++)
		{
			((int*)ptr)[i] = _taiwuInvestmentForMartialArtTournament[i];
		}
		ptr += 12;
		byte* ptr2 = ptr;
		ptr += 4;
		ptr += Members.Serialize(ptr);
		int num = (int)(ptr - ptr2 - 4);
		if (num > 4194304)
		{
			throw new Exception($"Size of field {"Members"} must be less than {4096}KB");
		}
		*(int*)ptr2 = num;
		byte* ptr3 = ptr;
		ptr += 4;
		ptr += LackingCoreMembers.Serialize(ptr);
		int num2 = (int)(ptr - ptr3 - 4);
		if (num2 > 4194304)
		{
			throw new Exception($"Size of field {"LackingCoreMembers"} must be less than {4096}KB");
		}
		*(int*)ptr3 = num2;
		int count = _availableMonasticTitleSuffixIds.Count;
		int num3 = 2 * count;
		if (num3 > 4194300)
		{
			throw new Exception($"Size of field {"_availableMonasticTitleSuffixIds"} must be less than {4096}KB");
		}
		*(int*)ptr = num3 + 2;
		ptr += 4;
		*(ushort*)ptr = (ushort)count;
		ptr += 2;
		for (int j = 0; j < count; j++)
		{
			((short*)ptr)[j] = _availableMonasticTitleSuffixIds[j];
		}
		ptr += num3;
		return (int)(ptr - pData);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		Id = *(short*)ptr;
		ptr += 2;
		OrgTemplateId = (sbyte)(*ptr);
		ptr++;
		ptr += Location.Deserialize(ptr);
		Culture = *(short*)ptr;
		ptr += 2;
		MaxCulture = *(short*)ptr;
		ptr += 2;
		Safety = *(short*)ptr;
		ptr += 2;
		MaxSafety = *(short*)ptr;
		ptr += 2;
		Population = *(int*)ptr;
		ptr += 4;
		MaxPopulation = *(int*)ptr;
		ptr += 4;
		StandardOnStagePopulation = *(int*)ptr;
		ptr += 4;
		ApprovingRateUpperLimitBonus = *(short*)ptr;
		ptr += 2;
		InfluencePowerUpdateDate = *(int*)ptr;
		ptr += 4;
		_minSeniorityId = *(short*)ptr;
		ptr += 2;
		_taiwuExploreStatus = *ptr;
		ptr++;
		_spiritualDebtInteractionOccurred = *ptr != 0;
		ptr++;
		if (_taiwuInvestmentForMartialArtTournament.Length != 3)
		{
			throw new Exception("Elements count of field _taiwuInvestmentForMartialArtTournament is not equal to declaration");
		}
		for (int i = 0; i < 3; i++)
		{
			_taiwuInvestmentForMartialArtTournament[i] = ((int*)ptr)[i];
		}
		ptr += 12;
		ptr += 4;
		ptr += Members.Deserialize(ptr);
		ptr += 4;
		ptr += LackingCoreMembers.Deserialize(ptr);
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		_availableMonasticTitleSuffixIds.Clear();
		for (int j = 0; j < num; j++)
		{
			_availableMonasticTitleSuffixIds.Add(((short*)ptr)[j]);
		}
		ptr += 2 * num;
		return (int)(ptr - pData);
	}
}
