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
	// Token: 0x02000881 RID: 2177
	public class BreakingDemandAction : IGeneralAction
	{
		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x060077C6 RID: 30662 RVA: 0x0045F9DD File Offset: 0x0045DBDD
		public sbyte ActionEnergyType
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x060077C7 RID: 30663 RVA: 0x0045F9E0 File Offset: 0x0045DBE0
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			GameData.Domains.CombatSkill.CombatSkill combatSkill = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(selfChar.GetId(), this.CombatSkillTemplateId));
			return !CombatSkillStateHelper.IsBrokenOut(combatSkill.GetActivationState()) && combatSkill.CanBreakout();
		}

		// Token: 0x060077C8 RID: 30664 RVA: 0x0045FA24 File Offset: 0x0045DC24
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			int selfCharId = selfChar.GetId();
			Location location = targetChar.GetLocation();
			monthlyEventCollection.AddRequestInstructionOnBreakout(selfCharId, location, targetChar.GetId(), this.CombatSkillTemplateId);
			CharacterDomain.AddLockMovementCharSet(selfCharId);
		}

		// Token: 0x060077C9 RID: 30665 RVA: 0x0045FA68 File Offset: 0x0045DC68
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			CombatSkillItem combatSkillCfg = Config.CombatSkill.Instance[this.CombatSkillTemplateId];
			SkillBookItem bookCfg = Config.SkillBook.Instance[combatSkillCfg.BookId];
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			bool agreeToRequest = this.AgreeToRequest;
			if (agreeToRequest)
			{
				GameData.Domains.CombatSkill.CombatSkill combatSkill = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(selfCharId, this.CombatSkillTemplateId));
				ushort readingState = combatSkill.GetReadingState();
				ushort activationState = CombatSkillStateHelper.GenerateRandomActivatedNormalPages(context.Random, readingState, 0);
				activationState = CombatSkillStateHelper.GenerateRandomActivatedOutlinePage(context.Random, readingState, activationState, selfChar.GetBehaviorType());
				sbyte availableStepsCount = selfChar.GetSkillBreakoutAvailableStepsCount(this.CombatSkillTemplateId);
				combatSkill.SetActivationState(activationState, context);
				combatSkill.SetBreakoutStepsCount(availableStepsCount, context);
				selfChar.ChangeHappiness(context, (int)(ItemTemplateHelper.GetBaseHappinessChange(10, combatSkillCfg.BookId) / 2));
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, ItemTemplateHelper.GetBaseFavorabilityChange(10, combatSkillCfg.BookId));
				lifeRecordCollection.AddRequestInstructionOnBreakoutSucceed(selfCharId, currDate, targetCharId, location, this.CombatSkillTemplateId);
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset = secretInformationCollection.AddAcceptRequestInstructionOnBreakout(targetCharId, selfCharId, this.CombatSkillTemplateId);
				int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			else
			{
				selfChar.ChangeHappiness(context, -3);
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, -3000);
				lifeRecordCollection.AddRequestInstructionOnBreakoutFail(selfCharId, currDate, targetCharId, location, this.CombatSkillTemplateId);
				SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset2 = secretInformationCollection2.AddRefuseRequestInstructionOnBreakout(targetCharId, selfCharId, this.CombatSkillTemplateId);
				int secretInfoId2 = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset2, true);
			}
		}

		// Token: 0x0400211A RID: 8474
		public short CombatSkillTemplateId;

		// Token: 0x0400211B RID: 8475
		public bool AgreeToRequest;
	}
}
