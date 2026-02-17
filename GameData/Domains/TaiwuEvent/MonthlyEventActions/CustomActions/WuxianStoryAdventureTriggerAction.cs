using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions.CustomActions
{
	// Token: 0x0200009E RID: 158
	[SerializableGameData(NotForDisplayModule = true, IsExtensible = true)]
	public class WuxianStoryAdventureTriggerAction : MonthlyActionBase, IDynamicAction, ISerializableGameData
	{
		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06001A27 RID: 6695 RVA: 0x0017554E File Offset: 0x0017374E
		public short DynamicActionType
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06001A28 RID: 6696 RVA: 0x00175551 File Offset: 0x00173751
		public WuxianStoryAdventureTriggerAction()
		{
			this._highGradeCharacters = new List<int>();
			this._normalCharacters = new List<int>();
			this._location = Location.Invalid;
		}

		// Token: 0x06001A29 RID: 6697 RVA: 0x00175583 File Offset: 0x00173783
		public override void MonthlyHandler()
		{
		}

		// Token: 0x06001A2A RID: 6698 RVA: 0x00175588 File Offset: 0x00173788
		public override void TriggerAction()
		{
			bool flag = this.State != 0;
			if (!flag)
			{
				DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
				Settlement settlement = DomainManager.Organization.GetSettlementByOrgTemplateId(12);
				Location rootLocation = settlement.GetLocation();
				List<short> blockIdList = new List<short>();
				DomainManager.Map.GetSettlementBlocks(rootLocation.AreaId, rootLocation.BlockId, blockIdList);
				CollectionUtils.Shuffle<short>(context.Random, blockIdList);
				foreach (short blockId in blockIdList)
				{
					bool flag2 = DomainManager.Adventure.TryCreateAdventureSite(context, rootLocation.AreaId, blockId, 181, this.Key);
					if (flag2)
					{
						this._location = new Location(rootLocation.AreaId, blockId);
						DomainManager.Adventure.ActivateAdventureSite(context, rootLocation.AreaId, blockId);
						this.CallCharacters();
						break;
					}
				}
			}
		}

		// Token: 0x06001A2B RID: 6699 RVA: 0x0017568C File Offset: 0x0017388C
		public override void CollectCalledCharacters(HashSet<int> calledCharacters)
		{
			foreach (int charId in this._highGradeCharacters)
			{
				Character character;
				bool flag = this.IsCharacterStillValid(charId, out character);
				if (flag)
				{
					calledCharacters.Add(charId);
				}
			}
			foreach (int charId2 in this._normalCharacters)
			{
				Character character;
				bool flag2 = this.IsCharacterStillValid(charId2, out character);
				if (flag2)
				{
					calledCharacters.Add(charId2);
				}
			}
		}

		// Token: 0x06001A2C RID: 6700 RVA: 0x00175750 File Offset: 0x00173950
		public override void Deactivate(bool isComplete)
		{
			this.State = 0;
			this.Month = 0;
			this.LastFinishDate = DomainManager.World.GetCurrDate();
			this._location = Location.Invalid;
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			foreach (int charId in this._highGradeCharacters)
			{
				Character character;
				bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
				if (!flag)
				{
					character.DeactivateExternalRelationState(context, 4);
					bool flag2 = character.IsCompletelyInfected();
					if (flag2)
					{
						Events.RaiseInfectedCharacterLocationChanged(context, charId, Location.Invalid, character.GetLocation());
					}
					else
					{
						Events.RaiseCharacterLocationChanged(context, charId, Location.Invalid, character.GetLocation());
					}
				}
			}
			foreach (int charId2 in this._normalCharacters)
			{
				Character character2;
				bool flag3 = !DomainManager.Character.TryGetElement_Objects(charId2, out character2);
				if (!flag3)
				{
					character2.DeactivateExternalRelationState(context, 4);
					bool flag4 = character2.IsCompletelyInfected();
					if (flag4)
					{
						Events.RaiseInfectedCharacterLocationChanged(context, charId2, Location.Invalid, character2.GetLocation());
					}
					else
					{
						Events.RaiseCharacterLocationChanged(context, charId2, Location.Invalid, character2.GetLocation());
					}
				}
			}
			bool flag5 = !isComplete;
			if (flag5)
			{
				DomainManager.World.WuxianEndingFailing1(context, false);
			}
			this._highGradeCharacters.Clear();
			this._normalCharacters.Clear();
		}

		// Token: 0x06001A2D RID: 6701 RVA: 0x001758F8 File Offset: 0x00173AF8
		public override MonthlyActionBase CreateCopy()
		{
			return Serializer.CreateCopy<WuxianStoryAdventureTriggerAction>(this);
		}

		// Token: 0x06001A2E RID: 6702 RVA: 0x00175910 File Offset: 0x00173B10
		public override void EnsurePrerequisites()
		{
			List<int> temp = new List<int>();
			foreach (int charId in this._highGradeCharacters)
			{
				Character character;
				bool flag = this.IsCharacterStillValid(charId, out character);
				if (flag)
				{
					temp.Add(charId);
				}
			}
			this._highGradeCharacters = temp;
			temp = new List<int>();
			foreach (int charId2 in this._normalCharacters)
			{
				Character character;
				bool flag2 = this.IsCharacterStillValid(charId2, out character);
				if (flag2)
				{
					temp.Add(charId2);
				}
			}
			this._normalCharacters = temp;
		}

		// Token: 0x06001A2F RID: 6703 RVA: 0x001759F0 File Offset: 0x00173BF0
		public override void FillEventArgBox(EventArgBox eventArgBox)
		{
			int leaderId = -1;
			int grade = -1;
			for (int i = 0; i < this._highGradeCharacters.Count; i++)
			{
				int charId = this._highGradeCharacters[i];
				OrganizationInfo orgInfo = DomainManager.Character.GetElement_Objects(charId).GetOrganizationInfo();
				bool flag = (int)orgInfo.Grade > grade;
				if (flag)
				{
					grade = (int)orgInfo.Grade;
					leaderId = charId;
				}
				AdaptableLog.Info("Adding 1 major character to adventure.");
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 1);
				defaultInterpolatedStringHandler.AppendLiteral("MajorCharacter_0_");
				defaultInterpolatedStringHandler.AppendFormatted<int>(i);
				eventArgBox.Set(defaultInterpolatedStringHandler.ToStringAndClear(), charId);
			}
			eventArgBox.Set("MajorCharacter_0_Count", this._highGradeCharacters.Count);
			for (int j = 0; j < this._normalCharacters.Count; j++)
			{
				int charId2 = this._normalCharacters[j];
				AdaptableLog.Info("Adding 1 participate character to adventure.");
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 1);
				defaultInterpolatedStringHandler.AppendLiteral("ParticipateCharacter_0_");
				defaultInterpolatedStringHandler.AppendFormatted<int>(j);
				eventArgBox.Set(defaultInterpolatedStringHandler.ToStringAndClear(), charId2);
			}
			eventArgBox.Set("ParticipateCharacter_0_Count", this._normalCharacters.Count);
			AdaptableLog.Info("Adding leader to adventure.");
			bool flag2 = leaderId >= 0;
			if (flag2)
			{
				eventArgBox.Set("SectLeader", leaderId);
			}
		}

		// Token: 0x06001A30 RID: 6704 RVA: 0x00175B58 File Offset: 0x00173D58
		private bool IsCharacterValidToCall(int charId, out Character character)
		{
			bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				OrganizationInfo orgInfo = character.GetOrganizationInfo();
				bool flag2 = character.IsActiveExternalRelationState(60);
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = character.GetAgeGroup() != 2;
					if (flag3)
					{
						result = false;
					}
					else
					{
						bool flag4 = character.GetKidnapperId() >= 0;
						if (flag4)
						{
							result = false;
						}
						else
						{
							bool flag5 = orgInfo.OrgTemplateId != 12;
							if (flag5)
							{
								result = false;
							}
							else
							{
								bool flag6 = orgInfo.Grade >= 6;
								if (flag6)
								{
									result = true;
								}
								else
								{
									bool flag7 = this._currMenteeCountInAdventure >= 15;
									if (flag7)
									{
										result = false;
									}
									else
									{
										this._currMenteeCountInAdventure++;
										result = true;
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06001A31 RID: 6705 RVA: 0x00175C24 File Offset: 0x00173E24
		private bool IsCharacterStillValid(int charId, out Character character)
		{
			bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				OrganizationInfo orgInfo = character.GetOrganizationInfo();
				bool flag2 = character.GetKidnapperId() >= 0;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = orgInfo.OrgTemplateId != 12;
					result = !flag3;
				}
			}
			return result;
		}

		// Token: 0x06001A32 RID: 6706 RVA: 0x00175C84 File Offset: 0x00173E84
		private void CallCharacters()
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			Settlement settlement = DomainManager.Organization.GetSettlementByOrgTemplateId(12);
			Location rootLocation = settlement.GetLocation();
			List<short> blockIdList = new List<short>();
			HashSet<int> test = new HashSet<int>();
			DomainManager.Map.GetSettlementBlocks(rootLocation.AreaId, rootLocation.BlockId, blockIdList);
			this._currMenteeCountInAdventure = 0;
			foreach (short blockId in blockIdList)
			{
				MapBlockData block = DomainManager.Map.GetBlockData(rootLocation.AreaId, blockId);
				HashSet<int> charIds = block.CharacterSet;
				bool flag = charIds == null;
				if (!flag)
				{
					foreach (int charId in charIds)
					{
						Character character;
						bool flag2 = this.IsCharacterValidToCall(charId, out character);
						if (flag2)
						{
							bool flag3 = character.GetOrganizationInfo().Grade >= 6;
							if (flag3)
							{
								this._highGradeCharacters.Add(charId);
							}
							else
							{
								this._normalCharacters.Add(charId);
							}
						}
					}
				}
			}
			foreach (int charId2 in this._highGradeCharacters)
			{
				Character character2;
				bool flag4 = !DomainManager.Character.TryGetElement_Objects(charId2, out character2);
				if (!flag4)
				{
					DomainManager.Character.LeaveGroup(context, character2, true);
					DomainManager.Character.GroupMove(context, character2, this._location);
					Events.RaiseCharacterLocationChanged(context, charId2, this._location, Location.Invalid);
					character2.ActiveExternalRelationState(context, 4);
				}
			}
			foreach (int charId3 in this._normalCharacters)
			{
				Character character3;
				bool flag5 = !DomainManager.Character.TryGetElement_Objects(charId3, out character3);
				if (!flag5)
				{
					DomainManager.Character.LeaveGroup(context, character3, true);
					DomainManager.Character.GroupMove(context, character3, this._location);
					Events.RaiseCharacterLocationChanged(context, charId3, this._location, Location.Invalid);
					character3.ActiveExternalRelationState(context, 4);
				}
			}
		}

		// Token: 0x06001A33 RID: 6707 RVA: 0x00175F08 File Offset: 0x00174108
		public override bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06001A34 RID: 6708 RVA: 0x00175F1C File Offset: 0x0017411C
		public override int GetSerializedSize()
		{
			int totalSize = 20;
			bool flag = this._highGradeCharacters != null;
			if (flag)
			{
				totalSize += 2 + 4 * this._highGradeCharacters.Count;
			}
			else
			{
				totalSize += 2;
			}
			bool flag2 = this._normalCharacters != null;
			if (flag2)
			{
				totalSize += 2 + 4 * this._normalCharacters.Count;
			}
			else
			{
				totalSize += 2;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001A35 RID: 6709 RVA: 0x00175F90 File Offset: 0x00174190
		public unsafe override int Serialize(byte* pData)
		{
			*(short*)pData = this.DynamicActionType;
			byte* pCurrData = pData + 2;
			*(short*)pCurrData = 7;
			pCurrData += 2;
			bool flag = this._highGradeCharacters != null;
			if (flag)
			{
				int elementsCount = this._highGradeCharacters.Count;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount);
				pCurrData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					*(int*)(pCurrData + (IntPtr)i * 4) = this._highGradeCharacters[i];
				}
				pCurrData += 4 * elementsCount;
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag2 = this._normalCharacters != null;
			if (flag2)
			{
				int elementsCount2 = this._normalCharacters.Count;
				Tester.Assert(elementsCount2 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount2);
				pCurrData += 2;
				for (int j = 0; j < elementsCount2; j++)
				{
					*(int*)(pCurrData + (IntPtr)j * 4) = this._normalCharacters[j];
				}
				pCurrData += 4 * elementsCount2;
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			pCurrData += this._location.Serialize(pCurrData);
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

		// Token: 0x06001A36 RID: 6710 RVA: 0x0017610C File Offset: 0x0017430C
		public unsafe override int Deserialize(byte* pData)
		{
			byte* pCurrData = pData + 2;
			ushort fieldCount = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag = fieldCount > 0;
			if (flag)
			{
				ushort elementsCount = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag2 = elementsCount > 0;
				if (flag2)
				{
					bool flag3 = this._highGradeCharacters == null;
					if (flag3)
					{
						this._highGradeCharacters = new List<int>((int)elementsCount);
					}
					else
					{
						this._highGradeCharacters.Clear();
					}
					for (int i = 0; i < (int)elementsCount; i++)
					{
						this._highGradeCharacters.Add(*(int*)(pCurrData + (IntPtr)i * 4));
					}
					pCurrData += 4 * elementsCount;
				}
				else
				{
					List<int> highGradeCharacters = this._highGradeCharacters;
					if (highGradeCharacters != null)
					{
						highGradeCharacters.Clear();
					}
				}
			}
			bool flag4 = fieldCount > 1;
			if (flag4)
			{
				ushort elementsCount2 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag5 = elementsCount2 > 0;
				if (flag5)
				{
					bool flag6 = this._normalCharacters == null;
					if (flag6)
					{
						this._normalCharacters = new List<int>((int)elementsCount2);
					}
					else
					{
						this._normalCharacters.Clear();
					}
					for (int j = 0; j < (int)elementsCount2; j++)
					{
						this._normalCharacters.Add(*(int*)(pCurrData + (IntPtr)j * 4));
					}
					pCurrData += 4 * elementsCount2;
				}
				else
				{
					List<int> normalCharacters = this._normalCharacters;
					if (normalCharacters != null)
					{
						normalCharacters.Clear();
					}
				}
			}
			bool flag7 = fieldCount > 2;
			if (flag7)
			{
				pCurrData += this._location.Deserialize(pCurrData);
			}
			bool flag8 = fieldCount > 3;
			if (flag8)
			{
				pCurrData += this.Key.Deserialize(pCurrData);
			}
			bool flag9 = fieldCount > 4;
			if (flag9)
			{
				this.State = *(sbyte*)pCurrData;
				pCurrData++;
			}
			bool flag10 = fieldCount > 5;
			if (flag10)
			{
				this.Month = *(int*)pCurrData;
				pCurrData += 4;
			}
			bool flag11 = fieldCount > 6;
			if (flag11)
			{
				this.LastFinishDate = *(int*)pCurrData;
				pCurrData += 4;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0400060A RID: 1546
		private const int MaxMenteeCountInAdventure = 15;

		// Token: 0x0400060B RID: 1547
		private int _currMenteeCountInAdventure = 0;

		// Token: 0x0400060C RID: 1548
		[SerializableGameDataField]
		private List<int> _highGradeCharacters;

		// Token: 0x0400060D RID: 1549
		[SerializableGameDataField]
		private List<int> _normalCharacters;

		// Token: 0x0400060E RID: 1550
		[SerializableGameDataField]
		private Location _location;

		// Token: 0x020009AA RID: 2474
		private static class FieldIds
		{
			// Token: 0x0400288E RID: 10382
			public const ushort HighGradeCharacters = 0;

			// Token: 0x0400288F RID: 10383
			public const ushort NormalCharacters = 1;

			// Token: 0x04002890 RID: 10384
			public const ushort Location = 2;

			// Token: 0x04002891 RID: 10385
			public const ushort Key = 3;

			// Token: 0x04002892 RID: 10386
			public const ushort State = 4;

			// Token: 0x04002893 RID: 10387
			public const ushort Month = 5;

			// Token: 0x04002894 RID: 10388
			public const ushort LastFinishDate = 6;

			// Token: 0x04002895 RID: 10389
			public const ushort Count = 7;

			// Token: 0x04002896 RID: 10390
			public static readonly string[] FieldId2FieldName = new string[]
			{
				"HighGradeCharacters",
				"NormalCharacters",
				"Location",
				"Key",
				"State",
				"Month",
				"LastFinishDate"
			};
		}
	}
}
