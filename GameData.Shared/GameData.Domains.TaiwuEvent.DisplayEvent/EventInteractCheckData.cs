using System;
using System.Collections.Generic;
using GameData.Domains.Character;
using GameData.Domains.Character.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.DisplayEvent;

[Serializable]
[SerializableGameData(NoCopyConstructors = true)]
public class EventInteractCheckData : ISerializableGameData
{
	[SerializableGameDataField]
	public short InteractCheckTemplateId;

	[SerializableGameDataField]
	public List<int> PhaseProbList;

	[SerializableGameDataField]
	public sbyte FailPhase;

	[SerializableGameDataField]
	public bool IsEscape;

	[SerializableGameDataField]
	public NameRelatedData SelfNameRelatedData;

	[SerializableGameDataField]
	public Dictionary<int, int> ConfessionLovePureFixProbDict;

	[SerializableGameDataField]
	public Dictionary<int, int> ConfessionLoveSecularFixProbDict;

	[SerializableGameDataField]
	public bool CombatPowerHigher;

	[SerializableGameDataField]
	public MainAttributes SelfMainAttributes;

	[SerializableGameDataField]
	public CombatSkillShorts SelfCombatSkillAttainments;

	[SerializableGameDataField]
	public LifeSkillShorts SelfLifeSkillAttainments;

	[SerializableGameDataField]
	public HitOrAvoidInts SelfHitValues;

	[SerializableGameDataField]
	public HitOrAvoidInts SelfAvoidValues;

	[SerializableGameDataField]
	public OuterAndInnerInts SelfPenetrations;

	[SerializableGameDataField]
	public OuterAndInnerInts SelfPenetrationResists;

	[SerializableGameDataField]
	public short SelfCastSpeed;

	[SerializableGameDataField]
	public short SelfAttackSpeed;

	[SerializableGameDataField]
	public short SelfMoveSpeed;

	[SerializableGameDataField]
	public NameRelatedData TargetNameRelatedData;

	[SerializableGameDataField]
	public CombatSkillShorts TargetCombatSkillAttainments;

	[SerializableGameDataField]
	public LifeSkillShorts TargetLifeSkillAttainments;

	[SerializableGameDataField]
	public HitOrAvoidInts TargetHitValues;

	[SerializableGameDataField]
	public HitOrAvoidInts TargetAvoidValues;

	[SerializableGameDataField]
	public OuterAndInnerInts TargetPenetrations;

	[SerializableGameDataField]
	public OuterAndInnerInts TargetPenetrationResists;

	[SerializableGameDataField]
	public short TargetCastSpeed;

	[SerializableGameDataField]
	public short TargetAttackSpeed;

	[SerializableGameDataField]
	public short TargetMoveSpeed;

	[SerializableGameDataField]
	public int TargetAlertFactor;

	[SerializableGameDataField]
	public LifeSkillShorts SelfLifeSkillQualities;

	[SerializableGameDataField]
	public CombatSkillShorts SelfCombatSkillQualities;

	[SerializableGameDataField]
	public sbyte StealSkillGrade;

	[SerializableGameDataField]
	public sbyte StealLifeSkillType;

	[SerializableGameDataField]
	public sbyte StealCombatSkillType;

	public EventInteractCheckData(short interactCheckTemplateId)
	{
		InteractCheckTemplateId = interactCheckTemplateId;
		PhaseProbList = new List<int>();
		FailPhase = 5;
	}

