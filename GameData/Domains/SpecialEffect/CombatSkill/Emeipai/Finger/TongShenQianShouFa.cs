using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Finger
{
	// Token: 0x0200055E RID: 1374
	public class TongShenQianShouFa : CombatSkillEffectBase
	{
		// Token: 0x0600409F RID: 16543 RVA: 0x0025F22A File Offset: 0x0025D42A
		public TongShenQianShouFa()
		{
		}

		// Token: 0x060040A0 RID: 16544 RVA: 0x0025F234 File Offset: 0x0025D434
		public TongShenQianShouFa(CombatSkillKey skillKey) : base(skillKey, 2205, -1)
		{
		}

		// Token: 0x060040A1 RID: 16545 RVA: 0x0025F248 File Offset: 0x0025D448
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(215, EDataModifyType.Custom, base.SkillTemplateId);
			base.CreateAffectedData(217, EDataModifyType.Custom, base.SkillTemplateId);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_NormalAttackCalcHitEnd(new Events.OnNormalAttackCalcHitEnd(this.OnNormalAttackCalcHitEnd));
		}

		// Token: 0x060040A2 RID: 16546 RVA: 0x0025F2A0 File Offset: 0x0025D4A0
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_NormalAttackCalcHitEnd(new Events.OnNormalAttackCalcHitEnd(this.OnNormalAttackCalcHitEnd));
		}

		// Token: 0x060040A3 RID: 16547 RVA: 0x0025F2C8 File Offset: 0x0025D4C8
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool _)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || !base.PowerMatchAffectRequire((int)power, 0);
			if (!flag)
			{
				base.AddMaxEffectCount(true);
			}
		}

		// Token: 0x060040A4 RID: 16548 RVA: 0x0025F308 File Offset: 0x0025D508
		private void OnNormalAttackCalcHitEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, int pursueIndex, bool hit, bool isFightback, bool isMind)
		{
			bool flag = (base.IsDirect ? defender : attacker) != base.CombatChar || hit || base.EffectCount <= 0;
			if (!flag)
			{
				bool nextAttackNoPrepare = base.CombatChar.NextAttackNoPrepare;
				if (!nextAttackNoPrepare)
				{
					base.CombatChar.NextAttackNoPrepare = true;
					base.ReduceEffectCount(1);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x060040A5 RID: 16549 RVA: 0x0025F374 File Offset: 0x0025D574
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.SkillKey != this.SkillKey;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId == 215 || fieldId == 217;
				bool flag3 = flag2;
				result = (!flag3 && dataValue);
			}
			return result;
		}
	}
}
