using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Blade
{
	// Token: 0x0200020E RID: 526
	public class LiuHeDaoFa : CombatSkillEffectBase
	{
		// Token: 0x06002EE2 RID: 12002 RVA: 0x00210FEC File Offset: 0x0020F1EC
		public LiuHeDaoFa()
		{
		}

		// Token: 0x06002EE3 RID: 12003 RVA: 0x00210FF6 File Offset: 0x0020F1F6
		public LiuHeDaoFa(CombatSkillKey skillKey) : base(skillKey, 5301, -1)
		{
		}

		// Token: 0x06002EE4 RID: 12004 RVA: 0x00211007 File Offset: 0x0020F207
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002EE5 RID: 12005 RVA: 0x0021101C File Offset: 0x0020F21C
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002EE6 RID: 12006 RVA: 0x00211034 File Offset: 0x0020F234
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = power > 0;
				if (flag2)
				{
					DomainManager.Combat.AddCombatState(context, base.CombatChar, 1, base.IsDirect ? 208 : 209, (int)power);
					base.ShowSpecialEffectTips(0);
				}
				base.RemoveSelf(context);
			}
		}
	}
}
