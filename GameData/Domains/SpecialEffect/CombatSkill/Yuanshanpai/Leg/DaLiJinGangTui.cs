using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Leg
{
	// Token: 0x020001FD RID: 509
	public class DaLiJinGangTui : AddWeaponEquipAttackOnAttack
	{
		// Token: 0x06002E77 RID: 11895 RVA: 0x0020EDDB File Offset: 0x0020CFDB
		public DaLiJinGangTui()
		{
		}

		// Token: 0x06002E78 RID: 11896 RVA: 0x0020EDFA File Offset: 0x0020CFFA
		public DaLiJinGangTui(CombatSkillKey skillKey) : base(skillKey, 5104)
		{
		}

		// Token: 0x06002E79 RID: 11897 RVA: 0x0020EE1F File Offset: 0x0020D01F
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06002E7A RID: 11898 RVA: 0x0020EE34 File Offset: 0x0020D034
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06002E7B RID: 11899 RVA: 0x0020EE4C File Offset: 0x0020D04C
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = index != 3 || context.SkillKey != this.SkillKey || base.CombatChar.GetAttackSkillPower() <= 0;
			if (!flag)
			{
				int changeDistance = (int)(this.ChangeDistanceUnit * (sbyte)base.CombatChar.GetAttackSkillPower() / 10);
				bool flag2 = changeDistance > 0;
				if (flag2)
				{
					CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
					ValueTuple<byte, byte> distanceRange = DomainManager.Combat.GetDistanceRange();
					DomainManager.Combat.ChangeDistance(context, enemyChar, base.IsDirect ? changeDistance : (-changeDistance), true);
					DomainManager.Combat.SetDisplayPosition(context, !base.CombatChar.IsAlly, base.IsDirect ? int.MinValue : DomainManager.Combat.GetDisplayPosition(!base.CombatChar.IsAlly, DomainManager.Combat.GetCurrentDistance()));
					bool flag3 = !base.IsDirect;
					if (flag3)
					{
						base.CombatChar.SetCurrentPosition(base.CombatChar.GetDisplayPosition(), context);
						enemyChar.SetCurrentPosition(enemyChar.GetDisplayPosition(), context);
					}
					bool flag4 = DomainManager.Combat.GetCurrentDistance() == (short)(base.IsDirect ? distanceRange.Item2 : distanceRange.Item1);
					if (flag4)
					{
						DomainManager.Combat.AddFlaw(context, base.CurrEnemyChar, this.FlawLevel, this.SkillKey, -1, (int)this.FlawCount, true);
						base.ShowSpecialEffectTips(1);
					}
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000DD0 RID: 3536
		private sbyte ChangeDistanceUnit = 5;

		// Token: 0x04000DD1 RID: 3537
		private sbyte FlawLevel = 2;

		// Token: 0x04000DD2 RID: 3538
		private sbyte FlawCount = 3;
	}
}
