using System;
using System.Collections.Generic;
using Config;
using Config.Common;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai.PrioritizedAction;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Taiwu.Display;
using GameData.Domains.Taiwu.Display.VillagerRoleArrangement;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.VillagerRole;

[SerializableGameData(IsExtensible = true, NotForDisplayModule = true, NoCopyConstructors = true)]
public class VillagerRoleDoctor : VillagerRoleBase, IVillagerRoleArrangementExecutor, IVillagerRoleSelectLocation
{
	public override short RoleTemplateId => 2;

	public sbyte InteractTargetGrade
	{
		get
		{
			int val = VillagerRoleFormulaImpl.Calculate(4, base.Personality);
			int val2 = VillagerRoleFormulaImpl.Calculate(5, DomainManager.World.GetMaxGradeOfXiangshuInfection());
			return (sbyte)Math.Min(val, val2);
		}
	}

	public int HealXiangshuInfectionAmount => VillagerRoleFormulaImpl.Calculate(9, MaxHealingAttainment, base.Personality);

	public int MaxHealingAttainment => Math.Max(Character.GetLifeSkillAttainment(8), Character.GetLifeSkillAttainment(9));

	public int AutoActionAuthorityIncome(GameData.Domains.Character.Character targetChar)
	{
		VillagerRoleFormulaItem formula = VillagerRoleFormula.Instance[6];
		VillagerRoleFormulaItem formula2 = VillagerRoleFormula.Instance[7];
		int adjustedResourceSatisfyingAmount = targetChar.GetAdjustedResourceSatisfyingAmount(7);
		int maxHealingAttainment = MaxHealingAttainment;
		int arg = formula.Calculate(adjustedResourceSatisfyingAmount, maxHealingAttainment);
		return formula2.Calculate(arg);
	}

	public int CalcPrioritizeActionSpiritualDebtIncome(sbyte targetGrade)
	{
		return VillagerRoleFormulaImpl.Calculate(8, targetGrade, MaxHealingAttainment);
	}

	void IVillagerRoleArrangementExecutor.ExecuteArrangementAction(DataContext context, VillagerRoleArrangementAction action)
	{
		Location location = Character.GetLocation();
		MapBlockData block = DomainManager.Map.GetBlock(location);
		HashSet<int> characterSet = block.CharacterSet;
		if (characterSet == null || characterSet.Count <= 1)
		{
			return;
		}
		sbyte targetGrade = InteractTargetGrade;
		bool hasChickenUpgrade = base.HasChickenUpgradeEffect;
		GameData.Domains.Character.Character character = Character.SelectRandomActionTarget(context, block.CharacterSet, Condition, includeBabies: true);
		if (character == null)
		{
			return;
		}
		Span<sbyte> span = stackalloc sbyte[5];
		SpanList<sbyte> actionTypes = span;
		GetValidActionTypes(character, ref actionTypes);
		if (hasChickenUpgrade && character.GetXiangshuInfection() >= 100)
		{
			actionTypes.Add(4);
		}
		if (actionTypes.Count > 0)
		{
			int id = Character.GetId();
			int currDate = DomainManager.World.GetCurrDate();
			int num = CalcPrioritizeActionSpiritualDebtIncome(character.GetInteractionGrade());
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			lifeRecordCollection.AddVillagerTreatment1(id, currDate, character.GetId(), location);
			lifeRecordCollection.AddVillagerTreatmentTaiwu(DomainManager.Taiwu.GetTaiwuCharId(), currDate, id, location, num);
			sbyte random = actionTypes.GetRandom(context.Random);
			if (random == 4)
			{
				character.ChangeXiangshuInfection(context, -HealXiangshuInfectionAmount);
				lifeRecordCollection.AddVillagerReduceXiangshuInfect(character.GetId(), currDate);
			}
			else
			{
				Character.DoHealAction(context, (EHealActionType)random, character);
			}
			DomainManager.Extra.ChangeAreaSpiritualDebt(context, location.AreaId, num);
		}
		bool Condition(GameData.Domains.Character.Character character2)
		{
			if (character2.GetInteractionGrade() > targetGrade)
			{
				return false;
			}
			if (HealActionCharacterFilter(character2))
			{
				return true;
			}
			if (hasChickenUpgrade)
			{
				byte xiangshuInfection = character2.GetXiangshuInfection();
				if (xiangshuInfection >= 100)
				{
					return true;
				}
			}
			return false;
		}
	}

