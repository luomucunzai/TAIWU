using System;
using System.Collections.Generic;
using GameData.Domains.Taiwu.LifeSkillCombat.Status;
using GameData.Serializer;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Snapshot
{
	// Token: 0x0200005E RID: 94
	[SerializableGameData(NotForDisplayModule = true)]
	public class StatusSnapshotDiff : ISerializableGameData
	{
		// Token: 0x0600156D RID: 5485 RVA: 0x00149FE0 File Offset: 0x001481E0
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x00149FE4 File Offset: 0x001481E4
		public int GetSerializedSize()
		{
			int totalSize = 0;
			totalSize += this.GridStatusDiffPrevious.GetSerializedSize();
			totalSize += this.GridStatusDiffCurrent.GetSerializedSize();
			bool flag = this.PlayerSelfDiffPrevious != null;
			if (flag)
			{
				totalSize++;
				totalSize += this.PlayerSelfDiffPrevious.GetSerializedSize();
			}
			else
			{
				totalSize++;
			}
			bool flag2 = this.PlayerSelfDiffCurrent != null;
			if (flag2)
			{
				totalSize++;
				totalSize += this.PlayerSelfDiffCurrent.GetSerializedSize();
			}
			else
			{
				totalSize++;
			}
			bool flag3 = this.PlayerAdversaryDiffPrevious != null;
			if (flag3)
			{
				totalSize++;
				totalSize += this.PlayerAdversaryDiffPrevious.GetSerializedSize();
			}
			else
			{
				totalSize++;
			}
			bool flag4 = this.PlayerAdversaryDiffCurrent != null;
			if (flag4)
			{
				totalSize++;
				totalSize += this.PlayerAdversaryDiffCurrent.GetSerializedSize();
			}
			else
			{
				totalSize++;
			}
			totalSize += 4;
			totalSize += 4;
			totalSize++;
			totalSize++;
			totalSize += 2;
			foreach (StatusSnapshotDiff.BookStateExtraDiff unit in this.BookStateExtraDiffList)
			{
				totalSize += unit.GetSerializedSize();
			}
			totalSize += 2;
			foreach (StatusSnapshotDiff.GridTrapStateExtraDiff unit2 in this.GridTrapStateExtraDiffList)
			{
				totalSize += unit2.GetSerializedSize();
			}
			totalSize++;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x0014A180 File Offset: 0x00148380
		public unsafe int Serialize(byte* pData)
		{
			byte* pCurrData = pData;
			pCurrData += this.GridStatusDiffPrevious.Serialize(pCurrData);
			pCurrData += this.GridStatusDiffCurrent.Serialize(pCurrData);
			bool flag = this.PlayerSelfDiffPrevious != null;
			if (flag)
			{
				*pCurrData = 1;
				pCurrData++;
				pCurrData += this.PlayerSelfDiffPrevious.Serialize(pCurrData);
			}
			else
			{
				*pCurrData = 0;
				pCurrData++;
			}
			bool flag2 = this.PlayerSelfDiffCurrent != null;
			if (flag2)
			{
				*pCurrData = 1;
				pCurrData++;
				pCurrData += this.PlayerSelfDiffCurrent.Serialize(pCurrData);
			}
			else
			{
				*pCurrData = 0;
				pCurrData++;
			}
			bool flag3 = this.PlayerAdversaryDiffPrevious != null;
			if (flag3)
			{
				*pCurrData = 1;
				pCurrData++;
				pCurrData += this.PlayerAdversaryDiffPrevious.Serialize(pCurrData);
			}
			else
			{
				*pCurrData = 0;
				pCurrData++;
			}
			bool flag4 = this.PlayerAdversaryDiffCurrent != null;
			if (flag4)
			{
				*pCurrData = 1;
				pCurrData++;
				pCurrData += this.PlayerAdversaryDiffCurrent.Serialize(pCurrData);
			}
			else
			{
				*pCurrData = 0;
				pCurrData++;
			}
			bool flag5 = this.ScoreSelfDiff != null;
			if (flag5)
			{
				*(int*)pCurrData = this.ScoreSelfDiff.Value;
				pCurrData += 4;
			}
			else
			{
				*(int*)pCurrData = -1;
				pCurrData += 4;
			}
			bool flag6 = this.ScoreAdversaryDiff != null;
			if (flag6)
			{
				*(int*)pCurrData = this.ScoreAdversaryDiff.Value;
				pCurrData += 4;
			}
			else
			{
				*(int*)pCurrData = -1;
				pCurrData += 4;
			}
			bool flag7 = this.CurrentPlayerIdDiff != null;
			if (flag7)
			{
				*pCurrData = (byte)this.CurrentPlayerIdDiff.Value;
				pCurrData++;
			}
			else
			{
				*pCurrData = (byte)-1;
				pCurrData++;
			}
			bool flag8 = this.WinnerPlayerId != null;
			if (flag8)
			{
				*pCurrData = (byte)this.WinnerPlayerId.Value;
				pCurrData++;
			}
			else
			{
				*pCurrData = (byte)-1;
				pCurrData++;
			}
			*(short*)pCurrData = (short)((ushort)this.BookStateExtraDiffList.Count);
			pCurrData += 2;
			foreach (StatusSnapshotDiff.BookStateExtraDiff unit in this.BookStateExtraDiffList)
			{
				pCurrData += unit.Serialize(pCurrData);
			}
			*(short*)pCurrData = (short)((ushort)this.GridTrapStateExtraDiffList.Count);
			pCurrData += 2;
			foreach (StatusSnapshotDiff.GridTrapStateExtraDiff unit2 in this.GridTrapStateExtraDiffList)
			{
				pCurrData += unit2.Serialize(pCurrData);
			}
			*pCurrData = (this.LoserPlayerIsForced ? 1 : 0);
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001570 RID: 5488 RVA: 0x0014A41C File Offset: 0x0014861C
		public unsafe int Deserialize(byte* pData)
		{
			if (this.GridStatusDiffPrevious == null)
			{
				this.GridStatusDiffPrevious = new GridList();
			}
			if (this.GridStatusDiffCurrent == null)
			{
				this.GridStatusDiffCurrent = new GridList();
			}
			byte* pCurrData = pData + this.GridStatusDiffPrevious.Deserialize(pData);
			pCurrData += this.GridStatusDiffCurrent.Deserialize(pCurrData);
			this.PlayerSelfDiffPrevious = null;
			bool flag = *pCurrData > 0;
			if (flag)
			{
				pCurrData++;
				this.PlayerSelfDiffPrevious = new Player();
				pCurrData += this.PlayerSelfDiffPrevious.Deserialize(pCurrData);
			}
			else
			{
				pCurrData++;
			}
			this.PlayerSelfDiffCurrent = null;
			bool flag2 = *pCurrData > 0;
			if (flag2)
			{
				pCurrData++;
				this.PlayerSelfDiffCurrent = new Player();
				pCurrData += this.PlayerSelfDiffCurrent.Deserialize(pCurrData);
			}
			else
			{
				pCurrData++;
			}
			this.PlayerAdversaryDiffPrevious = null;
			bool flag3 = *pCurrData > 0;
			if (flag3)
			{
				pCurrData++;
				this.PlayerAdversaryDiffPrevious = new Player();
				pCurrData += this.PlayerAdversaryDiffPrevious.Deserialize(pCurrData);
			}
			else
			{
				pCurrData++;
			}
			this.PlayerAdversaryDiffCurrent = null;
			bool flag4 = *pCurrData > 0;
			if (flag4)
			{
				pCurrData++;
				this.PlayerAdversaryDiffCurrent = new Player();
				pCurrData += this.PlayerAdversaryDiffCurrent.Deserialize(pCurrData);
			}
			else
			{
				pCurrData++;
			}
			this.ScoreSelfDiff = new int?(*(int*)pCurrData);
			pCurrData += 4;
			int? num = this.ScoreSelfDiff;
			int num2 = 0;
			bool flag5 = num.GetValueOrDefault() < num2 & num != null;
			if (flag5)
			{
				this.ScoreSelfDiff = null;
			}
			this.ScoreAdversaryDiff = new int?(*(int*)pCurrData);
			pCurrData += 4;
			num = this.ScoreAdversaryDiff;
			num2 = 0;
			bool flag6 = num.GetValueOrDefault() < num2 & num != null;
			if (flag6)
			{
				this.ScoreAdversaryDiff = null;
			}
			this.CurrentPlayerIdDiff = new sbyte?(*(sbyte*)pCurrData);
			pCurrData++;
			sbyte? b = this.CurrentPlayerIdDiff;
			num = ((b != null) ? new int?((int)b.GetValueOrDefault()) : null);
			num2 = -1;
			bool flag7 = num.GetValueOrDefault() == num2 & num != null;
			if (flag7)
			{
				this.CurrentPlayerIdDiff = null;
			}
			this.WinnerPlayerId = new sbyte?(*(sbyte*)pCurrData);
			pCurrData++;
			b = this.WinnerPlayerId;
			num = ((b != null) ? new int?((int)b.GetValueOrDefault()) : null);
			num2 = -1;
			bool flag8 = num.GetValueOrDefault() == num2 & num != null;
			if (flag8)
			{
				this.WinnerPlayerId = null;
			}
			ushort count = *(ushort*)pCurrData;
			pCurrData += 2;
			this.BookStateExtraDiffList.Clear();
			for (int i = 0; i < (int)count; i++)
			{
				StatusSnapshotDiff.BookStateExtraDiff unit = default(StatusSnapshotDiff.BookStateExtraDiff);
				pCurrData += unit.Deserialize(pCurrData);
				this.BookStateExtraDiffList.Add(unit);
			}
			ushort count2 = *(ushort*)pCurrData;
			pCurrData += 2;
			this.GridTrapStateExtraDiffList.Clear();
			for (int j = 0; j < (int)count2; j++)
			{
				StatusSnapshotDiff.GridTrapStateExtraDiff unit2 = default(StatusSnapshotDiff.GridTrapStateExtraDiff);
				pCurrData += unit2.Deserialize(pCurrData);
				this.GridTrapStateExtraDiffList.Add(unit2);
			}
			this.LoserPlayerIsForced = (*pCurrData != 0);
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x0014A764 File Offset: 0x00148964
		private static void Ensure(ref byte[] buf, int size)
		{
			bool flag = buf.Length < size;
			if (flag)
			{
				buf = new byte[size * 2];
			}
		}

		// Token: 0x06001572 RID: 5490 RVA: 0x0014A788 File Offset: 0x00148988
		private unsafe static bool BufferEquals(byte* pData, byte* pData2, int size)
		{
			for (int i = 0; i < size; i++)
			{
				bool flag = pData[i] != pData2[i];
				if (flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001573 RID: 5491 RVA: 0x0014A7C4 File Offset: 0x001489C4
		public unsafe StatusSnapshotDiff(in StatusSnapshot previous, in StatusSnapshot current)
		{
			byte[] aBuf = new byte[1024];
			byte[] bBuf = new byte[1024];
			this.GridStatusDiffPrevious = new GridList();
			this.GridStatusDiffCurrent = new GridList();
			for (int i = 0; i < 49; i++)
			{
				bool isDiff = false;
				Grid a = previous.GridStatus[i];
				Grid b = current.GridStatus[i];
				int aSize = a.GetSerializedSize();
				int bSize = b.GetSerializedSize();
				bool flag = aSize != bSize;
				if (flag)
				{
					byte[] array;
					byte* pData;
					if ((array = aBuf) == null || array.Length == 0)
					{
						pData = null;
					}
					else
					{
						pData = &array[0];
					}
					byte[] array2;
					byte* pData2;
					if ((array2 = bBuf) == null || array2.Length == 0)
					{
						pData2 = null;
					}
					else
					{
						pData2 = &array2[0];
					}
					a.Serialize(pData);
					b.Serialize(pData2);
					array = null;
					array2 = null;
					isDiff = true;
				}
				bool flag2 = !isDiff;
				if (flag2)
				{
					StatusSnapshotDiff.Ensure(ref aBuf, aSize);
					StatusSnapshotDiff.Ensure(ref bBuf, bSize);
					Array.Fill<byte>(aBuf, 0, 0, aBuf.Length);
					Array.Fill<byte>(bBuf, 0, 0, bBuf.Length);
					byte[] array2;
					byte* pData3;
					if ((array2 = aBuf) == null || array2.Length == 0)
					{
						pData3 = null;
					}
					else
					{
						pData3 = &array2[0];
					}
					byte[] array;
					byte* pData4;
					if ((array = bBuf) == null || array.Length == 0)
					{
						pData4 = null;
					}
					else
					{
						pData4 = &array[0];
					}
					a.Serialize(pData3);
					b.Serialize(pData4);
					isDiff = !StatusSnapshotDiff.BufferEquals(pData3, pData4, aSize);
					array2 = null;
					array = null;
				}
				bool flag3 = isDiff;
				if (flag3)
				{
					byte[] array;
					byte* pData5;
					if ((array = aBuf) == null || array.Length == 0)
					{
						pData5 = null;
					}
					else
					{
						pData5 = &array[0];
					}
					byte[] array2;
					byte* pData6;
					if ((array2 = bBuf) == null || array2.Length == 0)
					{
						pData6 = null;
					}
					else
					{
						pData6 = &array2[0];
					}
					a = new Grid();
					b = new Grid();
					a.Deserialize(pData5);
					b.Deserialize(pData6);
					this.GridStatusDiffPrevious.Add(a);
					this.GridStatusDiffCurrent.Add(b);
					array = null;
					array2 = null;
				}
			}
			bool isDiff2 = false;
			Player a2 = previous.Self;
			Player b2 = current.Self;
			int aSize2 = a2.GetSerializedSize();
			int bSize2 = b2.GetSerializedSize();
			bool flag4 = aSize2 != bSize2;
			if (flag4)
			{
				byte[] array2;
				byte* pData7;
				if ((array2 = aBuf) == null || array2.Length == 0)
				{
					pData7 = null;
				}
				else
				{
					pData7 = &array2[0];
				}
				byte[] array;
				byte* pData8;
				if ((array = bBuf) == null || array.Length == 0)
				{
					pData8 = null;
				}
				else
				{
					pData8 = &array[0];
				}
				a2.Serialize(pData7);
				b2.Serialize(pData8);
				array2 = null;
				array = null;
				isDiff2 = true;
			}
			bool flag5 = !isDiff2;
			if (flag5)
			{
				StatusSnapshotDiff.Ensure(ref aBuf, aSize2);
				StatusSnapshotDiff.Ensure(ref bBuf, bSize2);
				Array.Fill<byte>(aBuf, 0, 0, aBuf.Length);
				Array.Fill<byte>(bBuf, 0, 0, bBuf.Length);
				byte[] array;
				byte* pData9;
				if ((array = aBuf) == null || array.Length == 0)
				{
					pData9 = null;
				}
				else
				{
					pData9 = &array[0];
				}
				byte[] array2;
				byte* pData10;
				if ((array2 = bBuf) == null || array2.Length == 0)
				{
					pData10 = null;
				}
				else
				{
					pData10 = &array2[0];
				}
				a2.Serialize(pData9);
				b2.Serialize(pData10);
				isDiff2 = !StatusSnapshotDiff.BufferEquals(pData9, pData10, aSize2);
				array = null;
				array2 = null;
			}
			bool flag6 = isDiff2;
			if (flag6)
			{
				byte[] array2;
				byte* pData11;
				if ((array2 = aBuf) == null || array2.Length == 0)
				{
					pData11 = null;
				}
				else
				{
					pData11 = &array2[0];
				}
				byte[] array;
				byte* pData12;
				if ((array = bBuf) == null || array.Length == 0)
				{
					pData12 = null;
				}
				else
				{
					pData12 = &array[0];
				}
				this.PlayerSelfDiffPrevious = new Player();
				this.PlayerSelfDiffCurrent = new Player();
				this.PlayerSelfDiffPrevious.Deserialize(pData11);
				this.PlayerSelfDiffCurrent.Deserialize(pData12);
				array2 = null;
				array = null;
			}
			bool isDiff3 = false;
			Player a3 = previous.Adversary;
			Player b3 = current.Adversary;
			int aSize3 = a3.GetSerializedSize();
			int bSize3 = b3.GetSerializedSize();
			bool flag7 = aSize3 != bSize3;
			if (flag7)
			{
				byte[] array;
				byte* pData13;
				if ((array = aBuf) == null || array.Length == 0)
				{
					pData13 = null;
				}
				else
				{
					pData13 = &array[0];
				}
				byte[] array2;
				byte* pData14;
				if ((array2 = bBuf) == null || array2.Length == 0)
				{
					pData14 = null;
				}
				else
				{
					pData14 = &array2[0];
				}
				a3.Serialize(pData13);
				b3.Serialize(pData14);
				array = null;
				array2 = null;
				isDiff3 = true;
			}
			bool flag8 = !isDiff3;
			if (flag8)
			{
				StatusSnapshotDiff.Ensure(ref aBuf, aSize3);
				StatusSnapshotDiff.Ensure(ref bBuf, bSize3);
				Array.Fill<byte>(aBuf, 0, 0, aBuf.Length);
				Array.Fill<byte>(bBuf, 0, 0, bBuf.Length);
				byte[] array2;
				byte* pData15;
				if ((array2 = aBuf) == null || array2.Length == 0)
				{
					pData15 = null;
				}
				else
				{
					pData15 = &array2[0];
				}
				byte[] array;
				byte* pData16;
				if ((array = bBuf) == null || array.Length == 0)
				{
					pData16 = null;
				}
				else
				{
					pData16 = &array[0];
				}
				a3.Serialize(pData15);
				b3.Serialize(pData16);
				isDiff3 = !StatusSnapshotDiff.BufferEquals(pData15, pData16, aSize3);
				array2 = null;
				array = null;
			}
			bool flag9 = isDiff3;
			if (flag9)
			{
				byte[] array;
				byte* pData17;
				if ((array = aBuf) == null || array.Length == 0)
				{
					pData17 = null;
				}
				else
				{
					pData17 = &array[0];
				}
				byte[] array2;
				byte* pData18;
				if ((array2 = bBuf) == null || array2.Length == 0)
				{
					pData18 = null;
				}
				else
				{
					pData18 = &array2[0];
				}
				this.PlayerAdversaryDiffPrevious = new Player();
				this.PlayerAdversaryDiffCurrent = new Player();
				this.PlayerAdversaryDiffPrevious.Deserialize(pData17);
				this.PlayerAdversaryDiffCurrent.Deserialize(pData18);
				array = null;
				array2 = null;
			}
			bool flag10 = previous.CurrentPlayerId != current.CurrentPlayerId;
			if (flag10)
			{
				this.CurrentPlayerIdDiff = new sbyte?(current.CurrentPlayerId);
			}
			this.LoserPlayerIsForced = current.SuicideIsForced;
			bool flag11 = current.WinnerPlayerId != -1;
			if (flag11)
			{
				this.WinnerPlayerId = new sbyte?(current.WinnerPlayerId);
			}
			bool flag12 = previous.ScoreSelf != current.ScoreSelf;
			if (flag12)
			{
				this.ScoreSelfDiff = new int?(current.ScoreSelf);
			}
			bool flag13 = previous.ScoreAdversary != current.ScoreAdversary;
			if (flag13)
			{
				this.ScoreAdversaryDiff = new int?(current.ScoreAdversary);
			}
		}

		// Token: 0x06001574 RID: 5492 RVA: 0x0014AE1C File Offset: 0x0014901C
		public StatusSnapshotDiff(in StatusSnapshot current)
		{
			this.GridStatusDiffPrevious = new GridList();
			this.GridStatusDiffCurrent = new GridList();
			for (int i = 0; i < 49; i++)
			{
				Grid grid = Serializer.CreateCopy<Grid>(current.GridStatus[i]);
				this.GridStatusDiffPrevious.Add(grid);
				this.GridStatusDiffCurrent.Add(grid);
			}
			Player p = Serializer.CreateCopy<Player>(current.Self);
			this.PlayerSelfDiffPrevious = p;
			this.PlayerSelfDiffCurrent = p;
			Player p2 = Serializer.CreateCopy<Player>(current.Adversary);
			this.PlayerAdversaryDiffPrevious = p2;
			this.PlayerAdversaryDiffCurrent = p2;
			this.CurrentPlayerIdDiff = new sbyte?(current.CurrentPlayerId);
			this.LoserPlayerIsForced = current.SuicideIsForced;
			bool flag = current.WinnerPlayerId != -1;
			if (flag)
			{
				this.WinnerPlayerId = new sbyte?(current.WinnerPlayerId);
			}
			this.ScoreSelfDiff = new int?(current.ScoreSelf);
			this.ScoreAdversaryDiff = new int?(current.ScoreAdversary);
		}

		// Token: 0x0400036F RID: 879
		public GridList GridStatusDiffPrevious;

		// Token: 0x04000370 RID: 880
		public GridList GridStatusDiffCurrent;

		// Token: 0x04000371 RID: 881
		public Player PlayerSelfDiffPrevious;

		// Token: 0x04000372 RID: 882
		public Player PlayerSelfDiffCurrent;

		// Token: 0x04000373 RID: 883
		public Player PlayerAdversaryDiffPrevious;

		// Token: 0x04000374 RID: 884
		public Player PlayerAdversaryDiffCurrent;

		// Token: 0x04000375 RID: 885
		public readonly List<StatusSnapshotDiff.BookStateExtraDiff> BookStateExtraDiffList = new List<StatusSnapshotDiff.BookStateExtraDiff>();

		// Token: 0x04000376 RID: 886
		public readonly List<StatusSnapshotDiff.GridTrapStateExtraDiff> GridTrapStateExtraDiffList = new List<StatusSnapshotDiff.GridTrapStateExtraDiff>();

		// Token: 0x04000377 RID: 887
		public int? ScoreSelfDiff;

		// Token: 0x04000378 RID: 888
		public int? ScoreAdversaryDiff;

		// Token: 0x04000379 RID: 889
		public sbyte? CurrentPlayerIdDiff;

		// Token: 0x0400037A RID: 890
		public sbyte? WinnerPlayerId;

		// Token: 0x0400037B RID: 891
		public bool LoserPlayerIsForced;

		// Token: 0x02000988 RID: 2440
		public struct BookStateExtraDiff
		{
			// Token: 0x060084C5 RID: 33989 RVA: 0x004E4D34 File Offset: 0x004E2F34
			public int GetSerializedSize()
			{
				int totalSize = 0;
				totalSize++;
				totalSize++;
				totalSize += 4;
				totalSize += 4;
				return totalSize + 1;
			}

			// Token: 0x060084C6 RID: 33990 RVA: 0x004E4D60 File Offset: 0x004E2F60
			public unsafe int Serialize(byte* pData)
			{
				*pData = (byte)this.OwnerPlayerId;
				byte* pCurrData = pData + 1;
				*pCurrData = (byte)this.BookCdIndex;
				pCurrData++;
				*(int*)pCurrData = this.NewCdValue;
				pCurrData += 4;
				*(int*)pCurrData = this.NewDisplayCdValue;
				pCurrData += 4;
				*pCurrData = (byte)this.ByPlayerId;
				pCurrData++;
				return (int)((long)(pCurrData - pData));
			}

			// Token: 0x060084C7 RID: 33991 RVA: 0x004E4DB8 File Offset: 0x004E2FB8
			public unsafe int Deserialize(byte* pData)
			{
				this.OwnerPlayerId = *(sbyte*)pData;
				byte* pCurrData = pData + 1;
				this.BookCdIndex = *(sbyte*)pCurrData;
				pCurrData++;
				this.NewCdValue = *(int*)pCurrData;
				pCurrData += 4;
				this.NewDisplayCdValue = *(int*)pCurrData;
				pCurrData += 4;
				this.ByPlayerId = *(sbyte*)pCurrData;
				pCurrData++;
				return (int)((long)(pCurrData - pData));
			}

			// Token: 0x0400281C RID: 10268
			public sbyte OwnerPlayerId;

			// Token: 0x0400281D RID: 10269
			public sbyte BookCdIndex;

			// Token: 0x0400281E RID: 10270
			public int NewCdValue;

			// Token: 0x0400281F RID: 10271
			public int NewDisplayCdValue;

			// Token: 0x04002820 RID: 10272
			public sbyte ByPlayerId;
		}

		// Token: 0x02000989 RID: 2441
		public struct GridTrapStateExtraDiff
		{
			// Token: 0x060084C8 RID: 33992 RVA: 0x004E4E10 File Offset: 0x004E3010
			public int GetSerializedSize()
			{
				int totalSize = 0;
				totalSize += 4;
				totalSize += 4;
				return totalSize + 1;
			}

			// Token: 0x060084C9 RID: 33993 RVA: 0x004E4E34 File Offset: 0x004E3034
			public unsafe int Serialize(byte* pData)
			{
				*(int*)pData = (int)this.Type;
				byte* pCurrData = pData + 4;
				*(int*)pCurrData = this.GridIndex;
				pCurrData += 4;
				*pCurrData = (byte)this.OwnerPlayerId;
				pCurrData++;
				return (int)((long)(pCurrData - pData));
			}

			// Token: 0x060084CA RID: 33994 RVA: 0x004E4E74 File Offset: 0x004E3074
			public unsafe int Deserialize(byte* pData)
			{
				this.Type = (StatusSnapshotDiff.GridTrapStateExtraDiff.TrapChangeType)(*(int*)pData);
				byte* pCurrData = pData + 4;
				this.GridIndex = *(int*)pCurrData;
				pCurrData += 4;
				this.OwnerPlayerId = *(sbyte*)pCurrData;
				pCurrData++;
				return (int)((long)(pCurrData - pData));
			}

			// Token: 0x04002821 RID: 10273
			public StatusSnapshotDiff.GridTrapStateExtraDiff.TrapChangeType Type;

			// Token: 0x04002822 RID: 10274
			public int GridIndex;

			// Token: 0x04002823 RID: 10275
			public sbyte OwnerPlayerId;

			// Token: 0x02000D7F RID: 3455
			public enum TrapChangeType
			{
				// Token: 0x04003825 RID: 14373
				Added,
				// Token: 0x04003826 RID: 14374
				Lost,
				// Token: 0x04003827 RID: 14375
				Triggered
			}
		}
	}
}
