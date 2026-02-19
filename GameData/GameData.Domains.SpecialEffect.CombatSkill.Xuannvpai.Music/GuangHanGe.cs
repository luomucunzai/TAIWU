using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Music;

public class GuangHanGe : CombatSkillEffectBase
{
	private const sbyte AddPower = 40;

	private const sbyte AddTrickOrMarkCount = 3;

	private const int NeiliAllocationPercent = 20;

	public GuangHanGe()
	{
	}

	public GuangHanGe(CombatSkillKey skillKey)
		: base(skillKey, 8306, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		CombatDomain combat = DomainManager.Combat;
		bool flag = combat.AnyTeammateChar(base.CombatChar.IsAlly);
		bool flag2 = combat.AnyTeammateChar(!base.CombatChar.IsAlly);
		bool flag3 = (base.IsDirect ? (!flag2) : (!flag));
		bool flag4 = ((!base.IsDirect) ? (flag && !flag2) : (flag2 && !flag));
		if (flag3)
		{
			AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId), (EDataModifyType)1);
			ShowSpecialEffectTips(0);
		}
		if (flag4)
		{
			CombatCharacter combatCharacter = (base.IsDirect ? base.CombatChar : base.CurrEnemyChar);
			int[] characterList = combat.GetCharacterList(base.IsDirect ? (!base.CombatChar.IsAlly) : base.CombatChar.IsAlly);
			List<int> list = ObjectPool<List<int>>.Instance.Get();
			for (int i = 1; i < characterList.Length; i++)
			{
				if (characterList[i] >= 0)
				{
					list.Add(characterList[i]);
				}
			}
			int random = list.GetRandom(context.Random);
			CombatCharacter element_CombatCharacterDict = combat.GetElement_CombatCharacterDict(random);
			ObjectPool<List<int>>.Instance.Return(list);
			NeiliAllocation neiliAllocation = element_CombatCharacterDict.GetNeiliAllocation();
			for (byte b = 0; b < 4; b++)
			{
				int num = neiliAllocation[b] * 20 / 100;
				if (num > 0)
				{
					combatCharacter.ChangeNeiliAllocation(context, b, base.IsDirect ? num : (-num));
				}
			}
			ShowSpecialEffectTips(2);
		}
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		if (PowerMatchAffectRequire(power))
		{
			if (base.IsDirect)
			{
				DomainManager.Combat.AddTrick(context, base.CurrEnemyChar, 20, 3, addedByAlly: false);
			}
			else
			{
				DomainManager.Combat.AppendMindDefeatMark(context, base.CurrEnemyChar, 3, -1);
			}
			ShowSpecialEffectTips(1);
		}
		RemoveSelf(context);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId)
		{
			return 0;
		}
		if (dataKey.FieldId == 199)
		{
			return 40;
		}
		return 0;
	}
}
