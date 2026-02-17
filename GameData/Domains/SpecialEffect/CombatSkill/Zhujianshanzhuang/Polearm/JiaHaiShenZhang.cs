using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Polearm
{
	// Token: 0x020001C6 RID: 454
	public class JiaHaiShenZhang : PolearmUnlockEffectBase
	{
		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06002CE0 RID: 11488 RVA: 0x00209BDA File Offset: 0x00207DDA
		private static CValuePercent AddDurabilityPercent
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06002CE1 RID: 11489 RVA: 0x00209BE3 File Offset: 0x00207DE3
		private CValuePercent CastAddUnlockPercent
		{
			get
			{
				return base.IsDirectOrReverseEffectDoubling ? 16 : 8;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06002CE2 RID: 11490 RVA: 0x00209BF7 File Offset: 0x00207DF7
		protected override IEnumerable<sbyte> RequireMainAttributeTypes { get; } = new sbyte[]
		{
			3,
			4,
			2
		};

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06002CE3 RID: 11491 RVA: 0x00209BFF File Offset: 0x00207DFF
		protected override int RequireMainAttributeValue
		{
			get
			{
				return 55;
			}
		}

		// Token: 0x06002CE4 RID: 11492 RVA: 0x00209C03 File Offset: 0x00207E03
		public JiaHaiShenZhang()
		{
		}

		// Token: 0x06002CE5 RID: 11493 RVA: 0x00209C2F File Offset: 0x00207E2F
		public JiaHaiShenZhang(CombatSkillKey skillKey) : base(skillKey, 9306)
		{
		}

		// Token: 0x06002CE6 RID: 11494 RVA: 0x00209C61 File Offset: 0x00207E61
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
		}

		// Token: 0x06002CE7 RID: 11495 RVA: 0x00209C7E File Offset: 0x00207E7E
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
			base.OnDisable(context);
		}

		// Token: 0x06002CE8 RID: 11496 RVA: 0x00209C9C File Offset: 0x00207E9C
		protected override void OnCastAddUnlockAttackValue(DataContext context, CValuePercent power)
		{
			base.OnCastAddUnlockAttackValue(context, power);
			bool flag = !base.IsReverseOrUsingDirectWeapon;
			if (!flag)
			{
				base.CombatChar.ChangeAllUnlockAttackValue(context, this.CastAddUnlockPercent * power);
				base.ShowSpecialEffectTipsOnceInFrame(0);
			}
		}

		// Token: 0x06002CE9 RID: 11497 RVA: 0x00209CE4 File Offset: 0x00207EE4
		protected override void DoAffect(DataContext context, int weaponIndex)
		{
			ItemKey[] equipment = base.CombatChar.GetCharacter().GetEquipment();
			int i = 0;
			while (i < equipment.Length)
			{
				ItemKey key = equipment[i];
				int old;
				bool flag = key.IsValid() && DomainManager.Combat.EquipmentOldDurability.TryGetValue(key, out old);
				if (flag)
				{
					short current = DomainManager.Item.GetBaseItem(key).GetCurrDurability();
					bool flag2 = (int)current >= old;
					if (!flag2)
					{
						int addValue = Math.Max((old - (int)current) * JiaHaiShenZhang.AddDurabilityPercent, 1);
						base.ChangeDurability(context, base.CombatChar, key, addValue);
						base.ShowSpecialEffectTipsOnceInFrame(1);
					}
				}
				IL_9D:
				i++;
				continue;
				goto IL_9D;
			}
			List<ItemKey> pool = ObjectPool<List<ItemKey>>.Instance.Get();
			foreach (ItemKey key2 in base.EnemyChar.GetCharacter().GetEquipment())
			{
				bool flag3 = !pool.Contains(key2) && this.IsKeyCanCrippledCreate(key2);
				if (flag3)
				{
					pool.Add(key2);
				}
			}
			bool flag4 = pool.Count > 0;
			if (flag4)
			{
				ItemKey key3 = pool.GetRandom(context.Random);
				SpecialEffectItem effectConfig = SpecialEffect.Instance[base.EffectId];
				short effectId = effectConfig.RawCreateEffect;
				DomainManager.Item.AddExternEquipmentEffect(context, key3, effectId);
				DomainManager.SpecialEffect.AddEquipmentEffect(context, base.EnemyChar.GetId(), key3, effectId);
				base.EnemyChar.GetCharacter().SetEquipment(base.EnemyChar.GetCharacter().GetEquipment(), context);
				this._addedEffectKeys.Add(key3);
				DomainManager.Combat.ShowSpecialEffectTips(base.CharacterId, (int)effectConfig.RawCreateTips, 0);
			}
			ObjectPool<List<ItemKey>>.Instance.Return(pool);
		}

		// Token: 0x06002CEA RID: 11498 RVA: 0x00209EB8 File Offset: 0x002080B8
		private void OnCombatSettlement(DataContext context, sbyte combatStatus)
		{
			short effectId = SpecialEffect.Instance[base.EffectId].RawCreateEffect;
			foreach (ItemKey key in this._addedEffectKeys)
			{
				DomainManager.Item.RemoveExternEquipmentEffect(context, key, effectId);
			}
			this._addedEffectKeys.Clear();
		}

		// Token: 0x06002CEB RID: 11499 RVA: 0x00209F38 File Offset: 0x00208138
		private bool IsKeyCanCrippledCreate(ItemKey key)
		{
			bool flag = !key.IsValid() || this._addedEffectKeys.Contains(key);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				sbyte itemType = key.ItemType;
				if (!true)
				{
				}
				bool flag2;
				if (itemType != 0)
				{
					flag2 = (itemType == 1 && Config.Armor.Instance[key.TemplateId].AllowCrippledCreate);
				}
				else
				{
					flag2 = Config.Weapon.Instance[key.TemplateId].AllowCrippledCreate;
				}
				if (!true)
				{
				}
				result = flag2;
			}
			return result;
		}

		// Token: 0x04000D87 RID: 3463
		private readonly List<ItemKey> _addedEffectKeys = new List<ItemKey>();
	}
}
