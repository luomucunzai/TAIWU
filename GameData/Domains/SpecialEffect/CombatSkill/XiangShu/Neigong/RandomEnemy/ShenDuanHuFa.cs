using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Neigong.RandomEnemy
{
	// Token: 0x02000295 RID: 661
	public class ShenDuanHuFa : MinionBase
	{
		// Token: 0x06003158 RID: 12632 RVA: 0x0021A9F1 File Offset: 0x00218BF1
		public ShenDuanHuFa()
		{
		}

		// Token: 0x06003159 RID: 12633 RVA: 0x0021A9FB File Offset: 0x00218BFB
		public ShenDuanHuFa(CombatSkillKey skillKey) : base(skillKey, 16007)
		{
		}

		// Token: 0x0600315A RID: 12634 RVA: 0x0021AA0B File Offset: 0x00218C0B
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
		}

		// Token: 0x0600315B RID: 12635 RVA: 0x0021AA32 File Offset: 0x00218C32
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._infectionUid, base.DataHandlerKey);
			Events.UnRegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
		}

		// Token: 0x0600315C RID: 12636 RVA: 0x0021AA59 File Offset: 0x00218C59
		private void OnCombatBegin(DataContext context)
		{
			this._isCurrCombatChar = DomainManager.Combat.IsCurrentCombatCharacter(base.CombatChar);
			this.UpdateEnemyData(context);
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x0600315D RID: 12637 RVA: 0x0021AA8C File Offset: 0x00218C8C
		private void OnCombatCharChanged(DataContext context, bool isAlly)
		{
			bool flag = isAlly == base.CombatChar.IsAlly;
			if (flag)
			{
				bool isCurrChar = DomainManager.Combat.IsCurrentCombatCharacter(base.CombatChar);
				bool flag2 = this._isCurrCombatChar == isCurrChar;
				if (!flag2)
				{
					this._isCurrCombatChar = isCurrChar;
					this.InvalidateAffectDataCache(context);
				}
			}
			else
			{
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					GameDataBridge.RemovePostDataModificationHandler(this._infectionUid, base.DataHandlerKey);
					this.UpdateEnemyData(context);
				}
			}
		}

		// Token: 0x0600315E RID: 12638 RVA: 0x0021AB08 File Offset: 0x00218D08
		private void UpdateEnemyData(DataContext context)
		{
			int enemyCharId = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false).GetId();
			base.ClearAffectedData(context);
			base.AppendAffectedData(context, enemyCharId, 44, EDataModifyType.TotalPercent, -1);
			base.AppendAffectedData(context, enemyCharId, 45, EDataModifyType.TotalPercent, -1);
			base.AppendAffectedData(context, enemyCharId, 46, EDataModifyType.TotalPercent, -1);
			base.AppendAffectedData(context, enemyCharId, 47, EDataModifyType.TotalPercent, -1);
			this._infectionUid = base.ParseCharDataUid(enemyCharId, 65);
			GameDataBridge.AddPostDataModificationHandler(this._infectionUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.UpdateEffect));
			this.UpdateEffect(context, default(DataUid));
		}

		// Token: 0x0600315F RID: 12639 RVA: 0x0021ABB0 File Offset: 0x00218DB0
		private void UpdateEffect(DataContext context, DataUid dataUid)
		{
			int infection = (int)base.EnemyChar.GetCharacter().GetXiangshuInfection();
			this._reducePercent = -Math.Min(infection * 100 / 200, 50);
			bool isCurrCombatChar = this._isCurrCombatChar;
			if (isCurrCombatChar)
			{
				this.InvalidateAffectDataCache(context);
			}
		}

		// Token: 0x06003160 RID: 12640 RVA: 0x0021ABFC File Offset: 0x00218DFC
		private void InvalidateAffectDataCache(DataContext context)
		{
			int enemyCharId = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false).GetId();
			DomainManager.SpecialEffect.InvalidateCache(context, enemyCharId, 44);
			DomainManager.SpecialEffect.InvalidateCache(context, enemyCharId, 45);
			DomainManager.SpecialEffect.InvalidateCache(context, enemyCharId, 46);
			DomainManager.SpecialEffect.InvalidateCache(context, enemyCharId, 47);
		}

		// Token: 0x06003161 RID: 12641 RVA: 0x0021AC68 File Offset: 0x00218E68
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !this._isCurrCombatChar || !MinionBase.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = this._reducePercent;
			}
			return result;
		}

		// Token: 0x04000EA0 RID: 3744
		private const sbyte MaxReducePercent = 50;

		// Token: 0x04000EA1 RID: 3745
		private bool _isCurrCombatChar;

		// Token: 0x04000EA2 RID: 3746
		private DataUid _infectionUid;

		// Token: 0x04000EA3 RID: 3747
		private int _reducePercent;
	}
}
