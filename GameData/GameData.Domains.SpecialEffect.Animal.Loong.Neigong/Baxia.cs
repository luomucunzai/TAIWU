using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong;

public class Baxia : AnimalEffectBase
{
	public Baxia()
	{
	}

	public Baxia(CombatSkillKey skillKey)
		: base(skillKey)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		CreateAffectedData(320, (EDataModifyType)3, -1);
	}

	public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.FieldId != 320)
		{
			return base.GetModifiedValue(dataKey, dataValue);
		}
		if (dataKey.CustomParam1 == 1)
		{
			return base.GetModifiedValue(dataKey, dataValue);
		}
		ShowSpecialEffectTipsOnceInFrame(0);
		return 0L;
	}
}
