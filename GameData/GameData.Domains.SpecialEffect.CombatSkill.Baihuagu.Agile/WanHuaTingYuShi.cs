using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Agile;

public class WanHuaTingYuShi : BuffHitOrDebuffAvoid
{
	protected override sbyte AffectHitType => 3;

	private int InfinityMindMarkProgressAddPercent => base.IsDirect ? 100 : (-50);

	public WanHuaTingYuShi()
	{
	}

	public WanHuaTingYuShi(CombatSkillKey skillKey)
		: base(skillKey, 3407)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		if (base.IsDirect)
		{
			CreateAffectedAllEnemyData(305, (EDataModifyType)2, -1);
		}
		else
		{
			CreateAffectedData(305, (EDataModifyType)2, -1);
		}
		if (base.CanAffect)
		{
			ShowSpecialEffectTips(2);
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.FieldId == 305 && base.CanAffect)
		{
			return InfinityMindMarkProgressAddPercent;
		}
		return base.GetModifyValue(dataKey, currModifyValue);
	}
}
