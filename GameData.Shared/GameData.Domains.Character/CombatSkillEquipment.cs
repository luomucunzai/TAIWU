using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using GameData.Domains.Taiwu;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character;

[SerializableGameData(NotForArchive = true)]
public class CombatSkillEquipment : IEnumerable<short>, IEnumerable, ISerializableGameData
{
	private object _sourceObj;

	public ArraySegmentList<short> Neigong;

	public ArraySegmentList<short> Attack;

	public ArraySegmentList<short> Agility;

	public ArraySegmentList<short> Defense;

	public ArraySegmentList<short> Assistance;

	public ref ArraySegmentList<short> this[sbyte type] => type switch
	{
		0 => ref Neigong, 
		1 => ref Attack, 
		2 => ref Agility, 
		3 => ref Defense, 
		4 => ref Assistance, 
		_ => throw new IndexOutOfRangeException($"{type} is out of range [0, {5})"), 
	};

	public void Assign(CombatSkillEquipment other)
	{
		_sourceObj = other._sourceObj;
		Neigong = other.Neigong;
		Attack = other.Attack;
		Agility = other.Agility;
		Defense = other.Defense;
		Assistance = other.Assistance;
	}

	public bool IsCombatSkillEquipped(short templateId)
	{
		return this[Config.CombatSkill.Instance[templateId].EquipType].IndexOf(templateId) >= 0;
	}

	public void GetValidSkills(ICollection<short> result)
	{
		result.Clear();
		for (sbyte b = 0; b < 5; b++)
		{
			ArraySegmentList<short>.Enumerator enumerator = this[b].GetEnumerator();
			while (enumerator.MoveNext())
			{
				short current = enumerator.Current;
				if (current >= 0)
				{
					result.Add(current);
				}
			}
		}
	}

	public void GetValidSkills(sbyte equipType, ICollection<short> result)
	{
		result.Clear();
		ArraySegmentList<short>.Enumerator enumerator = this[equipType].GetEnumerator();
		while (enumerator.MoveNext())
		{
			short current = enumerator.Current;
			if (current >= 0)
			{
				result.Add(current);
			}
		}
	}

