using System;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill;

namespace GameData.Domains.SpecialEffect.Animal
{
	// Token: 0x020005E6 RID: 1510
	public abstract class AnimalEffectBase : CombatSkillEffectBase
	{
		// Token: 0x0600447C RID: 17532 RVA: 0x0026FC1B File Offset: 0x0026DE1B
		protected AnimalEffectBase()
		{
		}

		// Token: 0x0600447D RID: 17533 RVA: 0x0026FC25 File Offset: 0x0026DE25
		protected AnimalEffectBase(CombatSkillKey skillKey) : base(skillKey, -1, -1)
		{
		}

		// Token: 0x0600447E RID: 17534 RVA: 0x0026FC32 File Offset: 0x0026DE32
		public override void OnDataAdded(DataContext context)
		{
			base.AppendAffectedData(context, base.CharacterId, 217, EDataModifyType.Custom, base.SkillTemplateId);
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x0600447F RID: 17535 RVA: 0x0026FC4F File Offset: 0x0026DE4F
		protected bool CanAffect
		{
			get
			{
				return base.CombatChar.AnimalConfig != null;
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06004480 RID: 17536 RVA: 0x0026FC5F File Offset: 0x0026DE5F
		protected bool IsElite
		{
			get
			{
				AnimalItem animalConfig = base.CombatChar.AnimalConfig;
				return animalConfig != null && animalConfig.IsElite;
			}
		}

		// Token: 0x06004481 RID: 17537 RVA: 0x0026FC78 File Offset: 0x0026DE78
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId || dataKey.FieldId != 217;
			return flag && dataValue;
		}
	}
}
