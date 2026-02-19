using GameData.Domains.Item;
using GameData.Serializer;

namespace GameData.Domains.Taiwu.Display.VillagerRoleArrangement;

[SerializableGameData(NoCopyConstructors = true, NotForArchive = true)]
public class PriceSuppressionDisplayData : IVillagerRoleArrangementDisplayData, ISerializableGameData
{
	[SerializableGameDataField]
	public TemplateKey SuppressionItem;

	[SerializableGameDataField]
	public int PriceDropRate;

	[SerializableGameDataField]
	public int MaxPriceTimes;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 11;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += SuppressionItem.Serialize(ptr);
		*(int*)ptr = PriceDropRate;
		ptr += 4;
		*(int*)ptr = MaxPriceTimes;
		ptr += 4;
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
		ptr += SuppressionItem.Deserialize(ptr);
		PriceDropRate = *(int*)ptr;
		ptr += 4;
		MaxPriceTimes = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
