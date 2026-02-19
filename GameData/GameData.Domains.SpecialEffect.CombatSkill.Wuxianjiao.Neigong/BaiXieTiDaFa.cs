using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Neigong;

public class BaiXieTiDaFa : CombatSkillEffectBase
{
	private const sbyte PowerChangePerFeature = 3;

	private int _selfAddPower;

	private DataUid _selfNeiliAllocationUid;

	private Dictionary<int, int> _enemyReducePowers;

	private List<DataUid> _enemyNeiliAllocationUids;

	private bool _affected;

	public BaiXieTiDaFa()
	{
	}

	public BaiXieTiDaFa(CombatSkillKey skillKey)
		: base(skillKey, 12006, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		if (base.IsDirect)
		{
			_affected = true;
			_selfAddPower = CalcChangePower(base.CharacterId);
			_selfNeiliAllocationUid = ParseCharDataUid(17);
			GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_selfNeiliAllocationUid, base.DataHandlerKey, OnFeaturesChange);
			AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, -1), (EDataModifyType)1);
		}
		else
		{
			Events.RegisterHandler_CombatBegin(OnCombatBegin);
			Events.RegisterHandler_CombatCharChanged(OnCombatCharChanged);
			Events.RegisterHandler_CombatSettlement(OnCombatSettlement);
		}
	}

	public override void OnDisable(DataContext context)
	{
		if (base.IsDirect)
		{
			GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_selfNeiliAllocationUid, base.DataHandlerKey);
			return;
		}
		Events.UnRegisterHandler_CombatBegin(OnCombatBegin);
		Events.UnRegisterHandler_CombatCharChanged(OnCombatCharChanged);
		Events.UnRegisterHandler_CombatSettlement(OnCombatSettlement);
	}

	private void OnCombatBegin(DataContext context)
	{
		if (!DomainManager.Combat.IsCharInCombat(base.CharacterId))
		{
			return;
		}
		_affected = base.IsDirect || base.IsCurrent;
		_enemyReducePowers = new Dictionary<int, int>();
		_enemyNeiliAllocationUids = new List<DataUid>();
		int[] characterList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
		foreach (int num in characterList)
		{
			if (num >= 0)
			{
				_enemyReducePowers.Add(num, CalcChangePower(num));
				DataUid dataUid = new DataUid(4, 0, (ulong)num, 17u);
				GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(dataUid, base.DataHandlerKey, OnFeaturesChange);
				_enemyNeiliAllocationUids.Add(dataUid);
				AppendAffectedData(context, num, 199, (EDataModifyType)1, -1);
			}
		}
	}

	private void OnCombatCharChanged(DataContext context, bool isAlly)
	{
		if (DomainManager.Combat.IsCharInCombat(base.CharacterId))
		{
			bool flag = base.IsDirect || base.IsCurrent;
			if (flag != _affected)
			{
				_affected = flag;
				InvalidateAllEnemyCache(context, 199);
			}
		}
	}

	private void OnCombatSettlement(DataContext context, sbyte combatStatus)
	{
		if (_enemyReducePowers != null)
		{
			for (int i = 0; i < _enemyNeiliAllocationUids.Count; i++)
			{
				GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_enemyNeiliAllocationUids[i], base.DataHandlerKey);
			}
			_enemyReducePowers = null;
			_enemyNeiliAllocationUids = null;
			ClearAffectedData(context);
		}
	}

	private void OnFeaturesChange(DataContext context, DataUid dataUid)
	{
		int num = (int)dataUid.SubId0;
		int num2 = CalcChangePower(num);
		if (base.IsDirect)
		{
			_selfAddPower = num2;
		}
		else
		{
			_enemyReducePowers[num] = num2;
		}
		DomainManager.SpecialEffect.InvalidateCache(context, num, 199);
	}

	private int CalcChangePower(int charId)
	{
		List<short> featureIds = DomainManager.Character.GetElement_Objects(charId).GetFeatureIds();
		int num = 0;
		for (int i = 0; i < featureIds.Count; i++)
		{
			CharacterFeatureItem config = CharacterFeature.Instance[featureIds[i]];
			if (base.IsDirect ? config.IsBad() : config.IsGood())
			{
				num += (base.IsDirect ? 3 : (-3));
			}
		}
		return num;
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.FieldId == 199 && _affected)
		{
			return base.IsDirect ? _selfAddPower : _enemyReducePowers[dataKey.CharId];
		}
		return 0;
	}
}
