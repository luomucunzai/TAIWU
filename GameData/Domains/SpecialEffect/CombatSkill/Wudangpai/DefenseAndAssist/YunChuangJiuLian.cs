using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.DefenseAndAssist
{
	// Token: 0x020003DD RID: 989
	public class YunChuangJiuLian : DefenseSkillBase
	{
		// Token: 0x060037E7 RID: 14311 RVA: 0x00237DC8 File Offset: 0x00235FC8
		public YunChuangJiuLian()
		{
		}

		// Token: 0x060037E8 RID: 14312 RVA: 0x00237DD2 File Offset: 0x00235FD2
		public YunChuangJiuLian(CombatSkillKey skillKey) : base(skillKey, 4502)
		{
		}

		// Token: 0x060037E9 RID: 14313 RVA: 0x00237DE2 File Offset: 0x00235FE2
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x060037EA RID: 14314 RVA: 0x00237DFF File Offset: 0x00235FFF
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x060037EB RID: 14315 RVA: 0x00237E1C File Offset: 0x0023601C
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !isFightBack || attacker != base.CombatChar || !hit || !base.CanAffect;
			if (!flag)
			{
				DomainManager.Combat.AddCombatState(context, base.CombatChar, 1, base.IsDirect ? 24 : 25, 50);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x04001053 RID: 4179
		private const sbyte AddStatePowerUnit = 50;
	}
}
