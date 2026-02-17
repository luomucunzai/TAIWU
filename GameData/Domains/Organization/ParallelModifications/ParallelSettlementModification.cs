using System;

namespace GameData.Domains.Organization.ParallelModifications
{
	// Token: 0x02000650 RID: 1616
	public readonly struct ParallelSettlementModification
	{
		// Token: 0x06004872 RID: 18546 RVA: 0x0028D18A File Offset: 0x0028B38A
		public ParallelSettlementModification(short culture, short safety, int population)
		{
			this.Culture = culture;
			this.Safety = safety;
			this.Population = population;
		}

		// Token: 0x0400152B RID: 5419
		public readonly short Culture;

		// Token: 0x0400152C RID: 5420
		public readonly short Safety;

		// Token: 0x0400152D RID: 5421
		public readonly int Population;
	}
}
