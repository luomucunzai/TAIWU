using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Config;
using Config.EventConfig;
using GameData.Common;
using GameData.DLC.FiveLoong;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.Display;
using GameData.Domains.Map;
using GameData.Domains.Mod;
using GameData.Domains.TaiwuEvent.DisplayEvent;
using GameData.Domains.TaiwuEvent.Enum;
using GameData.Domains.TaiwuEvent.EventOption;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent
{
	// Token: 0x02000083 RID: 131
	public class TaiwuEvent
	{
		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06001722 RID: 5922 RVA: 0x00155B50 File Offset: 0x00153D50
		public bool IsEmpty
		{
			get
			{
				bool flag = string.IsNullOrEmpty(this.EventGuid);
				bool result;
				if (flag)
				{
					result = true;
				}
				else
				{
					bool flag2 = this.EventGuid == TaiwuEvent.Empty.EventGuid;
					if (flag2)
					{
						result = true;
					}
					else
					{
						bool flag3 = this.EventConfig == null;
						result = flag3;
					}
				}
				return result;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06001723 RID: 5923 RVA: 0x00155BA4 File Offset: 0x00153DA4
		public List<int> NeedNameRelatedDataCharacterIdList
		{
			get
			{
				bool flag = this._needNameRelatedDataCharacterIdList == null;
				if (flag)
				{
					this._needNameRelatedDataCharacterIdList = new List<int>();
				}
				return this._needNameRelatedDataCharacterIdList;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06001724 RID: 5924 RVA: 0x00155BD4 File Offset: 0x00153DD4
		// (set) Token: 0x06001725 RID: 5925 RVA: 0x00155BDC File Offset: 0x00153DDC
		public EventArgBox ArgBox
		{
			get
			{
				return this._argBox;
			}
			set
			{
				bool flag = value == null && DomainManager.TaiwuEvent.IsTriggeredEvent(this.EventGuid);
				if (!flag)
				{
					this._argBox = value;
					bool flag2 = this.EventConfig != null;
					if (flag2)
					{
						this.EventConfig.ArgBox = value;
						for (int i = 0; i < this.EventConfig.EventOptions.Length; i++)
						{
							this.EventConfig.EventOptions[i].ArgBox = value;
						}
					}
				}
			}
		}

		// Token: 0x06001726 RID: 5926 RVA: 0x00155C59 File Offset: 0x00153E59
		public TaiwuEvent()
		{
		}

		// Token: 0x06001727 RID: 5927 RVA: 0x00155C63 File Offset: 0x00153E63
		public TaiwuEvent(TaiwuEvent other)
		{
			this.EventGuid = other.EventGuid;
		}

		// Token: 0x06001728 RID: 5928 RVA: 0x00155C7C File Offset: 0x00153E7C
		public void AddOption(ValueTuple<string, string> optionInfo)
		{
			bool flag = this.ExtendEventOptions == null;
			if (flag)
			{
				this.ExtendEventOptions = new List<ValueTuple<string, string>>();
			}
			for (int i = 0; i < this.ExtendEventOptions.Count; i++)
			{
				ValueTuple<string, string> tuple = this.ExtendEventOptions[i];
				bool flag2 = optionInfo.Item1 == tuple.Item1 && optionInfo.Item2 == tuple.Item2;
				if (flag2)
				{
					return;
				}
			}
			this.ExtendEventOptions.Add(optionInfo);
		}

		// Token: 0x06001729 RID: 5929 RVA: 0x00155D08 File Offset: 0x00153F08
		private void UpdateAvatarGrowableElementToAge(GameData.Domains.Character.Character character, short age, ref AvatarData avatarData)
		{
			ValueTuple<bool, bool> valueTuple = character.IsAbleToGrowBeards(age);
			bool canGrowBeard = valueTuple.Item1;
			bool canGrowBeard2 = valueTuple.Item2;
			avatarData.SetGrowableElementShowingAbility(1, canGrowBeard);
			avatarData.SetGrowableElementShowingAbility(2, canGrowBeard2);
			avatarData.SetGrowableElementShowingAbility(3, GameData.Domains.Character.Character.IsAbleToGrowWrinkle1(age));
			avatarData.SetGrowableElementShowingAbility(4, GameData.Domains.Character.Character.IsAbleToGrowWrinkle2(age));
			avatarData.SetGrowableElementShowingAbility(5, GameData.Domains.Character.Character.IsAbleToGrowWrinkle3(age));
			avatarData.SetGrowableElementShowingState(1, character.IsAbleToGrowAvatarElement(1, age));
			avatarData.SetGrowableElementShowingState(2, character.IsAbleToGrowAvatarElement(2, age));
			avatarData.SetGrowableElementShowingState(3, character.IsAbleToGrowAvatarElement(3, age));
			avatarData.SetGrowableElementShowingState(4, character.IsAbleToGrowAvatarElement(4, age));
			avatarData.SetGrowableElementShowingState(5, character.IsAbleToGrowAvatarElement(5, age));
		}

		// Token: 0x0600172A RID: 5930 RVA: 0x00155DC0 File Offset: 0x00153FC0
		public bool TryExecuteScript(EventScriptRuntime scriptRuntime)
		{
			bool flag = this.EventConfig.Script == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				scriptRuntime.ExecuteScript(this.EventConfig.Script, this.ArgBox);
				string nextEvent = scriptRuntime.Current.NextEvent;
				bool flag2 = nextEvent != null;
				if (flag2)
				{
					DomainManager.TaiwuEvent.ToEvent(nextEvent);
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x0600172B RID: 5931 RVA: 0x00155E28 File Offset: 0x00154028
		public TaiwuEventDisplayData ToDisplayData()
		{
			TaiwuEvent.<>c__DisplayClass18_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			this.EventConfig.TaiwuEvent = this;
			this.NeedNameRelatedDataCharacterIdList.Clear();
			CS$<>8__locals1.data = new TaiwuEventDisplayData();
			CS$<>8__locals1.data.EventGuid = this.EventGuid;
			CS$<>8__locals1.data.EventTexture = this.EventConfig.EventBackground;
			CS$<>8__locals1.data.MaskControlCode = this.EventConfig.MaskControl;
			CS$<>8__locals1.data.MaskTweenTime = (ushort)(this.EventConfig.MaskTweenTime * 100f);
			bool notShowTargetRole = this.CheckEventBoolState("NotShowTargetRole", 4);
			bool notShowMainRole = this.CheckEventBoolState("NotShowMainRole", 3);
			string leftActorKey = string.Empty;
			bool flag = this.ArgBox.Get("ConchShip_PresetKey_LeftActorKey", ref leftActorKey);
			if (flag)
			{
				this.ArgBox.Remove<string>("ConchShip_PresetKey_LeftActorKey");
			}
			else
			{
				bool flag2 = !string.IsNullOrEmpty(this.EventConfig.MainRoleKey) && !notShowMainRole;
				if (flag2)
				{
					bool flag3 = !this.ArgBox.Contains<EventActorData>(this.EventConfig.MainRoleKey);
					if (flag3)
					{
						GameData.Domains.Character.Character character = this.ArgBox.GetCharacter(this.EventConfig.MainRoleKey);
						CS$<>8__locals1.data.MainCharacter = DomainManager.Character.GetCharacterDisplayData(character.GetId());
						short age = CS$<>8__locals1.data.MainCharacter.PhysiologicalAge;
						bool flag4 = this.ArgBox.Get("MainCharacterDisplayAge", ref age);
						if (flag4)
						{
							this.UpdateAvatarGrowableElementToAge(character, age, ref CS$<>8__locals1.data.MainCharacter.AvatarRelatedData.AvatarData);
							CS$<>8__locals1.data.MainCharacter.AvatarRelatedData.DisplayAge = age;
						}
						short clothTemplateId = -1;
						bool flag5 = this.ArgBox.Get("MainCharacterDisplayCloth", ref clothTemplateId);
						if (flag5)
						{
							ClothingItem config = Clothing.Instance.GetItem(clothTemplateId);
							bool flag6 = config != null;
							if (flag6)
							{
								CS$<>8__locals1.data.MainCharacter.AvatarRelatedData.ClothingDisplayId = config.DisplayId;
							}
						}
					}
					else
					{
						leftActorKey = this.EventConfig.MainRoleKey;
					}
				}
			}
			string actorKey = string.Empty;
			bool flag7 = this.ArgBox.Get("ActorKey", ref actorKey);
			if (flag7)
			{
				this.ArgBox.Remove<string>("ActorKey");
			}
			else
			{
				bool flag8 = !string.IsNullOrEmpty(this.EventConfig.TargetRoleKey) && !notShowTargetRole;
				if (flag8)
				{
					bool flag9 = !this.ArgBox.Contains<EventActorData>(this.EventConfig.TargetRoleKey);
					if (flag9)
					{
						GameData.Domains.Character.Character character2 = this.ArgBox.GetCharacter(this.EventConfig.TargetRoleKey);
						bool flag10 = character2 == null;
						if (flag10)
						{
							throw new Exception(this.EventGuid + ":no character key " + this.EventConfig.TargetRoleKey + " in ArgBox!");
						}
						CS$<>8__locals1.data.TargetCharacter = DomainManager.Character.GetCharacterDisplayData(character2.GetId());
						short age2 = CS$<>8__locals1.data.TargetCharacter.PhysiologicalAge;
						bool flag11 = this.ArgBox.Get("TargetCharacterDisplayAge", ref age2);
						if (flag11)
						{
							this.UpdateAvatarGrowableElementToAge(character2, age2, ref CS$<>8__locals1.data.TargetCharacter.AvatarRelatedData.AvatarData);
							CS$<>8__locals1.data.TargetCharacter.AvatarRelatedData.DisplayAge = age2;
						}
						short clothTemplateId2 = -1;
						bool flag12 = this.ArgBox.Get("TargetCharacterDisplayCloth", ref clothTemplateId2);
						if (flag12)
						{
							ClothingItem config2 = Clothing.Instance.GetItem(clothTemplateId2);
							bool flag13 = config2 != null;
							if (flag13)
							{
								CS$<>8__locals1.data.TargetCharacter.AvatarRelatedData.ClothingDisplayId = config2.DisplayId;
							}
						}
						short clothingDisplayId = -1;
						bool flag14 = this.ArgBox.Get("TargetCharacterDisplayingClothId", ref clothingDisplayId);
						if (flag14)
						{
							CS$<>8__locals1.data.TargetCharacter.AvatarRelatedData.ClothingDisplayId = clothingDisplayId;
						}
					}
					else
					{
						actorKey = this.EventConfig.TargetRoleKey;
					}
				}
			}
			bool flag15 = string.IsNullOrEmpty(this.EventConfig.EventBackground);
			if (flag15)
			{
				string specifyEventBackground = string.Empty;
				bool flag16 = !string.IsNullOrEmpty(DomainManager.TaiwuEvent.SeriesEventTexture);
				if (flag16)
				{
					CS$<>8__locals1.data.EventTexture = DomainManager.TaiwuEvent.SeriesEventTexture;
				}
				else
				{
					bool flag17 = this.ArgBox.Get("ConchShip_PresetKey_SpecifyEventBackground", ref specifyEventBackground) && !string.IsNullOrEmpty(specifyEventBackground);
					if (flag17)
					{
						CS$<>8__locals1.data.EventTexture = specifyEventBackground;
						this.ArgBox.Remove<string>("ConchShip_PresetKey_SpecifyEventBackground");
					}
					else
					{
						bool isTraveling = DomainManager.Map.IsTraveling;
						MapBlockData block;
						if (isTraveling)
						{
							Location location = DomainManager.Map.GetTravelCurrLocation();
							block = DomainManager.Map.GetBlock(location);
						}
						else
						{
							block = DomainManager.Map.GetBlock(EventArgBox.TaiwuAreaId, EventArgBox.TaiwuBlockId);
						}
						CS$<>8__locals1.data.EventTexture = block.GetConfig().EventBack;
					}
				}
			}
			else
			{
				bool flag18 = this.EventConfig.EventType == EEventType.ModEvent;
				if (flag18)
				{
					string modDirRoot = DomainManager.Mod.GetModDirectory(this.EventConfig.Package.ModIdString);
					bool flag19 = string.IsNullOrEmpty(modDirRoot);
					if (flag19)
					{
						CS$<>8__locals1.data.EventTexture = Path.Combine(this.EventConfig.Package.ModIdString, "../EventTextures/" + this.EventConfig.EventBackground + ".png").Replace("\\", "/");
					}
					else
					{
						CS$<>8__locals1.data.EventTexture = Path.Combine(modDirRoot, "Events/EventTextures/" + this.EventConfig.EventBackground + ".png").Replace("\\", "/");
					}
				}
			}
			CS$<>8__locals1.data.EventContent = this.EventConfig.GetReplacedContentString();
			bool flag20 = string.IsNullOrEmpty(CS$<>8__locals1.data.EventContent);
			if (flag20)
			{
				CS$<>8__locals1.data.EventContent = TaiwuEventTagHandler.DecodeTag(this.EventConfig.EventContent, this.ArgBox, this);
			}
			CS$<>8__locals1.data.ExtraFormatLanguageKeys = this.EventConfig.GetExtraFormatLanguageKeys();
			CS$<>8__locals1.data.EscOptionIndex = -1;
			CS$<>8__locals1.data.EventOptionInfos = new List<EventOptionInfo>();
			for (int i = 0; i < this.EventConfig.EventOptions.Length; i++)
			{
				this.<ToDisplayData>g__HandleOption|18_0(this.EventConfig.EventOptions[i], this, ref CS$<>8__locals1);
			}
			for (int j = 0; j < this.ExtendEventOptions.Count; j++)
			{
				ValueTuple<string, string> tuple = this.ExtendEventOptions[j];
				TaiwuEvent eventData = DomainManager.TaiwuEvent.GetEvent(tuple.Item1);
				bool flag21 = eventData != null;
				if (flag21)
				{
					eventData.ArgBox = this.ArgBox;
					TaiwuEventOption option = eventData.EventConfig[tuple.Item2];
					this.<ToDisplayData>g__HandleOption|18_0(option, eventData, ref CS$<>8__locals1);
				}
			}
			bool flag22 = CS$<>8__locals1.data.EventOptionInfos.Count <= 0;
			if (flag22)
			{
				throw new Exception("event " + CS$<>8__locals1.data.EventGuid + " failed to display cause no option!");
			}
			bool flag23 = CS$<>8__locals1.data.EventOptionInfos.Count > 36;
			if (flag23)
			{
				throw new Exception("event " + CS$<>8__locals1.data.EventGuid + " failed to display cause too many options in event!");
			}
			bool flag24 = this.CheckEventBoolState("ShuffleOptions", 0);
			if (flag24)
			{
				CollectionUtils.Shuffle<EventOptionInfo>(DomainManager.TaiwuEvent.MainThreadDataContext.Random, CS$<>8__locals1.data.EventOptionInfos);
			}
			bool flag25 = !string.IsNullOrEmpty(this.EventConfig.EscOptionKey);
			if (flag25)
			{
				CS$<>8__locals1.data.EscOptionIndex = (sbyte)(CS$<>8__locals1.data.EventOptionInfos.Count - 1);
				sbyte k = 0;
				while ((int)k < CS$<>8__locals1.data.EventOptionInfos.Count)
				{
					bool flag26 = CS$<>8__locals1.data.EventOptionInfos[(int)k].OptionKey == this.EventConfig.EscOptionKey;
					if (flag26)
					{
						EventOptionInfo escOption = CS$<>8__locals1.data.EventOptionInfos[(int)k];
						CS$<>8__locals1.data.EventOptionInfos.RemoveAt((int)k);
						CS$<>8__locals1.data.EventOptionInfos.Add(escOption);
						break;
					}
					k += 1;
				}
			}
			bool flag27 = this.NeedNameRelatedDataCharacterIdList.Count > 0;
			if (flag27)
			{
				CS$<>8__locals1.data.NameDecodeDataList = new List<TaiwuEventCharacterNameDecodeData>();
				List<NameRelatedData> list = DomainManager.Character.GetNameRelatedDataList(this.NeedNameRelatedDataCharacterIdList);
				int l = 0;
				int max = this.NeedNameRelatedDataCharacterIdList.Count;
				while (l < max)
				{
					int charId = this.NeedNameRelatedDataCharacterIdList[l];
					CS$<>8__locals1.data.NameDecodeDataList.Add(new TaiwuEventCharacterNameDecodeData
					{
						CharacterId = charId,
						NameRelatedData = list[l]
					});
					l++;
				}
				this.NeedNameRelatedDataCharacterIdList.Clear();
			}
			else
			{
				CS$<>8__locals1.data.NameDecodeDataList = null;
			}
			this._needNameRelatedDataCharacterIdList = null;
			CS$<>8__locals1.data.ExtraData = new TaiwuEventDisplayExtraData();
			CS$<>8__locals1.data.ExtraData.HereticTemplateId = -1;
			CS$<>8__locals1.data.ExtraData.ForbidViewCharacter = this.CheckEventBoolState("ForbidViewCharacter", 6);
			CS$<>8__locals1.data.ExtraData.ForbidViewSelf = this.CheckEventBoolState("ForbidViewSelf", 5);
			CS$<>8__locals1.data.ExtraData.HideRightFavorability = this.CheckEventBoolState("HideFavorability", 8);
			CS$<>8__locals1.data.ExtraData.HideLeftFavorability = this.CheckEventBoolState("ConchShip_PresetKey_HideLeftFavorability", 7);
			CS$<>8__locals1.data.ExtraData.TargetRoleUseAlternativeName = this.CheckEventBoolState("TargetRoleUseAlternativeName", 2);
			CS$<>8__locals1.data.ExtraData.MainRoleUseAlternativeName = this.CheckEventBoolState("MainRoleUseAlternativeName", 1);
			CS$<>8__locals1.data.ExtraData.RightCharacterShadow = this.CheckEventBoolState("ConchShip_PresetKey_RightCharacterShadow", 13);
			CS$<>8__locals1.data.ExtraData.RightForbiddenConsummateLevel = this.CheckEventBoolState("ConchShip_PresetKey_RightForbiddenConsummateLevel", 14);
			int caravanId = -1;
			bool flag28 = this.ArgBox.Get("CaravanId", ref caravanId);
			if (flag28)
			{
				CS$<>8__locals1.data.ExtraData.CaravanData = DomainManager.Merchant.GetCaravanDisplayData(DomainManager.TaiwuEvent.MainThreadDataContext, caravanId);
			}
			else
			{
				CS$<>8__locals1.data.ExtraData.CaravanData = null;
			}
			int jiaoId = -1;
			GameData.DLC.FiveLoong.Jiao jiao;
			bool flag29 = this.ArgBox.Get("JiaoId", ref jiaoId) && DomainManager.Extra.TryGetJiao(jiaoId, out jiao);
			if (flag29)
			{
				CS$<>8__locals1.data.ExtraData.JiaoDisplayData = DomainManager.Item.GetItemDisplayData(jiao.Key, -1);
			}
			else
			{
				CS$<>8__locals1.data.ExtraData.JiaoDisplayData = null;
			}
			short targetCharacterTemplateId = -1;
			bool flag30 = this.ArgBox.Get("TargetCharacterTemplateId", ref targetCharacterTemplateId);
			if (flag30)
			{
				CS$<>8__locals1.data.ExtraData.HereticTemplateId = targetCharacterTemplateId;
				CS$<>8__locals1.data.ExtraData.ForbidViewCharacter = true;
				CS$<>8__locals1.data.ExtraData.HideRightFavorability = true;
				this.ArgBox.Remove<short>("TargetCharacterTemplateId");
			}
			bool flag31 = !string.IsNullOrEmpty(leftActorKey);
			if (flag31)
			{
				this.ArgBox.Get<EventActorData>(leftActorKey, out CS$<>8__locals1.data.ExtraData.LeftActorData);
				CS$<>8__locals1.data.ExtraData.ForbidViewSelf = true;
				CS$<>8__locals1.data.ExtraData.LeftActorShowMarriageLook1 = this.CheckEventBoolState(string.Empty, 17);
				CS$<>8__locals1.data.ExtraData.LeftActorShowMarriageLook2 = this.CheckEventBoolState(string.Empty, 18);
			}
			bool flag32 = !string.IsNullOrEmpty(actorKey);
			if (flag32)
			{
				this.ArgBox.Get<EventActorData>(actorKey, out CS$<>8__locals1.data.ExtraData.ActorData);
				CS$<>8__locals1.data.ExtraData.ForbidViewCharacter = true;
				CS$<>8__locals1.data.ExtraData.RightActorShowMarriageLook1 = this.CheckEventBoolState(string.Empty, 19);
				CS$<>8__locals1.data.ExtraData.RightActorShowMarriageLook2 = this.CheckEventBoolState(string.Empty, 20);
			}
			EventSelectItemData selectItemData;
			bool flag33 = this.ArgBox.Get<EventSelectItemData>("SelectItemInfo", out selectItemData);
			if (flag33)
			{
				CS$<>8__locals1.data.ExtraData.SelectItemData = selectItemData;
			}
			else
			{
				CS$<>8__locals1.data.ExtraData.SelectItemData = null;
			}
			EventSelectReadingBookCountData selectReadingBookCountData;
			bool flag34 = this.ArgBox.Get<EventSelectReadingBookCountData>("SelectReadingBookCount", out selectReadingBookCountData);
			if (flag34)
			{
				CS$<>8__locals1.data.ExtraData.SelectReadingBookCountData = selectReadingBookCountData;
			}
			else
			{
				CS$<>8__locals1.data.ExtraData.SelectReadingBookCountData = null;
			}
			EventSelectNeigongLoopingCountData selectNeigongLoopingCount;
			bool flag35 = this.ArgBox.Get<EventSelectNeigongLoopingCountData>("SelectNeigongLoopingCount", out selectNeigongLoopingCount);
			if (flag35)
			{
				CS$<>8__locals1.data.ExtraData.SelectNeigongLoopingCountData = selectNeigongLoopingCount;
			}
			else
			{
				CS$<>8__locals1.data.ExtraData.SelectNeigongLoopingCountData = null;
			}
			EventSelectFuyuFaithCountData selectFuyuFaithCountData;
			bool flag36 = this.ArgBox.Get<EventSelectFuyuFaithCountData>("SelectFuyuFaithCount", out selectFuyuFaithCountData);
			if (flag36)
			{
				CS$<>8__locals1.data.ExtraData.SelectFuyuFaithCountData = selectFuyuFaithCountData;
			}
			else
			{
				CS$<>8__locals1.data.ExtraData.SelectFuyuFaithCountData = null;
			}
			EventSelectFameData selectFameData;
			bool flag37 = this.ArgBox.Get<EventSelectFameData>("SelectFameData", out selectFameData);
			if (flag37)
			{
				CS$<>8__locals1.data.ExtraData.SelectFameData = selectFameData;
			}
			else
			{
				CS$<>8__locals1.data.ExtraData.SelectFameData = null;
			}
			EventSelectCharacterData selectCharacterData;
			bool flag38 = this.ArgBox.Get<EventSelectCharacterData>("SelectCharacterData", out selectCharacterData);
			if (flag38)
			{
				CS$<>8__locals1.data.ExtraData.SelectCharacterData = selectCharacterData;
			}
			else
			{
				CS$<>8__locals1.data.ExtraData.SelectCharacterData = null;
			}
			EventInputRequestData inputRequestData;
			bool flag39 = this.ArgBox.Get<EventInputRequestData>("InputRequestData", out inputRequestData);
			if (flag39)
			{
				CS$<>8__locals1.data.ExtraData.InputRequestData = inputRequestData;
			}
			else
			{
				CS$<>8__locals1.data.ExtraData.InputRequestData = null;
			}
			bool selectAvatarFlag = false;
			bool flag40 = this.ArgBox.Get("SelectAvatarEvent", ref selectAvatarFlag) && selectAvatarFlag;
			if (flag40)
			{
				CS$<>8__locals1.data.ExtraData.SelectOneAvatarRelatedDataList = new List<AvatarRelatedData>();
				bool flag41 = -1 != CS$<>8__locals1.data.EscOptionIndex;
				if (flag41)
				{
					List<EventOptionInfo> eventOptionInfos = CS$<>8__locals1.data.EventOptionInfos;
					int escOptionIndex = (int)CS$<>8__locals1.data.EscOptionIndex;
					List<EventOptionInfo> eventOptionInfos2 = CS$<>8__locals1.data.EventOptionInfos;
					int index = eventOptionInfos2.Count - 1;
					List<EventOptionInfo> eventOptionInfos3 = CS$<>8__locals1.data.EventOptionInfos;
					EventOptionInfo value = eventOptionInfos3[eventOptionInfos3.Count - 1];
					EventOptionInfo value2 = CS$<>8__locals1.data.EventOptionInfos[(int)CS$<>8__locals1.data.EscOptionIndex];
					eventOptionInfos[escOptionIndex] = value;
					eventOptionInfos2[index] = value2;
					CS$<>8__locals1.data.EscOptionIndex = (sbyte)(CS$<>8__locals1.data.EventOptionInfos.Count - 1);
				}
				for (int m = 0; m < CS$<>8__locals1.data.EventOptionInfos.Count; m++)
				{
					AvatarRelatedData avatarRelatedData;
					this.ArgBox.Get<AvatarRelatedData>(CS$<>8__locals1.data.EventOptionInfos[m].OptionKey, out avatarRelatedData);
					bool flag42 = avatarRelatedData == null;
					if (flag42)
					{
						throw new Exception(this.EventGuid + "'s option " + CS$<>8__locals1.data.EventOptionInfos[m].OptionKey + ", not set an avatarRelatedData!");
					}
					CS$<>8__locals1.data.ExtraData.SelectOneAvatarRelatedDataList.Add(avatarRelatedData);
				}
				this.ArgBox.Remove<bool>("SelectAvatarEvent");
			}
			CS$<>8__locals1.data.ExtraData.MainRoleShyFlag = this.CheckEventBoolState("ConchShip_PresetKey_MainRoleShowBlush", 9);
			CS$<>8__locals1.data.ExtraData.TargetRoleShyFlag = this.CheckEventBoolState("ConchShip_PresetKey_TargetRoleShowBlush", 10);
			short mainRoleClothAdjustId = -1;
			this.ArgBox.Get("ConchShip_PresetKey_MainRoleAdjustClothId", ref mainRoleClothAdjustId);
			this.ArgBox.Remove<short>("ConchShip_PresetKey_MainRoleAdjustClothId");
			CS$<>8__locals1.data.ExtraData.MainRoleAdjustClothDisplayId = mainRoleClothAdjustId;
			short targetRoleClothAdjustId = -1;
			this.ArgBox.Get("ConchShip_PresetKey_TargetRoleAdjustClothId", ref targetRoleClothAdjustId);
			this.ArgBox.Remove<short>("ConchShip_PresetKey_TargetRoleAdjustClothId");
			CS$<>8__locals1.data.ExtraData.TargetRoleAdjustClothDisplayId = targetRoleClothAdjustId;
			CS$<>8__locals1.data.ExtraData.LeftRoleShowInjuryInfo = this.CheckEventBoolState("ConchShip_PresetKey_LeftRoleShowInjuryInfo", 11);
			CS$<>8__locals1.data.ExtraData.RightRoleShowInjuryInfo = this.CheckEventBoolState("ConchShip_PresetKey_RightRoleShowInjuryInfo", 12);
			CS$<>8__locals1.data.ExtraData.LeftForbidShowFavorChangeEffect = this.CheckEventBoolState("CS_PK_LeftForbidShowFavorChangeEffect", 15);
			CS$<>8__locals1.data.ExtraData.RightForbidShowFavorChangeEffect = this.CheckEventBoolState("CS_PK_RightForbidShowFavorChangeEffect", 16);
			return CS$<>8__locals1.data;
		}

		// Token: 0x0600172C RID: 5932 RVA: 0x00156F08 File Offset: 0x00155108
		private bool CheckEventBoolState(string key, short templateId)
		{
			bool value = false;
			this.ArgBox.Get(key, ref value);
			TaiwuEventItem eventConfig = this.EventConfig;
			if (eventConfig.BoolStateDict == null)
			{
				eventConfig.BoolStateDict = new Dictionary<short, EventBoolStateInfo>();
			}
			EventBoolStateInfo eventBoolStateInfo;
			this.EventConfig.BoolStateDict.TryGetValue(templateId, out eventBoolStateInfo);
			bool flag = eventBoolStateInfo == null;
			if (flag)
			{
				EventBoolStateItem config = EventBoolState.Instance[templateId];
				bool removeBeforeNextEvent = config.RemoveBeforeNextEvent;
				if (removeBeforeNextEvent)
				{
					this.ArgBox.Remove<bool>(key);
				}
			}
			else
			{
				bool removeBeforeNextEvent2 = eventBoolStateInfo.RemoveBeforeNextEvent;
				if (removeBeforeNextEvent2)
				{
					this.ArgBox.Remove<bool>(key);
				}
			}
			return value || (eventBoolStateInfo != null && eventBoolStateInfo.BoolState);
		}

		// Token: 0x0600172D RID: 5933 RVA: 0x00156FBC File Offset: 0x001551BC
		public TaiwuEventSummaryDisplayData ToSummaryDisplayData()
		{
			TaiwuEventSummaryDisplayData data = new TaiwuEventSummaryDisplayData();
			data.EventGuid = this.EventGuid;
			bool flag = string.IsNullOrEmpty(this.EventConfig.TargetRoleKey);
			if (flag)
			{
				throw new Exception("can not to summary display data because EventConfig.TargetRoleKey has not been set!");
			}
			GameData.Domains.Character.Character character = this.ArgBox.GetCharacter(this.EventConfig.TargetRoleKey);
			bool flag2 = character == null;
			TaiwuEventSummaryDisplayData result;
			if (flag2)
			{
				result = null;
			}
			else
			{
				data.CharacterId = character.GetId();
				result = data;
			}
			return result;
		}

		// Token: 0x0600172E RID: 5934 RVA: 0x00157034 File Offset: 0x00155234
		public void SetModInt(string dataName, bool isArchive, int val)
		{
			bool flag = !this.CheckModDataValid(true);
			if (!flag)
			{
				DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
				DomainManager.Mod.SetInt(context, this.EventConfig.Package.ModIdString, dataName, isArchive, val);
			}
		}

		// Token: 0x0600172F RID: 5935 RVA: 0x0015707C File Offset: 0x0015527C
		public void SetModBool(string dataName, bool isArchive, bool val)
		{
			bool flag = !this.CheckModDataValid(true);
			if (!flag)
			{
				DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
				DomainManager.Mod.SetBool(context, this.EventConfig.Package.ModIdString, dataName, isArchive, val);
			}
		}

		// Token: 0x06001730 RID: 5936 RVA: 0x001570C4 File Offset: 0x001552C4
		public void SetModFloat(string dataName, bool isArchive, float val)
		{
			bool flag = !this.CheckModDataValid(true);
			if (!flag)
			{
				DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
				DomainManager.Mod.SetFloat(context, this.EventConfig.Package.ModIdString, dataName, isArchive, val);
			}
		}

		// Token: 0x06001731 RID: 5937 RVA: 0x0015710C File Offset: 0x0015530C
		public void SetModString(string dataName, bool isArchive, string val)
		{
			bool flag = !this.CheckModDataValid(true);
			if (!flag)
			{
				DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
				DomainManager.Mod.SetString(context, this.EventConfig.Package.ModIdString, dataName, isArchive, val);
			}
		}

		// Token: 0x06001732 RID: 5938 RVA: 0x00157154 File Offset: 0x00155354
		public void SetSerializableModData(string dataName, bool isArchive, SerializableModData val)
		{
			bool flag = !this.CheckModDataValid(true);
			if (!flag)
			{
				DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
				DomainManager.Mod.SetSerializableModData(context, this.EventConfig.Package.ModIdString, dataName, isArchive, val);
			}
		}

		// Token: 0x06001733 RID: 5939 RVA: 0x0015719C File Offset: 0x0015539C
		public void RemoveModData(string dataName)
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			DomainManager.Mod.RemoveData(context, this.EventConfig.Package.ModIdString, dataName);
		}

		// Token: 0x06001734 RID: 5940 RVA: 0x001571D4 File Offset: 0x001553D4
		public bool RemoveModInt(string dataName, bool isArchive)
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			return DomainManager.Mod.RemoveInt(context, this.EventConfig.Package.ModIdString, dataName, isArchive);
		}

		// Token: 0x06001735 RID: 5941 RVA: 0x00157210 File Offset: 0x00155410
		public bool RemoveModBool(string dataName, bool isArchive)
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			return DomainManager.Mod.RemoveBool(context, this.EventConfig.Package.ModIdString, dataName, isArchive);
		}

		// Token: 0x06001736 RID: 5942 RVA: 0x0015724C File Offset: 0x0015544C
		public bool RemoveModFloat(string dataName, bool isArchive)
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			return DomainManager.Mod.RemoveFloat(context, this.EventConfig.Package.ModIdString, dataName, isArchive);
		}

		// Token: 0x06001737 RID: 5943 RVA: 0x00157288 File Offset: 0x00155488
		public bool RemoveModString(string dataName, bool isArchive)
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			return DomainManager.Mod.RemoveString(context, this.EventConfig.Package.ModIdString, dataName, isArchive);
		}

		// Token: 0x06001738 RID: 5944 RVA: 0x001572C4 File Offset: 0x001554C4
		public bool RemoveSerializableModData(string dataName, bool isArchive)
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			return DomainManager.Mod.RemoveSerializableModData(context, this.EventConfig.Package.ModIdString, dataName, isArchive);
		}

		// Token: 0x06001739 RID: 5945 RVA: 0x00157300 File Offset: 0x00155500
		public bool GetModData(string dataName, bool isArchive, ref int val)
		{
			bool flag = !this.CheckModDataValid(true);
			return !flag && DomainManager.Mod.TryGet(this.EventConfig.Package.ModIdString, dataName, isArchive, out val);
		}

		// Token: 0x0600173A RID: 5946 RVA: 0x00157344 File Offset: 0x00155544
		public bool GetModData(string dataName, bool isArchive, ref bool val)
		{
			bool flag = !this.CheckModDataValid(true);
			return !flag && DomainManager.Mod.TryGet(this.EventConfig.Package.ModIdString, dataName, isArchive, out val);
		}

		// Token: 0x0600173B RID: 5947 RVA: 0x00157388 File Offset: 0x00155588
		public bool GetModData(string dataName, bool isArchive, ref float val)
		{
			bool flag = !this.CheckModDataValid(true);
			return !flag && DomainManager.Mod.TryGet(this.EventConfig.Package.ModIdString, dataName, isArchive, out val);
		}

		// Token: 0x0600173C RID: 5948 RVA: 0x001573CC File Offset: 0x001555CC
		public bool GetModData(string dataName, bool isArchive, ref string val)
		{
			bool flag = !this.CheckModDataValid(true);
			return !flag && DomainManager.Mod.TryGet(this.EventConfig.Package.ModIdString, dataName, isArchive, out val);
		}

		// Token: 0x0600173D RID: 5949 RVA: 0x00157410 File Offset: 0x00155610
		public bool GetModData(string dataName, bool isArchive, ref SerializableModData val)
		{
			bool flag = !this.CheckModDataValid(true);
			return !flag && DomainManager.Mod.TryGet(this.EventConfig.Package.ModIdString, dataName, isArchive, out val);
		}

		// Token: 0x0600173E RID: 5950 RVA: 0x00157454 File Offset: 0x00155654
		public bool CheckModDataValid(bool appendWarning = true)
		{
			bool flag = string.IsNullOrEmpty(DomainManager.Mod.GetModDirectory(this.EventConfig.Package.ModIdString));
			bool result;
			if (flag)
			{
				string tag = "TaiwuEvent";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Unable to find mod ");
				defaultInterpolatedStringHandler.AppendFormatted(this.EventConfig.Package.ModIdString);
				defaultInterpolatedStringHandler.AppendLiteral(" with package group ");
				defaultInterpolatedStringHandler.AppendFormatted(this.EventConfig.Package.Group);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), appendWarning);
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06001740 RID: 5952 RVA: 0x00157534 File Offset: 0x00155734
		[CompilerGenerated]
		private void <ToDisplayData>g__HandleOption|18_0(TaiwuEventOption option, TaiwuEvent eventData, ref TaiwuEvent.<>c__DisplayClass18_0 A_3)
		{
			bool isVisible = option.IsVisible;
			if (isVisible)
			{
				EventOptionInfo optionInfo = default(EventOptionInfo);
				optionInfo.OptionKey = option.OptionKey;
				optionInfo.Behavior = option.Behavior;
				Func<string> getReplacedContent = option.GetReplacedContent;
				optionInfo.OptionContent = ((getReplacedContent != null) ? getReplacedContent() : null);
				bool flag = string.IsNullOrEmpty(optionInfo.OptionContent);
				if (flag)
				{
					optionInfo.OptionContent = TaiwuEventTagHandler.DecodeTag(option.OptionContent, this.ArgBox, this);
				}
				Func<List<string>> getExtraFormatLanguageKeys = option.GetExtraFormatLanguageKeys;
				optionInfo.ExtraFormatLanguageKeys = ((getExtraFormatLanguageKeys != null) ? getExtraFormatLanguageKeys() : null);
				List<TaiwuEventOptionConditionBase> optionAvailableConditions = option.OptionAvailableConditions;
				bool flag2 = optionAvailableConditions != null && optionAvailableConditions.Count > 0;
				if (flag2)
				{
					optionInfo.OptionAvailableConditions = new List<OptionAvailableInfo>();
					bool finalState = true;
					for (int i = 0; i < option.OptionAvailableConditions.Count; i++)
					{
						OptionAvailableInfo info = default(OptionAvailableInfo);
						TaiwuEventOptionConditionBase condition = option.OptionAvailableConditions[i];
						bool flag3 = condition.OrConditionCore != null && condition.OrConditionCore.Count > 0;
						if (flag3)
						{
							info.Data = new OptionAvailableInfoMinimumElement[condition.OrConditionCore.Count];
							for (int j = 0; j < condition.OrConditionCore.Count; j++)
							{
								OptionAvailableInfoMinimumElement element = default(OptionAvailableInfoMinimumElement);
								OptionConditionModifier.ModifyCondition(ref element, condition.OrConditionCore[i], this.ArgBox);
								info.PassState = (info.PassState || element.Pass);
								info.Hide = (info.Hide || element.Hide);
								info.Data[i] = element;
							}
						}
						else
						{
							OptionAvailableInfoMinimumElement element2 = default(OptionAvailableInfoMinimumElement);
							OptionConditionModifier.ModifyCondition(ref element2, condition, this.ArgBox);
							info.PassState = (info.PassState || element2.Pass);
							info.Hide = (info.Hide || element2.Hide);
							info.Data = new OptionAvailableInfoMinimumElement[]
							{
								element2
							};
						}
						bool flag4 = !info.Hide;
						if (flag4)
						{
							optionInfo.OptionAvailableConditions.Add(info);
						}
						finalState = (finalState && info.PassState);
					}
					bool flag5 = !finalState;
					if (flag5)
					{
						optionInfo.OptionState = -1;
					}
				}
				bool flag6 = !option.CheckAvailableConditionsFromCode();
				if (flag6)
				{
					optionInfo.OptionState = -1;
				}
				EventScriptRuntime runtime = DomainManager.TaiwuEvent.ScriptRuntime;
				runtime.StartRecordConditionHints();
				bool flag7 = !option.CheckAvailableConditionsFromScript();
				if (flag7)
				{
					optionInfo.OptionState = -1;
				}
				optionInfo.OptionAvailableConditionInfos = runtime.StopRecordConditionHints();
				bool flag8 = optionInfo.OptionState != -1;
				if (flag8)
				{
					bool flag9 = option.DefaultState == 1 && option.WasSelected;
					if (flag9)
					{
						optionInfo.OptionState = 2;
					}
					else
					{
						optionInfo.OptionState = option.DefaultState;
					}
				}
				bool flag10 = !this.ArgBox.Get(option.OptionKey + "_Type", ref optionInfo.OptionType);
				if (flag10)
				{
					optionInfo.OptionType = -1;
				}
				bool flag11 = option.OptionConsumeInfos != null;
				if (flag11)
				{
					optionInfo.OptionConsumeInfos = new List<OptionConsumeInfo>();
					for (int k = 0; k < option.OptionConsumeInfos.Count; k++)
					{
						OptionConsumeInfo consumeInfo = OptionConsumeHelper.ModifyOptionConsumeInfo(option.OptionConsumeInfos[k], this.ArgBox);
						GameData.Domains.Character.Character characterA = option.ArgBox.GetCharacter("RoleTaiwu");
						GameData.Domains.Character.Character characterB = null;
						bool flag12 = !string.IsNullOrEmpty(this.EventConfig.TargetRoleKey);
						if (flag12)
						{
							characterB = option.ArgBox.GetCharacter(this.EventConfig.TargetRoleKey);
						}
						bool hasEnough = consumeInfo.HasConsumeResource(characterA.GetId(), (characterB != null) ? characterB.GetId() : -1);
						consumeInfo.HasEnough = hasEnough;
						consumeInfo.HoldCount = consumeInfo.GetHoldCount(characterA.GetId(), (characterB != null) ? characterB.GetId() : -1);
						optionInfo.OptionConsumeInfos.Add(consumeInfo);
						bool flag13 = !hasEnough;
						if (flag13)
						{
							optionInfo.OptionState = -1;
						}
					}
				}
				A_3.data.EventOptionInfos.Add(optionInfo);
			}
			else
			{
				eventData.ArgBox = null;
			}
		}

		// Token: 0x0400052D RID: 1325
		public static readonly TaiwuEvent Empty = new TaiwuEvent
		{
			EventGuid = Guid.Empty.ToString(),
			EventConfig = null,
			ArgBox = null
		};

		// Token: 0x0400052E RID: 1326
		private List<int> _needNameRelatedDataCharacterIdList;

		// Token: 0x0400052F RID: 1327
		public string EventGuid;

		// Token: 0x04000530 RID: 1328
		public TaiwuEventItem EventConfig;

		// Token: 0x04000531 RID: 1329
		private EventArgBox _argBox;

		// Token: 0x04000532 RID: 1330
		public List<ValueTuple<string, string>> ExtendEventOptions;
	}
}
