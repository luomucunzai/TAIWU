using System;
using GameData.Serializer;

namespace GameData.Domains.Character;

[SerializableGameData(NotForArchive = true)]
public struct CombatResources : ISerializableGameData
{
	[SerializableGameDataField]
	public sbyte HealingCount;

	[SerializableGameDataField]
	public sbyte DetoxCount;

	[SerializableGameDataField]
	public sbyte BreathingCount;

	[SerializableGameDataField]
	public sbyte RecoverCount;

	public const sbyte BaseCount = 1;

	public const sbyte MaxCount = 99;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*pData = (byte)HealingCount;
		byte* num = pData + 1;
		*num = (byte)DetoxCount;
		byte* num2 = num + 1;
		*num2 = (byte)BreathingCount;
		byte* num3 = num2 + 1;
		*num3 = (byte)RecoverCount;
		int num4 = (int)(num3 + 1 - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		HealingCount = (sbyte)(*ptr);
		ptr++;
		DetoxCount = (sbyte)(*ptr);
		ptr++;
		BreathingCount = (sbyte)(*ptr);
		ptr++;
		RecoverCount = (sbyte)(*ptr);
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	private static sbyte ClampCount(int count)
	{
		return (sbyte)Math.Clamp(count, 0, 99);
	}

	public CombatResources Sub(CombatResources used)
	{
		return new CombatResources
		{
			HealingCount = ClampCount(HealingCount - used.HealingCount),
			DetoxCount = ClampCount(DetoxCount - used.DetoxCount),
			BreathingCount = ClampCount(BreathingCount - used.BreathingCount),
			RecoverCount = ClampCount(RecoverCount - used.RecoverCount)
		};
	}

	public sbyte Get(EHealActionType healType)
	{
		return healType switch
		{
			EHealActionType.Healing => HealingCount, 
			EHealActionType.Detox => DetoxCount, 
			EHealActionType.Breathing => BreathingCount, 
			EHealActionType.Recover => RecoverCount, 
			_ => throw new ArgumentOutOfRangeException("healType", healType, null), 
		};
	}

	public void Set(EHealActionType healType, int count)
	{
		switch (healType)
		{
		case EHealActionType.Healing:
		{
			sbyte b = (HealingCount = ClampCount(count));
			break;
		}
		case EHealActionType.Detox:
		{
			sbyte b = (DetoxCount = ClampCount(count));
			break;
		}
		case EHealActionType.Breathing:
		{
			sbyte b = (BreathingCount = ClampCount(count));
			break;
		}
		case EHealActionType.Recover:
		{
			sbyte b = (RecoverCount = ClampCount(count));
			break;
		}
		default:
			throw new ArgumentOutOfRangeException("healType", healType, null);
		}
	}

	public void Change(EHealActionType healType, int delta)
	{
		Set(healType, Get(healType) + delta);
	}
}
