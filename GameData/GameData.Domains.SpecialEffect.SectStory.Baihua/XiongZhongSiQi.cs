using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.SpecialEffect.Animal;

namespace GameData.Domains.SpecialEffect.SectStory.Baihua;

public class XiongZhongSiQi : CarrierEffectBase
{
	protected override short CombatStateId => 224;

	public XiongZhongSiQi(int charId)
		: base(charId)
	{
	}

	protected override void OnEnableSubClass(DataContext context)
	{
		CreateAffectedData(8, (EDataModifyType)3, -1);
		CreateAffectedData(7, (EDataModifyType)3, -1);
	}

	public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
	{
		bool flag = dataKey.CharId != base.CharacterId;
		bool flag2 = flag;
		if (!flag2)
		{
			ushort fieldId = dataKey.FieldId;
			bool flag3 = (uint)(fieldId - 7) <= 1u;
			flag2 = !flag3;
		}
		if (flag2)
		{
			return dataValue;
		}
		return 0;
	}
}
