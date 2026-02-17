using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Special
{
	// Token: 0x02000449 RID: 1097
	public class DieDaMeiShanZhou : CombatSkillEffectBase
	{
		// Token: 0x06003A3E RID: 14910 RVA: 0x00242BB7 File Offset: 0x00240DB7
		public DieDaMeiShanZhou()
		{
		}

		// Token: 0x06003A3F RID: 14911 RVA: 0x00242BC1 File Offset: 0x00240DC1
		public DieDaMeiShanZhou(CombatSkillKey skillKey) : base(skillKey, 7300, -1)
		{
		}

		// Token: 0x06003A40 RID: 14912 RVA: 0x00242BD2 File Offset: 0x00240DD2
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003A41 RID: 14913 RVA: 0x00242BE7 File Offset: 0x00240DE7
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003A42 RID: 14914 RVA: 0x00242BFC File Offset: 0x00240DFC
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					CombatCharacter affectChar = base.IsDirect ? base.CurrEnemyChar : base.CombatChar;
					CValuePercent percent = base.IsDirect ? DieDaMeiShanZhou.DirectChangeMobility : DieDaMeiShanZhou.ReverseChangeMobility;
					base.ChangeMobilityValue(context, affectChar, MoveSpecialConstants.MaxMobility * percent);
					base.ShowSpecialEffectTips(0);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0400110B RID: 4363
		private static readonly CValuePercent DirectChangeMobility = -60;

		// Token: 0x0400110C RID: 4364
		private static readonly CValuePercent ReverseChangeMobility = 30;
	}
}
