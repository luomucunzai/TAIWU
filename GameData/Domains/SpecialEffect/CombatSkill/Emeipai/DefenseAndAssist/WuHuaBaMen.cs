using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.DefenseAndAssist
{
	// Token: 0x02000565 RID: 1381
	public class WuHuaBaMen : AssistSkillBase
	{
		// Token: 0x060040CA RID: 16586 RVA: 0x002600CA File Offset: 0x0025E2CA
		public WuHuaBaMen()
		{
		}

		// Token: 0x060040CB RID: 16587 RVA: 0x002600E3 File Offset: 0x0025E2E3
		public WuHuaBaMen(CombatSkillKey skillKey) : base(skillKey, 2702)
		{
		}

		// Token: 0x060040CC RID: 16588 RVA: 0x00260104 File Offset: 0x0025E304
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			for (sbyte hitType = 0; hitType < 4; hitType += 1)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, (ushort)((base.IsDirect ? 32 : 38) + hitType), -1, -1, -1, -1), EDataModifyType.AddPercent);
			}
			this._tricksUid = new DataUid(8, 10, (ulong)((long)base.CharacterId), 28U);
			this._weaponUid = new DataUid(8, 10, (ulong)((long)base.CharacterId), 16U);
			GameDataBridge.AddPostDataModificationHandler(this._tricksUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.UpdateCanAffect));
			GameDataBridge.AddPostDataModificationHandler(this._weaponUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.UpdateCanAffect));
		}

		// Token: 0x060040CD RID: 16589 RVA: 0x002601CD File Offset: 0x0025E3CD
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			GameDataBridge.RemovePostDataModificationHandler(this._tricksUid, base.DataHandlerKey);
			GameDataBridge.RemovePostDataModificationHandler(this._weaponUid, base.DataHandlerKey);
		}

		// Token: 0x060040CE RID: 16590 RVA: 0x002601FC File Offset: 0x0025E3FC
		protected override void OnCanUseChanged(DataContext context, DataUid dataUid)
		{
			this.UpdateCanAffect(context, default(DataUid));
		}

		// Token: 0x060040CF RID: 16591 RVA: 0x0026021C File Offset: 0x0025E41C
		private void UpdateCanAffect(DataContext context, DataUid dataUid)
		{
			bool canAffect = base.CanAffect;
			bool flag = canAffect;
			if (flag)
			{
				IReadOnlyDictionary<int, sbyte> trickDict = base.CombatChar.GetTricks().Tricks;
				bool flag2 = trickDict.Count >= (int)this.TrickTypeCount;
				if (flag2)
				{
					List<sbyte> typeList = ObjectPool<List<sbyte>>.Instance.Get();
					typeList.Clear();
					foreach (sbyte type in trickDict.Values)
					{
						bool flag3 = !typeList.Contains(type) && base.CombatChar.IsTrickUsable(type);
						if (flag3)
						{
							typeList.Add(type);
						}
					}
					canAffect = (typeList.Count >= (int)this.TrickTypeCount);
					ObjectPool<List<sbyte>>.Instance.Return(typeList);
				}
				else
				{
					canAffect = false;
				}
			}
			bool flag4 = this._affecting != canAffect;
			if (flag4)
			{
				this._affecting = canAffect;
				base.SetConstAffecting(context, this._affecting);
				for (sbyte hitType = 0; hitType < 3; hitType += 1)
				{
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, (ushort)((base.IsDirect ? 32 : 38) + hitType));
				}
			}
		}

		// Token: 0x060040D0 RID: 16592 RVA: 0x00260374 File Offset: 0x0025E574
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect || !this._affecting;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = (int)this.AddProperty;
			}
			return result;
		}

		// Token: 0x0400130B RID: 4875
		private sbyte TrickTypeCount = 3;

		// Token: 0x0400130C RID: 4876
		private sbyte AddProperty = 15;

		// Token: 0x0400130D RID: 4877
		private DataUid _tricksUid;

		// Token: 0x0400130E RID: 4878
		private DataUid _weaponUid;

		// Token: 0x0400130F RID: 4879
		private bool _affecting;
	}
}
