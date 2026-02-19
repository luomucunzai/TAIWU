using System.Diagnostics.CodeAnalysis;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Combat;

public readonly struct CombatContext
{
	public DataContext Context { get; private init; }

	public CombatCharacter Attacker { get; private init; }

	public CombatCharacter Defender { get; private init; }

	public int BounceSourceId { get; init; }

	private bool IsBounce { get; init; }

	private sbyte SpecifyBodyPart { get; init; }

	private short SpecifySkillId { get; init; }

	private int SpecifyWeaponIndex { get; init; }

	private CombatProperty? SpecifyProperty { get; init; }

	private ECombatCriticalType CriticalType { get; init; }

	public IRandomSource Random => Context.Random;

	public int AttackerId => Attacker.GetId();

	public int DefenderId => Defender.GetId();

	public sbyte BodyPart => (SpecifyBodyPart >= 0) ? SpecifyBodyPart : (IsNormalAttack ? Attacker.NormalAttackBodyPart : Attacker.SkillAttackBodyPart);

	public sbyte InnerRatio => IsNormalAttack ? WeaponData.GetInnerRatio() : Skill.GetCurrInnerRatio();

	public sbyte OuterRatio => (sbyte)(100 - InnerRatio);

	public bool IsNormalAttack => SkillTemplateId < 0;

	public bool IsFightBack => IsNormalAttack && Attacker.GetIsFightBack();

	private static ItemDomain ItemDomain => DomainManager.Item;

	private static CombatDomain CombatDomain => DomainManager.Combat;

	private static CombatSkillDomain CombatSkillDomain => DomainManager.CombatSkill;

	public short SkillTemplateId => (SpecifySkillId >= 0) ? SpecifySkillId : Attacker.GetPerformingSkillId();

	public CombatSkillKey SkillKey => new CombatSkillKey(Attacker.GetId(), SkillTemplateId);

	public GameData.Domains.CombatSkill.CombatSkill Skill
	{
		get
		{
			GameData.Domains.CombatSkill.CombatSkill element;
			return CombatSkillDomain.TryGetElement_CombatSkills(SkillKey, out element) ? element : null;
		}
	}

	public CombatSkillData SkillData
	{
		get
		{
			CombatSkillData combatSkillData;
			return CombatDomain.TryGetCombatSkillData(Attacker.GetId(), SkillTemplateId, out combatSkillData) ? combatSkillData : null;
		}
	}

	public CombatSkillItem SkillConfig => Config.CombatSkill.Instance[SkillTemplateId];

	public ItemKey WeaponKey => (SpecifyWeaponIndex >= 0) ? Attacker.GetWeapons()[SpecifyWeaponIndex] : CombatDomain.GetUsingWeaponKey(Attacker);

	public GameData.Domains.Item.Weapon Weapon => ItemDomain.GetElement_Weapons(WeaponKey.Id);

	public CombatWeaponData WeaponData => CombatDomain.GetElement_WeaponDataDict(WeaponKey.Id);

	public WeaponItem WeaponConfig => Config.Weapon.Instance[WeaponKey.TemplateId];

	public sbyte WeaponPointCost => WeaponConfig.AttackPreparePointCost;

	public int WeaponAttack => CombatDomain.CalcWeaponAttack(Attacker, Weapon, SkillTemplateId);

	public int WeaponDefend => CombatDomain.CalcWeaponDefend(Attacker, Weapon, SkillTemplateId);

	public ItemKey ArmorKey => (BodyPart >= 0) ? Defender.Armors[BodyPart] : ItemKey.Invalid;

	public GameData.Domains.Item.Armor Armor => ArmorKey.IsValid() ? ItemDomain.GetElement_Armors(ArmorKey.Id) : null;

	public int ArmorAttack => CombatDomain.CalcArmorAttack(Defender, Armor);

	public int ArmorDefend => CombatDomain.CalcArmorDefend(Defender, Armor);

	public ItemKey WeaponOrShoesKey => (SkillTemplateId >= 0 && CombatSkillDomain.GetSkillType(AttackerId, SkillTemplateId) == 5 && Attacker.LegSkillUseShoes()) ? Attacker.Armors[5] : WeaponKey;

	public EquipmentBase WeaponOrShoes => ItemDomain.TryGetBaseEquipment(WeaponOrShoesKey);

	public int OuterStep => (BodyPart < 0) ? (-1) : DamageStepCollection.OuterDamageSteps[BodyPart];

	public int InnerStep => (BodyPart < 0) ? (-1) : DamageStepCollection.InnerDamageSteps[BodyPart];

	public int OuterOrigin => (BodyPart >= 0) ? Defender.GetOuterDamageValue()[BodyPart] : 0;

	public int InnerOrigin => (BodyPart >= 0) ? Defender.GetInnerDamageValue()[BodyPart] : 0;

	public EDataSumType OuterSumType => DataSumTypeHelper.CalcSumType(CalcDamageCanAdd(inner: false), CalcDamageCanReduce(inner: false));

	public EDataSumType InnerSumType => DataSumTypeHelper.CalcSumType(CalcDamageCanAdd(inner: true), CalcDamageCanReduce(inner: true));

	public int ExtraFlawCount => DomainManager.SpecialEffect.GetModifyValue(Attacker.GetId(), SkillTemplateId, 84, (EDataModifyType)0, BodyPart, -1, -1, (EDataSumType)0);

	public EDamageType OuterDamageType => (EDamageType)DomainManager.SpecialEffect.ModifyData(Attacker.GetId(), SkillTemplateId, 79, (int)DamageType, 0);

	public EDamageType InnerDamageType => (EDamageType)DomainManager.SpecialEffect.ModifyData(Attacker.GetId(), SkillTemplateId, 79, (int)DamageType, 1);

	private bool AllChangeToOld => Attacker.IsUnlockAttack && Attacker.UnlockEffect.ChangeToOld;

	public bool OuterInjuryChangeToOld => DomainManager.SpecialEffect.ModifyData(AttackerId, SkillTemplateId, 77, dataValue: false, 0, BodyPart) || AllChangeToOld;

	public bool InnerInjuryChangeToOld => DomainManager.SpecialEffect.ModifyData(AttackerId, SkillTemplateId, 77, dataValue: false, 1, BodyPart) || AllChangeToOld;

	public bool IsGodWeapon => DomainManager.SpecialEffect.ModifyData(Attacker.GetId(), -1, 181, dataValue: false, WeaponOrShoesKey.Id);

	public bool IsGodArmor => DomainManager.SpecialEffect.ModifyData(Defender.GetId(), -1, 182, dataValue: false, ArmorKey.Id);

	public DamageStepCollection DamageStepCollection => Defender.GetDamageStepCollection();

	public EDamageType DamageType => IsBounce ? EDamageType.Bounce : ((!IsFightBack) ? EDamageType.Direct : EDamageType.FightBack);

	public bool UseSkillAttackOdds => !IsNormalAttack || Attacker.IsAnimal;

	public CFormula.EAttackType AttackType
	{
		get
		{
			if (Attacker.IsUnlockAttack)
			{
				return CFormula.EAttackType.Unlock;
			}
			if (Attacker.IsAutoNormalAttackingSpecial)
			{
				return CFormula.EAttackType.Spirit;
			}
			if (UseSkillAttackOdds)
			{
				return (BodyPart < 0) ? CFormula.EAttackType.MindSkill : CFormula.EAttackType.Skill;
			}
			return CFormula.EAttackType.Normal;
		}
	}

	public int BaseDamage => CFormula.CalcBaseDamageValue(AttackType, WeaponPointCost);

	public int AttackOdds => CFormula.CalcBaseAttackOdds(AttackType);

	public CValuePercentBonus FlawBonus
	{
		get
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			int num = (int)CFormula.CalcFlawDamageBonus(Defender.GetFlawCount()[BodyPart], ExtraFlawCount);
			if (num > 0)
			{
				num *= DomainManager.SpecialEffect.GetModify(AttackerId, SkillTemplateId, 318, BodyPart, -1, -1, (EDataSumType)0);
			}
			return CValuePercentBonus.op_Implicit(num);
		}
	}

	public CValuePercentBonus ConsummateBonus => CFormulaHelper.CalcConsummateChangeDamagePercent(Attacker, Defender);

	public static CombatContext Create([DisallowNull] CombatCharacter attacker, [AllowNull] CombatCharacter defender = null, sbyte specifyBodyPart = -1, short specifySkillId = -1, int specifyWeaponIndex = -1, CombatProperty? combatProperty = null)
	{
		return new CombatContext
		{
			Context = attacker.GetDataContext(),
			Attacker = attacker,
			Defender = (defender ?? CombatDomain.GetCombatCharacter(!attacker.IsAlly, tryGetCoverCharacter: true)),
			BounceSourceId = -1,
			SpecifyProperty = combatProperty,
			SpecifyBodyPart = specifyBodyPart,
			SpecifySkillId = specifySkillId,
			SpecifyWeaponIndex = specifyWeaponIndex,
			IsBounce = false,
			CriticalType = ECombatCriticalType.Uncheck
		};
	}

	public CombatContext(CombatContext context)
	{
		Context = context.Context;
		Attacker = context.Attacker;
		Defender = context.Defender;
		BounceSourceId = context.BounceSourceId;
		SpecifyProperty = context.SpecifyProperty;
		SpecifyBodyPart = context.SpecifyBodyPart;
		SpecifySkillId = context.SpecifySkillId;
		SpecifyWeaponIndex = context.SpecifyWeaponIndex;
		IsBounce = context.IsBounce;
		CriticalType = context.CriticalType;
	}

	public CombatContext Bounce()
	{
		if (IsBounce)
		{
			PredefinedLog.Show(8, "Unable to bounce a bounce");
		}
		CombatContext result = new CombatContext(this);
		result.Defender = Attacker;
		result.IsBounce = true;
		result.BounceSourceId = DefenderId;
		result.SpecifyProperty = null;
		result.CriticalType = ECombatCriticalType.Uncheck;
		return result;
	}

	public CombatContext Property(CombatProperty property)
	{
		CombatContext result = new CombatContext(this);
		result.SpecifyProperty = property;
		return result;
	}

	public CombatContext Critical(bool critical)
	{
		CombatContext result = new CombatContext(this);
		result.CriticalType = (critical ? ECombatCriticalType.Critical : ECombatCriticalType.NoCritical);
		return result;
	}

	public static implicit operator DataContext(CombatContext context)
	{
		return context.Context;
	}

	private bool CalcDamageCanAdd(bool inner)
	{
		if (IsGodArmor && !IsGodWeapon)
		{
			return false;
		}
		return DomainManager.SpecialEffect.ModifyData(DefenderId, SkillTemplateId, 326, dataValue: true, inner ? 1 : 0, BodyPart, (int)DamageType);
	}

	private bool CalcDamageCanReduce(bool inner)
	{
		if (IsGodWeapon && !IsGodArmor)
		{
			return false;
		}
		return DomainManager.SpecialEffect.ModifyData(AttackerId, SkillTemplateId, 327, dataValue: true, inner ? 1 : 0, BodyPart, (int)DamageType);
	}

	public CombatProperty CalcProperty(sbyte hitType = -1)
	{
		if (SpecifyProperty.HasValue)
		{
			return SpecifyProperty.Value;
		}
		Tester.Assert(hitType >= 0);
		return CombatProperty.Create(this, hitType);
	}

	public OuterAndInnerInts CalcMixedDamage(sbyte hitType, CValuePercent power)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		int damageValue = BaseDamage * FlawBonus * power;
		CombatProperty combatProperty = CalcProperty(hitType);
		return CFormula.FormulaCalcMixedDamageValue(damageValue, AttackOdds, InnerRatio, combatProperty.AttackValue, combatProperty.DefendValue);
	}

	public bool CheckCritical(sbyte hitType)
	{
		if (CriticalType != ECombatCriticalType.Uncheck)
		{
			return CriticalType == ECombatCriticalType.Critical;
		}
		if (BodyPart < 0)
		{
			return false;
		}
		int hitOdds = CalcProperty(hitType).HitOdds;
		bool flag = DomainManager.SpecialEffect.ModifyData(DefenderId, SkillTemplateId, 234, dataValue: true, BodyPart, hitType, AttackerId);
		bool certainCritical = Attacker.IsBreakAttacking || DomainManager.SpecialEffect.ModifyData(AttackerId, SkillTemplateId, 248, dataValue: false, hitType);
		return flag && CombatDomain.CheckCritical(Random, AttackerId, hitOdds, certainCritical);
	}

	public void ApplyWeaponAndArmorPoison(int valueMultiplier = 1)
	{
		sbyte bodyPart = BodyPart;
		if ((bodyPart >= 0 && bodyPart < 7) || 1 == 0)
		{
			DomainManager.Combat.ApplyEquipmentPoison(this, Attacker, Defender, WeaponOrShoesKey, valueMultiplier);
			DomainManager.Combat.ApplyEquipmentPoison(this, Defender, Attacker, ArmorKey, valueMultiplier);
		}
	}

	public void CheckReduceWeaponDurability(sbyte breakOdds)
	{
		if (breakOdds > 0)
		{
			short num = WeaponOrShoes?.GetCurrDurability() ?? 0;
			if (num > 0 && ArmorAttack > WeaponDefend && (!IsGodWeapon || !IsNormalAttack) && CombatDomain.IsWeaponCanBreak(Weapon.GetItemSubType()))
			{
				Attacker.NeedReduceWeaponDurability = Random.CheckPercentProb(breakOdds);
			}
		}
	}

	public void CheckReduceArmorDurability(sbyte breakOdds)
	{
		if (breakOdds > 0)
		{
			short num = Armor?.GetCurrDurability() ?? 0;
			if (num > 0 && WeaponAttack > ArmorDefend && (!IsGodArmor || !IsNormalAttack))
			{
				Defender.NeedReduceArmorDurability = Random.CheckPercentProb(breakOdds);
			}
		}
	}
}
