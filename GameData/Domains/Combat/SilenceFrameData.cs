using System;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat
{
	// Token: 0x020006D3 RID: 1747
	[SerializableGameData(NotForArchive = true)]
	public struct SilenceFrameData : ISerializableGameData
	{
		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06006747 RID: 26439 RVA: 0x003B109B File Offset: 0x003AF29B
		public int TotalFrame
		{
			get
			{
				return this._totalFrame;
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06006748 RID: 26440 RVA: 0x003B10A3 File Offset: 0x003AF2A3
		public int LeftFrame
		{
			get
			{
				return this._leftFrame;
			}
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06006749 RID: 26441 RVA: 0x003B10AB File Offset: 0x003AF2AB
		public float LeftProgress
		{
			get
			{
				return this.Infinity ? 1f : (this.NotInSilencing ? 0f : ((float)this.LeftFrame / (float)MathUtils.Max(this.TotalFrame, 1)));
			}
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x0600674A RID: 26442 RVA: 0x003B10E0 File Offset: 0x003AF2E0
		public bool Silencing
		{
			get
			{
				return this.LeftFrame != 0;
			}
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x0600674B RID: 26443 RVA: 0x003B10EB File Offset: 0x003AF2EB
		public bool NotInSilencing
		{
			get
			{
				return this.LeftFrame == 0;
			}
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x0600674C RID: 26444 RVA: 0x003B10F6 File Offset: 0x003AF2F6
		public bool Infinity
		{
			get
			{
				return this.LeftFrame < 0;
			}
		}

		// Token: 0x0600674D RID: 26445 RVA: 0x003B1104 File Offset: 0x003AF304
		public static SilenceFrameData Create(int total)
		{
			return new SilenceFrameData
			{
				_totalFrame = total,
				_leftFrame = total
			};
		}

		// Token: 0x0600674E RID: 26446 RVA: 0x003B1130 File Offset: 0x003AF330
		public static SilenceFrameData Create(int total, int left)
		{
			return new SilenceFrameData
			{
				_totalFrame = total,
				_leftFrame = left
			};
		}

		// Token: 0x0600674F RID: 26447 RVA: 0x003B115C File Offset: 0x003AF35C
		public bool Cover(int newFrame)
		{
			bool infinity = this.Infinity;
			bool result;
			if (infinity)
			{
				result = false;
			}
			else
			{
				bool flag = this.NotInSilencing || newFrame < 0 || newFrame >= this.TotalFrame;
				if (flag)
				{
					this._leftFrame = newFrame;
					this._totalFrame = newFrame;
					result = true;
				}
				else
				{
					bool flag2 = newFrame > this.LeftFrame;
					if (flag2)
					{
						this._leftFrame = newFrame;
						result = true;
					}
					else
					{
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x06006750 RID: 26448 RVA: 0x003B11CC File Offset: 0x003AF3CC
		public bool Tick(int deltaFrame = 1)
		{
			bool flag = this.Infinity || this.NotInSilencing;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this._leftFrame -= deltaFrame;
				result = true;
			}
			return result;
		}

		// Token: 0x06006751 RID: 26449 RVA: 0x003B1208 File Offset: 0x003AF408
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x06006752 RID: 26450 RVA: 0x003B121C File Offset: 0x003AF41C
		public int GetSerializedSize()
		{
			int totalSize = 8;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06006753 RID: 26451 RVA: 0x003B1240 File Offset: 0x003AF440
		public unsafe int Serialize(byte* pData)
		{
			*(int*)pData = this._totalFrame;
			byte* pCurrData = pData + 4;
			*(int*)pCurrData = this._leftFrame;
			pCurrData += 4;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06006754 RID: 26452 RVA: 0x003B1284 File Offset: 0x003AF484
		public unsafe int Deserialize(byte* pData)
		{
			this._totalFrame = *(int*)pData;
			byte* pCurrData = pData + 4;
			this._leftFrame = *(int*)pCurrData;
			pCurrData += 4;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04001C21 RID: 7201
		[SerializableGameDataField]
		private int _totalFrame;

		// Token: 0x04001C22 RID: 7202
		[SerializableGameDataField]
		private int _leftFrame;
	}
}
