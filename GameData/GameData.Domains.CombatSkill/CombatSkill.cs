using System;
using System.Collections.Generic;
using System.Linq;
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
using GameData.Domains.TaiwuEvent;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.CombatSkill;

[SerializableGameData(NotForDisplayModule = true)]
public class CombatSkill : BaseGameDataObject, ISerializableGameData
{
	internal class FixedFieldInfos
	{
		public const uint Id_Offset = 0u;

		public const int Id_Size = 8;

		public const uint PracticeLevel_Offset = 8u;

		public const int PracticeLevel_Size = 1;

		public const uint ReadingState_Offset = 9u;

		public const int ReadingState_Size = 2;

		public const uint ActivationState_Offset = 11u;

		public const int ActivationState_Size = 2;

		public const uint ForcedBreakoutStepsCount_Offset = 13u;

		public const int ForcedBreakoutStepsCount_Size = 1;

		public const uint BreakoutStepsCount_Offset = 14u;

		public const int BreakoutStepsCount_Size = 1;

		public const uint InnerRatio_Offset = 15u;

		public const int InnerRatio_Size = 1;

		public const uint ObtainedNeili_Offset = 16u;

		public const int ObtainedNeili_Size = 2;

		public const uint Revoked_Offset = 18u;

		public const int Revoked_Size = 1;

		public const uint SpecialEffectId_Offset = 19u;

		public const int SpecialEffectId_Size = 8;
	}

	[CollectionObjectField(false, true, false, true, false)]
	private CombatSkillKey _id;

	[CollectionObjectField(false, true, false, false, false)]
	private sbyte _practiceLevel;

	[CollectionObjectField(false, true, false, false, false)]
	private ushort _readingState;

	[CollectionObjectField(false, true, false, false, false)]
	private ushort _activationState;

	[CollectionObjectField(false, true, false, false, false)]
	private sbyte _forcedBreakoutStepsCount;

	[CollectionObjectField(false, true, false, false, false)]
	private sbyte _breakoutStepsCount;

	[CollectionObjectField(false, true, false, false, false)]
	private sbyte _innerRatio;

	[CollectionObjectField(false, true, false, false, false)]
	private short _obtainedNeili;

	[CollectionObjectField(false, true, false, false, false)]
	private bool _revoked;

	[CollectionObjectField(false, true, false, false, false)]
	private long _specialEffectId;

	[CollectionObjectField(false, false, true, false, false)]
	private short _power;

	[CollectionObjectField(false, false, true, false, false)]
	private short _requirementsPower;

	[CollectionObjectField(false, false, true, false, false)]
	private short _maxPower;

	[CollectionObjectField(false, false, true, false, false)]
	private short _requirementPercent;

	[CollectionObjectField(false, false, true, false, false)]
	private sbyte _direction = -2;

	[CollectionObjectField(false, false, true, false, false)]
	private short _baseScore;

	[CollectionObjectField(false, false, true, false, false)]
	private int _plateAddMaxPower;

	[CollectionObjectField(false, false, true, false, false)]
	private sbyte _currInnerRatio;

	[CollectionObjectField(false, false, true, false, false)]
	private HitOrAvoidInts _hitValue;

	[CollectionObjectField(false, false, true, false, false)]
	private OuterAndInnerInts _penetrations;

	[CollectionObjectField(false, false, true, false, false)]
	private sbyte _costBreathAndStancePercent;

	[CollectionObjectField(false, false, true, false, false)]
	private sbyte _costBreathPercent;

	[CollectionObjectField(false, false, true, false, false)]
	private sbyte _costStancePercent;

	[CollectionObjectField(false, false, true, false, false)]
	private sbyte _costMobilityPercent;

	[CollectionObjectField(false, false, true, false, false)]
	private HitOrAvoidInts _addHitValueOnCast;

	[CollectionObjectField(false, false, true, false, false)]
	private OuterAndInnerInts _addPenetrateResist;

	[CollectionObjectField(false, false, true, false, false)]
	private HitOrAvoidInts _addAvoidValueOnCast;

	[CollectionObjectField(false, false, true, false, false)]
	private int _fightBackPower;

	[CollectionObjectField(false, false, true, false, false)]
	private OuterAndInnerInts _bouncePower;

	private static readonly short[] BaseScoreOfGrades = new short[9] { 100, 167, 278, 463, 772, 1286, 2143, 3572, 5954 };

	public const int FixedSize = 27;

	public const int DynamicCount = 0;

	private SpinLock _spinLock = new SpinLock(enableThreadOwnerTracking: false);

	public CombatSkillItem Template => Config.CombatSkill.Instance[_id.SkillTemplateId];

	[Obsolete("This method is obsolete, and will be removed in future.")]
	public void ChangePracticeLevel(DataContext context, int delta)
	{
	}

	[Obsolete("This method is obsolete, and will be removed in future. Use Character.GetBreakoutAvailableStepsCount instead.")]
	public sbyte GetBreakoutAvailableStepsCount(GameData.Domains.Character.Character character)
	{
		return character.GetSkillBreakoutAvailableStepsCount(_id.SkillTemplateId);
	}

