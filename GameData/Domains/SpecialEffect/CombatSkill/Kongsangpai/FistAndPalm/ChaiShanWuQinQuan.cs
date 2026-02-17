using System;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.FistAndPalm
{
	// Token: 0x0200048C RID: 1164
	public class ChaiShanWuQinQuan : CombatSkillEffectBase
	{
		// Token: 0x06003BF7 RID: 15351 RVA: 0x0024B4BC File Offset: 0x002496BC
		public ChaiShanWuQinQuan()
		{
		}

		// Token: 0x06003BF8 RID: 15352 RVA: 0x0024B4C6 File Offset: 0x002496C6
		public ChaiShanWuQinQuan(CombatSkillKey skillKey) : base(skillKey, 10100, -1)
		{
		}

		// Token: 0x06003BF9 RID: 15353 RVA: 0x0024B4D7 File Offset: 0x002496D7
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06003BFA RID: 15354 RVA: 0x0024B510 File Offset: 0x00249710
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06003BFB RID: 15355 RVA: 0x0024B54C File Offset: 0x0024974C
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !this.IsSrcSkillPerformed || defender.GetId() != base.CharacterId || !this._affected;
			if (!flag)
			{
				this._affected = false;
				base.ReduceEffectCount(1);
			}
		}

		// Token: 0x06003BFC RID: 15356 RVA: 0x0024B594 File Offset: 0x00249794
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = !this.IsSrcSkillPerformed;
			if (flag)
			{
				bool flag2 = charId == base.CharacterId && skillId == base.SkillTemplateId;
				if (flag2)
				{
					this.IsSrcSkillPerformed = true;
					bool flag3 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag3)
					{
						base.AppendAffectedData(context, base.CharacterId, 102, EDataModifyType.AddPercent, -1);
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
					bool flag5 = Config.CombatSkill.Instance[skillId].EquipType == 1 && this._affected;
					if (flag5)
					{
						this._affected = false;
						base.ReduceEffectCount(1);
					}
				}
			}
		}

		// Token: 0x06003BFD RID: 15357 RVA: 0x0024B670 File Offset: 0x00249870
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003BFE RID: 15358 RVA: 0x0024B6C0 File Offset: 0x002498C0
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 102 && dataKey.CustomParam0 == (base.IsDirect ? 0 : 1);
				if (flag2)
				{
					bool flag3 = !this._affected;
					if (flag3)
					{
						this._affected = true;
						base.ShowSpecialEffectTips(0);
					}
					result = -15;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x040011A1 RID: 4513
		private const sbyte ReduceDamagePercent = -15;

		// Token: 0x040011A2 RID: 4514
		private bool _affected;
	}
}
