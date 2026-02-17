using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.LegendaryBook.NpcEffect
{
	// Token: 0x02000143 RID: 323
	public class Blade : FeatureEffectBase
	{
		// Token: 0x06002A97 RID: 10903 RVA: 0x00202D4D File Offset: 0x00200F4D
		public Blade()
		{
		}

		// Token: 0x06002A98 RID: 10904 RVA: 0x00202D57 File Offset: 0x00200F57
		public Blade(int charId, short featureId) : base(charId, featureId, 41408)
		{
		}

		// Token: 0x06002A99 RID: 10905 RVA: 0x00202D68 File Offset: 0x00200F68
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(69, EDataModifyType.AddPercent, -1);
		}

		// Token: 0x06002A9A RID: 10906 RVA: 0x00202D78 File Offset: 0x00200F78
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 69;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool critical = dataKey.CustomParam2 == 1;
				int addDamage = 0;
				Weapon usingWeapon = DomainManager.Combat.GetUsingWeapon(base.CombatChar);
				bool flag2 = usingWeapon.GetItemSubType() == 9 && critical;
				if (flag2)
				{
					addDamage += 33;
				}
				bool flag3 = !dataKey.IsNormalAttack && !base.CombatChar.GetAutoCastingSkill() && dataKey.SkillTemplate.Type == 8;
				if (flag3)
				{
					addDamage += Math.Min(base.CurrEnemyChar.GetFlawCount().Sum() * 20, 180);
				}
				result = addDamage;
			}
			return result;
		}

		// Token: 0x04000CFC RID: 3324
		private const short AddDamageUnit = 20;

		// Token: 0x04000CFD RID: 3325
		private const short MaxAddDamage = 180;

		// Token: 0x04000CFE RID: 3326
		private const int DirectDamageAddPercent = 33;
	}
}
