using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class ProtagonistFeature : ConfigData<ProtagonistFeatureItem, short>
{
	public static class DefKey
	{
		public const short Strength = 0;

		public const short Dexterity = 1;

		public const short Concentration = 2;

		public const short Vitality = 3;

		public const short Energy = 4;

		public const short Intelligence = 5;

		public const short CloseFriend = 6;

		public const short Attraction = 7;

		public const short PoisonResists = 8;

		public const short Longevity = 9;

		public const short MaterialResources = 10;

		public const short Money = 11;

		public const short Clothing = 12;

		public const short Wines = 13;

		public const short Horse = 14;

		public const short CricketJar = 15;

		public const short PoisonMaterials = 16;

		public const short Medicines = 17;

		public const short Accessory = 18;

		public const short Construction = 19;

		public const short Literature = 20;

		public const short Religion = 21;

		public const short WitchDoctor = 22;

		public const short Artisan = 23;

		public const short LifeSkillLearning = 24;

		public const short CombatSkillLearning = 25;

		public const short CraftMaterials = 26;

		public const short CraftTools = 27;

		public const short MartialArtist = 28;

		public const short SkillBooks = 29;
	}

	public static class DefValue
	{
		public static ProtagonistFeatureItem Strength => Instance[(short)0];

		public static ProtagonistFeatureItem Dexterity => Instance[(short)1];

		public static ProtagonistFeatureItem Concentration => Instance[(short)2];

		public static ProtagonistFeatureItem Vitality => Instance[(short)3];

		public static ProtagonistFeatureItem Energy => Instance[(short)4];

		public static ProtagonistFeatureItem Intelligence => Instance[(short)5];

		public static ProtagonistFeatureItem CloseFriend => Instance[(short)6];

		public static ProtagonistFeatureItem Attraction => Instance[(short)7];

		public static ProtagonistFeatureItem PoisonResists => Instance[(short)8];

		public static ProtagonistFeatureItem Longevity => Instance[(short)9];

		public static ProtagonistFeatureItem MaterialResources => Instance[(short)10];

		public static ProtagonistFeatureItem Money => Instance[(short)11];

		public static ProtagonistFeatureItem Clothing => Instance[(short)12];

		public static ProtagonistFeatureItem Wines => Instance[(short)13];

		public static ProtagonistFeatureItem Horse => Instance[(short)14];

		public static ProtagonistFeatureItem CricketJar => Instance[(short)15];

		public static ProtagonistFeatureItem PoisonMaterials => Instance[(short)16];

		public static ProtagonistFeatureItem Medicines => Instance[(short)17];

		public static ProtagonistFeatureItem Accessory => Instance[(short)18];

		public static ProtagonistFeatureItem Construction => Instance[(short)19];

		public static ProtagonistFeatureItem Literature => Instance[(short)20];

		public static ProtagonistFeatureItem Religion => Instance[(short)21];

		public static ProtagonistFeatureItem WitchDoctor => Instance[(short)22];

		public static ProtagonistFeatureItem Artisan => Instance[(short)23];

		public static ProtagonistFeatureItem LifeSkillLearning => Instance[(short)24];

		public static ProtagonistFeatureItem CombatSkillLearning => Instance[(short)25];

		public static ProtagonistFeatureItem CraftMaterials => Instance[(short)26];

		public static ProtagonistFeatureItem CraftTools => Instance[(short)27];

		public static ProtagonistFeatureItem MartialArtist => Instance[(short)28];

		public static ProtagonistFeatureItem SkillBooks => Instance[(short)29];
	}

	public static ProtagonistFeature Instance = new ProtagonistFeature();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Type", "Cost", "PrerequisiteCost", "Name", "Desc" };

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
		_dataArray.Add(new ProtagonistFeatureItem(0, 0, 1, 0, 0, 1));
		_dataArray.Add(new ProtagonistFeatureItem(1, 0, 1, 0, 2, 3));
		_dataArray.Add(new ProtagonistFeatureItem(2, 0, 1, 1, 4, 5));
		_dataArray.Add(new ProtagonistFeatureItem(3, 0, 1, 1, 6, 7));
		_dataArray.Add(new ProtagonistFeatureItem(4, 0, 1, 2, 8, 9));
		_dataArray.Add(new ProtagonistFeatureItem(5, 0, 1, 2, 10, 11));
		_dataArray.Add(new ProtagonistFeatureItem(6, 0, 2, 3, 12, 13));
		_dataArray.Add(new ProtagonistFeatureItem(7, 0, 2, 3, 14, 15));
		_dataArray.Add(new ProtagonistFeatureItem(8, 0, 3, 5, 16, 17));
		_dataArray.Add(new ProtagonistFeatureItem(9, 0, 3, 5, 18, 19));
		_dataArray.Add(new ProtagonistFeatureItem(10, 1, 1, 0, 20, 21));
		_dataArray.Add(new ProtagonistFeatureItem(11, 1, 1, 0, 22, 23));
		_dataArray.Add(new ProtagonistFeatureItem(12, 1, 1, 1, 24, 25));
		_dataArray.Add(new ProtagonistFeatureItem(13, 1, 1, 1, 26, 27));
		_dataArray.Add(new ProtagonistFeatureItem(14, 1, 1, 2, 28, 29));
		_dataArray.Add(new ProtagonistFeatureItem(15, 1, 1, 2, 30, 31));
		_dataArray.Add(new ProtagonistFeatureItem(16, 1, 2, 3, 32, 33));
		_dataArray.Add(new ProtagonistFeatureItem(17, 1, 2, 3, 34, 35));
		_dataArray.Add(new ProtagonistFeatureItem(18, 1, 3, 5, 36, 37));
		_dataArray.Add(new ProtagonistFeatureItem(19, 1, 3, 5, 38, 39));
		_dataArray.Add(new ProtagonistFeatureItem(20, 2, 1, 0, 40, 41));
		_dataArray.Add(new ProtagonistFeatureItem(21, 2, 1, 0, 42, 43));
		_dataArray.Add(new ProtagonistFeatureItem(22, 2, 1, 1, 44, 45));
		_dataArray.Add(new ProtagonistFeatureItem(23, 2, 1, 1, 46, 47));
		_dataArray.Add(new ProtagonistFeatureItem(24, 2, 1, 2, 48, 49));
		_dataArray.Add(new ProtagonistFeatureItem(25, 2, 1, 2, 50, 51));
		_dataArray.Add(new ProtagonistFeatureItem(26, 2, 2, 3, 52, 53));
		_dataArray.Add(new ProtagonistFeatureItem(27, 2, 2, 3, 54, 55));
		_dataArray.Add(new ProtagonistFeatureItem(28, 2, 3, 5, 56, 57));
		_dataArray.Add(new ProtagonistFeatureItem(29, 2, 3, 5, 58, 59));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<ProtagonistFeatureItem>(30);
		CreateItems0();
	}
}
