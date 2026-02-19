using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.XueFeng;

public class XueYuYuBaHuang : CombatSkillEffectBase
{
	private const sbyte AddPowerUnit = 10;

	private const sbyte TransferInjuryCount = 8;

	private int _addPower;

	public XueYuYuBaHuang()
	{
	}

	public XueYuYuBaHuang(CombatSkillKey skillKey)
		: base(skillKey, 17075, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		_addPower = 10 * base.CombatChar.GetOldInjuries().GetSum();
		if (_addPower > 0)
		{
			CreateAffectedData(199, (EDataModifyType)1, base.SkillTemplateId);
			ShowSpecialEffectTips(0);
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
			List<(sbyte, bool)> list = ObjectPool<List<(sbyte, bool)>>.Instance.Get();
			List<sbyte> list2 = ObjectPool<List<sbyte>>.Instance.Get();
			Injuries oldInjuries = base.CombatChar.GetOldInjuries();
			list.Clear();
			for (sbyte b = 0; b < 7; b++)
			{
				(sbyte, sbyte) tuple = oldInjuries.Get(b);
				for (int i = 0; i < tuple.Item1; i++)
				{
					list.Add((b, false));
				}
				for (int j = 0; j < tuple.Item2; j++)
				{
					list.Add((b, true));
				}
			}
			while (list.Count > 8)
			{
				list.RemoveAt(context.Random.Next(list.Count));
			}
			if (list.Count > 0)
			{
				int[] characterList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
				List<CombatCharacter> list3 = new List<CombatCharacter>();
				CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
				for (int k = 0; k < characterList.Length; k++)
				{
					if (characterList[k] >= 0)
					{
						list3.Add(DomainManager.Combat.GetElement_CombatCharacterDict(characterList[k]));
					}
				}
				for (int l = 0; l < list.Count; l++)
				{
					(sbyte, bool) tuple2 = list[l];
					int index = context.Random.Next(list3.Count);
					CombatCharacter combatCharacter2 = list3[index];
					Injuries injuries = combatCharacter2.GetInjuries();
					list2.Clear();
					for (sbyte b2 = 0; b2 < 7; b2++)
					{
						if (injuries.Get(b2, tuple2.Item2) < 6)
						{
							list2.Add(b2);
						}
					}
					if (list2.Count > 0)
					{
						sbyte bodyPart = list2[context.Random.Next(list2.Count)];
						DomainManager.Combat.RemoveInjury(context, base.CombatChar, tuple2.Item1, tuple2.Item2, 1, updateDefeatMark: false, removeOldInjury: true);
						DomainManager.Combat.AddInjury(context, combatCharacter2, bodyPart, tuple2.Item2, 1, updateDefeatMark: true, changeToOld: true);
						continue;
					}
					list3.RemoveAt(index);
					if (list3.Count > 0)
					{
						l--;
						continue;
					}
					break;
				}
				DomainManager.Combat.UpdateBodyDefeatMark(context, base.CombatChar);
				DomainManager.Combat.AddToCheckFallenSet(combatCharacter.GetId());
				if (combatCharacter != base.CurrEnemyChar)
				{
					DomainManager.Combat.AddToCheckFallenSet(base.CurrEnemyChar.GetId());
				}
				ShowSpecialEffectTips(1);
			}
			ObjectPool<List<(sbyte, bool)>>.Instance.Return(list);
			ObjectPool<List<sbyte>>.Instance.Return(list2);
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
			return _addPower;
		}
		return 0;
	}
}
