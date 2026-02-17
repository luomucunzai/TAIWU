using System;
using Config;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;

namespace GameData.Domains.Character.Ai.GeneralAction.StudyDemand
{
	// Token: 0x02000887 RID: 2183
	public class ScamLifeSkillDemandAction : IGeneralAction
	{
		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x060077E4 RID: 30692 RVA: 0x00460DCC File Offset: 0x0045EFCC
		public sbyte ActionEnergyType
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x060077E5 RID: 30693 RVA: 0x00460DD0 File Offset: 0x0045EFD0
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return selfChar.FindLearnedLifeSkillIndex(Config.SkillBook.Instance[this.BookTemplateId].LifeSkillTemplateId) < 0;
		}

		// Token: 0x060077E6 RID: 30694 RVA: 0x00460E00 File Offset: 0x0045F000
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			Location location = selfChar.GetLocation();
			SkillBookItem bookCfg = Config.SkillBook.Instance[this.BookTemplateId];
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			bool flag = this.Phase <= 2;
			if (flag)
			{
				monthlyNotificationCollection.AddCheatLifeSkillFailure(selfCharId, location, targetCharId, bookCfg.LifeSkillTemplateId);
				this.ApplyChanges(context, selfChar, targetChar);
			}
			else
			{
				monthlyEventCollection.AddScamLifeSkill(selfCharId, location, targetCharId, 10, this.BookTemplateId, (int)(this.PageId + 1));
				CharacterDomain.AddLockMovementCharSet(selfCharId);
			}
		}

		// Token: 0x060077E7 RID: 30695 RVA: 0x00460EA4 File Offset: 0x0045F0A4
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			SkillBookItem bookCfg = Config.SkillBook.Instance[this.BookTemplateId];
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
				int secretInfoOffset = secretInformationCollection.AddScamLifeSkill(selfCharId, targetCharId, bookCfg.LifeSkillTemplateId);
				int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			switch (this.Phase)
			{
			case 0:
				lifeRecordCollection.AddScamLifeSkillFail1(selfCharId, currDate, targetCharId, location, 10, this.BookTemplateId, (int)(this.PageId + 1));
				break;
			case 1:
				lifeRecordCollection.AddScamLifeSkillFail2(selfCharId, currDate, targetCharId, location, 10, this.BookTemplateId, (int)(this.PageId + 1));
				break;
			case 2:
				lifeRecordCollection.AddScamLifeSkillFail3(selfCharId, currDate, targetCharId, location, 10, this.BookTemplateId, (int)(this.PageId + 1));
				break;
			case 3:
				lifeRecordCollection.AddScamLifeSkillFail4(selfCharId, currDate, targetCharId, location, 10, this.BookTemplateId, (int)(this.PageId + 1));
				break;
			case 4:
			{
				bool flag3 = selfCharId == taiwuCharId;
				if (flag3)
				{
					ItemKey itemKey = DomainManager.Item.CreateDemandedSkillBook(context, this.BookTemplateId, this.PageId, 0);
					selfChar.AddInventoryItem(context, itemKey, 1, false);
				}
				selfChar.LearnNewLifeSkill(context, bookCfg.LifeSkillTemplateId, (byte)(1 << (int)this.PageId));
				lifeRecordCollection.AddScamLifeSkillSucceed(selfCharId, currDate, targetCharId, location, 10, this.BookTemplateId, (int)(this.PageId + 1));
				bool flag4 = targetCharId == taiwuCharId;
				if (!flag4)
				{
					AiHelper.NpcCombatResultType combatResultType = DomainManager.Character.SimulateCharacterCombat(context, targetChar, selfChar, CombatType.Beat, true, 1);
					bool flag5 = combatResultType - AiHelper.NpcCombatResultType.MajorDefeat <= 1;
					bool flag6 = flag5;
					if (flag6)
					{
						DomainManager.Character.SimulateCharacterCombatResult(context, selfChar, targetChar, -40, -20, 0);
					}
					else
					{
						DomainManager.Character.SimulateCharacterCombatResult(context, targetChar, selfChar, -40, -20, 0);
					}
				}
				break;
			}
			default:
			{
				bool flag7 = selfCharId == taiwuCharId;
				if (flag7)
				{
					ItemKey itemKey2 = DomainManager.Item.CreateDemandedSkillBook(context, this.BookTemplateId, this.PageId, 0);
					selfChar.AddInventoryItem(context, itemKey2, 1, false);
				}
				selfChar.LearnNewLifeSkill(context, bookCfg.LifeSkillTemplateId, (byte)(1 << (int)this.PageId));
				lifeRecordCollection.AddScamLifeSkillSucceedAndEscaped(selfCharId, currDate, targetCharId, location, 10, this.BookTemplateId, (int)(this.PageId + 1));
				break;
			}
			}
		}

		// Token: 0x04002130 RID: 8496
		public short BookTemplateId;

		// Token: 0x04002131 RID: 8497
		public byte PageId;

		// Token: 0x04002132 RID: 8498
		public sbyte Phase;
	}
}
