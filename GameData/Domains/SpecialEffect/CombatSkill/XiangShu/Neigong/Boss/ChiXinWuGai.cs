using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Neigong.Boss
{
	// Token: 0x0200029C RID: 668
	public class ChiXinWuGai : BossNeigongBase
	{
		// Token: 0x06003191 RID: 12689 RVA: 0x0021B689 File Offset: 0x00219889
		public ChiXinWuGai()
		{
		}

		// Token: 0x06003192 RID: 12690 RVA: 0x0021B693 File Offset: 0x00219893
		public ChiXinWuGai(CombatSkillKey skillKey) : base(skillKey, 16113)
		{
		}

		// Token: 0x06003193 RID: 12691 RVA: 0x0021B6A3 File Offset: 0x002198A3
		protected override void ActivePhase2Effect(DataContext context)
		{
			base.AppendAffectedData(context, base.CharacterId, 116, EDataModifyType.Add, -1);
			base.AppendAffectedAllEnemyData(context, 116, EDataModifyType.Add, -1);
		}

		// Token: 0x06003194 RID: 12692 RVA: 0x0021B6C4 File Offset: 0x002198C4
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId && !DomainManager.Combat.IsCurrentCombatCharacter(base.CombatChar);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				CombatCharacter enemyChar = base.CurrEnemyChar;
				sbyte bodyPart = (sbyte)dataKey.CustomParam0;
				bool inner = dataKey.CustomParam1 == 1;
				bool flag2 = dataKey.CharId == base.CharacterId;
				if (flag2)
				{
					result = (int)(-(int)enemyChar.GetInjuries().Get(bodyPart, inner));
				}
				else
				{
					result = (int)base.CombatChar.GetInjuries().Get(bodyPart, inner);
				}
			}
			return result;
		}
	}
}
