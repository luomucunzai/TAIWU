using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.ZiWuXiao;

public class LingSanTianDiRen : CombatSkillEffectBase
{
	private const sbyte AddPowerUnit = 60;

	private int _addPower;

	public LingSanTianDiRen()
	{
	}

	public LingSanTianDiRen(CombatSkillKey skillKey)
		: base(skillKey, 17113, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		SkillEffectKey key = new SkillEffectKey(874, isDirect: true);
		Dictionary<SkillEffectKey, short> effectDict = base.CombatChar.GetSkillEffectCollection().EffectDict;
		_addPower = ((effectDict != null && effectDict.TryGetValue(key, out var value)) ? (60 * value) : 0);
		if (_addPower > 0)
		{
			CreateAffectedData(199, (EDataModifyType)1, base.SkillTemplateId);
			DomainManager.Combat.ChangeSkillEffectCount(context, base.CombatChar, key, (short)(-effectDict[key]));
			ShowSpecialEffectTips(0);
		}
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private unsafe void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		if (PowerMatchAffectRequire(power))
		{
			SkillEffectKey key = new SkillEffectKey(875, isDirect: true);
			Dictionary<SkillEffectKey, short> effectDict = base.CombatChar.GetSkillEffectCollection().EffectDict;
			if (effectDict != null && effectDict.ContainsKey(key))
			{
				DomainManager.Combat.CastSkillFree(context, base.CombatChar, 878);
				ShowSpecialEffectTips(1);
			}
			else
			{
				CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
				NeiliAllocation neiliAllocation = combatCharacter.GetNeiliAllocation();
				for (byte b = 0; b < 4; b++)
				{
					combatCharacter.ChangeNeiliAllocation(context, b, -neiliAllocation.Items[(int)b] / 2);
				}
				ShowSpecialEffectTips(2);
			}
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
