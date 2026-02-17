using System;

namespace GameData.Domains.TaiwuEvent.EventOption
{
	// Token: 0x020000BA RID: 186
	public class OptionConditionCooldown : TaiwuEventOptionConditionBase
	{
		// Token: 0x06001BBB RID: 7099 RVA: 0x0017DA82 File Offset: 0x0017BC82
		public OptionConditionCooldown(short id, string coolDownBoxKey, sbyte coolDownArg, Func<string, int, sbyte, bool> checkFunc) : base(id)
		{
			this.BoxKeyPrefix = coolDownBoxKey;
			this.CoolDownMonthCount = coolDownArg;
			this.ConditionChecker = checkFunc;
		}

		// Token: 0x06001BBC RID: 7100 RVA: 0x0017DAA4 File Offset: 0x0017BCA4
		public override bool CheckCondition(EventArgBox box)
		{
			int charId = -1;
			bool flag = box.Get("CharacterId", ref charId);
			return flag && this.ConditionChecker(this.BoxKeyPrefix, charId, this.CoolDownMonthCount);
		}

		// Token: 0x06001BBD RID: 7101 RVA: 0x0017DAE8 File Offset: 0x0017BCE8
		public override ValueTuple<short, string[]> GetDisplayData(EventArgBox box)
		{
			return new ValueTuple<short, string[]>(this.Id, new string[]
			{
				this.CoolDownMonthCount.ToString()
			});
		}

		// Token: 0x04000668 RID: 1640
		public readonly string BoxKeyPrefix;

		// Token: 0x04000669 RID: 1641
		public readonly sbyte CoolDownMonthCount;

		// Token: 0x0400066A RID: 1642
		public readonly Func<string, int, sbyte, bool> ConditionChecker;
	}
}
