using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Blade
{
	// Token: 0x020004D5 RID: 1237
	public class RuYiBaoLunDao : CombatSkillEffectBase
	{
		// Token: 0x06003D85 RID: 15749 RVA: 0x002522C1 File Offset: 0x002504C1
		public RuYiBaoLunDao()
		{
		}

		// Token: 0x06003D86 RID: 15750 RVA: 0x002522CB File Offset: 0x002504CB
		public RuYiBaoLunDao(CombatSkillKey skillKey) : base(skillKey, 11206, -1)
		{
		}

		// Token: 0x06003D87 RID: 15751 RVA: 0x002522DC File Offset: 0x002504DC
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06003D88 RID: 15752 RVA: 0x00252303 File Offset: 0x00250503
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06003D89 RID: 15753 RVA: 0x0025232C File Offset: 0x0025052C
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
						bool isDirect = base.IsDirect;
						if (isDirect)
						{
							base.AppendAffectedAllEnemyData(context, 166, EDataModifyType.Custom, -1);
						}
						else
						{
							base.AppendAffectedData(context, base.CharacterId, 166, EDataModifyType.Custom, -1);
						}
						base.AddMaxEffectCount(true);
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

		// Token: 0x06003D8A RID: 15754 RVA: 0x002523E0 File Offset: 0x002505E0
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003D8B RID: 15755 RVA: 0x00252430 File Offset: 0x00250630
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = !DomainManager.Combat.IsCurrentCombatCharacter(base.CombatChar) || dataKey.FieldId != 166;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				int stateType = dataKey.CustomParam0;
				bool flag2 = stateType != (base.IsDirect ? 1 : 2);
				if (flag2)
				{
					result = dataValue;
				}
				else
				{
					short stateId = (short)dataValue;
					int power = dataKey.CustomParam1;
					bool reverse = dataKey.CustomParam2 == 1;
					DataContext context = DomainManager.Combat.Context;
					CombatCharacter enemyChar = base.IsDirect ? DomainManager.Combat.GetElement_CombatCharacterDict(dataKey.CharId) : DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
					ValueTuple<short, bool> valueTuple = CombatDomain.CalcReversedCombatState(stateId, reverse);
					short rvsStateId = valueTuple.Item1;
					bool rvsReverse = valueTuple.Item2;
					DomainManager.Combat.AddCombatState(context, base.IsDirect ? base.CombatChar : enemyChar, base.IsDirect ? 1 : 2, stateId, power * 2, reverse, false);
					DomainManager.Combat.AddCombatState(context, base.IsDirect ? enemyChar : base.CombatChar, base.IsDirect ? 2 : 1, rvsStateId, power, rvsReverse, false);
					base.ShowSpecialEffectTips(0);
					base.ReduceEffectCount(1);
					result = -1;
				}
			}
			return result;
		}
	}
}
