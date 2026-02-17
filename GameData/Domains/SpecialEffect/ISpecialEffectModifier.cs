using System;

namespace GameData.Domains.SpecialEffect
{
	// Token: 0x020000E0 RID: 224
	public interface ISpecialEffectModifier
	{
		// Token: 0x06002893 RID: 10387 RVA: 0x001EFF4F File Offset: 0x001EE14F
		int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			return 0;
		}

		// Token: 0x06002894 RID: 10388 RVA: 0x001EFF52 File Offset: 0x001EE152
		bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			return dataValue;
		}

		// Token: 0x06002895 RID: 10389 RVA: 0x001EFF55 File Offset: 0x001EE155
		int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			return dataValue;
		}
	}
}
