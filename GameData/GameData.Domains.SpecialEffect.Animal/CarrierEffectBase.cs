using GameData.Common;
using GameData.DomainEvents;

namespace GameData.Domains.SpecialEffect.Animal;

public abstract class CarrierEffectBase : SpecialEffectBase
{
	protected abstract short CombatStateId { get; }

	protected CarrierEffectBase()
	{
	}

	protected CarrierEffectBase(int charId)
		: base(charId, -1)
	{
	}

	public sealed override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		Events.RegisterHandler_CombatBegin(OnCombatBegin);
		Events.RegisterHandler_CombatSettlement(OnCombatSettlement);
		OnEnableSubClass(context);
	}

	public sealed override void OnDisable(DataContext context)
	{
		OnDisableSubClass(context);
		Events.UnRegisterHandler_CombatBegin(OnCombatBegin);
		Events.UnRegisterHandler_CombatSettlement(OnCombatSettlement);
		base.OnDisable(context);
	}

	protected virtual void OnEnableSubClass(DataContext context)
	{
	}

	protected virtual void OnDisableSubClass(DataContext context)
	{
	}

	private void OnCombatBegin(DataContext context)
	{
		DomainManager.Combat.AddCombatState(context, base.CombatChar, 0, CombatStateId, 100, reverse: false, applyEffect: true, base.CharacterId);
	}

	private void OnCombatSettlement(DataContext context, sbyte combatStatus)
	{
		DomainManager.SpecialEffect.Remove(context, Id);
	}
}
