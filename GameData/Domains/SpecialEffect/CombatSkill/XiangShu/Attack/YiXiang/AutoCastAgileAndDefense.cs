using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.YiXiang
{
	// Token: 0x020002C6 RID: 710
	public class AutoCastAgileAndDefense : CombatSkillEffectBase
	{
		// Token: 0x06003271 RID: 12913 RVA: 0x0021F785 File Offset: 0x0021D985
		protected AutoCastAgileAndDefense()
		{
		}

		// Token: 0x06003272 RID: 12914 RVA: 0x0021F78F File Offset: 0x0021D98F
		protected AutoCastAgileAndDefense(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06003273 RID: 12915 RVA: 0x0021F79C File Offset: 0x0021D99C
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(199, EDataModifyType.AddPercent, -1);
			this._affectingAgileSkill = (this._affectingDefenseSkill = -1);
			this._agileSkillUid = base.ParseCombatCharacterDataUid(62);
			this._defenseSkillUid = base.ParseCombatCharacterDataUid(63);
			GameDataBridge.AddPostDataModificationHandler(this._agileSkillUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnAgileSkillChanged));
			GameDataBridge.AddPostDataModificationHandler(this._defenseSkillUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnDefenseSkillChanged));
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
		}

		// Token: 0x06003274 RID: 12916 RVA: 0x0021F832 File Offset: 0x0021DA32
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._agileSkillUid, base.DataHandlerKey);
			GameDataBridge.RemovePostDataModificationHandler(this._defenseSkillUid, base.DataHandlerKey);
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
		}

		// Token: 0x06003275 RID: 12917 RVA: 0x0021F86C File Offset: 0x0021DA6C
		private void CastAgileAndDefense(DataContext context)
		{
			this._affectingAgileSkill = this.ParseCanCastOrInvalidSkillId(base.CombatChar.GetAgileSkillList()[0]);
			this._affectingDefenseSkill = this.ParseCanCastOrInvalidSkillId(base.CombatChar.GetDefenceSkillList()[0]);
			bool flag = this._affectingAgileSkill >= 0 || this._affectingDefenseSkill >= 0;
			if (flag)
			{
				base.InvalidateCache(context, 199);
				base.ShowSpecialEffectTips(0);
			}
			bool flag2 = this._affectingAgileSkill > 0;
			if (flag2)
			{
				DomainManager.Combat.CastAgileOrDefenseWithoutPrepare(base.CombatChar, this._affectingAgileSkill);
			}
			bool flag3 = this._affectingDefenseSkill > 0;
			if (flag3)
			{
				DomainManager.Combat.CastAgileOrDefenseWithoutPrepare(base.CombatChar, this._affectingDefenseSkill);
			}
		}

		// Token: 0x06003276 RID: 12918 RVA: 0x0021F92F File Offset: 0x0021DB2F
		private short ParseCanCastOrInvalidSkillId(short skillId)
		{
			return DomainManager.Combat.CanCastSkill(base.CombatChar, skillId, true, false) ? skillId : -1;
		}

		// Token: 0x06003277 RID: 12919 RVA: 0x0021F94C File Offset: 0x0021DB4C
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				this.CastAgileAndDefense(context);
			}
		}

		// Token: 0x06003278 RID: 12920 RVA: 0x0021F984 File Offset: 0x0021DB84
		private void OnAgileSkillChanged(DataContext context, DataUid dataUid)
		{
			bool flag = this._affectingAgileSkill < 0 || this._affectingAgileSkill == base.CombatChar.GetAffectingMoveSkillId();
			if (!flag)
			{
				this._affectingAgileSkill = -1;
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			}
		}

		// Token: 0x06003279 RID: 12921 RVA: 0x0021F9D8 File Offset: 0x0021DBD8
		private void OnDefenseSkillChanged(DataContext context, DataUid dataUid)
		{
			bool flag = this._affectingDefenseSkill < 0 || this._affectingDefenseSkill == base.CombatChar.GetAffectingDefendSkillId();
			if (!flag)
			{
				this._affectingDefenseSkill = -1;
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			}
		}

		// Token: 0x0600327A RID: 12922 RVA: 0x0021FA2C File Offset: 0x0021DC2C
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
				bool flag2 = dataKey.CombatSkillId != this._affectingAgileSkill && dataKey.CombatSkillId != this._affectingDefenseSkill;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 199;
					if (flag3)
					{
						result = (int)this.AddPower;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04000EED RID: 3821
		protected sbyte AddPower;

		// Token: 0x04000EEE RID: 3822
		private short _affectingAgileSkill;

		// Token: 0x04000EEF RID: 3823
		private short _affectingDefenseSkill;

		// Token: 0x04000EF0 RID: 3824
		private DataUid _agileSkillUid;

		// Token: 0x04000EF1 RID: 3825
		private DataUid _defenseSkillUid;
	}
}
