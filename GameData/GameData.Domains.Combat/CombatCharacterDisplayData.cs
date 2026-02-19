using GameData.Domains.Character;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat;

public class CombatCharacterDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public DefeatMarkCollection DefeatMarks;

	[SerializableGameDataField]
	public Injuries OldInjuries;

	[SerializableGameDataField]
	public PoisonInts OldPoisons;

	[SerializableGameDataField]
	public short OldDisorderOfQi;

	[SerializableGameDataField]
	public sbyte Happiness;

	public CombatCharacterDisplayData()
	{
	}

	public CombatCharacterDisplayData(CombatCharacterDisplayData other)
	{
		DefeatMarks = new DefeatMarkCollection(other.DefeatMarks);
		OldInjuries = other.OldInjuries;
		OldPoisons = other.OldPoisons;
		OldDisorderOfQi = other.OldDisorderOfQi;
		Happiness = other.Happiness;
	}

	public void Assign(CombatCharacterDisplayData other)
	{
		DefeatMarks = new DefeatMarkCollection(other.DefeatMarks);
		OldInjuries = other.OldInjuries;
		OldPoisons = other.OldPoisons;
		OldDisorderOfQi = other.OldDisorderOfQi;
		Happiness = other.Happiness;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 43;
		num = ((DefeatMarks == null) ? (num + 2) : (num + (2 + DefeatMarks.GetSerializedSize())));
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (DefeatMarks != null)
		{
			byte* ptr2 = ptr;
			ptr += 2;
			int num = DefeatMarks.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)ptr2 = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += OldInjuries.Serialize(ptr);
		ptr += OldPoisons.Serialize(ptr);
		*(short*)ptr = OldDisorderOfQi;
		ptr += 2;
		*ptr = (byte)Happiness;
		ptr++;
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (DefeatMarks == null)
			{
				DefeatMarks = new DefeatMarkCollection();
			}
			ptr += DefeatMarks.Deserialize(ptr);
		}
		else
		{
			DefeatMarks = null;
		}
		ptr += OldInjuries.Deserialize(ptr);
		ptr += OldPoisons.Deserialize(ptr);
		OldDisorderOfQi = *(short*)ptr;
		ptr += 2;
		Happiness = (sbyte)(*ptr);
		ptr++;
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}
}
