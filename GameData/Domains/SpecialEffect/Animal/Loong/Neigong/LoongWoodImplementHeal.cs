using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.SpecialEffect.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x0200060C RID: 1548
	public class LoongWoodImplementHeal : ISpecialEffectImplement, ISpecialEffectModifier
	{
		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06004552 RID: 17746 RVA: 0x00272097 File Offset: 0x00270297
		// (set) Token: 0x06004553 RID: 17747 RVA: 0x0027209F File Offset: 0x0027029F
		public CombatSkillEffectBase EffectBase { get; set; }

		// Token: 0x06004554 RID: 17748 RVA: 0x002720A8 File Offset: 0x002702A8
		public void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x06004555 RID: 17749 RVA: 0x002720BD File Offset: 0x002702BD
		public void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x06004556 RID: 17750 RVA: 0x002720D4 File Offset: 0x002702D4
		private void OnCombatBegin(DataContext context)
		{
			CombatCharacter combatChar = this.EffectBase.CombatChar;
			combatChar.InnerInjuryAutoHealSpeeds.Add(2);
			combatChar.OuterInjuryAutoHealSpeeds.Add(2);
			combatChar.InnerOldInjuryAutoHealSpeeds.Add(1);
			combatChar.OuterOldInjuryAutoHealSpeeds.Add(1);
		}

		// Token: 0x0400147C RID: 5244
		private const short NewInjuryAutoHealSpeed = 2;

		// Token: 0x0400147D RID: 5245
		private const short OldInjuryAutoHealSpeed = 1;
	}
}
