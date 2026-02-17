using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.SpecialEffect.Animal;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.SectStory.Fulong
{
	// Token: 0x020000F6 RID: 246
	public class SoulWitheringBell : CarrierEffectBase
	{
		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x0600298D RID: 10637 RVA: 0x0020104B File Offset: 0x001FF24B
		protected override short CombatStateId
		{
			get
			{
				return 241;
			}
		}

		// Token: 0x0600298E RID: 10638 RVA: 0x00201052 File Offset: 0x001FF252
		public SoulWitheringBell(int charId) : base(charId)
		{
		}

		// Token: 0x0600298F RID: 10639 RVA: 0x00201060 File Offset: 0x001FF260
		protected override void OnEnableSubClass(DataContext context)
		{
			foreach (ushort fieldId in SoulWitheringBell.AllFieldIds)
			{
				base.CreateAffectedData(fieldId, EDataModifyType.Custom, -1);
			}
			this._dataUid = base.ParseCombatCharacterDataUid(base.EnemyChar.GetId(), 50);
			GameDataBridge.AddPostDataModificationHandler(this._dataUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnDefeatMarkChanged));
		}

		// Token: 0x06002990 RID: 10640 RVA: 0x002010F0 File Offset: 0x001FF2F0
		protected override void OnDisableSubClass(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._dataUid, base.DataHandlerKey);
		}

		// Token: 0x06002991 RID: 10641 RVA: 0x00201108 File Offset: 0x001FF308
		private void OnDefeatMarkChanged(DataContext context, DataUid arg2)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetMainCharacter(!base.CombatChar.IsAlly);
			bool flag = this._transferred || !DomainManager.Combat.IsCharacterHalfFallen(enemyChar);
			if (!flag)
			{
				this._transferred = true;
				DomainManager.Combat.RemoveCombatState(context, base.CombatChar, 0, this.CombatStateId);
				DomainManager.Combat.AddCombatState(context, enemyChar, 0, 242, 100, false, true, base.CharacterId);
				DomainManager.Combat.ShowSpecialEffectTips(base.CharacterId, 1717, 0);
				foreach (ushort fieldId in SoulWitheringBell.AllFieldIds)
				{
					base.AppendAffectedData(context, enemyChar.GetId(), fieldId, EDataModifyType.Custom, -1);
					base.RemoveAffectedData(context, base.CharacterId, fieldId);
				}
				DomainManager.TaiwuEvent.OnEvent_SoulWitheringBellTransfer();
			}
		}

		// Token: 0x06002992 RID: 10642 RVA: 0x00201214 File Offset: 0x001FF414
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = !SoulWitheringBell.AllFieldIds.Contains(dataKey.FieldId);
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = this._transferred ? (dataKey.CharId == base.CharacterId) : (dataKey.CharId != base.CharacterId);
				if (flag2)
				{
					result = dataValue;
				}
				else
				{
					result = dataValue * SoulWitheringBell.MinorAttributePercent;
				}
			}
			return result;
		}

		// Token: 0x04000CD2 RID: 3282
		private static readonly List<ushort> AllFieldIds = new List<ushort>
		{
			8,
			7,
			9,
			14,
			11,
			13,
			10,
			12,
			16,
			15
		};

		// Token: 0x04000CD3 RID: 3283
		private static readonly CValuePercent MinorAttributePercent = 50;

		// Token: 0x04000CD4 RID: 3284
		private DataUid _dataUid;

		// Token: 0x04000CD5 RID: 3285
		private bool _transferred;
	}
}
