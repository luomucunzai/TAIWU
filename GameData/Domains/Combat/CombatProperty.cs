using System;
using GameData.Domains.Character;

namespace GameData.Domains.Combat
{
	// Token: 0x020006AD RID: 1709
	public readonly struct CombatProperty
	{
		// Token: 0x060062E1 RID: 25313 RVA: 0x00382448 File Offset: 0x00380648
		public static CombatProperty Create(CombatContext context, sbyte hitType)
		{
			return new CombatProperty
			{
				HitValue = context.Attacker.GetHitValue(context, hitType),
				AvoidValue = context.Defender.GetAvoidValue(context, hitType),
				AttackValue = context.Attacker.GetPenetrate(context),
				DefendValue = context.Defender.GetPenetrateResist(context)
			};
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x060062E2 RID: 25314 RVA: 0x003824B9 File Offset: 0x003806B9
		// (set) Token: 0x060062E3 RID: 25315 RVA: 0x003824C1 File Offset: 0x003806C1
		public int HitValue { get; set; }

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x060062E4 RID: 25316 RVA: 0x003824CA File Offset: 0x003806CA
		// (set) Token: 0x060062E5 RID: 25317 RVA: 0x003824D2 File Offset: 0x003806D2
		public int AvoidValue { get; set; }

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x060062E6 RID: 25318 RVA: 0x003824DB File Offset: 0x003806DB
		// (set) Token: 0x060062E7 RID: 25319 RVA: 0x003824E3 File Offset: 0x003806E3
		public OuterAndInnerInts AttackValue { get; set; }

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x060062E8 RID: 25320 RVA: 0x003824EC File Offset: 0x003806EC
		// (set) Token: 0x060062E9 RID: 25321 RVA: 0x003824F4 File Offset: 0x003806F4
		public OuterAndInnerInts DefendValue { get; set; }

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x060062EA RID: 25322 RVA: 0x003824FD File Offset: 0x003806FD
		public bool IsValid
		{
			get
			{
				return this.HitValue >= 0;
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x060062EB RID: 25323 RVA: 0x0038250B File Offset: 0x0038070B
		public int HitOdds
		{
			get
			{
				return CFormula.FormulaCalcHitOdds(this.HitValue, this.AvoidValue);
			}
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x060062EC RID: 25324 RVA: 0x0038251E File Offset: 0x0038071E
		public int CriticalPercent
		{
			get
			{
				return CFormula.FormulaCalcCriticalPercent(this.HitOdds);
			}
		}
	}
}
