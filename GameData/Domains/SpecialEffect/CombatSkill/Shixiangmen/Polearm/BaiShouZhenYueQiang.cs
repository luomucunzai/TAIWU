using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Polearm
{
	// Token: 0x020003E8 RID: 1000
	public class BaiShouZhenYueQiang : CombatSkillEffectBase
	{
		// Token: 0x06003829 RID: 14377 RVA: 0x002393B0 File Offset: 0x002375B0
		public BaiShouZhenYueQiang()
		{
		}

		// Token: 0x0600382A RID: 14378 RVA: 0x002393BA File Offset: 0x002375BA
		public BaiShouZhenYueQiang(CombatSkillKey skillKey) : base(skillKey, 6305, -1)
		{
		}

		// Token: 0x0600382B RID: 14379 RVA: 0x002393CC File Offset: 0x002375CC
		public override void OnEnable(DataContext context)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			bool flag = (base.IsDirect ? enemyChar.GetAffectingDefendSkillId() : enemyChar.GetAffectingMoveSkillId()) < 0;
			if (flag)
			{
				bool flag2 = enemyChar.ChangeToEmptyHandOrOther(context);
				if (flag2)
				{
					ItemKey[] weapons = enemyChar.GetWeapons();
					for (int i = 0; i < 7; i++)
					{
						bool flag3 = i != enemyChar.GetUsingWeaponIndex() && weapons[i].IsValid();
						if (flag3)
						{
							DomainManager.Combat.SilenceWeapon(context, enemyChar, i, 300);
						}
					}
					base.ShowSpecialEffectTips(0);
				}
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600382C RID: 14380 RVA: 0x0023948B File Offset: 0x0023768B
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600382D RID: 14381 RVA: 0x002394A0 File Offset: 0x002376A0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0400106F RID: 4207
		private const short WeaponSilenceFrame = 300;
	}
}
