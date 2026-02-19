using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Throw;

public class GuiGuXueHaiTang : CombatSkillEffectBase
{
	private const sbyte DirectAddPowerUnit = 20;

	private const sbyte ReverseAddPowerUnit = 40;

	private const int DirectPoisonResistAddPercent = -50;

	private List<DataUid> _allCharMarkUid;

	public GuiGuXueHaiTang()
	{
	}

	public GuiGuXueHaiTang(CombatSkillKey skillKey)
		: base(skillKey, 10408, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Events.RegisterHandler_CombatBegin(OnCombatBegin);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CombatBegin(OnCombatBegin);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
		if (_allCharMarkUid == null)
		{
			return;
		}
		foreach (DataUid item in _allCharMarkUid)
		{
			GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(item, base.DataHandlerKey);
		}
	}

	private void OnCombatBegin(DataContext context)
	{
		AppendAffectedAllCombatCharData(context, 199, (EDataModifyType)1, -1);
		if (base.IsDirect)
		{
			AppendAffectedAllCombatCharData(context, 245, (EDataModifyType)1, -1);
		}
		else
		{
			AppendAffectedData(context, base.CharacterId, 162, (EDataModifyType)3, -1);
		}
		_allCharMarkUid = new List<DataUid>();
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		DomainManager.Combat.GetAllCharInCombat(list);
		for (int i = 0; i < list.Count; i++)
		{
			DataUid dataUid = new DataUid(8, 10, (ulong)list[i], 50u);
			_allCharMarkUid.Add(dataUid);
			GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(dataUid, base.DataHandlerKey, OnMarkChanged);
		}
		ObjectPool<List<int>>.Instance.Return(list);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (interrupted)
		{
			return;
		}
		if (SkillKey.IsMatch(charId, skillId) && PowerMatchAffectRequire(power))
		{
			if (base.EffectCount != base.MaxEffectCount)
			{
				if (base.EffectCount > 0)
				{
					DomainManager.Combat.ChangeSkillEffectToMaxCount(context, base.CombatChar, base.EffectKey);
				}
				else
				{
					AddMaxEffectCount();
				}
			}
			if (base.IsDirect)
			{
				InvalidateAllEnemyCache(context, 245);
			}
		}
		if (Config.CombatSkill.Instance[skillId].EquipType != 1)
		{
			return;
		}
		CombatCharacter element_CombatCharacterDict = DomainManager.Combat.GetElement_CombatCharacterDict(charId);
		if (!(base.IsDirect ? (element_CombatCharacterDict.IsAlly == base.CombatChar.IsAlly) : (element_CombatCharacterDict.GetId() != base.CharacterId)) && element_CombatCharacterDict.GetDefeatMarkCollection().PoisonMarkList.Exist((byte count) => count > 0) && base.EffectCount > 0)
		{
			DomainManager.Combat.AppendDieDefeatMark(context, element_CombatCharacterDict, SkillKey, 1);
			ShowSpecialEffectTips(0);
			ReduceEffectCount();
			if (element_CombatCharacterDict.GetDefeatMarkCollection().DieMarkList.Count == GameData.Domains.Combat.SharedConstValue.DefeatNeedDieMarkCount && !DomainManager.Combat.CheckHealthImmunity(context, element_CombatCharacterDict))
			{
				GameData.Domains.Character.Character character = element_CombatCharacterDict.GetCharacter();
				DomainManager.SpecialEffect.AddAddMaxHealthEffect(context, character.GetId(), -character.GetLeftMaxHealth());
				character.ChangeHealth(context, 0);
			}
		}
	}

	private void OnMarkChanged(DataContext context, DataUid dataUid)
	{
		InvalidateCache(context, (int)dataUid.SubId0, 199);
		InvalidateCache(context, (int)dataUid.SubId0, 232);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId && dataKey.FieldId == 245 && base.EffectCount > 0)
		{
			return -50;
		}
		ushort fieldId = dataKey.FieldId;
		if (1 == 0)
		{
		}
		int num = ((fieldId == 199) ? (base.IsDirect ? 20 : 40) : 0);
		if (1 == 0)
		{
		}
		int num2 = num;
		if (num2 == 0)
		{
			return 0;
		}
		DefeatMarkCollection defeatMarkCollection = DomainManager.Combat.GetElement_CombatCharacterDict(dataKey.CharId).GetDefeatMarkCollection();
		return num2 * defeatMarkCollection.DieMarkList.Count((CombatSkillKey t) => t.Equals(SkillKey));
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		if (dataKey.CharId == base.CharacterId && dataKey.FieldId == 162 && base.EffectCount > 0)
		{
			ShowSpecialEffectTipsOnceInFrame(1);
			return false;
		}
		return dataValue;
	}
}
