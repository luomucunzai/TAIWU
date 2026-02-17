using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Agile
{
	// Token: 0x02000215 RID: 533
	public class QianLiDuXing : AgileSkillBase
	{
		// Token: 0x06002EFD RID: 12029 RVA: 0x0021133D File Offset: 0x0020F53D
		public QianLiDuXing()
		{
		}

		// Token: 0x06002EFE RID: 12030 RVA: 0x00211347 File Offset: 0x0020F547
		public QianLiDuXing(CombatSkillKey skillKey) : base(skillKey, 5402)
		{
		}

		// Token: 0x06002EFF RID: 12031 RVA: 0x00211357 File Offset: 0x0020F557
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x06002F00 RID: 12032 RVA: 0x00211374 File Offset: 0x0020F574
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x06002F01 RID: 12033 RVA: 0x00211394 File Offset: 0x0020F594
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover != base.CombatChar || !isMove || isForced || !(base.IsDirect ? (distance < 0) : (distance > 0));
			if (!flag)
			{
				this._distanceAccumulator += (int)Math.Abs(distance);
				while (this._distanceAccumulator >= 10)
				{
					this._distanceAccumulator -= 10;
					bool flag2 = !base.CanAffect;
					if (!flag2)
					{
						base.ChangeMobilityValue(context, base.CombatChar, MoveSpecialConstants.MaxMobility * 5 / 100);
						base.ShowSpecialEffectTips(0);
					}
				}
			}
		}

		// Token: 0x04000DF3 RID: 3571
		private const sbyte RequireDistance = 10;

		// Token: 0x04000DF4 RID: 3572
		private const sbyte AddMobilityPercent = 5;

		// Token: 0x04000DF5 RID: 3573
		private int _distanceAccumulator;
	}
}
