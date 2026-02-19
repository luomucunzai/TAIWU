using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Item;

[SerializableGameData(IsExtensible = true)]
public class FullPoisonEffects : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort PoisonSlotList = 0;

		public const ushort IsIdentified = 1;

		public const ushort Count = 2;

		public static readonly string[] FieldId2FieldName = new string[2] { "PoisonSlotList", "IsIdentified" };
	}

	public static readonly int MaxSlotCount = 3;

	[SerializableGameDataField]
	public List<PoisonSlot> PoisonSlotList;

	[SerializableGameDataField]
	public bool IsIdentified;

	public bool IsCondensed => PoisonSlotList?.Any((PoisonSlot p) => p.IsCondensed) ?? false;

	public bool IsValid => PoisonSlotList?.Any((PoisonSlot p) => p.IsValid) ?? false;

	public int CurrentValidSlotCount => PoisonSlotList?.Count((PoisonSlot p) => p.IsValid) ?? 0;

	public bool IsMixed
	{
		get
		{
			List<PoisonSlot> poisonSlotList = PoisonSlotList;
			if (poisonSlotList == null)
			{
				return false;
			}
			return poisonSlotList.Count((PoisonSlot p) => p.IsValid && p.IsAddPoison) > 1;
		}
	}

	public bool IsThreeMixed => PoisonSlotList?.Count((PoisonSlot p) => p.IsValid && p.IsAddPoison) == MaxSlotCount;

	public void Clear()
	{
		PoisonSlotList?.Clear();
	}

	public void ClearCondense()
	{
		PoisonSlotList?.ForEach(delegate(PoisonSlot s)
		{
			s.CondensedMedicineTemplateIdList?.Clear();
		});
	}

	public void AddPoison(short templateId, IReadOnlyList<short> condensedMedicineTemplateIdList)
	{
		if (templateId < 0)
		{
			throw new Exception("Poison template Id is invalid.");
		}
		int num = PoisonSlotList?.FindIndex((PoisonSlot p) => p.IsSameType(templateId) || !p.IsValid) ?? (-1);
		if (num >= 0)
		{
			PoisonSlotList[num].SetPoison(templateId, condensedMedicineTemplateIdList);
			return;
		}
		if (IsValid && PoisonSlotList?.Count >= MaxSlotCount)
		{
			throw new Exception($"Poison slot cannot be more than {MaxSlotCount} on add poison");
		}
		PoisonSlot poisonSlot = new PoisonSlot();
		poisonSlot.SetPoison(templateId, condensedMedicineTemplateIdList);
		if (PoisonSlotList == null)
		{
			PoisonSlotList = new List<PoisonSlot>();
		}
		PoisonSlotList.Add(poisonSlot);
	}

	public void RemovePoison(short templateId)
	{
		int num = PoisonSlotList?.FindIndex((PoisonSlot p) => p.IsSameType(templateId)) ?? (-1);
		if (num < 0)
		{
			throw new Exception("Not find target poison to remove");
		}
		PoisonSlotList.RemoveAt(num);
	}

	public PoisonsAndLevels GetAllPoisonsAndLevels()
	{
		PoisonsAndLevels result = default(PoisonsAndLevels);
		result.Initialize();
		if (IsValid)
		{
			foreach (PoisonSlot poisonSlot in PoisonSlotList)
			{
				PoisonsAndLevels poisonsAndLevels = poisonSlot.GetPoisonsAndLevels();
				result.Add(poisonsAndLevels);
			}
		}
		return result;
	}

	public bool ContainsPoisonType(sbyte poisonType)
	{
		if (PoisonSlotList == null)
		{
			return false;
		}
		foreach (PoisonSlot poisonSlot in PoisonSlotList)
		{
			if (poisonSlot.GetPoisonType() == poisonType)
			{
				return true;
			}
		}
		return false;
	}

	public bool ContainsPoisonOfSameType(short medicineTemplateId)
	{
		if (PoisonSlotList == null)
		{
			return false;
		}
		foreach (PoisonSlot poisonSlot in PoisonSlotList)
		{
			if (poisonSlot.IsSameType(medicineTemplateId))
			{
				return true;
			}
		}
		return false;
	}

	public short GetMedicineTemplateId()
	{
		if (!IsValid)
		{
			return -1;
		}
		if (IsMixed)
		{
			return GetMixedMedicineTemplateId();
		}
		return PoisonSlotList.First().MedicineTemplateId;
	}

	public short GetMixedMedicineTemplateId()
	{
		if (!IsMixed)
		{
			return -1;
		}
		return GetAllPoisonsAndLevels().GetMixTemplateId();
	}

	public List<short> GetAllMedicineTemplateIds(bool includeCondensed = false)
	{
		if (!IsValid)
		{
			return null;
		}
		List<short> list = new List<short>();
		foreach (PoisonSlot poisonSlot in PoisonSlotList)
		{
			list.Add(poisonSlot.MedicineTemplateId);
			if (includeCondensed && poisonSlot.IsCondensed)
			{
				list.AddRange(poisonSlot.CondensedMedicineTemplateIdList);
			}
		}
		return list;
	}

	public short GetMedicineTemplateIdAt(int index)
	{
		List<short> allMedicineTemplateIds = GetAllMedicineTemplateIds();
		if (allMedicineTemplateIds.CheckIndex(index))
		{
			return allMedicineTemplateIds[index];
		}
		return -1;
	}

	public bool IsTwoPoisonsMix()
	{
		short medicineTemplateId = GetMedicineTemplateId();
		if (medicineTemplateId >= 389)
		{
			return medicineTemplateId <= 403;
		}
		return false;
	}

	public bool IsThreePoisonsMix()
	{
		short medicineTemplateId = GetMedicineTemplateId();
		if (medicineTemplateId >= 404)
		{
			return medicineTemplateId <= 423;
		}
		return false;
	}

	public int GetTotalPoisonCount()
	{
		if (!IsValid)
		{
			return 0;
		}
		return PoisonSlotList.Count((PoisonSlot s) => s.IsValid);
	}

	public int GetMaxGrade()
	{
		return GetAllMedicineTemplateIds()?.Max((short id) => (id <= -1) ? (-1) : Medicine.Instance[id].Grade) ?? 0;
	}

	public bool SameOf(FullPoisonEffects other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		if (IsIdentified != other.IsIdentified)
		{
			return false;
		}
		if (PoisonSlotList == null && other.PoisonSlotList == null)
		{
			return true;
		}
		if (PoisonSlotList == null || other.PoisonSlotList == null)
		{
			return false;
		}
		if (PoisonSlotList.Count != other.PoisonSlotList.Count)
		{
			return false;
		}
		for (int i = 0; i < PoisonSlotList.Count; i++)
		{
			if (!PoisonSlotList[i].SameOf(other.PoisonSlotList[i]))
			{
				return false;
			}
		}
		return true;
	}

	public FullPoisonEffects()
	{
	}

	public FullPoisonEffects(FullPoisonEffects other)
	{
		if (other.PoisonSlotList != null)
		{
			List<PoisonSlot> poisonSlotList = other.PoisonSlotList;
			int count = poisonSlotList.Count;
			PoisonSlotList = new List<PoisonSlot>(count);
			for (int i = 0; i < count; i++)
			{
				PoisonSlotList.Add(new PoisonSlot(poisonSlotList[i]));
			}
		}
		else
		{
			PoisonSlotList = null;
		}
		IsIdentified = other.IsIdentified;
	}

	public void Assign(FullPoisonEffects other)
	{
		if (other.PoisonSlotList != null)
		{
			List<PoisonSlot> poisonSlotList = other.PoisonSlotList;
			int count = poisonSlotList.Count;
			PoisonSlotList = new List<PoisonSlot>(count);
			for (int i = 0; i < count; i++)
			{
				PoisonSlotList.Add(new PoisonSlot(poisonSlotList[i]));
			}
		}
		else
		{
			PoisonSlotList = null;
		}
		IsIdentified = other.IsIdentified;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 3;
		if (PoisonSlotList != null)
		{
			num += 2;
			int count = PoisonSlotList.Count;
			for (int i = 0; i < count; i++)
			{
				PoisonSlot poisonSlot = PoisonSlotList[i];
				num = ((poisonSlot == null) ? (num + 2) : (num + (2 + poisonSlot.GetSerializedSize())));
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
		*(short*)ptr = 2;
		ptr += 2;
		if (PoisonSlotList != null)
		{
			int count = PoisonSlotList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				PoisonSlot poisonSlot = PoisonSlotList[i];
				if (poisonSlot != null)
				{
					byte* intPtr = ptr;
					ptr += 2;
					int num = poisonSlot.Serialize(ptr);
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
		*ptr = (IsIdentified ? ((byte)1) : ((byte)0));
		ptr++;
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
				if (PoisonSlotList == null)
				{
					PoisonSlotList = new List<PoisonSlot>(num2);
				}
				else
				{
					PoisonSlotList.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					ushort num3 = *(ushort*)ptr;
					ptr += 2;
					if (num3 > 0)
					{
						PoisonSlot poisonSlot = new PoisonSlot();
						ptr += poisonSlot.Deserialize(ptr);
						PoisonSlotList.Add(poisonSlot);
					}
					else
					{
						PoisonSlotList.Add(null);
					}
				}
			}
			else
			{
				PoisonSlotList?.Clear();
			}
		}
		if (num > 1)
		{
			IsIdentified = *ptr != 0;
			ptr++;
		}
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}
}
