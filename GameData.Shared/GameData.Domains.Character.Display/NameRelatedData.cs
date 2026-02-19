using System.Collections.Generic;
using Config;
using GameData.Serializer;

namespace GameData.Domains.Character.Display;

public struct NameRelatedData : ISerializableGameData
{
	[SerializableGameDataField]
	public short CharTemplateId;

	[SerializableGameDataField]
	public sbyte Gender;

	[SerializableGameDataField]
	public byte MonkType;

	[SerializableGameDataField]
	public FullName FullName;

	[SerializableGameDataField]
	public sbyte OrgTemplateId;

	[SerializableGameDataField]
	public sbyte OrgGrade;

	[SerializableGameDataField]
	public MonasticTitle MonasticTitle;

	[SerializableGameDataField]
	public int CustomDisplayNameId;

	[SerializableGameDataField]
	public int NickNameId;

	[SerializableGameDataField]
	public int ExtraNameTextTemplateId;

	public NameRelatedData()
	{
		CharTemplateId = 0;
		Gender = 0;
		MonkType = 0;
		FullName = default(FullName);
		OrgTemplateId = 0;
		OrgGrade = 0;
		MonasticTitle = default(MonasticTitle);
		CustomDisplayNameId = 0;
		NickNameId = -1;
		ExtraNameTextTemplateId = -1;
	}

	public (string surname, string givenName) GetMonasticTitleOrDisplayName(bool isTaiwu)
	{
		return GetMonasticTitleOrDisplayNameDetailed(isTaiwu, ignoreNickName: false);
	}

	public (string surname, string givenName) GetMonasticTitleOrDisplayNameDetailed(bool isTaiwu, bool ignoreNickName)
	{
		string nickName = GetNickName();
		if (!ignoreNickName && nickName != null)
		{
			return (surname: null, givenName: nickName);
		}
		string extraName = GetExtraName();
		if (extraName != null)
		{
			return (surname: null, givenName: extraName);
		}
		string monasticTitle = GetMonasticTitle(isTaiwu);
		if (!string.IsNullOrEmpty(monasticTitle))
		{
			return (surname: null, givenName: monasticTitle);
		}
		return GetDisplayNameDetailed(isTaiwu, ignoreNickName);
	}

	public string GetNickName()
	{
		IReadOnlyDictionary<int, string> customTexts = GetCustomTexts();
		if (NickNameId >= 0 && customTexts.TryGetValue(NickNameId, out var value))
		{
			return value;
		}
		return null;
	}

	private string GetExtraName()
	{
		if (ExtraNameTextTemplateId >= 0)
		{
			return ExtraNameText.Instance[ExtraNameTextTemplateId].Content;
		}
		return null;
	}

	public (string surname, string givenName) GetRealName()
	{
		if (CharTemplateId < 0)
		{
			return (surname: null, givenName: ExtraNameText.Instance[5].Content);
		}
		CharacterItem characterItem = Config.Character.Instance[CharTemplateId];
		if (FullName.Type == 0)
		{
			return (surname: characterItem.Surname, givenName: characterItem.GivenName);
		}
		var (item, item2) = FullName.GetName(Gender, GetCustomTexts());
		return (surname: item, givenName: item2);
	}

	public (string surname, string givenName) GetDisplayName(bool isTaiwu)
	{
		return GetDisplayNameDetailed(isTaiwu, ignoreNickName: false);
	}

