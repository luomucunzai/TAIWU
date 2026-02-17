using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Agile
{
	// Token: 0x0200040D RID: 1037
	public class HengJiangSuo : AgileSkillBase
	{
		// Token: 0x060038FD RID: 14589 RVA: 0x0023CB68 File Offset: 0x0023AD68
		public HengJiangSuo()
		{
		}

		// Token: 0x060038FE RID: 14590 RVA: 0x0023CB72 File Offset: 0x0023AD72
		public HengJiangSuo(CombatSkillKey skillKey) : base(skillKey, 6402)
		{
		}

		// Token: 0x060038FF RID: 14591 RVA: 0x0023CB82 File Offset: 0x0023AD82
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._distanceAccumulator = 0;
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x06003900 RID: 14592 RVA: 0x0023CBA6 File Offset: 0x0023ADA6
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x06003901 RID: 14593 RVA: 0x0023CBC4 File Offset: 0x0023ADC4
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover.IsAlly == base.CombatChar.IsAlly || !isMove || isForced || !(base.IsDirect ? (distance < 0) : (distance > 0));
			if (!flag)
			{
				this._distanceAccumulator += (int)Math.Abs(distance);
				bool flag2 = this._distanceAccumulator < 10;
				if (!flag2)
				{
					this._distanceAccumulator = 0;
					bool flag3 = !DomainManager.Combat.InAttackRange(base.CombatChar) || !base.CanAffect;
					if (!flag3)
					{
						base.CombatChar.NeedNormalAttackSkipPrepare++;
						base.ShowSpecialEffectTips(0);
					}
				}
			}
		}

		// Token: 0x040010AA RID: 4266
		private const sbyte RequireMoveDistance = 10;

		// Token: 0x040010AB RID: 4267
		private int _distanceAccumulator;
	}
}
