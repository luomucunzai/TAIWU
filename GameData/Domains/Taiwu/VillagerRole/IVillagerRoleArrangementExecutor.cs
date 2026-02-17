using System;
using GameData.Common;
using GameData.Domains.Character.Ai.PrioritizedAction;

namespace GameData.Domains.Taiwu.VillagerRole
{
	// Token: 0x02000047 RID: 71
	public interface IVillagerRoleArrangementExecutor : IVillagerRoleSelectLocation
	{
		// Token: 0x06001370 RID: 4976
		void ExecuteArrangementAction(DataContext context, VillagerRoleArrangementAction action);
	}
}
