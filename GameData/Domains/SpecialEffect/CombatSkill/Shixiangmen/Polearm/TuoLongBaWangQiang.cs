using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Polearm
{
	// Token: 0x020003EE RID: 1006
	public class TuoLongBaWangQiang : CombatSkillEffectBase
	{
		// Token: 0x0600384A RID: 14410 RVA: 0x00239C98 File Offset: 0x00237E98
		public TuoLongBaWangQiang()
		{
		}

		// Token: 0x0600384B RID: 14411 RVA: 0x00239CA2 File Offset: 0x00237EA2
		public TuoLongBaWangQiang(CombatSkillKey skillKey) : base(skillKey, 6307, -1)
		{
		}

		// Token: 0x0600384C RID: 14412 RVA: 0x00239CB4 File Offset: 0x00237EB4
		public override void OnEnable(DataContext context)
		{
			this._damageValue = new OuterAndInnerInts(0, 0);
			this._effectCount = 0;
			if (this.AffectDatas == null)
			{
				this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			}
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 116, -1, -1, -1, -1), EDataModifyType.Add);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 249, -1, -1, -1, -1), EDataModifyType.Custom);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 192, -1, -1, -1, -1), EDataModifyType.Custom);
			Events.RegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.OnAddDirectDamageValue));
			Events.RegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.AddDirectInjury));
			Events.RegisterHandler_AddDirectFatalDamageMark(new Events.OnAddDirectFatalDamageMark(this.AddDirectFatalDamageMark));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x0600384D RID: 14413 RVA: 0x00239DA4 File Offset: 0x00237FA4
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.OnAddDirectDamageValue));
			Events.UnRegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.AddDirectInjury));
			Events.UnRegisterHandler_AddDirectFatalDamageMark(new Events.OnAddDirectFatalDamageMark(this.AddDirectFatalDamageMark));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x0600384E RID: 14414 RVA: 0x00239E0C File Offset: 0x0023800C
		private void OnAddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId)
		{
			bool flag = attackerId == base.CharacterId && combatSkillId == base.SkillTemplateId;
			if (flag)
			{
				this._attackEnemyId = defenderId;
				this._bodyPart = bodyPart;
				if (isInner)
				{
					this._damageValue.Inner = this._damageValue.Inner + damageValue;
				}
				else
				{
					this._damageValue.Outer = this._damageValue.Outer + damageValue;
				}
			}
		}

		// Token: 0x0600384F RID: 14415 RVA: 0x00239E70 File Offset: 0x00238070
		private void AddDirectInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount, short combatSkillId)
		{
			bool flag = attackerId != base.CharacterId || combatSkillId != base.SkillTemplateId || this.IsSrcSkillPerformed;
			if (!flag)
			{
				sbyte markCount = base.IsDirect ? outerMarkCount : innerMarkCount;
				this._effectCount += (short)(markCount * 3);
			}
		}

		// Token: 0x06003850 RID: 14416 RVA: 0x00239EC4 File Offset: 0x002380C4
		private void AddDirectFatalDamageMark(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, int outerMarkCount, int innerMarkCount, short combatSkillId)
		{
			bool flag = attackerId != base.CharacterId || combatSkillId != base.SkillTemplateId || this.IsSrcSkillPerformed;
			if (!flag)
			{
				int markCount = base.IsDirect ? outerMarkCount : innerMarkCount;
				this._effectCount += (short)(markCount * 3);
			}
		}

		// Token: 0x06003851 RID: 14417 RVA: 0x00239F18 File Offset: 0x00238118
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || !base.PowerMatchAffectRequire((int)power, 0);
			if (!flag)
			{
				OuterAndInnerInts damageValue = this._damageValue;
				bool flag2 = damageValue.Outer <= 0 && damageValue.Inner <= 0;
				if (!flag2)
				{
					bool isSrcSkillPerformed = this.IsSrcSkillPerformed;
					if (isSrcSkillPerformed)
					{
						base.RemoveSelf(context);
					}
					else
					{
						bool flag3 = this._effectCount > 0;
						if (flag3)
						{
							DomainManager.Combat.AddSkillEffect(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), this._effectCount, this._effectCount, true);
						}
						this.IsSrcSkillPerformed = true;
						bool anyAdd = false;
						foreach (int enemyId in DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly))
						{
							bool flag4 = enemyId < 0 || enemyId == this._attackEnemyId;
							if (!flag4)
							{
								anyAdd = true;
								DomainManager.Combat.AddInjuryDamageValue(base.CombatChar, DomainManager.Combat.GetElement_CombatCharacterDict(enemyId), this._bodyPart, this._damageValue.Outer * 3, this._damageValue.Inner * 3, skillId, true);
							}
						}
						bool flag5 = anyAdd;
						if (flag5)
						{
							base.ShowSpecialEffectTips(0);
						}
					}
				}
			}
		}

		// Token: 0x06003852 RID: 14418 RVA: 0x0023A084 File Offset: 0x00238284
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003853 RID: 14419 RVA: 0x0023A0D4 File Offset: 0x002382D4
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
				bool flag2 = dataKey.FieldId == 116 && dataKey.CustomParam1 == (base.IsDirect ? 0 : 1);
				if (flag2)
				{
					int currInjury = dataKey.CustomParam2 + currModifyValue;
					bool flag3 = currInjury > 0;
					if (flag3)
					{
						return this.GetReduceValue(currInjury);
					}
				}
				result = 0;
			}
			return result;
		}

		// Token: 0x06003854 RID: 14420 RVA: 0x0023A144 File Offset: 0x00238344
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId == 192 || fieldId == 249;
				bool flag3 = flag2 && dataValue > 0;
				if (flag3)
				{
					result = dataValue + this.GetReduceValue(dataValue);
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x06003855 RID: 14421 RVA: 0x0023A1B0 File Offset: 0x002383B0
		private int GetReduceValue(int markCount)
		{
			int changeValue = Math.Min(base.EffectCount, markCount);
			base.ReduceEffectCount(changeValue);
			base.ShowSpecialEffectTips(1);
			return -changeValue;
		}

		// Token: 0x04001076 RID: 4214
		private const int DamageFactor = 3;

		// Token: 0x04001077 RID: 4215
		private const int EffectCountFactor = 3;

		// Token: 0x04001078 RID: 4216
		private int _attackEnemyId;

		// Token: 0x04001079 RID: 4217
		private sbyte _bodyPart;

		// Token: 0x0400107A RID: 4218
		private OuterAndInnerInts _damageValue;

		// Token: 0x0400107B RID: 4219
		private short _effectCount;
	}
}
