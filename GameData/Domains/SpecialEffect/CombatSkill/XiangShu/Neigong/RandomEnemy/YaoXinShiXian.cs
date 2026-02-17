using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Neigong.RandomEnemy
{
	// Token: 0x02000298 RID: 664
	public class YaoXinShiXian : MinionBase
	{
		// Token: 0x06003172 RID: 12658 RVA: 0x0021AFA9 File Offset: 0x002191A9
		public YaoXinShiXian()
		{
		}

		// Token: 0x06003173 RID: 12659 RVA: 0x0021AFB3 File Offset: 0x002191B3
		public YaoXinShiXian(CombatSkillKey skillKey) : base(skillKey, 16003)
		{
		}

		// Token: 0x06003174 RID: 12660 RVA: 0x0021AFC4 File Offset: 0x002191C4
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 44, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 45, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
		}

		// Token: 0x06003175 RID: 12661 RVA: 0x0021B03D File Offset: 0x0021923D
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._enemyHappinessUid, base.DataHandlerKey);
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
		}

		// Token: 0x06003176 RID: 12662 RVA: 0x0021B076 File Offset: 0x00219276
		private void OnCombatBegin(DataContext context)
		{
			this.UpdateEnemyDataUid(context);
		}

		// Token: 0x06003177 RID: 12663 RVA: 0x0021B084 File Offset: 0x00219284
		private void OnCombatCharChanged(DataContext context, bool isAlly)
		{
			bool flag = isAlly == base.CombatChar.IsAlly;
			if (!flag)
			{
				GameDataBridge.RemovePostDataModificationHandler(this._enemyHappinessUid, base.DataHandlerKey);
				this.UpdateEnemyDataUid(context);
			}
		}

		// Token: 0x06003178 RID: 12664 RVA: 0x0021B0C0 File Offset: 0x002192C0
		private void UpdateEnemyDataUid(DataContext context)
		{
			this._enemyHappinessUid = base.ParseCombatCharacterDataUid(base.EnemyChar.GetId(), 136);
			GameDataBridge.AddPostDataModificationHandler(this._enemyHappinessUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.UpdateAddPercent));
			this.UpdateAddPercent(context, default(DataUid));
		}

		// Token: 0x06003179 RID: 12665 RVA: 0x0021B11C File Offset: 0x0021931C
		private void UpdateAddPercent(DataContext context, DataUid dataUid)
		{
			sbyte happinessType = HappinessType.GetHappinessType(base.EnemyChar.GetHappiness());
			this._addPercent = (int)YaoXinShiXian.AddPenetrate[Math.Abs((int)(happinessType - 3))];
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 44);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 45);
		}

		// Token: 0x0600317A RID: 12666 RVA: 0x0021B178 File Offset: 0x00219378
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
				result = this._addPercent;
			}
			return result;
		}

		// Token: 0x04000EA9 RID: 3753
		private static readonly sbyte[] AddPenetrate = new sbyte[]
		{
			0,
			25,
			50,
			100
		};

		// Token: 0x04000EAA RID: 3754
		private DataUid _enemyHappinessUid;

		// Token: 0x04000EAB RID: 3755
		private int _addPercent;
	}
}
