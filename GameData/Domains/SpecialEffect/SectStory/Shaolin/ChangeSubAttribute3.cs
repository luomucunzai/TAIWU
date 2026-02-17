using System;
using System.Collections.Generic;

namespace GameData.Domains.SpecialEffect.SectStory.Shaolin
{
	// Token: 0x020000F1 RID: 241
	public class ChangeSubAttribute3 : ChangeSubAttributeBase
	{
		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600297F RID: 10623 RVA: 0x00200F08 File Offset: 0x001FF108
		protected override IReadOnlyList<ushort> SubAttributes { get; } = new ushort[]
		{
			11,
			12
		};

		// Token: 0x06002980 RID: 10624 RVA: 0x00200F10 File Offset: 0x001FF110
		public ChangeSubAttribute3(int charId, IReadOnlyList<int> parameters) : base(charId, parameters)
		{
		}
	}
}
