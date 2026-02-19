using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Polearm;

public class DaXiaoYeChaGun : CombatSkillEffectBase
{
	private const sbyte AddRange = 20;

	private const sbyte MaxAddPower = 40;

	private int _addPower;

	public DaXiaoYeChaGun()
	{
	}

	public DaXiaoYeChaGun(CombatSkillKey skillKey)
		: base(skillKey, 1303, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		_addPower = 0;
		CreateAffectedData(199, (EDataModifyType)1, base.SkillTemplateId);
		CreateAffectedData((ushort)(base.IsDirect ? 146 : 145), (EDataModifyType)0, base.SkillTemplateId);
		Events.RegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private int CalcAddPower()
	{
		short currentDistance = DomainManager.Combat.GetCurrentDistance();
		OuterAndInnerShorts attackRange = base.CombatChar.GetAttackRange();
		attackRange.Outer = Math.Max(attackRange.Outer, (short)20);
		attackRange.Inner = Math.Min(attackRange.Inner, (short)120);
		if (attackRange.Outer > currentDistance || attackRange.Inner < currentDistance)
		{
			return 0;
		}
		int num = (attackRange.Outer + attackRange.Inner) / 2;
		int val = 40 * (100 - (base.IsDirect ? ((attackRange.Inner - currentDistance) * 100 / (attackRange.Inner - num)) : ((currentDistance - attackRange.Outer) * 100 / (num - attackRange.Outer)))) / 100;
		return Math.Max(val, 0);
	}

	private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
	{
		if (SkillKey.IsMatch(attacker.GetId(), skillId))
		{
			_addPower = CalcAddPower();
			if (_addPower > 0)
			{
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
				ShowSpecialEffectTips(0);
			}
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId)
		{
			_addPower = 0;
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.SkillKey != SkillKey)
		{
			return 0;
		}
		ushort fieldId = dataKey.FieldId;
		if (1 == 0)
		{
		}
		int result;
		switch (fieldId)
		{
		case 145:
		case 146:
			result = 20;
			break;
		case 199:
			result = _addPower;
			break;
		default:
			result = 0;
			break;
		}
		if (1 == 0)
		{
		}
		return result;
	}
}
