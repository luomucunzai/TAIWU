using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Defense
{
	// Token: 0x020002B4 RID: 692
	public class XuanYuJueShen : DefenseSkillBase
	{
		// Token: 0x06003214 RID: 12820 RVA: 0x0021DB8D File Offset: 0x0021BD8D
		public XuanYuJueShen()
		{
		}

		// Token: 0x06003215 RID: 12821 RVA: 0x0021DB97 File Offset: 0x0021BD97
		public XuanYuJueShen(CombatSkillKey skillKey) : base(skillKey, 16309)
		{
		}

		// Token: 0x06003216 RID: 12822 RVA: 0x0021DBA7 File Offset: 0x0021BDA7
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._frameCounter = 0;
			Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x06003217 RID: 12823 RVA: 0x0021DBD3 File Offset: 0x0021BDD3
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
		}

		// Token: 0x06003218 RID: 12824 RVA: 0x0021DBF0 File Offset: 0x0021BDF0
		private void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			bool flag = base.CombatChar != combatChar || DomainManager.Combat.Pause;
			if (!flag)
			{
				this._frameCounter++;
				bool flag2 = this._frameCounter < 60 || !base.CanAffect;
				if (!flag2)
				{
					this._frameCounter = 0;
					CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
					sbyte behaviorType = enemyChar.GetCharacter().GetBehaviorType();
					bool flag3 = behaviorType == 0;
					if (flag3)
					{
						DomainManager.Combat.AddRandomInjury(context, enemyChar, false, 1, 1, false, -1);
					}
					else
					{
						bool flag4 = behaviorType == 1;
						if (flag4)
						{
							DomainManager.Combat.AddRandomInjury(context, enemyChar, true, 1, 1, false, -1);
						}
						else
						{
							bool flag5 = behaviorType == 2;
							if (flag5)
							{
								for (sbyte type = 0; type < 6; type += 1)
								{
									DomainManager.Combat.AddPoison(context, base.CombatChar, enemyChar, type, 3, 100, -1, true, true, default(ItemKey), false, false, false);
								}
							}
							else
							{
								bool flag6 = behaviorType == 3;
								if (flag6)
								{
									DomainManager.Combat.AddAcupoint(context, enemyChar, 2, new CombatSkillKey(-1, -1), -1, 1, true);
								}
								else
								{
									bool flag7 = behaviorType == 4;
									if (flag7)
									{
										DomainManager.Combat.AddFlaw(context, enemyChar, 2, new CombatSkillKey(-1, -1), -1, 1, true);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x04000ED3 RID: 3795
		private const sbyte AffectFrame = 60;

		// Token: 0x04000ED4 RID: 3796
		private const sbyte AddPoison = 100;

		// Token: 0x04000ED5 RID: 3797
		private int _frameCounter;
	}
}
