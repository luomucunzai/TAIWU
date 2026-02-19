using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.RawCreate;

public class ShanHeShenJie : RawCreateEquipmentBase
{
	private static CValuePercent ReplacementLossPercent => CValuePercent.op_Implicit(50);

	protected override int ReduceDurabilityValue => 4;

	public ShanHeShenJie()
	{
	}

	public ShanHeShenJie(int charId, ItemKey itemKey)
		: base(charId, itemKey, 30201)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		CreateAffectedData(309, (EDataModifyType)3, -1);
	}

	public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		if (dataKey.CharId != base.CharacterId || dataKey.FieldId != 309 || !base.CanAffect)
		{
			return dataValue;
		}
		ItemKey lhs = base.CombatChar.ChangingDurabilityItems.Peek();
		if (base.CombatChar.GetRawCreateCollection().EffectEquals(lhs, EquipItemKey))
		{
			return dataValue;
		}
		int num = dataValue * ReplacementLossPercent;
		if (num >= 0)
		{
			return dataValue;
		}
		dataValue -= num;
		DataContext context = DomainManager.Combat.Context;
		ChangeDurability(context, base.CombatChar, EquipItemKey, num);
		return dataValue;
	}
}
