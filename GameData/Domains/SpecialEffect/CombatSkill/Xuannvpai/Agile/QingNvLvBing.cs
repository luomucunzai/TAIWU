using System;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Agile
{
	// Token: 0x0200028C RID: 652
	public class QingNvLvBing : BuffHitOrDebuffAvoid
	{
		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06003123 RID: 12579 RVA: 0x00219D46 File Offset: 0x00217F46
		protected override sbyte AffectHitType
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06003124 RID: 12580 RVA: 0x00219D49 File Offset: 0x00217F49
		public QingNvLvBing()
		{
		}

		// Token: 0x06003125 RID: 12581 RVA: 0x00219D53 File Offset: 0x00217F53
		public QingNvLvBing(CombatSkillKey skillKey) : base(skillKey, 8406)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x06003126 RID: 12582 RVA: 0x00219D6A File Offset: 0x00217F6A
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003127 RID: 12583 RVA: 0x00219D87 File Offset: 0x00217F87
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			base.OnDisable(context);
		}

		// Token: 0x06003128 RID: 12584 RVA: 0x00219DA4 File Offset: 0x00217FA4
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = interrupted || (base.IsDirect ? (isAlly != base.CombatChar.IsAlly) : (isAlly == base.CombatChar.IsAlly)) || !base.CanAffect;
			if (!flag)
			{
				CombatCharacter affectCombatChar;
				bool flag2 = !DomainManager.Combat.TryGetElement_CombatCharacterDict(charId, out affectCombatChar) || affectCombatChar.GetAutoCastingSkill();
				if (!flag2)
				{
					bool flag3 = Config.CombatSkill.Instance[skillId].EquipType != 1;
					if (!flag3)
					{
						GameData.Domains.CombatSkill.CombatSkill combatSkill;
						bool flag4 = !DomainManager.CombatSkill.TryGetElement_CombatSkills(new CombatSkillKey(charId, skillId), out combatSkill);
						if (!flag4)
						{
							int hitDistribution = combatSkill.GetHitDistribution()[(int)this.AffectHitType];
							int effectRate = hitDistribution / 2;
							bool flag5 = effectRate <= 0 || !context.Random.CheckPercentProb(effectRate);
							if (!flag5)
							{
								bool flag6 = base.IsDirect && DomainManager.Combat.CanCastSkill(base.CombatChar, skillId, true, false);
								if (flag6)
								{
									DomainManager.Combat.CastSkillFree(context, affectCombatChar, skillId, ECombatCastFreePriority.QingNvLvBing);
								}
								else
								{
									bool flag7 = !base.IsDirect;
									if (flag7)
									{
										DomainManager.Combat.SilenceSkill(context, affectCombatChar, skillId, 2400, 100);
									}
								}
								base.ShowSpecialEffectTips(1);
							}
						}
					}
				}
			}
		}

		// Token: 0x04000E93 RID: 3731
		private const int ReverseSilenceFrame = 2400;
	}
}
