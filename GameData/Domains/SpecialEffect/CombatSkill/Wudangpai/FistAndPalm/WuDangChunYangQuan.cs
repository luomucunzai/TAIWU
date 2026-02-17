using System;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.FistAndPalm
{
	// Token: 0x020003D1 RID: 977
	public class WuDangChunYangQuan : CombatSkillEffectBase
	{
		// Token: 0x0600379B RID: 14235 RVA: 0x002365AA File Offset: 0x002347AA
		public WuDangChunYangQuan()
		{
		}

		// Token: 0x0600379C RID: 14236 RVA: 0x002365B4 File Offset: 0x002347B4
		public WuDangChunYangQuan(CombatSkillKey skillKey) : base(skillKey, 4106, -1)
		{
		}

		// Token: 0x0600379D RID: 14237 RVA: 0x002365C5 File Offset: 0x002347C5
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x0600379E RID: 14238 RVA: 0x002365EC File Offset: 0x002347EC
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x0600379F RID: 14239 RVA: 0x00236614 File Offset: 0x00234814
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = this.GetCanInterruptSKill() >= 0;
				if (flag2)
				{
					DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * 50 / 100);
				}
			}
		}

		// Token: 0x060037A0 RID: 14240 RVA: 0x00236674 File Offset: 0x00234874
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = context.SkillKey != this.SkillKey || index != 3;
			if (!flag)
			{
				short interruptSkillId = this.GetCanInterruptSKill();
				bool flag2 = base.CombatCharPowerMatchAffectRequire(0) && interruptSkillId >= 0;
				if (flag2)
				{
					bool flag3 = DomainManager.Combat.InterruptSkill(context, base.CurrEnemyChar, 100);
					if (flag3)
					{
						base.CurrEnemyChar.SetAnimationToPlayOnce(DomainManager.Combat.GetHittedAni(base.CurrEnemyChar, 2), context);
						DomainManager.Combat.SetProperLoopAniAndParticle(context, base.CurrEnemyChar, false);
					}
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x060037A1 RID: 14241 RVA: 0x0023672C File Offset: 0x0023492C
		private short GetCanInterruptSKill()
		{
			short skillId = base.CurrEnemyChar.GetPreparingSkillId();
			bool flag = skillId < 0 || Config.CombatSkill.Instance[skillId].EquipType != 1;
			short result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				sbyte innerRatio = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(base.CurrEnemyChar.GetId(), skillId)).GetCurrInnerRatio();
				result = ((base.IsDirect ? (innerRatio > 50) : (innerRatio < 50)) ? skillId : -1);
			}
			return result;
		}

		// Token: 0x060037A2 RID: 14242 RVA: 0x002367AC File Offset: 0x002349AC
		public static int CalcInterruptOdds(CombatSkillKey selfSkill, bool isDirect, CombatSkillKey enemySkill)
		{
			sbyte innerRatio = DomainManager.CombatSkill.GetElement_CombatSkills(enemySkill).GetCurrInnerRatio();
			return (int)((isDirect ? (innerRatio > 50) : (innerRatio < 50)) ? 100 : -1);
		}

		// Token: 0x04001038 RID: 4152
		private const sbyte PrepareProgressPercent = 50;
	}
}
