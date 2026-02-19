using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat;

public class DefeatMarkCollection : ISerializableGameData
{
	[SerializableGameDataField]
	public byte[] OuterInjuryMarkList = new byte[7];

	[SerializableGameDataField]
	public byte[] InnerInjuryMarkList = new byte[7];

	[SerializableGameDataField]
	public ByteList[] FlawMarkList = new ByteList[7];

	[SerializableGameDataField]
	public ByteList[] AcupointMarkList = new ByteList[7];

	[SerializableGameDataField]
	public byte[] PoisonMarkList = new byte[6];

	[SerializableGameDataField]
	public List<bool> MindMarkList = new List<bool>();

	[SerializableGameDataField]
	public List<CombatSkillKey> DieMarkList = new List<CombatSkillKey>();

	[SerializableGameDataField]
	public int FatalDamageMarkCount = 0;

	[SerializableGameDataField]
	public sbyte WugMarkCount = 0;

	[SerializableGameDataField]
	public sbyte QiDisorderMarkCount = 0;

	[SerializableGameDataField]
	public sbyte StateMarkCount = 0;

	[SerializableGameDataField]
	public (sbyte scatter, sbyte bulge) NeiliAllocationMarkCount = (scatter: 0, bulge: 0);

	[SerializableGameDataField]
	public sbyte HealthMarkCount = 0;

	private static short QiDisorderFirstExtra => GlobalConfig.Instance.DefeatMarkQiDisorderFirstExtra;

	private static short QiDisorderThreshold => GlobalConfig.Instance.DefeatMarkQiDisorderThreshold;

	public IEnumerable<DefeatMarkKey> GetAllKeys(short oldDisorderOfQi, PoisonInts oldPoisons, Injuries oldInjuries)
	{
		for (int i = 0; i < DieMarkList.Count; i++)
		{
			yield return EMarkType.Die;
		}
		for (int j = 0; j < HealthMarkCount; j++)
		{
			yield return EMarkType.Health;
		}
		for (int k = 0; k < WugMarkCount; k++)
		{
			yield return EMarkType.Wug;
		}
		for (int l = 0; l < StateMarkCount; l++)
		{
			yield return EMarkType.State;
		}
		for (int m = 0; m < NeiliAllocationMarkCount.scatter; m++)
		{
			yield return EMarkType.NeiliAllocation;
		}
		for (int n = 0; n < NeiliAllocationMarkCount.bulge; n++)
		{
			yield return (markType: EMarkType.NeiliAllocation, subType: 1);
		}
		sbyte oldMarkCountQiDisorder = CalcQiDisorderMarkCount(oldDisorderOfQi);
		for (int num = 0; num < QiDisorderMarkCount; num++)
		{
			yield return (markType: EMarkType.QiDisorder, subType: 0, subType2: (num < oldMarkCountQiDisorder) ? 1 : 0);
		}
		for (sbyte order = 0; order < 6; order++)
		{
			sbyte type = PoisonType.GetTypeBySortingOrder(order);
			int oldMarkCount = PoisonsAndLevels.CalcPoisonedLevel(oldPoisons[type]);
			for (int num2 = 0; num2 < PoisonMarkList[type]; num2++)
			{
				yield return (markType: EMarkType.Poison, subType: type, subType2: (num2 < oldMarkCount) ? 1 : 0);
			}
		}
		for (sbyte part = 0; part < 7; part++)
		{
			sbyte oldMarkCount2 = oldInjuries.Get(part, isInnerInjury: false);
			for (int num3 = 0; num3 < OuterInjuryMarkList[part]; num3++)
			{
				yield return (markType: EMarkType.Outer, subType: part, subType2: (num3 < oldMarkCount2) ? 1 : 0);
			}
		}
		for (sbyte part2 = 0; part2 < 7; part2++)
		{
			sbyte oldMarkCount3 = oldInjuries.Get(part2, isInnerInjury: true);
			for (int num4 = 0; num4 < InnerInjuryMarkList[part2]; num4++)
			{
				yield return (markType: EMarkType.Inner, subType: part2, subType2: (num4 < oldMarkCount3) ? 1 : 0);
			}
		}
		for (sbyte part3 = 0; part3 < 7; part3++)
		{
			foreach (byte flaw in FlawMarkList[part3])
			{
				yield return (markType: EMarkType.Flaw, subType: part3, subType2: flaw);
			}
		}
		for (sbyte part4 = 0; part4 < 7; part4++)
		{
			foreach (byte acupoint in AcupointMarkList[part4])
			{
				yield return (markType: EMarkType.Acupoint, subType: part4, subType2: acupoint);
			}
		}
		for (int num5 = 0; num5 < FatalDamageMarkCount; num5++)
		{
			yield return EMarkType.Fatal;
		}
		for (int num6 = 0; num6 < MindMarkList.Count; num6++)
		{
			yield return (markType: EMarkType.Mind, subType: 0, subType2: MindMarkList[num6] ? 1 : 0);
		}
	}

