using Config;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai.PrioritizedAction;
using GameData.Domains.Organization;
using GameData.Domains.TaiwuEvent.EventHelper;

namespace GameData.Domains.TaiwuEvent.FunctionDefinition;

public class TaiwuFunctions
{
	[EventFunction(47)]
	private static void AddLegacyPoint(EventScriptRuntime runtime, short templateId)
	{
		DomainManager.Taiwu.AddLegacyPoint(runtime.Context, templateId);
	}

	[EventFunction(48)]
	private static void ExpelTaiwuVillager(EventScriptRuntime runtime, GameData.Domains.Character.Character character)
	{
		DomainManager.Taiwu.ExpelVillager(runtime.Context, character.GetId());
	}

	[EventFunction(49)]
	private static void MakeAppointment(EventScriptRuntime runtime, GameData.Domains.Character.Character character, Settlement settlement)
	{
		DomainManager.Taiwu.AddAppointment(runtime.Context, character.GetId(), settlement.GetId());
	}

	[EventFunction(50)]
	private static void RemoveAppointment(EventScriptRuntime runtime, GameData.Domains.Character.Character character)
	{
		int id = character.GetId();
		if (DomainManager.Character.TryGetCharacterPrioritizedAction(id, out var action) && action is AppointmentAction appointmentAction)
		{
			DomainManager.Extra.SetPrioritizedActionCooldown(runtime.Context, id, appointmentAction.ActionType, PrioritizedActions.Instance[appointmentAction.ActionType].ActionCoolDown);
			DomainManager.Character.RemoveCharacterPrioritizedAction(runtime.Context, id);
		}
	}

	[EventFunction(147)]
	private static void CharacterTeachTaiwuProfession(EventScriptRuntime runtime, GameData.Domains.Character.Character character, int professionId)
	{
		int id = character.GetId();
		DomainManager.Extra.CharacterTeachTaiwuProfession(runtime.Context, id, professionId);
	}

	[EventFunction(156)]
	private static void TriggerLegacyPassingEvent(EventScriptRuntime runtime, bool isTaiwuDying, string onFinishEvent)
	{
		GameData.Domains.TaiwuEvent.EventHelper.EventHelper.TriggerLegacyPassingEvent(isTaiwuDying, onFinishEvent);
	}
}
