using GameData.Common;
using GameData.Domains.Character.Ai.PrioritizedAction;

namespace GameData.Domains.Taiwu.VillagerRole;

public interface IVillagerRoleArrangementExecutor : IVillagerRoleSelectLocation
{
	void ExecuteArrangementAction(DataContext context, VillagerRoleArrangementAction action);
}
