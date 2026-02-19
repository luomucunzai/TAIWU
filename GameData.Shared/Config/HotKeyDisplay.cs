using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class HotKeyDisplay : ConfigData<HotKeyDisplayItem, short>
{
	public static HotKeyDisplay Instance = new HotKeyDisplay();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Type", "DisplayText" };

	internal override int ToInt(short value)
	{
		return value;
	}

	internal override short ToTemplateId(int value)
	{
		return (short)value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new HotKeyDisplayItem(0, EHotKeyDisplayType.GetItem, 0, new List<HotkeyIndex>
		{
			new HotkeyIndex(1, 2),
			new HotkeyIndex(1, 5),
			new HotkeyIndex(1, 3)
		}));
		_dataArray.Add(new HotKeyDisplayItem(1, EHotKeyDisplayType.CGAnimation, 1, new List<HotkeyIndex>
		{
			new HotkeyIndex(1, 5)
		}));
		_dataArray.Add(new HotKeyDisplayItem(2, EHotKeyDisplayType.DigAnimations, 2, new List<HotkeyIndex>
		{
			new HotkeyIndex(1, 3)
		}));
		_dataArray.Add(new HotKeyDisplayItem(3, EHotKeyDisplayType.CBReadiness, 3, new List<HotkeyIndex>
		{
			new HotkeyIndex(1, 5)
		}));
		_dataArray.Add(new HotKeyDisplayItem(4, EHotKeyDisplayType.CBMoveAutomaticallyTips, 4, new List<HotkeyIndex>
		{
			new HotkeyIndex(1, 3)
		}));
		_dataArray.Add(new HotKeyDisplayItem(5, EHotKeyDisplayType.CBAttackTips, 5, new List<HotkeyIndex>
		{
			new HotkeyIndex(1, 5),
			new HotkeyIndex(4, 3)
		}));
		_dataArray.Add(new HotKeyDisplayItem(6, EHotKeyDisplayType.CBEquipmentTips, 6, new List<HotkeyIndex>
		{
			new HotkeyIndex(1, 8)
		}));
		_dataArray.Add(new HotKeyDisplayItem(7, EHotKeyDisplayType.CBCombatSkillTips, 6, new List<HotkeyIndex>
		{
			new HotkeyIndex(1, 8)
		}));
		_dataArray.Add(new HotKeyDisplayItem(8, EHotKeyDisplayType.AttentivelyTips, 7, new List<HotkeyIndex>
		{
			new HotkeyIndex(1, 5)
		}));
		_dataArray.Add(new HotKeyDisplayItem(9, EHotKeyDisplayType.DedicatedTips, 7, new List<HotkeyIndex>
		{
			new HotkeyIndex(1, 5)
		}));
		_dataArray.Add(new HotKeyDisplayItem(10, EHotKeyDisplayType.CompareEquipment, 8, new List<HotkeyIndex>
		{
			new HotkeyIndex(1, 10)
		}));
		_dataArray.Add(new HotKeyDisplayItem(11, EHotKeyDisplayType.DetailAndCompareEquipment, 9, new List<HotkeyIndex>
		{
			new HotkeyIndex(1, 9),
			new HotkeyIndex(1, 10)
		}));
		_dataArray.Add(new HotKeyDisplayItem(12, EHotKeyDisplayType.AllTips, 10, new List<HotkeyIndex>()));
		_dataArray.Add(new HotKeyDisplayItem(13, EHotKeyDisplayType.Detail, 6, new List<HotkeyIndex>
		{
			new HotkeyIndex(1, 9)
		}));
		_dataArray.Add(new HotKeyDisplayItem(14, EHotKeyDisplayType.PracticeCombatSkillTips, 11, new List<HotkeyIndex>
		{
			new HotkeyIndex(1, 5)
		}));
		_dataArray.Add(new HotKeyDisplayItem(15, EHotKeyDisplayType.CancelDetail, 12, new List<HotkeyIndex>
		{
			new HotkeyIndex(1, 9)
		}));
		_dataArray.Add(new HotKeyDisplayItem(16, EHotKeyDisplayType.CancelCompareEquipment, 13, new List<HotkeyIndex>
		{
			new HotkeyIndex(1, 10)
		}));
		_dataArray.Add(new HotKeyDisplayItem(17, EHotKeyDisplayType.Interaction, 14, new List<HotkeyIndex>
		{
			new HotkeyIndex(1, 8)
		}));
		_dataArray.Add(new HotKeyDisplayItem(18, EHotKeyDisplayType.CancelInteraction, 12, new List<HotkeyIndex>
		{
			new HotkeyIndex(1, 8)
		}));
		_dataArray.Add(new HotKeyDisplayItem(19, EHotKeyDisplayType.CombatResult, 15, new List<HotkeyIndex>
		{
			new HotkeyIndex(1, 5),
			new HotkeyIndex(1, 2)
		}));
		_dataArray.Add(new HotKeyDisplayItem(20, EHotKeyDisplayType.ExtraordinaryCricket, 15, new List<HotkeyIndex>
		{
			new HotkeyIndex(1, 5),
			new HotkeyIndex(1, 2)
		}));
		_dataArray.Add(new HotKeyDisplayItem(21, EHotKeyDisplayType.TasterUltimateResult, 15, new List<HotkeyIndex>
		{
			new HotkeyIndex(1, 2),
			new HotkeyIndex(1, 3)
		}));
		_dataArray.Add(new HotKeyDisplayItem(22, EHotKeyDisplayType.ProfessionSkillUnlockedSkip, 16, new List<HotkeyIndex>
		{
			new HotkeyIndex(1, 5)
		}));
		_dataArray.Add(new HotKeyDisplayItem(23, EHotKeyDisplayType.ProfessionSkillUnlockedClose, 17, new List<HotkeyIndex>
		{
			new HotkeyIndex(1, 5),
			new HotkeyIndex(1, 3),
			new HotkeyIndex(1, 2)
		}));
		_dataArray.Add(new HotKeyDisplayItem(24, EHotKeyDisplayType.InteractCheckUnlockedSkip, 16, new List<HotkeyIndex>
		{
			new HotkeyIndex(1, 5)
		}));
		_dataArray.Add(new HotKeyDisplayItem(25, EHotKeyDisplayType.InteractCheckUnlockedClose, 17, new List<HotkeyIndex>
		{
			new HotkeyIndex(1, 5),
			new HotkeyIndex(1, 3),
			new HotkeyIndex(1, 2)
		}));
		_dataArray.Add(new HotKeyDisplayItem(26, EHotKeyDisplayType.SectMainStoryUnlock, 17, new List<HotkeyIndex>
		{
			new HotkeyIndex(1, 5),
			new HotkeyIndex(1, 1),
			new HotkeyIndex(1, 2)
		}));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<HotKeyDisplayItem>(27);
		CreateItems0();
	}
}
