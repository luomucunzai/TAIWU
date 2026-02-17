using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat
{
	// Token: 0x02000709 RID: 1801
	[SerializableGameData(NotForArchive = true)]
	public class WeaponExpectInnerRatioData : ISerializableGameData
	{
		// Token: 0x06006816 RID: 26646 RVA: 0x003B39CA File Offset: 0x003B1BCA
		public void SetValue(int charId, int weaponIndex, sbyte expectInnerRatio)
		{
			this._internalValue[new IntPair(charId, weaponIndex)] = expectInnerRatio;
		}

		// Token: 0x06006817 RID: 26647 RVA: 0x003B39E0 File Offset: 0x003B1BE0
		public sbyte GetValue(int charId, int weaponIndex)
		{
			return this._internalValue.GetOrDefault(new IntPair(charId, weaponIndex), -1);
		}

		// Token: 0x06006818 RID: 26648 RVA: 0x003B39F5 File Offset: 0x003B1BF5
		public void Clear()
		{
			this._internalValue.Clear();
		}

		// Token: 0x06006819 RID: 26649 RVA: 0x003B3A03 File Offset: 0x003B1C03
		public WeaponExpectInnerRatioData()
		{
		}

		// Token: 0x0600681A RID: 26650 RVA: 0x003B3A18 File Offset: 0x003B1C18
		public WeaponExpectInnerRatioData(WeaponExpectInnerRatioData other)
		{
			this._internalValue = ((other._internalValue == null) ? null : new Dictionary<IntPair, sbyte>(other._internalValue));
		}

		// Token: 0x0600681B RID: 26651 RVA: 0x003B3A49 File Offset: 0x003B1C49
		public void Assign(WeaponExpectInnerRatioData other)
		{
			this._internalValue = ((other._internalValue == null) ? null : new Dictionary<IntPair, sbyte>(other._internalValue));
		}

		// Token: 0x0600681C RID: 26652 RVA: 0x003B3A68 File Offset: 0x003B1C68
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x0600681D RID: 26653 RVA: 0x003B3A7C File Offset: 0x003B1C7C
		public int GetSerializedSize()
		{
			int totalSize = 0;
			totalSize += SerializationHelper.DictionaryOfCustomTypeBasicTypePair.GetSerializedSize<IntPair, sbyte>(this._internalValue);
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600681E RID: 26654 RVA: 0x003B3AB0 File Offset: 0x003B1CB0
		public unsafe int Serialize(byte* pData)
		{
			byte* pCurrData = pData + SerializationHelper.DictionaryOfCustomTypeBasicTypePair.Serialize<IntPair, sbyte>(pData, ref this._internalValue);
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600681F RID: 26655 RVA: 0x003B3AEC File Offset: 0x003B1CEC
		public unsafe int Deserialize(byte* pData)
		{
			byte* pCurrData = pData + SerializationHelper.DictionaryOfCustomTypeBasicTypePair.Deserialize<IntPair, sbyte>(pData, ref this._internalValue);
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04001C7B RID: 7291
		[SerializableGameDataField]
		private Dictionary<IntPair, sbyte> _internalValue = new Dictionary<IntPair, sbyte>();
	}
}
