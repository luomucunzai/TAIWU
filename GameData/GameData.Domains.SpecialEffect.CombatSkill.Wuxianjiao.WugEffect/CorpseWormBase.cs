using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect;

public class CorpseWormBase : WugEffectBase
{
	private const int ChangeDamagePercent = 40;

	private const int GrowRequireHealthPercent = 50;

	private const sbyte GrownInfectCount = 5;

	private const int HealthDeltaValue = -60;

	private bool _affected;

	private bool _affectedOnMonthChange;

	private int HealthPercent => CValuePercent.ParseInt((int)CharObj.GetHealth(), (int)CharObj.GetLeftMaxHealth());

	private CValuePercentBonus FatalDamageBonus => CValuePercentBonus.op_Implicit((HealthPercent >= 50) ? (base.IsGood ? (-25) : 25) : 0);

	protected CorpseWormBase()
	{
	}

	protected CorpseWormBase(int charId, int type, short wugTemplateId, short effectId)
		: base(charId, type, wugTemplateId, effectId)
	{
		CostWugCount = 12;
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		if (AffectDatas == null)
		{
			AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		}
		if (base.IsGrown)
		{
			CreateAffectedData(263, (EDataModifyType)0, -1);
			CreateAffectedData(295, (EDataModifyType)3, -1);
			Events.RegisterHandler_AdvanceMonthFinish(OnAdvanceMonthFinish);
		}
		else
		{
			CreateAffectedData(294, (EDataModifyType)3, -1);
		}
		if (base.CanChangeToGrown)
		{
			Events.RegisterHandler_PostAdvanceMonthBegin(OnAdvanceMonthBegin);
		}
	}

	public override void OnDisable(DataContext context)
	{
		base.OnDisable(context);
		if (base.IsGrown)
		{
			Events.UnRegisterHandler_AdvanceMonthFinish(OnAdvanceMonthFinish);
		}
		if (base.CanChangeToGrown)
		{
			Events.UnRegisterHandler_PostAdvanceMonthBegin(OnAdvanceMonthBegin);
		}
	}

	protected override void AddAffectDataAndEvent(DataContext context)
	{
		_affected = false;
		AppendAffectedData(context, base.CharacterId, 102, (EDataModifyType)2, -1);
		Events.RegisterHandler_AttackSkillAttackEnd(OnAttackSkillAttackEnd);
	}

	protected override void ClearAffectDataAndEvent(DataContext context)
	{
		RemoveAffectedData(context, base.CharacterId, 102);
		Events.UnRegisterHandler_AttackSkillAttackEnd(OnAttackSkillAttackEnd);
	}

	private void OnAdvanceMonthBegin(DataContext context)
	{
		if (base.CanAffect)
		{
			short health = CharObj.GetHealth();
			short leftMaxHealth = CharObj.GetLeftMaxHealth();
			if (health <= leftMaxHealth * 50 / 100)
			{
				ChangeToGrown(context);
			}
		}
	}

	private void OnAdvanceMonthFinish(DataContext context)
	{
		if (_affectedOnMonthChange)
		{
			_affectedOnMonthChange = false;
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			AddLifeRecord(lifeRecordCollection.AddWugCorpseWormChangeHealth);
		}
	}

	private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
	{
		if (context.DefenderId == base.CharacterId && index >= 3 && _affected)
		{
			_affected = false;
			ShowEffectTips(context, 1);
			CostWugInCombat(context);
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || !base.CanAffect)
		{
			return 0;
		}
		if (dataKey.FieldId == 263)
		{
			_affectedOnMonthChange = true;
			return -60;
		}
		if (dataKey.FieldId == 102 && !dataKey.IsNormalAttack)
		{
			_affected = true;
			return base.IsGood ? (-40) : 40;
		}
		return 0;
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.FieldId != 295 || !base.IsElite || !base.CanAffect)
		{
			return dataValue;
		}
		return false;
	}

	public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		if (dataKey.CharId != base.CharacterId || dataKey.FieldId != 294 || !base.CanAffect || !base.IsElite)
		{
			return dataValue;
		}
		if (DomainManager.Combat.CheckHealthImmunity(base.CombatChar.GetDataContext(), base.CombatChar))
		{
			return dataValue;
		}
		int num = dataValue * FatalDamageBonus;
		int fatalDamageStep = base.CombatChar.GetDamageStepCollection().FatalDamageStep;
		int num2 = Math.Abs(dataValue - num);
		int num3 = GlobalConfig.Instance.ReduceHealthPerFatalDamageMark[2] * num2 / Math.Max(fatalDamageStep, 1);
		if (num2 > 0)
		{
			ShowEffectTips(DomainManager.Combat.Context, 2);
		}
		if (num3 > 0)
		{
			CharObj.ChangeHealth(DomainManager.Combat.Context, -num3);
			ShowEffectTips(base.CombatChar.GetDataContext(), 3);
		}
		return num;
	}

	protected override void ChangeToGrown(DataContext context)
	{
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		Location key = CharObj.GetLocation();
		if (!key.IsValid())
		{
			key = CharObj.GetValidLocation();
		}
		HashSet<int> characterSet = DomainManager.Map.GetBlock(key).CharacterSet;
		list.Clear();
		if (characterSet != null)
		{
			list.AddRange(characterSet);
		}
		list.Remove(base.CharacterId);
		if (DomainManager.Taiwu.GetTaiwuCharId() == base.CharacterId)
		{
			list.AddRange(DomainManager.Taiwu.GetGroupCharIds().GetCollection());
			list.Remove(base.CharacterId);
		}
		int num = Math.Min(5, list.Count);
		CharObj.SetHealth(0, context);
		CharObj.RemoveWug(context, WugConfig.TemplateId);
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		for (int i = 0; i < num; i++)
		{
			int index = context.Random.Next(0, list.Count);
			short wugTemplateId = ItemDomain.GetWugTemplateId(WugConfig.WugType, 4);
			DomainManager.Character.GetElement_Objects(list[index]).AddWug(context, wugTemplateId);
			AddLifeRecord(lifeRecordCollection.AddWugCorpseWormChangeToGrown, list[index], wugTemplateId);
			list.RemoveAt(index);
		}
		ObjectPool<List<int>>.Instance.Return(list);
	}
}
