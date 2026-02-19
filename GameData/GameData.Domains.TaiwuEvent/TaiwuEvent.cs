using System;
using System.Collections.Generic;
using System.IO;
using Config;
using Config.EventConfig;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.Display;
using GameData.Domains.Map;
using GameData.Domains.Mod;
using GameData.Domains.TaiwuEvent.DisplayEvent;
using GameData.Domains.TaiwuEvent.Enum;
using GameData.Domains.TaiwuEvent.EventOption;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent;

public class TaiwuEvent
{
	public static readonly TaiwuEvent Empty = new TaiwuEvent
	{
		EventGuid = Guid.Empty.ToString(),
		EventConfig = null,
		ArgBox = null
	};

	private List<int> _needNameRelatedDataCharacterIdList;

	public string EventGuid;

	public TaiwuEventItem EventConfig;

	private EventArgBox _argBox;

	public List<(string, string)> ExtendEventOptions;

	public bool IsEmpty
	{
		get
		{
			if (string.IsNullOrEmpty(EventGuid))
			{
				return true;
			}
			if (EventGuid == Empty.EventGuid)
			{
				return true;
			}
			if (EventConfig == null)
			{
				return true;
			}
			return false;
		}
	}

	public List<int> NeedNameRelatedDataCharacterIdList
	{
		get
		{
			if (_needNameRelatedDataCharacterIdList == null)
			{
				_needNameRelatedDataCharacterIdList = new List<int>();
			}
			return _needNameRelatedDataCharacterIdList;
		}
	}

	public EventArgBox ArgBox
	{
		get
		{
			return _argBox;
		}
		set
		{
			if (value == null && DomainManager.TaiwuEvent.IsTriggeredEvent(EventGuid))
			{
				return;
			}
			_argBox = value;
			if (EventConfig != null)
			{
				EventConfig.ArgBox = value;
				for (int i = 0; i < EventConfig.EventOptions.Length; i++)
				{
					EventConfig.EventOptions[i].ArgBox = value;
				}
			}
		}
	}

	public TaiwuEvent()
	{
	}

	public TaiwuEvent(TaiwuEvent other)
	{
		EventGuid = other.EventGuid;
	}

	public void AddOption((string, string) optionInfo)
	{
		if (ExtendEventOptions == null)
		{
			ExtendEventOptions = new List<(string, string)>();
		}
		for (int i = 0; i < ExtendEventOptions.Count; i++)
		{
			(string, string) tuple = ExtendEventOptions[i];
			if (optionInfo.Item1 == tuple.Item1 && optionInfo.Item2 == tuple.Item2)
			{
				return;
			}
		}
		ExtendEventOptions.Add(optionInfo);
	}

	private void UpdateAvatarGrowableElementToAge(GameData.Domains.Character.Character character, short age, ref AvatarData avatarData)
	{
		var (showable, showable2) = character.IsAbleToGrowBeards(age);
		avatarData.SetGrowableElementShowingAbility(1, showable);
		avatarData.SetGrowableElementShowingAbility(2, showable2);
		avatarData.SetGrowableElementShowingAbility(3, GameData.Domains.Character.Character.IsAbleToGrowWrinkle1(age));
		avatarData.SetGrowableElementShowingAbility(4, GameData.Domains.Character.Character.IsAbleToGrowWrinkle2(age));
		avatarData.SetGrowableElementShowingAbility(5, GameData.Domains.Character.Character.IsAbleToGrowWrinkle3(age));
		avatarData.SetGrowableElementShowingState(1, character.IsAbleToGrowAvatarElement(1, age));
		avatarData.SetGrowableElementShowingState(2, character.IsAbleToGrowAvatarElement(2, age));
		avatarData.SetGrowableElementShowingState(3, character.IsAbleToGrowAvatarElement(3, age));
		avatarData.SetGrowableElementShowingState(4, character.IsAbleToGrowAvatarElement(4, age));
		avatarData.SetGrowableElementShowingState(5, character.IsAbleToGrowAvatarElement(5, age));
	}

