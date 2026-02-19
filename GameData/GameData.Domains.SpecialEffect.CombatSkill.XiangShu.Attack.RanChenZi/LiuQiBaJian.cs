using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.RanChenZi;

public class LiuQiBaJian : CombatSkillEffectBase
{
	private const sbyte AddPowerUnit = 10;

	private const sbyte TransferCount = 6;

	private int _addPower;

	public LiuQiBaJian()
	{
	}

	public LiuQiBaJian(CombatSkillKey skillKey)
		: base(skillKey, 17134, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		_addPower = 10 * base.CombatChar.GetDefeatMarkCollection().MindMarkList.Count;
		if (_addPower > 0)
		{
			CreateAffectedData(199, (EDataModifyType)1, base.SkillTemplateId);
			ShowSpecialEffectTips(0);
		}
		sbyte[] array = new sbyte[3]
		{
			DomainManager.World.GetElement_XiangshuAvatarTaskStatuses(7).JuniorXiangshuTaskStatus,
			DomainManager.World.GetElement_XiangshuAvatarTaskStatuses(5).JuniorXiangshuTaskStatus,
			DomainManager.World.GetElement_XiangshuAvatarTaskStatuses(4).JuniorXiangshuTaskStatus
		};
		bool flag = !array.Exist((sbyte status) => status != 6);
		bool flag2 = !array.Exist((sbyte status) => status != 5);
		if (flag || flag2)
		{
			if (flag)
			{
				DomainManager.Combat.RemoveMindDefeatMark(context, base.CurrEnemyChar, 2, random: true);
			}
			else
			{
				DomainManager.Combat.AppendMindDefeatMark(context, base.CurrEnemyChar, 2, -1);
			}
			ShowSpecialEffectTips(flag, 2, 3);
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
			List<bool> mindMarkList = base.CombatChar.GetDefeatMarkCollection().MindMarkList;
			if (mindMarkList.Count > 0)
			{
				CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
				int num = Math.Min(6, mindMarkList.Count);
				for (int i = 0; i < num; i++)
				{
					DomainManager.Combat.TransferRandomMindDefeatMark(context, base.CombatChar, combatCharacter);
				}
				DomainManager.Combat.AddToCheckFallenSet(combatCharacter.GetId());
				ShowSpecialEffectTips(1);
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
