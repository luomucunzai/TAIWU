using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class TeaHorseCaravanEvent : ConfigData<TeaHorseCaravanEventItem, short>
{
	public static class DefKey
	{
		public const short FindMirage = 0;

		public const short FindBigfoot = 1;

		public const short FindAnimal = 2;

		public const short FindPlant = 3;

		public const short GetInformation = 4;

		public const short FindSettlement = 5;

		public const short FindWeather = 6;

		public const short Lost = 7;

		public const short MeetTheif = 8;

		public const short GoodsDamage = 9;

		public const short FindWreckage = 10;

		public const short HelpPasserby = 11;

		public const short Unacclimatized = 12;

		public const short GetHelp = 13;

		public const short FindVenison = 14;

		public const short FindFruit1 = 15;

		public const short FindFruit2 = 16;

		public const short FindFruit3 = 17;

		public const short FindVillage1 = 18;

		public const short FindVillage2 = 19;

		public const short FindVillage3 = 20;

		public const short MeetMerchan1 = 21;

		public const short MeetMerchan2 = 22;

		public const short MeetMerchan3 = 23;
	}

	public static class DefValue
	{
		public static TeaHorseCaravanEventItem FindMirage => Instance[(short)0];

		public static TeaHorseCaravanEventItem FindBigfoot => Instance[(short)1];

		public static TeaHorseCaravanEventItem FindAnimal => Instance[(short)2];

		public static TeaHorseCaravanEventItem FindPlant => Instance[(short)3];

		public static TeaHorseCaravanEventItem GetInformation => Instance[(short)4];

		public static TeaHorseCaravanEventItem FindSettlement => Instance[(short)5];

		public static TeaHorseCaravanEventItem FindWeather => Instance[(short)6];

		public static TeaHorseCaravanEventItem Lost => Instance[(short)7];

		public static TeaHorseCaravanEventItem MeetTheif => Instance[(short)8];

		public static TeaHorseCaravanEventItem GoodsDamage => Instance[(short)9];

		public static TeaHorseCaravanEventItem FindWreckage => Instance[(short)10];

		public static TeaHorseCaravanEventItem HelpPasserby => Instance[(short)11];

		public static TeaHorseCaravanEventItem Unacclimatized => Instance[(short)12];

		public static TeaHorseCaravanEventItem GetHelp => Instance[(short)13];

		public static TeaHorseCaravanEventItem FindVenison => Instance[(short)14];

		public static TeaHorseCaravanEventItem FindFruit1 => Instance[(short)15];

		public static TeaHorseCaravanEventItem FindFruit2 => Instance[(short)16];

		public static TeaHorseCaravanEventItem FindFruit3 => Instance[(short)17];

		public static TeaHorseCaravanEventItem FindVillage1 => Instance[(short)18];

		public static TeaHorseCaravanEventItem FindVillage2 => Instance[(short)19];

		public static TeaHorseCaravanEventItem FindVillage3 => Instance[(short)20];

		public static TeaHorseCaravanEventItem MeetMerchan1 => Instance[(short)21];

		public static TeaHorseCaravanEventItem MeetMerchan2 => Instance[(short)22];

		public static TeaHorseCaravanEventItem MeetMerchan3 => Instance[(short)23];
	}

	public static TeaHorseCaravanEvent Instance = new TeaHorseCaravanEvent();

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
		_dataArray.Add(new TeaHorseCaravanEventItem(0, 0, 1, 2, 1, forwardHappen: true, returnHappen: true, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0));
		_dataArray.Add(new TeaHorseCaravanEventItem(1, 2, 3, 2, 1, forwardHappen: true, returnHappen: true, 0, 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0));
		_dataArray.Add(new TeaHorseCaravanEventItem(2, 4, 5, 2, 1, forwardHappen: true, returnHappen: true, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0));
		_dataArray.Add(new TeaHorseCaravanEventItem(3, 6, 7, 2, 1, forwardHappen: true, returnHappen: true, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0));
		_dataArray.Add(new TeaHorseCaravanEventItem(4, 8, 9, 2, 1, forwardHappen: true, returnHappen: true, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0));
		_dataArray.Add(new TeaHorseCaravanEventItem(5, 10, 11, 2, 1, forwardHappen: true, returnHappen: true, 0, 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0));
		_dataArray.Add(new TeaHorseCaravanEventItem(6, 12, 13, 2, 1, forwardHappen: true, returnHappen: true, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0));
		_dataArray.Add(new TeaHorseCaravanEventItem(7, 14, 15, 3, 1, forwardHappen: true, returnHappen: true, 0, 0, 0, 0, 0, 0, -100, 0, 0, 0, 0, 0, 0, 0));
		_dataArray.Add(new TeaHorseCaravanEventItem(8, 16, 17, 5, 1, forwardHappen: true, returnHappen: true, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 3, 0, 0, 0));
		_dataArray.Add(new TeaHorseCaravanEventItem(9, 18, 19, 5, 1, forwardHappen: true, returnHappen: true, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0));
		_dataArray.Add(new TeaHorseCaravanEventItem(10, 20, 21, 4, 1, forwardHappen: true, returnHappen: true, 0, 0, 0, 0, 0, 0, 0, 4, 6, 0, 0, 0, 0, 1));
		_dataArray.Add(new TeaHorseCaravanEventItem(11, 22, 23, 7, 1, forwardHappen: true, returnHappen: true, 0, 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, -10, -5, 0));
		_dataArray.Add(new TeaHorseCaravanEventItem(12, 24, 25, 7, 1, forwardHappen: true, returnHappen: true, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -30, -10, 0));
		_dataArray.Add(new TeaHorseCaravanEventItem(13, 26, 27, 6, 1, forwardHappen: true, returnHappen: true, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 30, 0));
		_dataArray.Add(new TeaHorseCaravanEventItem(14, 28, 29, 6, 1, forwardHappen: true, returnHappen: true, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 30, 0));
		_dataArray.Add(new TeaHorseCaravanEventItem(15, 30, 31, 1, 1, forwardHappen: true, returnHappen: true, 0, 0, 30, 50, 10, 15, 0, 0, 0, 0, 0, 0, 0, 0));
		_dataArray.Add(new TeaHorseCaravanEventItem(16, 30, 31, 1, 1, forwardHappen: true, returnHappen: true, 0, 0, 30, 50, 10, 15, 0, 0, 0, 0, 0, 0, 0, 0));
		_dataArray.Add(new TeaHorseCaravanEventItem(17, 30, 31, 1, 1, forwardHappen: true, returnHappen: true, 0, 0, 30, 50, 10, 15, 0, 0, 0, 0, 0, 0, 0, 0));
		_dataArray.Add(new TeaHorseCaravanEventItem(18, 32, 33, 0, 1, forwardHappen: true, returnHappen: true, 60, 80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
		_dataArray.Add(new TeaHorseCaravanEventItem(19, 32, 33, 0, 1, forwardHappen: true, returnHappen: true, 60, 80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
		_dataArray.Add(new TeaHorseCaravanEventItem(20, 32, 33, 0, 1, forwardHappen: true, returnHappen: true, 60, 80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
		_dataArray.Add(new TeaHorseCaravanEventItem(21, 34, 35, 0, 1, forwardHappen: true, returnHappen: true, 40, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
		_dataArray.Add(new TeaHorseCaravanEventItem(22, 34, 35, 0, 1, forwardHappen: true, returnHappen: true, 40, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
		_dataArray.Add(new TeaHorseCaravanEventItem(23, 34, 35, 0, 1, forwardHappen: true, returnHappen: true, 40, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<TeaHorseCaravanEventItem>(24);
		CreateItems0();
	}
}
