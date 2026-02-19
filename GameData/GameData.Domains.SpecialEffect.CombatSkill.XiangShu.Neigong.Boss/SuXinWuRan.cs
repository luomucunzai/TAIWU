using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Neigong.Boss;

public class SuXinWuRan : BossNeigongBase
{
	private const sbyte AddPowerUnit = 20;

	private readonly List<DataUid> _enemyMarkUids = new List<DataUid>();

	public SuXinWuRan()
	{
	}

	public SuXinWuRan(CombatSkillKey skillKey)
		: base(skillKey, 16112)
	{
	}

	public override void OnDisable(DataContext context)
	{
		base.OnDisable(context);
		foreach (DataUid enemyMarkUid in _enemyMarkUids)
		{
			GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(enemyMarkUid, base.DataHandlerKey);
		}
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	protected override void ActivePhase2Effect(DataContext context)
	{
		int[] characterList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
		foreach (int num in characterList)
		{
			if (num >= 0)
			{
				DataUid dataUid = ParseCombatCharacterDataUid(num, 50);
				_enemyMarkUids.Add(dataUid);
				GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(dataUid, base.DataHandlerKey, OnMarkChanged);
			}
		}
		AppendAffectedAllEnemyData(context, 199, (EDataModifyType)1, -1);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId == base.CharacterId && power >= 100 && Config.CombatSkill.Instance[skillId].EquipType == 1)
		{
			if (base.CurrEnemyChar.GetDefeatMarkCollection().DieMarkList.Count == SharedConstValue.DefeatNeedDieMarkCount - 1 && !DomainManager.Combat.CheckHealthImmunity(context, base.CurrEnemyChar))
			{
				base.CurrEnemyChar.GetCharacter().SetHealth(0, context);
			}
			DomainManager.Combat.AppendDieDefeatMark(context, base.CurrEnemyChar, SkillKey, 1);
			ShowSpecialEffectTips(1);
		}
	}

	private void OnMarkChanged(DataContext context, DataUid dataUid)
	{
		DomainManager.SpecialEffect.InvalidateCache(context, (int)dataUid.SubId0, 199);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.FieldId == 199)
		{
			DefeatMarkCollection defeatMarkCollection = DomainManager.Combat.GetElement_CombatCharacterDict(dataKey.CharId).GetDefeatMarkCollection();
			int num = 0;
			for (int i = 0; i < defeatMarkCollection.DieMarkList.Count; i++)
			{
				if (defeatMarkCollection.DieMarkList[i].Equals(SkillKey))
				{
					num++;
				}
			}
			return 20 * num;
		}
		return 0;
	}
}
