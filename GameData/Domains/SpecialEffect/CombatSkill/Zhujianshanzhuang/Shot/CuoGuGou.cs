using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Shot
{
	// Token: 0x020001BB RID: 443
	public class CuoGuGou : CombatSkillEffectBase
	{
		// Token: 0x06002C8D RID: 11405 RVA: 0x00207DB4 File Offset: 0x00205FB4
		public CuoGuGou()
		{
		}

		// Token: 0x06002C8E RID: 11406 RVA: 0x00207DBE File Offset: 0x00205FBE
		public CuoGuGou(CombatSkillKey skillKey) : base(skillKey, 9401, -1)
		{
		}

		// Token: 0x06002C8F RID: 11407 RVA: 0x00207DCF File Offset: 0x00205FCF
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002C90 RID: 11408 RVA: 0x00207DE4 File Offset: 0x00205FE4
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002C91 RID: 11409 RVA: 0x00207DFC File Offset: 0x00205FFC
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0) && base.CombatChar.GetTrickCount(12) > 0;
				if (flag2)
				{
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						base.ChangeStanceValue(context, base.CurrEnemyChar, -1200);
					}
					else
					{
						base.ChangeBreathValue(context, base.CurrEnemyChar, -9000);
					}
					DomainManager.Combat.RemoveTrick(context, base.CombatChar, 12, 1, true, -1);
					base.ShowSpecialEffectTips(0);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000D6E RID: 3438
		private const sbyte ReduceBreathStance = 30;
	}
}
