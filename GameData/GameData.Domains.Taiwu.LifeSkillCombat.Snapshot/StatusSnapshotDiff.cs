using System;
using System.Collections.Generic;
using GameData.Domains.Taiwu.LifeSkillCombat.Status;
using GameData.Serializer;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Snapshot;

[SerializableGameData(NotForDisplayModule = true)]
public class StatusSnapshotDiff : ISerializableGameData
{
	public struct BookStateExtraDiff
	{
		public sbyte OwnerPlayerId;

		public sbyte BookCdIndex;

		public int NewCdValue;

		public int NewDisplayCdValue;

		public sbyte ByPlayerId;

		public int GetSerializedSize()
		{
			int num = 0;
			num++;
			num++;
			num += 4;
			num += 4;
			return num + 1;
		}

		public unsafe int Serialize(byte* pData)
		{
			byte* ptr = pData;
			*ptr = (byte)OwnerPlayerId;
			ptr++;
			*ptr = (byte)BookCdIndex;
			ptr++;
			*(int*)ptr = NewCdValue;
			ptr += 4;
			*(int*)ptr = NewDisplayCdValue;
			ptr += 4;
			*ptr = (byte)ByPlayerId;
			ptr++;
			return (int)(ptr - pData);
		}

		public unsafe int Deserialize(byte* pData)
		{
			byte* ptr = pData;
			OwnerPlayerId = (sbyte)(*ptr);
			ptr++;
			BookCdIndex = (sbyte)(*ptr);
			ptr++;
			NewCdValue = *(int*)ptr;
			ptr += 4;
			NewDisplayCdValue = *(int*)ptr;
			ptr += 4;
			ByPlayerId = (sbyte)(*ptr);
			ptr++;
			return (int)(ptr - pData);
		}
	}

	public struct GridTrapStateExtraDiff
	{
		public enum TrapChangeType
		{
			Added,
			Lost,
			Triggered
		}

		public TrapChangeType Type;

		public int GridIndex;

		public sbyte OwnerPlayerId;

		public int GetSerializedSize()
		{
			int num = 0;
			num += 4;
			num += 4;
			return num + 1;
		}

		public unsafe int Serialize(byte* pData)
		{
			byte* ptr = pData;
			*(TrapChangeType*)ptr = Type;
			ptr += 4;
			*(int*)ptr = GridIndex;
			ptr += 4;
			*ptr = (byte)OwnerPlayerId;
			ptr++;
			return (int)(ptr - pData);
		}

		public unsafe int Deserialize(byte* pData)
		{
			byte* ptr = pData;
			Type = *(TrapChangeType*)ptr;
			ptr += 4;
			GridIndex = *(int*)ptr;
			ptr += 4;
			OwnerPlayerId = (sbyte)(*ptr);
			ptr++;
			return (int)(ptr - pData);
		}
	}

	public GridList GridStatusDiffPrevious;

	public GridList GridStatusDiffCurrent;

	public Player PlayerSelfDiffPrevious;

	public Player PlayerSelfDiffCurrent;

	public Player PlayerAdversaryDiffPrevious;

	public Player PlayerAdversaryDiffCurrent;

	public readonly List<BookStateExtraDiff> BookStateExtraDiffList = new List<BookStateExtraDiff>();

	public readonly List<GridTrapStateExtraDiff> GridTrapStateExtraDiffList = new List<GridTrapStateExtraDiff>();

	public int? ScoreSelfDiff;

	public int? ScoreAdversaryDiff;

	public sbyte? CurrentPlayerIdDiff;

	public sbyte? WinnerPlayerId;

