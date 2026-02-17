using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Neigong
{
	// Token: 0x0200061C RID: 1564
	public class LingMaoZhuYing : AnimalEffectBase
	{
		// Token: 0x170002EA RID: 746
		// (get) Token: 0x060045A4 RID: 17828 RVA: 0x00273097 File Offset: 0x00271297
		private int FightBackPower
		{
			get
			{
				return base.IsElite ? 250 : 150;
			}
		}

		// Token: 0x060045A5 RID: 17829 RVA: 0x002730AD File Offset: 0x002712AD
		public LingMaoZhuYing()
		{
		}

		// Token: 0x060045A6 RID: 17830 RVA: 0x002730B7 File Offset: 0x002712B7
		public LingMaoZhuYing(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x060045A7 RID: 17831 RVA: 0x002730C4 File Offset: 0x002712C4
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 112, -1, -1, -1, -1), EDataModifyType.Add);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 250, -1, -1, -1, -1), EDataModifyType.Custom);
		}

		// Token: 0x060045A8 RID: 17832 RVA: 0x0027311C File Offset: 0x0027131C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 112 || !DomainManager.Combat.InAttackRange(base.CombatChar);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = this.FightBackPower;
			}
			return result;
		}

		// Token: 0x060045A9 RID: 17833 RVA: 0x0027316C File Offset: 0x0027136C
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 250 || !DomainManager.Combat.InAttackRange(base.CombatChar);
			bool result;
			if (flag)
			{
				result = base.GetModifiedValue(dataKey, dataValue);
			}
			else
			{
				base.ShowSpecialEffectTipsOnceInFrame(0);
				result = true;
			}
			return result;
		}
	}
}
