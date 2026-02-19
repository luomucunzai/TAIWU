using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.TemporaryModification;

[SerializableGameData(NotForDisplayModule = true)]
public class CharacterModificationRecords : RawDataBlock
{
	public int Savepoint;

	public CharacterModificationRecords()
	{
		Savepoint = -1;
	}

	public CharacterModificationRecords(int initialCapacity)
		: base(initialCapacity)
	{
		Savepoint = -1;
	}

	public void RecordHappiness(sbyte oriValue, sbyte currValue)
	{
		int delta = currValue - oriValue;
		int offset = BeginAddingRecord(RevertibleCharacterPropertyType.Happiness, 4);
		WriteDelta(offset, delta);
	}

	public unsafe static int RetrieveHappiness(byte* pModificationData)
	{
		return ReadDeltaInt(pModificationData);
	}

	public void RecordBaseMorality(short oriValue, short currValue)
	{
		int delta = currValue - oriValue;
		int offset = BeginAddingRecord(RevertibleCharacterPropertyType.BaseMorality, 4);
		WriteDelta(offset, delta);
	}

	public unsafe static int RetrieveBaseMorality(byte* pModificationData)
	{
		return ReadDeltaInt(pModificationData);
	}

	public void RecordFeatureIds(List<short> oriValue, List<short> currValue)
	{
		List<ShortListModification> modifications = CalcDelta(oriValue, currValue);
		ushort dataSize = CalcDataSize(modifications);
		int offset = BeginAddingRecord(RevertibleCharacterPropertyType.FeatureIds, dataSize);
		WriteDelta(offset, modifications);
	}

	public unsafe static List<ShortListModification> RetrieveFeatureIds(byte* pModificationData, ushort dataSize)
	{
		return ReadDeltaShortList(pModificationData, dataSize);
	}

	public void RecordBaseMainAttributes(MainAttributes oriValue, MainAttributes currValue)
	{
		MainAttributes delta = currValue.Subtract(oriValue);
		ushort dataSize = (ushort)delta.GetSerializedSize();
		int offset = BeginAddingRecord(RevertibleCharacterPropertyType.BaseMainAttributes, dataSize);
		WriteDelta(offset, delta);
	}

	public unsafe static MainAttributes RetrieveBaseMainAttributes(byte* pModificationData)
	{
		return ReadDeltaMainAttributes(pModificationData);
	}

	public void RecordDisorderOfQi(short oriValue, short currValue)
	{
		int delta = currValue - oriValue;
		int offset = BeginAddingRecord(RevertibleCharacterPropertyType.DisorderOfQi, 4);
		WriteDelta(offset, delta);
	}

	public unsafe static int RetrieveDisorderOfQi(byte* pModificationData)
	{
		return ReadDeltaInt(pModificationData);
	}

	public void RecordInjuries(Injuries oriValue, Injuries currValue)
	{
		Injuries delta = currValue.Subtract(oriValue);
		ushort dataSize = (ushort)delta.GetSerializedSize();
		int offset = BeginAddingRecord(RevertibleCharacterPropertyType.Injuries, dataSize);
		WriteDelta(offset, delta);
	}

	public unsafe static Injuries RetrieveInjuries(byte* pModificationData)
	{
		return ReadDeltaInjuries(pModificationData);
	}

	public void RecordExtraNeili(int oriValue, int currValue)
	{
		int delta = currValue - oriValue;
		int offset = BeginAddingRecord(RevertibleCharacterPropertyType.ExtraNeili, 4);
		WriteDelta(offset, delta);
	}

	public unsafe static int RetrieveExtraNeili(byte* pModificationData)
	{
		return ReadDeltaInt(pModificationData);
	}

	public void RecordConsummateLevel(sbyte oriValue, sbyte currValue)
	{
		int delta = currValue - oriValue;
		int offset = BeginAddingRecord(RevertibleCharacterPropertyType.ConsummateLevel, 4);
		WriteDelta(offset, delta);
	}

	public unsafe static int RetrieveConsummateLevel(byte* pModificationData)
	{
		return ReadDeltaInt(pModificationData);
	}

