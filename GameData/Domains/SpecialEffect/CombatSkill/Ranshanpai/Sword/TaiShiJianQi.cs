using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Sword
{
	// Token: 0x02000446 RID: 1094
	public class TaiShiJianQi : BuffByNeiliAllocation
	{
		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06003A26 RID: 14886 RVA: 0x002422A3 File Offset: 0x002404A3
		protected override bool ShowTipsOnAffecting
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003A27 RID: 14887 RVA: 0x002422A6 File Offset: 0x002404A6
		public TaiShiJianQi()
		{
		}

		// Token: 0x06003A28 RID: 14888 RVA: 0x002422B0 File Offset: 0x002404B0
		public TaiShiJianQi(CombatSkillKey skillKey) : base(skillKey, 7207)
		{
			this.RequireNeiliAllocationType = 3;
		}

		// Token: 0x06003A29 RID: 14889 RVA: 0x002422C7 File Offset: 0x002404C7
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_AttackSkillAttackHit(new Events.OnAttackSkillAttackHit(this.OnAttackSkillAttackHit));
		}

		// Token: 0x06003A2A RID: 14890 RVA: 0x002422E4 File Offset: 0x002404E4
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AttackSkillAttackHit(new Events.OnAttackSkillAttackHit(this.OnAttackSkillAttackHit));
			base.OnDisable(context);
		}

		// Token: 0x06003A2B RID: 14891 RVA: 0x00242304 File Offset: 0x00240504
		private void OnAttackSkillAttackHit(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId, int index, bool critical)
		{
			bool flag = !this.SkillKey.IsMatch(attacker.GetId(), skillId) || !base.Affecting;
			if (!flag)
			{
				int hit = (index < 3) ? base.SkillInstance.GetHitDistribution()[(int)base.CombatChar.SkillHitType[index]] : ((int)base.CombatChar.GetAttackSkillPower());
				bool flag2 = hit <= 0;
				if (!flag2)
				{
					short banableSkillId = defender.GetRandomBanableSkillId(context.Random, null, 4);
					bool flag3 = banableSkillId < 0;
					if (!flag3)
					{
						DomainManager.Combat.SilenceSkill(context, defender, banableSkillId, hit * 90 / 10, 100);
						base.ShowSpecialEffectTips(0);
					}
				}
			}
		}

		// Token: 0x04001104 RID: 4356
		private const int PerHitSilenceFrame = 90;
	}
}
