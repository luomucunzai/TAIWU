using System;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Blade
{
	// Token: 0x02000211 RID: 529
	public class TaiYiJinDao : AttackBodyPart
	{
		// Token: 0x06002EEC RID: 12012 RVA: 0x002110E4 File Offset: 0x0020F2E4
		public TaiYiJinDao()
		{
		}

		// Token: 0x06002EED RID: 12013 RVA: 0x002110EE File Offset: 0x0020F2EE
		public TaiYiJinDao(CombatSkillKey skillKey) : base(skillKey, 5303)
		{
			this.BodyParts = new sbyte[]
			{
				5,
				6
			};
			this.ReverseAddDamagePercent = 45;
		}

		// Token: 0x06002EEE RID: 12014 RVA: 0x0021111C File Offset: 0x0020F31C
		protected override void OnCastAffectPower(DataContext context)
		{
			CombatCharacter enemyChar = base.CurrEnemyChar;
			FlawOrAcupointCollection acupoint = enemyChar.GetAcupointCollection();
			acupoint.OfflineRecoverKeepTimePercent(100);
			enemyChar.SetAcupointCollection(acupoint, context);
			base.ShowSpecialEffectTips(1);
		}

		// Token: 0x04000DF0 RID: 3568
		private const int RecoverAcupointKeepTimePercent = 100;
	}
}
