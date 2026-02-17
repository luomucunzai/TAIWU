using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Config;
using Config.ConfigCells.Character;
using GameData.ArchiveData;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Dependencies;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.Global;
using GameData.Domains.Item;
using GameData.Domains.Organization;
using GameData.Domains.Taiwu;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using NLog;
using Redzen.Random;

namespace GameData.Domains.CombatSkill
{
	// Token: 0x02000802 RID: 2050
	[GameDataDomain(7)]
	public class CombatSkillDomain : BaseGameDataDomain
	{
		// Token: 0x06006B69 RID: 27497 RVA: 0x003C25FE File Offset: 0x003C07FE
		private void OnInitializedDomainData()
		{
		}

		// Token: 0x06006B6A RID: 27498 RVA: 0x003C2604 File Offset: 0x003C0804
		private void InitializeOnInitializeGameDataModule()
		{
			CombatSkillDomain.EquipAddPropertyDict = new short[CombatSkill.Instance.Count][];
			short skillId = 0;
			while ((int)skillId < CombatSkill.Instance.Count)
			{
				List<PropertyAndValue> addPropertyList = CombatSkill.Instance[skillId].PropertyAddList;
				bool flag = addPropertyList != null && addPropertyList.Count > 0;
				if (flag)
				{
					short[] addValueList = new short[112];
					Array.Clear(addValueList, 0, addValueList.Length);
					foreach (PropertyAndValue addProperty in addPropertyList)
					{
						addValueList[(int)addProperty.PropertyId] = addProperty.Value;
					}
					CombatSkillDomain.EquipAddPropertyDict[(int)skillId] = addValueList;
				}
				skillId += 1;
			}
			CombatSkillDomain.InitializeLearnableCombatSkillTemplateIds();
		}

		// Token: 0x06006B6B RID: 27499 RVA: 0x003C26E0 File Offset: 0x003C08E0
		private void InitializeOnEnterNewWorld()
		{
		}

		// Token: 0x06006B6C RID: 27500 RVA: 0x003C26E3 File Offset: 0x003C08E3
		private void OnLoadedArchiveData()
		{
		}

		// Token: 0x06006B6D RID: 27501 RVA: 0x003C26E6 File Offset: 0x003C08E6
		public override void FixAbnormalDomainArchiveData(DataContext context)
		{
			this.FixInvalidActivateState(context);
		}

