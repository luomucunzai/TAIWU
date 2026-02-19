using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier;

public abstract class LionBase : CarrierEffectBase
{
	protected abstract int AddOrReduceCostPercent { get; }

	protected LionBase()
	{
	}

	protected LionBase(int charId)
		: base(charId)
	{
	}

	protected override void OnEnableSubClass(DataContext context)
	{
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 255, -1), (EDataModifyType)2);
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 256, -1), (EDataModifyType)2);
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 150, -1), (EDataModifyType)2);
	}

	public override void OnDataAdded(DataContext context)
	{
		AppendAffectedAllEnemyData(context, 255, (EDataModifyType)2, -1);
		AppendAffectedAllEnemyData(context, 256, (EDataModifyType)2, -1);
		AppendAffectedAllEnemyData(context, 150, (EDataModifyType)2, -1);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		ushort fieldId = dataKey.FieldId;
		bool flag = ((fieldId == 150 || (uint)(fieldId - 255) <= 1u) ? true : false);
		if (!flag || !base.IsCurrent)
		{
			return 0;
		}
		return (dataKey.CharId == base.CharacterId) ? (-AddOrReduceCostPercent) : AddOrReduceCostPercent;
	}
}
