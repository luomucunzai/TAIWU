using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier;

public abstract class EagleBase : CarrierEffectBase
{
	protected abstract CValuePercent AddCriticalOddsPercent { get; }

	protected EagleBase()
	{
	}

	protected EagleBase(int charId)
		: base(charId)
	{
	}

	protected override void OnEnableSubClass(DataContext context)
	{
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 254, -1), (EDataModifyType)1);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		if (dataKey.CharId != base.CharacterId)
		{
			return 0;
		}
		if (dataKey.FieldId == 254)
		{
			return (int)DomainManager.Combat.GetCurrentDistance() * AddCriticalOddsPercent;
		}
		return 0;
	}
}
