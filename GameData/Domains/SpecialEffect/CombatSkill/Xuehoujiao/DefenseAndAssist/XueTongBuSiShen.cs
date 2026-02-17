using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.DefenseAndAssist
{
	// Token: 0x02000246 RID: 582
	public class XueTongBuSiShen : AssistSkillBase
	{
		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06002FE1 RID: 12257 RVA: 0x00214D42 File Offset: 0x00212F42
		private bool DefeatMarkMax
		{
			get
			{
				return base.CombatChar.GetDefeatMarkCollection().GetTotalCount() >= (int)GlobalConfig.NeedDefeatMarkCount[(int)DomainManager.Combat.GetCombatType()];
			}
		}

		// Token: 0x06002FE2 RID: 12258 RVA: 0x00214D69 File Offset: 0x00212F69
		public XueTongBuSiShen()
		{
		}

		// Token: 0x06002FE3 RID: 12259 RVA: 0x00214D73 File Offset: 0x00212F73
		public XueTongBuSiShen(CombatSkillKey skillKey) : base(skillKey, 15808)
		{
		}

		// Token: 0x06002FE4 RID: 12260 RVA: 0x00214D84 File Offset: 0x00212F84
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(69, EDataModifyType.AddPercent, -1);
			base.CreateAffectedData(102, EDataModifyType.AddPercent, -1);
			base.CreateAffectedData(206, EDataModifyType.TotalPercent, -1);
			base.CreateAffectedData(205, EDataModifyType.TotalPercent, -1);
			base.CreateAffectedData(282, EDataModifyType.Custom, -1);
			this._affecting = false;
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.AddDirectDamageValue));
			Events.RegisterHandler_CostBreathAndStance(new Events.OnCostBreathAndStance(this.CostBreathAndStance));
		}

		// Token: 0x06002FE5 RID: 12261 RVA: 0x00214E18 File Offset: 0x00213018
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.AddDirectDamageValue));
			Events.UnRegisterHandler_CostBreathAndStance(new Events.OnCostBreathAndStance(this.CostBreathAndStance));
			GameDataBridge.RemovePostDataModificationHandler(this._qiDisorderUid, base.DataHandlerKey);
			GameDataBridge.RemovePostDataModificationHandler(this._defeatMarkUid, base.DataHandlerKey);
			bool affecting = this._affecting;
			if (affecting)
			{
				Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
			}
		}

		// Token: 0x06002FE6 RID: 12262 RVA: 0x00214E94 File Offset: 0x00213094
		private void OnCombatBegin(DataContext context)
		{
			this._qiDisorderUid = new DataUid(4, 0, (ulong)((long)base.CharacterId), 21U);
			this._defeatMarkUid = new DataUid(8, 10, (ulong)((long)base.CharacterId), 50U);
			GameDataBridge.AddPostDataModificationHandler(this._qiDisorderUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.UpdateDirectCanAffect));
			GameDataBridge.AddPostDataModificationHandler(this._defeatMarkUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnDefeatMarkChanged));
			this.UpdateDirectCanAffect(context, default(DataUid));
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x06002FE7 RID: 12263 RVA: 0x00214F30 File Offset: 0x00213130
		private void AddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId)
		{
			bool flag = !this._affecting || !this.DefeatMarkMax;
			if (!flag)
			{
				int charId = base.CombatChar.GetId();
				bool typeMatch = (attackerId == charId) ? (base.IsDirect != isInner) : (defenderId == charId && base.IsDirect == isInner);
				bool flag2 = !typeMatch;
				if (!flag2)
				{
					base.ShowEffectTips(context);
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x06002FE8 RID: 12264 RVA: 0x00214FA4 File Offset: 0x002131A4
		private void CostBreathAndStance(DataContext context, int charId, bool isAlly, int costBreath, int costStance, short skillId)
		{
			bool typeMatch = base.IsDirect ? (costStance > 0) : (costBreath > 0);
			bool flag = base.CombatChar.GetId() != charId || !typeMatch || !this.DefeatMarkMax;
			if (!flag)
			{
				base.ShowEffectTips(context);
				base.ShowSpecialEffectTips(2);
			}
		}

		// Token: 0x06002FE9 RID: 12265 RVA: 0x00214FFC File Offset: 0x002131FC
		private void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			bool flag = base.CombatChar != combatChar || DomainManager.Combat.Pause || !DomainManager.Combat.IsCurrentCombatCharacter(base.CombatChar) || !this.DefeatMarkMax;
			if (!flag)
			{
				this._frameCounter++;
				bool flag2 = this._frameCounter >= 60;
				if (flag2)
				{
					this._frameCounter = 0;
					DomainManager.Combat.ChangeDisorderOfQiRandomRecovery(context, base.CombatChar, 400, true);
					base.ShowEffectTips(context);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06002FEA RID: 12266 RVA: 0x00215094 File Offset: 0x00213294
		protected override void OnCanUseChanged(DataContext context, DataUid dataUid)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.UpdateDirectCanAffect(context, default(DataUid));
			}
		}

		// Token: 0x06002FEB RID: 12267 RVA: 0x002150C0 File Offset: 0x002132C0
		private void UpdateDirectCanAffect(DataContext context, DataUid dataUid)
		{
			bool canAffect = base.CanAffect && DisorderLevelOfQi.GetDisorderLevelOfQi(this.CharObj.GetDisorderOfQi()) < 4;
			bool flag = this._affecting == canAffect;
			if (!flag)
			{
				this._affecting = canAffect;
				base.SetConstAffecting(context, canAffect);
				this.InvalidateAllCache(context);
				bool flag2 = canAffect;
				if (flag2)
				{
					this._frameCounter = 0;
					Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
				}
				else
				{
					Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
					DomainManager.Combat.AddToCheckFallenSet(base.CombatChar.GetId());
				}
			}
		}

		// Token: 0x06002FEC RID: 12268 RVA: 0x00215160 File Offset: 0x00213360
		private void OnDefeatMarkChanged(DataContext context, DataUid dataUid)
		{
			this.InvalidateAllCache(context);
		}

		// Token: 0x06002FED RID: 12269 RVA: 0x0021516C File Offset: 0x0021336C
		private void InvalidateAllCache(DataContext context)
		{
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 69);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 102);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 206);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 205);
		}

		// Token: 0x06002FEE RID: 12270 RVA: 0x002151D0 File Offset: 0x002133D0
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !this._affecting || !this.DefeatMarkMax;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.CustomParam0 == (base.IsDirect ? 0 : 1) && dataKey.FieldId == 69;
				if (flag2)
				{
					result = 80;
				}
				else
				{
					bool flag3 = dataKey.CustomParam0 == (base.IsDirect ? 1 : 0) && dataKey.FieldId == 102;
					if (flag3)
					{
						result = 80;
					}
					else
					{
						bool flag4 = dataKey.FieldId == (base.IsDirect ? 206 : 205);
						if (flag4)
						{
							result = -50;
						}
						else
						{
							result = 0;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06002FEF RID: 12271 RVA: 0x00215288 File Offset: 0x00213488
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
				result = (dataValue || this._affecting);
			}
			return result;
		}

		// Token: 0x04000E30 RID: 3632
		private const short AddQiDisorderFrame = 60;

		// Token: 0x04000E31 RID: 3633
		private const short AddQiDisorder = 400;

		// Token: 0x04000E32 RID: 3634
		private const int DamageAddPercent = 80;

		// Token: 0x04000E33 RID: 3635
		private const int CostStanceBreathOnCastReducePercent = 50;

		// Token: 0x04000E34 RID: 3636
		private bool _affecting;

		// Token: 0x04000E35 RID: 3637
		private int _frameCounter;

		// Token: 0x04000E36 RID: 3638
		private DataUid _qiDisorderUid;

		// Token: 0x04000E37 RID: 3639
		private DataUid _defeatMarkUid;
	}
}
