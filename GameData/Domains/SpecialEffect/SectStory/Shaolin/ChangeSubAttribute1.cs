using System;
using System.Collections.Generic;

namespace GameData.Domains.SpecialEffect.SectStory.Shaolin
{
	// Token: 0x020000EF RID: 239
	public class ChangeSubAttribute1 : ChangeSubAttributeBase
	{
		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600297B RID: 10619 RVA: 0x00200EB6 File Offset: 0x001FF0B6
		protected override IReadOnlyList<ushort> SubAttributes { get; } = new ushort[]
		{
			8,
			7
		};

		// Token: 0x0600297C RID: 10620 RVA: 0x00200EBE File Offset: 0x001FF0BE
		public ChangeSubAttribute1(int charId, IReadOnlyList<int> parameters) : base(charId, parameters)
		{
		}
	}
}
