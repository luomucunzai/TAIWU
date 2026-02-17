using System;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Polearm
{
	// Token: 0x02000411 RID: 1041
	public class DaMoZhangFa : CombatSkillEffectBase
	{
		// Token: 0x06003912 RID: 14610 RVA: 0x0023D01A File Offset: 0x0023B21A
		public DaMoZhangFa()
		{
		}

		// Token: 0x06003913 RID: 14611 RVA: 0x0023D024 File Offset: 0x0023B224
		public DaMoZhangFa(CombatSkillKey skillKey) : base(skillKey, 1308, -1)
		{
		}

		// Token: 0x06003914 RID: 14612 RVA: 0x0023D038 File Offset: 0x0023B238
		public override void OnEnable(DataContext context)
		{
			this._addPower = 0;
			this._addPowerSkillId = -1;
			base.CreateAffectedData(199, EDataModifyType.AddPercent, -1);
			base.CreateAffectedData(154, EDataModifyType.Custom, -1);
			Events.RegisterHandler_CastAgileOrDefenseWithoutPrepareBegin(new Events.OnCastAgileOrDefenseWithoutPrepareBegin(this.OnCastAgileOrDefenseWithoutPrepareBegin));
			Events.RegisterHandler_CastAgileOrDefenseWithoutPrepareEnd(new Events.OnCastAgileOrDefenseWithoutPrepareEnd(this.OnCastAgileOrDefenseWithoutPrepareEnd));
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			this._affectingDefendUid = base.ParseCombatCharacterDataUid(63);
			GameDataBridge.AddPostDataModificationHandler(this._affectingDefendUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.UpdatePower));
		}

		// Token: 0x06003915 RID: 14613 RVA: 0x0023D0E4 File Offset: 0x0023B2E4
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastAgileOrDefenseWithoutPrepareBegin(new Events.OnCastAgileOrDefenseWithoutPrepareBegin(this.OnCastAgileOrDefenseWithoutPrepareBegin));
			Events.UnRegisterHandler_CastAgileOrDefenseWithoutPrepareEnd(new Events.OnCastAgileOrDefenseWithoutPrepareEnd(this.OnCastAgileOrDefenseWithoutPrepareEnd));
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			GameDataBridge.RemovePostDataModificationHandler(this._affectingDefendUid, base.DataHandlerKey);
		}

		// Token: 0x06003916 RID: 14614 RVA: 0x0023D14C File Offset: 0x0023B34C
		private void OnCastAgileOrDefenseWithoutPrepareBegin(DataContext context, int charId, short skillId)
		{
			bool flag = charId != base.CharacterId || base.CombatChar.GetPreparingSkillId() != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = Config.CombatSkill.Instance[skillId].EquipType != 3;
				if (!flag2)
				{
					this._addPowerSkillId = (base.IsDirect ? skillId : base.SkillTemplateId);
					this._addPower = (int)((base.IsDirect ? base.SkillInstance : DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(base.CharacterId, skillId))).GetPower() * 20 / 100);
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x06003917 RID: 14615 RVA: 0x0023D210 File Offset: 0x0023B410
		private void OnCastAgileOrDefenseWithoutPrepareEnd(DataContext context, int charId, short skillId)
		{
			bool flag = charId != base.CharacterId || base.CombatChar.GetPreparingSkillId() != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = Config.CombatSkill.Instance[skillId].EquipType != 3;
				if (!flag2)
				{
					this.UpdateCanCastSkills(context);
				}
			}
		}

		// Token: 0x06003918 RID: 14616 RVA: 0x0023D26C File Offset: 0x0023B46C
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				this.UpdateCanCastSkills(context);
			}
		}

		// Token: 0x06003919 RID: 14617 RVA: 0x0023D2A4 File Offset: 0x0023B4A4
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = this.SkillKey.IsMatch(charId, skillId);
			if (flag)
			{
				this.UpdateCanCastSkills(context);
			}
			bool flag2 = charId != base.CharacterId || this._addPowerSkillId != base.SkillTemplateId;
			if (!flag2)
			{
				this._addPower = 0;
				this._addPowerSkillId = -1;
				base.InvalidateCache(context, 199);
			}
		}

		// Token: 0x0600391A RID: 14618 RVA: 0x0023D30C File Offset: 0x0023B50C
		private void UpdatePower(DataContext context, DataUid _)
		{
			bool flag = this._addPowerSkillId < 0 || base.CombatChar.GetAffectingDefendSkillId() == this._addPowerSkillId || base.SkillTemplateId == this._addPowerSkillId;
			if (!flag)
			{
				this._addPower = 0;
				this._addPowerSkillId = -1;
				base.InvalidateCache(context, 199);
			}
		}

		// Token: 0x0600391B RID: 14619 RVA: 0x0023D368 File Offset: 0x0023B568
		private void UpdateCanCastSkills(DataContext context)
		{
			base.CombatChar.CanCastDuringPrepareSkills.Clear();
			bool flag = base.CombatChar.GetPreparingSkillId() == base.SkillTemplateId && base.CombatChar.GetAffectingDefendSkillId() < 0;
			if (flag)
			{
				base.CombatChar.CanCastDuringPrepareSkills.AddRange(base.CombatChar.GetDefenceSkillList());
				base.CombatChar.CanCastDuringPrepareSkills.RemoveAll((short id) => id < 0);
			}
			DomainManager.Combat.UpdateSkillCanUse(context, base.CombatChar);
			bool flag2 = base.CombatChar.CanCastDuringPrepareSkills.Count > 0;
			if (flag2)
			{
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x0600391C RID: 14620 RVA: 0x0023D430 File Offset: 0x0023B630
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != this._addPowerSkillId || dataKey.FieldId != 199;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = this._addPower;
			}
			return result;
		}

		// Token: 0x0600391D RID: 14621 RVA: 0x0023D480 File Offset: 0x0023B680
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || base.CombatChar.GetPreparingSkillId() != base.SkillTemplateId || Config.CombatSkill.Instance[dataKey.CombatSkillId].EquipType != 3;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 154;
				result = (!flag2 && dataValue);
			}
			return result;
		}

		// Token: 0x040010B2 RID: 4274
		private const sbyte AddPowerRatio = 20;

		// Token: 0x040010B3 RID: 4275
		private int _addPower;

		// Token: 0x040010B4 RID: 4276
		private short _addPowerSkillId;

		// Token: 0x040010B5 RID: 4277
		private DataUid _affectingDefendUid;
	}
}
