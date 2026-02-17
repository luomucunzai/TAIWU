using System;
using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai.PrioritizedAction;
using GameData.Domains.Character.Filters;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.Taiwu.Display;
using GameData.Domains.Taiwu.Display.VillagerRoleArrangement;
using GameData.Domains.World.Notification;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Taiwu.VillagerRole
{
	// Token: 0x02000051 RID: 81
	[SerializableGameData(IsExtensible = true, NotForDisplayModule = true, NoCopyConstructors = true)]
	public class VillagerRoleLiterati : VillagerRoleBase, IVillagerRoleContact, IVillagerRoleInfluence, IVillagerRoleArrangementExecutor, IVillagerRoleSelectLocation
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060013DE RID: 5086 RVA: 0x0013B506 File Offset: 0x00139706
		public override short RoleTemplateId
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060013DF RID: 5087 RVA: 0x0013B509 File Offset: 0x00139709
		public bool IncreaseFavorability
		{
			get
			{
				return this.PositiveAction;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060013E0 RID: 5088 RVA: 0x0013B511 File Offset: 0x00139711
		public int LearnActionRepeatChance
		{
			get
			{
				return GameData.Domains.Taiwu.VillagerRole.SharedMethods.CalculateLiteratiLearnActionRepeatChance(this.Character.GetPersonalities());
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060013E1 RID: 5089 RVA: 0x0013B523 File Offset: 0x00139723
		public int LearnRequestSuccessChanceBonus
		{
			get
			{
				return GameData.Domains.Taiwu.VillagerRole.SharedMethods.CalculateLiteratiLearnRequestSuccessChanceBonus(this.Character.GetPersonalities());
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060013E2 RID: 5090 RVA: 0x0013B535 File Offset: 0x00139735
		public unsafe int ContactFavorabilityChange
		{
			get
			{
				return GameData.Domains.Taiwu.VillagerRole.SharedMethods.CalculateLiteratiContactFavorabilityChange(this.Character.GetPersonalities(), *this.Character.GetLifeSkillAttainments());
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060013E3 RID: 5091 RVA: 0x0013B557 File Offset: 0x00139757
		public int ContactCharacterAmount
		{
			get
			{
				return GameData.Domains.Taiwu.VillagerRole.SharedMethods.CalculateLiteratiContactCharacterAmount(this.Character.GetPersonalities());
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060013E4 RID: 5092 RVA: 0x0013B569 File Offset: 0x00139769
		public int EntertainTargetAmount
		{
			get
			{
				return GameData.Domains.Taiwu.VillagerRole.SharedMethods.CalculateLiteratiEntertainTargetAmount(this.Character.GetPersonalities());
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060013E5 RID: 5093 RVA: 0x0013B57B File Offset: 0x0013977B
		public unsafe int EntertainHappinessChange
		{
			get
			{
				return GameData.Domains.Taiwu.VillagerRole.SharedMethods.CalculateLiteratiEntertainHappinessChange(this.Character.GetPersonalities(), *this.Character.GetLifeSkillAttainments());
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060013E6 RID: 5094 RVA: 0x0013B59D File Offset: 0x0013979D
		public int SecretInformationGainChance
		{
			get
			{
				return GameData.Domains.Taiwu.VillagerRole.SharedMethods.CalculateLiteratiSecretInformationGainChance(this.Character.GetPersonalities());
			}
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x0013B5B0 File Offset: 0x001397B0
		void IVillagerRoleArrangementExecutor.ExecuteArrangementAction(DataContext context, VillagerRoleArrangementAction action)
		{
			int arrangementTemplateId = this.ArrangementTemplateId;
			int num = arrangementTemplateId;
			if (num == 2)
			{
				this.ApplyEntertainAction(context, action);
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060013E8 RID: 5096 RVA: 0x0013B5D8 File Offset: 0x001397D8
		internal int ActionEffectCount
		{
			get
			{
				return VillagerRoleFormulaImpl.Calculate(20, (int)this.Character.GetPersonality(1));
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060013E9 RID: 5097 RVA: 0x0013B5ED File Offset: 0x001397ED
		internal int ActionEffectValue
		{
			get
			{
				return VillagerRoleFormulaImpl.Calculate(21, this.CalcLiteratiLifeSkillMaxAttainment());
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060013EA RID: 5098 RVA: 0x0013B5FC File Offset: 0x001397FC
		internal int ExtraPeopleCount
		{
			get
			{
				return VillagerRoleFormulaImpl.Calculate(22, (int)this.Character.GetPersonality(1));
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060013EB RID: 5099 RVA: 0x0013B611 File Offset: 0x00139811
		internal int RelationChange
		{
			get
			{
				return VillagerRoleFormulaImpl.Calculate(23, this.CalcLiteratiLifeSkillMaxAttainment());
			}
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x0013B620 File Offset: 0x00139820
		private void ApplyEntertainAction(DataContext context, VillagerRoleArrangementAction action)
		{
			Location location = this.Character.GetLocation();
			bool flag = !location.IsValid();
			if (!flag)
			{
				List<short> settlementIds = ObjectPool<List<short>>.Instance.Get();
				settlementIds.Clear();
				DomainManager.Map.GetAreaSettlementIds(location.AreaId, settlementIds, true, true);
				bool flag2 = settlementIds.Count <= 0;
				if (flag2)
				{
					ObjectPool<List<short>>.Instance.Return(settlementIds);
				}
				else
				{
					int taiwuId = DomainManager.Taiwu.GetTaiwuCharId();
					int charId = this.Character.GetId();
					int currDate = DomainManager.World.GetCurrDate();
					LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
					int count = this.ActionEffectCount;
					int value = this.ActionEffectValue;
					short pickedSettlementId = settlementIds[context.Random.Next(settlementIds.Count)];
					Settlement settlement = DomainManager.Organization.GetSettlement(pickedSettlementId);
					short culture = settlement.GetCulture();
					short safety = settlement.GetSafety();
					List<int> extraCharIds = ObjectPool<List<int>>.Instance.Get();
					Dictionary<int, int> favorMap = ObjectPool<Dictionary<int, int>>.Instance.Get();
					int addCount = 0;
					int minusCount = 0;
					int i = 0;
					while (i < count)
					{
						IRandomSource random = context.Random;
						sbyte behaviorType = this.Character.GetBehaviorType();
						if (!true)
						{
						}
						int num;
						switch (behaviorType)
						{
						case 0:
							num = 75;
							break;
						case 1:
							num = 100;
							break;
						case 2:
							goto IL_158;
						case 3:
							num = 0;
							break;
						case 4:
							num = 25;
							break;
						default:
							goto IL_158;
						}
						IL_15E:
						if (!true)
						{
						}
						int sign = random.CheckPercentProb(num) ? 1 : -1;
						extraCharIds.Clear();
						bool flag3 = context.Random.NextBool();
						if (flag3)
						{
							settlement.ChangeSafety(context, sign * value);
						}
						else
						{
							settlement.ChangeCulture(context, sign * value);
						}
						bool hasChickenUpgradeEffect = base.HasChickenUpgradeEffect;
						if (hasChickenUpgradeEffect)
						{
							OrgMemberCollection memberCollection = settlement.GetMembers();
							bool flag4 = memberCollection != null;
							if (flag4)
							{
								List<int> members = ObjectPool<List<int>>.Instance.Get();
								members.Clear();
								memberCollection.GetAllMembers(members);
								foreach (int id in members)
								{
									extraCharIds.Add(id);
								}
								ObjectPool<List<int>>.Instance.Return(members);
							}
						}
						bool flag5 = extraCharIds.Count > 0;
						if (flag5)
						{
							int peopleCount = this.ExtraPeopleCount;
							int num2 = Math.Max(this.RelationChange, 0);
							IRandomSource random2 = context.Random;
							sbyte behaviorType2 = this.Character.GetBehaviorType();
							if (!true)
							{
							}
							switch (behaviorType2)
							{
							case 0:
								num = 75;
								break;
							case 1:
								num = 100;
								break;
							case 2:
								goto IL_2A9;
							case 3:
								num = 0;
								break;
							case 4:
								num = 25;
								break;
							default:
								goto IL_2A9;
							}
							IL_2AF:
							if (!true)
							{
							}
							int relationChange = num2 * (random2.CheckPercentProb(num) ? 1 : -1);
							extraCharIds.Remove(DomainManager.Taiwu.GetTaiwuCharId());
							CollectionUtils.Shuffle<int>(context.Random, extraCharIds);
							int diff = extraCharIds.Count - peopleCount;
							bool flag6 = diff > 0;
							if (flag6)
							{
								extraCharIds.RemoveRange(0, diff);
							}
							GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
							foreach (int targetId in extraCharIds)
							{
								GameData.Domains.Character.Character target;
								bool flag7 = DomainManager.Character.TryGetElement_Objects(targetId, out target);
								if (flag7)
								{
									DomainManager.Character.ChangeFavorabilityOptional(context, target, taiwu, relationChange, 5);
									favorMap.TryAdd(targetId, 0);
									Dictionary<int, int> dictionary = favorMap;
									num = targetId;
									dictionary[num] += relationChange;
								}
							}
							goto IL_399;
							IL_2A9:
							num = 50;
							goto IL_2AF;
						}
						IL_399:
						i++;
						continue;
						IL_158:
						num = 50;
						goto IL_15E;
					}
					short cultureNow = settlement.GetCulture();
					short safetyNow = settlement.GetSafety();
					bool flag8 = safetyNow > safety;
					if (flag8)
					{
						lifeRecordCollection.AddLiteratiSpreadingInfluenceSafetyUp(charId, currDate, pickedSettlementId);
					}
					else
					{
						bool flag9 = safetyNow < safety;
						if (flag9)
						{
							lifeRecordCollection.AddLiteratiSpreadingInfluenceSafetyDown(charId, currDate, pickedSettlementId);
						}
					}
					bool flag10 = cultureNow > culture;
					if (flag10)
					{
						lifeRecordCollection.AddLiteratiSpreadingInfluenceCultureUp(charId, currDate, pickedSettlementId);
					}
					else
					{
						bool flag11 = cultureNow < culture;
						if (flag11)
						{
							lifeRecordCollection.AddLiteratiSpreadingInfluenceCultureDown(charId, currDate, pickedSettlementId);
						}
					}
					foreach (KeyValuePair<int, int> keyValuePair in favorMap)
					{
						int num;
						int num3;
						keyValuePair.Deconstruct(out num, out num3);
						int id2 = num;
						int delta = num3;
						bool flag12 = delta < 0;
						if (flag12)
						{
							minusCount++;
							lifeRecordCollection.AddLiteratiBeConnectedRelationshipDown(id2, currDate, charId, pickedSettlementId);
						}
						else
						{
							bool flag13 = delta > 0;
							if (flag13)
							{
								addCount++;
								lifeRecordCollection.AddLiteratiBeConnectedRelationshipUp(id2, currDate, charId, pickedSettlementId);
							}
						}
					}
					bool flag14 = addCount > 0;
					if (flag14)
					{
						lifeRecordCollection.AddLiteratiConnectRelationshipUp(charId, currDate, pickedSettlementId, addCount);
						lifeRecordCollection.AddLiteratiConnectRelationshipUpTaiwu(taiwuId, currDate, charId, pickedSettlementId);
					}
					bool flag15 = minusCount > 0;
					if (flag15)
					{
						lifeRecordCollection.AddLiteratiConnectRelationshipDown(charId, currDate, pickedSettlementId, minusCount);
						lifeRecordCollection.AddLiteratiConnectRelationshipDownTaiwu(taiwuId, currDate, charId, pickedSettlementId);
					}
					ObjectPool<List<int>>.Instance.Return(extraCharIds);
					ObjectPool<List<short>>.Instance.Return(settlementIds);
					ObjectPool<Dictionary<int, int>>.Instance.Return(favorMap);
					CharacterDomain.AddLockMovementCharSet(charId);
				}
			}
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x0013BB8C File Offset: 0x00139D8C
		public override void ExecuteFixedAction(DataContext context)
		{
			bool flag = this.ArrangementTemplateId >= 0;
			if (!flag)
			{
				bool flag2 = this.WorkData != null && this.WorkData.WorkType == 1;
				if (!flag2)
				{
					this.TryAddNextAutoTravelTarget(context, new Predicate<MapBlockData>(this.AutoActionBlockFilter));
					this.AutoWriteAndDrawAction(context);
				}
			}
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x0013BBE8 File Offset: 0x00139DE8
		private bool AutoActionBlockFilter(MapBlockData blockData)
		{
			MapDomain mapDomain = DomainManager.Map;
			Location location = this.Character.GetLocation();
			bool result;
			if (location.IsValid() && mapDomain.GetStateIdByAreaId(location.AreaId) == mapDomain.GetStateIdByAreaId(blockData.AreaId))
			{
				HashSet<int> characterSet = blockData.CharacterSet;
				result = (characterSet != null && characterSet.Count >= 3);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x0013BC58 File Offset: 0x00139E58
		private void AutoWriteAndDrawAction(DataContext context)
		{
			Location location = this.Character.GetLocation();
			MapBlockData block = DomainManager.Map.GetBlock(location);
			int count = VillagerRoleFormulaImpl.Calculate(18, (int)this.Character.GetPersonality(2));
			ObjectPool<List<int>> listPool = ObjectPool<List<int>>.Instance;
			List<int> range = listPool.Get();
			range.Clear();
			range.AddRange(block.CharacterSet);
			range.Remove(this.Character.GetId());
			CollectionUtils.Shuffle<int>(context.Random, range);
			int rangeDiff = range.Count - count;
			bool flag = rangeDiff > 0;
			if (flag)
			{
				range.RemoveRange(0, rangeDiff);
			}
			int delta = VillagerRoleFormulaImpl.Calculate(19, this.CalcLiteratiLifeSkillMaxAttainment());
			int charId = this.Character.GetId();
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int addCount = 0;
			int minusCount = 0;
			foreach (int targetCharId in range)
			{
				GameData.Domains.Character.Character targetChar = DomainManager.Character.GetElement_Objects(targetCharId);
				IRandomSource random = context.Random;
				sbyte behaviorType = this.Character.GetBehaviorType();
				if (!true)
				{
				}
				int percentProb;
				switch (behaviorType)
				{
				case 0:
					percentProb = 75;
					break;
				case 1:
					percentProb = 100;
					break;
				case 2:
					goto IL_141;
				case 3:
					percentProb = 0;
					break;
				case 4:
					percentProb = 25;
					break;
				default:
					goto IL_141;
				}
				IL_147:
				if (!true)
				{
				}
				int sign = random.CheckPercentProb(percentProb) ? 1 : -1;
				int value = delta * sign;
				targetChar.ChangeHappiness(context, value);
				bool flag2 = value > 0;
				if (flag2)
				{
					addCount++;
					lifeRecordCollection.AddLiteratiBeEntertainedUp(targetCharId, currDate, charId, location);
				}
				else
				{
					bool flag3 = value < 0;
					if (flag3)
					{
						minusCount++;
						lifeRecordCollection.AddLiteratiBeEntertainedDown(targetCharId, currDate, charId, location);
					}
				}
				continue;
				IL_141:
				percentProb = 50;
				goto IL_147;
			}
			bool flag4 = addCount > 0;
			if (flag4)
			{
				lifeRecordCollection.AddLiteratiEntertainingUp(charId, currDate, location, addCount);
			}
			bool flag5 = minusCount > 0;
			if (flag5)
			{
				lifeRecordCollection.AddLiteratiEntertainingDown(charId, currDate, location, minusCount);
			}
			listPool.Return(range);
			CharacterDomain.AddLockMovementCharSet(charId);
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x0013BE8C File Offset: 0x0013A08C
		private void ApplyChickenUpgradeEffect(DataContext context, List<GameData.Domains.Character.Character> targets)
		{
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			HashSet<int> taiwuSecretInfos = new HashSet<int>();
			DomainManager.Information.GetSecretInformationOfCharacter(taiwuSecretInfos, taiwuCharId, true);
			List<int> charSecrets = new List<int>();
			List<ValueTuple<int, int>> allPotentialSecrets = new List<ValueTuple<int, int>>();
			foreach (GameData.Domains.Character.Character targetChar in targets)
			{
				int targetCharId = targetChar.GetId();
				DomainManager.Information.GetSecretInformationOfCharacter(charSecrets, targetCharId, false);
				foreach (int metaDataId in charSecrets)
				{
					bool flag = !taiwuSecretInfos.Contains(metaDataId);
					if (flag)
					{
						allPotentialSecrets.Add(new ValueTuple<int, int>(targetCharId, metaDataId));
					}
				}
			}
			bool flag2 = allPotentialSecrets.Count == 0;
			if (!flag2)
			{
				ValueTuple<int, int> secret = allPotentialSecrets.GetRandom(context.Random);
				DomainManager.Information.ReceiveSecretInformation(context, secret.Item2, taiwuCharId, secret.Item1);
				MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
				monthlyNotifications.AddChickenSecretInformation(this.Character.GetId(), this.Character.GetLocation(), secret.Item1);
			}
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x0013BFE8 File Offset: 0x0013A1E8
		public void SelectContactTargets(IRandomSource random, List<GameData.Domains.Character.Character> selectedCharList, int selectAmount)
		{
			VillagerRoleLiterati.<>c__DisplayClass33_0 CS$<>8__locals1 = new VillagerRoleLiterati.<>c__DisplayClass33_0();
			CS$<>8__locals1.<>4__this = this;
			selectedCharList.Clear();
			Location location = this.Character.GetLocation();
			CS$<>8__locals1.currBlock = DomainManager.Map.GetBlock(location);
			bool flag = CS$<>8__locals1.currBlock.BelongBlockId < 0;
			if (!flag)
			{
				MapCharacterFilter.Find(new Predicate<GameData.Domains.Character.Character>(CS$<>8__locals1.<SelectContactTargets>g__CharacterFilter|0), selectedCharList, location.AreaId, false);
				bool flag2 = selectedCharList.Count > selectAmount;
				if (flag2)
				{
					selectedCharList.RemoveRange(selectAmount, selectedCharList.Count - selectAmount);
				}
			}
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x0013C074 File Offset: 0x0013A274
		void IVillagerRoleInfluence.ApplyInfluenceAction(DataContext context)
		{
			int valueChange = this.InfluenceSettlementValueChange;
			bool flag = !this.PositiveAction;
			if (flag)
			{
				valueChange = -valueChange;
			}
			short areaId = this.Character.GetLocation().AreaId;
			MapAreaData areaData = DomainManager.Map.GetElement_Areas((int)areaId);
			int authorityGain = 0;
			foreach (SettlementInfo settlementInfo in areaData.SettlementInfos)
			{
				bool flag2 = settlementInfo.SettlementId < 0;
				if (!flag2)
				{
					Settlement settlement = DomainManager.Organization.GetSettlement(settlementInfo.SettlementId);
					settlement.ChangeCulture(context, valueChange);
					authorityGain += this.GetSettlementInfluenceAuthorityGain(settlement);
				}
			}
			DomainManager.Taiwu.GetTaiwu().ChangeResource(context, 7, authorityGain);
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060013F3 RID: 5107 RVA: 0x0013C130 File Offset: 0x0013A330
		public unsafe int InfluenceSettlementValueChange
		{
			get
			{
				return GameData.Domains.Taiwu.VillagerRole.SharedMethods.CalculateLiteratiInfluenceSettlementValueChange(this.Character.GetPersonalities(), *this.Character.GetLifeSkillAttainments());
			}
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x0013C154 File Offset: 0x0013A354
		public int GetSettlementInfluenceAuthorityGain(Settlement settlement)
		{
			bool flag = settlement == null;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int safetyValue = (int)(this.PositiveAction ? settlement.GetCulture() : (settlement.GetMaxCulture() - settlement.GetCulture()));
				result = safetyValue * (int)(50 + this.Character.GetPersonality(2) / 2) / 100;
			}
			return result;
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x0013C1A6 File Offset: 0x0013A3A6
		private static IEnumerable<sbyte> CalcLiteratiLifeSkillTypes()
		{
			yield return 0;
			yield return 1;
			yield return 2;
			yield return 3;
			yield break;
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x0013C1B0 File Offset: 0x0013A3B0
		private unsafe int CalcLiteratiLifeSkillMaxAttainment()
		{
			LifeSkillShorts items = *this.Character.GetLifeSkillAttainments();
			short max = short.MinValue;
			foreach (sbyte lifeSkillType in VillagerRoleLiterati.CalcLiteratiLifeSkillTypes())
			{
				bool flag = *items[(int)lifeSkillType] > max;
				if (flag)
				{
					max = *items[(int)lifeSkillType];
				}
			}
			return (int)max;
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x0013C234 File Offset: 0x0013A434
		public override IVillagerRoleArrangementDisplayData GetArrangementDisplayData()
		{
			return new EntertainingDisplayData
			{
				ActionEffectCount = this.ActionEffectCount,
				ActionEffectValue = this.ActionEffectValue,
				ExtraPeopleCount = this.ExtraPeopleCount,
				RelationChange = this.RelationChange
			};
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x0013C27C File Offset: 0x0013A47C
		public override bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x0013C290 File Offset: 0x0013A490
		public override int GetSerializedSize()
		{
			int totalSize = 7;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x0013C2B4 File Offset: 0x0013A4B4
		public unsafe override int Serialize(byte* pData)
		{
			*(short*)pData = 2;
			byte* pCurrData = pData + 2;
			*(int*)pCurrData = this.ArrangementTemplateId;
			pCurrData += 4;
			*pCurrData = (this.PositiveAction ? 1 : 0);
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x0013C300 File Offset: 0x0013A500
		public unsafe override int Deserialize(byte* pData)
		{
			ushort fieldCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = fieldCount > 0;
			if (flag)
			{
				this.ArrangementTemplateId = *(int*)pCurrData;
				pCurrData += 4;
			}
			bool flag2 = fieldCount > 1;
			if (flag2)
			{
				this.PositiveAction = (*pCurrData != 0);
				pCurrData++;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0400031D RID: 797
		[SerializableGameDataField]
		public bool PositiveAction = true;

		// Token: 0x02000962 RID: 2402
		private static class FieldIds
		{
			// Token: 0x0400275E RID: 10078
			public const ushort ArrangementTemplateId = 0;

			// Token: 0x0400275F RID: 10079
			public const ushort PositiveAction = 1;

			// Token: 0x04002760 RID: 10080
			public const ushort Count = 2;

			// Token: 0x04002761 RID: 10081
			public static readonly string[] FieldId2FieldName = new string[]
			{
				"ArrangementTemplateId",
				"PositiveAction"
			};
		}
	}
}
