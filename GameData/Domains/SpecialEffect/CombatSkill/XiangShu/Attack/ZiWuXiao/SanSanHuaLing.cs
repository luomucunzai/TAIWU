using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.ZiWuXiao
{
	// Token: 0x020002BC RID: 700
	public class SanSanHuaLing : CombatSkillEffectBase
	{
		// Token: 0x0600324A RID: 12874 RVA: 0x0021EE6D File Offset: 0x0021D06D
		public SanSanHuaLing()
		{
		}

		// Token: 0x0600324B RID: 12875 RVA: 0x0021EE77 File Offset: 0x0021D077
		public SanSanHuaLing(CombatSkillKey skillKey) : base(skillKey, 17110, -1)
		{
		}

		// Token: 0x0600324C RID: 12876 RVA: 0x0021EE88 File Offset: 0x0021D088
		public override void OnEnable(DataContext context)
		{
			this._selfLastNeiliAllocation = base.CombatChar.GetNeiliAllocation();
			this._selfNeiliAllocationUid = new DataUid(8, 10, (ulong)((long)base.CharacterId), 3U);
			GameDataBridge.AddPostDataModificationHandler(this._selfNeiliAllocationUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnSelfNeiliAllocationChanged));
			this.UpdateEnemyUid(true);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x0600324D RID: 12877 RVA: 0x0021EF1C File Offset: 0x0021D11C
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._selfNeiliAllocationUid, base.DataHandlerKey);
			GameDataBridge.RemovePostDataModificationHandler(this._enemyNeiliAllocationUid, base.DataHandlerKey);
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x0600324E RID: 12878 RVA: 0x0021EF84 File Offset: 0x0021D184
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

		// Token: 0x0600324F RID: 12879 RVA: 0x0021EFE0 File Offset: 0x0021D1E0
		private void OnCombatCharChanged(DataContext context, bool isAlly)
		{
			bool flag = isAlly == base.CombatChar.IsAlly;
			if (!flag)
			{
				this.UpdateEnemyUid(false);
			}
		}

		// Token: 0x06003250 RID: 12880 RVA: 0x0021F00C File Offset: 0x0021D20C
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003251 RID: 12881 RVA: 0x0021F05C File Offset: 0x0021D25C
		private unsafe void OnSelfNeiliAllocationChanged(DataContext context, DataUid dataUid)
		{
			NeiliAllocation newNeiliAllocation = base.CombatChar.GetNeiliAllocation();
			for (byte type = 0; type < 4; type += 1)
			{
				bool flag = *(ref newNeiliAllocation.Items.FixedElementField + (IntPtr)type * 2) < *(ref this._selfLastNeiliAllocation.Items.FixedElementField + (IntPtr)type * 2);
				if (flag)
				{
					base.ReduceEffectCount(1);
					break;
				}
			}
			this._selfLastNeiliAllocation = newNeiliAllocation;
		}

		// Token: 0x06003252 RID: 12882 RVA: 0x0021F0CC File Offset: 0x0021D2CC
		private unsafe void OnEnemyNeiliAllocationChanged(DataContext context, DataUid dataUid)
		{
			CombatCharacter currEnemy = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			NeiliAllocation newNeiliAllocation = currEnemy.GetNeiliAllocation();
			bool affected = false;
			for (byte type = 0; type < 4; type += 1)
			{
				int changeValue = Math.Abs((int)(*(ref newNeiliAllocation.Items.FixedElementField + (IntPtr)type * 2) - *(ref this._enemyLastNeiliAllocation.Items.FixedElementField + (IntPtr)type * 2)));
				bool flag = changeValue == 0;
				if (!flag)
				{
					base.CombatChar.ChangeNeiliAllocation(context, type, changeValue * 2, true, true);
					affected = true;
				}
			}
			bool flag2 = affected;
			if (flag2)
			{
				base.ShowSpecialEffectTips(0);
			}
			this._enemyLastNeiliAllocation = newNeiliAllocation;
		}

		// Token: 0x06003253 RID: 12883 RVA: 0x0021F17C File Offset: 0x0021D37C
		private void UpdateEnemyUid(bool init)
		{
			CombatCharacter currEnemy = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			this._enemyLastNeiliAllocation = currEnemy.GetNeiliAllocation();
			bool flag = !init;
			if (flag)
			{
				GameDataBridge.RemovePostDataModificationHandler(this._enemyNeiliAllocationUid, base.DataHandlerKey);
			}
			this._enemyNeiliAllocationUid = new DataUid(8, 10, (ulong)((long)currEnemy.GetId()), 3U);
			GameDataBridge.AddPostDataModificationHandler(this._enemyNeiliAllocationUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnEnemyNeiliAllocationChanged));
		}

		// Token: 0x04000EE5 RID: 3813
		private DataUid _selfNeiliAllocationUid;

		// Token: 0x04000EE6 RID: 3814
		private NeiliAllocation _selfLastNeiliAllocation;

		// Token: 0x04000EE7 RID: 3815
		private DataUid _enemyNeiliAllocationUid;

		// Token: 0x04000EE8 RID: 3816
		private NeiliAllocation _enemyLastNeiliAllocation;
	}
}
