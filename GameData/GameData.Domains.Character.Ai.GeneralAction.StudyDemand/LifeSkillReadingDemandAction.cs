using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.StudyDemand;

public class LifeSkillReadingDemandAction : IGeneralAction
{
	public ItemKey BookItemKey;

	public byte PageId;

	public bool AgreeToRequest;

	public sbyte ActionEnergyType => 2;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		bool flag = false;
		if (selfChar.GetOrganizationInfo().OrgTemplateId == 16)
		{
			flag = DomainManager.Taiwu.GetTaiwuTreasury().Inventory.Items.ContainsKey(BookItemKey);
		}
		if (!flag)
		{
			flag = selfChar.GetInventory().Items.ContainsKey(BookItemKey);
		}
		if (!flag)
		{
			return false;
		}
		int num = selfChar.FindLearnedLifeSkillIndex(BookItemKey.TemplateId);
		List<LifeSkillItem> learnedLifeSkills = selfChar.GetLearnedLifeSkills();
		return num < 0 || !learnedLifeSkills[num].IsPageRead(PageId);
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		Location location = selfChar.GetLocation();
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		monthlyEventCollection.AddRequestInstructionOnReadingLifeSkill(id, location, id2, (ulong)BookItemKey, PageId + 1);
		CharacterDomain.AddLockMovementCharSet(id);
	}

	public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		if (AgreeToRequest)
		{
			SkillBookItem skillBookItem = Config.SkillBook.Instance[BookItemKey.TemplateId];
			List<LifeSkillItem> learnedLifeSkills = selfChar.GetLearnedLifeSkills();
			int num = selfChar.FindLearnedLifeSkillIndex(skillBookItem.LifeSkillTemplateId);
			if (num < 0)
			{
				selfChar.LearnNewLifeSkill(context, skillBookItem.LifeSkillTemplateId, (byte)(1 << (int)PageId));
			}
			else
			{
				selfChar.ReadLifeSkillPage(context, num, PageId);
			}
			selfChar.ChangeHappiness(context, DomainManager.Item.GetBaseItem(BookItemKey).GetHappinessChange() / 2);
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, DomainManager.Item.GetBaseItem(BookItemKey).GetFavorabilityChange());
			lifeRecordCollection.AddRequestInstructionOnReadingSucceed(id, currDate, id2, location, 10, BookItemKey.TemplateId, PageId + 1);
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddAcceptRequestInstructionOnReading(id2, id, (ulong)BookItemKey);
			int num2 = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		}
		else
		{
			selfChar.ChangeHappiness(context, -3);
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, -3000);
			lifeRecordCollection.AddRequestInstructionOnReadingFail(id, currDate, id2, location, 10, BookItemKey.TemplateId, PageId + 1);
			SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset2 = secretInformationCollection2.AddRefuseRequestInstructionOnReading(id2, id, (ulong)BookItemKey);
			int num3 = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset2);
		}
	}
}
