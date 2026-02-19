using System;
using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Domains.Taiwu;

public class SkillBreakPlateObsolete : ISerializableGameData, IEquatable<SkillBreakPlateObsolete>
{
	private readonly int[][] _oddGridNeighbourPos = new int[6][]
	{
		new int[2] { -1, -1 },
		new int[2] { -1, 0 },
		new int[2] { 0, 1 },
		new int[2] { 1, 0 },
		new int[2] { 1, -1 },
		new int[2] { 0, -1 }
	};

	private readonly int[][] _evenGridNeighbourPos = new int[6][]
	{
		new int[2] { -1, 0 },
		new int[2] { -1, 1 },
		new int[2] { 0, 1 },
		new int[2] { 1, 1 },
		new int[2] { 1, 0 },
		new int[2] { 0, -1 }
	};

	public byte Width;

	public byte Height;

	public (byte, byte) StartPoint;

	public (byte, byte) EndPoint;

	public (byte, byte) CurrentPoint;

	public (byte, byte) ExtraCurrentPoint;

	public (byte, byte) LastPoint;

	public (byte, byte) ExtraPoint;

	public List<ushort> SelectStepLog;

	public byte ExtraPointType;

	public ushort SelectedPages;

	public byte BaseSuccessRate;

	public sbyte BonusGridSuccessRatePercentageFix;

	public bool Finished;

	public bool Failed;

	private sbyte _totalStepCount;

	private sbyte _usedStepCount;

	private sbyte _exceedingHalfStepCount;

	private sbyte _belowHalfStepCount;

	public SkillBreakPlateGridObsolete[][] Grids;

	private (bool doubleSuccessRate, int totalStepCount) _practicePageBonusInfo;

	private bool _useDirectPracticePage;

	public sbyte TotalStepCount
	{
		get
		{
			return _totalStepCount;
		}
		set
		{
			_totalStepCount = value;
			_exceedingHalfStepCount = (sbyte)Math.Max(0.0, (double)_usedStepCount - Math.Ceiling((float)_totalStepCount / 2f));
			_belowHalfStepCount = (sbyte)Math.Max(0.0, Math.Floor((float)_totalStepCount / 2f) - (double)_usedStepCount);
		}
	}

	public sbyte CostedStepCount
	{
		get
		{
			return _usedStepCount;
		}
		set
		{
			_usedStepCount = value;
			_exceedingHalfStepCount = (sbyte)Math.Max(0.0, (double)_usedStepCount - Math.Ceiling((float)_totalStepCount / 2f));
			_belowHalfStepCount = (sbyte)Math.Max(0.0, Math.Floor((float)_totalStepCount / 2f) - (double)_usedStepCount);
		}
	}

	public SkillBreakPlateObsolete(ushort selectedPages)
	{
		SelectedPages = selectedPages;
	}

	public (int succeedCount, int failCount) GetSucceedAndFailCount(bool exceptBonusAndPrev)
	{
		int num = 0;
		int num2 = 0;
		SkillBreakPlateGridObsolete[][] grids = Grids;
		foreach (SkillBreakPlateGridObsolete[] array in grids)
		{
			foreach (SkillBreakPlateGridObsolete skillBreakPlateGridObsolete in array)
			{
				if (skillBreakPlateGridObsolete.TemplateId == 0)
				{
					continue;
				}
				if (exceptBonusAndPrev)
				{
					sbyte templateId = skillBreakPlateGridObsolete.TemplateId;
					if (templateId == 2 || (uint)(templateId - 22) <= 1u)
					{
						continue;
					}
				}
				switch (skillBreakPlateGridObsolete.State)
				{
				case ESkillBreakGridState.Selected:
					num++;
					break;
				case ESkillBreakGridState.Failed:
					num2++;
					break;
				}
			}
		}
		return (succeedCount: num, failCount: num2);
	}

