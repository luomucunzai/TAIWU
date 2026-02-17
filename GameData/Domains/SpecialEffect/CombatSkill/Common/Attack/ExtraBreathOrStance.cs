using System;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x0200059A RID: 1434
	public abstract class ExtraBreathOrStance : CombatSkillEffectBase
	{
		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06004291 RID: 17041
		protected abstract bool IsBreath { get; }

		// Token: 0x06004292 RID: 17042 RVA: 0x00267826 File Offset: 0x00265A26
		private static bool IsAttack(short skillId)
		{
			return skillId >= 0 && Config.CombatSkill.Instance[skillId].EquipType == 1;
		}

		// Token: 0x06004293 RID: 17043 RVA: 0x00267842 File Offset: 0x00265A42
		protected ExtraBreathOrStance()
		{
		}

		// Token: 0x06004294 RID: 17044 RVA: 0x0026784C File Offset: 0x00265A4C
		protected ExtraBreathOrStance(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06004295 RID: 17045 RVA: 0x0026785C File Offset: 0x00265A5C
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(this.IsBreath ? 173 : 174, EDataModifyType.Add, -1);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_CastSkillUseExtraBreathOrStance(new Events.OnCastSkillUseExtraBreathOrStance(this.OnCastSkillUseExtraBreathOrStance));
			Events.RegisterHandler_CostBreathAndStance(new Events.OnCostBreathAndStance(this.OnCostBreathAndStance));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004296 RID: 17046 RVA: 0x002678D8 File Offset: 0x00265AD8
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_CastSkillUseExtraBreathOrStance(new Events.OnCastSkillUseExtraBreathOrStance(this.OnCastSkillUseExtraBreathOrStance));
			Events.UnRegisterHandler_CostBreathAndStance(new Events.OnCostBreathAndStance(this.OnCostBreathAndStance));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			base.OnDisable(context);
		}

		// Token: 0x06004297 RID: 17047 RVA: 0x00267938 File Offset: 0x00265B38
		private void OnCombatBegin(DataContext context)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.AppendAffectedData(context, 44, EDataModifyType.AddPercent, -1);
				base.AppendAffectedData(context, 45, EDataModifyType.AddPercent, -1);
			}
			else
			{
				base.AppendAffectedAllEnemyData(context, 46, EDataModifyType.AddPercent, -1);
				base.AppendAffectedAllEnemyData(context, 47, EDataModifyType.AddPercent, -1);
			}
		}

		// Token: 0x06004298 RID: 17048 RVA: 0x00267988 File Offset: 0x00265B88
		private void OnCastSkillUseExtraBreathOrStance(DataContext context, int charId, short skillId, int extraBreath, int extraStance)
		{
			bool flag = charId != base.CharacterId || (this.IsBreath ? (extraBreath <= 0) : (extraStance <= 0));
			if (!flag)
			{
				this._costingEffectCount = (this.IsBreath ? extraBreath : extraStance);
				base.ReduceEffectCount(this._costingEffectCount);
				this.InvalidateAffectDataCache(context);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06004299 RID: 17049 RVA: 0x002679F4 File Offset: 0x00265BF4
		private void OnCostBreathAndStance(DataContext context, int charId, bool isAlly, int costBreath, int costStance, short skillId)
		{
			bool flag = !this.SkillKey.IsMatch(charId, skillId);
			if (!flag)
			{
				int breathPercent = CValuePercent.ParseInt(costBreath, 30000);
				int stancePercent = CValuePercent.ParseInt(costStance, 4000);
				this._costingBreathOrStancePercent = (this.IsBreath ? stancePercent : breathPercent);
			}
		}

		// Token: 0x0600429A RID: 17050 RVA: 0x00267A48 File Offset: 0x00265C48
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId;
			if (!flag)
			{
				int effectCount = this._costingBreathOrStancePercent * (int)power / 50;
				this._costingBreathOrStancePercent = 0;
				bool flag2 = skillId == base.SkillTemplateId && effectCount > 0;
				if (flag2)
				{
					base.AddEffectCount(effectCount);
				}
				bool flag3 = this._costingEffectCount <= 0;
				if (!flag3)
				{
					this._costingEffectCount = 0;
					this.InvalidateAffectDataCache(context);
				}
			}
		}

		// Token: 0x0600429B RID: 17051 RVA: 0x00267ABC File Offset: 0x00265CBC
		private void InvalidateAffectDataCache(DataContext context)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.InvalidateCache(context, 44);
				base.InvalidateCache(context, 45);
			}
			else
			{
				base.InvalidateAllEnemyCache(context, 46);
				base.InvalidateAllEnemyCache(context, 47);
			}
		}

		// Token: 0x0600429C RID: 17052 RVA: 0x00267B04 File Offset: 0x00265D04
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			ushort fieldId = dataKey.FieldId;
			bool flag = fieldId - 46 <= 1;
			bool flag2 = flag;
			int result;
			if (flag2)
			{
				result = this._costingEffectCount * ExtraBreathOrStance.ReduceDefendPercent;
			}
			else
			{
				bool flag3 = dataKey.CharId != base.CharacterId;
				if (flag3)
				{
					result = 0;
				}
				else
				{
					bool flag4 = dataKey.FieldId == (this.IsBreath ? 173 : 174) && ExtraBreathOrStance.IsAttack(dataKey.CombatSkillId);
					if (flag4)
					{
						result = base.EffectCount;
					}
					else
					{
						fieldId = dataKey.FieldId;
						flag = (fieldId - 44 <= 1);
						bool flag5 = flag;
						if (flag5)
						{
							result = this._costingEffectCount * ExtraBreathOrStance.AddAttackPercent;
						}
						else
						{
							result = 0;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x040013B0 RID: 5040
		private static readonly CValuePercent AddAttackPercent = 50;

		// Token: 0x040013B1 RID: 5041
		private static readonly CValuePercent ReduceDefendPercent = -50;

		// Token: 0x040013B2 RID: 5042
		private int _costingBreathOrStancePercent;

		// Token: 0x040013B3 RID: 5043
		private int _costingEffectCount;
	}
}
