using System;
using GameData.Common;

namespace GameData.Dependencies
{
	// Token: 0x020008E0 RID: 2272
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public class BaseDataDependencyAttribute : Attribute
	{
		// Token: 0x040023C9 RID: 9161
		public DomainDataType SourceType;

		// Token: 0x040023CA RID: 9162
		public DataUid[] SourceUids;

		// Token: 0x040023CB RID: 9163
		public InfluenceCondition Condition;

		// Token: 0x040023CC RID: 9164
		public InfluenceScope Scope;
	}
}
