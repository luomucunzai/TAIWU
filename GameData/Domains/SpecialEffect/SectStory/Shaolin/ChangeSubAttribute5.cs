using System;
using System.Collections.Generic;

namespace GameData.Domains.SpecialEffect.SectStory.Shaolin
{
	// Token: 0x020000F3 RID: 243
	public class ChangeSubAttribute5 : ChangeSubAttributeBase
	{
		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06002983 RID: 10627 RVA: 0x00200F5C File Offset: 0x001FF15C
		protected override IReadOnlyList<ushort> SubAttributes { get; } = new ushort[]
		{
			15,
			16
		};

		// Token: 0x06002984 RID: 10628 RVA: 0x00200F64 File Offset: 0x001FF164
		public ChangeSubAttribute5(int charId, IReadOnlyList<int> parameters) : base(charId, parameters)
		{
		}
	}
}