	public IEnumerator<short> GetEnumerator()
	{
		for (sbyte type = 0; type < 5; type++)
		{
			ArraySegmentList<short> arraySegmentList = this[type];
			ArraySegmentList<short>.Enumerator enumerator = arraySegmentList.GetEnumerator();
			while (enumerator.MoveNext())
			{
				yield return enumerator.Current;
			}
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		return 10 + 2 * (Neigong.Count + Attack.Count + Agility.Count + Defense.Count + Assistance.Count);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += SerializeArraySegment(ptr, Neigong);
		ptr += SerializeArraySegment(ptr, Attack);
		ptr += SerializeArraySegment(ptr, Agility);
		ptr += SerializeArraySegment(ptr, Defense);
		ptr += SerializeArraySegment(ptr, Assistance);
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
		ptr += DeserializeArraySegment(ptr, ref Neigong);
		ptr += DeserializeArraySegment(ptr, ref Attack);
		ptr += DeserializeArraySegment(ptr, ref Agility);
		ptr += DeserializeArraySegment(ptr, ref Defense);
		ptr += DeserializeArraySegment(ptr, ref Assistance);
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	private unsafe int SerializeArraySegment(byte* pData, ArraySegmentList<short> arr)
	{
		byte* ptr = pData;
		*(ushort*)ptr = (ushort)arr.Count;
		ptr += 2;
		for (int i = 0; i < arr.Count; i++)
		{
			*(short*)ptr = arr[i];
			ptr += 2;
		}
		return (int)(ptr - pData);
	}

	private unsafe int DeserializeArraySegment(byte* pData, ref ArraySegmentList<short> arr)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (arr.Count != num)
		{
			arr = new short[num];
		}
		arr.Clear();
		for (int i = 0; i < num; i++)
		{
			arr.Add(*(short*)ptr);
			ptr += 2;
		}
		return (int)(ptr - pData);
	}

	public T GetSourceObject<T>() where T : class
	{
		return _sourceObj as T;
	}

	public void OfflineSetSlot(int slot, short skillTemplateId)
	{
		sbyte equipType = Config.CombatSkill.Instance[skillTemplateId].EquipType;
		OfflineEnsureCapacity(equipType, slot + 1);
		this[equipType][slot] = skillTemplateId;
	}

	public void OfflineAddSkill(short skillTemplateId)
	{
		sbyte equipType = Config.CombatSkill.Instance[skillTemplateId].EquipType;
		OfflineEnsureCapacity(equipType);
		this[equipType].Add(skillTemplateId);
	}

	public bool OfflineRemoveSkill(short skillTemplateId)
	{
		sbyte equipType = Config.CombatSkill.Instance[skillTemplateId].EquipType;
		ArraySegmentList<short> array = this[equipType];
		int num = array.IndexOf(skillTemplateId);
		if (num < 0)
		{
			return false;
		}
		array.RemoveAt(num);
		return true;
	}

	public void OfflineClear()
	{
		Neigong.Clear();
		Attack.Clear();
		Agility.Clear();
		Defense.Clear();
		Assistance.Clear();
	}

	public short OfflineRemoveLastSkill(sbyte equipType)
	{
		ArraySegmentList<short> arraySegmentList = this[equipType];
		int count = arraySegmentList.Count;
		if (count < 0)
		{
			return -1;
		}
		int index = count - 1;
		short result = arraySegmentList[index];
		arraySegmentList.RemoveAt(index);
		return result;
	}

	public bool OfflineEnsureCapacity(sbyte equipType, int capacity = -1)
	{
		ArraySegmentList<short> arraySegmentList = this[equipType];
		if (capacity < 0)
		{
			capacity = arraySegmentList.Count + 1;
		}
		if (arraySegmentList.Capacity >= capacity)
		{
			return false;
		}
		object sourceObj = _sourceObj;
		if (!(sourceObj is short[] array))
		{
			if (sourceObj is CombatSkillPlan combatSkillPlan)
			{
				combatSkillPlan.EnsureSkillListCapacity(equipType, capacity);
				Set(combatSkillPlan);
				return true;
			}
			throw new InvalidOperationException("No source object for CombatSkillEquipment.");
		}
		CombatSkillPlan combatSkillPlan2 = new CombatSkillPlan();
		combatSkillPlan2.Record(array);
		CombatSkillHelper.InitializeEquippedSkills(array);
		combatSkillPlan2.EnsureSkillListCapacity(equipType, capacity);
		Set(combatSkillPlan2);
		return true;
	}

	public void Set(short[] equippedCombatSkills)
	{
		_sourceObj = equippedCombatSkills;
		Neigong = ConvertPartialSegmentList(equippedCombatSkills, 0);
		Attack = ConvertPartialSegmentList(equippedCombatSkills, 1);
		Agility = ConvertPartialSegmentList(equippedCombatSkills, 2);
		Defense = ConvertPartialSegmentList(equippedCombatSkills, 3);
		Assistance = ConvertPartialSegmentList(equippedCombatSkills, 4);
	}

	public void Set(CombatSkillPlan combatSkillPlan)
	{
		_sourceObj = combatSkillPlan;
		Neigong = ConvertFullSegmentList(combatSkillPlan.NeigongList);
		Attack = ConvertFullSegmentList(combatSkillPlan.AttackSkillList);
		Agility = ConvertFullSegmentList(combatSkillPlan.AgilitySkillList);
		Defense = ConvertFullSegmentList(combatSkillPlan.DefenseSkillList);
		Assistance = ConvertFullSegmentList(combatSkillPlan.AssistanceSkillList);
	}

	public void CopyFrom(CombatSkillEquipment other)
	{
		for (sbyte b = 0; b < 5; b++)
		{
			ArraySegmentList<short> arraySegmentList = other[b];
			OfflineEnsureCapacity(b, arraySegmentList.Count);
			ref ArraySegmentList<short> reference = ref this[b];
			reference.Clear();
			for (int i = 0; i < arraySegmentList.Count; i++)
			{
				short num = arraySegmentList[i];
				if (num >= 0)
				{
					reference.Add(num);
				}
			}
		}
	}

	public bool EqualsTo(CombatSkillEquipment other)
	{
		for (sbyte b = 0; b < 5; b++)
		{
			ArraySegmentList<short> segB = this[b];
			ArraySegmentList<short> segA = other[b];
			if (!CheckArraySegmentEquals(ref segA, ref segB))
			{
				return false;
			}
		}
		return true;
	}

	private bool CheckArraySegmentEquals(ref ArraySegmentList<short> segA, ref ArraySegmentList<short> segB)
	{
		if (segA.Count != segB.Count)
		{
			return false;
		}
		int count = segA.Count;
		for (int i = 0; i < count; i++)
		{
			if (segA[i] != segB[i])
			{
				return false;
			}
		}
		return true;
	}

	private ArraySegmentList<short> ConvertFullSegmentList(short[] array)
	{
		return new ArraySegmentList<short>(array, 0, array.Length, GetNextIndex(array), (short)(-1));
	}

	private static ArraySegmentList<short> ConvertPartialSegmentList(short[] equippedSkills, sbyte equipType)
	{
		sbyte offset = CombatSkillHelper.SlotBeginIndexes[equipType];
		sbyte b = CombatSkillHelper.MaxSlotCounts[equipType];
		int nextIndex = GetNextIndex(equippedSkills, offset, b);
		return new ArraySegmentList<short>(equippedSkills, offset, b, nextIndex, (short)(-1));
	}

	private static int GetNextIndex(short[] array)
	{
		int num = array.Length;
		for (int i = 0; i < num; i++)
		{
			if (array[i] < 0)
			{
				return i;
			}
		}
		return num;
	}

	private static int GetNextIndex(short[] array, int offset, int maxLength)
	{
		for (int i = 0; i < maxLength; i++)
		{
			if (array[offset + i] < 0)
			{
				return i;
			}
		}
		return maxLength;
	}
}
