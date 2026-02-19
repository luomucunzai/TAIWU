using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect;

public class GoldenSilkwormBase : WugEffectBase
{
	private static readonly Dictionary<sbyte, short> WugType2SkillId = new Dictionary<sbyte, short>
	{
		[0] = 445,
		[1] = 446,
		[2] = 447,
		[3] = 448,
		[4] = 449,
		[5] = 450,
		[6] = 451,
		[7] = 452
	};

	private int ConsummateLevelBonusAddPercent => base.IsElite ? (base.IsGood ? 50 : (-50)) : 0;

	private static int CalcPoisonRatio(bool isGrown)
	{
		return isGrown ? 10 : 5;
	}

	protected GoldenSilkwormBase()
	{
	}

	protected GoldenSilkwormBase(int charId, int type, short wugTemplateId, short effectId)
		: base(charId, type, wugTemplateId, effectId)
	{
		CostWugCount = 16;
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		if (base.IsGrown)
		{
			CreateAffectedData(267, (EDataModifyType)3, -1);
		}
		else
		{
			CreateAffectedData(297, (EDataModifyType)2, -1);
		}
		Events.RegisterHandler_AdvanceMonthBegin(OnAdvanceMonthBegin);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_AdvanceMonthBegin(OnAdvanceMonthBegin);
		base.OnDisable(context);
	}

	private void OnAdvanceMonthBegin(DataContext context)
	{
		if (!base.CanAffect)
		{
			return;
		}
		EatingItems eatingItems = CharObj.GetEatingItems();
		if (base.IsGrown && base.IsElite)
		{
			List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
			for (sbyte b = 0; b < 8; b++)
			{
				int num = eatingItems.IndexOfWug(b);
				if (b != WugConfig.WugType && num >= 0)
				{
					MedicineItem medicineItem = Config.Medicine.Instance[eatingItems.Get(num).TemplateId];
					if (medicineItem.WugGrowthType == 4)
					{
						list.Add(b);
					}
				}
			}
			if (list.Count > 0)
			{
				EatWug(context, list.GetRandom(context.Random), eatGrown: true);
			}
		}
		else if (base.CanChangeToGrown && !eatingItems.ContainsAny())
		{
			ChangeToGrown(context);
		}
	}

	public override void OnEffectAdded(DataContext context, short replacedWug)
	{
		if (base.CanAffect)
		{
			bool flag = false;
			for (sbyte b = 0; b < 8; b++)
			{
				flag = EatWug(context, b) || flag;
			}
			if (flag)
			{
				OnAffected(context);
			}
		}
	}

	protected override void AddAffectDataAndEvent(DataContext context)
	{
		Events.RegisterHandler_AddWug(OnAddWug);
	}

	protected override void ClearAffectDataAndEvent(DataContext context)
	{
		Events.UnRegisterHandler_AddWug(OnAddWug);
	}

	private void OnAddWug(DataContext context, int charId, short wugTemplateId, short replacedWug)
	{
		if (charId == base.CharacterId && base.CanAffect)
		{
			MedicineItem medicineItem = Config.Medicine.Instance[wugTemplateId];
			if (EatWug(context, medicineItem.WugType))
			{
				OnAffected(context);
			}
		}
	}

	private bool EatWug(DataContext context, sbyte wugType, bool eatGrown = false)
	{
		if (wugType == WugConfig.WugType)
		{
			return false;
		}
		EatingItems eatingItems = CharObj.GetEatingItems();
		int num = eatingItems.IndexOfWug(wugType);
		if (num < 0)
		{
			return false;
		}
		short templateId = eatingItems.Get(num).TemplateId;
		MedicineItem medicineItem = Config.Medicine.Instance[templateId];
		if (!eatGrown && !WugGrowthType.IsWugGrowthTypeCombatOnly(medicineItem.WugGrowthType))
		{
			return false;
		}
		CharObj.RemoveWug(context, templateId);
		bool flag = medicineItem.WugGrowthType == 4;
		int poisonRatio = CalcPoisonRatio(flag);
		EatOtherWugEffect(context, medicineItem.WugType, poisonRatio);
		if (flag)
		{
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			AddLifeRecord((LifeRecordAddTemplate<sbyte, short>)lifeRecordCollection.AddWugKingGoldenSilkwormEatGrownWug, (sbyte)8, templateId);
		}
		return true;
	}

	private unsafe void EatOtherWugEffect(DataContext context, sbyte foodWugType, int poisonRatio)
	{
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[WugType2SkillId[foodWugType]];
		PoisonsAndLevels poisons = combatSkillItem.Poisons;
		for (sbyte b = 0; b < 6; b++)
		{
			int num = poisons.Values[b] * poisonRatio;
			sbyte b2 = poisons.Levels[b];
			if (b2 > 0)
			{
				if (DomainManager.Combat.IsInCombat())
				{
					if (base.IsGood)
					{
						DomainManager.Combat.ReducePoison(context, base.CombatChar, b, num);
					}
					else
					{
						DomainManager.Combat.AddPoison(context, base.CombatChar, base.CombatChar, b, b2, num, -1);
					}
				}
				else if (base.IsGood)
				{
					CharObj.ChangePoisoned(context, b, 3, -num);
				}
				else
				{
					CharObj.ChangePoisoned(context, b, 3, num);
				}
			}
		}
	}

	private void OnAffected(DataContext context)
	{
		ShowEffectTips(context, 1);
		ShowEffectTips(context, 2);
		CostWugInCombat(context);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.FieldId != 297 || !base.CanAffect || !base.IsElite)
		{
			return 0;
		}
		return ConsummateLevelBonusAddPercent;
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.FieldId != 267 || !base.CanAffect)
		{
			return dataValue;
		}
		return true;
	}
}
