namespace GameData.Domains.Information;

public class SecretInformationSelection
{
	public static class SelectionGroupKeys
	{
		public static readonly string Normal = "NormalSelection";

		public static readonly string Special = "SpecialSelection";

		public static readonly string MainAttributeCost = "MainAttributeCost";

		public static readonly string FiveBehavior = "FiveBehavior";

		public static readonly string OtherOption = "OtherOption";
	}

	public static class SelectionArgKey
	{
		public static readonly string Visible = "Visible";

		public static readonly string Available = "Available";

		public static readonly string ResultIndex = "ResultIndex";

		public static readonly string Content = "Content";

		public static readonly string Cost = "Cost";
	}

	public enum SelectionGroup
	{
		Normal,
		Special,
		MainAttributeCost,
		FiveBehavior,
		OtherOption
	}

	public struct SelectionItem
	{
		public short TemplateId;

		public short Priority;

		public string Content;

		public SelectionGroup SelectionGroup;

		public bool Available;

		public short ResultIndex;

		public short ExtraIndex;

		public short ExtraValue;

		public SelectionItem(short templateId, short priority, string content, SelectionGroup selectionGroup, bool available, short resultIndex, short extraIndex, short extraValue)
		{
			TemplateId = templateId;
			Priority = priority;
			Content = content;
			SelectionGroup = selectionGroup;
			Available = available;
			ResultIndex = resultIndex;
			ExtraIndex = extraIndex;
			ExtraValue = extraValue;
		}
	}
}
