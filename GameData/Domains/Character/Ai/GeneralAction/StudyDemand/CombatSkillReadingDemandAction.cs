using System;
using Config;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.StudyDemand
{
	// Token: 0x02000888 RID: 2184
	public class CombatSkillReadingDemandAction : IGeneralAction
	{
		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x060077E9 RID: 30697 RVA: 0x0046115D File Offset: 0x0045F35D
		public sbyte ActionEnergyType
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x060077EA RID: 30698 RVA: 0x00461160 File Offset: 0x0045F360
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
				CombatSkillKey combatSkillKey = new CombatSkillKey(selfChar.GetId(), Config.SkillBook.Instance[this.BookItemKey.TemplateId].CombatSkillTemplateId);
				GameData.Domains.CombatSkill.CombatSkill combatSkill;
				result = (!DomainManager.CombatSkill.TryGetElement_CombatSkills(combatSkillKey, out combatSkill) || !CombatSkillStateHelper.IsPageRead(combatSkill.GetReadingState(), this.InternalIndex));
			}
			return result;
		}

		// Token: 0x060077EB RID: 30699 RVA: 0x00461228 File Offset: 0x0045F428
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			Location location = selfChar.GetLocation();
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			monthlyEventCollection.AddRequestInstructionOnReadingCombatSkill(selfCharId, location, targetCharId, (ulong)this.BookItemKey, (int)(CombatSkillStateHelper.GetPageId(this.InternalIndex) + 1), (int)this.InternalIndex);
			CharacterDomain.AddLockMovementCharSet(selfCharId);
		}

		// Token: 0x060077EC RID: 30700 RVA: 0x00461288 File Offset: 0x0045F488
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			SkillBookItem bookCfg = Config.SkillBook.Instance[this.BookItemKey.TemplateId];
			short combatSkillTemplateId = bookCfg.CombatSkillTemplateId;
			byte pageId = CombatSkillStateHelper.GetPageId(this.InternalIndex);
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			bool agreeToRequest = this.AgreeToRequest;
			if (agreeToRequest)
			{
				CombatSkillKey combatSkillKey = new CombatSkillKey(selfCharId, combatSkillTemplateId);
				GameData.Domains.CombatSkill.CombatSkill combatSkill;
				bool flag = !DomainManager.CombatSkill.TryGetElement_CombatSkills(combatSkillKey, out combatSkill);
				if (flag)
				{
					combatSkill = selfChar.LearnNewCombatSkill(context, combatSkillTemplateId, 0);
				}
				ushort readingState = CombatSkillStateHelper.SetPageRead(combatSkill.GetReadingState(), this.InternalIndex);
				combatSkill.SetReadingState(readingState, context);
				DomainManager.CombatSkill.TryActivateCombatSkillBookPageWhenSetReadingState(context, selfCharId, combatSkillTemplateId, this.InternalIndex);
				selfChar.ChangeHappiness(context, (int)(DomainManager.Item.GetBaseItem(this.BookItemKey).GetHappinessChange() / 2));
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, DomainManager.Item.GetBaseItem(this.BookItemKey).GetFavorabilityChange());
				lifeRecordCollection.AddRequestInstructionOnReadingSucceed(selfCharId, currDate, targetCharId, location, 10, this.BookItemKey.TemplateId, (int)(pageId + 1));
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset = secretInformationCollection.AddAcceptRequestInstructionOnReading(targetCharId, selfCharId, (ulong)this.BookItemKey);
				int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			else
			{
				selfChar.ChangeHappiness(context, -3);
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, -3000);
				lifeRecordCollection.AddRequestInstructionOnReadingFail(selfCharId, currDate, targetCharId, location, 10, this.BookItemKey.TemplateId, (int)(pageId + 1));
				SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset2 = secretInformationCollection2.AddRefuseRequestInstructionOnReading(targetCharId, selfCharId, (ulong)this.BookItemKey);
				int secretInfoId2 = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset2, true);
			}
		}

		// Token: 0x04002133 RID: 8499
		public ItemKey BookItemKey;

		// Token: 0x04002134 RID: 8500
		public byte InternalIndex;

		// Token: 0x04002135 RID: 8501
		public bool AgreeToRequest;
	}
}
