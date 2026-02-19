using System;
using GameData.Common;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;

namespace GameData.Domains.Character.Ai.GeneralAction.BehaviorAction;

public class GainExpByReadingAction : IGeneralAction
{
	public short DurabilityReduction;

	public int ExpGain;

	public ItemKey ItemKey;

	public sbyte ActionEnergyType => 3;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		return selfChar.GetInventory().Items.ContainsKey(ItemKey);
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		throw new Exception("Current action requires no targetChar.");
	}

	public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		selfChar.ChangeExp(context, ExpGain);
		SkillBook element_SkillBooks = DomainManager.Item.GetElement_SkillBooks(ItemKey.Id);
		short num = (short)(element_SkillBooks.GetCurrDurability() - DurabilityReduction);
		if (num <= 0)
		{
			selfChar.RemoveInventoryItem(context, ItemKey, 1, deleteItem: true);
		}
		else
		{
			element_SkillBooks.SetCurrDurability(num, context);
		}
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		lifeRecordCollection.AddGainExpByReadingOldBook(id, currDate, location, ItemKey.ItemType, ItemKey.TemplateId);
	}
}
