using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.SectStory.Zhujian;

public class GearMateC : AutoCollectEffectBase
{
	public GearMateC(int charId)
		: base(charId)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		CreateAffectedAllEnemyData(286, (EDataModifyType)3, -1);
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		if (dataKey.FieldId != 286)
		{
			return dataValue;
		}
		if (!CharObj.IsCombatSkillEquipped(dataKey.CombatSkillId))
		{
			return dataValue;
		}
		return false;
	}
}
