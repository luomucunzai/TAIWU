using System;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.LegendaryBook.NpcEffect
{
	// Token: 0x0200014E RID: 334
	public class Sword : FeatureEffectBase
	{
		// Token: 0x06002AD2 RID: 10962 RVA: 0x00203DD0 File Offset: 0x00201FD0
		public Sword()
		{
		}

		// Token: 0x06002AD3 RID: 10963 RVA: 0x00203DDA File Offset: 0x00201FDA
		public Sword(int charId, short featureId) : base(charId, featureId, 41407)
		{
		}

		// Token: 0x06002AD4 RID: 10964 RVA: 0x00203DEB File Offset: 0x00201FEB
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(76, EDataModifyType.AddPercent, -1);
			base.CreateAffectedData(69, EDataModifyType.AddPercent, -1);
		}

		// Token: 0x06002AD5 RID: 10965 RVA: 0x00203E04 File Offset: 0x00202004
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
				bool flag2 = dataKey.FieldId == 76;
				if (flag2)
				{
					GameData.Domains.Item.Weapon currWeapon = DomainManager.Combat.GetUsingWeapon(base.CombatChar);
					bool flag3 = currWeapon.GetItemSubType() != 8;
					if (flag3)
					{
						result = 0;
					}
					else
					{
						result = ((dataKey.CustomParam1 == 1) ? 200 : 0);
					}
				}
				else
				{
					bool flag4 = dataKey.FieldId == 69;
					if (flag4)
					{
						bool flag5;
						if (!dataKey.IsNormalAttack && !base.CombatChar.GetAutoCastingSkill())
						{
							CombatSkillItem skillTemplate = dataKey.SkillTemplate;
							flag5 = (skillTemplate == null || skillTemplate.Type != 7);
						}
						else
						{
							flag5 = true;
						}
						bool flag6 = flag5;
						if (flag6)
						{
							result = 0;
						}
						else
						{
							int trickCount = base.EnemyChar.UsableTrickCount;
							int addDamage = 20 * (9 - trickCount);
							result = Math.Min(addDamage, 180);
						}
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04000D1D RID: 3357
		private const int AddPursueOdds = 200;

		// Token: 0x04000D1E RID: 3358
		private const short AddDamageUnit = 20;

		// Token: 0x04000D1F RID: 3359
		private const short MaxAddDamage = 180;
	}
}
