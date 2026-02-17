using System;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Shot
{
	// Token: 0x020001C2 RID: 450
	public class WuZiLianHuanKou : CombatSkillEffectBase
	{
		// Token: 0x06002CC2 RID: 11458 RVA: 0x0020949F File Offset: 0x0020769F
		public WuZiLianHuanKou()
		{
		}

		// Token: 0x06002CC3 RID: 11459 RVA: 0x002094A9 File Offset: 0x002076A9
		public WuZiLianHuanKou(CombatSkillKey skillKey) : base(skillKey, 9402, -1)
		{
		}

		// Token: 0x06002CC4 RID: 11460 RVA: 0x002094BA File Offset: 0x002076BA
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06002CC5 RID: 11461 RVA: 0x002094F3 File Offset: 0x002076F3
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06002CC6 RID: 11462 RVA: 0x0020952C File Offset: 0x0020772C
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = !this.IsSrcSkillPerformed || Config.CombatSkill.Instance[skillId].EquipType != 1 || !(base.IsDirect ? (charId == base.CharacterId) : (base.CombatChar.IsAlly != isAlly));
			if (!flag)
			{
				this._affectingSkill = new CombatSkillKey(charId, skillId);
				DomainManager.SpecialEffect.InvalidateCache(context, charId, 199);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06002CC7 RID: 11463 RVA: 0x002095B0 File Offset: 0x002077B0
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
						base.AddMaxEffectCount(true);
						bool isDirect = base.IsDirect;
						if (isDirect)
						{
							base.AppendAffectedData(context, base.CharacterId, 199, EDataModifyType.AddPercent, -1);
						}
						else
						{
							base.AppendAffectedAllEnemyData(context, 199, EDataModifyType.AddPercent, -1);
						}
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
					bool flag5 = charId == this._affectingSkill.CharId && skillId == this._affectingSkill.SkillTemplateId;
					if (flag5)
					{
						base.ReduceEffectCount(1);
						this._affectingSkill.CharId = -1;
						DomainManager.SpecialEffect.InvalidateCache(context, charId, 199);
					}
				}
			}
		}

		// Token: 0x06002CC8 RID: 11464 RVA: 0x002096D4 File Offset: 0x002078D4
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06002CC9 RID: 11465 RVA: 0x00209724 File Offset: 0x00207924
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !this.IsSrcSkillPerformed || dataKey.CharId != this._affectingSkill.CharId || dataKey.CombatSkillId != this._affectingSkill.SkillTemplateId;
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
					result = 10 * ((int)base.MaxEffectCount - base.EffectCount + 1) * (base.IsDirect ? 1 : -1);
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000D80 RID: 3456
		private const sbyte ChangePowerUnit = 10;

		// Token: 0x04000D81 RID: 3457
		private CombatSkillKey _affectingSkill;
	}
}
