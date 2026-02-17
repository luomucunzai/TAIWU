using System;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Blade
{
	// Token: 0x0200040C RID: 1036
	public class ZhanAoDaoFa : AttackBodyPart
	{
		// Token: 0x060038FA RID: 14586 RVA: 0x0023CAE7 File Offset: 0x0023ACE7
		public ZhanAoDaoFa()
		{
		}

		// Token: 0x060038FB RID: 14587 RVA: 0x0023CAF1 File Offset: 0x0023ACF1
		public ZhanAoDaoFa(CombatSkillKey skillKey) : base(skillKey, 6204)
		{
			this.BodyParts = new sbyte[]
			{
				5,
				6
			};
			this.ReverseAddDamagePercent = 45;
		}

		// Token: 0x060038FC RID: 14588 RVA: 0x0023CB20 File Offset: 0x0023AD20
		protected override void OnCastAffectPower(DataContext context)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			base.ChangeMobilityValue(context, enemyChar, -MoveSpecialConstants.MaxMobility);
			base.ClearAffectingAgileSkill(context, enemyChar);
			base.ShowSpecialEffectTips(1);
		}
	}
}