	public static sbyte CalcQiDisorderMarkCount(int disorderOfQi)
	{
		return (sbyte)MathUtils.Clamp((disorderOfQi - QiDisorderFirstExtra) / QiDisorderThreshold, 0, 6);
	}

	public static short CalcQiDisorderMarkThreshold(int disorderOfQi)
	{
		return (CalcQiDisorderMarkCount(disorderOfQi) == 0) ? ((short)(QiDisorderThreshold + QiDisorderFirstExtra)) : QiDisorderThreshold;
	}

	public int GetTotalCount()
	{
		return OuterInjuryMarkList.Sum() + InnerInjuryMarkList.Sum() + GetTotalFlawCount() + GetTotalAcupointCount() + PoisonMarkList.Sum() + MindMarkList.Count + DieMarkList.Count + FatalDamageMarkCount + WugMarkCount + QiDisorderMarkCount + StateMarkCount + NeiliAllocationMarkCount.scatter + NeiliAllocationMarkCount.bulge + HealthMarkCount;
	}

	public int GetTotalInjuryCount()
	{
		int num = 0;
		num += OuterInjuryMarkList.Sum();
		return num + InnerInjuryMarkList.Sum();
	}

	public int GetTotalFlawCount()
	{
		int num = 0;
		for (sbyte b = 0; b < 7; b++)
		{
			num += FlawMarkList[b].Count;
		}
		return num;
	}

	public int GetTotalAcupointCount()
	{
		int num = 0;
		for (sbyte b = 0; b < 7; b++)
		{
			num += AcupointMarkList[b].Count;
		}
		return num;
	}

	public void Clear()
	{
		for (sbyte b = 0; b < 7; b++)
		{
			OuterInjuryMarkList[b] = 0;
			InnerInjuryMarkList[b] = 0;
			FlawMarkList[b].Clear();
			AcupointMarkList[b].Clear();
		}
		for (int i = 0; i < 6; i++)
		{
			PoisonMarkList[i] = 0;
		}
		MindMarkList.Clear();
		DieMarkList.Clear();
		FatalDamageMarkCount = 0;
		WugMarkCount = (QiDisorderMarkCount = (StateMarkCount = (HealthMarkCount = 0)));
		NeiliAllocationMarkCount = (scatter: 0, bulge: 0);
	}

