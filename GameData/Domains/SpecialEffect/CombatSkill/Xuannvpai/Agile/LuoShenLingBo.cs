using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Agile
{
	// Token: 0x0200028A RID: 650
	public class LuoShenLingBo : AgileSkillBase
	{
		// Token: 0x0600311C RID: 12572 RVA: 0x00219B83 File Offset: 0x00217D83
		public LuoShenLingBo()
		{
		}

		// Token: 0x0600311D RID: 12573 RVA: 0x00219B8D File Offset: 0x00217D8D
		public LuoShenLingBo(CombatSkillKey skillKey) : base(skillKey, 8407)
		{
		}

		// Token: 0x0600311E RID: 12574 RVA: 0x00219B9D File Offset: 0x00217D9D
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._distanceAccumulator = 0;
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x0600311F RID: 12575 RVA: 0x00219BC1 File Offset: 0x00217DC1
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x06003120 RID: 12576 RVA: 0x00219BE0 File Offset: 0x00217DE0
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover != base.CombatChar || !isMove || isForced || !(base.IsDirect ? (distance < 0) : (distance > 0));
			if (!flag)
			{
				this._distanceAccumulator += (int)Math.Abs(distance);
				bool flag2 = this._distanceAccumulator < 20;
				if (!flag2)
				{
					int affectCount = this._distanceAccumulator / 20;
					this._distanceAccumulator -= affectCount * 20;
					bool flag3 = !base.CanAffect;
					if (!flag3)
					{
						base.ChangeBreathValue(context, base.CombatChar, 9000);
						base.ChangeStanceValue(context, base.CombatChar, 1200);
						DomainManager.Combat.UpdateSkillCostBreathStanceCanUse(context, base.CombatChar);
						base.ShowSpecialEffectTips(0);
						CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
						bool flag4 = !DomainManager.Combat.InAttackRange(enemyChar);
						if (!flag4)
						{
							int mindMarkCount = 1 + Math.Clamp((int)(base.CombatChar.GetCharacter().GetAttraction() / 360), 0, 2);
							DomainManager.Combat.AppendMindDefeatMark(context, enemyChar, mindMarkCount, -1, false);
							base.ShowSpecialEffectTips(1);
						}
					}
				}
			}
		}

		// Token: 0x04000E8E RID: 3726
		private const sbyte RequireMoveDistance = 20;

		// Token: 0x04000E8F RID: 3727
		private const sbyte AddBreathStance = 30;

		// Token: 0x04000E90 RID: 3728
		private const int AttractionUnit = 360;

		// Token: 0x04000E91 RID: 3729
		private const int MaxMindMarkCount = 3;

		// Token: 0x04000E92 RID: 3730
		private int _distanceAccumulator;
	}
}
