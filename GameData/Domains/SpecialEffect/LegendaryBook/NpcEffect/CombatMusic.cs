using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.LegendaryBook.NpcEffect
{
	// Token: 0x02000144 RID: 324
	public class CombatMusic : FeatureEffectBase
	{
		// Token: 0x06002A9B RID: 10907 RVA: 0x00202E39 File Offset: 0x00201039
		public CombatMusic()
		{
		}

		// Token: 0x06002A9C RID: 10908 RVA: 0x00202E43 File Offset: 0x00201043
		public CombatMusic(int charId, short featureId) : base(charId, featureId, 41413)
		{
		}

		// Token: 0x06002A9D RID: 10909 RVA: 0x00202E54 File Offset: 0x00201054
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06002A9E RID: 10910 RVA: 0x00202E92 File Offset: 0x00201092
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06002A9F RID: 10911 RVA: 0x00202EA8 File Offset: 0x002010A8
		private unsafe void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !DomainManager.Combat.IsCharInCombat(base.CharacterId, true) || attacker.GetId() != base.CharacterId || !hit || isFightBack;
			if (!flag)
			{
				ItemKey currWeapon = DomainManager.Combat.GetUsingWeaponKey(base.CombatChar);
				short subType = ItemTemplateHelper.GetItemSubType(currWeapon.ItemType, currWeapon.TemplateId);
				bool flag2 = !CombatSkillType.Instance[13].LegendaryBookWeaponSlotItemSubTypes.Contains(subType);
				if (!flag2)
				{
					NeiliAllocation neiliAllocation = base.CurrEnemyChar.GetNeiliAllocation();
					List<byte> typeRandomPool = ObjectPool<List<byte>>.Instance.Get();
					typeRandomPool.Clear();
					for (byte type = 0; type < 4; type += 1)
					{
						bool flag3 = *(ref neiliAllocation.Items.FixedElementField + (IntPtr)type * 2) > 0;
						if (flag3)
						{
							typeRandomPool.Add(type);
						}
					}
					bool flag4 = typeRandomPool.Count > 0;
					if (flag4)
					{
						base.CurrEnemyChar.ChangeNeiliAllocation(context, typeRandomPool.GetRandom(context.Random), -1, true, true);
					}
					ObjectPool<List<byte>>.Instance.Return(typeRandomPool);
				}
			}
		}

		// Token: 0x06002AA0 RID: 10912 RVA: 0x00202FC8 File Offset: 0x002011C8
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId < 0 || base.CombatChar.GetAutoCastingSkill() || CombatSkill.Instance[dataKey.CombatSkillId].Type != 13;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 69;
				if (flag2)
				{
					int addDamage = (int)(30 * base.CombatChar.GetTrickCount(9));
					result = Math.Min(addDamage, 180);
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000CFF RID: 3327
		private const short AddDamageUnit = 30;

		// Token: 0x04000D00 RID: 3328
		private const short MaxAddDamage = 180;
	}
}
