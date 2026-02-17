using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Finger
{
	// Token: 0x0200055D RID: 1373
	public class TianYiShenShou : CombatSkillEffectBase
	{
		// Token: 0x06004096 RID: 16534 RVA: 0x0025EEE2 File Offset: 0x0025D0E2
		public TianYiShenShou()
		{
		}

		// Token: 0x06004097 RID: 16535 RVA: 0x0025EEEC File Offset: 0x0025D0EC
		public TianYiShenShou(CombatSkillKey skillKey) : base(skillKey, 2208, -1)
		{
		}

		// Token: 0x06004098 RID: 16536 RVA: 0x0025EF00 File Offset: 0x0025D100
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(69, EDataModifyType.AddPercent, base.SkillTemplateId);
			base.CreateAffectedData(251, EDataModifyType.Custom, base.SkillTemplateId);
			base.CreateAffectedData(248, EDataModifyType.Custom, base.SkillTemplateId);
			base.CreateAffectedData(324, EDataModifyType.Custom, base.SkillTemplateId);
			Events.RegisterHandler_CostTrickDuringPreparingSkill(new Events.OnCostTrickDuringPreparingSkill(this.OnCostTrickDuringPreparingSkill));
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004099 RID: 16537 RVA: 0x0025EF98 File Offset: 0x0025D198
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CostTrickDuringPreparingSkill(new Events.OnCostTrickDuringPreparingSkill(this.OnCostTrickDuringPreparingSkill));
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			base.OnDisable(context);
		}

		// Token: 0x0600409A RID: 16538 RVA: 0x0025EFE4 File Offset: 0x0025D1E4
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker != base.CombatChar || !DomainManager.Combat.InAttackRange(attacker) || this._costedTrickCount <= 0;
			if (!flag)
			{
				for (int i = 0; i < this._costedTrickCount; i++)
				{
					base.ShowSpecialEffectTipsOnceInFrame(1);
					sbyte level = (sbyte)Math.Max(3 - i / 3, 0);
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						DomainManager.Combat.AddFlaw(context, defender, level, this.SkillKey, -1, 1, true);
					}
					else
					{
						DomainManager.Combat.AddAcupoint(context, defender, level, this.SkillKey, -1, 1, true);
					}
				}
			}
		}

		// Token: 0x0600409B RID: 16539 RVA: 0x0025F088 File Offset: 0x0025D288
		private void OnCostTrickDuringPreparingSkill(DataContext context, int charId)
		{
			bool flag = charId != base.CharacterId;
			if (!flag)
			{
				this._costedTrickCount++;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x0600409C RID: 16540 RVA: 0x0025F0C0 File Offset: 0x0025D2C0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool _)
		{
			bool flag = this.SkillKey.IsMatch(charId, skillId);
			if (flag)
			{
				this._costedTrickCount = 0;
			}
		}

		// Token: 0x0600409D RID: 16541 RVA: 0x0025F0E8 File Offset: 0x0025D2E8
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.SkillKey == this.SkillKey && dataKey.FieldId == 69;
			int result;
			if (flag)
			{
				result = this._costedTrickCount * 10;
			}
			else
			{
				result = base.GetModifyValue(dataKey, currModifyValue);
			}
			return result;
		}

		// Token: 0x0600409E RID: 16542 RVA: 0x0025F134 File Offset: 0x0025D334
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.SkillKey != this.SkillKey;
			bool result;
			if (flag)
			{
				result = base.GetModifiedValue(dataKey, dataValue);
			}
			else
			{
				bool flag2 = dataKey.FieldId == 251 && this._costedTrickCount >= 2;
				if (flag2)
				{
					base.ShowSpecialEffectTipsOnceInFrame(2);
				}
				bool flag3 = dataKey.FieldId == 248 && this._costedTrickCount >= 4;
				if (flag3)
				{
					base.ShowSpecialEffectTipsOnceInFrame(3);
				}
				ushort fieldId = dataKey.FieldId;
				if (!true)
				{
				}
				bool flag4;
				if (fieldId != 248)
				{
					if (fieldId != 251)
					{
						flag4 = (fieldId == 324 || base.GetModifiedValue(dataKey, dataValue));
					}
					else
					{
						flag4 = (dataValue || this._costedTrickCount >= 2);
					}
				}
				else
				{
					flag4 = (dataValue || this._costedTrickCount >= 4);
				}
				if (!true)
				{
				}
				result = flag4;
			}
			return result;
		}

		// Token: 0x040012F7 RID: 4855
		private const int AddDamagePercentPerTrick = 10;

		// Token: 0x040012F8 RID: 4856
		private const sbyte MaxFlawOrAcupointLevel = 3;

		// Token: 0x040012F9 RID: 4857
		private const sbyte MinFlawOrAcupointLevel = 0;

		// Token: 0x040012FA RID: 4858
		private const int FlawOrAcupointLevelAttenuation = 3;

		// Token: 0x040012FB RID: 4859
		private const int InevitableHitRequireCount = 2;

		// Token: 0x040012FC RID: 4860
		private const int CertainCriticalRequireCount = 4;

		// Token: 0x040012FD RID: 4861
		private int _costedTrickCount;
	}
}
