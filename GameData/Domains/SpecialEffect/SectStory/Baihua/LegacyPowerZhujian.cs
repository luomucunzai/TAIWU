using System;

namespace GameData.Domains.SpecialEffect.SectStory.Baihua
{
	// Token: 0x02000100 RID: 256
	public class LegacyPowerZhujian : LegacyPower
	{
		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060029B3 RID: 10675 RVA: 0x00201467 File Offset: 0x001FF667
		protected override short CombatStateId
		{
			get
			{
				return 234;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060029B4 RID: 10676 RVA: 0x0020146E File Offset: 0x001FF66E
		protected override sbyte OrgTemplateId
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x060029B5 RID: 10677 RVA: 0x00201472 File Offset: 0x001FF672
		public LegacyPowerZhujian(int charId) : base(charId)
		{
		}
	}
}
