using System;
using System.Runtime.CompilerServices;
using GameData.Domains.Information;
using GameData.Domains.Item;
using GameData.Domains.Item.Display;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Operation
{
	// Token: 0x02000062 RID: 98
	public class OperationForceAdversary : OperationBase
	{
		// Token: 0x06001590 RID: 5520 RVA: 0x0014B4CC File Offset: 0x001496CC
		public override string Inspect()
		{
			bool flag = this.SecretInformationDisplayPackage != null;
			string result;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 1);
				defaultInterpolatedStringHandler.AppendLiteral("via SecretInformationMetaDataId ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.SecretInformationDisplayPackage.SecretInformationDisplayDataList[0].SecretInformationMetaDataId);
				result = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			else
			{
				bool flag2 = this.ItemDisplayData != null;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(10, 1);
					defaultInterpolatedStringHandler.AppendLiteral("via Item $");
					defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(this.ItemDisplayData.Key);
					result = defaultInterpolatedStringHandler.ToStringAndClear();
				}
				else
				{
					result = string.Empty;
				}
			}
			return result;
		}

		// Token: 0x06001591 RID: 5521 RVA: 0x0014B570 File Offset: 0x00149770
		public OperationForceAdversary()
		{
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x0014B57C File Offset: 0x0014977C
		public override int GetSerializedSize()
		{
			int totalSize = base.GetSerializedSize();
			totalSize++;
			bool flag = this.SecretInformationDisplayPackage != null;
			if (flag)
			{
				totalSize += this.SecretInformationDisplayPackage.GetSerializedSize();
			}
			totalSize++;
			bool flag2 = this.ItemDisplayData != null;
			if (flag2)
			{
				totalSize += this.ItemDisplayData.GetSerializedSize();
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x0014B5E8 File Offset: 0x001497E8
		public unsafe override int Serialize(byte* pData)
		{
			byte* pCurrData = pData + base.Serialize(pData);
			*pCurrData = ((this.SecretInformationDisplayPackage != null) ? 1 : 0);
			pCurrData++;
			bool flag = this.SecretInformationDisplayPackage != null;
			if (flag)
			{
				pCurrData += this.SecretInformationDisplayPackage.Serialize(pCurrData);
			}
			*pCurrData = ((this.ItemDisplayData != null) ? 1 : 0);
			pCurrData++;
			bool flag2 = this.ItemDisplayData != null;
			if (flag2)
			{
				pCurrData += this.ItemDisplayData.Serialize(pCurrData);
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x0014B678 File Offset: 0x00149878
		public unsafe override int Deserialize(byte* pData)
		{
			byte* pCurrData = pData + base.Deserialize(pData);
			bool exist = *pCurrData != 0;
			pCurrData++;
			this.SecretInformationDisplayPackage = null;
			bool flag = exist;
			if (flag)
			{
				this.SecretInformationDisplayPackage = new SecretInformationDisplayPackage();
				pCurrData += this.SecretInformationDisplayPackage.Deserialize(pCurrData);
			}
			bool exist2 = *pCurrData != 0;
			pCurrData++;
			this.ItemDisplayData = null;
			bool flag2 = exist2;
			if (flag2)
			{
				this.ItemDisplayData = new ItemDisplayData();
				pCurrData += this.ItemDisplayData.Deserialize(pCurrData);
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x0014B718 File Offset: 0x00149918
		public OperationForceAdversary(sbyte playerId, int stamp, SecretInformationDisplayPackage secretInformationDisplayPackage) : base(playerId, stamp)
		{
			this.SecretInformationDisplayPackage = secretInformationDisplayPackage;
		}

		// Token: 0x06001596 RID: 5526 RVA: 0x0014B72B File Offset: 0x0014992B
		public OperationForceAdversary(sbyte playerId, int stamp, ItemDisplayData itemDisplayData) : base(playerId, stamp)
		{
			this.ItemDisplayData = itemDisplayData;
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x0014B73E File Offset: 0x0014993E
		public OperationForceAdversary(sbyte playerId, int stamp) : base(playerId, stamp)
		{
		}

		// Token: 0x0400037F RID: 895
		public SecretInformationDisplayPackage SecretInformationDisplayPackage;

		// Token: 0x04000380 RID: 896
		public ItemDisplayData ItemDisplayData;
	}
}
