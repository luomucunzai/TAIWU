using System;
using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.SocialStatusRandom
{
	// Token: 0x0200088C RID: 2188
	public class SocialStatusHealAction : IGeneralAction
	{
		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x060077FD RID: 30717 RVA: 0x00461C2D File Offset: 0x0045FE2D
		public sbyte ActionEnergyType
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x060077FE RID: 30718 RVA: 0x00461C30 File Offset: 0x0045FE30
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return selfChar.GetResource(5) >= this.HerbAmount;
		}

		// Token: 0x060077FF RID: 30719 RVA: 0x00461C54 File Offset: 0x0045FE54
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			Location location = targetChar.GetLocation();
			switch (this.Type)
			{
			case EHealActionType.Healing:
				monthlyEventCollection.AddAdviseHealInjury(selfCharId, location, targetCharId, this.HerbAmount);
				break;
			case EHealActionType.Detox:
				monthlyEventCollection.AddAdviseHealPoison(selfCharId, location, targetCharId, this.HerbAmount);
				break;
			case EHealActionType.Breathing:
				monthlyEventCollection.AddAdviseHealDisorderOfQi(selfCharId, location, targetCharId, this.HerbAmount);
				break;
			case EHealActionType.Recover:
				monthlyEventCollection.AddAdviseHealHealth(selfCharId, location, targetCharId, this.HerbAmount);
				break;
			}
			CharacterDomain.AddLockMovementCharSet(selfChar.GetId());
		}

		// Token: 0x06007800 RID: 30720 RVA: 0x00461CFC File Offset: 0x0045FEFC
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			selfChar.ChangeResource(context, 5, this.HerbAmount);
			DomainManager.Character.UseCombatResources(context, selfCharId, this.Type, 1);
			bool cureHasEffect = selfChar.DoHealAction(context, this.Type, targetChar, false, false);
			bool flag = cureHasEffect;
			if (flag)
			{
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, targetChar, selfChar, 3000);
			}
			lifeRecordCollection.AddCureSucceed(selfCharId, currDate, targetCharId, location);
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int secretInfoOffset = secretInformationCollection.AddCure(selfCharId, targetCharId);
			int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
		}

		// Token: 0x04002141 RID: 8513
		public EHealActionType Type;

		// Token: 0x04002142 RID: 8514
		public int HerbAmount;
	}
}
