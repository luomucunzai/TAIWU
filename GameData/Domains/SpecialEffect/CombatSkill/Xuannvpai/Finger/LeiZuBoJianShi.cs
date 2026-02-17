using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Finger
{
	// Token: 0x0200027B RID: 635
	public class LeiZuBoJianShi : CombatSkillEffectBase
	{
		// Token: 0x060030C3 RID: 12483 RVA: 0x00218756 File Offset: 0x00216956
		private static bool IsDetachable(ItemKey itemKey)
		{
			return itemKey.IsValid() && DomainManager.Item.GetBaseEquipment(itemKey).GetDetachable();
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060030C4 RID: 12484 RVA: 0x00218774 File Offset: 0x00216974
		private IReadOnlyList<ItemKey> CurrEnemyEquipments
		{
			get
			{
				return base.CurrEnemyChar.GetCharacter().GetEquipment();
			}
		}

		// Token: 0x060030C5 RID: 12485 RVA: 0x00218786 File Offset: 0x00216986
		private bool IsGodArmor(int itemId)
		{
			return DomainManager.SpecialEffect.ModifyData(base.CurrEnemyChar.GetId(), -1, 182, false, itemId, -1, -1);
		}

		// Token: 0x060030C6 RID: 12486 RVA: 0x002187A8 File Offset: 0x002169A8
		private bool IsDetachable(sbyte slot)
		{
			ItemKey itemKey = this.CurrEnemyEquipments[(int)slot];
			bool flag = !LeiZuBoJianShi.IsDetachable(itemKey);
			return !flag && (itemKey.ItemType != 1 || !this.IsGodArmor(itemKey.Id));
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x060030C7 RID: 12487 RVA: 0x002187F3 File Offset: 0x002169F3
		private short CombatStateId
		{
			get
			{
				return base.IsDirect ? 39 : 40;
			}
		}

		// Token: 0x060030C8 RID: 12488 RVA: 0x00218803 File Offset: 0x00216A03
		public LeiZuBoJianShi()
		{
		}

		// Token: 0x060030C9 RID: 12489 RVA: 0x0021880D File Offset: 0x00216A0D
		public LeiZuBoJianShi(CombatSkillKey skillKey) : base(skillKey, 8203, -1)
		{
		}

		// Token: 0x060030CA RID: 12490 RVA: 0x0021881E File Offset: 0x00216A1E
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060030CB RID: 12491 RVA: 0x00218833 File Offset: 0x00216A33
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060030CC RID: 12492 RVA: 0x00218848 File Offset: 0x00216A48
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = !this.SkillKey.IsMatch(charId, skillId);
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0) && context.Random.CheckPercentProb(50);
				if (flag2)
				{
					this.DoAffect(context);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x060030CD RID: 12493 RVA: 0x0021889C File Offset: 0x00216A9C
		private void DoAffect(DataContext context)
		{
			sbyte slot = this.RandomTargetSlot(context.Random);
			int itemGrade = this.DoChangeEquipment(context, slot);
			bool flag = itemGrade < 0;
			if (!flag)
			{
				int power = 20 * (itemGrade + 1);
				DomainManager.Combat.AddCombatState(context, base.CurrEnemyChar, 2, this.CombatStateId, power);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060030CE RID: 12494 RVA: 0x002188F4 File Offset: 0x00216AF4
		private sbyte RandomTargetSlot(IRandomSource random)
		{
			ItemKey clothKey = this.CurrEnemyEquipments[4];
			List<sbyte> slotPool = ObjectPool<List<sbyte>>.Instance.Get();
			slotPool.Clear();
			bool flag = LeiZuBoJianShi.IsDetachable(clothKey) && random.CheckPercentProb(50);
			if (flag)
			{
				slotPool.Add(4);
			}
			else
			{
				slotPool.AddRange(LeiZuBoJianShi.CanRemoveSlots.Where(new Func<sbyte, bool>(this.IsDetachable)));
			}
			bool flag2 = slotPool.Count == 0 && LeiZuBoJianShi.IsDetachable(clothKey);
			if (flag2)
			{
				slotPool.Add(4);
			}
			sbyte slot = (slotPool.Count > 0) ? slotPool.GetRandom(random) : -1;
			ObjectPool<List<sbyte>>.Instance.Return(slotPool);
			return slot;
		}

		// Token: 0x060030CF RID: 12495 RVA: 0x002189A8 File Offset: 0x00216BA8
		private int DoChangeEquipment(DataContext context, sbyte slot)
		{
			bool flag = slot < 0;
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				ItemKey itemKey = this.CurrEnemyEquipments[(int)slot];
				bool flag2 = base.CurrEnemyChar.GetRawCreateCollection().Contains(itemKey);
				if (flag2)
				{
					base.CurrEnemyChar.RevertRawCreate(context, itemKey);
					itemKey = this.CurrEnemyEquipments[(int)slot];
				}
				base.CurrEnemyChar.GetCharacter().ChangeEquipment(context, slot, -1, ItemKey.Invalid);
				for (sbyte i = 0; i < 7; i += 1)
				{
					bool flag3 = EquipmentSlotHelper.GetSlotByBodyPartType(i) == slot;
					if (flag3)
					{
						base.CurrEnemyChar.Armors[(int)i] = ItemKey.Invalid;
					}
				}
				result = (int)ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId);
			}
			return result;
		}

		// Token: 0x04000E7A RID: 3706
		private const sbyte AffectOdds = 50;

		// Token: 0x04000E7B RID: 3707
		private const sbyte RemoveClothOdds = 50;

		// Token: 0x04000E7C RID: 3708
		private const sbyte CombatStatePowerUnit = 20;

		// Token: 0x04000E7D RID: 3709
		private static readonly sbyte[] CanRemoveSlots = new sbyte[]
		{
			3,
			5,
			6,
			7,
			8,
			9,
			10
		};
	}
}
