using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.Common;
using GameData.DLC;
using GameData.Domains;
using GameData.Domains.Adventure;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.Taiwu.VillagerRole;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Domains.TaiwuEvent.MonthlyEventActions;
using GameData.Domains.World;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.DomainEvents
{
	// Token: 0x020008D6 RID: 2262
	public static class Events
	{
		// Token: 0x06007FDF RID: 32735 RVA: 0x004CC844 File Offset: 0x004CAA44
		public static void RaiseBeforeSendRequestToArchiveModule(DataContext context)
		{
			DomainManager.LifeRecord.CommitCurrLifeRecords();
			DomainManager.World.CommitInstantNotifications(context);
			DomainManager.Extra.CommitTravelingEvents(context);
			DomainManager.Extra.CommitTaiwuVillageStorages(context);
			DomainManager.Taiwu.CommitTaiwuSettlementTreasury(context);
		}

		// Token: 0x06007FE0 RID: 32736 RVA: 0x004CC882 File Offset: 0x004CAA82
		public static void RaiseBeforeSaveWorld(DataContext context)
		{
			DomainManager.LifeRecord.CommitCurrLifeRecords();
			DomainManager.World.CommitInstantNotifications(context);
			DomainManager.Extra.CommitTravelingEvents(context);
			DomainManager.Extra.CommitTaiwuVillageStorages(context);
			DomainManager.Taiwu.CommitTaiwuSettlementTreasury(context);
		}

		// Token: 0x06007FE1 RID: 32737 RVA: 0x004CC8C0 File Offset: 0x004CAAC0
		public static void RaiseTaiwuItemModified(DataContext context, ItemKey itemKey)
		{
			bool flag = ItemType.IsEquipmentItemType(itemKey.ItemType);
			if (flag)
			{
				EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(itemKey);
				baseEquipment.SetModificationState(baseEquipment.GetModificationState(), context);
				bool flag2 = itemKey.ItemType == 3;
				if (flag2)
				{
					DomainManager.Extra.RecordOwnedClothing(context, itemKey.TemplateId);
				}
			}
		}

		// Token: 0x06007FE2 RID: 32738 RVA: 0x004CC918 File Offset: 0x004CAB18
		public static void RaiseBossInvasionSpeedTypeChanged(DataContext context, byte prevInvasionSpeedType)
		{
			Events.<>c__DisplayClass101_0 CS$<>8__locals1;
			CS$<>8__locals1.context = context;
			byte curInvasionSpeedType = DomainManager.World.GetBossInvasionSpeedType();
			bool flag = GlobalConfig.Instance.SwordTombAdventureLastMonthCount[(int)curInvasionSpeedType] < 0 && GlobalConfig.Instance.SwordTombAdventureLastMonthCount[(int)prevInvasionSpeedType] > 0;
			if (flag)
			{
				Events.<RaiseBossInvasionSpeedTypeChanged>g__RemoveAllAttackingXiangshu|101_0(ref CS$<>8__locals1);
				DomainManager.Adventure.StopAllSwordTombAdventureCountDown(CS$<>8__locals1.context);
			}
			else
			{
				bool flag2 = GlobalConfig.Instance.SwordTombAdventureLastMonthCount[(int)curInvasionSpeedType] > 0 && GlobalConfig.Instance.SwordTombAdventureLastMonthCount[(int)prevInvasionSpeedType] < 0;
				if (flag2)
				{
					DomainManager.TaiwuEvent.ActivateNextSwordTomb();
				}
				else
				{
					bool isTaiwuDieOfCombatWithXiangshu = DomainManager.Taiwu.GetIsTaiwuDieOfCombatWithXiangshu();
					if (isTaiwuDieOfCombatWithXiangshu)
					{
						Events.<RaiseBossInvasionSpeedTypeChanged>g__RemoveAllAttackingXiangshu|101_0(ref CS$<>8__locals1);
					}
				}
			}
		}

		// Token: 0x06007FE3 RID: 32739 RVA: 0x004CC9CC File Offset: 0x004CABCC
		public static void RaiseCharacterFeatureAdded(DataContext context, GameData.Domains.Character.Character character, short featureId)
		{
			int charId = character.GetId();
			DomainManager.SpecialEffect.AddFeatureEffect(context, charId, featureId);
			DomainManager.Extra.RegisterCharacterTemporaryFeature(context, charId, featureId, -1);
			bool flag = charId == DomainManager.Taiwu.GetTaiwuCharId();
			if (flag)
			{
				DomainManager.Taiwu.TaiwuAddFeatureTryInterruptHuntTaiwuAction(context, featureId);
			}
		}

		// Token: 0x06007FE4 RID: 32740 RVA: 0x004CCA1C File Offset: 0x004CAC1C
		public static void RaiseCharacterFeatureRemoved(DataContext context, GameData.Domains.Character.Character character, short featureId)
		{
			int charId = character.GetId();
			DomainManager.SpecialEffect.RemoveFeatureEffect(context, charId, featureId);
			bool flag = charId == DomainManager.Taiwu.GetTaiwuCharId();
			if (flag)
			{
				DomainManager.Taiwu.TaiwuRemoveFeatureTryInterruptHuntTaiwuAction(context, featureId, character.GetFeatureIds());
			}
		}

		// Token: 0x06007FE5 RID: 32741 RVA: 0x004CCA64 File Offset: 0x004CAC64
		public static void RaiseItemRemovedFromInventory(DataContext context, GameData.Domains.Character.Character character, ItemKey itemKey, int amount)
		{
			int charId = character.GetId();
			DomainManager.Item.RemoveOwner(itemKey, ItemOwnerType.CharacterInventory, charId);
			short itemSubType = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId);
			bool flag = itemSubType == 1202;
			if (flag)
			{
				int bookCombatSkillType = (int)(itemKey.TemplateId - 211);
				DomainManager.LegendaryBook.UnregisterOwner(context, character, (sbyte)bookCombatSkillType);
			}
			bool flag2 = charId == DomainManager.Taiwu.GetTaiwuCharId();
			if (flag2)
			{
				bool flag3 = itemKey.ItemType == 0;
				if (flag3)
				{
					DomainManager.Extra.ClearLegendaryBookWeaponSlot(context, itemKey);
				}
				bool flag4 = ItemType.IsEquipmentItemType(itemKey.ItemType);
				if (flag4)
				{
					EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(itemKey);
					baseEquipment.SetModificationState(baseEquipment.GetModificationState(), context);
				}
			}
			IReadOnlyDictionary<ItemKey, int> giftItems = DomainManager.Extra.GetTaiwuGiftItems(charId);
			int giftAmount;
			bool flag5 = giftItems.Count > 0 && giftItems.TryGetValue(itemKey, out giftAmount);
			if (flag5)
			{
				int remainingAmount = character.GetInventory().Items.GetValueOrDefault(itemKey, 0);
				bool flag6 = remainingAmount < giftAmount;
				if (flag6)
				{
					DomainManager.Extra.SetTaiwuGiftItemAmount(context, charId, itemKey, amount);
				}
			}
		}

		// Token: 0x06007FE6 RID: 32742 RVA: 0x004CCB80 File Offset: 0x004CAD80
		public static void RaiseCharacterCreated(DataContext context, GameData.Domains.Character.Character character)
		{
			context.Equipping.SetInitialCombatSkillBreakouts(context, character);
			context.Equipping.SetInitialCombatSkillAttainmentPanels(context, character);
			DomainManager.Character.OnCharacterCreated(context, character);
			short settlementId = character.GetOrganizationInfo().SettlementId;
			bool flag = settlementId >= 0;
			if (flag)
			{
				DomainManager.Organization.GetSettlement(settlementId).AddSettlementFeatures(context, character);
			}
			character.SpecifyCurrNeili(context, int.MaxValue);
			bool flag2 = character != DomainManager.Taiwu.GetTaiwu();
			if (flag2)
			{
				context.Equipping.SelectEquipments(context, character, true, true);
			}
			character.SetCurrMainAttributes(character.GetMaxMainAttributes(), context);
			bool flag3 = character.GetActualAge() >= 0;
			if (flag3)
			{
				character.AdjustLifespan(context);
			}
			AvatarData avatar = character.GetAvatar();
			avatar.InitializeGrowableElementsShowingAbilitiesAndStates(character);
			character.SetAvatar(avatar, context);
			DomainManager.SpecialEffect.OnCharacterCreated(context, character);
		}

		// Token: 0x06007FE7 RID: 32743 RVA: 0x004CCC60 File Offset: 0x004CAE60
		public static void RaiseCharacterReincarnated(DataContext context, GameData.Domains.Character.Character character, int reincarnatedCharId)
		{
			int charId = character.GetId();
			InteractOfLove.OnLoverReincarnate(context, character, charId);
			short settlementId = character.GetOrganizationInfo().SettlementId;
			DomainManager.Character.TryEnterTaiwuDreamWhenReincarnated(reincarnatedCharId, settlementId);
			bool flag = ProfessionSkillHandle.BuddhistMonkSkill_IsDirectedSamsaraCharacter(reincarnatedCharId);
			if (flag)
			{
				int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
				Location location = character.GetLocation();
				MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
				monthlyEventCollection.AddTaiwuComingSuccess(taiwuCharId, reincarnatedCharId, location, charId);
				DeadCharacter reincarnatedDeadChar = DomainManager.Character.GetDeadCharacter(reincarnatedCharId);
				AvatarData avatar = new AvatarData(reincarnatedDeadChar.Avatar);
				avatar.InitializeGrowableElementsShowingAbilitiesAndStates(character);
				character.SetAvatar(avatar, context);
				DomainManager.Character.TryCreateRelation(context, charId, taiwuCharId);
				DomainManager.Character.DirectlySetFavorabilities(context, charId, taiwuCharId, 30000, 30000);
				short featureId;
				bool flag2 = ProfessionSkillHandle.BuddhistMonkSkill_TryGetSamsaraFeature(reincarnatedCharId, out featureId);
				if (flag2)
				{
					character.AddFeature(context, featureId, false);
					ProfessionSkillHandle.BuddhistMonkSkill_RemoveSamsaraFeature(context, reincarnatedCharId);
				}
				int motherId = ProfessionSkillHandle.BuddhistMonkSkill_GetDirectedSamsaraMother(reincarnatedCharId);
				ProfessionSkillHandle.BuddhistMonkSkill_TryRemoveDirectedSamsara(context, motherId, false);
			}
		}

		// Token: 0x06007FE8 RID: 32744 RVA: 0x004CCD5C File Offset: 0x004CAF5C
		public static void RaiseRelationAdded(DataContext context, int charId, int relatedCharId, ushort relationType)
		{
			Events.<>c__DisplayClass107_0 CS$<>8__locals1;
			CS$<>8__locals1.context = context;
			CS$<>8__locals1.charId = charId;
			GameData.Domains.Character.Character character;
			bool flag = relatedCharId == DomainManager.Taiwu.GetTaiwuCharId() && DomainManager.Character.TryGetElement_Objects(CS$<>8__locals1.charId, out character);
			if (flag)
			{
				if (relationType != 8192)
				{
					if (relationType == 32768)
					{
						OrganizationInfo orgInfo = character.GetOrganizationInfo();
						Events.<RaiseRelationAdded>g__TryAddBeggar5Seniority|107_3(orgInfo, ref CS$<>8__locals1);
					}
				}
				else
				{
					OrganizationInfo orgInfo2 = character.GetOrganizationInfo();
					Events.<RaiseRelationAdded>g__TryAddMartialArtist7Seniority|107_0(orgInfo2, ref CS$<>8__locals1);
					Events.<RaiseRelationAdded>g__TryAddBeggar4Seniority|107_2(orgInfo2, ref CS$<>8__locals1);
					Events.<RaiseRelationAdded>g__TryAddCivilian6Seniority|107_1(orgInfo2, ref CS$<>8__locals1);
				}
			}
		}

		// Token: 0x06007FE9 RID: 32745 RVA: 0x004CCE00 File Offset: 0x004CB000
		public static void RaiseCharacterAgeChanged(DataContext context, GameData.Domains.Character.Character character, int fromCurrAge, int toCurrAge)
		{
			bool flag = character.GetCreatingType() != 1;
			if (!flag)
			{
				int charId = character.GetId();
				sbyte ageGroup = character.GetAgeGroup();
				bool flag2 = ageGroup != 0;
				if (flag2)
				{
					DomainManager.Character.RemoveInfant(charId);
				}
				else
				{
					bool flag3 = !DomainManager.Character.IsInfantInMap(charId);
					if (flag3)
					{
						DomainManager.Character.AddInfant(charId);
					}
				}
				bool flag4 = fromCurrAge < 16 && toCurrAge >= 16;
				if (flag4)
				{
					DomainManager.Character.GenerateCharacterProfession(context, character);
				}
				OrganizationInfo orgInfo = character.GetOrganizationInfo();
				DomainManager.Organization.TryAddSectMemberFeature(context, character, orgInfo);
			}
		}

		// Token: 0x06007FEA RID: 32746 RVA: 0x004CCEA0 File Offset: 0x004CB0A0
		public static void RaiseIntelligentCharacterDead(DataContext context, GameData.Domains.Character.Character character, CharacterDeathTypeItem deathType, ref CharacterDeathInfo deathInfo)
		{
			int charId = character.GetId();
			Location charLocation = character.GetLocation();
			DomainManager.SpecialEffect.OnCharacterRemoved(context, character);
			DomainManager.Organization.OnCharacterDead(context, character);
			DomainManager.LegendaryBook.OnCharacterDead(context, character);
			DomainManager.Map.OnCharacterLocationChanged(context, charId, charLocation, Location.Invalid);
			DomainManager.Map.OnInfectedCharacterLocationChanged(context, charId, charLocation, Location.Invalid);
			DomainManager.Taiwu.ClearTeachTaiwuLifeSkillList(context, charId);
			DomainManager.Taiwu.ClearTeachTaiwuCombatSkillList(context, charId);
			DomainManager.CombatSkill.RemoveAllCombatSkills(charId);
			DomainManager.Extra.RemoveCharacterEquippedCombatSkills(context, charId);
			DomainManager.Extra.RemoveCharacterCombatSkillConfiguration(context, charId);
			DomainManager.Extra.ClearCharacterMasteredCombatSkills(context, charId);
			DomainManager.Extra.ClearCharTeammateCommands(context, charId);
			DomainManager.Extra.RemovePoisonImmunities(context, charId);
			DomainManager.Extra.RemoveCharacterProfessions(context, charId);
			IntPair samsaraInfo = DomainManager.Character.GetDirectedSamsaraInfo(charId);
			bool flag = samsaraInfo.First >= 0;
			if (flag)
			{
				MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
				monthlyNotificationCollection.AddMiscarriageAndReincarnationMotherKilled(charId, charLocation, samsaraInfo.First);
				DomainManager.Building.TryRemoveSamsaraPlatformBornData(context, charId);
				ProfessionSkillHandle.BuddhistMonkSkill_TryRemoveDirectedSamsara(context, charId, true);
			}
			DomainManager.World.TryRemoveLifeLinkCharacter(context, character);
			DomainManager.Information.RemoveCharacterAllInformation(context, charId);
			LifeRecordCollection collection = DomainManager.LifeRecord.GetLifeRecordCollection();
			collection.AddDeathRecord(character, deathType, ref deathInfo);
			DomainManager.LifeRecord.GenerateDead(charId);
			DomainManager.TaiwuEvent.OnCharacterDie(charId);
			DomainManager.Extra.TryRemoveStoneRoomCharacter(context, character, false);
			ProfessionSkillHandle.DukeSkill_RemoveCharacterTitle(context, charId);
			DomainManager.Extra.RemoveCharacterRevealedHobbies(context, charId);
			DomainManager.Extra.RemoveTaiwuGiftItemsForCharacter(context, charId);
			DomainManager.Extra.RemoveVillageSkillLegacy(context, charId);
		}

		// Token: 0x06007FEB RID: 32747 RVA: 0x004CD054 File Offset: 0x004CB254
		public static void RaiseNonIntelligentCharacterRemoved(DataContext context, GameData.Domains.Character.Character character)
		{
			int charId = character.GetId();
			DomainManager.SpecialEffect.OnCharacterRemoved(context, character);
			byte creatingType = character.GetCreatingType();
			Location srcLocation = character.GetLocation();
			bool flag = srcLocation.IsValid();
			if (flag)
			{
				byte b = creatingType;
				byte b2 = b;
				if (b2 != 0)
				{
					if (b2 - 2 <= 1)
					{
						Events.RaiseEnemyCharacterLocationChanged(context, charId, srcLocation, Location.Invalid);
					}
				}
				else
				{
					Events.RaiseFixedCharacterLocationChanged(context, charId, srcLocation, Location.Invalid);
				}
			}
			DomainManager.CombatSkill.RemoveAllCombatSkills(charId);
			DomainManager.Extra.ClearCharacterMasteredCombatSkills(context, charId);
			DomainManager.Extra.RemovePoisonImmunities(context, charId);
			DomainManager.Extra.RemoveCharacterCustomDisplayName(context, charId);
		}

		// Token: 0x06007FEC RID: 32748 RVA: 0x004CD0FC File Offset: 0x004CB2FC
		public static void RaiseTemporaryIntelligentCharacterRemoved(DataContext context, GameData.Domains.Character.Character character)
		{
			int charId = character.GetId();
			DomainManager.SpecialEffect.OnCharacterRemoved(context, character);
			Location location = character.GetLocation();
			Tester.Assert(!location.IsValid() || !DomainManager.Map.ContainsCharacter(location, charId), "");
			SettlementCharacter settlementChar;
			Tester.Assert(!DomainManager.Organization.TryGetSettlementCharacter(charId, out settlementChar), "");
			Tester.Assert(!DomainManager.Taiwu.VillagerHasWork(charId), "");
			Tester.Assert(!DomainManager.Taiwu.IsInGroup(charId), "");
			DomainManager.CombatSkill.RemoveAllCombatSkills(charId);
			Tester.Assert(DomainManager.LegendaryBook.GetCharOwnedBookTypes(charId) == null, "");
			DomainManager.Extra.ClearCharacterMasteredCombatSkills(context, charId);
			DomainManager.Extra.RemovePoisonImmunities(context, charId);
			DomainManager.LifeRecord.Remove(charId);
			DomainManager.TaiwuEvent.OnTemporaryIntelligentCharacterRemoved(charId);
		}

		// Token: 0x06007FED RID: 32749 RVA: 0x004CD1F0 File Offset: 0x004CB3F0
		public static void RaiseXiangshuInfectionFeatureChanged(DataContext context, GameData.Domains.Character.Character character, short featureId)
		{
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			int charId = character.GetId();
			bool flag = featureId == 218;
			if (flag)
			{
				int leaderId = character.GetLeaderId();
				int kidnapperId = character.GetKidnapperId();
				bool flag2 = !character.GetLocation().IsValid();
				if (flag2)
				{
					CrossAreaMoveInfo crossAreaMoveInfo;
					bool flag3 = (leaderId < 0 || charId == leaderId) && DomainManager.Character.TryGetElement_CrossAreaMoveInfos(charId, out crossAreaMoveInfo);
					if (flag3)
					{
						Location validLocation = DomainManager.Map.CrossAreaTravelInfoToLocation(crossAreaMoveInfo);
						DomainManager.Character.RemoveCrossAreaTravelInfo(context, charId);
						DomainManager.Character.GroupMove(context, character, validLocation);
					}
					else
					{
						bool flag4 = kidnapperId >= 0;
						if (flag4)
						{
							DomainManager.Character.RemoveKidnappedCharacter(context, charId, kidnapperId, true);
						}
						else
						{
							bool flag5 = character.IsActiveExternalRelationState(32);
							if (flag5)
							{
								sbyte orgTemplateId = DomainManager.Organization.GetPrisonerSect(charId);
								Sect sect = (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(orgTemplateId);
								sect.AddPrisoner(context, character, 39);
							}
							else
							{
								Location validLocation2 = character.GetValidLocation();
								character.SetLocation(validLocation2, context);
							}
						}
					}
				}
				DomainManager.Character.LeaveGroup(context, character, false);
				OrganizationInfo srcOrgInfo = character.GetOrganizationInfo();
				OrganizationInfo destOrgInfo = new OrganizationInfo(20, srcOrgInfo.Grade, true, -1);
				DomainManager.Organization.ChangeOrganization(context, character, destOrgInfo);
				DomainManager.Character.AddInfectedCharToSet(charId);
				DomainManager.LegendaryBook.LoseAllLegendaryBooks(context, character, true);
				DomainManager.World.TryRemoveLifeLinkCharacter(context, character);
				Location location = character.GetLocation();
				int date = DomainManager.World.GetCurrDate();
				bool flag6 = GameData.Domains.World.SharedMethods.SmallVillageXiangshuProgress();
				if (flag6)
				{
					DomainManager.LifeRecord.GetLifeRecordCollection().AddSmallVillagerXiangshuCompletelyInfected(charId, date, location);
				}
				else
				{
					DomainManager.LifeRecord.GetLifeRecordCollection().AddXiangshuCompletelyInfected(charId, date, location);
				}
				bool flag7 = srcOrgInfo.OrgTemplateId == 16 || DomainManager.Character.IsTaiwuPeople(charId);
				if (flag7)
				{
					DomainManager.World.GetMonthlyNotificationCollection().AddInfectXiangshuCompletely(charId, location);
				}
				Events.RaiseCharacterLocationChanged(context, charId, location, Location.Invalid);
				bool flag8 = !character.IsActiveExternalRelationState(60);
				if (flag8)
				{
					Events.RaiseInfectedCharacterLocationChanged(context, charId, Location.Invalid, location);
				}
			}
			else
			{
				bool flag9 = character.GetOrganizationInfo().OrgTemplateId == 20;
				if (flag9)
				{
					Location location2 = character.GetLocation();
					bool flag10 = !location2.IsValid();
					if (flag10)
					{
						location2 = character.GetValidLocation();
					}
					int date2 = DomainManager.World.GetCurrDate();
					LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
					bool flag11 = character.IsActiveExternalRelationState(8);
					if (flag11)
					{
						lifeRecordCollection.AddSavedFromInfection(charId, date2, taiwuCharId, DomainManager.Taiwu.GetTaiwuVillageLocation());
						DomainManager.Extra.TryRemoveStoneRoomCharacter(context, character, true);
					}
					else
					{
						bool flag12 = GameData.Domains.World.SharedMethods.SmallVillageXiangshuProgress();
						if (flag12)
						{
							lifeRecordCollection.AddSmallVillagerSavedFromInfection(charId, date2, taiwuCharId, location2);
						}
						else
						{
							lifeRecordCollection.AddSavedFromInfection(charId, date2, taiwuCharId, location2);
						}
					}
					DomainManager.Organization.JoinNearbyVillageTownAsBeggar(context, character, -1);
					DomainManager.Character.RemoveInfectedCharFromSet(charId);
					Events.RaiseInfectedCharacterLocationChanged(context, charId, location2, Location.Invalid);
					Events.RaiseCharacterLocationChanged(context, charId, Location.Invalid, location2);
				}
				else
				{
					bool flag13 = featureId == 217;
					if (flag13)
					{
						Location location3 = character.GetLocation();
						int date3 = DomainManager.World.GetCurrDate();
						DomainManager.LifeRecord.GetLifeRecordCollection().AddXiangshuPartiallyInfected(charId, date3, location3);
						bool flag14 = DomainManager.Character.IsTaiwuPeople(charId);
						if (flag14)
						{
							DomainManager.World.GetMonthlyNotificationCollection().AddInfectXiangshuPartially(charId, location3);
						}
					}
				}
			}
			Events.RaiseXiangshuInfectionFeatureChangedEnd(context, character, featureId);
		}

		// Token: 0x06007FEE RID: 32750 RVA: 0x004CD578 File Offset: 0x004CB778
		public static void RaiseCharacterOrganizationChanged(DataContext context, GameData.Domains.Character.Character character, OrganizationInfo srcOrgInfo, OrganizationInfo dstOrgInfo)
		{
			bool flag = srcOrgInfo.OrgTemplateId == dstOrgInfo.OrgTemplateId;
			if (!flag)
			{
				bool flag2 = srcOrgInfo.SettlementId >= 0;
				if (flag2)
				{
					DomainManager.Organization.GetSettlement(srcOrgInfo.SettlementId).RemoveSettlementFeatures(context, character);
				}
				bool flag3 = dstOrgInfo.SettlementId >= 0;
				if (flag3)
				{
					DomainManager.Organization.GetSettlement(dstOrgInfo.SettlementId).AddSettlementFeatures(context, character);
				}
				List<short> featureIds = character.GetFeatureIds();
				for (int i = featureIds.Count - 1; i >= 0; i--)
				{
					short featureId = featureIds[i];
					CharacterFeatureItem featureCfg = CharacterFeature.Instance[featureId];
					bool flag4 = featureCfg.IsAllowedForOrganization(dstOrgInfo.OrgTemplateId);
					if (!flag4)
					{
						character.RemoveFeature(context, featureId);
						string tag = "RaiseCharacterOrganizationChanged";
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Removing feature ");
						defaultInterpolatedStringHandler.AppendFormatted(featureCfg.Name);
						defaultInterpolatedStringHandler.AppendLiteral(" for ");
						defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(character);
						defaultInterpolatedStringHandler.AppendLiteral(" on organization changed.");
						AdaptableLog.TagInfo(tag, defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
				OrganizationItem config = Organization.Instance[srcOrgInfo.OrgTemplateId];
				bool flag5 = config.IsSect && DomainManager.Extra.TaiwuWantedInteracted(character.GetId());
				if (flag5)
				{
					DomainManager.Extra.TaiwuWantedRemoveInteracted(context, character.GetId());
				}
				bool isExpel = srcOrgInfo.OrgTemplateId == 16 && dstOrgInfo.OrgTemplateId != 20;
				bool isOutOfXiangShuEffected = srcOrgInfo.OrgTemplateId == 20 && dstOrgInfo.OrgTemplateId != 16;
				bool flag6 = isExpel || isOutOfXiangShuEffected;
				if (flag6)
				{
					character.ReturnVillagerRoleClothing(context, true);
				}
				bool flag7 = dstOrgInfo.OrgTemplateId == 16;
				if (flag7)
				{
					InstantNotificationCollection instantNotifications = DomainManager.World.GetInstantNotificationCollection();
					instantNotifications.AddJoinTaiwuVillage(character.GetId());
					DomainManager.Extra.CalcSingleVillagerSkillLegacy(context, character.GetId());
					DomainManager.Extra.RegisterVillagerSkillLegacyPoint(character.GetId());
				}
				else
				{
					bool flag8 = srcOrgInfo.OrgTemplateId == 16;
					if (flag8)
					{
						DomainManager.Extra.UnRegisterVillagerSkillLegacyPoint(character.GetId());
					}
				}
				DomainManager.Building.TryRemoveFeastCustomer(context, character.GetId());
			}
		}

		// Token: 0x06007FEF RID: 32751 RVA: 0x004CD7CC File Offset: 0x004CB9CC
		public static void RaiseLegendaryBookOwnerStateChanged(DataContext context, GameData.Domains.Character.Character character, sbyte ownerState)
		{
			int charId = character.GetId();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = character.GetLocation();
			List<sbyte> ownedBookTypes = DomainManager.LegendaryBook.GetCharOwnedBookTypes(charId);
			character.TryRetireTreasuryGuard(context);
			switch (ownerState)
			{
			case 1:
			{
				string tag = "LegendaryBook";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 1);
				defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(character);
				defaultInterpolatedStringHandler.AppendLiteral(" => 因奇书执迷入邪");
				AdaptableLog.TagInfo(tag, defaultInterpolatedStringHandler.ToStringAndClear());
				bool flag = location.IsValid() && location.AreaId < 45;
				if (flag)
				{
					DomainManager.LegendaryBook.UpgradeEnemyNestsByLegendaryBookOwner(context, location.AreaId, 4);
				}
				foreach (sbyte bookType in ownedBookTypes)
				{
					ItemKey bookItemKey = DomainManager.LegendaryBook.GetLegendaryBookItem(bookType);
					lifeRecordCollection.AddLegendaryBookShocked(charId, currDate, location, bookItemKey.ItemType, bookItemKey.TemplateId);
					monthlyNotifications.AddLegendaryBookShocked(charId, location, bookItemKey.ItemType, bookItemKey.TemplateId);
				}
				break;
			}
			case 2:
			{
				string tag2 = "LegendaryBook";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 1);
				defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(character);
				defaultInterpolatedStringHandler.AppendLiteral(" => 因奇书执迷化魔");
				AdaptableLog.TagInfo(tag2, defaultInterpolatedStringHandler.ToStringAndClear());
				OrganizationInfo orgInfo = character.GetOrganizationInfo();
				OrganizationMemberItem orgMemberCfg = OrganizationDomain.GetOrgMemberConfig(orgInfo);
				bool flag2 = orgInfo.Principal && orgMemberCfg.RestrictPrincipalAmount;
				if (flag2)
				{
					for (sbyte grade = orgInfo.Grade - 1; grade >= 0; grade -= 1)
					{
						bool restrictPrincipalAmount = OrganizationDomain.GetOrgMemberConfig(orgInfo.OrgTemplateId, grade).RestrictPrincipalAmount;
						if (!restrictPrincipalAmount)
						{
							ItemKey bookItemKey2 = DomainManager.LegendaryBook.GetLegendaryBookItem(ownedBookTypes[0]);
							lifeRecordCollection.AddResignPositionToStudyLegendaryBook(charId, currDate, location, bookItemKey2.ItemType, bookItemKey2.TemplateId, orgInfo.SettlementId, orgInfo.OrgTemplateId, orgInfo.Grade, true, character.GetGender());
							DomainManager.Organization.ChangeGrade(context, character, grade, true);
							int spouseId = DomainManager.Character.GetAliveSpouse(charId);
							bool flag3 = spouseId >= 0;
							if (flag3)
							{
								GameData.Domains.Character.Character spouse = DomainManager.Character.GetElement_Objects(spouseId);
								DomainManager.Organization.UpdateGradeAccordingToSpouse(context, spouse, character);
							}
							break;
						}
					}
				}
				DomainManager.World.TryRemoveLifeLinkCharacter(context, character);
				bool flag4 = location.IsValid() && location.AreaId < 45;
				if (flag4)
				{
					DomainManager.LegendaryBook.UpgradeEnemyNestsByLegendaryBookOwner(context, location.AreaId, 7);
				}
				foreach (sbyte bookType2 in ownedBookTypes)
				{
					short featureId = Config.CombatSkillType.Instance[bookType2].LegendaryBookFeature;
					DomainManager.Character.RegisterFeatureForAllXiangshuAvatars(context, featureId);
					ItemKey bookItemKey3 = DomainManager.LegendaryBook.GetLegendaryBookItem(bookType2);
					lifeRecordCollection.AddLegendaryBookInsane(charId, currDate, location, bookItemKey3.ItemType, bookItemKey3.TemplateId);
					monthlyNotifications.AddLegendaryBookInsane(charId, location, bookItemKey3.ItemType, bookItemKey3.TemplateId);
					monthlyEventCollection.AddSwordTombGetStronger((ulong)bookItemKey3);
				}
				break;
			}
			case 3:
			{
				DomainManager.World.TryRemoveLifeLinkCharacter(context, character);
				lifeRecordCollection.AddLegendaryBookConsumed(charId, currDate, location);
				foreach (sbyte bookType3 in ownedBookTypes)
				{
					ItemKey bookItemKey4 = DomainManager.LegendaryBook.GetLegendaryBookItem(bookType3);
					monthlyNotifications.AddLegendaryBookConsumed(charId, location, bookItemKey4.ItemType, bookItemKey4.TemplateId);
				}
				monthlyNotifications.AddXiangshuGetStrengthened();
				DomainManager.Organization.ChangeOrganization(context, character, OrganizationInfo.None);
				bool flag5 = location.IsValid() && location.AreaId < 45;
				if (flag5)
				{
					List<short> brokenAreas = ObjectPool<List<short>>.Instance.Get();
					DomainManager.Map.GetAllBrokenAreaInState(DomainManager.Map.GetStateIdByAreaId(location.AreaId), brokenAreas);
					bool flag6 = brokenAreas.Count > 0;
					if (flag6)
					{
						CollectionUtils.Shuffle<short>(context.Random, brokenAreas);
						List<MapBlockData> validBlocks = ObjectPool<List<MapBlockData>>.Instance.Get();
						short areaId = brokenAreas[0];
						DomainManager.Adventure.GetValidBlocksForRandomEnemy(areaId, -1, -1, true, true, true, validBlocks);
						bool flag7 = validBlocks.Count > 0;
						if (flag7)
						{
							CollectionUtils.Shuffle<MapBlockData>(context.Random, validBlocks);
							Location destLocation = validBlocks[0].GetLocation();
							DomainManager.Character.LeaveGroup(context, character, false);
							character.SetLocation(destLocation, context);
							Events.RaiseCharacterLocationChanged(context, charId, location, destLocation);
						}
						ObjectPool<List<MapBlockData>>.Instance.Return(validBlocks);
					}
					ObjectPool<List<short>>.Instance.Return(brokenAreas);
				}
				string tag3 = "LegendaryBook";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(12, 1);
				defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(character);
				defaultInterpolatedStringHandler.AppendLiteral(" => 因奇书被相枢吞噬");
				AdaptableLog.TagInfo(tag3, defaultInterpolatedStringHandler.ToStringAndClear());
				break;
			}
			}
		}

		// Token: 0x06007FF0 RID: 32752 RVA: 0x004CDD38 File Offset: 0x004CBF38
		public static void RaiseKidnappedStatusChanged(DataContext context, GameData.Domains.Character.Character character, bool isKidnapped)
		{
			int charId = character.GetId();
			if (isKidnapped)
			{
				DomainManager.Taiwu.RemoveVillagerWork(context, charId, true);
				DomainManager.Building.MakeTargetHomeless(context, charId);
				DomainManager.Building.TryRemoveFeastCustomer(context, charId);
			}
		}

		// Token: 0x06007FF1 RID: 32753 RVA: 0x004CDD7C File Offset: 0x004CBF7C
		public static void RaiseGraveLocationChanged(DataContext context, int graveId, Location srcLocation, Location destLocation)
		{
			DomainManager.Map.OnGraveLocationChanged(context, graveId, srcLocation, destLocation);
		}

		// Token: 0x06007FF2 RID: 32754 RVA: 0x004CDD8E File Offset: 0x004CBF8E
		public static void RaiseTemplateEnemyLocationChanged(DataContext context, MapTemplateEnemyInfo templateEnemyInfo, Location srcLocation, Location destLocation)
		{
			DomainManager.Map.OnTemplateEnemyLocationChanged(context, templateEnemyInfo, srcLocation, destLocation);
			DomainManager.Adventure.OnRandomEnemyLocationChange(context, templateEnemyInfo, srcLocation, destLocation);
		}

		// Token: 0x06007FF3 RID: 32755 RVA: 0x004CDDAF File Offset: 0x004CBFAF
		public static void RaiseInfectedCharacterLocationChanged(DataContext context, int charId, Location srcLocation, Location destLocation)
		{
			DomainManager.Map.OnInfectedCharacterLocationChanged(context, charId, srcLocation, destLocation);
		}

		// Token: 0x06007FF4 RID: 32756 RVA: 0x004CDDC1 File Offset: 0x004CBFC1
		public static void RaiseFixedCharacterLocationChanged(DataContext context, int charId, Location srcLocation, Location destLocation)
		{
			DomainManager.Map.OnFixedCharacterLocationChanged(context, charId, srcLocation, destLocation);
			DomainManager.Character.OnFixedCharacterLocationChanged(context, charId, srcLocation, destLocation);
		}

		// Token: 0x06007FF5 RID: 32757 RVA: 0x004CDDE2 File Offset: 0x004CBFE2
		public static void RaiseEnemyCharacterLocationChanged(DataContext context, int charId, Location srcLocation, Location destLocation)
		{
			DomainManager.Map.OnEnemyCharacterLocationChanged(context, charId, srcLocation, destLocation);
		}

		// Token: 0x06007FF6 RID: 32758 RVA: 0x004CDDF4 File Offset: 0x004CBFF4
		public static void RaiseCharacterCustomDisplayNameChanged(DataContext context, int charId)
		{
			DomainManager.TaiwuEvent.UpdateEventLogCharacterDisplayData(charId);
		}

		// Token: 0x06007FF7 RID: 32759 RVA: 0x004CDE03 File Offset: 0x004CC003
		public static void RaiseCombatEntry(DataContext context, List<int> enemyIds, short combatConfigTemplateId)
		{
			DomainManager.World.StatSectMainStoryCombatTimes(combatConfigTemplateId);
		}

		// Token: 0x06007FF8 RID: 32760 RVA: 0x004CDE14 File Offset: 0x004CC014
		public static void RaiseEventWindowFocusStateChanged(DataContext context, bool focusState)
		{
			DomainManager.Adventure.SetOperationBlock(focusState, context);
			bool flag = !focusState;
			if (flag)
			{
				DomainManager.TaiwuEvent.BlockEventLogStatusCheck();
			}
		}

		// Token: 0x06007FF9 RID: 32761 RVA: 0x004CDE42 File Offset: 0x004CC042
		public static void RaiseOneShotEventHandled(DataContext context, int oneShotEventType)
		{
			DomainManager.Extra.UpdateProfessionExtraSkillUnlocked(context, oneShotEventType);
		}

		// Token: 0x06007FFA RID: 32762 RVA: 0x004CDE52 File Offset: 0x004CC052
		public static void RaiseEventHandleComplete(DataContext context)
		{
			GameDataBridge.AddDisplayEvent(DisplayEventType.EventHandleComplete);
		}

		// Token: 0x06007FFB RID: 32763 RVA: 0x004CDE5D File Offset: 0x004CC05D
		public static void RaiseEventOnLegacyPassingStateChange(DataContext context, sbyte targetState)
		{
			DomainManager.TaiwuEvent.OnPassingLegacyStateChange(targetState);
		}

		// Token: 0x06007FFC RID: 32764 RVA: 0x004CDE6C File Offset: 0x004CC06C
		public static void ClearPassingLegacyWhileAdvancingMonthHandlers(DataContext context)
		{
			Events._handlersPassingLegacyWhileAdvancingMonth = null;
		}

		// Token: 0x06007FFD RID: 32765 RVA: 0x004CDE78 File Offset: 0x004CC078
		public static void RaiseCharacterApproveTaiwuStatusChanged(DataContext context, SettlementCharacter settlementChar, bool approve, bool updateBlock = true)
		{
			if (approve)
			{
				DomainManager.Taiwu.AddLegacyPoint(context, 38, 100);
				int seniority = ProfessionFormulaImpl.Calculate(55, (int)settlementChar.GetInfluencePower());
				DomainManager.Extra.ChangeProfessionSeniority(context, 8, seniority, true, false);
			}
			else
			{
				ProfessionData professionData = DomainManager.Extra.GetProfessionData(17);
				bool flag = professionData != null && professionData.SkillsData != null;
				if (flag)
				{
					ProfessionSkillHandle.DukeSkill_RemoveCharacterTitle(context, professionData, settlementChar.GetId());
				}
			}
			bool flag2 = !updateBlock;
			if (!flag2)
			{
				Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
				bool flag3 = !DomainManager.Taiwu.GetTaiwu().GetLocation().IsValid();
				if (!flag3)
				{
					MapBlockData blockData = DomainManager.Map.GetBlock(taiwuLocation);
					DomainManager.Map.SetBlockData(context, blockData);
				}
			}
		}

		// Token: 0x06007FFE RID: 32766 RVA: 0x004CDF4D File Offset: 0x004CC14D
		public static void RaiseEvaluationAddExp(DataContext context, int exp)
		{
			DomainManager.World.AddEmeiExp(context, exp);
		}

		// Token: 0x06007FFF RID: 32767 RVA: 0x004CDF5D File Offset: 0x004CC15D
		public static void RaiseCricketCombatStarted(DataContext context)
		{
			DomainManager.TaiwuEvent.RecordCharacterEnterCricketCombat();
		}

		// Token: 0x06008000 RID: 32768 RVA: 0x004CDF6B File Offset: 0x004CC16B
		public static void RaiseCricketCombatFinished(DataContext context, bool isTaiwuWin)
		{
			DomainManager.TaiwuEvent.RecordCombatResult(isTaiwuWin);
		}

		// Token: 0x06008001 RID: 32769 RVA: 0x004CDF7A File Offset: 0x004CC17A
		public static void RaiseLifeSkillCombatStarted(DataContext context)
		{
			DomainManager.TaiwuEvent.RecordCharacterEnterLifeCombat();
		}

		// Token: 0x06008002 RID: 32770 RVA: 0x004CDF88 File Offset: 0x004CC188
		public static void RaiseTaiwuOrTeammatePregnant(DataContext context, int charId, Location location)
		{
			DomainManager.World.GetMonthlyEventCollection().AddPregnant(charId, location);
		}

		// Token: 0x06008003 RID: 32771 RVA: 0x004CDFA0 File Offset: 0x004CC1A0
		public static void RaiseAdventureSiteStateChanged(DataContext context, short areaId, short blockId, AdventureSiteData siteData)
		{
			sbyte siteState = siteData.SiteState;
			sbyte b = siteState;
			if (b == 1)
			{
				GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
				Location taiwuLocation = taiwu.GetLocation();
				bool flag = taiwuLocation.AreaId != areaId;
				if (!flag)
				{
					Location adventureLocation = new Location(areaId, blockId);
					MapBlockData blockData = DomainManager.Map.GetBlock(adventureLocation);
					InstantNotificationCollection instantNotifications = DomainManager.World.GetInstantNotifications();
					bool flag2 = taiwuLocation.AreaId == areaId && blockData.Visible;
					if (flag2)
					{
						instantNotifications.AddBeginAdventure(adventureLocation, siteData.TemplateId);
					}
				}
			}
		}

		// Token: 0x06008004 RID: 32772 RVA: 0x004CE034 File Offset: 0x004CC234
		public static void RaiseAdventureRemoved(DataContext context, short areaId, short blockId, bool isTimeout, bool isComplete, AdventureSiteData siteData)
		{
			MonthlyActionBase monthlyAction = DomainManager.TaiwuEvent.GetMonthlyAction(siteData.MonthlyActionKey);
			bool flag = monthlyAction != null;
			if (flag)
			{
				IMonthlyActionGroup actionGroup = monthlyAction as IMonthlyActionGroup;
				bool flag2 = actionGroup != null;
				if (flag2)
				{
					actionGroup.DeactivateSubAction(areaId, blockId, isComplete);
				}
				else
				{
					monthlyAction.Deactivate(isComplete);
				}
				bool flag3 = siteData.MonthlyActionKey.ActionType == 6;
				if (flag3)
				{
					DomainManager.TaiwuEvent.RemoveTempDynamicAction(DomainManager.TaiwuEvent.MainThreadDataContext, siteData.MonthlyActionKey);
				}
			}
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			Location location = new Location(areaId, blockId);
			bool flag4 = siteData.SiteState >= 2;
			if (flag4)
			{
				monthlyNotificationCollection.AddEnemyNestDemise(location, siteData.TemplateId);
			}
			sbyte type = siteData.GetConfig().Type;
			bool flag5 = type >= 9 && type <= 14;
			if (flag5)
			{
				DomainManager.Extra.OnBuildingAreaEffectLocationChanged(context, 2, location, Location.Invalid);
			}
			short templateId = siteData.TemplateId;
			short num = templateId;
			if (num <= 170)
			{
				if (num == 28)
				{
					EventArgBox globalEventArgBox = DomainManager.TaiwuEvent.GetGlobalEventArgumentBox();
					int charId = -1;
					bool flag6 = globalEventArgBox.Get("StoryForeverLoverId", ref charId);
					if (flag6)
					{
						GameData.Domains.Character.Character lover;
						bool flag7 = charId >= 0 && DomainManager.Character.TryGetElement_Objects(charId, out lover);
						if (flag7)
						{
							OrganizationInfo orgInfo = lover.GetOrganizationInfo();
							OrganizationItem orgConfig = Organization.Instance[orgInfo.OrgTemplateId];
							bool isSect = orgConfig.IsSect;
							if (isSect)
							{
								DomainManager.Character.UnhideCharacterOnMap(context, lover, 16);
								short featureId = orgConfig.PunishmentFeature;
								bool flag8 = featureId >= 0;
								if (flag8)
								{
									lover.AddFeature(context, featureId, false);
								}
								Sect sect;
								bool flag9 = DomainManager.Organization.TryGetElement_Sects(orgInfo.SettlementId, out sect);
								if (flag9)
								{
									DomainManager.Organization.PunishSectMember(context, sect, lover, 3, 43, true);
									LifeRecordCollection lifeRecordCollection = EventHelper.GetLifeRecordCollection();
									lifeRecordCollection.AddSectPunishElope(charId, EventHelper.GetGameDate());
								}
							}
						}
						globalEventArgBox.Remove<int>("StoryForeverLoverId");
					}
					goto IL_273;
				}
				if (num != 170)
				{
					goto IL_273;
				}
			}
			else if (num != 184)
			{
				if (num != 193)
				{
					goto IL_273;
				}
				if (isTimeout)
				{
					MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
					monthlyEventCollection.AddSectMainStoryZhujianFailing();
				}
				goto IL_273;
			}
			if (isTimeout)
			{
				DomainManager.Extra.TriggerExtraTask(context, 40, 300);
			}
			IL_273:
			sbyte xiangshuAvatarId = XiangshuAvatarIds.GetXiangshuAvatarIdBySwordTomb(siteData.TemplateId);
			bool flag10 = xiangshuAvatarId >= 0;
			if (flag10)
			{
				for (short weakenedXiangshuTemplateId = XiangshuAvatarIds.WeakenedXiangshuBossBeginIds[(int)xiangshuAvatarId]; weakenedXiangshuTemplateId <= XiangshuAvatarIds.WeakenedXiangshuBossEndIds[(int)xiangshuAvatarId]; weakenedXiangshuTemplateId += 1)
				{
					GameData.Domains.Character.Character character;
					bool flag11 = DomainManager.Character.TryGetFixedCharacterByTemplateId(weakenedXiangshuTemplateId, out character);
					if (flag11)
					{
						DomainManager.Character.RemoveNonIntelligentCharacter(context, character);
					}
				}
				IReadOnlySet<int> swordTombKeepers = DomainManager.Taiwu.GetVillagerRoleSet(5);
				foreach (int charId2 in swordTombKeepers)
				{
					VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(charId2);
					VillagerRoleSwordTombKeeper keeper = villagerRole as VillagerRoleSwordTombKeeper;
					bool flag12 = keeper != null && keeper.ArrangementTemplateId == 3 && keeper.XiangshuAvatarId == xiangshuAvatarId;
					if (flag12)
					{
						DomainManager.Taiwu.RemoveVillagerWork(context, charId2, true);
					}
				}
			}
			bool flag13 = DomainManager.World.GetAdvancingMonthState() == 0;
			if (flag13)
			{
				DomainManager.TaiwuEvent.OnEvent_AdventureRemoved(siteData.TemplateId, location, isComplete);
			}
		}

		// Token: 0x06008005 RID: 32773 RVA: 0x004CE3D8 File Offset: 0x004CC5D8
		public static void RaiseCricketCreated(DataContext context, ItemKey cricketKey)
		{
			DomainManager.Extra.OnCricketCreated(context, cricketKey);
		}

		// Token: 0x06008006 RID: 32774 RVA: 0x004CE3E8 File Offset: 0x004CC5E8
		public static void RaiseCricketRemoved(DataContext context, ItemKey cricketKey)
		{
			DomainManager.Extra.OnCricketRemoved(context, cricketKey);
		}

		// Token: 0x06008007 RID: 32775 RVA: 0x004CE3F8 File Offset: 0x004CC5F8
		public static void RaiseCarrierCreated(DataContext context, ItemKey carrierKey)
		{
			DomainManager.Extra.OnCarrierCreated(context, carrierKey);
		}

		// Token: 0x06008008 RID: 32776 RVA: 0x004CE408 File Offset: 0x004CC608
		public static void RaiseCarrierRemoved(DataContext context, ItemKey carrierKey)
		{
			DomainManager.Extra.OnCarrierRemoved(context, carrierKey);
		}

		// Token: 0x06008009 RID: 32777 RVA: 0x004CE418 File Offset: 0x004CC618
		public static void RaiseSettlementInfoChanged(DataContext context, Settlement settlement)
		{
			List<short> blockIds = new List<short>();
			Location location = settlement.GetLocation();
			DomainManager.Map.GetSettlementBlocks(location.AreaId, location.BlockId, blockIds);
			foreach (short blockId in blockIds)
			{
				MapBlockData blockData = DomainManager.Map.GetBlockData(location.AreaId, blockId);
				DomainManager.Map.SetBlockData(context, blockData);
			}
		}

		// Token: 0x0600800A RID: 32778 RVA: 0x004CE4AC File Offset: 0x004CC6AC
		public static void ClearAllHandlers()
		{
			Events._handlersCharacterLocationChanged = null;
			Events._handlersMakeLove = null;
			Events._handlersEatingItem = null;
			Events._handlersXiangshuInfectionFeatureChangedEnd = null;
			Events._handlersCombatBegin = null;
			Events._handlersCombatSettlement = null;
			Events._handlersCombatEnd = null;
			Events._handlersChangeNeiliAllocationAfterCombatBegin = null;
			Events._handlersCreateGangqiAfterChangeNeiliAllocation = null;
			Events._handlersChangeBossPhase = null;
			Events._handlersGetTrick = null;
			Events._handlersRearrangeTrick = null;
			Events._handlersGetShaTrick = null;
			Events._handlersRemoveShaTrick = null;
			Events._handlersOverflowTrickRemoved = null;
			Events._handlersCostBreathAndStance = null;
			Events._handlersChangeWeapon = null;
			Events._handlersWeaponCdEnd = null;
			Events._handlersChangeTrickCountChanged = null;
			Events._handlersUnlockAttack = null;
			Events._handlersUnlockAttackEnd = null;
			Events._handlersNormalAttackPrepareEnd = null;
			Events._handlersNormalAttackOutOfRange = null;
			Events._handlersNormalAttackBegin = null;
			Events._handlersNormalAttackCalcHitEnd = null;
			Events._handlersNormalAttackCalcCriticalEnd = null;
			Events._handlersNormalAttackEnd = null;
			Events._handlersNormalAttackAllEnd = null;
			Events._handlersCastSkillUseExtraBreathOrStance = null;
			Events._handlersCastSkillUseMobilityAsBreathOrStance = null;
			Events._handlersCastLegSkillWithAgile = null;
			Events._handlersCastSkillOnLackBreathStance = null;
			Events._handlersCastSkillTrickCosted = null;
			Events._handlersJiTrickInsteadCostTricks = null;
			Events._handlersUselessTrickInsteadJiTricks = null;
			Events._handlersShaTrickInsteadCostTricks = null;
			Events._handlersCastSkillCosted = null;
			Events._handlersChangePreparingSkillBegin = null;
			Events._handlersCastAgileOrDefenseWithoutPrepareBegin = null;
			Events._handlersCastAgileOrDefenseWithoutPrepareEnd = null;
			Events._handlersPrepareSkillEffectNotYetCreated = null;
			Events._handlersPrepareSkillBegin = null;
			Events._handlersPrepareSkillProgressChange = null;
			Events._handlersPrepareSkillChangeDistance = null;
			Events._handlersPrepareSkillEnd = null;
			Events._handlersCastAttackSkillBegin = null;
			Events._handlersAttackSkillAttackBegin = null;
			Events._handlersAttackSkillAttackHit = null;
			Events._handlersAttackSkillAttackEnd = null;
			Events._handlersAttackSkillAttackEndOfAll = null;
			Events._handlersCastSkillEnd = null;
			Events._handlersCastSkillAllEnd = null;
			Events._handlersCalcLeveragingValue = null;
			Events._handlersWisdomCosted = null;
			Events._handlersHealedInjury = null;
			Events._handlersHealedPoison = null;
			Events._handlersUsedMedicine = null;
			Events._handlersUsedCustomItem = null;
			Events._handlersInterruptOtherAction = null;
			Events._handlersFlawAdded = null;
			Events._handlersFlawRemoved = null;
			Events._handlersAcuPointAdded = null;
			Events._handlersAcuPointRemoved = null;
			Events._handlersCombatCharChanged = null;
			Events._handlersAddInjury = null;
			Events._handlersAddDirectDamageValue = null;
			Events._handlersAddDirectInjury = null;
			Events._handlersBounceInjury = null;
			Events._handlersAddMindMark = null;
			Events._handlersAddMindDamage = null;
			Events._handlersAddFatalDamageMark = null;
			Events._handlersAddDirectFatalDamageMark = null;
			Events._handlersAddDirectFatalDamage = null;
			Events._handlersAddDirectPoisonMark = null;
			Events._handlersMoveStateChanged = null;
			Events._handlersMoveBegin = null;
			Events._handlersMoveEnd = null;
			Events._handlersIgnoredForceChangeDistance = null;
			Events._handlersDistanceChanged = null;
			Events._handlersSkillEffectChange = null;
			Events._handlersSkillSilence = null;
			Events._handlersSkillSilenceEnd = null;
			Events._handlersNeiliAllocationChanged = null;
			Events._handlersAddPoison = null;
			Events._handlersPoisonAffected = null;
			Events._handlersAddWug = null;
			Events._handlersRemoveWug = null;
			Events._handlersCompareDataCalcFinished = null;
			Events._handlersCombatStateMachineUpdateEnd = null;
			Events._handlersCombatCharFallen = null;
			Events._handlersCombatCostNeiliConfirm = null;
			Events._handlersCostTrickDuringPreparingSkill = null;
			Events._handlersCombatChangeDurability = null;
			Events._handlersPassingLegacyWhileAdvancingMonth = null;
			Events._handlersAdvanceMonthBegin = null;
			Events._handlersPostAdvanceMonthBegin = null;
			Events._handlersAdvanceMonthFinish = null;
			Events._handlersTaiwuMove = null;
		}

		// Token: 0x0600800B RID: 32779 RVA: 0x004CE706 File Offset: 0x004CC906
		public static void RegisterHandler_CharacterLocationChanged(Events.OnCharacterLocationChanged handler)
		{
			Events._handlersCharacterLocationChanged = (Events.OnCharacterLocationChanged)Delegate.Combine(Events._handlersCharacterLocationChanged, handler);
		}

		// Token: 0x0600800C RID: 32780 RVA: 0x004CE71E File Offset: 0x004CC91E
		public static void UnRegisterHandler_CharacterLocationChanged(Events.OnCharacterLocationChanged handler)
		{
			Events._handlersCharacterLocationChanged = (Events.OnCharacterLocationChanged)Delegate.Remove(Events._handlersCharacterLocationChanged, handler);
		}

		// Token: 0x0600800D RID: 32781 RVA: 0x004CE736 File Offset: 0x004CC936
		public static void RaiseCharacterLocationChanged(DataContext context, int charId, Location srcLocation, Location destLocation)
		{
			Events.OnCharacterLocationChanged handlersCharacterLocationChanged = Events._handlersCharacterLocationChanged;
			if (handlersCharacterLocationChanged != null)
			{
				handlersCharacterLocationChanged(context, charId, srcLocation, destLocation);
			}
		}

		// Token: 0x0600800E RID: 32782 RVA: 0x004CE74E File Offset: 0x004CC94E
		public static void RegisterHandler_MakeLove(Events.OnMakeLove handler)
		{
			Events._handlersMakeLove = (Events.OnMakeLove)Delegate.Combine(Events._handlersMakeLove, handler);
		}

		// Token: 0x0600800F RID: 32783 RVA: 0x004CE766 File Offset: 0x004CC966
		public static void UnRegisterHandler_MakeLove(Events.OnMakeLove handler)
		{
			Events._handlersMakeLove = (Events.OnMakeLove)Delegate.Remove(Events._handlersMakeLove, handler);
		}

		// Token: 0x06008010 RID: 32784 RVA: 0x004CE77E File Offset: 0x004CC97E
		public static void RaiseMakeLove(DataContext context, GameData.Domains.Character.Character character, GameData.Domains.Character.Character target, sbyte makeLoveState)
		{
			Events.OnMakeLove handlersMakeLove = Events._handlersMakeLove;
			if (handlersMakeLove != null)
			{
				handlersMakeLove(context, character, target, makeLoveState);
			}
		}

		// Token: 0x06008011 RID: 32785 RVA: 0x004CE796 File Offset: 0x004CC996
		public static void RegisterHandler_EatingItem(Events.OnEatingItem handler)
		{
			Events._handlersEatingItem = (Events.OnEatingItem)Delegate.Combine(Events._handlersEatingItem, handler);
		}

		// Token: 0x06008012 RID: 32786 RVA: 0x004CE7AE File Offset: 0x004CC9AE
		public static void UnRegisterHandler_EatingItem(Events.OnEatingItem handler)
		{
			Events._handlersEatingItem = (Events.OnEatingItem)Delegate.Remove(Events._handlersEatingItem, handler);
		}

		// Token: 0x06008013 RID: 32787 RVA: 0x004CE7C6 File Offset: 0x004CC9C6
		public static void RaiseEatingItem(DataContext context, GameData.Domains.Character.Character character, ItemKey itemKey)
		{
			Events.OnEatingItem handlersEatingItem = Events._handlersEatingItem;
			if (handlersEatingItem != null)
			{
				handlersEatingItem(context, character, itemKey);
			}
		}

		// Token: 0x06008014 RID: 32788 RVA: 0x004CE7DD File Offset: 0x004CC9DD
		public static void RegisterHandler_XiangshuInfectionFeatureChangedEnd(Events.OnXiangshuInfectionFeatureChangedEnd handler)
		{
			Events._handlersXiangshuInfectionFeatureChangedEnd = (Events.OnXiangshuInfectionFeatureChangedEnd)Delegate.Combine(Events._handlersXiangshuInfectionFeatureChangedEnd, handler);
		}

		// Token: 0x06008015 RID: 32789 RVA: 0x004CE7F5 File Offset: 0x004CC9F5
		public static void UnRegisterHandler_XiangshuInfectionFeatureChangedEnd(Events.OnXiangshuInfectionFeatureChangedEnd handler)
		{
			Events._handlersXiangshuInfectionFeatureChangedEnd = (Events.OnXiangshuInfectionFeatureChangedEnd)Delegate.Remove(Events._handlersXiangshuInfectionFeatureChangedEnd, handler);
		}

		// Token: 0x06008016 RID: 32790 RVA: 0x004CE80D File Offset: 0x004CCA0D
		public static void RaiseXiangshuInfectionFeatureChangedEnd(DataContext context, GameData.Domains.Character.Character character, short featureId)
		{
			Events.OnXiangshuInfectionFeatureChangedEnd handlersXiangshuInfectionFeatureChangedEnd = Events._handlersXiangshuInfectionFeatureChangedEnd;
			if (handlersXiangshuInfectionFeatureChangedEnd != null)
			{
				handlersXiangshuInfectionFeatureChangedEnd(context, character, featureId);
			}
		}

		// Token: 0x06008017 RID: 32791 RVA: 0x004CE824 File Offset: 0x004CCA24
		public static void RegisterHandler_CombatBegin(Events.OnCombatBegin handler)
		{
			Events._handlersCombatBegin = (Events.OnCombatBegin)Delegate.Combine(Events._handlersCombatBegin, handler);
		}

		// Token: 0x06008018 RID: 32792 RVA: 0x004CE83C File Offset: 0x004CCA3C
		public static void UnRegisterHandler_CombatBegin(Events.OnCombatBegin handler)
		{
			Events._handlersCombatBegin = (Events.OnCombatBegin)Delegate.Remove(Events._handlersCombatBegin, handler);
		}

		// Token: 0x06008019 RID: 32793 RVA: 0x004CE854 File Offset: 0x004CCA54
		public static void RaiseCombatBegin(DataContext context)
		{
			Events.OnCombatBegin handlersCombatBegin = Events._handlersCombatBegin;
			if (handlersCombatBegin != null)
			{
				handlersCombatBegin(context);
			}
		}

		// Token: 0x0600801A RID: 32794 RVA: 0x004CE869 File Offset: 0x004CCA69
		public static void RegisterHandler_CombatSettlement(Events.OnCombatSettlement handler)
		{
			Events._handlersCombatSettlement = (Events.OnCombatSettlement)Delegate.Combine(Events._handlersCombatSettlement, handler);
		}

		// Token: 0x0600801B RID: 32795 RVA: 0x004CE881 File Offset: 0x004CCA81
		public static void UnRegisterHandler_CombatSettlement(Events.OnCombatSettlement handler)
		{
			Events._handlersCombatSettlement = (Events.OnCombatSettlement)Delegate.Remove(Events._handlersCombatSettlement, handler);
		}

		// Token: 0x0600801C RID: 32796 RVA: 0x004CE899 File Offset: 0x004CCA99
		public static void RaiseCombatSettlement(DataContext context, sbyte combatStatus)
		{
			Events.OnCombatSettlement handlersCombatSettlement = Events._handlersCombatSettlement;
			if (handlersCombatSettlement != null)
			{
				handlersCombatSettlement(context, combatStatus);
			}
		}

		// Token: 0x0600801D RID: 32797 RVA: 0x004CE8AF File Offset: 0x004CCAAF
		public static void RegisterHandler_CombatEnd(Events.OnCombatEnd handler)
		{
			Events._handlersCombatEnd = (Events.OnCombatEnd)Delegate.Combine(Events._handlersCombatEnd, handler);
		}

		// Token: 0x0600801E RID: 32798 RVA: 0x004CE8C7 File Offset: 0x004CCAC7
		public static void UnRegisterHandler_CombatEnd(Events.OnCombatEnd handler)
		{
			Events._handlersCombatEnd = (Events.OnCombatEnd)Delegate.Remove(Events._handlersCombatEnd, handler);
		}

		// Token: 0x0600801F RID: 32799 RVA: 0x004CE8DF File Offset: 0x004CCADF
		public static void RaiseCombatEnd(DataContext context)
		{
			Events.OnCombatEnd handlersCombatEnd = Events._handlersCombatEnd;
			if (handlersCombatEnd != null)
			{
				handlersCombatEnd(context);
			}
		}

		// Token: 0x06008020 RID: 32800 RVA: 0x004CE8F4 File Offset: 0x004CCAF4
		public static void RegisterHandler_ChangeNeiliAllocationAfterCombatBegin(Events.OnChangeNeiliAllocationAfterCombatBegin handler)
		{
			Events._handlersChangeNeiliAllocationAfterCombatBegin = (Events.OnChangeNeiliAllocationAfterCombatBegin)Delegate.Combine(Events._handlersChangeNeiliAllocationAfterCombatBegin, handler);
		}

		// Token: 0x06008021 RID: 32801 RVA: 0x004CE90C File Offset: 0x004CCB0C
		public static void UnRegisterHandler_ChangeNeiliAllocationAfterCombatBegin(Events.OnChangeNeiliAllocationAfterCombatBegin handler)
		{
			Events._handlersChangeNeiliAllocationAfterCombatBegin = (Events.OnChangeNeiliAllocationAfterCombatBegin)Delegate.Remove(Events._handlersChangeNeiliAllocationAfterCombatBegin, handler);
		}

		// Token: 0x06008022 RID: 32802 RVA: 0x004CE924 File Offset: 0x004CCB24
		public static void RaiseChangeNeiliAllocationAfterCombatBegin(DataContext context, CombatCharacter character, NeiliAllocation allocationAfterBegin)
		{
			Events.OnChangeNeiliAllocationAfterCombatBegin handlersChangeNeiliAllocationAfterCombatBegin = Events._handlersChangeNeiliAllocationAfterCombatBegin;
			if (handlersChangeNeiliAllocationAfterCombatBegin != null)
			{
				handlersChangeNeiliAllocationAfterCombatBegin(context, character, allocationAfterBegin);
			}
		}

		// Token: 0x06008023 RID: 32803 RVA: 0x004CE93B File Offset: 0x004CCB3B
		public static void RegisterHandler_CreateGangqiAfterChangeNeiliAllocation(Events.OnCreateGangqiAfterChangeNeiliAllocation handler)
		{
			Events._handlersCreateGangqiAfterChangeNeiliAllocation = (Events.OnCreateGangqiAfterChangeNeiliAllocation)Delegate.Combine(Events._handlersCreateGangqiAfterChangeNeiliAllocation, handler);
		}

		// Token: 0x06008024 RID: 32804 RVA: 0x004CE953 File Offset: 0x004CCB53
		public static void UnRegisterHandler_CreateGangqiAfterChangeNeiliAllocation(Events.OnCreateGangqiAfterChangeNeiliAllocation handler)
		{
			Events._handlersCreateGangqiAfterChangeNeiliAllocation = (Events.OnCreateGangqiAfterChangeNeiliAllocation)Delegate.Remove(Events._handlersCreateGangqiAfterChangeNeiliAllocation, handler);
		}

		// Token: 0x06008025 RID: 32805 RVA: 0x004CE96B File Offset: 0x004CCB6B
		public static void RaiseCreateGangqiAfterChangeNeiliAllocation(DataContext context, CombatCharacter character)
		{
			Events.OnCreateGangqiAfterChangeNeiliAllocation handlersCreateGangqiAfterChangeNeiliAllocation = Events._handlersCreateGangqiAfterChangeNeiliAllocation;
			if (handlersCreateGangqiAfterChangeNeiliAllocation != null)
			{
				handlersCreateGangqiAfterChangeNeiliAllocation(context, character);
			}
		}

		// Token: 0x06008026 RID: 32806 RVA: 0x004CE981 File Offset: 0x004CCB81
		public static void RegisterHandler_ChangeBossPhase(Events.OnChangeBossPhase handler)
		{
			Events._handlersChangeBossPhase = (Events.OnChangeBossPhase)Delegate.Combine(Events._handlersChangeBossPhase, handler);
		}

		// Token: 0x06008027 RID: 32807 RVA: 0x004CE999 File Offset: 0x004CCB99
		public static void UnRegisterHandler_ChangeBossPhase(Events.OnChangeBossPhase handler)
		{
			Events._handlersChangeBossPhase = (Events.OnChangeBossPhase)Delegate.Remove(Events._handlersChangeBossPhase, handler);
		}

		// Token: 0x06008028 RID: 32808 RVA: 0x004CE9B1 File Offset: 0x004CCBB1
		public static void RaiseChangeBossPhase(DataContext context)
		{
			Events.OnChangeBossPhase handlersChangeBossPhase = Events._handlersChangeBossPhase;
			if (handlersChangeBossPhase != null)
			{
				handlersChangeBossPhase(context);
			}
		}

		// Token: 0x06008029 RID: 32809 RVA: 0x004CE9C6 File Offset: 0x004CCBC6
		public static void RegisterHandler_GetTrick(Events.OnGetTrick handler)
		{
			Events._handlersGetTrick = (Events.OnGetTrick)Delegate.Combine(Events._handlersGetTrick, handler);
		}

		// Token: 0x0600802A RID: 32810 RVA: 0x004CE9DE File Offset: 0x004CCBDE
		public static void UnRegisterHandler_GetTrick(Events.OnGetTrick handler)
		{
			Events._handlersGetTrick = (Events.OnGetTrick)Delegate.Remove(Events._handlersGetTrick, handler);
		}

		// Token: 0x0600802B RID: 32811 RVA: 0x004CE9F6 File Offset: 0x004CCBF6
		public static void RaiseGetTrick(DataContext context, int charId, bool isAlly, sbyte trickType, bool usable)
		{
			Events.OnGetTrick handlersGetTrick = Events._handlersGetTrick;
			if (handlersGetTrick != null)
			{
				handlersGetTrick(context, charId, isAlly, trickType, usable);
			}
		}

		// Token: 0x0600802C RID: 32812 RVA: 0x004CEA10 File Offset: 0x004CCC10
		public static void RegisterHandler_RearrangeTrick(Events.OnRearrangeTrick handler)
		{
			Events._handlersRearrangeTrick = (Events.OnRearrangeTrick)Delegate.Combine(Events._handlersRearrangeTrick, handler);
		}

		// Token: 0x0600802D RID: 32813 RVA: 0x004CEA28 File Offset: 0x004CCC28
		public static void UnRegisterHandler_RearrangeTrick(Events.OnRearrangeTrick handler)
		{
			Events._handlersRearrangeTrick = (Events.OnRearrangeTrick)Delegate.Remove(Events._handlersRearrangeTrick, handler);
		}

		// Token: 0x0600802E RID: 32814 RVA: 0x004CEA40 File Offset: 0x004CCC40
		public static void RaiseRearrangeTrick(DataContext context, int charId, bool isAlly)
		{
			Events.OnRearrangeTrick handlersRearrangeTrick = Events._handlersRearrangeTrick;
			if (handlersRearrangeTrick != null)
			{
				handlersRearrangeTrick(context, charId, isAlly);
			}
		}

		// Token: 0x0600802F RID: 32815 RVA: 0x004CEA57 File Offset: 0x004CCC57
		public static void RegisterHandler_GetShaTrick(Events.OnGetShaTrick handler)
		{
			Events._handlersGetShaTrick = (Events.OnGetShaTrick)Delegate.Combine(Events._handlersGetShaTrick, handler);
		}

		// Token: 0x06008030 RID: 32816 RVA: 0x004CEA6F File Offset: 0x004CCC6F
		public static void UnRegisterHandler_GetShaTrick(Events.OnGetShaTrick handler)
		{
			Events._handlersGetShaTrick = (Events.OnGetShaTrick)Delegate.Remove(Events._handlersGetShaTrick, handler);
		}

		// Token: 0x06008031 RID: 32817 RVA: 0x004CEA87 File Offset: 0x004CCC87
		public static void RaiseGetShaTrick(DataContext context, int charId, bool isAlly, bool real)
		{
			Events.OnGetShaTrick handlersGetShaTrick = Events._handlersGetShaTrick;
			if (handlersGetShaTrick != null)
			{
				handlersGetShaTrick(context, charId, isAlly, real);
			}
		}

		// Token: 0x06008032 RID: 32818 RVA: 0x004CEA9F File Offset: 0x004CCC9F
		public static void RegisterHandler_RemoveShaTrick(Events.OnRemoveShaTrick handler)
		{
			Events._handlersRemoveShaTrick = (Events.OnRemoveShaTrick)Delegate.Combine(Events._handlersRemoveShaTrick, handler);
		}

		// Token: 0x06008033 RID: 32819 RVA: 0x004CEAB7 File Offset: 0x004CCCB7
		public static void UnRegisterHandler_RemoveShaTrick(Events.OnRemoveShaTrick handler)
		{
			Events._handlersRemoveShaTrick = (Events.OnRemoveShaTrick)Delegate.Remove(Events._handlersRemoveShaTrick, handler);
		}

		// Token: 0x06008034 RID: 32820 RVA: 0x004CEACF File Offset: 0x004CCCCF
		public static void RaiseRemoveShaTrick(DataContext context, int charId)
		{
			Events.OnRemoveShaTrick handlersRemoveShaTrick = Events._handlersRemoveShaTrick;
			if (handlersRemoveShaTrick != null)
			{
				handlersRemoveShaTrick(context, charId);
			}
		}

		// Token: 0x06008035 RID: 32821 RVA: 0x004CEAE5 File Offset: 0x004CCCE5
		public static void RegisterHandler_OverflowTrickRemoved(Events.OnOverflowTrickRemoved handler)
		{
			Events._handlersOverflowTrickRemoved = (Events.OnOverflowTrickRemoved)Delegate.Combine(Events._handlersOverflowTrickRemoved, handler);
		}

		// Token: 0x06008036 RID: 32822 RVA: 0x004CEAFD File Offset: 0x004CCCFD
		public static void UnRegisterHandler_OverflowTrickRemoved(Events.OnOverflowTrickRemoved handler)
		{
			Events._handlersOverflowTrickRemoved = (Events.OnOverflowTrickRemoved)Delegate.Remove(Events._handlersOverflowTrickRemoved, handler);
		}

		// Token: 0x06008037 RID: 32823 RVA: 0x004CEB15 File Offset: 0x004CCD15
		public static void RaiseOverflowTrickRemoved(DataContext context, int charId, bool isAlly, int removedCount)
		{
			Events.OnOverflowTrickRemoved handlersOverflowTrickRemoved = Events._handlersOverflowTrickRemoved;
			if (handlersOverflowTrickRemoved != null)
			{
				handlersOverflowTrickRemoved(context, charId, isAlly, removedCount);
			}
		}

		// Token: 0x06008038 RID: 32824 RVA: 0x004CEB2D File Offset: 0x004CCD2D
		public static void RegisterHandler_CostBreathAndStance(Events.OnCostBreathAndStance handler)
		{
			Events._handlersCostBreathAndStance = (Events.OnCostBreathAndStance)Delegate.Combine(Events._handlersCostBreathAndStance, handler);
		}

		// Token: 0x06008039 RID: 32825 RVA: 0x004CEB45 File Offset: 0x004CCD45
		public static void UnRegisterHandler_CostBreathAndStance(Events.OnCostBreathAndStance handler)
		{
			Events._handlersCostBreathAndStance = (Events.OnCostBreathAndStance)Delegate.Remove(Events._handlersCostBreathAndStance, handler);
		}

		// Token: 0x0600803A RID: 32826 RVA: 0x004CEB5D File Offset: 0x004CCD5D
		public static void RaiseCostBreathAndStance(DataContext context, int charId, bool isAlly, int costBreath, int costStance, short skillId)
		{
			Events.OnCostBreathAndStance handlersCostBreathAndStance = Events._handlersCostBreathAndStance;
			if (handlersCostBreathAndStance != null)
			{
				handlersCostBreathAndStance(context, charId, isAlly, costBreath, costStance, skillId);
			}
		}

		// Token: 0x0600803B RID: 32827 RVA: 0x004CEB79 File Offset: 0x004CCD79
		public static void RegisterHandler_ChangeWeapon(Events.OnChangeWeapon handler)
		{
			Events._handlersChangeWeapon = (Events.OnChangeWeapon)Delegate.Combine(Events._handlersChangeWeapon, handler);
		}

		// Token: 0x0600803C RID: 32828 RVA: 0x004CEB91 File Offset: 0x004CCD91
		public static void UnRegisterHandler_ChangeWeapon(Events.OnChangeWeapon handler)
		{
			Events._handlersChangeWeapon = (Events.OnChangeWeapon)Delegate.Remove(Events._handlersChangeWeapon, handler);
		}

		// Token: 0x0600803D RID: 32829 RVA: 0x004CEBA9 File Offset: 0x004CCDA9
		public static void RaiseChangeWeapon(DataContext context, int charId, bool isAlly, CombatWeaponData newWeapon, CombatWeaponData oldWeapon)
		{
			Events.OnChangeWeapon handlersChangeWeapon = Events._handlersChangeWeapon;
			if (handlersChangeWeapon != null)
			{
				handlersChangeWeapon(context, charId, isAlly, newWeapon, oldWeapon);
			}
		}

		// Token: 0x0600803E RID: 32830 RVA: 0x004CEBC3 File Offset: 0x004CCDC3
		public static void RegisterHandler_WeaponCdEnd(Events.OnWeaponCdEnd handler)
		{
			Events._handlersWeaponCdEnd = (Events.OnWeaponCdEnd)Delegate.Combine(Events._handlersWeaponCdEnd, handler);
		}

		// Token: 0x0600803F RID: 32831 RVA: 0x004CEBDB File Offset: 0x004CCDDB
		public static void UnRegisterHandler_WeaponCdEnd(Events.OnWeaponCdEnd handler)
		{
			Events._handlersWeaponCdEnd = (Events.OnWeaponCdEnd)Delegate.Remove(Events._handlersWeaponCdEnd, handler);
		}

		// Token: 0x06008040 RID: 32832 RVA: 0x004CEBF3 File Offset: 0x004CCDF3
		public static void RaiseWeaponCdEnd(DataContext context, int charId, bool isAlly, CombatWeaponData weapon)
		{
			Events.OnWeaponCdEnd handlersWeaponCdEnd = Events._handlersWeaponCdEnd;
			if (handlersWeaponCdEnd != null)
			{
				handlersWeaponCdEnd(context, charId, isAlly, weapon);
			}
		}

		// Token: 0x06008041 RID: 32833 RVA: 0x004CEC0B File Offset: 0x004CCE0B
		public static void RegisterHandler_ChangeTrickCountChanged(Events.OnChangeTrickCountChanged handler)
		{
			Events._handlersChangeTrickCountChanged = (Events.OnChangeTrickCountChanged)Delegate.Combine(Events._handlersChangeTrickCountChanged, handler);
		}

		// Token: 0x06008042 RID: 32834 RVA: 0x004CEC23 File Offset: 0x004CCE23
		public static void UnRegisterHandler_ChangeTrickCountChanged(Events.OnChangeTrickCountChanged handler)
		{
			Events._handlersChangeTrickCountChanged = (Events.OnChangeTrickCountChanged)Delegate.Remove(Events._handlersChangeTrickCountChanged, handler);
		}

		// Token: 0x06008043 RID: 32835 RVA: 0x004CEC3B File Offset: 0x004CCE3B
		public static void RaiseChangeTrickCountChanged(DataContext context, CombatCharacter character, int addValue, bool bySelectChangeTrick)
		{
			Events.OnChangeTrickCountChanged handlersChangeTrickCountChanged = Events._handlersChangeTrickCountChanged;
			if (handlersChangeTrickCountChanged != null)
			{
				handlersChangeTrickCountChanged(context, character, addValue, bySelectChangeTrick);
			}
		}

		// Token: 0x06008044 RID: 32836 RVA: 0x004CEC53 File Offset: 0x004CCE53
		public static void RegisterHandler_UnlockAttack(Events.OnUnlockAttack handler)
		{
			Events._handlersUnlockAttack = (Events.OnUnlockAttack)Delegate.Combine(Events._handlersUnlockAttack, handler);
		}

		// Token: 0x06008045 RID: 32837 RVA: 0x004CEC6B File Offset: 0x004CCE6B
		public static void UnRegisterHandler_UnlockAttack(Events.OnUnlockAttack handler)
		{
			Events._handlersUnlockAttack = (Events.OnUnlockAttack)Delegate.Remove(Events._handlersUnlockAttack, handler);
		}

		// Token: 0x06008046 RID: 32838 RVA: 0x004CEC83 File Offset: 0x004CCE83
		public static void RaiseUnlockAttack(DataContext context, CombatCharacter combatChar, int weaponIndex)
		{
			Events.OnUnlockAttack handlersUnlockAttack = Events._handlersUnlockAttack;
			if (handlersUnlockAttack != null)
			{
				handlersUnlockAttack(context, combatChar, weaponIndex);
			}
		}

		// Token: 0x06008047 RID: 32839 RVA: 0x004CEC9A File Offset: 0x004CCE9A
		public static void RegisterHandler_UnlockAttackEnd(Events.OnUnlockAttackEnd handler)
		{
			Events._handlersUnlockAttackEnd = (Events.OnUnlockAttackEnd)Delegate.Combine(Events._handlersUnlockAttackEnd, handler);
		}

		// Token: 0x06008048 RID: 32840 RVA: 0x004CECB2 File Offset: 0x004CCEB2
		public static void UnRegisterHandler_UnlockAttackEnd(Events.OnUnlockAttackEnd handler)
		{
			Events._handlersUnlockAttackEnd = (Events.OnUnlockAttackEnd)Delegate.Remove(Events._handlersUnlockAttackEnd, handler);
		}

		// Token: 0x06008049 RID: 32841 RVA: 0x004CECCA File Offset: 0x004CCECA
		public static void RaiseUnlockAttackEnd(DataContext context, CombatCharacter attacker)
		{
			Events.OnUnlockAttackEnd handlersUnlockAttackEnd = Events._handlersUnlockAttackEnd;
			if (handlersUnlockAttackEnd != null)
			{
				handlersUnlockAttackEnd(context, attacker);
			}
		}

		// Token: 0x0600804A RID: 32842 RVA: 0x004CECE0 File Offset: 0x004CCEE0
		public static void RegisterHandler_NormalAttackPrepareEnd(Events.OnNormalAttackPrepareEnd handler)
		{
			Events._handlersNormalAttackPrepareEnd = (Events.OnNormalAttackPrepareEnd)Delegate.Combine(Events._handlersNormalAttackPrepareEnd, handler);
		}

		// Token: 0x0600804B RID: 32843 RVA: 0x004CECF8 File Offset: 0x004CCEF8
		public static void UnRegisterHandler_NormalAttackPrepareEnd(Events.OnNormalAttackPrepareEnd handler)
		{
			Events._handlersNormalAttackPrepareEnd = (Events.OnNormalAttackPrepareEnd)Delegate.Remove(Events._handlersNormalAttackPrepareEnd, handler);
		}

		// Token: 0x0600804C RID: 32844 RVA: 0x004CED10 File Offset: 0x004CCF10
		public static void RaiseNormalAttackPrepareEnd(DataContext context, int charId, bool isAlly)
		{
			Events.OnNormalAttackPrepareEnd handlersNormalAttackPrepareEnd = Events._handlersNormalAttackPrepareEnd;
			if (handlersNormalAttackPrepareEnd != null)
			{
				handlersNormalAttackPrepareEnd(context, charId, isAlly);
			}
		}

		// Token: 0x0600804D RID: 32845 RVA: 0x004CED27 File Offset: 0x004CCF27
		public static void RegisterHandler_NormalAttackOutOfRange(Events.OnNormalAttackOutOfRange handler)
		{
			Events._handlersNormalAttackOutOfRange = (Events.OnNormalAttackOutOfRange)Delegate.Combine(Events._handlersNormalAttackOutOfRange, handler);
		}

		// Token: 0x0600804E RID: 32846 RVA: 0x004CED3F File Offset: 0x004CCF3F
		public static void UnRegisterHandler_NormalAttackOutOfRange(Events.OnNormalAttackOutOfRange handler)
		{
			Events._handlersNormalAttackOutOfRange = (Events.OnNormalAttackOutOfRange)Delegate.Remove(Events._handlersNormalAttackOutOfRange, handler);
		}

		// Token: 0x0600804F RID: 32847 RVA: 0x004CED57 File Offset: 0x004CCF57
		public static void RaiseNormalAttackOutOfRange(DataContext context, int charId, bool isAlly)
		{
			Events.OnNormalAttackOutOfRange handlersNormalAttackOutOfRange = Events._handlersNormalAttackOutOfRange;
			if (handlersNormalAttackOutOfRange != null)
			{
				handlersNormalAttackOutOfRange(context, charId, isAlly);
			}
		}

		// Token: 0x06008050 RID: 32848 RVA: 0x004CED6E File Offset: 0x004CCF6E
		public static void RegisterHandler_NormalAttackBegin(Events.OnNormalAttackBegin handler)
		{
			Events._handlersNormalAttackBegin = (Events.OnNormalAttackBegin)Delegate.Combine(Events._handlersNormalAttackBegin, handler);
		}

		// Token: 0x06008051 RID: 32849 RVA: 0x004CED86 File Offset: 0x004CCF86
		public static void UnRegisterHandler_NormalAttackBegin(Events.OnNormalAttackBegin handler)
		{
			Events._handlersNormalAttackBegin = (Events.OnNormalAttackBegin)Delegate.Remove(Events._handlersNormalAttackBegin, handler);
		}

		// Token: 0x06008052 RID: 32850 RVA: 0x004CED9E File Offset: 0x004CCF9E
		public static void RaiseNormalAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex)
		{
			Events.OnNormalAttackBegin handlersNormalAttackBegin = Events._handlersNormalAttackBegin;
			if (handlersNormalAttackBegin != null)
			{
				handlersNormalAttackBegin(context, attacker, defender, trickType, pursueIndex);
			}
		}

		// Token: 0x06008053 RID: 32851 RVA: 0x004CEDB8 File Offset: 0x004CCFB8
		public static void RegisterHandler_NormalAttackCalcHitEnd(Events.OnNormalAttackCalcHitEnd handler)
		{
			Events._handlersNormalAttackCalcHitEnd = (Events.OnNormalAttackCalcHitEnd)Delegate.Combine(Events._handlersNormalAttackCalcHitEnd, handler);
		}

		// Token: 0x06008054 RID: 32852 RVA: 0x004CEDD0 File Offset: 0x004CCFD0
		public static void UnRegisterHandler_NormalAttackCalcHitEnd(Events.OnNormalAttackCalcHitEnd handler)
		{
			Events._handlersNormalAttackCalcHitEnd = (Events.OnNormalAttackCalcHitEnd)Delegate.Remove(Events._handlersNormalAttackCalcHitEnd, handler);
		}

		// Token: 0x06008055 RID: 32853 RVA: 0x004CEDE8 File Offset: 0x004CCFE8
		public static void RaiseNormalAttackCalcHitEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, int pursueIndex, bool hit, bool isFightBack, bool isMind)
		{
			Events.OnNormalAttackCalcHitEnd handlersNormalAttackCalcHitEnd = Events._handlersNormalAttackCalcHitEnd;
			if (handlersNormalAttackCalcHitEnd != null)
			{
				handlersNormalAttackCalcHitEnd(context, attacker, defender, pursueIndex, hit, isFightBack, isMind);
			}
		}

		// Token: 0x06008056 RID: 32854 RVA: 0x004CEE06 File Offset: 0x004CD006
		public static void RegisterHandler_NormalAttackCalcCriticalEnd(Events.OnNormalAttackCalcCriticalEnd handler)
		{
			Events._handlersNormalAttackCalcCriticalEnd = (Events.OnNormalAttackCalcCriticalEnd)Delegate.Combine(Events._handlersNormalAttackCalcCriticalEnd, handler);
		}

		// Token: 0x06008057 RID: 32855 RVA: 0x004CEE1E File Offset: 0x004CD01E
		public static void UnRegisterHandler_NormalAttackCalcCriticalEnd(Events.OnNormalAttackCalcCriticalEnd handler)
		{
			Events._handlersNormalAttackCalcCriticalEnd = (Events.OnNormalAttackCalcCriticalEnd)Delegate.Remove(Events._handlersNormalAttackCalcCriticalEnd, handler);
		}

		// Token: 0x06008058 RID: 32856 RVA: 0x004CEE36 File Offset: 0x004CD036
		public static void RaiseNormalAttackCalcCriticalEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, bool critical)
		{
			Events.OnNormalAttackCalcCriticalEnd handlersNormalAttackCalcCriticalEnd = Events._handlersNormalAttackCalcCriticalEnd;
			if (handlersNormalAttackCalcCriticalEnd != null)
			{
				handlersNormalAttackCalcCriticalEnd(context, attacker, defender, critical);
			}
		}

		// Token: 0x06008059 RID: 32857 RVA: 0x004CEE4E File Offset: 0x004CD04E
		public static void RegisterHandler_NormalAttackEnd(Events.OnNormalAttackEnd handler)
		{
			Events._handlersNormalAttackEnd = (Events.OnNormalAttackEnd)Delegate.Combine(Events._handlersNormalAttackEnd, handler);
		}

		// Token: 0x0600805A RID: 32858 RVA: 0x004CEE66 File Offset: 0x004CD066
		public static void UnRegisterHandler_NormalAttackEnd(Events.OnNormalAttackEnd handler)
		{
			Events._handlersNormalAttackEnd = (Events.OnNormalAttackEnd)Delegate.Remove(Events._handlersNormalAttackEnd, handler);
		}

		// Token: 0x0600805B RID: 32859 RVA: 0x004CEE7E File Offset: 0x004CD07E
		public static void RaiseNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			Events.OnNormalAttackEnd handlersNormalAttackEnd = Events._handlersNormalAttackEnd;
			if (handlersNormalAttackEnd != null)
			{
				handlersNormalAttackEnd(context, attacker, defender, trickType, pursueIndex, hit, isFightBack);
			}
		}

		// Token: 0x0600805C RID: 32860 RVA: 0x004CEE9C File Offset: 0x004CD09C
		public static void RegisterHandler_NormalAttackAllEnd(Events.OnNormalAttackAllEnd handler)
		{
			Events._handlersNormalAttackAllEnd = (Events.OnNormalAttackAllEnd)Delegate.Combine(Events._handlersNormalAttackAllEnd, handler);
		}

		// Token: 0x0600805D RID: 32861 RVA: 0x004CEEB4 File Offset: 0x004CD0B4
		public static void UnRegisterHandler_NormalAttackAllEnd(Events.OnNormalAttackAllEnd handler)
		{
			Events._handlersNormalAttackAllEnd = (Events.OnNormalAttackAllEnd)Delegate.Remove(Events._handlersNormalAttackAllEnd, handler);
		}

		// Token: 0x0600805E RID: 32862 RVA: 0x004CEECC File Offset: 0x004CD0CC
		public static void RaiseNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			Events.OnNormalAttackAllEnd handlersNormalAttackAllEnd = Events._handlersNormalAttackAllEnd;
			if (handlersNormalAttackAllEnd != null)
			{
				handlersNormalAttackAllEnd(context, attacker, defender);
			}
		}

		// Token: 0x0600805F RID: 32863 RVA: 0x004CEEE3 File Offset: 0x004CD0E3
		public static void RegisterHandler_CastSkillUseExtraBreathOrStance(Events.OnCastSkillUseExtraBreathOrStance handler)
		{
			Events._handlersCastSkillUseExtraBreathOrStance = (Events.OnCastSkillUseExtraBreathOrStance)Delegate.Combine(Events._handlersCastSkillUseExtraBreathOrStance, handler);
		}

		// Token: 0x06008060 RID: 32864 RVA: 0x004CEEFB File Offset: 0x004CD0FB
		public static void UnRegisterHandler_CastSkillUseExtraBreathOrStance(Events.OnCastSkillUseExtraBreathOrStance handler)
		{
			Events._handlersCastSkillUseExtraBreathOrStance = (Events.OnCastSkillUseExtraBreathOrStance)Delegate.Remove(Events._handlersCastSkillUseExtraBreathOrStance, handler);
		}

		// Token: 0x06008061 RID: 32865 RVA: 0x004CEF13 File Offset: 0x004CD113
		public static void RaiseCastSkillUseExtraBreathOrStance(DataContext context, int charId, short skillId, int extraBreath, int extraStance)
		{
			Events.OnCastSkillUseExtraBreathOrStance handlersCastSkillUseExtraBreathOrStance = Events._handlersCastSkillUseExtraBreathOrStance;
			if (handlersCastSkillUseExtraBreathOrStance != null)
			{
				handlersCastSkillUseExtraBreathOrStance(context, charId, skillId, extraBreath, extraStance);
			}
		}

		// Token: 0x06008062 RID: 32866 RVA: 0x004CEF2D File Offset: 0x004CD12D
		public static void RegisterHandler_CastSkillUseMobilityAsBreathOrStance(Events.OnCastSkillUseMobilityAsBreathOrStance handler)
		{
			Events._handlersCastSkillUseMobilityAsBreathOrStance = (Events.OnCastSkillUseMobilityAsBreathOrStance)Delegate.Combine(Events._handlersCastSkillUseMobilityAsBreathOrStance, handler);
		}

		// Token: 0x06008063 RID: 32867 RVA: 0x004CEF45 File Offset: 0x004CD145
		public static void UnRegisterHandler_CastSkillUseMobilityAsBreathOrStance(Events.OnCastSkillUseMobilityAsBreathOrStance handler)
		{
			Events._handlersCastSkillUseMobilityAsBreathOrStance = (Events.OnCastSkillUseMobilityAsBreathOrStance)Delegate.Remove(Events._handlersCastSkillUseMobilityAsBreathOrStance, handler);
		}

		// Token: 0x06008064 RID: 32868 RVA: 0x004CEF5D File Offset: 0x004CD15D
		public static void RaiseCastSkillUseMobilityAsBreathOrStance(DataContext context, int charId, short skillId, bool asBreath)
		{
			Events.OnCastSkillUseMobilityAsBreathOrStance handlersCastSkillUseMobilityAsBreathOrStance = Events._handlersCastSkillUseMobilityAsBreathOrStance;
			if (handlersCastSkillUseMobilityAsBreathOrStance != null)
			{
				handlersCastSkillUseMobilityAsBreathOrStance(context, charId, skillId, asBreath);
			}
		}

		// Token: 0x06008065 RID: 32869 RVA: 0x004CEF75 File Offset: 0x004CD175
		public static void RegisterHandler_CastLegSkillWithAgile(Events.OnCastLegSkillWithAgile handler)
		{
			Events._handlersCastLegSkillWithAgile = (Events.OnCastLegSkillWithAgile)Delegate.Combine(Events._handlersCastLegSkillWithAgile, handler);
		}

		// Token: 0x06008066 RID: 32870 RVA: 0x004CEF8D File Offset: 0x004CD18D
		public static void UnRegisterHandler_CastLegSkillWithAgile(Events.OnCastLegSkillWithAgile handler)
		{
			Events._handlersCastLegSkillWithAgile = (Events.OnCastLegSkillWithAgile)Delegate.Remove(Events._handlersCastLegSkillWithAgile, handler);
		}

		// Token: 0x06008067 RID: 32871 RVA: 0x004CEFA5 File Offset: 0x004CD1A5
		public static void RaiseCastLegSkillWithAgile(DataContext context, CombatCharacter combatChar, short legSkillId)
		{
			Events.OnCastLegSkillWithAgile handlersCastLegSkillWithAgile = Events._handlersCastLegSkillWithAgile;
			if (handlersCastLegSkillWithAgile != null)
			{
				handlersCastLegSkillWithAgile(context, combatChar, legSkillId);
			}
		}

		// Token: 0x06008068 RID: 32872 RVA: 0x004CEFBC File Offset: 0x004CD1BC
		public static void RegisterHandler_CastSkillOnLackBreathStance(Events.OnCastSkillOnLackBreathStance handler)
		{
			Events._handlersCastSkillOnLackBreathStance = (Events.OnCastSkillOnLackBreathStance)Delegate.Combine(Events._handlersCastSkillOnLackBreathStance, handler);
		}

		// Token: 0x06008069 RID: 32873 RVA: 0x004CEFD4 File Offset: 0x004CD1D4
		public static void UnRegisterHandler_CastSkillOnLackBreathStance(Events.OnCastSkillOnLackBreathStance handler)
		{
			Events._handlersCastSkillOnLackBreathStance = (Events.OnCastSkillOnLackBreathStance)Delegate.Remove(Events._handlersCastSkillOnLackBreathStance, handler);
		}

		// Token: 0x0600806A RID: 32874 RVA: 0x004CEFEC File Offset: 0x004CD1EC
		public static void RaiseCastSkillOnLackBreathStance(DataContext context, CombatCharacter combatChar, short skillId, int lackBreath, int lackStance, int costBreath, int costStance)
		{
			Events.OnCastSkillOnLackBreathStance handlersCastSkillOnLackBreathStance = Events._handlersCastSkillOnLackBreathStance;
			if (handlersCastSkillOnLackBreathStance != null)
			{
				handlersCastSkillOnLackBreathStance(context, combatChar, skillId, lackBreath, lackStance, costBreath, costStance);
			}
		}

		// Token: 0x0600806B RID: 32875 RVA: 0x004CF00A File Offset: 0x004CD20A
		public static void RegisterHandler_CastSkillTrickCosted(Events.OnCastSkillTrickCosted handler)
		{
			Events._handlersCastSkillTrickCosted = (Events.OnCastSkillTrickCosted)Delegate.Combine(Events._handlersCastSkillTrickCosted, handler);
		}

		// Token: 0x0600806C RID: 32876 RVA: 0x004CF022 File Offset: 0x004CD222
		public static void UnRegisterHandler_CastSkillTrickCosted(Events.OnCastSkillTrickCosted handler)
		{
			Events._handlersCastSkillTrickCosted = (Events.OnCastSkillTrickCosted)Delegate.Remove(Events._handlersCastSkillTrickCosted, handler);
		}

		// Token: 0x0600806D RID: 32877 RVA: 0x004CF03A File Offset: 0x004CD23A
		public static void RaiseCastSkillTrickCosted(DataContext context, CombatCharacter combatChar, short skillId, List<NeedTrick> costTricks)
		{
			Events.OnCastSkillTrickCosted handlersCastSkillTrickCosted = Events._handlersCastSkillTrickCosted;
			if (handlersCastSkillTrickCosted != null)
			{
				handlersCastSkillTrickCosted(context, combatChar, skillId, costTricks);
			}
		}

		// Token: 0x0600806E RID: 32878 RVA: 0x004CF052 File Offset: 0x004CD252
		public static void RegisterHandler_JiTrickInsteadCostTricks(Events.OnJiTrickInsteadCostTricks handler)
		{
			Events._handlersJiTrickInsteadCostTricks = (Events.OnJiTrickInsteadCostTricks)Delegate.Combine(Events._handlersJiTrickInsteadCostTricks, handler);
		}

		// Token: 0x0600806F RID: 32879 RVA: 0x004CF06A File Offset: 0x004CD26A
		public static void UnRegisterHandler_JiTrickInsteadCostTricks(Events.OnJiTrickInsteadCostTricks handler)
		{
			Events._handlersJiTrickInsteadCostTricks = (Events.OnJiTrickInsteadCostTricks)Delegate.Remove(Events._handlersJiTrickInsteadCostTricks, handler);
		}

		// Token: 0x06008070 RID: 32880 RVA: 0x004CF082 File Offset: 0x004CD282
		public static void RaiseJiTrickInsteadCostTricks(DataContext context, CombatCharacter character, int count)
		{
			Events.OnJiTrickInsteadCostTricks handlersJiTrickInsteadCostTricks = Events._handlersJiTrickInsteadCostTricks;
			if (handlersJiTrickInsteadCostTricks != null)
			{
				handlersJiTrickInsteadCostTricks(context, character, count);
			}
		}

		// Token: 0x06008071 RID: 32881 RVA: 0x004CF099 File Offset: 0x004CD299
		public static void RegisterHandler_UselessTrickInsteadJiTricks(Events.OnUselessTrickInsteadJiTricks handler)
		{
			Events._handlersUselessTrickInsteadJiTricks = (Events.OnUselessTrickInsteadJiTricks)Delegate.Combine(Events._handlersUselessTrickInsteadJiTricks, handler);
		}

		// Token: 0x06008072 RID: 32882 RVA: 0x004CF0B1 File Offset: 0x004CD2B1
		public static void UnRegisterHandler_UselessTrickInsteadJiTricks(Events.OnUselessTrickInsteadJiTricks handler)
		{
			Events._handlersUselessTrickInsteadJiTricks = (Events.OnUselessTrickInsteadJiTricks)Delegate.Remove(Events._handlersUselessTrickInsteadJiTricks, handler);
		}

		// Token: 0x06008073 RID: 32883 RVA: 0x004CF0C9 File Offset: 0x004CD2C9
		public static void RaiseUselessTrickInsteadJiTricks(DataContext context, CombatCharacter character, int count)
		{
			Events.OnUselessTrickInsteadJiTricks handlersUselessTrickInsteadJiTricks = Events._handlersUselessTrickInsteadJiTricks;
			if (handlersUselessTrickInsteadJiTricks != null)
			{
				handlersUselessTrickInsteadJiTricks(context, character, count);
			}
		}

		// Token: 0x06008074 RID: 32884 RVA: 0x004CF0E0 File Offset: 0x004CD2E0
		public static void RegisterHandler_ShaTrickInsteadCostTricks(Events.OnShaTrickInsteadCostTricks handler)
		{
			Events._handlersShaTrickInsteadCostTricks = (Events.OnShaTrickInsteadCostTricks)Delegate.Combine(Events._handlersShaTrickInsteadCostTricks, handler);
		}

		// Token: 0x06008075 RID: 32885 RVA: 0x004CF0F8 File Offset: 0x004CD2F8
		public static void UnRegisterHandler_ShaTrickInsteadCostTricks(Events.OnShaTrickInsteadCostTricks handler)
		{
			Events._handlersShaTrickInsteadCostTricks = (Events.OnShaTrickInsteadCostTricks)Delegate.Remove(Events._handlersShaTrickInsteadCostTricks, handler);
		}

		// Token: 0x06008076 RID: 32886 RVA: 0x004CF110 File Offset: 0x004CD310
		public static void RaiseShaTrickInsteadCostTricks(DataContext context, CombatCharacter character, short skillId)
		{
			Events.OnShaTrickInsteadCostTricks handlersShaTrickInsteadCostTricks = Events._handlersShaTrickInsteadCostTricks;
			if (handlersShaTrickInsteadCostTricks != null)
			{
				handlersShaTrickInsteadCostTricks(context, character, skillId);
			}
		}

		// Token: 0x06008077 RID: 32887 RVA: 0x004CF127 File Offset: 0x004CD327
		public static void RegisterHandler_CastSkillCosted(Events.OnCastSkillCosted handler)
		{
			Events._handlersCastSkillCosted = (Events.OnCastSkillCosted)Delegate.Combine(Events._handlersCastSkillCosted, handler);
		}

		// Token: 0x06008078 RID: 32888 RVA: 0x004CF13F File Offset: 0x004CD33F
		public static void UnRegisterHandler_CastSkillCosted(Events.OnCastSkillCosted handler)
		{
			Events._handlersCastSkillCosted = (Events.OnCastSkillCosted)Delegate.Remove(Events._handlersCastSkillCosted, handler);
		}

		// Token: 0x06008079 RID: 32889 RVA: 0x004CF157 File Offset: 0x004CD357
		public static void RaiseCastSkillCosted(DataContext context, CombatCharacter combatChar, short skillId)
		{
			Events.OnCastSkillCosted handlersCastSkillCosted = Events._handlersCastSkillCosted;
			if (handlersCastSkillCosted != null)
			{
				handlersCastSkillCosted(context, combatChar, skillId);
			}
		}

		// Token: 0x0600807A RID: 32890 RVA: 0x004CF16E File Offset: 0x004CD36E
		public static void RegisterHandler_ChangePreparingSkillBegin(Events.OnChangePreparingSkillBegin handler)
		{
			Events._handlersChangePreparingSkillBegin = (Events.OnChangePreparingSkillBegin)Delegate.Combine(Events._handlersChangePreparingSkillBegin, handler);
		}

		// Token: 0x0600807B RID: 32891 RVA: 0x004CF186 File Offset: 0x004CD386
		public static void UnRegisterHandler_ChangePreparingSkillBegin(Events.OnChangePreparingSkillBegin handler)
		{
			Events._handlersChangePreparingSkillBegin = (Events.OnChangePreparingSkillBegin)Delegate.Remove(Events._handlersChangePreparingSkillBegin, handler);
		}

		// Token: 0x0600807C RID: 32892 RVA: 0x004CF19E File Offset: 0x004CD39E
		public static void RaiseChangePreparingSkillBegin(DataContext context, int charId, short prevSkillId, short currSkillId)
		{
			Events.OnChangePreparingSkillBegin handlersChangePreparingSkillBegin = Events._handlersChangePreparingSkillBegin;
			if (handlersChangePreparingSkillBegin != null)
			{
				handlersChangePreparingSkillBegin(context, charId, prevSkillId, currSkillId);
			}
		}

		// Token: 0x0600807D RID: 32893 RVA: 0x004CF1B6 File Offset: 0x004CD3B6
		public static void RegisterHandler_CastAgileOrDefenseWithoutPrepareBegin(Events.OnCastAgileOrDefenseWithoutPrepareBegin handler)
		{
			Events._handlersCastAgileOrDefenseWithoutPrepareBegin = (Events.OnCastAgileOrDefenseWithoutPrepareBegin)Delegate.Combine(Events._handlersCastAgileOrDefenseWithoutPrepareBegin, handler);
		}

		// Token: 0x0600807E RID: 32894 RVA: 0x004CF1CE File Offset: 0x004CD3CE
		public static void UnRegisterHandler_CastAgileOrDefenseWithoutPrepareBegin(Events.OnCastAgileOrDefenseWithoutPrepareBegin handler)
		{
			Events._handlersCastAgileOrDefenseWithoutPrepareBegin = (Events.OnCastAgileOrDefenseWithoutPrepareBegin)Delegate.Remove(Events._handlersCastAgileOrDefenseWithoutPrepareBegin, handler);
		}

		// Token: 0x0600807F RID: 32895 RVA: 0x004CF1E6 File Offset: 0x004CD3E6
		public static void RaiseCastAgileOrDefenseWithoutPrepareBegin(DataContext context, int charId, short skillId)
		{
			Events.OnCastAgileOrDefenseWithoutPrepareBegin handlersCastAgileOrDefenseWithoutPrepareBegin = Events._handlersCastAgileOrDefenseWithoutPrepareBegin;
			if (handlersCastAgileOrDefenseWithoutPrepareBegin != null)
			{
				handlersCastAgileOrDefenseWithoutPrepareBegin(context, charId, skillId);
			}
		}

		// Token: 0x06008080 RID: 32896 RVA: 0x004CF1FD File Offset: 0x004CD3FD
		public static void RegisterHandler_CastAgileOrDefenseWithoutPrepareEnd(Events.OnCastAgileOrDefenseWithoutPrepareEnd handler)
		{
			Events._handlersCastAgileOrDefenseWithoutPrepareEnd = (Events.OnCastAgileOrDefenseWithoutPrepareEnd)Delegate.Combine(Events._handlersCastAgileOrDefenseWithoutPrepareEnd, handler);
		}

		// Token: 0x06008081 RID: 32897 RVA: 0x004CF215 File Offset: 0x004CD415
		public static void UnRegisterHandler_CastAgileOrDefenseWithoutPrepareEnd(Events.OnCastAgileOrDefenseWithoutPrepareEnd handler)
		{
			Events._handlersCastAgileOrDefenseWithoutPrepareEnd = (Events.OnCastAgileOrDefenseWithoutPrepareEnd)Delegate.Remove(Events._handlersCastAgileOrDefenseWithoutPrepareEnd, handler);
		}

		// Token: 0x06008082 RID: 32898 RVA: 0x004CF22D File Offset: 0x004CD42D
		public static void RaiseCastAgileOrDefenseWithoutPrepareEnd(DataContext context, int charId, short skillId)
		{
			Events.OnCastAgileOrDefenseWithoutPrepareEnd handlersCastAgileOrDefenseWithoutPrepareEnd = Events._handlersCastAgileOrDefenseWithoutPrepareEnd;
			if (handlersCastAgileOrDefenseWithoutPrepareEnd != null)
			{
				handlersCastAgileOrDefenseWithoutPrepareEnd(context, charId, skillId);
			}
		}

		// Token: 0x06008083 RID: 32899 RVA: 0x004CF244 File Offset: 0x004CD444
		public static void RegisterHandler_PrepareSkillEffectNotYetCreated(Events.OnPrepareSkillEffectNotYetCreated handler)
		{
			Events._handlersPrepareSkillEffectNotYetCreated = (Events.OnPrepareSkillEffectNotYetCreated)Delegate.Combine(Events._handlersPrepareSkillEffectNotYetCreated, handler);
		}

		// Token: 0x06008084 RID: 32900 RVA: 0x004CF25C File Offset: 0x004CD45C
		public static void UnRegisterHandler_PrepareSkillEffectNotYetCreated(Events.OnPrepareSkillEffectNotYetCreated handler)
		{
			Events._handlersPrepareSkillEffectNotYetCreated = (Events.OnPrepareSkillEffectNotYetCreated)Delegate.Remove(Events._handlersPrepareSkillEffectNotYetCreated, handler);
		}

		// Token: 0x06008085 RID: 32901 RVA: 0x004CF274 File Offset: 0x004CD474
		public static void RaisePrepareSkillEffectNotYetCreated(DataContext context, CombatCharacter character, short skillId)
		{
			Events.OnPrepareSkillEffectNotYetCreated handlersPrepareSkillEffectNotYetCreated = Events._handlersPrepareSkillEffectNotYetCreated;
			if (handlersPrepareSkillEffectNotYetCreated != null)
			{
				handlersPrepareSkillEffectNotYetCreated(context, character, skillId);
			}
		}

		// Token: 0x06008086 RID: 32902 RVA: 0x004CF28B File Offset: 0x004CD48B
		public static void RegisterHandler_PrepareSkillBegin(Events.OnPrepareSkillBegin handler)
		{
			Events._handlersPrepareSkillBegin = (Events.OnPrepareSkillBegin)Delegate.Combine(Events._handlersPrepareSkillBegin, handler);
		}

		// Token: 0x06008087 RID: 32903 RVA: 0x004CF2A3 File Offset: 0x004CD4A3
		public static void UnRegisterHandler_PrepareSkillBegin(Events.OnPrepareSkillBegin handler)
		{
			Events._handlersPrepareSkillBegin = (Events.OnPrepareSkillBegin)Delegate.Remove(Events._handlersPrepareSkillBegin, handler);
		}

		// Token: 0x06008088 RID: 32904 RVA: 0x004CF2BB File Offset: 0x004CD4BB
		public static void RaisePrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			Events.OnPrepareSkillBegin handlersPrepareSkillBegin = Events._handlersPrepareSkillBegin;
			if (handlersPrepareSkillBegin != null)
			{
				handlersPrepareSkillBegin(context, charId, isAlly, skillId);
			}
		}

		// Token: 0x06008089 RID: 32905 RVA: 0x004CF2D3 File Offset: 0x004CD4D3
		public static void RegisterHandler_PrepareSkillProgressChange(Events.OnPrepareSkillProgressChange handler)
		{
			Events._handlersPrepareSkillProgressChange = (Events.OnPrepareSkillProgressChange)Delegate.Combine(Events._handlersPrepareSkillProgressChange, handler);
		}

		// Token: 0x0600808A RID: 32906 RVA: 0x004CF2EB File Offset: 0x004CD4EB
		public static void UnRegisterHandler_PrepareSkillProgressChange(Events.OnPrepareSkillProgressChange handler)
		{
			Events._handlersPrepareSkillProgressChange = (Events.OnPrepareSkillProgressChange)Delegate.Remove(Events._handlersPrepareSkillProgressChange, handler);
		}

		// Token: 0x0600808B RID: 32907 RVA: 0x004CF303 File Offset: 0x004CD503
		public static void RaisePrepareSkillProgressChange(DataContext context, int charId, bool isAlly, short skillId, sbyte preparePercent)
		{
			Events.OnPrepareSkillProgressChange handlersPrepareSkillProgressChange = Events._handlersPrepareSkillProgressChange;
			if (handlersPrepareSkillProgressChange != null)
			{
				handlersPrepareSkillProgressChange(context, charId, isAlly, skillId, preparePercent);
			}
		}

		// Token: 0x0600808C RID: 32908 RVA: 0x004CF31D File Offset: 0x004CD51D
		public static void RegisterHandler_PrepareSkillChangeDistance(Events.OnPrepareSkillChangeDistance handler)
		{
			Events._handlersPrepareSkillChangeDistance = (Events.OnPrepareSkillChangeDistance)Delegate.Combine(Events._handlersPrepareSkillChangeDistance, handler);
		}

		// Token: 0x0600808D RID: 32909 RVA: 0x004CF335 File Offset: 0x004CD535
		public static void UnRegisterHandler_PrepareSkillChangeDistance(Events.OnPrepareSkillChangeDistance handler)
		{
			Events._handlersPrepareSkillChangeDistance = (Events.OnPrepareSkillChangeDistance)Delegate.Remove(Events._handlersPrepareSkillChangeDistance, handler);
		}

		// Token: 0x0600808E RID: 32910 RVA: 0x004CF34D File Offset: 0x004CD54D
		public static void RaisePrepareSkillChangeDistance(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			Events.OnPrepareSkillChangeDistance handlersPrepareSkillChangeDistance = Events._handlersPrepareSkillChangeDistance;
			if (handlersPrepareSkillChangeDistance != null)
			{
				handlersPrepareSkillChangeDistance(context, attacker, defender, skillId);
			}
		}

		// Token: 0x0600808F RID: 32911 RVA: 0x004CF365 File Offset: 0x004CD565
		public static void RegisterHandler_PrepareSkillEnd(Events.OnPrepareSkillEnd handler)
		{
			Events._handlersPrepareSkillEnd = (Events.OnPrepareSkillEnd)Delegate.Combine(Events._handlersPrepareSkillEnd, handler);
		}

		// Token: 0x06008090 RID: 32912 RVA: 0x004CF37D File Offset: 0x004CD57D
		public static void UnRegisterHandler_PrepareSkillEnd(Events.OnPrepareSkillEnd handler)
		{
			Events._handlersPrepareSkillEnd = (Events.OnPrepareSkillEnd)Delegate.Remove(Events._handlersPrepareSkillEnd, handler);
		}

		// Token: 0x06008091 RID: 32913 RVA: 0x004CF395 File Offset: 0x004CD595
		public static void RaisePrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			Events.OnPrepareSkillEnd handlersPrepareSkillEnd = Events._handlersPrepareSkillEnd;
			if (handlersPrepareSkillEnd != null)
			{
				handlersPrepareSkillEnd(context, charId, isAlly, skillId);
			}
		}

		// Token: 0x06008092 RID: 32914 RVA: 0x004CF3AD File Offset: 0x004CD5AD
		public static void RegisterHandler_CastAttackSkillBegin(Events.OnCastAttackSkillBegin handler)
		{
			Events._handlersCastAttackSkillBegin = (Events.OnCastAttackSkillBegin)Delegate.Combine(Events._handlersCastAttackSkillBegin, handler);
		}

		// Token: 0x06008093 RID: 32915 RVA: 0x004CF3C5 File Offset: 0x004CD5C5
		public static void UnRegisterHandler_CastAttackSkillBegin(Events.OnCastAttackSkillBegin handler)
		{
			Events._handlersCastAttackSkillBegin = (Events.OnCastAttackSkillBegin)Delegate.Remove(Events._handlersCastAttackSkillBegin, handler);
		}

		// Token: 0x06008094 RID: 32916 RVA: 0x004CF3DD File Offset: 0x004CD5DD
		public static void RaiseCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			Events.OnCastAttackSkillBegin handlersCastAttackSkillBegin = Events._handlersCastAttackSkillBegin;
			if (handlersCastAttackSkillBegin != null)
			{
				handlersCastAttackSkillBegin(context, attacker, defender, skillId);
			}
		}

		// Token: 0x06008095 RID: 32917 RVA: 0x004CF3F5 File Offset: 0x004CD5F5
		public static void RegisterHandler_AttackSkillAttackBegin(Events.OnAttackSkillAttackBegin handler)
		{
			Events._handlersAttackSkillAttackBegin = (Events.OnAttackSkillAttackBegin)Delegate.Combine(Events._handlersAttackSkillAttackBegin, handler);
		}

		// Token: 0x06008096 RID: 32918 RVA: 0x004CF40D File Offset: 0x004CD60D
		public static void UnRegisterHandler_AttackSkillAttackBegin(Events.OnAttackSkillAttackBegin handler)
		{
			Events._handlersAttackSkillAttackBegin = (Events.OnAttackSkillAttackBegin)Delegate.Remove(Events._handlersAttackSkillAttackBegin, handler);
		}

		// Token: 0x06008097 RID: 32919 RVA: 0x004CF425 File Offset: 0x004CD625
		public static void RaiseAttackSkillAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId, int index, bool hit)
		{
			Events.OnAttackSkillAttackBegin handlersAttackSkillAttackBegin = Events._handlersAttackSkillAttackBegin;
			if (handlersAttackSkillAttackBegin != null)
			{
				handlersAttackSkillAttackBegin(context, attacker, defender, skillId, index, hit);
			}
		}

		// Token: 0x06008098 RID: 32920 RVA: 0x004CF441 File Offset: 0x004CD641
		public static void RegisterHandler_AttackSkillAttackHit(Events.OnAttackSkillAttackHit handler)
		{
			Events._handlersAttackSkillAttackHit = (Events.OnAttackSkillAttackHit)Delegate.Combine(Events._handlersAttackSkillAttackHit, handler);
		}

		// Token: 0x06008099 RID: 32921 RVA: 0x004CF459 File Offset: 0x004CD659
		public static void UnRegisterHandler_AttackSkillAttackHit(Events.OnAttackSkillAttackHit handler)
		{
			Events._handlersAttackSkillAttackHit = (Events.OnAttackSkillAttackHit)Delegate.Remove(Events._handlersAttackSkillAttackHit, handler);
		}

		// Token: 0x0600809A RID: 32922 RVA: 0x004CF471 File Offset: 0x004CD671
		public static void RaiseAttackSkillAttackHit(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId, int index, bool critical)
		{
			Events.OnAttackSkillAttackHit handlersAttackSkillAttackHit = Events._handlersAttackSkillAttackHit;
			if (handlersAttackSkillAttackHit != null)
			{
				handlersAttackSkillAttackHit(context, attacker, defender, skillId, index, critical);
			}
		}

		// Token: 0x0600809B RID: 32923 RVA: 0x004CF48D File Offset: 0x004CD68D
		public static void RegisterHandler_AttackSkillAttackEnd(Events.OnAttackSkillAttackEnd handler)
		{
			Events._handlersAttackSkillAttackEnd = (Events.OnAttackSkillAttackEnd)Delegate.Combine(Events._handlersAttackSkillAttackEnd, handler);
		}

		// Token: 0x0600809C RID: 32924 RVA: 0x004CF4A5 File Offset: 0x004CD6A5
		public static void UnRegisterHandler_AttackSkillAttackEnd(Events.OnAttackSkillAttackEnd handler)
		{
			Events._handlersAttackSkillAttackEnd = (Events.OnAttackSkillAttackEnd)Delegate.Remove(Events._handlersAttackSkillAttackEnd, handler);
		}

		// Token: 0x0600809D RID: 32925 RVA: 0x004CF4BD File Offset: 0x004CD6BD
		public static void RaiseAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			Events.OnAttackSkillAttackEnd handlersAttackSkillAttackEnd = Events._handlersAttackSkillAttackEnd;
			if (handlersAttackSkillAttackEnd != null)
			{
				handlersAttackSkillAttackEnd(context, hitType, hit, index);
			}
		}

		// Token: 0x0600809E RID: 32926 RVA: 0x004CF4D5 File Offset: 0x004CD6D5
		public static void RegisterHandler_AttackSkillAttackEndOfAll(Events.OnAttackSkillAttackEndOfAll handler)
		{
			Events._handlersAttackSkillAttackEndOfAll = (Events.OnAttackSkillAttackEndOfAll)Delegate.Combine(Events._handlersAttackSkillAttackEndOfAll, handler);
		}

		// Token: 0x0600809F RID: 32927 RVA: 0x004CF4ED File Offset: 0x004CD6ED
		public static void UnRegisterHandler_AttackSkillAttackEndOfAll(Events.OnAttackSkillAttackEndOfAll handler)
		{
			Events._handlersAttackSkillAttackEndOfAll = (Events.OnAttackSkillAttackEndOfAll)Delegate.Remove(Events._handlersAttackSkillAttackEndOfAll, handler);
		}

		// Token: 0x060080A0 RID: 32928 RVA: 0x004CF505 File Offset: 0x004CD705
		public static void RaiseAttackSkillAttackEndOfAll(DataContext context, CombatCharacter character, int index)
		{
			Events.OnAttackSkillAttackEndOfAll handlersAttackSkillAttackEndOfAll = Events._handlersAttackSkillAttackEndOfAll;
			if (handlersAttackSkillAttackEndOfAll != null)
			{
				handlersAttackSkillAttackEndOfAll(context, character, index);
			}
		}

		// Token: 0x060080A1 RID: 32929 RVA: 0x004CF51C File Offset: 0x004CD71C
		public static void RegisterHandler_CastSkillEnd(Events.OnCastSkillEnd handler)
		{
			Events._handlersCastSkillEnd = (Events.OnCastSkillEnd)Delegate.Combine(Events._handlersCastSkillEnd, handler);
		}

		// Token: 0x060080A2 RID: 32930 RVA: 0x004CF534 File Offset: 0x004CD734
		public static void UnRegisterHandler_CastSkillEnd(Events.OnCastSkillEnd handler)
		{
			Events._handlersCastSkillEnd = (Events.OnCastSkillEnd)Delegate.Remove(Events._handlersCastSkillEnd, handler);
		}

		// Token: 0x060080A3 RID: 32931 RVA: 0x004CF54C File Offset: 0x004CD74C
		public static void RaiseCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			Events.OnCastSkillEnd handlersCastSkillEnd = Events._handlersCastSkillEnd;
			if (handlersCastSkillEnd != null)
			{
				handlersCastSkillEnd(context, charId, isAlly, skillId, power, interrupted);
			}
		}

		// Token: 0x060080A4 RID: 32932 RVA: 0x004CF568 File Offset: 0x004CD768
		public static void RegisterHandler_CastSkillAllEnd(Events.OnCastSkillAllEnd handler)
		{
			Events._handlersCastSkillAllEnd = (Events.OnCastSkillAllEnd)Delegate.Combine(Events._handlersCastSkillAllEnd, handler);
		}

		// Token: 0x060080A5 RID: 32933 RVA: 0x004CF580 File Offset: 0x004CD780
		public static void UnRegisterHandler_CastSkillAllEnd(Events.OnCastSkillAllEnd handler)
		{
			Events._handlersCastSkillAllEnd = (Events.OnCastSkillAllEnd)Delegate.Remove(Events._handlersCastSkillAllEnd, handler);
		}

		// Token: 0x060080A6 RID: 32934 RVA: 0x004CF598 File Offset: 0x004CD798
		public static void RaiseCastSkillAllEnd(DataContext context, int charId, short skillId)
		{
			Events.OnCastSkillAllEnd handlersCastSkillAllEnd = Events._handlersCastSkillAllEnd;
			if (handlersCastSkillAllEnd != null)
			{
				handlersCastSkillAllEnd(context, charId, skillId);
			}
		}

		// Token: 0x060080A7 RID: 32935 RVA: 0x004CF5AF File Offset: 0x004CD7AF
		public static void RegisterHandler_CalcLeveragingValue(Events.OnCalcLeveragingValue handler)
		{
			Events._handlersCalcLeveragingValue = (Events.OnCalcLeveragingValue)Delegate.Combine(Events._handlersCalcLeveragingValue, handler);
		}

		// Token: 0x060080A8 RID: 32936 RVA: 0x004CF5C7 File Offset: 0x004CD7C7
		public static void UnRegisterHandler_CalcLeveragingValue(Events.OnCalcLeveragingValue handler)
		{
			Events._handlersCalcLeveragingValue = (Events.OnCalcLeveragingValue)Delegate.Remove(Events._handlersCalcLeveragingValue, handler);
		}

		// Token: 0x060080A9 RID: 32937 RVA: 0x004CF5DF File Offset: 0x004CD7DF
		public static void RaiseCalcLeveragingValue(CombatContext context, sbyte hitType, bool hit, int index)
		{
			Events.OnCalcLeveragingValue handlersCalcLeveragingValue = Events._handlersCalcLeveragingValue;
			if (handlersCalcLeveragingValue != null)
			{
				handlersCalcLeveragingValue(context, hitType, hit, index);
			}
		}

		// Token: 0x060080AA RID: 32938 RVA: 0x004CF5F7 File Offset: 0x004CD7F7
		public static void RegisterHandler_WisdomCosted(Events.OnWisdomCosted handler)
		{
			Events._handlersWisdomCosted = (Events.OnWisdomCosted)Delegate.Combine(Events._handlersWisdomCosted, handler);
		}

		// Token: 0x060080AB RID: 32939 RVA: 0x004CF60F File Offset: 0x004CD80F
		public static void UnRegisterHandler_WisdomCosted(Events.OnWisdomCosted handler)
		{
			Events._handlersWisdomCosted = (Events.OnWisdomCosted)Delegate.Remove(Events._handlersWisdomCosted, handler);
		}

		// Token: 0x060080AC RID: 32940 RVA: 0x004CF627 File Offset: 0x004CD827
		public static void RaiseWisdomCosted(DataContext context, bool isAlly, int value)
		{
			Events.OnWisdomCosted handlersWisdomCosted = Events._handlersWisdomCosted;
			if (handlersWisdomCosted != null)
			{
				handlersWisdomCosted(context, isAlly, value);
			}
		}

		// Token: 0x060080AD RID: 32941 RVA: 0x004CF63E File Offset: 0x004CD83E
		public static void RegisterHandler_HealedInjury(Events.OnHealedInjury handler)
		{
			Events._handlersHealedInjury = (Events.OnHealedInjury)Delegate.Combine(Events._handlersHealedInjury, handler);
		}

		// Token: 0x060080AE RID: 32942 RVA: 0x004CF656 File Offset: 0x004CD856
		public static void UnRegisterHandler_HealedInjury(Events.OnHealedInjury handler)
		{
			Events._handlersHealedInjury = (Events.OnHealedInjury)Delegate.Remove(Events._handlersHealedInjury, handler);
		}

		// Token: 0x060080AF RID: 32943 RVA: 0x004CF66E File Offset: 0x004CD86E
		public static void RaiseHealedInjury(DataContext context, int doctorId, int patientId, bool isAlly, sbyte healMarkCount)
		{
			Events.OnHealedInjury handlersHealedInjury = Events._handlersHealedInjury;
			if (handlersHealedInjury != null)
			{
				handlersHealedInjury(context, doctorId, patientId, isAlly, healMarkCount);
			}
		}

		// Token: 0x060080B0 RID: 32944 RVA: 0x004CF688 File Offset: 0x004CD888
		public static void RegisterHandler_HealedPoison(Events.OnHealedPoison handler)
		{
			Events._handlersHealedPoison = (Events.OnHealedPoison)Delegate.Combine(Events._handlersHealedPoison, handler);
		}

		// Token: 0x060080B1 RID: 32945 RVA: 0x004CF6A0 File Offset: 0x004CD8A0
		public static void UnRegisterHandler_HealedPoison(Events.OnHealedPoison handler)
		{
			Events._handlersHealedPoison = (Events.OnHealedPoison)Delegate.Remove(Events._handlersHealedPoison, handler);
		}

		// Token: 0x060080B2 RID: 32946 RVA: 0x004CF6B8 File Offset: 0x004CD8B8
		public static void RaiseHealedPoison(DataContext context, int doctorId, int patientId, bool isAlly, sbyte healMarkCount)
		{
			Events.OnHealedPoison handlersHealedPoison = Events._handlersHealedPoison;
			if (handlersHealedPoison != null)
			{
				handlersHealedPoison(context, doctorId, patientId, isAlly, healMarkCount);
			}
		}

		// Token: 0x060080B3 RID: 32947 RVA: 0x004CF6D2 File Offset: 0x004CD8D2
		public static void RegisterHandler_UsedMedicine(Events.OnUsedMedicine handler)
		{
			Events._handlersUsedMedicine = (Events.OnUsedMedicine)Delegate.Combine(Events._handlersUsedMedicine, handler);
		}

		// Token: 0x060080B4 RID: 32948 RVA: 0x004CF6EA File Offset: 0x004CD8EA
		public static void UnRegisterHandler_UsedMedicine(Events.OnUsedMedicine handler)
		{
			Events._handlersUsedMedicine = (Events.OnUsedMedicine)Delegate.Remove(Events._handlersUsedMedicine, handler);
		}

		// Token: 0x060080B5 RID: 32949 RVA: 0x004CF702 File Offset: 0x004CD902
		public static void RaiseUsedMedicine(DataContext context, int charId, ItemKey itemKey)
		{
			Events.OnUsedMedicine handlersUsedMedicine = Events._handlersUsedMedicine;
			if (handlersUsedMedicine != null)
			{
				handlersUsedMedicine(context, charId, itemKey);
			}
		}

		// Token: 0x060080B6 RID: 32950 RVA: 0x004CF719 File Offset: 0x004CD919
		public static void RegisterHandler_UsedCustomItem(Events.OnUsedCustomItem handler)
		{
			Events._handlersUsedCustomItem = (Events.OnUsedCustomItem)Delegate.Combine(Events._handlersUsedCustomItem, handler);
		}

		// Token: 0x060080B7 RID: 32951 RVA: 0x004CF731 File Offset: 0x004CD931
		public static void UnRegisterHandler_UsedCustomItem(Events.OnUsedCustomItem handler)
		{
			Events._handlersUsedCustomItem = (Events.OnUsedCustomItem)Delegate.Remove(Events._handlersUsedCustomItem, handler);
		}

		// Token: 0x060080B8 RID: 32952 RVA: 0x004CF749 File Offset: 0x004CD949
		public static void RaiseUsedCustomItem(DataContext context, int charId, ItemKey itemKey)
		{
			Events.OnUsedCustomItem handlersUsedCustomItem = Events._handlersUsedCustomItem;
			if (handlersUsedCustomItem != null)
			{
				handlersUsedCustomItem(context, charId, itemKey);
			}
		}

		// Token: 0x060080B9 RID: 32953 RVA: 0x004CF760 File Offset: 0x004CD960
		public static void RegisterHandler_InterruptOtherAction(Events.OnInterruptOtherAction handler)
		{
			Events._handlersInterruptOtherAction = (Events.OnInterruptOtherAction)Delegate.Combine(Events._handlersInterruptOtherAction, handler);
		}

		// Token: 0x060080BA RID: 32954 RVA: 0x004CF778 File Offset: 0x004CD978
		public static void UnRegisterHandler_InterruptOtherAction(Events.OnInterruptOtherAction handler)
		{
			Events._handlersInterruptOtherAction = (Events.OnInterruptOtherAction)Delegate.Remove(Events._handlersInterruptOtherAction, handler);
		}

		// Token: 0x060080BB RID: 32955 RVA: 0x004CF790 File Offset: 0x004CD990
		public static void RaiseInterruptOtherAction(DataContext context, CombatCharacter combatChar, sbyte otherActionType)
		{
			Events.OnInterruptOtherAction handlersInterruptOtherAction = Events._handlersInterruptOtherAction;
			if (handlersInterruptOtherAction != null)
			{
				handlersInterruptOtherAction(context, combatChar, otherActionType);
			}
		}

		// Token: 0x060080BC RID: 32956 RVA: 0x004CF7A7 File Offset: 0x004CD9A7
		public static void RegisterHandler_FlawAdded(Events.OnFlawAdded handler)
		{
			Events._handlersFlawAdded = (Events.OnFlawAdded)Delegate.Combine(Events._handlersFlawAdded, handler);
		}

		// Token: 0x060080BD RID: 32957 RVA: 0x004CF7BF File Offset: 0x004CD9BF
		public static void UnRegisterHandler_FlawAdded(Events.OnFlawAdded handler)
		{
			Events._handlersFlawAdded = (Events.OnFlawAdded)Delegate.Remove(Events._handlersFlawAdded, handler);
		}

		// Token: 0x060080BE RID: 32958 RVA: 0x004CF7D7 File Offset: 0x004CD9D7
		public static void RaiseFlawAdded(DataContext context, CombatCharacter combatChar, sbyte bodyPart, sbyte level)
		{
			Events.OnFlawAdded handlersFlawAdded = Events._handlersFlawAdded;
			if (handlersFlawAdded != null)
			{
				handlersFlawAdded(context, combatChar, bodyPart, level);
			}
		}

		// Token: 0x060080BF RID: 32959 RVA: 0x004CF7EF File Offset: 0x004CD9EF
		public static void RegisterHandler_FlawRemoved(Events.OnFlawRemoved handler)
		{
			Events._handlersFlawRemoved = (Events.OnFlawRemoved)Delegate.Combine(Events._handlersFlawRemoved, handler);
		}

		// Token: 0x060080C0 RID: 32960 RVA: 0x004CF807 File Offset: 0x004CDA07
		public static void UnRegisterHandler_FlawRemoved(Events.OnFlawRemoved handler)
		{
			Events._handlersFlawRemoved = (Events.OnFlawRemoved)Delegate.Remove(Events._handlersFlawRemoved, handler);
		}

		// Token: 0x060080C1 RID: 32961 RVA: 0x004CF81F File Offset: 0x004CDA1F
		public static void RaiseFlawRemoved(DataContext context, CombatCharacter combatChar, sbyte bodyPart, sbyte level)
		{
			Events.OnFlawRemoved handlersFlawRemoved = Events._handlersFlawRemoved;
			if (handlersFlawRemoved != null)
			{
				handlersFlawRemoved(context, combatChar, bodyPart, level);
			}
		}

		// Token: 0x060080C2 RID: 32962 RVA: 0x004CF837 File Offset: 0x004CDA37
		public static void RegisterHandler_AcuPointAdded(Events.OnAcuPointAdded handler)
		{
			Events._handlersAcuPointAdded = (Events.OnAcuPointAdded)Delegate.Combine(Events._handlersAcuPointAdded, handler);
		}

		// Token: 0x060080C3 RID: 32963 RVA: 0x004CF84F File Offset: 0x004CDA4F
		public static void UnRegisterHandler_AcuPointAdded(Events.OnAcuPointAdded handler)
		{
			Events._handlersAcuPointAdded = (Events.OnAcuPointAdded)Delegate.Remove(Events._handlersAcuPointAdded, handler);
		}

		// Token: 0x060080C4 RID: 32964 RVA: 0x004CF867 File Offset: 0x004CDA67
		public static void RaiseAcuPointAdded(DataContext context, CombatCharacter combatChar, sbyte bodyPart, sbyte level)
		{
			Events.OnAcuPointAdded handlersAcuPointAdded = Events._handlersAcuPointAdded;
			if (handlersAcuPointAdded != null)
			{
				handlersAcuPointAdded(context, combatChar, bodyPart, level);
			}
		}

		// Token: 0x060080C5 RID: 32965 RVA: 0x004CF87F File Offset: 0x004CDA7F
		public static void RegisterHandler_AcuPointRemoved(Events.OnAcuPointRemoved handler)
		{
			Events._handlersAcuPointRemoved = (Events.OnAcuPointRemoved)Delegate.Combine(Events._handlersAcuPointRemoved, handler);
		}

		// Token: 0x060080C6 RID: 32966 RVA: 0x004CF897 File Offset: 0x004CDA97
		public static void UnRegisterHandler_AcuPointRemoved(Events.OnAcuPointRemoved handler)
		{
			Events._handlersAcuPointRemoved = (Events.OnAcuPointRemoved)Delegate.Remove(Events._handlersAcuPointRemoved, handler);
		}

		// Token: 0x060080C7 RID: 32967 RVA: 0x004CF8AF File Offset: 0x004CDAAF
		public static void RaiseAcuPointRemoved(DataContext context, CombatCharacter combatChar, sbyte bodyPart, sbyte level)
		{
			Events.OnAcuPointRemoved handlersAcuPointRemoved = Events._handlersAcuPointRemoved;
			if (handlersAcuPointRemoved != null)
			{
				handlersAcuPointRemoved(context, combatChar, bodyPart, level);
			}
		}

		// Token: 0x060080C8 RID: 32968 RVA: 0x004CF8C7 File Offset: 0x004CDAC7
		public static void RegisterHandler_CombatCharChanged(Events.OnCombatCharChanged handler)
		{
			Events._handlersCombatCharChanged = (Events.OnCombatCharChanged)Delegate.Combine(Events._handlersCombatCharChanged, handler);
		}

		// Token: 0x060080C9 RID: 32969 RVA: 0x004CF8DF File Offset: 0x004CDADF
		public static void UnRegisterHandler_CombatCharChanged(Events.OnCombatCharChanged handler)
		{
			Events._handlersCombatCharChanged = (Events.OnCombatCharChanged)Delegate.Remove(Events._handlersCombatCharChanged, handler);
		}

		// Token: 0x060080CA RID: 32970 RVA: 0x004CF8F7 File Offset: 0x004CDAF7
		public static void RaiseCombatCharChanged(DataContext context, bool isAlly)
		{
			Events.OnCombatCharChanged handlersCombatCharChanged = Events._handlersCombatCharChanged;
			if (handlersCombatCharChanged != null)
			{
				handlersCombatCharChanged(context, isAlly);
			}
		}

		// Token: 0x060080CB RID: 32971 RVA: 0x004CF90D File Offset: 0x004CDB0D
		public static void RegisterHandler_AddInjury(Events.OnAddInjury handler)
		{
			Events._handlersAddInjury = (Events.OnAddInjury)Delegate.Combine(Events._handlersAddInjury, handler);
		}

		// Token: 0x060080CC RID: 32972 RVA: 0x004CF925 File Offset: 0x004CDB25
		public static void UnRegisterHandler_AddInjury(Events.OnAddInjury handler)
		{
			Events._handlersAddInjury = (Events.OnAddInjury)Delegate.Remove(Events._handlersAddInjury, handler);
		}

		// Token: 0x060080CD RID: 32973 RVA: 0x004CF93D File Offset: 0x004CDB3D
		public static void RaiseAddInjury(DataContext context, CombatCharacter character, sbyte bodyPart, bool isInner, sbyte value, bool changeToOld)
		{
			Events.OnAddInjury handlersAddInjury = Events._handlersAddInjury;
			if (handlersAddInjury != null)
			{
				handlersAddInjury(context, character, bodyPart, isInner, value, changeToOld);
			}
		}

		// Token: 0x060080CE RID: 32974 RVA: 0x004CF959 File Offset: 0x004CDB59
		public static void RegisterHandler_AddDirectDamageValue(Events.OnAddDirectDamageValue handler)
		{
			Events._handlersAddDirectDamageValue = (Events.OnAddDirectDamageValue)Delegate.Combine(Events._handlersAddDirectDamageValue, handler);
		}

		// Token: 0x060080CF RID: 32975 RVA: 0x004CF971 File Offset: 0x004CDB71
		public static void UnRegisterHandler_AddDirectDamageValue(Events.OnAddDirectDamageValue handler)
		{
			Events._handlersAddDirectDamageValue = (Events.OnAddDirectDamageValue)Delegate.Remove(Events._handlersAddDirectDamageValue, handler);
		}

		// Token: 0x060080D0 RID: 32976 RVA: 0x004CF989 File Offset: 0x004CDB89
		public static void RaiseAddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId)
		{
			Events.OnAddDirectDamageValue handlersAddDirectDamageValue = Events._handlersAddDirectDamageValue;
			if (handlersAddDirectDamageValue != null)
			{
				handlersAddDirectDamageValue(context, attackerId, defenderId, bodyPart, isInner, damageValue, combatSkillId);
			}
		}

		// Token: 0x060080D1 RID: 32977 RVA: 0x004CF9A7 File Offset: 0x004CDBA7
		public static void RegisterHandler_AddDirectInjury(Events.OnAddDirectInjury handler)
		{
			Events._handlersAddDirectInjury = (Events.OnAddDirectInjury)Delegate.Combine(Events._handlersAddDirectInjury, handler);
		}

		// Token: 0x060080D2 RID: 32978 RVA: 0x004CF9BF File Offset: 0x004CDBBF
		public static void UnRegisterHandler_AddDirectInjury(Events.OnAddDirectInjury handler)
		{
			Events._handlersAddDirectInjury = (Events.OnAddDirectInjury)Delegate.Remove(Events._handlersAddDirectInjury, handler);
		}

		// Token: 0x060080D3 RID: 32979 RVA: 0x004CF9D8 File Offset: 0x004CDBD8
		public static void RaiseAddDirectInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount, short combatSkillId)
		{
			Events.OnAddDirectInjury handlersAddDirectInjury = Events._handlersAddDirectInjury;
			if (handlersAddDirectInjury != null)
			{
				handlersAddDirectInjury(context, attackerId, defenderId, isAlly, bodyPart, outerMarkCount, innerMarkCount, combatSkillId);
			}
		}

		// Token: 0x060080D4 RID: 32980 RVA: 0x004CFA03 File Offset: 0x004CDC03
		public static void RegisterHandler_BounceInjury(Events.OnBounceInjury handler)
		{
			Events._handlersBounceInjury = (Events.OnBounceInjury)Delegate.Combine(Events._handlersBounceInjury, handler);
		}

		// Token: 0x060080D5 RID: 32981 RVA: 0x004CFA1B File Offset: 0x004CDC1B
		public static void UnRegisterHandler_BounceInjury(Events.OnBounceInjury handler)
		{
			Events._handlersBounceInjury = (Events.OnBounceInjury)Delegate.Remove(Events._handlersBounceInjury, handler);
		}

		// Token: 0x060080D6 RID: 32982 RVA: 0x004CFA33 File Offset: 0x004CDC33
		public static void RaiseBounceInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount)
		{
			Events.OnBounceInjury handlersBounceInjury = Events._handlersBounceInjury;
			if (handlersBounceInjury != null)
			{
				handlersBounceInjury(context, attackerId, defenderId, isAlly, bodyPart, outerMarkCount, innerMarkCount);
			}
		}

		// Token: 0x060080D7 RID: 32983 RVA: 0x004CFA51 File Offset: 0x004CDC51
		public static void RegisterHandler_AddMindMark(Events.OnAddMindMark handler)
		{
			Events._handlersAddMindMark = (Events.OnAddMindMark)Delegate.Combine(Events._handlersAddMindMark, handler);
		}

		// Token: 0x060080D8 RID: 32984 RVA: 0x004CFA69 File Offset: 0x004CDC69
		public static void UnRegisterHandler_AddMindMark(Events.OnAddMindMark handler)
		{
			Events._handlersAddMindMark = (Events.OnAddMindMark)Delegate.Remove(Events._handlersAddMindMark, handler);
		}

		// Token: 0x060080D9 RID: 32985 RVA: 0x004CFA81 File Offset: 0x004CDC81
		public static void RaiseAddMindMark(DataContext context, CombatCharacter character, int count)
		{
			Events.OnAddMindMark handlersAddMindMark = Events._handlersAddMindMark;
			if (handlersAddMindMark != null)
			{
				handlersAddMindMark(context, character, count);
			}
		}

		// Token: 0x060080DA RID: 32986 RVA: 0x004CFA98 File Offset: 0x004CDC98
		public static void RegisterHandler_AddMindDamage(Events.OnAddMindDamage handler)
		{
			Events._handlersAddMindDamage = (Events.OnAddMindDamage)Delegate.Combine(Events._handlersAddMindDamage, handler);
		}

		// Token: 0x060080DB RID: 32987 RVA: 0x004CFAB0 File Offset: 0x004CDCB0
		public static void UnRegisterHandler_AddMindDamage(Events.OnAddMindDamage handler)
		{
			Events._handlersAddMindDamage = (Events.OnAddMindDamage)Delegate.Remove(Events._handlersAddMindDamage, handler);
		}

		// Token: 0x060080DC RID: 32988 RVA: 0x004CFAC8 File Offset: 0x004CDCC8
		public static void RaiseAddMindDamage(DataContext context, int attackerId, int defenderId, int damageValue, int markCount, short combatSkillId)
		{
			Events.OnAddMindDamage handlersAddMindDamage = Events._handlersAddMindDamage;
			if (handlersAddMindDamage != null)
			{
				handlersAddMindDamage(context, attackerId, defenderId, damageValue, markCount, combatSkillId);
			}
		}

		// Token: 0x060080DD RID: 32989 RVA: 0x004CFAE4 File Offset: 0x004CDCE4
		public static void RegisterHandler_AddFatalDamageMark(Events.OnAddFatalDamageMark handler)
		{
			Events._handlersAddFatalDamageMark = (Events.OnAddFatalDamageMark)Delegate.Combine(Events._handlersAddFatalDamageMark, handler);
		}

		// Token: 0x060080DE RID: 32990 RVA: 0x004CFAFC File Offset: 0x004CDCFC
		public static void UnRegisterHandler_AddFatalDamageMark(Events.OnAddFatalDamageMark handler)
		{
			Events._handlersAddFatalDamageMark = (Events.OnAddFatalDamageMark)Delegate.Remove(Events._handlersAddFatalDamageMark, handler);
		}

		// Token: 0x060080DF RID: 32991 RVA: 0x004CFB14 File Offset: 0x004CDD14
		public static void RaiseAddFatalDamageMark(DataContext context, CombatCharacter combatChar, int count)
		{
			Events.OnAddFatalDamageMark handlersAddFatalDamageMark = Events._handlersAddFatalDamageMark;
			if (handlersAddFatalDamageMark != null)
			{
				handlersAddFatalDamageMark(context, combatChar, count);
			}
		}

		// Token: 0x060080E0 RID: 32992 RVA: 0x004CFB2B File Offset: 0x004CDD2B
		public static void RegisterHandler_AddDirectFatalDamageMark(Events.OnAddDirectFatalDamageMark handler)
		{
			Events._handlersAddDirectFatalDamageMark = (Events.OnAddDirectFatalDamageMark)Delegate.Combine(Events._handlersAddDirectFatalDamageMark, handler);
		}

		// Token: 0x060080E1 RID: 32993 RVA: 0x004CFB43 File Offset: 0x004CDD43
		public static void UnRegisterHandler_AddDirectFatalDamageMark(Events.OnAddDirectFatalDamageMark handler)
		{
			Events._handlersAddDirectFatalDamageMark = (Events.OnAddDirectFatalDamageMark)Delegate.Remove(Events._handlersAddDirectFatalDamageMark, handler);
		}

		// Token: 0x060080E2 RID: 32994 RVA: 0x004CFB5C File Offset: 0x004CDD5C
		public static void RaiseAddDirectFatalDamageMark(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, int outerMarkCount, int innerMarkCount, short combatSkillId)
		{
			Events.OnAddDirectFatalDamageMark handlersAddDirectFatalDamageMark = Events._handlersAddDirectFatalDamageMark;
			if (handlersAddDirectFatalDamageMark != null)
			{
				handlersAddDirectFatalDamageMark(context, attackerId, defenderId, isAlly, bodyPart, outerMarkCount, innerMarkCount, combatSkillId);
			}
		}

		// Token: 0x060080E3 RID: 32995 RVA: 0x004CFB87 File Offset: 0x004CDD87
		public static void RegisterHandler_AddDirectFatalDamage(Events.OnAddDirectFatalDamage handler)
		{
			Events._handlersAddDirectFatalDamage = (Events.OnAddDirectFatalDamage)Delegate.Combine(Events._handlersAddDirectFatalDamage, handler);
		}

		// Token: 0x060080E4 RID: 32996 RVA: 0x004CFB9F File Offset: 0x004CDD9F
		public static void UnRegisterHandler_AddDirectFatalDamage(Events.OnAddDirectFatalDamage handler)
		{
			Events._handlersAddDirectFatalDamage = (Events.OnAddDirectFatalDamage)Delegate.Remove(Events._handlersAddDirectFatalDamage, handler);
		}

		// Token: 0x060080E5 RID: 32997 RVA: 0x004CFBB7 File Offset: 0x004CDDB7
		public static void RaiseAddDirectFatalDamage(CombatContext context, int outer, int inner)
		{
			Events.OnAddDirectFatalDamage handlersAddDirectFatalDamage = Events._handlersAddDirectFatalDamage;
			if (handlersAddDirectFatalDamage != null)
			{
				handlersAddDirectFatalDamage(context, outer, inner);
			}
		}

		// Token: 0x060080E6 RID: 32998 RVA: 0x004CFBCE File Offset: 0x004CDDCE
		public static void RegisterHandler_AddDirectPoisonMark(Events.OnAddDirectPoisonMark handler)
		{
			Events._handlersAddDirectPoisonMark = (Events.OnAddDirectPoisonMark)Delegate.Combine(Events._handlersAddDirectPoisonMark, handler);
		}

		// Token: 0x060080E7 RID: 32999 RVA: 0x004CFBE6 File Offset: 0x004CDDE6
		public static void UnRegisterHandler_AddDirectPoisonMark(Events.OnAddDirectPoisonMark handler)
		{
			Events._handlersAddDirectPoisonMark = (Events.OnAddDirectPoisonMark)Delegate.Remove(Events._handlersAddDirectPoisonMark, handler);
		}

		// Token: 0x060080E8 RID: 33000 RVA: 0x004CFBFE File Offset: 0x004CDDFE
		public static void RaiseAddDirectPoisonMark(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte poisonType, short skillId, int markCount)
		{
			Events.OnAddDirectPoisonMark handlersAddDirectPoisonMark = Events._handlersAddDirectPoisonMark;
			if (handlersAddDirectPoisonMark != null)
			{
				handlersAddDirectPoisonMark(context, attacker, defender, poisonType, skillId, markCount);
			}
		}

		// Token: 0x060080E9 RID: 33001 RVA: 0x004CFC1A File Offset: 0x004CDE1A
		public static void RegisterHandler_MoveStateChanged(Events.OnMoveStateChanged handler)
		{
			Events._handlersMoveStateChanged = (Events.OnMoveStateChanged)Delegate.Combine(Events._handlersMoveStateChanged, handler);
		}

		// Token: 0x060080EA RID: 33002 RVA: 0x004CFC32 File Offset: 0x004CDE32
		public static void UnRegisterHandler_MoveStateChanged(Events.OnMoveStateChanged handler)
		{
			Events._handlersMoveStateChanged = (Events.OnMoveStateChanged)Delegate.Remove(Events._handlersMoveStateChanged, handler);
		}

		// Token: 0x060080EB RID: 33003 RVA: 0x004CFC4A File Offset: 0x004CDE4A
		public static void RaiseMoveStateChanged(DataContext context, CombatCharacter character, byte moveState)
		{
			Events.OnMoveStateChanged handlersMoveStateChanged = Events._handlersMoveStateChanged;
			if (handlersMoveStateChanged != null)
			{
				handlersMoveStateChanged(context, character, moveState);
			}
		}

		// Token: 0x060080EC RID: 33004 RVA: 0x004CFC61 File Offset: 0x004CDE61
		public static void RegisterHandler_MoveBegin(Events.OnMoveBegin handler)
		{
			Events._handlersMoveBegin = (Events.OnMoveBegin)Delegate.Combine(Events._handlersMoveBegin, handler);
		}

		// Token: 0x060080ED RID: 33005 RVA: 0x004CFC79 File Offset: 0x004CDE79
		public static void UnRegisterHandler_MoveBegin(Events.OnMoveBegin handler)
		{
			Events._handlersMoveBegin = (Events.OnMoveBegin)Delegate.Remove(Events._handlersMoveBegin, handler);
		}

		// Token: 0x060080EE RID: 33006 RVA: 0x004CFC91 File Offset: 0x004CDE91
		public static void RaiseMoveBegin(DataContext context, CombatCharacter mover, int distance, bool isJump)
		{
			Events.OnMoveBegin handlersMoveBegin = Events._handlersMoveBegin;
			if (handlersMoveBegin != null)
			{
				handlersMoveBegin(context, mover, distance, isJump);
			}
		}

		// Token: 0x060080EF RID: 33007 RVA: 0x004CFCA9 File Offset: 0x004CDEA9
		public static void RegisterHandler_MoveEnd(Events.OnMoveEnd handler)
		{
			Events._handlersMoveEnd = (Events.OnMoveEnd)Delegate.Combine(Events._handlersMoveEnd, handler);
		}

		// Token: 0x060080F0 RID: 33008 RVA: 0x004CFCC1 File Offset: 0x004CDEC1
		public static void UnRegisterHandler_MoveEnd(Events.OnMoveEnd handler)
		{
			Events._handlersMoveEnd = (Events.OnMoveEnd)Delegate.Remove(Events._handlersMoveEnd, handler);
		}

		// Token: 0x060080F1 RID: 33009 RVA: 0x004CFCD9 File Offset: 0x004CDED9
		public static void RaiseMoveEnd(DataContext context, CombatCharacter mover, int distance, bool isJump)
		{
			Events.OnMoveEnd handlersMoveEnd = Events._handlersMoveEnd;
			if (handlersMoveEnd != null)
			{
				handlersMoveEnd(context, mover, distance, isJump);
			}
		}

		// Token: 0x060080F2 RID: 33010 RVA: 0x004CFCF1 File Offset: 0x004CDEF1
		public static void RegisterHandler_IgnoredForceChangeDistance(Events.OnIgnoredForceChangeDistance handler)
		{
			Events._handlersIgnoredForceChangeDistance = (Events.OnIgnoredForceChangeDistance)Delegate.Combine(Events._handlersIgnoredForceChangeDistance, handler);
		}

		// Token: 0x060080F3 RID: 33011 RVA: 0x004CFD09 File Offset: 0x004CDF09
		public static void UnRegisterHandler_IgnoredForceChangeDistance(Events.OnIgnoredForceChangeDistance handler)
		{
			Events._handlersIgnoredForceChangeDistance = (Events.OnIgnoredForceChangeDistance)Delegate.Remove(Events._handlersIgnoredForceChangeDistance, handler);
		}

		// Token: 0x060080F4 RID: 33012 RVA: 0x004CFD21 File Offset: 0x004CDF21
		public static void RaiseIgnoredForceChangeDistance(DataContext context, CombatCharacter mover, int distance)
		{
			Events.OnIgnoredForceChangeDistance handlersIgnoredForceChangeDistance = Events._handlersIgnoredForceChangeDistance;
			if (handlersIgnoredForceChangeDistance != null)
			{
				handlersIgnoredForceChangeDistance(context, mover, distance);
			}
		}

		// Token: 0x060080F5 RID: 33013 RVA: 0x004CFD38 File Offset: 0x004CDF38
		public static void RegisterHandler_DistanceChanged(Events.OnDistanceChanged handler)
		{
			Events._handlersDistanceChanged = (Events.OnDistanceChanged)Delegate.Combine(Events._handlersDistanceChanged, handler);
		}

		// Token: 0x060080F6 RID: 33014 RVA: 0x004CFD50 File Offset: 0x004CDF50
		public static void UnRegisterHandler_DistanceChanged(Events.OnDistanceChanged handler)
		{
			Events._handlersDistanceChanged = (Events.OnDistanceChanged)Delegate.Remove(Events._handlersDistanceChanged, handler);
		}

		// Token: 0x060080F7 RID: 33015 RVA: 0x004CFD68 File Offset: 0x004CDF68
		public static void RaiseDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			Events.OnDistanceChanged handlersDistanceChanged = Events._handlersDistanceChanged;
			if (handlersDistanceChanged != null)
			{
				handlersDistanceChanged(context, mover, distance, isMove, isForced);
			}
		}

		// Token: 0x060080F8 RID: 33016 RVA: 0x004CFD82 File Offset: 0x004CDF82
		public static void RegisterHandler_SkillEffectChange(Events.OnSkillEffectChange handler)
		{
			Events._handlersSkillEffectChange = (Events.OnSkillEffectChange)Delegate.Combine(Events._handlersSkillEffectChange, handler);
		}

		// Token: 0x060080F9 RID: 33017 RVA: 0x004CFD9A File Offset: 0x004CDF9A
		public static void UnRegisterHandler_SkillEffectChange(Events.OnSkillEffectChange handler)
		{
			Events._handlersSkillEffectChange = (Events.OnSkillEffectChange)Delegate.Remove(Events._handlersSkillEffectChange, handler);
		}

		// Token: 0x060080FA RID: 33018 RVA: 0x004CFDB2 File Offset: 0x004CDFB2
		public static void RaiseSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			Events.OnSkillEffectChange handlersSkillEffectChange = Events._handlersSkillEffectChange;
			if (handlersSkillEffectChange != null)
			{
				handlersSkillEffectChange(context, charId, key, oldCount, newCount, removed);
			}
		}

		// Token: 0x060080FB RID: 33019 RVA: 0x004CFDCE File Offset: 0x004CDFCE
		public static void RegisterHandler_SkillSilence(Events.OnSkillSilence handler)
		{
			Events._handlersSkillSilence = (Events.OnSkillSilence)Delegate.Combine(Events._handlersSkillSilence, handler);
		}

		// Token: 0x060080FC RID: 33020 RVA: 0x004CFDE6 File Offset: 0x004CDFE6
		public static void UnRegisterHandler_SkillSilence(Events.OnSkillSilence handler)
		{
			Events._handlersSkillSilence = (Events.OnSkillSilence)Delegate.Remove(Events._handlersSkillSilence, handler);
		}

		// Token: 0x060080FD RID: 33021 RVA: 0x004CFDFE File Offset: 0x004CDFFE
		public static void RaiseSkillSilence(DataContext context, CombatSkillKey skillKey)
		{
			Events.OnSkillSilence handlersSkillSilence = Events._handlersSkillSilence;
			if (handlersSkillSilence != null)
			{
				handlersSkillSilence(context, skillKey);
			}
		}

		// Token: 0x060080FE RID: 33022 RVA: 0x004CFE14 File Offset: 0x004CE014
		public static void RegisterHandler_SkillSilenceEnd(Events.OnSkillSilenceEnd handler)
		{
			Events._handlersSkillSilenceEnd = (Events.OnSkillSilenceEnd)Delegate.Combine(Events._handlersSkillSilenceEnd, handler);
		}

		// Token: 0x060080FF RID: 33023 RVA: 0x004CFE2C File Offset: 0x004CE02C
		public static void UnRegisterHandler_SkillSilenceEnd(Events.OnSkillSilenceEnd handler)
		{
			Events._handlersSkillSilenceEnd = (Events.OnSkillSilenceEnd)Delegate.Remove(Events._handlersSkillSilenceEnd, handler);
		}

		// Token: 0x06008100 RID: 33024 RVA: 0x004CFE44 File Offset: 0x004CE044
		public static void RaiseSkillSilenceEnd(DataContext context, CombatSkillKey skillKey)
		{
			Events.OnSkillSilenceEnd handlersSkillSilenceEnd = Events._handlersSkillSilenceEnd;
			if (handlersSkillSilenceEnd != null)
			{
				handlersSkillSilenceEnd(context, skillKey);
			}
		}

		// Token: 0x06008101 RID: 33025 RVA: 0x004CFE5A File Offset: 0x004CE05A
		public static void RegisterHandler_NeiliAllocationChanged(Events.OnNeiliAllocationChanged handler)
		{
			Events._handlersNeiliAllocationChanged = (Events.OnNeiliAllocationChanged)Delegate.Combine(Events._handlersNeiliAllocationChanged, handler);
		}

		// Token: 0x06008102 RID: 33026 RVA: 0x004CFE72 File Offset: 0x004CE072
		public static void UnRegisterHandler_NeiliAllocationChanged(Events.OnNeiliAllocationChanged handler)
		{
			Events._handlersNeiliAllocationChanged = (Events.OnNeiliAllocationChanged)Delegate.Remove(Events._handlersNeiliAllocationChanged, handler);
		}

		// Token: 0x06008103 RID: 33027 RVA: 0x004CFE8A File Offset: 0x004CE08A
		public static void RaiseNeiliAllocationChanged(DataContext context, int charId, byte type, int changeValue)
		{
			Events.OnNeiliAllocationChanged handlersNeiliAllocationChanged = Events._handlersNeiliAllocationChanged;
			if (handlersNeiliAllocationChanged != null)
			{
				handlersNeiliAllocationChanged(context, charId, type, changeValue);
			}
		}

		// Token: 0x06008104 RID: 33028 RVA: 0x004CFEA2 File Offset: 0x004CE0A2
		public static void RegisterHandler_AddPoison(Events.OnAddPoison handler)
		{
			Events._handlersAddPoison = (Events.OnAddPoison)Delegate.Combine(Events._handlersAddPoison, handler);
		}

		// Token: 0x06008105 RID: 33029 RVA: 0x004CFEBA File Offset: 0x004CE0BA
		public static void UnRegisterHandler_AddPoison(Events.OnAddPoison handler)
		{
			Events._handlersAddPoison = (Events.OnAddPoison)Delegate.Remove(Events._handlersAddPoison, handler);
		}

		// Token: 0x06008106 RID: 33030 RVA: 0x004CFED4 File Offset: 0x004CE0D4
		public static void RaiseAddPoison(DataContext context, int attackerId, int defenderId, sbyte poisonType, sbyte level, int addValue, short skillId, bool canBounce)
		{
			Events.OnAddPoison handlersAddPoison = Events._handlersAddPoison;
			if (handlersAddPoison != null)
			{
				handlersAddPoison(context, attackerId, defenderId, poisonType, level, addValue, skillId, canBounce);
			}
		}

		// Token: 0x06008107 RID: 33031 RVA: 0x004CFEFF File Offset: 0x004CE0FF
		public static void RegisterHandler_PoisonAffected(Events.OnPoisonAffected handler)
		{
			Events._handlersPoisonAffected = (Events.OnPoisonAffected)Delegate.Combine(Events._handlersPoisonAffected, handler);
		}

		// Token: 0x06008108 RID: 33032 RVA: 0x004CFF17 File Offset: 0x004CE117
		public static void UnRegisterHandler_PoisonAffected(Events.OnPoisonAffected handler)
		{
			Events._handlersPoisonAffected = (Events.OnPoisonAffected)Delegate.Remove(Events._handlersPoisonAffected, handler);
		}

		// Token: 0x06008109 RID: 33033 RVA: 0x004CFF2F File Offset: 0x004CE12F
		public static void RaisePoisonAffected(DataContext context, int charId, sbyte poisonType)
		{
			Events.OnPoisonAffected handlersPoisonAffected = Events._handlersPoisonAffected;
			if (handlersPoisonAffected != null)
			{
				handlersPoisonAffected(context, charId, poisonType);
			}
		}

		// Token: 0x0600810A RID: 33034 RVA: 0x004CFF46 File Offset: 0x004CE146
		public static void RegisterHandler_AddWug(Events.OnAddWug handler)
		{
			Events._handlersAddWug = (Events.OnAddWug)Delegate.Combine(Events._handlersAddWug, handler);
		}

		// Token: 0x0600810B RID: 33035 RVA: 0x004CFF5E File Offset: 0x004CE15E
		public static void UnRegisterHandler_AddWug(Events.OnAddWug handler)
		{
			Events._handlersAddWug = (Events.OnAddWug)Delegate.Remove(Events._handlersAddWug, handler);
		}

		// Token: 0x0600810C RID: 33036 RVA: 0x004CFF76 File Offset: 0x004CE176
		public static void RaiseAddWug(DataContext context, int charId, short wugTemplateId, short replacedWug)
		{
			Events.OnAddWug handlersAddWug = Events._handlersAddWug;
			if (handlersAddWug != null)
			{
				handlersAddWug(context, charId, wugTemplateId, replacedWug);
			}
		}

		// Token: 0x0600810D RID: 33037 RVA: 0x004CFF8E File Offset: 0x004CE18E
		public static void RegisterHandler_RemoveWug(Events.OnRemoveWug handler)
		{
			Events._handlersRemoveWug = (Events.OnRemoveWug)Delegate.Combine(Events._handlersRemoveWug, handler);
		}

		// Token: 0x0600810E RID: 33038 RVA: 0x004CFFA6 File Offset: 0x004CE1A6
		public static void UnRegisterHandler_RemoveWug(Events.OnRemoveWug handler)
		{
			Events._handlersRemoveWug = (Events.OnRemoveWug)Delegate.Remove(Events._handlersRemoveWug, handler);
		}

		// Token: 0x0600810F RID: 33039 RVA: 0x004CFFBE File Offset: 0x004CE1BE
		public static void RaiseRemoveWug(DataContext context, int charId, short wugTemplateId)
		{
			Events.OnRemoveWug handlersRemoveWug = Events._handlersRemoveWug;
			if (handlersRemoveWug != null)
			{
				handlersRemoveWug(context, charId, wugTemplateId);
			}
		}

		// Token: 0x06008110 RID: 33040 RVA: 0x004CFFD5 File Offset: 0x004CE1D5
		public static void RegisterHandler_CompareDataCalcFinished(Events.OnCompareDataCalcFinished handler)
		{
			Events._handlersCompareDataCalcFinished = (Events.OnCompareDataCalcFinished)Delegate.Combine(Events._handlersCompareDataCalcFinished, handler);
		}

		// Token: 0x06008111 RID: 33041 RVA: 0x004CFFED File Offset: 0x004CE1ED
		public static void UnRegisterHandler_CompareDataCalcFinished(Events.OnCompareDataCalcFinished handler)
		{
			Events._handlersCompareDataCalcFinished = (Events.OnCompareDataCalcFinished)Delegate.Remove(Events._handlersCompareDataCalcFinished, handler);
		}

		// Token: 0x06008112 RID: 33042 RVA: 0x004D0005 File Offset: 0x004CE205
		public static void RaiseCompareDataCalcFinished(CombatContext context, DamageCompareData compareData)
		{
			Events.OnCompareDataCalcFinished handlersCompareDataCalcFinished = Events._handlersCompareDataCalcFinished;
			if (handlersCompareDataCalcFinished != null)
			{
				handlersCompareDataCalcFinished(context, compareData);
			}
		}

		// Token: 0x06008113 RID: 33043 RVA: 0x004D001B File Offset: 0x004CE21B
		public static void RegisterHandler_CombatStateMachineUpdateEnd(Events.OnCombatStateMachineUpdateEnd handler)
		{
			Events._handlersCombatStateMachineUpdateEnd = (Events.OnCombatStateMachineUpdateEnd)Delegate.Combine(Events._handlersCombatStateMachineUpdateEnd, handler);
		}

		// Token: 0x06008114 RID: 33044 RVA: 0x004D0033 File Offset: 0x004CE233
		public static void UnRegisterHandler_CombatStateMachineUpdateEnd(Events.OnCombatStateMachineUpdateEnd handler)
		{
			Events._handlersCombatStateMachineUpdateEnd = (Events.OnCombatStateMachineUpdateEnd)Delegate.Remove(Events._handlersCombatStateMachineUpdateEnd, handler);
		}

		// Token: 0x06008115 RID: 33045 RVA: 0x004D004B File Offset: 0x004CE24B
		public static void RaiseCombatStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			Events.OnCombatStateMachineUpdateEnd handlersCombatStateMachineUpdateEnd = Events._handlersCombatStateMachineUpdateEnd;
			if (handlersCombatStateMachineUpdateEnd != null)
			{
				handlersCombatStateMachineUpdateEnd(context, combatChar);
			}
		}

		// Token: 0x06008116 RID: 33046 RVA: 0x004D0061 File Offset: 0x004CE261
		public static void RegisterHandler_CombatCharFallen(Events.OnCombatCharFallen handler)
		{
			Events._handlersCombatCharFallen = (Events.OnCombatCharFallen)Delegate.Combine(Events._handlersCombatCharFallen, handler);
		}

		// Token: 0x06008117 RID: 33047 RVA: 0x004D0079 File Offset: 0x004CE279
		public static void UnRegisterHandler_CombatCharFallen(Events.OnCombatCharFallen handler)
		{
			Events._handlersCombatCharFallen = (Events.OnCombatCharFallen)Delegate.Remove(Events._handlersCombatCharFallen, handler);
		}

		// Token: 0x06008118 RID: 33048 RVA: 0x004D0091 File Offset: 0x004CE291
		public static void RaiseCombatCharFallen(DataContext context, CombatCharacter combatChar)
		{
			Events.OnCombatCharFallen handlersCombatCharFallen = Events._handlersCombatCharFallen;
			if (handlersCombatCharFallen != null)
			{
				handlersCombatCharFallen(context, combatChar);
			}
		}

		// Token: 0x06008119 RID: 33049 RVA: 0x004D00A7 File Offset: 0x004CE2A7
		public static void RegisterHandler_CombatCostNeiliConfirm(Events.OnCombatCostNeiliConfirm handler)
		{
			Events._handlersCombatCostNeiliConfirm = (Events.OnCombatCostNeiliConfirm)Delegate.Combine(Events._handlersCombatCostNeiliConfirm, handler);
		}

		// Token: 0x0600811A RID: 33050 RVA: 0x004D00BF File Offset: 0x004CE2BF
		public static void UnRegisterHandler_CombatCostNeiliConfirm(Events.OnCombatCostNeiliConfirm handler)
		{
			Events._handlersCombatCostNeiliConfirm = (Events.OnCombatCostNeiliConfirm)Delegate.Remove(Events._handlersCombatCostNeiliConfirm, handler);
		}

		// Token: 0x0600811B RID: 33051 RVA: 0x004D00D7 File Offset: 0x004CE2D7
		public static void RaiseCombatCostNeiliConfirm(DataContext context, int charId, short skillId, short effectId)
		{
			Events.OnCombatCostNeiliConfirm handlersCombatCostNeiliConfirm = Events._handlersCombatCostNeiliConfirm;
			if (handlersCombatCostNeiliConfirm != null)
			{
				handlersCombatCostNeiliConfirm(context, charId, skillId, effectId);
			}
		}

		// Token: 0x0600811C RID: 33052 RVA: 0x004D00EF File Offset: 0x004CE2EF
		public static void RegisterHandler_CostTrickDuringPreparingSkill(Events.OnCostTrickDuringPreparingSkill handler)
		{
			Events._handlersCostTrickDuringPreparingSkill = (Events.OnCostTrickDuringPreparingSkill)Delegate.Combine(Events._handlersCostTrickDuringPreparingSkill, handler);
		}

		// Token: 0x0600811D RID: 33053 RVA: 0x004D0107 File Offset: 0x004CE307
		public static void UnRegisterHandler_CostTrickDuringPreparingSkill(Events.OnCostTrickDuringPreparingSkill handler)
		{
			Events._handlersCostTrickDuringPreparingSkill = (Events.OnCostTrickDuringPreparingSkill)Delegate.Remove(Events._handlersCostTrickDuringPreparingSkill, handler);
		}

		// Token: 0x0600811E RID: 33054 RVA: 0x004D011F File Offset: 0x004CE31F
		public static void RaiseCostTrickDuringPreparingSkill(DataContext context, int charId)
		{
			Events.OnCostTrickDuringPreparingSkill handlersCostTrickDuringPreparingSkill = Events._handlersCostTrickDuringPreparingSkill;
			if (handlersCostTrickDuringPreparingSkill != null)
			{
				handlersCostTrickDuringPreparingSkill(context, charId);
			}
		}

		// Token: 0x0600811F RID: 33055 RVA: 0x004D0135 File Offset: 0x004CE335
		public static void RegisterHandler_CombatChangeDurability(Events.OnCombatChangeDurability handler)
		{
			Events._handlersCombatChangeDurability = (Events.OnCombatChangeDurability)Delegate.Combine(Events._handlersCombatChangeDurability, handler);
		}

		// Token: 0x06008120 RID: 33056 RVA: 0x004D014D File Offset: 0x004CE34D
		public static void UnRegisterHandler_CombatChangeDurability(Events.OnCombatChangeDurability handler)
		{
			Events._handlersCombatChangeDurability = (Events.OnCombatChangeDurability)Delegate.Remove(Events._handlersCombatChangeDurability, handler);
		}

		// Token: 0x06008121 RID: 33057 RVA: 0x004D0165 File Offset: 0x004CE365
		public static void RaiseCombatChangeDurability(DataContext context, CombatCharacter character, ItemKey itemKey, int delta)
		{
			Events.OnCombatChangeDurability handlersCombatChangeDurability = Events._handlersCombatChangeDurability;
			if (handlersCombatChangeDurability != null)
			{
				handlersCombatChangeDurability(context, character, itemKey, delta);
			}
		}

		// Token: 0x06008122 RID: 33058 RVA: 0x004D017D File Offset: 0x004CE37D
		public static void RegisterHandler_PassingLegacyWhileAdvancingMonth(Events.OnPassingLegacyWhileAdvancingMonth handler)
		{
			Events._handlersPassingLegacyWhileAdvancingMonth = (Events.OnPassingLegacyWhileAdvancingMonth)Delegate.Combine(Events._handlersPassingLegacyWhileAdvancingMonth, handler);
		}

		// Token: 0x06008123 RID: 33059 RVA: 0x004D0195 File Offset: 0x004CE395
		public static void UnRegisterHandler_PassingLegacyWhileAdvancingMonth(Events.OnPassingLegacyWhileAdvancingMonth handler)
		{
			Events._handlersPassingLegacyWhileAdvancingMonth = (Events.OnPassingLegacyWhileAdvancingMonth)Delegate.Remove(Events._handlersPassingLegacyWhileAdvancingMonth, handler);
		}

		// Token: 0x06008124 RID: 33060 RVA: 0x004D01AD File Offset: 0x004CE3AD
		public static void RaisePassingLegacyWhileAdvancingMonth(DataContext context)
		{
			Events.OnPassingLegacyWhileAdvancingMonth handlersPassingLegacyWhileAdvancingMonth = Events._handlersPassingLegacyWhileAdvancingMonth;
			if (handlersPassingLegacyWhileAdvancingMonth != null)
			{
				handlersPassingLegacyWhileAdvancingMonth(context);
			}
		}

		// Token: 0x06008125 RID: 33061 RVA: 0x004D01C2 File Offset: 0x004CE3C2
		public static void RegisterHandler_AdvanceMonthBegin(Events.OnAdvanceMonthBegin handler)
		{
			Events._handlersAdvanceMonthBegin = (Events.OnAdvanceMonthBegin)Delegate.Combine(Events._handlersAdvanceMonthBegin, handler);
		}

		// Token: 0x06008126 RID: 33062 RVA: 0x004D01DA File Offset: 0x004CE3DA
		public static void UnRegisterHandler_AdvanceMonthBegin(Events.OnAdvanceMonthBegin handler)
		{
			Events._handlersAdvanceMonthBegin = (Events.OnAdvanceMonthBegin)Delegate.Remove(Events._handlersAdvanceMonthBegin, handler);
		}

		// Token: 0x06008127 RID: 33063 RVA: 0x004D01F2 File Offset: 0x004CE3F2
		public static void RaiseAdvanceMonthBegin(DataContext context)
		{
			Events.OnAdvanceMonthBegin handlersAdvanceMonthBegin = Events._handlersAdvanceMonthBegin;
			if (handlersAdvanceMonthBegin != null)
			{
				handlersAdvanceMonthBegin(context);
			}
		}

		// Token: 0x06008128 RID: 33064 RVA: 0x004D0207 File Offset: 0x004CE407
		public static void RegisterHandler_PostAdvanceMonthBegin(Events.OnPostAdvanceMonthBegin handler)
		{
			Events._handlersPostAdvanceMonthBegin = (Events.OnPostAdvanceMonthBegin)Delegate.Combine(Events._handlersPostAdvanceMonthBegin, handler);
		}

		// Token: 0x06008129 RID: 33065 RVA: 0x004D021F File Offset: 0x004CE41F
		public static void UnRegisterHandler_PostAdvanceMonthBegin(Events.OnPostAdvanceMonthBegin handler)
		{
			Events._handlersPostAdvanceMonthBegin = (Events.OnPostAdvanceMonthBegin)Delegate.Remove(Events._handlersPostAdvanceMonthBegin, handler);
		}

		// Token: 0x0600812A RID: 33066 RVA: 0x004D0237 File Offset: 0x004CE437
		public static void RaisePostAdvanceMonthBegin(DataContext context)
		{
			Events.OnPostAdvanceMonthBegin handlersPostAdvanceMonthBegin = Events._handlersPostAdvanceMonthBegin;
			if (handlersPostAdvanceMonthBegin != null)
			{
				handlersPostAdvanceMonthBegin(context);
			}
		}

		// Token: 0x0600812B RID: 33067 RVA: 0x004D024C File Offset: 0x004CE44C
		public static void RegisterHandler_AdvanceMonthFinish(Events.OnAdvanceMonthFinish handler)
		{
			Events._handlersAdvanceMonthFinish = (Events.OnAdvanceMonthFinish)Delegate.Combine(Events._handlersAdvanceMonthFinish, handler);
		}

		// Token: 0x0600812C RID: 33068 RVA: 0x004D0264 File Offset: 0x004CE464
		public static void UnRegisterHandler_AdvanceMonthFinish(Events.OnAdvanceMonthFinish handler)
		{
			Events._handlersAdvanceMonthFinish = (Events.OnAdvanceMonthFinish)Delegate.Remove(Events._handlersAdvanceMonthFinish, handler);
		}

		// Token: 0x0600812D RID: 33069 RVA: 0x004D027C File Offset: 0x004CE47C
		public static void RaiseAdvanceMonthFinish(DataContext context)
		{
			Events.OnAdvanceMonthFinish handlersAdvanceMonthFinish = Events._handlersAdvanceMonthFinish;
			if (handlersAdvanceMonthFinish != null)
			{
				handlersAdvanceMonthFinish(context);
			}
		}

		// Token: 0x0600812E RID: 33070 RVA: 0x004D0291 File Offset: 0x004CE491
		public static void RegisterHandler_TaiwuMove(Events.OnTaiwuMove handler)
		{
			Events._handlersTaiwuMove = (Events.OnTaiwuMove)Delegate.Combine(Events._handlersTaiwuMove, handler);
		}

		// Token: 0x0600812F RID: 33071 RVA: 0x004D02A9 File Offset: 0x004CE4A9
		public static void UnRegisterHandler_TaiwuMove(Events.OnTaiwuMove handler)
		{
			Events._handlersTaiwuMove = (Events.OnTaiwuMove)Delegate.Remove(Events._handlersTaiwuMove, handler);
		}

		// Token: 0x06008130 RID: 33072 RVA: 0x004D02C1 File Offset: 0x004CE4C1
		public static void RaiseTaiwuMove(DataContext context, MapBlockData fromBlock, MapBlockData toBlock, int actionPointCost)
		{
			Events.OnTaiwuMove handlersTaiwuMove = Events._handlersTaiwuMove;
			if (handlersTaiwuMove != null)
			{
				handlersTaiwuMove(context, fromBlock, toBlock, actionPointCost);
			}
		}

		// Token: 0x06008131 RID: 33073 RVA: 0x004D02DC File Offset: 0x004CE4DC
		[CompilerGenerated]
		internal static void <RaiseBossInvasionSpeedTypeChanged>g__RemoveAllAttackingXiangshu|101_0(ref Events.<>c__DisplayClass101_0 A_0)
		{
			List<short> adventureTemplateIdsOfXiangshuAttacking = DomainManager.Adventure.GetAttackingSwordTombs();
			sbyte xiangshuLevel = DomainManager.World.GetXiangshuLevel();
			for (int i = 0; i < adventureTemplateIdsOfXiangshuAttacking.Count; i++)
			{
				short adventureTemplateId = adventureTemplateIdsOfXiangshuAttacking[i];
				sbyte xiangshuAvatarId = XiangshuAvatarIds.GetXiangshuAvatarIdBySwordTomb(adventureTemplateId);
				bool flag = xiangshuAvatarId < 0;
				if (!flag)
				{
					short xiangshuTemplateId = XiangshuAvatarIds.GetCurrentLevelXiangshuTemplateId(xiangshuAvatarId, xiangshuLevel, true);
					GameData.Domains.Character.Character character;
					bool flag2 = !DomainManager.Character.TryGetFixedCharacterByTemplateId(xiangshuTemplateId, out character);
					if (!flag2)
					{
						Events.RaiseFixedCharacterLocationChanged(A_0.context, character.GetId(), character.GetLocation(), Location.Invalid);
						character.SetLocation(Location.Invalid, A_0.context);
						DomainManager.Character.RemoveNonIntelligentCharacter(A_0.context, character);
					}
				}
			}
		}

		// Token: 0x06008132 RID: 33074 RVA: 0x004D03A8 File Offset: 0x004CE5A8
		[CompilerGenerated]
		internal static void <RaiseRelationAdded>g__TryAddMartialArtist7Seniority|107_0(OrganizationInfo orgInfo, ref Events.<>c__DisplayClass107_0 A_1)
		{
			bool flag = !OrganizationDomain.IsSect(orgInfo.OrgTemplateId) || orgInfo.Grade < 3;
			if (!flag)
			{
				bool flag2 = !DomainManager.Extra.TryTriggerAddSeniorityPoint(A_1.context, 29, A_1.charId);
				if (!flag2)
				{
					int value = ProfessionFormulaImpl.Calculate(29, (int)orgInfo.Grade);
					DomainManager.Extra.ChangeProfessionSeniority(A_1.context, 3, value, true, false);
				}
			}
		}

		// Token: 0x06008133 RID: 33075 RVA: 0x004D041C File Offset: 0x004CE61C
		[CompilerGenerated]
		internal static void <RaiseRelationAdded>g__TryAddCivilian6Seniority|107_1(OrganizationInfo orgInfo, ref Events.<>c__DisplayClass107_0 A_1)
		{
			bool flag = OrganizationDomain.IsSect(orgInfo.OrgTemplateId) || orgInfo.Grade < 3;
			if (!flag)
			{
				bool flag2 = !DomainManager.Extra.TryTriggerAddSeniorityPoint(A_1.context, 70, A_1.charId);
				if (!flag2)
				{
					int value = ProfessionFormulaImpl.Calculate(70, (int)orgInfo.Grade);
					DomainManager.Extra.ChangeProfessionSeniority(A_1.context, 10, value, true, false);
				}
			}
		}

		// Token: 0x06008134 RID: 33076 RVA: 0x004D0490 File Offset: 0x004CE690
		[CompilerGenerated]
		internal static void <RaiseRelationAdded>g__TryAddBeggar4Seniority|107_2(OrganizationInfo orgInfo, ref Events.<>c__DisplayClass107_0 A_1)
		{
			bool flag = orgInfo.Grade > 2;
			if (!flag)
			{
				bool flag2 = !DomainManager.Extra.TryTriggerAddSeniorityPoint(A_1.context, 63, A_1.charId);
				if (!flag2)
				{
					int value = ProfessionFormulaImpl.Calculate(63, (int)orgInfo.Grade);
					DomainManager.Extra.ChangeProfessionSeniority(A_1.context, 9, value, true, false);
				}
			}
		}

		// Token: 0x06008135 RID: 33077 RVA: 0x004D04F4 File Offset: 0x004CE6F4
		[CompilerGenerated]
		internal static void <RaiseRelationAdded>g__TryAddBeggar5Seniority|107_3(OrganizationInfo orgInfo, ref Events.<>c__DisplayClass107_0 A_1)
		{
			bool flag = orgInfo.Grade < 6;
			if (!flag)
			{
				bool flag2 = !DomainManager.Extra.TryTriggerAddSeniorityPoint(A_1.context, 64, A_1.charId);
				if (!flag2)
				{
					int value = ProfessionFormulaImpl.Calculate(64, (int)orgInfo.Grade);
					DomainManager.Extra.ChangeProfessionSeniority(A_1.context, 9, value, true, false);
				}
			}
		}

		// Token: 0x04002322 RID: 8994
		private static Events.OnCharacterLocationChanged _handlersCharacterLocationChanged;

		// Token: 0x04002323 RID: 8995
		private static Events.OnMakeLove _handlersMakeLove;

		// Token: 0x04002324 RID: 8996
		private static Events.OnEatingItem _handlersEatingItem;

		// Token: 0x04002325 RID: 8997
		private static Events.OnXiangshuInfectionFeatureChangedEnd _handlersXiangshuInfectionFeatureChangedEnd;

		// Token: 0x04002326 RID: 8998
		private static Events.OnCombatBegin _handlersCombatBegin;

		// Token: 0x04002327 RID: 8999
		private static Events.OnCombatSettlement _handlersCombatSettlement;

		// Token: 0x04002328 RID: 9000
		private static Events.OnCombatEnd _handlersCombatEnd;

		// Token: 0x04002329 RID: 9001
		private static Events.OnChangeNeiliAllocationAfterCombatBegin _handlersChangeNeiliAllocationAfterCombatBegin;

		// Token: 0x0400232A RID: 9002
		private static Events.OnCreateGangqiAfterChangeNeiliAllocation _handlersCreateGangqiAfterChangeNeiliAllocation;

		// Token: 0x0400232B RID: 9003
		private static Events.OnChangeBossPhase _handlersChangeBossPhase;

		// Token: 0x0400232C RID: 9004
		private static Events.OnGetTrick _handlersGetTrick;

		// Token: 0x0400232D RID: 9005
		private static Events.OnRearrangeTrick _handlersRearrangeTrick;

		// Token: 0x0400232E RID: 9006
		private static Events.OnGetShaTrick _handlersGetShaTrick;

		// Token: 0x0400232F RID: 9007
		private static Events.OnRemoveShaTrick _handlersRemoveShaTrick;

		// Token: 0x04002330 RID: 9008
		private static Events.OnOverflowTrickRemoved _handlersOverflowTrickRemoved;

		// Token: 0x04002331 RID: 9009
		private static Events.OnCostBreathAndStance _handlersCostBreathAndStance;

		// Token: 0x04002332 RID: 9010
		private static Events.OnChangeWeapon _handlersChangeWeapon;

		// Token: 0x04002333 RID: 9011
		private static Events.OnWeaponCdEnd _handlersWeaponCdEnd;

		// Token: 0x04002334 RID: 9012
		private static Events.OnChangeTrickCountChanged _handlersChangeTrickCountChanged;

		// Token: 0x04002335 RID: 9013
		private static Events.OnUnlockAttack _handlersUnlockAttack;

		// Token: 0x04002336 RID: 9014
		private static Events.OnUnlockAttackEnd _handlersUnlockAttackEnd;

		// Token: 0x04002337 RID: 9015
		private static Events.OnNormalAttackPrepareEnd _handlersNormalAttackPrepareEnd;

		// Token: 0x04002338 RID: 9016
		private static Events.OnNormalAttackOutOfRange _handlersNormalAttackOutOfRange;

		// Token: 0x04002339 RID: 9017
		private static Events.OnNormalAttackBegin _handlersNormalAttackBegin;

		// Token: 0x0400233A RID: 9018
		private static Events.OnNormalAttackCalcHitEnd _handlersNormalAttackCalcHitEnd;

		// Token: 0x0400233B RID: 9019
		private static Events.OnNormalAttackCalcCriticalEnd _handlersNormalAttackCalcCriticalEnd;

		// Token: 0x0400233C RID: 9020
		private static Events.OnNormalAttackEnd _handlersNormalAttackEnd;

		// Token: 0x0400233D RID: 9021
		private static Events.OnNormalAttackAllEnd _handlersNormalAttackAllEnd;

		// Token: 0x0400233E RID: 9022
		private static Events.OnCastSkillUseExtraBreathOrStance _handlersCastSkillUseExtraBreathOrStance;

		// Token: 0x0400233F RID: 9023
		private static Events.OnCastSkillUseMobilityAsBreathOrStance _handlersCastSkillUseMobilityAsBreathOrStance;

		// Token: 0x04002340 RID: 9024
		private static Events.OnCastLegSkillWithAgile _handlersCastLegSkillWithAgile;

		// Token: 0x04002341 RID: 9025
		private static Events.OnCastSkillOnLackBreathStance _handlersCastSkillOnLackBreathStance;

		// Token: 0x04002342 RID: 9026
		private static Events.OnCastSkillTrickCosted _handlersCastSkillTrickCosted;

		// Token: 0x04002343 RID: 9027
		private static Events.OnJiTrickInsteadCostTricks _handlersJiTrickInsteadCostTricks;

		// Token: 0x04002344 RID: 9028
		private static Events.OnUselessTrickInsteadJiTricks _handlersUselessTrickInsteadJiTricks;

		// Token: 0x04002345 RID: 9029
		private static Events.OnShaTrickInsteadCostTricks _handlersShaTrickInsteadCostTricks;

		// Token: 0x04002346 RID: 9030
		private static Events.OnCastSkillCosted _handlersCastSkillCosted;

		// Token: 0x04002347 RID: 9031
		private static Events.OnChangePreparingSkillBegin _handlersChangePreparingSkillBegin;

		// Token: 0x04002348 RID: 9032
		private static Events.OnCastAgileOrDefenseWithoutPrepareBegin _handlersCastAgileOrDefenseWithoutPrepareBegin;

		// Token: 0x04002349 RID: 9033
		private static Events.OnCastAgileOrDefenseWithoutPrepareEnd _handlersCastAgileOrDefenseWithoutPrepareEnd;

		// Token: 0x0400234A RID: 9034
		private static Events.OnPrepareSkillEffectNotYetCreated _handlersPrepareSkillEffectNotYetCreated;

		// Token: 0x0400234B RID: 9035
		private static Events.OnPrepareSkillBegin _handlersPrepareSkillBegin;

		// Token: 0x0400234C RID: 9036
		private static Events.OnPrepareSkillProgressChange _handlersPrepareSkillProgressChange;

		// Token: 0x0400234D RID: 9037
		private static Events.OnPrepareSkillChangeDistance _handlersPrepareSkillChangeDistance;

		// Token: 0x0400234E RID: 9038
		private static Events.OnPrepareSkillEnd _handlersPrepareSkillEnd;

		// Token: 0x0400234F RID: 9039
		private static Events.OnCastAttackSkillBegin _handlersCastAttackSkillBegin;

		// Token: 0x04002350 RID: 9040
		private static Events.OnAttackSkillAttackBegin _handlersAttackSkillAttackBegin;

		// Token: 0x04002351 RID: 9041
		private static Events.OnAttackSkillAttackHit _handlersAttackSkillAttackHit;

		// Token: 0x04002352 RID: 9042
		private static Events.OnAttackSkillAttackEnd _handlersAttackSkillAttackEnd;

		// Token: 0x04002353 RID: 9043
		private static Events.OnAttackSkillAttackEndOfAll _handlersAttackSkillAttackEndOfAll;

		// Token: 0x04002354 RID: 9044
		private static Events.OnCastSkillEnd _handlersCastSkillEnd;

		// Token: 0x04002355 RID: 9045
		private static Events.OnCastSkillAllEnd _handlersCastSkillAllEnd;

		// Token: 0x04002356 RID: 9046
		private static Events.OnCalcLeveragingValue _handlersCalcLeveragingValue;

		// Token: 0x04002357 RID: 9047
		private static Events.OnWisdomCosted _handlersWisdomCosted;

		// Token: 0x04002358 RID: 9048
		private static Events.OnHealedInjury _handlersHealedInjury;

		// Token: 0x04002359 RID: 9049
		private static Events.OnHealedPoison _handlersHealedPoison;

		// Token: 0x0400235A RID: 9050
		private static Events.OnUsedMedicine _handlersUsedMedicine;

		// Token: 0x0400235B RID: 9051
		private static Events.OnUsedCustomItem _handlersUsedCustomItem;

		// Token: 0x0400235C RID: 9052
		private static Events.OnInterruptOtherAction _handlersInterruptOtherAction;

		// Token: 0x0400235D RID: 9053
		private static Events.OnFlawAdded _handlersFlawAdded;

		// Token: 0x0400235E RID: 9054
		private static Events.OnFlawRemoved _handlersFlawRemoved;

		// Token: 0x0400235F RID: 9055
		private static Events.OnAcuPointAdded _handlersAcuPointAdded;

		// Token: 0x04002360 RID: 9056
		private static Events.OnAcuPointRemoved _handlersAcuPointRemoved;

		// Token: 0x04002361 RID: 9057
		private static Events.OnCombatCharChanged _handlersCombatCharChanged;

		// Token: 0x04002362 RID: 9058
		private static Events.OnAddInjury _handlersAddInjury;

		// Token: 0x04002363 RID: 9059
		private static Events.OnAddDirectDamageValue _handlersAddDirectDamageValue;

		// Token: 0x04002364 RID: 9060
		private static Events.OnAddDirectInjury _handlersAddDirectInjury;

		// Token: 0x04002365 RID: 9061
		private static Events.OnBounceInjury _handlersBounceInjury;

		// Token: 0x04002366 RID: 9062
		private static Events.OnAddMindMark _handlersAddMindMark;

		// Token: 0x04002367 RID: 9063
		private static Events.OnAddMindDamage _handlersAddMindDamage;

		// Token: 0x04002368 RID: 9064
		private static Events.OnAddFatalDamageMark _handlersAddFatalDamageMark;

		// Token: 0x04002369 RID: 9065
		private static Events.OnAddDirectFatalDamageMark _handlersAddDirectFatalDamageMark;

		// Token: 0x0400236A RID: 9066
		private static Events.OnAddDirectFatalDamage _handlersAddDirectFatalDamage;

		// Token: 0x0400236B RID: 9067
		private static Events.OnAddDirectPoisonMark _handlersAddDirectPoisonMark;

		// Token: 0x0400236C RID: 9068
		private static Events.OnMoveStateChanged _handlersMoveStateChanged;

		// Token: 0x0400236D RID: 9069
		private static Events.OnMoveBegin _handlersMoveBegin;

		// Token: 0x0400236E RID: 9070
		private static Events.OnMoveEnd _handlersMoveEnd;

		// Token: 0x0400236F RID: 9071
		private static Events.OnIgnoredForceChangeDistance _handlersIgnoredForceChangeDistance;

		// Token: 0x04002370 RID: 9072
		private static Events.OnDistanceChanged _handlersDistanceChanged;

		// Token: 0x04002371 RID: 9073
		private static Events.OnSkillEffectChange _handlersSkillEffectChange;

		// Token: 0x04002372 RID: 9074
		private static Events.OnSkillSilence _handlersSkillSilence;

		// Token: 0x04002373 RID: 9075
		private static Events.OnSkillSilenceEnd _handlersSkillSilenceEnd;

		// Token: 0x04002374 RID: 9076
		private static Events.OnNeiliAllocationChanged _handlersNeiliAllocationChanged;

		// Token: 0x04002375 RID: 9077
		private static Events.OnAddPoison _handlersAddPoison;

		// Token: 0x04002376 RID: 9078
		private static Events.OnPoisonAffected _handlersPoisonAffected;

		// Token: 0x04002377 RID: 9079
		private static Events.OnAddWug _handlersAddWug;

		// Token: 0x04002378 RID: 9080
		private static Events.OnRemoveWug _handlersRemoveWug;

		// Token: 0x04002379 RID: 9081
		private static Events.OnCompareDataCalcFinished _handlersCompareDataCalcFinished;

		// Token: 0x0400237A RID: 9082
		private static Events.OnCombatStateMachineUpdateEnd _handlersCombatStateMachineUpdateEnd;

		// Token: 0x0400237B RID: 9083
		private static Events.OnCombatCharFallen _handlersCombatCharFallen;

		// Token: 0x0400237C RID: 9084
		private static Events.OnCombatCostNeiliConfirm _handlersCombatCostNeiliConfirm;

		// Token: 0x0400237D RID: 9085
		private static Events.OnCostTrickDuringPreparingSkill _handlersCostTrickDuringPreparingSkill;

		// Token: 0x0400237E RID: 9086
		private static Events.OnCombatChangeDurability _handlersCombatChangeDurability;

		// Token: 0x0400237F RID: 9087
		private static Events.OnPassingLegacyWhileAdvancingMonth _handlersPassingLegacyWhileAdvancingMonth;

		// Token: 0x04002380 RID: 9088
		private static Events.OnAdvanceMonthBegin _handlersAdvanceMonthBegin;

		// Token: 0x04002381 RID: 9089
		private static Events.OnPostAdvanceMonthBegin _handlersPostAdvanceMonthBegin;

		// Token: 0x04002382 RID: 9090
		private static Events.OnAdvanceMonthFinish _handlersAdvanceMonthFinish;

		// Token: 0x04002383 RID: 9091
		private static Events.OnTaiwuMove _handlersTaiwuMove;

		// Token: 0x02000CCA RID: 3274
		// (Invoke) Token: 0x06009029 RID: 36905
		public delegate void OnCharacterLocationChanged(DataContext context, int charId, Location srcLocation, Location destLocation);

		// Token: 0x02000CCB RID: 3275
		// (Invoke) Token: 0x0600902D RID: 36909
		public delegate void OnMakeLove(DataContext context, GameData.Domains.Character.Character character, GameData.Domains.Character.Character target, sbyte makeLoveState);

		// Token: 0x02000CCC RID: 3276
		// (Invoke) Token: 0x06009031 RID: 36913
		public delegate void OnEatingItem(DataContext context, GameData.Domains.Character.Character character, ItemKey itemKey);

		// Token: 0x02000CCD RID: 3277
		// (Invoke) Token: 0x06009035 RID: 36917
		public delegate void OnXiangshuInfectionFeatureChangedEnd(DataContext context, GameData.Domains.Character.Character character, short featureId);

		// Token: 0x02000CCE RID: 3278
		// (Invoke) Token: 0x06009039 RID: 36921
		public delegate void OnCombatBegin(DataContext context);

		// Token: 0x02000CCF RID: 3279
		// (Invoke) Token: 0x0600903D RID: 36925
		public delegate void OnCombatSettlement(DataContext context, sbyte combatStatus);

		// Token: 0x02000CD0 RID: 3280
		// (Invoke) Token: 0x06009041 RID: 36929
		public delegate void OnCombatEnd(DataContext context);

		// Token: 0x02000CD1 RID: 3281
		// (Invoke) Token: 0x06009045 RID: 36933
		public delegate void OnChangeNeiliAllocationAfterCombatBegin(DataContext context, CombatCharacter character, NeiliAllocation allocationAfterBegin);

		// Token: 0x02000CD2 RID: 3282
		// (Invoke) Token: 0x06009049 RID: 36937
		public delegate void OnCreateGangqiAfterChangeNeiliAllocation(DataContext context, CombatCharacter character);

		// Token: 0x02000CD3 RID: 3283
		// (Invoke) Token: 0x0600904D RID: 36941
		public delegate void OnChangeBossPhase(DataContext context);

		// Token: 0x02000CD4 RID: 3284
		// (Invoke) Token: 0x06009051 RID: 36945
		[DomainEvent(MaxReenterCount = 1)]
		public delegate void OnGetTrick(DataContext context, int charId, bool isAlly, sbyte trickType, bool usable);

		// Token: 0x02000CD5 RID: 3285
		// (Invoke) Token: 0x06009055 RID: 36949
		public delegate void OnRearrangeTrick(DataContext context, int charId, bool isAlly);

		// Token: 0x02000CD6 RID: 3286
		// (Invoke) Token: 0x06009059 RID: 36953
		[DomainEvent(MaxReenterCount = 2)]
		public delegate void OnGetShaTrick(DataContext context, int charId, bool isAlly, bool real);

		// Token: 0x02000CD7 RID: 3287
		// (Invoke) Token: 0x0600905D RID: 36957
		public delegate void OnRemoveShaTrick(DataContext context, int charId);

		// Token: 0x02000CD8 RID: 3288
		// (Invoke) Token: 0x06009061 RID: 36961
		public delegate void OnOverflowTrickRemoved(DataContext context, int charId, bool isAlly, int removedCount);

		// Token: 0x02000CD9 RID: 3289
		// (Invoke) Token: 0x06009065 RID: 36965
		public delegate void OnCostBreathAndStance(DataContext context, int charId, bool isAlly, int costBreath, int costStance, short skillId);

		// Token: 0x02000CDA RID: 3290
		// (Invoke) Token: 0x06009069 RID: 36969
		public delegate void OnChangeWeapon(DataContext context, int charId, bool isAlly, CombatWeaponData newWeapon, CombatWeaponData oldWeapon);

		// Token: 0x02000CDB RID: 3291
		// (Invoke) Token: 0x0600906D RID: 36973
		public delegate void OnWeaponCdEnd(DataContext context, int charId, bool isAlly, CombatWeaponData weapon);

		// Token: 0x02000CDC RID: 3292
		// (Invoke) Token: 0x06009071 RID: 36977
		public delegate void OnChangeTrickCountChanged(DataContext context, CombatCharacter character, int addValue, bool bySelectChangeTrick);

		// Token: 0x02000CDD RID: 3293
		// (Invoke) Token: 0x06009075 RID: 36981
		public delegate void OnUnlockAttack(DataContext context, CombatCharacter combatChar, int weaponIndex);

		// Token: 0x02000CDE RID: 3294
		// (Invoke) Token: 0x06009079 RID: 36985
		public delegate void OnUnlockAttackEnd(DataContext context, CombatCharacter attacker);

		// Token: 0x02000CDF RID: 3295
		// (Invoke) Token: 0x0600907D RID: 36989
		public delegate void OnNormalAttackPrepareEnd(DataContext context, int charId, bool isAlly);

		// Token: 0x02000CE0 RID: 3296
		// (Invoke) Token: 0x06009081 RID: 36993
		public delegate void OnNormalAttackOutOfRange(DataContext context, int charId, bool isAlly);

		// Token: 0x02000CE1 RID: 3297
		// (Invoke) Token: 0x06009085 RID: 36997
		public delegate void OnNormalAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex);

		// Token: 0x02000CE2 RID: 3298
		// (Invoke) Token: 0x06009089 RID: 37001
		public delegate void OnNormalAttackCalcHitEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, int pursueIndex, bool hit, bool isFightBack, bool isMind);

		// Token: 0x02000CE3 RID: 3299
		// (Invoke) Token: 0x0600908D RID: 37005
		public delegate void OnNormalAttackCalcCriticalEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, bool critical);

		// Token: 0x02000CE4 RID: 3300
		// (Invoke) Token: 0x06009091 RID: 37009
		public delegate void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack);

		// Token: 0x02000CE5 RID: 3301
		// (Invoke) Token: 0x06009095 RID: 37013
		public delegate void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender);

		// Token: 0x02000CE6 RID: 3302
		// (Invoke) Token: 0x06009099 RID: 37017
		public delegate void OnCastSkillUseExtraBreathOrStance(DataContext context, int charId, short skillId, int extraBreath, int extraStance);

		// Token: 0x02000CE7 RID: 3303
		// (Invoke) Token: 0x0600909D RID: 37021
		public delegate void OnCastSkillUseMobilityAsBreathOrStance(DataContext context, int charId, short skillId, bool asBreath);

		// Token: 0x02000CE8 RID: 3304
		// (Invoke) Token: 0x060090A1 RID: 37025
		public delegate void OnCastLegSkillWithAgile(DataContext context, CombatCharacter combatChar, short legSkillId);

		// Token: 0x02000CE9 RID: 3305
		// (Invoke) Token: 0x060090A5 RID: 37029
		public delegate void OnCastSkillOnLackBreathStance(DataContext context, CombatCharacter combatChar, short skillId, int lackBreath, int lackStance, int costBreath, int costStance);

		// Token: 0x02000CEA RID: 3306
		// (Invoke) Token: 0x060090A9 RID: 37033
		public delegate void OnCastSkillTrickCosted(DataContext context, CombatCharacter combatChar, short skillId, List<NeedTrick> costTricks);

		// Token: 0x02000CEB RID: 3307
		// (Invoke) Token: 0x060090AD RID: 37037
		public delegate void OnJiTrickInsteadCostTricks(DataContext context, CombatCharacter character, int count);

		// Token: 0x02000CEC RID: 3308
		// (Invoke) Token: 0x060090B1 RID: 37041
		public delegate void OnUselessTrickInsteadJiTricks(DataContext context, CombatCharacter character, int count);

		// Token: 0x02000CED RID: 3309
		// (Invoke) Token: 0x060090B5 RID: 37045
		public delegate void OnShaTrickInsteadCostTricks(DataContext context, CombatCharacter character, short skillId);

		// Token: 0x02000CEE RID: 3310
		// (Invoke) Token: 0x060090B9 RID: 37049
		public delegate void OnCastSkillCosted(DataContext context, CombatCharacter combatChar, short skillId);

		// Token: 0x02000CEF RID: 3311
		// (Invoke) Token: 0x060090BD RID: 37053
		public delegate void OnChangePreparingSkillBegin(DataContext context, int charId, short prevSkillId, short currSkillId);

		// Token: 0x02000CF0 RID: 3312
		// (Invoke) Token: 0x060090C1 RID: 37057
		public delegate void OnCastAgileOrDefenseWithoutPrepareBegin(DataContext context, int charId, short skillId);

		// Token: 0x02000CF1 RID: 3313
		// (Invoke) Token: 0x060090C5 RID: 37061
		public delegate void OnCastAgileOrDefenseWithoutPrepareEnd(DataContext context, int charId, short skillId);

		// Token: 0x02000CF2 RID: 3314
		// (Invoke) Token: 0x060090C9 RID: 37065
		public delegate void OnPrepareSkillEffectNotYetCreated(DataContext context, CombatCharacter character, short skillId);

		// Token: 0x02000CF3 RID: 3315
		// (Invoke) Token: 0x060090CD RID: 37069
		public delegate void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId);

		// Token: 0x02000CF4 RID: 3316
		// (Invoke) Token: 0x060090D1 RID: 37073
		public delegate void OnPrepareSkillProgressChange(DataContext context, int charId, bool isAlly, short skillId, sbyte preparePercent);

		// Token: 0x02000CF5 RID: 3317
		// (Invoke) Token: 0x060090D5 RID: 37077
		public delegate void OnPrepareSkillChangeDistance(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId);

		// Token: 0x02000CF6 RID: 3318
		// (Invoke) Token: 0x060090D9 RID: 37081
		public delegate void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId);

		// Token: 0x02000CF7 RID: 3319
		// (Invoke) Token: 0x060090DD RID: 37085
		public delegate void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId);

		// Token: 0x02000CF8 RID: 3320
		// (Invoke) Token: 0x060090E1 RID: 37089
		public delegate void OnAttackSkillAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId, int index, bool hit);

		// Token: 0x02000CF9 RID: 3321
		// (Invoke) Token: 0x060090E5 RID: 37093
		public delegate void OnAttackSkillAttackHit(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId, int index, bool critical);

		// Token: 0x02000CFA RID: 3322
		// (Invoke) Token: 0x060090E9 RID: 37097
		public delegate void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index);

		// Token: 0x02000CFB RID: 3323
		// (Invoke) Token: 0x060090ED RID: 37101
		public delegate void OnAttackSkillAttackEndOfAll(DataContext context, CombatCharacter character, int index);

		// Token: 0x02000CFC RID: 3324
		// (Invoke) Token: 0x060090F1 RID: 37105
		[DomainEvent(MaxReenterCount = 1)]
		public delegate void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted);

		// Token: 0x02000CFD RID: 3325
		// (Invoke) Token: 0x060090F5 RID: 37109
		public delegate void OnCastSkillAllEnd(DataContext context, int charId, short skillId);

		// Token: 0x02000CFE RID: 3326
		// (Invoke) Token: 0x060090F9 RID: 37113
		public delegate void OnCalcLeveragingValue(CombatContext context, sbyte hitType, bool hit, int index);

		// Token: 0x02000CFF RID: 3327
		// (Invoke) Token: 0x060090FD RID: 37117
		public delegate void OnWisdomCosted(DataContext context, bool isAlly, int value);

		// Token: 0x02000D00 RID: 3328
		// (Invoke) Token: 0x06009101 RID: 37121
		public delegate void OnHealedInjury(DataContext context, int doctorId, int patientId, bool isAlly, sbyte healMarkCount);

		// Token: 0x02000D01 RID: 3329
		// (Invoke) Token: 0x06009105 RID: 37125
		public delegate void OnHealedPoison(DataContext context, int doctorId, int patientId, bool isAlly, sbyte healMarkCount);

		// Token: 0x02000D02 RID: 3330
		// (Invoke) Token: 0x06009109 RID: 37129
		public delegate void OnUsedMedicine(DataContext context, int charId, ItemKey itemKey);

		// Token: 0x02000D03 RID: 3331
		// (Invoke) Token: 0x0600910D RID: 37133
		public delegate void OnUsedCustomItem(DataContext context, int charId, ItemKey itemKey);

		// Token: 0x02000D04 RID: 3332
		// (Invoke) Token: 0x06009111 RID: 37137
		public delegate void OnInterruptOtherAction(DataContext context, CombatCharacter combatChar, sbyte otherActionType);

		// Token: 0x02000D05 RID: 3333
		// (Invoke) Token: 0x06009115 RID: 37141
		public delegate void OnFlawAdded(DataContext context, CombatCharacter combatChar, sbyte bodyPart, sbyte level);

		// Token: 0x02000D06 RID: 3334
		// (Invoke) Token: 0x06009119 RID: 37145
		public delegate void OnFlawRemoved(DataContext context, CombatCharacter combatChar, sbyte bodyPart, sbyte level);

		// Token: 0x02000D07 RID: 3335
		// (Invoke) Token: 0x0600911D RID: 37149
		[DomainEvent(MaxReenterCount = 2)]
		public delegate void OnAcuPointAdded(DataContext context, CombatCharacter combatChar, sbyte bodyPart, sbyte level);

		// Token: 0x02000D08 RID: 3336
		// (Invoke) Token: 0x06009121 RID: 37153
		public delegate void OnAcuPointRemoved(DataContext context, CombatCharacter combatChar, sbyte bodyPart, sbyte level);

		// Token: 0x02000D09 RID: 3337
		// (Invoke) Token: 0x06009125 RID: 37157
		public delegate void OnCombatCharChanged(DataContext context, bool isAlly);

		// Token: 0x02000D0A RID: 3338
		// (Invoke) Token: 0x06009129 RID: 37161
		public delegate void OnAddInjury(DataContext context, CombatCharacter character, sbyte bodyPart, bool isInner, sbyte value, bool changeToOld);

		// Token: 0x02000D0B RID: 3339
		// (Invoke) Token: 0x0600912D RID: 37165
		public delegate void OnAddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId);

		// Token: 0x02000D0C RID: 3340
		// (Invoke) Token: 0x06009131 RID: 37169
		public delegate void OnAddDirectInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount, short combatSkillId);

		// Token: 0x02000D0D RID: 3341
		// (Invoke) Token: 0x06009135 RID: 37173
		public delegate void OnBounceInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount);

		// Token: 0x02000D0E RID: 3342
		// (Invoke) Token: 0x06009139 RID: 37177
		[DomainEvent(MaxReenterCount = 1)]
		public delegate void OnAddMindMark(DataContext context, CombatCharacter character, int count);

		// Token: 0x02000D0F RID: 3343
		// (Invoke) Token: 0x0600913D RID: 37181
		public delegate void OnAddMindDamage(DataContext context, int attackerId, int defenderId, int damageValue, int markCount, short combatSkillId);

		// Token: 0x02000D10 RID: 3344
		// (Invoke) Token: 0x06009141 RID: 37185
		public delegate void OnAddFatalDamageMark(DataContext context, CombatCharacter combatChar, int count);

		// Token: 0x02000D11 RID: 3345
		// (Invoke) Token: 0x06009145 RID: 37189
		public delegate void OnAddDirectFatalDamageMark(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, int outerMarkCount, int innerMarkCount, short combatSkillId);

		// Token: 0x02000D12 RID: 3346
		// (Invoke) Token: 0x06009149 RID: 37193
		public delegate void OnAddDirectFatalDamage(CombatContext context, int outer, int inner);

		// Token: 0x02000D13 RID: 3347
		// (Invoke) Token: 0x0600914D RID: 37197
		public delegate void OnAddDirectPoisonMark(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte poisonType, short skillId, int markCount);

		// Token: 0x02000D14 RID: 3348
		// (Invoke) Token: 0x06009151 RID: 37201
		public delegate void OnMoveStateChanged(DataContext context, CombatCharacter character, byte moveState);

		// Token: 0x02000D15 RID: 3349
		// (Invoke) Token: 0x06009155 RID: 37205
		public delegate void OnMoveBegin(DataContext context, CombatCharacter mover, int distance, bool isJump);

		// Token: 0x02000D16 RID: 3350
		// (Invoke) Token: 0x06009159 RID: 37209
		public delegate void OnMoveEnd(DataContext context, CombatCharacter mover, int distance, bool isJump);

		// Token: 0x02000D17 RID: 3351
		// (Invoke) Token: 0x0600915D RID: 37213
		public delegate void OnIgnoredForceChangeDistance(DataContext context, CombatCharacter mover, int distance);

		// Token: 0x02000D18 RID: 3352
		// (Invoke) Token: 0x06009161 RID: 37217
		[DomainEvent(MaxReenterCount = 7)]
		public delegate void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced);

		// Token: 0x02000D19 RID: 3353
		// (Invoke) Token: 0x06009165 RID: 37221
		[DomainEvent(MaxReenterCount = 1)]
		public delegate void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed);

		// Token: 0x02000D1A RID: 3354
		// (Invoke) Token: 0x06009169 RID: 37225
		public delegate void OnSkillSilence(DataContext context, CombatSkillKey skillKey);

		// Token: 0x02000D1B RID: 3355
		// (Invoke) Token: 0x0600916D RID: 37229
		public delegate void OnSkillSilenceEnd(DataContext context, CombatSkillKey skillKey);

		// Token: 0x02000D1C RID: 3356
		// (Invoke) Token: 0x06009171 RID: 37233
		[DomainEvent(MaxReenterCount = 1)]
		public delegate void OnNeiliAllocationChanged(DataContext context, int charId, byte type, int changeValue);

		// Token: 0x02000D1D RID: 3357
		// (Invoke) Token: 0x06009175 RID: 37237
		public delegate void OnAddPoison(DataContext context, int attackerId, int defenderId, sbyte poisonType, sbyte level, int addValue, short skillId, bool canBounce);

		// Token: 0x02000D1E RID: 3358
		// (Invoke) Token: 0x06009179 RID: 37241
		public delegate void OnPoisonAffected(DataContext context, int charId, sbyte poisonType);

		// Token: 0x02000D1F RID: 3359
		// (Invoke) Token: 0x0600917D RID: 37245
		[DomainEvent(MaxReenterCount = 1)]
		public delegate void OnAddWug(DataContext context, int charId, short wugTemplateId, short replacedWug);

		// Token: 0x02000D20 RID: 3360
		// (Invoke) Token: 0x06009181 RID: 37249
		public delegate void OnRemoveWug(DataContext context, int charId, short wugTemplateId);

		// Token: 0x02000D21 RID: 3361
		// (Invoke) Token: 0x06009185 RID: 37253
		public delegate void OnCompareDataCalcFinished(CombatContext context, DamageCompareData compareData);

		// Token: 0x02000D22 RID: 3362
		// (Invoke) Token: 0x06009189 RID: 37257
		public delegate void OnCombatStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar);

		// Token: 0x02000D23 RID: 3363
		// (Invoke) Token: 0x0600918D RID: 37261
		public delegate void OnCombatCharFallen(DataContext context, CombatCharacter combatChar);

		// Token: 0x02000D24 RID: 3364
		// (Invoke) Token: 0x06009191 RID: 37265
		public delegate void OnCombatCostNeiliConfirm(DataContext context, int charId, short skillId, short effectId);

		// Token: 0x02000D25 RID: 3365
		// (Invoke) Token: 0x06009195 RID: 37269
		public delegate void OnCostTrickDuringPreparingSkill(DataContext context, int charId);

		// Token: 0x02000D26 RID: 3366
		// (Invoke) Token: 0x06009199 RID: 37273
		public delegate void OnCombatChangeDurability(DataContext context, CombatCharacter character, ItemKey itemKey, int delta);

		// Token: 0x02000D27 RID: 3367
		// (Invoke) Token: 0x0600919D RID: 37277
		public delegate void OnPassingLegacyWhileAdvancingMonth(DataContext context);

		// Token: 0x02000D28 RID: 3368
		// (Invoke) Token: 0x060091A1 RID: 37281
		public delegate void OnAdvanceMonthBegin(DataContext context);

		// Token: 0x02000D29 RID: 3369
		// (Invoke) Token: 0x060091A5 RID: 37285
		public delegate void OnPostAdvanceMonthBegin(DataContext context);

		// Token: 0x02000D2A RID: 3370
		// (Invoke) Token: 0x060091A9 RID: 37289
		public delegate void OnAdvanceMonthFinish(DataContext context);

		// Token: 0x02000D2B RID: 3371
		// (Invoke) Token: 0x060091AD RID: 37293
		public delegate void OnTaiwuMove(DataContext context, MapBlockData fromBlock, MapBlockData toBlock, int actionPointCost);
	}
}
