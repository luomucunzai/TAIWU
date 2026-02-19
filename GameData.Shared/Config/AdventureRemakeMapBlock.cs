using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class AdventureRemakeMapBlock : ConfigData<AdventureRemakeMapBlockItem, short>
{
	public static class DefKey
	{
		public const short Valley = 0;
	}

	public static class DefValue
	{
		public static AdventureRemakeMapBlockItem Valley => Instance[(short)0];
	}

	public static AdventureRemakeMapBlock Instance = new AdventureRemakeMapBlock();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name", "Desc", "CircleCount" };

	internal override int ToInt(short value)
	{
		return value;
	}

	internal override short ToTemplateId(int value)
	{
		return (short)value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new AdventureRemakeMapBlockItem(0, 0, 1, new float[2] { 0.99f, 1f }, 4, new float[8] { 0f, 0.1f, 0.1f, 0.2f, 0.3f, 0.4f, 0.4f, 0.6f }, 20, new float[2] { 0.5f, 0.5f }));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<AdventureRemakeMapBlockItem>(1);
		CreateItems0();
	}
}
