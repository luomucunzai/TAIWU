using Config;
using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;

namespace GameData.Domains.Character.Ai.GeneralAction.TeachRandom;

public class TeachLifeSkillAction : IGeneralAction
{
	public short SkillTemplateId;

	public byte PageId;

	public bool Succeed;

	public sbyte ActionEnergyType => 4;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		return targetChar.FindLearnedLifeSkillIndex(SkillTemplateId) < 0;
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		Location location = selfChar.GetLocation();
		if (Succeed)
		{
			DomainManager.World.GetMonthlyNotificationCollection().AddTeachLifeSkillSuccess(id, location, id2, SkillTemplateId);
		}
		else
		{
			DomainManager.World.GetMonthlyNotificationCollection().AddTeachLifeSkillFailure(id, location, id2, SkillTemplateId);
		}
		ApplyChanges(context, selfChar, targetChar);
	}

	public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		Location location = selfChar.GetLocation();
		int currDate = DomainManager.World.GetCurrDate();
		int id = targetChar.GetId();
		int id2 = selfChar.GetId();
		short skillBookId = LifeSkill.Instance[SkillTemplateId].SkillBookId;
		SkillBookItem skillBookItem = Config.SkillBook.Instance[skillBookId];
		if (Succeed)
		{
			lifeRecordCollection.AddLearnLifeSkillWithInstructionSucceed(id, currDate, id2, location, 10, skillBookId, PageId + 1);
			if (id == DomainManager.Taiwu.GetTaiwuCharId())
			{
				ItemKey itemKey = DomainManager.Item.CreateDemandedSkillBook(context, skillBookId, PageId, 0);
				targetChar.AddInventoryItem(context, itemKey, 1);
			}
			LifeSkillItem lifeSkillItem = targetChar.LearnNewLifeSkill(context, SkillTemplateId, (byte)(1 << (int)PageId));
			targetChar.ChangeHappiness(context, ItemTemplateHelper.GetBaseHappinessChange(10, SkillTemplateId));
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, targetChar, selfChar, ItemTemplateHelper.GetBaseFavorabilityChange(10, SkillTemplateId));
		}
		else
		{
			lifeRecordCollection.AddLearnLifeSkillWithInstructionFail(id, currDate, id2, location, 10, skillBookId, PageId + 1);
		}
		SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
		int dataOffset = secretInformationCollection.AddInstructOnLifeSkill(id2, id, SkillTemplateId);
		int num = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
	}
}
