using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.EquipmentEffect;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Finger
{
	// Token: 0x02000163 RID: 355
	public class DuanXue : EquipmentEffectBase
	{
		// Token: 0x06002B18 RID: 11032 RVA: 0x00204988 File Offset: 0x00202B88
		public DuanXue()
		{
		}

		// Token: 0x06002B19 RID: 11033 RVA: 0x00204992 File Offset: 0x00202B92
		public DuanXue(int charId, ItemKey itemKey) : base(charId, itemKey, 40400, false)
		{
		}

		// Token: 0x06002B1A RID: 11034 RVA: 0x002049A4 File Offset: 0x00202BA4
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06002B1B RID: 11035 RVA: 0x002049B9 File Offset: 0x00202BB9
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06002B1C RID: 11036 RVA: 0x002049D0 File Offset: 0x00202BD0
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = attacker != base.CombatChar || !base.IsCurrWeapon() || pursueIndex != 4 || !context.Random.CheckPercentProb(50);
			if (!flag)
			{
				CombatCharacter enemyChar = base.CurrEnemyChar;
				short skillId = enemyChar.GetRandomBanableSkillId(context.Random, null, -1);
				bool flag2 = skillId >= 0;
				if (flag2)
				{
					DomainManager.Combat.SilenceSkill(context, enemyChar, skillId, 240, 100);
				}
			}
		}

		// Token: 0x04000D33 RID: 3379
		private const int SilenceOdds = 50;

		// Token: 0x04000D34 RID: 3380
		private const short SilenceFrame = 240;
	}
}
