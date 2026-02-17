using System;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Throw
{
	// Token: 0x0200047C RID: 1148
	public class JiuChiXiang : CombatSkillEffectBase
	{
		// Token: 0x06003B8C RID: 15244 RVA: 0x002488E0 File Offset: 0x00246AE0
		public JiuChiXiang()
		{
		}

		// Token: 0x06003B8D RID: 15245 RVA: 0x002488EA File Offset: 0x00246AEA
		public JiuChiXiang(CombatSkillKey skillKey) : base(skillKey, 10404, -1)
		{
		}

		// Token: 0x06003B8E RID: 15246 RVA: 0x002488FC File Offset: 0x00246AFC
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_PrepareSkillProgressChange(new Events.OnPrepareSkillProgressChange(this.OnPrepareSkillProgressChange));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06003B8F RID: 15247 RVA: 0x00248954 File Offset: 0x00246B54
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_PrepareSkillProgressChange(new Events.OnPrepareSkillProgressChange(this.OnPrepareSkillProgressChange));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06003B90 RID: 15248 RVA: 0x002489AC File Offset: 0x00246BAC
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = this.IsSrcSkillPerformed && !base.IsDirect && attacker.IsAlly && Config.CombatSkill.Instance[skillId].EquipType == 1;
			if (flag)
			{
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x06003B91 RID: 15249 RVA: 0x002489F8 File Offset: 0x00246BF8
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = !this.IsSrcSkillPerformed;
			if (flag)
			{
				bool flag2 = charId != base.CharacterId || skillId != base.SkillTemplateId;
				if (!flag2)
				{
					bool flag3 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag3)
					{
						this.IsSrcSkillPerformed = true;
						this._affectCharId = (base.IsDirect ? base.CurrEnemyChar.GetId() : base.CharacterId);
						base.AppendAffectedData(context, this._affectCharId, 69, EDataModifyType.AddPercent, -1);
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
					bool flag5 = charId == this._affectCharId && Config.CombatSkill.Instance[skillId].EquipType == 1;
					if (flag5)
					{
						base.ReduceEffectCount(1);
					}
				}
			}
		}

		// Token: 0x06003B92 RID: 15250 RVA: 0x00248AF8 File Offset: 0x00246CF8
		private void OnPrepareSkillProgressChange(DataContext context, int charId, bool isAlly, short skillId, sbyte preparePercent)
		{
			bool flag = this.IsSrcSkillPerformed && preparePercent == 90 && charId == this._affectCharId && Config.CombatSkill.Instance[skillId].EquipType == 1 && context.Random.CheckPercentProb((base.EffectCount % 2 == 1) ? 40 : (base.IsDirect ? 80 : 20));
			if (flag)
			{
				DomainManager.Combat.InterruptSkill(context, DomainManager.Combat.GetElement_CombatCharacterDict(charId), 100);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003B93 RID: 15251 RVA: 0x00248B84 File Offset: 0x00246D84
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003B94 RID: 15252 RVA: 0x00248BD4 File Offset: 0x00246DD4
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !this.IsSrcSkillPerformed;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 69 && dataKey.CombatSkillId >= 0;
				if (flag2)
				{
					base.ShowSpecialEffectTips(1);
					result = 80;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04001170 RID: 4464
		private const sbyte AddDamage = 80;

		// Token: 0x04001171 RID: 4465
		private const sbyte InterruptPreparePercent = 90;

		// Token: 0x04001172 RID: 4466
		private const sbyte InterruptOdds = 40;

		// Token: 0x04001173 RID: 4467
		private int _affectCharId;
	}
}
