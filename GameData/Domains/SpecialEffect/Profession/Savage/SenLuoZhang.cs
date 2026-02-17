using System;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Profession.Savage
{
	// Token: 0x02000110 RID: 272
	public class SenLuoZhang : ProfessionEffectBase
	{
		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060029ED RID: 10733 RVA: 0x0020188A File Offset: 0x001FFA8A
		protected override short CombatStateId
		{
			get
			{
				return 138;
			}
		}

		// Token: 0x060029EE RID: 10734 RVA: 0x00201891 File Offset: 0x001FFA91
		public SenLuoZhang()
		{
		}

		// Token: 0x060029EF RID: 10735 RVA: 0x0020189B File Offset: 0x001FFA9B
		public SenLuoZhang(int charId) : base(charId)
		{
		}

		// Token: 0x060029F0 RID: 10736 RVA: 0x002018A6 File Offset: 0x001FFAA6
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(198, EDataModifyType.AddPercent, -1);
		}

		// Token: 0x060029F1 RID: 10737 RVA: 0x002018C0 File Offset: 0x001FFAC0
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
				bool flag2 = dataKey.FieldId == 198;
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

		// Token: 0x04000CDE RID: 3294
		private const sbyte AddPercent = 20;
	}
}
