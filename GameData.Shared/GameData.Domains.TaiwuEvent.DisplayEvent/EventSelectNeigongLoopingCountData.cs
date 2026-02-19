using GameData.Domains.CombatSkill;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.DisplayEvent;

[SerializableGameData(NoCopyConstructors = true)]
public class EventSelectNeigongLoopingCountData : ISerializableGameData
{
	[SerializableGameDataField]
	public CombatSkillDisplayData SelectedCombatSkill;

	[SerializableGameDataField]
	public int MaxLoopingCount;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		num = ((SelectedCombatSkill == null) ? (num + 2) : (num + (2 + SelectedCombatSkill.GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (SelectedCombatSkill != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = SelectedCombatSkill.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(int*)ptr = MaxLoopingCount;
		ptr += 4;
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (SelectedCombatSkill == null)
			{
				SelectedCombatSkill = new CombatSkillDisplayData();
			}
			ptr += SelectedCombatSkill.Deserialize(ptr);
		}
		else
		{
			SelectedCombatSkill = null;
		}
		MaxLoopingCount = *(int*)ptr;
		ptr += 4;
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
