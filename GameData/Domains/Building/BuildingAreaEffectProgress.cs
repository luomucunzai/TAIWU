using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Building
{
	// Token: 0x020008BE RID: 2238
	[SerializableGameData(IsExtensible = true, NoCopyConstructors = true, NotForDisplayModule = true)]
	public class BuildingAreaEffectProgress : ISerializableGameData
	{
		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06007BD0 RID: 31696 RVA: 0x0048FCFC File Offset: 0x0048DEFC
		public EBuildingScaleEffect BuildingScaleEffect
		{
			get
			{
				return BuildingAreaEffectProgress.EffectType.ToBuildingScaleEffect[(int)this._effectType];
			}
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06007BD1 RID: 31697 RVA: 0x0048FD0A File Offset: 0x0048DF0A
		public int LocationCount
		{
			get
			{
				List<Location> currActiveLocations = this._currActiveLocations;
				return (currActiveLocations != null) ? currActiveLocations.Count : 0;
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06007BD2 RID: 31698 RVA: 0x0048FD1E File Offset: 0x0048DF1E
		public int Progress
		{
			get
			{
				return this._progress;
			}
		}

		// Token: 0x06007BD3 RID: 31699 RVA: 0x0048FD26 File Offset: 0x0048DF26
		public BuildingAreaEffectProgress(sbyte effectType)
		{
			this._effectType = effectType;
		}

		// Token: 0x06007BD4 RID: 31700 RVA: 0x0048FD37 File Offset: 0x0048DF37
		public BuildingAreaEffectProgress()
		{
		}

		// Token: 0x06007BD5 RID: 31701 RVA: 0x0048FD44 File Offset: 0x0048DF44
		public bool RemoveLocation(Location location)
		{
			bool flag = location.IsValid() && this._currActiveLocations != null;
			return flag && this._currActiveLocations.Remove(location);
		}

		// Token: 0x06007BD6 RID: 31702 RVA: 0x0048FD7F File Offset: 0x0048DF7F
		public void AddLocation(Location location)
		{
			if (this._currActiveLocations == null)
			{
				this._currActiveLocations = new List<Location>();
			}
			this._currActiveLocations.Add(location);
		}

		// Token: 0x06007BD7 RID: 31703 RVA: 0x0048FDA4 File Offset: 0x0048DFA4
		public int OfflineSetProgress(int value)
		{
			this._progress = value;
			return value;
		}

		// Token: 0x06007BD8 RID: 31704 RVA: 0x0048FDBC File Offset: 0x0048DFBC
		public int OfflineChangeProgress(int delta)
		{
			int maxCount = BuildingAreaEffectProgress.EffectType.MaxActiveCount[(int)this._effectType];
			int currCount = this.LocationCount;
			bool flag = currCount >= maxCount;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				this._progress += delta;
				int threshold = BuildingAreaEffectProgress.EffectType.ProgressThreshold[(int)this._effectType];
				int activateCount = this._progress / threshold;
				bool flag2 = activateCount <= 0;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					bool flag3 = currCount + activateCount >= maxCount;
					if (flag3)
					{
						activateCount = maxCount - currCount;
					}
					this._progress %= threshold;
					result = activateCount;
				}
			}
			return result;
		}

		// Token: 0x06007BD9 RID: 31705 RVA: 0x0048FE50 File Offset: 0x0048E050
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06007BDA RID: 31706 RVA: 0x0048FE64 File Offset: 0x0048E064
		public int GetSerializedSize()
		{
			int totalSize = 7;
			bool flag = this._currActiveLocations != null;
			if (flag)
			{
				totalSize += 2 + 4 * this._currActiveLocations.Count;
			}
			else
			{
				totalSize += 2;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06007BDB RID: 31707 RVA: 0x0048FEB0 File Offset: 0x0048E0B0
		public unsafe int Serialize(byte* pData)
		{
			*(short*)pData = 3;
			byte* pCurrData = pData + 2;
			*pCurrData = (byte)this._effectType;
			pCurrData++;
			*(int*)pCurrData = this._progress;
			pCurrData += 4;
			bool flag = this._currActiveLocations != null;
			if (flag)
			{
				int elementsCount = this._currActiveLocations.Count;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount);
				pCurrData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					pCurrData += this._currActiveLocations[i].Serialize(pCurrData);
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

		// Token: 0x06007BDC RID: 31708 RVA: 0x0048FF78 File Offset: 0x0048E178
		public unsafe int Deserialize(byte* pData)
		{
			ushort fieldCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = fieldCount > 0;
			if (flag)
			{
				this._effectType = *(sbyte*)pCurrData;
				pCurrData++;
			}
			bool flag2 = fieldCount > 1;
			if (flag2)
			{
				this._progress = *(int*)pCurrData;
				pCurrData += 4;
			}
			bool flag3 = fieldCount > 2;
			if (flag3)
			{
				ushort elementsCount = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag4 = elementsCount > 0;
				if (flag4)
				{
					bool flag5 = this._currActiveLocations == null;
					if (flag5)
					{
						this._currActiveLocations = new List<Location>((int)elementsCount);
					}
					else
					{
						this._currActiveLocations.Clear();
					}
					for (int i = 0; i < (int)elementsCount; i++)
					{
						Location element = default(Location);
						pCurrData += element.Deserialize(pCurrData);
						this._currActiveLocations.Add(element);
					}
				}
				else
				{
					List<Location> currActiveLocations = this._currActiveLocations;
					if (currActiveLocations != null)
					{
						currActiveLocations.Clear();
					}
				}
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04002250 RID: 8784
		[SerializableGameDataField]
		private sbyte _effectType;

		// Token: 0x04002251 RID: 8785
		[SerializableGameDataField]
		private int _progress;

		// Token: 0x04002252 RID: 8786
		[SerializableGameDataField]
		private List<Location> _currActiveLocations;

		// Token: 0x02000C74 RID: 3188
		public static class EffectType
		{
			// Token: 0x06008F2D RID: 36653 RVA: 0x0050011C File Offset: 0x004FE31C
			// Note: this type is marked as 'beforefieldinit'.
			static EffectType()
			{
				EBuildingScaleEffect[] array = new EBuildingScaleEffect[3];
				RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.4F71111385F5BB6225891CE68CE1D0D094B0112A1624724815A595BEC6ACF09C).FieldHandle);
				BuildingAreaEffectProgress.EffectType.ToBuildingScaleEffect = array;
			}

			// Token: 0x0400362C RID: 13868
			public const sbyte Animal = 0;

			// Token: 0x0400362D RID: 13869
			public const sbyte Cricket = 1;

			// Token: 0x0400362E RID: 13870
			public const sbyte Adventure = 2;

			// Token: 0x0400362F RID: 13871
			public const int Count = 3;

			// Token: 0x04003630 RID: 13872
			public static readonly int[] ProgressThreshold = new int[]
			{
				60,
				180,
				360
			};

			// Token: 0x04003631 RID: 13873
			public static readonly int[] MaxActiveCount = new int[]
			{
				6,
				3,
				1
			};

			// Token: 0x04003632 RID: 13874
			public static readonly EBuildingScaleEffect[] ToBuildingScaleEffect;
		}

		// Token: 0x02000C75 RID: 3189
		private static class FieldIds
		{
			// Token: 0x04003633 RID: 13875
			public const ushort EffectType = 0;

			// Token: 0x04003634 RID: 13876
			public const ushort Progress = 1;

			// Token: 0x04003635 RID: 13877
			public const ushort CurrActiveLocations = 2;

			// Token: 0x04003636 RID: 13878
			public const ushort Count = 3;

			// Token: 0x04003637 RID: 13879
			public static readonly string[] FieldId2FieldName = new string[]
			{
				"EffectType",
				"Progress",
				"CurrActiveLocations"
			};
		}
	}
}
