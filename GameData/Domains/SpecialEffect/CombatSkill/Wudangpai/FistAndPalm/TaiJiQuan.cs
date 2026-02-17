using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.FistAndPalm
{
	// Token: 0x020003CE RID: 974
	public class TaiJiQuan : CombatSkillEffectBase
	{
		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x0600378A RID: 14218 RVA: 0x00235FB8 File Offset: 0x002341B8
		private static CValuePercent HitFactor
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x0600378B RID: 14219 RVA: 0x00235FC1 File Offset: 0x002341C1
		private static CValuePercent AvoidFactor
		{
			get
			{
				return 300;
			}
		}

		// Token: 0x0600378C RID: 14220 RVA: 0x00235FCD File Offset: 0x002341CD
		public TaiJiQuan()
		{
		}

		// Token: 0x0600378D RID: 14221 RVA: 0x00235FD7 File Offset: 0x002341D7
		public TaiJiQuan(CombatSkillKey skillKey) : base(skillKey, 4108, -1)
		{
		}

		// Token: 0x0600378E RID: 14222 RVA: 0x00235FE8 File Offset: 0x002341E8
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(209, EDataModifyType.Custom, base.SkillTemplateId);
			base.CreateAffectedData(114, EDataModifyType.Custom, -1);
			base.CreateAffectedData(253, EDataModifyType.Custom, -1);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.RegisterHandler_CalcLeveragingValue(new Events.OnCalcLeveragingValue(this.OnCalcLeveragingValue));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600378F RID: 14223 RVA: 0x0023606C File Offset: 0x0023426C
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.UnRegisterHandler_CalcLeveragingValue(new Events.OnCalcLeveragingValue(this.OnCalcLeveragingValue));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003790 RID: 14224 RVA: 0x002360C2 File Offset: 0x002342C2
		private void OnCombatBegin(DataContext context)
		{
			base.AddMaxEffectCount(true);
		}

		// Token: 0x06003791 RID: 14225 RVA: 0x002360D0 File Offset: 0x002342D0
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			this.OnCalcLeveragingValue(context, hitType, hit, index);
			bool flag = context.SkillKey != this.SkillKey || base.IsDirect || index < 3 || this._leveragingValue <= 0;
			if (!flag)
			{
				CombatCharacter enemyChar = base.CurrEnemyChar;
				CValuePercent innerRatio = (int)base.SkillInstance.GetCurrInnerRatio();
				int innerFatalDamageValue = this._leveragingValue * innerRatio;
				int outerFatalDamageValue = this._leveragingValue - innerFatalDamageValue;
				this._leveragingValue = 0;
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 253);
				DomainManager.Combat.AddFatalDamageValue(context, enemyChar, innerFatalDamageValue, 1, -1, base.SkillTemplateId, EDamageType.None);
				DomainManager.Combat.AddFatalDamageValue(context, enemyChar, outerFatalDamageValue, 0, -1, base.SkillTemplateId, EDamageType.None);
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x06003792 RID: 14226 RVA: 0x002361B8 File Offset: 0x002343B8
		private void OnCalcLeveragingValue(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = hitType < 0 || hitType == 3;
			bool flag2 = flag || base.EffectCount <= 0;
			if (!flag2)
			{
				bool flag3 = (base.IsDirect ? context.DefenderId : context.AttackerId) != base.CharacterId;
				if (!flag3)
				{
					bool flag4 = !context.IsNormalAttack && index == 3;
					if (flag4)
					{
						bool leveragingBySkill = this._leveragingBySkill;
						if (leveragingBySkill)
						{
							this._leveragingBySkill = false;
							base.ReduceEffectCount(1);
						}
					}
					else
					{
						int power = context.IsNormalAttack ? 100 : context.Skill.GetHitDistribution()[(int)hitType];
						OuterAndInnerInts mixedDamage = CombatContext.Create(context.Attacker, context.Attacker, context.BodyPart, context.SkillTemplateId, -1, null).CalcMixedDamage(hitType, power);
						int addValue = (mixedDamage.Outer + mixedDamage.Inner) * (hit ? TaiJiQuan.HitFactor : TaiJiQuan.AvoidFactor);
						addValue *= (int)base.SkillInstance.GetPower();
						this._leveragingValue += addValue;
						DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 253);
						base.ShowSpecialEffectTips(0);
						bool isNormalAttack = context.IsNormalAttack;
						if (isNormalAttack)
						{
							base.ReduceEffectCount(1);
						}
						else
						{
							this._leveragingBySkill = true;
						}
					}
				}
			}
		}

		// Token: 0x06003793 RID: 14227 RVA: 0x0023634C File Offset: 0x0023454C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool _)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				DomainManager.Combat.RemoveSkillEffect(context, base.CombatChar, base.EffectKey);
				base.ShowSpecialEffectTips(2);
				base.IsDirect = !base.IsDirect;
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 209);
				base.AddMaxEffectCount(true);
			}
		}

		// Token: 0x06003794 RID: 14228 RVA: 0x002363C8 File Offset: 0x002345C8
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 209;
				if (flag2)
				{
					result = (base.IsDirect ? 0 : 1);
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x06003795 RID: 14229 RVA: 0x00236428 File Offset: 0x00234628
		public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId < 0;
			long result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 114 && dataKey.CustomParam0 == 1 && base.IsDirect && base.CombatChar.BeCalcInjuryInnerRatio >= 0;
				if (flag2)
				{
					bool isInner = dataKey.CustomParam1 == 1;
					int innerRatio = base.CombatChar.BeCalcInjuryInnerRatio;
					CValuePercent leveragingValueRatio = isInner ? innerRatio : (100 - innerRatio);
					int defendValue = (int)Math.Min((long)(this._leveragingValue * leveragingValueRatio), dataValue);
					this._leveragingValue -= defendValue;
					DomainManager.SpecialEffect.InvalidateCache(base.CombatChar.GetDataContext(), base.CharacterId, 253);
					base.ShowSpecialEffectTips(1);
					result = dataValue - (long)defendValue;
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x06003796 RID: 14230 RVA: 0x00236518 File Offset: 0x00234718
		public override List<CombatSkillEffectData> GetModifiedValue(AffectedDataKey dataKey, List<CombatSkillEffectData> dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			List<CombatSkillEffectData> result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				dataValue.Add(new CombatSkillEffectData(ECombatSkillEffectType.TaiJiQuanLeveragingValue, this._leveragingValue));
				result = dataValue;
			}
			return result;
		}

		// Token: 0x04001036 RID: 4150
		private int _leveragingValue;

		// Token: 0x04001037 RID: 4151
		private bool _leveragingBySkill;
	}
}