	public bool TryExecuteScript(EventScriptRuntime scriptRuntime)
	{
		if (EventConfig.Script == null)
		{
			return false;
		}
		scriptRuntime.ExecuteScript(EventConfig.Script, ArgBox);
		string nextEvent = scriptRuntime.Current.NextEvent;
		if (nextEvent != null)
		{
			DomainManager.TaiwuEvent.ToEvent(nextEvent);
			return true;
		}
		return false;
	}

	public TaiwuEventDisplayData ToDisplayData()
	{
		EventConfig.TaiwuEvent = this;
		NeedNameRelatedDataCharacterIdList.Clear();
		TaiwuEventDisplayData data = new TaiwuEventDisplayData();
		data.EventGuid = EventGuid;
		data.EventTexture = EventConfig.EventBackground;
		data.MaskControlCode = EventConfig.MaskControl;
		data.MaskTweenTime = (ushort)(EventConfig.MaskTweenTime * 100f);
		bool flag = CheckEventBoolState("NotShowTargetRole", 4);
		bool flag2 = CheckEventBoolState("NotShowMainRole", 3);
		string arg = string.Empty;
		if (ArgBox.Get("ConchShip_PresetKey_LeftActorKey", ref arg))
		{
			ArgBox.Remove<string>("ConchShip_PresetKey_LeftActorKey");
		}
		else if (!string.IsNullOrEmpty(EventConfig.MainRoleKey) && !flag2)
		{
			if (!ArgBox.Contains<EventActorData>(EventConfig.MainRoleKey))
			{
				GameData.Domains.Character.Character character = ArgBox.GetCharacter(EventConfig.MainRoleKey);
				data.MainCharacter = DomainManager.Character.GetCharacterDisplayData(character.GetId());
				short arg2 = data.MainCharacter.PhysiologicalAge;
				if (ArgBox.Get("MainCharacterDisplayAge", ref arg2))
				{
					UpdateAvatarGrowableElementToAge(character, arg2, ref data.MainCharacter.AvatarRelatedData.AvatarData);
					data.MainCharacter.AvatarRelatedData.DisplayAge = arg2;
				}
				short arg3 = -1;
				if (ArgBox.Get("MainCharacterDisplayCloth", ref arg3))
				{
					ClothingItem item = Clothing.Instance.GetItem(arg3);
					if (item != null)
					{
						data.MainCharacter.AvatarRelatedData.ClothingDisplayId = item.DisplayId;
					}
				}
			}
			else
			{
				arg = EventConfig.MainRoleKey;
			}
		}
		string arg4 = string.Empty;
		if (ArgBox.Get("ActorKey", ref arg4))
		{
			ArgBox.Remove<string>("ActorKey");
		}
		else if (!string.IsNullOrEmpty(EventConfig.TargetRoleKey) && !flag)
		{
			if (!ArgBox.Contains<EventActorData>(EventConfig.TargetRoleKey))
			{
				GameData.Domains.Character.Character character2 = ArgBox.GetCharacter(EventConfig.TargetRoleKey);
				if (character2 == null)
				{
					throw new Exception(EventGuid + ":no character key " + EventConfig.TargetRoleKey + " in ArgBox!");
				}
				data.TargetCharacter = DomainManager.Character.GetCharacterDisplayData(character2.GetId());
				short arg5 = data.TargetCharacter.PhysiologicalAge;
				if (ArgBox.Get("TargetCharacterDisplayAge", ref arg5))
				{
					UpdateAvatarGrowableElementToAge(character2, arg5, ref data.TargetCharacter.AvatarRelatedData.AvatarData);
					data.TargetCharacter.AvatarRelatedData.DisplayAge = arg5;
				}
				short arg6 = -1;
				if (ArgBox.Get("TargetCharacterDisplayCloth", ref arg6))
				{
					ClothingItem item2 = Clothing.Instance.GetItem(arg6);
					if (item2 != null)
					{
						data.TargetCharacter.AvatarRelatedData.ClothingDisplayId = item2.DisplayId;
					}
				}
				short arg7 = -1;
				if (ArgBox.Get("TargetCharacterDisplayingClothId", ref arg7))
				{
					data.TargetCharacter.AvatarRelatedData.ClothingDisplayId = arg7;
				}
			}
			else
			{
				arg4 = EventConfig.TargetRoleKey;
			}
		}
		if (string.IsNullOrEmpty(EventConfig.EventBackground))
		{
			string arg8 = string.Empty;
			if (!string.IsNullOrEmpty(DomainManager.TaiwuEvent.SeriesEventTexture))
			{
				data.EventTexture = DomainManager.TaiwuEvent.SeriesEventTexture;
			}
			else if (ArgBox.Get("ConchShip_PresetKey_SpecifyEventBackground", ref arg8) && !string.IsNullOrEmpty(arg8))
			{
				data.EventTexture = arg8;
				ArgBox.Remove<string>("ConchShip_PresetKey_SpecifyEventBackground");
			}
			else
			{
				MapBlockData block;
				if (DomainManager.Map.IsTraveling)
				{
					Location travelCurrLocation = DomainManager.Map.GetTravelCurrLocation();
					block = DomainManager.Map.GetBlock(travelCurrLocation);
				}
				else
				{
					block = DomainManager.Map.GetBlock(EventArgBox.TaiwuAreaId, EventArgBox.TaiwuBlockId);
				}
				data.EventTexture = block.GetConfig().EventBack;
			}
		}
		else if (EventConfig.EventType == EEventType.ModEvent)
		{
			string modDirectory = DomainManager.Mod.GetModDirectory(EventConfig.Package.ModIdString);
			if (string.IsNullOrEmpty(modDirectory))
			{
				data.EventTexture = Path.Combine(EventConfig.Package.ModIdString, "../EventTextures/" + EventConfig.EventBackground + ".png").Replace("\\", "/");
			}
			else
			{
				data.EventTexture = Path.Combine(modDirectory, "Events/EventTextures/" + EventConfig.EventBackground + ".png").Replace("\\", "/");
			}
		}
		data.EventContent = EventConfig.GetReplacedContentString();
		if (string.IsNullOrEmpty(data.EventContent))
		{
			data.EventContent = TaiwuEventTagHandler.DecodeTag(EventConfig.EventContent, ArgBox, this);
		}
		data.ExtraFormatLanguageKeys = EventConfig.GetExtraFormatLanguageKeys();
		data.EscOptionIndex = -1;
		data.EventOptionInfos = new List<EventOptionInfo>();
		for (int i = 0; i < EventConfig.EventOptions.Length; i++)
		{
			HandleOption(EventConfig.EventOptions[i], this);
		}
		for (int j = 0; j < ExtendEventOptions.Count; j++)
		{
			(string, string) tuple = ExtendEventOptions[j];
			TaiwuEvent taiwuEvent = DomainManager.TaiwuEvent.GetEvent(tuple.Item1);
			if (taiwuEvent != null)
			{
				taiwuEvent.ArgBox = ArgBox;
				TaiwuEventOption option = taiwuEvent.EventConfig[tuple.Item2];
				HandleOption(option, taiwuEvent);
			}
		}
		if (data.EventOptionInfos.Count <= 0)
		{
			throw new Exception("event " + data.EventGuid + " failed to display cause no option!");
		}
		if (data.EventOptionInfos.Count > 36)
		{
			throw new Exception("event " + data.EventGuid + " failed to display cause too many options in event!");
		}
		if (CheckEventBoolState("ShuffleOptions", 0))
		{
			CollectionUtils.Shuffle(DomainManager.TaiwuEvent.MainThreadDataContext.Random, data.EventOptionInfos);
		}
		if (!string.IsNullOrEmpty(EventConfig.EscOptionKey))
		{
			data.EscOptionIndex = (sbyte)(data.EventOptionInfos.Count - 1);
			for (sbyte b = 0; b < data.EventOptionInfos.Count; b++)
			{
				if (data.EventOptionInfos[b].OptionKey == EventConfig.EscOptionKey)
				{
					EventOptionInfo item3 = data.EventOptionInfos[b];
					data.EventOptionInfos.RemoveAt(b);
					data.EventOptionInfos.Add(item3);
					break;
				}
			}
		}
		if (NeedNameRelatedDataCharacterIdList.Count > 0)
		{
			data.NameDecodeDataList = new List<TaiwuEventCharacterNameDecodeData>();
			List<NameRelatedData> nameRelatedDataList = DomainManager.Character.GetNameRelatedDataList(NeedNameRelatedDataCharacterIdList);
			int k = 0;
			for (int count = NeedNameRelatedDataCharacterIdList.Count; k < count; k++)
			{
				int characterId = NeedNameRelatedDataCharacterIdList[k];
				data.NameDecodeDataList.Add(new TaiwuEventCharacterNameDecodeData
				{
					CharacterId = characterId,
					NameRelatedData = nameRelatedDataList[k]
				});
			}
			NeedNameRelatedDataCharacterIdList.Clear();
		}
		else
		{
			data.NameDecodeDataList = null;
		}
		_needNameRelatedDataCharacterIdList = null;
		data.ExtraData = new TaiwuEventDisplayExtraData();
		data.ExtraData.HereticTemplateId = -1;
		data.ExtraData.ForbidViewCharacter = CheckEventBoolState("ForbidViewCharacter", 6);
		data.ExtraData.ForbidViewSelf = CheckEventBoolState("ForbidViewSelf", 5);
		data.ExtraData.HideRightFavorability = CheckEventBoolState("HideFavorability", 8);
		data.ExtraData.HideLeftFavorability = CheckEventBoolState("ConchShip_PresetKey_HideLeftFavorability", 7);
		data.ExtraData.TargetRoleUseAlternativeName = CheckEventBoolState("TargetRoleUseAlternativeName", 2);
		data.ExtraData.MainRoleUseAlternativeName = CheckEventBoolState("MainRoleUseAlternativeName", 1);
		data.ExtraData.RightCharacterShadow = CheckEventBoolState("ConchShip_PresetKey_RightCharacterShadow", 13);
		data.ExtraData.RightForbiddenConsummateLevel = CheckEventBoolState("ConchShip_PresetKey_RightForbiddenConsummateLevel", 14);
		int arg9 = -1;
		if (ArgBox.Get("CaravanId", ref arg9))
		{
			data.ExtraData.CaravanData = DomainManager.Merchant.GetCaravanDisplayData(DomainManager.TaiwuEvent.MainThreadDataContext, arg9);
		}
		else
		{
			data.ExtraData.CaravanData = null;
		}
		int arg10 = -1;
		if (ArgBox.Get("JiaoId", ref arg10) && DomainManager.Extra.TryGetJiao(arg10, out var jiao))
		{
			data.ExtraData.JiaoDisplayData = DomainManager.Item.GetItemDisplayData(jiao.Key);
		}
		else
		{
			data.ExtraData.JiaoDisplayData = null;
		}
		short arg11 = -1;
		if (ArgBox.Get("TargetCharacterTemplateId", ref arg11))
		{
			data.ExtraData.HereticTemplateId = arg11;
			data.ExtraData.ForbidViewCharacter = true;
			data.ExtraData.HideRightFavorability = true;
			ArgBox.Remove<short>("TargetCharacterTemplateId");
		}
		if (!string.IsNullOrEmpty(arg))
		{
			ArgBox.Get(arg, out data.ExtraData.LeftActorData);
			data.ExtraData.ForbidViewSelf = true;
			data.ExtraData.LeftActorShowMarriageLook1 = CheckEventBoolState(string.Empty, 17);
			data.ExtraData.LeftActorShowMarriageLook2 = CheckEventBoolState(string.Empty, 18);
		}
		if (!string.IsNullOrEmpty(arg4))
		{
			ArgBox.Get(arg4, out data.ExtraData.ActorData);
			data.ExtraData.ForbidViewCharacter = true;
			data.ExtraData.RightActorShowMarriageLook1 = CheckEventBoolState(string.Empty, 19);
			data.ExtraData.RightActorShowMarriageLook2 = CheckEventBoolState(string.Empty, 20);
		}
		if (ArgBox.Get("SelectItemInfo", out EventSelectItemData arg12))
		{
			data.ExtraData.SelectItemData = arg12;
		}
		else
		{
			data.ExtraData.SelectItemData = null;
		}
		if (ArgBox.Get("SelectReadingBookCount", out EventSelectReadingBookCountData arg13))
		{
			data.ExtraData.SelectReadingBookCountData = arg13;
		}
		else
		{
			data.ExtraData.SelectReadingBookCountData = null;
		}
		if (ArgBox.Get("SelectNeigongLoopingCount", out EventSelectNeigongLoopingCountData arg14))
		{
			data.ExtraData.SelectNeigongLoopingCountData = arg14;
		}
		else
		{
			data.ExtraData.SelectNeigongLoopingCountData = null;
		}
		if (ArgBox.Get("SelectFuyuFaithCount", out EventSelectFuyuFaithCountData arg15))
		{
			data.ExtraData.SelectFuyuFaithCountData = arg15;
		}
		else
		{
			data.ExtraData.SelectFuyuFaithCountData = null;
		}
		if (ArgBox.Get("SelectFameData", out EventSelectFameData arg16))
		{
			data.ExtraData.SelectFameData = arg16;
		}
		else
		{
			data.ExtraData.SelectFameData = null;
		}
		if (ArgBox.Get("SelectCharacterData", out EventSelectCharacterData arg17))
		{
			data.ExtraData.SelectCharacterData = arg17;
		}
		else
		{
			data.ExtraData.SelectCharacterData = null;
		}
		if (ArgBox.Get("InputRequestData", out EventInputRequestData arg18))
		{
			data.ExtraData.InputRequestData = arg18;
		}
		else
		{
			data.ExtraData.InputRequestData = null;
		}
		bool arg19 = false;
		if (ArgBox.Get("SelectAvatarEvent", ref arg19) && arg19)
		{
			data.ExtraData.SelectOneAvatarRelatedDataList = new List<AvatarRelatedData>();
			if (-1 != data.EscOptionIndex)
			{
				List<EventOptionInfo> eventOptionInfos = data.EventOptionInfos;
				int escOptionIndex = data.EscOptionIndex;
				List<EventOptionInfo> eventOptionInfos2 = data.EventOptionInfos;
				int index = eventOptionInfos2.Count - 1;
				List<EventOptionInfo> eventOptionInfos3 = data.EventOptionInfos;
				EventOptionInfo value = eventOptionInfos3[eventOptionInfos3.Count - 1];
				EventOptionInfo value2 = data.EventOptionInfos[data.EscOptionIndex];
				eventOptionInfos[escOptionIndex] = value;
				eventOptionInfos2[index] = value2;
				data.EscOptionIndex = (sbyte)(data.EventOptionInfos.Count - 1);
			}
			for (int l = 0; l < data.EventOptionInfos.Count; l++)
			{
				ArgBox.Get(data.EventOptionInfos[l].OptionKey, out AvatarRelatedData arg20);
				if (arg20 == null)
				{
					throw new Exception(EventGuid + "'s option " + data.EventOptionInfos[l].OptionKey + ", not set an avatarRelatedData!");
				}
				data.ExtraData.SelectOneAvatarRelatedDataList.Add(arg20);
			}
			ArgBox.Remove<bool>("SelectAvatarEvent");
		}
		data.ExtraData.MainRoleShyFlag = CheckEventBoolState("ConchShip_PresetKey_MainRoleShowBlush", 9);
		data.ExtraData.TargetRoleShyFlag = CheckEventBoolState("ConchShip_PresetKey_TargetRoleShowBlush", 10);
		short arg21 = -1;
		ArgBox.Get("ConchShip_PresetKey_MainRoleAdjustClothId", ref arg21);
		ArgBox.Remove<short>("ConchShip_PresetKey_MainRoleAdjustClothId");
		data.ExtraData.MainRoleAdjustClothDisplayId = arg21;
		short arg22 = -1;
		ArgBox.Get("ConchShip_PresetKey_TargetRoleAdjustClothId", ref arg22);
		ArgBox.Remove<short>("ConchShip_PresetKey_TargetRoleAdjustClothId");
		data.ExtraData.TargetRoleAdjustClothDisplayId = arg22;
		data.ExtraData.LeftRoleShowInjuryInfo = CheckEventBoolState("ConchShip_PresetKey_LeftRoleShowInjuryInfo", 11);
		data.ExtraData.RightRoleShowInjuryInfo = CheckEventBoolState("ConchShip_PresetKey_RightRoleShowInjuryInfo", 12);
		data.ExtraData.LeftForbidShowFavorChangeEffect = CheckEventBoolState("CS_PK_LeftForbidShowFavorChangeEffect", 15);
		data.ExtraData.RightForbidShowFavorChangeEffect = CheckEventBoolState("CS_PK_RightForbidShowFavorChangeEffect", 16);
		return data;
		void HandleOption(TaiwuEventOption taiwuEventOption, TaiwuEvent eventData)
		{
			if (taiwuEventOption.IsVisible)
			{
				EventOptionInfo item4 = new EventOptionInfo
				{
					OptionKey = taiwuEventOption.OptionKey,
					Behavior = taiwuEventOption.Behavior,
					OptionContent = taiwuEventOption.GetReplacedContent?.Invoke()
				};
				if (string.IsNullOrEmpty(item4.OptionContent))
				{
					item4.OptionContent = TaiwuEventTagHandler.DecodeTag(taiwuEventOption.OptionContent, ArgBox, this);
				}
				item4.ExtraFormatLanguageKeys = taiwuEventOption.GetExtraFormatLanguageKeys?.Invoke();
				List<TaiwuEventOptionConditionBase> optionAvailableConditions = taiwuEventOption.OptionAvailableConditions;
				if (optionAvailableConditions != null && optionAvailableConditions.Count > 0)
				{
					item4.OptionAvailableConditions = new List<OptionAvailableInfo>();
					bool flag3 = true;
					for (int m = 0; m < taiwuEventOption.OptionAvailableConditions.Count; m++)
					{
						OptionAvailableInfo item5 = default(OptionAvailableInfo);
						TaiwuEventOptionConditionBase taiwuEventOptionConditionBase = taiwuEventOption.OptionAvailableConditions[m];
						if (taiwuEventOptionConditionBase.OrConditionCore != null && taiwuEventOptionConditionBase.OrConditionCore.Count > 0)
						{
							item5.Data = new OptionAvailableInfoMinimumElement[taiwuEventOptionConditionBase.OrConditionCore.Count];
							for (int n = 0; n < taiwuEventOptionConditionBase.OrConditionCore.Count; n++)
							{
								OptionAvailableInfoMinimumElement element = default(OptionAvailableInfoMinimumElement);
								OptionConditionModifier.ModifyCondition(ref element, taiwuEventOptionConditionBase.OrConditionCore[m], ArgBox);
								item5.PassState = item5.PassState || element.Pass;
								item5.Hide = item5.Hide || element.Hide;
								item5.Data[m] = element;
							}
						}
						else
						{
							OptionAvailableInfoMinimumElement element2 = default(OptionAvailableInfoMinimumElement);
							OptionConditionModifier.ModifyCondition(ref element2, taiwuEventOptionConditionBase, ArgBox);
							item5.PassState = item5.PassState || element2.Pass;
							item5.Hide = item5.Hide || element2.Hide;
							item5.Data = new OptionAvailableInfoMinimumElement[1] { element2 };
						}
						if (!item5.Hide)
						{
							item4.OptionAvailableConditions.Add(item5);
						}
						flag3 = flag3 && item5.PassState;
					}
					if (!flag3)
					{
						item4.OptionState = -1;
					}
				}
				if (!taiwuEventOption.CheckAvailableConditionsFromCode())
				{
					item4.OptionState = -1;
				}
				EventScriptRuntime scriptRuntime = DomainManager.TaiwuEvent.ScriptRuntime;
				scriptRuntime.StartRecordConditionHints();
				if (!taiwuEventOption.CheckAvailableConditionsFromScript())
				{
					item4.OptionState = -1;
				}
				item4.OptionAvailableConditionInfos = scriptRuntime.StopRecordConditionHints();
				if (item4.OptionState != -1)
				{
					if (taiwuEventOption.DefaultState == 1 && taiwuEventOption.WasSelected)
					{
						item4.OptionState = 2;
					}
					else
					{
						item4.OptionState = taiwuEventOption.DefaultState;
					}
				}
				if (!ArgBox.Get(taiwuEventOption.OptionKey + "_Type", ref item4.OptionType))
				{
					item4.OptionType = -1;
				}
				if (taiwuEventOption.OptionConsumeInfos != null)
				{
					item4.OptionConsumeInfos = new List<OptionConsumeInfo>();
					for (int num = 0; num < taiwuEventOption.OptionConsumeInfos.Count; num++)
					{
						OptionConsumeInfo optionConsumeInfo = OptionConsumeHelper.ModifyOptionConsumeInfo(taiwuEventOption.OptionConsumeInfos[num], ArgBox);
						GameData.Domains.Character.Character character3 = taiwuEventOption.ArgBox.GetCharacter("RoleTaiwu");
						GameData.Domains.Character.Character character4 = null;
						if (!string.IsNullOrEmpty(EventConfig.TargetRoleKey))
						{
							character4 = taiwuEventOption.ArgBox.GetCharacter(EventConfig.TargetRoleKey);
						}
						bool flag4 = (optionConsumeInfo.HasEnough = optionConsumeInfo.HasConsumeResource(character3.GetId(), character4?.GetId() ?? (-1)));
						optionConsumeInfo.HoldCount = optionConsumeInfo.GetHoldCount(character3.GetId(), character4?.GetId() ?? (-1));
						item4.OptionConsumeInfos.Add(optionConsumeInfo);
						if (!flag4)
						{
							item4.OptionState = -1;
						}
					}
				}
				data.EventOptionInfos.Add(item4);
			}
			else
			{
				eventData.ArgBox = null;
			}
		}
	}

