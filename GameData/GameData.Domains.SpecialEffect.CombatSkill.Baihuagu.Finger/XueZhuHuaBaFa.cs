using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Finger;

public class XueZhuHuaBaFa : CombatSkillEffectBase
{
	private const int PerFatalMarkCostEffectCount = 4;

	private const int AbsorbsHealthUnit = 12;

	private sbyte _effectCount;

	private readonly List<int> _affectingCharIds = new List<int>();

	private CValuePercent AbsorbsHealthPercent => CValuePercent.op_Implicit(base.IsDirect ? 20 : 40);

	public XueZhuHuaBaFa()
	{
	}

	public XueZhuHuaBaFa(CombatSkillKey skillKey, sbyte direction)
		: base(skillKey, 3108, direction)
	{
	}

	public override void OnEnable(DataContext context)
	{
		CreateAffectedData(53, (EDataModifyType)0, -1);
		CreateAffectedData(210, (EDataModifyType)3, base.SkillTemplateId);
		CreateAffectedData(222, (EDataModifyType)3, base.SkillTemplateId);
		Events.RegisterHandler_CombatBegin(OnCombatBegin);
		Events.RegisterHandler_CombatSettlement(OnCombatSettlement);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CombatBegin(OnCombatBegin);
		Events.UnRegisterHandler_CombatSettlement(OnCombatSettlement);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnCombatBegin(DataContext context)
	{
		if (!DomainManager.Combat.IsCharInCombat(base.CharacterId) || !base.CombatChar.GetAttackSkillList().Exist(base.SkillTemplateId))
		{
			return;
		}
		if (_effectCount > 0)
		{
			UpdateEffectCount(context);
		}
		if (base.IsDirect)
		{
			_affectingCharIds.Add(base.CharacterId);
		}
		else
		{
			_affectingCharIds.AddRange(from x in DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly)
				where x >= 0
				select x);
		}
		foreach (int affectingCharId in _affectingCharIds)
		{
			AppendAffectedData(context, affectingCharId, 304, (EDataModifyType)3, -1);
		}
	}

	private void OnCombatSettlement(DataContext context, sbyte combatStatus)
	{
		if (!DomainManager.Combat.IsCharInCombat(base.CharacterId))
		{
			return;
		}
		foreach (int affectingCharId in _affectingCharIds)
		{
			RemoveAffectedData(context, affectingCharId, 304);
		}
		_affectingCharIds.Clear();
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		if (SkillKey.IsMatch(charId, skillId) && PowerMatchAffectRequire(power) && _effectCount < base.MaxEffectCount && !DomainManager.Combat.CheckHealthImmunity(context, base.EnemyChar))
		{
			short health = base.EnemyChar.GetCharacter().GetHealth();
			int num = Math.Min((int)health * AbsorbsHealthPercent / 12, base.MaxEffectCount - _effectCount);
			int num2 = num * 12;
			if (num2 > 0)
			{
				base.EnemyChar.GetCharacter().ChangeHealth(context, -num2);
				ChangeEffectCount(context, num);
				ShowSpecialEffectTips(0);
			}
		}
	}

	private void ChangeEffectCount(DataContext context, int deltaValue)
	{
		if (deltaValue != 0)
		{
			_effectCount = (sbyte)Math.Clamp(_effectCount + deltaValue, 0, 127);
			UpdateEffectCount(context);
			DomainManager.SpecialEffect.SaveEffect(context, Id);
			InvalidateCache(context, 53);
		}
	}

	private void UpdateEffectCount(DataContext context)
	{
		DomainManager.Combat.AddSkillEffect(context, base.CombatChar, base.EffectKey, _effectCount, base.MaxEffectCount, autoRemoveOnNoCount: true);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId)
		{
			return 0;
		}
		if (dataKey.FieldId == 53)
		{
			return 12 * _effectCount;
		}
		return 0;
	}

	public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
	{
		if (dataKey.FieldId != 304)
		{
			return dataValue;
		}
		EDamageType customParam = (EDamageType)dataKey.CustomParam0;
		if (customParam != EDamageType.Direct)
		{
			return dataValue;
		}
		int num = Math.Min(_effectCount / 4, dataValue);
		if (num <= 0)
		{
			return dataValue;
		}
		dataValue = ((!base.IsDirect) ? (dataValue + num) : (dataValue - num));
		ChangeEffectCount(base.CombatChar.GetDataContext(), -num * 4);
		return dataValue;
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId)
		{
			return dataValue;
		}
		ushort fieldId = dataKey.FieldId;
		if ((fieldId == 210 || fieldId == 222) ? true : false)
		{
			return false;
		}
		return dataValue;
	}

	protected override int GetSubClassSerializedSize()
	{
		return base.GetSubClassSerializedSize() + 1;
	}

	protected unsafe override int SerializeSubClass(byte* pData)
	{
		byte* ptr = pData + base.SerializeSubClass(pData);
		*ptr = (byte)_effectCount;
		return GetSubClassSerializedSize();
	}

	protected unsafe override int DeserializeSubClass(byte* pData)
	{
		byte* ptr = pData + base.DeserializeSubClass(pData);
		_effectCount = (sbyte)(*ptr);
		return GetSubClassSerializedSize();
	}
}
