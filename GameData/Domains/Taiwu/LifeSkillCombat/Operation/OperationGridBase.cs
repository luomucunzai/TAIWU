using System;
using System.Runtime.CompilerServices;
using GameData.Domains.Taiwu.LifeSkillCombat.Status;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Operation
{
	// Token: 0x02000064 RID: 100
	[SerializableGameData(NotForDisplayModule = true)]
	public abstract class OperationGridBase : OperationBase
	{
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600159E RID: 5534 RVA: 0x0014B802 File Offset: 0x00149A02
		// (set) Token: 0x0600159F RID: 5535 RVA: 0x0014B80A File Offset: 0x00149A0A
		public int GridIndex { get; private set; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060015A0 RID: 5536 RVA: 0x0014B813 File Offset: 0x00149A13
		// (set) Token: 0x060015A1 RID: 5537 RVA: 0x0014B81B File Offset: 0x00149A1B
		public bool IsActive { get; private set; }

		// Token: 0x060015A2 RID: 5538 RVA: 0x0014B824 File Offset: 0x00149A24
		public OperationGridBase()
		{
		}

		// Token: 0x060015A3 RID: 5539 RVA: 0x0014B830 File Offset: 0x00149A30
		public override string Inspect()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Grid[");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.GridIndex % 7 - 3);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.GridIndex / 7 - 3);
			defaultInterpolatedStringHandler.AppendLiteral("]");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x060015A4 RID: 5540 RVA: 0x0014B898 File Offset: 0x00149A98
		public override int GetSerializedSize()
		{
			int totalSize = 0;
			totalSize += base.GetSerializedSize();
			totalSize += 4;
			return totalSize + 1;
		}

		// Token: 0x060015A5 RID: 5541 RVA: 0x0014B8C0 File Offset: 0x00149AC0
		public unsafe override int Serialize(byte* pData)
		{
			byte* pCurrData = pData + base.Serialize(pData);
			*(int*)pCurrData = this.GridIndex;
			pCurrData += 4;
			*pCurrData = (this.IsActive ? 1 : 0);
			pCurrData++;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x060015A6 RID: 5542 RVA: 0x0014B900 File Offset: 0x00149B00
		public unsafe override int Deserialize(byte* pData)
		{
			byte* pCurrData = pData + base.Deserialize(pData);
			this.GridIndex = *(int*)pCurrData;
			pCurrData += 4;
			this.IsActive = (*pCurrData != 0);
			pCurrData++;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060015A7 RID: 5543 RVA: 0x0014B93F File Offset: 0x00149B3F
		public Coordinate2D<sbyte> Coordinate
		{
			get
			{
				return Grid.ToCenterAnchoredCoordinate(this.GridIndex);
			}
		}

		// Token: 0x060015A8 RID: 5544 RVA: 0x0014B94C File Offset: 0x00149B4C
		public void MakeActive()
		{
			Tester.Assert(!this.IsActive, "");
			this.IsActive = true;
		}

		// Token: 0x060015A9 RID: 5545 RVA: 0x0014B96B File Offset: 0x00149B6B
		public void MakeNonActive()
		{
			Tester.Assert(this.IsActive, "");
			this.IsActive = false;
		}

		// Token: 0x060015AA RID: 5546 RVA: 0x0014B987 File Offset: 0x00149B87
		protected OperationGridBase(sbyte playerId, int stamp, int gridIndex) : base(playerId, stamp)
		{
			this.GridIndex = gridIndex;
		}
	}
}
