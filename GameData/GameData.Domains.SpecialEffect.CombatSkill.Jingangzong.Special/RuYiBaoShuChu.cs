using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Special;

public class RuYiBaoShuChu : CombatSkillEffectBase
{
	private const sbyte AddPowerUnit = 10;

	private static readonly CValuePercent StatePowerChangePercent = CValuePercent.op_Implicit(75);

	private int _addPower;

	public RuYiBaoShuChu()
	{
	}

	public RuYiBaoShuChu(CombatSkillKey skillKey)
		: base(skillKey, 11306, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		SkillEffectKey pestleEffect = DomainManager.Combat.GetUsingWeaponData(base.CombatChar).GetPestleEffect();
		_addPower = ((pestleEffect.SkillId >= 0) ? (10 * (Config.CombatSkill.Instance[pestleEffect.SkillId].Grade + 1)) : 0);
		if (_addPower > 0)
		{
			ShowSpecialEffectTips(0);
		}
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId), (EDataModifyType)1);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		if (charId != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		if (!interrupted)
		{
			if (PowerMatchAffectRequire(power))
			{
				sbyte b = (sbyte)(base.IsDirect ? 1 : 2);
				CombatStateCollection combatStateCollection = base.CombatChar.GetCombatStateCollection(b);
				if (combatStateCollection.StateDict.Count > 0)
				{
					List<short> list = ObjectPool<List<short>>.Instance.Get();
					list.Clear();
					list.AddRange(combatStateCollection.StateDict.Keys);
					for (int i = 0; i < list.Count; i++)
					{
						short num = list[i];
						int num2 = (int)combatStateCollection.StateDict[num].power * StatePowerChangePercent;
						if (num2 > 0)
						{
							DomainManager.Combat.AddCombatState(context, base.CombatChar, b, num, base.IsDirect ? num2 : (-num2), reverse: false, applyEffect: false);
						}
					}
					ObjectPool<List<short>>.Instance.Return(list);
					ShowSpecialEffectTips(1);
				}
			}
			DomainManager.Combat.GetUsingWeaponData(base.CombatChar).RemovePestleEffect(context);
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
