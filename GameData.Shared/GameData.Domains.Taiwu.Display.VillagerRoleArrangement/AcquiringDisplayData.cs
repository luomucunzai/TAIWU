using GameData.Domains.Item;
using GameData.Serializer;

namespace GameData.Domains.Taiwu.Display.VillagerRoleArrangement;

[SerializableGameData(NoCopyConstructors = true, NotForArchive = true)]
public class AcquiringDisplayData : IVillagerRoleArrangementDisplayData, ISerializableGameData
{
	[SerializableGameDataField]
	public int ExtraBuyPossibility;

	[SerializableGameDataField]
	public int PricePercent;

	[SerializableGameDataField]
	public TemplateKey AcquiringItem;

	[SerializableGameDataField]
	public int ExtraMerchantFavor;

	[SerializableGameDataField]
	public int BoughtAmount;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 19;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = ExtraBuyPossibility;
		ptr += 4;
		*(int*)ptr = PricePercent;
		ptr += 4;
		ptr += AcquiringItem.Serialize(ptr);
		*(int*)ptr = ExtraMerchantFavor;
		ptr += 4;
		*(int*)ptr = BoughtAmount;
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
		ExtraBuyPossibility = *(int*)ptr;
		ptr += 4;
		PricePercent = *(int*)ptr;
		ptr += 4;
		ptr += AcquiringItem.Deserialize(ptr);
		ExtraMerchantFavor = *(int*)ptr;
		ptr += 4;
		BoughtAmount = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
