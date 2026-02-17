using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Agile
{
	// Token: 0x02000470 RID: 1136
	public class YuFengFu : AgileSkillBase
	{
		// Token: 0x06003B42 RID: 15170 RVA: 0x00247411 File Offset: 0x00245611
		public YuFengFu()
		{
		}

		// Token: 0x06003B43 RID: 15171 RVA: 0x0024741B File Offset: 0x0024561B
		public YuFengFu(CombatSkillKey skillKey) : base(skillKey, 7401)
		{
		}

		// Token: 0x06003B44 RID: 15172 RVA: 0x0024742B File Offset: 0x0024562B
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._distanceAccumulator = 0;
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x06003B45 RID: 15173 RVA: 0x0024744F File Offset: 0x0024564F
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x06003B46 RID: 15174 RVA: 0x0024746C File Offset: 0x0024566C
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover != base.CombatChar || !isMove || isForced;
			if (!flag)
			{
				this._distanceAccumulator += (int)Math.Abs(distance);
				bool flag2 = this._distanceAccumulator < 10;
				if (!flag2)
				{
					this._distanceAccumulator = 0;
					bool flag3 = !base.CanAffect;
					if (!flag3)
					{
						DomainManager.Combat.AddCombatState(context, base.IsDirect ? base.CombatChar : base.CurrEnemyChar, base.IsDirect ? 1 : 2, base.IsDirect ? 34 : 35, 50);
						base.ShowSpecialEffectTips(0);
					}
				}
			}
		}

		// Token: 0x0400115A RID: 4442
		private const sbyte RequireMoveDistance = 10;

		// Token: 0x0400115B RID: 4443
		private const sbyte StatePowerUnit = 50;

		// Token: 0x0400115C RID: 4444
		private int _distanceAccumulator;
	}
}
