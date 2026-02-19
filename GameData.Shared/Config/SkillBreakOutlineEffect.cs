using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class SkillBreakOutlineEffect : ConfigData<SkillBreakOutlineEffectItem, sbyte>
{
	public static SkillBreakOutlineEffect Instance = new SkillBreakOutlineEffect();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Desc" };

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
		_dataArray.Add(new SkillBreakOutlineEffectItem(0, 0, 25, -50, 0, 0, 75, 0, 3, 0, 100, 0, 0));
		_dataArray.Add(new SkillBreakOutlineEffectItem(1, 1, 0, 0, 25, 50, 0, 0, 3, 0, 0, 0, 50));
		_dataArray.Add(new SkillBreakOutlineEffectItem(2, 2, 0, 0, 0, 50, 0, 0, 2, 0, 0, 0, 0));
		_dataArray.Add(new SkillBreakOutlineEffectItem(3, 3, 0, 0, -25, 50, 0, 0, 3, 0, 0, 100, 0));
		_dataArray.Add(new SkillBreakOutlineEffectItem(4, 4, -25, 50, 0, 0, 0, 75, 3, 50, 0, 0, 0));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<SkillBreakOutlineEffectItem>(5);
		CreateItems0();
	}
}
