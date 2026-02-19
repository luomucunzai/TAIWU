using System;
using System.Linq;
using Config;
using GameData.Serializer;

namespace GameData.Domains.Item;

[Obsolete("Instead by FullPoisonEffects. Now only for archive data fix. Do not delete this code.")]
public struct PoisonEffects : ISerializableGameData, IEquatable<PoisonEffects>
{
	private unsafe fixed short _medicineTemplateIds[1];

	public bool IsIdentified;

	private PoisonsAndLevels _poisons;

	public const int MaxPoisonCount = 3;

	public bool HasPoison => GetTotalPoisonCount() > 0;

	public unsafe void Initialize()
	{
		fixed (short* medicineTemplateIds = _medicineTemplateIds)
		{
			*medicineTemplateIds = -1;
		}
		_poisons.Initialize();
		IsIdentified = false;
	}

	public bool Equals(PoisonEffects other)
	{
		if (IsIdentified != other.IsIdentified)
		{
			return false;
		}
		if (!_poisons.Equals(other._poisons))
		{
			return false;
		}
		return true;
	}

	public unsafe bool IsTwoPoisonsMix()
	{
		if (_medicineTemplateIds[0] >= 389)
		{
			return _medicineTemplateIds[0] <= 403;
		}
		return false;
	}

	public unsafe bool IsThreePoisonsMix()
	{
		if (_medicineTemplateIds[0] >= 404)
		{
			return _medicineTemplateIds[0] <= 423;
		}
		return false;
	}

	public PoisonsAndLevels GetAllPoisonsAndLevels()
	{
		return _poisons;
	}

	public unsafe short[] GetAllMedicineTemplateIds()
	{
		short[] ids = new short[3];
		for (int i = 0; i < ids.Length; i++)
		{
			ids[i] = -1;
		}
		int index = 0;
		for (int j = 0; j < 6; j++)
		{
			short value = _poisons.Values[j];
			sbyte level = _poisons.Levels[j];
			int type = j;
			if (value <= 0)
			{
				continue;
			}
			Medicine.Instance.Iterate(delegate(MedicineItem m)
			{
				if (m.PoisonType == type && m.EffectValue == value && m.EffectThresholdValue == level)
				{
					short[] array = ids;
					int num = index;
					index = num + 1;
					array[num] = m.TemplateId;
					return false;
				}
				return true;
			});
		}
		return ids;
	}

	public unsafe short GetMedicineTemplateId()
	{
		return _medicineTemplateIds[0];
	}

	public short GetMedicineTemplateIdAt(int index)
	{
		if (index < 0 || index >= 3)
		{
			throw new ArgumentOutOfRangeException("index", index, "poison slot index is out of range.");
		}
		return GetAllMedicineTemplateIds()[index];
	}

	public unsafe void Remove(short materialTemplateId)
	{
		if (GetAllMedicineTemplateIds().Contains(materialTemplateId))
		{
			short num = materialTemplateId;
			if (num > -1)
			{
				MedicineItem medicineItem = Medicine.Instance[num];
				ref sbyte reference = ref _poisons.Levels[medicineItem.PoisonType];
				reference -= (sbyte)medicineItem.EffectThresholdValue;
				ref short reference2 = ref _poisons.Values[medicineItem.PoisonType];
				reference2 -= medicineItem.EffectValue;
				CalcFinialMedicineId();
			}
		}
	}

	public unsafe void Add(short materialTemplateId)
	{
		if (GetTotalPoisonCount() > 3)
		{
			throw new Exception($"can not set more than {3} poisons");
		}
		if (materialTemplateId <= -1)
		{
			return;
		}
		short[] allMedicineTemplateIds = GetAllMedicineTemplateIds();
		MedicineItem medicineItem = Medicine.Instance[materialTemplateId];
		int num = -1;
		short[] array = allMedicineTemplateIds;
		foreach (short num2 in array)
		{
			if (num2 >= 0 && Medicine.Instance[num2].PoisonType == medicineItem.PoisonType)
			{
				num = num2;
				break;
			}
		}
		bool flag = false;
		if (num <= -1)
		{
			flag = true;
		}
		else
		{
			MedicineItem medicineItem2 = Medicine.Instance[num];
			if (medicineItem2.PoisonType != medicineItem.PoisonType)
			{
				ref sbyte reference = ref _poisons.Levels[medicineItem2.PoisonType];
				reference -= (sbyte)medicineItem2.EffectThresholdValue;
				ref short reference2 = ref _poisons.Values[medicineItem2.PoisonType];
				reference2 -= medicineItem2.EffectValue;
				flag = true;
			}
			else if (medicineItem2.Grade < medicineItem.Grade)
			{
				flag = true;
			}
		}
		if (flag)
		{
			_poisons.Levels[medicineItem.PoisonType] = (sbyte)medicineItem.EffectThresholdValue;
			_poisons.Values[medicineItem.PoisonType] = medicineItem.EffectValue;
		}
		CalcFinialMedicineId();
	}

	public unsafe void CalcFinialMedicineId()
	{
		switch (GetTotalPoisonCount())
		{
		case 0:
			_medicineTemplateIds[0] = -1;
			break;
		case 1:
		{
			ref short medicineTemplateIds2 = ref _medicineTemplateIds[0];
			medicineTemplateIds2 = GetAllMedicineTemplateIds()[0];
			break;
		}
		default:
		{
			ref short medicineTemplateIds = ref _medicineTemplateIds[0];
			medicineTemplateIds = MixedPoisonType.PoisonsAndLevelToMedicineTemplateId(ref _poisons);
			break;
		}
		}
	}

	public sbyte GetTotalPoisonCount()
	{
		return _poisons.GetTotalPoisonCount();
	}

	public unsafe bool Contains(short templateId)
	{
		MedicineItem medicineItem = Medicine.Instance[templateId];
		for (int i = 0; i < 6; i++)
		{
			if (medicineItem.PoisonType == i && medicineItem.EffectValue == _poisons.Values[i])
			{
				return true;
			}
		}
		return false;
	}

	public int GetMaxGrade()
	{
		return GetAllMedicineTemplateIds().Max((short id) => (id <= -1) ? (-1) : Medicine.Instance[id].Grade);
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		return 21;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		fixed (short* medicineTemplateIds = _medicineTemplateIds)
		{
			*(short*)ptr = *medicineTemplateIds;
			ptr += 2;
		}
		ptr += _poisons.Serialize(ptr);
		*ptr = (IsIdentified ? ((byte)1) : ((byte)0));
		ptr++;
		return (int)(ptr - pData);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		fixed (short* medicineTemplateIds = _medicineTemplateIds)
		{
			*medicineTemplateIds = *(short*)ptr;
			ptr += 2;
		}
		ptr += _poisons.Deserialize(ptr);
		IsIdentified = *ptr != 0;
		ptr++;
		return (int)(ptr - pData);
	}
}
