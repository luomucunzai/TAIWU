using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.DefenseAndAssist
{
	// Token: 0x02000286 RID: 646
	public class XiaShuXianYiFa : AssistSkillBase
	{
		// Token: 0x06003106 RID: 12550 RVA: 0x00219725 File Offset: 0x00217925
		public XiaShuXianYiFa()
		{
		}

		// Token: 0x06003107 RID: 12551 RVA: 0x0021972F File Offset: 0x0021792F
		public XiaShuXianYiFa(CombatSkillKey skillKey) : base(skillKey, 8604)
		{
			this.SetConstAffectingOnCombatBegin = true;
		}

		// Token: 0x06003108 RID: 12552 RVA: 0x00219748 File Offset: 0x00217948
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.CreateAffectedData(179, EDataModifyType.AddPercent, -1);
			}
			else
			{
				base.CreateAffectedAllEnemyData(179, EDataModifyType.AddPercent, -1);
			}
		}

		// Token: 0x06003109 RID: 12553 RVA: 0x00219786 File Offset: 0x00217986
		protected override void OnCanUseChanged(DataContext context, DataUid dataUid)
		{
			base.SetConstAffecting(context, base.CanAffect);
		}

		// Token: 0x0600310A RID: 12554 RVA: 0x00219798 File Offset: 0x00217998
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
					bool flag3 = dataKey.FieldId == 179;
					if (flag3)
					{
						result = (base.IsDirect ? -25 : 25);
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04000E88 RID: 3720
		private const sbyte ChangeSpeedPercent = 25;
	}
}
