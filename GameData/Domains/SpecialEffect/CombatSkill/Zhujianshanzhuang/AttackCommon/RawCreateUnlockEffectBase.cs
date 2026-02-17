using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon
{
	// Token: 0x020001E3 RID: 483
	public abstract class RawCreateUnlockEffectBase : PolearmUnlockEffectBase
	{
		// Token: 0x06002DD6 RID: 11734 RVA: 0x0020D180 File Offset: 0x0020B380
		protected RawCreateUnlockEffectBase()
		{
		}

		// Token: 0x06002DD7 RID: 11735 RVA: 0x0020D18A File Offset: 0x0020B38A
		protected RawCreateUnlockEffectBase(CombatSkillKey skillKey, int type) : base(skillKey, type)
		{
		}

		// Token: 0x06002DD8 RID: 11736 RVA: 0x0020D196 File Offset: 0x0020B396
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(312, EDataModifyType.Custom, -1);
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x0020D1AF File Offset: 0x0020B3AF
		protected override void DoAffect(DataContext context, int weaponIndex)
		{
			base.CombatChar.InvokeRawCreate(context, base.EffectId);
		}

		// Token: 0x06002DDA RID: 11738 RVA: 0x0020D1C8 File Offset: 0x0020B3C8
		public override List<int> GetModifiedValue(AffectedDataKey dataKey, List<int> dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 312;
			List<int> result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				int weaponIndex = dataKey.CustomParam0;
				ItemKey weapon = base.CombatChar.GetWeapons()[weaponIndex];
				bool flag2 = base.IsDirect ? base.IsDirectWeapon(weapon) : (base.IsMatchWeaponType(weapon) && this.ReverseEffectDoubling);
				if (flag2)
				{
					dataValue.Add(base.EffectId);
				}
				result = dataValue;
			}
			return result;
		}
	}
}
