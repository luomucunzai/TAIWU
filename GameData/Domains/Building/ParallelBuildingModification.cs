using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.Building
{
	// Token: 0x020008C3 RID: 2243
	public class ParallelBuildingModification
	{
		// Token: 0x06007E84 RID: 32388 RVA: 0x004BC9FF File Offset: 0x004BABFF
		public unsafe void SetResource(sbyte resourceType, int count)
		{
			*(ref this.DeltaResources.Items.FixedElementField + (IntPtr)resourceType * 4) = count;
		}

		// Token: 0x06007E85 RID: 32389 RVA: 0x004BCA1C File Offset: 0x004BAC1C
		public unsafe int GetResource(sbyte resourceType)
		{
			return *(ref this.DeltaResources.Items.FixedElementField + (IntPtr)resourceType * 4);
		}

		// Token: 0x06007E86 RID: 32390 RVA: 0x004BCA44 File Offset: 0x004BAC44
		public void AddCollectableEarning(TemplateKey templateKey)
		{
			if (this.CollectableEarnings == null)
			{
				this.CollectableEarnings = new List<TemplateKey>();
			}
			this.CollectableEarnings.Add(templateKey);
		}

		// Token: 0x06007E87 RID: 32391 RVA: 0x004BCA67 File Offset: 0x004BAC67
		public void AddCollectableResources(sbyte resourceType, int amount)
		{
			if (this.CollectableResources == null)
			{
				this.CollectableResources = new List<IntPair>();
			}
			this.CollectableResources.Add(new IntPair((int)resourceType, amount));
		}

		// Token: 0x06007E88 RID: 32392 RVA: 0x004BCA90 File Offset: 0x004BAC90
		public void AddShopSalaryResources(int charId, sbyte resourceType, int amount)
		{
			if (this.ShopBuildingSalaryList == null)
			{
				this.ShopBuildingSalaryList = new List<ValueTuple<int, sbyte, int>>();
			}
			this.ShopBuildingSalaryList.Add(new ValueTuple<int, sbyte, int>(charId, resourceType, amount));
		}

		// Token: 0x06007E89 RID: 32393 RVA: 0x004BCABA File Offset: 0x004BACBA
		public void AddLearnCombatSkill(int charId, short templateId, byte pageInternalIndex)
		{
			if (this.LearnCombatSkills == null)
			{
				this.LearnCombatSkills = new List<ValueTuple<int, short, byte>>();
			}
			this.LearnCombatSkills.Add(new ValueTuple<int, short, byte>(charId, templateId, pageInternalIndex));
		}

		// Token: 0x06007E8A RID: 32394 RVA: 0x004BCAE4 File Offset: 0x004BACE4
		public void AddLearnLifeSkill(int charId, short templateId, byte pageId)
		{
			if (this.LearnLifeSkills == null)
			{
				this.LearnLifeSkills = new List<ValueTuple<int, short, byte>>();
			}
			this.LearnLifeSkills.Add(new ValueTuple<int, short, byte>(charId, templateId, pageId));
		}

		// Token: 0x04002299 RID: 8857
		public BuildingBlockKey BlockKey;

		// Token: 0x0400229A RID: 8858
		public BuildingBlockData BlockData;

		// Token: 0x0400229B RID: 8859
		public List<TemplateKey> CollectableEarnings;

		// Token: 0x0400229C RID: 8860
		public List<IntPair> CollectableResources;

		// Token: 0x0400229D RID: 8861
		[TupleElementNames(new string[]
		{
			"index",
			"exchangeResource"
		})]
		public List<ValueTuple<sbyte, IntPair>> ShopSoldItems;

		// Token: 0x0400229E RID: 8862
		public List<IntPair> RecruitLevelList;

		// Token: 0x0400229F RID: 8863
		public List<ItemKey> FixBookList;

		// Token: 0x040022A0 RID: 8864
		[TupleElementNames(new string[]
		{
			"charId",
			"skillTemplateId",
			"pageId"
		})]
		public List<ValueTuple<int, short, byte>> LearnLifeSkills;

		// Token: 0x040022A1 RID: 8865
		[TupleElementNames(new string[]
		{
			"charId",
			"skillTemplateId",
			"pageInternalIndex"
		})]
		public List<ValueTuple<int, short, byte>> LearnCombatSkills;

		// Token: 0x040022A2 RID: 8866
		[TupleElementNames(new string[]
		{
			"charId",
			"resourceType",
			"addCount"
		})]
		public List<ValueTuple<int, sbyte, int>> ShopBuildingSalaryList;

		// Token: 0x040022A3 RID: 8867
		public Dictionary<BuildingBlockKey, int> BuildingMoneyPrestigeSuccessRateCompensationChanged;

		// Token: 0x040022A4 RID: 8868
		public bool FreeOperator = false;

		// Token: 0x040022A5 RID: 8869
		public bool FreeShopManager = false;

		// Token: 0x040022A6 RID: 8870
		public bool RemoveMakeItemData = false;

		// Token: 0x040022A7 RID: 8871
		public bool ResetAllChildrenBlocks = false;

		// Token: 0x040022A8 RID: 8872
		public bool RemoveCollectResourceType = false;

		// Token: 0x040022A9 RID: 8873
		public bool RemoveFromAutoExpand = false;

		// Token: 0x040022AA RID: 8874
		public bool RemoveEventBookData = false;

		// Token: 0x040022AB RID: 8875
		public bool BuildingOperationComplete = false;

		// Token: 0x040022AC RID: 8876
		public bool RemoveResidence;

		// Token: 0x040022AD RID: 8877
		public bool AddBuilding;

		// Token: 0x040022AE RID: 8878
		public bool ExpandBuilding;

		// Token: 0x040022AF RID: 8879
		public ResourceInts DeltaResources;
	}
}
