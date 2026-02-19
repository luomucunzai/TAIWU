using System;

namespace Config;

[Serializable]
public class AdventurePersonalityContentWeights
{
	public short EmptyBlockWeight;

	public (string eventGuid, short weight)[] EventWeights;

	public (byte resId, short amount, short weight)[] NormalResWeights;

	public (byte itemType, short templateId, short amount, short weight)[] SpecialResWeights;

	public (string, short)[] BonusWeights;

	public readonly short[] ContentTypeWeights = new short[5];

	public AdventurePersonalityContentWeights(short emptyBlockWeight, (string, short)[] eventWeights, (byte, short, short)[] resWeights, (byte, short, short, short)[] itemsWeights, (string, short)[] bonusWeights)
	{
		EmptyBlockWeight = emptyBlockWeight;
		EventWeights = eventWeights;
		NormalResWeights = resWeights;
		SpecialResWeights = itemsWeights;
		BonusWeights = bonusWeights;
		ContentTypeWeights[0] = EmptyBlockWeight;
		(string, short)[] eventWeights2 = EventWeights;
		for (int i = 0; i < eventWeights2.Length; i++)
		{
			(string, short) tuple = eventWeights2[i];
			ContentTypeWeights[1] += tuple.Item2;
		}
		(byte, short, short)[] normalResWeights = NormalResWeights;
		for (int i = 0; i < normalResWeights.Length; i++)
		{
			(byte, short, short) tuple2 = normalResWeights[i];
			ContentTypeWeights[2] += tuple2.Item3;
		}
		(byte, short, short, short)[] specialResWeights = SpecialResWeights;
		for (int i = 0; i < specialResWeights.Length; i++)
		{
			(byte, short, short, short) tuple3 = specialResWeights[i];
			ContentTypeWeights[3] += tuple3.Item4;
		}
		(string, short)[] bonusWeights2 = BonusWeights;
		for (int i = 0; i < bonusWeights2.Length; i++)
		{
			(string, short) tuple4 = bonusWeights2[i];
			ContentTypeWeights[4] += tuple4.Item2;
		}
	}
}
