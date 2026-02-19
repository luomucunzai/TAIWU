using System.Collections.Generic;
using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions;

[SerializableGameData(NotForDisplayModule = true)]
public class ConfigWrapperAction : MonthlyActionBase, ISerializableGameData
{
	[field: SerializableGameDataField]
	public ConfigMonthlyAction CurrConfigMonthlyAction { get; private set; }

	public ConfigWrapperAction(MonthlyActionKey key)
	{
		Key = key;
	}

	public void CreateWrappedAction(short templateId, short assignedAreaId = -1)
	{
		if (CurrConfigMonthlyAction == null)
		{
			CurrConfigMonthlyAction = new ConfigMonthlyAction(templateId, assignedAreaId);
			CurrConfigMonthlyAction.Key = Key;
			CurrConfigMonthlyAction.SelectLocation();
			CurrConfigMonthlyAction.TriggerAction();
			if (CurrConfigMonthlyAction.State == 0)
			{
				CurrConfigMonthlyAction = null;
			}
		}
	}

	public override void MonthlyHandler()
	{
		CurrConfigMonthlyAction?.MonthlyHandler();
	}

	public override void ValidationHandler()
	{
		CurrConfigMonthlyAction?.ValidationHandler();
	}

	public override void Deactivate(bool isComplete)
	{
		CurrConfigMonthlyAction.Deactivate(isComplete);
		CurrConfigMonthlyAction = null;
	}

	public override void FillEventArgBox(EventArgBox eventArgBox)
	{
		if (CurrConfigMonthlyAction != null)
		{
			eventArgBox.Get<Location>("AdventureLocation", out Location arg);
			if (arg.IsValid())
			{
				CurrConfigMonthlyAction.EnsurePrerequisites();
				CurrConfigMonthlyAction.FillEventArgBox(eventArgBox);
			}
		}
	}

	public override void CollectCalledCharacters(HashSet<int> calledCharacters)
	{
		CurrConfigMonthlyAction?.CollectCalledCharacters(calledCharacters);
	}

	public override MonthlyActionBase CreateCopy()
	{
		return GameData.Serializer.Serializer.CreateCopy(this);
	}

	public ConfigWrapperAction()
	{
	}

	public override bool IsSerializedSizeFixed()
	{
		return false;
	}

	public override int GetSerializedSize()
	{
		int num = 12;
		num = ((CurrConfigMonthlyAction == null) ? (num + 2) : (num + (2 + CurrConfigMonthlyAction.GetSerializedSize())));
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe override int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (CurrConfigMonthlyAction != null)
		{
			byte* ptr2 = ptr;
			ptr += 2;
			int num = CurrConfigMonthlyAction.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)ptr2 = (ushort)num;
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
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (CurrConfigMonthlyAction == null)
			{
				CurrConfigMonthlyAction = new ConfigMonthlyAction();
			}
			ptr += CurrConfigMonthlyAction.Deserialize(ptr);
		}
		else
		{
			CurrConfigMonthlyAction = null;
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
