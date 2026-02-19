using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Extra;

[SerializableGameData(IsExtensible = true)]
public class MerchantExtraGoodsData : ISerializableGameData
{
	public enum ExtraGoodsType
	{
		None,
		Normal,
		Capitalist,
		Season
	}

	private static class FieldIds
	{
		public const ushort NormalExtraGoods = 0;

		public const ushort CapitalistSkillExtraGoods = 1;

		public const ushort SeasonExtraGoods = 2;

		public const ushort SeasonTemplateId = 3;

		public const ushort Count = 4;

		public static readonly string[] FieldId2FieldName = new string[4] { "NormalExtraGoods", "CapitalistSkillExtraGoods", "SeasonExtraGoods", "SeasonTemplateId" };
	}

	[SerializableGameDataField]
	public List<MerchantExtraGoodsItem> NormalExtraGoods = new List<MerchantExtraGoodsItem>();

	[SerializableGameDataField]
	public List<MerchantExtraGoodsItem> CapitalistSkillExtraGoods = new List<MerchantExtraGoodsItem>();

	[SerializableGameDataField]
	public List<MerchantExtraGoodsItem> SeasonExtraGoods = new List<MerchantExtraGoodsItem>();

	[SerializableGameDataField]
	public sbyte SeasonTemplateId = -1;

	private bool Check(List<MerchantExtraGoodsItem> list, int id, out int findIndex)
	{
		findIndex = list.FindIndex((MerchantExtraGoodsItem d) => d.Id == id);
		return findIndex >= 0;
	}

	public bool Check(int id, out ExtraGoodsType type)
	{
		if (Check(NormalExtraGoods, id, out var findIndex))
		{
			type = ExtraGoodsType.Normal;
			return true;
		}
		if (Check(CapitalistSkillExtraGoods, id, out findIndex))
		{
			type = ExtraGoodsType.Capitalist;
			return true;
		}
		if (Check(SeasonExtraGoods, id, out findIndex))
		{
			type = ExtraGoodsType.Season;
			return true;
		}
		type = ExtraGoodsType.None;
		return false;
	}

	public void Remove(int id)
	{
		if (Check(NormalExtraGoods, id, out var findIndex))
		{
			NormalExtraGoods.RemoveAt(findIndex);
		}
		if (Check(CapitalistSkillExtraGoods, id, out findIndex))
		{
			CapitalistSkillExtraGoods.RemoveAt(findIndex);
		}
		if (Check(SeasonExtraGoods, id, out findIndex))
		{
			SeasonExtraGoods.RemoveAt(findIndex);
		}
	}

	public void Clear()
	{
		NormalExtraGoods.Clear();
		CapitalistSkillExtraGoods.Clear();
		SeasonExtraGoods.Clear();
	}

	public MerchantExtraGoodsData()
	{
	}

	public MerchantExtraGoodsData(MerchantExtraGoodsData other)
	{
		NormalExtraGoods = ((other.NormalExtraGoods == null) ? null : new List<MerchantExtraGoodsItem>(other.NormalExtraGoods));
		CapitalistSkillExtraGoods = ((other.CapitalistSkillExtraGoods == null) ? null : new List<MerchantExtraGoodsItem>(other.CapitalistSkillExtraGoods));
		SeasonExtraGoods = ((other.SeasonExtraGoods == null) ? null : new List<MerchantExtraGoodsItem>(other.SeasonExtraGoods));
		SeasonTemplateId = other.SeasonTemplateId;
	}

	public void Assign(MerchantExtraGoodsData other)
	{
		NormalExtraGoods = ((other.NormalExtraGoods == null) ? null : new List<MerchantExtraGoodsItem>(other.NormalExtraGoods));
		CapitalistSkillExtraGoods = ((other.CapitalistSkillExtraGoods == null) ? null : new List<MerchantExtraGoodsItem>(other.CapitalistSkillExtraGoods));
		SeasonExtraGoods = ((other.SeasonExtraGoods == null) ? null : new List<MerchantExtraGoodsItem>(other.SeasonExtraGoods));
		SeasonTemplateId = other.SeasonTemplateId;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 3;
		num = ((NormalExtraGoods == null) ? (num + 2) : (num + (2 + 8 * NormalExtraGoods.Count)));
		num = ((CapitalistSkillExtraGoods == null) ? (num + 2) : (num + (2 + 8 * CapitalistSkillExtraGoods.Count)));
		num = ((SeasonExtraGoods == null) ? (num + 2) : (num + (2 + 8 * SeasonExtraGoods.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 4;
		ptr += 2;
		if (NormalExtraGoods != null)
		{
			int count = NormalExtraGoods.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += NormalExtraGoods[i].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (CapitalistSkillExtraGoods != null)
		{
			int count2 = CapitalistSkillExtraGoods.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				ptr += CapitalistSkillExtraGoods[j].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (SeasonExtraGoods != null)
		{
			int count3 = SeasonExtraGoods.Count;
			Tester.Assert(count3 <= 65535);
			*(ushort*)ptr = (ushort)count3;
			ptr += 2;
			for (int k = 0; k < count3; k++)
			{
				ptr += SeasonExtraGoods[k].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (byte)SeasonTemplateId;
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
				if (NormalExtraGoods == null)
				{
					NormalExtraGoods = new List<MerchantExtraGoodsItem>(num2);
				}
				else
				{
					NormalExtraGoods.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					MerchantExtraGoodsItem item = default(MerchantExtraGoodsItem);
					ptr += item.Deserialize(ptr);
					NormalExtraGoods.Add(item);
				}
			}
			else
			{
				NormalExtraGoods?.Clear();
			}
		}
		if (num > 1)
		{
			ushort num3 = *(ushort*)ptr;
			ptr += 2;
			if (num3 > 0)
			{
				if (CapitalistSkillExtraGoods == null)
				{
					CapitalistSkillExtraGoods = new List<MerchantExtraGoodsItem>(num3);
				}
				else
				{
					CapitalistSkillExtraGoods.Clear();
				}
				for (int j = 0; j < num3; j++)
				{
					MerchantExtraGoodsItem item2 = default(MerchantExtraGoodsItem);
					ptr += item2.Deserialize(ptr);
					CapitalistSkillExtraGoods.Add(item2);
				}
			}
			else
			{
				CapitalistSkillExtraGoods?.Clear();
			}
		}
		if (num > 2)
		{
			ushort num4 = *(ushort*)ptr;
			ptr += 2;
			if (num4 > 0)
			{
				if (SeasonExtraGoods == null)
				{
					SeasonExtraGoods = new List<MerchantExtraGoodsItem>(num4);
				}
				else
				{
					SeasonExtraGoods.Clear();
				}
				for (int k = 0; k < num4; k++)
				{
					MerchantExtraGoodsItem item3 = default(MerchantExtraGoodsItem);
					ptr += item3.Deserialize(ptr);
					SeasonExtraGoods.Add(item3);
				}
			}
			else
			{
				SeasonExtraGoods?.Clear();
			}
		}
		if (num > 3)
		{
			SeasonTemplateId = (sbyte)(*ptr);
			ptr++;
		}
		int num5 = (int)(ptr - pData);
		if (num5 > 4)
		{
			return (num5 + 3) / 4 * 4;
		}
		return num5;
	}
}
