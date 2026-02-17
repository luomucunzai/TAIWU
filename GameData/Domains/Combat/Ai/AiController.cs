using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Combat.Ai.Memory;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.GameDataBridge;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Combat.Ai
{
	// Token: 0x02000715 RID: 1813
	public class AiController
	{
		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06006859 RID: 26713 RVA: 0x003B4BEF File Offset: 0x003B2DEF
		public bool IsCombatDifficultyLevel1
		{
			get
			{
				return this._combatCharacter.IsAlly || DomainManager.World.GetCombatDifficulty() >= 1;
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x0600685A RID: 26714 RVA: 0x003B4C11 File Offset: 0x003B2E11
		public bool IsCombatDifficultyLevel2
		{
			get
			{
				return this._combatCharacter.IsAlly || DomainManager.World.GetCombatDifficulty() >= 2;
			}
		}

		// Token: 0x0600685B RID: 26715 RVA: 0x003B4C34 File Offset: 0x003B2E34
		public AiController(CombatCharacter combatCharacter)
		{
			this._combatCharacter = combatCharacter;
			this.Environment = new AiEnvironment(combatCharacter);
			this.Memory = new AiMemory(combatCharacter);
			this.AllowDefense = true;
			this._aiTree = new AiTree(combatCharacter, new AiDataFile(this.ProperAiData()));
		}

		// Token: 0x0600685C RID: 26716 RVA: 0x003B4C9C File Offset: 0x003B2E9C
		private AiDataItem ProperAiData()
		{
			bool isTaiwu = this._combatCharacter.IsTaiwu;
			AiDataItem result;
			if (isTaiwu)
			{
				result = AiData.Instance[0];
			}
			else
			{
				CombatConfigItem combatConfig = DomainManager.Combat.CombatConfig;
				bool flag = combatConfig.EnemyAi >= 0 && !this._combatCharacter.IsAlly;
				if (flag)
				{
					result = AiData.Instance[combatConfig.EnemyAi];
				}
				else
				{
					CharacterItem characterConfig = this._combatCharacter.GetCharacter().Template;
					result = AiData.Instance[characterConfig.CombatAi];
				}
			}
			return result;
		}

		// Token: 0x0600685D RID: 26717 RVA: 0x003B4D2C File Offset: 0x003B2F2C
		public void Init()
		{
			this.InitHazard();
			this.Environment.RegisterCallbacks();
			this.Memory.RegisterCallbacks();
			bool flag = !this.IsCombatDifficultyLevel2;
			if (!flag)
			{
				List<short> selfLearnedSkills = this._combatCharacter.GetCharacter().GetLearnedCombatSkills();
				foreach (int enemyId in DomainManager.Combat.GetCharacterList(!this._combatCharacter.IsAlly))
				{
					bool flag2 = enemyId < 0;
					if (!flag2)
					{
						List<short> attackSkillList = DomainManager.Combat.GetElement_CombatCharacterDict(enemyId).GetAttackSkillList();
						foreach (short skillId in attackSkillList)
						{
							bool flag3 = selfLearnedSkills.Contains(skillId);
							if (flag3)
							{
								this.Memory.EnemyRecordDict[enemyId].SkillRecord.Add(skillId, new ValueTuple<int, int>(400, 0));
							}
						}
					}
				}
			}
		}

		// Token: 0x0600685E RID: 26718 RVA: 0x003B4E4C File Offset: 0x003B304C
		public void UnInit()
		{
			this.UnInitHazard();
			this.Environment.UnregisterCallbacks();
			this.Memory.UnregisterCallbacks();
		}

		// Token: 0x0600685F RID: 26719 RVA: 0x003B4E6E File Offset: 0x003B306E
		public void ClearMemories()
		{
			this.Memory.ClearMemories();
			this._aiTree.ClearMemories();
		}

		// Token: 0x06006860 RID: 26720 RVA: 0x003B4E8C File Offset: 0x003B308C
		private void InitHazard()
		{
			this._maxHazardValue = Math.Min(750 + 750 * (int)this._combatCharacter.GetPersonalityValue(4) / 100, 1500);
			this._addHazardPerMark = AiController.AddHazardPerMark[(int)DomainManager.Combat.CombatConfig.CombatType];
			this._specialMarkAddHazardNeedCount = AiController.SpecialMarkAddHazardNeedCount[(int)DomainManager.Combat.CombatConfig.CombatType];
			this._lastMarks = new DefeatMarkCollection();
			this._lastMarks.Clear();
			this._defeatMarkUid = new DataUid(8, 10, (ulong)((long)this._combatCharacter.GetId()), 50U);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(14, 1);
			defaultInterpolatedStringHandler.AppendLiteral("CombatAiHazard");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._combatCharacter.GetId());
			this._defeatMarkDataHandlerKey = defaultInterpolatedStringHandler.ToStringAndClear();
			GameDataBridge.AddPostDataModificationHandler(this._defeatMarkUid, this._defeatMarkDataHandlerKey, new Action<DataContext, DataUid>(this.OnDefeatMarkChanged));
			this.OnDefeatMarkChanged(this._combatCharacter.GetDataContext(), this._defeatMarkUid);
		}

		// Token: 0x06006861 RID: 26721 RVA: 0x003B4F9B File Offset: 0x003B319B
		private void UnInitHazard()
		{
			GameDataBridge.RemovePostDataModificationHandler(this._defeatMarkUid, this._defeatMarkDataHandlerKey);
		}

		// Token: 0x06006862 RID: 26722 RVA: 0x003B4FB0 File Offset: 0x003B31B0
		private void OnDefeatMarkChanged(DataContext context, DataUid dataUid)
		{
			DefeatMarkCollection markCollection = this._combatCharacter.GetDefeatMarkCollection();
			int totalFlaw = 0;
			int totalAddFlaw = 0;
			int totalAcupoint = 0;
			int totalAddAcupoint = 0;
			int addHazard = 0;
			for (sbyte part = 0; part < 7; part += 1)
			{
				byte outerInjury = markCollection.OuterInjuryMarkList[(int)part];
				byte innerInjury = markCollection.InnerInjuryMarkList[(int)part];
				ByteList flawList = markCollection.FlawMarkList[(int)part];
				ByteList acupointList = markCollection.AcupointMarkList[(int)part];
				bool flag = outerInjury > this._lastMarks.OuterInjuryMarkList[(int)part];
				if (flag)
				{
					addHazard += this._addHazardPerMark * (int)(outerInjury - this._lastMarks.OuterInjuryMarkList[(int)part]);
				}
				bool flag2 = innerInjury > this._lastMarks.InnerInjuryMarkList[(int)part];
				if (flag2)
				{
					addHazard += this._addHazardPerMark * (int)(innerInjury - this._lastMarks.InnerInjuryMarkList[(int)part]);
				}
				totalFlaw += flawList.Count;
				bool flag3 = flawList.Count > this._lastMarks.FlawMarkList[(int)part].Count;
				if (flag3)
				{
					totalAddFlaw += flawList.Count - this._lastMarks.FlawMarkList[(int)part].Count;
				}
				totalAcupoint += acupointList.Count;
				bool flag4 = acupointList.Count > this._lastMarks.AcupointMarkList[(int)part].Count;
				if (flag4)
				{
					totalAddAcupoint += acupointList.Count - this._lastMarks.AcupointMarkList[(int)part].Count;
				}
				int maxAddCount = Math.Max(acupointList.Count - this._specialMarkAddHazardNeedCount, 0);
				addHazard += this._addHazardPerMark * Math.Min(acupointList.Count - this._lastMarks.AcupointMarkList[(int)part].Count, maxAddCount);
				this._lastMarks.OuterInjuryMarkList[(int)part] = outerInjury;
				this._lastMarks.InnerInjuryMarkList[(int)part] = innerInjury;
				this._lastMarks.FlawMarkList[(int)part].Clear();
				this._lastMarks.FlawMarkList[(int)part].AddRange(flawList);
				this._lastMarks.AcupointMarkList[(int)part].Clear();
				this._lastMarks.AcupointMarkList[(int)part].AddRange(acupointList);
			}
			bool flag5 = totalAddFlaw > 0;
			if (flag5)
			{
				int maxAddCount2 = Math.Max(totalFlaw - this._specialMarkAddHazardNeedCount, 0);
				addHazard += this._addHazardPerMark * Math.Min(totalAddFlaw, maxAddCount2);
			}
			bool flag6 = totalAddAcupoint > 0;
			if (flag6)
			{
				int maxAddCount3 = Math.Max(totalAcupoint - this._specialMarkAddHazardNeedCount, 0);
				addHazard += this._addHazardPerMark * Math.Min(totalAddAcupoint, maxAddCount3);
			}
			for (sbyte type = 0; type < 6; type += 1)
			{
				byte poison = markCollection.PoisonMarkList[(int)type];
				bool flag7 = poison > this._lastMarks.PoisonMarkList[(int)type];
				if (flag7)
				{
					addHazard += this._addHazardPerMark * (int)(poison - this._lastMarks.PoisonMarkList[(int)type]);
				}
				this._lastMarks.PoisonMarkList[(int)type] = poison;
			}
			bool flag8 = markCollection.MindMarkList.Count > this._lastMarks.MindMarkList.Count;
			if (flag8)
			{
				int maxAddCount4 = Math.Max(markCollection.MindMarkList.Count - this._specialMarkAddHazardNeedCount, 0);
				addHazard += this._addHazardPerMark * Math.Min(markCollection.MindMarkList.Count - this._lastMarks.MindMarkList.Count, maxAddCount4);
			}
			this._lastMarks.MindMarkList.Clear();
			this._lastMarks.MindMarkList.AddRange(markCollection.MindMarkList);
			bool flag9 = markCollection.DieMarkList.Count > this._lastMarks.DieMarkList.Count;
			if (flag9)
			{
				addHazard += this._addHazardPerMark * (markCollection.DieMarkList.Count - this._lastMarks.DieMarkList.Count);
			}
			this._lastMarks.DieMarkList.Clear();
			this._lastMarks.DieMarkList.AddRange(markCollection.DieMarkList);
			bool flag10 = markCollection.FatalDamageMarkCount > this._lastMarks.FatalDamageMarkCount;
			if (flag10)
			{
				addHazard += this._addHazardPerMark * (markCollection.FatalDamageMarkCount - this._lastMarks.FatalDamageMarkCount);
			}
			this._lastMarks.FatalDamageMarkCount = markCollection.FatalDamageMarkCount;
			this.ChangeHazardValue(context, addHazard);
		}

		// Token: 0x06006863 RID: 26723 RVA: 0x003B5404 File Offset: 0x003B3604
		public void ChangeHazardValue(DataContext context, int hazardValue)
		{
			int newValue = Math.Clamp(this._combatCharacter.GetHazardValue() + hazardValue, 0, this._maxHazardValue);
			this._combatCharacter.SetHazardValue(newValue, context);
		}

		// Token: 0x06006864 RID: 26724 RVA: 0x003B543C File Offset: 0x003B363C
		public CValuePercent GetHazardPercent()
		{
			return CValuePercent.Parse(this._combatCharacter.GetHazardValue(), this._maxHazardValue);
		}

		// Token: 0x06006865 RID: 26725 RVA: 0x003B5464 File Offset: 0x003B3664
		public bool IsHazard()
		{
			DefeatMarkCollection markCollection = this._combatCharacter.GetDefeatMarkCollection();
			return this._combatCharacter.GetHazardValue() >= this._maxHazardValue || markCollection.FatalDamageMarkCount >= 3 || markCollection.DieMarkList.Count >= 3;
		}

		// Token: 0x06006866 RID: 26726 RVA: 0x003B54B4 File Offset: 0x003B36B4
		public bool CanFlee()
		{
			return DomainManager.Combat.CanFlee(this._combatCharacter.IsAlly);
		}

		// Token: 0x06006867 RID: 26727 RVA: 0x003B54DC File Offset: 0x003B36DC
		public short GetBestCombatSkill(IRandomSource random, sbyte equipType, bool requireCanUse = false, int costMaxFrame = -1, int costMaxBreath = -1, int costMaxStance = -1, short exceptSkill = -1)
		{
			List<short> skillIdList = ObjectPool<List<short>>.Instance.Get();
			short bestSkillId = -1;
			skillIdList.Clear();
			skillIdList.AddRange(this._combatCharacter.GetCombatSkillList(equipType));
			skillIdList.RemoveAll((short id) => id < 0);
			skillIdList.Remove(exceptSkill);
			bool flag = skillIdList.Count > 0;
			if (flag)
			{
				this._skillScoreDict.Clear();
				foreach (short skillId in skillIdList)
				{
					if (!true)
					{
					}
					int num;
					if (equipType != 1)
					{
						if (equipType != 3)
						{
							num = 0;
						}
						else
						{
							num = this.CalcDefenseSkillScore(skillId, requireCanUse, costMaxFrame, costMaxBreath, costMaxStance);
						}
					}
					else
					{
						num = this.CalcAttackSkillScore(random, skillId, requireCanUse, costMaxFrame, costMaxBreath, costMaxStance);
					}
					if (!true)
					{
					}
					int score = num;
					this._skillScoreDict.Add(skillId, score);
				}
				int currHazard = this._combatCharacter.GetHazardValue();
				bool flag2 = !this.IsHazard() && currHazard < this._maxHazardValue / 2 && skillIdList.Count > 1;
				if (flag2)
				{
					int leftSkillCount = (skillIdList.Count - 1) * currHazard / (this._maxHazardValue / 2) + 1;
					CollectionUtils.Shuffle<short>(DomainManager.Combat.Context.Random, skillIdList);
					skillIdList.Sort((short skillL, short skillR) => this._skillScoreDict[skillR] - this._skillScoreDict[skillL]);
					while (skillIdList.Count > leftSkillCount)
					{
						skillIdList.RemoveAt(0);
					}
				}
				int maxScore = 0;
				for (int i = 0; i < skillIdList.Count; i++)
				{
					maxScore = Math.Max(maxScore, this._skillScoreDict[skillIdList[i]]);
				}
				foreach (short skillId2 in this._skillScoreDict.Keys)
				{
					bool flag3 = this._skillScoreDict[skillId2] != maxScore;
					if (flag3)
					{
						skillIdList.Remove(skillId2);
					}
				}
				bool flag4 = skillIdList.Count > 0;
				if (flag4)
				{
					bestSkillId = skillIdList.GetRandom(random);
				}
			}
			ObjectPool<List<short>>.Instance.Return(skillIdList);
			return bestSkillId;
		}

		// Token: 0x06006868 RID: 26728 RVA: 0x003B574C File Offset: 0x003B394C
		public int GetBestWeaponIndex(IRandomSource random, short skillId = -1, bool needInRange = false, int exceptIndex = -1)
		{
			bool hasUsableNormalWeapon = false;
			this._weaponScoreDict.Clear();
			for (int i = 0; i < 7; i++)
			{
				bool flag = i == exceptIndex || !this._combatCharacter.GetWeapons()[i].IsValid() || (i < 3 && this._combatCharacter.GetWeaponData(i).GetDurability() <= 0);
				if (!flag)
				{
					bool flag2 = i >= 3 && (hasUsableNormalWeapon || !Config.Character.Instance[this._combatCharacter.GetCharacter().GetTemplateId()].AllowUseFreeWeapon);
					if (flag2)
					{
						break;
					}
					bool flag3 = i < 3;
					if (flag3)
					{
						hasUsableNormalWeapon = true;
					}
					int score = this.CalcWeaponScore(i, skillId, needInRange);
					bool flag4 = score >= 0;
					if (flag4)
					{
						this._weaponScoreDict.Add(i, score);
					}
				}
			}
			bool flag5 = this._weaponScoreDict.Count == 0;
			int result;
			if (flag5)
			{
				result = -1;
			}
			else
			{
				int maxScore = 0;
				List<int> maxScoreIndexList = ObjectPool<List<int>>.Instance.Get();
				maxScoreIndexList.Clear();
				foreach (int score2 in this._weaponScoreDict.Values)
				{
					maxScore = Math.Max(maxScore, score2);
				}
				foreach (int index in this._weaponScoreDict.Keys)
				{
					bool flag6 = this._weaponScoreDict[index] == maxScore;
					if (flag6)
					{
						maxScoreIndexList.Add(index);
					}
				}
				int bestIndex = maxScoreIndexList.GetRandom(random);
				ObjectPool<List<int>>.Instance.Return(maxScoreIndexList);
				result = bestIndex;
			}
			return result;
		}

		// Token: 0x06006869 RID: 26729 RVA: 0x003B5940 File Offset: 0x003B3B40
		public unsafe int CalcAttackSkillScore(IRandomSource random, short skillId, bool requireCanUse = false, int costMaxFrame = -1, int costMaxBreath = -1, int costMaxStance = -1)
		{
			int charId = this._combatCharacter.GetId();
			GameData.Domains.CombatSkill.CombatSkill skillObj = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(charId, skillId));
			CombatSkillItem skillConfig = Config.CombatSkill.Instance[skillId];
			GameData.Domains.Character.Character character = this._combatCharacter.GetCharacter();
			OrganizationInfo orgInfo = character.GetOrganizationInfo();
			Personalities personalities = character.GetPersonalities();
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!this._combatCharacter.IsAlly, false);
			bool flag = requireCanUse && !DomainManager.Combat.GetCombatSkillData(charId, skillId).GetCanUse();
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = costMaxFrame >= 0 && DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(charId, skillId)).GetPrepareTotalProgress() / DomainManager.Combat.GetSkillPrepareSpeed(this._combatCharacter) > costMaxFrame;
				if (flag2)
				{
					result = -1;
				}
				else
				{
					bool flag3 = skillId >= 0 && DomainManager.Combat.CombatSkillDataExist(new CombatSkillKey(charId, skillId)) && DomainManager.Combat.GetCombatSkillData(charId, skillId).GetLeftCdFrame() != 0;
					if (flag3)
					{
						result = -1;
					}
					else
					{
						bool flag4 = costMaxBreath >= 0 || costMaxStance >= 0;
						if (flag4)
						{
							OuterAndInnerInts costBreathStance = DomainManager.Combat.GetSkillCostBreathStance(charId, skillObj);
							bool flag5 = costBreathStance.Outer > costMaxStance || costBreathStance.Inner > costMaxBreath;
							if (flag5)
							{
								return -1;
							}
						}
						bool flag6 = this.Memory.SelfRecord.SkillRecord.ContainsKey(skillId) && this.Memory.SelfRecord.SkillRecord[skillId].Item2 > 1;
						if (flag6)
						{
							result = 0;
						}
						else
						{
							int score = (int)Equipping.CalcCombatSkillScore(skillObj, skillConfig.EquipType, ref personalities, character.GetNeiliType(), orgInfo.OrgTemplateId, character.GetIdealSect(), DomainManager.LegendaryBook.GetCharOwnedBookTypes(character.GetId()));
							sbyte skillInnerRatio = skillObj.GetInnerRatio();
							OuterAndInnerInts enemyPenetrationResists = enemyChar.GetCharacter().GetPenetrationResists();
							int enemyDefendDiff = enemyPenetrationResists.Outer - enemyPenetrationResists.Inner;
							bool flag7 = (enemyDefendDiff > 0) ? (skillInnerRatio >= 50) : ((enemyDefendDiff < 0) ? (skillInnerRatio <= 50) : (skillInnerRatio == 50));
							if (flag7)
							{
								score += 150 * Math.Max((int)skillInnerRatio, (int)(100 - skillInnerRatio)) / 100;
							}
							HitOrAvoidInts charHit = character.GetHitValues();
							HitOrAvoidInts skillHit = skillObj.GetHitValue();
							HitOrAvoidInts hitDistribution = skillObj.GetHitDistribution();
							for (sbyte hitType = 0; hitType < 4; hitType += 1)
							{
								bool flag8 = *(ref hitDistribution.Items.FixedElementField + (IntPtr)hitType * 4) > 0 && *(ref charHit.Items.FixedElementField + (IntPtr)hitType * 4) * *(ref skillHit.Items.FixedElementField + (IntPtr)hitType * 4) / 100 >= *(ref this.Memory.EnemyRecordDict[enemyChar.GetId()].MaxAvoid.Items.FixedElementField + (IntPtr)hitType * 4);
								if (flag8)
								{
									score += *(ref hitDistribution.Items.FixedElementField + (IntPtr)hitType * 4) * 2;
								}
							}
							bool hasAtkAcupointEffect = skillConfig.HasAtkAcupointEffect;
							if (hasAtkAcupointEffect)
							{
								score += 100;
							}
							bool flag9 = orgInfo.OrgTemplateId >= 0 && Organization.Instance[orgInfo.OrgTemplateId].AllowPoisoning;
							if (flag9)
							{
								PoisonsAndLevels poisons = skillConfig.Poisons;
								int poisonScore = 0;
								for (int poisonType = 0; poisonType < 6; poisonType++)
								{
									poisonScore = Math.Max(poisonScore, (int)(*(ref poisons.Values.FixedElementField + (IntPtr)poisonType * 2) * (short)(*(ref poisons.Levels.FixedElementField + poisonType))));
								}
								score += poisonScore;
							}
							int bestWeaponIndex = this.GetBestWeaponIndex(random, skillId, false, -1);
							int rangeWeaponIndex = (DomainManager.CombatSkill.GetSkillType(this._combatCharacter.GetId(), skillId) != 5) ? bestWeaponIndex : 3;
							bool flag10 = rangeWeaponIndex >= 0;
							if (flag10)
							{
								GameData.Domains.Item.Weapon rangeWeapon = DomainManager.Item.GetElement_Weapons(this._combatCharacter.GetWeapons()[rangeWeaponIndex].Id);
								int minIndex = RecordCollection.GetIndexByDistance(rangeWeapon.GetMinDistance() - skillConfig.DistanceAdditionWhenCast);
								int maxIndex = RecordCollection.GetIndexByDistance(rangeWeapon.GetMaxDistance() - skillConfig.DistanceAdditionWhenCast);
								int totalDamage = 0;
								int damageInRange = 0;
								for (int i = 0; i < this.Memory.SelfRecord.MaxDamages.Length; i++)
								{
									OuterAndInnerInts damage = this.Memory.SelfRecord.MaxDamages[i];
									totalDamage += damage.Outer + damage.Inner;
									bool flag11 = minIndex <= i && i <= maxIndex;
									if (flag11)
									{
										damageInRange += damage.Outer + damage.Inner + this.Memory.SelfRecord.MaxMindDamages[i];
									}
								}
								bool flag12 = damageInRange > 0 && totalDamage > 0;
								if (flag12)
								{
									sbyte braveValue = this._combatCharacter.GetPersonalityValue(3);
									int reduceScore = 200 - 200 * (int)braveValue / 100 * damageInRange * 100 / totalDamage;
									score = Math.Max(score - reduceScore, 0);
								}
							}
							bool flag13 = this.Memory.SelfRecord.SkillRecord.ContainsKey(skillId);
							if (flag13)
							{
								score += this.Memory.SelfRecord.SkillRecord[skillId].Item1;
							}
							else
							{
								score += this.Memory.SelfRecord.GetSkillRecordMaxScore();
							}
							bool flag14 = bestWeaponIndex < 0;
							if (flag14)
							{
								score = -1;
							}
							else
							{
								bool flag15 = bestWeaponIndex >= 3;
								if (flag15)
								{
									score = score * 66 / 100;
								}
							}
							result = score;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600686A RID: 26730 RVA: 0x003B5EE4 File Offset: 0x003B40E4
		public unsafe int CalcDefenseSkillScore(short skillId, bool requireCanUse = false, int costMaxFrame = -1, int costMaxBreath = -1, int costMaxStance = -1)
		{
			int charId = this._combatCharacter.GetId();
			GameData.Domains.CombatSkill.CombatSkill skillObj = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(charId, skillId));
			CombatSkillItem skillConfig = Config.CombatSkill.Instance[skillId];
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!this._combatCharacter.IsAlly, false);
			bool flag = requireCanUse && !DomainManager.Combat.GetCombatSkillData(charId, skillId).GetCanUse();
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = costMaxFrame >= 0 && DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(charId, skillId)).GetPrepareTotalProgress() / DomainManager.Combat.GetSkillPrepareSpeed(this._combatCharacter) > costMaxFrame;
				if (flag2)
				{
					result = -1;
				}
				else
				{
					bool flag3 = costMaxBreath >= 0 || costMaxStance >= 0;
					if (flag3)
					{
						OuterAndInnerInts costBreathStance = DomainManager.Combat.GetSkillCostBreathStance(charId, skillObj);
						bool flag4 = costBreathStance.Outer > costMaxStance || costBreathStance.Inner > costMaxBreath;
						if (flag4)
						{
							return -1;
						}
					}
					bool flag5 = enemyChar.GetPreparingSkillId() >= 0;
					sbyte enemyInnerRatio;
					HitOrAvoidInts enemyHits;
					if (flag5)
					{
						CombatSkillKey enemySkillKey = new CombatSkillKey(enemyChar.GetId(), enemyChar.GetPreparingSkillId());
						GameData.Domains.CombatSkill.CombatSkill enemySkill = DomainManager.CombatSkill.GetElement_CombatSkills(enemySkillKey);
						enemyInnerRatio = enemySkill.GetCurrInnerRatio();
						enemyHits = enemySkill.GetHitValue();
					}
					else
					{
						CombatWeaponData enemyWeapon = enemyChar.GetWeaponData(-1);
						HitOrAvoidShorts weaponHits = enemyWeapon.Item.GetHitFactors();
						enemyInnerRatio = enemyWeapon.GetInnerRatio();
						for (int hitType = 0; hitType < 4; hitType++)
						{
							*(ref enemyHits.Items.FixedElementField + (IntPtr)hitType * 4) = (int)(*(ref weaponHits.Items.FixedElementField + (IntPtr)hitType * 2));
						}
					}
					int score = 0;
					score += (int)(skillConfig.AddOuterPenetrateResistOnCast * (short)(100 - enemyInnerRatio) / 300);
					score += (int)(skillConfig.AddInnerPenetrateResistOnCast * (short)enemyInnerRatio / 300);
					bool flag6 = (skillObj.GetBouncePower().Inner > 0 || skillObj.GetBouncePower().Outer > 0) && DomainManager.Combat.GetCurrentDistance() < skillConfig.BounceDistance;
					if (flag6)
					{
						score += (int)(skillConfig.BounceRateOfOuterInjury * (short)(100 - enemyInnerRatio) / 150);
						score += (int)(skillConfig.BounceRateOfInnerInjury * (short)enemyInnerRatio / 150);
					}
					bool flag7 = skillObj.GetFightBackPower() > 0 && DomainManager.Combat.InAttackRange(this._combatCharacter);
					if (flag7)
					{
						score += skillObj.GetFightBackPower() / 2;
					}
					HitOrAvoidInts addAvoid = skillObj.GetAddAvoidValueOnCast();
					for (int hitType2 = 0; hitType2 < 4; hitType2++)
					{
						score += *(ref addAvoid.Items.FixedElementField + (IntPtr)hitType2 * 4) * *(ref enemyHits.Items.FixedElementField + (IntPtr)hitType2 * 4) / 200;
					}
					bool flag8 = score > 0;
					if (flag8)
					{
						GameData.Domains.Character.Character character = this._combatCharacter.GetCharacter();
						Personalities personalities = character.GetPersonalities();
						score += (int)Equipping.CalcCombatSkillScore(skillObj, skillConfig.EquipType, ref personalities, this._combatCharacter.GetNeiliType(), character.GetOrganizationInfo().OrgTemplateId, character.GetIdealSect(), DomainManager.LegendaryBook.GetCharOwnedBookTypes(this._combatCharacter.GetId()));
					}
					result = score;
				}
			}
			return result;
		}

		// Token: 0x0600686B RID: 26731 RVA: 0x003B6214 File Offset: 0x003B4414
		private unsafe int CalcWeaponScore(int weaponIndex, short skillId = -1, bool needInRange = false)
		{
			CombatWeaponData combatWeaponData = this._combatCharacter.GetWeaponData(weaponIndex);
			bool flag = skillId >= 0 && !DomainManager.Combat.WeaponHasNeedTrick(this._combatCharacter, skillId, combatWeaponData);
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				ItemKey weaponKey = this._combatCharacter.GetWeapons()[weaponIndex];
				GameData.Domains.Item.Weapon weapon = DomainManager.Item.GetElement_Weapons(weaponKey.Id);
				WeaponItem weaponConfig = Config.Weapon.Instance[weaponKey.TemplateId];
				int innerRatioAdjustRange = (int)((short)weaponConfig.InnerRatioAdjustRange * this._combatCharacter.GetCharacter().GetInnerRatio() / 100);
				bool isFreeWeapon = weaponKey.TemplateId == 0 || weaponKey.TemplateId == 1 || weaponKey.TemplateId == 2;
				bool flag2 = (!isFreeWeapon && combatWeaponData.GetDurability() <= 0) || (needInRange && !this.InWeaponAttackRange(weaponKey));
				if (flag2)
				{
					result = -1;
				}
				else
				{
					bool flag3 = this.Memory.SelfRecord.WeaponRecord.ContainsKey(weaponKey.Id) && this.Memory.SelfRecord.WeaponRecord[weaponKey.Id].Item2 > 2;
					if (flag3)
					{
						result = 0;
					}
					else
					{
						HitOrAvoidShorts weaponHits = weapon.GetHitFactors(this._combatCharacter.GetId());
						int score = (int)((short)(Config.Weapon.Instance[weaponKey.TemplateId].Grade * 10) * DomainManager.Character.GetItemPower(this._combatCharacter.GetId(), weaponKey) / 100);
						bool flag4 = !isFreeWeapon;
						if (flag4)
						{
							score += 50;
						}
						bool flag5 = this.Memory.SelfRecord.WeaponRecord.ContainsKey(weaponKey.Id);
						if (flag5)
						{
							score += this.Memory.SelfRecord.WeaponRecord[weaponKey.Id].Item1;
						}
						bool flag6 = skillId >= 0;
						if (flag6)
						{
							GameData.Domains.CombatSkill.CombatSkill skillObj = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(this._combatCharacter.GetId(), skillId));
							CombatSkillItem skillConfig = Config.CombatSkill.Instance[skillId];
							sbyte[] weaponTricks = combatWeaponData.GetWeaponTricks();
							int trickScore = 0;
							for (int i = 0; i < skillConfig.TrickCost.Count; i++)
							{
								NeedTrick needTrick = skillConfig.TrickCost[i];
								int hasCount = weaponTricks.CountAll((sbyte type) => type == needTrick.TrickType);
								bool flag7 = (int)Math.Min(needTrick.NeedCount, 2) > hasCount;
								if (flag7)
								{
									trickScore = 0;
									break;
								}
								trickScore += hasCount * 100;
							}
							score += trickScore;
							HitOrAvoidInts hitDistribution = skillObj.GetHitDistribution();
							for (sbyte type3 = 0; type3 < 4; type3 += 1)
							{
								int skillHit = *(ref hitDistribution.Items.FixedElementField + (IntPtr)type3 * 4);
								short weaponHit = *(ref weaponHits.Items.FixedElementField + (IntPtr)type3 * 2);
								bool flag8 = skillHit > 0 && weaponHit != 0;
								if (flag8)
								{
									score += (int)weaponHit * skillHit * 2 / 100;
								}
							}
							score = Math.Max(score, 0);
							sbyte skillInnerRatio = skillObj.GetCurrInnerRatio();
							int minInnerRatio = (int)weaponConfig.DefaultInnerRatio - innerRatioAdjustRange;
							int maxInnerRatio = (int)weaponConfig.DefaultInnerRatio + innerRatioAdjustRange;
							int innerRatioDiff = ((int)skillInnerRatio > maxInnerRatio) ? ((int)skillInnerRatio - maxInnerRatio) : (((int)skillInnerRatio < minInnerRatio) ? (minInnerRatio - (int)skillInnerRatio) : 0);
							score += 50 - Math.Abs(innerRatioDiff / 2);
							bool flag9 = skillConfig.MostFittingWeaponID >= 0;
							if (flag9)
							{
								int idDiff = (int)(weaponKey.TemplateId - skillConfig.MostFittingWeaponID);
								bool flag10 = 0 <= idDiff && idDiff <= 8;
								if (flag10)
								{
									score += 200;
								}
							}
						}
						else
						{
							int hitAddScore = 200;
							HitOrAvoidInts charHits = this._combatCharacter.GetCharacter().GetHitValues();
							List<sbyte> hitTypeList = ObjectPool<List<sbyte>>.Instance.Get();
							Dictionary<int, int> hitValueDict = ObjectPool<Dictionary<int, int>>.Instance.Get();
							hitTypeList.Clear();
							hitValueDict.Clear();
							for (sbyte type2 = 0; type2 < 4; type2 += 1)
							{
								hitTypeList.Add(type2);
								hitValueDict.Add((int)type2, *(ref charHits.Items.FixedElementField + (IntPtr)type2 * 4));
							}
							hitTypeList.Sort((sbyte typeL, sbyte typeR) => hitValueDict[(int)typeR] - hitValueDict[(int)typeL]);
							for (int j = 0; j < hitTypeList.Count; j++)
							{
								bool flag11 = *(ref weaponHits.Items.FixedElementField + (IntPtr)hitTypeList[j] * 2) > 0;
								if (flag11)
								{
									score += hitAddScore;
								}
								hitAddScore -= 50;
							}
							ObjectPool<List<sbyte>>.Instance.Return(hitTypeList);
							ObjectPool<Dictionary<int, int>>.Instance.Return(hitValueDict);
							CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!this._combatCharacter.IsAlly, false);
							OuterAndInnerInts enemyPenetrationResists = this.Memory.EnemyRecordDict[enemyChar.GetId()].MaxPenetrateResist;
							int maxInnerRatio2 = Math.Clamp((int)weaponConfig.DefaultInnerRatio + innerRatioAdjustRange, 0, 100);
							int minInnerRatio2 = Math.Clamp((int)weaponConfig.DefaultInnerRatio - innerRatioAdjustRange, 0, 100);
							bool flag12 = enemyPenetrationResists.Outer > enemyPenetrationResists.Inner;
							if (flag12)
							{
								score += (100 - minInnerRatio2) * 3;
							}
							bool flag13 = enemyPenetrationResists.Inner > enemyPenetrationResists.Outer;
							if (flag13)
							{
								score += maxInnerRatio2 * 3;
							}
							else
							{
								bool flag14 = minInnerRatio2 <= 50 && maxInnerRatio2 >= 50;
								if (flag14)
								{
									score += 300;
								}
							}
							int minIndex = RecordCollection.GetIndexByDistance(weapon.GetMinDistance());
							int maxIndex = RecordCollection.GetIndexByDistance(weapon.GetMaxDistance());
							int baseScore = 200;
							int totalDamage = 0;
							int damageInRange = 0;
							for (int k = 0; k < this.Memory.SelfRecord.MaxDamages.Length; k++)
							{
								OuterAndInnerInts damage = this.Memory.SelfRecord.MaxDamages[k];
								totalDamage += damage.Outer + damage.Inner;
								bool flag15 = minIndex <= k && k <= maxIndex;
								if (flag15)
								{
									damageInRange += damage.Outer + damage.Inner + this.Memory.SelfRecord.MaxMindDamages[k];
								}
							}
							bool flag16 = damageInRange > 0 && totalDamage > 0;
							if (flag16)
							{
								sbyte braveValue = this._combatCharacter.GetPersonalityValue(3);
								int reduceScore = baseScore - baseScore * (int)braveValue / 100 * damageInRange * 100 / totalDamage;
								score = Math.Max(score - reduceScore, 0);
							}
						}
						result = score;
					}
				}
			}
			return result;
		}

		// Token: 0x0600686C RID: 26732 RVA: 0x003B68B4 File Offset: 0x003B4AB4
		public bool InWeaponAttackRange(ItemKey weaponKey)
		{
			ValueTuple<int, int> attackRange = DomainManager.Item.GetWeaponAttackRange(this._combatCharacter.GetId(), weaponKey);
			short currDistance = DomainManager.Combat.GetCurrentDistance();
			return attackRange.Item1 <= (int)currDistance && (int)currDistance <= attackRange.Item2;
		}

		// Token: 0x0600686D RID: 26733 RVA: 0x003B6900 File Offset: 0x003B4B00
		public short GetTargetDistance()
		{
			return this._combatCharacter.AiTargetDistance;
		}

		// Token: 0x0600686E RID: 26734 RVA: 0x003B6920 File Offset: 0x003B4B20
		public void Update(DataContext context)
		{
			bool flag = this.AutoStopJumpInReach();
			if (!flag)
			{
				this._aiTree.Update();
				this.UpdateTargetDistance(context);
			}
		}

		// Token: 0x0600686F RID: 26735 RVA: 0x003B694E File Offset: 0x003B4B4E
		public void UpdateOnlyMove(DataContext context)
		{
			this.UpdateTargetDistance(context);
			this.AutoStopJumpInReach();
		}

		// Token: 0x06006870 RID: 26736 RVA: 0x003B6960 File Offset: 0x003B4B60
		public bool AutoStopJumpInReach()
		{
			bool flag = !this._combatCharacter.KeepMoving || !this._combatCharacter.MoveData.PreparingJumpMove() || (this._combatCharacter.IsAlly && (!DomainManager.Combat.AiOptions.AutoMove || this._combatCharacter.PlayerControllingMove));
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				short currDistance = DomainManager.Combat.GetCurrentDistance();
				short targetDistance = this._combatCharacter.GetTargetDistance();
				bool flag2 = targetDistance < 0 || (this._combatCharacter.MoveForward ? (currDistance <= targetDistance) : (currDistance >= targetDistance)) || !this.MoveCanApproachTargetDistance(targetDistance);
				if (flag2)
				{
					DomainManager.Combat.SetMoveState(0, this._combatCharacter.IsAlly, false);
				}
				else
				{
					bool canPartlyJump = this._combatCharacter.MoveData.CanPartlyJump;
					if (canPartlyJump)
					{
						short preparedDistance = this._combatCharacter.GetJumpPreparedDistance();
						bool flag3 = preparedDistance > 0 && (this._combatCharacter.MoveForward ? (currDistance - preparedDistance <= targetDistance) : (currDistance + preparedDistance >= targetDistance));
						if (flag3)
						{
							DomainManager.Combat.SetMoveState(0, this._combatCharacter.IsAlly, false);
						}
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06006871 RID: 26737 RVA: 0x003B6AA8 File Offset: 0x003B4CA8
		public void UpdateTargetDistance(DataContext context)
		{
			bool playerControllingMove = this._combatCharacter.PlayerControllingMove;
			if (!playerControllingMove)
			{
				bool flag = !this._combatCharacter.IsAlly || DomainManager.Combat.IsAiMoving;
				if (flag)
				{
					this._combatCharacter.SetTargetDistance(this.GetTargetDistance(), context);
				}
				else
				{
					this._combatCharacter.SetTargetDistance(this._combatCharacter.PlayerTargetDistance, context);
				}
				short targetDistance = this._combatCharacter.GetTargetDistance();
				short currDistance = DomainManager.Combat.GetCurrentDistance();
				byte moveState = (targetDistance < 0 || targetDistance == currDistance || !this.MoveCanApproachTargetDistance(targetDistance)) ? 0 : ((targetDistance < currDistance) ? 1 : 2);
				byte currMoveState = (!this._combatCharacter.KeepMoving) ? 0 : (this._combatCharacter.MoveForward ? 1 : 2);
				bool flag2 = moveState != currMoveState;
				if (flag2)
				{
					DomainManager.Combat.SetMoveState(moveState, this._combatCharacter.IsAlly, false);
				}
			}
		}

		// Token: 0x06006872 RID: 26738 RVA: 0x003B6B94 File Offset: 0x003B4D94
		private bool MoveCanApproachTargetDistance(short targetDistance)
		{
			short currDistance = DomainManager.Combat.GetCurrentDistance();
			bool moveForward = targetDistance < currDistance;
			bool flag = !this._combatCharacter.MoveData.IsJumpMove(moveForward);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool isAlly = this._combatCharacter.IsAlly;
				if (isAlly)
				{
					result = (Math.Abs((int)(currDistance - targetDistance)) >= (int)DomainManager.Extra.GetJumpThreshold(this._combatCharacter.MoveData.JumpMoveSkillId));
				}
				else
				{
					int jumpDistance = (int)(this._combatCharacter.MoveData.CanPartlyJump ? 10 : (moveForward ? this._combatCharacter.MoveData.MaxJumpForwardDist : this._combatCharacter.MoveData.MaxJumpBackwardDist));
					result = (Math.Abs((int)currDistance - jumpDistance * (moveForward ? 1 : -1) - (int)targetDistance) < Math.Abs((int)(currDistance - targetDistance)));
				}
			}
			return result;
		}

		// Token: 0x06006873 RID: 26739 RVA: 0x003B6C6C File Offset: 0x003B4E6C
		// Note: this type is marked as 'beforefieldinit'.
		static AiController()
		{
			int[] array = new int[4];
			array[1] = 2;
			array[2] = 4;
			AiController.SpecialMarkAddHazardNeedCount = array;
		}

		// Token: 0x04001C88 RID: 7304
		private readonly CombatCharacter _combatCharacter;

		// Token: 0x04001C89 RID: 7305
		public readonly AiEnvironment Environment;

		// Token: 0x04001C8A RID: 7306
		public readonly AiMemory Memory;

		// Token: 0x04001C8B RID: 7307
		public bool AllowDefense;

		// Token: 0x04001C8C RID: 7308
		public static readonly int[] AddHazardPerMark = new int[]
		{
			150,
			75,
			50,
			150
		};

		// Token: 0x04001C8D RID: 7309
		public static readonly int[] SpecialMarkAddHazardNeedCount;

		// Token: 0x04001C8E RID: 7310
		private int _maxHazardValue;

		// Token: 0x04001C8F RID: 7311
		private int _addHazardPerMark;

		// Token: 0x04001C90 RID: 7312
		private int _specialMarkAddHazardNeedCount;

		// Token: 0x04001C91 RID: 7313
		private DefeatMarkCollection _lastMarks;

		// Token: 0x04001C92 RID: 7314
		private DataUid _defeatMarkUid;

		// Token: 0x04001C93 RID: 7315
		private string _defeatMarkDataHandlerKey;

		// Token: 0x04001C94 RID: 7316
		private const sbyte SkillMaxZeroScoreCount = 1;

		// Token: 0x04001C95 RID: 7317
		private const sbyte WeaponMaxZeroScoreCount = 2;

		// Token: 0x04001C96 RID: 7318
		private readonly Dictionary<short, int> _skillScoreDict = new Dictionary<short, int>();

		// Token: 0x04001C97 RID: 7319
		private readonly Dictionary<int, int> _weaponScoreDict = new Dictionary<int, int>();

		// Token: 0x04001C98 RID: 7320
		private readonly AiTree _aiTree;
	}
}
