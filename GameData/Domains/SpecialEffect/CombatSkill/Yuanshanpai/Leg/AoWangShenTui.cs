using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Leg
{
	// Token: 0x020001FC RID: 508
	public class AoWangShenTui : CombatSkillEffectBase
	{
		// Token: 0x06002E6B RID: 11883 RVA: 0x0020EA13 File Offset: 0x0020CC13
		public AoWangShenTui()
		{
		}

		// Token: 0x06002E6C RID: 11884 RVA: 0x0020EA1D File Offset: 0x0020CC1D
		public AoWangShenTui(CombatSkillKey skillKey) : base(skillKey, 5107, -1)
		{
		}

		// Token: 0x06002E6D RID: 11885 RVA: 0x0020EA30 File Offset: 0x0020CC30
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_PrepareSkillProgressChange(new Events.OnPrepareSkillProgressChange(this.OnPrepareSkillProgressChange));
		}

		// Token: 0x06002E6E RID: 11886 RVA: 0x0020EA90 File Offset: 0x0020CC90
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_PrepareSkillProgressChange(new Events.OnPrepareSkillProgressChange(this.OnPrepareSkillProgressChange));
			base.OnDisable(context);
		}

		// Token: 0x06002E6F RID: 11887 RVA: 0x0020EAF0 File Offset: 0x0020CCF0
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = !this.SkillKey.IsMatch(charId, skillId);
			if (!flag)
			{
				this._lastUpdateProgressPercent = 0;
				base.CombatChar.SetParticleToLoopByCombatSkill("Particle_Effect_BringCloserCenter", context);
			}
		}

		// Token: 0x06002E70 RID: 11888 RVA: 0x0020EB30 File Offset: 0x0020CD30
		private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = !this.SkillKey.IsMatch(charId, skillId);
			if (!flag)
			{
				base.CombatChar.SetParticleToLoopByCombatSkill(null, context);
			}
		}

		// Token: 0x06002E71 RID: 11889 RVA: 0x0020EB64 File Offset: 0x0020CD64
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = !this.SkillKey.IsMatch(charId, skillId);
			if (!flag)
			{
				this._lastUpdateProgressPercent = 0;
				base.CombatChar.SetParticleToLoopByCombatSkill(null, context);
			}
		}

		// Token: 0x06002E72 RID: 11890 RVA: 0x0020EBA0 File Offset: 0x0020CDA0
		private void OnPrepareSkillProgressChange(DataContext context, int charId, bool isAlly, short skillId, sbyte preparePercent)
		{
			bool flag = !this.SkillKey.IsMatch(charId, skillId);
			if (!flag)
			{
				int deltaPercent = (int)preparePercent - this._lastUpdateProgressPercent;
				while (deltaPercent >= 2 && !this.IsInAttackRangeCenter())
				{
					deltaPercent -= 2;
					this.DoChangeDistance(context);
				}
				while (deltaPercent >= 10 && this.IsInAttackRangeCenter())
				{
					deltaPercent -= 10;
					this.DoCenterDamage(context);
				}
				this._lastUpdateProgressPercent = (int)preparePercent - deltaPercent;
			}
		}

		// Token: 0x06002E73 RID: 11891 RVA: 0x0020EC20 File Offset: 0x0020CE20
		private int CalcRangeCenter()
		{
			OuterAndInnerInts attackRange = DomainManager.Combat.GetSkillAttackRange(base.CombatChar, base.SkillTemplateId);
			return (attackRange.Outer + attackRange.Inner) / 2;
		}

		// Token: 0x06002E74 RID: 11892 RVA: 0x0020EC58 File Offset: 0x0020CE58
		private bool IsInAttackRangeCenter()
		{
			int rangeCenter = this.CalcRangeCenter();
			short currDistance = DomainManager.Combat.GetCurrentDistance();
			return Math.Abs((int)currDistance - rangeCenter) <= 1;
		}

		// Token: 0x06002E75 RID: 11893 RVA: 0x0020EC8C File Offset: 0x0020CE8C
		private void DoChangeDistance(DataContext context)
		{
			int rangeCenter = this.CalcRangeCenter();
			short currDistance = DomainManager.Combat.GetCurrentDistance();
			bool flag = rangeCenter == (int)currDistance;
			if (!flag)
			{
				int direction = (rangeCenter < (int)currDistance) ? -1 : 1;
				DomainManager.Combat.ChangeDistance(context, base.EnemyChar, direction, true);
			}
		}

		// Token: 0x06002E76 RID: 11894 RVA: 0x0020ECD4 File Offset: 0x0020CED4
		private void DoCenterDamage(DataContext context)
		{
			short baseValue = base.IsDirect ? this.CharObj.GetRecoveryOfFlaw() : this.CharObj.GetRecoveryOfBlockedAcupoint();
			int damageValue = (int)(100 + baseValue / 2);
			sbyte part = base.EnemyChar.RandomInjuryBodyPartMustValid(context.Random, !base.IsDirect, null);
			int markCount = DomainManager.Combat.AddInjuryDamageValue(base.CombatChar, base.EnemyChar, part, base.IsDirect ? damageValue : 0, base.IsDirect ? 0 : damageValue, base.SkillTemplateId, true);
			for (int i = 0; i < markCount; i++)
			{
				bool flag = !context.Random.CheckPercentProb(50);
				if (!flag)
				{
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						DomainManager.Combat.AddFlaw(context, base.EnemyChar, 1, this.SkillKey, -1, 1, true);
					}
					else
					{
						DomainManager.Combat.AddAcupoint(context, base.EnemyChar, 1, this.SkillKey, -1, 1, true);
					}
				}
			}
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x04000DC7 RID: 3527
		private const string ParticleName = "Particle_Effect_BringCloserCenter";

		// Token: 0x04000DC8 RID: 3528
		private const int ChangeDistanceProgressPercent = 2;

		// Token: 0x04000DC9 RID: 3529
		private const int CenterDamageProgressPercent = 10;

		// Token: 0x04000DCA RID: 3530
		private const int CenterDamageRange = 1;

		// Token: 0x04000DCB RID: 3531
		private const int CenterDamageBase = 100;

		// Token: 0x04000DCC RID: 3532
		private const int CenterDamageDivisor = 2;

		// Token: 0x04000DCD RID: 3533
		private const int FlawOrAcupointOdds = 50;

		// Token: 0x04000DCE RID: 3534
		private const sbyte FlawOrAcupointLevel = 1;

		// Token: 0x04000DCF RID: 3535
		private int _lastUpdateProgressPercent;
	}
}
