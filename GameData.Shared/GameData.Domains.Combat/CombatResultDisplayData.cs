using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Combat.Math;
using GameData.Domains.Character;
using GameData.Domains.Character.Display;
using GameData.Domains.Item;
using GameData.Domains.Item.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat;

[SerializableGameData(NotForArchive = true, NoCopyConstructors = true)]
public class CombatResultDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public sbyte CombatStatus;

	[SerializableGameDataField]
	public CombatResultSnapshot SnapshotBeforeCombat;

	[SerializableGameDataField]
	public CombatResultSnapshot SnapshotAfterCombat;

	[SerializableGameDataField]
	public int Exp;

	[SerializableGameDataField]
	public ResourceInts Resource;

	[SerializableGameDataField]
	public int AreaSpiritualDebt;

	[SerializableGameDataField]
	public bool ShowReadingEvent;

	[SerializableGameDataField]
	public bool ShowLoopingEvent;

	[SerializableGameDataField]
	public List<sbyte> EvaluationList = new List<sbyte>();

	[SerializableGameDataField]
	public List<ItemDisplayData> ItemList = new List<ItemDisplayData>();

	[SerializableGameDataField]
	public List<CharacterDisplayData> CharList;

	[SerializableGameDataField]
	public List<short> LegacyTemplateIds;

	[SerializableGameDataField]
	public Dictionary<short, int> ChangedProficiencies;

	[SerializableGameDataField]
	public Dictionary<short, int> ChangedProficienciesDelta;

	public Dictionary<ItemKey, int> ItemSrcCharDict = new Dictionary<ItemKey, int>();

	public bool IsWin
	{
		get
		{
			if (CombatStatus != CombatStatusType.EnemyFail)
			{
				return CombatStatus == CombatStatusType.EnemyFlee;
			}
			return true;
		}
	}

	public IEnumerable<CombatEvaluationItem> Evaluations => EvaluationList.Select(ParseEvaluation);

	public IEnumerable<T> SelectEvaluations<T>(Func<CombatEvaluationItem, T> selector)
	{
		return Evaluations.Select(selector);
	}

	private static CombatEvaluationItem ParseEvaluation(sbyte evaluationTemplateId)
	{
		return CombatEvaluation.Instance[evaluationTemplateId];
	}

	public int ModifyValue(int baseValue, Func<CombatEvaluationItem, int> selectorB, Func<CombatEvaluationItem, int> selectorC, int extraAddPercent = 0)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		CValuePercentBonus val = CValuePercentBonus.op_Implicit(Math.Max(SelectEvaluations(selectorB).Sum() + extraAddPercent, -100));
		int num = 0;
		int num2 = 0;
		foreach (int item in SelectEvaluations(selectorC))
		{
			if (num < item)
			{
				num = item;
			}
			if (num2 > item)
			{
				num2 = item;
			}
		}
		CValuePercentBonus val2 = CValuePercentBonus.op_Implicit(Math.Max(num + num2, -100));
		return baseValue * val * val2;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 499;
		num = ((EvaluationList == null) ? (num + 2) : (num + (2 + EvaluationList.Count)));
		if (ItemList != null)
		{
			num += 2;
			int count = ItemList.Count;
			for (int i = 0; i < count; i++)
			{
				ItemDisplayData itemDisplayData = ItemList[i];
				num = ((itemDisplayData == null) ? (num + 2) : (num + (2 + itemDisplayData.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		if (CharList != null)
		{
			num += 2;
			int count2 = CharList.Count;
			for (int j = 0; j < count2; j++)
			{
				CharacterDisplayData characterDisplayData = CharList[j];
				num = ((characterDisplayData == null) ? (num + 2) : (num + (2 + characterDisplayData.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		num = ((LegacyTemplateIds == null) ? (num + 2) : (num + (2 + 2 * LegacyTemplateIds.Count)));
		num += DictionaryOfBasicTypePair.GetSerializedSize<short, int>((IReadOnlyDictionary<short, int>)ChangedProficiencies);
		num += DictionaryOfBasicTypePair.GetSerializedSize<short, int>((IReadOnlyDictionary<short, int>)ChangedProficienciesDelta);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (byte)CombatStatus;
		ptr++;
		ptr += SnapshotBeforeCombat.Serialize(ptr);
		ptr += SnapshotAfterCombat.Serialize(ptr);
		*(int*)ptr = Exp;
		ptr += 4;
		ptr += Resource.Serialize(ptr);
		*(int*)ptr = AreaSpiritualDebt;
		ptr += 4;
		*ptr = (ShowReadingEvent ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (ShowLoopingEvent ? ((byte)1) : ((byte)0));
		ptr++;
		if (EvaluationList != null)
		{
			int count = EvaluationList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr[i] = (byte)EvaluationList[i];
			}
			ptr += count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (ItemList != null)
		{
			int count2 = ItemList.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				ItemDisplayData itemDisplayData = ItemList[j];
				if (itemDisplayData != null)
				{
					byte* intPtr = ptr;
					ptr += 2;
					int num = itemDisplayData.Serialize(ptr);
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
		if (CharList != null)
		{
			int count3 = CharList.Count;
			Tester.Assert(count3 <= 65535);
			*(ushort*)ptr = (ushort)count3;
			ptr += 2;
			for (int k = 0; k < count3; k++)
			{
				CharacterDisplayData characterDisplayData = CharList[k];
				if (characterDisplayData != null)
				{
					byte* intPtr2 = ptr;
					ptr += 2;
					int num2 = characterDisplayData.Serialize(ptr);
					ptr += num2;
					Tester.Assert(num2 <= 65535);
					*(ushort*)intPtr2 = (ushort)num2;
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
		if (LegacyTemplateIds != null)
		{
			int count4 = LegacyTemplateIds.Count;
			Tester.Assert(count4 <= 65535);
			*(ushort*)ptr = (ushort)count4;
			ptr += 2;
			for (int l = 0; l < count4; l++)
			{
				((short*)ptr)[l] = LegacyTemplateIds[l];
			}
			ptr += 2 * count4;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += DictionaryOfBasicTypePair.Serialize<short, int>(ptr, ref ChangedProficiencies);
		ptr += DictionaryOfBasicTypePair.Serialize<short, int>(ptr, ref ChangedProficienciesDelta);
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		CombatStatus = (sbyte)(*ptr);
		ptr++;
		ptr += SnapshotBeforeCombat.Deserialize(ptr);
		ptr += SnapshotAfterCombat.Deserialize(ptr);
		Exp = *(int*)ptr;
		ptr += 4;
		ptr += Resource.Deserialize(ptr);
		AreaSpiritualDebt = *(int*)ptr;
		ptr += 4;
		ShowReadingEvent = *ptr != 0;
		ptr++;
		ShowLoopingEvent = *ptr != 0;
		ptr++;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (EvaluationList == null)
			{
				EvaluationList = new List<sbyte>(num);
			}
			else
			{
				EvaluationList.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				EvaluationList.Add((sbyte)ptr[i]);
			}
			ptr += (int)num;
		}
		else
		{
			EvaluationList?.Clear();
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (ItemList == null)
			{
				ItemList = new List<ItemDisplayData>(num2);
			}
			else
			{
				ItemList.Clear();
			}
			for (int j = 0; j < num2; j++)
			{
				ushort num3 = *(ushort*)ptr;
				ptr += 2;
				if (num3 > 0)
				{
					ItemDisplayData itemDisplayData = new ItemDisplayData();
					ptr += itemDisplayData.Deserialize(ptr);
					ItemList.Add(itemDisplayData);
				}
				else
				{
					ItemList.Add(null);
				}
			}
		}
		else
		{
			ItemList?.Clear();
		}
		ushort num4 = *(ushort*)ptr;
		ptr += 2;
		if (num4 > 0)
		{
			if (CharList == null)
			{
				CharList = new List<CharacterDisplayData>(num4);
			}
			else
			{
				CharList.Clear();
			}
			for (int k = 0; k < num4; k++)
			{
				ushort num5 = *(ushort*)ptr;
				ptr += 2;
				if (num5 > 0)
				{
					CharacterDisplayData characterDisplayData = new CharacterDisplayData();
					ptr += characterDisplayData.Deserialize(ptr);
					CharList.Add(characterDisplayData);
				}
				else
				{
					CharList.Add(null);
				}
			}
		}
		else
		{
			CharList?.Clear();
		}
		ushort num6 = *(ushort*)ptr;
		ptr += 2;
		if (num6 > 0)
		{
			if (LegacyTemplateIds == null)
			{
				LegacyTemplateIds = new List<short>(num6);
			}
			else
			{
				LegacyTemplateIds.Clear();
			}
			for (int l = 0; l < num6; l++)
			{
				LegacyTemplateIds.Add(((short*)ptr)[l]);
			}
			ptr += 2 * num6;
		}
		else
		{
			LegacyTemplateIds?.Clear();
		}
		ptr += DictionaryOfBasicTypePair.Deserialize<short, int>(ptr, ref ChangedProficiencies);
		ptr += DictionaryOfBasicTypePair.Deserialize<short, int>(ptr, ref ChangedProficienciesDelta);
		int num7 = (int)(ptr - pData);
		if (num7 > 4)
		{
			return (num7 + 3) / 4 * 4;
		}
		return num7;
	}
}
