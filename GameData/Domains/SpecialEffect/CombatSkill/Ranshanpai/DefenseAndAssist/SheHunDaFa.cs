using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.DefenseAndAssist
{
	// Token: 0x02000464 RID: 1124
	public class SheHunDaFa : DefenseSkillBase
	{
		// Token: 0x06003AFD RID: 15101 RVA: 0x00246136 File Offset: 0x00244336
		public SheHunDaFa()
		{
		}

		// Token: 0x06003AFE RID: 15102 RVA: 0x0024614B File Offset: 0x0024434B
		public SheHunDaFa(CombatSkillKey skillKey) : base(skillKey, 7503)
		{
		}

		// Token: 0x06003AFF RID: 15103 RVA: 0x00246166 File Offset: 0x00244366
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AutoRemove = false;
			Events.RegisterHandler_GetTrick(new Events.OnGetTrick(this.OnGetTrick));
			Events.RegisterHandler_AddMindMark(new Events.OnAddMindMark(this.OnAddMindMark));
		}

		// Token: 0x06003B00 RID: 15104 RVA: 0x0024619C File Offset: 0x0024439C
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_GetTrick(new Events.OnGetTrick(this.OnGetTrick));
			Events.UnRegisterHandler_AddMindMark(new Events.OnAddMindMark(this.OnAddMindMark));
			base.OnDisable(context);
		}

		// Token: 0x06003B01 RID: 15105 RVA: 0x002461CC File Offset: 0x002443CC
		private void OnGetTrick(DataContext context, int charId, bool isAlly, sbyte trickType, bool usable)
		{
			bool flag = charId != base.CharacterId || trickType != 20 || SheHunDaFa._affectingTrick || !base.CanAffect || !base.IsDirect;
			if (!flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!isAlly, false);
				SheHunDaFa._affectingTrick = true;
				DomainManager.Combat.AddTrick(context, enemyChar, 20, 1, false, false);
				SheHunDaFa._affectingTrick = false;
				base.ShowSpecialEffectTips(0);
				this.DoSilence(context, enemyChar);
			}
		}

		// Token: 0x06003B02 RID: 15106 RVA: 0x0024624C File Offset: 0x0024444C
		private void OnAddMindMark(DataContext context, CombatCharacter character, int count)
		{
			bool flag = character.GetId() != base.CharacterId || count <= 0 || SheHunDaFa._affectingMindMark || !base.CanAffect || base.IsDirect;
			if (!flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!character.IsAlly, false);
				SheHunDaFa._affectingMindMark = true;
				DomainManager.Combat.AppendMindDefeatMark(context, enemyChar, 1, -1, false);
				SheHunDaFa._affectingMindMark = false;
				base.ShowSpecialEffectTips(0);
				this.DoSilence(context, enemyChar);
			}
		}

		// Token: 0x06003B03 RID: 15107 RVA: 0x002462CC File Offset: 0x002444CC
		private void DoSilence(DataContext context, CombatCharacter enemyChar)
		{
			bool flag = base.CombatChar.GetCharacter().GetHitValues()[3] <= enemyChar.GetCharacter().GetAvoidValues()[3];
			if (!flag)
			{
				short skillId = enemyChar.GetRandomBanableSkillId(context.Random, delegate(short x)
				{
					List<short> silenced;
					return !this._silencedSkills.TryGetValue(enemyChar.GetId(), out silenced) || !silenced.Contains(x);
				}, -1);
				bool flag2 = skillId < 0;
				if (!flag2)
				{
					DomainManager.Combat.SilenceSkill(context, enemyChar, skillId, 3000, 100);
					this._silencedSkills.GetOrNew(enemyChar.GetId()).Add(skillId);
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x04001148 RID: 4424
		private const int SilenceFrame = 3000;

		// Token: 0x04001149 RID: 4425
		private static bool _affectingTrick;

		// Token: 0x0400114A RID: 4426
		private static bool _affectingMindMark;

		// Token: 0x0400114B RID: 4427
		private readonly Dictionary<int, List<short>> _silencedSkills = new Dictionary<int, List<short>>();
	}
}
