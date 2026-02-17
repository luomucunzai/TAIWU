using System;
using System.Collections.Generic;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Sword
{
	// Token: 0x020001B7 RID: 439
	public class ShengXieCanJian : SwordAttackSkillEffectBase
	{
		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06002C6B RID: 11371 RVA: 0x00207758 File Offset: 0x00205958
		protected override IEnumerable<sbyte> RequirePersonalityTypes
		{
			get
			{
				yield return 2;
				yield break;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06002C6C RID: 11372 RVA: 0x00207777 File Offset: 0x00205977
		protected override int RequirePersonalityValue
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06002C6D RID: 11373 RVA: 0x0020777B File Offset: 0x0020597B
		protected override ushort FieldId
		{
			get
			{
				return 74;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06002C6E RID: 11374 RVA: 0x0020777F File Offset: 0x0020597F
		protected override int AddValue
		{
			get
			{
				return 150;
			}
		}

		// Token: 0x06002C6F RID: 11375 RVA: 0x00207786 File Offset: 0x00205986
		public ShengXieCanJian()
		{
		}

		// Token: 0x06002C70 RID: 11376 RVA: 0x00207790 File Offset: 0x00205990
		public ShengXieCanJian(CombatSkillKey skillKey) : base(skillKey, 9102)
		{
		}
	}
}
