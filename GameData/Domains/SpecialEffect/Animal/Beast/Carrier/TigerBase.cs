using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier
{
	// Token: 0x0200063E RID: 1598
	public abstract class TigerBase : CarrierEffectBase
	{
		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06004655 RID: 18005
		protected abstract int AddDamagePercentUnit { get; }

		// Token: 0x06004656 RID: 18006 RVA: 0x002744DB File Offset: 0x002726DB
		protected TigerBase()
		{
		}

		// Token: 0x06004657 RID: 18007 RVA: 0x002744E5 File Offset: 0x002726E5
		protected TigerBase(int charId) : base(charId)
		{
		}

		// Token: 0x06004658 RID: 18008 RVA: 0x002744F0 File Offset: 0x002726F0
		protected override void OnEnableSubClass(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, -1, -1, -1, -1), EDataModifyType.AddPercent);
		}

		// Token: 0x06004659 RID: 18009 RVA: 0x0027451C File Offset: 0x0027271C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId >= 0 || dataKey.FieldId != 69;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = (int)base.CombatChar.PursueAttackCount * this.AddDamagePercentUnit;
			}
			return result;
		}
	}
}
