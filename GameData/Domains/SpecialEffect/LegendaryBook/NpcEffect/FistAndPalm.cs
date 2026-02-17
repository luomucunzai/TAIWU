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
	// Token: 0x02000147 RID: 327
	public class FistAndPalm : FeatureEffectBase
	{
		// Token: 0x06002AAB RID: 10923 RVA: 0x0020337E File Offset: 0x0020157E
		public FistAndPalm()
		{
		}

		// Token: 0x06002AAC RID: 10924 RVA: 0x00203388 File Offset: 0x00201588
		public FistAndPalm(int charId, short featureId) : base(charId, featureId, 41403)
		{
		}

		// Token: 0x06002AAD RID: 10925 RVA: 0x00203399 File Offset: 0x00201599
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06002AAE RID: 10926 RVA: 0x002033D7 File Offset: 0x002015D7
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06002AAF RID: 10927 RVA: 0x002033EC File Offset: 0x002015EC
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = attacker.GetId() != base.CharacterId || pursueIndex != 5;
			if (!flag)
			{
				GameData.Domains.Item.Weapon usingWeapon = DomainManager.Combat.GetUsingWeapon(attacker);
				bool flag2 = usingWeapon.GetItemSubType() == 4;
				if (flag2)
				{
					defender.ChangeToEmptyHandOrOther(context);
				}
			}
		}

		// Token: 0x06002AB0 RID: 10928 RVA: 0x0020343C File Offset: 0x0020163C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId < 0 || base.CombatChar.GetAutoCastingSkill() || CombatSkill.Instance[dataKey.CombatSkillId].Type != 3;
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
					short distance = DomainManager.Combat.GetCurrentDistance();
					int addDamage = (int)((distance > 50) ? 0 : (60 + (50 - distance) / 10 * 40));
					result = Math.Min(addDamage, 180);
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000D07 RID: 3335
		private const int NeedPursueIndex = 5;

		// Token: 0x04000D08 RID: 3336
		private const short MaxDistance = 50;

		// Token: 0x04000D09 RID: 3337
		private const short BaseAddDamage = 60;

		// Token: 0x04000D0A RID: 3338
		private const short AddDamageUnit = 40;

		// Token: 0x04000D0B RID: 3339
		private const short MaxAddDamage = 180;
	}
}
