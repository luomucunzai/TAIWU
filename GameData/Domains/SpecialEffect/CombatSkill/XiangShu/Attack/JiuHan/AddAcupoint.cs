using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.JiuHan
{
	// Token: 0x020002FF RID: 767
	public class AddAcupoint : CombatSkillEffectBase
	{
		// Token: 0x060033B3 RID: 13235 RVA: 0x00226602 File Offset: 0x00224802
		protected AddAcupoint()
		{
		}

		// Token: 0x060033B4 RID: 13236 RVA: 0x0022660C File Offset: 0x0022480C
		protected AddAcupoint(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x060033B5 RID: 13237 RVA: 0x00226619 File Offset: 0x00224819
		public override void OnEnable(DataContext context)
		{
			this._registeredStateMachineUpdateEnd = false;
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x060033B6 RID: 13238 RVA: 0x00226648 File Offset: 0x00224848
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

		// Token: 0x060033B7 RID: 13239 RVA: 0x00226698 File Offset: 0x00224898
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

		// Token: 0x060033B8 RID: 13240 RVA: 0x00226730 File Offset: 0x00224930
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x060033B9 RID: 13241 RVA: 0x00226780 File Offset: 0x00224980
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
					for (int i = 0; i < (int)this.AcupointCount; i++)
					{
						DomainManager.Combat.AddAcupoint(context, enemyChar, 2, this.SkillKey, -1, 1, true);
					}
					DomainManager.Combat.AddToCheckFallenSet(enemyChar.GetId());
					base.ShowSpecialEffectTips(0);
					base.ReduceEffectCount(1);
				}
			}
		}

		// Token: 0x04000F4A RID: 3914
		private const short AffectFrameCount = 180;

		// Token: 0x04000F4B RID: 3915
		private const sbyte AcupointLevel = 2;

		// Token: 0x04000F4C RID: 3916
		protected sbyte AcupointCount;

		// Token: 0x04000F4D RID: 3917
		private int _frameCounter;

		// Token: 0x04000F4E RID: 3918
		private bool _registeredStateMachineUpdateEnd;
	}
}
