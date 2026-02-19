using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.Character;

public static class NeiliProportionHelper
{
	public static sbyte GetNeiliType(this NeiliProportionOfFiveElements proportionOfFiveElements, sbyte birthMonth)
	{
		SpanList<(sbyte, sbyte)> spanList = stackalloc(sbyte, sbyte)[5];
		for (sbyte b = 0; b < 5; b++)
		{
			sbyte b2 = proportionOfFiveElements[b];
			for (sbyte b3 = 0; b3 < 5; b3++)
			{
				if (b3 == spanList.Count)
				{
					spanList.Add((b2, b));
					break;
				}
				if (spanList[b3].Item1 < b2)
				{
					spanList.Insert(b3, (b2, b));
					break;
				}
			}
		}
		if (spanList[0].Item1 <= 22 && spanList[4].Item1 >= 18)
		{
			return 5;
		}
		sbyte b4 = spanList[0].Item2;
		sbyte item = spanList[0].Item1;
		sbyte item2 = spanList[1].Item1;
		int num = 0;
		sbyte b5 = 1;
		while (b5 < 5 && spanList[b5].Item1 == spanList[0].Item1)
		{
			num++;
			b5++;
		}
		if (num > 0)
		{
			sbyte innateFiveElementsType = SharedMethods.GetInnateFiveElementsType(birthMonth);
			sbyte b6 = FiveElementsType.Countering[innateFiveElementsType];
			sbyte b7 = FiveElementsType.Countered[innateFiveElementsType];
			sbyte b8 = FiveElementsType.Producing[innateFiveElementsType];
			sbyte b9 = FiveElementsType.Produced[innateFiveElementsType];
			sbyte b10 = proportionOfFiveElements[innateFiveElementsType];
			sbyte b11 = proportionOfFiveElements[b6];
			sbyte b12 = proportionOfFiveElements[b7];
			sbyte b13 = proportionOfFiveElements[b8];
			sbyte b14 = proportionOfFiveElements[b9];
			if (item == b10)
			{
				b4 = innateFiveElementsType;
			}
			else if (item == b12)
			{
				b4 = b7;
			}
			else if (item == b11)
			{
				b4 = b6;
			}
			else if (item == b14)
			{
				b4 = b9;
			}
			else if (item == b13)
			{
				b4 = b8;
			}
		}
		sbyte index = FiveElementsType.Countering[b4];
		sbyte b15 = FiveElementsType.Countered[b4];
		sbyte index2 = FiveElementsType.Producing[b4];
		sbyte index3 = FiveElementsType.Produced[b4];
		sbyte b16 = proportionOfFiveElements[index];
		sbyte b17 = proportionOfFiveElements[b15];
		sbyte b18 = proportionOfFiveElements[index2];
		sbyte b19 = proportionOfFiveElements[index3];
		int num2 = item * 40 / 100;
		int num3 = item * 80 / 100;
		if (item2 < num2)
		{
			return b4;
		}
		if (b17 == item2)
		{
			return (sbyte)(6 + b15 * 6 + ((b17 >= num3) ? 3 : 2));
		}
		if (b16 == item2)
		{
			return (sbyte)(6 + b4 * 6 + ((b16 >= num3) ? 2 : 3));
		}
		if (b19 == item2)
		{
			return (sbyte)(6 + b4 * 6 + ((b19 >= num3) ? 4 : 5));
		}
		if (b18 == item2)
		{
			return (sbyte)(6 + b4 * 6 + ((b18 < num3) ? 1 : 0));
		}
		return b4;
	}
}
