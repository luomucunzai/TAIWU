using System;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.DaYueYaoChang
{
	// Token: 0x02000317 RID: 791
	public class AddPowerAndAddFlaw : CombatSkillEffectBase
	{
		// Token: 0x06003415 RID: 13333 RVA: 0x00227A75 File Offset: 0x00225C75
		protected AddPowerAndAddFlaw()
		{
		}

		// Token: 0x06003416 RID: 13334 RVA: 0x00227A7F File Offset: 0x00225C7F
		protected AddPowerAndAddFlaw(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06003417 RID: 13335 RVA: 0x00227A8C File Offset: 0x00225C8C
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003418 RID: 13336 RVA: 0x00227AB3 File Offset: 0x00225CB3
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003419 RID: 13337 RVA: 0x00227ADC File Offset: 0x00225CDC
		private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				short preparingSkill = enemyChar.GetPreparingSkillId();
				this._addPower = 0;
				bool flag2 = preparingSkill >= 0;
				if (flag2)
				{
					CombatSkillKey skillKey = new CombatSkillKey(enemyChar.GetId(), preparingSkill);
					sbyte direction = DomainManager.CombatSkill.GetElement_CombatSkills(skillKey).GetDirection();
					bool flag3 = direction == 1;
					if (flag3)
					{
						this._addPower += (int)this.AddPower;
					}
				}
				bool flag4 = preparingSkill < 0 || Config.CombatSkill.Instance[preparingSkill].EquipType != 1;
				if (flag4)
				{
					this._addPower += (int)this.AddPower;
				}
				bool flag5 = this._addPower > 0;
				if (flag5)
				{
					base.AppendAffectedData(context, 199, EDataModifyType.AddPercent, base.SkillTemplateId);
					base.ShowSpecialEffectTips(0);
				}
				bool flag6 = !DomainManager.Combat.InAttackRange(base.CombatChar);
				if (flag6)
				{
					for (int i = 0; i < (int)this.FlawCount; i++)
					{
						DomainManager.Combat.AddFlaw(context, enemyChar, 3, this.SkillKey, -1, 1, true);
					}
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x0600341A RID: 13338 RVA: 0x00227C38 File Offset: 0x00225E38
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0600341B RID: 13339 RVA: 0x00227C70 File Offset: 0x00225E70
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

		// Token: 0x04000F62 RID: 3938
		protected short AddPower;

		// Token: 0x04000F63 RID: 3939
		protected sbyte FlawCount;

		// Token: 0x04000F64 RID: 3940
		private int _addPower;
	}
}
