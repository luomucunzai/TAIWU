using GameData.Domains.Character;
using GameData.Serializer;

namespace GameData.Domains.Combat;

public class DamageCompareData : ISerializableGameData
{
	public bool IsAlly;

	public short SkillId;

	public int OuterAttackValue;

	public int InnerAttackValue;

	public int OuterDefendValue;

	public int InnerDefendValue;

	public readonly sbyte[] HitType = new sbyte[3];

	public readonly int[] HitValue = new int[3];

	public readonly int[] AvoidValue = new int[3];

	public DamageCompareData()
	{
		Clear();
	}

	public void Clear()
	{
		SkillId = -1;
		OuterAttackValue = -1;
		InnerAttackValue = -1;
		OuterDefendValue = -1;
		InnerDefendValue = -1;
		for (int i = 0; i < 3; i++)
		{
			HitType[i] = -1;
			HitValue[i] = -1;
			AvoidValue[i] = -1;
		}
	}

	public CombatProperty GetProperty(int index = 0)
	{
		return new CombatProperty
		{
			HitValue = HitValue[index],
			AvoidValue = AvoidValue[index],
			AttackValue = new OuterAndInnerInts(OuterAttackValue, InnerAttackValue),
			DefendValue = new OuterAndInnerInts(OuterDefendValue, InnerDefendValue)
		};
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 46;
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (IsAlly ? ((byte)1) : ((byte)0));
		ptr++;
		*(short*)ptr = SkillId;
		ptr += 2;
		*(int*)ptr = OuterAttackValue;
		ptr += 4;
		*(int*)ptr = InnerAttackValue;
		ptr += 4;
		*(int*)ptr = OuterDefendValue;
		ptr += 4;
		*(int*)ptr = InnerDefendValue;
		ptr += 4;
		for (int i = 0; i < 3; i++)
		{
			*ptr = (byte)HitType[i];
			ptr++;
			*(int*)ptr = HitValue[i];
			ptr += 4;
			*(int*)ptr = AvoidValue[i];
			ptr += 4;
		}
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		IsAlly = *ptr != 0;
		ptr++;
		SkillId = *(short*)ptr;
		ptr += 2;
		OuterAttackValue = *(int*)ptr;
		ptr += 4;
		InnerAttackValue = *(int*)ptr;
		ptr += 4;
		OuterDefendValue = *(int*)ptr;
		ptr += 4;
		InnerDefendValue = *(int*)ptr;
		ptr += 4;
		for (int i = 0; i < 3; i++)
		{
			HitType[i] = (sbyte)(*ptr);
			ptr++;
			HitValue[i] = *(int*)ptr;
			ptr += 4;
			AvoidValue[i] = *(int*)ptr;
			ptr += 4;
		}
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}
}
