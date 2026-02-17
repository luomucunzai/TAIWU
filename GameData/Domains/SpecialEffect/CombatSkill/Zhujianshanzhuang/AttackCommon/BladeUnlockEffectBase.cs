using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon
{
	// Token: 0x020001E1 RID: 481
	public abstract class BladeUnlockEffectBase : WeaponUnlockEffectBase, IExtraUnlockEffect
	{
		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06002DC3 RID: 11715 RVA: 0x0020CFF8 File Offset: 0x0020B1F8
		protected override short WeaponType
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06002DC4 RID: 11716 RVA: 0x0020CFFC File Offset: 0x0020B1FC
		protected override CValuePercent DirectAddUnlockPercent
		{
			get
			{
				return 24;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06002DC5 RID: 11717 RVA: 0x0020D008 File Offset: 0x0020B208
		protected override bool ReverseEffectDoubling
		{
			get
			{
				List<int> charIds = ObjectPool<List<int>>.Instance.Get();
				DomainManager.Combat.GetAllCharInCombat(charIds);
				bool anyMatchRequire = charIds.Any(new Func<int, bool>(this.CharHasRequireWeapon));
				ObjectPool<List<int>>.Instance.Return(charIds);
				return anyMatchRequire;
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06002DC6 RID: 11718
		protected abstract IEnumerable<short> RequireWeaponTypes { get; }

		// Token: 0x06002DC7 RID: 11719 RVA: 0x0020D051 File Offset: 0x0020B251
		protected BladeUnlockEffectBase()
		{
		}

		// Token: 0x06002DC8 RID: 11720 RVA: 0x0020D05B File Offset: 0x0020B25B
		protected BladeUnlockEffectBase(CombatSkillKey skillKey, int type) : base(skillKey, type)
		{
		}

		// Token: 0x06002DC9 RID: 11721 RVA: 0x0020D068 File Offset: 0x0020B268
		private bool IsWeaponMatchRequire(ItemKey weaponKey)
		{
			return this.RequireWeaponTypes.Any((short x) => x == ItemTemplateHelper.GetItemSubType(weaponKey.ItemType, weaponKey.TemplateId));
		}

		// Token: 0x06002DCA RID: 11722 RVA: 0x0020D09C File Offset: 0x0020B29C
		private bool CharHasRequireWeapon(int charId)
		{
			CombatCharacter character = DomainManager.Combat.GetElement_CombatCharacterDict(charId);
			ItemKey[] weapons = character.GetWeapons();
			for (int i = 0; i < 3; i++)
			{
				bool flag = weapons[i].IsValid() && this.IsWeaponMatchRequire(weapons[i]);
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002DCB RID: 11723 RVA: 0x0020D0FD File Offset: 0x0020B2FD
		protected virtual bool CanDoAffect()
		{
			return true;
		}

		// Token: 0x06002DCC RID: 11724 RVA: 0x0020D100 File Offset: 0x0020B300
		protected sealed override void DoAffect(DataContext context, int weaponIndex)
		{
			bool flag = this.CanDoAffect();
			if (flag)
			{
				base.CombatChar.InvokeExtraUnlockEffect(this, weaponIndex);
			}
		}

		// Token: 0x06002DCD RID: 11725
		public abstract void DoAffectAfterCost(DataContext context, int weaponIndex);
	}
}
