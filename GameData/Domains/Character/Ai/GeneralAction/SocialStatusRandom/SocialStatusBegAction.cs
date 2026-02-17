using System;
using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.SocialStatusRandom
{
	// Token: 0x0200088B RID: 2187
	public class SocialStatusBegAction : IGeneralAction
	{
		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x060077F8 RID: 30712 RVA: 0x00461A89 File Offset: 0x0045FC89
		public sbyte ActionEnergyType
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x060077F9 RID: 30713 RVA: 0x00461A8C File Offset: 0x0045FC8C
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return targetChar.GetResource(6) >= this.MoneyAmount;
		}

		// Token: 0x060077FA RID: 30714 RVA: 0x00461AB0 File Offset: 0x0045FCB0
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			int selfCharId = selfChar.GetId();
			Location location = targetChar.GetLocation();
			monthlyEventCollection.AddAskForMoney(selfCharId, location, targetChar.GetId());
			CharacterDomain.AddLockMovementCharSet(selfCharId);
		}

		// Token: 0x060077FB RID: 30715 RVA: 0x00461AF0 File Offset: 0x0045FCF0
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
				selfChar.ChangeResource(context, 6, this.MoneyAmount);
				targetChar.ChangeResource(context, 6, -this.MoneyAmount);
				short favorChange = AiHelper.GeneralActionConstants.GetBegSucceedFavorabilityChange(context.Random, behaviorType);
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, (int)favorChange);
				lifeRecordCollection.AddAskForMoneySucceed(selfCharId, currDate, targetCharId, location, 6, this.MoneyAmount);
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset = secretInformationCollection.AddAcceptRequestGivingMoney(targetCharId, selfCharId, this.MoneyAmount);
				int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			else
			{
				short favorChange2 = AiHelper.GeneralActionConstants.GetBegFailFavorabilityChange(context.Random, behaviorType);
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, (int)favorChange2);
				lifeRecordCollection.AddAskForMoneyFail(selfCharId, currDate, targetCharId, location, 6, this.MoneyAmount);
				SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset2 = secretInformationCollection2.AddRefuseRequestGivingMoney(targetCharId, selfCharId, this.MoneyAmount);
				int secretInfoId2 = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset2, true);
			}
		}

		// Token: 0x0400213F RID: 8511
		public bool Succeed;

		// Token: 0x04002140 RID: 8512
		public int MoneyAmount;
	}
}