	public bool LoserPlayerIsForced;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num += GridStatusDiffPrevious.GetSerializedSize();
		num += GridStatusDiffCurrent.GetSerializedSize();
		if (PlayerSelfDiffPrevious != null)
		{
			num++;
			num += PlayerSelfDiffPrevious.GetSerializedSize();
		}
		else
		{
			num++;
		}
		if (PlayerSelfDiffCurrent != null)
		{
			num++;
			num += PlayerSelfDiffCurrent.GetSerializedSize();
		}
		else
		{
			num++;
		}
		if (PlayerAdversaryDiffPrevious != null)
		{
			num++;
			num += PlayerAdversaryDiffPrevious.GetSerializedSize();
		}
		else
		{
			num++;
		}
		if (PlayerAdversaryDiffCurrent != null)
		{
			num++;
			num += PlayerAdversaryDiffCurrent.GetSerializedSize();
		}
		else
		{
			num++;
		}
		num += 4;
		num += 4;
		num++;
		num++;
		num += 2;
		foreach (BookStateExtraDiff bookStateExtraDiff in BookStateExtraDiffList)
		{
			num += bookStateExtraDiff.GetSerializedSize();
		}
		num += 2;
		foreach (GridTrapStateExtraDiff gridTrapStateExtraDiff in GridTrapStateExtraDiffList)
		{
			num += gridTrapStateExtraDiff.GetSerializedSize();
		}
		num++;
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += GridStatusDiffPrevious.Serialize(ptr);
		ptr += GridStatusDiffCurrent.Serialize(ptr);
		if (PlayerSelfDiffPrevious != null)
		{
			*ptr = 1;
			ptr++;
			ptr += PlayerSelfDiffPrevious.Serialize(ptr);
		}
		else
		{
			*ptr = 0;
			ptr++;
		}
		if (PlayerSelfDiffCurrent != null)
		{
			*ptr = 1;
			ptr++;
			ptr += PlayerSelfDiffCurrent.Serialize(ptr);
		}
		else
		{
			*ptr = 0;
			ptr++;
		}
		if (PlayerAdversaryDiffPrevious != null)
		{
			*ptr = 1;
			ptr++;
			ptr += PlayerAdversaryDiffPrevious.Serialize(ptr);
		}
		else
		{
			*ptr = 0;
			ptr++;
		}
		if (PlayerAdversaryDiffCurrent != null)
		{
			*ptr = 1;
			ptr++;
			ptr += PlayerAdversaryDiffCurrent.Serialize(ptr);
		}
		else
		{
			*ptr = 0;
			ptr++;
		}
		if (ScoreSelfDiff.HasValue)
		{
			*(int*)ptr = ScoreSelfDiff.Value;
			ptr += 4;
		}
		else
		{
			*(int*)ptr = -1;
			ptr += 4;
		}
		if (ScoreAdversaryDiff.HasValue)
		{
			*(int*)ptr = ScoreAdversaryDiff.Value;
			ptr += 4;
		}
		else
		{
			*(int*)ptr = -1;
			ptr += 4;
		}
		if (CurrentPlayerIdDiff.HasValue)
		{
			*ptr = (byte)CurrentPlayerIdDiff.Value;
			ptr++;
		}
		else
		{
			*ptr = byte.MaxValue;
			ptr++;
		}
		if (WinnerPlayerId.HasValue)
		{
			*ptr = (byte)WinnerPlayerId.Value;
			ptr++;
		}
		else
		{
			*ptr = byte.MaxValue;
			ptr++;
		}
		*(ushort*)ptr = (ushort)BookStateExtraDiffList.Count;
		ptr += 2;
		foreach (BookStateExtraDiff bookStateExtraDiff in BookStateExtraDiffList)
		{
			ptr += bookStateExtraDiff.Serialize(ptr);
		}
		*(ushort*)ptr = (ushort)GridTrapStateExtraDiffList.Count;
		ptr += 2;
		foreach (GridTrapStateExtraDiff gridTrapStateExtraDiff in GridTrapStateExtraDiffList)
		{
			ptr += gridTrapStateExtraDiff.Serialize(ptr);
		}
		*ptr = (LoserPlayerIsForced ? ((byte)1) : ((byte)0));
		ptr++;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		if (GridStatusDiffPrevious == null)
		{
			GridStatusDiffPrevious = new GridList();
		}
		if (GridStatusDiffCurrent == null)
		{
			GridStatusDiffCurrent = new GridList();
		}
		ptr += GridStatusDiffPrevious.Deserialize(ptr);
		ptr += GridStatusDiffCurrent.Deserialize(ptr);
		PlayerSelfDiffPrevious = null;
		if (*ptr != 0)
		{
			ptr++;
			PlayerSelfDiffPrevious = new Player();
			ptr += PlayerSelfDiffPrevious.Deserialize(ptr);
		}
		else
		{
			ptr++;
		}
		PlayerSelfDiffCurrent = null;
		if (*ptr != 0)
		{
			ptr++;
			PlayerSelfDiffCurrent = new Player();
			ptr += PlayerSelfDiffCurrent.Deserialize(ptr);
		}
		else
		{
			ptr++;
		}
		PlayerAdversaryDiffPrevious = null;
		if (*ptr != 0)
		{
			ptr++;
			PlayerAdversaryDiffPrevious = new Player();
			ptr += PlayerAdversaryDiffPrevious.Deserialize(ptr);
		}
		else
		{
			ptr++;
		}
		PlayerAdversaryDiffCurrent = null;
		if (*ptr != 0)
		{
			ptr++;
			PlayerAdversaryDiffCurrent = new Player();
			ptr += PlayerAdversaryDiffCurrent.Deserialize(ptr);
		}
		else
		{
			ptr++;
		}
		ScoreSelfDiff = *(int*)ptr;
		ptr += 4;
		if (ScoreSelfDiff < 0)
		{
			ScoreSelfDiff = null;
		}
		ScoreAdversaryDiff = *(int*)ptr;
		ptr += 4;
		if (ScoreAdversaryDiff < 0)
		{
			ScoreAdversaryDiff = null;
		}
		CurrentPlayerIdDiff = (sbyte)(*ptr);
		ptr++;
		if (CurrentPlayerIdDiff == -1)
		{
			CurrentPlayerIdDiff = null;
		}
		WinnerPlayerId = (sbyte)(*ptr);
		ptr++;
		if (WinnerPlayerId == -1)
		{
			WinnerPlayerId = null;
		}
		ushort num = *(ushort*)ptr;
		ptr += 2;
		BookStateExtraDiffList.Clear();
		for (int i = 0; i < num; i++)
		{
			BookStateExtraDiff item = default(BookStateExtraDiff);
			ptr += item.Deserialize(ptr);
			BookStateExtraDiffList.Add(item);
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		GridTrapStateExtraDiffList.Clear();
		for (int j = 0; j < num2; j++)
		{
			GridTrapStateExtraDiff item2 = default(GridTrapStateExtraDiff);
			ptr += item2.Deserialize(ptr);
			GridTrapStateExtraDiffList.Add(item2);
		}
		LoserPlayerIsForced = *ptr != 0;
		ptr++;
		int num3 = (int)(ptr - pData);
		return (num3 <= 4) ? num3 : ((num3 + 3) / 4 * 4);
	}

