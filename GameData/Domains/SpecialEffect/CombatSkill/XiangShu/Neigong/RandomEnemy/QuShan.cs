using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Neigong.RandomEnemy
{
	// Token: 0x02000294 RID: 660
	public class QuShan : MinionBase
	{
		// Token: 0x0600314F RID: 12623 RVA: 0x0021A7E7 File Offset: 0x002189E7
		public QuShan()
		{
		}

		// Token: 0x06003150 RID: 12624 RVA: 0x0021A7F1 File Offset: 0x002189F1
		public QuShan(CombatSkillKey skillKey) : base(skillKey, 16001)
		{
		}

		// Token: 0x06003151 RID: 12625 RVA: 0x0021A804 File Offset: 0x00218A04
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 145, -1, -1, -1, -1), EDataModifyType.Add);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 146, -1, -1, -1, -1), EDataModifyType.Add);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
		}

		// Token: 0x06003152 RID: 12626 RVA: 0x0021A883 File Offset: 0x00218A83
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._enemyMarkUid, base.DataHandlerKey);
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
		}

		// Token: 0x06003153 RID: 12627 RVA: 0x0021A8BC File Offset: 0x00218ABC
		private void OnCombatBegin(DataContext context)
		{
			this.UpdateEnemyDataUid(context);
		}

		// Token: 0x06003154 RID: 12628 RVA: 0x0021A8C8 File Offset: 0x00218AC8
		private void OnCombatCharChanged(DataContext context, bool isAlly)
		{
			bool flag = isAlly == base.CombatChar.IsAlly;
			if (!flag)
			{
				GameDataBridge.RemovePostDataModificationHandler(this._enemyMarkUid, base.DataHandlerKey);
				this.UpdateEnemyDataUid(context);
			}
		}

		// Token: 0x06003155 RID: 12629 RVA: 0x0021A904 File Offset: 0x00218B04
		private void UpdateEnemyDataUid(DataContext context)
		{
			this._enemyMarkUid = base.ParseCombatCharacterDataUid(base.EnemyChar.GetId(), 50);
			GameDataBridge.AddPostDataModificationHandler(this._enemyMarkUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.UpdateAddDistance));
			this.UpdateAddDistance(context, default(DataUid));
		}

		// Token: 0x06003156 RID: 12630 RVA: 0x0021A95C File Offset: 0x00218B5C
		private void UpdateAddDistance(DataContext context, DataUid dataUid)
		{
			int markCount = base.EnemyChar.GetDefeatMarkCollection().GetTotalCount();
			this._addAttackDistance = markCount / 3 * 10;
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 145);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 146);
		}

		// Token: 0x06003157 RID: 12631 RVA: 0x0021A9B8 File Offset: 0x00218BB8
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !MinionBase.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = this._addAttackDistance;
			}
			return result;
		}

		// Token: 0x04000E9D RID: 3741
		private const sbyte RequireMarkPerDistance = 3;

		// Token: 0x04000E9E RID: 3742
		private DataUid _enemyMarkUid;

		// Token: 0x04000E9F RID: 3743
		private int _addAttackDistance;
	}
}
