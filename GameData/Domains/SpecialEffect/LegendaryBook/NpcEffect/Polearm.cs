using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.LegendaryBook.NpcEffect
{
	// Token: 0x0200014A RID: 330
	public class Polearm : FeatureEffectBase
	{
		// Token: 0x06002ABC RID: 10940 RVA: 0x002037F3 File Offset: 0x002019F3
		public Polearm()
		{
		}

		// Token: 0x06002ABD RID: 10941 RVA: 0x002037FD File Offset: 0x002019FD
		public Polearm(int charId, short featureId) : base(charId, featureId, 41409)
		{
		}

		// Token: 0x06002ABE RID: 10942 RVA: 0x0020380E File Offset: 0x00201A0E
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06002ABF RID: 10943 RVA: 0x0020384C File Offset: 0x00201A4C
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06002AC0 RID: 10944 RVA: 0x00203864 File Offset: 0x00201A64
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightback)
		{
			bool flag = attacker.GetId() != base.CharacterId || pursueIndex != 5;
			if (!flag)
			{
				GameData.Domains.Item.Weapon usingWeapon = DomainManager.Combat.GetUsingWeapon(attacker);
				bool flag2 = usingWeapon.GetItemSubType() == 10;
				if (flag2)
				{
					defender.ChangeAffectingDefenseSkillLeftFrame(context, Polearm.DeltaFrame);
				}
			}
		}

		// Token: 0x06002AC1 RID: 10945 RVA: 0x002038B8 File Offset: 0x00201AB8
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
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
					bool flag3 = base.CombatChar.GetAutoCastingSkill() || dataKey.CombatSkillId < 0 || CombatSkill.Instance[dataKey.CombatSkillId].Type != 9;
					if (flag3)
					{
						result = 0;
					}
					else
					{
						int markCount = base.CombatChar.GetDefeatMarkCollection().GetTotalCount();
						int maxDamageMarkCount = (int)(GlobalConfig.NeedDefeatMarkCount[(int)DomainManager.Combat.GetCombatType()] / 2);
						int addDamage = 180 * markCount / maxDamageMarkCount;
						result = Math.Min(addDamage, 180);
					}
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000D13 RID: 3347
		private const int NeedPursueIndex = 5;

		// Token: 0x04000D14 RID: 3348
		private static readonly CValuePercent DeltaFrame = -50;

		// Token: 0x04000D15 RID: 3349
		private const short MaxAddDamage = 180;
	}
}
