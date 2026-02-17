using System;

namespace SerializableGameDataSourceGenerator
{
	// Token: 0x0200000F RID: 15
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class AutoGenerateSerializableGameDataAttribute : Attribute
	{
		// Token: 0x06000033 RID: 51 RVA: 0x0004F147 File Offset: 0x0004D347
		public AutoGenerateSerializableGameDataAttribute()
		{
			this.NotForDisplayModule = false;
			this.NotForArchive = false;
			this.NotRestrictCollectionSerializedSize = false;
			this.IsExtensible = false;
			this.NoCopyConstructors = false;
		}

		// Token: 0x04000013 RID: 19
		public bool NotForDisplayModule;

		// Token: 0x04000014 RID: 20
		public bool NotForArchive;

		// Token: 0x04000015 RID: 21
		public bool NotRestrictCollectionSerializedSize;

		// Token: 0x04000016 RID: 22
		public bool IsExtensible;

		// Token: 0x04000017 RID: 23
		public bool NoCopyConstructors;
	}
}
