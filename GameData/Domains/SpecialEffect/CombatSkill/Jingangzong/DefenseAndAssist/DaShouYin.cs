using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.DefenseAndAssist
{
	// Token: 0x020004C4 RID: 1220
	public class DaShouYin : AssistSkillBase
	{
		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06003D18 RID: 15640 RVA: 0x0024FF18 File Offset: 0x0024E118
		private int EnemyMaxTrickCount
		{
			get
			{
				return this._enemyUselessTrickCount + 5;
			}
		}

		// Token: 0x06003D19 RID: 15641 RVA: 0x0024FF22 File Offset: 0x0024E122
		public DaShouYin()
		{
		}

		// Token: 0x06003D1A RID: 15642 RVA: 0x0024FF2C File Offset: 0x0024E12C
		public DaShouYin(CombatSkillKey skillKey) : base(skillKey, 11706)
		{
		}

		// Token: 0x06003D1B RID: 15643 RVA: 0x0024FF3C File Offset: 0x0024E13C
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._selfNeiliAllocationUid = base.ParseNeiliAllocationDataUid();
			GameDataBridge.AddPostDataModificationHandler(this._selfNeiliAllocationUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.UpdateCanAffect));
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			Events.RegisterHandler_OverflowTrickRemoved(new Events.OnOverflowTrickRemoved(this.OnOverflowTrickRemoved));
		}

		// Token: 0x06003D1C RID: 15644 RVA: 0x0024FFB4 File Offset: 0x0024E1B4
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			GameDataBridge.RemovePostDataModificationHandler(this._selfNeiliAllocationUid, base.DataHandlerKey);
			this.RemoveEnemyUid();
			Events.UnRegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			Events.UnRegisterHandler_OverflowTrickRemoved(new Events.OnOverflowTrickRemoved(this.OnOverflowTrickRemoved));
		}

		// Token: 0x06003D1D RID: 15645 RVA: 0x00250008 File Offset: 0x0024E208
		private void OnCombatBegin(DataContext context)
		{
			base.AppendAffectedCurrEnemyData(context, 170, EDataModifyType.Custom, -1);
			this.AppendEnemyUid();
			this._affecting = false;
			this._enemyUselessTrickCount = 0;
			this.UpdateCanAffect(context, default(DataUid));
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x06003D1E RID: 15646 RVA: 0x00250060 File Offset: 0x0024E260
		private void OnCombatCharChanged(DataContext context, bool isAlly)
		{
			bool flag = isAlly != base.CombatChar.IsAlly;
			if (flag)
			{
				base.ClearAffectedData(context);
				base.AppendAffectedCurrEnemyData(context, 170, EDataModifyType.Custom, -1);
				this.RemoveEnemyUid();
				this.AppendEnemyUid();
			}
			this.UpdateCanAffect(context, default(DataUid));
		}

		// Token: 0x06003D1F RID: 15647 RVA: 0x002500BC File Offset: 0x0024E2BC
		private void AppendEnemyUid()
		{
			int enemyCharId = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false).GetId();
			this._enemyNeiliAllocationUid = base.ParseNeiliAllocationDataUid(enemyCharId);
			this._enemyTrickUid = base.ParseCombatCharacterDataUid(enemyCharId, 28);
			GameDataBridge.AddPostDataModificationHandler(this._enemyNeiliAllocationUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.UpdateCanAffect));
			GameDataBridge.AddPostDataModificationHandler(this._enemyTrickUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnTrickChanged));
		}

		// Token: 0x06003D20 RID: 15648 RVA: 0x00250141 File Offset: 0x0024E341
		private void RemoveEnemyUid()
		{
			GameDataBridge.RemovePostDataModificationHandler(this._enemyNeiliAllocationUid, base.DataHandlerKey);
			GameDataBridge.RemovePostDataModificationHandler(this._enemyTrickUid, base.DataHandlerKey);
		}

		// Token: 0x06003D21 RID: 15649 RVA: 0x00250168 File Offset: 0x0024E368
		private void OnOverflowTrickRemoved(DataContext context, int charId, bool isAlly, int removedCount)
		{
			bool flag = !this._affecting || isAlly == base.CombatChar.IsAlly || removedCount <= 0;
			if (!flag)
			{
				bool flag2 = this.EnemyMaxTrickCount >= 9;
				if (!flag2)
				{
					CombatCharacter affectChar = base.IsDirect ? DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false) : base.CombatChar;
					bool flag3 = affectChar.ChangeNeiliAllocationRandom(context, 5, removedCount, true);
					if (flag3)
					{
						base.ShowSpecialEffectTips(0);
					}
				}
			}
		}

		// Token: 0x06003D22 RID: 15650 RVA: 0x002501F4 File Offset: 0x0024E3F4
		private void OnTrickChanged(DataContext context, DataUid dataUid)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			this._enemyUselessTrickCount = enemyChar.UselessTrickCount;
			int trickCount = enemyChar.GetTricks().Tricks.Count;
			bool removeTrick = trickCount > this.EnemyMaxTrickCount;
			bool flag = this._affecting && removeTrick;
			if (flag)
			{
				DomainManager.Combat.RemoveOverflowTrick(context, enemyChar, true);
			}
		}

		// Token: 0x06003D23 RID: 15651 RVA: 0x00250260 File Offset: 0x0024E460
		protected override void OnCanUseChanged(DataContext context, DataUid dataUid)
		{
			this.UpdateCanAffect(context, default(DataUid));
		}

		// Token: 0x06003D24 RID: 15652 RVA: 0x00250280 File Offset: 0x0024E480
		private void UpdateCanAffect(DataContext context, DataUid dataUid)
		{
			bool flag = DomainManager.Combat.IsCurrentCombatCharacter(base.CombatChar) && base.CanAffect;
			bool canAffect;
			if (flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				int selfTotalValue = (int)base.CombatChar.GetNeiliAllocation().GetTotal();
				int enemyTotalValue = (int)enemyChar.GetNeiliAllocation().GetTotal();
				canAffect = (base.IsDirect ? (selfTotalValue > enemyTotalValue) : (selfTotalValue < enemyTotalValue));
			}
			else
			{
				canAffect = false;
			}
			bool flag2 = this._affecting == canAffect;
			if (!flag2)
			{
				this._affecting = canAffect;
				base.SetConstAffecting(context, canAffect);
				base.InvalidateAllEnemyCache(context, 170);
				bool flag3 = canAffect;
				if (flag3)
				{
					DomainManager.Combat.RemoveOverflowTrick(context, DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false), true);
				}
			}
		}

		// Token: 0x06003D25 RID: 15653 RVA: 0x00250368 File Offset: 0x0024E568
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = !this._affecting;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 170;
				if (flag2)
				{
					result = Math.Min(this.EnemyMaxTrickCount, dataValue);
				}
				else
				{
					result = base.GetModifiedValue(dataKey, dataValue);
				}
			}
			return result;
		}

		// Token: 0x040011F4 RID: 4596
		private const int AddNeiliAllocationCount = 5;

		// Token: 0x040011F5 RID: 4597
		private const int MaxUsableTrickCount = 5;

		// Token: 0x040011F6 RID: 4598
		private DataUid _selfNeiliAllocationUid;

		// Token: 0x040011F7 RID: 4599
		private DataUid _enemyNeiliAllocationUid;

		// Token: 0x040011F8 RID: 4600
		private DataUid _enemyTrickUid;

		// Token: 0x040011F9 RID: 4601
		private bool _affecting;

		// Token: 0x040011FA RID: 4602
		private int _enemyUselessTrickCount;
	}
}
