using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class LandFormType : ConfigData<LandFormTypeItem, sbyte>
{
	public static class DefKey
	{
		public const sbyte Flatlands = 0;

		public const sbyte Mountain = 1;

		public const sbyte Forest = 2;

		public const sbyte Lake = 3;

		public const sbyte Coast = 4;

		public const sbyte SnowMountain = 5;
	}

	public static class DefValue
	{
		public static LandFormTypeItem Flatlands => Instance[(sbyte)0];

		public static LandFormTypeItem Mountain => Instance[(sbyte)1];

		public static LandFormTypeItem Forest => Instance[(sbyte)2];

		public static LandFormTypeItem Lake => Instance[(sbyte)3];

		public static LandFormTypeItem Coast => Instance[(sbyte)4];

		public static LandFormTypeItem SnowMountain => Instance[(sbyte)5];
	}

	public static LandFormType Instance = new LandFormType();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name", "Desc" };

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
		_dataArray.Add(new LandFormTypeItem(0, 0, 1, new byte[10] { 2, 0, 2, 0, 2, 1, 5, 1, 7, 2 }, new byte[10] { 0, 0, 2, 2, 2, 0, 3, 0, 1, 0 }));
		_dataArray.Add(new LandFormTypeItem(1, 2, 3, new byte[10] { 1, 7, 2, 5, 2, 0, 1, 2, 0, 2 }, new byte[10] { 2, 3, 1, 0, 0, 1, 0, 2, 1, 0 }));
		_dataArray.Add(new LandFormTypeItem(2, 4, 5, new byte[10] { 1, 0, 7, 0, 4, 2, 3, 0, 3, 3 }, new byte[10] { 0, 0, 3, 3, 1, 2, 1, 0, 0, 0 }));
		_dataArray.Add(new LandFormTypeItem(3, 6, 7, new byte[10] { 7, 0, 2, 0, 3, 3, 2, 0, 4, 2 }, new byte[10] { 0, 0, 2, 1, 3, 3, 1, 0, 0, 0 }));
		_dataArray.Add(new LandFormTypeItem(4, 8, 9, new byte[10] { 7, 4, 1, 4, 1, 1, 0, 3, 0, 1 }, new byte[10] { 3, 2, 0, 0, 1, 1, 0, 1, 2, 0 }));
		_dataArray.Add(new LandFormTypeItem(5, 10, 11, new byte[10] { 0, 5, 1, 7, 5, 0, 0, 5, 0, 1 }, new byte[10] { 0, 1, 0, 1, 0, 0, 1, 1, 3, 3 }));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<LandFormTypeItem>(6);
		CreateItems0();
	}
}
