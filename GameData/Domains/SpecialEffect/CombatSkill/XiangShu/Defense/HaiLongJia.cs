using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Defense
{
	// Token: 0x020002AC RID: 684
	public class HaiLongJia : DefenseSkillBase
	{
		// Token: 0x060031E8 RID: 12776 RVA: 0x0021CB9D File Offset: 0x0021AD9D
		public HaiLongJia()
		{
		}

		// Token: 0x060031E9 RID: 12777 RVA: 0x0021CBA7 File Offset: 0x0021ADA7
		public HaiLongJia(CombatSkillKey skillKey) : base(skillKey, 16305)
		{
		}

		// Token: 0x060031EA RID: 12778 RVA: 0x0021CBB7 File Offset: 0x0021ADB7
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 114, -1, -1, -1, -1), EDataModifyType.Custom);
		}

		// Token: 0x060031EB RID: 12779 RVA: 0x0021CBEC File Offset: 0x0021ADEC
		public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			long result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				int defendTimePercent = (int)(base.CombatChar.DefendSkillLeftFrame * 100 / base.CombatChar.DefendSkillTotalFrame);
				bool flag2 = defendTimePercent > 0;
				if (flag2)
				{
					int damageUnit = base.CombatChar.GetDamageStepCollection().FatalDamageStep / 10;
					long costTimePercent = Math.Min(dataValue / (long)damageUnit, (long)defendTimePercent);
					base.CombatChar.DefendSkillLeftFrame = (short)Math.Max(0L, (long)base.CombatChar.DefendSkillLeftFrame - (long)base.CombatChar.DefendSkillTotalFrame * costTimePercent / 100L);
					base.ShowSpecialEffectTipsOnceInFrame(0);
					result = (long)damageUnit * (dataValue / (long)damageUnit - costTimePercent);
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}
	}
}