	[ObjectCollectionDependency(7, 0, new ushort[] { 4, 13, 3, 27 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(4, 0, new ushort[] { 5, 57, 17, 59 }, Scope = InfluenceScope.CombatSkillsOfTheChar)]
	[ObjectCollectionDependency(17, 2, new ushort[] { 199, 292 }, Scope = InfluenceScope.CombatSkillsOfTheCharacterAffectedByTheSpecialEffects)]
	[SingleValueDependency(8, new ushort[] { 19 }, Scope = InfluenceScope.CombatSkillsOfAllCharsInCombat)]
	[ObjectCollectionDependency(8, 10, new ushort[] { 3, 105 }, Scope = InfluenceScope.CombatSkillsOfTheCombatChar)]
	[SingleValueCollectionDependency(8, new ushort[] { 6, 7 }, Scope = InfluenceScope.CombatSkillsAffectedByPowerChangeInCombat)]
	[SingleValueCollectionDependency(8, new ushort[] { 8 }, Scope = InfluenceScope.CombatSkillsAffectedByPowerReplaceInCombat)]
	[ObjectCollectionDependency(8, 29, new ushort[] { 6 }, Scope = InfluenceScope.CombatSkillsAffectedByCombatSkillDataInCombat)]
	[SingleValueDependency(1, new ushort[] { 27 }, Scope = InfluenceScope.All)]
	private short CalcPower()
	{
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		if (DomainManager.Combat.IsInCombat() && DomainManager.Combat.TryGetElement_SkillDataDict(_id, out var element) && element.GetSilencing())
		{
			return 0;
		}
		if (DomainManager.Combat.IsInCombat() && DomainManager.Combat.GetAllSkillPowerReplaceInCombat().ContainsKey(_id))
		{
			return DomainManager.CombatSkill.GetElement_CombatSkills(DomainManager.Combat.GetAllSkillPowerReplaceInCombat()[_id]).GetPower();
		}
		int num = CalcBasePower();
		CValueModify val = CalcBasePowerModify();
		val += CalcEffectPowerModify();
		num *= val;
		num = Math.Max(num, 0);
		num = DomainManager.SpecialEffect.ModifyData(_id.CharId, _id.SkillTemplateId, 199, num);
		return (short)Math.Clamp(num, 10, GlobalConfig.Instance.CombatSkillMaxPower);
	}

	[ObjectCollectionDependency(7, 0, new ushort[] { 11, 12 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(4, 0, new ushort[] { 80, 97, 98, 99, 100 }, Scope = InfluenceScope.CombatSkillsOfTheChar)]
	private short CalcRequirementsPower()
	{
		int num = 0;
		short maxPower = GetMaxPower();
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(_id.CharId);
		List<(int, int, int)> requirementsAndActualValues = GetRequirementsAndActualValues(element_Objects);
		int num2 = requirementsAndActualValues.Count - 1;
		(int, int, int) tuple = requirementsAndActualValues[requirementsAndActualValues.Count - 1];
		int num3 = Math.Min(tuple.Item3 * 100 / tuple.Item2, maxPower);
		if (num2 > 0)
		{
			for (int i = 0; i < num2; i++)
			{
				(int, int, int) tuple2 = requirementsAndActualValues[i];
				int item = tuple2.Item2;
				int item2 = tuple2.Item3;
				int num4 = ((item > 0) ? Math.Min(item2 * 100 / item, maxPower) : 0);
				if (num4 >= num3)
				{
					num += num4;
					continue;
				}
				num += num3;
				num3 = num4;
			}
			num /= num2;
		}
		else
		{
			num = 100;
		}
		return (short)Math.Clamp(num, 0, 32767);
	}

	[ObjectCollectionDependency(7, 0, new ushort[] { 3 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(4, 0, new ushort[] { 112, 117, 57 }, Scope = InfluenceScope.CombatSkillsOfTheChar)]
	[SingleValueDependency(8, new ushort[] { 19 }, Scope = InfluenceScope.CombatSkillsOfAllCharsInCombat)]
	[ObjectCollectionDependency(17, 2, new ushort[] { 200, 240 }, Scope = InfluenceScope.CombatSkillsOfTheCharacterAffectedByTheSpecialEffects)]
	private short CalcMaxPower()
	{
		int combatSkillMaxBasePower = GlobalConfig.Instance.CombatSkillMaxBasePower;
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(_id.CharId);
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[_id.SkillTemplateId];
		EventArgBox globalEventArgumentBox = DomainManager.TaiwuEvent.GetGlobalEventArgumentBox();
		bool arg = false;
		globalEventArgumentBox.Get("IsGuardCombat", ref arg);
		if (arg && DomainManager.Combat.IsCharInCombat(_id.CharId) && element_Objects.IsTreasuryGuard())
		{
			return GlobalConfig.Instance.CombatSkillMaxPower;
		}
		combatSkillMaxBasePower += CombatSkillDomain.FiveElementIndexesSum(_id.CharId, combatSkillItem, NeiliType.Instance[element_Objects.GetNeiliType()].MaxPowerChange);
		foreach (SkillBreakPageEffectImplementItem pageEffect in GetPageEffects())
		{
			combatSkillMaxBasePower += pageEffect.AddMaxPower;
		}
		combatSkillMaxBasePower += GetBreakoutGridCombatSkillPropertyBonus(1);
		ref LifeSkillShorts lifeSkillAttainments = ref element_Objects.GetLifeSkillAttainments();
		foreach (SkillBreakPlateBonus breakBonuse in GetBreakBonuses())
		{
			combatSkillMaxBasePower += breakBonuse.CalcAddMaxPower(combatSkillItem.EquipType, ref lifeSkillAttainments);
		}
		combatSkillMaxBasePower += element_Objects.GetSkillBreakoutStepsMaxPower(_id.SkillTemplateId);
		CombatSkillEquipment combatSkillEquipment = element_Objects.GetCombatSkillEquipment();
		if (combatSkillEquipment.IsCombatSkillEquipped(_id.SkillTemplateId))
		{
			ArraySegmentList<short>.Enumerator enumerator3 = combatSkillEquipment.Neigong.GetEnumerator();
			while (enumerator3.MoveNext())
			{
				short current2 = enumerator3.Current;
				if (!DomainManager.CombatSkill.TryGetElement_CombatSkills((charId: _id.CharId, skillId: current2), out var element))
				{
					continue;
				}
				foreach (SkillBreakPlateBonus breakBonuse2 in element.GetBreakBonuses())
				{
					combatSkillMaxBasePower += breakBonuse2.CalcAddOtherSkillMaxPower(Template.EquipType);
				}
			}
		}
		ItemKey[] equipment = element_Objects.GetEquipment();
		for (sbyte b = 8; b <= 10; b++)
		{
			ItemKey itemKey = equipment[b];
			if (itemKey.IsValid() && itemKey.ItemType == 2)
			{
				AccessoryItem accessoryItem = Config.Accessory.Instance[itemKey.TemplateId];
				combatSkillMaxBasePower += accessoryItem.CombatSkillAddMaxPower;
			}
		}
		combatSkillMaxBasePower += DomainManager.SpecialEffect.GetModifyValue(_id.CharId, _id.SkillTemplateId, 200, (EDataModifyType)0, -1, -1, -1, (EDataSumType)0);
		return (short)Math.Clamp(combatSkillMaxBasePower, 0, GlobalConfig.Instance.CombatSkillMaxPower);
	}

	[ObjectCollectionDependency(7, 0, new ushort[] { 3 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(4, 0, new ushort[] { 112 }, Scope = InfluenceScope.CombatSkillsOfTheChar)]
	[ObjectCollectionDependency(17, 2, new ushort[] { 202 }, Scope = InfluenceScope.CombatSkillsOfTheCharacterAffectedByTheSpecialEffects)]
	[SingleValueCollectionDependency(19, new ushort[] { 121 }, Scope = InfluenceScope.CombatSkillsOfTaiwuChar)]
	private short CalcRequirementPercent()
	{
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		int num = 100;
		foreach (SkillBreakPageEffectImplementItem pageEffect in GetPageEffects())
		{
			num += pageEffect.AddRequirement;
		}
		foreach (SkillBreakPlateBonus breakBonuse in GetBreakBonuses())
		{
			num += breakBonuse.CalcReduceRequirements(Template.EquipType);
		}
		num += (short)DomainManager.SpecialEffect.GetModifyValue(_id.CharId, _id.SkillTemplateId, 202, (EDataModifyType)0, -1, -1, -1, (EDataSumType)0);
		num += GetBreakoutGridCombatSkillPropertyBonus(48);
		if (DomainManager.Extra.IsCombatSkillMasteredByCharacter(_id.CharId, _id.SkillTemplateId))
		{
			CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[_id.SkillTemplateId];
			if (combatSkillItem.GridCost == 2)
			{
				num += 150;
			}
			else if (combatSkillItem.GridCost == 3)
			{
				num += 100;
			}
		}
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(_id.CharId);
		CombatSkillItem config = Config.CombatSkill.Instance[_id.SkillTemplateId];
		(int, int) tuple = CombatSkillDomain.FiveElementIndexesTotal(_id.CharId, config, NeiliType.Instance[element_Objects.GetNeiliType()].RequirementChange);
		num *= CValuePercentBonus.op_Implicit(tuple.Item1 + tuple.Item2);
		return (short)Math.Max(num, 10);
	}

	[ObjectCollectionDependency(7, 0, new ushort[] { 3 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(17, 2, new ushort[] { 209 }, Scope = InfluenceScope.CombatSkillsOfTheCharacterAffectedByTheSpecialEffects)]
	private sbyte CalcDirection()
	{
		sbyte b = CombatSkillStateHelper.GetCombatSkillDirection(_activationState);
		if (DomainManager.SpecialEffect.ModifyData(_id.CharId, _id.SkillTemplateId, 210, dataValue: true))
		{
			b = (sbyte)DomainManager.SpecialEffect.ModifyData(_id.CharId, _id.SkillTemplateId, (ushort)209, (int)b, -1, -1, -1);
		}
		if (_direction != -2 && b != _direction)
		{
			CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[_id.SkillTemplateId];
			int index = ((b == 0) ? combatSkillItem.DirectEffectID : combatSkillItem.ReverseEffectID);
			if (Config.SpecialEffect.Instance[index].EffectActiveType == 3)
			{
				if (_id.CharId == DomainManager.Taiwu.GetTaiwuCharId())
				{
					DataContext currentThreadDataContext = DataContextManager.GetCurrentThreadDataContext();
					if (_specialEffectId >= 0)
					{
						DomainManager.SpecialEffect.Remove(currentThreadDataContext, _specialEffectId);
					}
					if (b >= 0)
					{
						DomainManager.SpecialEffect.Add(currentThreadDataContext, _id.CharId, _id.SkillTemplateId, 3, b);
					}
				}
				else
				{
					DomainManager.SpecialEffect.AddBrokenEffectChangedDuringAdvance(_specialEffectId, _id.CharId, _id.SkillTemplateId);
				}
			}
		}
		return b;
	}

	[ObjectCollectionDependency(7, 0, new ushort[] { 1, 10, 3 }, Scope = InfluenceScope.Self)]
	private short CalcBaseScore()
	{
		sbyte grade = Config.CombatSkill.Instance[_id.SkillTemplateId].Grade;
		int num = BaseScoreOfGrades[grade] * GetPower() / 100;
		if (CombatSkillStateHelper.IsBrokenOut(_activationState))
		{
			num = num * 3 / 2;
		}
		return (short)num;
	}

	[ObjectCollectionDependency(7, 0, new ushort[] { 6 }, Condition = InfluenceCondition.CombatSkillIsProactive, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(4, 0, new ushort[] { 92 }, Scope = InfluenceScope.CombatSkillsOfTheChar)]
	[ObjectCollectionDependency(17, 2, new ushort[] { 203 }, Scope = InfluenceScope.CombatSkillsOfTheCharacterAffectedByTheSpecialEffects)]
	[SingleValueCollectionDependency(19, new ushort[] { 121 }, Scope = InfluenceScope.CombatSkillsOfTaiwuChar)]
	private sbyte CalcCurrInnerRatio()
	{
		int baseInnerRatio = GetBaseInnerRatio();
		int innerRatio = DomainManager.Character.GetElement_Objects(_id.CharId).GetInnerRatio();
		int num = GetInnerRatioChangeRange() * innerRatio / 100;
		int min = Math.Max(baseInnerRatio - num, 0);
		int max = Math.Min(baseInnerRatio + num, 100);
		int num2 = Math.Clamp(GetInnerRatio(), min, max);
		num2 += DomainManager.SpecialEffect.GetModifyValue(_id.CharId, _id.SkillTemplateId, 203, (EDataModifyType)0, -1, -1, -1, (EDataSumType)0);
		num2 = Math.Clamp(num2, 0, 100);
		return (sbyte)num2;
	}

	[ObjectCollectionDependency(7, 0, new ushort[] { 1, 10 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(17, 2, new ushort[] { 224 }, Scope = InfluenceScope.CombatSkillsOfTheCharacterAffectedByTheSpecialEffects)]
	[SingleValueCollectionDependency(19, new ushort[] { 121 }, Scope = InfluenceScope.CombatSkillsOfTaiwuChar)]
	private unsafe HitOrAvoidInts CalcHitValue()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		bool flag = CombatSkillTemplateHelper.IsMindHitSkill(_id.SkillTemplateId);
		CValuePercent val = CValuePercent.op_Implicit((int)GetPower());
		CValuePercent val2 = CValuePercent.op_Implicit((int)GetTotalHit());
		HitOrAvoidInts result = default(HitOrAvoidInts);
		HitOrAvoidInts hitDistribution = GetHitDistribution();
		for (int i = 0; i < 3; i++)
		{
			result.Items[i] = ((!flag) ? (hitDistribution.Items[i] * val2 * val) : 0);
		}
		result.Items[3] = (flag ? ((int)val2 * val) : 0);
		return result;
	}

	[ObjectCollectionDependency(7, 0, new ushort[] { 1, 10, 15, 3 }, Scope = InfluenceScope.Self)]
	[SingleValueCollectionDependency(19, new ushort[] { 121 }, Scope = InfluenceScope.CombatSkillsOfTaiwuChar)]
	private OuterAndInnerInts CalcPenetrations()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[_id.SkillTemplateId];
		int penetrate = combatSkillItem.Penetrate;
		penetrate += GetBreakoutGridCombatSkillPropertyBonus(72);
		CValuePercentBonus val = CValuePercentBonus.op_Implicit(GetBreakoutGridCombatSkillPropertyBonus(29));
		CValuePercent val2 = CValuePercent.op_Implicit((int)GetPower());
		penetrate = penetrate * val * val2;
		OuterAndInnerInts result = default(OuterAndInnerInts);
		result.Inner = penetrate * GetCurrInnerRatio() / 100;
		result.Outer = penetrate - result.Inner;
		return result;
	}

	[ObjectCollectionDependency(7, 0, new ushort[] { 3 }, Scope = InfluenceScope.Self)]
	[SingleValueCollectionDependency(19, new ushort[] { 121 }, Scope = InfluenceScope.CombatSkillsOfTaiwuChar)]
	private sbyte CalcCostBreathAndStancePercent()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		int breathStanceTotalCost = Config.CombatSkill.Instance[_id.SkillTemplateId].BreathStanceTotalCost;
		breathStanceTotalCost += GetBreakoutGridCombatSkillPropertyBonus(3);
		CValuePercentBonus val = CValuePercentBonus.op_Implicit(0);
		foreach (SkillBreakPageEffectImplementItem pageEffect in GetPageEffects())
		{
			val += CValuePercentBonus.op_Implicit(pageEffect.CostBreathAndStance);
		}
		breathStanceTotalCost *= val;
		return (sbyte)Math.Clamp(breathStanceTotalCost, 0, 100);
	}

	[ObjectCollectionDependency(7, 0, new ushort[] { 18, 15 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(17, 2, new ushort[] { 205, 204 }, Scope = InfluenceScope.CombatSkillsOfTheCharacterAffectedByTheSpecialEffects)]
	private sbyte CalcCostBreathPercent()
	{
		sbyte costBreathAndStancePercent = GetCostBreathAndStancePercent();
		int num = costBreathAndStancePercent * GetCurrInnerRatio() / 100;
		int num2 = 100 + DomainManager.SpecialEffect.GetModifyValue(_id.CharId, _id.SkillTemplateId, 205, (EDataModifyType)1, -1, -1, -1, (EDataSumType)0);
		num2 += DomainManager.SpecialEffect.GetModifyValue(_id.CharId, _id.SkillTemplateId, 204, (EDataModifyType)1, -1, -1, -1, (EDataSumType)0);
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(_id.CharId);
		ref LifeSkillShorts lifeSkillAttainments = ref element_Objects.GetLifeSkillAttainments();
		foreach (SkillBreakPlateBonus breakBonuse in GetBreakBonuses())
		{
			num2 -= breakBonuse.CalcReduceCostBreath(Template.EquipType, ref lifeSkillAttainments);
		}
		num = num * num2 / 100;
		(int, int) totalPercentModifyValue = DomainManager.SpecialEffect.GetTotalPercentModifyValue(_id.CharId, _id.SkillTemplateId, 205);
		(int, int) totalPercentModifyValue2 = DomainManager.SpecialEffect.GetTotalPercentModifyValue(_id.CharId, _id.SkillTemplateId, 204);
		totalPercentModifyValue.Item1 = Math.Max(totalPercentModifyValue.Item1, totalPercentModifyValue2.Item1);
		totalPercentModifyValue.Item2 = Math.Min(totalPercentModifyValue.Item2, totalPercentModifyValue2.Item2);
		num2 = Math.Max(100 + totalPercentModifyValue.Item1 + totalPercentModifyValue.Item2, 0);
		num = num * num2 / 100;
		num = DomainManager.SpecialEffect.ModifyData(_id.CharId, _id.SkillTemplateId, 205, num);
		return (sbyte)Math.Clamp(num, 0, 100);
	}

	[ObjectCollectionDependency(7, 0, new ushort[] { 18, 15 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(17, 2, new ushort[] { 206, 204 }, Scope = InfluenceScope.CombatSkillsOfTheCharacterAffectedByTheSpecialEffects)]
	private sbyte CalcCostStancePercent()
	{
		sbyte costBreathAndStancePercent = GetCostBreathAndStancePercent();
		int num = costBreathAndStancePercent - costBreathAndStancePercent * GetCurrInnerRatio() / 100;
		int num2 = 100 + DomainManager.SpecialEffect.GetModifyValue(_id.CharId, _id.SkillTemplateId, 206, (EDataModifyType)1, -1, -1, -1, (EDataSumType)0);
		num2 += DomainManager.SpecialEffect.GetModifyValue(_id.CharId, _id.SkillTemplateId, 204, (EDataModifyType)1, -1, -1, -1, (EDataSumType)0);
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(_id.CharId);
		ref LifeSkillShorts lifeSkillAttainments = ref element_Objects.GetLifeSkillAttainments();
		foreach (SkillBreakPlateBonus breakBonuse in GetBreakBonuses())
		{
			num2 -= breakBonuse.CalcReduceCostStance(Template.EquipType, ref lifeSkillAttainments);
		}
		num = num * num2 / 100;
		(int, int) totalPercentModifyValue = DomainManager.SpecialEffect.GetTotalPercentModifyValue(_id.CharId, _id.SkillTemplateId, 206);
		(int, int) totalPercentModifyValue2 = DomainManager.SpecialEffect.GetTotalPercentModifyValue(_id.CharId, _id.SkillTemplateId, 204);
		totalPercentModifyValue.Item1 = Math.Max(totalPercentModifyValue.Item1, totalPercentModifyValue2.Item1);
		totalPercentModifyValue.Item2 = Math.Min(totalPercentModifyValue.Item2, totalPercentModifyValue2.Item2);
		num2 = Math.Max(100 + totalPercentModifyValue.Item1 + totalPercentModifyValue.Item2, 0);
		num = num * num2 / 100;
		num = DomainManager.SpecialEffect.ModifyData(_id.CharId, _id.SkillTemplateId, 206, num);
		return (sbyte)Math.Clamp(num, 0, 100);
	}

	[ObjectCollectionDependency(7, 0, new ushort[] { 3 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(17, 2, new ushort[] { 207 }, Scope = InfluenceScope.CombatSkillsOfTheCharacterAffectedByTheSpecialEffects)]
	[SingleValueCollectionDependency(19, new ushort[] { 121 }, Scope = InfluenceScope.CombatSkillsOfTaiwuChar)]
	private sbyte CalcCostMobilityPercent()
	{
		int mobilityCost = Config.CombatSkill.Instance[_id.SkillTemplateId].MobilityCost;
		mobilityCost = Math.Max(mobilityCost + GetBreakoutGridCombatSkillPropertyBonus(10), 0);
		int num = 0;
		if (Template.EquipType == 2)
		{
			foreach (SkillBreakPlateBonus breakBonuse in GetBreakBonuses())
			{
				num += breakBonuse.CalcCostMobilityByCast();
			}
		}
		mobilityCost = DomainManager.SpecialEffect.ModifyValueCustom(_id.CharId, _id.SkillTemplateId, 207, mobilityCost, -1, -1, -1, 0, num);
		return (sbyte)Math.Clamp(mobilityCost, 0, 100);
	}

	[ObjectCollectionDependency(7, 0, new ushort[] { 1, 10, 3 }, Scope = InfluenceScope.Self)]
	[SingleValueCollectionDependency(19, new ushort[] { 121 }, Scope = InfluenceScope.CombatSkillsOfTaiwuChar)]
	private HitOrAvoidInts CalcAddHitValueOnCast()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return CombatSkillDomain.CalcAddHitValueOnCast(this, CValuePercent.op_Implicit((int)GetPower()));
	}

	[ObjectCollectionDependency(7, 0, new ushort[] { 1, 10 }, Scope = InfluenceScope.Self)]
	private OuterAndInnerInts CalcAddPenetrateResist()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return CombatSkillDomain.CalcAddPenetrateResist(this, CValuePercent.op_Implicit((int)GetPower()));
	}

	[ObjectCollectionDependency(7, 0, new ushort[] { 1, 10 }, Scope = InfluenceScope.Self)]
	[SingleValueCollectionDependency(19, new ushort[] { 121 }, Scope = InfluenceScope.CombatSkillsOfTaiwuChar)]
	private HitOrAvoidInts CalcAddAvoidValueOnCast()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return CombatSkillDomain.CalcAddAvoidValueOnCast(this, CValuePercent.op_Implicit((int)GetPower()));
	}

	[ObjectCollectionDependency(7, 0, new ushort[] { 1, 10, 3 }, Scope = InfluenceScope.Self)]
	private int CalcFightBackPower()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return CombatSkillDomain.CalcFightBackPower(this, CValuePercent.op_Implicit((int)GetPower()));
	}

	[ObjectCollectionDependency(7, 0, new ushort[] { 1, 10 }, Scope = InfluenceScope.Self)]
	private OuterAndInnerInts CalcBouncePower()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return CombatSkillDomain.CalcBouncePower(this, CValuePercent.op_Implicit((int)GetPower()));
	}

	[SingleValueCollectionDependency(19, new ushort[] { 277 }, Scope = InfluenceScope.CombatSkillsOfTaiwuChar)]
	private int CalcPlateAddMaxPower()
	{
		if (DomainManager.Extra.TryGetElement_SkillBreakPlates(_id.SkillTemplateId, out var value))
		{
			return value.AddMaxPower;
		}
		return 0;
	}

	private int CalcBasePower()
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(_id.CharId);
		int fixCombatSkillPower = element_Objects.GetFixCombatSkillPower();
		if (fixCombatSkillPower >= 0)
		{
			return fixCombatSkillPower;
		}
		int num = GetRequirementsPower();
		foreach (SkillBreakPageEffectImplementItem pageEffect in GetPageEffects())
		{
			num += pageEffect.AddPower;
		}
		num += GetBreakoutGridCombatSkillPropertyBonus(0);
		foreach (SkillBreakPlateBonus breakBonuse in GetBreakBonuses())
		{
			num += breakBonuse.CalcAddPower(Template.EquipType);
		}
		List<short> featureIds = element_Objects.GetFeatureIds();
		for (int i = 0; i < featureIds.Count; i++)
		{
			short index = featureIds[i];
			CharacterFeatureItem characterFeatureItem = CharacterFeature.Instance[index];
			num += characterFeatureItem.CombatSkillPowerBonuses[Template.EquipType];
			if (Template.FiveElements != 5)
			{
				num += characterFeatureItem.FiveElementPowerBonuses[Template.FiveElements];
			}
		}
		return Math.Max(num, 0);
	}

	private CValueModify CalcBasePowerModify()
	{
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(_id.CharId);
		foreach (SolarTermItem item in element_Objects.GetInvokedSolarTerm())
		{
			if (CombatSkillDomain.FiveElementContains(_id.CharId, Template, item.FiveElementsTypesOfCombatSkillBuff))
			{
				num += element_Objects.GetSolarTermValue(GlobalConfig.Instance.SolarTermAddCombatSkillPower);
			}
		}
		ItemKey[] equipment = element_Objects.GetEquipment();
		for (int i = 8; i <= 10; i++)
		{
			ItemKey itemKey = equipment[i];
			if (itemKey.IsValid() && itemKey.ItemType == 2)
			{
				AccessoryItem accessoryItem = Config.Accessory.Instance[itemKey.TemplateId];
				if (accessoryItem.BonusCombatSkillSect == Template.SectId)
				{
					num += GlobalConfig.Instance.SectAccessoryBonusCombatSkillPower;
				}
			}
		}
		int num2 = _id.GetNeiliAllocationPowerAddPercent();
		if (DomainManager.Combat.IsInCombat() && Template.Type == 2)
		{
			short templateId = DomainManager.Combat.CombatConfig.TemplateId;
			if ((uint)(templateId - 159) <= 1u)
			{
				num2 += 200;
			}
		}
		return new CValueModify(num, CValuePercentBonus.op_Implicit(num2), default(CValuePercentBonus), default(CValuePercentBonus));
	}

	private CValueModify CalcEffectPowerModify()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		CValueModify effectAdd = DomainManager.SpecialEffect.GetModify(_id.CharId, _id.SkillTemplateId, 199, -1, -1, -1, (EDataSumType)1);
		CValueModify effectReduce = DomainManager.SpecialEffect.GetModify(_id.CharId, _id.SkillTemplateId, 199, -1, -1, -1, (EDataSumType)2);
		effectAdd = ((CValueModify)(ref effectAdd)).ChangeA(DomainManager.SpecialEffect.GetModify(_id.CharId, _id.SkillTemplateId, 257, -1, -1, -1, (EDataSumType)0));
		effectReduce = ((CValueModify)(ref effectReduce)).ChangeA(DomainManager.SpecialEffect.GetModify(_id.CharId, _id.SkillTemplateId, 258, -1, -1, -1, (EDataSumType)0));
		EDataReverseType reverseType = CalcPowerEffectReverseType();
		bool canReduce = DomainManager.SpecialEffect.ModifyData(_id.CharId, _id.SkillTemplateId, 201, dataValue: true);
		if (!DomainManager.Combat.IsInCombat())
		{
			return CalcFinalModify();
		}
		if (DomainManager.Combat.TryGetElement_CombatCharacterDict(_id.CharId, out var element))
		{
			ETeammateCommandImplement executingTeammateCommandImplement = element.ExecutingTeammateCommandImplement;
			if ((executingTeammateCommandImplement == ETeammateCommandImplement.Defend || executingTeammateCommandImplement == ETeammateCommandImplement.AttackSkill) ? true : false)
			{
				CombatCharacter mainCharacter = DomainManager.Combat.GetMainCharacter(element.IsAlly);
				CValuePercentBonus val = CValuePercentBonus.op_Implicit(DomainManager.SpecialEffect.GetModifyValue(mainCharacter.GetId(), 184, (EDataModifyType)0, (int)executingTeammateCommandImplement, -1, -1, (EDataSumType)0));
				effectAdd = ((CValueModify)(ref effectAdd)).ChangeA(element.ExecutingTeammateCommandConfig.IntArg * val);
			}
		}
		if (DomainManager.Combat.TryGetElement_SkillPowerAddInCombat(_id, out var value))
		{
			effectAdd = ((CValueModify)(ref effectAdd)).ChangeA(value.GetTotalChangeValue());
		}
		if (DomainManager.Combat.TryGetElement_SkillPowerReduceInCombat(_id, out value))
		{
			effectReduce = ((CValueModify)(ref effectReduce)).ChangeA(value.GetTotalChangeValue());
		}
		return CalcFinalModify();
		CValueModify CalcFinalModify()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			CValueModify val2 = effectAdd * reverseType;
			if (canReduce)
			{
				val2 += effectReduce * reverseType;
			}
			return val2;
		}
	}

	private EDataReverseType CalcPowerEffectReverseType()
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		int num = DomainManager.SpecialEffect.ModifyValue(_id.CharId, _id.SkillTemplateId, 292, 0);
		if (1 == 0)
		{
		}
		EDataReverseType result = ((num < 0) ? ((EDataReverseType)1) : ((num <= 0) ? ((EDataReverseType)0) : ((EDataReverseType)2)));
		if (1 == 0)
		{
		}
		return result;
	}

	public int GetCharPropertyBonus(ECharacterPropertyReferencedType propertyType)
	{
		short propertyId = (short)propertyType;
		int bonus = 0;
		bool flag = _id.CharId == DomainManager.Taiwu.GetTaiwuCharId();
		if (CombatSkillDomain.EquipAddPropertyDict[_id.SkillTemplateId] != null)
		{
			int bonusValue = CombatSkillDomain.EquipAddPropertyDict[_id.SkillTemplateId][propertyId];
			ApplyBonus(bonusValue);
		}
		foreach (SkillBreakPageEffectImplementItem pageEffect in GetPageEffects())
		{
			int mapping = pageEffect.GetMapping(propertyType);
			int bonusValue2 = CalcPageEffectValue(mapping);
			ApplyBonus(bonusValue2);
		}
		foreach (SkillBreakPlateBonus breakBonuse in GetBreakBonuses())
		{
			int bonusValue3 = breakBonuse.CalcEquipAddProperty(Template.EquipType, propertyType);
			ApplyBonus(bonusValue3);
		}
		if (flag && CombatSkillStateHelper.IsBrokenOut(_activationState))
		{
			SkillBreakBonusCollection breakGridBonusCollection = DomainManager.Taiwu.GetBreakGridBonusCollection(_id.SkillTemplateId);
			ApplyBonusCollection(breakGridBonusCollection);
		}
		if (flag && DomainManager.Extra.TryGetEmeiExtraBonusCollection(_id.SkillTemplateId, out var extraBonusCollection))
		{
			ApplyBonusCollection(extraBonusCollection);
		}
		return bonus;
		void ApplyBonus(int bonus2)
		{
			bonus += CalcCharacterPropertyBonus(propertyId, bonus2);
		}
		void ApplyBonusCollection(SkillBreakBonusCollection bonusCollection)
		{
			if (bonusCollection != null && bonusCollection.CharacterPropertyBonusDict.TryGetValue(propertyId, out var value))
			{
				ApplyBonus(value);
			}
		}
	}

	public int GetBreakoutGridCombatSkillPropertyBonus(short propertyId)
	{
		int bonus = 0;
		bool flag = _id.CharId == DomainManager.Taiwu.GetTaiwuCharId();
		if (flag && CombatSkillStateHelper.IsBrokenOut(_activationState))
		{
			SkillBreakBonusCollection breakGridBonusCollection = DomainManager.Taiwu.GetBreakGridBonusCollection(_id.SkillTemplateId);
			ApplyBonusCollection(breakGridBonusCollection);
		}
		if (flag && DomainManager.Extra.TryGetEmeiExtraBonusCollection(_id.SkillTemplateId, out var extraBonusCollection))
		{
			ApplyBonusCollection(extraBonusCollection);
		}
		return bonus;
		void ApplyBonusCollection(SkillBreakBonusCollection bonusCollection)
		{
			if (bonusCollection != null && bonusCollection.CombatSkillPropertyBonusDict.TryGetValue(propertyId, out var value))
			{
				bonus += value;
			}
		}
	}

	private int CalcPageEffectValue(int mappingValue)
	{
		return mappingValue * Template.GridCost;
	}

	private int CalcCharacterPropertyBonus(int propertyId, int bonus)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		CharacterPropertyReferencedItem characterPropertyReferencedItem = CharacterPropertyReferenced.Instance[propertyId];
		return CalcCharacterPropertyBonus(propertyId, bonus, CValuePercent.op_Implicit(characterPropertyReferencedItem.BoostedByPower ? GetPower() : 0));
	}

	private int CalcCharacterPropertyBonus(int propertyId, int bonus, CValuePercent power)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		CharacterPropertyReferencedItem characterPropertyReferencedItem = CharacterPropertyReferenced.Instance[propertyId];
		return (characterPropertyReferencedItem.BoostedByPower && bonus > 0) ? (bonus * power) : bonus;
	}

	public int CalcNeiliAllocationBonus(ECharacterPropertyReferencedType propertyType, int stepCount)
	{
		return CalcNeiliAllocationBonus(propertyType, stepCount, GetPower());
	}

	private int CalcNeiliAllocationBonus(ECharacterPropertyReferencedType propertyType, int stepCount, int power)
	{
		int mapping = Config.CombatSkill.Instance[_id.SkillTemplateId].GetMapping(propertyType);
		if (mapping <= 0)
		{
			return 0;
		}
		int combatSkillNeiliAllocationBonusPercent = GlobalConfig.Instance.CombatSkillNeiliAllocationBonusPercent;
		return mapping * stepCount * power * combatSkillNeiliAllocationBonusPercent / 10000;
	}

	public List<(short, short, bool)> GetBreakAddPropertyList(CValuePercent power)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		List<(short, short, bool)> list = new List<(short, short, bool)>();
		Dictionary<int, int> propertyIdValues = ObjectPool<Dictionary<int, int>>.Instance.Get();
		List<int> list2 = ObjectPool<List<int>>.Instance.Get();
		propertyIdValues.Clear();
		list2.Clear();
		foreach (ECharacterPropertyReferencedType bonusPropertyType in GameData.Domains.Character.Character.BonusPropertyTypes)
		{
			int propertyId = (int)bonusPropertyType;
			foreach (SkillBreakPageEffectImplementItem pageEffect in GetPageEffects())
			{
				int mapping = pageEffect.GetMapping(bonusPropertyType);
				if (mapping != 0)
				{
					int num = CalcPageEffectValue(mapping);
					if (num != 0)
					{
						AddProperty(propertyId, CalcCharacterPropertyBonus(propertyId, num, power));
					}
				}
			}
			foreach (SkillBreakPlateBonus breakBonuse in GetBreakBonuses())
			{
				int num2 = breakBonuse.CalcEquipAddProperty(Template.EquipType, bonusPropertyType);
				if (num2 != 0)
				{
					AddProperty(propertyId, CalcCharacterPropertyBonus(propertyId, num2, power));
				}
			}
		}
		if (_id.CharId == DomainManager.Taiwu.GetTaiwuCharId())
		{
			SkillBreakBonusCollection breakGridBonusCollection = DomainManager.Taiwu.GetBreakGridBonusCollection(_id.SkillTemplateId);
			if (breakGridBonusCollection != null && CombatSkillStateHelper.IsBrokenOut(_activationState))
			{
				foreach (KeyValuePair<short, short> item in breakGridBonusCollection.CharacterPropertyBonusDict)
				{
					AddProperty(item.Key, CalcCharacterBonus(item));
				}
				foreach (KeyValuePair<short, short> item2 in breakGridBonusCollection.CombatSkillPropertyBonusDict)
				{
					AddProperty(CharacterPropertyReferenced.Instance.Count + item2.Key, item2.Value);
				}
			}
			SkillBreakBonusCollection emeiBreakBonusCollection = DomainManager.Extra.GetEmeiBreakBonusCollection(_id.SkillTemplateId);
			if (emeiBreakBonusCollection != null)
			{
				foreach (KeyValuePair<short, short> item3 in emeiBreakBonusCollection.CharacterPropertyBonusDict)
				{
					AddProperty(item3.Key, CalcCharacterBonus(item3));
					list2.Add(item3.Key);
				}
				foreach (KeyValuePair<short, short> item4 in emeiBreakBonusCollection.CombatSkillPropertyBonusDict)
				{
					int num3 = CharacterPropertyReferenced.Instance.Count + item4.Key;
					AddProperty(num3, item4.Value);
					list2.Add(num3);
				}
			}
		}
		foreach (KeyValuePair<int, int> item5 in propertyIdValues)
		{
			list.Add(((short)item5.Key, (short)item5.Value, list2.Contains(item5.Key)));
		}
		ObjectPool<Dictionary<int, int>>.Instance.Return(propertyIdValues);
		ObjectPool<List<int>>.Instance.Return(list2);
		return list;
		void AddProperty(int key, int value)
		{
			propertyIdValues[key] = propertyIdValues.GetOrDefault(key) + value;
		}
		int CalcCharacterBonus(KeyValuePair<short, short> bonus)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			return CalcCharacterPropertyBonus(bonus.Key, bonus.Value, power);
		}
	}

	public List<(short, int)> GetNeiliAllocationPropertyList(int power)
	{
		CombatSkillItem config = Config.CombatSkill.Instance[_id.SkillTemplateId];
		GameData.Domains.Character.Character element;
		return DomainManager.Character.TryGetElement_Objects(_id.CharId, out element) ? config.CalcNeiliAllocationBonus(power, element.CalcNeiliAllocationStepCount) : config.CalcDefaultNeiliAllocationBonus();
	}

	public List<(int type, int required, int actual)> GetRequirementsAndActualValues(GameData.Domains.Character.Character character, bool skillExist = true)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[_id.SkillTemplateId];
		List<PropertyAndValue> usingRequirement = combatSkillItem.UsingRequirement;
		int num = (skillExist ? GetRequirementPercent() : 100);
		List<(int, int, int)> list = new List<(int, int, int)>();
		CValuePercent val = CValuePercent.op_Implicit(GlobalConfig.Instance.TreasuryGuardAttainmentPercent);
		int num2 = 0;
		if (character != null && skillExist)
		{
			sbyte equipType = combatSkillItem.EquipType;
			ref LifeSkillShorts lifeSkillAttainments = ref character.GetLifeSkillAttainments();
			foreach (SkillBreakPlateBonus breakBonuse in GetBreakBonuses())
			{
				num2 += breakBonuse.CalcAddLifeSkillRequirement(equipType, ref lifeSkillAttainments);
			}
		}
		EventArgBox globalEventArgumentBox = DomainManager.TaiwuEvent.GetGlobalEventArgumentBox();
		bool arg = false;
		globalEventArgumentBox.Get("IsGuardCombat", ref arg);
		for (int i = 0; i < usingRequirement.Count; i++)
		{
			PropertyAndValue propertyAndValue = usingRequirement[i];
			ECharacterPropertyReferencedType propertyId = (ECharacterPropertyReferencedType)propertyAndValue.PropertyId;
			int num3 = character?.GetPropertyValue(propertyId) ?? 0;
			if (propertyId.IsLifeSkillTypeAttainment())
			{
				num3 += num2;
			}
			if (arg && character != null && character.IsTreasuryGuard())
			{
				foreach (CombatCharacter teammateCharacter in DomainManager.Combat.GetTeammateCharacters(character.GetId()))
				{
					num3 += teammateCharacter.GetCharacter().GetPropertyValue(propertyId) * val;
				}
			}
			list.Add((propertyAndValue.PropertyId, propertyAndValue.Value * num / 100, num3));
		}
		int value;
		int item = (DomainManager.Extra.TryGetElement_CombatSkillProficiencies(_id, out value) ? value : 0);
		list.Add((110, 300, item));
		return list;
	}

	public bool CanBreakout()
	{
		return CombatSkillStateHelper.HasReadOutlinePages(_readingState) && CombatSkillStateHelper.IsReadNormalPagesMeetConditionOfBreakout(_readingState) && !_revoked;
	}

	public void ObtainNeili(DataContext context, short obtainedNeili)
	{
		short obtainedNeili2 = _obtainedNeili;
		short totalObtainableNeili = GetTotalObtainableNeili();
		if (_obtainedNeili < totalObtainableNeili)
		{
			_obtainedNeili += obtainedNeili;
			if (_obtainedNeili > totalObtainableNeili)
			{
				_obtainedNeili = totalObtainableNeili;
			}
			if (_obtainedNeili != obtainedNeili2)
			{
				SetObtainedNeili(_obtainedNeili, context);
			}
		}
	}

	public sbyte GetBaseInnerRatio()
	{
		sbyte baseInnerRatio = Config.CombatSkill.Instance[_id.SkillTemplateId].BaseInnerRatio;
		return (sbyte)Math.Clamp(baseInnerRatio + GetBreakoutGridCombatSkillPropertyBonus(4), 0, 100);
	}

	public sbyte GetInnerRatioChangeRange()
	{
		int innerRatioChangeRange = Config.CombatSkill.Instance[_id.SkillTemplateId].InnerRatioChangeRange;
		innerRatioChangeRange += GetBreakoutGridCombatSkillPropertyBonus(5);
		foreach (SkillBreakPlateBonus breakBonuse in GetBreakBonuses())
		{
			innerRatioChangeRange += breakBonuse.CalcInnerRatioChangeRange(Template.EquipType);
		}
		return (sbyte)Math.Clamp(innerRatioChangeRange, 0, 100);
	}

	public short GetTotalObtainableNeili()
	{
		int totalObtainableNeili = Config.CombatSkill.Instance[_id.SkillTemplateId].TotalObtainableNeili;
		totalObtainableNeili += GetBreakoutGridCombatSkillPropertyBonus(6);
		foreach (SkillBreakPlateBonus breakBonuse in GetBreakBonuses())
		{
			totalObtainableNeili += breakBonuse.CalcTotalObtainableNeili();
		}
		return (short)Math.Clamp(totalObtainableNeili, 0, 32767);
	}

	public sbyte GetFiveElementsChange()
	{
		sbyte fiveElementChangePerLoop = Config.CombatSkill.Instance[_id.SkillTemplateId].FiveElementChangePerLoop;
		return (sbyte)(fiveElementChangePerLoop + GetBreakoutGridCombatSkillPropertyBonus(8));
	}

	public sbyte[] GetSpecificGridCount(bool preview = false)
	{
		sbyte[] array = new sbyte[4];
		GetSpecificGridCount(array, preview);
		return array;
	}

	public void GetSpecificGridCount(Span<sbyte> specificGrids, bool preview = false)
	{
		specificGrids.Fill(0);
		for (sbyte b = 1; b < 5; b++)
		{
			specificGrids[b - 1] = GetSpecificGridCount(b, preview);
		}
	}

	public sbyte GetSpecificGridCount(sbyte equipType, bool preview = false)
	{
		int num = equipType - 1;
		sbyte b = (sbyte)GetBreakoutGridCombatSkillPropertyBonus((short)(49 + num));
		bool flag = DomainManager.Extra.IsCombatSkillMasteredByCharacter(_id.CharId, _id.SkillTemplateId);
		if (preview)
		{
			flag = !flag;
		}
		if (flag)
		{
			return b;
		}
		foreach (SkillBreakPlateBonus breakBonuse in GetBreakBonuses())
		{
			b += breakBonuse.CalcSpecificGridCount(equipType);
		}
		sbyte[] specificGrids = Config.CombatSkill.Instance[_id.SkillTemplateId].SpecificGrids;
		b += specificGrids[num];
		return (sbyte)(b + (sbyte)DomainManager.SpecialEffect.GetModifyValue(_id.CharId, _id.SkillTemplateId, 213, (EDataModifyType)0, num, -1, -1, (EDataSumType)0));
	}

	public sbyte GetGenericGridCount(bool preview = false)
	{
		bool flag = DomainManager.Extra.IsCombatSkillMasteredByCharacter(_id.CharId, _id.SkillTemplateId);
		if (preview)
		{
			flag = !flag;
		}
		if (flag)
		{
			return Config.CombatSkill.Instance[_id.SkillTemplateId].GridCost;
		}
		int num = Config.CombatSkill.Instance[_id.SkillTemplateId].GenericGrid + GetBreakoutGridCombatSkillPropertyBonus(9);
		num += DomainManager.SpecialEffect.GetModifyValue(_id.CharId, _id.SkillTemplateId, 214, (EDataModifyType)0, -1, -1, -1, (EDataSumType)0);
		num = Math.Max(num, 0);
		return (sbyte)num;
	}

	private short GetTotalHit()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		int totalHit = Config.CombatSkill.Instance[_id.SkillTemplateId].TotalHit;
		totalHit += GetBreakoutGridCombatSkillPropertyBonus(73);
		CValuePercentBonus val = CValuePercentBonus.op_Implicit(GetBreakoutGridCombatSkillPropertyBonus(30));
		foreach (SkillBreakPageEffectImplementItem pageEffect in GetPageEffects())
		{
			val += CValuePercentBonus.op_Implicit(pageEffect.HitFactor);
		}
		foreach (SkillBreakPlateBonus breakBonuse in GetBreakBonuses())
		{
			val += CValuePercentBonus.op_Implicit(breakBonuse.CalcTotalHit());
		}
		return (short)(totalHit * val);
	}

	public int GetPrepareTotalProgress()
	{
		int prepareTotalProgress = Config.CombatSkill.Instance[_id.SkillTemplateId].PrepareTotalProgress;
		int modifyValue = DomainManager.SpecialEffect.GetModifyValue(_id.CharId, _id.SkillTemplateId, 212, (EDataModifyType)1, -1, -1, -1, (EDataSumType)0);
		int num = GetBreakoutGridCombatSkillPropertyBonus(2);
		foreach (SkillBreakPageEffectImplementItem pageEffect in GetPageEffects())
		{
			num += pageEffect.CastFrame;
		}
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(_id.CharId);
		ref LifeSkillShorts lifeSkillAttainments = ref element_Objects.GetLifeSkillAttainments();
		foreach (SkillBreakPlateBonus breakBonuse in GetBreakBonuses())
		{
			num -= breakBonuse.CalcReduceCastFrame(Template.EquipType, ref lifeSkillAttainments);
		}
		return Math.Max(prepareTotalProgress * (100 + modifyValue + num) / 100, 0);
	}

	public short GetDistanceAdditionWhenCast(bool forward)
	{
		int distanceAdditionWhenCast = Config.CombatSkill.Instance[_id.SkillTemplateId].DistanceAdditionWhenCast;
		distanceAdditionWhenCast += GetBreakoutGridCombatSkillPropertyBonus(34);
		foreach (SkillBreakPageEffectImplementItem pageEffect in GetPageEffects())
		{
			distanceAdditionWhenCast += (forward ? pageEffect.AttackRangeForward : pageEffect.AttackRangeBackward);
		}
		foreach (SkillBreakPlateBonus breakBonuse in GetBreakBonuses())
		{
			distanceAdditionWhenCast += breakBonuse.CalcAddAttackRange(forward);
		}
		return (short)distanceAdditionWhenCast;
	}

	public short GetContinuousFrames()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		short continuousFrames = Config.CombatSkill.Instance[_id.SkillTemplateId].ContinuousFrames;
		CValuePercentBonus val = CValuePercentBonus.op_Implicit(GetBreakoutGridCombatSkillPropertyBonus(28));
		return (short)((int)continuousFrames * val);
	}

