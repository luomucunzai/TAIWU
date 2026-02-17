using System;
using GameData.Domains.Character;

namespace GameData.Domains.TaiwuEvent.EventOption
{
	// Token: 0x020000BC RID: 188
	public class OptionConditionTaiwuCooldown : TaiwuEventOptionConditionBase
	{
		// Token: 0x06001BC1 RID: 7105 RVA: 0x0017DB96 File Offset: 0x0017BD96
		public OptionConditionTaiwuCooldown(short id, string coolDownBoxKey, sbyte coolDownArg, Func<string, int, sbyte, bool> checkFunc) : base(id)
		{
			this.BoxKeyPrefix = coolDownBoxKey;
			this.CoolDownMonthCount = coolDownArg;
			this.ConditionChecker = checkFunc;
		}

		// Token: 0x06001BC2 RID: 7106 RVA: 0x0017DBB8 File Offset: 0x0017BDB8
		public override bool CheckCondition(EventArgBox box)
		{
			Character taiwu = DomainManager.Taiwu.GetTaiwu();
			return this.ConditionChecker(this.BoxKeyPrefix, taiwu.GetId(), this.CoolDownMonthCount);
		}

		// Token: 0x06001BC3 RID: 7107 RVA: 0x0017DBF4 File Offset: 0x0017BDF4
		public override ValueTuple<short, string[]> GetDisplayData(EventArgBox box)
		{
			return new ValueTuple<short, string[]>(this.Id, new string[]
			{
				this.CoolDownMonthCount.ToString()
			});
		}

		// Token: 0x0400066D RID: 1645
		public readonly string BoxKeyPrefix;

		// Token: 0x0400066E RID: 1646
		public readonly sbyte CoolDownMonthCount;

		// Token: 0x0400066F RID: 1647
		public readonly Func<string, int, sbyte, bool> ConditionChecker;
	}
}
