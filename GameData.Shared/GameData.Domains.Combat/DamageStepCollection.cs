using System;
using GameData.Serializer;

namespace GameData.Domains.Combat;

[Serializable]
public class DamageStepCollection : ISerializableGameData
{
	public int[] OuterDamageSteps = new int[7];

	public int[] InnerDamageSteps = new int[7];

	public int FatalDamageStep;

	public int MindDamageStep;

	public DamageStepCollection()
	{
	}

	public DamageStepCollection(DamageStepCollection other)
	{
		for (sbyte b = 0; b < 7; b++)
		{
			OuterDamageSteps[b] = other.OuterDamageSteps[b];
			InnerDamageSteps[b] = other.InnerDamageSteps[b];
		}
		FatalDamageStep = other.FatalDamageStep;
		MindDamageStep = other.MindDamageStep;
	}

	public DamageStepCollection(params int[] stepValues)
	{
		for (int i = 0; i < 7; i++)
		{
			OuterDamageSteps[i] = stepValues[i];
			InnerDamageSteps[i] = stepValues[7 + i];
		}
		FatalDamageStep = stepValues[14];
		MindDamageStep = stepValues[15];
	}

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
		byte* ptr = pData;
		for (int i = 0; i < 7; i++)
		{
			*(int*)ptr = OuterDamageSteps[i];
			ptr += 4;
		}
		for (int j = 0; j < 7; j++)
		{
			*(int*)ptr = InnerDamageSteps[j];
			ptr += 4;
		}
		*(int*)ptr = FatalDamageStep;
		ptr += 4;
		*(int*)ptr = MindDamageStep;
		ptr += 4;
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
		for (int i = 0; i < 7; i++)
		{
			OuterDamageSteps[i] = *(int*)ptr;
			ptr += 4;
		}
		for (int j = 0; j < 7; j++)
		{
			InnerDamageSteps[j] = *(int*)ptr;
			ptr += 4;
		}
		FatalDamageStep = *(int*)ptr;
		ptr += 4;
		MindDamageStep = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