	public bool AnyMarkAdded(DefeatMarkCollection other)
	{
		for (sbyte b = 0; b < 7; b++)
		{
			if (OuterInjuryMarkList[b] > other.OuterInjuryMarkList[b] || InnerInjuryMarkList[b] > other.InnerInjuryMarkList[b] || FlawMarkList[b].Count > other.FlawMarkList[b].Count || AcupointMarkList[b].Count > other.AcupointMarkList[b].Count)
			{
				return true;
			}
		}
		for (sbyte b2 = 0; b2 < 6; b2++)
		{
			if (PoisonMarkList[b2] > other.PoisonMarkList[b2])
			{
				return true;
			}
		}
		if (MindMarkList.Count > other.MindMarkList.Count || DieMarkList.Count > other.DieMarkList.Count)
		{
			return true;
		}
		if (FatalDamageMarkCount > other.FatalDamageMarkCount)
		{
			return true;
		}
		if (WugMarkCount > other.WugMarkCount)
		{
			return true;
		}
		if (QiDisorderMarkCount > other.QiDisorderMarkCount)
		{
			return true;
		}
		if (StateMarkCount > other.StateMarkCount)
		{
			return true;
		}
		if (NeiliAllocationMarkCount.scatter > other.NeiliAllocationMarkCount.scatter || NeiliAllocationMarkCount.bulge > other.NeiliAllocationMarkCount.bulge)
		{
			return true;
		}
		if (HealthMarkCount > other.HealthMarkCount)
		{
			return true;
		}
		return false;
	}

	public DefeatMarkCollection()
	{
		for (sbyte b = 0; b < 7; b++)
		{
			FlawMarkList[b] = new ByteList();
			AcupointMarkList[b] = new ByteList();
		}
	}

	public DefeatMarkCollection(DefeatMarkCollection other)
	{
		byte[] outerInjuryMarkList = other.OuterInjuryMarkList;
		int num = outerInjuryMarkList.Length;
		OuterInjuryMarkList = new byte[num];
		for (int i = 0; i < num; i++)
		{
			OuterInjuryMarkList[i] = outerInjuryMarkList[i];
		}
		byte[] innerInjuryMarkList = other.InnerInjuryMarkList;
		int num2 = innerInjuryMarkList.Length;
		InnerInjuryMarkList = new byte[num2];
		for (int j = 0; j < num2; j++)
		{
			InnerInjuryMarkList[j] = innerInjuryMarkList[j];
		}
		ByteList[] flawMarkList = other.FlawMarkList;
		int num3 = flawMarkList.Length;
		FlawMarkList = new ByteList[num3];
		for (int k = 0; k < num3; k++)
		{
			FlawMarkList[k] = new ByteList(flawMarkList[k]);
		}
		ByteList[] acupointMarkList = other.AcupointMarkList;
		int num4 = acupointMarkList.Length;
		AcupointMarkList = new ByteList[num4];
		for (int l = 0; l < num4; l++)
		{
			AcupointMarkList[l] = new ByteList(acupointMarkList[l]);
		}
		byte[] poisonMarkList = other.PoisonMarkList;
		int num5 = poisonMarkList.Length;
		PoisonMarkList = new byte[num5];
		for (int m = 0; m < num5; m++)
		{
			PoisonMarkList[m] = poisonMarkList[m];
		}
		MindMarkList = ((other.MindMarkList == null) ? null : new List<bool>(other.MindMarkList));
		DieMarkList = ((other.DieMarkList == null) ? null : new List<CombatSkillKey>(other.DieMarkList));
		FatalDamageMarkCount = other.FatalDamageMarkCount;
		WugMarkCount = other.WugMarkCount;
		QiDisorderMarkCount = other.QiDisorderMarkCount;
		StateMarkCount = other.StateMarkCount;
		NeiliAllocationMarkCount = other.NeiliAllocationMarkCount;
		HealthMarkCount = other.HealthMarkCount;
	}

