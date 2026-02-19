using System;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Shot;

public class ZhenSiBuZhenHuoFa : CombatSkillEffectBase
{
	private int _fatalMarkCount;

	public ZhenSiBuZhenHuoFa()
	{
	}

	public ZhenSiBuZhenHuoFa(CombatSkillKey skillKey)
		: base(skillKey, 3205, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		CreateAffectedData(89, (EDataModifyType)3, base.SkillTemplateId);
		Events.RegisterHandler_AddDirectFatalDamageMark(OnAddDirectFatalDamageMark);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_AddDirectFatalDamageMark(OnAddDirectFatalDamageMark);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnAddDirectFatalDamageMark(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, int outerMarkCount, int innerMarkCount, short combatSkillId)
	{
		if (SkillKey.IsMatch(attackerId, combatSkillId))
		{
			_fatalMarkCount += outerMarkCount + innerMarkCount;
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId)
		{
			if (_fatalMarkCount > 0)
			{
				base.CurrEnemyChar.WorsenRepeatableInjury(context, base.IsDirect, _fatalMarkCount);
				ShowSpecialEffectTips(1);
			}
			_fatalMarkCount = 0;
			RemoveSelf(context);
		}
	}

	public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
	{
		if (dataKey.SkillKey != SkillKey || dataKey.FieldId != 89)
		{
			return dataValue;
		}
		bool flag = dataKey.CustomParam1 == 1;
		if (base.IsDirect == flag || dataValue <= 0)
		{
			return dataValue;
		}
		EDamageType customParam = (EDamageType)dataKey.CustomParam0;
		if (1 == 0)
		{
		}
		CombatCharacter combatCharacter = customParam switch
		{
			EDamageType.Direct => base.CurrEnemyChar, 
			EDamageType.Bounce => base.CombatChar, 
			_ => null, 
		};
		if (1 == 0)
		{
		}
		CombatCharacter combatCharacter2 = combatCharacter;
		if (combatCharacter2 == null)
		{
			PredefinedLog.Show(7, Id, $"change to fatal by damage type {customParam}");
			combatCharacter2 = base.CurrEnemyChar;
		}
		DataContext dataContext = base.CombatChar.GetDataContext();
		sbyte bodyPart = (sbyte)dataKey.CustomParam2;
		int num = DomainManager.Combat.AddFatalDamageValue(dataContext, combatCharacter2, (int)Math.Clamp(dataValue, 0L, 2147483647L), flag ? 1 : 0, bodyPart, base.SkillTemplateId);
		ShowSpecialEffectTipsOnceInFrame(0);
		if (combatCharacter2 != base.CombatChar)
		{
			_fatalMarkCount += num;
		}
		return 0L;
	}
}
