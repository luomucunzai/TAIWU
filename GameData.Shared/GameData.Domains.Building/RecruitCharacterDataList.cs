using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Building;

[SerializableGameData(IsExtensible = true, NotForDisplayModule = true)]
public class RecruitCharacterDataList : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort Items = 0;

		public const ushort Count = 1;

		public static readonly string[] FieldId2FieldName = new string[1] { "Items" };
	}

	[SerializableGameDataField]
	public List<RecruitCharacterData> Items = new List<RecruitCharacterData>();

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		if (Items != null)
		{
			num += 2;
			int count = Items.Count;
			for (int i = 0; i < count; i++)
			{
				RecruitCharacterData recruitCharacterData = Items[i];
				num = ((recruitCharacterData == null) ? (num + 2) : (num + (2 + recruitCharacterData.GetSerializedSize())));
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
		if (Items != null)
		{
			int count = Items.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				RecruitCharacterData recruitCharacterData = Items[i];
				if (recruitCharacterData != null)
				{
					byte* intPtr = ptr;
					ptr += 2;
					int num = recruitCharacterData.Serialize(ptr);
					ptr += num;
					Tester.Assert(num <= 65535);
					*(ushort*)intPtr = (ushort)num;
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
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
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
				if (Items == null)
				{
					Items = new List<RecruitCharacterData>(num2);
				}
				else
				{
					Items.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					ushort num3 = *(ushort*)ptr;
					ptr += 2;
					if (num3 > 0)
					{
						RecruitCharacterData recruitCharacterData = new RecruitCharacterData();
						ptr += recruitCharacterData.Deserialize(ptr);
						Items.Add(recruitCharacterData);
					}
					else
					{
						Items.Add(null);
					}
				}
			}
			else
			{
				Items?.Clear();
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
