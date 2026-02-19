using Config;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.StudyDemand;

public class CombatSkillReadingDemandAction : IGeneralAction
{
	public ItemKey BookItemKey;

	public byte InternalIndex;

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
		CombatSkillKey objectId = new CombatSkillKey(selfChar.GetId(), Config.SkillBook.Instance[BookItemKey.TemplateId].CombatSkillTemplateId);
		GameData.Domains.CombatSkill.CombatSkill element;
		return !DomainManager.CombatSkill.TryGetElement_CombatSkills(objectId, out element) || !CombatSkillStateHelper.IsPageRead(element.GetReadingState(), InternalIndex);
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		Location location = selfChar.GetLocation();
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		monthlyEventCollection.AddRequestInstructionOnReadingCombatSkill(id, location, id2, (ulong)BookItemKey, CombatSkillStateHelper.GetPageId(InternalIndex) + 1, InternalIndex);
		CharacterDomain.AddLockMovementCharSet(id);
	}

	public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		SkillBookItem skillBookItem = Config.SkillBook.Instance[BookItemKey.TemplateId];
		short combatSkillTemplateId = skillBookItem.CombatSkillTemplateId;
		byte pageId = CombatSkillStateHelper.GetPageId(InternalIndex);
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		if (AgreeToRequest)
		{
			CombatSkillKey objectId = new CombatSkillKey(id, combatSkillTemplateId);
			if (!DomainManager.CombatSkill.TryGetElement_CombatSkills(objectId, out var element))
			{
				element = selfChar.LearnNewCombatSkill(context, combatSkillTemplateId, 0);
			}
			ushort readingState = CombatSkillStateHelper.SetPageRead(element.GetReadingState(), InternalIndex);
			element.SetReadingState(readingState, context);
			DomainManager.CombatSkill.TryActivateCombatSkillBookPageWhenSetReadingState(context, id, combatSkillTemplateId, InternalIndex);
			selfChar.ChangeHappiness(context, DomainManager.Item.GetBaseItem(BookItemKey).GetHappinessChange() / 2);
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, DomainManager.Item.GetBaseItem(BookItemKey).GetFavorabilityChange());
			lifeRecordCollection.AddRequestInstructionOnReadingSucceed(id, currDate, id2, location, 10, BookItemKey.TemplateId, pageId + 1);
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddAcceptRequestInstructionOnReading(id2, id, (ulong)BookItemKey);
			int num = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		}
		else
		{
			selfChar.ChangeHappiness(context, -3);
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, -3000);
			lifeRecordCollection.AddRequestInstructionOnReadingFail(id, currDate, id2, location, 10, BookItemKey.TemplateId, pageId + 1);
			SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset2 = secretInformationCollection2.AddRefuseRequestInstructionOnReading(id2, id, (ulong)BookItemKey);
			int num2 = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset2);
		}
	}
}
