using GameData.Serializer;

namespace GameData.Domains.SpecialEffect;

public class SpecialEffectWrapper : ISerializableGameData
{
	public SpecialEffectBase Effect;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num = 4 + Effect.GetSerializedSize();
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = Effect.Type;
		ptr += 4;
		ptr += Effect.Serialize(ptr);
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		int type = *(int*)ptr;
		ptr += 4;
		Effect = SpecialEffectType.CreateEffectObj(type);
		ptr += Effect.Deserialize(ptr);
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}
}
