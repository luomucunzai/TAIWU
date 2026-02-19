using GameData.Domains.CombatSkill;
using GameData.Serializer;

namespace GameData.Domains.Taiwu;

public class TaiwuLifeSkill : ISerializableGameData, TaiwuSkill
{
	private readonly sbyte[] _readingProgress;

	public TaiwuLifeSkill()
	{
		_readingProgress = new sbyte[5];
	}

	public TaiwuLifeSkill(ushort readingState)
		: this()
	{
		for (byte b = 0; b < 5; b++)
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
		return 5;
	}

	public unsafe int Serialize(byte* pData)
	{
		for (int i = 0; i < 5; i++)
		{
			pData[i] = (byte)_readingProgress[i];
		}
		return 5;
	}

	public unsafe int Deserialize(byte* pData)
	{
		for (int i = 0; i < 5; i++)
		{
			_readingProgress[i] = (sbyte)pData[i];
		}
		return 5;
	}

	public sbyte GetBookPageReadingProgress(byte index)
	{
		return _readingProgress[index];
	}

	public void SetBookPageReadingProgress(byte index, sbyte progress)
	{
		_readingProgress[index] = progress;
	}

	public sbyte[] GetAllBookPageReadingProgress()
	{
		return _readingProgress;
	}
}
