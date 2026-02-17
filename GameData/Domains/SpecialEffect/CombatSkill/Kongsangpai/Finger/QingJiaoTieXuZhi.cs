using System;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Finger
{
	// Token: 0x02000494 RID: 1172
	public class QingJiaoTieXuZhi : AttackBodyPart
	{
		// Token: 0x06003C24 RID: 15396 RVA: 0x0024C204 File Offset: 0x0024A404
		public QingJiaoTieXuZhi()
		{
		}

		// Token: 0x06003C25 RID: 15397 RVA: 0x0024C20E File Offset: 0x0024A40E
		public QingJiaoTieXuZhi(CombatSkillKey skillKey) : base(skillKey, 10204)
		{
			this.BodyParts = new sbyte[]
			{
				3,
				4
			};
			this.ReverseAddDamagePercent = 45;
		}

		// Token: 0x06003C26 RID: 15398 RVA: 0x0024C23C File Offset: 0x0024A43C
		protected override void OnCastAffectPower(DataContext context)
		{
			CombatCharacter enemyChar = base.CurrEnemyChar;
			FlawOrAcupointCollection acupoint = enemyChar.GetAcupointCollection();
			acupoint.OfflineRecoverKeepTimePercent(100);
			enemyChar.SetAcupointCollection(acupoint, context);
			base.ShowSpecialEffectTips(1);
		}

		// Token: 0x040011AF RID: 4527
		private const int RecoverAcupointKeepTimePercent = 100;
	}
}
