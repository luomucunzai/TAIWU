using System;
using System.Runtime.CompilerServices;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Character;

public struct PreexistenceCharIds : ISerializableGameData
{
	public const int MaxCount = 9;

	public unsafe fixed int CharIds[9];

	public int Count;

	public unsafe fixed sbyte Positions[9];

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		return 52;
	}

	public unsafe int Serialize(byte* pData)
	{
		for (int i = 0; i < 9; i++)
		{
			((int*)pData)[i] = CharIds[i];
		}
		pData += 36;
		*(int*)pData = Count;
		pData += 4;
		for (int j = 0; j < 9; j++)
		{
			pData[j] = (byte)Positions[j];
		}
		return 52;
	}

	public unsafe int Deserialize(byte* pData)
	{
		for (int i = 0; i < 9; i++)
		{
			CharIds[i] = ((int*)pData)[i];
		}
		pData += 36;
		Count = *(int*)pData;
		pData += 4;
		for (int j = 0; j < 9; j++)
		{
			Positions[j] = (sbyte)pData[j];
		}
		return 52;
	}

	public unsafe void Add(IRandomSource random, int charId)
	{
		if (Count < 9)
		{
			if (Count == 0)
			{
				GeneratePositions(random);
			}
			CharIds[Count++] = charId;
			return;
		}
		throw new Exception("Exceeded the max count of preexistence characters");
	}

	public void Reset()
	{
		Count = 0;
	}

	public unsafe int IndexOf(int charId)
	{
		for (int i = 0; i < Count; i++)
		{
			if (CharIds[i] == charId)
			{
				return i;
			}
		}
		return -1;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private unsafe void GeneratePositions(IRandomSource random)
	{
		for (sbyte b = 0; b < 9; b++)
		{
			Positions[b] = b;
		}
		fixed (sbyte* positions = Positions)
		{
			CollectionUtils.Shuffle(random, positions, 9);
		}
	}
}
