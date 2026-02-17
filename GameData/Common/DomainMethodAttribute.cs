using System;

namespace GameData.Common
{
	// Token: 0x020008F4 RID: 2292
	[AttributeUsage(AttributeTargets.Method)]
	public class DomainMethodAttribute : Attribute
	{
		// Token: 0x04002436 RID: 9270
		public bool IsPassthrough;
	}
}
