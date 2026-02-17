using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Leg
{
	// Token: 0x0200022B RID: 555
	public class LiaoYinTui : CombatSkillEffectBase
	{
		// Token: 0x06002F5D RID: 12125 RVA: 0x00212A3E File Offset: 0x00210C3E
		public LiaoYinTui()
		{
		}

		// Token: 0x06002F5E RID: 12126 RVA: 0x00212A48 File Offset: 0x00210C48
		public LiaoYinTui(CombatSkillKey skillKey) : base(skillKey, 15300, -1)
		{
		}

		// Token: 0x06002F5F RID: 12127 RVA: 0x00212A5C File Offset: 0x00210C5C
		public override void OnEnable(DataContext context)
		{
			this._hasGenderAddPower = false;
			DomainManager.Combat.AddSkillEffect(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), 0, base.MaxEffectCount, false);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002F60 RID: 12128 RVA: 0x00212B18 File Offset: 0x00210D18
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002F61 RID: 12129 RVA: 0x00212B70 File Offset: 0x00210D70
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !hit || attacker != base.CombatChar || base.EffectCount >= (int)base.MaxEffectCount || !DomainManager.Combat.IsCurrentCombatCharacter(base.CurrEnemyChar) || attacker.NormalAttackBodyPart != 2;
			if (!flag)
			{
				DomainManager.Combat.ChangeSkillEffectCount(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), 1, true, false);
				base.ShowSpecialEffectTips(0);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			}
		}

		// Token: 0x06002F62 RID: 12130 RVA: 0x00212C08 File Offset: 0x00210E08
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * 50 * base.EffectCount / 100);
			}
		}

		// Token: 0x06002F63 RID: 12131 RVA: 0x00212C60 File Offset: 0x00210E60
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker != base.CombatChar || skillId != base.SkillTemplateId;
			if (!flag)
			{
				this._hasGenderAddPower = (defender.GetCharacter().GetGender() == (base.IsDirect ? 1 : 0));
				bool hasGenderAddPower = this._hasGenderAddPower;
				if (hasGenderAddPower)
				{
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06002F64 RID: 12132 RVA: 0x00212CD8 File Offset: 0x00210ED8
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId;
			if (!flag)
			{
				bool flag2 = skillId == base.SkillTemplateId;
				if (flag2)
				{
					this._hasGenderAddPower = false;
					base.ReduceEffectCount(base.EffectCount);
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
				}
				else
				{
					bool flag3 = power > 0 && Config.CombatSkill.Instance[skillId].EquipType == 1 && DomainManager.Combat.IsCurrentCombatCharacter(base.CurrEnemyChar) && base.CombatChar.SkillAttackBodyPart == 2 && base.EffectCount < (int)base.MaxEffectCount;
					if (flag3)
					{
						DomainManager.Combat.ChangeSkillEffectCount(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), 1, true, false);
						base.ShowSpecialEffectTips(0);
						DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
					}
				}
			}
		}

		// Token: 0x06002F65 RID: 12133 RVA: 0x00212DD0 File Offset: 0x00210FD0
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
					result = 20 * base.EffectCount + (this._hasGenderAddPower ? 20 : 0);
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000E0A RID: 3594
		private const sbyte AddPrepareProgressUnit = 50;

		// Token: 0x04000E0B RID: 3595
		private const sbyte AddPowerUnit = 20;

		// Token: 0x04000E0C RID: 3596
		private const sbyte GenderAddPower = 20;

		// Token: 0x04000E0D RID: 3597
		private const sbyte EffectPartType = 2;

		// Token: 0x04000E0E RID: 3598
		private bool _hasGenderAddPower;
	}
}
