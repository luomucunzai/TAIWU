using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.FistAndPalm
{
	// Token: 0x020004BC RID: 1212
	public class BoLuoMiZhang : AddOrReduceNeiliAllocation
	{
		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06003CF9 RID: 15609 RVA: 0x0024F76B File Offset: 0x0024D96B
		protected override sbyte NeiliAllocationChange
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x06003CFA RID: 15610 RVA: 0x0024F76E File Offset: 0x0024D96E
		public BoLuoMiZhang()
		{
		}

		// Token: 0x06003CFB RID: 15611 RVA: 0x0024F778 File Offset: 0x0024D978
		public BoLuoMiZhang(CombatSkillKey skillKey) : base(skillKey, 11102)
		{
			this.AffectNeiliAllocationType = 2;
		}
	}
}
