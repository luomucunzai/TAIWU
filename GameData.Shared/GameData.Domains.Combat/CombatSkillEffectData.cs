using GameData.Serializer;

namespace GameData.Domains.Combat;

[SerializableGameData(NotForArchive = true)]
public struct CombatSkillEffectData : ISerializableGameData
{
	[SerializableGameDataField]
	private sbyte _internalType;

	[SerializableGameDataField]
	private int _internalParam0;

	public ECombatSkillEffectType Type => (ECombatSkillEffectType)_internalType;

	public int Value => _internalParam0;

	public CombatSkillEffectData(ECombatSkillEffectType type, int param0)
	{
		_internalType = (sbyte)type;
		_internalParam0 = param0;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 5;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*pData = (byte)_internalType;
		byte* num = pData + 1;
		*(int*)num = _internalParam0;
		int num2 = (int)(num + 4 - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		_internalType = (sbyte)(*ptr);
		ptr++;
		_internalParam0 = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
