using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Agile
{
	// Token: 0x02000340 RID: 832
	public class YaoYin : AgileSkillBase
	{
		// Token: 0x060034D0 RID: 13520 RVA: 0x0022A0B3 File Offset: 0x002282B3
		public YaoYin()
		{
		}

		// Token: 0x060034D1 RID: 13521 RVA: 0x0022A0BD File Offset: 0x002282BD
		public YaoYin(CombatSkillKey skillKey) : base(skillKey, 16212)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x060034D2 RID: 13522 RVA: 0x0022A0D4 File Offset: 0x002282D4
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 55, -1, -1, -1, -1), EDataModifyType.Custom);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 157, -1, -1, -1, -1), EDataModifyType.Custom);
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x060034D3 RID: 13523 RVA: 0x0022A13C File Offset: 0x0022833C
		protected override void OnMoveSkillCanAffectChanged(DataContext context, DataUid dataUid)
		{
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 55);
		}

		// Token: 0x060034D4 RID: 13524 RVA: 0x0022A154 File Offset: 0x00228354
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 55 && dataKey.CustomParam0 == 1;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 157;
					result = (!flag3 && dataValue);
				}
			}
			return result;
		}
	}
}
