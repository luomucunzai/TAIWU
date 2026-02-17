using System;
using Config;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat
{
	// Token: 0x020006D1 RID: 1745
	[SerializableGameData(NotForArchive = true)]
	public struct ShowSpecialEffectDisplayData : ISerializableGameData
	{
		// Token: 0x06006739 RID: 26425 RVA: 0x003B0A7C File Offset: 0x003AEC7C
		public static int CheckIndex(int effectId, byte index)
		{
			SpecialEffectItem config = SpecialEffect.Instance[effectId];
			bool flag = config.ShortDesc.Length <= (int)index;
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				result = (int)index;
			}
			return result;
		}

		// Token: 0x0600673A RID: 26426 RVA: 0x003B0AB4 File Offset: 0x003AECB4
		public ShowSpecialEffectDisplayData(int charId, int effectId, int index, ItemKey itemData)
		{
			this.Index = index;
			this.EffectId = effectId;
			this.ItemData = itemData;
			short skillTemplateId = SpecialEffect.Instance[effectId].SkillTemplateId;
			this.EffectDescription = DomainManager.CombatSkill.GetEffectDisplayData(charId, skillTemplateId);
		}

		// Token: 0x0600673B RID: 26427 RVA: 0x003B0AFC File Offset: 0x003AECFC
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x0600673C RID: 26428 RVA: 0x003B0B10 File Offset: 0x003AED10
		public int GetSerializedSize()
		{
			int totalSize = 16;
			totalSize += this.EffectDescription.GetSerializedSize();
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600673D RID: 26429 RVA: 0x003B0B44 File Offset: 0x003AED44
		public unsafe int Serialize(byte* pData)
		{
			*(int*)pData = this.Index;
			byte* pCurrData = pData + 4;
			*(int*)pCurrData = this.EffectId;
			pCurrData += 4;
			pCurrData += this.ItemData.Serialize(pCurrData);
			int fieldSize = this.EffectDescription.Serialize(pCurrData);
			pCurrData += fieldSize;
			Tester.Assert(fieldSize <= 65535, "");
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600673E RID: 26430 RVA: 0x003B0BC4 File Offset: 0x003AEDC4
		public unsafe int Deserialize(byte* pData)
		{
			this.Index = *(int*)pData;
			byte* pCurrData = pData + 4;
			this.EffectId = *(int*)pCurrData;
			pCurrData += 4;
			pCurrData += this.ItemData.Deserialize(pCurrData);
			pCurrData += this.EffectDescription.Deserialize(pCurrData);
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04001C19 RID: 7193
		public static readonly ShowSpecialEffectDisplayData Invalid = new ShowSpecialEffectDisplayData
		{
			Index = -1,
			ItemData = ItemKey.Invalid,
			EffectDescription = CombatSkillEffectDescriptionDisplayData.Invalid
		};

		// Token: 0x04001C1A RID: 7194
		[SerializableGameDataField]
		public int Index;

		// Token: 0x04001C1B RID: 7195
		[SerializableGameDataField]
		public int EffectId;

		// Token: 0x04001C1C RID: 7196
		[SerializableGameDataField]
		public ItemKey ItemData;

		// Token: 0x04001C1D RID: 7197
		[SerializableGameDataField]
		public CombatSkillEffectDescriptionDisplayData EffectDescription;
	}
}
