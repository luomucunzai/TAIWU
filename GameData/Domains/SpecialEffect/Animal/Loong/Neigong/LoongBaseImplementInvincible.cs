using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.SpecialEffect.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x020005E9 RID: 1513
	public class LoongBaseImplementInvincible : ISpecialEffectImplement, ISpecialEffectModifier
	{
		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06004494 RID: 17556 RVA: 0x0026FF8F File Offset: 0x0026E18F
		// (set) Token: 0x06004495 RID: 17557 RVA: 0x0026FF97 File Offset: 0x0026E197
		public CombatSkillEffectBase EffectBase { get; set; }

		// Token: 0x06004496 RID: 17558 RVA: 0x0026FFA0 File Offset: 0x0026E1A0
		public void OnEnable(DataContext context)
		{
			this.EffectBase.CreateAffectedData(282, EDataModifyType.Custom, -1);
		}

		// Token: 0x06004497 RID: 17559 RVA: 0x0026FFB6 File Offset: 0x0026E1B6
		public void OnDisable(DataContext context)
		{
		}

		// Token: 0x06004498 RID: 17560 RVA: 0x0026FFBC File Offset: 0x0026E1BC
		public bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != this.EffectBase.CharacterId || dataKey.FieldId != 282;
			return !flag || dataValue;
		}
	}
}
