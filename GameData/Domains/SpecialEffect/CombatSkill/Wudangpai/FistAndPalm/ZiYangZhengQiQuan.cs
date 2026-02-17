using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.FistAndPalm
{
	// Token: 0x020003D6 RID: 982
	public class ZiYangZhengQiQuan : CombatSkillEffectBase
	{
		// Token: 0x060037B1 RID: 14257 RVA: 0x00236CC0 File Offset: 0x00234EC0
		public ZiYangZhengQiQuan()
		{
		}

		// Token: 0x060037B2 RID: 14258 RVA: 0x00236CCA File Offset: 0x00234ECA
		public ZiYangZhengQiQuan(CombatSkillKey skillKey) : base(skillKey, 4107, -1)
		{
		}

		// Token: 0x060037B3 RID: 14259 RVA: 0x00236CDC File Offset: 0x00234EDC
		public override void OnEnable(DataContext context)
		{
			this._addPower = (int)(this.CharObj.GetRecoveryOfQiDisorder() * 12 / 100);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 85, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			bool flag = this._addPower > 0;
			if (flag)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
				base.ShowSpecialEffectTips(0);
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060037B4 RID: 14260 RVA: 0x00236D7E File Offset: 0x00234F7E
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060037B5 RID: 14261 RVA: 0x00236D94 File Offset: 0x00234F94
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					DomainManager.Combat.ChangeDisorderOfQiRandomRecovery(context, base.IsDirect ? base.CombatChar : base.CurrEnemyChar, 800 * (base.IsDirect ? -1 : 1), false);
					base.ShowSpecialEffectTips(1);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x060037B6 RID: 14262 RVA: 0x00236E18 File Offset: 0x00235018
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
					result = this._addPower;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x060037B7 RID: 14263 RVA: 0x00236E70 File Offset: 0x00235070
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 85;
				result = (!flag2 && dataValue);
			}
			return result;
		}

		// Token: 0x0400103D RID: 4157
		private const sbyte AddPowerPercent = 12;

		// Token: 0x0400103E RID: 4158
		private const short ChangeQiDisorder = 800;

		// Token: 0x0400103F RID: 4159
		private int _addPower;
	}
}
