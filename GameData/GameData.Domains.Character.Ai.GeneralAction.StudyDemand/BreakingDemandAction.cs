using Config;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.StudyDemand;

public class BreakingDemandAction : IGeneralAction
{
	public short CombatSkillTemplateId;

	public bool AgreeToRequest;

	public sbyte ActionEnergyType => 2;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(selfChar.GetId(), CombatSkillTemplateId));
		return !CombatSkillStateHelper.IsBrokenOut(element_CombatSkills.GetActivationState()) && element_CombatSkills.CanBreakout();
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		int id = selfChar.GetId();
		Location location = targetChar.GetLocation();
		monthlyEventCollection.AddRequestInstructionOnBreakout(id, location, targetChar.GetId(), CombatSkillTemplateId);
		CharacterDomain.AddLockMovementCharSet(id);
	}

	public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[CombatSkillTemplateId];
		SkillBookItem skillBookItem = Config.SkillBook.Instance[combatSkillItem.BookId];
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		if (AgreeToRequest)
		{
			GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(id, CombatSkillTemplateId));
			ushort readingState = element_CombatSkills.GetReadingState();
			ushort activationState = CombatSkillStateHelper.GenerateRandomActivatedNormalPages(context.Random, readingState, 0);
			activationState = CombatSkillStateHelper.GenerateRandomActivatedOutlinePage(context.Random, readingState, activationState, selfChar.GetBehaviorType());
			sbyte skillBreakoutAvailableStepsCount = selfChar.GetSkillBreakoutAvailableStepsCount(CombatSkillTemplateId);
			element_CombatSkills.SetActivationState(activationState, context);
			element_CombatSkills.SetBreakoutStepsCount(skillBreakoutAvailableStepsCount, context);
			selfChar.ChangeHappiness(context, ItemTemplateHelper.GetBaseHappinessChange(10, combatSkillItem.BookId) / 2);
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, ItemTemplateHelper.GetBaseFavorabilityChange(10, combatSkillItem.BookId));
			lifeRecordCollection.AddRequestInstructionOnBreakoutSucceed(id, currDate, id2, location, CombatSkillTemplateId);
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddAcceptRequestInstructionOnBreakout(id2, id, CombatSkillTemplateId);
			int num = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		}
		else
		{
			selfChar.ChangeHappiness(context, -3);
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, -3000);
			lifeRecordCollection.AddRequestInstructionOnBreakoutFail(id, currDate, id2, location, CombatSkillTemplateId);
			SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset2 = secretInformationCollection2.AddRefuseRequestInstructionOnBreakout(id2, id, CombatSkillTemplateId);
			int num2 = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset2);
		}
	}
}
