using System;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Sword
{
	// Token: 0x0200053E RID: 1342
	public class YuNvShenJian : CombatSkillEffectBase
	{
		// Token: 0x06003FE6 RID: 16358 RVA: 0x0025BFD6 File Offset: 0x0025A1D6
		private static int CalcAddPercent(short skillId)
		{
			return (int)(30 + (Config.CombatSkill.Instance[skillId].Grade + 1) * 3);
		}

		// Token: 0x06003FE7 RID: 16359 RVA: 0x0025BFEF File Offset: 0x0025A1EF
		public YuNvShenJian()
		{
		}

		// Token: 0x06003FE8 RID: 16360 RVA: 0x0025BFF9 File Offset: 0x0025A1F9
		public YuNvShenJian(CombatSkillKey skillKey) : base(skillKey, 2306, -1)
		{
		}

		// Token: 0x06003FE9 RID: 16361 RVA: 0x0025C00C File Offset: 0x0025A20C
		public override void OnEnable(DataContext context)
		{
			this.ClearFields();
			base.CreateAffectedData(199, EDataModifyType.AddPercent, base.SkillTemplateId);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_ChangePreparingSkillBegin(new Events.OnChangePreparingSkillBegin(this.OnChangePreparingSkillBegin));
		}

		// Token: 0x06003FEA RID: 16362 RVA: 0x0025C06A File Offset: 0x0025A26A
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_ChangePreparingSkillBegin(new Events.OnChangePreparingSkillBegin(this.OnChangePreparingSkillBegin));
		}

		// Token: 0x06003FEB RID: 16363 RVA: 0x0025C0A4 File Offset: 0x0025A2A4
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = base.CharacterId != charId;
			if (!flag)
			{
				bool flag2 = skillId == base.SkillTemplateId;
				if (flag2)
				{
					this._allSkillHasPower = true;
					this._affecting = true;
					base.CombatChar.CanCastDuringPrepareSkills.Clear();
					foreach (short attackSkillId in base.CombatChar.GetAttackSkillList())
					{
						bool flag3 = attackSkillId < 0 || attackSkillId == base.SkillTemplateId;
						if (!flag3)
						{
							CombatSkillKey skillKey = new CombatSkillKey(base.CharacterId, attackSkillId);
							GameData.Domains.CombatSkill.CombatSkill skillObj = DomainManager.CombatSkill.GetElement_CombatSkills(skillKey);
							sbyte innerRatio = skillObj.GetCurrInnerRatio();
							bool flag4 = !(base.IsDirect ? (innerRatio <= 40) : (innerRatio >= 60));
							if (!flag4)
							{
								base.CombatChar.CanCastDuringPrepareSkills.Add(attackSkillId);
								DomainManager.Combat.UpdateSkillCanUse(context, base.CombatChar, attackSkillId);
							}
						}
					}
					bool flag5 = base.CombatChar.CanCastDuringPrepareSkills.Count > 0;
					if (flag5)
					{
						base.ShowSpecialEffectTips(0);
					}
					bool flag6 = this._preparePercent > 0;
					if (flag6)
					{
						DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * this._preparePercent);
					}
				}
				else
				{
					bool affecting = this._affecting;
					if (affecting)
					{
						base.CombatChar.CanCastDuringPrepareSkills.Clear();
						DomainManager.Combat.UpdateSkillCanUse(context, base.CombatChar);
						DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress);
					}
				}
			}
		}

		// Token: 0x06003FEC RID: 16364 RVA: 0x0025C27C File Offset: 0x0025A47C
		private void OnChangePreparingSkillBegin(DataContext context, int charId, short prevSkillId, short currSkillId)
		{
			bool flag = charId != base.CharacterId || prevSkillId != base.SkillTemplateId || currSkillId == base.CombatChar.NeedUseSkillFreeId;
			if (!flag)
			{
				this._preparePercent = CValuePercent.Parse(base.CombatChar.SkillPrepareCurrProgress, base.CombatChar.SkillPrepareTotalProgress);
				this._preparePercent += YuNvShenJian.CalcAddPercent(currSkillId);
				this._addPower += YuNvShenJian.CalcAddPercent(currSkillId);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			}
		}

		// Token: 0x06003FED RID: 16365 RVA: 0x0025C320 File Offset: 0x0025A520
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId;
			if (!flag)
			{
				bool flag2 = skillId == base.SkillTemplateId;
				if (flag2)
				{
					base.CombatChar.CanCastDuringPrepareSkills.Clear();
					this.ClearFieldsWithInvalidateCache(context);
				}
				else
				{
					bool affecting = this._affecting;
					if (affecting)
					{
						this._allSkillHasPower = (this._allSkillHasPower && base.PowerMatchAffectRequire((int)power, 0));
						bool flag3 = this._allSkillHasPower && DomainManager.Combat.CanCastSkill(base.CombatChar, base.SkillTemplateId, true, false);
						if (flag3)
						{
							DomainManager.Combat.CastSkillFree(context, base.CombatChar, base.SkillTemplateId, ECombatCastFreePriority.YuNvShenJian);
						}
						else
						{
							this.ClearFieldsWithInvalidateCache(context);
							DomainManager.Combat.RaiseCastSkillEndByInterrupt(context, charId, isAlly, base.SkillTemplateId);
						}
					}
				}
			}
		}

		// Token: 0x06003FEE RID: 16366 RVA: 0x0025C3F7 File Offset: 0x0025A5F7
		private void ClearFields()
		{
			this._preparePercent = 0;
			this._addPower = 0;
			this._affecting = false;
		}

		// Token: 0x06003FEF RID: 16367 RVA: 0x0025C414 File Offset: 0x0025A614
		private void ClearFieldsWithInvalidateCache(DataContext context)
		{
			this.ClearFields();
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
		}

		// Token: 0x06003FF0 RID: 16368 RVA: 0x0025C438 File Offset: 0x0025A638
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
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

		// Token: 0x040012D1 RID: 4817
		private const sbyte RequireRatio = 60;

		// Token: 0x040012D2 RID: 4818
		private CValuePercent _preparePercent;

		// Token: 0x040012D3 RID: 4819
		private int _addPower;

		// Token: 0x040012D4 RID: 4820
		private bool _affecting;

		// Token: 0x040012D5 RID: 4821
		private bool _allSkillHasPower;
	}
}
