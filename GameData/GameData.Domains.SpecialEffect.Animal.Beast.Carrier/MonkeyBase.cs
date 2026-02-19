using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier;

public abstract class MonkeyBase : CarrierEffectBase
{
	protected abstract int PowerAddOrReduceRatio { get; }

	protected MonkeyBase()
	{
	}

	protected MonkeyBase(int charId)
		: base(charId)
	{
	}

	protected override void OnEnableSubClass(DataContext context)
	{
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 257, -1), (EDataModifyType)2);
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 258, -1), (EDataModifyType)2);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId)
		{
			return 0;
		}
		ushort fieldId = dataKey.FieldId;
		if (1 == 0)
		{
		}
		int result = fieldId switch
		{
			257 => PowerAddOrReduceRatio, 
			258 => -PowerAddOrReduceRatio, 
			_ => 0, 
		};
		if (1 == 0)
		{
		}
		return result;
	}
}