	public void RecordBaseLifeSkillQualifications(ref LifeSkillShorts oriValue, ref LifeSkillShorts currValue)
	{
		LifeSkillShorts delta = currValue.Subtract(ref oriValue);
		ushort dataSize = (ushort)delta.GetSerializedSize();
		int offset = BeginAddingRecord(RevertibleCharacterPropertyType.BaseLifeSkillQualifications, dataSize);
		WriteDelta(offset, ref delta);
	}

	public unsafe static LifeSkillShorts RetrieveBaseLifeSkillQualifications(byte* pModificationData)
	{
		return ReadDeltaLifeSkillShorts(pModificationData);
	}

	public void RecordBaseCombatSkillQualifications(ref CombatSkillShorts oriValue, ref CombatSkillShorts currValue)
	{
		CombatSkillShorts delta = currValue.Subtract(ref oriValue);
		ushort dataSize = (ushort)delta.GetSerializedSize();
		int offset = BeginAddingRecord(RevertibleCharacterPropertyType.BaseCombatSkillQualifications, dataSize);
		WriteDelta(offset, ref delta);
	}

	public unsafe static CombatSkillShorts RetrieveBaseCombatSkillQualifications(byte* pModificationData)
	{
		return ReadDeltaCombatSkillShorts(pModificationData);
	}

	public void RecordResources(ref ResourceInts oriValue, ref ResourceInts currValue)
	{
		ResourceInts delta = currValue.Subtract(ref oriValue);
		ushort dataSize = (ushort)delta.GetSerializedSize();
		int offset = BeginAddingRecord(RevertibleCharacterPropertyType.Resources, dataSize);
		WriteDelta(offset, ref delta);
	}

	public unsafe static ResourceInts RetrieveResources(byte* pModificationData)
	{
		return ReadDeltaResourceInts(pModificationData);
	}

	public void RecordCurrMainAttributes(MainAttributes oriValue, MainAttributes currValue)
	{
		MainAttributes delta = currValue.Subtract(oriValue);
		ushort dataSize = (ushort)delta.GetSerializedSize();
		int offset = BeginAddingRecord(RevertibleCharacterPropertyType.CurrMainAttributes, dataSize);
		WriteDelta(offset, delta);
	}

	public unsafe static MainAttributes RetrieveCurrMainAttributes(byte* pModificationData)
	{
		return ReadDeltaMainAttributes(pModificationData);
	}

	public void RecordPoisoned(ref PoisonInts oriValue, ref PoisonInts currValue)
	{
		PoisonInts delta = currValue.Subtract(ref oriValue);
		ushort dataSize = (ushort)delta.GetSerializedSize();
		int offset = BeginAddingRecord(RevertibleCharacterPropertyType.Poisoned, dataSize);
		WriteDelta(offset, ref delta);
	}

	public unsafe static PoisonInts RetrievePoisoned(byte* pModificationData)
	{
		return ReadDeltaPoisonInts(pModificationData);
	}

	public void RecordCurrNeili(int oriValue, int currValue)
	{
		int delta = currValue - oriValue;
		int offset = BeginAddingRecord(RevertibleCharacterPropertyType.CurrNeili, 4);
		WriteDelta(offset, delta);
	}

	public unsafe static int RetrieveCurrNeili(byte* pModificationData)
	{
		return ReadDeltaInt(pModificationData);
	}

	public void RecordExtraNeiliAllocation(NeiliAllocation oriValue, NeiliAllocation currValue)
	{
		NeiliAllocation delta = currValue.Subtract(oriValue);
		ushort dataSize = (ushort)delta.GetSerializedSize();
		int offset = BeginAddingRecord(RevertibleCharacterPropertyType.ExtraNeiliAllocation, dataSize);
		WriteDelta(offset, delta);
	}

	public unsafe static NeiliAllocation RetrieveExtraNeiliAllocation(byte* pModificationData)
	{
		return ReadDeltaNeiliAllocation(pModificationData);
	}

