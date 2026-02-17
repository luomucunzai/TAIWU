using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Domains;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.Display;
using HarmonyLib;

// Token: 0x02000006 RID: 6
[NullableContext(1)]
[Nullable(0)]
internal class Util
{
	// Token: 0x06000008 RID: 8 RVA: 0x00002854 File Offset: 0x00000A54
	[return: Nullable(2)]
	public static GameData.Domains.Character.Character getCharacter(string charNameOrId)
	{
		bool flag = charNameOrId == null || charNameOrId.Length == 0;
		GameData.Domains.Character.Character result;
		if (flag)
		{
			result = null;
		}
		else
		{
			int id;
			bool flag2 = !int.TryParse(charNameOrId, out id);
			if (flag2)
			{
				foreach (GameData.Domains.Character.Character character in ((Dictionary<int, GameData.Domains.Character.Character>)AccessTools.Field(typeof(CharacterDomain), "_objects").GetValue(DomainManager.Character)).Values)
				{
					try
					{
						NameRelatedData nameRelatedData = new NameRelatedData
						{
							CharTemplateId = character.GetTemplateId(),
							FullName = character.GetFullName(),
							Gender = character.GetGender(),
							MonasticTitle = character.GetMonasticTitle(),
							MonkType = character.GetMonkType(),
							OrgGrade = character.GetOrganizationInfo().Grade,
							OrgTemplateId = character.GetOrganizationInfo().OrgTemplateId
						};
						string name_UIShow = Util.GetName_UIShow(ref nameRelatedData, character.GetId() == DomainManager.Taiwu.GetTaiwuCharId(), true);
						string name_UIShow2 = Util.GetName_UIShow(ref nameRelatedData, character.GetId() == DomainManager.Taiwu.GetTaiwuCharId(), false);
						bool flag3 = charNameOrId.Equals(name_UIShow) || charNameOrId.Equals(name_UIShow2);
						if (flag3)
						{
							id = character.GetId();
							break;
						}
					}
					catch (Exception ex)
					{
					}
				}
			}
			GameData.Domains.Character.Character character2;
			result = ((id > -1 && DomainManager.Character.TryGetElement_Objects(id, out character2)) ? character2 : null);
		}
		return result;
	}