	private static void Ensure(ref byte[] buf, int size)
	{
		if (buf.Length < size)
		{
			buf = new byte[size * 2];
		}
	}

	private unsafe static bool BufferEquals(byte* pData, byte* pData2, int size)
	{
		for (int i = 0; i < size; i++)
		{
			if (pData[i] != pData2[i])
			{
				return false;
			}
		}
		return true;
	}

	public unsafe StatusSnapshotDiff(in StatusSnapshot previous, in StatusSnapshot current)
	{
		byte[] buf = new byte[1024];
		byte[] buf2 = new byte[1024];
		GridStatusDiffPrevious = new GridList();
		GridStatusDiffCurrent = new GridList();
		for (int i = 0; i < 49; i++)
		{
			bool flag = false;
			Grid grid = previous.GridStatus[i];
			Grid grid2 = current.GridStatus[i];
			int serializedSize = grid.GetSerializedSize();
			int serializedSize2 = grid2.GetSerializedSize();
			if (serializedSize != serializedSize2)
			{
				fixed (byte* pData = buf)
				{
					fixed (byte* pData2 = buf2)
					{
						grid.Serialize(pData);
						grid2.Serialize(pData2);
					}
				}
				flag = true;
			}
			if (!flag)
			{
				Ensure(ref buf, serializedSize);
				Ensure(ref buf2, serializedSize2);
				Array.Fill(buf, (byte)0, 0, buf.Length);
				Array.Fill(buf2, (byte)0, 0, buf2.Length);
				fixed (byte* pData3 = buf)
				{
					fixed (byte* ptr = buf2)
					{
						grid.Serialize(pData3);
						grid2.Serialize(ptr);
						flag = !BufferEquals(pData3, ptr, serializedSize);
					}
				}
			}
			if (!flag)
			{
				continue;
			}
			fixed (byte* pData4 = buf)
			{
				fixed (byte* pData5 = buf2)
				{
					grid = new Grid();
					grid2 = new Grid();
					grid.Deserialize(pData4);
					grid2.Deserialize(pData5);
					GridStatusDiffPrevious.Add(grid);
					GridStatusDiffCurrent.Add(grid2);
				}
			}
		}
		bool flag2 = false;
		Player self = previous.Self;
		Player self2 = current.Self;
		int serializedSize3 = self.GetSerializedSize();
		int serializedSize4 = self2.GetSerializedSize();
		if (serializedSize3 != serializedSize4)
		{
			fixed (byte* pData6 = buf)
			{
				fixed (byte* pData7 = buf2)
				{
					self.Serialize(pData6);
					self2.Serialize(pData7);
				}
			}
			flag2 = true;
		}
		if (!flag2)
		{
			Ensure(ref buf, serializedSize3);
			Ensure(ref buf2, serializedSize4);
			Array.Fill(buf, (byte)0, 0, buf.Length);
			Array.Fill(buf2, (byte)0, 0, buf2.Length);
			fixed (byte* pData8 = buf)
			{
				fixed (byte* ptr2 = buf2)
				{
					self.Serialize(pData8);
					self2.Serialize(ptr2);
					flag2 = !BufferEquals(pData8, ptr2, serializedSize3);
				}
			}
		}
		if (flag2)
		{
			fixed (byte* pData9 = buf)
			{
				fixed (byte* pData10 = buf2)
				{
					PlayerSelfDiffPrevious = new Player();
					PlayerSelfDiffCurrent = new Player();
					PlayerSelfDiffPrevious.Deserialize(pData9);
					PlayerSelfDiffCurrent.Deserialize(pData10);
				}
			}
		}
		bool flag3 = false;
		Player adversary = previous.Adversary;
		Player adversary2 = current.Adversary;
		int serializedSize5 = adversary.GetSerializedSize();
		int serializedSize6 = adversary2.GetSerializedSize();
		if (serializedSize5 != serializedSize6)
		{
			fixed (byte* pData11 = buf)
			{
				fixed (byte* pData12 = buf2)
				{
					adversary.Serialize(pData11);
					adversary2.Serialize(pData12);
				}
			}
			flag3 = true;
		}
		if (!flag3)
		{
			Ensure(ref buf, serializedSize5);
			Ensure(ref buf2, serializedSize6);
			Array.Fill(buf, (byte)0, 0, buf.Length);
			Array.Fill(buf2, (byte)0, 0, buf2.Length);
			fixed (byte* pData13 = buf)
			{
				fixed (byte* ptr3 = buf2)
				{
					adversary.Serialize(pData13);
					adversary2.Serialize(ptr3);
					flag3 = !BufferEquals(pData13, ptr3, serializedSize5);
				}
			}
		}
		if (flag3)
		{
			fixed (byte* pData14 = buf)
			{
				fixed (byte* pData15 = buf2)
				{
					PlayerAdversaryDiffPrevious = new Player();
					PlayerAdversaryDiffCurrent = new Player();
					PlayerAdversaryDiffPrevious.Deserialize(pData14);
					PlayerAdversaryDiffCurrent.Deserialize(pData15);
				}
			}
		}
		if (previous.CurrentPlayerId != current.CurrentPlayerId)
		{
			CurrentPlayerIdDiff = current.CurrentPlayerId;
		}
		LoserPlayerIsForced = current.SuicideIsForced;
		if (current.WinnerPlayerId != -1)
		{
			WinnerPlayerId = current.WinnerPlayerId;
		}
		if (previous.ScoreSelf != current.ScoreSelf)
		{
			ScoreSelfDiff = current.ScoreSelf;
		}
		if (previous.ScoreAdversary != current.ScoreAdversary)
		{
			ScoreAdversaryDiff = current.ScoreAdversary;
		}
	}

	public StatusSnapshotDiff(in StatusSnapshot current)
	{
		GridStatusDiffPrevious = new GridList();
		GridStatusDiffCurrent = new GridList();
		for (int i = 0; i < 49; i++)
		{
			Grid item = GameData.Serializer.Serializer.CreateCopy(current.GridStatus[i]);
			GridStatusDiffPrevious.Add(item);
			GridStatusDiffCurrent.Add(item);
		}
		PlayerSelfDiffCurrent = (PlayerSelfDiffPrevious = GameData.Serializer.Serializer.CreateCopy(current.Self));
		PlayerAdversaryDiffCurrent = (PlayerAdversaryDiffPrevious = GameData.Serializer.Serializer.CreateCopy(current.Adversary));
		CurrentPlayerIdDiff = current.CurrentPlayerId;
		LoserPlayerIsForced = current.SuicideIsForced;
		if (current.WinnerPlayerId != -1)
		{
			WinnerPlayerId = current.WinnerPlayerId;
		}
		ScoreSelfDiff = current.ScoreSelf;
		ScoreAdversaryDiff = current.ScoreAdversary;
	}
}
