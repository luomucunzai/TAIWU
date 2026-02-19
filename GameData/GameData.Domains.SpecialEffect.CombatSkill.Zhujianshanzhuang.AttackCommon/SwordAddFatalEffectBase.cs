using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;

public abstract class SwordAddFatalEffectBase : SwordUnlockEffectBase
{
	private const int EffectFatalDamagePercent = 5;

	private int _directDamageValue;

	private int SelfFatalDamagePercent => base.IsDirectOrReverseEffectDoubling ? 10 : 5;

	protected abstract CValueMultiplier FlawOrAcupointCount { get; }

	protected SwordAddFatalEffectBase()
	{
	}

	protected SwordAddFatalEffectBase(CombatSkillKey skillKey, int type)
		: base(skillKey, type)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		Events.RegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
		Events.RegisterHandler_AddDirectDamageValue(AddDirectDamageValue);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
		Events.UnRegisterHandler_AddDirectDamageValue(AddDirectDamageValue);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
		base.OnDisable(context);
	}

	private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
	{
		if (attacker.GetId() == base.CharacterId)
		{
			_directDamageValue = 0;
		}
	}

	private void AddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId)
	{
		if (attackerId == base.CharacterId)
		{
			_directDamageValue += damageValue;
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		if (charId != base.CharacterId || _directDamageValue <= 0)
		{
			return;
		}
		CValuePercent val = CValuePercent.op_Implicit(0);
		if (skillId == base.SkillTemplateId && base.IsReverseOrUsingDirectWeapon)
		{
			val += CValuePercent.op_Implicit(SelfFatalDamagePercent);
		}
		if (base.EffectCount > 0)
		{
			val += CValuePercent.op_Implicit(5);
		}
		val *= FlawOrAcupointCount;
		int num = _directDamageValue * val;
		_directDamageValue = 0;
		if (num > 0)
		{
			if (base.EffectCount > 0)
			{
				ReduceEffectCount();
			}
			DomainManager.Combat.AddFatalDamageValue(context, base.CurrEnemyChar, num, -1, -1, -1);
			ShowSpecialEffectTips(base.IsDirect, 1, 0);
		}
	}
}
