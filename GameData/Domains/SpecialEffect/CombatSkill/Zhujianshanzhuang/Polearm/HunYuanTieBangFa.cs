using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Polearm
{
	// Token: 0x020001C5 RID: 453
	public class HunYuanTieBangFa : RawCreateUnlockEffectBase
	{
		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06002CD7 RID: 11479 RVA: 0x00209A53 File Offset: 0x00207C53
		private static CValuePercent AddFatalDamagePercent
		{
			get
			{
				return 60;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06002CD8 RID: 11480 RVA: 0x00209A5C File Offset: 0x00207C5C
		protected override IEnumerable<sbyte> RequireMainAttributeTypes { get; } = new sbyte[]
		{
			0,
			1,
			3,
			4,
			2,
			5
		};

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06002CD9 RID: 11481 RVA: 0x00209A64 File Offset: 0x00207C64
		protected override int RequireMainAttributeValue
		{
			get
			{
				return 45;
			}
		}

		// Token: 0x06002CDA RID: 11482 RVA: 0x00209A68 File Offset: 0x00207C68
		public HunYuanTieBangFa()
		{
		}

		// Token: 0x06002CDB RID: 11483 RVA: 0x00209A89 File Offset: 0x00207C89
		public HunYuanTieBangFa(CombatSkillKey skillKey) : base(skillKey, 9307)
		{
		}

		// Token: 0x06002CDC RID: 11484 RVA: 0x00209AB0 File Offset: 0x00207CB0
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.AddDirectDamageValue));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002CDD RID: 11485 RVA: 0x00209ADF File Offset: 0x00207CDF
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.AddDirectDamageValue));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			base.OnDisable(context);
		}

		// Token: 0x06002CDE RID: 11486 RVA: 0x00209B10 File Offset: 0x00207D10
		private void AddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId)
		{
			bool flag = !this.SkillKey.IsMatch(attackerId, combatSkillId) || !base.IsReverseOrUsingDirectWeapon;
			if (!flag)
			{
				this._directDamageValue += damageValue;
			}
		}

		// Token: 0x06002CDF RID: 11487 RVA: 0x00209B50 File Offset: 0x00207D50
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = !this.SkillKey.IsMatch(charId, skillId) || this._directDamageValue <= 0;
			if (!flag)
			{
				int fatalDamageValue = this._directDamageValue * HunYuanTieBangFa.AddFatalDamagePercent * (int)power;
				this._directDamageValue = 0;
				bool flag2 = fatalDamageValue <= 0;
				if (!flag2)
				{
					DomainManager.Combat.AddFatalDamageValue(context, base.CurrEnemyChar, fatalDamageValue, -1, -1, -1, EDamageType.None);
					base.ShowSpecialEffectTips(base.IsDirect, 1, 0);
				}
			}
		}

		// Token: 0x04000D85 RID: 3461
		private int _directDamageValue;
	}
}
