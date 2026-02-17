using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong
{
	// Token: 0x02000571 RID: 1393
	public abstract class ChangeFiveElementsDirection : CombatSkillEffectBase
	{
		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06004116 RID: 16662
		protected abstract sbyte FiveElementsType { get; }

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06004117 RID: 16663
		protected abstract byte NeiliAllocationType { get; }

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06004118 RID: 16664 RVA: 0x00261676 File Offset: 0x0025F876
		private CostNeiliAllocationImplement NewCostNeiliAllocationImplement
		{
			get
			{
				return new CostNeiliAllocationImplement(CostNeiliAllocationImplement.EType.AddRange, CostNeiliAllocationImplement.EType.DamageCannotReduce, this.FiveElementsType, this.NeiliAllocationType);
			}
		}

		// Token: 0x06004119 RID: 16665 RVA: 0x0026168B File Offset: 0x0025F88B
		protected ChangeFiveElementsDirection()
		{
			this._costNeiliAllocationImplement = this.NewCostNeiliAllocationImplement;
			this._costNeiliAllocationImplement.EffectBase = this;
		}

		// Token: 0x0600411A RID: 16666 RVA: 0x002616AE File Offset: 0x0025F8AE
		protected ChangeFiveElementsDirection(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
			this._costNeiliAllocationImplement = this.NewCostNeiliAllocationImplement;
			this._costNeiliAllocationImplement.EffectBase = this;
		}

		// Token: 0x0600411B RID: 16667 RVA: 0x002616D4 File Offset: 0x0025F8D4
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 240, -1, -1, -1, -1), EDataModifyType.Custom);
			this._costNeiliAllocationImplement.OnEnable(context);
			this._costNeiliAllocationImplement.AutoAffectedData();
		}

		// Token: 0x0600411C RID: 16668 RVA: 0x00261727 File Offset: 0x0025F927
		public override void OnDisable(DataContext context)
		{
			this._costNeiliAllocationImplement.OnDisable(context);
		}

		// Token: 0x0600411D RID: 16669 RVA: 0x00261738 File Offset: 0x0025F938
		public override List<CastBoostEffectDisplayData> GetModifiedValue(AffectedDataKey dataKey, List<CastBoostEffectDisplayData> dataValue)
		{
			return this._costNeiliAllocationImplement.GetModifiedValue(dataKey, dataValue);
		}

		// Token: 0x0600411E RID: 16670 RVA: 0x00261758 File Offset: 0x0025F958
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			return this._costNeiliAllocationImplement.GetModifyValue(dataKey, currModifyValue);
		}

		// Token: 0x0600411F RID: 16671 RVA: 0x00261778 File Offset: 0x0025F978
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			return this._costNeiliAllocationImplement.GetModifiedValue(dataKey, dataValue);
		}

		// Token: 0x06004120 RID: 16672 RVA: 0x00261798 File Offset: 0x0025F998
		public override BoolArray8 GetModifiedValue(AffectedDataKey dataKey, BoolArray8 dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			BoolArray8 result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.CustomParam0 == 5;
				if (flag2)
				{
					dataValue[(int)this.FiveElementsType] = true;
				}
				result = dataValue;
			}
			return result;
		}

		// Token: 0x06004121 RID: 16673 RVA: 0x002617E0 File Offset: 0x0025F9E0
		protected override int GetSubClassSerializedSize()
		{
			return base.GetSubClassSerializedSize() + 1 + 1;
		}

		// Token: 0x06004122 RID: 16674 RVA: 0x002617FC File Offset: 0x0025F9FC
		protected unsafe override int SerializeSubClass(byte* pData)
		{
			base.SerializeSubClass(pData);
			return this.GetSubClassSerializedSize();
		}

		// Token: 0x06004123 RID: 16675 RVA: 0x0026181C File Offset: 0x0025FA1C
		protected unsafe override int DeserializeSubClass(byte* pData)
		{
			base.DeserializeSubClass(pData);
			return this.GetSubClassSerializedSize();
		}

		// Token: 0x04001327 RID: 4903
		private readonly CostNeiliAllocationImplement _costNeiliAllocationImplement;
	}
}
