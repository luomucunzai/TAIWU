using System;
using System.Runtime.Serialization;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Item;

[Serializable]
public struct PoisonsAndLevels : ISerializableGameData, ISerializable, IEquatable<PoisonsAndLevels>
{
	public const int MaxLevel = 3;

	public unsafe fixed short Values[6];

	public unsafe fixed sbyte Levels[6];

	public bool IsMixed => GetTotalPoisonCount() > 1;

	public bool IsThreeMixed => GetTotalPoisonCount() == FullPoisonEffects.MaxSlotCount;

	public unsafe void Initialize()
	{
		fixed (short* values = Values)
		{
			*(long*)values = 0L;
			((int*)values)[2] = 0;
		}
		fixed (sbyte* levels = Levels)
		{
			*(int*)levels = 0;
			((short*)levels)[2] = 0;
		}
	}

	public unsafe PoisonsAndLevels(params short[] poisons)
	{
		for (int i = 0; i < 6; i++)
		{
			Values[i] = poisons[i * 2];
			Levels[i] = (sbyte)poisons[i * 2 + 1];
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		return 18;
	}

	public unsafe int Serialize(byte* pData)
	{
		fixed (short* values = Values)
		{
			*(long*)pData = *(long*)values;
			((int*)pData)[2] = ((int*)values)[2];
		}
		fixed (sbyte* levels = Levels)
		{
			((int*)pData)[3] = *(int*)levels;
			((short*)pData)[8] = ((short*)levels)[2];
		}
		return 18;
	}

	public unsafe int Deserialize(byte* pData)
	{
		fixed (short* values = Values)
		{
			*(long*)values = *(long*)pData;
			((int*)values)[2] = ((int*)pData)[2];
		}
		fixed (sbyte* levels = Levels)
		{
			*(int*)levels = ((int*)pData)[3];
			((short*)levels)[2] = ((short*)pData)[8];
		}
		return 18;
	}

	public unsafe PoisonsAndLevels(SerializationInfo info, StreamingContext context)
	{
		fixed (short* values = Values)
		{
			*(ulong*)values = info.GetUInt64("0");
			((int*)values)[2] = (int)info.GetUInt32("1");
		}
		fixed (sbyte* levels = Levels)
		{
			*(uint*)levels = info.GetUInt32("2");
			((short*)levels)[2] = (short)info.GetUInt16("3");
		}
	}

	public unsafe void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		fixed (short* values = Values)
		{
			info.AddValue("0", *(ulong*)values);
			info.AddValue("1", ((uint*)values)[2]);
		}
		fixed (sbyte* levels = Levels)
		{
			info.AddValue("2", *(uint*)levels);
			info.AddValue("3", ((ushort*)levels)[2]);
		}
	}

	public static int CalcPoisonDelta(int baseDelta, sbyte level, int currPoisonedValue, int poisonResist)
	{
		sbyte b = CalcPoisonedLevel(currPoisonedValue);
		if (baseDelta <= 0)
		{
			if (level < b)
			{
				return 0;
			}
			return baseDelta;
		}
		int num = 0;
		int poisonValue = baseDelta - baseDelta * poisonResist / 1000;
		poisonValue = CalcEffectivePoison(poisonValue, level, b);
		while (poisonValue > 0)
		{
			int num2 = ((b == 3) ? int.MaxValue : GlobalConfig.Instance.PoisonLevelThresholds[b]);
			if (poisonValue + currPoisonedValue > num2)
			{
				int num3 = num2 - currPoisonedValue;
				poisonValue -= num3;
				num += num3;
				currPoisonedValue = num2;
				if (b < 3)
				{
					b++;
				}
				if (b >= level)
				{
					if (b >= level + 3 - 1)
					{
						return num;
					}
					poisonValue /= 3;
				}
				continue;
			}
			return num + poisonValue;
		}
		return num;
	}

	private static int CalcEffectivePoison(int poisonValue, int poisonLevel, int currPoisonedLevel)
	{
		return (currPoisonedLevel - poisonLevel) switch
		{
			0 => poisonValue / 3, 
			1 => poisonValue / 9, 
			2 => 0, 
			_ => poisonValue, 
		};
	}

	public static short CalcApplyItemPoisonAmount(short value, sbyte level)
	{
		return (short)(value * level * GlobalConfig.Instance.CalcApplyItemPoisonParam);
	}

	public static sbyte CalcPoisonedLevel(int poisoned)
	{
		short[] poisonLevelThresholds = GlobalConfig.Instance.PoisonLevelThresholds;
		if (poisoned >= poisonLevelThresholds[2])
		{
			return 3;
		}
		if (poisoned >= poisonLevelThresholds[1])
		{
			return 2;
		}
		if (poisoned >= poisonLevelThresholds[0])
		{
			return 1;
		}
		return 0;
	}

	public static sbyte CalcGradeByPoisonAndLevel(short value, sbyte level)
	{
		int num = level - 1;
		int num2 = value / (10 * (1 << level - 1)) - 1;
		return (sbyte)MathUtils.Clamp(num * 3 + num2, 0, 8);
	}

	public unsafe sbyte GetGrade(sbyte poisonType)
	{
		if (Values[poisonType] <= 0 || Levels[poisonType] <= 0)
		{
			return -1;
		}
		return CalcGradeByPoisonAndLevel(Values[poisonType], Levels[poisonType]);
	}

	public short GetMixTemplateId()
	{
		if (!IsMixed)
		{
			return -1;
		}
		return MixedPoisonType.PoisonsAndLevelToMedicineTemplateId(ref this);
	}

	public unsafe bool IsNonZero()
	{
		fixed (short* values = Values)
		{
			if (*(long*)values != 0L)
			{
				return true;
			}
			if (((uint*)values)[2] != 0)
			{
				return true;
			}
		}
		return false;
	}

	public unsafe void Add(PoisonsAndLevels other)
	{
		for (int i = 0; i < 6; i++)
		{
			if (Levels[i] == other.Levels[i])
			{
				int num = Values[i] + other.Values[i];
				if (num > 25000)
				{
					num = 25000;
				}
				Values[i] = (short)num;
			}
			else if (Levels[i] < other.Levels[i])
			{
				Levels[i] = other.Levels[i];
				Values[i] = other.Values[i];
			}
		}
	}

	public unsafe bool Equals(PoisonsAndLevels other)
	{
		for (int i = 0; i < 6; i++)
		{
			if (Values[i] != other.Values[i])
			{
				return false;
			}
			if (Levels[i] != other.Levels[i])
			{
				return false;
			}
		}
		return true;
	}

	public unsafe readonly (short value, sbyte level) GetValueAndLevel(sbyte poisonType)
	{
		if ((poisonType < 0 || poisonType >= 6) ? true : false)
		{
			throw new ArgumentOutOfRangeException("poisonType");
		}
		return (value: Values[poisonType], level: Levels[poisonType]);
	}

	public unsafe sbyte GetTotalPoisonCount()
	{
		sbyte b = 0;
		for (int i = 0; i < 6; i++)
		{
			if (Values[i] > 0)
			{
				b++;
			}
		}
		return b;
	}

	public unsafe sbyte GetTotalLevel()
	{
		sbyte b = 0;
		for (int i = 0; i < 6; i++)
		{
			b += Levels[i];
		}
		return b;
	}
}
