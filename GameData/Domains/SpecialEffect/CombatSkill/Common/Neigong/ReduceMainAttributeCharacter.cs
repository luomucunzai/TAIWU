using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong
{
	// Token: 0x0200057A RID: 1402
	public abstract class ReduceMainAttributeCharacter : ReduceMainAttribute
	{
		// Token: 0x17000270 RID: 624
		// (get) Token: 0x0600416F RID: 16751
		protected abstract bool DirectIsAlly { get; }

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06004170 RID: 16752
		protected abstract IReadOnlyList<ushort> FieldIds { get; }

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06004171 RID: 16753
		protected abstract EDataModifyType ModifyType { get; }

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06004172 RID: 16754
		protected abstract int CurrAddValue { get; }

		// Token: 0x06004173 RID: 16755
		protected abstract IEnumerable<DataUid> GetUpdateAffectingDataUids();

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06004174 RID: 16756 RVA: 0x00262C2A File Offset: 0x00260E2A
		protected override bool IsAffect
		{
			get
			{
				return this._affecting;
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06004175 RID: 16757 RVA: 0x00262C32 File Offset: 0x00260E32
		private bool TargetIsAlly
		{
			get
			{
				return base.IsDirect == this.DirectIsAlly;
			}
		}

		// Token: 0x06004176 RID: 16758 RVA: 0x00262C42 File Offset: 0x00260E42
		protected ReduceMainAttributeCharacter()
		{
		}

		// Token: 0x06004177 RID: 16759 RVA: 0x00262C57 File Offset: 0x00260E57
		protected ReduceMainAttributeCharacter(CombatSkillKey skillKey, int type) : base(skillKey, type)
		{
		}

		// Token: 0x06004178 RID: 16760
		protected abstract bool IsTargetMatchAffect(CombatCharacter target);

		// Token: 0x06004179 RID: 16761 RVA: 0x00262C6E File Offset: 0x00260E6E
		protected virtual void OnAffected()
		{
		}

		// Token: 0x0600417A RID: 16762 RVA: 0x00262C71 File Offset: 0x00260E71
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
		}

		// Token: 0x0600417B RID: 16763 RVA: 0x00262CA0 File Offset: 0x00260EA0
		public override void OnDisable(DataContext context)
		{
			foreach (DataUid uid in this._dataUids)
			{
				GameDataBridge.RemovePostDataModificationHandler(uid, base.DataHandlerKey);
			}
			this._dataUids.Clear();
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			base.OnDisable(context);
		}

		// Token: 0x0600417C RID: 16764 RVA: 0x00262D34 File Offset: 0x00260F34
		private void OnCombatBegin(DataContext context)
		{
			foreach (ushort fieldId in this.FieldIds)
			{
				bool targetIsAlly = this.TargetIsAlly;
				if (targetIsAlly)
				{
					base.AppendAffectedData(context, fieldId, this.ModifyType, -1);
				}
				else
				{
					base.AppendAffectedAllEnemyData(context, fieldId, this.ModifyType, -1);
				}
			}
			this._dataUids.AddRange(this.GetUpdateAffectingDataUids());
			foreach (DataUid uid in this._dataUids)
			{
				GameDataBridge.AddPostDataModificationHandler(uid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnDataChanged));
			}
		}

		// Token: 0x0600417D RID: 16765 RVA: 0x00262E18 File Offset: 0x00261018
		public override void OnProcess(DataContext context, int counterType)
		{
			base.OnProcess(context, counterType);
			this.UpdateAffecting(context);
			bool affecting = this._affecting;
			if (affecting)
			{
				this.InvalidAllCaches(context);
			}
		}

		// Token: 0x0600417E RID: 16766 RVA: 0x00262E49 File Offset: 0x00261049
		private void OnDataChanged(DataContext context, DataUid arg2)
		{
			this.UpdateAffecting(context);
		}

		// Token: 0x0600417F RID: 16767 RVA: 0x00262E54 File Offset: 0x00261054
		private void OnCombatCharChanged(DataContext context, bool isAlly)
		{
			bool flag = isAlly == base.CombatChar.IsAlly;
			if (flag)
			{
				this.UpdateAffecting(context);
			}
		}

		// Token: 0x06004180 RID: 16768 RVA: 0x00262E7C File Offset: 0x0026107C
		private void UpdateAffecting(DataContext context)
		{
			bool affecting = base.IsCurrent && this.CurrAddValue != 0 && this.IsTargetMatchAffect(this.TargetIsAlly ? base.CombatChar : base.EnemyChar);
			bool flag = affecting == this._affecting;
			if (!flag)
			{
				this._affecting = affecting;
				this.InvalidAllCaches(context);
				bool affecting2 = this._affecting;
				if (affecting2)
				{
					this.OnAffected();
				}
			}
		}

		// Token: 0x06004181 RID: 16769 RVA: 0x00262EEC File Offset: 0x002610EC
		private void InvalidAllCaches(DataContext context)
		{
			foreach (ushort fieldId in this.FieldIds)
			{
				bool targetIsAlly = this.TargetIsAlly;
				if (targetIsAlly)
				{
					base.InvalidateCache(context, fieldId);
				}
				else
				{
					base.InvalidateAllEnemyCache(context, fieldId);
				}
			}
		}

		// Token: 0x06004182 RID: 16770 RVA: 0x00262F58 File Offset: 0x00261158
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !this.IsAffect || !this.FieldIds.Contains(dataKey.FieldId);
			int result;
			if (flag)
			{
				result = base.GetModifyValue(dataKey, currModifyValue);
			}
			else
			{
				CombatCharacter combatChar;
				bool flag2 = !DomainManager.Combat.TryGetElement_CombatCharacterDict(dataKey.CharId, out combatChar);
				if (flag2)
				{
					result = base.GetModifyValue(dataKey, currModifyValue);
				}
				else
				{
					bool flag3 = this.TargetIsAlly ? (combatChar.GetId() != base.CharacterId) : (combatChar.IsAlly == base.CombatChar.IsAlly);
					if (flag3)
					{
						result = base.GetModifyValue(dataKey, currModifyValue);
					}
					else
					{
						result = this.CurrAddValue;
					}
				}
			}
			return result;
		}

		// Token: 0x0400134D RID: 4941
		private bool _affecting;

		// Token: 0x0400134E RID: 4942
		private readonly List<DataUid> _dataUids = new List<DataUid>();
	}
}
