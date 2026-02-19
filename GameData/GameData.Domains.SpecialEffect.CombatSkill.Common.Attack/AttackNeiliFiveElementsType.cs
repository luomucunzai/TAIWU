using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

public class AttackNeiliFiveElementsType : CombatSkillEffectBase
{
	private const sbyte DirectClearEffect = 3;

	private const sbyte ReverseAddDamagePercent = 90;

	protected sbyte AffectFiveElementsType;

	private bool _reverseTipsShowed;

	protected AttackNeiliFiveElementsType()
	{
	}

	protected AttackNeiliFiveElementsType(CombatSkillKey skillKey, int type)
		: base(skillKey, type, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Events.RegisterHandler_PrepareSkillEnd(OnPrepareSkillEnd);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_PrepareSkillEnd(OnPrepareSkillEnd);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private bool NeiliTypeMismatchAffectType(sbyte neiliType)
	{
		byte fiveElements = NeiliType.Instance[neiliType].FiveElements;
		return fiveElements != AffectFiveElementsType && fiveElements != FiveElementsType.Countering[AffectFiveElementsType];
	}

	private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
	{
		if (!SkillKey.IsMatch(charId, skillId))
		{
			return;
		}
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, tryGetCoverCharacter: true);
		if (NeiliTypeMismatchAffectType(combatCharacter.GetNeiliType()) || !DomainManager.Combat.InAttackRange(base.CombatChar))
		{
			RemoveSelf(context);
		}
		else if (base.IsDirect)
		{
			Dictionary<SkillEffectKey, short> effectDict = combatCharacter.GetSkillEffectCollection().EffectDict;
			if (effectDict != null && effectDict.Count > 0)
			{
				List<SkillEffectKey> list = new List<SkillEffectKey>();
				int num = Math.Min(effectDict.Count, 3);
				list.AddRange(effectDict.Keys);
				for (int i = 0; i < num; i++)
				{
					SkillEffectKey skillEffectKey = list[context.Random.Next(0, list.Count)];
					list.Remove(skillEffectKey);
					DomainManager.Combat.ChangeSkillEffectCount(context, combatCharacter, skillEffectKey, (short)(-effectDict[skillEffectKey]));
				}
			}
			ClearAffectingAgileSkill(context, combatCharacter);
			DomainManager.Combat.ClearAffectingDefenseSkill(context, combatCharacter);
			ShowSpecialEffectTips(0);
		}
		else
		{
			AppendAffectedData(context, base.CharacterId, 69, (EDataModifyType)1, base.SkillTemplateId);
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId)
		{
			RemoveSelf(context);
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.FieldId == 69 && dataKey.CombatSkillId == base.SkillTemplateId && CombatCharPowerMatchAffectRequire())
		{
			if (!_reverseTipsShowed)
			{
				ShowSpecialEffectTips(0);
				_reverseTipsShowed = true;
			}
			return 90;
		}
		return 0;
	}
}
