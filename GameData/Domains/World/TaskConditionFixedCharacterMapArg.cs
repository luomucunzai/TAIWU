using System;
using GameData.Domains.Map;

namespace GameData.Domains.World
{
	// Token: 0x02000027 RID: 39
	public sealed class TaskConditionFixedCharacterMapArg : TaskConditionCheckArgument
	{
		// Token: 0x040000C5 RID: 197
		public int CharacterId;

		// Token: 0x040000C6 RID: 198
		public Location Location;

		// Token: 0x040000C7 RID: 199
		public ETaskConditionType Type;
	}
}
