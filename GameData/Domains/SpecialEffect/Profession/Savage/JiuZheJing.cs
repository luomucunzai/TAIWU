using System;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Profession.Savage
{
	// Token: 0x0200010B RID: 267
	public class JiuZheJing : ProfessionEffectBase
	{
		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060029D9 RID: 10713 RVA: 0x0020168F File Offset: 0x001FF88F
		protected override short CombatStateId
		{
			get
			{
				return 133;
			}
		}

		// Token: 0x060029DA RID: 10714 RVA: 0x00201696 File Offset: 0x001FF896
		public JiuZheJing()
		{
		}

		// Token: 0x060029DB RID: 10715 RVA: 0x002016A0 File Offset: 0x001FF8A0
		public JiuZheJing(int charId) : base(charId)
		{
		}

		// Token: 0x060029DC RID: 10716 RVA: 0x002016AB File Offset: 0x001FF8AB
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(107, EDataModifyType.AddPercent, -1);
		}

		// Token: 0x060029DD RID: 10717 RVA: 0x002016C4 File Offset: 0x001FF8C4
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
				bool flag2 = dataKey.FieldId == 107;
				if (flag2)
				{
					result = -20;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000CD8 RID: 3288
		private const sbyte AddPercent = -20;
	}
}
