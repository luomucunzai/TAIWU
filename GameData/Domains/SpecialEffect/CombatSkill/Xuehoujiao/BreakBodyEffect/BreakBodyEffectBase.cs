using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect
{
	// Token: 0x0200024C RID: 588
	public class BreakBodyEffectBase : SpecialEffectBase
	{
		// Token: 0x06003004 RID: 12292 RVA: 0x002158A3 File Offset: 0x00213AA3
		protected BreakBodyEffectBase()
		{
		}

		// Token: 0x06003005 RID: 12293 RVA: 0x002158AD File Offset: 0x00213AAD
		protected BreakBodyEffectBase(int charId, int type) : base(charId, type)
		{
		}

		// Token: 0x06003006 RID: 12294 RVA: 0x002158BC File Offset: 0x00213ABC
		public override void OnEnable(DataContext context)
		{
			this._injuryUid = new DataUid(4, 0, (ulong)((long)base.CharacterId), 26U);
			GameDataBridge.AddPostDataModificationHandler(this._injuryUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnInjuriesUpdate));
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 168, -1, -1, -1, -1), EDataModifyType.Custom);
		}

		// Token: 0x06003007 RID: 12295 RVA: 0x0021592A File Offset: 0x00213B2A
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._injuryUid, base.DataHandlerKey);
		}

		// Token: 0x06003008 RID: 12296 RVA: 0x00215940 File Offset: 0x00213B40
		private void OnInjuriesUpdate(DataContext context, DataUid dataUid)
		{
			Injuries injuries = this.CharObj.GetInjuries();
			bool allHealed = true;
			for (int i = 0; i < this.AffectBodyParts.Length; i++)
			{
				bool flag = injuries.Get(this.AffectBodyParts[i], this.IsInner) > 0;
				if (flag)
				{
					allHealed = false;
					break;
				}
			}
			bool flag2 = allHealed;
			if (flag2)
			{
				this.CharObj.RemoveFeature(context, this.FeatureId);
				DomainManager.SpecialEffect.Remove(context, this.Id);
			}
		}

		// Token: 0x06003009 RID: 12297 RVA: 0x002159C8 File Offset: 0x00213BC8
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
				bool flag2 = dataKey.FieldId == 168 && this.AffectBodyParts.Exist((sbyte)dataKey.CustomParam0) && dataKey.CustomParam1 == (this.IsInner ? 1 : 0);
				if (flag2)
				{
					result = Math.Min(4, dataValue);
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x0600300A RID: 12298 RVA: 0x00215A3C File Offset: 0x00213C3C
		protected override int GetSubClassSerializedSize()
		{
			return 1 + this.AffectBodyParts.Length + 1 + 2;
		}

		// Token: 0x0600300B RID: 12299 RVA: 0x00215A5C File Offset: 0x00213C5C
		protected unsafe override int SerializeSubClass(byte* pData)
		{
			sbyte bodyPartCount = (sbyte)this.AffectBodyParts.Length;
			*pData = (byte)bodyPartCount;
			byte* pCurrData = pData + 1;
			for (int i = 0; i < (int)bodyPartCount; i++)
			{
				*pCurrData = (byte)this.AffectBodyParts[i];
				pCurrData++;
			}
			*pCurrData = (this.IsInner ? 1 : 0);
			pCurrData++;
			*(short*)pCurrData = this.FeatureId;
			pCurrData += 2;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x0600300C RID: 12300 RVA: 0x00215AC4 File Offset: 0x00213CC4
		protected unsafe override int DeserializeSubClass(byte* pData)
		{
			sbyte bodyPartCount = *(sbyte*)pData;
			byte* pCurrData = pData + 1;
			this.AffectBodyParts = new sbyte[(int)bodyPartCount];
			for (int i = 0; i < (int)bodyPartCount; i++)
			{
				this.AffectBodyParts[i] = *(sbyte*)pCurrData;
				pCurrData++;
			}
			this.IsInner = (*pCurrData != 0);
			pCurrData++;
			this.FeatureId = *(short*)pCurrData;
			pCurrData += 2;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x04000E3E RID: 3646
		private const sbyte BreakNeedInjury = 4;

		// Token: 0x04000E3F RID: 3647
		protected sbyte[] AffectBodyParts;

		// Token: 0x04000E40 RID: 3648
		protected bool IsInner;

		// Token: 0x04000E41 RID: 3649
		protected short FeatureId;

		// Token: 0x04000E42 RID: 3650
		private DataUid _injuryUid;
	}
}
