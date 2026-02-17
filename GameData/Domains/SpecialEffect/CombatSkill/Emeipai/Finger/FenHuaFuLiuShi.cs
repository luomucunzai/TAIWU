using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Finger
{
	// Token: 0x0200055A RID: 1370
	public class FenHuaFuLiuShi : CombatSkillEffectBase
	{
		// Token: 0x0600407B RID: 16507 RVA: 0x0025E439 File Offset: 0x0025C639
		public FenHuaFuLiuShi()
		{
		}

		// Token: 0x0600407C RID: 16508 RVA: 0x0025E443 File Offset: 0x0025C643
		public FenHuaFuLiuShi(CombatSkillKey skillKey) : base(skillKey, 2203, -1)
		{
		}

		// Token: 0x0600407D RID: 16509 RVA: 0x0025E454 File Offset: 0x0025C654
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 281, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x0600407E RID: 16510 RVA: 0x0025E4DC File Offset: 0x0025C6DC
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x0600407F RID: 16511 RVA: 0x0025E534 File Offset: 0x0025C734
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = !this._affected;
			if (!flag)
			{
				this._affected = false;
				base.ReduceEffectCount(1);
			}
		}

		// Token: 0x06004080 RID: 16512 RVA: 0x0025E560 File Offset: 0x0025C760
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker != base.CombatChar || skillId != base.SkillTemplateId || this.IsSrcSkillPerformed || !DomainManager.Combat.InAttackRange(base.CombatChar);
			if (!flag)
			{
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06004081 RID: 16513 RVA: 0x0025E5B0 File Offset: 0x0025C7B0
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
						bool isDirect = base.IsDirect;
						if (isDirect)
						{
							base.AppendAffectedData(context, base.CharacterId, 158, EDataModifyType.Custom, -1);
						}
						else
						{
							base.AppendAffectedCurrEnemyData(context, 158, EDataModifyType.Custom, -1);
						}
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
					bool flag5 = this._affected && Config.CombatSkill.Instance[skillId].EquipType == 1;
					if (flag5)
					{
						base.ReduceEffectCount(1);
					}
				}
			}
		}

		// Token: 0x06004082 RID: 16514 RVA: 0x0025E6A4 File Offset: 0x0025C8A4
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06004083 RID: 16515 RVA: 0x0025E6F4 File Offset: 0x0025C8F4
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.SkillKey != this.SkillKey || dataKey.FieldId != 281;
			return !flag || dataValue;
		}

		// Token: 0x06004084 RID: 16516 RVA: 0x0025E738 File Offset: 0x0025C938
		public unsafe override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = this.IsSrcSkillPerformed && dataKey.FieldId == 158 && dataKey.CustomParam0 != 3;
			if (flag)
			{
				CombatCharacter combatChar = DomainManager.Combat.GetElement_CombatCharacterDict(dataKey.CharId);
				bool flag2 = DomainManager.Combat.InAttackRange(combatChar);
				if (flag2)
				{
					int avoid = *(ref combatChar.GetCharacter().GetAvoidValues().Items.FixedElementField + (IntPtr)dataKey.CustomParam0 * 4);
					bool flag3 = base.IsDirect ? (dataValue <= avoid) : (dataValue > avoid);
					if (flag3)
					{
						bool flag4 = !this._affected;
						if (flag4)
						{
							this._affected = true;
							base.ShowSpecialEffectTips(1);
						}
						return avoid * FenHuaFuLiuShi.HitChangeToAvoidPercent;
					}
				}
			}
			return dataValue;
		}

		// Token: 0x040012F1 RID: 4849
		private static readonly CValuePercent HitChangeToAvoidPercent = 150;

		// Token: 0x040012F2 RID: 4850
		private bool _affected;
	}
}
