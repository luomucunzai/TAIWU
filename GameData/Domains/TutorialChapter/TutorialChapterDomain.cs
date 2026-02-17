using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Dependencies;
using GameData.Domains.Building;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.Relation;
using GameData.Domains.Map;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Domains.World;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TutorialChapter
{
	// Token: 0x02000039 RID: 57
	[GameDataDomain(15)]
	public class TutorialChapterDomain : BaseGameDataDomain
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000F26 RID: 3878 RVA: 0x000FBBB4 File Offset: 0x000F9DB4
		// (set) Token: 0x06000F27 RID: 3879 RVA: 0x000FBBBC File Offset: 0x000F9DBC
		public byte TutorialAreaSize { get; private set; } = 20;

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000F28 RID: 3880 RVA: 0x000FBBC5 File Offset: 0x000F9DC5
		public bool InGuiding
		{
			get
			{
				return this._inGuiding;
			}
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x000FBBD0 File Offset: 0x000F9DD0
		[DomainMethod]
		public unsafe int CreateFixedWorld(DataContext context, short templateId, WorldCreationInfo info)
		{
			this.SetInGuiding(true, context);
			TutorialChaptersItem config = TutorialChapters.Instance.GetItem(templateId);
			DomainManager.World.SetWorldCreationInfo(context, info, false);
			DomainManager.Map.CreateFixedTutorialArea(context);
			this.TutorialAreaSize = 20;
			bool flag = templateId == 0;
			if (flag)
			{
				Location locationA = EventHelper.GetTutorialLocationByCoordinate(9, 9);
				MapBlockData blockData = DomainManager.Map.GetBlock(locationA);
				blockData.TemplateId = 103;
				blockData.InitResources(context.Random);
				DomainManager.Map.GetBlock(locationA).InitResources(context.Random);
			}
			bool flag2 = templateId >= 1;
			if (flag2)
			{
				MapAreaData tutorialAreaData = DomainManager.Map.GetElement_Areas(136);
				SettlementInfo settlementInfo = tutorialAreaData.SettlementInfos[0];
				DomainManager.Taiwu.TryAddVisitedSettlement(settlementInfo.SettlementId, context);
				bool flag3 = templateId == 2;
				if (flag3)
				{
					Location forestLocation = EventHelper.GetTutorialLocationByCoordinate(8, 11);
					MapBlockData forestBlockData = DomainManager.Map.GetBlock(forestLocation);
					forestBlockData.CurrResources.Initialize();
					*(ref forestBlockData.CurrResources.Items.FixedElementField + 2) = *(ref forestBlockData.MaxResources.Items.FixedElementField + 2);
					DomainManager.Map.SetBlockData(context, forestBlockData);
				}
			}
			GameData.Domains.Character.Character huanxin = DomainManager.Character.CreateFixedCharacter(context, 444);
			int huanxinId = huanxin.GetId();
			DomainManager.Character.CompleteCreatingCharacter(huanxinId);
			GameData.Domains.Character.Character taiwu = huanxin;
			int taiwuId = huanxinId;
			GameData.Domains.Character.Character father = DomainManager.Character.CreateFixedCharacter(context, 453);
			int fatherId = father.GetId();
			bool flag4 = !RelationTypeHelper.AllowAddingAdoptiveParentRelation(huanxinId, fatherId);
			if (flag4)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to add adoptive parent relation: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(huanxinId);
				defaultInterpolatedStringHandler.AppendLiteral(" - ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(fatherId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			DomainManager.Character.AddRelation(context, huanxinId, fatherId, 64, int.MinValue);
			DomainManager.Character.DirectlySetFavorabilities(context, huanxinId, fatherId, 30000, 30000);
			DomainManager.Character.CompleteCreatingCharacter(fatherId);
			bool flag5 = config.MainCharacter == father.GetTemplateId();
			if (flag5)
			{
				taiwu = father;
				taiwuId = fatherId;
			}
			TutorialChaptersItem chapterConfig = TutorialChapters.Instance[templateId];
			foreach (Tuple<string, short> charInfo in chapterConfig.FixedCharacters)
			{
				bool flag6 = charInfo.Item2 == 444;
				if (flag6)
				{
					this._fixedCharacters.Add(charInfo.Item1, huanxinId);
				}
				else
				{
					bool flag7 = charInfo.Item2 == 453;
					if (!flag7)
					{
						short charTemplateId = charInfo.Item2;
						byte creatingType = Config.Character.Instance[charTemplateId].CreatingType;
						if (!true)
						{
						}
						GameData.Domains.Character.Character character2;
						switch (creatingType)
						{
						case 0:
							character2 = DomainManager.Character.CreateFixedCharacter(context, charTemplateId);
							break;
						case 1:
							goto IL_31D;
						case 2:
							character2 = DomainManager.Character.CreateRandomEnemy(context, charTemplateId, null);
							break;
						case 3:
							character2 = DomainManager.Character.CreateFixedEnemy(context, charTemplateId);
							break;
						default:
							goto IL_31D;
						}
						if (!true)
						{
						}
						GameData.Domains.Character.Character character = character2;
						int charId = character.GetId();
						DomainManager.Character.CompleteCreatingCharacter(charId);
						bool flag8 = charInfo.Item2 == 436;
						if (flag8)
						{
							AvatarData avatarData = character.GetAvatar();
							avatarData.ResetGrowableElementShowingAbility(0);
							character.SetAvatar(avatarData, context);
						}
						this._fixedCharacters.Add(charInfo.Item1, charId);
						goto IL_3C4;
						IL_31D:
						string paramName = "charTemplateId";
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(71, 1);
						defaultInterpolatedStringHandler.AppendLiteral("character with template id ");
						defaultInterpolatedStringHandler.AppendFormatted<short>(charTemplateId);
						defaultInterpolatedStringHandler.AppendLiteral(" is neither fixed character nor random enemy");
						throw new ArgumentOutOfRangeException(paramName, defaultInterpolatedStringHandler.ToStringAndClear());
					}
					this._fixedCharacters.Add(charInfo.Item1, fatherId);
				}
				IL_3C4:;
			}
			DomainManager.Taiwu.SetTaiwu(context, taiwu);
			DomainManager.Taiwu.JoinGroup(context, taiwuId, false);
			short areaId = 136;
			short blockId = ByteCoordinate.CoordinateToIndex(new ByteCoordinate(config.StartBlockCoordinate[0], config.StartBlockCoordinate[1]), this.TutorialAreaSize);
			Location location = new Location(areaId, blockId);
			taiwu.SetLocation(location, context);
			DomainManager.Global.OnCurrWorldArchiveDataReady(context, true);
			return taiwuId;
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x000FC021 File Offset: 0x000FA221
		[DomainMethod]
		public void StartChapter(DataContext context, int chapter)
		{
			this._curProgress = 0;
			this.SetTutorialChapter((short)chapter, context);
			DomainManager.TaiwuEvent.OnEvent_EnterTutorialChapter(this._tutorialChapter);
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x000FC048 File Offset: 0x000FA248
		[DomainMethod]
		public Location GetNextForceMoveToLocation()
		{
			return this.GetNextForceLocation();
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x000FC060 File Offset: 0x000FA260
		public void AdvanceProgress(DataContext context)
		{
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x000FC064 File Offset: 0x000FA264
		public void SpecialAdvanceMonthInTutorial()
		{
			bool flag = this._tutorialChapter == 2;
			if (flag)
			{
				Location bambooLocation = EventHelper.GetTutorialLocationByCoordinate(9, 9);
				List<BuildingBlockData> list = DomainManager.Building.GetBuildingBlockList(bambooLocation);
				foreach (BuildingBlockData blockData in list)
				{
					bool flag2 = blockData.OperationType == 0 && blockData.TemplateId == 258;
					if (flag2)
					{
						BuildingBlockItem configData = BuildingBlock.Instance.GetItem(blockData.TemplateId);
						blockData.Durability = configData.MaxDurability;
					}
				}
			}
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x000FC11C File Offset: 0x000FA31C
		public void SetTutorialEventArgBox(int chapter, EventArgBox argBox)
		{
			TutorialChaptersItem chapterConfig = TutorialChapters.Instance[chapter - 1];
			foreach (Tuple<string, short> charInfo in chapterConfig.FixedCharacters)
			{
				argBox.Set(charInfo.Item1, this._fixedCharacters[charInfo.Item1]);
			}
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x000FC170 File Offset: 0x000FA370
		public bool IsInTutorialChapter(int chapterIndex)
		{
			return this._inGuiding && (int)this.GetTutorialChapter() == chapterIndex;
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x000FC196 File Offset: 0x000FA396
		private void OnInitializedDomainData()
		{
			this._fixedCharacters = new Dictionary<string, int>();
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x000FC1A4 File Offset: 0x000FA3A4
		private void InitializeOnInitializeGameDataModule()
		{
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x000FC1A7 File Offset: 0x000FA3A7
		private void InitializeOnEnterNewWorld()
		{
			this._nextForceLocation = Location.Invalid;
			this._canMove = true;
			this._canAdvanceMonth = true;
			this._canOpenCharacterMenu = true;
			this._tutorialChapter = -1;
			this._guidVideoName = string.Empty;
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x000FC1DC File Offset: 0x000FA3DC
		private void OnLoadedArchiveData()
		{
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x000FC1E0 File Offset: 0x000FA3E0
		public TutorialChapterDomain() : base(12)
		{
			this._curProgress = 0;
			this._inGuiding = false;
			this._tutorialChapter = 0;
			this._canMove = false;
			this._canOpenCharacterMenu = false;
			this._guidVideoName = string.Empty;
			this._canAdvanceMonth = false;
			this._nextForceLocation = default(Location);
			this._neiliAllocateFitChapter7 = false;
			this._huanxinDying = false;
			this._huanxinSurprised = false;
			this._canShowEnterIndustry = false;
			this.OnInitializedDomainData();
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x000FC2A4 File Offset: 0x000FA4A4
		public int GetCurProgress()
		{
			return this._curProgress;
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x000FC2BC File Offset: 0x000FA4BC
		public void SetCurProgress(int value, DataContext context)
		{
			this._curProgress = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, this.DataStates, TutorialChapterDomain.CacheInfluences, context);
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x000FC2DC File Offset: 0x000FA4DC
		public bool GetInGuiding()
		{
			return this._inGuiding;
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x000FC2F4 File Offset: 0x000FA4F4
		public void SetInGuiding(bool value, DataContext context)
		{
			this._inGuiding = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, this.DataStates, TutorialChapterDomain.CacheInfluences, context);
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x000FC314 File Offset: 0x000FA514
		public short GetTutorialChapter()
		{
			return this._tutorialChapter;
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x000FC32C File Offset: 0x000FA52C
		public void SetTutorialChapter(short value, DataContext context)
		{
			this._tutorialChapter = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(2, this.DataStates, TutorialChapterDomain.CacheInfluences, context);
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x000FC34C File Offset: 0x000FA54C
		public bool GetCanMove()
		{
			return this._canMove;
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x000FC364 File Offset: 0x000FA564
		public void SetCanMove(bool value, DataContext context)
		{
			this._canMove = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, this.DataStates, TutorialChapterDomain.CacheInfluences, context);
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x000FC384 File Offset: 0x000FA584
		public bool GetCanOpenCharacterMenu()
		{
			return this._canOpenCharacterMenu;
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x000FC39C File Offset: 0x000FA59C
		public void SetCanOpenCharacterMenu(bool value, DataContext context)
		{
			this._canOpenCharacterMenu = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, this.DataStates, TutorialChapterDomain.CacheInfluences, context);
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x000FC3BC File Offset: 0x000FA5BC
		public string GetGuidVideoName()
		{
			return this._guidVideoName;
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x000FC3D4 File Offset: 0x000FA5D4
		public void SetGuidVideoName(string value, DataContext context)
		{
			this._guidVideoName = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, this.DataStates, TutorialChapterDomain.CacheInfluences, context);
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x000FC3F4 File Offset: 0x000FA5F4
		public bool GetCanAdvanceMonth()
		{
			return this._canAdvanceMonth;
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x000FC40C File Offset: 0x000FA60C
		public void SetCanAdvanceMonth(bool value, DataContext context)
		{
			this._canAdvanceMonth = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, this.DataStates, TutorialChapterDomain.CacheInfluences, context);
		}

		// Token: 0x06000F43 RID: 3907 RVA: 0x000FC42C File Offset: 0x000FA62C
		public Location GetNextForceLocation()
		{
			return this._nextForceLocation;
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x000FC444 File Offset: 0x000FA644
		public void SetNextForceLocation(Location value, DataContext context)
		{
			this._nextForceLocation = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, this.DataStates, TutorialChapterDomain.CacheInfluences, context);
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x000FC464 File Offset: 0x000FA664
		public bool GetNeiliAllocateFitChapter7()
		{
			return this._neiliAllocateFitChapter7;
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x000FC47C File Offset: 0x000FA67C
		public void SetNeiliAllocateFitChapter7(bool value, DataContext context)
		{
			this._neiliAllocateFitChapter7 = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, this.DataStates, TutorialChapterDomain.CacheInfluences, context);
		}

		// Token: 0x06000F47 RID: 3911 RVA: 0x000FC49C File Offset: 0x000FA69C
		public bool GetHuanxinDying()
		{
			return this._huanxinDying;
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x000FC4B4 File Offset: 0x000FA6B4
		public void SetHuanxinDying(bool value, DataContext context)
		{
			this._huanxinDying = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(9, this.DataStates, TutorialChapterDomain.CacheInfluences, context);
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x000FC4D4 File Offset: 0x000FA6D4
		public bool GetHuanxinSurprised()
		{
			return this._huanxinSurprised;
		}

		// Token: 0x06000F4A RID: 3914 RVA: 0x000FC4EC File Offset: 0x000FA6EC
		public void SetHuanxinSurprised(bool value, DataContext context)
		{
			this._huanxinSurprised = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(10, this.DataStates, TutorialChapterDomain.CacheInfluences, context);
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x000FC50C File Offset: 0x000FA70C
		public bool GetCanShowEnterIndustry()
		{
			return this._canShowEnterIndustry;
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x000FC524 File Offset: 0x000FA724
		public void SetCanShowEnterIndustry(bool value, DataContext context)
		{
			this._canShowEnterIndustry = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(11, this.DataStates, TutorialChapterDomain.CacheInfluences, context);
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x000FC542 File Offset: 0x000FA742
		public override void OnInitializeGameDataModule()
		{
			this.InitializeOnInitializeGameDataModule();
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x000FC54C File Offset: 0x000FA74C
		public override void OnEnterNewWorld()
		{
			this.InitializeOnEnterNewWorld();
			this.InitializeInternalDataOfCollections();
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x000FC55D File Offset: 0x000FA75D
		public override void OnLoadWorld()
		{
			this.InitializeInternalDataOfCollections();
			this.OnLoadedArchiveData();
			DomainManager.Global.CompleteLoading(15);
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x000FC57C File Offset: 0x000FA77C
		public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
		{
			int result;
			switch (dataId)
			{
			case 0:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 0);
				}
				result = Serializer.Serialize(this._curProgress, dataPool);
				break;
			case 1:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 1);
				}
				result = Serializer.Serialize(this._inGuiding, dataPool);
				break;
			case 2:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 2);
				}
				result = Serializer.Serialize(this._tutorialChapter, dataPool);
				break;
			case 3:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 3);
				}
				result = Serializer.Serialize(this._canMove, dataPool);
				break;
			case 4:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 4);
				}
				result = Serializer.Serialize(this._canOpenCharacterMenu, dataPool);
				break;
			case 5:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 5);
				}
				result = Serializer.Serialize(this._guidVideoName, dataPool);
				break;
			case 6:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 6);
				}
				result = Serializer.Serialize(this._canAdvanceMonth, dataPool);
				break;
			case 7:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 7);
				}
				result = Serializer.Serialize(this._nextForceLocation, dataPool);
				break;
			case 8:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 8);
				}
				result = Serializer.Serialize(this._neiliAllocateFitChapter7, dataPool);
				break;
			case 9:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 9);
				}
				result = Serializer.Serialize(this._huanxinDying, dataPool);
				break;
			case 10:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 10);
				}
				result = Serializer.Serialize(this._huanxinSurprised, dataPool);
				break;
			case 11:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 11);
				}
				result = Serializer.Serialize(this._canShowEnterIndustry, dataPool);
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

		// Token: 0x06000F51 RID: 3921 RVA: 0x000FC7D0 File Offset: 0x000FA9D0
		public override void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			switch (dataId)
			{
			case 0:
				Serializer.Deserialize(dataPool, valueOffset, ref this._curProgress);
				this.SetCurProgress(this._curProgress, context);
				break;
			case 1:
				Serializer.Deserialize(dataPool, valueOffset, ref this._inGuiding);
				this.SetInGuiding(this._inGuiding, context);
				break;
			case 2:
				Serializer.Deserialize(dataPool, valueOffset, ref this._tutorialChapter);
				this.SetTutorialChapter(this._tutorialChapter, context);
				break;
			case 3:
				Serializer.Deserialize(dataPool, valueOffset, ref this._canMove);
				this.SetCanMove(this._canMove, context);
				break;
			case 4:
				Serializer.Deserialize(dataPool, valueOffset, ref this._canOpenCharacterMenu);
				this.SetCanOpenCharacterMenu(this._canOpenCharacterMenu, context);
				break;
			case 5:
				Serializer.Deserialize(dataPool, valueOffset, ref this._guidVideoName);
				this.SetGuidVideoName(this._guidVideoName, context);
				break;
			case 6:
				Serializer.Deserialize(dataPool, valueOffset, ref this._canAdvanceMonth);
				this.SetCanAdvanceMonth(this._canAdvanceMonth, context);
				break;
			case 7:
				Serializer.Deserialize(dataPool, valueOffset, ref this._nextForceLocation);
				this.SetNextForceLocation(this._nextForceLocation, context);
				break;
			case 8:
				Serializer.Deserialize(dataPool, valueOffset, ref this._neiliAllocateFitChapter7);
				this.SetNeiliAllocateFitChapter7(this._neiliAllocateFitChapter7, context);
				break;
			case 9:
				Serializer.Deserialize(dataPool, valueOffset, ref this._huanxinDying);
				this.SetHuanxinDying(this._huanxinDying, context);
				break;
			case 10:
				Serializer.Deserialize(dataPool, valueOffset, ref this._huanxinSurprised);
				this.SetHuanxinSurprised(this._huanxinSurprised, context);
				break;
			case 11:
				Serializer.Deserialize(dataPool, valueOffset, ref this._canShowEnterIndustry);
				this.SetCanShowEnterIndustry(this._canShowEnterIndustry, context);
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

		// Token: 0x06000F52 RID: 3922 RVA: 0x000FCA00 File Offset: 0x000FAC00
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
				if (num != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short templateId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref templateId);
				WorldCreationInfo info = default(WorldCreationInfo);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref info);
				int returnValue = this.CreateFixedWorld(context, templateId, info);
				result = Serializer.Serialize(returnValue, returnDataPool);
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
				int chapter = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref chapter);
				this.StartChapter(context, chapter);
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
				Location returnValue2 = this.GetNextForceMoveToLocation();
				result = Serializer.Serialize(returnValue2, returnDataPool);
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

		// Token: 0x06000F53 RID: 3923 RVA: 0x000FCBB4 File Offset: 0x000FADB4
		public override void OnMonitorData(ushort dataId, ulong subId0, uint subId1, bool monitoring)
		{
			switch (dataId)
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
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x000FCC44 File Offset: 0x000FAE44
		public override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
		{
			int result;
			switch (dataId)
			{
			case 0:
			{
				bool flag = !BaseGameDataDomain.IsModified(this.DataStates, 0);
				if (flag)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 0);
					result = Serializer.Serialize(this._curProgress, dataPool);
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
					result = Serializer.Serialize(this._inGuiding, dataPool);
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
					result = Serializer.Serialize(this._tutorialChapter, dataPool);
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
					result = Serializer.Serialize(this._canMove, dataPool);
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
					result = Serializer.Serialize(this._canOpenCharacterMenu, dataPool);
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
					result = Serializer.Serialize(this._guidVideoName, dataPool);
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
					result = Serializer.Serialize(this._canAdvanceMonth, dataPool);
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
					result = Serializer.Serialize(this._nextForceLocation, dataPool);
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
					result = Serializer.Serialize(this._neiliAllocateFitChapter7, dataPool);
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
					result = Serializer.Serialize(this._huanxinDying, dataPool);
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
					result = Serializer.Serialize(this._huanxinSurprised, dataPool);
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
					result = Serializer.Serialize(this._canShowEnterIndustry, dataPool);
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

		// Token: 0x06000F55 RID: 3925 RVA: 0x000FCF98 File Offset: 0x000FB198
		public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			switch (dataId)
			{
			case 0:
			{
				bool flag = !BaseGameDataDomain.IsModified(this.DataStates, 0);
				if (!flag)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 0);
				}
				break;
			}
			case 1:
			{
				bool flag2 = !BaseGameDataDomain.IsModified(this.DataStates, 1);
				if (!flag2)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 1);
				}
				break;
			}
			case 2:
			{
				bool flag3 = !BaseGameDataDomain.IsModified(this.DataStates, 2);
				if (!flag3)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 2);
				}
				break;
			}
			case 3:
			{
				bool flag4 = !BaseGameDataDomain.IsModified(this.DataStates, 3);
				if (!flag4)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 3);
				}
				break;
			}
			case 4:
			{
				bool flag5 = !BaseGameDataDomain.IsModified(this.DataStates, 4);
				if (!flag5)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 4);
				}
				break;
			}
			case 5:
			{
				bool flag6 = !BaseGameDataDomain.IsModified(this.DataStates, 5);
				if (!flag6)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 5);
				}
				break;
			}
			case 6:
			{
				bool flag7 = !BaseGameDataDomain.IsModified(this.DataStates, 6);
				if (!flag7)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 6);
				}
				break;
			}
			case 7:
			{
				bool flag8 = !BaseGameDataDomain.IsModified(this.DataStates, 7);
				if (!flag8)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 7);
				}
				break;
			}
			case 8:
			{
				bool flag9 = !BaseGameDataDomain.IsModified(this.DataStates, 8);
				if (!flag9)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 8);
				}
				break;
			}
			case 9:
			{
				bool flag10 = !BaseGameDataDomain.IsModified(this.DataStates, 9);
				if (!flag10)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 9);
				}
				break;
			}
			case 10:
			{
				bool flag11 = !BaseGameDataDomain.IsModified(this.DataStates, 10);
				if (!flag11)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 10);
				}
				break;
			}
			case 11:
			{
				bool flag12 = !BaseGameDataDomain.IsModified(this.DataStates, 11);
				if (!flag12)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 11);
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

		// Token: 0x06000F56 RID: 3926 RVA: 0x000FD224 File Offset: 0x000FB424
		public override bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			bool result;
			switch (dataId)
			{
			case 0:
				result = BaseGameDataDomain.IsModified(this.DataStates, 0);
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

		// Token: 0x06000F57 RID: 3927 RVA: 0x000FD368 File Offset: 0x000FB568
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

		// Token: 0x06000F58 RID: 3928 RVA: 0x000FD444 File Offset: 0x000FB644
		public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Cannot process archive response of non-archive data ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.DataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x000FD484 File Offset: 0x000FB684
		private void InitializeInternalDataOfCollections()
		{
		}

		// Token: 0x04000216 RID: 534
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private int _curProgress;

		// Token: 0x04000217 RID: 535
		private Dictionary<string, int> _fixedCharacters;

		// Token: 0x04000219 RID: 537
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private bool _inGuiding = false;

		// Token: 0x0400021A RID: 538
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private short _tutorialChapter = -1;

		// Token: 0x0400021B RID: 539
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private bool _canMove = true;

		// Token: 0x0400021C RID: 540
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private bool _canOpenCharacterMenu = true;

		// Token: 0x0400021D RID: 541
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private string _guidVideoName = string.Empty;

		// Token: 0x0400021E RID: 542
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private bool _canAdvanceMonth = true;

		// Token: 0x0400021F RID: 543
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private Location _nextForceLocation = Location.Invalid;

		// Token: 0x04000220 RID: 544
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private bool _neiliAllocateFitChapter7;

		// Token: 0x04000221 RID: 545
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private bool _huanxinDying;

		// Token: 0x04000222 RID: 546
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private bool _huanxinSurprised;

		// Token: 0x04000223 RID: 547
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private bool _canShowEnterIndustry = true;

		// Token: 0x04000224 RID: 548
		public sbyte Counter;

		// Token: 0x04000225 RID: 549
		public sbyte CounterTarget;

		// Token: 0x04000226 RID: 550
		private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[12][];

		// Token: 0x04000227 RID: 551
		private Queue<uint> _pendingLoadingOperationIds;
	}
}
