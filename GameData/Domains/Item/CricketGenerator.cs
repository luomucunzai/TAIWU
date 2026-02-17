using System;
using System.Collections.Generic;
using System.Linq;

namespace GameData.Domains.Item
{
	// Token: 0x02000668 RID: 1640
	public static class CricketGenerator
	{
		// Token: 0x06004F8E RID: 20366 RVA: 0x002B50FF File Offset: 0x002B32FF
		private static sbyte GeneratorClamp(int grade)
		{
			return (sbyte)Math.Clamp(grade, 0, 8);
		}

		// Token: 0x06004F8F RID: 20367 RVA: 0x002B510C File Offset: 0x002B330C
		private static sbyte Generator1(sbyte orgGrade, sbyte wagerGrade, int eclecticAttainment)
		{
			return CricketGenerator.GeneratorClamp((int)wagerGrade);
		}

		// Token: 0x06004F90 RID: 20368 RVA: 0x002B5124 File Offset: 0x002B3324
		private static sbyte Generator2(sbyte orgGrade, sbyte wagerGrade, int eclecticAttainment)
		{
			return CricketGenerator.GeneratorClamp((int)orgGrade);
		}

		// Token: 0x06004F91 RID: 20369 RVA: 0x002B513C File Offset: 0x002B333C
		private static sbyte Generator3(sbyte orgGrade, sbyte wagerGrade, int eclecticAttainment)
		{
			return CricketGenerator.GeneratorClamp((int)(orgGrade - 3) + eclecticAttainment / 150);
		}

		// Token: 0x06004F92 RID: 20370 RVA: 0x002B5160 File Offset: 0x002B3360
		public static IEnumerable<sbyte> Generate(sbyte orgGrade, sbyte wagerGrade, int eclectic)
		{
			return from method in CricketGenerator.GeneratorMethods
			select method(orgGrade, wagerGrade, eclectic);
		}

		// Token: 0x040015AB RID: 5547
		private static readonly List<CricketGenerator.GeneratorDelegate> GeneratorMethods = new List<CricketGenerator.GeneratorDelegate>
		{
			new CricketGenerator.GeneratorDelegate(CricketGenerator.Generator1),
			new CricketGenerator.GeneratorDelegate(CricketGenerator.Generator2),
			new CricketGenerator.GeneratorDelegate(CricketGenerator.Generator3)
		};

		// Token: 0x02000AAC RID: 2732
		// (Invoke) Token: 0x060088BD RID: 35005
		public delegate sbyte GeneratorDelegate(sbyte orgGrade, sbyte wagerGrade, int eclecticAttainment);
	}
}
