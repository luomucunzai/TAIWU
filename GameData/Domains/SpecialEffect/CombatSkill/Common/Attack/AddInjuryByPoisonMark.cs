using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x02000587 RID: 1415
	public class AddInjuryByPoisonMark : CombatSkillEffectBase
	{
		// Token: 0x17000284 RID: 644
		// (get) Token: 0x060041E7 RID: 16871 RVA: 0x00264A48 File Offset: 0x00262C48
		protected virtual bool AlsoAddFlaw
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x060041E8 RID: 16872 RVA: 0x00264A4B File Offset: 0x00262C4B
		protected virtual bool AlsoAddAcupoint
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060041E9 RID: 16873 RVA: 0x00264A4E File Offset: 0x00262C4E
		protected AddInjuryByPoisonMark()
		{
		}

		// Token: 0x060041EA RID: 16874 RVA: 0x00264A58 File Offset: 0x00262C58
		protected AddInjuryByPoisonMark(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x060041EB RID: 16875 RVA: 0x00264A65 File Offset: 0x00262C65
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060041EC RID: 16876 RVA: 0x00264A7A File Offset: 0x00262C7A
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060041ED RID: 16877 RVA: 0x00264A90 File Offset: 0x00262C90
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, true);
					byte poisonMarkCount = (base.IsDirect ? enemyChar : base.CombatChar).GetDefeatMarkCollection().PoisonMarkList[(int)this.RequirePoisonType];
					bool flag3 = poisonMarkCount > 0;
					if (flag3)
					{
						DomainManager.Combat.AddRandomInjury(context, enemyChar, this.IsInnerInjury, (int)poisonMarkCount, 1, false, -1);
						bool alsoAddFlaw = this.AlsoAddFlaw;
						if (alsoAddFlaw)
						{
							DomainManager.Combat.AddFlaw(context, enemyChar, 1, this.SkillKey, -1, (int)poisonMarkCount, true);
						}
						bool alsoAddAcupoint = this.AlsoAddAcupoint;
						if (alsoAddAcupoint)
						{
							DomainManager.Combat.AddAcupoint(context, enemyChar, 1, this.SkillKey, -1, (int)poisonMarkCount, true);
						}
						base.ShowSpecialEffectTips(0);
						base.ShowSpecialEffectTips(1);
					}
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04001371 RID: 4977
		private const sbyte AddFlawOrAcupointLevel = 1;

		// Token: 0x04001372 RID: 4978
		protected sbyte RequirePoisonType;

		// Token: 0x04001373 RID: 4979
		protected bool IsInnerInjury;
	}
}
