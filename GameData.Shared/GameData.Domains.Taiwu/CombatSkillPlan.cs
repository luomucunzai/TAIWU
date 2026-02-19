using System;
using System.Linq;
using GameData.Domains.Character;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu;

[SerializableGameData(NotForDisplayModule = true)]
public class CombatSkillPlan : ISerializableGameData
{
	public const int MaxPlanCount = 9;

	[SerializableGameDataField]
	public short[] NeigongList = new short[9];

	[SerializableGameDataField]
	public short[] AttackSkillList = new short[9];

	[SerializableGameDataField]
	public short[] AgilitySkillList = new short[9];

	[SerializableGameDataField]
	public short[] DefenseSkillList = new short[9];

	[SerializableGameDataField]
	public short[] AssistanceSkillList = new short[9];

	[SerializableGameDataField]
	public byte[] GenericGridAllocation = new byte[4];

	public CombatSkillPlan()
	{
		Reset();
	}

	public void Reset()
	{
		for (int i = 0; i < NeigongList.Length; i++)
		{
			NeigongList[i] = -1;
		}
		for (int j = 0; j < AttackSkillList.Length; j++)
		{
			AttackSkillList[j] = -1;
		}
		for (int k = 0; k < AgilitySkillList.Length; k++)
		{
			AgilitySkillList[k] = -1;
		}
		for (int l = 0; l < DefenseSkillList.Length; l++)
		{
			DefenseSkillList[l] = -1;
		}
		for (int m = 0; m < AssistanceSkillList.Length; m++)
		{
			AssistanceSkillList[m] = -1;
		}
	}

	public void CopyFrom(CombatSkillPlan plan)
	{
		NeigongList = CopyArray(plan.NeigongList, NeigongList);
		AttackSkillList = CopyArray(plan.AttackSkillList, AttackSkillList);
		AgilitySkillList = CopyArray(plan.AgilitySkillList, AgilitySkillList);
		DefenseSkillList = CopyArray(plan.DefenseSkillList, DefenseSkillList);
		AssistanceSkillList = CopyArray(plan.AssistanceSkillList, AssistanceSkillList);
	}

	private short[] CopyArray(short[] srcArray, short[] dstArray)
	{
		if (srcArray.Length == dstArray.Length)
		{
			Array.Copy(srcArray, dstArray, srcArray.Length);
		}
		else
		{
			dstArray = srcArray.ToArray();
		}
		return dstArray;
	}

	public void Record(short[] skillIdList)
	{
		for (sbyte b = 0; b < 5; b++)
		{
			short[] skillList = GetSkillList(b);
			for (sbyte b2 = 0; b2 < skillList.Length; b2++)
			{
				skillList[b2] = (short)((b2 < CombatSkillHelper.MaxSlotCounts[b]) ? CombatSkillHelper.GetEquippedSkill(skillIdList, b, b2) : (-1));
			}
		}
	}

	public short[] GetSkillList(sbyte type)
	{
		return type switch
		{
			0 => NeigongList, 
			1 => AttackSkillList, 
			2 => AgilitySkillList, 
			3 => DefenseSkillList, 
			4 => AssistanceSkillList, 
			_ => null, 
		};
	}

	public void EnsureSkillListCapacity(sbyte type, int capacity)
	{
		short[] array = new short[capacity + 1];
		Array.Fill(array, (short)(-1));
		switch (type)
		{
		case 0:
			NeigongList.CopyTo(array, 0);
			NeigongList = array;
			break;
		case 1:
			AttackSkillList.CopyTo(array, 0);
			AttackSkillList = array;
			break;
		case 2:
			AgilitySkillList.CopyTo(array, 0);
			AgilitySkillList = array;
			break;
		case 3:
			DefenseSkillList.CopyTo(array, 0);
			DefenseSkillList = array;
			break;
		case 4:
			AssistanceSkillList.CopyTo(array, 0);
			AssistanceSkillList = array;
			break;
		default:
			throw new ArgumentException($"Unrecognized equip type {type}");
		}
	}

