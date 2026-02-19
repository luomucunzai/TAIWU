using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Config;
using GameData.Combat.Math;
using GameData.Domains.CombatSkill;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Taiwu;

[AutoGenerateSerializableGameData(IsExtensible = true)]
public class SkillBreakPlate : ISerializableGameData, IEnumerable<SkillBreakPlateGrid>, IEnumerable
{
	public static class FieldIds
	{
		public const ushort Grids = 0;

		public const ushort Width = 1;

		public const ushort Height = 2;

		public const ushort InternalState = 3;

		public const ushort StepBase = 4;

		public const ushort StepExtraNormal = 5;

		public const ushort StepCostedNormal = 6;

		public const ushort StepCostedGoneMad = 7;

		public const ushort Current = 8;

		public const ushort BaseSuccessRate = 9;

		public const ushort SuccessCount = 10;

		public const ushort FailedCount = 11;

		public const ushort SelectedPages = 12;

		public const ushort SelectPath = 13;

		public const ushort Bonuses = 14;

		public const ushort Count = 15;

		public static readonly string[] FieldId2FieldName = new string[15]
		{
			"Grids", "Width", "Height", "InternalState", "StepBase", "StepExtraNormal", "StepCostedNormal", "StepCostedGoneMad", "Current", "BaseSuccessRate",
			"SuccessCount", "FailedCount", "SelectedPages", "SelectPath", "Bonuses"
		};
	}

	private readonly SkillBreakPlateAxial[] _neighborAxial = new SkillBreakPlateAxial[6]
	{
		(q: -1, r: 0),
		(q: 1, r: 0),
		(q: 0, r: -1),
		(q: 0, r: 1),
		(q: -1, r: 1),
		(q: 1, r: -1)
	};

	private const byte SpecialGridOdds = 37;

	private const byte SuccessRateRandomRange = 15;

	private static readonly ISkillBreakPlateFormatter DefaultFormatter = new SkillBreakPlateFormatterDefault();

	private static readonly List<SkillBreakPlateIndex> IndexesCache0 = new List<SkillBreakPlateIndex>();

	private static readonly List<SkillBreakPlateIndex> IndexesCache1 = new List<SkillBreakPlateIndex>();

	private static readonly List<SkillBreakPlateIndex> IndexesCache2 = new List<SkillBreakPlateIndex>();

	private static List<(sbyte, short)> _specialGridTypeGenerateWeights;

	private static List<(sbyte, short)> _specialGridTypeConvertWeights;

	[SerializableGameDataField(FieldIndex = 0)]
	private SkillBreakPlateGrid[] _grids;

	[SerializableGameDataField(FieldIndex = 3)]
	private sbyte _internalState;

	[SerializableGameDataField(FieldIndex = 5)]
	private int _stepExtraNormal;

	[SerializableGameDataField(FieldIndex = 13)]
	private List<SkillBreakPlateIndex> _selectPath;

	[SerializableGameDataField(FieldIndex = 14)]
	private Dictionary<SkillBreakPlateIndex, SkillBreakPlateBonus> _bonuses;

	public int StepNormal => StepBase * CValuePercentBonus.op_Implicit(OutlineConfig.StepBonusNormal) + _stepExtraNormal;

	public int StepGoneMad => StepBase * CValuePercentBonus.op_Implicit(OutlineConfig.StepBonusGoneMad);

	public int StepTotal => StepNormal + StepGoneMad;

	public bool StepExhausted
	{
		get
		{
			if (StepCostedNormal >= StepNormal)
			{
				return StepCostedGoneMad >= StepGoneMad;
			}
			return false;
		}
	}

	public bool AnyNeighbors => GetNeighbors(Current).Any(MaybeSuccess);

	public ESkillBreakPlateState State => (ESkillBreakPlateState)_internalState;

	public bool Success => State == ESkillBreakPlateState.Success;

	public bool Failed => State == ESkillBreakPlateState.Failed;

	public bool Finished => State != ESkillBreakPlateState.NotFinished;

	public int AddMaxPower => GetIndexes().Where(IsSelectedPoint).Sum((Func<SkillBreakPlateIndex, int>)CalcAddMaxPower);

	public IReadOnlyList<SkillBreakPlateIndex> SelectPath => _selectPath;

	public sbyte OutlineType => CombatSkillStateHelper.GetActiveOutlinePageType(SelectedPages);

	public SkillBreakOutlineEffectItem OutlineConfig => SkillBreakOutlineEffect.Instance[OutlineType];

	public bool AnyBonus
	{
		get
		{
			Dictionary<SkillBreakPlateIndex, SkillBreakPlateBonus> bonuses = _bonuses;
			if (bonuses != null)
			{
				return bonuses.Count > 0;
			}
			return false;
		}
	}

	[SerializableGameDataField(FieldIndex = 1)]
	public byte Width { get; private set; }

	[SerializableGameDataField(FieldIndex = 2)]
	public byte Height { get; private set; }

	[SerializableGameDataField(FieldIndex = 4)]
	public int StepBase { get; set; }

	[SerializableGameDataField(FieldIndex = 6)]
	public int StepCostedNormal { get; private set; }

	[SerializableGameDataField(FieldIndex = 7)]
	public int StepCostedGoneMad { get; private set; }

	[SerializableGameDataField(FieldIndex = 8)]
	public SkillBreakPlateIndex Current { get; private set; }

	[SerializableGameDataField(FieldIndex = 9)]
	public byte BaseSuccessRate { get; set; }

