using System;

namespace GameData.Domains.World
{
	// Token: 0x02000028 RID: 40
	public sealed class TaskConditionRelationArg : TaskConditionCheckArgument
	{
		// Token: 0x040000C8 RID: 200
		public int CharacterId;

		// Token: 0x040000C9 RID: 201
		public ushort RelationType;

		// Token: 0x040000CA RID: 202
		public ETaskConditionType Type;
	}
}
