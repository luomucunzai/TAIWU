using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using Config.ConfigCells.Character;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Building;
using GameData.Domains.Character.Creation;
using GameData.Domains.Character.ParallelModifications;
using GameData.Domains.CombatSkill;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.Taiwu;
using GameData.Domains.Taiwu.VillagerRole;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Character.Ai
{
	// Token: 0x0200084D RID: 2125
	public class Equipping
	{
		// Token: 0x06007623 RID: 30243 RVA: 0x0044F898 File Offset: 0x0044DA98
		public Equipping()
		{
			this._brokenOutCombatSkills = new List<CombatSkillInitialBreakoutData>(32);
			this._brokenOutNeigongList = new List<ValueTuple<CombatSkillItem, int>>();
			this._combatSkillAttainmentPanels = new short[126];
			this._sectCandidateSkillsPool = new LocalObjectPool<SectCandidateSkills>(15, 30);
			this._sectCandidateSkillInfos = new List<SectCandidateSkills>();
			this._sortedSectCandidateSkillInfos = new List<SectCandidateSkills>();
			this._availableCombatSkills = new List<GameData.Domains.CombatSkill.CombatSkill>[5];
			for (int i = 0; i < 5; i++)
			{
				this._availableCombatSkills[i] = new List<GameData.Domains.CombatSkill.CombatSkill>();
			}
			this._equippedCombatSkills = new CombatSkillEquipment();
			this._equippedCombatSkills.Set(new CombatSkillPlan());
			this._categorizedCombatSkillsByGrade = new List<GameData.Domains.CombatSkill.CombatSkill>[9];
			for (sbyte grade = 0; grade <= 8; grade += 1)
			{
				this._categorizedCombatSkillsByGrade[(int)grade] = new List<GameData.Domains.CombatSkill.CombatSkill>();
			}
			this._masteredSkills = new List<short>();
			this._candidateCombatSkillsForLooping = new List<ValueTuple<CombatSkillItem, bool>>();
			this._equippedItems = new ItemKey[12];
			this._availableWeapons = new List<ValueTuple<ItemKey, int>>();
			this._suitableWeapons = new List<ValueTuple<short, short>>();
			this._fixedBestWeapons = new HashSet<short>();
			this._availableHelms = new List<GameData.Domains.Item.Armor>();
			this._availableTorsos = new List<GameData.Domains.Item.Armor>();
			this._availableBracers = new List<GameData.Domains.Item.Armor>();
			this._availableBoots = new List<GameData.Domains.Item.Armor>();
			this._availableAccessories = new List<GameData.Domains.Item.Accessory>();
			this._availableClothing = new List<GameData.Domains.Item.Clothing>();
			this._availableCarriers = new List<GameData.Domains.Item.Carrier>();
			this._availableReadingBooks = new List<ValueTuple<GameData.Domains.Item.SkillBook, int, byte>>();
			this._hasPersonalNeedToReadBooks = new List<short>();
			this._hasPersonalNeedToLearnCombatSkillTypes = new List<sbyte>();
			this._hasPersonalNeedToLearnLifeSkillTypes = new List<sbyte>();
			this._usedRelatedCharIds = new HashSet<int>();
		}

		// Token: 0x06007624 RID: 30244 RVA: 0x0044FA8C File Offset: 0x0044DC8C
		public void SetInitialCombatSkillBreakouts(DataContext context, Character character)
		{
			bool flag = character.GetAgeGroup() == 0;
			if (!flag)
			{
				ValueTuple<List<CombatSkillInitialBreakoutData>, List<ValueTuple<short, int, SerializableList<SkillBreakPlateBonus>>>, NeiliProportionOfFiveElements, int[]> valueTuple = this.ParallelSetInitialCombatSkillBreakouts(context, character, false);
				List<CombatSkillInitialBreakoutData> brokenOutSkills = valueTuple.Item1;
				List<ValueTuple<short, int, SerializableList<SkillBreakPlateBonus>>> breakPlateBonuses = valueTuple.Item2;
				NeiliProportionOfFiveElements neiliProportion = valueTuple.Item3;
				int[] extraNeiliAllocationProgress = valueTuple.Item4;
				bool flag2 = brokenOutSkills != null && brokenOutSkills.Count > 0;
				if (flag2)
				{
					Equipping.ComplementSetInitialCombatSkillBreakouts(context, brokenOutSkills, character, neiliProportion, extraNeiliAllocationProgress);
				}
				bool flag3 = breakPlateBonuses != null && breakPlateBonuses.Count > 0;
				if (flag3)
				{
					Equipping.ApplyBreakPlateBonuses(context, character.GetId(), breakPlateBonuses);
				}
			}
		}

		// Token: 0x06007625 RID: 30245 RVA: 0x0044FB14 File Offset: 0x0044DD14
		public void SetInitialCombatSkillAttainmentPanels(DataContext context, Character character)
		{
			bool flag = character.GetAgeGroup() == 0;
			if (!flag)
			{
				short[] panels = this.ParallelSetInitialCombatSkillAttainmentPanels(context, character, false);
				bool flag2 = panels != null;
				if (flag2)
				{
					Equipping.ComplementSetInitialCombatSkillAttainmentPanels(context, character, panels);
				}
			}
		}

		// Token: 0x06007626 RID: 30246 RVA: 0x0044FB4C File Offset: 0x0044DD4C
		public void SelectEquipments(DataContext context, Character character, bool isOutOfTaiwuGroup, bool removeUnequippedEquipment = false)
		{
			bool flag = character.GetAgeGroup() == 0;
			if (!flag)
			{
				SelectEquipmentsModification mod = this.ParallelSelectEquipments(context, character, isOutOfTaiwuGroup, removeUnequippedEquipment, false);
				Equipping.ComplementSelectEquipments(context, mod);
			}
		}

		// Token: 0x06007627 RID: 30247 RVA: 0x0044FB80 File Offset: 0x0044DD80
		public void SelectEquipmentsByCombatConfig(DataContext context, Character character, short combatTemplateId, bool isOutOfTaiwuGroup, bool removeUnequippedEquipment = false)
		{
			bool flag = character.GetAgeGroup() == 0;
			if (!flag)
			{
				SelectEquipmentsModification mod = this.ParallelSelectEquipmentsByCombatConfig(context, character, combatTemplateId, isOutOfTaiwuGroup, removeUnequippedEquipment, false);
				Equipping.ComplementSelectEquipments(context, mod);
			}
		}

		// Token: 0x06007628 RID: 30248 RVA: 0x0044FBB4 File Offset: 0x0044DDB4
		public unsafe SelectEquipmentsModification ParallelSelectEquipments(DataContext context, Character character, bool isOutOfTaiwuGroup, bool removeUnequippedEquipment = false, bool recordModification = true)
		{
			SelectEquipmentsModification mod = new SelectEquipmentsModification(character, removeUnequippedEquipment);
			int charId = character.GetId();
			sbyte* skillSlotTotalCounts = stackalloc sbyte[(UIntPtr)5];
			CharacterCombatSkillConfiguration configuration = DomainManager.Extra.TryGetCharacterCombatSkillConfiguration(charId);
			bool canAutoEquipCombatSkills = isOutOfTaiwuGroup || (configuration == null || !configuration.IsCombatSkillLocked);
			bool canAutoAllocateNeili = isOutOfTaiwuGroup || (configuration == null || !configuration.IsNeiliAllocationLocked);
			bool canAutoEquipItems = !character.IsCreatedWithFixedTemplate() && (isOutOfTaiwuGroup || !DomainManager.Extra.GetManualChangeEquipGroupCharIds().Contains(charId));
			this.ChooseLoopingNeigong(character, mod);
			bool flag = canAutoEquipCombatSkills;
			if (flag)
			{
				this.EquipCombatSkills(character, skillSlotTotalCounts, -1, mod);
			}
			bool flag2 = canAutoAllocateNeili;
			if (flag2)
			{
				Equipping.AllocateNeili(character, skillSlotTotalCounts, mod);
			}
			bool flag3 = canAutoEquipItems;
			if (flag3)
			{
				this.EquipItems(character, mod);
			}
			bool flag4 = recordModification && (mod.EquippedSkillsChanged || mod.NeiliAllocationChanged || mod.LoopingNeigongChanged || mod.EquippedItems != null || mod.MasteredSkillsChanged);
			if (flag4)
			{
				ParallelModificationsRecorder recorder = context.ParallelModificationsRecorder;
				recorder.RecordType(ParallelModificationType.SelectEquipments);
				recorder.RecordParameterClass<SelectEquipmentsModification>(mod);
			}
			return mod;
		}

		// Token: 0x06007629 RID: 30249 RVA: 0x0044FCD4 File Offset: 0x0044DED4
		private unsafe SelectEquipmentsModification ParallelSelectEquipmentsByCombatConfig(DataContext context, Character character, short combatConfigTemplateId, bool isOutOfTaiwuGroup, bool removeUnequippedEquipment = false, bool recordModification = true)
		{
			SelectEquipmentsModification mod = new SelectEquipmentsModification(character, removeUnequippedEquipment);
			sbyte* skillSlotTotalCounts = stackalloc sbyte[(UIntPtr)5];
			int charId = character.GetId();
			CharacterCombatSkillConfiguration configuration = DomainManager.Extra.TryGetCharacterCombatSkillConfiguration(charId);
			bool canAutoEquipCombatSkills = isOutOfTaiwuGroup || (configuration == null || !configuration.IsCombatSkillLocked);
			bool canAutoAllocateNeili = isOutOfTaiwuGroup || (configuration == null || !configuration.IsNeiliAllocationLocked);
			bool canAutoEquipItems = !character.IsCreatedWithFixedTemplate() && (isOutOfTaiwuGroup || !DomainManager.Extra.GetManualChangeEquipGroupCharIds().Contains(charId));
			this.ChooseLoopingNeigong(character, mod);
			bool flag = canAutoEquipCombatSkills;
			if (flag)
			{
				this.EquipCombatSkills(character, skillSlotTotalCounts, combatConfigTemplateId, mod);
			}
			bool flag2 = canAutoAllocateNeili;
			if (flag2)
			{
				Equipping.AllocateNeili(character, skillSlotTotalCounts, mod);
			}
			bool flag3 = canAutoEquipItems;
			if (flag3)
			{
				this.EquipItems(character, mod);
			}
			bool flag4 = recordModification && (mod.EquippedSkillsChanged || mod.NeiliAllocationChanged || mod.LoopingNeigongChanged || mod.EquippedItems != null || mod.MasteredSkillsChanged);
			if (flag4)
			{
				ParallelModificationsRecorder recorder = context.ParallelModificationsRecorder;
				recorder.RecordType(ParallelModificationType.SelectEquipments);
				recorder.RecordParameterClass<SelectEquipmentsModification>(mod);
			}
			return mod;
		}

		// Token: 0x0600762A RID: 30250 RVA: 0x0044FDF8 File Offset: 0x0044DFF8
		public static void ComplementSelectEquipments(DataContext context, SelectEquipmentsModification mod)
		{
			Character character = mod.Character;
			int charId = character.GetId();
			bool equippedSkillsChanged = mod.EquippedSkillsChanged;
			if (equippedSkillsChanged)
			{
				character.ApplyCombatSkillEquipmentModification(context, mod.CombatSkillEquipment);
			}
			bool neiliAllocationChanged = mod.NeiliAllocationChanged;
			if (neiliAllocationChanged)
			{
				character.SpecifyBaseNeiliAllocation(context, mod.NeiliAllocation);
			}
			bool loopingNeigongChanged = mod.LoopingNeigongChanged;
			if (loopingNeigongChanged)
			{
				character.SetLoopingNeigong(mod.LoopingNeigong, context);
			}
			bool flag = mod.EquippedItems != null;
			if (flag)
			{
				character.ChangeEquipment(context, mod.EquippedItems);
			}
			bool removeUnequippedEquipment = mod.RemoveUnequippedEquipment;
			if (removeUnequippedEquipment)
			{
				character.RemoveUnequippedEquipment(context);
			}
			bool personalNeedChanged = mod.PersonalNeedChanged;
			if (personalNeedChanged)
			{
				character.SetPersonalNeeds(character.GetPersonalNeeds(), context);
			}
			bool masteredSkillsChanged = mod.MasteredSkillsChanged;
			if (masteredSkillsChanged)
			{
				DomainManager.Extra.SetCharacterMasteredCombatSkills(context, charId, mod.MasteredCombatSkills);
			}
		}

		// Token: 0x0600762B RID: 30251 RVA: 0x0044FEC8 File Offset: 0x0044E0C8
		public unsafe void EquipCombatSkills(DataContext context, Character character, short combatConfigTemplateId)
		{
			int charId = character.GetId();
			SelectEquipmentsModification mod = new SelectEquipmentsModification(character, false);
			sbyte* skillSlotTotalCounts = stackalloc sbyte[(UIntPtr)5];
			this.EquipCombatSkills(character, skillSlotTotalCounts, combatConfigTemplateId, mod);
			bool masteredSkillsChanged = mod.MasteredSkillsChanged;
			if (masteredSkillsChanged)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 1);
				defaultInterpolatedStringHandler.AppendFormatted<Character>(character);
				defaultInterpolatedStringHandler.AppendLiteral(" changed mastered skills.");
				AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
				DomainManager.Extra.SetCharacterMasteredCombatSkills(context, charId, mod.MasteredCombatSkills);
			}
			bool equippedSkillsChanged = mod.EquippedSkillsChanged;
			if (equippedSkillsChanged)
			{
				bool flag = mod.GenericSkillSlotAllocation != null;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (flag)
				{
					int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
					bool flag2 = charId == taiwuCharId;
					if (flag2)
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 1);
						defaultInterpolatedStringHandler.AppendFormatted<Character>(character);
						defaultInterpolatedStringHandler.AppendLiteral(" changed generic skill slot allocation.");
						AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
						DomainManager.Taiwu.SetGenericGridAllocation(context, mod.GenericSkillSlotAllocation);
					}
					else
					{
						bool flag3 = character.GetLeaderId() == taiwuCharId;
						if (flag3)
						{
							CharacterCombatSkillConfiguration configuration = DomainManager.Extra.TryGetCharacterCombatSkillConfiguration(charId);
							bool flag4 = configuration != null;
							if (flag4)
							{
								defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 1);
								defaultInterpolatedStringHandler.AppendFormatted<Character>(character);
								defaultInterpolatedStringHandler.AppendLiteral(" changed generic skill slot allocation.");
								AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
								byte[] allocation = configuration.CurrentEquipPlan.GenericGridAllocation;
								for (int i = 0; i < allocation.Length; i++)
								{
									allocation[i] = mod.GenericSkillSlotAllocation[i];
								}
							}
						}
					}
				}
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 1);
				defaultInterpolatedStringHandler.AppendFormatted<Character>(character);
				defaultInterpolatedStringHandler.AppendLiteral(" changed equipped skills.");
				AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
				character.ApplyCombatSkillEquipmentModification(context, mod.CombatSkillEquipment);
			}
		}

		// Token: 0x0600762C RID: 30252 RVA: 0x0045008C File Offset: 0x0044E28C
		private unsafe void EquipCombatSkills(Character character, sbyte* skillSlotTotalCounts, short combatConfigTemplateId, SelectEquipmentsModification mod)
		{
			int charId = character.GetId();
			for (int i = 0; i < 5; i++)
			{
				this._availableCombatSkills[i].Clear();
			}
			Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(charId);
			CombatConfigItem combatConfig = CombatConfig.Instance.GetItem(combatConfigTemplateId);
			foreach (KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> keyValuePair in charCombatSkills)
			{
				short num;
				GameData.Domains.CombatSkill.CombatSkill combatSkill;
				keyValuePair.Deconstruct(out num, out combatSkill);
				short skillTemplateId = num;
				GameData.Domains.CombatSkill.CombatSkill skill = combatSkill;
				sbyte equipType = Config.CombatSkill.Instance[skillTemplateId].EquipType;
				bool revoked = skill.GetRevoked();
				if (!revoked)
				{
					bool flag = combatConfig != null && !this.MatchCombatSkillByCombatConfig(skillTemplateId, combatConfig);
					if (!flag)
					{
						this._availableCombatSkills[(int)equipType].Add(skill);
					}
				}
			}
			this._equippedCombatSkills.OfflineClear();
			CombatSkillEquipment oriCombatSkillEquipment = character.GetCombatSkillEquipment();
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			bool isTaiwu = charId == taiwuCharId;
			Equipping.EquipCombatSkillContext context = new Equipping.EquipCombatSkillContext
			{
				IsTaiwu = isTaiwu,
				EquipBestSkillsForWeapon = (character.GetLeaderId() == taiwuCharId),
				Equipments = character.GetEquipment(),
				Personalities = character.GetPersonalities(),
				NeiliType = character.GetNeiliType(),
				OrgTemplateId = character.GetOrganizationInfo().OrgTemplateId,
				IdealSectId = character.GetIdealSect(),
				OwnedLegendaryBookTypes = DomainManager.LegendaryBook.GetCharOwnedBookTypes(charId),
				EquippedSkills = this._equippedCombatSkills,
				CharacterCombatSkills = charCombatSkills,
				SlotTotalCounts = skillSlotTotalCounts
			};
			*skillSlotTotalCounts = character.GetCombatSkillSlotCountNeigong();
			this.SelectCombatSkills(ref context, 0);
			Span<sbyte> gridCounts = new Span<sbyte>((void*)skillSlotTotalCounts, 5);
			sbyte genericSlotCount = character.GetCombatSkillSlotCounts(gridCounts, context.EquippedSkills.Neigong);
			Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)4], 4);
			Span<byte> allocatedGenericSlots = span;
			span = new Span<byte>(stackalloc byte[(UIntPtr)4], 4);
			Span<byte> slotCountsProvidedByNeigong = span;
			for (sbyte equipType2 = 1; equipType2 < 5; equipType2 += 1)
			{
				*slotCountsProvidedByNeigong[(int)(equipType2 - 1)] = (byte)character.GetCombatSkillBasicSlotCount(equipType2, context.EquippedSkills.Neigong);
			}
			this.AllocateGenericSkillSlots(ref allocatedGenericSlots, ref slotCountsProvidedByNeigong, (int)genericSlotCount);
			for (sbyte index = 0; index < 4; index += 1)
			{
				int equipType3 = (int)(index + 1);
				int result = (int)(skillSlotTotalCounts[equipType3] + (sbyte)(*allocatedGenericSlots[(int)index]));
				skillSlotTotalCounts[equipType3] = (sbyte)result;
			}
			bool isTaiwu2 = context.IsTaiwu;
			if (isTaiwu2)
			{
				mod.GenericSkillSlotAllocation = new byte[4];
				for (int j = 0; j < 4; j++)
				{
					mod.GenericSkillSlotAllocation[j] = *allocatedGenericSlots[j];
				}
			}
			this.SelectCombatSkills(ref context, 1);
			this.SelectCombatSkills(ref context, 2);
			this.SelectCombatSkills(ref context, 3);
			this.SelectCombatSkills(ref context, 4);
			bool flag2 = !oriCombatSkillEquipment.EqualsTo(context.EquippedSkills);
			if (flag2)
			{
				oriCombatSkillEquipment.CopyFrom(context.EquippedSkills);
				mod.CombatSkillEquipment = oriCombatSkillEquipment;
				mod.EquippedSkillsChanged = true;
			}
		}

		// Token: 0x0600762D RID: 30253 RVA: 0x004503B4 File Offset: 0x0044E5B4
		private unsafe void AllocateGenericSkillSlots(ref Span<byte> result, ref Span<byte> currSlotCounts, int genericSlotsCount)
		{
			result.Fill(0);
			do
			{
				int minCost = int.MaxValue;
				int minCostEquipType = -1;
				for (sbyte equipType = 1; equipType < 5; equipType += 1)
				{
					int index = (int)(equipType - 1);
					bool flag = *result[index] + *currSlotCounts[index] >= (byte)CombatSkillHelper.MaxSlotCounts[(int)equipType];
					if (!flag)
					{
						int currCost = CombatSkillHelper.GetGenericAllocationNextCost(equipType, (int)(*result[index]));
						bool flag2 = currCost > minCost || currCost > genericSlotsCount;
						if (!flag2)
						{
							minCost = currCost;
							minCostEquipType = (int)equipType;
						}
					}
				}
				bool flag3 = minCostEquipType < 0;
				if (flag3)
				{
					break;
				}
				ref byte ptr = ref result[minCostEquipType - 1];
				ptr += 1;
				genericSlotsCount -= minCost;
			}
			while (genericSlotsCount > 0);
		}

		// Token: 0x0600762E RID: 30254 RVA: 0x0045046C File Offset: 0x0044E66C
		private unsafe void SelectCombatSkills(ref Equipping.EquipCombatSkillContext context, sbyte equipType)
		{
			sbyte slotCount = context.SlotTotalCounts[equipType];
			bool flag = slotCount <= 0;
			if (!flag)
			{
				List<GameData.Domains.CombatSkill.CombatSkill> candidateSkills = this._availableCombatSkills[(int)equipType];
				int candidateSkillsCount = candidateSkills.Count;
				int* pSkillInfos = stackalloc int[checked(unchecked((UIntPtr)candidateSkillsCount) * 4)];
				for (int i = 0; i < candidateSkillsCount; i++)
				{
					GameData.Domains.CombatSkill.CombatSkill skill = candidateSkills[i];
					short skillTemplateId = skill.GetId().SkillTemplateId;
					CombatSkillItem skillConfig = Config.CombatSkill.Instance[skillTemplateId];
					bool flag2 = skillConfig.ScoreBonusType == -2;
					if (flag2)
					{
						pSkillInfos[i] = 2147418112 + (int)skillTemplateId;
					}
					else
					{
						short score = Equipping.CalcCombatSkillScore(skill, equipType, ref context.Personalities, context.NeiliType, context.OrgTemplateId, context.IdealSectId, context.OwnedLegendaryBookTypes);
						bool equipBestSkillsForWeapon = context.EquipBestSkillsForWeapon;
						if (equipBestSkillsForWeapon)
						{
							score += Equipping.CalcCombatSkillScoreForCurrWeapons(skillConfig, context.Equipments);
						}
						pSkillInfos[i] = ((int)score << 16) + (int)skillTemplateId;
					}
				}
				CollectionUtils.Sort(pSkillInfos, candidateSkillsCount);
				int slotsUsed = 0;
				context.EquippedSkills.OfflineEnsureCapacity(equipType, (int)slotCount);
				ref ArraySegmentList<short> skillList = ref context.EquippedSkills[equipType];
				skillList.Clear();
				bool isTaiwu = context.IsTaiwu;
				if (isTaiwu)
				{
					Character taiwu = DomainManager.Taiwu.GetTaiwu();
					int j = 0;
					while (j < candidateSkillsCount && slotsUsed < (int)slotCount)
					{
						int selectedIndex = candidateSkillsCount - j - 1;
						int skillInfo = pSkillInfos[selectedIndex];
						short skillTemplateId2 = (short)skillInfo;
						sbyte slotCost = taiwu.GetCombatSkillGridCost(skillTemplateId2);
						bool flag3 = slotsUsed + (int)slotCost > (int)slotCount;
						if (!flag3)
						{
							skillList.Add(skillTemplateId2);
							slotsUsed += (int)slotCost;
						}
						j++;
					}
				}
				else
				{
					int k = 0;
					while (k < candidateSkillsCount && slotsUsed < (int)slotCount)
					{
						int selectedIndex2 = candidateSkillsCount - k - 1;
						int skillInfo2 = pSkillInfos[selectedIndex2];
						short skillTemplateId3 = (short)skillInfo2;
						ValueTuple<sbyte, bool> valueTuple = Equipping.CalcSlotCostInfo(context, skillTemplateId3);
						sbyte slotCost2 = valueTuple.Item1;
						bool isMastered = valueTuple.Item2;
						bool flag4 = slotsUsed + (int)slotCost2 > (int)slotCount;
						if (!flag4)
						{
							skillList.Add(skillTemplateId3);
							slotsUsed += (int)slotCost2;
						}
						k++;
					}
				}
			}
		}

		// Token: 0x0600762F RID: 30255 RVA: 0x00450690 File Offset: 0x0044E890
		[return: TupleElementNames(new string[]
		{
			"slotCost",
			"isMastered"
		})]
		private static ValueTuple<sbyte, bool> CalcSlotCostInfo(Equipping.EquipCombatSkillContext context, short skillTemplateId)
		{
			CombatSkillItem skillCfg = Config.CombatSkill.Instance[skillTemplateId];
			sbyte gridCost = skillCfg.GridCost;
			return new ValueTuple<sbyte, bool>(Math.Max(gridCost, 1), false);
		}

		// Token: 0x06007630 RID: 30256 RVA: 0x004506C4 File Offset: 0x0044E8C4
		private static short CalcCombatSkillScoreForCurrWeapons(CombatSkillItem skillCfg, ItemKey[] equipments)
		{
			int score = 0;
			for (int i = 0; i <= 2; i++)
			{
				ItemKey itemKey = equipments[i];
				bool flag = itemKey.ItemType != 0;
				if (!flag)
				{
					GameData.Domains.Item.Weapon weapon = DomainManager.Item.GetElement_Weapons(itemKey.Id);
					WeaponItem weaponCfg = Config.Weapon.Instance[itemKey.TemplateId];
					bool flag2 = weaponCfg.GroupId == skillCfg.MostFittingWeaponID;
					if (flag2)
					{
						score += (int)(50 * ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId));
					}
					bool flag3 = weapon.TricksMatchCombatSkill(skillCfg);
					if (flag3)
					{
						score += 100;
					}
				}
			}
			return (short)score;
		}

		// Token: 0x06007631 RID: 30257 RVA: 0x00450774 File Offset: 0x0044E974
		public unsafe static short CalcCombatSkillScore(GameData.Domains.CombatSkill.CombatSkill skill, sbyte equipType, ref Personalities personalities, sbyte neiliType, sbyte orgTemplateId, sbyte idealSectTemplateId, List<sbyte> ownedLegendaryBookTypes)
		{
			short skillTemplateId = skill.GetId().SkillTemplateId;
			CombatSkillItem skillConfig = Config.CombatSkill.Instance[skillTemplateId];
			bool flag = skillConfig.ScoreBonusType == -2;
			short result;
			if (flag)
			{
				result = short.MaxValue;
			}
			else
			{
				short score = 0;
				bool flag2 = skillConfig.SectId == orgTemplateId;
				if (flag2)
				{
					score += (short)(150 + (int)personalities.Items.FixedElementField);
				}
				bool flag3 = skillConfig.SectId == idealSectTemplateId;
				if (flag3)
				{
					score += (short)(75 + *(ref personalities.Items.FixedElementField + 2));
				}
				bool flag4 = !Equipping.CheckCounterWithNeiliType(skillConfig.FiveElements, neiliType);
				if (flag4)
				{
					score += (short)(150 + (int)personalities.Items.FixedElementField);
				}
				score += (short)(50 * skillConfig.Grade);
				score += skill.GetPower();
				bool flag5 = CombatSkillStateHelper.IsBrokenOut(skill.GetActivationState());
				if (flag5)
				{
					score += 100;
				}
				score += (short)(50 * (skillConfig.GridCost - 1));
				bool flag6 = ownedLegendaryBookTypes != null && equipType == 1 && ownedLegendaryBookTypes.Contains(skillConfig.Type);
				if (flag6)
				{
					score += 300;
				}
				result = score;
			}
			return result;
		}

		// Token: 0x06007632 RID: 30258 RVA: 0x004508A0 File Offset: 0x0044EAA0
		public static bool CheckCounterWithNeiliType(sbyte fiveElementsType, sbyte neiliTypeId)
		{
			NeiliTypeItem neiliTypeCfg = NeiliType.Instance[neiliTypeId];
			return neiliTypeCfg.InjuryOnUseType == fiveElementsType || neiliTypeCfg.MaxPowerChange[(int)fiveElementsType] < 0;
		}

		// Token: 0x06007633 RID: 30259 RVA: 0x004508D8 File Offset: 0x0044EAD8
		public static bool CheckCounterWithTargetFiveElementsType(short fiveElementsType, sbyte targetFiveElementsType)
		{
			return targetFiveElementsType == 5 || ((short)FiveElementsType.Countered[(int)targetFiveElementsType] != fiveElementsType && (short)FiveElementsType.Countering[(int)targetFiveElementsType] != fiveElementsType);
		}

		// Token: 0x06007634 RID: 30260 RVA: 0x0045090C File Offset: 0x0044EB0C
		public short[] ParallelSetInitialCombatSkillAttainmentPanels(DataContext context, Character character, bool recordModification = true)
		{
			short[] panels = this._combatSkillAttainmentPanels ?? new short[126];
			CombatSkillAttainmentPanelsHelper.Initialize(panels);
			int charId = character.GetId();
			sbyte selfOrgTemplateId = character.GetOrganizationInfo().OrgTemplateId;
			sbyte lovingOrgTemplateId = character.GetIdealSect();
			Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(charId);
			for (sbyte skillType = 0; skillType < 14; skillType += 1)
			{
				this.SetCombatSkillAttainmentPanel(charCombatSkills, selfOrgTemplateId, lovingOrgTemplateId, panels, skillType);
			}
			short[] oriPanels = character.GetCombatSkillAttainmentPanels();
			bool flag = CombatSkillAttainmentPanelsHelper.EqualAll(oriPanels, panels);
			short[] result;
			if (flag)
			{
				this._combatSkillAttainmentPanels = panels;
				result = null;
			}
			else
			{
				if (recordModification)
				{
					ParallelModificationsRecorder recorder = context.ParallelModificationsRecorder;
					recorder.RecordType(ParallelModificationType.SetInitialCombatSkillAttainmentPanels);
					recorder.RecordParameterClass<Character>(character);
					recorder.RecordParameterClass<short[]>(panels);
				}
				this._combatSkillAttainmentPanels = null;
				result = panels;
			}
			return result;
		}

		// Token: 0x06007635 RID: 30261 RVA: 0x004509E0 File Offset: 0x0044EBE0
		public static void ComplementSetInitialCombatSkillAttainmentPanels(DataContext context, Character character, short[] panels)
		{
			short[] oriPanels = character.GetCombatSkillAttainmentPanels();
			CombatSkillAttainmentPanelsHelper.CopyAll(panels, oriPanels);
			character.SetCombatSkillAttainmentPanels(oriPanels, context);
		}

		// Token: 0x06007636 RID: 30262 RVA: 0x00450A08 File Offset: 0x0044EC08
		private unsafe void SetCombatSkillAttainmentPanel(Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills, sbyte selfOrgTemplateId, sbyte lovingOrgTemplateId, short[] panels, sbyte combatSkillType)
		{
			int i = 0;
			int count = this._sectCandidateSkillInfos.Count;
			while (i < count)
			{
				this._sectCandidateSkillsPool.Return(this._sectCandidateSkillInfos[i]);
				i++;
			}
			this._sectCandidateSkillInfos.Clear();
			foreach (KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> keyValuePair in charCombatSkills)
			{
				short num;
				GameData.Domains.CombatSkill.CombatSkill combatSkill;
				keyValuePair.Deconstruct(out num, out combatSkill);
				short skillTemplateId = num;
				GameData.Domains.CombatSkill.CombatSkill skill = combatSkill;
				CombatSkillItem skillConfig = Config.CombatSkill.Instance[skillTemplateId];
				bool flag = skillConfig.Type != combatSkillType;
				if (!flag)
				{
					ushort activationState = skill.GetActivationState();
					bool flag2 = !CombatSkillStateHelper.IsBrokenOut(activationState);
					if (!flag2)
					{
						bool revoked = skill.GetRevoked();
						if (!revoked)
						{
							this.SetCombatSkillAttainmentPanel_AddSectCandidateSkill(skillConfig);
						}
					}
				}
			}
			this.SetCombatSkillAttainmentPanel_SortCandidateSects(selfOrgTemplateId, lovingOrgTemplateId);
			IntPtr intPtr = stackalloc byte[(UIntPtr)18];
			initblk(intPtr, 255, 18);
			short* pPanel = intPtr;
			int j = 0;
			int count2 = this._sortedSectCandidateSkillInfos.Count;
			while (j < count2)
			{
				short[] currSkillTemplateIds = this._sortedSectCandidateSkillInfos[j].SkillTemplateIds;
				for (int grade = 0; grade < 9; grade++)
				{
					bool flag3 = pPanel[grade] < 0 && currSkillTemplateIds[grade] >= 0;
					if (flag3)
					{
						pPanel[grade] = currSkillTemplateIds[grade];
					}
				}
				j++;
			}
			CombatSkillAttainmentPanelsHelper.SetPanel(panels, combatSkillType, pPanel);
		}

		// Token: 0x06007637 RID: 30263 RVA: 0x00450BA4 File Offset: 0x0044EDA4
		private void SetCombatSkillAttainmentPanel_AddSectCandidateSkill(CombatSkillItem config)
		{
			sbyte orgTemplateId = config.SectId;
			sbyte grade = config.Grade;
			int index = -1;
			int i = 0;
			int count = this._sectCandidateSkillInfos.Count;
			while (i < count)
			{
				bool flag = this._sectCandidateSkillInfos[i].OrgTemplateId == orgTemplateId;
				if (flag)
				{
					index = i;
					break;
				}
				i++;
			}
			bool flag2 = index >= 0;
			if (flag2)
			{
				SectCandidateSkills info = this._sectCandidateSkillInfos[index];
				info.Add(config.TemplateId, grade);
			}
			else
			{
				SectCandidateSkills info2 = this._sectCandidateSkillsPool.Get();
				info2.Initialize(orgTemplateId);
				info2.Add(config.TemplateId, grade);
				this._sectCandidateSkillInfos.Add(info2);
			}
		}

		// Token: 0x06007638 RID: 30264 RVA: 0x00450C68 File Offset: 0x0044EE68
		private void SetCombatSkillAttainmentPanel_SortCandidateSects(sbyte selfOrgTemplateId, sbyte lovingOrgTemplateId)
		{
			int maxValue = int.MinValue;
			int comboIndex = -1;
			int selfOrgIndex = -1;
			int lovingOrgIndex = -1;
			int i = 0;
			int count = this._sectCandidateSkillInfos.Count;
			while (i < count)
			{
				SectCandidateSkills info = this._sectCandidateSkillInfos[i];
				int value = ((int)info.CombatSkillsCount << 8) + (int)info.MaxGrade;
				bool flag = value > maxValue;
				if (flag)
				{
					maxValue = value;
					comboIndex = i;
				}
				sbyte orgTemplateId = info.OrgTemplateId;
				bool flag2 = orgTemplateId == selfOrgTemplateId;
				if (flag2)
				{
					selfOrgIndex = i;
				}
				else
				{
					bool flag3 = orgTemplateId == lovingOrgTemplateId;
					if (flag3)
					{
						lovingOrgIndex = i;
					}
				}
				i++;
			}
			this._sortedSectCandidateSkillInfos.Clear();
			bool flag4 = comboIndex >= 0;
			if (flag4)
			{
				SectCandidateSkills info2 = this._sectCandidateSkillInfos[comboIndex];
				bool flag5 = info2.CombatSkillsCount >= 3;
				if (flag5)
				{
					this._sortedSectCandidateSkillInfos.Add(info2);
				}
				else
				{
					comboIndex = -1;
				}
			}
			bool flag6 = selfOrgIndex >= 0 && selfOrgIndex != comboIndex;
			if (flag6)
			{
				this._sortedSectCandidateSkillInfos.Add(this._sectCandidateSkillInfos[selfOrgIndex]);
			}
			bool flag7 = lovingOrgIndex >= 0 && lovingOrgIndex != comboIndex && lovingOrgIndex != selfOrgIndex;
			if (flag7)
			{
				this._sortedSectCandidateSkillInfos.Add(this._sectCandidateSkillInfos[lovingOrgIndex]);
			}
			int j = 0;
			int count2 = this._sectCandidateSkillInfos.Count;
			while (j < count2)
			{
				bool flag8 = j == comboIndex || j == selfOrgIndex || j == lovingOrgIndex;
				if (!flag8)
				{
					this._sortedSectCandidateSkillInfos.Add(this._sectCandidateSkillInfos[j]);
				}
				j++;
			}
		}

		// Token: 0x06007639 RID: 30265 RVA: 0x00450E08 File Offset: 0x0044F008
		[return: TupleElementNames(new string[]
		{
			"brokenOutSkills",
			"breakPlateBonuses",
			"neiliProportion",
			"extraNeiliAllocationProgress",
			"skillTemplateId",
			"startIndex",
			"bonuses"
		})]
		public ValueTuple<List<CombatSkillInitialBreakoutData>, List<ValueTuple<short, int, SerializableList<SkillBreakPlateBonus>>>, NeiliProportionOfFiveElements, int[]> ParallelSetInitialCombatSkillBreakouts(DataContext context, Character character, bool recordModification = true)
		{
			this._brokenOutCombatSkills.Clear();
			this._brokenOutNeigongList.Clear();
			this._modifiedBreakPlateBonuses.Clear();
			this.PerformInitialCombatSkillBreakouts(context, character);
			bool flag = this._brokenOutCombatSkills.Count <= 0;
			ValueTuple<List<CombatSkillInitialBreakoutData>, List<ValueTuple<short, int, SerializableList<SkillBreakPlateBonus>>>, NeiliProportionOfFiveElements, int[]> result;
			if (flag)
			{
				result = new ValueTuple<List<CombatSkillInitialBreakoutData>, List<ValueTuple<short, int, SerializableList<SkillBreakPlateBonus>>>, NeiliProportionOfFiveElements, int[]>(null, null, default(NeiliProportionOfFiveElements), new int[4]);
			}
			else
			{
				ValueTuple<NeiliProportionOfFiveElements, int[]> valueTuple = this.PerformInitialNeigongLooping(context, character);
				NeiliProportionOfFiveElements neiliProportion = valueTuple.Item1;
				int[] extraNeiliAllocationProgress = valueTuple.Item2;
				List<CombatSkillInitialBreakoutData> brokenOutCombatSkills = this._brokenOutCombatSkills;
				List<ValueTuple<short, int, SerializableList<SkillBreakPlateBonus>>> breakPlateBonuses = this._modifiedBreakPlateBonuses;
				if (recordModification)
				{
					this._brokenOutCombatSkills = new List<CombatSkillInitialBreakoutData>(32);
					ParallelModificationsRecorder recorder = context.ParallelModificationsRecorder;
					recorder.RecordType(ParallelModificationType.SetInitialCombatSkillBreakouts);
					recorder.RecordParameterClass<List<CombatSkillInitialBreakoutData>>(brokenOutCombatSkills);
					recorder.RecordParameterClass<Character>(character);
					recorder.RecordParameterUnmanaged<NeiliProportionOfFiveElements>(neiliProportion);
					recorder.RecordParameterClass<int[]>(extraNeiliAllocationProgress);
					bool flag2 = this._modifiedBreakPlateBonuses.Count > 0;
					if (flag2)
					{
						this._modifiedBreakPlateBonuses = new List<ValueTuple<short, int, SerializableList<SkillBreakPlateBonus>>>();
						recorder.RecordType(ParallelModificationType.UpdateBreakPlateBonuses);
						recorder.RecordParameterClass<UpdateBreakPlateBonusesModification>(new UpdateBreakPlateBonusesModification(character)
						{
							ModifiedBonuses = breakPlateBonuses
						});
					}
				}
				result = new ValueTuple<List<CombatSkillInitialBreakoutData>, List<ValueTuple<short, int, SerializableList<SkillBreakPlateBonus>>>, NeiliProportionOfFiveElements, int[]>(brokenOutCombatSkills, breakPlateBonuses, neiliProportion, extraNeiliAllocationProgress);
			}
			return result;
		}

		// Token: 0x0600763A RID: 30266 RVA: 0x00450F38 File Offset: 0x0044F138
		public static void ComplementSetInitialCombatSkillBreakouts(DataContext context, List<CombatSkillInitialBreakoutData> brokenOutSkills, Character character, NeiliProportionOfFiveElements neiliProportion, int[] extraNeiliAllocationProgress)
		{
			int i = 0;
			int count = brokenOutSkills.Count;
			while (i < count)
			{
				CombatSkillInitialBreakoutData data = brokenOutSkills[i];
				GameData.Domains.CombatSkill.CombatSkill skill = data.CombatSkill;
				skill.SetActivationState(data.ActivationState, context);
				skill.SetForcedBreakoutStepsCount(data.ForceBreakoutStepsCount, context);
				skill.SetBreakoutStepsCount(data.BreakoutStepsCount, context);
				bool flag = data.ObtainedNeili != 0;
				if (flag)
				{
					skill.SetObtainedNeili(data.ObtainedNeili, context);
				}
				i++;
			}
			bool flag2 = character.Template.PresetNeiliProportionOfFiveElements.Sum() <= 0;
			if (flag2)
			{
				character.SetBaseNeiliProportionOfFiveElements(neiliProportion, context);
			}
			IntList progress = IntList.Create();
			for (int j = 0; j < 4; j++)
			{
				progress.Items.Add(extraNeiliAllocationProgress[j]);
			}
			DomainManager.CombatSkill.SetCharacterExtraNeiliAllocationAndProgress(context, character, progress, true);
		}

		// Token: 0x0600763B RID: 30267 RVA: 0x00451024 File Offset: 0x0044F224
		private void PerformInitialCombatSkillBreakouts(DataContext context, Character character)
		{
			int charId = character.GetId();
			Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(charId);
			Equipping.BreakoutCombatSkillContext breakoutContext = new Equipping.BreakoutCombatSkillContext(context.Random, character);
			foreach (KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> keyValuePair in charCombatSkills)
			{
				short num;
				GameData.Domains.CombatSkill.CombatSkill combatSkill;
				keyValuePair.Deconstruct(out num, out combatSkill);
				short skillTemplateId = num;
				GameData.Domains.CombatSkill.CombatSkill skill = combatSkill;
				ushort activationState = skill.GetActivationState();
				bool flag = !CombatSkillStateHelper.IsBrokenOut(activationState) && skill.CanBreakout();
				if (flag)
				{
					ValueTuple<ushort, sbyte, sbyte> valueTuple = Equipping.CalcCombatSkillBreakoutResult(ref breakoutContext, skill);
					ushort newActivationState = valueTuple.Item1;
					sbyte availableStepsCount = valueTuple.Item2;
					sbyte forcedStepsCount = valueTuple.Item3;
					bool flag2 = CombatSkillStateHelper.IsBrokenOut(newActivationState);
					if (flag2)
					{
						int index = this._brokenOutCombatSkills.Count;
						this._brokenOutCombatSkills.Add(new CombatSkillInitialBreakoutData(skill, newActivationState, availableStepsCount + forcedStepsCount, forcedStepsCount));
						CombatSkillItem skillConfig = Config.CombatSkill.Instance[skillTemplateId];
						bool flag3 = skillConfig.EquipType == 0;
						if (flag3)
						{
							this._brokenOutNeigongList.Add(new ValueTuple<CombatSkillItem, int>(skillConfig, index));
						}
					}
				}
			}
			Equipping.BreakPlateBonusContext breakPlateBonusContext = new Equipping.BreakPlateBonusContext(context.Random, character);
			foreach (List<GameData.Domains.CombatSkill.CombatSkill> list in this._categorizedCombatSkillsByGrade)
			{
				list.Clear();
			}
			foreach (CombatSkillInitialBreakoutData entry in this._brokenOutCombatSkills)
			{
				this._categorizedCombatSkillsByGrade[(int)entry.CombatSkill.Template.Grade].Add(entry.CombatSkill);
			}
			foreach (List<GameData.Domains.CombatSkill.CombatSkill> list2 in this._categorizedCombatSkillsByGrade)
			{
				CollectionUtils.Shuffle<GameData.Domains.CombatSkill.CombatSkill>(context.Random, list2);
				int bonusCount = list2.Count * 80 / 100;
				for (int i = 0; i < bonusCount; i++)
				{
					int index2 = context.Random.Next(list2.Count);
					GameData.Domains.CombatSkill.CombatSkill skill2 = list2[index2];
					SerializableList<SkillBreakPlateBonus> bonuses = this.CreateInitialBreakPlateBonuses(ref breakPlateBonusContext, skill2);
					CollectionUtils.SwapAndRemove<GameData.Domains.CombatSkill.CombatSkill>(list2, index2);
					bool flag4 = bonuses.Items.Count > 0;
					if (flag4)
					{
						this._modifiedBreakPlateBonuses.Add(new ValueTuple<short, int, SerializableList<SkillBreakPlateBonus>>(skill2.GetId().SkillTemplateId, 0, bonuses));
					}
				}
			}
		}

		// Token: 0x0600763C RID: 30268 RVA: 0x004512C8 File Offset: 0x0044F4C8
		private SerializableList<SkillBreakPlateBonus> CreateInitialBreakPlateBonuses(ref Equipping.BreakPlateBonusContext context, GameData.Domains.CombatSkill.CombatSkill combatSkill)
		{
			Character character = context.Character;
			int charId = character.GetId();
			OrganizationItem organizationCfg = this.GetSkillBreakBonusOrganization(character);
			sbyte grade = character.GetOrganizationInfo().Grade;
			int maxBonusCount = this.CalcMaxSkillBreakBonusCount(character, combatSkill);
			SerializableList<SkillBreakPlateBonus> breakBonusList = SerializableList<SkillBreakPlateBonus>.Create();
			CombatSkillItem skillCfg = combatSkill.Template;
			this._skillBreakBonusWeights.Clear();
			this._skillBreakBonusWeights.AddRange(organizationCfg.SkillBreakBonusWeights);
			bool flag = this._skillBreakBonusWeights.Count == 0;
			if (flag)
			{
				foreach (SkillBreakBonusEffectItem effectCfg in ((IEnumerable<SkillBreakBonusEffectItem>)SkillBreakBonusEffect.Instance))
				{
					this._skillBreakBonusWeights.Add(new ShortPair((short)effectCfg.TemplateId, 1));
				}
			}
			while (breakBonusList.Items.Count < maxBonusCount)
			{
				int index = Equipping.GetRandomSkillBreakBonusIndex(context.Random, this._skillBreakBonusWeights);
				sbyte bonusEffectId = (sbyte)this._skillBreakBonusWeights[index].First;
				bool flag2 = bonusEffectId < 0;
				if (flag2)
				{
					break;
				}
				bool flag3 = !skillCfg.MatchBreakPlateBonusEffect(SkillBreakBonusEffect.Instance[bonusEffectId]);
				if (flag3)
				{
					CollectionUtils.SwapAndRemove<ShortPair>(this._skillBreakBonusWeights, index);
				}
				else
				{
					sbyte b = bonusEffectId;
					sbyte b2 = b;
					if (b2 - 33 > 1)
					{
						if (b2 != 37)
						{
							TemplateKey groupTemplateKey = ItemDomain.GetRandomItemGroupIdByEffect(context.Random, bonusEffectId);
							bool flag4 = groupTemplateKey.ItemType < 0;
							if (!flag4)
							{
								short templateId = ItemTemplateHelper.GetTemplateIdInGroup(groupTemplateKey.ItemType, groupTemplateKey.TemplateId, grade);
								bool flag5 = templateId < 0;
								if (!flag5)
								{
									breakBonusList.Items.Add(SkillBreakPlateBonusHelper.CreateItem(new ItemKey(groupTemplateKey.ItemType, 0, templateId, -1)));
								}
							}
						}
						else
						{
							int expLevel = this.GradeToExpLevel(grade);
							breakBonusList.Items.Add(SkillBreakPlateBonusHelper.CreateExp(expLevel));
						}
					}
					else
					{
						ushort relationType = (bonusEffectId == 33) ? 16384 : 32768;
						int selectedRelatedCharId = this.SelectRelatedCharForSkillBreakBonus(ref context, charId, relationType);
						bool flag6 = selectedRelatedCharId < 0;
						if (!flag6)
						{
							breakBonusList.Items.Add(SkillBreakPlateBonusHelper.CreateRelation(charId, selectedRelatedCharId, relationType));
						}
					}
				}
			}
			return breakBonusList;
		}

		// Token: 0x0600763D RID: 30269 RVA: 0x0045151C File Offset: 0x0044F71C
		[return: TupleElementNames(new string[]
		{
			"neiliProportionOfFiveElements",
			"extraNeiliAllocationProgress"
		})]
		private ValueTuple<NeiliProportionOfFiveElements, int[]> PerformInitialNeigongLooping(DataContext context, Character character)
		{
			IRandomSource random = context.Random;
			NeiliProportionOfFiveElements neiliProportion = character.GetBaseNeiliProportionOfFiveElements();
			int[] extraNeiliAllocationProgress = new int[4];
			CharacterItem characterTemplate = character.Template;
			sbyte[] configExtraNeiliAllocation = characterTemplate.ExtraNeiliAllocationProgress;
			for (int i = 0; i < 4; i++)
			{
				int progress = CombatSkillDomain.GetExtraNeiliAllocationProgressByExtraNeiliAllocation((int)configExtraNeiliAllocation[i]);
				extraNeiliAllocationProgress[i] = progress;
			}
			int maxExtraNeiliAllocationProgress = CombatSkillDomain.GetNeiliAllocationMaxProgress();
			bool flag = this._brokenOutNeigongList.Count <= 0;
			ValueTuple<NeiliProportionOfFiveElements, int[]> result;
			if (flag)
			{
				result = new ValueTuple<NeiliProportionOfFiveElements, int[]>(neiliProportion, extraNeiliAllocationProgress);
			}
			else
			{
				int totalLoopsCount = Equipping.GenerateInitialNeigongLoopsCount(character);
				bool flag2 = totalLoopsCount <= 0;
				if (flag2)
				{
					result = new ValueTuple<NeiliProportionOfFiveElements, int[]>(neiliProportion, extraNeiliAllocationProgress);
				}
				else
				{
					this._brokenOutNeigongList.Sort(Equipping.Comparer);
					bool cannotGetExtraNeiliAllocationProgress = CreatingType.IsNonEvolutionaryType(character.GetCreatingType());
					int j = 0;
					int count = this._brokenOutNeigongList.Count;
					while (j < count && totalLoopsCount > 0)
					{
						ValueTuple<CombatSkillItem, int> valueTuple = this._brokenOutNeigongList[j];
						CombatSkillItem skillCfg = valueTuple.Item1;
						int index = valueTuple.Item2;
						GameData.Domains.CombatSkill.CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(character.GetId(), skillCfg.TemplateId));
						sbyte fiveElementsChange = skill.GetFiveElementsChange();
						ValueTuple<short, int, int[]> valueTuple2 = Equipping.GenerateInitialNeili(random, character, skillCfg, ref totalLoopsCount);
						short obtainedNeili = valueTuple2.Item1;
						int loopCount = valueTuple2.Item2;
						int[] extraNeiliAllocationProgress2 = valueTuple2.Item3;
						CombatSkillInitialBreakoutData breakoutData = this._brokenOutCombatSkills[index];
						breakoutData.ObtainedNeili = obtainedNeili;
						for (int k = 0; k < 4; k++)
						{
							bool flag3 = cannotGetExtraNeiliAllocationProgress;
							if (!flag3)
							{
								bool flag4 = extraNeiliAllocationProgress[k] >= maxExtraNeiliAllocationProgress;
								if (!flag4)
								{
									extraNeiliAllocationProgress[k] = extraNeiliAllocationProgress2[k];
								}
							}
						}
						this._brokenOutCombatSkills[index] = breakoutData;
						bool flag5 = fiveElementsChange > 0 && skillCfg.TransferTypeWhileLooping >= 0;
						if (flag5)
						{
							neiliProportion.Transfer(skillCfg.DestTypeWhileLooping, skillCfg.TransferTypeWhileLooping, (int)fiveElementsChange * loopCount);
						}
						j++;
					}
					bool flag6 = totalLoopsCount > 0;
					if (flag6)
					{
						CombatSkillItem skillCfg2 = this.SelectCombatSkillForAdjustingNeiliType(character, this._brokenOutNeigongList);
						bool flag7 = skillCfg2 != null;
						if (flag7)
						{
							GameData.Domains.CombatSkill.CombatSkill skill2 = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(character.GetId(), skillCfg2.TemplateId));
							sbyte fiveElementsChange2 = skill2.GetFiveElementsChange();
							bool flag8 = fiveElementsChange2 > 0 && skillCfg2.TransferTypeWhileLooping >= 0;
							if (flag8)
							{
								neiliProportion.Transfer(skillCfg2.DestTypeWhileLooping, skillCfg2.TransferTypeWhileLooping, (int)fiveElementsChange2 * totalLoopsCount);
							}
						}
						List<ValueTuple<CombatSkillItem, int>> brokenOutNeigongList = this._brokenOutNeigongList;
						bool flag9 = brokenOutNeigongList != null && brokenOutNeigongList.Count > 0;
						if (flag9)
						{
							CombatSkillItem skillCfgForProgress = this._brokenOutNeigongList[0].Item1;
							for (int l = 0; l < totalLoopsCount; l++)
							{
								int[] extraNeiliAllocationProgress3 = Equipping.GenerateInitialNeili(random, character, skillCfgForProgress, ref totalLoopsCount).Item3;
								for (int m = 0; m < 4; m++)
								{
									bool flag10 = cannotGetExtraNeiliAllocationProgress;
									if (!flag10)
									{
										bool flag11 = extraNeiliAllocationProgress[m] >= maxExtraNeiliAllocationProgress;
										if (!flag11)
										{
											extraNeiliAllocationProgress[m] += extraNeiliAllocationProgress3[m];
										}
									}
								}
							}
						}
					}
					result = new ValueTuple<NeiliProportionOfFiveElements, int[]>(neiliProportion, extraNeiliAllocationProgress);
				}
			}
			return result;
		}

		// Token: 0x0600763E RID: 30270 RVA: 0x00451858 File Offset: 0x0044FA58
		[return: TupleElementNames(new string[]
		{
			"activationState",
			"availableStepsCount",
			"forcedStepsCount"
		})]
		private static ValueTuple<ushort, sbyte, sbyte> CalcCombatSkillBreakoutResult(ref Equipping.BreakoutCombatSkillContext context, GameData.Domains.CombatSkill.CombatSkill skill)
		{
			short skillTemplateId = skill.GetId().SkillTemplateId;
			ushort readingState = skill.GetReadingState();
			IRandomSource random = context.Random;
			ushort activationState = CombatSkillStateHelper.GenerateRandomActivatedNormalPages(random, readingState, 0);
			sbyte availableStepsCount = context.Character.GetSkillBreakoutAvailableStepsCount(skillTemplateId);
			int forcedStepsCount = (int)(20 - availableStepsCount);
			bool flag = forcedStepsCount < 0;
			if (flag)
			{
				forcedStepsCount = 0;
			}
			int successRate = context.IsCreatedWithFixedTemplate ? 100 : CombatSkillHelper.CalcBreakoutSuccessRate(skillTemplateId, ref context.Qualifications);
			bool flag2 = random.CheckPercentProb(successRate);
			ValueTuple<ushort, sbyte, sbyte> result;
			if (flag2)
			{
				activationState = CombatSkillStateHelper.GenerateRandomActivatedOutlinePage(random, readingState, activationState, context.BehaviorType);
				result = new ValueTuple<ushort, sbyte, sbyte>(activationState, availableStepsCount, (sbyte)forcedStepsCount);
			}
			else
			{
				result = new ValueTuple<ushort, sbyte, sbyte>(0, availableStepsCount, (sbyte)forcedStepsCount);
			}
			return result;
		}

		// Token: 0x0600763F RID: 30271 RVA: 0x00451904 File Offset: 0x0044FB04
		private static int GenerateInitialNeigongLoopsCount(Character character)
		{
			int baseCount = (int)((character.GetActualAge() - 10) * 12);
			ref CombatSkillShorts combatSkillQualifications = ref character.GetCombatSkillQualifications();
			int interestPercent = (int)(60 + combatSkillQualifications.Items.FixedElementField);
			return baseCount * interestPercent / 100;
		}

		// Token: 0x06007640 RID: 30272 RVA: 0x00451940 File Offset: 0x0044FB40
		[return: TupleElementNames(new string[]
		{
			"ObtainedNeili",
			"loopCount",
			"extraNeiliAllocationProgress"
		})]
		private static ValueTuple<short, int, int[]> GenerateInitialNeili(IRandomSource random, Character character, CombatSkillItem skillCfg, ref int totalLoopsCount)
		{
			GameData.Domains.CombatSkill.CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(character.GetId(), skillCfg.TemplateId));
			short totalNeili = skill.GetTotalObtainableNeili();
			int obtainedNeili = 0;
			int currLoopCount = 0;
			int[] extraNeiliAllocationProgress = new int[4];
			while (obtainedNeili < (int)totalNeili & totalLoopsCount > 0)
			{
				ValueTuple<short, short, int[]> valueTuple = CombatSkillDomain.CalcNeigongLoopingEffect(random, character, skillCfg, true);
				short neili = valueTuple.Item1;
				int[] extraNeiliAllocationProgress2 = valueTuple.Item3;
				obtainedNeili += (int)neili;
				for (int i = 0; i < 4; i++)
				{
					extraNeiliAllocationProgress[i] += extraNeiliAllocationProgress2[i];
				}
				currLoopCount++;
				totalLoopsCount--;
			}
			bool flag = obtainedNeili > (int)totalNeili;
			if (flag)
			{
				obtainedNeili = (int)totalNeili;
			}
			return new ValueTuple<short, int, int[]>((short)obtainedNeili, currLoopCount, extraNeiliAllocationProgress);
		}

		// Token: 0x06007641 RID: 30273 RVA: 0x00451A00 File Offset: 0x0044FC00
		public void ParallelPracticeAndBreakoutCombatSkills(DataContext context, Character character)
		{
			this._canUpdateCombatSkills.Clear();
			this._brokenOutCombatSkills.Clear();
			this._failedToBreakoutCombatSkills.Clear();
			this._newPersonalNeeds.Clear();
			this._askingForHelpSkills.Clear();
			this._newlyActivatedCombatSkills.Clear();
			Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(character.GetId());
			Personalities personalities = character.GetPersonalities();
			sbyte neiliType = character.GetNeiliType();
			sbyte selfOrgTemplateId = character.GetOrganizationInfo().OrgTemplateId;
			sbyte lovingOrgTemplateId = character.GetIdealSect();
			foreach (PersonalNeed personalNeed in character.GetPersonalNeeds())
			{
				bool flag = personalNeed.TemplateId == 18;
				if (flag)
				{
					this._askingForHelpSkills.Add((int)personalNeed.CombatSkillTemplateId);
				}
			}
			foreach (KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> keyValuePair in charCombatSkills)
			{
				short num;
				GameData.Domains.CombatSkill.CombatSkill combatSkill3;
				keyValuePair.Deconstruct(out num, out combatSkill3);
				short skillTemplateId = num;
				GameData.Domains.CombatSkill.CombatSkill combatSkill = combatSkill3;
				ushort activationStates = combatSkill.GetActivationState();
				bool flag2 = CombatSkillStateHelper.IsBrokenOut(activationStates);
				if (flag2)
				{
					ushort readingState = combatSkill.GetReadingState();
					ushort newActivationState = CombatSkillStateHelper.GenerateRandomActivatedNormalPages(context.Random, readingState, activationStates);
					bool flag3 = newActivationState != activationStates;
					if (flag3)
					{
						this._newlyActivatedCombatSkills.Add(new ValueTuple<GameData.Domains.CombatSkill.CombatSkill, ushort>(combatSkill, newActivationState));
					}
				}
				else
				{
					bool flag4 = !combatSkill.CanBreakout();
					if (!flag4)
					{
						bool flag5 = this._askingForHelpSkills.Contains((int)skillTemplateId);
						if (!flag5)
						{
							int score = Equipping.CalcCombatSkillPracticeOrBreakoutScore(skillTemplateId, combatSkill, (short)selfOrgTemplateId, (short)lovingOrgTemplateId, ref personalities, neiliType);
							this._canUpdateCombatSkills.Add(new ValueTuple<GameData.Domains.CombatSkill.CombatSkill, int>(combatSkill, score));
						}
					}
				}
			}
			bool flag6 = this._newlyActivatedCombatSkills.Count >= 0;
			if (flag6)
			{
				ParallelModificationsRecorder recorder = context.ParallelModificationsRecorder;
				recorder.RecordType(ParallelModificationType.ActivateCombatSkillPages);
				recorder.RecordParameterClass<List<ValueTuple<GameData.Domains.CombatSkill.CombatSkill, ushort>>>(this._newlyActivatedCombatSkills);
				this._newlyActivatedCombatSkills = new List<ValueTuple<GameData.Domains.CombatSkill.CombatSkill, ushort>>();
			}
			bool flag7 = this._canUpdateCombatSkills.Count == 0;
			if (!flag7)
			{
				this._canUpdateCombatSkills.Sort(new Comparison<ValueTuple<GameData.Domains.CombatSkill.CombatSkill, int>>(this.CompareScore));
				Equipping.BreakoutCombatSkillContext breakoutContext = new Equipping.BreakoutCombatSkillContext(context.Random, character);
				for (int i = this._canUpdateCombatSkills.Count - 1; i >= 0; i--)
				{
					GameData.Domains.CombatSkill.CombatSkill combatSkill2 = this._canUpdateCombatSkills[i].Item1;
					bool flag8 = this.OfflineBreakoutCombatSkill(ref breakoutContext, combatSkill2);
					if (flag8)
					{
						break;
					}
				}
				PracticeAndBreakoutModification mod = new PracticeAndBreakoutModification(character);
				bool flag9 = this._brokenOutCombatSkills.Count != 0;
				if (flag9)
				{
					mod.BrokenOutCombatSkills = this._brokenOutCombatSkills;
					this._brokenOutCombatSkills = new List<CombatSkillInitialBreakoutData>(32);
				}
				bool flag10 = this._failedToBreakoutCombatSkills.Count != 0;
				if (flag10)
				{
					mod.FailedToBreakoutCombatSkills = this._failedToBreakoutCombatSkills;
					this._failedToBreakoutCombatSkills = new List<GameData.Domains.CombatSkill.CombatSkill>(8);
				}
				bool flag11 = this._newPersonalNeeds.Count != 0;
				if (flag11)
				{
					foreach (PersonalNeed need in this._newPersonalNeeds)
					{
						character.OfflineAddPersonalNeed(need);
					}
					mod.PersonalNeedsChanged = true;
				}
				mod.NewExp = breakoutContext.CharExp;
				mod.NewInjuries = breakoutContext.Injuries;
				mod.NewDisorderOfQi = breakoutContext.DisorderOfQi;
				ParallelModificationsRecorder recorder2 = context.ParallelModificationsRecorder;
				recorder2.RecordType(ParallelModificationType.PracticeAndBreakoutCombatSkills);
				recorder2.RecordParameterClass<PracticeAndBreakoutModification>(mod);
			}
		}

		// Token: 0x06007642 RID: 30274 RVA: 0x00451DD0 File Offset: 0x0044FFD0
		public static void ComplementActivateCombatSkillPages(DataContext context, [TupleElementNames(new string[]
		{
			"skill",
			"activationStates"
		})] List<ValueTuple<GameData.Domains.CombatSkill.CombatSkill, ushort>> newlyActivatedCombatSkills)
		{
			foreach (ValueTuple<GameData.Domains.CombatSkill.CombatSkill, ushort> pair in newlyActivatedCombatSkills)
			{
				pair.Item1.SetActivationState(pair.Item2, context);
			}
		}

		// Token: 0x06007643 RID: 30275 RVA: 0x00451E30 File Offset: 0x00450030
		public static void ComplementPracticeAndBreakoutCombatSkill(DataContext context, PracticeAndBreakoutModification mod)
		{
			Character character = mod.Character;
			character.SetExp(Math.Max(mod.NewExp, 0), context);
			character.SetInjuries(mod.NewInjuries, context);
			character.SetDisorderOfQi(Math.Clamp(mod.NewDisorderOfQi, DisorderLevelOfQi.MinValue, DisorderLevelOfQi.MaxValue), context);
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int charId = character.GetId();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = character.GetLocation();
			bool flag = mod.BrokenOutCombatSkills != null;
			if (flag)
			{
				bool equipChanged = false;
				foreach (CombatSkillInitialBreakoutData brokenOutCombatSkill in mod.BrokenOutCombatSkills)
				{
					GameData.Domains.CombatSkill.CombatSkill skill = brokenOutCombatSkill.CombatSkill;
					skill.SetActivationState(brokenOutCombatSkill.ActivationState, context);
					skill.SetForcedBreakoutStepsCount(brokenOutCombatSkill.ForceBreakoutStepsCount, context);
					skill.SetBreakoutStepsCount(brokenOutCombatSkill.BreakoutStepsCount, context);
					bool flag2 = brokenOutCombatSkill.ObtainedNeili != 0;
					if (flag2)
					{
						skill.SetObtainedNeili(brokenOutCombatSkill.ObtainedNeili, context);
					}
					CombatSkillItem skillConfig = Config.CombatSkill.Instance[skill.GetId().SkillTemplateId];
					bool flag3 = character.IsCombatSkillEquipped(skillConfig.TemplateId);
					if (flag3)
					{
						equipChanged = true;
					}
					short bookId = skillConfig.BookId;
					bool flag4 = bookId >= 0;
					if (flag4)
					{
						character.ChangeHappiness(context, (int)ItemTemplateHelper.GetBaseHappinessChange(10, bookId));
					}
					else
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Character ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
						defaultInterpolatedStringHandler.AppendLiteral(" is breaking out combat skill ");
						defaultInterpolatedStringHandler.AppendFormatted(skillConfig.Name);
						AdaptableLog.Warning(defaultInterpolatedStringHandler.ToStringAndClear(), false);
					}
				}
				bool flag5 = equipChanged;
				if (flag5)
				{
					DomainManager.SpecialEffect.UpdateEquippedSkillEffect(context, character);
				}
			}
			bool flag6 = mod.FailedToBreakoutCombatSkills != null;
			if (flag6)
			{
				foreach (GameData.Domains.CombatSkill.CombatSkill combatSkill in mod.FailedToBreakoutCombatSkills)
				{
					short templateId = combatSkill.GetId().SkillTemplateId;
					SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
					int secretInfoOffset = secretInformationCollection.AddBreakoutFail(charId, templateId);
					DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
				}
			}
			bool personalNeedsChanged = mod.PersonalNeedsChanged;
			if (personalNeedsChanged)
			{
				character.SetPersonalNeeds(character.GetPersonalNeeds(), context);
			}
		}

		// Token: 0x06007644 RID: 30276 RVA: 0x004520D4 File Offset: 0x004502D4
		private int CompareScore([TupleElementNames(new string[]
		{
			"combatSkill",
			"score"
		})] ValueTuple<GameData.Domains.CombatSkill.CombatSkill, int> x, [TupleElementNames(new string[]
		{
			"combatSkill",
			"score"
		})] ValueTuple<GameData.Domains.CombatSkill.CombatSkill, int> y)
		{
			return x.Item2 - y.Item2;
		}

		// Token: 0x06007645 RID: 30277 RVA: 0x004520E4 File Offset: 0x004502E4
		private bool OfflineBreakoutCombatSkill(ref Equipping.BreakoutCombatSkillContext context, GameData.Domains.CombatSkill.CombatSkill combatSkill)
		{
			CombatSkillItem combatSkillCfg = Config.CombatSkill.Instance[combatSkill.GetId().SkillTemplateId];
			int breakoutExpCost = (int)(Config.SkillBreakPlate.Instance[combatSkillCfg.Grade].CostExp * 10);
			bool flag = context.ExpPerMonth + context.CharExp >= breakoutExpCost;
			bool result;
			if (flag)
			{
				context.ExpPerMonth -= breakoutExpCost;
				bool flag2 = context.ExpPerMonth < 0;
				if (flag2)
				{
					context.CharExp += context.ExpPerMonth;
					context.ExpPerMonth = 0;
				}
				ValueTuple<ushort, sbyte, sbyte> valueTuple = Equipping.CalcCombatSkillBreakoutResult(ref context, combatSkill);
				ushort newActivationState = valueTuple.Item1;
				sbyte availableStepsCount = valueTuple.Item2;
				sbyte forcedStepsCount = valueTuple.Item3;
				bool flag3 = !context.IsCreatedWithFixedTemplate && DomainManager.SpecialEffect.ModifyData(context.Character.GetId(), -1, 267, false, -1, -1, -1);
				if (flag3)
				{
					int successRate = CombatSkillHelper.CalcBreakoutSuccessRate(combatSkill.GetId().SkillTemplateId, ref context.Qualifications);
					int extraDamageCount = (130 - successRate) / 10;
					for (int i = 0; i < extraDamageCount; i++)
					{
						CombatSkillHelper.CalcForceBreakoutInjuriesAndDisorderOfQi(context.Random, combatSkillCfg, ref context.Injuries, ref context.DisorderOfQi);
					}
				}
				bool flag4 = newActivationState == 0 && availableStepsCount < 10;
				if (flag4)
				{
					PersonalNeed need = PersonalNeed.CreatePersonalNeed(18, combatSkillCfg.TemplateId);
					this._newPersonalNeeds.Add(need);
					result = false;
				}
				else
				{
					bool flag5 = CombatSkillStateHelper.IsBrokenOut(newActivationState);
					if (flag5)
					{
						sbyte breakoutStepsCount = availableStepsCount + forcedStepsCount;
						this._brokenOutCombatSkills.Add(new CombatSkillInitialBreakoutData(combatSkill, newActivationState, breakoutStepsCount, forcedStepsCount));
						result = false;
					}
					else
					{
						PersonalNeed need2 = PersonalNeed.CreatePersonalNeed(18, combatSkillCfg.TemplateId);
						this._newPersonalNeeds.Add(need2);
						this._failedToBreakoutCombatSkills.Add(combatSkill);
						CombatSkillHelper.CalcForceBreakoutInjuriesAndDisorderOfQi(context.Random, combatSkillCfg, ref context.Injuries, ref context.DisorderOfQi);
						result = true;
					}
				}
			}
			else
			{
				PersonalNeed need3 = PersonalNeed.CreatePersonalNeed(16, breakoutExpCost);
				this._newPersonalNeeds.Add(need3);
				result = true;
			}
			return result;
		}

		// Token: 0x06007646 RID: 30278 RVA: 0x004522E8 File Offset: 0x004504E8
		public void ParallelUpdateBreakPlateBonuses(DataContext context, Character character)
		{
			this._canUpdateCombatSkills.Clear();
			this._newPersonalNeeds.Clear();
			this._modifiedBreakPlateBonuses.Clear();
			this._consumedItems.Clear();
			Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(character.GetId());
			Personalities personalities = character.GetPersonalities();
			sbyte neiliType = character.GetNeiliType();
			sbyte selfOrgTemplateId = character.GetOrganizationInfo().OrgTemplateId;
			sbyte lovingOrgTemplateId = character.GetIdealSect();
			foreach (KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> keyValuePair in charCombatSkills)
			{
				short num;
				GameData.Domains.CombatSkill.CombatSkill combatSkill2;
				keyValuePair.Deconstruct(out num, out combatSkill2);
				short skillTemplateId = num;
				GameData.Domains.CombatSkill.CombatSkill combatSkill = combatSkill2;
				ushort activationStates = combatSkill.GetActivationState();
				bool flag = combatSkill.GetRevoked() || !CombatSkillStateHelper.IsBrokenOut(activationStates);
				if (!flag)
				{
					int score = Equipping.CalcCombatSkillPracticeOrBreakoutScore(skillTemplateId, combatSkill, (short)selfOrgTemplateId, (short)lovingOrgTemplateId, ref personalities, neiliType);
					this._canUpdateCombatSkills.Add(new ValueTuple<GameData.Domains.CombatSkill.CombatSkill, int>(combatSkill, score));
				}
			}
			int expCost = 0;
			bool flag2 = this._canUpdateCombatSkills.Count > 0;
			if (flag2)
			{
				Equipping.BreakPlateBonusContext breakPlateBonusContext = new Equipping.BreakPlateBonusContext(context.Random, character);
				this._canUpdateCombatSkills.Sort(new Comparison<ValueTuple<GameData.Domains.CombatSkill.CombatSkill, int>>(this.CompareScore));
				for (int i = this._canUpdateCombatSkills.Count - 1; i >= 0; i--)
				{
					expCost += this.OfflineUpdateBonuses(ref breakPlateBonusContext, this._canUpdateCombatSkills[i].Item1);
				}
			}
			bool flag3 = this._modifiedBreakPlateBonuses.Count > 0 || this._newPersonalNeeds.Count > 0;
			if (flag3)
			{
				UpdateBreakPlateBonusesModification mod = new UpdateBreakPlateBonusesModification(character);
				ParallelModificationsRecorder recorder = context.ParallelModificationsRecorder;
				recorder.RecordType(ParallelModificationType.UpdateBreakPlateBonuses);
				recorder.RecordParameterClass<UpdateBreakPlateBonusesModification>(mod);
				bool flag4 = this._modifiedBreakPlateBonuses.Count > 0;
				if (flag4)
				{
					mod.ModifiedBonuses = this._modifiedBreakPlateBonuses;
					this._modifiedBreakPlateBonuses = new List<ValueTuple<short, int, SerializableList<SkillBreakPlateBonus>>>();
				}
				mod.ExpCost = expCost;
				bool flag5 = this._consumedItems.Count > 0;
				if (flag5)
				{
					mod.ToDeleteItems = this._consumedItems;
					this._consumedItems = new List<ItemKey>();
				}
				bool flag6 = this._newPersonalNeeds.Count > 0;
				if (flag6)
				{
					foreach (PersonalNeed need in this._newPersonalNeeds)
					{
						character.OfflineAddPersonalNeed(need);
					}
					mod.PersonalNeedsUpdated = true;
				}
			}
		}

		// Token: 0x06007647 RID: 30279 RVA: 0x00452598 File Offset: 0x00450798
		public static void ComplementUpdateBreakPlateBonuses(DataContext context, UpdateBreakPlateBonusesModification mod)
		{
			Character character = mod.Character;
			int charId = character.GetId();
			bool flag = mod.ModifiedBonuses != null;
			if (flag)
			{
				Equipping.ApplyBreakPlateBonuses(context, charId, mod.ModifiedBonuses);
				Equipping.AddBreakPlateBonusLifeRecords(charId, mod.ModifiedBonuses);
			}
			bool personalNeedsUpdated = mod.PersonalNeedsUpdated;
			if (personalNeedsUpdated)
			{
				character.SetPersonalNeeds(character.GetPersonalNeeds(), context);
			}
			List<ItemKey> toDeleteItems = mod.ToDeleteItems;
			bool flag2 = toDeleteItems != null && toDeleteItems.Count > 0;
			if (flag2)
			{
				foreach (ItemKey itemKey in mod.ToDeleteItems)
				{
					Events.RaiseItemRemovedFromInventory(context, character, itemKey, 1);
					DomainManager.Item.RemoveItem(context, itemKey);
				}
				character.SetInventory(character.GetInventory(), context);
			}
			bool flag3 = mod.ExpCost > 0;
			if (flag3)
			{
				character.ChangeExp(context, -mod.ExpCost);
			}
		}

		// Token: 0x06007648 RID: 30280 RVA: 0x004526A4 File Offset: 0x004508A4
		private static void ApplyBreakPlateBonuses(DataContext context, int charId, [TupleElementNames(new string[]
		{
			"skillTemplateId",
			"startIndex",
			"bonuses"
		})] List<ValueTuple<short, int, SerializableList<SkillBreakPlateBonus>>> modifiedBonuses)
		{
			foreach (ValueTuple<short, int, SerializableList<SkillBreakPlateBonus>> element in modifiedBonuses)
			{
				DomainManager.Extra.SetCharacterSkillBreakBonuses(context, charId, element.Item1, element.Item3);
			}
		}

		// Token: 0x06007649 RID: 30281 RVA: 0x0045270C File Offset: 0x0045090C
		private static void AddBreakPlateBonusLifeRecords(int charId, [TupleElementNames(new string[]
		{
			"skillTemplateId",
			"startIndex",
			"bonuses"
		})] List<ValueTuple<short, int, SerializableList<SkillBreakPlateBonus>>> modifiedBonuses)
		{
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			foreach (ValueTuple<short, int, SerializableList<SkillBreakPlateBonus>> element in modifiedBonuses)
			{
				for (int i = element.Item2; i < element.Item3.Items.Count; i++)
				{
					SkillBreakPlateBonus bonus = element.Item3.Items[i];
					switch (bonus.Type)
					{
					case ESkillBreakPlateBonusType.Item:
						lifeRecordCollection.AddCombatSkillKeyPointComprehensionByItems(charId, currDate, element.Item1, bonus.ItemType, bonus.ItemTemplateId);
						break;
					case ESkillBreakPlateBonusType.Relation:
					{
						bool flag = bonus.RelationType == 16384;
						if (flag)
						{
							lifeRecordCollection.AddCombatSkillKeyPointComprehensionByLoveRelationship(charId, currDate, element.Item1, bonus.RelationRelatedCharId);
						}
						else
						{
							lifeRecordCollection.AddCombatSkillKeyPointComprehensionByHatredRelationship(charId, currDate, element.Item1, bonus.RelationRelatedCharId);
						}
						break;
					}
					case ESkillBreakPlateBonusType.Exp:
						lifeRecordCollection.AddCombatSkillKeyPointComprehensionByExp(charId, currDate, element.Item1);
						break;
					}
				}
			}
		}

		// Token: 0x0600764A RID: 30282 RVA: 0x00452850 File Offset: 0x00450A50
		private int OfflineUpdateBonuses(ref Equipping.BreakPlateBonusContext context, GameData.Domains.CombatSkill.CombatSkill combatSkill)
		{
			Character character = context.Character;
			short skillTemplateId = combatSkill.GetId().SkillTemplateId;
			OrganizationItem organizationCfg = this.GetSkillBreakBonusOrganization(character);
			int maxBonusCount = this.CalcMaxSkillBreakBonusCount(character, combatSkill);
			SerializableList<SkillBreakPlateBonus> breakBonuses = DomainManager.Extra.GetCharacterSkillBreakBonuses(character.GetId(), skillTemplateId);
			List<SkillBreakPlateBonus> items = breakBonuses.Items;
			int bonusCount = (items != null) ? items.Count : 0;
			bool flag = bonusCount >= maxBonusCount;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool modified = false;
				int expCost = 0;
				for (int i = bonusCount; i < maxBonusCount; i++)
				{
					SkillBreakPlateBonus bonus = this.SelectSkillBreakBonus(ref context, combatSkill.Template, organizationCfg);
					bool flag2 = bonus.Type == ESkillBreakPlateBonusType.None;
					if (flag2)
					{
						int index = Equipping.GetRandomSkillBreakBonusIndex(context.Random, organizationCfg.SkillBreakBonusWeights);
						bool flag3 = index < 0;
						if (!flag3)
						{
							sbyte needBonusEffectId = (sbyte)organizationCfg.SkillBreakBonusWeights[index].First;
							PersonalNeed personalNeed = this.CreateSkillBreakPlateBonusNeed(context.Random, character, needBonusEffectId);
							this._newPersonalNeeds.Add(personalNeed);
							break;
						}
					}
					else
					{
						bool flag4 = bonus.Type == ESkillBreakPlateBonusType.Exp;
						if (flag4)
						{
							expCost += SkillBreakPlateConstants.ExpLevelValues[bonus.ExpLevel];
						}
						ref List<SkillBreakPlateBonus> ptr = ref breakBonuses.Items;
						if (ptr == null)
						{
							ptr = new List<SkillBreakPlateBonus>();
						}
						breakBonuses.Items.Add(bonus);
						modified = true;
					}
				}
				bool flag5 = modified;
				if (flag5)
				{
					this._modifiedBreakPlateBonuses.Add(new ValueTuple<short, int, SerializableList<SkillBreakPlateBonus>>(skillTemplateId, bonusCount, breakBonuses));
				}
				result = expCost;
			}
			return result;
		}

		// Token: 0x0600764B RID: 30283 RVA: 0x004529D4 File Offset: 0x00450BD4
		private int CalcMaxSkillBreakBonusCount(Character character, GameData.Domains.CombatSkill.CombatSkill combatSkill)
		{
			int bonusCount = combatSkill.Template.SkillBreakPlate.BonusCount;
			int talentPercentage = character.GetSkillBreakoutStepsPercentage(combatSkill.GetId().SkillTemplateId);
			return Math.Min(bonusCount * talentPercentage / 100, bonusCount);
		}

		// Token: 0x0600764C RID: 30284 RVA: 0x00452A18 File Offset: 0x00450C18
		private OrganizationItem GetSkillBreakBonusOrganization(Character character)
		{
			OrganizationInfo orgInfo = character.GetOrganizationInfo();
			sbyte orgTemplateId = orgInfo.OrgTemplateId;
			OrganizationItem organizationCfg = Organization.Instance[orgTemplateId];
			bool isSect = organizationCfg.IsSect;
			OrganizationItem result;
			if (isSect)
			{
				result = organizationCfg;
			}
			else
			{
				orgTemplateId = character.GetIdealSect();
				bool flag = orgTemplateId >= 0;
				if (flag)
				{
					result = Organization.Instance[orgTemplateId];
				}
				else
				{
					bool flag2 = orgInfo.SettlementId >= 0;
					if (flag2)
					{
						Settlement settlement = DomainManager.Organization.GetSettlement(orgInfo.SettlementId);
						Location location = settlement.GetLocation();
						bool flag3 = location.IsValid();
						if (flag3)
						{
							result = organizationCfg;
						}
						else
						{
							sbyte stateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(location.AreaId);
							orgTemplateId = MapState.Instance[stateTemplateId].SectID;
							result = Organization.Instance[orgTemplateId];
						}
					}
					else
					{
						result = organizationCfg;
					}
				}
			}
			return result;
		}

		// Token: 0x0600764D RID: 30285 RVA: 0x00452AF0 File Offset: 0x00450CF0
		private SkillBreakPlateBonus SelectSkillBreakBonus(ref Equipping.BreakPlateBonusContext context, CombatSkillItem skillCfg, OrganizationItem organizationCfg)
		{
			foreach (ShortPair shortPair in organizationCfg.SkillBreakBonusWeights)
			{
				sbyte bonusEffectId = (sbyte)shortPair.First;
				SkillBreakBonusEffectItem effectCfg = SkillBreakBonusEffect.Instance[bonusEffectId];
				bool flag = !skillCfg.MatchBreakPlateBonusEffect(effectCfg);
				if (!flag)
				{
					SkillBreakPlateBonus bonus = this.CreateSkillBreakBonus(ref context, bonusEffectId);
					bool flag2 = bonus.Type > ESkillBreakPlateBonusType.None;
					if (flag2)
					{
						return bonus;
					}
				}
			}
			return SkillBreakPlateBonus.Invalid;
		}

		// Token: 0x0600764E RID: 30286 RVA: 0x00452B74 File Offset: 0x00450D74
		private static int GetRandomSkillBreakBonusIndex(IRandomSource random, IReadOnlyList<ShortPair> bonusWeights)
		{
			bool flag = bonusWeights.Count == 0;
			if (!flag)
			{
				int totalWeight = 0;
				foreach (ShortPair weight in bonusWeights)
				{
					totalWeight += (int)weight.Second;
				}
				int randomValue = random.Next(0, totalWeight);
				for (int i = 0; i < bonusWeights.Count; i++)
				{
					randomValue -= (int)bonusWeights[i].Second;
					bool flag2 = randomValue < 0;
					if (flag2)
					{
						return i;
					}
				}
				throw new ArgumentException("Unable to get random from weight table.", "bonusWeights");
			}
			return -1;
		}

		// Token: 0x0600764F RID: 30287 RVA: 0x00452C34 File Offset: 0x00450E34
		private PersonalNeed CreateSkillBreakPlateBonusNeed(IRandomSource random, Character character, sbyte bonusEffectId)
		{
			sbyte charGrade = character.GetInteractionGrade();
			sbyte targetGrade = ItemDomain.GenerateRandomItemGrade(random, charGrade);
			switch (bonusEffectId)
			{
			case 33:
				return PersonalNeed.CreatePersonalNeed(25, 16384);
			case 34:
				return PersonalNeed.CreatePersonalNeed(25, 32768);
			case 37:
				return PersonalNeed.CreatePersonalNeed(16, SkillBreakPlateConstants.ExpLevelValues[this.GradeToExpLevel(targetGrade)]);
			}
			TemplateKey groupTemplateKey = ItemDomain.GetRandomItemGroupIdByEffect(random, bonusEffectId);
			bool flag = groupTemplateKey.ItemType < 0;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unable to create personal need with bonus effect ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(bonusEffectId);
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			short templateId = ItemTemplateHelper.GetTemplateIdInGroup(groupTemplateKey.ItemType, groupTemplateKey.TemplateId, targetGrade);
			return PersonalNeed.CreatePersonalNeed(10, groupTemplateKey.ItemType, templateId);
		}

		// Token: 0x06007650 RID: 30288 RVA: 0x00452D24 File Offset: 0x00450F24
		private int GradeToExpLevel(sbyte grade)
		{
			return Math.Clamp(SkillBreakPlateConstants.ExpLevelValues.Count - 1 - 8 + (int)grade, 0, SkillBreakPlateConstants.ExpLevelValues.Count - 1);
		}

		// Token: 0x06007651 RID: 30289 RVA: 0x00452D48 File Offset: 0x00450F48
		private SkillBreakPlateBonus CreateSkillBreakBonus(ref Equipping.BreakPlateBonusContext context, sbyte bonusEffectId)
		{
			Character character = context.Character;
			if (bonusEffectId - 33 > 1)
			{
				if (bonusEffectId == 37)
				{
					sbyte grade = character.GetInteractionGrade();
					return SkillBreakPlateBonusHelper.CreateExp(this.GradeToExpLevel(grade));
				}
				Inventory inventory = character.GetInventory();
				ItemKey selectedItemKey = ItemKey.Invalid;
				int maxGrade = -1;
				foreach (KeyValuePair<ItemKey, int> keyValuePair in inventory.Items)
				{
					ItemKey itemKey2;
					int num;
					keyValuePair.Deconstruct(out itemKey2, out num);
					ItemKey itemKey = itemKey2;
					sbyte itemBonusEffectId = ItemTemplateHelper.GetBreakBonusEffect(itemKey.ItemType, itemKey.TemplateId);
					bool flag = itemBonusEffectId != bonusEffectId;
					if (!flag)
					{
						sbyte itemGrade = ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId);
						bool flag2 = (int)itemGrade <= maxGrade;
						if (!flag2)
						{
							maxGrade = (int)itemGrade;
							selectedItemKey = itemKey;
						}
					}
				}
				bool flag3 = selectedItemKey.IsValid();
				if (flag3)
				{
					inventory.OfflineRemove(selectedItemKey, 1);
					this._consumedItems.Add(selectedItemKey);
					return SkillBreakPlateBonusHelper.CreateItem(selectedItemKey);
				}
			}
			else
			{
				int charId = character.GetId();
				ushort relationType = (bonusEffectId == 33) ? 16384 : 32768;
				int selectedRelatedCharId = this.SelectRelatedCharForSkillBreakBonus(ref context, charId, relationType);
				bool flag4 = selectedRelatedCharId < 0;
				if (!flag4)
				{
					return SkillBreakPlateBonusHelper.CreateRelation(charId, selectedRelatedCharId, relationType);
				}
			}
			return SkillBreakPlateBonus.Invalid;
		}

		// Token: 0x06007652 RID: 30290 RVA: 0x00452ED0 File Offset: 0x004510D0
		private int SelectRelatedCharForSkillBreakBonus(ref Equipping.BreakPlateBonusContext context, int charId, ushort relationType)
		{
			HashSet<int> relatedCharIds = DomainManager.Character.GetRelatedCharIds(charId, relationType);
			bool flag = relatedCharIds.Count == 0;
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				short currFavor = 0;
				int selectedCharId = -1;
				bool flag2 = context.UsedRelatedCharIds == null;
				if (flag2)
				{
					this.InitializeUsedRelatedCharactersForBreakBonus(ref context);
				}
				foreach (int relatedCharId in relatedCharIds)
				{
					bool flag3 = !DomainManager.Character.IsCharacterAlive(relatedCharId);
					if (!flag3)
					{
						bool flag4 = !DomainManager.Character.HasRelation(relatedCharId, charId, relationType);
						if (!flag4)
						{
							bool flag5 = this._usedRelatedCharIds.Contains(relatedCharId);
							if (!flag5)
							{
								short favor = DomainManager.Character.GetFavorability(charId, relatedCharId);
								bool flag6 = selectedCharId < 0;
								if (flag6)
								{
									selectedCharId = relatedCharId;
									currFavor = favor;
								}
								else
								{
									bool flag7 = relationType == 32768;
									if (flag7)
									{
										bool flag8 = favor >= currFavor;
										if (!flag8)
										{
											currFavor = favor;
											selectedCharId = (int)relationType;
										}
									}
									else
									{
										bool flag9 = favor <= currFavor;
										if (!flag9)
										{
											currFavor = favor;
											selectedCharId = (int)relationType;
										}
									}
								}
							}
						}
					}
				}
				bool flag10 = selectedCharId >= 0;
				if (flag10)
				{
					context.UsedRelatedCharIds.Add(selectedCharId);
				}
				result = selectedCharId;
			}
			return result;
		}

		// Token: 0x06007653 RID: 30291 RVA: 0x0045302C File Offset: 0x0045122C
		private void InitializeUsedRelatedCharactersForBreakBonus(ref Equipping.BreakPlateBonusContext context)
		{
			int charId = context.Character.GetId();
			this._usedRelatedCharIds.Clear();
			foreach (KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> keyValuePair in context.CharCombatSkills)
			{
				short num;
				GameData.Domains.CombatSkill.CombatSkill combatSkill;
				keyValuePair.Deconstruct(out num, out combatSkill);
				short templateId = num;
				GameData.Domains.CombatSkill.CombatSkill skill = combatSkill;
				bool flag = !CombatSkillStateHelper.IsBrokenOut(skill.GetActivationState());
				if (!flag)
				{
					SerializableList<SkillBreakPlateBonus> bonuses = DomainManager.Extra.GetCharacterSkillBreakBonuses(charId, templateId);
					List<SkillBreakPlateBonus> items = bonuses.Items;
					bool flag2 = items == null || items.Count <= 0;
					if (!flag2)
					{
						for (int i = 0; i < bonuses.Items.Count; i++)
						{
							SkillBreakPlateBonus bonus = bonuses.Items[i];
							bool flag3 = bonus.Type != ESkillBreakPlateBonusType.Relation;
							if (!flag3)
							{
								this._usedRelatedCharIds.Add(bonus.RelationRelatedCharId);
							}
						}
					}
				}
			}
			context.UsedRelatedCharIds = this._usedRelatedCharIds;
		}

		// Token: 0x06007654 RID: 30292 RVA: 0x00453164 File Offset: 0x00451364
		private unsafe static int CalcCombatSkillPracticeOrBreakoutScore(short skillTemplateId, GameData.Domains.CombatSkill.CombatSkill skill, short selfOrgTemplateId, short targetOrgTemplateId, ref Personalities personalities, sbyte neiliType)
		{
			int score = 0;
			CombatSkillItem skillCfg = Config.CombatSkill.Instance[skillTemplateId];
			bool flag = (short)skillCfg.SectId == selfOrgTemplateId;
			if (flag)
			{
				score += 150 + (int)personalities.Items.FixedElementField;
			}
			bool flag2 = (short)skillCfg.SectId == targetOrgTemplateId;
			if (flag2)
			{
				score += (int)(75 + *(ref personalities.Items.FixedElementField + 2));
			}
			bool flag3 = !Equipping.CheckCounterWithNeiliType(skillCfg.FiveElements, neiliType);
			if (flag3)
			{
				score += 150 + (int)personalities.Items.FixedElementField;
			}
			return score + (int)(50 * (8 - skillCfg.Grade));
		}

		// Token: 0x06007655 RID: 30293 RVA: 0x00453208 File Offset: 0x00451408
		public void EquipItems(DataContext context, Character character)
		{
			SelectEquipmentsModification mod = new SelectEquipmentsModification(character, false);
			this.EquipItems(character, mod);
			bool flag = mod.EquippedItems != null;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 1);
				defaultInterpolatedStringHandler.AppendFormatted<Character>(character);
				defaultInterpolatedStringHandler.AppendLiteral(" changed equipped items.");
				AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
				character.ChangeEquipment(context, mod.EquippedItems);
			}
		}

		// Token: 0x06007656 RID: 30294 RVA: 0x00453274 File Offset: 0x00451474
		private void EquipItems(Character character, SelectEquipmentsModification mod)
		{
			this.ClassifyAvailableItems(character);
			this.SelectWeapons(character, mod);
			OrganizationMemberItem orgMemberCfg = OrganizationDomain.GetOrgMemberConfig(character.GetOrganizationInfo());
			PresetEquipmentItemWithProb[] orgEquipment = orgMemberCfg.Equipment;
			this._equippedItems[3] = Equipping.SelectArmor(mod, this._availableHelms, orgEquipment[0]);
			this._equippedItems[5] = Equipping.SelectArmor(mod, this._availableTorsos, orgEquipment[1]);
			this._equippedItems[6] = Equipping.SelectArmor(mod, this._availableBracers, orgEquipment[2]);
			this._equippedItems[7] = Equipping.SelectArmor(mod, this._availableBoots, orgEquipment[3]);
			this._equippedItems[8] = Equipping.SelectAccessory(mod, this._availableAccessories, orgEquipment[4]);
			this._equippedItems[9] = Equipping.SelectAccessory(mod, this._availableAccessories, orgEquipment[5]);
			this._equippedItems[10] = Equipping.SelectAccessory(mod, this._availableAccessories, orgEquipment[6]);
			short idealClothingTemplateId = character.GetIdealClothingTemplateId();
			this._equippedItems[4] = this.SelectClothing(character, mod, idealClothingTemplateId);
			this._equippedItems[11] = Equipping.SelectCarrier(mod, this._availableCarriers, orgEquipment[7]);
			bool flag = !CollectionUtils.Equals<ItemKey>(character.GetEquipment(), this._equippedItems, 12);
			if (flag)
			{
				mod.EquippedItems = this._equippedItems;
				this._equippedItems = new ItemKey[12];
			}
		}

		// Token: 0x06007657 RID: 30295 RVA: 0x004533F4 File Offset: 0x004515F4
		public ItemKey SelectClothing(DataContext context, Character character)
		{
			this._availableClothing.Clear();
			ItemKey[] equipment = character.GetEquipment();
			sbyte gender = character.GetGender();
			bool flag = equipment[4].IsValid();
			if (flag)
			{
				EquipmentBase item = DomainManager.Item.GetBaseEquipment(equipment[4]);
				this.AddAvailableItem(item, item.GetEquipmentType(), gender);
			}
			Inventory inventory = character.GetInventory();
			foreach (KeyValuePair<ItemKey, int> keyValuePair in inventory.Items)
			{
				ItemKey itemKey2;
				int num;
				keyValuePair.Deconstruct(out itemKey2, out num);
				ItemKey itemKey = itemKey2;
				EquipmentBase item2 = DomainManager.Item.TryGetBaseEquipment(itemKey);
				bool flag2 = item2 != null && item2.GetEquipmentType() == 2;
				if (flag2)
				{
					this.AddAvailableItem(item2, item2.GetEquipmentType(), gender);
				}
			}
			SelectEquipmentsModification modification = new SelectEquipmentsModification(character, false);
			short idealClothingId = character.GetIdealClothingTemplateId();
			ItemKey selectedClothing = this.SelectClothing(character, modification, idealClothingId);
			bool personalNeedChanged = modification.PersonalNeedChanged;
			if (personalNeedChanged)
			{
				character.SetPersonalNeeds(character.GetPersonalNeeds(), context);
			}
			return selectedClothing;
		}

		// Token: 0x06007658 RID: 30296 RVA: 0x00453524 File Offset: 0x00451724
		private void ClassifyAvailableItems(Character character)
		{
			this._availableWeapons.Clear();
			this._availableHelms.Clear();
			this._availableTorsos.Clear();
			this._availableBracers.Clear();
			this._availableBoots.Clear();
			this._availableAccessories.Clear();
			this._availableClothing.Clear();
			this._availableCarriers.Clear();
			ItemKey[] equipment = character.GetEquipment();
			sbyte gender = character.GetGender();
			for (int i = 0; i < 12; i++)
			{
				ItemKey itemKey = equipment[i];
				bool flag = itemKey.IsValid();
				if (flag)
				{
					EquipmentBase item = DomainManager.Item.GetBaseEquipment(itemKey);
					this.AddAvailableItem(item, item.GetEquipmentType(), gender);
				}
			}
			Inventory inventory = character.GetInventory();
			foreach (KeyValuePair<ItemKey, int> keyValuePair in inventory.Items)
			{
				ItemKey itemKey3;
				int num;
				keyValuePair.Deconstruct(out itemKey3, out num);
				ItemKey itemKey2 = itemKey3;
				EquipmentBase item2 = DomainManager.Item.TryGetBaseEquipment(itemKey2);
				bool flag2 = item2 != null;
				if (flag2)
				{
					this.AddAvailableItem(item2, item2.GetEquipmentType(), gender);
				}
			}
		}

		// Token: 0x06007659 RID: 30297 RVA: 0x00453674 File Offset: 0x00451874
		private void AddAvailableItem(EquipmentBase item, sbyte equipmentType, sbyte gender)
		{
			bool flag = item.GetMaxDurability() > 0 && item.GetCurrDurability() <= 0;
			if (!flag)
			{
				switch (equipmentType)
				{
				case 0:
					this._availableWeapons.Add(new ValueTuple<ItemKey, int>(item.GetItemKey(), 0));
					break;
				case 1:
					this._availableHelms.Add((GameData.Domains.Item.Armor)item);
					break;
				case 2:
				{
					GameData.Domains.Item.Clothing clothing = (GameData.Domains.Item.Clothing)item;
					bool flag2 = clothing.GetAgeGroup() == 2;
					if (flag2)
					{
						this._availableClothing.Add(clothing);
					}
					break;
				}
				case 3:
					this._availableTorsos.Add((GameData.Domains.Item.Armor)item);
					break;
				case 4:
					this._availableBracers.Add((GameData.Domains.Item.Armor)item);
					break;
				case 5:
					this._availableBoots.Add((GameData.Domains.Item.Armor)item);
					break;
				case 6:
					this._availableAccessories.Add((GameData.Domains.Item.Accessory)item);
					break;
				case 7:
					this._availableCarriers.Add((GameData.Domains.Item.Carrier)item);
					break;
				}
			}
		}

		// Token: 0x0600765A RID: 30298 RVA: 0x00453790 File Offset: 0x00451990
		public unsafe static bool GetWeaponScores(Character character, [TupleElementNames(new string[]
		{
			"weapon",
			"score"
		})] List<ValueTuple<ItemKey, int>> availableWeapons, [TupleElementNames(new string[]
		{
			"itemTemplateId",
			"count"
		})] List<ValueTuple<short, short>> suitableWeapons, HashSet<short> fixedBestWeapons)
		{
			suitableWeapons.Clear();
			fixedBestWeapons.Clear();
			IntPtr intPtr = stackalloc byte[(UIntPtr)8];
			initblk(intPtr, 0, 8);
			short* pRequiredHitRates = intPtr;
			IntPtr intPtr2 = stackalloc byte[(UIntPtr)22];
			initblk(intPtr2, 0, 22);
			byte* pRequiredTricks = intPtr2;
			CombatSkillEquipment combatSkillEquipment = character.GetCombatSkillEquipment();
			Equipping.CalcAttackSkillsRequirement(combatSkillEquipment, pRequiredHitRates, pRequiredTricks, suitableWeapons, fixedBestWeapons);
			byte* pClonedRequiredTricks = stackalloc byte[(UIntPtr)22];
			bool hasMatchTricks = false;
			int i = 0;
			int weaponsCount = availableWeapons.Count;
			while (i < weaponsCount)
			{
				ItemKey weapon = availableWeapons[i].Item1;
				int score = Equipping.CalcWeaponScore(weapon, pRequiredTricks, pClonedRequiredTricks, pRequiredHitRates, ref hasMatchTricks, suitableWeapons, fixedBestWeapons);
				availableWeapons[i] = new ValueTuple<ItemKey, int>(weapon, score);
				i++;
			}
			return hasMatchTricks;
		}

		// Token: 0x0600765B RID: 30299 RVA: 0x00453838 File Offset: 0x00451A38
		private unsafe static int CalcWeaponScore(ItemKey itemKey, byte* pRequiredTricks, byte* pClonedRequiredTricks, short* pRequiredHitRates, ref bool hasMatchTricks, [TupleElementNames(new string[]
		{
			"itemTemplateId",
			"count"
		})] List<ValueTuple<short, short>> suitableWeapons, HashSet<short> fixedBestWeapons)
		{
			WeaponItem weaponConfig = Config.Weapon.Instance[itemKey.TemplateId];
			int score = (int)weaponConfig.Grade * 200;
			bool flag = !itemKey.IsValid();
			int result;
			if (flag)
			{
				result = score;
			}
			else
			{
				GameData.Domains.Item.Weapon weapon = DomainManager.Item.GetBaseItem(itemKey) as GameData.Domains.Item.Weapon;
				bool flag2 = ModificationStateHelper.IsActive(1, weapon.GetModificationState());
				if (flag2)
				{
					PoisonsAndLevels attachedPoisons = DomainManager.Item.GetAttachedPoisons(weapon.GetItemKey());
					for (sbyte poisonType = 0; poisonType < 6; poisonType += 1)
					{
						short value = *(ref attachedPoisons.Values.FixedElementField + (IntPtr)poisonType * 2);
						sbyte poisonsLevel = *(ref attachedPoisons.Levels.FixedElementField + poisonType);
						score += (int)(value * (short)poisonsLevel * 2);
					}
				}
				Buffer.MemoryCopy((void*)pRequiredTricks, (void*)pClonedRequiredTricks, 22L, 22L);
				int matchedTricksCount = Equipping.CalcMatchedTricksCount(pClonedRequiredTricks, weapon);
				short maxDurability = weapon.GetMaxDurability();
				bool flag3 = maxDurability > 0;
				if (flag3)
				{
					score = score * (int)weapon.GetCurrDurability() / (int)maxDurability;
				}
				bool flag4 = matchedTricksCount > 0;
				if (flag4)
				{
					score += matchedTricksCount * 10;
					hasMatchTricks = true;
					short baseItemTemplateId = itemKey.TemplateId - (short)weaponConfig.Grade;
					score += Equipping.GetSuitableWeaponCount(baseItemTemplateId, suitableWeapons) * 300;
					bool flag5 = fixedBestWeapons.Contains(itemKey.TemplateId);
					if (flag5)
					{
						score += 900;
					}
					HitOrAvoidShorts hitFactors = weapon.GetHitFactors();
					for (int hitType = 0; hitType < 4; hitType++)
					{
						score += (int)(pRequiredHitRates[hitType] * *(ref hitFactors.Items.FixedElementField + (IntPtr)hitType * 2) / 150);
					}
					score += 65536;
				}
				result = score;
			}
			return result;
		}

		// Token: 0x0600765C RID: 30300 RVA: 0x004539DC File Offset: 0x00451BDC
		private bool MatchCombatSkillByCombatConfig(short combatSkillTemplateId, CombatConfigItem combatConfig)
		{
			CombatSkillItem combatSkillCfg = Config.CombatSkill.Instance[combatSkillTemplateId];
			bool flag = combatSkillCfg.EquipType == 4 || combatSkillCfg.EquipType == 0;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = combatConfig.Sect >= 0 && combatConfig.Sect != combatSkillCfg.SectId;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = combatConfig.CombatSkillType != null && combatConfig.CombatSkillType.Count > 0 && !combatConfig.CombatSkillType.Contains(combatSkillCfg.Type);
					result = !flag3;
				}
			}
			return result;
		}

		// Token: 0x0600765D RID: 30301 RVA: 0x00453A74 File Offset: 0x00451C74
		private void SelectWeapons(Character character, SelectEquipmentsModification mod)
		{
			bool hasMatchedTricks = Equipping.GetWeaponScores(character, this._availableWeapons, this._suitableWeapons, this._fixedBestWeapons);
			bool flag = !hasMatchedTricks;
			if (flag)
			{
				foreach (short weaponId in this._fixedBestWeapons)
				{
					PersonalNeed personalNeed = PersonalNeed.CreatePersonalNeed(10, 0, weaponId);
					character.OfflineAddPersonalNeed(personalNeed);
					mod.PersonalNeedChanged = true;
				}
				foreach (ValueTuple<short, short> valueTuple in this._suitableWeapons)
				{
					short suitableWeaponId = valueTuple.Item1;
					PersonalNeed personalNeed2 = PersonalNeed.CreatePersonalNeed(10, 0, suitableWeaponId);
					character.OfflineAddPersonalNeed(personalNeed2);
					mod.PersonalNeedChanged = true;
				}
			}
			this._equippedItems[0] = this.SelectBestWeapon(true);
			this._equippedItems[1] = this.SelectBestWeapon(true);
			this._equippedItems[2] = this.SelectBestWeapon(false);
		}

		// Token: 0x0600765E RID: 30302 RVA: 0x00453BA4 File Offset: 0x00451DA4
		private void SelectFixedWeapons(Character character, SelectEquipmentsModification mod)
		{
			CharacterItem charConfig = Character.Instance[character.GetTemplateId()];
			for (sbyte slot = 0; slot <= 2; slot += 1)
			{
				PresetEquipmentItem presetWeapon = charConfig.PresetEquipment[(int)slot];
				bool flag = presetWeapon.TemplateId < 0;
				if (flag)
				{
					this._equippedItems[(int)slot] = ItemKey.Invalid;
				}
				else
				{
					int weaponIndex = this._availableWeapons.FindIndex(([TupleElementNames(new string[]
					{
						"weapon",
						"score"
					})] ValueTuple<ItemKey, int> pair) => pair.Item1.TemplateId == presetWeapon.TemplateId);
					ItemKey selectedWeapon = this._availableWeapons[weaponIndex].Item1;
					this._availableWeapons.RemoveAt(weaponIndex);
					this._equippedItems[(int)slot] = selectedWeapon;
				}
			}
		}

		// Token: 0x0600765F RID: 30303 RVA: 0x00453C68 File Offset: 0x00451E68
		private unsafe static void CalcAttackSkillsRequirement(CombatSkillEquipment skillEquipment, short* pRequiredHitRates, byte* pRequiredTricks, [TupleElementNames(new string[]
		{
			"itemTemplateId",
			"count"
		})] List<ValueTuple<short, short>> suitableWeapons, HashSet<short> fixedBestWeapons)
		{
			foreach (short ptr in skillEquipment.Attack)
			{
				short skillTemplateId = ptr;
				bool flag = skillTemplateId < 0;
				if (!flag)
				{
					CombatSkillItem skillConfig = Config.CombatSkill.Instance[skillTemplateId];
					bool flag2 = skillConfig.MostFittingWeaponID >= 0;
					if (flag2)
					{
						Equipping.RecordSuitableWeapon(skillConfig.MostFittingWeaponID, suitableWeapons);
					}
					bool flag3 = skillConfig.FixedBestWeaponID >= 0;
					if (flag3)
					{
						fixedBestWeapons.Add(skillConfig.FixedBestWeaponID);
					}
					bool flag4 = !CombatSkillTemplateHelper.IsMindHitSkill(skillTemplateId);
					if (flag4)
					{
						short* ptr2 = pRequiredHitRates + 2;
						*ptr2 += (short)skillConfig.PerHitDamageRateDistribution[0];
						short* ptr3 = pRequiredHitRates + 1;
						*ptr3 += (short)skillConfig.PerHitDamageRateDistribution[1];
						*pRequiredHitRates += (short)skillConfig.PerHitDamageRateDistribution[2];
					}
					else
					{
						short* ptr4 = pRequiredHitRates + 3;
						*ptr4 += (short)skillConfig.PerHitDamageRateDistribution[3];
					}
					List<NeedTrick> tricks = skillConfig.TrickCost;
					int trickIdx = 0;
					int tricksCount = tricks.Count;
					while (trickIdx < tricksCount)
					{
						NeedTrick trick = tricks[trickIdx];
						byte* ptr5 = pRequiredTricks + trick.TrickType;
						*ptr5 += trick.NeedCount;
						trickIdx++;
					}
				}
			}
		}

		// Token: 0x06007660 RID: 30304 RVA: 0x00453D98 File Offset: 0x00451F98
		private unsafe static bool CalcWeaponScores(short* pRequiredHitRates, byte* pRequiredTricks, [TupleElementNames(new string[]
		{
			"weapon",
			"score"
		})] List<ValueTuple<GameData.Domains.Item.Weapon, int>> availableWeapons, [TupleElementNames(new string[]
		{
			"itemTemplateId",
			"count"
		})] List<ValueTuple<short, short>> suitableWeapons)
		{
			byte* pClonedRequiredTricks = stackalloc byte[(UIntPtr)22];
			bool hasMatchTricks = false;
			int i = 0;
			int weaponsCount = availableWeapons.Count;
			while (i < weaponsCount)
			{
				GameData.Domains.Item.Weapon weapon = availableWeapons[i].Item1;
				short weaponTemplateId = weapon.GetTemplateId();
				WeaponItem weaponConfig = Config.Weapon.Instance[weaponTemplateId];
				Buffer.MemoryCopy((void*)pRequiredTricks, (void*)pClonedRequiredTricks, 22L, 22L);
				int matchedTricksCount = Equipping.CalcMatchedTricksCount(pClonedRequiredTricks, weapon);
				int score = (matchedTricksCount * 50 + (int)weaponConfig.Grade * 200) * (int)weapon.GetCurrDurability() / (int)weapon.GetMaxDurability();
				bool flag = matchedTricksCount > 0;
				if (flag)
				{
					hasMatchTricks = true;
					short baseItemTemplateId = weaponTemplateId - (short)weaponConfig.Grade;
					score += Equipping.GetSuitableWeaponCount(baseItemTemplateId, suitableWeapons) * 300;
					HitOrAvoidShorts hitFactors = weapon.GetHitFactors();
					for (int hitType = 0; hitType < 4; hitType++)
					{
						score += (int)(pRequiredHitRates[hitType] * *(ref hitFactors.Items.FixedElementField + (IntPtr)hitType * 2) / 150);
					}
					score += 65536;
				}
				availableWeapons[i] = new ValueTuple<GameData.Domains.Item.Weapon, int>(weapon, score);
				i++;
			}
			return hasMatchTricks;
		}

		// Token: 0x06007661 RID: 30305 RVA: 0x00453EC8 File Offset: 0x004520C8
		private ItemKey SelectBestWeapon(bool removeSameType)
		{
			int availableWeaponsCount = this._availableWeapons.Count;
			bool flag = availableWeaponsCount <= 0;
			ItemKey result;
			if (flag)
			{
				result = ItemKey.Invalid;
			}
			else
			{
				int maxScore = int.MinValue;
				int selectedIdx = 0;
				for (int i = 0; i < availableWeaponsCount; i++)
				{
					int score = this._availableWeapons[i].Item2;
					bool flag2 = score > maxScore;
					if (flag2)
					{
						maxScore = score;
						selectedIdx = i;
					}
				}
				ItemKey selectedWeapon = this._availableWeapons[selectedIdx].Item1;
				if (removeSameType)
				{
					CollectionUtils.SwapAndRemove<ValueTuple<ItemKey, int>>(this._availableWeapons, selectedIdx);
					WeaponItem selectedWeaponConfig = Config.Weapon.Instance[selectedWeapon.TemplateId];
					short selectedItemSubType = selectedWeaponConfig.ItemSubType;
					int j = 0;
					int count = this._availableWeapons.Count;
					while (j < count)
					{
						ItemKey currWeapon = this._availableWeapons[j].Item1;
						WeaponItem currWeaponConfig = Config.Weapon.Instance[currWeapon.TemplateId];
						short currItemSubType = currWeaponConfig.ItemSubType;
						bool flag3 = currItemSubType == selectedItemSubType && currItemSubType != 16;
						if (flag3)
						{
							CollectionUtils.SwapAndRemove<ValueTuple<ItemKey, int>>(this._availableWeapons, j);
							count--;
							j--;
						}
						j++;
					}
				}
				result = selectedWeapon;
			}
			return result;
		}

		// Token: 0x06007662 RID: 30306 RVA: 0x0045401C File Offset: 0x0045221C
		private static void RecordSuitableWeapon(short itemTemplateId, [TupleElementNames(new string[]
		{
			"itemTemplateId",
			"count"
		})] List<ValueTuple<short, short>> suitableWeapons)
		{
			int index = -1;
			int i = 0;
			int count = suitableWeapons.Count;
			while (i < count)
			{
				bool flag = suitableWeapons[i].Item1 != itemTemplateId;
				if (!flag)
				{
					index = i;
					break;
				}
				i++;
			}
			bool flag2 = index >= 0;
			if (flag2)
			{
				suitableWeapons[index] = new ValueTuple<short, short>(itemTemplateId, suitableWeapons[index].Item2 + 1);
			}
			else
			{
				suitableWeapons.Add(new ValueTuple<short, short>(itemTemplateId, 1));
			}
		}

		// Token: 0x06007663 RID: 30307 RVA: 0x0045409C File Offset: 0x0045229C
		private static int GetSuitableWeaponCount(short itemTemplateId, [TupleElementNames(new string[]
		{
			"itemTemplateId",
			"count"
		})] List<ValueTuple<short, short>> suitableWeapons)
		{
			int index = -1;
			int i = 0;
			int count = suitableWeapons.Count;
			while (i < count)
			{
				bool flag = suitableWeapons[i].Item1 != itemTemplateId;
				if (!flag)
				{
					index = i;
					break;
				}
				i++;
			}
			return (int)((index >= 0) ? suitableWeapons[index].Item2 : 0);
		}

		// Token: 0x06007664 RID: 30308 RVA: 0x004540FC File Offset: 0x004522FC
		private unsafe static int CalcMatchedTricksCount(byte* pClonedRequiredTricks, GameData.Domains.Item.Weapon weapon)
		{
			int matchedTricksCount = 0;
			List<sbyte> weaponTricks = weapon.GetTricks();
			int i = 0;
			int tricksCount = weaponTricks.Count;
			while (i < tricksCount)
			{
				sbyte trickType = weaponTricks[i];
				byte requiredTrickCount = pClonedRequiredTricks[trickType];
				bool flag = requiredTrickCount > 0;
				if (flag)
				{
					pClonedRequiredTricks[trickType] = requiredTrickCount - 1;
					matchedTricksCount++;
				}
				i++;
			}
			return matchedTricksCount;
		}

		// Token: 0x06007665 RID: 30309 RVA: 0x00454160 File Offset: 0x00452360
		private ItemKey SelectClothing(Character character, SelectEquipmentsModification mod, short orgClothingTemplateId)
		{
			bool flag = character.GetAgeGroup() != 2;
			ItemKey result;
			if (flag)
			{
				result = character.GetEquipment()[4];
			}
			else
			{
				bool flag2 = orgClothingTemplateId >= 0;
				if (flag2)
				{
					ItemKey clothingKey = this.SelectOrgClothing(character, orgClothingTemplateId);
					bool flag3 = clothingKey.IsValid();
					if (flag3)
					{
						return clothingKey;
					}
					PersonalNeed personalNeed = PersonalNeed.CreatePersonalNeed(10, 3, orgClothingTemplateId);
					character.OfflineAddPersonalNeed(personalNeed);
					mod.PersonalNeedChanged = true;
				}
				int maxScore = int.MinValue;
				int selectedIdx = -1;
				int i = 0;
				int count = this._availableClothing.Count;
				while (i < count)
				{
					GameData.Domains.Item.Clothing item = this._availableClothing[i];
					int score = Equipping.CalcEquipmentScore(item.GetItemKey(), -1).Item1;
					bool flag4 = score > maxScore;
					if (flag4)
					{
						maxScore = score;
						selectedIdx = i;
					}
					i++;
				}
				result = ((selectedIdx >= 0) ? this._availableClothing[selectedIdx].GetItemKey() : ItemKey.Invalid);
			}
			return result;
		}

		// Token: 0x06007666 RID: 30310 RVA: 0x00454260 File Offset: 0x00452460
		private ItemKey SelectOrgClothing(Character character, short orgClothingTemplateId)
		{
			ItemKey clothingKey = character.GetEquipment()[4];
			bool flag = clothingKey.IsValid() && clothingKey.TemplateId == orgClothingTemplateId;
			ItemKey result;
			if (flag)
			{
				result = clothingKey;
			}
			else
			{
				int i = 0;
				int count = this._availableClothing.Count;
				while (i < count)
				{
					GameData.Domains.Item.Clothing currClothing = this._availableClothing[i];
					bool flag2 = currClothing.GetTemplateId() == orgClothingTemplateId;
					if (flag2)
					{
						return currClothing.GetItemKey();
					}
					i++;
				}
				result = ItemKey.Invalid;
			}
			return result;
		}

		// Token: 0x06007667 RID: 30311 RVA: 0x004542EC File Offset: 0x004524EC
		private static ItemKey SelectEquipment<T>(SelectEquipmentsModification mod, List<T> availableEquipments, PresetEquipmentItemWithProb orgEquipment) where T : ItemBase
		{
			bool hasMeetOrgRequirement = false;
			int equipmentsCount = availableEquipments.Count;
			int maxScore = int.MinValue;
			int selectedIdx = 0;
			sbyte expectedGrade = mod.Character.GetOrganizationInfo().Grade;
			for (int i = 0; i < equipmentsCount; i++)
			{
				T item = availableEquipments[i];
				ValueTuple<int, bool> valueTuple = Equipping.CalcEquipmentScore(item.GetItemKey(), (int)orgEquipment.TemplateId);
				int score = valueTuple.Item1;
				bool itemMeetReq = valueTuple.Item2;
				hasMeetOrgRequirement = (hasMeetOrgRequirement || itemMeetReq);
				bool flag = score > maxScore;
				if (flag)
				{
					maxScore = score;
					selectedIdx = i;
				}
			}
			bool flag2 = !hasMeetOrgRequirement && orgEquipment.TemplateId >= 0;
			if (flag2)
			{
				short targetItemTemplateId = ItemTemplateHelper.GetTemplateIdInGroup(orgEquipment.Type, orgEquipment.TemplateId, expectedGrade);
				PersonalNeed personalNeed = PersonalNeed.CreatePersonalNeed(10, orgEquipment.Type, targetItemTemplateId);
				mod.Character.OfflineAddPersonalNeed(personalNeed);
				mod.PersonalNeedChanged = true;
			}
			bool flag3 = equipmentsCount <= 0;
			ItemKey result;
			if (flag3)
			{
				result = ItemKey.Invalid;
			}
			else
			{
				ItemKey itemKey = availableEquipments[selectedIdx].GetItemKey();
				CollectionUtils.SwapAndRemove<T>(availableEquipments, selectedIdx);
				result = itemKey;
			}
			return result;
		}

		// Token: 0x06007668 RID: 30312 RVA: 0x0045440C File Offset: 0x0045260C
		private static ItemKey SelectArmor(SelectEquipmentsModification mod, List<GameData.Domains.Item.Armor> availableArmors, PresetEquipmentItemWithProb orgEquipment)
		{
			return Equipping.SelectEquipment<GameData.Domains.Item.Armor>(mod, availableArmors, orgEquipment);
		}

		// Token: 0x06007669 RID: 30313 RVA: 0x00454428 File Offset: 0x00452628
		private static ItemKey SelectAccessory(SelectEquipmentsModification mod, List<GameData.Domains.Item.Accessory> availableAccessories, PresetEquipmentItemWithProb orgEquipment)
		{
			return Equipping.SelectEquipment<GameData.Domains.Item.Accessory>(mod, availableAccessories, orgEquipment);
		}

		// Token: 0x0600766A RID: 30314 RVA: 0x00454444 File Offset: 0x00452644
		private static ItemKey SelectCarrier(SelectEquipmentsModification mod, List<GameData.Domains.Item.Carrier> availableCarriers, PresetEquipmentItemWithProb orgEquipment)
		{
			return Equipping.SelectEquipment<GameData.Domains.Item.Carrier>(mod, availableCarriers, orgEquipment);
		}

		// Token: 0x0600766B RID: 30315 RVA: 0x00454460 File Offset: 0x00452660
		[return: TupleElementNames(new string[]
		{
			"score",
			"meetReq"
		})]
		public static ValueTuple<int, bool> CalcEquipmentScore(ItemKey itemKey, int expectedItemGroupTemplateId)
		{
			sbyte grade = ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId);
			int score = (int)grade * 200;
			bool meetReq = expectedItemGroupTemplateId >= 0 && (int)itemKey.TemplateId >= expectedItemGroupTemplateId && (int)itemKey.TemplateId <= expectedItemGroupTemplateId + 8;
			bool flag = meetReq;
			if (flag)
			{
				score += 300;
			}
			bool flag2 = itemKey.IsValid();
			if (flag2)
			{
				ItemBase itemBase = DomainManager.Item.GetBaseItem(itemKey);
				short currDurability = itemBase.GetCurrDurability();
				short maxDurability = itemBase.GetMaxDurability();
				score = ((maxDurability > 0) ? ((currDurability == 0) ? -1 : (score * (int)currDurability / (int)maxDurability)) : score);
			}
			return new ValueTuple<int, bool>(score, meetReq);
		}

		// Token: 0x0600766C RID: 30316 RVA: 0x00454508 File Offset: 0x00452708
		[Obsolete]
		private static bool MeetOrgRequirement(short itemTemplateId, PresetEquipmentItemWithProb orgEquipment)
		{
			return orgEquipment.TemplateId >= 0 && itemTemplateId >= orgEquipment.TemplateId && itemTemplateId <= orgEquipment.TemplateId + 8;
		}

		// Token: 0x0600766D RID: 30317 RVA: 0x00454540 File Offset: 0x00452740
		private void ChooseLoopingNeigong(Character character, SelectEquipmentsModification mod)
		{
			this._candidateCombatSkillsForLooping.Clear();
			Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(character.GetId());
			foreach (KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> keyValuePair in charCombatSkills)
			{
				short num;
				GameData.Domains.CombatSkill.CombatSkill combatSkill;
				keyValuePair.Deconstruct(out num, out combatSkill);
				short skillTemplateId = num;
				GameData.Domains.CombatSkill.CombatSkill skill = combatSkill;
				bool flag = !CharacterDomain.IsLoopable(skill);
				if (!flag)
				{
					CombatSkillItem skillCfg = Config.CombatSkill.Instance[skillTemplateId];
					bool canObtainNeili = skill.GetObtainedNeili() < skill.GetTotalObtainableNeili();
					this._candidateCombatSkillsForLooping.Add(new ValueTuple<CombatSkillItem, bool>(skillCfg, canObtainNeili));
				}
			}
			short loopingNeigong = Equipping.SelectCombatSkillForLooping(character, this._candidateCombatSkillsForLooping);
			bool flag2 = loopingNeigong != character.GetLoopingNeigong();
			if (flag2)
			{
				mod.LoopingNeigongChanged = true;
				mod.LoopingNeigong = loopingNeigong;
			}
		}

		// Token: 0x0600766E RID: 30318 RVA: 0x00454634 File Offset: 0x00452834
		private static short SelectCombatSkillForLooping(Character character, [TupleElementNames(new string[]
		{
			"skillCfg",
			"canObtainNeili"
		})] List<ValueTuple<CombatSkillItem, bool>> candidates)
		{
			sbyte selfOrgTemplateId = character.GetOrganizationInfo().OrgTemplateId;
			sbyte selfOrgElementType = Organization.Instance[selfOrgTemplateId].FiveElementsType;
			sbyte lovingOrgTemplateId = character.GetIdealSect();
			sbyte lovingOrgElementType = (lovingOrgTemplateId >= 0) ? Organization.Instance[lovingOrgTemplateId].FiveElementsType : -1;
			short selectedSkillTemplateId = -1;
			int maxScore = -1;
			int i = 0;
			int count = candidates.Count;
			while (i < count)
			{
				ValueTuple<CombatSkillItem, bool> valueTuple = candidates[i];
				CombatSkillItem skillConfig = valueTuple.Item1;
				bool canObtainNeili = valueTuple.Item2;
				GameData.Domains.CombatSkill.CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(character.GetId(), skillConfig.TemplateId));
				sbyte transferFromType = (skill.GetFiveElementsChange() > 0 && skillConfig.TransferTypeWhileLooping >= 0) ? NeiliProportionOfFiveElements.GetTransferSource(skillConfig.TransferTypeWhileLooping, skillConfig.DestTypeWhileLooping) : -1;
				int score = 0;
				score += (int)(50 * skillConfig.Grade);
				bool flag = canObtainNeili;
				if (flag)
				{
					score += 800;
				}
				bool flag2 = selfOrgElementType >= 0;
				if (flag2)
				{
					bool flag3 = !Equipping.CheckCounterWithTargetFiveElementsType((short)skillConfig.FiveElements, selfOrgElementType);
					if (flag3)
					{
						score += 200;
					}
					bool flag4 = skillConfig.DestTypeWhileLooping == selfOrgElementType;
					if (flag4)
					{
						score += 100;
					}
					bool flag5 = Equipping.CheckCounterWithTargetFiveElementsType((short)transferFromType, selfOrgElementType);
					if (flag5)
					{
						score += 100;
					}
				}
				bool flag6 = lovingOrgElementType >= 0;
				if (flag6)
				{
					bool flag7 = !Equipping.CheckCounterWithTargetFiveElementsType((short)skillConfig.FiveElements, lovingOrgElementType);
					if (flag7)
					{
						score += 200;
					}
					bool flag8 = skillConfig.DestTypeWhileLooping == lovingOrgElementType;
					if (flag8)
					{
						score += 100;
					}
					bool flag9 = Equipping.CheckCounterWithTargetFiveElementsType((short)transferFromType, lovingOrgElementType);
					if (flag9)
					{
						score += 100;
					}
				}
				bool flag10 = Equipping.CanObtainExtraNeiliAllocationProgressFromSkill(character, skillConfig);
				if (flag10)
				{
					score += 200;
				}
				bool flag11 = score > maxScore;
				if (flag11)
				{
					selectedSkillTemplateId = skillConfig.TemplateId;
					maxScore = score;
				}
				i++;
			}
			return (maxScore >= 0) ? selectedSkillTemplateId : -1;
		}

		// Token: 0x0600766F RID: 30319 RVA: 0x0045482C File Offset: 0x00452A2C
		private CombatSkillItem SelectCombatSkillForAdjustingNeiliType(Character character, [TupleElementNames(new string[]
		{
			"skillCfg",
			"index"
		})] List<ValueTuple<CombatSkillItem, int>> brokenOutNeigongList)
		{
			this._candidateCombatSkillsForLooping.Clear();
			int i = 0;
			int count = brokenOutNeigongList.Count;
			while (i < count)
			{
				CombatSkillItem skillCfg = brokenOutNeigongList[i].Item1;
				GameData.Domains.CombatSkill.CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(character.GetId(), skillCfg.TemplateId));
				bool flag = skill.GetFiveElementsChange() > 0 && skillCfg.TransferTypeWhileLooping >= 0;
				if (flag)
				{
					this._candidateCombatSkillsForLooping.Add(new ValueTuple<CombatSkillItem, bool>(skillCfg, false));
				}
				i++;
			}
			short skillTemplateId = Equipping.SelectCombatSkillForLooping(character, this._candidateCombatSkillsForLooping);
			return (skillTemplateId < 0) ? null : Config.CombatSkill.Instance[skillTemplateId];
		}

		// Token: 0x06007670 RID: 30320 RVA: 0x004548E4 File Offset: 0x00452AE4
		private static bool CanObtainExtraNeiliAllocationProgressFromSkill(Character character, CombatSkillItem skillCfg)
		{
			sbyte[] extraProgressCfg = skillCfg.ExtraNeiliAllocationProgress;
			IntList currentProgress;
			bool flag = !DomainManager.Extra.TryGetExtraNeiliAllocationProgress(character.GetId(), out currentProgress);
			if (flag)
			{
				for (int neiliAllocationType = 0; neiliAllocationType < 4; neiliAllocationType++)
				{
					bool flag2 = extraProgressCfg[neiliAllocationType] > 0;
					if (flag2)
					{
						return true;
					}
				}
			}
			for (int neiliAllocationType2 = 0; neiliAllocationType2 < 4; neiliAllocationType2++)
			{
				sbyte configProgressDelta = extraProgressCfg[neiliAllocationType2];
				bool flag3 = configProgressDelta <= 0;
				if (!flag3)
				{
					int newProgress = Math.Min((int)(100 * GlobalConfig.Instance.ExtraNeiliAllocationFromProgressRatio * GlobalConfig.Instance.MaxExtraNeiliAllocation), (int)(configProgressDelta * 100) + currentProgress.Items[neiliAllocationType2]);
					bool flag4 = newProgress <= currentProgress.Items[neiliAllocationType2];
					if (!flag4)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007671 RID: 30321 RVA: 0x004549C4 File Offset: 0x00452BC4
		public unsafe void AllocateNeili(DataContext context, Character character)
		{
			sbyte* skillSlotTotalCounts = stackalloc sbyte[(UIntPtr)5];
			SelectEquipmentsModification mod = new SelectEquipmentsModification(character, false);
			Equipping.AllocateNeili(character, skillSlotTotalCounts, mod);
			bool neiliAllocationChanged = mod.NeiliAllocationChanged;
			if (neiliAllocationChanged)
			{
				character.SpecifyBaseNeiliAllocation(context, mod.NeiliAllocation);
			}
		}

		// Token: 0x06007672 RID: 30322 RVA: 0x00454A00 File Offset: 0x00452C00
		private unsafe static void AllocateNeili(Character character, sbyte* skillSlotTotalCounts, SelectEquipmentsModification mod)
		{
			sbyte neiliType = character.GetNeiliType();
			sbyte[] proportions = NeiliType.Instance[neiliType].IdeaAllocationProportion;
			CharacterItem template = Character.Instance[character.GetTemplateId()];
			bool flag = character.GetCreatingType() == 0 && template.IdeaAllocationProportion.Sum() > 0;
			if (flag)
			{
				proportions = template.IdeaAllocationProportion;
			}
			IntPtr intPtr = stackalloc byte[(UIntPtr)4];
			*intPtr = (byte)proportions[0];
			*(intPtr + 1) = (byte)proportions[1];
			*(intPtr + 2) = (byte)proportions[2];
			*(intPtr + 3) = (byte)proportions[3];
			sbyte* pProportions = intPtr;
			NeiliAllocation allocations = Equipping.FindBestNeiliAllocation(character, pProportions);
			NeiliAllocation oriAllocations = character.GetBaseNeiliAllocation();
			bool flag2 = !NeiliAllocation.Equals(oriAllocations, allocations);
			if (flag2)
			{
				mod.NeiliAllocationChanged = true;
				mod.NeiliAllocation = allocations;
			}
		}

		// Token: 0x06007673 RID: 30323 RVA: 0x00454AB0 File Offset: 0x00452CB0
		private unsafe static void CalcNeiliAllocationProportions(sbyte* skillSlotTotalCounts, sbyte* pProportions)
		{
			for (int i = 0; i < 4; i++)
			{
				pProportions[i] = (sbyte)Math.Max((int)(skillSlotTotalCounts[i + 1] + 6), 6);
			}
		}

		// Token: 0x06007674 RID: 30324 RVA: 0x00454AE4 File Offset: 0x00452CE4
		private unsafe static NeiliAllocation FindBestNeiliAllocation(Character character, sbyte* pProportions)
		{
			IntPtr intPtr = stackalloc byte[(UIntPtr)4];
			cpblk(intPtr, ref <PrivateImplementationDetails>.054EDEC1D0211F624FED0CBCA9D4F9400B0E491C43742AF2C5B0ABEBF0C990D8, 4);
			sbyte* pAllocationTypes = intPtr;
			Span<sbyte> proportions = new Span<sbyte>((void*)pProportions, 4);
			Span<sbyte> allocationTypes = new Span<sbyte>((void*)pAllocationTypes, 4);
			proportions.Sort(allocationTypes, Equipping.ReverseComparer);
			short proportionSum = (short)(*pProportions + pProportions[1] + pProportions[2] + pProportions[3]);
			short maxTotalAllocation = CombatHelper.GetMaxTotalNeiliAllocationConsideringFeature(character.GetConsummateLevel(), character.GetFeatureIds());
			NeiliAllocation allocations = Equipping.CalcNeiliAllocation(pProportions, (int)proportionSum, (int)maxTotalAllocation);
			int availableNeili = character.GetPureCurrNeili();
			bool flag = CombatHelper.CalcRequiredNeili(allocations) <= availableNeili;
			NeiliAllocation result;
			if (flag)
			{
				allocations = Equipping.RestorePositions(allocations, pAllocationTypes);
				result = allocations;
			}
			else
			{
				allocations = Equipping.BinarySearch(pProportions, (int)proportionSum, (int)maxTotalAllocation, availableNeili);
				allocations = Equipping.RestorePositions(allocations, pAllocationTypes);
				Tester.Assert(CombatHelper.CalcRequiredNeili(allocations) <= availableNeili, "");
				short totalAllocation = allocations.GetTotal();
				NeiliAllocation tmpAllocations = Equipping.CalcNeiliAllocation(pProportions, (int)proportionSum, (int)(totalAllocation + 1));
				Tester.Assert(CombatHelper.CalcRequiredNeili(tmpAllocations) > availableNeili, "");
				result = allocations;
			}
			return result;
		}

		// Token: 0x06007675 RID: 30325 RVA: 0x00454BE4 File Offset: 0x00452DE4
		private unsafe static NeiliAllocation BinarySearch(sbyte* pProportions, int proportionSum, int maxTotalAllocation, int availableNeili)
		{
			int low = 0;
			int high = maxTotalAllocation;
			while (low <= high)
			{
				int middle = low + (high - low) / 2;
				NeiliAllocation allocations = Equipping.CalcNeiliAllocation(pProportions, proportionSum, middle);
				int comparison = CombatHelper.CalcRequiredNeili(allocations) - availableNeili;
				bool flag = comparison == 0;
				if (flag)
				{
					return allocations;
				}
				bool flag2 = comparison < 0;
				if (flag2)
				{
					low = middle + 1;
				}
				else
				{
					high = middle - 1;
				}
			}
			int targetTotalAllocation = low - 1;
			bool flag3 = targetTotalAllocation > 0;
			if (flag3)
			{
				return Equipping.CalcNeiliAllocation(pProportions, proportionSum, targetTotalAllocation);
			}
			NeiliAllocation allocation = default(NeiliAllocation);
			allocation.Initialize();
			return allocation;
		}

		// Token: 0x06007676 RID: 30326 RVA: 0x00454C80 File Offset: 0x00452E80
		private unsafe static NeiliAllocation CalcNeiliAllocation(sbyte* pProportions, int proportionSum, int totalAllocation)
		{
			int leftProportionSum = proportionSum;
			int leftTotalAllocation = totalAllocation;
			NeiliAllocation allocations = default(NeiliAllocation);
			for (int i = 0; i < 4; i++)
			{
				sbyte currProportion = pProportions[i];
				int currAllocation = leftTotalAllocation * (int)currProportion / leftProportionSum;
				bool flag = currAllocation > 100;
				if (flag)
				{
					currAllocation = 100;
				}
				*(ref allocations.Items.FixedElementField + (IntPtr)i * 2) = (short)currAllocation;
				leftTotalAllocation -= currAllocation;
				leftProportionSum -= (int)currProportion;
			}
			return allocations;
		}

		// Token: 0x06007677 RID: 30327 RVA: 0x00454CF4 File Offset: 0x00452EF4
		private unsafe static NeiliAllocation RestorePositions(NeiliAllocation allocations, sbyte* pAllocationTypes)
		{
			NeiliAllocation restored = default(NeiliAllocation);
			for (int i = 0; i < 4; i++)
			{
				sbyte type = pAllocationTypes[i];
				*(ref restored.Items.FixedElementField + (IntPtr)type * 2) = *(ref allocations.Items.FixedElementField + (IntPtr)i * 2);
			}
			return restored;
		}

		// Token: 0x06007678 RID: 30328 RVA: 0x00454D4C File Offset: 0x00452F4C
		[return: TupleElementNames(new string[]
		{
			"book",
			"learnedSkillIndex",
			"readingPage"
		})]
		public ValueTuple<GameData.Domains.Item.SkillBook, int, byte> GetCurrReadingBook(Character character)
		{
			this._availableReadingBooks.Clear();
			List<ItemKey> readableBookList = ObjectPool<List<ItemKey>>.Instance.Get();
			character.GetReadableBookList(readableBookList);
			foreach (ItemKey itemKey in readableBookList)
			{
				GameData.Domains.Item.SkillBook book = DomainManager.Item.GetElement_SkillBooks(itemKey.Id);
				bool flag = book.IsCombatSkillBook();
				int learnedSkillIndex;
				byte readingPage;
				if (flag)
				{
					ValueTuple<int, byte> combatSkillBookCurrReadingInfo = character.GetCombatSkillBookCurrReadingInfo(book);
					learnedSkillIndex = combatSkillBookCurrReadingInfo.Item1;
					readingPage = combatSkillBookCurrReadingInfo.Item2;
				}
				else
				{
					ValueTuple<int, byte> lifeSkillBookCurrReadingInfo = character.GetLifeSkillBookCurrReadingInfo(book);
					learnedSkillIndex = lifeSkillBookCurrReadingInfo.Item1;
					readingPage = lifeSkillBookCurrReadingInfo.Item2;
				}
				this._availableReadingBooks.Add(new ValueTuple<GameData.Domains.Item.SkillBook, int, byte>(book, learnedSkillIndex, readingPage));
			}
			ObjectPool<List<ItemKey>>.Instance.Return(readableBookList);
			bool flag2 = this._availableReadingBooks.Count == 0;
			ValueTuple<GameData.Domains.Item.SkillBook, int, byte> result;
			if (flag2)
			{
				result = new ValueTuple<GameData.Domains.Item.SkillBook, int, byte>(null, -1, 0);
			}
			else
			{
				this._hasPersonalNeedToReadBooks.Clear();
				this._hasPersonalNeedToLearnCombatSkillTypes.Clear();
				this._hasPersonalNeedToLearnLifeSkillTypes.Clear();
				foreach (PersonalNeed personalNeed in character.GetPersonalNeeds())
				{
					bool flag3 = personalNeed.TemplateId == 14;
					if (flag3)
					{
						this._hasPersonalNeedToLearnCombatSkillTypes.Add(personalNeed.CombatSkillType);
					}
					else
					{
						bool flag4 = personalNeed.TemplateId == 15;
						if (flag4)
						{
							this._hasPersonalNeedToLearnLifeSkillTypes.Add(personalNeed.LifeSkillType);
						}
					}
				}
				int indexWithMaxScore = -1;
				int maxScore = int.MinValue;
				int indexWithMinAttainmentDiff = -1;
				int minAttainmentDiff = int.MaxValue;
				int scoreOfMinAttainmentDiff = int.MinValue;
				for (int i = 0; i < this._availableReadingBooks.Count; i++)
				{
					GameData.Domains.Item.SkillBook book2 = this._availableReadingBooks[i].Item1;
					sbyte bookGrade = book2.GetGrade();
					short needAttainment = SkillGradeData.Instance[bookGrade].ReadingAttainmentRequirement;
					SkillBookItem bookConfig = Config.SkillBook.Instance[book2.GetItemKey().TemplateId];
					short attainment;
					int score = Equipping.CalcSkillBookScore(book2, bookConfig, character, this._hasPersonalNeedToLearnCombatSkillTypes, this._hasPersonalNeedToLearnLifeSkillTypes, out attainment);
					bool flag5 = score > maxScore;
					if (flag5)
					{
						maxScore = score;
						indexWithMaxScore = i;
					}
					int diff = (int)(attainment - needAttainment);
					bool flag6 = diff >= 0 && diff < minAttainmentDiff;
					if (flag6)
					{
						minAttainmentDiff = diff;
						indexWithMinAttainmentDiff = i;
						scoreOfMinAttainmentDiff = score;
					}
				}
				bool flag7 = indexWithMinAttainmentDiff >= 0;
				if (flag7)
				{
					scoreOfMinAttainmentDiff += 50;
				}
				result = ((scoreOfMinAttainmentDiff > maxScore) ? this._availableReadingBooks[indexWithMinAttainmentDiff] : this._availableReadingBooks[indexWithMaxScore]);
			}
			return result;
		}

		// Token: 0x06007679 RID: 30329 RVA: 0x00455014 File Offset: 0x00453214
		private unsafe static int CalcSkillBookScore(GameData.Domains.Item.SkillBook book, SkillBookItem bookConfig, Character character, List<sbyte> needToLearnCombatSkillTypes, List<sbyte> needToLearnLifeSkillTypes, out short attainment)
		{
			short roleTemplateId = DomainManager.Extra.GetVillagerRoleTemplateId(character.GetId());
			CombatSkillShorts combatSkillAttainments = *character.GetCombatSkillAttainments();
			LifeSkillShorts lifeSkillAttainments = *character.GetLifeSkillAttainments();
			CombatSkillShorts combatSkillQualifications = *character.GetCombatSkillQualifications();
			LifeSkillShorts lifeSkillQualifications = *character.GetLifeSkillQualifications();
			OrganizationInfo orgInfo = character.GetOrganizationInfo();
			sbyte lovingOrgMemberId = character.GetIdealSect();
			OrganizationMemberItem lovingOrgMemberCfg = OrganizationDomain.GetOrgMemberConfig((lovingOrgMemberId >= 0) ? lovingOrgMemberId : orgInfo.OrgTemplateId, orgInfo.Grade);
			OrganizationMemberItem selfOrgMemberCfg = OrganizationDomain.GetOrgMemberConfig(orgInfo.OrgTemplateId, orgInfo.Grade);
			VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(character.GetId());
			VillagerWorkData workData = (villagerRole != null) ? villagerRole.WorkData : null;
			BuildingBlockData blockData;
			int workerPreferredSkill = (int)((workData != null && DomainManager.Building.TryGetElement_BuildingBlocks(new BuildingBlockKey(workData.AreaId, workData.BlockId, workData.BuildingBlockIndex), out blockData)) ? ((bookConfig.CombatSkillType >= 0) ? blockData.ConfigData.RequireCombatSkillType : blockData.ConfigData.RequireLifeSkillType) : -1);
			bool flag = bookConfig.CombatSkillType >= 0;
			int score;
			if (flag)
			{
				sbyte skillType = bookConfig.CombatSkillType;
				attainment = *(ref combatSkillAttainments.Items.FixedElementField + (IntPtr)skillType * 2);
				score = Equipping.CalcCombatSkillBookScore(book, skillType, selfOrgMemberCfg, lovingOrgMemberCfg, ref combatSkillQualifications, roleTemplateId, needToLearnCombatSkillTypes);
				bool flag2 = workerPreferredSkill == (int)bookConfig.CombatSkillType;
				if (flag2)
				{
					score += 1000;
				}
			}
			else
			{
				sbyte skillType2 = bookConfig.LifeSkillType;
				attainment = *(ref lifeSkillAttainments.Items.FixedElementField + (IntPtr)skillType2 * 2);
				score = Equipping.CalcLifeSkillBookScore(book, skillType2, selfOrgMemberCfg, lovingOrgMemberCfg, ref lifeSkillQualifications, roleTemplateId, needToLearnLifeSkillTypes);
				bool flag3 = workerPreferredSkill == (int)bookConfig.LifeSkillType;
				if (flag3)
				{
					score += 1000;
				}
			}
			return score;
		}

		// Token: 0x0600767A RID: 30330 RVA: 0x004551D0 File Offset: 0x004533D0
		private unsafe static int CalcCombatSkillBookScore(GameData.Domains.Item.SkillBook book, sbyte combatSkillType, OrganizationMemberItem selfOrgMemberCfg, OrganizationMemberItem lovingOrgMemberCfg, ref CombatSkillShorts qualifications, short roleTemplateId, List<sbyte> needToLearnSkillTypes)
		{
			int score = 0;
			score += (int)(selfOrgMemberCfg.CombatSkillsAdjust[(int)combatSkillType] * 10);
			score += (int)(lovingOrgMemberCfg.CombatSkillsAdjust[(int)combatSkillType] * 5);
			score += (int)(*(ref qualifications.Items.FixedElementField + (IntPtr)combatSkillType * 2));
			bool flag = book != null;
			if (flag)
			{
				score += SkillBookStateHelper.GetTotalIncompleteStateValue(book.GetPageIncompleteState(), (int)book.GetPageCount());
			}
			bool flag2 = needToLearnSkillTypes != null && needToLearnSkillTypes.Contains(combatSkillType);
			if (flag2)
			{
				score += 100;
			}
			bool flag3 = roleTemplateId == 5;
			if (flag3)
			{
				score += 100;
			}
			return score;
		}

		// Token: 0x0600767B RID: 30331 RVA: 0x0045525C File Offset: 0x0045345C
		private unsafe static int CalcLifeSkillBookScore(GameData.Domains.Item.SkillBook book, sbyte lifeSkillType, OrganizationMemberItem selfOrgMemberCfg, OrganizationMemberItem lovingOrgMemberCfg, ref LifeSkillShorts qualifications, short roleTemplateId, List<sbyte> needToLearnSkillTypes)
		{
			int score = 0;
			score += (int)(selfOrgMemberCfg.LifeSkillsAdjust[(int)lifeSkillType] * 10);
			score += (int)(lovingOrgMemberCfg.LifeSkillsAdjust[(int)lifeSkillType] * 5);
			score += (int)(*(ref qualifications.Items.FixedElementField + (IntPtr)lifeSkillType * 2));
			bool flag = book != null;
			if (flag)
			{
				score += SkillBookStateHelper.GetTotalIncompleteStateValue(book.GetPageIncompleteState(), (int)book.GetPageCount());
			}
			bool flag2 = needToLearnSkillTypes != null && needToLearnSkillTypes.Contains(lifeSkillType);
			if (flag2)
			{
				score += 100;
			}
			HashSet<sbyte> lifeSkillTypes = TaiwuDomain.VillagerRoleNeedLifeSkillBooks[roleTemplateId];
			bool flag3 = lifeSkillTypes != null && lifeSkillTypes.Contains(lifeSkillType);
			if (flag3)
			{
				score += 100;
			}
			return score;
		}

		// Token: 0x0600767C RID: 30332 RVA: 0x004552FC File Offset: 0x004534FC
		public static void SortBooksByScore([TupleElementNames(new string[]
		{
			"itemKey",
			"score"
		})] List<ValueTuple<ItemKey, int>> items, Character character)
		{
			List<sbyte> needToLearnCombatSkillTypes = ObjectPool<List<sbyte>>.Instance.Get();
			List<sbyte> needToLearnLifeSkillTypes = ObjectPool<List<sbyte>>.Instance.Get();
			foreach (PersonalNeed personalNeed in character.GetPersonalNeeds())
			{
				bool flag = personalNeed.TemplateId == 14;
				if (flag)
				{
					needToLearnCombatSkillTypes.Add(personalNeed.CombatSkillType);
				}
				else
				{
					bool flag2 = personalNeed.TemplateId == 15;
					if (flag2)
					{
						needToLearnLifeSkillTypes.Add(personalNeed.LifeSkillType);
					}
				}
			}
			items.Sort(delegate([TupleElementNames(new string[]
			{
				"itemKey",
				"score"
			})] ValueTuple<ItemKey, int> a, [TupleElementNames(new string[]
			{
				"itemKey",
				"score"
			})] ValueTuple<ItemKey, int> b)
			{
				GameData.Domains.Item.SkillBook bookA = a.Item1.IsValid() ? DomainManager.Item.GetElement_SkillBooks(a.Item1.Id) : null;
				SkillBookItem bookConfigA = Config.SkillBook.Instance[a.Item1.TemplateId];
				short num;
				a.Item2 = Equipping.CalcSkillBookScore(bookA, bookConfigA, character, needToLearnCombatSkillTypes, needToLearnLifeSkillTypes, out num);
				GameData.Domains.Item.SkillBook bookB = b.Item1.IsValid() ? DomainManager.Item.GetElement_SkillBooks(b.Item1.Id) : null;
				SkillBookItem bookConfigB = Config.SkillBook.Instance[b.Item1.TemplateId];
				b.Item2 = Equipping.CalcSkillBookScore(bookB, bookConfigB, character, needToLearnCombatSkillTypes, needToLearnLifeSkillTypes, out num);
				return a.Item2.CompareTo(b.Item2);
			});
			ObjectPool<List<sbyte>>.Instance.Return(needToLearnCombatSkillTypes);
			ObjectPool<List<sbyte>>.Instance.Return(needToLearnLifeSkillTypes);
		}

		// Token: 0x04002098 RID: 8344
		private readonly List<GameData.Domains.CombatSkill.CombatSkill>[] _availableCombatSkills;

		// Token: 0x04002099 RID: 8345
		private List<short> _masteredSkills;

		// Token: 0x0400209A RID: 8346
		private readonly CombatSkillEquipment _equippedCombatSkills;

		// Token: 0x0400209B RID: 8347
		private short[] _combatSkillAttainmentPanels;

		// Token: 0x0400209C RID: 8348
		private readonly LocalObjectPool<SectCandidateSkills> _sectCandidateSkillsPool;

		// Token: 0x0400209D RID: 8349
		private readonly List<SectCandidateSkills> _sectCandidateSkillInfos;

		// Token: 0x0400209E RID: 8350
		private readonly List<SectCandidateSkills> _sortedSectCandidateSkillInfos;

		// Token: 0x0400209F RID: 8351
		[TupleElementNames(new string[]
		{
			"skillCfg",
			"index"
		})]
		private static readonly IComparer<ValueTuple<CombatSkillItem, int>> Comparer = new Equipping.GradeComparer();

		// Token: 0x040020A0 RID: 8352
		private List<CombatSkillInitialBreakoutData> _brokenOutCombatSkills;

		// Token: 0x040020A1 RID: 8353
		private readonly List<GameData.Domains.CombatSkill.CombatSkill>[] _categorizedCombatSkillsByGrade;

		// Token: 0x040020A2 RID: 8354
		[TupleElementNames(new string[]
		{
			"skillCfg",
			"index"
		})]
		private readonly List<ValueTuple<CombatSkillItem, int>> _brokenOutNeigongList;

		// Token: 0x040020A3 RID: 8355
		private readonly HashSet<int> _usedRelatedCharIds;

		// Token: 0x040020A4 RID: 8356
		private readonly List<ShortPair> _skillBreakBonusWeights = new List<ShortPair>();

		// Token: 0x040020A5 RID: 8357
		private const sbyte EnsuredSuccessStepCount = 20;

		// Token: 0x040020A6 RID: 8358
		[Obsolete]
		private const sbyte RequiredStepMinCount = 10;

		// Token: 0x040020A7 RID: 8359
		[Obsolete]
		private const sbyte RequiredStepBaseCount = 15;

		// Token: 0x040020A8 RID: 8360
		[Obsolete]
		private const sbyte BreakoutBaseSuccessRate = 75;

		// Token: 0x040020A9 RID: 8361
		[Obsolete]
		private const short BreakoutFailureDefeatMarkCount = 4;

		// Token: 0x040020AA RID: 8362
		[Obsolete]
		private const short BreakoutFailureDisorderOfQi = 2000;

		// Token: 0x040020AB RID: 8363
		private HashSet<int> _askingForHelpSkills = new HashSet<int>();

		// Token: 0x040020AC RID: 8364
		[TupleElementNames(new string[]
		{
			"combatSkill",
			"score"
		})]
		private readonly List<ValueTuple<GameData.Domains.CombatSkill.CombatSkill, int>> _canUpdateCombatSkills = new List<ValueTuple<GameData.Domains.CombatSkill.CombatSkill, int>>();

		// Token: 0x040020AD RID: 8365
		private readonly List<PersonalNeed> _newPersonalNeeds = new List<PersonalNeed>();

		// Token: 0x040020AE RID: 8366
		private List<ItemKey> _consumedItems = new List<ItemKey>();

		// Token: 0x040020AF RID: 8367
		[TupleElementNames(new string[]
		{
			"combatSkill",
			"activationState"
		})]
		private List<ValueTuple<GameData.Domains.CombatSkill.CombatSkill, ushort>> _newlyActivatedCombatSkills = new List<ValueTuple<GameData.Domains.CombatSkill.CombatSkill, ushort>>();

		// Token: 0x040020B0 RID: 8368
		private List<GameData.Domains.CombatSkill.CombatSkill> _failedToBreakoutCombatSkills = new List<GameData.Domains.CombatSkill.CombatSkill>();

		// Token: 0x040020B1 RID: 8369
		[TupleElementNames(new string[]
		{
			"skillTemplateId",
			"startIndex",
			"bonuses"
		})]
		private List<ValueTuple<short, int, SerializableList<SkillBreakPlateBonus>>> _modifiedBreakPlateBonuses = new List<ValueTuple<short, int, SerializableList<SkillBreakPlateBonus>>>();

		// Token: 0x040020B2 RID: 8370
		private ItemKey[] _equippedItems;

		// Token: 0x040020B3 RID: 8371
		[TupleElementNames(new string[]
		{
			"weapon",
			"score"
		})]
		private readonly List<ValueTuple<ItemKey, int>> _availableWeapons;

		// Token: 0x040020B4 RID: 8372
		[TupleElementNames(new string[]
		{
			"itemTemplateId",
			"count"
		})]
		private readonly List<ValueTuple<short, short>> _suitableWeapons;

		// Token: 0x040020B5 RID: 8373
		private readonly HashSet<short> _fixedBestWeapons;

		// Token: 0x040020B6 RID: 8374
		private readonly List<GameData.Domains.Item.Armor> _availableHelms;

		// Token: 0x040020B7 RID: 8375
		private readonly List<GameData.Domains.Item.Armor> _availableTorsos;

		// Token: 0x040020B8 RID: 8376
		private readonly List<GameData.Domains.Item.Armor> _availableBracers;

		// Token: 0x040020B9 RID: 8377
		private readonly List<GameData.Domains.Item.Armor> _availableBoots;

		// Token: 0x040020BA RID: 8378
		private readonly List<GameData.Domains.Item.Accessory> _availableAccessories;

		// Token: 0x040020BB RID: 8379
		private readonly List<GameData.Domains.Item.Clothing> _availableClothing;

		// Token: 0x040020BC RID: 8380
		private readonly List<GameData.Domains.Item.Carrier> _availableCarriers;

		// Token: 0x040020BD RID: 8381
		[TupleElementNames(new string[]
		{
			"skillCfg",
			"canObtainNeili"
		})]
		private readonly List<ValueTuple<CombatSkillItem, bool>> _candidateCombatSkillsForLooping;

		// Token: 0x040020BE RID: 8382
		private static readonly IComparer<sbyte> ReverseComparer = new ReverseComparerSbyte();

		// Token: 0x040020BF RID: 8383
		[TupleElementNames(new string[]
		{
			"book",
			"learnedSkillIndex",
			"readingPage"
		})]
		private readonly List<ValueTuple<GameData.Domains.Item.SkillBook, int, byte>> _availableReadingBooks;

		// Token: 0x040020C0 RID: 8384
		private readonly List<short> _hasPersonalNeedToReadBooks;

		// Token: 0x040020C1 RID: 8385
		private readonly List<sbyte> _hasPersonalNeedToLearnCombatSkillTypes;

		// Token: 0x040020C2 RID: 8386
		private readonly List<sbyte> _hasPersonalNeedToLearnLifeSkillTypes;

		// Token: 0x02000C31 RID: 3121
		public ref struct EquipCombatSkillContext
		{
			// Token: 0x04003535 RID: 13621
			public bool IsTaiwu;

			// Token: 0x04003536 RID: 13622
			public bool EquipBestSkillsForWeapon;

			// Token: 0x04003537 RID: 13623
			public ItemKey[] Equipments;

			// Token: 0x04003538 RID: 13624
			public sbyte SlotCostTemplateAdjust;

			// Token: 0x04003539 RID: 13625
			public Personalities Personalities;

			// Token: 0x0400353A RID: 13626
			public sbyte NeiliType;

			// Token: 0x0400353B RID: 13627
			public sbyte OrgTemplateId;

			// Token: 0x0400353C RID: 13628
			public sbyte IdealSectId;

			// Token: 0x0400353D RID: 13629
			public List<sbyte> OwnedLegendaryBookTypes;

			// Token: 0x0400353E RID: 13630
			public Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> CharacterCombatSkills;

			// Token: 0x0400353F RID: 13631
			public unsafe sbyte* SlotTotalCounts;

			// Token: 0x04003540 RID: 13632
			public CombatSkillEquipment EquippedSkills;
		}

		// Token: 0x02000C32 RID: 3122
		private class GradeComparer : IComparer<ValueTuple<CombatSkillItem, int>>
		{
			// Token: 0x06008E64 RID: 36452 RVA: 0x004FD83D File Offset: 0x004FBA3D
			public int Compare([TupleElementNames(new string[]
			{
				"skillCfg",
				"index"
			})] ValueTuple<CombatSkillItem, int> x, [TupleElementNames(new string[]
			{
				"skillCfg",
				"index"
			})] ValueTuple<CombatSkillItem, int> y)
			{
				return (int)(x.Item1.Grade - y.Item1.Grade);
			}
		}

		// Token: 0x02000C33 RID: 3123
		public struct BreakoutCombatSkillContext
		{
			// Token: 0x06008E66 RID: 36454 RVA: 0x004FD860 File Offset: 0x004FBA60
			public unsafe BreakoutCombatSkillContext(IRandomSource random, Character character)
			{
				this.Random = random;
				this.Character = character;
				this.Qualifications = *character.GetCombatSkillQualifications();
				this.IsCreatedWithFixedTemplate = (character.GetCreatingType() != 1);
				this.CharExp = character.GetExp();
				this.ExpPerMonth = character.GetExpPerMonth();
				this.Injuries = character.GetInjuries();
				this.DisorderOfQi = character.GetDisorderOfQi();
				this.BehaviorType = character.GetBehaviorType();
			}

			// Token: 0x04003541 RID: 13633
			public IRandomSource Random;

			// Token: 0x04003542 RID: 13634
			public Character Character;

			// Token: 0x04003543 RID: 13635
			public CombatSkillShorts Qualifications;

			// Token: 0x04003544 RID: 13636
			public bool IsCreatedWithFixedTemplate;

			// Token: 0x04003545 RID: 13637
			public sbyte BehaviorType;

			// Token: 0x04003546 RID: 13638
			public int CharExp;

			// Token: 0x04003547 RID: 13639
			public int ExpPerMonth;

			// Token: 0x04003548 RID: 13640
			public Injuries Injuries;

			// Token: 0x04003549 RID: 13641
			public short DisorderOfQi;
		}

		// Token: 0x02000C34 RID: 3124
		private struct BreakPlateBonusContext
		{
			// Token: 0x06008E67 RID: 36455 RVA: 0x004FD8DB File Offset: 0x004FBADB
			public BreakPlateBonusContext(IRandomSource random, Character character)
			{
				this.Random = random;
				this.Character = character;
				this.CharCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(character.GetId());
				this.UsedRelatedCharIds = null;
			}

			// Token: 0x0400354A RID: 13642
			public readonly IRandomSource Random;

			// Token: 0x0400354B RID: 13643
			public readonly Character Character;

			// Token: 0x0400354C RID: 13644
			public readonly Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> CharCombatSkills;

			// Token: 0x0400354D RID: 13645
			public HashSet<int> UsedRelatedCharIds;
		}
	}
}
