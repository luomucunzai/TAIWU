using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Agile
{
	// Token: 0x020001EC RID: 492
	public class TianKaiLiuYunShi : BuffHitOrDebuffAvoid
	{
		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06002E29 RID: 11817 RVA: 0x0020DEFE File Offset: 0x0020C0FE
		protected override sbyte AffectHitType
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06002E2A RID: 11818 RVA: 0x0020DF01 File Offset: 0x0020C101
		public TianKaiLiuYunShi()
		{
		}

		// Token: 0x06002E2B RID: 11819 RVA: 0x0020DF0B File Offset: 0x0020C10B
		public TianKaiLiuYunShi(CombatSkillKey skillKey) : base(skillKey, 9506)
		{
		}

		// Token: 0x06002E2C RID: 11820 RVA: 0x0020DF1B File Offset: 0x0020C11B
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_AttackSkillAttackHit(new Events.OnAttackSkillAttackHit(this.OnAttackSkillAttackHit));
		}

		// Token: 0x06002E2D RID: 11821 RVA: 0x0020DF38 File Offset: 0x0020C138
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AttackSkillAttackHit(new Events.OnAttackSkillAttackHit(this.OnAttackSkillAttackHit));
			base.OnDisable(context);
		}

		// Token: 0x06002E2E RID: 11822 RVA: 0x0020DF58 File Offset: 0x0020C158
		private void OnAttackSkillAttackHit(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId, int index, bool critical)
		{
			bool flag = attacker.GetId() != base.CharacterId || index == 3;
			if (!flag)
			{
				sbyte hitType = base.CombatChar.SkillHitType[index];
				bool flag2 = hitType != this.AffectHitType;
				if (!flag2)
				{
					int hit = DomainManager.CombatSkill.GetElement_CombatSkills(new ValueTuple<int, short>(attacker.GetId(), skillId)).GetHitDistribution()[(int)hitType];
					bool flag3 = hit <= 0;
					if (!flag3)
					{
						CombatCharacter affectChar = base.IsDirect ? attacker : defender;
						DomainManager.Combat.ChangeEquipmentPowerInCombat(affectChar.GetId(), hit / 10 * 2 * (base.IsDirect ? 1 : -1));
						base.ShowSpecialEffectTips(1);
					}
				}
			}
		}

		// Token: 0x04000DC1 RID: 3521
		private const int ChangeEquipmentPowerUnit = 2;
	}
}
