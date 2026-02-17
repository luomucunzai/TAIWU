using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.DefenseAndAssist
{
	// Token: 0x02000526 RID: 1318
	public class QianNianZui : AssistSkillBase
	{
		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06003F3E RID: 16190 RVA: 0x002590BD File Offset: 0x002572BD
		private CValuePercent EffectPercent
		{
			get
			{
				return this._eatingWine ? 100 : QianNianZui.NoWineEffectPercent;
			}
		}

		// Token: 0x06003F3F RID: 16191 RVA: 0x002590D5 File Offset: 0x002572D5
		public QianNianZui()
		{
		}

		// Token: 0x06003F40 RID: 16192 RVA: 0x002590DF File Offset: 0x002572DF
		public QianNianZui(CombatSkillKey skillKey) : base(skillKey, 14606)
		{
		}

		// Token: 0x06003F41 RID: 16193 RVA: 0x002590F0 File Offset: 0x002572F0
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._eatingWine = false;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 109 : 76, -1, -1, -1, -1), EDataModifyType.Add);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 107 : 74, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			this._eatingItemsUid = new DataUid(4, 0, (ulong)((long)base.CharacterId), 59U);
			GameDataBridge.AddPostDataModificationHandler(this._eatingItemsUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.UpdateCanAffect));
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x06003F42 RID: 16194 RVA: 0x002591B2 File Offset: 0x002573B2
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			GameDataBridge.RemovePostDataModificationHandler(this._eatingItemsUid, base.DataHandlerKey);
		}

		// Token: 0x06003F43 RID: 16195 RVA: 0x002591D0 File Offset: 0x002573D0
		private void OnCombatBegin(DataContext context)
		{
			this.UpdateCanAffect(context, default(DataUid));
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x06003F44 RID: 16196 RVA: 0x00259204 File Offset: 0x00257404
		protected override void OnCanUseChanged(DataContext context, DataUid dataUid)
		{
			this.UpdateCanAffect(context, default(DataUid));
		}

		// Token: 0x06003F45 RID: 16197 RVA: 0x00259223 File Offset: 0x00257423
		private unsafe void UpdateCanAffect(DataContext context, DataUid dataUid)
		{
			this._eatingWine = (*this.CharObj.GetEatingItems()).ContainsWine();
			base.SetConstAffecting(context, base.CanAffect);
		}

		// Token: 0x06003F46 RID: 16198 RVA: 0x00259250 File Offset: 0x00257450
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == (base.IsDirect ? 109 : 76);
				if (flag2)
				{
					result = (int)(QianNianZui.ChangePursueOdds[dataKey.CustomParam0] * (base.IsDirect ? -1 : 1)) * this.EffectPercent;
				}
				else
				{
					bool flag3 = dataKey.FieldId == (base.IsDirect ? 107 : 74);
					if (flag3)
					{
						result = (base.IsDirect ? -50 : 50) * this.EffectPercent;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x040012A3 RID: 4771
		private static readonly sbyte[] ChangePursueOdds = new sbyte[]
		{
			40,
			30,
			20,
			10,
			10
		};

		// Token: 0x040012A4 RID: 4772
		private const sbyte ChangeHitOdds = 50;

		// Token: 0x040012A5 RID: 4773
		private static readonly CValuePercent NoWineEffectPercent = 50;

		// Token: 0x040012A6 RID: 4774
		private DataUid _eatingItemsUid;

		// Token: 0x040012A7 RID: 4775
		private bool _eatingWine;
	}
}
