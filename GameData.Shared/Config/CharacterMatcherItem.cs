using System;
using Config.Common;

namespace Config;

[Serializable]
public class CharacterMatcherItem : ConfigItem<CharacterMatcherItem, byte>
{
	public readonly byte TemplateId;

	public readonly ECharacterMatcherAgeType AgeType;

	public readonly ECharacterMatcherIdentityType IdentityType;

	public readonly ECharacterMatcherGenderType GenderType;

	public readonly sbyte Organization;

	public readonly ECharacterMatcherSubCondition[] SubConditions;

	public CharacterMatcherItem(byte templateId, ECharacterMatcherAgeType ageType, ECharacterMatcherIdentityType identityType, ECharacterMatcherGenderType genderType, sbyte organization, ECharacterMatcherSubCondition[] subConditions)
	{
		TemplateId = templateId;
		AgeType = ageType;
		IdentityType = identityType;
		GenderType = genderType;
		Organization = organization;
		SubConditions = subConditions;
	}

	public CharacterMatcherItem()
	{
		TemplateId = 0;
		AgeType = ECharacterMatcherAgeType.NotRestricted;
		IdentityType = ECharacterMatcherIdentityType.NotRestricted;
		GenderType = ECharacterMatcherGenderType.NotRestricted;
		Organization = 0;
		SubConditions = null;
	}

	public CharacterMatcherItem(byte templateId, CharacterMatcherItem other)
	{
		TemplateId = templateId;
		AgeType = other.AgeType;
		IdentityType = other.IdentityType;
		GenderType = other.GenderType;
		Organization = other.Organization;
		SubConditions = other.SubConditions;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override CharacterMatcherItem Duplicate(int templateId)
	{
		return new CharacterMatcherItem((byte)templateId, this);
	}
}
