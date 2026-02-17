using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.DefenseAndAssist
{
	// Token: 0x020004C8 RID: 1224
	public class NaMaiGong : DefenseSkillBase
	{
		// Token: 0x06003D3C RID: 15676 RVA: 0x00250DE0 File Offset: 0x0024EFE0
		public NaMaiGong()
		{
		}

		// Token: 0x06003D3D RID: 15677 RVA: 0x00250DEA File Offset: 0x0024EFEA
		public NaMaiGong(CombatSkillKey skillKey) : base(skillKey, 11601)
		{
		}

		// Token: 0x06003D3E RID: 15678 RVA: 0x00250DFA File Offset: 0x0024EFFA
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06003D3F RID: 15679 RVA: 0x00250E17 File Offset: 0x0024F017
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06003D40 RID: 15680 RVA: 0x00250E34 File Offset: 0x0024F034
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !isFightBack || attacker != base.CombatChar || !hit || !base.CanAffect;
			if (!flag)
			{
				DomainManager.Combat.AddCombatState(context, DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false), 2, base.IsDirect ? 57 : 58, 50);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x04001204 RID: 4612
		private const sbyte AddStatePowerUnit = 50;
	}
}