	public (string surname, string givenName) GetDisplayNameDetailed(bool isTaiwu, bool ignoreNickName)
	{
		if (CharTemplateId < 0)
		{
			return (surname: null, givenName: ExtraNameText.Instance[5].Content);
		}
		string nickName = GetNickName();
		if (!ignoreNickName && nickName != null)
		{
			return (surname: null, givenName: nickName);
		}
		string extraName = GetExtraName();
		if (extraName != null)
		{
			return (surname: null, givenName: extraName);
		}
		CharacterItem characterItem = Config.Character.Instance[CharTemplateId];
		if (FullName.Type == 0)
		{
			return (surname: characterItem.Surname, givenName: characterItem.GivenName);
		}
		IReadOnlyDictionary<int, string> customTexts = GetCustomTexts();
		var (item, item2) = FullName.GetName(Gender, customTexts);
		if (isTaiwu && ShowTaiwuSurname())
		{
			item = ExtraNameText.Instance[0].Content;
		}
		short index = Config.Organization.Instance[OrgTemplateId].Members[OrgGrade];
		OrganizationMemberItem organizationMemberItem = OrganizationMember.Instance[index];
		if (organizationMemberItem.SurnameId >= 0)
		{
			item = LocalSurnames.Instance.SurnameCore[organizationMemberItem.SurnameId].Surname;
		}
		if (CustomDisplayNameId >= 0 && customTexts.TryGetValue(CustomDisplayNameId, out var value))
		{
			item2 = value;
		}
		return (surname: item, givenName: item2);
	}

	public string GetMonasticTitle(bool isTaiwu)
	{
		if (MonkType == 0)
		{
			return null;
		}
		if ((MonkType & 0x80) != 0)
		{
			if (MonasticTitle.SeniorityId < 0 || MonasticTitle.SuffixId < 0)
			{
				return null;
			}
			MonasticTitleItem[] monasticTitles = LocalMonasticTitles.Instance.MonasticTitles;
			string name = monasticTitles[MonasticTitle.SeniorityId].Name;
			string name2 = monasticTitles[MonasticTitle.SuffixId].Name;
			short index = Config.Organization.Instance[OrgTemplateId].Members[OrgGrade];
			string text = OrganizationMember.Instance[index].MonasticTitleSuffixes[Gender];
			return name + name2 + text;
		}
		(string surname, string givenName) displayName = GetDisplayName(isTaiwu);
		string item = displayName.surname;
		string item2 = displayName.givenName;
		string obj = ((!string.IsNullOrEmpty(item)) ? item : item2);
		ExtraNameTextItem extraNameTextItem = (((MonkType & 1) == 0) ? ((Gender == 1) ? ExtraNameText.Instance[4] : ExtraNameText.Instance[3]) : ((Gender == 1) ? ExtraNameText.Instance[2] : ExtraNameText.Instance[1]));
		return obj + extraNameTextItem.Content;
	}

	private bool ShowTaiwuSurname()
	{
		if (ExternalDataBridge.Context.HideTaiwuOriginalSurname)
		{
			return ExternalDataBridge.Context.GetWorldFunctionsStatus(26);
		}
		return false;
	}

	private IReadOnlyDictionary<int, string> GetCustomTexts()
	{
		return ExternalDataBridge.Context.CustomTexts;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 32;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = CharTemplateId;
		ptr += 2;
		*ptr = (byte)Gender;
		ptr++;
		*ptr = MonkType;
		ptr++;
		ptr += FullName.Serialize(ptr);
		*ptr = (byte)OrgTemplateId;
		ptr++;
		*ptr = (byte)OrgGrade;
		ptr++;
		ptr += MonasticTitle.Serialize(ptr);
		*(int*)ptr = CustomDisplayNameId;
		ptr += 4;
		*(int*)ptr = NickNameId;
		ptr += 4;
		*(int*)ptr = ExtraNameTextTemplateId;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		CharTemplateId = *(short*)ptr;
		ptr += 2;
		Gender = (sbyte)(*ptr);
		ptr++;
		MonkType = *ptr;
		ptr++;
		ptr += FullName.Deserialize(ptr);
		OrgTemplateId = (sbyte)(*ptr);
		ptr++;
		OrgGrade = (sbyte)(*ptr);
		ptr++;
		ptr += MonasticTitle.Deserialize(ptr);
		CustomDisplayNameId = *(int*)ptr;
		ptr += 4;
		NickNameId = *(int*)ptr;
		ptr += 4;
		ExtraNameTextTemplateId = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
