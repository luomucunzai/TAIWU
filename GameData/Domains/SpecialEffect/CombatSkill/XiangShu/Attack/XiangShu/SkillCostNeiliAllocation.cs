using System;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.XiangShu
{
	// Token: 0x020002D9 RID: 729
	public class SkillCostNeiliAllocation : CombatSkillEffectBase
	{
		// Token: 0x060032CF RID: 13007 RVA: 0x00220EB5 File Offset: 0x0021F0B5
		protected SkillCostNeiliAllocation()
		{
		}

		// Token: 0x060032D0 RID: 13008 RVA: 0x00220EBF File Offset: 0x0021F0BF
		protected SkillCostNeiliAllocation(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x060032D1 RID: 13009 RVA: 0x00220ECC File Offset: 0x0021F0CC
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x060032D2 RID: 13010 RVA: 0x00220F24 File Offset: 0x0021F124
		public override void OnDisable(DataContext context)
		{
			bool isSrcSkillPerformed = this.IsSrcSkillPerformed;
			if (isSrcSkillPerformed)
			{
				GameDataBridge.RemovePostDataModificationHandler(this._enemyNeiliAllocationUid, base.DataHandlerKey);
			}
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x060032D3 RID: 13011 RVA: 0x00220F98 File Offset: 0x0021F198
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = !this.IsSrcSkillPerformed;
				if (flag2)
				{
					bool flag3 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag3)
					{
						this.IsSrcSkillPerformed = true;
						base.AddMaxEffectCount(true);
						base.AppendAffectedAllEnemyData(context, 231, EDataModifyType.Custom, -1);
						this.UpdateEnemyUid(context, true);
						base.ShowSpecialEffectTips(0);
					}
					else
					{
						base.RemoveSelf(context);
					}
				}
				else
				{
					bool flag4 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag4)
					{
						base.RemoveSelf(context);
					}
				}
			}
		}

		// Token: 0x060032D4 RID: 13012 RVA: 0x00221038 File Offset: 0x0021F238
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = isAlly == base.CombatChar.IsAlly || !this.IsSrcSkillPerformed || DomainManager.Combat.GetElement_CombatCharacterDict(charId).GetAutoCastingSkill();
			if (!flag)
			{
				base.ReduceEffectCount(1);
			}
		}

		// Token: 0x060032D5 RID: 13013 RVA: 0x00221080 File Offset: 0x0021F280
		private void OnCombatCharChanged(DataContext context, bool isAlly)
		{
			bool flag = !this.IsSrcSkillPerformed || isAlly == base.CombatChar.IsAlly;
			if (!flag)
			{
				this.UpdateEnemyUid(context, false);
			}
		}

		// Token: 0x060032D6 RID: 13014 RVA: 0x002210B8 File Offset: 0x0021F2B8
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x060032D7 RID: 13015 RVA: 0x00221106 File Offset: 0x0021F306
		private void OnEnemyNeiliAllocationChanged(DataContext context, DataUid dataUid)
		{
			DomainManager.Combat.UpdateSkillCanUse(context, DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false));
		}

		// Token: 0x060032D8 RID: 13016 RVA: 0x00221130 File Offset: 0x0021F330
		private void UpdateEnemyUid(DataContext context, bool init)
		{
			CombatCharacter currEnemy = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			bool flag = !init;
			if (flag)
			{
				GameDataBridge.RemovePostDataModificationHandler(this._enemyNeiliAllocationUid, base.DataHandlerKey);
			}
			this._enemyNeiliAllocationUid = new DataUid(8, 10, (ulong)((long)currEnemy.GetId()), 3U);
			GameDataBridge.AddPostDataModificationHandler(this._enemyNeiliAllocationUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnEnemyNeiliAllocationChanged));
			DomainManager.Combat.UpdateSkillCanUse(context, currEnemy);
		}

		// Token: 0x060032D9 RID: 13017 RVA: 0x002211B4 File Offset: 0x0021F3B4
		public override ValueTuple<sbyte, sbyte> GetModifiedValue(AffectedDataKey dataKey, ValueTuple<sbyte, sbyte> dataValue)
		{
			CombatSkillItem skillConfig = Config.CombatSkill.Instance[dataKey.CombatSkillId];
			dataValue.Item1 = skillConfig.EquipType - 1;
			dataValue.Item2 += this.CostNeiliAllocationPerGrade * (skillConfig.Grade + 1);
			return dataValue;
		}

		// Token: 0x04000F08 RID: 3848
		protected sbyte CostNeiliAllocationPerGrade;

		// Token: 0x04000F09 RID: 3849
		private DataUid _enemyNeiliAllocationUid;
	}
}
