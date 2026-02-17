using System;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Whip
{
	// Token: 0x0200037C RID: 892
	public class BaLongShenBian : PoisonAddInjury
	{
		// Token: 0x060035D7 RID: 13783 RVA: 0x0022E189 File Offset: 0x0022C389
		public BaLongShenBian()
		{
		}

		// Token: 0x060035D8 RID: 13784 RVA: 0x0022E193 File Offset: 0x0022C393
		public BaLongShenBian(CombatSkillKey skillKey) : base(skillKey, 12407)
		{
			this.RequirePoisonType = 0;
		}

		// Token: 0x060035D9 RID: 13785 RVA: 0x0022E1AC File Offset: 0x0022C3AC
		protected override void OnCastMaxPower(DataContext context)
		{
			CombatCharacter poisonChar = base.IsDirect ? DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false) : base.CombatChar;
			int addPower = (int)(poisonChar.GetDefeatMarkCollection().PoisonMarkList[(int)this.RequirePoisonType] * 10);
			bool flag = addPower == 0;
			if (!flag)
			{
				DomainManager.Combat.AddSkillPowerInCombat(context, this.SkillKey, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), addPower);
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x04000FB2 RID: 4018
		private const int AddPowerUnit = 10;
	}
}
