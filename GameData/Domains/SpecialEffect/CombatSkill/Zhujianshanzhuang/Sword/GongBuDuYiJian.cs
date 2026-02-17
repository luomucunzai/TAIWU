using System;
using System.Collections.Generic;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Sword
{
	// Token: 0x020001B4 RID: 436
	public class GongBuDuYiJian : SwordAttackSkillEffectBase
	{
		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06002C55 RID: 11349 RVA: 0x0020747C File Offset: 0x0020567C
		protected override IEnumerable<sbyte> RequirePersonalityTypes
		{
			get
			{
				yield return 0;
				yield break;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06002C56 RID: 11350 RVA: 0x0020749B File Offset: 0x0020569B
		protected override int RequirePersonalityValue
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06002C57 RID: 11351 RVA: 0x0020749F File Offset: 0x0020569F
		protected override ushort FieldId
		{
			get
			{
				return 69;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06002C58 RID: 11352 RVA: 0x002074A3 File Offset: 0x002056A3
		protected override int AddValue
		{
			get
			{
				return 40;
			}
		}

		// Token: 0x06002C59 RID: 11353 RVA: 0x002074A7 File Offset: 0x002056A7
		public GongBuDuYiJian()
		{
		}

		// Token: 0x06002C5A RID: 11354 RVA: 0x002074B1 File Offset: 0x002056B1
		public GongBuDuYiJian(CombatSkillKey skillKey) : base(skillKey, 9101)
		{
		}
	}
}