	private bool CheckEventBoolState(string key, short templateId)
	{
		bool arg = false;
		ArgBox.Get(key, ref arg);
		TaiwuEventItem eventConfig = EventConfig;
		if (eventConfig.BoolStateDict == null)
		{
			eventConfig.BoolStateDict = new Dictionary<short, EventBoolStateInfo>();
		}
		EventConfig.BoolStateDict.TryGetValue(templateId, out var value);
		if (value == null)
		{
			EventBoolStateItem eventBoolStateItem = EventBoolState.Instance[templateId];
			if (eventBoolStateItem.RemoveBeforeNextEvent)
			{
				ArgBox.Remove<bool>(key);
			}
		}
		else if (value.RemoveBeforeNextEvent)
		{
			ArgBox.Remove<bool>(key);
		}
		return arg || (value?.BoolState ?? false);
	}

	public TaiwuEventSummaryDisplayData ToSummaryDisplayData()
	{
		TaiwuEventSummaryDisplayData taiwuEventSummaryDisplayData = new TaiwuEventSummaryDisplayData();
		taiwuEventSummaryDisplayData.EventGuid = EventGuid;
		if (string.IsNullOrEmpty(EventConfig.TargetRoleKey))
		{
			throw new Exception("can not to summary display data because EventConfig.TargetRoleKey has not been set!");
		}
		GameData.Domains.Character.Character character = ArgBox.GetCharacter(EventConfig.TargetRoleKey);
		if (character == null)
		{
			return null;
		}
		taiwuEventSummaryDisplayData.CharacterId = character.GetId();
		return taiwuEventSummaryDisplayData;
	}

