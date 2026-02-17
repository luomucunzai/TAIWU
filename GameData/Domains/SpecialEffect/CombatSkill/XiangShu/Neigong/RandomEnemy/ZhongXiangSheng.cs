using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Neigong.RandomEnemy
{
	// Token: 0x02000299 RID: 665
	public class ZhongXiangSheng : MinionBase
	{
		// Token: 0x0600317C RID: 12668 RVA: 0x0021B1C9 File Offset: 0x002193C9
		public ZhongXiangSheng()
		{
		}

		// Token: 0x0600317D RID: 12669 RVA: 0x0021B1D3 File Offset: 0x002193D3
		public ZhongXiangSheng(CombatSkillKey skillKey) : base(skillKey, 16006)
		{
		}

		// Token: 0x0600317E RID: 12670 RVA: 0x0021B1E3 File Offset: 0x002193E3
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
		}

		// Token: 0x0600317F RID: 12671 RVA: 0x0021B20A File Offset: 0x0021940A
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			GameDataBridge.RemovePostDataModificationHandler(this._moralityUid, base.DataHandlerKey);
		}

		// Token: 0x06003180 RID: 12672 RVA: 0x0021B243 File Offset: 0x00219443
		private void OnCombatBegin(DataContext context)
		{
			this._isCurrCombatChar = DomainManager.Combat.IsCurrentCombatCharacter(base.CombatChar);
			this.UpdateEnemyDataUid(context);
		}

		// Token: 0x06003181 RID: 12673 RVA: 0x0021B264 File Offset: 0x00219464
		private void OnCombatCharChanged(DataContext context, bool isAlly)
		{
			bool flag = isAlly == base.CombatChar.IsAlly;
			if (flag)
			{
				bool isCurrChar = DomainManager.Combat.IsCurrentCombatCharacter(base.CombatChar);
				bool flag2 = this._isCurrCombatChar == isCurrChar;
				if (!flag2)
				{
					this._isCurrCombatChar = isCurrChar;
					this.InvalidateAffectDataCache(context);
				}
			}
			else
			{
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					GameDataBridge.RemovePostDataModificationHandler(this._moralityUid, base.DataHandlerKey);
					this.UpdateEnemyDataUid(context);
				}
			}
		}

		// Token: 0x06003182 RID: 12674 RVA: 0x0021B2E0 File Offset: 0x002194E0
		private void UpdateAffected(DataContext context, DataUid dataUid)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			int enemyCharId = enemyChar.GetId();
			sbyte behaviorType = enemyChar.GetCharacter().GetBehaviorType();
			sbyte specifyParam;
			this._activatedEffectParam = (ZhongXiangSheng.BehaviorSpecifyParam.TryGetValue(behaviorType, out specifyParam) ? specifyParam : -80);
			ushort[] fieldIds = ZhongXiangSheng.BehaviorType2FieldIds[behaviorType];
			base.ClearAffectedData(context);
			for (int i = 0; i < fieldIds.Length; i++)
			{
				base.AppendAffectedData(context, enemyCharId, fieldIds[i], EDataModifyType.TotalPercent, -1);
			}
		}

		// Token: 0x06003183 RID: 12675 RVA: 0x0021B374 File Offset: 0x00219574
		private void UpdateEnemyDataUid(DataContext context)
		{
			this._moralityUid = base.ParseCharDataUid(base.EnemyChar.GetId(), 78);
			GameDataBridge.AddPostDataModificationHandler(this._moralityUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.UpdateAffected));
			this.UpdateAffected(context, default(DataUid));
		}

		// Token: 0x06003184 RID: 12676 RVA: 0x0021B3CC File Offset: 0x002195CC
		private void InvalidateAffectDataCache(DataContext context)
		{
			foreach (AffectedDataKey key in this.AffectDatas.Keys)
			{
				DomainManager.SpecialEffect.InvalidateCache(context, key.CharId, key.FieldId);
			}
		}

		// Token: 0x06003185 RID: 12677 RVA: 0x0021B438 File Offset: 0x00219638
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !this._isCurrCombatChar || !MinionBase.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = (int)this._activatedEffectParam;
			}
			return result;
		}

		// Token: 0x06003186 RID: 12678 RVA: 0x0021B46C File Offset: 0x0021966C
		// Note: this type is marked as 'beforefieldinit'.
		static ZhongXiangSheng()
		{
			Dictionary<sbyte, ushort[]> dictionary = new Dictionary<sbyte, ushort[]>();
			dictionary[0] = new ushort[]
			{
				9,
				10
			};
			dictionary[1] = new ushort[]
			{
				13,
				14
			};
			dictionary[2] = new ushort[]
			{
				8,
				7
			};
			dictionary[3] = new ushort[]
			{
				16,
				15
			};
			dictionary[4] = new ushort[]
			{
				11,
				12
			};
			ZhongXiangSheng.BehaviorType2FieldIds = dictionary;
			ZhongXiangSheng.BehaviorSpecifyParam = new Dictionary<sbyte, sbyte>
			{
				{
					2,
					-50
				}
			};
		}

		// Token: 0x04000EAC RID: 3756
		private const sbyte ReducePercent = -80;

		// Token: 0x04000EAD RID: 3757
		private static readonly Dictionary<sbyte, ushort[]> BehaviorType2FieldIds;

		// Token: 0x04000EAE RID: 3758
		private static readonly Dictionary<sbyte, sbyte> BehaviorSpecifyParam;

		// Token: 0x04000EAF RID: 3759
		private bool _isCurrCombatChar;

		// Token: 0x04000EB0 RID: 3760
		private DataUid _moralityUid;

		// Token: 0x04000EB1 RID: 3761
		private sbyte _activatedEffectParam;
	}
}
