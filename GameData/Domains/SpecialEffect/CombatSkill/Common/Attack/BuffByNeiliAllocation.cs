using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x02000593 RID: 1427
	public class BuffByNeiliAllocation : CombatSkillEffectBase
	{
		// Token: 0x17000290 RID: 656
		// (get) Token: 0x0600424C RID: 16972 RVA: 0x0026629C File Offset: 0x0026449C
		protected virtual bool ShowTipsOnAffecting
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x0600424D RID: 16973 RVA: 0x0026629F File Offset: 0x0026449F
		// (set) Token: 0x0600424E RID: 16974 RVA: 0x002662A7 File Offset: 0x002644A7
		private protected bool Affecting { protected get; private set; }

		// Token: 0x0600424F RID: 16975 RVA: 0x002662B0 File Offset: 0x002644B0
		protected BuffByNeiliAllocation()
		{
		}

		// Token: 0x06004250 RID: 16976 RVA: 0x002662BA File Offset: 0x002644BA
		protected BuffByNeiliAllocation(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06004251 RID: 16977 RVA: 0x002662C8 File Offset: 0x002644C8
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
			this._inited = false;
			this._selfNeiliAllocationUid = base.ParseNeiliAllocationDataUid();
			GameDataBridge.AddPostDataModificationHandler(this._selfNeiliAllocationUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.UpdateAffecting));
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004252 RID: 16978 RVA: 0x00266370 File Offset: 0x00264570
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._selfNeiliAllocationUid, base.DataHandlerKey);
			GameDataBridge.RemovePostDataModificationHandler(this._enemyNeiliAllocationUid, base.DataHandlerKey);
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004253 RID: 16979 RVA: 0x002663D8 File Offset: 0x002645D8
		private unsafe void UpdateAffecting(DataContext context, DataUid dataUid)
		{
			CombatCharacter currEnemy = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			NeiliAllocation selfNeiliAllocation = base.CombatChar.GetNeiliAllocation();
			NeiliAllocation selfOrigNeiliAllocation = base.CombatChar.GetOriginNeiliAllocation();
			NeiliAllocation enemyNeiliAllocation = currEnemy.GetNeiliAllocation();
			bool canAffect = base.IsDirect ? (*(ref selfNeiliAllocation.Items.FixedElementField + (IntPtr)this.RequireNeiliAllocationType * 2) > *(ref selfOrigNeiliAllocation.Items.FixedElementField + (IntPtr)this.RequireNeiliAllocationType * 2) && *(ref selfNeiliAllocation.Items.FixedElementField + (IntPtr)this.RequireNeiliAllocationType * 2) > *(ref enemyNeiliAllocation.Items.FixedElementField + (IntPtr)this.RequireNeiliAllocationType * 2)) : (*(ref selfNeiliAllocation.Items.FixedElementField + (IntPtr)this.RequireNeiliAllocationType * 2) < *(ref selfOrigNeiliAllocation.Items.FixedElementField + (IntPtr)this.RequireNeiliAllocationType * 2) && *(ref selfNeiliAllocation.Items.FixedElementField + (IntPtr)this.RequireNeiliAllocationType * 2) < *(ref enemyNeiliAllocation.Items.FixedElementField + (IntPtr)this.RequireNeiliAllocationType * 2));
			bool flag = this.Affecting != canAffect;
			if (flag)
			{
				this.Affecting = canAffect;
				bool inited = this._inited;
				if (inited)
				{
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
					this.OnInvalidCache(context);
				}
				bool flag2 = this.Affecting && this.ShowTipsOnAffecting;
				if (flag2)
				{
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06004254 RID: 16980 RVA: 0x0026655C File Offset: 0x0026475C
		private void OnCombatBegin(DataContext context)
		{
			CombatCharacter currEnemy = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			this._enemyNeiliAllocationUid = base.ParseNeiliAllocationDataUid(currEnemy.GetId());
			GameDataBridge.AddPostDataModificationHandler(this._enemyNeiliAllocationUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.UpdateAffecting));
			this._inited = true;
			this.UpdateAffecting(context, this._enemyNeiliAllocationUid);
		}

		// Token: 0x06004255 RID: 16981 RVA: 0x002665CC File Offset: 0x002647CC
		private void OnCombatCharChanged(DataContext context, bool isAlly)
		{
			bool flag = isAlly == base.CombatChar.IsAlly;
			if (!flag)
			{
				CombatCharacter currEnemy = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				GameDataBridge.RemovePostDataModificationHandler(this._enemyNeiliAllocationUid, base.DataHandlerKey);
				this._enemyNeiliAllocationUid = base.ParseNeiliAllocationDataUid(currEnemy.GetId());
				GameDataBridge.AddPostDataModificationHandler(this._enemyNeiliAllocationUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.UpdateAffecting));
				this.UpdateAffecting(context, this._enemyNeiliAllocationUid);
			}
		}

		// Token: 0x06004256 RID: 16982 RVA: 0x00266658 File Offset: 0x00264858
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool affecting = this.Affecting;
				if (affecting)
				{
					base.CombatChar.ChangeNeiliAllocation(context, this.RequireNeiliAllocationType, base.IsDirect ? -12 : 12, true, true);
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x06004257 RID: 16983 RVA: 0x002666BC File Offset: 0x002648BC
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !this.Affecting;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 199 && dataKey.CharId == base.CharacterId && dataKey.CombatSkillId == base.SkillTemplateId;
				if (flag2)
				{
					result = 40;
				}
				else
				{
					result = this.GetAffectedModifyValue(dataKey);
				}
			}
			return result;
		}

		// Token: 0x06004258 RID: 16984 RVA: 0x0026671B File Offset: 0x0026491B
		protected virtual void OnInvalidCache(DataContext context)
		{
		}

		// Token: 0x06004259 RID: 16985 RVA: 0x00266720 File Offset: 0x00264920
		protected virtual int GetAffectedModifyValue(AffectedDataKey dataKey)
		{
			return 0;
		}

		// Token: 0x04001395 RID: 5013
		private const sbyte AddPower = 40;

		// Token: 0x04001396 RID: 5014
		private const sbyte ChangeNeiliAllocation = 12;

		// Token: 0x04001397 RID: 5015
		protected byte RequireNeiliAllocationType;

		// Token: 0x04001399 RID: 5017
		private DataUid _selfNeiliAllocationUid;

		// Token: 0x0400139A RID: 5018
		private DataUid _enemyNeiliAllocationUid;

		// Token: 0x0400139B RID: 5019
		private bool _inited;
	}
}
