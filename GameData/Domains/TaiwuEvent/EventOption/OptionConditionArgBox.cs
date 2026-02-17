using System;
using System.Collections.Generic;

namespace GameData.Domains.TaiwuEvent.EventOption
{
	// Token: 0x020000C6 RID: 198
	public class OptionConditionArgBox : TaiwuEventOptionConditionBase
	{
		// Token: 0x06001BE1 RID: 7137 RVA: 0x0017E58A File Offset: 0x0017C78A
		public OptionConditionArgBox(short id, List<string> argBoxKeys, Func<EventArgBox, bool> checker) : base(id)
		{
			this._argBoxKeys = argBoxKeys;
			this.ConditionChecker = checker;
		}

		// Token: 0x06001BE2 RID: 7138 RVA: 0x0017E5A4 File Offset: 0x0017C7A4
		public override bool CheckCondition(EventArgBox box)
		{
			bool flag = box == null;
			return !flag && this.ConditionChecker(box);
		}

		// Token: 0x06001BE3 RID: 7139 RVA: 0x0017E5D0 File Offset: 0x0017C7D0
		public override ValueTuple<short, string[]> GetDisplayData(EventArgBox box)
		{
			List<string> res = new List<string>();
			foreach (string key in this._argBoxKeys)
			{
				string s = "";
				box.Get(key, ref s);
				res.Add(s);
			}
			return new ValueTuple<short, string[]>(this.Id, res.ToArray());
		}

		// Token: 0x04000684 RID: 1668
		public readonly List<string> _argBoxKeys;

		// Token: 0x04000685 RID: 1669
		public readonly Func<EventArgBox, bool> ConditionChecker;
	}
}
