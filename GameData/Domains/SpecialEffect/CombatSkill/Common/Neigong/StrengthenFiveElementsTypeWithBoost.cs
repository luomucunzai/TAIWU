using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong
{
	// Token: 0x0200057D RID: 1405
	public abstract class StrengthenFiveElementsTypeWithBoost : StrengthenFiveElementsType
	{
		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06004191 RID: 16785 RVA: 0x00263146 File Offset: 0x00261346
		protected override int DirectAddPower
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06004192 RID: 16786 RVA: 0x0026314A File Offset: 0x0026134A
		protected override int ReverseReduceCostPercent
		{
			get
			{
				return -20;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06004193 RID: 16787
		protected abstract byte CostNeiliAllocationType { get; }

		// Token: 0x06004194 RID: 16788 RVA: 0x0026314E File Offset: 0x0026134E
		protected StrengthenFiveElementsTypeWithBoost()
		{
		}

		// Token: 0x06004195 RID: 16789 RVA: 0x0026315F File Offset: 0x0026135F
		protected StrengthenFiveElementsTypeWithBoost(CombatSkillKey skillKey, int type) : base(skillKey, type)
		{
		}

		// Token: 0x06004196 RID: 16790 RVA: 0x00263174 File Offset: 0x00261374
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(235, EDataModifyType.Custom, -1);
			Events.RegisterHandler_CombatCostNeiliConfirm(new Events.OnCombatCostNeiliConfirm(this.OnCombatCostNeiliConfirm));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			bool flag = !base.IsDirect;
			if (flag)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, -1, -1, -1, -1), EDataModifyType.Add);
			}
		}

		// Token: 0x06004197 RID: 16791 RVA: 0x002631EA File Offset: 0x002613EA
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatCostNeiliConfirm(new Events.OnCombatCostNeiliConfirm(this.OnCombatCostNeiliConfirm));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			base.OnDisable(context);
		}

		// Token: 0x06004198 RID: 16792 RVA: 0x0026321C File Offset: 0x0026141C
		private unsafe void OnCombatCostNeiliConfirm(DataContext context, int charId, short skillId, short effectId)
		{
			bool flag = charId != base.CharacterId || (int)effectId != base.EffectId || this._affectingSkillId >= 0;
			if (!flag)
			{
				bool flag2 = skillId < 0 || base.CombatChar.GetPreparingSkillId() != skillId || !base.FiveElementsEquals(skillId, this.FiveElementsType);
				if (!flag2)
				{
					CastBoostEffectDisplayData costNeiliEffect = this.SkillKey.GetPureCostNeiliEffectData(this.CostNeiliAllocationType, skillId, false);
					bool flag3 = costNeiliEffect.NeiliAllocationType > 3;
					if (!flag3)
					{
						int requireValue = base.CombatChar.ApplySpecialEffectToNeiliAllocation(costNeiliEffect.NeiliAllocationType, costNeiliEffect.NeiliAllocationValue);
						bool flag4 = (int)(*base.CombatChar.GetNeiliAllocation()[(int)costNeiliEffect.NeiliAllocationType]) < -requireValue;
						if (!flag4)
						{
							base.ShowSpecialEffectTips(0);
							this._affectingSkillId = skillId;
							base.CombatChar.ChangeNeiliAllocation(context, costNeiliEffect.NeiliAllocationType, costNeiliEffect.NeiliAllocationValue, true, true);
							this.ApplyEffect(context);
						}
					}
				}
			}
		}

		// Token: 0x06004199 RID: 16793 RVA: 0x00263320 File Offset: 0x00261520
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != this._affectingSkillId;
			if (!flag)
			{
				this._affectingSkillId = -1;
				this.RevertEffect(context);
			}
		}

		// Token: 0x0600419A RID: 16794 RVA: 0x0026335C File Offset: 0x0026155C
		private void ApplyEffect(DataContext context)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				int addProgress = base.CombatChar.SkillPrepareTotalProgress * 50 / 100;
				int maxProgress = base.CombatChar.SkillPrepareTotalProgress * 75 / 100;
				base.CombatChar.SkillPrepareCurrProgress = Math.Max(base.CombatChar.SkillPrepareCurrProgress, Math.Min(base.CombatChar.SkillPrepareCurrProgress + addProgress, maxProgress));
				base.CombatChar.SetSkillPreparePercent((byte)(base.CombatChar.SkillPrepareCurrProgress * 100 / base.CombatChar.SkillPrepareTotalProgress), context);
			}
			else
			{
				base.InvalidateCache(context, 199);
			}
		}

		// Token: 0x0600419B RID: 16795 RVA: 0x00263404 File Offset: 0x00261604
		private void RevertEffect(DataContext context)
		{
			bool flag = !base.IsDirect;
			if (flag)
			{
				base.InvalidateCache(context, 199);
			}
		}

		// Token: 0x0600419C RID: 16796 RVA: 0x0026342C File Offset: 0x0026162C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			int value = base.GetModifyValue(dataKey, currModifyValue);
			bool flag = dataKey.CharId == base.CharacterId && dataKey.CombatSkillId == this._affectingSkillId && dataKey.FieldId == 199 && !base.IsDirect;
			if (flag)
			{
				value += 20;
			}
			return value;
		}

		// Token: 0x0600419D RID: 16797 RVA: 0x00263488 File Offset: 0x00261688
		public override List<CastBoostEffectDisplayData> GetModifiedValue(AffectedDataKey dataKey, List<CastBoostEffectDisplayData> dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			List<CastBoostEffectDisplayData> result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 235 && dataKey.CombatSkillId >= 0 && base.FiveElementsEquals(dataKey, this.FiveElementsType) && this._affectingSkillId < 0;
				if (flag2)
				{
					dataValue.Add(this.SkillKey.GetPureCostNeiliEffectData(this.CostNeiliAllocationType, dataKey.CombatSkillId, true));
				}
				result = dataValue;
			}
			return result;
		}

		// Token: 0x0400134F RID: 4943
		private const int AddPrepareProgressPercent = 50;

		// Token: 0x04001350 RID: 4944
		private const int AddPrepareProgressMaxPercent = 75;

		// Token: 0x04001351 RID: 4945
		private const sbyte AddPower = 20;

		// Token: 0x04001352 RID: 4946
		private short _affectingSkillId = -1;
	}
}
