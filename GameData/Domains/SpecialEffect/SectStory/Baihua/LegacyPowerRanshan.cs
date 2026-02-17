using System;

namespace GameData.Domains.SpecialEffect.SectStory.Baihua
{
	// Token: 0x020000FE RID: 254
	public class LegacyPowerRanshan : LegacyPower
	{
		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060029AD RID: 10669 RVA: 0x0020143D File Offset: 0x001FF63D
		protected override short CombatStateId
		{
			get
			{
				return 232;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060029AE RID: 10670 RVA: 0x00201444 File Offset: 0x001FF644
		protected override sbyte OrgTemplateId
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x060029AF RID: 10671 RVA: 0x00201447 File Offset: 0x001FF647
		public LegacyPowerRanshan(int charId) : base(charId)
		{
		}
	}
}
