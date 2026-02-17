using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Music
{
	// Token: 0x02000268 RID: 616
	public class DuanHunYouYinQu : CombatSkillEffectBase
	{
		// Token: 0x06003066 RID: 12390 RVA: 0x00216CE7 File Offset: 0x00214EE7
		public DuanHunYouYinQu()
		{
		}

		// Token: 0x06003067 RID: 12391 RVA: 0x00216CF1 File Offset: 0x00214EF1
		public DuanHunYouYinQu(CombatSkillKey skillKey) : base(skillKey, 8302, -1)
		{
		}

		// Token: 0x06003068 RID: 12392 RVA: 0x00216D04 File Offset: 0x00214F04
		public override void OnEnable(DataContext context)
		{
			sbyte happinessType = HappinessType.GetHappinessType((base.IsDirect ? base.CurrEnemyChar : base.CombatChar).GetHappiness());
			bool flag = happinessType < 3;
			if (flag)
			{
				this._addPower = (int)(10 * (3 - happinessType));
				base.CreateAffectedData(199, EDataModifyType.AddPercent, base.SkillTemplateId);
				base.ShowSpecialEffectTips(0);
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003069 RID: 12393 RVA: 0x00216D76 File Offset: 0x00214F76
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600306A RID: 12394 RVA: 0x00216D8C File Offset: 0x00214F8C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = power > 0;
				if (flag2)
				{
					DomainManager.Combat.AddCombatState(context, base.IsDirect ? base.CurrEnemyChar : base.CombatChar, 0, 42, (int)(power * 2));
				}
				sbyte happinessType = HappinessType.GetHappinessType((base.IsDirect ? base.CurrEnemyChar : base.CombatChar).GetHappiness());
				bool flag3 = base.PowerMatchAffectRequire((int)power, 0) && happinessType < 3;
				if (flag3)
				{
					int injuryCount = (int)(3 - happinessType);
					base.CurrEnemyChar.WorsenRepeatableInjury(context, injuryCount);
					base.ShowSpecialEffectTips(1);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0600306B RID: 12395 RVA: 0x00216E4C File Offset: 0x0021504C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.SkillKey != this.SkillKey;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 199;
				if (flag2)
				{
					result = this._addPower;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000E59 RID: 3673
		private const sbyte AddPowerUnit = 10;

		// Token: 0x04000E5A RID: 3674
		private int _addPower;
	}
}
