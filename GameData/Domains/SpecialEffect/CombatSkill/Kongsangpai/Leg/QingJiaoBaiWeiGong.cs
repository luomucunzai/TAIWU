using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Leg
{
	// Token: 0x02000489 RID: 1161
	public class QingJiaoBaiWeiGong : CombatSkillEffectBase
	{
		// Token: 0x06003BDA RID: 15322 RVA: 0x0024A287 File Offset: 0x00248487
		public QingJiaoBaiWeiGong()
		{
		}

		// Token: 0x06003BDB RID: 15323 RVA: 0x0024A291 File Offset: 0x00248491
		public QingJiaoBaiWeiGong(CombatSkillKey skillKey) : base(skillKey, 10304, -1)
		{
		}

		// Token: 0x06003BDC RID: 15324 RVA: 0x0024A2A4 File Offset: 0x002484A4
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(69, EDataModifyType.AddPercent, -1);
			this._affecting = false;
			this._addDamagePercent = 0;
			base.AddMaxEffectCount(false);
			this._defeatMarkUid = base.ParseCombatCharacterDataUid(50);
			GameDataBridge.AddPostDataModificationHandler(this._defeatMarkUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnDefeatMarkChanged));
			Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnCombatStateMachineUpdateEnd));
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003BDD RID: 15325 RVA: 0x0024A348 File Offset: 0x00248548
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._defeatMarkUid, base.DataHandlerKey);
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnCombatStateMachineUpdateEnd));
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003BDE RID: 15326 RVA: 0x0024A3B0 File Offset: 0x002485B0
		private void OnDefeatMarkChanged(DataContext context, DataUid dataUid)
		{
			this._delaying = true;
		}

		// Token: 0x06003BDF RID: 15327 RVA: 0x0024A3BC File Offset: 0x002485BC
		private void OnCombatStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			bool flag = combatChar.GetId() != base.CharacterId;
			if (!flag)
			{
				bool checking = this._checking;
				this._checking = false;
				bool flag2 = combatChar.NeedUseSkillFreeId >= 0 || !this._delaying;
				if (!flag2)
				{
					bool flag3 = combatChar.StateMachine.GetCurrentStateType() > CombatCharacterStateType.Idle;
					if (!flag3)
					{
						this._checking = true;
						bool flag4 = !checking;
						if (!flag4)
						{
							this._delaying = false;
							bool flag5 = base.EffectCount <= 0;
							if (!flag5)
							{
								bool flag6 = base.CombatChar.GetDefeatMarkCollection().GetTotalCount() <= (int)(GlobalConfig.NeedDefeatMarkCount[(int)DomainManager.Combat.GetCombatType()] / 2);
								if (!flag6)
								{
									bool flag7 = !DomainManager.Combat.CanCastSkill(base.CombatChar, base.SkillTemplateId, true, true);
									if (!flag7)
									{
										this._affecting = true;
										DomainManager.Combat.CastSkillFree(context, base.CombatChar, base.SkillTemplateId, ECombatCastFreePriority.Normal);
										base.ShowSpecialEffectTips(0);
										base.ReduceEffectCount(1);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06003BE0 RID: 15328 RVA: 0x0024A4E0 File Offset: 0x002486E0
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || !this._affecting;
			if (!flag)
			{
				DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * 75 / 100);
			}
		}

		// Token: 0x06003BE1 RID: 15329 RVA: 0x0024A538 File Offset: 0x00248738
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker != base.CombatChar || skillId != base.SkillTemplateId || !this._affecting;
			if (!flag)
			{
				DefeatMarkCollection markCollection = base.CombatChar.GetDefeatMarkCollection();
				this._addDamagePercent = 30 * (base.IsDirect ? markCollection.OuterInjuryMarkList : markCollection.InnerInjuryMarkList).Sum();
			}
		}

		// Token: 0x06003BE2 RID: 15330 RVA: 0x0024A59C File Offset: 0x0024879C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || !this._affecting;
			if (!flag)
			{
				this._affecting = false;
			}
		}

		// Token: 0x06003BE3 RID: 15331 RVA: 0x0024A5D8 File Offset: 0x002487D8
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId == base.CharacterId && dataKey.FieldId == 69 && this._affecting && dataKey.CombatSkillId == base.SkillTemplateId && dataKey.CustomParam0 == (base.IsDirect ? 0 : 1);
			int result;
			if (flag)
			{
				base.ShowSpecialEffectTips(1);
				result = this._addDamagePercent;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x04001189 RID: 4489
		private const sbyte PrepareProgressPercent = 75;

		// Token: 0x0400118A RID: 4490
		private const sbyte AddDamagePercentUnit = 30;

		// Token: 0x0400118B RID: 4491
		private DataUid _defeatMarkUid;

		// Token: 0x0400118C RID: 4492
		private bool _checking;

		// Token: 0x0400118D RID: 4493
		private bool _delaying;

		// Token: 0x0400118E RID: 4494
		private bool _affecting;

		// Token: 0x0400118F RID: 4495
		private int _addDamagePercent;
	}
}
