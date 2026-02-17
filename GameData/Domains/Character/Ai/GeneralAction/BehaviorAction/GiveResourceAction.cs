using System;
using GameData.Common;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.Notification;

namespace GameData.Domains.Character.Ai.GeneralAction.BehaviorAction
{
	// Token: 0x020008AB RID: 2219
	public class GiveResourceAction : IGeneralAction
	{
		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06007898 RID: 30872 RVA: 0x00464F82 File Offset: 0x00463182
		public sbyte ActionEnergyType
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06007899 RID: 30873 RVA: 0x00464F88 File Offset: 0x00463188
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return selfChar.GetResource(this.ResourceType) >= this.Amount;
		}

		// Token: 0x0600789A RID: 30874 RVA: 0x00464FB4 File Offset: 0x004631B4
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			Location location = selfChar.GetLocation();
			MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
			monthlyNotifications.AddGivePresentResource(selfCharId, location, this.ResourceType, targetCharId);
			this.ApplyChanges(context, selfChar, targetChar);
		}

		// Token: 0x0600789B RID: 30875 RVA: 0x00464FFC File Offset: 0x004631FC
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			selfChar.ChangeResource(context, this.ResourceType, -this.Amount);
			targetChar.ChangeResource(context, this.ResourceType, this.Amount);
			short favorChange = AiHelper.GeneralActionConstants.GetResourceFavorabilityChange(this.ResourceType, this.Amount);
			sbyte happinessChange = AiHelper.GeneralActionConstants.GetResourceHappinessChange(this.ResourceType, this.Amount);
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, targetChar, selfChar, (int)favorChange);
			targetChar.ChangeHappiness(context, (int)happinessChange);
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			lifeRecordCollection.AddGiveResource(selfCharId, currDate, targetCharId, location, this.ResourceType, this.Amount);
		}

		// Token: 0x04002188 RID: 8584
		public sbyte ResourceType;

		// Token: 0x04002189 RID: 8585
		public int Amount;
	}
}
