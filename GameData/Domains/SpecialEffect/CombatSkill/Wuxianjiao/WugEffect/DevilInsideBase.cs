using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.LifeRecord;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect
{
	// Token: 0x02000358 RID: 856
	public class DevilInsideBase : WugEffectBase
	{
		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06003536 RID: 13622 RVA: 0x0022BE31 File Offset: 0x0022A031
		private int AddXiangshuInfectionValue
		{
			get
			{
				return base.IsElite ? 50 : 5;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06003537 RID: 13623 RVA: 0x0022BE40 File Offset: 0x0022A040
		private int ChangeNeiliAllocationValue
		{
			get
			{
				return 2 * (base.IsGood ? 1 : -1) * ((!base.IsGrown && base.IsElite) ? 2 : 1);
			}
		}

		// Token: 0x06003538 RID: 13624 RVA: 0x0022BE68 File Offset: 0x0022A068
		public static bool CanGrown(Character character)
		{
			sbyte happinessType = character.GetHappinessType();
			return happinessType == 0 || happinessType == 6;
		}

		// Token: 0x06003539 RID: 13625 RVA: 0x0022BE93 File Offset: 0x0022A093
		protected DevilInsideBase()
		{
		}

		// Token: 0x0600353A RID: 13626 RVA: 0x0022BE9D File Offset: 0x0022A09D
		protected DevilInsideBase(int charId, int type, short wugTemplateId, short effectId) : base(charId, type, wugTemplateId, effectId)
		{
			this.CostWugCount = 0;
		}

		// Token: 0x0600353B RID: 13627 RVA: 0x0022BEB4 File Offset: 0x0022A0B4
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			bool isGrown = base.IsGrown;
			if (isGrown)
			{
				base.CreateAffectedData(262, EDataModifyType.Add, -1);
			}
			bool canChangeToGrown = base.CanChangeToGrown;
			if (canChangeToGrown)
			{
				base.CreateAffectedData(270, EDataModifyType.Add, -1);
			}
			Events.RegisterHandler_AdvanceMonthFinish(new Events.OnAdvanceMonthFinish(this.OnAdvanceMonthFinish));
		}

		// Token: 0x0600353C RID: 13628 RVA: 0x0022BF0C File Offset: 0x0022A10C
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AdvanceMonthFinish(new Events.OnAdvanceMonthFinish(this.OnAdvanceMonthFinish));
			base.OnDisable(context);
		}

		// Token: 0x0600353D RID: 13629 RVA: 0x0022BF29 File Offset: 0x0022A129
		protected override void AddAffectDataAndEvent(DataContext context)
		{
			this._changingNeiliAllocation = false;
			Events.RegisterHandler_NeiliAllocationChanged(new Events.OnNeiliAllocationChanged(this.OnNeiliAllocationChanged));
		}

		// Token: 0x0600353E RID: 13630 RVA: 0x0022BF45 File Offset: 0x0022A145
		protected override void ClearAffectDataAndEvent(DataContext context)
		{
			Events.UnRegisterHandler_NeiliAllocationChanged(new Events.OnNeiliAllocationChanged(this.OnNeiliAllocationChanged));
		}

		// Token: 0x0600353F RID: 13631 RVA: 0x0022BF5C File Offset: 0x0022A15C
		private void OnAdvanceMonthFinish(DataContext context)
		{
			LifeRecordCollection lifeRecord = DomainManager.LifeRecord.GetLifeRecordCollection();
			bool affectedXiangshuOnMonthChange = this._affectedXiangshuOnMonthChange;
			if (affectedXiangshuOnMonthChange)
			{
				this._affectedXiangshuOnMonthChange = false;
				base.AddLifeRecord(new WugEffectBase.LifeRecordAddTemplate(lifeRecord.AddWugDevilInsideXiangshuInfection));
			}
			bool affectedHappinessOnMonthChange = this._affectedHappinessOnMonthChange;
			if (affectedHappinessOnMonthChange)
			{
				this._affectedHappinessOnMonthChange = false;
				base.AddLifeRecord(new WugEffectBase.LifeRecordAddTemplate(lifeRecord.AddWugDevilInsideChangeHappiness));
			}
			bool flag = base.CanChangeToGrown && DevilInsideBase.CanGrown(this.CharObj);
			if (flag)
			{
				this.ChangeToGrown(context);
			}
		}

		// Token: 0x06003540 RID: 13632 RVA: 0x0022BFE4 File Offset: 0x0022A1E4
		private void OnNeiliAllocationChanged(DataContext context, int charId, byte type, int changeValue)
		{
			bool flag = charId != base.CharacterId || this._changingNeiliAllocation || !(base.IsGood ? (changeValue > 0) : (changeValue < 0)) || !base.CanAffect;
			if (!flag)
			{
				CombatCharacter changeWugChar = base.IsGood ? base.CombatChar : DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				this._changingNeiliAllocation = true;
				int changedNeiliAllocation = base.CombatChar.ChangeNeiliAllocation(context, (byte)context.Random.Next(0, 4), this.ChangeNeiliAllocationValue, true, true);
				this._changingNeiliAllocation = false;
				changeWugChar.ChangeWugCount(context, Math.Abs(changeValue));
				bool flag2 = changedNeiliAllocation == 0;
				if (!flag2)
				{
					base.ShowEffectTips(context, 1);
					bool isElite = base.IsElite;
					if (isElite)
					{
						base.ShowEffectTips(context, 2);
					}
					base.CostWugInCombat(context);
				}
			}
		}

		// Token: 0x06003541 RID: 13633 RVA: 0x0022C0C8 File Offset: 0x0022A2C8
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 270;
				if (flag2)
				{
					this._affectedHappinessOnMonthChange = true;
					sbyte happinessType = this.CharObj.GetHappinessType();
					if (!true)
					{
					}
					int num;
					if (happinessType <= 3)
					{
						if (happinessType >= 3)
						{
							num = 0;
						}
						else
						{
							num = -5;
						}
					}
					else
					{
						num = 5;
					}
					if (!true)
					{
					}
					result = num;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 262;
					if (flag3)
					{
						this._affectedXiangshuOnMonthChange = true;
						result = this.AddXiangshuInfectionValue;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04000FA0 RID: 4000
		private bool _changingNeiliAllocation;

		// Token: 0x04000FA1 RID: 4001
		private bool _affectedXiangshuOnMonthChange;

		// Token: 0x04000FA2 RID: 4002
		private bool _affectedHappinessOnMonthChange;
	}
}
