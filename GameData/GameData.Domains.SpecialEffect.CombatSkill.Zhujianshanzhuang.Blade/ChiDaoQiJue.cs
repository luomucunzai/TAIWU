using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Blade;

public class ChiDaoQiJue : BladeUnlockEffectBase
{
	private const int AbsorbBreathAndStancePercent = 40;

	private int ReduceBreathStance => base.IsDirectOrReverseEffectDoubling ? (-50) : (-25);

	protected override IEnumerable<short> RequireWeaponTypes
	{
		get
		{
			yield return 1;
			yield return 5;
			yield return 13;
		}
	}

	public ChiDaoQiJue()
	{
	}

	public ChiDaoQiJue(CombatSkillKey skillKey)
		: base(skillKey, 9205)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		CreateAffectedData(204, (EDataModifyType)1, -1);
	}

	protected override void OnAffectedChanged(DataContext context)
	{
		base.OnAffectedChanged(context);
		InvalidateCache(context, 204);
		DomainManager.Combat.UpdateSkillCanUse(context, base.CombatChar, base.SkillTemplateId);
		if (base.Affected)
		{
			ShowSpecialEffectTips(base.IsDirect, 1, 0);
		}
	}

	public override void DoAffectAfterCost(DataContext context, int weaponIndex)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		AbsorbBreathValue(context, base.CurrEnemyChar, CValuePercent.op_Implicit(40));
		AbsorbStanceValue(context, base.CurrEnemyChar, CValuePercent.op_Implicit(40));
		ShowSpecialEffectTips(base.IsDirect, 2, 1);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.SkillKey != SkillKey || dataKey.FieldId != 204 || !base.Affected)
		{
			return 0;
		}
		return ReduceBreathStance;
	}
}
