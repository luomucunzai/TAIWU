using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Profession;

[SerializableGameData(IsExtensible = true, NoCopyConstructors = true, NotForDisplayModule = true)]
public class ProfessionDataList : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort CurrProfessionId = 0;

		public const ushort Items = 1;

		public const ushort TaiwuDemandTeachingCount = 2;

		public const ushort LastTeachTaiwuDate = 3;

		public const ushort Count = 4;

		public static readonly string[] FieldId2FieldName = new string[4] { "CurrProfessionId", "Items", "TaiwuDemandTeachingCount", "LastTeachTaiwuDate" };
	}

	[SerializableGameDataField]
	public int CurrProfessionId;

	[SerializableGameDataField(SubDataMaxCount = int.MaxValue)]
	private List<ProfessionData> _items;

	[SerializableGameDataField]
	[Obsolete]
	private Dictionary<int, int> _taiwuDemandTeachingCount;

	[SerializableGameDataField]
	private int _lastTeachTaiwuDate;

	public ProfessionData CurrProfession
	{
		get
		{
			for (int num = _items.Count - 1; num >= 0; num--)
			{
				ProfessionData professionData = _items[num];
				if (professionData.TemplateId == CurrProfessionId)
				{
					return professionData;
				}
			}
			return null;
		}
	}

	public ProfessionDataList()
	{
		_items = new List<ProfessionData>();
		_lastTeachTaiwuDate = int.MinValue;
	}

	[Obsolete]
	public int GetTeachTaiwuCount(int professionId)
	{
		return _taiwuDemandTeachingCount?.GetValueOrDefault(professionId, 0) ?? 0;
	}

	[Obsolete]
	public void AddTeachTaiwuCount(int professionId, int count)
	{
		if (_taiwuDemandTeachingCount == null)
		{
			_taiwuDemandTeachingCount = new Dictionary<int, int>();
		}
		int valueOrDefault = _taiwuDemandTeachingCount.GetValueOrDefault(professionId, 0);
		_taiwuDemandTeachingCount[professionId] = valueOrDefault + count;
	}

	public int GetTeachTaiwuDate()
	{
		return _lastTeachTaiwuDate;
	}

	public void SetTeachTaiwuDate(int date)
	{
		_lastTeachTaiwuDate = date;
	}

	public ProfessionData GetProfession(int professionId)
	{
		for (int num = _items.Count - 1; num >= 0; num--)
		{
			ProfessionData professionData = _items[num];
			if (professionData.TemplateId == professionId)
			{
				return professionData;
			}
		}
		return null;
	}

	public ProfessionData ChangeCurrProfession(int professionId)
	{
		CurrProfessionId = professionId;
		ProfessionData profession = GetProfession(professionId);
		if (profession != null)
		{
			return profession;
		}
		profession = new ProfessionData(professionId, 1);
		_items.Add(profession);
		return profession;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 10;
		if (_items != null)
		{
			num += 2;
			int count = _items.Count;
			for (int i = 0; i < count; i++)
			{
				ProfessionData professionData = _items[i];
				num = ((professionData == null) ? (num + 4) : (num + (4 + professionData.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		num += DictionaryOfBasicTypePair.GetSerializedSize<int, int>((IReadOnlyDictionary<int, int>)_taiwuDemandTeachingCount);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 4;
		ptr += 2;
		*(int*)ptr = CurrProfessionId;
		ptr += 4;
		if (_items != null)
		{
			int count = _items.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ProfessionData professionData = _items[i];
				if (professionData != null)
				{
					byte* ptr2 = ptr;
					ptr += 4;
					int num = professionData.Serialize(ptr);
					ptr += num;
					Tester.Assert(num <= int.MaxValue);
					*(int*)ptr2 = num;
				}
				else
				{
					*(int*)ptr = 0;
					ptr += 4;
				}
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += DictionaryOfBasicTypePair.Serialize<int, int>(ptr, ref _taiwuDemandTeachingCount);
		*(int*)ptr = _lastTeachTaiwuDate;
		ptr += 4;
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
			CurrProfessionId = *(int*)ptr;
			ptr += 4;
		}
		if (num > 1)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (_items == null)
				{
					_items = new List<ProfessionData>(num2);
				}
				else
				{
					_items.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					int num3 = *(int*)ptr;
					ptr += 4;
					if (num3 > 0)
					{
						ProfessionData professionData = new ProfessionData();
						ptr += professionData.Deserialize(ptr);
						_items.Add(professionData);
					}
					else
					{
						_items.Add(null);
					}
				}
			}
			else
			{
				_items?.Clear();
			}
		}
		if (num > 2)
		{
			ptr += DictionaryOfBasicTypePair.Deserialize<int, int>(ptr, ref _taiwuDemandTeachingCount);
		}
		if (num > 3)
		{
			_lastTeachTaiwuDate = *(int*)ptr;
			ptr += 4;
		}
		int num4 = (int)(ptr - pData);
		return (num4 <= 4) ? num4 : ((num4 + 3) / 4 * 4);
	}
}
