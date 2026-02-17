using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.HuanXin
{
	// Token: 0x02000316 RID: 790
	public class ShiFeng : CombatSkillEffectBase
	{
		// Token: 0x06003410 RID: 13328 RVA: 0x002279DD File Offset: 0x00225BDD
		public ShiFeng()
		{
		}

		// Token: 0x06003411 RID: 13329 RVA: 0x002279E7 File Offset: 0x00225BE7
		public ShiFeng(CombatSkillKey skillKey) : base(skillKey, 17102, -1)
		{
		}

		// Token: 0x06003412 RID: 13330 RVA: 0x002279F8 File Offset: 0x00225BF8
		public override void OnEnable(DataContext context)
		{
			DomainManager.Combat.AddCombatState(context, base.CombatChar, 1, 93);
			base.ShowSpecialEffectTips(0);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003413 RID: 13331 RVA: 0x00227A2A File Offset: 0x00225C2A
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003414 RID: 13332 RVA: 0x00227A40 File Offset: 0x00225C40
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}
	}
}