	[SerializableGameDataField(FieldIndex = 10)]
	public int SuccessCount { get; set; }

	[SerializableGameDataField(FieldIndex = 11)]
	public int FailedCount { get; set; }

	[SerializableGameDataField(FieldIndex = 12)]
	public ushort SelectedPages { get; private set; }

	public SkillBreakPlateGrid this[int x, int y]
	{
		get
		{
			if (x < 0 || x >= Width)
			{
				throw new IndexOutOfRangeException($"x value {x} is out of range 0 ~ {Width - 1}");
			}
			if (y < 0 || y >= Height)
			{
				throw new IndexOutOfRangeException($"y value {y} is out of range 0 ~ {Height - 1}");
			}
			return _grids[x + y * Width];
		}
		private set
		{
			_grids[x + y * Width] = value;
		}
	}

	public SkillBreakPlateGrid this[SkillBreakPlateIndex index]
	{
		get
		{
			return this[index.X, index.Y];
		}
		private set
		{
			this[index.X, index.Y] = value;
		}
	}

	public static bool IsProtrusion(int y)
	{
		return y % 2 != 0;
	}

	private static void InitializedWeights()
	{
		if (_specialGridTypeGenerateWeights != null && _specialGridTypeConvertWeights != null)
		{
			return;
		}
		_specialGridTypeGenerateWeights = new List<(sbyte, short)>();
		_specialGridTypeConvertWeights = new List<(sbyte, short)>();
		foreach (SkillBreakGridTypeItem item in (IEnumerable<SkillBreakGridTypeItem>)SkillBreakGridType.Instance)
		{
			_specialGridTypeGenerateWeights.Add((item.TemplateId, item.WeightOnGenerate));
			_specialGridTypeConvertWeights.Add((item.TemplateId, item.WeightOnConvert));
		}
	}

	private static sbyte RandomGridType(IRandomSource random)
	{
		if (!random.CheckPercentProb(37))
		{
			return 3;
		}
		return RandomSpecialGridType(random);
	}

	private static sbyte RandomSpecialGridType(IRandomSource random, bool byGenerate = true)
	{
		InitializedWeights();
		return RandomUtils.GetRandomResult(byGenerate ? _specialGridTypeGenerateWeights : _specialGridTypeConvertWeights, random);
	}

	private static short RandomGridPower(IRandomSource random, ref int power, ref int chance, int powerPerGrid)
	{
		short num = 0;
		for (int i = 0; i < powerPerGrid; i++)
		{
			if (random.CheckProb(power, chance))
			{
				num++;
				power--;
			}
			chance--;
		}
		return num;
	}

	private static SkillBreakPlateGrid RandomGridData(IRandomSource random, sbyte templateId)
	{
		sbyte successRateFix = (sbyte)random.Next(-15, 16);
		sbyte percentProb = ((templateId > 3) ? GlobalConfig.Instance.BreakoutShowSpecialCellBaseOdds : GlobalConfig.Instance.BreakoutShowNormalCellBaseOdds);
		ESkillBreakGridState state = ((!random.CheckPercentProb(percentProb)) ? ESkillBreakGridState.Invisible : ESkillBreakGridState.Showed);
		return new SkillBreakPlateGrid(templateId, successRateFix, state);
	}

	public SkillBreakPlate(IRandomSource random, SkillBreakPlateItem config, ushort selectedPages, int success = 0, int failed = 0)
		: this(random, config.PlateWidth, config.PlateHeight, selectedPages, config.BonusCount, config.TotalMaxPower, success, failed)
	{
	}

	public SkillBreakPlate(IRandomSource random, byte width, byte height, ushort selectedPages, int bonus = 0, int power = 0, int success = 0, int failed = 0)
	{
		Width = width;
		Height = height;
		SelectedPages = selectedPages;
		StepCostedNormal = (StepCostedGoneMad = 0);
		Current = SkillBreakPlateIndex.Invalid;
		GenerateBreakGrids(random, bonus, power, success, failed);
		UpdateCanSelectGrids();
	}

	private void GenerateBreakGrids(IRandomSource random, int bonusCount, int power, int success, int failed)
	{
		_grids = new SkillBreakPlateGrid[Width * Height];
		IndexesCache0.Clear();
		IndexesCache1.Clear();
		for (int i = 0; i < Width; i++)
		{
			for (int j = 0; j < Height; j++)
			{
				if (IsStartPoint(i, j))
				{
					this[i, j] = new SkillBreakPlateGrid(0, 100, ESkillBreakGridState.Selected);
				}
				else if (IsEndPoint(i, j))
				{
					this[i, j] = new SkillBreakPlateGrid(1, 100, ESkillBreakGridState.Showed);
				}
				else if (IsOutPoint(i, j))
				{
					this[i, j] = new SkillBreakPlateGrid(3, 0, ESkillBreakGridState.Invisible);
				}
				else if (IsMarginPoint(i, j))
				{
					this[i, j] = RandomGridData(random, 3);
				}
				else
				{
					(IsCenterArea(i, j) ? IndexesCache0 : IndexesCache1).Add((x: i, y: j));
				}
			}
		}
		GenerateBreakGridsBonus(random, bonusCount);
		GenerateBreakGridsPower(random, power);
		GenerateBreakGridsPrev(random, success, failed);
	}

