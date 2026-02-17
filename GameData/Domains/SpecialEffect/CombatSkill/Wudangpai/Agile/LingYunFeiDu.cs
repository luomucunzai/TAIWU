using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Agile
{
	// Token: 0x020003E0 RID: 992
	public class LingYunFeiDu : AgileSkillBase
	{
		// Token: 0x060037F6 RID: 14326 RVA: 0x002380FE File Offset: 0x002362FE
		public LingYunFeiDu()
		{
		}

		// Token: 0x060037F7 RID: 14327 RVA: 0x00238108 File Offset: 0x00236308
		public LingYunFeiDu(CombatSkillKey skillKey) : base(skillKey, 4406)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x060037F8 RID: 14328 RVA: 0x00238120 File Offset: 0x00236320
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			this._affecting = false;
			this.OnMoveSkillCanAffectChanged(context, default(DataUid));
		}

		// Token: 0x060037F9 RID: 14329 RVA: 0x00238160 File Offset: 0x00236360
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
		}

		// Token: 0x060037FA RID: 14330 RVA: 0x00238198 File Offset: 0x00236398
		private unsafe void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover != base.CombatChar || !isMove || isForced || !(base.IsDirect ? (distance < 0) : (distance > 0)) || !this._affecting || DomainManager.Combat.IsMovedByTeammate(base.CombatChar);
			if (!flag)
			{
				byte neiliAllocationType = (byte)context.Random.Next(0, 4);
				base.CombatChar.ChangeNeiliAllocation(context, neiliAllocationType, 5, true, true);
				base.ShowSpecialEffectTips(0);
				bool flag2 = *base.CombatChar.GetNeiliAllocation()[(int)neiliAllocationType] < *base.CombatChar.GetOriginNeiliAllocation()[(int)neiliAllocationType];
				if (flag2)
				{
					base.CombatChar.ChangeNeiliAllocation(context, neiliAllocationType, 15, true, true);
				}
			}
		}

		// Token: 0x060037FB RID: 14331 RVA: 0x0023825C File Offset: 0x0023645C
		protected override void OnMoveSkillCanAffectChanged(DataContext context, DataUid dataUid)
		{
			bool canAffect = base.CanAffect;
			bool flag = this._affecting == canAffect;
			if (!flag)
			{
				this._affecting = canAffect;
				bool flag2 = canAffect;
				if (flag2)
				{
					DomainManager.Combat.EnableJumpMove(base.CombatChar, base.SkillTemplateId, base.IsDirect);
				}
				else
				{
					DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
				}
			}
		}

		// Token: 0x04001057 RID: 4183
		private const sbyte AddNeiliAllocation = 5;

		// Token: 0x04001058 RID: 4184
		private const sbyte ExtraAddNeiliAllocation = 15;

		// Token: 0x04001059 RID: 4185
		private bool _affecting;
	}
}