	public CombatSkillPlan(CombatSkillPlan other)
	{
		short[] neigongList = other.NeigongList;
		int num = neigongList.Length;
		NeigongList = new short[num];
		for (int i = 0; i < num; i++)
		{
			NeigongList[i] = neigongList[i];
		}
		short[] attackSkillList = other.AttackSkillList;
		int num2 = attackSkillList.Length;
		AttackSkillList = new short[num2];
		for (int j = 0; j < num2; j++)
		{
			AttackSkillList[j] = attackSkillList[j];
		}
		short[] agilitySkillList = other.AgilitySkillList;
		int num3 = agilitySkillList.Length;
		AgilitySkillList = new short[num3];
		for (int k = 0; k < num3; k++)
		{
			AgilitySkillList[k] = agilitySkillList[k];
		}
		short[] defenseSkillList = other.DefenseSkillList;
		int num4 = defenseSkillList.Length;
		DefenseSkillList = new short[num4];
		for (int l = 0; l < num4; l++)
		{
			DefenseSkillList[l] = defenseSkillList[l];
		}
		short[] assistanceSkillList = other.AssistanceSkillList;
		int num5 = assistanceSkillList.Length;
		AssistanceSkillList = new short[num5];
		for (int m = 0; m < num5; m++)
		{
			AssistanceSkillList[m] = assistanceSkillList[m];
		}
		byte[] genericGridAllocation = other.GenericGridAllocation;
		int num6 = genericGridAllocation.Length;
		GenericGridAllocation = new byte[num6];
		for (int n = 0; n < num6; n++)
		{
			GenericGridAllocation[n] = genericGridAllocation[n];
		}
	}