	private void GenerateBreakGridsBonus(IRandomSource random, int bonusCount)
	{
		IndexesCache2.Clear();
		IndexesCache2.AddRange(IndexesCache0.Where(IsBonusArea));
		IndexesCache2.AddRange(IndexesCache1.Where(IsBonusArea));
		bonusCount = MathUtils.Min(bonusCount, IndexesCache2.Count);
		for (int i = 0; i < bonusCount; i++)
		{
			int index = random.Next(IndexesCache2.Count);
			SkillBreakPlateIndex skillBreakPlateIndex = IndexesCache2[index];
			CollectionUtils.SwapAndRemove(IndexesCache2, index);
			int num = IndexesCache0.IndexOf(skillBreakPlateIndex);
			if (num >= 0)
			{
				CollectionUtils.SwapAndRemove(IndexesCache0, num);
			}
			int num2 = IndexesCache1.IndexOf(skillBreakPlateIndex);
			if (num2 >= 0)
			{
				CollectionUtils.SwapAndRemove(IndexesCache1, num2);
			}
			this[skillBreakPlateIndex] = new SkillBreakPlateGrid(2, 100, ESkillBreakGridState.Showed);
		}
	}

	private void GenerateBreakGridsPower(IRandomSource random, int power)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		CValuePercent val = CValuePercent.op_Implicit(MathUtils.Clamp(50 + OutlineConfig.PowerAddCenterRate, 0, 100));
		int power2 = power * val;
		int power3 = power - power2;
		int maxPowerPerGrid = OutlineConfig.MaxPowerPerGrid;
		int chance = IndexesCache0.Count * maxPowerPerGrid;
		int chance2 = IndexesCache1.Count * maxPowerPerGrid;
		foreach (SkillBreakPlateIndex item in IndexesCache0)
		{
			sbyte templateId = RandomGridType(random);
			this[item] = RandomGridData(random, templateId);
			this[item].AddMaxPower = RandomGridPower(random, ref power2, ref chance, maxPowerPerGrid);
		}
		foreach (SkillBreakPlateIndex item2 in IndexesCache1)
		{
			sbyte templateId2 = RandomGridType(random);
			this[item2] = RandomGridData(random, templateId2);
			this[item2].AddMaxPower = RandomGridPower(random, ref power3, ref chance2, maxPowerPerGrid);
		}
	}

	private void GenerateBreakGridsPrev(IRandomSource random, int lastSuccess, int lastFailed)
	{
		IndexesCache0.Clear();
		foreach (SkillBreakPlateIndex index2 in GetIndexes())
		{
			if (this[index2].TemplateId == 3)
			{
				IndexesCache0.Add(index2);
			}
		}
		for (int i = 0; i < IndexesCache0.Count; i++)
		{
			if (random.CheckProb(lastSuccess + lastFailed, IndexesCache0.Count - i))
			{
				bool flag = RandomIsInner(random, lastSuccess > 0, lastFailed > 0);
				if (flag)
				{
					lastSuccess--;
				}
				else
				{
					lastFailed--;
				}
				SkillBreakPlateIndex index = IndexesCache0[i];
				this[index].TemplateId = (sbyte)(flag ? 22 : 23);
			}
		}
	}

	public static bool RandomIsInner(IRandomSource random, bool anyInner, bool anyOuter)
	{
		if (anyOuter)
		{
			if (anyInner)
			{
				return random.CheckPercentProb(50);
			}
			return false;
		}
		return true;
	}

	public override string ToString()
	{
		return ToString(DefaultFormatter);
	}

	public string ToString(ISkillBreakPlateFormatter formatter)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine($"SkillBreakPlate Outline {OutlineType} ({Width}x{Height})");
		for (int num = Height - 1; num >= 0; num--)
		{
			bool flag = IsProtrusion(num);
			int num2 = (int)Width - ((!flag) ? 1 : 0);
			for (int i = 0; i < num2; i++)
			{
				if (!flag)
				{
					stringBuilder.Append(formatter.AlignSpace);
				}
				SkillBreakPlateGrid grid = this[i, num];
				stringBuilder.Append(formatter.Format((x: i, y: num), grid));
				if (flag && i != num2 - 1)
				{
					stringBuilder.Append(formatter.AlignSpace);
				}
			}
			stringBuilder.AppendLine();
		}
		return stringBuilder.ToString();
	}

	public bool CheckIndex(int x, int y)
	{
		if (x >= 0 && x < Width && y >= 0 && y < Height)
		{
			return !IsOutPoint(x, y);
		}
		return false;
	}

	public bool CheckIndex(SkillBreakPlateIndex index)
	{
		return CheckIndex(index.X, index.Y);
	}

	public bool CanSelectBreak(SkillBreakPlateIndex index)
	{
		if (!CheckIndex(index) || this[index].State != ESkillBreakGridState.CanSelect)
		{
			return false;
		}
		if (StepExhausted)
		{
			return CalcCostStep(index) <= 0;
		}
		return true;
	}

	public SkillBreakPlateBonus GetBonus(SkillBreakPlateIndex index)
	{
		return _bonuses?.GetOrDefault(index, SkillBreakPlateBonus.Invalid) ?? SkillBreakPlateBonus.Invalid;
	}

	public IEnumerable<SkillBreakPlateBonus> GetBonuses()
	{
		if (!Success)
		{
			return Enumerable.Empty<SkillBreakPlateBonus>();
		}
		return GetBonusesWithoutCheck();
	}

	public IEnumerable<SkillBreakPlateBonus> GetBonusesWithoutCheck()
	{
		IEnumerable<SkillBreakPlateBonus> enumerable = _bonuses?.Values;
		return enumerable ?? Enumerable.Empty<SkillBreakPlateBonus>();
	}

	public IEnumerable<SkillBreakPlateIndex> GetIndexes()
	{
		for (int x = 0; x < Width; x++)
		{
			for (int y = 0; y < Height; y++)
			{
				if (CheckIndex(x, y))
				{
					yield return (x: x, y: y);
				}
			}
		}
	}

	public IEnumerator<SkillBreakPlateGrid> GetEnumerator()
	{
		for (int x = 0; x < Width; x++)
		{
			for (int y = 0; y < Height; y++)
			{
				if (CheckIndex(x, y))
				{
					yield return this[x, y];
				}
			}
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	[Obsolete("Use overload instead.")]
	public int CalcCostExp(int baseCostExp)
	{
		return baseCostExp + baseCostExp * StepCostedGoneMad * 10 / 100;
	}

	public int CalcCostExp(int baseCostExp, SkillBreakPlateIndex index)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		CValuePercentBonus val = CValuePercentBonus.op_Implicit(0);
		val = ((!IsCenterArea(index.X, index.Y)) ? (val + CValuePercentBonus.op_Implicit(OutlineConfig.AddExpCostEdge)) : (val + CValuePercentBonus.op_Implicit(OutlineConfig.AddExpCostCenter)));
		val = ((!CalcStepIsInGoneMad(index)) ? (val + CValuePercentBonus.op_Implicit(OutlineConfig.AddExpCostNormal)) : (val + CValuePercentBonus.op_Implicit(OutlineConfig.AddExpCostGoneMad)));
		return baseCostExp * val;
	}

	public byte CalcCostStep(SkillBreakPlateIndex index)
	{
		return this[index].Template.CostBreakCount;
	}

	public bool CalcStepIsInGoneMad(SkillBreakPlateIndex index)
	{
		byte b = CalcCostStep(index);
		int num = StepCostedGoneMad + ((StepCostedNormal >= StepNormal) ? b : 0);
		if (StepCostedNormal >= StepNormal)
		{
			return num > 0;
		}
		return false;
	}

	public int CalcAddMaxPower(SkillBreakPlateIndex index)
	{
		int num = CalcAddMaxPowerBase(index);
		if (this[index].Template.IgnoreEffectAddMaxPower)
		{
			return num;
		}
		int num2 = 0;
		foreach (SkillBreakPlateIndex pureNeighbor in GetPureNeighbors(index))
		{
			if (!(pureNeighbor == index) && this[pureNeighbor].State == ESkillBreakGridState.Selected)
			{
				if (this[pureNeighbor].Template.ClearNeighborMaxPower && this[index].TemplateId != 2)
				{
					return 0;
				}
				num2++;
				num += this[pureNeighbor].Template.NeighborAddMaxPowerWhenActive;
			}
		}
		return num + num2 * this[index].Template.SucceedNeighborAddMaxPower;
	}

	public int CalcAddMaxPowerAsBonus(SkillBreakPlateIndex index, int impactRange)
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		foreach (SkillBreakPlateIndex pureNeighbor in GetPureNeighbors(index, impactRange))
		{
			SkillBreakPlateGrid skillBreakPlateGrid = this[pureNeighbor];
			int num4 = ((skillBreakPlateGrid.TemplateId != 2) ? CalcAddMaxPower(pureNeighbor) : 0);
			if (num4 == 0)
			{
				continue;
			}
			num += num4;
			if (skillBreakPlateGrid.State == ESkillBreakGridState.Selected)
			{
				if (skillBreakPlateGrid.RecordedStepIsGoneMad)
				{
					num3 += num4;
				}
				else
				{
					num2 += num4;
				}
			}
		}
		int num5 = num * CValuePercentBonus.op_Implicit(OutlineConfig.BonusAddMaxPower) + num2 * CValuePercent.op_Implicit(OutlineConfig.BonusAddMaxPowerNormal) + num3 * CValuePercent.op_Implicit(OutlineConfig.BonusAddMaxPowerGoneMad);
		CValuePercent val = CValuePercent.op_Implicit((int)GlobalConfig.Instance.BreakoutBonusAddPowerCorrectionFactor);
		return num5 * val;
	}

	private int CalcAddMaxPowerBase(SkillBreakPlateIndex index)
	{
		SkillBreakPlateGrid skillBreakPlateGrid = this[index];
		switch (skillBreakPlateGrid.TemplateId)
		{
		case 0:
		case 1:
			return 0;
		case 2:
			return CalcAddMaxPowerAsBonus(index, GetBonus(index).ImpactRange);
		default:
			return skillBreakPlateGrid.AddMaxPower;
		}
	}

	public short CalcSuccessRate(SkillBreakPlateIndex index)
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		SkillBreakPlateGrid skillBreakPlateGrid = this[index];
		if (skillBreakPlateGrid.Template.FixedSuccessRate > 0)
		{
			return skillBreakPlateGrid.Template.FixedSuccessRate;
		}
		if (skillBreakPlateGrid.State == ESkillBreakGridState.Selected && skillBreakPlateGrid.RecordedSuccessRate > 0)
		{
			return skillBreakPlateGrid.RecordedSuccessRate;
		}
		int num = MathUtils.Clamp(BaseSuccessRate + skillBreakPlateGrid.SuccessRateFix, 5, 100);
		num *= CValuePercentBonus.op_Implicit((int)skillBreakPlateGrid.Template.SuccessRateBonus);
		num *= CalcSuccessRateBonus(index);
		if (CheckIndex(Current))
		{
			num += this[Current].Template.NextSuccessRateBonus;
		}
		return (short)MathUtils.Clamp(num, 0, 100);
	}

	private CValuePercentBonus CalcSuccessRateBonus(SkillBreakPlateIndex index)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		int num2 = MathUtils.Max(StepCostedNormal - StepNormal * CValueHalf.RoundUp, 0);
		int num3 = MathUtils.Max(StepNormal * CValueHalf.RoundDown - StepCostedNormal, 0);
		int num4 = 0;
		foreach (SkillBreakPlateIndex pureNeighbor in GetPureNeighbors(index))
		{
			SkillBreakPlateGrid skillBreakPlateGrid = this[pureNeighbor];
			num += skillBreakPlateGrid.Template.BreakCountAboveHalfBonus * num2;
			num += skillBreakPlateGrid.Template.BreakCountBelowHalfBonus * num3;
			if (!(pureNeighbor == index))
			{
				num += skillBreakPlateGrid.Template.NeighborSuccessRateBonus;
				if (skillBreakPlateGrid.State == ESkillBreakGridState.Selected)
				{
					num += skillBreakPlateGrid.Template.NeighborSuccessRateBonusWhenActive;
					num4++;
				}
			}
		}
		return CValuePercentBonus.op_Implicit(num + this[index].Template.SucceedNeighborSuccessRateBonus * num4);
	}

	public int CalcDistance(SkillBreakPlateIndex a, SkillBreakPlateIndex b)
	{
		if (!CheckIndex(a) || !CheckIndex(b))
		{
			return -1;
		}
		return SkillBreakPlateAxial.Distance(a, b);
	}

	private IEnumerable<SkillBreakPlateIndex> GetNeighbors(SkillBreakPlateIndex pos)
	{
		if (!CheckIndex(pos))
		{
			return GetNeighborsByStart();
		}
		return GetNeighborsGeneral(pos);
	}

	private IEnumerable<SkillBreakPlateIndex> GetNeighborsByStart()
	{
		return (from pos in GetIndexes()
			where IsStartPoint(pos.X, pos.Y)
			select pos).SelectMany(GetNeighborsGeneral);
	}

	private IEnumerable<SkillBreakPlateIndex> GetNeighborsGeneral(SkillBreakPlateIndex pos)
	{
		SkillBreakPlateGrid grid = this[pos];
		SkillBreakPlateAxial axial = pos;
		SkillBreakPlateAxial[] neighborAxial = _neighborAxial;
		foreach (SkillBreakPlateAxial skillBreakPlateAxial in neighborAxial)
		{
			SkillBreakPlateIndex skillBreakPlateIndex = (SkillBreakPlateIndex)(axial + skillBreakPlateAxial * grid.Template.NextStepOffset);
			if (CheckIndex(skillBreakPlateIndex) && this[skillBreakPlateIndex].State.CanInteract())
			{
				yield return skillBreakPlateIndex;
			}
		}
		if (!grid.Template.NextStepCanJumpToSame)
		{
			yield break;
		}
		foreach (SkillBreakPlateIndex index in GetIndexes())
		{
			if (!(pos == index) && CalcDistance(pos, index) != grid.Template.NextStepOffset && this[index].TemplateId == grid.TemplateId && this[index].State.CanInteract())
			{
				yield return index;
			}
		}
	}

	private IEnumerable<SkillBreakPlateIndex> GetPureNeighbors(SkillBreakPlateIndex pos, int distance = 1)
	{
		foreach (SkillBreakPlateIndex index in GetIndexes())
		{
			if (CalcDistance(pos, index) <= distance)
			{
				yield return index;
			}
		}
	}

	private bool MaybeSuccess(SkillBreakPlateIndex index)
	{
		return CalcSuccessRate(index) > 0;
	}

	private bool IsSelectedPoint(SkillBreakPlateIndex pos)
	{
		return this[pos].State == ESkillBreakGridState.Selected;
	}

	private bool IsOutPoint(int x, int y)
	{
		if (!IsProtrusion(y))
		{
			return x == Width - 1;
		}
		return false;
	}

	private bool IsStartPoint(int x, int y)
	{
		if (y != 0 && y != Height - 1)
		{
			return false;
		}
		if (x != 0 && x != Width - (IsProtrusion(y) ? 1 : 2))
		{
			return false;
		}
		return true;
	}

	private bool IsEndPoint(int x, int y)
	{
		if (x == ((int)Width - ((!IsProtrusion(y)) ? 1 : 0)) / 2)
		{
			return y == Height / 2;
		}
		return false;
	}

	private bool IsMarginPoint(int x, int y)
	{
		int num = ((!IsProtrusion(y)) ? 1 : 0);
		if (x != 0 && x != Width - 1 - num && y != 0)
		{
			return y == Height - 1;
		}
		return true;
	}

	private bool IsCenterArea(int x, int y)
	{
		int num = Height / 2 + Height % 2;
		int num2 = (Height - num) / 2;
		if (y < num2 || y >= Height - num2)
		{
			return false;
		}
		int num3 = Width / 2 + Width % 2;
		int num4 = (Width - num3) / 2;
		int num5 = ((!IsProtrusion(y)) ? 1 : 0);
		if (x >= num4)
		{
			return x < Width - num4 - num5;
		}
		return false;
	}

	private bool IsBonusArea(SkillBreakPlateIndex index)
	{
		SkillBreakPlateIndex skillBreakPlateIndex = index;
		skillBreakPlateIndex.Deconstruct(out var x, out var y);
		int num = x;
		int num2 = y;
		int num3 = ((!IsProtrusion(num2)) ? 1 : 0);
		if (num >= 3 && num < Width - 3 - num3 && num2 >= 3)
		{
			return num2 < Height - 3;
		}
		return false;
	}

	public bool ClearBonus(SkillBreakPlateIndex index)
	{
		if (!SetBonusCheck(index) || _bonuses == null || !_bonuses.ContainsKey(index))
		{
			return false;
		}
		_bonuses.Remove(index);
		return true;
	}

	public bool SetBonus(SkillBreakPlateIndex index, SkillBreakPlateBonus bonus)
	{
		if (!SetBonusCheck(index) || bonus.ShouldBeRemoved())
		{
			return false;
		}
		if (_bonuses == null)
		{
			_bonuses = new Dictionary<SkillBreakPlateIndex, SkillBreakPlateBonus>();
		}
		_bonuses[index] = bonus;
		foreach (SkillBreakPlateIndex pureNeighbor in GetPureNeighbors(index, bonus.ImpactRange))
		{
			if (this[pureNeighbor].State == ESkillBreakGridState.Invisible)
			{
				this[pureNeighbor].State = ESkillBreakGridState.Showed;
			}
		}
		return true;
	}

	private bool SetBonusCheck(SkillBreakPlateIndex index)
	{
		if (!CheckIndex(index))
		{
			return false;
		}
		if (this[index].TemplateId == 2)
		{
			return this[index].State == ESkillBreakGridState.Selected;
		}
		return false;
	}

	public void ResetRelationBonuses()
	{
		if (_bonuses == null)
		{
			return;
		}
		IndexesCache0.Clear();
		IndexesCache0.AddRange(_bonuses.Keys);
		foreach (SkillBreakPlateIndex item in IndexesCache0)
		{
			_bonuses[item] = _bonuses[item].ResetRelationCharIds();
		}
	}

	public bool SelectBreak(IRandomSource random, SkillBreakPlateIndex index, out bool selectInGoneMad)
	{
		selectInGoneMad = CalcStepIsInGoneMad(index);
		if (!CanSelectBreak(index) || Finished)
		{
			return false;
		}
		short num = CalcSuccessRate(index);
		bool flag = random.CheckPercentProb(num);
		UpdateCurrentAndRecordPath(index, flag);
		bool flag2 = !flag && !GetPureNeighbors(index).Any((SkillBreakPlateIndex x) => this[x].State == ESkillBreakGridState.Selected && this[x].Template.NeighborFailedToCanSelect);
		if (selectInGoneMad)
		{
			StepCostedGoneMad += CalcCostStep(index);
		}
		else
		{
			StepCostedNormal += CalcCostStep(index);
		}
		this[index].State = (flag ? ESkillBreakGridState.Selected : ((!flag2) ? ESkillBreakGridState.CanSelect : ESkillBreakGridState.Failed));
		if (flag || flag2)
		{
			RecordSuccessRate(index, num, selectInGoneMad);
		}
		if (CalcCostStep(index) == 0)
		{
			selectInGoneMad = false;
		}
		if (flag)
		{
			SuccessCount++;
		}
		else
		{
			FailedCount++;
		}
		if (flag)
		{
			SelectBreakSpecialEffect(random, index);
		}
		UpdateState();
		UpdateCanSelectGrids();
		return true;
	}

	private void SelectBreakSpecialEffect(IRandomSource random, SkillBreakPlateIndex index)
	{
		SkillBreakGridTypeItem template = this[index].Template;
		_stepExtraNormal += template.AddStepNormal;
		SelectBreakSpecialEffectShowInvisible(random, template.ShowInvisibleCount);
		IndexesCache0.Clear();
		IndexesCache1.Clear();
		foreach (SkillBreakPlateIndex pureNeighbor in GetPureNeighbors(index))
		{
			if (!(pureNeighbor == index) && this[pureNeighbor].State.CanInteract())
			{
				if (this[pureNeighbor].Template.Type == ESkillBreakGridTypeType.Normal)
				{
					IndexesCache0.Add(pureNeighbor);
				}
				if (this[pureNeighbor].Template.Type == ESkillBreakGridTypeType.Special && template.AllNeighborSpecialConvertToNormalGrid)
				{
					this[pureNeighbor].TemplateId = 3;
				}
				if (!this[pureNeighbor].Template.IgnoreEffectAddMaxPower)
				{
					IndexesCache1.Add(pureNeighbor);
				}
			}
		}
		int transferPowerAndConvertToFailedNeighborCount = template.TransferPowerAndConvertToFailedNeighborCount;
		foreach (SkillBreakPlateIndex item in RandomUtils.GetRandomUnrepeated(random, transferPowerAndConvertToFailedNeighborCount, IndexesCache1))
		{
			this[item].State = ESkillBreakGridState.Failed;
			if (this[item].AddMaxPower > 0)
			{
				this[index].AddMaxPower = MathUtils.Max(this[index].AddMaxPower, 0) + this[item].AddMaxPower;
			}
			this[item].AddMaxPower = 0;
		}
		if (IndexesCache0.Count == 0)
		{
			return;
		}
		if (template.RandomNeighborNormalConvertToSameGrid)
		{
			int index2 = random.Next(IndexesCache0.Count);
			SkillBreakPlateIndex index3 = IndexesCache0[index2];
			CollectionUtils.SwapAndRemove(IndexesCache0, index2);
			this[index3].TemplateId = template.TemplateId;
		}
		if (!template.AllNeighborNormalConvertToSpecialGrid)
		{
			return;
		}
		foreach (SkillBreakPlateIndex item2 in IndexesCache0)
		{
			this[item2].TemplateId = RandomSpecialGridType(random, byGenerate: false);
		}
	}

	private void SelectBreakSpecialEffectShowInvisible(IRandomSource random, int showInvisibleCount)
	{
		IndexesCache0.Clear();
		foreach (SkillBreakPlateIndex index in GetIndexes())
		{
			if (this[index].State == ESkillBreakGridState.Invisible)
			{
				IndexesCache0.Add(index);
			}
		}
		foreach (SkillBreakPlateIndex item in RandomUtils.GetRandomUnrepeated(random, showInvisibleCount, IndexesCache0))
		{
			this[item].State = ESkillBreakGridState.Showed;
		}
	}

	private void UpdateCurrentAndRecordPath(SkillBreakPlateIndex index, bool success)
	{
		if (_selectPath == null)
		{
			_selectPath = new List<SkillBreakPlateIndex>();
		}
		if (Current == SkillBreakPlateIndex.Invalid)
		{
			foreach (SkillBreakPlateIndex pureNeighbor in GetPureNeighbors(index))
			{
				if (IsStartPoint(pureNeighbor.X, pureNeighbor.Y))
				{
					_selectPath.Add(pureNeighbor);
				}
			}
		}
		if (success)
		{
			Current = index;
		}
		_selectPath.Add(index);
	}

	private void RecordSuccessRate(SkillBreakPlateIndex index, short successRate, bool inGoneMad)
	{
		this[index].RecordedSuccessRate = successRate;
		this[index].RecordedStepIsGoneMad = inGoneMad;
	}

	private void UpdateCanSelectGrids()
	{
		ClearAllCanSelectGrids();
		if (Finished)
		{
			return;
		}
		foreach (SkillBreakPlateIndex neighbor in GetNeighbors(Current))
		{
			this[neighbor].State = (MaybeSuccess(neighbor) ? ESkillBreakGridState.CanSelect : ESkillBreakGridState.Showed);
		}
	}

	private void ClearAllCanSelectGrids()
	{
		foreach (SkillBreakPlateIndex index in GetIndexes())
		{
			if (this[index].State == ESkillBreakGridState.CanSelect)
			{
				this[index].State = ESkillBreakGridState.Showed;
			}
		}
	}

	private void UpdateState()
	{
		if (IsEndPoint(Current.X, Current.Y))
		{
			_internalState = 1;
		}
		else if (!AnyNeighbors)
		{
			_internalState = 2;
		}
		else
		{
			_internalState = 0;
		}
	}

	public bool UpdateSelectedPages(ushort selectedPages)
	{
		if (OutlineType != CombatSkillStateHelper.GetActiveOutlinePageType(SelectedPages))
		{
			return false;
		}
		SelectedPages = selectedPages;
		return true;
	}

	public void SelectBreakWithoutCheck(SkillBreakPlateIndex index)
	{
		Current = index;
		if (CheckIndex(index))
		{
			this[index].State = ESkillBreakGridState.Selected;
		}
		UpdateState();
	}

	public SkillBreakPlate()
	{
	}

	public SkillBreakPlate(SkillBreakPlate other)
	{
		SkillBreakPlateGrid[] grids = other._grids;
		int num = grids.Length;
		_grids = new SkillBreakPlateGrid[num];
		for (int i = 0; i < num; i++)
		{
			_grids[i] = new SkillBreakPlateGrid(grids[i]);
		}
		Width = other.Width;
		Height = other.Height;
		_internalState = other._internalState;
		StepBase = other.StepBase;
		_stepExtraNormal = other._stepExtraNormal;
		StepCostedNormal = other.StepCostedNormal;
		StepCostedGoneMad = other.StepCostedGoneMad;
		Current = other.Current;
		BaseSuccessRate = other.BaseSuccessRate;
		SuccessCount = other.SuccessCount;
		FailedCount = other.FailedCount;
		SelectedPages = other.SelectedPages;
		_selectPath = ((other._selectPath == null) ? null : new List<SkillBreakPlateIndex>(other._selectPath));
		_bonuses = ((other._bonuses == null) ? null : new Dictionary<SkillBreakPlateIndex, SkillBreakPlateBonus>(other._bonuses));
	}

	public void Assign(SkillBreakPlate other)
	{
		SkillBreakPlateGrid[] grids = other._grids;
		int num = grids.Length;
		_grids = new SkillBreakPlateGrid[num];
		for (int i = 0; i < num; i++)
		{
			_grids[i] = new SkillBreakPlateGrid(grids[i]);
		}
		Width = other.Width;
		Height = other.Height;
		_internalState = other._internalState;
		StepBase = other.StepBase;
		_stepExtraNormal = other._stepExtraNormal;
		StepCostedNormal = other.StepCostedNormal;
		StepCostedGoneMad = other.StepCostedGoneMad;
		Current = other.Current;
		BaseSuccessRate = other.BaseSuccessRate;
		SuccessCount = other.SuccessCount;
		FailedCount = other.FailedCount;
		SelectedPages = other.SelectedPages;
		_selectPath = ((other._selectPath == null) ? null : new List<SkillBreakPlateIndex>(other._selectPath));
		_bonuses = ((other._bonuses == null) ? null : new Dictionary<SkillBreakPlateIndex, SkillBreakPlateBonus>(other._bonuses));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 40;
		if (_grids != null)
		{
			num += 2;
			for (int i = 0; i < _grids.Length; i++)
			{
				num = ((_grids[i] == null) ? (num + 2) : (num + (2 + _grids[i].GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		num = ((_selectPath == null) ? (num + 2) : (num + (2 + 8 * _selectPath.Count)));
		num += 4;
		if (_bonuses != null)
		{
			foreach (KeyValuePair<SkillBreakPlateIndex, SkillBreakPlateBonus> bonuse in _bonuses)
			{
				num += bonuse.Key.GetSerializedSize();
				num += bonuse.Value.GetSerializedSize();
			}
		}
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 15;
		ptr += 2;
		if (_grids != null)
		{
			int num = _grids.Length;
			Tester.Assert(num <= 65535);
			*(ushort*)ptr = (ushort)num;
			ptr += 2;
			for (int i = 0; i < num; i++)
			{
				if (_grids[i] != null)
				{
					byte* intPtr = ptr;
					ptr += 2;
					int num2 = _grids[i].Serialize(ptr);
					ptr += num2;
					Tester.Assert(num2 <= 65535);
					*(ushort*)intPtr = (ushort)num2;
				}
				else
				{
					*(short*)ptr = 0;
					ptr += 2;
				}
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = Width;
		ptr++;
		*ptr = Height;
		ptr++;
		*ptr = (byte)_internalState;
		ptr++;
		*(int*)ptr = StepBase;
		ptr += 4;
		*(int*)ptr = _stepExtraNormal;
		ptr += 4;
		*(int*)ptr = StepCostedNormal;
		ptr += 4;
		*(int*)ptr = StepCostedGoneMad;
		ptr += 4;
		ptr += Current.Serialize(ptr);
		*ptr = BaseSuccessRate;
		ptr++;
		*(int*)ptr = SuccessCount;
		ptr += 4;
		*(int*)ptr = FailedCount;
		ptr += 4;
		*(ushort*)ptr = SelectedPages;
		ptr += 2;
		if (_selectPath != null)
		{
			int count = _selectPath.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int j = 0; j < count; j++)
			{
				ptr += _selectPath[j].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (_bonuses != null)
		{
			*(int*)ptr = _bonuses.Count;
			ptr += 4;
			foreach (KeyValuePair<SkillBreakPlateIndex, SkillBreakPlateBonus> bonuse in _bonuses)
			{
				ptr += bonuse.Key.Serialize(ptr);
				ptr += bonuse.Value.Serialize(ptr);
			}
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (_grids == null || _grids.Length != num2)
				{
					_grids = new SkillBreakPlateGrid[num2];
				}
				for (int i = 0; i < num2; i++)
				{
					ushort num3 = *(ushort*)ptr;
					ptr += 2;
					if (num3 > 0)
					{
						_grids[i] = new SkillBreakPlateGrid();
						ptr += _grids[i].Deserialize(ptr);
					}
					else
					{
						_grids[i] = null;
					}
				}
			}
			else
			{
				_grids = null;
			}
		}
		if (num > 1)
		{
			Width = *ptr;
			ptr++;
		}
		if (num > 2)
		{
			Height = *ptr;
			ptr++;
		}
		if (num > 3)
		{
			_internalState = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 4)
		{
			StepBase = *(int*)ptr;
			ptr += 4;
		}
		if (num > 5)
		{
			_stepExtraNormal = *(int*)ptr;
			ptr += 4;
		}
		if (num > 6)
		{
			StepCostedNormal = *(int*)ptr;
			ptr += 4;
		}
		if (num > 7)
		{
			StepCostedGoneMad = *(int*)ptr;
			ptr += 4;
		}
		if (num > 8)
		{
			SkillBreakPlateIndex current = Current;
			ptr += current.Deserialize(ptr);
			Current = current;
		}
		if (num > 9)
		{
			BaseSuccessRate = *ptr;
			ptr++;
		}
		if (num > 10)
		{
			SuccessCount = *(int*)ptr;
			ptr += 4;
		}
		if (num > 11)
		{
			FailedCount = *(int*)ptr;
			ptr += 4;
		}
		if (num > 12)
		{
			SelectedPages = *(ushort*)ptr;
			ptr += 2;
		}
		if (num > 13)
		{
			ushort num4 = *(ushort*)ptr;
			ptr += 2;
			if (num4 > 0)
			{
				if (_selectPath == null)
				{
					_selectPath = new List<SkillBreakPlateIndex>();
				}
				else
				{
					_selectPath.Clear();
				}
				for (int j = 0; j < num4; j++)
				{
					SkillBreakPlateIndex item = default(SkillBreakPlateIndex);
					ptr += item.Deserialize(ptr);
					_selectPath.Add(item);
				}
			}
			else
			{
				_selectPath?.Clear();
			}
		}
		if (num > 14)
		{
			int num5 = *(int*)ptr;
			ptr += 4;
			if (num5 > 0)
			{
				if (_bonuses == null)
				{
					_bonuses = new Dictionary<SkillBreakPlateIndex, SkillBreakPlateBonus>();
				}
				else
				{
					_bonuses.Clear();
				}
				for (int k = 0; k < num5; k++)
				{
					SkillBreakPlateIndex key = default(SkillBreakPlateIndex);
					ptr += key.Deserialize(ptr);
					SkillBreakPlateBonus value = default(SkillBreakPlateBonus);
					ptr += value.Deserialize(ptr);
					_bonuses.Add(key, value);
				}
			}
			else
			{
				_bonuses?.Clear();
			}
		}
		int num6 = (int)(ptr - pData);
		if (num6 > 4)
		{
			return (num6 + 3) / 4 * 4;
		}
		return num6;
	}
}
