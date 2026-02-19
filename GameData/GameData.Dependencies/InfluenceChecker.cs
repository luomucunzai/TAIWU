using System;
using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.Domains;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.Organization;
using GameData.Domains.SpecialEffect;
using GameData.Utilities;

namespace GameData.Dependencies;

public static class InfluenceChecker
{
	public static readonly LocalObjectPool<List<BaseGameDataObject>> InfluencedObjectsPool = new LocalObjectPool<List<BaseGameDataObject>>(8, 16);

	public static bool CheckCondition(DataContext context, BaseGameDataObject sourceObject, InfluenceCondition condition)
	{
		return condition switch
		{
			InfluenceCondition.None => true, 
			InfluenceCondition.CharIsTaiwu => CheckCondition_CharIsTaiwu(sourceObject), 
			InfluenceCondition.CharIsInTaiwuGroup => CheckCondition_CharIsInTaiwuGroup(sourceObject), 
			InfluenceCondition.ItemIsEquipped => CheckCondition_ItemIsEquipped(sourceObject), 
			InfluenceCondition.CivilianSettlementIsTaiwuVillage => CheckCondition_CivilianSettlementIsTaiwuVillage(sourceObject), 
			InfluenceCondition.CharIsInAnySect => CheckCondition_CharIsInAnySect(sourceObject), 
			InfluenceCondition.CharIsInAnyCivilianSettlement => CheckCondition_CharIsInAnyCivilianSettlement(sourceObject), 
			InfluenceCondition.CombatSkillIsProactive => CheckCondition_CombatSkillIsProactive(sourceObject), 
			InfluenceCondition.CharIsTaiwuWorker => CheckCondition_CharIsTaiwuWorker(sourceObject), 
			InfluenceCondition.CharIsTaiwuVillager => CheckCondition_CharIsTaiwuVillager(sourceObject), 
			InfluenceCondition.CharIsTaiwuVillageHead => CheckCondition_CharIsTaiwuVillageHead(sourceObject), 
			InfluenceCondition.CharIsTaskRelated => CheckCondition_CharIsTaskRelated(sourceObject), 
			InfluenceCondition.CombatSkillIsLearnedByTaiwu => CheckCondition_CombatSkillIsLearnedByTaiwu(sourceObject), 
			InfluenceCondition.CombatWeaponIsTaiwuWeapon => CheckCondition_CombatWeaponIsTaiwuWeapon(sourceObject), 
			InfluenceCondition.CombatWeaponIsNotTaiwuWeapon => CheckCondition_CombatWeaponIsNotTaiwuWeapon(sourceObject), 
			_ => throw new Exception($"Unsupported InfluenceCondition: {condition}"), 
		};
	}

