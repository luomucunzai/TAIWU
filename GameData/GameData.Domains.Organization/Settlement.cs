using System;
using System.Collections.Generic;
using System.Linq;
using CompDevLib.Interpreter;
using Config;
using Config.ConfigCells.Character;
using GameData.Common;
using GameData.Dependencies;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Character.Display;
using GameData.Domains.Character.Relation;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization.Display;
using GameData.Domains.Organization.SettlementTreasuryRecord;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.Taiwu.Profession.SkillsData;
using GameData.Domains.World;
using GameData.Domains.World.Notification;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Organization;

public abstract class Settlement : BaseGameDataObject, IValueSelector
{
	[CollectionObjectField(false, true, false, true, false)]
	protected short Id;

	[CollectionObjectField(false, true, false, true, false)]
	protected sbyte OrgTemplateId;

	[CollectionObjectField(false, true, false, true, false)]
	protected Location Location;

	[CollectionObjectField(false, true, false, false, false)]
	protected short Culture;

	[CollectionObjectField(false, true, false, false, false)]
	protected short MaxCulture;

	[CollectionObjectField(false, true, false, false, false)]
	protected short Safety;

	[CollectionObjectField(false, true, false, false, false)]
	protected short MaxSafety;

	[CollectionObjectField(false, true, false, false, false)]
	protected int Population;

	[CollectionObjectField(false, true, false, false, false)]
	protected int MaxPopulation;

	[CollectionObjectField(false, true, false, false, false)]
	protected int StandardOnStagePopulation;

	[CollectionObjectField(false, true, false, false, false)]
	protected OrgMemberCollection Members;

	[CollectionObjectField(false, true, false, false, false)]
	protected OrgMemberCollection LackingCoreMembers;

	[CollectionObjectField(false, true, false, false, false)]
	protected short ApprovingRateUpperLimitBonus;

	[CollectionObjectField(false, true, false, false, false)]
	protected int InfluencePowerUpdateDate;

	[CollectionObjectField(false, false, true, false, false)]
	protected short ApprovingRateUpperLimitTempBonus;

	protected SortedList<long, int> _membersSortedByCombatPower = new SortedList<long, int>();

	private SettlementLayeredTreasuries _treasuries;

	private readonly Dictionary<ShortPair, List<short>> _supplyItems = new Dictionary<ShortPair, List<short>>();

	private readonly Dictionary<sbyte, List<short>> _supplyBooks = new Dictionary<sbyte, List<short>>();

	public readonly bool[] HasTriggeredAllowEntryEvent = new bool[Enum.GetValues(typeof(SettlementTreasuryLayers)).Length];

	public OrganizationItem OrganizationConfig => Config.Organization.Instance[OrgTemplateId];

	public SettlementLayeredTreasuries Treasuries
	{
		get
		{
			if (_treasuries == null)
			{
				_treasuries = (DomainManager.Extra.TryGetElement_SettlementLayeredTreasuries(Id, out var value) ? value : new SettlementLayeredTreasuries());
			}
			return _treasuries;
		}
	}

	[SingleValueDependency(19, new ushort[] { 30 })]
	protected short CalcApprovingRateUpperLimitTempBonus()
	{
		return DomainManager.Extra.GetMaxApprovingRateTempBonus(Id);
	}

	protected Settlement()
	{
	}

	protected Settlement(short id, Location location, sbyte orgTemplateId, IRandomSource random)
	{
		Id = id;
		OrgTemplateId = orgTemplateId;
		Location = location;
		OrganizationItem organizationItem = Config.Organization.Instance[orgTemplateId];
		(short, short) tuple = CalcCultureAndSafety(organizationItem.Culture, random);
		Culture = tuple.Item1;
		MaxCulture = tuple.Item2;
		tuple = CalcCultureAndSafety(organizationItem.Safety, random);
		Safety = tuple.Item1;
		MaxSafety = tuple.Item2;
		Population = organizationItem.Population;
		MaxPopulation = organizationItem.Population;
		Members = new OrgMemberCollection();
		LackingCoreMembers = new OrgMemberCollection();
	}

	public void ChangeSafety(DataContext context, int delta)
	{
		Safety = (short)Math.Clamp(Safety + delta, 0, MaxSafety);
		SetSafety(Safety, context);
		Events.RaiseSettlementInfoChanged(context, this);
	}

	public void ChangeCulture(DataContext context, int delta)
	{
		Culture = (short)Math.Clamp(Culture + delta, 0, MaxCulture);
		SetCulture(Culture, context);
		Events.RaiseSettlementInfoChanged(context, this);
	}

	public GameData.Domains.Character.Character GetLeader()
	{
		HashSet<int> members = Members.GetMembers(8);
		foreach (int item in members)
		{
			if (DomainManager.Character.TryGetElement_Objects(item, out var element) && element.GetOrganizationInfo().Principal)
			{
				return element;
			}
		}
		return null;
	}

	public int GetMaxSupportingBlockCount()
	{
		return (1 + Culture / 50 + Culture % 50 > 0) ? 1 : 0;
	}

	public sbyte GetPunishmentTypeSeverity(PunishmentTypeItem punishmentTypeCfg, bool includeDefault = false)
	{
		sbyte customizedPunishmentSeverityTemplateId = -1;
		sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(Location.AreaId);
		return DomainManager.Extra.TryGetCustomizedCityPunishmentSeverity(stateTemplateIdByAreaId, this is Sect, punishmentTypeCfg.TemplateId, ref customizedPunishmentSeverityTemplateId) ? customizedPunishmentSeverityTemplateId : punishmentTypeCfg.GetSeverity(stateTemplateIdByAreaId, this is Sect, includeDefault);
	}

	public GameData.Domains.Character.Character GetAvailableHighMember(sbyte startHighGrade, sbyte endLowGrade, bool needAdult = true)
	{
		for (sbyte b = startHighGrade; b >= endLowGrade; b--)
		{
			HashSet<int> members = Members.GetMembers(b);
			foreach (int item in members)
			{
				if (!DomainManager.Character.TryGetElement_Objects(item, out var element) || (needAdult && element.GetAgeGroup() < 2) || element.IsActiveExternalRelationState(60) || element.GetKidnapperId() >= 0)
				{
					continue;
				}
				return element;
			}
		}
		return null;
	}

