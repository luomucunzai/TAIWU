using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Sword
{
	// Token: 0x020003BC RID: 956
	public class BaiHongJianFa : CombatSkillEffectBase
	{
		// Token: 0x0600372E RID: 14126 RVA: 0x0023468B File Offset: 0x0023288B
		public BaiHongJianFa()
		{
		}

		// Token: 0x0600372F RID: 14127 RVA: 0x00234695 File Offset: 0x00232895
		public BaiHongJianFa(CombatSkillKey skillKey) : base(skillKey, 4202, -1)
		{
		}

		// Token: 0x06003730 RID: 14128 RVA: 0x002346A6 File Offset: 0x002328A6
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x06003731 RID: 14129 RVA: 0x002346DF File Offset: 0x002328DF
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x06003732 RID: 14130 RVA: 0x00234718 File Offset: 0x00232918
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = !this.SkillKey.IsMatch(charId, skillId) || !base.CombatChar.GetAutoCastingSkill();
			if (!flag)
			{
				DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * 75 / 100);
			}
		}

		// Token: 0x06003733 RID: 14131 RVA: 0x00234770 File Offset: 0x00232970
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = !this.SkillKey.IsMatch(charId, skillId);
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0) && !base.CombatChar.GetAutoCastingSkill();
				if (flag2)
				{
					base.AddMaxEffectCount(true);
				}
			}
		}

		// Token: 0x06003734 RID: 14132 RVA: 0x002347C0 File Offset: 0x002329C0
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover.IsAlly == base.CombatChar.IsAlly || !isMove || !(base.IsDirect ? (distance > 0) : (distance < 0));
			if (!flag)
			{
				bool flag2 = base.EffectCount <= 0;
				if (!flag2)
				{
					this._enemyMoveAccumulator += (int)Math.Abs(distance);
					bool flag3 = this._enemyMoveAccumulator < 10;
					if (!flag3)
					{
						this._enemyMoveAccumulator = 0;
						bool changeToZero = base.EffectCount > 0;
						base.ReduceEffectCount(1);
						changeToZero = (changeToZero && base.EffectCount == 0);
						DomainManager.Combat.ChangeDistance(context, base.CombatChar, base.IsDirect ? -10 : 10);
						base.ShowSpecialEffectTips(0);
						bool flag4 = !changeToZero || !DomainManager.Combat.CanCastSkill(base.CombatChar, base.SkillTemplateId, true, true);
						if (!flag4)
						{
							DomainManager.Combat.CastSkillFree(context, base.CombatChar, base.SkillTemplateId, ECombatCastFreePriority.Normal);
							base.ShowSpecialEffectTips(1);
						}
					}
				}
			}
		}

		// Token: 0x04001020 RID: 4128
		private const sbyte EnemyMoveDistance = 10;

		// Token: 0x04001021 RID: 4129
		private const sbyte SelfMoveDistance = 10;

		// Token: 0x04001022 RID: 4130
		private const sbyte PrepareProgressPercent = 75;

		// Token: 0x04001023 RID: 4131
		private int _enemyMoveAccumulator;
	}
}
