using System;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.Information.Collection;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;

namespace GameData.Domains.Character.Ai.GeneralAction.WealthDemand
{
	// Token: 0x0200087D RID: 2173
	public class RobResourceAction : IGeneralAction
	{
		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x060077B2 RID: 30642 RVA: 0x0045F0F1 File Offset: 0x0045D2F1
		public sbyte ActionEnergyType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060077B3 RID: 30643 RVA: 0x0045F0F4 File Offset: 0x0045D2F4
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return targetChar.GetResource(this.ResourceType) >= this.Amount;
		}

		// Token: 0x060077B4 RID: 30644 RVA: 0x0045F120 File Offset: 0x0045D320
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			Location location = selfChar.GetLocation();
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			bool flag = this.Phase <= 2;
			if (flag)
			{
				monthlyNotificationCollection.AddRobResourceFailure(selfCharId, location, targetCharId, this.ResourceType);
				this.ApplyChanges(context, selfChar, targetChar);
			}
			else
			{
				monthlyEventCollection.AddRobResource(selfCharId, location, targetCharId, this.ResourceType, this.Amount);
				CharacterDomain.AddLockMovementCharSet(selfCharId);
			}
		}

		// Token: 0x060077B5 RID: 30645 RVA: 0x0045F1AC File Offset: 0x0045D3AC
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			bool flag = selfCharId != taiwuCharId;
			if (flag)
			{
				selfChar.ChangeCurrMainAttribute(context, 0, (int)(-(int)GlobalConfig.Instance.HarmfulActionCost));
			}
			bool flag2 = this.Phase >= 4;
			if (flag2)
			{
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset = secretInformationCollection.AddRobResource(selfCharId, targetCharId, this.ResourceType);
				int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			switch (this.Phase)
			{
			case 0:
				lifeRecordCollection.AddRobResourceFail1(selfCharId, currDate, targetCharId, location, this.ResourceType);
				break;
			case 1:
				lifeRecordCollection.AddRobResourceFail2(selfCharId, currDate, targetCharId, location, this.ResourceType);
				break;
			case 2:
				lifeRecordCollection.AddRobResourceFail3(selfCharId, currDate, targetCharId, location, this.ResourceType);
				break;
			case 3:
				lifeRecordCollection.AddRobResourceFail4(selfCharId, currDate, targetCharId, location, this.ResourceType);
				break;
			case 4:
			{
				lifeRecordCollection.AddRobResourceSucceed(selfCharId, currDate, targetCharId, location, this.ResourceType, this.Amount);
				bool flag3 = targetCharId == DomainManager.Taiwu.GetTaiwuCharId();
				if (!flag3)
				{
					AiHelper.NpcCombatResultType combatResultType = DomainManager.Character.SimulateCharacterCombat(context, targetChar, selfChar, CombatType.Beat, true, 1);
					bool flag4 = combatResultType - AiHelper.NpcCombatResultType.MajorDefeat <= 1;
					bool flag5 = flag4;
					if (flag5)
					{
						selfChar.ChangeResource(context, this.ResourceType, this.Amount);
						targetChar.ChangeResource(context, this.ResourceType, -this.Amount);
						int happinessChange = (int)(-(int)AiHelper.GeneralActionConstants.GetResourceHappinessChange(this.ResourceType, this.Amount));
						targetChar.ChangeHappiness(context, happinessChange);
						DomainManager.Character.SimulateCharacterCombatResult(context, selfChar, targetChar, -40, -20, 0);
					}
					else
					{
						lifeRecordCollection.AddRobResourceFailAndBeatenUp(selfCharId, currDate, targetCharId, location, this.ResourceType, this.Amount);
						DomainManager.Character.SimulateCharacterCombatResult(context, targetChar, selfChar, -40, -20, 0);
					}
				}
				break;
			}
			default:
			{
				selfChar.ChangeResource(context, this.ResourceType, this.Amount);
				targetChar.ChangeResource(context, this.ResourceType, -this.Amount);
				int happinessChange2 = (int)(-(int)AiHelper.GeneralActionConstants.GetResourceHappinessChange(this.ResourceType, this.Amount));
				targetChar.ChangeHappiness(context, happinessChange2);
				lifeRecordCollection.AddRobResourceSucceedAndEscaped(selfCharId, currDate, targetCharId, location, this.ResourceType, this.Amount);
				break;
			}
			}
		}

		// Token: 0x0400210C RID: 8460
		public sbyte ResourceType;

		// Token: 0x0400210D RID: 8461
		public int Amount;

		// Token: 0x0400210E RID: 8462
		public sbyte Phase;
	}
}
