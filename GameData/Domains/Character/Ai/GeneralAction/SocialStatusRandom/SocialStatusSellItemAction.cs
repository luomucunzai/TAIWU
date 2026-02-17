using System;
using GameData.Common;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.SocialStatusRandom
{
	// Token: 0x02000891 RID: 2193
	public class SocialStatusSellItemAction : IGeneralAction
	{
		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06007816 RID: 30742 RVA: 0x004621CA File Offset: 0x004603CA
		public sbyte ActionEnergyType
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x06007817 RID: 30743 RVA: 0x004621D0 File Offset: 0x004603D0
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return DomainManager.Merchant.MerchantHasTargetItem(selfChar.GetId(), this.ItemKey, this.Amount) && targetChar.GetResource(6) >= this.Price;
		}

		// Token: 0x06007818 RID: 30744 RVA: 0x00462218 File Offset: 0x00460418
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			Location location = targetChar.GetLocation();
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			monthlyEventCollection.AddAdviseSales(selfCharId, location, targetCharId, (ulong)this.ItemKey, this.Price, this.Amount);
			CharacterDomain.AddLockMovementCharSet(selfChar.GetId());
		}

		// Token: 0x06007819 RID: 30745 RVA: 0x00462274 File Offset: 0x00460474
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			sbyte behaviorType = selfChar.GetBehaviorType();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			bool succeed = this.Succeed;
			if (succeed)
			{
				selfChar.ChangeResource(context, 6, this.Price);
				targetChar.ChangeResource(context, 6, -this.Price);
				DomainManager.Merchant.RemoveExistingMerchantItem(context, selfCharId, this.ItemKey, this.Amount);
				targetChar.AddInventoryItem(context, this.ItemKey, this.Amount, false);
				short favorChange = AiHelper.GeneralActionConstants.GetBegSucceedFavorabilityChange(context.Random, behaviorType);
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, (int)favorChange);
				lifeRecordCollection.AddSellSucceed(selfCharId, currDate, targetCharId, location, this.ItemKey.ItemType, this.ItemKey.TemplateId);
			}
			else
			{
				short favorChange2 = AiHelper.GeneralActionConstants.GetBegFailFavorabilityChange(context.Random, behaviorType);
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, (int)favorChange2);
				lifeRecordCollection.AddSellFail(selfCharId, currDate, targetCharId, location, this.ItemKey.ItemType, this.ItemKey.TemplateId);
			}
		}

		// Token: 0x04002147 RID: 8519
		public bool Succeed;

		// Token: 0x04002148 RID: 8520
		public ItemKey ItemKey;

		// Token: 0x04002149 RID: 8521
		public int Amount;

		// Token: 0x0400214A RID: 8522
		public int Price;
	}
}
