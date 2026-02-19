using System;
using System.Collections.Generic;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Global.Inscription;

namespace GameData.Domains.Character.Creation;

[Serializable]
public class ProtagonistCreationInfo
{
	public string Surname;

	public string GivenName;

	public short Morality;

	public sbyte Gender;

	public short Age;

	public sbyte BirthMonth;

	public AvatarData Avatar;

	public AvatarExtraData AvatarExtraData;

	public short ClothingTemplateId;

	public List<short> ProtagonistFeatureIds;

	public sbyte TaiwuVillageStateTemplateId;

	public InscribedCharacter InscribedChar;
}
