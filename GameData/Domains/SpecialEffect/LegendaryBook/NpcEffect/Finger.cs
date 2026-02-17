using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.LegendaryBook.NpcEffect
{
	// Token: 0x02000146 RID: 326
	public class Finger : FeatureEffectBase
	{
		// Token: 0x06002AA5 RID: 10917 RVA: 0x002031D7 File Offset: 0x002013D7
		public Finger()
		{
		}

		// Token: 0x06002AA6 RID: 10918 RVA: 0x002031E1 File Offset: 0x002013E1
		public Finger(int charId, short featureId) : base(charId, featureId, 41404)
		{
		}

		// Token: 0x06002AA7 RID: 10919 RVA: 0x002031F2 File Offset: 0x002013F2
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06002AA8 RID: 10920 RVA: 0x00203230 File Offset: 0x00201430
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06002AA9 RID: 10921 RVA: 0x00203248 File Offset: 0x00201448
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !DomainManager.Combat.IsCharInCombat(base.CharacterId, true) || attacker != base.CombatChar || pursueIndex != 4;
			if (!flag)
			{
				ItemKey currWeapon = DomainManager.Combat.GetUsingWeaponKey(base.CombatChar);
				bool flag2 = ItemTemplateHelper.GetItemSubType(currWeapon.ItemType, currWeapon.TemplateId) != 4;
				if (!flag2)
				{
					CombatCharacter enemyChar = base.CurrEnemyChar;
					short skillId = enemyChar.GetRandomBanableSkillId(context.Random, null, -1);
					bool flag3 = skillId >= 0;
					if (flag3)
					{
						DomainManager.Combat.SilenceSkill(context, enemyChar, skillId, 1200, 100);
					}
				}
			}
		}

		// Token: 0x06002AAA RID: 10922 RVA: 0x002032EC File Offset: 0x002014EC
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId < 0 || base.CombatChar.GetAutoCastingSkill() || CombatSkill.Instance[dataKey.CombatSkillId].Type != 4;
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
					int acupointCount = base.CurrEnemyChar.GetAcupointCount().Sum();
					int addDamage = 20 * acupointCount;
					result = Math.Min(addDamage, 180);
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000D04 RID: 3332
		private const short AddDamageUnit = 20;

		// Token: 0x04000D05 RID: 3333
		private const short MaxAddDamage = 180;

		// Token: 0x04000D06 RID: 3334
		private const short SilenceFrame = 1200;
	}
}
