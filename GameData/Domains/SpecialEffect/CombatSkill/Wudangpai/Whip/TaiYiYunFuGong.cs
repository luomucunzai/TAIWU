using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Whip
{
	// Token: 0x020003B8 RID: 952
	public class TaiYiYunFuGong : CombatSkillEffectBase
	{
		// Token: 0x06003711 RID: 14097 RVA: 0x00233AF1 File Offset: 0x00231CF1
		public TaiYiYunFuGong()
		{
			sbyte[] array = new sbyte[5];
			array[0] = 1;
			array[1] = 1;
			this._addAcupointLevelList = array;
			base..ctor();
		}

		// Token: 0x06003712 RID: 14098 RVA: 0x00233B26 File Offset: 0x00231D26
		public TaiYiYunFuGong(CombatSkillKey skillKey)
		{
			sbyte[] array = new sbyte[5];
			array[0] = 1;
			array[1] = 1;
			this._addAcupointLevelList = array;
			base..ctor(skillKey, 4305, -1);
		}

		// Token: 0x06003713 RID: 14099 RVA: 0x00233B62 File Offset: 0x00231D62
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003714 RID: 14100 RVA: 0x00233B89 File Offset: 0x00231D89
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003715 RID: 14101 RVA: 0x00233BB0 File Offset: 0x00231DB0
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = context.SkillKey != this.SkillKey || index != 2 || !base.CombatCharPowerMatchAffectRequire(0);
			if (!flag)
			{
				int disorderLevel = (int)DisorderLevelOfQi.GetDisorderLevelOfQi((base.IsDirect ? this.CharObj : base.CurrEnemyChar.GetCharacter()).GetDisorderOfQi());
				this._addAcupointCount = (int)this._addAcupointCountList[base.IsDirect ? disorderLevel : (5 - disorderLevel - 1)];
				this._addAcupointLevel = (int)this._addAcupointLevelList[base.IsDirect ? disorderLevel : (5 - disorderLevel - 1)];
				bool flag2 = this._addAcupointCount > 0;
				if (flag2)
				{
					base.AppendAffectedCurrEnemyData(context, 134, EDataModifyType.Add, base.SkillTemplateId);
				}
				bool flag3 = this._addAcupointLevel > 0;
				if (flag3)
				{
					base.AppendAffectedCurrEnemyData(context, 132, EDataModifyType.Add, base.SkillTemplateId);
				}
				bool flag4 = this._addAcupointCount > 0 || this._addAcupointLevel > 0;
				if (flag4)
				{
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06003716 RID: 14102 RVA: 0x00233CBC File Offset: 0x00231EBC
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003717 RID: 14103 RVA: 0x00233CF4 File Offset: 0x00231EF4
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CurrEnemyChar.GetId() || dataKey.CombatSkillId != base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 134;
				if (flag2)
				{
					result = this._addAcupointCount;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 132;
					if (flag3)
					{
						result = this._addAcupointLevel;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04001011 RID: 4113
		private readonly sbyte[] _addAcupointCountList = new sbyte[]
		{
			2,
			1,
			1,
			0,
			0
		};

		// Token: 0x04001012 RID: 4114
		private readonly sbyte[] _addAcupointLevelList;

		// Token: 0x04001013 RID: 4115
		private int _addAcupointCount;

		// Token: 0x04001014 RID: 4116
		private int _addAcupointLevel;
	}
}
