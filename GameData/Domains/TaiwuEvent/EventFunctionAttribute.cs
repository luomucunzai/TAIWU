using System;

namespace GameData.Domains.TaiwuEvent
{
	// Token: 0x02000076 RID: 118
	[AttributeUsage(AttributeTargets.Method)]
	public class EventFunctionAttribute : Attribute
	{
		// Token: 0x060016C2 RID: 5826 RVA: 0x00153717 File Offset: 0x00151917
		public EventFunctionAttribute(int id)
		{
			this.Id = id;
		}

		// Token: 0x0400047F RID: 1151
		public readonly int Id;
	}
}