	public short GetBounceDistance()
	{
		short bounceDistance = Config.CombatSkill.Instance[_id.SkillTemplateId].BounceDistance;
		return (short)(bounceDistance + GetBreakoutGridCombatSkillPropertyBonus(27));
	}

	public unsafe HitOrAvoidInts GetHitDistribution()
	{
		sbyte[] perHitDamageRateDistribution = Config.CombatSkill.Instance[_id.SkillTemplateId].PerHitDamageRateDistribution;
		HitOrAvoidInts hitOrAvoidInts = default(HitOrAvoidInts);
		for (sbyte b = 0; b < 4; b++)
		{
			sbyte b2 = perHitDamageRateDistribution[b];
			if (b < 3)
			{
				b2 = (sbyte)(b2 + GetBreakoutGridCombatSkillPropertyBonus((short)(31 + b)));
			}
			hitOrAvoidInts.Items[b] = b2;
		}
		int num = 0;
		for (sbyte b3 = 0; b3 < 4; b3++)
		{
			num += hitOrAvoidInts.Items[b3];
		}
		if (num != 100)
		{
			for (sbyte b4 = 0; b4 < 4; b4++)
			{
				hitOrAvoidInts.Items[b4] = perHitDamageRateDistribution[b4];
			}
		}
		hitOrAvoidInts = DomainManager.SpecialEffect.ModifyData(_id.CharId, _id.SkillTemplateId, 224, hitOrAvoidInts);
		return hitOrAvoidInts;
	}

