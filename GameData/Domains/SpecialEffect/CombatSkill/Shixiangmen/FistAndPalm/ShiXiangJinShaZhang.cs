using System;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.FistAndPalm
{
	// Token: 0x020003FA RID: 1018
	public class ShiXiangJinShaZhang : AttackBodyPart
	{
		// Token: 0x06003893 RID: 14483 RVA: 0x0023B011 File Offset: 0x00239211
		public ShiXiangJinShaZhang()
		{
		}

		// Token: 0x06003894 RID: 14484 RVA: 0x0023B01B File Offset: 0x0023921B
		public ShiXiangJinShaZhang(CombatSkillKey skillKey) : base(skillKey, 6104)
		{
			this.BodyParts = new sbyte[]
			{
				3,
				4
			};
			this.ReverseAddDamagePercent = 45;
		}

		// Token: 0x06003895 RID: 14485 RVA: 0x0023B048 File Offset: 0x00239248
		protected override void OnCastAffectPower(DataContext context)
		{
			CombatCharacter enemyChar = base.CurrEnemyChar;
			FlawOrAcupointCollection flaw = enemyChar.GetFlawCollection();
			flaw.OfflineRecoverKeepTimePercent(100);
			enemyChar.SetFlawCollection(flaw, context);
			base.ShowSpecialEffectTips(1);
		}

		// Token: 0x04001091 RID: 4241
		private const int RecoverFlawKeepTimePercent = 100;
	}
}
