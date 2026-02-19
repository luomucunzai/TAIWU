using System;
using System.Collections.Generic;
using GameData.Domains.Character;
using GameData.Serializer;

namespace GameData.Domains.Combat;

public class InjuryAutoHealCollection : ISerializableGameData
{
	public readonly List<short>[] OuterBodyPartList;

	public readonly List<short>[] InnerBodyPartList;

	public InjuryAutoHealCollection()
	{
		OuterBodyPartList = new List<short>[7];
		InnerBodyPartList = new List<short>[7];
		for (sbyte b = 0; b < 7; b++)
		{
			OuterBodyPartList[b] = new List<short>();
			InnerBodyPartList[b] = new List<short>();
		}
	}

	public void SyncInjuries(ref Injuries injuries)
	{
		for (sbyte b = 0; b < 7; b++)
		{
			(sbyte outer, sbyte inner) tuple = injuries.Get(b);
			sbyte item = tuple.outer;
			sbyte item2 = tuple.inner;
			List<short> list = OuterBodyPartList[b];
			List<short> list2 = InnerBodyPartList[b];
			item = Math.Max(item, 0);
			item2 = Math.Max(item2, 0);
			while (item < list.Count)
			{
				list.RemoveAt(0);
			}
			while (item > list.Count)
			{
				list.Add(0);
			}
			while (item2 < list2.Count)
			{
				list2.RemoveAt(0);
			}
			while (item2 > list2.Count)
			{
				list2.Add(0);
			}
		}
	}

	public bool UpdateProgress(Dictionary<sbyte, OuterAndInnerInts> bodyPart2Deltas, int outerSpeed, int innerSpeed)
	{
		bodyPart2Deltas.Clear();
		if (outerSpeed <= 0 && innerSpeed <= 0)
		{
			return false;
		}
		for (sbyte b = 0; b < 7; b++)
		{
			int outer = UpdateProgress(isInner: false, b, outerSpeed);
			int inner = UpdateProgress(isInner: true, b, innerSpeed);
			bodyPart2Deltas[b] = new OuterAndInnerInts(outer, inner);
		}
		return true;
	}

	private int UpdateProgress(bool isInner, sbyte bodyPart, int speed)
	{
		if (speed <= 0)
		{
			return 0;
		}
		int num = 0;
		List<short> list = (isInner ? InnerBodyPartList : OuterBodyPartList)[bodyPart];
		for (int num2 = list.Count - 1; num2 >= 0; num2--)
		{
			list[num2] = (short)Math.Clamp(list[num2] + speed, 0, 900);
			if (list[num2] >= 900)
			{
				num++;
				list.RemoveAt(num2);
			}
		}
		return num;
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
			num += 1 + 2 * OuterBodyPartList[b].Count;
			num += 1 + 2 * InnerBodyPartList[b].Count;
		}
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		for (sbyte b = 0; b < 7; b++)
		{
			List<short> list = OuterBodyPartList[b];
			*ptr = (byte)(sbyte)list.Count;
			ptr++;
			for (int i = 0; i < list.Count; i++)
			{
				*(short*)ptr = list[i];
				ptr += 2;
			}
			list = InnerBodyPartList[b];
			*ptr = (byte)(sbyte)list.Count;
			ptr++;
			for (int j = 0; j < list.Count; j++)
			{
				*(short*)ptr = list[j];
				ptr += 2;
			}
		}
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		for (sbyte b = 0; b < 7; b++)
		{
			List<short> list = OuterBodyPartList[b];
			sbyte b2 = (sbyte)(*ptr);
			ptr++;
			list.Clear();
			for (int i = 0; i < b2; i++)
			{
				short item = *(short*)ptr;
				ptr += 2;
				list.Add(item);
			}
			list = InnerBodyPartList[b];
			b2 = (sbyte)(*ptr);
			ptr++;
			list.Clear();
			for (int j = 0; j < b2; j++)
			{
				short item2 = *(short*)ptr;
				ptr += 2;
				list.Add(item2);
			}
		}
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}
}
