using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.DefenseAndAssist
{
	// Token: 0x0200049F RID: 1183
	public class WuHuangBiDuShu : DefenseSkillBase
	{
		// Token: 0x06003C6F RID: 15471 RVA: 0x0024D7C7 File Offset: 0x0024B9C7
		public WuHuangBiDuShu()
		{
		}

		// Token: 0x06003C70 RID: 15472 RVA: 0x0024D7DC File Offset: 0x0024B9DC
		public WuHuangBiDuShu(CombatSkillKey skillKey) : base(skillKey, 10603)
		{
		}

		// Token: 0x06003C71 RID: 15473 RVA: 0x0024D7F8 File Offset: 0x0024B9F8
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 159, -1, -1, -1, -1), EDataModifyType.Custom);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 106 : 247, -1, -1, -1, -1), EDataModifyType.Custom);
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x06003C72 RID: 15474 RVA: 0x0024D870 File Offset: 0x0024BA70
		private void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			foreach (KeyValuePair<sbyte, int> poison in this._changePoisonDict)
			{
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					DomainManager.Combat.ReducePoison(context, this._poisonChar, poison.Key, poison.Value, true, false);
				}
				else
				{
					DomainManager.Combat.AddPoison(context, this._poisonChar, this._poisonChar, poison.Key, (sbyte)this._poisonChar.GetDefeatMarkCollection().PoisonMarkList[(int)poison.Key], poison.Value, -1, true, true, default(ItemKey), false, false, false);
				}
			}
			this._changePoisonDict.Clear();
			base.ShowSpecialEffectTips(1);
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
		}

		// Token: 0x06003C73 RID: 15475 RVA: 0x0024D96C File Offset: 0x0024BB6C
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = base.CharacterId != dataKey.CharId || !base.CanAffect;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 159;
				result = (!flag2 && dataValue);
			}
			return result;
		}

		// Token: 0x06003C74 RID: 15476 RVA: 0x0024D9B8 File Offset: 0x0024BBB8
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = base.CharacterId != dataKey.CharId || !base.CanAffect;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 106 && dataValue > 0;
				if (flag2)
				{
					sbyte poisonType = (sbyte)dataKey.CustomParam0;
					this._poisonChar = (base.IsDirect ? base.CombatChar : base.CurrEnemyChar);
					bool flag3 = this._changePoisonDict.Count == 0;
					if (flag3)
					{
						Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
					}
					bool flag4 = !this._changePoisonDict.ContainsKey(poisonType);
					if (flag4)
					{
						this._changePoisonDict.Add(poisonType, dataValue);
					}
					else
					{
						Dictionary<sbyte, int> changePoisonDict = this._changePoisonDict;
						sbyte key = poisonType;
						changePoisonDict[key] += dataValue;
					}
					result = 0;
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x06003C75 RID: 15477 RVA: 0x0024DA9C File Offset: 0x0024BC9C
		public override CombatCharacter GetModifiedValue(AffectedDataKey dataKey, CombatCharacter dataValue)
		{
			bool flag = dataKey.FieldId == 247 && dataKey.CharId == base.CharacterId;
			CombatCharacter result;
			if (flag)
			{
				base.ShowSpecialEffectTips(1);
				result = base.CurrEnemyChar;
			}
			else
			{
				result = dataValue;
			}
			return result;
		}

		// Token: 0x040011CA RID: 4554
		private CombatCharacter _poisonChar;

		// Token: 0x040011CB RID: 4555
		private readonly Dictionary<sbyte, int> _changePoisonDict = new Dictionary<sbyte, int>();
	}
}
