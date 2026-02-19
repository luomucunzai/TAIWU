using System;
using Config;
using GameData.Common;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.Notification;

namespace GameData.Domains.Character.Ai.GeneralAction.LifeSkillRandom;

public class LifeSkillCraftingAction : IGeneralAction
{
	public ItemKey ToolUsed;

	public ItemKey Material;

	public int RequiredMoney;

	public sbyte TargetItemType;

	public short TargetItemTemplateId;

	public sbyte LifeSkillType;

	public sbyte ActionEnergyType => 4;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		Inventory inventory = selfChar.GetInventory();
		return inventory.Items.ContainsKey(ToolUsed) && inventory.Items.ContainsKey(Material) && selfChar.GetResource(6) >= RequiredMoney;
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		throw new Exception("Current action requires no targetChar.");
	}

	public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		int id = selfChar.GetId();
		selfChar.ChangeResource(context, 6, -RequiredMoney);
		selfChar.RemoveInventoryItem(context, Material, 1, deleteItem: true);
		GameData.Domains.Item.CraftTool element_CraftTools = DomainManager.Item.GetElement_CraftTools(ToolUsed.Id);
		CraftToolItem craftToolItem = Config.CraftTool.Instance[ToolUsed.TemplateId];
		MaterialItem materialItem = Config.Material.Instance[Material.TemplateId];
		short num = craftToolItem.DurabilityCost[materialItem.Grade];
		short currDurability = element_CraftTools.GetCurrDurability();
		if (currDurability <= num)
		{
			selfChar.RemoveInventoryItem(context, ToolUsed, 1, deleteItem: true);
		}
		else
		{
			element_CraftTools.SetCurrDurability((short)(element_CraftTools.GetCurrDurability() - num), context);
		}
		ItemKey itemKey = DomainManager.Item.CreateItem(context, TargetItemType, TargetItemTemplateId);
		selfChar.AddInventoryItem(context, itemKey, 1);
		sbyte grade = ItemTemplateHelper.GetGrade(TargetItemType, TargetItemTemplateId);
		if (grade >= 6)
		{
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			monthlyNotificationCollection.AddMakeFamousItem(id, location, TargetItemType, TargetItemTemplateId);
			selfChar.RecordFameAction(context, 77, -1, 1);
		}
		lifeRecordCollection.AddMakeItem(id, currDate, location, TargetItemType, TargetItemTemplateId);
	}
}