	public EventInteractCheckData()
	{
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 376;
		num = ((PhaseProbList == null) ? (num + 2) : (num + (2 + 4 * PhaseProbList.Count)));
		num += DictionaryOfBasicTypePair.GetSerializedSize<int, int>((IReadOnlyDictionary<int, int>)ConfessionLovePureFixProbDict);
		num += DictionaryOfBasicTypePair.GetSerializedSize<int, int>((IReadOnlyDictionary<int, int>)ConfessionLoveSecularFixProbDict);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = InteractCheckTemplateId;
		ptr += 2;
		if (PhaseProbList != null)
		{
			int count = PhaseProbList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((int*)ptr)[i] = PhaseProbList[i];
			}
			ptr += 4 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (byte)FailPhase;
		ptr++;
		*ptr = (IsEscape ? ((byte)1) : ((byte)0));
		ptr++;
		ptr += SelfNameRelatedData.Serialize(ptr);
		ptr += DictionaryOfBasicTypePair.Serialize<int, int>(ptr, ref ConfessionLovePureFixProbDict);
		ptr += DictionaryOfBasicTypePair.Serialize<int, int>(ptr, ref ConfessionLoveSecularFixProbDict);
		*ptr = (CombatPowerHigher ? ((byte)1) : ((byte)0));
		ptr++;
		ptr += SelfMainAttributes.Serialize(ptr);
		ptr += SelfCombatSkillAttainments.Serialize(ptr);
		ptr += SelfLifeSkillAttainments.Serialize(ptr);
		ptr += SelfHitValues.Serialize(ptr);
		ptr += SelfAvoidValues.Serialize(ptr);
		ptr += SelfPenetrations.Serialize(ptr);
		ptr += SelfPenetrationResists.Serialize(ptr);
		*(short*)ptr = SelfCastSpeed;
		ptr += 2;
		*(short*)ptr = SelfAttackSpeed;
		ptr += 2;
		*(short*)ptr = SelfMoveSpeed;
		ptr += 2;
		ptr += TargetNameRelatedData.Serialize(ptr);
		ptr += TargetCombatSkillAttainments.Serialize(ptr);
		ptr += TargetLifeSkillAttainments.Serialize(ptr);
		ptr += TargetHitValues.Serialize(ptr);
		ptr += TargetAvoidValues.Serialize(ptr);
		ptr += TargetPenetrations.Serialize(ptr);
		ptr += TargetPenetrationResists.Serialize(ptr);
		*(short*)ptr = TargetCastSpeed;
		ptr += 2;
		*(short*)ptr = TargetAttackSpeed;
		ptr += 2;
		*(short*)ptr = TargetMoveSpeed;
		ptr += 2;
		*(int*)ptr = TargetAlertFactor;
		ptr += 4;
		ptr += SelfLifeSkillQualities.Serialize(ptr);
		ptr += SelfCombatSkillQualities.Serialize(ptr);
		*ptr = (byte)StealSkillGrade;
		ptr++;
		*ptr = (byte)StealLifeSkillType;
		ptr++;
		*ptr = (byte)StealCombatSkillType;
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
		InteractCheckTemplateId = *(short*)ptr;
		ptr += 2;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (PhaseProbList == null)
			{
				PhaseProbList = new List<int>(num);
			}
			else
			{
				PhaseProbList.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				PhaseProbList.Add(((int*)ptr)[i]);
			}
			ptr += 4 * num;
		}
		else
		{
			PhaseProbList?.Clear();
		}
		FailPhase = (sbyte)(*ptr);
		ptr++;
		IsEscape = *ptr != 0;
		ptr++;
		ptr += SelfNameRelatedData.Deserialize(ptr);
		ptr += DictionaryOfBasicTypePair.Deserialize<int, int>(ptr, ref ConfessionLovePureFixProbDict);
		ptr += DictionaryOfBasicTypePair.Deserialize<int, int>(ptr, ref ConfessionLoveSecularFixProbDict);
		CombatPowerHigher = *ptr != 0;
		ptr++;
		ptr += SelfMainAttributes.Deserialize(ptr);
		ptr += SelfCombatSkillAttainments.Deserialize(ptr);
		ptr += SelfLifeSkillAttainments.Deserialize(ptr);
		ptr += SelfHitValues.Deserialize(ptr);
		ptr += SelfAvoidValues.Deserialize(ptr);
		ptr += SelfPenetrations.Deserialize(ptr);
		ptr += SelfPenetrationResists.Deserialize(ptr);
		SelfCastSpeed = *(short*)ptr;
		ptr += 2;
		SelfAttackSpeed = *(short*)ptr;
		ptr += 2;
		SelfMoveSpeed = *(short*)ptr;
		ptr += 2;
		ptr += TargetNameRelatedData.Deserialize(ptr);
		ptr += TargetCombatSkillAttainments.Deserialize(ptr);
		ptr += TargetLifeSkillAttainments.Deserialize(ptr);
		ptr += TargetHitValues.Deserialize(ptr);
		ptr += TargetAvoidValues.Deserialize(ptr);
		ptr += TargetPenetrations.Deserialize(ptr);
		ptr += TargetPenetrationResists.Deserialize(ptr);
		TargetCastSpeed = *(short*)ptr;
		ptr += 2;
		TargetAttackSpeed = *(short*)ptr;
		ptr += 2;
		TargetMoveSpeed = *(short*)ptr;
		ptr += 2;
		TargetAlertFactor = *(int*)ptr;
		ptr += 4;
		ptr += SelfLifeSkillQualities.Deserialize(ptr);
		ptr += SelfCombatSkillQualities.Deserialize(ptr);
		StealSkillGrade = (sbyte)(*ptr);
		ptr++;
		StealLifeSkillType = (sbyte)(*ptr);
		ptr++;
		StealCombatSkillType = (sbyte)(*ptr);
		ptr++;
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
