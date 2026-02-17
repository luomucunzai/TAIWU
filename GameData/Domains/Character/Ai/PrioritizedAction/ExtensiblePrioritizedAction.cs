using System;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai.PrioritizedAction
{
	// Token: 0x02000859 RID: 2137
	[SerializableGameData(NotForDisplayModule = true)]
	public abstract class ExtensiblePrioritizedAction : BasePrioritizedAction
	{
		// Token: 0x060076DF RID: 30431 RVA: 0x00459550 File Offset: 0x00457750
		public override bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x060076E0 RID: 30432 RVA: 0x00459564 File Offset: 0x00457764
		public override int GetSerializedSize()
		{
			int totalSize = 19;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060076E1 RID: 30433 RVA: 0x0045958C File Offset: 0x0045778C
		public unsafe override int Serialize(byte* pData)
		{
			*(short*)pData = 2;
			byte* pCurrData = pData + 2;
			pCurrData += this.Target.Serialize(pCurrData);
			*pCurrData = (this.HasArrived ? 1 : 0);
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060076E2 RID: 30434 RVA: 0x004595DC File Offset: 0x004577DC
		public unsafe override int Deserialize(byte* pData)
		{
			ushort fieldCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = fieldCount > 0;
			if (flag)
			{
				pCurrData += this.Target.Deserialize(pCurrData);
			}
			bool flag2 = fieldCount > 1;
			if (flag2)
			{
				this.HasArrived = (*pCurrData != 0);
				pCurrData++;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x02000C3D RID: 3133
		private static class FieldIds
		{
			// Token: 0x0400356C RID: 13676
			public const ushort Target = 0;

			// Token: 0x0400356D RID: 13677
			public const ushort HasArrived = 1;

			// Token: 0x0400356E RID: 13678
			public const ushort Count = 2;

			// Token: 0x0400356F RID: 13679
			public static readonly string[] FieldId2FieldName = new string[]
			{
				"Target",
				"HasArrived"
			};
		}
	}
}
