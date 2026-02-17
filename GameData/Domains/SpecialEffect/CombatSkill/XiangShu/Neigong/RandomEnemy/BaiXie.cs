using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Neigong.RandomEnemy
{
	// Token: 0x02000290 RID: 656
	public class BaiXie : MinionBase
	{
		// Token: 0x0600313D RID: 12605 RVA: 0x0021A523 File Offset: 0x00218723
		public BaiXie()
		{
		}

		// Token: 0x0600313E RID: 12606 RVA: 0x0021A52D File Offset: 0x0021872D
		public BaiXie(CombatSkillKey skillKey) : base(skillKey, 16004)
		{
		}

		// Token: 0x0600313F RID: 12607 RVA: 0x0021A53D File Offset: 0x0021873D
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
		}

		// Token: 0x06003140 RID: 12608 RVA: 0x0021A552 File Offset: 0x00218752
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
		}

		// Token: 0x06003141 RID: 12609 RVA: 0x0021A568 File Offset: 0x00218768
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = base.CombatChar.IsAlly == isAlly || !base.IsCurrent || !MinionBase.CanAffect;
			if (!flag)
			{
				sbyte direction = DomainManager.CombatSkill.GetElement_CombatSkills(new ValueTuple<int, short>(charId, skillId)).GetDirection();
				bool flag2 = direction == -1;
				if (!flag2)
				{
					bool worsen = direction == 0;
					CombatCharacter affectChar = worsen ? DomainManager.Combat.GetElement_CombatCharacterDict(charId) : base.CombatChar;
					bool affected = worsen ? affectChar.WorsenRandomInjury(context, WorsenConstants.SpecialPercentBaiXie) : affectChar.RemoveRandomInjury(context, 2);
					bool flag3 = affected;
					if (flag3)
					{
						base.ShowSpecialEffectTips(worsen, 0, 1);
					}
				}
			}
		}

		// Token: 0x04000E98 RID: 3736
		private const sbyte ChangeInjury = 2;
	}
}
