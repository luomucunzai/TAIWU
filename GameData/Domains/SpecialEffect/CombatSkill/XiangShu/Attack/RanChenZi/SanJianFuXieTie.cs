using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.RanChenZi
{
	// Token: 0x020002EC RID: 748
	public class SanJianFuXieTie : CombatSkillEffectBase
	{
		// Token: 0x06003350 RID: 13136 RVA: 0x00224687 File Offset: 0x00222887
		public SanJianFuXieTie()
		{
		}

		// Token: 0x06003351 RID: 13137 RVA: 0x00224691 File Offset: 0x00222891
		public SanJianFuXieTie(CombatSkillKey skillKey) : base(skillKey, 17132, -1)
		{
		}

		// Token: 0x06003352 RID: 13138 RVA: 0x002246A4 File Offset: 0x002228A4
		public override void OnEnable(DataContext context)
		{
			sbyte taskStatus = DomainManager.World.GetElement_XiangshuAvatarTaskStatuses(1).JuniorXiangshuTaskStatus;
			bool flag = taskStatus > 4;
			if (flag)
			{
				bool goodEnding = taskStatus == 6;
				bool flag2 = goodEnding;
				if (flag2)
				{
					DomainManager.Combat.RemoveAllFlaw(context, base.CurrEnemyChar);
				}
				else
				{
					DomainManager.Combat.AddFlaw(context, base.CurrEnemyChar, 3, this.SkillKey, -1, 1, true);
				}
				base.ShowSpecialEffectTips(goodEnding, 1, 2);
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003353 RID: 13139 RVA: 0x00224723 File Offset: 0x00222923
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003354 RID: 13140 RVA: 0x00224738 File Offset: 0x00222938
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					CombatCharacter enemyChar = base.CurrEnemyChar;
					base.ChangeMobilityValue(context, enemyChar, -enemyChar.GetMobilityValue());
					base.ClearAffectingAgileSkill(context, enemyChar);
					base.ChangeBreathValue(context, enemyChar, -30000);
					base.ChangeStanceValue(context, enemyChar, -4000);
					base.ShowSpecialEffectTips(0);
				}
				base.RemoveSelf(context);
			}
		}
	}
}
