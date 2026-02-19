using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class TeaHorseCaravanTerrain : ConfigData<TeaHorseCaravanTerrainItem, short>
{
	public static class DefKey
	{
		public const short Plain = 0;

		public const short Mountain = 1;

		public const short Woods = 2;

		public const short Desert = 3;

		public const short Marsh = 4;

		public const short Town = 5;
	}

	public static class DefValue
	{
		public static TeaHorseCaravanTerrainItem Plain => Instance[(short)0];

		public static TeaHorseCaravanTerrainItem Mountain => Instance[(short)1];

		public static TeaHorseCaravanTerrainItem Woods => Instance[(short)2];

		public static TeaHorseCaravanTerrainItem Desert => Instance[(short)3];

		public static TeaHorseCaravanTerrainItem Marsh => Instance[(short)4];

		public static TeaHorseCaravanTerrainItem Town => Instance[(short)5];
	}

	public static TeaHorseCaravanTerrain Instance = new TeaHorseCaravanTerrain();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name", "Desc" };

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
		_dataArray.Add(new TeaHorseCaravanTerrainItem(0, 0, 1, 10));
		_dataArray.Add(new TeaHorseCaravanTerrainItem(1, 2, 3, 10));
		_dataArray.Add(new TeaHorseCaravanTerrainItem(2, 4, 5, 10));
		_dataArray.Add(new TeaHorseCaravanTerrainItem(3, 6, 7, 7));
		_dataArray.Add(new TeaHorseCaravanTerrainItem(4, 8, 9, 2));
		_dataArray.Add(new TeaHorseCaravanTerrainItem(5, 10, 11, 5));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<TeaHorseCaravanTerrainItem>(6);
		CreateItems0();
	}
}
