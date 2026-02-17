using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.DefenseAndAssist
{
	// Token: 0x02000243 RID: 579
	public class TianMoTong : DefenseSkillBase
	{
		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06002FCC RID: 12236 RVA: 0x00214688 File Offset: 0x00212888
		private sbyte AddPoisonType
		{
			get
			{
				return base.IsDirect ? 3 : 2;
			}
		}

		// Token: 0x06002FCD RID: 12237 RVA: 0x00214696 File Offset: 0x00212896
		public TianMoTong()
		{
		}

		// Token: 0x06002FCE RID: 12238 RVA: 0x002146A0 File Offset: 0x002128A0
		public TianMoTong(CombatSkillKey skillKey) : base(skillKey, 15707)
		{
		}

		// Token: 0x06002FCF RID: 12239 RVA: 0x002146B0 File Offset: 0x002128B0
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(102, EDataModifyType.AddPercent, -1);
			Events.RegisterHandler_BounceInjury(new Events.OnBounceInjury(this.OnBounceInjury));
		}

		// Token: 0x06002FD0 RID: 12240 RVA: 0x002146D8 File Offset: 0x002128D8
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_BounceInjury(new Events.OnBounceInjury(this.OnBounceInjury));
			base.OnDisable(context);
		}

		// Token: 0x06002FD1 RID: 12241 RVA: 0x002146F8 File Offset: 0x002128F8
		private void OnBounceInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount)
		{
			bool flag = attackerId != base.CharacterId || !base.CanAffect;
			if (!flag)
			{
				OuterAndInnerInts bouncePower = base.CombatChar.GetBouncePower(50);
				DomainManager.Combat.AddPoison(context, base.CombatChar, base.CurrEnemyChar, this.AddPoisonType, 2, 120 * (base.IsDirect ? bouncePower.Outer : bouncePower.Inner) / 100, base.SkillTemplateId, true, true, default(ItemKey), false, false, false);
				DomainManager.Combat.AddToCheckFallenSet(base.CurrEnemyChar.GetId());
				base.ShowSpecialEffectTips(1);
				bool flag2 = base.CurrEnemyChar.WorsenAllInjury(context, !base.IsDirect, WorsenConstants.LowPercent);
				if (flag2)
				{
					base.ShowSpecialEffectTips(2);
				}
			}
		}

		// Token: 0x06002FD2 RID: 12242 RVA: 0x002147C8 File Offset: 0x002129C8
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 102 && dataKey.CustomParam0 == (base.IsDirect ? 0 : 1);
				if (flag2)
				{
					base.ShowSpecialEffectTips(0);
					result = -60;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000E27 RID: 3623
		private const sbyte ReduceDamagePercent = -60;

		// Token: 0x04000E28 RID: 3624
		private const int AddPoison = 120;

		// Token: 0x04000E29 RID: 3625
		private const int AddPoisonLevel = 2;
	}
}
