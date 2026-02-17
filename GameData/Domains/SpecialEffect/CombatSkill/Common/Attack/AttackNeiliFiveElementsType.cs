using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x02000590 RID: 1424
	public class AttackNeiliFiveElementsType : CombatSkillEffectBase
	{
		// Token: 0x0600422F RID: 16943 RVA: 0x00265ABB File Offset: 0x00263CBB
		protected AttackNeiliFiveElementsType()
		{
		}

		// Token: 0x06004230 RID: 16944 RVA: 0x00265AC5 File Offset: 0x00263CC5
		protected AttackNeiliFiveElementsType(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06004231 RID: 16945 RVA: 0x00265AD2 File Offset: 0x00263CD2
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004232 RID: 16946 RVA: 0x00265AF9 File Offset: 0x00263CF9
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004233 RID: 16947 RVA: 0x00265B20 File Offset: 0x00263D20
		private bool NeiliTypeMismatchAffectType(sbyte neiliType)
		{
			byte neiliFiveElements = NeiliType.Instance[neiliType].FiveElements;
			return neiliFiveElements != (byte)this.AffectFiveElementsType && neiliFiveElements != (byte)FiveElementsType.Countering[(int)this.AffectFiveElementsType];
		}

		// Token: 0x06004234 RID: 16948 RVA: 0x00265B64 File Offset: 0x00263D64
		private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = !this.SkillKey.IsMatch(charId, skillId);
			if (!flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, true);
				bool flag2 = this.NeiliTypeMismatchAffectType(enemyChar.GetNeiliType()) || !DomainManager.Combat.InAttackRange(base.CombatChar);
				if (flag2)
				{
					base.RemoveSelf(context);
				}
				else
				{
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						Dictionary<SkillEffectKey, short> skillEffectDict = enemyChar.GetSkillEffectCollection().EffectDict;
						bool flag3 = skillEffectDict != null && skillEffectDict.Count > 0;
						if (flag3)
						{
							List<SkillEffectKey> effectKeys = new List<SkillEffectKey>();
							int removeCount = Math.Min(skillEffectDict.Count, 3);
							effectKeys.AddRange(skillEffectDict.Keys);
							for (int i = 0; i < removeCount; i++)
							{
								SkillEffectKey key = effectKeys[context.Random.Next(0, effectKeys.Count)];
								effectKeys.Remove(key);
								DomainManager.Combat.ChangeSkillEffectCount(context, enemyChar, key, -skillEffectDict[key], true, false);
							}
						}
						base.ClearAffectingAgileSkill(context, enemyChar);
						DomainManager.Combat.ClearAffectingDefenseSkill(context, enemyChar);
						base.ShowSpecialEffectTips(0);
					}
					else
					{
						base.AppendAffectedData(context, base.CharacterId, 69, EDataModifyType.AddPercent, base.SkillTemplateId);
					}
				}
			}
		}

		// Token: 0x06004235 RID: 16949 RVA: 0x00265CD0 File Offset: 0x00263ED0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06004236 RID: 16950 RVA: 0x00265D08 File Offset: 0x00263F08
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.FieldId == 69 && dataKey.CombatSkillId == base.SkillTemplateId && base.CombatCharPowerMatchAffectRequire(0);
			int result;
			if (flag)
			{
				bool flag2 = !this._reverseTipsShowed;
				if (flag2)
				{
					base.ShowSpecialEffectTips(0);
					this._reverseTipsShowed = true;
				}
				result = 90;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x04001388 RID: 5000
		private const sbyte DirectClearEffect = 3;

		// Token: 0x04001389 RID: 5001
		private const sbyte ReverseAddDamagePercent = 90;

		// Token: 0x0400138A RID: 5002
		protected sbyte AffectFiveElementsType;

		// Token: 0x0400138B RID: 5003
		private bool _reverseTipsShowed;
	}
}
