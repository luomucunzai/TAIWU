using System;

namespace GameData.Domains.TaiwuEvent.EventOption
{
	// Token: 0x020000C5 RID: 197
	public class OptionConditionJiaoIsNotFostered : TaiwuEventOptionConditionBase
	{
		// Token: 0x06001BDE RID: 7134 RVA: 0x0017E52C File Offset: 0x0017C72C
		public OptionConditionJiaoIsNotFostered(short id, Func<int, bool> conditionMatcher) : base(id)
		{
			this._conditionMatcher = conditionMatcher;
		}

		// Token: 0x06001BDF RID: 7135 RVA: 0x0017E540 File Offset: 0x0017C740
		public override bool CheckCondition(EventArgBox box)
		{
			return this._conditionMatcher(box.GetInt("PoolId"));
		}

		// Token: 0x06001BE0 RID: 7136 RVA: 0x0017E568 File Offset: 0x0017C768
		public override ValueTuple<short, string[]> GetDisplayData(EventArgBox box)
		{
			return new ValueTuple<short, string[]>(this.Id, Array.Empty<string>());
		}

		// Token: 0x04000683 RID: 1667
		private readonly Func<int, bool> _conditionMatcher;
	}
}
