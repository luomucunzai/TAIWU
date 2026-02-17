using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Blade
{
	// Token: 0x020001DF RID: 479
	public class RuanShiDaoFa : BladeUnlockEffectBase
	{
		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06002DAE RID: 11694 RVA: 0x0020CB6F File Offset: 0x0020AD6F
		private int StealTrickCount
		{
			get
			{
				return base.IsDirectOrReverseEffectDoubling ? 2 : 1;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06002DAF RID: 11695 RVA: 0x0020CB80 File Offset: 0x0020AD80
		protected override IEnumerable<short> RequireWeaponTypes
		{
			get
			{
				yield return 6;
				yield return 7;
				yield break;
			}
		}

		// Token: 0x06002DB0 RID: 11696 RVA: 0x0020CB9F File Offset: 0x0020AD9F
		public RuanShiDaoFa()
		{
		}

		// Token: 0x06002DB1 RID: 11697 RVA: 0x0020CBA9 File Offset: 0x0020ADA9
		public RuanShiDaoFa(CombatSkillKey skillKey) : base(skillKey, 9201)
		{
		}

		// Token: 0x06002DB2 RID: 11698 RVA: 0x0020CBB9 File Offset: 0x0020ADB9
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002DB3 RID: 11699 RVA: 0x0020CBD6 File Offset: 0x0020ADD6
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			base.OnDisable(context);
		}

		// Token: 0x06002DB4 RID: 11700 RVA: 0x0020CBF4 File Offset: 0x0020ADF4
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = this.SkillKey.IsMatch(charId, skillId) && base.PowerMatchAffectRequire((int)power, 0) && base.IsReverseOrUsingDirectWeapon;
			if (flag)
			{
				this.DoStealTrick(context);
			}
		}

		// Token: 0x06002DB5 RID: 11701 RVA: 0x0020CC34 File Offset: 0x0020AE34
		private void DoStealTrick(DataContext context)
		{
			List<sbyte> allUsableTricks = ObjectPool<List<sbyte>>.Instance.Get();
			foreach (sbyte trick in base.EnemyChar.GetTricks().Tricks.Values)
			{
				bool flag = base.CombatChar.IsTrickUsable(trick);
				if (flag)
				{
					allUsableTricks.Add(trick);
				}
			}
			bool flag2 = allUsableTricks.Count > 0;
			if (flag2)
			{
				List<sbyte> stealTricks = ObjectPool<List<sbyte>>.Instance.Get();
				stealTricks.AddRange(RandomUtils.GetRandomUnrepeated<sbyte>(context.Random, this.StealTrickCount, allUsableTricks, null));
				DomainManager.Combat.StealTrick(context, base.CombatChar, base.EnemyChar, stealTricks);
				ObjectPool<List<sbyte>>.Instance.Return(stealTricks);
				base.ShowSpecialEffectTips(base.IsDirect, 1, 0);
			}
			ObjectPool<List<sbyte>>.Instance.Return(allUsableTricks);
		}

		// Token: 0x06002DB6 RID: 11702 RVA: 0x0020CD2C File Offset: 0x0020AF2C
		public override void DoAffectAfterCost(DataContext context, int weaponIndex)
		{
			CombatWeaponData weapon = base.CombatChar.GetWeaponData(weaponIndex);
			sbyte[] tricks = weapon.GetWeaponTricks();
			DomainManager.Combat.AddTrick(context, base.CombatChar, tricks);
			base.ShowSpecialEffectTips(base.IsDirect, 2, 1);
		}
	}
}
