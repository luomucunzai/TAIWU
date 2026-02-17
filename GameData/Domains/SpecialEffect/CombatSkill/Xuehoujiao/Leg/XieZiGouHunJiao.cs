using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Leg
{
	// Token: 0x0200022D RID: 557
	public class XieZiGouHunJiao : CombatSkillEffectBase
	{
		// Token: 0x06002F6E RID: 12142 RVA: 0x002130E9 File Offset: 0x002112E9
		public XieZiGouHunJiao()
		{
		}

		// Token: 0x06002F6F RID: 12143 RVA: 0x002130F3 File Offset: 0x002112F3
		public XieZiGouHunJiao(CombatSkillKey skillKey) : base(skillKey, 15303, -1)
		{
		}

		// Token: 0x06002F70 RID: 12144 RVA: 0x00213104 File Offset: 0x00211304
		public override void OnEnable(DataContext context)
		{
			Character enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false).GetCharacter();
			bool flag = base.IsDirect ? (this.CharObj.GetMoveSpeed() > enemyChar.GetMoveSpeed()) : (this.CharObj.GetAttackSpeed() > enemyChar.GetAttackSpeed());
			if (flag)
			{
				this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
				base.ShowSpecialEffectTips(0);
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002F71 RID: 12145 RVA: 0x002131B2 File Offset: 0x002113B2
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002F72 RID: 12146 RVA: 0x002131C8 File Offset: 0x002113C8
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = power > 0;
				if (flag2)
				{
					DomainManager.Combat.AddCombatState(context, base.CombatChar, 1, base.IsDirect ? 73 : 74, (int)(20 * power / 10));
					base.ShowSpecialEffectTips(1);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06002F73 RID: 12147 RVA: 0x0021323C File Offset: 0x0021143C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 199;
				if (flag2)
				{
					result = 20;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000E13 RID: 3603
		private const sbyte AddPower = 20;

		// Token: 0x04000E14 RID: 3604
		private const sbyte StatePowerUnit = 20;
	}
}
