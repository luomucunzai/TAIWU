using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;

namespace GameData.Domains.SpecialEffect.Profession.Savage;

public class YanXueMing : ProfessionEffectBase
{
	private const sbyte AddPercent = 20;

	protected override short CombatStateId => 139;

	public YanXueMing()
	{
	}

	public YanXueMing(int charId)
		: base(charId)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		Events.RegisterHandler_CombatBegin(OnCombatBegin);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CombatBegin(OnCombatBegin);
		base.OnDisable(context);
	}

	private void OnCombatBegin(DataContext context)
	{
		AppendAffectedAllEnemyData(context, 283, (EDataModifyType)1, -1);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId == base.CharacterId)
		{
			return 0;
		}
		if (dataKey.FieldId == 283)
		{
			return 20;
		}
		return 0;
	}
}
