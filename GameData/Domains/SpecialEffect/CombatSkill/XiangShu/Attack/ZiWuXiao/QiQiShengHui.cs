using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.ZiWuXiao
{
	// Token: 0x020002BB RID: 699
	public class QiQiShengHui : CombatSkillEffectBase
	{
		// Token: 0x06003240 RID: 12864 RVA: 0x0021EB83 File Offset: 0x0021CD83
		public QiQiShengHui()
		{
		}

		// Token: 0x06003241 RID: 12865 RVA: 0x0021EB8D File Offset: 0x0021CD8D
		public QiQiShengHui(CombatSkillKey skillKey) : base(skillKey, 17111, -1)
		{
		}

		// Token: 0x06003242 RID: 12866 RVA: 0x0021EBA0 File Offset: 0x0021CDA0
		public override void OnEnable(DataContext context)
		{
			this.UpdateEnemyUid(true);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			Events.RegisterHandler_GetTrick(new Events.OnGetTrick(this.OnGetTrick));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06003243 RID: 12867 RVA: 0x0021EC00 File Offset: 0x0021CE00
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._enemyTricksUid, base.DataHandlerKey);
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			Events.UnRegisterHandler_GetTrick(new Events.OnGetTrick(this.OnGetTrick));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06003244 RID: 12868 RVA: 0x0021EC68 File Offset: 0x0021CE68
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.EffectCount <= 0;
				if (flag2)
				{
					this.IsSrcSkillPerformed = true;
					base.AddMaxEffectCount(true);
				}
				else
				{
					base.RemoveSelf(context);
				}
			}
		}

		// Token: 0x06003245 RID: 12869 RVA: 0x0021ECC4 File Offset: 0x0021CEC4
		private void OnCombatCharChanged(DataContext context, bool isAlly)
		{
			bool flag = isAlly == base.CombatChar.IsAlly;
			if (!flag)
			{
				this.UpdateEnemyUid(false);
			}
		}

		// Token: 0x06003246 RID: 12870 RVA: 0x0021ECF0 File Offset: 0x0021CEF0
		private void OnGetTrick(DataContext context, int charId, bool isAlly, sbyte trickType, bool usable)
		{
			bool flag = charId != base.CharacterId || usable;
			if (!flag)
			{
				base.ReduceEffectCount(1);
			}
		}

		// Token: 0x06003247 RID: 12871 RVA: 0x0021ED1C File Offset: 0x0021CF1C
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003248 RID: 12872 RVA: 0x0021ED6C File Offset: 0x0021CF6C
		private void OnEnemyTricksChanged(DataContext context, DataUid dataUid)
		{
			int newCount = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false).GetTricks().Tricks.Count;
			int addTrickCount = Math.Abs(newCount - this._enemyLastTrickCount);
			bool flag = addTrickCount > 0;
			if (flag)
			{
				DomainManager.Combat.AddRandomTrick(context, base.CombatChar, addTrickCount);
				base.ShowSpecialEffectTips(0);
			}
			this._enemyLastTrickCount = newCount;
		}

		// Token: 0x06003249 RID: 12873 RVA: 0x0021EDE0 File Offset: 0x0021CFE0
		private void UpdateEnemyUid(bool init)
		{
			CombatCharacter currEnemy = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			this._enemyLastTrickCount = currEnemy.GetTricks().Tricks.Count;
			bool flag = !init;
			if (flag)
			{
				GameDataBridge.RemovePostDataModificationHandler(this._enemyTricksUid, base.DataHandlerKey);
			}
			this._enemyTricksUid = new DataUid(8, 10, (ulong)((long)currEnemy.GetId()), 28U);
			GameDataBridge.AddPostDataModificationHandler(this._enemyTricksUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnEnemyTricksChanged));
		}

		// Token: 0x04000EE3 RID: 3811
		private DataUid _enemyTricksUid;

		// Token: 0x04000EE4 RID: 3812
		private int _enemyLastTrickCount;
	}
}
