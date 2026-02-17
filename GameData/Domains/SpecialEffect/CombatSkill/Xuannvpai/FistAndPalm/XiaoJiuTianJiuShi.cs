using System;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.FistAndPalm
{
	// Token: 0x02000274 RID: 628
	public class XiaoJiuTianJiuShi : CombatSkillEffectBase
	{
		// Token: 0x060030A3 RID: 12451 RVA: 0x00217F0B File Offset: 0x0021610B
		public XiaoJiuTianJiuShi()
		{
		}

		// Token: 0x060030A4 RID: 12452 RVA: 0x00217F15 File Offset: 0x00216115
		public XiaoJiuTianJiuShi(CombatSkillKey skillKey) : base(skillKey, 8103, -1)
		{
		}

		// Token: 0x060030A5 RID: 12453 RVA: 0x00217F26 File Offset: 0x00216126
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_CostBreathAndStance(new Events.OnCostBreathAndStance(this.OnCostBreathAndStance));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x060030A6 RID: 12454 RVA: 0x00217F5F File Offset: 0x0021615F
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_CostBreathAndStance(new Events.OnCostBreathAndStance(this.OnCostBreathAndStance));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x060030A7 RID: 12455 RVA: 0x00217F98 File Offset: 0x00216198
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = !this.IsSrcSkillPerformed;
			if (flag)
			{
				bool flag2 = charId == base.CharacterId && skillId == base.SkillTemplateId;
				if (flag2)
				{
					bool flag3 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag3)
					{
						this.IsSrcSkillPerformed = true;
						base.AppendAffectedData(context, base.CharacterId, 103, EDataModifyType.TotalPercent, -1);
						base.AddMaxEffectCount(true);
					}
					else
					{
						base.RemoveSelf(context);
					}
				}
			}
			else
			{
				bool flag4 = charId == base.CharacterId && skillId == base.SkillTemplateId && base.PowerMatchAffectRequire((int)power, 0);
				if (flag4)
				{
					base.RemoveSelf(context);
				}
				else
				{
					bool flag5 = Config.CombatSkill.Instance[skillId].EquipType == 1 && !base.PowerMatchAffectRequire((int)power, 1) && !interrupted;
					if (flag5)
					{
						bool flag6 = this._costBreathOrStance > 0;
						if (flag6)
						{
							bool isDirect = base.IsDirect;
							if (isDirect)
							{
								base.ChangeStanceValue(context, base.CombatChar, this._costBreathOrStance * 50 / 100);
							}
							else
							{
								base.ChangeBreathValue(context, base.CombatChar, this._costBreathOrStance * 50 / 100);
							}
							base.ShowSpecialEffectTips(1);
						}
						this._affected = false;
						base.ReduceEffectCount(1);
					}
				}
			}
		}

		// Token: 0x060030A8 RID: 12456 RVA: 0x002180E0 File Offset: 0x002162E0
		private void OnCostBreathAndStance(DataContext context, int charId, bool isAlly, int costBreath, int costStance, short skillId)
		{
			bool flag = base.CharacterId != charId || !this.IsSrcSkillPerformed;
			if (!flag)
			{
				this._costBreathOrStance = (base.IsDirect ? costStance : costBreath);
			}
		}

		// Token: 0x060030A9 RID: 12457 RVA: 0x00218120 File Offset: 0x00216320
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x060030AA RID: 12458 RVA: 0x00218170 File Offset: 0x00216370
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || base.CombatChar.GetPerformingSkillId() < 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 103 && dataKey.CustomParam0 == (base.IsDirect ? 0 : 1) && !base.CombatCharPowerMatchAffectRequire(1);
				if (flag2)
				{
					bool flag3 = !this._affected;
					if (flag3)
					{
						this._affected = true;
						base.ShowSpecialEffectTips(0);
					}
					result = -80;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000E73 RID: 3699
		private const sbyte ReduceBounceDamage = -80;

		// Token: 0x04000E74 RID: 3700
		private const sbyte RecoverBreathOrStance = 50;

		// Token: 0x04000E75 RID: 3701
		private int _costBreathOrStance;

		// Token: 0x04000E76 RID: 3702
		private bool _affected;
	}
}
