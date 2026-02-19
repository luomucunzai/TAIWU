using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Neigong;

public class TongTouTieBi : AnimalEffectBase
{
	private int ReduceFatalPercent => base.IsElite ? (-60) : (-30);

	private int AddFatalPercent => base.IsElite ? 120 : 60;

	public TongTouTieBi()
	{
	}

	public TongTouTieBi(CombatSkillKey skillKey)
		: base(skillKey)
	{
	}

	public override void OnEnable(DataContext context)
	{
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 191, -1), (EDataModifyType)2);
	}

	public override void OnDataAdded(DataContext context)
	{
		base.OnDataAdded(context);
		AppendAffectedAllEnemyData(context, 191, (EDataModifyType)2, -1);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.FieldId != 191 || !base.IsCurrent)
		{
			return 0;
		}
		EDamageType customParam = (EDamageType)dataKey.CustomParam1;
		if (customParam != EDamageType.Direct)
		{
			return 0;
		}
		if (dataKey.CharId == base.CharacterId)
		{
			ShowSpecialEffectTipsOnceInFrame(0);
		}
		return (dataKey.CharId == base.CharacterId) ? ReduceFatalPercent : AddFatalPercent;
	}
}
