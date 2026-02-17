using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Implement
{
	// Token: 0x02000580 RID: 1408
	public class CostNeiliAllocationImplement : ISpecialEffectImplement, ISpecialEffectModifier
	{
		// Token: 0x17000280 RID: 640
		// (get) Token: 0x060041B1 RID: 16817 RVA: 0x002639C2 File Offset: 0x00261BC2
		private CostNeiliAllocationImplement.EType Type
		{
			get
			{
				return this.EffectBase.IsDirect ? this._directType : this._reverseType;
			}
		}

		// Token: 0x060041B2 RID: 16818 RVA: 0x002639DF File Offset: 0x00261BDF
		public CostNeiliAllocationImplement(CostNeiliAllocationImplement.EType directType, CostNeiliAllocationImplement.EType reverseType, sbyte fiveElementsType, byte costType)
		{
			this._directType = directType;
			this._reverseType = reverseType;
			this._fiveElementsType = fiveElementsType;
			this._costType = costType;
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x060041B3 RID: 16819 RVA: 0x00263A0D File Offset: 0x00261C0D
		// (set) Token: 0x060041B4 RID: 16820 RVA: 0x00263A15 File Offset: 0x00261C15
		public CombatSkillEffectBase EffectBase { get; set; }

		// Token: 0x060041B5 RID: 16821 RVA: 0x00263A20 File Offset: 0x00261C20
		public void OnEnable(DataContext context)
		{
			CombatSkillEffectBase effectBase = this.EffectBase;
			if (effectBase.AffectDatas == null)
			{
				effectBase.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			}
			this.EffectBase.AffectDatas.Add(new AffectedDataKey(this.EffectBase.CharacterId, 235, -1, -1, -1, -1), EDataModifyType.Custom);
			Events.RegisterHandler_CombatCostNeiliConfirm(new Events.OnCombatCostNeiliConfirm(this.CombatCostNeiliConfirm));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.CastSkillEnd));
		}

		// Token: 0x060041B6 RID: 16822 RVA: 0x00263A97 File Offset: 0x00261C97
		public void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatCostNeiliConfirm(new Events.OnCombatCostNeiliConfirm(this.CombatCostNeiliConfirm));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.CastSkillEnd));
		}

		// Token: 0x060041B7 RID: 16823 RVA: 0x00263AC0 File Offset: 0x00261CC0
		public int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != this.EffectBase.CharacterId || dataKey.CombatSkillId != this._costNeiliEffectingSkillId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId - 145 <= 1;
				bool flag3 = flag2 && this.Type == CostNeiliAllocationImplement.EType.AddRange;
				if (flag3)
				{
					result = this._costNeiliEffectingAddRange;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x060041B8 RID: 16824 RVA: 0x00263B3C File Offset: 0x00261D3C
		public bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != this.EffectBase.CharacterId || dataKey.CombatSkillId != this._costNeiliEffectingSkillId;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 327 && this.Type == CostNeiliAllocationImplement.EType.DamageCannotReduce && dataKey.CustomParam2 == 1;
				result = (!flag2 && dataValue);
			}
			return result;
		}

		// Token: 0x060041B9 RID: 16825 RVA: 0x00263BA8 File Offset: 0x00261DA8
		public List<CastBoostEffectDisplayData> GetModifiedValue(AffectedDataKey dataKey, List<CastBoostEffectDisplayData> dataValue)
		{
			bool flag = dataKey.CharId != this.EffectBase.CharacterId;
			List<CastBoostEffectDisplayData> result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 235 && dataKey.CombatSkillId >= 0 && this.EffectBase.FiveElementsEquals(dataKey, this._fiveElementsType) && this._costNeiliEffectingSkillId < 0 && this.CheckAttackIfNeed(dataKey.CombatSkillId);
				if (flag2)
				{
					dataValue.Add(this.EffectBase.SkillKey.GetPureCostNeiliEffectData(this._costType, dataKey.CombatSkillId, true));
				}
				result = dataValue;
			}
			return result;
		}

		// Token: 0x060041BA RID: 16826 RVA: 0x00263C44 File Offset: 0x00261E44
		public void AutoAffectedData()
		{
			bool flag = this.ContainsType(CostNeiliAllocationImplement.EType.DamageCannotReduce);
			if (flag)
			{
				this.EffectBase.CreateAffectedData(327, EDataModifyType.Custom, -1);
			}
			bool flag2 = this.ContainsType(CostNeiliAllocationImplement.EType.AddRange);
			if (flag2)
			{
				this.EffectBase.CreateAffectedData(145, EDataModifyType.Add, -1);
				this.EffectBase.CreateAffectedData(146, EDataModifyType.Add, -1);
			}
		}

		// Token: 0x060041BB RID: 16827 RVA: 0x00263CA3 File Offset: 0x00261EA3
		private bool ContainsType(CostNeiliAllocationImplement.EType type)
		{
			return this._directType == type || this._reverseType == type;
		}

		// Token: 0x060041BC RID: 16828 RVA: 0x00263CBC File Offset: 0x00261EBC
		private bool CheckAttackIfNeed(short skillId)
		{
			CostNeiliAllocationImplement.EType type = this.Type;
			bool flag = type <= CostNeiliAllocationImplement.EType.DamageCannotReduce;
			bool flag2 = flag;
			return !flag2 || CombatSkill.Instance[skillId].EquipType == 1;
		}

		// Token: 0x060041BD RID: 16829 RVA: 0x00263D00 File Offset: 0x00261F00
		private unsafe void CombatCostNeiliConfirm(DataContext context, int charId, short skillId, short effectId)
		{
			bool flag = charId != this.EffectBase.CharacterId || (int)effectId != this.EffectBase.EffectId || this._costNeiliEffectingSkillId >= 0;
			if (!flag)
			{
				bool flag2 = skillId < 0 || this.EffectBase.CombatChar.GetPreparingSkillId() != skillId || !this.EffectBase.FiveElementsEquals(skillId, this._fiveElementsType);
				if (!flag2)
				{
					CastBoostEffectDisplayData costNeiliEffect = this.EffectBase.SkillKey.GetPureCostNeiliEffectData(this._costType, skillId, false);
					bool flag3 = costNeiliEffect.NeiliAllocationType > 3;
					if (!flag3)
					{
						bool flag4 = (int)(*(ref this.EffectBase.CombatChar.GetNeiliAllocation().Items.FixedElementField + (IntPtr)costNeiliEffect.NeiliAllocationType * 2)) < -costNeiliEffect.NeiliAllocationValue;
						if (!flag4)
						{
							DomainManager.Combat.ShowSpecialEffectTips(this.EffectBase.CharacterId, this.EffectBase.EffectId, 0);
							this._costNeiliEffectingSkillId = skillId;
							this.EffectBase.CombatChar.ChangeNeiliAllocation(context, costNeiliEffect.NeiliAllocationType, costNeiliEffect.NeiliAllocationValue, true, true);
							this.ApplyEffect(context);
						}
					}
				}
			}
		}

		// Token: 0x060041BE RID: 16830 RVA: 0x00263E38 File Offset: 0x00262038
		private void CastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != this.EffectBase.CharacterId || skillId != this._costNeiliEffectingSkillId;
			if (!flag)
			{
				this._costNeiliEffectingSkillId = -1;
				this.RevertEffect(context);
			}
		}

		// Token: 0x060041BF RID: 16831 RVA: 0x00263E7C File Offset: 0x0026207C
		private void ApplyEffect(DataContext context)
		{
			CostNeiliAllocationImplement.EType type = this.Type;
			CostNeiliAllocationImplement.EType etype = type;
			if (etype != CostNeiliAllocationImplement.EType.AddRange)
			{
				if (etype != CostNeiliAllocationImplement.EType.DamageCannotReduce)
				{
					short predefinedLogId = 7;
					object arg = this.EffectBase.EffectId;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
					defaultInterpolatedStringHandler.AppendFormatted<CostNeiliAllocationImplement.EType>(this.Type);
					defaultInterpolatedStringHandler.AppendLiteral(" not exist cost neili allocation effect to apply.");
					PredefinedLog.Show(predefinedLogId, arg, defaultInterpolatedStringHandler.ToStringAndClear());
				}
				else
				{
					DomainManager.SpecialEffect.InvalidateCache(context, this.EffectBase.CharacterId, 327);
				}
			}
			else
			{
				this._costNeiliEffectingAddRange = (int)(5 + 5 * this.EffectBase.CombatChar.GetCharacter().GetCombatSkillGridCost(this._costNeiliEffectingSkillId));
				DomainManager.SpecialEffect.InvalidateCache(context, this.EffectBase.CharacterId, 145);
				DomainManager.SpecialEffect.InvalidateCache(context, this.EffectBase.CharacterId, 146);
			}
		}

		// Token: 0x060041C0 RID: 16832 RVA: 0x00263F60 File Offset: 0x00262160
		private void RevertEffect(DataContext context)
		{
			CostNeiliAllocationImplement.EType type = this.Type;
			CostNeiliAllocationImplement.EType etype = type;
			if (etype != CostNeiliAllocationImplement.EType.AddRange)
			{
				if (etype != CostNeiliAllocationImplement.EType.DamageCannotReduce)
				{
					short predefinedLogId = 7;
					object arg = this.EffectBase.EffectId;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(50, 1);
					defaultInterpolatedStringHandler.AppendFormatted<CostNeiliAllocationImplement.EType>(this.Type);
					defaultInterpolatedStringHandler.AppendLiteral(" not exist cost neili allocation effect to revert.");
					PredefinedLog.Show(predefinedLogId, arg, defaultInterpolatedStringHandler.ToStringAndClear());
				}
				else
				{
					DomainManager.SpecialEffect.InvalidateCache(context, this.EffectBase.CharacterId, 327);
				}
			}
			else
			{
				this._costNeiliEffectingAddRange = 0;
				DomainManager.SpecialEffect.InvalidateCache(context, this.EffectBase.CharacterId, 145);
				DomainManager.SpecialEffect.InvalidateCache(context, this.EffectBase.CharacterId, 146);
			}
		}

		// Token: 0x04001359 RID: 4953
		private const int AddRangeBaseValue = 5;

		// Token: 0x0400135A RID: 4954
		private const int AddRangeValuePerGrid = 5;

		// Token: 0x0400135B RID: 4955
		private readonly CostNeiliAllocationImplement.EType _directType;

		// Token: 0x0400135C RID: 4956
		private readonly CostNeiliAllocationImplement.EType _reverseType;

		// Token: 0x0400135D RID: 4957
		private readonly sbyte _fiveElementsType;

		// Token: 0x0400135E RID: 4958
		private readonly byte _costType;

		// Token: 0x0400135F RID: 4959
		private short _costNeiliEffectingSkillId = -1;

		// Token: 0x04001360 RID: 4960
		private int _costNeiliEffectingAddRange;

		// Token: 0x02000A5A RID: 2650
		public enum EType
		{
			// Token: 0x04002A6D RID: 10861
			AddRange,
			// Token: 0x04002A6E RID: 10862
			DamageCannotReduce
		}
	}
}
