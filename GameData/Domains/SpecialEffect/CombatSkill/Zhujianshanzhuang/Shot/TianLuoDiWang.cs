using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Shot
{
	// Token: 0x020001C0 RID: 448
	public class TianLuoDiWang : CombatSkillEffectBase
	{
		// Token: 0x06002CB2 RID: 11442 RVA: 0x00208A34 File Offset: 0x00206C34
		public TianLuoDiWang()
		{
		}

		// Token: 0x06002CB3 RID: 11443 RVA: 0x00208A3E File Offset: 0x00206C3E
		public TianLuoDiWang(CombatSkillKey skillKey) : base(skillKey, 9404, -1)
		{
		}

		// Token: 0x06002CB4 RID: 11444 RVA: 0x00208A4F File Offset: 0x00206C4F
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06002CB5 RID: 11445 RVA: 0x00208A76 File Offset: 0x00206C76
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06002CB6 RID: 11446 RVA: 0x00208AA0 File Offset: 0x00206CA0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = !this.IsSrcSkillPerformed;
				if (flag2)
				{
					bool flag3 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag3)
					{
						this.IsSrcSkillPerformed = true;
						base.AddMaxEffectCount(true);
						base.AppendAffectedAllEnemyData(context, 151, EDataModifyType.Custom, -1);
						base.ShowSpecialEffectTips(0);
					}
					else
					{
						base.RemoveSelf(context);
					}
				}
				else
				{
					bool flag4 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag4)
					{
						base.RemoveSelf(context);
					}
				}
			}
		}

		// Token: 0x06002CB7 RID: 11447 RVA: 0x00208B38 File Offset: 0x00206D38
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06002CB8 RID: 11448 RVA: 0x00208B88 File Offset: 0x00206D88
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = !this.IsSrcSkillPerformed || DomainManager.Combat.GetElement_CombatCharacterDict(dataKey.CharId).IsAlly == base.CombatChar.IsAlly || !DomainManager.Combat.IsCurrentCombatCharacter(base.CombatChar);
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 151 && dataValue != 0 && dataKey.CustomParam0 == (base.IsDirect ? 1 : 0);
				if (flag2)
				{
					int costEffectCount = Math.Min(Math.Abs(dataValue), base.EffectCount);
					base.ReduceEffectCount(costEffectCount);
					result = dataValue + (base.IsDirect ? costEffectCount : (-costEffectCount));
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}
	}
}
