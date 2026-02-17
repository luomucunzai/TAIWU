using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Music
{
	// Token: 0x020005CA RID: 1482
	public class YunShuiYin : CombatSkillEffectBase
	{
		// Token: 0x060043E2 RID: 17378 RVA: 0x0026D2FC File Offset: 0x0026B4FC
		public YunShuiYin()
		{
		}

		// Token: 0x060043E3 RID: 17379 RVA: 0x0026D306 File Offset: 0x0026B506
		public YunShuiYin(CombatSkillKey skillKey) : base(skillKey, 3300, -1)
		{
		}

		// Token: 0x060043E4 RID: 17380 RVA: 0x0026D317 File Offset: 0x0026B517
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(302, EDataModifyType.Add, -1);
			Events.RegisterHandler_CostBreathAndStance(new Events.OnCostBreathAndStance(this.OnCostBreathAndStance));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060043E5 RID: 17381 RVA: 0x0026D34C File Offset: 0x0026B54C
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CostBreathAndStance(new Events.OnCostBreathAndStance(this.OnCostBreathAndStance));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060043E6 RID: 17382 RVA: 0x0026D374 File Offset: 0x0026B574
		private void OnCostBreathAndStance(DataContext context, int charId, bool isAlly, int costBreath, int costStance, short skillId)
		{
			bool flag = charId != base.CharacterId || base.EffectCount <= 0 || (costBreath <= 0 && costStance <= 0);
			if (!flag)
			{
				base.ReduceEffectCount(1);
				short maxBreath = 30000;
				short maxStance = 4000;
				CValuePercent percent = base.IsDirect ? CValuePercent.Parse(costBreath, (int)maxBreath) : CValuePercent.Parse(costStance, (int)maxStance);
				int addValue = (int)(base.IsDirect ? maxStance : maxBreath) * percent * YunShuiYin.ChangeBreathAndStanceCost;
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					base.ChangeStanceValue(context, base.CombatChar, addValue);
				}
				else
				{
					base.ChangeBreathValue(context, base.CombatChar, addValue);
				}
				base.ShowSpecialEffectTips(1);
				DomainManager.Combat.UpdateSkillCanUse(context, base.CombatChar);
			}
		}

		// Token: 0x060043E7 RID: 17383 RVA: 0x0026D444 File Offset: 0x0026B644
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = !this.SkillKey.IsMatch(charId, skillId) || !base.PowerMatchAffectRequire((int)power, 0);
			if (!flag)
			{
				base.AddMaxEffectCount(true);
				base.ShowSpecialEffectTips(0);
				DomainManager.Combat.UpdateSkillCanUse(context, base.CombatChar);
			}
		}

		// Token: 0x060043E8 RID: 17384 RVA: 0x0026D49C File Offset: 0x0026B69C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId == base.CharacterId && dataKey.FieldId == 302 && base.EffectCount > 0;
			int result;
			if (flag)
			{
				result = (base.IsDirect ? 1 : -1);
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x04001429 RID: 5161
		private static readonly CValuePercent ChangeBreathAndStanceCost = 25;
	}
}
