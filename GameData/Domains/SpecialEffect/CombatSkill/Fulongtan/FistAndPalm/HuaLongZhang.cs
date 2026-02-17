using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.FistAndPalm
{
	// Token: 0x0200051E RID: 1310
	public class HuaLongZhang : CombatSkillEffectBase
	{
		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06003F18 RID: 16152 RVA: 0x002587F2 File Offset: 0x002569F2
		private static CValuePercent AddPowerPercent
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x06003F19 RID: 16153 RVA: 0x002587FB File Offset: 0x002569FB
		public HuaLongZhang()
		{
		}

		// Token: 0x06003F1A RID: 16154 RVA: 0x00258805 File Offset: 0x00256A05
		public HuaLongZhang(CombatSkillKey skillKey) : base(skillKey, 14108, -1)
		{
		}

		// Token: 0x06003F1B RID: 16155 RVA: 0x00258818 File Offset: 0x00256A18
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 156, -1, -1, -1, -1), EDataModifyType.Custom);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003F1C RID: 16156 RVA: 0x00258864 File Offset: 0x00256A64
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003F1D RID: 16157 RVA: 0x0025887C File Offset: 0x00256A7C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					bool flag3 = !this._changedSkill;
					if (flag3)
					{
						base.AddMaxEffectCount(true);
					}
					else
					{
						CombatCharacter enemyChar = base.CurrEnemyChar;
						DomainManager.Combat.ClearAffectingDefenseSkill(context, enemyChar);
						base.ClearAffectingAgileSkill(context, enemyChar);
						base.ChangeMobilityValue(context, enemyChar, -enemyChar.GetMobilityValue());
						base.ShowSpecialEffectTips(1);
					}
				}
				this._changedSkill = false;
				DomainManager.Combat.RemoveSkillPowerReplaceInCombat(context, this.SkillKey);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			}
		}

		// Token: 0x06003F1E RID: 16158 RVA: 0x0025893C File Offset: 0x00256B3C
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || base.EffectCount == 0 || dataValue == (int)base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 156;
				if (flag2)
				{
					DataContext context = DomainManager.Combat.Context;
					CombatSkillKey powerSkillKey = new CombatSkillKey(base.CharacterId, (short)dataValue);
					CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(powerSkillKey);
					bool flag3 = DomainManager.CombatSkill.GetSkillType(base.CharacterId, (short)dataValue) == 3 && skill.GetDirection() == base.SkillInstance.GetDirection();
					if (flag3)
					{
						this._changedSkill = true;
						int addPower = (int)base.SkillInstance.GetPower() * HuaLongZhang.AddPowerPercent;
						DomainManager.Combat.AddSkillPowerInCombat(context, powerSkillKey, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), addPower);
						DomainManager.Combat.SetSkillPowerReplaceInCombat(context, this.SkillKey, powerSkillKey);
						base.ShowSpecialEffectTips(0);
						base.ReduceEffectCount(1);
						return (int)base.SkillTemplateId;
					}
				}
				result = dataValue;
			}
			return result;
		}

		// Token: 0x04001298 RID: 4760
		private bool _changedSkill;
	}
}
