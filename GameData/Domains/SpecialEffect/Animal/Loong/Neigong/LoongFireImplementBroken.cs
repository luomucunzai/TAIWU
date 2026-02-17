using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.SpecialEffect.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x020005F2 RID: 1522
	public class LoongFireImplementBroken : ISpecialEffectImplement, ISpecialEffectModifier
	{
		// Token: 0x170002CA RID: 714
		// (get) Token: 0x060044C1 RID: 17601 RVA: 0x002707FF File Offset: 0x0026E9FF
		// (set) Token: 0x060044C2 RID: 17602 RVA: 0x00270807 File Offset: 0x0026EA07
		public CombatSkillEffectBase EffectBase { get; set; }

		// Token: 0x060044C3 RID: 17603 RVA: 0x00270810 File Offset: 0x0026EA10
		public void OnEnable(DataContext context)
		{
			this.EffectBase.CreateAffectedAllEnemyData(168, EDataModifyType.Custom, -1);
		}

		// Token: 0x060044C4 RID: 17604 RVA: 0x00270826 File Offset: 0x0026EA26
		public void OnDisable(DataContext context)
		{
		}

		// Token: 0x060044C5 RID: 17605 RVA: 0x0027082C File Offset: 0x0026EA2C
		public int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId == this.EffectBase.CharacterId || dataKey.FieldId != 168;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				result = Math.Min(dataValue, 4);
			}
			return result;
		}

		// Token: 0x04001458 RID: 5208
		private const sbyte BreakNeedInjury = 4;
	}
}
