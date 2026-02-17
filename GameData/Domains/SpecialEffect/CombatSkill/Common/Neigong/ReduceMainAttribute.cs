using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong
{
	// Token: 0x02000579 RID: 1401
	public abstract class ReduceMainAttribute : CombatSkillEffectBase
	{
		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06004167 RID: 16743
		protected abstract bool IsAffect { get; }

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06004168 RID: 16744
		protected abstract sbyte MainAttributeType { get; }

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06004169 RID: 16745 RVA: 0x00262B9E File Offset: 0x00260D9E
		protected short CurrMainAttribute
		{
			get
			{
				return this.CharObj.GetCurrMainAttribute(this.MainAttributeType);
			}
		}

		// Token: 0x0600416A RID: 16746 RVA: 0x00262BB1 File Offset: 0x00260DB1
		protected ReduceMainAttribute()
		{
		}

		// Token: 0x0600416B RID: 16747 RVA: 0x00262BBB File Offset: 0x00260DBB
		protected ReduceMainAttribute(CombatSkillKey skillKey, int type = -1) : base(skillKey, type, -1)
		{
		}

		// Token: 0x0600416C RID: 16748 RVA: 0x00262BC8 File Offset: 0x00260DC8
		protected override IEnumerable<int> CalcFrameCounterPeriods()
		{
			yield return 120;
			yield break;
		}

		// Token: 0x0600416D RID: 16749 RVA: 0x00262BD8 File Offset: 0x00260DD8
		public override bool IsOn(int counterType)
		{
			return this.IsAffect;
		}

		// Token: 0x0600416E RID: 16750 RVA: 0x00262BE0 File Offset: 0x00260DE0
		public override void OnProcess(DataContext context, int counterType)
		{
			bool flag = base.CombatChar.GetCharacter().GetCurrMainAttribute(this.MainAttributeType) <= 0;
			if (!flag)
			{
				base.CombatChar.GetCharacter().ChangeCurrMainAttribute(context, this.MainAttributeType, -3);
			}
		}

		// Token: 0x0400134B RID: 4939
		private const int ReduceFrame = 120;

		// Token: 0x0400134C RID: 4940
		private const int ReduceValue = -3;
	}
}
