using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.SpecialEffect.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x020005F7 RID: 1527
	public class LoongMetalImplementJump : ISpecialEffectImplement, ISpecialEffectModifier
	{
		// Token: 0x170002CF RID: 719
		// (get) Token: 0x060044D6 RID: 17622 RVA: 0x002709DB File Offset: 0x0026EBDB
		// (set) Token: 0x060044D7 RID: 17623 RVA: 0x002709E3 File Offset: 0x0026EBE3
		public CombatSkillEffectBase EffectBase { get; set; }

		// Token: 0x060044D8 RID: 17624 RVA: 0x002709EC File Offset: 0x0026EBEC
		public void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x060044D9 RID: 17625 RVA: 0x00270A01 File Offset: 0x0026EC01
		public void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x060044DA RID: 17626 RVA: 0x00270A16 File Offset: 0x0026EC16
		private void OnCombatBegin(DataContext context)
		{
			DomainManager.Combat.EnableJumpMove(this.EffectBase.CombatChar, this.EffectBase.SkillTemplateId);
		}
	}
}
