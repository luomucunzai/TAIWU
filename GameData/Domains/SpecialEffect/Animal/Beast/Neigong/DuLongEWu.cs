using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Neigong
{
	// Token: 0x0200061A RID: 1562
	public class DuLongEWu : AnimalEffectBase
	{
		// Token: 0x0600459A RID: 17818 RVA: 0x00272E7A File Offset: 0x0027107A
		public DuLongEWu()
		{
		}

		// Token: 0x0600459B RID: 17819 RVA: 0x00272E84 File Offset: 0x00271084
		public DuLongEWu(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x0600459C RID: 17820 RVA: 0x00272E90 File Offset: 0x00271090
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 77, -1, -1, -1, -1), EDataModifyType.Custom);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 78, -1, -1, -1, -1), EDataModifyType.Custom);
		}

		// Token: 0x0600459D RID: 17821 RVA: 0x00272EE8 File Offset: 0x002710E8
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			bool result;
			if (flag)
			{
				result = base.GetModifiedValue(dataKey, dataValue);
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId - 77 <= 1;
				bool flag3 = flag2;
				if (flag3)
				{
					base.ShowSpecialEffectTipsOnceInFrame((dataKey.FieldId == 77) ? 0 : 1);
					result = true;
				}
				else
				{
					result = base.GetModifiedValue(dataKey, dataValue);
				}
			}
			return result;
		}
	}
}