	bool IVillagerRoleSelectLocation.NextLocationFilter(MapBlockData block)
	{
		if (block.CharacterSet == null)
		{
			return false;
		}
		if (block.IsCityTown())
		{
			return true;
		}
		bool hasChickenUpgradeEffect = base.HasChickenUpgradeEffect;
		using (HashSet<int>.Enumerator enumerator = block.CharacterSet.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				int current = enumerator.Current;
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(current);
				if (HealActionCharacterFilter(element_Objects))
				{
					return true;
				}
				if (hasChickenUpgradeEffect)
				{
					byte xiangshuInfection = element_Objects.GetXiangshuInfection();
					if (xiangshuInfection >= 100)
					{
						return true;
					}
				}
				return false;
			}
		}
		return false;
	}

	public override void ExecuteFixedAction(DataContext context)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		if (ArrangementTemplateId < 0 && (WorkData == null || WorkData.WorkType != 1))
		{
			BoolArray64 autoActionStates = base.AutoActionStates;
			if (((BoolArray64)(ref autoActionStates))[4])
			{
				TryAddNextAutoTravelTarget(context, AutoActionBlockFilter);
				AutoCureForAuthorityAction(context);
			}
		}
	}

	private bool AutoCureForAuthorityAction(DataContext context)
	{
		Location location = Character.GetLocation();
		sbyte interactTargetGrade = InteractTargetGrade;
		MapBlockData block = DomainManager.Map.GetBlock(location);
		Span<sbyte> span = stackalloc sbyte[5];
		SpanList<sbyte> actionTypes = span;
		int id = Character.GetId();
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		if (block.CharacterSet == null)
		{
			return false;
		}
		foreach (int item in block.CharacterSet)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			if (element_Objects.GetInteractionGrade() <= interactTargetGrade)
			{
				GetValidActionTypes(element_Objects, ref actionTypes);
				if (actionTypes.Count > 0)
				{
					EHealActionType random = (EHealActionType)actionTypes.GetRandom(context.Random);
					Character.DoHealAction(context, random, element_Objects);
					int num = AutoActionAuthorityIncome(element_Objects);
					DomainManager.Taiwu.AddResource(context, ItemSourceType.Resources, 7, num);
					lifeRecordCollection.AddVillagerTreatment0(id, currDate, item, location, num, 7);
					return true;
				}
			}
		}
		return false;
	}

	private void GetValidActionTypes(GameData.Domains.Character.Character character, ref SpanList<sbyte> actionTypes)
	{
		IReadOnlyList<EHealActionType> allHealActions = GameData.Domains.Character.Character.AllHealActions;
		foreach (EHealActionType item in allHealActions)
		{
			if (CanHeal(character, item))
			{
				actionTypes.Add((sbyte)item);
			}
		}
	}

	private bool AutoActionBlockFilter(MapBlockData blockData)
	{
		if (blockData.CharacterSet == null)
		{
			return false;
		}
		sbyte interactTargetGrade = InteractTargetGrade;
		foreach (int item in blockData.CharacterSet)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			if (element_Objects.GetInteractionGrade() > interactTargetGrade || !HealActionCharacterFilter(element_Objects))
			{
				continue;
			}
			return true;
		}
		return false;
	}

	private bool HealActionCharacterFilter(GameData.Domains.Character.Character character)
	{
		IReadOnlyList<EHealActionType> allHealActions = GameData.Domains.Character.Character.AllHealActions;
		foreach (EHealActionType item in allHealActions)
		{
			if (CanHeal(character, item))
			{
				return true;
			}
		}
		return false;
	}

	private bool CanHeal(GameData.Domains.Character.Character character, EHealActionType actionType)
	{
		if (1 == 0)
		{
		}
		int maxRequireAttainment = actionType switch
		{
			EHealActionType.Healing => character.GetInjuries().GetSum(), 
			EHealActionType.Detox => character.GetPoisonMarkCount(), 
			EHealActionType.Breathing => character.GetDisorderOfQi(), 
			EHealActionType.Recover => 100 - character.GetHealth() * 100 / Math.Max(1, (int)character.GetLeftMaxHealth()), 
			_ => throw new ArgumentOutOfRangeException("actionType", actionType, null), 
		};
		if (1 == 0)
		{
		}
		int num = maxRequireAttainment;
		return num >= VillagerRoleFormulaImpl.Calculate(10, (int)actionType) && Character.CalcHealEffect(actionType, character, out maxRequireAttainment) > 0;
	}

	public override IVillagerRoleArrangementDisplayData GetArrangementDisplayData()
	{
		return new HealingDisplayData
		{
			InteractTargetGrade = InteractTargetGrade,
			HealXiangshuInfectionAmount = HealXiangshuInfectionAmount
		};
	}
}