	public IEnumerable<int> GetBodyPartWeights()
	{
		sbyte[] configRate = Config.CombatSkill.Instance[_id.SkillTemplateId].InjuryPartAtkRateDistribution;
		CValuePercentBonus chestBonus = CValuePercentBonus.op_Implicit(GetBreakoutGridCombatSkillPropertyBonus(35));
		CValuePercentBonus bellyBonus = CValuePercentBonus.op_Implicit(GetBreakoutGridCombatSkillPropertyBonus(36));
		CValuePercentBonus headBonus = CValuePercentBonus.op_Implicit(GetBreakoutGridCombatSkillPropertyBonus(37));
		CValuePercentBonus handBonus = CValuePercentBonus.op_Implicit(GetBreakoutGridCombatSkillPropertyBonus(38));
		CValuePercentBonus legBonus = CValuePercentBonus.op_Implicit(GetBreakoutGridCombatSkillPropertyBonus(39));
		for (sbyte i = 0; i < 7; i++)
		{
			if (1 == 0)
			{
			}
			CValuePercentBonus val;
			switch (i)
			{
			case 0:
				val = chestBonus;
				break;
			case 1:
				val = bellyBonus;
				break;
			case 2:
				val = headBonus;
				break;
			case 3:
			case 4:
				val = handBonus;
				break;
			case 5:
			case 6:
				val = legBonus;
				break;
			default:
				val = CValuePercentBonus.op_Implicit(0);
				break;
			}
			if (1 == 0)
			{
			}
			CValuePercentBonus bonus = val;
			yield return (int)configRate[i] * bonus;
		}
	}

