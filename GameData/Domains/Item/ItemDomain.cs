using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using Config;
using Config.ConfigCells.Character;
using GameData.ArchiveData;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Dependencies;
using GameData.DLC;
using GameData.DLC.FiveLoong;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Character.Relation;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Extra;
using GameData.Domains.Global;
using GameData.Domains.Item.Display;
using GameData.Domains.Map;
using GameData.Domains.Taiwu;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Domains.World;
using GameData.Domains.World.Notification;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using NLog;
using Redzen.Random;

namespace GameData.Domains.Item
{
	// Token: 0x02000669 RID: 1641
	[GameDataDomain(6)]
	public class ItemDomain : BaseGameDataDomain
	{
		// Token: 0x06004F94 RID: 20372 RVA: 0x002B51F8 File Offset: 0x002B33F8
		[DomainMethod]
		public List<ItemDisplayData> CatchCricket(DataContext context, short colorId, short partId, short singLevel, sbyte cricketPlaceId)
		{
			List<ItemDisplayData> itemList = new List<ItemDisplayData>();
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			sbyte level = Math.Max(CricketParts.Instance[colorId].Level, CricketParts.Instance[partId].Level);
			int successOdds = (int)(10 + singLevel) - Math.Min((int)(level * 5), 40);
			bool success = singLevel >= ((CricketParts.Instance[colorId].MustSuccessLoud || CricketParts.Instance[partId].MustSuccessLoud) ? 80 : GlobalConfig.Instance.CatchCricketSuccessSingLevel);
			bool flag = !success;
			if (flag)
			{
				success = context.Random.CheckPercentProb((singLevel >= 80) ? successOdds : (successOdds / 2));
			}
			bool flag2 = success;
			if (flag2)
			{
				ItemKey itemKey = DomainManager.Item.CreateCricket(context, colorId, partId);
				Cricket cricket = DomainManager.Item.GetElement_Crickets(itemKey.Id);
				DomainManager.Taiwu.SetCricketLuckPoint(DomainManager.Taiwu.GetCricketLuckPoint() + cricket.CalcCatchLucky(), context);
				DomainManager.Taiwu.AddLegacyPoint(context, 32, 100 + Math.Abs(cricket.CalcCatchLucky()) * 5);
				taiwu.AddInventoryItem(context, itemKey, 1, false);
				this.AddCatchCricketProfessionSeniority(context, cricket);
				bool flag3 = context.Random.CheckPercentProb((int)(cricket.GetGrade() * 2));
				if (flag3)
				{
					cricket.SetCurrDurability(cricket.GetCurrDurability() - 1, context);
					ItemKey extraItemKey = DomainManager.Item.CreateMisc(context, 25);
					taiwu.AddInventoryItem(context, extraItemKey, 1, false);
					itemList.Add(DomainManager.Item.GetItemDisplayData(DomainManager.Item.GetBaseItem(extraItemKey), 1, -1, -1));
				}
				else
				{
					bool flag4 = cricket.GetGrade() > 0 && context.Random.CheckPercentProb(10);
					if (flag4)
					{
						ItemKey extraItemKey2 = DomainManager.Item.CreateCricket(context, (short)(cricket.GetGrade() - 1));
						Cricket extraCricket = DomainManager.Item.GetElement_Crickets(extraItemKey2.Id);
						cricket.SetCurrDurability(cricket.GetMaxDurability() / 2, context);
						extraCricket.SetCurrDurability(extraCricket.GetMaxDurability() / 2, context);
						taiwu.AddInventoryItem(context, extraItemKey2, 1, false);
						itemList.Add(DomainManager.Item.GetItemDisplayData(DomainManager.Item.GetBaseItem(extraItemKey2), 1, -1, -1));
						this.AddCatchCricketProfessionSeniority(context, extraCricket);
					}
				}
				itemList.Insert(0, DomainManager.Item.GetItemDisplayData(cricket, 1, -1, -1));
			}
			else
			{
				bool flag5 = singLevel >= 80;
				if (flag5)
				{
					short[] uselessItems = CricketPlace.Instance[cricketPlaceId].UselessItemList;
					short templateId = uselessItems[context.Random.Next(uselessItems.Length)];
					ItemKey itemKey2 = DomainManager.Item.CreateItem(context, 12, templateId);
					taiwu.AddInventoryItem(context, itemKey2, 1, false);
					itemList.Add(DomainManager.Item.GetItemDisplayData(DomainManager.Item.GetBaseItem(itemKey2), 1, -1, -1));
				}
			}
			return itemList;
		}

		// Token: 0x06004F95 RID: 20373 RVA: 0x002B54E8 File Offset: 0x002B36E8
		public void AddCatchCricketProfessionSeniority(DataContext context, Cricket cricket)
		{
			ProfessionFormulaItem formula = ProfessionFormula.Instance[106];
			int addSeniority = formula.Calculate(cricket.GetValue());
			DomainManager.Extra.ChangeProfessionSeniority(context, 17, addSeniority, true, false);
		}

		// Token: 0x06004F96 RID: 20374 RVA: 0x002B5524 File Offset: 0x002B3724
		[DomainMethod]
		public CricketData GetCricketData(int itemId)
		{
			Cricket cricket = DomainManager.Item.GetElement_Crickets(itemId);
			return new CricketData
			{
				Injuries = cricket.GetInjuries(),
				WinsCount = cricket.GetWinsCount(),
				LossesCount = cricket.GetLossesCount(),
				BestEnemyColorId = cricket.GetBestEnemyColorId(),
				BestEnemyPartId = cricket.GetBestEnemyPartId(),
				Age = (short)cricket.GetAge(),
				MaxAge = (short)cricket.CalcMaxAge(),
				IsSmart = DomainManager.Extra.IsCricketSmart(itemId),
				IsIdentified = DomainManager.Extra.IsCricketIdentified(itemId),
				CircketValue = cricket.GetValue()
			};
		}

		// Token: 0x06004F97 RID: 20375 RVA: 0x002B55CC File Offset: 0x002B37CC
		[DomainMethod]
		public List<CricketData> GetCricketDataList(List<ItemKey> itemList)
		{
			List<CricketData> dataList = new List<CricketData>();
			bool flag = itemList == null;
			List<CricketData> result;
			if (flag)
			{
				result = dataList;
			}
			else
			{
				for (int i = 0; i < itemList.Count; i++)
				{
					CricketData data = this.GetCricketData(itemList[i].Id);
					dataList.Add(data);
				}
				result = dataList;
			}
			return result;
		}

		// Token: 0x06004F98 RID: 20376 RVA: 0x002B5628 File Offset: 0x002B3828
		[DomainMethod]
		public void SetCricketRecord(DataContext context, int itemId, bool win, int enemyItemId)
		{
			Cricket cricket;
			bool flag = !DomainManager.Item.TryGetElement_Crickets(itemId, out cricket);
			if (!flag)
			{
				if (win)
				{
					Cricket enemyCricket;
					bool flag2 = !DomainManager.Item.TryGetElement_Crickets(enemyItemId, out enemyCricket);
					if (!flag2)
					{
						bool flag3 = cricket.GetGrade() - enemyCricket.GetGrade() > 2;
						if (!flag3)
						{
							int bestEnemyGrade = (int)((cricket.GetBestEnemyColorId() > 0) ? Math.Max(CricketParts.Instance[cricket.GetBestEnemyColorId()].Level, CricketParts.Instance[cricket.GetBestEnemyPartId()].Level) : 0);
							cricket.SetWinsCount(cricket.GetWinsCount() + 1, context);
							bool flag4 = (int)enemyCricket.GetGrade() >= bestEnemyGrade;
							if (flag4)
							{
								cricket.SetBestEnemyColorId(enemyCricket.GetColorId(), context);
								cricket.SetBestEnemyPartId(enemyCricket.GetPartId(), context);
							}
							ProfessionFormulaItem formula = ProfessionFormula.Instance[107];
							int addSeniority = formula.Calculate((int)enemyCricket.GetGrade());
							DomainManager.Extra.ChangeProfessionSeniority(context, 17, addSeniority, true, false);
						}
					}
				}
				else
				{
					cricket.SetLossesCount(cricket.GetLossesCount() + 1, context);
				}
			}
		}

		// Token: 0x06004F99 RID: 20377 RVA: 0x002B5754 File Offset: 0x002B3954
		[DomainMethod]
		public void AddCricketInjury(DataContext context, int itemId, int index, short value)
		{
			Cricket cricket = DomainManager.Item.GetElement_Crickets(itemId);
			short[] injuries = cricket.GetInjuries();
			short[] array = injuries;
			array[index] += value;
			cricket.SetInjuries(injuries, context);
		}

		// Token: 0x06004F9A RID: 20378 RVA: 0x002B578C File Offset: 0x002B398C
		[DomainMethod]
		public void SetCricketBattleConfig(sbyte minGrade, sbyte maxGrade, bool onlyNoInjuryCricket)
		{
			this._minCricketGrade = minGrade;
			this._maxCricketGrade = maxGrade;
			this._onlyNoInjuryCricket = onlyNoInjuryCricket;
		}

		// Token: 0x06004F9B RID: 20379 RVA: 0x002B57A4 File Offset: 0x002B39A4
		public List<CricketWagerData> SelectCricketWagers(DataContext context, int enemyId)
		{
			this._cricketBattleEnemyId = enemyId;
			this._cricketBattleSelfWager.Type = -1;
			this._cricketBattleEnemyWager.Type = -1;
			GameData.Domains.Character.Character enemy = DomainManager.Character.GetElement_Objects(enemyId);
			List<CricketWagerData> result = new List<CricketWagerData>();
			foreach (Wager wager in this.CalcEnemyWagers(context.Random, enemy))
			{
				long wagerValue = this.GetWagerValue(wager);
				CricketWagerData data = new CricketWagerData
				{
					Wager = wager,
					Crickets = this.GetNpcCricketDisplayDataListForCricketBattle(context, enemyId, this._cricketBattleEnemyCrickets, wager.Grade),
					MinWagerValue = ((wagerValue == 0L) ? 0L : Math.Max(wagerValue, (long)ItemDomain.WagerValueUnit))
				};
				data.PreRandomizedShowCricketIndex = (byte)context.Random.Next(data.Crickets.Count);
				result.Add(data);
			}
			return result;
		}

		// Token: 0x06004F9C RID: 20380 RVA: 0x002B58A8 File Offset: 0x002B3AA8
		public List<ItemDisplayData> GetNpcCricketDisplayDataListForCricketBattle(DataContext context, int charId, List<ItemKey> tempCreateCricketKeyList, sbyte wagerGrade = -1)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			sbyte orgGrade = character.GetOrganizationInfo().Grade;
			bool flag = wagerGrade < 0;
			if (flag)
			{
				wagerGrade = orgGrade;
			}
			short attainment = character.GetLifeSkillAttainment(15);
			List<ItemDisplayData> result = new List<ItemDisplayData>();
			List<sbyte> grades = ObjectPool<List<sbyte>>.Instance.Get();
			grades.AddRange(CricketGenerator.Generate(orgGrade, wagerGrade, (int)attainment));
			this.FillInventoryCricket(charId, grades, result);
			foreach (sbyte grade in grades)
			{
				sbyte finalGrade = Math.Clamp(grade, this._minCricketGrade, this._maxCricketGrade);
				short templateId = (short)finalGrade;
				ItemKey cricketKey = DomainManager.Item.CreateCricket(context, templateId, finalGrade == 8);
				bool onlyNoInjuryCricket = this._onlyNoInjuryCricket;
				if (onlyNoInjuryCricket)
				{
					Cricket cricket = DomainManager.Item.GetElement_Crickets(cricketKey.Id);
					short[] injuries = cricket.GetInjuries();
					Array.Clear(injuries, 0, injuries.Length);
					cricket.SetInjuries(injuries, context);
				}
				tempCreateCricketKeyList.Add(cricketKey);
				result.Add(DomainManager.Item.GetItemDisplayData(cricketKey, -1));
			}
			ObjectPool<List<sbyte>>.Instance.Return(grades);
			CollectionUtils.Shuffle<ItemDisplayData>(context.Random, result);
			return result;
		}

		// Token: 0x06004F9D RID: 20381 RVA: 0x002B5A08 File Offset: 0x002B3C08
		private void FillInventoryCricket(int charId, IList<sbyte> grades, List<ItemDisplayData> crickets)
		{
			sbyte minGrade = grades.Min<sbyte>();
			List<ItemDisplayData> inventoryCrickets = DomainManager.Character.GetInventoryItems(charId, 1100);
			inventoryCrickets.RemoveAll(delegate(ItemDisplayData data)
			{
				bool flag = data.Durability <= 0;
				bool result;
				if (flag)
				{
					result = true;
				}
				else
				{
					sbyte grade = ItemTemplateHelper.GetCricketGrade(data.CricketColorId, data.CricketPartId);
					bool flag2 = grade < this._minCricketGrade || grade > this._maxCricketGrade || grade < minGrade;
					if (flag2)
					{
						result = true;
					}
					else
					{
						bool flag3 = this._onlyNoInjuryCricket && this.GetElement_Crickets(data.Key.Id).GetInjuries().Sum() > 0;
						result = flag3;
					}
				}
				return result;
			});
			inventoryCrickets.Sort((ItemDisplayData lhs, ItemDisplayData rhs) => ItemDomain.<FillInventoryCricket>g__GetGrade|17_0(lhs).CompareTo(ItemDomain.<FillInventoryCricket>g__GetGrade|17_0(rhs)));
			while (inventoryCrickets.Count > grades.Count)
			{
				inventoryCrickets.RemoveAt(0);
			}
			crickets.AddRange(inventoryCrickets);
			for (int i = 0; i < inventoryCrickets.Count; i++)
			{
				minGrade = grades.Min<sbyte>();
				grades.Remove(minGrade);
			}
		}

		// Token: 0x06004F9E RID: 20382 RVA: 0x002B5AD0 File Offset: 0x002B3CD0
		public void SetWager(Wager selfWager, Wager enemyWager)
		{
			this._cricketBattleSelfWager = selfWager;
			this._cricketBattleEnemyWager = enemyWager;
		}

		// Token: 0x06004F9F RID: 20383 RVA: 0x002B5AE4 File Offset: 0x002B3CE4
		[DomainMethod]
		public bool SettlementCricketWagerByGiveUp(DataContext context, bool win)
		{
			return this.SettlementCricketWager(context, win, null, null);
		}

		// Token: 0x06004FA0 RID: 20384 RVA: 0x002B5B00 File Offset: 0x002B3D00
		[DomainMethod]
		public bool SettlementCricketWager(DataContext context, bool win, ItemKey[] taiwuCricketKeys, short[] durabilityList)
		{
			GameData.Domains.Character.Character enemy = DomainManager.Character.GetElement_Objects(this._cricketBattleEnemyId);
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			bool flag = taiwuCricketKeys != null && taiwuCricketKeys.Length > 0 && durabilityList != null && durabilityList.Length > 0;
			if (flag)
			{
				for (int i = 0; i < taiwuCricketKeys.Length; i++)
				{
					TaiwuEventDomain taiwuEvent = DomainManager.TaiwuEvent;
					string actionName = "CricketCombatOver";
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 1);
					defaultInterpolatedStringHandler.AppendLiteral("SelfCricket");
					defaultInterpolatedStringHandler.AppendFormatted<int>(i);
					taiwuEvent.SetListenerEventActionISerializableArg(actionName, defaultInterpolatedStringHandler.ToStringAndClear(), taiwuCricketKeys[i]);
					Cricket cricket = DomainManager.Item.GetElement_Crickets(taiwuCricketKeys[i].Id);
					cricket.SetCurrDurability(durabilityList[i], context);
				}
			}
			if (win)
			{
				this.TransferWager(context, enemy, taiwuChar, this._cricketBattleEnemyWager);
			}
			else
			{
				bool flag2 = this._cricketBattleSelfWager.Type != 2;
				if (flag2)
				{
					this.TransferWager(context, taiwuChar, enemy, this._cricketBattleSelfWager);
				}
				else
				{
					this.TaiwuTransferCharacterWager(context);
				}
			}
			int selfHappiness = win ? GlobalConfig.Instance.OtherCombatWinHappiness[(int)taiwuChar.GetBehaviorType()] : GlobalConfig.Instance.OtherCombatLoseHappiness[(int)taiwuChar.GetBehaviorType()];
			int favorabilityToEnemy = win ? GlobalConfig.Instance.OtherCombatWinFavorability[(int)taiwuChar.GetBehaviorType()] : GlobalConfig.Instance.OtherCombatLoseFavorability[(int)taiwuChar.GetBehaviorType()];
			int enemyHappiness = win ? GlobalConfig.Instance.OtherCombatLoseHappiness[(int)enemy.GetBehaviorType()] : GlobalConfig.Instance.OtherCombatWinHappiness[(int)enemy.GetBehaviorType()];
			int favorabilityToSelf = win ? GlobalConfig.Instance.OtherCombatLoseFavorability[(int)enemy.GetBehaviorType()] : GlobalConfig.Instance.OtherCombatWinFavorability[(int)enemy.GetBehaviorType()];
			taiwuChar.ChangeHappiness(context, selfHappiness);
			enemy.ChangeHappiness(context, enemyHappiness);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, taiwuChar, enemy, favorabilityToEnemy);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, enemy, taiwuChar, favorabilityToSelf);
			Events.RaiseCricketCombatFinished(context, win);
			bool flag3 = win && taiwuCricketKeys != null;
			if (flag3)
			{
				int taiwuCricketGradeSum = 0;
				foreach (ItemKey cricket2 in taiwuCricketKeys)
				{
					taiwuCricketGradeSum += (int)Cricket.Instance[cricket2.TemplateId].Grade;
				}
				int enemyCricketGradeSum = 0;
				foreach (ItemKey cricket3 in this._cricketBattleEnemyCrickets)
				{
					enemyCricketGradeSum += (int)Cricket.Instance[cricket3.TemplateId].Grade;
				}
				bool flag4 = taiwuCricketGradeSum <= enemyCricketGradeSum + 3;
				if (flag4)
				{
					DomainManager.Taiwu.AddLegacyPoint(context, 33, 100);
				}
			}
			foreach (ItemKey itemKey in this._cricketBattleEnemyCrickets)
			{
				DomainManager.Item.RemoveItem(context, itemKey);
			}
			this._cricketBattleEnemyCrickets.Clear();
			this._minCricketGrade = 0;
			this._maxCricketGrade = 8;
			this._onlyNoInjuryCricket = false;
			return win;
		}

		// Token: 0x06004FA1 RID: 20385 RVA: 0x002B5E50 File Offset: 0x002B4050
		private void TaiwuTransferCharacterWager(DataContext context)
		{
			GameData.Domains.Character.Character enemy = DomainManager.Character.GetElement_Objects(this._cricketBattleEnemyId);
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			int taiwuCharId = taiwuChar.GetId();
			bool flag = ExternalRelationStateHelper.IsActive(taiwuChar.GetExternalRelationState(), 2);
			if (flag)
			{
				KidnappedCharacterList kidnappedCharList = DomainManager.Character.GetKidnappedCharacters(taiwuCharId);
				int targetIndex = kidnappedCharList.IndexOf(this._cricketBattleSelfWager.CharId);
				bool flag2 = targetIndex >= 0;
				if (flag2)
				{
					DomainManager.Character.TransferKidnappedCharacter(context, this._cricketBattleEnemyId, taiwuChar.GetId(), kidnappedCharList.Get(targetIndex));
					return;
				}
			}
			bool flag3 = this._cricketBattleSelfWager.CharId == taiwuCharId;
			if (flag3)
			{
				EventHelper.TriggerLegacyPassingEvent(true, "");
			}
			else
			{
				bool flag4 = DomainManager.Taiwu.IsInGroup(this._cricketBattleSelfWager.CharId);
				if (flag4)
				{
					ItemKey rope = enemy.GetInventoryRope(context, enemy.GetOrganizationInfo().Grade);
					DomainManager.Character.AddKidnappedCharacter(context, this._cricketBattleEnemyId, this._cricketBattleSelfWager.CharId, rope);
				}
			}
		}

		// Token: 0x06004FA2 RID: 20386 RVA: 0x002B5F5C File Offset: 0x002B415C
		public void TransferWager(DataContext context, GameData.Domains.Character.Character srcChar, GameData.Domains.Character.Character destChar, Wager wager)
		{
			switch (wager.Type)
			{
			case 0:
			{
				bool flag = wager.Count > 0;
				if (flag)
				{
					DomainManager.Character.TransferResource(context, srcChar, destChar, wager.WagerResourceType, wager.Count);
				}
				break;
			}
			case 1:
			{
				bool flag2 = wager.Count > 0;
				if (flag2)
				{
					DomainManager.Character.TransferInventoryItem(context, srcChar, destChar, wager.ItemKey, wager.Count);
				}
				break;
			}
			case 2:
			{
				int winnerId = destChar.GetId();
				int loserId = srcChar.GetId();
				KidnappedCharacterList kidnappedCharList = DomainManager.Character.GetKidnappedCharacters(srcChar.GetId());
				KidnappedCharacter kidnappedChar = kidnappedCharList.Get(kidnappedCharList.IndexOf(wager.CharId));
				DomainManager.Character.TransferKidnappedCharacter(context, winnerId, loserId, kidnappedChar);
				break;
			}
			case 3:
			{
				bool flag3 = wager.Count > 0;
				if (flag3)
				{
					destChar.ChangeExp(context, wager.Count);
				}
				break;
			}
			}
		}

		// Token: 0x06004FA3 RID: 20387 RVA: 0x002B6058 File Offset: 0x002B4258
		public bool CheckCharacterHasWager(GameData.Domains.Character.Character character, Wager wager)
		{
			bool result;
			switch (wager.Type)
			{
			case 0:
				result = (character.GetResource(wager.WagerResourceType) >= wager.Count);
				break;
			case 1:
			{
				int amount;
				result = (character.GetInventory().Items.TryGetValue(wager.ItemKey, out amount) && amount >= wager.Count);
				break;
			}
			case 2:
			{
				GameData.Domains.Character.Character kidnappedChar;
				result = (DomainManager.Character.TryGetElement_Objects(wager.CharId, out kidnappedChar) && kidnappedChar.GetKidnapperId() == character.GetId());
				break;
			}
			case 3:
				result = true;
				break;
			default:
				result = false;
				break;
			}
			return result;
		}

		// Token: 0x06004FA4 RID: 20388 RVA: 0x002B6104 File Offset: 0x002B4304
		public bool NpcHasAnyCricketWager(int charId)
		{
			IRandomSource random = DataContextManager.GetCurrentThreadDataContext().Random;
			GameData.Domains.Character.Character enemy = DomainManager.Character.GetElement_Objects(charId);
			return this.CalcEnemyWagers(random, enemy).Any<Wager>();
		}

		// Token: 0x06004FA5 RID: 20389 RVA: 0x002B613C File Offset: 0x002B433C
		public Wager SelectCharacterValidWager(DataContext context, GameData.Domains.Character.Character character)
		{
			List<Wager> validWagers = context.AdvanceMonthRelatedData.Wagers.Occupy();
			ItemDomain.GetCharacterValidWagers(context.Random, character, validWagers);
			Wager wager = validWagers.GetRandomOrDefault(context.Random, Wager.CreateResource(6, 0));
			context.AdvanceMonthRelatedData.Wagers.Release(ref validWagers);
			Tester.Assert(this.CheckCharacterHasWager(character, wager), "");
			return wager;
		}

		// Token: 0x06004FA6 RID: 20390 RVA: 0x002B61A8 File Offset: 0x002B43A8
		public static void GetCharacterValidWagers(IRandomSource random, GameData.Domains.Character.Character character, List<Wager> validWagers)
		{
			validWagers.Clear();
			OrganizationInfo orgInfo = character.GetOrganizationInfo();
			Vector2 valueRange = Wager.BehaviorValueRange[(int)character.GetBehaviorType()];
			OrganizationItem orgCfg = Organization.Instance[orgInfo.OrgTemplateId];
			sbyte grade = orgInfo.Grade;
			do
			{
				short orgMemberId = orgCfg.Members[(int)grade];
				OrganizationMemberItem orgMemberCfg = OrganizationMember.Instance[orgMemberId];
				int expectedValue = orgMemberCfg.ExpectedWagerValue;
				Dictionary<ItemKey, int> items = character.GetInventory().Items;
				float minValue = Math.Max(valueRange.X * (float)expectedValue, 1f);
				float maxValue = Math.Max(valueRange.Y * (float)expectedValue, minValue);
				foreach (KeyValuePair<ItemKey, int> itemEntry in items)
				{
					int itemValue = DomainManager.Item.GetValue(itemEntry.Key);
					bool flag = minValue <= (float)itemValue && (float)itemValue <= maxValue;
					if (flag)
					{
						for (int i = 0; i < 3; i++)
						{
							validWagers.Add(Wager.CreateItem(itemEntry.Key, 1));
						}
					}
				}
				for (sbyte type = 0; type < 8; type += 1)
				{
					int resourceCount = character.GetResource(type);
					sbyte resourceWorth = GlobalConfig.ResourcesWorth[(int)type];
					short resourceUnit = GlobalConfig.UnitsOfResourceTransfer[(int)type];
					minValue = Math.Max(valueRange.X * (float)expectedValue, (float)((short)resourceWorth * resourceUnit));
					maxValue = Math.Max(valueRange.Y * (float)expectedValue, minValue);
					int minUnitCount = (int)(minValue / (float)resourceWorth / (float)resourceUnit);
					int maxUnitCount = (int)(Math.Min(maxValue / (float)resourceWorth, (float)resourceCount) / (float)resourceUnit);
					bool flag2 = resourceCount / (int)resourceUnit >= minUnitCount;
					if (flag2)
					{
						Wager wager = Wager.CreateResource(type, (int)resourceUnit * random.Next(minUnitCount, maxUnitCount + 1));
						for (int j = 0; j < (int)Wager.ResourceRandomWeight[(int)type]; j++)
						{
							validWagers.Add(wager);
						}
					}
				}
				grade -= 1;
			}
			while (validWagers.Count == 0 && grade >= 0);
		}

		// Token: 0x06004FA7 RID: 20391 RVA: 0x002B63DC File Offset: 0x002B45DC
		private IEnumerable<Wager> CalcEnemyWagers(IRandomSource random, GameData.Domains.Character.Character character)
		{
			ItemDomain.<>c__DisplayClass27_0 CS$<>8__locals1 = new ItemDomain.<>c__DisplayClass27_0();
			CS$<>8__locals1.<>4__this = this;
			sbyte charGrade = character.GetOrganizationInfo().Grade;
			sbyte taiwuFame = DomainManager.Taiwu.GetTaiwu().GetFame();
			ValueTuple<sbyte, sbyte> valueTuple = CricketSpecialConstants.CalcWagerGradeRange(charGrade, taiwuFame);
			CS$<>8__locals1.minGrade = valueTuple.Item1;
			CS$<>8__locals1.maxGrade = valueTuple.Item2;
			List<ItemKey> itemPool = ObjectPool<List<ItemKey>>.Instance.Get();
			Dictionary<ItemKey, int> items = character.GetInventory().Items;
			foreach (Func<ItemKey, bool> matcher in CricketSpecialConstants.WagerItemMatchers)
			{
				ItemDomain.<>c__DisplayClass27_1 CS$<>8__locals2 = new ItemDomain.<>c__DisplayClass27_1();
				itemPool.Clear();
				itemPool.AddRange(items.Keys.Where(matcher).Where(new Func<ItemKey, bool>(CS$<>8__locals1.<CalcEnemyWagers>g__GradeMatcher|0)));
				List<ItemKey> list = itemPool;
				Predicate<ItemKey> match;
				if ((match = CS$<>8__locals1.<>9__1) == null)
				{
					match = (CS$<>8__locals1.<>9__1 = ((ItemKey x) => CS$<>8__locals1.<>4__this.GetBaseItem(x).GetValue() < 1));
				}
				list.RemoveAll(match);
				bool flag = itemPool.Count == 0;
				if (!flag)
				{
					CS$<>8__locals2.highestGrade = itemPool.Max((ItemKey x) => ItemTemplateHelper.GetGrade(x.ItemType, x.TemplateId));
					itemPool.RemoveAll((ItemKey x) => ItemTemplateHelper.GetGrade(x.ItemType, x.TemplateId) < CS$<>8__locals2.highestGrade);
					ItemKey itemKey = itemPool.GetRandom(random);
					yield return Wager.CreateItem(itemKey, 1);
					CS$<>8__locals2 = null;
					itemKey = default(ItemKey);
					matcher = null;
				}
			}
			IEnumerator<Func<ItemKey, bool>> enumerator = null;
			ObjectPool<List<ItemKey>>.Instance.Return(itemPool);
			List<sbyte> resourcePool = ObjectPool<List<sbyte>>.Instance.Get();
			List<sbyte> resourceGrades = ObjectPool<List<sbyte>>.Instance.Get();
			sbyte b;
			for (sbyte resourceType = 0; resourceType < 8; resourceType = b + 1)
			{
				int resourceCount = character.GetResource(resourceType);
				for (sbyte resourceGrade = CS$<>8__locals1.maxGrade; resourceGrade >= CS$<>8__locals1.minGrade; resourceGrade = b - 1)
				{
					int gradeCount = CricketSpecialConstants.GradeToPriceResource(resourceType, resourceGrade);
					bool flag2 = gradeCount > resourceCount;
					if (!flag2)
					{
						resourcePool.Add(resourceType);
						resourceGrades.Add(resourceGrade);
						break;
					}
					b = resourceGrade;
				}
				b = resourceType;
			}
			foreach (sbyte resourceType2 in RandomUtils.GetRandomUnrepeated<sbyte>(random, 3, resourcePool, null))
			{
				int index = resourcePool.IndexOf(resourceType2);
				sbyte grade = resourceGrades[index];
				int count = CricketSpecialConstants.GradeToPriceResource(resourceType2, grade);
				yield return Wager.CreateResource(resourceType2, count);
			}
			IEnumerator<sbyte> enumerator2 = null;
			ObjectPool<List<sbyte>>.Instance.Return(resourcePool);
			ObjectPool<List<sbyte>>.Instance.Return(resourceGrades);
			int exp = CricketSpecialConstants.GradeToPriceExp(CS$<>8__locals1.maxGrade / 2);
			yield return Wager.CreateExp(exp);
			yield break;
			yield break;
		}

		// Token: 0x06004FA8 RID: 20392 RVA: 0x002B63FC File Offset: 0x002B45FC
		private long GetWagerValue(Wager wager)
		{
			sbyte type = wager.Type;
			bool flag = type == 0 || type == 3;
			bool flag2 = flag;
			long result;
			if (flag2)
			{
				result = wager.CalcWagerValue(0, 0, 0, 0, -1, 0);
			}
			else
			{
				bool flag3 = wager.Type == 1;
				if (flag3)
				{
					result = wager.CalcWagerValue(this.GetValue(wager.ItemKey), 0, 0, 0, -1, 0);
				}
				else
				{
					bool flag4 = wager.Type == 2;
					if (flag4)
					{
						GameData.Domains.Character.Character wagerChar = DomainManager.Character.GetElement_Objects(wager.CharId);
						result = wager.CalcWagerValue(0, wagerChar.GetFame(), wagerChar.GetAttraction(), wagerChar.GetPhysiologicalAge(), wagerChar.GetAvatar().Gender, wagerChar.GetOrganizationInfo().Grade);
					}
					else
					{
						bool flag5 = wager.Type == 3;
						if (flag5)
						{
							result = wager.CalcWagerValue(0, 0, 0, 0, -1, 0);
						}
						else
						{
							result = 0L;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06004FA9 RID: 20393 RVA: 0x002B64E4 File Offset: 0x002B46E4
		[DomainMethod]
		public void MakeCricketRebirth(DataContext ctx, ItemKey itemKey)
		{
			Cricket cricket;
			bool flag = itemKey.ItemType == 11 && this.TryGetElement_Crickets(itemKey.Id, out cricket);
			if (flag)
			{
				cricket.Rebirth(ctx);
				ProfessionFormulaItem formula = ProfessionFormula.Instance[110];
				int addSeniority = formula.Calculate(cricket.GetValue());
				DomainManager.Extra.ChangeProfessionSeniority(ctx, 17, addSeniority, true, false);
			}
		}

		// Token: 0x06004FAA RID: 20394 RVA: 0x002B6548 File Offset: 0x002B4748
		public List<int> GetAllCricketIdList()
		{
			return new List<int>(this._crickets.Keys);
		}

		// Token: 0x06004FAB RID: 20395 RVA: 0x002B656C File Offset: 0x002B476C
		public ItemKey CreateCricketByLuckPoint(DataContext context, ref int luckPoint, int simulateCount = 1)
		{
			List<ValueTuple<short, short>> weightTable = context.AdvanceMonthRelatedData.WeightTable.Occupy();
			short maxColorId = 0;
			short maxPartId = 0;
			sbyte maxLevel = 0;
			for (int i = 0; i < simulateCount; i++)
			{
				ValueTuple<short, short> tuple = ItemDomain.SimulateCricketByLuckPoint(context.Random, luckPoint, weightTable);
				luckPoint += tuple.CalcCatchLucky();
				sbyte level = tuple.CalcLevel();
				bool flag = level < maxLevel || (level == maxLevel && context.Random.CheckPercentProb(50));
				if (!flag)
				{
					ValueTuple<short, short> valueTuple = tuple;
					maxColorId = valueTuple.Item1;
					maxPartId = valueTuple.Item2;
				}
			}
			context.AdvanceMonthRelatedData.WeightTable.Release(ref weightTable);
			return this.CreateCricket(context, maxColorId, maxPartId);
		}

		// Token: 0x06004FAC RID: 20396 RVA: 0x002B6624 File Offset: 0x002B4824
		[return: TupleElementNames(new string[]
		{
			"colorId",
			"partId"
		})]
		private static ValueTuple<short, short> SimulateCricketByLuckPoint(IRandomSource random, int luckPoint, List<ValueTuple<short, short>> weightTable)
		{
			weightTable.Clear();
			foreach (CricketPlaceItem item in ((IEnumerable<CricketPlaceItem>)CricketPlace.Instance))
			{
				weightTable.Add(new ValueTuple<short, short>((short)item.TemplateId, (short)item.PlaceRate));
			}
			short placeId = RandomUtils.GetRandomResult<short>(weightTable, random);
			CricketPlaceItem placeConfig = CricketPlace.Instance[(int)placeId];
			weightTable.Clear();
			weightTable.Add(new ValueTuple<short, short>(4, (short)placeConfig.Cyan));
			weightTable.Add(new ValueTuple<short, short>(5, (short)placeConfig.Yellow));
			weightTable.Add(new ValueTuple<short, short>(6, (short)placeConfig.Purple));
			weightTable.Add(new ValueTuple<short, short>(7, (short)placeConfig.Red));
			weightTable.Add(new ValueTuple<short, short>(8, (short)placeConfig.Black));
			weightTable.Add(new ValueTuple<short, short>(9, (short)placeConfig.White));
			weightTable.Add(new ValueTuple<short, short>(0, (short)placeConfig.Trash));
			ECricketPartsType baseColorType = (ECricketPartsType)RandomUtils.GetRandomResult<short>(weightTable, random);
			bool flag = baseColorType == ECricketPartsType.Trash;
			ValueTuple<short, short> result;
			if (flag)
			{
				result = new ValueTuple<short, short>(0, 0);
			}
			else
			{
				weightTable.Clear();
				foreach (CricketPartsItem item2 in ((IEnumerable<CricketPartsItem>)CricketParts.Instance))
				{
					bool flag2 = item2.Type == baseColorType;
					if (flag2)
					{
						weightTable.Add(new ValueTuple<short, short>(item2.TemplateId, (short)item2.Rate));
					}
				}
				short selectedColorId = RandomUtils.GetRandomResult<short>(weightTable, random);
				bool isKing = random.Next(21) == 0;
				bool flag3 = isKing && random.CheckPercentProb((int)CricketParts.Instance[selectedColorId].AdvanceRate * luckPoint / 100);
				if (flag3)
				{
					bool flag4 = random.CheckPercentProb(luckPoint / 10);
					short cricketKingId;
					if (flag4)
					{
						weightTable.Clear();
						foreach (CricketPartsItem item3 in ((IEnumerable<CricketPartsItem>)CricketParts.Instance))
						{
							bool flag5 = item3.Type == ECricketPartsType.King;
							if (flag5)
							{
								weightTable.Add(new ValueTuple<short, short>(item3.TemplateId, (short)item3.Rate));
							}
						}
						cricketKingId = RandomUtils.GetRandomResult<short>(weightTable, random);
					}
					else
					{
						if (!true)
						{
						}
						short num;
						switch (baseColorType)
						{
						case ECricketPartsType.Cyan:
							num = 22;
							break;
						case ECricketPartsType.Yellow:
							num = 23;
							break;
						case ECricketPartsType.Purple:
							num = 24;
							break;
						case ECricketPartsType.Red:
							num = 25;
							break;
						case ECricketPartsType.Black:
							num = 26;
							break;
						case ECricketPartsType.White:
							num = 27;
							break;
						default:
							num = 0;
							break;
						}
						if (!true)
						{
						}
						cricketKingId = num;
					}
					result = new ValueTuple<short, short>(cricketKingId, 0);
				}
				else
				{
					weightTable.Clear();
					foreach (CricketPartsItem item4 in ((IEnumerable<CricketPartsItem>)CricketParts.Instance))
					{
						bool flag6 = item4.Type == ECricketPartsType.Parts;
						if (flag6)
						{
							weightTable.Add(new ValueTuple<short, short>(item4.TemplateId, (short)item4.Rate));
						}
					}
					short selectedPartId = RandomUtils.GetRandomResult<short>(weightTable, random);
					result = new ValueTuple<short, short>(selectedColorId, selectedPartId);
				}
			}
			return result;
		}

		// Token: 0x06004FAD RID: 20397 RVA: 0x002B6974 File Offset: 0x002B4B74
		[DomainMethod]
		public List<sbyte> GetWeaponTricks(ItemKey weaponKey)
		{
			Weapon weapon;
			bool flag = this.TryGetElement_Weapons(weaponKey.Id, out weapon);
			List<sbyte> tricks;
			if (flag)
			{
				tricks = weapon.GetTricks();
			}
			else
			{
				tricks = Weapon.Instance[weaponKey.TemplateId].Tricks;
			}
			return tricks;
		}

		// Token: 0x06004FAE RID: 20398 RVA: 0x002B69B8 File Offset: 0x002B4BB8
		[DomainMethod]
		[return: TupleElementNames(new string[]
		{
			"minDist",
			"maxDist"
		})]
		public ValueTuple<int, int> GetWeaponAttackRange(int charId, ItemKey weaponKey)
		{
			Weapon weapon;
			bool flag = !this.TryGetElement_Weapons(weaponKey.Id, out weapon);
			ValueTuple<int, int> result;
			if (flag)
			{
				WeaponItem weaponCfg = Weapon.Instance[weaponKey.TemplateId];
				result = new ValueTuple<int, int>((int)weaponCfg.MinDistance, (int)weaponCfg.MaxDistance);
			}
			else
			{
				ValueTuple<int, int> range = new ValueTuple<int, int>((int)weapon.GetMinDistance(), (int)weapon.GetMaxDistance());
				int addValue = DomainManager.SpecialEffect.GetModifyValue(charId, 29, EDataModifyType.Add, (int)weaponKey.ItemType, (int)weaponKey.TemplateId, weaponKey.Id, EDataSumType.All);
				range.Item1 = Math.Max(range.Item1 - addValue, 20);
				range.Item2 = Math.Min(range.Item2 + addValue, 120);
				result = range;
			}
			return result;
		}

		// Token: 0x06004FAF RID: 20399 RVA: 0x002B6A70 File Offset: 0x002B4C70
		[DomainMethod]
		public int GetWeaponPrepareFrame(int charId, ItemKey weaponKey)
		{
			Weapon weapon;
			bool flag = !this._weapons.TryGetValue(weaponKey.Id, out weapon);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = DomainManager.Combat.IsCharInCombat(charId, true);
				if (flag2)
				{
					CombatCharacter combatChar = DomainManager.Combat.GetElement_CombatCharacterDict(charId);
					result = combatChar.CalcNormalAttackStartupFrames(weapon);
				}
				else
				{
					int startupFrames = Weapon.Instance[weapon.GetTemplateId()].BaseStartupFrames;
					GameData.Domains.Character.Character character;
					bool flag3 = DomainManager.Character.TryGetElement_Objects(charId, out character);
					if (flag3)
					{
						result = weapon.CalcAttackStartupOrRecoveryFrame((int)character.GetAttackSpeed(), startupFrames);
					}
					else
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Try to get ");
						defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(weaponKey);
						defaultInterpolatedStringHandler.AppendLiteral(" prepare frame with a invalid charId ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
						AdaptableLog.Warning(defaultInterpolatedStringHandler.ToStringAndClear(), false);
						result = weapon.CalcAttackStartupOrRecoveryFrame(100, startupFrames);
					}
				}
			}
			return result;
		}

		// Token: 0x06004FB0 RID: 20400 RVA: 0x002B6B5C File Offset: 0x002B4D5C
		[DomainMethod]
		public short[] GetCricketCombatRecords(ItemKey cricketKey)
		{
			Cricket cricket = DomainManager.Item.GetElement_Crickets(cricketKey.Id);
			return new short[]
			{
				cricket.GetWinsCount(),
				cricket.GetLossesCount()
			};
		}

		// Token: 0x06004FB1 RID: 20401 RVA: 0x002B6B98 File Offset: 0x002B4D98
		[DomainMethod]
		public ItemDisplayData GetItemDisplayData(ItemKey itemKey, int charId = -1)
		{
			bool flag = itemKey.Id == -1;
			if (flag)
			{
				itemKey = this.CreateItem(DomainManager.TaiwuEvent.MainThreadDataContext, itemKey.ItemType, itemKey.TemplateId);
				short predefinedLogId = 12;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot use GetItemDisplayData for create item, ");
				defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(itemKey);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
				PredefinedLog.Show(predefinedLogId, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			ItemBase item = this.TryGetBaseItem(itemKey);
			bool flag2 = item != null;
			ItemDisplayData result;
			if (flag2)
			{
				result = DomainManager.Item.GetItemDisplayData(item, 1, charId, -1);
			}
			else
			{
				Logger logger = ItemDomain.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(60, 1);
				defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(itemKey);
				defaultInterpolatedStringHandler.AppendLiteral(" try to get deleted item display data through pure template.");
				logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				result = new ItemDisplayData(itemKey.ItemType, itemKey.TemplateId);
			}
			return result;
		}

		// Token: 0x06004FB2 RID: 20402 RVA: 0x002B6C84 File Offset: 0x002B4E84
		[Obsolete("注意此方法不会合并有毒物品！！！可以用GetItemDisplayDataListOptional替代")]
		[DomainMethod]
		public List<ItemDisplayData> GetItemDisplayDataList(List<ItemKey> itemKeyList, int charId = -1)
		{
			return this.GetItemDisplayDataListOptional(itemKeyList, charId, -1, false);
		}

		// Token: 0x06004FB3 RID: 20403 RVA: 0x002B6CA0 File Offset: 0x002B4EA0
		[DomainMethod]
		public List<ItemDisplayData> GetItemDisplayDataListOptional(List<ItemKey> itemKeyList, int charId = -1, sbyte itemSourceType = -1, bool merge = false)
		{
			List<ItemDisplayData> dataList = null;
			bool flag = itemKeyList != null && itemKeyList.Count > 0;
			if (flag)
			{
				if (merge)
				{
					Dictionary<ItemKey, int> dict = itemKeyList.GroupBy((ItemKey i) => i, (ItemKey key, [Nullable(1)] IEnumerable<ItemKey> keys) => new
					{
						key = key,
						amount = keys.Count<ItemKey>()
					}).ToDictionary(g => g.key, g => g.amount);
					dataList = CharacterDomain.GetItemDisplayData(charId, dict, (ItemSourceType)itemSourceType);
				}
				else
				{
					dataList = new List<ItemDisplayData>();
					foreach (ItemKey itemKey in itemKeyList)
					{
						ItemBase itemBase = this.TryGetBaseItem(itemKey);
						ItemDisplayData itemData = (itemBase != null) ? DomainManager.Item.GetItemDisplayData(itemBase, 1, charId, itemSourceType) : new ItemDisplayData(itemKey.ItemType, itemKey.TemplateId);
						bool flag2 = itemBase == null && !ItemTemplateHelper.IsThanksLetter(itemKey.ItemType, itemKey.TemplateId);
						if (flag2)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Try get not exist item display data by ");
							defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(itemKey);
							AdaptableLog.Warning(defaultInterpolatedStringHandler.ToStringAndClear(), false);
						}
						dataList.Add(itemData);
					}
				}
			}
			return dataList;
		}

		// Token: 0x06004FB4 RID: 20404 RVA: 0x002B6E50 File Offset: 0x002B5050
		[DomainMethod]
		public List<ItemDisplayData> GetItemDisplayDataListOptionalFromInventory(Inventory inventory, int charId = -1, sbyte itemSourceType = -1, bool merge = false)
		{
			List<ItemDisplayData> dataList = null;
			bool flag = ((inventory != null) ? inventory.Items : null) != null && inventory.Items.Count > 0;
			if (flag)
			{
				if (merge)
				{
					dataList = CharacterDomain.GetItemDisplayData(charId, inventory.Items, (ItemSourceType)itemSourceType);
				}
				else
				{
					dataList = new List<ItemDisplayData>();
					foreach (KeyValuePair<ItemKey, int> keyValuePair in inventory.Items)
					{
						ItemKey itemKey2;
						int num;
						keyValuePair.Deconstruct(out itemKey2, out num);
						ItemKey itemKey = itemKey2;
						int amount = num;
						ItemDisplayData itemData = DomainManager.Item.GetItemDisplayData(this.GetBaseItem(itemKey), amount, charId, itemSourceType);
						dataList.Add(itemData);
					}
				}
			}
			return dataList;
		}

		// Token: 0x06004FB5 RID: 20405 RVA: 0x002B6F28 File Offset: 0x002B5128
		[DomainMethod]
		public SkillBookPageDisplayData GetSkillBookPagesInfo(ItemKey itemKey)
		{
			bool flag = !this.ItemExists(itemKey);
			SkillBookPageDisplayData result;
			if (flag)
			{
				SkillBookItem configData = SkillBook.Instance[itemKey.TemplateId];
				bool isCombatSkill = configData.ItemSubType == 1001;
				result = new SkillBookPageDisplayData
				{
					ItemKey = itemKey,
					State = new sbyte[isCombatSkill ? 6 : 5],
					ReadingProgress = new sbyte[isCombatSkill ? 6 : 5],
					Type = new sbyte[isCombatSkill ? 6 : 5]
				};
			}
			else
			{
				SkillBook book = this.GetElement_SkillBooks(itemKey.Id);
				SkillBookPageDisplayData data = new SkillBookPageDisplayData();
				ushort pageState = book.GetPageIncompleteState();
				byte pageTypes = book.GetPageTypes();
				data.ItemKey = itemKey;
				bool flag2 = SkillGroup.FromItemSubType(book.GetItemSubType()) == 0;
				if (flag2)
				{
					short skillTemplateId = book.GetLifeSkillTemplateId();
					TaiwuLifeSkill lifeSkill;
					bool flag3 = DomainManager.Taiwu.TryGetTaiwuLifeSkill(skillTemplateId, out lifeSkill);
					if (flag3)
					{
						data.ReadingProgress = lifeSkill.GetAllBookPageReadingProgress();
					}
					else
					{
						TaiwuLifeSkill notLearnLifeSkill;
						bool flag4 = DomainManager.Taiwu.TryGetNotLearnLifeSkillReadingProgress(skillTemplateId, out notLearnLifeSkill);
						if (flag4)
						{
							data.ReadingProgress = notLearnLifeSkill.GetAllBookPageReadingProgress();
						}
						else
						{
							data.ReadingProgress = new sbyte[5];
						}
					}
					data.Type = new sbyte[5];
				}
				else
				{
					data.Type = new sbyte[6];
					data.Type[0] = SkillBookStateHelper.GetOutlinePageType(pageTypes);
					for (byte i = 1; i < 6; i += 1)
					{
						data.Type[(int)i] = SkillBookStateHelper.GetNormalPageType(pageTypes, i);
					}
					short skillTemplateId2 = book.GetCombatSkillTemplateId();
					sbyte[] readingProgress = null;
					TaiwuCombatSkill combatSkill;
					bool flag5 = DomainManager.Taiwu.TryGetElement_CombatSkills(skillTemplateId2, out combatSkill);
					if (flag5)
					{
						readingProgress = combatSkill.GetAllBookPageReadingProgress();
					}
					else
					{
						TaiwuCombatSkill notLearnCombatSkill;
						bool flag6 = DomainManager.Taiwu.TryGetNotLearnCombatSkillReadingProgress(skillTemplateId2, out notLearnCombatSkill);
						if (flag6)
						{
							readingProgress = notLearnCombatSkill.GetAllBookPageReadingProgress();
						}
					}
					data.ReadingProgress = new sbyte[6];
					bool flag7 = readingProgress != null;
					if (flag7)
					{
						byte j = 0;
						while ((int)j < data.ReadingProgress.Length)
						{
							data.ReadingProgress[(int)j] = readingProgress[(int)CombatSkillStateHelper.GetPageInternalIndex(data.Type[0], data.Type[(int)j], j)];
							j += 1;
						}
					}
				}
				data.State = new sbyte[data.ReadingProgress.Length];
				byte k = 0;
				while ((int)k < data.ReadingProgress.Length)
				{
					data.State[(int)k] = SkillBookStateHelper.GetPageIncompleteState(pageState, k);
					k += 1;
				}
				result = data;
			}
			return result;
		}

		// Token: 0x06004FB6 RID: 20406 RVA: 0x002B7198 File Offset: 0x002B5398
		[DomainMethod]
		public List<SkillBookPageDisplayData> GetSkillBookPageDisplayDataList(List<ItemKey> itemKeyList)
		{
			List<SkillBookPageDisplayData> skillBookPageDisplayDataList = new List<SkillBookPageDisplayData>();
			foreach (ItemKey itemKey in itemKeyList)
			{
				bool flag = itemKey.IsValid();
				if (flag)
				{
					skillBookPageDisplayDataList.Add(this.GetSkillBookPagesInfo(itemKey));
				}
			}
			return skillBookPageDisplayDataList;
		}

		// Token: 0x06004FB7 RID: 20407 RVA: 0x002B720C File Offset: 0x002B540C
		public ItemDisplayData GetItemDisplayData(ItemBase item, int amount = 1, int charId = -1, sbyte itemSourceType = -1)
		{
			ItemKey itemKey = item.GetItemKey();
			LoveTokenDataItem loveTokenDataItem;
			ItemDisplayData itemDisplayData = new ItemDisplayData(itemKey, amount)
			{
				Durability = item.GetCurrDurability(),
				MaxDurability = item.GetMaxDurability(),
				Weight = item.GetWeight(),
				Value = DomainManager.Item.GetValue(itemKey),
				OwnerCharId = charId,
				ItemSourceType = itemSourceType,
				CarrierTamePoint = DomainManager.Extra.GetCarrierTamePoint(itemKey.Id),
				IsReadingFinished = (itemKey.ItemType == 10 && DomainManager.Taiwu.GetTotalReadingProgress(itemKey.Id) >= 100),
				IsThreeCorpseKeepingLegendaryBook = DomainManager.Extra.IsThreeCorpseKeepingLegendaryBook(itemKey),
				LoveTokenDataItem = (DomainManager.Extra.TryGetLoveTokenData(itemKey, out loveTokenDataItem) ? loveTokenDataItem : new LoveTokenDataItem())
			};
			bool flag = ModificationStateHelper.IsActive(itemKey.ModificationState, 2);
			if (flag)
			{
				itemDisplayData.RefiningEffects = DomainManager.Item.GetRefinedEffects(itemKey);
			}
			bool flag2 = ModificationStateHelper.IsActive(itemKey.ModificationState, 1);
			if (flag2)
			{
				DomainManager.Extra.TryGetPoisonEffect(itemKey.Id, out itemDisplayData.PoisonEffects);
			}
			EquipmentBase equipBase = item as EquipmentBase;
			bool flag3 = equipBase != null;
			if (flag3)
			{
				itemDisplayData.EquipmentEffectIds = new List<short>(from x in DomainManager.Item.GetEquipmentEffects(equipBase)
				select x.TemplateId);
				itemDisplayData.MaterialResources = equipBase.GetMaterialResources();
			}
			Weapon weapon = item as Weapon;
			bool flag4 = weapon != null;
			if (flag4)
			{
				itemDisplayData.EquipmentAttack = weapon.GetEquipmentAttack();
				itemDisplayData.EquipmentDefense = weapon.GetEquipmentDefense();
				itemDisplayData.HitAvoidFactor = ((charId >= 0) ? weapon.GetHitFactors(charId) : weapon.GetHitFactors());
				itemDisplayData.PenetrationInfo.Item1 = weapon.GetPenetrationFactor();
			}
			else
			{
				Armor armor = item as Armor;
				bool flag5 = armor != null;
				if (flag5)
				{
					itemDisplayData.EquipmentAttack = armor.GetEquipmentAttack();
					itemDisplayData.EquipmentDefense = armor.GetEquipmentDefense();
					itemDisplayData.HitAvoidFactor = ((charId >= 0) ? armor.GetAvoidFactors(charId) : armor.GetAvoidFactors());
					armor.GetPenetrationResistFactors().Deconstruct(out itemDisplayData.PenetrationInfo.Item1, out itemDisplayData.PenetrationInfo.Item2);
					itemDisplayData.InjuryFactors = armor.GetInjuryFactor();
				}
				else
				{
					bool flag6 = itemKey.ItemType == 3;
					if (flag6)
					{
						itemDisplayData.WeavedClothingTemplateId = DomainManager.Extra.GetModifiedClothingTemplateId(itemKey);
					}
				}
			}
			Cricket cricket = item as Cricket;
			bool flag7 = cricket != null;
			if (flag7)
			{
				itemDisplayData.SpecialArg = ((int)((ushort)cricket.GetColorId()) << 16 | (int)((ushort)cricket.GetPartId()));
			}
			else
			{
				bool flag8 = item is Weapon || item is Armor;
				if (flag8)
				{
					itemDisplayData.PowerInfo = DomainManager.Character.GetItemPowerInfo(charId, itemKey);
					itemDisplayData.Requirements = DomainManager.Character.GetItemRequirementsAndActualValues(charId, itemKey);
				}
				else
				{
					bool flag9 = item is Misc && GameData.Domains.Combat.SharedConstValue.SwordFragment2BossId.ContainsKey(itemKey.TemplateId);
					if (flag9)
					{
						itemDisplayData.SpecialArg = (int)DomainManager.Item.GetSwordFragmentCurrSkill(itemKey);
					}
				}
			}
			bool flag10 = itemDisplayData.ItemSourceTypeEnum == ItemSourceType.Inventory || itemDisplayData.ItemSourceTypeEnum == ItemSourceType.Equipment || itemDisplayData.ItemSourceTypeEnum == ItemSourceType.JiaoPool;
			if (flag10)
			{
				itemDisplayData.UsingType = DomainManager.Character.GetItemUsingType(itemKey, charId);
			}
			else
			{
				itemDisplayData.UsingType = ItemDisplayData.ItemUsingType.Invalid;
			}
			GameData.DLC.FiveLoong.Jiao jiao;
			bool flag11 = DomainManager.Extra.TryGetJiaoByItemKey(itemKey, out jiao) && jiao.GrowthStage == 0;
			if (flag11)
			{
				JiaoItem jiaoConfig = Config.Jiao.Instance[jiao.TemplateId];
				ItemDisplayData itemDisplayData2 = itemDisplayData;
				JiaoEggItemDisplayData jiaoEggItemDisplayData = default(JiaoEggItemDisplayData);
				jiaoEggItemDisplayData.TemplateId = jiao.TemplateId;
				List<string> colorList = jiaoConfig.ColorList;
				jiaoEggItemDisplayData.ColorCount = Convert.ToSByte((colorList != null) ? colorList.Count : 0);
				jiaoEggItemDisplayData.Behavior = jiao.Behavior;
				jiaoEggItemDisplayData.Gender = jiao.Gender;
				jiaoEggItemDisplayData.Generation = jiao.Generation;
				itemDisplayData2.JiaoEggItemDisplayData = jiaoEggItemDisplayData;
			}
			return itemDisplayData;
		}

		// Token: 0x06004FB8 RID: 20408 RVA: 0x002B7612 File Offset: 0x002B5812
		private void OnInitializedDomainData()
		{
		}

		// Token: 0x06004FB9 RID: 20409 RVA: 0x002B7615 File Offset: 0x002B5815
		private void InitializeOnInitializeGameDataModule()
		{
			ItemDomain.InitializeWugTemplateIds();
			this.InitializeCreationTemplateIds();
			this.InitializeCategorizedItemTemplates();
			this.InitializeCategorizedEquipmentEffects();
			this.InitializeSkillBreakPlateBonusEffects();
			MixedPoisonType.InitializeMaskDict();
		}

		// Token: 0x06004FBA RID: 20410 RVA: 0x002B7640 File Offset: 0x002B5840
		private void InitializeOnEnterNewWorld()
		{
			this._nextItemId = 0;
			this._emptyHandKey = ItemKey.Invalid;
			this._branchKey = ItemKey.Invalid;
			this._stoneKey = ItemKey.Invalid;
			this._trackedSpecialItems.Clear();
		}

		// Token: 0x06004FBB RID: 20411 RVA: 0x002B7677 File Offset: 0x002B5877
		private void OnLoadedArchiveData()
		{
			this.InitializeTrackedSpecialItems();
		}

		// Token: 0x06004FBC RID: 20412 RVA: 0x002B7684 File Offset: 0x002B5884
		public override void FixAbnormalDomainArchiveData(DataContext context)
		{
			bool flag = DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 71, 23);
			if (flag)
			{
				int count = 0;
				foreach (KeyValuePair<int, CraftTool> keyValuePair in this._craftTools)
				{
					int num;
					CraftTool craftTool;
					keyValuePair.Deconstruct(out num, out craftTool);
					CraftTool tool = craftTool;
					short prevMaxDurability = tool.GetMaxDurability();
					short prevCurrDurability = tool.GetCurrDurability();
					bool flag2 = prevMaxDurability <= 0 || prevCurrDurability <= 0;
					if (!flag2)
					{
						CraftToolItem template = CraftTool.Instance[tool.GetTemplateId()];
						short newMaxDurability = ItemBase.GenerateMaxDurability(context.Random, template.MaxDurability);
						bool flag3 = newMaxDurability <= prevMaxDurability;
						if (!flag3)
						{
							tool.SetMaxDurability(newMaxDurability, context);
							short newCurrDurability = prevCurrDurability + newMaxDurability - prevMaxDurability;
							tool.SetCurrDurability(newCurrDurability, context);
							count++;
						}
					}
				}
				Logger logger = ItemDomain.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Regenerated max durability for ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(count);
				defaultInterpolatedStringHandler.AppendLiteral(" craft tools.");
				logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			bool flag4 = DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 79, 4);
			if (flag4)
			{
				foreach (Carrier carrier in this._carriers.Values)
				{
					short oldMax = carrier.GetMaxDurability();
					short oldCurr = carrier.GetCurrDurability();
					CarrierItem template2 = Carrier.Instance[carrier.GetTemplateId()];
					short newMaxDurability2 = ItemBase.GenerateMaxDurability(context.Random, template2.MaxDurability);
					carrier.SetMaxDurability(newMaxDurability2, context);
					carrier.SetCurrDurability(newMaxDurability2, context);
					Logger logger2 = ItemDomain.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 6);
					defaultInterpolatedStringHandler.AppendLiteral("Regenerate ");
					defaultInterpolatedStringHandler.AppendFormatted<ItemOwnerKey>(carrier.Owner);
					defaultInterpolatedStringHandler.AppendLiteral("'s ");
					defaultInterpolatedStringHandler.AppendFormatted(template2.Name);
					defaultInterpolatedStringHandler.AppendLiteral(" Durability: (");
					defaultInterpolatedStringHandler.AppendFormatted<short>(oldCurr);
					defaultInterpolatedStringHandler.AppendLiteral("/");
					defaultInterpolatedStringHandler.AppendFormatted<short>(oldMax);
					defaultInterpolatedStringHandler.AppendLiteral(") -> (");
					defaultInterpolatedStringHandler.AppendFormatted<short>(newMaxDurability2);
					defaultInterpolatedStringHandler.AppendLiteral("/");
					defaultInterpolatedStringHandler.AppendFormatted<short>(newMaxDurability2);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			this.TransferOldPoisonData(context);
			DomainManager.Character.RemoveDlcDuplicateClothing(context, true);
		}

		// Token: 0x06004FBD RID: 20413 RVA: 0x002B7968 File Offset: 0x002B5B68
		[Obsolete("This method is obsolete, and will be removed in future.")]
		[return: TupleElementNames(new string[]
		{
			"outer",
			"inner",
			"mind",
			"fatalDamage"
		})]
		public ValueTuple<int, int, int, int> GetWeaponBlockDamageValue(DataContext context, int charId, ItemKey weaponKey)
		{
			return new ValueTuple<int, int, int, int>(0, 0, 0, 0);
		}

		// Token: 0x06004FBE RID: 20414 RVA: 0x002B7984 File Offset: 0x002B5B84
		private void InitializeCategorizedEquipmentEffects()
		{
			if (ItemDomain._categorizedEquipmentEffects == null)
			{
				ItemDomain._categorizedEquipmentEffects = new List<short>[3];
			}
			for (int i = 0; i < 3; i++)
			{
				List<short>[] categorizedEquipmentEffects = ItemDomain._categorizedEquipmentEffects;
				int num = i;
				if (categorizedEquipmentEffects[num] == null)
				{
					categorizedEquipmentEffects[num] = new List<short>();
				}
				ItemDomain._categorizedEquipmentEffects[i].Clear();
			}
			foreach (EquipmentEffectItem equipmentEffectCfg in ((IEnumerable<EquipmentEffectItem>)EquipmentEffect.Instance))
			{
				bool special = equipmentEffectCfg.Special;
				if (!special)
				{
					switch (equipmentEffectCfg.Type)
					{
					case 0:
						ItemDomain._categorizedEquipmentEffects[0].Add(equipmentEffectCfg.TemplateId);
						ItemDomain._categorizedEquipmentEffects[1].Add(equipmentEffectCfg.TemplateId);
						ItemDomain._categorizedEquipmentEffects[2].Add(equipmentEffectCfg.TemplateId);
						break;
					case 1:
						ItemDomain._categorizedEquipmentEffects[0].Add(equipmentEffectCfg.TemplateId);
						break;
					case 2:
						ItemDomain._categorizedEquipmentEffects[1].Add(equipmentEffectCfg.TemplateId);
						break;
					}
				}
			}
		}

		// Token: 0x06004FBF RID: 20415 RVA: 0x002B7AC0 File Offset: 0x002B5CC0
		private void InitializeSkillBreakPlateBonusEffects()
		{
			int effectCount = SkillBreakBonusEffect.Instance.Count;
			ItemDomain._skillBreakPlateBonusEffects = new List<TemplateKey>[effectCount];
			for (int i = 0; i < effectCount; i++)
			{
				ItemDomain._skillBreakPlateBonusEffects[i] = new List<TemplateKey>();
			}
			for (sbyte itemType = 0; itemType < 13; itemType += 1)
			{
				IList<int> keys = ItemTemplateHelper.GetTemplateDataAllKeys(itemType);
				foreach (int key in keys)
				{
					short templateId = (short)key;
					sbyte bonusEffectId = ItemTemplateHelper.GetBreakBonusEffect(itemType, templateId);
					bool flag = bonusEffectId < 0;
					if (!flag)
					{
						short groupId = ItemTemplateHelper.GetGroupId(itemType, templateId);
						bool flag2 = groupId < 0;
						if (flag2)
						{
							groupId = templateId;
						}
						else
						{
							bool flag3 = groupId != templateId;
							if (flag3)
							{
								continue;
							}
						}
						ItemDomain._skillBreakPlateBonusEffects[(int)bonusEffectId].Add(new TemplateKey(itemType, groupId));
					}
				}
			}
		}

		// Token: 0x06004FC0 RID: 20416 RVA: 0x002B7BC4 File Offset: 0x002B5DC4
		private void InitializeCategorizedItemTemplates()
		{
			if (ItemDomain._categorizedItemTemplates == null)
			{
				ItemDomain._categorizedItemTemplates = new Dictionary<short, List<short>>[9];
			}
			for (int i = 0; i < 9; i++)
			{
				Dictionary<short, List<short>>[] categorizedItemTemplates = ItemDomain._categorizedItemTemplates;
				int num = i;
				if (categorizedItemTemplates[num] == null)
				{
					categorizedItemTemplates[num] = new Dictionary<short, List<short>>();
				}
				Dictionary<short, List<short>> subType2Templates = ItemDomain._categorizedItemTemplates[i];
				for (int itemType = 0; itemType < 13; itemType++)
				{
					short[] subTypes = ItemSubType.Type2SubTypes[itemType];
					foreach (short subType in subTypes)
					{
						List<short> templates;
						bool flag = subType2Templates.TryGetValue(subType, out templates);
						if (flag)
						{
							templates.Clear();
						}
						else
						{
							ItemDomain._categorizedItemTemplates[i].Add(subType, new List<short>());
						}
					}
				}
			}
			for (sbyte j = 0; j < 9; j += 1)
			{
				Dictionary<short, List<short>> subType2Templates2 = ItemDomain._categorizedItemTemplates[(int)j];
				for (sbyte itemType2 = 0; itemType2 < 13; itemType2 += 1)
				{
					short[] array2 = ItemSubType.Type2SubTypes[(int)itemType2];
					for (int l = 0; l < array2.Length; l++)
					{
						ItemDomain.<>c__DisplayClass78_0 CS$<>8__locals1 = new ItemDomain.<>c__DisplayClass78_0();
						CS$<>8__locals1.itemSubType = array2[l];
						bool flag2 = !ItemSubType.IsHobbyType(CS$<>8__locals1.itemSubType);
						if (!flag2)
						{
							IEnumerable<short> list;
							switch (itemType2)
							{
							case 0:
							{
								sbyte grade = ItemDomain.GetClosestNeighboringGradeWithValidItem<WeaponItem>(j, Weapon.Instance.ToList<WeaponItem>(), (ValueTuple<WeaponItem, sbyte> pair) => pair.Item1.Grade == pair.Item2 && CS$<>8__locals1.itemSubType == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
								list = from item in Weapon.Instance
								where item.Grade == grade && CS$<>8__locals1.itemSubType == item.ItemSubType && item.DropRate > 0 && item.BaseValue > 0 && item.Transferable && item.AllowRandomCreate
								select item.TemplateId;
								break;
							}
							case 1:
							{
								sbyte grade = ItemDomain.GetClosestNeighboringGradeWithValidItem<ArmorItem>(j, Armor.Instance.ToList<ArmorItem>(), (ValueTuple<ArmorItem, sbyte> pair) => pair.Item1.Grade == pair.Item2 && CS$<>8__locals1.itemSubType == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
								list = from item in Armor.Instance
								where item.Grade == grade && CS$<>8__locals1.itemSubType == item.ItemSubType && item.DropRate > 0 && item.BaseValue > 0 && item.Transferable && item.AllowRandomCreate
								select item.TemplateId;
								break;
							}
							case 2:
							{
								sbyte grade = ItemDomain.GetClosestNeighboringGradeWithValidItem<AccessoryItem>(j, Accessory.Instance.ToList<AccessoryItem>(), (ValueTuple<AccessoryItem, sbyte> pair) => pair.Item1.Grade == pair.Item2 && CS$<>8__locals1.itemSubType == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
								list = from item in Accessory.Instance
								where item.Grade == grade && CS$<>8__locals1.itemSubType == item.ItemSubType && item.DropRate > 0 && item.BaseValue > 0 && item.Transferable && item.AllowRandomCreate
								select item.TemplateId;
								break;
							}
							case 3:
							{
								sbyte grade = ItemDomain.GetClosestNeighboringGradeWithValidItem<ClothingItem>(j, Clothing.Instance.ToList<ClothingItem>(), (ValueTuple<ClothingItem, sbyte> pair) => pair.Item1.Grade == pair.Item2 && CS$<>8__locals1.itemSubType == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
								list = from item in Clothing.Instance
								where item.Grade == grade && CS$<>8__locals1.itemSubType == item.ItemSubType && item.DropRate > 0 && item.BaseValue > 0 && item.Transferable && item.AllowRandomCreate
								select item.TemplateId;
								break;
							}
							case 4:
							{
								sbyte grade = ItemDomain.GetClosestNeighboringGradeWithValidItem<CarrierItem>(j, Carrier.Instance.ToList<CarrierItem>(), (ValueTuple<CarrierItem, sbyte> pair) => pair.Item1.Grade == pair.Item2 && CS$<>8__locals1.itemSubType == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
								list = from item in Carrier.Instance
								where item.Grade == grade && CS$<>8__locals1.itemSubType == item.ItemSubType && item.DropRate > 0 && item.BaseValue > 0 && item.Transferable && item.AllowRandomCreate
								select item.TemplateId;
								break;
							}
							case 5:
							{
								sbyte grade = ItemDomain.GetClosestNeighboringGradeWithValidItem<MaterialItem>(j, Material.Instance.ToList<MaterialItem>(), (ValueTuple<MaterialItem, sbyte> pair) => pair.Item1.Grade == pair.Item2 && CS$<>8__locals1.itemSubType == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
								list = from item in Material.Instance
								where item.Grade == grade && CS$<>8__locals1.itemSubType == item.ItemSubType && item.DropRate > 0 && item.BaseValue > 0 && item.Transferable && item.AllowRandomCreate
								select item.TemplateId;
								break;
							}
							case 6:
							{
								sbyte grade = ItemDomain.GetClosestNeighboringGradeWithValidItem<CraftToolItem>(j, CraftTool.Instance.ToList<CraftToolItem>(), (ValueTuple<CraftToolItem, sbyte> pair) => pair.Item1.Grade == pair.Item2 && CS$<>8__locals1.itemSubType == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
								list = from item in CraftTool.Instance
								where item.Grade == grade && CS$<>8__locals1.itemSubType == item.ItemSubType && item.DropRate > 0 && item.BaseValue > 0 && item.Transferable && item.AllowRandomCreate
								select item.TemplateId;
								break;
							}
							case 7:
							{
								sbyte grade = ItemDomain.GetClosestNeighboringGradeWithValidItem<FoodItem>(j, Food.Instance.ToList<FoodItem>(), (ValueTuple<FoodItem, sbyte> pair) => pair.Item1.Grade == pair.Item2 && CS$<>8__locals1.itemSubType == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
								list = from item in Food.Instance
								where item.Grade == grade && CS$<>8__locals1.itemSubType == item.ItemSubType && item.DropRate > 0 && item.BaseValue > 0 && item.Transferable && item.AllowRandomCreate
								select item.TemplateId;
								break;
							}
							case 8:
							{
								sbyte grade = ItemDomain.GetClosestNeighboringGradeWithValidItem<MedicineItem>(j, Medicine.Instance.ToList<MedicineItem>(), (ValueTuple<MedicineItem, sbyte> pair) => pair.Item1.Grade == pair.Item2 && CS$<>8__locals1.itemSubType == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
								list = from item in Medicine.Instance
								where item.Grade == grade && CS$<>8__locals1.itemSubType == item.ItemSubType && item.DropRate > 0 && item.BaseValue > 0 && item.Transferable && item.AllowRandomCreate
								select item.TemplateId;
								break;
							}
							case 9:
							{
								sbyte grade = ItemDomain.GetClosestNeighboringGradeWithValidItem<TeaWineItem>(j, TeaWine.Instance.ToList<TeaWineItem>(), (ValueTuple<TeaWineItem, sbyte> pair) => pair.Item1.Grade == pair.Item2 && CS$<>8__locals1.itemSubType == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
								list = from item in TeaWine.Instance
								where item.Grade == grade && CS$<>8__locals1.itemSubType == item.ItemSubType && item.DropRate > 0 && item.BaseValue > 0 && item.Transferable && item.AllowRandomCreate
								select item.TemplateId;
								break;
							}
							case 10:
							{
								sbyte grade = ItemDomain.GetClosestNeighboringGradeWithValidItem<SkillBookItem>(j, SkillBook.Instance.ToList<SkillBookItem>(), (ValueTuple<SkillBookItem, sbyte> pair) => pair.Item1.Grade == pair.Item2 && CS$<>8__locals1.itemSubType == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
								list = from item in SkillBook.Instance
								where item.Grade == grade && CS$<>8__locals1.itemSubType == item.ItemSubType && item.DropRate > 0 && item.BaseValue > 0 && item.Transferable && item.AllowRandomCreate
								select item.TemplateId;
								break;
							}
							case 11:
							{
								sbyte grade = ItemDomain.GetClosestNeighboringGradeWithValidItem<CricketItem>(j, Cricket.Instance.ToList<CricketItem>(), (ValueTuple<CricketItem, sbyte> pair) => pair.Item1.Grade == pair.Item2 && CS$<>8__locals1.itemSubType == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
								list = from item in Cricket.Instance
								where item.Grade == grade && CS$<>8__locals1.itemSubType == item.ItemSubType && item.DropRate > 0 && item.BaseValue > 0 && item.Transferable && item.AllowRandomCreate
								select item.TemplateId;
								break;
							}
							case 12:
							{
								sbyte grade = ItemDomain.GetClosestNeighboringGradeWithValidItem<MiscItem>(j, Misc.Instance.ToList<MiscItem>(), (ValueTuple<MiscItem, sbyte> pair) => pair.Item1.Grade == pair.Item2 && CS$<>8__locals1.itemSubType == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
								list = from item in Misc.Instance
								where item != null && item.Grade == grade && CS$<>8__locals1.itemSubType == item.ItemSubType && item.DropRate > 0 && item.BaseValue > 0 && item.Transferable && item.AllowRandomCreate
								select item.TemplateId;
								break;
							}
							default:
								throw ItemTemplateHelper.CreateItemTypeException(itemType2);
							}
							subType2Templates2[CS$<>8__locals1.itemSubType].AddRange(list);
						}
					}
				}
			}
		}

		// Token: 0x06004FC1 RID: 20417 RVA: 0x002B8330 File Offset: 0x002B6530
		private void InitializeTrackedSpecialItems()
		{
			foreach (Misc misc in this._misc.Values)
			{
				short templateId = misc.GetTemplateId();
				bool flag = templateId == 225;
				if (flag)
				{
					this._trackedSpecialItems.Add(misc.GetItemKey());
				}
				else
				{
					bool flag2 = Misc.Instance[templateId].ItemSubType == 1202;
					if (flag2)
					{
						ItemKey itemKey = misc.GetItemKey();
						this._trackedSpecialItems.Add(itemKey);
						DomainManager.LegendaryBook.RegisterLegendaryBookItem(itemKey);
					}
				}
			}
		}

		// Token: 0x06004FC2 RID: 20418 RVA: 0x002B83F0 File Offset: 0x002B65F0
		public void CheckAndTrackSpecialItem(ItemKey itemKey)
		{
			bool flag = itemKey.ItemType == 12;
			if (flag)
			{
				bool flag2 = itemKey.TemplateId == 225;
				if (flag2)
				{
					this._trackedSpecialItems.Add(itemKey);
				}
				else
				{
					bool flag3 = Misc.Instance[itemKey.TemplateId].ItemSubType == 1202;
					if (flag3)
					{
						this._trackedSpecialItems.Add(itemKey);
						DomainManager.LegendaryBook.RegisterLegendaryBookItem(itemKey);
					}
				}
			}
		}

		// Token: 0x06004FC3 RID: 20419 RVA: 0x002B846C File Offset: 0x002B666C
		public bool HasTrackedSpecialItems(sbyte itemType, short itemTemplateId)
		{
			foreach (ItemKey itemKey in this._trackedSpecialItems)
			{
				bool flag = itemKey.ItemType == itemType && itemKey.TemplateId == itemTemplateId;
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004FC4 RID: 20420 RVA: 0x002B84E0 File Offset: 0x002B66E0
		public ItemKey GetWuYingFromCharacterInventory(GameData.Domains.Character.Character character)
		{
			Inventory inventory = character.GetInventory();
			foreach (ItemKey itemKey in this._trackedSpecialItems)
			{
				bool flag = 12 != itemKey.ItemType || 225 != itemKey.TemplateId;
				if (!flag)
				{
					bool flag2 = inventory.Items.ContainsKey(itemKey);
					if (flag2)
					{
						return itemKey;
					}
				}
			}
			return ItemKey.Invalid;
		}

		// Token: 0x06004FC5 RID: 20421 RVA: 0x002B8580 File Offset: 0x002B6780
		public static short GetRandomEquipmentEffect(IRandomSource random, sbyte itemType)
		{
			bool flag = !ItemDomain._categorizedEquipmentEffects.CheckIndex((int)itemType) || ItemDomain._categorizedEquipmentEffects[(int)itemType].Count == 0;
			short result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				result = ItemDomain._categorizedEquipmentEffects[(int)itemType].GetRandom(random);
			}
			return result;
		}

		// Token: 0x06004FC6 RID: 20422 RVA: 0x002B85C8 File Offset: 0x002B67C8
		public static TemplateKey GetRandomItemGroupIdByEffect(IRandomSource random, sbyte effectId)
		{
			List<TemplateKey> list = ItemDomain._skillBreakPlateBonusEffects[(int)effectId];
			return (list.Count >= 0) ? list.GetRandom(random) : TemplateKey.Invalid;
		}

		// Token: 0x06004FC7 RID: 20423 RVA: 0x002B85FC File Offset: 0x002B67FC
		public static bool IsValidEquipmentEffectForItemType(sbyte itemType, short equipmentEffect)
		{
			return ItemDomain._categorizedEquipmentEffects[(int)itemType].Contains(equipmentEffect);
		}

		// Token: 0x06004FC8 RID: 20424 RVA: 0x002B861C File Offset: 0x002B681C
		public static bool CanItemBeLost(ItemKey itemKey)
		{
			bool flag = !ItemTemplateHelper.IsTransferable(itemKey.ItemType, itemKey.TemplateId);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = ModificationStateHelper.IsActive(itemKey.ModificationState, 4);
				result = !flag2;
			}
			return result;
		}

		// Token: 0x06004FC9 RID: 20425 RVA: 0x002B8660 File Offset: 0x002B6860
		public ItemKey GetDefaultWeaponItemKey(DataContext context, short templateId)
		{
			ItemKey result;
			switch (templateId)
			{
			case 0:
			{
				bool flag = !this._emptyHandKey.IsValid();
				if (flag)
				{
					this.SetEmptyHandKey(this.CreateWeapon(context, templateId, 0), context);
				}
				result = this._emptyHandKey;
				break;
			}
			case 1:
			{
				bool flag2 = !this._branchKey.IsValid();
				if (flag2)
				{
					this.SetBranchKey(this.CreateWeapon(context, templateId, 0), context);
				}
				result = this._branchKey;
				break;
			}
			case 2:
			{
				bool flag3 = !this._stoneKey.IsValid();
				if (flag3)
				{
					this.SetStoneKey(this.CreateWeapon(context, templateId, 0), context);
				}
				result = this._stoneKey;
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 1);
				defaultInterpolatedStringHandler.AppendFormatted<short>(templateId);
				defaultInterpolatedStringHandler.AppendLiteral(" is not a valid default weapon template id.");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06004FCA RID: 20426 RVA: 0x002B8744 File Offset: 0x002B6944
		public ItemBase GetBaseItem(ItemKey itemKey)
		{
			int itemId = itemKey.Id;
			sbyte itemType = itemKey.ItemType;
			if (!true)
			{
			}
			ItemBase result;
			switch (itemType)
			{
			case 0:
				result = this._weapons[itemId];
				break;
			case 1:
				result = this._armors[itemId];
				break;
			case 2:
				result = this._accessories[itemId];
				break;
			case 3:
				result = this._clothing[itemId];
				break;
			case 4:
				result = this._carriers[itemId];
				break;
			case 5:
				result = this._materials[itemId];
				break;
			case 6:
				result = this._craftTools[itemId];
				break;
			case 7:
				result = this._foods[itemId];
				break;
			case 8:
				result = this._medicines[itemId];
				break;
			case 9:
				result = this._teaWines[itemId];
				break;
			case 10:
				result = this._skillBooks[itemId];
				break;
			case 11:
				result = this._crickets[itemId];
				break;
			case 12:
				result = this._misc[itemId];
				break;
			default:
				throw ItemTemplateHelper.CreateItemTypeException(itemKey.ItemType);
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06004FCB RID: 20427 RVA: 0x002B888C File Offset: 0x002B6A8C
		public ItemBase TryGetBaseItem(ItemKey itemKey)
		{
			int itemId = itemKey.Id;
			sbyte itemType = itemKey.ItemType;
			if (!true)
			{
			}
			ItemBase result;
			switch (itemType)
			{
			case 0:
			{
				Weapon item;
				result = (this._weapons.TryGetValue(itemId, out item) ? item : null);
				break;
			}
			case 1:
			{
				Armor item2;
				result = (this._armors.TryGetValue(itemId, out item2) ? item2 : null);
				break;
			}
			case 2:
			{
				Accessory item3;
				result = (this._accessories.TryGetValue(itemId, out item3) ? item3 : null);
				break;
			}
			case 3:
			{
				Clothing item4;
				result = (this._clothing.TryGetValue(itemId, out item4) ? item4 : null);
				break;
			}
			case 4:
			{
				Carrier item5;
				result = (this._carriers.TryGetValue(itemId, out item5) ? item5 : null);
				break;
			}
			case 5:
			{
				Material item6;
				result = (this._materials.TryGetValue(itemId, out item6) ? item6 : null);
				break;
			}
			case 6:
			{
				CraftTool item7;
				result = (this._craftTools.TryGetValue(itemId, out item7) ? item7 : null);
				break;
			}
			case 7:
			{
				Food item8;
				result = (this._foods.TryGetValue(itemId, out item8) ? item8 : null);
				break;
			}
			case 8:
			{
				Medicine item9;
				result = (this._medicines.TryGetValue(itemId, out item9) ? item9 : null);
				break;
			}
			case 9:
			{
				TeaWine item10;
				result = (this._teaWines.TryGetValue(itemId, out item10) ? item10 : null);
				break;
			}
			case 10:
			{
				SkillBook item11;
				result = (this._skillBooks.TryGetValue(itemId, out item11) ? item11 : null);
				break;
			}
			case 11:
			{
				Cricket item12;
				result = (this._crickets.TryGetValue(itemId, out item12) ? item12 : null);
				break;
			}
			case 12:
			{
				Misc item13;
				result = (this._misc.TryGetValue(itemId, out item13) ? item13 : null);
				break;
			}
			default:
				result = null;
				break;
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06004FCC RID: 20428 RVA: 0x002B8A58 File Offset: 0x002B6C58
		public bool ItemExists(ItemKey itemKey)
		{
			int itemId = itemKey.Id;
			sbyte itemType = itemKey.ItemType;
			if (!true)
			{
			}
			bool result;
			switch (itemType)
			{
			case 0:
				result = this._weapons.ContainsKey(itemId);
				break;
			case 1:
				result = this._armors.ContainsKey(itemId);
				break;
			case 2:
				result = this._accessories.ContainsKey(itemId);
				break;
			case 3:
				result = this._clothing.ContainsKey(itemId);
				break;
			case 4:
				result = this._carriers.ContainsKey(itemId);
				break;
			case 5:
				result = this._materials.ContainsKey(itemId);
				break;
			case 6:
				result = this._craftTools.ContainsKey(itemId);
				break;
			case 7:
				result = this._foods.ContainsKey(itemId);
				break;
			case 8:
				result = this._medicines.ContainsKey(itemId);
				break;
			case 9:
				result = this._teaWines.ContainsKey(itemId);
				break;
			case 10:
				result = this._skillBooks.ContainsKey(itemId);
				break;
			case 11:
				result = this._crickets.ContainsKey(itemId);
				break;
			case 12:
				result = this._misc.ContainsKey(itemId);
				break;
			default:
				throw ItemTemplateHelper.CreateItemTypeException(itemKey.ItemType);
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06004FCD RID: 20429 RVA: 0x002B8BA0 File Offset: 0x002B6DA0
		public EquipmentBase GetBaseEquipment(ItemKey itemKey)
		{
			int itemId = itemKey.Id;
			sbyte itemType = itemKey.ItemType;
			if (!true)
			{
			}
			EquipmentBase result;
			switch (itemType)
			{
			case 0:
				result = this._weapons[itemId];
				break;
			case 1:
				result = this._armors[itemId];
				break;
			case 2:
				result = this._accessories[itemId];
				break;
			case 3:
				result = this._clothing[itemId];
				break;
			case 4:
				result = this._carriers[itemId];
				break;
			default:
				throw ItemTemplateHelper.CreateItemTypeException(itemKey.ItemType);
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06004FCE RID: 20430 RVA: 0x002B8C3C File Offset: 0x002B6E3C
		public EquipmentBase TryGetBaseEquipment(ItemKey itemKey)
		{
			int itemId = itemKey.Id;
			sbyte itemType = itemKey.ItemType;
			if (!true)
			{
			}
			EquipmentBase result;
			switch (itemType)
			{
			case 0:
			{
				Weapon weapon;
				result = (this._weapons.TryGetValue(itemId, out weapon) ? weapon : null);
				break;
			}
			case 1:
			{
				Armor armor;
				result = (this._armors.TryGetValue(itemId, out armor) ? armor : null);
				break;
			}
			case 2:
			{
				Accessory accessory;
				result = (this._accessories.TryGetValue(itemId, out accessory) ? accessory : null);
				break;
			}
			case 3:
			{
				Clothing clothing;
				result = (this._clothing.TryGetValue(itemId, out clothing) ? clothing : null);
				break;
			}
			case 4:
			{
				Carrier carrier;
				result = (this._carriers.TryGetValue(itemId, out carrier) ? carrier : null);
				break;
			}
			default:
				result = null;
				break;
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06004FCF RID: 20431 RVA: 0x002B8D08 File Offset: 0x002B6F08
		public static bool IsPureStackable(ItemBase item)
		{
			return item.GetStackable() && !ModificationStateHelper.IsAnyActive(item.GetModificationState());
		}

		// Token: 0x06004FD0 RID: 20432 RVA: 0x002B8D34 File Offset: 0x002B6F34
		public int GetCharacterPropertyBonus(ItemKey itemKey, ECharacterPropertyReferencedType propertyType)
		{
			ItemBase item = this.GetBaseItem(itemKey);
			return item.GetCharacterPropertyBonus(propertyType);
		}

		// Token: 0x06004FD1 RID: 20433 RVA: 0x002B8D58 File Offset: 0x002B6F58
		public static int GetDestroyedDate(ItemKey itemKey)
		{
			short preserveDuration = ItemTemplateHelper.GetPreservationDuration(itemKey.ItemType, itemKey.TemplateId);
			return (preserveDuration >= 0) ? (DomainManager.World.GetCurrDate() + (int)preserveDuration) : int.MaxValue;
		}

		// Token: 0x06004FD2 RID: 20434 RVA: 0x002B8D94 File Offset: 0x002B6F94
		public static int GetDestroyedDate(ItemKey itemKey, int date)
		{
			short preserveDuration = ItemTemplateHelper.GetPreservationDuration(itemKey.ItemType, itemKey.TemplateId);
			return (preserveDuration >= 0) ? (date + (int)preserveDuration) : int.MaxValue;
		}

		// Token: 0x06004FD3 RID: 20435 RVA: 0x002B8DC8 File Offset: 0x002B6FC8
		public int GetStackableItemIdByTemplateId(sbyte itemType, short templateId)
		{
			int value;
			return this._stackableItems.TryGetValue(new TemplateKey(itemType, templateId), out value) ? value : -1;
		}

		// Token: 0x06004FD4 RID: 20436 RVA: 0x002B8DF4 File Offset: 0x002B6FF4
		[DomainMethod]
		public int GetValue(ItemKey itemKey)
		{
			return this.GetBaseItem(itemKey).GetValue();
		}

		// Token: 0x06004FD5 RID: 20437 RVA: 0x002B8E14 File Offset: 0x002B7014
		[DomainMethod]
		[Obsolete("Use GetValue Instead.")]
		public int GetPrice(ItemKey itemKey)
		{
			return this.GetBaseItem(itemKey).GetValue();
		}

		// Token: 0x06004FD6 RID: 20438 RVA: 0x002B8E34 File Offset: 0x002B7034
		public static short GetRandomItemIdInSubType(IRandomSource random, short itemSubType, sbyte grade)
		{
			bool flag = !ItemSubType.IsHobbyType(itemSubType);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (flag)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported itemSubType ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(itemSubType);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			sbyte itemType = ItemSubType.GetType(itemSubType);
			Logger logger = ItemDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 4);
			defaultInterpolatedStringHandler.AppendLiteral("Getting item of type ");
			defaultInterpolatedStringHandler.AppendFormatted<sbyte>(itemType);
			defaultInterpolatedStringHandler.AppendLiteral("(");
			defaultInterpolatedStringHandler.AppendFormatted(ItemType.TypeId2TypeName[(int)itemType]);
			defaultInterpolatedStringHandler.AppendLiteral(") and sub type ");
			defaultInterpolatedStringHandler.AppendFormatted<short>(itemSubType);
			defaultInterpolatedStringHandler.AppendLiteral(" with grade ");
			defaultInterpolatedStringHandler.AppendFormatted<sbyte>(grade);
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			return ItemDomain._categorizedItemTemplates[(int)grade][itemSubType].GetRandom(random);
		}

		// Token: 0x06004FD7 RID: 20439 RVA: 0x002B8F14 File Offset: 0x002B7114
		[Obsolete("This method will remove in future, use GetRandomItemIdInSubType instead.")]
		public static short GetRandomItemTemplateId(IRandomSource random, short itemSubType, sbyte grade)
		{
			return ItemDomain.GetRandomItemIdInSubType(random, itemSubType, grade);
		}

		// Token: 0x06004FD8 RID: 20440 RVA: 0x002B8F30 File Offset: 0x002B7130
		public static sbyte GetClosestNeighboringGradeWithValidItem<T>(sbyte grade, List<T> collection, Predicate<ValueTuple<T, sbyte>> matchingFunc)
		{
			int delta = (grade < 4) ? 1 : -1;
			int i = (int)grade;
			while (i >= 0 && i < 9)
			{
				sbyte curGrade = (sbyte)i;
				foreach (T item in collection)
				{
					bool flag = matchingFunc(new ValueTuple<T, sbyte>(item, curGrade));
					if (flag)
					{
						return curGrade;
					}
				}
				i += delta;
			}
			int j = (int)grade - delta;
			while (j >= 0 && j < 9)
			{
				sbyte curGrade2 = (sbyte)j;
				foreach (T item2 in collection)
				{
					bool flag2 = matchingFunc(new ValueTuple<T, sbyte>(item2, curGrade2));
					if (flag2)
					{
						return curGrade2;
					}
				}
				j -= delta;
			}
			return -1;
		}

		// Token: 0x06004FD9 RID: 20441 RVA: 0x002B9044 File Offset: 0x002B7244
		public short GetSwordFragmentCurrSkill(ItemKey itemKey)
		{
			sbyte bossId;
			bool flag = itemKey.ItemType != 12 || !GameData.Domains.Combat.SharedConstValue.SwordFragment2BossId.TryGetValue(itemKey.TemplateId, out bossId);
			short result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				List<short> skillList = Boss.Instance[bossId].PlayerCastSkills;
				XiangshuAvatarTaskStatus status = DomainManager.World.GetElement_XiangshuAvatarTaskStatuses((int)bossId);
				bool favorEnough = FavorabilityType.GetFavorabilityType(DomainManager.World.GetXiangshuAvatarFavorability(bossId)) >= 4;
				bool flag2 = status.JuniorXiangshuTaskStatus == 6;
				if (flag2)
				{
					result = (favorEnough ? skillList[2] : skillList[0]);
				}
				else
				{
					bool flag3 = status.JuniorXiangshuTaskStatus == 5;
					if (flag3)
					{
						result = (favorEnough ? skillList[3] : skillList[1]);
					}
					else
					{
						result = -1;
					}
				}
			}
			return result;
		}

		// Token: 0x06004FDA RID: 20442 RVA: 0x002B910C File Offset: 0x002B730C
		public static long GetItemWorth(ItemKey itemKey, int amount)
		{
			long worth = 0L;
			short itemDurability = DomainManager.Item.GetBaseItem(itemKey).GetCurrDurability();
			bool flag = itemDurability > 0;
			if (flag)
			{
				int value = DomainManager.Item.GetValue(itemKey);
				worth = (long)(value * amount);
			}
			return worth;
		}

		// Token: 0x06004FDB RID: 20443 RVA: 0x002B9150 File Offset: 0x002B7350
		public ClothingItem GetClothingItemByDisplayId(short displayId)
		{
			foreach (ClothingItem clothingItem in ((IEnumerable<ClothingItem>)Clothing.Instance))
			{
				bool flag = clothingItem.DisplayId == displayId;
				if (flag)
				{
					return clothingItem;
				}
			}
			return null;
		}

		// Token: 0x06004FDC RID: 20444 RVA: 0x002B91B0 File Offset: 0x002B73B0
		public List<int> GetAllCarrierIdList()
		{
			return new List<int>(this._carriers.Keys);
		}

		// Token: 0x06004FDD RID: 20445 RVA: 0x002B91D2 File Offset: 0x002B73D2
		[DomainMethod]
		public ItemKey GetEmptyToolKey(DataContext context)
		{
			return DomainManager.Extra.GetEmptyToolKey(context);
		}

		// Token: 0x06004FDE RID: 20446 RVA: 0x002B91E0 File Offset: 0x002B73E0
		[DomainMethod]
		public int GetRepairItemNeedResourceCount(ItemKey itemKey, short targetDurability = -1)
		{
			ItemBase item = DomainManager.Item.GetBaseItem(itemKey);
			EquipmentBase equip = DomainManager.Item.TryGetBaseEquipment(itemKey);
			bool flag = equip == null;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int requiredResourceAmount = ItemTemplateHelper.GetRepairNeedResourceCount(equip.GetMaterialResources(), itemKey, item.GetCurrDurability());
				result = requiredResourceAmount;
			}
			return result;
		}

		// Token: 0x06004FDF RID: 20447 RVA: 0x002B9230 File Offset: 0x002B7430
		[DomainMethod]
		[Obsolete("可以用DisassembleItemOptional替代")]
		public List<ItemDisplayData> DisassembleItem(DataContext context, int charId, ItemKey itemKey, ItemKey toolKey)
		{
			return this.DisassembleItemOptional(context, charId, itemKey, toolKey, 1, 1);
		}

		// Token: 0x06004FE0 RID: 20448 RVA: 0x002B9250 File Offset: 0x002B7450
		[DomainMethod]
		public List<ItemDisplayData> DisassembleItemOptional(DataContext context, int charId, ItemKey itemKey, ItemKey toolKey, sbyte itemSourceType, sbyte toolSourceType)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			ItemBase item = this.GetBaseItem(itemKey);
			sbyte resourceType = ItemTemplateHelper.GetResourceType(itemKey.ItemType, itemKey.TemplateId);
			bool flag = resourceType == -1;
			List<ItemDisplayData> result2;
			if (flag)
			{
				result2 = null;
			}
			else
			{
				List<ItemKey> keyList = new List<ItemKey>();
				sbyte grade = ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId);
				int sameGradeRate = ItemTemplateHelper.GetDisassembleSameGradeRate(grade);
				short disassemblyMaterialId = ItemTemplateHelper.GetDisassemblyMaterial(itemKey.ItemType, itemKey.TemplateId, context.Random, sameGradeRate);
				bool flag2 = disassemblyMaterialId > -1;
				if (flag2)
				{
					ItemKey disassemblyMaterialKey = this.CreateMaterial(context, disassemblyMaterialId);
					DomainManager.Taiwu.AddItem(context, disassemblyMaterialKey, 1, itemSourceType, false);
					keyList.Add(disassemblyMaterialKey);
				}
				bool flag3 = ModificationStateHelper.IsActive(item.GetModificationState(), 2);
				if (flag3)
				{
					short[] allMaterialTemplateIds = DomainManager.Item.GetRefinedEffects(itemKey).GetAllMaterialTemplateIds();
					if (allMaterialTemplateIds != null)
					{
						allMaterialTemplateIds.ForEach(delegate(int i, short materialId)
						{
							bool flag15 = materialId <= -1;
							bool result3;
							if (flag15)
							{
								result3 = false;
							}
							else
							{
								ItemKey materialKey = this.CreateMaterial(context, materialId);
								DomainManager.Taiwu.AddItem(context, materialKey, 1, itemSourceType, false);
								keyList.Add(materialKey);
								result3 = false;
							}
							return result3;
						});
					}
				}
				bool flag4 = ItemType.IsEquipmentItemType(itemKey.ItemType);
				if (flag4)
				{
					EquipmentBase equipItem = DomainManager.Item.GetBaseEquipment(itemKey);
					ResourceInts resourceInts = ItemTemplateHelper.GetDisassembleResources(equipItem.GetMaterialResources(), itemKey.ItemType, itemKey.TemplateId, 1);
					character.ChangeResources(context, ref resourceInts);
				}
				else
				{
					bool flag5 = itemKey.ItemType == 12;
					if (flag5)
					{
						MiscItem miscConfig = Misc.Instance[itemKey.TemplateId];
						MakeItemSubTypeItem makeConfig = MakeItemSubType.Instance[miscConfig.MakeItemSubType];
						bool flag6 = makeConfig == null;
						MaterialResources presetResources;
						if (flag6)
						{
							presetResources = default(MaterialResources);
						}
						else
						{
							presetResources = makeConfig.MaxMaterialResources;
						}
						ResourceInts resourceInts2 = ItemTemplateHelper.GetDisassembleResources(presetResources, itemKey.ItemType, itemKey.TemplateId, 1);
						character.ChangeResources(context, ref resourceInts2);
					}
					else
					{
						ResourceInts resource = ItemTemplateHelper.GetDisassembleResources(default(MaterialResources), itemKey.ItemType, itemKey.TemplateId, 1);
						character.ChangeResources(context, ref resource);
						bool flag7 = itemKey.ItemType == 5;
						if (flag7)
						{
							MaterialItem config = Material.Instance[itemKey.TemplateId];
							List<PresetInventoryItem> disassembleResultItemList = config.DisassembleResultItemList;
							bool flag8 = disassembleResultItemList != null && disassembleResultItemList.Count > 0 && config.DisassembleResultCount > 0;
							if (flag8)
							{
								int rate = 0;
								List<PresetInventoryItem> rateList = new List<PresetInventoryItem>();
								rateList.AddRange(config.DisassembleResultItemList);
								for (int k = 0; k < config.DisassembleResultItemList.Count; k++)
								{
									PresetInventoryItem resultItem = config.DisassembleResultItemList[k];
									int curRate = resultItem.Amount;
									rate += curRate;
									rateList[k] = new PresetInventoryItem(resultItem.Type, resultItem.TemplateId, rate, 100);
								}
								for (int j = 0; j < (int)config.DisassembleResultCount; j++)
								{
									int random = context.Random.Next(rateList.Last<PresetInventoryItem>().Amount) + 1;
									int index = rateList.FindIndex((PresetInventoryItem r) => random <= r.Amount);
									PresetInventoryItem result = rateList[index];
									ItemKey resultItemKey = DomainManager.Item.CreateItem(context, result.Type, result.TemplateId);
									keyList.Add(resultItemKey);
									DomainManager.Taiwu.AddItem(context, resultItemKey, 1, itemSourceType, false);
								}
							}
							int value = DomainManager.Item.GetValue(itemKey);
							int seniority = ProfessionFormulaImpl.Calculate(4, value);
							DomainManager.Extra.ChangeProfessionSeniority(context, 0, seniority, true, false);
						}
					}
				}
				bool flag9 = itemKey.ItemType != 5;
				if (flag9)
				{
					sbyte skillType = ItemTemplateHelper.GetCraftRequiredLifeSkillType(itemKey.ItemType, itemKey.TemplateId);
					bool flag10 = skillType - 6 <= 1 || skillType - 10 <= 1;
					bool flag11 = flag10;
					if (flag11)
					{
						int seniority2 = ProfessionFormulaImpl.Calculate(17, (int)grade);
						DomainManager.Extra.ChangeProfessionSeniority(context, 2, seniority2, true, false);
					}
				}
				DomainManager.Taiwu.RemoveItem(context, itemKey, 1, itemSourceType, true, false);
				bool flag12 = toolKey.IsValid();
				if (flag12)
				{
					CraftToolItem toolConfig = CraftTool.Instance[toolKey.TemplateId];
					short cost = toolConfig.DurabilityCost[(int)grade];
					bool flag13 = cost > 0;
					if (flag13)
					{
						this.ReduceToolDurability(context, charId, toolKey, (int)cost, toolSourceType);
					}
				}
				bool flag14 = keyList.Count > 0;
				if (flag14)
				{
					result2 = this.GetItemDisplayDataListOptional(keyList, charId, itemSourceType, true);
				}
				else
				{
					result2 = null;
				}
			}
			return result2;
		}

		// Token: 0x06004FE1 RID: 20449 RVA: 0x002B9744 File Offset: 0x002B7944
		[DomainMethod]
		public List<ItemDisplayData> DisassembleItemList(DataContext context, int charId, List<MultiplyOperation> operationList)
		{
			List<ItemDisplayData> dataList = new List<ItemDisplayData>();
			foreach (MultiplyOperation operation in operationList)
			{
				for (int j = 0; j < operation.Count; j++)
				{
					List<ItemDisplayData> resultList = this.DisassembleItemOptional(context, charId, operation.Target, operation.Tool, operation.TargetItemSourceType, operation.ToolItemSourceType);
					bool flag = resultList != null && resultList.Count > 0;
					if (flag)
					{
						using (List<ItemDisplayData>.Enumerator enumerator2 = resultList.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								ItemDisplayData newItem = enumerator2.Current;
								ItemDisplayData oldItem = dataList.Find((ItemDisplayData i) => i.Key.TemplateEquals(newItem.Key));
								bool flag2 = oldItem == null;
								if (flag2)
								{
									dataList.Add(newItem);
								}
								else
								{
									oldItem.Amount += newItem.Amount;
								}
							}
						}
					}
				}
			}
			return dataList;
		}

		// Token: 0x06004FE2 RID: 20450 RVA: 0x002B98AC File Offset: 0x002B7AAC
		[DomainMethod]
		[Obsolete("可以使用DiscardItemOptional替代")]
		public void DiscardItem(DataContext context, int charId, ItemKey itemKey, int count = 1)
		{
			this.DiscardItemOptional(context, charId, itemKey, 1, count);
		}

		// Token: 0x06004FE3 RID: 20451 RVA: 0x002B98BC File Offset: 0x002B7ABC
		[DomainMethod]
		public void DiscardItemOptional(DataContext context, int charId, ItemKey itemKey, sbyte itemSourceType, int count = 1)
		{
			bool isMiscResource = ItemTemplateHelper.IsMiscResource(itemKey.ItemType, itemKey.TemplateId);
			bool flag = isMiscResource;
			if (flag)
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
				sbyte resourceType = ItemTemplateHelper.GetMiscResourceType(itemKey.ItemType, itemKey.TemplateId);
				character.ChangeResourceWithoutChecking(context, resourceType, -count);
			}
			else
			{
				DomainManager.Taiwu.RemoveItem(context, itemKey, count, itemSourceType, true, false);
			}
		}

		// Token: 0x06004FE4 RID: 20452 RVA: 0x002B9921 File Offset: 0x002B7B21
		[DomainMethod]
		[Obsolete("可以使用DiscardItemListOptional替代")]
		public void DiscardItemList(DataContext context, int charId, List<ItemKey> keyList)
		{
			this.DiscardItemListOptional(context, charId, keyList, 1);
		}

		// Token: 0x06004FE5 RID: 20453 RVA: 0x002B9930 File Offset: 0x002B7B30
		[DomainMethod]
		public void DiscardItemListOptional(DataContext context, int charId, List<ItemKey> keyList, sbyte itemSourceType)
		{
			Tester.Assert(keyList != null, "");
			Tester.Assert(keyList.Count > 0, "");
			foreach (ItemKey key in keyList)
			{
				this.DiscardItemOptional(context, charId, key, itemSourceType, 1);
			}
		}

		// Token: 0x06004FE6 RID: 20454 RVA: 0x002B99AC File Offset: 0x002B7BAC
		[DomainMethod]
		public List<ItemKey> GetRepairableItems(DataContext context, int charId, ItemKey toolKey)
		{
			ItemDomain.<>c__DisplayClass116_0 CS$<>8__locals1;
			CS$<>8__locals1.ret = new List<ItemKey>();
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			Dictionary<ItemKey, int> inventoryItems = character.GetInventory().Items;
			bool flag = !toolKey.TemplateEquals(DomainManager.Item.GetEmptyToolKey(context));
			if (flag)
			{
				bool flag2 = !inventoryItems.ContainsKey(toolKey);
				if (flag2)
				{
					return CS$<>8__locals1.ret;
				}
				CraftTool tool = this.GetElement_CraftTools(toolKey.Id);
				bool flag3 = tool.GetCurrDurability() <= 0;
				if (flag3)
				{
					return CS$<>8__locals1.ret;
				}
			}
			CS$<>8__locals1.toolRequiredLifeSkillTypes = CraftTool.Instance[toolKey.TemplateId].RequiredLifeSkillTypes;
			foreach (KeyValuePair<ItemKey, int> keyValuePair in inventoryItems)
			{
				ItemKey itemKey3;
				int num;
				keyValuePair.Deconstruct(out itemKey3, out num);
				ItemKey itemKey = itemKey3;
				ItemDomain.<GetRepairableItems>g__Do|116_0(itemKey, ref CS$<>8__locals1);
			}
			foreach (ItemKey itemKey2 in character.GetEquipment())
			{
				ItemDomain.<GetRepairableItems>g__Do|116_0(itemKey2, ref CS$<>8__locals1);
			}
			return CS$<>8__locals1.ret;
		}

		// Token: 0x06004FE7 RID: 20455 RVA: 0x002B9AF8 File Offset: 0x002B7CF8
		[DomainMethod]
		public List<ItemKey> GetDisassemblableItems(DataContext context, int charId, ItemKey toolKey)
		{
			ItemDomain.<>c__DisplayClass117_0 CS$<>8__locals1;
			CS$<>8__locals1.ret = new List<ItemKey>();
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			Dictionary<ItemKey, int> inventoryItems = character.GetInventory().Items;
			bool flag = !toolKey.TemplateEquals(DomainManager.Item.GetEmptyToolKey(context));
			if (flag)
			{
				bool flag2 = !inventoryItems.ContainsKey(toolKey);
				if (flag2)
				{
					return CS$<>8__locals1.ret;
				}
				CraftTool tool = this.GetElement_CraftTools(toolKey.Id);
				bool flag3 = tool.GetCurrDurability() <= 0;
				if (flag3)
				{
					return CS$<>8__locals1.ret;
				}
			}
			CS$<>8__locals1.toolRequiredLifeSkillTypes = CraftTool.Instance[toolKey.TemplateId].RequiredLifeSkillTypes;
			foreach (KeyValuePair<ItemKey, int> keyValuePair in inventoryItems)
			{
				ItemKey itemKey3;
				int num;
				keyValuePair.Deconstruct(out itemKey3, out num);
				ItemKey itemKey = itemKey3;
				ItemDomain.<GetDisassemblableItems>g__Do|117_0(itemKey, ref CS$<>8__locals1);
			}
			foreach (ItemKey itemKey2 in character.GetEquipment())
			{
				ItemDomain.<GetDisassemblableItems>g__Do|117_0(itemKey2, ref CS$<>8__locals1);
			}
			return CS$<>8__locals1.ret;
		}

		// Token: 0x06004FE8 RID: 20456 RVA: 0x002B9C44 File Offset: 0x002B7E44
		public bool CheckItemNeedRepair(ItemKey itemKey)
		{
			bool flag = !itemKey.IsValid();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
				result = (baseItem.GetRepairable() && baseItem.GetCurrDurability() < baseItem.GetMaxDurability());
			}
			return result;
		}

		// Token: 0x06004FE9 RID: 20457 RVA: 0x002B9C90 File Offset: 0x002B7E90
		public void ReduceToolDurability(DataContext context, int charId, ItemKey toolKey, int reduceValue, sbyte itemSourceType)
		{
			bool flag = ItemTemplateHelper.IsEmptyTool(toolKey.ItemType, toolKey.TemplateId);
			if (!flag)
			{
				CraftTool tool = DomainManager.Item.GetElement_CraftTools(toolKey.Id);
				int curDurability = Math.Max(0, (int)tool.GetCurrDurability() - reduceValue);
				tool.SetCurrDurability((short)curDurability, context);
				bool flag2 = tool.GetCurrDurability() <= 0;
				if (flag2)
				{
					bool flag3 = charId == DomainManager.Taiwu.GetTaiwuCharId();
					if (flag3)
					{
						DomainManager.Taiwu.RemoveItem(context, toolKey, 1, itemSourceType, true, false);
					}
					else
					{
						GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
						character.RemoveInventoryItem(context, toolKey, 1, true, false);
					}
				}
			}
		}

		// Token: 0x06004FEA RID: 20458 RVA: 0x002B9D3A File Offset: 0x002B7F3A
		private void InitializeCreationTemplateIds()
		{
			Cricket.InitializeCricketWeights();
		}

		// Token: 0x06004FEB RID: 20459 RVA: 0x002B9D44 File Offset: 0x002B7F44
		public ItemKey CreateItem(DataContext context, sbyte itemType, short templateId)
		{
			bool flag = ItemTemplateHelper.IsStackable(itemType, templateId);
			ItemKey result;
			if (flag)
			{
				int itemId = this.GetStackableItem(context, itemType, templateId);
				result = new ItemKey(itemType, 0, templateId, itemId);
			}
			else
			{
				ItemBase item = this.CreateItemInternal(context, itemType, templateId);
				result = item.GetItemKey();
			}
			return result;
		}

		// Token: 0x06004FEC RID: 20460 RVA: 0x002B9D8C File Offset: 0x002B7F8C
		public ItemKey CreateCopyOfItem(DataContext context, ItemKey srcItemKey)
		{
			bool flag = ItemTemplateHelper.IsPureStackable(srcItemKey);
			ItemKey result;
			if (flag)
			{
				result = srcItemKey;
			}
			else
			{
				IRandomSource random = context.Random;
				int itemId = this.GenerateNextItemId(context);
				this.CopyModificationState(context, srcItemKey, itemId);
				switch (srcItemKey.ItemType)
				{
				case 0:
				{
					Weapon srcItem = this.GetElement_Weapons(srcItemKey.Id);
					Weapon item = Serializer.CreateCopy<Weapon>(srcItem);
					item.OfflineSetId(itemId);
					item.OfflineSetEquippedCharId(-1);
					this.AddElement_Weapons(itemId, item);
					ItemKey itemKey = item.GetItemKey();
					result = itemKey;
					break;
				}
				case 1:
				{
					Armor srcItem2 = this.GetElement_Armors(srcItemKey.Id);
					Armor item2 = Serializer.CreateCopy<Armor>(srcItem2);
					item2.OfflineSetId(itemId);
					item2.OfflineSetEquippedCharId(-1);
					this.AddElement_Armors(itemId, item2);
					result = item2.GetItemKey();
					break;
				}
				case 2:
				{
					Accessory srcItem3 = this.GetElement_Accessories(srcItemKey.Id);
					Accessory item3 = Serializer.CreateCopy<Accessory>(srcItem3);
					item3.OfflineSetId(itemId);
					item3.OfflineSetEquippedCharId(-1);
					this.AddElement_Accessories(itemId, item3);
					result = item3.GetItemKey();
					break;
				}
				case 3:
				{
					Clothing srcItem4 = this.GetElement_Clothing(srcItemKey.Id);
					Clothing item4 = Serializer.CreateCopy<Clothing>(srcItem4);
					item4.OfflineSetId(itemId);
					item4.OfflineSetEquippedCharId(-1);
					this.AddElement_Clothing(itemId, item4);
					result = item4.GetItemKey();
					break;
				}
				case 4:
				{
					Carrier srcItem5 = this.GetElement_Carriers(srcItemKey.Id);
					Carrier item5 = Serializer.CreateCopy<Carrier>(srcItem5);
					item5.OfflineSetId(itemId);
					item5.OfflineSetEquippedCharId(-1);
					this.AddElement_Carriers(itemId, item5);
					ItemKey itemKey2 = item5.GetItemKey();
					int srcJiaoId;
					GameData.DLC.FiveLoong.Jiao srcJiao;
					bool flag2 = DomainManager.Extra.TryGetJiaoIdByItemKey(srcItemKey, out srcJiaoId) && DomainManager.Extra.TryGetJiao(srcJiaoId, out srcJiao);
					if (flag2)
					{
						DomainManager.Extra.AddCopyOfJiao(context, srcJiao, itemKey2);
					}
					else
					{
						int srcLoongId;
						ChildrenOfLoong srcLoong;
						bool flag3 = DomainManager.Extra.TryGetChildrenOfLoongIdByItemKey(srcItemKey, out srcLoongId) && DomainManager.Extra.TryGetLoong(srcLoongId, out srcLoong);
						if (flag3)
						{
							DomainManager.Extra.AddCopyOfChildOfLoong(context, srcLoong, itemKey2);
						}
					}
					result = itemKey2;
					break;
				}
				case 5:
				{
					Material srcItem6 = this.GetElement_Materials(srcItemKey.Id);
					Material item6 = Serializer.CreateCopy<Material>(srcItem6);
					item6.OfflineSetId(itemId);
					this.AddElement_Materials(itemId, item6);
					result = item6.GetItemKey();
					break;
				}
				case 6:
				{
					CraftTool srcItem7 = this.GetElement_CraftTools(srcItemKey.Id);
					CraftTool item7 = Serializer.CreateCopy<CraftTool>(srcItem7);
					item7.OfflineSetId(itemId);
					this.AddElement_CraftTools(itemId, item7);
					result = item7.GetItemKey();
					break;
				}
				case 7:
				{
					Food srcItem8 = this.GetElement_Foods(srcItemKey.Id);
					Food item8 = Serializer.CreateCopy<Food>(srcItem8);
					item8.OfflineSetId(itemId);
					this.AddElement_Foods(itemId, item8);
					result = item8.GetItemKey();
					break;
				}
				case 8:
				{
					Medicine srcItem9 = this.GetElement_Medicines(srcItemKey.Id);
					Medicine item9 = Serializer.CreateCopy<Medicine>(srcItem9);
					item9.OfflineSetId(itemId);
					this.AddElement_Medicines(itemId, item9);
					result = item9.GetItemKey();
					break;
				}
				case 9:
				{
					TeaWine srcItem10 = this.GetElement_TeaWines(srcItemKey.Id);
					TeaWine item10 = Serializer.CreateCopy<TeaWine>(srcItem10);
					item10.OfflineSetId(itemId);
					this.AddElement_TeaWines(itemId, item10);
					result = item10.GetItemKey();
					break;
				}
				case 10:
				{
					SkillBook srcItem11 = this.GetElement_SkillBooks(srcItemKey.Id);
					SkillBook item11 = Serializer.CreateCopy<SkillBook>(srcItem11);
					item11.OfflineSetId(itemId);
					this.AddElement_SkillBooks(itemId, item11);
					result = item11.GetItemKey();
					break;
				}
				case 11:
				{
					Cricket srcItem12 = this.GetElement_Crickets(srcItemKey.Id);
					Cricket item12 = Serializer.CreateCopy<Cricket>(srcItem12);
					item12.OfflineSetId(itemId);
					this.AddElement_Crickets(itemId, item12);
					Events.RaiseCricketCreated(context, item12.GetItemKey());
					result = item12.GetItemKey();
					break;
				}
				case 12:
				{
					Misc srcItem13 = this.GetElement_Misc(srcItemKey.Id);
					Misc item13 = Serializer.CreateCopy<Misc>(srcItem13);
					item13.OfflineSetId(itemId);
					this.AddElement_Misc(itemId, item13);
					ItemKey itemKey3 = item13.GetItemKey();
					int srcJiaoId2;
					GameData.DLC.FiveLoong.Jiao srcJiao2;
					bool flag4 = DomainManager.Extra.TryGetJiaoIdByItemKey(srcItemKey, out srcJiaoId2) && DomainManager.Extra.TryGetJiao(srcJiaoId2, out srcJiao2);
					if (flag4)
					{
						DomainManager.Extra.AddCopyOfJiao(context, srcJiao2, itemKey3);
					}
					result = itemKey3;
					break;
				}
				default:
					throw ItemTemplateHelper.CreateItemTypeException(srcItemKey.ItemType);
				}
			}
			return result;
		}

		// Token: 0x06004FED RID: 20461 RVA: 0x002BA1E0 File Offset: 0x002B83E0
		private void CopyModificationState(DataContext context, ItemKey srcItemKey, int destItemId)
		{
			bool flag = ModificationStateHelper.IsActive(srcItemKey.ModificationState, 2);
			if (flag)
			{
				RefiningEffects refiningEffect = this.GetRefinedEffects(srcItemKey);
				this.AddElement_RefinedItems(destItemId, refiningEffect, context);
			}
			bool flag2 = ModificationStateHelper.IsActive(srcItemKey.ModificationState, 1);
			if (flag2)
			{
				FullPoisonEffects poisoned = this.GetPoisonEffects(srcItemKey);
				DomainManager.Extra.SetPoisonEffect(context, destItemId, new FullPoisonEffects(poisoned));
			}
		}

		// Token: 0x06004FEE RID: 20462 RVA: 0x002BA240 File Offset: 0x002B8440
		public ItemKey CreateEquipment(DataContext context, sbyte itemType, short templateId, int spawnSpecialEffectChance)
		{
			if (!true)
			{
			}
			ItemKey itemKey;
			switch (itemType)
			{
			case 0:
				itemKey = this.CreateWeapon(context, templateId, 0);
				break;
			case 1:
				itemKey = this.CreateArmor(context, templateId, 0);
				break;
			case 2:
				itemKey = this.CreateAccessory(context, templateId, 0);
				break;
			case 3:
				itemKey = this.CreateClothing(context, templateId, 0);
				break;
			case 4:
				itemKey = this.CreateCarrier(context, templateId);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 1);
				defaultInterpolatedStringHandler.AppendLiteral("ItemType ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(itemType);
				defaultInterpolatedStringHandler.AppendLiteral(" is not an equipment");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			if (!true)
			{
			}
			ItemKey result = itemKey;
			EquipmentBase equipment = this.GetBaseEquipment(result);
			bool flag = context.Random.CheckPercentProb(spawnSpecialEffectChance);
			if (flag)
			{
				equipment.OfflineGenerateEquipmentEffect(context.Random);
				equipment.SetEquipmentEffectId(equipment.GetEquipmentEffectId(), context);
				equipment.SetCurrDurability(equipment.GetCurrDurability(), context);
				equipment.SetMaxDurability(equipment.GetMaxDurability(), context);
			}
			return result;
		}

		// Token: 0x06004FEF RID: 20463 RVA: 0x002BA344 File Offset: 0x002B8544
		public ItemKey CreateWeapon(DataContext context, short templateId, sbyte spawnSpecialEffectMultiplier = 1)
		{
			WeaponItem itemCfg = Weapon.Instance[templateId];
			Tester.Assert(!itemCfg.Stackable, "");
			int itemId = this.GenerateNextItemId(context);
			Weapon item = new Weapon(context.Random, templateId, itemId);
			bool flag = context.Random.CheckPercentProb((int)(GlobalConfig.Instance.EquipmentWithEffectRate * spawnSpecialEffectMultiplier));
			if (flag)
			{
				item.OfflineGenerateEquipmentEffect(context.Random);
			}
			item.OfflineGenerateMaterialResources(context.Random);
			this.AddElement_Weapons(itemId, item);
			return item.GetItemKey();
		}

		// Token: 0x06004FF0 RID: 20464 RVA: 0x002BA3D4 File Offset: 0x002B85D4
		public ItemKey CreateArmor(DataContext context, short templateId, sbyte spawnSpecialEffectMultiplier = 1)
		{
			Tester.Assert(!Armor.Instance[templateId].Stackable, "");
			int itemId = this.GenerateNextItemId(context);
			Armor item = new Armor(context.Random, templateId, itemId);
			bool flag = context.Random.CheckPercentProb((int)(GlobalConfig.Instance.EquipmentWithEffectRate * spawnSpecialEffectMultiplier));
			if (flag)
			{
				item.OfflineGenerateEquipmentEffect(context.Random);
			}
			item.OfflineGenerateMaterialResources(context.Random);
			this.AddElement_Armors(itemId, item);
			return item.GetItemKey();
		}

		// Token: 0x06004FF1 RID: 20465 RVA: 0x002BA460 File Offset: 0x002B8660
		public ItemKey CreateAccessory(DataContext context, short templateId, sbyte spawnSpecialEffectMultiplier = 1)
		{
			Tester.Assert(!Accessory.Instance[templateId].Stackable, "");
			int itemId = this.GenerateNextItemId(context);
			Accessory item = new Accessory(context.Random, templateId, itemId);
			bool flag = context.Random.CheckPercentProb((int)(GlobalConfig.Instance.EquipmentWithEffectRate * spawnSpecialEffectMultiplier));
			if (flag)
			{
				item.OfflineGenerateEquipmentEffect(context.Random);
			}
			item.OfflineGenerateMaterialResources(context.Random);
			this.AddElement_Accessories(itemId, item);
			return item.GetItemKey();
		}

		// Token: 0x06004FF2 RID: 20466 RVA: 0x002BA4EC File Offset: 0x002B86EC
		public ItemKey CreateClothing(DataContext context, short templateId, sbyte gender)
		{
			Tester.Assert(!Clothing.Instance[templateId].Stackable, "");
			int itemId = this.GenerateNextItemId(context);
			Clothing item = new Clothing(context.Random, templateId, itemId, gender);
			item.OfflineGenerateMaterialResources(context.Random);
			this.AddElement_Clothing(itemId, item);
			return item.GetItemKey();
		}

		// Token: 0x06004FF3 RID: 20467 RVA: 0x002BA550 File Offset: 0x002B8750
		public ItemKey CreateCarrier(DataContext context, short templateId)
		{
			Tester.Assert(!Carrier.Instance[templateId].Stackable, "");
			int itemId = this.GenerateNextItemId(context);
			Carrier item = new Carrier(context.Random, templateId, itemId);
			item.OfflineGenerateMaterialResources(context.Random);
			this.AddElement_Carriers(itemId, item);
			return item.GetItemKey();
		}

		// Token: 0x06004FF4 RID: 20468 RVA: 0x002BA5B4 File Offset: 0x002B87B4
		public ItemKey CreateMaterial(DataContext context, short templateId)
		{
			Tester.Assert(Material.Instance[templateId].Stackable, "");
			int itemId = this.GetStackableItem(context, 5, templateId);
			return new ItemKey(5, 0, templateId, itemId);
		}

		// Token: 0x06004FF5 RID: 20469 RVA: 0x002BA5F4 File Offset: 0x002B87F4
		public ItemKey CreateCraftTool(DataContext context, short templateId)
		{
			Tester.Assert(!CraftTool.Instance[templateId].Stackable, "");
			int itemId = this.GenerateNextItemId(context);
			CraftTool item = new CraftTool(context.Random, templateId, itemId);
			this.AddElement_CraftTools(itemId, item);
			return item.GetItemKey();
		}

		// Token: 0x06004FF6 RID: 20470 RVA: 0x002BA64C File Offset: 0x002B884C
		public ItemKey CreateFood(DataContext context, short templateId)
		{
			Tester.Assert(Food.Instance[templateId].Stackable, "");
			int itemId = this.GetStackableItem(context, 7, templateId);
			return new ItemKey(7, 0, templateId, itemId);
		}

		// Token: 0x06004FF7 RID: 20471 RVA: 0x002BA68C File Offset: 0x002B888C
		public ItemKey CreateMedicine(DataContext context, short templateId)
		{
			Tester.Assert(Medicine.Instance[templateId].Stackable, "");
			int itemId = this.GetStackableItem(context, 8, templateId);
			return new ItemKey(8, 0, templateId, itemId);
		}

		// Token: 0x06004FF8 RID: 20472 RVA: 0x002BA6CC File Offset: 0x002B88CC
		public ItemKey CreateTeaWine(DataContext context, short templateId)
		{
			Tester.Assert(TeaWine.Instance[templateId].Stackable, "");
			int itemId = this.GetStackableItem(context, 9, templateId);
			return new ItemKey(9, 0, templateId, itemId);
		}

		// Token: 0x06004FF9 RID: 20473 RVA: 0x002BA710 File Offset: 0x002B8910
		public ItemKey CreateDemandedSkillBook(DataContext context, short templateId, byte ensuredPageIndex, byte pageTypes = 0)
		{
			SkillBookItem bookCfg = SkillBook.Instance[templateId];
			bool flag = bookCfg.CombatSkillType >= 0;
			ItemKey itemKey;
			if (flag)
			{
				sbyte lostPagesCount = (sbyte)context.Random.Next(6);
				itemKey = DomainManager.Item.CreateSkillBook(context, templateId, pageTypes, 0, lostPagesCount, true);
			}
			else
			{
				sbyte lostPagesCount2 = (sbyte)context.Random.Next(6);
				itemKey = DomainManager.Item.CreateSkillBook(context, templateId, 0, lostPagesCount2, -1, 50, true);
			}
			SkillBook skillBook = this._skillBooks[itemKey.Id];
			sbyte skillGroup = SkillGroup.FromItemSubType(bookCfg.ItemSubType);
			sbyte creatingGrade = bookCfg.Grade + 2;
			ushort incompleteState = SkillBook.GeneratePageIncompleteState(context.Random, skillGroup, creatingGrade, -1, -1, true);
			incompleteState = SkillBookStateHelper.SetPageIncompleteState(incompleteState, ensuredPageIndex, 0);
			skillBook.SetPageIncompleteState(incompleteState, context);
			return itemKey;
		}

		// Token: 0x06004FFA RID: 20474 RVA: 0x002BA7E0 File Offset: 0x002B89E0
		public ItemKey CreateSkillBook(DataContext context, short templateId, sbyte completePagesCount = -1, sbyte lostPagesCount = -1, sbyte outlinePageType = -1, sbyte normalPagesDirectProb = 50, bool outlineAlwaysComplete = true)
		{
			Tester.Assert(!SkillBook.Instance[templateId].Stackable, "");
			int itemId = this.GenerateNextItemId(context);
			SkillBook item = new SkillBook(context.Random, templateId, itemId, completePagesCount, lostPagesCount, outlinePageType, normalPagesDirectProb, outlineAlwaysComplete);
			this.AddElement_SkillBooks(itemId, item);
			return item.GetItemKey();
		}

		// Token: 0x06004FFB RID: 20475 RVA: 0x002BA840 File Offset: 0x002B8A40
		public ItemKey CreateSkillBook(DataContext context, short templateId, byte pageTypes, sbyte completePagesCount = -1, sbyte lostPagesCount = -1, bool outlineAlwaysComplete = true)
		{
			Tester.Assert(!SkillBook.Instance[templateId].Stackable, "");
			int itemId = this.GenerateNextItemId(context);
			SkillBook item = new SkillBook(context.Random, templateId, itemId, pageTypes, completePagesCount, lostPagesCount, outlineAlwaysComplete);
			this.AddElement_SkillBooks(itemId, item);
			return item.GetItemKey();
		}

		// Token: 0x06004FFC RID: 20476 RVA: 0x002BA89C File Offset: 0x002B8A9C
		public ItemKey CreateSkillBook(DataContext context, short templateId, ushort activationState)
		{
			Tester.Assert(!SkillBook.Instance[templateId].Stackable, "");
			int itemId = this.GenerateNextItemId(context);
			SkillBook item = new SkillBook(context.Random, templateId, itemId, activationState);
			this.AddElement_SkillBooks(itemId, item);
			return item.GetItemKey();
		}

		// Token: 0x06004FFD RID: 20477 RVA: 0x002BA8F4 File Offset: 0x002B8AF4
		public ItemKey CreateCricket(DataContext context, short colorId, short partId)
		{
			int itemId = this.GenerateNextItemId(context);
			Cricket item = new Cricket(context.Random, colorId, partId, itemId);
			Tester.Assert(!Cricket.Instance[item.GetTemplateId()].Stackable, "");
			this.AddElement_Crickets(itemId, item);
			Events.RaiseCricketCreated(context, item.GetItemKey());
			return item.GetItemKey();
		}

		// Token: 0x06004FFE RID: 20478 RVA: 0x002BA95C File Offset: 0x002B8B5C
		public ItemKey CreateCricket(DataContext context, short templateId)
		{
			Tester.Assert(!Cricket.Instance[templateId].Stackable, "");
			int itemId = this.GenerateNextItemId(context);
			Cricket item = new Cricket(context.Random, templateId, itemId);
			this.AddElement_Crickets(itemId, item);
			Events.RaiseCricketCreated(context, item.GetItemKey());
			return item.GetItemKey();
		}

		// Token: 0x06004FFF RID: 20479 RVA: 0x002BA9C0 File Offset: 0x002B8BC0
		public ItemKey CreateCricket(DataContext context, short templateId, bool isSpecial)
		{
			Tester.Assert(!Cricket.Instance[templateId].Stackable, "");
			int itemId = this.GenerateNextItemId(context);
			Cricket item = new Cricket(context.Random, templateId, itemId, isSpecial);
			this.AddElement_Crickets(itemId, item);
			Events.RaiseCricketCreated(context, item.GetItemKey());
			return item.GetItemKey();
		}

		// Token: 0x06005000 RID: 20480 RVA: 0x002BAA24 File Offset: 0x002B8C24
		public ItemKey CreateMisc(DataContext context, short templateId)
		{
			bool stackable = Misc.Instance[templateId].Stackable;
			ItemKey result;
			if (stackable)
			{
				int itemId = this.GetStackableItem(context, 12, templateId);
				result = new ItemKey(12, 0, templateId, itemId);
			}
			else
			{
				int itemId2 = this.GenerateNextItemId(context);
				Misc item = new Misc(context.Random, templateId, itemId2);
				this.AddElement_Misc(itemId2, item);
				result = item.GetItemKey();
			}
			return result;
		}

		// Token: 0x06005001 RID: 20481 RVA: 0x002BAA8C File Offset: 0x002B8C8C
		public void RemoveItem(DataContext context, ItemKey itemKey)
		{
			bool flag = ItemTemplateHelper.IsPureStackable(itemKey);
			if (!flag)
			{
				int itemId = itemKey.Id;
				this._trackedSpecialItems.Remove(itemKey);
				this.RemoveItemInternal(itemKey.ItemType, itemId);
				byte state = itemKey.ModificationState;
				bool flag2 = ModificationStateHelper.IsActive(state, 1);
				if (flag2)
				{
					this.RemoveElement_PoisonItems(itemId, context);
					DomainManager.Extra.RemovePoisonEffect(context, itemId);
				}
				bool flag3 = ModificationStateHelper.IsActive(state, 2);
				if (flag3)
				{
					this.RemoveElement_RefinedItems(itemId, context);
				}
				bool flag4 = ModificationStateHelper.IsActive(state, 4);
				if (flag4)
				{
					DomainManager.Extra.RemoveLoveTokenData(context, itemKey, true);
				}
			}
		}

		// Token: 0x06005002 RID: 20482 RVA: 0x002BAB24 File Offset: 0x002B8D24
		public void ForceRemoveItem(DataContext context, ItemKey itemKey)
		{
			int itemId = itemKey.Id;
			this._trackedSpecialItems.Remove(itemKey);
			this.RemoveItemInternal(itemKey.ItemType, itemId);
			bool flag = this._poisonItems.ContainsKey(itemId);
			if (flag)
			{
				this.RemoveElement_PoisonItems(itemId, context);
			}
			bool flag2 = DomainManager.Extra.PoisonEffects.ContainsKey(itemId);
			if (flag2)
			{
				DomainManager.Extra.RemovePoisonEffect(context, itemId);
			}
			bool flag3 = this._refinedItems.ContainsKey(itemId);
			if (flag3)
			{
				this.RemoveElement_RefinedItems(itemId, context);
			}
		}

		// Token: 0x06005003 RID: 20483 RVA: 0x002BABA8 File Offset: 0x002B8DA8
		public void RemoveItems(DataContext context, List<ItemKey> itemKeys)
		{
			int i = 0;
			int count = itemKeys.Count;
			while (i < count)
			{
				this.RemoveItem(context, itemKeys[i]);
				i++;
			}
		}

		// Token: 0x06005004 RID: 20484 RVA: 0x002BABDC File Offset: 0x002B8DDC
		public void RemoveItems(DataContext context, Dictionary<ItemKey, int> items)
		{
			foreach (KeyValuePair<ItemKey, int> keyValuePair in items)
			{
				ItemKey itemKey2;
				int num;
				keyValuePair.Deconstruct(out itemKey2, out num);
				ItemKey itemKey = itemKey2;
				this.RemoveItem(context, itemKey);
			}
		}

		// Token: 0x06005005 RID: 20485 RVA: 0x002BAC40 File Offset: 0x002B8E40
		public void RemoveItems(DataContext context, List<ValueTuple<ItemKey, int>> items)
		{
			foreach (ValueTuple<ItemKey, int> valueTuple in items)
			{
				ItemKey itemKey = valueTuple.Item1;
				this.RemoveItem(context, itemKey);
			}
		}

		// Token: 0x06005006 RID: 20486 RVA: 0x002BAC98 File Offset: 0x002B8E98
		public static short GenerateRandomItemTemplateId(IRandomSource random, sbyte itemType, short groupBeginId, sbyte expectedGrade)
		{
			sbyte randomItemGrade = ItemDomain.GenerateRandomItemGrade(random, expectedGrade);
			bool flag = itemType == 8 && Medicine.Instance[groupBeginId].ItemSubType == 800;
			short result;
			if (flag)
			{
				randomItemGrade = ItemDomain.GetRandomMedicineGrade(randomItemGrade);
				result = groupBeginId + (short)randomItemGrade;
			}
			else
			{
				result = ItemTemplateHelper.GetTemplateIdInGroup(itemType, groupBeginId, randomItemGrade);
			}
			return result;
		}

		// Token: 0x06005007 RID: 20487 RVA: 0x002BACEC File Offset: 0x002B8EEC
		public static sbyte GenerateRandomItemGrade(IRandomSource random, sbyte itemGrade)
		{
			int mean = (int)(itemGrade + -2);
			return (sbyte)RedzenHelper.SkewDistribute(random, (float)mean, 0.333333f, 3f, 0, (int)itemGrade);
		}

		// Token: 0x06005008 RID: 20488 RVA: 0x002BAD18 File Offset: 0x002B8F18
		public static sbyte GetRandomMedicineGrade(sbyte generatedGrade)
		{
			generatedGrade = generatedGrade / 2 + 1;
			return (generatedGrade > 5) ? 5 : generatedGrade;
		}

		// Token: 0x06005009 RID: 20489 RVA: 0x002BAD3C File Offset: 0x002B8F3C
		private int GenerateNextItemId(DataContext context)
		{
			int itemId = this._nextItemId;
			this._nextItemId++;
			bool flag = this._nextItemId > int.MaxValue;
			if (flag)
			{
				this._nextItemId = 0;
			}
			this.SetNextItemId(this._nextItemId, context);
			return itemId;
		}

		// Token: 0x0600500A RID: 20490 RVA: 0x002BAD8C File Offset: 0x002B8F8C
		private int GetStackableItem(DataContext context, sbyte itemType, short templateId)
		{
			TemplateKey templateKey = new TemplateKey(itemType, templateId);
			int itemId;
			bool flag = this._stackableItems.TryGetValue(templateKey, out itemId);
			int result;
			if (flag)
			{
				result = itemId;
			}
			else
			{
				ItemBase item = this.CreateItemInternal(context, itemType, templateId);
				Tester.Assert(item.GetModificationState() == 0, "");
				itemId = item.GetId();
				this.AddElement_StackableItems(templateKey, itemId, context);
				result = itemId;
			}
			return result;
		}

		// Token: 0x0600500B RID: 20491 RVA: 0x002BADF0 File Offset: 0x002B8FF0
		public ItemBase CreateUniqueStackableItem(DataContext context, sbyte itemType, short templateId)
		{
			ItemBase item = this.CreateItemInternal(context, itemType, templateId);
			Tester.Assert(item.GetModificationState() == 0, "");
			return item;
		}

		// Token: 0x0600500C RID: 20492 RVA: 0x002BAE24 File Offset: 0x002B9024
		private ItemBase CreateItemInternal(DataContext context, sbyte itemType, short templateId)
		{
			IRandomSource random = context.Random;
			int itemId = this.GenerateNextItemId(context);
			ItemBase result;
			switch (itemType)
			{
			case 0:
			{
				Weapon item = new Weapon(random, templateId, itemId);
				bool flag = context.Random.CheckPercentProb((int)GlobalConfig.Instance.EquipmentWithEffectRate);
				if (flag)
				{
					item.OfflineGenerateEquipmentEffect(context.Random);
				}
				item.OfflineGenerateMaterialResources(context.Random);
				this.AddElement_Weapons(itemId, item);
				result = item;
				break;
			}
			case 1:
			{
				Armor item2 = new Armor(random, templateId, itemId);
				bool flag2 = context.Random.CheckPercentProb((int)GlobalConfig.Instance.EquipmentWithEffectRate);
				if (flag2)
				{
					item2.OfflineGenerateEquipmentEffect(context.Random);
				}
				item2.OfflineGenerateMaterialResources(context.Random);
				this.AddElement_Armors(itemId, item2);
				result = item2;
				break;
			}
			case 2:
			{
				Accessory item3 = new Accessory(random, templateId, itemId);
				bool flag3 = context.Random.CheckPercentProb((int)GlobalConfig.Instance.EquipmentWithEffectRate);
				if (flag3)
				{
					item3.OfflineGenerateEquipmentEffect(context.Random);
				}
				item3.OfflineGenerateMaterialResources(context.Random);
				this.AddElement_Accessories(itemId, item3);
				result = item3;
				break;
			}
			case 3:
			{
				Clothing item4 = new Clothing(random, templateId, itemId, -1);
				item4.OfflineGenerateMaterialResources(context.Random);
				this.AddElement_Clothing(itemId, item4);
				result = item4;
				break;
			}
			case 4:
			{
				Carrier item5 = new Carrier(random, templateId, itemId);
				item5.OfflineGenerateMaterialResources(context.Random);
				this.AddElement_Carriers(itemId, item5);
				Events.RaiseCarrierCreated(context, item5.GetItemKey());
				result = item5;
				break;
			}
			case 5:
			{
				Material item6 = new Material(random, templateId, itemId);
				this.AddElement_Materials(itemId, item6);
				result = item6;
				break;
			}
			case 6:
			{
				CraftTool item7 = new CraftTool(random, templateId, itemId);
				this.AddElement_CraftTools(itemId, item7);
				result = item7;
				break;
			}
			case 7:
			{
				Food item8 = new Food(random, templateId, itemId);
				this.AddElement_Foods(itemId, item8);
				result = item8;
				break;
			}
			case 8:
			{
				Medicine item9 = new Medicine(random, templateId, itemId);
				this.AddElement_Medicines(itemId, item9);
				result = item9;
				break;
			}
			case 9:
			{
				TeaWine item10 = new TeaWine(random, templateId, itemId);
				this.AddElement_TeaWines(itemId, item10);
				result = item10;
				break;
			}
			case 10:
			{
				SkillBook item11 = new SkillBook(random, templateId, itemId, -1, -1, -1, 50, true);
				this.AddElement_SkillBooks(itemId, item11);
				result = item11;
				break;
			}
			case 11:
			{
				Cricket item12 = new Cricket(random, templateId, itemId);
				this.AddElement_Crickets(itemId, item12);
				Events.RaiseCricketCreated(context, item12.GetItemKey());
				result = item12;
				break;
			}
			case 12:
			{
				Misc item13 = new Misc(random, templateId, itemId);
				this.AddElement_Misc(itemId, item13);
				this.CheckAndTrackSpecialItem(item13.GetItemKey());
				result = item13;
				break;
			}
			default:
				throw ItemTemplateHelper.CreateItemTypeException(itemType);
			}
			return result;
		}

		// Token: 0x0600500D RID: 20493 RVA: 0x002BB0FC File Offset: 0x002B92FC
		private void RemoveItemInternal(sbyte itemType, int itemId)
		{
			switch (itemType)
			{
			case 0:
				this.RemoveElement_Weapons(itemId);
				break;
			case 1:
				this.RemoveElement_Armors(itemId);
				break;
			case 2:
				this.RemoveElement_Accessories(itemId);
				break;
			case 3:
				this.RemoveElement_Clothing(itemId);
				break;
			case 4:
				Events.RaiseCarrierRemoved(DataContextManager.GetCurrentThreadDataContext(), this.GetElement_Carriers(itemId).GetItemKey());
				this.RemoveElement_Carriers(itemId);
				break;
			case 5:
				this.RemoveElement_Materials(itemId);
				break;
			case 6:
				this.RemoveElement_CraftTools(itemId);
				break;
			case 7:
				this.RemoveElement_Foods(itemId);
				break;
			case 8:
				this.RemoveElement_Medicines(itemId);
				break;
			case 9:
				this.RemoveElement_TeaWines(itemId);
				break;
			case 10:
				this.RemoveElement_SkillBooks(itemId);
				break;
			case 11:
				Events.RaiseCricketRemoved(DataContextManager.GetCurrentThreadDataContext(), this.GetElement_Crickets(itemId).GetItemKey());
				this.RemoveElement_Crickets(itemId);
				break;
			case 12:
				this.RemoveElement_Misc(itemId);
				break;
			default:
				throw ItemTemplateHelper.CreateItemTypeException(itemType);
			}
		}

		// Token: 0x0600500E RID: 20494 RVA: 0x002BB220 File Offset: 0x002B9420
		public bool IsInStackableItems(ItemKey itemKey)
		{
			TemplateKey templateKey = new TemplateKey(itemKey.ItemType, itemKey.TemplateId);
			int id;
			return this._stackableItems.TryGetValue(templateKey, out id) && itemKey.Id == id;
		}

		// Token: 0x0600500F RID: 20495 RVA: 0x002BB264 File Offset: 0x002B9464
		public override void UnpackCrossArchiveGameData(DataContext context, CrossArchiveGameData crossArchiveGameData)
		{
			foreach (KeyValuePair<int, ItemBase> pair in crossArchiveGameData.ItemGroupPackage.Items)
			{
				this.UnpackCrossArchiveItem(context, crossArchiveGameData, pair.Key);
			}
			crossArchiveGameData.ItemGroupPackage = null;
		}

		// Token: 0x06005010 RID: 20496 RVA: 0x002BB2D0 File Offset: 0x002B94D0
		internal void PackCrossArchiveItem(CrossArchiveGameData crossArchiveGameData, ItemKey itemKey)
		{
			bool flag = !this.ItemExists(itemKey);
			if (!flag)
			{
				ItemBase item = DomainManager.Item.GetBaseItem(itemKey);
				item.ResetOwner();
				if (crossArchiveGameData.ItemGroupPackage == null)
				{
					crossArchiveGameData.ItemGroupPackage = new ItemGroupPackage();
				}
				this.PackItem(crossArchiveGameData.ItemGroupPackage, item);
			}
		}

		// Token: 0x06005011 RID: 20497 RVA: 0x002BB324 File Offset: 0x002B9524
		internal ItemKey UnpackCrossArchiveItem(DataContext context, CrossArchiveGameData crossArchiveGameData, ItemKey srcItemKey)
		{
			return this.UnpackCrossArchiveItem(context, crossArchiveGameData, srcItemKey.Id);
		}

		// Token: 0x06005012 RID: 20498 RVA: 0x002BB334 File Offset: 0x002B9534
		internal ItemKey UnpackCrossArchiveItem(DataContext context, CrossArchiveGameData crossArchiveGameData, int srcItemId)
		{
			if (crossArchiveGameData.UnpackedItems == null)
			{
				crossArchiveGameData.UnpackedItems = new Dictionary<int, ItemKey>();
			}
			ItemKey unpackedItemKey;
			bool flag = crossArchiveGameData.UnpackedItems.TryGetValue(srcItemId, out unpackedItemKey);
			ItemKey result;
			if (flag)
			{
				result = unpackedItemKey;
			}
			else
			{
				bool flag2 = crossArchiveGameData.ItemGroupPackage == null;
				if (flag2)
				{
					result = ItemKey.Invalid;
				}
				else
				{
					ItemKey itemKey = this.UnpackItem(context, crossArchiveGameData.ItemGroupPackage, srcItemId);
					bool flag3 = itemKey.IsValid();
					if (flag3)
					{
						crossArchiveGameData.UnpackedItems.Add(srcItemId, itemKey);
						bool flag4 = itemKey.ItemType == 11;
						if (flag4)
						{
							crossArchiveGameData.CricketCombatPlans.ReplaceCricket(srcItemId, itemKey);
						}
					}
					result = itemKey;
				}
			}
			return result;
		}

		// Token: 0x06005013 RID: 20499 RVA: 0x002BB3D8 File Offset: 0x002B95D8
		public void PackItem(ItemGroupPackage package, ItemBase item)
		{
			ItemKey itemKey = item.GetItemKey();
			bool flag = !package.Items.TryAdd(itemKey.Id, item);
			if (!flag)
			{
				bool flag2 = ModificationStateHelper.IsActive(itemKey.ModificationState, 2);
				if (flag2)
				{
					RefiningEffects refiningEffects = DomainManager.Item.GetRefinedEffects(itemKey);
					if (package.RefiningEffects == null)
					{
						package.RefiningEffects = new Dictionary<int, RefiningEffects>();
					}
					package.RefiningEffects.Add(itemKey.Id, refiningEffects);
				}
				bool flag3 = ModificationStateHelper.IsActive(itemKey.ModificationState, 1);
				if (flag3)
				{
					FullPoisonEffects poisonEffects = DomainManager.Item.GetPoisonEffects(itemKey);
					if (package.FullPoisonEffects == null)
					{
						package.FullPoisonEffects = new Dictionary<int, FullPoisonEffects>();
					}
					package.FullPoisonEffects.Add(itemKey.Id, new FullPoisonEffects(poisonEffects));
				}
				sbyte itemType = itemKey.ItemType;
				sbyte b = itemType;
				switch (b)
				{
				case 3:
				{
					short modifiedTemplateId = DomainManager.Extra.GetModifiedClothingTemplateId(itemKey);
					bool flag4 = modifiedTemplateId != itemKey.TemplateId;
					if (flag4)
					{
						if (package.ClothingDisplayModifications == null)
						{
							package.ClothingDisplayModifications = new Dictionary<int, short>();
						}
						package.ClothingDisplayModifications.Add(itemKey.Id, modifiedTemplateId);
					}
					break;
				}
				case 4:
				{
					int tamePoint = DomainManager.Extra.GetCarrierTamePoint(itemKey.Id);
					bool flag5 = tamePoint >= 0;
					if (flag5)
					{
						package.CarrierTamePoint.Add(itemKey.Id, tamePoint);
					}
					int jiaoId;
					GameData.DLC.FiveLoong.Jiao jiao;
					bool flag6 = DomainManager.Extra.TryGetJiaoIdByItemKey(itemKey, out jiaoId) && DomainManager.Extra.TryGetJiao(jiaoId, out jiao);
					if (flag6)
					{
						jiao.PettingCoolDown = -1;
						package.Jiaos.Add(jiaoId, jiao);
						package.JiaoKeyToId.Add(itemKey, jiaoId);
					}
					else
					{
						int loongId;
						ChildrenOfLoong loong;
						bool flag7 = DomainManager.Extra.TryGetChildrenOfLoongIdByItemKey(itemKey, out loongId) && DomainManager.Extra.TryGetLoong(loongId, out loong);
						if (flag7)
						{
							package.ChildrenOfLoong.Add(loongId, loong);
							package.ChildrenOfLoongKeyToId.Add(itemKey, loongId);
						}
					}
					break;
				}
				case 5:
				{
					int jiaoId2;
					GameData.DLC.FiveLoong.Jiao jiao2;
					bool flag8 = DomainManager.Extra.TryGetJiaoIdByItemKey(itemKey, out jiaoId2) && DomainManager.Extra.TryGetJiao(jiaoId2, out jiao2);
					if (flag8)
					{
						jiao2.PettingCoolDown = -1;
						package.Jiaos.Add(jiaoId2, jiao2);
						package.JiaoKeyToId.Add(itemKey, jiaoId2);
					}
					break;
				}
				default:
					if (b == 11)
					{
						bool isSmart = DomainManager.Extra.IsCricketSmart(itemKey.Id);
						bool isIdentified = DomainManager.Extra.IsCricketIdentified(itemKey.Id);
						bool flag9 = isSmart;
						if (flag9)
						{
							package.CricketIsSmart.Add(itemKey.Id, true);
						}
						bool flag10 = isIdentified;
						if (flag10)
						{
							package.CricketIsIdentified.Add(itemKey.Id, true);
						}
					}
					break;
				}
			}
		}

		// Token: 0x06005014 RID: 20500 RVA: 0x002BB6AC File Offset: 0x002B98AC
		public ItemKey UnpackItem(DataContext context, ItemGroupPackage package, int srcItemId)
		{
			ItemBase srcItem;
			bool flag = package.Items == null || !package.Items.TryGetValue(srcItemId, out srcItem);
			ItemKey result;
			if (flag)
			{
				result = ItemKey.Invalid;
			}
			else
			{
				ItemKey srcItemKey = srcItem.GetItemKey();
				bool flag2 = ItemTemplateHelper.IsPureStackable(srcItemKey);
				if (flag2)
				{
					ItemKey dstItemKey = srcItemKey;
					dstItemKey.Id = this.GetStackableItem(context, srcItemKey.ItemType, srcItemKey.TemplateId);
					result = dstItemKey;
				}
				else
				{
					int itemId = this.GenerateNextItemId(context);
					bool flag3 = ModificationStateHelper.IsActive(srcItemKey.ModificationState, 2);
					if (flag3)
					{
						RefiningEffects refiningEffect = package.RefiningEffects[srcItemId];
						this.AddElement_RefinedItems(itemId, refiningEffect, context);
					}
					bool flag4 = ModificationStateHelper.IsActive(srcItemKey.ModificationState, 1);
					if (flag4)
					{
						FullPoisonEffects poisonEffect = package.FullPoisonEffects[srcItemId];
						DomainManager.Extra.SetPoisonEffect(context, itemId, new FullPoisonEffects(poisonEffect));
					}
					switch (srcItemKey.ItemType)
					{
					case 0:
					{
						Weapon item = Serializer.CreateCopy<Weapon>((Weapon)srcItem);
						item.OfflineSetId(itemId);
						item.OfflineSetEquippedCharId(-1);
						this.AddElement_Weapons(itemId, item);
						ItemKey itemKey = item.GetItemKey();
						result = itemKey;
						break;
					}
					case 1:
					{
						Armor item2 = Serializer.CreateCopy<Armor>((Armor)srcItem);
						item2.OfflineSetId(itemId);
						item2.OfflineSetEquippedCharId(-1);
						this.AddElement_Armors(itemId, item2);
						ItemKey itemKey2 = item2.GetItemKey();
						result = itemKey2;
						break;
					}
					case 2:
					{
						Accessory item3 = Serializer.CreateCopy<Accessory>((Accessory)srcItem);
						item3.OfflineSetId(itemId);
						item3.OfflineSetEquippedCharId(-1);
						this.AddElement_Accessories(itemId, item3);
						ItemKey itemKey3 = item3.GetItemKey();
						result = itemKey3;
						break;
					}
					case 3:
					{
						Clothing item4 = Serializer.CreateCopy<Clothing>((Clothing)srcItem);
						item4.OfflineSetId(itemId);
						item4.OfflineSetEquippedCharId(-1);
						this.AddElement_Clothing(itemId, item4);
						ItemKey itemKey4 = item4.GetItemKey();
						short modifiedTemplateId;
						bool flag5 = package.ClothingDisplayModifications != null && package.ClothingDisplayModifications.TryGetValue(srcItemId, out modifiedTemplateId);
						if (flag5)
						{
							DomainManager.Extra.SetClothingDisplayModification(context, itemKey4, modifiedTemplateId);
						}
						result = itemKey4;
						break;
					}
					case 4:
					{
						Carrier item5 = Serializer.CreateCopy<Carrier>((Carrier)srcItem);
						item5.OfflineSetId(itemId);
						item5.OfflineSetEquippedCharId(-1);
						this.AddElement_Carriers(itemId, item5);
						ItemKey itemKey5 = item5.GetItemKey();
						int tamePoint;
						bool flag6 = package.CarrierTamePoint.TryGetValue(srcItemKey.Id, out tamePoint);
						if (flag6)
						{
							DomainManager.Extra.SetCarrierTamePoint(context, itemKey5.Id, tamePoint);
						}
						int id;
						GameData.DLC.FiveLoong.Jiao jiao;
						bool flag7 = package.JiaoKeyToId.TryGetValue(srcItemKey, out id) && package.Jiaos.TryGetValue(id, out jiao);
						if (flag7)
						{
							DomainManager.Extra.SetJiaoItemKey(context, id, jiao, itemKey5);
						}
						else
						{
							ChildrenOfLoong loong;
							bool flag8 = package.ChildrenOfLoongKeyToId.TryGetValue(srcItemKey, out id) && package.ChildrenOfLoong.TryGetValue(id, out loong);
							if (flag8)
							{
								DomainManager.Extra.SetChildrenOfLoongItemKey(context, id, loong, itemKey5);
							}
						}
						result = itemKey5;
						break;
					}
					case 5:
					{
						Material item6 = Serializer.CreateCopy<Material>((Material)srcItem);
						item6.OfflineSetId(itemId);
						this.AddElement_Materials(itemId, item6);
						ItemKey itemKey6 = item6.GetItemKey();
						int id2;
						GameData.DLC.FiveLoong.Jiao jiao2;
						bool flag9 = package.JiaoKeyToId.TryGetValue(srcItemKey, out id2) && package.Jiaos.TryGetValue(id2, out jiao2);
						if (flag9)
						{
							DomainManager.Extra.SetJiaoItemKey(context, id2, jiao2, itemKey6);
						}
						result = itemKey6;
						break;
					}
					case 6:
					{
						CraftTool item7 = Serializer.CreateCopy<CraftTool>((CraftTool)srcItem);
						item7.OfflineSetId(itemId);
						this.AddElement_CraftTools(itemId, item7);
						ItemKey itemKey7 = item7.GetItemKey();
						result = itemKey7;
						break;
					}
					case 7:
					{
						Food item8 = Serializer.CreateCopy<Food>((Food)srcItem);
						item8.OfflineSetId(itemId);
						this.AddElement_Foods(itemId, item8);
						ItemKey itemKey8 = item8.GetItemKey();
						result = itemKey8;
						break;
					}
					case 8:
					{
						Medicine item9 = Serializer.CreateCopy<Medicine>((Medicine)srcItem);
						item9.OfflineSetId(itemId);
						this.AddElement_Medicines(itemId, item9);
						ItemKey itemKey9 = item9.GetItemKey();
						result = itemKey9;
						break;
					}
					case 9:
					{
						TeaWine item10 = Serializer.CreateCopy<TeaWine>((TeaWine)srcItem);
						item10.OfflineSetId(itemId);
						this.AddElement_TeaWines(itemId, item10);
						ItemKey itemKey10 = item10.GetItemKey();
						result = itemKey10;
						break;
					}
					case 10:
					{
						SkillBook item11 = Serializer.CreateCopy<SkillBook>((SkillBook)srcItem);
						item11.OfflineSetId(itemId);
						this.AddElement_SkillBooks(itemId, item11);
						ItemKey itemKey11 = item11.GetItemKey();
						result = itemKey11;
						break;
					}
					case 11:
					{
						Cricket item12 = Serializer.CreateCopy<Cricket>((Cricket)srcItem);
						item12.OfflineSetId(itemId);
						this.AddElement_Crickets(itemId, item12);
						Events.RaiseCricketCreated(context, item12.GetItemKey());
						ItemKey itemKey12 = item12.GetItemKey();
						bool flag10 = package.CricketIsSmart.ContainsKey(srcItemKey.Id);
						if (flag10)
						{
							DomainManager.Extra.ForceCricketSmart(context, itemKey12);
						}
						bool flag11 = package.CricketIsIdentified.ContainsKey(srcItemKey.Id);
						if (flag11)
						{
							DomainManager.Extra.SetCricketIdentified(context, itemKey12.Id);
						}
						DomainManager.Taiwu.ReplaceCricketPlan(context, srcItemKey, itemKey12);
						result = itemKey12;
						break;
					}
					case 12:
					{
						int id3;
						GameData.DLC.FiveLoong.Jiao jiao3;
						bool flag12 = package.JiaoKeyToId.TryGetValue(srcItemKey, out id3) && package.Jiaos.TryGetValue(id3, out jiao3);
						if (flag12)
						{
							Material item13 = new Material(context.Random, (jiao3.GrowthStage == 0) ? DomainManager.Extra.GetJiaoEggTemplateIdByJiaoTemplateId(jiao3.TemplateId) : DomainManager.Extra.GetJiaoTeenagerTemplateIdByJiaoTemplateId(jiao3.TemplateId), itemId);
							this.AddElement_Materials(itemId, item13);
							ItemKey itemKey13 = item13.GetItemKey();
							DomainManager.Extra.SetJiaoItemKey(context, id3, jiao3, itemKey13);
							result = itemKey13;
						}
						else
						{
							Misc item14 = Serializer.CreateCopy<Misc>((Misc)srcItem);
							item14.OfflineSetId(itemId);
							this.AddElement_Misc(itemId, item14);
							ItemKey itemKey14 = item14.GetItemKey();
							this.CheckAndTrackSpecialItem(itemKey14);
							result = itemKey14;
						}
						break;
					}
					default:
						throw ItemTemplateHelper.CreateItemTypeException(srcItemKey.ItemType);
					}
				}
			}
			return result;
		}

		// Token: 0x06005015 RID: 20501 RVA: 0x002BBCB8 File Offset: 0x002B9EB8
		public void AddExternEquipmentEffect(DataContext context, ItemKey itemKey, short effectId)
		{
			GameData.Utilities.ShortList effectIds;
			bool exist = this._externEquipmentEffects.TryGetValue(itemKey.Id, out effectIds);
			ref List<short> ptr = ref effectIds.Items;
			if (ptr == null)
			{
				ptr = new List<short>();
			}
			effectIds.Items.Add(effectId);
			bool flag = exist;
			if (flag)
			{
				this.SetElement_ExternEquipmentEffects(itemKey.Id, effectIds, context);
			}
			else
			{
				this.AddElement_ExternEquipmentEffects(itemKey.Id, effectIds, context);
			}
			EquipmentBase equipment = this.GetBaseEquipment(itemKey);
			equipment.SetEquipmentEffectId(equipment.GetEquipmentEffectId(), context);
		}

		// Token: 0x06005016 RID: 20502 RVA: 0x002BBD38 File Offset: 0x002B9F38
		public void RemoveExternEquipmentEffect(DataContext context, ItemKey itemKey, short effectId)
		{
			GameData.Utilities.ShortList effectIds;
			bool flag = !this.TryGetElement_ExternEquipmentEffects(itemKey.Id, out effectIds);
			if (!flag)
			{
				effectIds.Items.Remove(effectId);
				bool flag2 = effectIds.Items.Count > 0;
				if (flag2)
				{
					this.SetElement_ExternEquipmentEffects(itemKey.Id, effectIds, context);
				}
				else
				{
					this.RemoveElement_ExternEquipmentEffects(itemKey.Id, context);
				}
				EquipmentBase equipment = this.TryGetBaseEquipment(itemKey);
				if (equipment != null)
				{
					equipment.SetEquipmentEffectId(equipment.GetEquipmentEffectId(), context);
				}
			}
		}

		// Token: 0x06005017 RID: 20503 RVA: 0x002BBDB6 File Offset: 0x002B9FB6
		public IEnumerable<EquipmentEffectItem> GetEquipmentEffects(EquipmentBase equipment)
		{
			bool flag = equipment.GetEquipmentEffectId() >= 0;
			if (flag)
			{
				yield return EquipmentEffect.Instance[equipment.GetEquipmentEffectId()];
			}
			GameData.Utilities.ShortList effectIds;
			bool flag2 = !this.TryGetElement_ExternEquipmentEffects(equipment.GetId(), out effectIds);
			if (flag2)
			{
				yield break;
			}
			foreach (short effectId in effectIds.Items)
			{
				yield return EquipmentEffect.Instance[effectId];
			}
			List<short>.Enumerator enumerator = default(List<short>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06005018 RID: 20504 RVA: 0x002BBDD0 File Offset: 0x002B9FD0
		[DomainMethod]
		public void ChangeDurability(DataContext dataContext, int charId, short changeValue, sbyte itemType, short startId, short endId)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			ItemKey[] equipment = character.GetEquipment();
			foreach (ItemKey itemKey in equipment)
			{
				bool flag = !itemKey.IsValid();
				if (!flag)
				{
					ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
					bool flag2 = itemKey.ItemType == itemType && itemKey.TemplateId >= startId && itemKey.TemplateId <= endId;
					if (flag2)
					{
						int durability = (int)(baseItem.GetCurrDurability() + changeValue);
						durability = Math.Clamp(durability, 0, (int)baseItem.GetMaxDurability());
						baseItem.SetCurrDurability((short)durability, dataContext);
					}
				}
			}
			Inventory inventory = character.GetInventory();
			foreach (KeyValuePair<ItemKey, int> keyValuePair in inventory.Items)
			{
				ItemKey itemKey3;
				int num;
				keyValuePair.Deconstruct(out itemKey3, out num);
				ItemKey itemKey2 = itemKey3;
				ItemBase baseItem2 = DomainManager.Item.GetBaseItem(itemKey2);
				bool flag3 = itemKey2.ItemType == itemType && itemKey2.TemplateId >= startId && itemKey2.TemplateId <= endId;
				if (flag3)
				{
					int durability2 = (int)(baseItem2.GetCurrDurability() + changeValue);
					durability2 = Math.Clamp(durability2, 0, (int)baseItem2.GetMaxDurability());
					baseItem2.SetCurrDurability((short)durability2, dataContext);
				}
			}
		}

		// Token: 0x06005019 RID: 20505 RVA: 0x002BBF5C File Offset: 0x002BA15C
		[DomainMethod]
		public void ChangePoisonIdentified(DataContext dataContext, int charId, bool isIdentified)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			Inventory inventory = character.GetInventory();
			foreach (ItemKey itemsKey in inventory.Items.Keys)
			{
				this.SetPoisonsIdentified(dataContext, itemsKey, isIdentified);
			}
		}

		// Token: 0x0600501A RID: 20506 RVA: 0x002BBFD0 File Offset: 0x002BA1D0
		[DomainMethod]
		public void GmCmd_StartCricketCombat(DataContext context, int enemyId)
		{
			Wager selfWager = Wager.CreateExp(0);
			List<CricketWagerData> list = this.SelectCricketWagers(context, enemyId);
			CricketWagerData enemyWagerData = list[list.Count - 1];
			this.SetWager(selfWager, enemyWagerData.Wager);
			GameDataBridge.AddDisplayEvent<int, Wager, CricketWagerData>(DisplayEventType.OpenCricketBattle, enemyId, selfWager, enemyWagerData);
		}

		// Token: 0x0600501B RID: 20507 RVA: 0x002BC014 File Offset: 0x002BA214
		public static void RegisterItemOwners(DataContext context, DataUid uid)
		{
			DomainManager.Item.InitializeOwnedItems();
			DomainManager.Organization.InitializeOwnedItems();
			DomainManager.Character.InitializeOwnedItems();
			DomainManager.Taiwu.InitializeOwnedItems();
			DomainManager.Map.InitializeOwnedItems();
			DomainManager.Merchant.InitializeOwnedItems();
			DomainManager.Building.InitializeOwnedItems();
			DomainManager.LegendaryBook.InitializeOwnedItems();
			DomainManager.Extra.InitializeOwnedItems();
			DomainManager.Item.CheckUnownedItems();
			GameDataBridge.RemovePostDataModificationHandler(uid, "RegisterItemOwners");
		}

		// Token: 0x0600501C RID: 20508 RVA: 0x002BC09C File Offset: 0x002BA29C
		private void InitializeOwnedItems()
		{
			this.SetOwner(this._emptyHandKey, ItemOwnerType.System, 6);
			this.SetOwner(this._branchKey, ItemOwnerType.System, 6);
			this.SetOwner(this._stoneKey, ItemOwnerType.System, 6);
		}

		// Token: 0x0600501D RID: 20509 RVA: 0x002BC0CC File Offset: 0x002BA2CC
		public unsafe void CheckUnownedItems()
		{
			Span<int> span = new Span<int>(stackalloc byte[(UIntPtr)52], 13);
			Span<int> unownedCount = span;
			foreach (KeyValuePair<int, Weapon> keyValuePair in this._weapons)
			{
				int num;
				Weapon weapon;
				keyValuePair.Deconstruct(out num, out weapon);
				Weapon item = weapon;
				bool flag = !ItemDomain.IsPureStackable(item) && item.Owner.OwnerType == ItemOwnerType.None;
				if (flag)
				{
					(*unownedCount[0])++;
					this.LogUnownedItem(item);
				}
			}
			foreach (KeyValuePair<int, Armor> keyValuePair2 in this._armors)
			{
				int num;
				Armor armor;
				keyValuePair2.Deconstruct(out num, out armor);
				Armor item2 = armor;
				bool flag2 = !ItemDomain.IsPureStackable(item2) && item2.Owner.OwnerType == ItemOwnerType.None;
				if (flag2)
				{
					(*unownedCount[1])++;
					this.LogUnownedItem(item2);
				}
			}
			foreach (KeyValuePair<int, Accessory> keyValuePair3 in this._accessories)
			{
				int num;
				Accessory accessory;
				keyValuePair3.Deconstruct(out num, out accessory);
				Accessory item3 = accessory;
				bool flag3 = !ItemDomain.IsPureStackable(item3) && item3.Owner.OwnerType == ItemOwnerType.None;
				if (flag3)
				{
					(*unownedCount[2])++;
					this.LogUnownedItem(item3);
				}
			}
			foreach (KeyValuePair<int, Clothing> keyValuePair4 in this._clothing)
			{
				int num;
				Clothing clothing;
				keyValuePair4.Deconstruct(out num, out clothing);
				Clothing item4 = clothing;
				bool flag4 = !ItemDomain.IsPureStackable(item4) && item4.Owner.OwnerType == ItemOwnerType.None;
				if (flag4)
				{
					(*unownedCount[3])++;
					this.LogUnownedItem(item4);
				}
			}
			foreach (KeyValuePair<int, Carrier> keyValuePair5 in this._carriers)
			{
				int num;
				Carrier carrier;
				keyValuePair5.Deconstruct(out num, out carrier);
				Carrier item5 = carrier;
				bool flag5 = !ItemDomain.IsPureStackable(item5) && item5.Owner.OwnerType == ItemOwnerType.None;
				if (flag5)
				{
					(*unownedCount[4])++;
					this.LogUnownedItem(item5);
				}
			}
			foreach (KeyValuePair<int, Material> keyValuePair6 in this._materials)
			{
				int num;
				Material material;
				keyValuePair6.Deconstruct(out num, out material);
				Material item6 = material;
				bool flag6 = !ItemDomain.IsPureStackable(item6) && item6.Owner.OwnerType == ItemOwnerType.None;
				if (flag6)
				{
					(*unownedCount[5])++;
					this.LogUnownedItem(item6);
				}
			}
			foreach (KeyValuePair<int, CraftTool> keyValuePair7 in this._craftTools)
			{
				int num;
				CraftTool craftTool;
				keyValuePair7.Deconstruct(out num, out craftTool);
				CraftTool item7 = craftTool;
				bool flag7 = !ItemDomain.IsPureStackable(item7) && item7.Owner.OwnerType == ItemOwnerType.None;
				if (flag7)
				{
					(*unownedCount[6])++;
					this.LogUnownedItem(item7);
				}
			}
			foreach (KeyValuePair<int, Food> keyValuePair8 in this._foods)
			{
				int num;
				Food food;
				keyValuePair8.Deconstruct(out num, out food);
				Food item8 = food;
				bool flag8 = !ItemDomain.IsPureStackable(item8) && item8.Owner.OwnerType == ItemOwnerType.None;
				if (flag8)
				{
					(*unownedCount[7])++;
					this.LogUnownedItem(item8);
				}
			}
			foreach (KeyValuePair<int, Medicine> keyValuePair9 in this._medicines)
			{
				int num;
				Medicine medicine;
				keyValuePair9.Deconstruct(out num, out medicine);
				Medicine item9 = medicine;
				bool flag9 = !ItemDomain.IsPureStackable(item9) && item9.Owner.OwnerType == ItemOwnerType.None;
				if (flag9)
				{
					(*unownedCount[8])++;
					this.LogUnownedItem(item9);
				}
			}
			foreach (KeyValuePair<int, TeaWine> keyValuePair10 in this._teaWines)
			{
				int num;
				TeaWine teaWine;
				keyValuePair10.Deconstruct(out num, out teaWine);
				TeaWine item10 = teaWine;
				bool flag10 = !ItemDomain.IsPureStackable(item10) && item10.Owner.OwnerType == ItemOwnerType.None;
				if (flag10)
				{
					(*unownedCount[9])++;
					this.LogUnownedItem(item10);
				}
			}
			foreach (KeyValuePair<int, SkillBook> keyValuePair11 in this._skillBooks)
			{
				int num;
				SkillBook skillBook;
				keyValuePair11.Deconstruct(out num, out skillBook);
				SkillBook item11 = skillBook;
				bool flag11 = !ItemDomain.IsPureStackable(item11) && item11.Owner.OwnerType == ItemOwnerType.None;
				if (flag11)
				{
					(*unownedCount[10])++;
					this.LogUnownedItem(item11);
				}
			}
			foreach (KeyValuePair<int, Cricket> keyValuePair12 in this._crickets)
			{
				int num;
				Cricket cricket;
				keyValuePair12.Deconstruct(out num, out cricket);
				Cricket item12 = cricket;
				bool flag12 = !ItemDomain.IsPureStackable(item12) && item12.Owner.OwnerType == ItemOwnerType.None;
				if (flag12)
				{
					(*unownedCount[11])++;
					this.LogUnownedItem(item12);
				}
			}
			foreach (KeyValuePair<int, Misc> keyValuePair13 in this._misc)
			{
				int num;
				Misc misc;
				keyValuePair13.Deconstruct(out num, out misc);
				Misc item13 = misc;
				bool flag13 = !ItemDomain.IsPureStackable(item13) && item13.Owner.OwnerType == ItemOwnerType.None;
				if (flag13)
				{
					(*unownedCount[12])++;
					this.LogUnownedItem(item13);
				}
			}
			int totalCount = 0;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			for (sbyte itemType = 0; itemType < 13; itemType += 1)
			{
				bool flag14 = *unownedCount[(int)itemType] > 0;
				if (flag14)
				{
					totalCount += *unownedCount[(int)itemType];
					Logger logger = ItemDomain.Logger;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 2);
					defaultInterpolatedStringHandler.AppendFormatted<int>(*unownedCount[(int)itemType]);
					defaultInterpolatedStringHandler.AppendLiteral(" unowned ");
					defaultInterpolatedStringHandler.AppendFormatted(ItemType.TypeId2TypeName[(int)itemType]);
					defaultInterpolatedStringHandler.AppendLiteral(" detected.");
					logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			Logger logger2 = ItemDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Total unowned items: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(totalCount);
			logger2.Info(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600501E RID: 20510 RVA: 0x002BC8A4 File Offset: 0x002BAAA4
		private void LogUnownedItem(ItemBase itemBase)
		{
			bool flag = itemBase.PrevOwner.OwnerType > ItemOwnerType.None;
			if (flag)
			{
				Logger logger = ItemDomain.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Item ");
				defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(itemBase.GetItemKey());
				defaultInterpolatedStringHandler.AppendLiteral(" is no longer owned buy any container. Prev owner: ");
				defaultInterpolatedStringHandler.AppendFormatted<ItemOwnerKey>(itemBase.PrevOwner);
				logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x0600501F RID: 20511 RVA: 0x002BC918 File Offset: 0x002BAB18
		public void SetOwner(ItemKey itemKey, ItemOwnerType ownerType, int ownerId)
		{
			bool flag = !itemKey.IsValid() || !this.ItemExists(itemKey);
			if (!flag)
			{
				ItemBase baseItem = this.GetBaseItem(itemKey);
				baseItem.SetOwner(ownerType, ownerId);
			}
		}

		// Token: 0x06005020 RID: 20512 RVA: 0x002BC954 File Offset: 0x002BAB54
		public void RemoveOwner(ItemKey itemKey, ItemOwnerType ownerType, int ownerId)
		{
			ItemBase baseItem = this.GetBaseItem(itemKey);
			baseItem.RemoveOwner(ownerType, ownerId);
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06005021 RID: 20513 RVA: 0x002BC973 File Offset: 0x002BAB73
		[Obsolete("Instead by FullPoisonEffects. Now only for archive data fix. Do not delete this code.")]
		public Dictionary<int, PoisonEffects> PoisonItems
		{
			get
			{
				return this._poisonItems;
			}
		}

		// Token: 0x06005022 RID: 20514 RVA: 0x002BC97B File Offset: 0x002BAB7B
		[Obsolete("Instead by FullPoisonEffects. Now only for archive data fix. Do not delete this code.")]
		public bool HasOldPoisonEffects(ItemKey itemKey)
		{
			return this._poisonItems.ContainsKey(itemKey.Id);
		}

		// Token: 0x06005023 RID: 20515 RVA: 0x002BC990 File Offset: 0x002BAB90
		[Obsolete("Instead by FullPoisonEffects. Now only for archive data fix. Do not delete this code.")]
		public PoisonEffects GetOldPoisonEffects(ItemKey itemKey)
		{
			return this._poisonItems[itemKey.Id];
		}

		// Token: 0x06005024 RID: 20516 RVA: 0x002BC9B4 File Offset: 0x002BABB4
		public PoisonsAndLevels GetAttachedPoisons(ItemKey itemKey)
		{
			FullPoisonEffects poisonEffects;
			bool flag = DomainManager.Extra.TryGetPoisonEffect(itemKey.Id, out poisonEffects);
			PoisonsAndLevels poisonsAndLevels;
			if (flag)
			{
				poisonsAndLevels = poisonEffects.GetAllPoisonsAndLevels();
			}
			else
			{
				poisonsAndLevels = default(PoisonsAndLevels);
				poisonsAndLevels.Initialize();
			}
			return poisonsAndLevels;
		}

		// Token: 0x06005025 RID: 20517 RVA: 0x002BC9FC File Offset: 0x002BABFC
		public void SetPoisonsIdentified(DataContext context, ItemKey itemKey, bool isIdentified)
		{
			FullPoisonEffects poisonEffects;
			bool flag = DomainManager.Extra.TryGetPoisonEffect(itemKey.Id, out poisonEffects);
			if (flag)
			{
				poisonEffects.IsIdentified = isIdentified;
				DomainManager.Extra.SetPoisonEffect(context, itemKey.Id, poisonEffects);
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06005026 RID: 20518 RVA: 0x002BCA3C File Offset: 0x002BAC3C
		public IReadOnlyDictionary<int, FullPoisonEffects> PoisonEffects
		{
			get
			{
				return DomainManager.Extra.PoisonEffects;
			}
		}

		// Token: 0x06005027 RID: 20519 RVA: 0x002BCA48 File Offset: 0x002BAC48
		public FullPoisonEffects GetPoisonEffects(ItemKey itemKey)
		{
			FullPoisonEffects poisonEffect;
			this.PoisonEffects.TryGetValue(itemKey.Id, out poisonEffect);
			if (poisonEffect == null)
			{
				poisonEffect = new FullPoisonEffects();
			}
			return poisonEffect;
		}

		// Token: 0x06005028 RID: 20520 RVA: 0x002BCA78 File Offset: 0x002BAC78
		[return: TupleElementNames(new string[]
		{
			"item",
			"keyChanged"
		})]
		public ValueTuple<ItemBase, bool> SetAttachedPoisons(DataContext context, ItemBase item, short medicineTemplateId, bool add, IReadOnlyList<short> condensedMedicineTemplateIdList = null)
		{
			ItemBase resultItem = item;
			bool keyChanged = false;
			int itemId = item.GetId();
			ItemKey itemKey = item.GetItemKey();
			byte state = item.GetModificationState();
			bool isActive = ModificationStateHelper.IsActive(state, 1);
			bool flag = isActive;
			if (flag)
			{
				FullPoisonEffects poisonEffects = this.PoisonEffects[itemId];
				bool flag2 = !add;
				if (flag2)
				{
					poisonEffects.RemovePoison(medicineTemplateId);
				}
				else
				{
					poisonEffects.AddPoison(medicineTemplateId, condensedMedicineTemplateIdList);
				}
				bool isValid = poisonEffects.IsValid;
				if (isValid)
				{
					DomainManager.Extra.SetPoisonEffect(context, itemId, poisonEffects);
				}
				else
				{
					DomainManager.Extra.RemovePoisonEffect(context, itemId);
					byte currState = ModificationStateHelper.Deactivate(state, 1);
					item.SetModificationState(currState, context);
					bool flag3 = ItemTemplateHelper.IsStackable(itemKey.ItemType, itemKey.TemplateId);
					if (flag3)
					{
						ItemKey newItemKey = DomainManager.Item.CreateItem(context, itemKey.ItemType, itemKey.TemplateId);
						resultItem = DomainManager.Item.GetBaseItem(newItemKey);
					}
					keyChanged = true;
				}
			}
			else
			{
				FullPoisonEffects poisonEffects2 = new FullPoisonEffects();
				poisonEffects2.AddPoison(medicineTemplateId, condensedMedicineTemplateIdList);
				bool flag4 = !ItemDomain.IsPureStackable(item);
				int id;
				if (flag4)
				{
					byte currState2 = ModificationStateHelper.Activate(state, 1);
					item.SetModificationState(currState2, context);
					id = item.GetId();
				}
				else
				{
					ItemBase newItem = this.CreateUniqueStackableItem(context, item.GetItemType(), item.GetTemplateId());
					byte newState = newItem.GetModificationState();
					newState = ModificationStateHelper.Activate(newState, 1);
					newItem.SetModificationState(newState, context);
					resultItem = newItem;
					id = newItem.GetId();
				}
				keyChanged = true;
				DomainManager.Extra.SetPoisonEffect(context, id, poisonEffects2);
			}
			bool flag5 = keyChanged;
			if (flag5)
			{
				InteractOfLove.CheckItemIsLoveTokenAndReplace(context, item.GetItemKey(), resultItem.GetItemKey());
			}
			return new ValueTuple<ItemBase, bool>(resultItem, keyChanged);
		}

		// Token: 0x06005029 RID: 20521 RVA: 0x002BCC30 File Offset: 0x002BAE30
		public ItemBase SetAttachedPoisons(DataContext context, ItemBase item, FullPoisonEffects poisonEffects)
		{
			bool flag = poisonEffects == null || item == null;
			ItemBase result2;
			if (flag)
			{
				result2 = item;
			}
			else
			{
				FullPoisonEffects fullPoisonEffects;
				bool flag2 = DomainManager.Extra.TryGetPoisonEffect(item.GetId(), out fullPoisonEffects);
				if (flag2)
				{
					DomainManager.Extra.RemovePoisonEffect(context, item.GetId());
				}
				bool flag3 = !poisonEffects.IsValid;
				if (flag3)
				{
					result2 = item;
				}
				else
				{
					ItemBase result = item;
					foreach (PoisonSlot poisonSlot in poisonEffects.PoisonSlotList)
					{
						result = this.SetAttachedPoisons(context, result, poisonSlot.MedicineTemplateId, true, poisonSlot.CondensedMedicineTemplateIdList).Item1;
					}
					this.SetPoisonsIdentified(context, result.GetItemKey(), poisonEffects.IsIdentified);
					result2 = result;
				}
			}
			return result2;
		}

		// Token: 0x0600502A RID: 20522 RVA: 0x002BCD0C File Offset: 0x002BAF0C
		[DomainMethod]
		public List<ItemDisplayData> IdentifyPoisons(DataContext context, int charId, ItemDisplayData itemDisplayData)
		{
			ItemDomain.<>c__DisplayClass187_0 CS$<>8__locals1 = new ItemDomain.<>c__DisplayClass187_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.context = context;
			CS$<>8__locals1.charId = charId;
			CS$<>8__locals1.itemDisplayData = itemDisplayData;
			List<ItemKey> itemList = CS$<>8__locals1.itemDisplayData.GetAllItemKeysFromPool();
			CS$<>8__locals1.character = DomainManager.Character.GetElement_Objects(CS$<>8__locals1.charId);
			CS$<>8__locals1.identifiedList = new List<ItemDisplayData>();
			itemList.ForEach(delegate(ItemKey k)
			{
				FullPoisonEffects poisonEffects;
				bool flag8 = CS$<>8__locals1.<>4__this.PoisonEffects.TryGetValue(k.Id, out poisonEffects);
				if (flag8)
				{
					bool isIdentified = poisonEffects.IsIdentified;
					if (!isIdentified)
					{
						List<short> idArray = poisonEffects.GetAllMedicineTemplateIds(false);
						int grade = idArray.Max((short id) => (int)((id > -1) ? ItemTemplateHelper.GetGrade(8, id) : -1));
						bool flag9 = grade > -1;
						if (flag9)
						{
							short attainment = GlobalConfig.Instance.PoisonAttainments[grade];
							short charAttainment = CS$<>8__locals1.character.GetLifeSkillAttainment(9);
							bool flag10 = charAttainment >= attainment;
							if (flag10)
							{
								base.<IdentifyPoisons>g__IdentifySuccess|1(k);
							}
						}
						else
						{
							base.<IdentifyPoisons>g__IdentifySuccess|1(k);
						}
					}
				}
				else
				{
					base.<IdentifyPoisons>g__IdentifySuccess|1(k);
				}
			});
			ItemDisplayData.ReturnItemKeyListToPool(itemList);
			DomainManager.World.AdvanceDaysInMonth(CS$<>8__locals1.context, 1);
			bool isOnCityTown = CS$<>8__locals1.character.IsOnCityTown();
			CS$<>8__locals1.inventory = CS$<>8__locals1.character.GetInventory();
			bool isTaiwu = CS$<>8__locals1.character.GetId() == DomainManager.Taiwu.GetTaiwuCharId();
			List<ItemSourceType> itemSourceTypeList = new List<ItemSourceType>();
			ItemSourceType itemSourceType = (ItemSourceType)CS$<>8__locals1.itemDisplayData.ItemSourceType;
			ItemSourceType itemSourceType2 = itemSourceType;
			switch (itemSourceType2)
			{
			case ItemSourceType.Equipment:
			case ItemSourceType.Inventory:
				break;
			case ItemSourceType.Warehouse:
			{
				bool flag = !isTaiwu;
				if (flag)
				{
					goto IL_17F;
				}
				itemSourceTypeList.Add(ItemSourceType.Warehouse);
				bool flag2 = isOnCityTown;
				if (flag2)
				{
					itemSourceTypeList.Add(ItemSourceType.Inventory);
				}
				itemSourceTypeList.Add(ItemSourceType.Treasury);
				goto IL_17F;
			}
			case ItemSourceType.Treasury:
			{
				bool flag3 = !isTaiwu;
				if (flag3)
				{
					goto IL_17F;
				}
				itemSourceTypeList.Add(ItemSourceType.Treasury);
				bool flag4 = isOnCityTown;
				if (flag4)
				{
					itemSourceTypeList.Add(ItemSourceType.Inventory);
				}
				itemSourceTypeList.Add(ItemSourceType.Warehouse);
				goto IL_17F;
			}
			default:
				if (itemSourceType2 != ItemSourceType.EquipmentPlan)
				{
					throw new ArgumentOutOfRangeException();
				}
				break;
			}
			itemSourceTypeList.Add(ItemSourceType.Inventory);
			bool flag5 = isOnCityTown && isTaiwu;
			if (flag5)
			{
				itemSourceTypeList.Add(ItemSourceType.Warehouse);
				itemSourceTypeList.Add(ItemSourceType.Treasury);
			}
			IL_17F:
			bool flag6 = !CS$<>8__locals1.<IdentifyPoisons>g__CheckItemInSourceList|2(itemSourceTypeList);
			if (flag6)
			{
				throw new Exception("IdentifyPoisons dot not have enough TestingNeedle");
			}
			bool flag7 = CS$<>8__locals1.itemDisplayData.HasAnyPoison && CS$<>8__locals1.identifiedList.Count == 1;
			if (flag7)
			{
				CS$<>8__locals1.identifiedList.RemoveAll((ItemDisplayData d) => !d.HasAnyPoison);
			}
			return CS$<>8__locals1.identifiedList;
		}

		// Token: 0x0600502B RID: 20523 RVA: 0x002BCF14 File Offset: 0x002BB114
		internal ItemKey RemoveOldPoisonEffect(DataContext dataContext, ItemKey itemKey)
		{
			ItemBase baseItem = this.GetBaseItem(itemKey);
			bool flag = ItemTemplateHelper.IsStackable(itemKey.ItemType, itemKey.TemplateId);
			ItemKey newItemKey;
			if (flag)
			{
				this.RemoveItem(dataContext, itemKey);
				newItemKey = this.CreateItem(dataContext, itemKey.ItemType, itemKey.TemplateId);
			}
			else
			{
				bool flag2 = DomainManager.Item.HasOldPoisonEffects(itemKey);
				if (flag2)
				{
					this.RemoveElement_PoisonItems(itemKey.Id, dataContext);
				}
				byte state = baseItem.GetModificationState();
				state = ModificationStateHelper.Deactivate(state, 1);
				baseItem.SetModificationState(state, dataContext);
				newItemKey = baseItem.GetItemKey();
			}
			Logger logger = ItemDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Fixing wrong poison effects of item ");
			defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(itemKey);
			logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
			return newItemKey;
		}

		// Token: 0x0600502C RID: 20524 RVA: 0x002BCFDC File Offset: 0x002BB1DC
		internal ItemKey RemovePoisonEffect(DataContext dataContext, ItemKey itemKey)
		{
			ItemBase baseItem = this.GetBaseItem(itemKey);
			bool flag = ItemTemplateHelper.IsStackable(itemKey.ItemType, itemKey.TemplateId);
			ItemKey newItemKey;
			if (flag)
			{
				this.RemoveItem(dataContext, itemKey);
				newItemKey = this.CreateItem(dataContext, itemKey.ItemType, itemKey.TemplateId);
			}
			else
			{
				FullPoisonEffects fullPoisonEffects;
				bool flag2 = DomainManager.Extra.TryGetPoisonEffect(itemKey.Id, out fullPoisonEffects);
				if (flag2)
				{
					DomainManager.Extra.RemovePoisonEffect(dataContext, itemKey.Id);
				}
				byte state = baseItem.GetModificationState();
				state = ModificationStateHelper.Deactivate(state, 1);
				baseItem.SetModificationState(state, dataContext);
				newItemKey = baseItem.GetItemKey();
			}
			Logger logger = ItemDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Fixing wrong poison effects of item ");
			defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(itemKey);
			logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
			return newItemKey;
		}

		// Token: 0x0600502D RID: 20525 RVA: 0x002BD0AC File Offset: 0x002BB2AC
		private void TransferOldPoisonData(DataContext context)
		{
			foreach (KeyValuePair<int, PoisonEffects> keyValuePair in this._poisonItems)
			{
				int num;
				PoisonEffects poisonEffects2;
				keyValuePair.Deconstruct(out num, out poisonEffects2);
				int id = num;
				PoisonEffects poisonEffects = poisonEffects2;
				FullPoisonEffects fullPoisonEffects = new FullPoisonEffects
				{
					IsIdentified = poisonEffects.IsIdentified,
					PoisonSlotList = new List<PoisonSlot>()
				};
				short[] medicineTemplateIds = poisonEffects.GetAllMedicineTemplateIds();
				foreach (short medicineTemplateId in medicineTemplateIds)
				{
					PoisonSlot poisonSlot = new PoisonSlot
					{
						MedicineTemplateId = medicineTemplateId
					};
					fullPoisonEffects.PoisonSlotList.Add(poisonSlot);
				}
				DomainManager.Extra.AddPoisonEffect(context, id, fullPoisonEffects);
			}
			this.ClearPoisonItems(context);
		}

		// Token: 0x0600502E RID: 20526 RVA: 0x002BD194 File Offset: 0x002BB394
		public RefiningEffects GetRefinedEffects(ItemKey itemKey)
		{
			return this._refinedItems[itemKey.Id];
		}

		// Token: 0x0600502F RID: 20527 RVA: 0x002BD1B8 File Offset: 0x002BB3B8
		[return: TupleElementNames(new string[]
		{
			"item",
			"keyChanged"
		})]
		public ValueTuple<ItemBase, bool> SetRefinedEffects(DataContext context, ItemBase item, int index, short materialTemplateId)
		{
			ItemBase resultItem = item;
			bool keyChanged = false;
			int itemId = item.GetId();
			byte state = item.GetModificationState();
			bool isActive = ModificationStateHelper.IsActive(state, 2);
			bool flag = isActive;
			if (flag)
			{
				RefiningEffects refineEffect = this._refinedItems[itemId];
				refineEffect.Set(index, materialTemplateId);
				bool flag2 = refineEffect.GetTotalRefiningCount() > 0;
				if (flag2)
				{
					this.SetElement_RefinedItems(itemId, refineEffect, context);
					item.SetModificationState(state, context);
				}
				else
				{
					this.RemoveElement_RefinedItems(itemId, context);
					byte currState = ModificationStateHelper.Deactivate(state, 2);
					item.SetModificationState(currState, context);
					keyChanged = true;
				}
			}
			else
			{
				RefiningEffects refiningEffects;
				refiningEffects.Initialize();
				refiningEffects.Set(index, materialTemplateId);
				bool flag3 = !ItemDomain.IsPureStackable(item);
				if (flag3)
				{
					byte currState2 = ModificationStateHelper.Activate(state, 2);
					item.SetModificationState(currState2, context);
					this.AddElement_RefinedItems(item.GetId(), refiningEffects, context);
					keyChanged = true;
				}
				else
				{
					ItemBase newItem = this.CreateUniqueStackableItem(context, item.GetItemType(), item.GetTemplateId());
					byte newState = newItem.GetModificationState();
					newState = ModificationStateHelper.Activate(newState, 2);
					newItem.SetModificationState(newState, context);
					this.AddElement_RefinedItems(newItem.GetId(), refiningEffects, context);
					keyChanged = true;
					resultItem = newItem;
				}
			}
			bool flag4 = keyChanged;
			if (flag4)
			{
				InteractOfLove.CheckItemIsLoveTokenAndReplace(context, item.GetItemKey(), resultItem.GetItemKey());
			}
			return new ValueTuple<ItemBase, bool>(resultItem, keyChanged);
		}

		// Token: 0x06005030 RID: 20528 RVA: 0x002BD314 File Offset: 0x002BB514
		public ItemBase SetRefinedEffects(DataContext context, ItemBase item, RefiningEffects refiningEffects)
		{
			bool flag = refiningEffects.GetTotalRefiningCount() == 0;
			ItemBase result2;
			if (flag)
			{
				result2 = item;
			}
			else
			{
				ItemBase result = item;
				short[] templateIds = refiningEffects.GetAllMaterialTemplateIds();
				for (int index = 0; index < templateIds.Length; index++)
				{
					short id = templateIds[index];
					result = this.SetRefinedEffects(context, result, index, id).Item1;
				}
				result2 = result;
			}
			return result2;
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06005031 RID: 20529 RVA: 0x002BD375 File Offset: 0x002BB575
		public bool VersionNeedRepairSectAccessory
		{
			get
			{
				return DomainManager.World.GetCurrWorldGameVersion() == null || DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 78, 31);
			}
		}

		// Token: 0x06005032 RID: 20530 RVA: 0x002BD398 File Offset: 0x002BB598
		public bool RemoveRefinedEffectsAndReturnMaterial(DataContext context, ItemKey baseKey, Inventory inventory, out ItemBase resultItem)
		{
			resultItem = null;
			bool flag = !ModificationStateHelper.IsActive(baseKey.ModificationState, 2);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = baseKey.ItemType != 2;
				if (flag2)
				{
					result = false;
				}
				else
				{
					AccessoryItem config = Accessory.Instance[baseKey.TemplateId];
					bool flag3 = config.GroupId != 250;
					if (flag3)
					{
						result = false;
					}
					else
					{
						ItemBase item = this.GetBaseItem(baseKey);
						RefiningEffects refinedEffects = DomainManager.Item.GetRefinedEffects(baseKey);
						for (int i = 0; i < 5; i++)
						{
							short material = refinedEffects.GetMaterialTemplateIdAt(i);
							bool flag4 = material < 0;
							if (!flag4)
							{
								ItemKey materialItem = this.CreateMaterial(context, material);
								bool flag5 = inventory == null;
								if (flag5)
								{
									DomainManager.Taiwu.AddItem(context, materialItem, 1, ItemSourceType.Warehouse, false);
								}
								else
								{
									inventory.OfflineAdd(materialItem, 1);
								}
								ValueTuple<ItemBase, bool> valueTuple = DomainManager.Item.SetRefinedEffects(context, item, i, -1);
								ItemBase newItem = valueTuple.Item1;
								bool keyChanged = valueTuple.Item2;
								bool flag6 = keyChanged;
								if (flag6)
								{
									resultItem = newItem;
								}
							}
						}
						result = (resultItem != null);
					}
				}
			}
			return result;
		}

		// Token: 0x06005033 RID: 20531 RVA: 0x002BD4C4 File Offset: 0x002BB6C4
		[DomainMethod]
		public List<SkillBookModifyDisplayData> GetTaiwuInventoryCombatSkillBooks()
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			Inventory inventory = taiwu.GetInventory();
			List<SkillBookModifyDisplayData> skillBookDisplayData = new List<SkillBookModifyDisplayData>();
			foreach (ItemKey key in inventory.Items.Keys)
			{
				bool flag = !key.IsValid() || key.ItemType != 10;
				if (!flag)
				{
					SkillBookItem config = SkillBook.Instance[key.TemplateId];
					SkillBook skillBook;
					bool flag2 = config.ItemSubType != 1001 || !this._skillBooks.TryGetValue(key.Id, out skillBook);
					if (!flag2)
					{
						short gainExp = SkillGradeData.Instance[skillBook.GetGrade()].ReadingExpGainPerPage;
						int normalPageCost = (int)(gainExp * 20);
						int outlinePageCost = (int)(gainExp * 60);
						SkillBookModifyDisplayData data = new SkillBookModifyDisplayData
						{
							ItemDisplayData = this.GetItemDisplayData(key, -1),
							PageTypes = skillBook.GetPageTypes(),
							PageIncompleteState = skillBook.GetPageIncompleteState(),
							NormalPageCostExp = normalPageCost,
							OutlinePageCostExp = outlinePageCost
						};
						skillBookDisplayData.Add(data);
					}
				}
			}
			return skillBookDisplayData;
		}

		// Token: 0x06005034 RID: 20532 RVA: 0x002BD618 File Offset: 0x002BB818
		[DomainMethod]
		[Obsolete("Use SetCombatSkillBookPage instead")]
		public bool ModifyCombatSkillBookPageOutline(DataContext context, ItemKey itemKey, sbyte behaviorType)
		{
			bool flag = DomainManager.World.GetLeftDaysInCurrMonth() < 10;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				SkillBook skillBook;
				bool flag2 = !this._skillBooks.TryGetValue(itemKey.Id, out skillBook);
				if (flag2)
				{
					result = false;
				}
				else
				{
					GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
					int expCost = (int)(SkillGradeData.Instance[skillBook.GetGrade()].ReadingExpGainPerPage * 60);
					bool flag3 = taiwu.GetExp() < expCost;
					if (flag3)
					{
						result = false;
					}
					else
					{
						DomainManager.World.AdvanceDaysInMonth(context, 10);
						taiwu.ChangeExp(context, -expCost);
						skillBook.SetOutlinePageType(context, behaviorType);
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06005035 RID: 20533 RVA: 0x002BD6C0 File Offset: 0x002BB8C0
		[DomainMethod]
		[Obsolete("Use SetCombatSkillBookPage instead")]
		public bool ModifyCombatSkillBookPageNormal(DataContext context, ItemKey itemKey, List<byte> pageIds, List<sbyte> directions)
		{
			bool flag = DomainManager.World.GetLeftDaysInCurrMonth() < 10;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = pageIds == null || pageIds.Count <= 0 || directions == null || directions.Count <= 0 || pageIds.Count != directions.Count;
				if (flag2)
				{
					result = false;
				}
				else
				{
					SkillBook skillBook;
					bool flag3 = !this._skillBooks.TryGetValue(itemKey.Id, out skillBook);
					if (flag3)
					{
						result = false;
					}
					else
					{
						GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
						int expCost = (int)(SkillGradeData.Instance[skillBook.GetGrade()].ReadingExpGainPerPage * 20) * pageIds.Count;
						bool flag4 = taiwu.GetExp() < expCost;
						if (flag4)
						{
							result = false;
						}
						else
						{
							DomainManager.World.AdvanceDaysInMonth(context, 10);
							taiwu.ChangeExp(context, -expCost);
							for (int i = 0; i < pageIds.Count; i++)
							{
								skillBook.SetNormalPageType(context, pageIds[i], directions[i]);
							}
							result = true;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06005036 RID: 20534 RVA: 0x002BD7DC File Offset: 0x002BB9DC
		[DomainMethod]
		public unsafe bool SetCombatSkillBookPage(DataContext context, ItemKey itemKey, sbyte behaviorType, List<sbyte> directions)
		{
			bool flag = DomainManager.World.GetLeftDaysInCurrMonth() < 10;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				SkillBook skillBook;
				bool flag2 = !this._skillBooks.TryGetValue(itemKey.Id, out skillBook);
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = directions == null || directions.Count != 5;
					if (flag3)
					{
						result = false;
					}
					else
					{
						byte pageTypes = skillBook.GetPageTypes();
						sbyte oldOutlineType = SkillBookStateHelper.GetOutlinePageType(pageTypes);
						bool needChangeOutline = oldOutlineType != behaviorType;
						int outlineCost = (int)(needChangeOutline ? (SkillGradeData.Instance[skillBook.GetGrade()].ReadingExpGainPerPage * 60) : 0);
						int normalCost = 0;
						Span<bool> span = new Span<bool>(stackalloc byte[(UIntPtr)5], 5);
						Span<bool> needModifyList = span;
						for (int i = 0; i < 5; i++)
						{
							sbyte oldNormalType = SkillBookStateHelper.GetNormalPageType(pageTypes, (byte)(i + 1));
							*needModifyList[i] = (oldNormalType != directions[i]);
							normalCost += (int)((*needModifyList[i]) ? (SkillGradeData.Instance[skillBook.GetGrade()].ReadingExpGainPerPage * 20) : 0);
						}
						int totalCost = outlineCost + normalCost;
						GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
						bool flag4 = taiwu.GetExp() < totalCost;
						if (flag4)
						{
							result = false;
						}
						else
						{
							DomainManager.World.AdvanceDaysInMonth(context, 10);
							taiwu.ChangeExp(context, -totalCost);
							bool flag5 = needChangeOutline;
							if (flag5)
							{
								skillBook.SetOutlinePageType(context, behaviorType);
							}
							for (int j = 0; j < 5; j++)
							{
								bool flag6 = *needModifyList[j];
								if (flag6)
								{
									skillBook.SetNormalPageType(context, (byte)(j + 1), directions[j]);
								}
							}
							result = true;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06005037 RID: 20535 RVA: 0x002BD998 File Offset: 0x002BBB98
		public int GetPageIncompleteState(ushort pageIncompleteState, byte pageId, ItemKey[] referenceBooks, ItemKey curReadingBook)
		{
			sbyte incompleteState = SkillBookStateHelper.GetPageIncompleteState(pageIncompleteState, pageId);
			bool flag = incompleteState == 0 || !curReadingBook.IsValid();
			int result;
			if (flag)
			{
				result = (int)incompleteState;
			}
			else
			{
				SkillBook book = this.GetElement_SkillBooks(curReadingBook.Id);
				foreach (ItemKey refBookKey in referenceBooks)
				{
					bool flag2 = !refBookKey.IsValid() || refBookKey.TemplateId != book.GetTemplateId();
					if (!flag2)
					{
						SkillBook refBook = this.GetElement_SkillBooks(refBookKey.Id);
						bool needSupply = true;
						bool flag3 = book.GetCombatSkillTemplateId() > -1;
						if (flag3)
						{
							byte pageTypes = book.GetPageTypes();
							byte refPageTypes = refBook.GetPageTypes();
							sbyte pageType = SkillBookStateHelper.GetNormalPageType(pageTypes, pageId);
							sbyte refPageType = SkillBookStateHelper.GetNormalPageType(refPageTypes, pageId);
							bool flag4 = pageType != refPageType;
							if (flag4)
							{
								needSupply = false;
							}
						}
						bool flag5 = needSupply;
						if (flag5)
						{
							sbyte refBookPageState = SkillBookStateHelper.GetPageIncompleteState(refBook.GetPageIncompleteState(), pageId);
							bool flag6 = refBookPageState >= 0 && refBookPageState < incompleteState;
							if (flag6)
							{
								incompleteState = refBookPageState;
							}
						}
					}
				}
				result = (int)incompleteState;
			}
			return result;
		}

		// Token: 0x06005038 RID: 20536 RVA: 0x002BDAB8 File Offset: 0x002BBCB8
		public bool HasNewDeadCricket()
		{
			return this._newDeadCrickets.Count > 0;
		}

		// Token: 0x06005039 RID: 20537 RVA: 0x002BDAD8 File Offset: 0x002BBCD8
		public bool IsNewDeadCricket(ItemKey itemKey)
		{
			return this._newDeadCrickets.Contains(itemKey);
		}

		// Token: 0x0600503A RID: 20538 RVA: 0x002BDAF8 File Offset: 0x002BBCF8
		public void UpdateCrickets(DataContext context)
		{
			this._newDeadCrickets.Clear();
			bool flag = DomainManager.World.GetCurrMonthInYear() != (sbyte)(GlobalConfig.Instance.CricketActiveStartMonth + 1);
			if (!flag)
			{
				MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
				foreach (Cricket cricket in this._crickets.Values)
				{
					bool flag2 = cricket.UpdateCricketAge(context);
					if (flag2)
					{
						ItemKey cricketKey = cricket.GetItemKey();
						List<CricketCollectionData> cricketCollectionData = DomainManager.Extra.GetCricketCollectionDataList();
						if (DomainManager.Taiwu.GetWarehouseItemCount(cricketKey) > 0)
						{
							goto IL_DC;
						}
						if ((from item in cricketCollectionData
						select item.Cricket).Contains(cricketKey))
						{
							goto IL_DC;
						}
						bool flag3 = DomainManager.Taiwu.GetTaiwu().GetInventory().Items.ContainsKey(cricketKey);
						IL_DD:
						bool flag4 = flag3;
						if (flag4)
						{
							short colorId = cricket.GetColorId();
							short partId = cricket.GetPartId();
							monthlyNotificationCollection.AddCricketEndLife(colorId, partId);
						}
						else
						{
							this._newDeadCrickets.Add(cricketKey);
						}
						continue;
						IL_DC:
						flag3 = true;
						goto IL_DD;
					}
				}
			}
		}

		// Token: 0x0600503B RID: 20539 RVA: 0x002BDC48 File Offset: 0x002BBE48
		[DomainMethod]
		public bool[] GetCricketsAliveState(List<ItemKey> keyList)
		{
			bool[] stateArray = new bool[keyList.Count];
			bool flag = keyList != null;
			if (flag)
			{
				for (int index = 0; index < keyList.Count; index++)
				{
					ItemKey itemKey = keyList[index];
					bool alive = false;
					Cricket cricket;
					bool flag2 = DomainManager.Item.TryGetElement_Crickets(itemKey.Id, out cricket);
					if (flag2)
					{
						alive = cricket.IsAlive;
					}
					stateArray[index] = alive;
				}
			}
			return stateArray;
		}

		// Token: 0x0600503C RID: 20540 RVA: 0x002BDCBC File Offset: 0x002BBEBC
		public static short GetWugTemplateId(sbyte wugType, sbyte wugGrowthType)
		{
			int index = (int)(wugType * 6 + wugGrowthType);
			return ItemDomain._wugTemplateIds[index];
		}

		// Token: 0x0600503D RID: 20541 RVA: 0x002BDCDB File Offset: 0x002BBEDB
		public static IEnumerable<short> GetWugTemplateIdGroup(sbyte wugType, bool isKing)
		{
			int group = (int)(wugType * 6);
			int begin = isKing ? 5 : group;
			int end = isKing ? 53 : (group + 5);
			int step = isKing ? 6 : 1;
			for (int i = begin; i < end; i += step)
			{
				yield return ItemDomain._wugTemplateIds[i];
			}
			yield break;
		}

		// Token: 0x0600503E RID: 20542 RVA: 0x002BDCF4 File Offset: 0x002BBEF4
		private static void InitializeWugTemplateIds()
		{
			ItemDomain._wugTemplateIds = new short[48];
			foreach (MedicineItem item in ((IEnumerable<MedicineItem>)Medicine.Instance))
			{
				bool flag = item.WugType < 0;
				if (!flag)
				{
					int index = (int)(item.WugType * 6 + item.WugGrowthType);
					ItemDomain._wugTemplateIds[index] = item.TemplateId;
				}
			}
		}

		// Token: 0x0600503F RID: 20543 RVA: 0x002BDD78 File Offset: 0x002BBF78
		public ItemDomain() : base(21)
		{
			this._weapons = new Dictionary<int, Weapon>(0);
			this._armors = new Dictionary<int, Armor>(0);
			this._accessories = new Dictionary<int, Accessory>(0);
			this._clothing = new Dictionary<int, Clothing>(0);
			this._carriers = new Dictionary<int, Carrier>(0);
			this._materials = new Dictionary<int, Material>(0);
			this._craftTools = new Dictionary<int, CraftTool>(0);
			this._foods = new Dictionary<int, Food>(0);
			this._medicines = new Dictionary<int, Medicine>(0);
			this._teaWines = new Dictionary<int, TeaWine>(0);
			this._skillBooks = new Dictionary<int, SkillBook>(0);
			this._crickets = new Dictionary<int, Cricket>(0);
			this._misc = new Dictionary<int, Misc>(0);
			this._nextItemId = 0;
			this._stackableItems = new Dictionary<TemplateKey, int>(0);
			this._poisonItems = new Dictionary<int, PoisonEffects>(0);
			this._refinedItems = new Dictionary<int, RefiningEffects>(0);
			this._emptyHandKey = default(ItemKey);
			this._branchKey = default(ItemKey);
			this._stoneKey = default(ItemKey);
			this._externEquipmentEffects = new Dictionary<int, GameData.Utilities.ShortList>(0);
			this.HelperDataWeapons = new ObjectCollectionHelperData(6, 0, ItemDomain.CacheInfluencesWeapons, this._dataStatesWeapons, true);
			this.HelperDataArmors = new ObjectCollectionHelperData(6, 1, ItemDomain.CacheInfluencesArmors, this._dataStatesArmors, true);
			this.HelperDataAccessories = new ObjectCollectionHelperData(6, 2, ItemDomain.CacheInfluencesAccessories, this._dataStatesAccessories, true);
			this.HelperDataClothing = new ObjectCollectionHelperData(6, 3, ItemDomain.CacheInfluencesClothing, this._dataStatesClothing, true);
			this.HelperDataCarriers = new ObjectCollectionHelperData(6, 4, ItemDomain.CacheInfluencesCarriers, this._dataStatesCarriers, true);
			this.HelperDataMaterials = new ObjectCollectionHelperData(6, 5, ItemDomain.CacheInfluencesMaterials, this._dataStatesMaterials, true);
			this.HelperDataCraftTools = new ObjectCollectionHelperData(6, 6, ItemDomain.CacheInfluencesCraftTools, this._dataStatesCraftTools, true);
			this.HelperDataFoods = new ObjectCollectionHelperData(6, 7, ItemDomain.CacheInfluencesFoods, this._dataStatesFoods, true);
			this.HelperDataMedicines = new ObjectCollectionHelperData(6, 8, ItemDomain.CacheInfluencesMedicines, this._dataStatesMedicines, true);
			this.HelperDataTeaWines = new ObjectCollectionHelperData(6, 9, ItemDomain.CacheInfluencesTeaWines, this._dataStatesTeaWines, true);
			this.HelperDataSkillBooks = new ObjectCollectionHelperData(6, 10, ItemDomain.CacheInfluencesSkillBooks, this._dataStatesSkillBooks, true);
			this.HelperDataCrickets = new ObjectCollectionHelperData(6, 11, ItemDomain.CacheInfluencesCrickets, this._dataStatesCrickets, true);
			this.HelperDataMisc = new ObjectCollectionHelperData(6, 12, ItemDomain.CacheInfluencesMisc, this._dataStatesMisc, true);
			this.OnInitializedDomainData();
		}

		// Token: 0x06005040 RID: 20544 RVA: 0x002BE0B4 File Offset: 0x002BC2B4
		public Weapon GetElement_Weapons(int objectId)
		{
			return this._weapons[objectId];
		}

		// Token: 0x06005041 RID: 20545 RVA: 0x002BE0D4 File Offset: 0x002BC2D4
		public bool TryGetElement_Weapons(int objectId, out Weapon element)
		{
			return this._weapons.TryGetValue(objectId, out element);
		}

		// Token: 0x06005042 RID: 20546 RVA: 0x002BE0F4 File Offset: 0x002BC2F4
		private unsafe void AddElement_Weapons(int objectId, Weapon instance)
		{
			instance.CollectionHelperData = this.HelperDataWeapons;
			instance.DataStatesOffset = this._dataStatesWeapons.Create();
			this._weapons.Add(objectId, instance);
			byte* pData = OperationAdder.DynamicObjectCollection_Add<int>(6, 0, objectId, instance.GetSerializedSize());
			instance.Serialize(pData);
		}

		// Token: 0x06005043 RID: 20547 RVA: 0x002BE144 File Offset: 0x002BC344
		private void RemoveElement_Weapons(int objectId)
		{
			Weapon instance;
			bool flag = !this._weapons.TryGetValue(objectId, out instance);
			if (!flag)
			{
				this._dataStatesWeapons.Remove(instance.DataStatesOffset);
				this._weapons.Remove(objectId);
				OperationAdder.DynamicObjectCollection_Remove<int>(6, 0, objectId);
			}
		}

		// Token: 0x06005044 RID: 20548 RVA: 0x002BE191 File Offset: 0x002BC391
		private void ClearWeapons()
		{
			this._dataStatesWeapons.Clear();
			this._weapons.Clear();
			OperationAdder.DynamicObjectCollection_Clear(6, 0);
		}

		// Token: 0x06005045 RID: 20549 RVA: 0x002BE1B4 File Offset: 0x002BC3B4
		public int GetElementField_Weapons(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
		{
			Weapon instance;
			bool flag = !this._weapons.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				string tag = "GetElementField_Weapons";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
				defaultInterpolatedStringHandler.AppendLiteral(" with field ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
				result = -1;
			}
			else
			{
				if (resetModified)
				{
					this._dataStatesWeapons.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
				switch (fieldId)
				{
				case 0:
					result = Serializer.Serialize(instance.GetId(), dataPool);
					break;
				case 1:
					result = Serializer.Serialize(instance.GetTemplateId(), dataPool);
					break;
				case 2:
					result = Serializer.Serialize(instance.GetMaxDurability(), dataPool);
					break;
				case 3:
					result = Serializer.Serialize(instance.GetEquipmentEffectId(), dataPool);
					break;
				case 4:
					result = Serializer.Serialize(instance.GetTricks(), dataPool);
					break;
				case 5:
					result = Serializer.Serialize(instance.GetCurrDurability(), dataPool);
					break;
				case 6:
					result = Serializer.Serialize(instance.GetModificationState(), dataPool);
					break;
				case 7:
					result = Serializer.Serialize(instance.GetEquippedCharId(), dataPool);
					break;
				case 8:
					result = Serializer.Serialize(instance.GetMaterialResources(), dataPool);
					break;
				case 9:
					result = Serializer.Serialize(instance.GetPenetrationFactor(), dataPool);
					break;
				case 10:
					result = Serializer.Serialize(instance.GetEquipmentAttack(), dataPool);
					break;
				case 11:
					result = Serializer.Serialize(instance.GetEquipmentDefense(), dataPool);
					break;
				case 12:
					result = Serializer.Serialize(instance.GetWeight(), dataPool);
					break;
				default:
				{
					bool flag2 = fieldId >= 83;
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

		// Token: 0x06005046 RID: 20550 RVA: 0x002BE3D8 File Offset: 0x002BC5D8
		public void SetElementField_Weapons(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			Weapon instance;
			bool flag = !this._weapons.TryGetValue(objectId, out instance);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
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
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
			{
				short value = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value);
				instance.SetMaxDurability(value, context);
				break;
			}
			case 3:
			{
				short value2 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value2);
				instance.SetEquipmentEffectId(value2, context);
				break;
			}
			case 4:
			{
				List<sbyte> value3 = instance.GetTricks();
				Serializer.Deserialize(dataPool, valueOffset, ref value3);
				instance.SetTricks(value3, context);
				break;
			}
			case 5:
			{
				short value4 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value4);
				instance.SetCurrDurability(value4, context);
				break;
			}
			case 6:
			{
				byte value5 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value5);
				instance.SetModificationState(value5, context);
				break;
			}
			case 7:
			{
				int value6 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value6);
				instance.SetEquippedCharId(value6, context);
				break;
			}
			case 8:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			default:
			{
				bool flag2 = fieldId >= 83;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (flag2)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = fieldId >= 13;
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

		// Token: 0x06005047 RID: 20551 RVA: 0x002BE65C File Offset: 0x002BC85C
		private int CheckModified_Weapons(int objectId, ushort fieldId, RawDataPool dataPool)
		{
			Weapon instance;
			bool flag = !this._weapons.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = fieldId >= 13;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesWeapons.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					this._dataStatesWeapons.ResetModified(instance.DataStatesOffset, (int)fieldId);
					switch (fieldId)
					{
					case 0:
						result = Serializer.Serialize(instance.GetId(), dataPool);
						break;
					case 1:
						result = Serializer.Serialize(instance.GetTemplateId(), dataPool);
						break;
					case 2:
						result = Serializer.Serialize(instance.GetMaxDurability(), dataPool);
						break;
					case 3:
						result = Serializer.Serialize(instance.GetEquipmentEffectId(), dataPool);
						break;
					case 4:
						result = Serializer.Serialize(instance.GetTricks(), dataPool);
						break;
					case 5:
						result = Serializer.Serialize(instance.GetCurrDurability(), dataPool);
						break;
					case 6:
						result = Serializer.Serialize(instance.GetModificationState(), dataPool);
						break;
					case 7:
						result = Serializer.Serialize(instance.GetEquippedCharId(), dataPool);
						break;
					case 8:
						result = Serializer.Serialize(instance.GetMaterialResources(), dataPool);
						break;
					case 9:
						result = Serializer.Serialize(instance.GetPenetrationFactor(), dataPool);
						break;
					case 10:
						result = Serializer.Serialize(instance.GetEquipmentAttack(), dataPool);
						break;
					case 11:
						result = Serializer.Serialize(instance.GetEquipmentDefense(), dataPool);
						break;
					case 12:
						result = Serializer.Serialize(instance.GetWeight(), dataPool);
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

		// Token: 0x06005048 RID: 20552 RVA: 0x002BE844 File Offset: 0x002BCA44
		private void ResetModifiedWrapper_Weapons(int objectId, ushort fieldId)
		{
			Weapon instance;
			bool flag = !this._weapons.TryGetValue(objectId, out instance);
			if (!flag)
			{
				bool flag2 = fieldId >= 13;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesWeapons.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (!flag3)
				{
					this._dataStatesWeapons.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
			}
		}

		// Token: 0x06005049 RID: 20553 RVA: 0x002BE8D4 File Offset: 0x002BCAD4
		private bool IsModifiedWrapper_Weapons(int objectId, ushort fieldId)
		{
			Weapon instance;
			bool flag = !this._weapons.TryGetValue(objectId, out instance);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = fieldId >= 13;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = this._dataStatesWeapons.IsModified(instance.DataStatesOffset, (int)fieldId);
			}
			return result;
		}

		// Token: 0x0600504A RID: 20554 RVA: 0x002BE94C File Offset: 0x002BCB4C
		public Armor GetElement_Armors(int objectId)
		{
			return this._armors[objectId];
		}

		// Token: 0x0600504B RID: 20555 RVA: 0x002BE96C File Offset: 0x002BCB6C
		public bool TryGetElement_Armors(int objectId, out Armor element)
		{
			return this._armors.TryGetValue(objectId, out element);
		}

		// Token: 0x0600504C RID: 20556 RVA: 0x002BE98C File Offset: 0x002BCB8C
		private unsafe void AddElement_Armors(int objectId, Armor instance)
		{
			instance.CollectionHelperData = this.HelperDataArmors;
			instance.DataStatesOffset = this._dataStatesArmors.Create();
			this._armors.Add(objectId, instance);
			byte* pData = OperationAdder.FixedObjectCollection_Add<int>(6, 1, objectId, 29);
			instance.Serialize(pData);
		}

		// Token: 0x0600504D RID: 20557 RVA: 0x002BE9D8 File Offset: 0x002BCBD8
		private void RemoveElement_Armors(int objectId)
		{
			Armor instance;
			bool flag = !this._armors.TryGetValue(objectId, out instance);
			if (!flag)
			{
				this._dataStatesArmors.Remove(instance.DataStatesOffset);
				this._armors.Remove(objectId);
				OperationAdder.FixedObjectCollection_Remove<int>(6, 1, objectId);
			}
		}

		// Token: 0x0600504E RID: 20558 RVA: 0x002BEA25 File Offset: 0x002BCC25
		private void ClearArmors()
		{
			this._dataStatesArmors.Clear();
			this._armors.Clear();
			OperationAdder.FixedObjectCollection_Clear(6, 1);
		}

		// Token: 0x0600504F RID: 20559 RVA: 0x002BEA48 File Offset: 0x002BCC48
		public int GetElementField_Armors(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
		{
			Armor instance;
			bool flag = !this._armors.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				string tag = "GetElementField_Armors";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
				defaultInterpolatedStringHandler.AppendLiteral(" with field ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
				result = -1;
			}
			else
			{
				if (resetModified)
				{
					this._dataStatesArmors.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
				switch (fieldId)
				{
				case 0:
					result = Serializer.Serialize(instance.GetId(), dataPool);
					break;
				case 1:
					result = Serializer.Serialize(instance.GetTemplateId(), dataPool);
					break;
				case 2:
					result = Serializer.Serialize(instance.GetMaxDurability(), dataPool);
					break;
				case 3:
					result = Serializer.Serialize(instance.GetEquipmentEffectId(), dataPool);
					break;
				case 4:
					result = Serializer.Serialize(instance.GetCurrDurability(), dataPool);
					break;
				case 5:
					result = Serializer.Serialize(instance.GetModificationState(), dataPool);
					break;
				case 6:
					result = Serializer.Serialize(instance.GetEquippedCharId(), dataPool);
					break;
				case 7:
					result = Serializer.Serialize(instance.GetMaterialResources(), dataPool);
					break;
				case 8:
					result = Serializer.Serialize(instance.GetPenetrationResistFactors(), dataPool);
					break;
				case 9:
					result = Serializer.Serialize(instance.GetEquipmentAttack(), dataPool);
					break;
				case 10:
					result = Serializer.Serialize(instance.GetEquipmentDefense(), dataPool);
					break;
				case 11:
					result = Serializer.Serialize(instance.GetWeight(), dataPool);
					break;
				case 12:
					result = Serializer.Serialize(instance.GetInjuryFactor(), dataPool);
					break;
				default:
				{
					bool flag2 = fieldId >= 53;
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

		// Token: 0x06005050 RID: 20560 RVA: 0x002BEC6C File Offset: 0x002BCE6C
		public void SetElementField_Armors(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			Armor instance;
			bool flag = !this._armors.TryGetValue(objectId, out instance);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
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
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
			{
				short value = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value);
				instance.SetMaxDurability(value, context);
				break;
			}
			case 3:
			{
				short value2 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value2);
				instance.SetEquipmentEffectId(value2, context);
				break;
			}
			case 4:
			{
				short value3 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value3);
				instance.SetCurrDurability(value3, context);
				break;
			}
			case 5:
			{
				byte value4 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value4);
				instance.SetModificationState(value4, context);
				break;
			}
			case 6:
			{
				int value5 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value5);
				instance.SetEquippedCharId(value5, context);
				break;
			}
			case 7:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			default:
			{
				bool flag2 = fieldId >= 53;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (flag2)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = fieldId >= 13;
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

		// Token: 0x06005051 RID: 20561 RVA: 0x002BEEC8 File Offset: 0x002BD0C8
		private int CheckModified_Armors(int objectId, ushort fieldId, RawDataPool dataPool)
		{
			Armor instance;
			bool flag = !this._armors.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = fieldId >= 13;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesArmors.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					this._dataStatesArmors.ResetModified(instance.DataStatesOffset, (int)fieldId);
					switch (fieldId)
					{
					case 0:
						result = Serializer.Serialize(instance.GetId(), dataPool);
						break;
					case 1:
						result = Serializer.Serialize(instance.GetTemplateId(), dataPool);
						break;
					case 2:
						result = Serializer.Serialize(instance.GetMaxDurability(), dataPool);
						break;
					case 3:
						result = Serializer.Serialize(instance.GetEquipmentEffectId(), dataPool);
						break;
					case 4:
						result = Serializer.Serialize(instance.GetCurrDurability(), dataPool);
						break;
					case 5:
						result = Serializer.Serialize(instance.GetModificationState(), dataPool);
						break;
					case 6:
						result = Serializer.Serialize(instance.GetEquippedCharId(), dataPool);
						break;
					case 7:
						result = Serializer.Serialize(instance.GetMaterialResources(), dataPool);
						break;
					case 8:
						result = Serializer.Serialize(instance.GetPenetrationResistFactors(), dataPool);
						break;
					case 9:
						result = Serializer.Serialize(instance.GetEquipmentAttack(), dataPool);
						break;
					case 10:
						result = Serializer.Serialize(instance.GetEquipmentDefense(), dataPool);
						break;
					case 11:
						result = Serializer.Serialize(instance.GetWeight(), dataPool);
						break;
					case 12:
						result = Serializer.Serialize(instance.GetInjuryFactor(), dataPool);
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

		// Token: 0x06005052 RID: 20562 RVA: 0x002BF0B0 File Offset: 0x002BD2B0
		private void ResetModifiedWrapper_Armors(int objectId, ushort fieldId)
		{
			Armor instance;
			bool flag = !this._armors.TryGetValue(objectId, out instance);
			if (!flag)
			{
				bool flag2 = fieldId >= 13;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesArmors.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (!flag3)
				{
					this._dataStatesArmors.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
			}
		}

		// Token: 0x06005053 RID: 20563 RVA: 0x002BF140 File Offset: 0x002BD340
		private bool IsModifiedWrapper_Armors(int objectId, ushort fieldId)
		{
			Armor instance;
			bool flag = !this._armors.TryGetValue(objectId, out instance);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = fieldId >= 13;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = this._dataStatesArmors.IsModified(instance.DataStatesOffset, (int)fieldId);
			}
			return result;
		}

		// Token: 0x06005054 RID: 20564 RVA: 0x002BF1B8 File Offset: 0x002BD3B8
		public Accessory GetElement_Accessories(int objectId)
		{
			return this._accessories[objectId];
		}

		// Token: 0x06005055 RID: 20565 RVA: 0x002BF1D8 File Offset: 0x002BD3D8
		public bool TryGetElement_Accessories(int objectId, out Accessory element)
		{
			return this._accessories.TryGetValue(objectId, out element);
		}

		// Token: 0x06005056 RID: 20566 RVA: 0x002BF1F8 File Offset: 0x002BD3F8
		private unsafe void AddElement_Accessories(int objectId, Accessory instance)
		{
			instance.CollectionHelperData = this.HelperDataAccessories;
			instance.DataStatesOffset = this._dataStatesAccessories.Create();
			this._accessories.Add(objectId, instance);
			byte* pData = OperationAdder.FixedObjectCollection_Add<int>(6, 2, objectId, 29);
			instance.Serialize(pData);
		}

		// Token: 0x06005057 RID: 20567 RVA: 0x002BF244 File Offset: 0x002BD444
		private void RemoveElement_Accessories(int objectId)
		{
			Accessory instance;
			bool flag = !this._accessories.TryGetValue(objectId, out instance);
			if (!flag)
			{
				this._dataStatesAccessories.Remove(instance.DataStatesOffset);
				this._accessories.Remove(objectId);
				OperationAdder.FixedObjectCollection_Remove<int>(6, 2, objectId);
			}
		}

		// Token: 0x06005058 RID: 20568 RVA: 0x002BF291 File Offset: 0x002BD491
		private void ClearAccessories()
		{
			this._dataStatesAccessories.Clear();
			this._accessories.Clear();
			OperationAdder.FixedObjectCollection_Clear(6, 2);
		}

		// Token: 0x06005059 RID: 20569 RVA: 0x002BF2B4 File Offset: 0x002BD4B4
		public int GetElementField_Accessories(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
		{
			Accessory instance;
			bool flag = !this._accessories.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				string tag = "GetElementField_Accessories";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
				defaultInterpolatedStringHandler.AppendLiteral(" with field ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
				result = -1;
			}
			else
			{
				if (resetModified)
				{
					this._dataStatesAccessories.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
				switch (fieldId)
				{
				case 0:
					result = Serializer.Serialize(instance.GetId(), dataPool);
					break;
				case 1:
					result = Serializer.Serialize(instance.GetTemplateId(), dataPool);
					break;
				case 2:
					result = Serializer.Serialize(instance.GetMaxDurability(), dataPool);
					break;
				case 3:
					result = Serializer.Serialize(instance.GetEquipmentEffectId(), dataPool);
					break;
				case 4:
					result = Serializer.Serialize(instance.GetCurrDurability(), dataPool);
					break;
				case 5:
					result = Serializer.Serialize(instance.GetModificationState(), dataPool);
					break;
				case 6:
					result = Serializer.Serialize(instance.GetEquippedCharId(), dataPool);
					break;
				case 7:
					result = Serializer.Serialize(instance.GetMaterialResources(), dataPool);
					break;
				default:
				{
					bool flag2 = fieldId >= 79;
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

		// Token: 0x0600505A RID: 20570 RVA: 0x002BF46C File Offset: 0x002BD66C
		public void SetElementField_Accessories(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			Accessory instance;
			bool flag = !this._accessories.TryGetValue(objectId, out instance);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
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
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
			{
				short value = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value);
				instance.SetMaxDurability(value, context);
				break;
			}
			case 3:
			{
				short value2 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value2);
				instance.SetEquipmentEffectId(value2, context);
				break;
			}
			case 4:
			{
				short value3 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value3);
				instance.SetCurrDurability(value3, context);
				break;
			}
			case 5:
			{
				byte value4 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value4);
				instance.SetModificationState(value4, context);
				break;
			}
			case 6:
			{
				int value5 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value5);
				instance.SetEquippedCharId(value5, context);
				break;
			}
			case 7:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			default:
			{
				bool flag2 = fieldId >= 79;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (flag2)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = fieldId >= 8;
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

		// Token: 0x0600505B RID: 20571 RVA: 0x002BF6C8 File Offset: 0x002BD8C8
		private int CheckModified_Accessories(int objectId, ushort fieldId, RawDataPool dataPool)
		{
			Accessory instance;
			bool flag = !this._accessories.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = fieldId >= 8;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesAccessories.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					this._dataStatesAccessories.ResetModified(instance.DataStatesOffset, (int)fieldId);
					switch (fieldId)
					{
					case 0:
						result = Serializer.Serialize(instance.GetId(), dataPool);
						break;
					case 1:
						result = Serializer.Serialize(instance.GetTemplateId(), dataPool);
						break;
					case 2:
						result = Serializer.Serialize(instance.GetMaxDurability(), dataPool);
						break;
					case 3:
						result = Serializer.Serialize(instance.GetEquipmentEffectId(), dataPool);
						break;
					case 4:
						result = Serializer.Serialize(instance.GetCurrDurability(), dataPool);
						break;
					case 5:
						result = Serializer.Serialize(instance.GetModificationState(), dataPool);
						break;
					case 6:
						result = Serializer.Serialize(instance.GetEquippedCharId(), dataPool);
						break;
					case 7:
						result = Serializer.Serialize(instance.GetMaterialResources(), dataPool);
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

		// Token: 0x0600505C RID: 20572 RVA: 0x002BF83C File Offset: 0x002BDA3C
		private void ResetModifiedWrapper_Accessories(int objectId, ushort fieldId)
		{
			Accessory instance;
			bool flag = !this._accessories.TryGetValue(objectId, out instance);
			if (!flag)
			{
				bool flag2 = fieldId >= 8;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesAccessories.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (!flag3)
				{
					this._dataStatesAccessories.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
			}
		}

		// Token: 0x0600505D RID: 20573 RVA: 0x002BF8CC File Offset: 0x002BDACC
		private bool IsModifiedWrapper_Accessories(int objectId, ushort fieldId)
		{
			Accessory instance;
			bool flag = !this._accessories.TryGetValue(objectId, out instance);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = fieldId >= 8;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = this._dataStatesAccessories.IsModified(instance.DataStatesOffset, (int)fieldId);
			}
			return result;
		}

		// Token: 0x0600505E RID: 20574 RVA: 0x002BF944 File Offset: 0x002BDB44
		public Clothing GetElement_Clothing(int objectId)
		{
			return this._clothing[objectId];
		}

		// Token: 0x0600505F RID: 20575 RVA: 0x002BF964 File Offset: 0x002BDB64
		public bool TryGetElement_Clothing(int objectId, out Clothing element)
		{
			return this._clothing.TryGetValue(objectId, out element);
		}

		// Token: 0x06005060 RID: 20576 RVA: 0x002BF984 File Offset: 0x002BDB84
		private unsafe void AddElement_Clothing(int objectId, Clothing instance)
		{
			instance.CollectionHelperData = this.HelperDataClothing;
			instance.DataStatesOffset = this._dataStatesClothing.Create();
			this._clothing.Add(objectId, instance);
			byte* pData = OperationAdder.FixedObjectCollection_Add<int>(6, 3, objectId, 30);
			instance.Serialize(pData);
		}

		// Token: 0x06005061 RID: 20577 RVA: 0x002BF9D0 File Offset: 0x002BDBD0
		private void RemoveElement_Clothing(int objectId)
		{
			Clothing instance;
			bool flag = !this._clothing.TryGetValue(objectId, out instance);
			if (!flag)
			{
				this._dataStatesClothing.Remove(instance.DataStatesOffset);
				this._clothing.Remove(objectId);
				OperationAdder.FixedObjectCollection_Remove<int>(6, 3, objectId);
			}
		}

		// Token: 0x06005062 RID: 20578 RVA: 0x002BFA1D File Offset: 0x002BDC1D
		private void ClearClothing()
		{
			this._dataStatesClothing.Clear();
			this._clothing.Clear();
			OperationAdder.FixedObjectCollection_Clear(6, 3);
		}

		// Token: 0x06005063 RID: 20579 RVA: 0x002BFA40 File Offset: 0x002BDC40
		public int GetElementField_Clothing(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
		{
			Clothing instance;
			bool flag = !this._clothing.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				string tag = "GetElementField_Clothing";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
				defaultInterpolatedStringHandler.AppendLiteral(" with field ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
				result = -1;
			}
			else
			{
				if (resetModified)
				{
					this._dataStatesClothing.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
				switch (fieldId)
				{
				case 0:
					result = Serializer.Serialize(instance.GetId(), dataPool);
					break;
				case 1:
					result = Serializer.Serialize(instance.GetTemplateId(), dataPool);
					break;
				case 2:
					result = Serializer.Serialize(instance.GetMaxDurability(), dataPool);
					break;
				case 3:
					result = Serializer.Serialize(instance.GetEquipmentEffectId(), dataPool);
					break;
				case 4:
					result = Serializer.Serialize(instance.GetCurrDurability(), dataPool);
					break;
				case 5:
					result = Serializer.Serialize(instance.GetModificationState(), dataPool);
					break;
				case 6:
					result = Serializer.Serialize(instance.GetEquippedCharId(), dataPool);
					break;
				case 7:
					result = Serializer.Serialize(instance.GetGender(), dataPool);
					break;
				case 8:
					result = Serializer.Serialize(instance.GetMaterialResources(), dataPool);
					break;
				default:
				{
					bool flag2 = fieldId >= 46;
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

		// Token: 0x06005064 RID: 20580 RVA: 0x002BFC0C File Offset: 0x002BDE0C
		public void SetElementField_Clothing(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			Clothing instance;
			bool flag = !this._clothing.TryGetValue(objectId, out instance);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
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
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
			{
				short value = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value);
				instance.SetMaxDurability(value, context);
				break;
			}
			case 3:
			{
				short value2 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value2);
				instance.SetEquipmentEffectId(value2, context);
				break;
			}
			case 4:
			{
				short value3 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value3);
				instance.SetCurrDurability(value3, context);
				break;
			}
			case 5:
			{
				byte value4 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value4);
				instance.SetModificationState(value4, context);
				break;
			}
			case 6:
			{
				int value5 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value5);
				instance.SetEquippedCharId(value5, context);
				break;
			}
			case 7:
			{
				sbyte value6 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value6);
				instance.SetGender(value6, context);
				break;
			}
			case 8:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			default:
			{
				bool flag2 = fieldId >= 46;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (flag2)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = fieldId >= 9;
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

		// Token: 0x06005065 RID: 20581 RVA: 0x002BFE8C File Offset: 0x002BE08C
		private int CheckModified_Clothing(int objectId, ushort fieldId, RawDataPool dataPool)
		{
			Clothing instance;
			bool flag = !this._clothing.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = fieldId >= 9;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesClothing.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					this._dataStatesClothing.ResetModified(instance.DataStatesOffset, (int)fieldId);
					switch (fieldId)
					{
					case 0:
						result = Serializer.Serialize(instance.GetId(), dataPool);
						break;
					case 1:
						result = Serializer.Serialize(instance.GetTemplateId(), dataPool);
						break;
					case 2:
						result = Serializer.Serialize(instance.GetMaxDurability(), dataPool);
						break;
					case 3:
						result = Serializer.Serialize(instance.GetEquipmentEffectId(), dataPool);
						break;
					case 4:
						result = Serializer.Serialize(instance.GetCurrDurability(), dataPool);
						break;
					case 5:
						result = Serializer.Serialize(instance.GetModificationState(), dataPool);
						break;
					case 6:
						result = Serializer.Serialize(instance.GetEquippedCharId(), dataPool);
						break;
					case 7:
						result = Serializer.Serialize(instance.GetGender(), dataPool);
						break;
					case 8:
						result = Serializer.Serialize(instance.GetMaterialResources(), dataPool);
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

		// Token: 0x06005066 RID: 20582 RVA: 0x002C001C File Offset: 0x002BE21C
		private void ResetModifiedWrapper_Clothing(int objectId, ushort fieldId)
		{
			Clothing instance;
			bool flag = !this._clothing.TryGetValue(objectId, out instance);
			if (!flag)
			{
				bool flag2 = fieldId >= 9;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesClothing.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (!flag3)
				{
					this._dataStatesClothing.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
			}
		}

		// Token: 0x06005067 RID: 20583 RVA: 0x002C00AC File Offset: 0x002BE2AC
		private bool IsModifiedWrapper_Clothing(int objectId, ushort fieldId)
		{
			Clothing instance;
			bool flag = !this._clothing.TryGetValue(objectId, out instance);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = fieldId >= 9;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = this._dataStatesClothing.IsModified(instance.DataStatesOffset, (int)fieldId);
			}
			return result;
		}

		// Token: 0x06005068 RID: 20584 RVA: 0x002C0124 File Offset: 0x002BE324
		public Carrier GetElement_Carriers(int objectId)
		{
			return this._carriers[objectId];
		}

		// Token: 0x06005069 RID: 20585 RVA: 0x002C0144 File Offset: 0x002BE344
		public bool TryGetElement_Carriers(int objectId, out Carrier element)
		{
			return this._carriers.TryGetValue(objectId, out element);
		}

		// Token: 0x0600506A RID: 20586 RVA: 0x002C0164 File Offset: 0x002BE364
		private unsafe void AddElement_Carriers(int objectId, Carrier instance)
		{
			instance.CollectionHelperData = this.HelperDataCarriers;
			instance.DataStatesOffset = this._dataStatesCarriers.Create();
			this._carriers.Add(objectId, instance);
			byte* pData = OperationAdder.FixedObjectCollection_Add<int>(6, 4, objectId, 29);
			instance.Serialize(pData);
		}

		// Token: 0x0600506B RID: 20587 RVA: 0x002C01B0 File Offset: 0x002BE3B0
		private void RemoveElement_Carriers(int objectId)
		{
			Carrier instance;
			bool flag = !this._carriers.TryGetValue(objectId, out instance);
			if (!flag)
			{
				this._dataStatesCarriers.Remove(instance.DataStatesOffset);
				this._carriers.Remove(objectId);
				OperationAdder.FixedObjectCollection_Remove<int>(6, 4, objectId);
			}
		}

		// Token: 0x0600506C RID: 20588 RVA: 0x002C01FD File Offset: 0x002BE3FD
		private void ClearCarriers()
		{
			this._dataStatesCarriers.Clear();
			this._carriers.Clear();
			OperationAdder.FixedObjectCollection_Clear(6, 4);
		}

		// Token: 0x0600506D RID: 20589 RVA: 0x002C0220 File Offset: 0x002BE420
		public int GetElementField_Carriers(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
		{
			Carrier instance;
			bool flag = !this._carriers.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				string tag = "GetElementField_Carriers";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
				defaultInterpolatedStringHandler.AppendLiteral(" with field ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
				result = -1;
			}
			else
			{
				if (resetModified)
				{
					this._dataStatesCarriers.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
				switch (fieldId)
				{
				case 0:
					result = Serializer.Serialize(instance.GetId(), dataPool);
					break;
				case 1:
					result = Serializer.Serialize(instance.GetTemplateId(), dataPool);
					break;
				case 2:
					result = Serializer.Serialize(instance.GetMaxDurability(), dataPool);
					break;
				case 3:
					result = Serializer.Serialize(instance.GetEquipmentEffectId(), dataPool);
					break;
				case 4:
					result = Serializer.Serialize(instance.GetCurrDurability(), dataPool);
					break;
				case 5:
					result = Serializer.Serialize(instance.GetModificationState(), dataPool);
					break;
				case 6:
					result = Serializer.Serialize(instance.GetEquippedCharId(), dataPool);
					break;
				case 7:
					result = Serializer.Serialize(instance.GetMaterialResources(), dataPool);
					break;
				default:
				{
					bool flag2 = fieldId >= 54;
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

		// Token: 0x0600506E RID: 20590 RVA: 0x002C03D8 File Offset: 0x002BE5D8
		public void SetElementField_Carriers(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			Carrier instance;
			bool flag = !this._carriers.TryGetValue(objectId, out instance);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
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
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
			{
				short value = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value);
				instance.SetMaxDurability(value, context);
				break;
			}
			case 3:
			{
				short value2 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value2);
				instance.SetEquipmentEffectId(value2, context);
				break;
			}
			case 4:
			{
				short value3 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value3);
				instance.SetCurrDurability(value3, context);
				break;
			}
			case 5:
			{
				byte value4 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value4);
				instance.SetModificationState(value4, context);
				break;
			}
			case 6:
			{
				int value5 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value5);
				instance.SetEquippedCharId(value5, context);
				break;
			}
			case 7:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			default:
			{
				bool flag2 = fieldId >= 54;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (flag2)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = fieldId >= 8;
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

		// Token: 0x0600506F RID: 20591 RVA: 0x002C0634 File Offset: 0x002BE834
		private int CheckModified_Carriers(int objectId, ushort fieldId, RawDataPool dataPool)
		{
			Carrier instance;
			bool flag = !this._carriers.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = fieldId >= 8;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesCarriers.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					this._dataStatesCarriers.ResetModified(instance.DataStatesOffset, (int)fieldId);
					switch (fieldId)
					{
					case 0:
						result = Serializer.Serialize(instance.GetId(), dataPool);
						break;
					case 1:
						result = Serializer.Serialize(instance.GetTemplateId(), dataPool);
						break;
					case 2:
						result = Serializer.Serialize(instance.GetMaxDurability(), dataPool);
						break;
					case 3:
						result = Serializer.Serialize(instance.GetEquipmentEffectId(), dataPool);
						break;
					case 4:
						result = Serializer.Serialize(instance.GetCurrDurability(), dataPool);
						break;
					case 5:
						result = Serializer.Serialize(instance.GetModificationState(), dataPool);
						break;
					case 6:
						result = Serializer.Serialize(instance.GetEquippedCharId(), dataPool);
						break;
					case 7:
						result = Serializer.Serialize(instance.GetMaterialResources(), dataPool);
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

		// Token: 0x06005070 RID: 20592 RVA: 0x002C07A8 File Offset: 0x002BE9A8
		private void ResetModifiedWrapper_Carriers(int objectId, ushort fieldId)
		{
			Carrier instance;
			bool flag = !this._carriers.TryGetValue(objectId, out instance);
			if (!flag)
			{
				bool flag2 = fieldId >= 8;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesCarriers.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (!flag3)
				{
					this._dataStatesCarriers.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
			}
		}

		// Token: 0x06005071 RID: 20593 RVA: 0x002C0838 File Offset: 0x002BEA38
		private bool IsModifiedWrapper_Carriers(int objectId, ushort fieldId)
		{
			Carrier instance;
			bool flag = !this._carriers.TryGetValue(objectId, out instance);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = fieldId >= 8;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = this._dataStatesCarriers.IsModified(instance.DataStatesOffset, (int)fieldId);
			}
			return result;
		}

		// Token: 0x06005072 RID: 20594 RVA: 0x002C08B0 File Offset: 0x002BEAB0
		public Material GetElement_Materials(int objectId)
		{
			return this._materials[objectId];
		}

		// Token: 0x06005073 RID: 20595 RVA: 0x002C08D0 File Offset: 0x002BEAD0
		public bool TryGetElement_Materials(int objectId, out Material element)
		{
			return this._materials.TryGetValue(objectId, out element);
		}

		// Token: 0x06005074 RID: 20596 RVA: 0x002C08F0 File Offset: 0x002BEAF0
		private unsafe void AddElement_Materials(int objectId, Material instance)
		{
			instance.CollectionHelperData = this.HelperDataMaterials;
			instance.DataStatesOffset = this._dataStatesMaterials.Create();
			this._materials.Add(objectId, instance);
			byte* pData = OperationAdder.FixedObjectCollection_Add<int>(6, 5, objectId, 11);
			instance.Serialize(pData);
		}

		// Token: 0x06005075 RID: 20597 RVA: 0x002C093C File Offset: 0x002BEB3C
		private void RemoveElement_Materials(int objectId)
		{
			Material instance;
			bool flag = !this._materials.TryGetValue(objectId, out instance);
			if (!flag)
			{
				this._dataStatesMaterials.Remove(instance.DataStatesOffset);
				this._materials.Remove(objectId);
				OperationAdder.FixedObjectCollection_Remove<int>(6, 5, objectId);
			}
		}

		// Token: 0x06005076 RID: 20598 RVA: 0x002C0989 File Offset: 0x002BEB89
		private void ClearMaterials()
		{
			this._dataStatesMaterials.Clear();
			this._materials.Clear();
			OperationAdder.FixedObjectCollection_Clear(6, 5);
		}

		// Token: 0x06005077 RID: 20599 RVA: 0x002C09AC File Offset: 0x002BEBAC
		public int GetElementField_Materials(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
		{
			Material instance;
			bool flag = !this._materials.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				string tag = "GetElementField_Materials";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
				defaultInterpolatedStringHandler.AppendLiteral(" with field ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
				result = -1;
			}
			else
			{
				if (resetModified)
				{
					this._dataStatesMaterials.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
				switch (fieldId)
				{
				case 0:
					result = Serializer.Serialize(instance.GetId(), dataPool);
					break;
				case 1:
					result = Serializer.Serialize(instance.GetTemplateId(), dataPool);
					break;
				case 2:
					result = Serializer.Serialize(instance.GetMaxDurability(), dataPool);
					break;
				case 3:
					result = Serializer.Serialize(instance.GetCurrDurability(), dataPool);
					break;
				case 4:
					result = Serializer.Serialize(instance.GetModificationState(), dataPool);
					break;
				default:
				{
					bool flag2 = fieldId >= 87;
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

		// Token: 0x06005078 RID: 20600 RVA: 0x002C0B1C File Offset: 0x002BED1C
		public void SetElementField_Materials(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			Material instance;
			bool flag = !this._materials.TryGetValue(objectId, out instance);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
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
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
			{
				short value = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value);
				instance.SetMaxDurability(value, context);
				break;
			}
			case 3:
			{
				short value2 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value2);
				instance.SetCurrDurability(value2, context);
				break;
			}
			case 4:
			{
				byte value3 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value3);
				instance.SetModificationState(value3, context);
				break;
			}
			default:
			{
				bool flag2 = fieldId >= 87;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (flag2)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = fieldId >= 5;
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

		// Token: 0x06005079 RID: 20601 RVA: 0x002C0D00 File Offset: 0x002BEF00
		private int CheckModified_Materials(int objectId, ushort fieldId, RawDataPool dataPool)
		{
			Material instance;
			bool flag = !this._materials.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = fieldId >= 5;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesMaterials.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					this._dataStatesMaterials.ResetModified(instance.DataStatesOffset, (int)fieldId);
					switch (fieldId)
					{
					case 0:
						result = Serializer.Serialize(instance.GetId(), dataPool);
						break;
					case 1:
						result = Serializer.Serialize(instance.GetTemplateId(), dataPool);
						break;
					case 2:
						result = Serializer.Serialize(instance.GetMaxDurability(), dataPool);
						break;
					case 3:
						result = Serializer.Serialize(instance.GetCurrDurability(), dataPool);
						break;
					case 4:
						result = Serializer.Serialize(instance.GetModificationState(), dataPool);
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

		// Token: 0x0600507A RID: 20602 RVA: 0x002C0E34 File Offset: 0x002BF034
		private void ResetModifiedWrapper_Materials(int objectId, ushort fieldId)
		{
			Material instance;
			bool flag = !this._materials.TryGetValue(objectId, out instance);
			if (!flag)
			{
				bool flag2 = fieldId >= 5;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesMaterials.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (!flag3)
				{
					this._dataStatesMaterials.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
			}
		}

		// Token: 0x0600507B RID: 20603 RVA: 0x002C0EC4 File Offset: 0x002BF0C4
		private bool IsModifiedWrapper_Materials(int objectId, ushort fieldId)
		{
			Material instance;
			bool flag = !this._materials.TryGetValue(objectId, out instance);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = fieldId >= 5;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = this._dataStatesMaterials.IsModified(instance.DataStatesOffset, (int)fieldId);
			}
			return result;
		}

		// Token: 0x0600507C RID: 20604 RVA: 0x002C0F3C File Offset: 0x002BF13C
		public CraftTool GetElement_CraftTools(int objectId)
		{
			return this._craftTools[objectId];
		}

		// Token: 0x0600507D RID: 20605 RVA: 0x002C0F5C File Offset: 0x002BF15C
		public bool TryGetElement_CraftTools(int objectId, out CraftTool element)
		{
			return this._craftTools.TryGetValue(objectId, out element);
		}

		// Token: 0x0600507E RID: 20606 RVA: 0x002C0F7C File Offset: 0x002BF17C
		private unsafe void AddElement_CraftTools(int objectId, CraftTool instance)
		{
			instance.CollectionHelperData = this.HelperDataCraftTools;
			instance.DataStatesOffset = this._dataStatesCraftTools.Create();
			this._craftTools.Add(objectId, instance);
			byte* pData = OperationAdder.FixedObjectCollection_Add<int>(6, 6, objectId, 11);
			instance.Serialize(pData);
		}

		// Token: 0x0600507F RID: 20607 RVA: 0x002C0FC8 File Offset: 0x002BF1C8
		private void RemoveElement_CraftTools(int objectId)
		{
			CraftTool instance;
			bool flag = !this._craftTools.TryGetValue(objectId, out instance);
			if (!flag)
			{
				this._dataStatesCraftTools.Remove(instance.DataStatesOffset);
				this._craftTools.Remove(objectId);
				OperationAdder.FixedObjectCollection_Remove<int>(6, 6, objectId);
			}
		}

		// Token: 0x06005080 RID: 20608 RVA: 0x002C1015 File Offset: 0x002BF215
		private void ClearCraftTools()
		{
			this._dataStatesCraftTools.Clear();
			this._craftTools.Clear();
			OperationAdder.FixedObjectCollection_Clear(6, 6);
		}

		// Token: 0x06005081 RID: 20609 RVA: 0x002C1038 File Offset: 0x002BF238
		public int GetElementField_CraftTools(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
		{
			CraftTool instance;
			bool flag = !this._craftTools.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				string tag = "GetElementField_CraftTools";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
				defaultInterpolatedStringHandler.AppendLiteral(" with field ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
				result = -1;
			}
			else
			{
				if (resetModified)
				{
					this._dataStatesCraftTools.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
				switch (fieldId)
				{
				case 0:
					result = Serializer.Serialize(instance.GetId(), dataPool);
					break;
				case 1:
					result = Serializer.Serialize(instance.GetTemplateId(), dataPool);
					break;
				case 2:
					result = Serializer.Serialize(instance.GetMaxDurability(), dataPool);
					break;
				case 3:
					result = Serializer.Serialize(instance.GetCurrDurability(), dataPool);
					break;
				case 4:
					result = Serializer.Serialize(instance.GetModificationState(), dataPool);
					break;
				default:
				{
					bool flag2 = fieldId >= 35;
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

		// Token: 0x06005082 RID: 20610 RVA: 0x002C11A8 File Offset: 0x002BF3A8
		public void SetElementField_CraftTools(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			CraftTool instance;
			bool flag = !this._craftTools.TryGetValue(objectId, out instance);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
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
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
			{
				short value = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value);
				instance.SetMaxDurability(value, context);
				break;
			}
			case 3:
			{
				short value2 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value2);
				instance.SetCurrDurability(value2, context);
				break;
			}
			case 4:
			{
				byte value3 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value3);
				instance.SetModificationState(value3, context);
				break;
			}
			default:
			{
				bool flag2 = fieldId >= 35;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (flag2)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = fieldId >= 5;
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

		// Token: 0x06005083 RID: 20611 RVA: 0x002C138C File Offset: 0x002BF58C
		private int CheckModified_CraftTools(int objectId, ushort fieldId, RawDataPool dataPool)
		{
			CraftTool instance;
			bool flag = !this._craftTools.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = fieldId >= 5;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesCraftTools.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					this._dataStatesCraftTools.ResetModified(instance.DataStatesOffset, (int)fieldId);
					switch (fieldId)
					{
					case 0:
						result = Serializer.Serialize(instance.GetId(), dataPool);
						break;
					case 1:
						result = Serializer.Serialize(instance.GetTemplateId(), dataPool);
						break;
					case 2:
						result = Serializer.Serialize(instance.GetMaxDurability(), dataPool);
						break;
					case 3:
						result = Serializer.Serialize(instance.GetCurrDurability(), dataPool);
						break;
					case 4:
						result = Serializer.Serialize(instance.GetModificationState(), dataPool);
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

		// Token: 0x06005084 RID: 20612 RVA: 0x002C14C0 File Offset: 0x002BF6C0
		private void ResetModifiedWrapper_CraftTools(int objectId, ushort fieldId)
		{
			CraftTool instance;
			bool flag = !this._craftTools.TryGetValue(objectId, out instance);
			if (!flag)
			{
				bool flag2 = fieldId >= 5;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesCraftTools.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (!flag3)
				{
					this._dataStatesCraftTools.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
			}
		}

		// Token: 0x06005085 RID: 20613 RVA: 0x002C1550 File Offset: 0x002BF750
		private bool IsModifiedWrapper_CraftTools(int objectId, ushort fieldId)
		{
			CraftTool instance;
			bool flag = !this._craftTools.TryGetValue(objectId, out instance);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = fieldId >= 5;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = this._dataStatesCraftTools.IsModified(instance.DataStatesOffset, (int)fieldId);
			}
			return result;
		}

		// Token: 0x06005086 RID: 20614 RVA: 0x002C15C8 File Offset: 0x002BF7C8
		public Food GetElement_Foods(int objectId)
		{
			return this._foods[objectId];
		}

		// Token: 0x06005087 RID: 20615 RVA: 0x002C15E8 File Offset: 0x002BF7E8
		public bool TryGetElement_Foods(int objectId, out Food element)
		{
			return this._foods.TryGetValue(objectId, out element);
		}

		// Token: 0x06005088 RID: 20616 RVA: 0x002C1608 File Offset: 0x002BF808
		private unsafe void AddElement_Foods(int objectId, Food instance)
		{
			instance.CollectionHelperData = this.HelperDataFoods;
			instance.DataStatesOffset = this._dataStatesFoods.Create();
			this._foods.Add(objectId, instance);
			byte* pData = OperationAdder.FixedObjectCollection_Add<int>(6, 7, objectId, 11);
			instance.Serialize(pData);
		}

		// Token: 0x06005089 RID: 20617 RVA: 0x002C1654 File Offset: 0x002BF854
		private void RemoveElement_Foods(int objectId)
		{
			Food instance;
			bool flag = !this._foods.TryGetValue(objectId, out instance);
			if (!flag)
			{
				this._dataStatesFoods.Remove(instance.DataStatesOffset);
				this._foods.Remove(objectId);
				OperationAdder.FixedObjectCollection_Remove<int>(6, 7, objectId);
			}
		}

		// Token: 0x0600508A RID: 20618 RVA: 0x002C16A1 File Offset: 0x002BF8A1
		private void ClearFoods()
		{
			this._dataStatesFoods.Clear();
			this._foods.Clear();
			OperationAdder.FixedObjectCollection_Clear(6, 7);
		}

		// Token: 0x0600508B RID: 20619 RVA: 0x002C16C4 File Offset: 0x002BF8C4
		public int GetElementField_Foods(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
		{
			Food instance;
			bool flag = !this._foods.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				string tag = "GetElementField_Foods";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
				defaultInterpolatedStringHandler.AppendLiteral(" with field ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
				result = -1;
			}
			else
			{
				if (resetModified)
				{
					this._dataStatesFoods.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
				switch (fieldId)
				{
				case 0:
					result = Serializer.Serialize(instance.GetId(), dataPool);
					break;
				case 1:
					result = Serializer.Serialize(instance.GetTemplateId(), dataPool);
					break;
				case 2:
					result = Serializer.Serialize(instance.GetMaxDurability(), dataPool);
					break;
				case 3:
					result = Serializer.Serialize(instance.GetCurrDurability(), dataPool);
					break;
				case 4:
					result = Serializer.Serialize(instance.GetModificationState(), dataPool);
					break;
				default:
				{
					bool flag2 = fieldId >= 71;
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

		// Token: 0x0600508C RID: 20620 RVA: 0x002C1834 File Offset: 0x002BFA34
		public void SetElementField_Foods(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			Food instance;
			bool flag = !this._foods.TryGetValue(objectId, out instance);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
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
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
			{
				short value = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value);
				instance.SetMaxDurability(value, context);
				break;
			}
			case 3:
			{
				short value2 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value2);
				instance.SetCurrDurability(value2, context);
				break;
			}
			case 4:
			{
				byte value3 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value3);
				instance.SetModificationState(value3, context);
				break;
			}
			default:
			{
				bool flag2 = fieldId >= 71;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (flag2)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = fieldId >= 5;
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

		// Token: 0x0600508D RID: 20621 RVA: 0x002C1A18 File Offset: 0x002BFC18
		private int CheckModified_Foods(int objectId, ushort fieldId, RawDataPool dataPool)
		{
			Food instance;
			bool flag = !this._foods.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = fieldId >= 5;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesFoods.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					this._dataStatesFoods.ResetModified(instance.DataStatesOffset, (int)fieldId);
					switch (fieldId)
					{
					case 0:
						result = Serializer.Serialize(instance.GetId(), dataPool);
						break;
					case 1:
						result = Serializer.Serialize(instance.GetTemplateId(), dataPool);
						break;
					case 2:
						result = Serializer.Serialize(instance.GetMaxDurability(), dataPool);
						break;
					case 3:
						result = Serializer.Serialize(instance.GetCurrDurability(), dataPool);
						break;
					case 4:
						result = Serializer.Serialize(instance.GetModificationState(), dataPool);
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

		// Token: 0x0600508E RID: 20622 RVA: 0x002C1B4C File Offset: 0x002BFD4C
		private void ResetModifiedWrapper_Foods(int objectId, ushort fieldId)
		{
			Food instance;
			bool flag = !this._foods.TryGetValue(objectId, out instance);
			if (!flag)
			{
				bool flag2 = fieldId >= 5;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesFoods.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (!flag3)
				{
					this._dataStatesFoods.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
			}
		}

		// Token: 0x0600508F RID: 20623 RVA: 0x002C1BDC File Offset: 0x002BFDDC
		private bool IsModifiedWrapper_Foods(int objectId, ushort fieldId)
		{
			Food instance;
			bool flag = !this._foods.TryGetValue(objectId, out instance);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = fieldId >= 5;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = this._dataStatesFoods.IsModified(instance.DataStatesOffset, (int)fieldId);
			}
			return result;
		}

		// Token: 0x06005090 RID: 20624 RVA: 0x002C1C54 File Offset: 0x002BFE54
		public Medicine GetElement_Medicines(int objectId)
		{
			return this._medicines[objectId];
		}

		// Token: 0x06005091 RID: 20625 RVA: 0x002C1C74 File Offset: 0x002BFE74
		public bool TryGetElement_Medicines(int objectId, out Medicine element)
		{
			return this._medicines.TryGetValue(objectId, out element);
		}

		// Token: 0x06005092 RID: 20626 RVA: 0x002C1C94 File Offset: 0x002BFE94
		private unsafe void AddElement_Medicines(int objectId, Medicine instance)
		{
			instance.CollectionHelperData = this.HelperDataMedicines;
			instance.DataStatesOffset = this._dataStatesMedicines.Create();
			this._medicines.Add(objectId, instance);
			byte* pData = OperationAdder.FixedObjectCollection_Add<int>(6, 8, objectId, 11);
			instance.Serialize(pData);
		}

		// Token: 0x06005093 RID: 20627 RVA: 0x002C1CE0 File Offset: 0x002BFEE0
		private void RemoveElement_Medicines(int objectId)
		{
			Medicine instance;
			bool flag = !this._medicines.TryGetValue(objectId, out instance);
			if (!flag)
			{
				this._dataStatesMedicines.Remove(instance.DataStatesOffset);
				this._medicines.Remove(objectId);
				OperationAdder.FixedObjectCollection_Remove<int>(6, 8, objectId);
			}
		}

		// Token: 0x06005094 RID: 20628 RVA: 0x002C1D2D File Offset: 0x002BFF2D
		private void ClearMedicines()
		{
			this._dataStatesMedicines.Clear();
			this._medicines.Clear();
			OperationAdder.FixedObjectCollection_Clear(6, 8);
		}

		// Token: 0x06005095 RID: 20629 RVA: 0x002C1D50 File Offset: 0x002BFF50
		public int GetElementField_Medicines(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
		{
			Medicine instance;
			bool flag = !this._medicines.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				string tag = "GetElementField_Medicines";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
				defaultInterpolatedStringHandler.AppendLiteral(" with field ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
				result = -1;
			}
			else
			{
				if (resetModified)
				{
					this._dataStatesMedicines.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
				switch (fieldId)
				{
				case 0:
					result = Serializer.Serialize(instance.GetId(), dataPool);
					break;
				case 1:
					result = Serializer.Serialize(instance.GetTemplateId(), dataPool);
					break;
				case 2:
					result = Serializer.Serialize(instance.GetMaxDurability(), dataPool);
					break;
				case 3:
					result = Serializer.Serialize(instance.GetCurrDurability(), dataPool);
					break;
				case 4:
					result = Serializer.Serialize(instance.GetModificationState(), dataPool);
					break;
				default:
				{
					bool flag2 = fieldId >= 83;
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

		// Token: 0x06005096 RID: 20630 RVA: 0x002C1EC0 File Offset: 0x002C00C0
		public void SetElementField_Medicines(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			Medicine instance;
			bool flag = !this._medicines.TryGetValue(objectId, out instance);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
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
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
			{
				short value = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value);
				instance.SetMaxDurability(value, context);
				break;
			}
			case 3:
			{
				short value2 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value2);
				instance.SetCurrDurability(value2, context);
				break;
			}
			case 4:
			{
				byte value3 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value3);
				instance.SetModificationState(value3, context);
				break;
			}
			default:
			{
				bool flag2 = fieldId >= 83;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (flag2)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = fieldId >= 5;
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

		// Token: 0x06005097 RID: 20631 RVA: 0x002C20A4 File Offset: 0x002C02A4
		private int CheckModified_Medicines(int objectId, ushort fieldId, RawDataPool dataPool)
		{
			Medicine instance;
			bool flag = !this._medicines.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = fieldId >= 5;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesMedicines.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					this._dataStatesMedicines.ResetModified(instance.DataStatesOffset, (int)fieldId);
					switch (fieldId)
					{
					case 0:
						result = Serializer.Serialize(instance.GetId(), dataPool);
						break;
					case 1:
						result = Serializer.Serialize(instance.GetTemplateId(), dataPool);
						break;
					case 2:
						result = Serializer.Serialize(instance.GetMaxDurability(), dataPool);
						break;
					case 3:
						result = Serializer.Serialize(instance.GetCurrDurability(), dataPool);
						break;
					case 4:
						result = Serializer.Serialize(instance.GetModificationState(), dataPool);
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

		// Token: 0x06005098 RID: 20632 RVA: 0x002C21D8 File Offset: 0x002C03D8
		private void ResetModifiedWrapper_Medicines(int objectId, ushort fieldId)
		{
			Medicine instance;
			bool flag = !this._medicines.TryGetValue(objectId, out instance);
			if (!flag)
			{
				bool flag2 = fieldId >= 5;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesMedicines.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (!flag3)
				{
					this._dataStatesMedicines.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
			}
		}

		// Token: 0x06005099 RID: 20633 RVA: 0x002C2268 File Offset: 0x002C0468
		private bool IsModifiedWrapper_Medicines(int objectId, ushort fieldId)
		{
			Medicine instance;
			bool flag = !this._medicines.TryGetValue(objectId, out instance);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = fieldId >= 5;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = this._dataStatesMedicines.IsModified(instance.DataStatesOffset, (int)fieldId);
			}
			return result;
		}

		// Token: 0x0600509A RID: 20634 RVA: 0x002C22E0 File Offset: 0x002C04E0
		public TeaWine GetElement_TeaWines(int objectId)
		{
			return this._teaWines[objectId];
		}

		// Token: 0x0600509B RID: 20635 RVA: 0x002C2300 File Offset: 0x002C0500
		public bool TryGetElement_TeaWines(int objectId, out TeaWine element)
		{
			return this._teaWines.TryGetValue(objectId, out element);
		}

		// Token: 0x0600509C RID: 20636 RVA: 0x002C2320 File Offset: 0x002C0520
		private unsafe void AddElement_TeaWines(int objectId, TeaWine instance)
		{
			instance.CollectionHelperData = this.HelperDataTeaWines;
			instance.DataStatesOffset = this._dataStatesTeaWines.Create();
			this._teaWines.Add(objectId, instance);
			byte* pData = OperationAdder.FixedObjectCollection_Add<int>(6, 9, objectId, 11);
			instance.Serialize(pData);
		}

		// Token: 0x0600509D RID: 20637 RVA: 0x002C2370 File Offset: 0x002C0570
		private void RemoveElement_TeaWines(int objectId)
		{
			TeaWine instance;
			bool flag = !this._teaWines.TryGetValue(objectId, out instance);
			if (!flag)
			{
				this._dataStatesTeaWines.Remove(instance.DataStatesOffset);
				this._teaWines.Remove(objectId);
				OperationAdder.FixedObjectCollection_Remove<int>(6, 9, objectId);
			}
		}

		// Token: 0x0600509E RID: 20638 RVA: 0x002C23BE File Offset: 0x002C05BE
		private void ClearTeaWines()
		{
			this._dataStatesTeaWines.Clear();
			this._teaWines.Clear();
			OperationAdder.FixedObjectCollection_Clear(6, 9);
		}

		// Token: 0x0600509F RID: 20639 RVA: 0x002C23E4 File Offset: 0x002C05E4
		public int GetElementField_TeaWines(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
		{
			TeaWine instance;
			bool flag = !this._teaWines.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				string tag = "GetElementField_TeaWines";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
				defaultInterpolatedStringHandler.AppendLiteral(" with field ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
				result = -1;
			}
			else
			{
				if (resetModified)
				{
					this._dataStatesTeaWines.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
				switch (fieldId)
				{
				case 0:
					result = Serializer.Serialize(instance.GetId(), dataPool);
					break;
				case 1:
					result = Serializer.Serialize(instance.GetTemplateId(), dataPool);
					break;
				case 2:
					result = Serializer.Serialize(instance.GetMaxDurability(), dataPool);
					break;
				case 3:
					result = Serializer.Serialize(instance.GetCurrDurability(), dataPool);
					break;
				case 4:
					result = Serializer.Serialize(instance.GetModificationState(), dataPool);
					break;
				default:
				{
					bool flag2 = fieldId >= 53;
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

		// Token: 0x060050A0 RID: 20640 RVA: 0x002C2554 File Offset: 0x002C0754
		public void SetElementField_TeaWines(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			TeaWine instance;
			bool flag = !this._teaWines.TryGetValue(objectId, out instance);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
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
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
			{
				short value = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value);
				instance.SetMaxDurability(value, context);
				break;
			}
			case 3:
			{
				short value2 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value2);
				instance.SetCurrDurability(value2, context);
				break;
			}
			case 4:
			{
				byte value3 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value3);
				instance.SetModificationState(value3, context);
				break;
			}
			default:
			{
				bool flag2 = fieldId >= 53;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (flag2)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = fieldId >= 5;
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

		// Token: 0x060050A1 RID: 20641 RVA: 0x002C2738 File Offset: 0x002C0938
		private int CheckModified_TeaWines(int objectId, ushort fieldId, RawDataPool dataPool)
		{
			TeaWine instance;
			bool flag = !this._teaWines.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = fieldId >= 5;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesTeaWines.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					this._dataStatesTeaWines.ResetModified(instance.DataStatesOffset, (int)fieldId);
					switch (fieldId)
					{
					case 0:
						result = Serializer.Serialize(instance.GetId(), dataPool);
						break;
					case 1:
						result = Serializer.Serialize(instance.GetTemplateId(), dataPool);
						break;
					case 2:
						result = Serializer.Serialize(instance.GetMaxDurability(), dataPool);
						break;
					case 3:
						result = Serializer.Serialize(instance.GetCurrDurability(), dataPool);
						break;
					case 4:
						result = Serializer.Serialize(instance.GetModificationState(), dataPool);
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

		// Token: 0x060050A2 RID: 20642 RVA: 0x002C286C File Offset: 0x002C0A6C
		private void ResetModifiedWrapper_TeaWines(int objectId, ushort fieldId)
		{
			TeaWine instance;
			bool flag = !this._teaWines.TryGetValue(objectId, out instance);
			if (!flag)
			{
				bool flag2 = fieldId >= 5;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesTeaWines.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (!flag3)
				{
					this._dataStatesTeaWines.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
			}
		}

		// Token: 0x060050A3 RID: 20643 RVA: 0x002C28FC File Offset: 0x002C0AFC
		private bool IsModifiedWrapper_TeaWines(int objectId, ushort fieldId)
		{
			TeaWine instance;
			bool flag = !this._teaWines.TryGetValue(objectId, out instance);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = fieldId >= 5;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = this._dataStatesTeaWines.IsModified(instance.DataStatesOffset, (int)fieldId);
			}
			return result;
		}

		// Token: 0x060050A4 RID: 20644 RVA: 0x002C2974 File Offset: 0x002C0B74
		public SkillBook GetElement_SkillBooks(int objectId)
		{
			return this._skillBooks[objectId];
		}

		// Token: 0x060050A5 RID: 20645 RVA: 0x002C2994 File Offset: 0x002C0B94
		public bool TryGetElement_SkillBooks(int objectId, out SkillBook element)
		{
			return this._skillBooks.TryGetValue(objectId, out element);
		}

		// Token: 0x060050A6 RID: 20646 RVA: 0x002C29B4 File Offset: 0x002C0BB4
		private unsafe void AddElement_SkillBooks(int objectId, SkillBook instance)
		{
			instance.CollectionHelperData = this.HelperDataSkillBooks;
			instance.DataStatesOffset = this._dataStatesSkillBooks.Create();
			this._skillBooks.Add(objectId, instance);
			byte* pData = OperationAdder.FixedObjectCollection_Add<int>(6, 10, objectId, 14);
			instance.Serialize(pData);
		}

		// Token: 0x060050A7 RID: 20647 RVA: 0x002C2A04 File Offset: 0x002C0C04
		private void RemoveElement_SkillBooks(int objectId)
		{
			SkillBook instance;
			bool flag = !this._skillBooks.TryGetValue(objectId, out instance);
			if (!flag)
			{
				this._dataStatesSkillBooks.Remove(instance.DataStatesOffset);
				this._skillBooks.Remove(objectId);
				OperationAdder.FixedObjectCollection_Remove<int>(6, 10, objectId);
			}
		}

		// Token: 0x060050A8 RID: 20648 RVA: 0x002C2A52 File Offset: 0x002C0C52
		private void ClearSkillBooks()
		{
			this._dataStatesSkillBooks.Clear();
			this._skillBooks.Clear();
			OperationAdder.FixedObjectCollection_Clear(6, 10);
		}

		// Token: 0x060050A9 RID: 20649 RVA: 0x002C2A78 File Offset: 0x002C0C78
		public int GetElementField_SkillBooks(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
		{
			SkillBook instance;
			bool flag = !this._skillBooks.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				string tag = "GetElementField_SkillBooks";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
				defaultInterpolatedStringHandler.AppendLiteral(" with field ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
				result = -1;
			}
			else
			{
				if (resetModified)
				{
					this._dataStatesSkillBooks.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
				switch (fieldId)
				{
				case 0:
					result = Serializer.Serialize(instance.GetId(), dataPool);
					break;
				case 1:
					result = Serializer.Serialize(instance.GetTemplateId(), dataPool);
					break;
				case 2:
					result = Serializer.Serialize(instance.GetMaxDurability(), dataPool);
					break;
				case 3:
					result = Serializer.Serialize(instance.GetCurrDurability(), dataPool);
					break;
				case 4:
					result = Serializer.Serialize(instance.GetModificationState(), dataPool);
					break;
				case 5:
					result = Serializer.Serialize(instance.GetPageTypes(), dataPool);
					break;
				case 6:
					result = Serializer.Serialize(instance.GetPageIncompleteState(), dataPool);
					break;
				default:
				{
					bool flag2 = fieldId >= 40;
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

		// Token: 0x060050AA RID: 20650 RVA: 0x002C2C14 File Offset: 0x002C0E14
		public void SetElementField_SkillBooks(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			SkillBook instance;
			bool flag = !this._skillBooks.TryGetValue(objectId, out instance);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
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
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
			{
				short value = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value);
				instance.SetMaxDurability(value, context);
				break;
			}
			case 3:
			{
				short value2 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value2);
				instance.SetCurrDurability(value2, context);
				break;
			}
			case 4:
			{
				byte value3 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value3);
				instance.SetModificationState(value3, context);
				break;
			}
			case 5:
			{
				byte value4 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value4);
				instance.SetPageTypes(value4, context);
				break;
			}
			case 6:
			{
				ushort value5 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value5);
				instance.SetPageIncompleteState(value5, context);
				break;
			}
			default:
			{
				bool flag2 = fieldId >= 40;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (flag2)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = fieldId >= 7;
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

		// Token: 0x060050AB RID: 20651 RVA: 0x002C2E40 File Offset: 0x002C1040
		private int CheckModified_SkillBooks(int objectId, ushort fieldId, RawDataPool dataPool)
		{
			SkillBook instance;
			bool flag = !this._skillBooks.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = fieldId >= 7;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesSkillBooks.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					this._dataStatesSkillBooks.ResetModified(instance.DataStatesOffset, (int)fieldId);
					switch (fieldId)
					{
					case 0:
						result = Serializer.Serialize(instance.GetId(), dataPool);
						break;
					case 1:
						result = Serializer.Serialize(instance.GetTemplateId(), dataPool);
						break;
					case 2:
						result = Serializer.Serialize(instance.GetMaxDurability(), dataPool);
						break;
					case 3:
						result = Serializer.Serialize(instance.GetCurrDurability(), dataPool);
						break;
					case 4:
						result = Serializer.Serialize(instance.GetModificationState(), dataPool);
						break;
					case 5:
						result = Serializer.Serialize(instance.GetPageTypes(), dataPool);
						break;
					case 6:
						result = Serializer.Serialize(instance.GetPageIncompleteState(), dataPool);
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

		// Token: 0x060050AC RID: 20652 RVA: 0x002C2FA0 File Offset: 0x002C11A0
		private void ResetModifiedWrapper_SkillBooks(int objectId, ushort fieldId)
		{
			SkillBook instance;
			bool flag = !this._skillBooks.TryGetValue(objectId, out instance);
			if (!flag)
			{
				bool flag2 = fieldId >= 7;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesSkillBooks.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (!flag3)
				{
					this._dataStatesSkillBooks.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
			}
		}

		// Token: 0x060050AD RID: 20653 RVA: 0x002C3030 File Offset: 0x002C1230
		private bool IsModifiedWrapper_SkillBooks(int objectId, ushort fieldId)
		{
			SkillBook instance;
			bool flag = !this._skillBooks.TryGetValue(objectId, out instance);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = fieldId >= 7;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = this._dataStatesSkillBooks.IsModified(instance.DataStatesOffset, (int)fieldId);
			}
			return result;
		}

		// Token: 0x060050AE RID: 20654 RVA: 0x002C30A8 File Offset: 0x002C12A8
		public Cricket GetElement_Crickets(int objectId)
		{
			return this._crickets[objectId];
		}

		// Token: 0x060050AF RID: 20655 RVA: 0x002C30C8 File Offset: 0x002C12C8
		public bool TryGetElement_Crickets(int objectId, out Cricket element)
		{
			return this._crickets.TryGetValue(objectId, out element);
		}

		// Token: 0x060050B0 RID: 20656 RVA: 0x002C30E8 File Offset: 0x002C12E8
		private unsafe void AddElement_Crickets(int objectId, Cricket instance)
		{
			instance.CollectionHelperData = this.HelperDataCrickets;
			instance.DataStatesOffset = this._dataStatesCrickets.Create();
			this._crickets.Add(objectId, instance);
			byte* pData = OperationAdder.FixedObjectCollection_Add<int>(6, 11, objectId, 34);
			instance.Serialize(pData);
		}

		// Token: 0x060050B1 RID: 20657 RVA: 0x002C3138 File Offset: 0x002C1338
		private void RemoveElement_Crickets(int objectId)
		{
			Cricket instance;
			bool flag = !this._crickets.TryGetValue(objectId, out instance);
			if (!flag)
			{
				this._dataStatesCrickets.Remove(instance.DataStatesOffset);
				this._crickets.Remove(objectId);
				OperationAdder.FixedObjectCollection_Remove<int>(6, 11, objectId);
			}
		}

		// Token: 0x060050B2 RID: 20658 RVA: 0x002C3186 File Offset: 0x002C1386
		private void ClearCrickets()
		{
			this._dataStatesCrickets.Clear();
			this._crickets.Clear();
			OperationAdder.FixedObjectCollection_Clear(6, 11);
		}

		// Token: 0x060050B3 RID: 20659 RVA: 0x002C31AC File Offset: 0x002C13AC
		public int GetElementField_Crickets(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
		{
			Cricket instance;
			bool flag = !this._crickets.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				string tag = "GetElementField_Crickets";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
				defaultInterpolatedStringHandler.AppendLiteral(" with field ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
				result = -1;
			}
			else
			{
				if (resetModified)
				{
					this._dataStatesCrickets.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
				switch (fieldId)
				{
				case 0:
					result = Serializer.Serialize(instance.GetId(), dataPool);
					break;
				case 1:
					result = Serializer.Serialize(instance.GetTemplateId(), dataPool);
					break;
				case 2:
					result = Serializer.Serialize(instance.GetMaxDurability(), dataPool);
					break;
				case 3:
					result = Serializer.Serialize(instance.GetCurrDurability(), dataPool);
					break;
				case 4:
					result = Serializer.Serialize(instance.GetModificationState(), dataPool);
					break;
				case 5:
					result = Serializer.Serialize(instance.GetColorId(), dataPool);
					break;
				case 6:
					result = Serializer.Serialize(instance.GetPartId(), dataPool);
					break;
				case 7:
					result = Serializer.Serialize(instance.GetInjuries(), dataPool);
					break;
				case 8:
					result = Serializer.Serialize(instance.GetWinsCount(), dataPool);
					break;
				case 9:
					result = Serializer.Serialize(instance.GetLossesCount(), dataPool);
					break;
				case 10:
					result = Serializer.Serialize(instance.GetBestEnemyColorId(), dataPool);
					break;
				case 11:
					result = Serializer.Serialize(instance.GetBestEnemyPartId(), dataPool);
					break;
				case 12:
					result = Serializer.Serialize(instance.GetAge(), dataPool);
					break;
				default:
				{
					bool flag2 = fieldId >= 39;
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

		// Token: 0x060050B4 RID: 20660 RVA: 0x002C33D0 File Offset: 0x002C15D0
		public void SetElementField_Crickets(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			Cricket instance;
			bool flag = !this._crickets.TryGetValue(objectId, out instance);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
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
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
			{
				short value = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value);
				instance.SetMaxDurability(value, context);
				break;
			}
			case 3:
			{
				short value2 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value2);
				instance.SetCurrDurability(value2, context);
				break;
			}
			case 4:
			{
				byte value3 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value3);
				instance.SetModificationState(value3, context);
				break;
			}
			case 5:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 6:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 7:
			{
				short[] value4 = instance.GetInjuries();
				Serializer.Deserialize(dataPool, valueOffset, ref value4);
				instance.SetInjuries(value4, context);
				break;
			}
			case 8:
			{
				short value5 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value5);
				instance.SetWinsCount(value5, context);
				break;
			}
			case 9:
			{
				short value6 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value6);
				instance.SetLossesCount(value6, context);
				break;
			}
			case 10:
			{
				short value7 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value7);
				instance.SetBestEnemyColorId(value7, context);
				break;
			}
			case 11:
			{
				short value8 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value8);
				instance.SetBestEnemyPartId(value8, context);
				break;
			}
			case 12:
			{
				sbyte value9 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value9);
				instance.SetAge(value9, context);
				break;
			}
			default:
			{
				bool flag2 = fieldId >= 39;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (flag2)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = fieldId >= 13;
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

		// Token: 0x060050B5 RID: 20661 RVA: 0x002C36F0 File Offset: 0x002C18F0
		private int CheckModified_Crickets(int objectId, ushort fieldId, RawDataPool dataPool)
		{
			Cricket instance;
			bool flag = !this._crickets.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = fieldId >= 13;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesCrickets.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					this._dataStatesCrickets.ResetModified(instance.DataStatesOffset, (int)fieldId);
					switch (fieldId)
					{
					case 0:
						result = Serializer.Serialize(instance.GetId(), dataPool);
						break;
					case 1:
						result = Serializer.Serialize(instance.GetTemplateId(), dataPool);
						break;
					case 2:
						result = Serializer.Serialize(instance.GetMaxDurability(), dataPool);
						break;
					case 3:
						result = Serializer.Serialize(instance.GetCurrDurability(), dataPool);
						break;
					case 4:
						result = Serializer.Serialize(instance.GetModificationState(), dataPool);
						break;
					case 5:
						result = Serializer.Serialize(instance.GetColorId(), dataPool);
						break;
					case 6:
						result = Serializer.Serialize(instance.GetPartId(), dataPool);
						break;
					case 7:
						result = Serializer.Serialize(instance.GetInjuries(), dataPool);
						break;
					case 8:
						result = Serializer.Serialize(instance.GetWinsCount(), dataPool);
						break;
					case 9:
						result = Serializer.Serialize(instance.GetLossesCount(), dataPool);
						break;
					case 10:
						result = Serializer.Serialize(instance.GetBestEnemyColorId(), dataPool);
						break;
					case 11:
						result = Serializer.Serialize(instance.GetBestEnemyPartId(), dataPool);
						break;
					case 12:
						result = Serializer.Serialize(instance.GetAge(), dataPool);
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

		// Token: 0x060050B6 RID: 20662 RVA: 0x002C38D8 File Offset: 0x002C1AD8
		private void ResetModifiedWrapper_Crickets(int objectId, ushort fieldId)
		{
			Cricket instance;
			bool flag = !this._crickets.TryGetValue(objectId, out instance);
			if (!flag)
			{
				bool flag2 = fieldId >= 13;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesCrickets.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (!flag3)
				{
					this._dataStatesCrickets.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
			}
		}

		// Token: 0x060050B7 RID: 20663 RVA: 0x002C3968 File Offset: 0x002C1B68
		private bool IsModifiedWrapper_Crickets(int objectId, ushort fieldId)
		{
			Cricket instance;
			bool flag = !this._crickets.TryGetValue(objectId, out instance);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = fieldId >= 13;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = this._dataStatesCrickets.IsModified(instance.DataStatesOffset, (int)fieldId);
			}
			return result;
		}

		// Token: 0x060050B8 RID: 20664 RVA: 0x002C39E0 File Offset: 0x002C1BE0
		public Misc GetElement_Misc(int objectId)
		{
			return this._misc[objectId];
		}

		// Token: 0x060050B9 RID: 20665 RVA: 0x002C3A00 File Offset: 0x002C1C00
		public bool TryGetElement_Misc(int objectId, out Misc element)
		{
			return this._misc.TryGetValue(objectId, out element);
		}

		// Token: 0x060050BA RID: 20666 RVA: 0x002C3A20 File Offset: 0x002C1C20
		private unsafe void AddElement_Misc(int objectId, Misc instance)
		{
			instance.CollectionHelperData = this.HelperDataMisc;
			instance.DataStatesOffset = this._dataStatesMisc.Create();
			this._misc.Add(objectId, instance);
			byte* pData = OperationAdder.FixedObjectCollection_Add<int>(6, 12, objectId, 11);
			instance.Serialize(pData);
		}

		// Token: 0x060050BB RID: 20667 RVA: 0x002C3A70 File Offset: 0x002C1C70
		private void RemoveElement_Misc(int objectId)
		{
			Misc instance;
			bool flag = !this._misc.TryGetValue(objectId, out instance);
			if (!flag)
			{
				this._dataStatesMisc.Remove(instance.DataStatesOffset);
				this._misc.Remove(objectId);
				OperationAdder.FixedObjectCollection_Remove<int>(6, 12, objectId);
			}
		}

		// Token: 0x060050BC RID: 20668 RVA: 0x002C3ABE File Offset: 0x002C1CBE
		private void ClearMisc()
		{
			this._dataStatesMisc.Clear();
			this._misc.Clear();
			OperationAdder.FixedObjectCollection_Clear(6, 12);
		}

		// Token: 0x060050BD RID: 20669 RVA: 0x002C3AE4 File Offset: 0x002C1CE4
		public int GetElementField_Misc(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
		{
			Misc instance;
			bool flag = !this._misc.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				string tag = "GetElementField_Misc";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
				defaultInterpolatedStringHandler.AppendLiteral(" with field ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
				result = -1;
			}
			else
			{
				if (resetModified)
				{
					this._dataStatesMisc.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
				switch (fieldId)
				{
				case 0:
					result = Serializer.Serialize(instance.GetId(), dataPool);
					break;
				case 1:
					result = Serializer.Serialize(instance.GetTemplateId(), dataPool);
					break;
				case 2:
					result = Serializer.Serialize(instance.GetMaxDurability(), dataPool);
					break;
				case 3:
					result = Serializer.Serialize(instance.GetCurrDurability(), dataPool);
					break;
				case 4:
					result = Serializer.Serialize(instance.GetModificationState(), dataPool);
					break;
				default:
				{
					bool flag2 = fieldId >= 47;
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

		// Token: 0x060050BE RID: 20670 RVA: 0x002C3C54 File Offset: 0x002C1E54
		public void SetElementField_Misc(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			Misc instance;
			bool flag = !this._misc.TryGetValue(objectId, out instance);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
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
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
			{
				short value = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value);
				instance.SetMaxDurability(value, context);
				break;
			}
			case 3:
			{
				short value2 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value2);
				instance.SetCurrDurability(value2, context);
				break;
			}
			case 4:
			{
				byte value3 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value3);
				instance.SetModificationState(value3, context);
				break;
			}
			default:
			{
				bool flag2 = fieldId >= 47;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (flag2)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = fieldId >= 5;
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

		// Token: 0x060050BF RID: 20671 RVA: 0x002C3E38 File Offset: 0x002C2038
		private int CheckModified_Misc(int objectId, ushort fieldId, RawDataPool dataPool)
		{
			Misc instance;
			bool flag = !this._misc.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = fieldId >= 5;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesMisc.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					this._dataStatesMisc.ResetModified(instance.DataStatesOffset, (int)fieldId);
					switch (fieldId)
					{
					case 0:
						result = Serializer.Serialize(instance.GetId(), dataPool);
						break;
					case 1:
						result = Serializer.Serialize(instance.GetTemplateId(), dataPool);
						break;
					case 2:
						result = Serializer.Serialize(instance.GetMaxDurability(), dataPool);
						break;
					case 3:
						result = Serializer.Serialize(instance.GetCurrDurability(), dataPool);
						break;
					case 4:
						result = Serializer.Serialize(instance.GetModificationState(), dataPool);
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

		// Token: 0x060050C0 RID: 20672 RVA: 0x002C3F6C File Offset: 0x002C216C
		private void ResetModifiedWrapper_Misc(int objectId, ushort fieldId)
		{
			Misc instance;
			bool flag = !this._misc.TryGetValue(objectId, out instance);
			if (!flag)
			{
				bool flag2 = fieldId >= 5;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesMisc.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (!flag3)
				{
					this._dataStatesMisc.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
			}
		}

		// Token: 0x060050C1 RID: 20673 RVA: 0x002C3FFC File Offset: 0x002C21FC
		private bool IsModifiedWrapper_Misc(int objectId, ushort fieldId)
		{
			Misc instance;
			bool flag = !this._misc.TryGetValue(objectId, out instance);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = fieldId >= 5;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = this._dataStatesMisc.IsModified(instance.DataStatesOffset, (int)fieldId);
			}
			return result;
		}

		// Token: 0x060050C2 RID: 20674 RVA: 0x002C4074 File Offset: 0x002C2274
		private int GetNextItemId()
		{
			return this._nextItemId;
		}

		// Token: 0x060050C3 RID: 20675 RVA: 0x002C408C File Offset: 0x002C228C
		private unsafe void SetNextItemId(int value, DataContext context)
		{
			this._nextItemId = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(13, this.DataStates, ItemDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(6, 13, 4);
			*(int*)pData = this._nextItemId;
			pData += 4;
		}

		// Token: 0x060050C4 RID: 20676 RVA: 0x002C40CC File Offset: 0x002C22CC
		private int GetElement_StackableItems(TemplateKey elementId)
		{
			return this._stackableItems[elementId];
		}

		// Token: 0x060050C5 RID: 20677 RVA: 0x002C40EC File Offset: 0x002C22EC
		private bool TryGetElement_StackableItems(TemplateKey elementId, out int value)
		{
			return this._stackableItems.TryGetValue(elementId, out value);
		}

		// Token: 0x060050C6 RID: 20678 RVA: 0x002C410C File Offset: 0x002C230C
		private unsafe void AddElement_StackableItems(TemplateKey elementId, int value, DataContext context)
		{
			this._stackableItems.Add(elementId, value);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, this.DataStates, ItemDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValueCollection_Add<TemplateKey>(6, 14, elementId, 4);
			*(int*)pData = value;
			pData += 4;
		}

		// Token: 0x060050C7 RID: 20679 RVA: 0x002C4150 File Offset: 0x002C2350
		private unsafe void SetElement_StackableItems(TemplateKey elementId, int value, DataContext context)
		{
			this._stackableItems[elementId] = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, this.DataStates, ItemDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValueCollection_Set<TemplateKey>(6, 14, elementId, 4);
			*(int*)pData = value;
			pData += 4;
		}

		// Token: 0x060050C8 RID: 20680 RVA: 0x002C4192 File Offset: 0x002C2392
		private void RemoveElement_StackableItems(TemplateKey elementId, DataContext context)
		{
			this._stackableItems.Remove(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, this.DataStates, ItemDomain.CacheInfluences, context);
			OperationAdder.FixedSingleValueCollection_Remove<TemplateKey>(6, 14, elementId);
		}

		// Token: 0x060050C9 RID: 20681 RVA: 0x002C41C0 File Offset: 0x002C23C0
		private void ClearStackableItems(DataContext context)
		{
			this._stackableItems.Clear();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, this.DataStates, ItemDomain.CacheInfluences, context);
			OperationAdder.FixedSingleValueCollection_Clear(6, 14);
		}

		// Token: 0x060050CA RID: 20682 RVA: 0x002C41EC File Offset: 0x002C23EC
		[Obsolete("DomainData _poisonItems is no longer in use.")]
		private PoisonEffects GetElement_PoisonItems(int elementId)
		{
			return this._poisonItems[elementId];
		}

		// Token: 0x060050CB RID: 20683 RVA: 0x002C420C File Offset: 0x002C240C
		[Obsolete("DomainData _poisonItems is no longer in use.")]
		private bool TryGetElement_PoisonItems(int elementId, out PoisonEffects value)
		{
			return this._poisonItems.TryGetValue(elementId, out value);
		}

		// Token: 0x060050CC RID: 20684 RVA: 0x002C422C File Offset: 0x002C242C
		[Obsolete("DomainData _poisonItems is no longer in use.")]
		private unsafe void AddElement_PoisonItems(int elementId, ref PoisonEffects value, DataContext context)
		{
			this._poisonItems.Add(elementId, value);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(15, this.DataStates, ItemDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValueCollection_Add<int>(6, 15, elementId, 21);
			pData += value.Serialize(pData);
		}

		// Token: 0x060050CD RID: 20685 RVA: 0x002C4278 File Offset: 0x002C2478
		[Obsolete("DomainData _poisonItems is no longer in use.")]
		private unsafe void SetElement_PoisonItems(int elementId, ref PoisonEffects value, DataContext context)
		{
			this._poisonItems[elementId] = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(15, this.DataStates, ItemDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValueCollection_Set<int>(6, 15, elementId, 21);
			pData += value.Serialize(pData);
		}

		// Token: 0x060050CE RID: 20686 RVA: 0x002C42C3 File Offset: 0x002C24C3
		[Obsolete("DomainData _poisonItems is no longer in use.")]
		private void RemoveElement_PoisonItems(int elementId, DataContext context)
		{
			this._poisonItems.Remove(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(15, this.DataStates, ItemDomain.CacheInfluences, context);
			OperationAdder.FixedSingleValueCollection_Remove<int>(6, 15, elementId);
		}

		// Token: 0x060050CF RID: 20687 RVA: 0x002C42F1 File Offset: 0x002C24F1
		[Obsolete("DomainData _poisonItems is no longer in use.")]
		private void ClearPoisonItems(DataContext context)
		{
			this._poisonItems.Clear();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(15, this.DataStates, ItemDomain.CacheInfluences, context);
			OperationAdder.FixedSingleValueCollection_Clear(6, 15);
		}

		// Token: 0x060050D0 RID: 20688 RVA: 0x002C4320 File Offset: 0x002C2520
		private RefiningEffects GetElement_RefinedItems(int elementId)
		{
			return this._refinedItems[elementId];
		}

		// Token: 0x060050D1 RID: 20689 RVA: 0x002C4340 File Offset: 0x002C2540
		private bool TryGetElement_RefinedItems(int elementId, out RefiningEffects value)
		{
			return this._refinedItems.TryGetValue(elementId, out value);
		}

		// Token: 0x060050D2 RID: 20690 RVA: 0x002C4360 File Offset: 0x002C2560
		private unsafe void AddElement_RefinedItems(int elementId, RefiningEffects value, DataContext context)
		{
			this._refinedItems.Add(elementId, value);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(16, this.DataStates, ItemDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValueCollection_Add<int>(6, 16, elementId, 10);
			pData += value.Serialize(pData);
		}

		// Token: 0x060050D3 RID: 20691 RVA: 0x002C43A8 File Offset: 0x002C25A8
		private unsafe void SetElement_RefinedItems(int elementId, RefiningEffects value, DataContext context)
		{
			this._refinedItems[elementId] = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(16, this.DataStates, ItemDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValueCollection_Set<int>(6, 16, elementId, 10);
			pData += value.Serialize(pData);
		}

		// Token: 0x060050D4 RID: 20692 RVA: 0x002C43EF File Offset: 0x002C25EF
		private void RemoveElement_RefinedItems(int elementId, DataContext context)
		{
			this._refinedItems.Remove(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(16, this.DataStates, ItemDomain.CacheInfluences, context);
			OperationAdder.FixedSingleValueCollection_Remove<int>(6, 16, elementId);
		}

		// Token: 0x060050D5 RID: 20693 RVA: 0x002C441D File Offset: 0x002C261D
		private void ClearRefinedItems(DataContext context)
		{
			this._refinedItems.Clear();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(16, this.DataStates, ItemDomain.CacheInfluences, context);
			OperationAdder.FixedSingleValueCollection_Clear(6, 16);
		}

		// Token: 0x060050D6 RID: 20694 RVA: 0x002C444C File Offset: 0x002C264C
		private ItemKey GetEmptyHandKey()
		{
			return this._emptyHandKey;
		}

		// Token: 0x060050D7 RID: 20695 RVA: 0x002C4464 File Offset: 0x002C2664
		private unsafe void SetEmptyHandKey(ItemKey value, DataContext context)
		{
			this._emptyHandKey = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(17, this.DataStates, ItemDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(6, 17, 8);
			pData += this._emptyHandKey.Serialize(pData);
		}

		// Token: 0x060050D8 RID: 20696 RVA: 0x002C44A8 File Offset: 0x002C26A8
		private ItemKey GetBranchKey()
		{
			return this._branchKey;
		}

		// Token: 0x060050D9 RID: 20697 RVA: 0x002C44C0 File Offset: 0x002C26C0
		private unsafe void SetBranchKey(ItemKey value, DataContext context)
		{
			this._branchKey = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(18, this.DataStates, ItemDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(6, 18, 8);
			pData += this._branchKey.Serialize(pData);
		}

		// Token: 0x060050DA RID: 20698 RVA: 0x002C4504 File Offset: 0x002C2704
		private ItemKey GetStoneKey()
		{
			return this._stoneKey;
		}

		// Token: 0x060050DB RID: 20699 RVA: 0x002C451C File Offset: 0x002C271C
		private unsafe void SetStoneKey(ItemKey value, DataContext context)
		{
			this._stoneKey = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(19, this.DataStates, ItemDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(6, 19, 8);
			pData += this._stoneKey.Serialize(pData);
		}

		// Token: 0x060050DC RID: 20700 RVA: 0x002C4560 File Offset: 0x002C2760
		private GameData.Utilities.ShortList GetElement_ExternEquipmentEffects(int elementId)
		{
			return this._externEquipmentEffects[elementId];
		}

		// Token: 0x060050DD RID: 20701 RVA: 0x002C4580 File Offset: 0x002C2780
		private bool TryGetElement_ExternEquipmentEffects(int elementId, out GameData.Utilities.ShortList value)
		{
			return this._externEquipmentEffects.TryGetValue(elementId, out value);
		}

		// Token: 0x060050DE RID: 20702 RVA: 0x002C459F File Offset: 0x002C279F
		private void AddElement_ExternEquipmentEffects(int elementId, GameData.Utilities.ShortList value, DataContext context)
		{
			this._externEquipmentEffects.Add(elementId, value);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, this.DataStates, ItemDomain.CacheInfluences, context);
		}

		// Token: 0x060050DF RID: 20703 RVA: 0x002C45C4 File Offset: 0x002C27C4
		private void SetElement_ExternEquipmentEffects(int elementId, GameData.Utilities.ShortList value, DataContext context)
		{
			this._externEquipmentEffects[elementId] = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, this.DataStates, ItemDomain.CacheInfluences, context);
		}

		// Token: 0x060050E0 RID: 20704 RVA: 0x002C45E9 File Offset: 0x002C27E9
		private void RemoveElement_ExternEquipmentEffects(int elementId, DataContext context)
		{
			this._externEquipmentEffects.Remove(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, this.DataStates, ItemDomain.CacheInfluences, context);
		}

		// Token: 0x060050E1 RID: 20705 RVA: 0x002C460D File Offset: 0x002C280D
		private void ClearExternEquipmentEffects(DataContext context)
		{
			this._externEquipmentEffects.Clear();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, this.DataStates, ItemDomain.CacheInfluences, context);
		}

		// Token: 0x060050E2 RID: 20706 RVA: 0x002C4630 File Offset: 0x002C2830
		public override void OnInitializeGameDataModule()
		{
			this.InitializeOnInitializeGameDataModule();
		}

		// Token: 0x060050E3 RID: 20707 RVA: 0x002C463C File Offset: 0x002C283C
		public unsafe override void OnEnterNewWorld()
		{
			this.InitializeOnEnterNewWorld();
			this.InitializeInternalDataOfCollections();
			foreach (KeyValuePair<int, Weapon> entry in this._weapons)
			{
				int objectId = entry.Key;
				Weapon instance = entry.Value;
				byte* pData = OperationAdder.DynamicObjectCollection_Add<int>(6, 0, objectId, instance.GetSerializedSize());
				instance.Serialize(pData);
			}
			foreach (KeyValuePair<int, Armor> entry2 in this._armors)
			{
				int objectId2 = entry2.Key;
				Armor instance2 = entry2.Value;
				byte* pData2 = OperationAdder.FixedObjectCollection_Add<int>(6, 1, objectId2, 29);
				instance2.Serialize(pData2);
			}
			foreach (KeyValuePair<int, Accessory> entry3 in this._accessories)
			{
				int objectId3 = entry3.Key;
				Accessory instance3 = entry3.Value;
				byte* pData3 = OperationAdder.FixedObjectCollection_Add<int>(6, 2, objectId3, 29);
				instance3.Serialize(pData3);
			}
			foreach (KeyValuePair<int, Clothing> entry4 in this._clothing)
			{
				int objectId4 = entry4.Key;
				Clothing instance4 = entry4.Value;
				byte* pData4 = OperationAdder.FixedObjectCollection_Add<int>(6, 3, objectId4, 30);
				instance4.Serialize(pData4);
			}
			foreach (KeyValuePair<int, Carrier> entry5 in this._carriers)
			{
				int objectId5 = entry5.Key;
				Carrier instance5 = entry5.Value;
				byte* pData5 = OperationAdder.FixedObjectCollection_Add<int>(6, 4, objectId5, 29);
				instance5.Serialize(pData5);
			}
			foreach (KeyValuePair<int, Material> entry6 in this._materials)
			{
				int objectId6 = entry6.Key;
				Material instance6 = entry6.Value;
				byte* pData6 = OperationAdder.FixedObjectCollection_Add<int>(6, 5, objectId6, 11);
				instance6.Serialize(pData6);
			}
			foreach (KeyValuePair<int, CraftTool> entry7 in this._craftTools)
			{
				int objectId7 = entry7.Key;
				CraftTool instance7 = entry7.Value;
				byte* pData7 = OperationAdder.FixedObjectCollection_Add<int>(6, 6, objectId7, 11);
				instance7.Serialize(pData7);
			}
			foreach (KeyValuePair<int, Food> entry8 in this._foods)
			{
				int objectId8 = entry8.Key;
				Food instance8 = entry8.Value;
				byte* pData8 = OperationAdder.FixedObjectCollection_Add<int>(6, 7, objectId8, 11);
				instance8.Serialize(pData8);
			}
			foreach (KeyValuePair<int, Medicine> entry9 in this._medicines)
			{
				int objectId9 = entry9.Key;
				Medicine instance9 = entry9.Value;
				byte* pData9 = OperationAdder.FixedObjectCollection_Add<int>(6, 8, objectId9, 11);
				instance9.Serialize(pData9);
			}
			foreach (KeyValuePair<int, TeaWine> entry10 in this._teaWines)
			{
				int objectId10 = entry10.Key;
				TeaWine instance10 = entry10.Value;
				byte* pData10 = OperationAdder.FixedObjectCollection_Add<int>(6, 9, objectId10, 11);
				instance10.Serialize(pData10);
			}
			foreach (KeyValuePair<int, SkillBook> entry11 in this._skillBooks)
			{
				int objectId11 = entry11.Key;
				SkillBook instance11 = entry11.Value;
				byte* pData11 = OperationAdder.FixedObjectCollection_Add<int>(6, 10, objectId11, 14);
				instance11.Serialize(pData11);
			}
			foreach (KeyValuePair<int, Cricket> entry12 in this._crickets)
			{
				int objectId12 = entry12.Key;
				Cricket instance12 = entry12.Value;
				byte* pData12 = OperationAdder.FixedObjectCollection_Add<int>(6, 11, objectId12, 34);
				instance12.Serialize(pData12);
			}
			foreach (KeyValuePair<int, Misc> entry13 in this._misc)
			{
				int objectId13 = entry13.Key;
				Misc instance13 = entry13.Value;
				byte* pData13 = OperationAdder.FixedObjectCollection_Add<int>(6, 12, objectId13, 11);
				instance13.Serialize(pData13);
			}
			byte* pData14 = OperationAdder.FixedSingleValue_Set(6, 13, 4);
			*(int*)pData14 = this._nextItemId;
			pData14 += 4;
			foreach (KeyValuePair<TemplateKey, int> entry14 in this._stackableItems)
			{
				TemplateKey elementId = entry14.Key;
				int value = entry14.Value;
				byte* pData15 = OperationAdder.FixedSingleValueCollection_Add<TemplateKey>(6, 14, elementId, 4);
				*(int*)pData15 = value;
				pData15 += 4;
			}
			foreach (KeyValuePair<int, PoisonEffects> entry15 in this._poisonItems)
			{
				int elementId2 = entry15.Key;
				PoisonEffects value2 = entry15.Value;
				byte* pData16 = OperationAdder.FixedSingleValueCollection_Add<int>(6, 15, elementId2, 21);
				pData16 += value2.Serialize(pData16);
			}
			foreach (KeyValuePair<int, RefiningEffects> entry16 in this._refinedItems)
			{
				int elementId3 = entry16.Key;
				RefiningEffects value3 = entry16.Value;
				byte* pData17 = OperationAdder.FixedSingleValueCollection_Add<int>(6, 16, elementId3, 10);
				pData17 += value3.Serialize(pData17);
			}
			byte* pData18 = OperationAdder.FixedSingleValue_Set(6, 17, 8);
			pData18 += this._emptyHandKey.Serialize(pData18);
			byte* pData19 = OperationAdder.FixedSingleValue_Set(6, 18, 8);
			pData19 += this._branchKey.Serialize(pData19);
			byte* pData20 = OperationAdder.FixedSingleValue_Set(6, 19, 8);
			pData20 += this._stoneKey.Serialize(pData20);
		}

		// Token: 0x060050E4 RID: 20708 RVA: 0x002C4D84 File Offset: 0x002C2F84
		public override void OnLoadWorld()
		{
			this._pendingLoadingOperationIds = new Queue<uint>();
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicObjectCollection_GetAllObjects(6, 0));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedObjectCollection_GetAllObjects(6, 1));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedObjectCollection_GetAllObjects(6, 2));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedObjectCollection_GetAllObjects(6, 3));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedObjectCollection_GetAllObjects(6, 4));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedObjectCollection_GetAllObjects(6, 5));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedObjectCollection_GetAllObjects(6, 6));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedObjectCollection_GetAllObjects(6, 7));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedObjectCollection_GetAllObjects(6, 8));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedObjectCollection_GetAllObjects(6, 9));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedObjectCollection_GetAllObjects(6, 10));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedObjectCollection_GetAllObjects(6, 11));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedObjectCollection_GetAllObjects(6, 12));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(6, 13));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValueCollection_GetAll(6, 14));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValueCollection_GetAll(6, 15));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValueCollection_GetAll(6, 16));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(6, 17));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(6, 18));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(6, 19));
		}

		// Token: 0x060050E5 RID: 20709 RVA: 0x002C4F24 File Offset: 0x002C3124
		public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
		{
			int result;
			switch (dataId)
			{
			case 0:
				result = this.GetElementField_Weapons((int)subId0, (ushort)subId1, dataPool, resetModified);
				break;
			case 1:
				result = this.GetElementField_Armors((int)subId0, (ushort)subId1, dataPool, resetModified);
				break;
			case 2:
				result = this.GetElementField_Accessories((int)subId0, (ushort)subId1, dataPool, resetModified);
				break;
			case 3:
				result = this.GetElementField_Clothing((int)subId0, (ushort)subId1, dataPool, resetModified);
				break;
			case 4:
				result = this.GetElementField_Carriers((int)subId0, (ushort)subId1, dataPool, resetModified);
				break;
			case 5:
				result = this.GetElementField_Materials((int)subId0, (ushort)subId1, dataPool, resetModified);
				break;
			case 6:
				result = this.GetElementField_CraftTools((int)subId0, (ushort)subId1, dataPool, resetModified);
				break;
			case 7:
				result = this.GetElementField_Foods((int)subId0, (ushort)subId1, dataPool, resetModified);
				break;
			case 8:
				result = this.GetElementField_Medicines((int)subId0, (ushort)subId1, dataPool, resetModified);
				break;
			case 9:
				result = this.GetElementField_TeaWines((int)subId0, (ushort)subId1, dataPool, resetModified);
				break;
			case 10:
				result = this.GetElementField_SkillBooks((int)subId0, (ushort)subId1, dataPool, resetModified);
				break;
			case 11:
				result = this.GetElementField_Crickets((int)subId0, (ushort)subId1, dataPool, resetModified);
				break;
			case 12:
				result = this.GetElementField_Misc((int)subId0, (ushort)subId1, dataPool, resetModified);
				break;
			case 13:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to get value of dataId: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 14:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to get value of dataId: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 15:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to get value of dataId: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 16:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to get value of dataId: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 17:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to get value of dataId: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 18:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to get value of dataId: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 19:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to get value of dataId: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 20:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to get value of dataId: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x060050E6 RID: 20710 RVA: 0x002C5230 File Offset: 0x002C3430
		public override void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			switch (dataId)
			{
			case 0:
				this.SetElementField_Weapons((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
				break;
			case 1:
				this.SetElementField_Armors((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
				break;
			case 2:
				this.SetElementField_Accessories((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
				break;
			case 3:
				this.SetElementField_Clothing((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
				break;
			case 4:
				this.SetElementField_Carriers((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
				break;
			case 5:
				this.SetElementField_Materials((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
				break;
			case 6:
				this.SetElementField_CraftTools((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
				break;
			case 7:
				this.SetElementField_Foods((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
				break;
			case 8:
				this.SetElementField_Medicines((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
				break;
			case 9:
				this.SetElementField_TeaWines((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
				break;
			case 10:
				this.SetElementField_SkillBooks((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
				break;
			case 11:
				this.SetElementField_Crickets((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
				break;
			case 12:
				this.SetElementField_Misc((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
				break;
			case 13:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 14:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 15:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 16:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 17:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 18:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 19:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 20:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x060050E7 RID: 20711 RVA: 0x002C5554 File Offset: 0x002C3754
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
				ItemDisplayData itemDisplayData = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemDisplayData);
				List<ItemDisplayData> returnValue = this.IdentifyPoisons(context, charId, itemDisplayData);
				result = Serializer.Serialize(returnValue, returnDataPool);
				break;
			}
			case 1:
			{
				int argsCount2 = operation.ArgsCount;
				int num2 = argsCount2;
				if (num2 != 4)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short colorId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref colorId);
				short partId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref partId);
				short singLevel = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref singLevel);
				sbyte cricketPlaceId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref cricketPlaceId);
				List<ItemDisplayData> returnValue2 = this.CatchCricket(context, colorId, partId, singLevel, cricketPlaceId);
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
				int itemId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemId);
				CricketData returnValue3 = this.GetCricketData(itemId);
				result = Serializer.Serialize(returnValue3, returnDataPool);
				break;
			}
			case 3:
			{
				int argsCount4 = operation.ArgsCount;
				int num4 = argsCount4;
				if (num4 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int itemId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemId2);
				bool win = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref win);
				int enemyItemId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref enemyItemId);
				this.SetCricketRecord(context, itemId2, win, enemyItemId);
				result = -1;
				break;
			}
			case 4:
			{
				int argsCount5 = operation.ArgsCount;
				int num5 = argsCount5;
				if (num5 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int itemId3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemId3);
				int index = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref index);
				short value = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref value);
				this.AddCricketInjury(context, itemId3, index, value);
				result = -1;
				break;
			}
			case 5:
			{
				int argsCount6 = operation.ArgsCount;
				int num6 = argsCount6;
				if (num6 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ItemKey weaponKey = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref weaponKey);
				List<sbyte> returnValue4 = this.GetWeaponTricks(weaponKey);
				result = Serializer.Serialize(returnValue4, returnDataPool);
				break;
			}
			case 6:
			{
				int argsCount7 = operation.ArgsCount;
				int num7 = argsCount7;
				if (num7 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ItemKey cricketKey = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref cricketKey);
				short[] returnValue5 = this.GetCricketCombatRecords(cricketKey);
				result = Serializer.Serialize(returnValue5, returnDataPool);
				break;
			}
			case 7:
			{
				int argsCount8 = operation.ArgsCount;
				int num8 = argsCount8;
				if (num8 != 1)
				{
					if (num8 != 2)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					ItemKey itemKey = default(ItemKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey);
					int charId2 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId2);
					ItemDisplayData returnValue6 = this.GetItemDisplayData(itemKey, charId2);
					result = Serializer.Serialize(returnValue6, returnDataPool);
				}
				else
				{
					ItemKey itemKey2 = default(ItemKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey2);
					ItemDisplayData returnValue7 = this.GetItemDisplayData(itemKey2, -1);
					result = Serializer.Serialize(returnValue7, returnDataPool);
				}
				break;
			}
			case 8:
			{
				int argsCount9 = operation.ArgsCount;
				int num9 = argsCount9;
				if (num9 != 1)
				{
					if (num9 != 2)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					List<ItemKey> itemKeyList = null;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKeyList);
					int charId3 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId3);
					List<ItemDisplayData> returnValue8 = this.GetItemDisplayDataList(itemKeyList, charId3);
					result = Serializer.Serialize(returnValue8, returnDataPool);
				}
				else
				{
					List<ItemKey> itemKeyList2 = null;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKeyList2);
					List<ItemDisplayData> returnValue9 = this.GetItemDisplayDataList(itemKeyList2, -1);
					result = Serializer.Serialize(returnValue9, returnDataPool);
				}
				break;
			}
			case 9:
			{
				int argsCount10 = operation.ArgsCount;
				int num10 = argsCount10;
				if (num10 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ItemKey itemKey3 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey3);
				SkillBookPageDisplayData returnValue10 = this.GetSkillBookPagesInfo(itemKey3);
				result = Serializer.Serialize(returnValue10, returnDataPool);
				break;
			}
			case 10:
			{
				int argsCount11 = operation.ArgsCount;
				int num11 = argsCount11;
				if (num11 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ItemKey itemKey4 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey4);
				int returnValue11 = this.GetValue(itemKey4);
				result = Serializer.Serialize(returnValue11, returnDataPool);
				break;
			}
			case 11:
			{
				int argsCount12 = operation.ArgsCount;
				int num12 = argsCount12;
				if (num12 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ItemKey itemKey5 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey5);
				int returnValue12 = this.GetPrice(itemKey5);
				result = Serializer.Serialize(returnValue12, returnDataPool);
				break;
			}
			case 12:
			{
				int argsCount13 = operation.ArgsCount;
				int num13 = argsCount13;
				if (num13 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId4 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId4);
				ItemKey itemKey6 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey6);
				ItemKey toolKey = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref toolKey);
				List<ItemDisplayData> returnValue13 = this.DisassembleItem(context, charId4, itemKey6, toolKey);
				result = Serializer.Serialize(returnValue13, returnDataPool);
				break;
			}
			case 13:
			{
				int argsCount14 = operation.ArgsCount;
				int num14 = argsCount14;
				if (num14 != 2)
				{
					if (num14 != 3)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					int charId5 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId5);
					ItemKey itemKey7 = default(ItemKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey7);
					int count = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref count);
					this.DiscardItem(context, charId5, itemKey7, count);
					result = -1;
				}
				else
				{
					int charId6 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId6);
					ItemKey itemKey8 = default(ItemKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey8);
					this.DiscardItem(context, charId6, itemKey8, 1);
					result = -1;
				}
				break;
			}
			case 14:
			{
				int argsCount15 = operation.ArgsCount;
				int num15 = argsCount15;
				if (num15 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId7 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId7);
				ItemKey toolKey2 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref toolKey2);
				List<ItemKey> returnValue14 = this.GetRepairableItems(context, charId7, toolKey2);
				result = Serializer.Serialize(returnValue14, returnDataPool);
				break;
			}
			case 15:
			{
				int argsCount16 = operation.ArgsCount;
				int num16 = argsCount16;
				if (num16 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId8 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId8);
				ItemKey toolKey3 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref toolKey3);
				List<ItemKey> returnValue15 = this.GetDisassemblableItems(context, charId8, toolKey3);
				result = Serializer.Serialize(returnValue15, returnDataPool);
				break;
			}
			case 16:
			{
				int argsCount17 = operation.ArgsCount;
				int num17 = argsCount17;
				if (num17 != 5)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId9 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId9);
				short changeValue = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref changeValue);
				sbyte itemType = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemType);
				short startId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref startId);
				short endId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref endId);
				this.ChangeDurability(context, charId9, changeValue, itemType, startId, endId);
				result = -1;
				break;
			}
			case 17:
			{
				int argsCount18 = operation.ArgsCount;
				int num18 = argsCount18;
				if (num18 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId10 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId10);
				bool isIdentified = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isIdentified);
				this.ChangePoisonIdentified(context, charId10, isIdentified);
				result = -1;
				break;
			}
			case 18:
			{
				int argsCount19 = operation.ArgsCount;
				int num19 = argsCount19;
				if (num19 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId11 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId11);
				List<ItemKey> keyList = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref keyList);
				this.DiscardItemList(context, charId11, keyList);
				result = -1;
				break;
			}
			case 19:
			{
				int argsCount20 = operation.ArgsCount;
				int num20 = argsCount20;
				if (num20 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId12 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId12);
				List<MultiplyOperation> operationList = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref operationList);
				List<ItemDisplayData> returnValue16 = this.DisassembleItemList(context, charId12, operationList);
				result = Serializer.Serialize(returnValue16, returnDataPool);
				break;
			}
			case 20:
			{
				int argsCount21 = operation.ArgsCount;
				int num21 = argsCount21;
				if (num21 != 3)
				{
					if (num21 != 4)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					int charId13 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId13);
					ItemKey itemKey9 = default(ItemKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey9);
					sbyte itemSourceType = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemSourceType);
					int count2 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref count2);
					this.DiscardItemOptional(context, charId13, itemKey9, itemSourceType, count2);
					result = -1;
				}
				else
				{
					int charId14 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId14);
					ItemKey itemKey10 = default(ItemKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey10);
					sbyte itemSourceType2 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemSourceType2);
					this.DiscardItemOptional(context, charId14, itemKey10, itemSourceType2, 1);
					result = -1;
				}
				break;
			}
			case 21:
			{
				int argsCount22 = operation.ArgsCount;
				int num22 = argsCount22;
				if (num22 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId15 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId15);
				List<ItemKey> keyList2 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref keyList2);
				sbyte itemSourceType3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemSourceType3);
				this.DiscardItemListOptional(context, charId15, keyList2, itemSourceType3);
				result = -1;
				break;
			}
			case 22:
			{
				int argsCount23 = operation.ArgsCount;
				int num23 = argsCount23;
				if (num23 != 5)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId16 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId16);
				ItemKey itemKey11 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey11);
				ItemKey toolKey4 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref toolKey4);
				sbyte itemSourceType4 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemSourceType4);
				sbyte toolSourceType = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref toolSourceType);
				List<ItemDisplayData> returnValue17 = this.DisassembleItemOptional(context, charId16, itemKey11, toolKey4, itemSourceType4, toolSourceType);
				result = Serializer.Serialize(returnValue17, returnDataPool);
				break;
			}
			case 23:
			{
				int argsCount24 = operation.ArgsCount;
				int num24 = argsCount24;
				if (num24 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte minGrade = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref minGrade);
				sbyte maxGrade = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref maxGrade);
				bool onlyNoInjuryCricket = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref onlyNoInjuryCricket);
				this.SetCricketBattleConfig(minGrade, maxGrade, onlyNoInjuryCricket);
				result = -1;
				break;
			}
			case 24:
			{
				int argsCount25 = operation.ArgsCount;
				int num25 = argsCount25;
				if (num25 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<ItemKey> itemList = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemList);
				List<CricketData> returnValue18 = this.GetCricketDataList(itemList);
				result = Serializer.Serialize(returnValue18, returnDataPool);
				break;
			}
			case 25:
			{
				int argsCount26 = operation.ArgsCount;
				int num26 = argsCount26;
				if (num26 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId17 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId17);
				ItemKey weaponKey2 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref weaponKey2);
				ValueTuple<int, int> returnValue19 = this.GetWeaponAttackRange(charId17, weaponKey2);
				result = Serializer.Serialize(returnValue19, returnDataPool);
				break;
			}
			case 26:
			{
				int argsCount27 = operation.ArgsCount;
				int num27 = argsCount27;
				if (num27 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<ItemKey> keyList3 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref keyList3);
				bool[] returnValue20 = this.GetCricketsAliveState(keyList3);
				result = Serializer.Serialize(returnValue20, returnDataPool);
				break;
			}
			case 27:
			{
				int argsCount28 = operation.ArgsCount;
				int num28 = argsCount28;
				if (num28 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ItemKey itemKey12 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey12);
				List<byte> pageIds = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref pageIds);
				List<sbyte> directions = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref directions);
				bool returnValue21 = this.ModifyCombatSkillBookPageNormal(context, itemKey12, pageIds, directions);
				result = Serializer.Serialize(returnValue21, returnDataPool);
				break;
			}
			case 28:
			{
				int argsCount29 = operation.ArgsCount;
				int num29 = argsCount29;
				if (num29 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ItemKey itemKey13 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey13);
				sbyte behaviorType = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref behaviorType);
				bool returnValue22 = this.ModifyCombatSkillBookPageOutline(context, itemKey13, behaviorType);
				result = Serializer.Serialize(returnValue22, returnDataPool);
				break;
			}
			case 29:
			{
				int argsCount30 = operation.ArgsCount;
				int num30 = argsCount30;
				if (num30 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<SkillBookModifyDisplayData> returnValue23 = this.GetTaiwuInventoryCombatSkillBooks();
				result = Serializer.Serialize(returnValue23, returnDataPool);
				break;
			}
			case 30:
				switch (operation.ArgsCount)
				{
				case 1:
				{
					List<ItemKey> itemKeyList3 = null;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKeyList3);
					List<ItemDisplayData> returnValue24 = this.GetItemDisplayDataListOptional(itemKeyList3, -1, -1, false);
					result = Serializer.Serialize(returnValue24, returnDataPool);
					break;
				}
				case 2:
				{
					List<ItemKey> itemKeyList4 = null;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKeyList4);
					int charId18 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId18);
					List<ItemDisplayData> returnValue25 = this.GetItemDisplayDataListOptional(itemKeyList4, charId18, -1, false);
					result = Serializer.Serialize(returnValue25, returnDataPool);
					break;
				}
				case 3:
				{
					List<ItemKey> itemKeyList5 = null;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKeyList5);
					int charId19 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId19);
					sbyte itemSourceType5 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemSourceType5);
					List<ItemDisplayData> returnValue26 = this.GetItemDisplayDataListOptional(itemKeyList5, charId19, itemSourceType5, false);
					result = Serializer.Serialize(returnValue26, returnDataPool);
					break;
				}
				case 4:
				{
					List<ItemKey> itemKeyList6 = null;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKeyList6);
					int charId20 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId20);
					sbyte itemSourceType6 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemSourceType6);
					bool merge = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref merge);
					List<ItemDisplayData> returnValue27 = this.GetItemDisplayDataListOptional(itemKeyList6, charId20, itemSourceType6, merge);
					result = Serializer.Serialize(returnValue27, returnDataPool);
					break;
				}
				default:
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				}
				break;
			case 31:
			{
				int argsCount31 = operation.ArgsCount;
				int num31 = argsCount31;
				if (num31 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<ItemKey> itemKeyList7 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKeyList7);
				List<SkillBookPageDisplayData> returnValue28 = this.GetSkillBookPageDisplayDataList(itemKeyList7);
				result = Serializer.Serialize(returnValue28, returnDataPool);
				break;
			}
			case 32:
			{
				int argsCount32 = operation.ArgsCount;
				int num32 = argsCount32;
				if (num32 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ItemKey returnValue29 = this.GetEmptyToolKey(context);
				result = Serializer.Serialize(returnValue29, returnDataPool);
				break;
			}
			case 33:
				switch (operation.ArgsCount)
				{
				case 1:
				{
					Inventory inventory = null;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref inventory);
					List<ItemDisplayData> returnValue30 = this.GetItemDisplayDataListOptionalFromInventory(inventory, -1, -1, false);
					result = Serializer.Serialize(returnValue30, returnDataPool);
					break;
				}
				case 2:
				{
					Inventory inventory2 = null;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref inventory2);
					int charId21 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId21);
					List<ItemDisplayData> returnValue31 = this.GetItemDisplayDataListOptionalFromInventory(inventory2, charId21, -1, false);
					result = Serializer.Serialize(returnValue31, returnDataPool);
					break;
				}
				case 3:
				{
					Inventory inventory3 = null;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref inventory3);
					int charId22 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId22);
					sbyte itemSourceType7 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemSourceType7);
					List<ItemDisplayData> returnValue32 = this.GetItemDisplayDataListOptionalFromInventory(inventory3, charId22, itemSourceType7, false);
					result = Serializer.Serialize(returnValue32, returnDataPool);
					break;
				}
				case 4:
				{
					Inventory inventory4 = null;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref inventory4);
					int charId23 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId23);
					sbyte itemSourceType8 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemSourceType8);
					bool merge2 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref merge2);
					List<ItemDisplayData> returnValue33 = this.GetItemDisplayDataListOptionalFromInventory(inventory4, charId23, itemSourceType8, merge2);
					result = Serializer.Serialize(returnValue33, returnDataPool);
					break;
				}
				default:
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				}
				break;
			case 34:
			{
				int argsCount33 = operation.ArgsCount;
				int num33 = argsCount33;
				if (num33 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool win2 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref win2);
				ItemKey[] taiwuCricketKeys = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref taiwuCricketKeys);
				short[] durabilityList = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref durabilityList);
				bool returnValue34 = this.SettlementCricketWager(context, win2, taiwuCricketKeys, durabilityList);
				result = Serializer.Serialize(returnValue34, returnDataPool);
				break;
			}
			case 35:
			{
				int argsCount34 = operation.ArgsCount;
				int num34 = argsCount34;
				if (num34 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int enemyId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref enemyId);
				this.GmCmd_StartCricketCombat(context, enemyId);
				result = -1;
				break;
			}
			case 36:
			{
				int argsCount35 = operation.ArgsCount;
				int num35 = argsCount35;
				if (num35 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool win3 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref win3);
				bool returnValue35 = this.SettlementCricketWagerByGiveUp(context, win3);
				result = Serializer.Serialize(returnValue35, returnDataPool);
				break;
			}
			case 37:
			{
				int argsCount36 = operation.ArgsCount;
				int num36 = argsCount36;
				if (num36 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ItemKey itemKey14 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey14);
				this.MakeCricketRebirth(context, itemKey14);
				result = -1;
				break;
			}
			case 38:
			{
				int argsCount37 = operation.ArgsCount;
				int num37 = argsCount37;
				if (num37 != 1)
				{
					if (num37 != 2)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					ItemKey itemKey15 = default(ItemKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey15);
					short targetDurability = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref targetDurability);
					int returnValue36 = this.GetRepairItemNeedResourceCount(itemKey15, targetDurability);
					result = Serializer.Serialize(returnValue36, returnDataPool);
				}
				else
				{
					ItemKey itemKey16 = default(ItemKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey16);
					int returnValue37 = this.GetRepairItemNeedResourceCount(itemKey16, -1);
					result = Serializer.Serialize(returnValue37, returnDataPool);
				}
				break;
			}
			case 39:
			{
				int argsCount38 = operation.ArgsCount;
				int num38 = argsCount38;
				if (num38 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ItemKey itemKey17 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey17);
				sbyte behaviorType2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref behaviorType2);
				List<sbyte> directions2 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref directions2);
				bool returnValue38 = this.SetCombatSkillBookPage(context, itemKey17, behaviorType2, directions2);
				result = Serializer.Serialize(returnValue38, returnDataPool);
				break;
			}
			case 40:
			{
				int argsCount39 = operation.ArgsCount;
				int num39 = argsCount39;
				if (num39 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId24 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId24);
				ItemKey weaponKey3 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref weaponKey3);
				int returnValue39 = this.GetWeaponPrepareFrame(charId24, weaponKey3);
				result = Serializer.Serialize(returnValue39, returnDataPool);
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

		// Token: 0x060050E8 RID: 20712 RVA: 0x002C6E50 File Offset: 0x002C5050
		public override void OnMonitorData(ushort dataId, ulong subId0, uint subId1, bool monitoring)
		{
			switch (dataId)
			{
			case 0:
				break;
			case 1:
				break;
			case 2:
				break;
			case 3:
				break;
			case 4:
				break;
			case 5:
				break;
			case 6:
				break;
			case 7:
				break;
			case 8:
				break;
			case 9:
				break;
			case 10:
				break;
			case 11:
				break;
			case 12:
				break;
			case 13:
				break;
			case 14:
				break;
			case 15:
				break;
			case 16:
				break;
			case 17:
				break;
			case 18:
				break;
			case 19:
				break;
			case 20:
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x060050E9 RID: 20713 RVA: 0x002C6F18 File Offset: 0x002C5118
		public override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
		{
			int result;
			switch (dataId)
			{
			case 0:
				result = this.CheckModified_Weapons((int)subId0, (ushort)subId1, dataPool);
				break;
			case 1:
				result = this.CheckModified_Armors((int)subId0, (ushort)subId1, dataPool);
				break;
			case 2:
				result = this.CheckModified_Accessories((int)subId0, (ushort)subId1, dataPool);
				break;
			case 3:
				result = this.CheckModified_Clothing((int)subId0, (ushort)subId1, dataPool);
				break;
			case 4:
				result = this.CheckModified_Carriers((int)subId0, (ushort)subId1, dataPool);
				break;
			case 5:
				result = this.CheckModified_Materials((int)subId0, (ushort)subId1, dataPool);
				break;
			case 6:
				result = this.CheckModified_CraftTools((int)subId0, (ushort)subId1, dataPool);
				break;
			case 7:
				result = this.CheckModified_Foods((int)subId0, (ushort)subId1, dataPool);
				break;
			case 8:
				result = this.CheckModified_Medicines((int)subId0, (ushort)subId1, dataPool);
				break;
			case 9:
				result = this.CheckModified_TeaWines((int)subId0, (ushort)subId1, dataPool);
				break;
			case 10:
				result = this.CheckModified_SkillBooks((int)subId0, (ushort)subId1, dataPool);
				break;
			case 11:
				result = this.CheckModified_Crickets((int)subId0, (ushort)subId1, dataPool);
				break;
			case 12:
				result = this.CheckModified_Misc((int)subId0, (ushort)subId1, dataPool);
				break;
			case 13:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 14:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 15:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 16:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 17:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 18:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 19:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 20:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x060050EA RID: 20714 RVA: 0x002C720C File Offset: 0x002C540C
		public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			switch (dataId)
			{
			case 0:
				this.ResetModifiedWrapper_Weapons((int)subId0, (ushort)subId1);
				break;
			case 1:
				this.ResetModifiedWrapper_Armors((int)subId0, (ushort)subId1);
				break;
			case 2:
				this.ResetModifiedWrapper_Accessories((int)subId0, (ushort)subId1);
				break;
			case 3:
				this.ResetModifiedWrapper_Clothing((int)subId0, (ushort)subId1);
				break;
			case 4:
				this.ResetModifiedWrapper_Carriers((int)subId0, (ushort)subId1);
				break;
			case 5:
				this.ResetModifiedWrapper_Materials((int)subId0, (ushort)subId1);
				break;
			case 6:
				this.ResetModifiedWrapper_CraftTools((int)subId0, (ushort)subId1);
				break;
			case 7:
				this.ResetModifiedWrapper_Foods((int)subId0, (ushort)subId1);
				break;
			case 8:
				this.ResetModifiedWrapper_Medicines((int)subId0, (ushort)subId1);
				break;
			case 9:
				this.ResetModifiedWrapper_TeaWines((int)subId0, (ushort)subId1);
				break;
			case 10:
				this.ResetModifiedWrapper_SkillBooks((int)subId0, (ushort)subId1);
				break;
			case 11:
				this.ResetModifiedWrapper_Crickets((int)subId0, (ushort)subId1);
				break;
			case 12:
				this.ResetModifiedWrapper_Misc((int)subId0, (ushort)subId1);
				break;
			case 13:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 14:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 15:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 16:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 17:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 18:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 19:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 20:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x060050EB RID: 20715 RVA: 0x002C74E4 File Offset: 0x002C56E4
		public override bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			bool result;
			switch (dataId)
			{
			case 0:
				result = this.IsModifiedWrapper_Weapons((int)subId0, (ushort)subId1);
				break;
			case 1:
				result = this.IsModifiedWrapper_Armors((int)subId0, (ushort)subId1);
				break;
			case 2:
				result = this.IsModifiedWrapper_Accessories((int)subId0, (ushort)subId1);
				break;
			case 3:
				result = this.IsModifiedWrapper_Clothing((int)subId0, (ushort)subId1);
				break;
			case 4:
				result = this.IsModifiedWrapper_Carriers((int)subId0, (ushort)subId1);
				break;
			case 5:
				result = this.IsModifiedWrapper_Materials((int)subId0, (ushort)subId1);
				break;
			case 6:
				result = this.IsModifiedWrapper_CraftTools((int)subId0, (ushort)subId1);
				break;
			case 7:
				result = this.IsModifiedWrapper_Foods((int)subId0, (ushort)subId1);
				break;
			case 8:
				result = this.IsModifiedWrapper_Medicines((int)subId0, (ushort)subId1);
				break;
			case 9:
				result = this.IsModifiedWrapper_TeaWines((int)subId0, (ushort)subId1);
				break;
			case 10:
				result = this.IsModifiedWrapper_SkillBooks((int)subId0, (ushort)subId1);
				break;
			case 11:
				result = this.IsModifiedWrapper_Crickets((int)subId0, (ushort)subId1);
				break;
			case 12:
				result = this.IsModifiedWrapper_Misc((int)subId0, (ushort)subId1);
				break;
			case 13:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to verify modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 14:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to verify modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 15:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to verify modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 16:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to verify modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 17:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to verify modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 18:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to verify modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 19:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to verify modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 20:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to verify modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x060050EC RID: 20716 RVA: 0x002C77BC File Offset: 0x002C59BC
		public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			switch (influence.TargetIndicator.DataId)
			{
			case 0:
			{
				bool flag = !unconditionallyInfluenceAll;
				if (flag)
				{
					List<BaseGameDataObject> influencedObjects = InfluenceChecker.InfluencedObjectsPool.Get();
					bool influenceAll = InfluenceChecker.GetScope<int, Weapon>(context, sourceObject, influence.Scope, this._weapons, influencedObjects);
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
						BaseGameDataDomain.InvalidateAllAndInfluencedCaches(ItemDomain.CacheInfluencesWeapons, this._dataStatesWeapons, influence, context);
					}
					influencedObjects.Clear();
					InfluenceChecker.InfluencedObjectsPool.Return(influencedObjects);
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(ItemDomain.CacheInfluencesWeapons, this._dataStatesWeapons, influence, context);
				}
				return;
			}
			case 1:
			{
				bool flag3 = !unconditionallyInfluenceAll;
				if (flag3)
				{
					List<BaseGameDataObject> influencedObjects2 = InfluenceChecker.InfluencedObjectsPool.Get();
					bool influenceAll2 = InfluenceChecker.GetScope<int, Armor>(context, sourceObject, influence.Scope, this._armors, influencedObjects2);
					bool flag4 = !influenceAll2;
					if (flag4)
					{
						int influencedObjectsCount2 = influencedObjects2.Count;
						for (int k = 0; k < influencedObjectsCount2; k++)
						{
							BaseGameDataObject targetObject2 = influencedObjects2[k];
							List<DataUid> targetUids2 = influence.TargetUids;
							int targetUidsCount2 = targetUids2.Count;
							for (int l = 0; l < targetUidsCount2; l++)
							{
								DataUid targetUid2 = targetUids2[l];
								targetObject2.InvalidateSelfAndInfluencedCache((ushort)targetUid2.SubId1, context);
							}
						}
					}
					else
					{
						BaseGameDataDomain.InvalidateAllAndInfluencedCaches(ItemDomain.CacheInfluencesArmors, this._dataStatesArmors, influence, context);
					}
					influencedObjects2.Clear();
					InfluenceChecker.InfluencedObjectsPool.Return(influencedObjects2);
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(ItemDomain.CacheInfluencesArmors, this._dataStatesArmors, influence, context);
				}
				return;
			}
			case 2:
			{
				bool flag5 = !unconditionallyInfluenceAll;
				if (flag5)
				{
					List<BaseGameDataObject> influencedObjects3 = InfluenceChecker.InfluencedObjectsPool.Get();
					bool influenceAll3 = InfluenceChecker.GetScope<int, Accessory>(context, sourceObject, influence.Scope, this._accessories, influencedObjects3);
					bool flag6 = !influenceAll3;
					if (flag6)
					{
						int influencedObjectsCount3 = influencedObjects3.Count;
						for (int m = 0; m < influencedObjectsCount3; m++)
						{
							BaseGameDataObject targetObject3 = influencedObjects3[m];
							List<DataUid> targetUids3 = influence.TargetUids;
							int targetUidsCount3 = targetUids3.Count;
							for (int n = 0; n < targetUidsCount3; n++)
							{
								DataUid targetUid3 = targetUids3[n];
								targetObject3.InvalidateSelfAndInfluencedCache((ushort)targetUid3.SubId1, context);
							}
						}
					}
					else
					{
						BaseGameDataDomain.InvalidateAllAndInfluencedCaches(ItemDomain.CacheInfluencesAccessories, this._dataStatesAccessories, influence, context);
					}
					influencedObjects3.Clear();
					InfluenceChecker.InfluencedObjectsPool.Return(influencedObjects3);
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(ItemDomain.CacheInfluencesAccessories, this._dataStatesAccessories, influence, context);
				}
				return;
			}
			case 3:
			{
				bool flag7 = !unconditionallyInfluenceAll;
				if (flag7)
				{
					List<BaseGameDataObject> influencedObjects4 = InfluenceChecker.InfluencedObjectsPool.Get();
					bool influenceAll4 = InfluenceChecker.GetScope<int, Clothing>(context, sourceObject, influence.Scope, this._clothing, influencedObjects4);
					bool flag8 = !influenceAll4;
					if (flag8)
					{
						int influencedObjectsCount4 = influencedObjects4.Count;
						for (int i2 = 0; i2 < influencedObjectsCount4; i2++)
						{
							BaseGameDataObject targetObject4 = influencedObjects4[i2];
							List<DataUid> targetUids4 = influence.TargetUids;
							int targetUidsCount4 = targetUids4.Count;
							for (int j2 = 0; j2 < targetUidsCount4; j2++)
							{
								DataUid targetUid4 = targetUids4[j2];
								targetObject4.InvalidateSelfAndInfluencedCache((ushort)targetUid4.SubId1, context);
							}
						}
					}
					else
					{
						BaseGameDataDomain.InvalidateAllAndInfluencedCaches(ItemDomain.CacheInfluencesClothing, this._dataStatesClothing, influence, context);
					}
					influencedObjects4.Clear();
					InfluenceChecker.InfluencedObjectsPool.Return(influencedObjects4);
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(ItemDomain.CacheInfluencesClothing, this._dataStatesClothing, influence, context);
				}
				return;
			}
			case 4:
			{
				bool flag9 = !unconditionallyInfluenceAll;
				if (flag9)
				{
					List<BaseGameDataObject> influencedObjects5 = InfluenceChecker.InfluencedObjectsPool.Get();
					bool influenceAll5 = InfluenceChecker.GetScope<int, Carrier>(context, sourceObject, influence.Scope, this._carriers, influencedObjects5);
					bool flag10 = !influenceAll5;
					if (flag10)
					{
						int influencedObjectsCount5 = influencedObjects5.Count;
						for (int i3 = 0; i3 < influencedObjectsCount5; i3++)
						{
							BaseGameDataObject targetObject5 = influencedObjects5[i3];
							List<DataUid> targetUids5 = influence.TargetUids;
							int targetUidsCount5 = targetUids5.Count;
							for (int j3 = 0; j3 < targetUidsCount5; j3++)
							{
								DataUid targetUid5 = targetUids5[j3];
								targetObject5.InvalidateSelfAndInfluencedCache((ushort)targetUid5.SubId1, context);
							}
						}
					}
					else
					{
						BaseGameDataDomain.InvalidateAllAndInfluencedCaches(ItemDomain.CacheInfluencesCarriers, this._dataStatesCarriers, influence, context);
					}
					influencedObjects5.Clear();
					InfluenceChecker.InfluencedObjectsPool.Return(influencedObjects5);
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(ItemDomain.CacheInfluencesCarriers, this._dataStatesCarriers, influence, context);
				}
				return;
			}
			case 5:
			{
				bool flag11 = !unconditionallyInfluenceAll;
				if (flag11)
				{
					List<BaseGameDataObject> influencedObjects6 = InfluenceChecker.InfluencedObjectsPool.Get();
					bool influenceAll6 = InfluenceChecker.GetScope<int, Material>(context, sourceObject, influence.Scope, this._materials, influencedObjects6);
					bool flag12 = !influenceAll6;
					if (flag12)
					{
						int influencedObjectsCount6 = influencedObjects6.Count;
						for (int i4 = 0; i4 < influencedObjectsCount6; i4++)
						{
							BaseGameDataObject targetObject6 = influencedObjects6[i4];
							List<DataUid> targetUids6 = influence.TargetUids;
							int targetUidsCount6 = targetUids6.Count;
							for (int j4 = 0; j4 < targetUidsCount6; j4++)
							{
								DataUid targetUid6 = targetUids6[j4];
								targetObject6.InvalidateSelfAndInfluencedCache((ushort)targetUid6.SubId1, context);
							}
						}
					}
					else
					{
						BaseGameDataDomain.InvalidateAllAndInfluencedCaches(ItemDomain.CacheInfluencesMaterials, this._dataStatesMaterials, influence, context);
					}
					influencedObjects6.Clear();
					InfluenceChecker.InfluencedObjectsPool.Return(influencedObjects6);
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(ItemDomain.CacheInfluencesMaterials, this._dataStatesMaterials, influence, context);
				}
				return;
			}
			case 6:
			{
				bool flag13 = !unconditionallyInfluenceAll;
				if (flag13)
				{
					List<BaseGameDataObject> influencedObjects7 = InfluenceChecker.InfluencedObjectsPool.Get();
					bool influenceAll7 = InfluenceChecker.GetScope<int, CraftTool>(context, sourceObject, influence.Scope, this._craftTools, influencedObjects7);
					bool flag14 = !influenceAll7;
					if (flag14)
					{
						int influencedObjectsCount7 = influencedObjects7.Count;
						for (int i5 = 0; i5 < influencedObjectsCount7; i5++)
						{
							BaseGameDataObject targetObject7 = influencedObjects7[i5];
							List<DataUid> targetUids7 = influence.TargetUids;
							int targetUidsCount7 = targetUids7.Count;
							for (int j5 = 0; j5 < targetUidsCount7; j5++)
							{
								DataUid targetUid7 = targetUids7[j5];
								targetObject7.InvalidateSelfAndInfluencedCache((ushort)targetUid7.SubId1, context);
							}
						}
					}
					else
					{
						BaseGameDataDomain.InvalidateAllAndInfluencedCaches(ItemDomain.CacheInfluencesCraftTools, this._dataStatesCraftTools, influence, context);
					}
					influencedObjects7.Clear();
					InfluenceChecker.InfluencedObjectsPool.Return(influencedObjects7);
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(ItemDomain.CacheInfluencesCraftTools, this._dataStatesCraftTools, influence, context);
				}
				return;
			}
			case 7:
			{
				bool flag15 = !unconditionallyInfluenceAll;
				if (flag15)
				{
					List<BaseGameDataObject> influencedObjects8 = InfluenceChecker.InfluencedObjectsPool.Get();
					bool influenceAll8 = InfluenceChecker.GetScope<int, Food>(context, sourceObject, influence.Scope, this._foods, influencedObjects8);
					bool flag16 = !influenceAll8;
					if (flag16)
					{
						int influencedObjectsCount8 = influencedObjects8.Count;
						for (int i6 = 0; i6 < influencedObjectsCount8; i6++)
						{
							BaseGameDataObject targetObject8 = influencedObjects8[i6];
							List<DataUid> targetUids8 = influence.TargetUids;
							int targetUidsCount8 = targetUids8.Count;
							for (int j6 = 0; j6 < targetUidsCount8; j6++)
							{
								DataUid targetUid8 = targetUids8[j6];
								targetObject8.InvalidateSelfAndInfluencedCache((ushort)targetUid8.SubId1, context);
							}
						}
					}
					else
					{
						BaseGameDataDomain.InvalidateAllAndInfluencedCaches(ItemDomain.CacheInfluencesFoods, this._dataStatesFoods, influence, context);
					}
					influencedObjects8.Clear();
					InfluenceChecker.InfluencedObjectsPool.Return(influencedObjects8);
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(ItemDomain.CacheInfluencesFoods, this._dataStatesFoods, influence, context);
				}
				return;
			}
			case 8:
			{
				bool flag17 = !unconditionallyInfluenceAll;
				if (flag17)
				{
					List<BaseGameDataObject> influencedObjects9 = InfluenceChecker.InfluencedObjectsPool.Get();
					bool influenceAll9 = InfluenceChecker.GetScope<int, Medicine>(context, sourceObject, influence.Scope, this._medicines, influencedObjects9);
					bool flag18 = !influenceAll9;
					if (flag18)
					{
						int influencedObjectsCount9 = influencedObjects9.Count;
						for (int i7 = 0; i7 < influencedObjectsCount9; i7++)
						{
							BaseGameDataObject targetObject9 = influencedObjects9[i7];
							List<DataUid> targetUids9 = influence.TargetUids;
							int targetUidsCount9 = targetUids9.Count;
							for (int j7 = 0; j7 < targetUidsCount9; j7++)
							{
								DataUid targetUid9 = targetUids9[j7];
								targetObject9.InvalidateSelfAndInfluencedCache((ushort)targetUid9.SubId1, context);
							}
						}
					}
					else
					{
						BaseGameDataDomain.InvalidateAllAndInfluencedCaches(ItemDomain.CacheInfluencesMedicines, this._dataStatesMedicines, influence, context);
					}
					influencedObjects9.Clear();
					InfluenceChecker.InfluencedObjectsPool.Return(influencedObjects9);
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(ItemDomain.CacheInfluencesMedicines, this._dataStatesMedicines, influence, context);
				}
				return;
			}
			case 9:
			{
				bool flag19 = !unconditionallyInfluenceAll;
				if (flag19)
				{
					List<BaseGameDataObject> influencedObjects10 = InfluenceChecker.InfluencedObjectsPool.Get();
					bool influenceAll10 = InfluenceChecker.GetScope<int, TeaWine>(context, sourceObject, influence.Scope, this._teaWines, influencedObjects10);
					bool flag20 = !influenceAll10;
					if (flag20)
					{
						int influencedObjectsCount10 = influencedObjects10.Count;
						for (int i8 = 0; i8 < influencedObjectsCount10; i8++)
						{
							BaseGameDataObject targetObject10 = influencedObjects10[i8];
							List<DataUid> targetUids10 = influence.TargetUids;
							int targetUidsCount10 = targetUids10.Count;
							for (int j8 = 0; j8 < targetUidsCount10; j8++)
							{
								DataUid targetUid10 = targetUids10[j8];
								targetObject10.InvalidateSelfAndInfluencedCache((ushort)targetUid10.SubId1, context);
							}
						}
					}
					else
					{
						BaseGameDataDomain.InvalidateAllAndInfluencedCaches(ItemDomain.CacheInfluencesTeaWines, this._dataStatesTeaWines, influence, context);
					}
					influencedObjects10.Clear();
					InfluenceChecker.InfluencedObjectsPool.Return(influencedObjects10);
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(ItemDomain.CacheInfluencesTeaWines, this._dataStatesTeaWines, influence, context);
				}
				return;
			}
			case 10:
			{
				bool flag21 = !unconditionallyInfluenceAll;
				if (flag21)
				{
					List<BaseGameDataObject> influencedObjects11 = InfluenceChecker.InfluencedObjectsPool.Get();
					bool influenceAll11 = InfluenceChecker.GetScope<int, SkillBook>(context, sourceObject, influence.Scope, this._skillBooks, influencedObjects11);
					bool flag22 = !influenceAll11;
					if (flag22)
					{
						int influencedObjectsCount11 = influencedObjects11.Count;
						for (int i9 = 0; i9 < influencedObjectsCount11; i9++)
						{
							BaseGameDataObject targetObject11 = influencedObjects11[i9];
							List<DataUid> targetUids11 = influence.TargetUids;
							int targetUidsCount11 = targetUids11.Count;
							for (int j9 = 0; j9 < targetUidsCount11; j9++)
							{
								DataUid targetUid11 = targetUids11[j9];
								targetObject11.InvalidateSelfAndInfluencedCache((ushort)targetUid11.SubId1, context);
							}
						}
					}
					else
					{
						BaseGameDataDomain.InvalidateAllAndInfluencedCaches(ItemDomain.CacheInfluencesSkillBooks, this._dataStatesSkillBooks, influence, context);
					}
					influencedObjects11.Clear();
					InfluenceChecker.InfluencedObjectsPool.Return(influencedObjects11);
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(ItemDomain.CacheInfluencesSkillBooks, this._dataStatesSkillBooks, influence, context);
				}
				return;
			}
			case 11:
			{
				bool flag23 = !unconditionallyInfluenceAll;
				if (flag23)
				{
					List<BaseGameDataObject> influencedObjects12 = InfluenceChecker.InfluencedObjectsPool.Get();
					bool influenceAll12 = InfluenceChecker.GetScope<int, Cricket>(context, sourceObject, influence.Scope, this._crickets, influencedObjects12);
					bool flag24 = !influenceAll12;
					if (flag24)
					{
						int influencedObjectsCount12 = influencedObjects12.Count;
						for (int i10 = 0; i10 < influencedObjectsCount12; i10++)
						{
							BaseGameDataObject targetObject12 = influencedObjects12[i10];
							List<DataUid> targetUids12 = influence.TargetUids;
							int targetUidsCount12 = targetUids12.Count;
							for (int j10 = 0; j10 < targetUidsCount12; j10++)
							{
								DataUid targetUid12 = targetUids12[j10];
								targetObject12.InvalidateSelfAndInfluencedCache((ushort)targetUid12.SubId1, context);
							}
						}
					}
					else
					{
						BaseGameDataDomain.InvalidateAllAndInfluencedCaches(ItemDomain.CacheInfluencesCrickets, this._dataStatesCrickets, influence, context);
					}
					influencedObjects12.Clear();
					InfluenceChecker.InfluencedObjectsPool.Return(influencedObjects12);
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(ItemDomain.CacheInfluencesCrickets, this._dataStatesCrickets, influence, context);
				}
				return;
			}
			case 12:
			{
				bool flag25 = !unconditionallyInfluenceAll;
				if (flag25)
				{
					List<BaseGameDataObject> influencedObjects13 = InfluenceChecker.InfluencedObjectsPool.Get();
					bool influenceAll13 = InfluenceChecker.GetScope<int, Misc>(context, sourceObject, influence.Scope, this._misc, influencedObjects13);
					bool flag26 = !influenceAll13;
					if (flag26)
					{
						int influencedObjectsCount13 = influencedObjects13.Count;
						for (int i11 = 0; i11 < influencedObjectsCount13; i11++)
						{
							BaseGameDataObject targetObject13 = influencedObjects13[i11];
							List<DataUid> targetUids13 = influence.TargetUids;
							int targetUidsCount13 = targetUids13.Count;
							for (int j11 = 0; j11 < targetUidsCount13; j11++)
							{
								DataUid targetUid13 = targetUids13[j11];
								targetObject13.InvalidateSelfAndInfluencedCache((ushort)targetUid13.SubId1, context);
							}
						}
					}
					else
					{
						BaseGameDataDomain.InvalidateAllAndInfluencedCaches(ItemDomain.CacheInfluencesMisc, this._dataStatesMisc, influence, context);
					}
					influencedObjects13.Clear();
					InfluenceChecker.InfluencedObjectsPool.Return(influencedObjects13);
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(ItemDomain.CacheInfluencesMisc, this._dataStatesMisc, influence, context);
				}
				return;
			}
			case 13:
				break;
			case 14:
				break;
			case 15:
				break;
			case 16:
				break;
			case 17:
				break;
			case 18:
				break;
			case 19:
				break;
			case 20:
				break;
			default:
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(influence.TargetIndicator.DataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Cannot invalidate cache state of non-cache data ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(influence.TargetIndicator.DataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060050ED RID: 20717 RVA: 0x002C8544 File Offset: 0x002C6744
		public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
		{
			switch (operation.DataId)
			{
			case 0:
				ResponseProcessor.ProcessDynamicObjectCollection<int, Weapon>(operation, pResult, this._weapons);
				break;
			case 1:
				ResponseProcessor.ProcessFixedObjectCollection<int, Armor>(operation, pResult, this._armors);
				break;
			case 2:
				ResponseProcessor.ProcessFixedObjectCollection<int, Accessory>(operation, pResult, this._accessories);
				break;
			case 3:
				ResponseProcessor.ProcessFixedObjectCollection<int, Clothing>(operation, pResult, this._clothing);
				break;
			case 4:
				ResponseProcessor.ProcessFixedObjectCollection<int, Carrier>(operation, pResult, this._carriers);
				break;
			case 5:
				ResponseProcessor.ProcessFixedObjectCollection<int, Material>(operation, pResult, this._materials);
				break;
			case 6:
				ResponseProcessor.ProcessFixedObjectCollection<int, CraftTool>(operation, pResult, this._craftTools);
				break;
			case 7:
				ResponseProcessor.ProcessFixedObjectCollection<int, Food>(operation, pResult, this._foods);
				break;
			case 8:
				ResponseProcessor.ProcessFixedObjectCollection<int, Medicine>(operation, pResult, this._medicines);
				break;
			case 9:
				ResponseProcessor.ProcessFixedObjectCollection<int, TeaWine>(operation, pResult, this._teaWines);
				break;
			case 10:
				ResponseProcessor.ProcessFixedObjectCollection<int, SkillBook>(operation, pResult, this._skillBooks);
				break;
			case 11:
				ResponseProcessor.ProcessFixedObjectCollection<int, Cricket>(operation, pResult, this._crickets);
				break;
			case 12:
				ResponseProcessor.ProcessFixedObjectCollection<int, Misc>(operation, pResult, this._misc);
				break;
			case 13:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single<int>(operation, pResult, ref this._nextItemId);
				break;
			case 14:
				ResponseProcessor.ProcessSingleValueCollection_BasicType_Fixed_Value<TemplateKey, int>(operation, pResult, this._stackableItems);
				break;
			case 15:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Fixed_Value<int, PoisonEffects>(operation, pResult, this._poisonItems, 21);
				break;
			case 16:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Fixed_Value<int, RefiningEffects>(operation, pResult, this._refinedItems, 10);
				break;
			case 17:
				ResponseProcessor.ProcessSingleValue_CustomType_Fixed_Value_Single<ItemKey>(operation, pResult, ref this._emptyHandKey);
				break;
			case 18:
				ResponseProcessor.ProcessSingleValue_CustomType_Fixed_Value_Single<ItemKey>(operation, pResult, ref this._branchKey);
				break;
			case 19:
				ResponseProcessor.ProcessSingleValue_CustomType_Fixed_Value_Single<ItemKey>(operation, pResult, ref this._stoneKey);
				break;
			case 20:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot process archive response of non-archive data ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.DataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.DataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
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
						DomainManager.Global.CompleteLoading(6);
					}
				}
			}
		}

		// Token: 0x060050EE RID: 20718 RVA: 0x002C8810 File Offset: 0x002C6A10
		private void InitializeInternalDataOfCollections()
		{
			foreach (KeyValuePair<int, Weapon> entry in this._weapons)
			{
				Weapon instance = entry.Value;
				instance.CollectionHelperData = this.HelperDataWeapons;
				instance.DataStatesOffset = this._dataStatesWeapons.Create();
			}
			foreach (KeyValuePair<int, Armor> entry2 in this._armors)
			{
				Armor instance2 = entry2.Value;
				instance2.CollectionHelperData = this.HelperDataArmors;
				instance2.DataStatesOffset = this._dataStatesArmors.Create();
			}
			foreach (KeyValuePair<int, Accessory> entry3 in this._accessories)
			{
				Accessory instance3 = entry3.Value;
				instance3.CollectionHelperData = this.HelperDataAccessories;
				instance3.DataStatesOffset = this._dataStatesAccessories.Create();
			}
			foreach (KeyValuePair<int, Clothing> entry4 in this._clothing)
			{
				Clothing instance4 = entry4.Value;
				instance4.CollectionHelperData = this.HelperDataClothing;
				instance4.DataStatesOffset = this._dataStatesClothing.Create();
			}
			foreach (KeyValuePair<int, Carrier> entry5 in this._carriers)
			{
				Carrier instance5 = entry5.Value;
				instance5.CollectionHelperData = this.HelperDataCarriers;
				instance5.DataStatesOffset = this._dataStatesCarriers.Create();
			}
			foreach (KeyValuePair<int, Material> entry6 in this._materials)
			{
				Material instance6 = entry6.Value;
				instance6.CollectionHelperData = this.HelperDataMaterials;
				instance6.DataStatesOffset = this._dataStatesMaterials.Create();
			}
			foreach (KeyValuePair<int, CraftTool> entry7 in this._craftTools)
			{
				CraftTool instance7 = entry7.Value;
				instance7.CollectionHelperData = this.HelperDataCraftTools;
				instance7.DataStatesOffset = this._dataStatesCraftTools.Create();
			}
			foreach (KeyValuePair<int, Food> entry8 in this._foods)
			{
				Food instance8 = entry8.Value;
				instance8.CollectionHelperData = this.HelperDataFoods;
				instance8.DataStatesOffset = this._dataStatesFoods.Create();
			}
			foreach (KeyValuePair<int, Medicine> entry9 in this._medicines)
			{
				Medicine instance9 = entry9.Value;
				instance9.CollectionHelperData = this.HelperDataMedicines;
				instance9.DataStatesOffset = this._dataStatesMedicines.Create();
			}
			foreach (KeyValuePair<int, TeaWine> entry10 in this._teaWines)
			{
				TeaWine instance10 = entry10.Value;
				instance10.CollectionHelperData = this.HelperDataTeaWines;
				instance10.DataStatesOffset = this._dataStatesTeaWines.Create();
			}
			foreach (KeyValuePair<int, SkillBook> entry11 in this._skillBooks)
			{
				SkillBook instance11 = entry11.Value;
				instance11.CollectionHelperData = this.HelperDataSkillBooks;
				instance11.DataStatesOffset = this._dataStatesSkillBooks.Create();
			}
			foreach (KeyValuePair<int, Cricket> entry12 in this._crickets)
			{
				Cricket instance12 = entry12.Value;
				instance12.CollectionHelperData = this.HelperDataCrickets;
				instance12.DataStatesOffset = this._dataStatesCrickets.Create();
			}
			foreach (KeyValuePair<int, Misc> entry13 in this._misc)
			{
				Misc instance13 = entry13.Value;
				instance13.CollectionHelperData = this.HelperDataMisc;
				instance13.DataStatesOffset = this._dataStatesMisc.Create();
			}
		}

		// Token: 0x060050F0 RID: 20720 RVA: 0x002C8E3E File Offset: 0x002C703E
		[CompilerGenerated]
		internal static sbyte <FillInventoryCricket>g__GetGrade|17_0(ItemDisplayData data)
		{
			return ItemTemplateHelper.GetCricketGrade(data.CricketColorId, data.CricketPartId);
		}

		// Token: 0x060050F1 RID: 20721 RVA: 0x002C8E54 File Offset: 0x002C7054
		[CompilerGenerated]
		internal static void <GetRepairableItems>g__Do|116_0(ItemKey itemKey, ref ItemDomain.<>c__DisplayClass116_0 A_1)
		{
			bool flag = !itemKey.IsValid();
			if (!flag)
			{
				bool flag2 = !DomainManager.Item.CheckItemNeedRepair(itemKey);
				if (!flag2)
				{
					sbyte itemRequiredLifeSkillType = ItemTemplateHelper.GetCraftRequiredLifeSkillType(itemKey.ItemType, itemKey.TemplateId);
					bool flag3 = !A_1.toolRequiredLifeSkillTypes.Contains(itemRequiredLifeSkillType);
					if (!flag3)
					{
						A_1.ret.Add(itemKey);
					}
				}
			}
		}

		// Token: 0x060050F2 RID: 20722 RVA: 0x002C8EBC File Offset: 0x002C70BC
		[CompilerGenerated]
		internal static void <GetDisassemblableItems>g__Do|117_0(ItemKey itemKey, ref ItemDomain.<>c__DisplayClass117_0 A_1)
		{
			bool flag = !itemKey.IsValid();
			if (!flag)
			{
				bool flag2 = !ItemTemplateHelper.GetCanDisassemble(itemKey.ItemType, itemKey.TemplateId);
				if (!flag2)
				{
					sbyte itemRequiredLifeSkillType = ItemTemplateHelper.GetCraftRequiredLifeSkillType(itemKey.ItemType, itemKey.TemplateId);
					bool flag3 = !A_1.toolRequiredLifeSkillTypes.Contains(itemRequiredLifeSkillType);
					if (!flag3)
					{
						ItemBase item = DomainManager.Item.GetBaseItem(itemKey);
						A_1.ret.Add(item.GetItemKey());
					}
				}
			}
		}

		// Token: 0x060050F3 RID: 20723 RVA: 0x002C8F3C File Offset: 0x002C713C
		[CompilerGenerated]
		internal static bool <IdentifyPoisons>g__CheckItem|187_4(IReadOnlyDictionary<ItemKey, int> items, out ItemKey testingNeedleKey)
		{
			testingNeedleKey = items.FirstOrDefault((KeyValuePair<ItemKey, int> p) => p.Key.ItemType == 12 && p.Key.TemplateId == 91).Key;
			int amount;
			bool flag = !testingNeedleKey.IsValid() || !items.TryGetValue(testingNeedleKey, out amount) || amount <= 0;
			return !flag;
		}

		// Token: 0x040015AC RID: 5548
		private static readonly int WagerValueUnit = (int)(GlobalConfig.UnitsOfResourceTransfer[0] * (short)GlobalConfig.ResourcesWorth[0]);

		// Token: 0x040015AD RID: 5549
		private sbyte _minCricketGrade = 0;

		// Token: 0x040015AE RID: 5550
		private sbyte _maxCricketGrade = 8;

		// Token: 0x040015AF RID: 5551
		private bool _onlyNoInjuryCricket;

		// Token: 0x040015B0 RID: 5552
		private int _cricketBattleEnemyId;

		// Token: 0x040015B1 RID: 5553
		private readonly List<ItemKey> _cricketBattleEnemyCrickets = new List<ItemKey>();

		// Token: 0x040015B2 RID: 5554
		private Wager _cricketBattleSelfWager;

		// Token: 0x040015B3 RID: 5555
		private Wager _cricketBattleEnemyWager;

		// Token: 0x040015B4 RID: 5556
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x040015B5 RID: 5557
		[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
		private readonly Dictionary<int, Weapon> _weapons;

		// Token: 0x040015B6 RID: 5558
		[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
		private readonly Dictionary<int, Armor> _armors;

		// Token: 0x040015B7 RID: 5559
		[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
		private readonly Dictionary<int, Accessory> _accessories;

		// Token: 0x040015B8 RID: 5560
		[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
		private readonly Dictionary<int, Clothing> _clothing;

		// Token: 0x040015B9 RID: 5561
		[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
		private readonly Dictionary<int, Carrier> _carriers;

		// Token: 0x040015BA RID: 5562
		[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
		private readonly Dictionary<int, Material> _materials;

		// Token: 0x040015BB RID: 5563
		[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
		private readonly Dictionary<int, CraftTool> _craftTools;

		// Token: 0x040015BC RID: 5564
		[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
		private readonly Dictionary<int, Food> _foods;

		// Token: 0x040015BD RID: 5565
		[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
		private readonly Dictionary<int, Medicine> _medicines;

		// Token: 0x040015BE RID: 5566
		[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
		private readonly Dictionary<int, TeaWine> _teaWines;

		// Token: 0x040015BF RID: 5567
		[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
		private readonly Dictionary<int, SkillBook> _skillBooks;

		// Token: 0x040015C0 RID: 5568
		[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
		private readonly Dictionary<int, Cricket> _crickets;

		// Token: 0x040015C1 RID: 5569
		[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
		private readonly Dictionary<int, Misc> _misc;

		// Token: 0x040015C2 RID: 5570
		[DomainData(DomainDataType.SingleValue, true, false, false, false)]
		private int _nextItemId;

		// Token: 0x040015C3 RID: 5571
		[DomainData(DomainDataType.SingleValueCollection, true, false, false, false)]
		private readonly Dictionary<TemplateKey, int> _stackableItems;

		// Token: 0x040015C4 RID: 5572
		[Obsolete("Now only for data fix. Use ExtraDomain.PoisonEffects instead")]
		[DomainData(DomainDataType.SingleValueCollection, true, false, false, false)]
		private readonly Dictionary<int, PoisonEffects> _poisonItems;

		// Token: 0x040015C5 RID: 5573
		[DomainData(DomainDataType.SingleValueCollection, true, false, false, false)]
		private readonly Dictionary<int, RefiningEffects> _refinedItems;

		// Token: 0x040015C6 RID: 5574
		[DomainData(DomainDataType.SingleValueCollection, false, false, false, false)]
		private readonly Dictionary<int, GameData.Utilities.ShortList> _externEquipmentEffects;

		// Token: 0x040015C7 RID: 5575
		private readonly HashSet<ItemKey> _trackedSpecialItems = new HashSet<ItemKey>();

		// Token: 0x040015C8 RID: 5576
		[DomainData(DomainDataType.SingleValue, true, false, false, false)]
		private ItemKey _emptyHandKey;

		// Token: 0x040015C9 RID: 5577
		[DomainData(DomainDataType.SingleValue, true, false, false, false)]
		private ItemKey _branchKey;

		// Token: 0x040015CA RID: 5578
		[DomainData(DomainDataType.SingleValue, true, false, false, false)]
		private ItemKey _stoneKey;

		// Token: 0x040015CB RID: 5579
		private static List<short>[] _categorizedEquipmentEffects;

		// Token: 0x040015CC RID: 5580
		private static Dictionary<short, List<short>>[] _categorizedItemTemplates;

		// Token: 0x040015CD RID: 5581
		private static List<TemplateKey>[] _skillBreakPlateBonusEffects;

		// Token: 0x040015CE RID: 5582
		public const int ItemGradeSatisfactionOffset = -2;

		// Token: 0x040015CF RID: 5583
		public const int NormalPageBaseCostExp = 20;

		// Token: 0x040015D0 RID: 5584
		public const int OutlinePageBaseCostExp = 60;

		// Token: 0x040015D1 RID: 5585
		private readonly HashSet<ItemKey> _newDeadCrickets = new HashSet<ItemKey>();

		// Token: 0x040015D2 RID: 5586
		private static short[] _wugTemplateIds;

		// Token: 0x040015D3 RID: 5587
		private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[21][];

		// Token: 0x040015D4 RID: 5588
		private static readonly DataInfluence[][] CacheInfluencesWeapons = new DataInfluence[13][];

		// Token: 0x040015D5 RID: 5589
		private readonly ObjectCollectionDataStates _dataStatesWeapons = new ObjectCollectionDataStates(13, 0);

		// Token: 0x040015D6 RID: 5590
		public readonly ObjectCollectionHelperData HelperDataWeapons;

		// Token: 0x040015D7 RID: 5591
		private static readonly DataInfluence[][] CacheInfluencesArmors = new DataInfluence[13][];

		// Token: 0x040015D8 RID: 5592
		private readonly ObjectCollectionDataStates _dataStatesArmors = new ObjectCollectionDataStates(13, 0);

		// Token: 0x040015D9 RID: 5593
		public readonly ObjectCollectionHelperData HelperDataArmors;

		// Token: 0x040015DA RID: 5594
		private static readonly DataInfluence[][] CacheInfluencesAccessories = new DataInfluence[8][];

		// Token: 0x040015DB RID: 5595
		private readonly ObjectCollectionDataStates _dataStatesAccessories = new ObjectCollectionDataStates(8, 0);

		// Token: 0x040015DC RID: 5596
		public readonly ObjectCollectionHelperData HelperDataAccessories;

		// Token: 0x040015DD RID: 5597
		private static readonly DataInfluence[][] CacheInfluencesClothing = new DataInfluence[9][];

		// Token: 0x040015DE RID: 5598
		private readonly ObjectCollectionDataStates _dataStatesClothing = new ObjectCollectionDataStates(9, 0);

		// Token: 0x040015DF RID: 5599
		public readonly ObjectCollectionHelperData HelperDataClothing;

		// Token: 0x040015E0 RID: 5600
		private static readonly DataInfluence[][] CacheInfluencesCarriers = new DataInfluence[8][];

		// Token: 0x040015E1 RID: 5601
		private readonly ObjectCollectionDataStates _dataStatesCarriers = new ObjectCollectionDataStates(8, 0);

		// Token: 0x040015E2 RID: 5602
		public readonly ObjectCollectionHelperData HelperDataCarriers;

		// Token: 0x040015E3 RID: 5603
		private static readonly DataInfluence[][] CacheInfluencesMaterials = new DataInfluence[5][];

		// Token: 0x040015E4 RID: 5604
		private readonly ObjectCollectionDataStates _dataStatesMaterials = new ObjectCollectionDataStates(5, 0);

		// Token: 0x040015E5 RID: 5605
		public readonly ObjectCollectionHelperData HelperDataMaterials;

		// Token: 0x040015E6 RID: 5606
		private static readonly DataInfluence[][] CacheInfluencesCraftTools = new DataInfluence[5][];

		// Token: 0x040015E7 RID: 5607
		private readonly ObjectCollectionDataStates _dataStatesCraftTools = new ObjectCollectionDataStates(5, 0);

		// Token: 0x040015E8 RID: 5608
		public readonly ObjectCollectionHelperData HelperDataCraftTools;

		// Token: 0x040015E9 RID: 5609
		private static readonly DataInfluence[][] CacheInfluencesFoods = new DataInfluence[5][];

		// Token: 0x040015EA RID: 5610
		private readonly ObjectCollectionDataStates _dataStatesFoods = new ObjectCollectionDataStates(5, 0);

		// Token: 0x040015EB RID: 5611
		public readonly ObjectCollectionHelperData HelperDataFoods;

		// Token: 0x040015EC RID: 5612
		private static readonly DataInfluence[][] CacheInfluencesMedicines = new DataInfluence[5][];

		// Token: 0x040015ED RID: 5613
		private readonly ObjectCollectionDataStates _dataStatesMedicines = new ObjectCollectionDataStates(5, 0);

		// Token: 0x040015EE RID: 5614
		public readonly ObjectCollectionHelperData HelperDataMedicines;

		// Token: 0x040015EF RID: 5615
		private static readonly DataInfluence[][] CacheInfluencesTeaWines = new DataInfluence[5][];

		// Token: 0x040015F0 RID: 5616
		private readonly ObjectCollectionDataStates _dataStatesTeaWines = new ObjectCollectionDataStates(5, 0);

		// Token: 0x040015F1 RID: 5617
		public readonly ObjectCollectionHelperData HelperDataTeaWines;

		// Token: 0x040015F2 RID: 5618
		private static readonly DataInfluence[][] CacheInfluencesSkillBooks = new DataInfluence[7][];

		// Token: 0x040015F3 RID: 5619
		private readonly ObjectCollectionDataStates _dataStatesSkillBooks = new ObjectCollectionDataStates(7, 0);

		// Token: 0x040015F4 RID: 5620
		public readonly ObjectCollectionHelperData HelperDataSkillBooks;

		// Token: 0x040015F5 RID: 5621
		private static readonly DataInfluence[][] CacheInfluencesCrickets = new DataInfluence[13][];

		// Token: 0x040015F6 RID: 5622
		private readonly ObjectCollectionDataStates _dataStatesCrickets = new ObjectCollectionDataStates(13, 0);

		// Token: 0x040015F7 RID: 5623
		public readonly ObjectCollectionHelperData HelperDataCrickets;

		// Token: 0x040015F8 RID: 5624
		private static readonly DataInfluence[][] CacheInfluencesMisc = new DataInfluence[5][];

		// Token: 0x040015F9 RID: 5625
		private readonly ObjectCollectionDataStates _dataStatesMisc = new ObjectCollectionDataStates(5, 0);

		// Token: 0x040015FA RID: 5626
		public readonly ObjectCollectionHelperData HelperDataMisc;

		// Token: 0x040015FB RID: 5627
		private Queue<uint> _pendingLoadingOperationIds;
	}
}
