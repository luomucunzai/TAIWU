using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Finger
{
	// Token: 0x020005D0 RID: 1488
	public class QingHuaYuMeiRen : ChangePoisonLevelVariant1
	{
		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06004407 RID: 17415 RVA: 0x0026DC80 File Offset: 0x0026BE80
		protected override sbyte AffectPoisonType
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x06004408 RID: 17416 RVA: 0x0026DC83 File Offset: 0x0026BE83
		public QingHuaYuMeiRen()
		{
		}

		// Token: 0x06004409 RID: 17417 RVA: 0x0026DC8D File Offset: 0x0026BE8D
		public QingHuaYuMeiRen(CombatSkillKey skillKey) : base(skillKey, 3103)
		{
		}
	}
}
