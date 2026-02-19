using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions.CustomActions;

[SerializableGameData(NotForDisplayModule = true)]
[Obsolete]
public class CallCharacterMonthlyAction : MonthlyActionBase, ISerializableGameData
{
	[SerializableGameDataField]
	public Location Location;

	[SerializableGameDataField]
	public List<CharacterSet> MajorCharacterSets;

	[SerializableGameDataField]
	public List<CharacterSet> ParticipatingCharacterSets;

	public CallCharacterHelper.SearchCharacterRule MajorCharacterSearchRule;

	public CallCharacterHelper.SearchCharacterRule ParticipateCharacterSearchRule;

	public PreparationRule PreparationRule;

	public short AdventureTemplateId;

	public Action<CallCharacterMonthlyAction> OnWaitTrigger;

	public Action<CallCharacterMonthlyAction> OnTrigger;

	public override void TriggerAction()
	{
		if (State == 0 && Location.IsValid())
		{
			State = 1;
			if (AdventureTemplateId >= 0 && !DomainManager.Adventure.TryCreateAdventureSite(DomainManager.TaiwuEvent.MainThreadDataContext, Location.AreaId, Location.BlockId, AdventureTemplateId, Key))
			{
				State = 0;
				Location = Location.Invalid;
			}
			else
			{
				OnTrigger?.Invoke(this);
			}
		}
	}

	public override void MonthlyHandler()
	{
		if (State == 0)
		{
			OnWaitTrigger?.Invoke(this);
			return;
		}
		bool flag = PreparationRule.PreparationDuration > 0 && Month >= PreparationRule.PreparationDuration;
		if (State == 1)
		{
			CallMajorCharacters();
			if (MajorCharacterSearchRule.AllowTemporaryCharacter || CallCharacterHelper.IsAllCharactersAtLocation(Location, MajorCharacterSearchRule, MajorCharacterSets))
			{
				CallParticipateCharacters();
			}
		}
		if (State == 2)
		{
			CallParticipateCharacters();
		}
		if (State == 3 && (flag || PreparationRule.CanStartEarly))
		{
			if (ParticipateCharacterSearchRule.AllowTemporaryCharacter || CallCharacterHelper.IsAllCharactersAtLocation(Location, ParticipateCharacterSearchRule, ParticipatingCharacterSets))
			{
				Activate();
			}
			else if (flag)
			{
				Deactivate(isComplete: false);
			}
		}
		if (State != 0)
		{
			Month++;
		}
	}

	public override void Activate()
	{
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		if (AdventureTemplateId >= 0)
		{
			MonthlyEventActionsManager.NewlyActivated++;
			DomainManager.Adventure.ActivateAdventureSite(mainThreadDataContext, Location.AreaId, Location.BlockId);
		}
		State = 5;
	}

	public override void InheritNonArchiveData(MonthlyActionBase action)
	{
		if (!(action is CallCharacterMonthlyAction callCharacterMonthlyAction))
		{
			AdaptableLog.TagWarning("CallCharacterMonthlyAction", $"fail to inherit action {action} with key {action.Key} due to invalid type.", appendWarningMessage: true);
		}
		else
		{
			PreparationRule = callCharacterMonthlyAction.PreparationRule;
			MajorCharacterSearchRule = callCharacterMonthlyAction.MajorCharacterSearchRule;
			ParticipateCharacterSearchRule = callCharacterMonthlyAction.ParticipateCharacterSearchRule;
			AdventureTemplateId = callCharacterMonthlyAction.AdventureTemplateId;
		}
	}

	public override void EnsurePrerequisites()
	{
		int num = CallCharacterHelper.RemoveInvalidCharacters(MajorCharacterSearchRule, MajorCharacterSets);
		if (num > 0)
		{
			AdaptableLog.TagWarning("ConfigMonthlyAction", $"{Key} removed {num} major characters that are invalid");
		}
		int num2 = CallCharacterHelper.RemoveInvalidCharacters(ParticipateCharacterSearchRule, ParticipatingCharacterSets);
		if (num2 > 0)
		{
			AdaptableLog.TagWarning("ConfigMonthlyAction", $"{Key} removed {num2} participate characters that are invalid");
		}
		if (!MajorCharacterSearchRule.AllowTemporaryCharacter && !CallCharacterHelper.IsAllCharactersAtLocation(Location, MajorCharacterSearchRule, MajorCharacterSets))
		{
			CallMajorCharacters();
		}
		if (!ParticipateCharacterSearchRule.AllowTemporaryCharacter && !CallCharacterHelper.IsAllCharactersAtLocation(Location, ParticipateCharacterSearchRule, ParticipatingCharacterSets))
		{
			CallParticipateCharacters();
		}
		State = 5;
	}

	public override MonthlyActionBase CreateCopy()
	{
		return GameData.Serializer.Serializer.CreateCopy(this);
	}

