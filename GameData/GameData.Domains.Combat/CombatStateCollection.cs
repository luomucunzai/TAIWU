using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Domains.Combat;

public class CombatStateCollection : ISerializableGameData
{
	public readonly Dictionary<short, (short power, bool reverse, int srcCharId)> StateDict = new Dictionary<short, (short, bool, int)>();

	public readonly Dictionary<short, long> State2EffectId = new Dictionary<short, long>();

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		return 2 + 9 * StateDict.Count;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = (short)StateDict.Count;
		ptr += 2;
		foreach (KeyValuePair<short, (short, bool, int)> item in StateDict)
		{
			*(short*)ptr = item.Key;
			ptr += 2;
			*(short*)ptr = item.Value.Item1;
			ptr += 2;
			*ptr = (item.Value.Item2 ? ((byte)1) : ((byte)0));
			ptr++;
			*(int*)ptr = item.Value.Item3;
			ptr += 4;
		}
		return (int)(ptr - pData);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		short num = *(short*)ptr;
		ptr += 2;
		StateDict.Clear();
		for (int i = 0; i < num; i++)
		{
			short key = *(short*)ptr;
			ptr += 2;
			short item = *(short*)ptr;
			ptr += 2;
			bool item2 = *ptr != 0;
			ptr++;
			int item3 = *(int*)ptr;
			ptr += 4;
			StateDict.Add(key, (item, item2, item3));
		}
		return (int)(ptr - pData);
	}
}