	public void Assign(CombatSkillPlan other)
	{
		short[] neigongList = other.NeigongList;
		int num = neigongList.Length;
		NeigongList = new short[num];
		for (int i = 0; i < num; i++)
		{
			NeigongList[i] = neigongList[i];
		}
		short[] attackSkillList = other.AttackSkillList;
		int num2 = attackSkillList.Length;
		AttackSkillList = new short[num2];
		for (int j = 0; j < num2; j++)
		{
			AttackSkillList[j] = attackSkillList[j];
		}
		short[] agilitySkillList = other.AgilitySkillList;
		int num3 = agilitySkillList.Length;
		AgilitySkillList = new short[num3];
		for (int k = 0; k < num3; k++)
		{
			AgilitySkillList[k] = agilitySkillList[k];
		}
		short[] defenseSkillList = other.DefenseSkillList;
		int num4 = defenseSkillList.Length;
		DefenseSkillList = new short[num4];
		for (int l = 0; l < num4; l++)
		{
			DefenseSkillList[l] = defenseSkillList[l];
		}
		short[] assistanceSkillList = other.AssistanceSkillList;
		int num5 = assistanceSkillList.Length;
		AssistanceSkillList = new short[num5];
		for (int m = 0; m < num5; m++)
		{
			AssistanceSkillList[m] = assistanceSkillList[m];
		}
		byte[] genericGridAllocation = other.GenericGridAllocation;
		int num6 = genericGridAllocation.Length;
		GenericGridAllocation = new byte[num6];
		for (int n = 0; n < num6; n++)
		{
			GenericGridAllocation[n] = genericGridAllocation[n];
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num = ((NeigongList == null) ? (num + 2) : (num + (2 + 2 * NeigongList.Length)));
		num = ((AttackSkillList == null) ? (num + 2) : (num + (2 + 2 * AttackSkillList.Length)));
		num = ((AgilitySkillList == null) ? (num + 2) : (num + (2 + 2 * AgilitySkillList.Length)));
		num = ((DefenseSkillList == null) ? (num + 2) : (num + (2 + 2 * DefenseSkillList.Length)));
		num = ((AssistanceSkillList == null) ? (num + 2) : (num + (2 + 2 * AssistanceSkillList.Length)));
		num = ((GenericGridAllocation == null) ? (num + 2) : (num + (2 + GenericGridAllocation.Length)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (NeigongList != null)
		{
			int num = NeigongList.Length;
			Tester.Assert(num <= 65535);
			*(ushort*)ptr = (ushort)num;
			ptr += 2;
			for (int i = 0; i < num; i++)
			{
				((short*)ptr)[i] = NeigongList[i];
			}
			ptr += 2 * num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (AttackSkillList != null)
		{
			int num2 = AttackSkillList.Length;
			Tester.Assert(num2 <= 65535);
			*(ushort*)ptr = (ushort)num2;
			ptr += 2;
			for (int j = 0; j < num2; j++)
			{
				((short*)ptr)[j] = AttackSkillList[j];
			}
			ptr += 2 * num2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (AgilitySkillList != null)
		{
			int num3 = AgilitySkillList.Length;
			Tester.Assert(num3 <= 65535);
			*(ushort*)ptr = (ushort)num3;
			ptr += 2;
			for (int k = 0; k < num3; k++)
			{
				((short*)ptr)[k] = AgilitySkillList[k];
			}
			ptr += 2 * num3;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (DefenseSkillList != null)
		{
			int num4 = DefenseSkillList.Length;
			Tester.Assert(num4 <= 65535);
			*(ushort*)ptr = (ushort)num4;
			ptr += 2;
			for (int l = 0; l < num4; l++)
			{
				((short*)ptr)[l] = DefenseSkillList[l];
			}
			ptr += 2 * num4;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (AssistanceSkillList != null)
		{
			int num5 = AssistanceSkillList.Length;
			Tester.Assert(num5 <= 65535);
			*(ushort*)ptr = (ushort)num5;
			ptr += 2;
			for (int m = 0; m < num5; m++)
			{
				((short*)ptr)[m] = AssistanceSkillList[m];
			}
			ptr += 2 * num5;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (GenericGridAllocation != null)
		{
			int num6 = GenericGridAllocation.Length;
			Tester.Assert(num6 <= 65535);
			*(ushort*)ptr = (ushort)num6;
			ptr += 2;
			for (int n = 0; n < num6; n++)
			{
				ptr[n] = GenericGridAllocation[n];
			}
			ptr += num6;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num7 = (int)(ptr - pData);
		if (num7 > 4)
		{
			return (num7 + 3) / 4 * 4;
		}
		return num7;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (NeigongList == null || NeigongList.Length != num)
			{
				NeigongList = new short[num];
			}
			for (int i = 0; i < num; i++)
			{
				NeigongList[i] = ((short*)ptr)[i];
			}
			ptr += 2 * num;
		}
		else
		{
			NeigongList = null;
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (AttackSkillList == null || AttackSkillList.Length != num2)
			{
				AttackSkillList = new short[num2];
			}
			for (int j = 0; j < num2; j++)
			{
				AttackSkillList[j] = ((short*)ptr)[j];
			}
			ptr += 2 * num2;
		}
		else
		{
			AttackSkillList = null;
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (AgilitySkillList == null || AgilitySkillList.Length != num3)
			{
				AgilitySkillList = new short[num3];
			}
			for (int k = 0; k < num3; k++)
			{
				AgilitySkillList[k] = ((short*)ptr)[k];
			}
			ptr += 2 * num3;
		}
		else
		{
			AgilitySkillList = null;
		}
		ushort num4 = *(ushort*)ptr;
		ptr += 2;
		if (num4 > 0)
		{
			if (DefenseSkillList == null || DefenseSkillList.Length != num4)
			{
				DefenseSkillList = new short[num4];
			}
			for (int l = 0; l < num4; l++)
			{
				DefenseSkillList[l] = ((short*)ptr)[l];
			}
			ptr += 2 * num4;
		}
		else
		{
			DefenseSkillList = null;
		}
		ushort num5 = *(ushort*)ptr;
		ptr += 2;
		if (num5 > 0)
		{
			if (AssistanceSkillList == null || AssistanceSkillList.Length != num5)
			{
				AssistanceSkillList = new short[num5];
			}
			for (int m = 0; m < num5; m++)
			{
				AssistanceSkillList[m] = ((short*)ptr)[m];
			}
			ptr += 2 * num5;
		}
		else
		{
			AssistanceSkillList = null;
		}
		ushort num6 = *(ushort*)ptr;
		ptr += 2;
		if (num6 > 0)
		{
			if (GenericGridAllocation == null || GenericGridAllocation.Length != num6)
			{
				GenericGridAllocation = new byte[num6];
			}
			for (int n = 0; n < num6; n++)
			{
				GenericGridAllocation[n] = ptr[n];
			}
			ptr += (int)num6;
		}
		else
		{
			GenericGridAllocation = null;
		}
		int num7 = (int)(ptr - pData);
		if (num7 > 4)
		{
			return (num7 + 3) / 4 * 4;
		}
		return num7;
	}
}
