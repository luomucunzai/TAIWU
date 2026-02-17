using System;
using System.Collections.Generic;
using GameData.Domains.Adventure.AdventureMap;

namespace GameData.Domains.Adventure
{
	// Token: 0x020008C7 RID: 2247
	public static class AdventureExtension
	{
		// Token: 0x06007F76 RID: 32630 RVA: 0x004C9DC0 File Offset: 0x004C7FC0
		public static ELinkDir Reverse(this ELinkDir dir)
		{
			bool flag = dir < ELinkDir.Right || dir > ELinkDir.Left;
			ELinkDir result;
			if (flag)
			{
				result = ELinkDir.Error;
			}
			else
			{
				result = ELinkDir.Left - dir;
			}
			return result;
		}

		// Token: 0x06007F77 RID: 32631 RVA: 0x004C9DEC File Offset: 0x004C7FEC
		public static AdvMapPos Offset(this ELinkDir dir, int length)
		{
			AdvMapPos offset = default(AdvMapPos);
			switch (dir)
			{
			case ELinkDir.Right:
				offset = new AdvMapPos(2 * length, 0);
				break;
			case ELinkDir.UpRight:
				offset = new AdvMapPos(length, length);
				break;
			case ELinkDir.DownRight:
				offset = new AdvMapPos(length, -length);
				break;
			case ELinkDir.UpLeft:
				offset = new AdvMapPos(-length, length);
				break;
			case ELinkDir.DownLeft:
				offset = new AdvMapPos(-length, -length);
				break;
			case ELinkDir.Left:
				offset = new AdvMapPos(-2 * length, 0);
				break;
			}
			return offset;
		}

		// Token: 0x06007F78 RID: 32632 RVA: 0x004C9E78 File Offset: 0x004C8078
		public static AdvMapPos Rotate(this ELinkDir dir, AdvMapPos basePos)
		{
			int x = (int)((basePos.X - basePos.Y) / 2);
			short y = basePos.Y;
			AdvMapPos result;
			switch (dir)
			{
			case ELinkDir.Right:
				result = new AdvMapPos(2, 0) * x + new AdvMapPos(1, 1) * (int)y;
				break;
			case ELinkDir.UpRight:
				result = new AdvMapPos(1, 1) * x + new AdvMapPos(-1, 1) * (int)y;
				break;
			case ELinkDir.DownRight:
				result = new AdvMapPos(1, -1) * x + new AdvMapPos(2, 0) * (int)y;
				break;
			case ELinkDir.UpLeft:
				result = new AdvMapPos(-1, 1) * x + new AdvMapPos(-2, 0) * (int)y;
				break;
			case ELinkDir.DownLeft:
				result = new AdvMapPos(-1, -1) * x + new AdvMapPos(1, -1) * (int)y;
				break;
			case ELinkDir.Left:
				result = new AdvMapPos(-2, 0) * x + new AdvMapPos(-1, -1) * (int)y;
				break;
			default:
				result = AdvMapPos.Error;
				break;
			}
			return result;
		}

		// Token: 0x06007F79 RID: 32633 RVA: 0x004C9FAC File Offset: 0x004C81AC
		public static HashSet<AdvMapPos> Expand(this IEnumerable<AdvMapPos> src)
		{
			HashSet<AdvMapPos> ret = new HashSet<AdvMapPos>();
			foreach (AdvMapPos one in src)
			{
				foreach (AdvMapPos pos in one.GetAroundPoints())
				{
					ret.Add(pos);
				}
			}
			return ret;
		}

		// Token: 0x06007F7A RID: 32634 RVA: 0x004CA030 File Offset: 0x004C8230
		public static HashSet<AdvMapPos> AddRange(this HashSet<AdvMapPos> src, IEnumerable<AdvMapPos> dst)
		{
			foreach (AdvMapPos one in dst)
			{
				src.Add(one);
			}
			return src;
		}
	}
}
