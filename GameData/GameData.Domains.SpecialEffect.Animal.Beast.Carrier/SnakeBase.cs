using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier;

public abstract class SnakeBase : CarrierEffectBase
{
	protected abstract int ChangeHealEffect { get; }

	protected SnakeBase()
	{
	}

	protected SnakeBase(int charId)
		: base(charId)
	{
	}

	protected override void OnEnableSubClass(DataContext context)
	{
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 119, -1), (EDataModifyType)2);
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 122, -1), (EDataModifyType)2);
	}

	public override void OnDataAdded(DataContext context)
	{
		AppendAffectedAllEnemyData(context, 120, (EDataModifyType)2, -1);
		AppendAffectedAllEnemyData(context, 123, (EDataModifyType)2, -1);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		bool flag = dataKey.CharId == base.CharacterId;
		bool flag2 = flag;
		if (flag2)
		{
			ushort fieldId = dataKey.FieldId;
			bool flag3 = ((fieldId == 119 || fieldId == 122) ? true : false);
			flag2 = flag3;
		}
		if (flag2)
		{
			return ChangeHealEffect;
		}
		bool flag4 = dataKey.CharId != base.CharacterId;
		bool flag5 = flag4;
		if (flag5)
		{
			ushort fieldId = dataKey.FieldId;
			bool flag3 = ((fieldId == 120 || fieldId == 123) ? true : false);
			flag5 = flag3;
		}
		if (flag5)
		{
			return -ChangeHealEffect;
		}
		return 0;
	}
}
