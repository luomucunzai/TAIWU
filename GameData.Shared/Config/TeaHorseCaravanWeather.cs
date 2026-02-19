using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class TeaHorseCaravanWeather : ConfigData<TeaHorseCaravanWeatherItem, short>
{
	public static TeaHorseCaravanWeather Instance = new TeaHorseCaravanWeather();

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
		_dataArray.Add(new TeaHorseCaravanWeatherItem(0, 0, 1, new List<sbyte>
		{
			0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
			10, 11
		}, new List<sbyte> { 0, 1, 2, 3, 4, 5 }, 0, 0, 100));
		_dataArray.Add(new TeaHorseCaravanWeatherItem(1, 2, 3, new List<sbyte> { 0, 1, 11 }, new List<sbyte> { 1, 2, 3 }, 5, 0, 10));
		_dataArray.Add(new TeaHorseCaravanWeatherItem(2, 4, 5, new List<sbyte> { 8, 9, 10 }, new List<sbyte> { 3 }, 5, 15, 10));
		_dataArray.Add(new TeaHorseCaravanWeatherItem(3, 6, 7, new List<sbyte> { 5, 6, 7 }, new List<sbyte> { 0, 1, 3, 5 }, 10, 0, 20));
		_dataArray.Add(new TeaHorseCaravanWeatherItem(4, 8, 9, new List<sbyte> { 2, 3, 4, 5, 6, 7 }, new List<sbyte> { 1, 2, 3, 4, 5 }, 10, 10, 10));
		_dataArray.Add(new TeaHorseCaravanWeatherItem(5, 10, 11, new List<sbyte>
		{
			0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
			10, 11
		}, new List<sbyte> { 0, 1, 2, 3, 4, 5 }, 0, 0, 20));
		_dataArray.Add(new TeaHorseCaravanWeatherItem(6, 12, 13, new List<sbyte> { 0, 1, 11 }, new List<sbyte> { 0, 1, 2, 3, 4, 5 }, 5, 0, 20));
		_dataArray.Add(new TeaHorseCaravanWeatherItem(7, 14, 15, new List<sbyte> { 0, 1, 11 }, new List<sbyte> { 0, 1, 2, 3, 4, 5 }, 10, 20, 10));
		_dataArray.Add(new TeaHorseCaravanWeatherItem(8, 16, 17, new List<sbyte> { 0, 1, 8, 9, 10, 11 }, new List<sbyte> { 0, 3, 5 }, 0, 5, 20));
		_dataArray.Add(new TeaHorseCaravanWeatherItem(9, 18, 19, new List<sbyte> { 2, 3, 4 }, new List<sbyte> { 3, 4 }, 0, 5, 10));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<TeaHorseCaravanWeatherItem>(10);
		CreateItems0();
	}
}
