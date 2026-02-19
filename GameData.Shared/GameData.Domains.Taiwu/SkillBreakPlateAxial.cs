using System;
using GameData.Utilities;

namespace GameData.Domains.Taiwu;

public struct SkillBreakPlateAxial : IEquatable<SkillBreakPlateAxial>
{
	public int Q { get; private set; }

	public int R { get; private set; }

	public int S => -Q - R;

	public static int Distance(SkillBreakPlateAxial a, SkillBreakPlateAxial b)
	{
		return (MathUtils.Abs(a.Q - b.Q) + MathUtils.Abs(a.R - b.R) + MathUtils.Abs(a.S - b.S)) / 2;
	}

	public static explicit operator SkillBreakPlateIndex(SkillBreakPlateAxial axial)
	{
		SkillBreakPlateAxial skillBreakPlateAxial = axial;
		skillBreakPlateAxial.Deconstruct(out var q, out var r);
		int num = q;
		int num2 = r;
		int item = num + (num2 + (num2 & 1)) / 2;
		int item2 = -num2;
		return (x: item, y: item2);
	}

	public static implicit operator SkillBreakPlateAxial(SkillBreakPlateIndex index)
	{
		SkillBreakPlateIndex skillBreakPlateIndex = index;
		skillBreakPlateIndex.Deconstruct(out var x, out var y);
		int num = x;
		int num2 = y;
		num2 = -num2;
		int item = num - (num2 + (num2 & 1)) / 2;
		int item2 = num2;
		return (q: item, r: item2);
	}

	public static implicit operator SkillBreakPlateAxial((int q, int r) tup)
	{
		SkillBreakPlateAxial result = default(SkillBreakPlateAxial);
		(result.Q, result.R) = tup;
		return result;
	}

	public static SkillBreakPlateAxial operator +(SkillBreakPlateAxial left, SkillBreakPlateAxial right)
	{
		return new SkillBreakPlateAxial
		{
			Q = left.Q + right.Q,
			R = left.R + right.R
		};
	}

	public static SkillBreakPlateAxial operator *(SkillBreakPlateAxial axial, int multiplier)
	{
		return new SkillBreakPlateAxial
		{
			Q = axial.Q * multiplier,
			R = axial.R * multiplier
		};
	}

	public void Deconstruct(out int q, out int r)
	{
		int q2 = Q;
		int r2 = R;
		q = q2;
		r = r2;
	}

	public void Deconstruct(out int q, out int r, out int s)
	{
		int q2 = Q;
		int r2 = R;
		int s2 = S;
		q = q2;
		r = r2;
		s = s2;
	}

	public override string ToString()
	{
		return $"Axial({Q},{R},{S})";
	}

	public bool Equals(SkillBreakPlateAxial other)
	{
		if (Q == other.Q)
		{
			return R == other.R;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is SkillBreakPlateAxial other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (Q * 397) ^ R;
	}

	public static bool operator ==(SkillBreakPlateAxial left, SkillBreakPlateAxial right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(SkillBreakPlateAxial left, SkillBreakPlateAxial right)
	{
		return !left.Equals(right);
	}
}
