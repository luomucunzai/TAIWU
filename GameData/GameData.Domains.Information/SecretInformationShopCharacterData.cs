using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Information;

[SerializableGameData(IsExtensible = true, NotForDisplayModule = true)]
public class SecretInformationShopCharacterData : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort CollectedSecretInformationIds = 0;

		public const ushort Count = 1;

		public static readonly string[] FieldId2FieldName = new string[1] { "CollectedSecretInformationIds" };
	}

	[SerializableGameDataField]
	public List<int> CollectedSecretInformationIds = new List<int>();

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		num = ((CollectedSecretInformationIds == null) ? (num + 2) : (num + (2 + 4 * CollectedSecretInformationIds.Count)));
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 1;
		ptr += 2;
		if (CollectedSecretInformationIds != null)
		{
			int count = CollectedSecretInformationIds.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((int*)ptr)[i] = CollectedSecretInformationIds[i];
			}
			ptr += 4 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
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
				if (CollectedSecretInformationIds == null)
				{
					CollectedSecretInformationIds = new List<int>(num2);
				}
				else
				{
					CollectedSecretInformationIds.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					CollectedSecretInformationIds.Add(((int*)ptr)[i]);
				}
				ptr += 4 * num2;
			}
			else
			{
				CollectedSecretInformationIds?.Clear();
			}
		}
		int num3 = (int)(ptr - pData);
		return (num3 <= 4) ? num3 : ((num3 + 3) / 4 * 4);
	}
}
