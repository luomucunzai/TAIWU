using System.Collections.Generic;
using GameData.Domains.Item;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Building;

[SerializableGameData]
public struct MakeResultStage : ISerializableGameData
{
	[SerializableGameDataField]
	public int LifeSkillRequiredAttainment;

	[SerializableGameDataField]
	public bool LifeSkillIsMeet;

	[SerializableGameDataField]
	private short _templateId;

	[SerializableGameDataField]
	public sbyte ItemType;

	[SerializableGameDataField]
	public List<short> TemplateIdList;

	[SerializableGameDataField]
	public List<short> SubTypeIdList;

	[SerializableGameDataField]
	public short SubTypeId;

	[SerializableGameDataField]
	public bool IsInit;

	public short TemplateId => _templateId;

	public MakeResultStage(int lifeSkillRequiredAttainment, bool lifeSkillIsMeet, sbyte itemType, short templateId, short subTypeId)
	{
		LifeSkillRequiredAttainment = lifeSkillRequiredAttainment;
		LifeSkillIsMeet = lifeSkillIsMeet;
		ItemType = itemType;
		_templateId = templateId;
		TemplateIdList = null;
		SubTypeIdList = null;
		SubTypeId = subTypeId;
		IsInit = true;
	}

	public MakeResultStage(int lifeSkillRequiredAttainment, bool lifeSkillIsMeet, sbyte itemType, List<short> templateIdList, List<short> subTypeIdList)
	{
		LifeSkillRequiredAttainment = lifeSkillRequiredAttainment;
		LifeSkillIsMeet = lifeSkillIsMeet;
		ItemType = itemType;
		TemplateIdList = templateIdList;
		SubTypeIdList = subTypeIdList;
		_templateId = -1;
		SubTypeId = -1;
		IsInit = true;
	}

	public (sbyte, short) GetGradeAndId(IRandomSource randomSource)
	{
		if (TemplateIdList != null && TemplateIdList.Count > 0)
		{
			_templateId = TemplateIdList.GetRandom(randomSource);
		}
		return (ItemTemplateHelper.GetGrade(ItemType, _templateId), _templateId);
	}

	public override string ToString()
	{
		string text = string.Empty;
		string empty = string.Empty;
		if (TemplateIdList != null && TemplateIdList.Count > 0)
		{
			foreach (short templateId in TemplateIdList)
			{
				empty = ItemTemplateHelper.GetName(ItemType, templateId);
				text = text + empty + " ";
			}
		}
		else
		{
			empty = ItemTemplateHelper.GetName(ItemType, _templateId);
			text = text + empty + " ";
		}
		return text;
	}

	public MakeResultStage(MakeResultStage other)
	{
		LifeSkillRequiredAttainment = other.LifeSkillRequiredAttainment;
		LifeSkillIsMeet = other.LifeSkillIsMeet;
		_templateId = other._templateId;
		ItemType = other.ItemType;
		TemplateIdList = new List<short>(other.TemplateIdList);
		SubTypeIdList = new List<short>(other.SubTypeIdList);
		SubTypeId = other.SubTypeId;
		IsInit = other.IsInit;
	}

	public void Assign(MakeResultStage other)
	{
		LifeSkillRequiredAttainment = other.LifeSkillRequiredAttainment;
		LifeSkillIsMeet = other.LifeSkillIsMeet;
		_templateId = other._templateId;
		ItemType = other.ItemType;
		TemplateIdList = new List<short>(other.TemplateIdList);
		SubTypeIdList = new List<short>(other.SubTypeIdList);
		SubTypeId = other.SubTypeId;
		IsInit = other.IsInit;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 11;
		num = ((TemplateIdList == null) ? (num + 2) : (num + (2 + 2 * TemplateIdList.Count)));
		num = ((SubTypeIdList == null) ? (num + 2) : (num + (2 + 2 * SubTypeIdList.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = LifeSkillRequiredAttainment;
		ptr += 4;
		*ptr = (LifeSkillIsMeet ? ((byte)1) : ((byte)0));
		ptr++;
		*(short*)ptr = _templateId;
		ptr += 2;
		*ptr = (byte)ItemType;
		ptr++;
		if (TemplateIdList != null)
		{
			int count = TemplateIdList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = TemplateIdList[i];
			}
			ptr += 2 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (SubTypeIdList != null)
		{
			int count2 = SubTypeIdList.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				((short*)ptr)[j] = SubTypeIdList[j];
			}
			ptr += 2 * count2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(short*)ptr = SubTypeId;
		ptr += 2;
		*ptr = (IsInit ? ((byte)1) : ((byte)0));
		ptr++;
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
		LifeSkillRequiredAttainment = *(int*)ptr;
		ptr += 4;
		LifeSkillIsMeet = *ptr != 0;
		ptr++;
		_templateId = *(short*)ptr;
		ptr += 2;
		ItemType = (sbyte)(*ptr);
		ptr++;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (TemplateIdList == null)
			{
				TemplateIdList = new List<short>(num);
			}
			else
			{
				TemplateIdList.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				TemplateIdList.Add(((short*)ptr)[i]);
			}
			ptr += 2 * num;
		}
		else
		{
			TemplateIdList?.Clear();
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (SubTypeIdList == null)
			{
				SubTypeIdList = new List<short>(num2);
			}
			else
			{
				SubTypeIdList.Clear();
			}
			for (int j = 0; j < num2; j++)
			{
				SubTypeIdList.Add(((short*)ptr)[j]);
			}
			ptr += 2 * num2;
		}
		else
		{
			SubTypeIdList?.Clear();
		}
		SubTypeId = *(short*)ptr;
		ptr += 2;
		IsInit = *ptr != 0;
		ptr++;
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
