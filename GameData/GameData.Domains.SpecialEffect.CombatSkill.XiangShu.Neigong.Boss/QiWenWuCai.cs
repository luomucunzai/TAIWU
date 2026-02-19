using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Neigong.Boss;

public class QiWenWuCai : BossNeigongBase
{
	private sbyte AddNeiliAllocationFrame = 60;

	private int _frameCounter;

	public QiWenWuCai()
	{
	}

	public QiWenWuCai(CombatSkillKey skillKey)
		: base(skillKey, 16103)
	{
	}

	public override void OnDisable(DataContext context)
	{
		base.OnDisable(context);
		Events.UnRegisterHandler_CombatStateMachineUpdateEnd(OnStateMachineUpdateEnd);
	}

	protected override void ActivePhase2Effect(DataContext context)
	{
		AppendAffectedData(context, base.CharacterId, 114, (EDataModifyType)3, -1);
		Events.RegisterHandler_CombatStateMachineUpdateEnd(OnStateMachineUpdateEnd);
	}

	private void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
	{
		if (base.CombatChar != combatChar || DomainManager.Combat.Pause)
		{
			return;
		}
		_frameCounter++;
		if (_frameCounter >= AddNeiliAllocationFrame)
		{
			_frameCounter = 0;
			for (byte b = 0; b < 4; b++)
			{
				base.CombatChar.ChangeNeiliAllocation(context, b, 1);
			}
		}
	}

	public unsafe override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
	{
		EDamageType customParam = (EDamageType)dataKey.CustomParam0;
		if (dataKey.CharId != base.CharacterId || customParam != EDamageType.Direct)
		{
			return dataValue;
		}
		NeiliAllocation neiliAllocation = base.CombatChar.GetNeiliAllocation();
		NeiliAllocation neiliAllocation2 = base.CurrEnemyChar.GetNeiliAllocation();
		for (byte b = 0; b < 4; b++)
		{
			if (neiliAllocation.Items[(int)b] < neiliAllocation2.Items[(int)b] * 2)
			{
				return dataValue;
			}
		}
		return 0L;
	}
}
