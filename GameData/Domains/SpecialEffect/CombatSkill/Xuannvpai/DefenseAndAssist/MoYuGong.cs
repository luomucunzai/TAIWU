using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.DefenseAndAssist
{
	// Token: 0x02000283 RID: 643
	public class MoYuGong : AssistSkillBase
	{
		// Token: 0x060030F9 RID: 12537 RVA: 0x00219474 File Offset: 0x00217674
		public MoYuGong()
		{
		}

		// Token: 0x060030FA RID: 12538 RVA: 0x0021947E File Offset: 0x0021767E
		public MoYuGong(CombatSkillKey skillKey) : base(skillKey, 8602)
		{
			this.SetConstAffectingOnCombatBegin = true;
		}

		// Token: 0x060030FB RID: 12539 RVA: 0x00219498 File Offset: 0x00217698
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.CreateAffectedData(178, EDataModifyType.AddPercent, -1);
			}
			else
			{
				base.CreateAffectedAllEnemyData(178, EDataModifyType.AddPercent, -1);
			}
		}

		// Token: 0x060030FC RID: 12540 RVA: 0x002194D6 File Offset: 0x002176D6
		protected override void OnCanUseChanged(DataContext context, DataUid dataUid)
		{
			base.SetConstAffecting(context, base.CanAffect);
		}

		// Token: 0x060030FD RID: 12541 RVA: 0x002194E8 File Offset: 0x002176E8
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = !base.IsDirect && !base.IsCurrent;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 178;
					if (flag3)
					{
						result = (base.IsDirect ? -33 : 33);
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04000E86 RID: 3718
		private const sbyte ChangeTimePercent = 33;
	}
}
