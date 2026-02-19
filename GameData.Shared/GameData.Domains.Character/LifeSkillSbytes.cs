using GameData.Serializer;

namespace GameData.Domains.Character;

public struct LifeSkillSbytes : ISerializableGameData
{
	public unsafe fixed sbyte Items[16];

	public unsafe void Initialize()
	{
		fixed (sbyte* items = Items)
		{
			*(long*)items = 0L;
			((long*)items)[1] = 0L;
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		return 16;
	}

	public unsafe int Serialize(byte* pData)
	{
		fixed (sbyte* items = Items)
		{
			*(long*)pData = *(long*)items;
			((long*)pData)[1] = ((long*)items)[1];
		}
		return 16;
	}

	public unsafe int Deserialize(byte* pData)
	{
		fixed (sbyte* items = Items)
		{
			*(long*)items = *(long*)pData;
			((long*)items)[1] = ((long*)pData)[1];
		}
		return 16;
	}

	public unsafe LifeSkillSbytes Subtract(LifeSkillSbytes other)
	{
		LifeSkillSbytes result = default(LifeSkillSbytes);
		for (int i = 0; i < 16; i++)
		{
			result.Items[i] = (sbyte)(Items[i] - other.Items[i]);
		}
		return result;
	}

	public unsafe LifeSkillSbytes GetReversed()
	{
		LifeSkillSbytes result = default(LifeSkillSbytes);
		for (int i = 0; i < 16; i++)
		{
			result.Items[i] = (sbyte)(-Items[i]);
		}
		return result;
	}
}
