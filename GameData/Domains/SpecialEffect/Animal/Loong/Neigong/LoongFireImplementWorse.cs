using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.SpecialEffect.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x020005F1 RID: 1521
	public class LoongFireImplementWorse : ISpecialEffectImplement, ISpecialEffectModifier
	{
		// Token: 0x060044BB RID: 17595 RVA: 0x0027072F File Offset: 0x0026E92F
		public LoongFireImplementWorse(CValuePercent worsenPercent)
		{
			this._worsenPercent = worsenPercent;
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x060044BC RID: 17596 RVA: 0x0027073F File Offset: 0x0026E93F
		// (set) Token: 0x060044BD RID: 17597 RVA: 0x00270747 File Offset: 0x0026E947
		public CombatSkillEffectBase EffectBase { get; set; }

		// Token: 0x060044BE RID: 17598 RVA: 0x00270750 File Offset: 0x0026E950
		public void OnEnable(DataContext context)
		{
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x060044BF RID: 17599 RVA: 0x00270765 File Offset: 0x0026E965
		public void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x060044C0 RID: 17600 RVA: 0x0027077C File Offset: 0x0026E97C
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightback)
		{
			bool flag = attacker.GetId() != this.EffectBase.CharacterId || isFightback || !hit || pursueIndex > 0;
			if (!flag)
			{
				sbyte bodyPart = attacker.NormalAttackBodyPart;
				bool flag2 = bodyPart < 0 || bodyPart >= 7;
				if (!flag2)
				{
					defender.WorsenInjury(context, bodyPart, true, this._worsenPercent);
					defender.WorsenInjury(context, bodyPart, false, this._worsenPercent);
					this.EffectBase.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x04001456 RID: 5206
		private readonly CValuePercent _worsenPercent;
	}
}
