using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.FistAndPalm
{
	// Token: 0x02000521 RID: 1313
	public class XiaoYuYangShenQuan : CombatSkillEffectBase
	{
		// Token: 0x06003F23 RID: 16163 RVA: 0x00258A9B File Offset: 0x00256C9B
		public XiaoYuYangShenQuan()
		{
		}

		// Token: 0x06003F24 RID: 16164 RVA: 0x00258AA5 File Offset: 0x00256CA5
		public XiaoYuYangShenQuan(CombatSkillKey skillKey) : base(skillKey, 14105, -1)
		{
		}

		// Token: 0x06003F25 RID: 16165 RVA: 0x00258AB6 File Offset: 0x00256CB6
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003F26 RID: 16166 RVA: 0x00258ADD File Offset: 0x00256CDD
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003F27 RID: 16167 RVA: 0x00258B04 File Offset: 0x00256D04
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = context.SkillKey != this.SkillKey || index < 3;
			if (!flag)
			{
				NeiliAllocation neiliAllocation = base.CombatChar.GetNeiliAllocation();
				bool flag2 = !base.CombatCharPowerMatchAffectRequire(0) || neiliAllocation.Items.FixedElementField < 10;
				if (!flag2)
				{
					CombatCharacter target = base.IsDirect ? base.CombatChar : base.EnemyChar;
					CValuePercent percent = base.IsDirect ? XiaoYuYangShenQuan.ChangeEffectCountPercent : (-XiaoYuYangShenQuan.ChangeEffectCountPercent);
					bool flag3 = !DomainManager.Combat.ChangeSkillEffectRandom(context, target, percent, 2, 1);
					if (!flag3)
					{
						base.CombatChar.ChangeNeiliAllocation(context, 0, -10, true, true);
						base.ShowSpecialEffectTips(0);
					}
				}
			}
		}

		// Token: 0x06003F28 RID: 16168 RVA: 0x00258BD8 File Offset: 0x00256DD8
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04001299 RID: 4761
		private const sbyte CostNeiliAllocation = 10;

		// Token: 0x0400129A RID: 4762
		private const int MaxChangeCount = 2;

		// Token: 0x0400129B RID: 4763
		private const sbyte RequireEquipType = 1;

		// Token: 0x0400129C RID: 4764
		private static readonly CValuePercent ChangeEffectCountPercent = 50;
	}
}
