using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong
{
	// Token: 0x0200057B RID: 1403
	public abstract class StrengthenFiveElementsType : CombatSkillEffectBase
	{
		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06004183 RID: 16771
		protected abstract int DirectAddPower { get; }

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06004184 RID: 16772
		protected abstract int ReverseReduceCostPercent { get; }

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06004185 RID: 16773
		protected abstract sbyte FiveElementsType { get; }

		// Token: 0x06004186 RID: 16774 RVA: 0x00263001 File Offset: 0x00261201
		protected StrengthenFiveElementsType()
		{
		}

		// Token: 0x06004187 RID: 16775 RVA: 0x0026300B File Offset: 0x0026120B
		protected StrengthenFiveElementsType(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06004188 RID: 16776 RVA: 0x00263018 File Offset: 0x00261218
		public override void OnEnable(DataContext context)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.CreateAffectedData(199, EDataModifyType.Add, -1);
			}
			else
			{
				base.CreateAffectedData(204, EDataModifyType.AddPercent, -1);
			}
		}

		// Token: 0x06004189 RID: 16777 RVA: 0x00263050 File Offset: 0x00261250
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
				bool flag2 = dataKey.CombatSkillId >= 0 && base.FiveElementsEquals(dataKey, this.FiveElementsType);
				if (flag2)
				{
					bool flag3 = dataKey.FieldId == 199 && base.IsDirect;
					if (flag3)
					{
						return this.DirectAddPower;
					}
					bool flag4 = dataKey.FieldId == 204;
					if (flag4)
					{
						return this.ReverseReduceCostPercent;
					}
				}
				result = 0;
			}
			return result;
		}

		// Token: 0x0600418A RID: 16778 RVA: 0x002630DA File Offset: 0x002612DA
		protected override int GetSubClassSerializedSize()
		{
			return base.GetSubClassSerializedSize() + 1 + 1;
		}

		// Token: 0x0600418B RID: 16779 RVA: 0x002630E8 File Offset: 0x002612E8
		protected unsafe override int SerializeSubClass(byte* pData)
		{
			base.SerializeSubClass(pData);
			return this.GetSubClassSerializedSize();
		}

		// Token: 0x0600418C RID: 16780 RVA: 0x00263108 File Offset: 0x00261308
		protected unsafe override int DeserializeSubClass(byte* pData)
		{
			base.DeserializeSubClass(pData);
			return this.GetSubClassSerializedSize();
		}
	}
}
