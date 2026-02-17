using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Config;
using Config.ConfigCells.Character;
using GameData.ArchiveData;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Dependencies;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.Item;
using GameData.Domains.Taiwu;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.CombatSkill
{
	// Token: 0x02000800 RID: 2048
	[SerializableGameData(NotForDisplayModule = true)]
	public class CombatSkill : BaseGameDataObject, ISerializableGameData
	{
		// Token: 0x06006ACE RID: 27342 RVA: 0x003BD6CC File Offset: 0x003BB8CC
		[Obsolete("This method is obsolete, and will be removed in future.")]
		public void ChangePracticeLevel(DataContext context, int delta)
		{
		}

		// Token: 0x06006ACF RID: 27343 RVA: 0x003BD6D0 File Offset: 0x003BB8D0
		[Obsolete("This method is obsolete, and will be removed in future. Use Character.GetBreakoutAvailableStepsCount instead.")]
		public sbyte GetBreakoutAvailableStepsCount(GameData.Domains.Character.Character character)
		{
			return character.GetSkillBreakoutAvailableStepsCount(this._id.SkillTemplateId);
		}

		// Token: 0x06006AD0 RID: 27344 RVA: 0x003BD6F4 File Offset: 0x003BB8F4
		[ObjectCollectionDependency(7, 0, new ushort[]
		{
			4,
			13,
			3,
			27
		}, Scope = InfluenceScope.Self)]
		[ObjectCollectionDependency(4, 0, new ushort[]
		{
			5,
			57,
			17,
			59
		}, Scope = InfluenceScope.CombatSkillsOfTheChar)]
		[ObjectCollectionDependency(17, 2, new ushort[]
		{
			199,
			292
		}, Scope = InfluenceScope.CombatSkillsOfTheCharacterAffectedByTheSpecialEffects)]
		[SingleValueDependency(8, new ushort[]
		{
			19
		}, Scope = InfluenceScope.CombatSkillsOfAllCharsInCombat)]
		[ObjectCollectionDependency(8, 10, new ushort[]
		{
			3,
			105
		}, Scope = InfluenceScope.CombatSkillsOfTheCombatChar)]
		[SingleValueCollectionDependency(8, new ushort[]
		{
			6,
			7
		}, Scope = InfluenceScope.CombatSkillsAffectedByPowerChangeInCombat)]
		[SingleValueCollectionDependency(8, new ushort[]
		{
			8
		}, Scope = InfluenceScope.CombatSkillsAffectedByPowerReplaceInCombat)]
		[ObjectCollectionDependency(8, 29, new ushort[]
		{
			6
		}, Scope = InfluenceScope.CombatSkillsAffectedByCombatSkillDataInCombat)]
		[SingleValueDependency(1, new ushort[]
		{
			27
		}, Scope = InfluenceScope.All)]
		private short CalcPower()
		{
			CombatSkillData combatSkillData;
			bool flag = DomainManager.Combat.IsInCombat() && DomainManager.Combat.TryGetElement_SkillDataDict(this._id, out combatSkillData) && combatSkillData.GetSilencing();
			short result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = DomainManager.Combat.IsInCombat() && DomainManager.Combat.GetAllSkillPowerReplaceInCombat().ContainsKey(this._id);
				if (flag2)
				{
					result = DomainManager.CombatSkill.GetElement_CombatSkills(DomainManager.Combat.GetAllSkillPowerReplaceInCombat()[this._id]).GetPower();
				}
				else
				{
					int power = this.CalcBasePower();
					CValueModify modify = this.CalcBasePowerModify();
					modify += this.CalcEffectPowerModify();
					power *= modify;
					power = Math.Max(power, 0);
					power = DomainManager.SpecialEffect.ModifyData(this._id.CharId, this._id.SkillTemplateId, 199, power, -1, -1, -1);
					result = (short)Math.Clamp(power, 10, (int)GlobalConfig.Instance.CombatSkillMaxPower);
				}
			}
			return result;
		}

		// Token: 0x06006AD1 RID: 27345 RVA: 0x003BD7F8 File Offset: 0x003BB9F8
		[ObjectCollectionDependency(7, 0, new ushort[]
		{
			11,
			12
		}, Scope = InfluenceScope.Self)]
		[ObjectCollectionDependency(4, 0, new ushort[]
		{
			80,
			97,
			98,
			99,
			100
		}, Scope = InfluenceScope.CombatSkillsOfTheChar)]
		private short CalcRequirementsPower()
		{
			int power = 0;
			short maxPower = this.GetMaxPower();
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(this._id.CharId);
			List<ValueTuple<int, int, int>> requirements = this.GetRequirementsAndActualValues(character, true);
			int requirementsCount = requirements.Count - 1;
			List<ValueTuple<int, int, int>> list = requirements;
			ValueTuple<int, int, int> proficiency = list[list.Count - 1];
			int proficiencyPower = Math.Min(proficiency.Item3 * 100 / proficiency.Item2, (int)maxPower);
			bool flag = requirementsCount > 0;
			if (flag)
			{
				for (int i = 0; i < requirementsCount; i++)
				{
					ValueTuple<int, int, int> valueTuple = requirements[i];
					int required = valueTuple.Item2;
					int actual = valueTuple.Item3;
					int requirementPower = (required > 0) ? Math.Min(actual * 100 / required, (int)maxPower) : 0;
					bool flag2 = requirementPower >= proficiencyPower;
					if (flag2)
					{
						power += requirementPower;
					}
					else
					{
						power += proficiencyPower;
						proficiencyPower = requirementPower;
					}
				}
				power /= requirementsCount;
			}
			else
			{
				power = 100;
			}
			return (short)Math.Clamp(power, 0, 32767);
		}

		// Token: 0x06006AD2 RID: 27346 RVA: 0x003BD8F8 File Offset: 0x003BBAF8
		[ObjectCollectionDependency(7, 0, new ushort[]
		{
			3
		}, Scope = InfluenceScope.Self)]
		[ObjectCollectionDependency(4, 0, new ushort[]
		{
			112,
			117,
			57
		}, Scope = InfluenceScope.CombatSkillsOfTheChar)]
		[SingleValueDependency(8, new ushort[]
		{
			19
		}, Scope = InfluenceScope.CombatSkillsOfAllCharsInCombat)]
		[ObjectCollectionDependency(17, 2, new ushort[]
		{
			200,
			240
		}, Scope = InfluenceScope.CombatSkillsOfTheCharacterAffectedByTheSpecialEffects)]
		private short CalcMaxPower()
		{
			int maxPower = (int)GlobalConfig.Instance.CombatSkillMaxBasePower;
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(this._id.CharId);
			CombatSkillItem skillConfig = CombatSkill.Instance[this._id.SkillTemplateId];
			bool flag = DomainManager.Combat.IsCharInCombat(this._id.CharId, true) && character.IsTreasuryGuard();
			short result;
			if (flag)
			{
				result = GlobalConfig.Instance.CombatSkillMaxPower;
			}
			else
			{
				maxPower += CombatSkillDomain.FiveElementIndexesSum(this._id.CharId, skillConfig, NeiliType.Instance[character.GetNeiliType()].MaxPowerChange);
				foreach (SkillBreakPageEffectImplementItem effect in this.GetPageEffects())
				{
					maxPower += effect.AddMaxPower;
				}
				maxPower += this.GetBreakoutGridCombatSkillPropertyBonus(1);
				ref LifeSkillShorts lifeSkillAttainments = ref character.GetLifeSkillAttainments();
				foreach (SkillBreakPlateBonus bonus in this.GetBreakBonuses())
				{
					maxPower += bonus.CalcAddMaxPower(skillConfig.EquipType, ref lifeSkillAttainments);
				}
				maxPower += character.GetSkillBreakoutStepsMaxPower(this._id.SkillTemplateId);
				CombatSkillEquipment combatSkillEquipment = character.GetCombatSkillEquipment();
				bool flag2 = combatSkillEquipment.IsCombatSkillEquipped(this._id.SkillTemplateId);
				if (flag2)
				{
					foreach (short ptr in combatSkillEquipment.Neigong)
					{
						short neigongId = ptr;
						CombatSkill skill;
						bool flag3 = DomainManager.CombatSkill.TryGetElement_CombatSkills(new ValueTuple<int, short>(this._id.CharId, neigongId), out skill);
						if (flag3)
						{
							foreach (SkillBreakPlateBonus bonus2 in skill.GetBreakBonuses())
							{
								maxPower += (int)bonus2.CalcAddOtherSkillMaxPower(this.Template.EquipType);
							}
						}
					}
				}
				ItemKey[] equipment = character.GetEquipment();
				for (sbyte i = 8; i <= 10; i += 1)
				{
					ItemKey itemKey = equipment[(int)i];
					bool flag4 = !itemKey.IsValid();
					if (!flag4)
					{
						bool flag5 = itemKey.ItemType != 2;
						if (!flag5)
						{
							AccessoryItem accessoryItem = Config.Accessory.Instance[itemKey.TemplateId];
							maxPower += accessoryItem.CombatSkillAddMaxPower;
						}
					}
				}
				maxPower += DomainManager.SpecialEffect.GetModifyValue(this._id.CharId, this._id.SkillTemplateId, 200, EDataModifyType.Add, -1, -1, -1, EDataSumType.All);
				result = (short)Math.Clamp(maxPower, 0, (int)GlobalConfig.Instance.CombatSkillMaxPower);
			}
			return result;
		}

		// Token: 0x06006AD3 RID: 27347 RVA: 0x003BDBE0 File Offset: 0x003BBDE0
		[ObjectCollectionDependency(7, 0, new ushort[]
		{
			3
		}, Scope = InfluenceScope.Self)]
		[ObjectCollectionDependency(4, 0, new ushort[]
		{
			112
		}, Scope = InfluenceScope.CombatSkillsOfTheChar)]
		[ObjectCollectionDependency(17, 2, new ushort[]
		{
			202
		}, Scope = InfluenceScope.CombatSkillsOfTheCharacterAffectedByTheSpecialEffects)]
		[SingleValueCollectionDependency(19, new ushort[]
		{
			121
		}, Scope = InfluenceScope.CombatSkillsOfTaiwuChar)]
		private short CalcRequirementPercent()
		{
			int requirementPercent = 100;
			foreach (SkillBreakPageEffectImplementItem effect in this.GetPageEffects())
			{
				requirementPercent += effect.AddRequirement;
			}
			foreach (SkillBreakPlateBonus bonus in this.GetBreakBonuses())
			{
				requirementPercent += bonus.CalcReduceRequirements(this.Template.EquipType);
			}
			requirementPercent += (int)((short)DomainManager.SpecialEffect.GetModifyValue(this._id.CharId, this._id.SkillTemplateId, 202, EDataModifyType.Add, -1, -1, -1, EDataSumType.All));
			requirementPercent += this.GetBreakoutGridCombatSkillPropertyBonus(48);
			bool flag = DomainManager.Extra.IsCombatSkillMasteredByCharacter(this._id.CharId, this._id.SkillTemplateId);
			if (flag)
			{
				CombatSkillItem template = CombatSkill.Instance[this._id.SkillTemplateId];
				bool flag2 = template.GridCost == 2;
				if (flag2)
				{
					requirementPercent += 150;
				}
				else
				{
					bool flag3 = template.GridCost == 3;
					if (flag3)
					{
						requirementPercent += 100;
					}
				}
			}
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(this._id.CharId);
			CombatSkillItem skillConfig = CombatSkill.Instance[this._id.SkillTemplateId];
			ValueTuple<int, int> total = CombatSkillDomain.FiveElementIndexesTotal(this._id.CharId, skillConfig, NeiliType.Instance[character.GetNeiliType()].RequirementChange);
			requirementPercent *= total.Item1 + total.Item2;
			return (short)Math.Max(requirementPercent, 10);
		}

		// Token: 0x06006AD4 RID: 27348 RVA: 0x003BDDB0 File Offset: 0x003BBFB0
		[ObjectCollectionDependency(7, 0, new ushort[]
		{
			3
		}, Scope = InfluenceScope.Self)]
		[ObjectCollectionDependency(17, 2, new ushort[]
		{
			209
		}, Scope = InfluenceScope.CombatSkillsOfTheCharacterAffectedByTheSpecialEffects)]
		private sbyte CalcDirection()
		{
			sbyte direction = CombatSkillStateHelper.GetCombatSkillDirection(this._activationState);
			bool canChangeDirection = DomainManager.SpecialEffect.ModifyData(this._id.CharId, this._id.SkillTemplateId, 210, true, -1, -1, -1);
			bool flag = canChangeDirection;
			if (flag)
			{
				direction = (sbyte)DomainManager.SpecialEffect.ModifyData(this._id.CharId, this._id.SkillTemplateId, 209, (int)direction, -1, -1, -1);
			}
			bool flag2 = this._direction != -2 && direction != this._direction;
			if (flag2)
			{
				CombatSkillItem configData = CombatSkill.Instance[this._id.SkillTemplateId];
				int effectId = (direction == 0) ? configData.DirectEffectID : configData.ReverseEffectID;
				bool flag3 = SpecialEffect.Instance[effectId].EffectActiveType == 3;
				if (flag3)
				{
					bool flag4 = this._id.CharId == DomainManager.Taiwu.GetTaiwuCharId();
					if (flag4)
					{
						DataContext context = DataContextManager.GetCurrentThreadDataContext();
						bool flag5 = this._specialEffectId >= 0L;
						if (flag5)
						{
							DomainManager.SpecialEffect.Remove(context, this._specialEffectId);
						}
						bool flag6 = direction >= 0;
						if (flag6)
						{
							DomainManager.SpecialEffect.Add(context, this._id.CharId, this._id.SkillTemplateId, 3, direction);
						}
					}
					else
					{
						DomainManager.SpecialEffect.AddBrokenEffectChangedDuringAdvance(this._specialEffectId, this._id.CharId, this._id.SkillTemplateId);
					}
				}
			}
			return direction;
		}

		// Token: 0x06006AD5 RID: 27349 RVA: 0x003BDF44 File Offset: 0x003BC144
		[ObjectCollectionDependency(7, 0, new ushort[]
		{
			1,
			10,
			3
		}, Scope = InfluenceScope.Self)]
		private short CalcBaseScore()
		{
			sbyte grade = CombatSkill.Instance[this._id.SkillTemplateId].Grade;
			int value = (int)(CombatSkill.BaseScoreOfGrades[(int)grade] * this.GetPower() / 100);
			bool flag = CombatSkillStateHelper.IsBrokenOut(this._activationState);
			if (flag)
			{
				value = value * 3 / 2;
			}
			return (short)value;
		}

		// Token: 0x06006AD6 RID: 27350 RVA: 0x003BDF9C File Offset: 0x003BC19C
		[ObjectCollectionDependency(7, 0, new ushort[]
		{
			6
		}, Condition = InfluenceCondition.CombatSkillIsProactive, Scope = InfluenceScope.Self)]
		[ObjectCollectionDependency(4, 0, new ushort[]
		{
			92
		}, Scope = InfluenceScope.CombatSkillsOfTheChar)]
		[ObjectCollectionDependency(17, 2, new ushort[]
		{
			203
		}, Scope = InfluenceScope.CombatSkillsOfTheCharacterAffectedByTheSpecialEffects)]
		[SingleValueCollectionDependency(19, new ushort[]
		{
			121
		}, Scope = InfluenceScope.CombatSkillsOfTaiwuChar)]
		private sbyte CalcCurrInnerRatio()
		{
			int baseRatio = (int)this.GetBaseInnerRatio();
			int charInnerRatio = (int)DomainManager.Character.GetElement_Objects(this._id.CharId).GetInnerRatio();
			int changeRange = (int)this.GetInnerRatioChangeRange() * charInnerRatio / 100;
			int min = Math.Max(baseRatio - changeRange, 0);
			int max = Math.Min(baseRatio + changeRange, 100);
			int currInnerRatio = Math.Clamp((int)this.GetInnerRatio(), min, max);
			currInnerRatio += DomainManager.SpecialEffect.GetModifyValue(this._id.CharId, this._id.SkillTemplateId, 203, EDataModifyType.Add, -1, -1, -1, EDataSumType.All);
			currInnerRatio = Math.Clamp(currInnerRatio, 0, 100);
			return (sbyte)currInnerRatio;
		}

		// Token: 0x06006AD7 RID: 27351 RVA: 0x003BE044 File Offset: 0x003BC244
		[ObjectCollectionDependency(7, 0, new ushort[]
		{
			1,
			10
		}, Scope = InfluenceScope.Self)]
		[ObjectCollectionDependency(17, 2, new ushort[]
		{
			224
		}, Scope = InfluenceScope.CombatSkillsOfTheCharacterAffectedByTheSpecialEffects)]
		[SingleValueCollectionDependency(19, new ushort[]
		{
			121
		}, Scope = InfluenceScope.CombatSkillsOfTaiwuChar)]
		private unsafe HitOrAvoidInts CalcHitValue()
		{
			bool isMindHit = CombatSkillTemplateHelper.IsMindHitSkill(this._id.SkillTemplateId);
			CValuePercent power = (int)this.GetPower();
			CValuePercent totalHit = (int)this.GetTotalHit();
			HitOrAvoidInts hitValue = default(HitOrAvoidInts);
			HitOrAvoidInts hitDistribution = this.GetHitDistribution();
			for (int i = 0; i < 3; i++)
			{
				*(ref hitValue.Items.FixedElementField + (IntPtr)i * 4) = ((!isMindHit) ? (*(ref hitDistribution.Items.FixedElementField + (IntPtr)i * 4) * totalHit * power) : 0);
			}
			*(ref hitValue.Items.FixedElementField + (IntPtr)3 * 4) = ((!isMindHit) ? 0 : ((int)totalHit * power));
			return hitValue;
		}

		// Token: 0x06006AD8 RID: 27352 RVA: 0x003BE104 File Offset: 0x003BC304
		[ObjectCollectionDependency(7, 0, new ushort[]
		{
			1,
			10,
			15,
			3
		}, Scope = InfluenceScope.Self)]
		[SingleValueCollectionDependency(19, new ushort[]
		{
			121
		}, Scope = InfluenceScope.CombatSkillsOfTaiwuChar)]
		private OuterAndInnerInts CalcPenetrations()
		{
			CombatSkillItem skillConfig = CombatSkill.Instance[this._id.SkillTemplateId];
			int totalPenetrate = (int)skillConfig.Penetrate;
			totalPenetrate += this.GetBreakoutGridCombatSkillPropertyBonus(72);
			CValuePercentBonus bonus = this.GetBreakoutGridCombatSkillPropertyBonus(29);
			CValuePercent power = (int)this.GetPower();
			totalPenetrate = totalPenetrate * bonus * power;
			OuterAndInnerInts penetrations;
			penetrations.Inner = totalPenetrate * (int)this.GetCurrInnerRatio() / 100;
			penetrations.Outer = totalPenetrate - penetrations.Inner;
			return penetrations;
		}

		// Token: 0x06006AD9 RID: 27353 RVA: 0x003BE18C File Offset: 0x003BC38C
		[ObjectCollectionDependency(7, 0, new ushort[]
		{
			3
		}, Scope = InfluenceScope.Self)]
		[SingleValueCollectionDependency(19, new ushort[]
		{
			121
		}, Scope = InfluenceScope.CombatSkillsOfTaiwuChar)]
		private sbyte CalcCostBreathAndStancePercent()
		{
			int totalCost = (int)CombatSkill.Instance[this._id.SkillTemplateId].BreathStanceTotalCost;
			totalCost += this.GetBreakoutGridCombatSkillPropertyBonus(3);
			CValuePercentBonus bonus = 0;
			foreach (SkillBreakPageEffectImplementItem effect in this.GetPageEffects())
			{
				bonus += effect.CostBreathAndStance;
			}
			totalCost *= bonus;
			return (sbyte)Math.Clamp(totalCost, 0, 100);
		}

		// Token: 0x06006ADA RID: 27354 RVA: 0x003BE22C File Offset: 0x003BC42C
		[ObjectCollectionDependency(7, 0, new ushort[]
		{
			18,
			15
		}, Scope = InfluenceScope.Self)]
		[ObjectCollectionDependency(17, 2, new ushort[]
		{
			205,
			204
		}, Scope = InfluenceScope.CombatSkillsOfTheCharacterAffectedByTheSpecialEffects)]
		private sbyte CalcCostBreathPercent()
		{
			sbyte costBreathAndStancePercent = this.GetCostBreathAndStancePercent();
			int costBreath = (int)(costBreathAndStancePercent * this.GetCurrInnerRatio() / 100);
			int percent = 100 + DomainManager.SpecialEffect.GetModifyValue(this._id.CharId, this._id.SkillTemplateId, 205, EDataModifyType.AddPercent, -1, -1, -1, EDataSumType.All);
			percent += DomainManager.SpecialEffect.GetModifyValue(this._id.CharId, this._id.SkillTemplateId, 204, EDataModifyType.AddPercent, -1, -1, -1, EDataSumType.All);
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(this._id.CharId);
			ref LifeSkillShorts lifeSkillAttainments = ref character.GetLifeSkillAttainments();
			foreach (SkillBreakPlateBonus bonus in this.GetBreakBonuses())
			{
				percent -= bonus.CalcReduceCostBreath(this.Template.EquipType, ref lifeSkillAttainments);
			}
			costBreath = costBreath * percent / 100;
			ValueTuple<int, int> totalPercent = DomainManager.SpecialEffect.GetTotalPercentModifyValue(this._id.CharId, this._id.SkillTemplateId, 205, -1, -1, -1);
			ValueTuple<int, int> totalPercentAll = DomainManager.SpecialEffect.GetTotalPercentModifyValue(this._id.CharId, this._id.SkillTemplateId, 204, -1, -1, -1);
			totalPercent.Item1 = Math.Max(totalPercent.Item1, totalPercentAll.Item1);
			totalPercent.Item2 = Math.Min(totalPercent.Item2, totalPercentAll.Item2);
			percent = Math.Max(100 + totalPercent.Item1 + totalPercent.Item2, 0);
			costBreath = costBreath * percent / 100;
			costBreath = DomainManager.SpecialEffect.ModifyData(this._id.CharId, this._id.SkillTemplateId, 205, costBreath, -1, -1, -1);
			return (sbyte)Math.Clamp(costBreath, 0, 100);
		}

		// Token: 0x06006ADB RID: 27355 RVA: 0x003BE408 File Offset: 0x003BC608
		[ObjectCollectionDependency(7, 0, new ushort[]
		{
			18,
			15
		}, Scope = InfluenceScope.Self)]
		[ObjectCollectionDependency(17, 2, new ushort[]
		{
			206,
			204
		}, Scope = InfluenceScope.CombatSkillsOfTheCharacterAffectedByTheSpecialEffects)]
		private sbyte CalcCostStancePercent()
		{
			sbyte costBreathAndStancePercent = this.GetCostBreathAndStancePercent();
			int costStance = (int)(costBreathAndStancePercent - costBreathAndStancePercent * this.GetCurrInnerRatio() / 100);
			int percent = 100 + DomainManager.SpecialEffect.GetModifyValue(this._id.CharId, this._id.SkillTemplateId, 206, EDataModifyType.AddPercent, -1, -1, -1, EDataSumType.All);
			percent += DomainManager.SpecialEffect.GetModifyValue(this._id.CharId, this._id.SkillTemplateId, 204, EDataModifyType.AddPercent, -1, -1, -1, EDataSumType.All);
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(this._id.CharId);
			ref LifeSkillShorts lifeSkillAttainments = ref character.GetLifeSkillAttainments();
			foreach (SkillBreakPlateBonus bonus in this.GetBreakBonuses())
			{
				percent -= bonus.CalcReduceCostStance(this.Template.EquipType, ref lifeSkillAttainments);
			}
			costStance = costStance * percent / 100;
			ValueTuple<int, int> totalPercent = DomainManager.SpecialEffect.GetTotalPercentModifyValue(this._id.CharId, this._id.SkillTemplateId, 206, -1, -1, -1);
			ValueTuple<int, int> totalPercentAll = DomainManager.SpecialEffect.GetTotalPercentModifyValue(this._id.CharId, this._id.SkillTemplateId, 204, -1, -1, -1);
			totalPercent.Item1 = Math.Max(totalPercent.Item1, totalPercentAll.Item1);
			totalPercent.Item2 = Math.Min(totalPercent.Item2, totalPercentAll.Item2);
			percent = Math.Max(100 + totalPercent.Item1 + totalPercent.Item2, 0);
			costStance = costStance * percent / 100;
			costStance = DomainManager.SpecialEffect.ModifyData(this._id.CharId, this._id.SkillTemplateId, 206, costStance, -1, -1, -1);
			return (sbyte)Math.Clamp(costStance, 0, 100);
		}

		// Token: 0x06006ADC RID: 27356 RVA: 0x003BE5E8 File Offset: 0x003BC7E8
		[ObjectCollectionDependency(7, 0, new ushort[]
		{
			3
		}, Scope = InfluenceScope.Self)]
		[ObjectCollectionDependency(17, 2, new ushort[]
		{
			207
		}, Scope = InfluenceScope.CombatSkillsOfTheCharacterAffectedByTheSpecialEffects)]
		[SingleValueCollectionDependency(19, new ushort[]
		{
			121
		}, Scope = InfluenceScope.CombatSkillsOfTaiwuChar)]
		private sbyte CalcCostMobilityPercent()
		{
			int costMobility = (int)CombatSkill.Instance[this._id.SkillTemplateId].MobilityCost;
			costMobility = Math.Max(costMobility + this.GetBreakoutGridCombatSkillPropertyBonus(10), 0);
			int bonus = 0;
			bool flag = this.Template.EquipType == 2;
			if (flag)
			{
				foreach (SkillBreakPlateBonus breakBonus in this.GetBreakBonuses())
				{
					bonus += breakBonus.CalcCostMobilityByCast();
				}
			}
			costMobility = DomainManager.SpecialEffect.ModifyValueCustom(this._id.CharId, this._id.SkillTemplateId, 207, costMobility, -1, -1, -1, 0, bonus, 0, 0);
			return (sbyte)Math.Clamp(costMobility, 0, 100);
		}

		// Token: 0x06006ADD RID: 27357 RVA: 0x003BE6BC File Offset: 0x003BC8BC
		[ObjectCollectionDependency(7, 0, new ushort[]
		{
			1,
			10,
			3
		}, Scope = InfluenceScope.Self)]
		[SingleValueCollectionDependency(19, new ushort[]
		{
			121
		}, Scope = InfluenceScope.CombatSkillsOfTaiwuChar)]
		private HitOrAvoidInts CalcAddHitValueOnCast()
		{
			return CombatSkillDomain.CalcAddHitValueOnCast(this, (int)this.GetPower());
		}

		// Token: 0x06006ADE RID: 27358 RVA: 0x003BE6E0 File Offset: 0x003BC8E0
		[ObjectCollectionDependency(7, 0, new ushort[]
		{
			1,
			10
		}, Scope = InfluenceScope.Self)]
		private OuterAndInnerInts CalcAddPenetrateResist()
		{
			return CombatSkillDomain.CalcAddPenetrateResist(this, (int)this.GetPower());
		}

		// Token: 0x06006ADF RID: 27359 RVA: 0x003BE704 File Offset: 0x003BC904
		[ObjectCollectionDependency(7, 0, new ushort[]
		{
			1,
			10
		}, Scope = InfluenceScope.Self)]
		[SingleValueCollectionDependency(19, new ushort[]
		{
			121
		}, Scope = InfluenceScope.CombatSkillsOfTaiwuChar)]
		private HitOrAvoidInts CalcAddAvoidValueOnCast()
		{
			return CombatSkillDomain.CalcAddAvoidValueOnCast(this, (int)this.GetPower());
		}

		// Token: 0x06006AE0 RID: 27360 RVA: 0x003BE728 File Offset: 0x003BC928
		[ObjectCollectionDependency(7, 0, new ushort[]
		{
			1,
			10,
			3
		}, Scope = InfluenceScope.Self)]
		private int CalcFightBackPower()
		{
			return CombatSkillDomain.CalcFightBackPower(this, (int)this.GetPower());
		}

		// Token: 0x06006AE1 RID: 27361 RVA: 0x003BE74C File Offset: 0x003BC94C
		[ObjectCollectionDependency(7, 0, new ushort[]
		{
			1,
			10
		}, Scope = InfluenceScope.Self)]
		private OuterAndInnerInts CalcBouncePower()
		{
			return CombatSkillDomain.CalcBouncePower(this, (int)this.GetPower());
		}

		// Token: 0x06006AE2 RID: 27362 RVA: 0x003BE770 File Offset: 0x003BC970
		[SingleValueCollectionDependency(19, new ushort[]
		{
			277
		}, Scope = InfluenceScope.CombatSkillsOfTaiwuChar)]
		private int CalcPlateAddMaxPower()
		{
			GameData.Domains.Taiwu.SkillBreakPlate plate;
			bool flag = DomainManager.Extra.TryGetElement_SkillBreakPlates(this._id.SkillTemplateId, out plate);
			int result;
			if (flag)
			{
				result = plate.AddMaxPower;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06006AE3 RID: 27363 RVA: 0x003BE7A7 File Offset: 0x003BC9A7
		public CombatSkillItem Template
		{
			get
			{
				return CombatSkill.Instance[this._id.SkillTemplateId];
			}
		}

		// Token: 0x06006AE4 RID: 27364 RVA: 0x003BE7C0 File Offset: 0x003BC9C0
		private int CalcBasePower()
		{
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(this._id.CharId);
			int fixPower = (int)character.GetFixCombatSkillPower();
			bool flag = fixPower >= 0;
			int result;
			if (flag)
			{
				result = fixPower;
			}
			else
			{
				int power = (int)this.GetRequirementsPower();
				foreach (SkillBreakPageEffectImplementItem effect in this.GetPageEffects())
				{
					power += effect.AddPower;
				}
				power += this.GetBreakoutGridCombatSkillPropertyBonus(0);
				foreach (SkillBreakPlateBonus bonus in this.GetBreakBonuses())
				{
					power += bonus.CalcAddPower(this.Template.EquipType);
				}
				List<short> featuresIds = character.GetFeatureIds();
				for (int i = 0; i < featuresIds.Count; i++)
				{
					short featureId = featuresIds[i];
					CharacterFeatureItem featureCfg = CharacterFeature.Instance[featureId];
					power += (int)featureCfg.CombatSkillPowerBonuses[(int)this.Template.EquipType];
					bool flag2 = this.Template.FiveElements != 5;
					if (flag2)
					{
						power += (int)featureCfg.FiveElementPowerBonuses[(int)this.Template.FiveElements];
					}
				}
				result = Math.Max(power, 0);
			}
			return result;
		}

		// Token: 0x06006AE5 RID: 27365 RVA: 0x003BE93C File Offset: 0x003BCB3C
		private CValueModify CalcBasePowerModify()
		{
			int add = 0;
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(this._id.CharId);
			foreach (SolarTermItem solarTerm in character.GetInvokedSolarTerm())
			{
				bool flag = CombatSkillDomain.FiveElementContains(this._id.CharId, this.Template, solarTerm.FiveElementsTypesOfCombatSkillBuff);
				if (flag)
				{
					add += character.GetSolarTermValue((int)GlobalConfig.Instance.SolarTermAddCombatSkillPower);
				}
			}
			ItemKey[] equipment = character.GetEquipment();
			for (int i = 8; i <= 10; i++)
			{
				ItemKey itemKey = equipment[i];
				bool flag2 = !itemKey.IsValid();
				if (!flag2)
				{
					bool flag3 = itemKey.ItemType != 2;
					if (!flag3)
					{
						AccessoryItem accessoryItem = Config.Accessory.Instance[itemKey.TemplateId];
						bool flag4 = accessoryItem.BonusCombatSkillSect == this.Template.SectId;
						if (flag4)
						{
							add += (int)GlobalConfig.Instance.SectAccessoryBonusCombatSkillPower;
						}
					}
				}
			}
			int addPercent = this._id.GetNeiliAllocationPowerAddPercent();
			bool flag5 = DomainManager.Combat.IsInCombat() && this.Template.Type == 2;
			if (flag5)
			{
				short templateId = DomainManager.Combat.CombatConfig.TemplateId;
				bool flag6 = templateId - 159 <= 1;
				bool flag7 = flag6;
				if (flag7)
				{
					addPercent += 200;
				}
			}
			return new CValueModify(add, addPercent, default(CValuePercentBonus), default(CValuePercentBonus));
		}

		// Token: 0x06006AE6 RID: 27366 RVA: 0x003BEAF8 File Offset: 0x003BCCF8
		private CValueModify CalcEffectPowerModify()
		{
			CombatSkill.<>c__DisplayClass55_0 CS$<>8__locals1;
			CS$<>8__locals1.effectAdd = DomainManager.SpecialEffect.GetModify(this._id.CharId, this._id.SkillTemplateId, 199, -1, -1, -1, EDataSumType.OnlyAdd);
			CS$<>8__locals1.effectReduce = DomainManager.SpecialEffect.GetModify(this._id.CharId, this._id.SkillTemplateId, 199, -1, -1, -1, EDataSumType.OnlyReduce);
			CS$<>8__locals1.effectAdd = CS$<>8__locals1.effectAdd.ChangeA(DomainManager.SpecialEffect.GetModify(this._id.CharId, this._id.SkillTemplateId, 257, -1, -1, -1, EDataSumType.All));
			CS$<>8__locals1.effectReduce = CS$<>8__locals1.effectReduce.ChangeA(DomainManager.SpecialEffect.GetModify(this._id.CharId, this._id.SkillTemplateId, 258, -1, -1, -1, EDataSumType.All));
			CS$<>8__locals1.reverseType = this.CalcPowerEffectReverseType();
			CS$<>8__locals1.canReduce = DomainManager.SpecialEffect.ModifyData(this._id.CharId, this._id.SkillTemplateId, 201, true, -1, -1, -1);
			bool flag = !DomainManager.Combat.IsInCombat();
			CValueModify result;
			if (flag)
			{
				result = CombatSkill.<CalcEffectPowerModify>g__CalcFinalModify|55_0(ref CS$<>8__locals1);
			}
			else
			{
				CombatCharacter combatChar;
				bool flag2 = DomainManager.Combat.TryGetElement_CombatCharacterDict(this._id.CharId, out combatChar);
				if (flag2)
				{
					ETeammateCommandImplement implement = combatChar.ExecutingTeammateCommandImplement;
					bool flag3 = implement == ETeammateCommandImplement.Defend || implement == ETeammateCommandImplement.AttackSkill;
					bool flag4 = flag3;
					if (flag4)
					{
						CombatCharacter mainChar = DomainManager.Combat.GetMainCharacter(combatChar.IsAlly);
						CValuePercentBonus teammateBonus = DomainManager.SpecialEffect.GetModifyValue(mainChar.GetId(), 184, EDataModifyType.Add, (int)implement, -1, -1, EDataSumType.All);
						CS$<>8__locals1.effectAdd = CS$<>8__locals1.effectAdd.ChangeA(combatChar.ExecutingTeammateCommandConfig.IntArg * teammateBonus);
					}
				}
				SkillPowerChangeCollection powerChangeCollection;
				bool flag5 = DomainManager.Combat.TryGetElement_SkillPowerAddInCombat(this._id, out powerChangeCollection);
				if (flag5)
				{
					CS$<>8__locals1.effectAdd = CS$<>8__locals1.effectAdd.ChangeA(powerChangeCollection.GetTotalChangeValue());
				}
				bool flag6 = DomainManager.Combat.TryGetElement_SkillPowerReduceInCombat(this._id, out powerChangeCollection);
				if (flag6)
				{
					CS$<>8__locals1.effectReduce = CS$<>8__locals1.effectReduce.ChangeA(powerChangeCollection.GetTotalChangeValue());
				}
				result = CombatSkill.<CalcEffectPowerModify>g__CalcFinalModify|55_0(ref CS$<>8__locals1);
			}
			return result;
		}

		// Token: 0x06006AE7 RID: 27367 RVA: 0x003BED50 File Offset: 0x003BCF50
		private EDataReverseType CalcPowerEffectReverseType()
		{
			int effectReverseStatus = DomainManager.SpecialEffect.ModifyValue(this._id.CharId, this._id.SkillTemplateId, 292, 0, -1, -1, -1, 0, 0, 0, 0);
			if (!true)
			{
			}
			EDataReverseType result;
			if (effectReverseStatus >= 0)
			{
				if (effectReverseStatus <= 0)
				{
					result = EDataReverseType.None;
				}
				else
				{
					result = EDataReverseType.ReduceToAdd;
				}
			}
			else
			{
				result = EDataReverseType.AddToReduce;
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06006AE8 RID: 27368 RVA: 0x003BEDB0 File Offset: 0x003BCFB0
		public int GetCharPropertyBonus(ECharacterPropertyReferencedType propertyType)
		{
			CombatSkill.<>c__DisplayClass57_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.propertyId = (short)propertyType;
			CS$<>8__locals1.bonus = 0;
			bool isTaiwu = this._id.CharId == DomainManager.Taiwu.GetTaiwuCharId();
			bool flag = CombatSkillDomain.EquipAddPropertyDict[(int)this._id.SkillTemplateId] != null;
			if (flag)
			{
				int value = (int)CombatSkillDomain.EquipAddPropertyDict[(int)this._id.SkillTemplateId][(int)CS$<>8__locals1.propertyId];
				this.<GetCharPropertyBonus>g__ApplyBonus|57_0(value, ref CS$<>8__locals1);
			}
			foreach (SkillBreakPageEffectImplementItem effect in this.GetPageEffects())
			{
				int mappingValue = effect.GetMapping(propertyType);
				int value2 = this.CalcPageEffectValue(mappingValue);
				this.<GetCharPropertyBonus>g__ApplyBonus|57_0(value2, ref CS$<>8__locals1);
			}
			foreach (SkillBreakPlateBonus breakBonus in this.GetBreakBonuses())
			{
				int value3 = breakBonus.CalcEquipAddProperty(this.Template.EquipType, propertyType);
				this.<GetCharPropertyBonus>g__ApplyBonus|57_0(value3, ref CS$<>8__locals1);
			}
			bool flag2 = isTaiwu && CombatSkillStateHelper.IsBrokenOut(this._activationState);
			if (flag2)
			{
				SkillBreakBonusCollection bonusCollection = DomainManager.Taiwu.GetBreakGridBonusCollection(this._id.SkillTemplateId);
				this.<GetCharPropertyBonus>g__ApplyBonusCollection|57_1(bonusCollection, ref CS$<>8__locals1);
			}
			SkillBreakBonusCollection extraBonusCollection;
			bool flag3 = isTaiwu && DomainManager.Extra.TryGetEmeiExtraBonusCollection(this._id.SkillTemplateId, out extraBonusCollection);
			if (flag3)
			{
				this.<GetCharPropertyBonus>g__ApplyBonusCollection|57_1(extraBonusCollection, ref CS$<>8__locals1);
			}
			return CS$<>8__locals1.bonus;
		}

		// Token: 0x06006AE9 RID: 27369 RVA: 0x003BEF64 File Offset: 0x003BD164
		public int GetBreakoutGridCombatSkillPropertyBonus(short propertyId)
		{
			CombatSkill.<>c__DisplayClass58_0 CS$<>8__locals1;
			CS$<>8__locals1.propertyId = propertyId;
			CS$<>8__locals1.bonus = 0;
			bool isTaiwu = this._id.CharId == DomainManager.Taiwu.GetTaiwuCharId();
			bool flag = isTaiwu && CombatSkillStateHelper.IsBrokenOut(this._activationState);
			if (flag)
			{
				SkillBreakBonusCollection bonusCollection = DomainManager.Taiwu.GetBreakGridBonusCollection(this._id.SkillTemplateId);
				CombatSkill.<GetBreakoutGridCombatSkillPropertyBonus>g__ApplyBonusCollection|58_0(bonusCollection, ref CS$<>8__locals1);
			}
			SkillBreakBonusCollection extraBonusCollection;
			bool flag2 = isTaiwu && DomainManager.Extra.TryGetEmeiExtraBonusCollection(this._id.SkillTemplateId, out extraBonusCollection);
			if (flag2)
			{
				CombatSkill.<GetBreakoutGridCombatSkillPropertyBonus>g__ApplyBonusCollection|58_0(extraBonusCollection, ref CS$<>8__locals1);
			}
			return CS$<>8__locals1.bonus;
		}

		// Token: 0x06006AEA RID: 27370 RVA: 0x003BF00C File Offset: 0x003BD20C
		private int CalcPageEffectValue(int mappingValue)
		{
			return mappingValue * (int)this.Template.GridCost;
		}

		// Token: 0x06006AEB RID: 27371 RVA: 0x003BF02C File Offset: 0x003BD22C
		private int CalcCharacterPropertyBonus(int propertyId, int bonus)
		{
			CharacterPropertyReferencedItem propertyConfig = CharacterPropertyReferenced.Instance[propertyId];
			return this.CalcCharacterPropertyBonus(propertyId, bonus, (int)(propertyConfig.BoostedByPower ? this.GetPower() : 0));
		}

		// Token: 0x06006AEC RID: 27372 RVA: 0x003BF068 File Offset: 0x003BD268
		private int CalcCharacterPropertyBonus(int propertyId, int bonus, CValuePercent power)
		{
			CharacterPropertyReferencedItem propertyConfig = CharacterPropertyReferenced.Instance[propertyId];
			return (propertyConfig.BoostedByPower && bonus > 0) ? (bonus * power) : bonus;
		}

		// Token: 0x06006AED RID: 27373 RVA: 0x003BF09C File Offset: 0x003BD29C
		public int CalcNeiliAllocationBonus(ECharacterPropertyReferencedType propertyType, int stepCount)
		{
			return this.CalcNeiliAllocationBonus(propertyType, stepCount, (int)this.GetPower());
		}

		// Token: 0x06006AEE RID: 27374 RVA: 0x003BF0BC File Offset: 0x003BD2BC
		private int CalcNeiliAllocationBonus(ECharacterPropertyReferencedType propertyType, int stepCount, int power)
		{
			int valuePerStep = CombatSkill.Instance[this._id.SkillTemplateId].GetMapping(propertyType);
			bool flag = valuePerStep <= 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int percent = GlobalConfig.Instance.CombatSkillNeiliAllocationBonusPercent;
				result = valuePerStep * stepCount * power * percent / 10000;
			}
			return result;
		}

		// Token: 0x06006AEF RID: 27375 RVA: 0x003BF114 File Offset: 0x003BD314
		public List<ValueTuple<short, short, bool>> GetBreakAddPropertyList(CValuePercent power)
		{
			CombatSkill.<>c__DisplayClass64_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.power = power;
			List<ValueTuple<short, short, bool>> addPropertyList = new List<ValueTuple<short, short, bool>>();
			CS$<>8__locals1.propertyIdValues = ObjectPool<Dictionary<int, int>>.Instance.Get();
			List<int> extraPropertyIds = ObjectPool<List<int>>.Instance.Get();
			CS$<>8__locals1.propertyIdValues.Clear();
			extraPropertyIds.Clear();
			foreach (ECharacterPropertyReferencedType bonusType in GameData.Domains.Character.Character.BonusPropertyTypes)
			{
				int bonusId = (int)bonusType;
				foreach (SkillBreakPageEffectImplementItem effect in this.GetPageEffects())
				{
					int mappingValue = effect.GetMapping(bonusType);
					bool flag = mappingValue == 0;
					if (!flag)
					{
						int value = this.CalcPageEffectValue(mappingValue);
						bool flag2 = value != 0;
						if (flag2)
						{
							this.<GetBreakAddPropertyList>g__AddProperty|64_1(bonusId, this.CalcCharacterPropertyBonus(bonusId, value, CS$<>8__locals1.power), ref CS$<>8__locals1);
						}
					}
				}
				foreach (SkillBreakPlateBonus breakBonus in this.GetBreakBonuses())
				{
					int value2 = breakBonus.CalcEquipAddProperty(this.Template.EquipType, bonusType);
					bool flag3 = value2 != 0;
					if (flag3)
					{
						this.<GetBreakAddPropertyList>g__AddProperty|64_1(bonusId, this.CalcCharacterPropertyBonus(bonusId, value2, CS$<>8__locals1.power), ref CS$<>8__locals1);
					}
				}
			}
			bool flag4 = this._id.CharId == DomainManager.Taiwu.GetTaiwuCharId();
			if (flag4)
			{
				SkillBreakBonusCollection bonusCollection = DomainManager.Taiwu.GetBreakGridBonusCollection(this._id.SkillTemplateId);
				bool flag5 = bonusCollection != null && CombatSkillStateHelper.IsBrokenOut(this._activationState);
				if (flag5)
				{
					foreach (KeyValuePair<short, short> bonus in bonusCollection.CharacterPropertyBonusDict)
					{
						this.<GetBreakAddPropertyList>g__AddProperty|64_1((int)bonus.Key, this.<GetBreakAddPropertyList>g__CalcCharacterBonus|64_0(bonus, ref CS$<>8__locals1), ref CS$<>8__locals1);
					}
					foreach (KeyValuePair<short, short> bonus2 in bonusCollection.CombatSkillPropertyBonusDict)
					{
						this.<GetBreakAddPropertyList>g__AddProperty|64_1(CharacterPropertyReferenced.Instance.Count + (int)bonus2.Key, (int)bonus2.Value, ref CS$<>8__locals1);
					}
				}
				SkillBreakBonusCollection extraCollection = DomainManager.Extra.GetEmeiBreakBonusCollection(this._id.SkillTemplateId);
				bool flag6 = extraCollection != null;
				if (flag6)
				{
					foreach (KeyValuePair<short, short> bonus3 in extraCollection.CharacterPropertyBonusDict)
					{
						this.<GetBreakAddPropertyList>g__AddProperty|64_1((int)bonus3.Key, this.<GetBreakAddPropertyList>g__CalcCharacterBonus|64_0(bonus3, ref CS$<>8__locals1), ref CS$<>8__locals1);
						extraPropertyIds.Add((int)bonus3.Key);
					}
					foreach (KeyValuePair<short, short> bonus4 in extraCollection.CombatSkillPropertyBonusDict)
					{
						int key = CharacterPropertyReferenced.Instance.Count + (int)bonus4.Key;
						this.<GetBreakAddPropertyList>g__AddProperty|64_1(key, (int)bonus4.Value, ref CS$<>8__locals1);
						extraPropertyIds.Add(key);
					}
				}
			}
			foreach (KeyValuePair<int, int> propertyIdValue in CS$<>8__locals1.propertyIdValues)
			{
				addPropertyList.Add(new ValueTuple<short, short, bool>((short)propertyIdValue.Key, (short)propertyIdValue.Value, extraPropertyIds.Contains(propertyIdValue.Key)));
			}
			ObjectPool<Dictionary<int, int>>.Instance.Return(CS$<>8__locals1.propertyIdValues);
			ObjectPool<List<int>>.Instance.Return(extraPropertyIds);
			return addPropertyList;
		}

		// Token: 0x06006AF0 RID: 27376 RVA: 0x003BF5B8 File Offset: 0x003BD7B8
		public List<ValueTuple<short, int>> GetNeiliAllocationPropertyList(int power)
		{
			CombatSkillItem config = CombatSkill.Instance[this._id.SkillTemplateId];
			GameData.Domains.Character.Character character;
			return DomainManager.Character.TryGetElement_Objects(this._id.CharId, out character) ? config.CalcNeiliAllocationBonus(power, new Func<ECharacterPropertyReferencedType, int>(character.CalcNeiliAllocationStepCount)) : config.CalcDefaultNeiliAllocationBonus();
		}

		// Token: 0x06006AF1 RID: 27377 RVA: 0x003BF614 File Offset: 0x003BD814
		[return: TupleElementNames(new string[]
		{
			"type",
			"required",
			"actual"
		})]
		public List<ValueTuple<int, int, int>> GetRequirementsAndActualValues(GameData.Domains.Character.Character character, bool skillExist = true)
		{
			CombatSkillItem config = CombatSkill.Instance[this._id.SkillTemplateId];
			List<PropertyAndValue> requirements = config.UsingRequirement;
			int requirementPercent = (int)(skillExist ? this.GetRequirementPercent() : 100);
			List<ValueTuple<int, int, int>> result = new List<ValueTuple<int, int, int>>();
			CValuePercent teammatePercent = GlobalConfig.Instance.TreasuryGuardAttainmentPercent;
			int addLifeSkillAttainment = 0;
			bool flag = character != null && skillExist;
			if (flag)
			{
				sbyte equipType = config.EquipType;
				ref LifeSkillShorts lifeSkillAttainments = ref character.GetLifeSkillAttainments();
				foreach (SkillBreakPlateBonus bonus in this.GetBreakBonuses())
				{
					addLifeSkillAttainment += bonus.CalcAddLifeSkillRequirement(equipType, ref lifeSkillAttainments);
				}
			}
			for (int i = 0; i < requirements.Count; i++)
			{
				PropertyAndValue requirement = requirements[i];
				ECharacterPropertyReferencedType propertyType = (ECharacterPropertyReferencedType)requirement.PropertyId;
				int value = (character != null) ? character.GetPropertyValue(propertyType) : 0;
				bool flag2 = propertyType.IsLifeSkillTypeAttainment();
				if (flag2)
				{
					value += addLifeSkillAttainment;
				}
				bool flag3 = character != null && character.IsTreasuryGuard();
				if (flag3)
				{
					foreach (CombatCharacter teammateCharacter in DomainManager.Combat.GetTeammateCharacters(character.GetId()))
					{
						value += teammateCharacter.GetCharacter().GetPropertyValue(propertyType) * teammatePercent;
					}
				}
				result.Add(new ValueTuple<int, int, int>((int)requirement.PropertyId, (int)requirement.Value * requirementPercent / 100, value));
			}
			int v;
			int proficiency = DomainManager.Extra.TryGetElement_CombatSkillProficiencies(this._id, out v) ? v : 0;
			result.Add(new ValueTuple<int, int, int>(110, 300, proficiency));
			return result;
		}

		// Token: 0x06006AF2 RID: 27378 RVA: 0x003BF7FC File Offset: 0x003BD9FC
		public bool CanBreakout()
		{
			return CombatSkillStateHelper.HasReadOutlinePages(this._readingState) && CombatSkillStateHelper.IsReadNormalPagesMeetConditionOfBreakout(this._readingState) && !this._revoked;
		}

		// Token: 0x06006AF3 RID: 27379 RVA: 0x003BF834 File Offset: 0x003BDA34
		public void ObtainNeili(DataContext context, short obtainedNeili)
		{
			short oriObtainedNeili = this._obtainedNeili;
			short totalObtainableNeili = this.GetTotalObtainableNeili();
			bool flag = this._obtainedNeili >= totalObtainableNeili;
			if (!flag)
			{
				this._obtainedNeili += obtainedNeili;
				bool flag2 = this._obtainedNeili > totalObtainableNeili;
				if (flag2)
				{
					this._obtainedNeili = totalObtainableNeili;
				}
				bool flag3 = this._obtainedNeili != oriObtainedNeili;
				if (flag3)
				{
					this.SetObtainedNeili(this._obtainedNeili, context);
				}
			}
		}

		// Token: 0x06006AF4 RID: 27380 RVA: 0x003BF8A8 File Offset: 0x003BDAA8
		public sbyte GetBaseInnerRatio()
		{
			sbyte baseInnerRatio = CombatSkill.Instance[this._id.SkillTemplateId].BaseInnerRatio;
			return (sbyte)Math.Clamp((int)baseInnerRatio + this.GetBreakoutGridCombatSkillPropertyBonus(4), 0, 100);
		}

		// Token: 0x06006AF5 RID: 27381 RVA: 0x003BF8E8 File Offset: 0x003BDAE8
		public sbyte GetInnerRatioChangeRange()
		{
			int innerRatioChangeRange = (int)CombatSkill.Instance[this._id.SkillTemplateId].InnerRatioChangeRange;
			innerRatioChangeRange += this.GetBreakoutGridCombatSkillPropertyBonus(5);
			foreach (SkillBreakPlateBonus bonus in this.GetBreakBonuses())
			{
				innerRatioChangeRange += bonus.CalcInnerRatioChangeRange(this.Template.EquipType);
			}
			return (sbyte)Math.Clamp(innerRatioChangeRange, 0, 100);
		}

		// Token: 0x06006AF6 RID: 27382 RVA: 0x003BF97C File Offset: 0x003BDB7C
		public short GetTotalObtainableNeili()
		{
			int value = (int)CombatSkill.Instance[this._id.SkillTemplateId].TotalObtainableNeili;
			value += this.GetBreakoutGridCombatSkillPropertyBonus(6);
			foreach (SkillBreakPlateBonus bonus in this.GetBreakBonuses())
			{
				value += bonus.CalcTotalObtainableNeili();
			}
			return (short)Math.Clamp(value, 0, 32767);
		}

		// Token: 0x06006AF7 RID: 27383 RVA: 0x003BFA08 File Offset: 0x003BDC08
		public sbyte GetFiveElementsChange()
		{
			sbyte fiveElementChange = CombatSkill.Instance[this._id.SkillTemplateId].FiveElementChangePerLoop;
			return (sbyte)((int)fiveElementChange + this.GetBreakoutGridCombatSkillPropertyBonus(8));
		}

		// Token: 0x06006AF8 RID: 27384 RVA: 0x003BFA40 File Offset: 0x003BDC40
		public sbyte[] GetSpecificGridCount(bool preview = false)
		{
			sbyte[] specificGrids = new sbyte[4];
			this.GetSpecificGridCount(specificGrids, preview);
			return specificGrids;
		}

		// Token: 0x06006AF9 RID: 27385 RVA: 0x003BFA68 File Offset: 0x003BDC68
		public unsafe void GetSpecificGridCount(Span<sbyte> specificGrids, bool preview = false)
		{
			specificGrids.Fill(0);
			for (sbyte equipType = 1; equipType < 5; equipType += 1)
			{
				*specificGrids[(int)(equipType - 1)] = this.GetSpecificGridCount(equipType, preview);
			}
		}

		// Token: 0x06006AFA RID: 27386 RVA: 0x003BFAA4 File Offset: 0x003BDCA4
		public sbyte GetSpecificGridCount(sbyte equipType, bool preview = false)
		{
			int i = (int)(equipType - 1);
			sbyte specificGridCount = (sbyte)this.GetBreakoutGridCombatSkillPropertyBonus((short)(49 + i));
			bool isMastered = DomainManager.Extra.IsCombatSkillMasteredByCharacter(this._id.CharId, this._id.SkillTemplateId);
			if (preview)
			{
				isMastered = !isMastered;
			}
			bool flag = isMastered;
			sbyte result;
			if (flag)
			{
				result = specificGridCount;
			}
			else
			{
				foreach (SkillBreakPlateBonus bonus in this.GetBreakBonuses())
				{
					specificGridCount += bonus.CalcSpecificGridCount(equipType);
				}
				sbyte[] configGrids = CombatSkill.Instance[this._id.SkillTemplateId].SpecificGrids;
				specificGridCount += configGrids[i];
				specificGridCount += (sbyte)DomainManager.SpecialEffect.GetModifyValue(this._id.CharId, this._id.SkillTemplateId, 213, EDataModifyType.Add, i, -1, -1, EDataSumType.All);
				result = specificGridCount;
			}
			return result;
		}

		// Token: 0x06006AFB RID: 27387 RVA: 0x003BFBA4 File Offset: 0x003BDDA4
		public sbyte GetGenericGridCount(bool preview = false)
		{
			bool isMastered = DomainManager.Extra.IsCombatSkillMasteredByCharacter(this._id.CharId, this._id.SkillTemplateId);
			if (preview)
			{
				isMastered = !isMastered;
			}
			bool flag = isMastered;
			sbyte result;
			if (flag)
			{
				result = CombatSkill.Instance[this._id.SkillTemplateId].GridCost;
			}
			else
			{
				int genericGrid = (int)CombatSkill.Instance[this._id.SkillTemplateId].GenericGrid + this.GetBreakoutGridCombatSkillPropertyBonus(9);
				genericGrid += DomainManager.SpecialEffect.GetModifyValue(this._id.CharId, this._id.SkillTemplateId, 214, EDataModifyType.Add, -1, -1, -1, EDataSumType.All);
				genericGrid = Math.Max(genericGrid, 0);
				result = (sbyte)genericGrid;
			}
			return result;
		}

		// Token: 0x06006AFC RID: 27388 RVA: 0x003BFC64 File Offset: 0x003BDE64
		private short GetTotalHit()
		{
			int totalHit = (int)CombatSkill.Instance[this._id.SkillTemplateId].TotalHit;
			totalHit += this.GetBreakoutGridCombatSkillPropertyBonus(73);
			CValuePercentBonus bonus = this.GetBreakoutGridCombatSkillPropertyBonus(30);
			foreach (SkillBreakPageEffectImplementItem effect in this.GetPageEffects())
			{
				bonus += effect.HitFactor;
			}
			foreach (SkillBreakPlateBonus breakBonus in this.GetBreakBonuses())
			{
				bonus += breakBonus.CalcTotalHit();
			}
			return (short)(totalHit * bonus);
		}

		// Token: 0x06006AFD RID: 27389 RVA: 0x003BFD54 File Offset: 0x003BDF54
		public int GetPrepareTotalProgress()
		{
			int prepareTotalProgress = CombatSkill.Instance[this._id.SkillTemplateId].PrepareTotalProgress;
			int percent = DomainManager.SpecialEffect.GetModifyValue(this._id.CharId, this._id.SkillTemplateId, 212, EDataModifyType.AddPercent, -1, -1, -1, EDataSumType.All);
			int gridAddPercent = this.GetBreakoutGridCombatSkillPropertyBonus(2);
			foreach (SkillBreakPageEffectImplementItem effect in this.GetPageEffects())
			{
				gridAddPercent += effect.CastFrame;
			}
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(this._id.CharId);
			ref LifeSkillShorts lifeSkillAttainments = ref character.GetLifeSkillAttainments();
			foreach (SkillBreakPlateBonus bonus in this.GetBreakBonuses())
			{
				gridAddPercent -= bonus.CalcReduceCastFrame(this.Template.EquipType, ref lifeSkillAttainments);
			}
			return Math.Max(prepareTotalProgress * (100 + percent + gridAddPercent) / 100, 0);
		}

		// Token: 0x06006AFE RID: 27390 RVA: 0x003BFE84 File Offset: 0x003BE084
		public short GetDistanceAdditionWhenCast(bool forward)
		{
			int distance = (int)CombatSkill.Instance[this._id.SkillTemplateId].DistanceAdditionWhenCast;
			distance += this.GetBreakoutGridCombatSkillPropertyBonus(34);
			foreach (SkillBreakPageEffectImplementItem effect in this.GetPageEffects())
			{
				distance += (forward ? effect.AttackRangeForward : effect.AttackRangeBackward);
			}
			foreach (SkillBreakPlateBonus bonus in this.GetBreakBonuses())
			{
				distance += bonus.CalcAddAttackRange(forward);
			}
			return (short)distance;
		}

		// Token: 0x06006AFF RID: 27391 RVA: 0x003BFF54 File Offset: 0x003BE154
		public short GetContinuousFrames()
		{
			short continuousFrames = CombatSkill.Instance[this._id.SkillTemplateId].ContinuousFrames;
			CValuePercentBonus bonus = this.GetBreakoutGridCombatSkillPropertyBonus(28);
			return (short)((int)continuousFrames * bonus);
		}

		// Token: 0x06006B00 RID: 27392 RVA: 0x003BFF98 File Offset: 0x003BE198
		public short GetBounceDistance()
		{
			short bounceDistance = CombatSkill.Instance[this._id.SkillTemplateId].BounceDistance;
			return (short)((int)bounceDistance + this.GetBreakoutGridCombatSkillPropertyBonus(27));
		}

		// Token: 0x06006B01 RID: 27393 RVA: 0x003BFFD0 File Offset: 0x003BE1D0
		public unsafe HitOrAvoidInts GetHitDistribution()
		{
			sbyte[] configValue = CombatSkill.Instance[this._id.SkillTemplateId].PerHitDamageRateDistribution;
			HitOrAvoidInts hitDistribution;
			for (sbyte hitType = 0; hitType < 4; hitType += 1)
			{
				sbyte distribution = configValue[(int)hitType];
				bool flag = hitType < 3;
				if (flag)
				{
					distribution = (sbyte)((int)distribution + this.GetBreakoutGridCombatSkillPropertyBonus((short)(31 + hitType)));
				}
				*(ref hitDistribution.Items.FixedElementField + (IntPtr)hitType * 4) = (int)distribution;
			}
			int sum = 0;
			for (sbyte hitType2 = 0; hitType2 < 4; hitType2 += 1)
			{
				sum += *(ref hitDistribution.Items.FixedElementField + (IntPtr)hitType2 * 4);
			}
			bool flag2 = sum != 100;
			if (flag2)
			{
				for (sbyte hitType3 = 0; hitType3 < 4; hitType3 += 1)
				{
					*(ref hitDistribution.Items.FixedElementField + (IntPtr)hitType3 * 4) = (int)configValue[(int)hitType3];
				}
			}
			hitDistribution = DomainManager.SpecialEffect.ModifyData(this._id.CharId, this._id.SkillTemplateId, 224, hitDistribution, -1, -1, -1);
			return hitDistribution;
		}

		// Token: 0x06006B02 RID: 27394 RVA: 0x003C00E0 File Offset: 0x003BE2E0
		public IEnumerable<int> GetBodyPartWeights()
		{
			sbyte[] configRate = CombatSkill.Instance[this._id.SkillTemplateId].InjuryPartAtkRateDistribution;
			CValuePercentBonus chestBonus = this.GetBreakoutGridCombatSkillPropertyBonus(35);
			CValuePercentBonus bellyBonus = this.GetBreakoutGridCombatSkillPropertyBonus(36);
			CValuePercentBonus headBonus = this.GetBreakoutGridCombatSkillPropertyBonus(37);
			CValuePercentBonus handBonus = this.GetBreakoutGridCombatSkillPropertyBonus(38);
			CValuePercentBonus legBonus = this.GetBreakoutGridCombatSkillPropertyBonus(39);
			sbyte b;
			for (sbyte i = 0; i < 7; i = b + 1)
			{
				if (!true)
				{
				}
				CValuePercentBonus cvaluePercentBonus;
				switch (i)
				{
				case 0:
					cvaluePercentBonus = chestBonus;
					break;
				case 1:
					cvaluePercentBonus = bellyBonus;
					break;
				case 2:
					cvaluePercentBonus = headBonus;
					break;
				case 3:
				case 4:
					cvaluePercentBonus = handBonus;
					break;
				case 5:
				case 6:
					cvaluePercentBonus = legBonus;
					break;
				default:
					cvaluePercentBonus = 0;
					break;
				}
				if (!true)
				{
				}
				CValuePercentBonus bonus = cvaluePercentBonus;
				yield return (int)configRate[(int)i] * bonus;
				bonus = default(CValuePercentBonus);
				b = i;
			}
			yield break;
		}

		// Token: 0x06006B03 RID: 27395 RVA: 0x003C00F0 File Offset: 0x003BE2F0
		private int[] GetConfigAffectRequirePower()
		{
			sbyte direction = this.GetDirection();
			bool flag = direction < 0 || direction >= 2;
			bool flag2 = flag;
			int[] result;
			if (flag2)
			{
				result = null;
			}
			else
			{
				CombatSkillItem configSkill = CombatSkill.Instance[this._id.SkillTemplateId];
				SpecialEffectItem configEffect = SpecialEffect.Instance[(direction == 0) ? configSkill.DirectEffectID : configSkill.ReverseEffectID];
				result = configEffect.AffectRequirePower;
			}
			return result;
		}

		// Token: 0x06006B04 RID: 27396 RVA: 0x003C0160 File Offset: 0x003BE360
		public unsafe int GetSumMax2HitDistribution()
		{
			HitOrAvoidInts hitDistribution = this.GetHitDistribution();
			int max0 = hitDistribution.Items.FixedElementField;
			int max = *(ref hitDistribution.Items.FixedElementField + 4);
			for (int i = 2; i < 4; i++)
			{
				int hit = *(ref hitDistribution.Items.FixedElementField + (IntPtr)i * 4);
				bool flag = max0 > max;
				if (flag)
				{
					bool flag2 = hit > max;
					if (flag2)
					{
						max = hit;
					}
				}
				else
				{
					bool flag3 = hit > max0;
					if (flag3)
					{
						max0 = hit;
					}
				}
			}
			return Math.Max(max0, 0) + Math.Max(max, 0);
		}

		// Token: 0x06006B05 RID: 27397 RVA: 0x003C01FC File Offset: 0x003BE3FC
		public bool AnyAffectRequirePower()
		{
			int[] configAffectRequirePower = this.GetConfigAffectRequirePower();
			return configAffectRequirePower != null && configAffectRequirePower.Length > 0;
		}

		// Token: 0x06006B06 RID: 27398 RVA: 0x003C0221 File Offset: 0x003BE421
		public IEnumerable<int> GetAffectRequirePower()
		{
			int[] configAffectRequirePower = this.GetConfigAffectRequirePower();
			bool flag = configAffectRequirePower == null || configAffectRequirePower.Length <= 0;
			if (flag)
			{
				yield break;
			}
			int sumMax2HitDistribution = this.GetSumMax2HitDistribution();
			foreach (int requirePower in configAffectRequirePower)
			{
				yield return (requirePower >= 0) ? requirePower : sumMax2HitDistribution;
			}
			int[] array = null;
			yield break;
		}

		// Token: 0x06006B07 RID: 27399 RVA: 0x003C0234 File Offset: 0x003BE434
		public bool PowerMatchAffectRequire(int power, int index)
		{
			int i = 0;
			foreach (int requirePower in this.GetAffectRequirePower())
			{
				bool flag = i++ == index;
				if (flag)
				{
					return power >= requirePower;
				}
			}
			short predefinedLogId = 8;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(57, 2);
			defaultInterpolatedStringHandler.AppendFormatted<short>(this._id.SkillTemplateId);
			defaultInterpolatedStringHandler.AppendLiteral(" do not has require power in index=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(index);
			defaultInterpolatedStringHandler.AppendLiteral(", match is always true");
			PredefinedLog.Show(predefinedLogId, defaultInterpolatedStringHandler.ToStringAndClear());
			return true;
		}

		// Token: 0x06006B08 RID: 27400 RVA: 0x003C02EC File Offset: 0x003BE4EC
		public int GetAffectRequirePower(int index)
		{
			int i = 0;
			foreach (int requirePower in this.GetAffectRequirePower())
			{
				bool flag = i++ == index;
				if (flag)
				{
					return requirePower;
				}
			}
			short predefinedLogId = 8;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(65, 2);
			defaultInterpolatedStringHandler.AppendFormatted<short>(this._id.SkillTemplateId);
			defaultInterpolatedStringHandler.AppendLiteral(" do not has require power in index=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(index);
			defaultInterpolatedStringHandler.AppendLiteral(", require power is always zero");
			PredefinedLog.Show(predefinedLogId, defaultInterpolatedStringHandler.ToStringAndClear());
			return 0;
		}

		// Token: 0x06006B09 RID: 27401 RVA: 0x003C03A0 File Offset: 0x003BE5A0
		public unsafe PoisonsAndLevels GetPoisons()
		{
			PoisonsAndLevels poisons = CombatSkill.Instance[this._id.SkillTemplateId].Poisons;
			for (sbyte type = 0; type < 6; type += 1)
			{
				CValuePercentBonus bonus = this.GetBreakoutGridCombatSkillPropertyBonus((short)(42 + type));
				foreach (SkillBreakPlateBonus breakBonus in this.GetBreakBonuses())
				{
					bonus += breakBonus.CalcPoison(type);
				}
				*(ref poisons.Values.FixedElementField + (IntPtr)type * 2) = (short)((int)(*(ref poisons.Values.FixedElementField + (IntPtr)type * 2)) * bonus);
			}
			return poisons;
		}

		// Token: 0x06006B0A RID: 27402 RVA: 0x003C0478 File Offset: 0x003BE678
		[return: TupleElementNames(new string[]
		{
			"type",
			"value"
		})]
		public ValueTuple<sbyte, sbyte> GetCostNeiliAllocation()
		{
			return DomainManager.SpecialEffect.ModifyData(this._id.CharId, this._id.SkillTemplateId, 231, new ValueTuple<sbyte, sbyte>(-1, 0), -1, -1, -1);
		}

		// Token: 0x06006B0B RID: 27403 RVA: 0x003C04BC File Offset: 0x003BE6BC
		public SpecialEffectItem TryGetSpecialEffect()
		{
			CombatSkillItem config = CombatSkill.Instance[this._id.SkillTemplateId];
			sbyte direction = this.GetDirection();
			if (!true)
			{
			}
			SpecialEffectItem result;
			if (direction != 0)
			{
				if (direction != 1)
				{
					result = null;
				}
				else
				{
					result = SpecialEffect.Instance[config.ReverseEffectID];
				}
			}
			else
			{
				result = SpecialEffect.Instance[config.DirectEffectID];
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06006B0C RID: 27404 RVA: 0x003C0529 File Offset: 0x003BE729
		public int GetReadNormalPagesCount()
		{
			return CombatSkillStateHelper.GetReadNormalPagesCount(this._readingState);
		}

		// Token: 0x06006B0D RID: 27405 RVA: 0x003C0536 File Offset: 0x003BE736
		public IEnumerable<SkillBreakPageEffectImplementItem> GetPageEffects()
		{
			foreach (SkillBreakPageEffectItem effect in ((IEnumerable<SkillBreakPageEffectItem>)SkillBreakPageEffect.Instance))
			{
				sbyte direction = effect.IsDirect ? 0 : 1;
				bool flag = CombatSkillStateHelper.GetPageActiveDirection(this._activationState, effect.PageId) != direction;
				if (!flag)
				{
					sbyte equipType = this.Template.EquipType;
					if (!true)
					{
					}
					int num;
					switch (equipType)
					{
					case 0:
						num = (int)effect.EffectNeigong;
						break;
					case 1:
						num = (int)effect.EffectAttack;
						break;
					case 2:
						num = (int)effect.EffectAgile;
						break;
					case 3:
						num = (int)effect.EffectDefense;
						break;
					case 4:
						num = (int)effect.EffectAssist;
						break;
					default:
						num = -1;
						break;
					}
					if (!true)
					{
					}
					int implementId = num;
					bool flag2 = implementId < 0;
					if (!flag2)
					{
						yield return SkillBreakPageEffectImplement.Instance[implementId];
						effect = null;
					}
				}
			}
			IEnumerator<SkillBreakPageEffectItem> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06006B0E RID: 27406 RVA: 0x003C0548 File Offset: 0x003BE748
		public IEnumerable<SkillBreakPlateBonus> GetBreakBonuses()
		{
			bool flag = !CombatSkillStateHelper.IsBrokenOut(this._activationState);
			IEnumerable<SkillBreakPlateBonus> result2;
			if (flag)
			{
				result2 = Enumerable.Empty<SkillBreakPlateBonus>();
			}
			else
			{
				short skillId = this._id.SkillTemplateId;
				IEnumerable<SkillBreakPlateBonus> result = null;
				GameData.Domains.Character.Character character;
				bool flag2 = DomainManager.Character.TryGetElement_Objects(this._id.CharId, out character) && character.IsGearMate;
				if (flag2)
				{
					result = DomainManager.Extra.GetGearMateById(this._id.CharId).SkillBreakBonusDict.GetOrDefault(skillId);
				}
				else
				{
					bool flag3 = this._id.CharId != DomainManager.Taiwu.GetTaiwuCharId();
					if (flag3)
					{
						result = DomainManager.Extra.GetCharacterSkillBreakBonuses(this._id.CharId, skillId).Items;
					}
					else
					{
						GameData.Domains.Taiwu.SkillBreakPlate plate;
						bool flag4 = DomainManager.Taiwu.TryGetSkillBreakPlate(skillId, out plate);
						if (flag4)
						{
							result = plate.GetBonuses();
						}
					}
				}
				result2 = (result ?? Enumerable.Empty<SkillBreakPlateBonus>());
			}
			return result2;
		}

		// Token: 0x06006B0F RID: 27407 RVA: 0x003C0638 File Offset: 0x003BE838
		public int CalcInjuryDamageStep(bool inner, sbyte bodyPart)
		{
			int baseValue = (inner ? this.Template.InnerDamageSteps : this.Template.OuterDamageSteps)[(int)bodyPart];
			CValuePercentBonus percentBonus = this.CalcInjuryDamageStepBonus(inner);
			return baseValue * percentBonus;
		}

		// Token: 0x06006B10 RID: 27408 RVA: 0x003C067C File Offset: 0x003BE87C
		public int CalcFatalDamageStep()
		{
			int baseValue = this.Template.FatalDamageStep;
			CValuePercentBonus percentBonus = this.CalcFatalDamageStepBonus();
			return baseValue * percentBonus;
		}

		// Token: 0x06006B11 RID: 27409 RVA: 0x003C06B0 File Offset: 0x003BE8B0
		public int CalcMindDamageStep()
		{
			int baseValue = this.Template.MindDamageStep;
			CValuePercentBonus percentBonus = this.CalcMindDamageStepBonus();
			return baseValue * percentBonus;
		}

		// Token: 0x06006B12 RID: 27410 RVA: 0x003C06E4 File Offset: 0x003BE8E4
		public CombatSkillDamageStepBonusDisplayData CalcStepBonusDisplayData()
		{
			return new CombatSkillDamageStepBonusDisplayData
			{
				InnerInjuryStepBonus = this.CalcInjuryDamageStepBonus(true),
				OuterInjuryStepBonus = this.CalcInjuryDamageStepBonus(false),
				FatalStepBonus = this.CalcFatalDamageStepBonus(),
				MindStepBonus = this.CalcMindDamageStepBonus()
			};
		}

		// Token: 0x06006B13 RID: 27411 RVA: 0x003C0738 File Offset: 0x003BE938
		private int CalcInjuryDamageStepBonus(bool inner)
		{
			int percentBonus = 0;
			foreach (SkillBreakPlateBonus bonus in this.GetBreakBonuses())
			{
				percentBonus += bonus.CalcAddInjuryStep(this.Template.EquipType, inner);
			}
			return percentBonus;
		}

		// Token: 0x06006B14 RID: 27412 RVA: 0x003C07A0 File Offset: 0x003BE9A0
		private int CalcFatalDamageStepBonus()
		{
			int percentBonus = 0;
			foreach (SkillBreakPlateBonus bonus in this.GetBreakBonuses())
			{
				percentBonus += bonus.CalcAddFatalStep(this.Template.EquipType);
			}
			return percentBonus;
		}

		// Token: 0x06006B15 RID: 27413 RVA: 0x003C0808 File Offset: 0x003BEA08
		private int CalcMindDamageStepBonus()
		{
			int percentBonus = 0;
			foreach (SkillBreakPlateBonus bonus in this.GetBreakBonuses())
			{
				percentBonus += bonus.CalcAddMindStep(this.Template.EquipType);
			}
			return percentBonus;
		}

		// Token: 0x06006B16 RID: 27414 RVA: 0x003C0870 File Offset: 0x003BEA70
		public int GetMakeDamageBreakBonus()
		{
			int bonus = 0;
			foreach (SkillBreakPageEffectImplementItem effect in this.GetPageEffects())
			{
				bonus += effect.MakeDamage;
			}
			foreach (SkillBreakPlateBonus breakBonus in this.GetBreakBonuses())
			{
				bonus += breakBonus.CalcMakeDamage();
			}
			return bonus;
		}

		// Token: 0x06006B17 RID: 27415 RVA: 0x003C0910 File Offset: 0x003BEB10
		public int GetAcceptDirectDamageBreakBonus(bool anyFatal)
		{
			int bonus = 0;
			foreach (SkillBreakPageEffectImplementItem effect in this.GetPageEffects())
			{
				bonus += (anyFatal ? effect.AcceptDirectDamageOnFatal : effect.AcceptDirectDamageNoFatal);
			}
			return bonus;
		}

		// Token: 0x06006B18 RID: 27416 RVA: 0x003C0974 File Offset: 0x003BEB74
		public CombatSkill(int charId, short skillTemplateId, ushort readingState = 0)
		{
			this._id = new CombatSkillKey(charId, skillTemplateId);
			this._readingState = readingState;
			this._activationState = 0;
			this._forcedBreakoutStepsCount = 0;
			this._breakoutStepsCount = 0;
			this._innerRatio = CombatSkill.Instance[skillTemplateId].BaseInnerRatio;
			this._obtainedNeili = 0;
			this._revoked = false;
			this._specialEffectId = -1L;
		}

		// Token: 0x06006B19 RID: 27417 RVA: 0x003C09F4 File Offset: 0x003BEBF4
		public CombatSkill(IRandomSource random, int charId, short skillTemplateId, sbyte outlinePageType, sbyte directPagesReadCount, sbyte reversePagesReadCount)
		{
			this._id = new CombatSkillKey(charId, skillTemplateId);
			this._readingState = 0;
			this._activationState = 0;
			this._forcedBreakoutStepsCount = 0;
			this._breakoutStepsCount = 0;
			this._innerRatio = CombatSkill.Instance[skillTemplateId].BaseInnerRatio;
			this._obtainedNeili = 0;
			this._revoked = false;
			this._specialEffectId = -1L;
			bool flag = outlinePageType >= 0;
			if (flag)
			{
				byte internalIndex = CombatSkillStateHelper.GetOutlinePageInternalIndex(outlinePageType);
				this._readingState = CombatSkillStateHelper.SetPageRead(this._readingState, internalIndex);
			}
			else
			{
				bool flag2 = outlinePageType == -2;
				if (flag2)
				{
					sbyte randomType = GameData.Domains.Character.BehaviorType.GetRandomBehaviorType(random);
					byte internalIndex2 = CombatSkillStateHelper.GetOutlinePageInternalIndex(randomType);
					this._readingState = CombatSkillStateHelper.SetPageRead(this._readingState, internalIndex2);
				}
			}
			bool flag3 = directPagesReadCount > 0;
			if (flag3)
			{
				this.OfflineSetRandomNormalPagesRead(random, 0, (int)directPagesReadCount);
			}
			bool flag4 = reversePagesReadCount > 0;
			if (flag4)
			{
				this.OfflineComplementNormalPages(random, 1, (int)reversePagesReadCount);
			}
		}

		// Token: 0x06006B1A RID: 27418 RVA: 0x003C0AF8 File Offset: 0x003BECF8
		public CombatSkill(IRandomSource random, PresetCombatSkill presetSkill)
		{
			this._id = new CombatSkillKey(-1, presetSkill.SkillTemplateId);
			this._readingState = 0;
			this._activationState = 0;
			this._forcedBreakoutStepsCount = 0;
			this._breakoutStepsCount = 0;
			this._innerRatio = CombatSkill.Instance[presetSkill.SkillTemplateId].BaseInnerRatio;
			this._obtainedNeili = 0;
			this._revoked = false;
			this._specialEffectId = -1L;
			bool flag = presetSkill.OutlinePagesReadCount > 0;
			if (flag)
			{
				this.OfflineSetRandomOutlinePagesRead(random, presetSkill.OutlinePagesReadCount);
			}
			else
			{
				bool flag2 = presetSkill.OutlinePagesReadCount < 0;
				if (flag2)
				{
					this.OfflineSetOutlinePagesRead(presetSkill.OutlinePagesReadStates);
				}
			}
			bool flag3 = presetSkill.DirectPagesReadCount > 0;
			if (flag3)
			{
				this.OfflineSetRandomNormalPagesRead(random, 0, (int)presetSkill.DirectPagesReadCount);
			}
			else
			{
				bool flag4 = presetSkill.DirectPagesReadCount < 0;
				if (flag4)
				{
					this.OfflineSetNormalPagesRead(0, presetSkill.DirectPagesReadStates);
				}
			}
			bool flag5 = presetSkill.ReversePagesReadCount > 0;
			if (flag5)
			{
				this.OfflineComplementNormalPages(random, 1, (int)presetSkill.ReversePagesReadCount);
			}
			else
			{
				bool flag6 = presetSkill.ReversePagesReadCount < 0;
				if (flag6)
				{
					this.OfflineSetNormalPagesRead(1, presetSkill.ReversePagesReadStates);
				}
			}
		}

		// Token: 0x06006B1B RID: 27419 RVA: 0x003C0C2D File Offset: 0x003BEE2D
		public void OfflineSetCharId(int charId)
		{
			this._id.CharId = charId;
		}

		// Token: 0x06006B1C RID: 27420 RVA: 0x003C0C3C File Offset: 0x003BEE3C
		public void OfflineSetSpecialEffectId(long specialEffectId)
		{
			this._specialEffectId = specialEffectId;
		}

		// Token: 0x06006B1D RID: 27421 RVA: 0x003C0C48 File Offset: 0x003BEE48
		private unsafe void OfflineSetRandomOutlinePagesRead(IRandomSource random, sbyte readPagesCount)
		{
			byte* pIndexes = stackalloc byte[(UIntPtr)5];
			for (byte i = 0; i < 5; i += 1)
			{
				pIndexes[i] = i;
			}
			byte* pShuffledIndexes = CollectionUtils.Shuffle<byte>(random, pIndexes, 5, (int)readPagesCount);
			for (byte* pIndex = pShuffledIndexes; pIndex < pShuffledIndexes + readPagesCount; pIndex++)
			{
				this._readingState = CombatSkillStateHelper.SetPageRead(this._readingState, *pIndex);
			}
		}

		// Token: 0x06006B1E RID: 27422 RVA: 0x003C0CA8 File Offset: 0x003BEEA8
		private void OfflineSetOutlinePagesRead(bool[] pagesReadStates)
		{
			byte index = 0;
			byte count = 5;
			while (index < count)
			{
				bool flag = pagesReadStates[(int)index];
				if (flag)
				{
					this._readingState = CombatSkillStateHelper.SetPageRead(this._readingState, index);
				}
				index += 1;
			}
		}

		// Token: 0x06006B1F RID: 27423 RVA: 0x003C0CE4 File Offset: 0x003BEEE4
		private unsafe void OfflineSetRandomNormalPagesRead(IRandomSource random, sbyte direction, int readPagesCount)
		{
			byte* pPageIds = stackalloc byte[(UIntPtr)5];
			for (int i = 0; i < 5; i++)
			{
				pPageIds[i] = (byte)(i + 1);
			}
			byte* pShuffledPageIds = CollectionUtils.Shuffle<byte>(random, pPageIds, 5, readPagesCount);
			for (byte* pPageId = pShuffledPageIds; pPageId < pShuffledPageIds + readPagesCount; pPageId++)
			{
				byte pageInternalIndex = CombatSkillStateHelper.GetNormalPageInternalIndex(direction, *pPageId);
				this._readingState = CombatSkillStateHelper.SetPageRead(this._readingState, pageInternalIndex);
			}
		}

		// Token: 0x06006B20 RID: 27424 RVA: 0x003C0D54 File Offset: 0x003BEF54
		private unsafe void OfflineComplementNormalPages(IRandomSource random, sbyte direction, int readPagesCount)
		{
			Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)5], 5);
			SpanList<byte> unreadPageIds = span;
			span = new Span<byte>(stackalloc byte[(UIntPtr)5], 5);
			SpanList<byte> readPageIds = span;
			for (int i = 0; i < 5; i++)
			{
				byte pageId = (byte)(i + 1);
				byte internalIndex = CombatSkillStateHelper.GetNormalPageInternalIndex(direction, pageId);
				bool flag = CombatSkillStateHelper.IsPageRead(this._readingState, internalIndex);
				if (flag)
				{
					readPageIds.Add(pageId);
				}
				else
				{
					unreadPageIds.Add(pageId);
				}
			}
			unreadPageIds.Shuffle(random);
			int currPageCount = 0;
			foreach (byte ptr in unreadPageIds)
			{
				byte pageId2 = ptr;
				bool flag2 = currPageCount >= readPagesCount;
				if (flag2)
				{
					return;
				}
				byte pageInternalIndex = CombatSkillStateHelper.GetNormalPageInternalIndex(direction, pageId2);
				this._readingState = CombatSkillStateHelper.SetPageRead(this._readingState, pageInternalIndex);
				currPageCount++;
			}
			readPageIds.Shuffle(random);
			foreach (byte ptr2 in readPageIds)
			{
				byte pageId3 = ptr2;
				bool flag3 = currPageCount >= readPagesCount;
				if (flag3)
				{
					break;
				}
				byte pageInternalIndex2 = CombatSkillStateHelper.GetNormalPageInternalIndex(direction, pageId3);
				this._readingState = CombatSkillStateHelper.SetPageRead(this._readingState, pageInternalIndex2);
				currPageCount++;
			}
		}

		// Token: 0x06006B21 RID: 27425 RVA: 0x003C0E94 File Offset: 0x003BF094
		private void OfflineSetNormalPagesRead(sbyte direction, bool[] pagesReadStates)
		{
			int i = 0;
			int count = 5;
			while (i < count)
			{
				bool flag = pagesReadStates[i];
				if (flag)
				{
					byte pageId = (byte)(i + 1);
					byte internalIndex = CombatSkillStateHelper.GetNormalPageInternalIndex(direction, pageId);
					this._readingState = CombatSkillStateHelper.SetPageRead(this._readingState, internalIndex);
				}
				i++;
			}
		}

		// Token: 0x06006B22 RID: 27426 RVA: 0x003C0EE4 File Offset: 0x003BF0E4
		public unsafe static ushort GenerateRandomReadingState(IRandomSource random, byte bookPageTypes, int readPagesCount)
		{
			ushort readingState = 0;
			bool flag = readPagesCount > 0;
			if (flag)
			{
				sbyte outlinePageType = SkillBookStateHelper.GetOutlinePageType(bookPageTypes);
				byte internalIndex = CombatSkillStateHelper.GetOutlinePageInternalIndex(outlinePageType);
				readingState = CombatSkillStateHelper.SetPageRead(readingState, internalIndex);
				readPagesCount--;
			}
			bool flag2 = readPagesCount > 0;
			if (flag2)
			{
				byte* pPageIds = stackalloc byte[(UIntPtr)5];
				for (int i = 0; i < 5; i++)
				{
					pPageIds[i] = (byte)(i + 1);
				}
				byte* pShuffledPageIds = CollectionUtils.Shuffle<byte>(random, pPageIds, 5, readPagesCount);
				for (byte* pPageId = pShuffledPageIds; pPageId < pShuffledPageIds + readPagesCount; pPageId++)
				{
					byte pageId = *pPageId;
					sbyte direction = SkillBookStateHelper.GetNormalPageType(bookPageTypes, pageId);
					byte internalIndex2 = CombatSkillStateHelper.GetNormalPageInternalIndex(direction, pageId);
					readingState = CombatSkillStateHelper.SetPageRead(readingState, internalIndex2);
				}
			}
			return readingState;
		}

		// Token: 0x06006B23 RID: 27427 RVA: 0x003C0FA0 File Offset: 0x003BF1A0
		public CombatSkillKey GetId()
		{
			return this._id;
		}

		// Token: 0x06006B24 RID: 27428 RVA: 0x003C0FB8 File Offset: 0x003BF1B8
		public sbyte GetPracticeLevel()
		{
			return this._practiceLevel;
		}

		// Token: 0x06006B25 RID: 27429 RVA: 0x003C0FD0 File Offset: 0x003BF1D0
		public unsafe void SetPracticeLevel(sbyte practiceLevel, DataContext context)
		{
			this._practiceLevel = practiceLevel;
			base.SetModifiedAndInvalidateInfluencedCache(1, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<CombatSkillKey>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 8U, 1);
				*pData = (byte)this._practiceLevel;
				pData++;
			}
		}

		// Token: 0x06006B26 RID: 27430 RVA: 0x003C1030 File Offset: 0x003BF230
		public ushort GetReadingState()
		{
			return this._readingState;
		}

		// Token: 0x06006B27 RID: 27431 RVA: 0x003C1048 File Offset: 0x003BF248
		public unsafe void SetReadingState(ushort readingState, DataContext context)
		{
			this._readingState = readingState;
			base.SetModifiedAndInvalidateInfluencedCache(2, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<CombatSkillKey>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 9U, 2);
				*(short*)pData = (short)this._readingState;
				pData += 2;
			}
		}

		// Token: 0x06006B28 RID: 27432 RVA: 0x003C10A8 File Offset: 0x003BF2A8
		public ushort GetActivationState()
		{
			return this._activationState;
		}

		// Token: 0x06006B29 RID: 27433 RVA: 0x003C10C0 File Offset: 0x003BF2C0
		public unsafe void SetActivationState(ushort activationState, DataContext context)
		{
			this._activationState = activationState;
			base.SetModifiedAndInvalidateInfluencedCache(3, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<CombatSkillKey>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 11U, 2);
				*(short*)pData = (short)this._activationState;
				pData += 2;
			}
		}

		// Token: 0x06006B2A RID: 27434 RVA: 0x003C1120 File Offset: 0x003BF320
		public sbyte GetForcedBreakoutStepsCount()
		{
			return this._forcedBreakoutStepsCount;
		}

		// Token: 0x06006B2B RID: 27435 RVA: 0x003C1138 File Offset: 0x003BF338
		public unsafe void SetForcedBreakoutStepsCount(sbyte forcedBreakoutStepsCount, DataContext context)
		{
			this._forcedBreakoutStepsCount = forcedBreakoutStepsCount;
			base.SetModifiedAndInvalidateInfluencedCache(4, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<CombatSkillKey>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 13U, 1);
				*pData = (byte)this._forcedBreakoutStepsCount;
				pData++;
			}
		}

		// Token: 0x06006B2C RID: 27436 RVA: 0x003C1198 File Offset: 0x003BF398
		public sbyte GetBreakoutStepsCount()
		{
			return this._breakoutStepsCount;
		}

		// Token: 0x06006B2D RID: 27437 RVA: 0x003C11B0 File Offset: 0x003BF3B0
		public unsafe void SetBreakoutStepsCount(sbyte breakoutStepsCount, DataContext context)
		{
			this._breakoutStepsCount = breakoutStepsCount;
			base.SetModifiedAndInvalidateInfluencedCache(5, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<CombatSkillKey>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 14U, 1);
				*pData = (byte)this._breakoutStepsCount;
				pData++;
			}
		}

		// Token: 0x06006B2E RID: 27438 RVA: 0x003C1210 File Offset: 0x003BF410
		public sbyte GetInnerRatio()
		{
			return this._innerRatio;
		}

		// Token: 0x06006B2F RID: 27439 RVA: 0x003C1228 File Offset: 0x003BF428
		public unsafe void SetInnerRatio(sbyte innerRatio, DataContext context)
		{
			this._innerRatio = innerRatio;
			base.SetModifiedAndInvalidateInfluencedCache(6, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<CombatSkillKey>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 15U, 1);
				*pData = (byte)this._innerRatio;
				pData++;
			}
		}

		// Token: 0x06006B30 RID: 27440 RVA: 0x003C1288 File Offset: 0x003BF488
		public short GetObtainedNeili()
		{
			return this._obtainedNeili;
		}

		// Token: 0x06006B31 RID: 27441 RVA: 0x003C12A0 File Offset: 0x003BF4A0
		public unsafe void SetObtainedNeili(short obtainedNeili, DataContext context)
		{
			this._obtainedNeili = obtainedNeili;
			base.SetModifiedAndInvalidateInfluencedCache(7, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<CombatSkillKey>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 16U, 2);
				*(short*)pData = this._obtainedNeili;
				pData += 2;
			}
		}

		// Token: 0x06006B32 RID: 27442 RVA: 0x003C1300 File Offset: 0x003BF500
		public bool GetRevoked()
		{
			return this._revoked;
		}

		// Token: 0x06006B33 RID: 27443 RVA: 0x003C1318 File Offset: 0x003BF518
		public unsafe void SetRevoked(bool revoked, DataContext context)
		{
			this._revoked = revoked;
			base.SetModifiedAndInvalidateInfluencedCache(8, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<CombatSkillKey>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 18U, 1);
				*pData = (this._revoked ? 1 : 0);
				pData++;
			}
		}

		// Token: 0x06006B34 RID: 27444 RVA: 0x003C1378 File Offset: 0x003BF578
		public long GetSpecialEffectId()
		{
			return this._specialEffectId;
		}

		// Token: 0x06006B35 RID: 27445 RVA: 0x003C1390 File Offset: 0x003BF590
		public unsafe void SetSpecialEffectId(long specialEffectId, DataContext context)
		{
			this._specialEffectId = specialEffectId;
			base.SetModifiedAndInvalidateInfluencedCache(9, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<CombatSkillKey>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 19U, 8);
				*(long*)pData = this._specialEffectId;
				pData += 8;
			}
		}

		// Token: 0x06006B36 RID: 27446 RVA: 0x003C13F4 File Offset: 0x003BF5F4
		public short GetPower()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			Thread.MemoryBarrier();
			bool flag = dataStates.IsCached(this.DataStatesOffset, 10);
			short power;
			if (flag)
			{
				power = this._power;
			}
			else
			{
				short value = this.CalcPower();
				bool lockTaken = false;
				try
				{
					this._spinLock.Enter(ref lockTaken);
					this._power = value;
					dataStates.SetCached(this.DataStatesOffset, 10);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLock.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				power = this._power;
			}
			return power;
		}

		// Token: 0x06006B37 RID: 27447 RVA: 0x003C149C File Offset: 0x003BF69C
		public short GetMaxPower()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			Thread.MemoryBarrier();
			bool flag = dataStates.IsCached(this.DataStatesOffset, 11);
			short maxPower;
			if (flag)
			{
				maxPower = this._maxPower;
			}
			else
			{
				short value = this.CalcMaxPower();
				bool lockTaken = false;
				try
				{
					this._spinLock.Enter(ref lockTaken);
					this._maxPower = value;
					dataStates.SetCached(this.DataStatesOffset, 11);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLock.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				maxPower = this._maxPower;
			}
			return maxPower;
		}

		// Token: 0x06006B38 RID: 27448 RVA: 0x003C1544 File Offset: 0x003BF744
		public short GetRequirementPercent()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			Thread.MemoryBarrier();
			bool flag = dataStates.IsCached(this.DataStatesOffset, 12);
			short requirementPercent;
			if (flag)
			{
				requirementPercent = this._requirementPercent;
			}
			else
			{
				short value = this.CalcRequirementPercent();
				bool lockTaken = false;
				try
				{
					this._spinLock.Enter(ref lockTaken);
					this._requirementPercent = value;
					dataStates.SetCached(this.DataStatesOffset, 12);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLock.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				requirementPercent = this._requirementPercent;
			}
			return requirementPercent;
		}

		// Token: 0x06006B39 RID: 27449 RVA: 0x003C15EC File Offset: 0x003BF7EC
		public sbyte GetDirection()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			Thread.MemoryBarrier();
			bool flag = dataStates.IsCached(this.DataStatesOffset, 13);
			sbyte direction;
			if (flag)
			{
				direction = this._direction;
			}
			else
			{
				sbyte value = this.CalcDirection();
				bool lockTaken = false;
				try
				{
					this._spinLock.Enter(ref lockTaken);
					this._direction = value;
					dataStates.SetCached(this.DataStatesOffset, 13);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLock.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				direction = this._direction;
			}
			return direction;
		}

		// Token: 0x06006B3A RID: 27450 RVA: 0x003C1694 File Offset: 0x003BF894
		public short GetBaseScore()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			Thread.MemoryBarrier();
			bool flag = dataStates.IsCached(this.DataStatesOffset, 14);
			short baseScore;
			if (flag)
			{
				baseScore = this._baseScore;
			}
			else
			{
				short value = this.CalcBaseScore();
				bool lockTaken = false;
				try
				{
					this._spinLock.Enter(ref lockTaken);
					this._baseScore = value;
					dataStates.SetCached(this.DataStatesOffset, 14);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLock.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				baseScore = this._baseScore;
			}
			return baseScore;
		}

		// Token: 0x06006B3B RID: 27451 RVA: 0x003C173C File Offset: 0x003BF93C
		public sbyte GetCurrInnerRatio()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			Thread.MemoryBarrier();
			bool flag = dataStates.IsCached(this.DataStatesOffset, 15);
			sbyte currInnerRatio;
			if (flag)
			{
				currInnerRatio = this._currInnerRatio;
			}
			else
			{
				sbyte value = this.CalcCurrInnerRatio();
				bool lockTaken = false;
				try
				{
					this._spinLock.Enter(ref lockTaken);
					this._currInnerRatio = value;
					dataStates.SetCached(this.DataStatesOffset, 15);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLock.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				currInnerRatio = this._currInnerRatio;
			}
			return currInnerRatio;
		}

		// Token: 0x06006B3C RID: 27452 RVA: 0x003C17E4 File Offset: 0x003BF9E4
		public HitOrAvoidInts GetHitValue()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			Thread.MemoryBarrier();
			bool flag = dataStates.IsCached(this.DataStatesOffset, 16);
			HitOrAvoidInts hitValue;
			if (flag)
			{
				hitValue = this._hitValue;
			}
			else
			{
				HitOrAvoidInts value = this.CalcHitValue();
				bool lockTaken = false;
				try
				{
					this._spinLock.Enter(ref lockTaken);
					this._hitValue = value;
					dataStates.SetCached(this.DataStatesOffset, 16);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLock.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				hitValue = this._hitValue;
			}
			return hitValue;
		}

		// Token: 0x06006B3D RID: 27453 RVA: 0x003C188C File Offset: 0x003BFA8C
		public OuterAndInnerInts GetPenetrations()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			Thread.MemoryBarrier();
			bool flag = dataStates.IsCached(this.DataStatesOffset, 17);
			OuterAndInnerInts penetrations;
			if (flag)
			{
				penetrations = this._penetrations;
			}
			else
			{
				OuterAndInnerInts value = this.CalcPenetrations();
				bool lockTaken = false;
				try
				{
					this._spinLock.Enter(ref lockTaken);
					this._penetrations = value;
					dataStates.SetCached(this.DataStatesOffset, 17);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLock.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				penetrations = this._penetrations;
			}
			return penetrations;
		}

		// Token: 0x06006B3E RID: 27454 RVA: 0x003C1934 File Offset: 0x003BFB34
		public sbyte GetCostBreathAndStancePercent()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			Thread.MemoryBarrier();
			bool flag = dataStates.IsCached(this.DataStatesOffset, 18);
			sbyte costBreathAndStancePercent;
			if (flag)
			{
				costBreathAndStancePercent = this._costBreathAndStancePercent;
			}
			else
			{
				sbyte value = this.CalcCostBreathAndStancePercent();
				bool lockTaken = false;
				try
				{
					this._spinLock.Enter(ref lockTaken);
					this._costBreathAndStancePercent = value;
					dataStates.SetCached(this.DataStatesOffset, 18);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLock.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				costBreathAndStancePercent = this._costBreathAndStancePercent;
			}
			return costBreathAndStancePercent;
		}

		// Token: 0x06006B3F RID: 27455 RVA: 0x003C19DC File Offset: 0x003BFBDC
		public sbyte GetCostBreathPercent()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			Thread.MemoryBarrier();
			bool flag = dataStates.IsCached(this.DataStatesOffset, 19);
			sbyte costBreathPercent;
			if (flag)
			{
				costBreathPercent = this._costBreathPercent;
			}
			else
			{
				sbyte value = this.CalcCostBreathPercent();
				bool lockTaken = false;
				try
				{
					this._spinLock.Enter(ref lockTaken);
					this._costBreathPercent = value;
					dataStates.SetCached(this.DataStatesOffset, 19);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLock.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				costBreathPercent = this._costBreathPercent;
			}
			return costBreathPercent;
		}

		// Token: 0x06006B40 RID: 27456 RVA: 0x003C1A84 File Offset: 0x003BFC84
		public sbyte GetCostStancePercent()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			Thread.MemoryBarrier();
			bool flag = dataStates.IsCached(this.DataStatesOffset, 20);
			sbyte costStancePercent;
			if (flag)
			{
				costStancePercent = this._costStancePercent;
			}
			else
			{
				sbyte value = this.CalcCostStancePercent();
				bool lockTaken = false;
				try
				{
					this._spinLock.Enter(ref lockTaken);
					this._costStancePercent = value;
					dataStates.SetCached(this.DataStatesOffset, 20);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLock.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				costStancePercent = this._costStancePercent;
			}
			return costStancePercent;
		}

		// Token: 0x06006B41 RID: 27457 RVA: 0x003C1B2C File Offset: 0x003BFD2C
		public sbyte GetCostMobilityPercent()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			Thread.MemoryBarrier();
			bool flag = dataStates.IsCached(this.DataStatesOffset, 21);
			sbyte costMobilityPercent;
			if (flag)
			{
				costMobilityPercent = this._costMobilityPercent;
			}
			else
			{
				sbyte value = this.CalcCostMobilityPercent();
				bool lockTaken = false;
				try
				{
					this._spinLock.Enter(ref lockTaken);
					this._costMobilityPercent = value;
					dataStates.SetCached(this.DataStatesOffset, 21);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLock.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				costMobilityPercent = this._costMobilityPercent;
			}
			return costMobilityPercent;
		}

		// Token: 0x06006B42 RID: 27458 RVA: 0x003C1BD4 File Offset: 0x003BFDD4
		public HitOrAvoidInts GetAddHitValueOnCast()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			Thread.MemoryBarrier();
			bool flag = dataStates.IsCached(this.DataStatesOffset, 22);
			HitOrAvoidInts addHitValueOnCast;
			if (flag)
			{
				addHitValueOnCast = this._addHitValueOnCast;
			}
			else
			{
				HitOrAvoidInts value = this.CalcAddHitValueOnCast();
				bool lockTaken = false;
				try
				{
					this._spinLock.Enter(ref lockTaken);
					this._addHitValueOnCast = value;
					dataStates.SetCached(this.DataStatesOffset, 22);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLock.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				addHitValueOnCast = this._addHitValueOnCast;
			}
			return addHitValueOnCast;
		}

		// Token: 0x06006B43 RID: 27459 RVA: 0x003C1C7C File Offset: 0x003BFE7C
		public OuterAndInnerInts GetAddPenetrateResist()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			Thread.MemoryBarrier();
			bool flag = dataStates.IsCached(this.DataStatesOffset, 23);
			OuterAndInnerInts addPenetrateResist;
			if (flag)
			{
				addPenetrateResist = this._addPenetrateResist;
			}
			else
			{
				OuterAndInnerInts value = this.CalcAddPenetrateResist();
				bool lockTaken = false;
				try
				{
					this._spinLock.Enter(ref lockTaken);
					this._addPenetrateResist = value;
					dataStates.SetCached(this.DataStatesOffset, 23);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLock.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				addPenetrateResist = this._addPenetrateResist;
			}
			return addPenetrateResist;
		}

		// Token: 0x06006B44 RID: 27460 RVA: 0x003C1D24 File Offset: 0x003BFF24
		public HitOrAvoidInts GetAddAvoidValueOnCast()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			Thread.MemoryBarrier();
			bool flag = dataStates.IsCached(this.DataStatesOffset, 24);
			HitOrAvoidInts addAvoidValueOnCast;
			if (flag)
			{
				addAvoidValueOnCast = this._addAvoidValueOnCast;
			}
			else
			{
				HitOrAvoidInts value = this.CalcAddAvoidValueOnCast();
				bool lockTaken = false;
				try
				{
					this._spinLock.Enter(ref lockTaken);
					this._addAvoidValueOnCast = value;
					dataStates.SetCached(this.DataStatesOffset, 24);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLock.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				addAvoidValueOnCast = this._addAvoidValueOnCast;
			}
			return addAvoidValueOnCast;
		}

		// Token: 0x06006B45 RID: 27461 RVA: 0x003C1DCC File Offset: 0x003BFFCC
		public int GetFightBackPower()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			Thread.MemoryBarrier();
			bool flag = dataStates.IsCached(this.DataStatesOffset, 25);
			int fightBackPower;
			if (flag)
			{
				fightBackPower = this._fightBackPower;
			}
			else
			{
				int value = this.CalcFightBackPower();
				bool lockTaken = false;
				try
				{
					this._spinLock.Enter(ref lockTaken);
					this._fightBackPower = value;
					dataStates.SetCached(this.DataStatesOffset, 25);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLock.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				fightBackPower = this._fightBackPower;
			}
			return fightBackPower;
		}

		// Token: 0x06006B46 RID: 27462 RVA: 0x003C1E74 File Offset: 0x003C0074
		public OuterAndInnerInts GetBouncePower()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			Thread.MemoryBarrier();
			bool flag = dataStates.IsCached(this.DataStatesOffset, 26);
			OuterAndInnerInts bouncePower;
			if (flag)
			{
				bouncePower = this._bouncePower;
			}
			else
			{
				OuterAndInnerInts value = this.CalcBouncePower();
				bool lockTaken = false;
				try
				{
					this._spinLock.Enter(ref lockTaken);
					this._bouncePower = value;
					dataStates.SetCached(this.DataStatesOffset, 26);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLock.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				bouncePower = this._bouncePower;
			}
			return bouncePower;
		}

		// Token: 0x06006B47 RID: 27463 RVA: 0x003C1F1C File Offset: 0x003C011C
		public short GetRequirementsPower()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			Thread.MemoryBarrier();
			bool flag = dataStates.IsCached(this.DataStatesOffset, 27);
			short requirementsPower;
			if (flag)
			{
				requirementsPower = this._requirementsPower;
			}
			else
			{
				short value = this.CalcRequirementsPower();
				bool lockTaken = false;
				try
				{
					this._spinLock.Enter(ref lockTaken);
					this._requirementsPower = value;
					dataStates.SetCached(this.DataStatesOffset, 27);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLock.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				requirementsPower = this._requirementsPower;
			}
			return requirementsPower;
		}

		// Token: 0x06006B48 RID: 27464 RVA: 0x003C1FC4 File Offset: 0x003C01C4
		public int GetPlateAddMaxPower()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			Thread.MemoryBarrier();
			bool flag = dataStates.IsCached(this.DataStatesOffset, 28);
			int plateAddMaxPower;
			if (flag)
			{
				plateAddMaxPower = this._plateAddMaxPower;
			}
			else
			{
				int value = this.CalcPlateAddMaxPower();
				bool lockTaken = false;
				try
				{
					this._spinLock.Enter(ref lockTaken);
					this._plateAddMaxPower = value;
					dataStates.SetCached(this.DataStatesOffset, 28);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLock.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				plateAddMaxPower = this._plateAddMaxPower;
			}
			return plateAddMaxPower;
		}

		// Token: 0x06006B49 RID: 27465 RVA: 0x003C206C File Offset: 0x003C026C
		public CombatSkill()
		{
		}

		// Token: 0x06006B4A RID: 27466 RVA: 0x003C208C File Offset: 0x003C028C
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x06006B4B RID: 27467 RVA: 0x003C20A0 File Offset: 0x003C02A0
		public int GetSerializedSize()
		{
			return 27;
		}

		// Token: 0x06006B4C RID: 27468 RVA: 0x003C20B8 File Offset: 0x003C02B8
		public unsafe int Serialize(byte* pData)
		{
			byte* pCurrData = pData + this._id.Serialize(pData);
			*pCurrData = (byte)this._practiceLevel;
			pCurrData++;
			*(short*)pCurrData = (short)this._readingState;
			pCurrData += 2;
			*(short*)pCurrData = (short)this._activationState;
			pCurrData += 2;
			*pCurrData = (byte)this._forcedBreakoutStepsCount;
			pCurrData++;
			*pCurrData = (byte)this._breakoutStepsCount;
			pCurrData++;
			*pCurrData = (byte)this._innerRatio;
			pCurrData++;
			*(short*)pCurrData = this._obtainedNeili;
			pCurrData += 2;
			*pCurrData = (this._revoked ? 1 : 0);
			pCurrData++;
			*(long*)pCurrData = this._specialEffectId;
			pCurrData += 8;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x06006B4D RID: 27469 RVA: 0x003C2150 File Offset: 0x003C0350
		public unsafe int Deserialize(byte* pData)
		{
			byte* pCurrData = pData + this._id.Deserialize(pData);
			this._practiceLevel = *(sbyte*)pCurrData;
			pCurrData++;
			this._readingState = *(ushort*)pCurrData;
			pCurrData += 2;
			this._activationState = *(ushort*)pCurrData;
			pCurrData += 2;
			this._forcedBreakoutStepsCount = *(sbyte*)pCurrData;
			pCurrData++;
			this._breakoutStepsCount = *(sbyte*)pCurrData;
			pCurrData++;
			this._innerRatio = *(sbyte*)pCurrData;
			pCurrData++;
			this._obtainedNeili = *(short*)pCurrData;
			pCurrData += 2;
			this._revoked = (*pCurrData != 0);
			pCurrData++;
			this._specialEffectId = *(long*)pCurrData;
			pCurrData += 8;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x06006B4F RID: 27471 RVA: 0x003C2200 File Offset: 0x003C0400
		[CompilerGenerated]
		internal static CValueModify <CalcEffectPowerModify>g__CalcFinalModify|55_0(ref CombatSkill.<>c__DisplayClass55_0 A_0)
		{
			CValueModify finalModify = A_0.effectAdd * A_0.reverseType;
			bool canReduce = A_0.canReduce;
			if (canReduce)
			{
				finalModify += A_0.effectReduce * A_0.reverseType;
			}
			return finalModify;
		}

		// Token: 0x06006B50 RID: 27472 RVA: 0x003C2247 File Offset: 0x003C0447
		[CompilerGenerated]
		private void <GetCharPropertyBonus>g__ApplyBonus|57_0(int bonusValue, ref CombatSkill.<>c__DisplayClass57_0 A_2)
		{
			A_2.bonus += this.CalcCharacterPropertyBonus((int)A_2.propertyId, bonusValue);
		}

		// Token: 0x06006B51 RID: 27473 RVA: 0x003C2264 File Offset: 0x003C0464
		[CompilerGenerated]
		private void <GetCharPropertyBonus>g__ApplyBonusCollection|57_1(SkillBreakBonusCollection bonusCollection, ref CombatSkill.<>c__DisplayClass57_0 A_2)
		{
			bool flag = bonusCollection == null;
			if (!flag)
			{
				short breakBonus;
				bool flag2 = bonusCollection.CharacterPropertyBonusDict.TryGetValue(A_2.propertyId, out breakBonus);
				if (flag2)
				{
					this.<GetCharPropertyBonus>g__ApplyBonus|57_0((int)breakBonus, ref A_2);
				}
			}
		}

		// Token: 0x06006B52 RID: 27474 RVA: 0x003C229C File Offset: 0x003C049C
		[CompilerGenerated]
		internal static void <GetBreakoutGridCombatSkillPropertyBonus>g__ApplyBonusCollection|58_0(SkillBreakBonusCollection bonusCollection, ref CombatSkill.<>c__DisplayClass58_0 A_1)
		{
			bool flag = bonusCollection == null;
			if (!flag)
			{
				short breakBonus;
				bool flag2 = bonusCollection.CombatSkillPropertyBonusDict.TryGetValue(A_1.propertyId, out breakBonus);
				if (flag2)
				{
					A_1.bonus += (int)breakBonus;
				}
			}
		}

		// Token: 0x06006B53 RID: 27475 RVA: 0x003C22D9 File Offset: 0x003C04D9
		[CompilerGenerated]
		private int <GetBreakAddPropertyList>g__CalcCharacterBonus|64_0(KeyValuePair<short, short> bonus, ref CombatSkill.<>c__DisplayClass64_0 A_2)
		{
			return this.CalcCharacterPropertyBonus((int)bonus.Key, (int)bonus.Value, A_2.power);
		}

		// Token: 0x06006B54 RID: 27476 RVA: 0x003C22F5 File Offset: 0x003C04F5
		[CompilerGenerated]
		private void <GetBreakAddPropertyList>g__AddProperty|64_1(int propertyId, int value, ref CombatSkill.<>c__DisplayClass64_0 A_3)
		{
			A_3.propertyIdValues[propertyId] = A_3.propertyIdValues.GetOrDefault(propertyId) + value;
		}

		// Token: 0x04001D7E RID: 7550
		[CollectionObjectField(false, true, false, true, false)]
		private CombatSkillKey _id;

		// Token: 0x04001D7F RID: 7551
		[CollectionObjectField(false, true, false, false, false)]
		private sbyte _practiceLevel;

		// Token: 0x04001D80 RID: 7552
		[CollectionObjectField(false, true, false, false, false)]
		private ushort _readingState;

		// Token: 0x04001D81 RID: 7553
		[CollectionObjectField(false, true, false, false, false)]
		private ushort _activationState;

		// Token: 0x04001D82 RID: 7554
		[CollectionObjectField(false, true, false, false, false)]
		private sbyte _forcedBreakoutStepsCount;

		// Token: 0x04001D83 RID: 7555
		[CollectionObjectField(false, true, false, false, false)]
		private sbyte _breakoutStepsCount;

		// Token: 0x04001D84 RID: 7556
		[CollectionObjectField(false, true, false, false, false)]
		private sbyte _innerRatio;

		// Token: 0x04001D85 RID: 7557
		[CollectionObjectField(false, true, false, false, false)]
		private short _obtainedNeili;

		// Token: 0x04001D86 RID: 7558
		[CollectionObjectField(false, true, false, false, false)]
		private bool _revoked;

		// Token: 0x04001D87 RID: 7559
		[CollectionObjectField(false, true, false, false, false)]
		private long _specialEffectId;

		// Token: 0x04001D88 RID: 7560
		[CollectionObjectField(false, false, true, false, false)]
		private short _power;

		// Token: 0x04001D89 RID: 7561
		[CollectionObjectField(false, false, true, false, false)]
		private short _requirementsPower;

		// Token: 0x04001D8A RID: 7562
		[CollectionObjectField(false, false, true, false, false)]
		private short _maxPower;

		// Token: 0x04001D8B RID: 7563
		[CollectionObjectField(false, false, true, false, false)]
		private short _requirementPercent;

		// Token: 0x04001D8C RID: 7564
		[CollectionObjectField(false, false, true, false, false)]
		private sbyte _direction = -2;

		// Token: 0x04001D8D RID: 7565
		[CollectionObjectField(false, false, true, false, false)]
		private short _baseScore;

		// Token: 0x04001D8E RID: 7566
		[CollectionObjectField(false, false, true, false, false)]
		private int _plateAddMaxPower;

		// Token: 0x04001D8F RID: 7567
		[CollectionObjectField(false, false, true, false, false)]
		private sbyte _currInnerRatio;

		// Token: 0x04001D90 RID: 7568
		[CollectionObjectField(false, false, true, false, false)]
		private HitOrAvoidInts _hitValue;

		// Token: 0x04001D91 RID: 7569
		[CollectionObjectField(false, false, true, false, false)]
		private OuterAndInnerInts _penetrations;

		// Token: 0x04001D92 RID: 7570
		[CollectionObjectField(false, false, true, false, false)]
		private sbyte _costBreathAndStancePercent;

		// Token: 0x04001D93 RID: 7571
		[CollectionObjectField(false, false, true, false, false)]
		private sbyte _costBreathPercent;

		// Token: 0x04001D94 RID: 7572
		[CollectionObjectField(false, false, true, false, false)]
		private sbyte _costStancePercent;

		// Token: 0x04001D95 RID: 7573
		[CollectionObjectField(false, false, true, false, false)]
		private sbyte _costMobilityPercent;

		// Token: 0x04001D96 RID: 7574
		[CollectionObjectField(false, false, true, false, false)]
		private HitOrAvoidInts _addHitValueOnCast;

		// Token: 0x04001D97 RID: 7575
		[CollectionObjectField(false, false, true, false, false)]
		private OuterAndInnerInts _addPenetrateResist;

		// Token: 0x04001D98 RID: 7576
		[CollectionObjectField(false, false, true, false, false)]
		private HitOrAvoidInts _addAvoidValueOnCast;

		// Token: 0x04001D99 RID: 7577
		[CollectionObjectField(false, false, true, false, false)]
		private int _fightBackPower;

		// Token: 0x04001D9A RID: 7578
		[CollectionObjectField(false, false, true, false, false)]
		private OuterAndInnerInts _bouncePower;

		// Token: 0x04001D9B RID: 7579
		private static readonly short[] BaseScoreOfGrades = new short[]
		{
			100,
			167,
			278,
			463,
			772,
			1286,
			2143,
			3572,
			5954
		};

		// Token: 0x04001D9C RID: 7580
		public const int FixedSize = 27;

		// Token: 0x04001D9D RID: 7581
		public const int DynamicCount = 0;

		// Token: 0x04001D9E RID: 7582
		private SpinLock _spinLock = new SpinLock(false);

		// Token: 0x02000BB0 RID: 2992
		internal class FixedFieldInfos
		{
			// Token: 0x04003230 RID: 12848
			public const uint Id_Offset = 0U;

			// Token: 0x04003231 RID: 12849
			public const int Id_Size = 8;

			// Token: 0x04003232 RID: 12850
			public const uint PracticeLevel_Offset = 8U;

			// Token: 0x04003233 RID: 12851
			public const int PracticeLevel_Size = 1;

			// Token: 0x04003234 RID: 12852
			public const uint ReadingState_Offset = 9U;

			// Token: 0x04003235 RID: 12853
			public const int ReadingState_Size = 2;

			// Token: 0x04003236 RID: 12854
			public const uint ActivationState_Offset = 11U;

			// Token: 0x04003237 RID: 12855
			public const int ActivationState_Size = 2;

			// Token: 0x04003238 RID: 12856
			public const uint ForcedBreakoutStepsCount_Offset = 13U;

			// Token: 0x04003239 RID: 12857
			public const int ForcedBreakoutStepsCount_Size = 1;

			// Token: 0x0400323A RID: 12858
			public const uint BreakoutStepsCount_Offset = 14U;

			// Token: 0x0400323B RID: 12859
			public const int BreakoutStepsCount_Size = 1;

			// Token: 0x0400323C RID: 12860
			public const uint InnerRatio_Offset = 15U;

			// Token: 0x0400323D RID: 12861
			public const int InnerRatio_Size = 1;

			// Token: 0x0400323E RID: 12862
			public const uint ObtainedNeili_Offset = 16U;

			// Token: 0x0400323F RID: 12863
			public const int ObtainedNeili_Size = 2;

			// Token: 0x04003240 RID: 12864
			public const uint Revoked_Offset = 18U;

			// Token: 0x04003241 RID: 12865
			public const int Revoked_Size = 1;

			// Token: 0x04003242 RID: 12866
			public const uint SpecialEffectId_Offset = 19U;

			// Token: 0x04003243 RID: 12867
			public const int SpecialEffectId_Size = 8;
		}
	}
}
