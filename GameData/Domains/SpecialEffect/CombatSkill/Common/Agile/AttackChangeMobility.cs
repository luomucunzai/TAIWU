using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Agile
{
	// Token: 0x020005B1 RID: 1457
	public class AttackChangeMobility : AgileSkillBase
	{
		// Token: 0x06004353 RID: 17235 RVA: 0x0026AF65 File Offset: 0x00269165
		protected AttackChangeMobility()
		{
		}

		// Token: 0x06004354 RID: 17236 RVA: 0x0026AF6F File Offset: 0x0026916F
		protected AttackChangeMobility(CombatSkillKey skillKey, int type) : base(skillKey, type)
		{
		}

		// Token: 0x06004355 RID: 17237 RVA: 0x0026AF7B File Offset: 0x0026917B
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06004356 RID: 17238 RVA: 0x0026AF98 File Offset: 0x00269198
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06004357 RID: 17239 RVA: 0x0026AFB8 File Offset: 0x002691B8
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !base.CanAffect || !hit || attacker != base.CombatChar || attacker.GetWeaponData(-1).Template.ItemSubType != this.RequireWeaponSubType;
			if (!flag)
			{
				CombatCharacter affectChar = base.IsDirect ? base.CombatChar : base.CurrEnemyChar;
				int changeMobility = MoveSpecialConstants.MaxMobility * AttackChangeMobility.ChangeMobilityPerMille[(int)base.CombatChar.GetConfigAttackPointCost()] / 1000;
				base.ChangeMobilityValue(context, affectChar, base.IsDirect ? changeMobility : (-changeMobility));
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x040013FC RID: 5116
		private static readonly int[] ChangeMobilityPerMille = new int[]
		{
			30,
			45,
			60
		};

		// Token: 0x040013FD RID: 5117
		protected short RequireWeaponSubType;
	}
}
