using System;

namespace GameData.Common
{
	// Token: 0x020008FA RID: 2298
	[AttributeUsage(AttributeTargets.Field)]
	public class ParallelModificationAttribute : Attribute
	{
		// Token: 0x0600824A RID: 33354 RVA: 0x004DA25E File Offset: 0x004D845E
		public ParallelModificationAttribute(ushort domainId, string methodName)
		{
			this.DomainId = domainId;
			this.MethodName = methodName;
		}

		// Token: 0x0600824B RID: 33355 RVA: 0x004DA276 File Offset: 0x004D8476
		public ParallelModificationAttribute(string className, string methodName)
		{
			this.DomainId = ushort.MaxValue;
			this.ClassName = className;
			this.MethodName = methodName;
		}

		// Token: 0x04002447 RID: 9287
		public readonly ushort DomainId;

		// Token: 0x04002448 RID: 9288
		public readonly string ClassName;

		// Token: 0x04002449 RID: 9289
		public readonly string MethodName;
	}
}
