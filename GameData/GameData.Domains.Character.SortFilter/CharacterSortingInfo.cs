using System.Text;
using GameData.Domains.Character.Relation;
using GameData.Domains.Combat;

namespace GameData.Domains.Character.SortFilter;

public class CharacterSortingInfo
{
	public Character Character;

	public byte[] NameBytes;

	public float HealthPercentage;

	public short FavorabilityToTaiwu;

	public int KidnappedCharCount;

	public int AttackMedal;

	public int DefenseMedal;

	public int WisdomMedal;

	public sbyte DefeatMarkCount;

	public sbyte CombatSkillAgeAdjust;

	public sbyte LifeSkillAgeAdjust;

	public short Morality;

	public sbyte Fame;

	public short Attraction;

	public HitOrAvoidInts HitValues;

	public HitOrAvoidInts AvoidValues;

	public sbyte BehaviorType;

	public sbyte FameType;

	public sbyte FavorType;

	public sbyte AttractionType;

	public sbyte HappinessType;

	public byte LivingStatus;

	public byte WorkStatus;

	public sbyte VillagerNeedWaitTime;

	public CharacterSortingInfo(Character character, Encoding encoding, CharacterSortFilter characterSortFilter = null)
	{
		Character = character;
		int id = character.GetId();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		FavorabilityToTaiwu = DomainManager.Character.GetFavorability(id, taiwuCharId);
		(string surname, string givenName) monasticTitleOrDisplayName = DomainManager.Character.GetMonasticTitleOrDisplayName(id);
		string item = monasticTitleOrDisplayName.surname;
		string item2 = monasticTitleOrDisplayName.givenName;
		NameBytes = encoding.GetBytes(item + item2);
		HitValues = character.GetHitValues();
		AvoidValues = character.GetAvoidValues();
		Morality = character.GetMorality();
		Fame = character.GetFame();
		Attraction = character.GetAttraction();
		HealthPercentage = (float)character.GetHealth() / (float)character.GetLeftMaxHealth();
		KidnappedCharCount = (character.IsActiveExternalRelationState(2) ? DomainManager.Character.GetKidnappedCharacters(id).GetCount() : 0);
		AttackMedal = character.GetFeatureMedalValue(0);
		DefenseMedal = character.GetFeatureMedalValue(1);
		WisdomMedal = character.GetFeatureMedalValue(2);
		DefeatMarkCount = (sbyte)CombatDomain.GetDefeatMarksCountOutOfCombat(character);
		LifeSkillAgeAdjust = character.GetLifeSkillQualificationAgeAdjust();
		CombatSkillAgeAdjust = character.GetCombatSkillQualificationAgeAdjust();
		BehaviorType = GameData.Domains.Character.BehaviorType.GetBehaviorType(Morality);
		FavorType = FavorabilityType.GetFavorabilityType(FavorabilityToTaiwu);
		AttractionType = GameData.Domains.Character.AttractionType.GetAttractionType(Attraction);
		HappinessType = character.GetHappiness();
		FameType = character.GetFameType();
		if (Character.GetOrganizationInfo().OrgTemplateId == 16)
		{
			WorkStatus = DomainManager.Taiwu.GetVillagerWorkStatus(Character);
			LivingStatus = DomainManager.Building.GetLivingStatus(Character.GetId());
			VillagerNeedWaitTime = DomainManager.Taiwu.GetVillagerNeedWaitTime(id, characterSortFilter);
		}
	}

	public int GetId()
	{
		return Character.GetId();
	}
}
