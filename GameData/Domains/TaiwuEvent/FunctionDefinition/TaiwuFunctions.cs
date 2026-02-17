using System;
using Config;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai.PrioritizedAction;
using GameData.Domains.Organization;
using GameData.Domains.TaiwuEvent.EventHelper;

namespace GameData.Domains.TaiwuEvent.FunctionDefinition
{
	// Token: 0x020000AB RID: 171
	public class TaiwuFunctions
	{
		// Token: 0x06001B39 RID: 6969 RVA: 0x0017B747 File Offset: 0x00179947
		[EventFunction(47)]
		private static void AddLegacyPoint(EventScriptRuntime runtime, short templateId)
		{
			DomainManager.Taiwu.AddLegacyPoint(runtime.Context, templateId, 100);
		}

		// Token: 0x06001B3A RID: 6970 RVA: 0x0017B75E File Offset: 0x0017995E
		[EventFunction(48)]
		private static void ExpelTaiwuVillager(EventScriptRuntime runtime, GameData.Domains.Character.Character character)
		{
			DomainManager.Taiwu.ExpelVillager(runtime.Context, character.GetId());
		}

		// Token: 0x06001B3B RID: 6971 RVA: 0x0017B778 File Offset: 0x00179978
		[EventFunction(49)]
		private static void MakeAppointment(EventScriptRuntime runtime, GameData.Domains.Character.Character character, Settlement settlement)
		{
			DomainManager.Taiwu.AddAppointment(runtime.Context, character.GetId(), settlement.GetId());
		}

		// Token: 0x06001B3C RID: 6972 RVA: 0x0017B798 File Offset: 0x00179998
		[EventFunction(50)]
		private static void RemoveAppointment(EventScriptRuntime runtime, GameData.Domains.Character.Character character)
		{
			int charId = character.GetId();
			BasePrioritizedAction action;
			AppointmentAction appointmentAction;
			bool flag;
			if (DomainManager.Character.TryGetCharacterPrioritizedAction(charId, out action))
			{
				appointmentAction = (action as AppointmentAction);
				flag = (appointmentAction == null);
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			if (!flag2)
			{
				DomainManager.Extra.SetPrioritizedActionCooldown(runtime.Context, charId, appointmentAction.ActionType, PrioritizedActions.Instance[appointmentAction.ActionType].ActionCoolDown);
				DomainManager.Character.RemoveCharacterPrioritizedAction(runtime.Context, charId);
			}
		}

		// Token: 0x06001B3D RID: 6973 RVA: 0x0017B814 File Offset: 0x00179A14
		[EventFunction(147)]
		private static void CharacterTeachTaiwuProfession(EventScriptRuntime runtime, GameData.Domains.Character.Character character, int professionId)
		{
			int charId = character.GetId();
			DomainManager.Extra.CharacterTeachTaiwuProfession(runtime.Context, charId, professionId);
		}

		// Token: 0x06001B3E RID: 6974 RVA: 0x0017B83C File Offset: 0x00179A3C
		[EventFunction(156)]
		private static void TriggerLegacyPassingEvent(EventScriptRuntime runtime, bool isTaiwuDying, string onFinishEvent)
		{
			EventHelper.TriggerLegacyPassingEvent(isTaiwuDying, onFinishEvent);
		}
	}
}
