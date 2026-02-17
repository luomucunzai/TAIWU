using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.DefenseAndAssist
{
	// Token: 0x02000284 RID: 644
	public class NvWaBuTianShi : DefenseSkillBase
	{
		// Token: 0x060030FE RID: 12542 RVA: 0x0021954A File Offset: 0x0021774A
		public NvWaBuTianShi()
		{
		}

		// Token: 0x060030FF RID: 12543 RVA: 0x00219554 File Offset: 0x00217754
		public NvWaBuTianShi(CombatSkillKey skillKey) : base(skillKey, 8506)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x06003100 RID: 12544 RVA: 0x0021956B File Offset: 0x0021776B
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(282, EDataModifyType.Custom, -1);
			this._originInjuries = base.CombatChar.GetInjuries();
		}

		// Token: 0x06003101 RID: 12545 RVA: 0x00219598 File Offset: 0x00217798
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			bool flag = base.CombatChar.GetDefeatMarkCollection().GetTotalCount() >= (int)GlobalConfig.NeedDefeatMarkCount[(int)DomainManager.Combat.GetCombatType()];
			if (flag)
			{
				Injuries newInjuries = base.CombatChar.GetInjuries().Subtract(this._originInjuries);
				bool anyHealed = false;
				for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
				{
					sbyte injuryValue = newInjuries.Get(bodyPart, !base.IsDirect);
					bool flag2 = injuryValue > 0;
					if (flag2)
					{
						DomainManager.Combat.RemoveInjury(context, base.CombatChar, bodyPart, !base.IsDirect, injuryValue, false, false);
						anyHealed = true;
					}
				}
				bool flag3 = anyHealed;
				if (flag3)
				{
					DomainManager.Combat.UpdateBodyDefeatMark(context, base.CombatChar);
					base.ShowSpecialEffectTips(0);
				}
			}
			DomainManager.Combat.AddToCheckFallenSet(base.CombatChar.GetId());
		}

		// Token: 0x06003102 RID: 12546 RVA: 0x0021968E File Offset: 0x0021788E
		protected override void OnDefendSkillCanAffectChanged(DataContext context, DataUid dataUid)
		{
			DomainManager.Combat.AddToCheckFallenSet(base.CombatChar.GetId());
		}

		// Token: 0x06003103 RID: 12547 RVA: 0x002196A8 File Offset: 0x002178A8
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 282;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				result = (dataValue || base.CanAffect);
			}
			return result;
		}

		// Token: 0x04000E87 RID: 3719
		private Injuries _originInjuries;
	}
}
