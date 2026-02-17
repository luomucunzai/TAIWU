using System;
using GameData.Common;

namespace GameData.Domains.Character.Ai.GeneralAction
{
	// Token: 0x0200086D RID: 2157
	public interface IGeneralAction
	{
		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x06007763 RID: 30563
		sbyte ActionEnergyType { get; }

		// Token: 0x06007764 RID: 30564
		bool CheckValid(Character selfChar, Character targetChar);

		// Token: 0x06007765 RID: 30565
		void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar);

		// Token: 0x06007766 RID: 30566
		void ApplyChanges(DataContext context, Character selfChar, Character targetChar);
	}
}