	public void SortMembersByCombatPower()
	{
		_membersSortedByCombatPower.Clear();
		for (sbyte b = 0; b < 9; b++)
		{
			HashSet<int> members = Members.GetMembers(b);
			foreach (int item in members)
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
				int combatPower = element_Objects.GetCombatPower();
				long key = ((long)combatPower << 32) + item;
				_membersSortedByCombatPower.Add(key, item);
			}
		}
	}

	public long GetRankingInfo(int ranking)
	{
		if (_membersSortedByCombatPower.Count <= ranking)
		{
			return 0L;
		}
		int index = _membersSortedByCombatPower.Count - ranking;
		return _membersSortedByCombatPower.Keys[index];
	}

	public int GetRankingCombatPower(int ranking)
	{
		if (_membersSortedByCombatPower.Count <= ranking)
		{
			return 0;
		}
		int index = _membersSortedByCombatPower.Count - ranking;
		long num = _membersSortedByCombatPower.Keys[index];
		return (int)(num >> 32);
	}

	public int GetCharacterRanking(int charId)
	{
		int num = _membersSortedByCombatPower.IndexOfValue(charId);
		return _membersSortedByCombatPower.Count - num;
	}

	public abstract SettlementNameRelatedData GetNameRelatedData();

	public override string ToString()
	{
		return GetNameRelatedData().GetName();
	}

	public int CalcInfluencePower()
	{
		int num = 0;
		for (sbyte b = 0; b < 9; b++)
		{
			HashSet<int> members = Members.GetMembers(b);
			foreach (int item in members)
			{
				SettlementCharacter settlementCharacter = DomainManager.Organization.GetSettlementCharacter(item);
				num += settlementCharacter.GetInfluencePower();
			}
		}
		return num;
	}

	public short CalcApprovingRate()
	{
		int num = 0;
		for (sbyte b = 0; b < 9; b++)
		{
			HashSet<int> members = Members.GetMembers(b);
			foreach (int item in members)
			{
				SettlementCharacter settlementCharacter = DomainManager.Organization.GetSettlementCharacter(item);
				num += settlementCharacter.GetApprovingRate();
			}
		}
		int val = OrganizationDomain.GetApprovingRateUpperLimit() + ApprovingRateUpperLimitBonus + GetApprovingRateUpperLimitTempBonus();
		val = Math.Min(val, 1000);
		return (short)((val >= 0) ? ((short)Math.Clamp(num, 0, val)) : 0);
	}

	public short CalcApprovingRateTotal()
	{
		int num = 0;
		for (sbyte b = 0; b < 9; b++)
		{
			HashSet<int> members = Members.GetMembers(b);
			foreach (int item in members)
			{
				SettlementCharacter settlementCharacter = DomainManager.Organization.GetSettlementCharacter(item);
				num += settlementCharacter.GetApprovingRate();
			}
		}
		return (short)Math.Clamp(num, 0, 1000);
	}

	public void UpdateMemberGrades(DataContext context)
	{
		if (OrgTemplateId == 16)
		{
			return;
		}
		if (OrgTemplateId == 12)
		{
			UpdateWuxianMemberGrades(context);
			return;
		}
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(8);
		AristocratSkillsData skillsData = professionData.GetSkillsData<AristocratSkillsData>();
		OrganizationItem organizationItem = Config.Organization.Instance[OrgTemplateId];
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int worldPopulationFactor = DomainManager.World.GetWorldPopulationFactor();
		if (organizationItem.Hereditary)
		{
			HashSet<int> hashSet = ObjectPool<HashSet<int>>.Instance.Get();
			for (sbyte b = 8; b > 0; b--)
			{
				HashSet<int> members = LackingCoreMembers.GetMembers(b);
				hashSet.Clear();
				OrganizationInfo orgInfo = new OrganizationInfo(OrgTemplateId, b, principal: true, Id);
				OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(orgInfo);
				HashSet<int> members2 = Members.GetMembers(b);
				int num = ((orgMemberConfig.DeputySpouseDowngrade >= 0) ? GetPrincipalAmount(b) : members2.Count);
				int num2 = GetExpectedCoreMemberAmount(orgMemberConfig);
				if (!orgMemberConfig.RestrictPrincipalAmount)
				{
					num2 = num2 * worldPopulationFactor / 100;
				}
				foreach (int item in members)
				{
					if (orgMemberConfig.Amount > 0 && num >= num2)
					{
						hashSet.UnionWith(members);
						break;
					}
					GetOrganizationMemberPotentialSuccessors(item, orgInfo, list);
					if (list.Count > 0)
					{
						int num3 = skillsData.GetRecommendedCharIdInList(list);
						if (num3 >= 0)
						{
							skillsData.OfflineRemoveRecommendedCharId(num3);
							DomainManager.Extra.SetProfessionData(context, professionData);
						}
						else
						{
							num3 = list.GetRandom(context.Random);
						}
						GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(num3);
						DomainManager.Organization.ChangeGrade(context, element_Objects, b, destPrincipal: true);
						int aliveSpouse = DomainManager.Character.GetAliveSpouse(num3);
						if (aliveSpouse >= 0)
						{
							GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(aliveSpouse);
							DomainManager.Organization.UpdateGradeAccordingToSpouse(context, element_Objects2, element_Objects);
						}
						hashSet.Add(item);
						if (b == 8)
						{
							OnOrganizationLeaderChange(context, num3, element_Objects.GetGender());
						}
						num++;
					}
				}
				members.ExceptWith(hashSet);
				SetLackingCoreMembers(LackingCoreMembers, context);
			}
			ObjectPool<HashSet<int>>.Instance.Return(hashSet);
		}
		else
		{
			for (sbyte b2 = 8; b2 > 0; b2--)
			{
				OrganizationMemberItem orgMemberConfig2 = OrganizationDomain.GetOrgMemberConfig(OrgTemplateId, b2);
				OrganizationInfo orgInfo2 = new OrganizationInfo(OrgTemplateId, b2, principal: true, Id);
				HashSet<int> members3 = Members.GetMembers(b2);
				int num4 = ((orgMemberConfig2.DeputySpouseDowngrade >= 0) ? GetPrincipalAmount(b2) : members3.Count);
				int num5 = GetExpectedCoreMemberAmount(orgMemberConfig2);
				if (!orgMemberConfig2.RestrictPrincipalAmount)
				{
					num5 = num5 * worldPopulationFactor / 100;
				}
				if (num4 < num5)
				{
					int num6 = num5 - num4;
					list.Clear();
					for (int i = 0; i < num6; i++)
					{
						if (list.Count <= 0)
						{
							GetNonHereditaryPotentialSuccessors(orgInfo2, list);
						}
						if (list.Count <= 0)
						{
							break;
						}
						int num7 = skillsData.GetRecommendedCharIdInList(list);
						if (num7 >= 0)
						{
							skillsData.OfflineRemoveRecommendedCharId(num7);
							DomainManager.Extra.SetProfessionData(context, professionData);
							list.Remove(num7);
						}
						else
						{
							int index = context.Random.Next(list.Count);
							num7 = list[index];
							list.RemoveAt(index);
						}
						GameData.Domains.Character.Character element_Objects3 = DomainManager.Character.GetElement_Objects(num7);
						DomainManager.Organization.ChangeGrade(context, element_Objects3, b2, destPrincipal: true);
						int aliveSpouse2 = DomainManager.Character.GetAliveSpouse(num7);
						if (aliveSpouse2 >= 0)
						{
							GameData.Domains.Character.Character element_Objects4 = DomainManager.Character.GetElement_Objects(aliveSpouse2);
							DomainManager.Organization.UpdateGradeAccordingToSpouse(context, element_Objects4, element_Objects3);
						}
						if (b2 == 8)
						{
							OnOrganizationLeaderChange(context, num7, element_Objects3.GetGender());
						}
					}
				}
			}
		}
		RecruitOrCreateLackingMembers(context);
		ObjectPool<List<int>>.Instance.Return(list);
	}

	private void UpdateWuxianMemberGrades(DataContext context)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(8);
		AristocratSkillsData skillsData = professionData.GetSkillsData<AristocratSkillsData>();
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		List<int> list2 = ObjectPool<List<int>>.Instance.Get();
		HashSet<int> hashSet = ObjectPool<HashSet<int>>.Instance.Get();
		list.Clear();
		list2.Clear();
		hashSet.Clear();
		int worldPopulationFactor = DomainManager.World.GetWorldPopulationFactor();
		GetWuxianPotentialSaintessesByHereditary(list2);
		HashSet<int> members = LackingCoreMembers.GetMembers(8);
		OrganizationInfo orgInfo = new OrganizationInfo(OrgTemplateId, 8, principal: true, Id);
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(orgInfo);
		HashSet<int> members2 = Members.GetMembers(8);
		HashSet<int> members3 = Members.GetMembers(7);
		int expectedCoreMemberAmount = GetExpectedCoreMemberAmount(orgMemberConfig);
		if (members2.Count < expectedCoreMemberAmount)
		{
			foreach (int item in members)
			{
				if (list.Count <= 0)
				{
					GetWuxianLeaderPotentialSuccessors(orgInfo, list2, list);
				}
				if (list.Count <= 0)
				{
					break;
				}
				int num = skillsData.GetRecommendedCharIdInList(list);
				if (num >= 0)
				{
					skillsData.OfflineRemoveRecommendedCharId(num);
					DomainManager.Extra.SetProfessionData(context, professionData);
					list.Remove(num);
				}
				else
				{
					int index = context.Random.Next(list.Count);
					num = list[index];
					list.RemoveAt(index);
				}
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(num);
				DomainManager.Organization.ChangeGrade(context, element_Objects, 8, destPrincipal: true);
				OnOrganizationLeaderChange(context, num, element_Objects.GetGender());
				hashSet.Add(item);
			}
			members.ExceptWith(hashSet);
			hashSet.Clear();
		}
		else
		{
			members.Clear();
		}
		orgInfo = new OrganizationInfo(OrgTemplateId, 7, principal: true, Id);
		orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(orgInfo);
		members = LackingCoreMembers.GetMembers(7);
		members2 = Members.GetMembers(7);
		expectedCoreMemberAmount = GetExpectedCoreMemberAmount(orgMemberConfig);
		if (members2.Count < expectedCoreMemberAmount)
		{
			list.Clear();
			foreach (int item2 in members)
			{
				if (list.Count <= 0)
				{
					GetWuxianSaintessesPotentialSuccessors(orgInfo, list2, list);
				}
				if (list.Count <= 0)
				{
					break;
				}
				int num2 = skillsData.GetRecommendedCharIdInList(list);
				if (num2 >= 0)
				{
					skillsData.OfflineRemoveRecommendedCharId(num2);
					DomainManager.Extra.SetProfessionData(context, professionData);
					list.Remove(num2);
				}
				else
				{
					int index2 = context.Random.Next(list.Count);
					num2 = list[index2];
					list.RemoveAt(index2);
				}
				list2.Remove(num2);
				GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(num2);
				DomainManager.Organization.ChangeGrade(context, element_Objects2, 7, destPrincipal: true);
				hashSet.Add(item2);
			}
			members.ExceptWith(hashSet);
			hashSet.Clear();
		}
		else
		{
			members.Clear();
		}
		for (sbyte b = 6; b > 0; b--)
		{
			members = LackingCoreMembers.GetMembers(b);
			orgInfo = new OrganizationInfo(OrgTemplateId, b, principal: true, Id);
			orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(orgInfo);
			members2 = Members.GetMembers(b);
			int num3 = ((orgMemberConfig.DeputySpouseDowngrade >= 0) ? GetPrincipalAmount(b) : members2.Count);
			int num4 = GetExpectedCoreMemberAmount(orgMemberConfig);
			if (!orgMemberConfig.RestrictPrincipalAmount)
			{
				num4 = num4 * worldPopulationFactor / 100;
			}
			hashSet.Clear();
			foreach (int item3 in members)
			{
				if (orgMemberConfig.Amount > 0 && num3 >= num4)
				{
					hashSet.UnionWith(members);
					break;
				}
				GetOrganizationMemberPotentialSuccessors(item3, orgInfo, list);
				if (list.Count > 0)
				{
					int num5 = skillsData.GetRecommendedCharIdInList(list);
					if (num5 >= 0)
					{
						skillsData.OfflineRemoveRecommendedCharId(num5);
						DomainManager.Extra.SetProfessionData(context, professionData);
					}
					else
					{
						num5 = list.GetRandom(context.Random);
					}
					GameData.Domains.Character.Character element_Objects3 = DomainManager.Character.GetElement_Objects(num5);
					DomainManager.Organization.ChangeGrade(context, element_Objects3, b, destPrincipal: true);
					hashSet.Add(item3);
					num3++;
				}
			}
			members.ExceptWith(hashSet);
		}
		RecruitOrCreateLackingMembers(context);
		ObjectPool<List<int>>.Instance.Return(list);
		ObjectPool<List<int>>.Instance.Return(list2);
		ObjectPool<HashSet<int>>.Instance.Return(hashSet);
		SetLackingCoreMembers(LackingCoreMembers, context);
	}

	protected virtual void RecruitOrCreateLackingMembers(DataContext context)
	{
		throw new NotImplementedException();
	}

	public int GetExpectedCoreMemberAmount(OrganizationMemberItem orgMemberCfg)
	{
		OrganizationItem organizationItem = Config.Organization.Instance[OrgTemplateId];
		if (!organizationItem.IsSect || orgMemberCfg.RestrictPrincipalAmount)
		{
			return orgMemberCfg.Amount;
		}
		sbyte sectMainStoryTaskStatus = DomainManager.World.GetSectMainStoryTaskStatus(OrgTemplateId);
		if (1 == 0)
		{
		}
		sbyte result = sectMainStoryTaskStatus switch
		{
			1 => orgMemberCfg.UpAmount, 
			2 => orgMemberCfg.DownAmount, 
			_ => orgMemberCfg.Amount, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public int GetPrincipalAmount(sbyte grade)
	{
		int num = 0;
		HashSet<int> members = Members.GetMembers(grade);
		foreach (int item in members)
		{
			if (DomainManager.Character.TryGetElement_Objects(item, out var element) && element.GetOrganizationInfo().Principal)
			{
				num++;
			}
		}
		return num;
	}

	private void GetWuxianPotentialSaintessesByHereditary(List<int> potentialSaintesses)
	{
		HashSet<int> members = Members.GetMembers(5);
		foreach (int item in members)
		{
			RelatedCharacters relatedCharacters = DomainManager.Character.GetRelatedCharacters(item);
			if (relatedCharacters != null)
			{
				GetSaintessCandidates(relatedCharacters.BloodChildren.GetCollection(), potentialSaintesses);
				GetSaintessCandidates(relatedCharacters.StepChildren.GetCollection(), potentialSaintesses);
				GetSaintessCandidates(relatedCharacters.AdoptiveChildren.GetCollection(), potentialSaintesses);
			}
		}
	}

	private void GetWuxianLeaderPotentialSuccessors(OrganizationInfo orgInfo, List<int> potentialSaintesses, List<int> potentialSuccessors)
	{
		HashSet<int> members = Members.GetMembers(7);
		if (members.Count > 0)
		{
			GetOrganizationMemberPotentialSuccessorsInSet(orgInfo, members, potentialSuccessors);
		}
		if (potentialSuccessors.Count <= 0)
		{
			GetWuxianSaintessesPotentialSuccessors(orgInfo, potentialSaintesses, potentialSuccessors);
		}
	}

	private void GetWuxianSaintessesPotentialSuccessors(OrganizationInfo orgInfo, List<int> potentialSaintesses, List<int> potentialSuccessors)
	{
		GetOrganizationMemberPotentialSuccessorsInSet(orgInfo, potentialSaintesses, potentialSuccessors);
		if (potentialSuccessors.Count > 0)
		{
			return;
		}
		for (sbyte b = 4; b >= 0; b--)
		{
			HashSet<int> members = Members.GetMembers(b);
			GetOrganizationMemberPotentialSuccessorsInSet(orgInfo, members, potentialSuccessors);
			if (potentialSuccessors.Count > 0)
			{
				break;
			}
		}
	}

	private void GetNonHereditaryPotentialSuccessors(OrganizationInfo orgInfo, List<int> potentialSuccessors)
	{
		for (sbyte b = (sbyte)(orgInfo.Grade - 1); b >= 0; b--)
		{
			HashSet<int> members = Members.GetMembers(b);
			GetOrganizationMemberPotentialSuccessorsInSet(orgInfo, members, potentialSuccessors);
			if (potentialSuccessors.Count > 0)
			{
				break;
			}
		}
	}

	private void GetOrganizationMemberPotentialSuccessors(int charId, OrganizationInfo orgInfo, List<int> potentialSuccessors)
	{
		potentialSuccessors.Clear();
		RelatedCharacters relatedCharacters = DomainManager.Character.GetRelatedCharacters(charId);
		if (relatedCharacters == null)
		{
			return;
		}
		GetOrganizationMemberPotentialSuccessorsInSet(orgInfo, relatedCharacters.BloodChildren.GetCollection(), potentialSuccessors);
		if (potentialSuccessors.Count > 0)
		{
			return;
		}
		GetOrganizationMemberPotentialSuccessorsInSet(orgInfo, relatedCharacters.StepChildren.GetCollection(), potentialSuccessors);
		if (potentialSuccessors.Count > 0)
		{
			return;
		}
		GetOrganizationMemberPotentialSuccessorsInSet(orgInfo, relatedCharacters.AdoptiveChildren.GetCollection(), potentialSuccessors);
		if (potentialSuccessors.Count > 0)
		{
			return;
		}
		if (relatedCharacters.HusbandsAndWives.GetCount() > 0)
		{
			foreach (int item in relatedCharacters.HusbandsAndWives.GetCollection())
			{
				if (DomainManager.Character.TryGetElement_Objects(item, out var element))
				{
					OrganizationInfo organizationInfo = element.GetOrganizationInfo();
					if (organizationInfo.OrgTemplateId != orgInfo.OrgTemplateId || organizationInfo.SettlementId != orgInfo.SettlementId || organizationInfo.Grade > orgInfo.Grade || (organizationInfo.Grade == orgInfo.Grade && organizationInfo.Principal))
					{
						break;
					}
					potentialSuccessors.Add(item);
				}
			}
			if (potentialSuccessors.Count > 0)
			{
				return;
			}
		}
		GetOrganizationMemberPotentialSuccessorsInSet(orgInfo, relatedCharacters.BloodBrothersAndSisters.GetCollection(), potentialSuccessors);
		if (potentialSuccessors.Count > 0)
		{
			return;
		}
		GetOrganizationMemberPotentialSuccessorsInSet(orgInfo, relatedCharacters.StepBrothersAndSisters.GetCollection(), potentialSuccessors);
		if (potentialSuccessors.Count > 0)
		{
			return;
		}
		GetOrganizationMemberPotentialSuccessorsInSet(orgInfo, relatedCharacters.AdoptiveBrothersAndSisters.GetCollection(), potentialSuccessors);
		if (potentialSuccessors.Count > 0)
		{
			return;
		}
		GetOrganizationMemberPotentialSuccessorsInSet(orgInfo, relatedCharacters.BloodParents.GetCollection(), potentialSuccessors);
		if (potentialSuccessors.Count > 0)
		{
			return;
		}
		GetOrganizationMemberPotentialSuccessorsInSet(orgInfo, relatedCharacters.StepParents.GetCollection(), potentialSuccessors);
		if (potentialSuccessors.Count <= 0)
		{
			GetOrganizationMemberPotentialSuccessorsInSet(orgInfo, relatedCharacters.AdoptiveParents.GetCollection(), potentialSuccessors);
			if (potentialSuccessors.Count <= 0)
			{
				GetNonHereditaryPotentialSuccessors(orgInfo, potentialSuccessors);
			}
		}
	}

	internal void GetOrganizationMemberPotentialSuccessorsForDisplay(int charId, OrganizationInfo orgInfo, List<int> potentialSuccessors)
	{
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(orgInfo);
		if (orgMemberConfig.TemplateId == 145)
		{
			List<int> list = ObjectPool<List<int>>.Instance.Get();
			list.Clear();
			GetWuxianPotentialSaintessesByHereditary(list);
			GetWuxianLeaderPotentialSuccessors(orgInfo, list, potentialSuccessors);
		}
		else if (orgMemberConfig.TemplateId == 146)
		{
			List<int> list2 = ObjectPool<List<int>>.Instance.Get();
			list2.Clear();
			GetWuxianPotentialSaintessesByHereditary(list2);
			GetWuxianSaintessesPotentialSuccessors(orgInfo, list2, potentialSuccessors);
		}
		else
		{
			OrganizationItem item = Config.Organization.Instance.GetItem(orgInfo.OrgTemplateId);
			if (item != null && item.Hereditary)
			{
				GetOrganizationMemberPotentialSuccessors(charId, orgInfo, potentialSuccessors);
			}
			else
			{
				GetNonHereditaryPotentialSuccessors(orgInfo, potentialSuccessors);
			}
		}
	}

	private static void GetOrganizationMemberPotentialSuccessorsInSet(OrganizationInfo orgInfo, IEnumerable<int> charIds, List<int> result)
	{
		result.Clear();
		int num = -1;
		sbyte gender = OrganizationDomain.GetOrgMemberConfig(orgInfo).Gender;
		foreach (int charId in charIds)
		{
			if (!DomainManager.Character.TryGetElement_Objects(charId, out var element) || !element.IsInteractableAsIntelligentCharacter())
			{
				continue;
			}
			OrganizationInfo organizationInfo = element.GetOrganizationInfo();
			if (orgInfo.SettlementId == organizationInfo.SettlementId && orgInfo.Grade > organizationInfo.Grade && organizationInfo.Principal && (gender == -1 || element.GetGender() == gender))
			{
				SettlementCharacter settlementCharacter = DomainManager.Organization.GetSettlementCharacter(charId);
				short influencePower = settlementCharacter.GetInfluencePower();
				if (influencePower > num)
				{
					result.Clear();
					num = influencePower;
					result.Add(charId);
				}
				else if (influencePower == num)
				{
					result.Add(charId);
				}
			}
		}
	}

	private void GetSaintessCandidates(HashSet<int> charSet, List<int> result)
	{
		foreach (int item in charSet)
		{
			if (DomainManager.Character.TryGetElement_Objects(item, out var element) && element.GetGender() == 0 && element.GetOrganizationInfo().Grade < 6)
			{
				RelatedCharacters relatedCharacters = DomainManager.Character.GetRelatedCharacters(item);
				if (relatedCharacters.AdoptiveChildren.GetCount() <= 0 && relatedCharacters.BloodChildren.GetCount() <= 0 && relatedCharacters.StepChildren.GetCount() <= 0 && relatedCharacters.HusbandsAndWives.GetCount() <= 0 && !result.Contains(item))
				{
					result.Add(item);
				}
			}
		}
	}

	public bool RemoveSettlementFeatures(DataContext context, GameData.Domains.Character.Character character)
	{
		IReadOnlyList<SettlementMemberFeature> settlementMemberFeatures = DomainManager.Extra.GetSettlementMemberFeatures(Id);
		if (settlementMemberFeatures == null)
		{
			return false;
		}
		bool result = false;
		List<short> featureIds = character.GetFeatureIds();
		foreach (SettlementMemberFeature item in settlementMemberFeatures)
		{
			int num = featureIds.IndexOf(item.FeatureId);
			if (num >= 0)
			{
				character.RemoveFeature(context, item.FeatureId);
				result = true;
			}
		}
		return result;
	}

	public bool AddSettlementFeatures(DataContext context, GameData.Domains.Character.Character character)
	{
		IReadOnlyList<SettlementMemberFeature> settlementMemberFeatures = DomainManager.Extra.GetSettlementMemberFeatures(Id);
		if (settlementMemberFeatures == null)
		{
			return false;
		}
		bool flag = false;
		sbyte grade = character.GetOrganizationInfo().Grade;
		foreach (SettlementMemberFeature item in settlementMemberFeatures)
		{
			if (grade >= item.MinGrade && grade <= item.MaxGrade)
			{
				flag |= character.AddFeature(context, item.FeatureId);
			}
		}
		return flag;
	}

	private void OnOrganizationLeaderChange(DataContext context, int charId, sbyte gender)
	{
		Dictionary<int, (GameData.Domains.Character.Character, short)> baseInfluencePowers = new Dictionary<int, (GameData.Domains.Character.Character, short)>();
		HashSet<int> relatedCharIds = new HashSet<int>();
		short influencePowerUpdateInterval = Config.Organization.Instance[OrgTemplateId].InfluencePowerUpdateInterval;
		if (influencePowerUpdateInterval > 0)
		{
			UpdateInfluencePowers(context, baseInfluencePowers, relatedCharIds);
			SetInfluencePowerUpdateDate(DomainManager.World.GetCurrDate() + influencePowerUpdateInterval, context);
		}
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		if (OrganizationDomain.IsSect(OrgTemplateId))
		{
			monthlyNotificationCollection.AddSectUpgrade(charId, Id, OrgTemplateId, 8, orgPrincipal: true, gender);
		}
		else
		{
			monthlyNotificationCollection.AddCivilianSettlementUpgrade(charId, Id, OrgTemplateId, 8, orgPrincipal: true, gender);
		}
	}

	public void UpdateInfluencePowers(DataContext context, Dictionary<int, (GameData.Domains.Character.Character character, short baseInfluencePower)> baseInfluencePowers, HashSet<int> relatedCharIds)
	{
		short morality = ((this is CivilianSettlement civilianSettlement) ? civilianSettlement.UpdateMainMorality(context) : Config.Organization.Instance[OrgTemplateId].MainMorality);
		sbyte behaviorType = GameData.Domains.Character.BehaviorType.GetBehaviorType(morality);
		int num;
		int num2;
		int num3;
		if (!OrganizationDomain.IsSect(OrgTemplateId))
		{
			num = 20;
			num2 = 80;
			num3 = 4000;
		}
		else
		{
			num = 20;
			num2 = 80;
			num3 = 2000;
		}
		baseInfluencePowers.Clear();
		short[] orgCharBaseInfluencePowers = GlobalConfig.Instance.OrgCharBaseInfluencePowers;
		for (sbyte b = 0; b <= 8; b++)
		{
			HashSet<int> members = Members.GetMembers(b);
			SettlementTreasury treasury = GetTreasury(b);
			foreach (int item3 in members)
			{
				short gradeInfluencePower = orgCharBaseInfluencePowers[b];
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item3);
				sbyte behaviorType2 = GameData.Domains.Character.BehaviorType.GetBehaviorType(element_Objects.GetMorality());
				bool principal = element_Objects.GetOrganizationInfo().Principal;
				int num4 = CalcBaseInfluencePower(gradeInfluencePower, behaviorType, behaviorType2, principal);
				num4 = num4 * treasury.CalcBonusInfluencePower(item3) / 100;
				baseInfluencePowers.Add(item3, (element_Objects, (short)((num4 * num + element_Objects.GetCombatPower() * num2 / num3) / 100)));
			}
		}
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(8);
		AristocratSkillsData skillsData = professionData.GetSkillsData<AristocratSkillsData>();
		bool flag = false;
		short num5 = CalcApprovingRate();
		foreach (KeyValuePair<int, (GameData.Domains.Character.Character, short)> baseInfluencePower in baseInfluencePowers)
		{
			int key = baseInfluencePower.Key;
			(GameData.Domains.Character.Character, short) value = baseInfluencePower.Value;
			GameData.Domains.Character.Character item = value.Item1;
			short item2 = value.Item2;
			SettlementCharacter settlementCharacter = DomainManager.Organization.GetSettlementCharacter(key);
			int num6 = settlementCharacter.CalcInfluencePower(item, item2, baseInfluencePowers, relatedCharIds);
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			if (num5 >= 600 && DomainManager.Character.TryGetRelation(key, taiwuCharId, out var relation))
			{
				sbyte favorabilityType = FavorabilityType.GetFavorabilityType(relation.Favorability);
				num6 += num6 * favorabilityType * 5 / 100;
			}
			settlementCharacter.SetInfluencePower((short)Math.Clamp(num6, 0, 32767), context);
			flag = skillsData.OfflineRemoveInfluencePowerBonus(key) || flag;
		}
		UpdateTreasury(context);
		if (flag)
		{
			DomainManager.Extra.SetProfessionData(context, professionData);
		}
	}

	public void UpdateTaiwuVillagerInfluencePowers(DataContext context, Dictionary<int, (GameData.Domains.Character.Character character, short baseInfluencePower)> baseInfluencePowers, HashSet<int> relatedCharIds)
	{
		baseInfluencePowers.Clear();
		short[] orgCharBaseInfluencePowers = GlobalConfig.Instance.OrgCharBaseInfluencePowers;
		for (sbyte b = 0; b <= 8; b++)
		{
			HashSet<int> members = Members.GetMembers(b);
			foreach (int item3 in members)
			{
				short num = orgCharBaseInfluencePowers[b];
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item3);
				baseInfluencePowers.Add(item3, (element_Objects, (short)((num * 20 + element_Objects.GetCombatPower() * 80 / 4000) / 100)));
			}
		}
		foreach (KeyValuePair<int, (GameData.Domains.Character.Character, short)> baseInfluencePower in baseInfluencePowers)
		{
			int key = baseInfluencePower.Key;
			(GameData.Domains.Character.Character, short) value = baseInfluencePower.Value;
			GameData.Domains.Character.Character item = value.Item1;
			short item2 = value.Item2;
			SettlementCharacter settlementCharacter = DomainManager.Organization.GetSettlementCharacter(key);
			int value2 = settlementCharacter.CalcInfluencePower(item, item2, baseInfluencePowers, relatedCharIds);
			settlementCharacter.SetInfluencePower((short)Math.Clamp(value2, 0, 32767), context);
		}
		DomainManager.Taiwu.UpdateTaiwuTreasury(context);
	}

	private static short CalcBaseInfluencePower(short gradeInfluencePower, sbyte mainBehaviorType, sbyte behaviorType, bool principal)
	{
		int num;
		switch (mainBehaviorType - behaviorType)
		{
		default:
			num = 50;
			break;
		case -1:
		case 1:
			num = 75;
			break;
		case 0:
			num = 100;
			break;
		}
		int num2 = num;
		int num3 = gradeInfluencePower * num2 / 100;
		if (!principal)
		{
			num3 /= 2;
		}
		return (short)num3;
	}

	private static (short currValue, short maxValue) CalcCultureAndSafety(short configValue, IRandomSource random)
	{
		int num2;
		if (configValue < 0)
		{
			int num = -configValue;
			num2 = random.Next(num / 2, num + 1) * 5;
		}
		else if (configValue != 0 && random.CheckPercentProb(50))
		{
			int num3 = (1 + random.Next(5)) * 5;
			num2 = configValue + (random.CheckPercentProb(35) ? num3 : (-num3));
			if (num2 < 0)
			{
				num2 = 0;
			}
		}
		else
		{
			num2 = configValue;
		}
		return (currValue: (short)(num2 / 2), maxValue: (short)num2);
	}

	public (int Curr, int Max) GetPopulationInfo()
	{
		short num = WorldCreation.DefValue.WorldPopulation.InfluenceFactors[DomainManager.World.GetWorldPopulationType()];
		int num2 = 0;
		int num3 = 0;
		foreach (int member in Members)
		{
			if (DomainManager.Character.TryGetElement_Objects(member, out var element) && element.GetCreatingType() == 1)
			{
				if ((element.GetDarkAshProtector() & 0xFFFFFFDFu) == 0)
				{
					num2++;
				}
				else
				{
					num3++;
				}
			}
		}
		return (Curr: num2 + num3, Max: (OrganizationConfig.PopulationThreshold != -1) ? (num * OrganizationConfig.PopulationThreshold / 100 + num3) : (-1));
	}

	public bool HasTreasury()
	{
		return OrgTemplateId != 0 && OrgTemplateId != 16 && Location.IsValid();
	}

	public SettlementTreasury GetTreasury(sbyte grade)
	{
		return Treasuries.GetTreasury(GetLayer(grade));
	}

	public int GetMemberSelfImproveSpeedFactor()
	{
		return GlobalConfig.Instance.MemberSelfImproveSpeedFactor[Treasuries.GetTreasuryResourceStatus()];
	}

	public int CalcItemContribution(ItemKey itemKey, int amount)
	{
		short itemSubType = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId);
		int value = DomainManager.Item.GetValue(itemKey);
		sbyte grade = ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId);
		SettlementTreasury treasury = GetTreasury(Treasuries, grade);
		return treasury.CalcAdjustedWorth(itemSubType, value) * amount * GlobalConfig.Instance.ItemContributionPercent / 100;
	}

	public void InitializeTreasurySupplyRequirements()
	{
		SettlementLayeredTreasuries treasuries = Treasuries;
		SettlementTreasury[] settlementTreasuries = treasuries.SettlementTreasuries;
		foreach (SettlementTreasury settlementTreasury in settlementTreasuries)
		{
			(sbyte min, sbyte max) groupGradeRange = Grade.GetGroupGradeRange(settlementTreasury.LayerIndex);
			sbyte item = groupGradeRange.min;
			sbyte item2 = groupGradeRange.max;
			OrganizationItem organizationItem = Config.Organization.Instance[OrgTemplateId];
			for (sbyte b = item; b <= item2; b++)
			{
				short index = organizationItem.Members[b];
				OrganizationMemberItem organizationMemberItem = OrganizationMember.Instance[index];
				foreach (PresetInventoryItem item3 in organizationMemberItem.Inventory)
				{
					if (CanItemBeSupplied(item3.Type, item3.TemplateId, b, out var targetTemplateId))
					{
						ShortPair key = new ShortPair(item3.Type, b);
						_supplyItems.TryAdd(key, new List<short>());
						_supplyItems[key].Add(targetTemplateId);
					}
				}
				PresetEquipmentItemWithProb[] equipment = organizationMemberItem.Equipment;
				for (int j = 0; j < equipment.Length; j++)
				{
					PresetEquipmentItemWithProb presetEquipmentItemWithProb = equipment[j];
					if (CanItemBeSupplied(presetEquipmentItemWithProb.Type, presetEquipmentItemWithProb.TemplateId, b, out var targetTemplateId2))
					{
						ShortPair key2 = new ShortPair(presetEquipmentItemWithProb.Type, b);
						_supplyItems.TryAdd(key2, new List<short>());
						_supplyItems[key2].Add(targetTemplateId2);
					}
				}
			}
		}
		foreach (PresetOrgMemberCombatSkill combatSkill in OrganizationMember.Instance[Config.Organization.Instance[OrgTemplateId].Members[8]].CombatSkills)
		{
			CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[combatSkill.SkillGroupId];
			IReadOnlyList<CombatSkillItem> learnableCombatSkills = CombatSkillDomain.GetLearnableCombatSkills(combatSkillItem.SectId, combatSkillItem.Type);
			foreach (CombatSkillItem item4 in learnableCombatSkills)
			{
				_supplyBooks.TryAdd(item4.Grade, new List<short>());
				_supplyBooks[item4.Grade].Add(item4.BookId);
			}
		}
	}

	public void UpdateTreasuryOnAdvanceMonth(DataContext context)
	{
		if (!HasTreasury())
		{
			return;
		}
		SettlementLayeredTreasuries treasuries = Treasuries;
		treasuries.AlertTime = (byte)Math.Max(0, treasuries.AlertTime - 1);
		SettlementTreasury[] settlementTreasuries = treasuries.SettlementTreasuries;
		foreach (SettlementTreasury settlementTreasury in settlementTreasuries)
		{
			settlementTreasury.ClearMemberUsedPresetContribution();
		}
		SettlementTreasury[] settlementTreasuries2 = treasuries.SettlementTreasuries;
		foreach (SettlementTreasury settlementTreasury2 in settlementTreasuries2)
		{
			GameData.Domains.Character.Character element;
			int[] array = (from x in settlementTreasury2.GuardIds.GetCollection()
				where !DomainManager.Character.TryGetElement_Objects(x, out element) || element.GetCreatingType() != 1
				select x).ToArray();
			foreach (int charId in array)
			{
				settlementTreasury2.GuardIds.Remove(charId);
			}
		}
		if (treasuries.SettlementTreasuries.Any((SettlementTreasury treasury) => treasury.GuardIds.GetCount() < GlobalConfig.Instance.TreasuryGuardCount || treasury.GuardIds.GetCollection().Any((int id) => !Sect.CanBeGuard(id))))
		{
			ForceUpdateTreasuryGuards(context);
		}
		Array.Fill(HasTriggeredAllowEntryEvent, value: false);
		DomainManager.Extra.SetTreasuries(context, Id, treasuries, needUpdateTotalValue: false);
	}

	public void UpdateTreasury(DataContext context)
	{
		if (!HasTreasury())
		{
			return;
		}
		SettlementLayeredTreasuries treasuries = Treasuries;
		int currDate = DomainManager.World.GetCurrDate();
		SettlementTreasuryRecordCollection settlementTreasuryRecordCollection = DomainManager.Organization.GetSettlementTreasuryRecordCollection(context, Id);
		settlementTreasuryRecordCollection.Clear();
		settlementTreasuryRecordCollection.AddClearRecord(currDate, Id);
		settlementTreasuryRecordCollection.AddSupplementResource(currDate, Id);
		settlementTreasuryRecordCollection.AddSupplementItem(currDate, Id);
		SettlementTreasuryLayers layer = GetLayer(0);
		SettlementTreasury treasury = GetTreasury(treasuries, layer);
		OfflineUpdateTreasuryHobbies(context.Random, treasury);
		int num = 0;
		SettlementTreasuryLayers[] values = Enum.GetValues<SettlementTreasuryLayers>();
		foreach (SettlementTreasuryLayers settlementTreasuryLayers in values)
		{
			SettlementTreasury treasury2 = GetTreasury(treasuries, settlementTreasuryLayers);
			treasury2.Contributions.Clear();
			if (settlementTreasuryLayers != layer)
			{
				treasury2.LovingItemSubTypes.Clear();
				treasury2.HatingItemSubTypes.Clear();
				treasury2.LovingItemSubTypes.AddRange(treasury.LovingItemSubTypes);
				treasury2.HatingItemSubTypes.AddRange(treasury.HatingItemSubTypes);
			}
			num += OfflineClearTreasury(context, treasury2);
		}
		treasuries.SupplyLevelAddOn = 0;
		treasuries.ResupplyTotalValue = OfflineResupplyTreasury(context);
		if (treasuries.ResupplyTotalValue * GlobalConfig.Instance.TreasurySupplyLevelUpPercent / 100 < num)
		{
			treasuries.SupplyLevelAddOn = 1;
			treasuries.ResupplyTotalValue = OfflineResupplyTreasury(context);
		}
		OfflineUpdateTreasuryGuards(context, treasuries);
		DomainManager.Extra.SetTreasuries(context, Id, treasuries, needUpdateTotalValue: true);
		DomainManager.Organization.SetSettlementTreasuryRecordCollection(context, Id, settlementTreasuryRecordCollection);
	}

	private void OfflineUpdateTreasuryHobbies(IRandomSource random, SettlementTreasury treasury)
	{
		treasury.LovingItemSubTypes.Clear();
		treasury.HatingItemSubTypes.Clear();
		MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(Location.AreaId);
		MapAreaItem config = element_Areas.GetConfig();
		treasury.LovingItemSubTypes.AddRange(config.LovingItemSubTypes);
		treasury.HatingItemSubTypes.AddRange(config.HatingItemSubTypes);
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		sbyte grade = 9;
		while (grade-- > 6)
		{
			HashSet<int> members = Members.GetMembers(grade);
			int num = int.MinValue;
			list.Clear();
			foreach (int item in members)
			{
				SettlementCharacter settlementCharacter = DomainManager.Organization.GetSettlementCharacter(item);
				short influencePower = settlementCharacter.GetInfluencePower();
				if (influencePower > num)
				{
					num = influencePower;
					list.Clear();
					list.Add(item);
				}
				else if (influencePower == num)
				{
					list.Add(item);
				}
			}
			if (list.Count > 0)
			{
				int random2 = list.GetRandom(random);
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(random2);
				short lovingItemSubType = element_Objects.GetLovingItemSubType();
				short hatingItemSubType = element_Objects.GetHatingItemSubType();
				if (!treasury.LovingItemSubTypes.Contains(lovingItemSubType) && !treasury.HatingItemSubTypes.Contains(lovingItemSubType))
				{
					treasury.LovingItemSubTypes.Add(lovingItemSubType);
				}
				if (!treasury.LovingItemSubTypes.Contains(hatingItemSubType) && !treasury.HatingItemSubTypes.Contains(hatingItemSubType))
				{
					treasury.HatingItemSubTypes.Add(hatingItemSubType);
				}
			}
		}
		ObjectPool<List<int>>.Instance.Return(list);
	}

	private int OfflineClearTreasury(DataContext context, SettlementTreasury treasury)
	{
		int num = 0;
		for (sbyte b = 0; b < 7; b++)
		{
			num += ResourceTypeHelper.ResourceAmountToWorth(b, treasury.Resources[b]);
			treasury.Resources.Set(b, 0);
		}
		num += treasury.Inventory.GetTotalValue();
		foreach (ItemKey key in treasury.Inventory.Items.Keys)
		{
			DomainManager.Item.RemoveItem(context, key);
		}
		treasury.Inventory.Items.Clear();
		return num;
	}

	private int OfflineResupplyTreasury(DataContext context)
	{
		int num = 0;
		SettlementLayeredTreasuries treasuries = Treasuries;
		SettlementTreasuryLayers[] values = Enum.GetValues<SettlementTreasuryLayers>();
		foreach (SettlementTreasuryLayers layer in values)
		{
			num += OfflineResupplyTreasury(context, GetTreasury(treasuries, layer));
		}
		return num;
	}

	private int OfflineResupplyTreasury(DataContext context, SettlementTreasury treasury)
	{
		int num = 0;
		(sbyte min, sbyte max) groupGradeRange = Grade.GetGroupGradeRange(treasury.LayerIndex);
		sbyte item = groupGradeRange.min;
		sbyte item2 = groupGradeRange.max;
		int supplyLevel = GetSupplyLevel();
		int supplyLevelAddOn = Treasuries.SupplyLevelAddOn;
		short[] array3;
		sbyte[] array6;
		if (supplyLevelAddOn > 0)
		{
			short[] array = GlobalConfig.Instance.TreasuryResourceSupplyRanges[supplyLevel];
			short[] array2 = GlobalConfig.Instance.TreasuryResourceSupplyRanges[supplyLevel - supplyLevelAddOn];
			array3 = new short[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array3[i] = (short)(array[i] - array2[i]);
			}
			sbyte[] array4 = GlobalConfig.Instance.TreasuryItemSupplyCounts[supplyLevel];
			sbyte[] array5 = GlobalConfig.Instance.TreasuryItemSupplyCounts[supplyLevel - supplyLevelAddOn];
			array6 = new sbyte[array4.Length];
			for (int j = 0; j < array4.Length; j++)
			{
				array6[j] = (sbyte)(array4[j] - array5[j]);
			}
		}
		else
		{
			array3 = GlobalConfig.Instance.TreasuryResourceSupplyRanges[supplyLevel];
			array6 = GlobalConfig.Instance.TreasuryItemSupplyCounts[supplyLevel];
		}
		for (sbyte b = 0; b < 7; b++)
		{
			int num2 = GetResourceSupplyThreshold(b, item2) * GameData.Domains.World.SharedMethods.GetGainResourcePercent(13) / 100;
			int worth = num2 * context.Random.Next((int)array3[0], array3[1] + 1) / 100;
			int num3 = ResourceTypeHelper.WorthToResourceAmount(b, worth);
			treasury.Resources.Add(b, num3);
			num += ResourceTypeHelper.ResourceAmountToWorth(b, num3);
		}
		for (sbyte b2 = item; b2 <= item2; b2++)
		{
			sbyte b3 = array6[b2];
			for (sbyte b4 = 0; b4 < 13; b4++)
			{
				ShortPair key = new ShortPair(b4, b2);
				if (_supplyItems.TryGetValue(key, out var value) && b3 > 0)
				{
					for (int k = 0; k < b3; k++)
					{
						short random = value.GetRandom(context.Random);
						ItemKey itemKey = DomainManager.Item.CreateItem(context, b4, random);
						treasury.Inventory.OfflineAdd(itemKey, 1);
						DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Treasury, Id);
						num += DomainManager.Item.GetValue(itemKey);
					}
				}
			}
			if (array6[b2] > 0 && _supplyBooks.TryGetValue(b2, out var value2) && value2.Count > 0)
			{
				for (int l = 0; l < array6[b2]; l++)
				{
					short random2 = value2.GetRandom(context.Random);
					ItemKey itemKey2 = DomainManager.Item.CreateItem(context, 10, random2);
					treasury.Inventory.OfflineAdd(itemKey2, 1);
					DomainManager.Item.SetOwner(itemKey2, ItemOwnerType.Treasury, Id);
					num += DomainManager.Item.GetValue(itemKey2);
				}
			}
		}
		return num;
	}

	public void ConfiscateItem(DataContext context, GameData.Domains.Character.Character character, List<ItemKey> itemKeys)
	{
		AdaptableLog.TagInfo(ToString(), $"Confiscating {itemKeys.Count} items from {character}.");
		Inventory inventory = character.GetInventory();
		SettlementLayeredTreasuries treasuries = Treasuries;
		foreach (ItemKey itemKey in itemKeys)
		{
			if (itemKey.IsValid())
			{
				sbyte grade = ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId);
				if (!inventory.Items.TryGetValue(itemKey, out var value))
				{
					int num = character.GetEquipment().IndexOf(itemKey);
					Tester.Assert(num >= 0);
					character.ChangeEquipment(context, (sbyte)num, -1, itemKey);
					value = 1;
				}
				inventory.OfflineRemove(itemKey, value);
				GetTreasury(treasuries, grade).Inventory.OfflineAdd(itemKey, value);
				Events.RaiseItemRemovedFromInventory(context, character, itemKey, value);
				DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Treasury, Id);
				SettlementTreasuryRecordCollection settlementTreasuryRecordCollection = DomainManager.Organization.GetSettlementTreasuryRecordCollection(context, Id);
				int currDate = DomainManager.World.GetCurrDate();
				settlementTreasuryRecordCollection.AddConfiscateItem(currDate, Id, character.GetId(), itemKey.ItemType, itemKey.TemplateId);
				DomainManager.Organization.SetSettlementTreasuryRecordCollection(context, Id, settlementTreasuryRecordCollection);
			}
		}
		character.SetInventory(inventory, context);
		DomainManager.Extra.SetTreasuries(context, Id, treasuries, needUpdateTotalValue: true);
	}

	public void ConfiscateResources(DataContext context, GameData.Domains.Character.Character character, ref ResourceInts resources)
	{
		int totalWorth = resources.GetTotalWorth();
		if (totalWorth > 0)
		{
			AdaptableLog.TagInfo(ToString(), $"Confiscating {totalWorth} worth of resources from {character}.");
			sbyte grade = character.GetOrganizationInfo().Grade;
			ref ResourceInts resources2 = ref character.GetResources();
			resources2 = resources2.Subtract(ref resources);
			character.SetResources(ref resources2, context);
			SettlementLayeredTreasuries treasuries = Treasuries;
			GetTreasury(treasuries, grade).Resources.Add(ref resources);
			DomainManager.Extra.SetTreasuries(context, Id, treasuries, needUpdateTotalValue: true);
		}
	}

	public void StoreItemInTreasury(DataContext context, GameData.Domains.Character.Character character, ItemKey itemKey, int amount, sbyte layerIndex)
	{
		SettlementLayeredTreasuries treasuries = Treasuries;
		SettlementTreasury settlementTreasury = ((layerIndex >= 0) ? treasuries.GetTreasury(layerIndex) : GetTreasury(treasuries, ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId)));
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int id = character.GetId();
		bool flag = id == DomainManager.Taiwu.GetTaiwuCharId();
		int currDate = DomainManager.World.GetCurrDate();
		SettlementTreasuryRecordCollection settlementTreasuryRecordCollection = DomainManager.Organization.GetSettlementTreasuryRecordCollection(context, Id);
		ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
		int num = settlementTreasury.CalcAdjustedWorth(baseItem.GetItemSubType(), baseItem.GetValue()) * amount;
		baseItem.SetOwner(ItemOwnerType.Treasury, Id);
		settlementTreasury.Inventory.OfflineAdd(itemKey, amount);
		settlementTreasury.OfflineChangeContribution(character, num);
		if (num > 0)
		{
			if (flag)
			{
				lifeRecordCollection.AddTaiwuStorageItemToTreasury(id, currDate, Id, itemKey.ItemType, itemKey.TemplateId);
			}
			else
			{
				lifeRecordCollection.AddStorageItemToTreasury(id, currDate, Id, itemKey.ItemType, itemKey.TemplateId, num);
			}
		}
		if (flag)
		{
			settlementTreasuryRecordCollection.AddTaiwuStorageItem(currDate, Id, id, itemKey.ItemType, itemKey.TemplateId);
		}
		else
		{
			settlementTreasuryRecordCollection.AddStorageItem(currDate, Id, id, itemKey.ItemType, itemKey.TemplateId, num);
		}
		DomainManager.Organization.SetSettlementTreasuryRecordCollection(context, Id, settlementTreasuryRecordCollection);
		DomainManager.Extra.SetTreasuries(context, Id, treasuries, needUpdateTotalValue: true);
	}

	public void TakeItemFromTreasury(DataContext context, GameData.Domains.Character.Character character, ItemKey itemKey, int amount)
	{
		SettlementLayeredTreasuries treasuries = Treasuries;
		SettlementTreasury settlementTreasury = GetTreasury(treasuries, ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId));
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int id = character.GetId();
		bool flag = id == DomainManager.Taiwu.GetTaiwuCharId();
		int currDate = DomainManager.World.GetCurrDate();
		SettlementTreasuryRecordCollection settlementTreasuryRecordCollection = DomainManager.Organization.GetSettlementTreasuryRecordCollection(context, Id);
		ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
		int num = settlementTreasury.CalcAdjustedWorth(baseItem.GetItemSubType(), baseItem.GetValue()) * amount;
		if (!settlementTreasury.Inventory.Items.ContainsKey(itemKey))
		{
			SettlementTreasury[] settlementTreasuries = treasuries.SettlementTreasuries;
			foreach (SettlementTreasury settlementTreasury2 in settlementTreasuries)
			{
				if (settlementTreasury2.Inventory.Items.ContainsKey(itemKey))
				{
					settlementTreasury = settlementTreasury2;
				}
			}
		}
		baseItem.RemoveOwner(ItemOwnerType.Treasury, Id);
		settlementTreasury.Inventory.OfflineRemove(itemKey, amount);
		settlementTreasury.OfflineChangeContribution(character, -num);
		if (num > 0)
		{
			if (flag)
			{
				lifeRecordCollection.AddTaiwuTakeItemFromTreasury(id, currDate, Id, itemKey.ItemType, itemKey.TemplateId);
			}
			else
			{
				lifeRecordCollection.AddTakeItemFromTreasury(id, currDate, Id, itemKey.ItemType, itemKey.TemplateId, num);
			}
		}
		if (flag)
		{
			settlementTreasuryRecordCollection.AddTaiwuTakeOutItem(currDate, Id, id, itemKey.ItemType, itemKey.TemplateId);
		}
		else
		{
			settlementTreasuryRecordCollection.AddTakeOutItem(currDate, Id, id, itemKey.ItemType, itemKey.TemplateId, num);
		}
		DomainManager.Organization.SetSettlementTreasuryRecordCollection(context, Id, settlementTreasuryRecordCollection);
		DomainManager.Extra.SetTreasuries(context, Id, treasuries, needUpdateTotalValue: true);
	}

	public void StoreResourceInTreasury(DataContext context, GameData.Domains.Character.Character character, sbyte resourceType, int amount, sbyte layerIndex)
	{
		int num = DomainManager.Organization.CalcResourceContribution(OrgTemplateId, resourceType, amount);
		SettlementLayeredTreasuries treasuries = Treasuries;
		SettlementTreasury settlementTreasury = ((layerIndex >= 0) ? treasuries.GetTreasury(layerIndex) : GetTreasury(treasuries, character.GetOrganizationInfo().Grade));
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int id = character.GetId();
		bool flag = id == DomainManager.Taiwu.GetTaiwuCharId();
		int currDate = DomainManager.World.GetCurrDate();
		SettlementTreasuryRecordCollection settlementTreasuryRecordCollection = DomainManager.Organization.GetSettlementTreasuryRecordCollection(context, Id);
		settlementTreasury.Resources.Add(resourceType, amount);
		settlementTreasury.OfflineChangeContribution(character, num);
		if (num > 0)
		{
			if (flag)
			{
				lifeRecordCollection.AddTaiwuStorageResourceToTreasury(id, currDate, Id, resourceType, amount);
			}
			else
			{
				lifeRecordCollection.AddStorageResourceToTreasury(id, currDate, Id, resourceType, amount, num);
			}
		}
		if (flag)
		{
			settlementTreasuryRecordCollection.AddTaiwuStorageResource(currDate, Id, id, resourceType, amount);
		}
		else
		{
			settlementTreasuryRecordCollection.AddStorageResource(currDate, Id, id, resourceType, amount, num);
		}
		DomainManager.Extra.SetTreasuries(context, Id, treasuries, needUpdateTotalValue: true);
		DomainManager.Organization.SetSettlementTreasuryRecordCollection(context, Id, settlementTreasuryRecordCollection);
	}

	public void TakeResourceFromTreasury(DataContext context, GameData.Domains.Character.Character character, sbyte resourceType, int amount, sbyte layerIndex)
	{
		int num = DomainManager.Organization.CalcResourceContribution(OrgTemplateId, resourceType, amount);
		SettlementLayeredTreasuries treasuries = Treasuries;
		SettlementTreasury settlementTreasury = ((layerIndex >= 0) ? treasuries.GetTreasury(layerIndex) : GetTreasury(treasuries, character.GetOrganizationInfo().Grade));
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int id = character.GetId();
		bool flag = id == DomainManager.Taiwu.GetTaiwuCharId();
		int currDate = DomainManager.World.GetCurrDate();
		SettlementTreasuryRecordCollection settlementTreasuryRecordCollection = DomainManager.Organization.GetSettlementTreasuryRecordCollection(context, Id);
		settlementTreasury.Resources.Subtract(resourceType, amount);
		settlementTreasury.OfflineChangeContribution(character, -num);
		if (num > 0)
		{
			if (flag)
			{
				lifeRecordCollection.AddTaiwuTakeResourceFromTreasury(id, currDate, Id, resourceType, amount);
			}
			else
			{
				lifeRecordCollection.AddTakeResourceFromTreasury(id, currDate, Id, resourceType, amount, num);
			}
		}
		if (flag)
		{
			settlementTreasuryRecordCollection.AddTaiwuTakeOutResource(currDate, Id, id, resourceType, amount);
		}
		else
		{
			settlementTreasuryRecordCollection.AddTakeOutResource(currDate, Id, id, resourceType, amount, num);
		}
		DomainManager.Extra.SetTreasuries(context, Id, treasuries, needUpdateTotalValue: true);
		DomainManager.Organization.SetSettlementTreasuryRecordCollection(context, Id, settlementTreasuryRecordCollection);
	}

	public static bool IsGuarding(int charId, bool includeNonIntelligentCharacter = false)
	{
		GameData.Domains.Character.Character element;
		return DomainManager.Character.TryGetElement_Objects(charId, out element) && ((element.GetCreatingType() == 1) ? CharacterMatcher.DefValue.InSettlement.Match(element) : includeNonIntelligentCharacter);
	}

	public IEnumerable<int> GetPrisonerRelatedGuards(SettlementPrisoner prisoner)
	{
		return from charId in Treasuries.GetTreasury((SettlementTreasuryLayers)Math.Clamp((int)prisoner.GetPrisonType(), 0, 2)).GuardIds.GetCollection()
			where IsGuarding(charId)
			select charId;
	}

	public void RefreshGuards(DataContext context)
	{
		for (sbyte b = 0; b < 3; b++)
		{
			foreach (int item in GetGuardsUnsorted(context, b))
			{
			}
		}
	}

	public IEnumerable<int> GetGuardsUnsorted(DataContext context, sbyte treasuryGrade)
	{
		SettlementTreasury treasury = Treasuries.GetTreasury(treasuryGrade);
		int count = 0;
		foreach (int guardId in treasury.GuardIds.GetCollection())
		{
			if (IsGuarding(guardId, includeNonIntelligentCharacter: true))
			{
				yield return guardId;
				count++;
			}
		}
		bool modified = false;
		while (count++ < GlobalConfig.Instance.TreasuryGuardCount)
		{
			modified = true;
			int charId = CreateNonIntelligentGuard(context, treasuryGrade, 0);
			treasury.GuardIds.Add(charId);
			yield return charId;
		}
		if (modified)
		{
			DomainManager.Extra.SetTreasuries(context, Id, Treasuries, needUpdateTotalValue: false);
		}
	}

	public IEnumerable<(GameData.Domains.Character.Character Character, short Favor)> GetGuardsAndFavors(DataContext context, sbyte treasuryGrade)
	{
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		return (from data in GetGuardsUnsorted(context, treasuryGrade).Select(delegate(int charId)
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
				return (character: element_Objects, DomainManager.Character.GetFavorability(charId, taiwuCharId));
			})
			orderby (data.character.GetCreatingType() != 1, -data.character.GetCombatPower())
			select data).Take(GlobalConfig.Instance.TreasuryGuardCount);
	}

	public IEnumerable<int> GetGuards(DataContext context, sbyte treasuryGrade)
	{
		return from characterAndFavor in GetGuardsAndFavors(context, treasuryGrade)
			select characterAndFavor.Character.GetId();
	}

	public CharacterDisplayData[] GetGuardsDisplayData(DataContext context, sbyte treasuryGrade)
	{
		return GetGuardsAndFavors(context, treasuryGrade).Select(delegate((GameData.Domains.Character.Character Character, short Favor) characterAndFavor)
		{
			CharacterDisplayData characterDisplayData = DomainManager.Character.GetCharacterDisplayData(characterAndFavor.Character.GetId());
			characterDisplayData.FavorabilityToTaiwu = characterAndFavor.Favor;
			return characterDisplayData;
		}).ToArray();
	}

	public int CreateNonIntelligentGuard(DataContext context, sbyte treasuryGrade, sbyte offset = 0)
	{
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		SettlementTreasury treasury = Treasuries.GetTreasury(treasuryGrade);
		sbyte orgTemplateId = GetOrgTemplateId();
		sbyte b = (sbyte)Math.Clamp(((this is Sect) ? GlobalConfig.Instance.SectTreasuryGuardMaxGrade[treasuryGrade] : GlobalConfig.Instance.TreasuryGuardMaxGrade[treasuryGrade]) + offset, 0, 8);
		short templateId = ((this is Sect) ? GameData.Domains.Character.Character.GetSectRandomEnemyTemplateIdByGrade(orgTemplateId, b) : CivilianSettlement.GetTreasuryGuardTemplateId(b));
		int num = DomainManager.Character.CreateNonIntelligentCharacter(context, templateId);
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(num);
		element_Objects.AddFeature(context, GetTreasuryGuardFeatureId(treasury.LayerIndex));
		DomainManager.Character.DirectlySetFavorabilities(context, num, taiwuCharId, DomainManager.Character.CalcInitialFavorability(context.Random, num, taiwuCharId), DomainManager.Character.CalcInitialFavorability(context.Random, taiwuCharId, num));
		return num;
	}

	public void ForceUpdateTreasuryGuards(DataContext context)
	{
		SettlementLayeredTreasuries treasuries = Treasuries;
		OfflineUpdateTreasuryGuards(context, treasuries);
		DomainManager.Extra.SetTreasuries(context, Id, treasuries, needUpdateTotalValue: false);
	}

	public void SetAlterTime(DataContext context, byte time)
	{
		SettlementLayeredTreasuries treasuries = Treasuries;
		Treasuries.AlertTime = time;
		DomainManager.Extra.SetTreasuries(context, Id, treasuries, needUpdateTotalValue: false);
	}

	public byte GetAlterTime(DataContext context)
	{
		return Treasuries.AlertTime;
	}

	private SettlementTreasuryLayers GetLayer(sbyte grade)
	{
		return (SettlementTreasuryLayers)Grade.GetGroup(grade);
	}

	private SettlementTreasury GetTreasury(SettlementLayeredTreasuries treasuries, sbyte grade)
	{
		return treasuries.GetTreasury(GetLayer(grade));
	}

	private SettlementTreasury GetTreasury(SettlementLayeredTreasuries treasuries, SettlementTreasuryLayers layer)
	{
		return treasuries.GetTreasury(layer);
	}

	private int GetResourceSupplyThreshold(sbyte resourceType, sbyte grade)
	{
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(OrgTemplateId, grade);
		return orgMemberConfig.GetAdjustedResourceSatisfyingThreshold(resourceType);
	}

	private bool CanItemBeSupplied(sbyte itemType, short templateId, sbyte targetGrade, out short targetTemplateId)
	{
		targetTemplateId = templateId;
		if (!ItemTemplateHelper.CheckTemplateValid(itemType, templateId))
		{
			return false;
		}
		if (ItemTemplateHelper.GetItemSubType(itemType, targetTemplateId) == 1204)
		{
			return true;
		}
		short groupId = ItemTemplateHelper.GetGroupId(itemType, templateId);
		if (groupId < 0)
		{
			return false;
		}
		for (int i = 0; i <= 8; i++)
		{
			targetTemplateId = (short)(groupId + i);
			if (!ItemTemplateHelper.CheckTemplateValid(itemType, targetTemplateId))
			{
				return false;
			}
			if (ItemTemplateHelper.GetGrade(itemType, targetTemplateId) == targetGrade)
			{
				short groupId2 = ItemTemplateHelper.GetGroupId(itemType, targetTemplateId);
				return groupId2 >= 0 && groupId2 == groupId;
			}
		}
		return false;
	}

	public Inventory GetSupplyItems()
	{
		Inventory inventory = new Inventory();
		foreach (KeyValuePair<ShortPair, List<short>> supplyItem in _supplyItems)
		{
			supplyItem.Deconstruct(out var key, out var value);
			ShortPair shortPair = key;
			List<short> list = value;
			sbyte itemType = (sbyte)shortPair.First;
			foreach (short item in list)
			{
				ItemKey itemKey = new ItemKey(itemType, 0, item, 0);
				inventory.OfflineAdd(itemKey, 1);
			}
		}
		foreach (List<short> value2 in _supplyBooks.Values)
		{
			foreach (short item2 in value2)
			{
				ItemKey itemKey2 = new ItemKey(10, 0, item2, 0);
				inventory.OfflineAdd(itemKey2, 1);
			}
		}
		return inventory;
	}

	public virtual int GetSupplyLevel()
	{
		return DomainManager.Organization.IsCreatingSettlements() ? 2 : (1 + Treasuries.SupplyLevelAddOn);
	}

	public virtual short GetTreasuryGuardFeatureId(sbyte layerIndex)
	{
		int num = layerIndex + 1;
		return (short)(536 + num);
	}

	protected abstract void OfflineUpdateTreasuryGuards(DataContext context, SettlementLayeredTreasuries treasuries);

	public short GetId()
	{
		return Id;
	}

	public sbyte GetOrgTemplateId()
	{
		return OrgTemplateId;
	}

	public Location GetLocation()
	{
		return Location;
	}

	public short GetCulture()
	{
		return Culture;
	}

	public abstract void SetCulture(short culture, DataContext context);

	public short GetMaxCulture()
	{
		return MaxCulture;
	}

	public abstract void SetMaxCulture(short maxCulture, DataContext context);

	public short GetSafety()
	{
		return Safety;
	}

	public abstract void SetSafety(short safety, DataContext context);

	public short GetMaxSafety()
	{
		return MaxSafety;
	}

	public abstract void SetMaxSafety(short maxSafety, DataContext context);

	public int GetPopulation()
	{
		return Population;
	}

	public abstract void SetPopulation(int population, DataContext context);

	public int GetMaxPopulation()
	{
		return MaxPopulation;
	}

	public abstract void SetMaxPopulation(int maxPopulation, DataContext context);

	public int GetStandardOnStagePopulation()
	{
		return StandardOnStagePopulation;
	}

	public abstract void SetStandardOnStagePopulation(int standardOnStagePopulation, DataContext context);

	public OrgMemberCollection GetMembers()
	{
		return Members;
	}

	public abstract void SetMembers(OrgMemberCollection members, DataContext context);

	public OrgMemberCollection GetLackingCoreMembers()
	{
		return LackingCoreMembers;
	}

	public abstract void SetLackingCoreMembers(OrgMemberCollection lackingCoreMembers, DataContext context);

	public short GetApprovingRateUpperLimitBonus()
	{
		return ApprovingRateUpperLimitBonus;
	}

	public abstract void SetApprovingRateUpperLimitBonus(short approvingRateUpperLimitBonus, DataContext context);

	public int GetInfluencePowerUpdateDate()
	{
		return InfluencePowerUpdateDate;
	}

	public abstract void SetInfluencePowerUpdateDate(int influencePowerUpdateDate, DataContext context);

	public abstract short GetApprovingRateUpperLimitTempBonus();

	public ValueInfo SelectValue(Evaluator evaluator, string identifier)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		if (1 == 0)
		{
		}
		ValueInfo result = ((identifier == "MapBlock") ? evaluator.PushEvaluationResult((object)DomainManager.Map.GetBlock(Location)) : ((!(identifier == "ArgBox")) ? ValueInfo.Void : evaluator.PushEvaluationResult((object)DomainManager.Extra.GetSectMainStoryEventArgBox(OrgTemplateId))));
		if (1 == 0)
		{
		}
		return result;
	}
}
