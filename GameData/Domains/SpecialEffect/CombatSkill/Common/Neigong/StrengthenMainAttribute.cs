using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong
{
	// Token: 0x0200057E RID: 1406
	public class StrengthenMainAttribute : CombatSkillEffectBase
	{
		// Token: 0x1700027E RID: 638
		// (get) Token: 0x0600419E RID: 16798 RVA: 0x00263508 File Offset: 0x00261708
		protected virtual bool ConsummateLevelRelatedMainAttributesHitAvoid
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x0600419F RID: 16799 RVA: 0x0026350B File Offset: 0x0026170B
		protected virtual bool ConsummateLevelRelatedMainAttributesPenetrations
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060041A0 RID: 16800 RVA: 0x0026350E File Offset: 0x0026170E
		protected StrengthenMainAttribute()
		{
		}

		// Token: 0x060041A1 RID: 16801 RVA: 0x00263518 File Offset: 0x00261718
		protected StrengthenMainAttribute(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x060041A2 RID: 16802 RVA: 0x00263528 File Offset: 0x00261728
		public override void OnEnable(DataContext context)
		{
			this._fieldId = (ushort)(1 + this.MainAttributeType);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, this._fieldId, -1, -1, -1, -1), EDataModifyType.Add);
			bool consummateLevelRelatedMainAttributesHitAvoid = this.ConsummateLevelRelatedMainAttributesHitAvoid;
			if (consummateLevelRelatedMainAttributesHitAvoid)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 237 : 236, -1, -1, -1, -1), EDataModifyType.Custom);
			}
			bool consummateLevelRelatedMainAttributesPenetrations = this.ConsummateLevelRelatedMainAttributesPenetrations;
			if (consummateLevelRelatedMainAttributesPenetrations)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 239 : 238, -1, -1, -1, -1), EDataModifyType.Custom);
			}
		}

		// Token: 0x060041A3 RID: 16803 RVA: 0x002635E8 File Offset: 0x002617E8
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
				bool flag2 = dataKey.FieldId == this._fieldId;
				if (flag2)
				{
					result = 30;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x060041A4 RID: 16804 RVA: 0x0026362C File Offset: 0x0026182C
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CustomParam0 != (int)this.MainAttributeType;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId - 236 <= 3;
				result = (flag2 || dataValue);
			}
			return result;
		}

		// Token: 0x060041A5 RID: 16805 RVA: 0x00263684 File Offset: 0x00261884
		protected override int GetSubClassSerializedSize()
		{
			return base.GetSubClassSerializedSize() + 1;
		}

		// Token: 0x060041A6 RID: 16806 RVA: 0x002636A0 File Offset: 0x002618A0
		protected unsafe override int SerializeSubClass(byte* pData)
		{
			byte* pCurrData = pData + base.SerializeSubClass(pData);
			*pCurrData = (byte)this.MainAttributeType;
			return this.GetSubClassSerializedSize();
		}

		// Token: 0x060041A7 RID: 16807 RVA: 0x002636CC File Offset: 0x002618CC
		protected unsafe override int DeserializeSubClass(byte* pData)
		{
			byte* pCurrData = pData + base.DeserializeSubClass(pData);
			this.MainAttributeType = *(sbyte*)pCurrData;
			return this.GetSubClassSerializedSize();
		}

		// Token: 0x04001353 RID: 4947
		private const sbyte AddMaxValue = 30;

		// Token: 0x04001354 RID: 4948
		protected sbyte MainAttributeType;

		// Token: 0x04001355 RID: 4949
		private ushort _fieldId;
	}
}
