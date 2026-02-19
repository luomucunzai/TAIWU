using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

public class StrengthenPoison : CombatSkillEffectBase
{
	private const sbyte AddPowerValuePercent = 100;

	private const sbyte AddPoisonToSelfOdds = 30;

	private const sbyte AddPoisonToSelfPercent = 20;

	private const sbyte ReducePoisonResistPercent = 50;

	private const int SilenceRequireRatio = 60;

	private const int SilenceFrame = 2400;

	private const int SilenceCount = 3;

	protected sbyte AffectPoisonType;

	private sbyte _addedPoisonLevel;

	private int _addedPoisonValue;

	protected virtual bool Variant1 => true;

	public StrengthenPoison()
	{
	}

	public StrengthenPoison(CombatSkillKey skillKey, int type)
		: base(skillKey, type, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		if (base.IsDirect)
		{
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 78, base.SkillTemplateId), (EDataModifyType)3);
		}
		else
		{
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 73, base.SkillTemplateId), (EDataModifyType)1);
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 233, base.SkillTemplateId), (EDataModifyType)1);
		}
		if (!base.IsDirect)
		{
			Events.RegisterHandler_AddPoison(OnAddPoison);
		}
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		if (!base.IsDirect)
		{
			Events.UnRegisterHandler_AddPoison(OnAddPoison);
		}
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void DirectVariant1(DataContext context)
	{
		if (base.IsDirect && Variant1 && base.CurrEnemyChar.GetDefeatMarkCollection().PoisonMarkList[AffectPoisonType] > 0)
		{
			ShowSpecialEffectTips(1);
			DomainManager.Combat.PoisonAffect(context, base.CurrEnemyChar, AffectPoisonType);
		}
	}

	private unsafe void DirectVariant2(DataContext context)
	{
		if (!base.IsDirect || Variant1)
		{
			return;
		}
		PoisonsAndLevels poisons = base.SkillInstance.GetPoisons();
		sbyte level = poisons.Levels[AffectPoisonType];
		short addValue = poisons.Values[AffectPoisonType];
		IReadOnlyList<sbyte> readOnlyList = (PoisonType.IsInner(AffectPoisonType) ? PoisonType.InnerPoison : PoisonType.OuterPoison);
		foreach (sbyte item in readOnlyList)
		{
			if (item != AffectPoisonType)
			{
				DomainManager.Combat.AddPoison(context, base.CombatChar, base.CurrEnemyChar, item, level, addValue, base.SkillTemplateId, applySpecialEffect: true, canBounce: true, default(ItemKey), isDirectPoison: true);
			}
		}
		ShowSpecialEffectTips(1);
	}

	private int ReverseVariant1()
	{
		if (base.IsDirect || !Variant1)
		{
			return 0;
		}
		ShowSpecialEffectTips(1);
		return -50;
	}

	private void ReverseVariant2(DataContext context)
	{
		if (base.IsDirect || Variant1)
		{
			return;
		}
		CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
		bool isInner = PoisonType.IsInner(AffectPoisonType);
		foreach (short randomUnrepeatedBanableSkillId in enemyChar.GetRandomUnrepeatedBanableSkillIds(context.Random, 3, Predicate, -1, -1))
		{
			DomainManager.Combat.SilenceSkill(context, enemyChar, randomUnrepeatedBanableSkillId, 2400);
			ShowSpecialEffectTipsOnceInFrame(1);
		}
		bool Predicate(short skillId)
		{
			CombatSkillKey objectId = new CombatSkillKey(enemyChar.GetId(), skillId);
			if (!DomainManager.CombatSkill.TryGetElement_CombatSkills(objectId, out var element))
			{
				return false;
			}
			int num = (isInner ? element.GetCurrInnerRatio() : (100 - element.GetCurrInnerRatio()));
			return num >= 60;
		}
	}

	private void OnAddPoison(DataContext context, int attackerId, int defenderId, sbyte poisonType, sbyte level, int addValue, short skillId, bool canBounce)
	{
		if (attackerId == base.CharacterId && poisonType == AffectPoisonType && skillId == base.SkillTemplateId)
		{
			_addedPoisonLevel = level;
			_addedPoisonValue = addValue;
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId)
		{
			if ((base.IsDirect || !Variant1) && PowerMatchAffectRequire(power))
			{
				DirectVariant1(context);
				DirectVariant2(context);
				ReverseVariant2(context);
			}
			if (!base.IsDirect && _addedPoisonValue > 0 && context.Random.CheckPercentProb(30))
			{
				DomainManager.Combat.AddPoison(context, base.CombatChar, base.CombatChar, AffectPoisonType, _addedPoisonLevel, _addedPoisonValue * 20 / 100, skillId);
				ShowSpecialEffectTips(2);
			}
			RemoveSelf(context);
		}
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId)
		{
			return dataValue;
		}
		if (dataKey.FieldId == 78 && dataKey.CustomParam0 == AffectPoisonType)
		{
			ShowSpecialEffectTips(0);
			return true;
		}
		return dataValue;
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId || dataKey.CustomParam0 != AffectPoisonType)
		{
			return 0;
		}
		if (dataKey.FieldId == 73)
		{
			ShowSpecialEffectTips(0);
			return 100;
		}
		if (dataKey.FieldId == 233 && dataKey.CustomParam1 > 0)
		{
			return ReverseVariant1();
		}
		return 0;
	}
}