	public void RecordXiangshuInfection(byte oriValue, byte currValue)
	{
		int delta = currValue - oriValue;
		int offset = BeginAddingRecord(RevertibleCharacterPropertyType.XiangshuInfection, 4);
		WriteDelta(offset, delta);
	}

	public unsafe static int RetrieveXiangshuInfection(byte* pModificationData)
	{
		return ReadDeltaInt(pModificationData);
	}

	public void RecordCurrAge(short oriValue, short currValue)
	{
		int offset = BeginAddingRecord(RevertibleCharacterPropertyType.CurrAge, 4);
		WriteDelta(offset, oriValue);
	}

	public unsafe static int RetrieveCurrAge(byte* pModificationData)
	{
		return ReadDeltaInt(pModificationData);
	}

	public void RecordHealth(short oriValue, short currValue)
	{
		int offset = BeginAddingRecord(RevertibleCharacterPropertyType.Health, 4);
		WriteDelta(offset, oriValue);
	}

	public unsafe static int RetrieveHealth(byte* pModificationData)
	{
		return ReadDeltaInt(pModificationData);
	}

	public void CreateSavepoint()
	{
		if (Savepoint >= 0)
		{
			throw new Exception("Cannot create savepoint when there is already a savepoint");
		}
		Savepoint = Size;
	}

	public void DeleteSavepoint()
	{
		if (Savepoint < 0)
		{
			throw new Exception("Cannot delete savepoint when there is no savepoint");
		}
		Savepoint = -1;
	}

	public unsafe (RevertibleCharacterPropertyType propertyType, int offset, ushort dataSize) Pop(byte* pRawData)
	{
		byte* ptr = pRawData + Size;
		ushort num = *((ushort*)ptr - 1);
		Size -= 1 + num + 2;
		if (Size < 0)
		{
			throw new Exception("Index of RawData out of range");
		}
		return (propertyType: (RevertibleCharacterPropertyType)pRawData[Size], offset: Size + 1, dataSize: num);
	}

