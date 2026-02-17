using System;
using GameData.Common;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;

namespace GameData.Domains.Character.Ai.GeneralAction.BehaviorAction
{
	// Token: 0x020008A4 RID: 2212
	public class GainExpByReadingAction : IGeneralAction
	{
		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06007875 RID: 30837 RVA: 0x0046489F File Offset: 0x00462A9F
		public sbyte ActionEnergyType
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06007876 RID: 30838 RVA: 0x004648A4 File Offset: 0x00462AA4
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return selfChar.GetInventory().Items.ContainsKey(this.ItemKey);
		}

		// Token: 0x06007877 RID: 30839 RVA: 0x004648CC File Offset: 0x00462ACC
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			throw new Exception("Current action requires no targetChar.");
		}

		// Token: 0x06007878 RID: 30840 RVA: 0x004648DC File Offset: 0x00462ADC
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			selfChar.ChangeExp(context, this.ExpGain);
			SkillBook skillBook = DomainManager.Item.GetElement_SkillBooks(this.ItemKey.Id);
			short durability = skillBook.GetCurrDurability() - this.DurabilityReduction;
			bool flag = durability <= 0;
			if (flag)
			{
				selfChar.RemoveInventoryItem(context, this.ItemKey, 1, true, false);
			}
			else
			{
				skillBook.SetCurrDurability(durability, context);
			}
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			lifeRecordCollection.AddGainExpByReadingOldBook(selfCharId, currDate, location, this.ItemKey.ItemType, this.ItemKey.TemplateId);
		}

		// Token: 0x04002175 RID: 8565
		public short DurabilityReduction;

		// Token: 0x04002176 RID: 8566
		public int ExpGain;

		// Token: 0x04002177 RID: 8567
		public ItemKey ItemKey;
	}
}
