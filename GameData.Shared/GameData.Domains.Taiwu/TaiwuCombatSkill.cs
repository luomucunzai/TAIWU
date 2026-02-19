using GameData.Domains.CombatSkill;
using GameData.Serializer;

namespace GameData.Domains.Taiwu;

public class TaiwuCombatSkill : ISerializableGameData, TaiwuSkill
{
	public static readonly sbyte[] FullPowerAddBreakSuccessRate = new sbyte[3] { 12, 6, 2 };

	private readonly sbyte[] _readingProgress;

	public int LastClearBreakPlateTime;

	public sbyte FullPowerCastTimes;

	public TaiwuCombatSkill()
	{
		_readingProgress = new sbyte[15];
		LastClearBreakPlateTime = -1000;
		FullPowerCastTimes = 0;
	}

	public TaiwuCombatSkill(ushort readingState)
		: this()
	{
		for (byte b = 0; b < 15; b++)
		{
			if (CombatSkillStateHelper.IsPageRead(readingState, b))
			{
				SetBookPageReadingProgress(b, 100);
			}
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 20;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		for (int i = 0; i < 15; i++)
		{
			ptr[i] = (byte)_readingProgress[i];
		}
		ptr += 15;
		*(int*)ptr = LastClearBreakPlateTime;
		ptr += 4;
		*ptr = (byte)FullPowerCastTimes;
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
		for (int i = 0; i < 15; i++)
		{
			_readingProgress[i] = (sbyte)ptr[i];
		}
		ptr += 15;
		LastClearBreakPlateTime = *(int*)ptr;
		ptr += 4;
		FullPowerCastTimes = (sbyte)(*ptr);
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public sbyte GetBookPageReadingProgress(byte pageInternalIndex)
	{
		return _readingProgress[pageInternalIndex];
	}

	public void SetBookPageReadingProgress(byte pageInternalIndex, sbyte progress)
	{
		_readingProgress[pageInternalIndex] = progress;
	}

	public sbyte[] GetAllBookPageReadingProgress()
	{
		return _readingProgress;
	}
}
