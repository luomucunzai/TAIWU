using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x020005A9 RID: 1449
	public abstract class WeaponAddAttackPrepareValue : CombatSkillEffectBase
	{
		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06004315 RID: 17173
		protected abstract int RequireWeaponSubType { get; }

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06004316 RID: 17174
		protected abstract int DirectSrcWeaponSubType { get; }

		// Token: 0x06004317 RID: 17175 RVA: 0x00269E53 File Offset: 0x00268053
		private static short GetItemSubType(ItemKey itemKey)
		{
			return ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId);
		}

		// Token: 0x06004318 RID: 17176 RVA: 0x00269E66 File Offset: 0x00268066
		protected WeaponAddAttackPrepareValue()
		{
		}

		// Token: 0x06004319 RID: 17177 RVA: 0x00269E70 File Offset: 0x00268070
		protected WeaponAddAttackPrepareValue(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x0600431A RID: 17178 RVA: 0x00269E7D File Offset: 0x0026807D
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600431B RID: 17179 RVA: 0x00269E92 File Offset: 0x00268092
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600431C RID: 17180 RVA: 0x00269EA8 File Offset: 0x002680A8
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					this.DoChangeWeaponCd(context);
					this.DoChangeWeapon(context);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0600431D RID: 17181 RVA: 0x00269EFC File Offset: 0x002680FC
		private void DoChangeWeaponCd(DataContext context)
		{
			CombatCharacter affectChar = base.IsDirect ? base.CombatChar : DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			ItemKey[] weapons = affectChar.GetWeapons();
			int currWeaponIndex = affectChar.GetUsingWeaponIndex();
			List<int> pool = ObjectPool<List<int>>.Instance.Get();
			pool.Clear();
			for (int i = 0; i < 3; i++)
			{
				bool flag = i == currWeaponIndex;
				if (!flag)
				{
					ItemKey weaponKey = weapons[i];
					bool flag2 = !weaponKey.IsValid();
					if (!flag2)
					{
						CombatWeaponData weaponData = affectChar.GetWeaponData(i);
						bool flag3 = weaponData.GetDurability() > 0 && (!base.IsDirect || weaponData.GetCdFrame() > 0);
						if (flag3)
						{
							pool.Add(i);
						}
					}
				}
			}
			foreach (int weaponIndex in RandomUtils.GetRandomUnrepeated<int>(context.Random, 2, pool, null))
			{
				DomainManager.Combat.ChangeWeaponCd(context, affectChar, weaponIndex, base.IsDirect ? -50 : 50);
				base.ShowSpecialEffectTipsOnceInFrame(0);
			}
			ObjectPool<List<int>>.Instance.Return(pool);
		}

		// Token: 0x0600431E RID: 17182 RVA: 0x0026A054 File Offset: 0x00268254
		private void DoChangeWeapon(DataContext context)
		{
			int usingWeaponIndex = base.CombatChar.GetUsingWeaponIndex();
			ItemKey[] weapons = base.CombatChar.GetWeapons();
			ItemKey currWeaponKey = weapons[usingWeaponIndex];
			bool canAffect = currWeaponKey.IsValid() && (int)WeaponAddAttackPrepareValue.GetItemSubType(currWeaponKey) == (base.IsDirect ? this.DirectSrcWeaponSubType : this.RequireWeaponSubType);
			List<int> pool = ObjectPool<List<int>>.Instance.Get();
			pool.Clear();
			bool flag = canAffect && base.IsDirect;
			if (flag)
			{
				bool canSwitchToRequireSubType = false;
				for (int i = 0; i < 3; i++)
				{
					bool flag2 = i == usingWeaponIndex;
					if (!flag2)
					{
						ItemKey weaponKey = weapons[i];
						bool flag3 = !weaponKey.IsValid() || (int)WeaponAddAttackPrepareValue.GetItemSubType(weaponKey) != this.RequireWeaponSubType;
						if (!flag3)
						{
							CombatWeaponData weaponData = DomainManager.Combat.GetElement_WeaponDataDict(weaponKey.Id);
							bool flag4 = weaponData.GetDurability() > 0 && weaponData.NotInAnyCd;
							if (flag4)
							{
								canSwitchToRequireSubType = true;
								pool.Add(i);
							}
						}
					}
				}
				canAffect = canSwitchToRequireSubType;
			}
			bool flag5 = canAffect;
			if (flag5)
			{
				bool flag6 = base.IsDirect && pool.Count > 0;
				if (flag6)
				{
					DomainManager.Combat.ChangeWeapon(context, pool[context.Random.Next(0, pool.Count)], base.CombatChar.IsAlly, true);
				}
				bool flag7 = !base.IsDirect && DomainManager.Combat.InAttackRange(base.CombatChar);
				if (flag7)
				{
					base.CombatChar.NeedNormalAttackSkipPrepare++;
					base.ShowSpecialEffectTips(1);
				}
			}
			ObjectPool<List<int>>.Instance.Return(pool);
		}

		// Token: 0x040013E3 RID: 5091
		private const int ChangeWeaponCdCount = 2;

		// Token: 0x040013E4 RID: 5092
		private const int FreeNormalAttackCount = 1;
	}
}
