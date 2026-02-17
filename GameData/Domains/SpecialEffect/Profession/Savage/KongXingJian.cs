using System;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Profession.Savage
{
	// Token: 0x0200010C RID: 268
	public class KongXingJian : ProfessionEffectBase
	{
		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060029DE RID: 10718 RVA: 0x00201703 File Offset: 0x001FF903
		protected override short CombatStateId
		{
			get
			{
				return 136;
			}
		}

		// Token: 0x060029DF RID: 10719 RVA: 0x0020170A File Offset: 0x001FF90A
		public KongXingJian()
		{
		}

		// Token: 0x060029E0 RID: 10720 RVA: 0x00201714 File Offset: 0x001FF914
		public KongXingJian(int charId) : base(charId)
		{
		}

		// Token: 0x060029E1 RID: 10721 RVA: 0x0020171F File Offset: 0x001FF91F
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(197, EDataModifyType.AddPercent, -1);
		}

		// Token: 0x060029E2 RID: 10722 RVA: 0x00201738 File Offset: 0x001FF938
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
				bool flag2 = dataKey.FieldId == 197;
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

		// Token: 0x04000CD9 RID: 3289
		private const sbyte AddPercent = 20;
	}
}
