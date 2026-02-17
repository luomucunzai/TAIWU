using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Carrier
{
	// Token: 0x02000616 RID: 1558
	public class Pulao : CarrierEffectBase
	{
		// Token: 0x06004580 RID: 17792 RVA: 0x00272905 File Offset: 0x00270B05
		public Pulao(int charId) : base(charId)
		{
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06004581 RID: 17793 RVA: 0x00272910 File Offset: 0x00270B10
		protected override short CombatStateId
		{
			get
			{
				return 202;
			}
		}

		// Token: 0x06004582 RID: 17794 RVA: 0x00272917 File Offset: 0x00270B17
		protected override void OnEnableSubClass(DataContext context)
		{
			base.CreateAffectedData(102, EDataModifyType.TotalPercent, -1);
		}

		// Token: 0x06004583 RID: 17795 RVA: 0x00272928 File Offset: 0x00270B28
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 102;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				sbyte innerRatio = dataKey.IsNormalAttack ? DomainManager.Combat.GetUsingWeaponData(enemyChar).GetInnerRatio() : DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(enemyChar.GetId(), dataKey.CombatSkillId)).GetCurrInnerRatio();
				int outerRatio = (int)(100 - innerRatio);
				result = -Math.Min(outerRatio, (int)innerRatio) / 2;
			}
			return result;
		}
	}
}
