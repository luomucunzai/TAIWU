using System;
using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai.PrioritizedAction;
using GameData.Domains.Character.Filters;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.Taiwu.Display;
using GameData.Domains.Taiwu.Display.VillagerRoleArrangement;
using GameData.Domains.World.Notification;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Taiwu.VillagerRole;

[SerializableGameData(IsExtensible = true, NotForDisplayModule = true, NoCopyConstructors = true)]
public class VillagerRoleLiterati : VillagerRoleBase, IVillagerRoleContact, IVillagerRoleInfluence, IVillagerRoleArrangementExecutor, IVillagerRoleSelectLocation
{
	private static class FieldIds
	{
		public const ushort ArrangementTemplateId = 0;

		public const ushort PositiveAction = 1;

		public const ushort Count = 2;

		public static readonly string[] FieldId2FieldName = new string[2] { "ArrangementTemplateId", "PositiveAction" };
	}

	[SerializableGameDataField]
	public bool PositiveAction = true;

	public override short RoleTemplateId => 4;

	public bool IncreaseFavorability => PositiveAction;

	public int LearnActionRepeatChance => SharedMethods.CalculateLiteratiLearnActionRepeatChance(Character.GetPersonalities());

	public int LearnRequestSuccessChanceBonus => SharedMethods.CalculateLiteratiLearnRequestSuccessChanceBonus(Character.GetPersonalities());

	public int ContactFavorabilityChange => SharedMethods.CalculateLiteratiContactFavorabilityChange(Character.GetPersonalities(), Character.GetLifeSkillAttainments());

	public int ContactCharacterAmount => SharedMethods.CalculateLiteratiContactCharacterAmount(Character.GetPersonalities());

	public int EntertainTargetAmount => SharedMethods.CalculateLiteratiEntertainTargetAmount(Character.GetPersonalities());

	public int EntertainHappinessChange => SharedMethods.CalculateLiteratiEntertainHappinessChange(Character.GetPersonalities(), Character.GetLifeSkillAttainments());

	public int SecretInformationGainChance => SharedMethods.CalculateLiteratiSecretInformationGainChance(Character.GetPersonalities());

	internal int ActionEffectCount => VillagerRoleFormulaImpl.Calculate(20, Character.GetPersonality(1));

	internal int ActionEffectValue => VillagerRoleFormulaImpl.Calculate(21, CalcLiteratiLifeSkillMaxAttainment());

	internal int ExtraPeopleCount => VillagerRoleFormulaImpl.Calculate(22, Character.GetPersonality(1));

	internal int RelationChange => VillagerRoleFormulaImpl.Calculate(23, CalcLiteratiLifeSkillMaxAttainment());

	public int InfluenceSettlementValueChange => SharedMethods.CalculateLiteratiInfluenceSettlementValueChange(Character.GetPersonalities(), Character.GetLifeSkillAttainments());

	void IVillagerRoleArrangementExecutor.ExecuteArrangementAction(DataContext context, VillagerRoleArrangementAction action)
	{
		int arrangementTemplateId = ArrangementTemplateId;
		int num = arrangementTemplateId;
		if (num == 2)
		{
			ApplyEntertainAction(context, action);
		}
	}

