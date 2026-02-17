using System;
using System.Collections.Generic;
using GameData.Dependencies;

namespace GameData.Common
{
	// Token: 0x020008F8 RID: 2296
	public class ObjectCollectionDataStates
	{
		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x0600823A RID: 33338 RVA: 0x004D9E82 File Offset: 0x004D8082
		public int Count
		{
			get
			{
				return this._nextMaxOffset / this._uintSize - this._unoccupiedOffsets.Count;
			}
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x0600823B RID: 33339 RVA: 0x004D9E9D File Offset: 0x004D809D
		public int Capacity
		{
			get
			{
				return this._dataStates.Length / this._uintSize;
			}
		}

		// Token: 0x0600823C RID: 33340 RVA: 0x004D9EB0 File Offset: 0x004D80B0
		public ObjectCollectionDataStates(int fieldsCount, int capacity)
		{
			bool flag = fieldsCount <= 0;
			if (flag)
			{
				throw new ArgumentOutOfRangeException("fieldsCount", "FieldsCount must be greater than zero");
			}
			bool flag2 = capacity < 0;
			if (flag2)
			{
				throw new ArgumentOutOfRangeException("capacity", "Capacity cannot be less than zero");
			}
			this._uintSize = (fieldsCount + 3) / 4;
			this._dataStates = ((capacity > 0) ? new byte[this._uintSize * capacity] : ObjectCollectionDataStates.EmptyArray);
			this._nextMaxOffset = 0;
			this._unoccupiedOffsets = new Stack<int>(capacity / 10);
		}

		// Token: 0x0600823D RID: 33341 RVA: 0x004D9F38 File Offset: 0x004D8138
		public int Create()
		{
			bool flag = this._unoccupiedOffsets.Count > 0;
			int offset;
			if (flag)
			{
				offset = this._unoccupiedOffsets.Pop();
			}
			else
			{
				this.EnsureCapacity(this._nextMaxOffset + this._uintSize);
				offset = this._nextMaxOffset;
				this._nextMaxOffset += this._uintSize;
			}
			Array.Clear(this._dataStates, offset, this._uintSize);
			return offset;
		}

		// Token: 0x0600823E RID: 33342 RVA: 0x004D9FB0 File Offset: 0x004D81B0
		public void Remove(int offset)
		{
			bool flag = offset < 0;
			if (!flag)
			{
				bool flag2 = offset == this._nextMaxOffset - this._uintSize;
				if (flag2)
				{
					this._nextMaxOffset -= this._uintSize;
				}
				else
				{
					this._unoccupiedOffsets.Push(offset);
				}
			}
		}

		// Token: 0x0600823F RID: 33343 RVA: 0x004D9FFE File Offset: 0x004D81FE
		public void Clear()
		{
			this._nextMaxOffset = 0;
			this._unoccupiedOffsets.Clear();
		}

		// Token: 0x06008240 RID: 33344 RVA: 0x004DA014 File Offset: 0x004D8214
		public bool IsCached(int offset, int fieldId)
		{
			return ((int)this._dataStates[offset + fieldId / 4] & 1 << fieldId % 4 * 2) != 0;
		}

		// Token: 0x06008241 RID: 33345 RVA: 0x004DA040 File Offset: 0x004D8240
		public void SetCached(int offset, int fieldId)
		{
			byte[] dataStates = this._dataStates;
			int num = offset + fieldId / 4;
			dataStates[num] |= (byte)(1 << fieldId % 4 * 2);
		}

		// Token: 0x06008242 RID: 33346 RVA: 0x004DA063 File Offset: 0x004D8263
		public void ResetCached(int offset, int fieldId)
		{
			byte[] dataStates = this._dataStates;
			int num = offset + fieldId / 4;
			dataStates[num] &= (byte)(~(byte)(1 << fieldId % 4 * 2));
		}

		// Token: 0x06008243 RID: 33347 RVA: 0x004DA088 File Offset: 0x004D8288
		public bool IsModified(int offset, int fieldId)
		{
			return ((int)this._dataStates[offset + fieldId / 4] & 2 << fieldId % 4 * 2) != 0;
		}

		// Token: 0x06008244 RID: 33348 RVA: 0x004DA0B4 File Offset: 0x004D82B4
		public void SetModified(int offset, int fieldId)
		{
			byte[] dataStates = this._dataStates;
			int num = offset + fieldId / 4;
			dataStates[num] |= (byte)(2 << fieldId % 4 * 2);
		}

		// Token: 0x06008245 RID: 33349 RVA: 0x004DA0D7 File Offset: 0x004D82D7
		public void ResetModified(int offset, int fieldId)
		{
			byte[] dataStates = this._dataStates;
			int num = offset + fieldId / 4;
			dataStates[num] &= (byte)(~(byte)(2 << fieldId % 4 * 2));
		}

		// Token: 0x06008246 RID: 33350 RVA: 0x004DA0FC File Offset: 0x004D82FC
		public void InvalidateAll(DataInfluence influence)
		{
			List<DataUid> targetUids = influence.TargetUids;
			int targetUidsCount = targetUids.Count;
			for (int offset = 0; offset < this._nextMaxOffset; offset += this._uintSize)
			{
				for (int i = 0; i < targetUidsCount; i++)
				{
					ushort fieldId = (ushort)targetUids[i].SubId1;
					int outerIndex = offset + (int)(fieldId / 4);
					int innerIndex = (int)(fieldId % 4 * 2);
					int state = ((int)this._dataStates[outerIndex] & ~(3 << innerIndex)) | 2 << innerIndex;
					this._dataStates[outerIndex] = (byte)state;
				}
			}
		}

		// Token: 0x06008247 RID: 33351 RVA: 0x004DA194 File Offset: 0x004D8394
		private void EnsureCapacity(int min)
		{
			bool flag = this._dataStates.Length >= min;
			if (!flag)
			{
				int newCapacity = (this._dataStates.Length == 0) ? (16 * this._uintSize) : (this._dataStates.Length * 2);
				bool flag2 = newCapacity > int.MaxValue;
				if (flag2)
				{
					newCapacity = int.MaxValue;
				}
				bool flag3 = newCapacity < min;
				if (flag3)
				{
					newCapacity = min;
				}
				byte[] newDataStates = new byte[newCapacity];
				bool flag4 = this._nextMaxOffset > 0;
				if (flag4)
				{
					Array.Copy(this._dataStates, newDataStates, this._nextMaxOffset);
				}
				this._dataStates = newDataStates;
			}
		}

		// Token: 0x0400243B RID: 9275
		public const int InvalidOffset = -1;

		// Token: 0x0400243C RID: 9276
		private const int DefaultObjectCapacity = 16;

		// Token: 0x0400243D RID: 9277
		private static readonly byte[] EmptyArray = Array.Empty<byte>();

		// Token: 0x0400243E RID: 9278
		private readonly int _uintSize;

		// Token: 0x0400243F RID: 9279
		private byte[] _dataStates;

		// Token: 0x04002440 RID: 9280
		private int _nextMaxOffset;

		// Token: 0x04002441 RID: 9281
		private readonly Stack<int> _unoccupiedOffsets;
	}
}
