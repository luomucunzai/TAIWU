using System;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Profession.Savage
{
	// Token: 0x0200010A RID: 266
	public class CangMangYe : ProfessionEffectBase
	{
		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060029D4 RID: 10708 RVA: 0x0020161D File Offset: 0x001FF81D
		protected override short CombatStateId
		{
			get
			{
				return 134;
			}
		}

		// Token: 0x060029D5 RID: 10709 RVA: 0x00201624 File Offset: 0x001FF824
		public CangMangYe()
		{
		}

		// Token: 0x060029D6 RID: 10710 RVA: 0x0020162E File Offset: 0x001FF82E
		public CangMangYe(int charId) : base(charId)
		{
		}

		// Token: 0x060029D7 RID: 10711 RVA: 0x00201639 File Offset: 0x001FF839
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(74, EDataModifyType.AddPercent, -1);
		}

		// Token: 0x060029D8 RID: 10712 RVA: 0x00201650 File Offset: 0x001FF850
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 74;
				if (flag2)
				{
					result = 20;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000CD7 RID: 3287
		private const sbyte AddPercent = 20;
	}
}
