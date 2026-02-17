using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.JinHuangEr
{
	// Token: 0x02000309 RID: 777
	public class AddFlaw : CombatSkillEffectBase
	{
		// Token: 0x060033DC RID: 13276 RVA: 0x00226D1D File Offset: 0x00224F1D
		protected AddFlaw()
		{
		}

		// Token: 0x060033DD RID: 13277 RVA: 0x00226D27 File Offset: 0x00224F27
		protected AddFlaw(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x060033DE RID: 13278 RVA: 0x00226D34 File Offset: 0x00224F34
		public override void OnEnable(DataContext context)
		{
			this._registeredStateMachineUpdateEnd = false;
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x060033DF RID: 13279 RVA: 0x00226D64 File Offset: 0x00224F64
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
			bool registeredStateMachineUpdateEnd = this._registeredStateMachineUpdateEnd;
			if (registeredStateMachineUpdateEnd)
			{
				Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
			}
		}

		// Token: 0x060033E0 RID: 13280 RVA: 0x00226DB4 File Offset: 0x00224FB4
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = !this.IsSrcSkillPerformed;
				if (flag2)
				{
					bool flag3 = !interrupted;
					if (flag3)
					{
						this.IsSrcSkillPerformed = true;
						this._frameCounter = 0;
						this._registeredStateMachineUpdateEnd = true;
						base.AddMaxEffectCount(true);
						Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
					}
					else
					{
						base.RemoveSelf(context);
					}
				}
				else
				{
					bool flag4 = !interrupted;
					if (flag4)
					{
						base.RemoveSelf(context);
					}
				}
			}
		}

		// Token: 0x060033E1 RID: 13281 RVA: 0x00226E4C File Offset: 0x0022504C
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x060033E2 RID: 13282 RVA: 0x00226E9C File Offset: 0x0022509C
		private void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			bool flag = base.CombatChar != combatChar || DomainManager.Combat.Pause;
			if (!flag)
			{
				this._frameCounter++;
				bool flag2 = this._frameCounter < 180;
				if (!flag2)
				{
					this._frameCounter = 0;
					CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
					for (int i = 0; i < (int)this.FlawCount; i++)
					{
						DomainManager.Combat.AddFlaw(context, enemyChar, 2, this.SkillKey, -1, 1, true);
					}
					DomainManager.Combat.AddToCheckFallenSet(enemyChar.GetId());
					base.ShowSpecialEffectTips(0);
					base.ReduceEffectCount(1);
				}
			}
		}

		// Token: 0x04000F56 RID: 3926
		private const short AffectFrameCount = 180;

		// Token: 0x04000F57 RID: 3927
		private const sbyte FlawLevel = 2;

		// Token: 0x04000F58 RID: 3928
		protected sbyte FlawCount;

		// Token: 0x04000F59 RID: 3929
		private int _frameCounter;

		// Token: 0x04000F5A RID: 3930
		private bool _registeredStateMachineUpdateEnd;
	}
}
