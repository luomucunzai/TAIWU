using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat
{
	// Token: 0x020006D6 RID: 1750
	[SerializableGameData]
	public class TeammateCommandChangeData : ISerializableGameData
	{
		// Token: 0x06006760 RID: 26464 RVA: 0x003B1444 File Offset: 0x003AF644
		public IReadOnlyList<sbyte> GetCharTeammateCommands(int charId)
		{
			return this.GetCharTeammateCommandsInternal(charId);
		}

		// Token: 0x06006761 RID: 26465 RVA: 0x003B1450 File Offset: 0x003AF650
		private List<sbyte> GetCharTeammateCommandsInternal(int charId)
		{
			for (int i = 0; i < this.SelfTeam.TeammateCharIds.Count; i++)
			{
				int teammateId = this.SelfTeam.TeammateCharIds[i];
				bool flag = teammateId == charId;
				if (flag)
				{
					return this.SelfTeam.ReplaceTeammateCommands[i].Items;
				}
			}
			for (int j = 0; j < this.EnemyTeam.TeammateCharIds.Count; j++)
			{
				int teammateId2 = this.EnemyTeam.TeammateCharIds[j];
				bool flag2 = teammateId2 == charId;
				if (flag2)
				{
					return this.EnemyTeam.ReplaceTeammateCommands[j].Items;
				}
			}
			return null;
		}

		// Token: 0x06006762 RID: 26466 RVA: 0x003B151C File Offset: 0x003AF71C
		public bool SetCharTeammateCommands(int charId, IEnumerable<sbyte> cmdTypes)
		{
			List<sbyte> finalCmdTypes = this.GetCharTeammateCommandsInternal(charId);
			bool flag = finalCmdTypes == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				finalCmdTypes.Clear();
				bool flag2 = cmdTypes != null;
				if (flag2)
				{
					finalCmdTypes.AddRange(cmdTypes);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06006763 RID: 26467 RVA: 0x003B155A File Offset: 0x003AF75A
		public TeammateCommandChangeData()
		{
		}

		// Token: 0x06006764 RID: 26468 RVA: 0x003B1564 File Offset: 0x003AF764
		public TeammateCommandChangeData(TeammateCommandChangeData other)
		{
			this.SelfTeam = new TeammateCommandChangeDataPart(other.SelfTeam);
			this.EnemyTeam = new TeammateCommandChangeDataPart(other.EnemyTeam);
		}

		// Token: 0x06006765 RID: 26469 RVA: 0x003B1590 File Offset: 0x003AF790
		public void Assign(TeammateCommandChangeData other)
		{
			this.SelfTeam = new TeammateCommandChangeDataPart(other.SelfTeam);
			this.EnemyTeam = new TeammateCommandChangeDataPart(other.EnemyTeam);
		}

		// Token: 0x06006766 RID: 26470 RVA: 0x003B15B8 File Offset: 0x003AF7B8
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06006767 RID: 26471 RVA: 0x003B15CC File Offset: 0x003AF7CC
		public int GetSerializedSize()
		{
			int totalSize = 0;
			bool flag = this.SelfTeam != null;
			if (flag)
			{
				totalSize += 2 + this.SelfTeam.GetSerializedSize();
			}
			else
			{
				totalSize += 2;
			}
			bool flag2 = this.EnemyTeam != null;
			if (flag2)
			{
				totalSize += 2 + this.EnemyTeam.GetSerializedSize();
			}
			else
			{
				totalSize += 2;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06006768 RID: 26472 RVA: 0x003B1638 File Offset: 0x003AF838
		public unsafe int Serialize(byte* pData)
		{
			bool flag = this.SelfTeam != null;
			byte* pCurrData;
			if (flag)
			{
				pCurrData = pData + 2;
				int fieldSize = this.SelfTeam.Serialize(pCurrData);
				pCurrData += fieldSize;
				Tester.Assert(fieldSize <= 65535, "");
				*(short*)pData = (short)((ushort)fieldSize);
			}
			else
			{
				*(short*)pData = 0;
				pCurrData = pData + 2;
			}
			bool flag2 = this.EnemyTeam != null;
			if (flag2)
			{
				byte* pSubDataCount = pCurrData;
				pCurrData += 2;
				int fieldSize2 = this.EnemyTeam.Serialize(pCurrData);
				pCurrData += fieldSize2;
				Tester.Assert(fieldSize2 <= 65535, "");
				*(short*)pSubDataCount = (short)((ushort)fieldSize2);
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06006769 RID: 26473 RVA: 0x003B170C File Offset: 0x003AF90C
		public unsafe int Deserialize(byte* pData)
		{
			ushort fieldSize = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = fieldSize > 0;
			if (flag)
			{
				bool flag2 = this.SelfTeam == null;
				if (flag2)
				{
					this.SelfTeam = new TeammateCommandChangeDataPart();
				}
				pCurrData += this.SelfTeam.Deserialize(pCurrData);
			}
			else
			{
				this.SelfTeam = null;
			}
			ushort fieldSize2 = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag3 = fieldSize2 > 0;
			if (flag3)
			{
				bool flag4 = this.EnemyTeam == null;
				if (flag4)
				{
					this.EnemyTeam = new TeammateCommandChangeDataPart();
				}
				pCurrData += this.EnemyTeam.Deserialize(pCurrData);
			}
			else
			{
				this.EnemyTeam = null;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04001C43 RID: 7235
		[SerializableGameDataField]
		public TeammateCommandChangeDataPart SelfTeam;

		// Token: 0x04001C44 RID: 7236
		[SerializableGameDataField]
		public TeammateCommandChangeDataPart EnemyTeam;
	}
}
