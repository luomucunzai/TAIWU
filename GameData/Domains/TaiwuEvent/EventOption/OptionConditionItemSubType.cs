using System;
using System.Runtime.CompilerServices;

namespace GameData.Domains.TaiwuEvent.EventOption
{
	// Token: 0x020000BE RID: 190
	public class OptionConditionItemSubType : TaiwuEventOptionConditionBase
	{
		// Token: 0x06001BC7 RID: 7111 RVA: 0x0017DD55 File Offset: 0x0017BF55
		public OptionConditionItemSubType(short id, int arg, Func<int, bool> checkFunc) : base(id)
		{
			this.Arg = arg;
			this.ConditionChecker = checkFunc;
		}

		// Token: 0x06001BC8 RID: 7112 RVA: 0x0017DD70 File Offset: 0x0017BF70
		public override bool CheckCondition(EventArgBox box)
		{
			return this.ConditionChecker(this.Arg);
		}

		// Token: 0x06001BC9 RID: 7113 RVA: 0x0017DD94 File Offset: 0x0017BF94
		public override ValueTuple<short, string[]> GetDisplayData(EventArgBox box)
		{
			short id = this.Id;
			string[] array = new string[1];
			int num = 0;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 1);
			defaultInterpolatedStringHandler.AppendLiteral("<Language Key=LK_ItemSubType_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Arg);
			defaultInterpolatedStringHandler.AppendLiteral("/>");
			array[num] = defaultInterpolatedStringHandler.ToStringAndClear();
			return new ValueTuple<short, string[]>(id, array);
		}

		// Token: 0x04000673 RID: 1651
		public readonly int Arg;

		// Token: 0x04000674 RID: 1652
		public readonly Func<int, bool> ConditionChecker;
	}
}
