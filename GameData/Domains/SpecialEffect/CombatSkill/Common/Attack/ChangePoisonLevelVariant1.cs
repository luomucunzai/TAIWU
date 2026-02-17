using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x02000596 RID: 1430
	public abstract class ChangePoisonLevelVariant1 : ChangePoisonLevel
	{
		// Token: 0x06004271 RID: 17009 RVA: 0x00266E43 File Offset: 0x00265043
		protected ChangePoisonLevelVariant1()
		{
		}

		// Token: 0x06004272 RID: 17010 RVA: 0x00266E4D File Offset: 0x0026504D
		protected ChangePoisonLevelVariant1(CombatSkillKey skillKey, int type) : base(skillKey, type)
		{
		}

		// Token: 0x06004273 RID: 17011 RVA: 0x00266E5C File Offset: 0x0026505C
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.CreateAffectedData(217, EDataModifyType.Custom, -1);
				base.CreateAffectedData(215, EDataModifyType.Custom, -1);
			}
			else
			{
				Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			}
		}

		// Token: 0x06004274 RID: 17012 RVA: 0x00266EB0 File Offset: 0x002650B0
		public override void OnDisable(DataContext context)
		{
			bool flag = !base.IsDirect;
			if (flag)
			{
				Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			}
			base.OnDisable(context);
		}

		// Token: 0x06004275 RID: 17013 RVA: 0x00266EE8 File Offset: 0x002650E8
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = defender != base.CombatChar || !this.AffectingSkillKey.IsMatch(attacker.GetId(), skillId);
			if (!flag)
			{
				DomainManager.Combat.AddGoneMadInjury(context, attacker, skillId, -50);
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x06004276 RID: 17014 RVA: 0x00266F38 File Offset: 0x00265138
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.CombatSkillId != this.AffectingSkillKey.SkillTemplateId;
				if (flag2)
				{
					bool flag3 = base.EffectCount > 0 && dataValue && base.IsMatchPoison(dataKey.CombatSkillId);
					if (!flag3)
					{
						return dataValue;
					}
					base.ReduceEffectCount(1);
				}
				ushort fieldId = dataKey.FieldId;
				bool flag4 = fieldId == 215 || fieldId == 217;
				bool flag5 = flag4;
				if (flag5)
				{
					base.ShowSpecialEffectTips(1);
				}
				ushort fieldId2 = dataKey.FieldId;
				if (!true)
				{
				}
				flag4 = (fieldId2 != 215 && fieldId2 != 217 && dataValue);
				if (!true)
				{
				}
				result = flag4;
			}
			return result;
		}

		// Token: 0x040013A3 RID: 5027
		private const int AddGoneMadFactor = -50;
	}
}
