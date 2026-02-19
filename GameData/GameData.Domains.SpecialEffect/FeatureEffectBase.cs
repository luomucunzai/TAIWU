using GameData.Serializer;

namespace GameData.Domains.SpecialEffect;

[SerializableGameData(NotForDisplayModule = true)]
public class FeatureEffectBase : SpecialEffectBase
{
	public short FeatureId;

	public FeatureEffectBase()
	{
	}

	public FeatureEffectBase(int charId, short featureId, int type)
		: base(charId, type)
	{
		FeatureId = featureId;
	}

	protected override int GetSubClassSerializedSize()
	{
		return 2;
	}

	protected unsafe override int SerializeSubClass(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = FeatureId;
		ptr += 2;
		return (int)(ptr - pData);
	}

	protected unsafe override int DeserializeSubClass(byte* pData)
	{
		byte* ptr = pData;
		FeatureId = *(short*)ptr;
		ptr += 2;
		return (int)(ptr - pData);
	}
}
