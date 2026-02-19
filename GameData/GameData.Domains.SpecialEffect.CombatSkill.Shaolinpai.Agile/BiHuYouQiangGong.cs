using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Agile;

public class BiHuYouQiangGong : AgileSkillBase
{
	private const sbyte ChangePower = 20;

	private bool _affecting;

	private bool _firstMoveSkillChanged = true;

	private DataUid _directDefendSkillUid;

	private short _directAffectingDefendSkill;

	private Dictionary<int, DataUid> _reverseDefendSkillUidDict;

	private Dictionary<int, short> _reverseAffectingDefendSkillDict;

	public BiHuYouQiangGong()
	{
	}

	public BiHuYouQiangGong(CombatSkillKey skillKey)
		: base(skillKey, 1401)
	{
		AutoRemove = false;
		ListenCanAffectChange = true;
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		_affecting = false;
		OnMoveSkillCanAffectChanged(context, default(DataUid));
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		if (base.IsDirect)
		{
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, -1), (EDataModifyType)1);
			_directAffectingDefendSkill = base.CombatChar.GetAffectingDefendSkillId();
			_directDefendSkillUid = new DataUid(8, 10, (ulong)base.CharacterId, 63u);
			GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_directDefendSkillUid, base.DataHandlerKey, OnDefendSkillChanged);
		}
		else
		{
			_reverseDefendSkillUidDict = new Dictionary<int, DataUid>();
			_reverseAffectingDefendSkillDict = new Dictionary<int, short>();
			int[] characterList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
			foreach (int num in characterList)
			{
				if (num >= 0)
				{
					AffectDatas.Add(new AffectedDataKey(num, 199, -1), (EDataModifyType)1);
					short affectingDefendSkillId = DomainManager.Combat.GetElement_CombatCharacterDict(num).GetAffectingDefendSkillId();
					if (affectingDefendSkillId >= 0)
					{
						_reverseAffectingDefendSkillDict.Add(num, affectingDefendSkillId);
					}
					DataUid dataUid = new DataUid(8, 10, (ulong)num, 63u);
					_reverseDefendSkillUidDict.Add(num, dataUid);
					GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(dataUid, base.DataHandlerKey, OnDefendSkillChanged);
				}
			}
		}
		ShowSpecialEffectTips(0);
	}

	public override void OnDisable(DataContext context)
	{
		base.OnDisable(context);
		if (base.IsDirect)
		{
			GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_directDefendSkillUid, base.DataHandlerKey);
			return;
		}
		foreach (DataUid value in _reverseDefendSkillUidDict.Values)
		{
			GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(value, base.DataHandlerKey);
		}
	}

	private void OnDefendSkillChanged(DataContext context, DataUid dataUid)
	{
		if (base.IsDirect)
		{
			_directAffectingDefendSkill = base.CombatChar.GetAffectingDefendSkillId();
		}
		else
		{
			int num = (int)dataUid.SubId0;
			short affectingDefendSkillId = DomainManager.Combat.GetElement_CombatCharacterDict(num).GetAffectingDefendSkillId();
			if (affectingDefendSkillId >= 0)
			{
				_reverseAffectingDefendSkillDict.Add(num, affectingDefendSkillId);
			}
			else
			{
				_reverseAffectingDefendSkillDict.Remove(num);
			}
		}
		if (AgileSkillChanged && (base.IsDirect ? (_directAffectingDefendSkill < 0) : (_reverseAffectingDefendSkillDict.Count <= 0)))
		{
			RemoveSelf(context);
		}
	}

	protected override void OnMoveSkillChanged(DataContext context, DataUid dataUid)
	{
		if (_firstMoveSkillChanged)
		{
			_firstMoveSkillChanged = false;
		}
		else if (base.IsDirect ? (_directAffectingDefendSkill < 0) : (_reverseAffectingDefendSkillDict.Count <= 0))
		{
			RemoveSelf(context);
		}
		else
		{
			AgileSkillChanged = true;
		}
	}

	protected override void OnMoveSkillCanAffectChanged(DataContext context, DataUid dataUid)
	{
		bool canAffect = base.CanAffect;
		if (_affecting == canAffect)
		{
			return;
		}
		_affecting = canAffect;
		if (base.IsDirect)
		{
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			return;
		}
		int[] characterList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
		for (int i = 0; i < characterList.Length; i++)
		{
			if (characterList[i] >= 0)
			{
				DomainManager.SpecialEffect.InvalidateCache(context, characterList[i], 199);
			}
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (!_affecting || Config.CombatSkill.Instance[dataKey.CombatSkillId].EquipType != 3)
		{
			return 0;
		}
		if (dataKey.FieldId == 199)
		{
			return base.IsDirect ? 20 : (-20);
		}
		return 0;
	}
}
