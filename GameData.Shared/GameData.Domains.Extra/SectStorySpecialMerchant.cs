using GameData.Domains.Merchant;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Extra;

[SerializableGameData(IsExtensible = true, NoCopyConstructors = true)]
public class SectStorySpecialMerchant : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort MerchantData = 0;

		public const ushort RefreshTime = 1;

		public const ushort MerchantExtraGoodsData = 2;

		public const ushort Count = 3;

		public static readonly string[] FieldId2FieldName = new string[3] { "MerchantData", "RefreshTime", "MerchantExtraGoodsData" };
	}

	[SerializableGameDataField]
	public MerchantData MerchantData;

	[SerializableGameDataField]
	public int RefreshTime;

	[SerializableGameDataField]
	public MerchantExtraGoodsData MerchantExtraGoodsData;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 6;
		num = ((MerchantData == null) ? (num + 2) : (num + (2 + MerchantData.GetSerializedSize())));
		num = ((MerchantExtraGoodsData == null) ? (num + 2) : (num + (2 + MerchantExtraGoodsData.GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 3;
		ptr += 2;
		if (MerchantData != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = MerchantData.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(int*)ptr = RefreshTime;
		ptr += 4;
		if (MerchantExtraGoodsData != null)
		{
			byte* intPtr2 = ptr;
			ptr += 2;
			int num2 = MerchantExtraGoodsData.Serialize(ptr);
			ptr += num2;
			Tester.Assert(num2 <= 65535);
			*(ushort*)intPtr2 = (ushort)num2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (MerchantData == null)
				{
					MerchantData = new MerchantData();
				}
				ptr += MerchantData.Deserialize(ptr);
			}
			else
			{
				MerchantData = null;
			}
		}
		if (num > 1)
		{
			RefreshTime = *(int*)ptr;
			ptr += 4;
		}
		if (num > 2)
		{
			ushort num3 = *(ushort*)ptr;
			ptr += 2;
			if (num3 > 0)
			{
				if (MerchantExtraGoodsData == null)
				{
					MerchantExtraGoodsData = new MerchantExtraGoodsData();
				}
				ptr += MerchantExtraGoodsData.Deserialize(ptr);
			}
			else
			{
				MerchantExtraGoodsData = null;
			}
		}
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}
}
