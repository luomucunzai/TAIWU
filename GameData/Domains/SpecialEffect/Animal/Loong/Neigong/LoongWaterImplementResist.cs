using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.SpecialEffect.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x02000607 RID: 1543
	public class LoongWaterImplementResist : ISpecialEffectImplement, ISpecialEffectModifier
	{
		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x0600453C RID: 17724 RVA: 0x00271E9D File Offset: 0x0027009D
		// (set) Token: 0x0600453D RID: 17725 RVA: 0x00271EA5 File Offset: 0x002700A5
		public CombatSkillEffectBase EffectBase { get; set; }

		// Token: 0x0600453E RID: 17726 RVA: 0x00271EAE File Offset: 0x002700AE
		public void OnEnable(DataContext context)
		{
			this.EffectBase.CreateAffectedAllEnemyData(245, EDataModifyType.AddPercent, -1);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x0600453F RID: 17727 RVA: 0x00271ED6 File Offset: 0x002700D6
		public void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x06004540 RID: 17728 RVA: 0x00271EEC File Offset: 0x002700EC
		private void OnCombatBegin(DataContext context)
		{
			bool flag = DomainManager.Combat.IsMainCharacter(this.EffectBase.CombatChar);
			if (flag)
			{
				this.EffectBase.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x06004541 RID: 17729 RVA: 0x00271F20 File Offset: 0x00270120
		public int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId == this.EffectBase.CharacterId || dataKey.FieldId != 245;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = ((!DomainManager.Combat.IsMainCharacter(this.EffectBase.CombatChar)) ? 0 : -50);
			}
			return result;
		}

		// Token: 0x04001479 RID: 5241
		private const int ReducePoisonResist = -50;
	}
}
