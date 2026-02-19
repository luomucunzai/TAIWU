using System;
using System.Collections.Generic;
using Config;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Item;

[SerializableGameData(IsExtensible = true)]
public class PoisonSlot : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort MedicineTemplateId = 0;

		public const ushort CondensedMedicineTemplateIdList = 1;

		public const ushort Count = 2;

		public static readonly string[] FieldId2FieldName = new string[2] { "MedicineTemplateId", "CondensedMedicineTemplateIdList" };
	}

	public static readonly int MaxMedicineCount = 3;

	[SerializableGameDataField]
	public short MedicineTemplateId = -1;

	[SerializableGameDataField]
	public List<short> CondensedMedicineTemplateIdList;

	public static int MaxCondensedMedicineCount => MaxMedicineCount - 1;

	public bool IsCondensed
	{
		get
		{
			if (IsValid)
			{
				List<short> condensedMedicineTemplateIdList = CondensedMedicineTemplateIdList;
				if (condensedMedicineTemplateIdList == null)
				{
					return false;
				}
				return condensedMedicineTemplateIdList.Count > 0;
			}
			return false;
		}
	}

	public int CurrentMedicineCount => (IsValid ? 1 : 0) + (CondensedMedicineTemplateIdList?.Count ?? 0);

	public bool MedicineCountIsMax => CurrentMedicineCount == MaxMedicineCount;

	public bool IsValid => MedicineTemplateId >= 0;

	public MedicineItem MedicineConfig
	{
		get
		{
			if (!IsValid)
			{
				return null;
			}
			return Medicine.Instance[MedicineTemplateId];
		}
	}

	public bool IsAddPoison
	{
		get
		{
			MedicineItem medicineConfig = MedicineConfig;
			if (medicineConfig == null)
			{
				return false;
			}
			return medicineConfig.EffectType == EMedicineEffectType.ApplyPoison;
		}
	}

	public unsafe PoisonsAndLevels GetPoisonsAndLevels()
	{
		PoisonsAndLevels result = default(PoisonsAndLevels);
		result.Initialize();
		if (IsValid && IsAddPoison)
		{
			result.Values[MedicineConfig.PoisonType] = GetPoisonValue();
			result.Levels[MedicineConfig.PoisonType] = (sbyte)MedicineConfig.EffectThresholdValue;
		}
		return result;
	}

	public short GetPoisonValue()
	{
		if (IsValid && IsAddPoison)
		{
			short num = MedicineConfig.EffectValue;
			if (IsCondensed && MedicineCountIsMax)
			{
				foreach (short condensedMedicineTemplateId in CondensedMedicineTemplateIdList)
				{
					num += Medicine.Instance[condensedMedicineTemplateId].EffectValue;
				}
				int condensedPoisonValueBonus = GlobalConfig.Instance.CondensedPoisonValueBonus;
				num = Convert.ToInt16(num * (100 + condensedPoisonValueBonus) / CurrentMedicineCount / 100);
			}
			return num;
		}
		return 0;
	}

	public void SetPoison(short materialTemplateId, IReadOnlyList<short> condensedMedicineTemplateIdList)
	{
		MedicineTemplateId = materialTemplateId;
		CondensedMedicineTemplateIdList?.Clear();
		if (condensedMedicineTemplateIdList == null || condensedMedicineTemplateIdList.Count <= 0)
		{
			return;
		}
		if (condensedMedicineTemplateIdList.Count != MaxCondensedMedicineCount)
		{
			throw new Exception("Condensed medicine count is wrong.");
		}
		foreach (short condensedMedicineTemplateId in condensedMedicineTemplateIdList)
		{
			MedicineItem medicineItem = Medicine.Instance[condensedMedicineTemplateId];
			if (MedicineConfig.PoisonType != medicineItem.PoisonType)
			{
				throw new Exception("Condensed medicine poison type is not same.");
			}
			if (MedicineConfig.EffectThresholdValue != medicineItem.EffectThresholdValue)
			{
				throw new Exception("Condensed medicine poison level is not same.");
			}
		}
		if (CondensedMedicineTemplateIdList == null)
		{
			CondensedMedicineTemplateIdList = new List<short>(2);
		}
		CondensedMedicineTemplateIdList.AddRange(condensedMedicineTemplateIdList);
	}

	public bool IsSameType(short templateId)
	{
		if (IsValid && templateId >= 0)
		{
			MedicineItem medicineItem = Medicine.Instance[MedicineTemplateId];
			MedicineItem medicineItem2 = Medicine.Instance[templateId];
			return medicineItem.PoisonType == medicineItem2.PoisonType;
		}
		return false;
	}

	public sbyte GetPoisonType()
	{
		if (MedicineTemplateId < 0)
		{
			return -1;
		}
		return MedicineConfig.PoisonType;
	}

	public List<short> GetAllMedicineTemplateId(bool includeCondensed = false)
	{
		if (!IsValid)
		{
			return null;
		}
		List<short> list = new List<short>();
		list.Add(MedicineTemplateId);
		if (IsCondensed && includeCondensed)
		{
			list.AddRange(CondensedMedicineTemplateIdList);
		}
		return list;
	}

	public void Clear()
	{
		MedicineTemplateId = -1;
		CondensedMedicineTemplateIdList?.Clear();
	}

	public bool SameOf(PoisonSlot other)
	{
		if (this == other)
		{
			return true;
		}
		if (other == null)
		{
			if (!IsValid)
			{
				return !IsCondensed;
			}
			return false;
		}
		if (MedicineTemplateId != other.MedicineTemplateId)
		{
			return false;
		}
		int num = CondensedMedicineTemplateIdList?.Count ?? 0;
		int num2 = other.CondensedMedicineTemplateIdList?.Count ?? 0;
		if (num != num2)
		{
			return false;
		}
		if (num == 0)
		{
			return true;
		}
		for (int i = 0; i < num; i++)
		{
			if (CondensedMedicineTemplateIdList[i] != other.CondensedMedicineTemplateIdList[i])
			{
				return false;
			}
		}
		return true;
	}

	public PoisonSlot()
	{
	}

	public PoisonSlot(PoisonSlot other)
	{
		MedicineTemplateId = other.MedicineTemplateId;
		CondensedMedicineTemplateIdList = ((other.CondensedMedicineTemplateIdList == null) ? null : new List<short>(other.CondensedMedicineTemplateIdList));
	}

	public void Assign(PoisonSlot other)
	{
		MedicineTemplateId = other.MedicineTemplateId;
		CondensedMedicineTemplateIdList = ((other.CondensedMedicineTemplateIdList == null) ? null : new List<short>(other.CondensedMedicineTemplateIdList));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		num = ((CondensedMedicineTemplateIdList == null) ? (num + 2) : (num + (2 + 2 * CondensedMedicineTemplateIdList.Count)));
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
		*(short*)ptr = MedicineTemplateId;
		ptr += 2;
		if (CondensedMedicineTemplateIdList != null)
		{
			int count = CondensedMedicineTemplateIdList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = CondensedMedicineTemplateIdList[i];
			}
			ptr += 2 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
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
			MedicineTemplateId = *(short*)ptr;
			ptr += 2;
		}
		if (num > 1)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (CondensedMedicineTemplateIdList == null)
				{
					CondensedMedicineTemplateIdList = new List<short>(num2);
				}
				else
				{
					CondensedMedicineTemplateIdList.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					CondensedMedicineTemplateIdList.Add(((short*)ptr)[i]);
				}
				ptr += 2 * num2;
			}
			else
			{
				CondensedMedicineTemplateIdList?.Clear();
			}
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
