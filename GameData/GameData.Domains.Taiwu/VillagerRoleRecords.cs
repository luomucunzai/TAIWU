using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Taiwu;

[AutoGenerateSerializableGameData(IsExtensible = true, NoCopyConstructors = true, NotForDisplayModule = true)]
public class VillagerRoleRecords : ISerializableGameData
{
	public static class FieldIds
	{
		public const ushort History = 0;

		public const ushort Count = 1;

		public static readonly string[] FieldId2FieldName = new string[1] { "History" };
	}

	[SerializableGameDataField(FieldIndex = 0)]
	public List<VillagerRoleRecordElement> History = new List<VillagerRoleRecordElement>();

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		if (History != null)
		{
			num += 2;
			for (int i = 0; i < History.Count; i++)
			{
				num = ((History[i] == null) ? (num + 2) : (num + (2 + History[i].GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 1;
		ptr += 2;
		if (History != null)
		{
			int count = History.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				if (History[i] != null)
				{
					byte* ptr2 = ptr;
					ptr += 2;
					int num = History[i].Serialize(ptr);
					ptr += num;
					Tester.Assert(num <= 65535);
					*(ushort*)ptr2 = (ushort)num;
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
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
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
				if (History == null)
				{
					History = new List<VillagerRoleRecordElement>();
				}
				else
				{
					History.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					ushort num3 = *(ushort*)ptr;
					ptr += 2;
					VillagerRoleRecordElement villagerRoleRecordElement;
					if (num3 > 0)
					{
						villagerRoleRecordElement = new VillagerRoleRecordElement();
						ptr += villagerRoleRecordElement.Deserialize(ptr);
					}
					else
					{
						villagerRoleRecordElement = null;
					}
					History.Add(villagerRoleRecordElement);
				}
			}
			else
			{
				History?.Clear();
			}
		}
		int num4 = (int)(ptr - pData);
		return (num4 <= 4) ? num4 : ((num4 + 3) / 4 * 4);
	}
}
