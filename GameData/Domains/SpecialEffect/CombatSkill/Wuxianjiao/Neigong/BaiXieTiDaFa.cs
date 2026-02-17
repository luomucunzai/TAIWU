using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Neigong
{
	// Token: 0x0200038C RID: 908
	public class BaiXieTiDaFa : CombatSkillEffectBase
	{
		// Token: 0x06003633 RID: 13875 RVA: 0x0022FE76 File Offset: 0x0022E076
		public BaiXieTiDaFa()
		{
		}

		// Token: 0x06003634 RID: 13876 RVA: 0x0022FE80 File Offset: 0x0022E080
		public BaiXieTiDaFa(CombatSkillKey skillKey) : base(skillKey, 12006, -1)
		{
		}

		// Token: 0x06003635 RID: 13877 RVA: 0x0022FE94 File Offset: 0x0022E094
		public override void OnEnable(DataContext context)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this._affected = true;
				this._selfAddPower = this.CalcChangePower(base.CharacterId);
				this._selfNeiliAllocationUid = base.ParseCharDataUid(17);
				GameDataBridge.AddPostDataModificationHandler(this._selfNeiliAllocationUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnFeaturesChange));
				this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, -1, -1, -1, -1), EDataModifyType.AddPercent);
			}
			else
			{
				Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
				Events.RegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
				Events.RegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
			}
		}

		// Token: 0x06003636 RID: 13878 RVA: 0x0022FF5C File Offset: 0x0022E15C
		public override void OnDisable(DataContext context)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				GameDataBridge.RemovePostDataModificationHandler(this._selfNeiliAllocationUid, base.DataHandlerKey);
			}
			else
			{
				Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
				Events.UnRegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
				Events.UnRegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
			}
		}

		// Token: 0x06003637 RID: 13879 RVA: 0x0022FFC4 File Offset: 0x0022E1C4
		private void OnCombatBegin(DataContext context)
		{
			bool flag = !DomainManager.Combat.IsCharInCombat(base.CharacterId, true);
			if (!flag)
			{
				this._affected = (base.IsDirect || base.IsCurrent);
				this._enemyReducePowers = new Dictionary<int, int>();
				this._enemyNeiliAllocationUids = new List<DataUid>();
				foreach (int enemyId in DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly))
				{
					bool flag2 = enemyId < 0;
					if (!flag2)
					{
						this._enemyReducePowers.Add(enemyId, this.CalcChangePower(enemyId));
						DataUid enemyFeaturesUid = new DataUid(4, 0, (ulong)((long)enemyId), 17U);
						GameDataBridge.AddPostDataModificationHandler(enemyFeaturesUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnFeaturesChange));
						this._enemyNeiliAllocationUids.Add(enemyFeaturesUid);
						base.AppendAffectedData(context, enemyId, 199, EDataModifyType.AddPercent, -1);
					}
				}
			}
		}

		// Token: 0x06003638 RID: 13880 RVA: 0x002300B4 File Offset: 0x0022E2B4
		private void OnCombatCharChanged(DataContext context, bool isAlly)
		{
			bool flag = !DomainManager.Combat.IsCharInCombat(base.CharacterId, true);
			if (!flag)
			{
				bool affected = base.IsDirect || base.IsCurrent;
				bool flag2 = affected == this._affected;
				if (!flag2)
				{
					this._affected = affected;
					base.InvalidateAllEnemyCache(context, 199);
				}
			}
		}

		// Token: 0x06003639 RID: 13881 RVA: 0x00230114 File Offset: 0x0022E314
		private void OnCombatSettlement(DataContext context, sbyte combatStatus)
		{
			bool flag = this._enemyReducePowers == null;
			if (!flag)
			{
				for (int i = 0; i < this._enemyNeiliAllocationUids.Count; i++)
				{
					GameDataBridge.RemovePostDataModificationHandler(this._enemyNeiliAllocationUids[i], base.DataHandlerKey);
				}
				this._enemyReducePowers = null;
				this._enemyNeiliAllocationUids = null;
				base.ClearAffectedData(context);
			}
		}

		// Token: 0x0600363A RID: 13882 RVA: 0x0023017C File Offset: 0x0022E37C
		private void OnFeaturesChange(DataContext context, DataUid dataUid)
		{
			int charId = (int)dataUid.SubId0;
			int changePower = this.CalcChangePower(charId);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this._selfAddPower = changePower;
			}
			else
			{
				this._enemyReducePowers[charId] = changePower;
			}
			DomainManager.SpecialEffect.InvalidateCache(context, charId, 199);
		}

		// Token: 0x0600363B RID: 13883 RVA: 0x002301D0 File Offset: 0x0022E3D0
		private int CalcChangePower(int charId)
		{
			List<short> featureIds = DomainManager.Character.GetElement_Objects(charId).GetFeatureIds();
			int changePower = 0;
			for (int i = 0; i < featureIds.Count; i++)
			{
				CharacterFeatureItem featureConfig = CharacterFeature.Instance[featureIds[i]];
				bool flag = base.IsDirect ? featureConfig.IsBad() : featureConfig.IsGood();
				if (flag)
				{
					changePower += (base.IsDirect ? 3 : -3);
				}
			}
			return changePower;
		}

		// Token: 0x0600363C RID: 13884 RVA: 0x00230250 File Offset: 0x0022E450
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.FieldId == 199 && this._affected;
			int result;
			if (flag)
			{
				result = (base.IsDirect ? this._selfAddPower : this._enemyReducePowers[dataKey.CharId]);
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x04000FCD RID: 4045
		private const sbyte PowerChangePerFeature = 3;

		// Token: 0x04000FCE RID: 4046
		private int _selfAddPower;

		// Token: 0x04000FCF RID: 4047
		private DataUid _selfNeiliAllocationUid;

		// Token: 0x04000FD0 RID: 4048
		private Dictionary<int, int> _enemyReducePowers;

		// Token: 0x04000FD1 RID: 4049
		private List<DataUid> _enemyNeiliAllocationUids;

		// Token: 0x04000FD2 RID: 4050
		private bool _affected;
	}
}
