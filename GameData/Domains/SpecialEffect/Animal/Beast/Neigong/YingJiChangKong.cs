using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Neigong
{
	// Token: 0x02000622 RID: 1570
	public class YingJiChangKong : AnimalEffectBase
	{
		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x060045CD RID: 17869 RVA: 0x0027377D File Offset: 0x0027197D
		private int AddAttackRange
		{
			get
			{
				return base.IsElite ? 40 : 20;
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x060045CE RID: 17870 RVA: 0x0027378D File Offset: 0x0027198D
		private CValuePercent AddCriticalOddsPercent
		{
			get
			{
				return base.IsElite ? 200 : 100;
			}
		}

		// Token: 0x060045CF RID: 17871 RVA: 0x002737A5 File Offset: 0x002719A5
		public YingJiChangKong()
		{
		}

		// Token: 0x060045D0 RID: 17872 RVA: 0x002737AF File Offset: 0x002719AF
		public YingJiChangKong(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x060045D1 RID: 17873 RVA: 0x002737BC File Offset: 0x002719BC
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 145, -1, -1, -1, -1), EDataModifyType.Add);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 146, -1, -1, -1, -1), EDataModifyType.Add);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 254, -1, -1, -1, -1), EDataModifyType.AddPercent);
			this._hazardUid = new DataUid(8, 10, (ulong)((long)base.CharacterId), 81U);
			GameDataBridge.AddPostDataModificationHandler(this._hazardUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnHazardChanged));
		}

		// Token: 0x060045D2 RID: 17874 RVA: 0x0027386D File Offset: 0x00271A6D
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._hazardUid, base.DataHandlerKey);
		}

		// Token: 0x060045D3 RID: 17875 RVA: 0x00273884 File Offset: 0x00271A84
		private void OnHazardChanged(DataContext context, DataUid arg2)
		{
			int addAttackRange = this.AddAttackRange * base.CombatChar.AiController.GetHazardPercent();
			bool flag = addAttackRange == this._addedAttackRange;
			if (!flag)
			{
				this._addedAttackRange = addAttackRange;
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 145);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 146);
			}
		}

		// Token: 0x060045D4 RID: 17876 RVA: 0x002738F4 File Offset: 0x00271AF4
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
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId - 145 <= 1;
				bool flag3 = flag2;
				if (flag3)
				{
					result = this._addedAttackRange;
				}
				else
				{
					bool flag4 = dataKey.FieldId == 254;
					if (flag4)
					{
						base.ShowSpecialEffectTipsOnceInFrame(0);
						result = (int)DomainManager.Combat.GetCurrentDistance() * this.AddCriticalOddsPercent;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04001498 RID: 5272
		private DataUid _hazardUid;

		// Token: 0x04001499 RID: 5273
		private int _addedAttackRange;
	}
}
