using System;
using System.Collections;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Taiwu
{
	// Token: 0x02000041 RID: 65
	[SerializableGameDataSourceGenerator.AutoGenerateSerializableGameData(NotForDisplayModule = true)]
	public class SkillBreakPlateList : IList<SkillBreakPlate>, ICollection<SkillBreakPlate>, IEnumerable<SkillBreakPlate>, IEnumerable, ISerializableGameData
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000F79 RID: 3961 RVA: 0x000FE588 File Offset: 0x000FC788
		private IList<SkillBreakPlate> ListImplementation
		{
			get
			{
				List<SkillBreakPlate> result;
				if ((result = this._list) == null)
				{
					result = (this._list = new List<SkillBreakPlate>());
				}
				return result;
			}
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x000FE5B0 File Offset: 0x000FC7B0
		public IEnumerator<SkillBreakPlate> GetEnumerator()
		{
			return this.ListImplementation.GetEnumerator();
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x000FE5D0 File Offset: 0x000FC7D0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.ListImplementation.GetEnumerator();
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x000FE5ED File Offset: 0x000FC7ED
		public void Add(SkillBreakPlate item)
		{
			this.ListImplementation.Add(item);
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x000FE5FD File Offset: 0x000FC7FD
		public void Clear()
		{
			this.ListImplementation.Clear();
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x000FE60C File Offset: 0x000FC80C
		public bool Contains(SkillBreakPlate item)
		{
			return this.ListImplementation.Contains(item);
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x000FE62A File Offset: 0x000FC82A
		public void CopyTo(SkillBreakPlate[] array, int arrayIndex)
		{
			this.ListImplementation.CopyTo(array, arrayIndex);
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x000FE63C File Offset: 0x000FC83C
		public bool Remove(SkillBreakPlate item)
		{
			return this.ListImplementation.Remove(item);
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000F81 RID: 3969 RVA: 0x000FE65A File Offset: 0x000FC85A
		public int Count
		{
			get
			{
				return this.ListImplementation.Count;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000F82 RID: 3970 RVA: 0x000FE667 File Offset: 0x000FC867
		public bool IsReadOnly
		{
			get
			{
				return this.ListImplementation.IsReadOnly;
			}
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x000FE674 File Offset: 0x000FC874
		public int IndexOf(SkillBreakPlate item)
		{
			return this.ListImplementation.IndexOf(item);
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x000FE692 File Offset: 0x000FC892
		public void Insert(int index, SkillBreakPlate item)
		{
			this.ListImplementation.Insert(index, item);
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x000FE6A3 File Offset: 0x000FC8A3
		public void RemoveAt(int index)
		{
			this.ListImplementation.RemoveAt(index);
		}

		// Token: 0x17000024 RID: 36
		public SkillBreakPlate this[int index]
		{
			get
			{
				return this.ListImplementation[index];
			}
			set
			{
				this.ListImplementation[index] = value;
			}
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x000FE6D1 File Offset: 0x000FC8D1
		public SkillBreakPlateList()
		{
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x000FE6E8 File Offset: 0x000FC8E8
		public SkillBreakPlateList(SkillBreakPlateList other)
		{
			bool flag = other._list != null;
			if (flag)
			{
				List<SkillBreakPlate> item = other._list;
				int elementsCount = item.Count;
				this._list = new List<SkillBreakPlate>(elementsCount);
				foreach (SkillBreakPlate element in item)
				{
					this._list.Add(new SkillBreakPlate(element));
				}
			}
			else
			{
				this._list = null;
			}
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x000FE78C File Offset: 0x000FC98C
		public void Assign(SkillBreakPlateList other)
		{
			bool flag = other._list != null;
			if (flag)
			{
				List<SkillBreakPlate> item = other._list;
				int elementsCount = item.Count;
				this._list = new List<SkillBreakPlate>(elementsCount);
				foreach (SkillBreakPlate element in item)
				{
					this._list.Add(new SkillBreakPlate(element));
				}
			}
			else
			{
				this._list = null;
			}
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x000FE81C File Offset: 0x000FCA1C
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x000FE830 File Offset: 0x000FCA30
		public int GetSerializedSize()
		{
			int totalSize = 0;
			bool flag = this._list != null;
			if (flag)
			{
				totalSize += 2;
				for (int i = 0; i < this._list.Count; i++)
				{
					bool flag2 = this._list[i] != null;
					if (flag2)
					{
						totalSize += 2 + this._list[i].GetSerializedSize();
					}
					else
					{
						totalSize += 2;
					}
				}
			}
			else
			{
				totalSize += 2;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x000FE8C0 File Offset: 0x000FCAC0
		public unsafe int Serialize(byte* pData)
		{
			bool flag = this._list != null;
			byte* pCurrData;
			if (flag)
			{
				int elementsCount = this._list.Count;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pData = (short)((ushort)elementsCount);
				pCurrData = pData + 2;
				for (int i = 0; i < elementsCount; i++)
				{
					bool flag2 = this._list[i] != null;
					if (flag2)
					{
						byte* pSubDataCount = pCurrData;
						pCurrData += 2;
						int fieldSize = this._list[i].Serialize(pCurrData);
						pCurrData += fieldSize;
						Tester.Assert(fieldSize <= 65535, "");
						*(short*)pSubDataCount = (short)((ushort)fieldSize);
					}
					else
					{
						*(short*)pCurrData = 0;
						pCurrData += 2;
					}
				}
			}
			else
			{
				*(short*)pData = 0;
				pCurrData = pData + 2;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x000FE9B4 File Offset: 0x000FCBB4
		public unsafe int Deserialize(byte* pData)
		{
			ushort elementsCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = elementsCount > 0;
			if (flag)
			{
				bool flag2 = this._list == null;
				if (flag2)
				{
					this._list = new List<SkillBreakPlate>();
				}
				else
				{
					this._list.Clear();
				}
				for (int i = 0; i < (int)elementsCount; i++)
				{
					ushort classCount = *(ushort*)pCurrData;
					pCurrData += 2;
					bool flag3 = classCount > 0;
					SkillBreakPlate element;
					if (flag3)
					{
						element = new SkillBreakPlate();
						pCurrData += element.Deserialize(pCurrData);
					}
					else
					{
						element = null;
					}
					this._list.Add(element);
				}
			}
			else
			{
				List<SkillBreakPlate> list = this._list;
				if (list != null)
				{
					list.Clear();
				}
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0400023F RID: 575
		[SerializableGameDataField]
		private List<SkillBreakPlate> _list = new List<SkillBreakPlate>();
	}
}
