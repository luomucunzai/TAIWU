using System;
using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.WealthDemand
{
	// Token: 0x0200087A RID: 2170
	public class RequestResourceAction : IGeneralAction
	{
		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x060077A3 RID: 30627 RVA: 0x0045E856 File Offset: 0x0045CA56
		public sbyte ActionEnergyType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060077A4 RID: 30628 RVA: 0x0045E85C File Offset: 0x0045CA5C
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return targetChar.GetResource(this.ResourceType) >= this.Amount;
		}

		// Token: 0x060077A5 RID: 30629 RVA: 0x0045E888 File Offset: 0x0045CA88
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			Location location = selfChar.GetLocation();
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			monthlyEventCollection.AddRequestResource(selfCharId, location, targetCharId, this.Amount, this.ResourceType);
			CharacterDomain.AddLockMovementCharSet(selfCharId);
		}

		// Token: 0x060077A6 RID: 30630 RVA: 0x0045E8D4 File Offset: 0x0045CAD4
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			Location location = selfChar.GetLocation();
			int currDate = DomainManager.World.GetCurrDate();
			bool agreeToRequest = this.AgreeToRequest;
			if (agreeToRequest)
			{
				selfChar.ChangeResource(context, this.ResourceType, this.Amount);
				targetChar.ChangeResource(context, this.ResourceType, -this.Amount);
				short deltaFavor = AiHelper.GeneralActionConstants.GetResourceFavorabilityChange(this.ResourceType, this.Amount);
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, (int)deltaFavor);
				sbyte happinessChange = AiHelper.GeneralActionConstants.GetResourceHappinessChange(this.ResourceType, this.Amount);
				selfChar.ChangeHappiness(context, (int)happinessChange);
				lifeRecordCollection.AddRequestResourceSucceed(selfCharId, currDate, targetCharId, location, this.ResourceType, this.Amount);
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset = secretInformationCollection.AddAcceptRequestResource(targetCharId, selfCharId, this.ResourceType);
				int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			else
			{
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, -3000);
				selfChar.ChangeHappiness(context, -3);
				lifeRecordCollection.AddRequestResourceFail(selfCharId, currDate, targetCharId, location, this.ResourceType, this.Amount);
				SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset2 = secretInformationCollection2.AddRefuseRequestResource(targetCharId, selfCharId, this.ResourceType);
				int secretInfoId2 = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset2, true);
			}
		}

		// Token: 0x04002103 RID: 8451
		public sbyte ResourceType;

		// Token: 0x04002104 RID: 8452
		public int Amount;

		// Token: 0x04002105 RID: 8453
		public bool AgreeToRequest;
	}
}