		// Token: 0x06006B6E RID: 27502 RVA: 0x003C26F4 File Offset: 0x003C08F4
		private void FixInvalidActivateState(DataContext context)
		{
			bool flag = !DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 76, 19) || !DomainManager.World.IsCurrWorldAfterVersion(0, 0, 76, 0);
			if (!flag)
			{
				int count = 0;
				foreach (KeyValuePair<CombatSkillKey, CombatSkill> keyValuePair in this._combatSkills)
				{
					CombatSkillKey combatSkillKey;
					CombatSkill combatSkill2;
					keyValuePair.Deconstruct(out combatSkillKey, out combatSkill2);
					CombatSkill combatSkill = combatSkill2;
					ushort activationState = combatSkill.GetActivationState();
					bool flag2 = !CombatSkillStateHelper.IsBrokenOut(activationState);
					if (!flag2)
					{
						bool flag3 = CombatSkillStateHelper.GetNormalPagesActivationCount(activationState) >= 5;
						if (!flag3)
						{
							combatSkill.SetActivationState(0, context);
							count++;
						}
					}
				}
				bool flag4 = count > 0;
				if (flag4)
				{
					Logger logger = CombatSkillDomain.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Un-breaking ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(count);
					defaultInterpolatedStringHandler.AppendLiteral(" combat skills.");
					logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
		}

		// Token: 0x06006B6F RID: 27503 RVA: 0x003C2814 File Offset: 0x003C0A14
		private static void InitializeLearnableCombatSkillTemplateIds()
		{
			CombatSkillDomain._learnableCombatSkillsCache = new List<CombatSkillItem>[16][];
			for (int i = 0; i < CombatSkillDomain._learnableCombatSkillsCache.Length; i++)
			{
				CombatSkillDomain._learnableCombatSkillsCache[i] = new List<CombatSkillItem>[14];
				for (int j = 0; j < 14; j++)
				{
					CombatSkillDomain._learnableCombatSkillsCache[i][j] = new List<CombatSkillItem>();
				}
			}
			foreach (CombatSkillItem combatSkillCfg in ((IEnumerable<CombatSkillItem>)CombatSkill.Instance))
			{
				bool flag = combatSkillCfg.BookId < 0;
				if (!flag)
				{
					CombatSkillDomain._learnableCombatSkillsCache[(int)combatSkillCfg.SectId][(int)combatSkillCfg.Type].Add(combatSkillCfg);
				}
			}
		}

		// Token: 0x06006B70 RID: 27504 RVA: 0x003C28E4 File Offset: 0x003C0AE4
		public static sbyte GetCombatSkillGradeGroup(sbyte grade, sbyte orgTemplateId, sbyte combatSkillType)
		{
			int cnt = CombatSkillDomain.GetLearnableCombatSkills(orgTemplateId, combatSkillType).Count;
			bool flag = cnt > 6;
			sbyte result;
			if (flag)
			{
				result = Grade.GetGroup(grade);
			}
			else
			{
				int avg = cnt / 3;
				int rem = cnt % 3;
				int mid = avg + ((rem > 0) ? 1 : 0);
				int high = mid + avg + ((rem > 1) ? 1 : 0);
				result = (((int)grade >= high) ? 2 : (((int)grade >= mid) ? 1 : 0));
			}
			return result;
		}

		// Token: 0x06006B71 RID: 27505 RVA: 0x003C294B File Offset: 0x003C0B4B
		public static IReadOnlyList<IReadOnlyList<CombatSkillItem>> GetLearnableCombatSkills(sbyte orgTemplateId)
		{
			return CombatSkillDomain._learnableCombatSkillsCache[(int)orgTemplateId];
		}

		// Token: 0x06006B72 RID: 27506 RVA: 0x003C2954 File Offset: 0x003C0B54
		public static IReadOnlyList<CombatSkillItem> GetLearnableCombatSkills(sbyte orgTemplateId, sbyte combatSkillType)
		{
			return CombatSkillDomain._learnableCombatSkillsCache[(int)orgTemplateId][(int)combatSkillType];
		}

		// Token: 0x06006B73 RID: 27507 RVA: 0x003C2960 File Offset: 0x003C0B60
		[DomainMethod]
		public bool SetActivePage(DataContext context, int charId, short skillId, byte pageId, sbyte direction)
		{
			bool flag = pageId <= 0 || pageId >= 6;
			bool flag2 = flag;
			bool result;
			if (flag2)
			{
				result = false;
			}
			else
			{
				flag = (direction <= 1);
				bool flag3 = !flag;
				if (flag3)
				{
					result = false;
				}
				else
				{
					GameData.Domains.Character.Character character;
					bool flag4 = !DomainManager.Character.TryGetElement_Objects(charId, out character);
					if (flag4)
					{
						result = false;
					}
					else
					{
						CombatSkill skill;
						bool flag5 = !this.TryGetElement_CombatSkills(new ValueTuple<int, short>(charId, skillId), out skill);
						if (flag5)
						{
							result = false;
						}
						else
						{
							bool isDirect = direction == 0;
							byte directIndex = CombatSkillStateHelper.GetPageInternalIndex(-1, 0, pageId);
							ushort readingState = skill.GetReadingState();
							bool flag6 = isDirect && !CombatSkillStateHelper.IsPageRead(readingState, directIndex);
							if (flag6)
							{
								result = false;
							}
							else
							{
								bool isReverse = direction == 1;
								byte reverseIndex = CombatSkillStateHelper.GetPageInternalIndex(-1, 1, pageId);
								bool flag7 = isReverse && !CombatSkillStateHelper.IsPageRead(readingState, reverseIndex);
								if (flag7)
								{
									result = false;
								}
								else
								{
									ushort activationState = skill.GetActivationState();
									sbyte prevDir = CombatSkillStateHelper.GetCombatSkillDirection(activationState);
									bool isTaiwu = character.IsTaiwu();
									activationState = CombatSkillStateHelper.SetPageInactive(activationState, directIndex);
									activationState = CombatSkillStateHelper.SetPageInactive(activationState, reverseIndex);
									bool flag8 = isDirect;
									if (flag8)
									{
										activationState = CombatSkillStateHelper.SetPageActive(activationState, directIndex);
									}
									bool flag9 = isReverse;
									if (flag9)
									{
										activationState = CombatSkillStateHelper.SetPageActive(activationState, reverseIndex);
									}
									bool flag10 = skill.GetActivationState() == activationState;
									if (flag10)
									{
										result = false;
									}
									else
									{
										bool isBrokenOut = CombatSkillStateHelper.IsBrokenOut(activationState);
										bool flag11 = isTaiwu && isBrokenOut && !DomainManager.Taiwu.UpdateBreakPlateSelectedPages(context, skillId, activationState);
										if (flag11)
										{
											result = false;
										}
										else
										{
											skill.SetActivationState(activationState, context);
											sbyte currDir = CombatSkillStateHelper.GetCombatSkillDirection(activationState);
											bool flag12 = isTaiwu && prevDir == 0 && currDir == 1;
											if (flag12)
											{
												bool flag13 = CombatSkill.Instance[skillId].SectId == 4;
												if (flag13)
												{
													GameDataBridge.AddDisplayEvent<short>(DisplayEventType.SectMainStoryWudangStart, skillId);
												}
											}
											bool flag14 = !character.IsCombatSkillEquipped(skillId);
											if (flag14)
											{
												result = true;
											}
											else
											{
												DomainManager.SpecialEffect.Remove(context, charId, skillId, 2);
												DomainManager.SpecialEffect.Add(context, charId, skillId, 2, -1);
												result = true;
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06006B74 RID: 27508 RVA: 0x003C2B84 File Offset: 0x003C0D84
		[DomainMethod]
		public bool DeActivePage(DataContext context, int charId, short skillId, byte pageId, sbyte direction)
		{
			bool flag = pageId <= 0 || pageId >= 6;
			bool flag2 = flag;
			bool result;
			if (flag2)
			{
				result = false;
			}
			else
			{
				flag = (direction <= 1);
				bool flag3 = !flag;
				if (flag3)
				{
					result = false;
				}
				else
				{
					GameData.Domains.Character.Character character;
					bool flag4 = !DomainManager.Character.TryGetElement_Objects(charId, out character);
					if (flag4)
					{
						result = false;
					}
					else
					{
						CombatSkill skill;
						bool flag5 = !this.TryGetElement_CombatSkills(new ValueTuple<int, short>(charId, skillId), out skill);
						if (flag5)
						{
							result = false;
						}
						else
						{
							bool isDirect = direction == 0;
							byte directIndex = CombatSkillStateHelper.GetNormalPageInternalIndex(0, pageId);
							ushort readingState = skill.GetReadingState();
							bool flag6 = isDirect && !CombatSkillStateHelper.IsPageRead(readingState, directIndex);
							if (flag6)
							{
								result = false;
							}
							else
							{
								bool isReverse = direction == 1;
								byte reverseIndex = CombatSkillStateHelper.GetNormalPageInternalIndex(1, pageId);
								bool flag7 = isReverse && !CombatSkillStateHelper.IsPageRead(readingState, reverseIndex);
								if (flag7)
								{
									result = false;
								}
								else
								{
									ushort activationState = skill.GetActivationState();
									bool flag8 = CombatSkillStateHelper.IsBrokenOut(activationState);
									if (flag8)
									{
										result = false;
									}
									else
									{
										activationState = CombatSkillStateHelper.SetPageInactive(activationState, directIndex);
										activationState = CombatSkillStateHelper.SetPageInactive(activationState, reverseIndex);
										bool flag9 = skill.GetActivationState() == activationState;
										if (flag9)
										{
											result = false;
										}
										else
										{
											skill.SetActivationState(activationState, context);
											bool flag10 = !character.IsCombatSkillEquipped(skillId);
											if (flag10)
											{
												result = true;
											}
											else
											{
												DomainManager.SpecialEffect.Remove(context, charId, skillId, 2);
												DomainManager.SpecialEffect.Add(context, charId, skillId, 2, -1);
												result = true;
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06006B75 RID: 27509 RVA: 0x003C2D0C File Offset: 0x003C0F0C
		public bool TryActivateCombatSkillBookPageWhenSetReadingState(DataContext context, int charId, short combatSkillTemplateId, byte pageInternalIndex)
		{
			bool flag = pageInternalIndex < 5;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = pageInternalIndex >= 15;
				if (flag2)
				{
					result = false;
				}
				else
				{
					CombatSkillKey combatSkillKey = new CombatSkillKey(charId, combatSkillTemplateId);
					CombatSkill combatSkill;
					bool flag3 = !this.TryGetElement_CombatSkills(combatSkillKey, out combatSkill);
					if (flag3)
					{
						result = false;
					}
					else
					{
						byte pageId = CombatSkillStateHelper.GetPageId(pageInternalIndex);
						ushort readState = combatSkill.GetReadingState();
						bool pageRead = CombatSkillStateHelper.IsPageRead(readState, pageInternalIndex);
						bool flag4 = !pageRead;
						if (flag4)
						{
							result = false;
						}
						else
						{
							ushort activationState = combatSkill.GetActivationState();
							bool isActive = CombatSkillStateHelper.IsPageActive(activationState, pageInternalIndex);
							bool flag5 = isActive;
							if (flag5)
							{
								result = false;
							}
							else
							{
								byte oppositeInternalIndex = CombatSkillStateHelper.GetNormalPageOppositeInternalIndex(pageInternalIndex);
								bool isOppositeActive = CombatSkillStateHelper.IsPageActive(activationState, oppositeInternalIndex);
								bool flag6 = isOppositeActive;
								if (flag6)
								{
									result = false;
								}
								else
								{
									sbyte readPageType = (pageInternalIndex < 10) ? 0 : 1;
									this.SetActivePage(context, charId, combatSkillTemplateId, pageId, readPageType);
									result = true;
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06006B76 RID: 27510 RVA: 0x003C2DF4 File Offset: 0x003C0FF4
		public Dictionary<short, CombatSkill> GetCharCombatSkills(int charId)
		{
			Dictionary<short, CombatSkill> charCombatSkills;
			return this._combatSkills.Collection.TryGetValue(charId, out charCombatSkills) ? charCombatSkills : CombatSkillDomain.EmptyCharCombatSkills;
		}

		// Token: 0x06006B77 RID: 27511 RVA: 0x003C2E24 File Offset: 0x003C1024
		[return: TupleElementNames(new string[]
		{
			"neili",
			"qiDisorder",
			"extraNeiliAllocationProgress"
		})]
		public static ValueTuple<short, short, int[]> CalcNeigongLoopingEffect(IRandomSource random, GameData.Domains.Character.Character character, CombatSkillItem skillCfg, bool includeReference = true)
		{
			ValueTuple<short, int, ItemKey> feast;
			bool isParticipantFeast = DomainManager.Building.IsCharacterParticipantFeast(character.GetId(), out feast);
			sbyte neiliType = character.GetNeiliType();
			byte neiliElementType = NeiliType.Instance[neiliType].FiveElements;
			ValueTuple<int, short> valueTuple = CombatSkillDomain.CalcNeigongLoopingEffect_GetBenefitAndQiDisorder(neiliElementType, character.GetId(), skillCfg);
			int benefit = valueTuple.Item1;
			short qiDisorder = valueTuple.Item2;
			ConsummateLevelItem consummateLevelConfig = ConsummateLevel.Instance[(int)(character.IsLoseConsummateBonusByFeature() ? 0 : character.GetConsummateLevel())];
			CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(character.GetId(), skillCfg.TemplateId));
			int obtainNeili = (int)skillCfg.ObtainedNeiliPerLoop + skill.GetBreakoutGridCombatSkillPropertyBonus(7);
			bool isTaiwu = character.GetId() == DomainManager.Taiwu.GetTaiwuCharId();
			bool flag = isTaiwu && includeReference;
			if (flag)
			{
				short combatSkillTemplateId = character.GetLoopingNeigong();
				CombatSkillItem skillConfig = CombatSkill.Instance[combatSkillTemplateId];
				benefit += CombatSkillDomain.GetTaiwuReferenceSkillNeiliBonus(skillConfig);
				benefit += DomainManager.Taiwu.GetQiArtStrategyDeltaNeiliBonus(random);
				benefit += CombatSkillDomain.CalcTaiwuProfessionNeigongLoopingEffectBonus();
			}
			benefit += consummateLevelConfig.LoopingNeiliBonus;
			OrganizationInfo orgInfo = character.GetOrganizationInfo();
			bool flag2 = !isTaiwu && orgInfo.SettlementId >= 0 && OrganizationDomain.IsSect(orgInfo.OrgTemplateId);
			if (flag2)
			{
				Settlement settlement = DomainManager.Organization.GetSettlement(orgInfo.SettlementId);
				benefit = benefit * settlement.GetMemberSelfImproveSpeedFactor() / 100;
			}
			bool flag3 = isParticipantFeast;
			if (flag3)
			{
				FeastItem config = Feast.Instance[feast.Item1];
				benefit += benefit * config.Loop / 100 * feast.Item2 / 100;
			}
			int neili = obtainNeili * benefit / 100;
			int neiliMin = neili * 3 / 4;
			int neiliMax = neili * 5 / 4;
			neili = random.Next(neiliMin, neiliMax + 1);
			bool flag4 = qiDisorder > 0;
			if (flag4)
			{
				qiDisorder = (short)random.Next((int)(qiDisorder + 1));
			}
			else
			{
				bool flag5 = qiDisorder < 0;
				if (flag5)
				{
					qiDisorder = (short)(-(short)random.Next((int)(-qiDisorder + 1)));
				}
			}
			bool flag6 = isParticipantFeast;
			if (flag6)
			{
				FeastItem config2 = Feast.Instance[feast.Item1];
				qiDisorder += (short)((int)qiDisorder * config2.Loop / 100 * feast.Item2 / 100);
			}
			sbyte[] skillConfigExtraProgress = skillCfg.ExtraNeiliAllocationProgress;
			int[] randomExtraNeiliAllocationProgress = RandomUtils.DistributeNIntoKBuckets(random, (int)skillConfigExtraProgress[4], 4);
			int[] extraNeiliAllocationProgress = new int[4];
			int bonus = 0;
			bool flag7 = isTaiwu;
			if (flag7)
			{
				short combatSkillTemplateId2 = character.GetLoopingNeigong();
				CombatSkillItem skillConfig2 = CombatSkill.Instance[combatSkillTemplateId2];
				bonus += CombatSkillDomain.GetTaiwuReferenceSkillNeiliAllocationBonus(skillConfig2);
				bonus += DomainManager.Taiwu.GetQiArtStrategyExtraNeiliAllocationBonus(random);
				bonus += CombatSkillDomain.CalcTaiwuProfessionNeigongLoopingEffectBonus();
			}
			bonus += consummateLevelConfig.LoopingNeiliAllocationBonus;
			bool flag8 = isParticipantFeast;
			if (flag8)
			{
				FeastItem config3 = Feast.Instance[feast.Item1];
				bonus += config3.Loop / 100 * feast.Item2;
			}
			for (int i = 0; i < 4; i++)
			{
				extraNeiliAllocationProgress[i] = 100 * ((int)skillConfigExtraProgress[i] + randomExtraNeiliAllocationProgress[i]);
				extraNeiliAllocationProgress[i] *= bonus;
			}
			return new ValueTuple<short, short, int[]>((short)neili, qiDisorder, extraNeiliAllocationProgress);
		}

		// Token: 0x06006B78 RID: 27512 RVA: 0x003C3140 File Offset: 0x003C1340
		private static int CalcTaiwuProfessionNeigongLoopingEffectBonus()
		{
			bool isEquipedTargetSkill = DomainManager.Extra.IsProfessionalSkillUnlocked(3, 1);
			bool flag = !isEquipedTargetSkill;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int seniority = DomainManager.Extra.GetProfessionData(3).Seniority;
				int maxSeniority = 3000000;
				int bonus = 50 + 50 * seniority / maxSeniority;
				result = bonus;
			}
			return result;
		}

		// Token: 0x06006B79 RID: 27513 RVA: 0x003C3194 File Offset: 0x003C1394
		[DomainMethod]
		[return: TupleElementNames(new string[]
		{
			"minNeili",
			"maxNeili"
		})]
		public ValueTuple<int, int> CalcTaiwuExtraDeltaNeiliPerLoop(DataContext context)
		{
			GameData.Domains.Character.Character character = DomainManager.Taiwu.GetTaiwu();
			short loopingNeigong = character.GetLoopingNeigong();
			bool flag = loopingNeigong < 0;
			ValueTuple<int, int> result;
			if (flag)
			{
				result = new ValueTuple<int, int>(0, 0);
			}
			else
			{
				CombatSkillItem skillConfig = CombatSkill.Instance[loopingNeigong];
				sbyte neiliType = character.GetNeiliType();
				byte neiliElementType = NeiliType.Instance[neiliType].FiveElements;
				ValueTuple<int, short> valueTuple = CombatSkillDomain.CalcNeigongLoopingEffect_GetBenefitAndQiDisorder(neiliElementType, character.GetId(), skillConfig);
				int bonus = valueTuple.Item1;
				short _qiDisorder = valueTuple.Item2;
				ConsummateLevelItem consummateLevelConfig = ConsummateLevel.Instance[(int)(character.IsLoseConsummateBonusByFeature() ? 0 : character.GetConsummateLevel())];
				CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(character.GetId(), skillConfig.TemplateId));
				int obtainNeili = (int)skillConfig.ObtainedNeiliPerLoop + skill.GetBreakoutGridCombatSkillPropertyBonus(7);
				int num = bonus;
				int maxBonus = bonus;
				int minBonus = num;
				int refBonus = CombatSkillDomain.GetTaiwuReferenceSkillNeiliBonus(skillConfig);
				minBonus += refBonus;
				maxBonus += refBonus;
				ValueTuple<int, int> strategyBonusRange = DomainManager.Taiwu.GetQiArtStrategyDeltaNeiliBonusRange();
				minBonus += strategyBonusRange.Item1;
				maxBonus += strategyBonusRange.Item2;
				int professionBonus = CombatSkillDomain.CalcTaiwuProfessionNeigongLoopingEffectBonus();
				minBonus += professionBonus;
				maxBonus += professionBonus;
				minBonus += consummateLevelConfig.LoopingNeiliBonus;
				maxBonus += consummateLevelConfig.LoopingNeiliBonus;
				int minNeili = obtainNeili * minBonus / 100;
				int maxNeili = obtainNeili * maxBonus / 100;
				int neiliMin = minNeili * 3 / 4;
				int neiliMax = maxNeili * 5 / 4;
				byte loopingDifficulty = DomainManager.Extra.GetLoopingDifficulty();
				short factor = WorldCreation.Instance[10].InfluenceFactors[(int)loopingDifficulty];
				neiliMin = neiliMin * (int)factor / 100;
				neiliMax = neiliMax * (int)factor / 100;
				result = new ValueTuple<int, int>(neiliMin - (int)skillConfig.ObtainedNeiliPerLoop, neiliMax - (int)skillConfig.ObtainedNeiliPerLoop);
			}
			return result;
		}

		// Token: 0x06006B7A RID: 27514 RVA: 0x003C3340 File Offset: 0x003C1540
		[DomainMethod]
		public IntList CalcTaiwuExtraDeltaNeiliAllocationPerLoop(DataContext context)
		{
			GameData.Domains.Character.Character character = DomainManager.Taiwu.GetTaiwu();
			short loopingNeigong = character.GetLoopingNeigong();
			int[] min = new int[4];
			int[] max = new int[4];
			IntList result = IntList.Create();
			for (int i = 0; i < 8; i++)
			{
				result.Items.Add(0);
			}
			bool flag = loopingNeigong < 0;
			IntList result2;
			if (flag)
			{
				result2 = result;
			}
			else
			{
				CombatSkillItem skillConfig = CombatSkill.Instance[loopingNeigong];
				sbyte[] basicProgress = skillConfig.ExtraNeiliAllocationProgress;
				for (int j = 0; j < 4; j++)
				{
					min[j] = (int)(100 * basicProgress[j]);
					max[j] = (int)(100 * (basicProgress[j] + basicProgress[4]));
				}
				int minBonus = 0;
				int maxBonus = 0;
				short combatSkillTemplateId = character.GetLoopingNeigong();
				int refBonus = CombatSkillDomain.GetTaiwuReferenceSkillNeiliAllocationBonus(skillConfig);
				minBonus += refBonus;
				maxBonus += refBonus;
				ValueTuple<int, int> strategyBonusRange = DomainManager.Taiwu.GetQiArtStrategyExtraNeiliAllocationBonusRange();
				minBonus += strategyBonusRange.Item1;
				maxBonus += strategyBonusRange.Item2;
				int professionBonus = CombatSkillDomain.CalcTaiwuProfessionNeigongLoopingEffectBonus();
				minBonus += professionBonus;
				maxBonus += professionBonus;
				ConsummateLevelItem consummateLevelConfig = ConsummateLevel.Instance[(int)(character.IsLoseConsummateBonusByFeature() ? 0 : character.GetConsummateLevel())];
				minBonus += consummateLevelConfig.LoopingNeiliAllocationBonus;
				maxBonus += consummateLevelConfig.LoopingNeiliAllocationBonus;
				for (int k = 0; k < 4; k++)
				{
					min[k] *= minBonus;
					max[k] *= maxBonus;
					min[k] -= (int)(basicProgress[k] * 100);
					max[k] -= (int)(basicProgress[k] * 100);
				}
				for (int l = 0; l < 4; l++)
				{
					result.Items[l] = min[l];
				}
				for (int m = 0; m < 4; m++)
				{
					result.Items[m + 4] = max[m];
				}
				result2 = result;
			}
			return result2;
		}

		// Token: 0x06006B7B RID: 27515 RVA: 0x003C355C File Offset: 0x003C175C
		private static int GetTaiwuReferenceSkillNeiliBonus(CombatSkillItem skillConfig)
		{
			List<short> referenceSkillList = DomainManager.Extra.GetReferenceSkillList();
			int referenceBonus = 0;
			bool flag = referenceSkillList != null && referenceSkillList.Count > 0;
			if (flag)
			{
				for (int i = 0; i < referenceSkillList.Count; i++)
				{
					short refSkill = referenceSkillList[i];
					bool flag2 = skillConfig.LoopBonusSkillList.Contains(refSkill);
					if (flag2)
					{
						referenceBonus += 10;
					}
					bool flag3 = refSkill != -1;
					if (flag3)
					{
						CombatSkillItem refSkillConfig = CombatSkill.Instance[refSkill];
						bool flag4 = refSkillConfig.SectId == skillConfig.SectId;
						if (flag4)
						{
							referenceBonus += 20;
						}
					}
				}
			}
			return referenceBonus;
		}

		// Token: 0x06006B7C RID: 27516 RVA: 0x003C3608 File Offset: 0x003C1808
		private static int GetTaiwuReferenceSkillNeiliAllocationBonus(CombatSkillItem skillConfig)
		{
			int referenceBonus = 0;
			List<short> referenceSkillList = DomainManager.Extra.GetReferenceSkillList();
			bool flag = referenceSkillList != null && referenceSkillList.Count > 0;
			if (flag)
			{
				for (int i = 0; i < referenceSkillList.Count; i++)
				{
					short refSkill = referenceSkillList[i];
					bool flag2 = skillConfig.LoopBonusSkillList.Contains(refSkill);
					if (flag2)
					{
						referenceBonus += 10;
					}
					bool flag3 = refSkill != -1;
					if (flag3)
					{
						CombatSkillItem refSkillConfig = CombatSkill.Instance[refSkill];
						bool flag4 = refSkillConfig.SectId == skillConfig.SectId;
						if (flag4)
						{
							referenceBonus += 20;
						}
					}
				}
			}
			return referenceBonus;
		}

		// Token: 0x06006B7D RID: 27517 RVA: 0x003C36B4 File Offset: 0x003C18B4
		public void ApplyNeigongLoopingEffect(DataContext context, GameData.Domains.Character.Character character, short combatSkillTemplateId, short obtainedNeili, int[] extraNeiliAllocationProgress)
		{
			int charId = character.GetId();
			CombatSkillKey skillKey = new CombatSkillKey(charId, combatSkillTemplateId);
			CombatSkill skill = this._combatSkills[skillKey];
			this.ApplyLoopingNeiliModify(context, character, obtainedNeili, skill);
			CombatSkillDomain.ApplyLoopingTransferNeiliProportionOfFiveElements(context, character, combatSkillTemplateId, skill);
			this.ApplyLoopingExtraNeiliAllocationProgressModify(context, character, extraNeiliAllocationProgress);
		}

		// Token: 0x06006B7E RID: 27518 RVA: 0x003C3700 File Offset: 0x003C1900
		public void ApplyLoopingNeiliModify(DataContext context, GameData.Domains.Character.Character character, short obtainedNeili, CombatSkill skill)
		{
			short oriObtainedNeili = skill.GetObtainedNeili();
			skill.ObtainNeili(context, obtainedNeili);
			int deltaNeili = (int)(skill.GetObtainedNeili() - oriObtainedNeili);
			bool flag = deltaNeili > 0;
			if (flag)
			{
				int currNeili = character.GetCurrNeili() + deltaNeili;
				int maxNeili = character.GetMaxNeili();
				bool flag2 = currNeili > maxNeili;
				if (flag2)
				{
					currNeili = maxNeili;
				}
				character.SetCurrNeili(currNeili, context);
			}
		}

		// Token: 0x06006B7F RID: 27519 RVA: 0x003C375C File Offset: 0x003C195C
		public void ApplyLoopingNeiliModifyForTaiwu(DataContext context, GameData.Domains.Character.Character taiwuChar, short obtainedNeili)
		{
			int charId = taiwuChar.GetId();
			short combatSkillTemplateId = taiwuChar.GetLoopingNeigong();
			CombatSkillKey skillKey = new CombatSkillKey(charId, combatSkillTemplateId);
			CombatSkill skill = this._combatSkills[skillKey];
			this.ApplyLoopingNeiliModify(context, taiwuChar, obtainedNeili, skill);
		}

		// Token: 0x06006B80 RID: 27520 RVA: 0x003C379C File Offset: 0x003C199C
		public unsafe void ApplyLoopingExtraNeiliAllocationProgressModify(DataContext context, GameData.Domains.Character.Character character, int[] extraNeiliAllocationProgress)
		{
			int charId = character.GetId();
			IntList characterExtraNeiliAllocationProgress = DomainManager.Extra.GetOrInitExtraNeiliAllocationProgress(context, charId);
			NeiliAllocation deltaAllocation;
			for (int neiliType = 0; neiliType < 4; neiliType++)
			{
				int currentProgress = characterExtraNeiliAllocationProgress.Items[neiliType];
				int maxProgress = CombatSkillDomain.GetNeiliAllocationMaxProgress();
				int deltaProgress = extraNeiliAllocationProgress[neiliType];
				bool flag = deltaProgress > 0 && currentProgress >= maxProgress;
				if (flag)
				{
					deltaProgress = 0;
				}
				characterExtraNeiliAllocationProgress.Items[neiliType] = Math.Max(0, currentProgress + deltaProgress);
				int newProgress = characterExtraNeiliAllocationProgress.Items[neiliType];
				int delta = CombatSkillDomain.CalculateDeltaNeiliAllocation(currentProgress, newProgress);
				*deltaAllocation[neiliType] = (short)delta;
			}
			DomainManager.Extra.SetExtraNeiliAllocationProgress(context, charId, characterExtraNeiliAllocationProgress);
			character.ChangeExtraNeiliAllocation(context, deltaAllocation);
		}

		// Token: 0x06006B81 RID: 27521 RVA: 0x003C3860 File Offset: 0x003C1A60
		private static void ApplyLoopingTransferNeiliProportionOfFiveElements(DataContext context, GameData.Domains.Character.Character character, short combatSkillTemplateId, CombatSkill loopingSkill)
		{
			ValueTuple<sbyte, sbyte, sbyte> loopingTransferNeiliProportionOfFiveElementsData = CombatSkillDomain.GetLoopingTransferNeiliProportionOfFiveElementsData(context, character, combatSkillTemplateId, loopingSkill);
			sbyte destinationType = loopingTransferNeiliProportionOfFiveElementsData.Item1;
			sbyte transferType = loopingTransferNeiliProportionOfFiveElementsData.Item2;
			sbyte amount = loopingTransferNeiliProportionOfFiveElementsData.Item3;
			bool flag = transferType >= 0;
			if (flag)
			{
				bool flag2 = amount <= 0;
				if (!flag2)
				{
					character.TransferNeiliProportionOfFiveElements(context, destinationType, transferType, (int)amount);
				}
			}
		}

		// Token: 0x06006B82 RID: 27522 RVA: 0x003C38B4 File Offset: 0x003C1AB4
		[return: TupleElementNames(new string[]
		{
			"destinationType",
			"transferType",
			"amount"
		})]
		private static ValueTuple<sbyte, sbyte, sbyte> GetLoopingTransferNeiliProportionOfFiveElementsData(DataContext context, GameData.Domains.Character.Character character, short combatSkillTemplateId, CombatSkill loopingSkill)
		{
			CombatSkillItem config = CombatSkill.Instance[combatSkillTemplateId];
			sbyte amount = loopingSkill.GetFiveElementsChange();
			sbyte destinationType = config.DestTypeWhileLooping;
			sbyte transferType = config.TransferTypeWhileLooping;
			bool isTaiwu = character.GetId() == DomainManager.Taiwu.GetTaiwuCharId();
			bool flag = isTaiwu;
			if (flag)
			{
				ValueTuple<sbyte, sbyte> overrideFiveElementTransferInfo = DomainManager.Taiwu.GetOverrideFiveElementTransferInfo();
				sbyte overrideDestinationType = overrideFiveElementTransferInfo.Item1;
				sbyte overrideTransferType = overrideFiveElementTransferInfo.Item2;
				bool flag2 = overrideDestinationType > -1;
				if (flag2)
				{
					destinationType = overrideDestinationType;
					transferType = overrideTransferType;
				}
				short anchordFiveElement = DomainManager.Taiwu.GetAnchoredFiveElements();
				sbyte neiliType = character.GetNeiliType();
				NeiliTypeItem neiliTypeConfig = NeiliType.Instance[neiliType];
				bool isConflict = neiliTypeConfig.ColorType == 2;
				bool flag3 = anchordFiveElement > -1 && !isConflict && (short)neiliTypeConfig.FiveElements == anchordFiveElement;
				if (flag3)
				{
					destinationType = -1;
					transferType = -1;
				}
				int amountBonus = DomainManager.Taiwu.GetQiArtStrategyFiveElementTransferAmountBonus(context.Random);
				amount = (sbyte)((int)amount * (100 + amountBonus) / 100);
			}
			return new ValueTuple<sbyte, sbyte, sbyte>(destinationType, transferType, amount);
		}

		// Token: 0x06006B83 RID: 27523 RVA: 0x003C39B0 File Offset: 0x003C1BB0
		[return: TupleElementNames(new string[]
		{
			"destinationType",
			"transferType",
			"amount"
		})]
		public ValueTuple<sbyte, sbyte, sbyte> GetLoopingTransferNeiliProportionOfFiveElementsDataForTaiwu(DataContext context, GameData.Domains.Character.Character taiwuChar)
		{
			int charId = taiwuChar.GetId();
			short combatSkillTemplateId = taiwuChar.GetLoopingNeigong();
			CombatSkillKey skillKey = new CombatSkillKey(charId, combatSkillTemplateId);
			CombatSkill skill = this._combatSkills[skillKey];
			return CombatSkillDomain.GetLoopingTransferNeiliProportionOfFiveElementsData(context, taiwuChar, combatSkillTemplateId, skill);
		}

		// Token: 0x06006B84 RID: 27524 RVA: 0x003C39F4 File Offset: 0x003C1BF4
		private static int CalculateDeltaNeiliAllocation(int currentProgress, int newProgress)
		{
			bool flag = newProgress > currentProgress;
			int result;
			if (flag)
			{
				result = CombatSkillDomain.GetNeiliAllocationProgressMilestoneCount(currentProgress, newProgress);
			}
			else
			{
				result = -CombatSkillDomain.GetNeiliAllocationProgressMilestoneCount(newProgress, currentProgress);
			}
			return result;
		}

		// Token: 0x06006B85 RID: 27525 RVA: 0x003C3A24 File Offset: 0x003C1C24
		[Obsolete]
		private static List<int> GenerateNeiliAllocationProgressMinestones(int currentProgress, int newProgress)
		{
			int basic = (int)(GlobalConfig.Instance.ExtraNeiliAllocationFromProgressRatio * 100);
			int delta = (int)(GlobalConfig.Instance.ExtraNeiliAllocationFromProgressRatioGrowth * 100);
			int p0 = 0;
			int d0 = basic;
			List<int> result = new List<int>();
			while (p0 <= newProgress)
			{
				bool flag = p0 > currentProgress;
				if (flag)
				{
					result.Add(p0);
				}
				p0 += d0;
				d0 += delta;
			}
			return result;
		}

		// Token: 0x06006B86 RID: 27526 RVA: 0x003C3A90 File Offset: 0x003C1C90
		private static int GetNeiliAllocationProgressMilestoneCount(int currentProgress, int newProgress)
		{
			int basic = (int)(GlobalConfig.Instance.ExtraNeiliAllocationFromProgressRatio * 100);
			int delta = (int)(GlobalConfig.Instance.ExtraNeiliAllocationFromProgressRatioGrowth * 100);
			int p0 = 0;
			int d0 = basic;
			int count = 0;
			while (p0 <= newProgress)
			{
				bool flag = p0 > currentProgress;
				if (flag)
				{
					count++;
				}
				p0 += d0;
				d0 += delta;
			}
			return count;
		}

		// Token: 0x06006B87 RID: 27527 RVA: 0x003C3AF4 File Offset: 0x003C1CF4
		private static int GetNeiliAllocationProgressMilestoneByIndex(int currentProgress, int newProgress, int index)
		{
			int basic = (int)(GlobalConfig.Instance.ExtraNeiliAllocationFromProgressRatio * 100);
			int delta = (int)(GlobalConfig.Instance.ExtraNeiliAllocationFromProgressRatioGrowth * 100);
			int p0 = 0;
			int d0 = basic;
			int count = -1;
			while (p0 <= newProgress)
			{
				bool flag = p0 > currentProgress;
				if (flag)
				{
					count++;
					bool flag2 = count == index;
					if (flag2)
					{
						return p0;
					}
				}
				p0 += d0;
				d0 += delta;
			}
			throw new ArgumentOutOfRangeException("index", "下标超出了可用进度节点的范围");
		}

		// Token: 0x06006B88 RID: 27528 RVA: 0x003C3B74 File Offset: 0x003C1D74
		public static int GetExtraNeiliAllocationByProgress(int currentProgress)
		{
			int basic = (int)(GlobalConfig.Instance.ExtraNeiliAllocationFromProgressRatio * 100);
			int delta = (int)(GlobalConfig.Instance.ExtraNeiliAllocationFromProgressRatioGrowth * 100);
			int p0 = 0;
			int d0 = basic;
			int extra = 0;
			while (p0 <= currentProgress)
			{
				bool flag = p0 > 0;
				if (flag)
				{
					extra++;
				}
				p0 += d0;
				d0 += delta;
			}
			return extra;
		}

		// Token: 0x06006B89 RID: 27529 RVA: 0x003C3BD8 File Offset: 0x003C1DD8
		public static int GetNeiliAllocationMaxProgress()
		{
			short i = GlobalConfig.Instance.MaxExtraNeiliAllocation;
			return CombatSkillDomain.GetExtraNeiliAllocationProgressByExtraNeiliAllocation((int)i);
		}

		// Token: 0x06006B8A RID: 27530 RVA: 0x003C3BFC File Offset: 0x003C1DFC
		public static int GetExtraNeiliAllocationProgressByExtraNeiliAllocation(int extraNeiliAllocation)
		{
			int basic = (int)(GlobalConfig.Instance.ExtraNeiliAllocationFromProgressRatio * 100);
			int delta = (int)(GlobalConfig.Instance.ExtraNeiliAllocationFromProgressRatioGrowth * 100);
			return basic * extraNeiliAllocation + delta * (extraNeiliAllocation * (extraNeiliAllocation - 1)) / 2;
		}

		// Token: 0x06006B8B RID: 27531 RVA: 0x003C3C40 File Offset: 0x003C1E40
		public unsafe void SetCharacterExtraNeiliAllocationAndProgress(DataContext context, GameData.Domains.Character.Character character, IntList extraNeiliAllocationProgress, bool canOverMax = false)
		{
			int maxProgress = CombatSkillDomain.GetNeiliAllocationMaxProgress();
			NeiliAllocation extraNeiliAllocation = default(NeiliAllocation);
			extraNeiliAllocation.Initialize();
			for (int i = 0; i < 4; i++)
			{
				int progress = canOverMax ? extraNeiliAllocationProgress.Items[i] : Math.Clamp(extraNeiliAllocationProgress.Items[i], 0, maxProgress);
				*(ref extraNeiliAllocation.Items.FixedElementField + (IntPtr)i * 2) = (short)CombatSkillDomain.GetExtraNeiliAllocationByProgress(progress);
			}
			DomainManager.Extra.SetExtraNeiliAllocationProgress(context, character.GetId(), extraNeiliAllocationProgress);
			character.SetExtraNeiliAllocation(extraNeiliAllocation, context);
		}

		// Token: 0x06006B8C RID: 27532 RVA: 0x003C3CD4 File Offset: 0x003C1ED4
		public short AddExtraNeiliAllocationProgressToGainExtraNeiliAllocation(DataContext context, GameData.Domains.Character.Character character, byte neiliAllocationType, int deltaNeiliAllocation, bool allowOverMax = false)
		{
			int maxProgress = CombatSkillDomain.GetNeiliAllocationMaxProgress();
			if (allowOverMax)
			{
				maxProgress = int.MaxValue;
			}
			IntList progress = DomainManager.Extra.GetOrInitExtraNeiliAllocationProgress(context, character.GetId());
			int milestonesCount = CombatSkillDomain.GetNeiliAllocationProgressMilestoneCount(progress.Items[(int)neiliAllocationType], maxProgress);
			int targetProgressIndex = Math.Min(milestonesCount - 1, deltaNeiliAllocation - 1);
			int targetProgress = CombatSkillDomain.GetNeiliAllocationProgressMilestoneByIndex(progress.Items[(int)neiliAllocationType], maxProgress, targetProgressIndex);
			NeiliAllocation extraNeiliAllocation = character.GetExtraNeiliAllocation();
			progress.Items[(int)neiliAllocationType] = targetProgress;
			short deltaExtraNeiliAllocation = (short)(targetProgressIndex + 1);
			ref short ptr = ref extraNeiliAllocation.Items.FixedElementField + (IntPtr)neiliAllocationType * 2;
			ptr += deltaExtraNeiliAllocation;
			character.SetExtraNeiliAllocation(extraNeiliAllocation, context);
			DomainManager.Extra.SetExtraNeiliAllocationProgress(context, character.GetId(), progress);
			return deltaExtraNeiliAllocation;
		}

		// Token: 0x06006B8D RID: 27533 RVA: 0x003C3D98 File Offset: 0x003C1F98
		public sbyte GetSkillDirection(int charId, short skillId)
		{
			return this.GetElement_CombatSkills(new CombatSkillKey(charId, skillId)).GetDirection();
		}

		// Token: 0x06006B8E RID: 27534 RVA: 0x003C3DBC File Offset: 0x003C1FBC
		public sbyte GetSkillType(int charId, short skillId)
		{
			sbyte type = CombatSkill.Instance[skillId].Type;
			return (sbyte)DomainManager.SpecialEffect.ModifyData(charId, skillId, 221, (int)type, -1, -1, -1);
		}

		// Token: 0x06006B8F RID: 27535 RVA: 0x003C3DF8 File Offset: 0x003C1FF8
		[return: TupleElementNames(new string[]
		{
			"benefit",
			"qiDisorder"
		})]
		private static ValueTuple<int, short> CalcNeigongLoopingEffect_GetBenefitAndQiDisorder(byte neiliElementType, int charId, CombatSkillItem skillConfig)
		{
			bool flag = neiliElementType == 5;
			ValueTuple<int, short> result;
			if (flag)
			{
				result = new ValueTuple<int, short>(100, 0);
			}
			else
			{
				bool flag2 = CombatSkillDomain.FiveElementEquals(charId, skillConfig, FiveElementsType.Producing[(int)neiliElementType]);
				if (flag2)
				{
					result = new ValueTuple<int, short>(200, (short)(skillConfig.Grade * -125));
				}
				else
				{
					bool flag3 = CombatSkillDomain.FiveElementEquals(charId, skillConfig, FiveElementsType.Countering[(int)neiliElementType]);
					if (flag3)
					{
						result = new ValueTuple<int, short>(50, (short)(skillConfig.Grade * 125));
					}
					else
					{
						result = new ValueTuple<int, short>(100, 0);
					}
				}
			}
			return result;
		}

		// Token: 0x06006B90 RID: 27536 RVA: 0x003C3E74 File Offset: 0x003C2074
		public CombatSkill CreateCombatSkill(int charId, short skillTemplateId, ushort readingState = 0)
		{
			CombatSkill skill = new CombatSkill(charId, skillTemplateId, readingState);
			this.AddElement_CombatSkills(skill.GetId(), skill);
			return skill;
		}

		// Token: 0x06006B91 RID: 27537 RVA: 0x003C3EA0 File Offset: 0x003C20A0
		public void RegisterCombatSkills(int charId, List<CombatSkill> combatSkills)
		{
			int i = 0;
			int count = combatSkills.Count;
			while (i < count)
			{
				CombatSkill skill = combatSkills[i];
				skill.OfflineSetCharId(charId);
				this.AddElement_CombatSkills(skill.GetId(), skill);
				i++;
			}
		}

		// Token: 0x06006B92 RID: 27538 RVA: 0x003C3EE5 File Offset: 0x003C20E5
		public void RemoveCombatSkill(int charId, short skillTemplateId)
		{
			this.RemoveElement_CombatSkills(new CombatSkillKey(charId, skillTemplateId));
		}

		// Token: 0x06006B93 RID: 27539 RVA: 0x003C3EF8 File Offset: 0x003C20F8
		public void RemoveAllCombatSkills(int charId)
		{
			Dictionary<short, CombatSkill> charCombatSkills = this.GetCharCombatSkills(charId);
			List<short> skillTemplateIdList = ObjectPool<List<short>>.Instance.Get();
			skillTemplateIdList.Clear();
			skillTemplateIdList.AddRange(charCombatSkills.Keys);
			for (int i = 0; i < skillTemplateIdList.Count; i++)
			{
				this.RemoveElement_CombatSkills(new CombatSkillKey(charId, skillTemplateIdList[i]));
			}
			this._combatSkills.RemoveCharStub(charId);
			ObjectPool<List<short>>.Instance.Return(skillTemplateIdList);
		}

		// Token: 0x06006B94 RID: 27540 RVA: 0x003C3F70 File Offset: 0x003C2170
		public override void PackCrossArchiveGameData(CrossArchiveGameData crossArchiveGameData)
		{
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			Dictionary<short, CombatSkill> combatSkills = this.GetCharCombatSkills(taiwuCharId);
			crossArchiveGameData.CombatSkills = new List<CombatSkill>();
			crossArchiveGameData.CombatSkills.AddRange(combatSkills.Values);
		}

		// Token: 0x06006B95 RID: 27541 RVA: 0x003C3FB0 File Offset: 0x003C21B0
		public void UnpackCrossArchiveGameData_CombatSkills(DataContext context, CrossArchiveGameData crossArchiveGameData, bool overwriteEquipments)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			Dictionary<short, CombatSkill> combatSkills = DomainManager.CombatSkill.GetCharCombatSkills(taiwuCharId);
			List<short> learnedCombatSkills = taiwu.GetLearnedCombatSkills();
			List<CombatSkill> dreamBackCombatSkills = crossArchiveGameData.CombatSkills;
			List<CombatSkill> conflictCombatSkills = null;
			foreach (CombatSkill dreamBackCombatSkill in dreamBackCombatSkills)
			{
				short templateId = dreamBackCombatSkill.GetId().SkillTemplateId;
				CombatSkill combatSkill;
				bool flag = combatSkills.TryGetValue(templateId, out combatSkill);
				if (flag)
				{
					ushort readingState = combatSkill.GetReadingState() | dreamBackCombatSkill.GetReadingState();
					short obtainedNeili = Math.Max(combatSkill.GetObtainedNeili(), dreamBackCombatSkill.GetObtainedNeili());
					bool revoked = combatSkill.GetRevoked();
					bool dreamBackRevoked = dreamBackCombatSkill.GetRevoked();
					combatSkill.SetReadingState(readingState, context);
					combatSkill.SetObtainedNeili(obtainedNeili, context);
					combatSkill.SetRevoked(revoked && dreamBackRevoked, context);
					ushort activationState = combatSkill.GetActivationState();
					ushort dreamBackActivationState = dreamBackCombatSkill.GetActivationState();
					bool flag2 = activationState != 0 && dreamBackActivationState > 0;
					if (flag2)
					{
						if (conflictCombatSkills == null)
						{
							conflictCombatSkills = new List<CombatSkill>();
						}
						conflictCombatSkills.Add(dreamBackCombatSkill);
					}
					else
					{
						bool flag3 = dreamBackActivationState > 0;
						if (flag3)
						{
							combatSkill.SetActivationState(dreamBackActivationState, context);
							combatSkill.SetBreakoutStepsCount(dreamBackCombatSkill.GetBreakoutStepsCount(), context);
							combatSkill.SetForcedBreakoutStepsCount(dreamBackCombatSkill.GetForcedBreakoutStepsCount(), context);
						}
					}
					if (overwriteEquipments)
					{
						combatSkill.SetInnerRatio(dreamBackCombatSkill.GetInnerRatio(), context);
					}
				}
				else
				{
					dreamBackCombatSkill.OfflineSetCharId(taiwuCharId);
					learnedCombatSkills.Add(templateId);
					this.AddElement_CombatSkills(dreamBackCombatSkill.GetId(), dreamBackCombatSkill);
				}
			}
			taiwu.SetLearnedCombatSkills(learnedCombatSkills, context);
			crossArchiveGameData.CombatSkills = conflictCombatSkills;
		}

		// Token: 0x06006B96 RID: 27542 RVA: 0x003C4188 File Offset: 0x003C2388
		[DomainMethod]
		public CombatSkillDisplayData GetCombatSkillDisplayDataOnce(int charId, short skillTemplateId)
		{
			GameData.Domains.Character.Character character = null;
			bool flag = charId >= 0 && !DomainManager.Character.TryGetElement_Objects(charId, out character);
			if (flag)
			{
				charId = -1;
			}
			bool flag2 = charId < 0;
			if (flag2)
			{
				charId = DomainManager.Taiwu.GetTaiwuCharId();
				character = DomainManager.Taiwu.GetTaiwu();
			}
			return this.CalcCombatSkillDisplayData(skillTemplateId, charId, character);
		}

		// Token: 0x06006B97 RID: 27543 RVA: 0x003C41E4 File Offset: 0x003C23E4
		[DomainMethod]
		public List<CombatSkillDisplayData> GetCombatSkillDisplayData(int charId, List<short> skillTemplateIdList)
		{
			return this.CalcCombatSkillDisplayDataList(charId, skillTemplateIdList);
		}

		// Token: 0x06006B98 RID: 27544 RVA: 0x003C4200 File Offset: 0x003C2400
		[DomainMethod]
		public List<CombatSkillDisplayData> GetCharacterEquipCombatSkillDisplayData(int charId)
		{
			GameData.Domains.Character.Character character;
			bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
			List<CombatSkillDisplayData> result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = this.CalcCombatSkillDisplayDataList(charId, character.GetCombatSkillEquipment());
			}
			return result;
		}

		// Token: 0x06006B99 RID: 27545 RVA: 0x003C4238 File Offset: 0x003C2438
		[DomainMethod]
		public List<CombatSkillEffectDescriptionDisplayData> GetEffectDescriptionData(int charId, List<short> skillIds)
		{
			List<CombatSkillEffectDescriptionDisplayData> result = new List<CombatSkillEffectDescriptionDisplayData>();
			foreach (short skillId in skillIds)
			{
				CombatSkillKey key = new CombatSkillKey(charId, skillId);
				CombatSkill skill;
				bool flag = this.TryGetElement_CombatSkills(key, out skill);
				if (flag)
				{
					result.Add(this.GetEffectDisplayData(skill));
				}
				else
				{
					result.Add(CombatSkillEffectDescriptionDisplayData.Invalid);
					short predefinedLogId = 13;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
					defaultInterpolatedStringHandler.AppendLiteral("GetEffectDescriptionData no exist key ");
					defaultInterpolatedStringHandler.AppendFormatted<CombatSkillKey>(key);
					PredefinedLog.Show(predefinedLogId, defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			return result;
		}

		// Token: 0x06006B9A RID: 27546 RVA: 0x003C42F8 File Offset: 0x003C24F8
		[DomainMethod]
		public sbyte GetCombatSkillBreakStepCount(int charId, short skillTemplateId)
		{
			return DomainManager.Character.GetElement_Objects(charId).GetSkillBreakoutAvailableStepsCount(skillTemplateId);
		}

		// Token: 0x06006B9B RID: 27547 RVA: 0x003C431C File Offset: 0x003C251C
		[DomainMethod]
		public int GetCombatSkillBreakoutStepsMaxPower(int charId, short skillTemplateId)
		{
			GameData.Domains.Character.Character character;
			return DomainManager.Character.TryGetElement_Objects(charId, out character) ? character.GetSkillBreakoutStepsMaxPower(skillTemplateId) : 0;
		}

		// Token: 0x06006B9C RID: 27548 RVA: 0x003C4348 File Offset: 0x003C2548
		private CombatSkillDisplayData CalcCombatSkillDisplayData(short skillTemplateId, int charId, GameData.Domains.Character.Character character)
		{
			bool flag = character == null;
			CombatSkillDisplayData result;
			if (flag)
			{
				short predefinedLogId = 12;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 2);
				defaultInterpolatedStringHandler.AppendLiteral("CalcCombatSkillDisplayData ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(skillTemplateId);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
				PredefinedLog.Show(predefinedLogId, defaultInterpolatedStringHandler.ToStringAndClear());
				result = null;
			}
			else
			{
				CombatSkillItem configData = CombatSkill.Instance[skillTemplateId];
				CombatCharacter combatChar = null;
				bool inCombat = DomainManager.Combat.IsInCombat() && DomainManager.Combat.TryGetElement_CombatCharacterDict(charId, out combatChar);
				CombatSkillKey skillKey = new CombatSkillKey(charId, skillTemplateId);
				bool skillExist = this._combatSkills.ContainsKey(skillKey);
				CombatSkill skill = skillExist ? this._combatSkills[skillKey] : new CombatSkill(charId, skillTemplateId, 0);
				bool isTaiwu = charId == DomainManager.Taiwu.GetTaiwuCharId();
				CombatSkillDisplayData data = new CombatSkillDisplayData();
				data.CharId = charId;
				data.TemplateId = skillTemplateId;
				data.ReadingState = skill.GetReadingState();
				data.ActivationState = skill.GetActivationState();
				data.CanAffect = character.GetCombatSkillCanAffect(skillTemplateId);
				data.Conflicting = (DomainManager.Extra.GetConflictCombatSkill(skillTemplateId) != null);
				data.GridCount = character.GetCombatSkillGridCost(skillTemplateId);
				data.Power = (skillExist ? skill.GetPower() : 100);
				data.MaxPower = (skillExist ? skill.GetMaxPower() : GlobalConfig.Instance.CombatSkillMaxBasePower);
				data.RequirementsPower = (skillExist ? skill.GetRequirementsPower() : -1);
				data.Requirements = skill.GetRequirementsAndActualValues(character, skillExist);
				data.BreakAddProperty = skill.GetBreakAddPropertyList((int)data.Power);
				data.NeiliAllocationAddProperty = skill.GetNeiliAllocationPropertyList((int)data.Power);
				data.BreakPlateIndex = (isTaiwu ? DomainManager.Extra.GetCombatSkillCurrBreakPlateIndex(skillTemplateId) : 0);
				data.EffectType = (skillExist ? skill.GetDirection() : -1);
				CombatSkillDisplayData combatSkillDisplayData = data;
				List<short> items = DomainManager.Extra.GetCharacterMasteredCombatSkills(charId).Items;
				combatSkillDisplayData.Mastered = (items != null && items.Contains(skillTemplateId));
				data.Revoked = skill.GetRevoked();
				data.JumpThreshold = (isTaiwu ? DomainManager.Extra.GetJumpThreshold(skillTemplateId) : -1);
				data.BaseInnerRatio = (skillExist ? skill.GetBaseInnerRatio() : configData.BaseInnerRatio);
				data.InnerRatioChangeRange = (skillExist ? skill.GetInnerRatioChangeRange() : configData.InnerRatioChangeRange);
				data.CurrInnerRatio = (skillExist ? skill.GetCurrInnerRatio() : skill.GetInnerRatio());
				data.ExpectInnerRatio = skill.GetInnerRatio();
				data.NewUnderstandingNeedExp = (skillExist ? this.GetNewUnderstandingNeedExp(charId, skillTemplateId) : -1);
				data.BreakSuccess = this.GetBreakSuccess(charId, skillTemplateId);
				data.EffectDescription = (skillExist ? this.GetEffectDisplayData(skill) : CombatSkillEffectDescriptionDisplayData.Invalid);
				data.DamageStepBonus = (skillExist ? skill.CalcStepBonusDisplayData() : default(CombatSkillDamageStepBonusDisplayData));
				bool flag2 = skillExist;
				if (flag2)
				{
					IEnumerable<SkillBreakPlateBonus> breakBonues = skill.GetBreakBonuses();
					data.BreakBonusGrades = new List<sbyte>();
					foreach (SkillBreakPlateBonus breakBonus in breakBonues)
					{
						data.BreakBonusGrades.Add(breakBonus.Grade);
					}
				}
				bool flag3 = !skillExist;
				if (flag3)
				{
					for (int i = 0; i < data.Requirements.Count; i++)
					{
						ValueTuple<int, int, int> requirement = data.Requirements[i];
						requirement.Item3 = -1;
						data.Requirements[i] = requirement;
					}
				}
				switch (configData.EquipType)
				{
				case 0:
					CombatSkillDomain.CalcNeigongSkillDisplayData(data, skillExist, skill, configData, false);
					break;
				case 1:
					this.CalcAttackSkillDisplayData(skillTemplateId, charId, data, skillExist, configData, skill);
					break;
				case 2:
					this.CalcAgileSkillDisplayData(data, skill, skillExist, configData);
					break;
				case 3:
					CombatSkillDomain.CalcDefenseSkillDisplayData(data, skillExist, skill, configData);
					break;
				}
				bool flag4 = CombatSkillHelper.IsProactiveSkill(configData.EquipType);
				if (flag4)
				{
					this.CalcProactiveSkillDisplayData(data, skill, skillExist, configData, inCombat, combatChar);
				}
				bool flag5 = inCombat;
				if (flag5)
				{
					this.CalcInCombatSkillDisplayData(data);
				}
				this.CalcSkillDisplayDataFiveElement(data, charId, character);
				result = data;
			}
			return result;
		}

		// Token: 0x06006B9D RID: 27549 RVA: 0x003C47B4 File Offset: 0x003C29B4
		private List<CombatSkillDisplayData> CalcCombatSkillDisplayDataList(int charId, IEnumerable<short> skillTemplateIdList)
		{
			List<CombatSkillDisplayData> dataList = new List<CombatSkillDisplayData>();
			bool flag = skillTemplateIdList == null;
			List<CombatSkillDisplayData> result;
			if (flag)
			{
				result = dataList;
			}
			else
			{
				GameData.Domains.Character.Character character = null;
				bool flag2 = charId >= 0 && !DomainManager.Character.TryGetElement_Objects(charId, out character);
				if (flag2)
				{
					charId = -1;
				}
				bool flag3 = charId < 0;
				if (flag3)
				{
					charId = DomainManager.Taiwu.GetTaiwuCharId();
					character = DomainManager.Taiwu.GetTaiwu();
				}
				foreach (short skillTemplateId in skillTemplateIdList)
				{
					bool flag4 = skillTemplateId >= 0;
					if (flag4)
					{
						dataList.Add(this.CalcCombatSkillDisplayData(skillTemplateId, charId, character));
					}
				}
				result = dataList;
			}
			return result;
		}

		// Token: 0x06006B9E RID: 27550 RVA: 0x003C487C File Offset: 0x003C2A7C
		private static void CalcNeigongSkillDisplayData(CombatSkillDisplayData data, bool skillExist, CombatSkill skill, CombatSkillItem configData, bool preview = false)
		{
			if (skillExist)
			{
				data.MaxObtainableNeili = skill.GetTotalObtainableNeili();
				data.ObtainedNeili = skill.GetObtainedNeili();
				data.SpecificGrids = skill.GetSpecificGridCount(preview);
				data.GenericGrid = skill.GetGenericGridCount(preview);
			}
			else
			{
				data.MaxObtainableNeili = configData.TotalObtainableNeili;
				data.ObtainedNeili = 0;
				data.SpecificGrids = configData.SpecificGrids;
				data.GenericGrid = configData.GenericGrid;
			}
		}

		// Token: 0x06006B9F RID: 27551 RVA: 0x003C48F4 File Offset: 0x003C2AF4
		private unsafe void CalcAttackSkillDisplayData(short skillTemplateId, int charId, CombatSkillDisplayData data, bool skillExist, CombatSkillItem configData, CombatSkill skill)
		{
			if (skillExist)
			{
				data.AddAttackDistanceForward = this.GetCombatSkillAddAttackDistance(charId, skillTemplateId, true);
				data.AddAttackDistanceBackward = this.GetCombatSkillAddAttackDistance(charId, skillTemplateId, false);
				data.Poisons = skill.GetPoisons();
				HitOrAvoidInts hitValue = skill.GetHitValue();
				OuterAndInnerInts penetrations = skill.GetPenetrations();
				TaiwuCombatSkill taiwuSkill;
				sbyte fullPowerCastTimes = (charId == DomainManager.Taiwu.GetTaiwuCharId() && DomainManager.Taiwu.TryGetElement_CombatSkills(skillTemplateId, out taiwuSkill)) ? taiwuSkill.FullPowerCastTimes : 0;
				data.HitValueStrength = hitValue.Items.FixedElementField;
				data.HitValueTechnique = *(ref hitValue.Items.FixedElementField + 4);
				data.HitValueSpeed = *(ref hitValue.Items.FixedElementField + (IntPtr)2 * 4);
				data.HitValueMind = *(ref hitValue.Items.FixedElementField + (IntPtr)3 * 4);
				data.PenetrateValueInner = penetrations.Inner;
				data.PenetrateValueOuter = penetrations.Outer;
				data.HitDistribution = skill.GetHitDistribution();
				data.BodyPartWeights = new List<int>(skill.GetBodyPartWeights());
				data.FullPowerCastTimes = fullPowerCastTimes;
			}
			else
			{
				data.AddAttackDistanceForward = configData.DistanceAdditionWhenCast;
				data.AddAttackDistanceBackward = configData.DistanceAdditionWhenCast;
				data.Poisons = configData.Poisons;
				sbyte[] distribution = configData.PerHitDamageRateDistribution;
				int totalHit = (int)configData.TotalHit;
				data.HitValueStrength = (data.HitValueTechnique = (data.HitValueSpeed = (data.HitValueMind = 0)));
				bool flag = CombatSkillTemplateHelper.IsMindHitSkill(skillTemplateId);
				if (flag)
				{
					data.HitValueMind = totalHit;
				}
				else
				{
					data.HitValueStrength = totalHit * (int)distribution[0] / 100;
					data.HitValueStrength = totalHit * (int)distribution[1] / 100;
					data.HitValueStrength = totalHit * (int)distribution[2] / 100;
				}
				short totalPenetrate = configData.Penetrate;
				data.PenetrateValueInner = (int)(totalPenetrate * (short)data.CurrInnerRatio / 100);
				data.PenetrateValueOuter = (int)totalPenetrate - data.PenetrateValueInner;
				data.HitDistribution.Items.FixedElementField = (int)distribution[0];
				*(ref data.HitDistribution.Items.FixedElementField + 4) = (int)distribution[1];
				*(ref data.HitDistribution.Items.FixedElementField + (IntPtr)2 * 4) = (int)distribution[2];
				*(ref data.HitDistribution.Items.FixedElementField + (IntPtr)3 * 4) = (int)distribution[3];
				data.BodyPartWeights = new List<int>(from x in configData.InjuryPartAtkRateDistribution
				select (int)x);
				data.FullPowerCastTimes = 0;
			}
		}

		// Token: 0x06006BA0 RID: 27552 RVA: 0x003C4B80 File Offset: 0x003C2D80
		private unsafe void CalcAgileSkillDisplayData(CombatSkillDisplayData data, CombatSkill skill, bool skillExist, CombatSkillItem configData)
		{
			data.JumpSpeed = CombatSkillDomain.CalcJumpSpeed(data.CharId, data.TemplateId);
			data.AddMoveSpeed = CombatSkillDomain.CalcCastAddMoveSpeed(skill, (int)data.Power);
			data.AddPercentMoveSpeed = CombatSkillDomain.CalcCastAddPercentMoveSpeed(skill, (int)data.Power);
			if (skillExist)
			{
				HitOrAvoidInts addHit = CombatSkillDomain.CalcAddHitValueOnCast(skill, (int)data.Power);
				data.AddHitStrength = addHit.Items.FixedElementField;
				data.AddHitTechnique = *(ref addHit.Items.FixedElementField + 4);
				data.AddHitSpeed = *(ref addHit.Items.FixedElementField + (IntPtr)2 * 4);
				data.AddHitMind = *(ref addHit.Items.FixedElementField + (IntPtr)3 * 4);
			}
			else
			{
				data.AddAvoidStrength = (int)configData.AddHitOnCast[0];
				data.AddAvoidTechnique = (int)configData.AddHitOnCast[1];
				data.AddAvoidSpeed = (int)configData.AddHitOnCast[2];
				data.AddAvoidMind = (int)configData.AddHitOnCast[3];
			}
		}

		// Token: 0x06006BA1 RID: 27553 RVA: 0x003C4C84 File Offset: 0x003C2E84
		private unsafe static void CalcDefenseSkillDisplayData(CombatSkillDisplayData data, bool skillExist, CombatSkill skill, CombatSkillItem configData)
		{
			if (skillExist)
			{
				HitOrAvoidInts addAvoid = CombatSkillDomain.CalcAddAvoidValueOnCast(skill, (int)data.Power);
				OuterAndInnerInts addPenetrateResist = CombatSkillDomain.CalcAddPenetrateResist(skill, (int)data.Power);
				OuterAndInnerInts bouncePower = CombatSkillDomain.CalcBouncePower(skill, (int)data.Power);
				data.EffectDuration = CombatSkillDomain.CalcContinuousFrames(skill);
				data.AddOuterDef = addPenetrateResist.Outer;
				data.AddInnerDef = addPenetrateResist.Inner;
				data.AddAvoidStrength = addAvoid.Items.FixedElementField;
				data.AddAvoidTechnique = *(ref addAvoid.Items.FixedElementField + 4);
				data.AddAvoidSpeed = *(ref addAvoid.Items.FixedElementField + (IntPtr)2 * 4);
				data.AddAvoidMind = *(ref addAvoid.Items.FixedElementField + (IntPtr)3 * 4);
				data.FightbackPower = CombatSkillDomain.CalcFightBackPower(skill, (int)data.Power);
				data.BouncePowerOuter = bouncePower.Outer;
				data.BouncePowerInner = bouncePower.Inner;
				data.BounceDistance = skill.GetBounceDistance();
			}
			else
			{
				data.EffectDuration = configData.ContinuousFrames;
				data.AddOuterDef = (int)configData.AddOuterPenetrateResistOnCast;
				data.AddInnerDef = (int)configData.AddInnerPenetrateResistOnCast;
				data.AddAvoidStrength = (int)configData.AddAvoidOnCast[0];
				data.AddAvoidTechnique = (int)configData.AddAvoidOnCast[1];
				data.AddAvoidSpeed = (int)configData.AddAvoidOnCast[2];
				data.AddAvoidMind = (int)configData.AddAvoidOnCast[3];
				data.FightbackPower = (int)configData.FightBackDamage;
				data.BouncePowerOuter = (int)configData.BounceRateOfOuterInjury;
				data.BouncePowerInner = (int)configData.BounceRateOfInnerInjury;
				data.BounceDistance = configData.BounceDistance;
			}
		}

		// Token: 0x06006BA2 RID: 27554 RVA: 0x003C4E1C File Offset: 0x003C301C
		private unsafe void CalcProactiveSkillDisplayData(CombatSkillDisplayData data, CombatSkill skill, bool skillExist, CombatSkillItem configData, bool inCombat, CombatCharacter combatChar)
		{
			data.CostMobility = (skillExist ? ((short)skill.GetCostMobilityPercent()) : configData.MobilityCost);
			data.CostNeiliAllocation = (skillExist ? skill.GetCostNeiliAllocation() : new ValueTuple<sbyte, sbyte>(-1, 0));
			data.CostTricks = new List<NeedTrick>();
			this.GetCombatSkillCostTrick(skill, data.CostTricks, true);
			if (inCombat)
			{
				int costMobility = MoveSpecialConstants.MaxMobility * (int)data.CostMobility / 100;
				NeiliAllocation neiliAllocations = combatChar.GetNeiliAllocation();
				TrickCollection tricks = combatChar.GetTricks();
				if (skillExist)
				{
					int num;
					int num2;
					DomainManager.Combat.GetSkillCostBreathStance(combatChar.GetId(), skill).Deconstruct(out num, out num2);
					int stance = num;
					int breath = num2;
					sbyte costStance = (sbyte)stance;
					sbyte costBreath = (sbyte)breath;
					data.CostStance = costStance;
					data.CostBreath = costBreath;
					data.CostBreathFontType = CombatSkillDomain.<CalcProactiveSkillDisplayData>g__Convert|63_0(combatChar.GetBreathValue() >= 30000 * (int)data.CostBreath / 100);
					data.CostStanceFontType = CombatSkillDomain.<CalcProactiveSkillDisplayData>g__Convert|63_0(combatChar.GetStanceValue() >= 4000 * (int)data.CostStance / 100);
				}
				else
				{
					short predefinedLogId = 13;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Skill not exist in combat ");
					defaultInterpolatedStringHandler.AppendFormatted<CombatCharacter>(combatChar);
					defaultInterpolatedStringHandler.AppendLiteral(" ");
					defaultInterpolatedStringHandler.AppendFormatted(configData.Name);
					PredefinedLog.Show(predefinedLogId, defaultInterpolatedStringHandler.ToStringAndClear());
					data.CostBreath = (data.CostStance = 50);
					data.CostBreathFontType = (data.CostStanceFontType = CombatSkillDomain.<CalcProactiveSkillDisplayData>g__Convert|63_0(false));
				}
				data.CostMobilityFontType = CombatSkillDomain.<CalcProactiveSkillDisplayData>g__Convert|63_0(combatChar.GetMobilityValue() >= costMobility);
				data.CostNeiliAllocationFontType = CombatSkillDomain.<CalcProactiveSkillDisplayData>g__Convert|63_0(*(ref neiliAllocations.Items.FixedElementField + (IntPtr)data.CostNeiliAllocation.Item1 * 2) >= (short)data.CostNeiliAllocation.Item2);
				data.CostWeaponDurabilityFontType = CombatSkillDomain.<CalcProactiveSkillDisplayData>g__Convert|63_0(combatChar.GetUsingWeaponIndex() >= 3 || DomainManager.Item.GetElement_Weapons(DomainManager.Combat.GetUsingWeaponKey(combatChar).Id).GetCurrDurability() >= (short)configData.WeaponDurableCost);
				data.CostWugFontType = CombatSkillDomain.<CalcProactiveSkillDisplayData>g__Convert|63_0(combatChar.GetWugCount() >= (short)configData.WugCost);
				data.CostTricksFontType = new List<sbyte>();
				for (int i = 0; i < data.CostTricks.Count; i++)
				{
					NeedTrick needTrick = data.CostTricks[i];
					int counter = 0;
					foreach (sbyte trickType in tricks.Tricks.Values)
					{
						bool flag = combatChar.TrickEquals(trickType, needTrick.TrickType);
						if (flag)
						{
							counter++;
						}
					}
					data.CostTricksFontType.Add(CombatSkillDomain.<CalcProactiveSkillDisplayData>g__Convert|63_0(counter >= (int)needTrick.NeedCount));
				}
			}
			else
			{
				data.CostBreath = (skillExist ? skill.GetCostBreathPercent() : (configData.BreathStanceTotalCost * data.CurrInnerRatio / 100));
				data.CostStance = (skillExist ? skill.GetCostStancePercent() : (configData.BreathStanceTotalCost - data.CostBreath));
				data.CostBreathFontType = (data.CostStanceFontType = (data.CostMobilityFontType = (data.CostNeiliAllocationFontType = (data.CostWeaponDurabilityFontType = (data.CostWugFontType = 0)))));
				data.CostTricksFontType = new List<sbyte>();
				for (int j = 0; j < data.CostTricks.Count; j++)
				{
					data.CostTricksFontType.Add(0);
				}
			}
		}

		// Token: 0x06006BA3 RID: 27555 RVA: 0x003C51F0 File Offset: 0x003C33F0
		private void CalcInCombatSkillDisplayData(CombatSkillDisplayData data)
		{
			CombatSkillData combatSkillData;
			bool flag = !DomainManager.Combat.TryGetCombatSkillData(data.CharId, data.TemplateId, out combatSkillData);
			if (!flag)
			{
				data.EffectData = combatSkillData.GetEffectData();
			}
		}

		// Token: 0x06006BA4 RID: 27556 RVA: 0x003C522C File Offset: 0x003C342C
		private void CalcSkillDisplayDataFiveElement(CombatSkillDisplayData data, int charId, GameData.Domains.Character.Character character)
		{
			bool isTaiwu = charId == DomainManager.Taiwu.GetTaiwuCharId();
			CombatSkillItem configData = CombatSkill.Instance[data.TemplateId];
			data.FiveElementDestTypeWhileLooping = configData.DestTypeWhileLooping;
			data.FiveElementTransferTypeWhileLooping = configData.TransferTypeWhileLooping;
			bool flag = !isTaiwu;
			if (!flag)
			{
				short loopingNeigong = character.GetLoopingNeigong();
				bool flag2 = loopingNeigong < 0;
				if (!flag2)
				{
					SByteList qiArtStrategyList;
					bool flag3 = !DomainManager.Extra.TryGetElement_QiArtStrategyMap(loopingNeigong, out qiArtStrategyList);
					if (!flag3)
					{
						bool flag4 = qiArtStrategyList.Items == null || qiArtStrategyList.Items.Count == 0;
						if (!flag4)
						{
							sbyte taiwuNeiliType = character.GetNeiliType();
							foreach (sbyte strategy in qiArtStrategyList.Items)
							{
								bool flag5 = strategy == -1;
								if (!flag5)
								{
									QiArtStrategyItem strategyConfig = QiArtStrategy.Instance[strategy];
									bool flag6 = strategyConfig.TransferToFiveElements > -1 && strategyConfig.FiveElementsTransferType > -1;
									if (flag6)
									{
										data.FiveElementDestTypeWhileLooping = strategyConfig.TransferToFiveElements;
										data.FiveElementTransferTypeWhileLooping = strategyConfig.FiveElementsTransferType;
									}
									short strategyAnchoredFiveElements = strategyConfig.AnchorFiveElements;
									NeiliTypeItem neiliTypeConfig = NeiliType.Instance[taiwuNeiliType];
									bool isConflict = neiliTypeConfig.ColorType == 2;
									bool flag7 = strategyAnchoredFiveElements != -1 && !isConflict && (short)neiliTypeConfig.FiveElements == strategyAnchoredFiveElements;
									if (flag7)
									{
										data.FiveElementDestTypeWhileLooping = strategyConfig.TransferToFiveElements;
										data.FiveElementTransferTypeWhileLooping = strategyConfig.FiveElementsTransferType;
										break;
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06006BA5 RID: 27557 RVA: 0x003C53DC File Offset: 0x003C35DC
		public CombatSkillEffectDescriptionDisplayData GetEffectDisplayData(CombatSkillKey skillKey)
		{
			CombatSkill skill;
			return this.TryGetElement_CombatSkills(skillKey, out skill) ? this.GetEffectDisplayData(skill) : CombatSkillEffectDescriptionDisplayData.Invalid;
		}

		// Token: 0x06006BA6 RID: 27558 RVA: 0x003C5408 File Offset: 0x003C3608
		public CombatSkillEffectDescriptionDisplayData GetEffectDisplayData(int charId, short skillTemplateId)
		{
			return this.GetEffectDisplayData(new CombatSkillKey(charId, skillTemplateId));
		}

		// Token: 0x06006BA7 RID: 27559 RVA: 0x003C5428 File Offset: 0x003C3628
		public CombatSkillEffectDescriptionDisplayData GetEffectDisplayData(CombatSkill skill)
		{
			CombatSkillItem config = CombatSkill.Instance[skill.GetId().SkillTemplateId];
			sbyte direction = skill.GetDirection();
			CombatSkillEffectDescriptionDisplayData result = default(CombatSkillEffectDescriptionDisplayData);
			bool flag = direction < 0 || direction >= 2;
			result.EffectId = (flag ? -1 : ((direction == 0) ? config.DirectEffectID : config.ReverseEffectID));
			result.AffectRequirePower = (skill.AnyAffectRequirePower() ? new List<int>(skill.GetAffectRequirePower()) : null);
			return result;
		}

		// Token: 0x06006BA8 RID: 27560 RVA: 0x003C54B0 File Offset: 0x003C36B0
		public short GetCombatSkillAddAttackDistance(int charId, short skillId, bool forward)
		{
			CombatSkillKey skillKey = new CombatSkillKey(charId, skillId);
			ushort fieldId = forward ? 145 : 146;
			int distance = DomainManager.SpecialEffect.GetModifyValue(charId, skillId, fieldId, EDataModifyType.Add, -1, -1, -1, EDataSumType.All);
			distance -= DomainManager.SpecialEffect.GetModifyValue(charId, -1, fieldId, EDataModifyType.Add, -1, -1, -1, EDataSumType.All);
			bool flag = skillId >= 0;
			if (flag)
			{
				CombatSkill skill;
				distance += (int)(this._combatSkills.TryGetValue(skillKey, out skill) ? skill.GetDistanceAdditionWhenCast(forward) : CombatSkill.Instance[skillId].DistanceAdditionWhenCast);
			}
			return (short)distance;
		}

		// Token: 0x06006BA9 RID: 27561 RVA: 0x003C5540 File Offset: 0x003C3740
		public void GetCombatSkillCostTrick(CombatSkill skill, List<NeedTrick> costTricks, bool applySpecialEffect = true)
		{
			CombatSkillKey skillKey = skill.GetId();
			costTricks.Clear();
			costTricks.AddRange(CombatSkill.Instance[skillKey.SkillTemplateId].TrickCost);
			List<int> removedTrickIndexes = ObjectPool<List<int>>.Instance.Get();
			removedTrickIndexes.Clear();
			for (int i = 0; i < costTricks.Count; i++)
			{
				bool isLast = removedTrickIndexes.Count == costTricks.Count - 1;
				short propertyId = (short)(53 + costTricks[i].TrickType);
				int index = i;
				NeedTrick value = costTricks[i];
				value.NeedCount = (byte)Math.Max(isLast ? 1 : 0, (int)costTricks[i].NeedCount + skill.GetBreakoutGridCombatSkillPropertyBonus(propertyId));
				costTricks[index] = value;
				bool flag = costTricks[i].NeedCount == 0;
				if (flag)
				{
					removedTrickIndexes.Add(i);
				}
			}
			for (int j = removedTrickIndexes.Count - 1; j >= 0; j--)
			{
				costTricks.RemoveAt(removedTrickIndexes[j]);
			}
			ObjectPool<List<int>>.Instance.Return(removedTrickIndexes);
			if (applySpecialEffect)
			{
				DomainManager.SpecialEffect.ModifyData(skillKey.CharId, skillKey.SkillTemplateId, 208, costTricks, -1, -1, -1);
			}
		}

		// Token: 0x06006BAA RID: 27562 RVA: 0x003C5688 File Offset: 0x003C3888
		public sbyte GetCombatSkillGridCost(int charId, short skillTemplateId)
		{
			GameData.Domains.Character.Character character;
			bool flag = DomainManager.Character.TryGetElement_Objects(charId, out character);
			sbyte result;
			if (flag)
			{
				result = character.GetCombatSkillGridCost(skillTemplateId);
			}
			else
			{
				result = CombatSkill.Instance[skillTemplateId].GridCost;
			}
			return result;
		}

		// Token: 0x06006BAB RID: 27563 RVA: 0x003C56C8 File Offset: 0x003C38C8
		public static int CalcJumpSpeed(int charId, short skillTemplateId)
		{
			GameData.Domains.Character.Character character;
			int moveSpeed = (int)(DomainManager.Character.TryGetElement_Objects(charId, out character) ? character.GetMoveSpeed() : 100);
			int jumpSpeed = CFormula.CalcJumpSpeed(moveSpeed);
			CValuePercentBonus percent = DomainManager.SpecialEffect.GetModifyValue(charId, skillTemplateId, 152, EDataModifyType.AddPercent, -1, -1, -1, EDataSumType.All);
			CombatSkillKey skillKey = new CombatSkillKey(charId, skillTemplateId);
			CombatSkill skill;
			bool flag = DomainManager.CombatSkill.TryGetElement_CombatSkills(skillKey, out skill);
			if (flag)
			{
				percent += skill.GetBreakoutGridCombatSkillPropertyBonus(69);
			}
			return jumpSpeed * percent;
		}

		// Token: 0x06006BAC RID: 27564 RVA: 0x003C5754 File Offset: 0x003C3954
		public static short CalcCastAddMoveSpeed(CombatSkill skill, CValuePercent power)
		{
			CombatSkillItem configData = CombatSkill.Instance[skill.GetId().SkillTemplateId];
			CValuePercent baseValue = (int)GlobalConfig.Instance.AgileSkillBaseAddSpeed;
			CValuePercentBonus gridBonus = skill.GetBreakoutGridCombatSkillPropertyBonus(11);
			return (short)((int)configData.AddMoveSpeedOnCast * baseValue * gridBonus * power);
		}

		// Token: 0x06006BAD RID: 27565 RVA: 0x003C57B4 File Offset: 0x003C39B4
		public static short CalcCastAddPercentMoveSpeed(CombatSkill skill, CValuePercent power)
		{
			CombatSkillItem configData = CombatSkill.Instance[skill.GetId().SkillTemplateId];
			return (short)((int)configData.AddPercentMoveSpeedOnCast * power);
		}

		// Token: 0x06006BAE RID: 27566 RVA: 0x003C57EC File Offset: 0x003C39EC
		public static HitOrAvoidInts CalcAddHitValueOnCast(CombatSkill skill, CValuePercent power)
		{
			CombatSkillItem configData = CombatSkill.Instance[skill.GetId().SkillTemplateId];
			CValuePercent baseValue = (int)GlobalConfig.Instance.AgileSkillBaseAddHit;
			HitOrAvoidInts addHit = default(HitOrAvoidInts);
			int globalGridBonus = 0;
			foreach (SkillBreakPlateBonus bonus in skill.GetBreakBonuses())
			{
				globalGridBonus += bonus.CalcAddHitOnCast();
			}
			for (int i = 0; i < 4; i++)
			{
				CValuePercentBonus gridBonus = skill.GetBreakoutGridCombatSkillPropertyBonus((short)(12 + i));
				gridBonus += globalGridBonus;
				addHit[i] = (int)configData.AddHitOnCast[i] * baseValue * gridBonus * power;
			}
			return addHit;
		}

		// Token: 0x06006BAF RID: 27567 RVA: 0x003C58DC File Offset: 0x003C3ADC
		public static OuterAndInnerInts CalcAddPenetrateResist(CombatSkill skill, CValuePercent power)
		{
			OuterAndInnerInts addPenetrate = default(OuterAndInnerInts);
			CombatSkillItem configData = CombatSkill.Instance[skill.GetId().SkillTemplateId];
			CValuePercent baseValue = (int)GlobalConfig.Instance.DefendSkillBaseAddPenetrateResist;
			CValuePercentBonus outerBonus = skill.GetBreakoutGridCombatSkillPropertyBonus(18);
			CValuePercentBonus innerBonus = skill.GetBreakoutGridCombatSkillPropertyBonus(19);
			foreach (SkillBreakPlateBonus bonus in skill.GetBreakBonuses())
			{
				outerBonus += bonus.CalcAddPenetrateResist();
				innerBonus += bonus.CalcAddPenetrateResist();
			}
			addPenetrate.Outer = (int)configData.AddOuterPenetrateResistOnCast * baseValue * outerBonus * power;
			addPenetrate.Inner = (int)configData.AddInnerPenetrateResistOnCast * baseValue * innerBonus * power;
			return addPenetrate;
		}

		// Token: 0x06006BB0 RID: 27568 RVA: 0x003C59E8 File Offset: 0x003C3BE8
		public static short CalcContinuousFrames(CombatSkill skill)
		{
			return (short)DomainManager.SpecialEffect.ModifyValue(skill.GetId().CharId, skill.GetId().SkillTemplateId, 176, (int)skill.GetContinuousFrames(), -1, -1, -1, 0, 0, 0, 0);
		}

		// Token: 0x06006BB1 RID: 27569 RVA: 0x003C5A28 File Offset: 0x003C3C28
		public static HitOrAvoidInts CalcAddAvoidValueOnCast(CombatSkill skill, CValuePercent power)
		{
			CombatSkillItem configData = CombatSkill.Instance[skill.GetId().SkillTemplateId];
			CValuePercent baseValue = (int)GlobalConfig.Instance.DefendSkillBaseAddAvoid;
			HitOrAvoidInts addAvoid = default(HitOrAvoidInts);
			int globalGridBonus = 0;
			foreach (SkillBreakPlateBonus bonus in skill.GetBreakBonuses())
			{
				globalGridBonus += bonus.CalcAddAvoidValueOnCast();
			}
			for (int i = 0; i < 4; i++)
			{
				CValuePercentBonus bonus2 = skill.GetBreakoutGridCombatSkillPropertyBonus((short)(20 + i));
				bonus2 += globalGridBonus;
				addAvoid[i] = (int)configData.AddAvoidOnCast[i] * baseValue * bonus2 * power;
			}
			return addAvoid;
		}

		// Token: 0x06006BB2 RID: 27570 RVA: 0x003C5B18 File Offset: 0x003C3D18
		public static int CalcFightBackPower(CombatSkill skill, CValuePercent power)
		{
			CombatSkillItem configData = CombatSkill.Instance[skill.GetId().SkillTemplateId];
			CValuePercent basePower = (int)GlobalConfig.Instance.DefendSkillBaseFightBackPower;
			CValuePercentBonus gridBonus = skill.GetBreakoutGridCombatSkillPropertyBonus(24);
			foreach (SkillBreakPlateBonus bonus in skill.GetBreakBonuses())
			{
				gridBonus += bonus.CalcFightBackPower();
			}
			return (int)configData.FightBackDamage * gridBonus * basePower * power;
		}

		// Token: 0x06006BB3 RID: 27571 RVA: 0x003C5BC8 File Offset: 0x003C3DC8
		public static OuterAndInnerInts CalcBouncePower(CombatSkill skill, CValuePercent power)
		{
			OuterAndInnerInts addPenetrate = default(OuterAndInnerInts);
			CombatSkillItem configData = CombatSkill.Instance[skill.GetId().SkillTemplateId];
			CValuePercent basePower = (int)GlobalConfig.Instance.DefendSkillBaseBouncePower;
			CValuePercentBonus outerBonus = skill.GetBreakoutGridCombatSkillPropertyBonus(25);
			CValuePercentBonus innerBonus = skill.GetBreakoutGridCombatSkillPropertyBonus(26);
			foreach (SkillBreakPlateBonus bonus in skill.GetBreakBonuses())
			{
				outerBonus += bonus.CalcBouncePower();
				innerBonus += bonus.CalcBouncePower();
			}
			addPenetrate.Outer = (int)configData.BounceRateOfOuterInjury * outerBonus * basePower * power;
			addPenetrate.Inner = (int)configData.BounceRateOfInnerInjury * innerBonus * basePower * power;
			return addPenetrate;
		}

		// Token: 0x06006BB4 RID: 27572 RVA: 0x003C5CD4 File Offset: 0x003C3ED4
		public int GetNewUnderstandingNeedExp(int charId, short skillTemplateId)
		{
			bool flag = charId != DomainManager.Taiwu.GetTaiwuCharId();
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				sbyte stepCount = DomainManager.CombatSkill.GetCombatSkillBreakStepCount(charId, skillTemplateId);
				int count = Math.Max((int)(50 - stepCount), 0);
				CombatSkillItem combatSkillConfig = CombatSkill.Instance[skillTemplateId];
				short costExp = Config.SkillBreakPlate.Instance[combatSkillConfig.Grade].CostExp;
				int totalCostExp = count * (int)costExp;
				result = totalCostExp;
			}
			return result;
		}

		// Token: 0x06006BB5 RID: 27573 RVA: 0x003C5D44 File Offset: 0x003C3F44
		public bool GetBreakSuccess(int charId, short skillTemplateId)
		{
			bool flag = charId != DomainManager.Taiwu.GetTaiwuCharId();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				CombatSkill skill;
				bool flag2 = !this.TryGetElement_CombatSkills(new ValueTuple<int, short>(charId, skillTemplateId), out skill);
				result = (!flag2 && CombatSkillStateHelper.IsBrokenOut(skill.GetActivationState()));
			}
			return result;
		}

		// Token: 0x06006BB6 RID: 27574 RVA: 0x003C5D98 File Offset: 0x003C3F98
		[DomainMethod]
		public CombatSkillDisplayData GetCombatSkillPreviewDisplayDataOnce(int charId, short skillTemplateId)
		{
			GameData.Domains.Character.Character character = null;
			bool flag = charId >= 0 && !DomainManager.Character.TryGetElement_Objects(charId, out character);
			if (flag)
			{
				charId = -1;
			}
			bool flag2 = charId < 0;
			if (flag2)
			{
				charId = DomainManager.Taiwu.GetTaiwuCharId();
				character = DomainManager.Taiwu.GetTaiwu();
			}
			CombatSkillDisplayData displayData = this.CalcCombatSkillDisplayData(skillTemplateId, charId, character);
			bool isMastered = DomainManager.Extra.IsCombatSkillMasteredByCharacter(charId, skillTemplateId);
			CombatSkillDisplayData combatSkillDisplayData = displayData;
			combatSkillDisplayData.GridCount += (isMastered ? 1 : -1);
			CombatSkillItem configData = CombatSkill.Instance[skillTemplateId];
			bool flag3 = configData.EquipType == 0;
			if (flag3)
			{
				CombatSkillKey skillKey = new CombatSkillKey(charId, skillTemplateId);
				bool skillExist = this._combatSkills.ContainsKey(skillKey);
				CombatSkill skill = skillExist ? this._combatSkills[skillKey] : new CombatSkill(charId, skillTemplateId, 0);
				CombatSkillDomain.CalcNeigongSkillDisplayData(displayData, skillExist, skill, configData, true);
			}
			displayData.Mastered = !isMastered;
			displayData.PreviewMastered = true;
			return displayData;
		}

		// Token: 0x06006BB7 RID: 27575 RVA: 0x003C5E90 File Offset: 0x003C4090
		[DomainMethod]
		public List<SkillBreakPlateBonus> GetCombatSkillBreakBonuses(int charId, short skillTemplateId)
		{
			List<SkillBreakPlateBonus> result = new List<SkillBreakPlateBonus>();
			CombatSkill skill;
			bool flag = !DomainManager.CombatSkill.TryGetElement_CombatSkills(new CombatSkillKey(charId, skillTemplateId), out skill);
			List<SkillBreakPlateBonus> result2;
			if (flag)
			{
				result2 = result;
			}
			else
			{
				result2 = skill.GetBreakBonuses().ToList<SkillBreakPlateBonus>();
			}
			return result2;
		}

		// Token: 0x06006BB8 RID: 27576 RVA: 0x003C5ED4 File Offset: 0x003C40D4
		public static List<BreakGrid> GetBonusBreakGrids(short skillTemplateId, sbyte behaviorType)
		{
			SkillBreakGridListItem configData = SkillBreakGridList.Instance[skillTemplateId];
			bool flag = configData == null;
			List<BreakGrid> result;
			if (flag)
			{
				result = null;
			}
			else
			{
				if (!true)
				{
				}
				List<BreakGrid> list;
				switch (behaviorType)
				{
				case 0:
					list = configData.BreakGridListJust;
					break;
				case 1:
					list = configData.BreakGridListKind;
					break;
				case 2:
					list = configData.BreakGridListEven;
					break;
				case 3:
					list = configData.BreakGridListRebel;
					break;
				case 4:
					list = configData.BreakGridListEgoistic;
					break;
				default:
					list = null;
					break;
				}
				if (!true)
				{
				}
				result = list;
			}
			return result;
		}

		// Token: 0x06006BB9 RID: 27577 RVA: 0x003C5F54 File Offset: 0x003C4154
		public static IEnumerable<int> FiveElementIndexes(int charId, CombatSkillItem config)
		{
			yield return (int)config.FiveElements;
			BoolArray8 alsoAs = DomainManager.SpecialEffect.ModifyData(charId, config.TemplateId, 240, 0, (int)config.FiveElements, -1, -1);
			int num;
			for (int i = 0; i <= 5; i = num + 1)
			{
				bool flag = i != (int)config.FiveElements && alsoAs[i];
				if (flag)
				{
					yield return i;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06006BBA RID: 27578 RVA: 0x003C5F6C File Offset: 0x003C416C
		public static bool FiveElementEquals(int charId, CombatSkillItem config, sbyte fiveElement)
		{
			bool flag = fiveElement < 0 || fiveElement > 5;
			bool flag2 = flag;
			return !flag2 && (config.FiveElements == fiveElement || DomainManager.SpecialEffect.ModifyData(charId, config.TemplateId, 240, 0, (int)config.FiveElements, -1, -1)[(int)fiveElement]);
		}

		// Token: 0x06006BBB RID: 27579 RVA: 0x003C5FD0 File Offset: 0x003C41D0
		public static bool FiveElementEquals(int charId, short skillId, sbyte fiveElement)
		{
			return skillId >= 0 && CombatSkillDomain.FiveElementEquals(charId, CombatSkill.Instance[skillId], fiveElement);
		}

		// Token: 0x06006BBC RID: 27580 RVA: 0x003C5FFC File Offset: 0x003C41FC
		public static bool FiveElementEquals(int charId, CombatSkillItem config, IEnumerable<sbyte> fiveElements)
		{
			return fiveElements.Any((sbyte fiveElement) => CombatSkillDomain.FiveElementEquals(charId, config, fiveElement));
		}

		// Token: 0x06006BBD RID: 27581 RVA: 0x003C6034 File Offset: 0x003C4234
		public static int FiveElementIndexesSum(int charId, CombatSkillItem config, sbyte[] properties)
		{
			int sum = 0;
			foreach (int fiveElement in CombatSkillDomain.FiveElementIndexes(charId, config))
			{
				sum += (int)properties[fiveElement];
			}
			return sum;
		}

		// Token: 0x06006BBE RID: 27582 RVA: 0x003C608C File Offset: 0x003C428C
		[return: TupleElementNames(new string[]
		{
			"min",
			"max"
		})]
		public static ValueTuple<int, int> FiveElementIndexesTotal(int charId, CombatSkillItem config, sbyte[] properties)
		{
			int min = 0;
			int max = 0;
			foreach (int fiveElement in CombatSkillDomain.FiveElementIndexes(charId, config))
			{
				int num = Math.Min(min, (int)properties[fiveElement]);
				max = Math.Max(max, (int)properties[fiveElement]);
				min = num;
			}
			return new ValueTuple<int, int>(min, max);
		}

		// Token: 0x06006BBF RID: 27583 RVA: 0x003C60FC File Offset: 0x003C42FC
		public static bool FiveElementMatch(int charId, CombatSkillItem config, List<sbyte> fiveElementsLimit)
		{
			return fiveElementsLimit == null || fiveElementsLimit.Count == 0 || CombatSkillDomain.FiveElementEquals(charId, config, fiveElementsLimit);
		}

		// Token: 0x06006BC0 RID: 27584 RVA: 0x003C6124 File Offset: 0x003C4324
		public static bool FiveElementContains(int charId, CombatSkillItem config, List<byte> fiveElements)
		{
			bool result;
			if (fiveElements != null && fiveElements.Count > 0)
			{
				result = CombatSkillDomain.FiveElementEquals(charId, config, from x in fiveElements
				select (sbyte)x);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06006BC1 RID: 27585 RVA: 0x003C6174 File Offset: 0x003C4374
		public CombatSkillDomain() : base(1)
		{
			this._combatSkills = new CombatSkillCollection(8192);
			this.HelperDataCombatSkills = new ObjectCollectionHelperData(7, 0, CombatSkillDomain.CacheInfluencesCombatSkills, this._dataStatesCombatSkills, true);
			this.OnInitializedDomainData();
		}

		// Token: 0x06006BC2 RID: 27586 RVA: 0x003C61CC File Offset: 0x003C43CC
		public CombatSkill GetElement_CombatSkills(CombatSkillKey objectId)
		{
			return this._combatSkills[objectId];
		}

		// Token: 0x06006BC3 RID: 27587 RVA: 0x003C61EC File Offset: 0x003C43EC
		public bool TryGetElement_CombatSkills(CombatSkillKey objectId, out CombatSkill element)
		{
			return this._combatSkills.TryGetValue(objectId, out element);
		}

		// Token: 0x06006BC4 RID: 27588 RVA: 0x003C620C File Offset: 0x003C440C
		private unsafe void AddElement_CombatSkills(CombatSkillKey objectId, CombatSkill instance)
		{
			instance.CollectionHelperData = this.HelperDataCombatSkills;
			instance.DataStatesOffset = this._dataStatesCombatSkills.Create();
			this._combatSkills.Add(objectId, instance);
			byte* pData = OperationAdder.FixedObjectCollection_Add<CombatSkillKey>(7, 0, objectId, 27);
			instance.Serialize(pData);
		}

		// Token: 0x06006BC5 RID: 27589 RVA: 0x003C6258 File Offset: 0x003C4458
		private void RemoveElement_CombatSkills(CombatSkillKey objectId)
		{
			CombatSkill instance;
			bool flag = !this._combatSkills.TryGetValue(objectId, out instance);
			if (!flag)
			{
				this._dataStatesCombatSkills.Remove(instance.DataStatesOffset);
				this._combatSkills.Remove(objectId);
				OperationAdder.FixedObjectCollection_Remove<CombatSkillKey>(7, 0, objectId);
			}
		}

		// Token: 0x06006BC6 RID: 27590 RVA: 0x003C62A5 File Offset: 0x003C44A5
		private void ClearCombatSkills()
		{
			this._dataStatesCombatSkills.Clear();
			this._combatSkills.Clear();
			OperationAdder.FixedObjectCollection_Clear(7, 0);
		}

		// Token: 0x06006BC7 RID: 27591 RVA: 0x003C62C8 File Offset: 0x003C44C8
		public int GetElementField_CombatSkills(CombatSkillKey objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
		{
			CombatSkill instance;
			bool flag = !this._combatSkills.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				string tag = "GetElementField_CombatSkills";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<CombatSkillKey>(objectId);
				defaultInterpolatedStringHandler.AppendLiteral(" with field ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
				result = -1;
			}
			else
			{
				if (resetModified)
				{
					this._dataStatesCombatSkills.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
				switch (fieldId)
				{
				case 0:
					result = Serializer.Serialize(instance.GetId(), dataPool);
					break;
				case 1:
					result = Serializer.Serialize(instance.GetPracticeLevel(), dataPool);
					break;
				case 2:
					result = Serializer.Serialize(instance.GetReadingState(), dataPool);
					break;
				case 3:
					result = Serializer.Serialize(instance.GetActivationState(), dataPool);
					break;
				case 4:
					result = Serializer.Serialize(instance.GetForcedBreakoutStepsCount(), dataPool);
					break;
				case 5:
					result = Serializer.Serialize(instance.GetBreakoutStepsCount(), dataPool);
					break;
				case 6:
					result = Serializer.Serialize(instance.GetInnerRatio(), dataPool);
					break;
				case 7:
					result = Serializer.Serialize(instance.GetObtainedNeili(), dataPool);
					break;
				case 8:
					result = Serializer.Serialize(instance.GetRevoked(), dataPool);
					break;
				case 9:
					result = Serializer.Serialize(instance.GetSpecialEffectId(), dataPool);
					break;
				case 10:
					result = Serializer.Serialize(instance.GetPower(), dataPool);
					break;
				case 11:
					result = Serializer.Serialize(instance.GetMaxPower(), dataPool);
					break;
				case 12:
					result = Serializer.Serialize(instance.GetRequirementPercent(), dataPool);
					break;
				case 13:
					result = Serializer.Serialize(instance.GetDirection(), dataPool);
					break;
				case 14:
					result = Serializer.Serialize(instance.GetBaseScore(), dataPool);
					break;
				case 15:
					result = Serializer.Serialize(instance.GetCurrInnerRatio(), dataPool);
					break;
				case 16:
					result = Serializer.Serialize(instance.GetHitValue(), dataPool);
					break;
				case 17:
					result = Serializer.Serialize(instance.GetPenetrations(), dataPool);
					break;
				case 18:
					result = Serializer.Serialize(instance.GetCostBreathAndStancePercent(), dataPool);
					break;
				case 19:
					result = Serializer.Serialize(instance.GetCostBreathPercent(), dataPool);
					break;
				case 20:
					result = Serializer.Serialize(instance.GetCostStancePercent(), dataPool);
					break;
				case 21:
					result = Serializer.Serialize(instance.GetCostMobilityPercent(), dataPool);
					break;
				case 22:
					result = Serializer.Serialize(instance.GetAddHitValueOnCast(), dataPool);
					break;
				case 23:
					result = Serializer.Serialize(instance.GetAddPenetrateResist(), dataPool);
					break;
				case 24:
					result = Serializer.Serialize(instance.GetAddAvoidValueOnCast(), dataPool);
					break;
				case 25:
					result = Serializer.Serialize(instance.GetFightBackPower(), dataPool);
					break;
				case 26:
					result = Serializer.Serialize(instance.GetBouncePower(), dataPool);
					break;
				case 27:
					result = Serializer.Serialize(instance.GetRequirementsPower(), dataPool);
					break;
				case 28:
					result = Serializer.Serialize(instance.GetPlateAddMaxPower(), dataPool);
					break;
				default:
				{
					bool flag2 = fieldId >= 29;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					if (flag2)
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to get readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				}
			}
			return result;
		}

		// Token: 0x06006BC8 RID: 27592 RVA: 0x003C664C File Offset: 0x003C484C
		public void SetElementField_CombatSkills(CombatSkillKey objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			CombatSkill instance;
			bool flag = !this._combatSkills.TryGetValue(objectId, out instance);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<CombatSkillKey>(objectId);
				defaultInterpolatedStringHandler.AppendLiteral(" with field ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			switch (fieldId)
			{
			case 0:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 1:
			{
				sbyte value = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value);
				instance.SetPracticeLevel(value, context);
				break;
			}
			case 2:
			{
				ushort value2 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value2);
				instance.SetReadingState(value2, context);
				break;
			}
			case 3:
			{
				ushort value3 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value3);
				instance.SetActivationState(value3, context);
				break;
			}
			case 4:
			{
				sbyte value4 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value4);
				instance.SetForcedBreakoutStepsCount(value4, context);
				break;
			}
			case 5:
			{
				sbyte value5 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value5);
				instance.SetBreakoutStepsCount(value5, context);
				break;
			}
			case 6:
			{
				sbyte value6 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value6);
				instance.SetInnerRatio(value6, context);
				break;
			}
			case 7:
			{
				short value7 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value7);
				instance.SetObtainedNeili(value7, context);
				break;
			}
			case 8:
			{
				bool value8 = false;
				Serializer.Deserialize(dataPool, valueOffset, ref value8);
				instance.SetRevoked(value8, context);
				break;
			}
			case 9:
			{
				long value9 = 0L;
				Serializer.Deserialize(dataPool, valueOffset, ref value9);
				instance.SetSpecialEffectId(value9, context);
				break;
			}
			default:
			{
				bool flag2 = fieldId >= 29;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (flag2)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = fieldId >= 29;
				if (flag3)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set cache field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06006BC9 RID: 27593 RVA: 0x003C68D4 File Offset: 0x003C4AD4
		private int CheckModified_CombatSkills(CombatSkillKey objectId, ushort fieldId, RawDataPool dataPool)
		{
			CombatSkill instance;
			bool flag = !this._combatSkills.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = fieldId >= 29;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesCombatSkills.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					this._dataStatesCombatSkills.ResetModified(instance.DataStatesOffset, (int)fieldId);
					switch (fieldId)
					{
					case 0:
						result = Serializer.Serialize(instance.GetId(), dataPool);
						break;
					case 1:
						result = Serializer.Serialize(instance.GetPracticeLevel(), dataPool);
						break;
					case 2:
						result = Serializer.Serialize(instance.GetReadingState(), dataPool);
						break;
					case 3:
						result = Serializer.Serialize(instance.GetActivationState(), dataPool);
						break;
					case 4:
						result = Serializer.Serialize(instance.GetForcedBreakoutStepsCount(), dataPool);
						break;
					case 5:
						result = Serializer.Serialize(instance.GetBreakoutStepsCount(), dataPool);
						break;
					case 6:
						result = Serializer.Serialize(instance.GetInnerRatio(), dataPool);
						break;
					case 7:
						result = Serializer.Serialize(instance.GetObtainedNeili(), dataPool);
						break;
					case 8:
						result = Serializer.Serialize(instance.GetRevoked(), dataPool);
						break;
					case 9:
						result = Serializer.Serialize(instance.GetSpecialEffectId(), dataPool);
						break;
					case 10:
						result = Serializer.Serialize(instance.GetPower(), dataPool);
						break;
					case 11:
						result = Serializer.Serialize(instance.GetMaxPower(), dataPool);
						break;
					case 12:
						result = Serializer.Serialize(instance.GetRequirementPercent(), dataPool);
						break;
					case 13:
						result = Serializer.Serialize(instance.GetDirection(), dataPool);
						break;
					case 14:
						result = Serializer.Serialize(instance.GetBaseScore(), dataPool);
						break;
					case 15:
						result = Serializer.Serialize(instance.GetCurrInnerRatio(), dataPool);
						break;
					case 16:
						result = Serializer.Serialize(instance.GetHitValue(), dataPool);
						break;
					case 17:
						result = Serializer.Serialize(instance.GetPenetrations(), dataPool);
						break;
					case 18:
						result = Serializer.Serialize(instance.GetCostBreathAndStancePercent(), dataPool);
						break;
					case 19:
						result = Serializer.Serialize(instance.GetCostBreathPercent(), dataPool);
						break;
					case 20:
						result = Serializer.Serialize(instance.GetCostStancePercent(), dataPool);
						break;
					case 21:
						result = Serializer.Serialize(instance.GetCostMobilityPercent(), dataPool);
						break;
					case 22:
						result = Serializer.Serialize(instance.GetAddHitValueOnCast(), dataPool);
						break;
					case 23:
						result = Serializer.Serialize(instance.GetAddPenetrateResist(), dataPool);
						break;
					case 24:
						result = Serializer.Serialize(instance.GetAddAvoidValueOnCast(), dataPool);
						break;
					case 25:
						result = Serializer.Serialize(instance.GetFightBackPower(), dataPool);
						break;
					case 26:
						result = Serializer.Serialize(instance.GetBouncePower(), dataPool);
						break;
					case 27:
						result = Serializer.Serialize(instance.GetRequirementsPower(), dataPool);
						break;
					case 28:
						result = Serializer.Serialize(instance.GetPlateAddMaxPower(), dataPool);
						break;
					default:
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					}
				}
			}
			return result;
		}

		// Token: 0x06006BCA RID: 27594 RVA: 0x003C6C1C File Offset: 0x003C4E1C
		private void ResetModifiedWrapper_CombatSkills(CombatSkillKey objectId, ushort fieldId)
		{
			CombatSkill instance;
			bool flag = !this._combatSkills.TryGetValue(objectId, out instance);
			if (!flag)
			{
				bool flag2 = fieldId >= 29;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesCombatSkills.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (!flag3)
				{
					this._dataStatesCombatSkills.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
			}
		}

		// Token: 0x06006BCB RID: 27595 RVA: 0x003C6CAC File Offset: 0x003C4EAC
		private bool IsModifiedWrapper_CombatSkills(CombatSkillKey objectId, ushort fieldId)
		{
			CombatSkill instance;
			bool flag = !this._combatSkills.TryGetValue(objectId, out instance);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = fieldId >= 29;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = this._dataStatesCombatSkills.IsModified(instance.DataStatesOffset, (int)fieldId);
			}
			return result;
		}

		// Token: 0x06006BCC RID: 27596 RVA: 0x003C6D22 File Offset: 0x003C4F22
		public override void OnInitializeGameDataModule()
		{
			this.InitializeOnInitializeGameDataModule();
		}

		// Token: 0x06006BCD RID: 27597 RVA: 0x003C6D2C File Offset: 0x003C4F2C
		public unsafe override void OnEnterNewWorld()
		{
			this.InitializeOnEnterNewWorld();
			this.InitializeInternalDataOfCollections();
			foreach (KeyValuePair<CombatSkillKey, CombatSkill> entry in this._combatSkills)
			{
				CombatSkillKey objectId = entry.Key;
				CombatSkill instance = entry.Value;
				byte* pData = OperationAdder.FixedObjectCollection_Add<CombatSkillKey>(7, 0, objectId, 27);
				instance.Serialize(pData);
			}
		}

		// Token: 0x06006BCE RID: 27598 RVA: 0x003C6DB0 File Offset: 0x003C4FB0
		public override void OnLoadWorld()
		{
			this._pendingLoadingOperationIds = new Queue<uint>();
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedObjectCollection_GetAllObjects(7, 0));
		}

		// Token: 0x06006BCF RID: 27599 RVA: 0x003C6DD4 File Offset: 0x003C4FD4
		public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
		{
			if (dataId != 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return this.GetElementField_CombatSkills((CombatSkillKey)subId0, (ushort)subId1, dataPool, resetModified);
		}

		// Token: 0x06006BD0 RID: 27600 RVA: 0x003C6E30 File Offset: 0x003C5030
		public override void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			if (dataId != 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.SetElementField_CombatSkills((CombatSkillKey)subId0, (ushort)subId1, valueOffset, dataPool, context);
		}

		// Token: 0x06006BD1 RID: 27601 RVA: 0x003C6E8C File Offset: 0x003C508C
		public override int CallMethod(Operation operation, RawDataPool argDataPool, RawDataPool returnDataPool, DataContext context)
		{
			int argsOffset = operation.ArgsOffset;
			int result;
			switch (operation.MethodId)
			{
			case 0:
			{
				int argsCount = operation.ArgsCount;
				int num = argsCount;
				if (num != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId);
				List<short> skillTemplateIdList = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref skillTemplateIdList);
				List<CombatSkillDisplayData> returnValue = this.GetCombatSkillDisplayData(charId, skillTemplateIdList);
				result = Serializer.Serialize(returnValue, returnDataPool);
				break;
			}
			case 1:
			{
				int argsCount2 = operation.ArgsCount;
				int num2 = argsCount2;
				if (num2 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId2);
				short skillTemplateId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref skillTemplateId);
				sbyte returnValue2 = this.GetCombatSkillBreakStepCount(charId2, skillTemplateId);
				result = Serializer.Serialize(returnValue2, returnDataPool);
				break;
			}
			case 2:
			{
				int argsCount3 = operation.ArgsCount;
				int num3 = argsCount3;
				if (num3 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId3);
				List<CombatSkillDisplayData> returnValue3 = this.GetCharacterEquipCombatSkillDisplayData(charId3);
				result = Serializer.Serialize(returnValue3, returnDataPool);
				break;
			}
			case 3:
			{
				int argsCount4 = operation.ArgsCount;
				int num4 = argsCount4;
				if (num4 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId4 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId4);
				short skillTemplateId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref skillTemplateId2);
				CombatSkillDisplayData returnValue4 = this.GetCombatSkillDisplayDataOnce(charId4, skillTemplateId2);
				result = Serializer.Serialize(returnValue4, returnDataPool);
				break;
			}
			case 4:
			{
				int argsCount5 = operation.ArgsCount;
				int num5 = argsCount5;
				if (num5 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId5 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId5);
				List<short> skillIds = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref skillIds);
				List<CombatSkillEffectDescriptionDisplayData> returnValue5 = this.GetEffectDescriptionData(charId5, skillIds);
				result = Serializer.Serialize(returnValue5, returnDataPool);
				break;
			}
			case 5:
			{
				int argsCount6 = operation.ArgsCount;
				int num6 = argsCount6;
				if (num6 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ValueTuple<int, int> returnValue6 = this.CalcTaiwuExtraDeltaNeiliPerLoop(context);
				result = Serializer.Serialize(returnValue6, returnDataPool);
				break;
			}
			case 6:
			{
				int argsCount7 = operation.ArgsCount;
				int num7 = argsCount7;
				if (num7 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				IntList returnValue7 = this.CalcTaiwuExtraDeltaNeiliAllocationPerLoop(context);
				result = Serializer.Serialize(returnValue7, returnDataPool);
				break;
			}
			case 7:
			{
				int argsCount8 = operation.ArgsCount;
				int num8 = argsCount8;
				if (num8 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId6 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId6);
				short skillTemplateId3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref skillTemplateId3);
				CombatSkillDisplayData returnValue8 = this.GetCombatSkillPreviewDisplayDataOnce(charId6, skillTemplateId3);
				result = Serializer.Serialize(returnValue8, returnDataPool);
				break;
			}
			case 8:
			{
				int argsCount9 = operation.ArgsCount;
				int num9 = argsCount9;
				if (num9 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId7 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId7);
				short skillTemplateId4 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref skillTemplateId4);
				int returnValue9 = this.GetCombatSkillBreakoutStepsMaxPower(charId7, skillTemplateId4);
				result = Serializer.Serialize(returnValue9, returnDataPool);
				break;
			}
			case 9:
			{
				int argsCount10 = operation.ArgsCount;
				int num10 = argsCount10;
				if (num10 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId8 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId8);
				short skillTemplateId5 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref skillTemplateId5);
				List<SkillBreakPlateBonus> returnValue10 = this.GetCombatSkillBreakBonuses(charId8, skillTemplateId5);
				result = Serializer.Serialize(returnValue10, returnDataPool);
				break;
			}
			case 10:
			{
				int argsCount11 = operation.ArgsCount;
				int num11 = argsCount11;
				if (num11 != 4)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId9 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId9);
				short skillId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref skillId);
				byte pageId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref pageId);
				sbyte direction = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref direction);
				bool returnValue11 = this.SetActivePage(context, charId9, skillId, pageId, direction);
				result = Serializer.Serialize(returnValue11, returnDataPool);
				break;
			}
			case 11:
			{
				int argsCount12 = operation.ArgsCount;
				int num12 = argsCount12;
				if (num12 != 4)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId10 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId10);
				short skillId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref skillId2);
				byte pageId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref pageId2);
				sbyte direction2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref direction2);
				bool returnValue12 = this.DeActivePage(context, charId10, skillId2, pageId2, direction2);
				result = Serializer.Serialize(returnValue12, returnDataPool);
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06006BD2 RID: 27602 RVA: 0x003C750C File Offset: 0x003C570C
		public override void OnMonitorData(ushort dataId, ulong subId0, uint subId1, bool monitoring)
		{
			if (dataId != 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06006BD3 RID: 27603 RVA: 0x003C7554 File Offset: 0x003C5754
		public override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
		{
			if (dataId != 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return this.CheckModified_CombatSkills((CombatSkillKey)subId0, (ushort)subId1, dataPool);
		}

		// Token: 0x06006BD4 RID: 27604 RVA: 0x003C75B0 File Offset: 0x003C57B0
		public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			if (dataId != 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.ResetModifiedWrapper_CombatSkills((CombatSkillKey)subId0, (ushort)subId1);
		}

		// Token: 0x06006BD5 RID: 27605 RVA: 0x003C7608 File Offset: 0x003C5808
		public override bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			if (dataId != 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return this.IsModifiedWrapper_CombatSkills((CombatSkillKey)subId0, (ushort)subId1);
		}

		// Token: 0x06006BD6 RID: 27606 RVA: 0x003C7660 File Offset: 0x003C5860
		public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
		{
			ushort dataId = influence.TargetIndicator.DataId;
			ushort num = dataId;
			if (num != 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(influence.TargetIndicator.DataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			bool flag = !unconditionallyInfluenceAll;
			if (flag)
			{
				List<BaseGameDataObject> influencedObjects = InfluenceChecker.InfluencedObjectsPool.Get();
				bool influenceAll = InfluenceChecker.GetScope<CombatSkillKey, CombatSkill>(context, sourceObject, influence.Scope, this._combatSkills, influencedObjects);
				bool flag2 = !influenceAll;
				if (flag2)
				{
					int influencedObjectsCount = influencedObjects.Count;
					for (int i = 0; i < influencedObjectsCount; i++)
					{
						BaseGameDataObject targetObject = influencedObjects[i];
						List<DataUid> targetUids = influence.TargetUids;
						int targetUidsCount = targetUids.Count;
						for (int j = 0; j < targetUidsCount; j++)
						{
							DataUid targetUid = targetUids[j];
							targetObject.InvalidateSelfAndInfluencedCache((ushort)targetUid.SubId1, context);
						}
					}
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CombatSkillDomain.CacheInfluencesCombatSkills, this._dataStatesCombatSkills, influence, context);
				}
				influencedObjects.Clear();
				InfluenceChecker.InfluencedObjectsPool.Return(influencedObjects);
			}
			else
			{
				BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CombatSkillDomain.CacheInfluencesCombatSkills, this._dataStatesCombatSkills, influence, context);
			}
		}

		// Token: 0x06006BD7 RID: 27607 RVA: 0x003C77AC File Offset: 0x003C59AC
		public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
		{
			ushort dataId = operation.DataId;
			ushort num = dataId;
			if (num != 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.DataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			ResponseProcessor.ProcessFixedObjectCollection<CombatSkillKey, CombatSkill>(operation, pResult, this._combatSkills);
			bool flag = this._pendingLoadingOperationIds != null;
			if (flag)
			{
				uint currPendingOperationId = this._pendingLoadingOperationIds.Peek();
				bool flag2 = currPendingOperationId == operation.Id;
				if (flag2)
				{
					this._pendingLoadingOperationIds.Dequeue();
					bool flag3 = this._pendingLoadingOperationIds.Count <= 0;
					if (flag3)
					{
						this._pendingLoadingOperationIds = null;
						this.InitializeInternalDataOfCollections();
						this.OnLoadedArchiveData();
						DomainManager.Global.CompleteLoading(7);
					}
				}
			}
		}

		// Token: 0x06006BD8 RID: 27608 RVA: 0x003C7884 File Offset: 0x003C5A84
		private void InitializeInternalDataOfCollections()
		{
			foreach (KeyValuePair<CombatSkillKey, CombatSkill> entry in this._combatSkills)
			{
				CombatSkill instance = entry.Value;
				instance.CollectionHelperData = this.HelperDataCombatSkills;
				instance.DataStatesOffset = this._dataStatesCombatSkills.Create();
			}
		}

		// Token: 0x06006BDA RID: 27610 RVA: 0x003C7929 File Offset: 0x003C5B29
		[CompilerGenerated]
		internal static sbyte <CalcProactiveSkillDisplayData>g__Convert|63_0(bool enough)
		{
			return enough ? 1 : 2;
		}

		// Token: 0x04001DA0 RID: 7584
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04001DA1 RID: 7585
		[DomainData(DomainDataType.ObjectCollection, true, false, true, true, CollectionCapacity = 8192)]
		private readonly CombatSkillCollection _combatSkills;

		// Token: 0x04001DA2 RID: 7586
		private static readonly Dictionary<short, CombatSkill> EmptyCharCombatSkills = new Dictionary<short, CombatSkill>();

		// Token: 0x04001DA3 RID: 7587
		public static short[][] EquipAddPropertyDict;

		// Token: 0x04001DA4 RID: 7588
		private static List<CombatSkillItem>[][] _learnableCombatSkillsCache;

		// Token: 0x04001DA5 RID: 7589
		public const byte MaxCostTrickTypeCount = 3;

		// Token: 0x04001DA6 RID: 7590
		private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[1][];

		// Token: 0x04001DA7 RID: 7591
		private static readonly DataInfluence[][] CacheInfluencesCombatSkills = new DataInfluence[29][];

		// Token: 0x04001DA8 RID: 7592
		private readonly ObjectCollectionDataStates _dataStatesCombatSkills = new ObjectCollectionDataStates(29, 8192);

		// Token: 0x04001DA9 RID: 7593
		public readonly ObjectCollectionHelperData HelperDataCombatSkills;

		// Token: 0x04001DAA RID: 7594
		private Queue<uint> _pendingLoadingOperationIds;
	}
}
