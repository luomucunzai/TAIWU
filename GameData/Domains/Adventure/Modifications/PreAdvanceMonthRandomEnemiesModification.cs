using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace GameData.Domains.Adventure.Modifications
{
	// Token: 0x020008C8 RID: 2248
	public class PreAdvanceMonthRandomEnemiesModification
	{
		// Token: 0x040022F1 RID: 8945
		public short AreaId;

		// Token: 0x040022F2 RID: 8946
		[TupleElementNames(new string[]
		{
			"charId",
			"enemyInfo"
		})]
		public List<ValueTuple<int, MapTemplateEnemyInfo>> RandomEnemyAttackRecords = new List<ValueTuple<int, MapTemplateEnemyInfo>>();

		// Token: 0x040022F3 RID: 8947
		[TupleElementNames(new string[]
		{
			"charId",
			"animal"
		})]
		public List<ValueTuple<int, int>> AnimalAttackRecords = new List<ValueTuple<int, int>>();
	}
}
