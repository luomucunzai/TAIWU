using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.DefenseAndAssist
{
	// Token: 0x02000566 RID: 1382
	public class XinWuDingYi : AssistSkillBase
	{
		// Token: 0x060040D1 RID: 16593 RVA: 0x002603B6 File Offset: 0x0025E5B6
		public XinWuDingYi()
		{
		}

		// Token: 0x060040D2 RID: 16594 RVA: 0x002603C8 File Offset: 0x0025E5C8
		public XinWuDingYi(CombatSkillKey skillKey) : base(skillKey, 2701)
		{
		}

		// Token: 0x060040D3 RID: 16595 RVA: 0x002603E0 File Offset: 0x0025E5E0
		public override void OnEnable(DataContext context)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this._gettingExtraTrick = false;
				Events.RegisterHandler_GetTrick(new Events.OnGetTrick(this.OnGetTrick));
			}
			else
			{
				this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 164, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			}
		}

		// Token: 0x060040D4 RID: 16596 RVA: 0x00260448 File Offset: 0x0025E648
		public override void OnDisable(DataContext context)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				Events.UnRegisterHandler_GetTrick(new Events.OnGetTrick(this.OnGetTrick));
			}
		}

		// Token: 0x060040D5 RID: 16597 RVA: 0x00260474 File Offset: 0x0025E674
		private void OnGetTrick(DataContext context, int charId, bool isAlly, sbyte trickType, bool usable)
		{
			bool flag = base.CharacterId != charId || !usable || !base.CanAffect || this._gettingExtraTrick || !context.Random.CheckPercentProb((int)this.AffectOdds);
			if (!flag)
			{
				this._getTrickType = trickType;
				Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
			}
		}

		// Token: 0x060040D6 RID: 16598 RVA: 0x002604D8 File Offset: 0x0025E6D8
		private void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			this._gettingExtraTrick = true;
			DomainManager.Combat.AddTrick(context, base.CombatChar, this._getTrickType, true);
			this._gettingExtraTrick = false;
			base.ShowSpecialEffectTips(0);
			base.ShowEffectTips(context);
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
		}

		// Token: 0x060040D7 RID: 16599 RVA: 0x00260530 File Offset: 0x0025E730
		public override List<NeedTrick> GetModifiedValue(AffectedDataKey dataKey, List<NeedTrick> dataValue)
		{
			DataContext context = DomainManager.Combat.Context;
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			List<NeedTrick> result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 164;
				if (flag2)
				{
					bool affected = false;
					for (int i = 0; i < dataValue.Count; i++)
					{
						NeedTrick costTrick = dataValue[i];
						bool flag3 = base.CombatChar.IsTrickUseless(costTrick.TrickType);
						if (!flag3)
						{
							byte costCount = costTrick.NeedCount;
							for (int _ = 0; _ < (int)costCount; _++)
							{
								bool flag4 = !context.Random.CheckPercentProb((int)this.AffectOdds);
								if (!flag4)
								{
									costTrick.NeedCount -= 1;
									dataValue[i] = costTrick;
									affected = true;
								}
							}
						}
					}
					bool flag5 = affected;
					if (flag5)
					{
						base.ShowSpecialEffectTips(0);
						base.ShowEffectTips(context);
					}
				}
				result = dataValue;
			}
			return result;
		}

		// Token: 0x04001310 RID: 4880
		private sbyte AffectOdds = 30;

		// Token: 0x04001311 RID: 4881
		private sbyte _getTrickType;

		// Token: 0x04001312 RID: 4882
		private bool _gettingExtraTrick;
	}
}
