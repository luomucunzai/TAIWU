using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.FistAndPalm;

public class DaYuYangShenQuan : PowerUpOnCast
{
	private CValuePercent AddPowerPercent => CValuePercent.op_Implicit(base.IsDirect ? 40 : 80);

	protected override EDataModifyType ModifyType => (EDataModifyType)1;

	public DaYuYangShenQuan()
	{
	}

	public DaYuYangShenQuan(CombatSkillKey skillKey)
		: base(skillKey, 14107)
	{
	}

	public override void OnEnable(DataContext context)
	{
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, tryGetCoverCharacter: true);
		SkillEffectCollection skillEffectCollection = (base.IsDirect ? base.CombatChar : combatCharacter).GetSkillEffectCollection();
		Dictionary<SkillEffectKey, short> effectDict = skillEffectCollection.EffectDict;
		if (effectDict != null && effectDict.Count > 0)
		{
			int num = 0;
			int num2 = 0;
			foreach (KeyValuePair<SkillEffectKey, short> item in effectDict)
			{
				int num3 = item.Value * 100 / skillEffectCollection.MaxEffectCountDict[item.Key];
				num += (base.IsDirect ? num3 : (100 - num3)) * AddPowerPercent;
				num2++;
			}
			if (num2 == 0)
			{
				PowerUpValue = 0;
			}
			else
			{
				PowerUpValue = num / num2;
			}
		}
		base.OnEnable(context);
	}
}
