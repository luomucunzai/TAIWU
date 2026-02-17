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

namespace GameData.Domains.Taiwu.VillagerRole
{
	// Token: 0x0200004E RID: 78
	[SerializableGameData(IsExtensible = true, NotForDisplayModule = true, NoCopyConstructors = true)]
	public class VillagerRoleDoctor : VillagerRoleBase, IVillagerRoleArrangementExecutor, IVillagerRoleSelectLocation
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060013A1 RID: 5025 RVA: 0x0013978D File Offset: 0x0013798D
		public override short RoleTemplateId
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060013A2 RID: 5026 RVA: 0x00139790 File Offset: 0x00137990
		public sbyte InteractTargetGrade
		{
			get
			{
				int grade = VillagerRoleFormulaImpl.Calculate(4, base.Personality);
				int maxGrade = VillagerRoleFormulaImpl.Calculate(5, (int)DomainManager.World.GetMaxGradeOfXiangshuInfection());
				return (sbyte)Math.Min(grade, maxGrade);
			}
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x001397C8 File Offset: 0x001379C8
		public int AutoActionAuthorityIncome(GameData.Domains.Character.Character targetChar)
		{
			VillagerRoleFormulaItem baseFormula = VillagerRoleFormula.Instance[6];
			VillagerRoleFormulaItem adjustFormula = VillagerRoleFormula.Instance[7];
			int authorityThreshold = targetChar.GetAdjustedResourceSatisfyingAmount(7);
			int healingAttainment = this.MaxHealingAttainment;
			int baseValue = baseFormula.Calculate(authorityThreshold, healingAttainment);
			return adjustFormula.Calculate(baseValue);
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x00139815 File Offset: 0x00137A15
		public int CalcPrioritizeActionSpiritualDebtIncome(sbyte targetGrade)
		{
			return VillagerRoleFormulaImpl.Calculate(8, (int)targetGrade, this.MaxHealingAttainment);
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060013A5 RID: 5029 RVA: 0x00139824 File Offset: 0x00137A24
		public int HealXiangshuInfectionAmount
		{
			get
			{
				return VillagerRoleFormulaImpl.Calculate(9, this.MaxHealingAttainment, base.Personality);
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060013A6 RID: 5030 RVA: 0x00139839 File Offset: 0x00137A39
		public int MaxHealingAttainment
		{
			get
			{
				return (int)Math.Max(this.Character.GetLifeSkillAttainment(8), this.Character.GetLifeSkillAttainment(9));
			}
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x0013985C File Offset: 0x00137A5C
		unsafe void IVillagerRoleArrangementExecutor.ExecuteArrangementAction(DataContext context, VillagerRoleArrangementAction action)
		{
			VillagerRoleDoctor.<>c__DisplayClass10_0 CS$<>8__locals1 = new VillagerRoleDoctor.<>c__DisplayClass10_0();
			CS$<>8__locals1.<>4__this = this;
			Location location = this.Character.GetLocation();
			MapBlockData targetBlock = DomainManager.Map.GetBlock(location);
			HashSet<int> characterSet = targetBlock.CharacterSet;
			bool flag = characterSet == null || characterSet.Count <= 1;
			if (!flag)
			{
				CS$<>8__locals1.targetGrade = this.InteractTargetGrade;
				CS$<>8__locals1.hasChickenUpgrade = base.HasChickenUpgradeEffect;
				GameData.Domains.Character.Character targetChar = this.Character.SelectRandomActionTarget(context, targetBlock.CharacterSet, new Predicate<GameData.Domains.Character.Character>(CS$<>8__locals1.<GameData.Domains.Taiwu.VillagerRole.IVillagerRoleArrangementExecutor.ExecuteArrangementAction>g__Condition|0), true);
				bool flag2 = targetChar == null;
				if (!flag2)
				{
					Span<sbyte> span = new Span<sbyte>(stackalloc byte[(UIntPtr)5], 5);
					SpanList<sbyte> actionTypes = span;
					this.GetValidActionTypes(targetChar, ref actionTypes);
					bool flag3 = CS$<>8__locals1.hasChickenUpgrade && targetChar.GetXiangshuInfection() >= 100;
					if (flag3)
					{
						actionTypes.Add(4);
					}
					bool flag4 = actionTypes.Count <= 0;
					if (!flag4)
					{
						int docId = this.Character.GetId();
						int currDate = DomainManager.World.GetCurrDate();
						int income = this.CalcPrioritizeActionSpiritualDebtIncome(targetChar.GetInteractionGrade());
						LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
						lifeRecordCollection.AddVillagerTreatment1(docId, currDate, targetChar.GetId(), location);
						lifeRecordCollection.AddVillagerTreatmentTaiwu(DomainManager.Taiwu.GetTaiwuCharId(), currDate, docId, location, income);
						sbyte actionType = actionTypes.GetRandom(context.Random);
						bool flag5 = actionType == 4;
						if (flag5)
						{
							targetChar.ChangeXiangshuInfection(context, -this.HealXiangshuInfectionAmount);
							lifeRecordCollection.AddVillagerReduceXiangshuInfect(targetChar.GetId(), currDate);
						}
						else
						{
							this.Character.DoHealAction(context, (EHealActionType)actionType, targetChar, false, false);
						}
						DomainManager.Extra.ChangeAreaSpiritualDebt(context, location.AreaId, income, true, true);
					}
				}
			}
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x00139A20 File Offset: 0x00137C20
		bool IVillagerRoleSelectLocation.NextLocationFilter(MapBlockData block)
		{
			bool flag = block.CharacterSet == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = block.IsCityTown();
				if (flag2)
				{
					result = true;
				}
				else
				{
					bool hasChickenUpgrade = base.HasChickenUpgradeEffect;
					using (HashSet<int>.Enumerator enumerator = block.CharacterSet.GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							int charId = enumerator.Current;
							GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
							bool flag3 = this.HealActionCharacterFilter(character);
							if (flag3)
							{
								return true;
							}
							bool flag4 = hasChickenUpgrade;
							if (flag4)
							{
								byte infection = character.GetXiangshuInfection();
								bool flag5 = infection >= 100;
								if (flag5)
								{
									return true;
								}
							}
							return false;
						}
					}
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060013A9 RID: 5033 RVA: 0x00139AEC File Offset: 0x00137CEC
		public override void ExecuteFixedAction(DataContext context)
		{
			bool flag = this.ArrangementTemplateId >= 0;
			if (!flag)
			{
				bool flag2 = this.WorkData != null && this.WorkData.WorkType == 1;
				if (!flag2)
				{
					bool flag3 = base.AutoActionStates[4];
					if (flag3)
					{
						this.TryAddNextAutoTravelTarget(context, new Predicate<MapBlockData>(this.AutoActionBlockFilter));
						this.AutoCureForAuthorityAction(context);
					}
				}
			}
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x00139B5C File Offset: 0x00137D5C
		private unsafe bool AutoCureForAuthorityAction(DataContext context)
		{
			Location location = this.Character.GetLocation();
			sbyte targetGrade = this.InteractTargetGrade;
			MapBlockData block = DomainManager.Map.GetBlock(location);
			Span<sbyte> span = new Span<sbyte>(stackalloc byte[(UIntPtr)5], 5);
			SpanList<sbyte> actionTypes = span;
			int docId = this.Character.GetId();
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			bool flag = block.CharacterSet == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				foreach (int charId in block.CharacterSet)
				{
					GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
					bool flag2 = character.GetInteractionGrade() > targetGrade;
					if (!flag2)
					{
						this.GetValidActionTypes(character, ref actionTypes);
						bool flag3 = actionTypes.Count <= 0;
						if (!flag3)
						{
							EHealActionType actionType = (EHealActionType)actionTypes.GetRandom(context.Random);
							this.Character.DoHealAction(context, actionType, character, false, false);
							int income = this.AutoActionAuthorityIncome(character);
							DomainManager.Taiwu.AddResource(context, ItemSourceType.Resources, 7, income);
							lifeRecordCollection.AddVillagerTreatment0(docId, currDate, charId, location, income, 7);
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x00139CC0 File Offset: 0x00137EC0
		private void GetValidActionTypes(GameData.Domains.Character.Character character, ref SpanList<sbyte> actionTypes)
		{
			IReadOnlyList<EHealActionType> allActionTypes = GameData.Domains.Character.Character.AllHealActions;
			foreach (EHealActionType actionType in allActionTypes)
			{
				bool flag = this.CanHeal(character, actionType);
				if (flag)
				{
					actionTypes.Add((sbyte)actionType);
				}
			}
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x00139D20 File Offset: 0x00137F20
		private bool AutoActionBlockFilter(MapBlockData blockData)
		{
			bool flag = blockData.CharacterSet == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				sbyte interactTargetGrade = this.InteractTargetGrade;
				foreach (int charId in blockData.CharacterSet)
				{
					GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
					bool flag2 = character.GetInteractionGrade() > interactTargetGrade;
					if (!flag2)
					{
						bool flag3 = this.HealActionCharacterFilter(character);
						if (flag3)
						{
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x00139DC4 File Offset: 0x00137FC4
		private bool HealActionCharacterFilter(GameData.Domains.Character.Character character)
		{
			IReadOnlyList<EHealActionType> actionTypes = GameData.Domains.Character.Character.AllHealActions;
			foreach (EHealActionType actionType in actionTypes)
			{
				bool flag = this.CanHeal(character, actionType);
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x00139E28 File Offset: 0x00138028
		private bool CanHeal(GameData.Domains.Character.Character character, EHealActionType actionType)
		{
			if (!true)
			{
			}
			int num;
			switch (actionType)
			{
			case EHealActionType.Healing:
				num = character.GetInjuries().GetSum();
				break;
			case EHealActionType.Detox:
				num = character.GetPoisonMarkCount();
				break;
			case EHealActionType.Breathing:
				num = (int)character.GetDisorderOfQi();
				break;
			case EHealActionType.Recover:
				num = 100 - (int)(character.GetHealth() * 100) / Math.Max(1, (int)character.GetLeftMaxHealth(false));
				break;
			default:
				throw new ArgumentOutOfRangeException("actionType", actionType, null);
			}
			if (!true)
			{
			}
			int value = num;
			return value >= VillagerRoleFormulaImpl.Calculate(10, (int)actionType) && this.Character.CalcHealEffect(actionType, character, out num, false) > 0;
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x00139ED0 File Offset: 0x001380D0
		public override IVillagerRoleArrangementDisplayData GetArrangementDisplayData()
		{
			return new HealingDisplayData
			{
				InteractTargetGrade = (int)this.InteractTargetGrade,
				HealXiangshuInfectionAmount = this.HealXiangshuInfectionAmount
			};
		}
	}
}
