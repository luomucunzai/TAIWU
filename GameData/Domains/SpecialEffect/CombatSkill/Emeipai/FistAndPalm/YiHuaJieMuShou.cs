using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.FistAndPalm
{
	// Token: 0x02000555 RID: 1365
	public class YiHuaJieMuShou : CombatSkillEffectBase
	{
		// Token: 0x06004065 RID: 16485 RVA: 0x0025DF25 File Offset: 0x0025C125
		public YiHuaJieMuShou()
		{
		}

		// Token: 0x06004066 RID: 16486 RVA: 0x0025DF2F File Offset: 0x0025C12F
		public YiHuaJieMuShou(CombatSkillKey skillKey) : base(skillKey, 2104, -1)
		{
		}

		// Token: 0x06004067 RID: 16487 RVA: 0x0025DF40 File Offset: 0x0025C140
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004068 RID: 16488 RVA: 0x0025DF67 File Offset: 0x0025C167
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004069 RID: 16489 RVA: 0x0025DF90 File Offset: 0x0025C190
		private unsafe void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || !DomainManager.Combat.InAttackRange(base.CombatChar);
			if (!flag)
			{
				MainAttributes selfAttributes = this.CharObj.GetMaxMainAttributes();
				MainAttributes enemyAttributes = base.CurrEnemyChar.GetCharacter().GetMaxMainAttributes();
				bool flag2 = *(ref selfAttributes.Items.FixedElementField + 2) > *(ref enemyAttributes.Items.FixedElementField + 2);
				if (flag2)
				{
					OuterAndInnerInts penetrationResists = base.CurrEnemyChar.GetCharacter().GetPenetrationResists();
					this._transferPenetrateResist = (base.IsDirect ? penetrationResists.Outer : penetrationResists.Inner) * 40 / 100;
					base.AppendAffectedCurrEnemyData(context, 46, EDataModifyType.Add, -1);
					base.AppendAffectedCurrEnemyData(context, 47, EDataModifyType.Add, -1);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x0600406A RID: 16490 RVA: 0x0025E06C File Offset: 0x0025C26C
		private unsafe void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				MainAttributes selfAttributes = this.CharObj.GetCurrMainAttributes();
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0) && *(ref selfAttributes.Items.FixedElementField + 2) >= 10;
				if (flag2)
				{
					this.CharObj.ChangeCurrMainAttribute(context, 1, -10);
					DomainManager.Combat.AddCombatState(context, base.CurrEnemyChar, 1, base.IsDirect ? 13 : 15, 250);
					DomainManager.Combat.AddCombatState(context, base.CurrEnemyChar, 2, base.IsDirect ? 14 : 16, 250);
					base.ShowSpecialEffectTips(1);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0600406B RID: 16491 RVA: 0x0025E140 File Offset: 0x0025C340
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CurrEnemyChar.GetId();
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == (base.IsDirect ? 47 : 46);
				if (flag2)
				{
					result = this._transferPenetrateResist;
				}
				else
				{
					bool flag3 = dataKey.FieldId == (base.IsDirect ? 46 : 47);
					if (flag3)
					{
						result = -this._transferPenetrateResist;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x040012EB RID: 4843
		private const sbyte TransferPenetrateResistPercent = 40;

		// Token: 0x040012EC RID: 4844
		private const sbyte CostAttribute = 10;

		// Token: 0x040012ED RID: 4845
		private const short StatePower = 250;

		// Token: 0x040012EE RID: 4846
		private int _transferPenetrateResist;
	}
}