	private int[] GetConfigAffectRequirePower()
	{
		sbyte direction = GetDirection();
		if ((direction < 0 || direction >= 2) ? true : false)
		{
			return null;
		}
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[_id.SkillTemplateId];
		SpecialEffectItem specialEffectItem = Config.SpecialEffect.Instance[(direction == 0) ? combatSkillItem.DirectEffectID : combatSkillItem.ReverseEffectID];
		return specialEffectItem.AffectRequirePower;
	}

	public unsafe int GetSumMax2HitDistribution()
	{
		HitOrAvoidInts hitDistribution = GetHitDistribution();
		int num = hitDistribution.Items[0];
		int num2 = hitDistribution.Items[1];
		for (int i = 2; i < 4; i++)
		{
			int num3 = hitDistribution.Items[i];
			if (num > num2)
			{
				if (num3 > num2)
				{
					num2 = num3;
				}
			}
			else if (num3 > num)
			{
				num = num3;
			}
		}
		return Math.Max(num, 0) + Math.Max(num2, 0);
	}

	public bool AnyAffectRequirePower()
	{
		int[] configAffectRequirePower = GetConfigAffectRequirePower();
		return configAffectRequirePower != null && configAffectRequirePower.Length > 0;
	}

	public IEnumerable<int> GetAffectRequirePower()
	{
		int[] configAffectRequirePower = GetConfigAffectRequirePower();
		if (configAffectRequirePower != null && configAffectRequirePower.Length > 0)
		{
			int sumMax2HitDistribution = GetSumMax2HitDistribution();
			int[] array = configAffectRequirePower;
			foreach (int requirePower in array)
			{
				yield return (requirePower >= 0) ? requirePower : sumMax2HitDistribution;
			}
		}
	}

