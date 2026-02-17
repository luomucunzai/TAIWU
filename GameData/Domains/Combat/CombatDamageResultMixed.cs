using System;
using GameData.Domains.Character;

namespace GameData.Domains.Combat
{
	// Token: 0x020006B0 RID: 1712
	public readonly struct CombatDamageResultMixed
	{
		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x060062F3 RID: 25331 RVA: 0x0038255E File Offset: 0x0038075E
		// (set) Token: 0x060062F4 RID: 25332 RVA: 0x00382566 File Offset: 0x00380766
		public CombatDamageResult Outer { get; set; }

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x060062F5 RID: 25333 RVA: 0x0038256F File Offset: 0x0038076F
		// (set) Token: 0x060062F6 RID: 25334 RVA: 0x00382577 File Offset: 0x00380777
		public CombatDamageResult Inner { get; set; }

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x060062F7 RID: 25335 RVA: 0x00382580 File Offset: 0x00380780
		// (set) Token: 0x060062F8 RID: 25336 RVA: 0x00382588 File Offset: 0x00380788
		public int CriticalPercent { get; set; }

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x060062F9 RID: 25337 RVA: 0x00382594 File Offset: 0x00380794
		public int TotalDamage
		{
			get
			{
				return this.Outer.TotalDamage + this.Inner.TotalDamage;
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x060062FA RID: 25338 RVA: 0x003825C0 File Offset: 0x003807C0
		public OuterAndInnerInts MarkCounts
		{
			get
			{
				return new OuterAndInnerInts(this.Outer.MarkCount, this.Inner.MarkCount);
			}
		}

		// Token: 0x060062FB RID: 25339 RVA: 0x003825EE File Offset: 0x003807EE
		public void Deconstruct(out CombatDamageResult outer, out CombatDamageResult inner)
		{
			outer = this.Outer;
			inner = this.Inner;
		}
	}
}
