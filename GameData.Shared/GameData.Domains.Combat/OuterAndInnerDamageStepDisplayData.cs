using GameData.Serializer;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Combat;

[AutoGenerateSerializableGameData(NoCopyConstructors = true, NotForArchive = true)]
public struct OuterAndInnerDamageStepDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public DamageStepDisplayData Outer;

	[SerializableGameDataField]
	public DamageStepDisplayData Inner;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 48;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += Outer.Serialize(ptr);
		ptr += Inner.Serialize(ptr);
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
		ptr += Outer.Deserialize(ptr);
		ptr += Inner.Deserialize(ptr);
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
