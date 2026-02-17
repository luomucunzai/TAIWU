using System;

namespace GameData.Domains.Information
{
	// Token: 0x02000686 RID: 1670
	public class SecretInformationSelection
	{
		// Token: 0x02000AFE RID: 2814
		public static class SelectionGroupKeys
		{
			// Token: 0x04002D84 RID: 11652
			public static readonly string Normal = "NormalSelection";

			// Token: 0x04002D85 RID: 11653
			public static readonly string Special = "SpecialSelection";

			// Token: 0x04002D86 RID: 11654
			public static readonly string MainAttributeCost = "MainAttributeCost";

			// Token: 0x04002D87 RID: 11655
			public static readonly string FiveBehavior = "FiveBehavior";

			// Token: 0x04002D88 RID: 11656
			public static readonly string OtherOption = "OtherOption";
		}

		// Token: 0x02000AFF RID: 2815
		public static class SelectionArgKey
		{
			// Token: 0x04002D89 RID: 11657
			public static readonly string Visible = "Visible";

			// Token: 0x04002D8A RID: 11658
			public static readonly string Available = "Available";

			// Token: 0x04002D8B RID: 11659
			public static readonly string ResultIndex = "ResultIndex";

			// Token: 0x04002D8C RID: 11660
			public static readonly string Content = "Content";

			// Token: 0x04002D8D RID: 11661
			public static readonly string Cost = "Cost";
		}

		// Token: 0x02000B00 RID: 2816
		public enum SelectionGroup
		{
			// Token: 0x04002D8F RID: 11663
			Normal,
			// Token: 0x04002D90 RID: 11664
			Special,
			// Token: 0x04002D91 RID: 11665
			MainAttributeCost,
			// Token: 0x04002D92 RID: 11666
			FiveBehavior,
			// Token: 0x04002D93 RID: 11667
			OtherOption
		}

		// Token: 0x02000B01 RID: 2817
		public struct SelectionItem
		{
			// Token: 0x060089A7 RID: 35239 RVA: 0x004EF52A File Offset: 0x004ED72A
			public SelectionItem(short templateId, short priority, string content, SecretInformationSelection.SelectionGroup selectionGroup, bool available, short resultIndex, short extraIndex, short extraValue)
			{
				this.TemplateId = templateId;
				this.Priority = priority;
				this.Content = content;
				this.SelectionGroup = selectionGroup;
				this.Available = available;
				this.ResultIndex = resultIndex;
				this.ExtraIndex = extraIndex;
				this.ExtraValue = extraValue;
			}

			// Token: 0x04002D94 RID: 11668
			public short TemplateId;

			// Token: 0x04002D95 RID: 11669
			public short Priority;

			// Token: 0x04002D96 RID: 11670
			public string Content;

			// Token: 0x04002D97 RID: 11671
			public SecretInformationSelection.SelectionGroup SelectionGroup;

			// Token: 0x04002D98 RID: 11672
			public bool Available;

			// Token: 0x04002D99 RID: 11673
			public short ResultIndex;

			// Token: 0x04002D9A RID: 11674
			public short ExtraIndex;

			// Token: 0x04002D9B RID: 11675
			public short ExtraValue;
		}
	}
}
