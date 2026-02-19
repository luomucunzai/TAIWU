using Config;

namespace GameData.Domains.Character;

public static class DisorderLevelOfQi
{
	public const sbyte Smooth = 0;

	public const sbyte Sluggish = 1;

	public const sbyte Blocked = 2;

	public const sbyte Disordered = 3;

	public const sbyte Cutoff = 4;

	public const int Count = 5;

	public static short MinValue => QiDisorderEffect.DefValue.Smooth.ThresholdMin;

	public static short MaxValue => QiDisorderEffect.DefValue.Cutoff.ThresholdMax;

	public static short GetShowValue(short disorderOfQi)
	{
		return (short)(disorderOfQi / 10);
	}

	public static sbyte GetDisorderLevelOfQi(short disorderOfQi)
	{
		sbyte b;
		for (b = 0; b < 4; b++)
		{
			if (QiDisorderEffect.Instance[b].ThresholdMax > disorderOfQi)
			{
				return b;
			}
		}
		return b;
	}

	public static QiDisorderEffectItem GetDisorderLevelOfQiConfig(short disorderOfQi)
	{
		QiDisorderEffectItem qiDisorderEffectItem = null;
		for (int i = 0; i < 4; i++)
		{
			if ((qiDisorderEffectItem = QiDisorderEffect.Instance[i]).ThresholdMax > disorderOfQi)
			{
				return qiDisorderEffectItem;
			}
		}
		return QiDisorderEffect.DefValue.Cutoff;
	}
}
