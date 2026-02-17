using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.DefenseAndAssist
{
	// Token: 0x020003DC RID: 988
	public class YinYangZhouTianFa : AssistSkillBase
	{
		// Token: 0x060037DD RID: 14301 RVA: 0x00237A34 File Offset: 0x00235C34
		public YinYangZhouTianFa()
		{
		}

		// Token: 0x060037DE RID: 14302 RVA: 0x00237A49 File Offset: 0x00235C49
		public YinYangZhouTianFa(CombatSkillKey skillKey) : base(skillKey, 4603)
		{
		}

		// Token: 0x060037DF RID: 14303 RVA: 0x00237A64 File Offset: 0x00235C64
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			this._dataUids.Add(base.ParseCharDataUid(21));
			foreach (int enemyCharId in DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly))
			{
				bool flag = enemyCharId >= 0;
				if (flag)
				{
					this._dataUids.Add(base.ParseCharDataUid(enemyCharId, 21));
				}
			}
			foreach (DataUid dataUid in this._dataUids)
			{
				GameDataBridge.AddPostDataModificationHandler(dataUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnQiDisorderChanged));
			}
		}

		// Token: 0x060037E0 RID: 14304 RVA: 0x00237B54 File Offset: 0x00235D54
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			foreach (DataUid dataUid in this._dataUids)
			{
				GameDataBridge.RemovePostDataModificationHandler(dataUid, base.DataHandlerKey);
			}
		}

		// Token: 0x060037E1 RID: 14305 RVA: 0x00237BD4 File Offset: 0x00235DD4
		public override void OnDataAdded(DataContext context)
		{
			base.AppendAffectedAllEnemyData(context, 203, EDataModifyType.Add, -1);
		}

		// Token: 0x060037E2 RID: 14306 RVA: 0x00237BE6 File Offset: 0x00235DE6
		private void OnCombatCharChanged(DataContext context, bool isAlly)
		{
			this.UpdateAffected(context);
		}

		// Token: 0x060037E3 RID: 14307 RVA: 0x00237BF1 File Offset: 0x00235DF1
		private void OnQiDisorderChanged(DataContext context, DataUid uid)
		{
			this.UpdateAffected(context);
		}

		// Token: 0x060037E4 RID: 14308 RVA: 0x00237BFC File Offset: 0x00235DFC
		private void UpdateAffected(DataContext context)
		{
			bool affected = base.IsCurrent && this.CharObj.GetDisorderOfQi() < base.CurrEnemyChar.GetCharacter().GetDisorderOfQi();
			bool flag = affected == this._affected;
			if (!flag)
			{
				this._affected = affected;
				base.SetConstAffecting(context, this._affected);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CurrEnemyChar.GetId(), 203);
				bool affected2 = this._affected;
				if (affected2)
				{
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x060037E5 RID: 14309 RVA: 0x00237C88 File Offset: 0x00235E88
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || power < 100 || !base.CanAffect;
			if (!flag)
			{
				CombatSkillItem skillConfig = Config.CombatSkill.Instance[skillId];
				bool flag2 = skillConfig.EquipType != 1;
				if (!flag2)
				{
					DomainManager.Combat.ChangeDisorderOfQiRandomRecovery(context, base.IsDirect ? base.CombatChar : base.CurrEnemyChar, (int)(50 * skillConfig.GridCost * (base.IsDirect ? -1 : 1)), false);
					base.ShowSpecialEffectTips(0);
					base.ShowEffectTips(context);
				}
			}
		}

		// Token: 0x060037E6 RID: 14310 RVA: 0x00237D20 File Offset: 0x00235F20
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !base.CanAffect || dataKey.FieldId != 203;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				GameData.Domains.Character.Character enemyChar;
				bool flag2 = !DomainManager.Character.TryGetElement_Objects(dataKey.CharId, out enemyChar);
				if (flag2)
				{
					result = 0;
				}
				else
				{
					bool flag3 = this.CharObj.GetDisorderOfQi() >= enemyChar.GetDisorderOfQi();
					if (flag3)
					{
						result = 0;
					}
					else
					{
						sbyte currRatio = base.SkillInstance.GetCurrInnerRatio();
						int changeRatio = (int)((base.IsDirect ? currRatio : (100 - currRatio)) * 20 / 100);
						result = (base.IsDirect ? changeRatio : (-changeRatio));
					}
				}
			}
			return result;
		}

		// Token: 0x0400104F RID: 4175
		private const sbyte ChangeQiDisorderUnit = 50;

		// Token: 0x04001050 RID: 4176
		private const int ChangeInnerRatioPercent = 20;

		// Token: 0x04001051 RID: 4177
		private bool _affected;

		// Token: 0x04001052 RID: 4178
		private readonly List<DataUid> _dataUids = new List<DataUid>();
	}
}
