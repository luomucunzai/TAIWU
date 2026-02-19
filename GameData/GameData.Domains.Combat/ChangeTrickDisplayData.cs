using GameData.Serializer;

namespace GameData.Domains.Combat;

[SerializableGameData(NotForArchive = true)]
public struct ChangeTrickDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public bool CanChangeTrick;

	[SerializableGameDataField]
	public sbyte CostCount;

	[SerializableGameDataField]
	public short AddHitRate;

	[SerializableGameDataField]
	public short AddBreakBlock;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 6;
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (CanChangeTrick ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (byte)CostCount;
		ptr++;
		*(short*)ptr = AddHitRate;
		ptr += 2;
		*(short*)ptr = AddBreakBlock;
		ptr += 2;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		CanChangeTrick = *ptr != 0;
		ptr++;
		CostCount = (sbyte)(*ptr);
		ptr++;
		AddHitRate = *(short*)ptr;
		ptr += 2;
		AddBreakBlock = *(short*)ptr;
		ptr += 2;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}
}