	public bool Equals(SkillBreakPlateObsolete other)
	{
		if (other == null)
		{
			return false;
		}
		if (Width != other.Width)
		{
			return false;
		}
		if (Height != other.Height)
		{
			return false;
		}
		byte b2;
		byte b;
		(b, b2) = StartPoint;
		byte b4;
		byte b3;
		(b3, b4) = other.StartPoint;
		if (b != b3 || b2 != b4)
		{
			return false;
		}
		(b4, b3) = EndPoint;
		(b2, b) = other.EndPoint;
		if (b4 != b2 || b3 != b)
		{
			return false;
		}
		(b, b2) = CurrentPoint;
		(b3, b4) = other.CurrentPoint;
		if (b != b3 || b2 != b4)
		{
			return false;
		}
		(b4, b3) = ExtraCurrentPoint;
		(b2, b) = other.ExtraCurrentPoint;
		if (b4 != b2 || b3 != b)
		{
			return false;
		}
		(b, b2) = LastPoint;
		(b3, b4) = other.LastPoint;
		if (b != b3 || b2 != b4)
		{
			return false;
		}
		(b4, b3) = ExtraPoint;
		(b2, b) = other.ExtraPoint;
		if (b4 != b2 || b3 != b)
		{
			return false;
		}
		if (ExtraPointType != other.ExtraPointType)
		{
			return false;
		}
		if (SelectedPages != other.SelectedPages)
		{
			return false;
		}
		if (BaseSuccessRate != other.BaseSuccessRate)
		{
			return false;
		}
		if (Finished != other.Finished)
		{
			return false;
		}
		if (Failed != other.Failed)
		{
			return false;
		}
		if (_totalStepCount != other._totalStepCount)
		{
			return false;
		}
		if (_usedStepCount != other._usedStepCount)
		{
			return false;
		}
		if (_exceedingHalfStepCount != other._exceedingHalfStepCount)
		{
			return false;
		}
		if (_belowHalfStepCount != other._belowHalfStepCount)
		{
			return false;
		}
		(bool, int) practicePageBonusInfo = _practicePageBonusInfo;
		(bool, int) practicePageBonusInfo2 = other._practicePageBonusInfo;
		if (practicePageBonusInfo.Item1 != practicePageBonusInfo2.Item1 || practicePageBonusInfo.Item2 != practicePageBonusInfo2.Item2)
		{
			return false;
		}
		if (_useDirectPracticePage != other._useDirectPracticePage)
		{
			return false;
		}
		if (SelectStepLog == null)
		{
			if (other.SelectStepLog != null)
			{
				return false;
			}
		}
		else if (other.SelectStepLog != null)
		{
			if (SelectStepLog.Count != other.SelectStepLog.Count)
			{
				return false;
			}
			for (int i = 0; i < SelectStepLog.Count; i++)
			{
				if (SelectStepLog[i] != other.SelectStepLog[i])
				{
					return false;
				}
			}
		}
		for (int j = 0; j < Grids.Length; j++)
		{
			SkillBreakPlateGridObsolete[] array = Grids[j];
			for (int k = 0; k < array.Length; k++)
			{
				SkillBreakPlateGridObsolete skillBreakPlateGridObsolete = array[k];
				SkillBreakPlateGridObsolete skillBreakPlateGridObsolete2 = Grids[j][k];
				if (skillBreakPlateGridObsolete.TemplateId != skillBreakPlateGridObsolete2.TemplateId)
				{
					return false;
				}
				if (skillBreakPlateGridObsolete.State != skillBreakPlateGridObsolete2.State)
				{
					return false;
				}
				if (skillBreakPlateGridObsolete.BonusType != skillBreakPlateGridObsolete2.BonusType)
				{
					return false;
				}
				if (skillBreakPlateGridObsolete.SuccessRateFix != skillBreakPlateGridObsolete2.SuccessRateFix)
				{
					return false;
				}
			}
		}
		return true;
	}

	public void CalcSkillBreakBonusCollection(SkillBreakBonusCollection bonusCollection)
	{
		bonusCollection.Clear();
		for (byte b = 0; b < Height; b++)
		{
			for (byte b2 = 0; b2 < Width; b2++)
			{
				SkillBreakPlateGridObsolete skillBreakPlateGridObsolete = Grids[b][b2];
				if (skillBreakPlateGridObsolete.State == ESkillBreakGridState.Selected && skillBreakPlateGridObsolete.BonusType >= 0)
				{
					bonusCollection.AddBonusType(skillBreakPlateGridObsolete.BonusType);
				}
			}
		}
	}

	public SkillBreakPlateObsolete()
	{
	}

