using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Agile
{
	// Token: 0x020003B3 RID: 947
	public class YuSuoDaoXuan : AgileSkillBase
	{
		// Token: 0x060036F8 RID: 14072 RVA: 0x00233193 File Offset: 0x00231393
		public YuSuoDaoXuan()
		{
		}

		// Token: 0x060036F9 RID: 14073 RVA: 0x0023319D File Offset: 0x0023139D
		public YuSuoDaoXuan(CombatSkillKey skillKey) : base(skillKey, 12604)
		{
		}

		// Token: 0x060036FA RID: 14074 RVA: 0x002331AD File Offset: 0x002313AD
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._distanceAccumulator = 0;
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x060036FB RID: 14075 RVA: 0x002331D1 File Offset: 0x002313D1
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x060036FC RID: 14076 RVA: 0x002331F0 File Offset: 0x002313F0
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
						DomainManager.Combat.AddCombatState(context, base.IsDirect ? base.CombatChar : base.CurrEnemyChar, base.IsDirect ? 1 : 2, base.IsDirect ? 61 : 62, 50);
						base.ShowSpecialEffectTips(0);
					}
				}
			}
		}

		// Token: 0x04001009 RID: 4105
		private const sbyte RequireMoveDistance = 10;

		// Token: 0x0400100A RID: 4106
		private const sbyte StatePowerUnit = 50;

		// Token: 0x0400100B RID: 4107
		private int _distanceAccumulator;
	}
}
