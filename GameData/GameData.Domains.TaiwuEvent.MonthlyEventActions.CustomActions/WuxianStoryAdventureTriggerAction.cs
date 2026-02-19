using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions.CustomActions;

[SerializableGameData(NotForDisplayModule = true, IsExtensible = true)]
public class WuxianStoryAdventureTriggerAction : MonthlyActionBase, IDynamicAction, ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort HighGradeCharacters = 0;

		public const ushort NormalCharacters = 1;

		public const ushort Location = 2;

		public const ushort Key = 3;

		public const ushort State = 4;

		public const ushort Month = 5;

		public const ushort LastFinishDate = 6;

		public const ushort Count = 7;

		public static readonly string[] FieldId2FieldName = new string[7] { "HighGradeCharacters", "NormalCharacters", "Location", "Key", "State", "Month", "LastFinishDate" };
	}

	private const int MaxMenteeCountInAdventure = 15;

	private int _currMenteeCountInAdventure = 0;

	[SerializableGameDataField]
	private List<int> _highGradeCharacters;

	[SerializableGameDataField]
	private List<int> _normalCharacters;

	[SerializableGameDataField]
	private Location _location;

	public short DynamicActionType => 3;

	public WuxianStoryAdventureTriggerAction()
	{
		_highGradeCharacters = new List<int>();
		_normalCharacters = new List<int>();
		_location = Location.Invalid;
	}

	public override void MonthlyHandler()
	{
	}

	public override void TriggerAction()
	{
		if (State != 0)
		{
			return;
		}
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(12);
		Location location = settlementByOrgTemplateId.GetLocation();
		List<short> list = new List<short>();
		DomainManager.Map.GetSettlementBlocks(location.AreaId, location.BlockId, list);
		CollectionUtils.Shuffle(mainThreadDataContext.Random, list);
		foreach (short item in list)
		{
			if (DomainManager.Adventure.TryCreateAdventureSite(mainThreadDataContext, location.AreaId, item, 181, Key))
			{
				_location = new Location(location.AreaId, item);
				DomainManager.Adventure.ActivateAdventureSite(mainThreadDataContext, location.AreaId, item);
				CallCharacters();
				break;
			}
		}
	}

	public override void CollectCalledCharacters(HashSet<int> calledCharacters)
	{
		GameData.Domains.Character.Character character;
		foreach (int highGradeCharacter in _highGradeCharacters)
		{
			if (IsCharacterStillValid(highGradeCharacter, out character))
			{
				calledCharacters.Add(highGradeCharacter);
			}
		}
		foreach (int normalCharacter in _normalCharacters)
		{
			if (IsCharacterStillValid(normalCharacter, out character))
			{
				calledCharacters.Add(normalCharacter);
			}
		}
	}

	public override void Deactivate(bool isComplete)
	{
		State = 0;
		Month = 0;
		LastFinishDate = DomainManager.World.GetCurrDate();
		_location = Location.Invalid;
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		foreach (int highGradeCharacter in _highGradeCharacters)
		{
			if (DomainManager.Character.TryGetElement_Objects(highGradeCharacter, out var element))
			{
				element.DeactivateExternalRelationState(mainThreadDataContext, 4);
				if (element.IsCompletelyInfected())
				{
					Events.RaiseInfectedCharacterLocationChanged(mainThreadDataContext, highGradeCharacter, Location.Invalid, element.GetLocation());
				}
				else
				{
					Events.RaiseCharacterLocationChanged(mainThreadDataContext, highGradeCharacter, Location.Invalid, element.GetLocation());
				}
			}
		}
		foreach (int normalCharacter in _normalCharacters)
		{
			if (DomainManager.Character.TryGetElement_Objects(normalCharacter, out var element2))
			{
				element2.DeactivateExternalRelationState(mainThreadDataContext, 4);
				if (element2.IsCompletelyInfected())
				{
					Events.RaiseInfectedCharacterLocationChanged(mainThreadDataContext, normalCharacter, Location.Invalid, element2.GetLocation());
				}
				else
				{
					Events.RaiseCharacterLocationChanged(mainThreadDataContext, normalCharacter, Location.Invalid, element2.GetLocation());
				}
			}
		}
		if (!isComplete)
		{
			DomainManager.World.WuxianEndingFailing1(mainThreadDataContext, isComplete: false);
		}
		_highGradeCharacters.Clear();
		_normalCharacters.Clear();
	}

	public override MonthlyActionBase CreateCopy()
	{
		return GameData.Serializer.Serializer.CreateCopy(this);
	}

	public override void EnsurePrerequisites()
	{
		List<int> list = new List<int>();
		GameData.Domains.Character.Character character;
		foreach (int highGradeCharacter in _highGradeCharacters)
		{
			if (IsCharacterStillValid(highGradeCharacter, out character))
			{
				list.Add(highGradeCharacter);
			}
		}
		_highGradeCharacters = list;
		list = new List<int>();
		foreach (int normalCharacter in _normalCharacters)
		{
			if (IsCharacterStillValid(normalCharacter, out character))
			{
				list.Add(normalCharacter);
			}
		}
		_normalCharacters = list;
	}

	public override void FillEventArgBox(EventArgBox eventArgBox)
	{
		int num = -1;
		int num2 = -1;
		for (int i = 0; i < _highGradeCharacters.Count; i++)
		{
			int num3 = _highGradeCharacters[i];
			OrganizationInfo organizationInfo = DomainManager.Character.GetElement_Objects(num3).GetOrganizationInfo();
			if (organizationInfo.Grade > num2)
			{
				num2 = organizationInfo.Grade;
				num = num3;
			}
			AdaptableLog.Info("Adding 1 major character to adventure.");
			eventArgBox.Set($"MajorCharacter_0_{i}", num3);
		}
		eventArgBox.Set("MajorCharacter_0_Count", _highGradeCharacters.Count);
		for (int j = 0; j < _normalCharacters.Count; j++)
		{
			int arg = _normalCharacters[j];
			AdaptableLog.Info("Adding 1 participate character to adventure.");
			eventArgBox.Set($"ParticipateCharacter_0_{j}", arg);
		}
		eventArgBox.Set("ParticipateCharacter_0_Count", _normalCharacters.Count);
		AdaptableLog.Info("Adding leader to adventure.");
		if (num >= 0)
		{
			eventArgBox.Set("SectLeader", num);
		}
	}

	private bool IsCharacterValidToCall(int charId, out GameData.Domains.Character.Character character)
	{
		if (!DomainManager.Character.TryGetElement_Objects(charId, out character))
		{
			return false;
		}
		OrganizationInfo organizationInfo = character.GetOrganizationInfo();
		if (character.IsActiveExternalRelationState(60))
		{
			return false;
		}
		if (character.GetAgeGroup() != 2)
		{
			return false;
		}
		if (character.GetKidnapperId() >= 0)
		{
			return false;
		}
		if (organizationInfo.OrgTemplateId != 12)
		{
			return false;
		}
		if (organizationInfo.Grade >= 6)
		{
			return true;
		}
		if (_currMenteeCountInAdventure >= 15)
		{
			return false;
		}
		_currMenteeCountInAdventure++;
		return true;
	}

	private bool IsCharacterStillValid(int charId, out GameData.Domains.Character.Character character)
	{
		if (!DomainManager.Character.TryGetElement_Objects(charId, out character))
		{
			return false;
		}
		OrganizationInfo organizationInfo = character.GetOrganizationInfo();
		if (character.GetKidnapperId() >= 0)
		{
			return false;
		}
		if (organizationInfo.OrgTemplateId != 12)
		{
			return false;
		}
		return true;
	}

	private void CallCharacters()
	{
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(12);
		Location location = settlementByOrgTemplateId.GetLocation();
		List<short> list = new List<short>();
		HashSet<int> hashSet = new HashSet<int>();
		DomainManager.Map.GetSettlementBlocks(location.AreaId, location.BlockId, list);
		_currMenteeCountInAdventure = 0;
		foreach (short item in list)
		{
			MapBlockData blockData = DomainManager.Map.GetBlockData(location.AreaId, item);
			HashSet<int> characterSet = blockData.CharacterSet;
			if (characterSet == null)
			{
				continue;
			}
			foreach (int item2 in characterSet)
			{
				if (IsCharacterValidToCall(item2, out var character))
				{
					if (character.GetOrganizationInfo().Grade >= 6)
					{
						_highGradeCharacters.Add(item2);
					}
					else
					{
						_normalCharacters.Add(item2);
					}
				}
			}
		}
		foreach (int highGradeCharacter in _highGradeCharacters)
		{
			if (DomainManager.Character.TryGetElement_Objects(highGradeCharacter, out var element))
			{
				DomainManager.Character.LeaveGroup(mainThreadDataContext, element);
				DomainManager.Character.GroupMove(mainThreadDataContext, element, _location);
				Events.RaiseCharacterLocationChanged(mainThreadDataContext, highGradeCharacter, _location, Location.Invalid);
				element.ActiveExternalRelationState(mainThreadDataContext, 4);
			}
		}
		foreach (int normalCharacter in _normalCharacters)
		{
			if (DomainManager.Character.TryGetElement_Objects(normalCharacter, out var element2))
			{
				DomainManager.Character.LeaveGroup(mainThreadDataContext, element2);
				DomainManager.Character.GroupMove(mainThreadDataContext, element2, _location);
				Events.RaiseCharacterLocationChanged(mainThreadDataContext, normalCharacter, _location, Location.Invalid);
				element2.ActiveExternalRelationState(mainThreadDataContext, 4);
			}
		}
	}

	public override bool IsSerializedSizeFixed()
	{
		return false;
	}

	public override int GetSerializedSize()
	{
		int num = 20;
		num = ((_highGradeCharacters == null) ? (num + 2) : (num + (2 + 4 * _highGradeCharacters.Count)));
		num = ((_normalCharacters == null) ? (num + 2) : (num + (2 + 4 * _normalCharacters.Count)));
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe override int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = DynamicActionType;
		ptr += 2;
		*(short*)ptr = 7;
		ptr += 2;
		if (_highGradeCharacters != null)
		{
			int count = _highGradeCharacters.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((int*)ptr)[i] = _highGradeCharacters[i];
			}
			ptr += 4 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (_normalCharacters != null)
		{
			int count2 = _normalCharacters.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				((int*)ptr)[j] = _normalCharacters[j];
			}
			ptr += 4 * count2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += _location.Serialize(ptr);
		ptr += Key.Serialize(ptr);
		*ptr = (byte)State;
		ptr++;
		*(int*)ptr = Month;
		ptr += 4;
		*(int*)ptr = LastFinishDate;
		ptr += 4;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe override int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += 2;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (_highGradeCharacters == null)
				{
					_highGradeCharacters = new List<int>(num2);
				}
				else
				{
					_highGradeCharacters.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					_highGradeCharacters.Add(((int*)ptr)[i]);
				}
				ptr += 4 * num2;
			}
			else
			{
				_highGradeCharacters?.Clear();
			}
		}
		if (num > 1)
		{
			ushort num3 = *(ushort*)ptr;
			ptr += 2;
			if (num3 > 0)
			{
				if (_normalCharacters == null)
				{
					_normalCharacters = new List<int>(num3);
				}
				else
				{
					_normalCharacters.Clear();
				}
				for (int j = 0; j < num3; j++)
				{
					_normalCharacters.Add(((int*)ptr)[j]);
				}
				ptr += 4 * num3;
			}
			else
			{
				_normalCharacters?.Clear();
			}
		}
		if (num > 2)
		{
			ptr += _location.Deserialize(ptr);
		}
		if (num > 3)
		{
			ptr += Key.Deserialize(ptr);
		}
		if (num > 4)
		{
			State = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 5)
		{
			Month = *(int*)ptr;
			ptr += 4;
		}
		if (num > 6)
		{
			LastFinishDate = *(int*)ptr;
			ptr += 4;
		}
		int num4 = (int)(ptr - pData);
		return (num4 <= 4) ? num4 : ((num4 + 3) / 4 * 4);
	}
}