	public void SetModInt(string dataName, bool isArchive, int val)
	{
		if (CheckModDataValid())
		{
			DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
			DomainManager.Mod.SetInt(mainThreadDataContext, EventConfig.Package.ModIdString, dataName, isArchive, val);
		}
	}

	public void SetModBool(string dataName, bool isArchive, bool val)
	{
		if (CheckModDataValid())
		{
			DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
			DomainManager.Mod.SetBool(mainThreadDataContext, EventConfig.Package.ModIdString, dataName, isArchive, val);
		}
	}

	public void SetModFloat(string dataName, bool isArchive, float val)
	{
		if (CheckModDataValid())
		{
			DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
			DomainManager.Mod.SetFloat(mainThreadDataContext, EventConfig.Package.ModIdString, dataName, isArchive, val);
		}
	}

	public void SetModString(string dataName, bool isArchive, string val)
	{
		if (CheckModDataValid())
		{
			DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
			DomainManager.Mod.SetString(mainThreadDataContext, EventConfig.Package.ModIdString, dataName, isArchive, val);
		}
	}

	public void SetSerializableModData(string dataName, bool isArchive, SerializableModData val)
	{
		if (CheckModDataValid())
		{
			DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
			DomainManager.Mod.SetSerializableModData(mainThreadDataContext, EventConfig.Package.ModIdString, dataName, isArchive, val);
		}
	}

