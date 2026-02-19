using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.TaiwuEvent.EventManager;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions.CustomActions;

[SerializableGameData(NotForDisplayModule = true)]
public class LegendaryBookMonthlyAction : MonthlyActionBase, IDynamicAction, ISerializableGameData
{
	[SerializableGameDataField]
	public Location Location;

	[SerializableGameDataField]
	public sbyte BookType;

	[SerializableGameDataField]
	public sbyte BookAppearType;

	[SerializableGameDataField]
	public int PrevOwnerId;

	[SerializableGameDataField]
	public sbyte ActivateDelay;

	[SerializableGameDataField]
	public List<CharacterSet> ParticipatingCharacterSets;

	private static List<CharacterSet> _tempCalledCharSets = new List<CharacterSet>();

	private const int ActivateDelayMin = 3;

	private const int ActivateDelayMax = 9;

	private static readonly CallCharacterHelper.SearchCharacterRule SearchRule = new CallCharacterHelper.SearchCharacterRule
	{
		AllowTemporaryCharacter = false,
		SearchRange = 2,
		SubRules = new List<CallCharacterHelper.SearchCharacterSubRule>
		{
			new CallCharacterHelper.SearchCharacterSubRule(0, 4, CheckCharacterAvailable, CheckSectLeaderAvailable)
		}
	};

	public short DynamicActionType => 1;

	public LegendaryBookMonthlyAction()
	{
		Key = MonthlyActionKey.Invalid;
		ParticipatingCharacterSets = new List<CharacterSet>();
		Location = Location.Invalid;
		BookType = -1;
		BookAppearType = -1;
		PrevOwnerId = -1;
	}

	public override void TriggerAction()
	{
		if (Location.IsValid() && BookType >= 0 && DomainManager.LegendaryBook.GetLegendaryBookItem(BookType).IsValid() && DomainManager.LegendaryBook.GetOwner(BookType) < 0)
		{
			DomainManager.LegendaryBook.RegisterLegendaryBookMonthlyAction(this);
			DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
			short num = AiHelper.LegendaryBookRelatedConstants.LegendaryBookAdventures[BookType];
			if (DomainManager.Adventure.TryCreateAdventureSite(mainThreadDataContext, Location.AreaId, Location.BlockId, num, Key))
			{
				sbyte firstLegendaryBookDelay = DomainManager.Extra.GetFirstLegendaryBookDelay();
				bool flag = firstLegendaryBookDelay == 0;
				DomainManager.Extra.SetFirstLegendaryBookDelay(mainThreadDataContext, -1);
				MonthlyEventActionsManager.NewlyTriggered++;
				State = 1;
				ActivateDelay = (sbyte)((!flag) ? ((sbyte)mainThreadDataContext.Random.Next(3, 10)) : 0);
				AdaptableLog.TagInfo("LegendaryBookMonthlyAction", "Legendary book adventure " + Config.Adventure.Instance[num].Name + " created at " + Location.ToString());
			}
			else
			{
				Location = Location.Invalid;
			}
		}
	}

	public override void MonthlyHandler()
	{
		if (State == 0)
		{
			if (!Location.IsValid())
			{
				DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
				short areaId = (short)mainThreadDataContext.Random.Next(45);
				MapBlockData randomMapBlockDataInAreaByFilters = DomainManager.Map.GetRandomMapBlockDataInAreaByFilters(mainThreadDataContext.Random, areaId, null, includeBlocksWithAdventure: false);
				Location = randomMapBlockDataInAreaByFilters.GetLocation();
			}
			TriggerAction();
		}
		if (State == 5 && Month > ActivateDelay)
		{
			CallParticipateCharacters();
		}
		if (State == 1 && Month >= ActivateDelay)
		{
			Activate();
			State = 5;
		}
		if (State != 0)
		{
			Month++;
		}
	}

