using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Polearm;

public class HunYuanTieBangFa : RawCreateUnlockEffectBase
{
	private int _directDamageValue;

	private static CValuePercent AddFatalDamagePercent => CValuePercent.op_Implicit(60);

	protected override IEnumerable<sbyte> RequireMainAttributeTypes { get; } = new sbyte[6] { 0, 1, 3, 4, 2, 5 };

	protected override int RequireMainAttributeValue => 45;

	public HunYuanTieBangFa()
	{
	}

	public HunYuanTieBangFa(CombatSkillKey skillKey)
		: base(skillKey, 9307)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		Events.RegisterHandler_AddDirectDamageValue(AddDirectDamageValue);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_AddDirectDamageValue(AddDirectDamageValue);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
		base.OnDisable(context);
	}

	private void AddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId)
	{
		if (SkillKey.IsMatch(attackerId, combatSkillId) && base.IsReverseOrUsingDirectWeapon)
		{
			_directDamageValue += damageValue;
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		if (SkillKey.IsMatch(charId, skillId) && _directDamageValue > 0)
		{
			int num = _directDamageValue * AddFatalDamagePercent * CValuePercent.op_Implicit((int)power);
			_directDamageValue = 0;
			if (num > 0)
			{
				DomainManager.Combat.AddFatalDamageValue(context, base.CurrEnemyChar, num, -1, -1, -1);
				ShowSpecialEffectTips(base.IsDirect, 1, 0);
			}
		}
	}
}
