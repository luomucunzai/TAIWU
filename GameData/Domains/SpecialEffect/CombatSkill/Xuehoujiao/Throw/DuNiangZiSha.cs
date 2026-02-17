using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Throw
{
	// Token: 0x02000218 RID: 536
	public class DuNiangZiSha : CombatSkillEffectBase
	{
		// Token: 0x06002F0B RID: 12043 RVA: 0x00211708 File Offset: 0x0020F908
		public DuNiangZiSha()
		{
		}

		// Token: 0x06002F0C RID: 12044 RVA: 0x00211712 File Offset: 0x0020F912
		public DuNiangZiSha(CombatSkillKey skillKey) : base(skillKey, 15405, -1)
		{
		}

		// Token: 0x06002F0D RID: 12045 RVA: 0x00211723 File Offset: 0x0020F923
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06002F0E RID: 12046 RVA: 0x0021175C File Offset: 0x0020F95C
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06002F0F RID: 12047 RVA: 0x00211798 File Offset: 0x0020F998
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
						this._movedDistance = 0;
						this._affectTimes = 0;
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

		// Token: 0x06002F10 RID: 12048 RVA: 0x00211828 File Offset: 0x0020FA28
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover.GetId() != base.CharacterId || !isMove || isForced || !(base.IsDirect ? (distance < 0) : (distance > 0));
			if (!flag)
			{
				bool flag2 = base.CombatChar.GetTrickCount(14) <= 0;
				if (!flag2)
				{
					this._movedDistance += (int)Math.Abs(distance);
					while (this._movedDistance >= 10 && base.EffectCount > 0)
					{
						this._movedDistance -= 10;
						this._affectTimes++;
						DomainManager.Combat.RemoveTrick(context, base.CombatChar, 14, 1, true, -1);
						bool flag3 = this._affectTimes == 1;
						if (flag3)
						{
							Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
						}
						base.ReduceEffectCount(1);
					}
				}
			}
		}

		// Token: 0x06002F11 RID: 12049 RVA: 0x0021191C File Offset: 0x0020FB1C
		private void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			bool flag = combatChar.IsAlly != base.CombatChar.IsAlly;
			if (flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				for (int i = 0; i < this._affectTimes; i++)
				{
					bool flag2 = DomainManager.Combat.ChangeDistance(context, enemyChar, base.IsDirect ? -10 : 10, true);
					if (flag2)
					{
						DomainManager.Combat.AddAcupoint(context, enemyChar, 0, new CombatSkillKey(-1, -1), -1, 1, true);
					}
				}
				this._affectTimes = 0;
				base.ShowSpecialEffectTips(0);
				Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
			}
		}

		// Token: 0x06002F12 RID: 12050 RVA: 0x002119D4 File Offset: 0x0020FBD4
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000DF8 RID: 3576
		private const sbyte MoveDistance = 10;

		// Token: 0x04000DF9 RID: 3577
		private int _movedDistance;

		// Token: 0x04000DFA RID: 3578
		private int _affectTimes;
	}
}
