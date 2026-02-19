using GameData.Domains.Information.Collection;
using GameData.Domains.Map;
using GameData.Serializer;

namespace GameData.Domains.Information;

[SerializableGameData(NotRestrictCollectionSerializedSize = true)]
public class SecretInformationDisplayData : ISerializableGameData
{
	public static class FilterFlag
	{
		public const byte RelatedToTaiwu = 1;

		public const byte NotRelatedToTaiwu = 2;

		public const byte RelatedToTaiwuFriendly = 4;

		public const byte RelatedToTaiwuEnemy = 8;

		public const byte Other = 240;
	}

	[SerializableGameDataField]
	public int SecretInformationMetaDataId;

	[SerializableGameDataField]
	public short SecretInformationTemplateId;

	[SerializableGameDataField]
	public int HolderCount;

	[SerializableGameDataField]
	public int SourceCharacterId;

	[SerializableGameDataField]
	public int AuthorityCostWhenDisseminating;

	[SerializableGameDataField]
	public int AuthorityCostWhenDisseminatingForBroadcast;

	[SerializableGameDataField]
	public SecretInformationCollection RawData;

	[SerializableGameDataField]
	public int UsedCount;

	[SerializableGameDataField]
	public FullBlockName Location;

	[SerializableGameDataField]
	public byte FilterMask;

	[SerializableGameDataField]
	public sbyte DisplaySize;

	[SerializableGameDataField]
	public int ShopValue;

	public static bool IsSecretInformationValid(int metaDataId)
	{
		return metaDataId >= 0;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 32;
		num = ((RawData == null) ? (num + 2) : (num + (2 + RawData.GetSerializedSize())));
		num += 2 + Location.GetSerializedSize();
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = SecretInformationMetaDataId;
		ptr += 4;
		*(short*)ptr = SecretInformationTemplateId;
		ptr += 2;
		*(int*)ptr = HolderCount;
		ptr += 4;
		*(int*)ptr = SourceCharacterId;
		ptr += 4;
		*(int*)ptr = UsedCount;
		ptr += 4;
		*(int*)ptr = AuthorityCostWhenDisseminating;
		ptr += 4;
		*(int*)ptr = AuthorityCostWhenDisseminatingForBroadcast;
		ptr += 4;
		*ptr = FilterMask;
		ptr++;
		*ptr = (byte)DisplaySize;
		ptr++;
		*(int*)ptr = ShopValue;
		ptr += 4;
		if (RawData != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = RawData.Serialize(ptr);
			ptr += num;
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += Location.Serialize(ptr);
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		SecretInformationMetaDataId = *(int*)ptr;
		ptr += 4;
		SecretInformationTemplateId = *(short*)ptr;
		ptr += 2;
		HolderCount = *(int*)ptr;
		ptr += 4;
		SourceCharacterId = *(int*)ptr;
		ptr += 4;
		UsedCount = *(int*)ptr;
		ptr += 4;
		AuthorityCostWhenDisseminating = *(int*)ptr;
		ptr += 4;
		AuthorityCostWhenDisseminatingForBroadcast = *(int*)ptr;
		ptr += 4;
		FilterMask = *ptr;
		ptr++;
		DisplaySize = (sbyte)(*ptr);
		ptr++;
		ShopValue = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (RawData == null)
			{
				RawData = new SecretInformationCollection();
			}
			ptr += RawData.Deserialize(ptr);
		}
		else
		{
			RawData = null;
		}
		ptr += Location.Deserialize(ptr);
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
