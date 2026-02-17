using System;
using System.Collections.Generic;

namespace GameData.Domains.TaiwuEvent.EventOption
{
	// Token: 0x020000B2 RID: 178
	public abstract class TaiwuEventOptionConditionBase
	{
		// Token: 0x06001BA2 RID: 7074 RVA: 0x0017D42C File Offset: 0x0017B62C
		public TaiwuEventOptionConditionBase(short id)
		{
			this.Id = id;
		}

		// Token: 0x06001BA3 RID: 7075 RVA: 0x0017D440 File Offset: 0x0017B640
		public virtual bool CheckCondition(EventArgBox box)
		{
			return false;
		}

		// Token: 0x06001BA4 RID: 7076 RVA: 0x0017D453 File Offset: 0x0017B653
		public virtual ValueTuple<short, string[]> GetDisplayData(EventArgBox box)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001BA5 RID: 7077 RVA: 0x0017D45C File Offset: 0x0017B65C
		public virtual List<int> GetCharIdList(EventArgBox box)
		{
			return null;
		}

		// Token: 0x04000656 RID: 1622
		public readonly short Id;

		// Token: 0x04000657 RID: 1623
		public List<TaiwuEventOptionConditionBase> OrConditionCore;
	}
}
