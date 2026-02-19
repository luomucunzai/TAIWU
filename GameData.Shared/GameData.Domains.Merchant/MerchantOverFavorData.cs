using System;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Merchant;

[SerializableGameData(IsExtensible = true, NoCopyConstructors = true)]
public class MerchantOverFavorData : ISerializableGameData, ICloneable
{
	private static class FieldIds
	{
		public const ushort MerchantOverFavorLevelDataArray = 0;

		public const ushort Count = 1;

		public static readonly string[] FieldId2FieldName = new string[1] { "MerchantOverFavorLevelDataArray" };
	}

	[SerializableGameDataField]
	public MerchantOverFavorLevelData[] MerchantOverFavorLevelDataArray = new MerchantOverFavorLevelData[7];

	public object Clone()
	{
		MerchantOverFavorData merchantOverFavorData = new MerchantOverFavorData
		{
			MerchantOverFavorLevelDataArray = new MerchantOverFavorLevelData[MerchantOverFavorLevelDataArray.Length]
		};
		for (int i = 0; i < MerchantOverFavorLevelDataArray.Length; i++)
		{
			MerchantOverFavorLevelData merchantOverFavorLevelData = MerchantOverFavorLevelDataArray[i];
			merchantOverFavorData.MerchantOverFavorLevelDataArray[i] = merchantOverFavorLevelData?.Clone() as MerchantOverFavorLevelData;
		}
		return merchantOverFavorData;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		if (MerchantOverFavorLevelDataArray != null)
		{
			num += 2;
			int num2 = MerchantOverFavorLevelDataArray.Length;
			for (int i = 0; i < num2; i++)
			{
				MerchantOverFavorLevelData merchantOverFavorLevelData = MerchantOverFavorLevelDataArray[i];
				num = ((merchantOverFavorLevelData == null) ? (num + 2) : (num + (2 + merchantOverFavorLevelData.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 1;
		ptr += 2;
		if (MerchantOverFavorLevelDataArray != null)
		{
			int num = MerchantOverFavorLevelDataArray.Length;
			Tester.Assert(num <= 65535);
			*(ushort*)ptr = (ushort)num;
			ptr += 2;
			for (int i = 0; i < num; i++)
			{
				MerchantOverFavorLevelData merchantOverFavorLevelData = MerchantOverFavorLevelDataArray[i];
				if (merchantOverFavorLevelData != null)
				{
					byte* intPtr = ptr;
					ptr += 2;
					int num2 = merchantOverFavorLevelData.Serialize(ptr);
					ptr += num2;
					Tester.Assert(num2 <= 65535);
					*(ushort*)intPtr = (ushort)num2;
				}
				else
				{
					*(short*)ptr = 0;
					ptr += 2;
				}
			}
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
				if (MerchantOverFavorLevelDataArray == null || MerchantOverFavorLevelDataArray.Length != num2)
				{
					MerchantOverFavorLevelDataArray = new MerchantOverFavorLevelData[num2];
				}
				for (int i = 0; i < num2; i++)
				{
					ushort num3 = *(ushort*)ptr;
					ptr += 2;
					if (num3 > 0)
					{
						MerchantOverFavorLevelData merchantOverFavorLevelData = MerchantOverFavorLevelDataArray[i] ?? new MerchantOverFavorLevelData();
						ptr += merchantOverFavorLevelData.Deserialize(ptr);
						MerchantOverFavorLevelDataArray[i] = merchantOverFavorLevelData;
					}
					else
					{
						MerchantOverFavorLevelDataArray[i] = null;
					}
				}
			}
			else
			{
				MerchantOverFavorLevelDataArray = null;
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
