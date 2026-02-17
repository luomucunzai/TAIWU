using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Dependencies;
using GameData.DomainEvents;
using GameData.Domains.Adventure;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.Extra;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.TaiwuEvent.MonthlyEventActions.CustomActions;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.LegendaryBook
{
	// Token: 0x02000658 RID: 1624
	[GameDataDomain(11)]
	public class LegendaryBookDomain : BaseGameDataDomain
	{
		// Token: 0x06004D5A RID: 19802 RVA: 0x002AAC5C File Offset: 0x002A8E5C
		public void InitializeOwnedItems()
		{
			for (int index = 0; index < this._legendaryBookMonthlyActions.Length; index++)
			{
				LegendaryBookMonthlyAction action = this._legendaryBookMonthlyActions[index];
				bool flag = action == null;
				if (!flag)
				{
					ItemKey bookItemKey = this._legendaryBookItems[(int)action.BookType];
					DomainManager.Item.SetOwner(bookItemKey, ItemOwnerType.System, 11);
				}
			}
		}

		// Token: 0x06004D5B RID: 19803 RVA: 0x002AACB9 File Offset: 0x002A8EB9
		private void OnInitializedDomainData()
		{
			this.InitializeLegendaryBookItems();
		}

		// Token: 0x06004D5C RID: 19804 RVA: 0x002AACC3 File Offset: 0x002A8EC3
		private void InitializeOnInitializeGameDataModule()
		{
		}

		// Token: 0x06004D5D RID: 19805 RVA: 0x002AACC8 File Offset: 0x002A8EC8
		private void InitializeOnEnterNewWorld()
		{
			for (int i = 0; i < 14; i++)
			{
				this._bookOwners[i] = -1;
			}
			this.InitializeCharBookTypes();
		}

		// Token: 0x06004D5E RID: 19806 RVA: 0x002AACF7 File Offset: 0x002A8EF7
		private void OnLoadedArchiveData()
		{
			this.InitializeCharBookTypes();
		}

		// Token: 0x06004D5F RID: 19807 RVA: 0x002AAD04 File Offset: 0x002A8F04
		public override void FixAbnormalDomainArchiveData(DataContext context)
		{
			bool flag = DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 71, 58);
			if (flag)
			{
				this.FixAbnormalLostLegendaryBooksByThreeCorpsesGiveUpAction(context);
			}
		}

		// Token: 0x06004D60 RID: 19808 RVA: 0x002AAD30 File Offset: 0x002A8F30
		private void FixAbnormalLostLegendaryBooksByThreeCorpsesGiveUpAction(DataContext context)
		{
			bool flag;
			if (DomainManager.World.GetSectMainStoryTaskStatus(7) == 1)
			{
				SectStoryThreeCorpsesCharacter ranshanThreeCorpsesCharacterByTemplateId = DomainManager.Extra.GetRanshanThreeCorpsesCharacterByTemplateId(660);
				if (ranshanThreeCorpsesCharacterByTemplateId == null || !ranshanThreeCorpsesCharacterByTemplateId.IsGoodEnd)
				{
					ranshanThreeCorpsesCharacterByTemplateId = DomainManager.Extra.GetRanshanThreeCorpsesCharacterByTemplateId(661);
					if (ranshanThreeCorpsesCharacterByTemplateId == null || !ranshanThreeCorpsesCharacterByTemplateId.IsGoodEnd)
					{
						ranshanThreeCorpsesCharacterByTemplateId = DomainManager.Extra.GetRanshanThreeCorpsesCharacterByTemplateId(662);
						flag = (ranshanThreeCorpsesCharacterByTemplateId != null && ranshanThreeCorpsesCharacterByTemplateId.IsGoodEnd);
						goto IL_64;
					}
				}
				flag = true;
				IL_64:;
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				for (sbyte combatSkillType = 0; combatSkillType < 14; combatSkillType += 1)
				{
					bool flag3 = this._legendaryBookItems[(int)combatSkillType] != ItemKey.Invalid && DomainManager.Item.GetBaseItem(this._legendaryBookItems[(int)combatSkillType]).Owner.OwnerType == ItemOwnerType.None;
					if (flag3)
					{
						short areaId = (short)context.Random.Next(45);
						MapBlockData blockData = DomainManager.Map.GetRandomMapBlockDataInAreaByFilters(context.Random, areaId, null, false);
						LegendaryBookMonthlyAction action = new LegendaryBookMonthlyAction
						{
							Location = blockData.GetLocation(),
							BookType = combatSkillType,
							BookAppearType = 0
						};
						DomainManager.TaiwuEvent.AddTempDynamicAction<LegendaryBookMonthlyAction>(context, action);
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Fix abnormal lost legendary book by generating its adventure: ");
						defaultInterpolatedStringHandler.AppendFormatted<sbyte>(combatSkillType);
						AdaptableLog.Warning(defaultInterpolatedStringHandler.ToStringAndClear(), false);
					}
				}
			}
		}

		// Token: 0x06004D61 RID: 19809 RVA: 0x002AAE94 File Offset: 0x002A9094
		internal void RegisterLegendaryBookMonthlyAction(LegendaryBookMonthlyAction action)
		{
			bool flag = this._legendaryBookMonthlyActions[(int)action.BookType] != null;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Legendary book monthly action already registered ");
				defaultInterpolatedStringHandler.AppendFormatted<CombatSkillTypeItem>(Config.CombatSkillType.Instance[action.BookType]);
				AdaptableLog.Warning(defaultInterpolatedStringHandler.ToStringAndClear(), false);
			}
			this._legendaryBookMonthlyActions[(int)action.BookType] = action;
		}

		// Token: 0x06004D62 RID: 19810 RVA: 0x002AAF01 File Offset: 0x002A9101
		internal void UnregisterLegendaryBookMonthlyAction(sbyte bookType)
		{
			this._legendaryBookMonthlyActions[(int)bookType] = null;
		}

		// Token: 0x06004D63 RID: 19811 RVA: 0x002AAF10 File Offset: 0x002A9110
		public LegendaryBookMonthlyAction GetOnGoingMonthlyAction(sbyte bookType)
		{
			return this._legendaryBookMonthlyActions[(int)bookType];
		}

		// Token: 0x06004D64 RID: 19812 RVA: 0x002AAF2A File Offset: 0x002A912A
		public void ClearActCrazyShockedCharacters()
		{
			this._actCrazyShockedCharIds.Clear();
		}

		// Token: 0x06004D65 RID: 19813 RVA: 0x002AAF39 File Offset: 0x002A9139
		public void AddActCrazyShockedCharacters(int charId)
		{
			this._actCrazyShockedCharIds.Add(charId);
		}

		// Token: 0x06004D66 RID: 19814 RVA: 0x002AAF4C File Offset: 0x002A914C
		public bool IsCharacterActingCrazy(GameData.Domains.Character.Character character)
		{
			sbyte ownerState = character.GetLegendaryBookOwnerState();
			if (!true)
			{
			}
			bool result = ownerState >= 1 && (ownerState != 1 || this._actCrazyShockedCharIds.Contains(character.GetId()));
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06004D67 RID: 19815 RVA: 0x002AAF94 File Offset: 0x002A9194
		public void CreateLegendaryBooksAccordingToXiangshuProgress(DataContext context)
		{
			bool flag = !DomainManager.World.GetWorldFunctionsStatus(21);
			if (flag)
			{
				DomainManager.Extra.SetFirstLegendaryBookDelay(context, (sbyte)context.Random.Next(3, 9));
			}
			else
			{
				sbyte monthLeft = DomainManager.Extra.GetFirstLegendaryBookDelay();
				bool flag2 = monthLeft > 0;
				if (flag2)
				{
					DomainManager.Extra.SetFirstLegendaryBookDelay(context, monthLeft - 1);
				}
				else
				{
					List<TemplateKey> missingItems = context.AdvanceMonthRelatedData.ItemTemplateKeys.Occupy();
					for (sbyte combatSkillType = 0; combatSkillType < 14; combatSkillType += 1)
					{
						short templateId = (short)(211 + (int)combatSkillType);
						bool flag3 = !DomainManager.Item.HasTrackedSpecialItems(12, templateId);
						if (flag3)
						{
							missingItems.Add(new TemplateKey(12, templateId));
						}
					}
					CollectionUtils.Shuffle<TemplateKey>(context.Random, missingItems);
					int legendaryBookUnlockProgress = Math.Clamp((int)(DomainManager.World.GetXiangshuLevel() - 1), 0, GlobalConfig.Instance.LegendaryBookAppearAmounts.Length - 1);
					sbyte expectedBookAmount = GlobalConfig.Instance.LegendaryBookAppearAmounts[legendaryBookUnlockProgress];
					int currAmount = 14 - missingItems.Count;
					bool flag4 = currAmount < (int)expectedBookAmount && context.Random.CheckPercentProb((int)GlobalConfig.Instance.LegendaryBookAppearChance);
					if (flag4)
					{
						bool flag5 = currAmount == 0;
						TemplateKey itemToCreate;
						short areaId;
						if (flag5)
						{
							GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
							itemToCreate = missingItems[0];
							foreach (TemplateKey missingItem in missingItems)
							{
								bool flag6 = missingItem.TemplateId > 213 && taiwu.GetCombatSkillAttainment((sbyte)(itemToCreate.TemplateId - 211)) < taiwu.GetCombatSkillAttainment((sbyte)(missingItem.TemplateId - 211));
								if (flag6)
								{
									itemToCreate = missingItem;
								}
							}
							areaId = DomainManager.Taiwu.GetTaiwuVillageLocation().AreaId;
						}
						else
						{
							itemToCreate = missingItems.GetRandom(context.Random);
							areaId = (short)context.Random.Next(45);
						}
						sbyte bookType = (sbyte)(itemToCreate.TemplateId - 211);
						ItemKey item = DomainManager.Item.CreateItem(context, itemToCreate.ItemType, itemToCreate.TemplateId);
						MapBlockData blockData = DomainManager.Map.GetRandomMapBlockDataInAreaByFilters(context.Random, areaId, null, false);
						LegendaryBookMonthlyAction action = new LegendaryBookMonthlyAction
						{
							Location = blockData.GetLocation(),
							BookType = bookType,
							BookAppearType = 0
						};
						DomainManager.TaiwuEvent.AddTempDynamicAction<LegendaryBookMonthlyAction>(context, action);
					}
					context.AdvanceMonthRelatedData.ItemTemplateKeys.Release(ref missingItems);
				}
			}
		}

		// Token: 0x06004D68 RID: 19816 RVA: 0x002AB234 File Offset: 0x002A9434
		public void Test_GiveUnownedLegendaryBookToTaiwu(DataContext context)
		{
			List<ItemKey> warehouseItemKeys = DomainManager.Taiwu.GetWarehouseAllItemKey();
			sbyte bookType = 0;
			while ((int)bookType < this._legendaryBookItems.Length)
			{
				ItemKey itemKey = this._legendaryBookItems[(int)bookType];
				bool flag = itemKey.IsValid() && this.GetOwner(bookType) < 0 && !warehouseItemKeys.Contains(itemKey);
				if (flag)
				{
					DomainManager.Taiwu.GetTaiwu().AddInventoryItem(context, itemKey, 1, false);
				}
				bookType += 1;
			}
		}

		// Token: 0x06004D69 RID: 19817 RVA: 0x002AB2B0 File Offset: 0x002A94B0
		public void UpdateLegendaryBookOwnersStatuses(DataContext context)
		{
			List<int> owners = context.AdvanceMonthRelatedData.CharIdList.Occupy();
			owners.AddRange(this._charBookTypes.Keys);
			foreach (int ownerCharId in owners)
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(ownerCharId);
				bool flag = character.GetId() == DomainManager.Taiwu.GetTaiwuCharId();
				if (!flag)
				{
					bool flag2 = character.GetAgeGroup() != 2;
					if (flag2)
					{
						this.LoseAllLegendaryBooks(context, character, true);
					}
					else
					{
						bool flag3 = character.GetKidnapperId() >= 0;
						if (flag3)
						{
							DomainManager.Character.RemoveKidnappedCharacter(context, ownerCharId, character.GetKidnapperId(), true);
						}
						this.UpdateOwnerStatus(context, character);
					}
				}
			}
			HashSet<int> consumedCharSet = ObjectPool<HashSet<int>>.Instance.Get();
			DomainManager.Extra.GetLegendaryBookConsumedCharacters(consumedCharSet);
			foreach (int charId in consumedCharSet)
			{
				GameData.Domains.Character.Character character2;
				bool flag4 = DomainManager.Character.TryGetElement_Objects(charId, out character2);
				if (flag4)
				{
					character2.ActivateAdvanceMonthStatus(7);
				}
			}
			ObjectPool<HashSet<int>>.Instance.Return(consumedCharSet);
			context.AdvanceMonthRelatedData.CharIdList.Release(ref owners);
		}

		// Token: 0x06004D6A RID: 19818 RVA: 0x002AB430 File Offset: 0x002A9630
		private void UpdateOwnerStatus(DataContext context, GameData.Domains.Character.Character character)
		{
			int charId = character.GetId();
			byte invasionSpeedType = DomainManager.World.GetBossInvasionSpeedType();
			int shockedMonths;
			DomainManager.Extra.TryGetElement_LegendaryBookShockedMonths(charId, out shockedMonths);
			switch (character.GetLegendaryBookOwnerState())
			{
			case 0:
				this.UpdateLegendaryBookOwnerGrowth(context, character);
				break;
			case 1:
			{
				bool flag = shockedMonths >= (int)(GlobalConfig.SwordTombAdventureCountDownCoolDown[(int)invasionSpeedType] * 2);
				if (flag)
				{
					character.AddFeature(context, 205, true);
					DomainManager.Character.LeaveGroup(context, character, false);
					Events.RaiseLegendaryBookOwnerStateChanged(context, character, 2);
				}
				else
				{
					bool flag2 = context.Random.CheckPercentProb(50);
					if (flag2)
					{
						this.AddActCrazyShockedCharacters(charId);
						string tag = "LegendaryBook";
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(12, 1);
						defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(character);
						defaultInterpolatedStringHandler.AppendLiteral(" => 入邪发狂判定通过");
						AdaptableLog.TagInfo(tag, defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
				DomainManager.Extra.SetLegendaryBookShockedMonths(context, charId, shockedMonths + 1);
				bool flag3 = DomainManager.Extra.IsCharacterHiddenByLegendaryBook(charId);
				if (flag3)
				{
					bool flag4 = !character.GetLocation().IsValid();
					if (flag4)
					{
						short settlementId = character.GetOrganizationInfo().SettlementId;
						bool flag5 = settlementId < 0;
						if (flag5)
						{
							settlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
						}
						Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
						Location location = settlement.GetLocation();
						character.SetLocation(location, context);
					}
					DomainManager.Extra.RemoveLegendaryBookHiddenChar(context, charId);
					DomainManager.Character.UnhideCharacterOnMap(context, character, 16);
				}
				break;
			}
			case 2:
			{
				bool flag6 = shockedMonths >= (int)(GlobalConfig.SwordTombAdventureCountDownCoolDown[(int)invasionSpeedType] * 3);
				if (flag6)
				{
					List<sbyte> ownedBookTypes = DomainManager.LegendaryBook.GetCharOwnedBookTypes(charId);
					foreach (sbyte combatSkillType in ownedBookTypes)
					{
						short featureId = Config.CombatSkillType.Instance[combatSkillType].LegendaryBookConsumedFeature;
						character.AddFeature(context, featureId, true);
					}
					DomainManager.Extra.AddLegendaryBookConsumed(context, charId);
					Events.RaiseLegendaryBookOwnerStateChanged(context, character, 3);
					this.LoseAllLegendaryBooks(context, character, true);
				}
				else
				{
					DomainManager.Extra.SetLegendaryBookShockedMonths(context, charId, shockedMonths + 1);
				}
				break;
			}
			case 3:
				this.LoseAllLegendaryBooks(context, character, true);
				break;
			}
			bool flag7 = this.IsCharacterActingCrazy(character);
			if (flag7)
			{
				character.ActivateAdvanceMonthStatus(7);
			}
		}

		// Token: 0x06004D6B RID: 19819 RVA: 0x002AB6A4 File Offset: 0x002A98A4
		private unsafe void UpdateLegendaryBookOwnerGrowth(DataContext context, GameData.Domains.Character.Character character)
		{
			int charId = character.GetId();
			sbyte consummateLevel = character.GetConsummateLevel();
			sbyte behaviorType = character.GetBehaviorType();
			bool flag = character.GetOrganizationInfo().OrgTemplateId == 16;
			if (flag)
			{
				DomainManager.Character.LeaveGroup(context, character, false);
				DomainManager.Organization.JoinNearbyVillageTownAsBeggar(context, character, -1);
				bool flag2 = character.IsCrossAreaTraveling();
				if (flag2)
				{
					bool flag3 = !character.GetLocation().IsValid();
					if (flag3)
					{
						Location validLocation = character.GetValidLocation();
						character.SetLocation(validLocation, context);
					}
					DomainManager.Character.RemoveCrossAreaTravelInfo(context, charId);
				}
				DomainManager.Character.HideCharacterOnMap(context, character, 16, true);
				DomainManager.World.GetMonthlyNotificationCollection().AddVillagerLeftForLegendaryBook(charId);
				DomainManager.Extra.AddLegendaryBookHiddenChar(context, charId);
			}
			bool flag4 = consummateLevel < 18;
			if (flag4)
			{
				consummateLevel = (sbyte)Math.Clamp((int)(consummateLevel + 2), 0, 18);
				character.SetConsummateLevel(consummateLevel, context);
			}
			bool flag5 = consummateLevel >= 18;
			if (flag5)
			{
				character.AddFeature(context, 204, false);
				DomainManager.Extra.SetLegendaryBookShockedMonths(context, charId, 1);
				Events.RaiseLegendaryBookOwnerStateChanged(context, character, 1);
			}
			int grade = (int)(consummateLevel / 2);
			int xiangshuMinionTemplateId = 298 + grade;
			CharacterItem xiangshuMinionCfg = Config.Character.Instance[xiangshuMinionTemplateId];
			bool flag6 = character.GetExtraNeili() < xiangshuMinionCfg.ExtraNeili;
			if (flag6)
			{
				character.SetExtraNeili(xiangshuMinionCfg.ExtraNeili, context);
			}
			Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> combatSkills = DomainManager.CombatSkill.GetCharCombatSkills(charId);
			List<sbyte> ownedBookTypes = DomainManager.LegendaryBook.GetCharOwnedBookTypes(charId);
			Span<bool> span = new Span<bool>(stackalloc byte[(UIntPtr)14], 14);
			Span<bool> learnedSkillTypes = span;
			learnedSkillTypes.Fill(false);
			foreach (CombatSkillItem skillCfg in ((IEnumerable<CombatSkillItem>)Config.CombatSkill.Instance))
			{
				bool flag7 = (int)skillCfg.Grade > grade;
				if (!flag7)
				{
					bool flag8 = !ownedBookTypes.Contains(skillCfg.Type);
					if (!flag8)
					{
						bool flag9 = skillCfg.BookId < 0;
						if (!flag9)
						{
							GameData.Domains.CombatSkill.CombatSkill combatSkill;
							bool flag10 = !combatSkills.TryGetValue(skillCfg.TemplateId, out combatSkill);
							if (flag10)
							{
								byte pageTypes = GameData.Domains.Item.SkillBook.GenerateCombatPageTypes(context.Random, -1, 50);
								combatSkill = character.LearnNewCombatSkill(context, skillCfg.TemplateId, CombatSkillStateHelper.GenerateReadingStateFromSkillBook(pageTypes));
								*learnedSkillTypes[(int)skillCfg.Type] = true;
							}
							bool flag11 = !CombatSkillStateHelper.IsBrokenOut(combatSkill.GetActivationState());
							if (flag11)
							{
								byte pageTypes2 = GameData.Domains.Item.SkillBook.GenerateCombatPageTypes(context.Random, -1, 50);
								ushort readingState = combatSkill.GetReadingState() | CombatSkillStateHelper.GenerateReadingStateFromSkillBook(pageTypes2);
								combatSkill.SetReadingState(readingState, context);
								bool flag12 = combatSkill.CanBreakout();
								if (flag12)
								{
									ushort activationState = CombatSkillStateHelper.GenerateRandomActivatedNormalPages(context.Random, readingState, 0);
									activationState = CombatSkillStateHelper.GenerateRandomActivatedOutlinePage(context.Random, readingState, activationState, behaviorType);
									sbyte availableStepsCount = character.GetSkillBreakoutAvailableStepsCount(skillCfg.TemplateId);
									combatSkill.SetActivationState(activationState, context);
									combatSkill.SetBreakoutStepsCount(availableStepsCount, context);
									*learnedSkillTypes[(int)skillCfg.Type] = true;
								}
							}
						}
					}
				}
			}
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = character.GetLocation();
			foreach (sbyte bookType in ownedBookTypes)
			{
				bool flag13 = !(*learnedSkillTypes[(int)bookType]);
				if (!flag13)
				{
					ItemKey itemKey = this.GetLegendaryBookItem(bookType);
					lifeRecordCollection.AddBoostedByLegendaryBooks(charId, currDate, location, itemKey.ItemType, itemKey.TemplateId);
				}
			}
		}

		// Token: 0x06004D6C RID: 19820 RVA: 0x002ABA7C File Offset: 0x002A9C7C
		public void UpdateLegendaryBookOwnersActions(DataContext context)
		{
			List<int> owners = ObjectPool<List<int>>.Instance.Get();
			owners.AddRange(this._charBookTypes.Keys);
			foreach (int ownerCharId in owners)
			{
				GameData.Domains.Character.Character character;
				bool flag = !DomainManager.Character.TryGetElement_Objects(ownerCharId, out character);
				if (!flag)
				{
					bool flag2 = ownerCharId == DomainManager.Taiwu.GetTaiwuCharId();
					if (!flag2)
					{
						bool flag3 = !this.IsCharacterActingCrazy(character);
						if (!flag3)
						{
							this.UpdateOwnerAction(context, character);
						}
					}
				}
			}
			ObjectPool<List<int>>.Instance.Return(owners);
		}

		// Token: 0x06004D6D RID: 19821 RVA: 0x002ABB3C File Offset: 0x002A9D3C
		private void UpdateOwnerAction(DataContext context, GameData.Domains.Character.Character character)
		{
			Location location = character.GetLocation();
			bool flag = !location.IsValid();
			if (!flag)
			{
				bool flag2 = character.IsActiveExternalRelationState(60);
				if (!flag2)
				{
					GameData.Domains.Character.Character harmActionTarget = this.SelectHarmActionTarget(context, character, location);
					bool flag3 = harmActionTarget == null;
					if (!flag3)
					{
						LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
						int currDate = DomainManager.World.GetCurrDate();
						lifeRecordCollection.AddActCrazy(character.GetId(), currDate, location);
						DomainManager.Character.HandleAttackAction(context, character, harmActionTarget);
					}
				}
			}
		}

		// Token: 0x06004D6E RID: 19822 RVA: 0x002ABBBC File Offset: 0x002A9DBC
		private GameData.Domains.Character.Character SelectHarmActionTarget(DataContext context, GameData.Domains.Character.Character character, Location location)
		{
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			int selfCharId = character.GetId();
			bool flag = location.Equals(taiwuChar.GetLocation());
			GameData.Domains.Character.Character result;
			if (flag)
			{
				result = taiwuChar;
			}
			else
			{
				MapBlockData block = DomainManager.Map.GetBlock(location);
				bool flag2 = block.CharacterSet == null || block.CharacterSet.Count == 1;
				if (flag2)
				{
					result = null;
				}
				else
				{
					List<int> charIdList = context.AdvanceMonthRelatedData.TargetCharIdList.Occupy();
					charIdList.AddRange(block.CharacterSet);
					charIdList.RemoveAll((int charId) => DomainManager.Character.GetElement_Objects(charId).GetAgeGroup() == 0 || charId == selfCharId);
					int targetCharId = charIdList.GetRandomOrDefault(context.Random, -1);
					context.AdvanceMonthRelatedData.TargetCharIdList.Release(ref charIdList);
					bool flag3 = targetCharId < 0;
					if (flag3)
					{
						result = null;
					}
					else
					{
						result = DomainManager.Character.GetElement_Objects(targetCharId);
					}
				}
			}
			return result;
		}

		// Token: 0x06004D6F RID: 19823 RVA: 0x002ABCA8 File Offset: 0x002A9EA8
		public void UpgradeEnemyNestsByLegendaryBookOwner(DataContext context, short areaId, int upgradeCount)
		{
			MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
			monthlyNotifications.AddRandomEnemyGrow(new Location(areaId, -1));
		}

		// Token: 0x06004D70 RID: 19824 RVA: 0x002ABCD0 File Offset: 0x002A9ED0
		public void RegisterOwner(DataContext context, GameData.Domains.Character.Character character, sbyte bookType)
		{
			string tag = "LegendaryBook";
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 2);
			defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(character);
			defaultInterpolatedStringHandler.AppendLiteral(" => 得到奇书 ");
			defaultInterpolatedStringHandler.AppendFormatted(Config.Misc.Instance[211 + (int)bookType].Name);
			AdaptableLog.TagInfo(tag, defaultInterpolatedStringHandler.ToStringAndClear());
			int oriOwner = this._bookOwners[(int)bookType];
			bool flag = oriOwner >= 0;
			if (flag)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Book ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(bookType);
				defaultInterpolatedStringHandler.AppendLiteral(" already has owner: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(oriOwner);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			int charId = character.GetId();
			this.SetElement_BookOwners((int)bookType, charId, context);
			this.RegisterCharBookType(charId, bookType);
			character.TryRetireTreasuryGuard(context);
			bool flag2 = charId == DomainManager.Taiwu.GetTaiwuCharId();
			if (flag2)
			{
				DomainManager.World.SetWorldFunctionsStatus(context, 21);
			}
			else
			{
				bool flag3 = DomainManager.Taiwu.GetLegacyPassingState() != 4;
				if (flag3)
				{
					short featureId = Config.CombatSkillType.Instance[bookType].LegendaryBookFeature;
					character.AddFeature(context, featureId, false);
				}
			}
		}

		// Token: 0x06004D71 RID: 19825 RVA: 0x002ABE04 File Offset: 0x002AA004
		public void UnregisterOwner(DataContext context, GameData.Domains.Character.Character character, sbyte bookType)
		{
			string tag = "LegendaryBook";
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 2);
			defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(character);
			defaultInterpolatedStringHandler.AppendLiteral(" => 失去奇书 ");
			defaultInterpolatedStringHandler.AppendFormatted(Config.Misc.Instance[211 + (int)bookType].Name);
			AdaptableLog.TagInfo(tag, defaultInterpolatedStringHandler.ToStringAndClear());
			int oriOwner = this._bookOwners[(int)bookType];
			int charId = character.GetId();
			bool flag = oriOwner < 0;
			if (flag)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Book ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(bookType);
				defaultInterpolatedStringHandler.AppendLiteral(" does not have owner");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			bool flag2 = oriOwner != charId;
			if (flag2)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Wrong owner of book ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(bookType);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(oriOwner);
				defaultInterpolatedStringHandler.AppendLiteral(" - ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			List<short> featureIds = character.GetFeatureIds();
			short bookFeatureId = Config.CombatSkillType.Instance[bookType].LegendaryBookFeature;
			bool isLegendaryBookConsumed = DomainManager.Extra.IsLegendaryBookConsumed(charId);
			bool flag3 = isLegendaryBookConsumed || featureIds.Contains(205);
			if (flag3)
			{
				DomainManager.Character.UnregisterFeatureForAllXiangshuAvatars(context, bookFeatureId);
				DomainManager.Extra.RegisterPrevLegendaryBookOwner(context, character, bookType);
				MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
				sbyte currAdvancingMonthState = DomainManager.World.GetAdvancingMonthState();
				bool flag4 = currAdvancingMonthState > 0 && currAdvancingMonthState < 20;
				if (flag4)
				{
					ItemKey itemKey = DomainManager.LegendaryBook.GetLegendaryBookItem(bookType);
					monthlyEventCollection.AddSwordTombBackToNormal((ulong)itemKey);
				}
			}
			this.SetElement_BookOwners((int)bookType, -1, context);
			this.UnregisterCharBookType(charId, bookType);
			DomainManager.Extra.RemoveContestForLegendaryBookCharacters(context, bookType);
			DomainManager.Extra.AddPreviousLegendaryBookOwner(context, charId, bookType);
			bool flag5 = !DomainManager.Character.IsCharacterAlive(charId);
			if (!flag5)
			{
				character.RemoveFeature(context, bookFeatureId);
				bool flag6 = this._charBookTypes.ContainsKey(charId) || isLegendaryBookConsumed;
				if (!flag6)
				{
					DomainManager.Extra.RemoveLegendaryBookShockedMonths(context, charId);
					character.RemoveFeature(context, 204);
					character.RemoveFeature(context, 205);
				}
			}
		}

		// Token: 0x06004D72 RID: 19826 RVA: 0x002AC04C File Offset: 0x002AA24C
		public int GetOwner(sbyte bookType)
		{
			return this._bookOwners[(int)bookType];
		}

		// Token: 0x06004D73 RID: 19827 RVA: 0x002AC068 File Offset: 0x002AA268
		public sbyte GetConsumedCharacterLegendaryBookType(GameData.Domains.Character.Character character)
		{
			short minFeatureId = Config.CombatSkillType.Instance[0].LegendaryBookConsumedFeature;
			short maxFeatureId = Config.CombatSkillType.Instance[13].LegendaryBookConsumedFeature;
			foreach (short featureId in character.GetFeatureIds())
			{
				bool flag = featureId >= minFeatureId && featureId <= maxFeatureId;
				if (flag)
				{
					return (sbyte)(featureId - minFeatureId);
				}
			}
			return -1;
		}

		// Token: 0x06004D74 RID: 19828 RVA: 0x002AC100 File Offset: 0x002AA300
		public void UpdateBossCharacterLegendaryBookFeatures(DataContext context, GameData.Domains.Character.Character character)
		{
			sbyte xiangshuType = character.GetXiangshuType();
			bool flag = xiangshuType == 1;
			if (flag)
			{
				foreach (CombatSkillTypeItem combatSkillTypeCfg in ((IEnumerable<CombatSkillTypeItem>)Config.CombatSkillType.Instance))
				{
					character.RemoveFeatureGroup(context, combatSkillTypeCfg.LegendaryBookFeature);
				}
				for (sbyte combatSkillType = 0; combatSkillType < 14; combatSkillType += 1)
				{
					int ownerId = this._bookOwners[(int)combatSkillType];
					bool flag2 = ownerId < 0;
					if (!flag2)
					{
						GameData.Domains.Character.Character owner = DomainManager.Character.GetElement_Objects(ownerId);
						bool flag3 = owner.GetLegendaryBookOwnerState() != 2;
						if (!flag3)
						{
							short featureToAdd = Config.CombatSkillType.Instance[combatSkillType].LegendaryBookFeature;
							character.AddFeature(context, featureToAdd, false);
						}
					}
				}
			}
			else
			{
				bool flag4 = xiangshuType == 2 || character.GetTemplateId() == 443;
				if (flag4)
				{
					foreach (CombatSkillTypeItem combatSkillTypeCfg2 in ((IEnumerable<CombatSkillTypeItem>)Config.CombatSkillType.Instance))
					{
						character.RemoveFeatureGroup(context, combatSkillTypeCfg2.LegendaryBookFeature);
					}
					HashSet<int> consumedCharIds = ObjectPool<HashSet<int>>.Instance.Get();
					DomainManager.Extra.GetLegendaryBookConsumedCharacters(consumedCharIds);
					foreach (int consumedCharId in consumedCharIds)
					{
						GameData.Domains.Character.Character consumedChar;
						bool flag5 = !DomainManager.Character.TryGetElement_Objects(consumedCharId, out consumedChar);
						if (!flag5)
						{
							short minFeatureId = Config.CombatSkillType.Instance[0].LegendaryBookConsumedFeature;
							short maxFeatureId = Config.CombatSkillType.Instance[13].LegendaryBookConsumedFeature;
							foreach (short featureId in consumedChar.GetFeatureIds())
							{
								bool flag6 = featureId < minFeatureId || featureId > maxFeatureId;
								if (!flag6)
								{
									int combatSkillType2 = (int)(featureId - minFeatureId);
									short featureToAdd2 = Config.CombatSkillType.Instance[combatSkillType2].LegendaryBookFeature;
									character.AddFeature(context, featureToAdd2, false);
								}
							}
						}
					}
					ObjectPool<HashSet<int>>.Instance.Return(consumedCharIds);
				}
				else
				{
					bool flag7 = xiangshuType == 4;
					if (flag7)
					{
						foreach (CombatSkillTypeItem combatSkillTypeCfg3 in ((IEnumerable<CombatSkillTypeItem>)Config.CombatSkillType.Instance))
						{
							character.RemoveFeatureGroup(context, combatSkillTypeCfg3.LegendaryBookFeature);
						}
						List<short> features = DomainManager.Extra.GetWoodenXiangshuAvatarSelectedFeatures();
						foreach (short featureId2 in features)
						{
							character.AddFeature(context, featureId2, false);
						}
					}
				}
			}
		}

		// Token: 0x06004D75 RID: 19829 RVA: 0x002AC428 File Offset: 0x002AA628
		public sbyte GetCharacterLegendaryBookOwnerState(int charId)
		{
			bool flag = DomainManager.Extra.IsLegendaryBookConsumed(charId);
			sbyte result;
			if (flag)
			{
				result = 3;
			}
			else
			{
				GameData.Domains.Character.Character character;
				bool flag2 = DomainManager.LegendaryBook.GetCharOwnedBookTypes(charId) == null || !DomainManager.Character.TryGetElement_Objects(charId, out character);
				if (flag2)
				{
					result = -1;
				}
				else
				{
					List<short> featureIds = character.GetFeatureIds();
					bool flag3 = featureIds.Contains(204);
					if (flag3)
					{
						result = 1;
					}
					else
					{
						bool flag4 = featureIds.Contains(205);
						if (flag4)
						{
							result = 2;
						}
						else
						{
							result = 0;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06004D76 RID: 19830 RVA: 0x002AC4AC File Offset: 0x002AA6AC
		public sbyte GetLegendaryBookAppearType(int prevOwnerId)
		{
			bool flag = prevOwnerId < 0;
			sbyte result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = !DomainManager.Character.IsCharacterAlive(prevOwnerId);
				if (flag2)
				{
					result = 2;
				}
				else
				{
					bool flag3 = DomainManager.Extra.IsLegendaryBookConsumed(prevOwnerId);
					if (flag3)
					{
						result = 1;
					}
					else
					{
						result = 3;
					}
				}
			}
			return result;
		}

		// Token: 0x06004D77 RID: 19831 RVA: 0x002AC4F4 File Offset: 0x002AA6F4
		public List<sbyte> GetCharOwnedBookTypes(int charId)
		{
			List<sbyte> bookTypes;
			return this._charBookTypes.TryGetValue(charId, out bookTypes) ? bookTypes : null;
		}

		// Token: 0x06004D78 RID: 19832 RVA: 0x002AC51C File Offset: 0x002AA71C
		public void OnCharacterDead(DataContext context, GameData.Domains.Character.Character character)
		{
			int charId = character.GetId();
			DomainManager.Extra.RemoveLegendaryBookConsumed(context, charId);
			DomainManager.Extra.RemoveLegendaryBookShockedMonths(context, charId);
			DomainManager.Extra.ApplyRanshanThreeCorpsesLegendaryBookActionTargetDeadResult(context, charId);
			this.LoseAllLegendaryBooks(context, character, true);
		}

		// Token: 0x06004D79 RID: 19833 RVA: 0x002AC564 File Offset: 0x002AA764
		public bool LoseAllLegendaryBooks(DataContext context, GameData.Domains.Character.Character character, bool createAdventures)
		{
			int charId = character.GetId();
			List<sbyte> bookTypes = this.GetCharOwnedBookTypes(charId);
			bool flag = bookTypes == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Dictionary<ItemKey, int> inventoryItems = character.GetInventory().Items;
				List<ItemKey> toRemoveItems = context.AdvanceMonthRelatedData.ItemKeys.Occupy();
				foreach (ItemKey itemKey in inventoryItems.Keys)
				{
					bool flag2 = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId) == 1202;
					if (flag2)
					{
						toRemoveItems.Add(itemKey);
					}
				}
				sbyte appearType = this.GetLegendaryBookAppearType(charId);
				Inventory inventory = character.GetInventory();
				Location location = character.GetLocation();
				short areaId = location.AreaId;
				bool flag3 = areaId < 0;
				if (flag3)
				{
					areaId = (short)context.Random.Next(45);
				}
				MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
				monthlyNotificationCollection.AddRandomEnemyDecay(location);
				foreach (ItemKey itemKey2 in toRemoveItems)
				{
					sbyte bookCombatSkillType = (sbyte)(itemKey2.TemplateId - 211);
					bool flag4 = appearType != 2;
					if (flag4)
					{
						character.RemoveInventoryItem(context, itemKey2, 1, false, false);
					}
					else
					{
						inventory.OfflineRemove(itemKey2, 1);
						this.UnregisterOwner(context, character, bookCombatSkillType);
						DomainManager.Item.GetBaseItem(itemKey2).ResetOwner();
					}
					monthlyNotificationCollection.AddLegendaryBookLost(charId, location, itemKey2.ItemType, itemKey2.TemplateId);
					DomainManager.Item.SetOwner(itemKey2, ItemOwnerType.System, 11);
					bool flag5 = !createAdventures;
					if (!flag5)
					{
						MapBlockData mapBlockData = DomainManager.Map.GetRandomMapBlockDataInAreaByFilters(context.Random, areaId, null, false);
						LegendaryBookMonthlyAction action = new LegendaryBookMonthlyAction
						{
							Location = ((mapBlockData != null) ? mapBlockData.GetLocation() : Location.Invalid),
							BookType = bookCombatSkillType,
							BookAppearType = appearType,
							PrevOwnerId = charId
						};
						DomainManager.TaiwuEvent.AddTempDynamicAction<LegendaryBookMonthlyAction>(context, action);
					}
				}
				context.AdvanceMonthRelatedData.ItemKeys.Release(ref toRemoveItems);
				result = true;
			}
			return result;
		}

		// Token: 0x06004D7A RID: 19834 RVA: 0x002AC7CC File Offset: 0x002AA9CC
		public void LoseTargetLegendaryBook(DataContext context, GameData.Domains.Character.Character character, bool createAdventures, ItemKey itemKey)
		{
			int charId = character.GetId();
			sbyte appearType = this.GetLegendaryBookAppearType(charId);
			Inventory inventory = character.GetInventory();
			Location location = character.GetLocation();
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			sbyte bookCombatSkillType = (sbyte)(itemKey.TemplateId - 211);
			short areaId = location.AreaId;
			bool flag = areaId < 0;
			if (flag)
			{
				areaId = (short)context.Random.Next(45);
			}
			bool flag2 = appearType != 2;
			if (flag2)
			{
				character.RemoveInventoryItem(context, itemKey, 1, false, false);
			}
			else
			{
				inventory.OfflineRemove(itemKey, 1);
				this.UnregisterOwner(context, character, bookCombatSkillType);
				DomainManager.Item.GetBaseItem(itemKey).ResetOwner();
			}
			monthlyNotificationCollection.AddLegendaryBookLost(charId, location, itemKey.ItemType, itemKey.TemplateId);
			DomainManager.Item.SetOwner(itemKey, ItemOwnerType.System, 11);
			bool flag3 = !createAdventures;
			if (!flag3)
			{
				MapBlockData mapBlockData = DomainManager.Map.GetRandomMapBlockDataInAreaByFilters(context.Random, areaId, null, false);
				LegendaryBookMonthlyAction action = new LegendaryBookMonthlyAction
				{
					Location = ((mapBlockData != null) ? mapBlockData.GetLocation() : Location.Invalid),
					BookType = bookCombatSkillType,
					BookAppearType = appearType,
					PrevOwnerId = charId
				};
				DomainManager.TaiwuEvent.AddTempDynamicAction<LegendaryBookMonthlyAction>(context, action);
			}
		}

		// Token: 0x06004D7B RID: 19835 RVA: 0x002AC908 File Offset: 0x002AAB08
		public LegendaryBookCharacterRelatedData GetLegendaryBookCharacterRelatedData(int charId, sbyte bookType = -1)
		{
			GameData.Domains.Character.Character character;
			bool flag = !GameData.Domains.Character.Character.IsCharacterIdValid(charId) || !DomainManager.Character.TryGetElement_Objects(charId, out character);
			LegendaryBookCharacterRelatedData result;
			if (flag)
			{
				result = null;
			}
			else
			{
				Location location = character.GetLocation();
				FullBlockName blockName = DomainManager.Map.GetBlockFullName(location);
				bool flag2 = location != Location.Invalid;
				if (flag2)
				{
					MapBlockData blockData = DomainManager.Map.GetBlock(character.GetValidLocation());
					bool flag3 = blockData.RootBlockId > -1;
					if (flag3)
					{
						blockData = DomainManager.Map.GetBlockData(blockData.AreaId, blockData.RootBlockId);
					}
					blockName = DomainManager.Map.GetBlockFullName(blockData.GetLocation());
				}
				LegendaryBookCharacterRelatedData res = new LegendaryBookCharacterRelatedData
				{
					Id = charId,
					CurrAge = character.GetCurrAge(),
					Gender = character.GetGender(),
					FeatureId = -1,
					ConsummateLevel = character.GetConsummateLevel(),
					Charm = character.GetAttraction(),
					BehaviorType = character.GetBehaviorType(),
					HappinessType = character.GetHappinessType(),
					Favorability = DomainManager.Character.GetFavorability(charId, DomainManager.Taiwu.GetTaiwuCharId()),
					FameType = character.GetFameType(),
					HealthType = DomainManager.Character.GetHealthType(charId),
					BookType = bookType,
					Location = location,
					AvatarRelatedData = DomainManager.Character.GetAvatarRelatedData(charId),
					NameRelatedData = DomainManager.Character.GetNameRelatedData(charId),
					OrganizationInfo = character.GetOrganizationInfo(),
					FullBlockName = blockName
				};
				List<short> featureIds = character.GetFeatureIds();
				bool flag4 = featureIds.Contains(204);
				if (flag4)
				{
					res.FeatureId = 204;
				}
				else
				{
					bool flag5 = featureIds.Contains(205);
					if (flag5)
					{
						res.FeatureId = 205;
					}
				}
				result = res;
			}
			return result;
		}

		// Token: 0x06004D7C RID: 19836 RVA: 0x002ACAD8 File Offset: 0x002AACD8
		[DomainMethod]
		public LegendaryBookIncrementData GetLegendaryBookIncrementData()
		{
			LegendaryBookIncrementData res = new LegendaryBookIncrementData();
			for (sbyte i = 0; i < 14; i += 1)
			{
				LegendaryBookMonthlyAction action = this._legendaryBookMonthlyActions[(int)i];
				bool flag = action != null;
				if (flag)
				{
					bool flag2 = action.State == 5;
					if (flag2)
					{
						MapBlockData blockData = DomainManager.Map.GetBlock(action.Location);
						FullBlockName fullName = DomainManager.Map.GetBlockFullName(blockData.GetLocation());
						bool flag3 = blockData.RootBlockId > -1;
						if (flag3)
						{
							blockData = DomainManager.Map.GetBlockData(blockData.AreaId, blockData.RootBlockId);
						}
						res.BookLocationMap.Add(i, action.Location);
						res.BookDurationMap.Add(i, (int)DomainManager.Adventure.GetAdventureSite(action.Location.AreaId, action.Location.BlockId).RemainingMonths);
						res.BlockDataMap.TryAdd(i, blockData);
						res.BlockNameDataMap.TryAdd(i, fullName);
					}
					else
					{
						res.BookDurationMap.Add(i, (int)action.ActivateDelay - action.Month + 1);
					}
				}
				else
				{
					int ownerId = this.GetOwner(i);
					LegendaryBookCharacterRelatedData data = this.GetLegendaryBookCharacterRelatedData(ownerId, i);
					bool flag4 = data != null;
					if (flag4)
					{
						Location location = DomainManager.Character.GetElement_Objects(ownerId).GetLocation();
						bool flag5 = location.IsValid();
						if (flag5)
						{
							MapBlockData blockData2 = DomainManager.Map.GetBlock(location);
							FullBlockName fullName2 = DomainManager.Map.GetBlockFullName(blockData2.GetLocation());
							bool flag6 = blockData2.RootBlockId > -1;
							if (flag6)
							{
								blockData2 = DomainManager.Map.GetBlockData(blockData2.AreaId, blockData2.RootBlockId);
							}
							res.BlockDataMap.TryAdd(i, blockData2);
							res.BlockNameDataMap.TryAdd(i, fullName2);
						}
						res.OwnerMap.Add(i, ownerId);
						res.CharacterMap.TryAdd(ownerId, data);
					}
					foreach (int contestId in DomainManager.Extra.GetContestForLegendaryBookCharacterSet(i).GetCollection())
					{
						data = this.GetLegendaryBookCharacterRelatedData(contestId, i);
						bool flag7 = data != null;
						if (flag7)
						{
							res.ContestList.Add(contestId);
							res.CharacterMap.TryAdd(contestId, data);
						}
					}
				}
			}
			foreach (LegendaryBookCharacterRelatedData data2 in res.CharacterMap.Values)
			{
				bool flag8 = data2.FeatureId == 204;
				if (flag8)
				{
					res.ShockedList.Add(data2.Id);
				}
				else
				{
					bool flag9 = data2.FeatureId == 205;
					if (flag9)
					{
						res.InsaneList.Add(data2.Id);
					}
				}
			}
			HashSet<int> charIds = new HashSet<int>();
			DomainManager.Extra.GetLegendaryBookConsumedCharacters(charIds);
			foreach (int consumedId in charIds)
			{
				LegendaryBookCharacterRelatedData data3 = this.GetLegendaryBookCharacterRelatedData(consumedId, -1);
				bool flag10 = data3 != null;
				if (flag10)
				{
					res.ConsumedList.Add(consumedId);
					res.CharacterMap.TryAdd(consumedId, data3);
				}
			}
			foreach (LegendaryBookCharacterRelatedData data4 in res.CharacterMap.Values)
			{
				bool flag11 = data4.FeatureId == 204;
				if (!flag11)
				{
					List<short> featureIds = DomainManager.Character.GetElement_Objects(data4.Id).GetFeatureIds();
					for (sbyte j = 0; j < 14; j += 1)
					{
						CombatSkillTypeItem config = Config.CombatSkillType.Instance[j];
						bool flag12 = featureIds.Contains(config.LegendaryBookFeature);
						if (flag12)
						{
							data4.FeatureId = config.LegendaryBookFeature;
							break;
						}
						bool flag13 = featureIds.Contains(config.LegendaryBookConsumedFeature);
						if (flag13)
						{
							data4.FeatureId = config.LegendaryBookConsumedFeature;
							break;
						}
					}
				}
			}
			return res;
		}

		// Token: 0x06004D7D RID: 19837 RVA: 0x002ACF74 File Offset: 0x002AB174
		[DomainMethod]
		public List<IntPair> GmCmd_GetAllLegendaryBookStates()
		{
			List<IntPair> res = new List<IntPair>();
			for (sbyte i = 0; i < 14; i += 1)
			{
				res.Add(new IntPair(this._bookOwners[(int)i], -1));
			}
			for (short areaId = 0; areaId < 135; areaId += 1)
			{
				foreach (KeyValuePair<short, AdventureSiteData> data in DomainManager.Adventure.GetElement_AdventureAreas((int)areaId).AdventureSites)
				{
					bool flag = Adventure.Instance[data.Value.TemplateId].Type == 17;
					if (flag)
					{
						res[(int)this.GetBookTypeByAdventureTemplateId(data.Value.TemplateId)] = new IntPair(-1, (int)areaId);
					}
				}
			}
			return res;
		}

		// Token: 0x06004D7E RID: 19838 RVA: 0x002AD068 File Offset: 0x002AB268
		[DomainMethod]
		public int GetAllLegendaryBooksOwningState()
		{
			int res = 0;
			for (sbyte bookType = 0; bookType < 14; bookType += 1)
			{
				IntPair intPair;
				bool flag = this.GetOwner(bookType) >= 0 || DomainManager.Extra.TryGetElement_PrevLegendaryBookOwnerCopies(bookType, out intPair) || DomainManager.Extra.IsBookOwnedByTaiwu(bookType) || DomainManager.Extra.IsLegendaryBookOwned(bookType);
				if (flag)
				{
					res |= 1 << (int)bookType;
				}
			}
			return res;
		}

		// Token: 0x06004D7F RID: 19839 RVA: 0x002AD0D4 File Offset: 0x002AB2D4
		[DomainMethod]
		public void GmCmd_GiveAllTaiwuLegendaryBookToRandomNpc(DataContext context)
		{
			List<GameData.Domains.Character.Character> chars = new List<GameData.Domains.Character.Character>();
			DomainManager.Character.FindIntelligentCharacters((GameData.Domains.Character.Character _) => true, chars);
			for (sbyte bookType = 0; bookType < 14; bookType += 1)
			{
				int randIndex = context.Random.Next(0, chars.Count);
				GameData.Domains.Character.Character target = chars[randIndex];
				int currentOwner = this.GetOwner(bookType);
				bool flag = currentOwner > 0;
				if (flag)
				{
					DomainManager.Character.TransferInventoryItem(context, DomainManager.Character.GetElement_Objects(currentOwner), target, this.GetLegendaryBookItem(bookType), 1);
				}
			}
		}

		// Token: 0x06004D80 RID: 19840 RVA: 0x002AD17C File Offset: 0x002AB37C
		public bool IsCharacterLegendaryBookOwnerOrContest(int charId)
		{
			bool flag = this._charBookTypes.ContainsKey(charId);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				for (sbyte i = 0; i < 14; i += 1)
				{
					bool flag2 = DomainManager.Extra.GetContestForLegendaryBookCharacterSet(i).Contains(charId);
					if (flag2)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06004D81 RID: 19841 RVA: 0x002AD1D4 File Offset: 0x002AB3D4
		private void InitializeCharBookTypes()
		{
			this._charBookTypes = new Dictionary<int, List<sbyte>>();
			for (sbyte bookType = 0; bookType < 14; bookType += 1)
			{
				int charId = this._bookOwners[(int)bookType];
				bool flag = charId >= 0;
				if (flag)
				{
					this.RegisterCharBookType(charId, bookType);
				}
			}
		}

		// Token: 0x06004D82 RID: 19842 RVA: 0x002AD220 File Offset: 0x002AB420
		private void RegisterCharBookType(int charId, sbyte bookType)
		{
			List<sbyte> bookTypes;
			bool flag = !this._charBookTypes.TryGetValue(charId, out bookTypes);
			if (flag)
			{
				bookTypes = new List<sbyte>();
				this._charBookTypes.Add(charId, bookTypes);
			}
			bookTypes.Add(bookType);
		}

		// Token: 0x06004D83 RID: 19843 RVA: 0x002AD264 File Offset: 0x002AB464
		private void UnregisterCharBookType(int charId, sbyte bookType)
		{
			List<sbyte> bookTypes;
			bool flag = !this._charBookTypes.TryGetValue(charId, out bookTypes);
			if (!flag)
			{
				bookTypes.Remove(bookType);
				bool flag2 = bookTypes.Count <= 0;
				if (flag2)
				{
					this._charBookTypes.Remove(charId);
				}
			}
		}

		// Token: 0x06004D84 RID: 19844 RVA: 0x002AD2B0 File Offset: 0x002AB4B0
		private sbyte GetBookTypeByAdventureTemplateId(short templateId)
		{
			if (!true)
			{
			}
			sbyte result;
			switch (templateId)
			{
			case 145:
				result = 5;
				break;
			case 146:
				result = 1;
				break;
			case 147:
				result = 2;
				break;
			case 148:
				result = 6;
				break;
			case 149:
				result = 0;
				break;
			case 150:
				result = 11;
				break;
			case 151:
				result = 13;
				break;
			case 152:
				result = 8;
				break;
			case 153:
				result = 12;
				break;
			case 154:
				result = 7;
				break;
			case 155:
				result = 10;
				break;
			case 156:
				result = 3;
				break;
			case 157:
				result = 4;
				break;
			case 158:
				result = 9;
				break;
			default:
				result = -1;
				break;
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06004D85 RID: 19845 RVA: 0x002AD354 File Offset: 0x002AB554
		private void InitializeLegendaryBookItems()
		{
			for (sbyte combatSkillType = 0; combatSkillType < 14; combatSkillType += 1)
			{
				this._legendaryBookItems[(int)combatSkillType] = ItemKey.Invalid;
			}
		}

		// Token: 0x06004D86 RID: 19846 RVA: 0x002AD388 File Offset: 0x002AB588
		public ItemKey GetLegendaryBookItem(sbyte combatSkillType)
		{
			return this._legendaryBookItems[(int)combatSkillType];
		}

		// Token: 0x06004D87 RID: 19847 RVA: 0x002AD3A8 File Offset: 0x002AB5A8
		internal void RegisterLegendaryBookItem(ItemKey itemKey)
		{
			bool condition = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId) == 1202;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Target item ");
			defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(itemKey);
			defaultInterpolatedStringHandler.AppendLiteral(" is not a valid legendary book.");
			Tester.Assert(condition, defaultInterpolatedStringHandler.ToStringAndClear());
			int combatSkillType = (int)(itemKey.TemplateId - 211);
			bool condition2 = !this._legendaryBookItems[combatSkillType].IsValid();
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Legendary book ");
			defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(itemKey);
			defaultInterpolatedStringHandler.AppendLiteral(" of the same type already exist ");
			defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(this._legendaryBookItems[combatSkillType]);
			Tester.Assert(condition2, defaultInterpolatedStringHandler.ToStringAndClear());
			this._legendaryBookItems[combatSkillType] = itemKey;
		}

		// Token: 0x06004D88 RID: 19848 RVA: 0x002AD484 File Offset: 0x002AB684
		internal void UnregisterLegendaryBookItem(ItemKey itemKey)
		{
			bool condition = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId) == 1202;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Target item ");
			defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(itemKey);
			defaultInterpolatedStringHandler.AppendLiteral(" is not a valid legendary book.");
			Tester.Assert(condition, defaultInterpolatedStringHandler.ToStringAndClear());
			int combatSkillType = (int)(itemKey.TemplateId - 211);
			this._legendaryBookItems[combatSkillType] = ItemKey.Invalid;
		}

		// Token: 0x06004D89 RID: 19849 RVA: 0x002AD504 File Offset: 0x002AB704
		public bool IsAnyLegendaryBookOwned()
		{
			return this._charBookTypes.Count > 0;
		}

		// Token: 0x06004D8A RID: 19850 RVA: 0x002AD524 File Offset: 0x002AB724
		public LegendaryBookDomain() : base(2)
		{
			this._bookOwners = new int[14];
			this._legendaryBookOwnerData = new LegendaryBookOwnerData();
			this.OnInitializedDomainData();
		}

		// Token: 0x06004D8B RID: 19851 RVA: 0x002AD58C File Offset: 0x002AB78C
		public int GetElement_BookOwners(int index)
		{
			return this._bookOwners[index];
		}

		// Token: 0x06004D8C RID: 19852 RVA: 0x002AD5A8 File Offset: 0x002AB7A8
		public unsafe void SetElement_BookOwners(int index, int value, DataContext context)
		{
			this._bookOwners[index] = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(index, this._dataStatesBookOwners, LegendaryBookDomain.CacheInfluencesBookOwners, context);
			byte* pData = OperationAdder.FixedElementList_Set(11, 0, index, 4);
			*(int*)pData = value;
			pData += 4;
		}

		// Token: 0x06004D8D RID: 19853 RVA: 0x002AD5E4 File Offset: 0x002AB7E4
		public LegendaryBookOwnerData GetLegendaryBookOwnerData()
		{
			return this._legendaryBookOwnerData;
		}

		// Token: 0x06004D8E RID: 19854 RVA: 0x002AD5FC File Offset: 0x002AB7FC
		public void SetLegendaryBookOwnerData(LegendaryBookOwnerData value, DataContext context)
		{
			this._legendaryBookOwnerData = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, this.DataStates, LegendaryBookDomain.CacheInfluences, context);
		}

		// Token: 0x06004D8F RID: 19855 RVA: 0x002AD619 File Offset: 0x002AB819
		public override void OnInitializeGameDataModule()
		{
			this.InitializeOnInitializeGameDataModule();
		}

		// Token: 0x06004D90 RID: 19856 RVA: 0x002AD624 File Offset: 0x002AB824
		public unsafe override void OnEnterNewWorld()
		{
			this.InitializeOnEnterNewWorld();
			this.InitializeInternalDataOfCollections();
			byte* pData = OperationAdder.FixedElementList_InsertRange(11, 0, 0, 14, 56);
			for (int i = 0; i < 14; i++)
			{
				*(int*)(pData + (IntPtr)i * 4) = this._bookOwners[i];
			}
			pData += 56;
		}

		// Token: 0x06004D91 RID: 19857 RVA: 0x002AD675 File Offset: 0x002AB875
		public override void OnLoadWorld()
		{
			this._pendingLoadingOperationIds = new Queue<uint>();
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedElementList_GetAll(11, 0));
		}

		// Token: 0x06004D92 RID: 19858 RVA: 0x002AD698 File Offset: 0x002AB898
		public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
		{
			int result;
			if (dataId != 0)
			{
				if (dataId != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 1);
				}
				result = Serializer.Serialize(this._legendaryBookOwnerData, dataPool);
			}
			else
			{
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this._dataStatesBookOwners, (int)subId0);
				}
				result = Serializer.Serialize(this._bookOwners[(int)subId0], dataPool);
			}
			return result;
		}

		// Token: 0x06004D93 RID: 19859 RVA: 0x002AD730 File Offset: 0x002AB930
		public override void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			if (dataId != 0)
			{
				if (dataId != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				Serializer.Deserialize(dataPool, valueOffset, ref this._legendaryBookOwnerData);
				this.SetLegendaryBookOwnerData(this._legendaryBookOwnerData, context);
			}
			else
			{
				int value = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value);
				this._bookOwners[(int)subId0] = value;
				this.SetElement_BookOwners((int)subId0, value, context);
			}
		}

		// Token: 0x06004D94 RID: 19860 RVA: 0x002AD7C4 File Offset: 0x002AB9C4
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
				if (num != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<IntPair> returnValue = this.GmCmd_GetAllLegendaryBookStates();
				result = Serializer.Serialize(returnValue, returnDataPool);
				break;
			}
			case 1:
			{
				int argsCount2 = operation.ArgsCount;
				int num2 = argsCount2;
				if (num2 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.GmCmd_GiveAllTaiwuLegendaryBookToRandomNpc(context);
				result = -1;
				break;
			}
			case 2:
			{
				int argsCount3 = operation.ArgsCount;
				int num3 = argsCount3;
				if (num3 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				LegendaryBookIncrementData returnValue2 = this.GetLegendaryBookIncrementData();
				result = Serializer.Serialize(returnValue2, returnDataPool);
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
				int returnValue3 = this.GetAllLegendaryBooksOwningState();
				result = Serializer.Serialize(returnValue3, returnDataPool);
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

		// Token: 0x06004D95 RID: 19861 RVA: 0x002AD99C File Offset: 0x002ABB9C
		public override void OnMonitorData(ushort dataId, ulong subId0, uint subId1, bool monitoring)
		{
			if (dataId != 0)
			{
				if (dataId != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
		}

		// Token: 0x06004D96 RID: 19862 RVA: 0x002AD9EC File Offset: 0x002ABBEC
		public override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
		{
			int result;
			if (dataId != 0)
			{
				if (dataId != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag = !BaseGameDataDomain.IsModified(this.DataStates, 1);
				if (flag)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 1);
					result = Serializer.Serialize(this._legendaryBookOwnerData, dataPool);
				}
			}
			else
			{
				bool flag2 = !BaseGameDataDomain.IsModified(this._dataStatesBookOwners, (int)subId0);
				if (flag2)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this._dataStatesBookOwners, (int)subId0);
					result = Serializer.Serialize(this._bookOwners[(int)subId0], dataPool);
				}
			}
			return result;
		}

		// Token: 0x06004D97 RID: 19863 RVA: 0x002ADAAC File Offset: 0x002ABCAC
		public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			if (dataId != 0)
			{
				if (dataId != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag = !BaseGameDataDomain.IsModified(this.DataStates, 1);
				if (!flag)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 1);
				}
			}
			else
			{
				bool flag2 = !BaseGameDataDomain.IsModified(this._dataStatesBookOwners, (int)subId0);
				if (!flag2)
				{
					BaseGameDataDomain.ResetModified(this._dataStatesBookOwners, (int)subId0);
				}
			}
		}

		// Token: 0x06004D98 RID: 19864 RVA: 0x002ADB44 File Offset: 0x002ABD44
		public override bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			bool result;
			if (dataId != 0)
			{
				if (dataId != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = BaseGameDataDomain.IsModified(this.DataStates, 1);
			}
			else
			{
				result = BaseGameDataDomain.IsModified(this._dataStatesBookOwners, (int)subId0);
			}
			return result;
		}

		// Token: 0x06004D99 RID: 19865 RVA: 0x002ADBB0 File Offset: 0x002ABDB0
		public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
		{
			ushort dataId = influence.TargetIndicator.DataId;
			ushort num = dataId;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (num != 0)
			{
				if (num != 1)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(influence.TargetIndicator.DataId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Cannot invalidate cache state of non-cache data ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(influence.TargetIndicator.DataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06004D9A RID: 19866 RVA: 0x002ADC4C File Offset: 0x002ABE4C
		public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
		{
			ushort dataId = operation.DataId;
			ushort num = dataId;
			if (num == 0)
			{
				ResponseProcessor.ProcessElementList_BasicType_Fixed_Value<int>(operation, pResult, this._bookOwners, 14, 4);
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
							DomainManager.Global.CompleteLoading(11);
						}
					}
				}
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (num != 1)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.DataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Cannot process archive response of non-archive data ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.DataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06004D9B RID: 19867 RVA: 0x002ADD66 File Offset: 0x002ABF66
		private void InitializeInternalDataOfCollections()
		{
		}

		// Token: 0x04001564 RID: 5476
		[DomainData(DomainDataType.ElementList, true, false, true, true, ArrayElementsCount = 14)]
		private readonly int[] _bookOwners;

		// Token: 0x04001565 RID: 5477
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private LegendaryBookOwnerData _legendaryBookOwnerData;

		// Token: 0x04001566 RID: 5478
		private Dictionary<int, List<sbyte>> _charBookTypes;

		// Token: 0x04001567 RID: 5479
		private readonly HashSet<int> _actCrazyShockedCharIds = new HashSet<int>();

		// Token: 0x04001568 RID: 5480
		private readonly LegendaryBookMonthlyAction[] _legendaryBookMonthlyActions = new LegendaryBookMonthlyAction[14];

		// Token: 0x04001569 RID: 5481
		private readonly ItemKey[] _legendaryBookItems = new ItemKey[14];

		// Token: 0x0400156A RID: 5482
		private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[2][];

		// Token: 0x0400156B RID: 5483
		private static readonly DataInfluence[][] CacheInfluencesBookOwners = new DataInfluence[14][];

		// Token: 0x0400156C RID: 5484
		private readonly byte[] _dataStatesBookOwners = new byte[4];

		// Token: 0x0400156D RID: 5485
		private Queue<uint> _pendingLoadingOperationIds;
	}
}
