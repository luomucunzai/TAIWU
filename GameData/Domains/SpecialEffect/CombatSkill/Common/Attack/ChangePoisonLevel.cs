using System;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x02000595 RID: 1429
	public abstract class ChangePoisonLevel : CombatSkillEffectBase
	{
		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06004262 RID: 16994
		protected abstract sbyte AffectPoisonType { get; }

		// Token: 0x06004263 RID: 16995 RVA: 0x00266AB8 File Offset: 0x00264CB8
		protected bool IsMatchOwnAffect(CombatSkillKey skillKey)
		{
			return skillKey.CharId == base.CharacterId && skillKey.SkillTemplateId >= 0 && skillKey.SkillTemplateId == this.AffectingSkillKey.SkillTemplateId;
		}

		// Token: 0x06004264 RID: 16996 RVA: 0x00266AF8 File Offset: 0x00264CF8
		protected bool IsMatchPoison(short skillId)
		{
			bool flag = skillId < 0 || !CombatSkillTemplateHelper.IsAttack(skillId);
			return !flag && DomainManager.Combat.CheckSkillPoison(skillId, this.AffectPoisonType);
		}

		// Token: 0x06004265 RID: 16997 RVA: 0x00266B33 File Offset: 0x00264D33
		protected ChangePoisonLevel()
		{
		}

		// Token: 0x06004266 RID: 16998 RVA: 0x00266B3D File Offset: 0x00264D3D
		protected ChangePoisonLevel(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06004267 RID: 16999 RVA: 0x00266B4C File Offset: 0x00264D4C
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(base.IsDirect ? 72 : 105, EDataModifyType.Add, -1);
			this.AffectingSkillKey = new CombatSkillKey(-1, -1);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_CastSkillAllEnd(new Events.OnCastSkillAllEnd(this.OnCastSkillAllEnd));
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.PrepareSkillBegin));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06004268 RID: 17000 RVA: 0x00266BC8 File Offset: 0x00264DC8
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_CastSkillAllEnd(new Events.OnCastSkillAllEnd(this.OnCastSkillAllEnd));
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.PrepareSkillBegin));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06004269 RID: 17001 RVA: 0x00266C20 File Offset: 0x00264E20
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = this.SkillKey.IsMatch(charId, skillId) && base.PowerMatchAffectRequire((int)power, 0);
			if (flag)
			{
				base.AddMaxEffectCount(true);
			}
		}

		// Token: 0x0600426A RID: 17002 RVA: 0x00266C58 File Offset: 0x00264E58
		private void OnCastSkillAllEnd(DataContext context, int charId, short skillId)
		{
			bool flag = this.AffectingSkillKey.IsMatch(charId, skillId);
			if (flag)
			{
				this.AffectingSkillKey = new CombatSkillKey(-1, -1);
			}
		}

		// Token: 0x0600426B RID: 17003 RVA: 0x00266C84 File Offset: 0x00264E84
		private void PrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = !this.IsMatchPoison(skillId) || this.AffectingSkillKey.CharId >= 0;
			if (!flag)
			{
				bool flag2 = (base.IsDirect ? (isAlly != base.CombatChar.IsAlly) : (isAlly == base.CombatChar.IsAlly)) || base.EffectCount <= 0;
				if (!flag2)
				{
					this.AffectingSkillKey = new CombatSkillKey(charId, skillId);
					this.OnAffecting(context);
					base.ReduceEffectCount(1);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x0600426C RID: 17004 RVA: 0x00266D1C File Offset: 0x00264F1C
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = !this.SkillKey.IsMatch(charId, key.SkillId) || key.IsDirect != base.IsDirect;
			if (!flag)
			{
				this.OnEffectCountChanged(context);
			}
		}

		// Token: 0x0600426D RID: 17005 RVA: 0x00266D60 File Offset: 0x00264F60
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CombatSkillId < 0 || Config.CombatSkill.Instance[dataKey.CombatSkillId].EquipType != 1;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.CustomParam0 == (int)this.AffectPoisonType;
				bool flag3 = flag2;
				if (flag3)
				{
					ushort fieldId = dataKey.FieldId;
					bool flag4 = fieldId == 72 || fieldId == 105;
					flag3 = flag4;
				}
				bool flag5 = flag3 && this.IsMatchOwnAffect(dataKey.SkillKey);
				if (flag5)
				{
					result = ((dataKey.FieldId == 72) ? 1 : -1);
				}
				else
				{
					bool flag6 = dataKey.SkillKey == this.AffectingSkillKey;
					if (flag6)
					{
						result = this.AffectingGetModifyValue(dataKey, currModifyValue);
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x0600426E RID: 17006 RVA: 0x00266E29 File Offset: 0x00265029
		protected virtual void OnAffecting(DataContext context)
		{
		}

		// Token: 0x0600426F RID: 17007 RVA: 0x00266E2C File Offset: 0x0026502C
		protected virtual void OnEffectCountChanged(DataContext context)
		{
		}

		// Token: 0x06004270 RID: 17008 RVA: 0x00266E30 File Offset: 0x00265030
		protected virtual int AffectingGetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			return 0;
		}

		// Token: 0x040013A2 RID: 5026
		protected CombatSkillKey AffectingSkillKey;
	}
}
