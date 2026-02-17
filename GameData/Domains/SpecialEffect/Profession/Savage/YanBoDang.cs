using System;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Profession.Savage
{
	// Token: 0x02000112 RID: 274
	public class YanBoDang : ProfessionEffectBase
	{
		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060029F9 RID: 10745 RVA: 0x002019B0 File Offset: 0x001FFBB0
		protected override short CombatStateId
		{
			get
			{
				return 137;
			}
		}

		// Token: 0x060029FA RID: 10746 RVA: 0x002019B7 File Offset: 0x001FFBB7
		public YanBoDang()
		{
		}

		// Token: 0x060029FB RID: 10747 RVA: 0x002019C1 File Offset: 0x001FFBC1
		public YanBoDang(int charId) : base(charId)
		{
		}

		// Token: 0x060029FC RID: 10748 RVA: 0x002019CC File Offset: 0x001FFBCC
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(152, EDataModifyType.AddPercent, -1);
		}

		// Token: 0x060029FD RID: 10749 RVA: 0x002019E8 File Offset: 0x001FFBE8
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
				bool flag2 = dataKey.FieldId == 152;
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

		// Token: 0x04000CE0 RID: 3296
		private const sbyte AddPercent = 20;
	}
}
