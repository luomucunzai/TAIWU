using System;
using System.Collections.Generic;

namespace GameData.Domains.SpecialEffect.SectStory.Shaolin
{
	// Token: 0x020000F0 RID: 240
	public class ChangeSubAttribute2 : ChangeSubAttributeBase
	{
		// Token: 0x170000ED RID: 237
		// (get) Token: 0x0600297D RID: 10621 RVA: 0x00200EDE File Offset: 0x001FF0DE
		protected override IReadOnlyList<ushort> SubAttributes { get; } = new ushort[]
		{
			9,
			10
		};

		// Token: 0x0600297E RID: 10622 RVA: 0x00200EE6 File Offset: 0x001FF0E6
		public ChangeSubAttribute2(int charId, IReadOnlyList<int> parameters) : base(charId, parameters)
		{
		}
	}
}
