using System;
using GameData.Domains.Building;

namespace GameData.Domains.World
{
	// Token: 0x0200002A RID: 42
	public sealed class TaskConditionBuildingBlockArg : TaskConditionCheckArgument
	{
		// Token: 0x040000CD RID: 205
		public BuildingBlockKey BuildingBlockKey;

		// Token: 0x040000CE RID: 206
		public ETaskConditionType Type;
	}
}