	public static bool GetScope<TKey, TValue>(DataContext context, BaseGameDataObject sourceObject, InfluenceScope scope, IDictionary<TKey, TValue> targetCollection, List<BaseGameDataObject> influencedObjects) where TKey : unmanaged where TValue : BaseGameDataObject
	{
		return scope switch
		{
			InfluenceScope.All => true, 
			InfluenceScope.Self => GetScope_Self(sourceObject, influencedObjects), 
			InfluenceScope.TaiwuChar => GetScope_TaiwuChar(influencedObjects), 
			InfluenceScope.CharWhoEquippedTheItem => GetScope_CharWhoEquippedTheItem(sourceObject, targetCollection, influencedObjects), 
			InfluenceScope.CombatSkillOwner => GetScope_CombatSkillOwner(sourceObject, targetCollection, influencedObjects), 
			InfluenceScope.AllCharsInTaiwuVillage => GetScope_AllCharsInTaiwuVillage(influencedObjects), 
			InfluenceScope.AllNonHeadCharsInTaiwuVillage => GetScope_AllNonHeadCharsInTaiwuVillage(influencedObjects), 
			InfluenceScope.AllCharsInCombat => GetScope_AllCharsInCombat(influencedObjects), 
			InfluenceScope.AllCombatCharsInCombat => GetScope_AllCombatCharsInCombat(influencedObjects), 
			InfluenceScope.CombatSkillsOfAllCharsInCombat => GetScope_CombatSkillsOfAllCharsInCombat(influencedObjects), 
			InfluenceScope.CombatSkillsOfTheChar => GetScope_CombatSkillsOfTheChar(sourceObject, influencedObjects), 
			InfluenceScope.CombatSkillsOfTaiwuChar => GetScope_CombatSkillsOfTaiwuChar(sourceObject, influencedObjects), 
			InfluenceScope.CombatSkillsOfTheCombatChar => GetScope_CombatSkillsOfTheCombatChar(sourceObject, influencedObjects), 
			InfluenceScope.CombatCharOfTheCombatSkillData => GetScope_CombatCharOfTheCombatSkillData(sourceObject, influencedObjects), 
			InfluenceScope.CombatCharOfTheCombatWeaponData => GetScope_CombatCharOfTheCombatWeaponData(sourceObject, influencedObjects), 
			InfluenceScope.CombatCharOfTheChar => GetScope_CombatCharOfTheChar(sourceObject, influencedObjects), 
			InfluenceScope.CharOfTheCombatChar => GetScope_CharOfTheCombatChar(sourceObject, influencedObjects), 
			InfluenceScope.SectCharOfTheChar => GetScope_SectCharOfTheChar(sourceObject, targetCollection, influencedObjects), 
			InfluenceScope.CivilianSettlementCharOfTheChar => GetScope_CivilianSettlementCharOfTheChar(sourceObject, targetCollection, influencedObjects), 
			InfluenceScope.SectCharsOfTheSect => GetScope_SectCharsOfTheSect(sourceObject, targetCollection, influencedObjects), 
			InfluenceScope.CivilianSettlementCharsOfTheCivilianSettlement => GetScope_CivilianSettlementCharsOfTheCivilianSettlement(sourceObject, targetCollection, influencedObjects), 
			InfluenceScope.SectCharsOfTheChar => GetScope_SectCharsOfTheChar(sourceObject, targetCollection, influencedObjects), 
			InfluenceScope.CivilianSettlementCharsOfTheChar => GetScope_CivilianSettlementCharsOfTheChar(sourceObject, targetCollection, influencedObjects), 
			InfluenceScope.CharacterAffectedByTheSpecialEffects => GetScope_CharacterAffectedByTheSpecialEffects(sourceObject, influencedObjects), 
			InfluenceScope.CombatSkillsOfTheCharacterAffectedByTheSpecialEffects => GetScope_CombatSkillsOfTheCharacterAffectedByTheSpecialEffects(sourceObject, influencedObjects), 
			InfluenceScope.CombatCharacterAffectedByTheSpecialEffects => GetScope_CombatCharacterAffectedByTheSpecialEffects(sourceObject, influencedObjects), 
			InfluenceScope.CombatSkillDataAffectedByTheSpecialEffects => GetScope_CombatSkillDataAffectedByTheSpecialEffects(sourceObject, influencedObjects), 
			InfluenceScope.CombatSkillsAffectedByPowerChangeInCombat => GetScope_CombatSkillsAffectedByPowerChangeInCombat(influencedObjects), 
			InfluenceScope.CombatSkillsAffectedByPowerReplaceInCombat => GetScope_CombatSkillsAffectedByPowerReplaceInCombat(influencedObjects), 
			InfluenceScope.CombatSkillsAffectedByCombatSkillDataInCombat => GetScope_CombatSkillsAffectedByCombatSkillDataInCombat(sourceObject, influencedObjects), 
			InfluenceScope.TaiwuAndGearMates => GetScope_TaiwuAndGearMates(influencedObjects), 
			_ => throw new Exception($"Unsupported InfluenceScope: {scope}"), 
		};
	}

	public static bool CheckCondition_CharIsTaiwu(BaseGameDataObject sourceObject)
	{
		return sourceObject == DomainManager.Taiwu.GetTaiwu();
	}

