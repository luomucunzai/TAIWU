using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Agile
{
	// Token: 0x02000338 RID: 824
	public class HuaJian : AgileSkillBase
	{
		// Token: 0x060034A4 RID: 13476 RVA: 0x00229513 File Offset: 0x00227713
		public HuaJian()
		{
		}

		// Token: 0x060034A5 RID: 13477 RVA: 0x0022951D File Offset: 0x0022771D
		public HuaJian(CombatSkillKey skillKey) : base(skillKey, 16211)
		{
		}

		// Token: 0x060034A6 RID: 13478 RVA: 0x0022952D File Offset: 0x0022772D
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._distanceAccumulator = 0;
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x060034A7 RID: 13479 RVA: 0x00229551 File Offset: 0x00227751
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x060034A8 RID: 13480 RVA: 0x00229570 File Offset: 0x00227770
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
						DomainManager.Combat.AddTrick(context, base.CombatChar, 21, true);
						base.ShowSpecialEffectTips(0);
					}
				}
			}
		}

		// Token: 0x04000F82 RID: 3970
		private const sbyte RequireMoveDistance = 10;

		// Token: 0x04000F83 RID: 3971
		private int _distanceAccumulator;
	}
}
