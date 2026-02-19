using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class SkillBreakBonusEffect : ConfigData<SkillBreakBonusEffectItem, sbyte>
{
	public static class DefKey
	{
		public const sbyte MusicBook = 0;

		public const sbyte ChessBook = 1;

		public const sbyte PoemBook = 2;

		public const sbyte PaintingBook = 3;

		public const sbyte MedicineBook = 4;

		public const sbyte ToxicologyBook = 5;

		public const sbyte ForgingBook = 6;

		public const sbyte WoodWorkingBook = 7;

		public const sbyte WeavingBook = 8;

		public const sbyte JadeBook = 9;

		public const sbyte MathBook = 10;

		public const sbyte AppraisalBook = 11;

		public const sbyte CookingBook = 12;

		public const sbyte EclecticBook = 13;

		public const sbyte BuddhismBook = 14;

		public const sbyte TaoismBook = 15;

		public const sbyte HealInjuryOuter = 16;

		public const sbyte HealInjuryInner = 17;

		public const sbyte HealPoison = 18;

		public const sbyte HealQiDisorder = 19;

		public const sbyte HealHealth = 20;

		public const sbyte Attack = 21;

		public const sbyte Defence = 22;

		public const sbyte HitValue = 23;

		public const sbyte AvoidValue = 24;

		public const sbyte BreathStance = 25;

		public const sbyte Cast = 26;

		public const sbyte MoveSpeed = 27;

		public const sbyte AttackSpeed = 28;

		public const sbyte MetalMaterial = 29;

		public const sbyte WoodMaterial = 30;

		public const sbyte JadeMaterial = 31;

		public const sbyte FabricMaterial = 32;

		public const sbyte RelationAdore = 33;

		public const sbyte RelationEnemy = 34;

		public const sbyte Fruit = 35;

		public const sbyte Tea = 36;

		public const sbyte Exp = 37;

		public const sbyte Food = 38;

		public const sbyte Wine = 39;

		public const sbyte BloodDew = 40;

		public const sbyte HotPoison = 41;

		public const sbyte GloomyPoison = 42;

		public const sbyte RedPoison = 43;

		public const sbyte ColdPoison = 44;

		public const sbyte RottenPoison = 45;

		public const sbyte IllusoryPoison = 46;

		public const sbyte Friend = 47;
	}

	public static class DefValue
	{
		public static SkillBreakBonusEffectItem MusicBook => Instance[(sbyte)0];

		public static SkillBreakBonusEffectItem ChessBook => Instance[(sbyte)1];

		public static SkillBreakBonusEffectItem PoemBook => Instance[(sbyte)2];

		public static SkillBreakBonusEffectItem PaintingBook => Instance[(sbyte)3];

		public static SkillBreakBonusEffectItem MedicineBook => Instance[(sbyte)4];

		public static SkillBreakBonusEffectItem ToxicologyBook => Instance[(sbyte)5];

		public static SkillBreakBonusEffectItem ForgingBook => Instance[(sbyte)6];

		public static SkillBreakBonusEffectItem WoodWorkingBook => Instance[(sbyte)7];

		public static SkillBreakBonusEffectItem WeavingBook => Instance[(sbyte)8];

		public static SkillBreakBonusEffectItem JadeBook => Instance[(sbyte)9];

		public static SkillBreakBonusEffectItem MathBook => Instance[(sbyte)10];

		public static SkillBreakBonusEffectItem AppraisalBook => Instance[(sbyte)11];

		public static SkillBreakBonusEffectItem CookingBook => Instance[(sbyte)12];

		public static SkillBreakBonusEffectItem EclecticBook => Instance[(sbyte)13];

		public static SkillBreakBonusEffectItem BuddhismBook => Instance[(sbyte)14];

		public static SkillBreakBonusEffectItem TaoismBook => Instance[(sbyte)15];

		public static SkillBreakBonusEffectItem HealInjuryOuter => Instance[(sbyte)16];

		public static SkillBreakBonusEffectItem HealInjuryInner => Instance[(sbyte)17];

		public static SkillBreakBonusEffectItem HealPoison => Instance[(sbyte)18];

		public static SkillBreakBonusEffectItem HealQiDisorder => Instance[(sbyte)19];

		public static SkillBreakBonusEffectItem HealHealth => Instance[(sbyte)20];

		public static SkillBreakBonusEffectItem Attack => Instance[(sbyte)21];

		public static SkillBreakBonusEffectItem Defence => Instance[(sbyte)22];

		public static SkillBreakBonusEffectItem HitValue => Instance[(sbyte)23];

		public static SkillBreakBonusEffectItem AvoidValue => Instance[(sbyte)24];

		public static SkillBreakBonusEffectItem BreathStance => Instance[(sbyte)25];

		public static SkillBreakBonusEffectItem Cast => Instance[(sbyte)26];

		public static SkillBreakBonusEffectItem MoveSpeed => Instance[(sbyte)27];

		public static SkillBreakBonusEffectItem AttackSpeed => Instance[(sbyte)28];

		public static SkillBreakBonusEffectItem MetalMaterial => Instance[(sbyte)29];

		public static SkillBreakBonusEffectItem WoodMaterial => Instance[(sbyte)30];

		public static SkillBreakBonusEffectItem JadeMaterial => Instance[(sbyte)31];

		public static SkillBreakBonusEffectItem FabricMaterial => Instance[(sbyte)32];

		public static SkillBreakBonusEffectItem RelationAdore => Instance[(sbyte)33];

		public static SkillBreakBonusEffectItem RelationEnemy => Instance[(sbyte)34];

		public static SkillBreakBonusEffectItem Fruit => Instance[(sbyte)35];

		public static SkillBreakBonusEffectItem Tea => Instance[(sbyte)36];

		public static SkillBreakBonusEffectItem Exp => Instance[(sbyte)37];

		public static SkillBreakBonusEffectItem Food => Instance[(sbyte)38];

		public static SkillBreakBonusEffectItem Wine => Instance[(sbyte)39];

		public static SkillBreakBonusEffectItem BloodDew => Instance[(sbyte)40];

		public static SkillBreakBonusEffectItem HotPoison => Instance[(sbyte)41];

		public static SkillBreakBonusEffectItem GloomyPoison => Instance[(sbyte)42];

		public static SkillBreakBonusEffectItem RedPoison => Instance[(sbyte)43];

		public static SkillBreakBonusEffectItem ColdPoison => Instance[(sbyte)44];

		public static SkillBreakBonusEffectItem RottenPoison => Instance[(sbyte)45];

		public static SkillBreakBonusEffectItem IllusoryPoison => Instance[(sbyte)46];

		public static SkillBreakBonusEffectItem Friend => Instance[(sbyte)47];
	}

	public static SkillBreakBonusEffect Instance = new SkillBreakBonusEffect();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "EffectNeigong", "EffectAttack", "EffectAgile", "EffectDefense", "EffectAssist", "TemplateId", "ShortName", "Name" };

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
		_dataArray.Add(new SkillBreakBonusEffectItem(0, 0, 1, 0, 0, 0, 0, 0));
		_dataArray.Add(new SkillBreakBonusEffectItem(1, 2, 3, 1, 1, 1, 1, 1));
		_dataArray.Add(new SkillBreakBonusEffectItem(2, 4, 5, 2, 2, 2, 2, 2));
		_dataArray.Add(new SkillBreakBonusEffectItem(3, 6, 7, 3, 3, 3, 3, 3));
		_dataArray.Add(new SkillBreakBonusEffectItem(4, 8, 9, -1, 4, -1, 4, -1));
		_dataArray.Add(new SkillBreakBonusEffectItem(5, 10, 11, -1, 5, -1, 5, -1));
		_dataArray.Add(new SkillBreakBonusEffectItem(6, 12, 13, -1, 6, -1, 6, -1));
		_dataArray.Add(new SkillBreakBonusEffectItem(7, 14, 15, -1, 7, -1, 7, -1));
		_dataArray.Add(new SkillBreakBonusEffectItem(8, 16, 17, -1, 8, -1, 8, -1));
		_dataArray.Add(new SkillBreakBonusEffectItem(9, 18, 19, -1, 9, -1, 9, -1));
		_dataArray.Add(new SkillBreakBonusEffectItem(10, 20, 21, -1, 10, 10, 10, -1));
		_dataArray.Add(new SkillBreakBonusEffectItem(11, 22, 23, -1, 11, 11, 11, -1));
		_dataArray.Add(new SkillBreakBonusEffectItem(12, 24, 25, -1, 12, 12, 12, -1));
		_dataArray.Add(new SkillBreakBonusEffectItem(13, 26, 27, -1, 13, 13, 13, -1));
		_dataArray.Add(new SkillBreakBonusEffectItem(14, 28, 29, 14, 14, 14, 14, 14));
		_dataArray.Add(new SkillBreakBonusEffectItem(15, 30, 31, 15, 15, 15, 15, 15));
		_dataArray.Add(new SkillBreakBonusEffectItem(16, 32, 33, 16, 16, 16, 16, 16));
		_dataArray.Add(new SkillBreakBonusEffectItem(17, 32, 34, 16, 16, 16, 16, 16));
		_dataArray.Add(new SkillBreakBonusEffectItem(18, 35, 36, 17, 17, 17, 17, 17));
		_dataArray.Add(new SkillBreakBonusEffectItem(19, 37, 38, 18, 18, 18, 18, 18));
		_dataArray.Add(new SkillBreakBonusEffectItem(20, 39, 40, 19, 19, 19, 19, 19));
		_dataArray.Add(new SkillBreakBonusEffectItem(21, 41, 42, 20, 20, 20, 20, 20));
		_dataArray.Add(new SkillBreakBonusEffectItem(22, 43, 44, 21, 21, 21, 21, 21));
		_dataArray.Add(new SkillBreakBonusEffectItem(23, 45, 46, 22, 22, 22, 22, 22));
		_dataArray.Add(new SkillBreakBonusEffectItem(24, 47, 48, 23, 23, 23, 23, 23));
		_dataArray.Add(new SkillBreakBonusEffectItem(25, 49, 50, 24, 24, 24, 24, 24));
		_dataArray.Add(new SkillBreakBonusEffectItem(26, 51, 52, 25, 25, 25, 25, 25));
		_dataArray.Add(new SkillBreakBonusEffectItem(27, 53, 54, 26, 26, 26, 26, 26));
		_dataArray.Add(new SkillBreakBonusEffectItem(28, 55, 56, 27, 27, 27, 27, 27));
		_dataArray.Add(new SkillBreakBonusEffectItem(29, 57, 58, 28, 32, 36, 40, 44));
		_dataArray.Add(new SkillBreakBonusEffectItem(30, 59, 60, 29, 33, 37, 41, 45));
		_dataArray.Add(new SkillBreakBonusEffectItem(31, 61, 62, 30, 34, 38, 42, 46));
		_dataArray.Add(new SkillBreakBonusEffectItem(32, 63, 64, 31, 35, 39, 43, 47));
		_dataArray.Add(new SkillBreakBonusEffectItem(33, 65, 66, 48, 48, 48, 48, 48));
		_dataArray.Add(new SkillBreakBonusEffectItem(34, 67, 68, 49, 49, 49, 49, 49));
		_dataArray.Add(new SkillBreakBonusEffectItem(35, 69, 69, 50, -1, -1, -1, -1));
		_dataArray.Add(new SkillBreakBonusEffectItem(36, 70, 70, 51, 51, 51, 51, 51));
		_dataArray.Add(new SkillBreakBonusEffectItem(37, 71, 71, 52, 52, 52, 52, 52));
		_dataArray.Add(new SkillBreakBonusEffectItem(38, 72, 72, 53, 53, 53, 53, 53));
		_dataArray.Add(new SkillBreakBonusEffectItem(39, 73, 73, 54, 54, 54, 54, 54));
		_dataArray.Add(new SkillBreakBonusEffectItem(40, 74, 74, 55, 55, 55, 55, 55));
		_dataArray.Add(new SkillBreakBonusEffectItem(41, 75, 76, -1, 56, -1, -1, -1));
		_dataArray.Add(new SkillBreakBonusEffectItem(42, 75, 77, -1, 56, -1, -1, -1));
		_dataArray.Add(new SkillBreakBonusEffectItem(43, 75, 78, -1, 56, -1, -1, -1));
		_dataArray.Add(new SkillBreakBonusEffectItem(44, 75, 79, -1, 56, -1, -1, -1));
		_dataArray.Add(new SkillBreakBonusEffectItem(45, 75, 80, -1, 56, -1, -1, -1));
		_dataArray.Add(new SkillBreakBonusEffectItem(46, 75, 81, -1, 56, -1, -1, -1));
		_dataArray.Add(new SkillBreakBonusEffectItem(47, 82, 83, 57, 57, 57, 57, 57));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<SkillBreakBonusEffectItem>(48);
		CreateItems0();
	}
}
