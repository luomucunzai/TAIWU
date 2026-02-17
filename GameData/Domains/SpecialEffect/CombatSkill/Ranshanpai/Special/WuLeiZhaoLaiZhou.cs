using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Special
{
	// Token: 0x02000451 RID: 1105
	public class WuLeiZhaoLaiZhou : CombatSkillEffectBase
	{
		// Token: 0x06003A8B RID: 14987 RVA: 0x00243F5C File Offset: 0x0024215C
		public WuLeiZhaoLaiZhou()
		{
		}

		// Token: 0x06003A8C RID: 14988 RVA: 0x00243F66 File Offset: 0x00242166
		public WuLeiZhaoLaiZhou(CombatSkillKey skillKey) : base(skillKey, 7303, -1)
		{
		}

		// Token: 0x06003A8D RID: 14989 RVA: 0x00243F77 File Offset: 0x00242177
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003A8E RID: 14990 RVA: 0x00243F8C File Offset: 0x0024218C
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003A8F RID: 14991 RVA: 0x00243FA4 File Offset: 0x002421A4
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					ItemKey[] equipments = (base.IsDirect ? base.CurrEnemyChar.GetCharacter() : this.CharObj).GetEquipment();
					sbyte[] armorTypes = new sbyte[]
					{
						1,
						3,
						4,
						5
					};
					int injuryCount = 0;
					foreach (sbyte slotType in EquipmentSlot.EquipmentType2Slots[0])
					{
						ItemKey equipkey = equipments[(int)slotType];
						bool flag3 = equipkey.IsValid() && ItemTemplateHelper.GetResourceType(equipkey.ItemType, equipkey.TemplateId) == 2;
						if (flag3)
						{
							injuryCount++;
							break;
						}
					}
					foreach (sbyte armorType in armorTypes)
					{
						bool added = false;
						foreach (sbyte slotType2 in EquipmentSlot.EquipmentType2Slots[(int)armorType])
						{
							ItemKey equipkey2 = equipments[(int)slotType2];
							bool flag4 = equipkey2.IsValid() && ItemTemplateHelper.GetResourceType(equipkey2.ItemType, equipkey2.TemplateId) == 2;
							if (flag4)
							{
								injuryCount++;
								added = true;
								break;
							}
						}
						bool flag5 = added;
						if (flag5)
						{
							break;
						}
					}
					foreach (sbyte slotType3 in EquipmentSlot.EquipmentType2Slots[6])
					{
						ItemKey equipkey3 = equipments[(int)slotType3];
						bool flag6 = equipkey3.IsValid() && ItemTemplateHelper.GetResourceType(equipkey3.ItemType, equipkey3.TemplateId) == 2;
						if (flag6)
						{
							injuryCount++;
							break;
						}
					}
					bool flag7 = injuryCount > 0;
					if (flag7)
					{
						DomainManager.Combat.AddRandomInjury(context, base.CurrEnemyChar, !base.IsDirect, injuryCount, 1, false, -1);
						base.ShowSpecialEffectTips(0);
					}
				}
				base.RemoveSelf(context);
			}
		}
	}
}
