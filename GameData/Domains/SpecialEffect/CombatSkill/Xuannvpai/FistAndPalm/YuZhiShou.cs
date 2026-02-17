using System;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.FistAndPalm
{
	// Token: 0x02000277 RID: 631
	public class YuZhiShou : CombatSkillEffectBase
	{
		// Token: 0x060030B3 RID: 12467 RVA: 0x00218432 File Offset: 0x00216632
		public YuZhiShou()
		{
		}

		// Token: 0x060030B4 RID: 12468 RVA: 0x0021843C File Offset: 0x0021663C
		public YuZhiShou(CombatSkillKey skillKey) : base(skillKey, 8101, -1)
		{
		}

		// Token: 0x060030B5 RID: 12469 RVA: 0x0021844D File Offset: 0x0021664D
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x060030B6 RID: 12470 RVA: 0x00218474 File Offset: 0x00216674
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x060030B7 RID: 12471 RVA: 0x0021849C File Offset: 0x0021669C
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
						base.AppendAffectedData(context, base.CharacterId, 69, EDataModifyType.AddPercent, -1);
						base.AppendAffectedAllEnemyData(context, 69, EDataModifyType.AddPercent, -1);
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

		// Token: 0x060030B8 RID: 12472 RVA: 0x00218584 File Offset: 0x00216784
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x060030B9 RID: 12473 RVA: 0x002185D4 File Offset: 0x002167D4
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CombatSkillId < 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 69;
				if (flag2)
				{
					bool flag3 = !this._affected;
					if (flag3)
					{
						this._affected = true;
						base.ShowSpecialEffectTips(0);
					}
					result = (base.IsDirect ? 30 : -30);
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000E77 RID: 3703
		private const sbyte ChangeDamagePercent = 30;

		// Token: 0x04000E78 RID: 3704
		private bool _affected;
	}
}
