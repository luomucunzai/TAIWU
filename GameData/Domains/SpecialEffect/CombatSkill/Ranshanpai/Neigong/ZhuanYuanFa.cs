using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Neigong
{
	// Token: 0x0200045A RID: 1114
	public class ZhuanYuanFa : CombatSkillEffectBase
	{
		// Token: 0x06003AB9 RID: 15033 RVA: 0x00244D88 File Offset: 0x00242F88
		public ZhuanYuanFa()
		{
		}

		// Token: 0x06003ABA RID: 15034 RVA: 0x00244D92 File Offset: 0x00242F92
		public ZhuanYuanFa(CombatSkillKey skillKey) : base(skillKey, 7001, -1)
		{
		}

		// Token: 0x06003ABB RID: 15035 RVA: 0x00244DA4 File Offset: 0x00242FA4
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(199, EDataModifyType.Add, -1);
			this.UpdateAddPower(context, default(DataUid));
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
		}

		// Token: 0x06003ABC RID: 15036 RVA: 0x00244DF5 File Offset: 0x00242FF5
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
		}

		// Token: 0x06003ABD RID: 15037 RVA: 0x00244E1C File Offset: 0x0024301C
		private void OnCombatBegin(DataContext context)
		{
			bool flag = !DomainManager.Combat.IsCharInCombat(base.CharacterId, true);
			if (!flag)
			{
				this._selfBehaviorUid = new DataUid(4, 0, (ulong)((long)base.CharacterId), 78U);
				this._enemyBehaviorUid = new DataUid(4, 0, (ulong)((long)base.CurrEnemyChar.GetId()), 78U);
				GameDataBridge.AddPostDataModificationHandler(this._selfBehaviorUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.UpdateAddPower));
				GameDataBridge.AddPostDataModificationHandler(this._enemyBehaviorUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.UpdateAddPower));
				this.UpdateAddPower(context, default(DataUid));
				Events.RegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			}
		}

		// Token: 0x06003ABE RID: 15038 RVA: 0x00244ED8 File Offset: 0x002430D8
		private void OnCombatSettlement(DataContext context, sbyte combatStatus)
		{
			bool flag = !DomainManager.Combat.IsCharInCombat(base.CharacterId, false);
			if (!flag)
			{
				GameDataBridge.RemovePostDataModificationHandler(this._selfBehaviorUid, base.DataHandlerKey);
				GameDataBridge.RemovePostDataModificationHandler(this._enemyBehaviorUid, base.DataHandlerKey);
				this.UpdateAddPower(context, default(DataUid));
				Events.UnRegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			}
		}

		// Token: 0x06003ABF RID: 15039 RVA: 0x00244F48 File Offset: 0x00243148
		private void OnCombatCharChanged(DataContext context, bool isAlly)
		{
			bool flag = base.CombatChar.IsAlly == isAlly;
			if (!flag)
			{
				GameDataBridge.RemovePostDataModificationHandler(this._enemyBehaviorUid, base.DataHandlerKey);
				this._enemyBehaviorUid = new DataUid(4, 0, (ulong)((long)base.CurrEnemyChar.GetId()), 78U);
				GameDataBridge.AddPostDataModificationHandler(this._enemyBehaviorUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.UpdateAddPower));
				this.UpdateAddPower(context, default(DataUid));
			}
		}

		// Token: 0x06003AC0 RID: 15040 RVA: 0x00244FC8 File Offset: 0x002431C8
		private void UpdateAddPower(DataContext context, DataUid dataUid = default(DataUid))
		{
			sbyte selfBehaviorType = this.CharObj.GetBehaviorType();
			this._currAddPower = 0;
			bool flag = selfBehaviorType == 2;
			if (flag)
			{
				this._currAddPower += 10;
			}
			else
			{
				bool flag2 = DomainManager.Combat.IsInCombat();
				if (flag2)
				{
					sbyte enemyBehaviorType = base.CurrEnemyChar.GetCharacter().GetBehaviorType();
					bool sameBehavior = Math.Sign((int)(selfBehaviorType - 2)) == Math.Sign((int)(enemyBehaviorType - 2));
					bool flag3 = enemyBehaviorType != 2 && base.IsDirect == sameBehavior;
					if (flag3)
					{
						this._currAddPower += 10;
					}
				}
			}
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
		}

		// Token: 0x06003AC1 RID: 15041 RVA: 0x00245078 File Offset: 0x00243278
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 199;
				if (flag2)
				{
					result = (int)this._currAddPower;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x0400112E RID: 4398
		private const sbyte EvenAddPower = 10;

		// Token: 0x0400112F RID: 4399
		private const sbyte EnemyAddPower = 10;

		// Token: 0x04001130 RID: 4400
		private sbyte _currAddPower;

		// Token: 0x04001131 RID: 4401
		private DataUid _selfBehaviorUid;

		// Token: 0x04001132 RID: 4402
		private DataUid _enemyBehaviorUid;
	}
}
