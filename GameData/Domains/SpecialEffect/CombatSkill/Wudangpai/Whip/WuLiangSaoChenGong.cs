using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Whip
{
	// Token: 0x020003BA RID: 954
	public class WuLiangSaoChenGong : CombatSkillEffectBase
	{
		// Token: 0x0600371F RID: 14111 RVA: 0x00234074 File Offset: 0x00232274
		public WuLiangSaoChenGong()
		{
			sbyte[] array = new sbyte[5];
			array[0] = 40;
			array[1] = 20;
			this._directAddDamagePercentList = array;
			this._reverseAddPowerList = new sbyte[]
			{
				0,
				20,
				40,
				60,
				80
			};
			this._reverseAddDamagePercentList = new sbyte[]
			{
				0,
				0,
				20,
				40,
				60
			};
			base..ctor();
		}

		// Token: 0x06003720 RID: 14112 RVA: 0x002340E4 File Offset: 0x002322E4
		public WuLiangSaoChenGong(CombatSkillKey skillKey)
		{
			sbyte[] array = new sbyte[5];
			array[0] = 40;
			array[1] = 20;
			this._directAddDamagePercentList = array;
			this._reverseAddPowerList = new sbyte[]
			{
				0,
				20,
				40,
				60,
				80
			};
			this._reverseAddDamagePercentList = new sbyte[]
			{
				0,
				0,
				20,
				40,
				60
			};
			base..ctor(skillKey, 4306, -1);
		}

		// Token: 0x06003721 RID: 14113 RVA: 0x0023415C File Offset: 0x0023235C
		public override void OnEnable(DataContext context)
		{
			int disorderLevel = (int)DisorderLevelOfQi.GetDisorderLevelOfQi((base.IsDirect ? this.CharObj : base.CurrEnemyChar.GetCharacter()).GetDisorderOfQi());
			this._addPower = (int)(base.IsDirect ? this._directAddPowerList : this._reverseAddPowerList)[disorderLevel];
			bool flag = this._addPower > 0;
			if (flag)
			{
				this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
				base.ShowSpecialEffectTips(0);
			}
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003722 RID: 14114 RVA: 0x0023421A File Offset: 0x0023241A
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003723 RID: 14115 RVA: 0x00234244 File Offset: 0x00232444
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = context.SkillKey != this.SkillKey || index != 2 || !base.CombatCharPowerMatchAffectRequire(0);
			if (!flag)
			{
				int disorderLevel = (int)DisorderLevelOfQi.GetDisorderLevelOfQi((base.IsDirect ? this.CharObj : base.CurrEnemyChar.GetCharacter()).GetDisorderOfQi());
				this._addDamagePercent = (int)(base.IsDirect ? this._directAddDamagePercentList : this._reverseAddDamagePercentList)[disorderLevel];
				bool flag2 = this._addDamagePercent > 0;
				if (flag2)
				{
					base.AppendAffectedData(context, base.CharacterId, 69, EDataModifyType.AddPercent, base.SkillTemplateId);
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x06003724 RID: 14116 RVA: 0x002342F8 File Offset: 0x002324F8
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003725 RID: 14117 RVA: 0x00234330 File Offset: 0x00232530
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CombatSkillId != base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 199 && dataKey.CharId == base.CharacterId;
				if (flag2)
				{
					result = this._addPower;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 69;
					if (flag3)
					{
						result = this._addDamagePercent;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04001016 RID: 4118
		private readonly sbyte[] _directAddPowerList = new sbyte[]
		{
			40,
			30,
			20,
			10,
			0
		};

		// Token: 0x04001017 RID: 4119
		private readonly sbyte[] _directAddDamagePercentList;

		// Token: 0x04001018 RID: 4120
		private readonly sbyte[] _reverseAddPowerList;

		// Token: 0x04001019 RID: 4121
		private readonly sbyte[] _reverseAddDamagePercentList;

		// Token: 0x0400101A RID: 4122
		private int _addPower;

		// Token: 0x0400101B RID: 4123
		private int _addDamagePercent;
	}
}
