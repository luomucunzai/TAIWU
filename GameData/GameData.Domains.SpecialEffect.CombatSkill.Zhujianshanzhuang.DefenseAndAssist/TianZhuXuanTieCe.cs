using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.DefenseAndAssist;

public class TianZhuXuanTieCe : AssistSkillBase
{
	private const int MaxAddPower = 100;

	private const int GodRequirePower = 50;

	private static readonly sbyte[] RequireLifeSkillTypes = new sbyte[4] { 6, 7, 11, 10 };

	private static readonly int[] RequireAttainments = new int[9] { 80, 120, 160, 200, 240, 280, 320, 360, 400 };

	private readonly Dictionary<int, int> _itemAddedPower = new Dictionary<int, int>();

	private bool _affecting;

	private DataUid _dataUid;

	public TianZhuXuanTieCe()
	{
	}

	public TianZhuXuanTieCe(CombatSkillKey skillKey)
		: base(skillKey, 9706)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		CreateAffectedData(315, (EDataModifyType)0, -1);
		CreateAffectedData((ushort)(base.IsDirect ? 181 : 182), (EDataModifyType)3, -1);
		_dataUid = ParseCharDataUid(57);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_dataUid, base.DataHandlerKey, OnEquipmentChanged);
		_affecting = false;
		Events.RegisterHandler_CombatBegin(OnCombatBegin);
	}

	public override void OnDisable(DataContext context)
	{
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_dataUid, base.DataHandlerKey);
		Events.UnRegisterHandler_CombatBegin(OnCombatBegin);
		base.OnDisable(context);
	}

	private void OnCombatBegin(DataContext context)
	{
		UpdateCanAffect(context);
		UpdateAddedPower(context);
	}

	protected override void OnCanUseChanged(DataContext context, DataUid dataUid)
	{
		UpdateCanAffect(context);
	}

	private void OnEquipmentChanged(DataContext context, DataUid dataUid)
	{
		UpdateCanAffect(context);
		UpdateAddedPower(context);
	}

	private bool IsItemAffected(int itemId)
	{
		int value;
		return _itemAddedPower.TryGetValue(itemId, out value) && value >= 50;
	}

	private void UpdateCanAffect(DataContext context)
	{
		bool canAffect = base.CanAffect;
		if (canAffect != _affecting)
		{
			_affecting = canAffect;
			SetConstAffecting(context, canAffect);
			if (canAffect)
			{
				ShowEffectTips(context);
			}
		}
	}

	private void UpdateAddedPower(DataContext context)
	{
		bool flag = false;
		ItemKey[] equipment = CharObj.GetEquipment();
		for (int i = 0; i < equipment.Length; i++)
		{
			ItemKey key = equipment[i];
			if (key.IsValid())
			{
				bool flag2 = IsItemAffected(key.Id);
				int num = CalcAddPowerValue(key);
				flag = flag || num != _itemAddedPower.GetOrDefault(key.Id);
				_itemAddedPower[key.Id] = num;
				bool flag3 = IsItemAffected(key.Id);
				if (!flag2 && flag3 && _affecting)
				{
					ShowSpecialEffectTipsOnceInFrame(0);
				}
			}
		}
		if (flag)
		{
			InvalidateCache(context, 315);
		}
	}

	private int CalcAddPowerValue(ItemKey key)
	{
		if ((int)key.ItemType != ((!base.IsDirect) ? 1 : 0))
		{
			return 0;
		}
		sbyte grade = ItemTemplateHelper.GetGrade(key.ItemType, key.TemplateId);
		int num = RequireAttainments[grade];
		int num2 = 0;
		sbyte[] requireLifeSkillTypes = RequireLifeSkillTypes;
		foreach (sbyte lifeSkillType in requireLifeSkillTypes)
		{
			short lifeSkillAttainment = CharObj.GetLifeSkillAttainment(lifeSkillType);
			num2 += Math.Min(lifeSkillAttainment * 100 / num, 100);
		}
		return num2 / RequireLifeSkillTypes.Length;
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (!_affecting)
		{
			return 0;
		}
		ushort fieldId = dataKey.FieldId;
		if (1 == 0)
		{
		}
		int result = ((fieldId == 315) ? _itemAddedPower.GetOrDefault(dataKey.CustomParam0) : 0);
		if (1 == 0)
		{
		}
		return result;
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		bool flag = !_affecting;
		bool flag2 = flag;
		if (!flag2)
		{
			ushort fieldId = dataKey.FieldId;
			bool flag3 = (uint)(fieldId - 181) <= 1u;
			flag2 = !flag3;
		}
		if (flag2)
		{
			return dataValue;
		}
		return IsItemAffected(dataKey.CustomParam0);
	}
}
