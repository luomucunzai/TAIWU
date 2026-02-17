using System;

namespace GameData.Domains.TaiwuEvent.EventOption
{
	// Token: 0x020000B4 RID: 180
	public class OptionConditionInt : TaiwuEventOptionConditionBase
	{
		// Token: 0x06001BA9 RID: 7081 RVA: 0x0017D4DD File Offset: 0x0017B6DD
		public OptionConditionInt(short id, int arg, Func<int, bool> checkFunc) : base(id)
		{
			this.Arg = arg;
			this.ConditionChecker = checkFunc;
		}

		// Token: 0x06001BAA RID: 7082 RVA: 0x0017D4F8 File Offset: 0x0017B6F8
		public override bool CheckCondition(EventArgBox box)
		{
			return this.ConditionChecker(this.Arg);
		}

		// Token: 0x06001BAB RID: 7083 RVA: 0x0017D51C File Offset: 0x0017B71C
		public override ValueTuple<short, string[]> GetDisplayData(EventArgBox box)
		{
			return new ValueTuple<short, string[]>(this.Id, new string[]
			{
				this.Arg.ToString()
			});
		}

		// Token: 0x0400065A RID: 1626
		public readonly int Arg;

		// Token: 0x0400065B RID: 1627
		public readonly Func<int, bool> ConditionChecker;
	}
}
