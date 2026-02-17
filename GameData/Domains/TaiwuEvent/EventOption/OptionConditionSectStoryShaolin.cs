using System;

namespace GameData.Domains.TaiwuEvent.EventOption
{
	// Token: 0x020000C3 RID: 195
	public class OptionConditionSectStoryShaolin : TaiwuEventOptionConditionBase
	{
		// Token: 0x06001BD8 RID: 7128 RVA: 0x0017E3E2 File Offset: 0x0017C5E2
		public OptionConditionSectStoryShaolin(short id, Func<bool> conditionMatcher) : base(id)
		{
			this._conditionMatcher = conditionMatcher;
		}

		// Token: 0x06001BD9 RID: 7129 RVA: 0x0017E3F4 File Offset: 0x0017C5F4
		public override bool CheckCondition(EventArgBox box)
		{
			return this._conditionMatcher();
		}

		// Token: 0x06001BDA RID: 7130 RVA: 0x0017E414 File Offset: 0x0017C614
		public override ValueTuple<short, string[]> GetDisplayData(EventArgBox box)
		{
			return new ValueTuple<short, string[]>(this.Id, Array.Empty<string>());
		}

		// Token: 0x0400067E RID: 1662
		private Func<bool> _conditionMatcher;
	}
}
