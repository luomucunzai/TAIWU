using System;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x0200059C RID: 1436
	public class IgnoreArmor : CombatSkillEffectBase
	{
		// Token: 0x060042A5 RID: 17061 RVA: 0x00267E1F File Offset: 0x0026601F
		protected IgnoreArmor()
		{
		}

		// Token: 0x060042A6 RID: 17062 RVA: 0x00267E29 File Offset: 0x00266029
		protected IgnoreArmor(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x060042A7 RID: 17063 RVA: 0x00267E38 File Offset: 0x00266038
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(281, EDataModifyType.Custom, -1);
			base.CreateAffectedData(base.IsDirect ? 44 : 46, EDataModifyType.AddPercent, -1);
			base.CreateAffectedData(base.IsDirect ? 45 : 47, EDataModifyType.AddPercent, -1);
			Events.RegisterHandler_AttackSkillAttackBegin(new Events.OnAttackSkillAttackBegin(this.OnAttackSkillAttackBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
		}

		// Token: 0x060042A8 RID: 17064 RVA: 0x00267EB8 File Offset: 0x002660B8
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AttackSkillAttackBegin(new Events.OnAttackSkillAttackBegin(this.OnAttackSkillAttackBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
		}

		// Token: 0x060042A9 RID: 17065 RVA: 0x00267EF4 File Offset: 0x002660F4
		private void OnAttackSkillAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId, int index, bool hit)
		{
			bool flag = attacker.GetId() != base.CharacterId || skillId != base.SkillTemplateId || !hit;
			if (!flag)
			{
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060042AA RID: 17066 RVA: 0x00267F30 File Offset: 0x00266130
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = this._affecting && (base.IsDirect ? (charId == base.CharacterId) : (isAlly != base.CombatChar.IsAlly));
			if (flag)
			{
				this._affecting = false;
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, base.IsDirect ? 44 : 46);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, base.IsDirect ? 45 : 47);
			}
			bool flag2 = charId != base.CharacterId || skillId != base.SkillTemplateId || !base.PowerMatchAffectRequire((int)power, 0);
			if (!flag2)
			{
				base.AddMaxEffectCount(true);
			}
		}

		// Token: 0x060042AB RID: 17067 RVA: 0x00267FEC File Offset: 0x002661EC
		private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = !DomainManager.Combat.IsCurrentCombatCharacter(base.CombatChar) || base.EffectCount <= 0 || charId != (base.IsDirect ? base.CharacterId : base.CurrEnemyChar.GetId()) || Config.CombatSkill.Instance[skillId].EquipType != 1;
			if (!flag)
			{
				short originalMainAttribute = this.CharObj.GetCurrMainAttribute(this.RequireMainAttributeType);
				bool flag2 = originalMainAttribute < 10;
				if (!flag2)
				{
					this.CharObj.ChangeCurrMainAttribute(context, this.RequireMainAttributeType, -10);
					short currentMainAttribute = this.CharObj.GetCurrMainAttribute(this.RequireMainAttributeType);
					bool flag3 = originalMainAttribute <= currentMainAttribute;
					if (!flag3)
					{
						this._deltaMainAttribute = (int)(originalMainAttribute - currentMainAttribute);
						this._affecting = true;
						DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, base.IsDirect ? 44 : 46);
						DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, base.IsDirect ? 45 : 47);
						base.ShowSpecialEffectTips(1);
						base.ReduceEffectCount(1);
					}
				}
			}
		}

		// Token: 0x060042AC RID: 17068 RVA: 0x0026810C File Offset: 0x0026630C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !this._affecting;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId - 44 <= 3;
				bool flag3 = flag2;
				if (flag3)
				{
					result = this._deltaMainAttribute * 3;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x060042AD RID: 17069 RVA: 0x0026816C File Offset: 0x0026636C
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.SkillKey != this.SkillKey || dataKey.FieldId != 281;
			return !flag || dataValue;
		}

		// Token: 0x040013B7 RID: 5047
		private const sbyte CostMainAttribute = 10;

		// Token: 0x040013B8 RID: 5048
		private const sbyte PenetrateOrResistChangeUnitPercent = 3;

		// Token: 0x040013B9 RID: 5049
		protected sbyte RequireMainAttributeType;

		// Token: 0x040013BA RID: 5050
		private bool _affecting;

		// Token: 0x040013BB RID: 5051
		private int _deltaMainAttribute;
	}
}
