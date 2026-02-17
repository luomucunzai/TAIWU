using System;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Profession.Savage
{
	// Token: 0x0200010D RID: 269
	public class LianShanCui : ProfessionEffectBase
	{
		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060029E3 RID: 10723 RVA: 0x0020177A File Offset: 0x001FF97A
		protected override short CombatStateId
		{
			get
			{
				return 135;
			}
		}

		// Token: 0x060029E4 RID: 10724 RVA: 0x00201781 File Offset: 0x001FF981
		public LianShanCui()
		{
		}

		// Token: 0x060029E5 RID: 10725 RVA: 0x0020178B File Offset: 0x001FF98B
		public LianShanCui(int charId) : base(charId)
		{
		}

		// Token: 0x060029E6 RID: 10726 RVA: 0x00201796 File Offset: 0x001FF996
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(195, EDataModifyType.AddPercent, -1);
			base.CreateAffectedData(196, EDataModifyType.AddPercent, -1);
		}

		// Token: 0x060029E7 RID: 10727 RVA: 0x002017C0 File Offset: 0x001FF9C0
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
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId - 195 <= 1;
				bool flag3 = flag2;
				if (flag3)
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

		// Token: 0x04000CDA RID: 3290
		private const sbyte AddPercent = 20;
	}
}