	public void RemoveModData(string dataName)
	{
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		DomainManager.Mod.RemoveData(mainThreadDataContext, EventConfig.Package.ModIdString, dataName);
	}

	public bool RemoveModInt(string dataName, bool isArchive)
	{
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		return DomainManager.Mod.RemoveInt(mainThreadDataContext, EventConfig.Package.ModIdString, dataName, isArchive);
	}

	public bool RemoveModBool(string dataName, bool isArchive)
	{
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		return DomainManager.Mod.RemoveBool(mainThreadDataContext, EventConfig.Package.ModIdString, dataName, isArchive);
	}

	public bool RemoveModFloat(string dataName, bool isArchive)
	{
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		return DomainManager.Mod.RemoveFloat(mainThreadDataContext, EventConfig.Package.ModIdString, dataName, isArchive);
	}

	public bool RemoveModString(string dataName, bool isArchive)
	{
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		return DomainManager.Mod.RemoveString(mainThreadDataContext, EventConfig.Package.ModIdString, dataName, isArchive);
	}

	public bool RemoveSerializableModData(string dataName, bool isArchive)
	{
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		return DomainManager.Mod.RemoveSerializableModData(mainThreadDataContext, EventConfig.Package.ModIdString, dataName, isArchive);
	}

	public bool GetModData(string dataName, bool isArchive, ref int val)
	{
		if (!CheckModDataValid())
		{
			return false;
		}
		return DomainManager.Mod.TryGet(EventConfig.Package.ModIdString, dataName, isArchive, out val);
	}

