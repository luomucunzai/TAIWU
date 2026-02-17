using System;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Agile
{
	// Token: 0x02000289 RID: 649
	public class GuSheTaXue : CheckHitEffect
	{
		// Token: 0x06003119 RID: 12569 RVA: 0x00219B07 File Offset: 0x00217D07
		public GuSheTaXue()
		{
		}

		// Token: 0x0600311A RID: 12570 RVA: 0x00219B11 File Offset: 0x00217D11
		public GuSheTaXue(CombatSkillKey skillKey) : base(skillKey, 8405)
		{
			this.CheckHitType = 3;
		}

		// Token: 0x0600311B RID: 12571 RVA: 0x00219B28 File Offset: 0x00217D28
		protected override bool HitEffect(DataContext context)
		{
			CombatCharacter trickChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			int maxReplaceCount = 1 + Math.Clamp((int)(base.CombatChar.GetCharacter().GetAttraction() / 360), 0, 2);
			int replaceCount = trickChar.ReplaceUsableTrick(context, 20, maxReplaceCount);
			return replaceCount > 0;
		}

		// Token: 0x04000E8C RID: 3724
		private const int AttractionUnit = 360;

		// Token: 0x04000E8D RID: 3725
		private const int MaxReplaceCount = 3;
	}
}
