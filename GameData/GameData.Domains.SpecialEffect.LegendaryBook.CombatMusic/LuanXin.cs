using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.EquipmentEffect;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.LegendaryBook.CombatMusic;

public class LuanXin : EquipmentEffectBase
{
	public LuanXin()
	{
	}

	public LuanXin(int charId, ItemKey itemKey)
		: base(charId, itemKey, 41300, autoRemoveAfterCombat: false)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Events.RegisterHandler_NormalAttackEnd(OnNormalAttackEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_NormalAttackEnd(OnNormalAttackEnd);
	}

	private unsafe void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
	{
		if (attacker.GetId() != base.CharacterId || !IsCurrWeapon() || !hit)
		{
			return;
		}
		NeiliAllocation neiliAllocation = base.CurrEnemyChar.GetNeiliAllocation();
		List<byte> list = ObjectPool<List<byte>>.Instance.Get();
		list.Clear();
		for (byte b = 0; b < 4; b++)
		{
			if (neiliAllocation.Items[(int)b] > 0)
			{
				list.Add(b);
			}
		}
		if (list.Count > 0)
		{
			base.CurrEnemyChar.ChangeNeiliAllocation(context, list.GetRandom(context.Random), -1);
		}
		ObjectPool<List<byte>>.Instance.Return(list);
	}
}
