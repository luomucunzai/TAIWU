using Config;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.StudyDemand;

public class RequestCombatSkillDemandAction : IGeneralAction
{
	public short BookTemplateId;

	public byte InternalIndex;

	public byte GeneratedPageTypes;

	public bool AgreeToRequest;

	public bool Succeed;

	public sbyte ActionEnergyType => 2;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		return !selfChar.GetLearnedCombatSkills().Contains(Config.SkillBook.Instance[BookTemplateId].CombatSkillTemplateId);
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		int id = selfChar.GetId();
		Location location = targetChar.GetLocation();
		monthlyEventCollection.AddRequestInstructionOnCombatSkill(id, location, targetChar.GetId(), 10, BookTemplateId, CombatSkillStateHelper.GetPageId(InternalIndex) + 1, InternalIndex, GeneratedPageTypes);
		CharacterDomain.AddLockMovementCharSet(id);
	}

	public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		SkillBookItem skillBookItem = Config.SkillBook.Instance[BookTemplateId];
		short combatSkillTemplateId = skillBookItem.CombatSkillTemplateId;
		byte pageId = CombatSkillStateHelper.GetPageId(InternalIndex);
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		if (AgreeToRequest)
		{
			if (Succeed)
			{
				int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
				if (id == taiwuCharId)
				{
					ItemKey itemKey = DomainManager.Item.CreateDemandedSkillBook(context, BookTemplateId, InternalIndex, GeneratedPageTypes);
					selfChar.AddInventoryItem(context, itemKey, 1);
					ProfessionFormulaItem formulaCfg = ProfessionFormula.Instance[50];
					int baseDelta = formulaCfg.Calculate(Config.CombatSkill.Instance[combatSkillTemplateId].Grade);
					DomainManager.Extra.ChangeProfessionSeniority(context, 7, baseDelta);
				}
				else if (id2 == taiwuCharId)
				{
					ProfessionFormulaItem formulaCfg2 = ProfessionFormula.Instance[53];
					int baseDelta2 = formulaCfg2.Calculate(Config.CombatSkill.Instance[combatSkillTemplateId].Grade);
					DomainManager.Extra.ChangeProfessionSeniority(context, 7, baseDelta2);
				}
				selfChar.LearnNewCombatSkill(context, combatSkillTemplateId, (ushort)(1 << (int)InternalIndex));
				selfChar.ChangeHappiness(context, ItemTemplateHelper.GetBaseHappinessChange(10, BookTemplateId) / 2);
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, ItemTemplateHelper.GetBaseFavorabilityChange(10, BookTemplateId));
				lifeRecordCollection.AddRequestInstructionOnCombatSkillSucceed(id, currDate, id2, location, 10, BookTemplateId, pageId + 1);
			}
			else
			{
				lifeRecordCollection.AddRequestInstructionOnCombatSkillFailToLearn(id, currDate, id2, location, 10, BookTemplateId, pageId + 1);
			}
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddInstructOnCombatSkill(id2, id, skillBookItem.CombatSkillTemplateId);
			int num = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
			dataOffset = secretInformationCollection.AddAcceptRequestInstructionOnCombatSkill(id2, id, skillBookItem.CombatSkillTemplateId);
			num = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		}
		else
		{
			selfChar.ChangeHappiness(context, -3);
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, -3000);
			lifeRecordCollection.AddRequestInstructionOnCombatSkillFail(id, currDate, id2, location, 10, BookTemplateId, pageId + 1);
			SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset2 = secretInformationCollection2.AddRefuseRequestInstructionOnCombatSkill(id2, id, skillBookItem.CombatSkillTemplateId);
			int num2 = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset2);
		}
	}
}