	private void ApplyEntertainAction(DataContext context, VillagerRoleArrangementAction action)
	{
		Location location = Character.GetLocation();
		if (!location.IsValid())
		{
			return;
		}
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		DomainManager.Map.GetAreaSettlementIds(location.AreaId, list, containsMainCity: true, containsSect: true);
		if (list.Count <= 0)
		{
			ObjectPool<List<short>>.Instance.Return(list);
			return;
		}
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		int id = Character.GetId();
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int actionEffectCount = ActionEffectCount;
		int actionEffectValue = ActionEffectValue;
		short settlementId = list[context.Random.Next(list.Count)];
		Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
		short culture = settlement.GetCulture();
		short safety = settlement.GetSafety();
		List<int> list2 = ObjectPool<List<int>>.Instance.Get();
		Dictionary<int, int> dictionary = ObjectPool<Dictionary<int, int>>.Instance.Get();
		int num = 0;
		int num2 = 0;
		int key;
		for (int i = 0; i < actionEffectCount; i++)
		{
			IRandomSource random = context.Random;
			sbyte behaviorType = Character.GetBehaviorType();
			if (1 == 0)
			{
			}
			key = behaviorType switch
			{
				0 => 75, 
				1 => 100, 
				3 => 0, 
				4 => 25, 
				_ => 50, 
			};
			if (1 == 0)
			{
			}
			int num3 = (random.CheckPercentProb(key) ? 1 : (-1));
			list2.Clear();
			if (context.Random.NextBool())
			{
				settlement.ChangeSafety(context, num3 * actionEffectValue);
			}
			else
			{
				settlement.ChangeCulture(context, num3 * actionEffectValue);
			}
			if (base.HasChickenUpgradeEffect)
			{
				OrgMemberCollection members = settlement.GetMembers();
				if (members != null)
				{
					List<int> list3 = ObjectPool<List<int>>.Instance.Get();
					list3.Clear();
					members.GetAllMembers(list3);
					foreach (int item in list3)
					{
						list2.Add(item);
					}
					ObjectPool<List<int>>.Instance.Return(list3);
				}
			}
			if (list2.Count <= 0)
			{
				continue;
			}
			int extraPeopleCount = ExtraPeopleCount;
			int num4 = Math.Max(RelationChange, 0);
			IRandomSource random2 = context.Random;
			sbyte behaviorType2 = Character.GetBehaviorType();
			if (1 == 0)
			{
			}
			key = behaviorType2 switch
			{
				0 => 75, 
				1 => 100, 
				3 => 0, 
				4 => 25, 
				_ => 50, 
			};
			if (1 == 0)
			{
			}
			int num5 = num4 * (random2.CheckPercentProb(key) ? 1 : (-1));
			list2.Remove(DomainManager.Taiwu.GetTaiwuCharId());
			CollectionUtils.Shuffle(context.Random, list2);
			int num6 = list2.Count - extraPeopleCount;
			if (num6 > 0)
			{
				list2.RemoveRange(0, num6);
			}
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			foreach (int item2 in list2)
			{
				if (DomainManager.Character.TryGetElement_Objects(item2, out var element))
				{
					DomainManager.Character.ChangeFavorabilityOptional(context, element, taiwu, num5, 5);
					dictionary.TryAdd(item2, 0);
					Dictionary<int, int> dictionary2 = dictionary;
					key = item2;
					dictionary2[key] += num5;
				}
			}
		}
		short culture2 = settlement.GetCulture();
		short safety2 = settlement.GetSafety();
		if (safety2 > safety)
		{
			lifeRecordCollection.AddLiteratiSpreadingInfluenceSafetyUp(id, currDate, settlementId);
		}
		else if (safety2 < safety)
		{
			lifeRecordCollection.AddLiteratiSpreadingInfluenceSafetyDown(id, currDate, settlementId);
		}
		if (culture2 > culture)
		{
			lifeRecordCollection.AddLiteratiSpreadingInfluenceCultureUp(id, currDate, settlementId);
		}
		else if (culture2 < culture)
		{
			lifeRecordCollection.AddLiteratiSpreadingInfluenceCultureDown(id, currDate, settlementId);
		}
		foreach (KeyValuePair<int, int> item3 in dictionary)
		{
			item3.Deconstruct(out key, out var value);
			int selfCharId = key;
			int num7 = value;
			if (num7 < 0)
			{
				num2++;
				lifeRecordCollection.AddLiteratiBeConnectedRelationshipDown(selfCharId, currDate, id, settlementId);
			}
			else if (num7 > 0)
			{
				num++;
				lifeRecordCollection.AddLiteratiBeConnectedRelationshipUp(selfCharId, currDate, id, settlementId);
			}
		}
		if (num > 0)
		{
			lifeRecordCollection.AddLiteratiConnectRelationshipUp(id, currDate, settlementId, num);
			lifeRecordCollection.AddLiteratiConnectRelationshipUpTaiwu(taiwuCharId, currDate, id, settlementId);
		}
		if (num2 > 0)
		{
			lifeRecordCollection.AddLiteratiConnectRelationshipDown(id, currDate, settlementId, num2);
			lifeRecordCollection.AddLiteratiConnectRelationshipDownTaiwu(taiwuCharId, currDate, id, settlementId);
		}
		ObjectPool<List<int>>.Instance.Return(list2);
		ObjectPool<List<short>>.Instance.Return(list);
		ObjectPool<Dictionary<int, int>>.Instance.Return(dictionary);
		CharacterDomain.AddLockMovementCharSet(id);
	}

