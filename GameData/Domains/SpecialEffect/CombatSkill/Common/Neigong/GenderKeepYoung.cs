using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong
{
	// Token: 0x02000575 RID: 1397
	public abstract class GenderKeepYoung : CombatSkillEffectBase
	{
		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06004140 RID: 16704
		protected abstract sbyte ReduceFatalDamageValueType { get; }

		// Token: 0x06004141 RID: 16705 RVA: 0x00261FDE File Offset: 0x002601DE
		protected GenderKeepYoung()
		{
		}

		// Token: 0x06004142 RID: 16706 RVA: 0x00261FE8 File Offset: 0x002601E8
		protected GenderKeepYoung(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06004143 RID: 16707 RVA: 0x00261FF8 File Offset: 0x002601F8
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 1, -1, -1, -1, -1), EDataModifyType.Add);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 2, -1, -1, -1, -1), EDataModifyType.Add);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 3, -1, -1, -1, -1), EDataModifyType.Add);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 4, -1, -1, -1, -1), EDataModifyType.Add);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 5, -1, -1, -1, -1), EDataModifyType.Add);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 6, -1, -1, -1, -1), EDataModifyType.Add);
			}
			else
			{
				Events.RegisterHandler_ChangeNeiliAllocationAfterCombatBegin(new Events.OnChangeNeiliAllocationAfterCombatBegin(this.ChangeNeiliAllocationAfterCombatBegin));
			}
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 191, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 192, -1, -1, -1, -1), EDataModifyType.Custom);
			bool flag = this.CharObj.GetGender() == this.RequireGender;
			if (flag)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, -1, -1, -1, -1), EDataModifyType.AddPercent);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 25, -1, -1, -1, -1), EDataModifyType.Custom);
			}
			this._featureUid = new DataUid(4, 0, (ulong)((long)base.CharacterId), 17U);
			GameDataBridge.AddPostDataModificationHandler(this._featureUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnFeaturesChange));
		}

		// Token: 0x06004144 RID: 16708 RVA: 0x002621B2 File Offset: 0x002603B2
		public override void OnDataAdded(DataContext context)
		{
			this.UpdateAvatar(context);
		}

		// Token: 0x06004145 RID: 16709 RVA: 0x002621C0 File Offset: 0x002603C0
		public override void OnDisable(DataContext context)
		{
			bool flag = !base.IsDirect;
			if (flag)
			{
				Events.UnRegisterHandler_ChangeNeiliAllocationAfterCombatBegin(new Events.OnChangeNeiliAllocationAfterCombatBegin(this.ChangeNeiliAllocationAfterCombatBegin));
			}
			GameDataBridge.RemovePostDataModificationHandler(this._featureUid, base.DataHandlerKey);
			this.UpdateAvatar(context);
		}

		// Token: 0x06004146 RID: 16710 RVA: 0x00262208 File Offset: 0x00260408
		private void ChangeNeiliAllocationAfterCombatBegin(DataContext context, CombatCharacter character, NeiliAllocation allocationAfterBegin)
		{
			bool flag = character.GetId() != base.CharacterId;
			if (!flag)
			{
				bool flag2 = character.GetNeiliAllocation().GetTotal() == allocationAfterBegin.GetTotal();
				if (flag2)
				{
					base.ShowSpecialEffectTips(1);
					character.ChangeAllNeiliAllocation(context, 100, true);
				}
			}
		}

		// Token: 0x06004147 RID: 16711 RVA: 0x0026225D File Offset: 0x0026045D
		private void OnFeaturesChange(DataContext context, DataUid dataUid)
		{
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 25);
		}

		// Token: 0x06004148 RID: 16712 RVA: 0x0026228C File Offset: 0x0026048C
		private void UpdateAvatar(DataContext context)
		{
			AvatarData avatar = this.CharObj.GetAvatar();
			bool flag = avatar.UpdateGrowableElementsShowingAbilities(this.CharObj);
			if (flag)
			{
				this.CharObj.SetAvatar(avatar, context);
			}
		}

		// Token: 0x06004149 RID: 16713 RVA: 0x002622C4 File Offset: 0x002604C4
		private bool IsAffectType(int type)
		{
			return base.IsDirect ? (type == (int)this.ReduceFatalDamageValueType) : (type != (int)this.ReduceFatalDamageValueType);
		}

		// Token: 0x0600414A RID: 16714 RVA: 0x002622F8 File Offset: 0x002604F8
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
				bool flag2 = dataKey.FieldId == 191 && this.IsAffectType(dataKey.CustomParam0);
				if (flag2)
				{
					EDamageType damageType = (EDamageType)dataKey.CustomParam1;
					bool flag3 = damageType != EDamageType.Direct && base.IsDirect;
					if (flag3)
					{
						result = 0;
					}
					else
					{
						base.ShowSpecialEffectTips(0);
						result = (base.IsDirect ? -50 : 100);
					}
				}
				else
				{
					bool flag4 = this.CharObj.HasVirginity() && dataKey.FieldId == 199;
					if (flag4)
					{
						result = 10;
					}
					else
					{
						ushort fieldId = dataKey.FieldId;
						bool flag5 = fieldId - 1 <= 5;
						bool flag6 = flag5;
						if (flag6)
						{
							result = 20;
						}
						else
						{
							result = 0;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600414B RID: 16715 RVA: 0x002623D0 File Offset: 0x002605D0
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = this.CharObj.HasVirginity() && dataKey.FieldId == 25;
				if (flag2)
				{
					result = Math.Min(dataValue, (int)GlobalConfig.Instance.RejuvenatedAge);
				}
				else
				{
					bool flag3 = dataKey.FieldId == 192 && dataKey.CustomParam2 == 0 && !base.IsDirect && this.IsAffectType(dataKey.CustomParam0);
					if (flag3)
					{
						base.ShowSpecialEffectTips(0);
						result = dataValue * 2;
					}
					else
					{
						result = dataValue;
					}
				}
			}
			return result;
		}

		// Token: 0x0600414C RID: 16716 RVA: 0x0026246C File Offset: 0x0026066C
		protected override int GetSubClassSerializedSize()
		{
			return base.GetSubClassSerializedSize() + 1;
		}

		// Token: 0x0600414D RID: 16717 RVA: 0x00262488 File Offset: 0x00260688
		protected unsafe override int SerializeSubClass(byte* pData)
		{
			byte* pCurrData = pData + base.SerializeSubClass(pData);
			*pCurrData = (byte)this.RequireGender;
			return this.GetSubClassSerializedSize();
		}

		// Token: 0x0600414E RID: 16718 RVA: 0x002624B4 File Offset: 0x002606B4
		protected unsafe override int DeserializeSubClass(byte* pData)
		{
			byte* pCurrData = pData + base.DeserializeSubClass(pData);
			this.RequireGender = *(sbyte*)pCurrData;
			return this.GetSubClassSerializedSize();
		}

		// Token: 0x04001335 RID: 4917
		private const sbyte AddPower = 10;

		// Token: 0x04001336 RID: 4918
		private const sbyte AddMaxMainAttribute = 20;

		// Token: 0x04001337 RID: 4919
		private const int ReverseChangeNeiliAllocationPercent = 100;

		// Token: 0x04001338 RID: 4920
		private const int DirectChangeFatalDamageValueTotalPercent = -50;

		// Token: 0x04001339 RID: 4921
		private const int ReverseChangeFatalDamageValueTotalPercent = 100;

		// Token: 0x0400133A RID: 4922
		private const int ReverseChangeFatalDamageCountMultiplier = 2;

		// Token: 0x0400133B RID: 4923
		protected sbyte RequireGender;

		// Token: 0x0400133C RID: 4924
		private DataUid _featureUid;
	}
}
