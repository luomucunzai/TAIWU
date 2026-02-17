using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.SpecialEffect.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x020005F3 RID: 1523
	public class LoongFireImplementBrokenHit : ISpecialEffectImplement, ISpecialEffectModifier
	{
		// Token: 0x170002CB RID: 715
		// (get) Token: 0x060044C7 RID: 17607 RVA: 0x0027087C File Offset: 0x0026EA7C
		// (set) Token: 0x060044C8 RID: 17608 RVA: 0x00270884 File Offset: 0x0026EA84
		public CombatSkillEffectBase EffectBase { get; set; }

		// Token: 0x060044C9 RID: 17609 RVA: 0x0027088D File Offset: 0x0026EA8D
		public void OnEnable(DataContext context)
		{
			this.EffectBase.CreateAffectedData(251, EDataModifyType.Custom, -1);
		}

		// Token: 0x060044CA RID: 17610 RVA: 0x002708A3 File Offset: 0x0026EAA3
		public void OnDisable(DataContext context)
		{
		}

		// Token: 0x060044CB RID: 17611 RVA: 0x002708A8 File Offset: 0x0026EAA8
		public bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != this.EffectBase.CharacterId || dataKey.FieldId != 251 || !dataKey.IsNormalAttack;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				CombatCharacter enemyChar = this.EffectBase.CurrEnemyChar;
				sbyte bodyPartType = this.EffectBase.CombatChar.NormalAttackBodyPart;
				bool flag2 = !DomainManager.Combat.CheckBodyPartInjury(enemyChar, bodyPartType, false);
				if (flag2)
				{
					result = dataValue;
				}
				else
				{
					this.EffectBase.ShowSpecialEffectTips(1);
					result = true;
				}
			}
			return result;
		}
	}
}
