using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.EquipmentEffect;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Special
{
	// Token: 0x02000131 RID: 305
	public class GuiJi : EquipmentEffectBase
	{
		// Token: 0x06002A62 RID: 10850 RVA: 0x002026FF File Offset: 0x002008FF
		public GuiJi()
		{
		}

		// Token: 0x06002A63 RID: 10851 RVA: 0x00202709 File Offset: 0x00200909
		public GuiJi(int charId, ItemKey itemKey) : base(charId, itemKey, 41000, false)
		{
		}

		// Token: 0x06002A64 RID: 10852 RVA: 0x0020271B File Offset: 0x0020091B
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.OnAddDirectDamageValue));
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06002A65 RID: 10853 RVA: 0x0020274A File Offset: 0x0020094A
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.OnAddDirectDamageValue));
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			base.OnDisable(context);
		}

		// Token: 0x06002A66 RID: 10854 RVA: 0x0020277C File Offset: 0x0020097C
		private void OnAddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId)
		{
			bool flag = attackerId != base.CharacterId || defenderId == base.CharacterId || combatSkillId >= 0 || !base.CombatChar.GetChangeTrickAttack() || !base.IsCurrWeapon();
			if (!flag)
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

		// Token: 0x06002A67 RID: 10855 RVA: 0x002027F4 File Offset: 0x002009F4
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

		// Token: 0x04000CF5 RID: 3317
		private const int ExtraAttackBodyPartCount = 1;

		// Token: 0x04000CF6 RID: 3318
		private sbyte _exceptBodyPart;

		// Token: 0x04000CF7 RID: 3319
		private OuterAndInnerInts _damageValues;
	}
}
