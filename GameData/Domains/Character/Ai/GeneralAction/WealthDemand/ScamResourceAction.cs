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
	// Token: 0x0200087C RID: 2172
	public class ScamResourceAction : IGeneralAction
	{
		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x060077AD RID: 30637 RVA: 0x0045EDB9 File Offset: 0x0045CFB9
		public sbyte ActionEnergyType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060077AE RID: 30638 RVA: 0x0045EDBC File Offset: 0x0045CFBC
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return targetChar.GetResource(this.ResourceType) >= this.Amount;
		}

		// Token: 0x060077AF RID: 30639 RVA: 0x0045EDE8 File Offset: 0x0045CFE8
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
				monthlyNotificationCollection.AddCheatResourceFailure(selfCharId, location, targetCharId, this.ResourceType);
				this.ApplyChanges(context, selfChar, targetChar);
			}
			else
			{
				monthlyEventCollection.AddScamResource(selfCharId, location, targetCharId, this.ResourceType, this.Amount);
				CharacterDomain.AddLockMovementCharSet(selfCharId);
			}
		}

		// Token: 0x060077B0 RID: 30640 RVA: 0x0045EE74 File Offset: 0x0045D074
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
				selfChar.ChangeCurrMainAttribute(context, 2, (int)(-(int)GlobalConfig.Instance.HarmfulActionCost));
			}
			bool flag2 = this.Phase >= 4;
			if (flag2)
			{
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset = secretInformationCollection.AddScamResource(selfCharId, targetCharId, this.ResourceType);
				int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			switch (this.Phase)
			{
			case 0:
				lifeRecordCollection.AddScamResourceFail1(selfCharId, currDate, targetCharId, location, this.ResourceType);
				break;
			case 1:
				lifeRecordCollection.AddScamResourceFail2(selfCharId, currDate, targetCharId, location, this.ResourceType);
				break;
			case 2:
				lifeRecordCollection.AddScamResourceFail3(selfCharId, currDate, targetCharId, location, this.ResourceType);
				break;
			case 3:
				lifeRecordCollection.AddScamResourceFail4(selfCharId, currDate, targetCharId, location, this.ResourceType);
				break;
			case 4:
			{
				lifeRecordCollection.AddScamResourceSucceed(selfCharId, currDate, targetCharId, location, this.ResourceType, this.Amount);
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
						lifeRecordCollection.AddScamResourceFailAndBeatenUp(selfCharId, currDate, targetCharId, location, this.ResourceType, this.Amount);
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
				lifeRecordCollection.AddScamResourceSucceedAndEscaped(selfCharId, currDate, targetCharId, location, this.ResourceType, this.Amount);
				break;
			}
			}
		}

		// Token: 0x04002109 RID: 8457
		public sbyte ResourceType;

		// Token: 0x0400210A RID: 8458
		public int Amount;

		// Token: 0x0400210B RID: 8459
		public sbyte Phase;
	}
}
