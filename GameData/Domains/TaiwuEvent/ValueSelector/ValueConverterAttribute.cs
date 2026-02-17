using System;

namespace GameData.Domains.TaiwuEvent.ValueSelector
{
	// Token: 0x02000086 RID: 134
	public class ValueConverterAttribute : Attribute
	{
		// Token: 0x060018DF RID: 6367 RVA: 0x00167C85 File Offset: 0x00165E85
		public ValueConverterAttribute(Type srcType, Type dstType)
		{
			this.SrcType = srcType;
			this.DstType = dstType;
		}

		// Token: 0x0400058E RID: 1422
		public readonly Type SrcType;

		// Token: 0x0400058F RID: 1423
		public readonly Type DstType;
	}
}
