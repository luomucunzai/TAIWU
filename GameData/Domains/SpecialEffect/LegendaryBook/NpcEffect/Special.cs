using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.LegendaryBook.NpcEffect
{
	// Token: 0x0200014C RID: 332
	public class Special : FeatureEffectBase
	{
		// Token: 0x06002AC7 RID: 10951 RVA: 0x00203A18 File Offset: 0x00201C18
		public Special()
		{
		}

		// Token: 0x06002AC8 RID: 10952 RVA: 0x00203A22 File Offset: 0x00201C22
		public Special(int charId, short featureId) : base(charId, featureId, 41410)
		{
		}

		// Token: 0x06002AC9 RID: 10953 RVA: 0x00203A34 File Offset: 0x00201C34
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.OnAddDirectDamageValue));
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06002ACA RID: 10954 RVA: 0x00203A97 File Offset: 0x00201C97
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.OnAddDirectDamageValue));
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			base.OnDisable(context);
		}

		// Token: 0x06002ACB RID: 10955 RVA: 0x00203AC8 File Offset: 0x00201CC8
		private void OnAddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId)
		{
			bool flag = attackerId != base.CharacterId || defenderId == base.CharacterId || combatSkillId >= 0 || !base.CombatChar.GetChangeTrickAttack();
			if (!flag)
			{
				GameData.Domains.Item.Weapon usingWeapon = DomainManager.Combat.GetUsingWeapon(base.CombatChar);
				short subType = usingWeapon.GetItemSubType();
				bool flag2 = !CombatSkillType.Instance[10].LegendaryBookWeaponSlotItemSubTypes.Contains(subType);
				if (!flag2)
				{
					this._exceptBodyPart = bodyPart;
					if (isInner)
					{
						this._damageValues.Inner = this._damageValues.Inner + damageValue;
					}
					else
					{
						this._damageValues.Outer = this._damageValues.Outer + damageValue;
					}
				}
			}
		}

		// Token: 0x06002ACC RID: 10956 RVA: 0x00203B70 File Offset: 0x00201D70
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightback)
		{
			bool flag = attacker.GetId() != base.CharacterId || !this._damageValues.IsNonZero;
			if (!flag)
			{
				List<sbyte> pool = ObjectPool<List<sbyte>>.Instance.Get();
				pool.Clear();
				pool.AddRange(defender.GetAvailableBodyParts());
				pool.Remove(this._exceptBodyPart);
				foreach (sbyte otherSidePart in RandomUtils.GetRandomUnrepeated<sbyte>(context.Random, 1, pool, null))
				{
					DomainManager.Combat.AddInjuryDamageValue(base.CombatChar, defender, otherSidePart, this._damageValues.Outer, this._damageValues.Inner, -1, true);
					base.CombatChar.ApplyChangeTrickFlawOrAcupoint(context, defender, otherSidePart);
				}
				ObjectPool<List<sbyte>>.Instance.Return(pool);
				this._damageValues = new ValueTuple<int, int>(0, 0);
			}
		}

		// Token: 0x06002ACD RID: 10957 RVA: 0x00203C70 File Offset: 0x00201E70
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 69;
				if (flag2)
				{
					bool flag3 = base.CombatChar.GetAutoCastingSkill() || dataKey.CombatSkillId < 0 || CombatSkill.Instance[dataKey.CombatSkillId].Type != 10;
					if (flag3)
					{
						result = 0;
					}
					else
					{
						int addDamage = 180 * (4000 - base.CurrEnemyChar.GetStanceValue()) / 4000;
						result = Math.Min(addDamage, 180);
					}
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000D18 RID: 3352
		private const int ExtraAttackBodyPartCount = 1;

		// Token: 0x04000D19 RID: 3353
		private sbyte _exceptBodyPart;

		// Token: 0x04000D1A RID: 3354
		private OuterAndInnerInts _damageValues;

		// Token: 0x04000D1B RID: 3355
		private const short MaxAddDamage = 180;
	}
}
