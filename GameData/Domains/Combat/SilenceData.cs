using System;
using System.Collections.Generic;
using GameData.Domains.Item;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat
{
	// Token: 0x020006D2 RID: 1746
	[SerializableGameData(NotForArchive = true)]
	public class SilenceData : ISerializableGameData
	{
		// Token: 0x06006740 RID: 26432 RVA: 0x003B0C63 File Offset: 0x003AEE63
		public SilenceData()
		{
		}

		// Token: 0x06006741 RID: 26433 RVA: 0x003B0C90 File Offset: 0x003AEE90
		public SilenceData(SilenceData other)
		{
			this.CombatSkill = ((other.CombatSkill == null) ? null : new Dictionary<short, SilenceFrameData>(other.CombatSkill));
			this.WeaponKeys = ((other.WeaponKeys == null) ? null : new List<ItemKey>(other.WeaponKeys));
			this.WeaponFrames = ((other.WeaponFrames == null) ? null : new List<SilenceFrameData>(other.WeaponFrames));
		}

		// Token: 0x06006742 RID: 26434 RVA: 0x003B0D1C File Offset: 0x003AEF1C
		public void Assign(SilenceData other)
		{
			this.CombatSkill = ((other.CombatSkill == null) ? null : new Dictionary<short, SilenceFrameData>(other.CombatSkill));
			this.WeaponKeys = ((other.WeaponKeys == null) ? null : new List<ItemKey>(other.WeaponKeys));
			this.WeaponFrames = ((other.WeaponFrames == null) ? null : new List<SilenceFrameData>(other.WeaponFrames));
		}

		// Token: 0x06006743 RID: 26435 RVA: 0x003B0D80 File Offset: 0x003AEF80
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06006744 RID: 26436 RVA: 0x003B0D94 File Offset: 0x003AEF94
		public int GetSerializedSize()
		{
			int totalSize = 0;
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, SilenceFrameData>(this.CombatSkill);
			bool flag = this.WeaponKeys != null;
			if (flag)
			{
				totalSize += 2 + 8 * this.WeaponKeys.Count;
			}
			else
			{
				totalSize += 2;
			}
			bool flag2 = this.WeaponFrames != null;
			if (flag2)
			{
				totalSize += 2 + 8 * this.WeaponFrames.Count;
			}
			else
			{
				totalSize += 2;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06006745 RID: 26437 RVA: 0x003B0E14 File Offset: 0x003AF014
		public unsafe int Serialize(byte* pData)
		{
			byte* pCurrData = pData + SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<short, SilenceFrameData>(pData, ref this.CombatSkill);
			bool flag = this.WeaponKeys != null;
			if (flag)
			{
				int elementsCount = this.WeaponKeys.Count;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount);
				pCurrData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					pCurrData += this.WeaponKeys[i].Serialize(pCurrData);
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag2 = this.WeaponFrames != null;
			if (flag2)
			{
				int elementsCount2 = this.WeaponFrames.Count;
				Tester.Assert(elementsCount2 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount2);
				pCurrData += 2;
				for (int j = 0; j < elementsCount2; j++)
				{
					pCurrData += this.WeaponFrames[j].Serialize(pCurrData);
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06006746 RID: 26438 RVA: 0x003B0F44 File Offset: 0x003AF144
		public unsafe int Deserialize(byte* pData)
		{
			byte* pCurrData = pData + SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<short, SilenceFrameData>(pData, ref this.CombatSkill);
			ushort elementsCount = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag = elementsCount > 0;
			if (flag)
			{
				bool flag2 = this.WeaponKeys == null;
				if (flag2)
				{
					this.WeaponKeys = new List<ItemKey>((int)elementsCount);
				}
				else
				{
					this.WeaponKeys.Clear();
				}
				for (int i = 0; i < (int)elementsCount; i++)
				{
					ItemKey element = default(ItemKey);
					pCurrData += element.Deserialize(pCurrData);
					this.WeaponKeys.Add(element);
				}
			}
			else
			{
				List<ItemKey> weaponKeys = this.WeaponKeys;
				if (weaponKeys != null)
				{
					weaponKeys.Clear();
				}
			}
			ushort elementsCount2 = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag3 = elementsCount2 > 0;
			if (flag3)
			{
				bool flag4 = this.WeaponFrames == null;
				if (flag4)
				{
					this.WeaponFrames = new List<SilenceFrameData>((int)elementsCount2);
				}
				else
				{
					this.WeaponFrames.Clear();
				}
				for (int j = 0; j < (int)elementsCount2; j++)
				{
					SilenceFrameData element2 = default(SilenceFrameData);
					pCurrData += element2.Deserialize(pCurrData);
					this.WeaponFrames.Add(element2);
				}
			}
			else
			{
				List<SilenceFrameData> weaponFrames = this.WeaponFrames;
				if (weaponFrames != null)
				{
					weaponFrames.Clear();
				}
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04001C1E RID: 7198
		[SerializableGameDataField]
		public Dictionary<short, SilenceFrameData> CombatSkill = new Dictionary<short, SilenceFrameData>();

		// Token: 0x04001C1F RID: 7199
		[SerializableGameDataField]
		public List<ItemKey> WeaponKeys = new List<ItemKey>();

		// Token: 0x04001C20 RID: 7200
		[SerializableGameDataField]
		public List<SilenceFrameData> WeaponFrames = new List<SilenceFrameData>();
	}
}
