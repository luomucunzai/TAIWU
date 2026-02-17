using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Finger
{
	// Token: 0x020003A1 RID: 929
	public class SheGuShou : CombatSkillEffectBase
	{
		// Token: 0x0600368C RID: 13964 RVA: 0x00230FFF File Offset: 0x0022F1FF
		public SheGuShou()
		{
		}

		// Token: 0x0600368D RID: 13965 RVA: 0x00231009 File Offset: 0x0022F209
		public SheGuShou(CombatSkillKey skillKey) : base(skillKey, 12200, -1)
		{
		}

		// Token: 0x0600368E RID: 13966 RVA: 0x0023101C File Offset: 0x0022F21C
		public override void OnEnable(DataContext context)
		{
			this._addPower = (int)(base.CombatChar.GetWugCount() / 2);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
			bool flag = this._addPower > 0;
			if (flag)
			{
				base.ShowSpecialEffectTips(0);
			}
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600368F RID: 13967 RVA: 0x002310A7 File Offset: 0x0022F2A7
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003690 RID: 13968 RVA: 0x002310D0 File Offset: 0x0022F2D0
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = index != 3 || context.SkillKey != this.SkillKey || !base.CombatCharPowerMatchAffectRequire(0);
			if (!flag)
			{
				short selfToxicology = context.Attacker.GetCharacter().GetLifeSkillAttainment(9);
				short enemyToxicology = context.Defender.GetCharacter().GetLifeSkillAttainment(9);
				int toxicologyDifference = (int)(base.IsDirect ? (selfToxicology - enemyToxicology) : (enemyToxicology - selfToxicology));
				int addWugCount = 9 + Math.Max(0, toxicologyDifference / 20);
				bool flag2 = base.CombatChar.ChangeWugCount(context, addWugCount);
				if (flag2)
				{
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x06003691 RID: 13969 RVA: 0x00231174 File Offset: 0x0022F374
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003692 RID: 13970 RVA: 0x002311AC File Offset: 0x0022F3AC
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

		// Token: 0x04000FE2 RID: 4066
		private const int AddWugCount = 9;

		// Token: 0x04000FE3 RID: 4067
		private const int ToxicologyAddWugUnit = 20;

		// Token: 0x04000FE4 RID: 4068
		private int _addPower;
	}
}
