using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Character.Relation;
using GameData.Domains.Taiwu.Profession;

namespace GameData.Domains.Organization;

public abstract class SettlementCharacter : BaseGameDataObject
{
	[CollectionObjectField(false, true, false, true, false)]
	protected int Id;

	[CollectionObjectField(false, true, false, true, false)]
	protected sbyte OrgTemplateId;

	[CollectionObjectField(false, true, false, false, false)]
	protected short SettlementId;

	[CollectionObjectField(false, true, false, false, false)]
	protected bool ApprovedTaiwu;

	[CollectionObjectField(false, true, false, false, false)]
	protected short InfluencePower;

	[CollectionObjectField(false, true, false, false, false)]
	protected short InfluencePowerBonus;

	protected SettlementCharacter()
	{
	}

	protected SettlementCharacter(int charId, sbyte orgTemplateId, short settlementId)
	{
		Id = charId;
		OrgTemplateId = orgTemplateId;
		SettlementId = settlementId;
		ApprovedTaiwu = false;
	}

	public void SetApprovedTaiwu(DataContext context, bool approve)
	{
		if (ApprovedTaiwu != approve)
		{
			SetApprovedTaiwu(approve, context);
			Events.RaiseCharacterApproveTaiwuStatusChanged(context, this, approve);
		}
	}

	public short GetApprovingRate()
	{
		if (!ApprovedTaiwu && !ProfessionSkillHandle.DukeSkill_CheckCharacterHasTitle(Id))
		{
			return 0;
		}
		if (DomainManager.Organization.GetPrisonerSect(Id) >= 0)
		{
			return 0;
		}
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		if (!DomainManager.Character.TryGetRelation(Id, taiwuCharId, out var relation))
		{
			return 0;
		}
		if ((relation.RelationType & 0x8000) != 0)
		{
			return 0;
		}
		sbyte favorabilityType = FavorabilityType.GetFavorabilityType(relation.Favorability);
		int num = InfluencePower * (60 + 10 * favorabilityType) / 100;
		return (short)num;
	}

	public short CalcInfluencePower(GameData.Domains.Character.Character character, short baseInfluencePower, Dictionary<int, (GameData.Domains.Character.Character character, short baseInfluencePower)> baseInfluencePowers, HashSet<int> relatedCharIds)
	{
		if (character.IsCompletelyInfected() || character.GetLegendaryBookOwnerState() >= 2)
		{
			return 0;
		}
		int num = baseInfluencePower + InfluencePowerBonus;
		relatedCharIds.Clear();
		DomainManager.Character.GetAllRelatedCharIds(Id, relatedCharIds);
		foreach (int relatedCharId in relatedCharIds)
		{
			if (baseInfluencePowers.TryGetValue(relatedCharId, out (GameData.Domains.Character.Character, short) value) && DomainManager.Character.IsCharacterAlive(relatedCharId))
			{
				RelatedCharacter relation = DomainManager.Character.GetRelation(relatedCharId, Id);
				ushort relationType = relation.RelationType;
				short favorability = relation.Favorability;
				if ((favorability > 0 && RelationType.ContainPositiveRelations(relationType)) || (favorability < 0 && RelationType.ContainNegativeRelations(relationType)))
				{
					num += value.Item2 * favorability / 300000;
				}
			}
		}
		return (short)num;
	}

	public int GetId()
	{
		return Id;
	}

	public sbyte GetOrgTemplateId()
	{
		return OrgTemplateId;
	}

	public short GetSettlementId()
	{
		return SettlementId;
	}

	public abstract void SetSettlementId(short settlementId, DataContext context);

	public bool GetApprovedTaiwu()
	{
		return ApprovedTaiwu;
	}

	public abstract void SetApprovedTaiwu(bool approvedTaiwu, DataContext context);

	public short GetInfluencePower()
	{
		return InfluencePower;
	}

	public abstract void SetInfluencePower(short influencePower, DataContext context);

	public short GetInfluencePowerBonus()
	{
		return InfluencePowerBonus;
	}

	public abstract void SetInfluencePowerBonus(short influencePowerBonus, DataContext context);
}
