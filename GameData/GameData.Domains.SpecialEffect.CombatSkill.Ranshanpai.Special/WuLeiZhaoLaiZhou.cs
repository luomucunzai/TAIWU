using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Special;

public class WuLeiZhaoLaiZhou : CombatSkillEffectBase
{
	public WuLeiZhaoLaiZhou()
	{
	}

	public WuLeiZhaoLaiZhou(CombatSkillKey skillKey)
		: base(skillKey, 7303, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		if (PowerMatchAffectRequire(power))
		{
			ItemKey[] equipment = (base.IsDirect ? base.CurrEnemyChar.GetCharacter() : CharObj).GetEquipment();
			sbyte[] array = new sbyte[4] { 1, 3, 4, 5 };
			int num = 0;
			sbyte[] array2 = EquipmentSlot.EquipmentType2Slots[0];
			foreach (sbyte b in array2)
			{
				ItemKey itemKey = equipment[b];
				if (itemKey.IsValid() && ItemTemplateHelper.GetResourceType(itemKey.ItemType, itemKey.TemplateId) == 2)
				{
					num++;
					break;
				}
			}
			sbyte[] array3 = array;
			foreach (sbyte b2 in array3)
			{
				bool flag = false;
				sbyte[] array4 = EquipmentSlot.EquipmentType2Slots[b2];
				foreach (sbyte b3 in array4)
				{
					ItemKey itemKey2 = equipment[b3];
					if (itemKey2.IsValid() && ItemTemplateHelper.GetResourceType(itemKey2.ItemType, itemKey2.TemplateId) == 2)
					{
						num++;
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			sbyte[] array5 = EquipmentSlot.EquipmentType2Slots[6];
			foreach (sbyte b4 in array5)
			{
				ItemKey itemKey3 = equipment[b4];
				if (itemKey3.IsValid() && ItemTemplateHelper.GetResourceType(itemKey3.ItemType, itemKey3.TemplateId) == 2)
				{
					num++;
					break;
				}
			}
			if (num > 0)
			{
				DomainManager.Combat.AddRandomInjury(context, base.CurrEnemyChar, !base.IsDirect, num, 1, changeToOld: false, -1);
				ShowSpecialEffectTips(0);
			}
		}
		RemoveSelf(context);
	}
}