	public SkillBreakPlateObsolete(SkillBreakPlateObsolete other)
	{
		Width = other.Width;
		Height = other.Height;
		StartPoint = other.StartPoint;
		EndPoint = other.EndPoint;
		CurrentPoint = other.CurrentPoint;
		ExtraCurrentPoint = other.ExtraCurrentPoint;
		LastPoint = other.LastPoint;
		ExtraPoint = other.ExtraPoint;
		if (other.SelectStepLog != null)
		{
			SelectStepLog = new List<ushort>();
			SelectStepLog.AddRange(other.SelectStepLog);
		}
		ExtraPointType = other.ExtraPointType;
		SelectedPages = other.SelectedPages;
		BaseSuccessRate = other.BaseSuccessRate;
		BonusGridSuccessRatePercentageFix = other.BonusGridSuccessRatePercentageFix;
		Finished = other.Finished;
		Failed = other.Failed;
		_totalStepCount = other._totalStepCount;
		_usedStepCount = other._usedStepCount;
		_exceedingHalfStepCount = other._exceedingHalfStepCount;
		_belowHalfStepCount = other._belowHalfStepCount;
		Grids = new SkillBreakPlateGridObsolete[Height][];
		for (int i = 0; i < Height; i++)
		{
			Grids[i] = new SkillBreakPlateGridObsolete[Width];
			Array.Copy(other.Grids[i], Grids[i], Width);
		}
		_practicePageBonusInfo = other._practicePageBonusInfo;
		_useDirectPracticePage = other._useDirectPracticePage;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 23 + GetVariableDataSize();
		return num + ((num % 4 != 0) ? (4 - num % 4) : 0);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = Width;
		ptr++;
		*ptr = Height;
		ptr++;
		*ptr = StartPoint.Item1;
		ptr++;
		*ptr = StartPoint.Item2;
		ptr++;
		*ptr = EndPoint.Item1;
		ptr++;
		*ptr = EndPoint.Item2;
		ptr++;
		*ptr = CurrentPoint.Item1;
		ptr++;
		*ptr = CurrentPoint.Item2;
		ptr++;
		*ptr = ExtraCurrentPoint.Item1;
		ptr++;
		*ptr = ExtraCurrentPoint.Item2;
		ptr++;
		*ptr = LastPoint.Item1;
		ptr++;
		*ptr = LastPoint.Item2;
		ptr++;
		*ptr = ExtraPoint.Item1;
		ptr++;
		*ptr = ExtraPoint.Item2;
		ptr++;
		*ptr = ExtraPointType;
		ptr++;
		*(ushort*)ptr = SelectedPages;
		ptr += 2;
		*ptr = BaseSuccessRate;
		ptr++;
		*ptr = (byte)BonusGridSuccessRatePercentageFix;
		ptr++;
		*ptr = (byte)TotalStepCount;
		ptr++;
		*ptr = (byte)CostedStepCount;
		ptr++;
		*ptr = (Finished ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (Failed ? ((byte)1) : ((byte)0));
		ptr++;
		for (int i = 0; i < Grids.Length; i++)
		{
			for (int j = 0; j < Grids[i].Length; j++)
			{
				ptr += Grids[i][j].Serialize(ptr);
			}
		}
		if (SelectStepLog == null)
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		else
		{
			*(ushort*)ptr = (ushort)SelectStepLog.Count;
			ptr += 2;
			for (int k = 0; k < SelectStepLog.Count; k++)
			{
				*(ushort*)ptr = SelectStepLog[k];
				ptr += 2;
			}
		}
		return ((int)(ptr - pData) + 3) / 4 * 4;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		Width = *ptr;
		ptr++;
		Height = *ptr;
		ptr++;
		StartPoint.Item1 = *ptr;
		ptr++;
		StartPoint.Item2 = *ptr;
		ptr++;
		EndPoint.Item1 = *ptr;
		ptr++;
		EndPoint.Item2 = *ptr;
		ptr++;
		CurrentPoint.Item1 = *ptr;
		ptr++;
		CurrentPoint.Item2 = *ptr;
		ptr++;
		ExtraCurrentPoint.Item1 = *ptr;
		ptr++;
		ExtraCurrentPoint.Item2 = *ptr;
		ptr++;
		LastPoint.Item1 = *ptr;
		ptr++;
		LastPoint.Item2 = *ptr;
		ptr++;
		ExtraPoint.Item1 = *ptr;
		ptr++;
		ExtraPoint.Item2 = *ptr;
		ptr++;
		ExtraPointType = *ptr;
		ptr++;
		SelectedPages = *(ushort*)ptr;
		ptr += 2;
		BaseSuccessRate = *ptr;
		ptr++;
		BonusGridSuccessRatePercentageFix = (sbyte)(*ptr);
		ptr++;
		TotalStepCount = (sbyte)(*ptr);
		ptr++;
		CostedStepCount = (sbyte)(*ptr);
		ptr++;
		Finished = *ptr != 0;
		ptr++;
		Failed = *ptr != 0;
		ptr++;
		Grids = new SkillBreakPlateGridObsolete[Height][];
		for (int i = 0; i < Height; i++)
		{
			Grids[i] = new SkillBreakPlateGridObsolete[Width];
			for (int j = 0; j < Grids[i].Length; j++)
			{
				SkillBreakPlateGridObsolete skillBreakPlateGridObsolete = new SkillBreakPlateGridObsolete();
				ptr += skillBreakPlateGridObsolete.Deserialize(ptr);
				Grids[i][j] = skillBreakPlateGridObsolete;
			}
		}
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			SelectStepLog = new List<ushort>(num);
			for (ushort num2 = 0; num2 < num; num2++)
			{
				SelectStepLog.Add(*(ushort*)ptr);
				ptr += 2;
			}
		}
		return ((int)(ptr - pData) + 3) / 4 * 4;
	}

	private int GetVariableDataSize()
	{
		int num = Grids[0][0].GetSerializedSize() * Width * Height;
		int num2 = 2;
		if (SelectStepLog != null)
		{
			num2 += SelectStepLog.Count * 2;
		}
		return num + num2;
	}
}
