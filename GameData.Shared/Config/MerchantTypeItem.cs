using System;
using Config.Common;

namespace Config;

[Serializable]
public class MerchantTypeItem : ConfigItem<MerchantTypeItem, sbyte>
{
	public readonly sbyte TemplateId;

	public readonly string Name;

	public readonly short HeadArea;

	public readonly sbyte HeadLevel;

	public readonly short BranchArea;

	public readonly sbyte BranchLevel;

	public readonly EMerchantTypeCityAttributeType CityAttributeType;

	public readonly string CaravanAvatar;

	public readonly string Icon;

	public readonly string Prologue;

	public readonly string IntroduceDialog;

	public readonly string FavorDialog1;

	public readonly string FavorDialog2;

	public readonly string FavorDialog3;

	public readonly string SpringSeasonDialog;

	public readonly string SummerSeasonDialog;

	public readonly string AutumnSeasonDialog;

	public readonly string WinterSeasonDialog;

	public readonly string SpringMarketsAdventureSeasonDialog;

	public readonly string EventContent;

	public readonly string EventDialogContent;

	public readonly string TaiwuVillagerMerchantChangingTypeContent;

	public readonly string RefreshDesc;

	public MerchantTypeItem(sbyte templateId, int name, short headArea, sbyte headLevel, short branchArea, sbyte branchLevel, EMerchantTypeCityAttributeType cityAttributeType, string caravanAvatar, string icon, int prologue, int introduceDialog, int favorDialog1, int favorDialog2, int favorDialog3, int springSeasonDialog, int summerSeasonDialog, int autumnSeasonDialog, int winterSeasonDialog, int springMarketsAdventureSeasonDialog, int eventContent, int eventDialogContent, int taiwuVillagerMerchantChangingTypeContent, int refreshDesc)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("MerchantType_language", name);
		HeadArea = headArea;
		HeadLevel = headLevel;
		BranchArea = branchArea;
		BranchLevel = branchLevel;
		CityAttributeType = cityAttributeType;
		CaravanAvatar = caravanAvatar;
		Icon = icon;
		Prologue = LocalStringManager.GetConfig("MerchantType_language", prologue);
		IntroduceDialog = LocalStringManager.GetConfig("MerchantType_language", introduceDialog);
		FavorDialog1 = LocalStringManager.GetConfig("MerchantType_language", favorDialog1);
		FavorDialog2 = LocalStringManager.GetConfig("MerchantType_language", favorDialog2);
		FavorDialog3 = LocalStringManager.GetConfig("MerchantType_language", favorDialog3);
		SpringSeasonDialog = LocalStringManager.GetConfig("MerchantType_language", springSeasonDialog);
		SummerSeasonDialog = LocalStringManager.GetConfig("MerchantType_language", summerSeasonDialog);
		AutumnSeasonDialog = LocalStringManager.GetConfig("MerchantType_language", autumnSeasonDialog);
		WinterSeasonDialog = LocalStringManager.GetConfig("MerchantType_language", winterSeasonDialog);
		SpringMarketsAdventureSeasonDialog = LocalStringManager.GetConfig("MerchantType_language", springMarketsAdventureSeasonDialog);
		EventContent = LocalStringManager.GetConfig("MerchantType_language", eventContent);
		EventDialogContent = LocalStringManager.GetConfig("MerchantType_language", eventDialogContent);
		TaiwuVillagerMerchantChangingTypeContent = LocalStringManager.GetConfig("MerchantType_language", taiwuVillagerMerchantChangingTypeContent);
		RefreshDesc = LocalStringManager.GetConfig("MerchantType_language", refreshDesc);
	}

	public MerchantTypeItem()
	{
		TemplateId = 0;
		Name = null;
		HeadArea = 0;
		HeadLevel = 0;
		BranchArea = 0;
		BranchLevel = 0;
		CityAttributeType = EMerchantTypeCityAttributeType.Invalid;
		CaravanAvatar = null;
		Icon = null;
		Prologue = null;
		IntroduceDialog = null;
		FavorDialog1 = null;
		FavorDialog2 = null;
		FavorDialog3 = null;
		SpringSeasonDialog = null;
		SummerSeasonDialog = null;
		AutumnSeasonDialog = null;
		WinterSeasonDialog = null;
		SpringMarketsAdventureSeasonDialog = null;
		EventContent = null;
		EventDialogContent = null;
		TaiwuVillagerMerchantChangingTypeContent = null;
		RefreshDesc = null;
	}

	public MerchantTypeItem(sbyte templateId, MerchantTypeItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		HeadArea = other.HeadArea;
		HeadLevel = other.HeadLevel;
		BranchArea = other.BranchArea;
		BranchLevel = other.BranchLevel;
		CityAttributeType = other.CityAttributeType;
		CaravanAvatar = other.CaravanAvatar;
		Icon = other.Icon;
		Prologue = other.Prologue;
		IntroduceDialog = other.IntroduceDialog;
		FavorDialog1 = other.FavorDialog1;
		FavorDialog2 = other.FavorDialog2;
		FavorDialog3 = other.FavorDialog3;
		SpringSeasonDialog = other.SpringSeasonDialog;
		SummerSeasonDialog = other.SummerSeasonDialog;
		AutumnSeasonDialog = other.AutumnSeasonDialog;
		WinterSeasonDialog = other.WinterSeasonDialog;
		SpringMarketsAdventureSeasonDialog = other.SpringMarketsAdventureSeasonDialog;
		EventContent = other.EventContent;
		EventDialogContent = other.EventDialogContent;
		TaiwuVillagerMerchantChangingTypeContent = other.TaiwuVillagerMerchantChangingTypeContent;
		RefreshDesc = other.RefreshDesc;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override MerchantTypeItem Duplicate(int templateId)
	{
		return new MerchantTypeItem((sbyte)templateId, this);
	}
}
