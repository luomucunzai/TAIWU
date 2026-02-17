using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Blade
{
	// Token: 0x0200052B RID: 1323
	public class DaLiangYiZuiDao : CombatSkillEffectBase
	{
		// Token: 0x06003F63 RID: 16227 RVA: 0x00259A6F File Offset: 0x00257C6F
		public DaLiangYiZuiDao()
		{
		}

		// Token: 0x06003F64 RID: 16228 RVA: 0x00259A79 File Offset: 0x00257C79
		public DaLiangYiZuiDao(CombatSkillKey skillKey) : base(skillKey, 14205, -1)
		{
		}

		// Token: 0x06003F65 RID: 16229 RVA: 0x00259A8A File Offset: 0x00257C8A
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003F66 RID: 16230 RVA: 0x00259AB1 File Offset: 0x00257CB1
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003F67 RID: 16231 RVA: 0x00259AD8 File Offset: 0x00257CD8
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker.GetId() != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.AppendAffectedData(context, base.CharacterId, 69, EDataModifyType.AddPercent, base.SkillTemplateId);
			}
		}

		// Token: 0x06003F68 RID: 16232 RVA: 0x00259B24 File Offset: 0x00257D24
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003F69 RID: 16233 RVA: 0x00259B5C File Offset: 0x00257D5C
		public unsafe override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CombatSkillId != base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 69 && dataKey.CustomParam0 == (base.IsDirect ? 0 : 1);
				if (flag2)
				{
					int addValue = (int)(10 * base.CombatChar.GetInjuries().Get((sbyte)dataKey.CustomParam1, !base.IsDirect) * ((*this.CharObj.GetEatingItems()).ContainsWine() ? 2 : 1));
					bool flag3 = addValue > 0;
					if (flag3)
					{
						base.ShowSpecialEffectTips(0);
					}
					result = addValue;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x040012AD RID: 4781
		private const sbyte AddDamagePercent = 10;
	}
}
