using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Config;
using GameData.Domains.Building;
using GameData.Domains.Character;
using GameData.Domains.Character.Relation;
using GameData.Domains.Combat;
using GameData.Domains.Information;
using GameData.Domains.Item;
using GameData.Domains.Item.Display;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.TaiwuEvent.EventHelper;

namespace GameData.Domains.TaiwuEvent.EventOption
{
	// Token: 0x020000AF RID: 175
	public static class OptionConditionMatcher
	{
		// Token: 0x06001B56 RID: 6998 RVA: 0x0017BD38 File Offset: 0x00179F38
		public static bool CharacterNotInGroup(GameData.Domains.Character.Character arg0)
		{
			return !DomainManager.Taiwu.GetGroupCharIds().Contains(arg0.GetId());
		}

		// Token: 0x06001B57 RID: 6999 RVA: 0x0017BD68 File Offset: 0x00179F68
		public static bool TeamMateLess(int arg0)
		{
			return EventHelper.GetValidGroupCharacterCount(true, false, true) < arg0;
		}

		// Token: 0x06001B58 RID: 7000 RVA: 0x0017BD88 File Offset: 0x00179F88
		public static bool MovePointMore(sbyte arg0)
		{
			return DomainManager.World.GetLeftDaysInCurrMonth() >= (int)arg0;
		}

		// Token: 0x06001B59 RID: 7001 RVA: 0x0017BDAC File Offset: 0x00179FAC
		public static bool AuthorityMore(int arg0)
		{
			GameData.Domains.Character.Character taiwuChar;
			bool flag = DomainManager.Character.TryGetElement_Objects(EventArgBox.TaiwuCharacterId, out taiwuChar);
			return flag && taiwuChar.GetResource(7) >= arg0;
		}

		// Token: 0x06001B5A RID: 7002 RVA: 0x0017BDE8 File Offset: 0x00179FE8
		public static bool MoneyMore(int arg0)
		{
			GameData.Domains.Character.Character taiwuChar;
			bool flag = DomainManager.Character.TryGetElement_Objects(EventArgBox.TaiwuCharacterId, out taiwuChar);
			return flag && taiwuChar.GetResource(6) >= arg0;
		}

