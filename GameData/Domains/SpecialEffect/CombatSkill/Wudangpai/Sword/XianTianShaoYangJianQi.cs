using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Sword
{
	// Token: 0x020003C3 RID: 963
	public class XianTianShaoYangJianQi : CombatSkillEffectBase
	{
		// Token: 0x0600375D RID: 14173 RVA: 0x002353AB File Offset: 0x002335AB
		public XianTianShaoYangJianQi()
		{
		}

		// Token: 0x0600375E RID: 14174 RVA: 0x002353B5 File Offset: 0x002335B5
		public XianTianShaoYangJianQi(CombatSkillKey skillKey) : base(skillKey, 4207, -1)
		{
		}

		// Token: 0x0600375F RID: 14175 RVA: 0x002353C8 File Offset: 0x002335C8
		public override void OnEnable(DataContext context)
		{
			int age = (int)this.CharObj.GetActualAge();
			this._addPowerInThisCombat = (base.IsDirect ? (80 - age) : (age / 2));
			this._addPower = 0;
			bool flag = this._addPowerInThisCombat > 0;
			if (flag)
			{
				base.CreateAffectedData(199, EDataModifyType.AddPercent, base.SkillTemplateId);
			}
			bool flag2 = base.IsDirect ? (age <= 40) : (age >= 40);
			if (flag2)
			{
				base.CreateAffectedData(201, EDataModifyType.Custom, base.SkillTemplateId);
			}
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003760 RID: 14176 RVA: 0x00235475 File Offset: 0x00233675
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003761 RID: 14177 RVA: 0x0023549C File Offset: 0x0023369C
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || this._addPowerInThisCombat <= 0;
			if (!flag)
			{
				this._addPower = this._addPowerInThisCombat;
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003762 RID: 14178 RVA: 0x00235500 File Offset: 0x00233700
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || this._addPowerInThisCombat <= 0;
			if (!flag)
			{
				this._addPower = 0;
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			}
		}

		// Token: 0x06003763 RID: 14179 RVA: 0x00235554 File Offset: 0x00233754
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

		// Token: 0x06003764 RID: 14180 RVA: 0x002355AC File Offset: 0x002337AC
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.SkillKey != this.SkillKey || dataKey.FieldId != 201;
			return flag && base.GetModifiedValue(dataKey, dataValue);
		}

		// Token: 0x0400102A RID: 4138
		private const sbyte PowerNoReduceAge = 40;

		// Token: 0x0400102B RID: 4139
		private int _addPowerInThisCombat;

		// Token: 0x0400102C RID: 4140
		private int _addPower;
	}
}
