using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.TaiwuEvent.EventManager;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions.CustomActions
{
	// Token: 0x0200009B RID: 155
	[SerializableGameData(NotForDisplayModule = true)]
	public class LegendaryBookMonthlyAction : MonthlyActionBase, IDynamicAction, ISerializableGameData
	{
		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060019EC RID: 6636 RVA: 0x00172AE2 File Offset: 0x00170CE2
		public short DynamicActionType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060019ED RID: 6637 RVA: 0x00172AE5 File Offset: 0x00170CE5
		public LegendaryBookMonthlyAction()
		{
			this.Key = MonthlyActionKey.Invalid;
			this.ParticipatingCharacterSets = new List<CharacterSet>();
			this.Location = Location.Invalid;
			this.BookType = -1;
			this.BookAppearType = -1;
			this.PrevOwnerId = -1;
		}

		// Token: 0x060019EE RID: 6638 RVA: 0x00172B28 File Offset: 0x00170D28
		public override void TriggerAction()
		{
			bool flag = !this.Location.IsValid() || this.BookType < 0;
			if (!flag)
			{
				bool flag2 = !DomainManager.LegendaryBook.GetLegendaryBookItem(this.BookType).IsValid();
				if (!flag2)
				{
					bool flag3 = DomainManager.LegendaryBook.GetOwner(this.BookType) >= 0;
					if (!flag3)
					{
						DomainManager.LegendaryBook.RegisterLegendaryBookMonthlyAction(this);
						DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
						short adventureId = AiHelper.LegendaryBookRelatedConstants.LegendaryBookAdventures[(int)this.BookType];
						bool flag4 = DomainManager.Adventure.TryCreateAdventureSite(context, this.Location.AreaId, this.Location.BlockId, adventureId, this.Key);
						if (flag4)
						{
							sbyte firstMonthDelay = DomainManager.Extra.GetFirstLegendaryBookDelay();
							bool isFirst = firstMonthDelay == 0;
							DomainManager.Extra.SetFirstLegendaryBookDelay(context, -1);
							MonthlyEventActionsManager.NewlyTriggered++;
							this.State = 1;
							this.ActivateDelay = (isFirst ? 0 : ((sbyte)context.Random.Next(3, 10)));
							AdaptableLog.TagInfo("LegendaryBookMonthlyAction", "Legendary book adventure " + Adventure.Instance[adventureId].Name + " created at " + this.Location.ToString());
						}
						else
						{
							this.Location = Location.Invalid;
						}
					}
				}
			}
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x00172C90 File Offset: 0x00170E90
		public override void MonthlyHandler()
		{
			bool flag = this.State == 0;
			if (flag)
			{
				bool flag2 = !this.Location.IsValid();
				if (flag2)
				{
					DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
					short areaId = (short)context.Random.Next(45);
					MapBlockData blockData = DomainManager.Map.GetRandomMapBlockDataInAreaByFilters(context.Random, areaId, null, false);
					this.Location = blockData.GetLocation();
				}
				this.TriggerAction();
			}
			bool flag3 = this.State == 5 && this.Month > (int)this.ActivateDelay;
			if (flag3)
			{
				this.CallParticipateCharacters();
			}
			bool flag4 = this.State == 1 && this.Month >= (int)this.ActivateDelay;
			if (flag4)
			{
				this.Activate();
				this.State = 5;
			}
			bool flag5 = this.State != 0;
			if (flag5)
			{
				this.Month++;
			}
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x00172D80 File Offset: 0x00170F80
		public override void Activate()
		{
			short adventureId = AiHelper.LegendaryBookRelatedConstants.LegendaryBookAdventures[(int)this.BookType];
			AdaptableLog.TagInfo("LegendaryBookMonthlyAction", "Legendary book adventure " + Adventure.Instance[adventureId].Name + " activated at " + this.Location.ToString());
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			MonthlyEventActionsManager.NewlyActivated++;
			DomainManager.Adventure.ActivateAdventureSite(context, this.Location.AreaId, this.Location.BlockId);
			DomainManager.Map.EnsureBlockVisible(context, this.Location);
			ItemKey itemKey = DomainManager.LegendaryBook.GetLegendaryBookItem(this.BookType);
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
			switch (this.BookAppearType)
			{
			case 0:
				monthlyEventCollection.AddFightForNewLegendaryBook(this.Location, (ulong)itemKey);
				monthlyNotifications.AddFightForNewLegendaryBook(this.Location, itemKey.ItemType, itemKey.TemplateId);
				break;
			case 1:
				monthlyEventCollection.AddFightForLegendaryBookOwnerConsumed(this.PrevOwnerId, this.Location, (ulong)itemKey);
				monthlyNotifications.AddFightForLegendaryBookOwnerConsumed(this.PrevOwnerId, this.Location, itemKey.ItemType, itemKey.TemplateId);
				break;
			case 2:
				monthlyEventCollection.AddFightForLegendaryBookOwnerDie(this.PrevOwnerId, this.Location, (ulong)itemKey);
				monthlyNotifications.AddFightForLegendaryBookOwnerDie(this.PrevOwnerId, this.Location, itemKey.ItemType, itemKey.TemplateId);
				break;
			case 3:
				monthlyEventCollection.AddFightForLegendaryBookAbandoned(this.PrevOwnerId, this.Location, (ulong)itemKey);
				monthlyNotifications.AddFightForLegendaryBookAbandoned(this.PrevOwnerId, this.Location, itemKey.ItemType, itemKey.TemplateId);
				break;
			}
		}

		// Token: 0x060019F1 RID: 6641 RVA: 0x00172F54 File Offset: 0x00171154
		public override void Deactivate(bool isComplete)
		{
			short adventureId = AiHelper.LegendaryBookRelatedConstants.LegendaryBookAdventures[(int)this.BookType];
			AdaptableLog.TagInfo("LegendaryBookMonthlyAction", "Legendary book adventure " + Adventure.Instance[adventureId].Name + " removed at " + this.Location.ToString());
			DomainManager.LegendaryBook.UnregisterLegendaryBookMonthlyAction(this.BookType);
			bool flag = DomainManager.LegendaryBook.GetOwner(this.BookType) < 0 && !this.AssignRandomOwner();
			if (flag)
			{
				DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
				MapBlockData blockData = DomainManager.Map.GetRandomMapBlockDataInAreaByFilters(context.Random, this.Location.AreaId, null, false);
				short? num = (blockData != null) ? new short?(blockData.BlockId) : null;
				int? num2 = (num != null) ? new int?((int)num.GetValueOrDefault()) : null;
				int blockId = (int)this.Location.BlockId;
				bool flag2 = num2.GetValueOrDefault() == blockId & num2 != null;
				if (flag2)
				{
					blockData = null;
				}
				LegendaryBookMonthlyAction action = new LegendaryBookMonthlyAction
				{
					BookType = this.BookType,
					BookAppearType = this.BookAppearType,
					PrevOwnerId = this.PrevOwnerId,
					Location = ((blockData != null) ? blockData.GetLocation() : Location.Invalid)
				};
				DomainManager.TaiwuEvent.AddTempDynamicAction<LegendaryBookMonthlyAction>(context, action);
			}
			this.ClearCalledCharacters();
			this.State = 0;
			this.Month = 0;
			this.LastFinishDate = DomainManager.World.GetCurrDate();
			this.Location = Location.Invalid;
		}

		// Token: 0x060019F2 RID: 6642 RVA: 0x001730F4 File Offset: 0x001712F4
		private bool AssignRandomOwner()
		{
			List<GameData.Domains.Character.Character> charList = ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Get();
			charList.Clear();
			foreach (CharacterSet charSet in this.ParticipatingCharacterSets)
			{
				foreach (int participateCharId in charSet.GetCollection())
				{
					GameData.Domains.Character.Character character;
					bool flag = !DomainManager.Character.TryGetElement_Objects(participateCharId, out character);
					if (!flag)
					{
						bool flag2 = !character.GetLocation().Equals(this.Location);
						if (!flag2)
						{
							bool flag3 = character.IsCompletelyInfected();
							if (!flag3)
							{
								bool flag4 = character.GetLegendaryBookOwnerState() >= 3;
								if (!flag4)
								{
									bool flag5 = character.GetKidnapperId() >= 0;
									if (!flag5)
									{
										charList.Add(character);
									}
								}
							}
						}
					}
				}
			}
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			bool flag6 = charList.Count == 0;
			bool result;
			if (flag6)
			{
				ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(charList);
				AdaptableLog.TagInfo("LegendaryBookMonthlyAction", "Failed to assign random owner.");
				result = false;
			}
			else
			{
				GameData.Domains.Character.Character randomOwner = charList.GetRandom(context.Random);
				ItemKey itemKey = DomainManager.LegendaryBook.GetLegendaryBookItem(this.BookType);
				randomOwner.AddInventoryItem(context, itemKey, 1, false);
				ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(charList);
				MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
				monthlyNotifications.AddLegendaryBookAppeared(randomOwner.GetId(), this.Location, itemKey.ItemType, itemKey.TemplateId);
				LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
				int currDate = DomainManager.World.GetCurrDate();
				lifeRecordCollection.AddGainLegendaryBook(randomOwner.GetId(), currDate, this.Location, itemKey.ItemType, itemKey.TemplateId);
				result = true;
			}
			return result;
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x00173304 File Offset: 0x00171504
		public override MonthlyActionBase CreateCopy()
		{
			return Serializer.CreateCopy<LegendaryBookMonthlyAction>(this);
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x0017331C File Offset: 0x0017151C
		public override void CollectCalledCharacters(HashSet<int> calledCharacters)
		{
			foreach (CharacterSet charSet in this.ParticipatingCharacterSets)
			{
				calledCharacters.UnionWith(charSet.GetCollection());
			}
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x0017337C File Offset: 0x0017157C
		public override void FillEventArgBox(EventArgBox eventArgBox)
		{
			AdaptableLog.Info("Adding participating characters to adventure.");
			CharacterSet participateCharSet = (this.ParticipatingCharacterSets.Count > 0) ? this.ParticipatingCharacterSets[0] : default(CharacterSet);
			this.FillIntelligentCharactersToArgBox(eventArgBox, "ParticipateCharacter_0", participateCharSet);
			AdventureCharacterSortUtils.Sort(eventArgBox, false, 0, CharacterSortType.CombatPower, false);
		}

		// Token: 0x060019F6 RID: 6646 RVA: 0x001733D4 File Offset: 0x001715D4
		private void FillIntelligentCharactersToArgBox(EventArgBox eventArgBox, string keyPrefix, CharacterSet charSet)
		{
			int amountAdded = 0;
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			foreach (int charId in charSet.GetCollection())
			{
				GameData.Domains.Character.Character character;
				bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
				if (!flag)
				{
					bool flag2 = character.IsCrossAreaTraveling();
					if (flag2)
					{
						DomainManager.Character.GroupMove(context, character, this.Location);
					}
					bool flag3 = !character.GetLocation().Equals(this.Location);
					if (!flag3)
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
						defaultInterpolatedStringHandler.AppendFormatted(keyPrefix);
						defaultInterpolatedStringHandler.AppendLiteral("_");
						defaultInterpolatedStringHandler.AppendFormatted<int>(amountAdded);
						eventArgBox.Set(defaultInterpolatedStringHandler.ToStringAndClear(), charId);
						amountAdded++;
					}
				}
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Adding ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(amountAdded);
			defaultInterpolatedStringHandler.AppendLiteral(" real characters to adventure.");
			AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			eventArgBox.Set(keyPrefix + "_Count", amountAdded);
		}

		// Token: 0x060019F7 RID: 6647 RVA: 0x0017351C File Offset: 0x0017171C
		private void ClearCalledCharacters()
		{
			CallCharacterHelper.ClearCalledCharacters(this.ParticipatingCharacterSets, true, true);
		}

		// Token: 0x060019F8 RID: 6648 RVA: 0x00173530 File Offset: 0x00171730
		private void CallParticipateCharacters()
		{
			foreach (CharacterSet tempCalledSet in LegendaryBookMonthlyAction._tempCalledCharSets)
			{
				tempCalledSet.Clear();
			}
			LegendaryBookMonthlyAction._tempCalledCharSets.Clear();
			CallCharacterHelper.CallCharacters(this.Location, LegendaryBookMonthlyAction.SearchRule, LegendaryBookMonthlyAction._tempCalledCharSets, true);
			for (int index = 0; index < LegendaryBookMonthlyAction._tempCalledCharSets.Count; index++)
			{
				CharacterSet tempCalledSet2 = LegendaryBookMonthlyAction._tempCalledCharSets[index];
				bool flag = this.ParticipatingCharacterSets.Count <= index;
				if (flag)
				{
					this.ParticipatingCharacterSets.Add(default(CharacterSet));
				}
				CharacterSet charSet = this.ParticipatingCharacterSets[index];
				charSet.AddRange(tempCalledSet2.GetCollection());
				this.ParticipatingCharacterSets[index] = charSet;
			}
		}

		// Token: 0x060019F9 RID: 6649 RVA: 0x0017362C File Offset: 0x0017182C
		private static bool CheckCharacterAvailable(GameData.Domains.Character.Character character)
		{
			return CharacterMatcher.DefValue.AvailableForLegendaryBookAdventure.Match(character);
		}

		// Token: 0x060019FA RID: 6650 RVA: 0x0017363C File Offset: 0x0017183C
		private static bool CheckSectLeaderAvailable(GameData.Domains.Character.Character character)
		{
			OrganizationInfo orgInfo = character.GetOrganizationInfo();
			return orgInfo.Grade != 8 || !orgInfo.Principal || !Organization.Instance[orgInfo.OrgTemplateId].IsSect || DomainManager.TaiwuEvent.MainThreadDataContext.Random.CheckPercentProb(20);
		}

		// Token: 0x060019FB RID: 6651 RVA: 0x00173698 File Offset: 0x00171898
		public override bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x060019FC RID: 6652 RVA: 0x001736AC File Offset: 0x001718AC
		public override int GetSerializedSize()
		{
			int totalSize = 25;
			bool flag = this.ParticipatingCharacterSets != null;
			if (flag)
			{
				totalSize += 2;
				int elementsCount = this.ParticipatingCharacterSets.Count;
				for (int i = 0; i < elementsCount; i++)
				{
					totalSize += this.ParticipatingCharacterSets[i].GetSerializedSize();
				}
			}
			else
			{
				totalSize += 2;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060019FD RID: 6653 RVA: 0x00173728 File Offset: 0x00171928
		public unsafe override int Serialize(byte* pData)
		{
			*(short*)pData = this.DynamicActionType;
			byte* pCurrData = pData + 2;
			pCurrData += this.Location.Serialize(pCurrData);
			*pCurrData = (byte)this.BookType;
			pCurrData++;
			*pCurrData = (byte)this.BookAppearType;
			pCurrData++;
			*(int*)pCurrData = this.PrevOwnerId;
			pCurrData += 4;
			*pCurrData = (byte)this.ActivateDelay;
			pCurrData++;
			bool flag = this.ParticipatingCharacterSets != null;
			if (flag)
			{
				int elementsCount = this.ParticipatingCharacterSets.Count;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount);
				pCurrData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					int subDataSize = this.ParticipatingCharacterSets[i].Serialize(pCurrData);
					pCurrData += subDataSize;
					Tester.Assert(subDataSize <= 65535, "");
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			pCurrData += this.Key.Serialize(pCurrData);
			*pCurrData = (byte)this.State;
			pCurrData++;
			*(int*)pCurrData = this.Month;
			pCurrData += 4;
			*(int*)pCurrData = this.LastFinishDate;
			pCurrData += 4;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060019FE RID: 6654 RVA: 0x0017386C File Offset: 0x00171A6C
		public unsafe override int Deserialize(byte* pData)
		{
			byte* pCurrData = pData + 2;
			pCurrData += this.Location.Deserialize(pCurrData);
			this.BookType = *(sbyte*)pCurrData;
			pCurrData++;
			this.BookAppearType = *(sbyte*)pCurrData;
			pCurrData++;
			this.PrevOwnerId = *(int*)pCurrData;
			pCurrData += 4;
			this.ActivateDelay = *(sbyte*)pCurrData;
			pCurrData++;
			ushort elementsCount = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag = elementsCount > 0;
			if (flag)
			{
				bool flag2 = this.ParticipatingCharacterSets == null;
				if (flag2)
				{
					this.ParticipatingCharacterSets = new List<CharacterSet>((int)elementsCount);
				}
				else
				{
					this.ParticipatingCharacterSets.Clear();
				}
				for (int i = 0; i < (int)elementsCount; i++)
				{
					CharacterSet element = default(CharacterSet);
					pCurrData += element.Deserialize(pCurrData);
					this.ParticipatingCharacterSets.Add(element);
				}
			}
			else
			{
				List<CharacterSet> participatingCharacterSets = this.ParticipatingCharacterSets;
				if (participatingCharacterSets != null)
				{
					participatingCharacterSets.Clear();
				}
			}
			pCurrData += this.Key.Deserialize(pCurrData);
			this.State = *(sbyte*)pCurrData;
			pCurrData++;
			this.Month = *(int*)pCurrData;
			pCurrData += 4;
			this.LastFinishDate = *(int*)pCurrData;
			pCurrData += 4;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x040005FA RID: 1530
		[SerializableGameDataField]
		public Location Location;

		// Token: 0x040005FB RID: 1531
		[SerializableGameDataField]
		public sbyte BookType;

		// Token: 0x040005FC RID: 1532
		[SerializableGameDataField]
		public sbyte BookAppearType;

		// Token: 0x040005FD RID: 1533
		[SerializableGameDataField]
		public int PrevOwnerId;

		// Token: 0x040005FE RID: 1534
		[SerializableGameDataField]
		public sbyte ActivateDelay;

		// Token: 0x040005FF RID: 1535
		[SerializableGameDataField]
		public List<CharacterSet> ParticipatingCharacterSets;

		// Token: 0x04000600 RID: 1536
		private static List<CharacterSet> _tempCalledCharSets = new List<CharacterSet>();

		// Token: 0x04000601 RID: 1537
		private const int ActivateDelayMin = 3;

		// Token: 0x04000602 RID: 1538
		private const int ActivateDelayMax = 9;

		// Token: 0x04000603 RID: 1539
		private static readonly CallCharacterHelper.SearchCharacterRule SearchRule = new CallCharacterHelper.SearchCharacterRule
		{
			AllowTemporaryCharacter = false,
			SearchRange = 2,
			SubRules = new List<CallCharacterHelper.SearchCharacterSubRule>
			{
				new CallCharacterHelper.SearchCharacterSubRule(0, 4, new Predicate<GameData.Domains.Character.Character>(LegendaryBookMonthlyAction.CheckCharacterAvailable), new Predicate<GameData.Domains.Character.Character>(LegendaryBookMonthlyAction.CheckSectLeaderAvailable), null, null)
			}
		};
	}
}