	private unsafe int BeginAddingRecord(RevertibleCharacterPropertyType propertyType, ushort dataSize)
	{
		int size = Size;
		int num = Size + 1 + dataSize + 2;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			byte* ptr = rawData + size;
			*(int*)ptr = (int)propertyType;
			*(ushort*)(ptr + 1 + (int)dataSize) = dataSize;
		}
		return size + 1;
	}

	private unsafe void WriteDelta(int offset, int delta)
	{
		fixed (byte* rawData = RawData)
		{
			byte* ptr = rawData + offset;
			*(int*)ptr = delta;
		}
	}

	private unsafe static int ReadDeltaInt(byte* pModificationData)
	{
		return *(int*)pModificationData;
	}

	private static List<ShortListModification> CalcDelta(List<short> srcElements, List<short> destElements)
	{
		List<ShortListModification> list = new List<ShortListModification>();
		int num = 0;
		int num2 = 0;
		int count = srcElements.Count;
		int count2 = destElements.Count;
		short num3 = 0;
		while (num < count && num2 < count2)
		{
			short num4 = srcElements[num];
			int num5 = destElements.IndexOf(num4, num2);
			if (num5 == num2)
			{
				num++;
				num2++;
				num3++;
				continue;
			}
			if (num5 < 0)
			{
				list.Add(new ShortListModification(1, num3, num4));
				num++;
				continue;
			}
			for (int i = num2; i < num5; i++)
			{
				list.Add(new ShortListModification(0, num3++, destElements[i]));
			}
			num++;
			num2 = num5 + 1;
			num3++;
		}
		if (num < count)
		{
			for (int j = num; j < count; j++)
			{
				list.Add(new ShortListModification(1, num3, srcElements[j]));
			}
		}
		else if (num2 < count2)
		{
			for (int k = num2; k < count2; k++)
			{
				list.Add(new ShortListModification(0, num3++, destElements[k]));
			}
		}
		return list;
	}

	private static ushort CalcDataSize(List<ShortListModification> modifications)
	{
		int num = ShortListModification.GetFixedSerializedSize() * modifications.Count;
		if (num > 65535)
		{
			throw new Exception("Data size of modifications must be less than 64KB");
		}
		return (ushort)num;
	}

	private unsafe void WriteDelta(int offset, List<ShortListModification> modifications)
	{
		fixed (byte* rawData = RawData)
		{
			byte* ptr = rawData + offset;
			int i = 0;
			for (int count = modifications.Count; i < count; i++)
			{
				ptr += modifications[i].Serialize(ptr);
			}
		}
	}

	private unsafe static List<ShortListModification> ReadDeltaShortList(byte* pModificationData, ushort dataSize)
	{
		int num = dataSize / ShortListModification.GetFixedSerializedSize();
		Tester.Assert(ShortListModification.GetFixedSerializedSize() * num == dataSize);
		List<ShortListModification> list = new List<ShortListModification>();
		byte* ptr = pModificationData;
		for (int i = 0; i < num; i++)
		{
			ShortListModification item = default(ShortListModification);
			ptr += item.Deserialize(ptr);
			list.Add(item);
		}
		return list;
	}

	private unsafe void WriteDelta(int offset, MainAttributes delta)
	{
		fixed (byte* rawData = RawData)
		{
			byte* pData = rawData + offset;
			delta.Serialize(pData);
		}
	}

	private unsafe static MainAttributes ReadDeltaMainAttributes(byte* pModificationData)
	{
		MainAttributes result = default(MainAttributes);
		result.Deserialize(pModificationData);
		return result;
	}

	private unsafe void WriteDelta(int offset, Injuries delta)
	{
		fixed (byte* rawData = RawData)
		{
			byte* pData = rawData + offset;
			delta.Serialize(pData);
		}
	}

	private unsafe static Injuries ReadDeltaInjuries(byte* pModificationData)
	{
		Injuries result = default(Injuries);
		result.Deserialize(pModificationData);
		return result;
	}

	private unsafe void WriteDelta(int offset, ref LifeSkillShorts delta)
	{
		fixed (byte* rawData = RawData)
		{
			byte* pData = rawData + offset;
			delta.Serialize(pData);
		}
	}

	private unsafe static LifeSkillShorts ReadDeltaLifeSkillShorts(byte* pModificationData)
	{
		LifeSkillShorts result = default(LifeSkillShorts);
		result.Deserialize(pModificationData);
		return result;
	}

	private unsafe void WriteDelta(int offset, ref CombatSkillShorts delta)
	{
		fixed (byte* rawData = RawData)
		{
			byte* pData = rawData + offset;
			delta.Serialize(pData);
		}
	}

	private unsafe static CombatSkillShorts ReadDeltaCombatSkillShorts(byte* pModificationData)
	{
		CombatSkillShorts result = default(CombatSkillShorts);
		result.Deserialize(pModificationData);
		return result;
	}

	private unsafe void WriteDelta(int offset, ref ResourceInts delta)
	{
		fixed (byte* rawData = RawData)
		{
			byte* pData = rawData + offset;
			delta.Serialize(pData);
		}
	}

	private unsafe static ResourceInts ReadDeltaResourceInts(byte* pModificationData)
	{
		ResourceInts result = default(ResourceInts);
		result.Deserialize(pModificationData);
		return result;
	}

	private unsafe void WriteDelta(int offset, ref PoisonInts delta)
	{
		fixed (byte* rawData = RawData)
		{
			byte* pData = rawData + offset;
			delta.Serialize(pData);
		}
	}

	private unsafe static PoisonInts ReadDeltaPoisonInts(byte* pModificationData)
	{
		PoisonInts result = default(PoisonInts);
		result.Deserialize(pModificationData);
		return result;
	}

	private unsafe void WriteDelta(int offset, NeiliAllocation delta)
	{
		fixed (byte* rawData = RawData)
		{
			byte* pData = rawData + offset;
			delta.Serialize(pData);
		}
	}

	private unsafe static NeiliAllocation ReadDeltaNeiliAllocation(byte* pModificationData)
	{
		NeiliAllocation result = default(NeiliAllocation);
		result.Deserialize(pModificationData);
		return result;
	}
}
