using System;
using System.Runtime.CompilerServices;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Sword
{
	// Token: 0x02000442 RID: 1090
	public class KaiHeJianShu : CombatSkillEffectBase
	{
		// Token: 0x06003A1A RID: 14874 RVA: 0x002420B6 File Offset: 0x002402B6
		public KaiHeJianShu()
		{
		}

		// Token: 0x06003A1B RID: 14875 RVA: 0x002420C0 File Offset: 0x002402C0
		public KaiHeJianShu(CombatSkillKey skillKey) : base(skillKey, 7203, -1)
		{
		}

		// Token: 0x06003A1C RID: 14876 RVA: 0x002420D1 File Offset: 0x002402D1
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003A1D RID: 14877 RVA: 0x002420E6 File Offset: 0x002402E6
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003A1E RID: 14878 RVA: 0x002420FC File Offset: 0x002402FC
		private unsafe void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = power > 0;
				if (flag2)
				{
					short[] array;
					if (!base.IsDirect)
					{
						RuntimeHelpers.InitializeArray(array = new short[4], fieldof(<PrivateImplementationDetails>.74BBD4C99CDECAE50A93AAA3E9B2C1CE8E25D882ED188367C400B3DFB03B8F99).FieldHandle);
					}
					else
					{
						RuntimeHelpers.InitializeArray(array = new short[4], fieldof(<PrivateImplementationDetails>.E2E87A79BB01327DD83C5D29498B36D4913C8615FEF39475120EC4AF88E9EAEB).FieldHandle);
					}
					short[] stateIdList = array;
					HitOrAvoidInts selfValue = base.IsDirect ? this.CharObj.GetAvoidValues() : this.CharObj.GetHitValues();
					HitOrAvoidInts enemyValue = base.IsDirect ? base.CurrEnemyChar.GetCharacter().GetAvoidValues() : base.CurrEnemyChar.GetCharacter().GetHitValues();
					bool affected = false;
					for (sbyte type = 0; type < 4; type += 1)
					{
						bool flag3 = *(ref selfValue.Items.FixedElementField + (IntPtr)type * 4) >= *(ref enemyValue.Items.FixedElementField + (IntPtr)type * 4);
						if (flag3)
						{
							affected = true;
							DomainManager.Combat.AddCombatState(context, base.CurrEnemyChar, 2, stateIdList[(int)type], (int)(25 * power / 10));
						}
					}
					bool flag4 = affected;
					if (flag4)
					{
						base.ShowSpecialEffectTips(0);
					}
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04001103 RID: 4355
		private const sbyte StatePowerUnit = 25;
	}
}