	public bool GetModData(string dataName, bool isArchive, ref bool val)
	{
		if (!CheckModDataValid())
		{
			return false;
		}
		return DomainManager.Mod.TryGet(EventConfig.Package.ModIdString, dataName, isArchive, out val);
	}

	public bool GetModData(string dataName, bool isArchive, ref float val)
	{
		if (!CheckModDataValid())
		{
			return false;
		}
		return DomainManager.Mod.TryGet(EventConfig.Package.ModIdString, dataName, isArchive, out val);
	}

	public bool GetModData(string dataName, bool isArchive, ref string val)
	{
		if (!CheckModDataValid())
		{
			return false;
		}
		return DomainManager.Mod.TryGet(EventConfig.Package.ModIdString, dataName, isArchive, out val);
	}

	public bool GetModData(string dataName, bool isArchive, ref SerializableModData val)
	{
		if (!CheckModDataValid())
		{
			return false;
		}
		return DomainManager.Mod.TryGet(EventConfig.Package.ModIdString, dataName, isArchive, out val);
	}

	public bool CheckModDataValid(bool appendWarning = true)
	{
		if (string.IsNullOrEmpty(DomainManager.Mod.GetModDirectory(EventConfig.Package.ModIdString)))
		{
			AdaptableLog.TagWarning("TaiwuEvent", $"Unable to find mod {EventConfig.Package.ModIdString} with package group {EventConfig.Package.Group}.", appendWarning);
			return false;
		}
		return true;
	}
}
