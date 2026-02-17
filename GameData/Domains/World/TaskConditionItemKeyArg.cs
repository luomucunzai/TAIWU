using System;
using GameData.Domains.Item;

namespace GameData.Domains.World
{
	// Token: 0x02000029 RID: 41
	public sealed class TaskConditionItemKeyArg : TaskConditionCheckArgument
	{
		// Token: 0x040000CB RID: 203
		public ItemKey ItemKey;

		// Token: 0x040000CC RID: 204
		public ETaskConditionType Type;
	}
}
