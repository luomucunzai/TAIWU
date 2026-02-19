using Config;
using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.StudyDemand;

public class RequestLifeSkillDemandAction : IGeneralAction
{
	public short BookTemplateId;

	public byte PageId;

	public bool AgreeToRequest;

	public bool Succeed;

	public sbyte ActionEnergyType => 2;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		return selfChar.FindLearnedLifeSkillIndex(Config.SkillBook.Instance[BookTemplateId].LifeSkillTemplateId) < 0;
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		Location location = selfChar.GetLocation();
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		monthlyEventCollection.AddRequestInstructionOnLifeSkill(id, location, id2, 10, BookTemplateId, PageId + 1);
		CharacterDomain.AddLockMovementCharSet(id);
	}

	public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		SkillBookItem skillBookItem = Config.SkillBook.Instance[BookTemplateId];
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
					ItemKey itemKey = DomainManager.Item.CreateDemandedSkillBook(context, BookTemplateId, PageId, 0);
					selfChar.AddInventoryItem(context, itemKey, 1);
					ProfessionFormulaItem formulaCfg = ProfessionFormula.Instance[101];
					int baseDelta = formulaCfg.Calculate(LifeSkill.Instance[skillBookItem.LifeSkillTemplateId].Grade);
					DomainManager.Extra.ChangeProfessionSeniority(context, 16, baseDelta);
				}
				else if (id2 == taiwuCharId)
				{
					ProfessionFormulaItem formulaCfg2 = ProfessionFormula.Instance[104];
					int baseDelta2 = formulaCfg2.Calculate(LifeSkill.Instance[skillBookItem.LifeSkillTemplateId].Grade);
					DomainManager.Extra.ChangeProfessionSeniority(context, 16, baseDelta2);
				}
				selfChar.LearnNewLifeSkill(context, skillBookItem.LifeSkillTemplateId, (byte)(1 << (int)PageId));
				selfChar.ChangeHappiness(context, ItemTemplateHelper.GetBaseHappinessChange(10, BookTemplateId) / 2);
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, ItemTemplateHelper.GetBaseFavorabilityChange(10, BookTemplateId));
				lifeRecordCollection.AddRequestInstructionOnLifeSkillSucceed(id, currDate, id2, location, 10, BookTemplateId, PageId + 1);
			}
			else
			{
				lifeRecordCollection.AddRequestInstructionOnLifeSkillFailToLearn(id, currDate, id2, location, 10, BookTemplateId, PageId + 1);
			}
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddInstructOnLifeSkill(id2, id, skillBookItem.LifeSkillTemplateId);
			int num = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
			dataOffset = secretInformationCollection.AddAcceptRequestInstructionOnLifeSkill(id2, id, skillBookItem.LifeSkillTemplateId);
			num = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		}
		else
		{
			selfChar.ChangeHappiness(context, -3);
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, -3000);
			lifeRecordCollection.AddRequestInstructionOnLifeSkillFail(id, currDate, id2, location, 10, BookTemplateId, PageId + 1);
			SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset2 = secretInformationCollection2.AddRefuseRequestInstructionOnLifeSkill(id2, id, skillBookItem.LifeSkillTemplateId);
			int num2 = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset2);
		}
	}
}
