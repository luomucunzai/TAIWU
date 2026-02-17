using System;
using System.Collections.Generic;

namespace GameData.Domains.SpecialEffect.SectStory.Shaolin
{
	// Token: 0x020000F2 RID: 242
	public class ChangeSubAttribute4 : ChangeSubAttributeBase
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06002981 RID: 10625 RVA: 0x00200F32 File Offset: 0x001FF132
		protected override IReadOnlyList<ushort> SubAttributes { get; } = new ushort[]
		{
			14,
			13
		};

		// Token: 0x06002982 RID: 10626 RVA: 0x00200F3A File Offset: 0x001FF13A
		public ChangeSubAttribute4(int charId, IReadOnlyList<int> parameters) : base(charId, parameters)
		{
		}
	}
}
