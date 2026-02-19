using System;

namespace Config;

[Serializable]
public abstract class AdventureBranch
{
	public short Length;

	public string GlobalEvent;

	public (byte, short)[] SkillWeights;

	public (byte, short, short[])[] TerrainPersonalityWeights;

	public AdventurePersonalityContentWeights[] PersonalityContentWeights;

	protected AdventureBranch(short len, string globalEvent, int[] skillCfg, int[] terrainPersonalityCfg, int[] emptyBlockCfg, string[] eventCfg, int[] resRewardCfg, int[] itemRewardCfg, string[] bonusRewardCfg)
	{
		Length = len;
		GlobalEvent = globalEvent;
		SkillWeights = new(byte, short)[skillCfg.Length / 2];
		for (int i = 0; i < skillCfg.Length; i += 2)
		{
			SkillWeights[i / 2] = ((byte)skillCfg[i], (short)skillCfg[i + 1]);
		}
		TerrainPersonalityWeights = new(byte, short, short[])[terrainPersonalityCfg[0]];
		int num = 2;
		for (int j = 0; j < TerrainPersonalityWeights.Length; j++)
		{
			int num2 = terrainPersonalityCfg[num - 1];
			short[] array = null;
			if (num2 == 7)
			{
				array = new short[5];
				for (int k = 0; k < array.Length; k++)
				{
					array[k] = (short)terrainPersonalityCfg[num + k + 2];
				}
			}
			TerrainPersonalityWeights[j] = ((byte)terrainPersonalityCfg[num], (short)terrainPersonalityCfg[num + 1], array);
			num += num2 + 1;
		}
		PersonalityContentWeights = new AdventurePersonalityContentWeights[5];
		int num3 = 1;
		int num4 = 1;
		int num5 = 1;
		int num6 = 1;
		for (int l = 0; l < PersonalityContentWeights.Length; l++)
		{
			(string, short)[] array2 = new(string, short)[int.Parse(eventCfg[num3 - 1])];
			(byte, short, short)[] array3 = new(byte, short, short)[resRewardCfg[num4 - 1]];
			(byte, short, short, short)[] array4 = new(byte, short, short, short)[itemRewardCfg[num5 - 1]];
			(string, short)[] array5 = new(string, short)[int.Parse(bonusRewardCfg[num6 - 1])];
			for (int m = 0; m < array2.Length; m++)
			{
				num = num3 + m * 2;
				array2[m] = (eventCfg[num], short.Parse(eventCfg[num + 1]));
			}
			for (int n = 0; n < array3.Length; n++)
			{
				num = num4 + n * 3;
				array3[n] = ((byte)resRewardCfg[num], (short)resRewardCfg[num + 1], (short)resRewardCfg[num + 2]);
			}
			for (int num7 = 0; num7 < array4.Length; num7++)
			{
				num = num5 + num7 * 4;
				array4[num7] = ((byte)itemRewardCfg[num], (short)itemRewardCfg[num + 1], (short)itemRewardCfg[num + 2], (short)itemRewardCfg[num + 3]);
			}
			for (int num8 = 0; num8 < array5.Length; num8++)
			{
				num = num6 + num8 * 2;
				array5[num8] = (bonusRewardCfg[num], short.Parse(bonusRewardCfg[num + 1]));
			}
			num3 += array2.Length * 2 + 1;
			num4 += array3.Length * 3 + 1;
			num5 += array4.Length * 4 + 1;
			num6 += array5.Length * 2 + 1;
			PersonalityContentWeights[l] = new AdventurePersonalityContentWeights((short)emptyBlockCfg[l], array2, array3, array4, array5);
		}
	}

	protected AdventureBranch()
	{
	}
}
