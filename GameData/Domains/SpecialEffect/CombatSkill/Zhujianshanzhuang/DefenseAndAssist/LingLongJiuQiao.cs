using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.DefenseAndAssist
{
	// Token: 0x020001D2 RID: 466
	public class LingLongJiuQiao : DefenseSkillBase
	{
		// Token: 0x06002D33 RID: 11571 RVA: 0x0020A91A File Offset: 0x00208B1A
		public LingLongJiuQiao()
		{
		}

		// Token: 0x06002D34 RID: 11572 RVA: 0x0020A924 File Offset: 0x00208B24
		public LingLongJiuQiao(CombatSkillKey skillKey) : base(skillKey, 9705)
		{
		}

		// Token: 0x06002D35 RID: 11573 RVA: 0x0020A934 File Offset: 0x00208B34
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06002D36 RID: 11574 RVA: 0x0020A951 File Offset: 0x00208B51
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			base.OnDisable(context);
		}

		// Token: 0x06002D37 RID: 11575 RVA: 0x0020A970 File Offset: 0x00208B70
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = hit || pursueIndex > 0 || defender != base.CombatChar || !base.CanAffect;
			if (!flag)
			{
				this.DoEffect(context);
			}
		}

		// Token: 0x06002D38 RID: 11576 RVA: 0x0020A9AC File Offset: 0x00208BAC
		private void DoEffect(DataContext context)
		{
			ItemKey[] equipments = this.CharObj.GetEquipment();
			List<ItemKey> pool = ObjectPool<List<ItemKey>>.Instance.Get();
			pool.Clear();
			for (int i = 8; i <= 10; i++)
			{
				ItemKey key = equipments[i];
				bool flag = key.IsValid() && DomainManager.Item.GetElement_Accessories(key.Id).GetCurrDurability() > 0;
				if (flag)
				{
					pool.Add(key);
				}
			}
			bool flag2 = pool.Count > 0;
			if (flag2)
			{
				ItemKey key2 = pool.GetRandom(context.Random);
				bool affected = this.AddRandomUnlockValue(context, ItemTemplateHelper.GetGrade(key2.ItemType, key2.TemplateId));
				affected = (this.AddOrReduceDurability(context) || affected);
				bool flag3 = affected;
				if (flag3)
				{
					base.ChangeDurability(context, base.CombatChar, key2, -1);
				}
			}
			ObjectPool<List<ItemKey>>.Instance.Return(pool);
		}

		// Token: 0x06002D39 RID: 11577 RVA: 0x0020AA98 File Offset: 0x00208C98
		private bool AddOrReduceDurability(DataContext context)
		{
			CombatCharacter affectChar = base.IsDirect ? base.CombatChar : base.CurrEnemyChar;
			ItemKey[] equipments = affectChar.GetCharacter().GetEquipment();
			List<ItemKey> pool = ObjectPool<List<ItemKey>>.Instance.Get();
			foreach (sbyte slot in LingLongJiuQiao.TargetEquipmentSlots)
			{
				ItemKey key = equipments[(int)slot];
				bool flag = !key.IsValid();
				if (!flag)
				{
					ItemBase item = DomainManager.Item.GetBaseItem(key);
					bool flag2 = base.IsDirect ? (item.GetCurrDurability() < item.GetMaxDurability()) : (item.GetCurrDurability() > 0);
					if (flag2)
					{
						pool.Add(key);
					}
				}
			}
			bool affected = pool.Count > 0;
			bool flag3 = affected;
			if (flag3)
			{
				ItemKey key2 = pool.GetRandom(context.Random);
				int delta = 3 * (base.IsDirect ? 1 : -1);
				base.ChangeDurability(context, affectChar, key2, delta);
				base.ShowSpecialEffectTips(1);
			}
			ObjectPool<List<ItemKey>>.Instance.Return(pool);
			return affected;
		}

		// Token: 0x06002D3A RID: 11578 RVA: 0x0020ABCC File Offset: 0x00208DCC
		private bool AddRandomUnlockValue(DataContext context, sbyte grade)
		{
			int addValue = GlobalConfig.Instance.UnlockAttackUnit * (int)(grade + 1);
			List<int> pool = ObjectPool<List<int>>.Instance.Get();
			pool.Clear();
			List<int> unlockValues = base.CombatChar.GetUnlockPrepareValue();
			for (int i = 0; i < 3; i++)
			{
				bool flag = unlockValues[i] < GlobalConfig.Instance.UnlockAttackUnit && base.CombatChar.CanUnlockAttackByConfig(i);
				if (flag)
				{
					pool.Add(i);
				}
			}
			bool affected = pool.Count > 0;
			bool flag2 = affected;
			if (flag2)
			{
				int index = pool.GetRandom(context.Random);
				base.CombatChar.ChangeUnlockAttackValue(context, index, addValue);
				base.ShowSpecialEffectTips(0);
			}
			ObjectPool<List<int>>.Instance.Return(pool);
			return affected;
		}

		// Token: 0x04000D94 RID: 3476
		private const int AddOrReduceDurabilityValue = 3;

		// Token: 0x04000D95 RID: 3477
		private static readonly IReadOnlyList<sbyte> TargetEquipmentSlots = new sbyte[]
		{
			0,
			1,
			2,
			3,
			5,
			6,
			7
		};
	}
}
