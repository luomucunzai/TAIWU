using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Neigong.Boss
{
	// Token: 0x020002A5 RID: 677
	public class SuXinWuRan : BossNeigongBase
	{
		// Token: 0x060031C3 RID: 12739 RVA: 0x0021C0DB File Offset: 0x0021A2DB
		public SuXinWuRan()
		{
		}

		// Token: 0x060031C4 RID: 12740 RVA: 0x0021C0F0 File Offset: 0x0021A2F0
		public SuXinWuRan(CombatSkillKey skillKey) : base(skillKey, 16112)
		{
		}

		// Token: 0x060031C5 RID: 12741 RVA: 0x0021C10C File Offset: 0x0021A30C
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			foreach (DataUid dataUid in this._enemyMarkUids)
			{
				GameDataBridge.RemovePostDataModificationHandler(dataUid, base.DataHandlerKey);
			}
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060031C6 RID: 12742 RVA: 0x0021C184 File Offset: 0x0021A384
		protected override void ActivePhase2Effect(DataContext context)
		{
			foreach (int enemyCharId in DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly))
			{
				bool flag = enemyCharId < 0;
				if (!flag)
				{
					DataUid markUid = base.ParseCombatCharacterDataUid(enemyCharId, 50);
					this._enemyMarkUids.Add(markUid);
					GameDataBridge.AddPostDataModificationHandler(markUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnMarkChanged));
				}
			}
			base.AppendAffectedAllEnemyData(context, 199, EDataModifyType.AddPercent, -1);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060031C7 RID: 12743 RVA: 0x0021C224 File Offset: 0x0021A424
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || power < 100 || Config.CombatSkill.Instance[skillId].EquipType != 1;
			if (!flag)
			{
				bool flag2 = base.CurrEnemyChar.GetDefeatMarkCollection().DieMarkList.Count == (int)(SharedConstValue.DefeatNeedDieMarkCount - 1) && !DomainManager.Combat.CheckHealthImmunity(context, base.CurrEnemyChar);
				if (flag2)
				{
					base.CurrEnemyChar.GetCharacter().SetHealth(0, context);
				}
				DomainManager.Combat.AppendDieDefeatMark(context, base.CurrEnemyChar, this.SkillKey, 1);
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x060031C8 RID: 12744 RVA: 0x0021C2CE File Offset: 0x0021A4CE
		private void OnMarkChanged(DataContext context, DataUid dataUid)
		{
			DomainManager.SpecialEffect.InvalidateCache(context, (int)dataUid.SubId0, 199);
		}

		// Token: 0x060031C9 RID: 12745 RVA: 0x0021C2EC File Offset: 0x0021A4EC
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.FieldId == 199;
			int result;
			if (flag)
			{
				DefeatMarkCollection markCollection = DomainManager.Combat.GetElement_CombatCharacterDict(dataKey.CharId).GetDefeatMarkCollection();
				int dieMarkCount = 0;
				for (int i = 0; i < markCollection.DieMarkList.Count; i++)
				{
					bool flag2 = markCollection.DieMarkList[i].Equals(this.SkillKey);
					if (flag2)
					{
						dieMarkCount++;
					}
				}
				result = 20 * dieMarkCount;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x04000EC0 RID: 3776
		private const sbyte AddPowerUnit = 20;

		// Token: 0x04000EC1 RID: 3777
		private readonly List<DataUid> _enemyMarkUids = new List<DataUid>();
	}
}
