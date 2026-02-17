using System;
using GameData.Common;

namespace GameData.Domains.SpecialEffect
{
	// Token: 0x020000DF RID: 223
	public interface IFrameCounterHandler
	{
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06002890 RID: 10384
		int CharacterId { get; }

		// Token: 0x06002891 RID: 10385 RVA: 0x001EFF4C File Offset: 0x001EE14C
		bool IsOn(int counterType)
		{
			return true;
		}

		// Token: 0x06002892 RID: 10386
		void OnProcess(DataContext context, int counterType);
	}
}
