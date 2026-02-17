using System;

namespace GameData.Domains.Combat
{
	// Token: 0x020006AF RID: 1711
	public readonly struct CombatDamageResult
	{
		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x060062ED RID: 25325 RVA: 0x0038252B File Offset: 0x0038072B
		// (set) Token: 0x060062EE RID: 25326 RVA: 0x00382533 File Offset: 0x00380733
		public int TotalDamage { get; set; }

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x060062EF RID: 25327 RVA: 0x0038253C File Offset: 0x0038073C
		// (set) Token: 0x060062F0 RID: 25328 RVA: 0x00382544 File Offset: 0x00380744
		public int LeftDamage { get; set; }

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x060062F1 RID: 25329 RVA: 0x0038254D File Offset: 0x0038074D
		// (set) Token: 0x060062F2 RID: 25330 RVA: 0x00382555 File Offset: 0x00380755
		public int MarkCount { get; set; }
	}
}