	public static bool CheckCondition_CharIsInTaiwuGroup(BaseGameDataObject sourceObject)
	{
		GameData.Domains.Character.Character character = (GameData.Domains.Character.Character)sourceObject;
		return character.GetLeaderId() == DomainManager.Taiwu.GetTaiwuCharId();
	}

	public static bool CheckCondition_ItemIsEquipped(BaseGameDataObject sourceObject)
	{
		EquipmentBase equipmentBase = (EquipmentBase)sourceObject;
		return equipmentBase.GetEquippedCharId() >= 0;
	}

	public static bool CheckCondition_CivilianSettlementIsTaiwuVillage(BaseGameDataObject sourceObject)
	{
		CivilianSettlement civilianSettlement = (CivilianSettlement)sourceObject;
		return civilianSettlement.GetId() == DomainManager.Taiwu.GetTaiwuVillageSettlementId();
	}

	public static bool CheckCondition_CharIsInAnySect(BaseGameDataObject sourceObject)
	{
		GameData.Domains.Character.Character character = (GameData.Domains.Character.Character)sourceObject;
		return OrganizationDomain.IsSect(character.GetOrganizationInfo().OrgTemplateId);
	}

	public static bool CheckCondition_CharIsInAnyCivilianSettlement(BaseGameDataObject sourceObject)
	{
		GameData.Domains.Character.Character character = (GameData.Domains.Character.Character)sourceObject;
		return !OrganizationDomain.IsSect(character.GetOrganizationInfo().OrgTemplateId);
	}