		// Token: 0x06001B5B RID: 7003 RVA: 0x0017BE24 File Offset: 0x0017A024
		public static bool MonthCooldownCount(string arg0, int arg1, sbyte arg2)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted(arg0);
			defaultInterpolatedStringHandler.AppendLiteral("_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(arg1);
			string boxKey = defaultInterpolatedStringHandler.ToStringAndClear();
			EventArgBox globalBox = EventHelper.GetGlobalArgBox();
			int logMonth = -1;
			bool flag = globalBox.Get(boxKey, ref logMonth);
			bool result;
			if (flag)
			{
				int curMonth = DomainManager.World.GetCurrDate();
				result = (curMonth - logMonth >= (int)arg2);
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06001B5C RID: 7004 RVA: 0x0017BE9C File Offset: 0x0017A09C
		public static bool AreaSpiritualDebtMore(MapAreaData arg0, short arg1)
		{
			return DomainManager.Extra.GetAreaSpiritualDebt(arg0.GetAreaId()) >= (int)arg1;
		}

		// Token: 0x06001B5D RID: 7005 RVA: 0x0017BEC4 File Offset: 0x0017A0C4
		public static bool FavorAtLeast(int arg0, int arg1, sbyte arg2)
		{
			short favorValue = DomainManager.Character.GetFavorability(arg0, arg1);
			return FavorabilityType.GetFavorabilityType(favorValue) >= arg2;
		}

		// Token: 0x06001B5E RID: 7006 RVA: 0x0017BEF0 File Offset: 0x0017A0F0
		public static bool ProfessionFavorAtLeast(int arg0, int arg1)
		{
			short favorValue = DomainManager.Character.GetFavorability(arg0, arg1);
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(arg0);
			sbyte behaviorType = character.GetBehaviorType();
			sbyte favorType = EventHelper.GetProfessionFavorabilityType(behaviorType);
			return FavorabilityType.GetFavorabilityType(favorValue) >= favorType;
		}

		// Token: 0x06001B5F RID: 7007 RVA: 0x0017BF38 File Offset: 0x0017A138
		public static bool SwornBrotherOrSisterLess(int arg0)
		{
			HashSet<int> swordCharIds = DomainManager.Character.GetRelatedCharIds(EventArgBox.TaiwuCharacterId, 512);
			int aliveCharCount = 0;
			foreach (int charId in swordCharIds)
			{
				bool flag = DomainManager.Character.IsCharacterAlive(charId);
				if (flag)
				{
					aliveCharCount++;
				}
			}
			return aliveCharCount < arg0;
		}

		// Token: 0x06001B60 RID: 7008 RVA: 0x0017BFBC File Offset: 0x0017A1BC
		public static bool CharacterFameNegative(int arg0, string arg1)
		{
			EventArgBox box = EventHelper.GetSecretInformationParameters(arg0);
			int reactorId = -1;
			box.Get(arg1, ref reactorId);
			sbyte fameReactor = EventHelper.GetSecretInformationCharacterFameTypeSnapshot(arg0, reactorId);
			return !FameType.IsNonNegative(fameReactor, false);
		}

		// Token: 0x06001B61 RID: 7009 RVA: 0x0017BFF4 File Offset: 0x0017A1F4
		public static bool SecretInformationLoverOf(int arg0, string arg1)
		{
			EventArgBox box = EventHelper.GetSecretInformationParameters(arg0);
			int reactorId = -1;
			box.Get(arg1, ref reactorId);
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			HashSet<SecretInformationRelationshipType> pastRelationsBetweenTaiwuAndReactor = EventHelper.CheckSecretInformationRelationshipWithSnapshot(reactorId, arg0, taiwuCharId, arg0);
			IEnumerable<SecretInformationRelationshipType> union = pastRelationsBetweenTaiwuAndReactor.Intersect(new SecretInformationRelationshipType[]
			{
				SecretInformationRelationshipType.Lover
			});
			return union.Any<SecretInformationRelationshipType>();
		}

		// Token: 0x06001B62 RID: 7010 RVA: 0x0017C04C File Offset: 0x0017A24C
		public static bool SecretInformationIsVictim(int arg0, string arg1)
		{
			EventArgBox box = EventHelper.GetSecretInformationParameters(arg0);
			int reactorId = -1;
			box.Get(arg1, ref reactorId);
			return DomainManager.Taiwu.GetTaiwuCharId() == reactorId;
		}

		// Token: 0x06001B63 RID: 7011 RVA: 0x0017C080 File Offset: 0x0017A280
		public static bool CharacterFamePositive(int arg0, string arg1)
		{
			EventArgBox box = EventHelper.GetSecretInformationParameters(arg0);
			int reactorId = -1;
			box.Get(arg1, ref reactorId);
			sbyte fameReactor = EventHelper.GetSecretInformationCharacterFameTypeSnapshot(arg0, reactorId);
			return FameType.IsNonNegative(fameReactor, true);
		}

		// Token: 0x06001B64 RID: 7012 RVA: 0x0017C0B4 File Offset: 0x0017A2B4
		public static bool SecretInformationGoodRelationOf(int arg0, string arg1)
		{
			EventArgBox box = EventHelper.GetSecretInformationParameters(arg0);
			int reactorId = -1;
			box.Get(arg1, ref reactorId);
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			HashSet<SecretInformationRelationshipType> pastRelationsBetweenTaiwuAndReactor = EventHelper.CheckSecretInformationRelationshipWithSnapshot(reactorId, arg0, taiwuCharId, arg0);
			IEnumerable<SecretInformationRelationshipType> union = pastRelationsBetweenTaiwuAndReactor.Intersect(new SecretInformationRelationshipType[]
			{
				SecretInformationRelationshipType.Allied
			});
			return union.Any<SecretInformationRelationshipType>();
		}

		// Token: 0x06001B65 RID: 7013 RVA: 0x0017C10C File Offset: 0x0017A30C
		public static bool TaiwuIsBehaviorType(sbyte arg0, sbyte arg1, sbyte arg2, sbyte arg3, sbyte arg4)
		{
			sbyte beh = DomainManager.Taiwu.GetTaiwu().GetBehaviorType();
			bool flag = beh == arg0;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = beh == arg1;
				if (flag2)
				{
					result = true;
				}
				else
				{
					bool flag3 = beh == arg2;
					if (flag3)
					{
						result = true;
					}
					else
					{
						bool flag4 = beh == arg3;
						if (flag4)
						{
							result = true;
						}
						else
						{
							bool flag5 = beh == arg4;
							result = flag5;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06001B66 RID: 7014 RVA: 0x0017C174 File Offset: 0x0017A374
		public static bool CurrentAreaSpiritualDebtMore(MapAreaData arg0, short arg1)
		{
			return OptionConditionMatcher.AreaSpiritualDebtMore(arg0, arg1);
		}

		// Token: 0x06001B67 RID: 7015 RVA: 0x0017C190 File Offset: 0x0017A390
		public static bool ConsummateLevelHigherThan(GameData.Domains.Character.Character arg0)
		{
			return DomainManager.Taiwu.GetTaiwu().GetConsummateLevel() <= arg0.GetConsummateLevel();
		}

		// Token: 0x06001B68 RID: 7016 RVA: 0x0017C1BC File Offset: 0x0017A3BC
		public static bool ExpMore(int arg0)
		{
			return DomainManager.Taiwu.GetTaiwu().GetExp() >= arg0;
		}

		// Token: 0x06001B69 RID: 7017 RVA: 0x0017C1E4 File Offset: 0x0017A3E4
		public static bool TeamMateMore(int arg0)
		{
			return EventHelper.GetValidGroupCharacterCount(true, false, true) >= arg0;
		}

		// Token: 0x06001B6A RID: 7018 RVA: 0x0017C204 File Offset: 0x0017A404
		public static bool DynamicTeammateCountMax(int arg0)
		{
			return EventHelper.GetValidGroupCharacterCount(true, false, true) < arg0;
		}

		// Token: 0x06001B6B RID: 7019 RVA: 0x0017C224 File Offset: 0x0017A424
		public static bool CharacterNotTaiwuVillager(GameData.Domains.Character.Character arg0)
		{
			return arg0.GetOrganizationInfo().OrgTemplateId != 16;
		}

		// Token: 0x06001B6C RID: 7020 RVA: 0x0017C248 File Offset: 0x0017A448
		[Obsolete]
		public static bool CharacterGradeNotSatisfy(GameData.Domains.Character.Character arg0)
		{
			return EventHelper.CheckSeniorityIsSatisfyByGrade(arg0);
		}

		// Token: 0x06001B6D RID: 7021 RVA: 0x0017C260 File Offset: 0x0017A460
		public static bool RelationshipNotEnemy(GameData.Domains.Character.Character arg0)
		{
			return !EventHelper.CheckHasRelationship(arg0, DomainManager.Taiwu.GetTaiwu(), 32768);
		}

		// Token: 0x06001B6E RID: 7022 RVA: 0x0017C28C File Offset: 0x0017A48C
		public static bool ProfessionSkillConditionNotSatisfy(int arg0)
		{
			return EventHelper.ProfessionSkillConditionNotSatisfy(arg0);
		}

		// Token: 0x06001B6F RID: 7023 RVA: 0x0017C2A4 File Offset: 0x0017A4A4
		public static bool ProfessionSeniorityNotSatisfy(int arg0)
		{
			return EventHelper.ProfessionSkillSeniorityNotSatisfy(arg0);
		}

		// Token: 0x06001B70 RID: 7024 RVA: 0x0017C2BC File Offset: 0x0017A4BC
		public static bool CharacterDisorderOfQiNotSatisfy(GameData.Domains.Character.Character arg0, int arg1)
		{
			return (int)arg0.GetDisorderOfQi() <= arg1 * 10;
		}

		// Token: 0x06001B71 RID: 7025 RVA: 0x0017C2E0 File Offset: 0x0017A4E0
		public static bool TaiwuDisorderOfQiNotSatisfy(int arg0)
		{
			return (int)DomainManager.Taiwu.GetTaiwu().GetDisorderOfQi() <= arg0 * 10;
		}

		// Token: 0x06001B72 RID: 7026 RVA: 0x0017C30C File Offset: 0x0017A50C
		public static bool TaiwuHasItemBySubType(int arg0)
		{
			return !EventHelper.SelectCharacterItemByGrade(DomainManager.Taiwu.GetTaiwuCharId(), 0, 8, false, -1, (short)arg0, null).Equals(ItemKey.Invalid);
		}

		// Token: 0x06001B73 RID: 7027 RVA: 0x0017C344 File Offset: 0x0017A544
		public static bool CharacterNotMonk(GameData.Domains.Character.Character arg0)
		{
			return arg0.GetMonkType() != 2 && arg0.GetMonkType() != 130;
		}

		// Token: 0x06001B74 RID: 7028 RVA: 0x0017C374 File Offset: 0x0017A574
		public static bool CharacterBehaviorTypeNeedSatisfy(GameData.Domains.Character.Character arg0, sbyte arg1, sbyte arg2)
		{
			return arg0.GetBehaviorType() == arg1 || arg0.GetBehaviorType() == arg2;
		}

		// Token: 0x06001B75 RID: 7029 RVA: 0x0017C39C File Offset: 0x0017A59C
		public static bool CharacterIsCastellan(GameData.Domains.Character.Character arg0)
		{
			OrganizationInfo organizationInfo = arg0.GetOrganizationInfo();
			sbyte grade = organizationInfo.Grade;
			return grade == 8 && organizationInfo.Principal && organizationInfo.OrgTemplateId >= 21 && organizationInfo.OrgTemplateId <= 35;
		}

		// Token: 0x06001B76 RID: 7030 RVA: 0x0017C3E4 File Offset: 0x0017A5E4
		public static bool CharacterPartlyInfected(GameData.Domains.Character.Character arg0)
		{
			return EventHelper.CheckRoleInfection(arg0, 1);
		}

		// Token: 0x06001B77 RID: 7031 RVA: 0x0017C400 File Offset: 0x0017A600
		public static bool CharactetNotYixiangAffect(GameData.Domains.Character.Character arg0)
		{
			return !EventHelper.CheckCharacterNearYixiang(arg0);
		}

		// Token: 0x06001B78 RID: 7032 RVA: 0x0017C41C File Offset: 0x0017A61C
		public static bool CharacterNotLegendaryInfected(GameData.Domains.Character.Character arg0)
		{
			return !EventHelper.CheckCharacterIsLegendaryInfected(arg0);
		}

		// Token: 0x06001B79 RID: 7033 RVA: 0x0017C438 File Offset: 0x0017A638
		public static bool CharacterNotXiangshuInfected(GameData.Domains.Character.Character arg0)
		{
			return !EventHelper.CheckCharacterIsXiangshuInfected(arg0);
		}

		// Token: 0x06001B7A RID: 7034 RVA: 0x0017C454 File Offset: 0x0017A654
		public static bool DynamicExpMore(int arg0)
		{
			return DomainManager.Taiwu.GetTaiwu().GetExp() >= arg0;
		}

		// Token: 0x06001B7B RID: 7035 RVA: 0x0017C47C File Offset: 0x0017A67C
		private unsafe static bool CharacterNeedSickUnsafe(GameData.Domains.Character.Character arg0)
		{
			Injuries injuries = arg0.GetInjuries();
			bool flag = injuries.HasAnyInjury();
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = injuries.HasAnyInjury(true);
				if (flag2)
				{
					result = true;
				}
				else
				{
					for (int i = 0; i < 6; i++)
					{
						bool flag3 = *(ref arg0.GetPoisoned().Items.FixedElementField + (IntPtr)i * 4) > 0;
						if (flag3)
						{
							return true;
						}
					}
					bool flag4 = arg0.GetDisorderOfQi() > 0;
					if (flag4)
					{
						result = true;
					}
					else
					{
						short health = arg0.GetHealth();
						short leftMaxHealth = arg0.GetLeftMaxHealth(false);
						bool flag5 = leftMaxHealth > health;
						result = flag5;
					}
				}
			}
			return result;
		}

		// Token: 0x06001B7C RID: 7036 RVA: 0x0017C530 File Offset: 0x0017A730
		public static bool CharacterNeedAdult(GameData.Domains.Character.Character arg0)
		{
			return arg0.GetAgeGroup() >= 2;
		}

		// Token: 0x06001B7D RID: 7037 RVA: 0x0017C550 File Offset: 0x0017A750
		public unsafe static bool TaiwuCanEat(int arg0)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			EatingItems eatingItems = *taiwu.GetEatingItems();
			sbyte currMaxEatingSlotsCount = taiwu.GetCurrMaxEatingSlotsCount();
			sbyte index = eatingItems.GetAvailableEatingSlot(currMaxEatingSlotsCount);
			bool flag = index < 0;
			return !flag;
		}

		// Token: 0x06001B7E RID: 7038 RVA: 0x0017C59C File Offset: 0x0017A79C
		public unsafe static bool CharacterCanEat(GameData.Domains.Character.Character arg0)
		{
			EatingItems eatingItems = *arg0.GetEatingItems();
			sbyte currMaxEatingSlotsCount = arg0.GetCurrMaxEatingSlotsCount();
			sbyte index = eatingItems.GetAvailableEatingSlot(currMaxEatingSlotsCount);
			bool flag = index < 0;
			return !flag;
		}

		// Token: 0x06001B7F RID: 7039 RVA: 0x0017C5DC File Offset: 0x0017A7DC
		public static bool TaiwuHasSutraPavilionEntryPremit()
		{
			Settlement settlement = DomainManager.Organization.GetSettlementByOrgTemplateId(1);
			OptionConditionMatcher.<>c__DisplayClass41_0 CS$<>8__locals1;
			CS$<>8__locals1.orgMembers = settlement.GetMembers();
			return OptionConditionMatcher.<TaiwuHasSutraPavilionEntryPremit>g__AnyApprovedTaiwu|41_0(6, ref CS$<>8__locals1) || OptionConditionMatcher.<TaiwuHasSutraPavilionEntryPremit>g__AnyApprovedTaiwu|41_0(7, ref CS$<>8__locals1) || OptionConditionMatcher.<TaiwuHasSutraPavilionEntryPremit>g__AnyApprovedTaiwu|41_0(8, ref CS$<>8__locals1);
		}

		// Token: 0x06001B80 RID: 7040 RVA: 0x0017C628 File Offset: 0x0017A828
		public static bool CharacterNeedSick(GameData.Domains.Character.Character arg0)
		{
			return OptionConditionMatcher.CharacterNeedSickUnsafe(arg0);
		}

		// Token: 0x06001B81 RID: 7041 RVA: 0x0017C640 File Offset: 0x0017A840
		public static bool ResourceMore(sbyte arg0, int arg1)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			return taiwu.GetResource(arg0) >= arg1;
		}

		// Token: 0x06001B82 RID: 7042 RVA: 0x0017C66C File Offset: 0x0017A86C
		public static bool TaiwuMonthCooldownCount(string arg0, int arg1, sbyte arg2)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted(arg0);
			defaultInterpolatedStringHandler.AppendLiteral("_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(arg1);
			string boxKey = defaultInterpolatedStringHandler.ToStringAndClear();
			EventArgBox globalBox = EventHelper.GetGlobalArgBox();
			int logMonth = -1;
			bool flag = globalBox.Get(boxKey, ref logMonth);
			bool result;
			if (flag)
			{
				int curMonth = DomainManager.World.GetCurrDate();
				result = (curMonth - logMonth >= (int)arg2);
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06001B83 RID: 7043 RVA: 0x0017C6E4 File Offset: 0x0017A8E4
		public static bool CharacterInSect(GameData.Domains.Character.Character arg0)
		{
			return DomainManager.Organization.IsInAnySect(arg0.GetId());
		}

		// Token: 0x06001B84 RID: 7044 RVA: 0x0017C708 File Offset: 0x0017A908
		public static bool DlcFiveLoongJiaoIsNotFostered(int arg0)
		{
			return !DomainManager.Extra.IsJiaoFostered(arg0);
		}

		// Token: 0x06001B85 RID: 7045 RVA: 0x0017C728 File Offset: 0x0017A928
		public static bool SwordTombInformationUsedCountLimit(EventArgBox arg0)
		{
			int taiwuId = DomainManager.Taiwu.GetTaiwuCharId();
			NormalInformation normalInformation;
			arg0.Get<NormalInformation>("SwordTombInformation", out normalInformation);
			return DomainManager.Information.GetNormalInformationUsedCount(taiwuId, normalInformation) == 0;
		}

		// Token: 0x06001B86 RID: 7046 RVA: 0x0017C764 File Offset: 0x0017A964
		public static bool SwordTombInformationGiftItem(EventArgBox arg0)
		{
			return EventHelper.GetSwordTombInformationGiftItem(arg0) != ItemKey.Invalid;
		}

		// Token: 0x06001B87 RID: 7047 RVA: 0x0017C788 File Offset: 0x0017A988
		public static bool SwordTombInformationGiftCombatSkill(EventArgBox arg0)
		{
			return EventHelper.GetSwordTombInformationGiftCombatSkill(arg0) > -1;
		}

		// Token: 0x06001B88 RID: 7048 RVA: 0x0017C7A4 File Offset: 0x0017A9A4
		public static bool SwordTombInformationGiftTeammate(EventArgBox arg0)
		{
			return EventHelper.IsSwordTombInformationGiftCharacterAbleToJoin(arg0);
		}

		// Token: 0x06001B89 RID: 7049 RVA: 0x0017C7BC File Offset: 0x0017A9BC
		public static bool TaiwuGroupHaveAdult(int arg0)
		{
			return EventHelper.TaiwuGroupHaveAdult();
		}

		// Token: 0x06001B8A RID: 7050 RVA: 0x0017C7D4 File Offset: 0x0017A9D4
		public static bool TaiwuPrisonerHaveInfectedCharacter(int arg0)
		{
			return EventHelper.TaiwuPrisonerHaveInfectedCharacter();
		}

		// Token: 0x06001B8B RID: 7051 RVA: 0x0017C7EC File Offset: 0x0017A9EC
		public static bool InteractionOffCooldown(int arg0, short arg1)
		{
			return DomainManager.TaiwuEvent.IsInteractionEventOptionOffCooldown(arg0, arg1);
		}

		// Token: 0x06001B8C RID: 7052 RVA: 0x0017C80C File Offset: 0x0017AA0C
		public static bool AliveCricketMore(int arg0)
		{
			return EventHelper.AbleToCricketBattleWithGivenLimit(arg0);
		}

		// Token: 0x06001B8D RID: 7053 RVA: 0x0017C824 File Offset: 0x0017AA24
		public static bool TaiwuCanGetTaught(GameData.Domains.Character.Character arg0)
		{
			return DomainManager.Taiwu.IsTaiwuAbleToGetTaught(arg0);
		}

		// Token: 0x06001B8E RID: 7054 RVA: 0x0017C844 File Offset: 0x0017AA44
		public static bool CharacterCanBeRecommended(GameData.Domains.Character.Character arg0)
		{
			return EventHelper.IsCharacterAbleToBeRecommended(arg0.GetId());
		}

		// Token: 0x06001B8F RID: 7055 RVA: 0x0017C864 File Offset: 0x0017AA64
		public static bool TaiwuHasNormalCricket(GameData.Domains.Character.Character arg0)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			List<ItemDisplayData> inventoryItems = DomainManager.Character.GetAllInventoryItems(taiwu.GetId());
			foreach (ItemDisplayData item in inventoryItems)
			{
				ItemKey itemKey = item.Key;
				bool flag = itemKey.ItemType == 11 && EventHelper.CanIdentifyCricket(itemKey.Id);
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001B90 RID: 7056 RVA: 0x0017C900 File Offset: 0x0017AB00
		public static bool TaiwuHasUnNormalCricket(GameData.Domains.Character.Character arg0)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			List<ItemDisplayData> inventoryItems = DomainManager.Character.GetAllInventoryItems(taiwu.GetId());
			foreach (ItemDisplayData item in inventoryItems)
			{
				ItemKey itemKey = item.Key;
				bool flag = itemKey.ItemType == 11 && EventHelper.GetItemCurrDurability(itemKey) > 0 && !EventHelper.IsCricketGreat(itemKey) && EventHelper.CanUpgradeCricket(itemKey.Id);
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001B91 RID: 7057 RVA: 0x0017C9B0 File Offset: 0x0017ABB0
		public static bool CharacterGradeNotSatisfyByProfession(GameData.Domains.Character.Character arg0, int arg1)
		{
			return EventHelper.CheckSeniorityIsSatisfyByGradeByProfession(arg0, arg1);
		}

		// Token: 0x06001B92 RID: 7058 RVA: 0x0017C9CC File Offset: 0x0017ABCC
		public static bool TaiwuHealthMore(int arg0)
		{
			return DomainManager.Taiwu.GetTaiwu().GetBaseMaxHealth() >= 1;
		}

		// Token: 0x06001B93 RID: 7059 RVA: 0x0017C9F4 File Offset: 0x0017ABF4
		public static bool CharacterHasNormalHealth(GameData.Domains.Character.Character arg0)
		{
			foreach (short featureId in arg0.GetFeatureIds())
			{
				bool ignoreHealthMark = CharacterFeature.Instance[featureId].IgnoreHealthMark;
				if (ignoreHealthMark)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001B94 RID: 7060 RVA: 0x0017CA60 File Offset: 0x0017AC60
		public static bool CharacterOnOrganizationBlock(GameData.Domains.Character.Character arg0)
		{
			return EventHelper.IsCharacterAtTargetSectLocation(arg0, arg0.GetOrganizationInfo().SettlementId);
		}

		// Token: 0x06001B95 RID: 7061 RVA: 0x0017CA84 File Offset: 0x0017AC84
		public static bool TaiwuAndCharacterNotHusbandOrWife(GameData.Domains.Character.Character arg0)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			return !EventHelper.CheckHasRelationship(taiwu, arg0, 1024);
		}

		// Token: 0x06001B96 RID: 7062 RVA: 0x0017CAB0 File Offset: 0x0017ACB0
		public static bool TaiwuAndCharacterNotSwornBrotherOrSister(GameData.Domains.Character.Character arg0)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			return !EventHelper.CheckHasRelationship(taiwu, arg0, 512);
		}

		// Token: 0x06001B97 RID: 7063 RVA: 0x0017CADC File Offset: 0x0017ACDC
		public static bool CharacterOnOrganizationRange(GameData.Domains.Character.Character arg0)
		{
			return EventHelper.IsCharacterAtOrganizationRange(arg0, arg0.GetOrganizationInfo().SettlementId);
		}

		// Token: 0x06001B98 RID: 7064 RVA: 0x0017CB00 File Offset: 0x0017AD00
		public static bool CharacterInjuryPoisonDisorderOfQiLess(GameData.Domains.Character.Character arg0, int arg1)
		{
			return EventHelper.CharacterInjuryPoisonDisorderOfQiLess(arg0, arg1);
		}

		// Token: 0x06001B99 RID: 7065 RVA: 0x0017CB1C File Offset: 0x0017AD1C
		public static bool CharacterShouldNotDirectFallenInCombat(GameData.Domains.Character.Character arg0, sbyte arg1)
		{
			return !EventHelper.IsCharacterDirectFallenInCombat(arg0.GetId(), (CombatType)arg1);
		}

		// Token: 0x06001B9A RID: 7066 RVA: 0x0017CB40 File Offset: 0x0017AD40
		public static bool StoneRoomCapacityMore(int arg0)
		{
			Location location = DomainManager.Taiwu.GetTaiwuVillageLocation();
			BuildingBlockData blockData = DomainManager.Building.GetBuildingBlockData(44);
			sbyte villageLevel = DomainManager.Building.BuildingBlockLevel(new BuildingBlockKey(location.AreaId, location.BlockId, blockData.BlockIndex));
			int maxCapacity = BuildingScale.DefValue.StoneRoomCapacity.GetLevelEffect((int)villageLevel);
			return DomainManager.Extra.GetStoneRoomCharList().Count + arg0 <= maxCapacity;
		}

		// Token: 0x06001B9B RID: 7067 RVA: 0x0017CBB1 File Offset: 0x0017ADB1
		[CompilerGenerated]
		internal static bool <TaiwuHasSutraPavilionEntryPremit>g__AnyApprovedTaiwu|41_0(sbyte grade, ref OptionConditionMatcher.<>c__DisplayClass41_0 A_1)
		{
			return A_1.orgMembers.GetMembers(grade).Any(new Func<int, bool>(EventHelper.IsSectCharApprovedTaiwu));
		}
	}
}
