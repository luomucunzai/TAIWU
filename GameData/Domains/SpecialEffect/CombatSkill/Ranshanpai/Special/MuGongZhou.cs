using System;
using System.Linq;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Special
{
	// Token: 0x0200044F RID: 1103
	public class MuGongZhou : CurseSilenceCombatSkill
	{
		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06003A7A RID: 14970 RVA: 0x00243C01 File Offset: 0x00241E01
		protected override sbyte TargetEquipType
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06003A7B RID: 14971 RVA: 0x00243C04 File Offset: 0x00241E04
		public MuGongZhou()
		{
		}

		// Token: 0x06003A7C RID: 14972 RVA: 0x00243C0E File Offset: 0x00241E0E
		public MuGongZhou(CombatSkillKey skillKey) : base(skillKey, 7305)
		{
		}

		// Token: 0x06003A7D RID: 14973 RVA: 0x00243C1E File Offset: 0x00241E1E
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_PrepareSkillProgressChange(new Events.OnPrepareSkillProgressChange(this.OnPrepareSkillProgressChange));
		}

		// Token: 0x06003A7E RID: 14974 RVA: 0x00243C3B File Offset: 0x00241E3B
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillProgressChange(new Events.OnPrepareSkillProgressChange(this.OnPrepareSkillProgressChange));
			base.OnDisable(context);
		}

		// Token: 0x06003A7F RID: 14975 RVA: 0x00243C58 File Offset: 0x00241E58
		private void OnPrepareSkillProgressChange(DataContext context, int charId, bool isAlly, short skillId, sbyte preparePercent)
		{
			bool flag = Config.CombatSkill.Instance[skillId].EquipType != 2 || preparePercent != 90 || base.SilencingSkills.All((CombatSkillKey x) => x.CharId != charId) || !context.Random.CheckPercentProb(50);
			if (!flag)
			{
				DomainManager.Combat.InterruptSkill(context, DomainManager.Combat.GetElement_CombatCharacterDict(charId), 100);
				base.ShowSpecialEffectTips(2);
			}
		}

		// Token: 0x06003A80 RID: 14976 RVA: 0x00243CE5 File Offset: 0x00241EE5
		protected override void OnSilenceBegin(DataContext context, CombatSkillKey skillKey)
		{
		}

		// Token: 0x06003A81 RID: 14977 RVA: 0x00243CE8 File Offset: 0x00241EE8
		protected override void OnSilenceEnd(DataContext context, CombatSkillKey skillKey)
		{
		}

		// Token: 0x04001123 RID: 4387
		private const sbyte InterruptPreparePercent = 90;

		// Token: 0x04001124 RID: 4388
		private const sbyte InterruptOdds = 50;
	}
}
