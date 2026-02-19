using System;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Character;

public struct Genome : ISerializableGameData
{
	private const int GenesCount = 256;

	private const int GenesBytes = 64;

	public unsafe fixed byte Genes[64];

	private const int SegmentsCount = 16;

	private const int AlleleFrequencyQ = 128;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		return 64;
	}

	public unsafe int Serialize(byte* pData)
	{
		fixed (byte* genes = Genes)
		{
			for (int i = 0; i < 8; i++)
			{
				((long*)pData)[i] = ((long*)genes)[i];
			}
		}
		return 64;
	}

	public unsafe int Deserialize(byte* pData)
	{
		fixed (byte* genes = Genes)
		{
			for (int i = 0; i < 8; i++)
			{
				((long*)genes)[i] = ((long*)pData)[i];
			}
		}
		return 64;
	}

	public unsafe static void CreateRandom(IRandomSource randomSource, ref Genome genome)
	{
		byte* ptr = stackalloc byte[512];
		randomSource.NextBytes(new Span<byte>(ptr, 512));
		for (int i = 0; i < 64; i++)
		{
			uint num = 0u;
			for (int j = 0; j < 8; j++)
			{
				uint num2 = ((ptr[i * 8 + j] % 128 != 0) ? 1u : 0u);
				num |= num2 << j;
			}
			genome.Genes[i] = (byte)num;
		}
	}

	public unsafe static void EraseAffectedRecessiveTraits(ref Genome genome)
	{
		fixed (byte* genes = genome.Genes)
		{
			for (int i = 0; i < 4; i++)
			{
				((long*)genes)[i] = -1L;
			}
		}
	}

	public unsafe static void Inherit(IRandomSource randomSource, ref Genome female, ref Genome male, ref Genome offspring)
	{
		uint num = randomSource.NextUInt();
		fixed (byte* genes = female.Genes)
		{
			fixed (byte* genes2 = male.Genes)
			{
				fixed (byte* genes3 = offspring.Genes)
				{
					for (int i = 0; i < 16; i++)
					{
						uint num2 = (num >> i * 2) & 1;
						uint num3 = (num >> i * 2 + 1) & 1;
						ushort num4 = ((num2 == 0) ? ((ushort*)genes)[i] : ((ushort*)(genes + 32))[i]);
						ushort num5 = ((num3 == 0) ? ((ushort*)genes2)[i] : ((ushort*)(genes2 + 32))[i]);
						((short*)genes3)[i] = (short)num4;
						((short*)(genes3 + 32))[i] = (short)num5;
					}
				}
			}
		}
	}
}
