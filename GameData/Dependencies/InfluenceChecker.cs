using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

namespace GameData.Dependencies
{
	// Token: 0x020008E7 RID: 2279
	public static class InfluenceChecker
	{
		// Token: 0x060081C3 RID: 33219 RVA: 0x004D80D4 File Offset: 0x004D62D4
		public static bool CheckCondition(DataContext context, BaseGameDataObject sourceObject, InfluenceCondition condition)
		{
			bool result;
			switch (condition)
			{
			case InfluenceCondition.None:
				result = true;
				break;
			case InfluenceCondition.CharIsTaiwu:
				result = InfluenceChecker.CheckCondition_CharIsTaiwu(sourceObject);
				break;
			case InfluenceCondition.CharIsInTaiwuGroup:
				result = InfluenceChecker.CheckCondition_CharIsInTaiwuGroup(sourceObject);
				break;
			case InfluenceCondition.ItemIsEquipped:
				result = InfluenceChecker.CheckCondition_ItemIsEquipped(sourceObject);
				break;
			case InfluenceCondition.CivilianSettlementIsTaiwuVillage:
				result = InfluenceChecker.CheckCondition_CivilianSettlementIsTaiwuVillage(sourceObject);
				break;
			case InfluenceCondition.CharIsInAnySect:
				result = InfluenceChecker.CheckCondition_CharIsInAnySect(sourceObject);
				break;
			case InfluenceCondition.CharIsInAnyCivilianSettlement:
				result = InfluenceChecker.CheckCondition_CharIsInAnyCivilianSettlement(sourceObject);
				break;
			case InfluenceCondition.CombatSkillIsProactive:
				result = InfluenceChecker.CheckCondition_CombatSkillIsProactive(sourceObject);
				break;
			case InfluenceCondition.CharIsTaiwuWorker:
				result = InfluenceChecker.CheckCondition_CharIsTaiwuWorker(sourceObject);
				break;
			case InfluenceCondition.CharIsTaiwuVillager:
				result = InfluenceChecker.CheckCondition_CharIsTaiwuVillager(sourceObject);
				break;
			case InfluenceCondition.CharIsTaiwuVillageHead:
				result = InfluenceChecker.CheckCondition_CharIsTaiwuVillageHead(sourceObject);
				break;
			case InfluenceCondition.CharIsTaskRelated:
				result = InfluenceChecker.CheckCondition_CharIsTaskRelated(sourceObject);
				break;
			case InfluenceCondition.CombatSkillIsLearnedByTaiwu:
				result = InfluenceChecker.CheckCondition_CombatSkillIsLearnedByTaiwu(sourceObject);
				break;
			case InfluenceCondition.CombatWeaponIsTaiwuWeapon:
				result = InfluenceChecker.CheckCondition_CombatWeaponIsTaiwuWeapon(sourceObject);
				break;
			case InfluenceCondition.CombatWeaponIsNotTaiwuWeapon:
				result = InfluenceChecker.CheckCondition_CombatWeaponIsNotTaiwuWeapon(sourceObject);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported InfluenceCondition: ");
				defaultInterpolatedStringHandler.AppendFormatted<InfluenceCondition>(condition);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x060081C4 RID: 33220 RVA: 0x004D81EC File Offset: 0x004D63EC
		public static bool GetScope<[IsUnmanaged] TKey, TValue>(DataContext context, BaseGameDataObject sourceObject, InfluenceScope scope, IDictionary<TKey, TValue> targetCollection, List<BaseGameDataObject> influencedObjects) where TKey : struct, ValueType where TValue : BaseGameDataObject
		{
			bool result;
			switch (scope)
			{
			case InfluenceScope.All:
				result = true;
				break;
			case InfluenceScope.Self:
				result = InfluenceChecker.GetScope_Self(sourceObject, influencedObjects);
				break;
			case InfluenceScope.TaiwuChar:
				result = InfluenceChecker.GetScope_TaiwuChar(influencedObjects);
				break;
			case InfluenceScope.CharWhoEquippedTheItem:
				result = InfluenceChecker.GetScope_CharWhoEquippedTheItem<TKey, TValue>(sourceObject, targetCollection, influencedObjects);
				break;
			case InfluenceScope.CombatSkillOwner:
				result = InfluenceChecker.GetScope_CombatSkillOwner<TKey, TValue>(sourceObject, targetCollection, influencedObjects);
				break;
			case InfluenceScope.AllCharsInTaiwuVillage:
				result = InfluenceChecker.GetScope_AllCharsInTaiwuVillage(influencedObjects);
				break;
			case InfluenceScope.AllNonHeadCharsInTaiwuVillage:
				result = InfluenceChecker.GetScope_AllNonHeadCharsInTaiwuVillage(influencedObjects);
				break;
			case InfluenceScope.AllCharsInCombat:
				result = InfluenceChecker.GetScope_AllCharsInCombat(influencedObjects);
				break;
			case InfluenceScope.AllCombatCharsInCombat:
				result = InfluenceChecker.GetScope_AllCombatCharsInCombat(influencedObjects);
				break;
			case InfluenceScope.CombatSkillsOfAllCharsInCombat:
				result = InfluenceChecker.GetScope_CombatSkillsOfAllCharsInCombat(influencedObjects);
				break;
			case InfluenceScope.CombatSkillsOfTheChar:
				result = InfluenceChecker.GetScope_CombatSkillsOfTheChar(sourceObject, influencedObjects);
				break;
			case InfluenceScope.CombatSkillsOfTaiwuChar:
				result = InfluenceChecker.GetScope_CombatSkillsOfTaiwuChar(sourceObject, influencedObjects);
				break;
			case InfluenceScope.CombatSkillsOfTheCombatChar:
				result = InfluenceChecker.GetScope_CombatSkillsOfTheCombatChar(sourceObject, influencedObjects);
				break;
			case InfluenceScope.CombatCharOfTheCombatSkillData:
				result = InfluenceChecker.GetScope_CombatCharOfTheCombatSkillData(sourceObject, influencedObjects);
				break;
			case InfluenceScope.CombatCharOfTheCombatWeaponData:
				result = InfluenceChecker.GetScope_CombatCharOfTheCombatWeaponData(sourceObject, influencedObjects);
				break;
			case InfluenceScope.CombatCharOfTheChar:
				result = InfluenceChecker.GetScope_CombatCharOfTheChar(sourceObject, influencedObjects);
				break;
			case InfluenceScope.CharOfTheCombatChar:
				result = InfluenceChecker.GetScope_CharOfTheCombatChar(sourceObject, influencedObjects);
				break;
			case InfluenceScope.SectCharOfTheChar:
				result = InfluenceChecker.GetScope_SectCharOfTheChar<TKey, TValue>(sourceObject, targetCollection, influencedObjects);
				break;
			case InfluenceScope.CivilianSettlementCharOfTheChar:
				result = InfluenceChecker.GetScope_CivilianSettlementCharOfTheChar<TKey, TValue>(sourceObject, targetCollection, influencedObjects);
				break;
			case InfluenceScope.SectCharsOfTheSect:
				result = InfluenceChecker.GetScope_SectCharsOfTheSect<TKey, TValue>(sourceObject, targetCollection, influencedObjects);
				break;
			case InfluenceScope.CivilianSettlementCharsOfTheCivilianSettlement:
				result = InfluenceChecker.GetScope_CivilianSettlementCharsOfTheCivilianSettlement<TKey, TValue>(sourceObject, targetCollection, influencedObjects);
				break;
			case InfluenceScope.SectCharsOfTheChar:
				result = InfluenceChecker.GetScope_SectCharsOfTheChar<TKey, TValue>(sourceObject, targetCollection, influencedObjects);
				break;
			case InfluenceScope.CivilianSettlementCharsOfTheChar:
				result = InfluenceChecker.GetScope_CivilianSettlementCharsOfTheChar<TKey, TValue>(sourceObject, targetCollection, influencedObjects);
				break;
			case InfluenceScope.CharacterAffectedByTheSpecialEffects:
				result = InfluenceChecker.GetScope_CharacterAffectedByTheSpecialEffects(sourceObject, influencedObjects);
				break;
			case InfluenceScope.CombatSkillsOfTheCharacterAffectedByTheSpecialEffects:
				result = InfluenceChecker.GetScope_CombatSkillsOfTheCharacterAffectedByTheSpecialEffects(sourceObject, influencedObjects);
				break;
			case InfluenceScope.CombatCharacterAffectedByTheSpecialEffects:
				result = InfluenceChecker.GetScope_CombatCharacterAffectedByTheSpecialEffects(sourceObject, influencedObjects);
				break;
			case InfluenceScope.CombatSkillDataAffectedByTheSpecialEffects:
				result = InfluenceChecker.GetScope_CombatSkillDataAffectedByTheSpecialEffects(sourceObject, influencedObjects);
				break;
			case InfluenceScope.CombatSkillsAffectedByPowerChangeInCombat:
				result = InfluenceChecker.GetScope_CombatSkillsAffectedByPowerChangeInCombat(influencedObjects);
				break;
			case InfluenceScope.CombatSkillsAffectedByPowerReplaceInCombat:
				result = InfluenceChecker.GetScope_CombatSkillsAffectedByPowerReplaceInCombat(influencedObjects);
				break;
			case InfluenceScope.CombatSkillsAffectedByCombatSkillDataInCombat:
				result = InfluenceChecker.GetScope_CombatSkillsAffectedByCombatSkillDataInCombat(sourceObject, influencedObjects);
				break;
			case InfluenceScope.TaiwuAndGearMates:
				result = InfluenceChecker.GetScope_TaiwuAndGearMates(influencedObjects);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported InfluenceScope: ");
				defaultInterpolatedStringHandler.AppendFormatted<InfluenceScope>(scope);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x060081C5 RID: 33221 RVA: 0x004D8448 File Offset: 0x004D6648
		public static bool CheckCondition_CharIsTaiwu(BaseGameDataObject sourceObject)
		{
			return sourceObject == DomainManager.Taiwu.GetTaiwu();
		}

		// Token: 0x060081C6 RID: 33222 RVA: 0x004D8468 File Offset: 0x004D6668
		public static bool CheckCondition_CharIsInTaiwuGroup(BaseGameDataObject sourceObject)
		{
			GameData.Domains.Character.Character character = (GameData.Domains.Character.Character)sourceObject;
			return character.GetLeaderId() == DomainManager.Taiwu.GetTaiwuCharId();
		}

		// Token: 0x060081C7 RID: 33223 RVA: 0x004D8494 File Offset: 0x004D6694
		public static bool CheckCondition_ItemIsEquipped(BaseGameDataObject sourceObject)
		{
			EquipmentBase baseItem = (EquipmentBase)sourceObject;
			return baseItem.GetEquippedCharId() >= 0;
		}

		// Token: 0x060081C8 RID: 33224 RVA: 0x004D84BC File Offset: 0x004D66BC
		public static bool CheckCondition_CivilianSettlementIsTaiwuVillage(BaseGameDataObject sourceObject)
		{
			CivilianSettlement civilianSettlement = (CivilianSettlement)sourceObject;
			return civilianSettlement.GetId() == DomainManager.Taiwu.GetTaiwuVillageSettlementId();
		}

		// Token: 0x060081C9 RID: 33225 RVA: 0x004D84E8 File Offset: 0x004D66E8
		public static bool CheckCondition_CharIsInAnySect(BaseGameDataObject sourceObject)
		{
			GameData.Domains.Character.Character character = (GameData.Domains.Character.Character)sourceObject;
			return OrganizationDomain.IsSect(character.GetOrganizationInfo().OrgTemplateId);
		}

		// Token: 0x060081CA RID: 33226 RVA: 0x004D8514 File Offset: 0x004D6714
		public static bool CheckCondition_CharIsInAnyCivilianSettlement(BaseGameDataObject sourceObject)
		{
			GameData.Domains.Character.Character character = (GameData.Domains.Character.Character)sourceObject;
			return !OrganizationDomain.IsSect(character.GetOrganizationInfo().OrgTemplateId);
		}

		// Token: 0x060081CB RID: 33227 RVA: 0x004D8540 File Offset: 0x004D6740
		public static bool CheckCondition_CombatSkillIsProactive(BaseGameDataObject sourceObject)
		{
			short skillTemplateId = ((GameData.Domains.CombatSkill.CombatSkill)sourceObject).GetId().SkillTemplateId;
			CombatSkillItem config = Config.CombatSkill.Instance[skillTemplateId];
			return GameData.Domains.Character.CombatSkillHelper.IsProactiveSkill(config.EquipType);
		}

		// Token: 0x060081CC RID: 33228 RVA: 0x004D857C File Offset: 0x004D677C
		public static bool CheckCondition_CombatSkillIsLearnedByTaiwu(BaseGameDataObject sourceObject)
		{
			return ((GameData.Domains.CombatSkill.CombatSkill)sourceObject).GetId().CharId == DomainManager.Taiwu.GetTaiwuCharId();
		}

		// Token: 0x060081CD RID: 33229 RVA: 0x004D85AC File Offset: 0x004D67AC
		public static bool CheckCondition_CombatWeaponIsTaiwuWeapon(BaseGameDataObject sourceObject)
		{
			return ((CombatWeaponData)sourceObject).Character.GetId() == DomainManager.Taiwu.GetTaiwuCharId();
		}

		// Token: 0x060081CE RID: 33230 RVA: 0x004D85DC File Offset: 0x004D67DC
		public static bool CheckCondition_CombatWeaponIsNotTaiwuWeapon(BaseGameDataObject sourceObject)
		{
			return ((CombatWeaponData)sourceObject).Character.GetId() != DomainManager.Taiwu.GetTaiwuCharId();
		}

		// Token: 0x060081CF RID: 33231 RVA: 0x004D8610 File Offset: 0x004D6810
		public static bool CheckCondition_CharIsTaiwuWorker(BaseGameDataObject sourceObject)
		{
			GameData.Domains.Character.Character character = (GameData.Domains.Character.Character)sourceObject;
			return character.IsActiveExternalRelationState(1);
		}

		// Token: 0x060081D0 RID: 33232 RVA: 0x004D8630 File Offset: 0x004D6830
		public static bool CheckCondition_CharIsTaiwuVillager(BaseGameDataObject sourceObject)
		{
			GameData.Domains.Character.Character character = (GameData.Domains.Character.Character)sourceObject;
			return character.GetOrganizationInfo().OrgTemplateId == 16;
		}

		// Token: 0x060081D1 RID: 33233 RVA: 0x004D8658 File Offset: 0x004D6858
		public static bool CheckCondition_CharIsTaiwuVillageHead(BaseGameDataObject sourceObject)
		{
			GameData.Domains.Character.Character character = (GameData.Domains.Character.Character)sourceObject;
			OrganizationInfo orgInfo = character.GetOrganizationInfo();
			return orgInfo.OrgTemplateId == 16 && orgInfo.Grade == 7;
		}

		// Token: 0x060081D2 RID: 33234 RVA: 0x004D8690 File Offset: 0x004D6890
		public static bool CheckCondition_CharIsTaskRelated(BaseGameDataObject sourceObject)
		{
			GameData.Domains.Character.Character character = (GameData.Domains.Character.Character)sourceObject;
			return character == DomainManager.Taiwu.GetTaiwu() || Config.Character.Instance[character.GetTemplateId()].CreatingType == 0;
		}

		// Token: 0x060081D3 RID: 33235 RVA: 0x004D86D4 File Offset: 0x004D68D4
		public static bool GetScope_Self(BaseGameDataObject sourceObject, List<BaseGameDataObject> influencedObjects)
		{
			influencedObjects.Add(sourceObject);
			return false;
		}

		// Token: 0x060081D4 RID: 33236 RVA: 0x004D86F0 File Offset: 0x004D68F0
		public static bool GetScope_TaiwuChar(List<BaseGameDataObject> influencedObjects)
		{
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			influencedObjects.Add(taiwuChar);
			return false;
		}

		// Token: 0x060081D5 RID: 33237 RVA: 0x004D8718 File Offset: 0x004D6918
		public static bool GetScope_CharWhoEquippedTheItem<[IsUnmanaged] TKey, TValue>(BaseGameDataObject sourceObject, IDictionary<TKey, TValue> targetCollection, List<BaseGameDataObject> influencedObjects) where TKey : struct, ValueType where TValue : BaseGameDataObject
		{
			EquipmentBase baseItem = (EquipmentBase)sourceObject;
			int equippedCharId = baseItem.GetEquippedCharId();
			bool flag = equippedCharId >= 0;
			if (flag)
			{
				IDictionary<int, GameData.Domains.Character.Character> characters = (IDictionary<int, GameData.Domains.Character.Character>)targetCollection;
				GameData.Domains.Character.Character character;
				bool flag2 = characters.TryGetValue(equippedCharId, out character);
				if (flag2)
				{
					influencedObjects.Add(character);
				}
			}
			return false;
		}

		// Token: 0x060081D6 RID: 33238 RVA: 0x004D8768 File Offset: 0x004D6968
		public static bool GetScope_CombatSkillOwner<[IsUnmanaged] TKey, TValue>(BaseGameDataObject sourceObject, IDictionary<TKey, TValue> targetCollection, List<BaseGameDataObject> influencedObjects) where TKey : struct, ValueType where TValue : BaseGameDataObject
		{
			GameData.Domains.CombatSkill.CombatSkill combatSkill = (GameData.Domains.CombatSkill.CombatSkill)sourceObject;
			IDictionary<int, GameData.Domains.Character.Character> characters = (IDictionary<int, GameData.Domains.Character.Character>)targetCollection;
			GameData.Domains.Character.Character character = characters[combatSkill.GetId().CharId];
			influencedObjects.Add(character);
			return false;
		}

		// Token: 0x060081D7 RID: 33239 RVA: 0x004D87A4 File Offset: 0x004D69A4
		public static bool GetScope_AllCharsInTaiwuVillage(List<BaseGameDataObject> influencedObjects)
		{
			List<int> memberIds = ObjectPool<List<int>>.Instance.Get();
			short taiwuVillageSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
			Settlement taiwuVillageSettlement = DomainManager.Organization.GetSettlement(taiwuVillageSettlementId);
			taiwuVillageSettlement.GetMembers().GetAllMembers(memberIds);
			for (int i = 0; i < memberIds.Count; i++)
			{
				influencedObjects.Add(DomainManager.Character.GetElement_Objects(memberIds[i]));
			}
			ObjectPool<List<int>>.Instance.Return(memberIds);
			return false;
		}

		// Token: 0x060081D8 RID: 33240 RVA: 0x004D8824 File Offset: 0x004D6A24
		public static bool GetScope_AllNonHeadCharsInTaiwuVillage(List<BaseGameDataObject> influencedObjects)
		{
			short taiwuVillageSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
			Settlement taiwuVillageSettlement = DomainManager.Organization.GetSettlement(taiwuVillageSettlementId);
			OrgMemberCollection members = taiwuVillageSettlement.GetMembers();
			for (sbyte grade = 0; grade < 7; grade += 1)
			{
				HashSet<int> gradeMembers = members.GetMembers(grade);
				foreach (int memberId in gradeMembers)
				{
					influencedObjects.Add(DomainManager.Character.GetElement_Objects(memberId));
				}
			}
			return false;
		}

		// Token: 0x060081D9 RID: 33241 RVA: 0x004D88C8 File Offset: 0x004D6AC8
		public static bool GetScope_AllCharsInCombat(List<BaseGameDataObject> influencedObjects)
		{
			List<int> allCharsInCombat = ObjectPool<List<int>>.Instance.Get();
			DomainManager.Combat.GetAllCharInCombat(allCharsInCombat);
			for (int i = 0; i < allCharsInCombat.Count; i++)
			{
				influencedObjects.Add(DomainManager.Character.GetElement_Objects(allCharsInCombat[i]));
			}
			ObjectPool<List<int>>.Instance.Return(allCharsInCombat);
			return false;
		}

		// Token: 0x060081DA RID: 33242 RVA: 0x004D892C File Offset: 0x004D6B2C
		public static bool GetScope_AllCombatCharsInCombat(List<BaseGameDataObject> influencedObjects)
		{
			List<int> allCharsInCombat = ObjectPool<List<int>>.Instance.Get();
			DomainManager.Combat.GetAllCharInCombat(allCharsInCombat);
			for (int i = 0; i < allCharsInCombat.Count; i++)
			{
				influencedObjects.Add(DomainManager.Combat.GetElement_CombatCharacterDict(allCharsInCombat[i]));
			}
			ObjectPool<List<int>>.Instance.Return(allCharsInCombat);
			return false;
		}

		// Token: 0x060081DB RID: 33243 RVA: 0x004D8990 File Offset: 0x004D6B90
		public static bool GetScope_CombatSkillsOfAllCharsInCombat(List<BaseGameDataObject> influencedObjects)
		{
			List<int> allCharsInCombat = ObjectPool<List<int>>.Instance.Get();
			DomainManager.Combat.GetAllCharInCombat(allCharsInCombat);
			for (int i = 0; i < allCharsInCombat.Count; i++)
			{
				Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> combatSkills = DomainManager.CombatSkill.GetCharCombatSkills(allCharsInCombat[i]);
				foreach (KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> entry in combatSkills)
				{
					influencedObjects.Add(entry.Value);
				}
			}
			ObjectPool<List<int>>.Instance.Return(allCharsInCombat);
			return false;
		}

		// Token: 0x060081DC RID: 33244 RVA: 0x004D8A40 File Offset: 0x004D6C40
		public static bool GetScope_CombatSkillsOfTheChar(BaseGameDataObject sourceObject, List<BaseGameDataObject> influencedObjects)
		{
			GameData.Domains.Character.Character character = (GameData.Domains.Character.Character)sourceObject;
			int charId = character.GetId();
			Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> combatSkills = DomainManager.CombatSkill.GetCharCombatSkills(charId);
			foreach (KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> entry in combatSkills)
			{
				influencedObjects.Add(entry.Value);
			}
			return false;
		}

		// Token: 0x060081DD RID: 33245 RVA: 0x004D8ABC File Offset: 0x004D6CBC
		public static bool GetScope_CombatSkillsOfTaiwuChar(BaseGameDataObject sourceObject, List<BaseGameDataObject> influencedObjects)
		{
			int charId = DomainManager.Taiwu.GetTaiwuCharId();
			Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> combatSkills = DomainManager.CombatSkill.GetCharCombatSkills(charId);
			foreach (KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> entry in combatSkills)
			{
				influencedObjects.Add(entry.Value);
			}
			return false;
		}

		// Token: 0x060081DE RID: 33246 RVA: 0x004D8B34 File Offset: 0x004D6D34
		public static bool GetScope_CombatSkillsOfTheCombatChar(BaseGameDataObject sourceObject, List<BaseGameDataObject> influencedObjects)
		{
			CombatCharacter combatChar = (CombatCharacter)sourceObject;
			int charId = combatChar.GetId();
			Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> combatSkills = DomainManager.CombatSkill.GetCharCombatSkills(charId);
			foreach (KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> entry in combatSkills)
			{
				influencedObjects.Add(entry.Value);
			}
			return false;
		}

		// Token: 0x060081DF RID: 33247 RVA: 0x004D8BB0 File Offset: 0x004D6DB0
		public static bool GetScope_CombatCharOfTheCombatSkillData(BaseGameDataObject sourceObject, List<BaseGameDataObject> influencedObjects)
		{
			CombatSkillData data = (CombatSkillData)sourceObject;
			CombatCharacter combatChar;
			bool flag = DomainManager.Combat.TryGetElement_CombatCharacterDict(data.GetId().CharId, out combatChar);
			if (flag)
			{
				influencedObjects.Add(combatChar);
			}
			influencedObjects.Add(combatChar);
			return false;
		}

		// Token: 0x060081E0 RID: 33248 RVA: 0x004D8BF8 File Offset: 0x004D6DF8
		public static bool GetScope_CombatCharOfTheCombatWeaponData(BaseGameDataObject sourceObject, List<BaseGameDataObject> influencedObjects)
		{
			CombatWeaponData data = (CombatWeaponData)sourceObject;
			int charId = DomainManager.Combat.GetWeaponCharId(data.GetId());
			bool flag = charId >= 0;
			if (flag)
			{
				influencedObjects.Add(DomainManager.Combat.GetElement_CombatCharacterDict(charId));
			}
			return false;
		}

		// Token: 0x060081E1 RID: 33249 RVA: 0x004D8C40 File Offset: 0x004D6E40
		public static bool GetScope_CombatCharOfTheChar(BaseGameDataObject sourceObject, List<BaseGameDataObject> influencedObjects)
		{
			GameData.Domains.Character.Character character = (GameData.Domains.Character.Character)sourceObject;
			bool flag = DomainManager.Combat.IsCharInCombat(character.GetId(), true);
			if (flag)
			{
				influencedObjects.Add(DomainManager.Combat.GetElement_CombatCharacterDict(character.GetId()));
			}
			return false;
		}

		// Token: 0x060081E2 RID: 33250 RVA: 0x004D8C88 File Offset: 0x004D6E88
		public static bool GetScope_CharOfTheCombatChar(BaseGameDataObject sourceObject, List<BaseGameDataObject> influencedObjects)
		{
			influencedObjects.Add(((CombatCharacter)sourceObject).GetCharacter());
			return false;
		}

		// Token: 0x060081E3 RID: 33251 RVA: 0x004D8CB0 File Offset: 0x004D6EB0
		public static bool GetScope_SectCharOfTheChar<[IsUnmanaged] TKey, TValue>(BaseGameDataObject sourceObject, IDictionary<TKey, TValue> targetCollection, List<BaseGameDataObject> influencedObjects) where TKey : struct, ValueType where TValue : BaseGameDataObject
		{
			GameData.Domains.Character.Character character = (GameData.Domains.Character.Character)sourceObject;
			IDictionary<int, SectCharacter> sectCharacters = (IDictionary<int, SectCharacter>)targetCollection;
			SectCharacter sectCharacter = sectCharacters[character.GetId()];
			influencedObjects.Add(sectCharacter);
			return false;
		}

		// Token: 0x060081E4 RID: 33252 RVA: 0x004D8CE8 File Offset: 0x004D6EE8
		public static bool GetScope_CivilianSettlementCharOfTheChar<[IsUnmanaged] TKey, TValue>(BaseGameDataObject sourceObject, IDictionary<TKey, TValue> targetCollection, List<BaseGameDataObject> influencedObjects) where TKey : struct, ValueType where TValue : BaseGameDataObject
		{
			GameData.Domains.Character.Character character = (GameData.Domains.Character.Character)sourceObject;
			IDictionary<int, CivilianSettlementCharacter> civilianSettlementChars = (IDictionary<int, CivilianSettlementCharacter>)targetCollection;
			CivilianSettlementCharacter civilianSettlementChar = civilianSettlementChars[character.GetId()];
			influencedObjects.Add(civilianSettlementChar);
			return false;
		}

		// Token: 0x060081E5 RID: 33253 RVA: 0x004D8D20 File Offset: 0x004D6F20
		public static bool GetScope_SectCharsOfTheSect<[IsUnmanaged] TKey, TValue>(BaseGameDataObject sourceObject, IDictionary<TKey, TValue> targetCollection, List<BaseGameDataObject> influencedObjects) where TKey : struct, ValueType where TValue : BaseGameDataObject
		{
			Sect sect = (Sect)sourceObject;
			OrgMemberCollection members = sect.GetMembers();
			IDictionary<int, SectCharacter> sectCharacters = (IDictionary<int, SectCharacter>)targetCollection;
			for (sbyte grade = 0; grade < 9; grade += 1)
			{
				HashSet<int> gradeMembers = members.GetMembers(grade);
				foreach (int charId in gradeMembers)
				{
					influencedObjects.Add(sectCharacters[charId]);
				}
			}
			return false;
		}

		// Token: 0x060081E6 RID: 33254 RVA: 0x004D8DB8 File Offset: 0x004D6FB8
		public static bool GetScope_CivilianSettlementCharsOfTheCivilianSettlement<[IsUnmanaged] TKey, TValue>(BaseGameDataObject sourceObject, IDictionary<TKey, TValue> targetCollection, List<BaseGameDataObject> influencedObjects) where TKey : struct, ValueType where TValue : BaseGameDataObject
		{
			CivilianSettlement civilianSettlement = (CivilianSettlement)sourceObject;
			OrgMemberCollection members = civilianSettlement.GetMembers();
			IDictionary<int, CivilianSettlementCharacter> civilianSettlementChars = (IDictionary<int, CivilianSettlementCharacter>)targetCollection;
			for (sbyte grade = 0; grade < 9; grade += 1)
			{
				HashSet<int> gradeMembers = members.GetMembers(grade);
				foreach (int charId in gradeMembers)
				{
					influencedObjects.Add(civilianSettlementChars[charId]);
				}
			}
			return false;
		}

		// Token: 0x060081E7 RID: 33255 RVA: 0x004D8E50 File Offset: 0x004D7050
		public static bool GetScope_SectCharsOfTheChar<[IsUnmanaged] TKey, TValue>(BaseGameDataObject sourceObject, IDictionary<TKey, TValue> targetCollection, List<BaseGameDataObject> influencedObjects) where TKey : struct, ValueType where TValue : BaseGameDataObject
		{
			GameData.Domains.Character.Character character = (GameData.Domains.Character.Character)sourceObject;
			short settlementId = character.GetOrganizationInfo().SettlementId;
			Sect sect = DomainManager.Organization.GetElement_Sects(settlementId);
			OrgMemberCollection members = sect.GetMembers();
			IDictionary<int, SectCharacter> sectCharacters = (IDictionary<int, SectCharacter>)targetCollection;
			for (sbyte grade = 0; grade < 9; grade += 1)
			{
				HashSet<int> gradeMembers = members.GetMembers(grade);
				foreach (int charId in gradeMembers)
				{
					influencedObjects.Add(sectCharacters[charId]);
				}
			}
			return false;
		}

		// Token: 0x060081E8 RID: 33256 RVA: 0x004D8F08 File Offset: 0x004D7108
		public static bool GetScope_CivilianSettlementCharsOfTheChar<[IsUnmanaged] TKey, TValue>(BaseGameDataObject sourceObject, IDictionary<TKey, TValue> targetCollection, List<BaseGameDataObject> influencedObjects) where TKey : struct, ValueType where TValue : BaseGameDataObject
		{
			GameData.Domains.Character.Character character = (GameData.Domains.Character.Character)sourceObject;
			short settlementId = character.GetOrganizationInfo().SettlementId;
			CivilianSettlement civilianSettlement = DomainManager.Organization.GetElement_CivilianSettlements(settlementId);
			OrgMemberCollection members = civilianSettlement.GetMembers();
			IDictionary<int, CivilianSettlementCharacter> civilianSettlementChars = (IDictionary<int, CivilianSettlementCharacter>)targetCollection;
			for (sbyte grade = 0; grade < 9; grade += 1)
			{
				HashSet<int> gradeMembers = members.GetMembers(grade);
				foreach (int charId in gradeMembers)
				{
					influencedObjects.Add(civilianSettlementChars[charId]);
				}
			}
			return false;
		}

		// Token: 0x060081E9 RID: 33257 RVA: 0x004D8FC0 File Offset: 0x004D71C0
		public static bool GetScope_CharacterAffectedByTheSpecialEffects(BaseGameDataObject sourceObject, List<BaseGameDataObject> influencedObjects)
		{
			AffectedData affectedData = (AffectedData)sourceObject;
			int charId = affectedData.GetId();
			influencedObjects.Add(DomainManager.Character.GetElement_Objects(charId));
			return false;
		}

		// Token: 0x060081EA RID: 33258 RVA: 0x004D8FF4 File Offset: 0x004D71F4
		public static bool GetScope_CombatSkillsOfTheCharacterAffectedByTheSpecialEffects(BaseGameDataObject sourceObject, List<BaseGameDataObject> influencedObjects)
		{
			AffectedData affectedData = (AffectedData)sourceObject;
			int charId = affectedData.GetId();
			Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> combatSkills = DomainManager.CombatSkill.GetCharCombatSkills(charId);
			foreach (KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> entry in combatSkills)
			{
				influencedObjects.Add(entry.Value);
			}
			return false;
		}

		// Token: 0x060081EB RID: 33259 RVA: 0x004D9070 File Offset: 0x004D7270
		public static bool GetScope_CombatCharacterAffectedByTheSpecialEffects(BaseGameDataObject sourceObject, List<BaseGameDataObject> influencedObjects)
		{
			AffectedData affectedData = (AffectedData)sourceObject;
			int charId = affectedData.GetId();
			CombatCharacter influencedObject;
			bool flag = DomainManager.Combat.TryGetElement_CombatCharacterDict(charId, out influencedObject);
			if (flag)
			{
				influencedObjects.Add(influencedObject);
			}
			return false;
		}

		// Token: 0x060081EC RID: 33260 RVA: 0x004D90AC File Offset: 0x004D72AC
		public static bool GetScope_CombatSkillDataAffectedByTheSpecialEffects(BaseGameDataObject sourceObject, List<BaseGameDataObject> influencedObjects)
		{
			AffectedData affectedData = (AffectedData)sourceObject;
			int charId = affectedData.GetId();
			CombatCharacter combatChar;
			bool flag = DomainManager.Combat.TryGetElement_CombatCharacterDict(charId, out combatChar);
			if (flag)
			{
				foreach (CombatSkillKey combatSkillKey in combatChar.GetCombatSkillKeys())
				{
					CombatSkillData combatSkillData;
					bool flag2 = DomainManager.Combat.TryGetCombatSkillData(combatSkillKey.CharId, combatSkillKey.SkillTemplateId, out combatSkillData);
					if (flag2)
					{
						influencedObjects.Add(combatSkillData);
					}
				}
			}
			return false;
		}

		// Token: 0x060081ED RID: 33261 RVA: 0x004D914C File Offset: 0x004D734C
		public static bool GetScope_CombatSkillsAffectedByPowerChangeInCombat(List<BaseGameDataObject> influencedObjects)
		{
			List<CombatSkillKey> skillKeys = ObjectPool<List<CombatSkillKey>>.Instance.Get();
			skillKeys.Clear();
			skillKeys.AddRange(DomainManager.Combat.GetAllSkillPowerReduceInCombat().Keys);
			skillKeys.AddRange(DomainManager.Combat.GetAllSkillPowerAddInCombat().Keys);
			for (int i = 0; i < skillKeys.Count; i++)
			{
				influencedObjects.Add(DomainManager.CombatSkill.GetElement_CombatSkills(skillKeys[i]));
			}
			ObjectPool<List<CombatSkillKey>>.Instance.Return(skillKeys);
			return false;
		}

		// Token: 0x060081EE RID: 33262 RVA: 0x004D91D8 File Offset: 0x004D73D8
		public static bool GetScope_CombatSkillsAffectedByPowerReplaceInCombat(List<BaseGameDataObject> influencedObjects)
		{
			List<CombatSkillKey> skillKeys = ObjectPool<List<CombatSkillKey>>.Instance.Get();
			skillKeys.Clear();
			skillKeys.AddRange(DomainManager.Combat.GetAllSkillPowerReplaceInCombat().Keys);
			for (int i = 0; i < skillKeys.Count; i++)
			{
				influencedObjects.Add(DomainManager.CombatSkill.GetElement_CombatSkills(skillKeys[i]));
			}
			ObjectPool<List<CombatSkillKey>>.Instance.Return(skillKeys);
			return false;
		}

		// Token: 0x060081EF RID: 33263 RVA: 0x004D924C File Offset: 0x004D744C
		public static bool GetScope_CombatSkillsAffectedByCombatSkillDataInCombat(BaseGameDataObject sourceObject, List<BaseGameDataObject> influencedObjects)
		{
			CombatSkillData combatSkillData = (CombatSkillData)sourceObject;
			influencedObjects.Add(DomainManager.CombatSkill.GetElement_CombatSkills(combatSkillData.GetId()));
			return false;
		}

		// Token: 0x060081F0 RID: 33264 RVA: 0x004D9280 File Offset: 0x004D7480
		public static bool GetScope_TaiwuAndGearMates(List<BaseGameDataObject> influencedObjects)
		{
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			influencedObjects.Add(taiwuChar);
			List<GameData.Domains.Character.Character> gearMates = DomainManager.Extra.GetAllGearMateCharacter();
			foreach (GameData.Domains.Character.Character gearMate in gearMates)
			{
				influencedObjects.Add(gearMate);
			}
			return false;
		}

		// Token: 0x040023E5 RID: 9189
		public static readonly LocalObjectPool<List<BaseGameDataObject>> InfluencedObjectsPool = new LocalObjectPool<List<BaseGameDataObject>>(8, 16);
	}
}
