using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Neigong.Boss
{
	// Token: 0x020002A0 RID: 672
	public class QingGuoJueShi : BossNeigongBase
	{
		// Token: 0x060031A8 RID: 12712 RVA: 0x0021BB0E File Offset: 0x00219D0E
		public QingGuoJueShi()
		{
		}

		// Token: 0x060031A9 RID: 12713 RVA: 0x0021BB18 File Offset: 0x00219D18
		public QingGuoJueShi(CombatSkillKey skillKey) : base(skillKey, 16104)
		{
		}

		// Token: 0x060031AA RID: 12714 RVA: 0x0021BB28 File Offset: 0x00219D28
		protected override void ActivePhase2Effect(DataContext context)
		{
			base.AppendAffectedData(context, base.CharacterId, 85, EDataModifyType.Custom, -1);
			base.AppendAffectedData(context, base.CharacterId, 89, EDataModifyType.Custom, -1);
		}

		// Token: 0x060031AB RID: 12715 RVA: 0x0021BB50 File Offset: 0x00219D50
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId < 0 || base.CombatChar.UsableTrickCount <= base.EnemyChar.UsableTrickCount;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 85;
				result = (!flag2 && dataValue);
			}
			return result;
		}

		// Token: 0x060031AC RID: 12716 RVA: 0x0021BBB4 File Offset: 0x00219DB4
		public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
		{
			EDamageType damageType = (EDamageType)dataKey.CustomParam0;
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId < 0 || damageType != EDamageType.Direct || base.CombatChar.UsableTrickCount <= base.EnemyChar.UsableTrickCount;
			long result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 89;
				if (flag2)
				{
					result = dataValue * 3L;
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x04000EB8 RID: 3768
		private const sbyte DamageMultiplier = 3;
	}
}