	public override void ExecuteFixedAction(DataContext context)
	{
		if (ArrangementTemplateId < 0 && (WorkData == null || WorkData.WorkType != 1))
		{
			TryAddNextAutoTravelTarget(context, AutoActionBlockFilter);
			AutoWriteAndDrawAction(context);
		}
	}

	private bool AutoActionBlockFilter(MapBlockData blockData)
	{
		MapDomain map = DomainManager.Map;
		Location location = Character.GetLocation();
		if (location.IsValid() && map.GetStateIdByAreaId(location.AreaId) == map.GetStateIdByAreaId(blockData.AreaId))
		{
			HashSet<int> characterSet = blockData.CharacterSet;
			if (characterSet != null && characterSet.Count >= 3)
			{
				return true;
			}
		}
		return false;
	}

	private void AutoWriteAndDrawAction(DataContext context)
	{
		Location location = Character.GetLocation();
		MapBlockData block = DomainManager.Map.GetBlock(location);
		int num = VillagerRoleFormulaImpl.Calculate(18, Character.GetPersonality(2));
		ObjectPool<List<int>> instance = ObjectPool<List<int>>.Instance;
		List<int> list = instance.Get();
		list.Clear();
		list.AddRange(block.CharacterSet);
		list.Remove(Character.GetId());
		CollectionUtils.Shuffle(context.Random, list);
		int num2 = list.Count - num;
		if (num2 > 0)
		{
			list.RemoveRange(0, num2);
		}
		int num3 = VillagerRoleFormulaImpl.Calculate(19, CalcLiteratiLifeSkillMaxAttainment());
		int id = Character.GetId();
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int num4 = 0;
		int num5 = 0;
		foreach (int item in list)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			IRandomSource random = context.Random;
			sbyte behaviorType = Character.GetBehaviorType();
			if (1 == 0)
			{
			}
			int percentProb = behaviorType switch
			{
				0 => 75, 
				1 => 100, 
				3 => 0, 
				4 => 25, 
				_ => 50, 
			};
			if (1 == 0)
			{
			}
			int num6 = (random.CheckPercentProb(percentProb) ? 1 : (-1));
			int num7 = num3 * num6;
			element_Objects.ChangeHappiness(context, num7);
			if (num7 > 0)
			{
				num4++;
				lifeRecordCollection.AddLiteratiBeEntertainedUp(item, currDate, id, location);
			}
			else if (num7 < 0)
			{
				num5++;
				lifeRecordCollection.AddLiteratiBeEntertainedDown(item, currDate, id, location);
			}
		}
		if (num4 > 0)
		{
			lifeRecordCollection.AddLiteratiEntertainingUp(id, currDate, location, num4);
		}
		if (num5 > 0)
		{
			lifeRecordCollection.AddLiteratiEntertainingDown(id, currDate, location, num5);
		}
		instance.Return(list);
		CharacterDomain.AddLockMovementCharSet(id);
	}

	private void ApplyChickenUpgradeEffect(DataContext context, List<GameData.Domains.Character.Character> targets)
	{
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		HashSet<int> hashSet = new HashSet<int>();
		DomainManager.Information.GetSecretInformationOfCharacter(hashSet, taiwuCharId);
		List<int> list = new List<int>();
		List<(int, int)> list2 = new List<(int, int)>();
		foreach (GameData.Domains.Character.Character target in targets)
		{
			int id = target.GetId();
			DomainManager.Information.GetSecretInformationOfCharacter(list, id, includeBroadcast: false);
			foreach (int item in list)
			{
				if (!hashSet.Contains(item))
				{
					list2.Add((id, item));
				}
			}
		}
		if (list2.Count != 0)
		{
			(int, int) random = list2.GetRandom(context.Random);
			DomainManager.Information.ReceiveSecretInformation(context, random.Item2, taiwuCharId, random.Item1);
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			monthlyNotificationCollection.AddChickenSecretInformation(Character.GetId(), Character.GetLocation(), random.Item1);
		}
	}

	public void SelectContactTargets(IRandomSource random, List<GameData.Domains.Character.Character> selectedCharList, int selectAmount)
	{
		selectedCharList.Clear();
		Location location = Character.GetLocation();
		MapBlockData currBlock = DomainManager.Map.GetBlock(location);
		if (currBlock.BelongBlockId >= 0)
		{
			MapCharacterFilter.Find(CharacterFilter, selectedCharList, location.AreaId);
			if (selectedCharList.Count > selectAmount)
			{
				selectedCharList.RemoveRange(selectAmount, selectedCharList.Count - selectAmount);
			}
		}
		bool CharacterFilter(GameData.Domains.Character.Character character)
		{
			if (OrganizationDomain.IsLargeSect(character.GetOrganizationInfo().OrgTemplateId))
			{
				return false;
			}
			if (character == Character)
			{
				return false;
			}
			Location location2 = character.GetLocation();
			MapBlockData block = DomainManager.Map.GetBlock(location2);
			if (block.IsNonDeveloped())
			{
				return false;
			}
			if (block.BelongBlockId != currBlock.BelongBlockId)
			{
				return false;
			}
			return true;
		}
	}

	void IVillagerRoleInfluence.ApplyInfluenceAction(DataContext context)
	{
		int num = InfluenceSettlementValueChange;
		if (!PositiveAction)
		{
			num = -num;
		}
		short areaId = Character.GetLocation().AreaId;
		MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(areaId);
		int num2 = 0;
		SettlementInfo[] settlementInfos = element_Areas.SettlementInfos;
		for (int i = 0; i < settlementInfos.Length; i++)
		{
			SettlementInfo settlementInfo = settlementInfos[i];
			if (settlementInfo.SettlementId >= 0)
			{
				Settlement settlement = DomainManager.Organization.GetSettlement(settlementInfo.SettlementId);
				settlement.ChangeCulture(context, num);
				num2 += GetSettlementInfluenceAuthorityGain(settlement);
			}
		}
		DomainManager.Taiwu.GetTaiwu().ChangeResource(context, 7, num2);
	}

	public int GetSettlementInfluenceAuthorityGain(Settlement settlement)
	{
		if (settlement == null)
		{
			return 0;
		}
		int num = (PositiveAction ? settlement.GetCulture() : (settlement.GetMaxCulture() - settlement.GetCulture()));
		return num * (50 + Character.GetPersonality(2) / 2) / 100;
	}

	private static IEnumerable<sbyte> CalcLiteratiLifeSkillTypes()
	{
		yield return 0;
		yield return 1;
		yield return 2;
		yield return 3;
	}

	private int CalcLiteratiLifeSkillMaxAttainment()
	{
		LifeSkillShorts lifeSkillAttainments = Character.GetLifeSkillAttainments();
		short num = short.MinValue;
		foreach (sbyte item in CalcLiteratiLifeSkillTypes())
		{
			if (lifeSkillAttainments[item] > num)
			{
				num = lifeSkillAttainments[item];
			}
		}
		return num;
	}

	public override IVillagerRoleArrangementDisplayData GetArrangementDisplayData()
	{
		return new EntertainingDisplayData
		{
			ActionEffectCount = ActionEffectCount,
			ActionEffectValue = ActionEffectValue,
			ExtraPeopleCount = ExtraPeopleCount,
			RelationChange = RelationChange
		};
	}

	public override bool IsSerializedSizeFixed()
	{
		return false;
	}

	public override int GetSerializedSize()
	{
		int num = 7;
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe override int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 2;
		ptr += 2;
		*(int*)ptr = ArrangementTemplateId;
		ptr += 4;
		*ptr = (PositiveAction ? ((byte)1) : ((byte)0));
		ptr++;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe override int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			ArrangementTemplateId = *(int*)ptr;
			ptr += 4;
		}
		if (num > 1)
		{
			PositiveAction = *ptr != 0;
			ptr++;
		}
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}
}
