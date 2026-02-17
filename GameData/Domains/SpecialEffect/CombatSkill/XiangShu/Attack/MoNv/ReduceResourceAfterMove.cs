using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.MoNv
{
	// Token: 0x020002F5 RID: 757
	public class ReduceResourceAfterMove : CombatSkillEffectBase
	{
		// Token: 0x06003380 RID: 13184 RVA: 0x002253A4 File Offset: 0x002235A4
		protected ReduceResourceAfterMove()
		{
		}

		// Token: 0x06003381 RID: 13185 RVA: 0x002253AE File Offset: 0x002235AE
		protected ReduceResourceAfterMove(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06003382 RID: 13186 RVA: 0x002253BB File Offset: 0x002235BB
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06003383 RID: 13187 RVA: 0x002253F4 File Offset: 0x002235F4
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06003384 RID: 13188 RVA: 0x00225430 File Offset: 0x00223630
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = !this.IsSrcSkillPerformed;
				if (flag2)
				{
					bool flag3 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag3)
					{
						this.IsSrcSkillPerformed = true;
						base.AddMaxEffectCount(true);
					}
					else
					{
						base.RemoveSelf(context);
					}
				}
				else
				{
					bool flag4 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag4)
					{
						base.RemoveSelf(context);
					}
				}
			}
		}

		// Token: 0x06003385 RID: 13189 RVA: 0x002254B0 File Offset: 0x002236B0
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = !this.IsSrcSkillPerformed || mover.IsAlly == base.CombatChar.IsAlly || !isMove || isForced;
			if (!flag)
			{
				this._movedDistance += (int)Math.Abs(distance);
				bool flag2 = this._movedDistance >= 20;
				if (flag2)
				{
					this._movedDistance -= 20;
					CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
					base.ChangeBreathValue(context, enemyChar, -30000);
					base.ChangeStanceValue(context, enemyChar, -4000);
					base.ChangeMobilityValue(context, enemyChar, -MoveSpecialConstants.MaxMobility);
					base.ClearAffectingAgileSkill(context, enemyChar);
					base.ShowSpecialEffectTips(0);
					base.ReduceEffectCount(1);
				}
			}
		}

		// Token: 0x06003386 RID: 13190 RVA: 0x00225580 File Offset: 0x00223780
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000F3B RID: 3899
		private const sbyte RequireMoveDistance = 20;

		// Token: 0x04000F3C RID: 3900
		private int _movedDistance;
	}
}
