using System;

namespace GameData.Domains.TaiwuEvent.EventOption
{
	// Token: 0x020000BB RID: 187
	public class OptionConditionInteractionOffCooldown : TaiwuEventOptionConditionBase
	{
		// Token: 0x06001BBE RID: 7102 RVA: 0x0017DB19 File Offset: 0x0017BD19
		public OptionConditionInteractionOffCooldown(short id, short interactionTemplateId, Func<int, short, bool> checkFunc) : base(id)
		{
			this.InteractionTemplateId = interactionTemplateId;
			this.ConditionChecker = checkFunc;
		}

		// Token: 0x06001BBF RID: 7103 RVA: 0x0017DB34 File Offset: 0x0017BD34
		public override bool CheckCondition(EventArgBox box)
		{
			int charId = -1;
			bool flag = !box.Get("CharacterId", ref charId);
			return !flag && this.ConditionChecker(charId, this.InteractionTemplateId);
		}

		// Token: 0x06001BC0 RID: 7104 RVA: 0x0017DB74 File Offset: 0x0017BD74
		public override ValueTuple<short, string[]> GetDisplayData(EventArgBox box)
		{
			return new ValueTuple<short, string[]>(this.Id, Array.Empty<string>());
		}

		// Token: 0x0400066B RID: 1643
		public readonly short InteractionTemplateId;

		// Token: 0x0400066C RID: 1644
		public readonly Func<int, short, bool> ConditionChecker;
	}
}
