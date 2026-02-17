using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.DefenseAndAssist
{
	// Token: 0x02000498 RID: 1176
	public class BaiShuiChangShengFa : AssistSkillBase
	{
		// Token: 0x06003C36 RID: 15414 RVA: 0x0024C5BB File Offset: 0x0024A7BB
		public BaiShuiChangShengFa()
		{
		}

		// Token: 0x06003C37 RID: 15415 RVA: 0x0024C5C5 File Offset: 0x0024A7C5
		public BaiShuiChangShengFa(CombatSkillKey skillKey) : base(skillKey, 10706)
		{
		}

		// Token: 0x06003C38 RID: 15416 RVA: 0x0024C5D5 File Offset: 0x0024A7D5
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_PoisonAffected(new Events.OnPoisonAffected(this.OnPoisonAffected));
		}

		// Token: 0x06003C39 RID: 15417 RVA: 0x0024C5EA File Offset: 0x0024A7EA
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PoisonAffected(new Events.OnPoisonAffected(this.OnPoisonAffected));
		}

		// Token: 0x06003C3A RID: 15418 RVA: 0x0024C600 File Offset: 0x0024A800
		private void OnPoisonAffected(DataContext context, int charId, sbyte poisonType)
		{
			bool flag = !base.CanAffect;
			if (!flag)
			{
				CombatCharacter poisonChar = base.IsDirect ? DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false) : base.CombatChar;
				bool flag2 = charId != poisonChar.GetId();
				if (!flag2)
				{
					byte markCount = poisonChar.GetDefeatMarkCollection().PoisonMarkList[(int)poisonType];
					bool flag3 = markCount <= 0;
					if (!flag3)
					{
						DomainManager.Combat.ChangeDisorderOfQiRandomRecovery(context, base.CombatChar, (int)markCount * -100, false);
						base.CombatChar.ChangeNeiliAllocation(context, 3, (int)(markCount * 3), true, true);
						base.ShowSpecialEffectTips(0);
						base.ShowSpecialEffectTips(1);
						base.ShowEffectTips(context);
					}
				}
			}
		}

		// Token: 0x040011B2 RID: 4530
		private const int ChangeNeiliAllocationPerMark = 3;

		// Token: 0x040011B3 RID: 4531
		private const int ChangeDisorderOfQiPerMark = -100;
	}
}
