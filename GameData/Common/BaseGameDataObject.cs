using System;
using GameData.Dependencies;
using GameData.Domains;
using GameData.Utilities;

namespace GameData.Common
{
	// Token: 0x020008EE RID: 2286
	public abstract class BaseGameDataObject
	{
		// Token: 0x06008214 RID: 33300 RVA: 0x004D979E File Offset: 0x004D799E
		protected BaseGameDataObject()
		{
			this.DataStatesOffset = -1;
		}

		// Token: 0x06008215 RID: 33301 RVA: 0x004D97B0 File Offset: 0x004D79B0
		protected void SetModifiedAndInvalidateInfluencedCache(ushort fieldId, DataContext context)
		{
			this.CollectionHelperData.DataStates.SetModified(this.DataStatesOffset, (int)fieldId);
			DataInfluence[] influences = this.CollectionHelperData.CacheInfluences[(int)fieldId];
			bool flag = influences == null;
			if (!flag)
			{
				Tester.Assert(context != null, "");
				int influencesCount = influences.Length;
				for (int i = 0; i < influencesCount; i++)
				{
					DataInfluence influence = influences[i];
					bool flag2 = InfluenceChecker.CheckCondition(context, this, influence.Condition);
					if (flag2)
					{
						BaseGameDataDomain domain = DomainManager.Domains[(int)influence.TargetIndicator.DomainId];
						domain.InvalidateCache(this, influence, context, false);
					}
				}
			}
		}

		// Token: 0x06008216 RID: 33302 RVA: 0x004D9854 File Offset: 0x004D7A54
		public void InvalidateSelfAndInfluencedCache(ushort fieldId, DataContext context)
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			dataStates.SetModified(this.DataStatesOffset, (int)fieldId);
			bool flag = !dataStates.IsCached(this.DataStatesOffset, (int)fieldId);
			if (!flag)
			{
				dataStates.ResetCached(this.DataStatesOffset, (int)fieldId);
				DataInfluence[] influences = this.CollectionHelperData.CacheInfluences[(int)fieldId];
				bool flag2 = influences == null;
				if (!flag2)
				{
					Tester.Assert(context != null, "");
					int influencesCount = influences.Length;
					for (int i = 0; i < influencesCount; i++)
					{
						DataInfluence influence = influences[i];
						bool flag3 = InfluenceChecker.CheckCondition(context, this, influence.Condition);
						if (flag3)
						{
							BaseGameDataDomain domain = DomainManager.Domains[(int)influence.TargetIndicator.DomainId];
							domain.InvalidateCache(this, influence, context, false);
						}
					}
				}
			}
		}

		// Token: 0x04002418 RID: 9240
		public ObjectCollectionHelperData CollectionHelperData;

		// Token: 0x04002419 RID: 9241
		public int DataStatesOffset;
	}
}
