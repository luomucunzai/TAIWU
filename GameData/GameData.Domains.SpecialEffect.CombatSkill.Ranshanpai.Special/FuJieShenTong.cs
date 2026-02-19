using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Special;

public class FuJieShenTong : CombatSkillEffectBase
{
	private const int ReduceNeiliAllocationBasePercent = 20;

	private const int ReduceNeiliAllocationUnitPercent = 5;

	private DataUid _neiliAllocationUid;

	private bool _affecting;

	public FuJieShenTong()
	{
	}

	public FuJieShenTong(CombatSkillKey skillKey)
		: base(skillKey, 7308, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		CreateAffectedData(217, (EDataModifyType)3, base.SkillTemplateId);
		CreateAffectedData(290, (EDataModifyType)3, base.SkillTemplateId);
		_neiliAllocationUid = ParseNeiliAllocationDataUid();
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_neiliAllocationUid, base.DataHandlerKey, OnNeiliAllocationChanged);
		Events.RegisterHandler_CombatBegin(OnCombatBegin);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_neiliAllocationUid, base.DataHandlerKey);
		Events.UnRegisterHandler_CombatBegin(OnCombatBegin);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnNeiliAllocationChanged(DataContext context, DataUid arg2)
	{
		UpdateAffecting(context);
	}

	private void OnCombatBegin(DataContext context)
	{
		UpdateAffecting(context);
		ShowSpecialEffectTips(0);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		if (charId != base.CharacterId || skillId != base.SkillTemplateId || !PowerMatchAffectRequire(power))
		{
			return;
		}
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
		CombatCharacter combatCharacter2 = (base.IsDirect ? combatCharacter : base.CombatChar);
		Dictionary<byte, int> dictionary = ObjectPool<Dictionary<byte, int>>.Instance.Get();
		foreach (short bannedSkillId in combatCharacter2.GetBannedSkillIds(requireNotInfinity: true))
		{
			if (base.IsDirect)
			{
				DomainManager.Combat.ResetSkillCd(context, combatCharacter2, bannedSkillId);
			}
			else
			{
				DomainManager.Combat.ClearSkillCd(context, combatCharacter2, bannedSkillId);
			}
			DomainManager.Combat.AddGoneMadInjury(context, combatCharacter, bannedSkillId);
			ShowSpecialEffectTipsOnceInFrame(1);
			ShowSpecialEffectTipsOnceInFrame(2);
			byte relatedNeiliAllocationType = Config.CombatSkill.Instance[bannedSkillId].GetRelatedNeiliAllocationType();
			dictionary[relatedNeiliAllocationType] = dictionary.GetOrDefault(relatedNeiliAllocationType) + 1;
		}
		foreach (KeyValuePair<byte, int> item in dictionary)
		{
			item.Deconstruct(out var key, out var value);
			byte b = key;
			int num = value;
			CValuePercent val = CValuePercent.op_Implicit(20 + 5 * num);
			int num2 = (int)base.CombatChar.GetNeiliAllocation()[b] * val;
			base.CombatChar.ChangeNeiliAllocation(context, b, -num2);
		}
	}

	private void UpdateAffecting(DataContext context)
	{
		bool flag = !base.CombatChar.AnyLowerThanOriginNeiliAllocation();
		if (flag != _affecting)
		{
			_affecting = flag;
			DomainManager.Combat.UpdateSkillCanUse(context, base.CombatChar, base.SkillTemplateId);
		}
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		if (dataKey.SkillKey != SkillKey)
		{
			return dataValue;
		}
		ushort fieldId = dataKey.FieldId;
		if (1 == 0)
		{
		}
		bool result = fieldId switch
		{
			217 => false, 
			290 => _affecting, 
			_ => dataValue, 
		};
		if (1 == 0)
		{
		}
		return result;
	}
}
