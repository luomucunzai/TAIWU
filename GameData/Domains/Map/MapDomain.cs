using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Config;
using Config.ConfigCells;
using Config.ConfigCells.Character;
using GameData.ArchiveData;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Common.Algorithm;
using GameData.Common.SingleValueCollection;
using GameData.Dependencies;
using GameData.DLC.FiveLoong;
using GameData.DomainEvents;
using GameData.Domains.Adventure;
using GameData.Domains.Building;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Character.Filters;
using GameData.Domains.Character.ParallelModifications;
using GameData.Domains.Character.Relation;
using GameData.Domains.Combat;
using GameData.Domains.Extra;
using GameData.Domains.Global;
using GameData.Domains.Item;
using GameData.Domains.Item.Filters;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map.TeammateBubble;
using GameData.Domains.Merchant;
using GameData.Domains.Organization;
using GameData.Domains.Organization.ParallelModifications;
using GameData.Domains.Taiwu;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.Taiwu.Profession.SkillsData;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Domains.TaiwuEvent.MonthlyEventActions;
using GameData.Domains.World;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;
using GameData.Domains.World.TravelingEvent;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using NLog;
using Redzen.Random;

namespace GameData.Domains.Map
{
	// Token: 0x020008B7 RID: 2231
	[GameDataDomain(2)]
	public class MapDomain : BaseGameDataDomain
	{
		// Token: 0x060078C1 RID: 30913 RVA: 0x00466EF4 File Offset: 0x004650F4
		private void OnInitializedDomainData()
		{
			for (int i = 0; i < 139; i++)
			{
				this._areas[i] = new MapAreaData();
			}
			this.InitRegularBlockArrayAndFuncs();
			this.TaiwuLastLocation = Location.Invalid;
		}

		// Token: 0x060078C2 RID: 30914 RVA: 0x00466F36 File Offset: 0x00465136
		private void InitializeOnInitializeGameDataModule()
		{
		}

		// Token: 0x060078C3 RID: 30915 RVA: 0x00466F39 File Offset: 0x00465139
		private void InitializeOnEnterNewWorld()
		{
			Events.RegisterHandler_CharacterLocationChanged(new Events.OnCharacterLocationChanged(this.OnCharacterLocationChanged));
			this.InitializeTeammateBubble();
		}

		// Token: 0x060078C4 RID: 30916 RVA: 0x00466F58 File Offset: 0x00465158
		private unsafe void OnLoadedArchiveData()
		{
			for (short areaId = 0; areaId < 45; areaId += 1)
			{
				this.GetRegularAreaBlocks(areaId).ConvertToRegularCollection();
			}
			this._brokenAreaBlocks.ConvertToRegularCollection();
			this._bornAreaBlocks.ConvertToRegularCollection();
			this._guideAreaBlocks.ConvertToRegularCollection();
			this._secretVillageAreaBlocks.ConvertToRegularCollection();
			this._brokenPerformAreaBlocks.ConvertToRegularCollection();
			for (short areaId2 = 0; areaId2 < 139; areaId2 += 1)
			{
				Span<MapBlockData> blocks = this.GetAreaBlocks(areaId2);
				short blockId = 0;
				while ((int)blockId < blocks.Length)
				{
					MapBlockData blockData = *blocks[(int)blockId];
					bool flag = blockData.RootBlockId >= 0;
					if (flag)
					{
						MapBlockData rootBlock = *blocks[(int)blockData.RootBlockId];
						MapBlockData mapBlockData = rootBlock;
						if (mapBlockData.GroupBlockList == null)
						{
							mapBlockData.GroupBlockList = new List<MapBlockData>();
						}
						bool flag2 = !rootBlock.GroupBlockList.Contains(blockData);
						if (flag2)
						{
							rootBlock.GroupBlockList.Add(blockData);
						}
					}
					blockId += 1;
				}
			}
			Events.RegisterHandler_CharacterLocationChanged(new Events.OnCharacterLocationChanged(this.OnCharacterLocationChanged));
			this.InitializeTravelMap();
			this.InitializeTeammateBubble();
		}

		// Token: 0x060078C5 RID: 30917 RVA: 0x0046709C File Offset: 0x0046529C
		public unsafe override void FixAbnormalDomainArchiveData(DataContext context)
		{
			List<MapBlockData> neighborBlocks = ObjectPool<List<MapBlockData>>.Instance.Get();
			for (short areaId = 0; areaId < 45; areaId += 1)
			{
				MapAreaData area = this._areas[(int)areaId];
				for (int i = 0; i < area.SettlementInfos.Length; i++)
				{
					short blockId = area.SettlementInfos[i].BlockId;
					bool flag = blockId < 0;
					if (!flag)
					{
						neighborBlocks.Clear();
						this.GetNeighborBlocks(areaId, blockId, neighborBlocks, (int)this.GetBlockRange(this.GetBlock(areaId, blockId)));
						foreach (MapBlockData neighborBlock in neighborBlocks)
						{
							bool flag2 = neighborBlock.BelongBlockId < 0;
							if (flag2)
							{
								neighborBlock.BelongBlockId = blockId;
								this.SetBlockData(context, neighborBlock);
							}
						}
					}
				}
			}
			bool flag3 = DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 69, 61);
			if (flag3)
			{
				this.FixUnexpectedItemsOnMapBlocks(context);
			}
			bool flag4 = DomainManager.Extra.GetStationInited() == 0;
			if (flag4)
			{
				for (short areaId2 = 0; areaId2 < 135; areaId2 += 1)
				{
					bool flag5 = !this._areas[(int)areaId2].StationUnlocked;
					if (flag5)
					{
						this.UnlockStation(context, areaId2, false);
					}
				}
				DomainManager.Extra.SetStationInited(1, context);
			}
			short areaId3 = 0;
			while ((int)areaId3 < this._animalPlaceData.Length)
			{
				AnimalPlaceData areaAnimalPlaceData = this._animalPlaceData[(int)areaId3];
				bool flag6 = areaAnimalPlaceData != null;
				if (flag6)
				{
					foreach (KeyValuePair<short, short> pair in areaAnimalPlaceData.BlockAnimalCharacterTemplateIds)
					{
						DomainManager.Extra.CreateAnimalByCharacterTemplateId(context, pair.Value, new Location(areaId3, pair.Key));
					}
					this.SetElement_AnimalPlaceData((int)areaId3, null, context);
				}
				areaId3 += 1;
			}
			short areaId4 = 0;
			while ((int)areaId4 < this._cricketPlaceData.Length)
			{
				bool flag7 = this._cricketPlaceData[(int)areaId4] == null;
				if (!flag7)
				{
					this._cricketPlaceData[(int)areaId4].FixInvalidData(areaId4);
					this.SetElement_CricketPlaceData((int)areaId4, this._cricketPlaceData[(int)areaId4], context);
				}
				areaId4 += 1;
			}
			for (short areaId5 = 0; areaId5 < 45; areaId5 += 1)
			{
				Span<MapBlockData> areaBlocks = this.GetAreaBlocks(areaId5);
				for (int j = 0; j < areaBlocks.Length; j++)
				{
					MapBlockData block = *areaBlocks[j];
					MapBlockItem blockCfg = block.GetConfig();
					bool flag8 = blockCfg.Size <= 1;
					if (!flag8)
					{
						bool flag9 = !block.Destroyed;
						if (!flag9)
						{
							block.Destroyed = false;
							this.SetBlockData(context, block);
							MapDomain.Logger.Warn("Fixing destroyed multi-block block " + blockCfg.Name + ".");
						}
					}
				}
			}
			bool flag10 = DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 79, 24);
			if (flag10)
			{
				AdaptableLog.Info("Fixing TemplateEnemyList during.");
				for (short areaId6 = 0; areaId6 < 139; areaId6 += 1)
				{
					Span<MapBlockData> areaBlocks2 = this.GetAreaBlocks(areaId6);
					int k = 0;
					while (k < areaBlocks2.Length)
					{
						MapBlockData block2 = *areaBlocks2[k];
						List<MapTemplateEnemyInfo> templateEnemyList = block2.TemplateEnemyList;
						if (templateEnemyList == null)
						{
							goto IL_37B;
						}
						int count = templateEnemyList.Count;
						if (count == 0)
						{
							goto IL_37B;
						}
						bool flag11 = false;
						IL_383:
						bool flag12 = flag11;
						if (!flag12)
						{
							int index = block2.TemplateEnemyList.Count;
							while (index-- > 0)
							{
								MapTemplateEnemyInfo tmp = block2.TemplateEnemyList[index];
								tmp.Duration = -1;
								block2.TemplateEnemyList[index] = tmp;
							}
							this.SetBlockData(context, block2);
						}
						k++;
						continue;
						IL_37B:
						flag11 = true;
						goto IL_383;
					}
				}
			}
		}

		// Token: 0x060078C6 RID: 30918 RVA: 0x004674D8 File Offset: 0x004656D8
		private unsafe void FixUnexpectedItemsOnMapBlocks(DataContext context)
		{
			HashSet<ItemKey> itemKeys = new HashSet<ItemKey>();
			for (short areaId = 0; areaId < 139; areaId += 1)
			{
				Span<MapBlockData> blocks = this.GetAreaBlocks(areaId);
				int blockIdx = 0;
				int blocksCount = blocks.Length;
				while (blockIdx < blocksCount)
				{
					MapBlockData block = *blocks[blockIdx];
					SortedList<ItemKeyAndDate, int> blockItems = block.Items;
					bool flag = blockItems == null;
					if (!flag)
					{
						bool isChanged = false;
						for (int index = blockItems.Keys.Count - 1; index >= 0; index--)
						{
							ItemKeyAndDate blockItem = blockItems.Keys[index];
							ItemKey itemKey = blockItem.ItemKey;
							bool flag2 = !ItemTemplateHelper.IsPureStackable(itemKey) && !itemKeys.Add(itemKey);
							if (flag2)
							{
								Logger logger = MapDomain.Logger;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 2);
								defaultInterpolatedStringHandler.AppendLiteral("Removing duplicate item ");
								defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(itemKey);
								defaultInterpolatedStringHandler.AppendLiteral(" on map block at ");
								defaultInterpolatedStringHandler.AppendFormatted<Location>(block.GetLocation());
								defaultInterpolatedStringHandler.AppendLiteral(".");
								logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
								block.RemoveItem(blockItem);
								isChanged = true;
							}
							else
							{
								bool flag3 = !ItemDomain.CanItemBeLost(itemKey);
								if (flag3)
								{
									Logger logger2 = MapDomain.Logger;
									DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 2);
									defaultInterpolatedStringHandler.AppendLiteral("Removing unexpected item ");
									defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(itemKey);
									defaultInterpolatedStringHandler.AppendLiteral(" on map block at ");
									defaultInterpolatedStringHandler.AppendFormatted<Location>(block.GetLocation());
									defaultInterpolatedStringHandler.AppendLiteral(".");
									logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
									block.RemoveItem(blockItem);
									DomainManager.Item.RemoveItem(context, itemKey);
									isChanged = true;
									bool flag4 = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId) == 1202;
									if (flag4)
									{
										DomainManager.LegendaryBook.UnregisterLegendaryBookItem(itemKey);
									}
								}
								else
								{
									bool flag5 = !DomainManager.Item.ItemExists(itemKey);
									if (flag5)
									{
										Logger logger3 = MapDomain.Logger;
										DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 2);
										defaultInterpolatedStringHandler.AppendLiteral("Removing non-existing item ");
										defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(itemKey);
										defaultInterpolatedStringHandler.AppendLiteral(" on map block at ");
										defaultInterpolatedStringHandler.AppendFormatted<Location>(block.GetLocation());
										logger3.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
										block.RemoveItem(blockItem);
										isChanged = true;
									}
									else
									{
										bool flag6 = ItemType.IsEquipmentItemType(itemKey.ItemType);
										if (flag6)
										{
											int ownerId = DomainManager.Item.GetBaseEquipment(itemKey).GetEquippedCharId();
											GameData.Domains.Character.Character character;
											bool flag7 = ownerId >= 0 && DomainManager.Character.TryGetElement_Objects(ownerId, out character) && character.GetEquipment().Exist(itemKey);
											if (flag7)
											{
												Logger logger4 = MapDomain.Logger;
												DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(63, 3);
												defaultInterpolatedStringHandler.AppendLiteral("Removing equipment ");
												defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(itemKey);
												defaultInterpolatedStringHandler.AppendLiteral(" on map block at ");
												defaultInterpolatedStringHandler.AppendFormatted<Location>(block.GetLocation());
												defaultInterpolatedStringHandler.AppendLiteral(" because it is equipped by ");
												defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(character);
												logger4.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
												block.RemoveItem(blockItem);
												isChanged = true;
											}
										}
									}
								}
							}
						}
						bool flag8 = isChanged;
						if (flag8)
						{
							this.SetBlockData(context, block);
						}
					}
					blockIdx++;
				}
			}
		}

		// Token: 0x060078C7 RID: 30919 RVA: 0x00467848 File Offset: 0x00465A48
		public AreaBlockCollection GetRegularAreaBlocks(short areaId)
		{
			bool flag = areaId == 138;
			AreaBlockCollection result;
			if (flag)
			{
				result = this._brokenPerformAreaBlocks;
			}
			else
			{
				result = this._regularAreaBlocksArray[(int)areaId];
			}
			return result;
		}

		// Token: 0x060078C8 RID: 30920 RVA: 0x00467877 File Offset: 0x00465A77
		public void AddRegularBlockData(DataContext context, Location blockKey, MapBlockData data)
		{
			this._regularAreaBlocksAddFuncs[(int)blockKey.AreaId](blockKey.BlockId, data, context);
		}

		// Token: 0x060078C9 RID: 30921 RVA: 0x00467895 File Offset: 0x00465A95
		public void SetRegularBlockData(DataContext context, MapBlockData block)
		{
			this._regularAreaBlocksSetFuncs[(int)block.AreaId](block.BlockId, block, context);
		}

		// Token: 0x060078CA RID: 30922 RVA: 0x004678B4 File Offset: 0x00465AB4
		private void InitRegularBlockArrayAndFuncs()
		{
			this._regularAreaBlocksArray = new AreaBlockCollection[]
			{
				this._areaBlocks0,
				this._areaBlocks1,
				this._areaBlocks2,
				this._areaBlocks3,
				this._areaBlocks4,
				this._areaBlocks5,
				this._areaBlocks6,
				this._areaBlocks7,
				this._areaBlocks8,
				this._areaBlocks9,
				this._areaBlocks10,
				this._areaBlocks11,
				this._areaBlocks12,
				this._areaBlocks13,
				this._areaBlocks14,
				this._areaBlocks15,
				this._areaBlocks16,
				this._areaBlocks17,
				this._areaBlocks18,
				this._areaBlocks19,
				this._areaBlocks20,
				this._areaBlocks21,
				this._areaBlocks22,
				this._areaBlocks23,
				this._areaBlocks24,
				this._areaBlocks25,
				this._areaBlocks26,
				this._areaBlocks27,
				this._areaBlocks28,
				this._areaBlocks29,
				this._areaBlocks30,
				this._areaBlocks31,
				this._areaBlocks32,
				this._areaBlocks33,
				this._areaBlocks34,
				this._areaBlocks35,
				this._areaBlocks36,
				this._areaBlocks37,
				this._areaBlocks38,
				this._areaBlocks39,
				this._areaBlocks40,
				this._areaBlocks41,
				this._areaBlocks42,
				this._areaBlocks43,
				this._areaBlocks44
			};
			this._regularAreaBlocksAddFuncs = new Action<short, MapBlockData, DataContext>[]
			{
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks0),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks1),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks2),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks3),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks4),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks5),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks6),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks7),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks8),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks9),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks10),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks11),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks12),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks13),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks14),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks15),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks16),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks17),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks18),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks19),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks20),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks21),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks22),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks23),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks24),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks25),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks26),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks27),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks28),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks29),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks30),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks31),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks32),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks33),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks34),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks35),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks36),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks37),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks38),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks39),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks40),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks41),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks42),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks43),
				new Action<short, MapBlockData, DataContext>(this.AddElement_AreaBlocks44)
			};
			this._regularAreaBlocksSetFuncs = new Action<short, MapBlockData, DataContext>[]
			{
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks0),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks1),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks2),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks3),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks4),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks5),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks6),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks7),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks8),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks9),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks10),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks11),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks12),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks13),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks14),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks15),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks16),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks17),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks18),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks19),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks20),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks21),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks22),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks23),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks24),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks25),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks26),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks27),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks28),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks29),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks30),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks31),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks32),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks33),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks34),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks35),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks36),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks37),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks38),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks39),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks40),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks41),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks42),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks43),
				new Action<short, MapBlockData, DataContext>(this.SetElement_AreaBlocks44)
			};
		}

		// Token: 0x060078CB RID: 30923 RVA: 0x00468030 File Offset: 0x00466230
		public unsafe void GetPassableBlocksInArea(short areaId, List<MapBlockData> result)
		{
			result.Clear();
			Span<MapBlockData> areaBlocks = this.GetAreaBlocks(areaId);
			Span<MapBlockData> span = areaBlocks;
			for (int i = 0; i < span.Length; i++)
			{
				MapBlockData block = *span[i];
				bool flag = block.IsPassable();
				if (flag)
				{
					result.Add(block);
				}
			}
		}

		// Token: 0x060078CC RID: 30924 RVA: 0x00468080 File Offset: 0x00466280
		public Span<MapBlockData> GetAreaBlocks(short areaId)
		{
			Span<MapBlockData> result;
			if (areaId >= 45)
			{
				if (areaId >= 135)
				{
					switch (areaId)
					{
					case 135:
						result = new Span<MapBlockData>(this._bornAreaBlocks.GetArray(), 0, this._bornAreaBlocks.Count);
						break;
					case 136:
						result = new Span<MapBlockData>(this._guideAreaBlocks.GetArray(), 0, this._guideAreaBlocks.Count);
						break;
					case 137:
						result = new Span<MapBlockData>(this._secretVillageAreaBlocks.GetArray(), 0, this._secretVillageAreaBlocks.Count);
						break;
					case 138:
						result = new Span<MapBlockData>(this._brokenPerformAreaBlocks.GetArray(), 0, this._brokenPerformAreaBlocks.Count);
						break;
					default:
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Invalid area id: ");
						defaultInterpolatedStringHandler.AppendFormatted<short>(areaId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					}
				}
				else
				{
					MapBlockData[] allBrokenAreaBlocks = this._brokenAreaBlocks.GetArray();
					bool flag = allBrokenAreaBlocks.Length == 0;
					if (flag)
					{
						result = new Span<MapBlockData>(allBrokenAreaBlocks, 0, 0);
					}
					else
					{
						int blockPerArea = 25;
						int blockIdBegin = (int)((short)(blockPerArea * (int)(areaId - 45)));
						result = new Span<MapBlockData>(allBrokenAreaBlocks, blockIdBegin, blockPerArea);
					}
				}
			}
			else
			{
				AreaBlockCollection blockCollection = this.GetRegularAreaBlocks(areaId);
				result = new Span<MapBlockData>(blockCollection.GetArray(), 0, blockCollection.Count);
			}
			return result;
		}

		// Token: 0x060078CD RID: 30925 RVA: 0x004681D8 File Offset: 0x004663D8
		public void SetBlockData(DataContext context, MapBlockData block)
		{
			short areaId = block.AreaId;
			short num = areaId;
			if (num >= 45)
			{
				if (num >= 135)
				{
					switch (num)
					{
					case 135:
						this.SetElement_BornAreaBlocks(block.BlockId, block, context);
						break;
					case 136:
						this.SetElement_GuideAreaBlocks(block.BlockId, block, context);
						break;
					case 137:
						this.SetElement_SecretVillageAreaBlocks(block.BlockId, block, context);
						break;
					case 138:
						this.SetElement_BrokenPerformAreaBlocks(block.BlockId, block, context);
						break;
					default:
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Invalid area id: ");
						defaultInterpolatedStringHandler.AppendFormatted<short>(block.AreaId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					}
				}
				else
				{
					short blockId = 25 * (block.AreaId - 45) + block.BlockId;
					this.SetElement_BrokenAreaBlocks(blockId, block, context);
				}
			}
			else
			{
				this.SetRegularBlockData(context, block);
			}
		}

		// Token: 0x060078CE RID: 30926 RVA: 0x004682C0 File Offset: 0x004664C0
		public void OnCharacterLocationChanged(DataContext context, int charId, Location srcLocation, Location destLocation)
		{
			bool flag = srcLocation.IsValid();
			if (flag)
			{
				MapBlockData block = this.GetBlock(srcLocation);
				bool flag2 = block.RemoveCharacter(charId);
				if (flag2)
				{
					this.SetBlockData(context, block);
				}
			}
			bool flag3 = destLocation.IsValid();
			if (flag3)
			{
				MapBlockData block2 = this.GetBlock(destLocation);
				block2.AddCharacter(charId);
				this.SetBlockData(context, block2);
				DomainManager.Taiwu.CheckNotTaiwu(charId);
			}
		}

		// Token: 0x060078CF RID: 30927 RVA: 0x00468330 File Offset: 0x00466530
		public void OnInfectedCharacterLocationChanged(DataContext context, int charId, Location srcLocation, Location destLocation)
		{
			bool flag = srcLocation.IsValid();
			if (flag)
			{
				MapBlockData block = this.GetBlock(srcLocation);
				bool flag2 = block.RemoveInfectedCharacter(charId);
				if (flag2)
				{
					this.SetBlockData(context, block);
				}
			}
			bool flag3 = destLocation.IsValid();
			if (flag3)
			{
				MapBlockData block2 = this.GetBlock(destLocation);
				block2.AddInfectedCharacter(charId);
				this.SetBlockData(context, block2);
				DomainManager.Taiwu.CheckNotTaiwu(charId);
			}
		}

		// Token: 0x060078D0 RID: 30928 RVA: 0x004683A0 File Offset: 0x004665A0
		public void OnFixedCharacterLocationChanged(DataContext context, int charId, Location srcLocation, Location destLocation)
		{
			bool flag = srcLocation.IsValid();
			if (flag)
			{
				MapBlockData block = this.GetBlock(srcLocation);
				bool flag2 = block.RemoveFixedCharacter(charId);
				if (flag2)
				{
					this.SetBlockData(context, block);
				}
			}
			bool flag3 = destLocation.IsValid();
			if (flag3)
			{
				MapBlockData block2 = this.GetBlock(destLocation);
				block2.AddFixedCharacter(charId);
				this.SetBlockData(context, block2);
				this.SetBlockAndViewRangeVisible(context, destLocation.AreaId, destLocation.BlockId);
			}
		}

		// Token: 0x060078D1 RID: 30929 RVA: 0x00468418 File Offset: 0x00466618
		public void OnEnemyCharacterLocationChanged(DataContext context, int charId, Location srcLocation, Location destLocation)
		{
			bool flag = srcLocation.IsValid();
			if (flag)
			{
				MapBlockData block = this.GetBlock(srcLocation);
				bool flag2 = block.RemoveEnemyCharacter(charId);
				if (flag2)
				{
					this.SetBlockData(context, block);
				}
			}
			bool flag3 = destLocation.IsValid();
			if (flag3)
			{
				MapBlockData block2 = this.GetBlock(destLocation);
				block2.AddEnemyCharacter(charId);
				this.SetBlockData(context, block2);
			}
		}

		// Token: 0x060078D2 RID: 30930 RVA: 0x0046847C File Offset: 0x0046667C
		public void OnGraveLocationChanged(DataContext context, int charId, Location srcLocation, Location destLocation)
		{
			bool flag = srcLocation.IsValid();
			if (flag)
			{
				MapBlockData block = this.GetBlock(srcLocation);
				bool flag2 = block.RemoveGrave(charId);
				if (flag2)
				{
					this.SetBlockData(context, block);
				}
			}
			bool flag3 = destLocation.IsValid();
			if (flag3)
			{
				MapBlockData block2 = this.GetBlock(destLocation);
				block2.AddGrave(charId);
				this.SetBlockData(context, block2);
			}
		}

		// Token: 0x060078D3 RID: 30931 RVA: 0x004684E0 File Offset: 0x004666E0
		public void OnTemplateEnemyLocationChanged(DataContext context, MapTemplateEnemyInfo templateEnemyInfo, Location srcLocation, Location destLocation)
		{
			bool flag = srcLocation.IsValid();
			if (flag)
			{
				MapBlockData block = this.GetBlock(srcLocation);
				bool flag2 = block.RemoveTemplateEnemy(templateEnemyInfo);
				if (flag2)
				{
					this.SetBlockData(context, block);
				}
			}
			bool flag3 = destLocation.IsValid();
			if (flag3)
			{
				MapBlockData block2 = this.GetBlock(destLocation);
				templateEnemyInfo.BlockId = destLocation.BlockId;
				block2.AddTemplateEnemy(new MapTemplateEnemyInfo(templateEnemyInfo.TemplateId, destLocation.BlockId, templateEnemyInfo.SourceAdventureBlockId, templateEnemyInfo.Duration));
				this.SetBlockData(context, block2);
			}
		}

		// Token: 0x060078D4 RID: 30932 RVA: 0x0046856C File Offset: 0x0046676C
		public int GetStationUnlockedRegularAreaCount()
		{
			int count = 0;
			for (short areaId = 0; areaId < 45; areaId += 1)
			{
				bool stationUnlocked = this._areas[(int)areaId].StationUnlocked;
				if (stationUnlocked)
				{
					count++;
				}
			}
			return count;
		}

		// Token: 0x060078D5 RID: 30933 RVA: 0x004685AC File Offset: 0x004667AC
		[SingleValueCollectionDependency(19, new ushort[]
		{
			163
		})]
		private void CalcLoongLocations(List<LoongLocationData> value)
		{
			value.Clear();
			foreach (LoongInfo loongInfo in DomainManager.Extra.FiveLoongDict.Values)
			{
				bool flag = !loongInfo.IsDisappear;
				if (flag)
				{
					value.Add(new LoongLocationData(loongInfo));
				}
			}
		}

		// Token: 0x060078D6 RID: 30934 RVA: 0x00468620 File Offset: 0x00466820
		[SingleValueCollectionDependency(19, new ushort[]
		{
			194
		})]
		private void CalcFleeBeasts(List<Location> value)
		{
			value.Clear();
			foreach (Location location in DomainManager.Extra.GetAllFleeBeastLocations())
			{
				bool flag = !value.Contains(location);
				if (flag)
				{
					value.Add(location);
				}
			}
		}

		// Token: 0x060078D7 RID: 30935 RVA: 0x0046868C File Offset: 0x0046688C
		[SingleValueCollectionDependency(19, new ushort[]
		{
			194
		})]
		private void CalcFleeLoongs(List<Location> value)
		{
			value.Clear();
			foreach (Location location in DomainManager.Extra.GetAllFleeJiaoLoongLocations())
			{
				bool flag = !value.Contains(location);
				if (flag)
				{
					value.Add(location);
				}
			}
		}

		// Token: 0x060078D8 RID: 30936 RVA: 0x004686F8 File Offset: 0x004668F8
		[SingleValueCollectionDependency(19, new ushort[]
		{
			195
		})]
		private void CalcAlterSettlementLocations(List<Location> value)
		{
			value.Clear();
			foreach (short settlementId in DomainManager.Extra.GetAllAlteredSettlementIds())
			{
				Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
				value.Add(settlement.GetLocation());
			}
		}

		// Token: 0x060078D9 RID: 30937 RVA: 0x00468768 File Offset: 0x00466968
		[SingleValueCollectionDependency(19, new ushort[]
		{
			281
		})]
		[SingleValueCollectionDependency(1, new ushort[]
		{
			1
		})]
		[SingleValueCollectionDependency(1, new ushort[]
		{
			27
		})]
		[ObjectCollectionDependency(4, 0, new ushort[]
		{
			47
		}, Condition = InfluenceCondition.CharIsTaiwu)]
		[SingleValueCollectionDependency(5, new ushort[]
		{
			46
		})]
		private void CalcVisibleMapPickups(List<MapElementPickupDisplayData> value)
		{
			value.Clear();
			foreach (MapPickupCollection collection in DomainManager.Extra.PickupDict.Values)
			{
				bool flag = collection == null || collection.Count <= 0;
				if (!flag)
				{
					List<MapPickup> pickups = collection.Where(new Func<MapPickup, bool>(MapPickupHelper.IsVisible)).ToList<MapPickup>();
					pickups.Sort(new Comparison<MapPickup>(MapPickupHelper.CompareVisiblePickups));
					value.AddRange(pickups.Select(new Func<MapPickup, MapElementPickupDisplayData>(this.GetPickupDisplayData)));
				}
			}
		}

		// Token: 0x060078DA RID: 30938 RVA: 0x00468820 File Offset: 0x00466A20
		public void AddHunterAnimal(DataContext context, short areaId, short blockId, short animalId)
		{
			this._hunterAnimalsCache.Add(new HunterAnimalKey(areaId, blockId, animalId));
			this.SetHunterAnimalsCache(this._hunterAnimalsCache, context);
		}

		// Token: 0x060078DB RID: 30939 RVA: 0x00468848 File Offset: 0x00466A48
		public void MoveHunterAnimal(DataContext context, Location before, Location after, short animalId)
		{
			HunterAnimalKey beforeAnimal = new HunterAnimalKey(before.AreaId, before.BlockId, animalId);
			bool flag = !this._hunterAnimalsCache.Contains(beforeAnimal);
			if (!flag)
			{
				this._hunterAnimalsCache.Remove(beforeAnimal);
				HunterAnimalKey afterAnimal = new HunterAnimalKey(after.AreaId, after.BlockId, animalId);
				this._hunterAnimalsCache.Add(afterAnimal);
				this.SetHunterAnimalsCache(this._hunterAnimalsCache, context);
			}
		}

		// Token: 0x060078DC RID: 30940 RVA: 0x004688C0 File Offset: 0x00466AC0
		public void RemoveHunterAnimal(DataContext context, Location location, short animalId)
		{
			HunterAnimalKey animal = new HunterAnimalKey(location.AreaId, location.BlockId, animalId);
			bool flag = !this._hunterAnimalsCache.Contains(animal);
			if (!flag)
			{
				this._hunterAnimalsCache.Remove(animal);
				this.SetHunterAnimalsCache(this._hunterAnimalsCache, context);
			}
		}

		// Token: 0x060078DD RID: 30941 RVA: 0x00468914 File Offset: 0x00466B14
		public void ClearHunterAnim(DataContext context)
		{
			bool flag = this._hunterAnimalsCache.Count == 0;
			if (!flag)
			{
				this._hunterAnimalsCache.Clear();
				this.SetHunterAnimalsCache(this._hunterAnimalsCache, context);
			}
		}

		// Token: 0x060078DE RID: 30942 RVA: 0x00468950 File Offset: 0x00466B50
		private static void AddAnimalProfessionSeniority(DataContext context, Location destLocation)
		{
			ProfessionData professionData;
			bool flag = !DomainManager.Extra.TryGetElement_TaiwuProfessions(1, out professionData);
			if (!flag)
			{
				bool flag2 = !destLocation.IsValid();
				if (!flag2)
				{
					List<GameData.Domains.Character.Animal> animals;
					bool flag3 = !DomainManager.Extra.TryGetAnimalsByLocation(destLocation, out animals);
					if (!flag3)
					{
						ProfessionFormulaItem formula = ProfessionFormula.Instance[8];
						foreach (GameData.Domains.Character.Animal animal in animals)
						{
							bool flag4 = !DomainManager.Extra.TryTriggerAddSeniorityPoint(context, formula.TemplateId, animal.Id);
							if (!flag4)
							{
								sbyte consummateLevel = Config.Character.Instance[animal.CharacterTemplateId].ConsummateLevel;
								int addSeniority = formula.Calculate((int)consummateLevel);
								DomainManager.Extra.ChangeProfessionSeniority(context, 1, addSeniority, true, false);
							}
						}
					}
				}
			}
		}

		// Token: 0x060078DF RID: 30943 RVA: 0x00468A4C File Offset: 0x00466C4C
		[DomainMethod]
		public AreaDisplayData[] GetAllAreaDisplayData()
		{
			AreaDisplayData[] ret = new AreaDisplayData[135];
			for (short areaId = 0; areaId < 135; areaId += 1)
			{
				ret[(int)areaId] = this.GetAreaDisplayData(areaId);
			}
			return ret;
		}

		// Token: 0x060078E0 RID: 30944 RVA: 0x00468A90 File Offset: 0x00466C90
		public AreaDisplayData GetAreaDisplayData(short areaId)
		{
			List<GameData.Domains.Character.Character> cache = ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Get();
			cache.Clear();
			MapCharacterFilter.FindInfected(new Predicate<GameData.Domains.Character.Character>(CharacterMatchers.MatchCompletelyInfected), cache, areaId);
			int infectedCount = cache.Count;
			cache.Clear();
			MapCharacterFilter.Find(new Predicate<GameData.Domains.Character.Character>(CharacterMatchers.MatchNotTaiwuOwnedLegendaryBook), cache, areaId, true);
			int legendaryBookOwnerCount = cache.Count;
			ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(cache);
			bool anyPurpleBamboo = DomainManager.Map.IsContainsPurpleBamboo(areaId);
			AreaAdventureData adventureData = DomainManager.Adventure.GetElement_AdventureAreas((int)areaId);
			AreaDisplayData result = default(AreaDisplayData);
			result.IsBroken = (areaId >= 45);
			result.AnyPurpleBamboo = anyPurpleBamboo;
			result._loongStatusInternal = DomainManager.Extra.GetAreaLoongStatus(areaId);
			result.AnyFleeBeast = DomainManager.Map.GetFleeBeasts().Any((Location x) => x.AreaId == areaId);
			result.AnyFleeLoongson = DomainManager.Map.GetFleeLoongs().Any((Location x) => x.AreaId == areaId);
			result.InfectedCount = infectedCount;
			result.LegendaryCount = legendaryBookOwnerCount;
			result.BrokenLevel = DomainManager.Map.QueryAreaBrokenLevel(areaId);
			result.PurpleBambooTemplateIds = (anyPurpleBamboo ? new List<short>(DomainManager.Map.IterAreaPurpleBambooTemplateIds(areaId)) : null);
			result.AdventureTemplates = new List<short>(from adv in adventureData.AdventureSites.Values
			where adv.SiteState >= 1
			select adv.TemplateId);
			result.PastLifeRelationCount = DomainManager.Extra.GetAreaPastLifeRelationCount(areaId);
			result.HasSectZhujianSpecialMerchant = (DomainManager.Taiwu.GetAreaMerchantInfo(areaId).Item1 == OpenShopEventArguments.EMerchantSourceType.SpecialBuilding);
			return result;
		}

		// Token: 0x060078E1 RID: 30945 RVA: 0x00468CA4 File Offset: 0x00466EA4
		[DomainMethod]
		public unsafe List<MapBlockDisplayData> GetBlockDisplayDataInArea(short areaId)
		{
			Span<MapBlockData> blocks = this.GetAreaBlocks(areaId);
			this._blockDisplayDataCache.Clear();
			this._blockDisplayDataCache.EnsureCapacity(blocks.Length);
			List<GameData.Domains.Character.Character> characterCache = ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Get();
			for (int i = 0; i < blocks.Length; i++)
			{
				MapBlockData block = *blocks[i];
				MapBlockDisplayData displayData = this.GetMapBlockDisplayData(block.GetLocation(), -1, characterCache);
				this._blockDisplayDataCache.Add(displayData);
			}
			ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(characterCache);
			return this._blockDisplayDataCache;
		}

		// Token: 0x060078E2 RID: 30946 RVA: 0x00468D40 File Offset: 0x00466F40
		private unsafe MapBlockDisplayData GetMapBlockDisplayData(Location location, int professionId, List<GameData.Domains.Character.Character> characters)
		{
			MapDomain.<>c__DisplayClass100_0 CS$<>8__locals1 = new MapDomain.<>c__DisplayClass100_0();
			CS$<>8__locals1.characters = characters;
			MapBlockDisplayData data = new MapBlockDisplayData
			{
				TreasureExpect = DomainManager.Extra.FindTreasureExpect(location),
				ProfessionId = professionId,
				Count0 = 0,
				Count1 = 0,
				Count2 = 0
			};
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			CS$<>8__locals1.block = DomainManager.Map.GetBlock(location.AreaId, location.BlockId);
			if (professionId <= 1)
			{
				if (professionId != 0)
				{
					if (professionId == 1)
					{
						data.Count0 = DomainManager.Extra.QueryAnimalCount(location);
					}
				}
				else
				{
					data.Count0 = (CS$<>8__locals1.block.Destroyed ? 1 : 0);
				}
			}
			else if (professionId != 5)
			{
				switch (professionId)
				{
				case 10:
					foreach (int charId in CS$<>8__locals1.<GetMapBlockDisplayData>g__IterCharIds|0(true))
					{
						HashSet<int> characterSet = DomainManager.Character.GetRelatedCharIds(charId, 32768);
						bool flag = characterSet.Contains(taiwuCharId);
						if (flag)
						{
							data.Count0++;
						}
					}
					break;
				case 11:
					break;
				case 12:
					CS$<>8__locals1.<GetMapBlockDisplayData>g__QueryCharacters|1(true);
					foreach (GameData.Domains.Character.Character character in CS$<>8__locals1.characters)
					{
						sbyte behaviorType = character.GetBehaviorType();
						bool flag2 = behaviorType == 3;
						if (flag2)
						{
							data.Count0++;
						}
						else
						{
							bool flag3 = behaviorType == 4;
							if (flag3)
							{
								data.Count1++;
							}
						}
					}
					break;
				case 13:
					CS$<>8__locals1.<GetMapBlockDisplayData>g__QueryCharacters|1(true);
					foreach (GameData.Domains.Character.Character character2 in CS$<>8__locals1.characters)
					{
						bool flag4 = character2.GetInjuries().HasAnyInjury();
						if (flag4)
						{
							data.Count0++;
						}
						PoisonInts poisoned = *character2.GetPoisoned();
						bool flag5 = poisoned.IsNonZero();
						if (flag5)
						{
							data.Count1++;
						}
						bool flag6 = character2.GetDisorderOfQi() > 0;
						if (flag6)
						{
							data.Count2++;
						}
					}
					break;
				default:
					if (professionId == 17)
					{
						foreach (int charId2 in CS$<>8__locals1.<GetMapBlockDisplayData>g__IterCharIds|0(false))
						{
							bool flag7 = ProfessionSkillHandle.DukeSkill_CheckCharacterHasTitle(charId2);
							if (flag7)
							{
								data.Count0++;
							}
						}
					}
					break;
				}
			}
			else
			{
				CS$<>8__locals1.<GetMapBlockDisplayData>g__QueryCharacters|1(false);
				foreach (GameData.Domains.Character.Character character3 in CS$<>8__locals1.characters)
				{
					List<short> featureIds = character3.GetFeatureIds();
					bool flag8 = featureIds.Contains(217);
					if (flag8)
					{
						data.Count0++;
					}
					else
					{
						bool flag9 = featureIds.Contains(218);
						if (flag9)
						{
							data.Count1++;
						}
					}
				}
			}
			return data;
		}

		// Token: 0x060078E3 RID: 30947 RVA: 0x004690E4 File Offset: 0x004672E4
		[DomainMethod]
		public int[] GetAllAreaCompletelyInfectedCharCount()
		{
			List<GameData.Domains.Character.Character> charList = ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Get();
			int[] result = new int[135];
			for (short areaId = 0; areaId < 135; areaId += 1)
			{
				charList.Clear();
				MapCharacterFilter.FindInfected(new Predicate<GameData.Domains.Character.Character>(CharacterMatchers.MatchCompletelyInfected), charList, areaId);
				result[(int)areaId] = charList.Count;
			}
			ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(charList);
			return result;
		}

		// Token: 0x060078E4 RID: 30948 RVA: 0x00469158 File Offset: 0x00467358
		[DomainMethod]
		public int[] GetAllStateCompletelyInfectedCharCount()
		{
			List<GameData.Domains.Character.Character> charList = ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Get();
			int[] result = new int[15];
			for (sbyte stateId = 0; stateId < 15; stateId += 1)
			{
				charList.Clear();
				MapCharacterFilter.FindStateInfected(new Predicate<GameData.Domains.Character.Character>(CharacterMatchers.MatchCompletelyInfected), charList, stateId);
				result[(int)stateId] = charList.Count;
			}
			ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(charList);
			return result;
		}

		// Token: 0x060078E5 RID: 30949 RVA: 0x004691C4 File Offset: 0x004673C4
		[DomainMethod]
		public Dictionary<TravelRouteKey, TravelRoute> GetTravelRoutesInState(sbyte stateId)
		{
			bool flag = stateId < 0;
			Dictionary<TravelRouteKey, TravelRoute> result;
			if (flag)
			{
				result = null;
			}
			else
			{
				Dictionary<TravelRouteKey, TravelRoute> routeDict = new Dictionary<TravelRouteKey, TravelRoute>();
				List<short> areaIdList = ObjectPool<List<short>>.Instance.Get();
				Dictionary<TravelRouteKey, TravelRoute> findRouteDict = DomainManager.World.GetWorldFunctionsStatus(4) ? this._travelRouteDict : this._bornStateTravelRouteDict;
				this.GetAllAreaInState(stateId, areaIdList);
				for (int i = 0; i < areaIdList.Count - 1; i++)
				{
					for (int j = i + 1; j < areaIdList.Count; j++)
					{
						TravelRouteKey key = new TravelRouteKey(areaIdList[i], areaIdList[j]);
						routeDict.Add(key, findRouteDict[key]);
					}
				}
				ObjectPool<List<short>>.Instance.Return(areaIdList);
				result = routeDict;
			}
			return result;
		}

		// Token: 0x060078E6 RID: 30950 RVA: 0x00469294 File Offset: 0x00467494
		public Location CrossAreaTravelInfoToLocation(CrossAreaMoveInfo crossAreaMoveInfos)
		{
			bool flag = crossAreaMoveInfos.CostedDays == 0;
			Location result;
			if (flag)
			{
				result = new Location(crossAreaMoveInfos.FromAreaId, crossAreaMoveInfos.FromBlockId);
			}
			else
			{
				int days = 0;
				for (int i = 0; i < crossAreaMoveInfos.Route.CostList.Count; i++)
				{
					short cost = crossAreaMoveInfos.Route.CostList[i];
					days += (int)cost;
					bool flag2 = days >= crossAreaMoveInfos.CostedDays;
					if (flag2)
					{
						short areaId = crossAreaMoveInfos.Route.AreaList[i];
						MapAreaData areaData = DomainManager.Map.GetElement_Areas((int)areaId);
						return new Location(areaId, areaData.StationBlockId);
					}
				}
				result = Location.Invalid;
			}
			return result;
		}

		// Token: 0x060078E7 RID: 30951 RVA: 0x00469351 File Offset: 0x00467551
		public void GetAllAreaInState(sbyte stateId, List<short> areaList)
		{
			GameData.Domains.Map.SharedMethods.GetAreaListInState(stateId, areaList);
		}

		// Token: 0x060078E8 RID: 30952 RVA: 0x0046935C File Offset: 0x0046755C
		public void GetAllRegularAreaInState(sbyte stateId, List<short> areaList)
		{
			GameData.Domains.Map.SharedMethods.GetRegularAreaListInState(stateId, areaList);
		}

		// Token: 0x060078E9 RID: 30953 RVA: 0x00469367 File Offset: 0x00467567
		public void GetAllBrokenAreaInState(sbyte stateId, List<short> areaList)
		{
			GameData.Domains.Map.SharedMethods.GetBrokenAreaListInState(stateId, areaList);
		}

		// Token: 0x060078EA RID: 30954 RVA: 0x00469374 File Offset: 0x00467574
		public byte GetAreaSize(short areaId)
		{
			bool flag = areaId < 45 || areaId >= 135;
			byte result;
			if (flag)
			{
				MapAreaItem areaConfigData = this._areas[(int)areaId].GetConfig();
				short taiwuVillageSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
				bool flag2 = taiwuVillageSettlementId >= 0 && DomainManager.Organization.GetSettlement(taiwuVillageSettlementId).GetLocation().AreaId == areaId;
				if (flag2)
				{
					result = (byte)GlobalConfig.Instance.TaiwuVillageForceAreaSize;
				}
				else
				{
					result = areaConfigData.Size;
				}
			}
			else
			{
				result = 5;
			}
			return result;
		}

		// Token: 0x060078EB RID: 30955 RVA: 0x004693F8 File Offset: 0x004675F8
		public bool IsAreaBroken(short areaId)
		{
			int brokenAreaIdStart = 45;
			int brokenAreaIdEnd = 135;
			return (int)areaId >= brokenAreaIdStart && (int)areaId < brokenAreaIdEnd;
		}

		// Token: 0x060078EC RID: 30956 RVA: 0x00469420 File Offset: 0x00467620
		public short GetMainSettlementMainBlockId(short areaId)
		{
			MapAreaData area = this._areas[(int)areaId];
			SettlementInfo mainSettlementInfo = area.SettlementInfos[0];
			return mainSettlementInfo.BlockId;
		}

		// Token: 0x060078ED RID: 30957 RVA: 0x00469450 File Offset: 0x00467650
		public short GetNearestSettlementIdByLocation(Location location)
		{
			MapAreaData areaMapData = DomainManager.Map.GetElement_Areas((int)location.AreaId);
			byte areaSize = DomainManager.Map.GetAreaSize(location.AreaId);
			ByteCoordinate selfCoordinate = ByteCoordinate.IndexToCoordinate(location.BlockId, areaSize);
			int initDistance = int.MaxValue;
			short settlementId = -1;
			for (int i = 0; i < areaMapData.SettlementInfos.Length; i++)
			{
				SettlementInfo settlementInfo = areaMapData.SettlementInfos[i];
				int distance = selfCoordinate.GetManhattanDistance(ByteCoordinate.IndexToCoordinate(settlementInfo.BlockId, areaSize));
				bool flag = distance < initDistance;
				if (flag)
				{
					initDistance = distance;
					settlementId = settlementInfo.SettlementId;
				}
			}
			return settlementId;
		}

		// Token: 0x060078EE RID: 30958 RVA: 0x004694FC File Offset: 0x004676FC
		public ByteCoordinate GetWorldPos(short areaId)
		{
			sbyte[] pos = this._areas[(int)areaId].GetConfig().WorldMapPos;
			return new ByteCoordinate((byte)pos[0], (byte)pos[1]);
		}

		// Token: 0x060078EF RID: 30959 RVA: 0x00469530 File Offset: 0x00467730
		public sbyte GetStateIdByStateTemplateId(short stateTemplateId)
		{
			return (sbyte)(stateTemplateId - 1);
		}

		// Token: 0x060078F0 RID: 30960 RVA: 0x00469548 File Offset: 0x00467748
		public sbyte GetStateIdByAreaId(short areaId)
		{
			return this.GetStateIdByStateTemplateId((short)this._areas[(int)areaId].GetConfig().StateID);
		}

		// Token: 0x060078F1 RID: 30961 RVA: 0x00469574 File Offset: 0x00467774
		public sbyte GetStateTemplateIdByAreaId(short areaId)
		{
			return this._areas[(int)areaId].GetConfig().StateID;
		}

		// Token: 0x060078F2 RID: 30962 RVA: 0x00469598 File Offset: 0x00467798
		[return: TupleElementNames(new string[]
		{
			"stateName",
			"areaName"
		})]
		public ValueTuple<string, string> GetStateAndAreaNameByAreaId(short areaId)
		{
			MapAreaItem areaConfig = this._areas[(int)areaId].GetConfig();
			string areaName = areaConfig.Name;
			string stateName = MapState.Instance[areaConfig.StateID].Name;
			return new ValueTuple<string, string>(stateName, areaName);
		}

		// Token: 0x060078F3 RID: 30963 RVA: 0x004695DC File Offset: 0x004677DC
		public short GetAreaIdByAreaTemplateId(short areaTemplateId)
		{
			short areaId = 0;
			while ((int)areaId < this._areas.Length)
			{
				bool flag = this._areas[(int)areaId].GetTemplateId() == areaTemplateId;
				if (flag)
				{
					return areaId;
				}
				areaId += 1;
			}
			return -1;
		}

		// Token: 0x060078F4 RID: 30964 RVA: 0x00469620 File Offset: 0x00467820
		public void GetEdgeBlockList(short areaId, List<short> blockIdList, bool excludeTravelBlock = false, bool strictSelect = true)
		{
			HashSet<short> edgeTemplates = ObjectPool<HashSet<short>>.Instance.Get();
			edgeTemplates.Clear();
			edgeTemplates.Add(126);
			this.GetEdgeBlockList(areaId, blockIdList, null, excludeTravelBlock, strictSelect, false);
			ObjectPool<HashSet<short>>.Instance.Return(edgeTemplates);
		}

		// Token: 0x060078F5 RID: 30965 RVA: 0x00469664 File Offset: 0x00467864
		public unsafe void GetEdgeBlockList(short areaId, List<short> blockIdList, ISet<short> edgeTemplates, bool excludeTravelBlock = false, bool strictSelect = true, bool containsBigBlock = false)
		{
			byte areaSize = this.GetAreaSize(areaId);
			Span<MapBlockData> blocks = this.GetAreaBlocks(areaId);
			int blockCount = blocks.Length;
			List<short> travelBlocks = null;
			byte minX = byte.MaxValue;
			byte minY = byte.MaxValue;
			byte maxX = 0;
			byte maxY = 0;
			if (excludeTravelBlock)
			{
				travelBlocks = ObjectPool<List<short>>.Instance.Get();
				travelBlocks.Clear();
			}
			if (strictSelect)
			{
				for (byte x = 0; x < areaSize; x += 1)
				{
					for (byte y = 0; y < areaSize; y += 1)
					{
						MapBlockData block = *blocks[(int)ByteCoordinate.CoordinateToIndex(new ByteCoordinate(x, y), areaSize)];
						bool flag = block.IsPassable() && (!containsBigBlock && block.RootBlockId < 0) && block.GroupBlockList == null;
						if (flag)
						{
							minX = Math.Min(x, minX);
							minY = Math.Min(y, minY);
							maxX = Math.Max(x, maxX);
							maxY = Math.Max(y, maxY);
						}
					}
				}
			}
			blockIdList.Clear();
			short blockId = 0;
			while ((int)blockId < blockCount)
			{
				MapBlockData block2 = *blocks[(int)blockId];
				bool flag2 = !block2.IsPassable() || (!containsBigBlock && block2.RootBlockId >= 0) || (block2.GroupBlockList != null && block2.GroupBlockList.Count > 0);
				if (!flag2)
				{
					ByteCoordinate pos = ByteCoordinate.IndexToCoordinate(blockId, areaSize);
					bool isEdge = pos.X == 0 || pos.X == areaSize - 1 || pos.Y == 0 || pos.Y == areaSize - 1;
					bool flag3 = !isEdge;
					if (flag3)
					{
						if (strictSelect)
						{
							bool nearLeft = pos.X < areaSize / 2;
							bool nearRight = !nearLeft;
							bool nearBottom = pos.Y < areaSize / 2;
							bool nearTop = !nearBottom;
							int xDist = (int)(nearLeft ? pos.X : (areaSize - pos.X));
							int yDist = (int)(nearBottom ? pos.Y : (areaSize - pos.Y));
							bool flag4 = xDist < yDist;
							if (flag4)
							{
								nearTop = (nearBottom = false);
							}
							else
							{
								bool flag5 = xDist > yDist;
								if (flag5)
								{
									nearRight = (nearLeft = false);
								}
							}
							isEdge = ((!nearLeft || pos.X == minX) && (!nearRight || pos.X == maxX) && (!nearBottom || pos.Y == minY) && (!nearTop || pos.Y == maxY));
						}
						else
						{
							isEdge = (edgeTemplates.Contains(blocks[(int)(blockId - 1)]->TemplateId) || edgeTemplates.Contains(blocks[(int)(blockId + 1)]->TemplateId) || edgeTemplates.Contains(blocks[(int)(blockId - (short)areaSize)]->TemplateId) || edgeTemplates.Contains(blocks[(int)(blockId + (short)areaSize)]->TemplateId));
						}
					}
					bool flag6 = isEdge && (!excludeTravelBlock || !travelBlocks.Contains(blockId));
					if (flag6)
					{
						blockIdList.Add(blockId);
					}
				}
				blockId += 1;
			}
			if (excludeTravelBlock)
			{
				ObjectPool<List<short>>.Instance.Return(travelBlocks);
			}
		}

		// Token: 0x060078F6 RID: 30966 RVA: 0x004699B4 File Offset: 0x00467BB4
		public short GetRandomSettlementId(sbyte stateId, IRandomSource random, bool containsMainCityAndSect = false)
		{
			int regularAreasPerState = 3;
			List<short> randomPool = ObjectPool<List<short>>.Instance.Get();
			short settlementId = -1;
			randomPool.Clear();
			for (int index = 0; index < regularAreasPerState; index++)
			{
				short areaId = (short)((int)stateId * regularAreasPerState + index);
				foreach (SettlementInfo settlementInfo in this._areas[(int)areaId].SettlementInfos)
				{
					bool flag = settlementInfo.SettlementId >= 0 && (containsMainCityAndSect || this.GetBlock(areaId, settlementInfo.BlockId).BlockType == EMapBlockType.Town);
					if (flag)
					{
						randomPool.Add(settlementInfo.SettlementId);
					}
				}
			}
			bool flag2 = randomPool.Count > 0;
			if (flag2)
			{
				settlementId = randomPool[random.Next(0, randomPool.Count)];
			}
			return settlementId;
		}

		// Token: 0x060078F7 RID: 30967 RVA: 0x00469A98 File Offset: 0x00467C98
		public short GetRandomStateSettlementId(IRandomSource random, sbyte stateId, bool containsMainCity = false, bool containsSect = false)
		{
			List<short> settlementIds = ObjectPool<List<short>>.Instance.Get();
			this.GetStateSettlementIds(stateId, settlementIds, containsMainCity, containsSect);
			bool flag = settlementIds.Count == 0;
			short result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				short settlementId = settlementIds.GetRandom(random);
				ObjectPool<List<short>>.Instance.Return(settlementIds);
				result = settlementId;
			}
			return result;
		}

		// Token: 0x060078F8 RID: 30968 RVA: 0x00469AE8 File Offset: 0x00467CE8
		public void GetStateSettlementIds(sbyte stateId, List<short> settlementIds, bool containsMainCity = false, bool containsSect = false)
		{
			settlementIds.Clear();
			int regularAreasPerState = 3;
			for (int index = 0; index < regularAreasPerState; index++)
			{
				short areaId = (short)((int)stateId * regularAreasPerState + index);
				foreach (SettlementInfo settlementInfo in this._areas[(int)areaId].SettlementInfos)
				{
					bool flag = settlementInfo.SettlementId < 0;
					if (!flag)
					{
						MapBlockData blockData = this.GetBlock(areaId, settlementInfo.BlockId);
						switch (blockData.BlockType)
						{
						case EMapBlockType.City:
							if (containsMainCity)
							{
								settlementIds.Add(settlementInfo.SettlementId);
							}
							break;
						case EMapBlockType.Sect:
							if (containsSect)
							{
								settlementIds.Add(settlementInfo.SettlementId);
							}
							break;
						case EMapBlockType.Town:
							settlementIds.Add(settlementInfo.SettlementId);
							break;
						}
					}
				}
			}
		}

		// Token: 0x060078F9 RID: 30969 RVA: 0x00469BDC File Offset: 0x00467DDC
		public void GetAreaSettlementIds(short areaId, List<short> settlementIds, bool containsMainCity = false, bool containsSect = false)
		{
			settlementIds.Clear();
			foreach (SettlementInfo settlementInfo in this._areas[(int)areaId].SettlementInfos)
			{
				bool flag = settlementInfo.SettlementId < 0;
				if (!flag)
				{
					MapBlockData blockData = this.GetBlock(areaId, settlementInfo.BlockId);
					switch (blockData.BlockType)
					{
					case EMapBlockType.City:
						if (containsMainCity)
						{
							settlementIds.Add(settlementInfo.SettlementId);
						}
						break;
					case EMapBlockType.Sect:
						if (containsSect)
						{
							settlementIds.Add(settlementInfo.SettlementId);
						}
						break;
					case EMapBlockType.Town:
						settlementIds.Add(settlementInfo.SettlementId);
						break;
					}
				}
			}
		}

		// Token: 0x060078FA RID: 30970 RVA: 0x00469CA0 File Offset: 0x00467EA0
		public CrossAreaMoveInfo CalcAreaTravelRoute(GameData.Domains.Character.Character character, short fromAreaId, short fromBlockId, short toAreaId)
		{
			bool flag = fromAreaId == toAreaId;
			if (flag)
			{
				throw new ArgumentException("fromAreaId cannot equals toAreaId");
			}
			bool reversePath = fromAreaId > toAreaId;
			TravelRouteKey routeKey = new TravelRouteKey(reversePath ? toAreaId : fromAreaId, reversePath ? fromAreaId : toAreaId);
			ItemKey carrierKey = character.GetEquipment()[11];
			int carrierReducePercent = 0;
			Dictionary<TravelRouteKey, TravelRoute> findRouteDict = this._travelRouteDict;
			TravelRoute route = new TravelRoute(findRouteDict[routeKey]);
			bool flag2 = reversePath;
			if (flag2)
			{
				route.PosList.Reverse();
				route.AreaList.Reverse();
				route.CostList.Reverse();
			}
			route.AreaList.RemoveAt(0);
			bool flag3 = carrierKey.IsValid();
			if (flag3)
			{
				GameData.Domains.Item.Carrier carrier = DomainManager.Item.GetElement_Carriers(carrierKey.Id);
				bool flag4 = carrier.GetCurrDurability() > 0;
				if (flag4)
				{
					carrierReducePercent = (int)carrier.GetTravelTimeReduction();
				}
			}
			for (int i = 0; i < route.CostList.Count; i++)
			{
				int timeCost = (int)route.CostList[i];
				timeCost -= timeCost * carrierReducePercent / 100;
				timeCost = Math.Max(timeCost, 1);
				route.CostList[i] = (short)timeCost;
			}
			return new CrossAreaMoveInfo
			{
				FromAreaId = fromAreaId,
				FromBlockId = fromBlockId,
				ToAreaId = toAreaId,
				Route = route
			};
		}

		// Token: 0x060078FB RID: 30971 RVA: 0x00469E04 File Offset: 0x00468004
		public bool AllowCrossAreaTravel(short fromAreaId, short toAreaId)
		{
			bool flag = fromAreaId >= 135 || toAreaId >= 135;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool reversePath = fromAreaId > toAreaId;
				TravelRouteKey routeKey = new TravelRouteKey(reversePath ? toAreaId : fromAreaId, reversePath ? fromAreaId : toAreaId);
				bool flag2 = !this._travelRouteDict.ContainsKey(routeKey);
				if (flag2)
				{
					Logger logger = MapDomain.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Invalid travel route ");
					defaultInterpolatedStringHandler.AppendFormatted<TravelRouteKey>(routeKey);
					defaultInterpolatedStringHandler.AppendLiteral(": path not exists.");
					logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					result = false;
				}
				else
				{
					Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
					result = ((fromAreaId != taiwuVillageLocation.AreaId && toAreaId != taiwuVillageLocation.AreaId) || (DomainManager.Map.GetElement_Areas((int)taiwuVillageLocation.AreaId).StationUnlocked && DomainManager.World.GetWorldFunctionsStatus(4)));
				}
			}
			return result;
		}

		// Token: 0x060078FC RID: 30972 RVA: 0x00469EF4 File Offset: 0x004680F4
		public int GetTotalTimeCost(GameData.Domains.Character.Character character, short fromAreaId, short toAreaId)
		{
			bool flag = fromAreaId == toAreaId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = !this.AllowCrossAreaTravel(fromAreaId, toAreaId);
				if (flag2)
				{
					result = int.MaxValue;
				}
				else
				{
					bool reversePath = fromAreaId > toAreaId;
					TravelRouteKey routeKey = new TravelRouteKey(reversePath ? toAreaId : fromAreaId, reversePath ? fromAreaId : toAreaId);
					ItemKey carrierKey = character.GetEquipment()[11];
					int carrierReducePercent = 0;
					bool flag3 = carrierKey.IsValid();
					if (flag3)
					{
						GameData.Domains.Item.Carrier carrier = DomainManager.Item.GetElement_Carriers(carrierKey.Id);
						bool flag4 = carrier.GetCurrDurability() > 0;
						if (flag4)
						{
							carrierReducePercent = (int)carrier.GetTravelTimeReduction();
						}
					}
					TravelRoute route = this._travelRouteDict[routeKey];
					int timeCost = 0;
					for (int i = 0; i < route.CostList.Count; i++)
					{
						timeCost += Math.Max((int)route.CostList[i] * (100 - carrierReducePercent) / 100, 1);
					}
					result = timeCost;
				}
			}
			return result;
		}

		// Token: 0x060078FD RID: 30973 RVA: 0x00469FF0 File Offset: 0x004681F0
		public unsafe static void ParallelUpdateBrokenBlockOnMonthChange(DataContext context, int areaIdInt)
		{
			ParallelModificationsRecorder recorder = context.ParallelModificationsRecorder;
			Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks((short)areaIdInt);
			for (int i = 0; i < areaBlocks.Length; i++)
			{
				MapBlockData block = *areaBlocks[i];
				bool flag = block.CountDown();
				if (flag)
				{
					recorder.RecordType(ParallelModificationType.UpdateBrokenArea);
					recorder.RecordParameterClass<MapBlockData>(block);
				}
			}
		}

		// Token: 0x060078FE RID: 30974 RVA: 0x0046A050 File Offset: 0x00468250
		public unsafe static void ParallelUpdateOnMonthChange(DataContext context, int areaIdInt)
		{
			short areaId = (short)areaIdInt;
			MapAreaData area = DomainManager.Map.GetElement_Areas((int)areaId);
			ParallelMapAreaModification mod = new ParallelMapAreaModification();
			List<sbyte> recoverResourceType = Month.Instance[DomainManager.World.GetCurrMonthInYear()].RecoverResourceType;
			int maxRecoverPercent = (int)(14 - DomainManager.World.GetWorldResourceAmountType() * 3);
			Dictionary<sbyte, List<Location>> resourceSpeedUpDict = DomainManager.Taiwu.ResourceRecoverSpeedUpDict;
			List<ValueTuple<short, short>> potentialDisasterBlocks = context.AdvanceMonthRelatedData.WeightTable.Occupy();
			Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks(areaId);
			short blockId = 0;
			while ((int)blockId < blocks.Length)
			{
				MapBlockData block = *blocks[(int)blockId];
				Location location = new Location(areaId, blockId);
				bool flag = !block.IsPassable();
				if (!flag)
				{
					bool destroyed = block.Destroyed;
					if (destroyed)
					{
						bool flag2 = DomainManager.Extra.IsMapBlockRecoveryLocked(location);
						if (flag2)
						{
							goto IL_2E2;
						}
						int currentResource = 0;
						int maxResource = 0;
						for (sbyte type = 0; type < 6; type += 1)
						{
							currentResource += (int)(*(ref block.CurrResources.Items.FixedElementField + (IntPtr)type * 2));
							maxResource += (int)(*(ref block.MaxResources.Items.FixedElementField + (IntPtr)type * 2));
						}
						bool flag3 = currentResource >= maxResource / 2;
						if (flag3)
						{
							block.Destroyed = false;
						}
					}
					int buildingBonus = DomainManager.Building.GetBuildingBlockEffect(location, EBuildingScaleEffect.MapResourceRegenBonus, -1);
					for (int i = 0; i < recoverResourceType.Count; i++)
					{
						sbyte type2 = recoverResourceType[i];
						int maxAddValue = Math.Max((int)(*(ref block.MaxResources.Items.FixedElementField + (IntPtr)type2 * 2)) * maxRecoverPercent / 100, 1);
						int addValue = context.Random.Next(maxAddValue + 1);
						addValue *= buildingBonus;
						addValue = addValue * (int)GameData.Domains.World.SharedMethods.GetGainResourcePercent(0) / 100;
						List<Location> value;
						bool flag4 = resourceSpeedUpDict.TryGetValue(type2, out value) && value.Contains(location);
						if (flag4)
						{
							addValue *= 3;
						}
						*(ref block.CurrResources.Items.FixedElementField + (IntPtr)type2 * 2) = (short)Math.Min((int)(*(ref block.CurrResources.Items.FixedElementField + (IntPtr)type2 * 2)) + addValue, (int)(*(ref block.MaxResources.Items.FixedElementField + (IntPtr)type2 * 2)));
					}
					bool flag5 = block.Items != null;
					if (flag5)
					{
						block.DestroyItems(mod.DestroyedUniqueItems);
					}
					short maxMalice = block.GetMaxMalice();
					bool disasterCondition = maxMalice > 0 && block.Malice * 100 / maxMalice >= 25 && block.RootBlockId < 0 && block.GroupBlockList == null;
					bool flag6 = block.GetConfig().SubType == EMapBlockSubType.DLCLoong;
					if (flag6)
					{
						disasterCondition = false;
					}
					bool flag7 = disasterCondition;
					if (flag7)
					{
						potentialDisasterBlocks.Add(new ValueTuple<short, short>(blockId, block.Malice * 100 / maxMalice));
					}
					block.CountDown();
				}
				IL_2E2:
				blockId += 1;
			}
			bool flag8 = areaId < 45;
			if (flag8)
			{
				MapDomain.TriggerDisasters(context.Random, areaId, potentialDisasterBlocks, mod);
			}
			context.AdvanceMonthRelatedData.WeightTable.Release(ref potentialDisasterBlocks);
			for (int j = 0; j < area.SettlementInfos.Length; j++)
			{
				SettlementInfo settlementInfo = area.SettlementInfos[j];
				bool flag9 = settlementInfo.SettlementId >= 0 && !mod.SettlementDict.ContainsKey(settlementInfo.SettlementId);
				if (flag9)
				{
					Settlement settlement = DomainManager.Organization.GetSettlement(settlementInfo.SettlementId);
					bool flag10 = settlement.GetSafety() >= settlement.GetMaxSafety() / 2;
					if (flag10)
					{
						mod.SettlementDict.Add(settlementInfo.SettlementId, new ParallelSettlementModification(settlement.GetCulture(), settlement.GetSafety(), Math.Min(settlement.GetPopulation() + 1, settlement.GetMaxPopulation())));
					}
				}
			}
			ParallelModificationsRecorder recorder = context.ParallelModificationsRecorder;
			mod.AreaId = areaId;
			recorder.RecordType(ParallelModificationType.UpdateMapArea);
			recorder.RecordParameterClass<ParallelMapAreaModification>(mod);
		}

		// Token: 0x060078FF RID: 30975 RVA: 0x0046A474 File Offset: 0x00468674
		private unsafe static void TriggerDisasters(IRandomSource random, short areaId, List<ValueTuple<short, short>> potentialDisasterBlocks, ParallelMapAreaModification mod)
		{
			potentialDisasterBlocks.Sort((ValueTuple<short, short> a, ValueTuple<short, short> b) => b.Item2.CompareTo(a.Item2));
			Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(areaId);
			List<MapBlockData> neighborBlocks = ObjectPool<List<MapBlockData>>.Instance.Get();
			for (int i = 0; i < potentialDisasterBlocks.Count; i++)
			{
				short blockId = potentialDisasterBlocks[i].Item1;
				MapBlockData block = *areaBlocks[(int)blockId];
				short maxMalice = block.GetMaxMalice();
				int malicePercentage = (int)(block.Malice * 100 / maxMalice);
				List<MapBlockData> groupBlockList = block.GroupBlockList;
				bool flag = (groupBlockList != null && groupBlockList.Count > 0) || block.RootBlockId >= 0;
				if (!flag)
				{
					for (int index = 0; index < GlobalConfig.Instance.DisasterTriggerCurrBlockThresholds.Length; index++)
					{
						sbyte currBlockThreshold = GlobalConfig.Instance.DisasterTriggerCurrBlockThresholds[index];
						bool flag2 = malicePercentage < (int)currBlockThreshold;
						if (!flag2)
						{
							sbyte triggerRange = GlobalConfig.Instance.DisasterTriggerRanges[index];
							bool flag3 = triggerRange > 0;
							if (flag3)
							{
								DomainManager.Map.GetRealNeighborBlocks(areaId, blockId, neighborBlocks, (int)triggerRange, false);
							}
							else
							{
								neighborBlocks.Clear();
							}
							int malicePercentageSum = MapDomain.GetMalicePercentageSum(neighborBlocks) + malicePercentage;
							short neighborSumThreshold = GlobalConfig.Instance.DisasterTriggerNeighborSumThresholds[index];
							bool flag4 = malicePercentageSum < (int)neighborSumThreshold;
							if (!flag4)
							{
								MapDomain map = DomainManager.Map;
								short blockId2 = blockId;
								List<MapBlockData> neighborBlocks2 = neighborBlocks;
								sbyte[] disasterTriggerRanges = GlobalConfig.Instance.DisasterTriggerRanges;
								map.GetRealNeighborBlocks(areaId, blockId2, neighborBlocks2, (int)disasterTriggerRanges[disasterTriggerRanges.Length - 1], false);
								MapDomain.TriggerDisaster(random, areaId, blockId, neighborBlocks, mod);
								break;
							}
						}
					}
				}
			}
			ObjectPool<List<MapBlockData>>.Instance.Return(neighborBlocks);
		}

		// Token: 0x06007900 RID: 30976 RVA: 0x0046A61C File Offset: 0x0046881C
		private static int GetMalicePercentageSum(List<MapBlockData> blocks)
		{
			int percentageSum = 0;
			foreach (MapBlockData block in blocks)
			{
				short maxMalice = block.GetMaxMalice();
				bool flag = maxMalice <= 0;
				if (!flag)
				{
					percentageSum += (int)(block.Malice * 100 / maxMalice);
				}
			}
			return percentageSum;
		}

		// Token: 0x06007901 RID: 30977 RVA: 0x0046A698 File Offset: 0x00468898
		private unsafe static void TriggerDisaster(IRandomSource random, short areaId, short blockId, List<MapBlockData> neighborBlocks, ParallelMapAreaModification mod)
		{
			Location blockKey = new Location(areaId, blockId);
			MapAreaData area = DomainManager.Map.GetElement_Areas((int)areaId);
			MapBlockData block = DomainManager.Map.GetBlock(blockKey);
			mod.DisasterBlocks.Add(blockId);
			short adventureId = DomainManager.Adventure.GenerateDisasterAdventureId(random, block);
			bool flag = adventureId >= 0;
			if (flag)
			{
				mod.DisasterAdventureId.Add(blockId, adventureId);
			}
			block.Destroyed = true;
			block.Malice = 0;
			foreach (MapBlockData neighborBlock in neighborBlocks)
			{
				neighborBlock.Malice = 0;
			}
			for (sbyte type = 0; type < 6; type += 1)
			{
				*(ref block.CurrResources.Items.FixedElementField + (IntPtr)type * 2) = 0;
			}
			block.DestroyItemsDirect(mod.DestroyedUniqueItems);
			for (int i = 0; i < area.SettlementInfos.Length; i++)
			{
				SettlementInfo settlementInfo = area.SettlementInfos[i];
				bool flag2 = settlementInfo.BlockId == blockId && settlementInfo.SettlementId >= 0;
				if (flag2)
				{
					Settlement settlement = DomainManager.Organization.GetSettlement(settlementInfo.SettlementId);
					mod.SettlementDict.Add(settlementInfo.SettlementId, new ParallelSettlementModification(0, 0, settlement.GetPopulation() * random.Next(70, 91) / 100));
					break;
				}
			}
			bool flag3 = block.CharacterSet != null;
			if (flag3)
			{
				foreach (int charId in block.CharacterSet)
				{
					GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
					int randResult = random.Next(5);
					bool flag4 = character.GetAgeGroup() != 2 || DomainManager.Taiwu.GetGroupCharIds().Contains(charId) || character.GetFeatureIds().Contains(200);
					if (flag4)
					{
						randResult--;
					}
					switch (randResult)
					{
					case 1:
					{
						AddOrIncreaseInjuryParams param = new AddOrIncreaseInjuryParams(BodyPartType.GetRandomBodyPartType(random), false, (sbyte)random.Next(1, 3));
						mod.CharInjuries.Add(new ValueTuple<GameData.Domains.Character.Character, AddOrIncreaseInjuryParams>(character, param));
						break;
					}
					case 2:
						for (int j = 0; j < 2; j++)
						{
							AddOrIncreaseInjuryParams param2 = new AddOrIncreaseInjuryParams(BodyPartType.GetRandomBodyPartType(random), false, (sbyte)random.Next(3, 5));
							mod.CharInjuries.Add(new ValueTuple<GameData.Domains.Character.Character, AddOrIncreaseInjuryParams>(character, param2));
						}
						break;
					case 3:
						for (int k = 0; k < 2; k++)
						{
							AddOrIncreaseInjuryParams param3 = new AddOrIncreaseInjuryParams(BodyPartType.GetRandomBodyPartType(random), false, (sbyte)random.Next(5, 7));
							mod.CharInjuries.Add(new ValueTuple<GameData.Domains.Character.Character, AddOrIncreaseInjuryParams>(character, param3));
						}
						break;
					case 4:
						mod.DeadCharList.Add(charId);
						break;
					}
				}
			}
			bool flag5 = block.GraveSet != null;
			if (flag5)
			{
				mod.DamageGraveList.AddRange(block.GraveSet);
			}
		}

		// Token: 0x06007902 RID: 30978 RVA: 0x0046AA0C File Offset: 0x00468C0C
		public unsafe void ComplementUpdateMapArea(DataContext context, ParallelMapAreaModification mod)
		{
			MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Span<MapBlockData> blocks = this.GetAreaBlocks(mod.AreaId);
			int i = 0;
			int count = blocks.Length;
			while (i < count)
			{
				this.SetBlockData(context, *blocks[i]);
				i++;
			}
			foreach (KeyValuePair<short, ParallelSettlementModification> settlement in mod.SettlementDict)
			{
				CivilianSettlement civilianSettlement;
				bool flag = DomainManager.Organization.TryGetElement_CivilianSettlements(settlement.Key, out civilianSettlement);
				if (flag)
				{
					civilianSettlement.SetCulture(settlement.Value.Culture, context);
					civilianSettlement.SetSafety(settlement.Value.Safety, context);
					civilianSettlement.SetPopulation(settlement.Value.Population, context);
				}
				else
				{
					Sect sect;
					bool flag2 = DomainManager.Organization.TryGetElement_Sects(settlement.Key, out sect);
					if (flag2)
					{
						sect.SetCulture(settlement.Value.Culture, context);
						sect.SetSafety(settlement.Value.Safety, context);
						sect.SetPopulation(settlement.Value.Population, context);
					}
				}
			}
			bool flag3 = mod.DisasterBlocks.Count > 0;
			if (flag3)
			{
				Location disasterLocation = new Location(mod.AreaId, -1);
				monthlyNotifications.AddNaturalDisasterOccurred(disasterLocation, mod.DeadCharList.Count, mod.DisasterBlocks.Count);
				List<MapBlockData> neighborBlocks = new List<MapBlockData>();
				foreach (short blockId in mod.DisasterBlocks)
				{
					DomainManager.Map.GetRealNeighborBlocks(mod.AreaId, blockId, neighborBlocks, 2, false);
					int count2 = context.Random.Next(GlobalConfig.Instance.GenerateXiangshuMinionAfterDisasterRangeMax) + GlobalConfig.Instance.GenerateXiangshuMinionAfterDisasterBase;
					while (count2-- > 0)
					{
						bool flag4 = DomainManager.Map.GetBlock(mod.AreaId, blockId).BlockType != EMapBlockType.Developed || context.Random.CheckPercentProb(GlobalConfig.Instance.GenerateXiangshuMinionAfterDisasterInDevelopedBlockProbabilityPercentage);
						if (flag4)
						{
							DomainManager.Adventure.CreateTemporaryEnemiesOnValidBlocks(context, Location.Invalid, (short)Math.Clamp(298 + (int)DomainManager.World.GetXiangshuLevel() - context.Random.Next(GlobalConfig.Instance.GenerateXiangshuMinionAfterDisasterGradeMinusMax), 298, 306), 1, neighborBlocks, sbyte.MinValue);
						}
					}
				}
			}
			GameData.Domains.Character.Character uncommittedChar = null;
			int j = 0;
			int count3 = mod.CharInjuries.Count;
			while (j < count3)
			{
				ValueTuple<GameData.Domains.Character.Character, AddOrIncreaseInjuryParams> valueTuple = mod.CharInjuries[j];
				GameData.Domains.Character.Character character = valueTuple.Item1;
				AddOrIncreaseInjuryParams param = valueTuple.Item2;
				character.GetInjuries().Change(param.BodyPartType, param.IsInnerInjury, (int)param.InjuryValue);
				bool flag5 = character != uncommittedChar;
				if (flag5)
				{
					if (uncommittedChar != null)
					{
						uncommittedChar.SetInjuries(uncommittedChar.GetInjuries(), context);
					}
					uncommittedChar = character;
					Location location = character.GetLocation();
					bool flag6 = !location.IsValid();
					if (flag6)
					{
						location = character.GetLocation();
					}
					lifeRecordCollection.AddNaturalDisasterButSurvive(character.GetId(), currDate, location);
				}
				j++;
			}
			if (uncommittedChar != null)
			{
				uncommittedChar.SetInjuries(uncommittedChar.GetInjuries(), context);
			}
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			for (int k = 0; k < mod.DeadCharList.Count; k++)
			{
				int deadCharId = mod.DeadCharList[k];
				bool flag7 = deadCharId == taiwuCharId;
				if (flag7)
				{
					DomainManager.World.SetTaiwuDying(false);
				}
				GameData.Domains.Character.Character deadChar;
				bool flag8 = DomainManager.Character.TryGetElement_Objects(deadCharId, out deadChar);
				if (flag8)
				{
					DomainManager.Character.MakeCharacterDead(context, deadChar, 6);
				}
			}
			for (int l = 0; l < mod.DamageGraveList.Count; l++)
			{
				int charId = mod.DamageGraveList[l];
				Grave grave = DomainManager.Character.GetElement_Graves(charId);
				bool flag9 = grave.GetLevel() > 1;
				if (flag9)
				{
					grave.SetLevel(grave.GetLevel() - 1, context);
				}
				else
				{
					DomainManager.Character.RemoveGrave(context, grave);
				}
			}
			foreach (KeyValuePair<short, short> entry in mod.DisasterAdventureId)
			{
				MapDomain.Logger.Info<string, short, short>("Disaster generate material adventure {0} at ({1}, {2})", Adventure.Instance[entry.Value].Name, mod.AreaId, entry.Key);
				DomainManager.Adventure.TryCreateAdventureSite(context, mod.AreaId, entry.Key, entry.Value, MonthlyActionKey.Invalid);
				monthlyNotifications.AddDisasterAndPreciousMaterial(new Location(mod.AreaId, entry.Key));
			}
			DomainManager.Item.RemoveItems(context, mod.DestroyedUniqueItems);
		}

		// Token: 0x06007903 RID: 30979 RVA: 0x0046AF7C File Offset: 0x0046917C
		[Obsolete]
		public void CheckAnimalAttackTaiwuOnAdvanceMonth(IRandomSource randomSource)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			Location location = taiwu.GetLocation();
			bool flag = !location.IsValid();
			if (!flag)
			{
				bool flag2 = this._animalPlaceData.CheckIndex((int)location.AreaId);
				if (flag2)
				{
					AnimalAreaData animalAreaData = DomainManager.Extra.GetElement_AnimalAreaData((int)location.AreaId);
					List<short> blockData;
					bool flag3 = animalAreaData.BlockAnimalCharacterTemplateIdList != null && animalAreaData.BlockAnimalCharacterTemplateIdList.TryGetValue(location.BlockId, out blockData) && blockData.Count > 0;
					if (flag3)
					{
						List<short> templateIdList = ObjectPool<List<short>>.Instance.Get();
						templateIdList.AddRange(blockData);
						for (short i = (short)(templateIdList.Count - 1); i >= 0; i -= 1)
						{
							bool flag4 = !Config.Character.Instance[templateIdList[(int)i]].RandomAnimalAttack;
							if (flag4)
							{
								templateIdList.RemoveAt((int)i);
							}
						}
						bool flag5 = templateIdList.Count > 0;
						if (flag5)
						{
							MonthlyEventCollection collection = DomainManager.World.GetMonthlyEventCollection();
							collection.AddRandomAnimalAttack(location, templateIdList.Max((short a, short b) => Config.Character.Instance[a].ConsummateLevel.CompareTo(Config.Character.Instance[b].ConsummateLevel)));
						}
						else
						{
							ObjectPool<List<short>>.Instance.Return(templateIdList);
						}
					}
				}
			}
		}

		// Token: 0x06007904 RID: 30980 RVA: 0x0046B0D5 File Offset: 0x004692D5
		[Obsolete]
		public int CalcAnimalCountInArea(short areaId)
		{
			return this.CalcAnimalCountInArea(DomainManager.Extra.GetElement_AnimalAreaData((int)areaId));
		}

		// Token: 0x06007905 RID: 30981 RVA: 0x0046B0E8 File Offset: 0x004692E8
		[Obsolete]
		public int CalcAnimalCountInArea(AnimalAreaData data)
		{
			bool flag = data == null;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int count = 0;
				foreach (KeyValuePair<short, List<short>> pair in data.BlockAnimalCharacterTemplateIdList)
				{
					count += pair.Value.Count;
				}
				result = count;
			}
			return result;
		}

		// Token: 0x06007906 RID: 30982 RVA: 0x0046B15C File Offset: 0x0046935C
		[Obsolete]
		public unsafe void UpdateAnimalAreaData(DataContext context)
		{
			ObjectPool<List<short>> pool = ObjectPool<List<short>>.Instance;
			List<short> blockIds = pool.Get();
			short areaId = 0;
			while ((int)areaId < this._animalPlaceData.Length)
			{
				bool flag = this.IsAreaBroken(areaId);
				if (!flag)
				{
					AnimalAreaData data = DomainManager.Extra.GetElement_AnimalAreaData((int)areaId) ?? new AnimalAreaData();
					bool flag2 = this.CalcAnimalCountInArea(data) >= 12;
					if (flag2)
					{
						blockIds.Clear();
						foreach (KeyValuePair<short, List<short>> pair in data.BlockAnimalCharacterTemplateIdList)
						{
							blockIds.Add(pair.Key);
						}
						CollectionUtils.Shuffle<short>(context.Random, blockIds);
						foreach (short blockId in blockIds)
						{
							List<short> blockAnimalIds;
							bool flag3 = !data.BlockAnimalCharacterTemplateIdList.TryGetValue(blockId, out blockAnimalIds);
							if (!flag3)
							{
								bool flag4 = blockAnimalIds == null || blockAnimalIds.Count <= 0;
								if (flag4)
								{
									data.BlockAnimalCharacterTemplateIdList.Remove(blockId);
								}
								else
								{
									Location location = new Location(areaId, blockId);
									bool flag5 = this._fleeBeasts.Contains(location);
									if (!flag5)
									{
										bool containsNonAttackAnimal = false;
										using (List<short>.Enumerator enumerator3 = blockAnimalIds.GetEnumerator())
										{
											if (enumerator3.MoveNext())
											{
												short animalId = enumerator3.Current;
												bool flag6 = !Config.Character.Instance[areaId].RandomAnimalAttack;
												if (flag6)
												{
													containsNonAttackAnimal = true;
												}
											}
										}
										bool flag7 = containsNonAttackAnimal;
										if (!flag7)
										{
											this.RemoveBlockSingleAnimal(location, blockAnimalIds.GetRandom(context.Random), context);
											break;
										}
									}
								}
							}
						}
					}
					else
					{
						blockIds.Clear();
						Span<MapBlockData> areaBlocks = this.GetAreaBlocks(areaId);
						for (int i = 0; i < areaBlocks.Length; i++)
						{
							MapBlockData block = *areaBlocks[i];
							bool flag8 = block.GetConfig().Type == EMapBlockType.Wild;
							if (flag8)
							{
								blockIds.Add(block.BlockId);
							}
						}
						CollectionUtils.Shuffle<short>(context.Random, blockIds);
						short blockId2 = blockIds.GetRandom(context.Random);
						List<short> blockData;
						bool flag9 = !data.BlockAnimalCharacterTemplateIdList.TryGetValue(blockId2, out blockData);
						if (flag9)
						{
							data.BlockAnimalCharacterTemplateIdList.Add(blockId2, blockData = new List<short>());
						}
						blockData.Add(GameData.Domains.Map.SharedConstValue.AnimalCharIdGroups.GetRandom(context.Random)[0]);
						DomainManager.Extra.SetAnimalAreaData(context, areaId, data);
					}
				}
				areaId += 1;
			}
			pool.Return(blockIds);
		}

		// Token: 0x06007907 RID: 30983 RVA: 0x0046B444 File Offset: 0x00469644
		[Obsolete]
		public void AnimalsGetInRange(Location location, int range, Dictionary<Location, List<short>> result)
		{
			bool flag = !location.IsValid();
			if (!flag)
			{
				AnimalAreaData data = DomainManager.Extra.GetElement_AnimalAreaData((int)location.AreaId) ?? new AnimalAreaData();
				List<MapBlockData> rangeBlocks = ObjectPool<List<MapBlockData>>.Instance.Get();
				rangeBlocks.Clear();
				this.GetRealNeighborBlocks(location.AreaId, location.BlockId, rangeBlocks, range, true);
				foreach (MapBlockData block in rangeBlocks)
				{
					Location targetLocation = new Location(block.AreaId, block.BlockId);
					List<short> targetBlockData;
					bool flag2 = data.BlockAnimalCharacterTemplateIdList.TryGetValue(block.BlockId, out targetBlockData) && targetBlockData.Count > 0;
					if (flag2)
					{
						List<short> resultList;
						bool flag3 = !result.TryGetValue(targetLocation, out resultList) || resultList == null;
						if (flag3)
						{
							resultList = (result[targetLocation] = new List<short>());
						}
						resultList.AddRange(targetBlockData);
					}
				}
				ObjectPool<List<MapBlockData>>.Instance.Return(rangeBlocks);
			}
		}

		// Token: 0x06007908 RID: 30984 RVA: 0x0046B570 File Offset: 0x00469770
		[Obsolete]
		public void AnimalsWillAttackGetInRange(Location location, int range, Dictionary<Location, List<short>> result)
		{
			this.AnimalsGetInRange(location, range, result);
			foreach (Location key in result.Keys.ToArray<Location>())
			{
				List<short> animals = result[key];
				animals.RemoveAll(delegate(short id)
				{
					CharacterItem config = Config.Character.Instance.GetItem(id);
					bool flag2 = config != null;
					return flag2 && !config.RandomAnimalAttack;
				});
				bool flag = animals.Count <= 0;
				if (flag)
				{
					result.Remove(key);
				}
			}
		}

		// Token: 0x06007909 RID: 30985 RVA: 0x0046B5F8 File Offset: 0x004697F8
		[Obsolete]
		public bool AnimalMoveToLocation(DataContext context, Location animalLocation, short animalId, Location animalTarget)
		{
			bool flag = !animalLocation.IsValid() || !animalTarget.IsValid();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				AnimalAreaData data = DomainManager.Extra.GetElement_AnimalAreaData((int)animalLocation.AreaId);
				List<short> animalList;
				bool flag2 = data == null || !data.BlockAnimalCharacterTemplateIdList.TryGetValue(animalLocation.BlockId, out animalList);
				if (flag2)
				{
					result = false;
				}
				else
				{
					int index = animalList.LastIndexOf(animalId);
					bool flag3 = index < 0;
					if (flag3)
					{
						result = false;
					}
					else
					{
						AnimalAreaData targetData = DomainManager.Extra.GetElement_AnimalAreaData((int)animalTarget.AreaId) ?? new AnimalAreaData();
						List<short> targetAnimalList;
						bool flag4 = !targetData.BlockAnimalCharacterTemplateIdList.TryGetValue(animalTarget.BlockId, out targetAnimalList);
						if (flag4)
						{
							targetData.BlockAnimalCharacterTemplateIdList.Add(animalTarget.BlockId, targetAnimalList = new List<short>());
						}
						targetAnimalList.Add(animalList[index]);
						animalList.RemoveAt(index);
						DomainManager.Extra.SetAnimalAreaData(context, animalTarget.AreaId, data);
						this.MoveHunterAnimal(context, animalLocation, animalTarget, animalId);
						ItemKey carrierKey = DomainManager.Extra.GetFleeCarrierByLocation(animalId, animalLocation);
						bool flag5 = carrierKey.IsValid();
						if (flag5)
						{
							DomainManager.Extra.SetFleeCarrierLocation(context, carrierKey, animalTarget);
						}
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x0600790A RID: 30986 RVA: 0x0046B73C File Offset: 0x0046993C
		[Obsolete]
		public unsafe void AnimalCollectInBlockByHunterSkill(DataContext context, Location location, int range)
		{
			short areaId = location.AreaId;
			short blockId = location.BlockId;
			AnimalAreaData data = DomainManager.Extra.GetElement_AnimalAreaData((int)areaId) ?? new AnimalAreaData();
			List<short> blockData;
			bool flag = !data.BlockAnimalCharacterTemplateIdList.TryGetValue(blockId, out blockData);
			if (flag)
			{
				data.BlockAnimalCharacterTemplateIdList.Add(blockId, blockData = new List<short>());
			}
			byte areaSize = this.GetAreaSize(areaId);
			ByteCoordinate origin = ByteCoordinate.IndexToCoordinate(blockId, areaSize);
			short* offset4X = stackalloc short[(UIntPtr)8];
			short* offset4Y = stackalloc short[(UIntPtr)8];
			*offset4X = 1;
			offset4X[1] = -1;
			offset4X[2] = 0;
			offset4X[3] = 0;
			*offset4Y = 0;
			offset4Y[1] = 0;
			offset4Y[2] = 1;
			offset4Y[3] = -1;
			for (int i = 1; i <= range; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					int x = (int)((short)origin.X + offset4X[j]) * i;
					int y = (int)((short)origin.Y + offset4Y[j]) * i;
					bool flag2 = x < 0 || y < 0 || x >= (int)areaSize || y >= (int)areaSize;
					if (!flag2)
					{
						short targetBlockId = ByteCoordinate.CoordinateToIndex(new ByteCoordinate((byte)x, (byte)y), areaSize);
						List<short> targetBlockData;
						bool flag3 = !data.BlockAnimalCharacterTemplateIdList.TryGetValue(targetBlockId, out targetBlockData);
						if (flag3)
						{
							data.BlockAnimalCharacterTemplateIdList.Add(targetBlockId, targetBlockData = new List<short>());
						}
						blockData.AddRange(targetBlockData);
						targetBlockData.Clear();
					}
				}
			}
			DomainManager.Extra.SetAnimalAreaData(context, areaId, data);
		}

		// Token: 0x0600790B RID: 30987 RVA: 0x0046B8E0 File Offset: 0x00469AE0
		[Obsolete]
		public unsafe bool AnimalGenerateInAreaByHunterSkill(DataContext context, short areaId, short animalCharId)
		{
			bool flag = this.IsAreaBroken(areaId);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				AnimalAreaData data = DomainManager.Extra.GetElement_AnimalAreaData((int)areaId) ?? new AnimalAreaData();
				List<MapBlockData> blocks = new List<MapBlockData>();
				Span<MapBlockData> areaBlocks = this.GetAreaBlocks(areaId);
				for (int i = 0; i < areaBlocks.Length; i++)
				{
					MapBlockData block2 = *areaBlocks[i];
					blocks.Add(block2);
				}
				blocks.RemoveAll((MapBlockData block) => block.BlockId == DomainManager.Taiwu.GetTaiwu().GetLocation().BlockId);
				blocks.RemoveAll((MapBlockData block) => !block.Visible);
				blocks.RemoveAll((MapBlockData block) => block.IsCityTown());
				blocks.RemoveAll((MapBlockData block) => !block.IsPassable());
				bool flag2 = blocks.Count <= 0;
				if (flag2)
				{
					result = false;
				}
				else
				{
					short blockId = blocks.GetRandom(context.Random).BlockId;
					List<short> blockData;
					bool flag3 = !data.BlockAnimalCharacterTemplateIdList.TryGetValue(blockId, out blockData);
					if (flag3)
					{
						data.BlockAnimalCharacterTemplateIdList.Add(blockId, blockData = new List<short>());
					}
					blockData.Add(animalCharId);
					DomainManager.Extra.SetAnimalAreaData(context, areaId, data);
					this.AddHunterAnimal(context, areaId, blockId, animalCharId);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600790C RID: 30988 RVA: 0x0046BA68 File Offset: 0x00469C68
		[Obsolete]
		public unsafe void AnimalRandomGenerateInArea(DataContext context, short areaId)
		{
			IRandomSource random = context.Random;
			AnimalAreaData data = DomainManager.Extra.GetElement_AnimalAreaData((int)areaId) ?? new AnimalAreaData();
			List<short> blockIds = ObjectPool<List<short>>.Instance.Get();
			blockIds.Clear();
			Span<MapBlockData> areaBlocks = this.GetAreaBlocks(areaId);
			for (int i = 0; i < areaBlocks.Length; i++)
			{
				MapBlockData block = *areaBlocks[i];
				bool flag = block.GetConfig().Type != EMapBlockType.Wild;
				if (!flag)
				{
					float rate = 10f * (float)block.CurrResources.GetSum() / (float)block.MaxResources.GetSum();
					rate -= (float)data.BlockAnimalCharacterTemplateIdList.Sum((KeyValuePair<short, List<short>> b) => b.Value.Count);
					rate = Math.Clamp(rate, 0f, 100f);
					bool flag2 = random.NextFloat() < rate;
					if (flag2)
					{
						blockIds.Add(block.BlockId);
					}
				}
			}
			bool flag3 = blockIds.Count > 0;
			if (flag3)
			{
				short blockId = blockIds.GetRandom(context.Random);
				List<short> blockData;
				bool flag4 = !data.BlockAnimalCharacterTemplateIdList.TryGetValue(blockId, out blockData);
				if (flag4)
				{
					data.BlockAnimalCharacterTemplateIdList.Add(blockId, blockData = new List<short>());
				}
				blockData.Add(GameData.Domains.Map.SharedConstValue.AnimalCharIdGroups.GetRandom(random)[0]);
				DomainManager.Extra.SetAnimalAreaData(context, areaId, data);
			}
			ObjectPool<List<short>>.Instance.Return(blockIds);
			DomainManager.Extra.SetAnimalAreaData(context, areaId, data);
		}

		// Token: 0x0600790D RID: 30989 RVA: 0x0046BC08 File Offset: 0x00469E08
		[Obsolete]
		public unsafe void AnimalRandomLostInArea(DataContext context, short areaId)
		{
			IRandomSource random = context.Random;
			AnimalAreaData data = DomainManager.Extra.GetElement_AnimalAreaData((int)areaId) ?? new AnimalAreaData();
			Span<MapBlockData> areaBlocks = this.GetAreaBlocks(areaId);
			for (int i = 0; i < areaBlocks.Length; i++)
			{
				MapBlockData block = *areaBlocks[i];
				bool flag = !data.BlockAnimalCharacterTemplateIdList.ContainsKey(block.BlockId);
				if (!flag)
				{
					float rate = 10f * (float)block.CurrResources.GetSum() / (float)block.MaxResources.GetSum();
					rate -= (float)data.BlockAnimalCharacterTemplateIdList.Sum((KeyValuePair<short, List<short>> b) => b.Value.Count);
					rate = Math.Clamp(100f - rate, 0f, 100f);
					bool flag2 = random.NextFloat() < rate;
					if (flag2)
					{
						List<short> blockData;
						bool flag3 = data.BlockAnimalCharacterTemplateIdList.TryGetValue(block.BlockId, out blockData) && blockData.Count > 0;
						if (flag3)
						{
							blockData.RemoveAt(context.Random.Next(blockData.Count));
						}
					}
				}
			}
			DomainManager.Extra.SetAnimalAreaData(context, areaId, data);
		}

		// Token: 0x0600790E RID: 30990 RVA: 0x0046BD54 File Offset: 0x00469F54
		public bool LocationHasCricket(DataContext context, Location location)
		{
			CricketPlaceData data = this._cricketPlaceData[(int)location.AreaId];
			bool flag = data != null;
			if (flag)
			{
				int index = Array.IndexOf<short>(data.CricketBlocks, location.BlockId);
				bool flag2 = index >= 0;
				if (flag2)
				{
					return !data.CricketTriggered[index];
				}
			}
			CricketPlaceExtraData cricketPlaceExtraData;
			short num;
			return DomainManager.Extra.TryGetElement_CricketPlaceExtraData(location.AreaId, out cricketPlaceExtraData) && cricketPlaceExtraData != null && cricketPlaceExtraData.ExtraMapUnits != null && cricketPlaceExtraData.ExtraMapUnits.TryGetValue(location.BlockId, out num);
		}

		// Token: 0x0600790F RID: 30991 RVA: 0x0046BDF0 File Offset: 0x00469FF0
		public void UpdateCricketPlaceData(DataContext context)
		{
			sbyte currMonthInYear = DomainManager.World.GetCurrMonthInYear();
			bool flag = currMonthInYear == (sbyte)GlobalConfig.Instance.CricketActiveStartMonth;
			if (flag)
			{
				for (short areaId = 0; areaId < 45; areaId += 1)
				{
					this.InitializeCricketPlaceData(context, areaId);
				}
				this.InitializeCricketPlaceData(context, 137);
				this.InitializeCricketPlaceData(context, 138);
				MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
				monthlyNotifications.AddCricketsAppeared();
			}
			else
			{
				bool flag2 = currMonthInYear == (sbyte)GlobalConfig.Instance.CricketActiveEndMonth;
				if (flag2)
				{
					for (short areaId2 = 0; areaId2 < 45; areaId2 += 1)
					{
						this.SetElement_CricketPlaceData((int)areaId2, null, context);
					}
					this.SetElement_CricketPlaceData(137, null, context);
					this.SetElement_CricketPlaceData(138, null, context);
				}
			}
			DomainManager.Extra.UpdateExtraCricketMapUnit(context);
		}

		// Token: 0x06007910 RID: 30992 RVA: 0x0046BEC8 File Offset: 0x0046A0C8
		private void InitializeCricketPlaceData(DataContext context, short areaId)
		{
			CricketPlaceData cricketData = new CricketPlaceData();
			cricketData.Init(areaId, context.Random, 3, 5, -1, -1);
			this.SetElement_CricketPlaceData((int)areaId, cricketData, context);
		}

		// Token: 0x06007911 RID: 30993 RVA: 0x0046BEF8 File Offset: 0x0046A0F8
		[Obsolete]
		private void UpdateCricketPlaceDataPerArea(DataContext context, short areaId)
		{
			sbyte currMonthInYear = DomainManager.World.GetCurrMonthInYear();
			bool flag = currMonthInYear == (sbyte)GlobalConfig.Instance.CricketActiveStartMonth;
			if (flag)
			{
				CricketPlaceData cricketData = new CricketPlaceData();
				cricketData.Init(areaId, context.Random, 3, 5, -1, -1);
				this.SetElement_CricketPlaceData((int)areaId, cricketData, context);
			}
			else
			{
				bool flag2 = currMonthInYear == (sbyte)GlobalConfig.Instance.CricketActiveEndMonth;
				if (flag2)
				{
					this.SetElement_CricketPlaceData((int)areaId, null, context);
				}
			}
		}

		// Token: 0x06007912 RID: 30994 RVA: 0x0046BF65 File Offset: 0x0046A165
		public void SetCricketPlaceData(DataContext context, short areaId, CricketPlaceData cricketData)
		{
			this.SetElement_CricketPlaceData((int)areaId, cricketData, context);
		}

		// Token: 0x06007913 RID: 30995 RVA: 0x0046BF74 File Offset: 0x0046A174
		[DomainMethod]
		public bool TryTriggerCricketCatch(DataContext context)
		{
			MapDomain.<>c__DisplayClass149_0 CS$<>8__locals1;
			CS$<>8__locals1.context = context;
			CS$<>8__locals1.taiwu = DomainManager.Taiwu.GetTaiwu();
			Location location = CS$<>8__locals1.taiwu.GetLocation();
			CricketPlaceExtraData cricketPlaceExtraData;
			short num;
			bool flag = DomainManager.Extra.TryGetElement_CricketPlaceExtraData(location.AreaId, out cricketPlaceExtraData) && cricketPlaceExtraData != null && cricketPlaceExtraData.ExtraMapUnits != null && cricketPlaceExtraData.ExtraMapUnits.TryGetValue(location.BlockId, out num);
			bool result;
			if (flag)
			{
				bool flag2 = !MapDomain.<TryTriggerCricketCatch>g__TryCostSweepNet|149_0(ref CS$<>8__locals1);
				if (flag2)
				{
					result = false;
				}
				else
				{
					DomainManager.Extra.RemoveExtraCricketMapUnit(CS$<>8__locals1.context, location);
					MapDomain.<TryTriggerCricketCatch>g__CricketLuckPointPostProcess|149_1(ref CS$<>8__locals1);
					result = true;
				}
			}
			else
			{
				CricketPlaceData cricketPlaceData = this.GetElement_CricketPlaceData((int)location.AreaId);
				bool flag3 = cricketPlaceData == null;
				if (flag3)
				{
					result = false;
				}
				else
				{
					int index = Array.IndexOf<short>(cricketPlaceData.CricketBlocks, location.BlockId);
					bool flag4 = index < 0 || cricketPlaceData.CricketTriggered[index];
					if (flag4)
					{
						result = false;
					}
					else
					{
						bool flag5 = !MapDomain.<TryTriggerCricketCatch>g__TryCostSweepNet|149_0(ref CS$<>8__locals1);
						if (flag5)
						{
							result = false;
						}
						else
						{
							int grpIdx = index / 3;
							bool isTrueCricket = (int)cricketPlaceData.RealCircketIdx[grpIdx] == index % 3;
							bool flag6 = isTrueCricket;
							if (flag6)
							{
								for (int i = 0; i < 3; i++)
								{
									cricketPlaceData.CricketTriggered[3 * grpIdx + i] = true;
								}
								this.SetElement_CricketPlaceData((int)location.AreaId, cricketPlaceData, CS$<>8__locals1.context);
								result = true;
							}
							else
							{
								MapDomain.<TryTriggerCricketCatch>g__CricketLuckPointPostProcess|149_1(ref CS$<>8__locals1);
								cricketPlaceData.CricketTriggered[index] = true;
								cricketPlaceData.ChangePlace(location.AreaId, index);
								this.SetElement_CricketPlaceData((int)location.AreaId, cricketPlaceData, CS$<>8__locals1.context);
								result = false;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06007914 RID: 30996 RVA: 0x0046C120 File Offset: 0x0046A320
		public bool IsCricketInLocation(Location location)
		{
			bool flag = !location.IsValid();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				CricketPlaceData cricketPlaceData = this._cricketPlaceData[(int)location.AreaId];
				int i = 0;
				for (;;)
				{
					int num = i;
					int? num2;
					if (cricketPlaceData == null)
					{
						num2 = null;
					}
					else
					{
						short[] cricketBlocks = cricketPlaceData.CricketBlocks;
						num2 = ((cricketBlocks != null) ? new int?(cricketBlocks.Length) : null);
					}
					int? num3 = num2;
					if (num >= num3.GetValueOrDefault())
					{
						goto Block_6;
					}
					bool flag2 = cricketPlaceData.CricketBlocks[i] == location.BlockId && !cricketPlaceData.CricketTriggered[i];
					if (flag2)
					{
						break;
					}
					i++;
				}
				return true;
				Block_6:
				CricketPlaceExtraData extraData;
				bool flag3;
				if (DomainManager.Extra.TryGetElement_CricketPlaceExtraData(location.AreaId, out extraData))
				{
					Dictionary<short, short> extraMapUnits = extraData.ExtraMapUnits;
					flag3 = (extraMapUnits != null && extraMapUnits.Count > 0);
				}
				else
				{
					flag3 = false;
				}
				bool flag4 = flag3;
				result = (flag4 && extraData.ExtraMapUnits.ContainsKey(location.BlockId));
			}
			return result;
		}

		// Token: 0x06007915 RID: 30997 RVA: 0x0046C218 File Offset: 0x0046A418
		[Obsolete("Use DomainManager.Extra.ChangeAreaSpiritualDebt instead")]
		public void ChangeAreaSpiritualDebt(DataContext context, short areaId, int delta)
		{
			MapAreaData area = this._areas[(int)areaId];
			int[] spiritualDebtLimit = GlobalConfig.Instance.SpiritualDebtLimit;
			area.SpiritualDebt = (short)Math.Clamp((int)area.SpiritualDebt + delta, spiritualDebtLimit[0], spiritualDebtLimit[1]);
			bool flag = delta > 0;
			if (flag)
			{
				DomainManager.World.GetInstantNotifications().AddGraceIncreased(new Location(areaId, -1));
			}
			this.SetElement_Areas((int)areaId, area, context);
		}

		// Token: 0x06007916 RID: 30998 RVA: 0x0046C280 File Offset: 0x0046A480
		[Obsolete("Use DomainManager.Extra.SetAreaSpiritualDebt instead")]
		public void SetAreaSpiritualDebt(DataContext context, short areaId, short value)
		{
			MapAreaData area = this._areas[(int)areaId];
			short originalValue = area.SpiritualDebt;
			int[] spiritualDebtLimit = GlobalConfig.Instance.SpiritualDebtLimit;
			area.SpiritualDebt = (short)Math.Clamp((int)value, spiritualDebtLimit[0], spiritualDebtLimit[1]);
			bool flag = area.SpiritualDebt > originalValue;
			if (flag)
			{
				DomainManager.World.GetInstantNotifications().AddGraceIncreased(new Location(areaId, -1));
			}
			this.SetElement_Areas((int)areaId, area, context);
		}

		// Token: 0x06007917 RID: 30999 RVA: 0x0046C2EA File Offset: 0x0046A4EA
		[Obsolete("Use SetAreaSpritualDebt instead")]
		public void ChangeSpiritualDebt(DataContext context, short areaId, short spiritualDebt)
		{
			DomainManager.Extra.SetAreaSpiritualDebt(context, areaId, (int)spiritualDebt, true, true);
		}

		// Token: 0x06007918 RID: 31000 RVA: 0x0046C2FC File Offset: 0x0046A4FC
		[Obsolete("Use ChangeAreaSpiritualDebt instead")]
		public void SetSpiritualDebtByChange(DataContext context, short areaId, short changeValue)
		{
			DomainManager.Extra.ChangeAreaSpiritualDebt(context, areaId, (int)changeValue, true, true);
		}

		// Token: 0x06007919 RID: 31001 RVA: 0x0046C310 File Offset: 0x0046A510
		public void GetNearAreaList(short areaId, List<short> areaList)
		{
			bool flag = areaId >= 45;
			if (!flag)
			{
				areaList.Clear();
				areaList.Add(areaId);
				areaList.AddRange(this._regularAreaNearList[areaId].Items);
			}
		}

		// Token: 0x0600791A RID: 31002 RVA: 0x0046C354 File Offset: 0x0046A554
		public void ChangeSettlementSafetyInArea(DataContext context, short areaId, int delta)
		{
			MapAreaData areaData = this._areas[(int)areaId];
			foreach (SettlementInfo settlementInfo in areaData.SettlementInfos)
			{
				bool flag = settlementInfo.SettlementId < 0;
				if (!flag)
				{
					Settlement settlement = DomainManager.Organization.GetSettlement(settlementInfo.SettlementId);
					settlement.ChangeSafety(context, delta);
				}
			}
		}

		// Token: 0x0600791B RID: 31003 RVA: 0x0046C3B8 File Offset: 0x0046A5B8
		public void ChangeSettlementCultureInArea(DataContext context, short areaId, int delta)
		{
			MapAreaData areaData = this._areas[(int)areaId];
			foreach (SettlementInfo settlementInfo in areaData.SettlementInfos)
			{
				bool flag = settlementInfo.SettlementId < 0;
				if (!flag)
				{
					Settlement settlement = DomainManager.Organization.GetSettlement(settlementInfo.SettlementId);
					settlement.ChangeCulture(context, delta);
				}
			}
		}

		// Token: 0x0600791C RID: 31004 RVA: 0x0046C41C File Offset: 0x0046A61C
		[DomainMethod]
		public MapAreaData GetAreaByAreaId(short areaId)
		{
			return this.GetElement_Areas((int)areaId);
		}

		// Token: 0x0600791D RID: 31005 RVA: 0x0046C438 File Offset: 0x0046A638
		public short GetSpiritualDebtLowestAreaIdByAreaId(short currAreaId)
		{
			sbyte stateId = this.GetStateIdByAreaId(currAreaId);
			List<short> areas = ObjectPool<List<short>>.Instance.Get();
			this.GetAllAreaInState(stateId, areas);
			int lowestSpiritualDebtValue = int.MaxValue;
			short lowestSpiritualDebtAreaId = currAreaId;
			foreach (short areaId in areas)
			{
				bool flag = this.IsAreaBroken(areaId);
				if (!flag)
				{
					int value = DomainManager.Extra.GetAreaSpiritualDebt(areaId);
					bool flag2 = value < lowestSpiritualDebtValue;
					if (flag2)
					{
						lowestSpiritualDebtValue = value;
						lowestSpiritualDebtAreaId = areaId;
					}
				}
			}
			ObjectPool<List<short>>.Instance.Return(areas);
			return lowestSpiritualDebtAreaId;
		}

		// Token: 0x0600791E RID: 31006 RVA: 0x0046C4F0 File Offset: 0x0046A6F0
		[DomainMethod]
		public MapBlockData GetBlockData(short areaId, short blockId)
		{
			this.LastGetBlockDataPosition_Debug = new Location(areaId, blockId);
			return this.GetBlock(areaId, blockId);
		}

		// Token: 0x0600791F RID: 31007 RVA: 0x0046C518 File Offset: 0x0046A718
		[DomainMethod]
		public FullBlockName GetBlockFullName(Location location)
		{
			bool flag = location.AreaId < 0;
			FullBlockName result;
			if (flag)
			{
				result = new FullBlockName
				{
					areaTemplateId = -1,
					stateTemplateId = -1,
					BelongBlockData = null,
					BlockData = null
				};
			}
			else
			{
				FullBlockName fullBlockName = new FullBlockName
				{
					areaTemplateId = this.GetElement_Areas((int)location.AreaId).GetTemplateId(),
					stateTemplateId = this.GetStateTemplateIdByAreaId(location.AreaId)
				};
				MapBlockData blockData = this.GetBlock(location);
				fullBlockName.BlockData = MapBlockData.SimpleClone(blockData);
				bool flag2 = blockData.BelongBlockId >= 0;
				if (flag2)
				{
					MapBlockData belongBlockData = this.GetBlock(blockData.AreaId, blockData.BelongBlockId);
					fullBlockName.BelongBlockData = MapBlockData.SimpleClone(belongBlockData);
				}
				result = fullBlockName;
			}
			return result;
		}

		// Token: 0x06007920 RID: 31008 RVA: 0x0046C5EC File Offset: 0x0046A7EC
		[DomainMethod]
		public List<CollectResourceResult> CollectAllResourcesFree(DataContext context)
		{
			List<CollectResourceResult> result = new List<CollectResourceResult>();
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			MapDomain mapDomain = DomainManager.Map;
			foreach (Location blockLocation in ProfessionSkillHandle.GetSavageSkill_1_EffectRange(taiwu.GetLocation()))
			{
				MapBlockData blockData = mapDomain.GetBlock(blockLocation);
				bool flag = 126 == blockData.TemplateId;
				if (!flag)
				{
					for (sbyte resourceType = 0; resourceType < 6; resourceType += 1)
					{
						short currentResource;
						short maxResource;
						CollectResourceResult step = this.CalcCollectResourceResult(context.Random, blockData, resourceType, out currentResource, out maxResource);
						this.ApplyCollectResourceResult(context, taiwu, blockData, currentResource, maxResource, false, ref step);
						result.Add(step);
					}
				}
			}
			return result;
		}

		// Token: 0x06007921 RID: 31009 RVA: 0x0046C6C4 File Offset: 0x0046A8C4
		internal unsafe CollectResourceResult CalcCollectResourceResult(IRandomSource random, MapBlockData blockData, sbyte resourceType, out short currentResource, out short maxResource)
		{
			CollectResourceResult result = default(CollectResourceResult);
			currentResource = *(ref blockData.CurrResources.Items.FixedElementField + (IntPtr)resourceType * 2);
			maxResource = Math.Max(*(ref blockData.MaxResources.Items.FixedElementField + (IntPtr)resourceType * 2), 1);
			result.ResourceType = resourceType;
			result.ResourceCount = (short)this.GetCollectResourceAmount(random, blockData, resourceType);
			result.ResourceCount = result.ResourceCount * GameData.Domains.World.SharedMethods.GetGainResourcePercent(11) / 100;
			return result;
		}

		// Token: 0x06007922 RID: 31010 RVA: 0x0046C74C File Offset: 0x0046A94C
		internal unsafe void ApplyCollectResourceResult(DataContext ctx, GameData.Domains.Character.Character character, MapBlockData blockData, short currentResource, short maxResource, bool costResource, ref CollectResourceResult result)
		{
			IRandomSource random = ctx.Random;
			int charId = character.GetId();
			sbyte resourceType = result.ResourceType;
			character.ChangeResource(ctx, resourceType, (int)result.ResourceCount);
			bool forceGetItemInTutorial = DomainManager.TutorialChapter.IsInTutorialChapter(3) && resourceType == 1 && EventHelper.GetBoolFromGlobalArgBox("WaitCollectWoodOuter3") && EventArgBox.TaiwuAreaId == 136;
			short itemTemplateId = blockData.GetCollectItemTemplateId(random, resourceType);
			CValuePercentBonus buildingBonus = DomainManager.Building.GetBuildingBlockEffect(blockData.GetLocation(), EBuildingScaleEffect.CollectResourceGetItemBonus, -1);
			int collectItemChance = blockData.GetCollectItemChance(resourceType) * buildingBonus;
			bool flag = forceGetItemInTutorial;
			if (flag)
			{
				ItemKey itemKey = DomainManager.Item.CreateItem(ctx, 5, 2);
				EventHelper.RemoveFromGlobalArgBox<bool>("WaitCollectWoodOuter3");
				character.AddInventoryItem(ctx, itemKey, 1, false);
				EventHelper.SaveGlobalArg<ItemKey>("TutorialWoodOuter3Got", itemKey);
				result.ItemDisplayData = DomainManager.Item.GetItemDisplayData(itemKey, charId);
				result.ItemDisplayData.Amount = 1;
			}
			else
			{
				bool flag2 = itemTemplateId >= 0 && random.CheckPercentProb(collectItemChance);
				if (flag2)
				{
					int neighborOddsMultiplier = 1;
					bool flag3 = charId == DomainManager.Taiwu.GetTaiwuCharId();
					if (flag3)
					{
						MapBlockItem blockConfig = MapBlock.Instance[blockData.TemplateId];
						List<MapBlockData> neighborList = ObjectPool<List<MapBlockData>>.Instance.Get();
						this.GetNeighborBlocks(blockData.AreaId, blockData.BlockId, neighborList, 1);
						for (int i = 0; i < neighborList.Count; i++)
						{
							bool flag4 = blockConfig.ResourceCollectionType == MapBlock.Instance[neighborList[i].TemplateId].ResourceCollectionType;
							if (flag4)
							{
								neighborOddsMultiplier += 2;
							}
						}
						ObjectPool<List<MapBlockData>>.Instance.Return(neighborList);
					}
					this.UpgradeCollectMaterial(random, blockData.GetResourceCollectionConfig(), resourceType, maxResource, currentResource, neighborOddsMultiplier, ref itemTemplateId);
					ItemKey itemKey2 = DomainManager.Item.CreateItem(ctx, 5, itemTemplateId);
					character.AddInventoryItem(ctx, itemKey2, 1, false);
					result.ItemDisplayData = DomainManager.Item.GetItemDisplayData(itemKey2, charId);
					result.ItemDisplayData.Amount = 1;
				}
				else
				{
					result.ItemDisplayData = null;
				}
			}
			bool flag5 = !DomainManager.TutorialChapter.GetInGuiding() && charId == DomainManager.Taiwu.GetTaiwuCharId();
			if (flag5)
			{
				ProfessionFormulaItem formula = ProfessionFormula.Instance[0];
				int addSeniority = formula.Calculate((int)currentResource);
				int addSeniority2 = 0;
				bool flag6 = result.ItemDisplayData != null;
				if (flag6)
				{
					ProfessionFormulaItem formula2 = ProfessionFormula.Instance[1];
					addSeniority2 = formula2.Calculate(result.ItemDisplayData.Value);
				}
				DomainManager.Extra.ChangeProfessionSeniority(ctx, 0, addSeniority + addSeniority2, true, false);
			}
			if (costResource)
			{
				ResourceTypeItem resourceConfig = Config.ResourceType.Instance[resourceType];
				*(ref blockData.CurrResources.Items.FixedElementField + (IntPtr)resourceType * 2) = (short)Math.Max((int)(currentResource - (short)resourceConfig.ResourceReducePerCollection), 0);
				this.SetBlockData(ctx, blockData);
				this.AddBlockMalice(ctx, blockData.AreaId, blockData.BlockId, 10);
			}
			VillagerWorkData workData = DomainManager.Taiwu.GetVillagerMapWorkData(blockData.AreaId, blockData.BlockId, 10);
			bool flag7 = workData != null;
			if (flag7)
			{
				DomainManager.Taiwu.SetVillagerWork(ctx, workData.CharacterId, workData, false);
			}
		}

		// Token: 0x06007923 RID: 31011 RVA: 0x0046CA88 File Offset: 0x0046AC88
		public void UpgradeCollectMaterial(IRandomSource random, ResourceCollectionItem collectionConfig, sbyte resourceType, short maxResource, short currentResource, int neighborOddsMultiplier, ref short itemTemplateId)
		{
			sbyte maxAddGrade = collectionConfig.MaxAddGrade[(int)resourceType];
			int gradeUpOdds = (int)collectionConfig.GradeUpOdds[(int)resourceType] + Math.Max((int)(maxResource - 100), 0) / 10 * neighborOddsMultiplier;
			bool flag = random.CheckPercentProb(gradeUpOdds);
			if (flag)
			{
				int odds = gradeUpOdds * (int)currentResource / (int)maxResource;
				bool flag2 = odds >= 100;
				if (flag2)
				{
					itemTemplateId += (short)maxAddGrade;
				}
				else
				{
					for (int i = 0; i < (int)maxAddGrade; i++)
					{
						bool flag3 = random.CheckPercentProb(odds);
						if (flag3)
						{
							itemTemplateId += 1;
						}
					}
				}
			}
		}

		// Token: 0x06007924 RID: 31012 RVA: 0x0046CB18 File Offset: 0x0046AD18
		[DomainMethod]
		public CollectResourceResult CollectResource(DataContext context, int charId, sbyte resourceType, bool costTime = true, bool costResource = true)
		{
			bool flag = costTime && DomainManager.World.GetLeftDaysInCurrMonth() == 0;
			if (flag)
			{
				throw new Exception("No enough time for resource collection");
			}
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			MapBlockData blockData = this.GetBlock(character.GetLocation());
			short currentResource;
			short maxResource;
			CollectResourceResult result = this.CalcCollectResourceResult(context.Random, blockData, resourceType, out currentResource, out maxResource);
			this.ApplyCollectResourceResult(context, character, blockData, currentResource, maxResource, costResource, ref result);
			return result;
		}

		// Token: 0x06007925 RID: 31013 RVA: 0x0046CB90 File Offset: 0x0046AD90
		[DomainMethod]
		public List<MapBlockData> GetMapBlockDataList(List<Location> locationList)
		{
			return this.GetMapBlockDataListOptional(locationList, false, false);
		}

		// Token: 0x06007926 RID: 31014 RVA: 0x0046CBAC File Offset: 0x0046ADAC
		[DomainMethod]
		public List<MapBlockData> GetMapBlockDataListOptional(List<Location> locationList, bool includeRoot = false, bool includeBelong = false)
		{
			List<MapBlockData> dataList = new List<MapBlockData>();
			bool flag = locationList != null;
			if (flag)
			{
				for (int i = 0; i < locationList.Count; i++)
				{
					MapBlockData block = this.GetBlock(locationList[i]);
					dataList.Add(block);
					bool flag2 = includeRoot && block.RootBlockId > -1;
					if (flag2)
					{
						MapBlockData rootBlock = this.GetBlock(block.AreaId, block.RootBlockId);
						dataList.Add(rootBlock);
					}
					bool flag3 = includeBelong && block.BelongBlockId > -1;
					if (flag3)
					{
						MapBlockData belongBlock = this.GetBlock(block.AreaId, block.BelongBlockId);
						dataList.Add(belongBlock);
					}
				}
			}
			return dataList;
		}

		// Token: 0x06007927 RID: 31015 RVA: 0x0046CC74 File Offset: 0x0046AE74
		[DomainMethod]
		public bool IsContainsPurpleBamboo(short areaId)
		{
			using (IEnumerator<short> enumerator = this.IterAreaPurpleBambooTemplateIds(areaId).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					short _ = enumerator.Current;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007928 RID: 31016 RVA: 0x0046CCC8 File Offset: 0x0046AEC8
		public IEnumerable<short> IterAreaPurpleBambooTemplateIds(short areaId)
		{
			byte size = this.GetAreaSize(areaId);
			short num;
			for (short i = 0; i < (short)(size * size); i = num + 1)
			{
				MapBlockData blockData = this.GetBlock(areaId, i);
				bool flag = blockData.FixedCharacterSet == null;
				if (!flag)
				{
					foreach (int charId in blockData.FixedCharacterSet)
					{
						GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
						bool flag2 = character.GetXiangshuType() == 3;
						if (flag2)
						{
							yield return character.GetTemplateId();
						}
						character = null;
					}
					HashSet<int>.Enumerator enumerator = default(HashSet<int>.Enumerator);
					blockData = null;
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x06007929 RID: 31017 RVA: 0x0046CCE0 File Offset: 0x0046AEE0
		[DomainMethod]
		public List<short> GetBelongBlockTemplateIdList(List<Location> locationList)
		{
			List<short> templateIdList = new List<short>();
			bool flag = locationList != null;
			if (flag)
			{
				for (int i = 0; i < locationList.Count; i++)
				{
					Location location = locationList[i];
					short belongBlockId = this.GetBlock(location).BelongBlockId;
					templateIdList.Add((belongBlockId >= 0) ? this.GetBlock(location.AreaId, belongBlockId).TemplateId : -1);
				}
			}
			return templateIdList;
		}

		// Token: 0x0600792A RID: 31018 RVA: 0x0046CD58 File Offset: 0x0046AF58
		[DomainMethod]
		public LocationNameRelatedData GetLocationNameRelatedData(Location location)
		{
			bool flag = location.AreaId < 0;
			LocationNameRelatedData result;
			if (flag)
			{
				result = new LocationNameRelatedData(-1);
			}
			else
			{
				MapAreaData area = this._areas[(int)location.AreaId];
				LocationNameRelatedData data = new LocationNameRelatedData(area.GetTemplateId());
				bool flag2 = location.BlockId < 0;
				if (flag2)
				{
					result = data;
				}
				else
				{
					MapBlockData block = this.GetBlock(location).GetRootBlock();
					bool flag3 = block.IsCityTown();
					if (flag3)
					{
						data.SettlementMapBlockTemplateId = block.TemplateId;
						int settlementIdx = area.GetSettlementIndex(block.BlockId);
						SettlementInfo settlementInfo = area.SettlementInfos[settlementIdx];
						data.SettlementRandomNameId = settlementInfo.RandomNameId;
					}
					else
					{
						ValueTuple<short, sbyte> referenceSettlementAndDirection = area.GetReferenceSettlementAndDirection(location.BlockId);
						short settlementIdx2 = referenceSettlementAndDirection.Item1;
						sbyte direction = referenceSettlementAndDirection.Item2;
						bool flag4 = settlementIdx2 >= 0;
						if (flag4)
						{
							SettlementInfo settlementInfo2 = area.SettlementInfos[(int)settlementIdx2];
							MapBlockData settlementBlock = this.GetBlock(location.AreaId, settlementInfo2.BlockId);
							data.SettlementMapBlockTemplateId = settlementBlock.TemplateId;
							data.SettlementRandomNameId = settlementInfo2.RandomNameId;
						}
						data.Direction = direction;
					}
					result = data;
				}
			}
			return result;
		}

		// Token: 0x0600792B RID: 31019 RVA: 0x0046CE84 File Offset: 0x0046B084
		[DomainMethod]
		public List<LocationNameRelatedData> GetLocationNameRelatedDataList(List<Location> locations)
		{
			int locationsCount = locations.Count;
			List<LocationNameRelatedData> dataList = new List<LocationNameRelatedData>(locationsCount);
			for (int i = 0; i < locationsCount; i++)
			{
				dataList.Add(this.GetLocationNameRelatedData(locations[i]));
			}
			return dataList;
		}

		// Token: 0x0600792C RID: 31020 RVA: 0x0046CECC File Offset: 0x0046B0CC
		[DomainMethod]
		public void ChangeBlockTemplate(DataContext context, Location location, short blockTemplateId, bool isTurnVisible)
		{
			MapBlockData blockData = this.GetBlock(location);
			this.ChangeBlockTemplate(context, blockData, blockTemplateId);
			if (isTurnVisible)
			{
				this.SetBlockAndViewRangeVisible(context, location.AreaId, location.BlockId);
			}
		}

		// Token: 0x0600792D RID: 31021 RVA: 0x0046CF08 File Offset: 0x0046B108
		public bool TryGetBlock(Location location, out MapBlockData blockData)
		{
			blockData = null;
			short areaId2 = location.AreaId;
			bool flag = areaId2 < 0 || areaId2 >= 139;
			bool flag2 = flag;
			bool result;
			if (flag2)
			{
				result = false;
			}
			else
			{
				short areaId = location.AreaId;
				if (!true)
				{
				}
				AreaBlockCollection areaBlockCollection;
				if (areaId >= 45)
				{
					if (areaId >= 135)
					{
						switch (areaId)
						{
						case 135:
							areaBlockCollection = this._bornAreaBlocks;
							break;
						case 136:
							areaBlockCollection = this._guideAreaBlocks;
							break;
						case 137:
							areaBlockCollection = this._secretVillageAreaBlocks;
							break;
						case 138:
							areaBlockCollection = this._brokenPerformAreaBlocks;
							break;
						default:
							areaBlockCollection = null;
							break;
						}
					}
					else
					{
						areaBlockCollection = this._brokenAreaBlocks;
					}
				}
				else
				{
					areaBlockCollection = this.GetRegularAreaBlocks(areaId);
				}
				if (!true)
				{
				}
				AreaBlockCollection areaBlocks = areaBlockCollection;
				short blockId = location.BlockId;
				bool flag3 = this.IsAreaBroken(areaId);
				if (flag3)
				{
					blockId += 25 * (areaId - 45);
				}
				result = (areaBlocks != null && areaBlocks.TryGetValue(blockId, out blockData));
			}
			return result;
		}

		// Token: 0x0600792E RID: 31022 RVA: 0x0046CFFC File Offset: 0x0046B1FC
		public MapBlockData GetBlock(short areaId, short blockId)
		{
			MapBlockData data;
			bool flag = this.TryGetBlock(new Location(areaId, blockId), out data);
			if (flag)
			{
				return data;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Failed to get block at ");
			defaultInterpolatedStringHandler.AppendFormatted<short>(areaId);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<short>(blockId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600792F RID: 31023 RVA: 0x0046D068 File Offset: 0x0046B268
		public MapBlockData GetBlock(Location key)
		{
			return this.GetBlock(key.AreaId, key.BlockId);
		}

		// Token: 0x06007930 RID: 31024 RVA: 0x0046D08C File Offset: 0x0046B28C
		public bool SplitMultiBlock(DataContext context, MapBlockData blockData)
		{
			MapBlockItem config = blockData.GetConfig();
			bool flag = config.SplitOrMergeBlockId < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = config.Size < 2;
				result = (!flag2 && this.ChangeBlockTemplate(context, blockData, config.SplitOrMergeBlockId));
			}
			return result;
		}

		// Token: 0x06007931 RID: 31025 RVA: 0x0046D0D8 File Offset: 0x0046B2D8
		public bool MergeMultiBlock(DataContext context, MapBlockData blockData)
		{
			MapBlockItem config = blockData.GetConfig();
			bool flag = config.SplitOrMergeBlockId < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = config.Size > 1;
				result = (!flag2 && this.ChangeBlockTemplateByMerge(context, blockData, config.SplitOrMergeBlockId));
			}
			return result;
		}

		// Token: 0x06007932 RID: 31026 RVA: 0x0046D124 File Offset: 0x0046B324
		public bool ChangeBlockTemplate(DataContext context, MapBlockData blockData, short newTemplateId)
		{
			bool flag = MapBlock.Instance[newTemplateId].Size > 1;
			bool result;
			if (flag)
			{
				result = this.ChangeBlockTemplateByMerge(context, blockData, newTemplateId);
			}
			else
			{
				blockData = blockData.GetRootBlock();
				List<MapBlockData> groupBlocks = ObjectPool<List<MapBlockData>>.Instance.Get();
				groupBlocks.Clear();
				bool flag2 = blockData.GroupBlockList == null;
				if (flag2)
				{
					groupBlocks.Add(blockData);
				}
				else
				{
					groupBlocks.Add(blockData);
					groupBlocks.AddRange(blockData.GroupBlockList);
					blockData.GroupBlockList = null;
				}
				foreach (MapBlockData block in groupBlocks)
				{
					block.RootBlockId = -1;
					block.ChangeTemplateId(newTemplateId, true);
					block.InitResources(context.Random);
					block.Destroyed = false;
					this.SetBlockData(context, block);
				}
				ObjectPool<List<MapBlockData>>.Instance.Return(groupBlocks);
				result = true;
			}
			return result;
		}

		// Token: 0x06007933 RID: 31027 RVA: 0x0046D22C File Offset: 0x0046B42C
		public unsafe bool ChangeBlockTemplateByMerge(DataContext context, MapBlockData blockData, short newTemplateId)
		{
			byte areaSize = this.GetAreaSize(blockData.AreaId);
			Span<MapBlockData> areaBlocks = this.GetAreaBlocks(blockData.AreaId);
			ByteCoordinate blockPos = ByteCoordinate.IndexToCoordinate(blockData.BlockId, areaSize);
			MapBlockItem newConfig = MapBlock.Instance[newTemplateId];
			for (byte x = 0; x < newConfig.Size; x += 1)
			{
				for (byte y = 0; y < newConfig.Size; y += 1)
				{
					ByteCoordinate childPos = blockPos + new ByteCoordinate(x, y);
					short childId = ByteCoordinate.CoordinateToIndex(childPos, areaSize);
					MapBlockData block = *areaBlocks[(int)childId];
					Tester.Assert(block.IsPassable(), "childBlock.IsPassable()");
					bool flag = block == blockData;
					if (flag)
					{
						block.ChangeTemplateId(newTemplateId, true);
					}
					else
					{
						block.ChangeTemplateId(-1, true);
						block.SetToSizeBlock(blockData);
					}
					block.InitResources(context.Random);
					block.Destroyed = false;
					this.SetBlockData(context, block);
				}
			}
			blockData.SetVisible(blockData.Visible, context);
			return true;
		}

		// Token: 0x06007934 RID: 31028 RVA: 0x0046D350 File Offset: 0x0046B550
		private void GetInSightBlocks(List<MapBlockData> inSightBlocks)
		{
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
			bool flag = taiwuLocation.IsValid();
			if (flag)
			{
				MapBlockData taiwuBlock = this.GetBlock(taiwuLocation);
				this.GetNeighborBlocks(taiwuLocation.AreaId, taiwuLocation.BlockId, inSightBlocks, (int)taiwuBlock.GetConfig().ViewRange);
				inSightBlocks.Add(taiwuBlock.GetRootBlock());
			}
		}

		// Token: 0x06007935 RID: 31029 RVA: 0x0046D3B0 File Offset: 0x0046B5B0
		public unsafe void GetNeighborBlocks(short areaId, short blockId, List<MapBlockData> neighborBlocks, int maxSteps = 1)
		{
			byte areaSize = this.GetAreaSize(areaId);
			Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks(areaId);
			MapBlockData centerBlock = blocks[(int)blockId]->GetRootBlock();
			ByteCoordinate blockPos = ByteCoordinate.IndexToCoordinate(centerBlock.BlockId, areaSize);
			byte blockSize = centerBlock.GetConfig().Size;
			neighborBlocks.Clear();
			byte x = (byte)Math.Max((int)blockPos.X - maxSteps, 0);
			while ((int)x < Math.Min((int)(blockPos.X + blockSize) + maxSteps, (int)areaSize))
			{
				byte y = (byte)Math.Max((int)blockPos.Y - maxSteps, 0);
				while ((int)y < Math.Min((int)(blockPos.Y + blockSize) + maxSteps, (int)areaSize))
				{
					MapBlockData neighborBlock = blocks[(int)ByteCoordinate.CoordinateToIndex(new ByteCoordinate(x, y), areaSize)]->GetRootBlock();
					bool flag = neighborBlock.BlockId != centerBlock.BlockId && (int)centerBlock.GetManhattanDistanceToPos(x, y) <= maxSteps && neighborBlock.IsPassable() && !neighborBlocks.Contains(neighborBlock);
					if (flag)
					{
						neighborBlocks.Add(neighborBlock);
					}
					y += 1;
				}
				x += 1;
			}
		}

		// Token: 0x06007936 RID: 31030 RVA: 0x0046D4E0 File Offset: 0x0046B6E0
		public void GetLocationByDistance(Location centerLocation, int minStep, int maxStep, ref List<MapBlockData> mapBlockList)
		{
			ByteCoordinate centerBlockPos = DomainManager.Map.GetBlock(centerLocation).GetBlockPos();
			DomainManager.Map.GetRealNeighborBlocks(centerLocation.AreaId, centerLocation.BlockId, mapBlockList, maxStep, false);
			bool flag = minStep <= 0;
			if (!flag)
			{
				for (int i = mapBlockList.Count - 1; i >= 0; i--)
				{
					bool flag2 = (int)mapBlockList[i].GetManhattanDistanceToPos(centerBlockPos.X, centerBlockPos.Y) < minStep;
					if (flag2)
					{
						CollectionUtils.SwapAndRemove<MapBlockData>(mapBlockList, i);
					}
				}
			}
		}

		// Token: 0x06007937 RID: 31031 RVA: 0x0046D574 File Offset: 0x0046B774
		public unsafe void GetTaiwuVillageDistanceLocations(List<Location> locations, int distance)
		{
			Location location = DomainManager.Taiwu.GetTaiwuVillageLocation();
			byte areaSize = this.GetAreaSize(location.AreaId);
			Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks(location.AreaId);
			ByteCoordinate blockPos = ByteCoordinate.IndexToCoordinate(location.BlockId, areaSize);
			for (byte x = 0; x < areaSize; x += 1)
			{
				for (byte y = 0; y < areaSize; y += 1)
				{
					bool flag = MathF.Abs((float)(x - blockPos.X)) > (float)distance && MathF.Abs((float)(y - blockPos.Y)) > (float)distance;
					if (flag)
					{
						short index = ByteCoordinate.CoordinateToIndex(new ByteCoordinate(x, y), areaSize);
						MapBlockData block = blocks[(int)index]->GetRootBlock();
						locations.Add(new Location(block.AreaId, block.BlockId));
					}
				}
			}
		}

		// Token: 0x06007938 RID: 31032 RVA: 0x0046D660 File Offset: 0x0046B860
		public unsafe void GetAreaNotSettlementLocations(List<Location> locations, short areaId)
		{
			Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks(areaId);
			MapAreaData areaMapData = DomainManager.Map.GetElement_Areas((int)areaId);
			List<short> settlementList = ObjectPool<List<short>>.Instance.Get();
			settlementList.Clear();
			for (int i = 0; i < areaMapData.SettlementInfos.Length; i++)
			{
				short blockId = areaMapData.SettlementInfos[i].BlockId;
				bool flag = blockId >= 0;
				if (flag)
				{
					settlementList.Add(blockId);
				}
			}
			for (int j = 0; j < blocks.Length; j++)
			{
				bool flag2 = !settlementList.Contains(blocks[j]->BlockId);
				if (flag2)
				{
					MapBlockData mapBlockData = *blocks[j];
					locations.Add(new Location(mapBlockData.AreaId, mapBlockData.BlockId));
				}
			}
			ObjectPool<List<short>>.Instance.Return(settlementList);
		}

		// Token: 0x06007939 RID: 31033 RVA: 0x0046D750 File Offset: 0x0046B950
		public unsafe void GetRealNeighborBlocks(short areaId, short blockId, List<MapBlockData> neighborBlocks, int maxSteps = 1, bool includeCenter = false)
		{
			byte areaSize = this.GetAreaSize(areaId);
			Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks(areaId);
			MapBlockData centerBlock = *blocks[(int)blockId];
			ByteCoordinate blockPos = ByteCoordinate.IndexToCoordinate(centerBlock.BlockId, areaSize);
			neighborBlocks.Clear();
			byte x = (byte)Math.Max((int)blockPos.X - maxSteps, 0);
			while ((int)x < Math.Min((int)blockPos.X + maxSteps + 1, (int)areaSize))
			{
				byte y = (byte)Math.Max((int)blockPos.Y - maxSteps, 0);
				while ((int)y < Math.Min((int)blockPos.Y + maxSteps + 1, (int)areaSize))
				{
					ByteCoordinate pos = new ByteCoordinate(x, y);
					MapBlockData neighborBlock = *blocks[(int)ByteCoordinate.CoordinateToIndex(pos, areaSize)];
					bool flag = neighborBlock.IsPassable() && blockPos.GetManhattanDistance(pos) <= maxSteps && (neighborBlock.BlockId != centerBlock.BlockId || includeCenter) && !neighborBlocks.Contains(neighborBlock);
					if (flag)
					{
						neighborBlocks.Add(neighborBlock);
					}
					y += 1;
				}
				x += 1;
			}
		}

		// Token: 0x0600793A RID: 31034 RVA: 0x0046D870 File Offset: 0x0046BA70
		public unsafe Location GetRandomGraveBlock(IRandomSource random, Location location)
		{
			Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks(location.AreaId);
			MapBlockData centerBlock = blocks[(int)location.BlockId]->GetRootBlock();
			bool flag = !centerBlock.IsCityTown() && centerBlock.BlockType != EMapBlockType.Station;
			Location result;
			if (flag)
			{
				result = location;
			}
			else
			{
				byte areaSize = this.GetAreaSize(location.AreaId);
				ByteCoordinate blockPos = ByteCoordinate.IndexToCoordinate(centerBlock.BlockId, areaSize);
				byte blockSize = centerBlock.GetConfig().Size;
				int range = (blockSize >= 2) ? 3 : 2;
				List<short> blockRandomPool = ObjectPool<List<short>>.Instance.Get();
				blockRandomPool.Clear();
				byte x = (byte)Math.Max((int)blockPos.X - range, 0);
				while ((int)x < Math.Min((int)(blockPos.X + blockSize) + range, (int)areaSize))
				{
					byte y = (byte)Math.Max((int)blockPos.Y - range, 0);
					while ((int)y < Math.Min((int)(blockPos.Y + blockSize) + range, (int)areaSize))
					{
						MapBlockData block = *blocks[(int)ByteCoordinate.CoordinateToIndex(new ByteCoordinate(x, y), areaSize)];
						short blockId = block.BlockId;
						bool flag2 = blockId != centerBlock.BlockId && block.IsPassable() && !block.IsCityTown() && block.BlockType != EMapBlockType.Station && (int)centerBlock.GetManhattanDistanceToPos(x, y) <= range;
						if (flag2)
						{
							blockRandomPool.Add(blockId);
						}
						y += 1;
					}
					x += 1;
				}
				short randomBlockId = blockRandomPool[random.Next(blockRandomPool.Count)];
				ObjectPool<List<short>>.Instance.Return(blockRandomPool);
				result = new Location(location.AreaId, randomBlockId);
			}
			return result;
		}

		// Token: 0x0600793B RID: 31035 RVA: 0x0046DA2C File Offset: 0x0046BC2C
		public unsafe short GetRandomAdjacentBlockId(IRandomSource random, short areaId, short blockId)
		{
			byte areaSize = this.GetAreaSize(areaId);
			int centerX = (int)(blockId % (short)areaSize);
			int centerY = (int)(blockId / (short)areaSize);
			IntPtr intPtr = stackalloc byte[(UIntPtr)8];
			*intPtr = (byte)((sbyte)(centerX - 1));
			*(intPtr + 1) = (byte)((sbyte)centerY);
			*(intPtr + 2) = (byte)((sbyte)(centerX + 1));
			*(intPtr + 3) = (byte)((sbyte)centerY);
			*(intPtr + 4) = (byte)((sbyte)centerX);
			*(intPtr + 5) = (byte)((sbyte)(centerY - 1));
			*(intPtr + 6) = (byte)((sbyte)centerX);
			*(intPtr + 7) = (byte)((sbyte)(centerY + 1));
			sbyte* pCoords = intPtr;
			Span<MapBlockData> blocks = this.GetAreaBlocks(areaId);
			for (int candidatesCount = 4; candidatesCount > 0; candidatesCount--)
			{
				int selectedIndex = random.Next(candidatesCount);
				sbyte selectedX = pCoords[selectedIndex * 2];
				sbyte selectedY = pCoords[selectedIndex * 2 + 1];
				bool flag = selectedX >= 0 && selectedX < (sbyte)areaSize && selectedY >= 0 && selectedY < (sbyte)areaSize;
				if (flag)
				{
					int selectedBlockId = (int)(selectedX + selectedY * (sbyte)areaSize);
					MapBlockData selectedBlock = *blocks[selectedBlockId];
					bool flag2 = selectedBlock.IsPassable();
					if (flag2)
					{
						return (short)selectedBlockId;
					}
				}
				short* pCombinedCoords = (short*)pCoords;
				short last = pCombinedCoords[candidatesCount - 1];
				pCombinedCoords[selectedIndex] = last;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Failed to get passable adjacent block: (");
			defaultInterpolatedStringHandler.AppendFormatted<short>(areaId);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<short>(blockId);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600793C RID: 31036 RVA: 0x0046DB84 File Offset: 0x0046BD84
		public unsafe short GetRandomEdgeBlock(IRandomSource random, short areaId, sbyte edgeType)
		{
			Span<MapBlockData> blocks = this.GetAreaBlocks(areaId);
			List<short> blockIdList = ObjectPool<List<short>>.Instance.Get();
			byte areaSize = this.GetAreaSize(areaId);
			blockIdList.Clear();
			for (byte i = 0; i < areaSize; i += 1)
			{
				for (byte j = 0; j < areaSize; j += 1)
				{
					byte x = (edgeType == 2 || edgeType == 3) ? j : ((edgeType == 0) ? i : (areaSize - 1 - i));
					byte y = (edgeType == 0 || edgeType == 1) ? j : ((edgeType == 3) ? i : (areaSize - 1 - i));
					ByteCoordinate coord = new ByteCoordinate(x, y);
					short blockId = ByteCoordinate.CoordinateToIndex(coord, areaSize);
					bool flag = blocks[(int)blockId]->IsPassable();
					if (flag)
					{
						blockIdList.Add(blockId);
					}
				}
				bool flag2 = blockIdList.Count > 0;
				if (flag2)
				{
					break;
				}
			}
			short result = (blockIdList.Count > 0) ? blockIdList[random.Next(blockIdList.Count)] : 0;
			ObjectPool<List<short>>.Instance.Return(blockIdList);
			return result;
		}

		// Token: 0x0600793D RID: 31037 RVA: 0x0046DC9C File Offset: 0x0046BE9C
		public void EnsureBlockVisible(DataContext context, Location location)
		{
			bool flag = !location.IsValid();
			if (!flag)
			{
				MapBlockData block = this.GetBlock(location);
				bool visible = block.Visible;
				if (!visible)
				{
					block.SetVisible(true, context);
				}
			}
		}

		// Token: 0x0600793E RID: 31038 RVA: 0x0046DCD8 File Offset: 0x0046BED8
		public void HideAllBlocks(DataContext context)
		{
			List<MapBlockData> exceptBlocks = ObjectPool<List<MapBlockData>>.Instance.Get();
			this.GetInSightBlocks(exceptBlocks);
			for (short areaId = 0; areaId < 139; areaId += 1)
			{
				this.HideAllBlocks(context, areaId, exceptBlocks);
			}
			ObjectPool<List<MapBlockData>>.Instance.Return(exceptBlocks);
			DomainManager.Merchant.RefreshCaravanInTaiwuState(context);
		}

		// Token: 0x0600793F RID: 31039 RVA: 0x0046DD30 File Offset: 0x0046BF30
		public unsafe void HideAllBlocks(DataContext context, short areaId, List<MapBlockData> exceptBlocks)
		{
			Span<MapBlockData> blocks = this.GetAreaBlocks(areaId);
			for (int i = 0; i < blocks.Length; i++)
			{
				MapBlockData block = *blocks[i];
				bool flag = !block.IsPassable();
				if (!flag)
				{
					bool visible = exceptBlocks.Contains(block) || exceptBlocks.Contains(block.GetRootBlock());
					bool flag2 = visible != block.Visible;
					if (flag2)
					{
						block.SetVisible(visible, context);
					}
				}
			}
		}

		// Token: 0x06007940 RID: 31040 RVA: 0x0046DDB0 File Offset: 0x0046BFB0
		public void AddBlockMalice(DataContext context, short areaId, short blockId, int addValue)
		{
			MapBlockData block = this.GetBlock(areaId, blockId);
			short maxMalice = block.GetMaxMalice();
			bool flag = maxMalice <= 0;
			if (!flag)
			{
				block.Malice = (short)Math.Clamp((int)block.Malice + addValue, 0, (int)block.GetMaxMalice());
				this.SetBlockData(context, block);
			}
		}

		// Token: 0x06007941 RID: 31041 RVA: 0x0046DE00 File Offset: 0x0046C000
		public void AddBlockItem(DataContext context, MapBlockData block, ItemKey itemKey, int amount)
		{
			SortedList<ItemKeyAndDate, int> items = block.Items;
			bool flag = items != null && items.Count > 500;
			if (flag)
			{
				DomainManager.Item.RemoveItem(context, itemKey);
			}
			else
			{
				this.OfflineAddBlockItem(block, itemKey, amount);
			}
			this.SetBlockData(context, block);
		}

		// Token: 0x06007942 RID: 31042 RVA: 0x0046DE50 File Offset: 0x0046C050
		public void AddBlockItems(DataContext context, MapBlockData block, [TupleElementNames(new string[]
		{
			"itemKey",
			"amount"
		})] List<ValueTuple<ItemKey, int>> items)
		{
			SortedList<ItemKeyAndDate, int> items2 = block.Items;
			int? num = ((items2 != null) ? new int?(items2.Count) : null) + items.Count;
			int num2 = 500;
			bool flag = num.GetValueOrDefault() > num2 & num != null;
			if (flag)
			{
				DomainManager.Item.RemoveItems(context, items);
			}
			else
			{
				this.OfflineAddBlockItems(block, items);
			}
			this.SetBlockData(context, block);
		}

		// Token: 0x06007943 RID: 31043 RVA: 0x0046DEE9 File Offset: 0x0046C0E9
		public void RemoveBlockItem(DataContext context, MapBlockData block, ItemKeyAndDate itemKeyAndDate)
		{
			this.OfflineRemoveBlockItem(block, itemKeyAndDate);
			this.SetBlockData(context, block);
		}

		// Token: 0x06007944 RID: 31044 RVA: 0x0046DF00 File Offset: 0x0046C100
		private void OfflineAddBlockItem(MapBlockData block, ItemKey itemKey, int amount)
		{
			int locationId = block.GetLocation().GetHashCode();
			DomainManager.Item.SetOwner(itemKey, ItemOwnerType.MapBlock, locationId);
			block.AddItem(itemKey, amount);
		}

		// Token: 0x06007945 RID: 31045 RVA: 0x0046DF3C File Offset: 0x0046C13C
		private void OfflineAddBlockItems(MapBlockData block, [TupleElementNames(new string[]
		{
			"itemKey",
			"amount"
		})] List<ValueTuple<ItemKey, int>> items)
		{
			int locationId = block.GetLocation().GetHashCode();
			int i = 0;
			int count = items.Count;
			while (i < count)
			{
				ValueTuple<ItemKey, int> valueTuple = items[i];
				ItemKey itemKey = valueTuple.Item1;
				int amount = valueTuple.Item2;
				DomainManager.Item.SetOwner(itemKey, ItemOwnerType.MapBlock, locationId);
				i++;
			}
			block.AddItems(items);
		}

		// Token: 0x06007946 RID: 31046 RVA: 0x0046DFA8 File Offset: 0x0046C1A8
		private void OfflineRemoveBlockItem(MapBlockData block, ItemKeyAndDate itemKeyAndDate)
		{
			DomainManager.Item.RemoveOwner(itemKeyAndDate.ItemKey, ItemOwnerType.MapBlock, block.GetLocation().GetHashCode());
			block.RemoveItem(itemKeyAndDate);
		}

		// Token: 0x06007947 RID: 31047 RVA: 0x0046DFE4 File Offset: 0x0046C1E4
		private void OfflineRemoveBlockItemByCount(MapBlockData block, ItemKeyAndDate itemKeyAndDate, int count)
		{
			DomainManager.Item.RemoveOwner(itemKeyAndDate.ItemKey, ItemOwnerType.MapBlock, block.GetLocation().GetHashCode());
			block.RemoveItemByCount(itemKeyAndDate, count);
		}

		// Token: 0x06007948 RID: 31048 RVA: 0x0046E024 File Offset: 0x0046C224
		public unsafe void InitializeOwnedItems()
		{
			for (short areaId = 0; areaId < 139; areaId += 1)
			{
				Span<MapBlockData> blocks = this.GetAreaBlocks(areaId);
				int blockIdx = 0;
				int blocksCount = blocks.Length;
				while (blockIdx < blocksCount)
				{
					MapBlockData block = *blocks[blockIdx];
					SortedList<ItemKeyAndDate, int> blockItems = block.Items;
					bool flag = blockItems == null;
					if (!flag)
					{
						Location location = new Location(areaId, block.BlockId);
						IList<ItemKeyAndDate> keys = blockItems.Keys;
						int itemIdx = 0;
						int elementCount = blockItems.Count;
						while (itemIdx < elementCount)
						{
							ItemKey itemKey = keys[itemIdx].ItemKey;
							DomainManager.Item.SetOwner(itemKey, ItemOwnerType.MapBlock, location.GetHashCode());
							itemIdx++;
						}
					}
					blockIdx++;
				}
			}
		}

		// Token: 0x06007949 RID: 31049 RVA: 0x0046E104 File Offset: 0x0046C304
		public List<ValueTuple<Location, short>> CalcBlockTravelRoute(IRandomSource random, Location start, Location end, bool isMerchant = true)
		{
			List<ValueTuple<Location, short>> route = new List<ValueTuple<Location, short>>();
			List<ValueTuple<short, short>> areaRoute = new List<ValueTuple<short, short>>();
			Location currLocation = start;
			sbyte startEdge = -1;
			AStarMap aStarMap = new AStarMap();
			List<ByteCoordinate> pathInArea = ObjectPool<List<ByteCoordinate>>.Instance.Get();
			List<ByteCoordinate> avoidPosList = ObjectPool<List<ByteCoordinate>>.Instance.Get();
			List<short> settlementBlockIdList = ObjectPool<List<short>>.Instance.Get();
			Dictionary<TravelRouteKey, TravelRoute> findRouteDict = (isMerchant || start.AreaId >= 135 || DomainManager.World.GetWorldFunctionsStatus(4)) ? this._travelRouteDict : this._bornStateTravelRouteDict;
			areaRoute.Add(new ValueTuple<short, short>(start.AreaId, 0));
			bool flag = start.AreaId != end.AreaId;
			if (flag)
			{
				TravelRouteKey routeKey = new TravelRouteKey(start.AreaId, end.AreaId);
				bool reverse = routeKey.FromAreaId > routeKey.ToAreaId;
				bool flag2 = reverse;
				if (flag2)
				{
					routeKey.Reverse();
				}
				TravelRoute travelRoute = findRouteDict[routeKey];
				bool flag3 = !reverse;
				if (flag3)
				{
					for (int i = 0; i < travelRoute.CostList.Count; i++)
					{
						areaRoute.Add(new ValueTuple<short, short>(travelRoute.AreaList[i + 1], travelRoute.CostList[i]));
					}
				}
				else
				{
					for (int j = travelRoute.CostList.Count - 1; j >= 0; j--)
					{
						areaRoute.Add(new ValueTuple<short, short>(travelRoute.AreaList[j], travelRoute.CostList[j]));
					}
				}
			}
			Predicate<ValueTuple<Location, short>> <>9__2;
			Func<MapBlockData, bool> <>9__3;
			for (int k = 0; k < areaRoute.Count; k++)
			{
				ValueTuple<short, short> routePoint = areaRoute[k];
				short areaId = routePoint.Item1;
				byte areaSize = this.GetAreaSize(areaId);
				bool flag4 = startEdge >= 0;
				if (flag4)
				{
					short startBlockId = this.GetRandomEdgeBlock(random, areaId, startEdge);
					currLocation.AreaId = areaId;
					currLocation.BlockId = startBlockId;
					route.Add(new ValueTuple<Location, short>(currLocation, routePoint.Item2));
				}
				aStarMap.InitMap((int)areaSize, (int)areaSize, (ByteCoordinate coord) => this.GetBlock(areaId, ByteCoordinate.CoordinateToIndex(coord, areaSize)).MoveCost);
				avoidPosList.Clear();
				bool flag5 = isMerchant && areaId < 45;
				if (flag5)
				{
					settlementBlockIdList.Clear();
					foreach (SettlementInfo settlementInfo in this._areas[(int)areaId].SettlementInfos)
					{
						bool flag6 = settlementInfo.BlockId >= 0;
						if (flag6)
						{
							MapBlockData blockData = this.GetBlock(areaId, settlementInfo.BlockId);
							List<MapBlockData> groupBlockList = blockData.GroupBlockList;
							int groupBlockCount = (groupBlockList != null) ? groupBlockList.Count : 0;
							bool flag7 = groupBlockCount > 1;
							if (flag7)
							{
								settlementBlockIdList.Add(blockData.GroupBlockList.GetRandom(random).BlockId);
							}
							else
							{
								settlementBlockIdList.Add(settlementInfo.BlockId);
							}
						}
					}
					bool flag8 = settlementBlockIdList.Count > 1;
					if (flag8)
					{
						MapBlockData curBlockData = this.GetBlock(areaId, currLocation.BlockId);
						settlementBlockIdList.Sort(delegate(short aBlockId, short bBlockId)
						{
							MapBlockData aBlockData = this.GetBlock(areaId, aBlockId);
							int aDistance = (aBlockData.GetRootBlock().BlockId == end.BlockId) ? int.MaxValue : curBlockData.GetBlockPos().GetManhattanDistance(aBlockData.GetBlockPos());
							MapBlockData bBlockData = this.GetBlock(areaId, bBlockId);
							int bDistance = (bBlockData.GetRootBlock().BlockId == end.BlockId) ? int.MaxValue : curBlockData.GetBlockPos().GetManhattanDistance(bBlockData.GetBlockPos());
							return aDistance.CompareTo(bDistance);
						});
					}
					foreach (short cityBlockId in settlementBlockIdList)
					{
						this.CalcPathInArea(aStarMap, route, pathInArea, areaId, areaSize, currLocation.BlockId, cityBlockId, null);
						currLocation.BlockId = cityBlockId;
						avoidPosList.AddRange(pathInArea);
					}
				}
				bool flag9 = k == areaRoute.Count - 1;
				if (flag9)
				{
					bool flag10;
					if (isMerchant)
					{
						List<ValueTuple<Location, short>> list = route;
						Predicate<ValueTuple<Location, short>> match;
						if ((match = <>9__2) == null)
						{
							match = (<>9__2 = ((ValueTuple<Location, short> r) => r.Item1 == end));
						}
						flag10 = list.Exists(match);
					}
					else
					{
						flag10 = false;
					}
					bool flag11 = flag10;
					if (flag11)
					{
						MapBlockData blockData2 = this.GetBlock(areaId, end.BlockId);
						List<MapBlockData> blockList = ObjectPool<List<MapBlockData>>.Instance.Get();
						List<MapBlockData> groupBlockList2 = blockData2.GroupBlockList;
						int groupBlockCount2 = (groupBlockList2 != null) ? groupBlockList2.Count : 0;
						bool flag12 = groupBlockCount2 > 1;
						if (flag12)
						{
							List<MapBlockData> list2 = blockList;
							IEnumerable<MapBlockData> groupBlockList3 = blockData2.GroupBlockList;
							Func<MapBlockData, bool> predicate;
							if ((predicate = <>9__3) == null)
							{
								predicate = (<>9__3 = ((MapBlockData b) => b.GetLocation() != end));
							}
							list2.AddRange(groupBlockList3.Where(predicate));
							bool flag13 = blockList.Count > 0;
							if (flag13)
							{
								end.BlockId = blockList.GetRandom(random).BlockId;
							}
						}
						ObjectPool<List<MapBlockData>>.Instance.Return(blockList);
					}
					this.CalcPathInArea(aStarMap, route, pathInArea, areaId, areaSize, currLocation.BlockId, end.BlockId, avoidPosList);
				}
				else
				{
					sbyte[] fromPos = DomainManager.Map.GetElement_Areas((int)routePoint.Item1).GetConfig().WorldMapPos;
					sbyte[] toPos = DomainManager.Map.GetElement_Areas((int)areaRoute[k + 1].Item1).GetConfig().WorldMapPos;
					startEdge = MapAreaEdge.GetEnterEdge(fromPos, toPos);
					short blockId = this.GetRandomEdgeBlock(random, areaId, MapAreaEdge.GetOppositeEdge(startEdge));
					this.CalcPathInArea(aStarMap, route, pathInArea, areaId, areaSize, currLocation.BlockId, blockId, avoidPosList);
				}
			}
			ObjectPool<List<ByteCoordinate>>.Instance.Return(pathInArea);
			ObjectPool<List<ByteCoordinate>>.Instance.Return(avoidPosList);
			ObjectPool<List<short>>.Instance.Return(settlementBlockIdList);
			return route;
		}

		// Token: 0x0600794A RID: 31050 RVA: 0x0046E750 File Offset: 0x0046C950
		[DomainMethod]
		public List<Location> GetPathInAreaWithoutCost(Location start, Location end)
		{
			List<Location> locations = new List<Location>();
			MapDomain.GetPathInAreaWithoutCost(start, end, locations);
			return locations;
		}

		// Token: 0x0600794B RID: 31051 RVA: 0x0046E774 File Offset: 0x0046C974
		public static void GetPathInAreaWithoutCost(Location start, Location end, List<Location> locations)
		{
			locations.Clear();
			bool flag = start.Equals(end);
			if (!flag)
			{
				byte areaSize = DomainManager.Map.GetAreaSize(start.AreaId);
				MapDomain._aStarMap.InitMap((int)areaSize, (int)areaSize, delegate(ByteCoordinate coord)
				{
					MapBlockData block = DomainManager.Map.GetBlock(start.AreaId, ByteCoordinate.CoordinateToIndex(coord, areaSize));
					bool flag2 = !block.IsPassable();
					sbyte result;
					if (flag2)
					{
						result = sbyte.MaxValue;
					}
					else
					{
						result = 1;
					}
					return result;
				});
				List<ByteCoordinate> pathInArea = ObjectPool<List<ByteCoordinate>>.Instance.Get();
				MapDomain._aStarMap.FindWay(ByteCoordinate.IndexToCoordinate(start.BlockId, areaSize), ByteCoordinate.IndexToCoordinate(end.BlockId, areaSize), ref pathInArea, null);
				foreach (ByteCoordinate coord2 in pathInArea)
				{
					Location location = new Location(start.AreaId, ByteCoordinate.CoordinateToIndex(coord2, areaSize));
					locations.Add(location);
				}
				ObjectPool<List<ByteCoordinate>>.Instance.Return(pathInArea);
			}
		}

		// Token: 0x0600794C RID: 31052 RVA: 0x0046E89C File Offset: 0x0046CA9C
		public bool ContainsCharacter(Location location, int charId)
		{
			MapBlockData block = this.GetBlock(location.AreaId, location.BlockId);
			HashSet<int> characterSet = block.CharacterSet;
			return characterSet != null && characterSet.Contains(charId);
		}

		// Token: 0x0600794D RID: 31053 RVA: 0x0046E8D4 File Offset: 0x0046CAD4
		private void CalcPathInArea(AStarMap aStarMap, List<ValueTuple<Location, short>> route, List<ByteCoordinate> pathInArea, short areaId, byte areaSize, short startBlock, short endBlock, List<ByteCoordinate> avoidPosList = null)
		{
			pathInArea.Clear();
			aStarMap.FindWay(ByteCoordinate.IndexToCoordinate(startBlock, areaSize), ByteCoordinate.IndexToCoordinate(endBlock, areaSize), ref pathInArea, avoidPosList);
			for (int i = 1; i < pathInArea.Count; i++)
			{
				short blockId = ByteCoordinate.CoordinateToIndex(pathInArea[i], areaSize);
				route.Add(new ValueTuple<Location, short>(new Location(areaId, blockId), (short)this.GetBlock(areaId, blockId).MoveCost));
			}
		}

		// Token: 0x0600794E RID: 31054 RVA: 0x0046E950 File Offset: 0x0046CB50
		private short GetMerchantSettlementDestBlockId(short blockId, byte areaSize, byte blockSize, sbyte inEdge, sbyte outEdge)
		{
			bool flag = blockSize < 2 || inEdge < 0 || outEdge < 0 || outEdge == MapAreaEdge.GetOppositeEdge(inEdge);
			short result;
			if (flag)
			{
				result = blockId;
			}
			else
			{
				switch (inEdge)
				{
				case 0:
					result = ((outEdge == 2) ? (blockId + (short)(areaSize * (blockSize - 1))) : blockId);
					break;
				case 1:
					result = ((outEdge == 2) ? (blockId + (short)(areaSize * (blockSize - 1)) + (short)blockSize - 1) : (blockId + (short)blockSize - 1));
					break;
				case 2:
					result = ((outEdge == 0) ? (blockId + (short)(areaSize * (blockSize - 1))) : (blockId + (short)(areaSize * (blockSize - 1)) + (short)blockSize - 1));
					break;
				case 3:
					result = ((outEdge == 2) ? blockId : (blockId + (short)blockSize - 1));
					break;
				default:
					result = blockId;
					break;
				}
			}
			return result;
		}

		// Token: 0x0600794F RID: 31055 RVA: 0x0046EA00 File Offset: 0x0046CC00
		public unsafe int GetCollectResourceAmount(IRandomSource random, MapBlockData blockData, sbyte resourceType)
		{
			ResourceTypeItem resourceConfig = Config.ResourceType.Instance[resourceType];
			short currentResource = *(ref blockData.CurrResources.Items.FixedElementField + (IntPtr)resourceType * 2);
			int resourceMultiplier = (int)resourceConfig.CollectMultiplier;
			CValuePercentBonus buildingBonus = DomainManager.Building.GetBuildingBlockEffect(blockData.GetLocation(), EBuildingScaleEffect.CollectResourceIncomeBonus, -1);
			return (int)currentResource * (((currentResource >= 100) ? 60 : 40) + random.Next(-20, 21)) / 100 * resourceMultiplier * buildingBonus;
		}

		// Token: 0x06007950 RID: 31056 RVA: 0x0046EA7C File Offset: 0x0046CC7C
		public List<Location> GetBlockLocationGroup(Location location, bool includeSelf = true)
		{
			List<Location> retList = new List<Location>();
			retList.Add(location);
			bool flag = !location.IsValid();
			List<Location> retList2;
			if (flag)
			{
				retList2 = retList;
			}
			else
			{
				MapBlockData srcBlockData = DomainManager.Map.GetBlock(location);
				MapBlockData rootBlockData = null;
				short rootBlockId = srcBlockData.RootBlockId;
				bool flag2 = -1 != rootBlockId;
				if (flag2)
				{
					rootBlockData = DomainManager.Map.GetBlockData(location.AreaId, rootBlockId);
					bool flag3 = rootBlockData == null;
					if (flag3)
					{
						return retList;
					}
				}
				else
				{
					List<MapBlockData> groupBlockList = srcBlockData.GroupBlockList;
					bool flag4 = groupBlockList != null && groupBlockList.Count > 0;
					if (flag4)
					{
						rootBlockData = srcBlockData;
					}
				}
				bool flag5 = rootBlockData != null;
				if (flag5)
				{
					Location rootLocation = new Location(rootBlockData.AreaId, rootBlockData.BlockId);
					bool flag6 = !retList.Contains(rootLocation);
					if (flag6)
					{
						retList.Add(rootLocation);
					}
					List<MapBlockData> groupList = rootBlockData.GroupBlockList;
					bool flag7 = groupList != null && groupList.Count > 0;
					if (flag7)
					{
						groupList.ForEach(delegate(MapBlockData e)
						{
							Location cellLocation = new Location(e.AreaId, e.BlockId);
							bool flag8 = !retList.Contains(cellLocation);
							if (flag8)
							{
								retList.Add(cellLocation);
							}
						});
					}
				}
				retList2 = retList;
			}
			return retList2;
		}

		// Token: 0x06007951 RID: 31057 RVA: 0x0046EBBC File Offset: 0x0046CDBC
		public void DestroyMapBlockItemsDirect(DataContext context, MapBlockData blockData)
		{
			List<ItemKey> itemsToDestroy = ObjectPool<List<ItemKey>>.Instance.Get();
			blockData.DestroyItemsDirect(itemsToDestroy);
			this.SetBlockData(context, blockData);
			DomainManager.Item.RemoveItems(context, itemsToDestroy);
			ObjectPool<List<ItemKey>>.Instance.Return(itemsToDestroy);
		}

		// Token: 0x06007952 RID: 31058 RVA: 0x0046EC00 File Offset: 0x0046CE00
		public void ClearBlockRandomEnemies(DataContext context, MapBlockData blockData)
		{
			bool flag = blockData.TemplateEnemyList == null;
			if (!flag)
			{
				Location location = blockData.GetLocation();
				for (int i = blockData.TemplateEnemyList.Count - 1; i >= 0; i--)
				{
					MapTemplateEnemyInfo templateEnemy = blockData.TemplateEnemyList[i];
					Events.RaiseTemplateEnemyLocationChanged(context, templateEnemy, location, Location.Invalid);
				}
			}
		}

		// Token: 0x06007953 RID: 31059 RVA: 0x0046EC64 File Offset: 0x0046CE64
		[Obsolete]
		public bool RemoveBlockAnimal(Location location, DataContext context)
		{
			return false;
		}

		// Token: 0x06007954 RID: 31060 RVA: 0x0046EC78 File Offset: 0x0046CE78
		[Obsolete]
		public bool RemoveBlockSingleAnimal(Location location, short animalId, DataContext context)
		{
			AnimalAreaData data = DomainManager.Extra.GetElement_AnimalAreaData((int)location.AreaId);
			List<short> animalList;
			bool flag = data.BlockAnimalCharacterTemplateIdList != null && data.BlockAnimalCharacterTemplateIdList.TryGetValue(location.BlockId, out animalList);
			if (flag)
			{
				int index = animalList.IndexOf(animalId);
				bool flag2 = index >= 0;
				if (flag2)
				{
					animalList.RemoveAt(index);
					DomainManager.Extra.SetAnimalAreaData(context, location.AreaId, data);
					this.RemoveHunterAnimal(context, location, animalId);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007955 RID: 31061 RVA: 0x0046ED04 File Offset: 0x0046CF04
		[Obsolete]
		public void AddBlockSingleAnimal(Location location, short templateId, DataContext context)
		{
			AnimalAreaData data = DomainManager.Extra.GetElement_AnimalAreaData((int)location.AreaId);
			bool flag = data.BlockAnimalCharacterTemplateIdList.ContainsKey(location.BlockId);
			if (flag)
			{
				data.BlockAnimalCharacterTemplateIdList[location.BlockId].Add(templateId);
			}
			else
			{
				data.BlockAnimalCharacterTemplateIdList.Add(location.BlockId, new List<short>
				{
					templateId
				});
			}
			DomainManager.Extra.SetAnimalAreaData(context, location.AreaId, data);
		}

		// Token: 0x06007956 RID: 31062 RVA: 0x0046ED88 File Offset: 0x0046CF88
		[Obsolete("This method is obsolete, and will be removed in future. Use ChangeBlockTemplate instead.")]
		public void ChangeBlockTemplateUnsafe(DataContext context, Location location, short blockTemplateId, bool isTurnVisible, bool isCheckCanChange)
		{
		}

		// Token: 0x06007957 RID: 31063 RVA: 0x0046ED8B File Offset: 0x0046CF8B
		[Obsolete("This method is obsolete, and will be removed in future. Use ChangeBlockTemplate instead.")]
		public void HeavenlyTreeChangeBlockTemplateUnsafe(DataContext context, Location location, short blockTemplateId)
		{
		}

		// Token: 0x06007958 RID: 31064 RVA: 0x0046ED8E File Offset: 0x0046CF8E
		[Obsolete("This method is obsolete, and will be removed in future. Use SplitMultiBlock instead.")]
		public void ChangeBigSizeBlockToSmallSizeBlockTemplateUnSafe(DataContext context, Location location)
		{
		}

		// Token: 0x06007959 RID: 31065 RVA: 0x0046ED91 File Offset: 0x0046CF91
		[Obsolete("This method is obsolete, and will be removed in future. Use ChangeBlockTemplate instead.")]
		public void ChangeBlockTemplateUnSafe(DataContext context, Location location, short blockTemplateId)
		{
		}

		// Token: 0x0600795A RID: 31066 RVA: 0x0046ED94 File Offset: 0x0046CF94
		[Obsolete("This method is obsolete, and will be removed in future. Use FiveLoongDlcEntry.MapBlockToLoong instead.")]
		public void ChangeBlockToLoongBlockTemplateUnSafe(DataContext context, Location location, MapBlockData centerBlock, short blockTemplateId)
		{
		}

		// Token: 0x0600795B RID: 31067 RVA: 0x0046ED98 File Offset: 0x0046CF98
		public void CreateFixedTutorialArea(DataContext context)
		{
			Dictionary<int, List<short>> blockTypeDict = new Dictionary<int, List<short>>();
			Dictionary<int, List<short>> blockSubTypeDict = new Dictionary<int, List<short>>();
			List<short> allBlockKeys = MapBlock.Instance.GetAllKeys();
			for (int i = 0; i < allBlockKeys.Count; i++)
			{
				MapBlockItem blockConfig = MapBlock.Instance[allBlockKeys[i]];
				int blockType = (int)blockConfig.Type;
				int blockSubType = (int)blockConfig.SubType;
				bool flag = !blockTypeDict.ContainsKey(blockType);
				if (flag)
				{
					blockTypeDict.Add(blockType, new List<short>());
				}
				blockTypeDict[blockType].Add(blockConfig.TemplateId);
				bool flag2 = !blockSubTypeDict.ContainsKey(blockSubType);
				if (flag2)
				{
					blockSubTypeDict.Add(blockSubType, new List<short>());
				}
				blockSubTypeDict[blockSubType].Add(blockConfig.TemplateId);
			}
			DomainManager.Organization.BeginCreatingSettlements(context.Random);
			this._swCreatingNormalAreas = new Stopwatch();
			this._swCreatingSettlements = new Stopwatch();
			this._swInitializingAreaTravelRoutes = new Stopwatch();
			this.CreateEmptyStateAreas(context);
			DomainManager.Organization.CreateEmptySects(context);
			MapAreaData bornArea = this._areas[135];
			bornArea.Init(0, 135);
			this._bornAreaBlocks.Init(0);
			this.SetElement_Areas(135, bornArea, context);
			MapAreaData guideArea = this._areas[136];
			MapAreaItem guideAreaConfig = MapArea.Instance[136];
			guideArea.Init(136, 136);
			this._guideAreaBlocks.Init((int)(guideAreaConfig.Size * guideAreaConfig.Size));
			this.CreateNormalArea(context, guideArea, 136, blockTypeDict, blockSubTypeDict, -1);
			guideArea.Discovered = true;
			this.SetElement_Areas(136, guideArea, context);
			MapAreaData secretVillageArea = this._areas[137];
			secretVillageArea.Init(137, 137);
			this._secretVillageAreaBlocks.Init(0);
			this.SetElement_Areas(137, secretVillageArea, context);
			short brokenAreaId = -1;
			sbyte stateId = this.GetStateIdByStateTemplateId((short)DomainManager.World.GetTaiwuVillageStateTemplateId());
			for (int j = 0; j < 6; j++)
			{
				brokenAreaId = (short)((int)(45 + stateId * 6) + j);
				bool flag3 = context.Random.NextBool();
				if (flag3)
				{
					break;
				}
			}
			Tester.Assert(brokenAreaId >= 0, "");
			MapAreaData brokenPerformArea = this._areas[138];
			MapAreaItem brokenPerformAreaConfig = this.GetElement_Areas((int)brokenAreaId).GetConfig();
			brokenPerformArea.Init(brokenPerformAreaConfig.TemplateId, 138);
			this._brokenPerformAreaBlocks.Init(0);
			this.SetElement_Areas(138, brokenPerformArea, context);
			DomainManager.Organization.EndCreatingSettlements(context);
			Logger logger = MapDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
			defaultInterpolatedStringHandler.AppendLiteral("CreateNormalAreas: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this._swCreatingNormalAreas.Elapsed.TotalMilliseconds, "N1");
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			Logger logger2 = MapDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
			defaultInterpolatedStringHandler.AppendLiteral("CreateSettlements: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this._swCreatingSettlements.Elapsed.TotalMilliseconds, "N1");
			logger2.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			Logger logger3 = MapDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 1);
			defaultInterpolatedStringHandler.AppendLiteral("InitializeAreaTravelRoutes: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this._swInitializingAreaTravelRoutes.Elapsed.TotalMilliseconds, "N1");
			logger3.Info(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600795C RID: 31068 RVA: 0x0046F13C File Offset: 0x0046D33C
		private void CreateEmptyStateAreas(DataContext context)
		{
			Dictionary<sbyte, List<short>> thirdAreaDict = new Dictionary<sbyte, List<short>>();
			List<short> allAreaKeys = MapArea.Instance.GetAllKeys();
			for (int i = 31; i < allAreaKeys.Count; i++)
			{
				short areaTemplateId = allAreaKeys[i];
				sbyte stateTemplateId = MapArea.Instance[areaTemplateId].StateID;
				bool flag = stateTemplateId < 0;
				if (!flag)
				{
					bool flag2 = !thirdAreaDict.ContainsKey(stateTemplateId);
					if (flag2)
					{
						thirdAreaDict.Add(stateTemplateId, new List<short>());
					}
					thirdAreaDict[stateTemplateId].Add(areaTemplateId);
				}
			}
			List<short> brokenAreaTemplateIdList = ObjectPool<List<short>>.Instance.Get();
			brokenAreaTemplateIdList.Clear();
			for (int stateId = 0; stateId < 15; stateId++)
			{
				MapStateItem stateConfig = MapState.Instance[stateId + 1];
				List<short> thirdAreaList = thirdAreaDict[stateConfig.TemplateId];
				short thirdAreaTemplateId = thirdAreaList[context.Random.Next(thirdAreaList.Count)];
				for (int stateAreaIndex = 0; stateAreaIndex < 3; stateAreaIndex++)
				{
					short areaId = (short)(stateId * 3 + stateAreaIndex);
					bool flag3 = stateAreaIndex == 0;
					short areaTemplateId2;
					if (flag3)
					{
						areaTemplateId2 = (short)stateConfig.MainAreaID;
					}
					else
					{
						bool flag4 = stateAreaIndex == 1;
						if (flag4)
						{
							areaTemplateId2 = (short)stateConfig.SectAreaID;
						}
						else
						{
							areaTemplateId2 = thirdAreaTemplateId;
						}
					}
					MapAreaData area = this._areas[(int)areaId];
					area.Init(areaTemplateId2, areaId);
					MapAreaItem areaConfigData = area.GetConfig();
					bool taiwuVillageInArea = areaConfigData.StateID == DomainManager.World.GetTaiwuVillageStateTemplateId() && stateAreaIndex == 2;
					bool flag5 = taiwuVillageInArea;
					if (flag5)
					{
						AreaBlockCollection areaBlocks = this.GetRegularAreaBlocks(areaId);
						areaBlocks.Init(1);
						this.AddRegularBlockData(context, new Location(areaId, 0), new MapBlockData(areaId, 0, 0));
						short settlementId = DomainManager.Organization.CreateSettlement(context, new Location(areaId, 0), 16);
						Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
						CivilianSettlement cs = settlement as CivilianSettlement;
						short randomNameId = (cs != null) ? cs.GetRandomNameId() : -1;
						area.SettlementInfos[1] = new SettlementInfo(settlementId, 0, settlement.GetOrgTemplateId(), randomNameId);
						DomainManager.Taiwu.SetTaiwuVillageSettlementId(settlementId, context);
						DomainManager.Building.CreateBuildingArea(context, areaId, 0, 0);
						DomainManager.Building.AddTaiwuBuildingArea(context, new Location(areaId, 0));
					}
					else
					{
						this.GetRegularAreaBlocks(areaId).Init(0);
					}
				}
				thirdAreaList.Remove(thirdAreaTemplateId);
				brokenAreaTemplateIdList.AddRange(thirdAreaList);
			}
			this._brokenAreaBlocks.Init(0);
			for (int j = 0; j < brokenAreaTemplateIdList.Count; j++)
			{
				short areaId2 = (short)(45 + j);
				MapAreaData area2 = this._areas[(int)areaId2];
				short areaTemplateId3 = brokenAreaTemplateIdList[j];
				area2.Init(areaTemplateId3, areaId2);
			}
			ObjectPool<List<short>>.Instance.Return(brokenAreaTemplateIdList);
		}

		// Token: 0x0600795D RID: 31069 RVA: 0x0046F418 File Offset: 0x0046D618
		public void CreateAllAreas(DataContext context)
		{
			Dictionary<int, List<short>> blockTypeDict = new Dictionary<int, List<short>>();
			Dictionary<int, List<short>> blockSubTypeDict = new Dictionary<int, List<short>>();
			List<short> allBlockKeys = MapBlock.Instance.GetAllKeys();
			for (int i = 0; i < allBlockKeys.Count; i++)
			{
				MapBlockItem blockConfig = MapBlock.Instance[allBlockKeys[i]];
				int blockType = (int)blockConfig.Type;
				int blockSubType = (int)blockConfig.SubType;
				bool flag = !blockTypeDict.ContainsKey(blockType);
				if (flag)
				{
					blockTypeDict.Add(blockType, new List<short>());
				}
				blockTypeDict[blockType].Add(blockConfig.TemplateId);
				bool flag2 = !blockSubTypeDict.ContainsKey(blockSubType);
				if (flag2)
				{
					blockSubTypeDict.Add(blockSubType, new List<short>());
				}
				blockSubTypeDict[blockSubType].Add(blockConfig.TemplateId);
			}
			DomainManager.Organization.BeginCreatingSettlements(context.Random);
			this._swCreatingNormalAreas = new Stopwatch();
			this._swCreatingSettlements = new Stopwatch();
			this._swInitializingAreaTravelRoutes = new Stopwatch();
			this.CreateStateAreas(context, blockTypeDict, blockSubTypeDict);
			this._swInitializingAreaTravelRoutes.Start();
			this.InitAreaTravelRoute(context);
			this._swInitializingAreaTravelRoutes.Stop();
			for (short areaId = 0; areaId < 135; areaId += 1)
			{
				this.SetElement_Areas((int)areaId, this._areas[(int)areaId], context);
			}
			sbyte taiwuStateTemplateId = DomainManager.World.GetTaiwuVillageStateTemplateId();
			sbyte[] neighborStateList = MapState.Instance[taiwuStateTemplateId].NeighborStates;
			List<sbyte> initUnlockStationStateList = ObjectPool<List<sbyte>>.Instance.Get();
			List<short> areaIdList = ObjectPool<List<short>>.Instance.Get();
			initUnlockStationStateList.Clear();
			initUnlockStationStateList.Add(this.GetStateIdByStateTemplateId((short)taiwuStateTemplateId));
			for (int j = 0; j < neighborStateList.Length; j++)
			{
				initUnlockStationStateList.Add(this.GetStateIdByStateTemplateId((short)neighborStateList[j]));
			}
			while (initUnlockStationStateList.Count > (int)GlobalConfig.Instance.MapInitUnlockStationStateCount)
			{
				initUnlockStationStateList.RemoveAt(context.Random.Next(1, initUnlockStationStateList.Count));
			}
			for (int k = 0; k < initUnlockStationStateList.Count; k++)
			{
				this.GetAllAreaInState(initUnlockStationStateList[k], areaIdList);
				foreach (short areaId2 in areaIdList)
				{
					bool flag3 = !this._areas[(int)areaId2].StationUnlocked;
					if (flag3)
					{
						this.UnlockStation(context, areaId2, false);
					}
				}
			}
			DomainManager.Extra.SetStationInited(1, context);
			ObjectPool<List<sbyte>>.Instance.Return(initUnlockStationStateList);
			ObjectPool<List<short>>.Instance.Return(areaIdList);
			Stopwatch swGeneratingInitialEnemies = new Stopwatch();
			swGeneratingInitialEnemies.Start();
			DomainManager.Adventure.GenerateBrokenAreaInitialEnemies(context);
			swGeneratingInitialEnemies.Stop();
			MapAreaData bornArea = this._areas[135];
			MapAreaItem bornAreaConfig = MapArea.Instance[0];
			bornArea.Init(0, 135);
			this._bornAreaBlocks.Init((int)(bornAreaConfig.Size * bornAreaConfig.Size));
			this.CreateNormalArea(context, bornArea, 135, blockTypeDict, blockSubTypeDict, -1);
			bornArea.Discovered = true;
			this.SetElement_Areas(135, bornArea, context);
			DomainManager.Taiwu.TryAddVisitedSettlement(bornArea.SettlementInfos[0].SettlementId, context);
			MapAreaData guideArea = this._areas[136];
			MapAreaItem guideAreaConfig = MapArea.Instance[136];
			guideArea.Init(136, 136);
			this._guideAreaBlocks.Init((int)(guideAreaConfig.Size * guideAreaConfig.Size));
			this.CreateNormalArea(context, guideArea, 136, blockTypeDict, blockSubTypeDict, -1);
			guideArea.Discovered = true;
			this.SetElement_Areas(136, guideArea, context);
			MapAreaData secretVillageArea = this._areas[137];
			MapAreaItem secretVillageAreaConfig = MapArea.Instance[137];
			secretVillageArea.Init(secretVillageAreaConfig.TemplateId, 137);
			this._secretVillageAreaBlocks.Init((int)(secretVillageAreaConfig.Size * secretVillageAreaConfig.Size));
			this.CreateNormalArea(context, secretVillageArea, 137, blockTypeDict, blockSubTypeDict, -1);
			secretVillageArea.Discovered = true;
			secretVillageArea.StationUnlocked = true;
			this.SetElement_Areas(137, secretVillageArea, context);
			short brokenAreaId = -1;
			sbyte stateId = this.GetStateIdByStateTemplateId((short)DomainManager.World.GetTaiwuVillageStateTemplateId());
			for (int l = 0; l < 6; l++)
			{
				brokenAreaId = (short)((int)(45 + stateId * 6) + l);
				bool flag4 = context.Random.NextBool();
				if (flag4)
				{
					break;
				}
			}
			Tester.Assert(brokenAreaId >= 0, "");
			MapAreaData brokenPerformArea = this._areas[138];
			MapAreaItem brokenPerformAreaConfig = this.GetElement_Areas((int)brokenAreaId).GetConfig();
			brokenPerformArea.Init(brokenPerformAreaConfig.TemplateId, 138);
			this._brokenPerformAreaBlocks.Init((int)(brokenPerformAreaConfig.Size * brokenPerformAreaConfig.Size));
			this.CreateNormalArea(context, brokenPerformArea, 138, blockTypeDict, blockSubTypeDict, -1);
			brokenPerformArea.Discovered = true;
			brokenPerformArea.StationUnlocked = true;
			this.SetElement_Areas(138, brokenPerformArea, context);
			this.SetBlockAndViewRangeVisible(context, 138, brokenPerformArea.StationBlockId);
			DomainManager.Organization.EndCreatingSettlements(context);
			this.InitializeTravelMap();
			Logger logger = MapDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
			defaultInterpolatedStringHandler.AppendLiteral("CreateNormalAreas: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this._swCreatingNormalAreas.Elapsed.TotalMilliseconds, "N1");
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			Logger logger2 = MapDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
			defaultInterpolatedStringHandler.AppendLiteral("CreateSettlements: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this._swCreatingSettlements.Elapsed.TotalMilliseconds, "N1");
			logger2.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			Logger logger3 = MapDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 1);
			defaultInterpolatedStringHandler.AppendLiteral("InitializeAreaTravelRoutes: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this._swInitializingAreaTravelRoutes.Elapsed.TotalMilliseconds, "N1");
			logger3.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			Logger logger4 = MapDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 1);
			defaultInterpolatedStringHandler.AppendLiteral("InitializeBrokenAreaEnemies: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(swGeneratingInitialEnemies.Elapsed.TotalMilliseconds, "N1");
			logger4.Info(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600795E RID: 31070 RVA: 0x0046FAB4 File Offset: 0x0046DCB4
		private void CreateStateAreas(DataContext context, Dictionary<int, List<short>> blockTypeDict, Dictionary<int, List<short>> blockSubTypeDict)
		{
			Dictionary<sbyte, List<short>> thirdAreaDict = new Dictionary<sbyte, List<short>>();
			List<short> allAreaKeys = MapArea.Instance.GetAllKeys();
			for (int i = 31; i < allAreaKeys.Count; i++)
			{
				short areaTemplateId = allAreaKeys[i];
				sbyte stateTemplateId = MapArea.Instance[areaTemplateId].StateID;
				bool flag = stateTemplateId < 0;
				if (!flag)
				{
					bool flag2 = !thirdAreaDict.ContainsKey(stateTemplateId);
					if (flag2)
					{
						thirdAreaDict.Add(stateTemplateId, new List<short>());
					}
					thirdAreaDict[stateTemplateId].Add(areaTemplateId);
				}
			}
			List<short> brokenAreaTemplateIdList = ObjectPool<List<short>>.Instance.Get();
			List<short> ruinBlockRandomPool = ObjectPool<List<short>>.Instance.Get();
			brokenAreaTemplateIdList.Clear();
			for (int stateId = 0; stateId < 15; stateId++)
			{
				MapStateItem stateConfig = MapState.Instance[stateId + 1];
				List<short> thirdAreaList = thirdAreaDict[stateConfig.TemplateId];
				short thirdAreaTemplateId = thirdAreaList[context.Random.Next(thirdAreaList.Count)];
				for (int stateAreaIndex = 0; stateAreaIndex < 3; stateAreaIndex++)
				{
					short areaId = (short)(stateId * 3 + stateAreaIndex);
					bool flag3 = stateAreaIndex == 0;
					short areaTemplateId2;
					if (flag3)
					{
						areaTemplateId2 = (short)stateConfig.MainAreaID;
					}
					else
					{
						bool flag4 = stateAreaIndex == 1;
						if (flag4)
						{
							areaTemplateId2 = (short)stateConfig.SectAreaID;
						}
						else
						{
							areaTemplateId2 = thirdAreaTemplateId;
						}
					}
					MapAreaData area = this._areas[(int)areaId];
					MapAreaItem configData = MapArea.Instance[areaTemplateId2];
					area.Init(areaTemplateId2, areaId);
					byte areaSize = configData.Size;
					bool taiwuVillageInArea = configData.StateID == DomainManager.World.GetTaiwuVillageStateTemplateId() && stateAreaIndex == 2;
					bool flag5 = taiwuVillageInArea;
					if (flag5)
					{
						areaSize = (byte)GlobalConfig.Instance.TaiwuVillageForceAreaSize;
					}
					this.GetRegularAreaBlocks(areaId).Init((int)(areaSize * areaSize));
					this.CreateNormalArea(context, area, areaId, blockTypeDict, blockSubTypeDict, stateAreaIndex);
				}
				thirdAreaList.Remove(thirdAreaTemplateId);
				brokenAreaTemplateIdList.AddRange(thirdAreaList);
			}
			ruinBlockRandomPool.Clear();
			ruinBlockRandomPool.Add(118);
			ruinBlockRandomPool.Add(119);
			ruinBlockRandomPool.Add(120);
			ruinBlockRandomPool.Add(121);
			ruinBlockRandomPool.Add(122);
			ruinBlockRandomPool.Add(123);
			this._brokenAreaBlocks.Init(2250);
			for (int j = 0; j < brokenAreaTemplateIdList.Count; j++)
			{
				short areaId2 = (short)(45 + j);
				MapAreaData area2 = this._areas[(int)areaId2];
				short areaTemplateId3 = brokenAreaTemplateIdList[j];
				area2.Init(areaTemplateId3, areaId2);
				this.CreateBrokenArea(context, this._areas[(int)areaId2], areaId2, ruinBlockRandomPool);
			}
			for (sbyte areaId3 = 0; areaId3 < 45; areaId3 += 1)
			{
				int k = 0;
				int times = context.Random.Next(3, 6);
				while (k < times)
				{
					DomainManager.Extra.AnimalRandomGenerateInArea(context, (short)areaId3);
					k++;
				}
			}
			ObjectPool<List<short>>.Instance.Return(brokenAreaTemplateIdList);
			ObjectPool<List<short>>.Instance.Return(ruinBlockRandomPool);
		}

		// Token: 0x0600795F RID: 31071 RVA: 0x0046FDBC File Offset: 0x0046DFBC
		private unsafe void CreateNormalArea(DataContext context, MapAreaData mapAreaData, short areaId, Dictionary<int, List<short>> blockTypeDict, Dictionary<int, List<short>> blockSubTypeDict, int indexInState = -1)
		{
			MapDomain.<>c__DisplayClass231_0 CS$<>8__locals1 = new MapDomain.<>c__DisplayClass231_0();
			CS$<>8__locals1.areaId = areaId;
			CS$<>8__locals1.mapAreaData = mapAreaData;
			this._swCreatingNormalAreas.Start();
			short* offset4X = stackalloc short[(UIntPtr)8];
			short* offset4Y = stackalloc short[(UIntPtr)8];
			*offset4X = 1;
			offset4X[1] = -1;
			offset4X[2] = 0;
			offset4X[3] = 0;
			*offset4Y = 0;
			offset4Y[1] = 0;
			offset4Y[2] = 1;
			offset4Y[3] = -1;
			MapAreaItem areaConfigData = CS$<>8__locals1.mapAreaData.GetConfig();
			bool taiwuVillageInArea = areaConfigData.StateID == DomainManager.World.GetTaiwuVillageStateTemplateId() && indexInState == 2;
			CS$<>8__locals1.areaSize = areaConfigData.Size;
			bool flag = taiwuVillageInArea;
			if (flag)
			{
				CS$<>8__locals1.areaSize = (byte)GlobalConfig.Instance.TaiwuVillageForceAreaSize;
			}
			CS$<>8__locals1.areaBlocks = ((CS$<>8__locals1.areaId < 45) ? this.GetRegularAreaBlocks(CS$<>8__locals1.areaId) : ((CS$<>8__locals1.areaId == 135) ? this._bornAreaBlocks : ((CS$<>8__locals1.areaId == 136) ? this._guideAreaBlocks : ((CS$<>8__locals1.areaId == 138) ? this._brokenPerformAreaBlocks : this._secretVillageAreaBlocks)))).GetArray();
			int settlementCount = areaConfigData.OrganizationId.Length + (taiwuVillageInArea ? 1 : 0);
			List<short> staticBlockIdList = ObjectPool<List<short>>.Instance.Get();
			bool flag2 = areaConfigData.CustomBlockConfig == null;
			if (flag2)
			{
				int keepRange = (int)(CS$<>8__locals1.areaSize * 3 / 4);
				bool flag3 = taiwuVillageInArea;
				if (flag3)
				{
					keepRange = Math.Max(keepRange, (from p in GameData.Domains.Map.SharedConstValue.TaiwuEnsuredSurroundingBlockOffsets
					select Math.Abs(p.Item1 + p.Item2)).Max());
				}
				byte[,] edgeClipMap = this.GetAreaShape(context.Random, CS$<>8__locals1.areaSize, (byte)keepRange, true);
				byte[,] availableBlockMap = new byte[(int)CS$<>8__locals1.areaSize, (int)CS$<>8__locals1.areaSize];
				for (byte x = 0; x < CS$<>8__locals1.areaSize; x += 1)
				{
					for (byte y = 0; y < CS$<>8__locals1.areaSize; y += 1)
					{
						bool flag4 = edgeClipMap[(int)x, (int)y] == 0;
						if (flag4)
						{
							short blockId = ByteCoordinate.CoordinateToIndex(new ByteCoordinate(x, y), CS$<>8__locals1.areaSize);
							CS$<>8__locals1.areaBlocks[(int)blockId] = new MapBlockData(CS$<>8__locals1.areaId, blockId, 126);
							availableBlockMap[(int)x, (int)y] = 1;
						}
					}
				}
				this.PlaceStaticBlocks(CS$<>8__locals1.areaId, areaConfigData, CS$<>8__locals1.areaSize, CS$<>8__locals1.areaBlocks, availableBlockMap, edgeClipMap, taiwuVillageInArea, staticBlockIdList, context.Random);
				CS$<>8__locals1.mapAreaData.Discovered = taiwuVillageInArea;
				this.PlaceOtherBlocks(CS$<>8__locals1.areaId, areaConfigData, CS$<>8__locals1.areaSize, CS$<>8__locals1.areaBlocks, availableBlockMap, staticBlockIdList, settlementCount, context.Random);
				this.FixSeriesBlocks(areaConfigData, CS$<>8__locals1.areaSize, CS$<>8__locals1.areaBlocks, blockTypeDict, context.Random);
				this.FixEncircleBlocks(areaConfigData, CS$<>8__locals1.areaSize, CS$<>8__locals1.areaBlocks, blockTypeDict, context.Random);
			}
			else
			{
				short[][] presetMapData = CustomMapBlockConfig.Data[areaConfigData.CustomBlockConfig];
				CS$<>8__locals1.areaSize = (byte)presetMapData.GetLength(0);
				byte i = 0;
				while ((int)i < presetMapData.Length)
				{
					short[] linePreset = presetMapData[(int)i];
					byte j = 0;
					while ((int)j < linePreset.Length)
					{
						short blockTemplateId = linePreset[(int)j];
						MapBlockItem blockConfig = MapBlock.Instance[blockTemplateId];
						bool flag5 = blockConfig == null;
						if (!flag5)
						{
							ByteCoordinate byteCoordinate = new ByteCoordinate(i, j);
							short blockId2 = ByteCoordinate.CoordinateToIndex(byteCoordinate, CS$<>8__locals1.areaSize);
							MapBlockData block = CS$<>8__locals1.areaBlocks[(int)blockId2];
							bool flag6 = block == null;
							if (flag6)
							{
								block = new MapBlockData(CS$<>8__locals1.areaId, blockId2, blockTemplateId);
								block.Visible = true;
								CS$<>8__locals1.areaBlocks[(int)blockId2] = block;
								bool flag7 = block.IsCityTown();
								if (flag7)
								{
									staticBlockIdList.Add(blockId2);
								}
							}
							else
							{
								bool flag8 = -1 != block.RootBlockId;
								if (flag8)
								{
									goto IL_4C5;
								}
							}
							bool flag9 = blockConfig.Size > 1;
							if (flag9)
							{
								for (byte aIndex = 0; aIndex < blockConfig.Size; aIndex += 1)
								{
									for (byte bIndex = 0; bIndex < blockConfig.Size; bIndex += 1)
									{
										bool flag10 = aIndex + bIndex == 0 || byteCoordinate.X + aIndex >= CS$<>8__locals1.areaSize || byteCoordinate.Y + bIndex >= CS$<>8__locals1.areaSize;
										if (!flag10)
										{
											ByteCoordinate childByteCoordinate = new ByteCoordinate(byteCoordinate.X + aIndex, byteCoordinate.Y + bIndex);
											short childBlockId = ByteCoordinate.CoordinateToIndex(childByteCoordinate, CS$<>8__locals1.areaSize);
											CS$<>8__locals1.areaBlocks[(int)childBlockId] = new MapBlockData(CS$<>8__locals1.areaId, childBlockId, -1);
											CS$<>8__locals1.areaBlocks[(int)childBlockId].SetToSizeBlock(block);
											CS$<>8__locals1.areaBlocks[(int)childBlockId].Visible = true;
										}
									}
								}
							}
						}
						IL_4C5:
						j += 1;
					}
					i += 1;
				}
				List<MapBlockData> rangeBlockList = ObjectPool<List<MapBlockData>>.Instance.Get();
				for (int k = 0; k < staticBlockIdList.Count; k++)
				{
					bool shouldPrint = false;
					bool flag11 = !staticBlockIdList.CheckIndex(k);
					if (flag11)
					{
						shouldPrint = true;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
						defaultInterpolatedStringHandler.AppendLiteral("staticBlockIdList invalid index: ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(k);
						AdaptableLog.Warning(defaultInterpolatedStringHandler.ToStringAndClear(), false);
					}
					bool flag12 = !CS$<>8__locals1.areaBlocks.CheckIndex((int)staticBlockIdList[k]);
					if (flag12)
					{
						shouldPrint = true;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 1);
						defaultInterpolatedStringHandler.AppendLiteral("areaBlocks invalid index: ");
						defaultInterpolatedStringHandler.AppendFormatted<short>(staticBlockIdList[k]);
						AdaptableLog.Warning(defaultInterpolatedStringHandler.ToStringAndClear(), false);
					}
					bool flag13 = shouldPrint;
					if (flag13)
					{
						MapDomain.PrintAreaDebug(CS$<>8__locals1.areaBlocks, CS$<>8__locals1.areaSize);
						StringBuilder sb = new StringBuilder();
						int l = 0;
						int jc = staticBlockIdList.Count;
						while (l < jc)
						{
							bool flag14 = l != 0;
							if (flag14)
							{
								sb.Append(", ");
							}
							StringBuilder stringBuilder = sb;
							StringBuilder stringBuilder2 = stringBuilder;
							StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(0, 1, stringBuilder);
							appendInterpolatedStringHandler.AppendFormatted<short>(staticBlockIdList[l]);
							stringBuilder2.Append(ref appendInterpolatedStringHandler);
							l++;
						}
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
						defaultInterpolatedStringHandler.AppendFormatted("staticBlockIdList");
						defaultInterpolatedStringHandler.AppendLiteral(": ");
						defaultInterpolatedStringHandler.AppendFormatted<StringBuilder>(sb);
						AdaptableLog.Warning(defaultInterpolatedStringHandler.ToStringAndClear(), false);
					}
					else
					{
						MapBlockData block2 = CS$<>8__locals1.areaBlocks[(int)staticBlockIdList[k]];
						MapBlockItem blockConfig2 = block2.GetConfig();
						bool flag15 = blockConfig2.Range > 0;
						if (flag15)
						{
							ByteCoordinate byteCoordinate2 = ByteCoordinate.IndexToCoordinate(block2.BlockId, CS$<>8__locals1.areaSize);
							rangeBlockList.Clear();
							byte x2 = (byte)Math.Max((int)(byteCoordinate2.X - blockConfig2.Range), 0);
							while ((int)x2 < Math.Min((int)(byteCoordinate2.X + blockConfig2.Size + blockConfig2.Range), (int)CS$<>8__locals1.areaSize))
							{
								byte y2 = (byte)Math.Max((int)(byteCoordinate2.Y - blockConfig2.Range), 0);
								while ((int)y2 < Math.Min((int)(byteCoordinate2.Y + blockConfig2.Size + blockConfig2.Range), (int)CS$<>8__locals1.areaSize))
								{
									MapBlockData neighborBlock = CS$<>8__locals1.areaBlocks[(int)ByteCoordinate.CoordinateToIndex(new ByteCoordinate(x2, y2), CS$<>8__locals1.areaSize)].GetRootBlock();
									bool flag16 = neighborBlock.BlockId != block2.BlockId && block2.GetManhattanDistanceToPos(x2, y2) <= blockConfig2.Range && neighborBlock.IsPassable() && !rangeBlockList.Contains(neighborBlock);
									if (flag16)
									{
										rangeBlockList.Add(neighborBlock);
									}
									y2 += 1;
								}
								x2 += 1;
							}
							foreach (MapBlockData rangeBlock in rangeBlockList)
							{
								bool flag17 = rangeBlock.IsPassable() && rangeBlock.GetConfig().Size == 1 && rangeBlock.BelongBlockId == -1;
								if (flag17)
								{
									rangeBlock.BelongBlockId = block2.BlockId;
								}
							}
						}
					}
				}
				ObjectPool<List<MapBlockData>>.Instance.Return(rangeBlockList);
			}
			List<MapBlockData> maliceBlockRandomPool = ObjectPool<List<MapBlockData>>.Instance.Get();
			maliceBlockRandomPool.Clear();
			short blockId3 = 0;
			while ((int)blockId3 < CS$<>8__locals1.areaBlocks.Length)
			{
				MapBlockData block3 = CS$<>8__locals1.areaBlocks[(int)blockId3];
				bool flag18 = block3.IsPassable();
				if (flag18)
				{
					block3.InitResources(context.Random);
					bool flag19 = block3.GetConfig().MaxMalice > 0;
					if (flag19)
					{
						maliceBlockRandomPool.Add(block3);
					}
				}
				blockId3 += 1;
			}
			int initMaliceBlockCount = maliceBlockRandomPool.Count * 5 / 100;
			for (int m = 0; m < initMaliceBlockCount; m++)
			{
				int index = context.Random.Next(maliceBlockRandomPool.Count);
				MapBlockData block4 = maliceBlockRandomPool[index];
				short maxMalice = block4.GetMaxMalice();
				int initialMalicePercent = RedzenHelper.SkewDistribute(context.Random, 20f, 8f, 1.8f, 0, 80);
				block4.Malice = (short)((int)maxMalice * initialMalicePercent / 100);
				maliceBlockRandomPool.RemoveAt(index);
			}
			ObjectPool<List<MapBlockData>>.Instance.Return(maliceBlockRandomPool);
			bool flag20 = CS$<>8__locals1.areaId < 45;
			if (flag20)
			{
				short blockId4 = 0;
				while ((int)blockId4 < CS$<>8__locals1.areaBlocks.Length)
				{
					this.AddRegularBlockData(context, new Location(CS$<>8__locals1.areaId, blockId4), CS$<>8__locals1.areaBlocks[(int)blockId4]);
					blockId4 += 1;
				}
				bool flag21 = CS$<>8__locals1.mapAreaData.StationBlockId >= 0;
				if (flag21)
				{
					CS$<>8__locals1.areaBlocks[(int)CS$<>8__locals1.mapAreaData.StationBlockId].SetVisible(true, context);
				}
			}
			else
			{
				bool flag22 = CS$<>8__locals1.areaId == 135;
				if (flag22)
				{
					short blockId5 = 0;
					while ((int)blockId5 < CS$<>8__locals1.areaBlocks.Length)
					{
						this.AddElement_BornAreaBlocks(blockId5, CS$<>8__locals1.areaBlocks[(int)blockId5], context);
						blockId5 += 1;
					}
				}
				else
				{
					bool flag23 = CS$<>8__locals1.areaId == 136;
					if (flag23)
					{
						short blockId6 = 0;
						while ((int)blockId6 < CS$<>8__locals1.areaBlocks.Length)
						{
							this.AddElement_GuideAreaBlocks(blockId6, CS$<>8__locals1.areaBlocks[(int)blockId6], context);
							blockId6 += 1;
						}
					}
					else
					{
						bool flag24 = CS$<>8__locals1.areaId == 137;
						if (flag24)
						{
							short blockId7 = 0;
							while ((int)blockId7 < CS$<>8__locals1.areaBlocks.Length)
							{
								this.AddElement_SecretVillageAreaBlocks(blockId7, CS$<>8__locals1.areaBlocks[(int)blockId7], context);
								blockId7 += 1;
							}
						}
						else
						{
							bool flag25 = CS$<>8__locals1.areaId == 138;
							if (flag25)
							{
								short blockId8 = 0;
								while ((int)blockId8 < CS$<>8__locals1.areaBlocks.Length)
								{
									this.AddElement_BrokenPerformAreaBlocks(blockId8, CS$<>8__locals1.areaBlocks[(int)blockId8], context);
									blockId8 += 1;
								}
							}
						}
					}
				}
			}
			List<MapBlockData> neighborBlocks = ObjectPool<List<MapBlockData>>.Instance.Get();
			bool isStoryStockade = CS$<>8__locals1.areaId == 138;
			bool flag26 = isStoryStockade;
			if (flag26)
			{
				settlementCount = 1;
			}
			CS$<>8__locals1.taiwuVillageBlockId = -1;
			for (int n = settlementCount - 1; n >= 0; n--)
			{
				short blockId9 = staticBlockIdList[n];
				Location location = new Location(CS$<>8__locals1.areaId, blockId9);
				MapBlockData block5 = CS$<>8__locals1.areaBlocks[(int)blockId9];
				bool isTaiwuVillage = taiwuVillageInArea && n == settlementCount - 1;
				sbyte orgTemplateId = isTaiwuVillage ? 16 : (isStoryStockade ? 38 : areaConfigData.OrganizationId[n]);
				this._swCreatingSettlements.Start();
				short settlementId = DomainManager.Organization.CreateSettlement(context, location, orgTemplateId);
				this._swCreatingSettlements.Stop();
				Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
				CivilianSettlement cs = settlement as CivilianSettlement;
				short randomNameId = (cs != null) ? cs.GetRandomNameId() : -1;
				CS$<>8__locals1.mapAreaData.SettlementInfos[n] = new SettlementInfo(settlementId, blockId9, settlement.GetOrgTemplateId(), randomNameId);
				bool flag27 = isTaiwuVillage;
				if (flag27)
				{
					CS$<>8__locals1.taiwuVillageBlockId = block5.BlockId;
					DomainManager.Taiwu.SetTaiwuVillageSettlementId(settlementId, context);
				}
				bool flag28 = CS$<>8__locals1.areaId != 137;
				if (flag28)
				{
					this.GetNeighborBlocks(CS$<>8__locals1.areaId, blockId9, neighborBlocks, (int)((CS$<>8__locals1.areaId != 135) ? 1 : block5.GetConfig().ViewRange));
					block5.SetVisible(true, context);
					bool flag29 = block5.GroupBlockList != null;
					if (flag29)
					{
						for (int j2 = 0; j2 < block5.GroupBlockList.Count; j2++)
						{
							block5.SetVisible(true, context);
						}
					}
					for (int j3 = 0; j3 < neighborBlocks.Count; j3++)
					{
						MapBlockData neighborBlock2 = neighborBlocks[j3];
						neighborBlock2.SetVisible(true, context);
						bool flag30 = neighborBlock2.GroupBlockList != null;
						if (flag30)
						{
							for (int k2 = 0; k2 < neighborBlock2.GroupBlockList.Count; k2++)
							{
								neighborBlock2.GroupBlockList[k2].SetVisible(true, context);
							}
						}
					}
				}
				bool flag31 = CS$<>8__locals1.areaBlocks[(int)blockId9].IsCityTown();
				if (flag31)
				{
					DomainManager.Building.CreateBuildingArea(context, CS$<>8__locals1.areaId, blockId9, CS$<>8__locals1.areaBlocks[(int)blockId9].TemplateId);
					bool flag32 = orgTemplateId == 16;
					if (flag32)
					{
						DomainManager.Building.AddTaiwuBuildingArea(context, new Location(CS$<>8__locals1.areaId, blockId9));
					}
				}
			}
			bool flag33 = CS$<>8__locals1.taiwuVillageBlockId >= 0;
			if (flag33)
			{
				MapDomain.<>c__DisplayClass231_1 CS$<>8__locals2 = new MapDomain.<>c__DisplayClass231_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.locations = new List<Location>();
				ValueTuple<sbyte, sbyte, sbyte>* directions = stackalloc ValueTuple<sbyte, sbyte, sbyte>[checked(unchecked((UIntPtr)8) * (UIntPtr)sizeof(ValueTuple<sbyte, sbyte, sbyte>))];
				*directions = new ValueTuple<sbyte, sbyte, sbyte>(1, 0, 0);
				directions[sizeof(ValueTuple<sbyte, sbyte, sbyte>) / sizeof(ValueTuple<sbyte, sbyte, sbyte>)] = new ValueTuple<sbyte, sbyte, sbyte>(0, 1, 0);
				directions[(IntPtr)2 * (IntPtr)sizeof(ValueTuple<sbyte, sbyte, sbyte>) / (IntPtr)sizeof(ValueTuple<sbyte, sbyte, sbyte>)] = new ValueTuple<sbyte, sbyte, sbyte>(-1, 0, 1);
				directions[(IntPtr)3 * (IntPtr)sizeof(ValueTuple<sbyte, sbyte, sbyte>) / (IntPtr)sizeof(ValueTuple<sbyte, sbyte, sbyte>)] = new ValueTuple<sbyte, sbyte, sbyte>(0, -1, 1);
				directions[(IntPtr)4 * (IntPtr)sizeof(ValueTuple<sbyte, sbyte, sbyte>) / (IntPtr)sizeof(ValueTuple<sbyte, sbyte, sbyte>)] = new ValueTuple<sbyte, sbyte, sbyte>(1, 1, -1);
				directions[(IntPtr)5 * (IntPtr)sizeof(ValueTuple<sbyte, sbyte, sbyte>) / (IntPtr)sizeof(ValueTuple<sbyte, sbyte, sbyte>)] = new ValueTuple<sbyte, sbyte, sbyte>(-1, -1, 0);
				directions[(IntPtr)6 * (IntPtr)sizeof(ValueTuple<sbyte, sbyte, sbyte>) / (IntPtr)sizeof(ValueTuple<sbyte, sbyte, sbyte>)] = new ValueTuple<sbyte, sbyte, sbyte>(-1, 1, -1);
				directions[(IntPtr)7 * (IntPtr)sizeof(ValueTuple<sbyte, sbyte, sbyte>) / (IntPtr)sizeof(ValueTuple<sbyte, sbyte, sbyte>)] = new ValueTuple<sbyte, sbyte, sbyte>(1, -1, -1);
				CollectionUtils.Shuffle<ValueTuple<sbyte, sbyte, sbyte>>(context.Random, directions, 8);
				CS$<>8__locals2.origin = ByteCoordinate.IndexToCoordinate(CS$<>8__locals2.CS$<>8__locals1.taiwuVillageBlockId, CS$<>8__locals2.CS$<>8__locals1.areaSize);
				for (int i2 = 0; i2 < 8; i2++)
				{
					ValueTuple<sbyte, sbyte, sbyte> valueTuple = directions[(IntPtr)i2 * (IntPtr)sizeof(ValueTuple<sbyte, sbyte, sbyte>) / (IntPtr)sizeof(ValueTuple<sbyte, sbyte, sbyte>)];
					MapDomain.<>c__DisplayClass231_2 CS$<>8__locals3;
					CS$<>8__locals3.offsetX = valueTuple.Item1;
					CS$<>8__locals3.offsetY = valueTuple.Item2;
					sbyte offsetDistance = valueTuple.Item3;
					int distance = ((i2 == 0) ? 3 : context.Random.Next(6, 8)) + (int)offsetDistance;
					ByteCoordinate coordinate = CS$<>8__locals2.<CreateNormalArea>g__GiveCoordinate|3(distance, ref CS$<>8__locals3);
					while (!CS$<>8__locals2.<CreateNormalArea>g__IsBigBlockCanPlace|2(CS$<>8__locals2.CS$<>8__locals1.areaBlocks[(int)ByteCoordinate.CoordinateToIndex(coordinate, CS$<>8__locals2.CS$<>8__locals1.areaSize)], 2))
					{
						ByteCoordinate coordinateUnmoved = coordinate;
						for (int j4 = 0; j4 < 4; j4++)
						{
							MapBlockData currentBlock = CS$<>8__locals2.CS$<>8__locals1.areaBlocks[(int)ByteCoordinate.CoordinateToIndex(coordinate, CS$<>8__locals2.CS$<>8__locals1.areaSize)];
							bool flag34 = CS$<>8__locals2.<CreateNormalArea>g__IsBigBlockCanPlace|2(currentBlock, 2);
							if (flag34)
							{
								break;
							}
							bool flag35 = currentBlock.RootBlockId >= 0;
							if (flag35)
							{
								bool flag36 = CS$<>8__locals2.<CreateNormalArea>g__IsBigBlockCanPlace|2(CS$<>8__locals2.CS$<>8__locals1.areaBlocks[(int)currentBlock.RootBlockId], CS$<>8__locals2.CS$<>8__locals1.areaSize);
								if (flag36)
								{
									coordinate = ByteCoordinate.IndexToCoordinate(currentBlock.RootBlockId, CS$<>8__locals2.CS$<>8__locals1.areaSize);
									break;
								}
							}
							int newX = (int)(coordinateUnmoved.X + (byte)CS$<>8__locals3.offsetX);
							int newY = (int)(coordinateUnmoved.Y + (byte)CS$<>8__locals3.offsetY);
							bool flag37 = newX < 0 || newX >= (int)CS$<>8__locals2.CS$<>8__locals1.areaSize;
							if (flag37)
							{
								newY += (int)offset4Y[j4];
							}
							bool flag38 = newY < 0 || newY >= (int)CS$<>8__locals2.CS$<>8__locals1.areaSize;
							if (flag38)
							{
								newX += (int)offset4X[j4];
							}
							bool flag39 = newX >= 0 && newX < (int)CS$<>8__locals2.CS$<>8__locals1.areaSize;
							if (flag39)
							{
								coordinate.X = (byte)newX;
							}
							bool flag40 = newY >= 0 && newY < (int)CS$<>8__locals2.CS$<>8__locals1.areaSize;
							if (flag40)
							{
								coordinate.Y = (byte)newY;
							}
						}
						bool flag41 = !CS$<>8__locals2.<CreateNormalArea>g__IsBigBlockCanPlace|2(CS$<>8__locals2.CS$<>8__locals1.areaBlocks[(int)ByteCoordinate.CoordinateToIndex(coordinate, CS$<>8__locals2.CS$<>8__locals1.areaSize)], 2);
						if (flag41)
						{
							distance++;
							ByteCoordinate nextCoordinate = CS$<>8__locals2.<CreateNormalArea>g__GiveCoordinate|3(distance, ref CS$<>8__locals3);
							bool flag42 = nextCoordinate.X == 0 || nextCoordinate.Y == 0 || nextCoordinate.X == CS$<>8__locals2.CS$<>8__locals1.areaSize - 1 || nextCoordinate.Y == CS$<>8__locals2.CS$<>8__locals1.areaSize - 1;
							if (flag42)
							{
								CS$<>8__locals2.<CreateNormalArea>g__PrintBigBlockCanPlaceMap|1();
								throw new Exception("can not place sword tomb, need re-run: CreateNormalArea");
							}
							coordinate = nextCoordinate;
						}
					}
					CS$<>8__locals2.locations.Add(new Location(CS$<>8__locals2.CS$<>8__locals1.areaId, ByteCoordinate.CoordinateToIndex(coordinate, CS$<>8__locals2.CS$<>8__locals1.areaSize)));
				}
				int i3 = 0;
				IEnumerable<Location> locations = CS$<>8__locals2.locations;
				Func<Location, int> keySelector;
				if ((keySelector = CS$<>8__locals2.<>9__4) == null)
				{
					keySelector = (CS$<>8__locals2.<>9__4 = ((Location loc) => CS$<>8__locals2.origin.GetManhattanDistance(ByteCoordinate.IndexToCoordinate(loc.BlockId, CS$<>8__locals2.CS$<>8__locals1.areaSize))));
				}
				foreach (Location location2 in locations.OrderBy(keySelector))
				{
					this.SetElement_SwordTombLocations(i3, location2, context);
					i3++;
				}
			}
			ObjectPool<List<MapBlockData>>.Instance.Return(neighborBlocks);
			ObjectPool<List<short>>.Instance.Return(staticBlockIdList);
			this._swCreatingNormalAreas.Stop();
		}

		// Token: 0x06007960 RID: 31072 RVA: 0x00471098 File Offset: 0x0046F298
		public unsafe static void PrintAreaDebug(Span<MapBlockData> blockCollection, byte areaSize)
		{
			StringBuilder sb = new StringBuilder();
			AdaptableLog.Info("The blockCollection is:");
			for (int y = 0; y < (int)areaSize; y++)
			{
				sb.Clear();
				for (int x = 0; x < (int)areaSize; x++)
				{
					short index = ByteCoordinate.CoordinateToIndex(new ByteCoordinate((byte)x, (byte)y), areaSize);
					StringBuilder stringBuilder = sb;
					StringBuilder stringBuilder2 = stringBuilder;
					StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(1, 1, stringBuilder);
					appendInterpolatedStringHandler.AppendLiteral(" ");
					appendInterpolatedStringHandler.AppendFormatted<short>(blockCollection[(int)index]->TemplateId, "D3");
					stringBuilder2.Append(ref appendInterpolatedStringHandler);
				}
				AdaptableLog.Info(sb.ToString());
			}
			AdaptableLog.Info("The BelongBlockId is:");
			for (int y2 = 0; y2 < (int)areaSize; y2++)
			{
				sb.Clear();
				for (int x2 = 0; x2 < (int)areaSize; x2++)
				{
					short index2 = ByteCoordinate.CoordinateToIndex(new ByteCoordinate((byte)x2, (byte)y2), areaSize);
					int belongBlockId = (int)((*blockCollection[(int)index2] != null) ? blockCollection[(int)index2]->BelongBlockId : -1);
					StringBuilder stringBuilder3 = sb;
					string value;
					if (belongBlockId < 0)
					{
						value = " XXX";
					}
					else
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 1);
						defaultInterpolatedStringHandler.AppendLiteral(" ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(belongBlockId, "D3");
						value = defaultInterpolatedStringHandler.ToStringAndClear();
					}
					stringBuilder3.Append(value);
				}
				AdaptableLog.Info(sb.ToString());
			}
			AdaptableLog.Info("The TemplateId is:");
			for (int y3 = 0; y3 < (int)areaSize; y3++)
			{
				sb.Clear();
				for (int x3 = 0; x3 < (int)areaSize; x3++)
				{
					short index3 = ByteCoordinate.CoordinateToIndex(new ByteCoordinate((byte)x3, (byte)y3), areaSize);
					int templateId = (int)((*blockCollection[(int)index3] != null) ? blockCollection[(int)index3]->TemplateId : -1);
					StringBuilder stringBuilder4 = sb;
					string value2;
					if (templateId < 0)
					{
						value2 = " XXX";
					}
					else
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 1);
						defaultInterpolatedStringHandler.AppendLiteral(" ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(templateId, "D3");
						value2 = defaultInterpolatedStringHandler.ToStringAndClear();
					}
					stringBuilder4.Append(value2);
				}
				AdaptableLog.Info(sb.ToString());
			}
		}

		// Token: 0x06007961 RID: 31073 RVA: 0x004712D8 File Offset: 0x0046F4D8
		private void CreateBrokenArea(DataContext context, MapAreaData mapAreaData, short areaId, List<short> ruinBlockRandomPool)
		{
			byte areaSize = 5;
			short blockIdBegin = (short)(areaSize * areaSize) * (areaId - 45);
			byte[,] edgeClipMap = this.GetAreaShape(context.Random, areaSize, areaSize, true);
			List<short> validBlockIdList = ObjectPool<List<short>>.Instance.Get();
			validBlockIdList.Clear();
			for (byte x = 0; x < areaSize; x += 1)
			{
				for (byte y = 0; y < areaSize; y += 1)
				{
					short blockId = ByteCoordinate.CoordinateToIndex(new ByteCoordinate(x, y), areaSize);
					bool validBlock = edgeClipMap[(int)x, (int)y] > 0;
					short blockTemplateId = validBlock ? ruinBlockRandomPool[context.Random.Next(ruinBlockRandomPool.Count)] : 126;
					MapBlockData block = new MapBlockData(areaId, blockId, blockTemplateId);
					bool flag = validBlock;
					if (flag)
					{
						validBlockIdList.Add(blockId);
					}
					block.InitResources(context.Random);
					this.AddElement_BrokenAreaBlocks(blockIdBegin + blockId, block, context);
				}
			}
			mapAreaData.Discovered = false;
			mapAreaData.StationBlockId = validBlockIdList[context.Random.Next(validBlockIdList.Count)];
			MapBlockData stationBlock = this._brokenAreaBlocks[blockIdBegin + mapAreaData.StationBlockId];
			stationBlock.ChangeTemplateId(38, false);
			stationBlock.SetVisible(true, context);
			this.SetElement_BrokenAreaBlocks(blockIdBegin + mapAreaData.StationBlockId, stationBlock, context);
			ObjectPool<List<short>>.Instance.Return(validBlockIdList);
		}

		// Token: 0x06007962 RID: 31074 RVA: 0x00471440 File Offset: 0x0046F640
		private byte[,] GetAreaShape(IRandomSource random, byte mapSize, byte keepRange, bool ensureEdge = false)
		{
			MapDomain.<>c__DisplayClass234_0 CS$<>8__locals1 = new MapDomain.<>c__DisplayClass234_0();
			CS$<>8__locals1.mapSize = mapSize;
			bool flag = CS$<>8__locals1.mapSize < keepRange;
			byte[,] result;
			if (flag)
			{
				AdaptableLog.TagWarning("MapCreate", "error,sizeMax < sizeMin", false);
				result = null;
			}
			else
			{
				CS$<>8__locals1.map = new byte[(int)CS$<>8__locals1.mapSize, (int)CS$<>8__locals1.mapSize];
				byte centerPos = CS$<>8__locals1.mapSize / 2 - 1;
				float safeRange = (float)keepRange / 2f;
				ByteCoordinate center = new ByteCoordinate(centerPos, centerPos);
				List<ByteCoordinate> smoothPosList = ObjectPool<List<ByteCoordinate>>.Instance.Get();
				smoothPosList.Clear();
				for (byte i = 0; i < CS$<>8__locals1.mapSize; i += 1)
				{
					for (byte j = 0; j < CS$<>8__locals1.mapSize; j += 1)
					{
						ByteCoordinate pos3 = new ByteCoordinate(i, j);
						bool flag2 = ByteCoordinate.Distance(center, pos3) > (double)safeRange;
						if (flag2)
						{
							byte rate = 50;
							bool flag3 = ensureEdge && (i == 0 || i == CS$<>8__locals1.mapSize - 1 || j == 0 || j == CS$<>8__locals1.mapSize - 1);
							if (flag3)
							{
								rate += 25;
							}
							CS$<>8__locals1.map[(int)i, (int)j] = ((random.Next(100) < (int)rate) ? 1 : 0);
							smoothPosList.Add(pos3);
						}
						else
						{
							CS$<>8__locals1.map[(int)i, (int)j] = 1;
						}
					}
				}
				for (int a = 0; a < 1; a++)
				{
					for (int s = 0; s < smoothPosList.Count; s++)
					{
						ByteCoordinate pos2 = smoothPosList[s];
						int wallCount = CS$<>8__locals1.<GetAreaShape>g__CountWalls|0((int)pos2.X, (int)pos2.Y);
						bool flag4 = wallCount > 4;
						if (flag4)
						{
							CS$<>8__locals1.map[(int)pos2.X, (int)pos2.Y] = 0;
						}
						else
						{
							bool flag5 = wallCount < 4;
							if (flag5)
							{
								CS$<>8__locals1.map[(int)pos2.X, (int)pos2.Y] = 1;
							}
						}
					}
				}
				ObjectPool<List<ByteCoordinate>>.Instance.Return(smoothPosList);
				CS$<>8__locals1.island = ObjectPool<List<ByteCoordinate>>.Instance.Get();
				List<ByteCoordinate> edgeWallList = ObjectPool<List<ByteCoordinate>>.Instance.Get();
				CS$<>8__locals1.island.Clear();
				this.SpreadIsland(center, CS$<>8__locals1.island, CS$<>8__locals1.mapSize, new Predicate<ByteCoordinate>(CS$<>8__locals1.<GetAreaShape>g__CanPass|1), new byte[(int)CS$<>8__locals1.mapSize, (int)CS$<>8__locals1.mapSize]);
				for (byte k = 0; k < CS$<>8__locals1.mapSize; k += 1)
				{
					for (byte l = 0; l < CS$<>8__locals1.mapSize; l += 1)
					{
						bool flag6 = !CS$<>8__locals1.island.Contains(new ByteCoordinate(k, l));
						if (flag6)
						{
							CS$<>8__locals1.map[(int)k, (int)l] = 0;
						}
					}
				}
				CS$<>8__locals1.island.Clear();
				edgeWallList.Clear();
				for (byte m = 0; m < CS$<>8__locals1.mapSize - 1; m += 1)
				{
					bool flag7 = CS$<>8__locals1.map[0, (int)m] == 0;
					if (flag7)
					{
						edgeWallList.Add(new ByteCoordinate(0, m));
					}
					bool flag8 = CS$<>8__locals1.map[(int)(m + 1), (int)(CS$<>8__locals1.mapSize - 1)] == 0;
					if (flag8)
					{
						edgeWallList.Add(new ByteCoordinate(m + 1, CS$<>8__locals1.mapSize - 1));
					}
					bool flag9 = CS$<>8__locals1.map[(int)m, (int)(CS$<>8__locals1.mapSize - 1)] == 0;
					if (flag9)
					{
						edgeWallList.Add(new ByteCoordinate(m, CS$<>8__locals1.mapSize - 1));
					}
					bool flag10 = CS$<>8__locals1.map[(int)(CS$<>8__locals1.mapSize - 1), (int)(m + 1)] == 0;
					if (flag10)
					{
						edgeWallList.Add(new ByteCoordinate(CS$<>8__locals1.mapSize - 1, m + 1));
					}
				}
				while (edgeWallList.Count > 0)
				{
					this.SpreadIsland(edgeWallList[0], CS$<>8__locals1.island, CS$<>8__locals1.mapSize, new Predicate<ByteCoordinate>(CS$<>8__locals1.<GetAreaShape>g__IsWall|2), new byte[(int)CS$<>8__locals1.mapSize, (int)CS$<>8__locals1.mapSize]);
					edgeWallList.RemoveAll((ByteCoordinate pos) => CS$<>8__locals1.island.Contains(pos));
				}
				for (byte n = 0; n < CS$<>8__locals1.mapSize; n += 1)
				{
					for (byte j2 = 0; j2 < CS$<>8__locals1.mapSize; j2 += 1)
					{
						bool flag11 = !CS$<>8__locals1.island.Contains(new ByteCoordinate(n, j2));
						if (flag11)
						{
							CS$<>8__locals1.map[(int)n, (int)j2] = 1;
						}
					}
				}
				for (byte i2 = 0; i2 < CS$<>8__locals1.mapSize; i2 += 1)
				{
					for (byte j3 = 0; j3 < CS$<>8__locals1.mapSize; j3 += 1)
					{
						bool flag12 = CS$<>8__locals1.map[(int)i2, (int)j3] == 1 && (i2 == 0 || i2 == CS$<>8__locals1.mapSize - 1 || j3 == 0 || j3 == CS$<>8__locals1.mapSize - 1 || CS$<>8__locals1.map[(int)(i2 - 1), (int)j3] == 0 || CS$<>8__locals1.map[(int)(i2 + 1), (int)j3] == 0 || CS$<>8__locals1.map[(int)i2, (int)(j3 - 1)] == 0 || CS$<>8__locals1.map[(int)i2, (int)(j3 + 1)] == 0);
						if (flag12)
						{
							CS$<>8__locals1.map[(int)i2, (int)j3] = 2;
						}
					}
				}
				ObjectPool<List<ByteCoordinate>>.Instance.Return(CS$<>8__locals1.island);
				ObjectPool<List<ByteCoordinate>>.Instance.Return(edgeWallList);
				result = CS$<>8__locals1.map;
			}
			return result;
		}

		// Token: 0x06007963 RID: 31075 RVA: 0x00471A14 File Offset: 0x0046FC14
		private void PlaceStaticBlocks(short areaId, MapAreaItem areaConfigData, byte mapSize, MapBlockData[] areaBlocks, byte[,] availableBlockMap, byte[,] edgeClipMap, bool createTaiwuVillage, List<short> staticBlockIds, IRandomSource random)
		{
			List<short> staticBlockCore = ObjectPool<List<short>>.Instance.Get();
			staticBlockCore.Clear();
			bool isStoryStockade = areaId == 138;
			bool flag = isStoryStockade;
			if (flag)
			{
				staticBlockCore.Add(36);
			}
			else
			{
				staticBlockCore.AddRange(areaConfigData.SettlementBlockCore);
			}
			int settlementCount = staticBlockCore.Count;
			if (createTaiwuVillage)
			{
				staticBlockCore.Add(0);
			}
			bool flag2 = areaConfigData.SceneryBlockCore != null;
			if (flag2)
			{
				staticBlockCore.AddRange(areaConfigData.SceneryBlockCore);
			}
			bool flag3 = areaConfigData.BigBaseBlockCore != null;
			if (flag3)
			{
				for (int i = 0; i < areaConfigData.BigBaseBlockCore.Count; i++)
				{
					short[] bigBlock = areaConfigData.BigBaseBlockCore[i];
					int count = (bigBlock[1] == 1) ? 1 : random.Next(1, (int)bigBlock[1]);
					for (int j = 0; j < count; j++)
					{
						staticBlockCore.Add(bigBlock[0]);
					}
				}
			}
			staticBlockIds.Clear();
			bool flag4 = staticBlockCore.Contains(areaConfigData.CenterBlock);
			if (flag4)
			{
				ByteCoordinate center2 = new ByteCoordinate((mapSize - 1) / 2, (mapSize - 1) / 2);
				this.PlaceStaticBlock(areaId, areaConfigData, mapSize, areaBlocks, areaConfigData.CenterBlock, center2, availableBlockMap, random, false, false);
				staticBlockIds.Add(ByteCoordinate.CoordinateToIndex(center2, mapSize));
				staticBlockCore.Remove(areaConfigData.CenterBlock);
				settlementCount--;
			}
			bool flag5 = staticBlockCore.Count > 0;
			if (flag5)
			{
				List<ByteCoordinate> canUsePosList = ObjectPool<List<ByteCoordinate>>.Instance.Get();
				ByteCoordinate center = new ByteCoordinate((mapSize - 1) / 2, (mapSize - 1) / 2);
				int maxBlockRange = 0;
				MapBlock.Instance.Iterate(delegate(MapBlockItem b)
				{
					maxBlockRange = Math.Max(maxBlockRange, (int)(b.Range * 2));
					return true;
				});
				Predicate<ByteCoordinate> <>9__1;
				Predicate<ByteCoordinate> <>9__2;
				for (int index = 0; index < staticBlockCore.Count; index++)
				{
					MapBlockItem blockConfig = MapBlock.Instance[staticBlockCore[index]];
					this.GetStaticBlockPosRandomPool(blockConfig, mapSize, availableBlockMap, edgeClipMap, canUsePosList, true);
					List<ByteCoordinate> list = canUsePosList;
					Predicate<ByteCoordinate> match;
					if ((match = <>9__1) == null)
					{
						match = (<>9__1 = ((ByteCoordinate coord) => coord.GetManhattanDistance(center) <= maxBlockRange));
					}
					list.RemoveAll(match);
					bool flag6 = index < settlementCount && canUsePosList.Count == 0;
					if (flag6)
					{
						this.GetStaticBlockPosRandomPool(blockConfig, mapSize, availableBlockMap, edgeClipMap, canUsePosList, false);
					}
					List<ByteCoordinate> list2 = canUsePosList;
					Predicate<ByteCoordinate> match2;
					if ((match2 = <>9__2) == null)
					{
						match2 = (<>9__2 = ((ByteCoordinate coord) => coord.GetManhattanDistance(center) <= maxBlockRange));
					}
					list2.RemoveAll(match2);
					canUsePosList.RemoveAll(delegate(ByteCoordinate coord)
					{
						short blockId = ByteCoordinate.CoordinateToIndex(coord, mapSize);
						byte blockRange = this.GetBlockRange(areaId, blockConfig.TemplateId);
						int xMin = Math.Max(0, (int)(coord.X - blockRange));
						int yMin = Math.Max(0, (int)(coord.Y - blockRange));
						int xMax = Math.Min((int)(mapSize - 1), (int)(coord.X + blockConfig.Size + blockRange));
						int yMax = Math.Min((int)(mapSize - 1), (int)(coord.Y + blockConfig.Size + blockRange));
						byte k = (byte)xMin;
						while ((int)k < xMax)
						{
							byte l = (byte)yMin;
							while ((int)l < yMax)
							{
								short groupBlockId = ByteCoordinate.CoordinateToIndex(new ByteCoordinate(k, l), mapSize);
								MapBlockData groupBlock = areaBlocks[(int)groupBlockId];
								bool flag9 = groupBlock == null;
								if (!flag9)
								{
									bool flag10 = groupBlock.BelongBlockId >= 0 || groupBlock.IsCityTown();
									if (flag10)
									{
										return true;
									}
								}
								l += 1;
							}
							k += 1;
						}
						return areaBlocks[(int)blockId] != null && (areaBlocks[(int)blockId].BelongBlockId >= 0 || areaBlocks[(int)blockId].IsCityTown());
					});
					bool flag7 = index < settlementCount && canUsePosList.Count == 0;
					if (flag7)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(46, 3);
						defaultInterpolatedStringHandler.AppendLiteral("Area ");
						defaultInterpolatedStringHandler.AppendFormatted<short>(areaId);
						defaultInterpolatedStringHandler.AppendLiteral(" is too small to place the ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(index);
						defaultInterpolatedStringHandler.AppendLiteral(" static block ");
						defaultInterpolatedStringHandler.AppendFormatted<short>(blockConfig.TemplateId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					bool isTaiwuVillage = createTaiwuVillage && index == areaConfigData.SettlementBlockCore.Length;
					bool flag8 = canUsePosList.Count > 0;
					if (flag8)
					{
						ByteCoordinate byteCoordinate = (isTaiwuVillage || (areaId == 138 && blockConfig.TemplateId == 36)) ? center : canUsePosList.GetRandom(random);
						this.PlaceStaticBlock(areaId, areaConfigData, mapSize, areaBlocks, staticBlockCore[index], byteCoordinate, availableBlockMap, random, index == (createTaiwuVillage ? areaConfigData.SettlementBlockCore.Length : ((int)areaConfigData.StationLocate)), isTaiwuVillage);
						staticBlockIds.Add(ByteCoordinate.CoordinateToIndex(byteCoordinate, mapSize));
					}
				}
				ObjectPool<List<ByteCoordinate>>.Instance.Return(canUsePosList);
			}
			ObjectPool<List<short>>.Instance.Return(staticBlockCore);
		}

		// Token: 0x06007964 RID: 31076 RVA: 0x00471F0C File Offset: 0x0047010C
		private void GetStaticBlockPosRandomPool(MapBlockItem blockConfig, byte mapSize, byte[,] availableBlockMap, byte[,] edgeClipMap, List<ByteCoordinate> posList, bool calcRange = true)
		{
			posList.Clear();
			for (byte x = 0; x < mapSize; x += 1)
			{
				for (byte y = 0; y < mapSize; y += 1)
				{
					bool canUse = true;
					int minI = (int)(calcRange ? (x - blockConfig.Range) : x);
					int minJ = (int)(calcRange ? (y - blockConfig.Range) : y);
					int maxI = (int)(calcRange ? (x + blockConfig.Size + blockConfig.Range) : (x + blockConfig.Size));
					int maxJ = (int)(calcRange ? (y + blockConfig.Size + blockConfig.Range) : (y + blockConfig.Size));
					for (int i = minI; i < maxI; i++)
					{
						for (int j = minJ; j < maxJ; j++)
						{
							bool flag = i <= 0 || i >= (int)(mapSize - 1) || j <= 0 || j >= (int)(mapSize - 1) || availableBlockMap[i, j] == 1 || edgeClipMap[i, j] != 1;
							if (flag)
							{
								canUse = false;
								break;
							}
						}
						bool flag2 = !canUse;
						if (flag2)
						{
							break;
						}
					}
					bool flag3 = canUse;
					if (flag3)
					{
						posList.Add(new ByteCoordinate(x, y));
					}
				}
			}
		}

		// Token: 0x06007965 RID: 31077 RVA: 0x00472054 File Offset: 0x00470254
		private byte GetBlockRange(short areaId, short blockTemplateId)
		{
			MapBlockItem blockConfig = MapBlock.Instance[blockTemplateId];
			bool flag = areaId == 138 && blockConfig.TemplateId == 36;
			byte result;
			if (flag)
			{
				result = 2;
			}
			else
			{
				result = blockConfig.Range;
			}
			return result;
		}

		// Token: 0x06007966 RID: 31078 RVA: 0x00472095 File Offset: 0x00470295
		private byte GetBlockRange(MapBlockData blockData)
		{
			return this.GetBlockRange(blockData.AreaId, blockData.TemplateId);
		}

		// Token: 0x06007967 RID: 31079 RVA: 0x004720AC File Offset: 0x004702AC
		private void PlaceStaticBlock(short areaId, MapAreaItem areaConfigData, byte areaSize, MapBlockData[] areaBlocks, short blockTemplateId, ByteCoordinate byteCoordinate, byte[,] availableBlockMap, IRandomSource random, bool hasStation = false, bool createTaiwuVillage = false)
		{
			short blockId = ByteCoordinate.CoordinateToIndex(byteCoordinate, areaSize);
			MapBlockData staticBlock = new MapBlockData(areaId, blockId, blockTemplateId);
			MapBlockItem blockConfig = MapBlock.Instance[blockTemplateId];
			List<ByteCoordinate> staticBlockLocationList = ObjectPool<List<ByteCoordinate>>.Instance.Get();
			areaBlocks[(int)blockId] = staticBlock;
			staticBlockLocationList.Clear();
			staticBlockLocationList.Add(byteCoordinate);
			availableBlockMap[(int)byteCoordinate.X, (int)byteCoordinate.Y] = 1;
			bool flag = blockConfig.Size > 1;
			if (flag)
			{
				for (byte i = 0; i < blockConfig.Size; i += 1)
				{
					for (byte j = 0; j < blockConfig.Size; j += 1)
					{
						bool flag2 = i + j == 0 || byteCoordinate.X + i >= areaSize || byteCoordinate.Y + j >= areaSize;
						if (!flag2)
						{
							ByteCoordinate childByteCoordinate = new ByteCoordinate(byteCoordinate.X + i, byteCoordinate.Y + j);
							short childBlockId = ByteCoordinate.CoordinateToIndex(childByteCoordinate, areaSize);
							availableBlockMap[(int)childByteCoordinate.X, (int)childByteCoordinate.Y] = 1;
							areaBlocks[(int)childBlockId] = new MapBlockData(areaId, childBlockId, -1);
							areaBlocks[(int)childBlockId].SetToSizeBlock(staticBlock);
							staticBlockLocationList.Add(childByteCoordinate);
						}
					}
				}
			}
			byte blockRange = this.GetBlockRange(staticBlock);
			bool flag3 = blockRange > 0;
			if (flag3)
			{
				int xMin = Math.Max(0, (int)(byteCoordinate.X - blockRange));
				int yMin = Math.Max(0, (int)(byteCoordinate.Y - blockRange));
				int xMax = Math.Min((int)(areaSize - 1), (int)(byteCoordinate.X + blockConfig.Size + blockRange));
				int yMax = Math.Min((int)(areaSize - 1), (int)(byteCoordinate.Y + blockConfig.Size + blockRange));
				List<short> rangeBlockList = ObjectPool<List<short>>.Instance.Get();
				List<short[]> blockTemplateIdList = ObjectPool<List<short[]>>.Instance.Get();
				rangeBlockList.Clear();
				byte k = (byte)xMin;
				while ((int)k < xMax)
				{
					byte l = (byte)yMin;
					while ((int)l < yMax)
					{
						ByteCoordinate rangeByteCoordinate = new ByteCoordinate(k, l);
						bool flag4 = rangeByteCoordinate.GetMinManhattanDistance(staticBlockLocationList) > (int)blockRange || availableBlockMap[(int)k, (int)l] == 1;
						if (!flag4)
						{
							availableBlockMap[(int)k, (int)l] = 1;
							rangeBlockList.Add(ByteCoordinate.CoordinateToIndex(rangeByteCoordinate, areaSize));
						}
						l += 1;
					}
					k += 1;
				}
				blockTemplateIdList.Clear();
				RandomUtils.GenerateRandomWeightCellList(random, areaConfigData.DevelopedBlockCore, rangeBlockList.Count, ref blockTemplateIdList, 1);
				for (int m = 0; m < rangeBlockList.Count; m++)
				{
					short rangeBlockId = rangeBlockList[m];
					areaBlocks[(int)rangeBlockId] = new MapBlockData(areaId, rangeBlockId, blockTemplateIdList[m][0]);
					areaBlocks[(int)rangeBlockId].BelongBlockId = blockId;
				}
				if (hasStation)
				{
					bool flag5 = rangeBlockList.Count <= 0;
					if (flag5)
					{
						StringBuilder sb = new StringBuilder();
						AdaptableLog.Info("The availableBlockMap is:");
						for (int y = 0; y < (int)areaSize; y++)
						{
							sb.Clear();
							for (int x = 0; x < (int)areaSize; x++)
							{
								bool flag6 = x != 0;
								if (flag6)
								{
									sb.Append(" ");
								}
								StringBuilder stringBuilder = sb;
								StringBuilder stringBuilder2 = stringBuilder;
								StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(0, 1, stringBuilder);
								appendInterpolatedStringHandler.AppendFormatted<byte>(availableBlockMap[x, y]);
								stringBuilder2.Append(ref appendInterpolatedStringHandler);
							}
							AdaptableLog.Info(sb.ToString());
						}
						AdaptableLog.Info("The BelongBlockId is:");
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
						for (int y2 = 0; y2 < (int)areaSize; y2++)
						{
							sb.Clear();
							for (int x2 = 0; x2 < (int)areaSize; x2++)
							{
								short index = ByteCoordinate.CoordinateToIndex(new ByteCoordinate((byte)x2, (byte)y2), areaSize);
								int belongBlockId = (int)((areaBlocks[(int)index] != null) ? areaBlocks[(int)index].BelongBlockId : -1);
								bool flag7 = x2 != 0;
								if (flag7)
								{
									sb.Append(" ");
								}
								StringBuilder stringBuilder3 = sb;
								string value;
								if (belongBlockId < 0)
								{
									value = "XXX";
								}
								else
								{
									defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
									defaultInterpolatedStringHandler.AppendFormatted<int>(belongBlockId, "D3");
									value = defaultInterpolatedStringHandler.ToStringAndClear();
								}
								stringBuilder3.Append(value);
							}
							AdaptableLog.Info(sb.ToString());
						}
						AdaptableLog.Info("The TemplateId is:");
						for (int y3 = 0; y3 < (int)areaSize; y3++)
						{
							sb.Clear();
							for (int x3 = 0; x3 < (int)areaSize; x3++)
							{
								short index2 = ByteCoordinate.CoordinateToIndex(new ByteCoordinate((byte)x3, (byte)y3), areaSize);
								int templateId = (int)((areaBlocks[(int)index2] != null) ? areaBlocks[(int)index2].TemplateId : -1);
								bool flag8 = x3 != 0;
								if (flag8)
								{
									sb.Append(" ");
								}
								StringBuilder stringBuilder4 = sb;
								string value2;
								if (templateId < 0)
								{
									value2 = "XXX";
								}
								else
								{
									defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
									defaultInterpolatedStringHandler.AppendFormatted<int>(templateId, "D3");
									value2 = defaultInterpolatedStringHandler.ToStringAndClear();
								}
								stringBuilder4.Append(value2);
							}
							AdaptableLog.Info(sb.ToString());
						}
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(94, 4);
						defaultInterpolatedStringHandler.AppendLiteral("rangeBlockList.Count must be > 0 when creating station | (templateId: ");
						defaultInterpolatedStringHandler.AppendFormatted<short>(blockConfig.TemplateId);
						defaultInterpolatedStringHandler.AppendLiteral(", areaSize: ");
						defaultInterpolatedStringHandler.AppendFormatted<byte>(areaSize);
						defaultInterpolatedStringHandler.AppendLiteral(", coord: ");
						defaultInterpolatedStringHandler.AppendFormatted<byte>(byteCoordinate.X);
						defaultInterpolatedStringHandler.AppendLiteral(", ");
						defaultInterpolatedStringHandler.AppendFormatted<byte>(byteCoordinate.Y);
						defaultInterpolatedStringHandler.AppendLiteral(")");
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					List<short> majorStations = ObjectPool<List<short>>.Instance.Get();
					majorStations.Clear();
					majorStations.AddRange(rangeBlockList.Where(delegate(short bId)
					{
						ByteCoordinate coord = ByteCoordinate.IndexToCoordinate(bId, areaSize);
						bool flag11 = staticBlockLocationList.Contains(coord);
						bool result;
						if (flag11)
						{
							result = false;
						}
						else
						{
							foreach (ByteCoordinate groupCoord in staticBlockLocationList)
							{
								bool flag12 = groupCoord.GetManhattanDistance(coord) != 2;
								if (flag12)
								{
									return false;
								}
							}
							result = true;
						}
						return result;
					}));
					MapAreaData mapAreaData = this._areas[(int)areaId];
					bool flag9 = majorStations.Count > 0;
					MapBlockData stationBlock;
					if (flag9)
					{
						stationBlock = areaBlocks[(int)majorStations.GetRandom(random)];
					}
					else
					{
						stationBlock = areaBlocks[(int)rangeBlockList.OrderByDescending(delegate(short bId)
						{
							ByteCoordinate coord = ByteCoordinate.IndexToCoordinate(bId, areaSize);
							return staticBlockLocationList.Contains(coord) ? 0 : staticBlockLocationList.Max((ByteCoordinate groupCoord) => groupCoord.GetManhattanDistance(coord));
						}).First<short>()];
					}
					bool flag10 = areaId == 138 || createTaiwuVillage;
					if (flag10)
					{
						stationBlock.ChangeTemplateId(38, false);
					}
					else
					{
						stationBlock.ChangeTemplateId(37, false);
					}
					mapAreaData.StationBlockId = stationBlock.BlockId;
					ObjectPool<List<short>>.Instance.Return(majorStations);
				}
				ObjectPool<List<short>>.Instance.Return(rangeBlockList);
				ObjectPool<List<short[]>>.Instance.Return(blockTemplateIdList);
			}
			ObjectPool<List<ByteCoordinate>>.Instance.Return(staticBlockLocationList);
		}

		// Token: 0x06007968 RID: 31080 RVA: 0x004727D4 File Offset: 0x004709D4
		private void PlaceOtherBlocks(short areaId, MapAreaItem areaConfigData, byte areaSize, MapBlockData[] areaBlocks, byte[,] availableBlockMap, List<short> staticBlockIds, int settlementCount, IRandomSource random)
		{
			List<MapBlockData> staticBlockList = ObjectPool<List<MapBlockData>>.Instance.Get();
			List<short> normalBlockIdList = ObjectPool<List<short>>.Instance.Get();
			List<short[]> normalTemplateIdList = ObjectPool<List<short[]>>.Instance.Get();
			List<short> wildBlockIdList = ObjectPool<List<short>>.Instance.Get();
			List<short[]> wildTemplateIdList = ObjectPool<List<short[]>>.Instance.Get();
			staticBlockList.Clear();
			for (int i = 0; i < settlementCount; i++)
			{
				staticBlockList.Add(areaBlocks[(int)staticBlockIds[i]]);
			}
			normalBlockIdList.Clear();
			wildBlockIdList.Clear();
			for (byte x = 0; x < areaSize; x += 1)
			{
				for (byte y = 0; y < areaSize; y += 1)
				{
					bool flag = availableBlockMap[(int)x, (int)y] == 0;
					if (flag)
					{
						short blockId = ByteCoordinate.CoordinateToIndex(new ByteCoordinate(x, y), areaSize);
						availableBlockMap[(int)x, (int)y] = 1;
						bool flag2 = this.InNormalBlockRange(blockId, staticBlockList, areaSize);
						if (flag2)
						{
							normalBlockIdList.Add(blockId);
						}
						else
						{
							wildBlockIdList.Add(blockId);
						}
					}
				}
			}
			normalTemplateIdList.Clear();
			RandomUtils.GenerateRandomWeightCellList(random, areaConfigData.NormalBlockCore, normalBlockIdList.Count, ref normalTemplateIdList, 1);
			for (int j = 0; j < normalBlockIdList.Count; j++)
			{
				short blockId2 = normalBlockIdList[j];
				areaBlocks[(int)blockId2] = new MapBlockData(areaId, blockId2, normalTemplateIdList[j][0]);
			}
			wildTemplateIdList.Clear();
			RandomUtils.GenerateRandomWeightCellList(random, areaConfigData.WildBlockCore, wildBlockIdList.Count, ref wildTemplateIdList, 1);
			for (int k = 0; k < wildBlockIdList.Count; k++)
			{
				short blockId3 = wildBlockIdList[k];
				areaBlocks[(int)blockId3] = new MapBlockData(areaId, blockId3, wildTemplateIdList[k][0]);
			}
			int blockId4 = 0;
			int len = areaBlocks.Length;
			while (blockId4 < len)
			{
				MapBlockData block = areaBlocks[blockId4];
				bool flag3 = block.TemplateId == 124;
				if (flag3)
				{
					int maxSteps = 1;
					ByteCoordinate blockPos = ByteCoordinate.IndexToCoordinate(block.BlockId, areaSize);
					int blockSize = 1;
					bool banAbyss = false;
					byte x2 = (byte)Math.Max((int)blockPos.X - maxSteps, 0);
					while ((int)x2 < Math.Min((int)blockPos.X + blockSize + maxSteps, (int)areaSize))
					{
						byte y2 = (byte)Math.Max((int)blockPos.Y - maxSteps, 0);
						while ((int)y2 < Math.Min((int)blockPos.Y + blockSize + maxSteps, (int)areaSize))
						{
							MapBlockData neighborBlock = areaBlocks[(int)ByteCoordinate.CoordinateToIndex(new ByteCoordinate(x2, y2), areaSize)];
							bool flag4 = neighborBlock.BlockId != block.BlockId && (neighborBlock.TemplateId == 124 || neighborBlock.TemplateId == 126);
							if (flag4)
							{
								banAbyss = true;
								break;
							}
							y2 += 1;
						}
						bool flag5 = banAbyss;
						if (flag5)
						{
							break;
						}
						x2 += 1;
					}
					bool flag6 = banAbyss;
					if (flag6)
					{
						areaBlocks[blockId4] = new MapBlockData(block.AreaId, block.BlockId, MapBlock.Instance.First((MapBlockItem b) => b.Type == EMapBlockType.Normal && b.Size == 1 && b.TemplateId != 124).TemplateId);
					}
				}
				blockId4++;
			}
			ObjectPool<List<MapBlockData>>.Instance.Return(staticBlockList);
			ObjectPool<List<short>>.Instance.Return(normalBlockIdList);
			ObjectPool<List<short[]>>.Instance.Return(normalTemplateIdList);
			ObjectPool<List<short>>.Instance.Return(wildBlockIdList);
			ObjectPool<List<short[]>>.Instance.Return(wildTemplateIdList);
		}

		// Token: 0x06007969 RID: 31081 RVA: 0x00472B58 File Offset: 0x00470D58
		private bool InNormalBlockRange(short blockId, List<MapBlockData> staticBlocks, byte mapSize)
		{
			ByteCoordinate blockPos = ByteCoordinate.IndexToCoordinate(blockId, mapSize);
			for (int i = 0; i < staticBlocks.Count; i++)
			{
				MapBlockData staticBlock = staticBlocks[i];
				byte range = staticBlock.GetConfig().Range;
				ByteCoordinate staticBlockPos = ByteCoordinate.IndexToCoordinate(staticBlock.BlockId, mapSize);
				int distance = MathUtils.GetManhattanDistance((int)(staticBlockPos.X - range) - GlobalConfig.Instance.MapNormalBlockRange, (int)(staticBlockPos.Y - range) - GlobalConfig.Instance.MapNormalBlockRange, (int)blockPos.X, (int)blockPos.Y, (int)staticBlock.GetConfig().Size + ((int)range + GlobalConfig.Instance.MapNormalBlockRange) * 2);
				bool flag = distance <= 0;
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600796A RID: 31082 RVA: 0x00472C20 File Offset: 0x00470E20
		private ByteCoordinate[] GetSniffRectList(ByteCoordinate byteCoordinate, int range, byte mapSize)
		{
			List<ByteCoordinate> list = ObjectPool<List<ByteCoordinate>>.Instance.Get();
			list.Clear();
			byte xMin = (byte)Math.Max((int)byteCoordinate.X - range, 0);
			byte yMin = (byte)Math.Max((int)byteCoordinate.Y - range, 0);
			byte xMax = (byte)Math.Min((int)byteCoordinate.X + range, (int)(mapSize - 1));
			byte yMax = (byte)Math.Min((int)byteCoordinate.Y + range, (int)(mapSize - 1));
			for (byte x = xMin; x < xMax; x += 1)
			{
				list.Add(new ByteCoordinate(x, yMin));
				list.Add(new ByteCoordinate(x, yMax));
			}
			for (byte y = yMin + 1; y < yMax; y += 1)
			{
				list.Add(new ByteCoordinate(xMin, y));
				list.Add(new ByteCoordinate(xMax, y));
			}
			ByteCoordinate[] result = list.ToArray();
			ObjectPool<List<ByteCoordinate>>.Instance.Return(list);
			return result;
		}

		// Token: 0x0600796B RID: 31083 RVA: 0x00472D14 File Offset: 0x00470F14
		private void FixSeriesBlocks(MapAreaItem areaConfigData, byte mapSize, MapBlockData[] areaBlocks, Dictionary<int, List<short>> blockTypeDict, IRandomSource random)
		{
			MapDomain.<>c__DisplayClass243_0 CS$<>8__locals1 = new MapDomain.<>c__DisplayClass243_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.mapSize = mapSize;
			CS$<>8__locals1.areaBlocks = areaBlocks;
			CS$<>8__locals1.blockTypeDict = blockTypeDict;
			CS$<>8__locals1.random = random;
			bool flag = areaConfigData.SeriesBlockCore == null;
			if (!flag)
			{
				foreach (short[] array in areaConfigData.SeriesBlockCore)
				{
					int blockSubType = (int)array[0];
					List<MapBlockData> blocks = CS$<>8__locals1.areaBlocks.FindAll((MapBlockData block) => block.CanChangeBlockType() && block.BlockSubType == (EMapBlockSubType)blockSubType);
					int seriesGroupCount = (int)array[1];
					int seriesBlockCount = CS$<>8__locals1.random.Next((int)array[2], (int)array[3]);
					for (int i = 0; i < seriesGroupCount; i++)
					{
						MapBlockData block2 = blocks.GetRandom(CS$<>8__locals1.random);
						bool flag2 = block2 == null;
						if (flag2)
						{
							break;
						}
						CS$<>8__locals1.<FixSeriesBlocks>g__BlockInfect|0(block2, seriesBlockCount);
						blocks.Remove(block2);
						seriesBlockCount = CS$<>8__locals1.random.Next((int)array[2], (int)array[3]) - 1;
					}
				}
			}
		}

		// Token: 0x0600796C RID: 31084 RVA: 0x00472E4C File Offset: 0x0047104C
		private void FixEncircleBlocks(MapAreaItem areaConfigData, byte mapSize, MapBlockData[] areaBlocks, Dictionary<int, List<short>> blockTypeDict, IRandomSource random)
		{
			MapDomain.<>c__DisplayClass244_0 CS$<>8__locals1 = new MapDomain.<>c__DisplayClass244_0();
			CS$<>8__locals1.random = random;
			CS$<>8__locals1.areaBlocks = areaBlocks;
			CS$<>8__locals1.mapSize = mapSize;
			bool flag = areaConfigData.EncircleBlockCore == null;
			if (!flag)
			{
				List<ByteCoordinate> centerPosList = ObjectPool<List<ByteCoordinate>>.Instance.Get();
				List<ByteCoordinate> edgePosList = ObjectPool<List<ByteCoordinate>>.Instance.Get();
				List<MapBlockData> circleBlocks = ObjectPool<List<MapBlockData>>.Instance.Get();
				for (int index = 0; index < areaConfigData.EncircleBlockCore.Count; index++)
				{
					short[] encircleData = areaConfigData.EncircleBlockCore[index];
					List<short> targetTypeIdList;
					bool flag2 = !blockTypeDict.TryGetValue((int)encircleData[0], out targetTypeIdList);
					if (!flag2)
					{
						targetTypeIdList.RemoveAll((short templateId) => MapBlock.Instance[templateId].Size != 1);
						bool flag3 = targetTypeIdList.Count == 0;
						if (!flag3)
						{
							short centerTemplateId = encircleData[1];
							byte[,] circleMap = this.GetAreaShape(CS$<>8__locals1.random, (byte)(encircleData[4] * 2 + 1), (byte)encircleData[3], true);
							bool flag4 = centerTemplateId != -1;
							ByteCoordinate circleCenter;
							if (flag4)
							{
								circleCenter = ByteCoordinate.IndexToCoordinate(CS$<>8__locals1.areaBlocks.FindAll((MapBlockData block) => block.TemplateId == centerTemplateId).GetRandom(CS$<>8__locals1.random).BlockId, CS$<>8__locals1.mapSize);
							}
							else
							{
								circleCenter = new ByteCoordinate((byte)CS$<>8__locals1.random.Next((int)(CS$<>8__locals1.mapSize / 4), (int)(CS$<>8__locals1.mapSize * 3 / 4)), (byte)CS$<>8__locals1.random.Next((int)(CS$<>8__locals1.mapSize / 4), (int)(CS$<>8__locals1.mapSize * 3 / 4)));
							}
							centerPosList.Clear();
							edgePosList.Clear();
							circleBlocks.Clear();
							byte x = 0;
							while ((int)x < circleMap.GetLength(0))
							{
								byte y = 0;
								while ((int)y < circleMap.GetLength(1))
								{
									byte data = circleMap[(int)x, (int)y];
									ByteCoordinate pos5 = new ByteCoordinate(x, y);
									bool flag5 = data == 1;
									if (flag5)
									{
										centerPosList.Add(pos5);
									}
									else
									{
										bool flag6 = data == 2;
										if (flag6)
										{
											edgePosList.Add(pos5);
										}
									}
									y += 1;
								}
								x += 1;
							}
							bool flag7 = edgePosList.Count > 0;
							if (flag7)
							{
								int[] offset = new int[2];
								int[] pos2 = new int[2];
								for (int i = 0; i < edgePosList.Count; i++)
								{
									ByteCoordinate edgePos = edgePosList[i];
									offset[0] = (int)(edgePos.X - circleCenter.X);
									offset[1] = (int)(edgePos.Y - circleCenter.Y);
									pos2[0] = (int)circleCenter.X + offset[0];
									pos2[1] = (int)circleCenter.Y + offset[1];
									bool flag8 = pos2[0] < 0 || pos2[0] >= (int)CS$<>8__locals1.mapSize || pos2[1] < 0 || pos2[1] >= (int)CS$<>8__locals1.mapSize;
									if (!flag8)
									{
										ByteCoordinate realPosition = new ByteCoordinate((byte)pos2[0], (byte)pos2[1]);
										short blockIndex = ByteCoordinate.CoordinateToIndex(realPosition, CS$<>8__locals1.mapSize);
										MapBlockData blockData = null;
										bool flag9 = CS$<>8__locals1.areaBlocks.CheckIndex((int)blockIndex);
										if (flag9)
										{
											blockData = CS$<>8__locals1.areaBlocks[(int)blockIndex];
										}
										bool flag10 = blockData != null && blockData.CanChangeBlockType();
										if (flag10)
										{
											circleBlocks.Add(blockData);
										}
									}
								}
							}
							else
							{
								Logger logger = MapDomain.Logger;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 2);
								defaultInterpolatedStringHandler.AppendFormatted(areaConfigData.Name);
								defaultInterpolatedStringHandler.AppendLiteral(":encircleType ");
								defaultInterpolatedStringHandler.AppendFormatted<short>(encircleData[0]);
								defaultInterpolatedStringHandler.AppendLiteral(" has no edgePosList");
								logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
							}
							bool flag11 = circleBlocks.Count > 0;
							if (flag11)
							{
								for (int j = 0; j < (int)encircleData[2]; j++)
								{
									bool flag12 = circleBlocks.Count <= 0;
									if (flag12)
									{
										break;
									}
									circleBlocks.RemoveAt(CS$<>8__locals1.random.Next(circleBlocks.Count));
								}
								circleBlocks.ForEach(delegate(MapBlockData block)
								{
									block.ChangeTemplateId(targetTypeIdList.GetRandom(CS$<>8__locals1.random), true);
								});
							}
							else
							{
								Logger logger2 = MapDomain.Logger;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(68, 3);
								defaultInterpolatedStringHandler.AppendLiteral("Error:areaId ");
								defaultInterpolatedStringHandler.AppendFormatted<short>(areaConfigData.TemplateId);
								defaultInterpolatedStringHandler.AppendLiteral(" failed to set encircle data of type ");
								defaultInterpolatedStringHandler.AppendFormatted<short>(encircleData[0]);
								defaultInterpolatedStringHandler.AppendLiteral(" circle center is ");
								defaultInterpolatedStringHandler.AppendFormatted<ByteCoordinate>(circleCenter);
								logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
							}
						}
					}
				}
				List<List<ByteCoordinate>> areaIslands = ObjectPool<List<List<ByteCoordinate>>>.Instance.Get();
				List<ByteCoordinate> notLinkedPosList = ObjectPool<List<ByteCoordinate>>.Instance.Get();
				areaIslands.Clear();
				notLinkedPosList.Clear();
				short blockId = 0;
				while ((int)blockId < CS$<>8__locals1.areaBlocks.Length)
				{
					bool flag13 = CS$<>8__locals1.areaBlocks[(int)blockId].IsPassable();
					if (flag13)
					{
						notLinkedPosList.Add(ByteCoordinate.IndexToCoordinate(blockId, CS$<>8__locals1.mapSize));
					}
					blockId += 1;
				}
				while (notLinkedPosList.Count > 0)
				{
					List<ByteCoordinate> island = ObjectPool<List<ByteCoordinate>>.Instance.Get();
					island.Clear();
					areaIslands.Add(island);
					this.SpreadIsland(notLinkedPosList[0], island, CS$<>8__locals1.mapSize, new Predicate<ByteCoordinate>(CS$<>8__locals1.<FixEncircleBlocks>g__CanPass|0), new byte[(int)CS$<>8__locals1.mapSize, (int)CS$<>8__locals1.mapSize]);
					notLinkedPosList.RemoveAll((ByteCoordinate pos) => island.Contains(pos));
				}
				bool flag14 = areaIslands.Count > 1;
				if (flag14)
				{
					List<short> baseBlockRandomPool = ObjectPool<List<short>>.Instance.Get();
					baseBlockRandomPool.Clear();
					for (int k = 0; k < areaConfigData.WildBlockCore.Count; k++)
					{
						short blockTemplateId = areaConfigData.WildBlockCore[k][0];
						bool flag15 = blockTemplateId != 124;
						if (flag15)
						{
							baseBlockRandomPool.Add(blockTemplateId);
						}
					}
					areaIslands.Sort(delegate(List<ByteCoordinate> left, List<ByteCoordinate> right)
					{
						bool flag21 = left.Count != right.Count;
						int result;
						if (flag21)
						{
							result = right.Count - left.Count;
						}
						else
						{
							result = left.GetHashCode() - right.GetHashCode();
						}
						return result;
					});
					List<ByteCoordinate> mainIsland = areaIslands[0];
					for (int l = 1; l < areaIslands.Count; l++)
					{
						List<ByteCoordinate> island2 = areaIslands[l];
						ByteCoordinate mainIslandBlockPos = mainIsland[CS$<>8__locals1.random.Next(mainIsland.Count)];
						MapBlockData linkBlock = null;
						int minDist = int.MaxValue;
						foreach (ByteCoordinate pos3 in island2)
						{
							int distance = pos3.GetManhattanDistance(mainIslandBlockPos);
							bool flag16 = distance < minDist;
							if (flag16)
							{
								linkBlock = CS$<>8__locals1.areaBlocks[(int)ByteCoordinate.CoordinateToIndex(pos3, CS$<>8__locals1.mapSize)];
								minDist = distance;
							}
						}
						ByteCoordinate linkBlockPos = ByteCoordinate.IndexToCoordinate(linkBlock.BlockId, CS$<>8__locals1.mapSize);
						MapBlockData mainIslandLinkBlock = null;
						minDist = int.MaxValue;
						foreach (ByteCoordinate pos4 in mainIsland)
						{
							int distance2 = pos4.GetManhattanDistance(linkBlockPos);
							bool flag17 = distance2 < minDist;
							if (flag17)
							{
								mainIslandLinkBlock = CS$<>8__locals1.areaBlocks[(int)ByteCoordinate.CoordinateToIndex(pos4, CS$<>8__locals1.mapSize)];
								minDist = distance2;
							}
						}
						ByteCoordinate mainIslandLinkBlockPos = ByteCoordinate.IndexToCoordinate(mainIslandLinkBlock.BlockId, CS$<>8__locals1.mapSize);
						int xAddValue = Math.Sign((int)(mainIslandLinkBlockPos.X - linkBlockPos.X));
						int yAddValue = Math.Sign((int)(mainIslandLinkBlockPos.Y - linkBlockPos.Y));
						while (linkBlockPos != mainIslandLinkBlockPos)
						{
							bool flag18 = linkBlockPos.X != mainIslandLinkBlockPos.X && (linkBlockPos.Y == mainIslandLinkBlockPos.Y || CS$<>8__locals1.random.CheckPercentProb(50));
							if (flag18)
							{
								linkBlockPos.X = (byte)((int)linkBlockPos.X + xAddValue);
							}
							else
							{
								linkBlockPos.Y = (byte)((int)linkBlockPos.Y + yAddValue);
							}
							short blockIndex2 = ByteCoordinate.CoordinateToIndex(linkBlockPos, CS$<>8__locals1.mapSize);
							bool flag19 = !CS$<>8__locals1.areaBlocks[(int)blockIndex2].IsPassable();
							if (flag19)
							{
								CS$<>8__locals1.areaBlocks[(int)blockIndex2].ChangeTemplateId(baseBlockRandomPool.GetRandom(CS$<>8__locals1.random), false);
							}
						}
						bool flag20 = mainIslandLinkBlock.TemplateId == 124;
						if (flag20)
						{
							mainIslandLinkBlock.ChangeTemplateId(baseBlockRandomPool.GetRandom(CS$<>8__locals1.random), false);
						}
					}
					ObjectPool<List<short>>.Instance.Return(baseBlockRandomPool);
				}
				ObjectPool<List<ByteCoordinate>>.Instance.Return(centerPosList);
				ObjectPool<List<ByteCoordinate>>.Instance.Return(edgePosList);
				ObjectPool<List<MapBlockData>>.Instance.Return(circleBlocks);
				areaIslands.ForEach(delegate(List<ByteCoordinate> island)
				{
					ObjectPool<List<ByteCoordinate>>.Instance.Return(island);
				});
				ObjectPool<List<List<ByteCoordinate>>>.Instance.Return(areaIslands);
				ObjectPool<List<ByteCoordinate>>.Instance.Return(notLinkedPosList);
			}
		}

		// Token: 0x0600796D RID: 31085 RVA: 0x004737F4 File Offset: 0x004719F4
		private void SpreadIsland(ByteCoordinate pos, List<ByteCoordinate> island, byte mapSize, Predicate<ByteCoordinate> canPass, byte[,] reachMap)
		{
			bool flag = reachMap[(int)pos.X, (int)pos.Y] == 1;
			if (!flag)
			{
				reachMap[(int)pos.X, (int)pos.Y] = 1;
				bool flag2 = !canPass(pos);
				if (!flag2)
				{
					island.Add(pos);
					bool flag3 = pos.X > 0;
					if (flag3)
					{
						this.SpreadIsland(new ByteCoordinate(pos.X - 1, pos.Y), island, mapSize, canPass, reachMap);
					}
					bool flag4 = pos.X < mapSize - 1;
					if (flag4)
					{
						this.SpreadIsland(new ByteCoordinate(pos.X + 1, pos.Y), island, mapSize, canPass, reachMap);
					}
					bool flag5 = pos.Y > 0;
					if (flag5)
					{
						this.SpreadIsland(new ByteCoordinate(pos.X, pos.Y - 1), island, mapSize, canPass, reachMap);
					}
					bool flag6 = pos.Y < mapSize - 1;
					if (flag6)
					{
						this.SpreadIsland(new ByteCoordinate(pos.X, pos.Y + 1), island, mapSize, canPass, reachMap);
					}
				}
			}
		}

		// Token: 0x0600796E RID: 31086 RVA: 0x00473914 File Offset: 0x00471B14
		private void InitAreaTravelRoute(DataContext context)
		{
			MapDomain.<>c__DisplayClass246_0 CS$<>8__locals1 = new MapDomain.<>c__DisplayClass246_0();
			CS$<>8__locals1.<>4__this = this;
			this.ClearTravelRouteDict(context);
			for (short areaId5 = 0; areaId5 < 135; areaId5 += 1)
			{
				MapAreaData area = this._areas[(int)areaId5];
				MapAreaItem areaConfig = area.GetConfig();
				AreaTravelRoute[] routeList = areaConfig.NeighborAreas;
				foreach (AreaTravelRoute routeConfig in routeList)
				{
					short neighborAreaId = this.GetAreaIdByAreaTemplateId(routeConfig.DestAreaId);
					TravelRouteKey key = new TravelRouteKey(areaId5, neighborAreaId);
					TravelRoute route = new TravelRoute();
					area.NeighborAreas.Add(neighborAreaId);
					this._areas[(int)neighborAreaId].NeighborAreas.Add(areaId5);
					route.PosList.AddRange(routeConfig.MapPosList);
					route.AreaList.Add(areaId5);
					route.AreaList.Add(neighborAreaId);
					route.CostList.Add(routeConfig.CostDays);
					bool flag = areaId5 > neighborAreaId;
					if (flag)
					{
						key.Reverse();
						route.PosList.Reverse();
						route.AreaList.Reverse();
					}
					this.AddElement_TravelRouteDict(key, route, context);
				}
			}
			AStarMapForTravel aStarMap = new AStarMapForTravel();
			List<short> path = ObjectPool<List<short>>.Instance.Get();
			aStarMap.InitMap(new Func<short, short, short>(CS$<>8__locals1.<InitAreaTravelRoute>g__GetTravelCost|0));
			for (short fromArea = 0; fromArea < 134; fromArea += 1)
			{
				for (short toArea = fromArea + 1; toArea < 135; toArea += 1)
				{
					TravelRouteKey key2 = new TravelRouteKey(fromArea, toArea);
					bool flag2 = this._travelRouteDict.ContainsKey(key2);
					if (!flag2)
					{
						TravelRoute route2 = new TravelRoute();
						path.Clear();
						aStarMap.FindWay(fromArea, toArea, ref path);
						route2.AreaList.AddRange(path);
						for (int i = 0; i < path.Count - 1; i++)
						{
							short subRouteFromArea = path[i];
							short subRouteToArea = path[i + 1];
							bool reverse = subRouteFromArea > subRouteToArea;
							TravelRouteKey subRouteKey = new TravelRouteKey(subRouteFromArea, subRouteToArea);
							bool flag3 = reverse;
							if (flag3)
							{
								subRouteKey.Reverse();
							}
							TravelRoute subRoute = this._travelRouteDict[subRouteKey];
							bool flag4 = !reverse;
							if (flag4)
							{
								route2.PosList.AddRange(subRoute.PosList);
								for (int j = 0; j < subRoute.CostList.Count; j++)
								{
									route2.CostList.Add(subRoute.CostList[j]);
								}
							}
							else
							{
								for (int k = subRoute.PosList.Count - 1; k >= 0; k--)
								{
									route2.PosList.Add(subRoute.PosList[k]);
								}
								for (int l = subRoute.CostList.Count - 1; l >= 0; l--)
								{
									route2.CostList.Add(subRoute.CostList[l]);
								}
							}
							bool flag5 = i < path.Count - 2;
							if (flag5)
							{
								sbyte[] toAreaPos = this._areas[(int)subRouteToArea].GetConfig().WorldMapPos;
								route2.PosList.Add(new ByteCoordinate((byte)toAreaPos[0], (byte)toAreaPos[1]));
							}
						}
						this.AddElement_TravelRouteDict(key2, route2, context);
					}
				}
			}
			CS$<>8__locals1.areaIdInBornState = ObjectPool<List<short>>.Instance.Get();
			short firstNormalAreaId = (short)((DomainManager.World.GetTaiwuVillageStateTemplateId() - 1) * 3);
			short firstBrokenAreaId = (short)(45 + (DomainManager.World.GetTaiwuVillageStateTemplateId() - 1) * 6);
			CS$<>8__locals1.areaIdInBornState.Clear();
			for (int m = 0; m < 3; m++)
			{
				CS$<>8__locals1.areaIdInBornState.Add((short)((int)firstNormalAreaId + m));
			}
			for (int n = 0; n < 6; n++)
			{
				CS$<>8__locals1.areaIdInBornState.Add((short)((int)firstBrokenAreaId + n));
			}
			aStarMap.InitMap(new Func<short, short, short>(CS$<>8__locals1.<InitAreaTravelRoute>g__GetTravelCostInState|1));
			for (int fromIndex = 0; fromIndex < CS$<>8__locals1.areaIdInBornState.Count - 1; fromIndex++)
			{
				short fromArea2 = CS$<>8__locals1.areaIdInBornState[fromIndex];
				for (int toIndex = fromIndex + 1; toIndex < CS$<>8__locals1.areaIdInBornState.Count; toIndex++)
				{
					short toArea2 = CS$<>8__locals1.areaIdInBornState[toIndex];
					TravelRouteKey key3 = new TravelRouteKey(fromArea2, toArea2);
					bool flag6 = this._bornStateTravelRouteDict.ContainsKey(key3);
					if (!flag6)
					{
						TravelRoute route3 = new TravelRoute();
						path.Clear();
						aStarMap.FindWay(fromArea2, toArea2, ref path);
						route3.AreaList.AddRange(path);
						for (int i2 = 0; i2 < path.Count - 1; i2++)
						{
							short subRouteFromArea2 = path[i2];
							short subRouteToArea2 = path[i2 + 1];
							bool reverse2 = subRouteFromArea2 > subRouteToArea2;
							TravelRouteKey subRouteKey2 = new TravelRouteKey(subRouteFromArea2, subRouteToArea2);
							bool flag7 = reverse2;
							if (flag7)
							{
								subRouteKey2.Reverse();
							}
							TravelRoute subRoute2 = this._travelRouteDict[subRouteKey2];
							bool flag8 = !reverse2;
							if (flag8)
							{
								route3.PosList.AddRange(subRoute2.PosList);
								for (int j2 = 0; j2 < subRoute2.CostList.Count; j2++)
								{
									route3.CostList.Add(subRoute2.CostList[j2]);
								}
							}
							else
							{
								for (int j3 = subRoute2.PosList.Count - 1; j3 >= 0; j3--)
								{
									route3.PosList.Add(subRoute2.PosList[j3]);
								}
								for (int j4 = subRoute2.CostList.Count - 1; j4 >= 0; j4--)
								{
									route3.CostList.Add(subRoute2.CostList[j4]);
								}
							}
							bool flag9 = i2 < path.Count - 2;
							if (flag9)
							{
								sbyte[] toAreaPos2 = this._areas[(int)subRouteToArea2].GetConfig().WorldMapPos;
								route3.PosList.Add(new ByteCoordinate((byte)toAreaPos2[0], (byte)toAreaPos2[1]));
							}
						}
						this.AddElement_BornStateTravelRouteDict(key3, route3, context);
					}
				}
			}
			for (short areaId2 = 0; areaId2 < 134; areaId2 += 1)
			{
				TravelRouteKey key4 = new TravelRouteKey(areaId2, 135);
				TravelRoute route4 = new TravelRoute();
				route4.AreaList.Add(areaId2);
				route4.AreaList.Add(135);
				route4.CostList.Add(10);
				this.AddElement_TravelRouteDict(key4, route4, context);
			}
			ObjectPool<List<short>>.Instance.Return(CS$<>8__locals1.areaIdInBornState);
			ObjectPool<List<short>>.Instance.Return(path);
			List<short> allAreaList = new List<short>();
			for (short areaId3 = 0; areaId3 < 45; areaId3 += 1)
			{
				allAreaList.Add(areaId3);
			}
			short areaId;
			short areaId4;
			for (areaId = 0; areaId < 45; areaId = areaId4 + 1)
			{
				GameData.Utilities.ShortList otherAreaList = GameData.Utilities.ShortList.Create();
				otherAreaList.Items.AddRange(allAreaList);
				otherAreaList.Items.Remove(areaId);
				otherAreaList.Items.Sort(delegate(short area1, short area2)
				{
					TravelRouteKey routeKey = new TravelRouteKey(areaId, area1);
					TravelRouteKey routeKey2 = new TravelRouteKey(areaId, area2);
					bool flag10 = areaId > area1;
					if (flag10)
					{
						routeKey.Reverse();
					}
					bool flag11 = areaId > area2;
					if (flag11)
					{
						routeKey2.Reverse();
					}
					return CS$<>8__locals1.<>4__this._travelRouteDict[routeKey].GetTotalTimeCost().CompareTo(CS$<>8__locals1.<>4__this._travelRouteDict[routeKey2].GetTotalTimeCost());
				});
				this.AddElement_RegularAreaNearList(areaId, otherAreaList, context);
				areaId4 = areaId;
			}
		}

		// Token: 0x0600796F RID: 31087 RVA: 0x0047411C File Offset: 0x0047231C
		public override void UnpackCrossArchiveGameData(DataContext context, CrossArchiveGameData crossArchiveGameData)
		{
			DomainManager.Map.SetCrossArchiveLockMoveTime(true, context);
			GameDataBridge.AddDisplayEvent(DisplayEventType.TaiwuCrossArchiveSpecialEffect);
			Location villageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
			List<MapBlockData> nearBlocks = ObjectPool<List<MapBlockData>>.Instance.Get();
			DomainManager.Map.GetLocationByDistance(villageLocation, 15, 15, ref nearBlocks);
			bool flag = nearBlocks.Count > 0;
			if (flag)
			{
				MapBlockData mapBlockData = nearBlocks.GetRandom(context.Random);
				DomainManager.Map.SetTeleportMove(true);
				DomainManager.Map.Move(context, mapBlockData.BlockId, true);
				DomainManager.Map.SetTeleportMove(false);
			}
			ObjectPool<List<MapBlockData>>.Instance.Return(nearBlocks);
			List<MapBlockData> exceptBlocks = ObjectPool<List<MapBlockData>>.Instance.Get();
			this.GetInSightBlocks(exceptBlocks);
			this.HideAllBlocks(context, DomainManager.Taiwu.GetTaiwuVillageLocation().AreaId, exceptBlocks);
			ObjectPool<List<MapBlockData>>.Instance.Return(exceptBlocks);
			MapBlockData villageBlock = DomainManager.Map.GetBlock(villageLocation);
			villageBlock.SetVisible(true, context);
		}

		// Token: 0x06007970 RID: 31088 RVA: 0x0047420C File Offset: 0x0047240C
		[DomainMethod]
		public void RetrieveDreamBackLocation(DataContext context, Location location)
		{
			bool flag = DomainManager.TaiwuEvent.GetGlobalEventArgumentBox().GetBool("ConchShip_PresetKey_FuyuHiltGuiding") || DomainManager.Taiwu.GetTaiwu().GetLocation() != location;
			if (flag)
			{
				GameDataBridge.AddDisplayEvent<bool>(DisplayEventType.SetEventLockInputState, false);
			}
			else
			{
				foreach (EDreamBackLocationType type in this.RemoveDreamBackLocation(context, location))
				{
					TaiwuEventDomain taiwuEvent = DomainManager.TaiwuEvent;
					if (!true)
					{
					}
					sbyte arg;
					switch (type)
					{
					case EDreamBackLocationType.Inventory:
						arg = 1;
						break;
					case EDreamBackLocationType.LifeSkill:
						arg = 3;
						break;
					case EDreamBackLocationType.CombatSkill:
						arg = 2;
						break;
					default:
						arg = -1;
						break;
					}
					if (!true)
					{
					}
					taiwuEvent.OnEvent_TaiwuCrossArchiveFindMemory(arg);
				}
			}
		}

		// Token: 0x06007971 RID: 31089 RVA: 0x004742D4 File Offset: 0x004724D4
		public void CreateDreamBackLocation(DataContext context, Location location, EDreamBackLocationType locationType)
		{
			List<DreamBackLocationData> dreamBackLocationData = DomainManager.Extra.GetDreamBackLocationData();
			DreamBackLocationData data = DreamBackLocationData.Create(location, locationType);
			dreamBackLocationData.Add(data);
			DomainManager.Extra.SetDreamBackLocationData(dreamBackLocationData, context);
		}

		// Token: 0x06007972 RID: 31090 RVA: 0x0047430A File Offset: 0x0047250A
		public IEnumerable<EDreamBackLocationType> RemoveDreamBackLocation(DataContext context, Location location)
		{
			List<DreamBackLocationData> dreamBackLocationData = DomainManager.Extra.GetDreamBackLocationData();
			bool anyRemoved = false;
			int num;
			for (int i = dreamBackLocationData.Count - 1; i >= 0; i = num - 1)
			{
				DreamBackLocationData locationData = dreamBackLocationData[i];
				bool flag = locationData.Location != location;
				if (!flag)
				{
					CollectionUtils.SwapAndRemove<DreamBackLocationData>(dreamBackLocationData, i);
					anyRemoved = true;
					yield return locationData.Type;
					locationData = default(DreamBackLocationData);
				}
				num = i;
			}
			bool flag2 = anyRemoved;
			if (flag2)
			{
				DomainManager.Extra.SetDreamBackLocationData(dreamBackLocationData, context);
			}
			yield break;
		}

		// Token: 0x06007973 RID: 31091 RVA: 0x00474328 File Offset: 0x00472528
		[DomainMethod]
		public void GmCmd_SetLockTime(bool isLock)
		{
			this._lockMoveTime = isLock;
		}

		// Token: 0x06007974 RID: 31092 RVA: 0x00474332 File Offset: 0x00472532
		[DomainMethod]
		public void GmCmd_SetTeleportMove(bool teleportOn)
		{
			this._teleportMove = teleportOn;
		}

		// Token: 0x06007975 RID: 31093 RVA: 0x0047433C File Offset: 0x0047253C
		[DomainMethod]
		public unsafe void GmCmd_ShowAllMapBlock(DataContext context)
		{
			for (short areaId = 0; areaId < 139; areaId += 1)
			{
				Span<MapBlockData> blocks = this.GetAreaBlocks(areaId);
				for (int i = 0; i < blocks.Length; i++)
				{
					MapBlockData block = *blocks[i];
					bool flag = block.TemplateId != 126;
					if (flag)
					{
						block.SetVisible(true, context);
					}
				}
				this._areas[(int)areaId].Discovered = true;
				this.SetElement_Areas((int)areaId, this._areas[(int)areaId], context);
			}
			DomainManager.Merchant.RefreshCaravanInTaiwuState(context);
		}

		// Token: 0x06007976 RID: 31094 RVA: 0x004743D6 File Offset: 0x004725D6
		[DomainMethod]
		public void GmCmd_HideAllMapBlock(DataContext context)
		{
			DomainManager.Map.HideAllBlocks(context);
			DomainManager.Merchant.RefreshCaravanInTaiwuState(context);
		}

		// Token: 0x06007977 RID: 31095 RVA: 0x004743F4 File Offset: 0x004725F4
		[DomainMethod]
		public void GmCmd_UnlockAllStation(DataContext context)
		{
			for (short areaId = 0; areaId < 135; areaId += 1)
			{
				bool flag = !this._areas[(int)areaId].StationUnlocked;
				if (flag)
				{
					MapBlockData stationBlock = this.GetBlock(areaId, this._areas[(int)areaId].StationBlockId);
					this.UnlockStation(context, areaId, false);
					bool flag2 = !stationBlock.Visible;
					if (flag2)
					{
						stationBlock.SetVisible(true, context);
					}
				}
			}
		}

		// Token: 0x06007978 RID: 31096 RVA: 0x00474466 File Offset: 0x00472666
		[DomainMethod]
		public void GmCmd_ChangeSpiritualDebt(DataContext context, short areaId, int spiritualDebt)
		{
			DomainManager.Extra.SetAreaSpiritualDebt(context, areaId, spiritualDebt, true, true);
		}

		// Token: 0x06007979 RID: 31097 RVA: 0x0047447C File Offset: 0x0047267C
		[DomainMethod]
		public void GmCmd_ChangeAllSpiritualDebt(DataContext context, int spiritualDebt)
		{
			short i = 0;
			while ((int)i < DomainManager.Map._areas.Length)
			{
				DomainManager.Extra.SetAreaSpiritualDebt(context, i, spiritualDebt, true, true);
				i += 1;
			}
		}

		// Token: 0x0600797A RID: 31098 RVA: 0x004744B6 File Offset: 0x004726B6
		[DomainMethod]
		public void GmCmd_SetMapBlockData(DataContext context, MapBlockData mapBlockData)
		{
			this.SetBlockData(context, mapBlockData);
		}

		// Token: 0x0600797B RID: 31099 RVA: 0x004744C4 File Offset: 0x004726C4
		[DomainMethod]
		public void GmCmd_CreateFixedCharacterAtCurrentBlock(DataContext context, short templateId)
		{
			GameData.Domains.Character.Character taiwuCharacter = DomainManager.Taiwu.GetTaiwu();
			GameData.Domains.Character.Character character;
			bool flag = Config.Character.Instance[templateId].CreatingType == 0 && DomainManager.Character.TryGetFixedCharacterByTemplateId(templateId, out character);
			if (flag)
			{
				EventHelper.MoveFixedCharacter(character, taiwuCharacter.GetLocation());
			}
			else
			{
				character = DomainManager.Character.CreateFixedCharacter(context, templateId);
				bool flag2 = character != null && taiwuCharacter != null;
				if (flag2)
				{
					Location location = taiwuCharacter.GetLocation();
					DomainManager.Character.CompleteCreatingCharacter(character.GetId());
					EventHelper.MoveFixedCharacter(character, location);
				}
			}
		}

		// Token: 0x0600797C RID: 31100 RVA: 0x00474554 File Offset: 0x00472754
		[DomainMethod]
		public void GmCmd_AddAnimal(DataContext context, short templateId)
		{
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
			DomainManager.Extra.CreateAnimalByCharacterTemplateId(context, templateId, taiwuLocation);
		}

		// Token: 0x0600797D RID: 31101 RVA: 0x00474580 File Offset: 0x00472780
		[DomainMethod]
		public void GmCmd_AddRandomEnemyOnMap(DataContext context, short templateId)
		{
			byte creatingType = Config.Character.Instance[templateId].CreatingType;
			bool condition = creatingType - 2 <= 1;
			Tester.Assert(condition, "");
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			MapBlockData block = DomainManager.Map.GetBlock(taiwu.GetLocation());
			block.AddTemplateEnemy(new MapTemplateEnemyInfo(templateId, block.BlockId, -1));
			DomainManager.Map.SetBlockData(context, block);
		}

		// Token: 0x0600797E RID: 31102 RVA: 0x004745F8 File Offset: 0x004727F8
		[DomainMethod]
		public unsafe void GmCmd_TurnMapBlockIntoAshes(DataContext context)
		{
			short areaId = DomainManager.Taiwu.GetTaiwu().GetLocation().AreaId;
			Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(areaId);
			for (int i = 0; i < areaBlocks.Length; i++)
			{
				MapBlockData block = *areaBlocks[i];
				bool flag = !block.IsCityTown() && (block.IsNonDeveloped() || block.GetConfig().Type == EMapBlockType.Developed);
				if (flag)
				{
					DomainManager.Map.ChangeBlockTemplate(context, block, context.Random.NextBool() ? 118 : 124);
				}
			}
		}

		// Token: 0x0600797F RID: 31103 RVA: 0x0047468E File Offset: 0x0047288E
		[DomainMethod]
		public void GMCmd_ThrowBackend()
		{
			throw new Exception("a backend test exception");
		}

		// Token: 0x06007980 RID: 31104 RVA: 0x0047469C File Offset: 0x0047289C
		[DomainMethod]
		public bool GmCmd_TriggerTravelingEvent(DataContext context, short templateId)
		{
			TravelingEventItem configData = TravelingEvent.Instance[templateId];
			bool flag = string.IsNullOrEmpty(configData.Event);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				TravelingEventCollection travelingEventCollection = DomainManager.Extra.GetTravelingEventCollection();
				short currAreaId = DomainManager.Taiwu.GetTaiwu().GetValidLocation().AreaId;
				int offset = this.AddTravelingEvent(context, templateId, currAreaId);
				bool flag2 = offset < 0;
				if (flag2)
				{
					result = false;
				}
				else
				{
					TaiwuEvent taiwuEvent = DomainManager.TaiwuEvent.GetEvent(configData.Event);
					bool flag3 = taiwuEvent != null;
					if (flag3)
					{
						TaiwuEvent taiwuEvent2 = taiwuEvent;
						if (taiwuEvent2.ArgBox == null)
						{
							taiwuEvent2.ArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
						}
						travelingEventCollection.FillEventArgBox(offset, taiwuEvent.ArgBox);
						bool flag4 = taiwuEvent.EventConfig.CheckCondition();
						if (!flag4)
						{
							int size = travelingEventCollection.GetRecordSize(offset);
							travelingEventCollection.Remove(offset, size);
							Logger logger = MapDomain.Logger;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(75, 3);
							defaultInterpolatedStringHandler.AppendLiteral("Traveling event ");
							defaultInterpolatedStringHandler.AppendFormatted<short>(templateId);
							defaultInterpolatedStringHandler.AppendLiteral(" - ");
							defaultInterpolatedStringHandler.AppendFormatted(configData.Name);
							defaultInterpolatedStringHandler.AppendLiteral(" is triggering ");
							defaultInterpolatedStringHandler.AppendFormatted(taiwuEvent.EventGuid);
							defaultInterpolatedStringHandler.AppendLiteral(" when OnCheckEventCondition return false.");
							logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
							return false;
						}
						DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
						DomainManager.TaiwuEvent.TravelingEventCheckComplete();
						DomainManager.Map.SetOnHandlingTravelingEventBlock(true, context);
					}
					else
					{
						Logger logger2 = MapDomain.Logger;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 3);
						defaultInterpolatedStringHandler.AppendLiteral("Monthly Event ");
						defaultInterpolatedStringHandler.AppendFormatted<short>(templateId);
						defaultInterpolatedStringHandler.AppendLiteral(" - ");
						defaultInterpolatedStringHandler.AppendFormatted(configData.Name);
						defaultInterpolatedStringHandler.AppendLiteral(" (");
						defaultInterpolatedStringHandler.AppendFormatted(configData.Event);
						defaultInterpolatedStringHandler.AppendLiteral(") not found.");
						logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06007981 RID: 31105 RVA: 0x004748AC File Offset: 0x00472AAC
		[DomainMethod]
		public int GmCmd_GetTreasuryValueByTaiwuLocation()
		{
			Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
			bool flag = !location.IsValid();
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				Settlement settlement = DomainManager.Organization.GetSettlementByLocation(location);
				bool flag2 = settlement == null;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					result = settlement.Treasuries.CurrentTotalValue;
				}
			}
			return result;
		}

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06007982 RID: 31106 RVA: 0x00474905 File Offset: 0x00472B05
		// (set) Token: 0x06007983 RID: 31107 RVA: 0x0047490D File Offset: 0x00472B0D
		public bool TempDisableTriggerNormalPickupByTaiwuEscape { get; set; }

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06007984 RID: 31108 RVA: 0x00474916 File Offset: 0x00472B16
		// (set) Token: 0x06007985 RID: 31109 RVA: 0x0047491E File Offset: 0x00472B1E
		public Location TaiwuLastLocation { get; set; }

		// Token: 0x06007986 RID: 31110 RVA: 0x00474928 File Offset: 0x00472B28
		public void SetMapPickupUsed(DataContext context, MapPickup pickup)
		{
			bool flag = pickup == null;
			if (flag)
			{
				throw new ArgumentNullException("pickup");
			}
			MapPickupCollection pickupCollection;
			bool flag2 = !DomainManager.Extra.TryGetElement_PickupDict(pickup.Location, out pickupCollection);
			if (flag2)
			{
				throw new ArgumentException("SetPickupUsed: Trying to set a pickup not in pickup collection");
			}
			bool flag3 = pickupCollection == null;
			if (!flag3)
			{
				this.SetMapPickupUsedInternal(context, pickupCollection, pickup);
			}
		}

		// Token: 0x06007987 RID: 31111 RVA: 0x00474981 File Offset: 0x00472B81
		private void SetMapPickupUsedInternal(DataContext context, MapPickupCollection pickupCollection, MapPickup pickup)
		{
			pickup.SetAsUsed();
			DomainManager.Extra.SetMapPickupCollection(context, pickup.Location, pickupCollection);
		}

		// Token: 0x06007988 RID: 31112 RVA: 0x004749A0 File Offset: 0x00472BA0
		public IEnumerable<MapPickup> GetMapPickups(Location location)
		{
			MapPickupCollection pickupCollection;
			bool flag = !DomainManager.Extra.TryGetElement_PickupDict(location, out pickupCollection);
			IEnumerable<MapPickup> result;
			if (flag)
			{
				result = Enumerable.Empty<MapPickup>();
			}
			else
			{
				sbyte xiangshuProgress = DomainManager.World.GetXiangshuProgress();
				sbyte xiangshuLevel = GameData.Domains.World.SharedMethods.GetXiangshuLevel(xiangshuProgress);
				result = pickupCollection.IterVisiblePickups(xiangshuLevel);
			}
			return result;
		}

		// Token: 0x06007989 RID: 31113 RVA: 0x004749EC File Offset: 0x00472BEC
		public MapPickup FindFirstVisibleMapPickupOnLocation(Location location)
		{
			List<MapPickup> pickups = this.GetMapPickups(location).ToList<MapPickup>();
			pickups.Sort(new Comparison<MapPickup>(MapPickupHelper.CompareVisiblePickups));
			bool flag = pickups.Count == 0;
			MapPickup result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = pickups[0];
			}
			return result;
		}

		// Token: 0x0600798A RID: 31114 RVA: 0x00474A38 File Offset: 0x00472C38
		public void TriggerNormalMapPickup(DataContext context, MapPickup pickup)
		{
			bool flag = pickup == null;
			if (!flag)
			{
				MapPickupCollection pickupCollection;
				bool flag2 = !DomainManager.Extra.TryGetElement_PickupDict(pickup.Location, out pickupCollection);
				if (!flag2)
				{
					bool flag3 = pickupCollection == null;
					if (!flag3)
					{
						bool isEventType = pickup.IsEventType;
						if (isEventType)
						{
							throw new ArgumentException("TriggerNormalMapPickup: Trying to trigger a pickup that is an event");
						}
						this.SetMapPickupUsedInternal(context, pickupCollection, pickup);
						GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
						int invokeBonusRate = 0;
						foreach (ItemKey itemKey in taiwu.GetEquipment())
						{
							IExploreBonusRateItem bonusRateItem = DomainManager.Item.TryGetBaseItem(itemKey) as IExploreBonusRateItem;
							bool flag4 = bonusRateItem != null;
							if (flag4)
							{
								invokeBonusRate += bonusRateItem.GetExploreBonusRate();
							}
						}
						bool bonusIsExtra = true;
						bool invokeBonus = context.Random.CheckPercentProb(invokeBonusRate);
						bool flag5 = invokeBonus && pickup.Type == MapPickup.EMapPickupType.Item;
						if (flag5)
						{
							IItemConfig config = ItemConfigHelper.GetConfig(pickup.ItemType, pickup.ItemTemplateId);
							bool flag6 = config.ItemSubType != 505;
							if (flag6)
							{
								bonusIsExtra = false;
								IItemConfig upgradeConfig = config.Upgrade();
								bool flag7 = upgradeConfig == null;
								if (flag7)
								{
									invokeBonus = false;
								}
								else
								{
									pickup = new MapPickup(pickup)
									{
										ItemTemplateId = upgradeConfig.TemplateId
									};
								}
							}
						}
						MapDomain.ApplyPickupDelegate handler;
						bool flag8 = MapDomain.ApplyPickups.TryGetValue(pickup.Type, out handler);
						if (flag8)
						{
							handler(context, pickup);
							bool flag9 = invokeBonus && bonusIsExtra;
							if (flag9)
							{
								handler(context, pickup);
							}
						}
						MapElementPickupDisplayData displayData = this.GetPickupDisplayData(pickup);
						GameDataBridge.AddDisplayEvent<MapElementPickupDisplayData>(DisplayEventType.PlayMapPickupEffect, displayData);
						bool flag10 = invokeBonus && pickup.Template.ExtraBonusReplaceInstantNotification >= 0;
						if (flag10)
						{
							this.AddNotification(pickup, pickup.Template.ExtraBonusReplaceInstantNotification);
						}
						else
						{
							bool flag11 = pickup.Template.InstantNotification >= 0;
							if (flag11)
							{
								this.AddNotification(pickup, pickup.Template.InstantNotification);
							}
							bool flag12 = invokeBonus && pickup.Template.ExtraBonusAddInstantNotification >= 0;
							if (flag12)
							{
								this.AddNotification(pickup, pickup.Template.ExtraBonusAddInstantNotification);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600798B RID: 31115 RVA: 0x00474C64 File Offset: 0x00472E64
		private void AddNotification(MapPickup pickup, short notificationId)
		{
			InstantNotificationCollection collection = DomainManager.World.GetInstantNotificationCollection();
			switch (notificationId)
			{
			case 179:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Resource, "");
				collection.AddMapPickupsResource(pickup.Location, pickup.ResourceType, pickup.ResourceCount);
				break;
			case 180:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsFoodIngredients(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 181:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsMaterials(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 182:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsHerbal0(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 183:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsHerbal1(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 184:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsPoisonCorrected(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 185:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsInjuryMedicineCorrected(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 186:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsAntidoteCorrected(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 187:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsGainMedicineCorrected(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 188:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsFruit(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 189:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsChickenDishes(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 190:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsMeatDishes(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 191:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsVegetarianDishes(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 192:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsSeafoodDishes(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 193:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsWine(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 194:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsTea(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 195:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsTool(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 196:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsAccessory(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 197:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsPoisonCream(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 198:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsHarrier(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 199:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsToken(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 200:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsNeedleBox(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 201:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsThorn(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 202:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsHiddenWeapon(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 203:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsFlute(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 204:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsGloves(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 205:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsFurGloves(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 206:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsPestle(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 207:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsSword(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 208:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsBlade(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 209:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsPolearm(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 210:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupQin(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 211:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsWhisk(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 212:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsWhip(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 213:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsCrest(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 214:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsShoes(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 215:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsArmor(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 216:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsArmGuard(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 217:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsCarDrop(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 218:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.ExpBonus, "");
				collection.AddMapPickupsExp(pickup.Location, pickup.ExpCount);
				break;
			case 219:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.ReadEffect, "");
				collection.AddMapPickupsReading();
				break;
			case 220:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.LoopEffect, "");
				collection.AddMapPickupsQiArt();
				break;
			case 221:
			{
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.DebtBonus, "");
				sbyte stateTemplateId = this.GetStateTemplateIdByAreaId(pickup.Location.AreaId);
				collection.AddMapPickupsMorale(stateTemplateId);
				break;
			}
			case 244:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Resource, "");
				collection.AddMapPickupsResourceUpdate(pickup.ResourceType, pickup.ResourceCount);
				break;
			case 245:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.ExpBonus, "");
				collection.AddMapPickupsExpUpdate(pickup.ExpCount);
				break;
			case 246:
			{
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.DebtBonus, "");
				sbyte stateTemplateIdUpdate = this.GetStateTemplateIdByAreaId(pickup.Location.AreaId);
				collection.AddMapPickupsMoraleUpdate(stateTemplateIdUpdate);
				break;
			}
			case 247:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsItemUpdate(pickup.ItemType, pickup.ItemTemplateId);
				break;
			case 248:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.ReadEffect, "");
				collection.AddMapPickupsReadingUpdate();
				break;
			case 249:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.LoopEffect, "");
				collection.AddMapPickupsQiArtUpdate();
				break;
			case 254:
				Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item, "");
				collection.AddMapPickupsMedicineUpdate(pickup.ItemType, pickup.ItemTemplateId);
				break;
			}
		}

		// Token: 0x0600798C RID: 31116 RVA: 0x00475712 File Offset: 0x00473912
		private static void AddResourceByPickup(DataContext context, MapPickup pickup)
		{
			DomainManager.Taiwu.AddResource(context, ItemSourceType.Inventory, pickup.ResourceType, pickup.ResourceCount);
		}

		// Token: 0x0600798D RID: 31117 RVA: 0x00475730 File Offset: 0x00473930
		private static void AddItemByPickup(DataContext context, MapPickup pickup)
		{
			ItemKey itemKey = DomainManager.Item.CreateItem(context, pickup.ItemType, pickup.ItemTemplateId);
			MapDomain.AddItem(context, itemKey, 1);
		}

		// Token: 0x0600798E RID: 31118 RVA: 0x0047575F File Offset: 0x0047395F
		private static void LoopOnceByPickup(DataContext context, MapPickup pickup)
		{
			DomainManager.Taiwu.ApplyNeigongLoopingImprovementOnce(context, 100);
			DomainManager.Taiwu.TryAddLoopingEvent(context, (int)GlobalConfig.Instance.BaseLoopingEventProbability);
		}

		// Token: 0x0600798F RID: 31119 RVA: 0x00475788 File Offset: 0x00473988
		private static void ReadOnceByPickup(DataContext context, MapPickup pickup)
		{
			bool haveEvent = DomainManager.Taiwu.UpdateReadingProgressOnce(context, false, false);
			bool flag = !haveEvent;
			if (!flag)
			{
				ItemKey currBook = DomainManager.Taiwu.GetCurReadingBook();
				DomainManager.Extra.AddReadingEventBookId(context, currBook.Id);
			}
		}

		// Token: 0x06007990 RID: 31120 RVA: 0x004757CC File Offset: 0x004739CC
		private static void AddExpByPickup(DataContext context, MapPickup pickup)
		{
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			taiwuChar.ChangeExp(context, pickup.ExpCount);
		}

		// Token: 0x06007991 RID: 31121 RVA: 0x004757F4 File Offset: 0x004739F4
		private static void AddDebtByPickup(DataContext context, MapPickup pickup)
		{
			sbyte stateId = DomainManager.Map.GetStateIdByAreaId(pickup.Location.AreaId);
			List<short> areaIdList = ObjectPool<List<short>>.Instance.Get();
			DomainManager.Map.GetAllAreaInState(stateId, areaIdList);
			foreach (short areaId in areaIdList)
			{
				DomainManager.Extra.ChangeAreaSpiritualDebt(context, areaId, pickup.DebtCount, true, false);
			}
		}

		// Token: 0x06007992 RID: 31122 RVA: 0x00475884 File Offset: 0x00473A84
		public void SetPickupAtFirst(DataContext context, MapPickup pickup)
		{
			bool flag = pickup == null;
			if (flag)
			{
				throw new ArgumentNullException("pickup");
			}
			MapPickupCollection pickupCollection;
			bool flag2 = !DomainManager.Extra.TryGetElement_PickupDict(pickup.Location, out pickupCollection);
			if (flag2)
			{
				throw new ArgumentException("IgnorePickup: Trying Ignore a pickup not in pickup collection");
			}
			pickupCollection.SetPickupAtFirst(pickup);
			DomainManager.Extra.SetMapPickupCollection(context, pickup.Location, pickupCollection);
		}

		// Token: 0x06007993 RID: 31123 RVA: 0x004758E4 File Offset: 0x00473AE4
		public void IgnoreOneMapPickup(DataContext context, MapPickup pickup)
		{
			bool flag = pickup == null;
			if (flag)
			{
				throw new ArgumentNullException("pickup");
			}
			MapPickupCollection pickupCollection;
			bool flag2 = !DomainManager.Extra.TryGetElement_PickupDict(pickup.Location, out pickupCollection);
			if (flag2)
			{
				throw new ArgumentException("IgnorePickup: Trying Ignore a pickup not in pickup collection");
			}
			pickupCollection.IgnorePickup(pickup);
			DomainManager.Extra.SetMapPickupCollection(context, pickup.Location, pickupCollection);
		}

		// Token: 0x06007994 RID: 31124 RVA: 0x00475944 File Offset: 0x00473B44
		public static ValueTuple<bool, PresetItemWithCount> CheckMapPickupConfigItemRewards(MapPickupsItem pickupConfig, int index)
		{
			List<PresetItemWithCount> itemRewards = pickupConfig.EventSecondItemRewards;
			bool flag = index < 0 || itemRewards == null || index > itemRewards.Count - 1;
			ValueTuple<bool, PresetItemWithCount> result;
			if (flag)
			{
				result = new ValueTuple<bool, PresetItemWithCount>(false, null);
			}
			else
			{
				PresetItemWithCount itemReward = itemRewards[index];
				bool flag2 = !itemReward.IsValid;
				if (flag2)
				{
					result = new ValueTuple<bool, PresetItemWithCount>(false, null);
				}
				else
				{
					result = new ValueTuple<bool, PresetItemWithCount>(true, itemReward);
				}
			}
			return result;
		}

		// Token: 0x06007995 RID: 31125 RVA: 0x004759A8 File Offset: 0x00473BA8
		public static ValueTuple<bool, ResourceInfo?> CheckMapPickupConfigResourceRewards(MapPickupsItem pickupConfig, int index)
		{
			List<ResourceInfo> resourceRewards = pickupConfig.EventSecondResourceRewards;
			bool flag = index < 0 || resourceRewards == null || index > resourceRewards.Count - 1;
			ValueTuple<bool, ResourceInfo?> result;
			if (flag)
			{
				result = new ValueTuple<bool, ResourceInfo?>(false, null);
			}
			else
			{
				ResourceInfo resourceReward = resourceRewards[index];
				bool flag2 = resourceReward.ResourceType < 0;
				if (flag2)
				{
					result = new ValueTuple<bool, ResourceInfo?>(false, null);
				}
				else
				{
					result = new ValueTuple<bool, ResourceInfo?>(true, new ResourceInfo?(resourceReward));
				}
			}
			return result;
		}

		// Token: 0x06007996 RID: 31126 RVA: 0x00475A28 File Offset: 0x00473C28
		public static ValueTuple<bool, PropertyAndValue?> CheckMapPickupConfigQualificationRewards(MapPickupsItem pickupConfig, int index)
		{
			List<PropertyAndValue> propertyRewards = pickupConfig.EventSecondPropertyRewards;
			bool flag = index < 0 || propertyRewards == null || index > propertyRewards.Count - 1;
			ValueTuple<bool, PropertyAndValue?> result;
			if (flag)
			{
				result = new ValueTuple<bool, PropertyAndValue?>(false, null);
			}
			else
			{
				PropertyAndValue qualificationReward = propertyRewards[index];
				short propertyId = qualificationReward.PropertyId;
				bool isLifeSkillQualification = propertyId >= 34 && propertyId <= 49;
				bool isCombatSkillQualification = propertyId >= 66 && propertyId <= 79;
				bool flag2 = !isLifeSkillQualification && !isCombatSkillQualification;
				if (flag2)
				{
					result = new ValueTuple<bool, PropertyAndValue?>(false, null);
				}
				else
				{
					result = new ValueTuple<bool, PropertyAndValue?>(true, new PropertyAndValue?(qualificationReward));
				}
			}
			return result;
		}

		// Token: 0x06007997 RID: 31127 RVA: 0x00475AD8 File Offset: 0x00473CD8
		public static ValueTuple<bool, PropertyAndValue?> CheckMapPickupConfigCricketLuckPointRewards(MapPickupsItem pickupConfig, int index)
		{
			List<PropertyAndValue> propertyRewards = pickupConfig.EventSecondPropertyRewards;
			bool flag = index < 0 || propertyRewards == null || index > propertyRewards.Count - 1;
			ValueTuple<bool, PropertyAndValue?> result;
			if (flag)
			{
				result = new ValueTuple<bool, PropertyAndValue?>(false, null);
			}
			else
			{
				PropertyAndValue qualificationReward = propertyRewards[index];
				short propertyId = qualificationReward.PropertyId;
				bool flag2 = propertyId != 111;
				if (flag2)
				{
					result = new ValueTuple<bool, PropertyAndValue?>(false, null);
				}
				else
				{
					result = new ValueTuple<bool, PropertyAndValue?>(true, new PropertyAndValue?(qualificationReward));
				}
			}
			return result;
		}

		// Token: 0x06007998 RID: 31128 RVA: 0x00475B60 File Offset: 0x00473D60
		public static ValueTuple<bool, int?> CheckMapPickupConfigDebtRewards(MapPickupsItem pickupConfig, int index)
		{
			List<int> debtRewards = pickupConfig.EventSecondDebtRewards;
			bool flag = index < 0 || debtRewards == null || index > debtRewards.Count - 1;
			ValueTuple<bool, int?> result;
			if (flag)
			{
				result = new ValueTuple<bool, int?>(false, null);
			}
			else
			{
				int debtReward = debtRewards[index];
				bool flag2 = debtReward < 0;
				if (flag2)
				{
					result = new ValueTuple<bool, int?>(false, null);
				}
				else
				{
					result = new ValueTuple<bool, int?>(true, new int?(debtReward));
				}
			}
			return result;
		}

		// Token: 0x06007999 RID: 31129 RVA: 0x00475BD8 File Offset: 0x00473DD8
		public static ValueTuple<bool, int?> CheckMapPickupConfigExpRewards(MapPickupsItem pickupConfig, int index)
		{
			List<int> expRewards = pickupConfig.EventSecondExpRewards;
			bool flag = index < 0 || expRewards == null || index > expRewards.Count - 1;
			ValueTuple<bool, int?> result;
			if (flag)
			{
				result = new ValueTuple<bool, int?>(false, null);
			}
			else
			{
				int expReward = expRewards[index];
				result = new ValueTuple<bool, int?>(true, new int?(expReward));
			}
			return result;
		}

		// Token: 0x0600799A RID: 31130 RVA: 0x00475C34 File Offset: 0x00473E34
		public bool GiveMapPickupEventUserSelectedItemReward(DataContext context, ItemKey itemKey, int count)
		{
			MapDomain.AddItem(context, itemKey, count);
			return true;
		}

		// Token: 0x0600799B RID: 31131 RVA: 0x00475C50 File Offset: 0x00473E50
		public bool GiveMapPickupEventNormalReward(DataContext context, MapPickup pickup, int index)
		{
			bool flag = index < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				ValueTuple<bool, PresetItemWithCount> valueTuple = MapDomain.CheckMapPickupConfigItemRewards(pickup.Template, index);
				bool isValidItemReward = valueTuple.Item1;
				PresetItemWithCount itemReward = valueTuple.Item2;
				bool flag2 = isValidItemReward;
				if (flag2)
				{
					result = MapDomain.AddItemByEventPickup(context, itemReward);
				}
				else
				{
					ValueTuple<bool, ResourceInfo?> valueTuple2 = MapDomain.CheckMapPickupConfigResourceRewards(pickup.Template, index);
					bool isValidResourceReward = valueTuple2.Item1;
					ResourceInfo? resourceReward = valueTuple2.Item2;
					bool flag3 = isValidResourceReward;
					if (flag3)
					{
						result = MapDomain.AddResourceByEventPickup(context, resourceReward.Value);
					}
					else
					{
						ValueTuple<bool, PropertyAndValue?> valueTuple3 = MapDomain.CheckMapPickupConfigQualificationRewards(pickup.Template, index);
						bool isValidQualificationReward = valueTuple3.Item1;
						PropertyAndValue? qualificationReward = valueTuple3.Item2;
						bool flag4 = isValidQualificationReward;
						if (flag4)
						{
							result = MapDomain.AddQualificationByEventPickup(context, qualificationReward.Value);
						}
						else
						{
							ValueTuple<bool, PropertyAndValue?> valueTuple4 = MapDomain.CheckMapPickupConfigCricketLuckPointRewards(pickup.Template, index);
							bool isValidCricketLuckPointReward = valueTuple4.Item1;
							PropertyAndValue? cricketLuckPointReward = valueTuple4.Item2;
							bool flag5 = isValidCricketLuckPointReward;
							if (flag5)
							{
								result = MapDomain.AddCricketLuckPointByEventPickup(context, cricketLuckPointReward.Value);
							}
							else
							{
								ValueTuple<bool, int?> valueTuple5 = MapDomain.CheckMapPickupConfigDebtRewards(pickup.Template, index);
								bool isValidDebtReward = valueTuple5.Item1;
								int? debtReward = valueTuple5.Item2;
								bool flag6 = isValidDebtReward;
								if (flag6)
								{
									result = MapDomain.AddDebtByEventPickup(context, pickup, debtReward.Value);
								}
								else
								{
									ValueTuple<bool, int?> valueTuple6 = MapDomain.CheckMapPickupConfigExpRewards(pickup.Template, index);
									bool isValidExpReward = valueTuple6.Item1;
									int? expReward = valueTuple6.Item2;
									bool flag7 = isValidExpReward;
									result = (flag7 && MapDomain.AddExpByEventPickup(context, expReward.Value));
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600799C RID: 31132 RVA: 0x00475DB4 File Offset: 0x00473FB4
		private static bool AddDebtByEventPickup(DataContext context, MapPickup pickup, int debtReward)
		{
			short areaId = pickup.Location.AreaId;
			DomainManager.Extra.ChangeAreaSpiritualDebt(context, areaId, debtReward, true, true);
			return true;
		}

		// Token: 0x0600799D RID: 31133 RVA: 0x00475DE4 File Offset: 0x00473FE4
		private static bool AddExpByEventPickup(DataContext context, int expReward)
		{
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			taiwuChar.ChangeExp(context, expReward);
			return true;
		}

		// Token: 0x0600799E RID: 31134 RVA: 0x00475E0C File Offset: 0x0047400C
		private unsafe static bool AddQualificationByEventPickup(DataContext context, PropertyAndValue qualificationReward)
		{
			short value = qualificationReward.Value;
			GameData.Domains.Character.Character character = DomainManager.Taiwu.GetTaiwu();
			ECharacterPropertyReferencedType lifeSkillQualificationStartRef = ECharacterPropertyReferencedType.QualificationMusic;
			ECharacterPropertyReferencedType lifeSkillQualificationEndRef = ECharacterPropertyReferencedType.QualificationEclectic;
			ECharacterPropertyReferencedType combatSkillQualificationStartRef = ECharacterPropertyReferencedType.QualificationNeigong;
			ECharacterPropertyReferencedType combatSkillQualificationEndRef = ECharacterPropertyReferencedType.QualificationCombatMusic;
			short propertyId = qualificationReward.PropertyId;
			bool flag = propertyId >= (short)lifeSkillQualificationStartRef && propertyId <= (short)lifeSkillQualificationEndRef;
			bool result;
			if (flag)
			{
				LifeSkillShorts qualifications = *character.GetBaseLifeSkillQualifications();
				int skillType = (int)(propertyId - (short)lifeSkillQualificationStartRef);
				qualifications.Set(skillType, qualifications.Get(skillType) + value);
				character.SetBaseLifeSkillQualifications(ref qualifications, context);
				result = true;
			}
			else
			{
				bool flag2 = propertyId >= (short)combatSkillQualificationStartRef && propertyId <= (short)combatSkillQualificationEndRef;
				if (flag2)
				{
					CombatSkillShorts qualifications2 = *character.GetBaseCombatSkillQualifications();
					int skillType2 = (int)(propertyId - (short)combatSkillQualificationStartRef);
					*qualifications2[skillType2] = *qualifications2[skillType2] + value;
					character.SetBaseCombatSkillQualifications(ref qualifications2, context);
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x0600799F RID: 31135 RVA: 0x00475EF4 File Offset: 0x004740F4
		private static bool AddCricketLuckPointByEventPickup(DataContext context, PropertyAndValue cricketLuckPointReward)
		{
			short value = cricketLuckPointReward.Value;
			DomainManager.Taiwu.SetCricketLuckPoint(DomainManager.Taiwu.GetCricketLuckPoint() + (int)value, context);
			return true;
		}

		// Token: 0x060079A0 RID: 31136 RVA: 0x00475F28 File Offset: 0x00474128
		private static bool AddResourceByEventPickup(DataContext context, ResourceInfo resourceReward)
		{
			sbyte resourceType = resourceReward.ResourceType;
			int count = resourceReward.ResourceCount;
			DomainManager.Taiwu.AddResource(context, ItemSourceType.Inventory, resourceType, count);
			return true;
		}

		// Token: 0x060079A1 RID: 31137 RVA: 0x00475F58 File Offset: 0x00474158
		private static bool AddItemByEventPickup(DataContext context, PresetItemWithCount itemReward)
		{
			sbyte itemType = itemReward.ItemType;
			short templateId = itemReward.TemplateId;
			int count = itemReward.Count;
			ItemKey itemKey = DomainManager.Item.CreateItem(context, itemType, templateId);
			bool flag = !itemKey.IsValid();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				MapDomain.AddItem(context, itemKey, count);
				result = true;
			}
			return result;
		}

		// Token: 0x060079A2 RID: 31138 RVA: 0x00475FAE File Offset: 0x004741AE
		private static void AddItem(DataContext context, ItemKey itemKey, int count)
		{
			DomainManager.Taiwu.AddItem(context, itemKey, count, ItemSourceType.Inventory, false);
		}

		// Token: 0x060079A3 RID: 31139 RVA: 0x00475FC4 File Offset: 0x004741C4
		public MapElementPickupDisplayData GetPickupDisplayData(MapPickup pickup)
		{
			return new MapElementPickupDisplayData
			{
				Pickup = pickup,
				BanReason = pickup.CalcMapPickupBanReason(),
				CanAutoBeatXiangshuMinion = pickup.CalcCanAutoBeatXiangshuMinion()
			};
		}

		// Token: 0x060079A4 RID: 31140 RVA: 0x00475FFF File Offset: 0x004741FF
		[DomainMethod]
		public void TeleportByTraveler(DataContext context, short destBlockId)
		{
			this.SetTeleportMove(true);
			this.Move(context, destBlockId);
			this.SetTeleportMove(false);
		}

		// Token: 0x060079A5 RID: 31141 RVA: 0x0047601C File Offset: 0x0047421C
		[DomainMethod]
		public bool BuildTravelerPalace(DataContext context, Location location)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(11);
			TravelerSkillsData skillData = professionData.GetSkillsData<TravelerSkillsData>();
			bool flag = skillData.PalaceCount >= 3;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				skillData.OfflineBuildPalace(location);
				DomainManager.Extra.SetProfessionData(context, professionData);
				result = true;
			}
			return result;
		}

		// Token: 0x060079A6 RID: 31142 RVA: 0x0047606C File Offset: 0x0047426C
		[DomainMethod]
		public bool ChangeTravelerPalaceName(DataContext context, int index, string newName)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(11);
			TravelerSkillsData skillData = professionData.GetSkillsData<TravelerSkillsData>();
			TravelerPalaceData palaceData = skillData.TryGetPalaceData(index);
			bool flag = palaceData == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				palaceData.CustomName = newName;
				DomainManager.Extra.SetProfessionData(context, professionData);
				result = true;
			}
			return result;
		}

		// Token: 0x060079A7 RID: 31143 RVA: 0x004760C0 File Offset: 0x004742C0
		[DomainMethod]
		public bool DestroyTravelerPalace(DataContext context, int index)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(11);
			TravelerSkillsData skillData = professionData.GetSkillsData<TravelerSkillsData>();
			bool success = skillData.OfflineDestroyPalace(index);
			bool flag = success;
			if (flag)
			{
				DomainManager.Extra.SetProfessionData(context, professionData);
			}
			return success;
		}

		// Token: 0x060079A8 RID: 31144 RVA: 0x00476104 File Offset: 0x00474304
		[DomainMethod]
		public bool TeleportOnTravelerPalace(DataContext context, int index)
		{
			bool isTraveling = this.IsTraveling;
			bool result;
			if (isTraveling)
			{
				result = false;
			}
			else
			{
				bool flag = DomainManager.Extra.GetTotalActionPointsRemaining() < 10;
				if (flag)
				{
					result = false;
				}
				else
				{
					DomainManager.Extra.ConsumeActionPoint(context, 10);
					ProfessionData professionData = DomainManager.Extra.GetProfessionData(11);
					TravelerSkillsData skillData = professionData.GetSkillsData<TravelerSkillsData>();
					TravelerPalaceData palaceData = skillData.TryGetPalaceData(index);
					bool flag2 = palaceData == null;
					if (flag2)
					{
						result = false;
					}
					else
					{
						Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
						bool flag3 = location.AreaId != palaceData.Location.AreaId;
						if (flag3)
						{
							this.QuickTravel(context, palaceData.Location.AreaId);
						}
						this.TeleportByTraveler(context, palaceData.Location.BlockId);
						foreach (int groupCharId in DomainManager.Taiwu.GetGroupCharIds().GetCollection())
						{
							GameData.Domains.Character.Character groupChar = DomainManager.Character.GetElement_Objects(groupCharId);
							MapDomain.MakeRandomTravelerPalaceDisaster(context, groupChar);
						}
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x060079A9 RID: 31145 RVA: 0x00476240 File Offset: 0x00474440
		private static void MakeRandomTravelerPalaceDisaster(DataContext context, GameData.Domains.Character.Character groupChar)
		{
			switch (context.Random.Next(4))
			{
			case 0:
				for (int i = 0; i < ProfessionRelatedConstants.TravelerPalaceRandomInjuryCount(context.Random); i++)
				{
					sbyte bodyPart = (sbyte)context.Random.Next(7);
					bool inner = context.Random.NextBool();
					groupChar.ChangeInjury(context, bodyPart, inner, 1);
				}
				break;
			case 1:
			{
				List<sbyte> poisonTypes = ObjectPool<List<sbyte>>.Instance.Get();
				for (sbyte j = 0; j < 6; j += 1)
				{
					poisonTypes.Add(j);
				}
				foreach (sbyte poisonType in RandomUtils.GetRandomUnrepeated<sbyte>(context.Random, 3, poisonTypes, null))
				{
					groupChar.ChangePoisoned(context, poisonType, 3, ProfessionRelatedConstants.TravelerRandomPoisonValue(context.Random));
				}
				ObjectPool<List<sbyte>>.Instance.Return(poisonTypes);
				break;
			}
			case 2:
			{
				int addQiDisorder = ProfessionRelatedConstants.TravelerRandomQiDisorderValue(context.Random);
				groupChar.ChangeDisorderOfQiRandomRecovery(context, addQiDisorder);
				break;
			}
			case 3:
			{
				int reduceHealth = ProfessionRelatedConstants.TravelerRandomHealthValue(context.Random);
				groupChar.ChangeHealth(context, -reduceHealth);
				break;
			}
			}
		}

		// Token: 0x060079AA RID: 31146 RVA: 0x00476398 File Offset: 0x00474598
		[DomainMethod]
		public Location QueryFixedCharacterLocation(short templateId)
		{
			GameData.Domains.Character.Character character;
			return DomainManager.Character.TryGetFixedCharacterByTemplateId(templateId, out character) ? character.GetLocation() : Location.Invalid;
		}

		// Token: 0x060079AB RID: 31147 RVA: 0x004763C8 File Offset: 0x004745C8
		[DomainMethod]
		public Location QueryFixedCharacterLocationInArea(short templateId, short areaId)
		{
			Location location = this.QueryFixedCharacterLocation(templateId);
			return (location.AreaId == areaId) ? location : Location.Invalid;
		}

		// Token: 0x060079AC RID: 31148 RVA: 0x004763F4 File Offset: 0x004745F4
		[DomainMethod]
		public Location QueryTemplateBlockLocation(int templateId)
		{
			for (short areaId = 0; areaId < 45; areaId += 1)
			{
				Location location = this.QueryTemplateBlockLocationInArea(templateId, areaId);
				bool flag = location.IsValid();
				if (flag)
				{
					return location;
				}
			}
			return Location.Invalid;
		}

		// Token: 0x060079AD RID: 31149 RVA: 0x0047643C File Offset: 0x0047463C
		[DomainMethod]
		public Location QueryTemplateBlockLocationInArea(int templateId, short areaId)
		{
			bool flag = areaId < 0 || areaId >= 45;
			bool flag2 = flag;
			Location invalid;
			if (flag2)
			{
				invalid = Location.Invalid;
			}
			else
			{
				AreaBlockCollection areaBlocks = this._regularAreaBlocksArray[(int)areaId];
				MapBlockData[] blockArray = areaBlocks.GetArray();
				for (int i = 0; i < blockArray.Length; i++)
				{
					bool flag3 = (int)blockArray[i].TemplateId == templateId;
					if (flag3)
					{
						return new Location(areaId, blockArray[i].BlockId);
					}
				}
				invalid = Location.Invalid;
			}
			return invalid;
		}

		// Token: 0x060079AE RID: 31150 RVA: 0x004764C4 File Offset: 0x004746C4
		public unsafe void QueryRootBlocks(List<MapBlockData> rootBlocks, Location location)
		{
			rootBlocks.Clear();
			bool flag = !location.IsValid();
			if (!flag)
			{
				Span<MapBlockData> areaBlocks = this.GetAreaBlocks(location.AreaId);
				Span<MapBlockData> span = areaBlocks;
				for (int i = 0; i < span.Length; i++)
				{
					MapBlockData block = *span[i];
					bool flag2 = block.BlockId == location.BlockId || block.RootBlockId == location.BlockId;
					if (flag2)
					{
						rootBlocks.Add(block);
					}
				}
			}
		}

		// Token: 0x060079AF RID: 31151 RVA: 0x00476548 File Offset: 0x00474748
		public void QueryRegularBelongBlocks(List<MapBlockData> belongBlocks, Location location, bool includeSect, params MapDomain.MapBlockDataFilter[] extraFilters)
		{
			belongBlocks.Clear();
			bool flag = !location.IsValid();
			if (!flag)
			{
				bool flag2 = (int)location.AreaId >= this._regularAreaBlocksArray.Length;
				if (!flag2)
				{
					AreaBlockCollection areaBlocks = this._regularAreaBlocksArray[(int)location.AreaId];
					foreach (MapBlockData blockData in areaBlocks.GetArray())
					{
						bool flag3 = !MapDomain.IsFiltersPass(blockData, extraFilters) || !blockData.IsPassable();
						if (!flag3)
						{
							bool flag4 = includeSect && (blockData.BlockId == location.BlockId || blockData.RootBlockId == location.BlockId);
							if (flag4)
							{
								belongBlocks.Add(blockData);
							}
							else
							{
								bool flag5 = blockData.BelongBlockId == location.BlockId;
								if (flag5)
								{
									belongBlocks.Add(blockData);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060079B0 RID: 31152 RVA: 0x00476640 File Offset: 0x00474840
		public int QueryAreaBrokenLevel(short areaId)
		{
			bool flag = areaId < 45 || areaId >= 135;
			bool flag2 = flag;
			int result;
			if (flag2)
			{
				result = -1;
			}
			else
			{
				result = (int)DomainManager.Adventure.GetElement_BrokenAreaEnemies((int)(areaId - 45)).Level;
			}
			return result;
		}

		// Token: 0x060079B1 RID: 31153 RVA: 0x00476684 File Offset: 0x00474884
		private static bool IsFiltersPass(MapBlockData blockData, IEnumerable<MapDomain.MapBlockDataFilter> filters)
		{
			bool flag = filters == null;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				foreach (MapDomain.MapBlockDataFilter filter in filters)
				{
					bool flag2 = !filter(blockData);
					if (flag2)
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060079B2 RID: 31154 RVA: 0x004766F0 File Offset: 0x004748F0
		public static bool QueryFilterAnyCharacter(MapBlockData blockData)
		{
			return blockData.CharacterSet != null && blockData.CharacterSet.Count > 0;
		}

		// Token: 0x060079B3 RID: 31155 RVA: 0x0047671C File Offset: 0x0047491C
		public MapBlockData GetRandomMapBlockDataByFilters(IRandomSource random, sbyte stateTemplateId, sbyte areaFilterType, List<short> mapBlockSubTypes, bool includeBlockWithAdventure)
		{
			bool flag = mapBlockSubTypes != null && mapBlockSubTypes.Count > 0;
			if (flag)
			{
				EMapBlockSubType mapBlockSubType = (EMapBlockSubType)mapBlockSubTypes[random.Next(0, mapBlockSubTypes.Count)];
				bool flag2 = mapBlockSubType == EMapBlockSubType.TaiwuCun;
				if (flag2)
				{
					return this.GetBlock(DomainManager.Taiwu.GetTaiwuVillageLocation());
				}
				bool flag3 = mapBlockSubType > EMapBlockSubType.TaiwuCun && mapBlockSubType < EMapBlockSubType.Farmland;
				if (flag3)
				{
					for (short areaId = 0; areaId < 45; areaId += 1)
					{
						AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(areaId);
						short settlementMainBlockId = DomainManager.Map.GetMainSettlementMainBlockId(areaId);
						MapBlockData settlementMainBlock = DomainManager.Map.GetBlock(areaId, settlementMainBlockId);
						bool flag4 = settlementMainBlock.BlockSubType != mapBlockSubType;
						if (!flag4)
						{
							List<short> settlementBlocks = new List<short>();
							this.GetSettlementBlocks(areaId, settlementMainBlockId, settlementBlocks);
							for (int i = settlementBlocks.Count - 1; i >= 0; i--)
							{
								short settlementBlockId = settlementBlocks[i];
								bool flag5 = !includeBlockWithAdventure && adventuresInArea.AdventureSites.ContainsKey(settlementBlockId);
								if (flag5)
								{
									settlementBlocks.Remove(settlementBlockId);
								}
							}
							return this.GetBlock(areaId, settlementBlocks[random.Next(settlementBlocks.Count)]);
						}
					}
				}
			}
			List<short> areaList = ObjectPool<List<short>>.Instance.Get();
			areaList.Clear();
			bool flag6 = stateTemplateId == 0;
			if (flag6)
			{
				areaList.Add(135);
			}
			else
			{
				bool flag7 = stateTemplateId == -1;
				if (flag7)
				{
					stateTemplateId = (sbyte)random.Next(1, 16);
				}
				int normalAreaCount = 3;
				DomainManager.Map.GetAllAreaInState(stateTemplateId - 1, areaList);
				areaList.RemoveRange(normalAreaCount, 6);
			}
			MapStateItem stateConfig = MapState.Instance[stateTemplateId];
			if (!true)
			{
			}
			short num;
			switch (areaFilterType)
			{
			case -1:
				num = areaList[random.Next(0, areaList.Count)];
				break;
			case 0:
				num = DomainManager.Taiwu.GetTaiwuVillageLocation().AreaId;
				break;
			case 1:
				num = (short)stateConfig.MainAreaID;
				break;
			case 2:
				num = (short)stateConfig.SectAreaID;
				break;
			case 3:
				num = areaList.First((short area) => area != (short)stateConfig.MainAreaID && area != (short)stateConfig.SectAreaID);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unrecognized area filter type ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(areaFilterType);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			if (!true)
			{
			}
			short selectedAreaId = num;
			ObjectPool<List<short>>.Instance.Return(areaList);
			return this.GetRandomMapBlockDataInAreaByFilters(random, selectedAreaId, mapBlockSubTypes, includeBlockWithAdventure);
		}

		// Token: 0x060079B4 RID: 31156 RVA: 0x004769CC File Offset: 0x00474BCC
		public unsafe void GetMapBlocksInAreaByFilters(short areaId, Predicate<MapBlockData> predicate, List<MapBlockData> result)
		{
			result.Clear();
			Span<MapBlockData> areaBlocks = this.GetAreaBlocks(areaId);
			Span<MapBlockData> span = areaBlocks;
			for (int i = 0; i < span.Length; i++)
			{
				MapBlockData areaBlock = *span[i];
				bool flag = !areaBlock.IsPassable();
				if (!flag)
				{
					bool flag2 = DomainManager.Extra.IsLocationInDreamBack(areaBlock.GetLocation());
					if (!flag2)
					{
						bool flag3 = predicate(areaBlock);
						if (flag3)
						{
							result.Add(areaBlock);
						}
					}
				}
			}
		}

		// Token: 0x060079B5 RID: 31157 RVA: 0x00476A48 File Offset: 0x00474C48
		public MapBlockData GetRandomMapBlockDataInAreaByFilters(IRandomSource random, short areaId, IReadOnlyCollection<short> mapBlockSubTypes, bool includeBlocksWithAdventure)
		{
			MapDomain.<>c__DisplayClass325_0 CS$<>8__locals1 = new MapDomain.<>c__DisplayClass325_0();
			CS$<>8__locals1.includeBlocksWithAdventure = includeBlocksWithAdventure;
			CS$<>8__locals1.mapBlockSubTypes = mapBlockSubTypes;
			List<MapBlockData> selectableBlocks = ObjectPool<List<MapBlockData>>.Instance.Get();
			CS$<>8__locals1.adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(areaId).AdventureSites;
			Predicate<MapBlockData> predicate = (CS$<>8__locals1.mapBlockSubTypes != null && CS$<>8__locals1.mapBlockSubTypes.Count > 0) ? new Predicate<MapBlockData>(CS$<>8__locals1.<GetRandomMapBlockDataInAreaByFilters>g__IsBlockInList|1) : new Predicate<MapBlockData>(CS$<>8__locals1.<GetRandomMapBlockDataInAreaByFilters>g__IsBlockNormalOrWild|0);
			this.GetMapBlocksInAreaByFilters(areaId, predicate, selectableBlocks);
			MapBlockData block = (selectableBlocks.Count > 0) ? selectableBlocks[random.Next(selectableBlocks.Count)] : null;
			ObjectPool<List<MapBlockData>>.Instance.Return(selectableBlocks);
			return block;
		}

		// Token: 0x060079B6 RID: 31158 RVA: 0x00476AFC File Offset: 0x00474CFC
		public MapBlockData SelectBlockInCurrentOrNeighborState(IRandomSource random, Location centerLocation, Predicate<MapBlockData> condition, bool taiwuVillageInfluenceRangeIsLast = false)
		{
			MapBlockData blockData = this.SelectBlockInArea(random, centerLocation.AreaId, condition, taiwuVillageInfluenceRangeIsLast);
			bool flag = blockData != null;
			MapBlockData result;
			if (flag)
			{
				result = blockData;
			}
			else
			{
				sbyte stateId = DomainManager.Map.GetStateIdByAreaId(centerLocation.AreaId);
				blockData = this.SelectBlockInState(random, stateId, condition);
				bool flag2 = blockData != null;
				if (flag2)
				{
					result = blockData;
				}
				else
				{
					sbyte stateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(centerLocation.AreaId);
					sbyte[] neighborStateTemplateIds = MapState.Instance[stateTemplateId].NeighborStates;
					foreach (sbyte neighborStateTemplateId in neighborStateTemplateIds)
					{
						sbyte neighborStateId = DomainManager.Map.GetStateIdByStateTemplateId((short)neighborStateTemplateId);
						blockData = this.SelectBlockInState(random, neighborStateId, condition);
						bool flag3 = blockData != null;
						if (flag3)
						{
							return blockData;
						}
					}
					if (taiwuVillageInfluenceRangeIsLast)
					{
						blockData = this.SelectBlockInTaiwuVillageInfluenceRange(random, condition);
						bool flag4 = blockData != null;
						if (flag4)
						{
							return blockData;
						}
					}
					result = null;
				}
			}
			return result;
		}

		// Token: 0x060079B7 RID: 31159 RVA: 0x00476BF0 File Offset: 0x00474DF0
		private MapBlockData SelectBlockInState(IRandomSource random, sbyte stateId, Predicate<MapBlockData> condition)
		{
			List<short> areaIds = ObjectPool<List<short>>.Instance.Get();
			DomainManager.Map.GetAllRegularAreaInState(stateId, areaIds);
			CollectionUtils.Shuffle<short>(random, areaIds);
			MapBlockData blockData = null;
			foreach (short areaId in areaIds)
			{
				blockData = this.SelectBlockInArea(random, areaId, condition, false);
				bool flag = blockData != null;
				if (flag)
				{
					break;
				}
			}
			ObjectPool<List<short>>.Instance.Return(areaIds);
			return blockData;
		}

		// Token: 0x060079B8 RID: 31160 RVA: 0x00476C88 File Offset: 0x00474E88
		public MapBlockData SelectBlockInArea(IRandomSource random, short areaId, Predicate<MapBlockData> condition, bool exceptTaiwuVillageInfluenceRange = false)
		{
			List<MapBlockData> blocks = ObjectPool<List<MapBlockData>>.Instance.Get();
			DomainManager.Map.GetMapBlocksInAreaByFilters(areaId, condition, blocks);
			if (exceptTaiwuVillageInfluenceRange)
			{
				short settlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
				blocks.RemoveAll((MapBlockData b) => DomainManager.Map.IsLocationInSettlementInfluenceRange(b.GetLocation(), settlementId));
			}
			MapBlockData selectedBlock = blocks.GetRandomOrDefault(random, null);
			ObjectPool<List<MapBlockData>>.Instance.Return(blocks);
			return selectedBlock;
		}

		// Token: 0x060079B9 RID: 31161 RVA: 0x00476CFC File Offset: 0x00474EFC
		public MapBlockData SelectBlockInTaiwuVillageInfluenceRange(IRandomSource random, Predicate<MapBlockData> condition)
		{
			List<MapBlockData> blocks = ObjectPool<List<MapBlockData>>.Instance.Get();
			Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
			DomainManager.Map.GetMapBlocksInAreaByFilters(taiwuVillageLocation.AreaId, condition, blocks);
			short settlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
			blocks.RemoveAll((MapBlockData b) => !DomainManager.Map.IsLocationInSettlementInfluenceRange(b.GetLocation(), settlementId));
			MapBlockData selectedBlock = blocks.GetRandomOrDefault(random, null);
			ObjectPool<List<MapBlockData>>.Instance.Return(blocks);
			return selectedBlock;
		}

		// Token: 0x060079BA RID: 31162 RVA: 0x00476D78 File Offset: 0x00474F78
		public MapBlockData GetBelongSettlementBlock(Location location)
		{
			MapBlockData blockData = this.GetBlock(location);
			bool flag = blockData.IsCityTown();
			MapBlockData result;
			if (flag)
			{
				result = blockData.GetRootBlock();
			}
			else
			{
				bool flag2 = blockData.BelongBlockId >= 0;
				if (flag2)
				{
					MapBlockData belongBlock = this.GetBlock(new Location(location.AreaId, blockData.BelongBlockId));
					bool flag3 = belongBlock.IsCityTown();
					if (flag3)
					{
						return belongBlock;
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x060079BB RID: 31163 RVA: 0x00476DE4 File Offset: 0x00474FE4
		public bool IsLocationInSettlementInfluenceRange(Location location, short settlementId)
		{
			Location settlementLocation = DomainManager.Organization.GetSettlement(settlementId).GetLocation();
			bool flag = location.AreaId != settlementLocation.AreaId;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				MapBlockData blockData = this.GetBlock(location);
				result = (blockData.BlockId == settlementLocation.BlockId || blockData.RootBlockId == settlementLocation.BlockId || blockData.BelongBlockId == settlementLocation.BlockId);
			}
			return result;
		}

		// Token: 0x060079BC RID: 31164 RVA: 0x00476E58 File Offset: 0x00475058
		public bool IsLocationInOrganizationInfluenceRange(Location location, sbyte orgTemplateId)
		{
			MapBlockData block = this.GetBelongSettlementBlock(location);
			bool flag = block == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Settlement settlement = DomainManager.Organization.GetSettlementByLocation(block.GetLocation());
				result = (settlement.GetOrgTemplateId() == orgTemplateId);
			}
			return result;
		}

		// Token: 0x060079BD RID: 31165 RVA: 0x00476E98 File Offset: 0x00475098
		public bool IsLocationOnSettlementBlock(Location location, short settlementId)
		{
			MapBlockData block = this.GetBlock(location);
			MapBlockData rootBlock = (block != null) ? block.GetRootBlock() : null;
			Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
			bool flag = rootBlock == null || settlement == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Location settlementLocation = settlement.GetLocation();
				result = (rootBlock.AreaId == settlementLocation.AreaId && rootBlock.BlockId == settlementLocation.BlockId);
			}
			return result;
		}

		// Token: 0x060079BE RID: 31166 RVA: 0x00476F08 File Offset: 0x00475108
		public bool CheckLocationsHasSameRoot(Location locationA, Location locationB)
		{
			bool flag = !locationA.IsValid() || !locationB.IsValid();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = locationA == locationB;
				if (flag2)
				{
					result = true;
				}
				else
				{
					MapBlockData blockA = this.GetBlock(locationA);
					MapBlockData blockB = this.GetBlock(locationB);
					result = (blockA.GetRootBlock() == blockB.GetRootBlock());
				}
			}
			return result;
		}

		// Token: 0x060079BF RID: 31167 RVA: 0x00476F68 File Offset: 0x00475168
		public void GetSettlementBlocks(short areaId, short blockId, List<short> blockIds)
		{
			blockIds.Add(blockId);
			List<MapBlockData> groupBlocks = this.GetRegularAreaBlocks(areaId)[blockId].GroupBlockList;
			bool flag = groupBlocks == null;
			if (!flag)
			{
				int i = 0;
				int count = groupBlocks.Count;
				while (i < count)
				{
					blockIds.Add(groupBlocks[i].BlockId);
					i++;
				}
			}
		}

		// Token: 0x060079C0 RID: 31168 RVA: 0x00476FC8 File Offset: 0x004751C8
		public void GetSettlementBlocksWithoutAdventure(short areaId, short blockId, List<short> blockIds)
		{
			List<MapBlockData> groupBlocks = this.GetRegularAreaBlocks(areaId)[blockId].GroupBlockList;
			bool flag = groupBlocks == null;
			if (!flag)
			{
				AreaAdventureData adventures = DomainManager.Adventure.GetAdventuresInArea(areaId);
				int i = 0;
				int count = groupBlocks.Count;
				while (i < count)
				{
					bool flag2 = !adventures.AdventureSites.Keys.Contains(groupBlocks[i].BlockId);
					if (flag2)
					{
						blockIds.Add(groupBlocks[i].BlockId);
					}
					i++;
				}
				bool flag3 = !adventures.AdventureSites.Keys.Contains(blockId);
				if (flag3)
				{
					blockIds.Add(blockId);
				}
			}
		}

		// Token: 0x060079C1 RID: 31169 RVA: 0x0047707C File Offset: 0x0047527C
		public void GetSettlementBlocksAndAffiliatedBlocks(short areaId, short blockId, List<short> blockIds)
		{
			blockIds.Add(blockId);
			AreaBlockCollection areaBlocks = this.GetRegularAreaBlocks(areaId);
			int i = 0;
			int count = areaBlocks.Count;
			while (i < count)
			{
				MapBlockData block = areaBlocks[(short)i];
				bool flag = block.RootBlockId == blockId || (block.BelongBlockId == blockId && block.IsPassable());
				if (flag)
				{
					blockIds.Add(block.BlockId);
				}
				i++;
			}
		}

		// Token: 0x060079C2 RID: 31170 RVA: 0x004770F0 File Offset: 0x004752F0
		public void GetSettlementAffiliatedBlocks(short areaId, short blockId, List<MapBlockData> blocks)
		{
			blocks.Clear();
			AreaBlockCollection areaBlocks = this.GetRegularAreaBlocks(areaId);
			int i = 0;
			int count = areaBlocks.Count;
			while (i < count)
			{
				MapBlockData block = areaBlocks[(short)i];
				bool flag = block.BelongBlockId == blockId && block.IsPassable();
				if (flag)
				{
					blocks.Add(block);
				}
				i++;
			}
		}

		// Token: 0x060079C3 RID: 31171 RVA: 0x00477154 File Offset: 0x00475354
		[DomainMethod]
		public unsafe Dictionary<short, short> GetAllSettlementInfluenceRangeBlocks(short areaId)
		{
			Span<MapBlockData> blocks = this.GetAreaBlocks(areaId);
			Dictionary<short, short> res = new Dictionary<short, short>();
			Span<MapBlockData> span = blocks;
			for (int i = 0; i < span.Length; i++)
			{
				MapBlockData block = *span[i];
				MapBlockData settlementBlock = this.GetBelongSettlementBlock(block.GetLocation());
				bool flag = settlementBlock != null;
				if (flag)
				{
					res.Add(block.BlockId, settlementBlock.BlockId);
				}
			}
			return res;
		}

		// Token: 0x060079C4 RID: 31172 RVA: 0x004771C8 File Offset: 0x004753C8
		[Obsolete("Use IsLocationInSettlementInfluenceRange")]
		[DomainMethod]
		public bool IsLocationInBuildingEffectRange(Location location, Location settlementLocation)
		{
			bool flag = location.AreaId != settlementLocation.AreaId;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				MapBlockData blockData = this.GetBlock(location);
				result = (blockData.BlockId == settlementLocation.BlockId || blockData.RootBlockId == settlementLocation.BlockId || blockData.BelongBlockId == settlementLocation.BlockId);
			}
			return result;
		}

		// Token: 0x060079C5 RID: 31173 RVA: 0x00477228 File Offset: 0x00475428
		[Obsolete("Use IsLocationOnSettlementBlock instead.")]
		public bool IsLocationInSettlementRange(Location location, sbyte organizationTemplateId)
		{
			return this.IsLocationOnSettlementBlock(location, DomainManager.Organization.GetSettlementIdByOrgTemplateId(organizationTemplateId));
		}

		// Token: 0x060079C6 RID: 31174 RVA: 0x0047723C File Offset: 0x0047543C
		[Obsolete("Use IsLocationOnSettlementBlock instead.")]
		public bool BelongSettlementBlock(short areaId, short blockId, Location location)
		{
			bool flag = areaId != location.AreaId;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = blockId == location.BlockId;
				if (flag2)
				{
					result = true;
				}
				else
				{
					List<MapBlockData> groupBlocks = this.GetRegularAreaBlocks(areaId)[blockId].GroupBlockList;
					bool flag3 = groupBlocks == null;
					if (flag3)
					{
						result = false;
					}
					else
					{
						int i = 0;
						int count = groupBlocks.Count;
						while (i < count)
						{
							bool flag4 = groupBlocks[i].BlockId == location.BlockId;
							if (flag4)
							{
								return true;
							}
							i++;
						}
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x060079C7 RID: 31175 RVA: 0x004772D4 File Offset: 0x004754D4
		[Obsolete("Use IsLocationInSettlementInfluenceRange instead.")]
		public bool BelongSettlementBlocksAndAffiliatedBlocks(short areaId, short blockId, Location location)
		{
			List<short> blockIds = ObjectPool<List<short>>.Instance.Get();
			blockIds.Clear();
			this.GetSettlementBlocksAndAffiliatedBlocks(areaId, blockId, blockIds);
			bool flag = areaId == location.AreaId && blockIds.Contains(location.BlockId);
			bool result;
			if (flag)
			{
				ObjectPool<List<short>>.Instance.Return(blockIds);
				result = true;
			}
			else
			{
				ObjectPool<List<short>>.Instance.Return(blockIds);
				result = false;
			}
			return result;
		}

		// Token: 0x060079C8 RID: 31176 RVA: 0x0047733D File Offset: 0x0047553D
		[Obsolete("Use IsLocationInSettlementInfluenceRange")]
		public bool IsLocationAtSettlementRange(Location currLocation, short settlementId)
		{
			return this.IsLocationInSettlementInfluenceRange(currLocation, settlementId);
		}

		// Token: 0x060079C9 RID: 31177 RVA: 0x00477347 File Offset: 0x00475547
		public void InitializeSpecialBlocksData()
		{
			this.UpdateWudangHeavenlyTreeLocations();
			this.UpdateFulongFlameLocations();
		}

		// Token: 0x060079CA RID: 31178 RVA: 0x00477358 File Offset: 0x00475558
		public void UpdateWudangHeavenlyTreeLocations()
		{
			List<SectStoryHeavenlyTreeExtendable> trees = DomainManager.Extra.GetAllHeavenlyTrees();
			this._wudangHeavenlyTrees.Clear();
			foreach (SectStoryHeavenlyTreeExtendable tree in trees)
			{
				this._wudangHeavenlyTrees.Add(tree.Location);
			}
		}

		// Token: 0x060079CB RID: 31179 RVA: 0x004773D0 File Offset: 0x004755D0
		public void UpdateFulongFlameLocations()
		{
			List<FulongInFlameArea> flameAreas = DomainManager.Extra.GetAllFulongInFlameAreas();
			this._fulongLightedBlocks.Clear();
			foreach (FulongInFlameArea area in flameAreas)
			{
				foreach (short blockId in area.LightedBlocks.Keys)
				{
					this._fulongLightedBlocks.Add((int)blockId);
				}
			}
		}

		// Token: 0x060079CC RID: 31180 RVA: 0x00477488 File Offset: 0x00475688
		public bool IsBlockOccupiedByCriticalAdventure(short areaId, short blockId)
		{
			AdventureSiteData conflictSite;
			return DomainManager.Adventure.GetElement_AdventureAreas((int)areaId).AdventureSites.TryGetValue(blockId, out conflictSite) && !Config.AdventureType.Instance[Adventure.Instance[conflictSite.TemplateId].Type].IsTrivial;
		}

		// Token: 0x060079CD RID: 31181 RVA: 0x004774E0 File Offset: 0x004756E0
		public bool IsBlockSpecial(MapBlockData mapBlockData, bool strictCheck = true)
		{
			bool flag = FiveLoongDlcEntry.IsBlockLoongBlock(mapBlockData);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = this.IsLocationInFulongFlameArea(mapBlockData.GetLocation());
				if (flag2)
				{
					result = true;
				}
				else
				{
					if (strictCheck)
					{
						bool flag3 = this._wudangHeavenlyTrees.Contains(mapBlockData.GetLocation());
						if (flag3)
						{
							return true;
						}
					}
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060079CE RID: 31182 RVA: 0x00477538 File Offset: 0x00475738
		public bool IsLocationInFulongFlameArea(Location location)
		{
			List<FulongInFlameArea> flameAreas = DomainManager.Extra.GetAllFulongInFlameAreas();
			foreach (FulongInFlameArea area in flameAreas)
			{
				bool flag = area.IsLocationInActiveFlame(location);
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060079CF RID: 31183 RVA: 0x004775A4 File Offset: 0x004757A4
		public bool IsBlockAvailable(MapBlockData mapBlockData, bool strictCheck)
		{
			return mapBlockData.IsNonDeveloped() && !mapBlockData.Destroyed && !this.IsBlockSpecial(mapBlockData, strictCheck) && mapBlockData.GetConfig().TemplateId != 126 && mapBlockData.IsPassable() && !this.IsBlockOccupiedByCriticalAdventure(mapBlockData.AreaId, mapBlockData.BlockId) && (!strictCheck || mapBlockData.GetConfig().TemplateId != 124);
		}

		// Token: 0x060079D0 RID: 31184 RVA: 0x00477618 File Offset: 0x00475818
		public void RemoveTrivialObjectsOnBlocks(DataContext context, List<MapBlockData> mapBlocks)
		{
			foreach (MapBlockData block in mapBlocks)
			{
				Location location = block.GetLocation();
				List<int> animals;
				bool flag = DomainManager.Extra.TryGetAnimalIdsByLocation(location, out animals);
				if (flag)
				{
					for (int index = animals.Count - 1; index >= 0; index--)
					{
						int animalId = animals[index];
						DomainManager.Extra.ApplyAnimalDeadByAccident(context, animalId);
					}
				}
				AdventureSiteData adventureSiteData;
				bool flag2 = DomainManager.Adventure.GetElement_AdventureAreas((int)location.AreaId).AdventureSites.TryGetValue(location.BlockId, out adventureSiteData);
				if (flag2)
				{
					DomainManager.Adventure.RemoveAdventureSite(context, location.AreaId, location.BlockId, false, false);
				}
				List<MapTemplateEnemyInfo> templateEnemyList = block.TemplateEnemyList;
				bool flag3 = templateEnemyList != null && templateEnemyList.Count > 0;
				if (flag3)
				{
					for (int index2 = block.TemplateEnemyList.Count - 1; index2 >= 0; index2--)
					{
						MapTemplateEnemyInfo enemyInfo = block.TemplateEnemyList[index2];
						Events.RaiseTemplateEnemyLocationChanged(context, enemyInfo, location, Location.Invalid);
					}
				}
				bool flag4 = this.LocationHasCricket(context, location);
				if (flag4)
				{
					int index3 = Array.IndexOf<short>(this._cricketPlaceData[(int)location.AreaId].CricketBlocks, location.BlockId);
					this._cricketPlaceData[(int)location.AreaId].CricketTriggered[index3] = true;
					this.SetElement_CricketPlaceData((int)location.AreaId, this._cricketPlaceData[(int)location.AreaId], context);
				}
			}
		}

		// Token: 0x060079D1 RID: 31185 RVA: 0x004777D4 File Offset: 0x004759D4
		public bool TryGetAvailableBlockIdInRange(short areaId, int range, List<MapBlockData> neighborBlocks)
		{
			Span<MapBlockData> mapBlocks = DomainManager.Map.GetAreaBlocks(areaId);
			HashSet<short> visited = new HashSet<short>();
			return this.TryGetAvailableBlockIdInRange(areaId, range, true, mapBlocks, visited, neighborBlocks) || this.TryGetAvailableBlockIdInRange(areaId, range, false, mapBlocks, visited, neighborBlocks);
		}

		// Token: 0x060079D2 RID: 31186 RVA: 0x00477818 File Offset: 0x00475A18
		private unsafe bool TryGetAvailableBlockIdInRange(short areaId, int range, bool isStrict, Span<MapBlockData> mapBlocks, HashSet<short> visited, List<MapBlockData> neighborBlocks)
		{
			int extraRange = range + (isStrict ? 2 : 1);
			byte areaSize = this.GetAreaSize(areaId);
			visited.Clear();
			Span<MapBlockData> span = mapBlocks;
			for (int i = 0; i < span.Length; i++)
			{
				MapBlockData centerBlock = *span[i];
				bool ok = true;
				ByteCoordinate blockPos = ByteCoordinate.IndexToCoordinate(centerBlock.BlockId, areaSize);
				neighborBlocks.Clear();
				for (int x = (int)blockPos.X - extraRange; x <= (int)blockPos.X + extraRange; x++)
				{
					bool flag = !ok;
					if (flag)
					{
						break;
					}
					for (int y = (int)blockPos.Y - extraRange; y <= (int)blockPos.Y + extraRange; y++)
					{
						bool flag2 = x < 0 || x >= (int)areaSize || y < 0 || y >= (int)areaSize;
						if (flag2)
						{
							ok = false;
							break;
						}
						ByteCoordinate pos = new ByteCoordinate((byte)x, (byte)y);
						int distance = blockPos.GetManhattanDistance(pos);
						bool flag3 = distance > extraRange;
						if (!flag3)
						{
							MapBlockData neighborBlock = *mapBlocks[(int)ByteCoordinate.CoordinateToIndex(pos, areaSize)];
							bool flag4 = neighborBlocks.Contains(neighborBlock);
							if (!flag4)
							{
								bool flag5 = visited.Contains(neighborBlock.BlockId);
								if (flag5)
								{
									ok = false;
									break;
								}
								bool flag6 = !this.IsBlockAvailable(neighborBlock, isStrict);
								if (flag6)
								{
									visited.Add(neighborBlock.BlockId);
									ok = false;
									break;
								}
								bool flag7 = distance <= range;
								if (flag7)
								{
									neighborBlocks.Add(neighborBlock);
								}
							}
						}
					}
				}
				bool flag8 = ok;
				if (flag8)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060079D3 RID: 31187 RVA: 0x004779D3 File Offset: 0x00475BD3
		public void IncreaseMoveBanned(DataContext context)
		{
			this.SetMoveBanned(this._moveBanned + 1, context);
		}

		// Token: 0x060079D4 RID: 31188 RVA: 0x004779E8 File Offset: 0x00475BE8
		public void DecreaseMoveBanned(DataContext context)
		{
			this.SetMoveBanned(this._moveBanned - 1, context);
			bool flag = this._moveBanned < 0;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Move banned count less than zero, current value is ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._moveBanned);
				AdaptableLog.Warning(defaultInterpolatedStringHandler.ToStringAndClear(), false);
			}
		}

		// Token: 0x060079D5 RID: 31189 RVA: 0x00477A48 File Offset: 0x00475C48
		public void Move(DataContext context, short destBlockId, bool notCostTime)
		{
			Logger logger = MapDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Move to ");
			defaultInterpolatedStringHandler.AppendFormatted<short>(destBlockId);
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			Location srcLocation = taiwuChar.GetLocation();
			this.TaiwuLastLocation = srcLocation;
			Location destLocation = new Location(srcLocation.AreaId, destBlockId);
			MapAreaData areaData = this._areas[(int)srcLocation.AreaId];
			MapBlockData srcBlock = this.GetBlock(srcLocation);
			MapBlockData destBlock = this.GetBlock(destLocation);
			byte areaSize = this.GetAreaSize(srcLocation.AreaId);
			MapBlockItem blockConfig = destBlock.GetConfig();
			int requiredActionPoints = (int)blockConfig.MoveCost * DomainManager.Taiwu.GetMoveTimeCostPercent() / 10;
			requiredActionPoints = Math.Max(1, requiredActionPoints);
			ByteCoordinate fromByteCoordinate = ByteCoordinate.IndexToCoordinate(srcLocation.BlockId, areaSize);
			ByteCoordinate toByteCoordinate = ByteCoordinate.IndexToCoordinate(destBlockId, areaSize);
			bool isNeighbor = Math.Abs((int)(toByteCoordinate.X - fromByteCoordinate.X)) + Math.Abs((int)(toByteCoordinate.Y - fromByteCoordinate.Y)) == 1;
			bool flag = fromByteCoordinate == toByteCoordinate;
			if (!flag)
			{
				bool inGuiding = DomainManager.TutorialChapter.InGuiding;
				if (inGuiding)
				{
					Location forceNextLocation = DomainManager.TutorialChapter.GetNextForceLocation();
					bool flag2 = forceNextLocation != Location.Invalid && forceNextLocation != destLocation;
					if (flag2)
					{
						return;
					}
				}
				bool flag3 = !destBlock.IsPassable();
				if (flag3)
				{
					short predefinedLogId = 11;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Block is not passable: ");
					defaultInterpolatedStringHandler.AppendFormatted<short>(destBlockId);
					PredefinedLog.Show(predefinedLogId, defaultInterpolatedStringHandler.ToStringAndClear());
				}
				else
				{
					bool flag4 = !isNeighbor && !this._teleportMove;
					if (flag4)
					{
						short predefinedLogId2 = 11;
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Can only move to a neighbor block: src=");
						defaultInterpolatedStringHandler.AppendFormatted<short>(srcLocation.BlockId);
						defaultInterpolatedStringHandler.AppendLiteral(", dst=");
						defaultInterpolatedStringHandler.AppendFormatted<short>(destBlockId);
						PredefinedLog.Show(predefinedLogId2, defaultInterpolatedStringHandler.ToStringAndClear());
					}
					else
					{
						bool flag5 = !DomainManager.Extra.IsActionPointEnough(requiredActionPoints) && !notCostTime;
						if (flag5)
						{
							short predefinedLogId3 = 11;
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Cannot advance days across month: ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(requiredActionPoints);
							defaultInterpolatedStringHandler.AppendLiteral(", left days: ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(DomainManager.Extra.GetTotalActionPointsRemaining());
							PredefinedLog.Show(predefinedLogId3, defaultInterpolatedStringHandler.ToStringAndClear());
						}
						else
						{
							HashSet<int> groupCharIds = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
							foreach (int charId in groupCharIds)
							{
								GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
								character.SetLocation(destLocation, context);
							}
							DomainManager.Extra.GearMateFollowTaiwu(context);
							ProfessionData professionData = DomainManager.Extra.GetProfessionData(11);
							TravelerSkillsData skillsData = professionData.GetSkillsData<TravelerSkillsData>();
							int addSeniority = skillsData.RecordMovementConsumedActionPoints(requiredActionPoints);
							bool flag6 = addSeniority > 0;
							if (flag6)
							{
								DomainManager.Extra.ChangeProfessionSeniority(context, 11, addSeniority, true, false);
							}
							else
							{
								DomainManager.Extra.SetProfessionData(context, professionData);
							}
							this.SetBlockAndViewRangeVisibleByMove(context, destBlock);
							MapDomain.AddAnimalProfessionSeniority(context, destLocation);
							bool flag7 = destBlock.IsCityTown();
							if (flag7)
							{
								short settlementId = areaData.SettlementInfos[areaData.GetSettlementIndex(destBlock.GetRootBlock().BlockId)].SettlementId;
								DomainManager.Taiwu.TryAddVisitedSettlement(settlementId, context);
								Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
								Location location = settlement.GetLocation();
								MapBlockData mapBlockData = DomainManager.Map.GetBlock(location);
								short mapBlockTemplateId = mapBlockData.GetConfig().TemplateId;
								bool flag8 = !DomainManager.Extra.CheckIsUnlockedSectXuannvMusicByMapBlockId(mapBlockTemplateId);
								if (flag8)
								{
									DomainManager.Extra.UnlockSectXuannvMusicByMapBlockId(context, mapBlockTemplateId);
								}
							}
							bool flag9 = blockConfig.TemplateId == 124;
							if (flag9)
							{
								List<sbyte> bodyPartRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
								IEnumerable<int> damagedCharIds = groupCharIds;
								bool flag10 = taiwuChar.IsActiveExternalRelationState(2);
								if (flag10)
								{
									damagedCharIds = damagedCharIds.Union(from c in DomainManager.Character.GetKidnappedCharacters(taiwuChar.GetId()).GetCollection()
									select c.CharId);
								}
								foreach (int charId2 in damagedCharIds)
								{
									GameData.Domains.Character.Character character2 = DomainManager.Character.GetElement_Objects(charId2);
									Injuries injuries = character2.GetInjuries();
									bodyPartRandomPool.Clear();
									bodyPartRandomPool.AddRange(BodyPart.Instance.GetAllKeys());
									bodyPartRandomPool.RemoveAll((sbyte part) => injuries.Get(part, false) >= 6);
									bool flag11 = bodyPartRandomPool.Count > 0;
									if (flag11)
									{
										injuries.Change(bodyPartRandomPool[context.Random.Next(bodyPartRandomPool.Count)], false, 1);
										character2.SetInjuries(injuries, context);
									}
									this.ReduceCharCarrierDurability(context, charId2);
								}
								ObjectPool<List<sbyte>>.Instance.Return(bodyPartRandomPool);
							}
							else
							{
								bool flag12 = blockConfig.SubType == EMapBlockSubType.Ruin || destBlock.Destroyed;
								if (flag12)
								{
									foreach (int charId3 in groupCharIds)
									{
										this.ReduceCharCarrierDurability(context, charId3);
									}
								}
							}
							this.UpdateIsTaiwuInFulongFlameArea(context);
							bool flag13 = !notCostTime;
							if (flag13)
							{
								DomainManager.World.ConsumeActionPoint(context, requiredActionPoints);
							}
							Events.RaiseTaiwuMove(context, srcBlock, destBlock, requiredActionPoints);
						}
					}
				}
			}
		}

		// Token: 0x060079D6 RID: 31190 RVA: 0x00478030 File Offset: 0x00476230
		[DomainMethod]
		public void Move(DataContext context, short destBlockId)
		{
			this.Move(context, destBlockId, this._lockMoveTime || this._teleportMove || this._crossArchiveLockMoveTime);
		}

		// Token: 0x060079D7 RID: 31191 RVA: 0x00478058 File Offset: 0x00476258
		[DomainMethod]
		public void MoveFinish(DataContext context, Location previous, Location current)
		{
			bool flag = !DomainManager.Map.GetCrossArchiveLockMoveTime();
			if (flag)
			{
				MapBlockData currentBlockData = this.GetBlockData(current.AreaId, current.BlockId);
				bool flag2 = currentBlockData != null;
				if (flag2)
				{
					MapBlockItem blockConfig = currentBlockData.GetConfig();
					bool flag3 = blockConfig != null;
					if (flag3)
					{
						bool flag4 = blockConfig.TemplateId == 124;
						if (flag4)
						{
							DomainManager.World.GetInstantNotifications().AddWalkThroughAbyss();
						}
						else
						{
							bool flag5 = blockConfig.SubType == EMapBlockSubType.Ruin && this.IsAnyGroupCharEquippingCarrier();
							if (flag5)
							{
								DomainManager.World.GetInstantNotifications().AddWalkThroughErosionBlock();
							}
						}
					}
					bool flag6 = currentBlockData.Destroyed && this.IsAnyGroupCharEquippingCarrier();
					if (flag6)
					{
						DomainManager.World.GetInstantNotifications().AddWalkThroughDestroyBlock();
					}
				}
			}
			DomainManager.Extra.UpdateFollowMovementCharacters(context);
			this.RefreshTaiwuMoveRecord(previous);
			DomainManager.TaiwuEvent.OnEvent_TaiwuBlockChanged(previous, current);
		}

		// Token: 0x060079D8 RID: 31192 RVA: 0x00478144 File Offset: 0x00476344
		private void RefreshTaiwuMoveRecord(Location previous)
		{
			this.TaiwuMoveRecord.Enqueue(previous);
			while (this.TaiwuMoveRecord.Count > 3)
			{
				this.TaiwuMoveRecord.Dequeue();
			}
		}

		// Token: 0x060079D9 RID: 31193 RVA: 0x00478184 File Offset: 0x00476384
		[DomainMethod]
		public bool IsContinuousMovingBreak()
		{
			return DomainManager.TaiwuEvent.IsShowingEvent || DomainManager.TaiwuEvent.GetHasListeningEvent();
		}

		// Token: 0x060079DA RID: 31194 RVA: 0x004781B0 File Offset: 0x004763B0
		[DomainMethod]
		public MapHealSimulateResult SimulateHealCost(int typeInt, int doctorId, int patientId, bool needPay = false, bool isExpensiveHeal = false)
		{
			bool flag = GameData.Domains.Character.Character.AllHealActions.Contains((EHealActionType)typeInt);
			MapHealSimulateResult result;
			if (flag)
			{
				result = this.SimulateHealCost((EHealActionType)typeInt, doctorId, patientId, needPay, isExpensiveHeal);
			}
			else
			{
				Logger logger = MapDomain.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("SimulateHealCost by invalid type ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(typeInt);
				logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				result = default(MapHealSimulateResult);
			}
			return result;
		}

		// Token: 0x060079DB RID: 31195 RVA: 0x00478220 File Offset: 0x00476420
		[DomainMethod]
		public bool HealOnMap(DataContext context, int typeInt, int doctorId, int patientId, bool needPay = false, int payerId = -1, bool isExpensiveHeal = false)
		{
			bool flag = GameData.Domains.Character.Character.AllHealActions.Contains((EHealActionType)typeInt);
			bool result;
			if (flag)
			{
				result = this.HealOnMap(context, (EHealActionType)typeInt, doctorId, patientId, needPay, payerId, isExpensiveHeal);
			}
			else
			{
				Logger logger = MapDomain.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 1);
				defaultInterpolatedStringHandler.AppendLiteral("HealOnMap by invalid type ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(typeInt);
				logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				result = false;
			}
			return result;
		}

		// Token: 0x060079DC RID: 31196 RVA: 0x0047828C File Offset: 0x0047648C
		private void ReduceCharCarrierDurability(DataContext context, int charId)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			ItemKey carrier = character.GetEquipment()[11];
			bool flag = carrier.IsValid();
			if (flag)
			{
				int reduceDurability = -context.Random.Next(GlobalConfig.CarrierDurationReduceOnRuinBlock[0], GlobalConfig.CarrierDurationReduceOnRuinBlock[1]);
				ItemBase baseItem = DomainManager.Item.GetBaseItem(carrier);
				short currDurability = baseItem.GetCurrDurability();
				short durability = (short)Math.Clamp(reduceDurability + (int)currDurability, 0, (int)baseItem.GetMaxDurability());
				baseItem.SetCurrDurability(durability, context);
			}
		}

		// Token: 0x060079DD RID: 31197 RVA: 0x00478314 File Offset: 0x00476514
		private bool IsAnyGroupCharEquippingCarrier()
		{
			HashSet<int> groupCharIds = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
			foreach (int charId in groupCharIds)
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
				ItemKey carrier = character.GetEquipment()[11];
				bool flag = carrier.IsValid();
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060079DE RID: 31198 RVA: 0x004783AC File Offset: 0x004765AC
		private MapHealSimulateResult SimulateHealCost(EHealActionType type, int doctorId, int patientId, bool needPay = false, bool isExpensiveHeal = false)
		{
			GameData.Domains.Character.Character doctorChar = DomainManager.Character.GetElement_Objects(doctorId);
			GameData.Domains.Character.Character patientChar = DomainManager.Character.GetElement_Objects(patientId);
			int maxRequireAttainment;
			int healEffect = doctorChar.CalcHealEffect(type, patientChar, out maxRequireAttainment, isExpensiveHeal);
			int costHerb = patientChar.CalcHealCostHerb(type, isExpensiveHeal);
			int costMoney = needPay ? patientChar.CalcHealCostMoney(type, doctorChar.GetBehaviorType(), isExpensiveHeal) : 0;
			int costSpiritualDebt = isExpensiveHeal ? patientChar.CalcHealCostSpiritualDebt(type) : 0;
			return new MapHealSimulateResult(type, costHerb, costMoney, healEffect, costSpiritualDebt, maxRequireAttainment);
		}

		// Token: 0x060079DF RID: 31199 RVA: 0x00478428 File Offset: 0x00476628
		private bool HealOnMap(DataContext context, EHealActionType type, int doctorId, int patientId, bool needPay = false, int payerId = -1, bool isExpensiveHeal = false)
		{
			GameData.Domains.Character.Character doctorChar = DomainManager.Character.GetElement_Objects(doctorId);
			GameData.Domains.Character.Character patientChar = DomainManager.Character.GetElement_Objects(patientId);
			bool flag = DomainManager.Character.GetUsableCombatResources(doctorId).Get(type) <= 0;
			bool result;
			if (flag)
			{
				MapDomain.Logger.Warn("Heal count not enough");
				result = false;
			}
			else
			{
				int costHerb = patientChar.CalcHealCostHerb(type, isExpensiveHeal);
				bool flag2 = doctorChar.GetResource(5) < costHerb;
				if (flag2)
				{
					Logger logger = MapDomain.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Herb not enough. need ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(costHerb);
					defaultInterpolatedStringHandler.AppendLiteral(", has ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(doctorChar.GetResource(5));
					logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					result = false;
				}
				else
				{
					int costMoney = needPay ? patientChar.CalcHealCostMoney(type, doctorChar.GetBehaviorType(), isExpensiveHeal) : 0;
					GameData.Domains.Character.Character payerChar = needPay ? DomainManager.Character.GetElement_Objects(payerId) : null;
					bool flag3 = costMoney > 0 && payerChar != null && payerChar.GetResource(6) < costMoney;
					if (flag3)
					{
						Logger logger2 = MapDomain.Logger;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Money not enough. need ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(costMoney);
						defaultInterpolatedStringHandler.AppendLiteral(", has ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(payerChar.GetResource(6));
						logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
						result = false;
					}
					else
					{
						if (isExpensiveHeal)
						{
							GameData.Domains.Character.Character doctor = DomainManager.Character.GetElement_Objects(doctorId);
							Settlement settlement = DomainManager.Organization.GetSettlement(doctor.GetOrganizationInfo().SettlementId);
							bool flag4 = settlement == null;
							if (flag4)
							{
								Logger logger3 = MapDomain.Logger;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 1);
								defaultInterpolatedStringHandler.AppendLiteral("DoctorId:");
								defaultInterpolatedStringHandler.AppendFormatted<int>(doctorId);
								defaultInterpolatedStringHandler.AppendLiteral(", Settlement  is null");
								logger3.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
								return false;
							}
							int costSpiritualDebt = patientChar.CalcHealCostSpiritualDebt(type);
							DomainManager.Extra.ChangeAreaSpiritualDebt(context, settlement.GetLocation().AreaId, -costSpiritualDebt, true, true);
						}
						bool flag5 = DomainManager.World.GetLeftDaysInCurrMonth() == 0;
						if (flag5)
						{
							MapDomain.Logger.Warn("Time not enough to heal");
							result = false;
						}
						else
						{
							DomainManager.Character.UseCombatResources(context, doctorId, type, 1);
							bool flag6 = costHerb > 0;
							if (flag6)
							{
								doctorChar.ChangeResource(context, 5, -costHerb);
							}
							bool flag7 = costMoney > 0 && payerChar != null;
							if (flag7)
							{
								payerChar.ChangeResource(context, 6, -costMoney);
								doctorChar.ChangeResource(context, 6, costMoney);
							}
							DomainManager.World.AdvanceDaysInMonth(context, 1);
							doctorChar.DoHealAction(context, type, patientChar, doctorId == DomainManager.Taiwu.GetTaiwuCharId(), isExpensiveHeal);
							result = true;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060079E0 RID: 31200 RVA: 0x004786EC File Offset: 0x004768EC
		public void UpdateIsTaiwuInFulongFlameArea(DataContext context)
		{
			bool flag = !this.IsLocationInFulongFlameArea(DomainManager.Taiwu.GetTaiwu().GetLocation());
			if (flag)
			{
				bool isTaiwuInFulongFlameArea = this._isTaiwuInFulongFlameArea;
				if (isTaiwuInFulongFlameArea)
				{
					this._isTaiwuInFulongFlameArea = false;
					this.SetIsTaiwuInFulongFlameArea(this._isTaiwuInFulongFlameArea, context);
				}
			}
			else
			{
				bool flag2 = !this._isTaiwuInFulongFlameArea;
				if (flag2)
				{
					this._canTriggerFulongFlameTeammateBubble = true;
					this._isTaiwuInFulongFlameArea = true;
					this.SetIsTaiwuInFulongFlameArea(this._isTaiwuInFulongFlameArea, context);
				}
			}
		}

		// Token: 0x060079E1 RID: 31201 RVA: 0x00478766 File Offset: 0x00476966
		public void SetTeleportMove(bool teleport)
		{
			this._teleportMove = teleport;
		}

		// Token: 0x060079E2 RID: 31202 RVA: 0x00478770 File Offset: 0x00476970
		public int GetTaiwuViewRange(MapBlockData blockData)
		{
			int viewRange = (int)blockData.GetConfig().ViewRange;
			bool flag = DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(36);
			if (flag)
			{
				viewRange += DomainManager.Extra.GetProfessionData(11).GetSeniorityVisionRangeBonus();
			}
			return viewRange;
		}

		// Token: 0x060079E3 RID: 31203 RVA: 0x004787B4 File Offset: 0x004769B4
		public unsafe void SetBlockAndViewRangeVisibleByMove(DataContext context, MapBlockData blockData)
		{
			List<MapBlockData> areaInvisibleBlocks = ObjectPool<List<MapBlockData>>.Instance.Get();
			areaInvisibleBlocks.Clear();
			Span<MapBlockData> areaBlocks = this.GetAreaBlocks(blockData.AreaId);
			for (int i = 0; i < areaBlocks.Length; i++)
			{
				MapBlockData block = *areaBlocks[i];
				bool flag = !block.Visible;
				if (flag)
				{
					areaInvisibleBlocks.Add(block);
				}
			}
			int viewRange = this.GetTaiwuViewRange(blockData);
			this.SetBlockAndNeighborVisible(context, blockData, viewRange);
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(11);
			TravelerSkillsData skillsData = professionData.GetSkillsData<TravelerSkillsData>();
			bool changed = false;
			foreach (MapBlockData block2 in areaInvisibleBlocks)
			{
				bool flag2 = !block2.Visible;
				if (!flag2)
				{
					int addSeniority = skillsData.RecordExploredMapBlock((int)(block2.GetConfig().MoveCost * 10));
					bool flag3 = addSeniority > 0;
					if (flag3)
					{
						DomainManager.Extra.ChangeProfessionSeniority(context, 11, addSeniority, true, false);
						changed = false;
					}
					else
					{
						changed = true;
					}
				}
			}
			bool flag4 = changed;
			if (flag4)
			{
				DomainManager.Extra.SetProfessionData(context, professionData);
			}
			ObjectPool<List<MapBlockData>>.Instance.Return(areaInvisibleBlocks);
		}

		// Token: 0x060079E4 RID: 31204 RVA: 0x004788F8 File Offset: 0x00476AF8
		public void SetBlockAndViewRangeVisible(DataContext context, short areaId, short blockId)
		{
			MapBlockData block = this.GetBlock(areaId, blockId);
			this.SetBlockAndNeighborVisible(context, block, (int)block.GetConfig().ViewRange);
		}

		// Token: 0x060079E5 RID: 31205 RVA: 0x00478924 File Offset: 0x00476B24
		public void SetBlockAndNeighborVisible(DataContext context, MapBlockData block, int range)
		{
			bool flag = !block.Visible;
			if (flag)
			{
				block.SetVisible(true, context);
			}
			List<MapBlockData> neighborBlocks = ObjectPool<List<MapBlockData>>.Instance.Get();
			this.GetNeighborBlocks(block.AreaId, block.BlockId, neighborBlocks, range);
			for (int i = 0; i < neighborBlocks.Count; i++)
			{
				MapBlockData neighborBlock = neighborBlocks[i];
				bool flag2 = !neighborBlock.Visible;
				if (flag2)
				{
					neighborBlock.SetVisible(true, context);
				}
			}
			ObjectPool<List<MapBlockData>>.Instance.Return(neighborBlocks);
		}

		// Token: 0x060079E6 RID: 31206 RVA: 0x004789B0 File Offset: 0x00476BB0
		[DomainMethod]
		public TeammateBubbleCollection GetTeammateBubbleCollection(DataContext context, bool isTraveling)
		{
			TeammateBubbleCollection collection;
			return (DomainManager.Adventure.GetCurAdventureId() < 0 && this.TryUpdateTeammateTypes() && this.TryGetTeammateBubbleCollection(context, isTraveling, out collection)) ? collection : null;
		}

		// Token: 0x060079E7 RID: 31207 RVA: 0x004789E8 File Offset: 0x00476BE8
		public bool IsTeammateBubbleAbleToDisplayByTeammateTypes(short templateId)
		{
			return (this._availableBubbleCache[(int)templateId] & this._teammateTypes) != 0;
		}

		// Token: 0x060079E8 RID: 31208 RVA: 0x00478A10 File Offset: 0x00476C10
		[return: TupleElementNames(new string[]
		{
			"charId",
			"index",
			"subtype"
		})]
		public ValueTuple<int, int, int> GetSelectedBubbleBestMatchTeammate(short templateId)
		{
			ValueTuple<int, int, int> res = new ValueTuple<int, int, int>(-1, -1, -1);
			sbyte personalityType = TeammateBubble.Instance[templateId].PersonalityType;
			int prevPriority = -1;
			int prevPersonality = -1;
			this._teammateHighestPriorityText.Clear();
			foreach (ValueTuple<int, int, int> teammate in this._teammates)
			{
				bool flag = !this.IsTeammateAbleToDisplayBubble(teammate.Item1);
				if (!flag)
				{
					for (int subtype = 0; subtype < 12; subtype++)
					{
						int mask = 1 << subtype;
						bool flag2 = (mask & this._availableBubbleCache[(int)templateId]) != 0 && (mask & teammate.Item3) != 0;
						if (flag2)
						{
							this._teammateHighestPriorityText.Add(new ValueTuple<int, int, int>(teammate.Item1, teammate.Item2, subtype));
						}
					}
				}
			}
			foreach (ValueTuple<int, int, int> teammate2 in this._teammateHighestPriorityText)
			{
				int currPriority = TeammateBubbleSubType.GetPriority(teammate2.Item3);
				sbyte currPersonality = DomainManager.Character.GetElement_Objects(teammate2.Item1).GetPersonality(personalityType);
				bool flag3 = !GameData.Domains.Character.Character.IsCharacterIdValid(res.Item1) || prevPriority < currPriority || (prevPriority == currPriority && prevPersonality < (int)currPersonality);
				if (flag3)
				{
					res = teammate2;
					prevPersonality = (int)currPersonality;
					prevPriority = currPriority;
				}
			}
			return res;
		}

		// Token: 0x060079E9 RID: 31209 RVA: 0x00478BBC File Offset: 0x00476DBC
		public bool TryUpdateTeammateTypes()
		{
			bool flag = this.GetTeammateCount() == 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this._lastTaiwuLocation == DomainManager.Taiwu.GetTaiwu().GetValidLocation();
				if (flag2)
				{
					result = false;
				}
				else
				{
					this._teammateTypes = 0;
					foreach (ValueTuple<int, int, int> teammate in this._teammates)
					{
						bool flag3 = this.IsTeammateAbleToDisplayBubble(teammate.Item1);
						if (flag3)
						{
							this._teammateTypes |= teammate.Item3;
						}
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060079EA RID: 31210 RVA: 0x00478C78 File Offset: 0x00476E78
		public bool TryGetTeammateBubbleCollection(DataContext context, bool isTraveling, out TeammateBubbleCollection collection)
		{
			Location location = DomainManager.Taiwu.GetTaiwu().GetValidLocation();
			Settlement settlement = DomainManager.Organization.GetSettlementByLocation(location);
			MapBlockData mapBlock = DomainManager.Map.GetBlock(location);
			AdventureSiteData siteData = DomainManager.Adventure.GetAdventureSite(location.AreaId, location.BlockId);
			short adventureId = (siteData != null && siteData.SiteState == 1) ? siteData.TemplateId : -1;
			collection = new TeammateBubbleCollection();
			bool result;
			if (isTraveling)
			{
				result = this.TryGetTravelingBubble(location.AreaId, collection);
			}
			else
			{
				for (ETeammateBubbleBubbleElementType type = ETeammateBubbleBubbleElementType.Lost; type < ETeammateBubbleBubbleElementType.Count; type++)
				{
					switch (type)
					{
					case ETeammateBubbleBubbleElementType.Lost:
					{
						bool flag = this.TryGetDreamBackBubble(location, collection);
						if (flag)
						{
							return true;
						}
						break;
					}
					case ETeammateBubbleBubbleElementType.Story:
					{
						bool flag2 = this.TryGetStoryBubble(adventureId, collection);
						if (flag2)
						{
							return true;
						}
						break;
					}
					case ETeammateBubbleBubbleElementType.StoryMapblockEffect:
					{
						bool flag3 = this.TryGetStoryMapBlockEffectBubble(location, collection);
						if (flag3)
						{
							return true;
						}
						break;
					}
					case ETeammateBubbleBubbleElementType.Queerbook:
					{
						bool flag4 = this.TryGetLegendaryBookBubble(adventureId, collection);
						if (flag4)
						{
							return true;
						}
						break;
					}
					case ETeammateBubbleBubbleElementType.EnemyNest:
					{
						bool flag5 = this.TryGetEnemyNestBubble(adventureId, collection);
						if (flag5)
						{
							return true;
						}
						break;
					}
					case ETeammateBubbleBubbleElementType.SummerCombatMatch:
					{
						bool flag6 = this.TryGetCombatMatchBubble(adventureId, collection);
						if (flag6)
						{
							return true;
						}
						break;
					}
					case ETeammateBubbleBubbleElementType.TaiwuVillage:
					{
						bool flag7 = this.TryGetTaiwuVillageBubble(mapBlock.TemplateId, collection);
						if (flag7)
						{
							return true;
						}
						break;
					}
					case ETeammateBubbleBubbleElementType.Chicken:
					{
						bool flag8 = this.TryGetChickenBubble(settlement, collection);
						if (flag8)
						{
							return true;
						}
						break;
					}
					case ETeammateBubbleBubbleElementType.SectCombatMatch:
					{
						bool flag9 = this.TryGetSectCombatMatchBubble(adventureId, mapBlock.TemplateId, collection);
						if (flag9)
						{
							return true;
						}
						break;
					}
					case ETeammateBubbleBubbleElementType.Treasure:
					{
						bool flag10 = this.TryGetMaterialResourceBubble(adventureId, collection);
						if (flag10)
						{
							return true;
						}
						break;
					}
					case ETeammateBubbleBubbleElementType.RelatedCharacter:
					{
						bool flag11 = this.TryGetRelatedCharacterBubble(mapBlock, collection);
						if (flag11)
						{
							return true;
						}
						break;
					}
					case ETeammateBubbleBubbleElementType.Infected:
					{
						bool flag12 = this.TryGetInfectedCharacterBubble(mapBlock, collection);
						if (flag12)
						{
							return true;
						}
						break;
					}
					case ETeammateBubbleBubbleElementType.LegendaryBookInsane:
					{
						bool flag13 = this.TryGetLegendaryBookInsaneCharacterBubble(mapBlock, collection);
						if (flag13)
						{
							return true;
						}
						break;
					}
					case ETeammateBubbleBubbleElementType.Grave:
					{
						bool flag14 = this.TryGetNonEnemyGraveBubble(mapBlock, collection);
						if (flag14)
						{
							return true;
						}
						break;
					}
					case ETeammateBubbleBubbleElementType.SectLeader:
					{
						bool flag15 = this.TryGetSectLeaderBubble(mapBlock, collection);
						if (flag15)
						{
							return true;
						}
						break;
					}
					case ETeammateBubbleBubbleElementType.DestroyedArea:
					{
						bool flag16 = this.TryGetBrokenAreaBubble(location.AreaId, mapBlock.TemplateId, collection);
						if (flag16)
						{
							return true;
						}
						break;
					}
					case ETeammateBubbleBubbleElementType.Animal:
					{
						bool flag17 = this.TryGetAnimalCharacterBubble(location, collection);
						if (flag17)
						{
							return true;
						}
						break;
					}
					case ETeammateBubbleBubbleElementType.Caravan:
					{
						bool flag18 = this.TryGetCaravanCharacterBubble(collection);
						if (flag18)
						{
							return true;
						}
						break;
					}
					case ETeammateBubbleBubbleElementType.SwordGrave:
					{
						bool flag19 = this.TryGetSwordTombBubble(adventureId, collection);
						if (flag19)
						{
							return true;
						}
						break;
					}
					case ETeammateBubbleBubbleElementType.SettlementAdventure:
					{
						bool flag20 = this.TryGetSettlementAdventureBubble(adventureId, collection);
						if (flag20)
						{
							return true;
						}
						break;
					}
					case ETeammateBubbleBubbleElementType.Organization:
					{
						bool flag21 = this.TryGetOrganizationBubble(mapBlock.TemplateId, collection);
						if (flag21)
						{
							return true;
						}
						break;
					}
					case ETeammateBubbleBubbleElementType.City:
					{
						bool flag22 = this.TryGetCityBubble(mapBlock.TemplateId, collection);
						if (flag22)
						{
							return true;
						}
						break;
					}
					case ETeammateBubbleBubbleElementType.Village:
					{
						bool flag23 = this.TryGetVillageBubble(mapBlock.TemplateId, collection);
						if (flag23)
						{
							return true;
						}
						break;
					}
					case ETeammateBubbleBubbleElementType.Worker:
					{
						bool flag24 = this.TryGetWorkerBubble(location, collection);
						if (flag24)
						{
							return true;
						}
						break;
					}
					case ETeammateBubbleBubbleElementType.Cricket:
					{
						bool flag25 = this.TryGetCricketBubble(context, location, collection);
						if (flag25)
						{
							return true;
						}
						break;
					}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060079EB RID: 31211 RVA: 0x00479068 File Offset: 0x00477268
		private void InitializeTeammateBubble()
		{
			this._lastTaiwuLocation = Location.Invalid;
			this._availableBubbleCache.Clear();
			this._elementTypeBubbleCache.Clear();
			foreach (TeammateBubbleItem item in ((IEnumerable<TeammateBubbleItem>)TeammateBubble.Instance))
			{
				bool flag = !this._elementTypeBubbleCache.ContainsKey(item.BubbleElementType);
				if (flag)
				{
					this._elementTypeBubbleCache.Add(item.BubbleElementType, new HashSet<short>());
				}
				this._elementTypeBubbleCache[item.BubbleElementType].Add(item.TemplateId);
				this._availableBubbleCache[(int)item.TemplateId] = 0;
				bool flag2 = !string.IsNullOrEmpty(item.SpecialDesc0);
				if (flag2)
				{
					Dictionary<int, int> availableBubbleCache = this._availableBubbleCache;
					int templateId = (int)item.TemplateId;
					availableBubbleCache[templateId] |= 1;
				}
				bool flag3 = !string.IsNullOrEmpty(item.SpecialDesc1);
				if (flag3)
				{
					Dictionary<int, int> availableBubbleCache = this._availableBubbleCache;
					int templateId = (int)item.TemplateId;
					availableBubbleCache[templateId] |= 2;
				}
				bool flag4 = !string.IsNullOrEmpty(item.SpecialDesc2);
				if (flag4)
				{
					Dictionary<int, int> availableBubbleCache = this._availableBubbleCache;
					int templateId = (int)item.TemplateId;
					availableBubbleCache[templateId] |= 4;
				}
				bool flag5 = !string.IsNullOrEmpty(item.SpecialDesc3);
				if (flag5)
				{
					Dictionary<int, int> availableBubbleCache = this._availableBubbleCache;
					int templateId = (int)item.TemplateId;
					availableBubbleCache[templateId] |= 8;
				}
				bool flag6 = !string.IsNullOrEmpty(item.SpecialDesc4);
				if (flag6)
				{
					Dictionary<int, int> availableBubbleCache = this._availableBubbleCache;
					int templateId = (int)item.TemplateId;
					availableBubbleCache[templateId] |= 16;
				}
				bool flag7 = !string.IsNullOrEmpty(item.FamilyDesc);
				if (flag7)
				{
					Dictionary<int, int> availableBubbleCache = this._availableBubbleCache;
					int templateId = (int)item.TemplateId;
					availableBubbleCache[templateId] |= 32;
				}
				bool flag8 = !string.IsNullOrEmpty(item.FriendDesc);
				if (flag8)
				{
					Dictionary<int, int> availableBubbleCache = this._availableBubbleCache;
					int templateId = (int)item.TemplateId;
					availableBubbleCache[templateId] |= 64;
				}
				bool flag9 = !string.IsNullOrEmpty(item.BehaviorDesc[0]);
				if (flag9)
				{
					Dictionary<int, int> availableBubbleCache = this._availableBubbleCache;
					int templateId = (int)item.TemplateId;
					availableBubbleCache[templateId] |= 128;
				}
				bool flag10 = !string.IsNullOrEmpty(item.BehaviorDesc[1]);
				if (flag10)
				{
					Dictionary<int, int> availableBubbleCache = this._availableBubbleCache;
					int templateId = (int)item.TemplateId;
					availableBubbleCache[templateId] |= 256;
				}
				bool flag11 = !string.IsNullOrEmpty(item.BehaviorDesc[2]);
				if (flag11)
				{
					Dictionary<int, int> availableBubbleCache = this._availableBubbleCache;
					int templateId = (int)item.TemplateId;
					availableBubbleCache[templateId] |= 512;
				}
				bool flag12 = !string.IsNullOrEmpty(item.BehaviorDesc[3]);
				if (flag12)
				{
					Dictionary<int, int> availableBubbleCache = this._availableBubbleCache;
					int templateId = (int)item.TemplateId;
					availableBubbleCache[templateId] |= 1024;
				}
				bool flag13 = !string.IsNullOrEmpty(item.BehaviorDesc[4]);
				if (flag13)
				{
					Dictionary<int, int> availableBubbleCache = this._availableBubbleCache;
					int templateId = (int)item.TemplateId;
					availableBubbleCache[templateId] |= 2048;
				}
			}
			this.UpdateTeammateBubbleData();
			for (int index = 0; index < 3; index++)
			{
				GameDataBridge.AddPostDataModificationHandler(new DataUid(5, 35, (ulong)((long)index), uint.MaxValue), "UpdateTeammateBubbleData", new Action<DataContext, DataUid>(this.UpdateTeammateBubbleData));
			}
		}

		// Token: 0x060079EC RID: 31212 RVA: 0x0047946C File Offset: 0x0047766C
		private void UpdateTeammateBubbleData(DataContext context, DataUid uid)
		{
			this.UpdateTeammateBubbleData();
		}

		// Token: 0x060079ED RID: 31213 RVA: 0x00479478 File Offset: 0x00477678
		public void UpdateTeammateBubbleData()
		{
			this._teammates.Clear();
			CharacterSet groupChars = DomainManager.Taiwu.GetGroupCharIds();
			for (int i = 0; i < 3; i++)
			{
				int charId = DomainManager.Taiwu.GetElement_CombatGroupCharIds(i);
				bool flag = charId >= 0 && groupChars.Contains(charId);
				if (flag)
				{
					this._teammates.Add(new ValueTuple<int, int, int>(charId, i, this.GetTeammateType(charId)));
				}
			}
		}

		// Token: 0x060079EE RID: 31214 RVA: 0x004794EC File Offset: 0x004776EC
		private int GetTeammateHighestPriority(int charId)
		{
			int res = 0;
			int teammateType = 0;
			foreach (ValueTuple<int, int, int> teammate in this._teammates)
			{
				bool flag = teammate.Item1 == charId;
				if (flag)
				{
					teammateType = teammate.Item3;
					break;
				}
			}
			for (int i = 0; i < 12; i++)
			{
				bool flag2 = (teammateType & 1 << i) != 0;
				if (flag2)
				{
					res = Math.Max(res, TeammateBubbleSubType.GetPriority(i));
				}
			}
			return res;
		}

		// Token: 0x060079EF RID: 31215 RVA: 0x0047959C File Offset: 0x0047779C
		private int GetTeammateType(int charId)
		{
			int res = 0;
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			bool flag = character.GetFeatureIds().Contains(554);
			if (flag)
			{
				res |= 1;
			}
			short templateId = character.GetTemplateId();
			bool flag2 = templateId == 464;
			if (flag2)
			{
				res |= 2;
			}
			bool flag3 = templateId == 463;
			if (flag3)
			{
				res |= 4;
			}
			bool flag4 = templateId == 466;
			if (flag4)
			{
				res |= 8;
			}
			bool flag5 = templateId == 465;
			if (flag5)
			{
				res |= 16;
			}
			RelatedCharacter relation;
			bool flag6 = !DomainManager.Character.TryGetRelation(charId, DomainManager.Taiwu.GetTaiwuCharId(), out relation);
			int result;
			if (flag6)
			{
				result = 0;
			}
			else
			{
				ushort relationType = relation.RelationType;
				bool flag7 = RelationType.IsFamilyRelation(relationType);
				if (flag7)
				{
					res |= 32;
				}
				bool flag8 = RelationType.IsFriendRelation(relationType);
				if (flag8)
				{
					res |= 64;
				}
				switch (character.GetBehaviorType())
				{
				case 0:
					res |= 128;
					break;
				case 1:
					res |= 256;
					break;
				case 2:
					res |= 512;
					break;
				case 3:
					res |= 1024;
					break;
				case 4:
					res |= 2048;
					break;
				}
				result = res;
			}
			return result;
		}

		// Token: 0x060079F0 RID: 31216 RVA: 0x004796DC File Offset: 0x004778DC
		private int GetTeammateCount()
		{
			return this._teammates.Count;
		}

		// Token: 0x060079F1 RID: 31217 RVA: 0x004796FC File Offset: 0x004778FC
		private bool IsTeammateAbleToDisplayBubble(int charId)
		{
			return this.GetTeammateCount() == 1 || this._lastTeammateBubble.Item1 != charId || this._lastTeammateBubble.Item2 < 2;
		}

		// Token: 0x060079F2 RID: 31218 RVA: 0x00479738 File Offset: 0x00477938
		private void SetLastTeammateBubble(int charId)
		{
			this._lastTeammateBubble = ((this._lastTeammateBubble.Item1 == charId) ? new ValueTuple<int, int>(charId, this._lastTeammateBubble.Item2 + 1) : new ValueTuple<int, int>(charId, 1));
			this._lastTaiwuLocation = DomainManager.Taiwu.GetTaiwu().GetValidLocation();
		}

		// Token: 0x060079F3 RID: 31219 RVA: 0x0047978C File Offset: 0x0047798C
		private void ApplyTeammateBubble(short templateId, TeammateBubbleCollection collection)
		{
			ValueTuple<int, int, int> teammate = this.GetSelectedBubbleBestMatchTeammate(templateId);
			collection.AddNoneParameterBubble(TeammateBubble.Instance[templateId], teammate.Item2, teammate.Item3);
			this.SetLastTeammateBubble(teammate.Item1);
		}

		// Token: 0x060079F4 RID: 31220 RVA: 0x004797D0 File Offset: 0x004779D0
		private void ApplyAdventureTeammateBubble(short templateId, TeammateBubbleCollection collection, short adventureId)
		{
			ValueTuple<int, int, int> teammate = this.GetSelectedBubbleBestMatchTeammate(templateId);
			collection.AddAdventureParameterBubble(TeammateBubble.Instance[templateId], teammate.Item2, teammate.Item3, adventureId);
			this.SetLastTeammateBubble(teammate.Item1);
		}

		// Token: 0x060079F5 RID: 31221 RVA: 0x00479814 File Offset: 0x00477A14
		private void ApplyCombatMatchTeammateBubble(short templateId, TeammateBubbleCollection collection, sbyte type)
		{
			ValueTuple<int, int, int> teammate = this.GetSelectedBubbleBestMatchTeammate(templateId);
			collection.AddSingleCombatSkillTypeParameterBubble(TeammateBubble.Instance[templateId], teammate.Item2, teammate.Item3, type);
			this.SetLastTeammateBubble(teammate.Item1);
		}

		// Token: 0x060079F6 RID: 31222 RVA: 0x00479858 File Offset: 0x00477A58
		private void ApplyIdentityTeammateBubble(short templateId, TeammateBubbleCollection collection, int type)
		{
			ValueTuple<int, int, int> teammate = this.GetSelectedBubbleBestMatchTeammate(templateId);
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(teammate.Item1);
			OrganizationInfo orgInfo = character.GetOrganizationInfo();
			sbyte gender = character.GetGender();
			collection.AddCharacterIdentityBubble(TeammateBubble.Instance[templateId], teammate.Item2, teammate.Item3, type, orgInfo, gender);
			this.SetLastTeammateBubble(teammate.Item1);
		}

		// Token: 0x060079F7 RID: 31223 RVA: 0x004798BC File Offset: 0x00477ABC
		private bool TryGetBrokenAreaBubble(short areaId, short mapBlockTemplateId, TeammateBubbleCollection collection)
		{
			bool flag = MapAreaData.IsBrokenArea(areaId) && mapBlockTemplateId == 38 && this.IsTeammateBubbleAbleToDisplayByTeammateTypes(187);
			bool result;
			if (flag)
			{
				this.ApplyTeammateBubble(187, collection);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060079F8 RID: 31224 RVA: 0x00479900 File Offset: 0x00477B00
		private bool TryGetTravelingBubble(short areaId, TeammateBubbleCollection collection)
		{
			sbyte stateId = DomainManager.Map.GetStateTemplateIdByAreaId(areaId);
			bool flag = this._lastTaiwuLocation != Location.Invalid && DomainManager.Map.GetStateTemplateIdByAreaId(this._lastTaiwuLocation.AreaId) == stateId;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				foreach (short templateId in this._elementTypeBubbleCache[ETeammateBubbleBubbleElementType.Traveling])
				{
					bool flag2 = !this.IsTeammateBubbleAbleToDisplayByTeammateTypes(templateId);
					if (!flag2)
					{
						TeammateBubbleItem config = TeammateBubble.Instance[templateId];
						bool flag3 = config.MapStateTemplateId == stateId;
						if (flag3)
						{
							this.ApplyTeammateBubble(templateId, collection);
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060079F9 RID: 31225 RVA: 0x004799E0 File Offset: 0x00477BE0
		private bool TryGetDreamBackBubble(Location location, TeammateBubbleCollection collection)
		{
			List<DreamBackLocationData> dataList = DomainManager.Extra.GetDreamBackLocationData();
			bool flag = dataList == null || dataList.Count == 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				foreach (short templateId in this._elementTypeBubbleCache[ETeammateBubbleBubbleElementType.Lost])
				{
					bool flag2 = !this.IsTeammateBubbleAbleToDisplayByTeammateTypes(templateId);
					if (!flag2)
					{
						foreach (DreamBackLocationData data in dataList)
						{
							bool flag3 = data.Location == location;
							if (flag3)
							{
								this.ApplyTeammateBubble(templateId, collection);
								return true;
							}
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060079FA RID: 31226 RVA: 0x00479AD4 File Offset: 0x00477CD4
		private bool TryGetStoryBubble(short adventureTemplateId, TeammateBubbleCollection collection)
		{
			bool flag = adventureTemplateId < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				foreach (short templateId in this._elementTypeBubbleCache[ETeammateBubbleBubbleElementType.Story])
				{
					bool flag2 = !this.IsTeammateBubbleAbleToDisplayByTeammateTypes(templateId);
					if (!flag2)
					{
						TeammateBubbleItem config = TeammateBubble.Instance[templateId];
						bool flag3 = config.AdventureTemplateIdList != null && config.AdventureTemplateIdList.Contains(adventureTemplateId);
						if (flag3)
						{
							this.ApplyTeammateBubble(templateId, collection);
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060079FB RID: 31227 RVA: 0x00479B8C File Offset: 0x00477D8C
		private bool TryGetStoryMapBlockEffectBubble(Location location, TeammateBubbleCollection collection)
		{
			bool canTriggerFulongFlameTeammateBubble = this._canTriggerFulongFlameTeammateBubble;
			bool result;
			if (canTriggerFulongFlameTeammateBubble)
			{
				this.ApplyTeammateBubble(188, collection);
				this._canTriggerFulongFlameTeammateBubble = false;
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060079FC RID: 31228 RVA: 0x00479BC4 File Offset: 0x00477DC4
		private bool TryGetLegendaryBookBubble(short adventureId, TeammateBubbleCollection collection)
		{
			bool flag = adventureId < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				foreach (short templateId in this._elementTypeBubbleCache[ETeammateBubbleBubbleElementType.Queerbook])
				{
					bool flag2 = !this.IsTeammateBubbleAbleToDisplayByTeammateTypes(templateId);
					if (!flag2)
					{
						TeammateBubbleItem config = TeammateBubble.Instance[templateId];
						bool flag3 = config.AdventureTemplateIdList != null && config.AdventureTemplateIdList.Contains(adventureId);
						if (flag3)
						{
							this.ApplyAdventureTeammateBubble(templateId, collection, adventureId);
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060079FD RID: 31229 RVA: 0x00479C7C File Offset: 0x00477E7C
		private bool TryGetChickenBubble(Settlement settlement, TeammateBubbleCollection collection)
		{
			bool flag = settlement == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				GameData.Domains.Building.Chicken chicken;
				bool flag2 = DomainManager.Building.TryGetFirstChickenInSettlement(settlement, out chicken);
				if (flag2)
				{
					foreach (short templateId in this._elementTypeBubbleCache[ETeammateBubbleBubbleElementType.Chicken])
					{
						bool flag3 = !this.IsTeammateBubbleAbleToDisplayByTeammateTypes(templateId);
						if (!flag3)
						{
							this.ApplyTeammateBubble(templateId, collection);
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060079FE RID: 31230 RVA: 0x00479D1C File Offset: 0x00477F1C
		private bool TryGetSectCombatMatchBubble(short adventureId, short mapBlockTemplateId, TeammateBubbleCollection collection)
		{
			bool flag = adventureId != 26;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				foreach (short templateId in this._elementTypeBubbleCache[ETeammateBubbleBubbleElementType.SectCombatMatch])
				{
					bool flag2 = !this.IsTeammateBubbleAbleToDisplayByTeammateTypes(templateId);
					if (!flag2)
					{
						TeammateBubbleItem config = TeammateBubble.Instance[templateId];
						bool flag3 = mapBlockTemplateId == config.MapBlockTemplateId;
						if (flag3)
						{
							this.ApplyTeammateBubble(templateId, collection);
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060079FF RID: 31231 RVA: 0x00479DC8 File Offset: 0x00477FC8
		private bool TryGetMaterialResourceBubble(short adventureId, TeammateBubbleCollection collection)
		{
			bool flag = adventureId < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				foreach (short templateId in this._elementTypeBubbleCache[ETeammateBubbleBubbleElementType.Treasure])
				{
					bool flag2 = !this.IsTeammateBubbleAbleToDisplayByTeammateTypes(templateId);
					if (!flag2)
					{
						TeammateBubbleItem config = TeammateBubble.Instance[templateId];
						bool flag3 = config.AdventureTemplateIdList != null && config.AdventureTemplateIdList.Contains(adventureId);
						if (flag3)
						{
							this.ApplyTeammateBubble(templateId, collection);
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06007A00 RID: 31232 RVA: 0x00479E80 File Offset: 0x00478080
		private bool TryGetSwordTombBubble(short adventureTemplateId, TeammateBubbleCollection collection)
		{
			bool flag = adventureTemplateId < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				foreach (short templateId in this._elementTypeBubbleCache[ETeammateBubbleBubbleElementType.SwordGrave])
				{
					bool flag2 = !this.IsTeammateBubbleAbleToDisplayByTeammateTypes(templateId);
					if (!flag2)
					{
						TeammateBubbleItem config = TeammateBubble.Instance[templateId];
						bool flag3 = config.AdventureTemplateIdList != null && config.AdventureTemplateIdList.Contains(adventureTemplateId);
						if (flag3)
						{
							this.ApplyTeammateBubble(templateId, collection);
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06007A01 RID: 31233 RVA: 0x00479F38 File Offset: 0x00478138
		private bool TryGetSettlementAdventureBubble(short adventureTemplateId, TeammateBubbleCollection collection)
		{
			bool flag = adventureTemplateId < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				foreach (short templateId in this._elementTypeBubbleCache[ETeammateBubbleBubbleElementType.SettlementAdventure])
				{
					bool flag2 = !this.IsTeammateBubbleAbleToDisplayByTeammateTypes(templateId);
					if (!flag2)
					{
						TeammateBubbleItem config = TeammateBubble.Instance[templateId];
						bool flag3 = config.AdventureTemplateIdList != null && config.AdventureTemplateIdList.Contains(adventureTemplateId);
						if (flag3)
						{
							this.ApplyTeammateBubble(templateId, collection);
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06007A02 RID: 31234 RVA: 0x00479FF0 File Offset: 0x004781F0
		private bool TryGetCombatMatchBubble(short adventureTemplateId, TeammateBubbleCollection collection)
		{
			bool flag = adventureTemplateId < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				foreach (short templateId in this._elementTypeBubbleCache[ETeammateBubbleBubbleElementType.SummerCombatMatch])
				{
					bool flag2 = !this.IsTeammateBubbleAbleToDisplayByTeammateTypes(templateId);
					if (!flag2)
					{
						TeammateBubbleItem config = TeammateBubble.Instance[templateId];
						bool flag3 = config == null;
						if (flag3)
						{
							return false;
						}
						bool flag4 = config.AdventureTemplateIdList != null && config.AdventureTemplateIdList.Contains(adventureTemplateId);
						if (flag4)
						{
							this.ApplyCombatMatchTeammateBubble(templateId, collection, this._combatMatchIdToCombatSkillType[adventureTemplateId]);
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06007A03 RID: 31235 RVA: 0x0047A0C4 File Offset: 0x004782C4
		private bool TryGetTaiwuVillageBubble(short mapBlockTemplateId, TeammateBubbleCollection collection)
		{
			foreach (short templateId in this._elementTypeBubbleCache[ETeammateBubbleBubbleElementType.TaiwuVillage])
			{
				bool flag = !this.IsTeammateBubbleAbleToDisplayByTeammateTypes(templateId);
				if (!flag)
				{
					TeammateBubbleItem config = TeammateBubble.Instance[templateId];
					bool flag2 = mapBlockTemplateId == config.MapBlockTemplateId;
					if (flag2)
					{
						this.ApplyTeammateBubble(templateId, collection);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007A04 RID: 31236 RVA: 0x0047A15C File Offset: 0x0047835C
		private bool TryGetOrganizationBubble(short mapBlockTemplateId, TeammateBubbleCollection collection)
		{
			foreach (short templateId in this._elementTypeBubbleCache[ETeammateBubbleBubbleElementType.Organization])
			{
				bool flag = !this.IsTeammateBubbleAbleToDisplayByTeammateTypes(templateId);
				if (!flag)
				{
					TeammateBubbleItem config = TeammateBubble.Instance[templateId];
					bool flag2 = mapBlockTemplateId == config.MapBlockTemplateId;
					if (flag2)
					{
						this.ApplyTeammateBubble(templateId, collection);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007A05 RID: 31237 RVA: 0x0047A1F8 File Offset: 0x004783F8
		private bool TryGetCityBubble(short mapBlockTemplateId, TeammateBubbleCollection collection)
		{
			foreach (short templateId in this._elementTypeBubbleCache[ETeammateBubbleBubbleElementType.City])
			{
				bool flag = !this.IsTeammateBubbleAbleToDisplayByTeammateTypes(templateId);
				if (!flag)
				{
					TeammateBubbleItem config = TeammateBubble.Instance[templateId];
					bool flag2 = mapBlockTemplateId == config.MapBlockTemplateId;
					if (flag2)
					{
						this.ApplyTeammateBubble(templateId, collection);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007A06 RID: 31238 RVA: 0x0047A294 File Offset: 0x00478494
		private bool TryGetVillageBubble(short mapBlockTemplateId, TeammateBubbleCollection collection)
		{
			foreach (short templateId in this._elementTypeBubbleCache[ETeammateBubbleBubbleElementType.Village])
			{
				bool flag = !this.IsTeammateBubbleAbleToDisplayByTeammateTypes(templateId);
				if (!flag)
				{
					TeammateBubbleItem config = TeammateBubble.Instance[templateId];
					bool flag2 = mapBlockTemplateId == config.MapBlockTemplateId;
					if (flag2)
					{
						this.ApplyTeammateBubble(templateId, collection);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007A07 RID: 31239 RVA: 0x0047A330 File Offset: 0x00478530
		private bool TryGetEnemyNestBubble(short adventureId, TeammateBubbleCollection collection)
		{
			bool flag = adventureId < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				foreach (short templateId in this._elementTypeBubbleCache[ETeammateBubbleBubbleElementType.EnemyNest])
				{
					bool flag2 = !this.IsTeammateBubbleAbleToDisplayByTeammateTypes(templateId);
					if (!flag2)
					{
						TeammateBubbleItem config = TeammateBubble.Instance[templateId];
						bool flag3 = config.AdventureTemplateIdList != null && config.AdventureTemplateIdList.Contains(adventureId);
						if (flag3)
						{
							this.ApplyTeammateBubble(templateId, collection);
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06007A08 RID: 31240 RVA: 0x0047A3E8 File Offset: 0x004785E8
		private bool TryGetWorkerBubble(Location location, TeammateBubbleCollection collection)
		{
			VoidValue voidValue;
			bool flag = DomainManager.Taiwu.TryGetElement_VillagerWorkLocations(location, out voidValue);
			if (flag)
			{
				foreach (short templateId in this._elementTypeBubbleCache[ETeammateBubbleBubbleElementType.Worker])
				{
					bool flag2 = !this.IsTeammateBubbleAbleToDisplayByTeammateTypes(templateId);
					if (!flag2)
					{
						this.ApplyTeammateBubble(templateId, collection);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007A09 RID: 31241 RVA: 0x0047A47C File Offset: 0x0047867C
		private bool TryGetCricketBubble(DataContext context, Location location, TeammateBubbleCollection collection)
		{
			bool flag = DomainManager.Map.LocationHasCricket(context, location);
			if (flag)
			{
				foreach (short templateId in this._elementTypeBubbleCache[ETeammateBubbleBubbleElementType.Cricket])
				{
					bool flag2 = !this.IsTeammateBubbleAbleToDisplayByTeammateTypes(templateId);
					if (!flag2)
					{
						this.ApplyTeammateBubble(templateId, collection);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007A0A RID: 31242 RVA: 0x0047A50C File Offset: 0x0047870C
		private bool TryGetAnimalCharacterBubble(Location location, TeammateBubbleCollection collection)
		{
			List<int> animals;
			GameData.Domains.Character.Animal animal;
			bool flag = DomainManager.Extra.TryGetAnimalIdsByLocation(location, out animals) && animals.Count > 0 && DomainManager.Extra.TryGetAnimal(animals[0], out animal);
			bool result;
			if (flag)
			{
				short templateId = (animal.ItemKey == ItemKey.Invalid) ? 155 : 156;
				bool flag2 = !this.IsTeammateBubbleAbleToDisplayByTeammateTypes(templateId);
				if (flag2)
				{
					result = false;
				}
				else
				{
					this.ApplyTeammateBubble(templateId, collection);
					result = true;
				}
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06007A0B RID: 31243 RVA: 0x0047A598 File Offset: 0x00478798
		private bool TryGetCaravanCharacterBubble(TeammateBubbleCollection collection)
		{
			int id;
			bool flag = DomainManager.Merchant.TryGetFirstTaiwuLocationCaravanId(out id);
			if (flag)
			{
				foreach (short templateId in this._elementTypeBubbleCache[ETeammateBubbleBubbleElementType.Caravan])
				{
					bool flag2 = !this.IsTeammateBubbleAbleToDisplayByTeammateTypes(templateId);
					if (!flag2)
					{
						this.ApplyTeammateBubble(templateId, collection);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007A0C RID: 31244 RVA: 0x0047A628 File Offset: 0x00478828
		private bool TryGetRelatedCharacterBubble(MapBlockData block, TeammateBubbleCollection collection)
		{
			bool flag = block.CharacterSet == null || block.CharacterSet.Count == 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				int taiwuId = DomainManager.Taiwu.GetTaiwuCharId();
				foreach (int id in block.CharacterSet)
				{
					RelatedCharacter relation;
					bool flag2 = !DomainManager.Character.TryGetRelation(id, taiwuId, out relation);
					if (!flag2)
					{
						ushort relationType = relation.RelationType;
						bool flag3 = RelationType.IsFamilyRelation(relationType);
						if (flag3)
						{
							bool flag4 = !this.IsTeammateBubbleAbleToDisplayByTeammateTypes(158);
							if (!flag4)
							{
								this.ApplyTeammateBubble(158, collection);
								return true;
							}
						}
						else
						{
							bool flag5 = RelationType.IsFriendRelation(relationType);
							if (flag5)
							{
								bool flag6 = !this.IsTeammateBubbleAbleToDisplayByTeammateTypes(159);
								if (!flag6)
								{
									this.ApplyTeammateBubble(159, collection);
									return true;
								}
							}
							else
							{
								bool flag7 = (32768 & relationType) > 0;
								if (flag7)
								{
									bool flag8 = !this.IsTeammateBubbleAbleToDisplayByTeammateTypes(160);
									if (!flag8)
									{
										this.ApplyTeammateBubble(160, collection);
										return true;
									}
								}
							}
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06007A0D RID: 31245 RVA: 0x0047A784 File Offset: 0x00478984
		private bool TryGetInfectedCharacterBubble(MapBlockData block, TeammateBubbleCollection collection)
		{
			bool flag = block.InfectedCharacterSet == null || block.InfectedCharacterSet.Count == 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				foreach (int id in block.InfectedCharacterSet)
				{
					GameData.Domains.Character.Character character;
					bool flag2 = DomainManager.Character.TryGetElement_Objects(id, out character);
					if (flag2)
					{
						List<short> features = character.GetFeatureIds();
						foreach (short templateId in this._elementTypeBubbleCache[ETeammateBubbleBubbleElementType.Infected])
						{
							bool flag3 = !this.IsTeammateBubbleAbleToDisplayByTeammateTypes(templateId);
							if (!flag3)
							{
								TeammateBubbleItem config = TeammateBubble.Instance[templateId];
								bool flag4 = config.CharacterFeatureTemplateIdList == null;
								if (!flag4)
								{
									foreach (short featureId in config.CharacterFeatureTemplateIdList)
									{
										bool flag5 = features.Contains(featureId);
										if (flag5)
										{
											this.ApplyTeammateBubble(templateId, collection);
											return true;
										}
									}
								}
							}
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06007A0E RID: 31246 RVA: 0x0047A92C File Offset: 0x00478B2C
		private bool TryGetLegendaryBookInsaneCharacterBubble(MapBlockData block, TeammateBubbleCollection collection)
		{
			bool flag = block.InfectedCharacterSet == null || block.InfectedCharacterSet.Count == 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				foreach (int id in block.InfectedCharacterSet)
				{
					GameData.Domains.Character.Character character;
					bool flag2 = DomainManager.Character.TryGetElement_Objects(id, out character);
					if (flag2)
					{
						List<sbyte> types = DomainManager.LegendaryBook.GetCharOwnedBookTypes(id);
						bool flag3 = types == null;
						if (!flag3)
						{
							List<short> features = character.GetFeatureIds();
							bool flag4 = features.Contains(204);
							if (flag4)
							{
								bool flag5 = !this.IsTeammateBubbleAbleToDisplayByTeammateTypes(165);
								if (!flag5)
								{
									this.ApplyTeammateBubble(165, collection);
									return true;
								}
							}
							else
							{
								bool flag6 = features.Contains(205);
								if (flag6)
								{
									bool flag7 = !this.IsTeammateBubbleAbleToDisplayByTeammateTypes(166);
									if (!flag7)
									{
										this.ApplyTeammateBubble(166, collection);
										return true;
									}
								}
							}
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06007A0F RID: 31247 RVA: 0x0047AA5C File Offset: 0x00478C5C
		private bool TryGetNonEnemyGraveBubble(MapBlockData block, TeammateBubbleCollection collection)
		{
			bool flag = block.GraveSet == null || block.GraveSet.Count == 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				int taiwuId = DomainManager.Taiwu.GetTaiwuCharId();
				foreach (int id in block.GraveSet)
				{
					RelatedCharacter relation;
					bool flag2 = !DomainManager.Character.TryGetRelation(id, taiwuId, out relation);
					if (!flag2)
					{
						ushort relationType = relation.RelationType;
						bool flag3 = (32768 & relationType) == 0;
						if (flag3)
						{
							bool flag4 = !this.IsTeammateBubbleAbleToDisplayByTeammateTypes(167);
							if (!flag4)
							{
								this.ApplyTeammateBubble(167, collection);
								return true;
							}
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06007A10 RID: 31248 RVA: 0x0047AB40 File Offset: 0x00478D40
		private bool TryGetSectLeaderBubble(MapBlockData block, TeammateBubbleCollection collection)
		{
			bool flag = block.CharacterSet == null || block.CharacterSet.Count == 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				foreach (int id in block.CharacterSet)
				{
					GameData.Domains.Character.Character character;
					bool flag2 = DomainManager.Character.TryGetElement_Objects(id, out character);
					if (flag2)
					{
						List<short> features = character.GetFeatureIds();
						foreach (short templateId in this._elementTypeBubbleCache[ETeammateBubbleBubbleElementType.SectLeader])
						{
							bool flag3 = !this.IsTeammateBubbleAbleToDisplayByTeammateTypes(templateId);
							if (!flag3)
							{
								TeammateBubbleItem config = TeammateBubble.Instance[templateId];
								bool flag4 = config.CharacterFeatureTemplateIdList == null;
								if (!flag4)
								{
									foreach (short featureId in config.CharacterFeatureTemplateIdList)
									{
										bool flag5 = features.Contains(featureId);
										if (flag5)
										{
											this.ApplyIdentityTeammateBubble(templateId, collection, id);
											return true;
										}
									}
								}
							}
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06007A11 RID: 31249 RVA: 0x0047ACE8 File Offset: 0x00478EE8
		public static sbyte GetSectOrgTemplateIdByStateTemplateId(sbyte mapStateTemplateId)
		{
			return MapState.Instance[mapStateTemplateId].SectID;
		}

		// Token: 0x06007A12 RID: 31250 RVA: 0x0047AD0C File Offset: 0x00478F0C
		public static short GetCharacterTemplateId(sbyte mapStateTemplateId, sbyte gender)
		{
			return MapState.Instance[mapStateTemplateId].TemplateCharacterIds[(int)gender];
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06007A13 RID: 31251 RVA: 0x0047AD30 File Offset: 0x00478F30
		public bool IsTraveling
		{
			get
			{
				return this._travelInfo.Traveling;
			}
		}

		// Token: 0x06007A14 RID: 31252 RVA: 0x0047AD3D File Offset: 0x00478F3D
		private void InitializeTravelMap()
		{
			this._dijkstraMap.Initialize(this.GetAllAreaIds(), new DijkstraAlgorithm<short>.DijkstraAlgorithmGetNeighbors(this.GetAreaNeighbors));
		}

		// Token: 0x06007A15 RID: 31253 RVA: 0x0047AD60 File Offset: 0x00478F60
		[DomainMethod]
		public void UnlockStation(DataContext context, short areaId, bool costAuthority = true)
		{
			MapAreaData areaData = this._areas[(int)areaId];
			bool flag = areaId >= 135;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(16, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid area id ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(areaId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			bool stationUnlocked = areaData.StationUnlocked;
			if (stationUnlocked)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Station of area ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(areaId);
				defaultInterpolatedStringHandler.AppendLiteral(" already unlocked");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (costAuthority)
			{
				GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
				int foundAreasCount = 0;
				for (short i = 0; i < 135; i += 1)
				{
					bool stationUnlocked2 = this._areas[(int)i].StationUnlocked;
					if (stationUnlocked2)
					{
						foundAreasCount++;
					}
				}
				int needAuthority = Math.Max((int)GlobalConfig.Instance.MapAreaOpenPrestige * (foundAreasCount - (int)(9 * GlobalConfig.Instance.MapInitUnlockStationStateCount) + 1), 0);
				bool flag2 = taiwuChar.GetResource(7) < needAuthority;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(61, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Authority not enough to unlock station at area ");
					defaultInterpolatedStringHandler.AppendFormatted<short>(areaId);
					defaultInterpolatedStringHandler.AppendLiteral(", need ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(needAuthority);
					defaultInterpolatedStringHandler.AppendLiteral(", have ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(taiwuChar.GetResource(7));
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				taiwuChar.ChangeResource(context, 7, -needAuthority);
				DomainManager.Taiwu.AddLegacyPoint(context, 37, 100);
				MapDomain.AddUnlockStationSeniority(context, needAuthority);
			}
			areaData.Discovered = true;
			areaData.StationUnlocked = true;
			this.SetElement_Areas((int)areaId, areaData, context);
			foreach (short neighborAreaId in areaData.NeighborAreas)
			{
				MapAreaData neighborArea = this._areas[(int)neighborAreaId];
				bool flag3 = !neighborArea.Discovered;
				if (flag3)
				{
					neighborArea.Discovered = true;
					this.SetElement_Areas((int)neighborAreaId, neighborArea, context);
				}
			}
		}

		// Token: 0x06007A16 RID: 31254 RVA: 0x0047AF98 File Offset: 0x00479198
		[DomainMethod]
		public void QuickTravel(DataContext context, short destAreaId)
		{
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			short destBlockId = this.GetPureTravelBlockId(context, destAreaId, taiwuChar.GetLocation().AreaId);
			HashSet<int> groupCharIds = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
			foreach (int charId in groupCharIds)
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
				character.SetLocation(new Location(destAreaId, destBlockId), context);
			}
			DomainManager.Extra.GearMateFollowTaiwu(context);
		}

		// Token: 0x06007A17 RID: 31255 RVA: 0x0047B044 File Offset: 0x00479244
		[DomainMethod]
		public CrossAreaMoveInfo GetTravelCost(short fromAreaId, short fromBlockId, short toAreaId)
		{
			sbyte fromStateTemplateId = this.GetStateTemplateIdByAreaId(fromAreaId);
			CrossAreaMoveInfo moveInfo = new CrossAreaMoveInfo
			{
				FromAreaId = fromAreaId,
				FromBlockId = fromBlockId,
				ToAreaId = toAreaId
			};
			bool flag = fromAreaId == toAreaId;
			CrossAreaMoveInfo result;
			if (flag)
			{
				result = moveInfo;
			}
			else
			{
				this._carrierReduceTravelCostDaysPercent = this.CalcCarrierReduceTravelCostDaysPercent();
				bool flag2 = toAreaId == 137 || fromAreaId == 137 || toAreaId == 138 || fromAreaId == 138;
				if (flag2)
				{
					TravelRoute route = new TravelRoute();
					route.AreaList.Add(fromAreaId);
					route.AreaList.Add(toAreaId);
					sbyte[] pos = this._areas[(int)fromAreaId].GetConfig().WorldMapPos;
					route.PosList.Add(new ByteCoordinate((byte)pos[0], (byte)pos[1]));
					route.PosList.Add(new ByteCoordinate(0, 0));
					route.CostList.Add(0);
					route.CostList.Add(0);
					moveInfo.Route = route;
				}
				else
				{
					IReadOnlyList<DijkstraAlgorithm<short>.IReadonlyDijkstraNode> shortestRoute = this._dijkstraMap.FindShortestPath(fromAreaId, toAreaId);
					List<ByteCoordinate> tempRoutePos = ObjectPool<List<ByteCoordinate>>.Instance.Get();
					TravelRoute route2 = moveInfo.Route = new TravelRoute();
					route2.AreaList.EnsureCapacity(shortestRoute.Count);
					route2.CostList.EnsureCapacity(shortestRoute.Count);
					int cost = 0;
					foreach (DijkstraAlgorithm<short>.IReadonlyDijkstraNode node in shortestRoute)
					{
						bool flag3 = node.Pos == fromAreaId;
						if (!flag3)
						{
							route2.AreaList.Add(node.Pos);
							route2.CostList.Add((short)(node.Cost / 1000 - cost));
							cost = node.Cost / 1000;
							MapAreaItem lastArea = this._areas[(int)node.Last].GetConfig();
							MapAreaItem nowArea = this._areas[(int)node.Pos].GetConfig();
							tempRoutePos.Clear();
							bool flag4 = nowArea.TemplateId > lastArea.TemplateId;
							if (flag4)
							{
								foreach (AreaTravelRoute neighborArea in lastArea.NeighborAreas)
								{
									bool flag5 = neighborArea.DestAreaId != nowArea.TemplateId;
									if (!flag5)
									{
										tempRoutePos.AddRange(neighborArea.MapPosList);
										break;
									}
								}
							}
							else
							{
								foreach (AreaTravelRoute neighborArea2 in nowArea.NeighborAreas)
								{
									bool flag6 = neighborArea2.DestAreaId != lastArea.TemplateId;
									if (!flag6)
									{
										tempRoutePos.AddRange(neighborArea2.MapPosList);
										tempRoutePos.Reverse();
										break;
									}
								}
							}
							route2.PosList.AddRange(tempRoutePos);
							bool flag7 = node.Pos != toAreaId;
							if (flag7)
							{
								route2.PosList.Add(new ByteCoordinate((byte)nowArea.WorldMapPos[0], (byte)nowArea.WorldMapPos[1]));
							}
						}
					}
					ObjectPool<List<ByteCoordinate>>.Instance.Return(tempRoutePos);
				}
				moveInfo.MoneyCost = (int)(moveInfo.Route.GetTotalTimeCost() * (short)MapState.Instance[fromStateTemplateId].TravalMoney * 5);
				int foundAreasCount = 0;
				for (short areaId = 0; areaId < 135; areaId += 1)
				{
					bool stationUnlocked = this._areas[(int)areaId].StationUnlocked;
					if (stationUnlocked)
					{
						foundAreasCount++;
					}
				}
				moveInfo.AuthorityCost = 0;
				for (int i = 0; i < moveInfo.Route.AreaList.Count; i++)
				{
					short areaId2 = moveInfo.Route.AreaList[i];
					bool flag8 = !this._areas[(int)areaId2].StationUnlocked;
					if (flag8)
					{
						moveInfo.AuthorityCost += Math.Max((int)GlobalConfig.Instance.MapAreaOpenPrestige * (foundAreasCount - (int)(9 * GlobalConfig.Instance.MapInitUnlockStationStateCount) + 1), 0);
						foundAreasCount++;
					}
				}
				bool flag9 = fromAreaId == 135 || fromAreaId == 138 || fromAreaId == 137;
				if (flag9)
				{
					moveInfo.MoneyCost = 0;
					moveInfo.AuthorityCost = 0;
				}
				result = moveInfo;
			}
			return result;
		}

		// Token: 0x06007A18 RID: 31256 RVA: 0x0047B4CC File Offset: 0x004796CC
		[DomainMethod]
		public TravelPreviewDisplayData GetTravelPreview(short toAreaId)
		{
			TravelPreviewDisplayData result = new TravelPreviewDisplayData
			{
				ToAreaId = -1
			};
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			Location taiwuLocation = taiwuChar.GetLocation();
			bool flag = !taiwuLocation.IsValid() || toAreaId == taiwuLocation.AreaId || toAreaId < 0;
			TravelPreviewDisplayData result2;
			if (flag)
			{
				result2 = result;
			}
			else
			{
				result.ToAreaId = toAreaId;
				CrossAreaMoveInfo cost = this.GetTravelCost(taiwuLocation.AreaId, taiwuLocation.BlockId, toAreaId);
				result.AuthorityCost = cost.AuthorityCost;
				result.MoneyCost = cost.MoneyCost;
				result.DaysCost = (from x in cost.Route.CostList
				select (int)x).Sum();
				result.CurrentAuthority = taiwuChar.GetResource(7);
				bool flag2 = result.AuthorityCost > 0;
				if (flag2)
				{
					result.NeedUnlockStations = new List<short>();
					foreach (short areaId in cost.Route.AreaList)
					{
						bool flag3 = !this._areas[(int)areaId].StationUnlocked;
						if (flag3)
						{
							result.NeedUnlockStations.Add(areaId);
						}
					}
				}
				result2 = result;
			}
			return result2;
		}

		// Token: 0x06007A19 RID: 31257 RVA: 0x0047B62C File Offset: 0x0047982C
		[DomainMethod]
		public bool UnlockTravelPath(DataContext context, short toAreaId)
		{
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			Location taiwuLocation = taiwuChar.GetLocation();
			CrossAreaMoveInfo moveInfo = this.GetTravelCost(taiwuLocation.AreaId, taiwuLocation.BlockId, toAreaId);
			return moveInfo.AuthorityCost > 0 && this.UnlockTravelPathInternal(context, moveInfo);
		}

		// Token: 0x06007A1A RID: 31258 RVA: 0x0047B678 File Offset: 0x00479878
		[DomainMethod]
		public void StartTravel(DataContext context, short toAreaId)
		{
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			Location taiwuLocation = taiwuChar.GetLocation();
			CrossAreaMoveInfo moveInfo = this.GetTravelCost(taiwuLocation.AreaId, taiwuLocation.BlockId, toAreaId);
			moveInfo.AutoCheckFreeTravel();
			bool flag = !this.UnlockTravelPathInternal(context, moveInfo);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Authority not enough to travel to area ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(moveInfo.ToAreaId);
				defaultInterpolatedStringHandler.AppendLiteral(", need ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(moveInfo.AuthorityCost);
				defaultInterpolatedStringHandler.AppendLiteral(", have ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(taiwuChar.GetResource(7));
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			bool flag2 = moveInfo.MoneyCost > 0;
			if (flag2)
			{
				taiwuChar.ChangeResource(context, 6, -moveInfo.MoneyCost);
			}
			this.StartTravelWithoutCost(context, moveInfo);
		}

		// Token: 0x06007A1B RID: 31259 RVA: 0x0047B754 File Offset: 0x00479954
		[DomainMethod]
		public void DirectTravelToTaiwuVillage(DataContext context)
		{
			this.StopTravel(context);
			short toAreaId = DomainManager.Taiwu.GetTaiwuVillageLocation().AreaId;
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			Location taiwuLocation = taiwuChar.GetLocation();
			CrossAreaMoveInfo moveInfo = this.GetTravelCost(taiwuLocation.AreaId, taiwuLocation.BlockId, toAreaId);
			moveInfo.AutoCheckFreeTravel();
			bool flag = moveInfo.MoneyCost > 0;
			if (flag)
			{
				taiwuChar.ChangeResource(context, 6, -moveInfo.MoneyCost);
			}
			this.DirectTravel(context, moveInfo);
		}

		// Token: 0x06007A1C RID: 31260 RVA: 0x0047B7D0 File Offset: 0x004799D0
		[DomainMethod]
		public bool ContinueTravel(DataContext context)
		{
			bool flag = this._travelInfo.Traveling && this._travelInfo.CurrentAreaId == this._travelInfo.ToAreaId;
			bool result;
			if (flag)
			{
				this.FinishTravel(context, this._travelInfo.ToAreaId);
				result = false;
			}
			else
			{
				this.EnsureCostDays(context);
				int costDays = this._travelInfo.NextCostDays;
				bool flag2 = costDays == -1;
				if (flag2)
				{
					result = false;
				}
				else
				{
					int actionPoints = DomainManager.Extra.GetTotalActionPointsRemaining();
					int cost = costDays * 10;
					int actualCost = Math.Min(cost, actionPoints);
					DomainManager.Extra.ConsumeActionPoint(context, actualCost);
					this.RecordTravelCostedDays(context, this._travelInfo.CostedDays + actualCost / 10);
					bool flag3 = actualCost < cost;
					if (flag3)
					{
						DomainManager.World.AdvanceMonth(context);
						result = false;
					}
					else
					{
						int routeIndex = this._travelInfo.RouteIndex;
						short fromAreaId = (routeIndex == 0) ? this._travelInfo.FromAreaId : this._travelInfo.Route.AreaList[routeIndex - 1];
						short toAreaId = this._travelInfo.Route.AreaList[routeIndex];
						bool flag4 = fromAreaId < 135;
						if (flag4)
						{
							List<ValueTuple<Location, short>> blockRoute = this.CalcBlockTravelRoute(context.Random, new Location(fromAreaId, this._areas[(int)fromAreaId].StationBlockId), new Location(toAreaId, this._areas[(int)toAreaId].StationBlockId), false);
							for (int i = 0; i < blockRoute.Count; i++)
							{
								Location location = blockRoute[i].Item1;
								this.SetBlockAndViewRangeVisible(context, location.AreaId, location.BlockId);
							}
						}
						else
						{
							bool flag5 = toAreaId != 137 && toAreaId != 138;
							if (flag5)
							{
								this.SetBlockAndViewRangeVisible(context, toAreaId, this._areas[(int)toAreaId].StationBlockId);
							}
						}
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06007A1D RID: 31261 RVA: 0x0047B9D0 File Offset: 0x00479BD0
		[DomainMethod]
		public short ContinueTravelWithDetectTravelingEvent(DataContext context)
		{
			bool flag = this._travelInfo.Traveling && this.ContinueTravel(context);
			short result;
			if (flag)
			{
				result = this.DetectTravelingEvent(context);
			}
			else
			{
				result = -1;
			}
			return result;
		}

		// Token: 0x06007A1E RID: 31262 RVA: 0x0047BA08 File Offset: 0x00479C08
		[DomainMethod]
		public void RecordTravelCostedDays(DataContext context, int costedDays)
		{
			this._travelInfo.CostedDays = costedDays;
			this.SetTravelInfo(this._travelInfo, context);
		}

		// Token: 0x06007A1F RID: 31263 RVA: 0x0047BA28 File Offset: 0x00479C28
		[DomainMethod]
		public void StopTravel(DataContext context)
		{
			bool flag = !this._travelInfo.Traveling;
			if (!flag)
			{
				this.FinishTravel(context, this._travelInfo.CurrentAreaId);
			}
		}

		// Token: 0x06007A20 RID: 31264 RVA: 0x0047BA60 File Offset: 0x00479C60
		[DomainMethod]
		public void TaiwuBeKidnapped(DataContext context, Location targetLocation, int hunterCharId)
		{
			short areaId = targetLocation.AreaId;
			Tester.Assert(areaId >= 0 && areaId < 135, "");
			DomainManager.Map.StopTravel(context);
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			bool flag = taiwu.GetLocation().AreaId != targetLocation.AreaId;
			if (flag)
			{
				KidnappedTravelData kidnappedData = new KidnappedTravelData
				{
					Target = targetLocation,
					HunterCharId = hunterCharId
				};
				DomainManager.Extra.SetKidnappedTravelData(kidnappedData, context);
				this.DirectTravel(context, targetLocation.AreaId);
			}
			else
			{
				GameDataBridge.AddDisplayEvent<bool, float, bool>(DisplayEventType.OperateBlackMaskView, true, 1f, true);
				this.SetTeleportMove(true);
				this.Move(context, targetLocation.BlockId, true);
				this.SetTeleportMove(false);
				DomainManager.TaiwuEvent.OnEvent_TaiwuBeHuntedArrivedSect(hunterCharId);
			}
		}

		// Token: 0x06007A21 RID: 31265 RVA: 0x0047BB30 File Offset: 0x00479D30
		public void DirectTravel(DataContext context, short toAreaId)
		{
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			Location taiwuLocation = taiwuChar.GetLocation();
			CrossAreaMoveInfo moveInfo = this.GetTravelCost(taiwuLocation.AreaId, taiwuLocation.BlockId, toAreaId);
			this.DirectTravel(context, moveInfo);
		}

		// Token: 0x06007A22 RID: 31266 RVA: 0x0047BB70 File Offset: 0x00479D70
		public void DirectTravel(DataContext context, CrossAreaMoveInfo moveInfo)
		{
			bool flag = !DomainManager.Extra.GetIsDirectTraveling();
			if (flag)
			{
				DomainManager.Extra.SetIsDirectTraveling(true, context);
			}
			this.StartTravelWithoutCost(context, moveInfo);
		}

		// Token: 0x06007A23 RID: 31267 RVA: 0x0047BBA8 File Offset: 0x00479DA8
		public void StartTravelWithoutCost(DataContext context, CrossAreaMoveInfo moveInfo)
		{
			bool traveling = this._travelInfo.Traveling;
			if (traveling)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Last travel to area ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(this._travelInfo.ToAreaId);
				defaultInterpolatedStringHandler.AppendLiteral(" is not finished");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.SetTravelInfo(moveInfo, context);
			HashSet<int> groupCharIds = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
			Location invalidLocation = Location.Invalid;
			foreach (int charId in groupCharIds)
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
				character.SetLocation(invalidLocation, context);
			}
			DomainManager.Extra.GearMateFollowTaiwu(context);
		}

		// Token: 0x06007A24 RID: 31268 RVA: 0x0047BC90 File Offset: 0x00479E90
		public Location GetTravelCurrLocation()
		{
			bool flag = this._travelInfo.ToAreaId < 0;
			if (flag)
			{
				throw new Exception("Taiwu is not travelling");
			}
			short areaId = this._travelInfo.CurrentAreaId;
			Location result = new Location(areaId, this._areas[(int)areaId].StationBlockId);
			bool flag2 = !result.IsValid();
			if (flag2)
			{
				result.AreaId = this._travelInfo.FromAreaId;
				result.BlockId = this._travelInfo.FromBlockId;
			}
			return result;
		}

		// Token: 0x06007A25 RID: 31269 RVA: 0x0047BD18 File Offset: 0x00479F18
		internal bool UnlockTravelPathInternal(DataContext context, CrossAreaMoveInfo moveInfo)
		{
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			bool flag = moveInfo.AuthorityCost <= 0;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = taiwuChar.GetResource(7) < moveInfo.AuthorityCost;
				if (flag2)
				{
					result = false;
				}
				else
				{
					taiwuChar.ChangeResource(context, 7, -moveInfo.AuthorityCost);
					MapDomain.AddUnlockStationSeniority(context, moveInfo.AuthorityCost);
					for (int i = 0; i < moveInfo.Route.AreaList.Count; i++)
					{
						short areaId = moveInfo.Route.AreaList[i];
						bool flag3 = !this._areas[(int)areaId].StationUnlocked;
						if (flag3)
						{
							this.UnlockStation(context, areaId, false);
							DomainManager.Taiwu.AddLegacyPoint(context, 37, 100);
						}
						foreach (short neighborAreaId in this._areas[(int)areaId].NeighborAreas)
						{
							MapAreaData neighborArea = this._areas[(int)neighborAreaId];
							bool discovered = neighborArea.Discovered;
							if (!discovered)
							{
								neighborArea.Discovered = true;
								this.SetElement_Areas((int)neighborAreaId, neighborArea, context);
							}
						}
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06007A26 RID: 31270 RVA: 0x0047BE74 File Offset: 0x0047A074
		private void EnsureCostDays(DataContext context)
		{
			bool flag = !this._travelInfo.Traveling;
			if (!flag)
			{
				this._carrierReduceTravelCostDaysPercent = this.CalcCarrierReduceTravelCostDaysPercent();
				int dynamicCost = this.GetAreaCostDays(this._travelInfo.CurrentAreaId, this._travelInfo.NextAreaId);
				bool flag2 = dynamicCost < 0;
				if (!flag2)
				{
					short nextCost = this._travelInfo.Route.CostList[this._travelInfo.RouteIndex + 1];
					bool flag3 = (int)nextCost == dynamicCost;
					if (!flag3)
					{
						int correction = Math.Min((int)nextCost - dynamicCost, this._travelInfo.NextCostDays - 1);
						bool flag4 = correction == 0;
						if (!flag4)
						{
							List<short> costList = this._travelInfo.Route.CostList;
							int index = this._travelInfo.RouteIndex + 1;
							costList[index] -= (short)correction;
							this.SetTravelInfo(this._travelInfo, context);
						}
					}
				}
			}
		}

		// Token: 0x06007A27 RID: 31271 RVA: 0x0047BF68 File Offset: 0x0047A168
		private IEnumerable<short> GetAllAreaIds()
		{
			short i = 0;
			while ((int)i < this._areas.Length)
			{
				yield return i;
				short num = i;
				i = num + 1;
			}
			yield break;
		}

		// Token: 0x06007A28 RID: 31272 RVA: 0x0047BF78 File Offset: 0x0047A178
		[return: TupleElementNames(new string[]
		{
			"area",
			"cost"
		})]
		private IEnumerable<ValueTuple<short, int>> GetAreaNeighbors(short areaId)
		{
			MapAreaItem config = this._areas[(int)areaId].GetConfig();
			foreach (AreaTravelRoute neighborArea in config.NeighborAreas)
			{
				yield return new ValueTuple<short, int>(this.GetAreaIdByAreaTemplateId(neighborArea.DestAreaId), this.GetAreaPathDays(neighborArea.CostDays));
				neighborArea = default(AreaTravelRoute);
			}
			AreaTravelRoute[] array = null;
			foreach (MapAreaItem extraConfig in ((IEnumerable<MapAreaItem>)MapArea.Instance))
			{
				foreach (AreaTravelRoute neighborArea2 in extraConfig.NeighborAreas)
				{
					bool flag = neighborArea2.DestAreaId == config.TemplateId;
					if (flag)
					{
						yield return new ValueTuple<short, int>(this.GetAreaIdByAreaTemplateId(extraConfig.TemplateId), this.GetAreaPathDays(neighborArea2.CostDays));
					}
					neighborArea2 = default(AreaTravelRoute);
				}
				AreaTravelRoute[] array2 = null;
				extraConfig = null;
			}
			IEnumerator<MapAreaItem> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06007A29 RID: 31273 RVA: 0x0047BF90 File Offset: 0x0047A190
		private int GetAreaCostDays(short areaA, short areaB)
		{
			bool flag = areaA == areaB;
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				MapAreaItem configA = this._areas[(int)areaA].GetConfig();
				MapAreaItem configB = this._areas[(int)areaB].GetConfig();
				foreach (AreaTravelRoute route in configA.NeighborAreas)
				{
					bool flag2 = route.DestAreaId == configB.TemplateId;
					if (flag2)
					{
						return this.GetAreaCostDays(route.CostDays);
					}
				}
				foreach (AreaTravelRoute route2 in configB.NeighborAreas)
				{
					bool flag3 = route2.DestAreaId == configA.TemplateId;
					if (flag3)
					{
						return this.GetAreaCostDays(route2.CostDays);
					}
				}
				result = -1;
			}
			return result;
		}

		// Token: 0x06007A2A RID: 31274 RVA: 0x0047C068 File Offset: 0x0047A268
		private int GetAreaPathDays(short configCostDays)
		{
			return Math.Max(this.GetAreaCostDays(configCostDays), 1) * 1000 + 1;
		}

		// Token: 0x06007A2B RID: 31275 RVA: 0x0047C090 File Offset: 0x0047A290
		private int GetAreaCostDays(short configCostDays)
		{
			return (int)configCostDays - (int)configCostDays * this._carrierReduceTravelCostDaysPercent / 100;
		}

		// Token: 0x06007A2C RID: 31276 RVA: 0x0047C0B0 File Offset: 0x0047A2B0
		private int CalcCarrierReduceTravelCostDaysPercent()
		{
			KidnappedTravelData kidnappedData = DomainManager.Extra.GetKidnappedTravelData();
			bool valid = kidnappedData.Valid;
			int result;
			if (valid)
			{
				result = (int)Config.Carrier.Instance[26].BaseTravelTimeReduction;
			}
			else
			{
				GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
				ItemKey carrierKey = taiwuChar.GetEquipment()[11];
				bool flag = !carrierKey.IsValid();
				if (flag)
				{
					result = 0;
				}
				else
				{
					GameData.Domains.Item.Carrier carrier = DomainManager.Item.GetElement_Carriers(carrierKey.Id);
					result = (int)((carrier.GetCurrDurability() > 0) ? carrier.GetTravelTimeReduction() : 0);
				}
			}
			return result;
		}

		// Token: 0x06007A2D RID: 31277 RVA: 0x0047C144 File Offset: 0x0047A344
		private void FinishTravel(DataContext context, short destAreaId)
		{
			short destBlockId = (destAreaId == this._travelInfo.FromAreaId) ? this._travelInfo.FromBlockId : this.GetPureTravelBlockId(context, destAreaId, this._travelInfo.FromAreaId);
			Location arriveLocation = new Location(destAreaId, destBlockId);
			bool isDirectTraveling = DomainManager.Extra.GetIsDirectTraveling();
			if (isDirectTraveling)
			{
				DomainManager.Extra.SetIsDirectTraveling(false, context);
			}
			KidnappedTravelData kidnappedData = DomainManager.Extra.GetKidnappedTravelData();
			bool valid = kidnappedData.Valid;
			if (valid)
			{
				bool flag = destAreaId == kidnappedData.Target.AreaId;
				if (flag)
				{
					arriveLocation.BlockId = kidnappedData.Target.BlockId;
					GameDataBridge.AddDisplayEvent(DisplayEventType.HidePartWorldMap);
					DomainManager.TaiwuEvent.OnEvent_TaiwuBeHuntedArrivedSect(kidnappedData.HunterCharId);
					DomainManager.Extra.SetKidnappedTravelData(KidnappedTravelData.Invalid, context);
				}
			}
			HashSet<int> groupCharIds = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
			foreach (int charId in groupCharIds)
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
				character.SetLocation(arriveLocation, context);
			}
			DomainManager.Extra.GearMateFollowTaiwu(context);
			this._travelInfo.ToAreaId = -1;
			this.SetTravelInfo(this._travelInfo, context);
			MapAreaData areaData = this.GetElement_Areas((int)destAreaId);
			bool flag2 = !DomainManager.Map.IsAreaBroken(destAreaId);
			if (flag2)
			{
				sbyte stateId = areaData.GetConfig().StateID;
				bool flag3 = !DomainManager.Extra.CheckIsUnlockedSectXuannvMusicByMapStateId(stateId);
				if (flag3)
				{
					DomainManager.Extra.UnlockSectXuannvMusicByMapStateId(context, stateId);
				}
			}
			DomainManager.Merchant.RefreshCaravanInTaiwuState(context);
			DomainManager.Extra.ClearTravelingEvents(context);
			DomainManager.Extra.ClearGainsInTravel(context);
		}

		// Token: 0x06007A2E RID: 31278 RVA: 0x0047C318 File Offset: 0x0047A518
		private short GetPureTravelBlockId(DataContext context, short destAreaId, short fromAreaId)
		{
			short destBlockId = this._areas[(int)destAreaId].StationBlockId;
			short destAreaId2 = destAreaId;
			bool flag = destAreaId2 - 137 <= 1;
			bool flag2 = !flag;
			bool flag3 = flag2;
			if (flag3)
			{
				bool flag4 = fromAreaId == 135 || fromAreaId == 138;
				flag3 = !flag4;
			}
			bool flag5 = flag3;
			short result;
			if (flag5)
			{
				result = destBlockId;
			}
			else
			{
				List<short> edgeBlocks = new List<short>();
				this.GetEdgeBlockList(destAreaId, edgeBlocks, true, true);
				edgeBlocks.RemoveAll((short blockId) => this.GetBlockData(destAreaId, blockId).TemplateId == 124);
				edgeBlocks.RemoveAll(delegate(short blockId)
				{
					MapBlockData block = this.GetBlockData(destAreaId, blockId);
					bool flag6 = block.TemplateEnemyList != null && block.TemplateEnemyList.Count > 0;
					return flag6 || DomainManager.Extra.IsLocationContainsAnimal(new Location(destAreaId, blockId));
				});
				destBlockId = edgeBlocks.GetRandom(context.Random);
				MapBlockData destBlock = this.GetBlock(destAreaId, destBlockId);
				this.SetBlockAndViewRangeVisibleByMove(context, destBlock);
				result = destBlockId;
			}
			return result;
		}

		// Token: 0x06007A2F RID: 31279 RVA: 0x0047C410 File Offset: 0x0047A610
		private static void AddUnlockStationSeniority(DataContext context, int needAuthority)
		{
			ProfessionFormulaItem formula = ProfessionFormula.Instance[73];
			int addSeniority = formula.Calculate(needAuthority);
			DomainManager.Extra.ChangeProfessionSeniority(context, 11, addSeniority, true, false);
		}

		// Token: 0x06007A30 RID: 31280 RVA: 0x0047C444 File Offset: 0x0047A644
		public unsafe short DetectTravelingEvent(DataContext context)
		{
			List<ValueTuple<short, short>> weightTable = this._travelingEventWeights;
			weightTable.Clear();
			int routeIndex = this._travelInfo.RouteIndex;
			short currAreaId = this._travelInfo.CurrentAreaId;
			short fromAreaId = this._travelInfo.LastAreaId;
			short destAreaId = this._travelInfo.NextAreaId;
			bool flag = this._travelInfo.ToAreaId < 0;
			if (flag)
			{
				fromAreaId = (currAreaId = (destAreaId = DomainManager.Taiwu.GetTaiwu().GetLocation().AreaId));
			}
			sbyte currStateTemplateId = this.GetStateTemplateIdByAreaId(currAreaId);
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			Personalities personalities = taiwuChar.GetPersonalities();
			sbyte fame = taiwuChar.GetFame();
			List<short> costList = this._travelInfo.Route.CostList;
			int leftDays = DomainManager.World.GetLeftDaysInCurrMonth() - (int)((costList.CheckIndex(routeIndex) ? costList[routeIndex] : costList[0]) + 1);
			foreach (TravelingEventItem cfg in ((IEnumerable<TravelingEventItem>)TravelingEvent.Instance))
			{
				bool flag2 = cfg.OccurRate <= 0;
				if (!flag2)
				{
					bool flag3 = leftDays < (int)cfg.NeedTime;
					if (!flag3)
					{
						bool flag4 = fame < cfg.FameLimit[0] || fame > cfg.FameLimit[1];
						if (!flag4)
						{
							bool flag5 = !this.CheckValidAreaToTrigger(cfg, currAreaId, fromAreaId, destAreaId);
							if (!flag5)
							{
								bool flag6 = cfg.StateTemplateId >= 0 && cfg.StateTemplateId != currStateTemplateId;
								if (!flag6)
								{
									bool flag7 = cfg.IsUnique && DomainManager.Extra.HasTravelingEventBeenTriggered(cfg.TemplateId);
									if (!flag7)
									{
										bool flag8 = !this.CheckTravelingEventSpecialCondition(context.Random, cfg.TemplateId);
										if (!flag8)
										{
											int occurRate = (int)cfg.OccurRate;
											bool flag9 = cfg.NeedPersonality >= 0;
											if (flag9)
											{
												sbyte personality = *(ref personalities.Items.FixedElementField + cfg.NeedPersonality);
												occurRate = occurRate * (int)personality / 50;
											}
											bool flag10 = cfg.FameMultiplier != 0;
											if (flag10)
											{
												occurRate = occurRate * (int)cfg.FameMultiplier * (int)fame / 100;
											}
											weightTable.Add(new ValueTuple<short, short>(cfg.TemplateId, (short)Math.Clamp(occurRate, 0, 32767)));
										}
									}
								}
							}
						}
					}
				}
			}
			CollectionUtils.Shuffle<ValueTuple<short, short>>(context.Random, weightTable);
			weightTable.Sort(new Comparison<ValueTuple<short, short>>(MapDomain.<DetectTravelingEvent>g__CompareTravelingEventsDec|472_0));
			foreach (ValueTuple<short, short> valueTuple in weightTable)
			{
				short templateId = valueTuple.Item1;
				short occurRate2 = valueTuple.Item2;
				bool flag11 = !context.Random.CheckPercentProb((int)occurRate2);
				if (!flag11)
				{
					int offset = this.AddTravelingEvent(context, templateId, currAreaId);
					bool flag12 = offset < 0;
					if (!flag12)
					{
						DomainManager.Extra.AddTriggeredTravelingEvent(context, templateId);
						TravelingEventItem configData = TravelingEvent.Instance[templateId];
						bool flag13 = string.IsNullOrEmpty(configData.Event);
						if (flag13)
						{
							return templateId;
						}
						TravelingEventCollection travelingEventCollection = DomainManager.Extra.GetTravelingEventCollection();
						TaiwuEvent taiwuEvent = DomainManager.TaiwuEvent.GetEvent(configData.Event);
						bool flag14 = taiwuEvent != null;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
						if (!flag14)
						{
							Logger logger = MapDomain.Logger;
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 3);
							defaultInterpolatedStringHandler.AppendLiteral("Monthly Event ");
							defaultInterpolatedStringHandler.AppendFormatted<short>(templateId);
							defaultInterpolatedStringHandler.AppendLiteral(" - ");
							defaultInterpolatedStringHandler.AppendFormatted(configData.Name);
							defaultInterpolatedStringHandler.AppendLiteral(" (");
							defaultInterpolatedStringHandler.AppendFormatted(configData.Event);
							defaultInterpolatedStringHandler.AppendLiteral(") not found.");
							logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
							return -1;
						}
						TaiwuEvent taiwuEvent2 = taiwuEvent;
						if (taiwuEvent2.ArgBox == null)
						{
							taiwuEvent2.ArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
						}
						travelingEventCollection.FillEventArgBox(offset, taiwuEvent.ArgBox);
						bool flag15 = taiwuEvent.EventConfig.CheckCondition();
						if (flag15)
						{
							DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
							DomainManager.TaiwuEvent.TravelingEventCheckComplete();
							DomainManager.Map.SetOnHandlingTravelingEventBlock(true, context);
							ProfessionFormulaItem formula = ProfessionFormula.Instance[76];
							int addSeniority = formula.Calculate();
							DomainManager.Extra.ChangeProfessionSeniority(context, 11, addSeniority, true, false);
							return templateId;
						}
						int size = travelingEventCollection.GetRecordSize(offset);
						travelingEventCollection.Remove(offset, size);
						Logger logger2 = MapDomain.Logger;
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(75, 3);
						defaultInterpolatedStringHandler.AppendLiteral("Traveling event ");
						defaultInterpolatedStringHandler.AppendFormatted<short>(templateId);
						defaultInterpolatedStringHandler.AppendLiteral(" - ");
						defaultInterpolatedStringHandler.AppendFormatted(configData.Name);
						defaultInterpolatedStringHandler.AppendLiteral(" is triggering ");
						defaultInterpolatedStringHandler.AppendFormatted(taiwuEvent.EventGuid);
						defaultInterpolatedStringHandler.AppendLiteral(" when OnCheckEventCondition return false.");
						logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
						return -1;
					}
				}
			}
			return -1;
		}

		// Token: 0x06007A31 RID: 31281 RVA: 0x0047C9B4 File Offset: 0x0047ABB4
		private bool CheckValidAreaToTrigger(TravelingEventItem config, short currAreaId, short fromAreaId, short toAreaId)
		{
			bool result;
			switch (config.TriggerType)
			{
			case ETravelingEventTriggerType.Any:
				result = (this.CheckTriggerAreaType(config.TriggerAreaType, fromAreaId) || this.CheckTriggerAreaType(config.TriggerAreaType, currAreaId) || this.CheckTriggerAreaType(config.TriggerAreaType, toAreaId));
				break;
			case ETravelingEventTriggerType.OnArea:
				result = this.CheckTriggerAreaType(config.TriggerAreaType, currAreaId);
				break;
			case ETravelingEventTriggerType.ToArea:
				result = this.CheckTriggerAreaType(config.TriggerAreaType, toAreaId);
				break;
			case ETravelingEventTriggerType.FromArea:
				result = this.CheckTriggerAreaType(config.TriggerAreaType, fromAreaId);
				break;
			default:
				result = false;
				break;
			}
			return result;
		}

		// Token: 0x06007A32 RID: 31282 RVA: 0x0047CA4C File Offset: 0x0047AC4C
		private bool CheckTriggerAreaType(ETravelingEventTriggerAreaType areaType, short areaId)
		{
			bool result;
			switch (areaType)
			{
			case ETravelingEventTriggerAreaType.Any:
				result = true;
				break;
			case ETravelingEventTriggerAreaType.NormalArea:
				result = (areaId < 45);
				break;
			case ETravelingEventTriggerAreaType.BrokenArea:
				result = (areaId >= 45 && areaId < 135);
				break;
			case ETravelingEventTriggerAreaType.SectArea:
			{
				MapAreaItem areaCfg = this._areas[(int)areaId].GetConfig();
				result = ((short)MapState.Instance[areaCfg.StateID].SectAreaID == areaCfg.TemplateId);
				break;
			}
			case ETravelingEventTriggerAreaType.MainCityArea:
			{
				MapAreaItem areaCfg2 = this._areas[(int)areaId].GetConfig();
				result = ((short)MapState.Instance[areaCfg2.StateID].MainAreaID == areaCfg2.TemplateId);
				break;
			}
			default:
				result = false;
				break;
			}
			return result;
		}

		// Token: 0x06007A33 RID: 31283 RVA: 0x0047CB08 File Offset: 0x0047AD08
		private int AddTravelingEvent(DataContext context, short templateId, short areaId)
		{
			TravelingEventCollection travelingEventCollection = DomainManager.Extra.GetTravelingEventCollection();
			TravelingEventItem config = TravelingEvent.Instance[templateId];
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			Location location = new Location(areaId, -1);
			switch (config.Type)
			{
			case ETravelingEventType.AreaMaterial:
			{
				ValueTuple<sbyte, short> valueTuple = this.GenerateTravelingEventItemParameter(context, config);
				sbyte itemType = valueTuple.Item1;
				short itemTemplateId = valueTuple.Item2;
				bool flag = string.IsNullOrEmpty(config.Event);
				if (flag)
				{
					ItemKey itemKey = DomainManager.Item.CreateItem(context, itemType, itemTemplateId);
					DomainManager.Extra.AddGainsInTravel(context, itemKey);
					taiwuChar.AddInventoryItem(context, itemKey, 1, false);
				}
				return travelingEventCollection.AddType_AreaMaterial(templateId, taiwuCharId, itemType, itemTemplateId);
			}
			case ETravelingEventType.AreaResource:
			{
				ValueTuple<sbyte, int> valueTuple2 = this.GenerateTravelingEventResourceParameter(context, config);
				sbyte resourceType = valueTuple2.Item1;
				int amount = valueTuple2.Item2;
				bool flag2 = resourceType < 0 || amount <= 0;
				if (flag2)
				{
					return -1;
				}
				bool flag3 = string.IsNullOrEmpty(config.Event);
				if (flag3)
				{
					taiwuChar.ChangeResource(context, resourceType, amount);
				}
				return travelingEventCollection.AddType_AreaResource(templateId, taiwuCharId, amount, resourceType);
			}
			case ETravelingEventType.AreaFood:
			{
				ValueTuple<sbyte, short> valueTuple3 = this.GenerateTravelingEventItemParameter(context, config);
				sbyte itemType2 = valueTuple3.Item1;
				short itemTemplateId2 = valueTuple3.Item2;
				bool flag4 = string.IsNullOrEmpty(config.Event);
				if (flag4)
				{
					ItemKey itemKey2 = DomainManager.Item.CreateItem(context, itemType2, itemTemplateId2);
					DomainManager.Extra.AddGainsInTravel(context, itemKey2);
					taiwuChar.AddInventoryItem(context, itemKey2, 1, false);
				}
				return travelingEventCollection.AddType_AreaFood(templateId, taiwuCharId, itemType2, itemTemplateId2);
			}
			case ETravelingEventType.Heal:
			{
				int value = context.Random.Next(config.ValueRange[0], config.ValueRange[1]);
				switch (templateId)
				{
				case 45:
				{
					sbyte bodyPartType = taiwuChar.GetRandomInjuredBodyPartToHeal(context.Random, false, 6);
					taiwuChar.ChangeInjury(context, bodyPartType, false, (sbyte)(-(sbyte)value));
					return travelingEventCollection.AddHealOuterInjury(taiwuCharId, value);
				}
				case 46:
				{
					sbyte bodyPartType2 = taiwuChar.GetRandomInjuredBodyPartToHeal(context.Random, true, 6);
					bool flag5 = bodyPartType2 < 0;
					if (flag5)
					{
						return -1;
					}
					taiwuChar.ChangeInjury(context, bodyPartType2, true, (sbyte)(-(sbyte)value));
					return travelingEventCollection.AddHealInnerInjury(taiwuCharId, value);
				}
				case 47:
				{
					sbyte poisonType = taiwuChar.GetRandomPoisonTypeToDetox(context.Random, 3);
					bool flag6 = poisonType < 0;
					if (flag6)
					{
						return -1;
					}
					taiwuChar.ChangePoisoned(context, poisonType, 3, -value);
					return travelingEventCollection.AddHealPoison(taiwuCharId, poisonType);
				}
				case 48:
					taiwuChar.ChangeDisorderOfQi(context, (int)((short)(-(short)value)));
					return travelingEventCollection.AddHealDisorderOfQi(taiwuCharId, value);
				case 49:
					taiwuChar.ChangeHealth(context, value);
					return travelingEventCollection.AddHealLifeSpan(taiwuCharId, value);
				default:
					return -1;
				}
				break;
			}
			case ETravelingEventType.CharacterGiftItem:
			{
				int charId = (config.FameMultiplier != 0) ? this.GenerateTravelingEventAreaCharacterParameter(context, config, areaId) : this.GenerateTravelingEventFriendOrFamilyCharacterParameter(context, config, areaId);
				bool flag7 = charId < 0;
				if (flag7)
				{
					return -1;
				}
				ItemKey itemKey3 = this.GenerateTravelingEventCharacterItemParameter(context, config, charId);
				bool flag8 = !itemKey3.IsValid();
				if (flag8)
				{
					return -1;
				}
				bool flag9 = string.IsNullOrEmpty(config.Event);
				if (flag9)
				{
					GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
					DomainManager.Character.TransferInventoryItem(context, character, taiwuChar, itemKey3, 1);
					DomainManager.Extra.AddGainsInTravel(context, itemKey3);
				}
				return travelingEventCollection.AddType_CharacterGiftItem(templateId, taiwuCharId, location, charId, itemKey3.ItemType, itemKey3.TemplateId);
			}
			case ETravelingEventType.CharacterGiftResource:
			{
				int charId2 = (config.FameMultiplier != 0) ? this.GenerateTravelingEventAreaCharacterParameter(context, config, areaId) : this.GenerateTravelingEventFriendOrFamilyCharacterParameter(context, config, areaId);
				bool flag10 = charId2 < 0;
				if (flag10)
				{
					return -1;
				}
				ValueTuple<sbyte, int> valueTuple4 = this.GenerateTravelingEventCharacterResourceParameter(context, config, charId2);
				sbyte resourceType2 = valueTuple4.Item1;
				int amount2 = valueTuple4.Item2;
				bool flag11 = resourceType2 < 0 || amount2 <= 0;
				if (flag11)
				{
					return -1;
				}
				bool flag12 = string.IsNullOrEmpty(config.Event);
				if (flag12)
				{
					GameData.Domains.Character.Character character2 = DomainManager.Character.GetElement_Objects(charId2);
					DomainManager.Character.TransferResource(context, character2, taiwuChar, resourceType2, amount2);
				}
				return travelingEventCollection.AddType_CharacterGiftResource(templateId, taiwuCharId, location, charId2, amount2, resourceType2);
			}
			case ETravelingEventType.AttributeRegen:
			{
				bool flag13 = string.IsNullOrEmpty(config.Event);
				if (flag13)
				{
					sbyte mainAttrType = (sbyte)(config.CharacterProperty - 0);
					taiwuChar.ChangeCurrMainAttribute(context, mainAttrType, 10);
				}
				return travelingEventCollection.AddType_AttributeRegen(templateId, taiwuCharId, location, 10);
			}
			case ETravelingEventType.AreaInteraction:
			{
				int targetCharId = this.GenerateTravelingEventAreaCharacterParameter(context, config, location.AreaId);
				bool flag14 = targetCharId < 0;
				if (flag14)
				{
					return -1;
				}
				return travelingEventCollection.AddType_AreaInteraction(templateId, taiwuCharId, location, targetCharId);
			}
			case ETravelingEventType.SpiritualDebt:
			{
				short settlementId = this.GenerateTravelingEventSettlementParameter(context, config, location.AreaId);
				bool flag15 = settlementId < 0;
				if (flag15)
				{
					return -1;
				}
				return travelingEventCollection.AddType_SpiritualDebt(templateId, taiwuCharId, settlementId);
			}
			case ETravelingEventType.SectVisit:
				return travelingEventCollection.AddType_SectVisit(templateId, taiwuCharId, location);
			case ETravelingEventType.Combat:
			{
				short enemyCharTemplateId = this.GenerateTravelingEventEnemyCharTemplateParameter(context, config);
				bool flag16 = enemyCharTemplateId < 0;
				if (flag16)
				{
					return -1;
				}
				return travelingEventCollection.AddType_Combat(templateId, taiwuCharId, enemyCharTemplateId);
			}
			case ETravelingEventType.SectCombat:
			{
				short enemyCharTemplateId2 = this.GenerateTravelingEventEnemyCharTemplateParameter(context, config);
				bool flag17 = enemyCharTemplateId2 < 0;
				if (flag17)
				{
					return -1;
				}
				return travelingEventCollection.AddType_SectCombat(templateId, taiwuCharId, enemyCharTemplateId2);
			}
			case ETravelingEventType.CharacterRecommendVillager:
			{
				int recommenderCharId = (config.FameMultiplier != 0) ? this.GenerateTravelingEventAreaCharacterParameter(context, config, areaId) : this.GenerateTravelingEventFriendOrFamilyCharacterParameter(context, config, areaId);
				bool flag18 = recommenderCharId < 0;
				if (flag18)
				{
					return -1;
				}
				int recommendedCharId = this.GetRecommendedCharacterId(context, recommenderCharId);
				bool flag19 = recommendedCharId < 0;
				if (flag19)
				{
					return -1;
				}
				return travelingEventCollection.AddType_CharacterRecommendVillager(templateId, taiwuCharId, location, recommenderCharId, recommendedCharId);
			}
			case ETravelingEventType.AttributeCost:
				return travelingEventCollection.AddType_AttributeCost(templateId, taiwuCharId, 10);
			case ETravelingEventType.CarrierDurability:
			{
				ItemKey carrierKey = taiwuChar.GetEquipment()[11];
				EquipmentBase carrier = DomainManager.Item.TryGetBaseEquipment(carrierKey);
				bool flag20 = (carrier == null || carrier.GetCurrDurability() == 0) && DomainManager.World.GetLeftDaysInCurrMonth() < (int)(config.NeedTime + 3);
				if (flag20)
				{
					return -1;
				}
				return travelingEventCollection.AddRoadBlock(taiwuCharId);
			}
			}
			return -1;
		}

		// Token: 0x06007A34 RID: 31284 RVA: 0x0047D168 File Offset: 0x0047B368
		private bool CheckTravelingEventSpecialCondition(IRandomSource random, short templateId)
		{
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			bool result;
			switch (templateId)
			{
			case 45:
				result = (taiwuChar.GetRandomInjuredBodyPartToHeal(random, false, 6) >= 0);
				break;
			case 46:
				result = (taiwuChar.GetRandomInjuredBodyPartToHeal(random, true, 6) >= 0);
				break;
			case 47:
				result = (taiwuChar.GetRandomPoisonTypeToDetox(random, 3) >= 0);
				break;
			case 48:
				result = (taiwuChar.GetDisorderOfQi() > 0);
				break;
			case 49:
				result = (taiwuChar.GetHealth() < taiwuChar.GetLeftMaxHealth(false));
				break;
			default:
			{
				TravelingEventItem config = TravelingEvent.Instance.GetItem(templateId);
				bool flag = config != null;
				if (flag)
				{
					bool flag2 = config.Type == ETravelingEventType.Combat && DomainManager.Taiwu.IsTaiwuAvoidTravelingEnemies();
					if (flag2)
					{
						result = false;
						break;
					}
				}
				result = true;
				break;
			}
			}
			return result;
		}

		// Token: 0x06007A35 RID: 31285 RVA: 0x0047D240 File Offset: 0x0047B440
		[return: TupleElementNames(new string[]
		{
			"itemType",
			"itemTemplateId"
		})]
		private ValueTuple<sbyte, short> GenerateTravelingEventItemParameter(DataContext context, TravelingEventItem config)
		{
			Tester.Assert(config.ItemRange.Count > 0 && config.ItemGradeWeight != null && config.ItemGradeWeight.Length != 0, "");
			List<TemplateKey> validTemplateKeys = context.AdvanceMonthRelatedData.ItemTemplateKeys.Occupy();
			int grade = RandomUtils.GetRandomIndex(config.ItemGradeWeight, context.Random);
			PresetItemTemplateIdGroup presetItemTemplateIdGroup = config.ItemRange.GetRandom(context.Random);
			for (int i = 0; i < (int)presetItemTemplateIdGroup.GroupLength; i++)
			{
				short currTemplateId = (short)((int)presetItemTemplateIdGroup.StartId + i);
				bool flag = (int)ItemTemplateHelper.GetGrade(presetItemTemplateIdGroup.ItemType, currTemplateId) == grade;
				if (flag)
				{
					validTemplateKeys.Add(new TemplateKey(presetItemTemplateIdGroup.ItemType, currTemplateId));
				}
			}
			bool flag2 = validTemplateKeys.Count == 0;
			if (flag2)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(55, 4);
				defaultInterpolatedStringHandler.AppendLiteral("No valid item of type ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(presetItemTemplateIdGroup.ItemType);
				defaultInterpolatedStringHandler.AppendLiteral(" and grade ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(grade);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendLiteral("for traveling event ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(config.TemplateId);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted(config.Name);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			TemplateKey randomKey = validTemplateKeys.GetRandom(context.Random);
			context.AdvanceMonthRelatedData.ItemTemplateKeys.Release(ref validTemplateKeys);
			return new ValueTuple<sbyte, short>(randomKey.ItemType, randomKey.TemplateId);
		}

		// Token: 0x06007A36 RID: 31286 RVA: 0x0047D3D8 File Offset: 0x0047B5D8
		private ItemKey GenerateTravelingEventCharacterItemParameter(DataContext context, TravelingEventItem config, int charId)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			Inventory inventory = character.GetInventory();
			List<ItemKey> validItemKeys = context.AdvanceMonthRelatedData.ItemKeys.Occupy();
			List<ValueTuple<ItemBase, int>>[] classifiedItems = context.AdvanceMonthRelatedData.ClassifiedItems.Occupy();
			foreach (KeyValuePair<ItemKey, int> keyValuePair in inventory.Items)
			{
				ItemKey itemKey2;
				int num;
				keyValuePair.Deconstruct(out itemKey2, out num);
				ItemKey itemKey = itemKey2;
				int amount = num;
				bool flag = itemKey.ItemType != config.FilterItemType;
				if (!flag)
				{
					bool flag2 = ItemTemplateHelper.IsTransferable(itemKey.ItemType, itemKey.TemplateId);
					if (!flag2)
					{
						ItemBase item = DomainManager.Item.GetBaseItem(itemKey);
						classifiedItems[(int)(item.GetGrade() / 3)].Add(new ValueTuple<ItemBase, int>(item, amount));
					}
				}
			}
			foreach (List<ValueTuple<ItemBase, int>> gradeGroup in classifiedItems)
			{
				bool flag3 = gradeGroup.Count > 0;
				if (flag3)
				{
					validItemKeys.Add(gradeGroup.GetRandom(context.Random).Item1.GetItemKey());
				}
			}
			ItemKey selectedItemKey = validItemKeys.GetRandomOrDefault(context.Random, ItemKey.Invalid);
			context.AdvanceMonthRelatedData.ItemKeys.Release(ref validItemKeys);
			context.AdvanceMonthRelatedData.ClassifiedItems.Release(ref classifiedItems);
			return selectedItemKey;
		}

		// Token: 0x06007A37 RID: 31287 RVA: 0x0047D564 File Offset: 0x0047B764
		private short GenerateTravelingEventSettlementParameter(DataContext context, TravelingEventItem config, short areaId)
		{
			List<short> settlementIds = ObjectPool<List<short>>.Instance.Get();
			settlementIds.Clear();
			foreach (SettlementInfo settlementInfo in this._areas[(int)areaId].SettlementInfos)
			{
				bool flag = settlementInfo.SettlementId >= 0 && this.GetBlock(areaId, settlementInfo.BlockId).BlockType != EMapBlockType.Sect;
				if (flag)
				{
					settlementIds.Add(settlementInfo.SettlementId);
				}
			}
			int selectedSettlementId = (int)((settlementIds.Count > 0) ? settlementIds.GetRandom(context.Random) : -1);
			ObjectPool<List<short>>.Instance.Return(settlementIds);
			return (short)selectedSettlementId;
		}

		// Token: 0x06007A38 RID: 31288 RVA: 0x0047D614 File Offset: 0x0047B814
		[return: TupleElementNames(new string[]
		{
			"resourceType",
			"amount"
		})]
		private ValueTuple<sbyte, int> GenerateTravelingEventResourceParameter(DataContext context, TravelingEventItem config)
		{
			Tester.Assert(config.ResourceWeights != null && config.ResourceWeights.Length != 0, "");
			sbyte resourceType = (sbyte)RandomUtils.GetRandomIndex(config.ResourceWeights, context.Random);
			int worth = context.Random.Next(50, 151) * (int)GlobalConfig.ResourcesWorth[0];
			int amount = worth / (int)GlobalConfig.ResourcesWorth[(int)resourceType];
			return new ValueTuple<sbyte, int>(resourceType, amount);
		}

		// Token: 0x06007A39 RID: 31289 RVA: 0x0047D688 File Offset: 0x0047B888
		[return: TupleElementNames(new string[]
		{
			"resourceType",
			"amount"
		})]
		private ValueTuple<sbyte, int> GenerateTravelingEventCharacterResourceParameter(DataContext context, TravelingEventItem config, int charId)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			List<sbyte> validResTypes = ObjectPool<List<sbyte>>.Instance.Get();
			validResTypes.Clear();
			for (sbyte resType = 0; resType < 7; resType += 1)
			{
				bool flag = character.GetResource(resType) < 100;
				if (!flag)
				{
					validResTypes.Add(resType);
				}
			}
			sbyte resourceType = (validResTypes.Count > 0) ? validResTypes.GetRandom(context.Random) : -1;
			ObjectPool<List<sbyte>>.Instance.Return(validResTypes);
			bool flag2 = resourceType < 0;
			ValueTuple<sbyte, int> result;
			if (flag2)
			{
				result = new ValueTuple<sbyte, int>(-1, 0);
			}
			else
			{
				int amount = character.GetResource(resourceType) / context.Random.Next(5, 11);
				result = new ValueTuple<sbyte, int>(resourceType, amount);
			}
			return result;
		}

		// Token: 0x06007A3A RID: 31290 RVA: 0x0047D748 File Offset: 0x0047B948
		private short GenerateTravelingEventEnemyCharTemplateParameter(DataContext context, TravelingEventItem config)
		{
			sbyte xiangshuLevel = DomainManager.World.GetXiangshuLevel();
			sbyte enemyGrade = Math.Clamp(xiangshuLevel, 0, 8);
			return CharacterDomain.GetRandomEnemyCharTemplateId(context.Random, config.OrgTemplateId, enemyGrade);
		}

		// Token: 0x06007A3B RID: 31291 RVA: 0x0047D780 File Offset: 0x0047B980
		private unsafe int GenerateTravelingEventAreaCharacterParameter(DataContext context, TravelingEventItem config, short areaId)
		{
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			List<int> charIdList = context.AdvanceMonthRelatedData.CharIdList.Occupy();
			Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks(areaId);
			int i = 0;
			int blocksCount = blocks.Length;
			while (i < blocksCount)
			{
				HashSet<int> charIds = blocks[i]->CharacterSet;
				bool flag = charIds == null;
				if (!flag)
				{
					foreach (int charId in charIds)
					{
						GameData.Domains.Character.Character character;
						bool flag2 = !DomainManager.Character.TryGetElement_Objects(charId, out character);
						if (!flag2)
						{
							bool flag3 = !character.IsInteractableAsIntelligentCharacter();
							if (!flag3)
							{
								bool flag4 = DomainManager.Character.HasRelation(charId, taiwuCharId, 32768);
								if (!flag4)
								{
									charIdList.Add(charId);
								}
							}
						}
					}
				}
				i++;
			}
			int selectedCharId = charIdList.GetRandomOrDefault(context.Random, -1);
			context.AdvanceMonthRelatedData.CharIdList.Release(ref charIdList);
			return selectedCharId;
		}

		// Token: 0x06007A3C RID: 31292 RVA: 0x0047D8B8 File Offset: 0x0047BAB8
		private unsafe int GenerateTravelingEventFriendOrFamilyCharacterParameter(DataContext context, TravelingEventItem config, short areaId)
		{
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			List<int> charIdList = context.AdvanceMonthRelatedData.CharIdList.Occupy();
			Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks(areaId);
			int i = 0;
			int blocksCount = blocks.Length;
			while (i < blocksCount)
			{
				HashSet<int> charIds = blocks[i]->CharacterSet;
				bool flag = charIds == null;
				if (!flag)
				{
					foreach (int charId in charIds)
					{
						GameData.Domains.Character.Character character;
						bool flag2 = !DomainManager.Character.TryGetElement_Objects(charId, out character);
						if (!flag2)
						{
							bool flag3 = !character.IsInteractableAsIntelligentCharacter();
							if (!flag3)
							{
								RelatedCharacter targetToSelf;
								bool flag4 = !DomainManager.Character.TryGetRelation(charId, taiwuCharId, out targetToSelf);
								if (!flag4)
								{
									bool flag5 = AiHelper.ActionTargetRelationCategory.GetTargetRelationCategory(targetToSelf.RelationType) != 1;
									if (!flag5)
									{
										charIdList.Add(charId);
									}
								}
							}
						}
					}
				}
				i++;
			}
			int selectedCharId = charIdList.GetRandomOrDefault(context.Random, -1);
			context.AdvanceMonthRelatedData.CharIdList.Release(ref charIdList);
			return selectedCharId;
		}

		// Token: 0x06007A3D RID: 31293 RVA: 0x0047DA08 File Offset: 0x0047BC08
		private int GetRecommendedCharacterId(DataContext context, int recommenderId)
		{
			HashSet<int> relatedCharIds = context.AdvanceMonthRelatedData.RelatedCharIds.Occupy();
			DomainManager.Character.GetAllTwoWayRelatedCharIds(recommenderId, relatedCharIds);
			OrganizationInfo taiwuOrgInfo = DomainManager.Taiwu.GetTaiwu().GetOrganizationInfo();
			sbyte xiangshuLevel = DomainManager.World.GetXiangshuLevel();
			sbyte maxGrade = GlobalConfig.Instance.XiangshuInfectionGradeUpperLimits[(int)xiangshuLevel];
			int charIdWithMaxFavor = -1;
			short maxFavor = -30000;
			foreach (int relatedCharId in relatedCharIds)
			{
				GameData.Domains.Character.Character character;
				bool flag = !DomainManager.Character.TryGetElement_Objects(relatedCharId, out character);
				if (!flag)
				{
					bool flag2 = !character.IsInteractableAsIntelligentCharacter();
					if (!flag2)
					{
						OrganizationInfo orgInfo = character.GetOrganizationInfo();
						bool flag3 = orgInfo.Grade > maxGrade;
						if (!flag3)
						{
							bool flag4 = orgInfo.OrgTemplateId == taiwuOrgInfo.OrgTemplateId;
							if (!flag4)
							{
								bool flag5 = character.IsTreasuryGuard();
								if (!flag5)
								{
									RelatedCharacter relation = DomainManager.Character.GetRelation(recommenderId, relatedCharId);
									bool flag6 = relation.Favorability > maxFavor;
									if (flag6)
									{
										charIdWithMaxFavor = relatedCharId;
										maxFavor = relation.Favorability;
									}
								}
							}
						}
					}
				}
			}
			context.AdvanceMonthRelatedData.RelatedCharIds.Release(ref relatedCharIds);
			return charIdWithMaxFavor;
		}

		// Token: 0x06007A3E RID: 31294 RVA: 0x0047DB68 File Offset: 0x0047BD68
		public MapDomain() : base(68)
		{
			this._areas = new MapAreaData[139];
			this._areaBlocks0 = new AreaBlockCollection();
			this._areaBlocks1 = new AreaBlockCollection();
			this._areaBlocks2 = new AreaBlockCollection();
			this._areaBlocks3 = new AreaBlockCollection();
			this._areaBlocks4 = new AreaBlockCollection();
			this._areaBlocks5 = new AreaBlockCollection();
			this._areaBlocks6 = new AreaBlockCollection();
			this._areaBlocks7 = new AreaBlockCollection();
			this._areaBlocks8 = new AreaBlockCollection();
			this._areaBlocks9 = new AreaBlockCollection();
			this._areaBlocks10 = new AreaBlockCollection();
			this._areaBlocks11 = new AreaBlockCollection();
			this._areaBlocks12 = new AreaBlockCollection();
			this._areaBlocks13 = new AreaBlockCollection();
			this._areaBlocks14 = new AreaBlockCollection();
			this._areaBlocks15 = new AreaBlockCollection();
			this._areaBlocks16 = new AreaBlockCollection();
			this._areaBlocks17 = new AreaBlockCollection();
			this._areaBlocks18 = new AreaBlockCollection();
			this._areaBlocks19 = new AreaBlockCollection();
			this._areaBlocks20 = new AreaBlockCollection();
			this._areaBlocks21 = new AreaBlockCollection();
			this._areaBlocks22 = new AreaBlockCollection();
			this._areaBlocks23 = new AreaBlockCollection();
			this._areaBlocks24 = new AreaBlockCollection();
			this._areaBlocks25 = new AreaBlockCollection();
			this._areaBlocks26 = new AreaBlockCollection();
			this._areaBlocks27 = new AreaBlockCollection();
			this._areaBlocks28 = new AreaBlockCollection();
			this._areaBlocks29 = new AreaBlockCollection();
			this._areaBlocks30 = new AreaBlockCollection();
			this._areaBlocks31 = new AreaBlockCollection();
			this._areaBlocks32 = new AreaBlockCollection();
			this._areaBlocks33 = new AreaBlockCollection();
			this._areaBlocks34 = new AreaBlockCollection();
			this._areaBlocks35 = new AreaBlockCollection();
			this._areaBlocks36 = new AreaBlockCollection();
			this._areaBlocks37 = new AreaBlockCollection();
			this._areaBlocks38 = new AreaBlockCollection();
			this._areaBlocks39 = new AreaBlockCollection();
			this._areaBlocks40 = new AreaBlockCollection();
			this._areaBlocks41 = new AreaBlockCollection();
			this._areaBlocks42 = new AreaBlockCollection();
			this._areaBlocks43 = new AreaBlockCollection();
			this._areaBlocks44 = new AreaBlockCollection();
			this._brokenAreaBlocks = new AreaBlockCollection();
			this._bornAreaBlocks = new AreaBlockCollection();
			this._guideAreaBlocks = new AreaBlockCollection();
			this._secretVillageAreaBlocks = new AreaBlockCollection();
			this._brokenPerformAreaBlocks = new AreaBlockCollection();
			this._travelRouteDict = new Dictionary<TravelRouteKey, TravelRoute>(0);
			this._bornStateTravelRouteDict = new Dictionary<TravelRouteKey, TravelRoute>(0);
			this._animalPlaceData = new AnimalPlaceData[139];
			this._cricketPlaceData = new CricketPlaceData[139];
			this._regularAreaNearList = new Dictionary<short, GameData.Utilities.ShortList>(0);
			this._swordTombLocations = new Location[8];
			this._travelInfo = new CrossAreaMoveInfo();
			this._onHandlingTravelingEventBlock = false;
			this._hunterAnimalsCache = new List<HunterAnimalKey>();
			this._moveBanned = 0;
			this._crossArchiveLockMoveTime = false;
			this._fleeBeasts = new List<Location>();
			this._fleeLoongs = new List<Location>();
			this._loongLocations = new List<LoongLocationData>();
			this._alterSettlementLocations = new List<Location>();
			this._isTaiwuInFulongFlameArea = false;
			this._visibleMapPickups = new List<MapElementPickupDisplayData>();
			this.OnInitializedDomainData();
		}

		// Token: 0x06007A3F RID: 31295 RVA: 0x0047E218 File Offset: 0x0047C418
		public MapAreaData GetElement_Areas(int index)
		{
			return this._areas[index];
		}

		// Token: 0x06007A40 RID: 31296 RVA: 0x0047E234 File Offset: 0x0047C434
		private unsafe void SetElement_Areas(int index, MapAreaData value, DataContext context)
		{
			this._areas[index] = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(index, this._dataStatesAreas, MapDomain.CacheInfluencesAreas, context);
			bool flag = value != null;
			if (flag)
			{
				int dataSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicElementList_Set(2, 0, index, dataSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicElementList_Set(2, 0, index, 0);
			}
		}

		// Token: 0x06007A41 RID: 31297 RVA: 0x0047E290 File Offset: 0x0047C490
		public MapBlockData GetElement_AreaBlocks0(short elementId)
		{
			return this._areaBlocks0[elementId];
		}

		// Token: 0x06007A42 RID: 31298 RVA: 0x0047E2B0 File Offset: 0x0047C4B0
		public bool TryGetElement_AreaBlocks0(short elementId, out MapBlockData value)
		{
			return this._areaBlocks0.TryGetValue(elementId, out value);
		}

		// Token: 0x06007A43 RID: 31299 RVA: 0x0047E2D0 File Offset: 0x0047C4D0
		private unsafe void AddElement_AreaBlocks0(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks0.Add(elementId, value);
			this._modificationsAreaBlocks0.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 1, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 1, elementId, 0);
			}
		}

		// Token: 0x06007A44 RID: 31300 RVA: 0x0047E340 File Offset: 0x0047C540
		private unsafe void SetElement_AreaBlocks0(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks0[elementId] = value;
			this._modificationsAreaBlocks0.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 1, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 1, elementId, 0);
			}
		}

		// Token: 0x06007A45 RID: 31301 RVA: 0x0047E3AD File Offset: 0x0047C5AD
		private void RemoveElement_AreaBlocks0(short elementId, DataContext context)
		{
			this._areaBlocks0.Remove(elementId);
			this._modificationsAreaBlocks0.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 1, elementId);
		}

		// Token: 0x06007A46 RID: 31302 RVA: 0x0047E3E6 File Offset: 0x0047C5E6
		private void ClearAreaBlocks0(DataContext context)
		{
			this._areaBlocks0.Clear();
			this._modificationsAreaBlocks0.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 1);
		}

		// Token: 0x06007A47 RID: 31303 RVA: 0x0047E41C File Offset: 0x0047C61C
		public MapBlockData GetElement_AreaBlocks1(short elementId)
		{
			return this._areaBlocks1[elementId];
		}

		// Token: 0x06007A48 RID: 31304 RVA: 0x0047E43C File Offset: 0x0047C63C
		public bool TryGetElement_AreaBlocks1(short elementId, out MapBlockData value)
		{
			return this._areaBlocks1.TryGetValue(elementId, out value);
		}

		// Token: 0x06007A49 RID: 31305 RVA: 0x0047E45C File Offset: 0x0047C65C
		private unsafe void AddElement_AreaBlocks1(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks1.Add(elementId, value);
			this._modificationsAreaBlocks1.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(2, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 2, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 2, elementId, 0);
			}
		}

		// Token: 0x06007A4A RID: 31306 RVA: 0x0047E4CC File Offset: 0x0047C6CC
		private unsafe void SetElement_AreaBlocks1(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks1[elementId] = value;
			this._modificationsAreaBlocks1.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(2, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 2, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 2, elementId, 0);
			}
		}

		// Token: 0x06007A4B RID: 31307 RVA: 0x0047E539 File Offset: 0x0047C739
		private void RemoveElement_AreaBlocks1(short elementId, DataContext context)
		{
			this._areaBlocks1.Remove(elementId);
			this._modificationsAreaBlocks1.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(2, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 2, elementId);
		}

		// Token: 0x06007A4C RID: 31308 RVA: 0x0047E572 File Offset: 0x0047C772
		private void ClearAreaBlocks1(DataContext context)
		{
			this._areaBlocks1.Clear();
			this._modificationsAreaBlocks1.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(2, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 2);
		}

		// Token: 0x06007A4D RID: 31309 RVA: 0x0047E5A8 File Offset: 0x0047C7A8
		public MapBlockData GetElement_AreaBlocks2(short elementId)
		{
			return this._areaBlocks2[elementId];
		}

		// Token: 0x06007A4E RID: 31310 RVA: 0x0047E5C8 File Offset: 0x0047C7C8
		public bool TryGetElement_AreaBlocks2(short elementId, out MapBlockData value)
		{
			return this._areaBlocks2.TryGetValue(elementId, out value);
		}

		// Token: 0x06007A4F RID: 31311 RVA: 0x0047E5E8 File Offset: 0x0047C7E8
		private unsafe void AddElement_AreaBlocks2(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks2.Add(elementId, value);
			this._modificationsAreaBlocks2.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 3, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 3, elementId, 0);
			}
		}

		// Token: 0x06007A50 RID: 31312 RVA: 0x0047E658 File Offset: 0x0047C858
		private unsafe void SetElement_AreaBlocks2(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks2[elementId] = value;
			this._modificationsAreaBlocks2.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 3, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 3, elementId, 0);
			}
		}

		// Token: 0x06007A51 RID: 31313 RVA: 0x0047E6C5 File Offset: 0x0047C8C5
		private void RemoveElement_AreaBlocks2(short elementId, DataContext context)
		{
			this._areaBlocks2.Remove(elementId);
			this._modificationsAreaBlocks2.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 3, elementId);
		}

		// Token: 0x06007A52 RID: 31314 RVA: 0x0047E6FE File Offset: 0x0047C8FE
		private void ClearAreaBlocks2(DataContext context)
		{
			this._areaBlocks2.Clear();
			this._modificationsAreaBlocks2.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 3);
		}

		// Token: 0x06007A53 RID: 31315 RVA: 0x0047E734 File Offset: 0x0047C934
		public MapBlockData GetElement_AreaBlocks3(short elementId)
		{
			return this._areaBlocks3[elementId];
		}

		// Token: 0x06007A54 RID: 31316 RVA: 0x0047E754 File Offset: 0x0047C954
		public bool TryGetElement_AreaBlocks3(short elementId, out MapBlockData value)
		{
			return this._areaBlocks3.TryGetValue(elementId, out value);
		}

		// Token: 0x06007A55 RID: 31317 RVA: 0x0047E774 File Offset: 0x0047C974
		private unsafe void AddElement_AreaBlocks3(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks3.Add(elementId, value);
			this._modificationsAreaBlocks3.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 4, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 4, elementId, 0);
			}
		}

		// Token: 0x06007A56 RID: 31318 RVA: 0x0047E7E4 File Offset: 0x0047C9E4
		private unsafe void SetElement_AreaBlocks3(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks3[elementId] = value;
			this._modificationsAreaBlocks3.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 4, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 4, elementId, 0);
			}
		}

		// Token: 0x06007A57 RID: 31319 RVA: 0x0047E851 File Offset: 0x0047CA51
		private void RemoveElement_AreaBlocks3(short elementId, DataContext context)
		{
			this._areaBlocks3.Remove(elementId);
			this._modificationsAreaBlocks3.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 4, elementId);
		}

		// Token: 0x06007A58 RID: 31320 RVA: 0x0047E88A File Offset: 0x0047CA8A
		private void ClearAreaBlocks3(DataContext context)
		{
			this._areaBlocks3.Clear();
			this._modificationsAreaBlocks3.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 4);
		}

		// Token: 0x06007A59 RID: 31321 RVA: 0x0047E8C0 File Offset: 0x0047CAC0
		public MapBlockData GetElement_AreaBlocks4(short elementId)
		{
			return this._areaBlocks4[elementId];
		}

		// Token: 0x06007A5A RID: 31322 RVA: 0x0047E8E0 File Offset: 0x0047CAE0
		public bool TryGetElement_AreaBlocks4(short elementId, out MapBlockData value)
		{
			return this._areaBlocks4.TryGetValue(elementId, out value);
		}

		// Token: 0x06007A5B RID: 31323 RVA: 0x0047E900 File Offset: 0x0047CB00
		private unsafe void AddElement_AreaBlocks4(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks4.Add(elementId, value);
			this._modificationsAreaBlocks4.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 5, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 5, elementId, 0);
			}
		}

		// Token: 0x06007A5C RID: 31324 RVA: 0x0047E970 File Offset: 0x0047CB70
		private unsafe void SetElement_AreaBlocks4(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks4[elementId] = value;
			this._modificationsAreaBlocks4.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 5, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 5, elementId, 0);
			}
		}

		// Token: 0x06007A5D RID: 31325 RVA: 0x0047E9DD File Offset: 0x0047CBDD
		private void RemoveElement_AreaBlocks4(short elementId, DataContext context)
		{
			this._areaBlocks4.Remove(elementId);
			this._modificationsAreaBlocks4.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 5, elementId);
		}

		// Token: 0x06007A5E RID: 31326 RVA: 0x0047EA16 File Offset: 0x0047CC16
		private void ClearAreaBlocks4(DataContext context)
		{
			this._areaBlocks4.Clear();
			this._modificationsAreaBlocks4.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 5);
		}

		// Token: 0x06007A5F RID: 31327 RVA: 0x0047EA4C File Offset: 0x0047CC4C
		public MapBlockData GetElement_AreaBlocks5(short elementId)
		{
			return this._areaBlocks5[elementId];
		}

		// Token: 0x06007A60 RID: 31328 RVA: 0x0047EA6C File Offset: 0x0047CC6C
		public bool TryGetElement_AreaBlocks5(short elementId, out MapBlockData value)
		{
			return this._areaBlocks5.TryGetValue(elementId, out value);
		}

		// Token: 0x06007A61 RID: 31329 RVA: 0x0047EA8C File Offset: 0x0047CC8C
		private unsafe void AddElement_AreaBlocks5(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks5.Add(elementId, value);
			this._modificationsAreaBlocks5.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 6, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 6, elementId, 0);
			}
		}

		// Token: 0x06007A62 RID: 31330 RVA: 0x0047EAFC File Offset: 0x0047CCFC
		private unsafe void SetElement_AreaBlocks5(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks5[elementId] = value;
			this._modificationsAreaBlocks5.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 6, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 6, elementId, 0);
			}
		}

		// Token: 0x06007A63 RID: 31331 RVA: 0x0047EB69 File Offset: 0x0047CD69
		private void RemoveElement_AreaBlocks5(short elementId, DataContext context)
		{
			this._areaBlocks5.Remove(elementId);
			this._modificationsAreaBlocks5.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 6, elementId);
		}

		// Token: 0x06007A64 RID: 31332 RVA: 0x0047EBA2 File Offset: 0x0047CDA2
		private void ClearAreaBlocks5(DataContext context)
		{
			this._areaBlocks5.Clear();
			this._modificationsAreaBlocks5.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 6);
		}

		// Token: 0x06007A65 RID: 31333 RVA: 0x0047EBD8 File Offset: 0x0047CDD8
		public MapBlockData GetElement_AreaBlocks6(short elementId)
		{
			return this._areaBlocks6[elementId];
		}

		// Token: 0x06007A66 RID: 31334 RVA: 0x0047EBF8 File Offset: 0x0047CDF8
		public bool TryGetElement_AreaBlocks6(short elementId, out MapBlockData value)
		{
			return this._areaBlocks6.TryGetValue(elementId, out value);
		}

		// Token: 0x06007A67 RID: 31335 RVA: 0x0047EC18 File Offset: 0x0047CE18
		private unsafe void AddElement_AreaBlocks6(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks6.Add(elementId, value);
			this._modificationsAreaBlocks6.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 7, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 7, elementId, 0);
			}
		}

		// Token: 0x06007A68 RID: 31336 RVA: 0x0047EC88 File Offset: 0x0047CE88
		private unsafe void SetElement_AreaBlocks6(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks6[elementId] = value;
			this._modificationsAreaBlocks6.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 7, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 7, elementId, 0);
			}
		}

		// Token: 0x06007A69 RID: 31337 RVA: 0x0047ECF5 File Offset: 0x0047CEF5
		private void RemoveElement_AreaBlocks6(short elementId, DataContext context)
		{
			this._areaBlocks6.Remove(elementId);
			this._modificationsAreaBlocks6.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 7, elementId);
		}

		// Token: 0x06007A6A RID: 31338 RVA: 0x0047ED2E File Offset: 0x0047CF2E
		private void ClearAreaBlocks6(DataContext context)
		{
			this._areaBlocks6.Clear();
			this._modificationsAreaBlocks6.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 7);
		}

		// Token: 0x06007A6B RID: 31339 RVA: 0x0047ED64 File Offset: 0x0047CF64
		public MapBlockData GetElement_AreaBlocks7(short elementId)
		{
			return this._areaBlocks7[elementId];
		}

		// Token: 0x06007A6C RID: 31340 RVA: 0x0047ED84 File Offset: 0x0047CF84
		public bool TryGetElement_AreaBlocks7(short elementId, out MapBlockData value)
		{
			return this._areaBlocks7.TryGetValue(elementId, out value);
		}

		// Token: 0x06007A6D RID: 31341 RVA: 0x0047EDA4 File Offset: 0x0047CFA4
		private unsafe void AddElement_AreaBlocks7(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks7.Add(elementId, value);
			this._modificationsAreaBlocks7.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 8, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 8, elementId, 0);
			}
		}

		// Token: 0x06007A6E RID: 31342 RVA: 0x0047EE14 File Offset: 0x0047D014
		private unsafe void SetElement_AreaBlocks7(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks7[elementId] = value;
			this._modificationsAreaBlocks7.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 8, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 8, elementId, 0);
			}
		}

		// Token: 0x06007A6F RID: 31343 RVA: 0x0047EE81 File Offset: 0x0047D081
		private void RemoveElement_AreaBlocks7(short elementId, DataContext context)
		{
			this._areaBlocks7.Remove(elementId);
			this._modificationsAreaBlocks7.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 8, elementId);
		}

		// Token: 0x06007A70 RID: 31344 RVA: 0x0047EEBA File Offset: 0x0047D0BA
		private void ClearAreaBlocks7(DataContext context)
		{
			this._areaBlocks7.Clear();
			this._modificationsAreaBlocks7.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 8);
		}

		// Token: 0x06007A71 RID: 31345 RVA: 0x0047EEF0 File Offset: 0x0047D0F0
		public MapBlockData GetElement_AreaBlocks8(short elementId)
		{
			return this._areaBlocks8[elementId];
		}

		// Token: 0x06007A72 RID: 31346 RVA: 0x0047EF10 File Offset: 0x0047D110
		public bool TryGetElement_AreaBlocks8(short elementId, out MapBlockData value)
		{
			return this._areaBlocks8.TryGetValue(elementId, out value);
		}

		// Token: 0x06007A73 RID: 31347 RVA: 0x0047EF30 File Offset: 0x0047D130
		private unsafe void AddElement_AreaBlocks8(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks8.Add(elementId, value);
			this._modificationsAreaBlocks8.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(9, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 9, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 9, elementId, 0);
			}
		}

		// Token: 0x06007A74 RID: 31348 RVA: 0x0047EFA0 File Offset: 0x0047D1A0
		private unsafe void SetElement_AreaBlocks8(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks8[elementId] = value;
			this._modificationsAreaBlocks8.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(9, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 9, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 9, elementId, 0);
			}
		}

		// Token: 0x06007A75 RID: 31349 RVA: 0x0047F010 File Offset: 0x0047D210
		private void RemoveElement_AreaBlocks8(short elementId, DataContext context)
		{
			this._areaBlocks8.Remove(elementId);
			this._modificationsAreaBlocks8.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(9, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 9, elementId);
		}

		// Token: 0x06007A76 RID: 31350 RVA: 0x0047F04B File Offset: 0x0047D24B
		private void ClearAreaBlocks8(DataContext context)
		{
			this._areaBlocks8.Clear();
			this._modificationsAreaBlocks8.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(9, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 9);
		}

		// Token: 0x06007A77 RID: 31351 RVA: 0x0047F084 File Offset: 0x0047D284
		public MapBlockData GetElement_AreaBlocks9(short elementId)
		{
			return this._areaBlocks9[elementId];
		}

		// Token: 0x06007A78 RID: 31352 RVA: 0x0047F0A4 File Offset: 0x0047D2A4
		public bool TryGetElement_AreaBlocks9(short elementId, out MapBlockData value)
		{
			return this._areaBlocks9.TryGetValue(elementId, out value);
		}

		// Token: 0x06007A79 RID: 31353 RVA: 0x0047F0C4 File Offset: 0x0047D2C4
		private unsafe void AddElement_AreaBlocks9(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks9.Add(elementId, value);
			this._modificationsAreaBlocks9.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(10, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 10, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 10, elementId, 0);
			}
		}

		// Token: 0x06007A7A RID: 31354 RVA: 0x0047F134 File Offset: 0x0047D334
		private unsafe void SetElement_AreaBlocks9(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks9[elementId] = value;
			this._modificationsAreaBlocks9.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(10, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 10, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 10, elementId, 0);
			}
		}

		// Token: 0x06007A7B RID: 31355 RVA: 0x0047F1A4 File Offset: 0x0047D3A4
		private void RemoveElement_AreaBlocks9(short elementId, DataContext context)
		{
			this._areaBlocks9.Remove(elementId);
			this._modificationsAreaBlocks9.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(10, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 10, elementId);
		}

		// Token: 0x06007A7C RID: 31356 RVA: 0x0047F1DF File Offset: 0x0047D3DF
		private void ClearAreaBlocks9(DataContext context)
		{
			this._areaBlocks9.Clear();
			this._modificationsAreaBlocks9.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(10, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 10);
		}

		// Token: 0x06007A7D RID: 31357 RVA: 0x0047F218 File Offset: 0x0047D418
		public MapBlockData GetElement_AreaBlocks10(short elementId)
		{
			return this._areaBlocks10[elementId];
		}

		// Token: 0x06007A7E RID: 31358 RVA: 0x0047F238 File Offset: 0x0047D438
		public bool TryGetElement_AreaBlocks10(short elementId, out MapBlockData value)
		{
			return this._areaBlocks10.TryGetValue(elementId, out value);
		}

		// Token: 0x06007A7F RID: 31359 RVA: 0x0047F258 File Offset: 0x0047D458
		private unsafe void AddElement_AreaBlocks10(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks10.Add(elementId, value);
			this._modificationsAreaBlocks10.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(11, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 11, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 11, elementId, 0);
			}
		}

		// Token: 0x06007A80 RID: 31360 RVA: 0x0047F2C8 File Offset: 0x0047D4C8
		private unsafe void SetElement_AreaBlocks10(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks10[elementId] = value;
			this._modificationsAreaBlocks10.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(11, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 11, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 11, elementId, 0);
			}
		}

		// Token: 0x06007A81 RID: 31361 RVA: 0x0047F338 File Offset: 0x0047D538
		private void RemoveElement_AreaBlocks10(short elementId, DataContext context)
		{
			this._areaBlocks10.Remove(elementId);
			this._modificationsAreaBlocks10.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(11, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 11, elementId);
		}

		// Token: 0x06007A82 RID: 31362 RVA: 0x0047F373 File Offset: 0x0047D573
		private void ClearAreaBlocks10(DataContext context)
		{
			this._areaBlocks10.Clear();
			this._modificationsAreaBlocks10.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(11, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 11);
		}

		// Token: 0x06007A83 RID: 31363 RVA: 0x0047F3AC File Offset: 0x0047D5AC
		public MapBlockData GetElement_AreaBlocks11(short elementId)
		{
			return this._areaBlocks11[elementId];
		}

		// Token: 0x06007A84 RID: 31364 RVA: 0x0047F3CC File Offset: 0x0047D5CC
		public bool TryGetElement_AreaBlocks11(short elementId, out MapBlockData value)
		{
			return this._areaBlocks11.TryGetValue(elementId, out value);
		}

		// Token: 0x06007A85 RID: 31365 RVA: 0x0047F3EC File Offset: 0x0047D5EC
		private unsafe void AddElement_AreaBlocks11(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks11.Add(elementId, value);
			this._modificationsAreaBlocks11.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(12, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 12, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 12, elementId, 0);
			}
		}

		// Token: 0x06007A86 RID: 31366 RVA: 0x0047F45C File Offset: 0x0047D65C
		private unsafe void SetElement_AreaBlocks11(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks11[elementId] = value;
			this._modificationsAreaBlocks11.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(12, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 12, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 12, elementId, 0);
			}
		}

		// Token: 0x06007A87 RID: 31367 RVA: 0x0047F4CC File Offset: 0x0047D6CC
		private void RemoveElement_AreaBlocks11(short elementId, DataContext context)
		{
			this._areaBlocks11.Remove(elementId);
			this._modificationsAreaBlocks11.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(12, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 12, elementId);
		}

		// Token: 0x06007A88 RID: 31368 RVA: 0x0047F507 File Offset: 0x0047D707
		private void ClearAreaBlocks11(DataContext context)
		{
			this._areaBlocks11.Clear();
			this._modificationsAreaBlocks11.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(12, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 12);
		}

		// Token: 0x06007A89 RID: 31369 RVA: 0x0047F540 File Offset: 0x0047D740
		public MapBlockData GetElement_AreaBlocks12(short elementId)
		{
			return this._areaBlocks12[elementId];
		}

		// Token: 0x06007A8A RID: 31370 RVA: 0x0047F560 File Offset: 0x0047D760
		public bool TryGetElement_AreaBlocks12(short elementId, out MapBlockData value)
		{
			return this._areaBlocks12.TryGetValue(elementId, out value);
		}

		// Token: 0x06007A8B RID: 31371 RVA: 0x0047F580 File Offset: 0x0047D780
		private unsafe void AddElement_AreaBlocks12(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks12.Add(elementId, value);
			this._modificationsAreaBlocks12.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(13, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 13, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 13, elementId, 0);
			}
		}

		// Token: 0x06007A8C RID: 31372 RVA: 0x0047F5F0 File Offset: 0x0047D7F0
		private unsafe void SetElement_AreaBlocks12(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks12[elementId] = value;
			this._modificationsAreaBlocks12.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(13, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 13, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 13, elementId, 0);
			}
		}

		// Token: 0x06007A8D RID: 31373 RVA: 0x0047F660 File Offset: 0x0047D860
		private void RemoveElement_AreaBlocks12(short elementId, DataContext context)
		{
			this._areaBlocks12.Remove(elementId);
			this._modificationsAreaBlocks12.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(13, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 13, elementId);
		}

		// Token: 0x06007A8E RID: 31374 RVA: 0x0047F69B File Offset: 0x0047D89B
		private void ClearAreaBlocks12(DataContext context)
		{
			this._areaBlocks12.Clear();
			this._modificationsAreaBlocks12.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(13, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 13);
		}

		// Token: 0x06007A8F RID: 31375 RVA: 0x0047F6D4 File Offset: 0x0047D8D4
		public MapBlockData GetElement_AreaBlocks13(short elementId)
		{
			return this._areaBlocks13[elementId];
		}

		// Token: 0x06007A90 RID: 31376 RVA: 0x0047F6F4 File Offset: 0x0047D8F4
		public bool TryGetElement_AreaBlocks13(short elementId, out MapBlockData value)
		{
			return this._areaBlocks13.TryGetValue(elementId, out value);
		}

		// Token: 0x06007A91 RID: 31377 RVA: 0x0047F714 File Offset: 0x0047D914
		private unsafe void AddElement_AreaBlocks13(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks13.Add(elementId, value);
			this._modificationsAreaBlocks13.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 14, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 14, elementId, 0);
			}
		}

		// Token: 0x06007A92 RID: 31378 RVA: 0x0047F784 File Offset: 0x0047D984
		private unsafe void SetElement_AreaBlocks13(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks13[elementId] = value;
			this._modificationsAreaBlocks13.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 14, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 14, elementId, 0);
			}
		}

		// Token: 0x06007A93 RID: 31379 RVA: 0x0047F7F4 File Offset: 0x0047D9F4
		private void RemoveElement_AreaBlocks13(short elementId, DataContext context)
		{
			this._areaBlocks13.Remove(elementId);
			this._modificationsAreaBlocks13.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 14, elementId);
		}

		// Token: 0x06007A94 RID: 31380 RVA: 0x0047F82F File Offset: 0x0047DA2F
		private void ClearAreaBlocks13(DataContext context)
		{
			this._areaBlocks13.Clear();
			this._modificationsAreaBlocks13.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 14);
		}

		// Token: 0x06007A95 RID: 31381 RVA: 0x0047F868 File Offset: 0x0047DA68
		public MapBlockData GetElement_AreaBlocks14(short elementId)
		{
			return this._areaBlocks14[elementId];
		}

		// Token: 0x06007A96 RID: 31382 RVA: 0x0047F888 File Offset: 0x0047DA88
		public bool TryGetElement_AreaBlocks14(short elementId, out MapBlockData value)
		{
			return this._areaBlocks14.TryGetValue(elementId, out value);
		}

		// Token: 0x06007A97 RID: 31383 RVA: 0x0047F8A8 File Offset: 0x0047DAA8
		private unsafe void AddElement_AreaBlocks14(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks14.Add(elementId, value);
			this._modificationsAreaBlocks14.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(15, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 15, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 15, elementId, 0);
			}
		}

		// Token: 0x06007A98 RID: 31384 RVA: 0x0047F918 File Offset: 0x0047DB18
		private unsafe void SetElement_AreaBlocks14(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks14[elementId] = value;
			this._modificationsAreaBlocks14.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(15, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 15, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 15, elementId, 0);
			}
		}

		// Token: 0x06007A99 RID: 31385 RVA: 0x0047F988 File Offset: 0x0047DB88
		private void RemoveElement_AreaBlocks14(short elementId, DataContext context)
		{
			this._areaBlocks14.Remove(elementId);
			this._modificationsAreaBlocks14.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(15, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 15, elementId);
		}

		// Token: 0x06007A9A RID: 31386 RVA: 0x0047F9C3 File Offset: 0x0047DBC3
		private void ClearAreaBlocks14(DataContext context)
		{
			this._areaBlocks14.Clear();
			this._modificationsAreaBlocks14.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(15, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 15);
		}

		// Token: 0x06007A9B RID: 31387 RVA: 0x0047F9FC File Offset: 0x0047DBFC
		public MapBlockData GetElement_AreaBlocks15(short elementId)
		{
			return this._areaBlocks15[elementId];
		}

		// Token: 0x06007A9C RID: 31388 RVA: 0x0047FA1C File Offset: 0x0047DC1C
		public bool TryGetElement_AreaBlocks15(short elementId, out MapBlockData value)
		{
			return this._areaBlocks15.TryGetValue(elementId, out value);
		}

		// Token: 0x06007A9D RID: 31389 RVA: 0x0047FA3C File Offset: 0x0047DC3C
		private unsafe void AddElement_AreaBlocks15(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks15.Add(elementId, value);
			this._modificationsAreaBlocks15.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(16, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 16, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 16, elementId, 0);
			}
		}

		// Token: 0x06007A9E RID: 31390 RVA: 0x0047FAAC File Offset: 0x0047DCAC
		private unsafe void SetElement_AreaBlocks15(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks15[elementId] = value;
			this._modificationsAreaBlocks15.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(16, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 16, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 16, elementId, 0);
			}
		}

		// Token: 0x06007A9F RID: 31391 RVA: 0x0047FB1C File Offset: 0x0047DD1C
		private void RemoveElement_AreaBlocks15(short elementId, DataContext context)
		{
			this._areaBlocks15.Remove(elementId);
			this._modificationsAreaBlocks15.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(16, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 16, elementId);
		}

		// Token: 0x06007AA0 RID: 31392 RVA: 0x0047FB57 File Offset: 0x0047DD57
		private void ClearAreaBlocks15(DataContext context)
		{
			this._areaBlocks15.Clear();
			this._modificationsAreaBlocks15.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(16, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 16);
		}

		// Token: 0x06007AA1 RID: 31393 RVA: 0x0047FB90 File Offset: 0x0047DD90
		public MapBlockData GetElement_AreaBlocks16(short elementId)
		{
			return this._areaBlocks16[elementId];
		}

		// Token: 0x06007AA2 RID: 31394 RVA: 0x0047FBB0 File Offset: 0x0047DDB0
		public bool TryGetElement_AreaBlocks16(short elementId, out MapBlockData value)
		{
			return this._areaBlocks16.TryGetValue(elementId, out value);
		}

		// Token: 0x06007AA3 RID: 31395 RVA: 0x0047FBD0 File Offset: 0x0047DDD0
		private unsafe void AddElement_AreaBlocks16(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks16.Add(elementId, value);
			this._modificationsAreaBlocks16.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(17, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 17, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 17, elementId, 0);
			}
		}

		// Token: 0x06007AA4 RID: 31396 RVA: 0x0047FC40 File Offset: 0x0047DE40
		private unsafe void SetElement_AreaBlocks16(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks16[elementId] = value;
			this._modificationsAreaBlocks16.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(17, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 17, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 17, elementId, 0);
			}
		}

		// Token: 0x06007AA5 RID: 31397 RVA: 0x0047FCB0 File Offset: 0x0047DEB0
		private void RemoveElement_AreaBlocks16(short elementId, DataContext context)
		{
			this._areaBlocks16.Remove(elementId);
			this._modificationsAreaBlocks16.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(17, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 17, elementId);
		}

		// Token: 0x06007AA6 RID: 31398 RVA: 0x0047FCEB File Offset: 0x0047DEEB
		private void ClearAreaBlocks16(DataContext context)
		{
			this._areaBlocks16.Clear();
			this._modificationsAreaBlocks16.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(17, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 17);
		}

		// Token: 0x06007AA7 RID: 31399 RVA: 0x0047FD24 File Offset: 0x0047DF24
		public MapBlockData GetElement_AreaBlocks17(short elementId)
		{
			return this._areaBlocks17[elementId];
		}

		// Token: 0x06007AA8 RID: 31400 RVA: 0x0047FD44 File Offset: 0x0047DF44
		public bool TryGetElement_AreaBlocks17(short elementId, out MapBlockData value)
		{
			return this._areaBlocks17.TryGetValue(elementId, out value);
		}

		// Token: 0x06007AA9 RID: 31401 RVA: 0x0047FD64 File Offset: 0x0047DF64
		private unsafe void AddElement_AreaBlocks17(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks17.Add(elementId, value);
			this._modificationsAreaBlocks17.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(18, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 18, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 18, elementId, 0);
			}
		}

		// Token: 0x06007AAA RID: 31402 RVA: 0x0047FDD4 File Offset: 0x0047DFD4
		private unsafe void SetElement_AreaBlocks17(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks17[elementId] = value;
			this._modificationsAreaBlocks17.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(18, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 18, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 18, elementId, 0);
			}
		}

		// Token: 0x06007AAB RID: 31403 RVA: 0x0047FE44 File Offset: 0x0047E044
		private void RemoveElement_AreaBlocks17(short elementId, DataContext context)
		{
			this._areaBlocks17.Remove(elementId);
			this._modificationsAreaBlocks17.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(18, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 18, elementId);
		}

		// Token: 0x06007AAC RID: 31404 RVA: 0x0047FE7F File Offset: 0x0047E07F
		private void ClearAreaBlocks17(DataContext context)
		{
			this._areaBlocks17.Clear();
			this._modificationsAreaBlocks17.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(18, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 18);
		}

		// Token: 0x06007AAD RID: 31405 RVA: 0x0047FEB8 File Offset: 0x0047E0B8
		public MapBlockData GetElement_AreaBlocks18(short elementId)
		{
			return this._areaBlocks18[elementId];
		}

		// Token: 0x06007AAE RID: 31406 RVA: 0x0047FED8 File Offset: 0x0047E0D8
		public bool TryGetElement_AreaBlocks18(short elementId, out MapBlockData value)
		{
			return this._areaBlocks18.TryGetValue(elementId, out value);
		}

		// Token: 0x06007AAF RID: 31407 RVA: 0x0047FEF8 File Offset: 0x0047E0F8
		private unsafe void AddElement_AreaBlocks18(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks18.Add(elementId, value);
			this._modificationsAreaBlocks18.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(19, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 19, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 19, elementId, 0);
			}
		}

		// Token: 0x06007AB0 RID: 31408 RVA: 0x0047FF68 File Offset: 0x0047E168
		private unsafe void SetElement_AreaBlocks18(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks18[elementId] = value;
			this._modificationsAreaBlocks18.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(19, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 19, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 19, elementId, 0);
			}
		}

		// Token: 0x06007AB1 RID: 31409 RVA: 0x0047FFD8 File Offset: 0x0047E1D8
		private void RemoveElement_AreaBlocks18(short elementId, DataContext context)
		{
			this._areaBlocks18.Remove(elementId);
			this._modificationsAreaBlocks18.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(19, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 19, elementId);
		}

		// Token: 0x06007AB2 RID: 31410 RVA: 0x00480013 File Offset: 0x0047E213
		private void ClearAreaBlocks18(DataContext context)
		{
			this._areaBlocks18.Clear();
			this._modificationsAreaBlocks18.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(19, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 19);
		}

		// Token: 0x06007AB3 RID: 31411 RVA: 0x0048004C File Offset: 0x0047E24C
		public MapBlockData GetElement_AreaBlocks19(short elementId)
		{
			return this._areaBlocks19[elementId];
		}

		// Token: 0x06007AB4 RID: 31412 RVA: 0x0048006C File Offset: 0x0047E26C
		public bool TryGetElement_AreaBlocks19(short elementId, out MapBlockData value)
		{
			return this._areaBlocks19.TryGetValue(elementId, out value);
		}

		// Token: 0x06007AB5 RID: 31413 RVA: 0x0048008C File Offset: 0x0047E28C
		private unsafe void AddElement_AreaBlocks19(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks19.Add(elementId, value);
			this._modificationsAreaBlocks19.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 20, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 20, elementId, 0);
			}
		}

		// Token: 0x06007AB6 RID: 31414 RVA: 0x004800FC File Offset: 0x0047E2FC
		private unsafe void SetElement_AreaBlocks19(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks19[elementId] = value;
			this._modificationsAreaBlocks19.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 20, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 20, elementId, 0);
			}
		}

		// Token: 0x06007AB7 RID: 31415 RVA: 0x0048016C File Offset: 0x0047E36C
		private void RemoveElement_AreaBlocks19(short elementId, DataContext context)
		{
			this._areaBlocks19.Remove(elementId);
			this._modificationsAreaBlocks19.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 20, elementId);
		}

		// Token: 0x06007AB8 RID: 31416 RVA: 0x004801A7 File Offset: 0x0047E3A7
		private void ClearAreaBlocks19(DataContext context)
		{
			this._areaBlocks19.Clear();
			this._modificationsAreaBlocks19.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 20);
		}

		// Token: 0x06007AB9 RID: 31417 RVA: 0x004801E0 File Offset: 0x0047E3E0
		public MapBlockData GetElement_AreaBlocks20(short elementId)
		{
			return this._areaBlocks20[elementId];
		}

		// Token: 0x06007ABA RID: 31418 RVA: 0x00480200 File Offset: 0x0047E400
		public bool TryGetElement_AreaBlocks20(short elementId, out MapBlockData value)
		{
			return this._areaBlocks20.TryGetValue(elementId, out value);
		}

		// Token: 0x06007ABB RID: 31419 RVA: 0x00480220 File Offset: 0x0047E420
		private unsafe void AddElement_AreaBlocks20(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks20.Add(elementId, value);
			this._modificationsAreaBlocks20.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(21, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 21, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 21, elementId, 0);
			}
		}

		// Token: 0x06007ABC RID: 31420 RVA: 0x00480290 File Offset: 0x0047E490
		private unsafe void SetElement_AreaBlocks20(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks20[elementId] = value;
			this._modificationsAreaBlocks20.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(21, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 21, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 21, elementId, 0);
			}
		}

		// Token: 0x06007ABD RID: 31421 RVA: 0x00480300 File Offset: 0x0047E500
		private void RemoveElement_AreaBlocks20(short elementId, DataContext context)
		{
			this._areaBlocks20.Remove(elementId);
			this._modificationsAreaBlocks20.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(21, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 21, elementId);
		}

		// Token: 0x06007ABE RID: 31422 RVA: 0x0048033B File Offset: 0x0047E53B
		private void ClearAreaBlocks20(DataContext context)
		{
			this._areaBlocks20.Clear();
			this._modificationsAreaBlocks20.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(21, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 21);
		}

		// Token: 0x06007ABF RID: 31423 RVA: 0x00480374 File Offset: 0x0047E574
		public MapBlockData GetElement_AreaBlocks21(short elementId)
		{
			return this._areaBlocks21[elementId];
		}

		// Token: 0x06007AC0 RID: 31424 RVA: 0x00480394 File Offset: 0x0047E594
		public bool TryGetElement_AreaBlocks21(short elementId, out MapBlockData value)
		{
			return this._areaBlocks21.TryGetValue(elementId, out value);
		}

		// Token: 0x06007AC1 RID: 31425 RVA: 0x004803B4 File Offset: 0x0047E5B4
		private unsafe void AddElement_AreaBlocks21(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks21.Add(elementId, value);
			this._modificationsAreaBlocks21.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(22, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 22, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 22, elementId, 0);
			}
		}

		// Token: 0x06007AC2 RID: 31426 RVA: 0x00480424 File Offset: 0x0047E624
		private unsafe void SetElement_AreaBlocks21(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks21[elementId] = value;
			this._modificationsAreaBlocks21.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(22, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 22, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 22, elementId, 0);
			}
		}

		// Token: 0x06007AC3 RID: 31427 RVA: 0x00480494 File Offset: 0x0047E694
		private void RemoveElement_AreaBlocks21(short elementId, DataContext context)
		{
			this._areaBlocks21.Remove(elementId);
			this._modificationsAreaBlocks21.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(22, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 22, elementId);
		}

		// Token: 0x06007AC4 RID: 31428 RVA: 0x004804CF File Offset: 0x0047E6CF
		private void ClearAreaBlocks21(DataContext context)
		{
			this._areaBlocks21.Clear();
			this._modificationsAreaBlocks21.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(22, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 22);
		}

		// Token: 0x06007AC5 RID: 31429 RVA: 0x00480508 File Offset: 0x0047E708
		public MapBlockData GetElement_AreaBlocks22(short elementId)
		{
			return this._areaBlocks22[elementId];
		}

		// Token: 0x06007AC6 RID: 31430 RVA: 0x00480528 File Offset: 0x0047E728
		public bool TryGetElement_AreaBlocks22(short elementId, out MapBlockData value)
		{
			return this._areaBlocks22.TryGetValue(elementId, out value);
		}

		// Token: 0x06007AC7 RID: 31431 RVA: 0x00480548 File Offset: 0x0047E748
		private unsafe void AddElement_AreaBlocks22(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks22.Add(elementId, value);
			this._modificationsAreaBlocks22.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(23, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 23, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 23, elementId, 0);
			}
		}

		// Token: 0x06007AC8 RID: 31432 RVA: 0x004805B8 File Offset: 0x0047E7B8
		private unsafe void SetElement_AreaBlocks22(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks22[elementId] = value;
			this._modificationsAreaBlocks22.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(23, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 23, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 23, elementId, 0);
			}
		}

		// Token: 0x06007AC9 RID: 31433 RVA: 0x00480628 File Offset: 0x0047E828
		private void RemoveElement_AreaBlocks22(short elementId, DataContext context)
		{
			this._areaBlocks22.Remove(elementId);
			this._modificationsAreaBlocks22.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(23, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 23, elementId);
		}

		// Token: 0x06007ACA RID: 31434 RVA: 0x00480663 File Offset: 0x0047E863
		private void ClearAreaBlocks22(DataContext context)
		{
			this._areaBlocks22.Clear();
			this._modificationsAreaBlocks22.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(23, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 23);
		}

		// Token: 0x06007ACB RID: 31435 RVA: 0x0048069C File Offset: 0x0047E89C
		public MapBlockData GetElement_AreaBlocks23(short elementId)
		{
			return this._areaBlocks23[elementId];
		}

		// Token: 0x06007ACC RID: 31436 RVA: 0x004806BC File Offset: 0x0047E8BC
		public bool TryGetElement_AreaBlocks23(short elementId, out MapBlockData value)
		{
			return this._areaBlocks23.TryGetValue(elementId, out value);
		}

		// Token: 0x06007ACD RID: 31437 RVA: 0x004806DC File Offset: 0x0047E8DC
		private unsafe void AddElement_AreaBlocks23(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks23.Add(elementId, value);
			this._modificationsAreaBlocks23.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(24, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 24, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 24, elementId, 0);
			}
		}

		// Token: 0x06007ACE RID: 31438 RVA: 0x0048074C File Offset: 0x0047E94C
		private unsafe void SetElement_AreaBlocks23(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks23[elementId] = value;
			this._modificationsAreaBlocks23.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(24, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 24, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 24, elementId, 0);
			}
		}

		// Token: 0x06007ACF RID: 31439 RVA: 0x004807BC File Offset: 0x0047E9BC
		private void RemoveElement_AreaBlocks23(short elementId, DataContext context)
		{
			this._areaBlocks23.Remove(elementId);
			this._modificationsAreaBlocks23.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(24, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 24, elementId);
		}

		// Token: 0x06007AD0 RID: 31440 RVA: 0x004807F7 File Offset: 0x0047E9F7
		private void ClearAreaBlocks23(DataContext context)
		{
			this._areaBlocks23.Clear();
			this._modificationsAreaBlocks23.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(24, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 24);
		}

		// Token: 0x06007AD1 RID: 31441 RVA: 0x00480830 File Offset: 0x0047EA30
		public MapBlockData GetElement_AreaBlocks24(short elementId)
		{
			return this._areaBlocks24[elementId];
		}

		// Token: 0x06007AD2 RID: 31442 RVA: 0x00480850 File Offset: 0x0047EA50
		public bool TryGetElement_AreaBlocks24(short elementId, out MapBlockData value)
		{
			return this._areaBlocks24.TryGetValue(elementId, out value);
		}

		// Token: 0x06007AD3 RID: 31443 RVA: 0x00480870 File Offset: 0x0047EA70
		private unsafe void AddElement_AreaBlocks24(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks24.Add(elementId, value);
			this._modificationsAreaBlocks24.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(25, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 25, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 25, elementId, 0);
			}
		}

		// Token: 0x06007AD4 RID: 31444 RVA: 0x004808E0 File Offset: 0x0047EAE0
		private unsafe void SetElement_AreaBlocks24(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks24[elementId] = value;
			this._modificationsAreaBlocks24.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(25, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 25, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 25, elementId, 0);
			}
		}

		// Token: 0x06007AD5 RID: 31445 RVA: 0x00480950 File Offset: 0x0047EB50
		private void RemoveElement_AreaBlocks24(short elementId, DataContext context)
		{
			this._areaBlocks24.Remove(elementId);
			this._modificationsAreaBlocks24.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(25, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 25, elementId);
		}

		// Token: 0x06007AD6 RID: 31446 RVA: 0x0048098B File Offset: 0x0047EB8B
		private void ClearAreaBlocks24(DataContext context)
		{
			this._areaBlocks24.Clear();
			this._modificationsAreaBlocks24.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(25, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 25);
		}

		// Token: 0x06007AD7 RID: 31447 RVA: 0x004809C4 File Offset: 0x0047EBC4
		public MapBlockData GetElement_AreaBlocks25(short elementId)
		{
			return this._areaBlocks25[elementId];
		}

		// Token: 0x06007AD8 RID: 31448 RVA: 0x004809E4 File Offset: 0x0047EBE4
		public bool TryGetElement_AreaBlocks25(short elementId, out MapBlockData value)
		{
			return this._areaBlocks25.TryGetValue(elementId, out value);
		}

		// Token: 0x06007AD9 RID: 31449 RVA: 0x00480A04 File Offset: 0x0047EC04
		private unsafe void AddElement_AreaBlocks25(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks25.Add(elementId, value);
			this._modificationsAreaBlocks25.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(26, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 26, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 26, elementId, 0);
			}
		}

		// Token: 0x06007ADA RID: 31450 RVA: 0x00480A74 File Offset: 0x0047EC74
		private unsafe void SetElement_AreaBlocks25(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks25[elementId] = value;
			this._modificationsAreaBlocks25.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(26, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 26, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 26, elementId, 0);
			}
		}

		// Token: 0x06007ADB RID: 31451 RVA: 0x00480AE4 File Offset: 0x0047ECE4
		private void RemoveElement_AreaBlocks25(short elementId, DataContext context)
		{
			this._areaBlocks25.Remove(elementId);
			this._modificationsAreaBlocks25.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(26, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 26, elementId);
		}

		// Token: 0x06007ADC RID: 31452 RVA: 0x00480B1F File Offset: 0x0047ED1F
		private void ClearAreaBlocks25(DataContext context)
		{
			this._areaBlocks25.Clear();
			this._modificationsAreaBlocks25.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(26, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 26);
		}

		// Token: 0x06007ADD RID: 31453 RVA: 0x00480B58 File Offset: 0x0047ED58
		public MapBlockData GetElement_AreaBlocks26(short elementId)
		{
			return this._areaBlocks26[elementId];
		}

		// Token: 0x06007ADE RID: 31454 RVA: 0x00480B78 File Offset: 0x0047ED78
		public bool TryGetElement_AreaBlocks26(short elementId, out MapBlockData value)
		{
			return this._areaBlocks26.TryGetValue(elementId, out value);
		}

		// Token: 0x06007ADF RID: 31455 RVA: 0x00480B98 File Offset: 0x0047ED98
		private unsafe void AddElement_AreaBlocks26(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks26.Add(elementId, value);
			this._modificationsAreaBlocks26.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(27, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 27, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 27, elementId, 0);
			}
		}

		// Token: 0x06007AE0 RID: 31456 RVA: 0x00480C08 File Offset: 0x0047EE08
		private unsafe void SetElement_AreaBlocks26(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks26[elementId] = value;
			this._modificationsAreaBlocks26.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(27, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 27, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 27, elementId, 0);
			}
		}

		// Token: 0x06007AE1 RID: 31457 RVA: 0x00480C78 File Offset: 0x0047EE78
		private void RemoveElement_AreaBlocks26(short elementId, DataContext context)
		{
			this._areaBlocks26.Remove(elementId);
			this._modificationsAreaBlocks26.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(27, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 27, elementId);
		}

		// Token: 0x06007AE2 RID: 31458 RVA: 0x00480CB3 File Offset: 0x0047EEB3
		private void ClearAreaBlocks26(DataContext context)
		{
			this._areaBlocks26.Clear();
			this._modificationsAreaBlocks26.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(27, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 27);
		}

		// Token: 0x06007AE3 RID: 31459 RVA: 0x00480CEC File Offset: 0x0047EEEC
		public MapBlockData GetElement_AreaBlocks27(short elementId)
		{
			return this._areaBlocks27[elementId];
		}

		// Token: 0x06007AE4 RID: 31460 RVA: 0x00480D0C File Offset: 0x0047EF0C
		public bool TryGetElement_AreaBlocks27(short elementId, out MapBlockData value)
		{
			return this._areaBlocks27.TryGetValue(elementId, out value);
		}

		// Token: 0x06007AE5 RID: 31461 RVA: 0x00480D2C File Offset: 0x0047EF2C
		private unsafe void AddElement_AreaBlocks27(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks27.Add(elementId, value);
			this._modificationsAreaBlocks27.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(28, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 28, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 28, elementId, 0);
			}
		}

		// Token: 0x06007AE6 RID: 31462 RVA: 0x00480D9C File Offset: 0x0047EF9C
		private unsafe void SetElement_AreaBlocks27(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks27[elementId] = value;
			this._modificationsAreaBlocks27.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(28, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 28, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 28, elementId, 0);
			}
		}

		// Token: 0x06007AE7 RID: 31463 RVA: 0x00480E0C File Offset: 0x0047F00C
		private void RemoveElement_AreaBlocks27(short elementId, DataContext context)
		{
			this._areaBlocks27.Remove(elementId);
			this._modificationsAreaBlocks27.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(28, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 28, elementId);
		}

		// Token: 0x06007AE8 RID: 31464 RVA: 0x00480E47 File Offset: 0x0047F047
		private void ClearAreaBlocks27(DataContext context)
		{
			this._areaBlocks27.Clear();
			this._modificationsAreaBlocks27.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(28, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 28);
		}

		// Token: 0x06007AE9 RID: 31465 RVA: 0x00480E80 File Offset: 0x0047F080
		public MapBlockData GetElement_AreaBlocks28(short elementId)
		{
			return this._areaBlocks28[elementId];
		}

		// Token: 0x06007AEA RID: 31466 RVA: 0x00480EA0 File Offset: 0x0047F0A0
		public bool TryGetElement_AreaBlocks28(short elementId, out MapBlockData value)
		{
			return this._areaBlocks28.TryGetValue(elementId, out value);
		}

		// Token: 0x06007AEB RID: 31467 RVA: 0x00480EC0 File Offset: 0x0047F0C0
		private unsafe void AddElement_AreaBlocks28(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks28.Add(elementId, value);
			this._modificationsAreaBlocks28.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(29, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 29, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 29, elementId, 0);
			}
		}

		// Token: 0x06007AEC RID: 31468 RVA: 0x00480F30 File Offset: 0x0047F130
		private unsafe void SetElement_AreaBlocks28(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks28[elementId] = value;
			this._modificationsAreaBlocks28.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(29, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 29, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 29, elementId, 0);
			}
		}

		// Token: 0x06007AED RID: 31469 RVA: 0x00480FA0 File Offset: 0x0047F1A0
		private void RemoveElement_AreaBlocks28(short elementId, DataContext context)
		{
			this._areaBlocks28.Remove(elementId);
			this._modificationsAreaBlocks28.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(29, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 29, elementId);
		}

		// Token: 0x06007AEE RID: 31470 RVA: 0x00480FDB File Offset: 0x0047F1DB
		private void ClearAreaBlocks28(DataContext context)
		{
			this._areaBlocks28.Clear();
			this._modificationsAreaBlocks28.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(29, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 29);
		}

		// Token: 0x06007AEF RID: 31471 RVA: 0x00481014 File Offset: 0x0047F214
		public MapBlockData GetElement_AreaBlocks29(short elementId)
		{
			return this._areaBlocks29[elementId];
		}

		// Token: 0x06007AF0 RID: 31472 RVA: 0x00481034 File Offset: 0x0047F234
		public bool TryGetElement_AreaBlocks29(short elementId, out MapBlockData value)
		{
			return this._areaBlocks29.TryGetValue(elementId, out value);
		}

		// Token: 0x06007AF1 RID: 31473 RVA: 0x00481054 File Offset: 0x0047F254
		private unsafe void AddElement_AreaBlocks29(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks29.Add(elementId, value);
			this._modificationsAreaBlocks29.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(30, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 30, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 30, elementId, 0);
			}
		}

		// Token: 0x06007AF2 RID: 31474 RVA: 0x004810C4 File Offset: 0x0047F2C4
		private unsafe void SetElement_AreaBlocks29(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks29[elementId] = value;
			this._modificationsAreaBlocks29.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(30, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 30, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 30, elementId, 0);
			}
		}

		// Token: 0x06007AF3 RID: 31475 RVA: 0x00481134 File Offset: 0x0047F334
		private void RemoveElement_AreaBlocks29(short elementId, DataContext context)
		{
			this._areaBlocks29.Remove(elementId);
			this._modificationsAreaBlocks29.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(30, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 30, elementId);
		}

		// Token: 0x06007AF4 RID: 31476 RVA: 0x0048116F File Offset: 0x0047F36F
		private void ClearAreaBlocks29(DataContext context)
		{
			this._areaBlocks29.Clear();
			this._modificationsAreaBlocks29.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(30, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 30);
		}

		// Token: 0x06007AF5 RID: 31477 RVA: 0x004811A8 File Offset: 0x0047F3A8
		public MapBlockData GetElement_AreaBlocks30(short elementId)
		{
			return this._areaBlocks30[elementId];
		}

		// Token: 0x06007AF6 RID: 31478 RVA: 0x004811C8 File Offset: 0x0047F3C8
		public bool TryGetElement_AreaBlocks30(short elementId, out MapBlockData value)
		{
			return this._areaBlocks30.TryGetValue(elementId, out value);
		}

		// Token: 0x06007AF7 RID: 31479 RVA: 0x004811E8 File Offset: 0x0047F3E8
		private unsafe void AddElement_AreaBlocks30(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks30.Add(elementId, value);
			this._modificationsAreaBlocks30.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(31, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 31, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 31, elementId, 0);
			}
		}

		// Token: 0x06007AF8 RID: 31480 RVA: 0x00481258 File Offset: 0x0047F458
		private unsafe void SetElement_AreaBlocks30(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks30[elementId] = value;
			this._modificationsAreaBlocks30.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(31, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 31, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 31, elementId, 0);
			}
		}

		// Token: 0x06007AF9 RID: 31481 RVA: 0x004812C8 File Offset: 0x0047F4C8
		private void RemoveElement_AreaBlocks30(short elementId, DataContext context)
		{
			this._areaBlocks30.Remove(elementId);
			this._modificationsAreaBlocks30.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(31, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 31, elementId);
		}

		// Token: 0x06007AFA RID: 31482 RVA: 0x00481303 File Offset: 0x0047F503
		private void ClearAreaBlocks30(DataContext context)
		{
			this._areaBlocks30.Clear();
			this._modificationsAreaBlocks30.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(31, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 31);
		}

		// Token: 0x06007AFB RID: 31483 RVA: 0x0048133C File Offset: 0x0047F53C
		public MapBlockData GetElement_AreaBlocks31(short elementId)
		{
			return this._areaBlocks31[elementId];
		}

		// Token: 0x06007AFC RID: 31484 RVA: 0x0048135C File Offset: 0x0047F55C
		public bool TryGetElement_AreaBlocks31(short elementId, out MapBlockData value)
		{
			return this._areaBlocks31.TryGetValue(elementId, out value);
		}

		// Token: 0x06007AFD RID: 31485 RVA: 0x0048137C File Offset: 0x0047F57C
		private unsafe void AddElement_AreaBlocks31(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks31.Add(elementId, value);
			this._modificationsAreaBlocks31.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(32, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 32, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 32, elementId, 0);
			}
		}

		// Token: 0x06007AFE RID: 31486 RVA: 0x004813EC File Offset: 0x0047F5EC
		private unsafe void SetElement_AreaBlocks31(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks31[elementId] = value;
			this._modificationsAreaBlocks31.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(32, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 32, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 32, elementId, 0);
			}
		}

		// Token: 0x06007AFF RID: 31487 RVA: 0x0048145C File Offset: 0x0047F65C
		private void RemoveElement_AreaBlocks31(short elementId, DataContext context)
		{
			this._areaBlocks31.Remove(elementId);
			this._modificationsAreaBlocks31.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(32, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 32, elementId);
		}

		// Token: 0x06007B00 RID: 31488 RVA: 0x00481497 File Offset: 0x0047F697
		private void ClearAreaBlocks31(DataContext context)
		{
			this._areaBlocks31.Clear();
			this._modificationsAreaBlocks31.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(32, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 32);
		}

		// Token: 0x06007B01 RID: 31489 RVA: 0x004814D0 File Offset: 0x0047F6D0
		public MapBlockData GetElement_AreaBlocks32(short elementId)
		{
			return this._areaBlocks32[elementId];
		}

		// Token: 0x06007B02 RID: 31490 RVA: 0x004814F0 File Offset: 0x0047F6F0
		public bool TryGetElement_AreaBlocks32(short elementId, out MapBlockData value)
		{
			return this._areaBlocks32.TryGetValue(elementId, out value);
		}

		// Token: 0x06007B03 RID: 31491 RVA: 0x00481510 File Offset: 0x0047F710
		private unsafe void AddElement_AreaBlocks32(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks32.Add(elementId, value);
			this._modificationsAreaBlocks32.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(33, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 33, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 33, elementId, 0);
			}
		}

		// Token: 0x06007B04 RID: 31492 RVA: 0x00481580 File Offset: 0x0047F780
		private unsafe void SetElement_AreaBlocks32(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks32[elementId] = value;
			this._modificationsAreaBlocks32.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(33, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 33, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 33, elementId, 0);
			}
		}

		// Token: 0x06007B05 RID: 31493 RVA: 0x004815F0 File Offset: 0x0047F7F0
		private void RemoveElement_AreaBlocks32(short elementId, DataContext context)
		{
			this._areaBlocks32.Remove(elementId);
			this._modificationsAreaBlocks32.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(33, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 33, elementId);
		}

		// Token: 0x06007B06 RID: 31494 RVA: 0x0048162B File Offset: 0x0047F82B
		private void ClearAreaBlocks32(DataContext context)
		{
			this._areaBlocks32.Clear();
			this._modificationsAreaBlocks32.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(33, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 33);
		}

		// Token: 0x06007B07 RID: 31495 RVA: 0x00481664 File Offset: 0x0047F864
		public MapBlockData GetElement_AreaBlocks33(short elementId)
		{
			return this._areaBlocks33[elementId];
		}

		// Token: 0x06007B08 RID: 31496 RVA: 0x00481684 File Offset: 0x0047F884
		public bool TryGetElement_AreaBlocks33(short elementId, out MapBlockData value)
		{
			return this._areaBlocks33.TryGetValue(elementId, out value);
		}

		// Token: 0x06007B09 RID: 31497 RVA: 0x004816A4 File Offset: 0x0047F8A4
		private unsafe void AddElement_AreaBlocks33(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks33.Add(elementId, value);
			this._modificationsAreaBlocks33.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(34, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 34, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 34, elementId, 0);
			}
		}

		// Token: 0x06007B0A RID: 31498 RVA: 0x00481714 File Offset: 0x0047F914
		private unsafe void SetElement_AreaBlocks33(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks33[elementId] = value;
			this._modificationsAreaBlocks33.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(34, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 34, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 34, elementId, 0);
			}
		}

		// Token: 0x06007B0B RID: 31499 RVA: 0x00481784 File Offset: 0x0047F984
		private void RemoveElement_AreaBlocks33(short elementId, DataContext context)
		{
			this._areaBlocks33.Remove(elementId);
			this._modificationsAreaBlocks33.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(34, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 34, elementId);
		}

		// Token: 0x06007B0C RID: 31500 RVA: 0x004817BF File Offset: 0x0047F9BF
		private void ClearAreaBlocks33(DataContext context)
		{
			this._areaBlocks33.Clear();
			this._modificationsAreaBlocks33.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(34, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 34);
		}

		// Token: 0x06007B0D RID: 31501 RVA: 0x004817F8 File Offset: 0x0047F9F8
		public MapBlockData GetElement_AreaBlocks34(short elementId)
		{
			return this._areaBlocks34[elementId];
		}

		// Token: 0x06007B0E RID: 31502 RVA: 0x00481818 File Offset: 0x0047FA18
		public bool TryGetElement_AreaBlocks34(short elementId, out MapBlockData value)
		{
			return this._areaBlocks34.TryGetValue(elementId, out value);
		}

		// Token: 0x06007B0F RID: 31503 RVA: 0x00481838 File Offset: 0x0047FA38
		private unsafe void AddElement_AreaBlocks34(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks34.Add(elementId, value);
			this._modificationsAreaBlocks34.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(35, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 35, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 35, elementId, 0);
			}
		}

		// Token: 0x06007B10 RID: 31504 RVA: 0x004818A8 File Offset: 0x0047FAA8
		private unsafe void SetElement_AreaBlocks34(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks34[elementId] = value;
			this._modificationsAreaBlocks34.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(35, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 35, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 35, elementId, 0);
			}
		}

		// Token: 0x06007B11 RID: 31505 RVA: 0x00481918 File Offset: 0x0047FB18
		private void RemoveElement_AreaBlocks34(short elementId, DataContext context)
		{
			this._areaBlocks34.Remove(elementId);
			this._modificationsAreaBlocks34.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(35, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 35, elementId);
		}

		// Token: 0x06007B12 RID: 31506 RVA: 0x00481953 File Offset: 0x0047FB53
		private void ClearAreaBlocks34(DataContext context)
		{
			this._areaBlocks34.Clear();
			this._modificationsAreaBlocks34.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(35, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 35);
		}

		// Token: 0x06007B13 RID: 31507 RVA: 0x0048198C File Offset: 0x0047FB8C
		public MapBlockData GetElement_AreaBlocks35(short elementId)
		{
			return this._areaBlocks35[elementId];
		}

		// Token: 0x06007B14 RID: 31508 RVA: 0x004819AC File Offset: 0x0047FBAC
		public bool TryGetElement_AreaBlocks35(short elementId, out MapBlockData value)
		{
			return this._areaBlocks35.TryGetValue(elementId, out value);
		}

		// Token: 0x06007B15 RID: 31509 RVA: 0x004819CC File Offset: 0x0047FBCC
		private unsafe void AddElement_AreaBlocks35(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks35.Add(elementId, value);
			this._modificationsAreaBlocks35.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(36, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 36, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 36, elementId, 0);
			}
		}

		// Token: 0x06007B16 RID: 31510 RVA: 0x00481A3C File Offset: 0x0047FC3C
		private unsafe void SetElement_AreaBlocks35(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks35[elementId] = value;
			this._modificationsAreaBlocks35.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(36, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 36, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 36, elementId, 0);
			}
		}

		// Token: 0x06007B17 RID: 31511 RVA: 0x00481AAC File Offset: 0x0047FCAC
		private void RemoveElement_AreaBlocks35(short elementId, DataContext context)
		{
			this._areaBlocks35.Remove(elementId);
			this._modificationsAreaBlocks35.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(36, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 36, elementId);
		}

		// Token: 0x06007B18 RID: 31512 RVA: 0x00481AE7 File Offset: 0x0047FCE7
		private void ClearAreaBlocks35(DataContext context)
		{
			this._areaBlocks35.Clear();
			this._modificationsAreaBlocks35.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(36, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 36);
		}

		// Token: 0x06007B19 RID: 31513 RVA: 0x00481B20 File Offset: 0x0047FD20
		public MapBlockData GetElement_AreaBlocks36(short elementId)
		{
			return this._areaBlocks36[elementId];
		}

		// Token: 0x06007B1A RID: 31514 RVA: 0x00481B40 File Offset: 0x0047FD40
		public bool TryGetElement_AreaBlocks36(short elementId, out MapBlockData value)
		{
			return this._areaBlocks36.TryGetValue(elementId, out value);
		}

		// Token: 0x06007B1B RID: 31515 RVA: 0x00481B60 File Offset: 0x0047FD60
		private unsafe void AddElement_AreaBlocks36(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks36.Add(elementId, value);
			this._modificationsAreaBlocks36.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(37, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 37, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 37, elementId, 0);
			}
		}

		// Token: 0x06007B1C RID: 31516 RVA: 0x00481BD0 File Offset: 0x0047FDD0
		private unsafe void SetElement_AreaBlocks36(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks36[elementId] = value;
			this._modificationsAreaBlocks36.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(37, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 37, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 37, elementId, 0);
			}
		}

		// Token: 0x06007B1D RID: 31517 RVA: 0x00481C40 File Offset: 0x0047FE40
		private void RemoveElement_AreaBlocks36(short elementId, DataContext context)
		{
			this._areaBlocks36.Remove(elementId);
			this._modificationsAreaBlocks36.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(37, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 37, elementId);
		}

		// Token: 0x06007B1E RID: 31518 RVA: 0x00481C7B File Offset: 0x0047FE7B
		private void ClearAreaBlocks36(DataContext context)
		{
			this._areaBlocks36.Clear();
			this._modificationsAreaBlocks36.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(37, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 37);
		}

		// Token: 0x06007B1F RID: 31519 RVA: 0x00481CB4 File Offset: 0x0047FEB4
		public MapBlockData GetElement_AreaBlocks37(short elementId)
		{
			return this._areaBlocks37[elementId];
		}

		// Token: 0x06007B20 RID: 31520 RVA: 0x00481CD4 File Offset: 0x0047FED4
		public bool TryGetElement_AreaBlocks37(short elementId, out MapBlockData value)
		{
			return this._areaBlocks37.TryGetValue(elementId, out value);
		}

		// Token: 0x06007B21 RID: 31521 RVA: 0x00481CF4 File Offset: 0x0047FEF4
		private unsafe void AddElement_AreaBlocks37(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks37.Add(elementId, value);
			this._modificationsAreaBlocks37.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(38, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 38, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 38, elementId, 0);
			}
		}

		// Token: 0x06007B22 RID: 31522 RVA: 0x00481D64 File Offset: 0x0047FF64
		private unsafe void SetElement_AreaBlocks37(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks37[elementId] = value;
			this._modificationsAreaBlocks37.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(38, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 38, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 38, elementId, 0);
			}
		}

		// Token: 0x06007B23 RID: 31523 RVA: 0x00481DD4 File Offset: 0x0047FFD4
		private void RemoveElement_AreaBlocks37(short elementId, DataContext context)
		{
			this._areaBlocks37.Remove(elementId);
			this._modificationsAreaBlocks37.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(38, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 38, elementId);
		}

		// Token: 0x06007B24 RID: 31524 RVA: 0x00481E0F File Offset: 0x0048000F
		private void ClearAreaBlocks37(DataContext context)
		{
			this._areaBlocks37.Clear();
			this._modificationsAreaBlocks37.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(38, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 38);
		}

		// Token: 0x06007B25 RID: 31525 RVA: 0x00481E48 File Offset: 0x00480048
		public MapBlockData GetElement_AreaBlocks38(short elementId)
		{
			return this._areaBlocks38[elementId];
		}

		// Token: 0x06007B26 RID: 31526 RVA: 0x00481E68 File Offset: 0x00480068
		public bool TryGetElement_AreaBlocks38(short elementId, out MapBlockData value)
		{
			return this._areaBlocks38.TryGetValue(elementId, out value);
		}

		// Token: 0x06007B27 RID: 31527 RVA: 0x00481E88 File Offset: 0x00480088
		private unsafe void AddElement_AreaBlocks38(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks38.Add(elementId, value);
			this._modificationsAreaBlocks38.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(39, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 39, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 39, elementId, 0);
			}
		}

		// Token: 0x06007B28 RID: 31528 RVA: 0x00481EF8 File Offset: 0x004800F8
		private unsafe void SetElement_AreaBlocks38(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks38[elementId] = value;
			this._modificationsAreaBlocks38.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(39, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 39, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 39, elementId, 0);
			}
		}

		// Token: 0x06007B29 RID: 31529 RVA: 0x00481F68 File Offset: 0x00480168
		private void RemoveElement_AreaBlocks38(short elementId, DataContext context)
		{
			this._areaBlocks38.Remove(elementId);
			this._modificationsAreaBlocks38.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(39, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 39, elementId);
		}

		// Token: 0x06007B2A RID: 31530 RVA: 0x00481FA3 File Offset: 0x004801A3
		private void ClearAreaBlocks38(DataContext context)
		{
			this._areaBlocks38.Clear();
			this._modificationsAreaBlocks38.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(39, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 39);
		}

		// Token: 0x06007B2B RID: 31531 RVA: 0x00481FDC File Offset: 0x004801DC
		public MapBlockData GetElement_AreaBlocks39(short elementId)
		{
			return this._areaBlocks39[elementId];
		}

		// Token: 0x06007B2C RID: 31532 RVA: 0x00481FFC File Offset: 0x004801FC
		public bool TryGetElement_AreaBlocks39(short elementId, out MapBlockData value)
		{
			return this._areaBlocks39.TryGetValue(elementId, out value);
		}

		// Token: 0x06007B2D RID: 31533 RVA: 0x0048201C File Offset: 0x0048021C
		private unsafe void AddElement_AreaBlocks39(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks39.Add(elementId, value);
			this._modificationsAreaBlocks39.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(40, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 40, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 40, elementId, 0);
			}
		}

		// Token: 0x06007B2E RID: 31534 RVA: 0x0048208C File Offset: 0x0048028C
		private unsafe void SetElement_AreaBlocks39(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks39[elementId] = value;
			this._modificationsAreaBlocks39.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(40, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 40, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 40, elementId, 0);
			}
		}

		// Token: 0x06007B2F RID: 31535 RVA: 0x004820FC File Offset: 0x004802FC
		private void RemoveElement_AreaBlocks39(short elementId, DataContext context)
		{
			this._areaBlocks39.Remove(elementId);
			this._modificationsAreaBlocks39.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(40, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 40, elementId);
		}

		// Token: 0x06007B30 RID: 31536 RVA: 0x00482137 File Offset: 0x00480337
		private void ClearAreaBlocks39(DataContext context)
		{
			this._areaBlocks39.Clear();
			this._modificationsAreaBlocks39.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(40, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 40);
		}

		// Token: 0x06007B31 RID: 31537 RVA: 0x00482170 File Offset: 0x00480370
		public MapBlockData GetElement_AreaBlocks40(short elementId)
		{
			return this._areaBlocks40[elementId];
		}

		// Token: 0x06007B32 RID: 31538 RVA: 0x00482190 File Offset: 0x00480390
		public bool TryGetElement_AreaBlocks40(short elementId, out MapBlockData value)
		{
			return this._areaBlocks40.TryGetValue(elementId, out value);
		}

		// Token: 0x06007B33 RID: 31539 RVA: 0x004821B0 File Offset: 0x004803B0
		private unsafe void AddElement_AreaBlocks40(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks40.Add(elementId, value);
			this._modificationsAreaBlocks40.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(41, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 41, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 41, elementId, 0);
			}
		}

		// Token: 0x06007B34 RID: 31540 RVA: 0x00482220 File Offset: 0x00480420
		private unsafe void SetElement_AreaBlocks40(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks40[elementId] = value;
			this._modificationsAreaBlocks40.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(41, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 41, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 41, elementId, 0);
			}
		}

		// Token: 0x06007B35 RID: 31541 RVA: 0x00482290 File Offset: 0x00480490
		private void RemoveElement_AreaBlocks40(short elementId, DataContext context)
		{
			this._areaBlocks40.Remove(elementId);
			this._modificationsAreaBlocks40.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(41, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 41, elementId);
		}

		// Token: 0x06007B36 RID: 31542 RVA: 0x004822CB File Offset: 0x004804CB
		private void ClearAreaBlocks40(DataContext context)
		{
			this._areaBlocks40.Clear();
			this._modificationsAreaBlocks40.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(41, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 41);
		}

		// Token: 0x06007B37 RID: 31543 RVA: 0x00482304 File Offset: 0x00480504
		public MapBlockData GetElement_AreaBlocks41(short elementId)
		{
			return this._areaBlocks41[elementId];
		}

		// Token: 0x06007B38 RID: 31544 RVA: 0x00482324 File Offset: 0x00480524
		public bool TryGetElement_AreaBlocks41(short elementId, out MapBlockData value)
		{
			return this._areaBlocks41.TryGetValue(elementId, out value);
		}

		// Token: 0x06007B39 RID: 31545 RVA: 0x00482344 File Offset: 0x00480544
		private unsafe void AddElement_AreaBlocks41(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks41.Add(elementId, value);
			this._modificationsAreaBlocks41.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(42, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 42, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 42, elementId, 0);
			}
		}

		// Token: 0x06007B3A RID: 31546 RVA: 0x004823B4 File Offset: 0x004805B4
		private unsafe void SetElement_AreaBlocks41(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks41[elementId] = value;
			this._modificationsAreaBlocks41.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(42, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 42, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 42, elementId, 0);
			}
		}

		// Token: 0x06007B3B RID: 31547 RVA: 0x00482424 File Offset: 0x00480624
		private void RemoveElement_AreaBlocks41(short elementId, DataContext context)
		{
			this._areaBlocks41.Remove(elementId);
			this._modificationsAreaBlocks41.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(42, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 42, elementId);
		}

		// Token: 0x06007B3C RID: 31548 RVA: 0x0048245F File Offset: 0x0048065F
		private void ClearAreaBlocks41(DataContext context)
		{
			this._areaBlocks41.Clear();
			this._modificationsAreaBlocks41.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(42, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 42);
		}

		// Token: 0x06007B3D RID: 31549 RVA: 0x00482498 File Offset: 0x00480698
		public MapBlockData GetElement_AreaBlocks42(short elementId)
		{
			return this._areaBlocks42[elementId];
		}

		// Token: 0x06007B3E RID: 31550 RVA: 0x004824B8 File Offset: 0x004806B8
		public bool TryGetElement_AreaBlocks42(short elementId, out MapBlockData value)
		{
			return this._areaBlocks42.TryGetValue(elementId, out value);
		}

		// Token: 0x06007B3F RID: 31551 RVA: 0x004824D8 File Offset: 0x004806D8
		private unsafe void AddElement_AreaBlocks42(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks42.Add(elementId, value);
			this._modificationsAreaBlocks42.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(43, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 43, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 43, elementId, 0);
			}
		}

		// Token: 0x06007B40 RID: 31552 RVA: 0x00482548 File Offset: 0x00480748
		private unsafe void SetElement_AreaBlocks42(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks42[elementId] = value;
			this._modificationsAreaBlocks42.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(43, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 43, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 43, elementId, 0);
			}
		}

		// Token: 0x06007B41 RID: 31553 RVA: 0x004825B8 File Offset: 0x004807B8
		private void RemoveElement_AreaBlocks42(short elementId, DataContext context)
		{
			this._areaBlocks42.Remove(elementId);
			this._modificationsAreaBlocks42.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(43, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 43, elementId);
		}

		// Token: 0x06007B42 RID: 31554 RVA: 0x004825F3 File Offset: 0x004807F3
		private void ClearAreaBlocks42(DataContext context)
		{
			this._areaBlocks42.Clear();
			this._modificationsAreaBlocks42.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(43, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 43);
		}

		// Token: 0x06007B43 RID: 31555 RVA: 0x0048262C File Offset: 0x0048082C
		public MapBlockData GetElement_AreaBlocks43(short elementId)
		{
			return this._areaBlocks43[elementId];
		}

		// Token: 0x06007B44 RID: 31556 RVA: 0x0048264C File Offset: 0x0048084C
		public bool TryGetElement_AreaBlocks43(short elementId, out MapBlockData value)
		{
			return this._areaBlocks43.TryGetValue(elementId, out value);
		}

		// Token: 0x06007B45 RID: 31557 RVA: 0x0048266C File Offset: 0x0048086C
		private unsafe void AddElement_AreaBlocks43(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks43.Add(elementId, value);
			this._modificationsAreaBlocks43.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(44, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 44, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 44, elementId, 0);
			}
		}

		// Token: 0x06007B46 RID: 31558 RVA: 0x004826DC File Offset: 0x004808DC
		private unsafe void SetElement_AreaBlocks43(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks43[elementId] = value;
			this._modificationsAreaBlocks43.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(44, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 44, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 44, elementId, 0);
			}
		}

		// Token: 0x06007B47 RID: 31559 RVA: 0x0048274C File Offset: 0x0048094C
		private void RemoveElement_AreaBlocks43(short elementId, DataContext context)
		{
			this._areaBlocks43.Remove(elementId);
			this._modificationsAreaBlocks43.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(44, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 44, elementId);
		}

		// Token: 0x06007B48 RID: 31560 RVA: 0x00482787 File Offset: 0x00480987
		private void ClearAreaBlocks43(DataContext context)
		{
			this._areaBlocks43.Clear();
			this._modificationsAreaBlocks43.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(44, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 44);
		}

		// Token: 0x06007B49 RID: 31561 RVA: 0x004827C0 File Offset: 0x004809C0
		public MapBlockData GetElement_AreaBlocks44(short elementId)
		{
			return this._areaBlocks44[elementId];
		}

		// Token: 0x06007B4A RID: 31562 RVA: 0x004827E0 File Offset: 0x004809E0
		public bool TryGetElement_AreaBlocks44(short elementId, out MapBlockData value)
		{
			return this._areaBlocks44.TryGetValue(elementId, out value);
		}

		// Token: 0x06007B4B RID: 31563 RVA: 0x00482800 File Offset: 0x00480A00
		private unsafe void AddElement_AreaBlocks44(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks44.Add(elementId, value);
			this._modificationsAreaBlocks44.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(45, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 45, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 45, elementId, 0);
			}
		}

		// Token: 0x06007B4C RID: 31564 RVA: 0x00482870 File Offset: 0x00480A70
		private unsafe void SetElement_AreaBlocks44(short elementId, MapBlockData value, DataContext context)
		{
			this._areaBlocks44[elementId] = value;
			this._modificationsAreaBlocks44.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(45, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 45, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 45, elementId, 0);
			}
		}

		// Token: 0x06007B4D RID: 31565 RVA: 0x004828E0 File Offset: 0x00480AE0
		private void RemoveElement_AreaBlocks44(short elementId, DataContext context)
		{
			this._areaBlocks44.Remove(elementId);
			this._modificationsAreaBlocks44.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(45, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 45, elementId);
		}

		// Token: 0x06007B4E RID: 31566 RVA: 0x0048291B File Offset: 0x00480B1B
		private void ClearAreaBlocks44(DataContext context)
		{
			this._areaBlocks44.Clear();
			this._modificationsAreaBlocks44.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(45, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 45);
		}

		// Token: 0x06007B4F RID: 31567 RVA: 0x00482954 File Offset: 0x00480B54
		public MapBlockData GetElement_BrokenAreaBlocks(short elementId)
		{
			return this._brokenAreaBlocks[elementId];
		}

		// Token: 0x06007B50 RID: 31568 RVA: 0x00482974 File Offset: 0x00480B74
		public bool TryGetElement_BrokenAreaBlocks(short elementId, out MapBlockData value)
		{
			return this._brokenAreaBlocks.TryGetValue(elementId, out value);
		}

		// Token: 0x06007B51 RID: 31569 RVA: 0x00482994 File Offset: 0x00480B94
		private unsafe void AddElement_BrokenAreaBlocks(short elementId, MapBlockData value, DataContext context)
		{
			this._brokenAreaBlocks.Add(elementId, value);
			this._modificationsBrokenAreaBlocks.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(46, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 46, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 46, elementId, 0);
			}
		}

		// Token: 0x06007B52 RID: 31570 RVA: 0x00482A04 File Offset: 0x00480C04
		private unsafe void SetElement_BrokenAreaBlocks(short elementId, MapBlockData value, DataContext context)
		{
			this._brokenAreaBlocks[elementId] = value;
			this._modificationsBrokenAreaBlocks.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(46, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 46, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 46, elementId, 0);
			}
		}

		// Token: 0x06007B53 RID: 31571 RVA: 0x00482A74 File Offset: 0x00480C74
		private void RemoveElement_BrokenAreaBlocks(short elementId, DataContext context)
		{
			this._brokenAreaBlocks.Remove(elementId);
			this._modificationsBrokenAreaBlocks.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(46, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 46, elementId);
		}

		// Token: 0x06007B54 RID: 31572 RVA: 0x00482AAF File Offset: 0x00480CAF
		private void ClearBrokenAreaBlocks(DataContext context)
		{
			this._brokenAreaBlocks.Clear();
			this._modificationsBrokenAreaBlocks.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(46, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 46);
		}

		// Token: 0x06007B55 RID: 31573 RVA: 0x00482AE8 File Offset: 0x00480CE8
		public MapBlockData GetElement_BornAreaBlocks(short elementId)
		{
			return this._bornAreaBlocks[elementId];
		}

		// Token: 0x06007B56 RID: 31574 RVA: 0x00482B08 File Offset: 0x00480D08
		public bool TryGetElement_BornAreaBlocks(short elementId, out MapBlockData value)
		{
			return this._bornAreaBlocks.TryGetValue(elementId, out value);
		}

		// Token: 0x06007B57 RID: 31575 RVA: 0x00482B28 File Offset: 0x00480D28
		private unsafe void AddElement_BornAreaBlocks(short elementId, MapBlockData value, DataContext context)
		{
			this._bornAreaBlocks.Add(elementId, value);
			this._modificationsBornAreaBlocks.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(47, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 47, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 47, elementId, 0);
			}
		}

		// Token: 0x06007B58 RID: 31576 RVA: 0x00482B98 File Offset: 0x00480D98
		private unsafe void SetElement_BornAreaBlocks(short elementId, MapBlockData value, DataContext context)
		{
			this._bornAreaBlocks[elementId] = value;
			this._modificationsBornAreaBlocks.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(47, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 47, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 47, elementId, 0);
			}
		}

		// Token: 0x06007B59 RID: 31577 RVA: 0x00482C08 File Offset: 0x00480E08
		private void RemoveElement_BornAreaBlocks(short elementId, DataContext context)
		{
			this._bornAreaBlocks.Remove(elementId);
			this._modificationsBornAreaBlocks.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(47, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 47, elementId);
		}

		// Token: 0x06007B5A RID: 31578 RVA: 0x00482C43 File Offset: 0x00480E43
		private void ClearBornAreaBlocks(DataContext context)
		{
			this._bornAreaBlocks.Clear();
			this._modificationsBornAreaBlocks.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(47, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 47);
		}

		// Token: 0x06007B5B RID: 31579 RVA: 0x00482C7C File Offset: 0x00480E7C
		public MapBlockData GetElement_GuideAreaBlocks(short elementId)
		{
			return this._guideAreaBlocks[elementId];
		}

		// Token: 0x06007B5C RID: 31580 RVA: 0x00482C9C File Offset: 0x00480E9C
		public bool TryGetElement_GuideAreaBlocks(short elementId, out MapBlockData value)
		{
			return this._guideAreaBlocks.TryGetValue(elementId, out value);
		}

		// Token: 0x06007B5D RID: 31581 RVA: 0x00482CBC File Offset: 0x00480EBC
		private unsafe void AddElement_GuideAreaBlocks(short elementId, MapBlockData value, DataContext context)
		{
			this._guideAreaBlocks.Add(elementId, value);
			this._modificationsGuideAreaBlocks.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(48, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 48, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 48, elementId, 0);
			}
		}

		// Token: 0x06007B5E RID: 31582 RVA: 0x00482D2C File Offset: 0x00480F2C
		private unsafe void SetElement_GuideAreaBlocks(short elementId, MapBlockData value, DataContext context)
		{
			this._guideAreaBlocks[elementId] = value;
			this._modificationsGuideAreaBlocks.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(48, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 48, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 48, elementId, 0);
			}
		}

		// Token: 0x06007B5F RID: 31583 RVA: 0x00482D9C File Offset: 0x00480F9C
		private void RemoveElement_GuideAreaBlocks(short elementId, DataContext context)
		{
			this._guideAreaBlocks.Remove(elementId);
			this._modificationsGuideAreaBlocks.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(48, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 48, elementId);
		}

		// Token: 0x06007B60 RID: 31584 RVA: 0x00482DD7 File Offset: 0x00480FD7
		private void ClearGuideAreaBlocks(DataContext context)
		{
			this._guideAreaBlocks.Clear();
			this._modificationsGuideAreaBlocks.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(48, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 48);
		}

		// Token: 0x06007B61 RID: 31585 RVA: 0x00482E10 File Offset: 0x00481010
		public MapBlockData GetElement_SecretVillageAreaBlocks(short elementId)
		{
			return this._secretVillageAreaBlocks[elementId];
		}

		// Token: 0x06007B62 RID: 31586 RVA: 0x00482E30 File Offset: 0x00481030
		public bool TryGetElement_SecretVillageAreaBlocks(short elementId, out MapBlockData value)
		{
			return this._secretVillageAreaBlocks.TryGetValue(elementId, out value);
		}

		// Token: 0x06007B63 RID: 31587 RVA: 0x00482E50 File Offset: 0x00481050
		private unsafe void AddElement_SecretVillageAreaBlocks(short elementId, MapBlockData value, DataContext context)
		{
			this._secretVillageAreaBlocks.Add(elementId, value);
			this._modificationsSecretVillageAreaBlocks.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(49, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 49, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 49, elementId, 0);
			}
		}

		// Token: 0x06007B64 RID: 31588 RVA: 0x00482EC0 File Offset: 0x004810C0
		private unsafe void SetElement_SecretVillageAreaBlocks(short elementId, MapBlockData value, DataContext context)
		{
			this._secretVillageAreaBlocks[elementId] = value;
			this._modificationsSecretVillageAreaBlocks.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(49, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 49, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 49, elementId, 0);
			}
		}

		// Token: 0x06007B65 RID: 31589 RVA: 0x00482F30 File Offset: 0x00481130
		private void RemoveElement_SecretVillageAreaBlocks(short elementId, DataContext context)
		{
			this._secretVillageAreaBlocks.Remove(elementId);
			this._modificationsSecretVillageAreaBlocks.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(49, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 49, elementId);
		}

		// Token: 0x06007B66 RID: 31590 RVA: 0x00482F6B File Offset: 0x0048116B
		private void ClearSecretVillageAreaBlocks(DataContext context)
		{
			this._secretVillageAreaBlocks.Clear();
			this._modificationsSecretVillageAreaBlocks.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(49, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 49);
		}

		// Token: 0x06007B67 RID: 31591 RVA: 0x00482FA4 File Offset: 0x004811A4
		public MapBlockData GetElement_BrokenPerformAreaBlocks(short elementId)
		{
			return this._brokenPerformAreaBlocks[elementId];
		}

		// Token: 0x06007B68 RID: 31592 RVA: 0x00482FC4 File Offset: 0x004811C4
		public bool TryGetElement_BrokenPerformAreaBlocks(short elementId, out MapBlockData value)
		{
			return this._brokenPerformAreaBlocks.TryGetValue(elementId, out value);
		}

		// Token: 0x06007B69 RID: 31593 RVA: 0x00482FE4 File Offset: 0x004811E4
		private unsafe void AddElement_BrokenPerformAreaBlocks(short elementId, MapBlockData value, DataContext context)
		{
			this._brokenPerformAreaBlocks.Add(elementId, value);
			this._modificationsBrokenPerformAreaBlocks.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(50, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 50, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<short>(2, 50, elementId, 0);
			}
		}

		// Token: 0x06007B6A RID: 31594 RVA: 0x00483054 File Offset: 0x00481254
		private unsafe void SetElement_BrokenPerformAreaBlocks(short elementId, MapBlockData value, DataContext context)
		{
			this._brokenPerformAreaBlocks[elementId] = value;
			this._modificationsBrokenPerformAreaBlocks.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(50, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 50, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<short>(2, 50, elementId, 0);
			}
		}

		// Token: 0x06007B6B RID: 31595 RVA: 0x004830C4 File Offset: 0x004812C4
		private void RemoveElement_BrokenPerformAreaBlocks(short elementId, DataContext context)
		{
			this._brokenPerformAreaBlocks.Remove(elementId);
			this._modificationsBrokenPerformAreaBlocks.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(50, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 50, elementId);
		}

		// Token: 0x06007B6C RID: 31596 RVA: 0x004830FF File Offset: 0x004812FF
		private void ClearBrokenPerformAreaBlocks(DataContext context)
		{
			this._brokenPerformAreaBlocks.Clear();
			this._modificationsBrokenPerformAreaBlocks.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(50, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 50);
		}

		// Token: 0x06007B6D RID: 31597 RVA: 0x00483138 File Offset: 0x00481338
		public TravelRoute GetElement_TravelRouteDict(TravelRouteKey elementId)
		{
			return this._travelRouteDict[elementId];
		}

		// Token: 0x06007B6E RID: 31598 RVA: 0x00483158 File Offset: 0x00481358
		public bool TryGetElement_TravelRouteDict(TravelRouteKey elementId, out TravelRoute value)
		{
			return this._travelRouteDict.TryGetValue(elementId, out value);
		}

		// Token: 0x06007B6F RID: 31599 RVA: 0x00483178 File Offset: 0x00481378
		private unsafe void AddElement_TravelRouteDict(TravelRouteKey elementId, TravelRoute value, DataContext context)
		{
			this._travelRouteDict.Add(elementId, value);
			this._modificationsTravelRouteDict.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(51, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<TravelRouteKey>(2, 51, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<TravelRouteKey>(2, 51, elementId, 0);
			}
		}

		// Token: 0x06007B70 RID: 31600 RVA: 0x004831E8 File Offset: 0x004813E8
		private unsafe void SetElement_TravelRouteDict(TravelRouteKey elementId, TravelRoute value, DataContext context)
		{
			this._travelRouteDict[elementId] = value;
			this._modificationsTravelRouteDict.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(51, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<TravelRouteKey>(2, 51, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<TravelRouteKey>(2, 51, elementId, 0);
			}
		}

		// Token: 0x06007B71 RID: 31601 RVA: 0x00483258 File Offset: 0x00481458
		private void RemoveElement_TravelRouteDict(TravelRouteKey elementId, DataContext context)
		{
			this._travelRouteDict.Remove(elementId);
			this._modificationsTravelRouteDict.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(51, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<TravelRouteKey>(2, 51, elementId);
		}

		// Token: 0x06007B72 RID: 31602 RVA: 0x00483293 File Offset: 0x00481493
		private void ClearTravelRouteDict(DataContext context)
		{
			this._travelRouteDict.Clear();
			this._modificationsTravelRouteDict.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(51, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 51);
		}

		// Token: 0x06007B73 RID: 31603 RVA: 0x004832CC File Offset: 0x004814CC
		public TravelRoute GetElement_BornStateTravelRouteDict(TravelRouteKey elementId)
		{
			return this._bornStateTravelRouteDict[elementId];
		}

		// Token: 0x06007B74 RID: 31604 RVA: 0x004832EC File Offset: 0x004814EC
		public bool TryGetElement_BornStateTravelRouteDict(TravelRouteKey elementId, out TravelRoute value)
		{
			return this._bornStateTravelRouteDict.TryGetValue(elementId, out value);
		}

		// Token: 0x06007B75 RID: 31605 RVA: 0x0048330C File Offset: 0x0048150C
		private unsafe void AddElement_BornStateTravelRouteDict(TravelRouteKey elementId, TravelRoute value, DataContext context)
		{
			this._bornStateTravelRouteDict.Add(elementId, value);
			this._modificationsBornStateTravelRouteDict.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(52, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<TravelRouteKey>(2, 52, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<TravelRouteKey>(2, 52, elementId, 0);
			}
		}

		// Token: 0x06007B76 RID: 31606 RVA: 0x0048337C File Offset: 0x0048157C
		private unsafe void SetElement_BornStateTravelRouteDict(TravelRouteKey elementId, TravelRoute value, DataContext context)
		{
			this._bornStateTravelRouteDict[elementId] = value;
			this._modificationsBornStateTravelRouteDict.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(52, this.DataStates, MapDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<TravelRouteKey>(2, 52, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<TravelRouteKey>(2, 52, elementId, 0);
			}
		}

		// Token: 0x06007B77 RID: 31607 RVA: 0x004833EC File Offset: 0x004815EC
		private void RemoveElement_BornStateTravelRouteDict(TravelRouteKey elementId, DataContext context)
		{
			this._bornStateTravelRouteDict.Remove(elementId);
			this._modificationsBornStateTravelRouteDict.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(52, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<TravelRouteKey>(2, 52, elementId);
		}

		// Token: 0x06007B78 RID: 31608 RVA: 0x00483427 File Offset: 0x00481627
		private void ClearBornStateTravelRouteDict(DataContext context)
		{
			this._bornStateTravelRouteDict.Clear();
			this._modificationsBornStateTravelRouteDict.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(52, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 52);
		}

		// Token: 0x06007B79 RID: 31609 RVA: 0x00483460 File Offset: 0x00481660
		[Obsolete("DomainData _animalPlaceData is no longer in use.")]
		public AnimalPlaceData GetElement_AnimalPlaceData(int index)
		{
			return this._animalPlaceData[index];
		}

		// Token: 0x06007B7A RID: 31610 RVA: 0x0048347C File Offset: 0x0048167C
		[Obsolete("DomainData _animalPlaceData is no longer in use.")]
		private unsafe void SetElement_AnimalPlaceData(int index, AnimalPlaceData value, DataContext context)
		{
			this._animalPlaceData[index] = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(index, this._dataStatesAnimalPlaceData, MapDomain.CacheInfluencesAnimalPlaceData, context);
			bool flag = value != null;
			if (flag)
			{
				int dataSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicElementList_Set(2, 53, index, dataSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicElementList_Set(2, 53, index, 0);
			}
		}

		// Token: 0x06007B7B RID: 31611 RVA: 0x004834DC File Offset: 0x004816DC
		public CricketPlaceData GetElement_CricketPlaceData(int index)
		{
			return this._cricketPlaceData[index];
		}

		// Token: 0x06007B7C RID: 31612 RVA: 0x004834F8 File Offset: 0x004816F8
		private unsafe void SetElement_CricketPlaceData(int index, CricketPlaceData value, DataContext context)
		{
			this._cricketPlaceData[index] = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(index, this._dataStatesCricketPlaceData, MapDomain.CacheInfluencesCricketPlaceData, context);
			bool flag = value != null;
			if (flag)
			{
				int dataSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicElementList_Set(2, 54, index, dataSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicElementList_Set(2, 54, index, 0);
			}
		}

		// Token: 0x06007B7D RID: 31613 RVA: 0x00483558 File Offset: 0x00481758
		private GameData.Utilities.ShortList GetElement_RegularAreaNearList(short elementId)
		{
			return this._regularAreaNearList[elementId];
		}

		// Token: 0x06007B7E RID: 31614 RVA: 0x00483578 File Offset: 0x00481778
		private bool TryGetElement_RegularAreaNearList(short elementId, out GameData.Utilities.ShortList value)
		{
			return this._regularAreaNearList.TryGetValue(elementId, out value);
		}

		// Token: 0x06007B7F RID: 31615 RVA: 0x00483598 File Offset: 0x00481798
		private unsafe void AddElement_RegularAreaNearList(short elementId, GameData.Utilities.ShortList value, DataContext context)
		{
			this._regularAreaNearList.Add(elementId, value);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(55, this.DataStates, MapDomain.CacheInfluences, context);
			int dataSize = value.GetSerializedSize();
			byte* pData = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 55, elementId, dataSize);
			pData += value.Serialize(pData);
		}

		// Token: 0x06007B80 RID: 31616 RVA: 0x004835E8 File Offset: 0x004817E8
		private unsafe void SetElement_RegularAreaNearList(short elementId, GameData.Utilities.ShortList value, DataContext context)
		{
			this._regularAreaNearList[elementId] = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(55, this.DataStates, MapDomain.CacheInfluences, context);
			int dataSize = value.GetSerializedSize();
			byte* pData = OperationAdder.DynamicSingleValueCollection_Set<short>(2, 55, elementId, dataSize);
			pData += value.Serialize(pData);
		}

		// Token: 0x06007B81 RID: 31617 RVA: 0x00483636 File Offset: 0x00481836
		private void RemoveElement_RegularAreaNearList(short elementId, DataContext context)
		{
			this._regularAreaNearList.Remove(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(55, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<short>(2, 55, elementId);
		}

		// Token: 0x06007B82 RID: 31618 RVA: 0x00483664 File Offset: 0x00481864
		private void ClearRegularAreaNearList(DataContext context)
		{
			this._regularAreaNearList.Clear();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(55, this.DataStates, MapDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(2, 55);
		}

		// Token: 0x06007B83 RID: 31619 RVA: 0x00483690 File Offset: 0x00481890
		public Location GetElement_SwordTombLocations(int index)
		{
			return this._swordTombLocations[index];
		}

		// Token: 0x06007B84 RID: 31620 RVA: 0x004836B0 File Offset: 0x004818B0
		public unsafe void SetElement_SwordTombLocations(int index, Location value, DataContext context)
		{
			this._swordTombLocations[index] = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(index, this._dataStatesSwordTombLocations, MapDomain.CacheInfluencesSwordTombLocations, context);
			byte* pData = OperationAdder.FixedElementList_Set(2, 56, index, 4);
			pData += value.Serialize(pData);
		}

		// Token: 0x06007B85 RID: 31621 RVA: 0x004836F4 File Offset: 0x004818F4
		public CrossAreaMoveInfo GetTravelInfo()
		{
			return this._travelInfo;
		}

		// Token: 0x06007B86 RID: 31622 RVA: 0x0048370C File Offset: 0x0048190C
		private unsafe void SetTravelInfo(CrossAreaMoveInfo value, DataContext context)
		{
			this._travelInfo = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(57, this.DataStates, MapDomain.CacheInfluences, context);
			int dataSize = this._travelInfo.GetSerializedSize();
			byte* pData = OperationAdder.DynamicSingleValue_Set(2, 57, dataSize);
			pData += this._travelInfo.Serialize(pData);
		}

		// Token: 0x06007B87 RID: 31623 RVA: 0x0048375C File Offset: 0x0048195C
		public bool GetOnHandlingTravelingEventBlock()
		{
			return this._onHandlingTravelingEventBlock;
		}

		// Token: 0x06007B88 RID: 31624 RVA: 0x00483774 File Offset: 0x00481974
		public void SetOnHandlingTravelingEventBlock(bool value, DataContext context)
		{
			this._onHandlingTravelingEventBlock = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(58, this.DataStates, MapDomain.CacheInfluences, context);
		}

		// Token: 0x06007B89 RID: 31625 RVA: 0x00483794 File Offset: 0x00481994
		public List<HunterAnimalKey> GetHunterAnimalsCache()
		{
			return this._hunterAnimalsCache;
		}

		// Token: 0x06007B8A RID: 31626 RVA: 0x004837AC File Offset: 0x004819AC
		private void SetHunterAnimalsCache(List<HunterAnimalKey> value, DataContext context)
		{
			this._hunterAnimalsCache = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(59, this.DataStates, MapDomain.CacheInfluences, context);
		}

		// Token: 0x06007B8B RID: 31627 RVA: 0x004837CC File Offset: 0x004819CC
		public int GetMoveBanned()
		{
			return this._moveBanned;
		}

		// Token: 0x06007B8C RID: 31628 RVA: 0x004837E4 File Offset: 0x004819E4
		private void SetMoveBanned(int value, DataContext context)
		{
			this._moveBanned = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(60, this.DataStates, MapDomain.CacheInfluences, context);
		}

		// Token: 0x06007B8D RID: 31629 RVA: 0x00483804 File Offset: 0x00481A04
		public bool GetCrossArchiveLockMoveTime()
		{
			return this._crossArchiveLockMoveTime;
		}

		// Token: 0x06007B8E RID: 31630 RVA: 0x0048381C File Offset: 0x00481A1C
		public void SetCrossArchiveLockMoveTime(bool value, DataContext context)
		{
			this._crossArchiveLockMoveTime = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(61, this.DataStates, MapDomain.CacheInfluences, context);
		}

		// Token: 0x06007B8F RID: 31631 RVA: 0x0048383C File Offset: 0x00481A3C
		public List<Location> GetFleeBeasts()
		{
			Thread.MemoryBarrier();
			bool flag = BaseGameDataDomain.IsCached(this.DataStates, 62);
			List<Location> fleeBeasts;
			if (flag)
			{
				fleeBeasts = this._fleeBeasts;
			}
			else
			{
				List<Location> value = new List<Location>();
				this.CalcFleeBeasts(value);
				bool lockTaken = false;
				try
				{
					this._spinLockFleeBeasts.Enter(ref lockTaken);
					this._fleeBeasts.Assign(value);
					BaseGameDataDomain.SetCached(this.DataStates, 62);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLockFleeBeasts.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				fleeBeasts = this._fleeBeasts;
			}
			return fleeBeasts;
		}

		// Token: 0x06007B90 RID: 31632 RVA: 0x004838E0 File Offset: 0x00481AE0
		public List<Location> GetFleeLoongs()
		{
			Thread.MemoryBarrier();
			bool flag = BaseGameDataDomain.IsCached(this.DataStates, 63);
			List<Location> fleeLoongs;
			if (flag)
			{
				fleeLoongs = this._fleeLoongs;
			}
			else
			{
				List<Location> value = new List<Location>();
				this.CalcFleeLoongs(value);
				bool lockTaken = false;
				try
				{
					this._spinLockFleeLoongs.Enter(ref lockTaken);
					this._fleeLoongs.Assign(value);
					BaseGameDataDomain.SetCached(this.DataStates, 63);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLockFleeLoongs.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				fleeLoongs = this._fleeLoongs;
			}
			return fleeLoongs;
		}

		// Token: 0x06007B91 RID: 31633 RVA: 0x00483984 File Offset: 0x00481B84
		public List<LoongLocationData> GetLoongLocations()
		{
			Thread.MemoryBarrier();
			bool flag = BaseGameDataDomain.IsCached(this.DataStates, 64);
			List<LoongLocationData> loongLocations;
			if (flag)
			{
				loongLocations = this._loongLocations;
			}
			else
			{
				List<LoongLocationData> value = new List<LoongLocationData>();
				this.CalcLoongLocations(value);
				bool lockTaken = false;
				try
				{
					this._spinLockLoongLocations.Enter(ref lockTaken);
					this._loongLocations.Assign(value);
					BaseGameDataDomain.SetCached(this.DataStates, 64);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLockLoongLocations.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				loongLocations = this._loongLocations;
			}
			return loongLocations;
		}

		// Token: 0x06007B92 RID: 31634 RVA: 0x00483A28 File Offset: 0x00481C28
		public List<Location> GetAlterSettlementLocations()
		{
			Thread.MemoryBarrier();
			bool flag = BaseGameDataDomain.IsCached(this.DataStates, 65);
			List<Location> alterSettlementLocations;
			if (flag)
			{
				alterSettlementLocations = this._alterSettlementLocations;
			}
			else
			{
				List<Location> value = new List<Location>();
				this.CalcAlterSettlementLocations(value);
				bool lockTaken = false;
				try
				{
					this._spinLockAlterSettlementLocations.Enter(ref lockTaken);
					this._alterSettlementLocations.Assign(value);
					BaseGameDataDomain.SetCached(this.DataStates, 65);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLockAlterSettlementLocations.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				alterSettlementLocations = this._alterSettlementLocations;
			}
			return alterSettlementLocations;
		}

		// Token: 0x06007B93 RID: 31635 RVA: 0x00483ACC File Offset: 0x00481CCC
		public bool GetIsTaiwuInFulongFlameArea()
		{
			return this._isTaiwuInFulongFlameArea;
		}

		// Token: 0x06007B94 RID: 31636 RVA: 0x00483AE4 File Offset: 0x00481CE4
		public void SetIsTaiwuInFulongFlameArea(bool value, DataContext context)
		{
			this._isTaiwuInFulongFlameArea = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(66, this.DataStates, MapDomain.CacheInfluences, context);
		}

		// Token: 0x06007B95 RID: 31637 RVA: 0x00483B04 File Offset: 0x00481D04
		public List<MapElementPickupDisplayData> GetVisibleMapPickups()
		{
			Thread.MemoryBarrier();
			bool flag = BaseGameDataDomain.IsCached(this.DataStates, 67);
			List<MapElementPickupDisplayData> visibleMapPickups;
			if (flag)
			{
				visibleMapPickups = this._visibleMapPickups;
			}
			else
			{
				List<MapElementPickupDisplayData> value = new List<MapElementPickupDisplayData>();
				this.CalcVisibleMapPickups(value);
				bool lockTaken = false;
				try
				{
					this._spinLockVisibleMapPickups.Enter(ref lockTaken);
					this._visibleMapPickups.Assign(value);
					BaseGameDataDomain.SetCached(this.DataStates, 67);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLockVisibleMapPickups.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				visibleMapPickups = this._visibleMapPickups;
			}
			return visibleMapPickups;
		}

		// Token: 0x06007B96 RID: 31638 RVA: 0x00483BA8 File Offset: 0x00481DA8
		public override void OnInitializeGameDataModule()
		{
			this.InitializeOnInitializeGameDataModule();
		}

		// Token: 0x06007B97 RID: 31639 RVA: 0x00483BB4 File Offset: 0x00481DB4
		public unsafe override void OnEnterNewWorld()
		{
			this.InitializeOnEnterNewWorld();
			this.InitializeInternalDataOfCollections();
			int dataSize = 0;
			for (int i = 0; i < 139; i++)
			{
				MapAreaData element = this._areas[i];
				bool flag = element != null;
				if (flag)
				{
					dataSize += 4 + element.GetSerializedSize();
				}
				else
				{
					dataSize += 4;
				}
			}
			byte* pData = OperationAdder.DynamicElementList_InsertRange(2, 0, 0, 139, dataSize);
			for (int j = 0; j < 139; j++)
			{
				MapAreaData element2 = this._areas[j];
				bool flag2 = element2 != null;
				if (flag2)
				{
					byte* pSubContentSize = pData;
					pData += 4;
					int subContentSize = element2.Serialize(pData);
					pData += subContentSize;
					*(int*)pSubContentSize = subContentSize;
				}
				else
				{
					*(int*)pData = 0;
					pData += 4;
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry in this._areaBlocks0)
			{
				short elementId = entry.Key;
				MapBlockData value = entry.Value;
				bool flag3 = value != null;
				if (flag3)
				{
					int contentSize = value.GetSerializedSize();
					byte* pData2 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 1, elementId, contentSize);
					pData2 += value.Serialize(pData2);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 1, elementId, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry2 in this._areaBlocks1)
			{
				short elementId2 = entry2.Key;
				MapBlockData value2 = entry2.Value;
				bool flag4 = value2 != null;
				if (flag4)
				{
					int contentSize2 = value2.GetSerializedSize();
					byte* pData3 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 2, elementId2, contentSize2);
					pData3 += value2.Serialize(pData3);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 2, elementId2, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry3 in this._areaBlocks2)
			{
				short elementId3 = entry3.Key;
				MapBlockData value3 = entry3.Value;
				bool flag5 = value3 != null;
				if (flag5)
				{
					int contentSize3 = value3.GetSerializedSize();
					byte* pData4 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 3, elementId3, contentSize3);
					pData4 += value3.Serialize(pData4);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 3, elementId3, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry4 in this._areaBlocks3)
			{
				short elementId4 = entry4.Key;
				MapBlockData value4 = entry4.Value;
				bool flag6 = value4 != null;
				if (flag6)
				{
					int contentSize4 = value4.GetSerializedSize();
					byte* pData5 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 4, elementId4, contentSize4);
					pData5 += value4.Serialize(pData5);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 4, elementId4, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry5 in this._areaBlocks4)
			{
				short elementId5 = entry5.Key;
				MapBlockData value5 = entry5.Value;
				bool flag7 = value5 != null;
				if (flag7)
				{
					int contentSize5 = value5.GetSerializedSize();
					byte* pData6 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 5, elementId5, contentSize5);
					pData6 += value5.Serialize(pData6);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 5, elementId5, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry6 in this._areaBlocks5)
			{
				short elementId6 = entry6.Key;
				MapBlockData value6 = entry6.Value;
				bool flag8 = value6 != null;
				if (flag8)
				{
					int contentSize6 = value6.GetSerializedSize();
					byte* pData7 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 6, elementId6, contentSize6);
					pData7 += value6.Serialize(pData7);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 6, elementId6, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry7 in this._areaBlocks6)
			{
				short elementId7 = entry7.Key;
				MapBlockData value7 = entry7.Value;
				bool flag9 = value7 != null;
				if (flag9)
				{
					int contentSize7 = value7.GetSerializedSize();
					byte* pData8 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 7, elementId7, contentSize7);
					pData8 += value7.Serialize(pData8);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 7, elementId7, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry8 in this._areaBlocks7)
			{
				short elementId8 = entry8.Key;
				MapBlockData value8 = entry8.Value;
				bool flag10 = value8 != null;
				if (flag10)
				{
					int contentSize8 = value8.GetSerializedSize();
					byte* pData9 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 8, elementId8, contentSize8);
					pData9 += value8.Serialize(pData9);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 8, elementId8, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry9 in this._areaBlocks8)
			{
				short elementId9 = entry9.Key;
				MapBlockData value9 = entry9.Value;
				bool flag11 = value9 != null;
				if (flag11)
				{
					int contentSize9 = value9.GetSerializedSize();
					byte* pData10 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 9, elementId9, contentSize9);
					pData10 += value9.Serialize(pData10);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 9, elementId9, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry10 in this._areaBlocks9)
			{
				short elementId10 = entry10.Key;
				MapBlockData value10 = entry10.Value;
				bool flag12 = value10 != null;
				if (flag12)
				{
					int contentSize10 = value10.GetSerializedSize();
					byte* pData11 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 10, elementId10, contentSize10);
					pData11 += value10.Serialize(pData11);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 10, elementId10, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry11 in this._areaBlocks10)
			{
				short elementId11 = entry11.Key;
				MapBlockData value11 = entry11.Value;
				bool flag13 = value11 != null;
				if (flag13)
				{
					int contentSize11 = value11.GetSerializedSize();
					byte* pData12 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 11, elementId11, contentSize11);
					pData12 += value11.Serialize(pData12);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 11, elementId11, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry12 in this._areaBlocks11)
			{
				short elementId12 = entry12.Key;
				MapBlockData value12 = entry12.Value;
				bool flag14 = value12 != null;
				if (flag14)
				{
					int contentSize12 = value12.GetSerializedSize();
					byte* pData13 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 12, elementId12, contentSize12);
					pData13 += value12.Serialize(pData13);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 12, elementId12, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry13 in this._areaBlocks12)
			{
				short elementId13 = entry13.Key;
				MapBlockData value13 = entry13.Value;
				bool flag15 = value13 != null;
				if (flag15)
				{
					int contentSize13 = value13.GetSerializedSize();
					byte* pData14 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 13, elementId13, contentSize13);
					pData14 += value13.Serialize(pData14);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 13, elementId13, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry14 in this._areaBlocks13)
			{
				short elementId14 = entry14.Key;
				MapBlockData value14 = entry14.Value;
				bool flag16 = value14 != null;
				if (flag16)
				{
					int contentSize14 = value14.GetSerializedSize();
					byte* pData15 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 14, elementId14, contentSize14);
					pData15 += value14.Serialize(pData15);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 14, elementId14, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry15 in this._areaBlocks14)
			{
				short elementId15 = entry15.Key;
				MapBlockData value15 = entry15.Value;
				bool flag17 = value15 != null;
				if (flag17)
				{
					int contentSize15 = value15.GetSerializedSize();
					byte* pData16 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 15, elementId15, contentSize15);
					pData16 += value15.Serialize(pData16);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 15, elementId15, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry16 in this._areaBlocks15)
			{
				short elementId16 = entry16.Key;
				MapBlockData value16 = entry16.Value;
				bool flag18 = value16 != null;
				if (flag18)
				{
					int contentSize16 = value16.GetSerializedSize();
					byte* pData17 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 16, elementId16, contentSize16);
					pData17 += value16.Serialize(pData17);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 16, elementId16, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry17 in this._areaBlocks16)
			{
				short elementId17 = entry17.Key;
				MapBlockData value17 = entry17.Value;
				bool flag19 = value17 != null;
				if (flag19)
				{
					int contentSize17 = value17.GetSerializedSize();
					byte* pData18 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 17, elementId17, contentSize17);
					pData18 += value17.Serialize(pData18);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 17, elementId17, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry18 in this._areaBlocks17)
			{
				short elementId18 = entry18.Key;
				MapBlockData value18 = entry18.Value;
				bool flag20 = value18 != null;
				if (flag20)
				{
					int contentSize18 = value18.GetSerializedSize();
					byte* pData19 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 18, elementId18, contentSize18);
					pData19 += value18.Serialize(pData19);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 18, elementId18, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry19 in this._areaBlocks18)
			{
				short elementId19 = entry19.Key;
				MapBlockData value19 = entry19.Value;
				bool flag21 = value19 != null;
				if (flag21)
				{
					int contentSize19 = value19.GetSerializedSize();
					byte* pData20 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 19, elementId19, contentSize19);
					pData20 += value19.Serialize(pData20);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 19, elementId19, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry20 in this._areaBlocks19)
			{
				short elementId20 = entry20.Key;
				MapBlockData value20 = entry20.Value;
				bool flag22 = value20 != null;
				if (flag22)
				{
					int contentSize20 = value20.GetSerializedSize();
					byte* pData21 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 20, elementId20, contentSize20);
					pData21 += value20.Serialize(pData21);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 20, elementId20, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry21 in this._areaBlocks20)
			{
				short elementId21 = entry21.Key;
				MapBlockData value21 = entry21.Value;
				bool flag23 = value21 != null;
				if (flag23)
				{
					int contentSize21 = value21.GetSerializedSize();
					byte* pData22 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 21, elementId21, contentSize21);
					pData22 += value21.Serialize(pData22);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 21, elementId21, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry22 in this._areaBlocks21)
			{
				short elementId22 = entry22.Key;
				MapBlockData value22 = entry22.Value;
				bool flag24 = value22 != null;
				if (flag24)
				{
					int contentSize22 = value22.GetSerializedSize();
					byte* pData23 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 22, elementId22, contentSize22);
					pData23 += value22.Serialize(pData23);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 22, elementId22, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry23 in this._areaBlocks22)
			{
				short elementId23 = entry23.Key;
				MapBlockData value23 = entry23.Value;
				bool flag25 = value23 != null;
				if (flag25)
				{
					int contentSize23 = value23.GetSerializedSize();
					byte* pData24 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 23, elementId23, contentSize23);
					pData24 += value23.Serialize(pData24);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 23, elementId23, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry24 in this._areaBlocks23)
			{
				short elementId24 = entry24.Key;
				MapBlockData value24 = entry24.Value;
				bool flag26 = value24 != null;
				if (flag26)
				{
					int contentSize24 = value24.GetSerializedSize();
					byte* pData25 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 24, elementId24, contentSize24);
					pData25 += value24.Serialize(pData25);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 24, elementId24, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry25 in this._areaBlocks24)
			{
				short elementId25 = entry25.Key;
				MapBlockData value25 = entry25.Value;
				bool flag27 = value25 != null;
				if (flag27)
				{
					int contentSize25 = value25.GetSerializedSize();
					byte* pData26 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 25, elementId25, contentSize25);
					pData26 += value25.Serialize(pData26);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 25, elementId25, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry26 in this._areaBlocks25)
			{
				short elementId26 = entry26.Key;
				MapBlockData value26 = entry26.Value;
				bool flag28 = value26 != null;
				if (flag28)
				{
					int contentSize26 = value26.GetSerializedSize();
					byte* pData27 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 26, elementId26, contentSize26);
					pData27 += value26.Serialize(pData27);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 26, elementId26, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry27 in this._areaBlocks26)
			{
				short elementId27 = entry27.Key;
				MapBlockData value27 = entry27.Value;
				bool flag29 = value27 != null;
				if (flag29)
				{
					int contentSize27 = value27.GetSerializedSize();
					byte* pData28 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 27, elementId27, contentSize27);
					pData28 += value27.Serialize(pData28);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 27, elementId27, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry28 in this._areaBlocks27)
			{
				short elementId28 = entry28.Key;
				MapBlockData value28 = entry28.Value;
				bool flag30 = value28 != null;
				if (flag30)
				{
					int contentSize28 = value28.GetSerializedSize();
					byte* pData29 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 28, elementId28, contentSize28);
					pData29 += value28.Serialize(pData29);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 28, elementId28, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry29 in this._areaBlocks28)
			{
				short elementId29 = entry29.Key;
				MapBlockData value29 = entry29.Value;
				bool flag31 = value29 != null;
				if (flag31)
				{
					int contentSize29 = value29.GetSerializedSize();
					byte* pData30 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 29, elementId29, contentSize29);
					pData30 += value29.Serialize(pData30);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 29, elementId29, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry30 in this._areaBlocks29)
			{
				short elementId30 = entry30.Key;
				MapBlockData value30 = entry30.Value;
				bool flag32 = value30 != null;
				if (flag32)
				{
					int contentSize30 = value30.GetSerializedSize();
					byte* pData31 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 30, elementId30, contentSize30);
					pData31 += value30.Serialize(pData31);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 30, elementId30, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry31 in this._areaBlocks30)
			{
				short elementId31 = entry31.Key;
				MapBlockData value31 = entry31.Value;
				bool flag33 = value31 != null;
				if (flag33)
				{
					int contentSize31 = value31.GetSerializedSize();
					byte* pData32 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 31, elementId31, contentSize31);
					pData32 += value31.Serialize(pData32);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 31, elementId31, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry32 in this._areaBlocks31)
			{
				short elementId32 = entry32.Key;
				MapBlockData value32 = entry32.Value;
				bool flag34 = value32 != null;
				if (flag34)
				{
					int contentSize32 = value32.GetSerializedSize();
					byte* pData33 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 32, elementId32, contentSize32);
					pData33 += value32.Serialize(pData33);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 32, elementId32, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry33 in this._areaBlocks32)
			{
				short elementId33 = entry33.Key;
				MapBlockData value33 = entry33.Value;
				bool flag35 = value33 != null;
				if (flag35)
				{
					int contentSize33 = value33.GetSerializedSize();
					byte* pData34 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 33, elementId33, contentSize33);
					pData34 += value33.Serialize(pData34);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 33, elementId33, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry34 in this._areaBlocks33)
			{
				short elementId34 = entry34.Key;
				MapBlockData value34 = entry34.Value;
				bool flag36 = value34 != null;
				if (flag36)
				{
					int contentSize34 = value34.GetSerializedSize();
					byte* pData35 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 34, elementId34, contentSize34);
					pData35 += value34.Serialize(pData35);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 34, elementId34, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry35 in this._areaBlocks34)
			{
				short elementId35 = entry35.Key;
				MapBlockData value35 = entry35.Value;
				bool flag37 = value35 != null;
				if (flag37)
				{
					int contentSize35 = value35.GetSerializedSize();
					byte* pData36 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 35, elementId35, contentSize35);
					pData36 += value35.Serialize(pData36);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 35, elementId35, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry36 in this._areaBlocks35)
			{
				short elementId36 = entry36.Key;
				MapBlockData value36 = entry36.Value;
				bool flag38 = value36 != null;
				if (flag38)
				{
					int contentSize36 = value36.GetSerializedSize();
					byte* pData37 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 36, elementId36, contentSize36);
					pData37 += value36.Serialize(pData37);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 36, elementId36, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry37 in this._areaBlocks36)
			{
				short elementId37 = entry37.Key;
				MapBlockData value37 = entry37.Value;
				bool flag39 = value37 != null;
				if (flag39)
				{
					int contentSize37 = value37.GetSerializedSize();
					byte* pData38 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 37, elementId37, contentSize37);
					pData38 += value37.Serialize(pData38);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 37, elementId37, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry38 in this._areaBlocks37)
			{
				short elementId38 = entry38.Key;
				MapBlockData value38 = entry38.Value;
				bool flag40 = value38 != null;
				if (flag40)
				{
					int contentSize38 = value38.GetSerializedSize();
					byte* pData39 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 38, elementId38, contentSize38);
					pData39 += value38.Serialize(pData39);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 38, elementId38, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry39 in this._areaBlocks38)
			{
				short elementId39 = entry39.Key;
				MapBlockData value39 = entry39.Value;
				bool flag41 = value39 != null;
				if (flag41)
				{
					int contentSize39 = value39.GetSerializedSize();
					byte* pData40 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 39, elementId39, contentSize39);
					pData40 += value39.Serialize(pData40);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 39, elementId39, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry40 in this._areaBlocks39)
			{
				short elementId40 = entry40.Key;
				MapBlockData value40 = entry40.Value;
				bool flag42 = value40 != null;
				if (flag42)
				{
					int contentSize40 = value40.GetSerializedSize();
					byte* pData41 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 40, elementId40, contentSize40);
					pData41 += value40.Serialize(pData41);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 40, elementId40, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry41 in this._areaBlocks40)
			{
				short elementId41 = entry41.Key;
				MapBlockData value41 = entry41.Value;
				bool flag43 = value41 != null;
				if (flag43)
				{
					int contentSize41 = value41.GetSerializedSize();
					byte* pData42 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 41, elementId41, contentSize41);
					pData42 += value41.Serialize(pData42);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 41, elementId41, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry42 in this._areaBlocks41)
			{
				short elementId42 = entry42.Key;
				MapBlockData value42 = entry42.Value;
				bool flag44 = value42 != null;
				if (flag44)
				{
					int contentSize42 = value42.GetSerializedSize();
					byte* pData43 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 42, elementId42, contentSize42);
					pData43 += value42.Serialize(pData43);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 42, elementId42, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry43 in this._areaBlocks42)
			{
				short elementId43 = entry43.Key;
				MapBlockData value43 = entry43.Value;
				bool flag45 = value43 != null;
				if (flag45)
				{
					int contentSize43 = value43.GetSerializedSize();
					byte* pData44 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 43, elementId43, contentSize43);
					pData44 += value43.Serialize(pData44);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 43, elementId43, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry44 in this._areaBlocks43)
			{
				short elementId44 = entry44.Key;
				MapBlockData value44 = entry44.Value;
				bool flag46 = value44 != null;
				if (flag46)
				{
					int contentSize44 = value44.GetSerializedSize();
					byte* pData45 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 44, elementId44, contentSize44);
					pData45 += value44.Serialize(pData45);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 44, elementId44, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry45 in this._areaBlocks44)
			{
				short elementId45 = entry45.Key;
				MapBlockData value45 = entry45.Value;
				bool flag47 = value45 != null;
				if (flag47)
				{
					int contentSize45 = value45.GetSerializedSize();
					byte* pData46 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 45, elementId45, contentSize45);
					pData46 += value45.Serialize(pData46);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 45, elementId45, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry46 in this._brokenAreaBlocks)
			{
				short elementId46 = entry46.Key;
				MapBlockData value46 = entry46.Value;
				bool flag48 = value46 != null;
				if (flag48)
				{
					int contentSize46 = value46.GetSerializedSize();
					byte* pData47 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 46, elementId46, contentSize46);
					pData47 += value46.Serialize(pData47);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 46, elementId46, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry47 in this._bornAreaBlocks)
			{
				short elementId47 = entry47.Key;
				MapBlockData value47 = entry47.Value;
				bool flag49 = value47 != null;
				if (flag49)
				{
					int contentSize47 = value47.GetSerializedSize();
					byte* pData48 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 47, elementId47, contentSize47);
					pData48 += value47.Serialize(pData48);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 47, elementId47, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry48 in this._guideAreaBlocks)
			{
				short elementId48 = entry48.Key;
				MapBlockData value48 = entry48.Value;
				bool flag50 = value48 != null;
				if (flag50)
				{
					int contentSize48 = value48.GetSerializedSize();
					byte* pData49 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 48, elementId48, contentSize48);
					pData49 += value48.Serialize(pData49);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 48, elementId48, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry49 in this._secretVillageAreaBlocks)
			{
				short elementId49 = entry49.Key;
				MapBlockData value49 = entry49.Value;
				bool flag51 = value49 != null;
				if (flag51)
				{
					int contentSize49 = value49.GetSerializedSize();
					byte* pData50 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 49, elementId49, contentSize49);
					pData50 += value49.Serialize(pData50);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 49, elementId49, 0);
				}
			}
			foreach (KeyValuePair<short, MapBlockData> entry50 in this._brokenPerformAreaBlocks)
			{
				short elementId50 = entry50.Key;
				MapBlockData value50 = entry50.Value;
				bool flag52 = value50 != null;
				if (flag52)
				{
					int contentSize50 = value50.GetSerializedSize();
					byte* pData51 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 50, elementId50, contentSize50);
					pData51 += value50.Serialize(pData51);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<short>(2, 50, elementId50, 0);
				}
			}
			foreach (KeyValuePair<TravelRouteKey, TravelRoute> entry51 in this._travelRouteDict)
			{
				TravelRouteKey elementId51 = entry51.Key;
				TravelRoute value51 = entry51.Value;
				bool flag53 = value51 != null;
				if (flag53)
				{
					int contentSize51 = value51.GetSerializedSize();
					byte* pData52 = OperationAdder.DynamicSingleValueCollection_Add<TravelRouteKey>(2, 51, elementId51, contentSize51);
					pData52 += value51.Serialize(pData52);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<TravelRouteKey>(2, 51, elementId51, 0);
				}
			}
			foreach (KeyValuePair<TravelRouteKey, TravelRoute> entry52 in this._bornStateTravelRouteDict)
			{
				TravelRouteKey elementId52 = entry52.Key;
				TravelRoute value52 = entry52.Value;
				bool flag54 = value52 != null;
				if (flag54)
				{
					int contentSize52 = value52.GetSerializedSize();
					byte* pData53 = OperationAdder.DynamicSingleValueCollection_Add<TravelRouteKey>(2, 52, elementId52, contentSize52);
					pData53 += value52.Serialize(pData53);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<TravelRouteKey>(2, 52, elementId52, 0);
				}
			}
			int dataSize2 = 0;
			for (int k = 0; k < 139; k++)
			{
				AnimalPlaceData element3 = this._animalPlaceData[k];
				bool flag55 = element3 != null;
				if (flag55)
				{
					dataSize2 += 4 + element3.GetSerializedSize();
				}
				else
				{
					dataSize2 += 4;
				}
			}
			byte* pData54 = OperationAdder.DynamicElementList_InsertRange(2, 53, 0, 139, dataSize2);
			for (int l = 0; l < 139; l++)
			{
				AnimalPlaceData element4 = this._animalPlaceData[l];
				bool flag56 = element4 != null;
				if (flag56)
				{
					byte* pSubContentSize2 = pData54;
					pData54 += 4;
					int subContentSize2 = element4.Serialize(pData54);
					pData54 += subContentSize2;
					*(int*)pSubContentSize2 = subContentSize2;
				}
				else
				{
					*(int*)pData54 = 0;
					pData54 += 4;
				}
			}
			int dataSize3 = 0;
			for (int m = 0; m < 139; m++)
			{
				CricketPlaceData element5 = this._cricketPlaceData[m];
				bool flag57 = element5 != null;
				if (flag57)
				{
					dataSize3 += 4 + element5.GetSerializedSize();
				}
				else
				{
					dataSize3 += 4;
				}
			}
			byte* pData55 = OperationAdder.DynamicElementList_InsertRange(2, 54, 0, 139, dataSize3);
			for (int n = 0; n < 139; n++)
			{
				CricketPlaceData element6 = this._cricketPlaceData[n];
				bool flag58 = element6 != null;
				if (flag58)
				{
					byte* pSubContentSize3 = pData55;
					pData55 += 4;
					int subContentSize3 = element6.Serialize(pData55);
					pData55 += subContentSize3;
					*(int*)pSubContentSize3 = subContentSize3;
				}
				else
				{
					*(int*)pData55 = 0;
					pData55 += 4;
				}
			}
			foreach (KeyValuePair<short, GameData.Utilities.ShortList> entry53 in this._regularAreaNearList)
			{
				short elementId53 = entry53.Key;
				GameData.Utilities.ShortList value53 = entry53.Value;
				int dataSize4 = value53.GetSerializedSize();
				byte* pData56 = OperationAdder.DynamicSingleValueCollection_Add<short>(2, 55, elementId53, dataSize4);
				pData56 += value53.Serialize(pData56);
			}
			byte* pData57 = OperationAdder.FixedElementList_InsertRange(2, 56, 0, 8, 32);
			for (int i2 = 0; i2 < 8; i2++)
			{
				pData57 += this._swordTombLocations[i2].Serialize(pData57);
			}
			int dataSize5 = this._travelInfo.GetSerializedSize();
			byte* pData58 = OperationAdder.DynamicSingleValue_Set(2, 57, dataSize5);
			pData58 += this._travelInfo.Serialize(pData58);
		}

		// Token: 0x06007B98 RID: 31640 RVA: 0x00486788 File Offset: 0x00484988
		public override void OnLoadWorld()
		{
			this._pendingLoadingOperationIds = new Queue<uint>();
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicElementList_GetAll(2, 0));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 1));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 2));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 3));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 4));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 5));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 6));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 7));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 8));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 9));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 10));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 11));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 12));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 13));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 14));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 15));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 16));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 17));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 18));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 19));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 20));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 21));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 22));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 23));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 24));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 25));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 26));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 27));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 28));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 29));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 30));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 31));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 32));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 33));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 34));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 35));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 36));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 37));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 38));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 39));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 40));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 41));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 42));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 43));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 44));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 45));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 46));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 47));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 48));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 49));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 50));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 51));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 52));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicElementList_GetAll(2, 53));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicElementList_GetAll(2, 54));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 55));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedElementList_GetAll(2, 56));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValue_Get(2, 57));
		}

		// Token: 0x06007B99 RID: 31641 RVA: 0x00486C20 File Offset: 0x00484E20
		public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
		{
			int result;
			switch (dataId)
			{
			case 0:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this._dataStatesAreas, (int)subId0);
				}
				result = Serializer.Serialize(this._areas[(int)subId0], dataPool);
				break;
			case 1:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 1);
					this._modificationsAreaBlocks0.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks0, dataPool);
				break;
			case 2:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 2);
					this._modificationsAreaBlocks1.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks1, dataPool);
				break;
			case 3:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 3);
					this._modificationsAreaBlocks2.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks2, dataPool);
				break;
			case 4:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 4);
					this._modificationsAreaBlocks3.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks3, dataPool);
				break;
			case 5:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 5);
					this._modificationsAreaBlocks4.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks4, dataPool);
				break;
			case 6:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 6);
					this._modificationsAreaBlocks5.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks5, dataPool);
				break;
			case 7:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 7);
					this._modificationsAreaBlocks6.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks6, dataPool);
				break;
			case 8:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 8);
					this._modificationsAreaBlocks7.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks7, dataPool);
				break;
			case 9:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 9);
					this._modificationsAreaBlocks8.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks8, dataPool);
				break;
			case 10:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 10);
					this._modificationsAreaBlocks9.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks9, dataPool);
				break;
			case 11:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 11);
					this._modificationsAreaBlocks10.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks10, dataPool);
				break;
			case 12:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 12);
					this._modificationsAreaBlocks11.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks11, dataPool);
				break;
			case 13:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 13);
					this._modificationsAreaBlocks12.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks12, dataPool);
				break;
			case 14:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 14);
					this._modificationsAreaBlocks13.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks13, dataPool);
				break;
			case 15:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 15);
					this._modificationsAreaBlocks14.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks14, dataPool);
				break;
			case 16:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 16);
					this._modificationsAreaBlocks15.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks15, dataPool);
				break;
			case 17:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 17);
					this._modificationsAreaBlocks16.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks16, dataPool);
				break;
			case 18:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 18);
					this._modificationsAreaBlocks17.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks17, dataPool);
				break;
			case 19:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 19);
					this._modificationsAreaBlocks18.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks18, dataPool);
				break;
			case 20:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 20);
					this._modificationsAreaBlocks19.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks19, dataPool);
				break;
			case 21:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 21);
					this._modificationsAreaBlocks20.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks20, dataPool);
				break;
			case 22:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 22);
					this._modificationsAreaBlocks21.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks21, dataPool);
				break;
			case 23:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 23);
					this._modificationsAreaBlocks22.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks22, dataPool);
				break;
			case 24:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 24);
					this._modificationsAreaBlocks23.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks23, dataPool);
				break;
			case 25:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 25);
					this._modificationsAreaBlocks24.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks24, dataPool);
				break;
			case 26:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 26);
					this._modificationsAreaBlocks25.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks25, dataPool);
				break;
			case 27:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 27);
					this._modificationsAreaBlocks26.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks26, dataPool);
				break;
			case 28:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 28);
					this._modificationsAreaBlocks27.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks27, dataPool);
				break;
			case 29:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 29);
					this._modificationsAreaBlocks28.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks28, dataPool);
				break;
			case 30:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 30);
					this._modificationsAreaBlocks29.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks29, dataPool);
				break;
			case 31:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 31);
					this._modificationsAreaBlocks30.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks30, dataPool);
				break;
			case 32:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 32);
					this._modificationsAreaBlocks31.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks31, dataPool);
				break;
			case 33:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 33);
					this._modificationsAreaBlocks32.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks32, dataPool);
				break;
			case 34:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 34);
					this._modificationsAreaBlocks33.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks33, dataPool);
				break;
			case 35:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 35);
					this._modificationsAreaBlocks34.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks34, dataPool);
				break;
			case 36:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 36);
					this._modificationsAreaBlocks35.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks35, dataPool);
				break;
			case 37:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 37);
					this._modificationsAreaBlocks36.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks36, dataPool);
				break;
			case 38:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 38);
					this._modificationsAreaBlocks37.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks37, dataPool);
				break;
			case 39:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 39);
					this._modificationsAreaBlocks38.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks38, dataPool);
				break;
			case 40:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 40);
					this._modificationsAreaBlocks39.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks39, dataPool);
				break;
			case 41:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 41);
					this._modificationsAreaBlocks40.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks40, dataPool);
				break;
			case 42:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 42);
					this._modificationsAreaBlocks41.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks41, dataPool);
				break;
			case 43:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 43);
					this._modificationsAreaBlocks42.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks42, dataPool);
				break;
			case 44:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 44);
					this._modificationsAreaBlocks43.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks43, dataPool);
				break;
			case 45:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 45);
					this._modificationsAreaBlocks44.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._areaBlocks44, dataPool);
				break;
			case 46:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 46);
					this._modificationsBrokenAreaBlocks.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._brokenAreaBlocks, dataPool);
				break;
			case 47:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 47);
					this._modificationsBornAreaBlocks.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._bornAreaBlocks, dataPool);
				break;
			case 48:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 48);
					this._modificationsGuideAreaBlocks.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._guideAreaBlocks, dataPool);
				break;
			case 49:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 49);
					this._modificationsSecretVillageAreaBlocks.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._secretVillageAreaBlocks, dataPool);
				break;
			case 50:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 50);
					this._modificationsBrokenPerformAreaBlocks.Reset();
				}
				result = Serializer.SerializeModifications<short>(this._brokenPerformAreaBlocks, dataPool);
				break;
			case 51:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 51);
					this._modificationsTravelRouteDict.Reset();
				}
				result = Serializer.SerializeModifications<TravelRouteKey>(this._travelRouteDict, dataPool);
				break;
			case 52:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 52);
					this._modificationsBornStateTravelRouteDict.Reset();
				}
				result = Serializer.SerializeModifications<TravelRouteKey>(this._bornStateTravelRouteDict, dataPool);
				break;
			case 53:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this._dataStatesAnimalPlaceData, (int)subId0);
				}
				result = Serializer.Serialize(this._animalPlaceData[(int)subId0], dataPool);
				break;
			case 54:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this._dataStatesCricketPlaceData, (int)subId0);
				}
				result = Serializer.Serialize(this._cricketPlaceData[(int)subId0], dataPool);
				break;
			case 55:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to get value of dataId: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 56:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this._dataStatesSwordTombLocations, (int)subId0);
				}
				result = Serializer.Serialize(this._swordTombLocations[(int)subId0], dataPool);
				break;
			case 57:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 57);
				}
				result = Serializer.Serialize(this._travelInfo, dataPool);
				break;
			case 58:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 58);
				}
				result = Serializer.Serialize(this._onHandlingTravelingEventBlock, dataPool);
				break;
			case 59:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 59);
				}
				result = Serializer.Serialize(this._hunterAnimalsCache, dataPool);
				break;
			case 60:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 60);
				}
				result = Serializer.Serialize(this._moveBanned, dataPool);
				break;
			case 61:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 61);
				}
				result = Serializer.Serialize(this._crossArchiveLockMoveTime, dataPool);
				break;
			case 62:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 62);
				}
				result = Serializer.Serialize(this.GetFleeBeasts(), dataPool);
				break;
			case 63:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 63);
				}
				result = Serializer.Serialize(this.GetFleeLoongs(), dataPool);
				break;
			case 64:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 64);
				}
				result = Serializer.Serialize(this.GetLoongLocations(), dataPool);
				break;
			case 65:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 65);
				}
				result = Serializer.Serialize(this.GetAlterSettlementLocations(), dataPool);
				break;
			case 66:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 66);
				}
				result = Serializer.Serialize(this._isTaiwuInFulongFlameArea, dataPool);
				break;
			case 67:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 67);
				}
				result = Serializer.Serialize(this.GetVisibleMapPickups(), dataPool);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06007B9A RID: 31642 RVA: 0x00487B38 File Offset: 0x00485D38
		public override void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			switch (dataId)
			{
			case 0:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 1:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 3:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 4:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 5:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 6:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 7:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 8:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 9:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 10:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 11:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 12:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 13:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 14:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 15:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 16:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 17:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 18:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 19:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 20:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 21:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 22:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 23:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 24:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 25:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 26:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 27:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 28:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 29:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 30:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 31:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 32:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 33:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 34:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 35:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 36:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 37:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 38:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 39:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 40:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 41:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 42:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 43:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 44:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 45:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 46:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 47:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 48:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 49:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 50:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 51:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 52:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 53:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 54:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 55:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 56:
			{
				Location value = default(Location);
				Serializer.Deserialize(dataPool, valueOffset, ref value);
				this._swordTombLocations[(int)subId0] = value;
				this.SetElement_SwordTombLocations((int)subId0, value, context);
				break;
			}
			case 57:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 58:
				Serializer.Deserialize(dataPool, valueOffset, ref this._onHandlingTravelingEventBlock);
				this.SetOnHandlingTravelingEventBlock(this._onHandlingTravelingEventBlock, context);
				break;
			case 59:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 60:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 61:
				Serializer.Deserialize(dataPool, valueOffset, ref this._crossArchiveLockMoveTime);
				this.SetCrossArchiveLockMoveTime(this._crossArchiveLockMoveTime, context);
				break;
			case 62:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 63:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 64:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 65:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 66:
				Serializer.Deserialize(dataPool, valueOffset, ref this._isTaiwuInFulongFlameArea);
				this.SetIsTaiwuInFulongFlameArea(this._isTaiwuInFulongFlameArea, context);
				break;
			case 67:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06007B9B RID: 31643 RVA: 0x00488874 File Offset: 0x00486A74
		public override int CallMethod(Operation operation, RawDataPool argDataPool, RawDataPool returnDataPool, DataContext context)
		{
			int argsOffset = operation.ArgsOffset;
			int result;
			switch (operation.MethodId)
			{
			case 0:
			{
				int argsCount = operation.ArgsCount;
				int num = argsCount;
				if (num != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool isLock = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isLock);
				this.GmCmd_SetLockTime(isLock);
				result = -1;
				break;
			}
			case 1:
			{
				int argsCount2 = operation.ArgsCount;
				int num2 = argsCount2;
				if (num2 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool teleportOn = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref teleportOn);
				this.GmCmd_SetTeleportMove(teleportOn);
				result = -1;
				break;
			}
			case 2:
			{
				int argsCount3 = operation.ArgsCount;
				int num3 = argsCount3;
				if (num3 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.GmCmd_ShowAllMapBlock(context);
				result = -1;
				break;
			}
			case 3:
			{
				int argsCount4 = operation.ArgsCount;
				int num4 = argsCount4;
				if (num4 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.GmCmd_UnlockAllStation(context);
				result = -1;
				break;
			}
			case 4:
			{
				int argsCount5 = operation.ArgsCount;
				int num5 = argsCount5;
				if (num5 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short areaId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref areaId);
				int spiritualDebt = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref spiritualDebt);
				this.GmCmd_ChangeSpiritualDebt(context, areaId, spiritualDebt);
				result = -1;
				break;
			}
			case 5:
			{
				int argsCount6 = operation.ArgsCount;
				int num6 = argsCount6;
				if (num6 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				MapBlockData mapBlockData = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref mapBlockData);
				this.GmCmd_SetMapBlockData(context, mapBlockData);
				result = -1;
				break;
			}
			case 6:
			{
				int argsCount7 = operation.ArgsCount;
				int num7 = argsCount7;
				if (num7 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short templateId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref templateId);
				this.GmCmd_CreateFixedCharacterAtCurrentBlock(context, templateId);
				result = -1;
				break;
			}
			case 7:
			{
				int argsCount8 = operation.ArgsCount;
				int num8 = argsCount8;
				if (num8 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short destBlockId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref destBlockId);
				this.Move(context, destBlockId);
				result = -1;
				break;
			}
			case 8:
			{
				int argsCount9 = operation.ArgsCount;
				int num9 = argsCount9;
				if (num9 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				Location previous = default(Location);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref previous);
				Location current = default(Location);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref current);
				this.MoveFinish(context, previous, current);
				result = -1;
				break;
			}
			case 9:
			{
				int argsCount10 = operation.ArgsCount;
				int num10 = argsCount10;
				if (num10 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool returnValue = this.IsContinuousMovingBreak();
				result = Serializer.Serialize(returnValue, returnDataPool);
				break;
			}
			case 10:
			{
				int argsCount11 = operation.ArgsCount;
				int num11 = argsCount11;
				if (num11 != 1)
				{
					if (num11 != 2)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					short areaId2 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref areaId2);
					bool costAuthority = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref costAuthority);
					this.UnlockStation(context, areaId2, costAuthority);
					result = -1;
				}
				else
				{
					short areaId3 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref areaId3);
					this.UnlockStation(context, areaId3, true);
					result = -1;
				}
				break;
			}
			case 11:
			{
				int argsCount12 = operation.ArgsCount;
				int num12 = argsCount12;
				if (num12 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short fromAreaId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref fromAreaId);
				short fromBlockId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref fromBlockId);
				short toAreaId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref toAreaId);
				CrossAreaMoveInfo returnValue2 = this.GetTravelCost(fromAreaId, fromBlockId, toAreaId);
				result = Serializer.Serialize(returnValue2, returnDataPool);
				break;
			}
			case 12:
			{
				int argsCount13 = operation.ArgsCount;
				int num13 = argsCount13;
				if (num13 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short toAreaId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref toAreaId2);
				this.StartTravel(context, toAreaId2);
				result = -1;
				break;
			}
			case 13:
			{
				int argsCount14 = operation.ArgsCount;
				int num14 = argsCount14;
				if (num14 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool returnValue3 = this.ContinueTravel(context);
				result = Serializer.Serialize(returnValue3, returnDataPool);
				break;
			}
			case 14:
			{
				int argsCount15 = operation.ArgsCount;
				int num15 = argsCount15;
				if (num15 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.StopTravel(context);
				result = -1;
				break;
			}
			case 15:
			{
				int argsCount16 = operation.ArgsCount;
				int num16 = argsCount16;
				if (num16 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int costedDays = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref costedDays);
				this.RecordTravelCostedDays(context, costedDays);
				result = -1;
				break;
			}
			case 16:
			{
				int argsCount17 = operation.ArgsCount;
				int num17 = argsCount17;
				if (num17 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int[] returnValue4 = this.GetAllAreaCompletelyInfectedCharCount();
				result = Serializer.Serialize(returnValue4, returnDataPool);
				break;
			}
			case 17:
			{
				int argsCount18 = operation.ArgsCount;
				int num18 = argsCount18;
				if (num18 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte stateId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref stateId);
				Dictionary<TravelRouteKey, TravelRoute> returnValue5 = this.GetTravelRoutesInState(stateId);
				result = Serializer.Serialize(returnValue5, returnDataPool);
				break;
			}
			case 18:
			{
				int argsCount19 = operation.ArgsCount;
				int num19 = argsCount19;
				if (num19 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool returnValue6 = this.TryTriggerCricketCatch(context);
				result = Serializer.Serialize(returnValue6, returnDataPool);
				break;
			}
			case 19:
			{
				int argsCount20 = operation.ArgsCount;
				int num20 = argsCount20;
				if (num20 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short areaId4 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref areaId4);
				short blockId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockId);
				MapBlockData returnValue7 = this.GetBlockData(areaId4, blockId);
				result = Serializer.Serialize(returnValue7, returnDataPool);
				break;
			}
			case 20:
				switch (operation.ArgsCount)
				{
				case 2:
				{
					int charId = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId);
					sbyte resourceType = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref resourceType);
					CollectResourceResult returnValue8 = this.CollectResource(context, charId, resourceType, true, true);
					result = Serializer.Serialize(returnValue8, returnDataPool);
					break;
				}
				case 3:
				{
					int charId2 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId2);
					sbyte resourceType2 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref resourceType2);
					bool costTime = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref costTime);
					CollectResourceResult returnValue9 = this.CollectResource(context, charId2, resourceType2, costTime, true);
					result = Serializer.Serialize(returnValue9, returnDataPool);
					break;
				}
				case 4:
				{
					int charId3 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId3);
					sbyte resourceType3 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref resourceType3);
					bool costTime2 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref costTime2);
					bool costResource = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref costResource);
					CollectResourceResult returnValue10 = this.CollectResource(context, charId3, resourceType3, costTime2, costResource);
					result = Serializer.Serialize(returnValue10, returnDataPool);
					break;
				}
				default:
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				}
				break;
			case 21:
			{
				int argsCount21 = operation.ArgsCount;
				int num21 = argsCount21;
				if (num21 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<Location> locationList = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref locationList);
				List<MapBlockData> returnValue11 = this.GetMapBlockDataList(locationList);
				result = Serializer.Serialize(returnValue11, returnDataPool);
				break;
			}
			case 22:
			{
				int argsCount22 = operation.ArgsCount;
				int num22 = argsCount22;
				if (num22 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<Location> locationList2 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref locationList2);
				List<short> returnValue12 = this.GetBelongBlockTemplateIdList(locationList2);
				result = Serializer.Serialize(returnValue12, returnDataPool);
				break;
			}
			case 23:
			{
				int argsCount23 = operation.ArgsCount;
				int num23 = argsCount23;
				if (num23 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				Location location = default(Location);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref location);
				LocationNameRelatedData returnValue13 = this.GetLocationNameRelatedData(location);
				result = Serializer.Serialize(returnValue13, returnDataPool);
				break;
			}
			case 24:
			{
				int argsCount24 = operation.ArgsCount;
				int num24 = argsCount24;
				if (num24 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<Location> locations = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref locations);
				List<LocationNameRelatedData> returnValue14 = this.GetLocationNameRelatedDataList(locations);
				result = Serializer.Serialize(returnValue14, returnDataPool);
				break;
			}
			case 25:
			{
				int argsCount25 = operation.ArgsCount;
				int num25 = argsCount25;
				if (num25 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				Location location2 = default(Location);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref location2);
				short blockTemplateId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockTemplateId);
				bool isTurnVisible = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isTurnVisible);
				this.ChangeBlockTemplate(context, location2, blockTemplateId, isTurnVisible);
				result = -1;
				break;
			}
			case 26:
			{
				int argsCount26 = operation.ArgsCount;
				int num26 = argsCount26;
				if (num26 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short areaId5 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref areaId5);
				bool returnValue15 = this.IsContainsPurpleBamboo(areaId5);
				result = Serializer.Serialize(returnValue15, returnDataPool);
				break;
			}
			case 27:
			{
				int argsCount27 = operation.ArgsCount;
				int num27 = argsCount27;
				if (num27 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int[] returnValue16 = this.GetAllStateCompletelyInfectedCharCount();
				result = Serializer.Serialize(returnValue16, returnDataPool);
				break;
			}
			case 28:
			{
				int argsCount28 = operation.ArgsCount;
				int num28 = argsCount28;
				if (num28 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				Location location3 = default(Location);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref location3);
				FullBlockName returnValue17 = this.GetBlockFullName(location3);
				result = Serializer.Serialize(returnValue17, returnDataPool);
				break;
			}
			case 29:
				switch (operation.ArgsCount)
				{
				case 1:
				{
					List<Location> locationList3 = null;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref locationList3);
					List<MapBlockData> returnValue18 = this.GetMapBlockDataListOptional(locationList3, false, false);
					result = Serializer.Serialize(returnValue18, returnDataPool);
					break;
				}
				case 2:
				{
					List<Location> locationList4 = null;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref locationList4);
					bool includeRoot = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref includeRoot);
					List<MapBlockData> returnValue19 = this.GetMapBlockDataListOptional(locationList4, includeRoot, false);
					result = Serializer.Serialize(returnValue19, returnDataPool);
					break;
				}
				case 3:
				{
					List<Location> locationList5 = null;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref locationList5);
					bool includeRoot2 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref includeRoot2);
					bool includeBelong = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref includeBelong);
					List<MapBlockData> returnValue20 = this.GetMapBlockDataListOptional(locationList5, includeRoot2, includeBelong);
					result = Serializer.Serialize(returnValue20, returnDataPool);
					break;
				}
				default:
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				}
				break;
			case 30:
			{
				int argsCount29 = operation.ArgsCount;
				int num29 = argsCount29;
				if (num29 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				Location location4 = default(Location);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref location4);
				Location settlementLocation = default(Location);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref settlementLocation);
				bool returnValue21 = this.IsLocationInBuildingEffectRange(location4, settlementLocation);
				result = Serializer.Serialize(returnValue21, returnDataPool);
				break;
			}
			case 31:
			{
				int argsCount30 = operation.ArgsCount;
				int num30 = argsCount30;
				if (num30 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short returnValue22 = this.ContinueTravelWithDetectTravelingEvent(context);
				result = Serializer.Serialize(returnValue22, returnDataPool);
				break;
			}
			case 32:
			{
				int argsCount31 = operation.ArgsCount;
				int num31 = argsCount31;
				if (num31 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<CollectResourceResult> returnValue23 = this.CollectAllResourcesFree(context);
				result = Serializer.Serialize(returnValue23, returnDataPool);
				break;
			}
			case 33:
			{
				int argsCount32 = operation.ArgsCount;
				int num32 = argsCount32;
				if (num32 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short destAreaId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref destAreaId);
				this.QuickTravel(context, destAreaId);
				result = -1;
				break;
			}
			case 34:
			{
				int argsCount33 = operation.ArgsCount;
				int num33 = argsCount33;
				if (num33 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short templateId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref templateId2);
				Location returnValue24 = this.QueryFixedCharacterLocation(templateId2);
				result = Serializer.Serialize(returnValue24, returnDataPool);
				break;
			}
			case 35:
			{
				int argsCount34 = operation.ArgsCount;
				int num34 = argsCount34;
				if (num34 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				AreaDisplayData[] returnValue25 = this.GetAllAreaDisplayData();
				result = Serializer.Serialize(returnValue25, returnDataPool);
				break;
			}
			case 36:
			{
				int argsCount35 = operation.ArgsCount;
				int num35 = argsCount35;
				if (num35 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int templateId3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref templateId3);
				Location returnValue26 = this.QueryTemplateBlockLocation(templateId3);
				result = Serializer.Serialize(returnValue26, returnDataPool);
				break;
			}
			case 37:
			{
				int argsCount36 = operation.ArgsCount;
				int num36 = argsCount36;
				if (num36 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short areaId6 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref areaId6);
				List<MapBlockDisplayData> returnValue27 = this.GetBlockDisplayDataInArea(areaId6);
				result = Serializer.Serialize(returnValue27, returnDataPool);
				break;
			}
			case 38:
			{
				int argsCount37 = operation.ArgsCount;
				int num37 = argsCount37;
				if (num37 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short toAreaId3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref toAreaId3);
				bool returnValue28 = this.UnlockTravelPath(context, toAreaId3);
				result = Serializer.Serialize(returnValue28, returnDataPool);
				break;
			}
			case 39:
			{
				int argsCount38 = operation.ArgsCount;
				int num38 = argsCount38;
				if (num38 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.GmCmd_HideAllMapBlock(context);
				result = -1;
				break;
			}
			case 40:
			{
				int argsCount39 = operation.ArgsCount;
				int num39 = argsCount39;
				if (num39 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				Location start = default(Location);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref start);
				Location end = default(Location);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref end);
				List<Location> returnValue29 = this.GetPathInAreaWithoutCost(start, end);
				result = Serializer.Serialize(returnValue29, returnDataPool);
				break;
			}
			case 41:
			{
				int argsCount40 = operation.ArgsCount;
				int num40 = argsCount40;
				if (num40 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short toAreaId4 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref toAreaId4);
				TravelPreviewDisplayData returnValue30 = this.GetTravelPreview(toAreaId4);
				result = Serializer.Serialize(returnValue30, returnDataPool);
				break;
			}
			case 42:
			{
				int argsCount41 = operation.ArgsCount;
				int num41 = argsCount41;
				if (num41 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				Location location5 = default(Location);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref location5);
				this.RetrieveDreamBackLocation(context, location5);
				result = -1;
				break;
			}
			case 43:
			{
				int argsCount42 = operation.ArgsCount;
				int num42 = argsCount42;
				if (num42 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short areaId7 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref areaId7);
				MapAreaData returnValue31 = this.GetAreaByAreaId(areaId7);
				result = Serializer.Serialize(returnValue31, returnDataPool);
				break;
			}
			case 44:
			{
				int argsCount43 = operation.ArgsCount;
				int num43 = argsCount43;
				if (num43 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short templateId4 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref templateId4);
				this.GmCmd_AddAnimal(context, templateId4);
				result = -1;
				break;
			}
			case 45:
			{
				int argsCount44 = operation.ArgsCount;
				int num44 = argsCount44;
				if (num44 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool isTraveling = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isTraveling);
				TeammateBubbleCollection returnValue32 = this.GetTeammateBubbleCollection(context, isTraveling);
				result = Serializer.Serialize(returnValue32, returnDataPool);
				break;
			}
			case 46:
			{
				int argsCount45 = operation.ArgsCount;
				int num45 = argsCount45;
				if (num45 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short templateId5 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref templateId5);
				this.GmCmd_AddRandomEnemyOnMap(context, templateId5);
				result = -1;
				break;
			}
			case 47:
			{
				int argsCount46 = operation.ArgsCount;
				int num46 = argsCount46;
				if (num46 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.GMCmd_ThrowBackend();
				result = -1;
				break;
			}
			case 48:
				switch (operation.ArgsCount)
				{
				case 3:
				{
					int typeInt = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref typeInt);
					int doctorId = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref doctorId);
					int patientId = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref patientId);
					MapHealSimulateResult returnValue33 = this.SimulateHealCost(typeInt, doctorId, patientId, false, false);
					result = Serializer.Serialize(returnValue33, returnDataPool);
					break;
				}
				case 4:
				{
					int typeInt2 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref typeInt2);
					int doctorId2 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref doctorId2);
					int patientId2 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref patientId2);
					bool needPay = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref needPay);
					MapHealSimulateResult returnValue34 = this.SimulateHealCost(typeInt2, doctorId2, patientId2, needPay, false);
					result = Serializer.Serialize(returnValue34, returnDataPool);
					break;
				}
				case 5:
				{
					int typeInt3 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref typeInt3);
					int doctorId3 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref doctorId3);
					int patientId3 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref patientId3);
					bool needPay2 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref needPay2);
					bool isExpensiveHeal = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isExpensiveHeal);
					MapHealSimulateResult returnValue35 = this.SimulateHealCost(typeInt3, doctorId3, patientId3, needPay2, isExpensiveHeal);
					result = Serializer.Serialize(returnValue35, returnDataPool);
					break;
				}
				default:
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				}
				break;
			case 49:
				switch (operation.ArgsCount)
				{
				case 3:
				{
					int typeInt4 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref typeInt4);
					int doctorId4 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref doctorId4);
					int patientId4 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref patientId4);
					bool returnValue36 = this.HealOnMap(context, typeInt4, doctorId4, patientId4, false, -1, false);
					result = Serializer.Serialize(returnValue36, returnDataPool);
					break;
				}
				case 4:
				{
					int typeInt5 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref typeInt5);
					int doctorId5 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref doctorId5);
					int patientId5 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref patientId5);
					bool needPay3 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref needPay3);
					bool returnValue37 = this.HealOnMap(context, typeInt5, doctorId5, patientId5, needPay3, -1, false);
					result = Serializer.Serialize(returnValue37, returnDataPool);
					break;
				}
				case 5:
				{
					int typeInt6 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref typeInt6);
					int doctorId6 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref doctorId6);
					int patientId6 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref patientId6);
					bool needPay4 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref needPay4);
					int payerId = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref payerId);
					bool returnValue38 = this.HealOnMap(context, typeInt6, doctorId6, patientId6, needPay4, payerId, false);
					result = Serializer.Serialize(returnValue38, returnDataPool);
					break;
				}
				case 6:
				{
					int typeInt7 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref typeInt7);
					int doctorId7 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref doctorId7);
					int patientId7 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref patientId7);
					bool needPay5 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref needPay5);
					int payerId2 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref payerId2);
					bool isExpensiveHeal2 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isExpensiveHeal2);
					bool returnValue39 = this.HealOnMap(context, typeInt7, doctorId7, patientId7, needPay5, payerId2, isExpensiveHeal2);
					result = Serializer.Serialize(returnValue39, returnDataPool);
					break;
				}
				default:
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				}
				break;
			case 50:
			{
				int argsCount47 = operation.ArgsCount;
				int num47 = argsCount47;
				if (num47 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short destBlockId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref destBlockId2);
				this.TeleportByTraveler(context, destBlockId2);
				result = -1;
				break;
			}
			case 51:
			{
				int argsCount48 = operation.ArgsCount;
				int num48 = argsCount48;
				if (num48 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				Location location6 = default(Location);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref location6);
				bool returnValue40 = this.BuildTravelerPalace(context, location6);
				result = Serializer.Serialize(returnValue40, returnDataPool);
				break;
			}
			case 52:
			{
				int argsCount49 = operation.ArgsCount;
				int num49 = argsCount49;
				if (num49 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int index = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref index);
				bool returnValue41 = this.TeleportOnTravelerPalace(context, index);
				result = Serializer.Serialize(returnValue41, returnDataPool);
				break;
			}
			case 53:
			{
				int argsCount50 = operation.ArgsCount;
				int num50 = argsCount50;
				if (num50 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int index2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref index2);
				string newName = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref newName);
				bool returnValue42 = this.ChangeTravelerPalaceName(context, index2, newName);
				result = Serializer.Serialize(returnValue42, returnDataPool);
				break;
			}
			case 54:
			{
				int argsCount51 = operation.ArgsCount;
				int num51 = argsCount51;
				if (num51 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int index3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref index3);
				bool returnValue43 = this.DestroyTravelerPalace(context, index3);
				result = Serializer.Serialize(returnValue43, returnDataPool);
				break;
			}
			case 55:
			{
				int argsCount52 = operation.ArgsCount;
				int num52 = argsCount52;
				if (num52 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int spiritualDebt2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref spiritualDebt2);
				this.GmCmd_ChangeAllSpiritualDebt(context, spiritualDebt2);
				result = -1;
				break;
			}
			case 56:
			{
				int argsCount53 = operation.ArgsCount;
				int num53 = argsCount53;
				if (num53 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				Location targetLocation = default(Location);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref targetLocation);
				int hunterCharId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref hunterCharId);
				this.TaiwuBeKidnapped(context, targetLocation, hunterCharId);
				result = -1;
				break;
			}
			case 57:
			{
				int argsCount54 = operation.ArgsCount;
				int num54 = argsCount54;
				if (num54 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.DirectTravelToTaiwuVillage(context);
				result = -1;
				break;
			}
			case 58:
			{
				int argsCount55 = operation.ArgsCount;
				int num55 = argsCount55;
				if (num55 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int templateId6 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref templateId6);
				short areaId8 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref areaId8);
				Location returnValue44 = this.QueryTemplateBlockLocationInArea(templateId6, areaId8);
				result = Serializer.Serialize(returnValue44, returnDataPool);
				break;
			}
			case 59:
			{
				int argsCount56 = operation.ArgsCount;
				int num56 = argsCount56;
				if (num56 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short templateId7 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref templateId7);
				short areaId9 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref areaId9);
				Location returnValue45 = this.QueryFixedCharacterLocationInArea(templateId7, areaId9);
				result = Serializer.Serialize(returnValue45, returnDataPool);
				break;
			}
			case 60:
			{
				int argsCount57 = operation.ArgsCount;
				int num57 = argsCount57;
				if (num57 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short areaId10 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref areaId10);
				Dictionary<short, short> returnValue46 = this.GetAllSettlementInfluenceRangeBlocks(areaId10);
				result = Serializer.SerializeDefault<Dictionary<short, short>>(returnValue46, returnDataPool);
				break;
			}
			case 61:
			{
				int argsCount58 = operation.ArgsCount;
				int num58 = argsCount58;
				if (num58 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.GmCmd_TurnMapBlockIntoAshes(context);
				result = -1;
				break;
			}
			case 62:
			{
				int argsCount59 = operation.ArgsCount;
				int num59 = argsCount59;
				if (num59 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short templateId8 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref templateId8);
				bool returnValue47 = this.GmCmd_TriggerTravelingEvent(context, templateId8);
				result = Serializer.Serialize(returnValue47, returnDataPool);
				break;
			}
			case 63:
			{
				int argsCount60 = operation.ArgsCount;
				int num60 = argsCount60;
				if (num60 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int returnValue48 = this.GmCmd_GetTreasuryValueByTaiwuLocation();
				result = Serializer.Serialize(returnValue48, returnDataPool);
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06007B9C RID: 31644 RVA: 0x0048AA50 File Offset: 0x00488C50
		public override void OnMonitorData(ushort dataId, ulong subId0, uint subId1, bool monitoring)
		{
			switch (dataId)
			{
			case 0:
				break;
			case 1:
				this._modificationsAreaBlocks0.ChangeRecording(monitoring);
				break;
			case 2:
				this._modificationsAreaBlocks1.ChangeRecording(monitoring);
				break;
			case 3:
				this._modificationsAreaBlocks2.ChangeRecording(monitoring);
				break;
			case 4:
				this._modificationsAreaBlocks3.ChangeRecording(monitoring);
				break;
			case 5:
				this._modificationsAreaBlocks4.ChangeRecording(monitoring);
				break;
			case 6:
				this._modificationsAreaBlocks5.ChangeRecording(monitoring);
				break;
			case 7:
				this._modificationsAreaBlocks6.ChangeRecording(monitoring);
				break;
			case 8:
				this._modificationsAreaBlocks7.ChangeRecording(monitoring);
				break;
			case 9:
				this._modificationsAreaBlocks8.ChangeRecording(monitoring);
				break;
			case 10:
				this._modificationsAreaBlocks9.ChangeRecording(monitoring);
				break;
			case 11:
				this._modificationsAreaBlocks10.ChangeRecording(monitoring);
				break;
			case 12:
				this._modificationsAreaBlocks11.ChangeRecording(monitoring);
				break;
			case 13:
				this._modificationsAreaBlocks12.ChangeRecording(monitoring);
				break;
			case 14:
				this._modificationsAreaBlocks13.ChangeRecording(monitoring);
				break;
			case 15:
				this._modificationsAreaBlocks14.ChangeRecording(monitoring);
				break;
			case 16:
				this._modificationsAreaBlocks15.ChangeRecording(monitoring);
				break;
			case 17:
				this._modificationsAreaBlocks16.ChangeRecording(monitoring);
				break;
			case 18:
				this._modificationsAreaBlocks17.ChangeRecording(monitoring);
				break;
			case 19:
				this._modificationsAreaBlocks18.ChangeRecording(monitoring);
				break;
			case 20:
				this._modificationsAreaBlocks19.ChangeRecording(monitoring);
				break;
			case 21:
				this._modificationsAreaBlocks20.ChangeRecording(monitoring);
				break;
			case 22:
				this._modificationsAreaBlocks21.ChangeRecording(monitoring);
				break;
			case 23:
				this._modificationsAreaBlocks22.ChangeRecording(monitoring);
				break;
			case 24:
				this._modificationsAreaBlocks23.ChangeRecording(monitoring);
				break;
			case 25:
				this._modificationsAreaBlocks24.ChangeRecording(monitoring);
				break;
			case 26:
				this._modificationsAreaBlocks25.ChangeRecording(monitoring);
				break;
			case 27:
				this._modificationsAreaBlocks26.ChangeRecording(monitoring);
				break;
			case 28:
				this._modificationsAreaBlocks27.ChangeRecording(monitoring);
				break;
			case 29:
				this._modificationsAreaBlocks28.ChangeRecording(monitoring);
				break;
			case 30:
				this._modificationsAreaBlocks29.ChangeRecording(monitoring);
				break;
			case 31:
				this._modificationsAreaBlocks30.ChangeRecording(monitoring);
				break;
			case 32:
				this._modificationsAreaBlocks31.ChangeRecording(monitoring);
				break;
			case 33:
				this._modificationsAreaBlocks32.ChangeRecording(monitoring);
				break;
			case 34:
				this._modificationsAreaBlocks33.ChangeRecording(monitoring);
				break;
			case 35:
				this._modificationsAreaBlocks34.ChangeRecording(monitoring);
				break;
			case 36:
				this._modificationsAreaBlocks35.ChangeRecording(monitoring);
				break;
			case 37:
				this._modificationsAreaBlocks36.ChangeRecording(monitoring);
				break;
			case 38:
				this._modificationsAreaBlocks37.ChangeRecording(monitoring);
				break;
			case 39:
				this._modificationsAreaBlocks38.ChangeRecording(monitoring);
				break;
			case 40:
				this._modificationsAreaBlocks39.ChangeRecording(monitoring);
				break;
			case 41:
				this._modificationsAreaBlocks40.ChangeRecording(monitoring);
				break;
			case 42:
				this._modificationsAreaBlocks41.ChangeRecording(monitoring);
				break;
			case 43:
				this._modificationsAreaBlocks42.ChangeRecording(monitoring);
				break;
			case 44:
				this._modificationsAreaBlocks43.ChangeRecording(monitoring);
				break;
			case 45:
				this._modificationsAreaBlocks44.ChangeRecording(monitoring);
				break;
			case 46:
				this._modificationsBrokenAreaBlocks.ChangeRecording(monitoring);
				break;
			case 47:
				this._modificationsBornAreaBlocks.ChangeRecording(monitoring);
				break;
			case 48:
				this._modificationsGuideAreaBlocks.ChangeRecording(monitoring);
				break;
			case 49:
				this._modificationsSecretVillageAreaBlocks.ChangeRecording(monitoring);
				break;
			case 50:
				this._modificationsBrokenPerformAreaBlocks.ChangeRecording(monitoring);
				break;
			case 51:
				this._modificationsTravelRouteDict.ChangeRecording(monitoring);
				break;
			case 52:
				this._modificationsBornStateTravelRouteDict.ChangeRecording(monitoring);
				break;
			case 53:
				break;
			case 54:
				break;
			case 55:
				break;
			case 56:
				break;
			case 57:
				break;
			case 58:
				break;
			case 59:
				break;
			case 60:
				break;
			case 61:
				break;
			case 62:
				break;
			case 63:
				break;
			case 64:
				break;
			case 65:
				break;
			case 66:
				break;
			case 67:
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06007B9D RID: 31645 RVA: 0x0048AFA0 File Offset: 0x004891A0
		public override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
		{
			int result;
			switch (dataId)
			{
			case 0:
			{
				bool flag = !BaseGameDataDomain.IsModified(this._dataStatesAreas, (int)subId0);
				if (flag)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this._dataStatesAreas, (int)subId0);
					result = Serializer.Serialize(this._areas[(int)subId0], dataPool);
				}
				break;
			}
			case 1:
			{
				bool flag2 = !BaseGameDataDomain.IsModified(this.DataStates, 1);
				if (flag2)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 1);
					int offset = Serializer.SerializeModifications<short>(this._areaBlocks0, dataPool, this._modificationsAreaBlocks0);
					this._modificationsAreaBlocks0.Reset();
					result = offset;
				}
				break;
			}
			case 2:
			{
				bool flag3 = !BaseGameDataDomain.IsModified(this.DataStates, 2);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 2);
					int offset2 = Serializer.SerializeModifications<short>(this._areaBlocks1, dataPool, this._modificationsAreaBlocks1);
					this._modificationsAreaBlocks1.Reset();
					result = offset2;
				}
				break;
			}
			case 3:
			{
				bool flag4 = !BaseGameDataDomain.IsModified(this.DataStates, 3);
				if (flag4)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 3);
					int offset3 = Serializer.SerializeModifications<short>(this._areaBlocks2, dataPool, this._modificationsAreaBlocks2);
					this._modificationsAreaBlocks2.Reset();
					result = offset3;
				}
				break;
			}
			case 4:
			{
				bool flag5 = !BaseGameDataDomain.IsModified(this.DataStates, 4);
				if (flag5)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 4);
					int offset4 = Serializer.SerializeModifications<short>(this._areaBlocks3, dataPool, this._modificationsAreaBlocks3);
					this._modificationsAreaBlocks3.Reset();
					result = offset4;
				}
				break;
			}
			case 5:
			{
				bool flag6 = !BaseGameDataDomain.IsModified(this.DataStates, 5);
				if (flag6)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 5);
					int offset5 = Serializer.SerializeModifications<short>(this._areaBlocks4, dataPool, this._modificationsAreaBlocks4);
					this._modificationsAreaBlocks4.Reset();
					result = offset5;
				}
				break;
			}
			case 6:
			{
				bool flag7 = !BaseGameDataDomain.IsModified(this.DataStates, 6);
				if (flag7)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 6);
					int offset6 = Serializer.SerializeModifications<short>(this._areaBlocks5, dataPool, this._modificationsAreaBlocks5);
					this._modificationsAreaBlocks5.Reset();
					result = offset6;
				}
				break;
			}
			case 7:
			{
				bool flag8 = !BaseGameDataDomain.IsModified(this.DataStates, 7);
				if (flag8)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 7);
					int offset7 = Serializer.SerializeModifications<short>(this._areaBlocks6, dataPool, this._modificationsAreaBlocks6);
					this._modificationsAreaBlocks6.Reset();
					result = offset7;
				}
				break;
			}
			case 8:
			{
				bool flag9 = !BaseGameDataDomain.IsModified(this.DataStates, 8);
				if (flag9)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 8);
					int offset8 = Serializer.SerializeModifications<short>(this._areaBlocks7, dataPool, this._modificationsAreaBlocks7);
					this._modificationsAreaBlocks7.Reset();
					result = offset8;
				}
				break;
			}
			case 9:
			{
				bool flag10 = !BaseGameDataDomain.IsModified(this.DataStates, 9);
				if (flag10)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 9);
					int offset9 = Serializer.SerializeModifications<short>(this._areaBlocks8, dataPool, this._modificationsAreaBlocks8);
					this._modificationsAreaBlocks8.Reset();
					result = offset9;
				}
				break;
			}
			case 10:
			{
				bool flag11 = !BaseGameDataDomain.IsModified(this.DataStates, 10);
				if (flag11)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 10);
					int offset10 = Serializer.SerializeModifications<short>(this._areaBlocks9, dataPool, this._modificationsAreaBlocks9);
					this._modificationsAreaBlocks9.Reset();
					result = offset10;
				}
				break;
			}
			case 11:
			{
				bool flag12 = !BaseGameDataDomain.IsModified(this.DataStates, 11);
				if (flag12)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 11);
					int offset11 = Serializer.SerializeModifications<short>(this._areaBlocks10, dataPool, this._modificationsAreaBlocks10);
					this._modificationsAreaBlocks10.Reset();
					result = offset11;
				}
				break;
			}
			case 12:
			{
				bool flag13 = !BaseGameDataDomain.IsModified(this.DataStates, 12);
				if (flag13)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 12);
					int offset12 = Serializer.SerializeModifications<short>(this._areaBlocks11, dataPool, this._modificationsAreaBlocks11);
					this._modificationsAreaBlocks11.Reset();
					result = offset12;
				}
				break;
			}
			case 13:
			{
				bool flag14 = !BaseGameDataDomain.IsModified(this.DataStates, 13);
				if (flag14)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 13);
					int offset13 = Serializer.SerializeModifications<short>(this._areaBlocks12, dataPool, this._modificationsAreaBlocks12);
					this._modificationsAreaBlocks12.Reset();
					result = offset13;
				}
				break;
			}
			case 14:
			{
				bool flag15 = !BaseGameDataDomain.IsModified(this.DataStates, 14);
				if (flag15)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 14);
					int offset14 = Serializer.SerializeModifications<short>(this._areaBlocks13, dataPool, this._modificationsAreaBlocks13);
					this._modificationsAreaBlocks13.Reset();
					result = offset14;
				}
				break;
			}
			case 15:
			{
				bool flag16 = !BaseGameDataDomain.IsModified(this.DataStates, 15);
				if (flag16)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 15);
					int offset15 = Serializer.SerializeModifications<short>(this._areaBlocks14, dataPool, this._modificationsAreaBlocks14);
					this._modificationsAreaBlocks14.Reset();
					result = offset15;
				}
				break;
			}
			case 16:
			{
				bool flag17 = !BaseGameDataDomain.IsModified(this.DataStates, 16);
				if (flag17)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 16);
					int offset16 = Serializer.SerializeModifications<short>(this._areaBlocks15, dataPool, this._modificationsAreaBlocks15);
					this._modificationsAreaBlocks15.Reset();
					result = offset16;
				}
				break;
			}
			case 17:
			{
				bool flag18 = !BaseGameDataDomain.IsModified(this.DataStates, 17);
				if (flag18)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 17);
					int offset17 = Serializer.SerializeModifications<short>(this._areaBlocks16, dataPool, this._modificationsAreaBlocks16);
					this._modificationsAreaBlocks16.Reset();
					result = offset17;
				}
				break;
			}
			case 18:
			{
				bool flag19 = !BaseGameDataDomain.IsModified(this.DataStates, 18);
				if (flag19)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 18);
					int offset18 = Serializer.SerializeModifications<short>(this._areaBlocks17, dataPool, this._modificationsAreaBlocks17);
					this._modificationsAreaBlocks17.Reset();
					result = offset18;
				}
				break;
			}
			case 19:
			{
				bool flag20 = !BaseGameDataDomain.IsModified(this.DataStates, 19);
				if (flag20)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 19);
					int offset19 = Serializer.SerializeModifications<short>(this._areaBlocks18, dataPool, this._modificationsAreaBlocks18);
					this._modificationsAreaBlocks18.Reset();
					result = offset19;
				}
				break;
			}
			case 20:
			{
				bool flag21 = !BaseGameDataDomain.IsModified(this.DataStates, 20);
				if (flag21)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 20);
					int offset20 = Serializer.SerializeModifications<short>(this._areaBlocks19, dataPool, this._modificationsAreaBlocks19);
					this._modificationsAreaBlocks19.Reset();
					result = offset20;
				}
				break;
			}
			case 21:
			{
				bool flag22 = !BaseGameDataDomain.IsModified(this.DataStates, 21);
				if (flag22)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 21);
					int offset21 = Serializer.SerializeModifications<short>(this._areaBlocks20, dataPool, this._modificationsAreaBlocks20);
					this._modificationsAreaBlocks20.Reset();
					result = offset21;
				}
				break;
			}
			case 22:
			{
				bool flag23 = !BaseGameDataDomain.IsModified(this.DataStates, 22);
				if (flag23)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 22);
					int offset22 = Serializer.SerializeModifications<short>(this._areaBlocks21, dataPool, this._modificationsAreaBlocks21);
					this._modificationsAreaBlocks21.Reset();
					result = offset22;
				}
				break;
			}
			case 23:
			{
				bool flag24 = !BaseGameDataDomain.IsModified(this.DataStates, 23);
				if (flag24)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 23);
					int offset23 = Serializer.SerializeModifications<short>(this._areaBlocks22, dataPool, this._modificationsAreaBlocks22);
					this._modificationsAreaBlocks22.Reset();
					result = offset23;
				}
				break;
			}
			case 24:
			{
				bool flag25 = !BaseGameDataDomain.IsModified(this.DataStates, 24);
				if (flag25)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 24);
					int offset24 = Serializer.SerializeModifications<short>(this._areaBlocks23, dataPool, this._modificationsAreaBlocks23);
					this._modificationsAreaBlocks23.Reset();
					result = offset24;
				}
				break;
			}
			case 25:
			{
				bool flag26 = !BaseGameDataDomain.IsModified(this.DataStates, 25);
				if (flag26)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 25);
					int offset25 = Serializer.SerializeModifications<short>(this._areaBlocks24, dataPool, this._modificationsAreaBlocks24);
					this._modificationsAreaBlocks24.Reset();
					result = offset25;
				}
				break;
			}
			case 26:
			{
				bool flag27 = !BaseGameDataDomain.IsModified(this.DataStates, 26);
				if (flag27)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 26);
					int offset26 = Serializer.SerializeModifications<short>(this._areaBlocks25, dataPool, this._modificationsAreaBlocks25);
					this._modificationsAreaBlocks25.Reset();
					result = offset26;
				}
				break;
			}
			case 27:
			{
				bool flag28 = !BaseGameDataDomain.IsModified(this.DataStates, 27);
				if (flag28)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 27);
					int offset27 = Serializer.SerializeModifications<short>(this._areaBlocks26, dataPool, this._modificationsAreaBlocks26);
					this._modificationsAreaBlocks26.Reset();
					result = offset27;
				}
				break;
			}
			case 28:
			{
				bool flag29 = !BaseGameDataDomain.IsModified(this.DataStates, 28);
				if (flag29)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 28);
					int offset28 = Serializer.SerializeModifications<short>(this._areaBlocks27, dataPool, this._modificationsAreaBlocks27);
					this._modificationsAreaBlocks27.Reset();
					result = offset28;
				}
				break;
			}
			case 29:
			{
				bool flag30 = !BaseGameDataDomain.IsModified(this.DataStates, 29);
				if (flag30)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 29);
					int offset29 = Serializer.SerializeModifications<short>(this._areaBlocks28, dataPool, this._modificationsAreaBlocks28);
					this._modificationsAreaBlocks28.Reset();
					result = offset29;
				}
				break;
			}
			case 30:
			{
				bool flag31 = !BaseGameDataDomain.IsModified(this.DataStates, 30);
				if (flag31)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 30);
					int offset30 = Serializer.SerializeModifications<short>(this._areaBlocks29, dataPool, this._modificationsAreaBlocks29);
					this._modificationsAreaBlocks29.Reset();
					result = offset30;
				}
				break;
			}
			case 31:
			{
				bool flag32 = !BaseGameDataDomain.IsModified(this.DataStates, 31);
				if (flag32)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 31);
					int offset31 = Serializer.SerializeModifications<short>(this._areaBlocks30, dataPool, this._modificationsAreaBlocks30);
					this._modificationsAreaBlocks30.Reset();
					result = offset31;
				}
				break;
			}
			case 32:
			{
				bool flag33 = !BaseGameDataDomain.IsModified(this.DataStates, 32);
				if (flag33)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 32);
					int offset32 = Serializer.SerializeModifications<short>(this._areaBlocks31, dataPool, this._modificationsAreaBlocks31);
					this._modificationsAreaBlocks31.Reset();
					result = offset32;
				}
				break;
			}
			case 33:
			{
				bool flag34 = !BaseGameDataDomain.IsModified(this.DataStates, 33);
				if (flag34)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 33);
					int offset33 = Serializer.SerializeModifications<short>(this._areaBlocks32, dataPool, this._modificationsAreaBlocks32);
					this._modificationsAreaBlocks32.Reset();
					result = offset33;
				}
				break;
			}
			case 34:
			{
				bool flag35 = !BaseGameDataDomain.IsModified(this.DataStates, 34);
				if (flag35)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 34);
					int offset34 = Serializer.SerializeModifications<short>(this._areaBlocks33, dataPool, this._modificationsAreaBlocks33);
					this._modificationsAreaBlocks33.Reset();
					result = offset34;
				}
				break;
			}
			case 35:
			{
				bool flag36 = !BaseGameDataDomain.IsModified(this.DataStates, 35);
				if (flag36)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 35);
					int offset35 = Serializer.SerializeModifications<short>(this._areaBlocks34, dataPool, this._modificationsAreaBlocks34);
					this._modificationsAreaBlocks34.Reset();
					result = offset35;
				}
				break;
			}
			case 36:
			{
				bool flag37 = !BaseGameDataDomain.IsModified(this.DataStates, 36);
				if (flag37)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 36);
					int offset36 = Serializer.SerializeModifications<short>(this._areaBlocks35, dataPool, this._modificationsAreaBlocks35);
					this._modificationsAreaBlocks35.Reset();
					result = offset36;
				}
				break;
			}
			case 37:
			{
				bool flag38 = !BaseGameDataDomain.IsModified(this.DataStates, 37);
				if (flag38)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 37);
					int offset37 = Serializer.SerializeModifications<short>(this._areaBlocks36, dataPool, this._modificationsAreaBlocks36);
					this._modificationsAreaBlocks36.Reset();
					result = offset37;
				}
				break;
			}
			case 38:
			{
				bool flag39 = !BaseGameDataDomain.IsModified(this.DataStates, 38);
				if (flag39)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 38);
					int offset38 = Serializer.SerializeModifications<short>(this._areaBlocks37, dataPool, this._modificationsAreaBlocks37);
					this._modificationsAreaBlocks37.Reset();
					result = offset38;
				}
				break;
			}
			case 39:
			{
				bool flag40 = !BaseGameDataDomain.IsModified(this.DataStates, 39);
				if (flag40)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 39);
					int offset39 = Serializer.SerializeModifications<short>(this._areaBlocks38, dataPool, this._modificationsAreaBlocks38);
					this._modificationsAreaBlocks38.Reset();
					result = offset39;
				}
				break;
			}
			case 40:
			{
				bool flag41 = !BaseGameDataDomain.IsModified(this.DataStates, 40);
				if (flag41)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 40);
					int offset40 = Serializer.SerializeModifications<short>(this._areaBlocks39, dataPool, this._modificationsAreaBlocks39);
					this._modificationsAreaBlocks39.Reset();
					result = offset40;
				}
				break;
			}
			case 41:
			{
				bool flag42 = !BaseGameDataDomain.IsModified(this.DataStates, 41);
				if (flag42)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 41);
					int offset41 = Serializer.SerializeModifications<short>(this._areaBlocks40, dataPool, this._modificationsAreaBlocks40);
					this._modificationsAreaBlocks40.Reset();
					result = offset41;
				}
				break;
			}
			case 42:
			{
				bool flag43 = !BaseGameDataDomain.IsModified(this.DataStates, 42);
				if (flag43)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 42);
					int offset42 = Serializer.SerializeModifications<short>(this._areaBlocks41, dataPool, this._modificationsAreaBlocks41);
					this._modificationsAreaBlocks41.Reset();
					result = offset42;
				}
				break;
			}
			case 43:
			{
				bool flag44 = !BaseGameDataDomain.IsModified(this.DataStates, 43);
				if (flag44)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 43);
					int offset43 = Serializer.SerializeModifications<short>(this._areaBlocks42, dataPool, this._modificationsAreaBlocks42);
					this._modificationsAreaBlocks42.Reset();
					result = offset43;
				}
				break;
			}
			case 44:
			{
				bool flag45 = !BaseGameDataDomain.IsModified(this.DataStates, 44);
				if (flag45)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 44);
					int offset44 = Serializer.SerializeModifications<short>(this._areaBlocks43, dataPool, this._modificationsAreaBlocks43);
					this._modificationsAreaBlocks43.Reset();
					result = offset44;
				}
				break;
			}
			case 45:
			{
				bool flag46 = !BaseGameDataDomain.IsModified(this.DataStates, 45);
				if (flag46)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 45);
					int offset45 = Serializer.SerializeModifications<short>(this._areaBlocks44, dataPool, this._modificationsAreaBlocks44);
					this._modificationsAreaBlocks44.Reset();
					result = offset45;
				}
				break;
			}
			case 46:
			{
				bool flag47 = !BaseGameDataDomain.IsModified(this.DataStates, 46);
				if (flag47)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 46);
					int offset46 = Serializer.SerializeModifications<short>(this._brokenAreaBlocks, dataPool, this._modificationsBrokenAreaBlocks);
					this._modificationsBrokenAreaBlocks.Reset();
					result = offset46;
				}
				break;
			}
			case 47:
			{
				bool flag48 = !BaseGameDataDomain.IsModified(this.DataStates, 47);
				if (flag48)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 47);
					int offset47 = Serializer.SerializeModifications<short>(this._bornAreaBlocks, dataPool, this._modificationsBornAreaBlocks);
					this._modificationsBornAreaBlocks.Reset();
					result = offset47;
				}
				break;
			}
			case 48:
			{
				bool flag49 = !BaseGameDataDomain.IsModified(this.DataStates, 48);
				if (flag49)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 48);
					int offset48 = Serializer.SerializeModifications<short>(this._guideAreaBlocks, dataPool, this._modificationsGuideAreaBlocks);
					this._modificationsGuideAreaBlocks.Reset();
					result = offset48;
				}
				break;
			}
			case 49:
			{
				bool flag50 = !BaseGameDataDomain.IsModified(this.DataStates, 49);
				if (flag50)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 49);
					int offset49 = Serializer.SerializeModifications<short>(this._secretVillageAreaBlocks, dataPool, this._modificationsSecretVillageAreaBlocks);
					this._modificationsSecretVillageAreaBlocks.Reset();
					result = offset49;
				}
				break;
			}
			case 50:
			{
				bool flag51 = !BaseGameDataDomain.IsModified(this.DataStates, 50);
				if (flag51)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 50);
					int offset50 = Serializer.SerializeModifications<short>(this._brokenPerformAreaBlocks, dataPool, this._modificationsBrokenPerformAreaBlocks);
					this._modificationsBrokenPerformAreaBlocks.Reset();
					result = offset50;
				}
				break;
			}
			case 51:
			{
				bool flag52 = !BaseGameDataDomain.IsModified(this.DataStates, 51);
				if (flag52)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 51);
					int offset51 = Serializer.SerializeModifications<TravelRouteKey>(this._travelRouteDict, dataPool, this._modificationsTravelRouteDict);
					this._modificationsTravelRouteDict.Reset();
					result = offset51;
				}
				break;
			}
			case 52:
			{
				bool flag53 = !BaseGameDataDomain.IsModified(this.DataStates, 52);
				if (flag53)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 52);
					int offset52 = Serializer.SerializeModifications<TravelRouteKey>(this._bornStateTravelRouteDict, dataPool, this._modificationsBornStateTravelRouteDict);
					this._modificationsBornStateTravelRouteDict.Reset();
					result = offset52;
				}
				break;
			}
			case 53:
			{
				bool flag54 = !BaseGameDataDomain.IsModified(this._dataStatesAnimalPlaceData, (int)subId0);
				if (flag54)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this._dataStatesAnimalPlaceData, (int)subId0);
					result = Serializer.Serialize(this._animalPlaceData[(int)subId0], dataPool);
				}
				break;
			}
			case 54:
			{
				bool flag55 = !BaseGameDataDomain.IsModified(this._dataStatesCricketPlaceData, (int)subId0);
				if (flag55)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this._dataStatesCricketPlaceData, (int)subId0);
					result = Serializer.Serialize(this._cricketPlaceData[(int)subId0], dataPool);
				}
				break;
			}
			case 55:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 56:
			{
				bool flag56 = !BaseGameDataDomain.IsModified(this._dataStatesSwordTombLocations, (int)subId0);
				if (flag56)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this._dataStatesSwordTombLocations, (int)subId0);
					result = Serializer.Serialize(this._swordTombLocations[(int)subId0], dataPool);
				}
				break;
			}
			case 57:
			{
				bool flag57 = !BaseGameDataDomain.IsModified(this.DataStates, 57);
				if (flag57)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 57);
					result = Serializer.Serialize(this._travelInfo, dataPool);
				}
				break;
			}
			case 58:
			{
				bool flag58 = !BaseGameDataDomain.IsModified(this.DataStates, 58);
				if (flag58)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 58);
					result = Serializer.Serialize(this._onHandlingTravelingEventBlock, dataPool);
				}
				break;
			}
			case 59:
			{
				bool flag59 = !BaseGameDataDomain.IsModified(this.DataStates, 59);
				if (flag59)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 59);
					result = Serializer.Serialize(this._hunterAnimalsCache, dataPool);
				}
				break;
			}
			case 60:
			{
				bool flag60 = !BaseGameDataDomain.IsModified(this.DataStates, 60);
				if (flag60)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 60);
					result = Serializer.Serialize(this._moveBanned, dataPool);
				}
				break;
			}
			case 61:
			{
				bool flag61 = !BaseGameDataDomain.IsModified(this.DataStates, 61);
				if (flag61)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 61);
					result = Serializer.Serialize(this._crossArchiveLockMoveTime, dataPool);
				}
				break;
			}
			case 62:
			{
				bool flag62 = !BaseGameDataDomain.IsModified(this.DataStates, 62);
				if (flag62)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 62);
					result = Serializer.Serialize(this.GetFleeBeasts(), dataPool);
				}
				break;
			}
			case 63:
			{
				bool flag63 = !BaseGameDataDomain.IsModified(this.DataStates, 63);
				if (flag63)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 63);
					result = Serializer.Serialize(this.GetFleeLoongs(), dataPool);
				}
				break;
			}
			case 64:
			{
				bool flag64 = !BaseGameDataDomain.IsModified(this.DataStates, 64);
				if (flag64)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 64);
					result = Serializer.Serialize(this.GetLoongLocations(), dataPool);
				}
				break;
			}
			case 65:
			{
				bool flag65 = !BaseGameDataDomain.IsModified(this.DataStates, 65);
				if (flag65)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 65);
					result = Serializer.Serialize(this.GetAlterSettlementLocations(), dataPool);
				}
				break;
			}
			case 66:
			{
				bool flag66 = !BaseGameDataDomain.IsModified(this.DataStates, 66);
				if (flag66)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 66);
					result = Serializer.Serialize(this._isTaiwuInFulongFlameArea, dataPool);
				}
				break;
			}
			case 67:
			{
				bool flag67 = !BaseGameDataDomain.IsModified(this.DataStates, 67);
				if (flag67)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 67);
					result = Serializer.Serialize(this.GetVisibleMapPickups(), dataPool);
				}
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06007B9E RID: 31646 RVA: 0x0048C614 File Offset: 0x0048A814
		public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			switch (dataId)
			{
			case 0:
			{
				bool flag = !BaseGameDataDomain.IsModified(this._dataStatesAreas, (int)subId0);
				if (!flag)
				{
					BaseGameDataDomain.ResetModified(this._dataStatesAreas, (int)subId0);
				}
				break;
			}
			case 1:
			{
				bool flag2 = !BaseGameDataDomain.IsModified(this.DataStates, 1);
				if (!flag2)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 1);
					this._modificationsAreaBlocks0.Reset();
				}
				break;
			}
			case 2:
			{
				bool flag3 = !BaseGameDataDomain.IsModified(this.DataStates, 2);
				if (!flag3)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 2);
					this._modificationsAreaBlocks1.Reset();
				}
				break;
			}
			case 3:
			{
				bool flag4 = !BaseGameDataDomain.IsModified(this.DataStates, 3);
				if (!flag4)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 3);
					this._modificationsAreaBlocks2.Reset();
				}
				break;
			}
			case 4:
			{
				bool flag5 = !BaseGameDataDomain.IsModified(this.DataStates, 4);
				if (!flag5)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 4);
					this._modificationsAreaBlocks3.Reset();
				}
				break;
			}
			case 5:
			{
				bool flag6 = !BaseGameDataDomain.IsModified(this.DataStates, 5);
				if (!flag6)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 5);
					this._modificationsAreaBlocks4.Reset();
				}
				break;
			}
			case 6:
			{
				bool flag7 = !BaseGameDataDomain.IsModified(this.DataStates, 6);
				if (!flag7)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 6);
					this._modificationsAreaBlocks5.Reset();
				}
				break;
			}
			case 7:
			{
				bool flag8 = !BaseGameDataDomain.IsModified(this.DataStates, 7);
				if (!flag8)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 7);
					this._modificationsAreaBlocks6.Reset();
				}
				break;
			}
			case 8:
			{
				bool flag9 = !BaseGameDataDomain.IsModified(this.DataStates, 8);
				if (!flag9)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 8);
					this._modificationsAreaBlocks7.Reset();
				}
				break;
			}
			case 9:
			{
				bool flag10 = !BaseGameDataDomain.IsModified(this.DataStates, 9);
				if (!flag10)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 9);
					this._modificationsAreaBlocks8.Reset();
				}
				break;
			}
			case 10:
			{
				bool flag11 = !BaseGameDataDomain.IsModified(this.DataStates, 10);
				if (!flag11)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 10);
					this._modificationsAreaBlocks9.Reset();
				}
				break;
			}
			case 11:
			{
				bool flag12 = !BaseGameDataDomain.IsModified(this.DataStates, 11);
				if (!flag12)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 11);
					this._modificationsAreaBlocks10.Reset();
				}
				break;
			}
			case 12:
			{
				bool flag13 = !BaseGameDataDomain.IsModified(this.DataStates, 12);
				if (!flag13)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 12);
					this._modificationsAreaBlocks11.Reset();
				}
				break;
			}
			case 13:
			{
				bool flag14 = !BaseGameDataDomain.IsModified(this.DataStates, 13);
				if (!flag14)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 13);
					this._modificationsAreaBlocks12.Reset();
				}
				break;
			}
			case 14:
			{
				bool flag15 = !BaseGameDataDomain.IsModified(this.DataStates, 14);
				if (!flag15)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 14);
					this._modificationsAreaBlocks13.Reset();
				}
				break;
			}
			case 15:
			{
				bool flag16 = !BaseGameDataDomain.IsModified(this.DataStates, 15);
				if (!flag16)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 15);
					this._modificationsAreaBlocks14.Reset();
				}
				break;
			}
			case 16:
			{
				bool flag17 = !BaseGameDataDomain.IsModified(this.DataStates, 16);
				if (!flag17)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 16);
					this._modificationsAreaBlocks15.Reset();
				}
				break;
			}
			case 17:
			{
				bool flag18 = !BaseGameDataDomain.IsModified(this.DataStates, 17);
				if (!flag18)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 17);
					this._modificationsAreaBlocks16.Reset();
				}
				break;
			}
			case 18:
			{
				bool flag19 = !BaseGameDataDomain.IsModified(this.DataStates, 18);
				if (!flag19)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 18);
					this._modificationsAreaBlocks17.Reset();
				}
				break;
			}
			case 19:
			{
				bool flag20 = !BaseGameDataDomain.IsModified(this.DataStates, 19);
				if (!flag20)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 19);
					this._modificationsAreaBlocks18.Reset();
				}
				break;
			}
			case 20:
			{
				bool flag21 = !BaseGameDataDomain.IsModified(this.DataStates, 20);
				if (!flag21)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 20);
					this._modificationsAreaBlocks19.Reset();
				}
				break;
			}
			case 21:
			{
				bool flag22 = !BaseGameDataDomain.IsModified(this.DataStates, 21);
				if (!flag22)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 21);
					this._modificationsAreaBlocks20.Reset();
				}
				break;
			}
			case 22:
			{
				bool flag23 = !BaseGameDataDomain.IsModified(this.DataStates, 22);
				if (!flag23)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 22);
					this._modificationsAreaBlocks21.Reset();
				}
				break;
			}
			case 23:
			{
				bool flag24 = !BaseGameDataDomain.IsModified(this.DataStates, 23);
				if (!flag24)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 23);
					this._modificationsAreaBlocks22.Reset();
				}
				break;
			}
			case 24:
			{
				bool flag25 = !BaseGameDataDomain.IsModified(this.DataStates, 24);
				if (!flag25)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 24);
					this._modificationsAreaBlocks23.Reset();
				}
				break;
			}
			case 25:
			{
				bool flag26 = !BaseGameDataDomain.IsModified(this.DataStates, 25);
				if (!flag26)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 25);
					this._modificationsAreaBlocks24.Reset();
				}
				break;
			}
			case 26:
			{
				bool flag27 = !BaseGameDataDomain.IsModified(this.DataStates, 26);
				if (!flag27)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 26);
					this._modificationsAreaBlocks25.Reset();
				}
				break;
			}
			case 27:
			{
				bool flag28 = !BaseGameDataDomain.IsModified(this.DataStates, 27);
				if (!flag28)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 27);
					this._modificationsAreaBlocks26.Reset();
				}
				break;
			}
			case 28:
			{
				bool flag29 = !BaseGameDataDomain.IsModified(this.DataStates, 28);
				if (!flag29)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 28);
					this._modificationsAreaBlocks27.Reset();
				}
				break;
			}
			case 29:
			{
				bool flag30 = !BaseGameDataDomain.IsModified(this.DataStates, 29);
				if (!flag30)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 29);
					this._modificationsAreaBlocks28.Reset();
				}
				break;
			}
			case 30:
			{
				bool flag31 = !BaseGameDataDomain.IsModified(this.DataStates, 30);
				if (!flag31)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 30);
					this._modificationsAreaBlocks29.Reset();
				}
				break;
			}
			case 31:
			{
				bool flag32 = !BaseGameDataDomain.IsModified(this.DataStates, 31);
				if (!flag32)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 31);
					this._modificationsAreaBlocks30.Reset();
				}
				break;
			}
			case 32:
			{
				bool flag33 = !BaseGameDataDomain.IsModified(this.DataStates, 32);
				if (!flag33)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 32);
					this._modificationsAreaBlocks31.Reset();
				}
				break;
			}
			case 33:
			{
				bool flag34 = !BaseGameDataDomain.IsModified(this.DataStates, 33);
				if (!flag34)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 33);
					this._modificationsAreaBlocks32.Reset();
				}
				break;
			}
			case 34:
			{
				bool flag35 = !BaseGameDataDomain.IsModified(this.DataStates, 34);
				if (!flag35)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 34);
					this._modificationsAreaBlocks33.Reset();
				}
				break;
			}
			case 35:
			{
				bool flag36 = !BaseGameDataDomain.IsModified(this.DataStates, 35);
				if (!flag36)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 35);
					this._modificationsAreaBlocks34.Reset();
				}
				break;
			}
			case 36:
			{
				bool flag37 = !BaseGameDataDomain.IsModified(this.DataStates, 36);
				if (!flag37)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 36);
					this._modificationsAreaBlocks35.Reset();
				}
				break;
			}
			case 37:
			{
				bool flag38 = !BaseGameDataDomain.IsModified(this.DataStates, 37);
				if (!flag38)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 37);
					this._modificationsAreaBlocks36.Reset();
				}
				break;
			}
			case 38:
			{
				bool flag39 = !BaseGameDataDomain.IsModified(this.DataStates, 38);
				if (!flag39)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 38);
					this._modificationsAreaBlocks37.Reset();
				}
				break;
			}
			case 39:
			{
				bool flag40 = !BaseGameDataDomain.IsModified(this.DataStates, 39);
				if (!flag40)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 39);
					this._modificationsAreaBlocks38.Reset();
				}
				break;
			}
			case 40:
			{
				bool flag41 = !BaseGameDataDomain.IsModified(this.DataStates, 40);
				if (!flag41)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 40);
					this._modificationsAreaBlocks39.Reset();
				}
				break;
			}
			case 41:
			{
				bool flag42 = !BaseGameDataDomain.IsModified(this.DataStates, 41);
				if (!flag42)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 41);
					this._modificationsAreaBlocks40.Reset();
				}
				break;
			}
			case 42:
			{
				bool flag43 = !BaseGameDataDomain.IsModified(this.DataStates, 42);
				if (!flag43)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 42);
					this._modificationsAreaBlocks41.Reset();
				}
				break;
			}
			case 43:
			{
				bool flag44 = !BaseGameDataDomain.IsModified(this.DataStates, 43);
				if (!flag44)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 43);
					this._modificationsAreaBlocks42.Reset();
				}
				break;
			}
			case 44:
			{
				bool flag45 = !BaseGameDataDomain.IsModified(this.DataStates, 44);
				if (!flag45)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 44);
					this._modificationsAreaBlocks43.Reset();
				}
				break;
			}
			case 45:
			{
				bool flag46 = !BaseGameDataDomain.IsModified(this.DataStates, 45);
				if (!flag46)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 45);
					this._modificationsAreaBlocks44.Reset();
				}
				break;
			}
			case 46:
			{
				bool flag47 = !BaseGameDataDomain.IsModified(this.DataStates, 46);
				if (!flag47)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 46);
					this._modificationsBrokenAreaBlocks.Reset();
				}
				break;
			}
			case 47:
			{
				bool flag48 = !BaseGameDataDomain.IsModified(this.DataStates, 47);
				if (!flag48)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 47);
					this._modificationsBornAreaBlocks.Reset();
				}
				break;
			}
			case 48:
			{
				bool flag49 = !BaseGameDataDomain.IsModified(this.DataStates, 48);
				if (!flag49)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 48);
					this._modificationsGuideAreaBlocks.Reset();
				}
				break;
			}
			case 49:
			{
				bool flag50 = !BaseGameDataDomain.IsModified(this.DataStates, 49);
				if (!flag50)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 49);
					this._modificationsSecretVillageAreaBlocks.Reset();
				}
				break;
			}
			case 50:
			{
				bool flag51 = !BaseGameDataDomain.IsModified(this.DataStates, 50);
				if (!flag51)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 50);
					this._modificationsBrokenPerformAreaBlocks.Reset();
				}
				break;
			}
			case 51:
			{
				bool flag52 = !BaseGameDataDomain.IsModified(this.DataStates, 51);
				if (!flag52)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 51);
					this._modificationsTravelRouteDict.Reset();
				}
				break;
			}
			case 52:
			{
				bool flag53 = !BaseGameDataDomain.IsModified(this.DataStates, 52);
				if (!flag53)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 52);
					this._modificationsBornStateTravelRouteDict.Reset();
				}
				break;
			}
			case 53:
			{
				bool flag54 = !BaseGameDataDomain.IsModified(this._dataStatesAnimalPlaceData, (int)subId0);
				if (!flag54)
				{
					BaseGameDataDomain.ResetModified(this._dataStatesAnimalPlaceData, (int)subId0);
				}
				break;
			}
			case 54:
			{
				bool flag55 = !BaseGameDataDomain.IsModified(this._dataStatesCricketPlaceData, (int)subId0);
				if (!flag55)
				{
					BaseGameDataDomain.ResetModified(this._dataStatesCricketPlaceData, (int)subId0);
				}
				break;
			}
			case 55:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 56:
			{
				bool flag56 = !BaseGameDataDomain.IsModified(this._dataStatesSwordTombLocations, (int)subId0);
				if (!flag56)
				{
					BaseGameDataDomain.ResetModified(this._dataStatesSwordTombLocations, (int)subId0);
				}
				break;
			}
			case 57:
			{
				bool flag57 = !BaseGameDataDomain.IsModified(this.DataStates, 57);
				if (!flag57)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 57);
				}
				break;
			}
			case 58:
			{
				bool flag58 = !BaseGameDataDomain.IsModified(this.DataStates, 58);
				if (!flag58)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 58);
				}
				break;
			}
			case 59:
			{
				bool flag59 = !BaseGameDataDomain.IsModified(this.DataStates, 59);
				if (!flag59)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 59);
				}
				break;
			}
			case 60:
			{
				bool flag60 = !BaseGameDataDomain.IsModified(this.DataStates, 60);
				if (!flag60)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 60);
				}
				break;
			}
			case 61:
			{
				bool flag61 = !BaseGameDataDomain.IsModified(this.DataStates, 61);
				if (!flag61)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 61);
				}
				break;
			}
			case 62:
			{
				bool flag62 = !BaseGameDataDomain.IsModified(this.DataStates, 62);
				if (!flag62)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 62);
				}
				break;
			}
			case 63:
			{
				bool flag63 = !BaseGameDataDomain.IsModified(this.DataStates, 63);
				if (!flag63)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 63);
				}
				break;
			}
			case 64:
			{
				bool flag64 = !BaseGameDataDomain.IsModified(this.DataStates, 64);
				if (!flag64)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 64);
				}
				break;
			}
			case 65:
			{
				bool flag65 = !BaseGameDataDomain.IsModified(this.DataStates, 65);
				if (!flag65)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 65);
				}
				break;
			}
			case 66:
			{
				bool flag66 = !BaseGameDataDomain.IsModified(this.DataStates, 66);
				if (!flag66)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 66);
				}
				break;
			}
			case 67:
			{
				bool flag67 = !BaseGameDataDomain.IsModified(this.DataStates, 67);
				if (!flag67)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 67);
				}
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06007B9F RID: 31647 RVA: 0x0048D638 File Offset: 0x0048B838
		public override bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			bool result;
			switch (dataId)
			{
			case 0:
				result = BaseGameDataDomain.IsModified(this._dataStatesAreas, (int)subId0);
				break;
			case 1:
				result = BaseGameDataDomain.IsModified(this.DataStates, 1);
				break;
			case 2:
				result = BaseGameDataDomain.IsModified(this.DataStates, 2);
				break;
			case 3:
				result = BaseGameDataDomain.IsModified(this.DataStates, 3);
				break;
			case 4:
				result = BaseGameDataDomain.IsModified(this.DataStates, 4);
				break;
			case 5:
				result = BaseGameDataDomain.IsModified(this.DataStates, 5);
				break;
			case 6:
				result = BaseGameDataDomain.IsModified(this.DataStates, 6);
				break;
			case 7:
				result = BaseGameDataDomain.IsModified(this.DataStates, 7);
				break;
			case 8:
				result = BaseGameDataDomain.IsModified(this.DataStates, 8);
				break;
			case 9:
				result = BaseGameDataDomain.IsModified(this.DataStates, 9);
				break;
			case 10:
				result = BaseGameDataDomain.IsModified(this.DataStates, 10);
				break;
			case 11:
				result = BaseGameDataDomain.IsModified(this.DataStates, 11);
				break;
			case 12:
				result = BaseGameDataDomain.IsModified(this.DataStates, 12);
				break;
			case 13:
				result = BaseGameDataDomain.IsModified(this.DataStates, 13);
				break;
			case 14:
				result = BaseGameDataDomain.IsModified(this.DataStates, 14);
				break;
			case 15:
				result = BaseGameDataDomain.IsModified(this.DataStates, 15);
				break;
			case 16:
				result = BaseGameDataDomain.IsModified(this.DataStates, 16);
				break;
			case 17:
				result = BaseGameDataDomain.IsModified(this.DataStates, 17);
				break;
			case 18:
				result = BaseGameDataDomain.IsModified(this.DataStates, 18);
				break;
			case 19:
				result = BaseGameDataDomain.IsModified(this.DataStates, 19);
				break;
			case 20:
				result = BaseGameDataDomain.IsModified(this.DataStates, 20);
				break;
			case 21:
				result = BaseGameDataDomain.IsModified(this.DataStates, 21);
				break;
			case 22:
				result = BaseGameDataDomain.IsModified(this.DataStates, 22);
				break;
			case 23:
				result = BaseGameDataDomain.IsModified(this.DataStates, 23);
				break;
			case 24:
				result = BaseGameDataDomain.IsModified(this.DataStates, 24);
				break;
			case 25:
				result = BaseGameDataDomain.IsModified(this.DataStates, 25);
				break;
			case 26:
				result = BaseGameDataDomain.IsModified(this.DataStates, 26);
				break;
			case 27:
				result = BaseGameDataDomain.IsModified(this.DataStates, 27);
				break;
			case 28:
				result = BaseGameDataDomain.IsModified(this.DataStates, 28);
				break;
			case 29:
				result = BaseGameDataDomain.IsModified(this.DataStates, 29);
				break;
			case 30:
				result = BaseGameDataDomain.IsModified(this.DataStates, 30);
				break;
			case 31:
				result = BaseGameDataDomain.IsModified(this.DataStates, 31);
				break;
			case 32:
				result = BaseGameDataDomain.IsModified(this.DataStates, 32);
				break;
			case 33:
				result = BaseGameDataDomain.IsModified(this.DataStates, 33);
				break;
			case 34:
				result = BaseGameDataDomain.IsModified(this.DataStates, 34);
				break;
			case 35:
				result = BaseGameDataDomain.IsModified(this.DataStates, 35);
				break;
			case 36:
				result = BaseGameDataDomain.IsModified(this.DataStates, 36);
				break;
			case 37:
				result = BaseGameDataDomain.IsModified(this.DataStates, 37);
				break;
			case 38:
				result = BaseGameDataDomain.IsModified(this.DataStates, 38);
				break;
			case 39:
				result = BaseGameDataDomain.IsModified(this.DataStates, 39);
				break;
			case 40:
				result = BaseGameDataDomain.IsModified(this.DataStates, 40);
				break;
			case 41:
				result = BaseGameDataDomain.IsModified(this.DataStates, 41);
				break;
			case 42:
				result = BaseGameDataDomain.IsModified(this.DataStates, 42);
				break;
			case 43:
				result = BaseGameDataDomain.IsModified(this.DataStates, 43);
				break;
			case 44:
				result = BaseGameDataDomain.IsModified(this.DataStates, 44);
				break;
			case 45:
				result = BaseGameDataDomain.IsModified(this.DataStates, 45);
				break;
			case 46:
				result = BaseGameDataDomain.IsModified(this.DataStates, 46);
				break;
			case 47:
				result = BaseGameDataDomain.IsModified(this.DataStates, 47);
				break;
			case 48:
				result = BaseGameDataDomain.IsModified(this.DataStates, 48);
				break;
			case 49:
				result = BaseGameDataDomain.IsModified(this.DataStates, 49);
				break;
			case 50:
				result = BaseGameDataDomain.IsModified(this.DataStates, 50);
				break;
			case 51:
				result = BaseGameDataDomain.IsModified(this.DataStates, 51);
				break;
			case 52:
				result = BaseGameDataDomain.IsModified(this.DataStates, 52);
				break;
			case 53:
				result = BaseGameDataDomain.IsModified(this._dataStatesAnimalPlaceData, (int)subId0);
				break;
			case 54:
				result = BaseGameDataDomain.IsModified(this._dataStatesCricketPlaceData, (int)subId0);
				break;
			case 55:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to verify modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 56:
				result = BaseGameDataDomain.IsModified(this._dataStatesSwordTombLocations, (int)subId0);
				break;
			case 57:
				result = BaseGameDataDomain.IsModified(this.DataStates, 57);
				break;
			case 58:
				result = BaseGameDataDomain.IsModified(this.DataStates, 58);
				break;
			case 59:
				result = BaseGameDataDomain.IsModified(this.DataStates, 59);
				break;
			case 60:
				result = BaseGameDataDomain.IsModified(this.DataStates, 60);
				break;
			case 61:
				result = BaseGameDataDomain.IsModified(this.DataStates, 61);
				break;
			case 62:
				result = BaseGameDataDomain.IsModified(this.DataStates, 62);
				break;
			case 63:
				result = BaseGameDataDomain.IsModified(this.DataStates, 63);
				break;
			case 64:
				result = BaseGameDataDomain.IsModified(this.DataStates, 64);
				break;
			case 65:
				result = BaseGameDataDomain.IsModified(this.DataStates, 65);
				break;
			case 66:
				result = BaseGameDataDomain.IsModified(this.DataStates, 66);
				break;
			case 67:
				result = BaseGameDataDomain.IsModified(this.DataStates, 67);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06007BA0 RID: 31648 RVA: 0x0048DCA0 File Offset: 0x0048BEA0
		public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			switch (influence.TargetIndicator.DataId)
			{
			case 0:
				break;
			case 1:
				break;
			case 2:
				break;
			case 3:
				break;
			case 4:
				break;
			case 5:
				break;
			case 6:
				break;
			case 7:
				break;
			case 8:
				break;
			case 9:
				break;
			case 10:
				break;
			case 11:
				break;
			case 12:
				break;
			case 13:
				break;
			case 14:
				break;
			case 15:
				break;
			case 16:
				break;
			case 17:
				break;
			case 18:
				break;
			case 19:
				break;
			case 20:
				break;
			case 21:
				break;
			case 22:
				break;
			case 23:
				break;
			case 24:
				break;
			case 25:
				break;
			case 26:
				break;
			case 27:
				break;
			case 28:
				break;
			case 29:
				break;
			case 30:
				break;
			case 31:
				break;
			case 32:
				break;
			case 33:
				break;
			case 34:
				break;
			case 35:
				break;
			case 36:
				break;
			case 37:
				break;
			case 38:
				break;
			case 39:
				break;
			case 40:
				break;
			case 41:
				break;
			case 42:
				break;
			case 43:
				break;
			case 44:
				break;
			case 45:
				break;
			case 46:
				break;
			case 47:
				break;
			case 48:
				break;
			case 49:
				break;
			case 50:
				break;
			case 51:
				break;
			case 52:
				break;
			case 53:
				break;
			case 54:
				break;
			case 55:
				break;
			case 56:
				break;
			case 57:
				break;
			case 58:
				break;
			case 59:
				break;
			case 60:
				break;
			case 61:
				break;
			case 62:
				BaseGameDataDomain.InvalidateSelfAndInfluencedCache(62, this.DataStates, MapDomain.CacheInfluences, context);
				return;
			case 63:
				BaseGameDataDomain.InvalidateSelfAndInfluencedCache(63, this.DataStates, MapDomain.CacheInfluences, context);
				return;
			case 64:
				BaseGameDataDomain.InvalidateSelfAndInfluencedCache(64, this.DataStates, MapDomain.CacheInfluences, context);
				return;
			case 65:
				BaseGameDataDomain.InvalidateSelfAndInfluencedCache(65, this.DataStates, MapDomain.CacheInfluences, context);
				return;
			case 66:
				break;
			case 67:
				BaseGameDataDomain.InvalidateSelfAndInfluencedCache(67, this.DataStates, MapDomain.CacheInfluences, context);
				return;
			default:
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(influence.TargetIndicator.DataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Cannot invalidate cache state of non-cache data ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(influence.TargetIndicator.DataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06007BA1 RID: 31649 RVA: 0x0048E000 File Offset: 0x0048C200
		public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			switch (operation.DataId)
			{
			case 0:
				ResponseProcessor.ProcessElementList_CustomType_Dynamic_Ref<MapAreaData>(operation, pResult, this._areas, 139);
				break;
			case 1:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks0);
				break;
			case 2:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks1);
				break;
			case 3:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks2);
				break;
			case 4:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks3);
				break;
			case 5:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks4);
				break;
			case 6:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks5);
				break;
			case 7:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks6);
				break;
			case 8:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks7);
				break;
			case 9:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks8);
				break;
			case 10:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks9);
				break;
			case 11:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks10);
				break;
			case 12:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks11);
				break;
			case 13:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks12);
				break;
			case 14:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks13);
				break;
			case 15:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks14);
				break;
			case 16:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks15);
				break;
			case 17:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks16);
				break;
			case 18:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks17);
				break;
			case 19:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks18);
				break;
			case 20:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks19);
				break;
			case 21:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks20);
				break;
			case 22:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks21);
				break;
			case 23:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks22);
				break;
			case 24:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks23);
				break;
			case 25:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks24);
				break;
			case 26:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks25);
				break;
			case 27:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks26);
				break;
			case 28:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks27);
				break;
			case 29:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks28);
				break;
			case 30:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks29);
				break;
			case 31:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks30);
				break;
			case 32:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks31);
				break;
			case 33:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks32);
				break;
			case 34:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks33);
				break;
			case 35:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks34);
				break;
			case 36:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks35);
				break;
			case 37:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks36);
				break;
			case 38:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks37);
				break;
			case 39:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks38);
				break;
			case 40:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks39);
				break;
			case 41:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks40);
				break;
			case 42:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks41);
				break;
			case 43:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks42);
				break;
			case 44:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks43);
				break;
			case 45:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._areaBlocks44);
				break;
			case 46:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._brokenAreaBlocks);
				break;
			case 47:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._bornAreaBlocks);
				break;
			case 48:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._guideAreaBlocks);
				break;
			case 49:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._secretVillageAreaBlocks);
				break;
			case 50:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<short, MapBlockData>(operation, pResult, this._brokenPerformAreaBlocks);
				break;
			case 51:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<TravelRouteKey, TravelRoute>(operation, pResult, this._travelRouteDict);
				break;
			case 52:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<TravelRouteKey, TravelRoute>(operation, pResult, this._bornStateTravelRouteDict);
				break;
			case 53:
				ResponseProcessor.ProcessElementList_CustomType_Dynamic_Ref<AnimalPlaceData>(operation, pResult, this._animalPlaceData, 139);
				break;
			case 54:
				ResponseProcessor.ProcessElementList_CustomType_Dynamic_Ref<CricketPlaceData>(operation, pResult, this._cricketPlaceData, 139);
				break;
			case 55:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Value<short, GameData.Utilities.ShortList>(operation, pResult, this._regularAreaNearList);
				break;
			case 56:
				ResponseProcessor.ProcessElementList_CustomType_Fixed_Value<Location>(operation, pResult, this._swordTombLocations, 8, 4);
				break;
			case 57:
				ResponseProcessor.ProcessSingleValue_CustomType_Dynamic_Ref_Single<CrossAreaMoveInfo>(operation, pResult, this._travelInfo);
				break;
			case 58:
				goto IL_65B;
			case 59:
				goto IL_65B;
			case 60:
				goto IL_65B;
			case 61:
				goto IL_65B;
			case 62:
				goto IL_65B;
			case 63:
				goto IL_65B;
			case 64:
				goto IL_65B;
			case 65:
				goto IL_65B;
			case 66:
				goto IL_65B;
			case 67:
				goto IL_65B;
			default:
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.DataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			bool flag = this._pendingLoadingOperationIds != null;
			if (flag)
			{
				uint currPendingOperationId = this._pendingLoadingOperationIds.Peek();
				bool flag2 = currPendingOperationId == operation.Id;
				if (flag2)
				{
					this._pendingLoadingOperationIds.Dequeue();
					bool flag3 = this._pendingLoadingOperationIds.Count <= 0;
					if (flag3)
					{
						this._pendingLoadingOperationIds = null;
						this.InitializeInternalDataOfCollections();
						this.OnLoadedArchiveData();
						DomainManager.Global.CompleteLoading(2);
					}
				}
			}
			return;
			IL_65B:
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Cannot process archive response of non-archive data ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.DataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06007BA2 RID: 31650 RVA: 0x0048E69C File Offset: 0x0048C89C
		private void InitializeInternalDataOfCollections()
		{
		}

		// Token: 0x06007BA4 RID: 31652 RVA: 0x0048E78C File Offset: 0x0048C98C
		[CompilerGenerated]
		internal static bool <TryTriggerCricketCatch>g__TryCostSweepNet|149_0(ref MapDomain.<>c__DisplayClass149_0 A_0)
		{
			List<ValueTuple<ItemKey, int>> ret = new List<ValueTuple<ItemKey, int>>();
			CharacterItemFilterWrappers.FindByTemplateId(A_0.taiwu, 12, 9, ret, true, false);
			bool flag = ret.Count == 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				ItemKey itemKey = ret[0].Item1;
				A_0.taiwu.RemoveInventoryItem(A_0.context, itemKey, 1, false, false);
				result = true;
			}
			return result;
		}

		// Token: 0x06007BA5 RID: 31653 RVA: 0x0048E7EC File Offset: 0x0048C9EC
		[CompilerGenerated]
		internal static void <TryTriggerCricketCatch>g__CricketLuckPointPostProcess|149_1(ref MapDomain.<>c__DisplayClass149_0 A_0)
		{
			DomainManager.Taiwu.SetCricketLuckPoint(DomainManager.Taiwu.GetCricketLuckPoint() + A_0.context.Random.Next(6, 13), A_0.context);
		}

		// Token: 0x06007BA6 RID: 31654 RVA: 0x0048E820 File Offset: 0x0048CA20
		[CompilerGenerated]
		internal static int <DetectTravelingEvent>g__CompareTravelingEventsDec|472_0([TupleElementNames(new string[]
		{
			"templateId",
			"weight"
		})] ValueTuple<short, short> a, [TupleElementNames(new string[]
		{
			"templateId",
			"weight"
		})] ValueTuple<short, short> b)
		{
			sbyte aOrder = TravelingEvent.Instance[a.Item1].OccurOrder;
			return TravelingEvent.Instance[b.Item1].OccurOrder.CompareTo(aOrder);
		}

		// Token: 0x0400219C RID: 8604
		public const sbyte RegularStateCount = 15;

		// Token: 0x0400219D RID: 8605
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x0400219E RID: 8606
		[DomainData(DomainDataType.ElementList, true, false, true, false, ArrayElementsCount = 139)]
		private readonly MapAreaData[] _areas;

		// Token: 0x0400219F RID: 8607
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks0;

		// Token: 0x040021A0 RID: 8608
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks1;

		// Token: 0x040021A1 RID: 8609
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks2;

		// Token: 0x040021A2 RID: 8610
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks3;

		// Token: 0x040021A3 RID: 8611
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks4;

		// Token: 0x040021A4 RID: 8612
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks5;

		// Token: 0x040021A5 RID: 8613
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks6;

		// Token: 0x040021A6 RID: 8614
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks7;

		// Token: 0x040021A7 RID: 8615
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks8;

		// Token: 0x040021A8 RID: 8616
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks9;

		// Token: 0x040021A9 RID: 8617
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks10;

		// Token: 0x040021AA RID: 8618
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks11;

		// Token: 0x040021AB RID: 8619
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks12;

		// Token: 0x040021AC RID: 8620
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks13;

		// Token: 0x040021AD RID: 8621
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks14;

		// Token: 0x040021AE RID: 8622
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks15;

		// Token: 0x040021AF RID: 8623
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks16;

		// Token: 0x040021B0 RID: 8624
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks17;

		// Token: 0x040021B1 RID: 8625
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks18;

		// Token: 0x040021B2 RID: 8626
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks19;

		// Token: 0x040021B3 RID: 8627
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks20;

		// Token: 0x040021B4 RID: 8628
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks21;

		// Token: 0x040021B5 RID: 8629
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks22;

		// Token: 0x040021B6 RID: 8630
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks23;

		// Token: 0x040021B7 RID: 8631
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks24;

		// Token: 0x040021B8 RID: 8632
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks25;

		// Token: 0x040021B9 RID: 8633
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks26;

		// Token: 0x040021BA RID: 8634
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks27;

		// Token: 0x040021BB RID: 8635
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks28;

		// Token: 0x040021BC RID: 8636
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks29;

		// Token: 0x040021BD RID: 8637
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks30;

		// Token: 0x040021BE RID: 8638
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks31;

		// Token: 0x040021BF RID: 8639
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks32;

		// Token: 0x040021C0 RID: 8640
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks33;

		// Token: 0x040021C1 RID: 8641
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks34;

		// Token: 0x040021C2 RID: 8642
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks35;

		// Token: 0x040021C3 RID: 8643
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks36;

		// Token: 0x040021C4 RID: 8644
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks37;

		// Token: 0x040021C5 RID: 8645
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks38;

		// Token: 0x040021C6 RID: 8646
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks39;

		// Token: 0x040021C7 RID: 8647
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks40;

		// Token: 0x040021C8 RID: 8648
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks41;

		// Token: 0x040021C9 RID: 8649
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks42;

		// Token: 0x040021CA RID: 8650
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks43;

		// Token: 0x040021CB RID: 8651
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _areaBlocks44;

		// Token: 0x040021CC RID: 8652
		private AreaBlockCollection[] _regularAreaBlocksArray;

		// Token: 0x040021CD RID: 8653
		private Action<short, MapBlockData, DataContext>[] _regularAreaBlocksAddFuncs;

		// Token: 0x040021CE RID: 8654
		private Action<short, MapBlockData, DataContext>[] _regularAreaBlocksSetFuncs;

		// Token: 0x040021CF RID: 8655
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _brokenAreaBlocks;

		// Token: 0x040021D0 RID: 8656
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _bornAreaBlocks;

		// Token: 0x040021D1 RID: 8657
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _guideAreaBlocks;

		// Token: 0x040021D2 RID: 8658
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _secretVillageAreaBlocks;

		// Token: 0x040021D3 RID: 8659
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private AreaBlockCollection _brokenPerformAreaBlocks;

		// Token: 0x040021D4 RID: 8660
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private Dictionary<TravelRouteKey, TravelRoute> _travelRouteDict;

		// Token: 0x040021D5 RID: 8661
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private Dictionary<TravelRouteKey, TravelRoute> _bornStateTravelRouteDict;

		// Token: 0x040021D6 RID: 8662
		[Obsolete]
		[DomainData(DomainDataType.ElementList, true, false, true, false, ArrayElementsCount = 139)]
		private readonly AnimalPlaceData[] _animalPlaceData;

		// Token: 0x040021D7 RID: 8663
		[DomainData(DomainDataType.ElementList, true, false, true, false, ArrayElementsCount = 139)]
		private readonly CricketPlaceData[] _cricketPlaceData;

		// Token: 0x040021D8 RID: 8664
		[DomainData(DomainDataType.SingleValueCollection, true, false, false, false)]
		private readonly Dictionary<short, GameData.Utilities.ShortList> _regularAreaNearList;

		// Token: 0x040021D9 RID: 8665
		[DomainData(DomainDataType.ElementList, true, false, true, true, ArrayElementsCount = 8)]
		private readonly Location[] _swordTombLocations;

		// Token: 0x040021DA RID: 8666
		[DomainData(DomainDataType.SingleValue, false, false, true, false)]
		private List<HunterAnimalKey> _hunterAnimalsCache;

		// Token: 0x040021DB RID: 8667
		[DomainData(DomainDataType.SingleValue, false, true, true, true)]
		private List<LoongLocationData> _loongLocations;

		// Token: 0x040021DC RID: 8668
		[DomainData(DomainDataType.SingleValue, false, true, true, true)]
		private List<Location> _fleeBeasts;

		// Token: 0x040021DD RID: 8669
		[DomainData(DomainDataType.SingleValue, false, true, true, true)]
		private List<Location> _fleeLoongs;

		// Token: 0x040021DE RID: 8670
		private readonly List<MapBlockDisplayData> _blockDisplayDataCache = new List<MapBlockDisplayData>();

		// Token: 0x040021DF RID: 8671
		public Location LastGetBlockDataPosition_Debug;

		// Token: 0x040021E0 RID: 8672
		private static AStarMap _aStarMap = new AStarMap();

		// Token: 0x040021E1 RID: 8673
		private const sbyte InitMaliceBlockPercent = 5;

		// Token: 0x040021E2 RID: 8674
		private Stopwatch _swCreatingNormalAreas;

		// Token: 0x040021E3 RID: 8675
		private Stopwatch _swCreatingSettlements;

		// Token: 0x040021E4 RID: 8676
		private Stopwatch _swInitializingAreaTravelRoutes;

		// Token: 0x040021E5 RID: 8677
		private static readonly Dictionary<MapPickup.EMapPickupType, MapDomain.ApplyPickupDelegate> ApplyPickups = new Dictionary<MapPickup.EMapPickupType, MapDomain.ApplyPickupDelegate>
		{
			{
				MapPickup.EMapPickupType.Resource,
				new MapDomain.ApplyPickupDelegate(MapDomain.AddResourceByPickup)
			},
			{
				MapPickup.EMapPickupType.Item,
				new MapDomain.ApplyPickupDelegate(MapDomain.AddItemByPickup)
			},
			{
				MapPickup.EMapPickupType.LoopEffect,
				new MapDomain.ApplyPickupDelegate(MapDomain.LoopOnceByPickup)
			},
			{
				MapPickup.EMapPickupType.ReadEffect,
				new MapDomain.ApplyPickupDelegate(MapDomain.ReadOnceByPickup)
			},
			{
				MapPickup.EMapPickupType.ExpBonus,
				new MapDomain.ApplyPickupDelegate(MapDomain.AddExpByPickup)
			},
			{
				MapPickup.EMapPickupType.DebtBonus,
				new MapDomain.ApplyPickupDelegate(MapDomain.AddDebtByPickup)
			}
		};

		// Token: 0x040021E6 RID: 8678
		[DomainData(DomainDataType.SingleValue, false, true, true, true)]
		private List<MapElementPickupDisplayData> _visibleMapPickups;

		// Token: 0x040021E9 RID: 8681
		private HashSet<Location> _wudangHeavenlyTrees = new HashSet<Location>();

		// Token: 0x040021EA RID: 8682
		private HashSet<int> _fulongLightedBlocks = new HashSet<int>();

		// Token: 0x040021EB RID: 8683
		[DomainData(DomainDataType.SingleValue, false, false, true, false)]
		private int _moveBanned;

		// Token: 0x040021EC RID: 8684
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private bool _crossArchiveLockMoveTime;

		// Token: 0x040021ED RID: 8685
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private bool _isTaiwuInFulongFlameArea;

		// Token: 0x040021EE RID: 8686
		public Queue<Location> TaiwuMoveRecord = new Queue<Location>(3);

		// Token: 0x040021EF RID: 8687
		private bool _lockMoveTime = false;

		// Token: 0x040021F0 RID: 8688
		private bool _teleportMove = false;

		// Token: 0x040021F1 RID: 8689
		[TupleElementNames(new string[]
		{
			"charId",
			"count"
		})]
		private ValueTuple<int, int> _lastTeammateBubble;

		// Token: 0x040021F2 RID: 8690
		private Location _lastTaiwuLocation;

		// Token: 0x040021F3 RID: 8691
		private bool _canTriggerFulongFlameTeammateBubble;

		// Token: 0x040021F4 RID: 8692
		[TupleElementNames(new string[]
		{
			"charId",
			"index",
			"subtype"
		})]
		private readonly List<ValueTuple<int, int, int>> _teammates = new List<ValueTuple<int, int, int>>();

		// Token: 0x040021F5 RID: 8693
		[TupleElementNames(new string[]
		{
			"charId",
			"index",
			"subtype"
		})]
		private readonly List<ValueTuple<int, int, int>> _teammateHighestPriorityText = new List<ValueTuple<int, int, int>>();

		// Token: 0x040021F6 RID: 8694
		private readonly Dictionary<int, int> _availableBubbleCache = new Dictionary<int, int>();

		// Token: 0x040021F7 RID: 8695
		private readonly Dictionary<ETeammateBubbleBubbleElementType, HashSet<short>> _elementTypeBubbleCache = new Dictionary<ETeammateBubbleBubbleElementType, HashSet<short>>();

		// Token: 0x040021F8 RID: 8696
		private readonly Dictionary<short, sbyte> _combatMatchIdToCombatSkillType = new Dictionary<short, sbyte>
		{
			{
				2,
				13
			},
			{
				0,
				8
			},
			{
				1,
				7
			},
			{
				13,
				10
			},
			{
				17,
				12
			},
			{
				14,
				3
			},
			{
				18,
				4
			},
			{
				11,
				6
			},
			{
				16,
				5
			},
			{
				15,
				11
			},
			{
				12,
				9
			}
		};

		// Token: 0x040021F9 RID: 8697
		private int _teammateTypes;

		// Token: 0x040021FA RID: 8698
		private const int AreaFindPathMultiplier = 1000;

		// Token: 0x040021FB RID: 8699
		[DomainData(DomainDataType.SingleValue, true, false, true, false)]
		private CrossAreaMoveInfo _travelInfo;

		// Token: 0x040021FC RID: 8700
		private readonly DijkstraMap _dijkstraMap = new DijkstraMap();

		// Token: 0x040021FD RID: 8701
		private int _carrierReduceTravelCostDaysPercent;

		// Token: 0x040021FE RID: 8702
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private bool _onHandlingTravelingEventBlock;

		// Token: 0x040021FF RID: 8703
		private readonly List<ValueTuple<short, short>> _travelingEventWeights = new List<ValueTuple<short, short>>();

		// Token: 0x04002200 RID: 8704
		[DomainData(DomainDataType.SingleValue, false, true, true, true)]
		private List<Location> _alterSettlementLocations;

		// Token: 0x04002201 RID: 8705
		private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[68][];

		// Token: 0x04002202 RID: 8706
		private static readonly DataInfluence[][] CacheInfluencesAreas = new DataInfluence[139][];

		// Token: 0x04002203 RID: 8707
		private readonly byte[] _dataStatesAreas = new byte[35];

		// Token: 0x04002204 RID: 8708
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks0 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002205 RID: 8709
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks1 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002206 RID: 8710
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks2 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002207 RID: 8711
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks3 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002208 RID: 8712
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks4 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002209 RID: 8713
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks5 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x0400220A RID: 8714
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks6 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x0400220B RID: 8715
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks7 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x0400220C RID: 8716
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks8 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x0400220D RID: 8717
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks9 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x0400220E RID: 8718
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks10 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x0400220F RID: 8719
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks11 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002210 RID: 8720
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks12 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002211 RID: 8721
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks13 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002212 RID: 8722
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks14 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002213 RID: 8723
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks15 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002214 RID: 8724
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks16 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002215 RID: 8725
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks17 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002216 RID: 8726
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks18 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002217 RID: 8727
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks19 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002218 RID: 8728
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks20 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002219 RID: 8729
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks21 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x0400221A RID: 8730
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks22 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x0400221B RID: 8731
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks23 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x0400221C RID: 8732
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks24 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x0400221D RID: 8733
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks25 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x0400221E RID: 8734
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks26 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x0400221F RID: 8735
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks27 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002220 RID: 8736
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks28 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002221 RID: 8737
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks29 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002222 RID: 8738
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks30 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002223 RID: 8739
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks31 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002224 RID: 8740
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks32 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002225 RID: 8741
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks33 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002226 RID: 8742
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks34 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002227 RID: 8743
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks35 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002228 RID: 8744
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks36 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002229 RID: 8745
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks37 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x0400222A RID: 8746
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks38 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x0400222B RID: 8747
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks39 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x0400222C RID: 8748
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks40 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x0400222D RID: 8749
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks41 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x0400222E RID: 8750
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks42 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x0400222F RID: 8751
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks43 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002230 RID: 8752
		private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks44 = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002231 RID: 8753
		private SingleValueCollectionModificationCollection<short> _modificationsBrokenAreaBlocks = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002232 RID: 8754
		private SingleValueCollectionModificationCollection<short> _modificationsBornAreaBlocks = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002233 RID: 8755
		private SingleValueCollectionModificationCollection<short> _modificationsGuideAreaBlocks = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002234 RID: 8756
		private SingleValueCollectionModificationCollection<short> _modificationsSecretVillageAreaBlocks = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002235 RID: 8757
		private SingleValueCollectionModificationCollection<short> _modificationsBrokenPerformAreaBlocks = SingleValueCollectionModificationCollection<short>.Create();

		// Token: 0x04002236 RID: 8758
		private SingleValueCollectionModificationCollection<TravelRouteKey> _modificationsTravelRouteDict = SingleValueCollectionModificationCollection<TravelRouteKey>.Create();

		// Token: 0x04002237 RID: 8759
		private SingleValueCollectionModificationCollection<TravelRouteKey> _modificationsBornStateTravelRouteDict = SingleValueCollectionModificationCollection<TravelRouteKey>.Create();

		// Token: 0x04002238 RID: 8760
		private static readonly DataInfluence[][] CacheInfluencesAnimalPlaceData = new DataInfluence[139][];

		// Token: 0x04002239 RID: 8761
		private readonly byte[] _dataStatesAnimalPlaceData = new byte[35];

		// Token: 0x0400223A RID: 8762
		private static readonly DataInfluence[][] CacheInfluencesCricketPlaceData = new DataInfluence[139][];

		// Token: 0x0400223B RID: 8763
		private readonly byte[] _dataStatesCricketPlaceData = new byte[35];

		// Token: 0x0400223C RID: 8764
		private static readonly DataInfluence[][] CacheInfluencesSwordTombLocations = new DataInfluence[8][];

		// Token: 0x0400223D RID: 8765
		private readonly byte[] _dataStatesSwordTombLocations = new byte[2];

		// Token: 0x0400223E RID: 8766
		private SpinLock _spinLockFleeBeasts = new SpinLock(false);

		// Token: 0x0400223F RID: 8767
		private SpinLock _spinLockFleeLoongs = new SpinLock(false);

		// Token: 0x04002240 RID: 8768
		private SpinLock _spinLockLoongLocations = new SpinLock(false);

		// Token: 0x04002241 RID: 8769
		private SpinLock _spinLockAlterSettlementLocations = new SpinLock(false);

		// Token: 0x04002242 RID: 8770
		private SpinLock _spinLockVisibleMapPickups = new SpinLock(false);

		// Token: 0x04002243 RID: 8771
		private Queue<uint> _pendingLoadingOperationIds;

		// Token: 0x02000C48 RID: 3144
		// (Invoke) Token: 0x06008E95 RID: 36501
		public delegate void ApplyPickupDelegate(DataContext context, MapPickup pickup);

		// Token: 0x02000C49 RID: 3145
		// (Invoke) Token: 0x06008E99 RID: 36505
		public delegate bool MapBlockDataFilter(MapBlockData blockData);
	}
}
