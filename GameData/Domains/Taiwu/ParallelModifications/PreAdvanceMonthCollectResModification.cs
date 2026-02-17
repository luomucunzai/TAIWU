using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace GameData.Domains.Taiwu.ParallelModifications
{
	// Token: 0x02000057 RID: 87
	public class PreAdvanceMonthCollectResModification
	{
		// Token: 0x060014F2 RID: 5362 RVA: 0x0014498C File Offset: 0x00142B8C
		public PreAdvanceMonthCollectResModification()
		{
			this.AddBlockMalice = new List<ValueTuple<short, short, int>>();
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x001449A4 File Offset: 0x00142BA4
		public void AddResource(sbyte resourceType, int count)
		{
			switch (resourceType)
			{
			case 0:
				this.GetFood += count;
				break;
			case 1:
				this.GetWood += count;
				break;
			case 2:
				this.GetMetal += count;
				break;
			case 3:
				this.GetJade += count;
				break;
			case 4:
				this.GetFabric += count;
				break;
			case 5:
				this.GetHerb += count;
				break;
			case 6:
				this.GetMoney += count;
				break;
			case 7:
				this.GetAuthority += count;
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported resourceType: ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(resourceType);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x00144A94 File Offset: 0x00142C94
		public int GetResource(sbyte resourceType)
		{
			if (!true)
			{
			}
			int result;
			switch (resourceType)
			{
			case 0:
				result = this.GetFood;
				break;
			case 1:
				result = this.GetWood;
				break;
			case 2:
				result = this.GetMetal;
				break;
			case 3:
				result = this.GetJade;
				break;
			case 4:
				result = this.GetFabric;
				break;
			case 5:
				result = this.GetHerb;
				break;
			case 6:
				result = this.GetMoney;
				break;
			case 7:
				result = this.GetAuthority;
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported resourceType: ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(resourceType);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x0400032E RID: 814
		public int GetFood;

		// Token: 0x0400032F RID: 815
		public int GetWood;

		// Token: 0x04000330 RID: 816
		public int GetMetal;

		// Token: 0x04000331 RID: 817
		public int GetJade;

		// Token: 0x04000332 RID: 818
		public int GetFabric;

		// Token: 0x04000333 RID: 819
		public int GetHerb;

		// Token: 0x04000334 RID: 820
		public int GetMoney;

		// Token: 0x04000335 RID: 821
		public int GetAuthority;

		// Token: 0x04000336 RID: 822
		public List<ValueTuple<short, short, int>> AddBlockMalice;
	}
}
