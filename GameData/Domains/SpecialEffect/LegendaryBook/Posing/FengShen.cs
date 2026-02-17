using System;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Posing
{
	// Token: 0x02000137 RID: 311
	public class FengShen : CombatSkillEffectBase
	{
		// Token: 0x06002A73 RID: 10867 RVA: 0x002029A7 File Offset: 0x00200BA7
		public FengShen()
		{
			this.IsLegendaryBookEffect = true;
		}

		// Token: 0x06002A74 RID: 10868 RVA: 0x002029B8 File Offset: 0x00200BB8
		public FengShen(CombatSkillKey skillKey) : base(skillKey, 40102, -1)
		{
			this.IsLegendaryBookEffect = true;
		}

		// Token: 0x06002A75 RID: 10869 RVA: 0x002029D0 File Offset: 0x00200BD0
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x06002A76 RID: 10870 RVA: 0x002029E5 File Offset: 0x00200BE5
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x06002A77 RID: 10871 RVA: 0x002029FC File Offset: 0x00200BFC
		private void OnCombatBegin(DataContext context)
		{
			bool flag = !DomainManager.Combat.IsCharInCombat(base.CharacterId, true) || !base.CombatChar.GetAgileSkillList().Exist(base.SkillTemplateId) || !DomainManager.Combat.SkillCanUseInCurrCombat(this.CharObj.GetId(), Config.CombatSkill.Instance[base.SkillTemplateId]);
			if (!flag)
			{
				DomainManager.Combat.CastAgileOrDefenseWithoutPrepare(base.CombatChar, base.SkillTemplateId);
			}
		}
	}
}
