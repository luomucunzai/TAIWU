using System.Collections.Generic;
using GameData.Domains.Item;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Display;

[SerializableGameData(NotForArchive = true, NoCopyConstructors = true)]
public class CharacterDisplayDataForMapBlock : ISerializableGameData
{
	[SerializableGameDataField]
	public int CharacterId;

	[SerializableGameDataField]
	public short TemplateId;

	[SerializableGameDataField]
	public byte CreatingType;

	[SerializableGameDataField]
	public NameRelatedData NameRelatedData;

	[SerializableGameDataField]
	public AvatarRelatedData AvatarRelatedData;

	[SerializableGameDataField]
	public short Age;

	[SerializableGameDataField]
	public short Charm;

	[SerializableGameDataField]
	public sbyte Gender;

	[SerializableGameDataField]
	public bool IsReclusiveChar;

	[SerializableGameDataField]
	public sbyte HappinessType;

	[SerializableGameDataField]
	public sbyte BehaviorType;

	[SerializableGameDataField]
	public sbyte FameType;

	[SerializableGameDataField]
	public OrganizationInfo OrganizationInfo;

	[SerializableGameDataField]
	public int InfluencePower;

	[SerializableGameDataField]
	public short Health;

	[SerializableGameDataField]
	public short LeftMaxHealth;

	[SerializableGameDataField]
	public List<short> TitleIdList;

	[SerializableGameDataField]
	public short PreexistenceCharCount;

	[SerializableGameDataField]
	public short DisorderOfQi;

	[SerializableGameDataField]
	public NeiliProportionOfFiveElements NeiliProportionOfFiveElements;

	[SerializableGameDataField]
	public sbyte ConsummateLevel;

	[SerializableGameDataField]
	public sbyte BirthMonth;

	[SerializableGameDataField]
	public int AttackMedal;

	[SerializableGameDataField]
	public int DefenceMedal;

	[SerializableGameDataField]
	public int WisdomMedal;

	[SerializableGameDataField]
	public Injuries Injuries;

	[SerializableGameDataField]
	public PoisonInts Poisons;

	[SerializableGameDataField]
	public List<sbyte> TeammateCommands;

	[SerializableGameDataField]
	public List<short> RelationshipToTaiwuList;

	[SerializableGameDataField]
	public CharacterLoveAndHateItemInfo LoveAndHateItemInfo;

	[SerializableGameDataField]
	public Dictionary<short, int> RelationshipCountDict;

	[SerializableGameDataField]
	public List<CharacterDisplayData> BloodParentCharDataList;

	[SerializableGameDataField]
	public List<CharacterDisplayData> BloodChildCharDataList;

	[SerializableGameDataField]
	public List<CharacterDisplayData> BloodBrotherOrSisterCharDataList;

	[SerializableGameDataField]
	public List<CharacterDisplayData> HusbandOrWifeCharDataList;

	[SerializableGameDataField]
	public Dictionary<short, bool> VisibleCharacterInteractionEventOptionDict;

	[SerializableGameDataField]
	public int NoInteractionReason;

	[SerializableGameDataField]
	public short FavorabilityToTaiwu;

	[SerializableGameDataField]
	public short[] LearnedCombatSkillCountArray;

	[SerializableGameDataField]
	public short[] LearnedHighestGradeNeigongCombatSkillArray;

	[SerializableGameDataField]
	public short[] LearnedHighestGradePosingCombatSkillArray;

	[SerializableGameDataField]
	public short[] LearnedHighestGradeStuntCombatSkillArray;

	[SerializableGameDataField]
	public short[] LearnedHighestGradeAttackCombatSkillArray;

	[SerializableGameDataField]
	public MainAttributes MainAttributes;

	[SerializableGameDataField]
	public MainAttributes CurrMainAttributes;

	[SerializableGameDataField]
	public CombatSkillShorts CombatSkillQualifications;

	[SerializableGameDataField]
	public CombatSkillShorts CombatSkillAttainments;

	[SerializableGameDataField]
	public sbyte CombatSkillGrowthType;

	[SerializableGameDataField]
	public LifeSkillShorts LifeSkillQualifications;

	[SerializableGameDataField]
	public LifeSkillShorts LifeSkillAttainments;

	[SerializableGameDataField]
	public sbyte LifeSkillGrowthType;

	[SerializableGameDataField]
	public List<short> FeatureIds;

	[SerializableGameDataField]
	public ItemKey[] EquipmentArray;

