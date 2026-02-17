using System;

namespace GameData.Domains.Building
{
	// Token: 0x020008C0 RID: 2240
	public interface IBuildingEffectValue
	{
		// Token: 0x06007E73 RID: 32371
		void Change(int delta);

		// Token: 0x06007E74 RID: 32372
		void Change(int index, int delta);

		// Token: 0x06007E75 RID: 32373
		int Get();

		// Token: 0x06007E76 RID: 32374
		int Get(int index);

		// Token: 0x06007E77 RID: 32375
		void Clear();
	}
}
