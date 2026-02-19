using System;
using System.Collections.Generic;
using Config;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat;

public class FlawOrAcupointCollection : ISerializableGameData
{
	public struct ReduceKeepTimeResult
	{
		public bool DataChanged;

		public bool CountChanged;

		public List<(sbyte part, sbyte level)> RemovedList;
	}

	public delegate int ReduceFrameDelegate(int totalFrame);

	public readonly SortedDictionary<sbyte, List<(sbyte level, int totalFrame, int leftFrame)>> BodyPartDict;

	private readonly List<(sbyte part, sbyte level)> _removedList = new List<(sbyte, sbyte)>();

	public FlawOrAcupointCollection()
	{
		BodyPartDict = new SortedDictionary<sbyte, List<(sbyte, int, int)>>();
		for (sbyte b = 0; b < 7; b++)
		{
			BodyPartDict.Add(b, new List<(sbyte, int, int)>());
		}
	}

	public int GetTotalCount()
	{
		int num = 0;
		foreach (List<(sbyte, int, int)> value in BodyPartDict.Values)
		{
			num += value.Count;
		}
		return num;
	}

	public ReduceKeepTimeResult ReduceKeepTime(CombatCharacter combatChar, int recoverSpeed, byte[] countArray, bool isFlaw)
	{
		recoverSpeed = CFormula.CalcFlawOrAcupointRecoverSpeed(recoverSpeed);
		ushort fieldId = (ushort)(isFlaw ? 316 : 300);
		recoverSpeed = DomainManager.SpecialEffect.ModifyValue(combatChar.GetId(), fieldId, recoverSpeed);
		recoverSpeed = Math.Max(recoverSpeed, 1);
		return ReduceKeepTimePercentInternal(combatChar, (int _) => recoverSpeed, countArray, isFlaw);
	}

	public ReduceKeepTimeResult ReduceKeepTimePercent(CombatCharacter combatChar, int reducePercent, byte[] countArray, bool isFlaw)
	{
		return ReduceKeepTimePercentInternal(combatChar, (int totalFrame) => totalFrame * reducePercent / 100, countArray, isFlaw);
	}

	private ReduceKeepTimeResult ReduceKeepTimePercentInternal(CombatCharacter combatChar, ReduceFrameDelegate reduceFrameDelegate, byte[] countArray, bool isFlaw)
	{
		bool dataChanged = false;
		bool countChanged = false;
		_removedList.Clear();
		foreach (KeyValuePair<sbyte, List<(sbyte, int, int)>> item in BodyPartDict)
		{
			item.Deconstruct(out var key, out var value);
			sbyte b = key;
			List<(sbyte, int, int)> list = value;
			for (int num = list.Count - 1; num >= 0; num--)
			{
				(sbyte, int, int) value2 = list[num];
				value2.Item3 -= reduceFrameDelegate(value2.Item2);
				value2.Item3 = Math.Min(value2.Item3, value2.Item2);
				if (value2.Item3 <= 0)
				{
					_removedList.Add((b, value2.Item1));
					list.RemoveAt(num);
					countArray[b]--;
					countChanged = true;
				}
				else
				{
					list[num] = value2;
				}
				dataChanged = true;
			}
		}
		return new ReduceKeepTimeResult
		{
			DataChanged = dataChanged,
			CountChanged = countChanged,
			RemovedList = _removedList
		};
	}

	public bool OfflineRecoverKeepTimePercent(int recoverPercent)
	{
		if (recoverPercent <= 0)
		{
			return false;
		}
		bool result = false;
		foreach (List<(sbyte, int, int)> value2 in BodyPartDict.Values)
		{
			for (int i = 0; i < value2.Count; i++)
			{
				result = true;
				(sbyte, int, int) value = value2[i];
				value.Item3 = Math.Clamp(value.Item3 + value.Item2 * recoverPercent / 100, 0, value.Item2);
				value2[i] = value;
			}
		}
		return result;
	}

	public int CalcAcupointParam(sbyte bodyPart)
	{
		if (!BodyPartDict.TryGetValue(bodyPart, out List<(sbyte, int, int)> value) || value.Count == 0)
		{
			return 0;
		}
		int num = 0;
		foreach (var item in value)
		{
			int val = item.Item3 * 100 / item.Item2;
			num = Math.Max(num, val);
		}
		if (num == 0)
		{
			return 0;
		}
		BodyPartItem bodyPartItem = BodyPart.Instance[bodyPart];
		Tester.Assert(bodyPartItem.AcupointParam.Length == bodyPartItem.AcupointTime.Length);
		for (int i = 0; i < bodyPartItem.AcupointTime.Length; i++)
		{
			int num2 = bodyPartItem.AcupointTime[i];
			if (num2 > num)
			{
				return (i > 0) ? bodyPartItem.AcupointParam[i - 1] : 0;
			}
		}
		return bodyPartItem.AcupointParam[^1];
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		for (sbyte b = 0; b < 7; b++)
		{
			num += 1 + 9 * BodyPartDict[b].Count;
		}
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		for (sbyte b = 0; b < 7; b++)
		{
			List<(sbyte, int, int)> list = BodyPartDict[b];
			*ptr = (byte)(sbyte)list.Count;
			ptr++;
			for (int i = 0; i < list.Count; i++)
			{
				(sbyte, int, int) tuple = list[i];
				*ptr = (byte)tuple.Item1;
				ptr++;
				*(int*)ptr = tuple.Item2;
				ptr += 4;
				*(int*)ptr = tuple.Item3;
				ptr += 4;
			}
		}
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		(sbyte, int, int) item = default((sbyte, int, int));
		for (sbyte b = 0; b < 7; b++)
		{
			List<(sbyte, int, int)> list = BodyPartDict[b];
			sbyte b2 = (sbyte)(*ptr);
			ptr++;
			list.Clear();
			for (int i = 0; i < b2; i++)
			{
				item.Item1 = (sbyte)(*ptr);
				ptr++;
				item.Item2 = *(int*)ptr;
				ptr += 4;
				item.Item3 = *(int*)ptr;
				ptr += 4;
				list.Add(item);
			}
		}
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}
}
