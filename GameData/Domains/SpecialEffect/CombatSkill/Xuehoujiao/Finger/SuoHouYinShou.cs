using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Finger
{
	// Token: 0x0200023D RID: 573
	public class SuoHouYinShou : CombatSkillEffectBase
	{
		// Token: 0x06002FA7 RID: 12199 RVA: 0x00213C22 File Offset: 0x00211E22
		public SuoHouYinShou()
		{
		}

		// Token: 0x06002FA8 RID: 12200 RVA: 0x00213C2C File Offset: 0x00211E2C
		public SuoHouYinShou(CombatSkillKey skillKey) : base(skillKey, 15201, -1)
		{
		}

		// Token: 0x06002FA9 RID: 12201 RVA: 0x00213C40 File Offset: 0x00211E40
		public override void OnEnable(DataContext context)
		{
			DomainManager.Combat.AddSkillEffect(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), 0, base.MaxEffectCount, false);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002FAA RID: 12202 RVA: 0x00213CE0 File Offset: 0x00211EE0
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002FAB RID: 12203 RVA: 0x00213D1C File Offset: 0x00211F1C
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !hit || attacker != base.CombatChar || base.EffectCount >= (int)base.MaxEffectCount || !DomainManager.Combat.IsCurrentCombatCharacter(base.CurrEnemyChar) || attacker.NormalAttackBodyPart != 1;
			if (!flag)
			{
				DomainManager.Combat.ChangeSkillEffectCount(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), 1, true, false);
				base.ShowSpecialEffectTips(0);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			}
		}

		// Token: 0x06002FAC RID: 12204 RVA: 0x00213DB4 File Offset: 0x00211FB4
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * 50 * base.EffectCount / 100);
			}
		}

		// Token: 0x06002FAD RID: 12205 RVA: 0x00213E0C File Offset: 0x0021200C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId;
			if (!flag)
			{
				bool flag2 = skillId == base.SkillTemplateId;
				if (flag2)
				{
					bool flag3 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag3)
					{
						bool isDirect = base.IsDirect;
						if (isDirect)
						{
							base.ChangeStanceValue(context, base.CurrEnemyChar, -1600);
						}
						else
						{
							base.ChangeBreathValue(context, base.CurrEnemyChar, -12000);
						}
						base.ShowSpecialEffectTips(1);
					}
					base.ReduceEffectCount(base.EffectCount);
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
				}
				else
				{
					bool flag4 = power > 0 && Config.CombatSkill.Instance[skillId].EquipType == 1 && DomainManager.Combat.IsCurrentCombatCharacter(base.CurrEnemyChar) && base.CombatChar.SkillAttackBodyPart == 1 && base.EffectCount < (int)base.MaxEffectCount;
					if (flag4)
					{
						DomainManager.Combat.ChangeSkillEffectCount(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), 1, true, false);
						base.ShowSpecialEffectTips(0);
						DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
					}
				}
			}
		}

		// Token: 0x06002FAE RID: 12206 RVA: 0x00213F48 File Offset: 0x00212148
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
					result = 20 * base.EffectCount;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000E1C RID: 3612
		private const sbyte AddPrepareProgressUnit = 50;

		// Token: 0x04000E1D RID: 3613
		private const sbyte AddPowerUnit = 20;

		// Token: 0x04000E1E RID: 3614
		private const sbyte ReduceBreathStance = -40;

		// Token: 0x04000E1F RID: 3615
		private const sbyte EffectPartType = 1;
	}
}
