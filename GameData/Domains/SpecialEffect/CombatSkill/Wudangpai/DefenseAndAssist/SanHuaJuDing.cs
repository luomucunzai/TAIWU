using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.DefenseAndAssist
{
	// Token: 0x020003D8 RID: 984
	public class SanHuaJuDing : AssistSkillBase
	{
		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x060037BA RID: 14266 RVA: 0x00236EF6 File Offset: 0x002350F6
		private short SelfPower
		{
			get
			{
				return base.SkillInstance.GetPower();
			}
		}

		// Token: 0x060037BB RID: 14267 RVA: 0x00236F03 File Offset: 0x00235103
		public SanHuaJuDing()
		{
		}

		// Token: 0x060037BC RID: 14268 RVA: 0x00236F0D File Offset: 0x0023510D
		public SanHuaJuDing(CombatSkillKey skillKey) : base(skillKey, 4607)
		{
		}

		// Token: 0x060037BD RID: 14269 RVA: 0x00236F20 File Offset: 0x00235120
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnCombatStateMachineUpdateEnd));
			this._originalPower = this.SelfPower;
			this._neiliAllocationUid = base.ParseNeiliAllocationDataUid();
			GameDataBridge.AddPostDataModificationHandler(this._neiliAllocationUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnNeiliAllocationChanged));
		}

		// Token: 0x060037BE RID: 14270 RVA: 0x00236FA4 File Offset: 0x002351A4
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnCombatStateMachineUpdateEnd));
			GameDataBridge.RemovePostDataModificationHandler(this._neiliAllocationUid, base.DataHandlerKey);
			base.OnDisable(context);
		}

		// Token: 0x060037BF RID: 14271 RVA: 0x00237002 File Offset: 0x00235202
		protected override IEnumerable<int> CalcFrameCounterPeriods()
		{
			yield return 30;
			yield break;
		}

		// Token: 0x060037C0 RID: 14272 RVA: 0x00237012 File Offset: 0x00235212
		public override bool IsOn(int counterType)
		{
			return this._affecting && this._addedPower < this._originalPower;
		}

		// Token: 0x060037C1 RID: 14273 RVA: 0x0023702D File Offset: 0x0023522D
		public override void OnProcess(DataContext context, int counterType)
		{
			this._addedPower = (short)Math.Min((int)(this._addedPower + 1), (int)this._originalPower);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
		}

		// Token: 0x060037C2 RID: 14274 RVA: 0x00237064 File Offset: 0x00235264
		private void OnCombatBegin(DataContext context)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.AppendAffectedData(context, 199, EDataModifyType.Custom, -1);
			}
			else
			{
				base.AppendAffectedData(context, 199, EDataModifyType.Custom, base.SkillTemplateId);
				base.AppendAffectedAllEnemyData(context, 199, EDataModifyType.Custom, -1);
			}
			this.OnNeiliAllocationChanged(context, this._neiliAllocationUid);
		}

		// Token: 0x060037C3 RID: 14275 RVA: 0x002370C0 File Offset: 0x002352C0
		private void OnCombatCharChanged(DataContext context, bool isAlly)
		{
			bool flag = isAlly == base.CombatChar.IsAlly;
			if (flag)
			{
				base.InvalidateAllEnemyCache(context, 199);
			}
		}

		// Token: 0x060037C4 RID: 14276 RVA: 0x002370F0 File Offset: 0x002352F0
		private void OnCombatStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			bool flag = !DomainManager.Combat.IsCharInCombat(combatChar.GetId(), true) || combatChar.GetId() != base.CharacterId;
			if (!flag)
			{
				int changingPower = (int)((this.SelfPower < 100) ? 0 : (this.SelfPower / 10 * (base.IsDirect ? 1 : -1)));
				bool flag2 = changingPower == this._changingPower;
				if (!flag2)
				{
					this._changingPower = changingPower;
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						base.InvalidateCache(context, 199);
					}
					else
					{
						base.InvalidateAllEnemyCache(context, 199);
					}
				}
			}
		}

		// Token: 0x060037C5 RID: 14277 RVA: 0x0023718C File Offset: 0x0023538C
		private unsafe void OnNeiliAllocationChanged(DataContext context, DataUid dataUid)
		{
			bool allNotLess = true;
			NeiliAllocation originNeiliAllocation = base.CombatChar.GetOriginNeiliAllocation();
			NeiliAllocation currNeiliAllocation = base.CombatChar.GetNeiliAllocation();
			for (int i = 0; i < 4; i++)
			{
				allNotLess = (allNotLess && *(ref currNeiliAllocation.Items.FixedElementField + (IntPtr)i * 2) >= *(ref originNeiliAllocation.Items.FixedElementField + (IntPtr)i * 2));
			}
			bool flag = this._affecting == allNotLess;
			if (!flag)
			{
				this._affecting = allNotLess;
				base.SetConstAffecting(context, this._affecting);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			}
		}

		// Token: 0x060037C6 RID: 14278 RVA: 0x00237234 File Offset: 0x00235434
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = !base.CanAffect || dataKey.FieldId != 199;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.CombatSkillId == base.SkillTemplateId;
				if (flag2)
				{
					result = ((dataKey.CharId != base.CharacterId) ? dataValue : (this._affecting ? (dataValue + (int)this._addedPower) : ((dataValue + (int)this._addedPower) / 2)));
				}
				else
				{
					bool flag3 = !base.IsDirect && !base.IsCurrent;
					if (flag3)
					{
						result = dataValue;
					}
					else
					{
						bool flag4 = base.IsDirect ? (base.CharacterId == dataKey.CharId) : (base.CharacterId != dataKey.CharId);
						if (flag4)
						{
							result = dataValue + this._changingPower;
						}
						else
						{
							result = dataValue;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x04001040 RID: 4160
		private const int PowerAddFrame = 30;

		// Token: 0x04001041 RID: 4161
		private const int PowerAddUnit = 1;

		// Token: 0x04001042 RID: 4162
		private const int SpreadRequirePower = 100;

		// Token: 0x04001043 RID: 4163
		private const int SpreadPowerDivisor = 10;

		// Token: 0x04001044 RID: 4164
		private DataUid _neiliAllocationUid;

		// Token: 0x04001045 RID: 4165
		private bool _affecting;

		// Token: 0x04001046 RID: 4166
		private short _originalPower;

		// Token: 0x04001047 RID: 4167
		private short _addedPower;

		// Token: 0x04001048 RID: 4168
		private int _changingPower;
	}
}
