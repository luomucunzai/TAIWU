using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.RawCreate
{
	// Token: 0x02000199 RID: 409
	public class XianYuanShenJie : RawCreateEquipmentBase
	{
		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06002BD6 RID: 11222 RVA: 0x00206153 File Offset: 0x00204353
		protected override int ReduceDurabilityValue
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x06002BD7 RID: 11223 RVA: 0x00206156 File Offset: 0x00204356
		public XianYuanShenJie()
		{
		}

		// Token: 0x06002BD8 RID: 11224 RVA: 0x00206160 File Offset: 0x00204360
		public XianYuanShenJie(int charId, ItemKey itemKey) : base(charId, itemKey, 30204)
		{
		}

		// Token: 0x06002BD9 RID: 11225 RVA: 0x00206171 File Offset: 0x00204371
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_NormalAttackCalcCriticalEnd(new Events.OnNormalAttackCalcCriticalEnd(this.OnNormalAttackCalcCriticalEnd));
		}

		// Token: 0x06002BDA RID: 11226 RVA: 0x0020618E File Offset: 0x0020438E
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackCalcCriticalEnd(new Events.OnNormalAttackCalcCriticalEnd(this.OnNormalAttackCalcCriticalEnd));
			base.OnDisable(context);
		}

		// Token: 0x06002BDB RID: 11227 RVA: 0x002061AC File Offset: 0x002043AC
		private void OnNormalAttackCalcCriticalEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, bool critical)
		{
			bool flag = attacker.GetId() != base.CharacterId || !critical || !base.CanAffect || attacker.PursueAttackCount > 0;
			if (!flag)
			{
				bool flag2 = attacker.IsUnlockAttack ? (attacker.UnlockWeapon.GetItemKey() != this.EquipItemKey) : (DomainManager.Combat.GetUsingWeaponKey(attacker) != this.EquipItemKey);
				if (!flag2)
				{
					sbyte level = (sbyte)context.Random.Next(0, 3);
					bool flaw = context.Random.CheckPercentProb(50);
					bool flag3 = flaw;
					if (flag3)
					{
						DomainManager.Combat.AddFlaw(context, defender, level, CombatSkillKey.Invalid, -1, 1, true);
					}
					else
					{
						DomainManager.Combat.AddAcupoint(context, defender, level, CombatSkillKey.Invalid, -1, 1, true);
					}
				}
			}
		}
	}
}
