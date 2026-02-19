using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class LifeLinkFeatureEffect : ConfigData<LifeLinkFeatureEffectItem, sbyte>
{
	public static LifeLinkFeatureEffect Instance = new LifeLinkFeatureEffect();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "FeatureId", "TemplateId", "FiveElements", "CriticalProbPercent" };

	internal override int ToInt(sbyte value)
	{
		return value;
	}

	internal override sbyte ToTemplateId(int value)
	{
		return (sbyte)value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new LifeLinkFeatureEffectItem(0, 0, 516, 50));
		_dataArray.Add(new LifeLinkFeatureEffectItem(1, 1, 517, 50));
		_dataArray.Add(new LifeLinkFeatureEffectItem(2, 2, 518, 50));
		_dataArray.Add(new LifeLinkFeatureEffectItem(3, 3, 519, 50));
		_dataArray.Add(new LifeLinkFeatureEffectItem(4, 4, 520, 50));
		_dataArray.Add(new LifeLinkFeatureEffectItem(5, 0, 521, -50));
		_dataArray.Add(new LifeLinkFeatureEffectItem(6, 1, 522, -50));
		_dataArray.Add(new LifeLinkFeatureEffectItem(7, 2, 523, -50));
		_dataArray.Add(new LifeLinkFeatureEffectItem(8, 3, 524, -50));
		_dataArray.Add(new LifeLinkFeatureEffectItem(9, 4, 525, -50));
		_dataArray.Add(new LifeLinkFeatureEffectItem(10, 0, 526, 25));
		_dataArray.Add(new LifeLinkFeatureEffectItem(11, 1, 527, 25));
		_dataArray.Add(new LifeLinkFeatureEffectItem(12, 2, 528, 25));
		_dataArray.Add(new LifeLinkFeatureEffectItem(13, 3, 529, 25));
		_dataArray.Add(new LifeLinkFeatureEffectItem(14, 4, 530, 25));
		_dataArray.Add(new LifeLinkFeatureEffectItem(15, 0, 531, -25));
		_dataArray.Add(new LifeLinkFeatureEffectItem(16, 1, 532, -25));
		_dataArray.Add(new LifeLinkFeatureEffectItem(17, 2, 533, -25));
		_dataArray.Add(new LifeLinkFeatureEffectItem(18, 3, 534, -25));
		_dataArray.Add(new LifeLinkFeatureEffectItem(19, 4, 535, -25));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<LifeLinkFeatureEffectItem>(20);
		CreateItems0();
	}
}
