using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier;

public abstract class BearBase : CarrierEffectBase
{
	protected abstract int AddOrReduceDirectFatalDamagePercent { get; }

	protected BearBase()
	{
	}

	protected BearBase(int charId)
		: base(charId)
	{
	}

	protected override void OnEnableSubClass(DataContext context)
	{
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 191, -1), (EDataModifyType)2);
	}

	public override void OnDataAdded(DataContext context)
	{
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
		return (dataKey.CharId == base.CharacterId) ? (-AddOrReduceDirectFatalDamagePercent) : AddOrReduceDirectFatalDamagePercent;
	}
}