	public void ClearCalledCharacters()
	{
		CallCharacterHelper.ClearCalledCharacters(MajorCharacterSets, unHideCharacters: false, removeExternalState: true);
		CallCharacterHelper.ClearCalledCharacters(ParticipatingCharacterSets, unHideCharacters: false, removeExternalState: true);
	}

	private void CallMajorCharacters()
	{
		if (Location.IsValid() && CallCharacterHelper.CallCharacters(Location, MajorCharacterSearchRule, MajorCharacterSets, modifyExternalState: true))
		{
			State = 2;
		}
	}

	private void CallParticipateCharacters()
	{
		if (Location.IsValid() && CallCharacterHelper.CallCharacters(Location, ParticipateCharacterSearchRule, ParticipatingCharacterSets, modifyExternalState: true))
		{
			State = 3;
		}
	}

	private void FillIntelligentCharactersToArgBox(EventArgBox eventArgBox, string keyPrefix, CharacterSet charSet, CallCharacterHelper.SearchCharacterSubRule searchSubRule, bool allowTempChars)
	{
		int i = 0;
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		foreach (int item in charSet.GetCollection())
		{
			if (!DomainManager.Character.TryGetElement_Objects(item, out var element))
			{
				continue;
			}
			if (element.IsCrossAreaTraveling())
			{
				DomainManager.Character.GroupMove(mainThreadDataContext, element, Location);
			}
			if (element.GetLocation().Equals(Location))
			{
				if (searchSubRule.MaxAmount > 0 && i >= searchSubRule.MaxAmount)
				{
					Events.RaiseCharacterLocationChanged(mainThreadDataContext, item, element.GetLocation(), Location);
					element.SetLocation(Location, mainThreadDataContext);
					continue;
				}
				eventArgBox.Set($"{keyPrefix}_{i}", item);
				i++;
			}
		}
		AdaptableLog.Info($"Adding {i} real characters to adventure.");
		if (allowTempChars && searchSubRule.CreateTemporaryCharacterFunc != null)
		{
			AdaptableLog.Info($"creating {Math.Max(0, searchSubRule.MinAmount - i)} temporary characters.");
			for (; i < searchSubRule.MinAmount; i++)
			{
				int num = searchSubRule.CreateTemporaryCharacterFunc(mainThreadDataContext);
				DomainManager.Adventure.AddTemporaryIntelligentCharacter(num);
				eventArgBox.Set($"{keyPrefix}_{i}", num);
			}
		}
		eventArgBox.Set(keyPrefix + "_Count", i);
	}

	public CallCharacterMonthlyAction()
	{
		MajorCharacterSets = new List<CharacterSet>();
		ParticipatingCharacterSets = new List<CharacterSet>();
	}

	public override bool IsSerializedSizeFixed()
	{
		return false;
	}

	public override int GetSerializedSize()
	{
		int num = 16;
		if (MajorCharacterSets != null)
		{
			num += 2;
			int count = MajorCharacterSets.Count;
			for (int i = 0; i < count; i++)
			{
				num += MajorCharacterSets[i].GetSerializedSize();
			}
		}
		else
		{
			num += 2;
		}
		if (ParticipatingCharacterSets != null)
		{
			num += 2;
			int count2 = ParticipatingCharacterSets.Count;
			for (int j = 0; j < count2; j++)
			{
				num += ParticipatingCharacterSets[j].GetSerializedSize();
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
		ptr += Location.Serialize(ptr);
		if (MajorCharacterSets != null)
		{
			int count = MajorCharacterSets.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				int num = MajorCharacterSets[i].Serialize(ptr);
				ptr += num;
				Tester.Assert(num <= 65535);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (ParticipatingCharacterSets != null)
		{
			int count2 = ParticipatingCharacterSets.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				int num2 = ParticipatingCharacterSets[j].Serialize(ptr);
				ptr += num2;
				Tester.Assert(num2 <= 65535);
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
		int num3 = (int)(ptr - pData);
		return (num3 <= 4) ? num3 : ((num3 + 3) / 4 * 4);
	}

	public unsafe override int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += Location.Deserialize(ptr);
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (MajorCharacterSets == null)
			{
				MajorCharacterSets = new List<CharacterSet>(num);
			}
			else
			{
				MajorCharacterSets.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				CharacterSet item = default(CharacterSet);
				ptr += item.Deserialize(ptr);
				MajorCharacterSets.Add(item);
			}
		}
		else
		{
			MajorCharacterSets?.Clear();
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (ParticipatingCharacterSets == null)
			{
				ParticipatingCharacterSets = new List<CharacterSet>(num2);
			}
			else
			{
				ParticipatingCharacterSets.Clear();
			}
			for (int j = 0; j < num2; j++)
			{
				CharacterSet item2 = default(CharacterSet);
				ptr += item2.Deserialize(ptr);
				ParticipatingCharacterSets.Add(item2);
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
		int num3 = (int)(ptr - pData);
		return (num3 <= 4) ? num3 : ((num3 + 3) / 4 * 4);
	}
}
