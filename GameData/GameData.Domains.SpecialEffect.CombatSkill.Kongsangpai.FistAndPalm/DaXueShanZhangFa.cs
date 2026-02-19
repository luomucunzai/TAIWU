using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.FistAndPalm;

public class DaXueShanZhangFa : CombatSkillEffectBase
{
	private const int AddPowerUnit = 10;

	private short _affectingSkillId;

	private int _addPower;

	public DaXueShanZhangFa()
	{
	}

	public DaXueShanZhangFa(CombatSkillKey skillKey)
		: base(skillKey, 10101, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Events.RegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.RegisterHandler_SkillEffectChange(OnSkillEffectChange);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.UnRegisterHandler_SkillEffectChange(OnSkillEffectChange);
	}

	private unsafe void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
	{
		if (!IsSrcSkillPerformed || charId != base.CharacterId || Config.CombatSkill.Instance[skillId].EquipType != 1)
		{
			return;
		}
		MainAttributes currMainAttributes = CharObj.GetCurrMainAttributes();
		MainAttributes currMainAttributes2 = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, tryGetCoverCharacter: true).GetCharacter().GetCurrMainAttributes();
		_affectingSkillId = skillId;
		_addPower = 0;
		for (sbyte b = 0; b < 6; b++)
		{
			if (base.IsDirect ? (currMainAttributes.Items[b] > currMainAttributes2.Items[b]) : (currMainAttributes.Items[b] < currMainAttributes2.Items[b]))
			{
				_addPower += 10;
			}
		}
		if (_addPower > 0)
		{
			AppendAffectedData(context, charId, 199, (EDataModifyType)1, skillId);
			ShowSpecialEffectTips(0);
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (!IsSrcSkillPerformed)
		{
			if (charId == base.CharacterId && skillId == base.SkillTemplateId)
			{
				IsSrcSkillPerformed = true;
				if (PowerMatchAffectRequire(power))
				{
					AddMaxEffectCount();
				}
				else
				{
					RemoveSelf(context);
				}
			}
		}
		else if (charId == base.CharacterId)
		{
			if (skillId == base.SkillTemplateId && PowerMatchAffectRequire(power))
			{
				RemoveSelf(context);
			}
			else if (Config.CombatSkill.Instance[skillId].EquipType == 1)
			{
				ClearAffectedData(context);
				ReduceEffectCount();
			}
		}
	}

	private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
	{
		if (removed && IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect)
		{
			RemoveSelf(context);
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != _affectingSkillId)
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