	public override void Activate()
	{
		short index = AiHelper.LegendaryBookRelatedConstants.LegendaryBookAdventures[BookType];
		AdaptableLog.TagInfo("LegendaryBookMonthlyAction", "Legendary book adventure " + Config.Adventure.Instance[index].Name + " activated at " + Location.ToString());
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		MonthlyEventActionsManager.NewlyActivated++;
		DomainManager.Adventure.ActivateAdventureSite(mainThreadDataContext, Location.AreaId, Location.BlockId);
		DomainManager.Map.EnsureBlockVisible(mainThreadDataContext, Location);
		ItemKey legendaryBookItem = DomainManager.LegendaryBook.GetLegendaryBookItem(BookType);
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		switch (BookAppearType)
		{
		case 0:
			monthlyEventCollection.AddFightForNewLegendaryBook(Location, (ulong)legendaryBookItem);
			monthlyNotificationCollection.AddFightForNewLegendaryBook(Location, legendaryBookItem.ItemType, legendaryBookItem.TemplateId);
			break;
		case 1:
			monthlyEventCollection.AddFightForLegendaryBookOwnerConsumed(PrevOwnerId, Location, (ulong)legendaryBookItem);
			monthlyNotificationCollection.AddFightForLegendaryBookOwnerConsumed(PrevOwnerId, Location, legendaryBookItem.ItemType, legendaryBookItem.TemplateId);
			break;
		case 2:
			monthlyEventCollection.AddFightForLegendaryBookOwnerDie(PrevOwnerId, Location, (ulong)legendaryBookItem);
			monthlyNotificationCollection.AddFightForLegendaryBookOwnerDie(PrevOwnerId, Location, legendaryBookItem.ItemType, legendaryBookItem.TemplateId);
			break;
		case 3:
			monthlyEventCollection.AddFightForLegendaryBookAbandoned(PrevOwnerId, Location, (ulong)legendaryBookItem);
			monthlyNotificationCollection.AddFightForLegendaryBookAbandoned(PrevOwnerId, Location, legendaryBookItem.ItemType, legendaryBookItem.TemplateId);
			break;
		}
	}

	public override void Deactivate(bool isComplete)
	{
		short index = AiHelper.LegendaryBookRelatedConstants.LegendaryBookAdventures[BookType];
		AdaptableLog.TagInfo("LegendaryBookMonthlyAction", "Legendary book adventure " + Config.Adventure.Instance[index].Name + " removed at " + Location.ToString());
		DomainManager.LegendaryBook.UnregisterLegendaryBookMonthlyAction(BookType);
		if (DomainManager.LegendaryBook.GetOwner(BookType) < 0 && !AssignRandomOwner())
		{
			DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
			MapBlockData mapBlockData = DomainManager.Map.GetRandomMapBlockDataInAreaByFilters(mainThreadDataContext.Random, Location.AreaId, null, includeBlocksWithAdventure: false);
			if (mapBlockData?.BlockId == Location.BlockId)
			{
				mapBlockData = null;
			}
			LegendaryBookMonthlyAction action = new LegendaryBookMonthlyAction
			{
				BookType = BookType,
				BookAppearType = BookAppearType,
				PrevOwnerId = PrevOwnerId,
				Location = (mapBlockData?.GetLocation() ?? Location.Invalid)
			};
			DomainManager.TaiwuEvent.AddTempDynamicAction(mainThreadDataContext, action);
		}
		ClearCalledCharacters();
		State = 0;
		Month = 0;
		LastFinishDate = DomainManager.World.GetCurrDate();
		Location = Location.Invalid;
	}

	private bool AssignRandomOwner()
	{
		List<GameData.Domains.Character.Character> list = ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Get();
		list.Clear();
		foreach (CharacterSet participatingCharacterSet in ParticipatingCharacterSets)
		{
			foreach (int item in participatingCharacterSet.GetCollection())
			{
				if (DomainManager.Character.TryGetElement_Objects(item, out var element) && element.GetLocation().Equals(Location) && !element.IsCompletelyInfected() && element.GetLegendaryBookOwnerState() < 3 && element.GetKidnapperId() < 0)
				{
					list.Add(element);
				}
			}
		}
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		if (list.Count == 0)
		{
			ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(list);
			AdaptableLog.TagInfo("LegendaryBookMonthlyAction", "Failed to assign random owner.");
			return false;
		}
		GameData.Domains.Character.Character random = list.GetRandom(mainThreadDataContext.Random);
		ItemKey legendaryBookItem = DomainManager.LegendaryBook.GetLegendaryBookItem(BookType);
		random.AddInventoryItem(mainThreadDataContext, legendaryBookItem, 1);
		ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(list);
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		monthlyNotificationCollection.AddLegendaryBookAppeared(random.GetId(), Location, legendaryBookItem.ItemType, legendaryBookItem.TemplateId);
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		lifeRecordCollection.AddGainLegendaryBook(random.GetId(), currDate, Location, legendaryBookItem.ItemType, legendaryBookItem.TemplateId);
		return true;
	}

	public override MonthlyActionBase CreateCopy()
	{
		return GameData.Serializer.Serializer.CreateCopy(this);
	}

	public override void CollectCalledCharacters(HashSet<int> calledCharacters)
	{
		foreach (CharacterSet participatingCharacterSet in ParticipatingCharacterSets)
		{
			calledCharacters.UnionWith(participatingCharacterSet.GetCollection());
		}
	}

	public override void FillEventArgBox(EventArgBox eventArgBox)
	{
		AdaptableLog.Info("Adding participating characters to adventure.");
		CharacterSet charSet = ((ParticipatingCharacterSets.Count > 0) ? ParticipatingCharacterSets[0] : default(CharacterSet));
		FillIntelligentCharactersToArgBox(eventArgBox, "ParticipateCharacter_0", charSet);
		AdventureCharacterSortUtils.Sort(eventArgBox, isMajorChar: false, 0, CharacterSortType.CombatPower, ascendingOrder: false);
	}

