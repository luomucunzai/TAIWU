using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Carrier;

public class Yazi : CarrierEffectBase
{
	private static readonly CValuePercent AddDamagePercent = CValuePercent.op_Implicit(33);

	private OuterAndInnerInts _addingDamageValue;

	protected override short CombatStateId => 200;

	public Yazi(int charId)
		: base(charId)
	{
	}

	protected override void OnEnableSubClass(DataContext context)
	{
		base.OnEnableSubClass(context);
		CreateAffectedData(89, (EDataModifyType)3, -1);
		Events.RegisterHandler_AddDirectDamageValue(OnAddDirectDamageValue);
	}

	protected override void OnDisableSubClass(DataContext context)
	{
		Events.UnRegisterHandler_AddDirectDamageValue(OnAddDirectDamageValue);
		base.OnDisableSubClass(context);
	}

	private void OnAddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		if (defenderId == base.CharacterId && damageValue > 0)
		{
			if (isInner)
			{
				_addingDamageValue.Inner += damageValue * AddDamagePercent;
			}
			else
			{
				_addingDamageValue.Outer += damageValue * AddDamagePercent;
			}
		}
	}

	public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
	{
		if (dataKey.FieldId == 89 && dataKey.CharId == base.CharacterId)
		{
			ref int reference = ref dataKey.CustomParam1 == 1 ? ref _addingDamageValue.Inner : ref _addingDamageValue.Outer;
			if (reference > 0)
			{
				dataValue += reference;
				reference = 0;
			}
			return dataValue;
		}
		return base.GetModifiedValue(dataKey, dataValue);
	}
}
