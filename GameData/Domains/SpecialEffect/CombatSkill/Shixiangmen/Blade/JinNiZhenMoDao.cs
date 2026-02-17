using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Blade
{
	// Token: 0x02000406 RID: 1030
	public class JinNiZhenMoDao : CombatSkillEffectBase
	{
		// Token: 0x060038D4 RID: 14548 RVA: 0x0023C0D5 File Offset: 0x0023A2D5
		public JinNiZhenMoDao()
		{
		}

		// Token: 0x060038D5 RID: 14549 RVA: 0x0023C0DF File Offset: 0x0023A2DF
		public JinNiZhenMoDao(CombatSkillKey skillKey) : base(skillKey, 6206, -1)
		{
		}

		// Token: 0x060038D6 RID: 14550 RVA: 0x0023C0F0 File Offset: 0x0023A2F0
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x060038D7 RID: 14551 RVA: 0x0023C12C File Offset: 0x0023A32C
		public override void OnDisable(DataContext context)
		{
			this.SetDirectionCanCast(context, base.CombatChar, !base.IsDirect, true);
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x060038D8 RID: 14552 RVA: 0x0023C188 File Offset: 0x0023A388
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId;
			if (!flag)
			{
				sbyte affectDirection = base.IsDirect ? 1 : 0;
				bool flag2 = !this.IsSrcSkillPerformed;
				if (flag2)
				{
					bool flag3 = skillId == base.SkillTemplateId;
					if (flag3)
					{
						CombatCharacter enemyChar0 = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
						CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, true);
						bool flag4 = enemyChar0.GetPreparingSkillId() >= 0 && DomainManager.CombatSkill.GetSkillDirection(enemyChar0.GetId(), enemyChar0.GetPreparingSkillId()) == affectDirection && DomainManager.Combat.InterruptSkill(context, enemyChar0, 100);
						if (flag4)
						{
							enemyChar0.SetAnimationToPlayOnce(DomainManager.Combat.GetHittedAni(enemyChar0, 2), context);
							DomainManager.Combat.SetProperLoopAniAndParticle(context, enemyChar0, false);
						}
						this.ClearAffectingSkill(context, enemyChar0, affectDirection);
						bool flag5 = enemyChar != enemyChar0;
						if (flag5)
						{
							this.ClearAffectingSkill(context, enemyChar, affectDirection);
						}
						this.SetDirectionCanCast(context, !isAlly, !base.IsDirect, false);
						base.ShowSpecialEffectTips(0);
					}
				}
				else
				{
					bool flag6 = DomainManager.CombatSkill.GetSkillDirection(charId, skillId) == (base.IsDirect ? 0 : 1);
					if (flag6)
					{
						base.ReduceEffectCount(1);
					}
				}
			}
		}

		// Token: 0x060038D9 RID: 14553 RVA: 0x0023C2E8 File Offset: 0x0023A4E8
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = !this.IsSrcSkillPerformed;
				if (flag2)
				{
					this.IsSrcSkillPerformed = true;
					this.SetDirectionCanCast(context, !isAlly, !base.IsDirect, true);
					this.SetDirectionCanCast(context, base.CombatChar, !base.IsDirect, false);
					base.AddMaxEffectCount(true);
				}
				else
				{
					base.RemoveSelf(context);
				}
			}
		}

		// Token: 0x060038DA RID: 14554 RVA: 0x0023C370 File Offset: 0x0023A570
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x060038DB RID: 14555 RVA: 0x0023C3C0 File Offset: 0x0023A5C0
		private void ClearAffectingSkill(DataContext context, CombatCharacter combatChar, sbyte direction)
		{
			short defendSkillId = combatChar.GetAffectingDefendSkillId();
			short moveSkillId = combatChar.GetAffectingMoveSkillId();
			bool flag = defendSkillId >= 0 && DomainManager.CombatSkill.GetSkillDirection(combatChar.GetId(), defendSkillId) == direction;
			if (flag)
			{
				DomainManager.Combat.ClearAffectingDefenseSkill(context, combatChar);
			}
			bool flag2 = moveSkillId >= 0 && DomainManager.CombatSkill.GetSkillDirection(combatChar.GetId(), moveSkillId) == direction;
			if (flag2)
			{
				base.ClearAffectingAgileSkill(context, combatChar);
			}
		}

		// Token: 0x060038DC RID: 14556 RVA: 0x0023C430 File Offset: 0x0023A630
		private void SetDirectionCanCast(DataContext context, bool isAlly, bool isDirect, bool canCast)
		{
			int[] charList = DomainManager.Combat.GetCharacterList(isAlly);
			for (int i = 0; i < charList.Length; i++)
			{
				bool flag = charList[i] >= 0;
				if (flag)
				{
					this.SetDirectionCanCast(context, DomainManager.Combat.GetElement_CombatCharacterDict(charList[i]), isDirect, canCast);
				}
			}
		}

		// Token: 0x060038DD RID: 14557 RVA: 0x0023C484 File Offset: 0x0023A684
		private void SetDirectionCanCast(DataContext context, CombatCharacter combatChar, bool isDirect, bool canCast)
		{
			if (isDirect)
			{
				combatChar.CanCastDirectSkill = canCast;
			}
			else
			{
				combatChar.CanCastReverseSkill = canCast;
			}
			DomainManager.Combat.UpdateSkillCanUse(context, combatChar);
			DomainManager.Combat.UpdateTeammateCommandUsable(context, combatChar, ETeammateCommandImplement.Defend);
		}

		// Token: 0x060038DE RID: 14558 RVA: 0x0023C4C4 File Offset: 0x0023A6C4
		public static int CalcInterruptOdds(CombatSkillKey selfSkill, bool isDirect, CombatSkillKey enemySkill)
		{
			sbyte direction = DomainManager.CombatSkill.GetSkillDirection(enemySkill.CharId, enemySkill.SkillTemplateId);
			return (direction == (isDirect ? 1 : 0)) ? 100 : 0;
		}
	}
}
