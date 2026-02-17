using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x02000606 RID: 1542
	public class LoongWaterImplementPoison : ISpecialEffectImplement, ISpecialEffectModifier
	{
		// Token: 0x06004536 RID: 17718 RVA: 0x00271D7F File Offset: 0x0026FF7F
		public LoongWaterImplementPoison(int addPoisonDelayFrame, int addPoisonValue)
		{
			this._addPoisonDelayFrame = addPoisonDelayFrame;
			this._addPoisonValue = addPoisonValue;
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06004537 RID: 17719 RVA: 0x00271D97 File Offset: 0x0026FF97
		// (set) Token: 0x06004538 RID: 17720 RVA: 0x00271D9F File Offset: 0x0026FF9F
		public CombatSkillEffectBase EffectBase { get; set; }

		// Token: 0x06004539 RID: 17721 RVA: 0x00271DA8 File Offset: 0x0026FFA8
		public void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnCombatStateMachineUpdateEnd));
		}

		// Token: 0x0600453A RID: 17722 RVA: 0x00271DBD File Offset: 0x0026FFBD
		public void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnCombatStateMachineUpdateEnd));
		}

		// Token: 0x0600453B RID: 17723 RVA: 0x00271DD4 File Offset: 0x0026FFD4
		private void OnCombatStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			bool flag = combatChar.GetId() != this.EffectBase.CharacterId || DomainManager.Combat.Pause;
			if (!flag)
			{
				this._addPoisonFrameCounter++;
				bool flag2 = this._addPoisonFrameCounter < this._addPoisonDelayFrame;
				if (!flag2)
				{
					this._addPoisonFrameCounter -= this._addPoisonDelayFrame;
					CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!combatChar.IsAlly, false);
					DomainManager.Combat.AddPoison(context, combatChar, enemyChar, (sbyte)context.Random.Next(6), 3, this._addPoisonValue, this.EffectBase.SkillTemplateId, true, true, default(ItemKey), false, false, false);
					this.EffectBase.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x04001474 RID: 5236
		private const int AddPoisonLevel = 3;

		// Token: 0x04001475 RID: 5237
		private readonly int _addPoisonDelayFrame;

		// Token: 0x04001476 RID: 5238
		private readonly int _addPoisonValue;

		// Token: 0x04001477 RID: 5239
		private int _addPoisonFrameCounter;
	}
}
