using System;
using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Domains.Adventure;

[Serializable]
public class AdventureMapPoint : ISerializableGameData
{
	[SerializableGameDataField]
	public int TerrainId;

	[SerializableGameDataField]
	public sbyte NodeType;

	[SerializableGameDataField]
	public short PosX;

	[SerializableGameDataField]
	public short PosY;

	[SerializableGameDataField]
	public sbyte SevenElementType;

	[SerializableGameDataField]
	public sbyte SevenElementCost;

	[SerializableGameDataField]
	public sbyte NodeContentType;

	[SerializableGameDataField]
	public int NodeContentIndex;

	public bool JudgeSuccess;

	[SerializableGameDataField]
	public sbyte AffiliatedBranchIdx;

	[SerializableGameDataField]
	public short JudgeSkill = -1;

	[SerializableGameDataField]
	public short JudgeValue;

	[SerializableGameDataField]
	public short Index;

	public bool IsEvent
	{
		get
		{
			if (NodeContentType != 0)
			{
				return NodeContentType == 10;
			}
			return true;
		}
	}

	public bool NeedAttainment
	{
		get
		{
			if (JudgeSkill >= 0 && JudgeValue > 0 && NodeContentType >= 0)
			{
				return !IsEvent;
			}
			return false;
		}
	}

	public void Assign(AdventureMapPoint other)
	{
		TerrainId = other.TerrainId;
		NodeType = other.NodeType;
		PosX = other.PosX;
		PosY = other.PosY;
		SevenElementType = other.SevenElementType;
		SevenElementCost = other.SevenElementCost;
		NodeContentType = other.NodeContentType;
		NodeContentIndex = other.NodeContentIndex;
		AffiliatedBranchIdx = other.AffiliatedBranchIdx;
		JudgeSkill = other.JudgeSkill;
		JudgeValue = other.JudgeValue;
		Index = other.Index;
	}

	public override string ToString()
	{
		return $"{PosX}:{PosY}";
	}

	public string GetDetailedInfo()
	{
		return $"{{Position:({PosX},{PosY}),NodeType:{NodeType},SevenElementType:{SevenElementType},NodeContentType{NodeContentType},NodeContentIndex{NodeContentIndex},AffiliatedBranchIndex:{AffiliatedBranchIdx}}}";
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 23;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(int*)pData = TerrainId;
		byte* num = pData + 4;
		*num = (byte)NodeType;
		byte* num2 = num + 1;
		*(short*)num2 = PosX;
		byte* num3 = num2 + 2;
		*(short*)num3 = PosY;
		byte* num4 = num3 + 2;
		*num4 = (byte)SevenElementType;
		byte* num5 = num4 + 1;
		*num5 = (byte)SevenElementCost;
		byte* num6 = num5 + 1;
		*num6 = (byte)NodeContentType;
		byte* num7 = num6 + 1;
		*(int*)num7 = NodeContentIndex;
		byte* num8 = num7 + 4;
		*num8 = (byte)AffiliatedBranchIdx;
		byte* num9 = num8 + 1;
		*(short*)num9 = JudgeSkill;
		byte* num10 = num9 + 2;
		*(short*)num10 = JudgeValue;
		byte* num11 = num10 + 2;
		*(short*)num11 = Index;
		int num12 = (int)(num11 + 2 - pData);
		if (num12 > 4)
		{
			return (num12 + 3) / 4 * 4;
		}
		return num12;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		TerrainId = *(int*)ptr;
		ptr += 4;
		NodeType = (sbyte)(*ptr);
		ptr++;
		PosX = *(short*)ptr;
		ptr += 2;
		PosY = *(short*)ptr;
		ptr += 2;
		SevenElementType = (sbyte)(*ptr);
		ptr++;
		SevenElementCost = (sbyte)(*ptr);
		ptr++;
		NodeContentType = (sbyte)(*ptr);
		ptr++;
		NodeContentIndex = *(int*)ptr;
		ptr += 4;
		AffiliatedBranchIdx = (sbyte)(*ptr);
		ptr++;
		JudgeSkill = *(short*)ptr;
		ptr += 2;
		JudgeValue = *(short*)ptr;
		ptr += 2;
		Index = *(short*)ptr;
		ptr += 2;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public bool GetAttainmentEnough(List<short> lifeSkillAttainments)
	{
		if (NeedAttainment)
		{
			return lifeSkillAttainments[JudgeSkill] >= JudgeValue;
		}
		return false;
	}
}
