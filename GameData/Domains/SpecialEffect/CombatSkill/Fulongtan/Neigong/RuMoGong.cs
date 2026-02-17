using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Neigong
{
	// Token: 0x02000517 RID: 1303
	public class RuMoGong : CombatSkillEffectBase
	{
		// Token: 0x06003EEE RID: 16110 RVA: 0x0025782C File Offset: 0x00255A2C
		public RuMoGong()
		{
		}

		// Token: 0x06003EEF RID: 16111 RVA: 0x00257836 File Offset: 0x00255A36
		public RuMoGong(CombatSkillKey skillKey) : base(skillKey, 14006, -1)
		{
		}

		// Token: 0x06003EF0 RID: 16112 RVA: 0x00257848 File Offset: 0x00255A48
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 102 : 69, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			Events.RegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
		}

		// Token: 0x06003EF1 RID: 16113 RVA: 0x002578AF File Offset: 0x00255AAF
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
		}

		// Token: 0x06003EF2 RID: 16114 RVA: 0x002578D8 File Offset: 0x00255AD8
		private void OnNormalAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex)
		{
			bool flag = base.CombatChar == (base.IsDirect ? defender : attacker) && DomainManager.Combat.InAttackRange(attacker) && base.CombatChar.GetDefeatMarkCollection().FatalDamageMarkCount > 0 && pursueIndex == 0;
			if (flag)
			{
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003EF3 RID: 16115 RVA: 0x00257930 File Offset: 0x00255B30
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = base.CombatChar == (base.IsDirect ? defender : attacker) && DomainManager.Combat.InAttackRange(attacker) && base.CombatChar.GetDefeatMarkCollection().FatalDamageMarkCount > 0;
			if (flag)
			{
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003EF4 RID: 16116 RVA: 0x00257984 File Offset: 0x00255B84
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
				int fatalMarkCount = base.CombatChar.GetDefeatMarkCollection().FatalDamageMarkCount;
				bool flag2 = dataKey.FieldId == 102;
				if (flag2)
				{
					result = -3 * fatalMarkCount;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 69;
					if (flag3)
					{
						result = 3 * fatalMarkCount;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x0400128D RID: 4749
		private const sbyte DirectReducePercentPerMark = -3;

		// Token: 0x0400128E RID: 4750
		private const sbyte ReverseAddPercentPerMark = 3;
	}
}
