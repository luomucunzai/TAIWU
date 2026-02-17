using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.EquipmentEffect;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Polearm
{
	// Token: 0x0200013D RID: 317
	public class ChuangZhen : EquipmentEffectBase
	{
		// Token: 0x06002A86 RID: 10886 RVA: 0x00202BE9 File Offset: 0x00200DE9
		public ChuangZhen()
		{
		}

		// Token: 0x06002A87 RID: 10887 RVA: 0x00202BF3 File Offset: 0x00200DF3
		public ChuangZhen(int charId, ItemKey itemKey) : base(charId, itemKey, 40900, false)
		{
		}

		// Token: 0x06002A88 RID: 10888 RVA: 0x00202C05 File Offset: 0x00200E05
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06002A89 RID: 10889 RVA: 0x00202C22 File Offset: 0x00200E22
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			base.OnDisable(context);
		}

		// Token: 0x06002A8A RID: 10890 RVA: 0x00202C40 File Offset: 0x00200E40
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightback)
		{
			bool flag = attacker.GetId() == base.CharacterId && base.IsCurrWeapon() && pursueIndex == 5;
			if (flag)
			{
				defender.ChangeAffectingDefenseSkillLeftFrame(context, ChuangZhen.DeltaFrame);
			}
		}

		// Token: 0x04000CFA RID: 3322
		private const int NeedPursueIndex = 5;

		// Token: 0x04000CFB RID: 3323
		private static readonly CValuePercent DeltaFrame = -50;
	}
}
