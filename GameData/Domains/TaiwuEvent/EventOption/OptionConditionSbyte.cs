using System;

namespace GameData.Domains.TaiwuEvent.EventOption
{
	// Token: 0x020000B3 RID: 179
	public class OptionConditionSbyte : TaiwuEventOptionConditionBase
	{
		// Token: 0x06001BA6 RID: 7078 RVA: 0x0017D46F File Offset: 0x0017B66F
		public OptionConditionSbyte(short id, sbyte arg, Func<sbyte, bool> checker) : base(id)
		{
			this.Arg = arg;
			this.ConditionChecker = checker;
		}

		// Token: 0x06001BA7 RID: 7079 RVA: 0x0017D488 File Offset: 0x0017B688
		public override bool CheckCondition(EventArgBox box)
		{
			return this.ConditionChecker(this.Arg);
		}

		// Token: 0x06001BA8 RID: 7080 RVA: 0x0017D4AC File Offset: 0x0017B6AC
		public override ValueTuple<short, string[]> GetDisplayData(EventArgBox box)
		{
			return new ValueTuple<short, string[]>(this.Id, new string[]
			{
				this.Arg.ToString()
			});
		}

		// Token: 0x04000658 RID: 1624
		public readonly sbyte Arg;

		// Token: 0x04000659 RID: 1625
		public readonly Func<sbyte, bool> ConditionChecker;
	}
}
