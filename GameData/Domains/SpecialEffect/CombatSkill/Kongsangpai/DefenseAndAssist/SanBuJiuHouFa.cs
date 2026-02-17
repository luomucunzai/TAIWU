using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.DefenseAndAssist
{
	// Token: 0x0200049C RID: 1180
	public class SanBuJiuHouFa : AssistSkillBase
	{
		// Token: 0x06003C59 RID: 15449 RVA: 0x0024D1DA File Offset: 0x0024B3DA
		public SanBuJiuHouFa()
		{
		}

		// Token: 0x06003C5A RID: 15450 RVA: 0x0024D1E4 File Offset: 0x0024B3E4
		public SanBuJiuHouFa(CombatSkillKey skillKey) : base(skillKey, 10701)
		{
		}

		// Token: 0x06003C5B RID: 15451 RVA: 0x0024D1F4 File Offset: 0x0024B3F4
		public override void OnEnable(DataContext context)
		{
			this._currAddValue = this.CalcAddValue();
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 32, -1, -1, -1, -1), EDataModifyType.AddPercent);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 33, -1, -1, -1, -1), EDataModifyType.AddPercent);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 34, -1, -1, -1, -1), EDataModifyType.AddPercent);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 35, -1, -1, -1, -1), EDataModifyType.AddPercent);
			}
			else
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 38, -1, -1, -1, -1), EDataModifyType.AddPercent);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 39, -1, -1, -1, -1), EDataModifyType.AddPercent);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 40, -1, -1, -1, -1), EDataModifyType.AddPercent);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 41, -1, -1, -1, -1), EDataModifyType.AddPercent);
			}
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x06003C5C RID: 15452 RVA: 0x0024D32B File Offset: 0x0024B52B
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x06003C5D RID: 15453 RVA: 0x0024D340 File Offset: 0x0024B540
		private int CalcAddValue()
		{
			short currDistance = DomainManager.Combat.GetCurrentDistance();
			bool flag = currDistance >= 50;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int affectUnit = (int)((50 - currDistance) / 10);
				result = 5 + affectUnit * 5;
			}
			return result;
		}

		// Token: 0x06003C5E RID: 15454 RVA: 0x0024D37C File Offset: 0x0024B57C
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			int addValue = this.CalcAddValue();
			bool flag = this._currAddValue != addValue;
			if (flag)
			{
				this._currAddValue = addValue;
				this.InvalidateCache(context);
				base.SetConstAffecting(context, this._currAddValue > 0);
			}
		}

		// Token: 0x06003C5F RID: 15455 RVA: 0x0024D3C3 File Offset: 0x0024B5C3
		protected override void OnCanUseChanged(DataContext context, DataUid dataUid)
		{
			this.InvalidateCache(context);
		}

		// Token: 0x06003C60 RID: 15456 RVA: 0x0024D3D0 File Offset: 0x0024B5D0
		private void InvalidateCache(DataContext context)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 32);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 33);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 34);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 35);
			}
			else
			{
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 38);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 39);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 40);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 41);
			}
		}

		// Token: 0x06003C61 RID: 15457 RVA: 0x0024D490 File Offset: 0x0024B690
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			return (!base.CanAffect) ? 0 : this._currAddValue;
		}

		// Token: 0x040011C2 RID: 4546
		private const sbyte MaxAffectDistance = 50;

		// Token: 0x040011C3 RID: 4547
		private const int DistancePerUnit = 10;

		// Token: 0x040011C4 RID: 4548
		private const int AddPercentValueBase = 5;

		// Token: 0x040011C5 RID: 4549
		private const int AddPercentValuePerUnit = 5;

		// Token: 0x040011C6 RID: 4550
		private int _currAddValue;
	}
}
