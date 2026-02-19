using System.Collections.Generic;
using GameData.Domains.Adventure.AdventureMap;

namespace GameData.Domains.Adventure;

public static class AdventureExtension
{
	public static ELinkDir Reverse(this ELinkDir dir)
	{
		if (dir < ELinkDir.Right || dir > ELinkDir.Left)
		{
			return ELinkDir.Error;
		}
		return 5 - dir;
	}

	public static AdvMapPos Offset(this ELinkDir dir, int length)
	{
		AdvMapPos result = default(AdvMapPos);
		switch (dir)
		{
		case ELinkDir.Left:
			result = new AdvMapPos(-2 * length, 0);
			break;
		case ELinkDir.Right:
			result = new AdvMapPos(2 * length, 0);
			break;
		case ELinkDir.DownLeft:
			result = new AdvMapPos(-length, -length);
			break;
		case ELinkDir.UpLeft:
			result = new AdvMapPos(-length, length);
			break;
		case ELinkDir.DownRight:
			result = new AdvMapPos(length, -length);
			break;
		case ELinkDir.UpRight:
			result = new AdvMapPos(length, length);
			break;
		}
		return result;
	}

	public static AdvMapPos Rotate(this ELinkDir dir, AdvMapPos basePos)
	{
		int num = (basePos.X - basePos.Y) / 2;
		short y = basePos.Y;
		return dir switch
		{
			ELinkDir.UpRight => new AdvMapPos(1, 1) * num + new AdvMapPos(-1, 1) * y, 
			ELinkDir.Right => new AdvMapPos(2, 0) * num + new AdvMapPos(1, 1) * y, 
			ELinkDir.DownRight => new AdvMapPos(1, -1) * num + new AdvMapPos(2, 0) * y, 
			ELinkDir.DownLeft => new AdvMapPos(-1, -1) * num + new AdvMapPos(1, -1) * y, 
			ELinkDir.Left => new AdvMapPos(-2, 0) * num + new AdvMapPos(-1, -1) * y, 
			ELinkDir.UpLeft => new AdvMapPos(-1, 1) * num + new AdvMapPos(-2, 0) * y, 
			_ => AdvMapPos.Error, 
		};
	}

	public static HashSet<AdvMapPos> Expand(this IEnumerable<AdvMapPos> src)
	{
		HashSet<AdvMapPos> hashSet = new HashSet<AdvMapPos>();
		foreach (AdvMapPos item2 in src)
		{
			AdvMapPos[] aroundPoints = item2.GetAroundPoints();
			foreach (AdvMapPos item in aroundPoints)
			{
				hashSet.Add(item);
			}
		}
		return hashSet;
	}

	public static HashSet<AdvMapPos> AddRange(this HashSet<AdvMapPos> src, IEnumerable<AdvMapPos> dst)
	{
		foreach (AdvMapPos item in dst)
		{
			src.Add(item);
		}
		return src;
	}
}
