using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions.CustomActions
{
	// Token: 0x02000097 RID: 151
	[SerializableGameData(NotForDisplayModule = true)]
	[Obsolete]
	public class CallCharacterMonthlyAction : MonthlyActionBase, ISerializableGameData
	{
		// Token: 0x060019AA RID: 6570 RVA: 0x001700E0 File Offset: 0x0016E2E0
		public override void TriggerAction()
		{
			bool flag = this.State != 0 || !this.Location.IsValid();
			if (!flag)
			{
				this.State = 1;
				bool flag2 = this.AdventureTemplateId >= 0 && !DomainManager.Adventure.TryCreateAdventureSite(DomainManager.TaiwuEvent.MainThreadDataContext, this.Location.AreaId, this.Location.BlockId, this.AdventureTemplateId, this.Key);
				if (flag2)
				{
					this.State = 0;
					this.Location = Location.Invalid;
				}
				else
				{
					Action<CallCharacterMonthlyAction> onTrigger = this.OnTrigger;
					if (onTrigger != null)
					{
						onTrigger(this);
					}
				}
			}
		}

		// Token: 0x060019AB RID: 6571 RVA: 0x00170188 File Offset: 0x0016E388
		public override void MonthlyHandler()
		{
			bool flag = this.State == 0;
			if (flag)
			{
				Action<CallCharacterMonthlyAction> onWaitTrigger = this.OnWaitTrigger;
				if (onWaitTrigger != null)
				{
					onWaitTrigger(this);
				}
			}
			else
			{
				bool timeIsUp = this.PreparationRule.PreparationDuration > 0 && this.Month >= (int)this.PreparationRule.PreparationDuration;
				bool flag2 = this.State == 1;
				if (flag2)
				{
					this.CallMajorCharacters();
					bool flag3 = this.MajorCharacterSearchRule.AllowTemporaryCharacter || CallCharacterHelper.IsAllCharactersAtLocation(this.Location, this.MajorCharacterSearchRule, this.MajorCharacterSets);
					if (flag3)
					{
						this.CallParticipateCharacters();
					}
				}
				bool flag4 = this.State == 2;
				if (flag4)
				{
					this.CallParticipateCharacters();
				}
				bool flag5 = this.State == 3 && (timeIsUp || this.PreparationRule.CanStartEarly);
				if (flag5)
				{
					bool flag6 = this.ParticipateCharacterSearchRule.AllowTemporaryCharacter || CallCharacterHelper.IsAllCharactersAtLocation(this.Location, this.ParticipateCharacterSearchRule, this.ParticipatingCharacterSets);
					if (flag6)
					{
						this.Activate();
					}
					else
					{
						bool flag7 = timeIsUp;
						if (flag7)
						{
							this.Deactivate(false);
						}
					}
				}
				bool flag8 = this.State != 0;
				if (flag8)
				{
					this.Month++;
				}
			}
		}

		// Token: 0x060019AC RID: 6572 RVA: 0x001702CC File Offset: 0x0016E4CC
		public override void Activate()
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			bool flag = this.AdventureTemplateId >= 0;
			if (flag)
			{
				MonthlyEventActionsManager.NewlyActivated++;
				DomainManager.Adventure.ActivateAdventureSite(context, this.Location.AreaId, this.Location.BlockId);
			}
			this.State = 5;
		}

		// Token: 0x060019AD RID: 6573 RVA: 0x0017032C File Offset: 0x0016E52C
		public override void InheritNonArchiveData(MonthlyActionBase action)
		{
			CallCharacterMonthlyAction callCharacterMonthlyAction = action as CallCharacterMonthlyAction;
			bool flag = callCharacterMonthlyAction == null;
			if (flag)
			{
				string tag = "CallCharacterMonthlyAction";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 2);
				defaultInterpolatedStringHandler.AppendLiteral("fail to inherit action ");
				defaultInterpolatedStringHandler.AppendFormatted<MonthlyActionBase>(action);
				defaultInterpolatedStringHandler.AppendLiteral(" with key ");
				defaultInterpolatedStringHandler.AppendFormatted<MonthlyActionKey>(action.Key);
				defaultInterpolatedStringHandler.AppendLiteral(" due to invalid type.");
				AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), true);
			}
			else
			{
				this.PreparationRule = callCharacterMonthlyAction.PreparationRule;
				this.MajorCharacterSearchRule = callCharacterMonthlyAction.MajorCharacterSearchRule;
				this.ParticipateCharacterSearchRule = callCharacterMonthlyAction.ParticipateCharacterSearchRule;
				this.AdventureTemplateId = callCharacterMonthlyAction.AdventureTemplateId;
			}
		}

		// Token: 0x060019AE RID: 6574 RVA: 0x001703DC File Offset: 0x0016E5DC
		public override void EnsurePrerequisites()
		{
			int removedMajorCharCount = CallCharacterHelper.RemoveInvalidCharacters(this.MajorCharacterSearchRule, this.MajorCharacterSets);
			bool flag = removedMajorCharCount > 0;
			if (flag)
			{
				string tag = "ConfigMonthlyAction";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 2);
				defaultInterpolatedStringHandler.AppendFormatted<MonthlyActionKey>(this.Key);
				defaultInterpolatedStringHandler.AppendLiteral(" removed ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(removedMajorCharCount);
				defaultInterpolatedStringHandler.AppendLiteral(" major characters that are invalid");
				AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
			}
			int removedParticipateCharCount = CallCharacterHelper.RemoveInvalidCharacters(this.ParticipateCharacterSearchRule, this.ParticipatingCharacterSets);
			bool flag2 = removedParticipateCharCount > 0;
			if (flag2)
			{
				string tag2 = "ConfigMonthlyAction";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 2);
				defaultInterpolatedStringHandler.AppendFormatted<MonthlyActionKey>(this.Key);
				defaultInterpolatedStringHandler.AppendLiteral(" removed ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(removedParticipateCharCount);
				defaultInterpolatedStringHandler.AppendLiteral(" participate characters that are invalid");
				AdaptableLog.TagWarning(tag2, defaultInterpolatedStringHandler.ToStringAndClear(), false);
			}
			bool flag3 = !this.MajorCharacterSearchRule.AllowTemporaryCharacter && !CallCharacterHelper.IsAllCharactersAtLocation(this.Location, this.MajorCharacterSearchRule, this.MajorCharacterSets);
			if (flag3)
			{
				this.CallMajorCharacters();
			}
			bool flag4 = !this.ParticipateCharacterSearchRule.AllowTemporaryCharacter && !CallCharacterHelper.IsAllCharactersAtLocation(this.Location, this.ParticipateCharacterSearchRule, this.ParticipatingCharacterSets);
			if (flag4)
			{
				this.CallParticipateCharacters();
			}
			this.State = 5;
		}

		// Token: 0x060019AF RID: 6575 RVA: 0x00170534 File Offset: 0x0016E734
		public override MonthlyActionBase CreateCopy()
		{
			return Serializer.CreateCopy<CallCharacterMonthlyAction>(this);
		}

		// Token: 0x060019B0 RID: 6576 RVA: 0x0017054C File Offset: 0x0016E74C
		public void ClearCalledCharacters()
		{
			CallCharacterHelper.ClearCalledCharacters(this.MajorCharacterSets, false, true);
			CallCharacterHelper.ClearCalledCharacters(this.ParticipatingCharacterSets, false, true);
		}

		// Token: 0x060019B1 RID: 6577 RVA: 0x0017056C File Offset: 0x0016E76C
		private void CallMajorCharacters()
		{
			bool flag = !this.Location.IsValid();
			if (!flag)
			{
				bool flag2 = !CallCharacterHelper.CallCharacters(this.Location, this.MajorCharacterSearchRule, this.MajorCharacterSets, true);
				if (!flag2)
				{
					this.State = 2;
				}
			}
		}

		// Token: 0x060019B2 RID: 6578 RVA: 0x001705B8 File Offset: 0x0016E7B8
		private void CallParticipateCharacters()
		{
			bool flag = !this.Location.IsValid();
			if (!flag)
			{
				bool flag2 = !CallCharacterHelper.CallCharacters(this.Location, this.ParticipateCharacterSearchRule, this.ParticipatingCharacterSets, true);
				if (!flag2)
				{
					this.State = 3;
				}
			}
		}

		// Token: 0x060019B3 RID: 6579 RVA: 0x00170604 File Offset: 0x0016E804
		private void FillIntelligentCharactersToArgBox(EventArgBox eventArgBox, string keyPrefix, CharacterSet charSet, CallCharacterHelper.SearchCharacterSubRule searchSubRule, bool allowTempChars)
		{
			int amountAdded = 0;
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			foreach (int charId in charSet.GetCollection())
			{
				Character character;
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
						bool flag4 = searchSubRule.MaxAmount > 0 && amountAdded >= searchSubRule.MaxAmount;
						if (flag4)
						{
							Events.RaiseCharacterLocationChanged(context, charId, character.GetLocation(), this.Location);
							character.SetLocation(this.Location, context);
						}
						else
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
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Adding ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(amountAdded);
			defaultInterpolatedStringHandler.AppendLiteral(" real characters to adventure.");
			AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			bool flag5 = allowTempChars && searchSubRule.CreateTemporaryCharacterFunc != null;
			if (flag5)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 1);
				defaultInterpolatedStringHandler.AppendLiteral("creating ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(Math.Max(0, searchSubRule.MinAmount - amountAdded));
				defaultInterpolatedStringHandler.AppendLiteral(" temporary characters.");
				AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
				while (amountAdded < searchSubRule.MinAmount)
				{
					int charId2 = searchSubRule.CreateTemporaryCharacterFunc(context);
					DomainManager.Adventure.AddTemporaryIntelligentCharacter(charId2);
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
					defaultInterpolatedStringHandler.AppendFormatted(keyPrefix);
					defaultInterpolatedStringHandler.AppendLiteral("_");
					defaultInterpolatedStringHandler.AppendFormatted<int>(amountAdded);
					eventArgBox.Set(defaultInterpolatedStringHandler.ToStringAndClear(), charId2);
					amountAdded++;
				}
			}
			eventArgBox.Set(keyPrefix + "_Count", amountAdded);
		}

		// Token: 0x060019B4 RID: 6580 RVA: 0x00170868 File Offset: 0x0016EA68
		public CallCharacterMonthlyAction()
		{
			this.MajorCharacterSets = new List<CharacterSet>();
			this.ParticipatingCharacterSets = new List<CharacterSet>();
		}

		// Token: 0x060019B5 RID: 6581 RVA: 0x00170888 File Offset: 0x0016EA88
		public override bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x060019B6 RID: 6582 RVA: 0x0017089C File Offset: 0x0016EA9C
		public override int GetSerializedSize()
		{
			int totalSize = 16;
			bool flag = this.MajorCharacterSets != null;
			if (flag)
			{
				totalSize += 2;
				int elementsCount = this.MajorCharacterSets.Count;
				for (int i = 0; i < elementsCount; i++)
				{
					totalSize += this.MajorCharacterSets[i].GetSerializedSize();
				}
			}
			else
			{
				totalSize += 2;
			}
			bool flag2 = this.ParticipatingCharacterSets != null;
			if (flag2)
			{
				totalSize += 2;
				int elementsCount2 = this.ParticipatingCharacterSets.Count;
				for (int j = 0; j < elementsCount2; j++)
				{
					totalSize += this.ParticipatingCharacterSets[j].GetSerializedSize();
				}
			}
			else
			{
				totalSize += 2;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x00170970 File Offset: 0x0016EB70
		public unsafe override int Serialize(byte* pData)
		{
			byte* pCurrData = pData + this.Location.Serialize(pData);
			bool flag = this.MajorCharacterSets != null;
			if (flag)
			{
				int elementsCount = this.MajorCharacterSets.Count;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount);
				pCurrData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					int subDataSize = this.MajorCharacterSets[i].Serialize(pCurrData);
					pCurrData += subDataSize;
					Tester.Assert(subDataSize <= 65535, "");
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag2 = this.ParticipatingCharacterSets != null;
			if (flag2)
			{
				int elementsCount2 = this.ParticipatingCharacterSets.Count;
				Tester.Assert(elementsCount2 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount2);
				pCurrData += 2;
				for (int j = 0; j < elementsCount2; j++)
				{
					int subDataSize2 = this.ParticipatingCharacterSets[j].Serialize(pCurrData);
					pCurrData += subDataSize2;
					Tester.Assert(subDataSize2 <= 65535, "");
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

		// Token: 0x060019B8 RID: 6584 RVA: 0x00170B0C File Offset: 0x0016ED0C
		public unsafe override int Deserialize(byte* pData)
		{
			byte* pCurrData = pData + this.Location.Deserialize(pData);
			ushort elementsCount = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag = elementsCount > 0;
			if (flag)
			{
				bool flag2 = this.MajorCharacterSets == null;
				if (flag2)
				{
					this.MajorCharacterSets = new List<CharacterSet>((int)elementsCount);
				}
				else
				{
					this.MajorCharacterSets.Clear();
				}
				for (int i = 0; i < (int)elementsCount; i++)
				{
					CharacterSet element = default(CharacterSet);
					pCurrData += element.Deserialize(pCurrData);
					this.MajorCharacterSets.Add(element);
				}
			}
			else
			{
				List<CharacterSet> majorCharacterSets = this.MajorCharacterSets;
				if (majorCharacterSets != null)
				{
					majorCharacterSets.Clear();
				}
			}
			ushort elementsCount2 = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag3 = elementsCount2 > 0;
			if (flag3)
			{
				bool flag4 = this.ParticipatingCharacterSets == null;
				if (flag4)
				{
					this.ParticipatingCharacterSets = new List<CharacterSet>((int)elementsCount2);
				}
				else
				{
					this.ParticipatingCharacterSets.Clear();
				}
				for (int j = 0; j < (int)elementsCount2; j++)
				{
					CharacterSet element2 = default(CharacterSet);
					pCurrData += element2.Deserialize(pCurrData);
					this.ParticipatingCharacterSets.Add(element2);
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

		// Token: 0x040005CB RID: 1483
		[SerializableGameDataField]
		public Location Location;

		// Token: 0x040005CC RID: 1484
		[SerializableGameDataField]
		public List<CharacterSet> MajorCharacterSets;

		// Token: 0x040005CD RID: 1485
		[SerializableGameDataField]
		public List<CharacterSet> ParticipatingCharacterSets;

		// Token: 0x040005CE RID: 1486
		public CallCharacterHelper.SearchCharacterRule MajorCharacterSearchRule;

		// Token: 0x040005CF RID: 1487
		public CallCharacterHelper.SearchCharacterRule ParticipateCharacterSearchRule;

		// Token: 0x040005D0 RID: 1488
		public PreparationRule PreparationRule;

		// Token: 0x040005D1 RID: 1489
		public short AdventureTemplateId;

		// Token: 0x040005D2 RID: 1490
		public Action<CallCharacterMonthlyAction> OnWaitTrigger;

		// Token: 0x040005D3 RID: 1491
		public Action<CallCharacterMonthlyAction> OnTrigger;
	}
}
