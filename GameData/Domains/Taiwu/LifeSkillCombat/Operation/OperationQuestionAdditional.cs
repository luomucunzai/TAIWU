using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Operation
{
	// Token: 0x02000068 RID: 104
	public class OperationQuestionAdditional : OperationQuestion
	{
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060015D0 RID: 5584 RVA: 0x0014C84D File Offset: 0x0014AA4D
		// (set) Token: 0x060015D1 RID: 5585 RVA: 0x0014C855 File Offset: 0x0014AA55
		public sbyte RequiredCardTemplateId { get; private set; }

		// Token: 0x060015D2 RID: 5586 RVA: 0x0014C85E File Offset: 0x0014AA5E
		public OperationQuestionAdditional()
		{
		}

		// Token: 0x060015D3 RID: 5587 RVA: 0x0014C868 File Offset: 0x0014AA68
		public override string Inspect()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(14, 2);
			defaultInterpolatedStringHandler.AppendFormatted(base.Inspect());
			defaultInterpolatedStringHandler.AppendLiteral(" with cardId[");
			defaultInterpolatedStringHandler.AppendFormatted<sbyte>(this.RequiredCardTemplateId);
			defaultInterpolatedStringHandler.AppendLiteral("]");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x060015D4 RID: 5588 RVA: 0x0014C8BC File Offset: 0x0014AABC
		public override int GetSerializedSize()
		{
			int totalSize = base.GetSerializedSize();
			totalSize++;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060015D5 RID: 5589 RVA: 0x0014C8EC File Offset: 0x0014AAEC
		public unsafe override int Serialize(byte* pData)
		{
			byte* pCurrData = pData + base.Serialize(pData);
			*pCurrData = (byte)this.RequiredCardTemplateId;
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060015D6 RID: 5590 RVA: 0x0014C930 File Offset: 0x0014AB30
		public unsafe override int Deserialize(byte* pData)
		{
			byte* pCurrData = pData + base.Deserialize(pData);
			this.RequiredCardTemplateId = *(sbyte*)pCurrData;
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060015D7 RID: 5591 RVA: 0x0014C973 File Offset: 0x0014AB73
		public override IEnumerable<sbyte> PickEffectiveEffectCards(IEnumerable<sbyte> wantUseEffectCardIds)
		{
			foreach (sbyte effectCardId in base.PickEffectiveEffectCards(wantUseEffectCardIds))
			{
				yield return effectCardId;
			}
			IEnumerator<sbyte> enumerator = null;
			yield return this.RequiredCardTemplateId;
			yield break;
			yield break;
		}

		// Token: 0x060015D8 RID: 5592 RVA: 0x0014C98A File Offset: 0x0014AB8A
		public OperationQuestionAdditional(sbyte playerId, int stamp, int gridIndex, int basePoint, IEnumerable<sbyte> wantUseEffectCards, sbyte requiredCardTemplateId) : base(playerId, stamp, gridIndex, basePoint, wantUseEffectCards)
		{
			this.RequiredCardTemplateId = requiredCardTemplateId;
		}
	}
}
