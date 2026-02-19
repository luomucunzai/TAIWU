using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Building;

[AutoGenerateSerializableGameData(IsExtensible = true, NoCopyConstructors = true)]
public class BuildingOptionAutoAddSoldItemPreset : ISerializableGameData
{
	public enum EGradeOrder : sbyte
	{
		Invalid,
		High,
		Low
	}

	[Flags]
	public enum EPropertyOrder : sbyte
	{
		Invalid = 0,
		MaxValue = 1,
		MaxAmount = 2
	}

	public static class FieldIds
	{
		public const ushort ItemTypeList = 0;

		public const ushort MinGrade = 1;

		public const ushort MaxGrade = 2;

		public const ushort GradeOrder = 3;

		public const ushort PropertyOrder = 4;

		public const ushort Count = 5;

		public static readonly string[] FieldId2FieldName = new string[5] { "ItemTypeList", "MinGrade", "MaxGrade", "GradeOrder", "PropertyOrder" };
	}

	[SerializableGameDataField(FieldIndex = 0)]
	public List<sbyte> ItemTypeList;

	[SerializableGameDataField(FieldIndex = 1)]
	public sbyte MinGrade;

	[SerializableGameDataField(FieldIndex = 2)]
	public sbyte MaxGrade = 8;

	[SerializableGameDataField(FieldIndex = 3)]
	public sbyte GradeOrder = 1;

	[SerializableGameDataField(FieldIndex = 4)]
	public sbyte PropertyOrder = 3;

	public EPropertyOrder PropertyOrderEnum => (EPropertyOrder)PropertyOrder;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 6;
		num = ((ItemTypeList == null) ? (num + 2) : (num + (2 + ItemTypeList.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 5;
		ptr += 2;
		if (ItemTypeList != null)
		{
			int count = ItemTypeList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				*ptr = (byte)ItemTypeList[i];
				ptr++;
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (byte)MinGrade;
		ptr++;
		*ptr = (byte)MaxGrade;
		ptr++;
		*ptr = (byte)GradeOrder;
		ptr++;
		*ptr = (byte)PropertyOrder;
		ptr++;
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
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (ItemTypeList == null)
				{
					ItemTypeList = new List<sbyte>();
				}
				else
				{
					ItemTypeList.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					sbyte item = (sbyte)(*ptr);
					ptr++;
					ItemTypeList.Add(item);
				}
			}
			else
			{
				ItemTypeList?.Clear();
			}
		}
		if (num > 1)
		{
			MinGrade = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 2)
		{
			MaxGrade = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 3)
		{
			GradeOrder = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 4)
		{
			PropertyOrder = (sbyte)(*ptr);
			ptr++;
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
