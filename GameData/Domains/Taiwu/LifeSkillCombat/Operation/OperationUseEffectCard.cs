using System;
using System.Runtime.CompilerServices;
using Config;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Operation
{
	// Token: 0x0200006B RID: 107
	public class OperationUseEffectCard : OperationBase
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060015E2 RID: 5602 RVA: 0x0014CA82 File Offset: 0x0014AC82
		// (set) Token: 0x060015E3 RID: 5603 RVA: 0x0014CA8A File Offset: 0x0014AC8A
		public OperationUseEffectCard.UseEffectCardInfo Info { get; private set; }

		// Token: 0x060015E4 RID: 5604 RVA: 0x0014CA94 File Offset: 0x0014AC94
		public override string Inspect()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 5);
			defaultInterpolatedStringHandler.AppendFormatted(LifeSkillCombatEffect.Instance[this.Info.EffectCardTemplateId].Name);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Info.CellIndex);
			defaultInterpolatedStringHandler.AppendLiteral("_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Info.CellIndex2);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<sbyte>(this.Info.TargetBookStateIndex);
			defaultInterpolatedStringHandler.AppendLiteral("_");
			defaultInterpolatedStringHandler.AppendFormatted<sbyte>(this.Info.TargetBookOwnerPlayerId);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x060015E5 RID: 5605 RVA: 0x0014CB53 File Offset: 0x0014AD53
		public OperationUseEffectCard()
		{
		}

		// Token: 0x060015E6 RID: 5606 RVA: 0x0014CB5D File Offset: 0x0014AD5D
		public OperationUseEffectCard(sbyte playerId, int stamp, OperationUseEffectCard.UseEffectCardInfo info)
		{
			base.PlayerId = playerId;
			this.Stamp = stamp;
			this.Info = info;
		}

		// Token: 0x060015E7 RID: 5607 RVA: 0x0014CB80 File Offset: 0x0014AD80
		public override int GetSerializedSize()
		{
			int totalSize = base.GetSerializedSize();
			totalSize++;
			totalSize += 4;
			totalSize += 4;
			totalSize++;
			totalSize++;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x0014CBC0 File Offset: 0x0014ADC0
		public unsafe override int Serialize(byte* pData)
		{
			byte* pCurrData = pData + base.Serialize(pData);
			*pCurrData = (byte)this.Info.EffectCardTemplateId;
			pCurrData++;
			*(int*)pCurrData = this.Info.CellIndex;
			pCurrData += 4;
			*(int*)pCurrData = this.Info.CellIndex2;
			pCurrData += 4;
			*pCurrData = (byte)this.Info.TargetBookStateIndex;
			pCurrData++;
			*pCurrData = (byte)this.Info.TargetBookOwnerPlayerId;
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060015E9 RID: 5609 RVA: 0x0014CC4C File Offset: 0x0014AE4C
		public unsafe override int Deserialize(byte* pData)
		{
			byte* pCurrData = pData + base.Deserialize(pData);
			OperationUseEffectCard.UseEffectCardInfo info = default(OperationUseEffectCard.UseEffectCardInfo);
			info.EffectCardTemplateId = *(sbyte*)pCurrData;
			pCurrData++;
			info.CellIndex = *(int*)pCurrData;
			pCurrData += 4;
			info.CellIndex2 = *(int*)pCurrData;
			pCurrData += 4;
			info.TargetBookStateIndex = *(sbyte*)pCurrData;
			pCurrData++;
			info.TargetBookOwnerPlayerId = *(sbyte*)pCurrData;
			pCurrData++;
			this.Info = info;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x02000991 RID: 2449
		public struct UseEffectCardInfo
		{
			// Token: 0x04002846 RID: 10310
			public sbyte EffectCardTemplateId;

			// Token: 0x04002847 RID: 10311
			public int CellIndex;

			// Token: 0x04002848 RID: 10312
			public int CellIndex2;

			// Token: 0x04002849 RID: 10313
			public sbyte TargetBookStateIndex;

			// Token: 0x0400284A RID: 10314
			public sbyte TargetBookOwnerPlayerId;
		}
	}
}
