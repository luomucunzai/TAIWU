using System;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Implement
{
	// Token: 0x02000582 RID: 1410
	public interface ISpecialEffectImplement : ISpecialEffectModifier
	{
		// Token: 0x17000282 RID: 642
		// (get) Token: 0x060041C2 RID: 16834
		// (set) Token: 0x060041C3 RID: 16835
		CombatSkillEffectBase EffectBase { get; set; }

		// Token: 0x060041C4 RID: 16836
		void OnEnable(DataContext context);

		// Token: 0x060041C5 RID: 16837
		void OnDisable(DataContext context);
	}
}
