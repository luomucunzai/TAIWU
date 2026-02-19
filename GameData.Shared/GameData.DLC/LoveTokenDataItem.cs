using GameData.Serializer;

namespace GameData.DLC;

public class LoveTokenDataItem : ISerializableGameData
{
	[SerializableGameDataField]
	public int TaiwuCharId;

	[SerializableGameDataField]
	public int LoverCharId;

	[SerializableGameDataField]
	public int BecomeLoverTime;

	[SerializableGameDataField]
	public int CurHolderCharId;

	[SerializableGameDataField]
	public bool IsTaiwuPresent;

	public bool IsValid
	{
		get
		{
			if (LoverCharId > -1)
			{
				return TaiwuCharId > -1;
			}
			return false;
		}
	}

	public LoveTokenDataItem(int becomeLoverTime, int loverCharId, int taiwuCharId, int curHolderCharId, bool isTaiwuPresent)
	{
		BecomeLoverTime = becomeLoverTime;
		LoverCharId = loverCharId;
		TaiwuCharId = taiwuCharId;
		CurHolderCharId = curHolderCharId;
		IsTaiwuPresent = isTaiwuPresent;
	}

	public LoveTokenDataItem()
	{
		BecomeLoverTime = 0;
		LoverCharId = -1;
		TaiwuCharId = -1;
		CurHolderCharId = -1;
		IsTaiwuPresent = false;
	}

	public LoveTokenDataItem(LoveTokenDataItem other)
	{
		TaiwuCharId = other.TaiwuCharId;
		LoverCharId = other.LoverCharId;
		BecomeLoverTime = other.BecomeLoverTime;
		CurHolderCharId = other.CurHolderCharId;
		IsTaiwuPresent = other.IsTaiwuPresent;
	}

	public void Assign(LoveTokenDataItem other)
	{
		TaiwuCharId = other.TaiwuCharId;
		LoverCharId = other.LoverCharId;
		BecomeLoverTime = other.BecomeLoverTime;
		CurHolderCharId = other.CurHolderCharId;
		IsTaiwuPresent = other.IsTaiwuPresent;
	}

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
		*(int*)pData = TaiwuCharId;
		byte* num = pData + 4;
		*(int*)num = LoverCharId;
		byte* num2 = num + 4;
		*(int*)num2 = BecomeLoverTime;
		byte* num3 = num2 + 4;
		*(int*)num3 = CurHolderCharId;
		byte* num4 = num3 + 4;
		*num4 = (IsTaiwuPresent ? ((byte)1) : ((byte)0));
		int num5 = (int)(num4 + 1 - pData);
		if (num5 > 4)
		{
			return (num5 + 3) / 4 * 4;
		}
		return num5;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		TaiwuCharId = *(int*)ptr;
		ptr += 4;
		LoverCharId = *(int*)ptr;
		ptr += 4;
		BecomeLoverTime = *(int*)ptr;
		ptr += 4;
		CurHolderCharId = *(int*)ptr;
		ptr += 4;
		IsTaiwuPresent = *ptr != 0;
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
