using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Extra;

[SerializableGameData(IsExtensible = true)]
public class SectStoryThreeCorpsesCharacter : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort TemplateId = 0;

		public const ushort IsGoodEnd = 1;

		public const ushort Target = 2;

		public const ushort EndDate = 3;

		public const ushort NextDate = 4;

		public const ushort Notch = 5;

		public const ushort LegendaryBooks = 6;

		public const ushort IsUpgraded = 7;

		public const ushort Id = 8;

		public const ushort TargetOwner = 9;

		public const ushort Progress = 10;

		public const ushort IsAroundTaiwu = 11;

		public const ushort TaiwuId = 12;

		public const ushort PassLegacyEventTriggered = 13;

		public const ushort Count = 14;

		public static readonly string[] FieldId2FieldName = new string[14]
		{
			"TemplateId", "IsGoodEnd", "Target", "EndDate", "NextDate", "Notch", "LegendaryBooks", "IsUpgraded", "Id", "TargetOwner",
			"Progress", "IsAroundTaiwu", "TaiwuId", "PassLegacyEventTriggered"
		};
	}

	[SerializableGameDataField]
	public int Id;

	[SerializableGameDataField]
	public short TemplateId;

	[SerializableGameDataField]
	public bool IsGoodEnd;

	[SerializableGameDataField]
	public sbyte Progress;

	[SerializableGameDataField]
	public sbyte Target;

	[SerializableGameDataField]
	public int TargetOwner;

	[SerializableGameDataField]
	public int EndDate;

	[SerializableGameDataField]
	public int NextDate;

	[SerializableGameDataField]
	public sbyte Notch;

	[SerializableGameDataField]
	public List<sbyte> LegendaryBooks;

	[SerializableGameDataField]
	public bool IsUpgraded;

	[SerializableGameDataField]
	public bool IsAroundTaiwu;

	[SerializableGameDataField]
	public int TaiwuId;

	[SerializableGameDataField]
	public bool PassLegacyEventTriggered;

	public SectStoryThreeCorpsesCharacter(int id, short templateId, int taiwuId)
	{
		Id = id;
		TemplateId = templateId;
		IsGoodEnd = false;
		Progress = 0;
		Target = -1;
		TargetOwner = -1;
		EndDate = -1;
		NextDate = -1;
		Notch = 0;
		LegendaryBooks = null;
		IsUpgraded = false;
		IsAroundTaiwu = true;
		TaiwuId = taiwuId;
		PassLegacyEventTriggered = false;
	}

	public SectStoryThreeCorpsesCharacter()
	{
	}

	public SectStoryThreeCorpsesCharacter(SectStoryThreeCorpsesCharacter other)
	{
		TemplateId = other.TemplateId;
		IsGoodEnd = other.IsGoodEnd;
		Target = other.Target;
		EndDate = other.EndDate;
		NextDate = other.NextDate;
		Notch = other.Notch;
		LegendaryBooks = ((other.LegendaryBooks == null) ? null : new List<sbyte>(other.LegendaryBooks));
		IsUpgraded = other.IsUpgraded;
		Id = other.Id;
		TargetOwner = other.TargetOwner;
		Progress = other.Progress;
		IsAroundTaiwu = other.IsAroundTaiwu;
		TaiwuId = other.TaiwuId;
		PassLegacyEventTriggered = other.PassLegacyEventTriggered;
	}

	public void Assign(SectStoryThreeCorpsesCharacter other)
	{
		TemplateId = other.TemplateId;
		IsGoodEnd = other.IsGoodEnd;
		Target = other.Target;
		EndDate = other.EndDate;
		NextDate = other.NextDate;
		Notch = other.Notch;
		LegendaryBooks = ((other.LegendaryBooks == null) ? null : new List<sbyte>(other.LegendaryBooks));
		IsUpgraded = other.IsUpgraded;
		Id = other.Id;
		TargetOwner = other.TargetOwner;
		Progress = other.Progress;
		IsAroundTaiwu = other.IsAroundTaiwu;
		TaiwuId = other.TaiwuId;
		PassLegacyEventTriggered = other.PassLegacyEventTriggered;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 31;
		num = ((LegendaryBooks == null) ? (num + 2) : (num + (2 + LegendaryBooks.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 14;
		ptr += 2;
		*(short*)ptr = TemplateId;
		ptr += 2;
		*ptr = (IsGoodEnd ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (byte)Target;
		ptr++;
		*(int*)ptr = EndDate;
		ptr += 4;
		*(int*)ptr = NextDate;
		ptr += 4;
		*ptr = (byte)Notch;
		ptr++;
		if (LegendaryBooks != null)
		{
			int count = LegendaryBooks.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr[i] = (byte)LegendaryBooks[i];
			}
			ptr += count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (IsUpgraded ? ((byte)1) : ((byte)0));
		ptr++;
		*(int*)ptr = Id;
		ptr += 4;
		*(int*)ptr = TargetOwner;
		ptr += 4;
		*ptr = (byte)Progress;
		ptr++;
		*ptr = (IsAroundTaiwu ? ((byte)1) : ((byte)0));
		ptr++;
		*(int*)ptr = TaiwuId;
		ptr += 4;
		*ptr = (PassLegacyEventTriggered ? ((byte)1) : ((byte)0));
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
			TemplateId = *(short*)ptr;
			ptr += 2;
		}
		if (num > 1)
		{
			IsGoodEnd = *ptr != 0;
			ptr++;
		}
		if (num > 2)
		{
			Target = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 3)
		{
			EndDate = *(int*)ptr;
			ptr += 4;
		}
		if (num > 4)
		{
			NextDate = *(int*)ptr;
			ptr += 4;
		}
		if (num > 5)
		{
			Notch = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 6)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (LegendaryBooks == null)
				{
					LegendaryBooks = new List<sbyte>(num2);
				}
				else
				{
					LegendaryBooks.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					LegendaryBooks.Add((sbyte)ptr[i]);
				}
				ptr += (int)num2;
			}
			else
			{
				LegendaryBooks?.Clear();
			}
		}
		if (num > 7)
		{
			IsUpgraded = *ptr != 0;
			ptr++;
		}
		if (num > 8)
		{
			Id = *(int*)ptr;
			ptr += 4;
		}
		if (num > 9)
		{
			TargetOwner = *(int*)ptr;
			ptr += 4;
		}
		if (num > 10)
		{
			Progress = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 11)
		{
			IsAroundTaiwu = *ptr != 0;
			ptr++;
		}
		if (num > 12)
		{
			TaiwuId = *(int*)ptr;
			ptr += 4;
		}
		if (num > 13)
		{
			PassLegacyEventTriggered = *ptr != 0;
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