	public static bool CheckCondition_CombatSkillIsProactive(BaseGameDataObject sourceObject)
	{
		short skillTemplateId = ((GameData.Domains.CombatSkill.CombatSkill)sourceObject).GetId().SkillTemplateId;
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillTemplateId];
		return GameData.Domains.Character.CombatSkillHelper.IsProactiveSkill(combatSkillItem.EquipType);
	}

	public static bool CheckCondition_CombatSkillIsLearnedByTaiwu(BaseGameDataObject sourceObject)
	{
		return ((GameData.Domains.CombatSkill.CombatSkill)sourceObject).GetId().CharId == DomainManager.Taiwu.GetTaiwuCharId();
	}

	public static bool CheckCondition_CombatWeaponIsTaiwuWeapon(BaseGameDataObject sourceObject)
	{
		return ((CombatWeaponData)sourceObject).Character.GetId() == DomainManager.Taiwu.GetTaiwuCharId();
	}

	public static bool CheckCondition_CombatWeaponIsNotTaiwuWeapon(BaseGameDataObject sourceObject)
	{
		return ((CombatWeaponData)sourceObject).Character.GetId() != DomainManager.Taiwu.GetTaiwuCharId();
	}

	public static bool CheckCondition_CharIsTaiwuWorker(BaseGameDataObject sourceObject)
	{
		GameData.Domains.Character.Character character = (GameData.Domains.Character.Character)sourceObject;
		return character.IsActiveExternalRelationState(1);
	}

	public static bool CheckCondition_CharIsTaiwuVillager(BaseGameDataObject sourceObject)
	{
		GameData.Domains.Character.Character character = (GameData.Domains.Character.Character)sourceObject;
		return character.GetOrganizationInfo().OrgTemplateId == 16;
	}

	public static bool CheckCondition_CharIsTaiwuVillageHead(BaseGameDataObject sourceObject)
	{
		GameData.Domains.Character.Character character = (GameData.Domains.Character.Character)sourceObject;
		OrganizationInfo organizationInfo = character.GetOrganizationInfo();
		return organizationInfo.OrgTemplateId == 16 && organizationInfo.Grade == 7;
	}

	public static bool CheckCondition_CharIsTaskRelated(BaseGameDataObject sourceObject)
	{
		GameData.Domains.Character.Character character = (GameData.Domains.Character.Character)sourceObject;
		return character == DomainManager.Taiwu.GetTaiwu() || Config.Character.Instance[character.GetTemplateId()].CreatingType == 0;
	}

	public static bool GetScope_Self(BaseGameDataObject sourceObject, List<BaseGameDataObject> influencedObjects)
	{
		influencedObjects.Add(sourceObject);
		return false;
	}

	public static bool GetScope_TaiwuChar(List<BaseGameDataObject> influencedObjects)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		influencedObjects.Add(taiwu);
		return false;
	}

	public static bool GetScope_CharWhoEquippedTheItem<TKey, TValue>(BaseGameDataObject sourceObject, IDictionary<TKey, TValue> targetCollection, List<BaseGameDataObject> influencedObjects) where TKey : unmanaged where TValue : BaseGameDataObject
	{
		EquipmentBase equipmentBase = (EquipmentBase)sourceObject;
		int equippedCharId = equipmentBase.GetEquippedCharId();
		if (equippedCharId >= 0)
		{
			IDictionary<int, GameData.Domains.Character.Character> dictionary = (IDictionary<int, GameData.Domains.Character.Character>)targetCollection;
			if (dictionary.TryGetValue(equippedCharId, out var value))
			{
				influencedObjects.Add(value);
			}
		}
		return false;
	}

	public static bool GetScope_CombatSkillOwner<TKey, TValue>(BaseGameDataObject sourceObject, IDictionary<TKey, TValue> targetCollection, List<BaseGameDataObject> influencedObjects) where TKey : unmanaged where TValue : BaseGameDataObject
	{
		GameData.Domains.CombatSkill.CombatSkill combatSkill = (GameData.Domains.CombatSkill.CombatSkill)sourceObject;
		IDictionary<int, GameData.Domains.Character.Character> dictionary = (IDictionary<int, GameData.Domains.Character.Character>)targetCollection;
		GameData.Domains.Character.Character item = dictionary[combatSkill.GetId().CharId];
		influencedObjects.Add(item);
		return false;
	}

	public static bool GetScope_AllCharsInTaiwuVillage(List<BaseGameDataObject> influencedObjects)
	{
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		short taiwuVillageSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
		Settlement settlement = DomainManager.Organization.GetSettlement(taiwuVillageSettlementId);
		settlement.GetMembers().GetAllMembers(list);
		for (int i = 0; i < list.Count; i++)
		{
			influencedObjects.Add(DomainManager.Character.GetElement_Objects(list[i]));
		}
		ObjectPool<List<int>>.Instance.Return(list);
		return false;
	}

	public static bool GetScope_AllNonHeadCharsInTaiwuVillage(List<BaseGameDataObject> influencedObjects)
	{
		short taiwuVillageSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
		Settlement settlement = DomainManager.Organization.GetSettlement(taiwuVillageSettlementId);
		OrgMemberCollection members = settlement.GetMembers();
		for (sbyte b = 0; b < 7; b++)
		{
			HashSet<int> members2 = members.GetMembers(b);
			foreach (int item in members2)
			{
				influencedObjects.Add(DomainManager.Character.GetElement_Objects(item));
			}
		}
		return false;
	}

	public static bool GetScope_AllCharsInCombat(List<BaseGameDataObject> influencedObjects)
	{
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		DomainManager.Combat.GetAllCharInCombat(list);
		for (int i = 0; i < list.Count; i++)
		{
			influencedObjects.Add(DomainManager.Character.GetElement_Objects(list[i]));
		}
		ObjectPool<List<int>>.Instance.Return(list);
		return false;
	}

	public static bool GetScope_AllCombatCharsInCombat(List<BaseGameDataObject> influencedObjects)
	{
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		DomainManager.Combat.GetAllCharInCombat(list);
		for (int i = 0; i < list.Count; i++)
		{
			influencedObjects.Add(DomainManager.Combat.GetElement_CombatCharacterDict(list[i]));
		}
		ObjectPool<List<int>>.Instance.Return(list);
		return false;
	}

	public static bool GetScope_CombatSkillsOfAllCharsInCombat(List<BaseGameDataObject> influencedObjects)
	{
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		DomainManager.Combat.GetAllCharInCombat(list);
		for (int i = 0; i < list.Count; i++)
		{
			Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(list[i]);
			foreach (KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> item in charCombatSkills)
			{
				influencedObjects.Add(item.Value);
			}
		}
		ObjectPool<List<int>>.Instance.Return(list);
		return false;
	}

	public static bool GetScope_CombatSkillsOfTheChar(BaseGameDataObject sourceObject, List<BaseGameDataObject> influencedObjects)
	{
		GameData.Domains.Character.Character character = (GameData.Domains.Character.Character)sourceObject;
		int id = character.GetId();
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(id);
		foreach (KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> item in charCombatSkills)
		{
			influencedObjects.Add(item.Value);
		}
		return false;
	}

	public static bool GetScope_CombatSkillsOfTaiwuChar(BaseGameDataObject sourceObject, List<BaseGameDataObject> influencedObjects)
	{
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(taiwuCharId);
		foreach (KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> item in charCombatSkills)
		{
			influencedObjects.Add(item.Value);
		}
		return false;
	}

	public static bool GetScope_CombatSkillsOfTheCombatChar(BaseGameDataObject sourceObject, List<BaseGameDataObject> influencedObjects)
	{
		CombatCharacter combatCharacter = (CombatCharacter)sourceObject;
		int id = combatCharacter.GetId();
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(id);
		foreach (KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> item in charCombatSkills)
		{
			influencedObjects.Add(item.Value);
		}
		return false;
	}

	public static bool GetScope_CombatCharOfTheCombatSkillData(BaseGameDataObject sourceObject, List<BaseGameDataObject> influencedObjects)
	{
		CombatSkillData combatSkillData = (CombatSkillData)sourceObject;
		if (DomainManager.Combat.TryGetElement_CombatCharacterDict(combatSkillData.GetId().CharId, out var element))
		{
			influencedObjects.Add(element);
		}
		influencedObjects.Add(element);
		return false;
	}

	public static bool GetScope_CombatCharOfTheCombatWeaponData(BaseGameDataObject sourceObject, List<BaseGameDataObject> influencedObjects)
	{
		CombatWeaponData combatWeaponData = (CombatWeaponData)sourceObject;
		int weaponCharId = DomainManager.Combat.GetWeaponCharId(combatWeaponData.GetId());
		if (weaponCharId >= 0)
		{
			influencedObjects.Add(DomainManager.Combat.GetElement_CombatCharacterDict(weaponCharId));
		}
		return false;
	}

	public static bool GetScope_CombatCharOfTheChar(BaseGameDataObject sourceObject, List<BaseGameDataObject> influencedObjects)
	{
		GameData.Domains.Character.Character character = (GameData.Domains.Character.Character)sourceObject;
		if (DomainManager.Combat.IsCharInCombat(character.GetId()))
		{
			influencedObjects.Add(DomainManager.Combat.GetElement_CombatCharacterDict(character.GetId()));
		}
		return false;
	}

	public static bool GetScope_CharOfTheCombatChar(BaseGameDataObject sourceObject, List<BaseGameDataObject> influencedObjects)
	{
		influencedObjects.Add(((CombatCharacter)sourceObject).GetCharacter());
		return false;
	}

	public static bool GetScope_SectCharOfTheChar<TKey, TValue>(BaseGameDataObject sourceObject, IDictionary<TKey, TValue> targetCollection, List<BaseGameDataObject> influencedObjects) where TKey : unmanaged where TValue : BaseGameDataObject
	{
		GameData.Domains.Character.Character character = (GameData.Domains.Character.Character)sourceObject;
		IDictionary<int, SectCharacter> dictionary = (IDictionary<int, SectCharacter>)targetCollection;
		SectCharacter item = dictionary[character.GetId()];
		influencedObjects.Add(item);
		return false;
	}

	public static bool GetScope_CivilianSettlementCharOfTheChar<TKey, TValue>(BaseGameDataObject sourceObject, IDictionary<TKey, TValue> targetCollection, List<BaseGameDataObject> influencedObjects) where TKey : unmanaged where TValue : BaseGameDataObject
	{
		GameData.Domains.Character.Character character = (GameData.Domains.Character.Character)sourceObject;
		IDictionary<int, CivilianSettlementCharacter> dictionary = (IDictionary<int, CivilianSettlementCharacter>)targetCollection;
		CivilianSettlementCharacter item = dictionary[character.GetId()];
		influencedObjects.Add(item);
		return false;
	}

	public static bool GetScope_SectCharsOfTheSect<TKey, TValue>(BaseGameDataObject sourceObject, IDictionary<TKey, TValue> targetCollection, List<BaseGameDataObject> influencedObjects) where TKey : unmanaged where TValue : BaseGameDataObject
	{
		Sect sect = (Sect)sourceObject;
		OrgMemberCollection members = sect.GetMembers();
		IDictionary<int, SectCharacter> dictionary = (IDictionary<int, SectCharacter>)targetCollection;
		for (sbyte b = 0; b < 9; b++)
		{
			HashSet<int> members2 = members.GetMembers(b);
			foreach (int item in members2)
			{
				influencedObjects.Add(dictionary[item]);
			}
		}
		return false;
	}

	public static bool GetScope_CivilianSettlementCharsOfTheCivilianSettlement<TKey, TValue>(BaseGameDataObject sourceObject, IDictionary<TKey, TValue> targetCollection, List<BaseGameDataObject> influencedObjects) where TKey : unmanaged where TValue : BaseGameDataObject
	{
		CivilianSettlement civilianSettlement = (CivilianSettlement)sourceObject;
		OrgMemberCollection members = civilianSettlement.GetMembers();
		IDictionary<int, CivilianSettlementCharacter> dictionary = (IDictionary<int, CivilianSettlementCharacter>)targetCollection;
		for (sbyte b = 0; b < 9; b++)
		{
			HashSet<int> members2 = members.GetMembers(b);
			foreach (int item in members2)
			{
				influencedObjects.Add(dictionary[item]);
			}
		}
		return false;
	}

	public static bool GetScope_SectCharsOfTheChar<TKey, TValue>(BaseGameDataObject sourceObject, IDictionary<TKey, TValue> targetCollection, List<BaseGameDataObject> influencedObjects) where TKey : unmanaged where TValue : BaseGameDataObject
	{
		GameData.Domains.Character.Character character = (GameData.Domains.Character.Character)sourceObject;
		short settlementId = character.GetOrganizationInfo().SettlementId;
		Sect element_Sects = DomainManager.Organization.GetElement_Sects(settlementId);
		OrgMemberCollection members = element_Sects.GetMembers();
		IDictionary<int, SectCharacter> dictionary = (IDictionary<int, SectCharacter>)targetCollection;
		for (sbyte b = 0; b < 9; b++)
		{
			HashSet<int> members2 = members.GetMembers(b);
			foreach (int item in members2)
			{
				influencedObjects.Add(dictionary[item]);
			}
		}
		return false;
	}

	public static bool GetScope_CivilianSettlementCharsOfTheChar<TKey, TValue>(BaseGameDataObject sourceObject, IDictionary<TKey, TValue> targetCollection, List<BaseGameDataObject> influencedObjects) where TKey : unmanaged where TValue : BaseGameDataObject
	{
		GameData.Domains.Character.Character character = (GameData.Domains.Character.Character)sourceObject;
		short settlementId = character.GetOrganizationInfo().SettlementId;
		CivilianSettlement element_CivilianSettlements = DomainManager.Organization.GetElement_CivilianSettlements(settlementId);
		OrgMemberCollection members = element_CivilianSettlements.GetMembers();
		IDictionary<int, CivilianSettlementCharacter> dictionary = (IDictionary<int, CivilianSettlementCharacter>)targetCollection;
		for (sbyte b = 0; b < 9; b++)
		{
			HashSet<int> members2 = members.GetMembers(b);
			foreach (int item in members2)
			{
				influencedObjects.Add(dictionary[item]);
			}
		}
		return false;
	}

	public static bool GetScope_CharacterAffectedByTheSpecialEffects(BaseGameDataObject sourceObject, List<BaseGameDataObject> influencedObjects)
	{
		AffectedData affectedData = (AffectedData)sourceObject;
		int id = affectedData.GetId();
		influencedObjects.Add(DomainManager.Character.GetElement_Objects(id));
		return false;
	}

	public static bool GetScope_CombatSkillsOfTheCharacterAffectedByTheSpecialEffects(BaseGameDataObject sourceObject, List<BaseGameDataObject> influencedObjects)
	{
		AffectedData affectedData = (AffectedData)sourceObject;
		int id = affectedData.GetId();
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(id);
		foreach (KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> item in charCombatSkills)
		{
			influencedObjects.Add(item.Value);
		}
		return false;
	}

	public static bool GetScope_CombatCharacterAffectedByTheSpecialEffects(BaseGameDataObject sourceObject, List<BaseGameDataObject> influencedObjects)
	{
		AffectedData affectedData = (AffectedData)sourceObject;
		int id = affectedData.GetId();
		if (DomainManager.Combat.TryGetElement_CombatCharacterDict(id, out var element))
		{
			influencedObjects.Add(element);
		}
		return false;
	}

	public static bool GetScope_CombatSkillDataAffectedByTheSpecialEffects(BaseGameDataObject sourceObject, List<BaseGameDataObject> influencedObjects)
	{
		AffectedData affectedData = (AffectedData)sourceObject;
		int id = affectedData.GetId();
		if (DomainManager.Combat.TryGetElement_CombatCharacterDict(id, out var element))
		{
			foreach (CombatSkillKey combatSkillKey in element.GetCombatSkillKeys())
			{
				if (DomainManager.Combat.TryGetCombatSkillData(combatSkillKey.CharId, combatSkillKey.SkillTemplateId, out var combatSkillData))
				{
					influencedObjects.Add(combatSkillData);
				}
			}
		}
		return false;
	}

	public static bool GetScope_CombatSkillsAffectedByPowerChangeInCombat(List<BaseGameDataObject> influencedObjects)
	{
		List<CombatSkillKey> list = ObjectPool<List<CombatSkillKey>>.Instance.Get();
		list.Clear();
		list.AddRange(DomainManager.Combat.GetAllSkillPowerReduceInCombat().Keys);
		list.AddRange(DomainManager.Combat.GetAllSkillPowerAddInCombat().Keys);
		for (int i = 0; i < list.Count; i++)
		{
			influencedObjects.Add(DomainManager.CombatSkill.GetElement_CombatSkills(list[i]));
		}
		ObjectPool<List<CombatSkillKey>>.Instance.Return(list);
		return false;
	}

	public static bool GetScope_CombatSkillsAffectedByPowerReplaceInCombat(List<BaseGameDataObject> influencedObjects)
	{
		List<CombatSkillKey> list = ObjectPool<List<CombatSkillKey>>.Instance.Get();
		list.Clear();
		list.AddRange(DomainManager.Combat.GetAllSkillPowerReplaceInCombat().Keys);
		for (int i = 0; i < list.Count; i++)
		{
			influencedObjects.Add(DomainManager.CombatSkill.GetElement_CombatSkills(list[i]));
		}
		ObjectPool<List<CombatSkillKey>>.Instance.Return(list);
		return false;
	}

	public static bool GetScope_CombatSkillsAffectedByCombatSkillDataInCombat(BaseGameDataObject sourceObject, List<BaseGameDataObject> influencedObjects)
	{
		CombatSkillData combatSkillData = (CombatSkillData)sourceObject;
		influencedObjects.Add(DomainManager.CombatSkill.GetElement_CombatSkills(combatSkillData.GetId()));
		return false;
	}

	public static bool GetScope_TaiwuAndGearMates(List<BaseGameDataObject> influencedObjects)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		influencedObjects.Add(taiwu);
		List<GameData.Domains.Character.Character> allGearMateCharacter = DomainManager.Extra.GetAllGearMateCharacter();
		foreach (GameData.Domains.Character.Character item in allGearMateCharacter)
		{
			influencedObjects.Add(item);
		}
		return false;
	}
}