	public void Assign(DefeatMarkCollection other)
	{
		byte[] outerInjuryMarkList = other.OuterInjuryMarkList;
		int num = outerInjuryMarkList.Length;
		OuterInjuryMarkList = new byte[num];
		for (int i = 0; i < num; i++)
		{
			OuterInjuryMarkList[i] = outerInjuryMarkList[i];
		}
		byte[] innerInjuryMarkList = other.InnerInjuryMarkList;
		int num2 = innerInjuryMarkList.Length;
		InnerInjuryMarkList = new byte[num2];
		for (int j = 0; j < num2; j++)
		{
			InnerInjuryMarkList[j] = innerInjuryMarkList[j];
		}
		ByteList[] flawMarkList = other.FlawMarkList;
		int num3 = flawMarkList.Length;
		FlawMarkList = new ByteList[num3];
		for (int k = 0; k < num3; k++)
		{
			FlawMarkList[k] = new ByteList(flawMarkList[k]);
		}
		ByteList[] acupointMarkList = other.AcupointMarkList;
		int num4 = acupointMarkList.Length;
		AcupointMarkList = new ByteList[num4];
		for (int l = 0; l < num4; l++)
		{
			AcupointMarkList[l] = new ByteList(acupointMarkList[l]);
		}
		byte[] poisonMarkList = other.PoisonMarkList;
		int num5 = poisonMarkList.Length;
		PoisonMarkList = new byte[num5];
		for (int m = 0; m < num5; m++)
		{
			PoisonMarkList[m] = poisonMarkList[m];
		}
		MindMarkList = ((other.MindMarkList == null) ? null : new List<bool>(other.MindMarkList));
		DieMarkList = ((other.DieMarkList == null) ? null : new List<CombatSkillKey>(other.DieMarkList));
		FatalDamageMarkCount = other.FatalDamageMarkCount;
		WugMarkCount = other.WugMarkCount;
		QiDisorderMarkCount = other.QiDisorderMarkCount;
		StateMarkCount = other.StateMarkCount;
		NeiliAllocationMarkCount = other.NeiliAllocationMarkCount;
		HealthMarkCount = other.HealthMarkCount;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 10;
		num = ((OuterInjuryMarkList == null) ? (num + 2) : (num + (2 + OuterInjuryMarkList.Length)));
		num = ((InnerInjuryMarkList == null) ? (num + 2) : (num + (2 + InnerInjuryMarkList.Length)));
		if (FlawMarkList != null)
		{
			num += 2;
			int num2 = FlawMarkList.Length;
			for (int i = 0; i < num2; i++)
			{
				ByteList byteList = FlawMarkList[i];
				num = ((byteList == null) ? (num + 2) : (num + (2 + byteList.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		if (AcupointMarkList != null)
		{
			num += 2;
			int num3 = AcupointMarkList.Length;
			for (int j = 0; j < num3; j++)
			{
				ByteList byteList2 = AcupointMarkList[j];
				num = ((byteList2 == null) ? (num + 2) : (num + (2 + byteList2.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		num = ((PoisonMarkList == null) ? (num + 2) : (num + (2 + PoisonMarkList.Length)));
		num = ((MindMarkList == null) ? (num + 2) : (num + (2 + MindMarkList.Count)));
		num = ((DieMarkList == null) ? (num + 2) : (num + (2 + 8 * DieMarkList.Count)));
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (OuterInjuryMarkList != null)
		{
			int num = OuterInjuryMarkList.Length;
			Tester.Assert(num <= 65535);
			*(ushort*)ptr = (ushort)num;
			ptr += 2;
			for (int i = 0; i < num; i++)
			{
				ptr[i] = OuterInjuryMarkList[i];
			}
			ptr += num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (InnerInjuryMarkList != null)
		{
			int num2 = InnerInjuryMarkList.Length;
			Tester.Assert(num2 <= 65535);
			*(ushort*)ptr = (ushort)num2;
			ptr += 2;
			for (int j = 0; j < num2; j++)
			{
				ptr[j] = InnerInjuryMarkList[j];
			}
			ptr += num2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (FlawMarkList != null)
		{
			int num3 = FlawMarkList.Length;
			Tester.Assert(num3 <= 65535);
			*(ushort*)ptr = (ushort)num3;
			ptr += 2;
			for (int k = 0; k < num3; k++)
			{
				ByteList byteList = FlawMarkList[k];
				if (byteList != null)
				{
					byte* ptr2 = ptr;
					ptr += 2;
					int num4 = byteList.Serialize(ptr);
					ptr += num4;
					Tester.Assert(num4 <= 65535);
					*(ushort*)ptr2 = (ushort)num4;
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
		if (AcupointMarkList != null)
		{
			int num5 = AcupointMarkList.Length;
			Tester.Assert(num5 <= 65535);
			*(ushort*)ptr = (ushort)num5;
			ptr += 2;
			for (int l = 0; l < num5; l++)
			{
				ByteList byteList2 = AcupointMarkList[l];
				if (byteList2 != null)
				{
					byte* ptr3 = ptr;
					ptr += 2;
					int num6 = byteList2.Serialize(ptr);
					ptr += num6;
					Tester.Assert(num6 <= 65535);
					*(ushort*)ptr3 = (ushort)num6;
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
		if (PoisonMarkList != null)
		{
			int num7 = PoisonMarkList.Length;
			Tester.Assert(num7 <= 65535);
			*(ushort*)ptr = (ushort)num7;
			ptr += 2;
			for (int m = 0; m < num7; m++)
			{
				ptr[m] = PoisonMarkList[m];
			}
			ptr += num7;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (MindMarkList != null)
		{
			int count = MindMarkList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int n = 0; n < count; n++)
			{
				ptr[n] = (MindMarkList[n] ? ((byte)1) : ((byte)0));
			}
			ptr += count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (DieMarkList != null)
		{
			int count2 = DieMarkList.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int num8 = 0; num8 < count2; num8++)
			{
				ptr += DieMarkList[num8].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(int*)ptr = FatalDamageMarkCount;
		ptr += 4;
		*ptr = (byte)WugMarkCount;
		ptr++;
		*ptr = (byte)QiDisorderMarkCount;
		ptr++;
		*ptr = (byte)StateMarkCount;
		ptr++;
		ptr += SerializationHelper.Serialize<sbyte, sbyte>(ptr, NeiliAllocationMarkCount);
		*ptr = (byte)HealthMarkCount;
		ptr++;
		int num9 = (int)(ptr - pData);
		return (num9 <= 4) ? num9 : ((num9 + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (OuterInjuryMarkList == null || OuterInjuryMarkList.Length != num)
			{
				OuterInjuryMarkList = new byte[num];
			}
			for (int i = 0; i < num; i++)
			{
				OuterInjuryMarkList[i] = ptr[i];
			}
			ptr += (int)num;
		}
		else
		{
			OuterInjuryMarkList = null;
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (InnerInjuryMarkList == null || InnerInjuryMarkList.Length != num2)
			{
				InnerInjuryMarkList = new byte[num2];
			}
			for (int j = 0; j < num2; j++)
			{
				InnerInjuryMarkList[j] = ptr[j];
			}
			ptr += (int)num2;
		}
		else
		{
			InnerInjuryMarkList = null;
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (FlawMarkList == null || FlawMarkList.Length != num3)
			{
				FlawMarkList = new ByteList[num3];
			}
			for (int k = 0; k < num3; k++)
			{
				ushort num4 = *(ushort*)ptr;
				ptr += 2;
				if (num4 > 0)
				{
					ByteList byteList = FlawMarkList[k] ?? new ByteList();
					ptr += byteList.Deserialize(ptr);
					FlawMarkList[k] = byteList;
				}
				else
				{
					FlawMarkList[k] = null;
				}
			}
		}
		else
		{
			FlawMarkList = null;
		}
		ushort num5 = *(ushort*)ptr;
		ptr += 2;
		if (num5 > 0)
		{
			if (AcupointMarkList == null || AcupointMarkList.Length != num5)
			{
				AcupointMarkList = new ByteList[num5];
			}
			for (int l = 0; l < num5; l++)
			{
				ushort num6 = *(ushort*)ptr;
				ptr += 2;
				if (num6 > 0)
				{
					ByteList byteList2 = AcupointMarkList[l] ?? new ByteList();
					ptr += byteList2.Deserialize(ptr);
					AcupointMarkList[l] = byteList2;
				}
				else
				{
					AcupointMarkList[l] = null;
				}
			}
		}
		else
		{
			AcupointMarkList = null;
		}
		ushort num7 = *(ushort*)ptr;
		ptr += 2;
		if (num7 > 0)
		{
			if (PoisonMarkList == null || PoisonMarkList.Length != num7)
			{
				PoisonMarkList = new byte[num7];
			}
			for (int m = 0; m < num7; m++)
			{
				PoisonMarkList[m] = ptr[m];
			}
			ptr += (int)num7;
		}
		else
		{
			PoisonMarkList = null;
		}
		ushort num8 = *(ushort*)ptr;
		ptr += 2;
		if (num8 > 0)
		{
			if (MindMarkList == null)
			{
				MindMarkList = new List<bool>(num8);
			}
			else
			{
				MindMarkList.Clear();
			}
			for (int n = 0; n < num8; n++)
			{
				MindMarkList.Add(ptr[n] != 0);
			}
			ptr += (int)num8;
		}
		else
		{
			MindMarkList?.Clear();
		}
		ushort num9 = *(ushort*)ptr;
		ptr += 2;
		if (num9 > 0)
		{
			if (DieMarkList == null)
			{
				DieMarkList = new List<CombatSkillKey>(num9);
			}
			else
			{
				DieMarkList.Clear();
			}
			for (int num10 = 0; num10 < num9; num10++)
			{
				CombatSkillKey item = default(CombatSkillKey);
				ptr += item.Deserialize(ptr);
				DieMarkList.Add(item);
			}
		}
		else
		{
			DieMarkList?.Clear();
		}
		FatalDamageMarkCount = *(int*)ptr;
		ptr += 4;
		WugMarkCount = (sbyte)(*ptr);
		ptr++;
		QiDisorderMarkCount = (sbyte)(*ptr);
		ptr++;
		StateMarkCount = (sbyte)(*ptr);
		ptr++;
		ptr += SerializationHelper.Deserialize<sbyte, sbyte>(ptr, ref NeiliAllocationMarkCount);
		HealthMarkCount = (sbyte)(*ptr);
		ptr++;
		int num11 = (int)(ptr - pData);
		return (num11 <= 4) ? num11 : ((num11 + 3) / 4 * 4);
	}

	public IEnumerable<DefeatMarkKey> GetAllKeys(CombatCharacter combatChar)
	{
		return GetAllKeys(combatChar.GetOldDisorderOfQi(), combatChar.GetOldPoison(), combatChar.GetOldInjuries());
	}

	public IEnumerable<DefeatMarkKey> GetAllKeysWithoutOld()
	{
		PoisonInts oldPoisons = default(PoisonInts);
		oldPoisons.Initialize();
		Injuries oldInjuries = default(Injuries);
		oldInjuries.Initialize();
		return GetAllKeys(0, oldPoisons, oldInjuries);
	}

	public void SyncMindMark(DataContext context, CombatCharacter combatChar)
	{
		Tester.Assert(combatChar.GetDefeatMarkCollection() == this);
		int count = MindMarkList.Count;
		if (SyncMindMark(combatChar))
		{
			combatChar.SetDefeatMarkCollection(this, context);
		}
		int count2 = MindMarkList.Count;
		if (count2 > count)
		{
			DomainManager.Combat.AddToCheckFallenSet(combatChar.GetId());
		}
	}

	public void SyncOtherMark(DataContext context, CombatCharacter combatChar)
	{
		Tester.Assert(combatChar.GetDefeatMarkCollection() == this);
		int totalCount = GetTotalCount();
		if (SyncOtherMark(combatChar))
		{
			combatChar.SetDefeatMarkCollection(this, context);
		}
		int totalCount2 = GetTotalCount();
		if (totalCount2 > totalCount)
		{
			DomainManager.Combat.AddToCheckFallenSet(combatChar.GetId());
		}
	}

	public void SyncWugMark(DataContext context, DataUid uid)
	{
		int objectId = (int)uid.SubId0;
		if (DomainManager.Combat.TryGetElement_CombatCharacterDict(objectId, out var element))
		{
			Tester.Assert(element.GetDefeatMarkCollection() == this);
			sbyte wugMarkCount = WugMarkCount;
			if (SyncWugMark(element))
			{
				element.SetDefeatMarkCollection(this, context);
			}
			sbyte wugMarkCount2 = WugMarkCount;
			if (wugMarkCount < wugMarkCount2)
			{
				DomainManager.Combat.AddToCheckFallenSet(element.GetId());
			}
		}
	}

	public void SyncQiDisorderMark(DataContext context, DataUid uid)
	{
		int objectId = (int)uid.SubId0;
		if (DomainManager.Combat.TryGetElement_CombatCharacterDict(objectId, out var element))
		{
			Tester.Assert(element.GetDefeatMarkCollection() == this);
			sbyte qiDisorderMarkCount = QiDisorderMarkCount;
			if (SyncQiDisorderMark(element))
			{
				element.SetDefeatMarkCollection(this, context);
			}
			sbyte qiDisorderMarkCount2 = QiDisorderMarkCount;
			if (qiDisorderMarkCount < qiDisorderMarkCount2)
			{
				DomainManager.Combat.AddToCheckFallenSet(element.GetId());
			}
		}
	}

	public void SyncCombatStateMark(DataContext context, CombatCharacter combatChar)
	{
		Tester.Assert(combatChar.GetDefeatMarkCollection() == this);
		sbyte stateMarkCount = StateMarkCount;
		if (SyncCombatStateMark(combatChar))
		{
			combatChar.SetDefeatMarkCollection(this, context);
		}
		sbyte stateMarkCount2 = StateMarkCount;
		if (stateMarkCount < stateMarkCount2)
		{
			DomainManager.Combat.AddToCheckFallenSet(combatChar.GetId());
		}
	}

	public void SyncNeiliAllocationMark(DataContext context, CombatCharacter combatChar)
	{
		Tester.Assert(combatChar.GetDefeatMarkCollection() == this);
		(sbyte, sbyte) neiliAllocationMarkCount = NeiliAllocationMarkCount;
		if (SyncNeiliAllocationMark(combatChar))
		{
			combatChar.SetDefeatMarkCollection(this, context);
		}
		(sbyte, sbyte) neiliAllocationMarkCount2 = NeiliAllocationMarkCount;
		if (neiliAllocationMarkCount.Item1 + neiliAllocationMarkCount.Item2 < neiliAllocationMarkCount2.Item1 + neiliAllocationMarkCount2.Item2)
		{
			DomainManager.Combat.AddToCheckFallenSet(combatChar.GetId());
		}
	}

	public void SyncHealthMark(DataContext context, DataUid uid)
	{
		int objectId = (int)uid.SubId0;
		if (DomainManager.Combat.TryGetElement_CombatCharacterDict(objectId, out var element))
		{
			Tester.Assert(element.GetDefeatMarkCollection() == this);
			sbyte healthMarkCount = HealthMarkCount;
			if (SyncHealthMark(element))
			{
				element.SetDefeatMarkCollection(this, context);
			}
			sbyte healthMarkCount2 = HealthMarkCount;
			if (healthMarkCount < healthMarkCount2)
			{
				DomainManager.Combat.AddToCheckFallenSet(element.GetId());
			}
		}
	}

	private bool SyncMindMark(CombatCharacter combatChar)
	{
		List<bool> list = ObjectPool<List<bool>>.Instance.Get();
		list.Clear();
		list.AddRange(combatChar.GetMindMarkTime().MarkList.Select((SilenceFrameData x) => x.Infinity));
		CollectionUtils.Sort(list, (bool a, bool b) => b.CompareTo(a));
		bool result;
		if (list.SequenceEqual(MindMarkList))
		{
			result = false;
		}
		else
		{
			result = true;
			MindMarkList.Clear();
			MindMarkList.AddRange(list);
		}
		ObjectPool<List<bool>>.Instance.Return(list);
		return result;
	}

	private bool SyncOtherMark(CombatCharacter combatChar)
	{
		bool flag = SyncWugMark(combatChar);
		flag = SyncQiDisorderMark(combatChar) || flag;
		flag = SyncCombatStateMark(combatChar) || flag;
		flag = SyncNeiliAllocationMark(combatChar) || flag;
		return SyncHealthMark(combatChar) || flag;
	}

	private bool SyncWugMark(CombatCharacter combatChar)
	{
		sbyte b = combatChar.GetCharacter().GetEatingItems().CountOfWugMark();
		if (b == WugMarkCount)
		{
			return false;
		}
		WugMarkCount = b;
		return true;
	}

	private bool SyncQiDisorderMark(CombatCharacter combatChar)
	{
		sbyte qiDisorderMarkCount = GetQiDisorderMarkCount(combatChar);
		if (qiDisorderMarkCount == QiDisorderMarkCount)
		{
			return false;
		}
		QiDisorderMarkCount = qiDisorderMarkCount;
		return true;
	}

	private bool SyncCombatStateMark(CombatCharacter combatChar)
	{
		sbyte combatStateMarkCount = GetCombatStateMarkCount(combatChar);
		if (combatStateMarkCount == StateMarkCount)
		{
			return false;
		}
		StateMarkCount = combatStateMarkCount;
		return true;
	}

	private bool SyncNeiliAllocationMark(CombatCharacter combatChar)
	{
		(sbyte, sbyte) neiliAllocationMarkCount = GetNeiliAllocationMarkCount(combatChar);
		var (b, b2) = neiliAllocationMarkCount;
		var (b3, b4) = NeiliAllocationMarkCount;
		if (b == b3 && b2 == b4)
		{
			return false;
		}
		NeiliAllocationMarkCount = neiliAllocationMarkCount;
		return true;
	}

	private bool SyncHealthMark(CombatCharacter combatChar)
	{
		sbyte healthMarkCount = GetHealthMarkCount(combatChar.GetCharacter());
		if (healthMarkCount == HealthMarkCount)
		{
			return false;
		}
		HealthMarkCount = healthMarkCount;
		return true;
	}

	private static sbyte GetQiDisorderMarkCount(CombatCharacter combatChar)
	{
		return CalcQiDisorderMarkCount(combatChar.GetCharacter().GetDisorderOfQi());
	}

	private static sbyte GetCombatStateMarkCount(CombatCharacter combatChar)
	{
		short defeatMarkCombatStatePower = GlobalConfig.Instance.DefeatMarkCombatStatePower;
		sbyte defeatMarkCombatStateMaxCount = GlobalConfig.Instance.DefeatMarkCombatStateMaxCount;
		return (sbyte)Math.Clamp(-combatChar.GetCombatStateTotalBuffPower() / defeatMarkCombatStatePower, 0, defeatMarkCombatStateMaxCount);
	}

	private static (sbyte scatter, sbyte bulge) GetNeiliAllocationMarkCount(CombatCharacter combatChar)
	{
		sbyte b = 0;
		sbyte b2 = 0;
		for (byte b3 = 0; b3 < 4; b3++)
		{
			NeiliAllocationStatusItem config = combatChar.GetNeiliAllocationStatus(b3).GetConfig();
			if (config.MarkCount < 0)
			{
				b += Math.Abs(config.MarkCount);
			}
			else if (config.MarkCount > 0)
			{
				b2 += Math.Abs(config.MarkCount);
			}
		}
		return (scatter: b, bulge: b2);
	}

	public static sbyte GetHealthMarkCount(GameData.Domains.Character.Character character)
	{
		EHealthType healthType = character.GetHealthType();
		if (1 == 0)
		{
		}
		sbyte result;
		switch (healthType)
		{
		case EHealthType.Dying:
			result = 8;
			break;
		case EHealthType.CriticallyIll:
			result = 6;
			break;
		case EHealthType.Weak:
			result = 4;
			break;
		case EHealthType.Sick:
			result = 2;
			break;
		case EHealthType.Healthy:
		case EHealthType.Unknown:
			result = 0;
			break;
		default:
			result = 0;
			break;
		}
		if (1 == 0)
		{
		}
		return result;
	}
}
