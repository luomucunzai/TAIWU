using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Special;

public class QiBaiPoShenFa : CurseSilenceCombatSkill
{
	private HitOrAvoidInts _reduceAvoid;

	private OuterAndInnerInts _reducePenetrateResist;

	private int CalcReducePower => base.IsDirect ? 50 : 25;

	protected override sbyte TargetEquipType => 3;

	public QiBaiPoShenFa()
	{
	}

	public QiBaiPoShenFa(CombatSkillKey skillKey)
		: base(skillKey, 7306)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		CreateAffectedAllEnemyData(38, (EDataModifyType)0, -1);
		CreateAffectedAllEnemyData(39, (EDataModifyType)0, -1);
		CreateAffectedAllEnemyData(40, (EDataModifyType)0, -1);
		CreateAffectedAllEnemyData(41, (EDataModifyType)0, -1);
		CreateAffectedAllEnemyData(46, (EDataModifyType)0, -1);
		CreateAffectedAllEnemyData(47, (EDataModifyType)0, -1);
	}

	protected override void OnSilenceBegin(DataContext context, CombatSkillKey _)
	{
		UpdateReduceValues(context);
		ShowSpecialEffectTips(2);
	}

	protected override void OnSilenceEnd(DataContext context, CombatSkillKey skillKey)
	{
		UpdateReduceValues(context);
	}

	private void UpdateReduceValues(DataContext context)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		_reduceAvoid.Initialize();
		_reducePenetrateResist = OuterAndInnerInts.Zero;
		foreach (CombatSkillKey silencingSkill in base.SilencingSkills)
		{
			GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(silencingSkill);
			HitOrAvoidInts hitOrAvoidInts = CombatSkillDomain.CalcAddAvoidValueOnCast(element_CombatSkills, CValuePercent.op_Implicit(CalcReducePower));
			OuterAndInnerInts outerAndInnerInts = CombatSkillDomain.CalcAddPenetrateResist(element_CombatSkills, CValuePercent.op_Implicit(CalcReducePower));
			_reduceAvoid -= hitOrAvoidInts;
			_reducePenetrateResist -= outerAndInnerInts;
		}
		InvalidateAllEnemyCache(context, 38);
		InvalidateAllEnemyCache(context, 39);
		InvalidateAllEnemyCache(context, 40);
		InvalidateAllEnemyCache(context, 41);
		InvalidateAllEnemyCache(context, 46);
		InvalidateAllEnemyCache(context, 47);
	}

	public unsafe override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		ushort fieldId = dataKey.FieldId;
		if (1 == 0)
		{
		}
		int result = fieldId switch
		{
			38 => _reduceAvoid.Items[0], 
			39 => _reduceAvoid.Items[1], 
			40 => _reduceAvoid.Items[2], 
			41 => _reduceAvoid.Items[3], 
			46 => _reducePenetrateResist.Outer, 
			47 => _reducePenetrateResist.Inner, 
			_ => base.GetModifyValue(dataKey, currModifyValue), 
		};
		if (1 == 0)
		{
		}
		return result;
	}
}
