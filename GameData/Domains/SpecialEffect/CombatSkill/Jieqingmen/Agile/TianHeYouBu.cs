using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Agile
{
	// Token: 0x02000507 RID: 1287
	public class TianHeYouBu : AgileSkillBase
	{
		// Token: 0x06003EB3 RID: 16051 RVA: 0x00256DCD File Offset: 0x00254FCD
		public TianHeYouBu()
		{
		}

		// Token: 0x06003EB4 RID: 16052 RVA: 0x00256DD7 File Offset: 0x00254FD7
		public TianHeYouBu(CombatSkillKey skillKey) : base(skillKey, 13403)
		{
		}

		// Token: 0x06003EB5 RID: 16053 RVA: 0x00256DE7 File Offset: 0x00254FE7
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._distanceAccumulator = 0;
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x06003EB6 RID: 16054 RVA: 0x00256E0B File Offset: 0x0025500B
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x06003EB7 RID: 16055 RVA: 0x00256E28 File Offset: 0x00255028
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover != base.CombatChar || !isMove || isForced || !(base.IsDirect ? (distance < 0) : (distance > 0));
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
						byte type = base.IsDirect ? 0 : 2;
						bool flag4 = base.CombatChar.AbsorbNeiliAllocation(context, base.EnemyChar, type, 2);
						if (flag4)
						{
							base.ShowSpecialEffectTips(0);
						}
					}
				}
			}
		}

		// Token: 0x04001281 RID: 4737
		private const sbyte RequireMoveDistance = 10;

		// Token: 0x04001282 RID: 4738
		private const int AbsorbNeiliAllocationValue = 2;

		// Token: 0x04001283 RID: 4739
		private int _distanceAccumulator;
	}
}
