using System;
using Config;
using GameData.Common;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.Notification;

namespace GameData.Domains.Character.Ai.GeneralAction.LifeSkillRandom
{
	// Token: 0x02000894 RID: 2196
	public class LifeSkillCraftingAction : IGeneralAction
	{
		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06007825 RID: 30757 RVA: 0x004628D0 File Offset: 0x00460AD0
		public sbyte ActionEnergyType
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x06007826 RID: 30758 RVA: 0x004628D4 File Offset: 0x00460AD4
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			Inventory inventory = selfChar.GetInventory();
			return inventory.Items.ContainsKey(this.ToolUsed) && inventory.Items.ContainsKey(this.Material) && selfChar.GetResource(6) >= this.RequiredMoney;
		}

		// Token: 0x06007827 RID: 30759 RVA: 0x00462928 File Offset: 0x00460B28
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			throw new Exception("Current action requires no targetChar.");
		}

		// Token: 0x06007828 RID: 30760 RVA: 0x00462938 File Offset: 0x00460B38
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			int selfCharId = selfChar.GetId();
			selfChar.ChangeResource(context, 6, -this.RequiredMoney);
			selfChar.RemoveInventoryItem(context, this.Material, 1, true, false);
			GameData.Domains.Item.CraftTool tool = DomainManager.Item.GetElement_CraftTools(this.ToolUsed.Id);
			CraftToolItem toolConfig = Config.CraftTool.Instance[this.ToolUsed.TemplateId];
			MaterialItem materialConfig = Config.Material.Instance[this.Material.TemplateId];
			short durabilityCost = toolConfig.DurabilityCost[(int)materialConfig.Grade];
			short currDurability = tool.GetCurrDurability();
			bool flag = currDurability <= durabilityCost;
			if (flag)
			{
				selfChar.RemoveInventoryItem(context, this.ToolUsed, 1, true, false);
			}
			else
			{
				tool.SetCurrDurability(tool.GetCurrDurability() - durabilityCost, context);
			}
			ItemKey item = DomainManager.Item.CreateItem(context, this.TargetItemType, this.TargetItemTemplateId);
			selfChar.AddInventoryItem(context, item, 1, false);
			sbyte targetItemGrade = ItemTemplateHelper.GetGrade(this.TargetItemType, this.TargetItemTemplateId);
			bool flag2 = targetItemGrade >= 6;
			if (flag2)
			{
				MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
				monthlyNotifications.AddMakeFamousItem(selfCharId, location, this.TargetItemType, this.TargetItemTemplateId);
				selfChar.RecordFameAction(context, 77, -1, 1, true);
			}
			lifeRecordCollection.AddMakeItem(selfCharId, currDate, location, this.TargetItemType, this.TargetItemTemplateId);
		}

		// Token: 0x04002150 RID: 8528
		public ItemKey ToolUsed;

		// Token: 0x04002151 RID: 8529
		public ItemKey Material;

		// Token: 0x04002152 RID: 8530
		public int RequiredMoney;

		// Token: 0x04002153 RID: 8531
		public sbyte TargetItemType;

		// Token: 0x04002154 RID: 8532
		public short TargetItemTemplateId;

		// Token: 0x04002155 RID: 8533
		public sbyte LifeSkillType;
	}
}
