using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.EquipmentEffect;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.LegendaryBook.CombatMusic
{
	// Token: 0x02000177 RID: 375
	public class LuanXin : EquipmentEffectBase
	{
		// Token: 0x06002B5D RID: 11101 RVA: 0x0020531E File Offset: 0x0020351E
		public LuanXin()
		{
		}

		// Token: 0x06002B5E RID: 11102 RVA: 0x00205328 File Offset: 0x00203528
		public LuanXin(int charId, ItemKey itemKey) : base(charId, itemKey, 41300, false)
		{
		}

		// Token: 0x06002B5F RID: 11103 RVA: 0x0020533A File Offset: 0x0020353A
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06002B60 RID: 11104 RVA: 0x0020534F File Offset: 0x0020354F
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06002B61 RID: 11105 RVA: 0x00205364 File Offset: 0x00203564
		private unsafe void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = attacker.GetId() != base.CharacterId || !base.IsCurrWeapon() || !hit;
			if (!flag)
			{
				NeiliAllocation neiliAllocation = base.CurrEnemyChar.GetNeiliAllocation();
				List<byte> typeRandomPool = ObjectPool<List<byte>>.Instance.Get();
				typeRandomPool.Clear();
				for (byte type = 0; type < 4; type += 1)
				{
					bool flag2 = *(ref neiliAllocation.Items.FixedElementField + (IntPtr)type * 2) > 0;
					if (flag2)
					{
						typeRandomPool.Add(type);
					}
				}
				bool flag3 = typeRandomPool.Count > 0;
				if (flag3)
				{
					base.CurrEnemyChar.ChangeNeiliAllocation(context, typeRandomPool.GetRandom(context.Random), -1, true, true);
				}
				ObjectPool<List<byte>>.Instance.Return(typeRandomPool);
			}
		}
	}
}
