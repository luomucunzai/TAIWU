using System.Collections.Generic;
using System.Text;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.DisplayEvent;

public class EventSelectCombatSkillData : ISerializableGameData
{
	[SerializableGameDataField]
	public string ResultSaveKey;

	[SerializableGameDataField]
	public string OptionKey;

	[SerializableGameDataField]
	public int CharId;

	[SerializableGameDataField]
	public List<short> CanSelectCombatSkillIdList;

	public int SelectResultIndex;

	public EventSelectCombatSkillData()
	{
	}

	public EventSelectCombatSkillData(EventSelectCombatSkillData other)
	{
		ResultSaveKey = other.ResultSaveKey;
		OptionKey = other.OptionKey;
		CharId = other.CharId;
		CanSelectCombatSkillIdList = new List<short>(other.CanSelectCombatSkillIdList);
	}

	public void Assign(EventSelectCombatSkillData other)
	{
		ResultSaveKey = other.ResultSaveKey;
		OptionKey = other.OptionKey;
		CharId = other.CharId;
		CanSelectCombatSkillIdList = new List<short>(other.CanSelectCombatSkillIdList);
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		num = ((ResultSaveKey == null) ? (num + 2) : (num + (2 + 2 * ResultSaveKey.Length)));
		num = ((OptionKey == null) ? (num + 2) : (num + (2 + 2 * OptionKey.Length)));
		num = ((CanSelectCombatSkillIdList == null) ? (num + 2) : (num + (2 + 2 * CanSelectCombatSkillIdList.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (ResultSaveKey != null)
		{
			int length = ResultSaveKey.Length;
			Tester.Assert(length <= 65535);
			*(ushort*)ptr = (ushort)length;
			ptr += 2;
			fixed (char* resultSaveKey = ResultSaveKey)
			{
				for (int i = 0; i < length; i++)
				{
					((short*)ptr)[i] = (short)resultSaveKey[i];
				}
			}
			ptr += 2 * length;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (OptionKey != null)
		{
			int length2 = OptionKey.Length;
			Tester.Assert(length2 <= 65535);
			*(ushort*)ptr = (ushort)length2;
			ptr += 2;
			fixed (char* optionKey = OptionKey)
			{
				for (int j = 0; j < length2; j++)
				{
					((short*)ptr)[j] = (short)optionKey[j];
				}
			}
			ptr += 2 * length2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(int*)ptr = CharId;
		ptr += 4;
		if (CanSelectCombatSkillIdList != null)
		{
			int count = CanSelectCombatSkillIdList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int k = 0; k < count; k++)
			{
				((short*)ptr)[k] = CanSelectCombatSkillIdList[k];
			}
			ptr += 2 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
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
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			int num2 = 2 * num;
			ResultSaveKey = Encoding.Unicode.GetString(ptr, num2);
			ptr += num2;
		}
		else
		{
			ResultSaveKey = null;
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			int num4 = 2 * num3;
			OptionKey = Encoding.Unicode.GetString(ptr, num4);
			ptr += num4;
		}
		else
		{
			OptionKey = null;
		}
		CharId = *(int*)ptr;
		ptr += 4;
		ushort num5 = *(ushort*)ptr;
		ptr += 2;
		if (num5 > 0)
		{
			if (CanSelectCombatSkillIdList == null)
			{
				CanSelectCombatSkillIdList = new List<short>(num5);
			}
			else
			{
				CanSelectCombatSkillIdList.Clear();
			}
			for (int i = 0; i < num5; i++)
			{
				CanSelectCombatSkillIdList.Add(((short*)ptr)[i]);
			}
			ptr += 2 * num5;
		}
		else
		{
			CanSelectCombatSkillIdList?.Clear();
		}
		int num6 = (int)(ptr - pData);
		if (num6 > 4)
		{
			return (num6 + 3) / 4 * 4;
		}
		return num6;
	}
}
