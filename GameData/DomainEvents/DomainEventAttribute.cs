using System;

namespace GameData.DomainEvents
{
	// Token: 0x020008D5 RID: 2261
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Delegate)]
	public class DomainEventAttribute : Attribute
	{
		// Token: 0x04002321 RID: 8993
		public int MaxReenterCount = 0;
	}
}
