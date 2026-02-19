using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Sword;

public class ShenMuBiXieJian : CombatSkillEffectBase
{
	private const sbyte AffectSkillCount = 2;

	private const sbyte TransferPower = 10;

	private readonly List<short> _transferPowerSkillList = new List<short>();

	private DataUid _addPowerUid;

	private bool _affectInCast;

	public ShenMuBiXieJian()
	{
	}

	public ShenMuBiXieJian(CombatSkillKey skillKey)
		: base(skillKey, 12303, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		if (base.IsDirect)
		{
			_addPowerUid = new DataUid(8, 6, ulong.MaxValue);
			GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_addPowerUid, base.DataHandlerKey, OnAddPowerChanged);
		}
		Events.RegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
		Events.RegisterHandler_AttackSkillAttackBegin(OnAttackSkillAttackBegin);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		if (base.IsDirect)
		{
			GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_addPowerUid, base.DataHandlerKey);
		}
		Events.UnRegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
		Events.UnRegisterHandler_AttackSkillAttackBegin(OnAttackSkillAttackBegin);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
	{
		if (attacker.GetId() == base.CharacterId && skillId == base.SkillTemplateId)
		{
			_affectInCast = false;
		}
	}

	private void OnAttackSkillAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId, int index, bool hit)
	{
		if (attacker.GetId() == base.CharacterId && skillId == base.SkillTemplateId && hit)
		{
			_affectInCast = true;
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (!(charId != base.CharacterId || skillId != base.SkillTemplateId || interrupted) && _affectInCast)
		{
			if (base.IsDirect == !PowerMatchAffectRequire(power))
			{
				DoTransferPower(context);
			}
			else if (_transferPowerSkillList.Count > 0)
			{
				DoReturnPower(context);
			}
		}
	}

	private void DoTransferPower(DataContext context)
	{
		SkillEffectKey effectKey = new SkillEffectKey(base.SkillTemplateId, base.IsDirect);
		int needMinPower = 20;
		if (!base.IsDirect && base.SkillInstance.GetPower() < needMinPower)
		{
			return;
		}
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		IEnumerable<short> enumerable = from x in base.CombatChar.GetAttackSkillList()
			where x >= 0 && x != base.SkillTemplateId
			select x;
		if (base.IsDirect)
		{
			enumerable = from x in enumerable
				select DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(base.CharacterId, x)) into x
				where x.GetPower() >= needMinPower
				select x.GetId().SkillTemplateId;
		}
		list.AddRange(enumerable);
		if (list.Count > 0)
		{
			int num = Math.Min(2, list.Count);
			if (!base.IsDirect)
			{
				num = Math.Min(num, (base.SkillInstance.GetPower() - 10) / 10);
			}
			if (num > 0)
			{
				CollectionUtils.Shuffle(context.Random, list);
				for (int num2 = 0; num2 < num; num2++)
				{
					short num3 = list[num2];
					CombatSkillKey combatSkillKey = new CombatSkillKey(base.CharacterId, num3);
					_transferPowerSkillList.Add(num3);
					DomainManager.Combat.ReduceSkillPowerInCombat(context, base.IsDirect ? combatSkillKey : SkillKey, effectKey, -10);
					DomainManager.Combat.AddSkillPowerInCombat(context, base.IsDirect ? SkillKey : combatSkillKey, effectKey, 10);
				}
				ShowSpecialEffectTips(0);
			}
		}
		ObjectPool<List<short>>.Instance.Return(list);
	}

	private void DoReturnPower(DataContext context)
	{
		SkillEffectKey skillEffectKey = new SkillEffectKey(base.SkillTemplateId, base.IsDirect);
		int num = 0;
		for (int i = 0; i < _transferPowerSkillList.Count; i++)
		{
			CombatSkillKey skillKey = new CombatSkillKey(base.CharacterId, _transferPowerSkillList[i]);
			if (base.IsDirect)
			{
				DomainManager.Combat.RemoveSkillPowerReduceInCombat(context, skillKey, skillEffectKey);
				DomainManager.Combat.AddSkillPowerInCombat(context, skillKey, skillEffectKey, 20);
			}
			else
			{
				num += DomainManager.Combat.RemoveSkillPowerAddInCombat(context, skillKey, skillEffectKey);
			}
		}
		_transferPowerSkillList.Clear();
		if (base.IsDirect)
		{
			DomainManager.Combat.RemoveSkillPowerAddInCombat(context, SkillKey, skillEffectKey);
		}
		else if (num > 0)
		{
			DomainManager.Combat.AddSkillPowerInCombat(context, SkillKey, skillEffectKey, num * 2);
		}
		if (base.IsDirect || num > 0)
		{
			ShowSpecialEffectTips(0);
		}
	}

	private void OnAddPowerChanged(DataContext context, DataUid dataUid)
	{
		if (!DomainManager.Combat.GetAllSkillPowerAddInCombat().ContainsKey(SkillKey))
		{
			_transferPowerSkillList.Clear();
		}
	}
}
