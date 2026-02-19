using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Character;

[SerializableGameData(NotForDisplayModule = true)]
public class PregnantState : ISerializableGameData
{
	[SerializableGameDataField(CollectionMaxElementsCount = 255)]
	public List<short> MotherFeatureIds;

	[SerializableGameDataField]
	public bool CreateMotherRelation;

	[SerializableGameDataField]
	public bool CreateFatherRelation;

	[SerializableGameDataField]
	public int FatherId;

	[SerializableGameDataField(CollectionMaxElementsCount = 255)]
	public List<short> FatherFeatureIds;

	[SerializableGameDataField]
	public Genome FatherGenome;

	[SerializableGameDataField]
	public int ExpectedBirthDate;

	[SerializableGameDataField]
	public bool IsHuman;

	public PregnantState(Character mother, Character father, bool isRaped)
	{
		MotherFeatureIds = new List<short>(mother.GetFeatureIds());
		CreateMotherRelation = true;
		if (father != null)
		{
			CreateFatherRelation = !isRaped;
			FatherId = father.GetId();
			FatherFeatureIds = new List<short>(father.GetFeatureIds());
			FatherGenome = father.GetGenome();
		}
		else
		{
			CreateFatherRelation = false;
			FatherId = -1;
		}
	}

	public static bool CheckPregnant(IRandomSource random, Character father, Character mother, bool isRape)
	{
		sbyte b = (sbyte)(isRape ? 20 : 60);
		int id = father.GetId();
		int id2 = mother.GetId();
		if (father.GetGender() == mother.GetGender())
		{
			return false;
		}
		if (mother.GetFeatureIds().Contains(197))
		{
			return false;
		}
		if (DomainManager.Character.TryGetElement_PregnancyLockEndDates(id2, out var _))
		{
			return false;
		}
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		bool flag = id2 == taiwuCharId || id == taiwuCharId;
		int num = (flag ? 20 : 40);
		short fertility = father.GetFertility();
		int count = DomainManager.Character.GetRelatedCharIds(id, 2).Count;
		if (fertility / num < count)
		{
			return false;
		}
		short fertility2 = mother.GetFertility();
		int count2 = DomainManager.Character.GetRelatedCharIds(id2, 2).Count;
		if (fertility2 / num < count2)
		{
			return false;
		}
		if (flag)
		{
			return random.CheckPercentProb(b * fertility * fertility2 / 10000);
		}
		int num2 = b * fertility * fertility2 / 10000;
		if (father.GetOrganizationInfo().OrgTemplateId == 16 || mother.GetOrganizationInfo().OrgTemplateId == 16)
		{
			int count3 = DomainManager.Building.GetHomeless().GetCount();
			if (count3 > 0)
			{
				num2 /= 10 * count3;
			}
		}
		return random.CheckPercentProb(num2);
	}

	public override string ToString()
	{
		return $"{{{{Father:{FatherId}, ExpectedBirthDate:{ExpectedBirthDate}, IsHuman:{IsHuman}}}";
	}

	public PregnantState()
	{
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 75;
		num = ((MotherFeatureIds == null) ? (num + 1) : (num + (1 + 2 * MotherFeatureIds.Count)));
		num = ((FatherFeatureIds == null) ? (num + 1) : (num + (1 + 2 * FatherFeatureIds.Count)));
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (MotherFeatureIds != null)
		{
			int count = MotherFeatureIds.Count;
			Tester.Assert(count <= 255);
			*ptr = (byte)count;
			ptr++;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = MotherFeatureIds[i];
			}
			ptr += 2 * count;
		}
		else
		{
			*ptr = 0;
			ptr++;
		}
		*ptr = (CreateMotherRelation ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (CreateFatherRelation ? ((byte)1) : ((byte)0));
		ptr++;
		*(int*)ptr = FatherId;
		ptr += 4;
		if (FatherFeatureIds != null)
		{
			int count2 = FatherFeatureIds.Count;
			Tester.Assert(count2 <= 255);
			*ptr = (byte)count2;
			ptr++;
			for (int j = 0; j < count2; j++)
			{
				((short*)ptr)[j] = FatherFeatureIds[j];
			}
			ptr += 2 * count2;
		}
		else
		{
			*ptr = 0;
			ptr++;
		}
		ptr += FatherGenome.Serialize(ptr);
		*(int*)ptr = ExpectedBirthDate;
		ptr += 4;
		*ptr = (IsHuman ? ((byte)1) : ((byte)0));
		ptr++;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		byte b = *ptr;
		ptr++;
		if (b > 0)
		{
			if (MotherFeatureIds == null)
			{
				MotherFeatureIds = new List<short>(b);
			}
			else
			{
				MotherFeatureIds.Clear();
			}
			for (int i = 0; i < b; i++)
			{
				MotherFeatureIds.Add(((short*)ptr)[i]);
			}
			ptr += 2 * b;
		}
		else
		{
			MotherFeatureIds?.Clear();
		}
		CreateMotherRelation = *ptr != 0;
		ptr++;
		CreateFatherRelation = *ptr != 0;
		ptr++;
		FatherId = *(int*)ptr;
		ptr += 4;
		byte b2 = *ptr;
		ptr++;
		if (b2 > 0)
		{
			if (FatherFeatureIds == null)
			{
				FatherFeatureIds = new List<short>(b2);
			}
			else
			{
				FatherFeatureIds.Clear();
			}
			for (int j = 0; j < b2; j++)
			{
				FatherFeatureIds.Add(((short*)ptr)[j]);
			}
			ptr += 2 * b2;
		}
		else
		{
			FatherFeatureIds?.Clear();
		}
		ptr += FatherGenome.Deserialize(ptr);
		ExpectedBirthDate = *(int*)ptr;
		ptr += 4;
		IsHuman = *ptr != 0;
		ptr++;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}
}
