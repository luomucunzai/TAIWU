using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;

namespace GameData.Domains.SpecialEffect.Misc
{
	// Token: 0x02000118 RID: 280
	public class AddPenetrateAndPenetrateResist : SpecialEffectBase
	{
		// Token: 0x06002A1C RID: 10780 RVA: 0x00201E0B File Offset: 0x0020000B
		public AddPenetrateAndPenetrateResist()
		{
		}

		// Token: 0x06002A1D RID: 10781 RVA: 0x00201E15 File Offset: 0x00200015
		public AddPenetrateAndPenetrateResist(int charId, OuterAndInnerInts addPenetrate, OuterAndInnerInts addPenetrateResist) : base(charId, 1000000)
		{
			this._addPenetrate = addPenetrate;
			this._addPenetrateResist = addPenetrateResist;
		}

		// Token: 0x06002A1E RID: 10782 RVA: 0x00201E34 File Offset: 0x00200034
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 44, -1, -1, -1, -1), EDataModifyType.Add);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 45, -1, -1, -1, -1), EDataModifyType.Add);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 46, -1, -1, -1, -1), EDataModifyType.Add);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 47, -1, -1, -1, -1), EDataModifyType.Add);
		}

		// Token: 0x06002A1F RID: 10783 RVA: 0x00201EC8 File Offset: 0x002000C8
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
				bool flag2 = dataKey.FieldId == 44;
				if (flag2)
				{
					result = this._addPenetrate.Outer;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 45;
					if (flag3)
					{
						result = this._addPenetrate.Inner;
					}
					else
					{
						bool flag4 = dataKey.FieldId == 46;
						if (flag4)
						{
							result = this._addPenetrateResist.Outer;
						}
						else
						{
							bool flag5 = dataKey.FieldId == 47;
							if (flag5)
							{
								result = this._addPenetrateResist.Inner;
							}
							else
							{
								result = 0;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06002A20 RID: 10784 RVA: 0x00201F68 File Offset: 0x00200168
		protected override int GetSubClassSerializedSize()
		{
			return this._addPenetrate.GetSerializedSize() + this._addPenetrateResist.GetSerializedSize();
		}

		// Token: 0x06002A21 RID: 10785 RVA: 0x00201F94 File Offset: 0x00200194
		protected unsafe override int SerializeSubClass(byte* pData)
		{
			*(int*)pData = this._addPenetrate.Outer;
			byte* pCurrData = pData + 4;
			*(int*)pCurrData = this._addPenetrate.Inner;
			pCurrData += 4;
			*(int*)pCurrData = this._addPenetrateResist.Outer;
			pCurrData += 4;
			*(int*)pCurrData = this._addPenetrateResist.Inner;
			pCurrData += 4;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x06002A22 RID: 10786 RVA: 0x00201FF4 File Offset: 0x002001F4
		protected unsafe override int DeserializeSubClass(byte* pData)
		{
			this._addPenetrate.Outer = *(int*)pData;
			byte* pCurrData = pData + 4;
			this._addPenetrate.Inner = *(int*)pCurrData;
			pCurrData += 4;
			this._addPenetrateResist.Outer = *(int*)pCurrData;
			pCurrData += 4;
			this._addPenetrateResist.Inner = *(int*)pCurrData;
			pCurrData += 4;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x04000CE8 RID: 3304
		private OuterAndInnerInts _addPenetrate;

		// Token: 0x04000CE9 RID: 3305
		private OuterAndInnerInts _addPenetrateResist;
	}
}
