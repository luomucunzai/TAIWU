using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong
{
	// Token: 0x02000570 RID: 1392
	public class BaseSectNeigong : CombatSkillEffectBase
	{
		// Token: 0x0600410F RID: 16655 RVA: 0x002614F6 File Offset: 0x0025F6F6
		protected BaseSectNeigong()
		{
		}

		// Token: 0x06004110 RID: 16656 RVA: 0x00261500 File Offset: 0x0025F700
		protected BaseSectNeigong(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06004111 RID: 16657 RVA: 0x00261510 File Offset: 0x0025F710
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 200, -1, -1, -1, -1), EDataModifyType.Add);
			}
			else
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 202, -1, -1, -1, -1), EDataModifyType.Add);
			}
		}

		// Token: 0x06004112 RID: 16658 RVA: 0x00261578 File Offset: 0x0025F778
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
				bool flag2 = dataKey.CombatSkillId >= 0 && Config.CombatSkill.Instance[dataKey.CombatSkillId].SectId == this.SectId;
				if (flag2)
				{
					bool flag3 = dataKey.FieldId == 200;
					if (flag3)
					{
						return 30;
					}
					bool flag4 = dataKey.FieldId == 202;
					if (flag4)
					{
						return -10;
					}
				}
				result = 0;
			}
			return result;
		}

		// Token: 0x06004113 RID: 16659 RVA: 0x00261604 File Offset: 0x0025F804
		protected override int GetSubClassSerializedSize()
		{
			return base.GetSubClassSerializedSize() + 1;
		}

		// Token: 0x06004114 RID: 16660 RVA: 0x00261620 File Offset: 0x0025F820
		protected unsafe override int SerializeSubClass(byte* pData)
		{
			byte* pCurrData = pData + base.SerializeSubClass(pData);
			*pCurrData = (byte)this.SectId;
			return this.GetSubClassSerializedSize();
		}

		// Token: 0x06004115 RID: 16661 RVA: 0x0026164C File Offset: 0x0025F84C
		protected unsafe override int DeserializeSubClass(byte* pData)
		{
			byte* pCurrData = pData + base.DeserializeSubClass(pData);
			this.SectId = *(sbyte*)pCurrData;
			return this.GetSubClassSerializedSize();
		}

		// Token: 0x04001324 RID: 4900
		private const sbyte DirectAddMaxPower = 30;

		// Token: 0x04001325 RID: 4901
		private const sbyte ReverseReduceRequirementPercent = -10;

		// Token: 0x04001326 RID: 4902
		protected sbyte SectId;
	}
}
