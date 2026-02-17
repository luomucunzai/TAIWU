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
	// Token: 0x0200087B RID: 2171
	public class StealResourceAction : IGeneralAction
	{
		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x060077A8 RID: 30632 RVA: 0x0045EA3C File Offset: 0x0045CC3C
		public sbyte ActionEnergyType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060077A9 RID: 30633 RVA: 0x0045EA40 File Offset: 0x0045CC40
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return targetChar.GetResource(this.ResourceType) >= this.Amount;
		}

		// Token: 0x060077AA RID: 30634 RVA: 0x0045EA6C File Offset: 0x0045CC6C
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			Location location = selfChar.GetLocation();
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			bool flag = this.Phase <= 3;
			if (flag)
			{
				monthlyNotificationCollection.AddStealResourceFailure(selfCharId, location, targetCharId, this.ResourceType);
				this.ApplyChanges(context, selfChar, targetChar);
			}
			else
			{
				monthlyNotificationCollection.AddStealResourceSuccess(selfCharId, location, targetCharId, this.ResourceType);
				bool flag2 = this.Phase == 4;
				if (flag2)
				{
					monthlyEventCollection.AddStealResourceButBeCaught(selfCharId, location, targetCharId, this.ResourceType, this.Amount);
				}
				else
				{
					monthlyEventCollection.AddStealResourceAndEscape(selfCharId, location, targetCharId, this.ResourceType, this.Amount);
					this.ApplyChanges(context, selfChar, targetChar);
				}
				CharacterDomain.AddLockMovementCharSet(selfCharId);
			}
		}

		// Token: 0x060077AB RID: 30635 RVA: 0x0045EB3C File Offset: 0x0045CD3C
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
				selfChar.ChangeCurrMainAttribute(context, 1, (int)(-(int)GlobalConfig.Instance.HarmfulActionCost));
			}
			bool flag2 = this.Phase >= 4;
			if (flag2)
			{
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset = secretInformationCollection.AddStealResource(selfCharId, targetCharId, this.ResourceType);
				int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			switch (this.Phase)
			{
			case 0:
				lifeRecordCollection.AddStealResourceFail1(selfCharId, currDate, targetCharId, location, this.ResourceType);
				break;
			case 1:
				lifeRecordCollection.AddStealResourceFail2(selfCharId, currDate, targetCharId, location, this.ResourceType);
				break;
			case 2:
				lifeRecordCollection.AddStealResourceFail3(selfCharId, currDate, targetCharId, location, this.ResourceType);
				break;
			case 3:
				lifeRecordCollection.AddStealResourceFail4(selfCharId, currDate, targetCharId, location, this.ResourceType);
				break;
			case 4:
			{
				lifeRecordCollection.AddStealResourceSucceed(selfCharId, currDate, targetCharId, location, this.ResourceType, this.Amount);
				bool flag3 = targetCharId == taiwuCharId;
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
						lifeRecordCollection.AddStealResourceFailAndBeatenUp(selfCharId, currDate, targetCharId, location, this.ResourceType, this.Amount);
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
				lifeRecordCollection.AddStealResourceSucceedAndEscaped(selfCharId, currDate, targetCharId, location, this.ResourceType, this.Amount);
				break;
			}
			}
		}

		// Token: 0x04002106 RID: 8454
		public sbyte ResourceType;

		// Token: 0x04002107 RID: 8455
		public int Amount;

		// Token: 0x04002108 RID: 8456
		public sbyte Phase;
	}
}
