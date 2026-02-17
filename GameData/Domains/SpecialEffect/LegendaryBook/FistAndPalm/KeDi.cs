using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.EquipmentEffect;

namespace GameData.Domains.SpecialEffect.LegendaryBook.FistAndPalm
{
	// Token: 0x02000160 RID: 352
	public class KeDi : EquipmentEffectBase
	{
		// Token: 0x06002B0F RID: 11023 RVA: 0x002048C4 File Offset: 0x00202AC4
		public KeDi()
		{
		}

		// Token: 0x06002B10 RID: 11024 RVA: 0x002048CE File Offset: 0x00202ACE
		public KeDi(int charId, ItemKey itemKey) : base(charId, itemKey, 40300, false)
		{
		}

		// Token: 0x06002B11 RID: 11025 RVA: 0x002048E0 File Offset: 0x00202AE0
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06002B12 RID: 11026 RVA: 0x002048FD File Offset: 0x00202AFD
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			base.OnDisable(context);
		}

		// Token: 0x06002B13 RID: 11027 RVA: 0x0020491C File Offset: 0x00202B1C
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightback)
		{
			bool flag = attacker.GetId() == base.CharacterId && base.IsCurrWeapon() && pursueIndex == 5;
			if (flag)
			{
				defender.ChangeToEmptyHandOrOther(context);
			}
		}

		// Token: 0x04000D32 RID: 3378
		private const int NeedPursueIndex = 5;
	}
}
