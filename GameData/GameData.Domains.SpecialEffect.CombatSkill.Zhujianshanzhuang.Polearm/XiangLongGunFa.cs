using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Polearm;

public class XiangLongGunFa : PolearmUnlockEffectBase, IExtraUnlockEffect
{
	private CValuePercent CastAddUnlockPercent => CValuePercent.op_Implicit(base.IsDirectOrReverseEffectDoubling ? 8 : 4);

	private static CValuePercent UnlockAddUnlockPercent => CValuePercent.op_Implicit(15);

	protected override IEnumerable<sbyte> RequireMainAttributeTypes { get; } = new sbyte[1];

	protected override int RequireMainAttributeValue => 75;

	public XiangLongGunFa()
	{
	}

	public XiangLongGunFa(CombatSkillKey skillKey)
		: base(skillKey, 9300)
	{
	}

	protected override void OnCastAddUnlockAttackValue(DataContext context, CValuePercent power)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		base.OnCastAddUnlockAttackValue(context, power);
		if (base.IsReverseOrUsingDirectWeapon)
		{
			base.CombatChar.ChangeAllUnlockAttackValue(context, CastAddUnlockPercent * power);
			ShowSpecialEffectTipsOnceInFrame(0);
		}
	}

	protected override void DoAffect(DataContext context, int weaponIndex)
	{
		base.CombatChar.InvokeExtraUnlockEffect(this, weaponIndex);
	}

	public void DoAffectAfterCost(DataContext context, int weaponIndex)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		base.CombatChar.ChangeAllUnlockAttackValue(context, UnlockAddUnlockPercent);
		ShowSpecialEffectTips(0);
	}
}
