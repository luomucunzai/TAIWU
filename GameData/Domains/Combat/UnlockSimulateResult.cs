using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat
{
	// Token: 0x02000707 RID: 1799
	[SerializableGameData(NotForArchive = true)]
	public class UnlockSimulateResult : ISerializableGameData
	{
		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x06006805 RID: 26629 RVA: 0x003B338C File Offset: 0x003B158C
		public bool AllBlocked
		{
			get
			{
				List<int> list = this._blockedRawCreateEffects;
				bool result;
				if (list != null && list.Count > 0)
				{
					list = this._rawCreateEffects;
					result = (list == null || list.Count <= 0);
				}
				else
				{
					result = false;
				}
				return result;
			}
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x06006806 RID: 26630 RVA: 0x003B33C8 File Offset: 0x003B15C8
		public IEnumerable<int> AllRawCreateEffects
		{
			get
			{
				bool flag = this._rawCreateEffects != null;
				if (flag)
				{
					foreach (int effect in this._rawCreateEffects)
					{
						yield return effect;
					}
					List<int>.Enumerator enumerator = default(List<int>.Enumerator);
				}
				bool flag2 = this._blockedRawCreateEffects != null;
				if (flag2)
				{
					foreach (int effect2 in this._blockedRawCreateEffects)
					{
						yield return effect2;
					}
					List<int>.Enumerator enumerator2 = default(List<int>.Enumerator);
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06006807 RID: 26631 RVA: 0x003B33E7 File Offset: 0x003B15E7
		public IReadOnlyList<int> BlockedRawCreateEffects
		{
			get
			{
				return this._blockedRawCreateEffects;
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06006808 RID: 26632 RVA: 0x003B33F0 File Offset: 0x003B15F0
		public int AllRawCreateEffectsCount
		{
			get
			{
				List<int> rawCreateEffects = this._rawCreateEffects;
				int result;
				if (rawCreateEffects == null)
				{
					List<int> blockedRawCreateEffects = this._blockedRawCreateEffects;
					int? num = (blockedRawCreateEffects != null) ? new int?(blockedRawCreateEffects.Count) : null;
					result = ((num != null) ? new int?(num.GetValueOrDefault()) : null).GetValueOrDefault();
				}
				else
				{
					result = rawCreateEffects.Count;
				}
				return result;
			}
		}

		// Token: 0x06006809 RID: 26633 RVA: 0x003B3458 File Offset: 0x003B1658
		public UnlockSimulateResult(IEnumerable<int> rawCreateEffects, Func<int, bool> blockedChecker)
		{
			this._rawCreateEffects = new List<int>(rawCreateEffects);
			this._blockedRawCreateEffects = new List<int>(this._rawCreateEffects.Where(blockedChecker));
			this._rawCreateEffects.RemoveAll(new Predicate<int>(this._blockedRawCreateEffects.Contains));
		}

		// Token: 0x0600680A RID: 26634 RVA: 0x003B34AE File Offset: 0x003B16AE
		public UnlockSimulateResult()
		{
		}

		// Token: 0x0600680B RID: 26635 RVA: 0x003B34B8 File Offset: 0x003B16B8
		public UnlockSimulateResult(UnlockSimulateResult other)
		{
			this._rawCreateEffects = ((other._rawCreateEffects == null) ? null : new List<int>(other._rawCreateEffects));
			this._blockedRawCreateEffects = ((other._blockedRawCreateEffects == null) ? null : new List<int>(other._blockedRawCreateEffects));
		}

		// Token: 0x0600680C RID: 26636 RVA: 0x003B3505 File Offset: 0x003B1705
		public void Assign(UnlockSimulateResult other)
		{
			this._rawCreateEffects = ((other._rawCreateEffects == null) ? null : new List<int>(other._rawCreateEffects));
			this._blockedRawCreateEffects = ((other._blockedRawCreateEffects == null) ? null : new List<int>(other._blockedRawCreateEffects));
		}

		// Token: 0x0600680D RID: 26637 RVA: 0x003B3540 File Offset: 0x003B1740
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x0600680E RID: 26638 RVA: 0x003B3554 File Offset: 0x003B1754
		public int GetSerializedSize()
		{
			int totalSize = 0;
			bool flag = this._rawCreateEffects != null;
			if (flag)
			{
				totalSize += 2 + 4 * this._rawCreateEffects.Count;
			}
			else
			{
				totalSize += 2;
			}
			bool flag2 = this._blockedRawCreateEffects != null;
			if (flag2)
			{
				totalSize += 2 + 4 * this._blockedRawCreateEffects.Count;
			}
			else
			{
				totalSize += 2;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600680F RID: 26639 RVA: 0x003B35C4 File Offset: 0x003B17C4
		public unsafe int Serialize(byte* pData)
		{
			bool flag = this._rawCreateEffects != null;
			byte* pCurrData;
			if (flag)
			{
				int elementsCount = this._rawCreateEffects.Count;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pData = (short)((ushort)elementsCount);
				pCurrData = pData + 2;
				for (int i = 0; i < elementsCount; i++)
				{
					*(int*)(pCurrData + (IntPtr)i * 4) = this._rawCreateEffects[i];
				}
				pCurrData += 4 * elementsCount;
			}
			else
			{
				*(short*)pData = 0;
				pCurrData = pData + 2;
			}
			bool flag2 = this._blockedRawCreateEffects != null;
			if (flag2)
			{
				int elementsCount2 = this._blockedRawCreateEffects.Count;
				Tester.Assert(elementsCount2 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount2);
				pCurrData += 2;
				for (int j = 0; j < elementsCount2; j++)
				{
					*(int*)(pCurrData + (IntPtr)j * 4) = this._blockedRawCreateEffects[j];
				}
				pCurrData += 4 * elementsCount2;
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06006810 RID: 26640 RVA: 0x003B36E8 File Offset: 0x003B18E8
		public unsafe int Deserialize(byte* pData)
		{
			ushort elementsCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = elementsCount > 0;
			if (flag)
			{
				bool flag2 = this._rawCreateEffects == null;
				if (flag2)
				{
					this._rawCreateEffects = new List<int>((int)elementsCount);
				}
				else
				{
					this._rawCreateEffects.Clear();
				}
				for (int i = 0; i < (int)elementsCount; i++)
				{
					this._rawCreateEffects.Add(*(int*)(pCurrData + (IntPtr)i * 4));
				}
				pCurrData += 4 * elementsCount;
			}
			else
			{
				List<int> rawCreateEffects = this._rawCreateEffects;
				if (rawCreateEffects != null)
				{
					rawCreateEffects.Clear();
				}
			}
			ushort elementsCount2 = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag3 = elementsCount2 > 0;
			if (flag3)
			{
				bool flag4 = this._blockedRawCreateEffects == null;
				if (flag4)
				{
					this._blockedRawCreateEffects = new List<int>((int)elementsCount2);
				}
				else
				{
					this._blockedRawCreateEffects.Clear();
				}
				for (int j = 0; j < (int)elementsCount2; j++)
				{
					this._blockedRawCreateEffects.Add(*(int*)(pCurrData + (IntPtr)j * 4));
				}
				pCurrData += 4 * elementsCount2;
			}
			else
			{
				List<int> blockedRawCreateEffects = this._blockedRawCreateEffects;
				if (blockedRawCreateEffects != null)
				{
					blockedRawCreateEffects.Clear();
				}
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04001C77 RID: 7287
		[SerializableGameDataField]
		private List<int> _rawCreateEffects;

		// Token: 0x04001C78 RID: 7288
		[SerializableGameDataField]
		private List<int> _blockedRawCreateEffects;
	}
}
