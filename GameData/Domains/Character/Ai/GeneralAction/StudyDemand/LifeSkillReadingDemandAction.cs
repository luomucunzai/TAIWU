using System;
using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.StudyDemand
{
	// Token: 0x02000889 RID: 2185
	public class LifeSkillReadingDemandAction : IGeneralAction
	{
		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x060077EE RID: 30702 RVA: 0x0046146B File Offset: 0x0045F66B
		public sbyte ActionEnergyType
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x060077EF RID: 30703 RVA: 0x00461470 File Offset: 0x0045F670
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			bool hasItem = false;
			bool flag = selfChar.GetOrganizationInfo().OrgTemplateId == 16;
			if (flag)
			{
				hasItem = DomainManager.Taiwu.GetTaiwuTreasury().Inventory.Items.ContainsKey(this.BookItemKey);
			}
			bool flag2 = !hasItem;
			if (flag2)
			{
				hasItem = selfChar.GetInventory().Items.ContainsKey(this.BookItemKey);
			}
			bool flag3 = !hasItem;
			bool result;
			if (flag3)
			{
				result = false;
			}
			else
			{
				int lifeSkillIndex = selfChar.FindLearnedLifeSkillIndex(this.BookItemKey.TemplateId);
				List<LifeSkillItem> learnedLifeSkills = selfChar.GetLearnedLifeSkills();
				result = (lifeSkillIndex < 0 || !learnedLifeSkills[lifeSkillIndex].IsPageRead(this.PageId));
			}
			return result;
		}

		// Token: 0x060077F0 RID: 30704 RVA: 0x00461524 File Offset: 0x0045F724
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			Location location = selfChar.GetLocation();
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			monthlyEventCollection.AddRequestInstructionOnReadingLifeSkill(selfCharId, location, targetCharId, (ulong)this.BookItemKey, (int)(this.PageId + 1));
			CharacterDomain.AddLockMovementCharSet(selfCharId);
		}

		// Token: 0x060077F1 RID: 30705 RVA: 0x00461578 File Offset: 0x0045F778
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			bool agreeToRequest = this.AgreeToRequest;
			if (agreeToRequest)
			{
				SkillBookItem bookCfg = Config.SkillBook.Instance[this.BookItemKey.TemplateId];
				List<LifeSkillItem> learnedLifeSkills = selfChar.GetLearnedLifeSkills();
				int index = selfChar.FindLearnedLifeSkillIndex(bookCfg.LifeSkillTemplateId);
				bool flag = index < 0;
				if (flag)
				{
					selfChar.LearnNewLifeSkill(context, bookCfg.LifeSkillTemplateId, (byte)(1 << (int)this.PageId));
				}
				else
				{
					selfChar.ReadLifeSkillPage(context, index, this.PageId);
				}
				selfChar.ChangeHappiness(context, (int)(DomainManager.Item.GetBaseItem(this.BookItemKey).GetHappinessChange() / 2));
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, DomainManager.Item.GetBaseItem(this.BookItemKey).GetFavorabilityChange());
				lifeRecordCollection.AddRequestInstructionOnReadingSucceed(selfCharId, currDate, targetCharId, location, 10, this.BookItemKey.TemplateId, (int)(this.PageId + 1));
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset = secretInformationCollection.AddAcceptRequestInstructionOnReading(targetCharId, selfCharId, (ulong)this.BookItemKey);
				int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			else
			{
				selfChar.ChangeHappiness(context, -3);
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, -3000);
				lifeRecordCollection.AddRequestInstructionOnReadingFail(selfCharId, currDate, targetCharId, location, 10, this.BookItemKey.TemplateId, (int)(this.PageId + 1));
				SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset2 = secretInformationCollection2.AddRefuseRequestInstructionOnReading(targetCharId, selfCharId, (ulong)this.BookItemKey);
				int secretInfoId2 = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset2, true);
			}
		}

		// Token: 0x04002136 RID: 8502
		public ItemKey BookItemKey;

		// Token: 0x04002137 RID: 8503
		public byte PageId;

		// Token: 0x04002138 RID: 8504
		public bool AgreeToRequest;
	}
}
