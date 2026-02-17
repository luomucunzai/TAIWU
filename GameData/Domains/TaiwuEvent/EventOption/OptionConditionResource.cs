using System;

namespace GameData.Domains.TaiwuEvent.EventOption
{
	// Token: 0x020000C4 RID: 196
	public class OptionConditionResource : TaiwuEventOptionConditionBase
	{
		// Token: 0x06001BDB RID: 7131 RVA: 0x0017E438 File Offset: 0x0017C638
		public OptionConditionResource(short id, sbyte resourceType, int count, Func<sbyte, int, bool> conditionMatcher) : base(id)
		{
			this._resourceType = resourceType;
			this._count = count;
			this._conditionMatcher = conditionMatcher;
		}

		// Token: 0x06001BDC RID: 7132 RVA: 0x0017E4B0 File Offset: 0x0017C6B0
		public override bool CheckCondition(EventArgBox box)
		{
			return this._conditionMatcher(this._resourceType, this._count);
		}

		// Token: 0x06001BDD RID: 7133 RVA: 0x0017E4DC File Offset: 0x0017C6DC
		public override ValueTuple<short, string[]> GetDisplayData(EventArgBox box)
		{
			return new ValueTuple<short, string[]>(this.Id, new string[]
			{
				"<Language Key=" + this.ResourceTypeName[(int)this._resourceType] + "/>",
				this._count.ToString()
			});
		}

		// Token: 0x0400067F RID: 1663
		public readonly sbyte _resourceType;

		// Token: 0x04000680 RID: 1664
		public readonly int _count;

		// Token: 0x04000681 RID: 1665
		private Func<sbyte, int, bool> _conditionMatcher;

		// Token: 0x04000682 RID: 1666
		private readonly string[] ResourceTypeName = new string[]
		{
			"LK_Resource_Name_Food",
			"LK_Resource_Name_Wood",
			"LK_Resource_Name_Metal",
			"LK_Resource_Name_Jade",
			"LK_Resource_Name_Fabric",
			"LK_Resource_Name_Herb",
			"LK_Resource_Name_Money",
			"LK_Resource_Name_Authority"
		};
	}
}
