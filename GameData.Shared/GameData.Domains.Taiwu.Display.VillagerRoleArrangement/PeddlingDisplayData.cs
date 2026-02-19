using GameData.Serializer;

namespace GameData.Domains.Taiwu.Display.VillagerRoleArrangement;

[SerializableGameData(NoCopyConstructors = true, NotForArchive = true)]
public class PeddlingDisplayData : IVillagerRoleArrangementDisplayData, ISerializableGameData
{
	[SerializableGameDataField]
	public sbyte InteractTargetGrade;

	[SerializableGameDataField]
	public int BuyPriceRate;

	[SerializableGameDataField]
	public int SellPriceRate;

	[SerializableGameDataField]
	public int AddFavorA;

	[SerializableGameDataField]
	public int AddFavorB;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 17;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*pData = (byte)InteractTargetGrade;
		byte* num = pData + 1;
		*(int*)num = BuyPriceRate;
		byte* num2 = num + 4;
		*(int*)num2 = SellPriceRate;
		byte* num3 = num2 + 4;
		*(int*)num3 = AddFavorA;
		byte* num4 = num3 + 4;
		*(int*)num4 = AddFavorB;
		int num5 = (int)(num4 + 4 - pData);
		if (num5 > 4)
		{
			return (num5 + 3) / 4 * 4;
		}
		return num5;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		InteractTargetGrade = (sbyte)(*ptr);
		ptr++;
		BuyPriceRate = *(int*)ptr;
		ptr += 4;
		SellPriceRate = *(int*)ptr;
		ptr += 4;
		AddFavorA = *(int*)ptr;
		ptr += 4;
		AddFavorB = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
