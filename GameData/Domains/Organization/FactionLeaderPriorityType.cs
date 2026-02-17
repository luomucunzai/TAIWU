using System;
using GameData.Domains.Character.Relation;

namespace GameData.Domains.Organization
{
	// Token: 0x02000641 RID: 1601
	public static class FactionLeaderPriorityType
	{
		// Token: 0x0600467E RID: 18046 RVA: 0x0027546C File Offset: 0x0027366C
		public static sbyte GetFactionLeaderPriorityType(ushort relationType)
		{
			bool flag = relationType == 0;
			sbyte result;
			if (flag)
			{
				result = 5;
			}
			else
			{
				bool flag2 = RelationType.ContainParentRelations(relationType);
				if (flag2)
				{
					result = 0;
				}
				else
				{
					bool flag3 = RelationType.HasRelation(relationType, 2048);
					if (flag3)
					{
						result = 1;
					}
					else
					{
						bool flag4 = RelationType.HasRelation(relationType, 1024);
						if (flag4)
						{
							result = 2;
						}
						else
						{
							bool flag5 = RelationType.ContainBrotherOrSisterRelations(relationType);
							if (flag5)
							{
								result = 3;
							}
							else
							{
								bool flag6 = RelationType.HasRelation(relationType, 512);
								if (flag6)
								{
									result = 4;
								}
								else
								{
									result = 5;
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x040014A4 RID: 5284
		public const sbyte Invalid = -1;

		// Token: 0x040014A5 RID: 5285
		public const sbyte Parent = 0;

		// Token: 0x040014A6 RID: 5286
		public const sbyte Mentor = 1;

		// Token: 0x040014A7 RID: 5287
		public const sbyte Spouse = 2;

		// Token: 0x040014A8 RID: 5288
		public const sbyte Sibling = 3;

		// Token: 0x040014A9 RID: 5289
		public const sbyte SwornSibling = 4;

		// Token: 0x040014AA RID: 5290
		public const sbyte SameGradeGeneral = 5;
	}
}
