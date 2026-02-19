using GameData.Serializer;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Building;

[AutoGenerateSerializableGameData]
public struct ProductionData : ISerializableGameData
{
	[SerializableGameDataField]
	public int Weight;

	[SerializableGameDataField]
	public bool CanProduce;

	public ProductionData(int weight, bool canProduce)
	{
		Weight = weight;
		CanProduce = canProduce;
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
		*(int*)pData = Weight;
		byte* num = pData + 4;
		*num = (CanProduce ? ((byte)1) : ((byte)0));
		int num2 = (int)(num + 1 - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		Weight = *(int*)ptr;
		ptr += 4;
		CanProduce = *ptr != 0;
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
