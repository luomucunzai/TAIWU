using System;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Agile
{
	// Token: 0x020003AE RID: 942
	public class DunDiBaiZuXian : CheckHitEffect
	{
		// Token: 0x060036E0 RID: 14048 RVA: 0x00232AE9 File Offset: 0x00230CE9
		public DunDiBaiZuXian()
		{
		}

		// Token: 0x060036E1 RID: 14049 RVA: 0x00232AF3 File Offset: 0x00230CF3
		public DunDiBaiZuXian(CombatSkillKey skillKey) : base(skillKey, 12605)
		{
			this.CheckHitType = 2;
		}

		// Token: 0x060036E2 RID: 14050 RVA: 0x00232B0C File Offset: 0x00230D0C
		protected override bool HitEffect(DataContext context)
		{
			CombatCharacter enemyChar = base.CurrEnemyChar;
			bool flag = (base.IsDirect ? enemyChar.GetAffectingDefendSkillId() : enemyChar.GetAffectingMoveSkillId()) < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				int leftPercent = base.IsDirect ? ((int)enemyChar.GetDefendSkillTimePercent()) : (enemyChar.GetMobilityValue() * 100 / MoveSpecialConstants.MaxMobility);
				bool flag2 = leftPercent > 30;
				if (flag2)
				{
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						enemyChar.DefendSkillLeftFrame -= enemyChar.DefendSkillTotalFrame * 30 / 100;
						enemyChar.SetDefendSkillTimePercent((byte)(enemyChar.DefendSkillLeftFrame * 100 / enemyChar.DefendSkillTotalFrame), context);
					}
					else
					{
						base.ChangeMobilityValue(context, enemyChar, -MoveSpecialConstants.MaxMobility * 30 / 100);
					}
				}
				else
				{
					bool isDirect2 = base.IsDirect;
					if (isDirect2)
					{
						DomainManager.Combat.ClearAffectingDefenseSkill(context, enemyChar);
					}
					else
					{
						base.ClearAffectingAgileSkill(context, enemyChar);
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x04001000 RID: 4096
		private const sbyte ReducePercent = 30;
	}
}