	[SerializableGameDataField]
	public ItemKey[] HighestGradeItemArray;

	[SerializableGameDataField]
	public ItemKey[] HighestGradeCombatSkillBookArray;

	[SerializableGameDataField]
	public ItemKey[] HighestGradeLifeSkillBookArray;

	[SerializableGameDataField]
	public short[] SkillBookCountArray;

	[SerializableGameDataField]
	public List<sbyte> LegendaryBookTypeList;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 386;
		num = ((TitleIdList == null) ? (num + 2) : (num + (2 + 2 * TitleIdList.Count)));
		num = ((TeammateCommands == null) ? (num + 2) : (num + (2 + TeammateCommands.Count)));
		num = ((RelationshipToTaiwuList == null) ? (num + 2) : (num + (2 + 2 * RelationshipToTaiwuList.Count)));
		num += DictionaryOfBasicTypePair.GetSerializedSize<short, int>((IReadOnlyDictionary<short, int>)RelationshipCountDict);
		if (BloodParentCharDataList != null)
		{
			num += 2;
			int count = BloodParentCharDataList.Count;
			for (int i = 0; i < count; i++)
			{
				CharacterDisplayData characterDisplayData = BloodParentCharDataList[i];
				num = ((characterDisplayData == null) ? (num + 2) : (num + (2 + characterDisplayData.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		if (BloodChildCharDataList != null)
		{
			num += 2;
			int count2 = BloodChildCharDataList.Count;
			for (int j = 0; j < count2; j++)
			{
				CharacterDisplayData characterDisplayData2 = BloodChildCharDataList[j];
				num = ((characterDisplayData2 == null) ? (num + 2) : (num + (2 + characterDisplayData2.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		if (BloodBrotherOrSisterCharDataList != null)
		{
			num += 2;
			int count3 = BloodBrotherOrSisterCharDataList.Count;
			for (int k = 0; k < count3; k++)
			{
				CharacterDisplayData characterDisplayData3 = BloodBrotherOrSisterCharDataList[k];
				num = ((characterDisplayData3 == null) ? (num + 2) : (num + (2 + characterDisplayData3.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		if (HusbandOrWifeCharDataList != null)
		{
			num += 2;
			int count4 = HusbandOrWifeCharDataList.Count;
			for (int l = 0; l < count4; l++)
			{
				CharacterDisplayData characterDisplayData4 = HusbandOrWifeCharDataList[l];
				num = ((characterDisplayData4 == null) ? (num + 2) : (num + (2 + characterDisplayData4.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		num += DictionaryOfBasicTypePair.GetSerializedSize<short, bool>((IReadOnlyDictionary<short, bool>)VisibleCharacterInteractionEventOptionDict);
		num = ((LearnedCombatSkillCountArray == null) ? (num + 2) : (num + (2 + 2 * LearnedCombatSkillCountArray.Length)));
		num = ((LearnedHighestGradeNeigongCombatSkillArray == null) ? (num + 2) : (num + (2 + 2 * LearnedHighestGradeNeigongCombatSkillArray.Length)));
		num = ((LearnedHighestGradePosingCombatSkillArray == null) ? (num + 2) : (num + (2 + 2 * LearnedHighestGradePosingCombatSkillArray.Length)));
		num = ((LearnedHighestGradeStuntCombatSkillArray == null) ? (num + 2) : (num + (2 + 2 * LearnedHighestGradeStuntCombatSkillArray.Length)));
		num = ((LearnedHighestGradeAttackCombatSkillArray == null) ? (num + 2) : (num + (2 + 2 * LearnedHighestGradeAttackCombatSkillArray.Length)));
		num = ((FeatureIds == null) ? (num + 2) : (num + (2 + 2 * FeatureIds.Count)));
		num = ((EquipmentArray == null) ? (num + 2) : (num + (2 + 8 * EquipmentArray.Length)));
		num = ((HighestGradeItemArray == null) ? (num + 2) : (num + (2 + 8 * HighestGradeItemArray.Length)));
		num = ((HighestGradeCombatSkillBookArray == null) ? (num + 2) : (num + (2 + 8 * HighestGradeCombatSkillBookArray.Length)));
		num = ((HighestGradeLifeSkillBookArray == null) ? (num + 2) : (num + (2 + 8 * HighestGradeLifeSkillBookArray.Length)));
		num = ((SkillBookCountArray == null) ? (num + 2) : (num + (2 + 2 * SkillBookCountArray.Length)));
		num = ((LegendaryBookTypeList == null) ? (num + 2) : (num + (2 + LegendaryBookTypeList.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = CharacterId;
		ptr += 4;
		*(short*)ptr = TemplateId;
		ptr += 2;
		*ptr = CreatingType;
		ptr++;
		ptr += NameRelatedData.Serialize(ptr);
		ptr += AvatarRelatedData.Serialize(ptr);
		*(short*)ptr = Age;
		ptr += 2;
		*(short*)ptr = Charm;
		ptr += 2;
		*ptr = (byte)Gender;
		ptr++;
		*ptr = (IsReclusiveChar ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (byte)HappinessType;
		ptr++;
		*ptr = (byte)BehaviorType;
		ptr++;
		*ptr = (byte)FameType;
		ptr++;
		ptr += OrganizationInfo.Serialize(ptr);
		*(int*)ptr = InfluencePower;
		ptr += 4;
		*(short*)ptr = Health;
		ptr += 2;
		*(short*)ptr = LeftMaxHealth;
		ptr += 2;
		if (TitleIdList != null)
		{
			int count = TitleIdList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = TitleIdList[i];
			}
			ptr += 2 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(short*)ptr = PreexistenceCharCount;
		ptr += 2;
		*(short*)ptr = DisorderOfQi;
		ptr += 2;
		ptr += NeiliProportionOfFiveElements.Serialize(ptr);
		*ptr = (byte)ConsummateLevel;
		ptr++;
		*ptr = (byte)BirthMonth;
		ptr++;
		*(int*)ptr = AttackMedal;
		ptr += 4;
		*(int*)ptr = DefenceMedal;
		ptr += 4;
		*(int*)ptr = WisdomMedal;
		ptr += 4;
		ptr += Injuries.Serialize(ptr);
		ptr += Poisons.Serialize(ptr);
		if (TeammateCommands != null)
		{
			int count2 = TeammateCommands.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				ptr[j] = (byte)TeammateCommands[j];
			}
			ptr += count2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (RelationshipToTaiwuList != null)
		{
			int count3 = RelationshipToTaiwuList.Count;
			Tester.Assert(count3 <= 65535);
			*(ushort*)ptr = (ushort)count3;
			ptr += 2;
			for (int k = 0; k < count3; k++)
			{
				((short*)ptr)[k] = RelationshipToTaiwuList[k];
			}
			ptr += 2 * count3;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += LoveAndHateItemInfo.Serialize(ptr);
		ptr += DictionaryOfBasicTypePair.Serialize<short, int>(ptr, ref RelationshipCountDict);
		if (BloodParentCharDataList != null)
		{
			int count4 = BloodParentCharDataList.Count;
			Tester.Assert(count4 <= 65535);
			*(ushort*)ptr = (ushort)count4;
			ptr += 2;
			for (int l = 0; l < count4; l++)
			{
				CharacterDisplayData characterDisplayData = BloodParentCharDataList[l];
				if (characterDisplayData != null)
				{
					byte* intPtr = ptr;
					ptr += 2;
					int num = characterDisplayData.Serialize(ptr);
					ptr += num;
					Tester.Assert(num <= 65535);
					*(ushort*)intPtr = (ushort)num;
				}
				else
				{
					*(short*)ptr = 0;
					ptr += 2;
				}
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (BloodChildCharDataList != null)
		{
			int count5 = BloodChildCharDataList.Count;
			Tester.Assert(count5 <= 65535);
			*(ushort*)ptr = (ushort)count5;
			ptr += 2;
			for (int m = 0; m < count5; m++)
			{
				CharacterDisplayData characterDisplayData2 = BloodChildCharDataList[m];
				if (characterDisplayData2 != null)
				{
					byte* intPtr2 = ptr;
					ptr += 2;
					int num2 = characterDisplayData2.Serialize(ptr);
					ptr += num2;
					Tester.Assert(num2 <= 65535);
					*(ushort*)intPtr2 = (ushort)num2;
				}
				else
				{
					*(short*)ptr = 0;
					ptr += 2;
				}
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (BloodBrotherOrSisterCharDataList != null)
		{
			int count6 = BloodBrotherOrSisterCharDataList.Count;
			Tester.Assert(count6 <= 65535);
			*(ushort*)ptr = (ushort)count6;
			ptr += 2;
			for (int n = 0; n < count6; n++)
			{
				CharacterDisplayData characterDisplayData3 = BloodBrotherOrSisterCharDataList[n];
				if (characterDisplayData3 != null)
				{
					byte* intPtr3 = ptr;
					ptr += 2;
					int num3 = characterDisplayData3.Serialize(ptr);
					ptr += num3;
					Tester.Assert(num3 <= 65535);
					*(ushort*)intPtr3 = (ushort)num3;
				}
				else
				{
					*(short*)ptr = 0;
					ptr += 2;
				}
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (HusbandOrWifeCharDataList != null)
		{
			int count7 = HusbandOrWifeCharDataList.Count;
			Tester.Assert(count7 <= 65535);
			*(ushort*)ptr = (ushort)count7;
			ptr += 2;
			for (int num4 = 0; num4 < count7; num4++)
			{
				CharacterDisplayData characterDisplayData4 = HusbandOrWifeCharDataList[num4];
				if (characterDisplayData4 != null)
				{
					byte* intPtr4 = ptr;
					ptr += 2;
					int num5 = characterDisplayData4.Serialize(ptr);
					ptr += num5;
					Tester.Assert(num5 <= 65535);
					*(ushort*)intPtr4 = (ushort)num5;
				}
				else
				{
					*(short*)ptr = 0;
					ptr += 2;
				}
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += DictionaryOfBasicTypePair.Serialize<short, bool>(ptr, ref VisibleCharacterInteractionEventOptionDict);
		*(int*)ptr = NoInteractionReason;
		ptr += 4;
		*(short*)ptr = FavorabilityToTaiwu;
		ptr += 2;
		if (LearnedCombatSkillCountArray != null)
		{
			int num6 = LearnedCombatSkillCountArray.Length;
			Tester.Assert(num6 <= 65535);
			*(ushort*)ptr = (ushort)num6;
			ptr += 2;
			for (int num7 = 0; num7 < num6; num7++)
			{
				((short*)ptr)[num7] = LearnedCombatSkillCountArray[num7];
			}
			ptr += 2 * num6;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (LearnedHighestGradeNeigongCombatSkillArray != null)
		{
			int num8 = LearnedHighestGradeNeigongCombatSkillArray.Length;
			Tester.Assert(num8 <= 65535);
			*(ushort*)ptr = (ushort)num8;
			ptr += 2;
			for (int num9 = 0; num9 < num8; num9++)
			{
				((short*)ptr)[num9] = LearnedHighestGradeNeigongCombatSkillArray[num9];
			}
			ptr += 2 * num8;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (LearnedHighestGradePosingCombatSkillArray != null)
		{
			int num10 = LearnedHighestGradePosingCombatSkillArray.Length;
			Tester.Assert(num10 <= 65535);
			*(ushort*)ptr = (ushort)num10;
			ptr += 2;
			for (int num11 = 0; num11 < num10; num11++)
			{
				((short*)ptr)[num11] = LearnedHighestGradePosingCombatSkillArray[num11];
			}
			ptr += 2 * num10;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (LearnedHighestGradeStuntCombatSkillArray != null)
		{
			int num12 = LearnedHighestGradeStuntCombatSkillArray.Length;
			Tester.Assert(num12 <= 65535);
			*(ushort*)ptr = (ushort)num12;
			ptr += 2;
			for (int num13 = 0; num13 < num12; num13++)
			{
				((short*)ptr)[num13] = LearnedHighestGradeStuntCombatSkillArray[num13];
			}
			ptr += 2 * num12;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (LearnedHighestGradeAttackCombatSkillArray != null)
		{
			int num14 = LearnedHighestGradeAttackCombatSkillArray.Length;
			Tester.Assert(num14 <= 65535);
			*(ushort*)ptr = (ushort)num14;
			ptr += 2;
			for (int num15 = 0; num15 < num14; num15++)
			{
				((short*)ptr)[num15] = LearnedHighestGradeAttackCombatSkillArray[num15];
			}
			ptr += 2 * num14;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += MainAttributes.Serialize(ptr);
		ptr += CurrMainAttributes.Serialize(ptr);
		ptr += CombatSkillQualifications.Serialize(ptr);
		ptr += CombatSkillAttainments.Serialize(ptr);
		*ptr = (byte)CombatSkillGrowthType;
		ptr++;
		ptr += LifeSkillQualifications.Serialize(ptr);
		ptr += LifeSkillAttainments.Serialize(ptr);
		*ptr = (byte)LifeSkillGrowthType;
		ptr++;
		if (FeatureIds != null)
		{
			int count8 = FeatureIds.Count;
			Tester.Assert(count8 <= 65535);
			*(ushort*)ptr = (ushort)count8;
			ptr += 2;
			for (int num16 = 0; num16 < count8; num16++)
			{
				((short*)ptr)[num16] = FeatureIds[num16];
			}
			ptr += 2 * count8;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (EquipmentArray != null)
		{
			int num17 = EquipmentArray.Length;
			Tester.Assert(num17 <= 65535);
			*(ushort*)ptr = (ushort)num17;
			ptr += 2;
			for (int num18 = 0; num18 < num17; num18++)
			{
				ptr += EquipmentArray[num18].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (HighestGradeItemArray != null)
		{
			int num19 = HighestGradeItemArray.Length;
			Tester.Assert(num19 <= 65535);
			*(ushort*)ptr = (ushort)num19;
			ptr += 2;
			for (int num20 = 0; num20 < num19; num20++)
			{
				ptr += HighestGradeItemArray[num20].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (HighestGradeCombatSkillBookArray != null)
		{
			int num21 = HighestGradeCombatSkillBookArray.Length;
			Tester.Assert(num21 <= 65535);
			*(ushort*)ptr = (ushort)num21;
			ptr += 2;
			for (int num22 = 0; num22 < num21; num22++)
			{
				ptr += HighestGradeCombatSkillBookArray[num22].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (HighestGradeLifeSkillBookArray != null)
		{
			int num23 = HighestGradeLifeSkillBookArray.Length;
			Tester.Assert(num23 <= 65535);
			*(ushort*)ptr = (ushort)num23;
			ptr += 2;
			for (int num24 = 0; num24 < num23; num24++)
			{
				ptr += HighestGradeLifeSkillBookArray[num24].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (SkillBookCountArray != null)
		{
			int num25 = SkillBookCountArray.Length;
			Tester.Assert(num25 <= 65535);
			*(ushort*)ptr = (ushort)num25;
			ptr += 2;
			for (int num26 = 0; num26 < num25; num26++)
			{
				((short*)ptr)[num26] = SkillBookCountArray[num26];
			}
			ptr += 2 * num25;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (LegendaryBookTypeList != null)
		{
			int count9 = LegendaryBookTypeList.Count;
			Tester.Assert(count9 <= 65535);
			*(ushort*)ptr = (ushort)count9;
			ptr += 2;
			for (int num27 = 0; num27 < count9; num27++)
			{
				ptr[num27] = (byte)LegendaryBookTypeList[num27];
			}
			ptr += count9;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num28 = (int)(ptr - pData);
		if (num28 > 4)
		{
			return (num28 + 3) / 4 * 4;
		}
		return num28;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		CharacterId = *(int*)ptr;
		ptr += 4;
		TemplateId = *(short*)ptr;
		ptr += 2;
		CreatingType = *ptr;
		ptr++;
		ptr += NameRelatedData.Deserialize(ptr);
		if (AvatarRelatedData == null)
		{
			AvatarRelatedData = new AvatarRelatedData();
		}
		ptr += AvatarRelatedData.Deserialize(ptr);
		Age = *(short*)ptr;
		ptr += 2;
		Charm = *(short*)ptr;
		ptr += 2;
		Gender = (sbyte)(*ptr);
		ptr++;
		IsReclusiveChar = *ptr != 0;
		ptr++;
		HappinessType = (sbyte)(*ptr);
		ptr++;
		BehaviorType = (sbyte)(*ptr);
		ptr++;
		FameType = (sbyte)(*ptr);
		ptr++;
		ptr += OrganizationInfo.Deserialize(ptr);
		InfluencePower = *(int*)ptr;
		ptr += 4;
		Health = *(short*)ptr;
		ptr += 2;
		LeftMaxHealth = *(short*)ptr;
		ptr += 2;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (TitleIdList == null)
			{
				TitleIdList = new List<short>(num);
			}
			else
			{
				TitleIdList.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				TitleIdList.Add(((short*)ptr)[i]);
			}
			ptr += 2 * num;
		}
		else
		{
			TitleIdList?.Clear();
		}
		PreexistenceCharCount = *(short*)ptr;
		ptr += 2;
		DisorderOfQi = *(short*)ptr;
		ptr += 2;
		ptr += NeiliProportionOfFiveElements.Deserialize(ptr);
		ConsummateLevel = (sbyte)(*ptr);
		ptr++;
		BirthMonth = (sbyte)(*ptr);
		ptr++;
		AttackMedal = *(int*)ptr;
		ptr += 4;
		DefenceMedal = *(int*)ptr;
		ptr += 4;
		WisdomMedal = *(int*)ptr;
		ptr += 4;
		ptr += Injuries.Deserialize(ptr);
		ptr += Poisons.Deserialize(ptr);
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (TeammateCommands == null)
			{
				TeammateCommands = new List<sbyte>(num2);
			}
			else
			{
				TeammateCommands.Clear();
			}
			for (int j = 0; j < num2; j++)
			{
				TeammateCommands.Add((sbyte)ptr[j]);
			}
			ptr += (int)num2;
		}
		else
		{
			TeammateCommands?.Clear();
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (RelationshipToTaiwuList == null)
			{
				RelationshipToTaiwuList = new List<short>(num3);
			}
			else
			{
				RelationshipToTaiwuList.Clear();
			}
			for (int k = 0; k < num3; k++)
			{
				RelationshipToTaiwuList.Add(((short*)ptr)[k]);
			}
			ptr += 2 * num3;
		}
		else
		{
			RelationshipToTaiwuList?.Clear();
		}
		if (LoveAndHateItemInfo == null)
		{
			LoveAndHateItemInfo = new CharacterLoveAndHateItemInfo();
		}
		ptr += LoveAndHateItemInfo.Deserialize(ptr);
		ptr += DictionaryOfBasicTypePair.Deserialize<short, int>(ptr, ref RelationshipCountDict);
		ushort num4 = *(ushort*)ptr;
		ptr += 2;
		if (num4 > 0)
		{
			if (BloodParentCharDataList == null)
			{
				BloodParentCharDataList = new List<CharacterDisplayData>(num4);
			}
			else
			{
				BloodParentCharDataList.Clear();
			}
			for (int l = 0; l < num4; l++)
			{
				ushort num5 = *(ushort*)ptr;
				ptr += 2;
				if (num5 > 0)
				{
					CharacterDisplayData characterDisplayData = new CharacterDisplayData();
					ptr += characterDisplayData.Deserialize(ptr);
					BloodParentCharDataList.Add(characterDisplayData);
				}
				else
				{
					BloodParentCharDataList.Add(null);
				}
			}
		}
		else
		{
			BloodParentCharDataList?.Clear();
		}
		ushort num6 = *(ushort*)ptr;
		ptr += 2;
		if (num6 > 0)
		{
			if (BloodChildCharDataList == null)
			{
				BloodChildCharDataList = new List<CharacterDisplayData>(num6);
			}
			else
			{
				BloodChildCharDataList.Clear();
			}
			for (int m = 0; m < num6; m++)
			{
				ushort num7 = *(ushort*)ptr;
				ptr += 2;
				if (num7 > 0)
				{
					CharacterDisplayData characterDisplayData2 = new CharacterDisplayData();
					ptr += characterDisplayData2.Deserialize(ptr);
					BloodChildCharDataList.Add(characterDisplayData2);
				}
				else
				{
					BloodChildCharDataList.Add(null);
				}
			}
		}
		else
		{
			BloodChildCharDataList?.Clear();
		}
		ushort num8 = *(ushort*)ptr;
		ptr += 2;
		if (num8 > 0)
		{
			if (BloodBrotherOrSisterCharDataList == null)
			{
				BloodBrotherOrSisterCharDataList = new List<CharacterDisplayData>(num8);
			}
			else
			{
				BloodBrotherOrSisterCharDataList.Clear();
			}
			for (int n = 0; n < num8; n++)
			{
				ushort num9 = *(ushort*)ptr;
				ptr += 2;
				if (num9 > 0)
				{
					CharacterDisplayData characterDisplayData3 = new CharacterDisplayData();
					ptr += characterDisplayData3.Deserialize(ptr);
					BloodBrotherOrSisterCharDataList.Add(characterDisplayData3);
				}
				else
				{
					BloodBrotherOrSisterCharDataList.Add(null);
				}
			}
		}
		else
		{
			BloodBrotherOrSisterCharDataList?.Clear();
		}
		ushort num10 = *(ushort*)ptr;
		ptr += 2;
		if (num10 > 0)
		{
			if (HusbandOrWifeCharDataList == null)
			{
				HusbandOrWifeCharDataList = new List<CharacterDisplayData>(num10);
			}
			else
			{
				HusbandOrWifeCharDataList.Clear();
			}
			for (int num11 = 0; num11 < num10; num11++)
			{
				ushort num12 = *(ushort*)ptr;
				ptr += 2;
				if (num12 > 0)
				{
					CharacterDisplayData characterDisplayData4 = new CharacterDisplayData();
					ptr += characterDisplayData4.Deserialize(ptr);
					HusbandOrWifeCharDataList.Add(characterDisplayData4);
				}
				else
				{
					HusbandOrWifeCharDataList.Add(null);
				}
			}
		}
		else
		{
			HusbandOrWifeCharDataList?.Clear();
		}
		ptr += DictionaryOfBasicTypePair.Deserialize<short, bool>(ptr, ref VisibleCharacterInteractionEventOptionDict);
		NoInteractionReason = *(int*)ptr;
		ptr += 4;
		FavorabilityToTaiwu = *(short*)ptr;
		ptr += 2;
		ushort num13 = *(ushort*)ptr;
		ptr += 2;
		if (num13 > 0)
		{
			if (LearnedCombatSkillCountArray == null || LearnedCombatSkillCountArray.Length != num13)
			{
				LearnedCombatSkillCountArray = new short[num13];
			}
			for (int num14 = 0; num14 < num13; num14++)
			{
				LearnedCombatSkillCountArray[num14] = ((short*)ptr)[num14];
			}
			ptr += 2 * num13;
		}
		else
		{
			LearnedCombatSkillCountArray = null;
		}
		ushort num15 = *(ushort*)ptr;
		ptr += 2;
		if (num15 > 0)
		{
			if (LearnedHighestGradeNeigongCombatSkillArray == null || LearnedHighestGradeNeigongCombatSkillArray.Length != num15)
			{
				LearnedHighestGradeNeigongCombatSkillArray = new short[num15];
			}
			for (int num16 = 0; num16 < num15; num16++)
			{
				LearnedHighestGradeNeigongCombatSkillArray[num16] = ((short*)ptr)[num16];
			}
			ptr += 2 * num15;
		}
		else
		{
			LearnedHighestGradeNeigongCombatSkillArray = null;
		}
		ushort num17 = *(ushort*)ptr;
		ptr += 2;
		if (num17 > 0)
		{
			if (LearnedHighestGradePosingCombatSkillArray == null || LearnedHighestGradePosingCombatSkillArray.Length != num17)
			{
				LearnedHighestGradePosingCombatSkillArray = new short[num17];
			}
			for (int num18 = 0; num18 < num17; num18++)
			{
				LearnedHighestGradePosingCombatSkillArray[num18] = ((short*)ptr)[num18];
			}
			ptr += 2 * num17;
		}
		else
		{
			LearnedHighestGradePosingCombatSkillArray = null;
		}
		ushort num19 = *(ushort*)ptr;
		ptr += 2;
		if (num19 > 0)
		{
			if (LearnedHighestGradeStuntCombatSkillArray == null || LearnedHighestGradeStuntCombatSkillArray.Length != num19)
			{
				LearnedHighestGradeStuntCombatSkillArray = new short[num19];
			}
			for (int num20 = 0; num20 < num19; num20++)
			{
				LearnedHighestGradeStuntCombatSkillArray[num20] = ((short*)ptr)[num20];
			}
			ptr += 2 * num19;
		}
		else
		{
			LearnedHighestGradeStuntCombatSkillArray = null;
		}
		ushort num21 = *(ushort*)ptr;
		ptr += 2;
		if (num21 > 0)
		{
			if (LearnedHighestGradeAttackCombatSkillArray == null || LearnedHighestGradeAttackCombatSkillArray.Length != num21)
			{
				LearnedHighestGradeAttackCombatSkillArray = new short[num21];
			}
			for (int num22 = 0; num22 < num21; num22++)
			{
				LearnedHighestGradeAttackCombatSkillArray[num22] = ((short*)ptr)[num22];
			}
			ptr += 2 * num21;
		}
		else
		{
			LearnedHighestGradeAttackCombatSkillArray = null;
		}
		ptr += MainAttributes.Deserialize(ptr);
		ptr += CurrMainAttributes.Deserialize(ptr);
		ptr += CombatSkillQualifications.Deserialize(ptr);
		ptr += CombatSkillAttainments.Deserialize(ptr);
		CombatSkillGrowthType = (sbyte)(*ptr);
		ptr++;
		ptr += LifeSkillQualifications.Deserialize(ptr);
		ptr += LifeSkillAttainments.Deserialize(ptr);
		LifeSkillGrowthType = (sbyte)(*ptr);
		ptr++;
		ushort num23 = *(ushort*)ptr;
		ptr += 2;
		if (num23 > 0)
		{
			if (FeatureIds == null)
			{
				FeatureIds = new List<short>(num23);
			}
			else
			{
				FeatureIds.Clear();
			}
			for (int num24 = 0; num24 < num23; num24++)
			{
				FeatureIds.Add(((short*)ptr)[num24]);
			}
			ptr += 2 * num23;
		}
		else
		{
			FeatureIds?.Clear();
		}
		ushort num25 = *(ushort*)ptr;
		ptr += 2;
		if (num25 > 0)
		{
			if (EquipmentArray == null || EquipmentArray.Length != num25)
			{
				EquipmentArray = new ItemKey[num25];
			}
			for (int num26 = 0; num26 < num25; num26++)
			{
				ItemKey itemKey = default(ItemKey);
				ptr += itemKey.Deserialize(ptr);
				EquipmentArray[num26] = itemKey;
			}
		}
		else
		{
			EquipmentArray = null;
		}
		ushort num27 = *(ushort*)ptr;
		ptr += 2;
		if (num27 > 0)
		{
			if (HighestGradeItemArray == null || HighestGradeItemArray.Length != num27)
			{
				HighestGradeItemArray = new ItemKey[num27];
			}
			for (int num28 = 0; num28 < num27; num28++)
			{
				ItemKey itemKey2 = default(ItemKey);
				ptr += itemKey2.Deserialize(ptr);
				HighestGradeItemArray[num28] = itemKey2;
			}
		}
		else
		{
			HighestGradeItemArray = null;
		}
		ushort num29 = *(ushort*)ptr;
		ptr += 2;
		if (num29 > 0)
		{
			if (HighestGradeCombatSkillBookArray == null || HighestGradeCombatSkillBookArray.Length != num29)
			{
				HighestGradeCombatSkillBookArray = new ItemKey[num29];
			}
			for (int num30 = 0; num30 < num29; num30++)
			{
				ItemKey itemKey3 = default(ItemKey);
				ptr += itemKey3.Deserialize(ptr);
				HighestGradeCombatSkillBookArray[num30] = itemKey3;
			}
		}
		else
		{
			HighestGradeCombatSkillBookArray = null;
		}
		ushort num31 = *(ushort*)ptr;
		ptr += 2;
		if (num31 > 0)
		{
			if (HighestGradeLifeSkillBookArray == null || HighestGradeLifeSkillBookArray.Length != num31)
			{
				HighestGradeLifeSkillBookArray = new ItemKey[num31];
			}
			for (int num32 = 0; num32 < num31; num32++)
			{
				ItemKey itemKey4 = default(ItemKey);
				ptr += itemKey4.Deserialize(ptr);
				HighestGradeLifeSkillBookArray[num32] = itemKey4;
			}
		}
		else
		{
			HighestGradeLifeSkillBookArray = null;
		}
		ushort num33 = *(ushort*)ptr;
		ptr += 2;
		if (num33 > 0)
		{
			if (SkillBookCountArray == null || SkillBookCountArray.Length != num33)
			{
				SkillBookCountArray = new short[num33];
			}
			for (int num34 = 0; num34 < num33; num34++)
			{
				SkillBookCountArray[num34] = ((short*)ptr)[num34];
			}
			ptr += 2 * num33;
		}
		else
		{
			SkillBookCountArray = null;
		}
		ushort num35 = *(ushort*)ptr;
		ptr += 2;
		if (num35 > 0)
		{
			if (LegendaryBookTypeList == null)
			{
				LegendaryBookTypeList = new List<sbyte>(num35);
			}
			else
			{
				LegendaryBookTypeList.Clear();
			}
			for (int num36 = 0; num36 < num35; num36++)
			{
				LegendaryBookTypeList.Add((sbyte)ptr[num36]);
			}
			ptr += (int)num35;
		}
		else
		{
			LegendaryBookTypeList?.Clear();
		}
		int num37 = (int)(ptr - pData);
		if (num37 > 4)
		{
			return (num37 + 3) / 4 * 4;
		}
		return num37;
	}
}
