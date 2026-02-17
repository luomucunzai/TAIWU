using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.SpecialEffect.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x02000608 RID: 1544
	public class LoongWaterImplementMix : ISpecialEffectImplement, ISpecialEffectModifier
	{
		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06004543 RID: 17731 RVA: 0x00271F85 File Offset: 0x00270185
		// (set) Token: 0x06004544 RID: 17732 RVA: 0x00271F8D File Offset: 0x0027018D
		public CombatSkillEffectBase EffectBase { get; set; }

		// Token: 0x06004545 RID: 17733 RVA: 0x00271F96 File Offset: 0x00270196
		public void OnEnable(DataContext context)
		{
			this.EffectBase.CreateAffectedAllEnemyData(272, EDataModifyType.Custom, -1);
		}

		// Token: 0x06004546 RID: 17734 RVA: 0x00271FAC File Offset: 0x002701AC
		public void OnDisable(DataContext context)
		{
		}

		// Token: 0x06004547 RID: 17735 RVA: 0x00271FB0 File Offset: 0x002701B0
		public bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId == this.EffectBase.CharacterId || dataKey.FieldId != 272;
			return !flag || dataValue;
		}
	}
}
