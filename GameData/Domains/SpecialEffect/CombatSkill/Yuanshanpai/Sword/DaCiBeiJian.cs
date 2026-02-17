using System;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Sword
{
	// Token: 0x020001EF RID: 495
	public class DaCiBeiJian : AttackBodyPart
	{
		// Token: 0x06002E3C RID: 11836 RVA: 0x0020E358 File Offset: 0x0020C558
		public DaCiBeiJian()
		{
		}

		// Token: 0x06002E3D RID: 11837 RVA: 0x0020E362 File Offset: 0x0020C562
		public DaCiBeiJian(CombatSkillKey skillKey) : base(skillKey, 5203)
		{
			this.BodyParts = new sbyte[]
			{
				3,
				4
			};
			this.ReverseAddDamagePercent = 45;
		}

		// Token: 0x06002E3E RID: 11838 RVA: 0x0020E390 File Offset: 0x0020C590
		protected override void OnCastAffectPower(DataContext context)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			base.ChangeMobilityValue(context, enemyChar, -MoveSpecialConstants.MaxMobility);
			base.ClearAffectingAgileSkill(context, enemyChar);
			base.ShowSpecialEffectTips(1);
		}
	}
}
