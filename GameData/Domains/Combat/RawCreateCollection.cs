using System;
using System.Collections.Generic;
using GameData.Domains.Item;
using GameData.Serializer;

namespace GameData.Domains.Combat
{
	// Token: 0x020006CE RID: 1742
	[SerializableGameData(NotForArchive = true)]
	public class RawCreateCollection : ISerializableGameData
	{
		// Token: 0x06006720 RID: 26400 RVA: 0x003B055F File Offset: 0x003AE75F
		public RawCreateCollection()
		{
		}

		// Token: 0x06006721 RID: 26401 RVA: 0x003B058C File Offset: 0x003AE78C
		public RawCreateCollection(RawCreateCollection other)
		{
			this.Effects = ((other.Effects == null) ? null : new Dictionary<ItemKey, int>(other.Effects));
		}

		// Token: 0x06006722 RID: 26402 RVA: 0x003B05DE File Offset: 0x003AE7DE
		public void Assign(RawCreateCollection other)
		{
			this.Effects = ((other.Effects == null) ? null : new Dictionary<ItemKey, int>(other.Effects));
		}

		// Token: 0x06006723 RID: 26403 RVA: 0x003B0600 File Offset: 0x003AE800
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06006724 RID: 26404 RVA: 0x003B0614 File Offset: 0x003AE814
		public int GetSerializedSize()
		{
			int totalSize = 0;
			totalSize += SerializationHelper.DictionaryOfCustomTypeBasicTypePair.GetSerializedSize<ItemKey, int>(this.Effects);
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06006725 RID: 26405 RVA: 0x003B0648 File Offset: 0x003AE848
		public unsafe int Serialize(byte* pData)
		{
			byte* pCurrData = pData + SerializationHelper.DictionaryOfCustomTypeBasicTypePair.Serialize<ItemKey, int>(pData, ref this.Effects);
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06006726 RID: 26406 RVA: 0x003B0684 File Offset: 0x003AE884
		public unsafe int Deserialize(byte* pData)
		{
			byte* pCurrData = pData + SerializationHelper.DictionaryOfCustomTypeBasicTypePair.Deserialize<ItemKey, int>(pData, ref this.Effects);
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06006727 RID: 26407 RVA: 0x003B06C0 File Offset: 0x003AE8C0
		public int TryGetEffectId(ItemKey key)
		{
			int effectId;
			return this.Effects.TryGetValue(key, out effectId) ? effectId : -1;
		}

		// Token: 0x06006728 RID: 26408 RVA: 0x003B06E1 File Offset: 0x003AE8E1
		public bool EffectEquals(ItemKey lhs, ItemKey rhs)
		{
			return this.TryGetEffectId(lhs) == this.TryGetEffectId(rhs);
		}

		// Token: 0x06006729 RID: 26409 RVA: 0x003B06F3 File Offset: 0x003AE8F3
		public bool Any()
		{
			return this.Effects.Count > 0 || this.Sources.Count > 0;
		}

		// Token: 0x0600672A RID: 26410 RVA: 0x003B0714 File Offset: 0x003AE914
		public bool Contains(ItemKey newKey)
		{
			return this.Effects.ContainsKey(newKey);
		}

		// Token: 0x0600672B RID: 26411 RVA: 0x003B0722 File Offset: 0x003AE922
		public void Add(ItemKey newKey, ItemKey oldKey, int effectId, long specialEffectId)
		{
			this.Effects.Add(newKey, effectId);
			this.Sources.Add(newKey, oldKey);
			this.SpecialEffects.Add(newKey, specialEffectId);
		}

		// Token: 0x0600672C RID: 26412 RVA: 0x003B0750 File Offset: 0x003AE950
		public void Remove(ItemKey newKey, out ItemKey oldKey)
		{
			oldKey = this.Sources[newKey];
			this.Effects.Remove(newKey);
			this.Sources.Remove(newKey);
			this.SpecialEffects.Remove(newKey);
		}

		// Token: 0x0600672D RID: 26413 RVA: 0x003B078C File Offset: 0x003AE98C
		public void Clear()
		{
			this.Effects.Clear();
			this.Sources.Clear();
			this.SpecialEffects.Clear();
		}

		// Token: 0x04001C13 RID: 7187
		[SerializableGameDataField]
		public Dictionary<ItemKey, int> Effects = new Dictionary<ItemKey, int>();

		// Token: 0x04001C14 RID: 7188
		public readonly Dictionary<ItemKey, ItemKey> Sources = new Dictionary<ItemKey, ItemKey>();

		// Token: 0x04001C15 RID: 7189
		public readonly Dictionary<ItemKey, long> SpecialEffects = new Dictionary<ItemKey, long>();
	}
}
