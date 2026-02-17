using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong
{
	// Token: 0x02000578 RID: 1400
	public class ReduceFiveElementsDamage : CombatSkillEffectBase
	{
		// Token: 0x06004160 RID: 16736 RVA: 0x00262A07 File Offset: 0x00260C07
		protected ReduceFiveElementsDamage()
		{
		}

		// Token: 0x06004161 RID: 16737 RVA: 0x00262A11 File Offset: 0x00260C11
		protected ReduceFiveElementsDamage(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06004162 RID: 16738 RVA: 0x00262A20 File Offset: 0x00260C20
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 102, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 117, -1, -1, -1, -1), EDataModifyType.AddPercent);
		}

		// Token: 0x06004163 RID: 16739 RVA: 0x00262A78 File Offset: 0x00260C78
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				sbyte selfNeiliFiveElementsType = (sbyte)NeiliType.Instance[this.CharObj.GetNeiliType()].FiveElements;
				bool flag2 = dataKey.CombatSkillId >= 0 && selfNeiliFiveElementsType == this.RequireSelfFiveElementsType && base.FiveElementsEquals(dataKey, this.AffectFiveElementsType);
				if (flag2)
				{
					bool flag3 = dataKey.FieldId == 117;
					if (flag3)
					{
						return -50;
					}
					bool flag4 = dataKey.FieldId == 102;
					if (flag4)
					{
						return -30;
					}
				}
				result = 0;
			}
			return result;
		}

		// Token: 0x06004164 RID: 16740 RVA: 0x00262B14 File Offset: 0x00260D14
		protected override int GetSubClassSerializedSize()
		{
			return base.GetSubClassSerializedSize() + 1;
		}

		// Token: 0x06004165 RID: 16741 RVA: 0x00262B30 File Offset: 0x00260D30
		protected unsafe override int SerializeSubClass(byte* pData)
		{
			byte* pCurrData = pData + base.SerializeSubClass(pData);
			*pCurrData = (byte)this.RequireSelfFiveElementsType;
			pCurrData++;
			*pCurrData = (byte)this.AffectFiveElementsType;
			return this.GetSubClassSerializedSize();
		}

		// Token: 0x06004166 RID: 16742 RVA: 0x00262B68 File Offset: 0x00260D68
		protected unsafe override int DeserializeSubClass(byte* pData)
		{
			byte* pCurrData = pData + base.DeserializeSubClass(pData);
			this.RequireSelfFiveElementsType = *(sbyte*)pCurrData;
			pCurrData++;
			this.AffectFiveElementsType = *(sbyte*)pCurrData;
			return this.GetSubClassSerializedSize();
		}

		// Token: 0x04001347 RID: 4935
		private const sbyte DirectDamageReducePercent = -30;

		// Token: 0x04001348 RID: 4936
		private const sbyte ReduceGongMadInjury = -50;

		// Token: 0x04001349 RID: 4937
		protected sbyte RequireSelfFiveElementsType;

		// Token: 0x0400134A RID: 4938
		protected sbyte AffectFiveElementsType;
	}
}