	// Token: 0x06000009 RID: 9 RVA: 0x00002A08 File Offset: 0x00000C08
	public unsafe static void SetCombatSkillQualificationGrowthType(GameData.Domains.Character.Character character, sbyte growthType, DataContext context)
	{
		AccessTools.Field(typeof(GameData.Domains.Character.Character), "_combatSkillQualificationGrowthType").SetValue(character, growthType);
		AccessTools.Method(typeof(GameData.Domains.Character.Character), "SetModifiedAndInvalidateInfluencedCache", new Type[]
		{
			typeof(ushort),
			typeof(DataContext)
		}, null).Invoke(character, new object[]
		{
			33,
			context
		});
		bool flag = !character.CollectionHelperData.IsArchive;
		if (!flag)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField<int>(character.CollectionHelperData.DomainId, character.CollectionHelperData.DataId, character.GetId(), 134U, 1);
			*ptr = (byte)growthType;
			byte* ptr2 = ptr + 1;
		}
	}

	// Token: 0x0600000A RID: 10 RVA: 0x00002ACC File Offset: 0x00000CCC
	public unsafe static void SetLifeSkillQualificationGrowthType(GameData.Domains.Character.Character character, sbyte growthType, DataContext context)
	{
		AccessTools.Field(typeof(GameData.Domains.Character.Character), "_lifeSkillQualificationGrowthType").SetValue(character, growthType);
		AccessTools.Method(typeof(GameData.Domains.Character.Character), "SetModifiedAndInvalidateInfluencedCache", new Type[]
		{
			typeof(ushort),
			typeof(DataContext)
		}, null).Invoke(character, new object[]
		{
			31,
			context
		});
		bool flag = !character.CollectionHelperData.IsArchive;
		if (!flag)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField<int>(character.CollectionHelperData.DomainId, character.CollectionHelperData.DataId, character.GetId(), 105U, 1);
			*ptr = (byte)growthType;
			byte* ptr2 = ptr + 1;
		}
	}

	// Token: 0x0600000B RID: 11 RVA: 0x00002B8C File Offset: 0x00000D8C
	public unsafe static void SetBirthMonth(GameData.Domains.Character.Character character, sbyte birthMonth, DataContext context)
	{
		AccessTools.Field(typeof(GameData.Domains.Character.Character), "_birthMonth").SetValue(character, birthMonth);
		AccessTools.Method(typeof(GameData.Domains.Character.Character), "SetModifiedAndInvalidateInfluencedCache", new Type[]
		{
			typeof(ushort),
			typeof(DataContext)
		}, null).Invoke(character, new object[]
		{
			5,
			context
		});
		bool flag = !character.CollectionHelperData.IsArchive;
		if (!flag)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField<int>(character.CollectionHelperData.DomainId, character.CollectionHelperData.DataId, character.GetId(), 10U, 1);
			*ptr = (byte)birthMonth;
			byte* ptr2 = ptr + 1;
		}
	}

	// Token: 0x0600000C RID: 12 RVA: 0x00002C4C File Offset: 0x00000E4C
	public unsafe static void SetBisexual(GameData.Domains.Character.Character character, bool bisexual, DataContext context)
	{
		AccessTools.Field(typeof(GameData.Domains.Character.Character), "_bisexual").SetValue(character, bisexual);
		AccessTools.Method(typeof(GameData.Domains.Character.Character), "SetModifiedAndInvalidateInfluencedCache", new Type[]
		{
			typeof(ushort),
			typeof(DataContext)
		}, null).Invoke(character, new object[]
		{
			14,
			context
		});
		bool flag = !character.CollectionHelperData.IsArchive;
		if (!flag)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField<int>(character.CollectionHelperData.DomainId, character.CollectionHelperData.DataId, character.GetId(), 27U, 1);
			*ptr = BitConverter.GetBytes(bisexual)[0];
			byte* ptr2 = ptr + 1;
		}
	}

	// Token: 0x0600000D RID: 13 RVA: 0x00002D14 File Offset: 0x00000F14
	public unsafe static void SetGender(GameData.Domains.Character.Character character, sbyte gender, DataContext context)
	{
		AccessTools.Field(typeof(GameData.Domains.Character.Character), "_gender").SetValue(character, gender);
		AccessTools.Method(typeof(GameData.Domains.Character.Character), "SetModifiedAndInvalidateInfluencedCache", new Type[]
		{
			typeof(ushort),
			typeof(DataContext)
		}, null).Invoke(character, new object[]
		{
			3,
			context
		});
		bool flag = !character.CollectionHelperData.IsArchive;
		if (!flag)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField<int>(character.CollectionHelperData.DomainId, character.CollectionHelperData.DataId, character.GetId(), 7U, 1);
			*ptr = (byte)gender;
			byte* ptr2 = ptr + 1;
		}
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002DD4 File Offset: 0x00000FD4
	public unsafe static void SetTransGender(GameData.Domains.Character.Character character, bool transgender, DataContext context)
	{
		AccessTools.Field(typeof(GameData.Domains.Character.Character), "_transgender").SetValue(character, transgender);
		AccessTools.Method(typeof(GameData.Domains.Character.Character), "SetModifiedAndInvalidateInfluencedCache", new Type[]
		{
			typeof(ushort),
			typeof(DataContext)
		}, null).Invoke(character, new object[]
		{
			13,
			context
		});
		bool flag = !character.CollectionHelperData.IsArchive;
		if (!flag)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField<int>(character.CollectionHelperData.DomainId, character.CollectionHelperData.DataId, character.GetId(), 26U, 1);
			*ptr = (transgender ? 1 : 0);
			byte* ptr2 = ptr + 1;
		}
	}

	// Token: 0x0600000F RID: 15 RVA: 0x00002E9C File Offset: 0x0000109C
	public static void updateAvatarData(ref AvatarData target, AvatarData reference)
	{
		target.AvatarId = reference.AvatarId;
		target.ColorSkinId = reference.ColorSkinId;
		target.ColorClothId = reference.ColorClothId;
		target.Wrinkle1Id = reference.Wrinkle1Id;
		target.Wrinkle2Id = reference.Wrinkle2Id;
		target.Wrinkle3Id = reference.Wrinkle3Id;
		target.EyesLeftId = reference.EyesLeftId;
		target.EyesRightId = reference.EyesRightId;
		target.FrontHairId = reference.FrontHairId;
		target.ColorFrontHairId = reference.ColorFrontHairId;
		target.BackHairId = reference.BackHairId;
		target.ColorBackHairId = reference.ColorBackHairId;
		target.EyebrowId = reference.EyebrowId;
		target.EyebrowDistancePercent = reference.EyebrowDistancePercent;
		target.EyebrowHeight = reference.EyebrowHeight;
		target.EyebrowScale = reference.EyebrowScale;
		target.EyebrowAngle = reference.EyebrowAngle;
		target.ColorEyebrowId = reference.ColorEyebrowId;
		target.EyesMainId = reference.EyesMainId;
		target.EyesDistancePercent = reference.EyesDistancePercent;
		target.EyesHeightPercent = reference.EyesHeightPercent;
		target.EyesScale = reference.EyesScale;
		target.EyesAngle = reference.EyesAngle;
		target.ColorEyeballId = reference.ColorEyeballId;
		target.NoseId = reference.NoseId;
		target.NoseScale = reference.NoseScale;
		target.NoseHeightPercent = reference.NoseHeightPercent;
		target.MouthId = reference.MouthId;
		target.MouthHeightPercent = reference.MouthHeightPercent;
		target.MouthScale = reference.MouthScale;
		target.ColorMouthId = reference.ColorMouthId;
		target.Feature1Id = reference.Feature1Id;
		target.Feature2Id = reference.Feature2Id;
		target.ColorFeature1Id = reference.ColorFeature1Id;
		target.ColorFeature2Id = reference.ColorFeature2Id;
		target.Beard1Id = reference.Beard1Id;
		target.Beard2Id = reference.Beard2Id;
		target.ColorBeard1Id = reference.ColorBeard1Id;
		target.ColorBeard2Id = reference.ColorBeard2Id;
	}

	// Token: 0x06000010 RID: 16 RVA: 0x000030A8 File Offset: 0x000012A8
	public static string GetName_UIShow(ref NameRelatedData data, bool isTaiwu, bool getRealName)
	{
		ValueTuple<string, string> valueTuple = Util.internalGetName_UIShow(ref data, isTaiwu, getRealName);
		string text = (valueTuple.Item1 ?? string.Empty) + valueTuple.Item2;
		string text2 = Util.internalGetMonasticTitle(ref data, isTaiwu);
		return string.IsNullOrEmpty(text2) ? text : text2;
	}

	// Token: 0x06000011 RID: 17 RVA: 0x000030F4 File Offset: 0x000012F4
	[return: Nullable(new byte[]
	{
		0,
		1,
		1
	})]
	private static ValueTuple<string, string> internalGetName_UIShow(ref NameRelatedData data, bool isTaiwu, bool getRealName)
	{
		bool flag = data.CharTemplateId < 0;
		ValueTuple<string, string> result;
		if (flag)
		{
			result = new ValueTuple<string, string>(null, DomainManager.World.GetElement_CustomTexts(1572));
		}
		else
		{
			CharacterItem characterItem = Config.Character.Instance[data.CharTemplateId];
			FullName other = default(FullName);
			bool flag2 = !isTaiwu;
			if (flag2)
			{
				bool flag3 = characterItem.CreatingType == 2;
				if (flag3)
				{
					bool flag4 = data.FullName.Equals(other);
					if (flag4)
					{
						return new ValueTuple<string, string>(characterItem.Surname, characterItem.GivenName);
					}
				}
				else
				{
					bool flag5 = characterItem.CreatingType != 1;
					if (flag5)
					{
						return new ValueTuple<string, string>(characterItem.Surname, characterItem.GivenName);
					}
				}
			}
			ValueTuple<string, string> name = data.FullName.GetName(data.Gender, DomainManager.World.GetCustomTexts());
			string text = name.Item1;
			string item = name.Item2;
			if (getRealName)
			{
				result = new ValueTuple<string, string>(text, item);
			}
			else
			{
				if (isTaiwu)
				{
					text = "太吾";
				}
				short index = Organization.Instance[data.OrgTemplateId].Members[(int)data.OrgGrade];
				short surnameId = OrganizationMember.Instance[index].SurnameId;
				bool flag6 = surnameId >= 0;
				if (flag6)
				{
					text = LocalSurnames.Instance.SurnameCore[(int)surnameId].Surname;
				}
				result = new ValueTuple<string, string>(text, item);
			}
		}
		return result;
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00003270 File Offset: 0x00001470
	private static string internalGetMonasticTitle(ref NameRelatedData data, bool isTaiwu)
	{
		bool flag = data.MonkType == 0;
		string result;
		if (flag)
		{
			result = null;
		}
		else
		{
			bool flag2 = (data.MonkType & 128) > 0;
			if (flag2)
			{
				MonasticTitleItem[] monasticTitles = LocalMonasticTitles.Instance.MonasticTitles;
				string name = monasticTitles[(int)data.MonasticTitle.SeniorityId].Name;
				string name2 = monasticTitles[(int)data.MonasticTitle.SuffixId].Name;
				short index = Organization.Instance[data.OrgTemplateId].Members[(int)data.OrgGrade];
				string str = OrganizationMember.Instance[index].MonasticTitleSuffixes[(int)data.Gender];
				result = name + name2 + str;
			}
			else
			{
				ValueTuple<string, string> valueTuple = Util.internalGetName_UIShow(ref data, isTaiwu, false);
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				string str2 = (!string.IsNullOrEmpty(item)) ? item : item2;
				ushort elementId = ((data.MonkType & 1) == 0) ? ((data.Gender == 1) ? 991 : 990) : ((data.Gender == 1) ? 989 : 988);
				string element_CustomTexts = DomainManager.World.GetElement_CustomTexts((int)elementId);
				result = str2 + element_CustomTexts;
			}
		}
		return result;
	}
}
