using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong
{
	// Token: 0x0200057F RID: 1407
	public class TransferFiveElementsNeili : CombatSkillEffectBase
	{
		// Token: 0x060041A8 RID: 16808 RVA: 0x002636F6 File Offset: 0x002618F6
		protected TransferFiveElementsNeili()
		{
		}

		// Token: 0x060041A9 RID: 16809 RVA: 0x00263700 File Offset: 0x00261900
		protected TransferFiveElementsNeili(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x060041AA RID: 16810 RVA: 0x00263710 File Offset: 0x00261910
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 26, -1, -1, -1, -1), EDataModifyType.Custom);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 234, -1, -1, -1, -1), EDataModifyType.Custom);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, -1, -1, -1, -1), EDataModifyType.AddPercent);
		}

		// Token: 0x060041AB RID: 16811 RVA: 0x0026378C File Offset: 0x0026198C
		public unsafe override NeiliProportionOfFiveElements GetModifiedValue(AffectedDataKey dataKey, NeiliProportionOfFiveElements dataValue)
		{
			ref sbyte ptr = ref dataValue.Items.FixedElementField + this.DestFiveElementsType;
			ptr += *(ref dataValue.Items.FixedElementField + this.SrcFiveElementsType);
			*(ref dataValue.Items.FixedElementField + this.SrcFiveElementsType) = 0;
			return dataValue;
		}

		// Token: 0x060041AC RID: 16812 RVA: 0x002637E0 File Offset: 0x002619E0
		public unsafe override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId < 0 || dataKey.FieldId != 234;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = *(ref this.CharObj.GetBaseNeiliProportionOfFiveElements().Items.FixedElementField + this.SrcFiveElementsType) > 0;
				if (flag2)
				{
					result = dataValue;
				}
				else
				{
					bool flag3 = !base.FiveElementsEqualsEnemy(dataKey.CombatSkillId, this.SrcFiveElementsType) && !base.FiveElementsEqualsEnemy(dataKey.CombatSkillId, this.DestFiveElementsType);
					if (flag3)
					{
						result = dataValue;
					}
					else
					{
						base.ShowSpecialEffectTips(0);
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x060041AD RID: 16813 RVA: 0x00263890 File Offset: 0x00261A90
		public unsafe override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 199 && dataKey.CombatSkillId >= 0 && base.FiveElementsEquals(dataKey, this.DestFiveElementsType);
				if (flag2)
				{
					NeiliProportionOfFiveElements proportion = this.CharObj.GetNeiliProportionOfFiveElements();
					NeiliProportionOfFiveElements baseProportion = this.CharObj.GetBaseNeiliProportionOfFiveElements();
					result = Math.Max((int)(*(ref proportion.Items.FixedElementField + this.DestFiveElementsType) - *(ref baseProportion.Items.FixedElementField + this.DestFiveElementsType)), 0) / 10;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x060041AE RID: 16814 RVA: 0x00263938 File Offset: 0x00261B38
		protected override int GetSubClassSerializedSize()
		{
			return base.GetSubClassSerializedSize() + 1 + 1;
		}

		// Token: 0x060041AF RID: 16815 RVA: 0x00263954 File Offset: 0x00261B54
		protected unsafe override int SerializeSubClass(byte* pData)
		{
			byte* pCurrData = pData + base.SerializeSubClass(pData);
			*pCurrData = (byte)this.SrcFiveElementsType;
			pCurrData++;
			*pCurrData = (byte)this.DestFiveElementsType;
			return this.GetSubClassSerializedSize();
		}

		// Token: 0x060041B0 RID: 16816 RVA: 0x0026398C File Offset: 0x00261B8C
		protected unsafe override int DeserializeSubClass(byte* pData)
		{
			byte* pCurrData = pData + base.DeserializeSubClass(pData);
			this.SrcFiveElementsType = *(sbyte*)pCurrData;
			pCurrData++;
			this.DestFiveElementsType = *(sbyte*)pCurrData;
			return this.GetSubClassSerializedSize();
		}

		// Token: 0x04001356 RID: 4950
		private const sbyte AddPowerRatio = 10;

		// Token: 0x04001357 RID: 4951
		protected sbyte SrcFiveElementsType;

		// Token: 0x04001358 RID: 4952
		protected sbyte DestFiveElementsType;
	}
}
