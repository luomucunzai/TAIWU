using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.SpecialEffect.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong;

public class LoongWoodImplementRange : ISpecialEffectImplement, ISpecialEffectModifier
{
	private const int MinRangeSpace = 2;

	private readonly int _markToReduceRange;

	private readonly int _markToAddRange;

	private DataUid _defeatMarkUid;

	private int _injuryMarkCount;

	public CombatSkillEffectBase EffectBase { get; set; }

	public LoongWoodImplementRange(int markToReduceRange, int markToAddRange)
	{
		_markToReduceRange = markToReduceRange;
		_markToAddRange = markToAddRange;
	}

	public void OnEnable(DataContext context)
	{
		EffectBase.CreateAffectedData(145, (EDataModifyType)0, -1);
		EffectBase.CreateAffectedData(146, (EDataModifyType)0, -1);
		EffectBase.CreateAffectedAllEnemyData(273, (EDataModifyType)0, -1);
		_defeatMarkUid = new DataUid(8, 10, (ulong)EffectBase.CharacterId, 50u);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_defeatMarkUid, EffectBase.DataHandlerKey, OnDefeatMarkChanged);
	}

	public void OnDisable(DataContext context)
	{
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_defeatMarkUid, EffectBase.DataHandlerKey);
	}

	private void OnDefeatMarkChanged(DataContext context, DataUid _)
	{
		DefeatMarkCollection defeatMarkCollection = EffectBase.CombatChar.GetDefeatMarkCollection();
		int totalInjuryCount = defeatMarkCollection.GetTotalInjuryCount();
		if (totalInjuryCount == _injuryMarkCount)
		{
			return;
		}
		_injuryMarkCount = totalInjuryCount;
		DomainManager.SpecialEffect.InvalidateCache(context, EffectBase.CharacterId, 145);
		DomainManager.SpecialEffect.InvalidateCache(context, EffectBase.CharacterId, 146);
		int[] characterList = DomainManager.Combat.GetCharacterList(!EffectBase.CombatChar.IsAlly);
		int[] array = characterList;
		foreach (int num in array)
		{
			if (num >= 0)
			{
				DomainManager.SpecialEffect.InvalidateCache(context, num, 273);
			}
		}
	}

	public int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		ushort fieldId = dataKey.FieldId;
		bool flag = (uint)(fieldId - 145) <= 1u;
		if (flag && dataKey.CharId == EffectBase.CharacterId)
		{
			return _injuryMarkCount * _markToAddRange;
		}
		if (dataKey.FieldId == 273 && dataKey.CharId != EffectBase.CharacterId)
		{
			int customParam = dataKey.CustomParam0;
			int customParam2 = dataKey.CustomParam1;
			int num = customParam2 - customParam;
			return Math.Min((num - 2) / 2, _injuryMarkCount * _markToReduceRange);
		}
		return 0;
	}
}