	private void FillIntelligentCharactersToArgBox(EventArgBox eventArgBox, string keyPrefix, CharacterSet charSet)
	{
		int num = 0;
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		foreach (int item in charSet.GetCollection())
		{
			if (DomainManager.Character.TryGetElement_Objects(item, out var element))
			{
				if (element.IsCrossAreaTraveling())
				{
					DomainManager.Character.GroupMove(mainThreadDataContext, element, Location);
				}
				if (element.GetLocation().Equals(Location))
				{
					eventArgBox.Set($"{keyPrefix}_{num}", item);
					num++;
				}
			}
		}
		AdaptableLog.Info($"Adding {num} real characters to adventure.");
		eventArgBox.Set(keyPrefix + "_Count", num);
	}

	private void ClearCalledCharacters()
	{
		CallCharacterHelper.ClearCalledCharacters(ParticipatingCharacterSets, unHideCharacters: true, removeExternalState: true);
	}

	private void CallParticipateCharacters()
	{
		foreach (CharacterSet tempCalledCharSet in _tempCalledCharSets)
		{
			tempCalledCharSet.Clear();
		}
		_tempCalledCharSets.Clear();
		CallCharacterHelper.CallCharacters(Location, SearchRule, _tempCalledCharSets, modifyExternalState: true);
		for (int i = 0; i < _tempCalledCharSets.Count; i++)
		{
			CharacterSet characterSet = _tempCalledCharSets[i];
			if (ParticipatingCharacterSets.Count <= i)
			{
				ParticipatingCharacterSets.Add(default(CharacterSet));
			}
			CharacterSet value = ParticipatingCharacterSets[i];
			value.AddRange(characterSet.GetCollection());
			ParticipatingCharacterSets[i] = value;
		}
	}

	private static bool CheckCharacterAvailable(GameData.Domains.Character.Character character)
	{
		return CharacterMatcher.DefValue.AvailableForLegendaryBookAdventure.Match(character);
	}

	private static bool CheckSectLeaderAvailable(GameData.Domains.Character.Character character)
	{
		OrganizationInfo organizationInfo = character.GetOrganizationInfo();
		return organizationInfo.Grade != 8 || !organizationInfo.Principal || !Config.Organization.Instance[organizationInfo.OrgTemplateId].IsSect || DomainManager.TaiwuEvent.MainThreadDataContext.Random.CheckPercentProb(20);
	}

	public override bool IsSerializedSizeFixed()
	{
		return false;
	}

	public override int GetSerializedSize()
	{
		int num = 25;
		if (ParticipatingCharacterSets != null)
		{
			num += 2;
			int count = ParticipatingCharacterSets.Count;
			for (int i = 0; i < count; i++)
			{
				num += ParticipatingCharacterSets[i].GetSerializedSize();
			}
		}
		else
		{
			num += 2;
		}
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe override int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = DynamicActionType;
		ptr += 2;
		ptr += Location.Serialize(ptr);
		*ptr = (byte)BookType;
		ptr++;
		*ptr = (byte)BookAppearType;
		ptr++;
		*(int*)ptr = PrevOwnerId;
		ptr += 4;
		*ptr = (byte)ActivateDelay;
		ptr++;
		if (ParticipatingCharacterSets != null)
		{
			int count = ParticipatingCharacterSets.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				int num = ParticipatingCharacterSets[i].Serialize(ptr);
				ptr += num;
				Tester.Assert(num <= 65535);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += Key.Serialize(ptr);
		*ptr = (byte)State;
		ptr++;
		*(int*)ptr = Month;
		ptr += 4;
		*(int*)ptr = LastFinishDate;
		ptr += 4;
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}

	public unsafe override int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += 2;
		ptr += Location.Deserialize(ptr);
		BookType = (sbyte)(*ptr);
		ptr++;
		BookAppearType = (sbyte)(*ptr);
		ptr++;
		PrevOwnerId = *(int*)ptr;
		ptr += 4;
		ActivateDelay = (sbyte)(*ptr);
		ptr++;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (ParticipatingCharacterSets == null)
			{
				ParticipatingCharacterSets = new List<CharacterSet>(num);
			}
			else
			{
				ParticipatingCharacterSets.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				CharacterSet item = default(CharacterSet);
				ptr += item.Deserialize(ptr);
				ParticipatingCharacterSets.Add(item);
			}
		}
		else
		{
			ParticipatingCharacterSets?.Clear();
		}
		ptr += Key.Deserialize(ptr);
		State = (sbyte)(*ptr);
		ptr++;
		Month = *(int*)ptr;
		ptr += 4;
		LastFinishDate = *(int*)ptr;
		ptr += 4;
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}
}