	public bool PowerMatchAffectRequire(int power, int index)
	{
		int num = 0;
		foreach (int item in GetAffectRequirePower())
		{
			if (num++ == index)
			{
				return power >= item;
			}
		}
		PredefinedLog.Show(8, $"{_id.SkillTemplateId} do not has require power in index={index}, match is always true");
		return true;
	}

	public int GetAffectRequirePower(int index)
	{
		int num = 0;
		foreach (int item in GetAffectRequirePower())
		{
			if (num++ == index)
			{
				return item;
			}
		}
		PredefinedLog.Show(8, $"{_id.SkillTemplateId} do not has require power in index={index}, require power is always zero");
		return 0;
	}

	public unsafe PoisonsAndLevels GetPoisons()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		PoisonsAndLevels poisons = Config.CombatSkill.Instance[_id.SkillTemplateId].Poisons;
		for (sbyte b = 0; b < 6; b++)
		{
			CValuePercentBonus val = CValuePercentBonus.op_Implicit(GetBreakoutGridCombatSkillPropertyBonus((short)(42 + b)));
			foreach (SkillBreakPlateBonus breakBonuse in GetBreakBonuses())
			{
				val += CValuePercentBonus.op_Implicit(breakBonuse.CalcPoison(b));
			}
			poisons.Values[b] = (short)((int)poisons.Values[b] * val);
		}
		return poisons;
	}

	public (sbyte type, sbyte value) GetCostNeiliAllocation()
	{
		return DomainManager.SpecialEffect.ModifyData(_id.CharId, _id.SkillTemplateId, (ushort)231, ((sbyte)(-1), (sbyte)0), -1, -1, -1);
	}

	public SpecialEffectItem TryGetSpecialEffect()
	{
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[_id.SkillTemplateId];
		sbyte direction = GetDirection();
		if (1 == 0)
		{
		}
		SpecialEffectItem result = direction switch
		{
			0 => Config.SpecialEffect.Instance[combatSkillItem.DirectEffectID], 
			1 => Config.SpecialEffect.Instance[combatSkillItem.ReverseEffectID], 
			_ => null, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public int GetReadNormalPagesCount()
	{
		return CombatSkillStateHelper.GetReadNormalPagesCount(_readingState);
	}

	public IEnumerable<SkillBreakPageEffectImplementItem> GetPageEffects()
	{
		foreach (SkillBreakPageEffectItem effect in (IEnumerable<SkillBreakPageEffectItem>)SkillBreakPageEffect.Instance)
		{
			sbyte direction = ((!effect.IsDirect) ? ((sbyte)1) : ((sbyte)0));
			if (CombatSkillStateHelper.GetPageActiveDirection(_activationState, effect.PageId) == direction)
			{
				sbyte equipType = Template.EquipType;
				if (1 == 0)
				{
				}
				int num = equipType switch
				{
					0 => effect.EffectNeigong, 
					1 => effect.EffectAttack, 
					2 => effect.EffectAgile, 
					3 => effect.EffectDefense, 
					4 => effect.EffectAssist, 
					_ => -1, 
				};
				if (1 == 0)
				{
				}
				int implementId = num;
				if (implementId >= 0)
				{
					yield return SkillBreakPageEffectImplement.Instance[implementId];
				}
			}
		}
	}

	public IEnumerable<SkillBreakPlateBonus> GetBreakBonuses()
	{
		if (!CombatSkillStateHelper.IsBrokenOut(_activationState))
		{
			return Enumerable.Empty<SkillBreakPlateBonus>();
		}
		short skillTemplateId = _id.SkillTemplateId;
		IEnumerable<SkillBreakPlateBonus> enumerable = null;
		GameData.Domains.Taiwu.SkillBreakPlate breakPlate;
		if (DomainManager.Character.TryGetElement_Objects(_id.CharId, out var element) && element.IsGearMate)
		{
			enumerable = DomainManager.Extra.GetGearMateById(_id.CharId).SkillBreakBonusDict.GetOrDefault(skillTemplateId);
		}
		else if (_id.CharId != DomainManager.Taiwu.GetTaiwuCharId())
		{
			enumerable = DomainManager.Extra.GetCharacterSkillBreakBonuses(_id.CharId, skillTemplateId).Items;
		}
		else if (DomainManager.Taiwu.TryGetSkillBreakPlate(skillTemplateId, out breakPlate))
		{
			enumerable = breakPlate.GetBonuses();
		}
		return enumerable ?? Enumerable.Empty<SkillBreakPlateBonus>();
	}

	public int CalcInjuryDamageStep(bool inner, sbyte bodyPart)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		int num = (inner ? Template.InnerDamageSteps : Template.OuterDamageSteps)[bodyPart];
		CValuePercentBonus val = CValuePercentBonus.op_Implicit(CalcInjuryDamageStepBonus(inner));
		return num * val;
	}

	public int CalcFatalDamageStep()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		int fatalDamageStep = Template.FatalDamageStep;
		CValuePercentBonus val = CValuePercentBonus.op_Implicit(CalcFatalDamageStepBonus());
		return fatalDamageStep * val;
	}

	public int CalcMindDamageStep()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		int mindDamageStep = Template.MindDamageStep;
		CValuePercentBonus val = CValuePercentBonus.op_Implicit(CalcMindDamageStepBonus());
		return mindDamageStep * val;
	}

	public CombatSkillDamageStepBonusDisplayData CalcStepBonusDisplayData()
	{
		return new CombatSkillDamageStepBonusDisplayData
		{
			InnerInjuryStepBonus = CalcInjuryDamageStepBonus(inner: true),
			OuterInjuryStepBonus = CalcInjuryDamageStepBonus(inner: false),
			FatalStepBonus = CalcFatalDamageStepBonus(),
			MindStepBonus = CalcMindDamageStepBonus()
		};
	}

	private int CalcInjuryDamageStepBonus(bool inner)
	{
		int num = 0;
		foreach (SkillBreakPlateBonus breakBonuse in GetBreakBonuses())
		{
			num += breakBonuse.CalcAddInjuryStep(Template.EquipType, inner);
		}
		return num;
	}

	private int CalcFatalDamageStepBonus()
	{
		int num = 0;
		foreach (SkillBreakPlateBonus breakBonuse in GetBreakBonuses())
		{
			num += breakBonuse.CalcAddFatalStep(Template.EquipType);
		}
		return num;
	}

	private int CalcMindDamageStepBonus()
	{
		int num = 0;
		foreach (SkillBreakPlateBonus breakBonuse in GetBreakBonuses())
		{
			num += breakBonuse.CalcAddMindStep(Template.EquipType);
		}
		return num;
	}

	public int GetMakeDamageBreakBonus()
	{
		int num = 0;
		foreach (SkillBreakPageEffectImplementItem pageEffect in GetPageEffects())
		{
			num += pageEffect.MakeDamage;
		}
		foreach (SkillBreakPlateBonus breakBonuse in GetBreakBonuses())
		{
			num += breakBonuse.CalcMakeDamage();
		}
		return num;
	}

	public int GetAcceptDirectDamageBreakBonus(bool anyFatal)
	{
		int num = 0;
		foreach (SkillBreakPageEffectImplementItem pageEffect in GetPageEffects())
		{
			num += (anyFatal ? pageEffect.AcceptDirectDamageOnFatal : pageEffect.AcceptDirectDamageNoFatal);
		}
		return num;
	}

	public CombatSkill(int charId, short skillTemplateId, ushort readingState = 0)
	{
		_id = new CombatSkillKey(charId, skillTemplateId);
		_readingState = readingState;
		_activationState = 0;
		_forcedBreakoutStepsCount = 0;
		_breakoutStepsCount = 0;
		_innerRatio = Config.CombatSkill.Instance[skillTemplateId].BaseInnerRatio;
		_obtainedNeili = 0;
		_revoked = false;
		_specialEffectId = -1L;
	}

	public CombatSkill(IRandomSource random, int charId, short skillTemplateId, sbyte outlinePageType, sbyte directPagesReadCount, sbyte reversePagesReadCount)
	{
		_id = new CombatSkillKey(charId, skillTemplateId);
		_readingState = 0;
		_activationState = 0;
		_forcedBreakoutStepsCount = 0;
		_breakoutStepsCount = 0;
		_innerRatio = Config.CombatSkill.Instance[skillTemplateId].BaseInnerRatio;
		_obtainedNeili = 0;
		_revoked = false;
		_specialEffectId = -1L;
		if (outlinePageType >= 0)
		{
			byte outlinePageInternalIndex = CombatSkillStateHelper.GetOutlinePageInternalIndex(outlinePageType);
			_readingState = CombatSkillStateHelper.SetPageRead(_readingState, outlinePageInternalIndex);
		}
		else if (outlinePageType == -2)
		{
			sbyte randomBehaviorType = GameData.Domains.Character.BehaviorType.GetRandomBehaviorType(random);
			byte outlinePageInternalIndex2 = CombatSkillStateHelper.GetOutlinePageInternalIndex(randomBehaviorType);
			_readingState = CombatSkillStateHelper.SetPageRead(_readingState, outlinePageInternalIndex2);
		}
		if (directPagesReadCount > 0)
		{
			OfflineSetRandomNormalPagesRead(random, 0, directPagesReadCount);
		}
		if (reversePagesReadCount > 0)
		{
			OfflineComplementNormalPages(random, 1, reversePagesReadCount);
		}
	}

	public CombatSkill(IRandomSource random, PresetCombatSkill presetSkill)
	{
		_id = new CombatSkillKey(-1, presetSkill.SkillTemplateId);
		_readingState = 0;
		_activationState = 0;
		_forcedBreakoutStepsCount = 0;
		_breakoutStepsCount = 0;
		_innerRatio = Config.CombatSkill.Instance[presetSkill.SkillTemplateId].BaseInnerRatio;
		_obtainedNeili = 0;
		_revoked = false;
		_specialEffectId = -1L;
		if (presetSkill.OutlinePagesReadCount > 0)
		{
			OfflineSetRandomOutlinePagesRead(random, presetSkill.OutlinePagesReadCount);
		}
		else if (presetSkill.OutlinePagesReadCount < 0)
		{
			OfflineSetOutlinePagesRead(presetSkill.OutlinePagesReadStates);
		}
		if (presetSkill.DirectPagesReadCount > 0)
		{
			OfflineSetRandomNormalPagesRead(random, 0, presetSkill.DirectPagesReadCount);
		}
		else if (presetSkill.DirectPagesReadCount < 0)
		{
			OfflineSetNormalPagesRead(0, presetSkill.DirectPagesReadStates);
		}
		if (presetSkill.ReversePagesReadCount > 0)
		{
			OfflineComplementNormalPages(random, 1, presetSkill.ReversePagesReadCount);
		}
		else if (presetSkill.ReversePagesReadCount < 0)
		{
			OfflineSetNormalPagesRead(1, presetSkill.ReversePagesReadStates);
		}
	}

	public void OfflineSetCharId(int charId)
	{
		_id.CharId = charId;
	}

	public void OfflineSetSpecialEffectId(long specialEffectId)
	{
		_specialEffectId = specialEffectId;
	}

	private unsafe void OfflineSetRandomOutlinePagesRead(IRandomSource random, sbyte readPagesCount)
	{
		byte* ptr = stackalloc byte[5];
		for (byte b = 0; b < 5; b++)
		{
			ptr[(int)b] = b;
		}
		byte* ptr2 = CollectionUtils.Shuffle(random, ptr, 5, readPagesCount);
		for (byte* ptr3 = ptr2; ptr3 < ptr2 + readPagesCount; ptr3++)
		{
			_readingState = CombatSkillStateHelper.SetPageRead(_readingState, *ptr3);
		}
	}

	private void OfflineSetOutlinePagesRead(bool[] pagesReadStates)
	{
		byte b = 0;
		byte b2 = 5;
		while (b < b2)
		{
			if (pagesReadStates[b])
			{
				_readingState = CombatSkillStateHelper.SetPageRead(_readingState, b);
			}
			b++;
		}
	}

	private unsafe void OfflineSetRandomNormalPagesRead(IRandomSource random, sbyte direction, int readPagesCount)
	{
		byte* ptr = stackalloc byte[5];
		for (int i = 0; i < 5; i++)
		{
			ptr[i] = (byte)(i + 1);
		}
		byte* ptr2 = CollectionUtils.Shuffle(random, ptr, 5, readPagesCount);
		for (byte* ptr3 = ptr2; ptr3 < ptr2 + readPagesCount; ptr3++)
		{
			byte normalPageInternalIndex = CombatSkillStateHelper.GetNormalPageInternalIndex(direction, *ptr3);
			_readingState = CombatSkillStateHelper.SetPageRead(_readingState, normalPageInternalIndex);
		}
	}

	private void OfflineComplementNormalPages(IRandomSource random, sbyte direction, int readPagesCount)
	{
		Span<byte> span = stackalloc byte[5];
		SpanList<byte> spanList = span;
		span = stackalloc byte[5];
		SpanList<byte> spanList2 = span;
		for (int i = 0; i < 5; i++)
		{
			byte b = (byte)(i + 1);
			byte normalPageInternalIndex = CombatSkillStateHelper.GetNormalPageInternalIndex(direction, b);
			if (CombatSkillStateHelper.IsPageRead(_readingState, normalPageInternalIndex))
			{
				spanList2.Add(b);
			}
			else
			{
				spanList.Add(b);
			}
		}
		spanList.Shuffle(random);
		int num = 0;
		SpanList<byte>.Enumerator enumerator = spanList.GetEnumerator();
		while (enumerator.MoveNext())
		{
			byte current = enumerator.Current;
			if (num >= readPagesCount)
			{
				return;
			}
			byte normalPageInternalIndex2 = CombatSkillStateHelper.GetNormalPageInternalIndex(direction, current);
			_readingState = CombatSkillStateHelper.SetPageRead(_readingState, normalPageInternalIndex2);
			num++;
		}
		spanList2.Shuffle(random);
		SpanList<byte>.Enumerator enumerator2 = spanList2.GetEnumerator();
		while (enumerator2.MoveNext())
		{
			byte current2 = enumerator2.Current;
			if (num >= readPagesCount)
			{
				break;
			}
			byte normalPageInternalIndex3 = CombatSkillStateHelper.GetNormalPageInternalIndex(direction, current2);
			_readingState = CombatSkillStateHelper.SetPageRead(_readingState, normalPageInternalIndex3);
			num++;
		}
	}

	private void OfflineSetNormalPagesRead(sbyte direction, bool[] pagesReadStates)
	{
		int i = 0;
		for (int num = 5; i < num; i++)
		{
			if (pagesReadStates[i])
			{
				byte pageId = (byte)(i + 1);
				byte normalPageInternalIndex = CombatSkillStateHelper.GetNormalPageInternalIndex(direction, pageId);
				_readingState = CombatSkillStateHelper.SetPageRead(_readingState, normalPageInternalIndex);
			}
		}
	}

	public unsafe static ushort GenerateRandomReadingState(IRandomSource random, byte bookPageTypes, int readPagesCount)
	{
		ushort num = 0;
		if (readPagesCount > 0)
		{
			sbyte outlinePageType = SkillBookStateHelper.GetOutlinePageType(bookPageTypes);
			byte outlinePageInternalIndex = CombatSkillStateHelper.GetOutlinePageInternalIndex(outlinePageType);
			num = CombatSkillStateHelper.SetPageRead(num, outlinePageInternalIndex);
			readPagesCount--;
		}
		if (readPagesCount > 0)
		{
			byte* ptr = stackalloc byte[5];
			for (int i = 0; i < 5; i++)
			{
				ptr[i] = (byte)(i + 1);
			}
			byte* ptr2 = CollectionUtils.Shuffle(random, ptr, 5, readPagesCount);
			for (byte* ptr3 = ptr2; ptr3 < ptr2 + readPagesCount; ptr3++)
			{
				byte pageId = *ptr3;
				sbyte normalPageType = SkillBookStateHelper.GetNormalPageType(bookPageTypes, pageId);
				byte normalPageInternalIndex = CombatSkillStateHelper.GetNormalPageInternalIndex(normalPageType, pageId);
				num = CombatSkillStateHelper.SetPageRead(num, normalPageInternalIndex);
			}
		}
		return num;
	}

	public CombatSkillKey GetId()
	{
		return _id;
	}

	public sbyte GetPracticeLevel()
	{
		return _practiceLevel;
	}

	public unsafe void SetPracticeLevel(sbyte practiceLevel, DataContext context)
	{
		_practiceLevel = practiceLevel;
		SetModifiedAndInvalidateInfluencedCache(1, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 8u, 1);
			*ptr = (byte)_practiceLevel;
			ptr++;
		}
	}

	public ushort GetReadingState()
	{
		return _readingState;
	}

	public unsafe void SetReadingState(ushort readingState, DataContext context)
	{
		_readingState = readingState;
		SetModifiedAndInvalidateInfluencedCache(2, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 9u, 2);
			*(ushort*)ptr = _readingState;
			ptr += 2;
		}
	}

	public ushort GetActivationState()
	{
		return _activationState;
	}

	public unsafe void SetActivationState(ushort activationState, DataContext context)
	{
		_activationState = activationState;
		SetModifiedAndInvalidateInfluencedCache(3, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 11u, 2);
			*(ushort*)ptr = _activationState;
			ptr += 2;
		}
	}

	public sbyte GetForcedBreakoutStepsCount()
	{
		return _forcedBreakoutStepsCount;
	}

	public unsafe void SetForcedBreakoutStepsCount(sbyte forcedBreakoutStepsCount, DataContext context)
	{
		_forcedBreakoutStepsCount = forcedBreakoutStepsCount;
		SetModifiedAndInvalidateInfluencedCache(4, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 13u, 1);
			*ptr = (byte)_forcedBreakoutStepsCount;
			ptr++;
		}
	}

	public sbyte GetBreakoutStepsCount()
	{
		return _breakoutStepsCount;
	}

	public unsafe void SetBreakoutStepsCount(sbyte breakoutStepsCount, DataContext context)
	{
		_breakoutStepsCount = breakoutStepsCount;
		SetModifiedAndInvalidateInfluencedCache(5, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 14u, 1);
			*ptr = (byte)_breakoutStepsCount;
			ptr++;
		}
	}

	public sbyte GetInnerRatio()
	{
		return _innerRatio;
	}

	public unsafe void SetInnerRatio(sbyte innerRatio, DataContext context)
	{
		_innerRatio = innerRatio;
		SetModifiedAndInvalidateInfluencedCache(6, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 15u, 1);
			*ptr = (byte)_innerRatio;
			ptr++;
		}
	}

	public short GetObtainedNeili()
	{
		return _obtainedNeili;
	}

	public unsafe void SetObtainedNeili(short obtainedNeili, DataContext context)
	{
		_obtainedNeili = obtainedNeili;
		SetModifiedAndInvalidateInfluencedCache(7, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 16u, 2);
			*(short*)ptr = _obtainedNeili;
			ptr += 2;
		}
	}

	public bool GetRevoked()
	{
		return _revoked;
	}

	public unsafe void SetRevoked(bool revoked, DataContext context)
	{
		_revoked = revoked;
		SetModifiedAndInvalidateInfluencedCache(8, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 18u, 1);
			*ptr = (_revoked ? ((byte)1) : ((byte)0));
			ptr++;
		}
	}

	public long GetSpecialEffectId()
	{
		return _specialEffectId;
	}

	public unsafe void SetSpecialEffectId(long specialEffectId, DataContext context)
	{
		_specialEffectId = specialEffectId;
		SetModifiedAndInvalidateInfluencedCache(9, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 19u, 8);
			*(long*)ptr = _specialEffectId;
			ptr += 8;
		}
	}

	public short GetPower()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 10))
		{
			return _power;
		}
		short power = CalcPower();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_power = power;
			dataStates.SetCached(DataStatesOffset, 10);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _power;
	}

	public short GetMaxPower()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 11))
		{
			return _maxPower;
		}
		short maxPower = CalcMaxPower();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_maxPower = maxPower;
			dataStates.SetCached(DataStatesOffset, 11);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _maxPower;
	}

	public short GetRequirementPercent()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 12))
		{
			return _requirementPercent;
		}
		short requirementPercent = CalcRequirementPercent();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_requirementPercent = requirementPercent;
			dataStates.SetCached(DataStatesOffset, 12);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _requirementPercent;
	}

	public sbyte GetDirection()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 13))
		{
			return _direction;
		}
		sbyte direction = CalcDirection();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_direction = direction;
			dataStates.SetCached(DataStatesOffset, 13);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _direction;
	}

	public short GetBaseScore()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 14))
		{
			return _baseScore;
		}
		short baseScore = CalcBaseScore();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_baseScore = baseScore;
			dataStates.SetCached(DataStatesOffset, 14);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _baseScore;
	}

	public sbyte GetCurrInnerRatio()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 15))
		{
			return _currInnerRatio;
		}
		sbyte currInnerRatio = CalcCurrInnerRatio();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_currInnerRatio = currInnerRatio;
			dataStates.SetCached(DataStatesOffset, 15);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _currInnerRatio;
	}

	public HitOrAvoidInts GetHitValue()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 16))
		{
			return _hitValue;
		}
		HitOrAvoidInts hitValue = CalcHitValue();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_hitValue = hitValue;
			dataStates.SetCached(DataStatesOffset, 16);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _hitValue;
	}

	public OuterAndInnerInts GetPenetrations()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 17))
		{
			return _penetrations;
		}
		OuterAndInnerInts penetrations = CalcPenetrations();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_penetrations = penetrations;
			dataStates.SetCached(DataStatesOffset, 17);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _penetrations;
	}

	public sbyte GetCostBreathAndStancePercent()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 18))
		{
			return _costBreathAndStancePercent;
		}
		sbyte costBreathAndStancePercent = CalcCostBreathAndStancePercent();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_costBreathAndStancePercent = costBreathAndStancePercent;
			dataStates.SetCached(DataStatesOffset, 18);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _costBreathAndStancePercent;
	}

	public sbyte GetCostBreathPercent()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 19))
		{
			return _costBreathPercent;
		}
		sbyte costBreathPercent = CalcCostBreathPercent();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_costBreathPercent = costBreathPercent;
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
		return _costBreathPercent;
	}

	public sbyte GetCostStancePercent()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 20))
		{
			return _costStancePercent;
		}
		sbyte costStancePercent = CalcCostStancePercent();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_costStancePercent = costStancePercent;
			dataStates.SetCached(DataStatesOffset, 20);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _costStancePercent;
	}

	public sbyte GetCostMobilityPercent()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 21))
		{
			return _costMobilityPercent;
		}
		sbyte costMobilityPercent = CalcCostMobilityPercent();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_costMobilityPercent = costMobilityPercent;
			dataStates.SetCached(DataStatesOffset, 21);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _costMobilityPercent;
	}

	public HitOrAvoidInts GetAddHitValueOnCast()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 22))
		{
			return _addHitValueOnCast;
		}
		HitOrAvoidInts addHitValueOnCast = CalcAddHitValueOnCast();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_addHitValueOnCast = addHitValueOnCast;
			dataStates.SetCached(DataStatesOffset, 22);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _addHitValueOnCast;
	}

	public OuterAndInnerInts GetAddPenetrateResist()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 23))
		{
			return _addPenetrateResist;
		}
		OuterAndInnerInts addPenetrateResist = CalcAddPenetrateResist();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_addPenetrateResist = addPenetrateResist;
			dataStates.SetCached(DataStatesOffset, 23);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _addPenetrateResist;
	}

	public HitOrAvoidInts GetAddAvoidValueOnCast()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 24))
		{
			return _addAvoidValueOnCast;
		}
		HitOrAvoidInts addAvoidValueOnCast = CalcAddAvoidValueOnCast();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_addAvoidValueOnCast = addAvoidValueOnCast;
			dataStates.SetCached(DataStatesOffset, 24);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _addAvoidValueOnCast;
	}

	public int GetFightBackPower()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 25))
		{
			return _fightBackPower;
		}
		int fightBackPower = CalcFightBackPower();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_fightBackPower = fightBackPower;
			dataStates.SetCached(DataStatesOffset, 25);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _fightBackPower;
	}

	public OuterAndInnerInts GetBouncePower()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 26))
		{
			return _bouncePower;
		}
		OuterAndInnerInts bouncePower = CalcBouncePower();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_bouncePower = bouncePower;
			dataStates.SetCached(DataStatesOffset, 26);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _bouncePower;
	}

	public short GetRequirementsPower()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 27))
		{
			return _requirementsPower;
		}
		short requirementsPower = CalcRequirementsPower();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_requirementsPower = requirementsPower;
			dataStates.SetCached(DataStatesOffset, 27);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _requirementsPower;
	}

	public int GetPlateAddMaxPower()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 28))
		{
			return _plateAddMaxPower;
		}
		int plateAddMaxPower = CalcPlateAddMaxPower();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_plateAddMaxPower = plateAddMaxPower;
			dataStates.SetCached(DataStatesOffset, 28);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _plateAddMaxPower;
	}

	public CombatSkill()
	{
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		return 27;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += _id.Serialize(ptr);
		*ptr = (byte)_practiceLevel;
		ptr++;
		*(ushort*)ptr = _readingState;
		ptr += 2;
		*(ushort*)ptr = _activationState;
		ptr += 2;
		*ptr = (byte)_forcedBreakoutStepsCount;
		ptr++;
		*ptr = (byte)_breakoutStepsCount;
		ptr++;
		*ptr = (byte)_innerRatio;
		ptr++;
		*(short*)ptr = _obtainedNeili;
		ptr += 2;
		*ptr = (_revoked ? ((byte)1) : ((byte)0));
		ptr++;
		*(long*)ptr = _specialEffectId;
		ptr += 8;
		return (int)(ptr - pData);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += _id.Deserialize(ptr);
		_practiceLevel = (sbyte)(*ptr);
		ptr++;
		_readingState = *(ushort*)ptr;
		ptr += 2;
		_activationState = *(ushort*)ptr;
		ptr += 2;
		_forcedBreakoutStepsCount = (sbyte)(*ptr);
		ptr++;
		_breakoutStepsCount = (sbyte)(*ptr);
		ptr++;
		_innerRatio = (sbyte)(*ptr);
		ptr++;
		_obtainedNeili = *(short*)ptr;
		ptr += 2;
		_revoked = *ptr != 0;
		ptr++;
		_specialEffectId = *(long*)ptr;
		ptr += 8;
		return (int)(ptr - pData);
	}
}
