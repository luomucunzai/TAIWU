using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Neigong
{
	// Token: 0x02000454 RID: 1108
	public class ShengShenFa : CombatSkillEffectBase
	{
		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06003A96 RID: 14998 RVA: 0x002441F4 File Offset: 0x002423F4
		private int AddPower
		{
			get
			{
				return base.IsDirect ? 4 : 2;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06003A97 RID: 14999 RVA: 0x00244202 File Offset: 0x00242402
		private int ReducePower
		{
			get
			{
				return base.IsDirect ? 12 : 2;
			}
		}

		// Token: 0x06003A98 RID: 15000 RVA: 0x00244211 File Offset: 0x00242411
		public ShengShenFa()
		{
		}

		// Token: 0x06003A99 RID: 15001 RVA: 0x0024421B File Offset: 0x0024241B
		public ShengShenFa(CombatSkillKey skillKey, sbyte direction) : base(skillKey, 7004, direction)
		{
			this._currAddPower = 0;
		}

		// Token: 0x06003A9A RID: 15002 RVA: 0x00244234 File Offset: 0x00242434
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, -1, -1, -1, -1), EDataModifyType.Add);
			Events.RegisterHandler_PostAdvanceMonthBegin(new Events.OnPostAdvanceMonthBegin(this.OnAdvanceMonthBegin));
			Events.RegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
		}

		// Token: 0x06003A9B RID: 15003 RVA: 0x00244292 File Offset: 0x00242492
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PostAdvanceMonthBegin(new Events.OnPostAdvanceMonthBegin(this.OnAdvanceMonthBegin));
			Events.UnRegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
		}

		// Token: 0x06003A9C RID: 15004 RVA: 0x002442BC File Offset: 0x002424BC
		private void OnAdvanceMonthBegin(DataContext context)
		{
			bool flag = this.CharObj.IsCombatSkillEquipped(base.SkillTemplateId) && this.CharObj.GetCombatSkillCanAffect(base.SkillTemplateId);
			if (flag)
			{
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					this._currAddPower = (sbyte)Math.Min((int)this._currAddPower + this.AddPower, 20);
				}
				else
				{
					this._currAddPower = (sbyte)Math.Max((int)this._currAddPower - this.ReducePower, 0);
				}
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			}
			else
			{
				bool flag2 = this._currAddPower > 0;
				if (flag2)
				{
					this._currAddPower = 0;
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
					bool flag3 = base.CharacterId == DomainManager.Taiwu.GetTaiwuCharId();
					if (flag3)
					{
						DomainManager.World.GetMonthlyNotificationCollection().AddAccumulatedSkillPowerLost(base.SkillTemplateId);
					}
				}
			}
			DomainManager.SpecialEffect.SaveEffect(context, this.Id);
		}

		// Token: 0x06003A9D RID: 15005 RVA: 0x002443BC File Offset: 0x002425BC
		private void OnCombatSettlement(DataContext context, sbyte combatStatus)
		{
			bool flag = !DomainManager.Combat.IsCharInCombat(base.CharacterId, false) || !this.CharObj.GetCombatSkillEquipment().IsCombatSkillEquipped(base.SkillTemplateId) || !this.CharObj.GetCombatSkillCanAffect(base.SkillTemplateId);
			if (!flag)
			{
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					this._currAddPower = (sbyte)Math.Max((int)this._currAddPower - this.ReducePower, 0);
				}
				else
				{
					this._currAddPower = (sbyte)Math.Min((int)this._currAddPower + this.AddPower, 20);
				}
				DomainManager.SpecialEffect.SaveEffect(context, this.Id);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			}
		}

		// Token: 0x06003A9E RID: 15006 RVA: 0x0024447C File Offset: 0x0024267C
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
				bool flag2 = dataKey.FieldId == 199;
				if (flag2)
				{
					result = (int)this._currAddPower;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x06003A9F RID: 15007 RVA: 0x002444C4 File Offset: 0x002426C4
		protected override int GetSubClassSerializedSize()
		{
			return base.GetSubClassSerializedSize() + 1;
		}

		// Token: 0x06003AA0 RID: 15008 RVA: 0x002444E0 File Offset: 0x002426E0
		protected unsafe override int SerializeSubClass(byte* pData)
		{
			byte* pCurrData = pData + base.SerializeSubClass(pData);
			*pCurrData = (byte)this._currAddPower;
			return this.GetSubClassSerializedSize();
		}

		// Token: 0x06003AA1 RID: 15009 RVA: 0x0024450C File Offset: 0x0024270C
		protected unsafe override int DeserializeSubClass(byte* pData)
		{
			byte* pCurrData = pData + base.DeserializeSubClass(pData);
			this._currAddPower = *(sbyte*)pCurrData;
			return this.GetSubClassSerializedSize();
		}

		// Token: 0x04001127 RID: 4391
		private const sbyte MaxAddPower = 20;

		// Token: 0x04001128 RID: 4392
		private sbyte _currAddPower;
	}
}
