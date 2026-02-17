using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x020005E8 RID: 1512
	public abstract class LoongBase : AnimalEffectBase
	{
		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x0600448B RID: 17547
		protected abstract IEnumerable<ISpecialEffectImplement> Implements { get; }

		// Token: 0x0600448C RID: 17548 RVA: 0x0026FD92 File Offset: 0x0026DF92
		protected LoongBase()
		{
			this.CreateImplements();
		}

		// Token: 0x0600448D RID: 17549 RVA: 0x0026FDA3 File Offset: 0x0026DFA3
		protected LoongBase(CombatSkillKey skillKey) : base(skillKey)
		{
			this.CreateImplements();
		}

		// Token: 0x0600448E RID: 17550 RVA: 0x0026FDB8 File Offset: 0x0026DFB8
		private void CreateImplements()
		{
			this._implements = new List<ISpecialEffectImplement>(this.Implements);
			foreach (ISpecialEffectImplement implement in this._implements)
			{
				implement.EffectBase = this;
			}
		}

		// Token: 0x0600448F RID: 17551 RVA: 0x0026FE20 File Offset: 0x0026E020
		public override void OnEnable(DataContext context)
		{
			foreach (ISpecialEffectImplement implement in this._implements)
			{
				implement.OnEnable(context);
			}
		}

		// Token: 0x06004490 RID: 17552 RVA: 0x0026FE78 File Offset: 0x0026E078
		public override void OnDisable(DataContext context)
		{
			foreach (ISpecialEffectImplement implement in this._implements)
			{
				implement.OnDisable(context);
			}
		}

		// Token: 0x06004491 RID: 17553 RVA: 0x0026FED0 File Offset: 0x0026E0D0
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			return this._implements.Sum((ISpecialEffectImplement implement) => implement.GetModifyValue(dataKey, currModifyValue));
		}

		// Token: 0x06004492 RID: 17554 RVA: 0x0026FF10 File Offset: 0x0026E110
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			dataValue = base.GetModifiedValue(dataKey, dataValue);
			return this._implements.Aggregate(dataValue, (bool current, ISpecialEffectImplement implement) => implement.GetModifiedValue(dataKey, current));
		}

		// Token: 0x06004493 RID: 17555 RVA: 0x0026FF58 File Offset: 0x0026E158
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			return this._implements.Aggregate(dataValue, (int current, ISpecialEffectImplement implement) => implement.GetModifiedValue(dataKey, current));
		}

		// Token: 0x0400144B RID: 5195
		private List<ISpecialEffectImplement> _implements;
	}
}
