using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.DefenseAndAssist
{
	// Token: 0x02000436 RID: 1078
	public class QianJinZhui : DefenseSkillBase
	{
		// Token: 0x060039DB RID: 14811 RVA: 0x00240D5C File Offset: 0x0023EF5C
		public QianJinZhui()
		{
		}

		// Token: 0x060039DC RID: 14812 RVA: 0x00240D66 File Offset: 0x0023EF66
		public QianJinZhui(CombatSkillKey skillKey) : base(skillKey, 1502)
		{
		}

		// Token: 0x060039DD RID: 14813 RVA: 0x00240D78 File Offset: 0x0023EF78
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 148, -1, -1, -1, -1), EDataModifyType.Custom);
			base.ShowSpecialEffectTips(0);
			Events.RegisterHandler_IgnoredForceChangeDistance(new Events.OnIgnoredForceChangeDistance(this.IgnoredForceChangeDistance));
		}

		// Token: 0x060039DE RID: 14814 RVA: 0x00240DD4 File Offset: 0x0023EFD4
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_IgnoredForceChangeDistance(new Events.OnIgnoredForceChangeDistance(this.IgnoredForceChangeDistance));
			base.OnDisable(context);
		}

		// Token: 0x060039DF RID: 14815 RVA: 0x00240DF4 File Offset: 0x0023EFF4
		private void IgnoredForceChangeDistance(DataContext context, CombatCharacter mover, int distance)
		{
			bool flag = mover != base.CombatChar || (base.IsDirect ? (distance < 0) : (distance > 0)) || distance == 0 || !base.CanAffect;
			if (!flag)
			{
				base.ChangeStanceValue(context, base.CurrEnemyChar, -1200);
				base.ChangeBreathValue(context, base.CurrEnemyChar, -9000);
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x060039E0 RID: 14816 RVA: 0x00240E64 File Offset: 0x0023F064
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect || !(base.IsDirect ? (dataKey.CustomParam0 > 0) : (dataKey.CustomParam0 < 0));
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 148;
				result = (!flag2 && dataValue);
			}
			return result;
		}

		// Token: 0x040010EB RID: 4331
		private const int ChangeStanceBreathValuePercent = 30;
	}
}
