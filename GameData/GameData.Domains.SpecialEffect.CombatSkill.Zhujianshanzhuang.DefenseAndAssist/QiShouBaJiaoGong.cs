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

public class QiShouBaJiaoGong : AssistSkillBase
{
	private const sbyte RequireWeaponTypeCount = 3;

	private const sbyte AddSpeed = 30;

	private List<DataUid> _weaponDurabilityUids;

	private bool _affecting;

	public QiShouBaJiaoGong()
	{
	}

	public QiShouBaJiaoGong(CombatSkillKey skillKey)
		: base(skillKey, 9700)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, (ushort)(base.IsDirect ? 11 : 9), -1), (EDataModifyType)1);
		Events.RegisterHandler_CombatBegin(OnCombatBegin);
	}

	public override void OnDisable(DataContext context)
	{
		base.OnDisable(context);
		if (_weaponDurabilityUids != null)
		{
			for (int i = 0; i < _weaponDurabilityUids.Count; i++)
			{
				GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_weaponDurabilityUids[i], base.DataHandlerKey);
			}
		}
		Events.UnRegisterHandler_CombatBegin(OnCombatBegin);
	}

	private void OnCombatBegin(DataContext context)
	{
		_affecting = false;
		UpdateCanAffect(context, default(DataUid));
		_weaponDurabilityUids = new List<DataUid>();
		ItemKey[] weapons = base.CombatChar.GetWeapons();
		for (int i = 0; i < 3; i++)
		{
			ItemKey itemKey = weapons[i];
			if (itemKey.IsValid())
			{
				DataUid dataUid = new DataUid(8, 30, (ulong)itemKey, 3u);
				GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(dataUid, base.DataHandlerKey, UpdateCanAffect);
				_weaponDurabilityUids.Add(dataUid);
			}
		}
	}

	protected override void OnCanUseChanged(DataContext context, DataUid dataUid)
	{
		UpdateCanAffect(context, default(DataUid));
	}

	private void UpdateCanAffect(DataContext context, DataUid dataUid)
	{
		bool flag = base.CanAffect;
		if (flag)
		{
			ItemKey[] weapons = base.CombatChar.GetWeapons();
			List<short> list = ObjectPool<List<short>>.Instance.Get();
			list.Clear();
			for (int i = 0; i < 3; i++)
			{
				ItemKey itemKey = weapons[i];
				if (itemKey.IsValid())
				{
					short itemSubType = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId);
					if (!list.Contains(itemSubType))
					{
						list.Add(itemSubType);
					}
				}
			}
			flag = list.Count >= 3;
			ObjectPool<List<short>>.Instance.Return(list);
		}
		if (_affecting != flag)
		{
			_affecting = flag;
			SetConstAffecting(context, flag);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, (ushort)(base.IsDirect ? 11 : 9));
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || !_affecting)
		{
			return 0;
		}
		return 30;
	}
}
