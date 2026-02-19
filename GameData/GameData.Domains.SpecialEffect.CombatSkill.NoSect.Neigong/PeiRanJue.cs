using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.NoSect.Neigong;

public class PeiRanJue : CombatSkillEffectBase
{
	private static readonly CValuePercentBonus ReduceGongMadInjury = CValuePercentBonus.op_Implicit(-75);

	public PeiRanJue()
	{
	}

	public PeiRanJue(CombatSkillKey skillKey)
		: base(skillKey, 0, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		CreateAffectedData(117, (EDataModifyType)3, -1);
	}

	public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		if (dataKey.CharId != base.CharacterId || dataKey.FieldId != 117)
		{
			return base.GetModifiedValue(dataKey, dataValue);
		}
		return dataValue * ReduceGongMadInjury;
	}
}
