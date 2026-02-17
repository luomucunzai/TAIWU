using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Dependencies;
using GameData.Domains.Character;
using GameData.Domains.Global;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord.GeneralRecord;
using GameData.Domains.Map;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using NLog;
using Redzen.Random;

namespace GameData.Domains.LifeRecord
{
	// Token: 0x02000657 RID: 1623
	[GameDataDomain(13, CustomArchiveModuleCode = true)]
	public class LifeRecordDomain : BaseGameDataDomain
	{
		// Token: 0x06004D2E RID: 19758 RVA: 0x002A9223 File Offset: 0x002A7423
		private void OnInitializedDomainData()
		{
		}

		// Token: 0x06004D2F RID: 19759 RVA: 0x002A9226 File Offset: 0x002A7426
		private void InitializeOnInitializeGameDataModule()
		{
		}

		// Token: 0x06004D30 RID: 19760 RVA: 0x002A9229 File Offset: 0x002A7429
		private void InitializeOnEnterNewWorld()
		{
		}

		// Token: 0x06004D31 RID: 19761 RVA: 0x002A922C File Offset: 0x002A742C
		private void OnLoadedArchiveData()
		{
		}

		// Token: 0x06004D32 RID: 19762 RVA: 0x002A9230 File Offset: 0x002A7430
		private unsafe void ProcessLifeRecordArchiveResponse(OperationWrapper operation, byte* pResult)
		{
			switch (operation.MethodId)
			{
			case 0:
				return;
			case 1:
			{
				ReadonlyLifeRecordsWithTotalCount records;
				ResponseProcessor.LifeRecord_Get(pResult, out records);
				GameDataBridge.TryReturnPassthroughMethod<ReadonlyLifeRecordsWithTotalCount>(operation.Id, records);
				return;
			}
			case 2:
			{
				ReadonlyLifeRecordsWithDate records2;
				ResponseProcessor.LifeRecord_GetByDate(pResult, out records2);
				GameDataBridge.TryReturnPassthroughMethod<ReadonlyLifeRecordsWithDate>(operation.Id, records2);
				return;
			}
			case 3:
			{
				ReadonlyLifeRecords records3;
				ResponseProcessor.LifeRecord_GetLast(pResult, out records3);
				GameDataBridge.TryReturnPassthroughMethod<ReadonlyLifeRecords>(operation.Id, records3);
				return;
			}
			case 4:
			{
				List<int> indexes;
				ResponseProcessor.LifeRecord_Search(pResult, out indexes);
				GameDataBridge.TryReturnPassthroughMethod<List<int>>(operation.Id, indexes);
				return;
			}
			case 5:
				return;
			case 6:
				return;
			case 7:
			{
				ReadonlyLifeRecords records4;
				ResponseProcessor.LifeRecord_GetDead(pResult, out records4);
				GameDataBridge.TryReturnPassthroughMethod<ReadonlyLifeRecords>(operation.Id, records4);
				return;
			}
			case 8:
				return;
			case 11:
			{
				ReadonlyLifeRecords records5;
				ResponseProcessor.LifeRecord_GetAllByChar(pResult, out records5);
				Action<ReadonlyLifeRecords> onReceiveCharacterLifeRecords = LifeRecordDomain._onReceiveCharacterLifeRecords;
				if (onReceiveCharacterLifeRecords != null)
				{
					onReceiveCharacterLifeRecords(records5);
				}
				LifeRecordDomain._onReceiveCharacterLifeRecords = null;
				return;
			}
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Unsupported LifeRecordMethodId: ");
			defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06004D33 RID: 19763 RVA: 0x002A9374 File Offset: 0x002A7574
		public void Add(LifeRecordCollection collection)
		{
			OperationAdder.LifeRecord_Add(collection);
		}

		// Token: 0x06004D34 RID: 19764 RVA: 0x002A937E File Offset: 0x002A757E
		[DomainMethod(IsPassthrough = true)]
		public void Get(uint operationId, int charId, int beginIndex, int count)
		{
			OperationAdder.LifeRecord_Get(charId, beginIndex, count, true, operationId);
		}

		// Token: 0x06004D35 RID: 19765 RVA: 0x002A9390 File Offset: 0x002A7590
		[DomainMethod(IsPassthrough = true)]
		public void GetByDate(uint operationId, int charId, int startDate, int monthCount)
		{
			int currDate = DomainManager.World.GetCurrDate();
			OperationAdder.LifeRecord_GetByDate(charId, startDate, monthCount, currDate, true, operationId);
		}

		// Token: 0x06004D36 RID: 19766 RVA: 0x002A93B6 File Offset: 0x002A75B6
		[DomainMethod(IsPassthrough = true)]
		public void GetLast(uint operationId, int charId, int count)
		{
			OperationAdder.LifeRecord_GetLast(charId, count, true, operationId);
		}

		// Token: 0x06004D37 RID: 19767 RVA: 0x002A93C4 File Offset: 0x002A75C4
		[DomainMethod(IsPassthrough = true)]
		public void GetRelated(uint operationId, int charId, int date, short recordType, int relatedCharId)
		{
			List<short> types = LifeRecord.Instance[recordType].RelatedIds;
			bool flag = types.Count <= 0;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Record type ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(recordType);
				defaultInterpolatedStringHandler.AppendLiteral(" does not have related records.");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			OperationAdder.LifeRecord_Search(relatedCharId, date, types, charId, true, operationId);
		}

		// Token: 0x06004D38 RID: 19768 RVA: 0x002A943C File Offset: 0x002A763C
		public void Remove(int charId)
		{
			OperationAdder.LifeRecord_Remove(charId);
		}

		// Token: 0x06004D39 RID: 19769 RVA: 0x002A9446 File Offset: 0x002A7646
		public void GenerateDead(int charId)
		{
			OperationAdder.LifeRecord_GenerateDead(charId);
		}

		// Token: 0x06004D3A RID: 19770 RVA: 0x002A9450 File Offset: 0x002A7650
		[DomainMethod(IsPassthrough = true)]
		public void GetDead(uint operationId, int charId)
		{
			OperationAdder.LifeRecord_GetDead(charId, true, operationId);
		}

		// Token: 0x06004D3B RID: 19771 RVA: 0x002A945C File Offset: 0x002A765C
		public void RemoveDead(int charId)
		{
			OperationAdder.LifeRecord_RemoveDead(charId);
		}

		// Token: 0x06004D3C RID: 19772 RVA: 0x002A9468 File Offset: 0x002A7668
		public void GetTaiwuLifeRecords()
		{
			int charId = DomainManager.Taiwu.GetTaiwuCharId();
			OperationAdder.LifeRecord_GetAllByChar(charId, false, 0U);
		}

		// Token: 0x06004D3D RID: 19773 RVA: 0x002A948C File Offset: 0x002A768C
		public LifeRecordCollection GetLifeRecordCollection()
		{
			return this._currLifeRecords;
		}

		// Token: 0x06004D3E RID: 19774 RVA: 0x002A94A4 File Offset: 0x002A76A4
		public void CommitCurrLifeRecords()
		{
			bool flag = this._currLifeRecords.Count <= 0;
			if (!flag)
			{
				DomainManager.LifeRecord.Add(this._currLifeRecords);
				this._currLifeRecords.Clear();
			}
		}

		// Token: 0x06004D3F RID: 19775 RVA: 0x002A94E8 File Offset: 0x002A76E8
		[DomainMethod]
		public ArgumentCollectionRenderArguments GetRecordRenderInfoArguments(DataContext context, string key, RecordArgumentsRequest request, bool isDreamBack = false)
		{
			ArgumentCollectionRenderArguments data = new ArgumentCollectionRenderArguments();
			data.Key = key;
			List<int> list = request.Characters;
			bool flag = list != null && list.Count > 0;
			if (flag)
			{
				data.CharNameAndLifeDataList = (isDreamBack ? DomainManager.Extra.GetNameAndLifeRelatedDataListForDreamBack(request.Characters) : DomainManager.Character.GetNameAndLifeRelatedDataList(request.Characters));
			}
			List<Location> locations = request.Locations;
			bool flag2 = locations != null && locations.Count > 0;
			if (flag2)
			{
				data.LocationNames = DomainManager.Map.GetLocationNameRelatedDataList(request.Locations);
			}
			List<short> settlements = request.Settlements;
			bool flag3 = settlements != null && settlements.Count > 0;
			if (flag3)
			{
				data.SettlementNames = DomainManager.Organization.GetSettlementNameRelatedData(request.Settlements);
			}
			list = request.JiaoLoongs;
			bool flag4 = list != null && list.Count > 0;
			if (flag4)
			{
				data.JiaoLoongNames = DomainManager.Extra.GetJiaoLoongNameRelatedDataList(request.JiaoLoongs);
			}
			return data;
		}

		// Token: 0x06004D40 RID: 19776 RVA: 0x002A95EC File Offset: 0x002A77EC
		public override void PackCrossArchiveGameData(CrossArchiveGameData crossArchiveGameData)
		{
			LifeRecordDomain._onReceiveCharacterLifeRecords = (Action<ReadonlyLifeRecords>)Delegate.Combine(LifeRecordDomain._onReceiveCharacterLifeRecords, new Action<ReadonlyLifeRecords>(delegate(ReadonlyLifeRecords lifeRecords)
			{
				crossArchiveGameData.LifeRecords = lifeRecords;
			}));
			this.GetTaiwuLifeRecords();
		}

		// Token: 0x06004D41 RID: 19777 RVA: 0x002A9630 File Offset: 0x002A7830
		public void InitializeTestRelatedData()
		{
			this._sourceRecordTemplateIds.Clear();
			foreach (LifeRecordItem item in ((IEnumerable<LifeRecordItem>)LifeRecord.Instance))
			{
				bool isSourceRecord = item.IsSourceRecord;
				if (isSourceRecord)
				{
					this._sourceRecordTemplateIds.Add(item.TemplateId);
				}
			}
			this._templateId2Name.Clear();
			Type defKeysType = Type.GetType("Config.LifeRecord+DefKey");
			Tester.Assert(defKeysType != null, "");
			FieldInfo[] defKeysFieldInfos = defKeysType.GetFields(BindingFlags.Static | BindingFlags.Public);
			foreach (FieldInfo info in defKeysFieldInfos)
			{
				string name = info.Name;
				short templateId = (short)info.GetValue(null);
				this._templateId2Name.Add(templateId, name);
			}
			this._lifeRecordCollectionType = Type.GetType("GameData.Domains.LifeRecord.LifeRecordCollection");
		}

		// Token: 0x06004D42 RID: 19778 RVA: 0x002A972C File Offset: 0x002A792C
		public void AddRandomLifeRecord(DataContext context, LifeRecordCollection lifeRecords, GameData.Domains.Character.Character character, MapBlockData mapBlockData)
		{
			for (int i = 0; i < 3; i++)
			{
				bool success = this.AddRandomLifeRecordInternal(context, lifeRecords, character, mapBlockData);
				bool flag = success;
				if (flag)
				{
					break;
				}
			}
		}

		// Token: 0x06004D43 RID: 19779 RVA: 0x002A9760 File Offset: 0x002A7960
		public void ShowLifeRecords(int charId, int beginIndex, int desiredCount, ReadonlyLifeRecordsWithTotalCount lifeRecords)
		{
			bool flag = this._lifeRecordCollectionType == null;
			if (flag)
			{
				this.InitializeTestRelatedData();
			}
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			ValueTuple<string, string> realName = CharacterDomain.GetRealName(character);
			string surname = realName.Item1;
			string givenName = realName.Item2;
			Logger logger = LifeRecordDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 4);
			defaultInterpolatedStringHandler.AppendFormatted(surname);
			defaultInterpolatedStringHandler.AppendFormatted(givenName);
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(lifeRecords.Records.Count);
			defaultInterpolatedStringHandler.AppendLiteral("/");
			defaultInterpolatedStringHandler.AppendFormatted<int>(lifeRecords.TotalCount);
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			List<LifeRecordRenderInfo> renderInfos = new List<LifeRecordRenderInfo>();
			ArgumentCollection argumentCollection = new ArgumentCollection();
			lifeRecords.Records.GetRenderInfos(renderInfos, argumentCollection);
			foreach (LifeRecordRenderInfo info in renderInfos)
			{
				int year = info.Date / 12;
				int month = info.Date % 12;
				Logger logger2 = LifeRecordDomain.Logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 3);
				defaultInterpolatedStringHandler.AppendLiteral("  ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(year + 1);
				defaultInterpolatedStringHandler.AppendLiteral("年");
				defaultInterpolatedStringHandler.AppendFormatted<int>(month + 1);
				defaultInterpolatedStringHandler.AppendLiteral("月: ");
				defaultInterpolatedStringHandler.AppendFormatted(info.Text);
				logger2.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06004D44 RID: 19780 RVA: 0x002A98F4 File Offset: 0x002A7AF4
		public void ShowLifeRecords(int charId, int desiredCount, ReadonlyLifeRecords lifeRecords)
		{
			bool flag = this._lifeRecordCollectionType == null;
			if (flag)
			{
				this.InitializeTestRelatedData();
			}
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			ValueTuple<string, string> realName = CharacterDomain.GetRealName(character);
			string surname = realName.Item1;
			string givenName = realName.Item2;
			Logger logger = LifeRecordDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 4);
			defaultInterpolatedStringHandler.AppendFormatted(surname);
			defaultInterpolatedStringHandler.AppendFormatted(givenName);
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(lifeRecords.Count);
			defaultInterpolatedStringHandler.AppendLiteral("/");
			defaultInterpolatedStringHandler.AppendFormatted<int>(desiredCount);
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			List<LifeRecordRenderInfo> renderInfos = new List<LifeRecordRenderInfo>();
			ArgumentCollection argumentCollection = new ArgumentCollection();
			lifeRecords.GetRenderInfos(renderInfos, argumentCollection);
			foreach (LifeRecordRenderInfo info in renderInfos)
			{
				int year = info.Date / 12;
				int month = info.Date % 12;
				Logger logger2 = LifeRecordDomain.Logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 3);
				defaultInterpolatedStringHandler.AppendLiteral("  ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(year + 1);
				defaultInterpolatedStringHandler.AppendLiteral("年");
				defaultInterpolatedStringHandler.AppendFormatted<int>(month + 1);
				defaultInterpolatedStringHandler.AppendLiteral("月: ");
				defaultInterpolatedStringHandler.AppendFormatted(info.Text);
				logger2.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06004D45 RID: 19781 RVA: 0x002A9A78 File Offset: 0x002A7C78
		private bool AddRandomLifeRecordInternal(DataContext context, LifeRecordCollection lifeRecords, GameData.Domains.Character.Character character, MapBlockData mapBlockData)
		{
			int randomIndex = context.Random.Next(this._sourceRecordTemplateIds.Count);
			short recordTemplateId = this._sourceRecordTemplateIds[randomIndex];
			LifeRecordItem config = LifeRecord.Instance[recordTemplateId];
			string name = this._templateId2Name[config.TemplateId];
			MethodInfo methodInfo = this._lifeRecordCollectionType.GetMethod("Add" + name);
			Tester.Assert(methodInfo != null, "");
			List<object> arguments = new List<object>
			{
				character.GetId(),
				DomainManager.World.GetCurrDate()
			};
			int i = 0;
			int count = config.Parameters.Length;
			while (i < count)
			{
				string paramName = config.Parameters[i];
				bool flag = string.IsNullOrEmpty(paramName);
				if (flag)
				{
					break;
				}
				sbyte paramType = ParameterType.Parse(paramName);
				bool success = LifeRecordDomain.AddArguments(context.Random, arguments, paramType, character, mapBlockData);
				bool flag2 = !success;
				if (flag2)
				{
					return false;
				}
				i++;
			}
			bool flag3 = config.RelatedIds.Count > 1;
			if (flag3)
			{
				randomIndex = context.Random.Next(config.RelatedIds.Count);
				short relatedId = config.RelatedIds[randomIndex];
				arguments.Add(relatedId);
			}
			methodInfo.Invoke(lifeRecords, arguments.ToArray());
			return true;
		}

		// Token: 0x06004D46 RID: 19782 RVA: 0x002A9BF0 File Offset: 0x002A7DF0
		private static bool AddArguments(IRandomSource random, List<object> arguments, sbyte paramType, GameData.Domains.Character.Character character, MapBlockData mapBlockData)
		{
			switch (paramType)
			{
			case 0:
			{
				int otherCharId = LifeRecordDomain.GetOtherCharacter(mapBlockData.CharacterSet, character.GetId());
				bool flag = otherCharId < 0;
				if (flag)
				{
					return false;
				}
				arguments.Add(otherCharId);
				return true;
			}
			case 1:
			{
				Location location = character.GetLocation();
				bool flag2 = !location.IsValid();
				if (flag2)
				{
					location = character.GetValidLocation();
				}
				arguments.Add(location);
				return true;
			}
			case 2:
			{
				ValueTuple<sbyte, short> item = LifeRecordDomain.GetItem(character);
				sbyte itemType = item.Item1;
				short itemTemplateId = item.Item2;
				bool flag3 = itemType < 0;
				if (flag3)
				{
					return false;
				}
				arguments.Add(itemType);
				arguments.Add(itemTemplateId);
				return true;
			}
			case 3:
			{
				short combatSkillId = (short)random.Next(0, CombatSkill.Instance.Count);
				arguments.Add(combatSkillId);
				return true;
			}
			case 4:
			{
				sbyte resourceType = (sbyte)random.Next(0, 8);
				arguments.Add(resourceType);
				return true;
			}
			case 5:
			{
				short settlementId = character.GetOrganizationInfo().SettlementId;
				bool flag4 = settlementId < 0;
				if (flag4)
				{
					return false;
				}
				arguments.Add(settlementId);
				return true;
			}
			case 6:
			{
				OrganizationInfo orgInfo = character.GetOrganizationInfo();
				arguments.Add(orgInfo.OrgTemplateId);
				arguments.Add(orgInfo.Grade);
				arguments.Add(orgInfo.Principal);
				arguments.Add(character.GetGender());
				return true;
			}
			case 20:
			{
				bool flag5 = mapBlockData.TemplateEnemyList == null || mapBlockData.TemplateEnemyList.Count == 0;
				if (flag5)
				{
					return false;
				}
				short randomEnemyId = mapBlockData.TemplateEnemyList[0].TemplateId;
				arguments.Add(randomEnemyId);
				return true;
			}
			case 21:
			{
				short templateId = (short)random.Next(CharacterFeature.Instance.Count);
				arguments.Add(templateId);
				return true;
			}
			case 22:
			{
				int val = random.Next(100);
				arguments.Add(val);
				return true;
			}
			case 23:
			{
				short lifeSkillTemplateId = (short)random.Next(LifeSkill.Instance.Count);
				arguments.Add(lifeSkillTemplateId);
				return true;
			}
			case 24:
			{
				sbyte merchantType = (sbyte)random.Next(7);
				arguments.Add(merchantType);
				return true;
			}
			case 26:
			{
				sbyte combatType = (sbyte)random.Next(4);
				arguments.Add(combatType);
				return true;
			}
			case 27:
			{
				sbyte lifeSkillType = (sbyte)random.Next(16);
				arguments.Add(lifeSkillType);
				return true;
			}
			case 28:
			{
				sbyte combatSkillType = (sbyte)random.Next(14);
				arguments.Add(combatSkillType);
				return true;
			}
			case 29:
			{
				short infoTemplateId = (short)random.Next(Information.Instance.Count);
				arguments.Add(infoTemplateId);
				return true;
			}
			case 30:
			{
				short secretInfoTemplateId = (short)random.Next(SecretInformation.Instance.Count);
				arguments.Add(secretInfoTemplateId);
				return true;
			}
			case 31:
			{
				short punishmentType = (short)random.Next(PunishmentType.Instance.Count);
				arguments.Add(punishmentType);
				return true;
			}
			case 32:
			{
				short characterTitle = (short)random.Next(CharacterTitle.Instance.Count);
				arguments.Add(characterTitle);
				return true;
			}
			case 33:
			{
				float floatValue = random.NextFloat();
				arguments.Add(floatValue);
				return true;
			}
			case 34:
			{
				int otherCharId2 = LifeRecordDomain.GetOtherCharacter(mapBlockData.CharacterSet, character.GetId());
				bool flag6 = otherCharId2 < 0;
				if (flag6)
				{
					return false;
				}
				arguments.Add(otherCharId2);
				return true;
			}
			case 35:
			{
				sbyte month = (sbyte)random.Next(Month.Instance.Count);
				arguments.Add(month);
				return true;
			}
			case 36:
			{
				int profession = random.Next(Profession.Instance.Count);
				arguments.Add(profession);
				return true;
			}
			case 37:
			{
				int professionSkill = random.Next(ProfessionSkill.Instance.Count);
				arguments.Add(professionSkill);
				return true;
			}
			case 38:
			{
				sbyte itemGrade = (sbyte)random.Next(9);
				arguments.Add(itemGrade);
				return true;
			}
			case 39:
				return false;
			case 40:
			{
				short music = (short)random.Next(Music.Instance.Count);
				arguments.Add(music);
				return true;
			}
			case 41:
			{
				sbyte state = (sbyte)random.Next(MapState.Instance.Count);
				arguments.Add(state);
				return true;
			}
			case 42:
				return false;
			case 43:
			{
				short jiaoProperty = (short)random.Next(JiaoProperty.Instance.Count);
				arguments.Add(jiaoProperty);
				return false;
			}
			case 44:
			{
				sbyte destinyType = (sbyte)random.Next(DestinyType.Instance.Count);
				arguments.Add(destinyType);
				return true;
			}
			case 45:
			{
				short templateId2 = (short)random.Next(SecretInformation.Instance.Count);
				arguments.Add(new ValueTuple<short, int>(templateId2, -1));
				return true;
			}
			}
			return false;
		}

		// Token: 0x06004D47 RID: 19783 RVA: 0x002AA23C File Offset: 0x002A843C
		private static int GetOtherCharacter(HashSet<int> charIds, int selfCharId)
		{
			bool flag = charIds == null;
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				foreach (int charId in charIds)
				{
					bool flag2 = charId != selfCharId;
					if (flag2)
					{
						return charId;
					}
				}
				result = -1;
			}
			return result;
		}

		// Token: 0x06004D48 RID: 19784 RVA: 0x002AA2A8 File Offset: 0x002A84A8
		[return: TupleElementNames(new string[]
		{
			"itemType",
			"itemTemplateId"
		})]
		private static ValueTuple<sbyte, short> GetItem(GameData.Domains.Character.Character character)
		{
			Inventory inventory = character.GetInventory();
			bool flag = inventory.Items.Count > 0;
			if (flag)
			{
				using (Dictionary<ItemKey, int>.Enumerator enumerator = inventory.Items.GetEnumerator())
				{
					enumerator.MoveNext();
					KeyValuePair<ItemKey, int> keyValuePair = enumerator.Current;
					ItemKey itemKey = keyValuePair.Key;
					return new ValueTuple<sbyte, short>(itemKey.ItemType, itemKey.TemplateId);
				}
			}
			ItemKey[] equipment = character.GetEquipment();
			for (int i = 0; i < 12; i++)
			{
				ItemKey itemKey2 = equipment[i];
				bool flag2 = !itemKey2.IsValid();
				if (!flag2)
				{
					return new ValueTuple<sbyte, short>(itemKey2.ItemType, itemKey2.TemplateId);
				}
			}
			return new ValueTuple<sbyte, short>(-1, -1);
		}

		// Token: 0x06004D49 RID: 19785 RVA: 0x002AA38C File Offset: 0x002A858C
		public LifeRecordDomain() : base(1)
		{
			this._lifeRecord = 0;
			this.OnInitializedDomainData();
		}

		// Token: 0x06004D4A RID: 19786 RVA: 0x002AA3C8 File Offset: 0x002A85C8
		private int GetLifeRecord()
		{
			return this._lifeRecord;
		}

		// Token: 0x06004D4B RID: 19787 RVA: 0x002AA3E0 File Offset: 0x002A85E0
		private void SetLifeRecord(int value, DataContext context)
		{
			this._lifeRecord = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, this.DataStates, LifeRecordDomain.CacheInfluences, context);
		}

		// Token: 0x06004D4C RID: 19788 RVA: 0x002AA3FD File Offset: 0x002A85FD
		public override void OnInitializeGameDataModule()
		{
			this.InitializeOnInitializeGameDataModule();
		}

		// Token: 0x06004D4D RID: 19789 RVA: 0x002AA407 File Offset: 0x002A8607
		public override void OnEnterNewWorld()
		{
			this.InitializeOnEnterNewWorld();
			this.InitializeInternalDataOfCollections();
		}

		// Token: 0x06004D4E RID: 19790 RVA: 0x002AA418 File Offset: 0x002A8618
		public override void OnLoadWorld()
		{
			this.InitializeInternalDataOfCollections();
			this.OnLoadedArchiveData();
			DomainManager.Global.CompleteLoading(13);
		}

		// Token: 0x06004D4F RID: 19791 RVA: 0x002AA438 File Offset: 0x002A8638
		public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (dataId != 0)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Not allow to get value of dataId: ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06004D50 RID: 19792 RVA: 0x002AA4AC File Offset: 0x002A86AC
		public override void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (dataId != 0)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06004D51 RID: 19793 RVA: 0x002AA520 File Offset: 0x002A8720
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
				if (num != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId);
				int beginIndex = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref beginIndex);
				int count = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref count);
				uint operationId = GameDataBridge.RecordPassthroughMethod(operation);
				this.Get(operationId, charId, beginIndex, count);
				result = -1;
				break;
			}
			case 1:
			{
				int argsCount2 = operation.ArgsCount;
				int num2 = argsCount2;
				if (num2 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId2);
				int startDate = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref startDate);
				int monthCount = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref monthCount);
				uint operationId2 = GameDataBridge.RecordPassthroughMethod(operation);
				this.GetByDate(operationId2, charId2, startDate, monthCount);
				result = -1;
				break;
			}
			case 2:
			{
				int argsCount3 = operation.ArgsCount;
				int num3 = argsCount3;
				if (num3 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId3);
				int count2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref count2);
				uint operationId3 = GameDataBridge.RecordPassthroughMethod(operation);
				this.GetLast(operationId3, charId3, count2);
				result = -1;
				break;
			}
			case 3:
			{
				int argsCount4 = operation.ArgsCount;
				int num4 = argsCount4;
				if (num4 != 4)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId4 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId4);
				int date = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref date);
				short recordType = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref recordType);
				int relatedCharId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref relatedCharId);
				uint operationId4 = GameDataBridge.RecordPassthroughMethod(operation);
				this.GetRelated(operationId4, charId4, date, recordType, relatedCharId);
				result = -1;
				break;
			}
			case 4:
			{
				int argsCount5 = operation.ArgsCount;
				int num5 = argsCount5;
				if (num5 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId5 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId5);
				uint operationId5 = GameDataBridge.RecordPassthroughMethod(operation);
				this.GetDead(operationId5, charId5);
				result = -1;
				break;
			}
			case 5:
			{
				int argsCount6 = operation.ArgsCount;
				int num6 = argsCount6;
				if (num6 != 2)
				{
					if (num6 != 3)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					string key = null;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key);
					RecordArgumentsRequest request = default(RecordArgumentsRequest);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref request);
					bool isDreamBack = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isDreamBack);
					ArgumentCollectionRenderArguments returnValue = this.GetRecordRenderInfoArguments(context, key, request, isDreamBack);
					result = Serializer.Serialize(returnValue, returnDataPool);
				}
				else
				{
					string key2 = null;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key2);
					RecordArgumentsRequest request2 = default(RecordArgumentsRequest);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref request2);
					ArgumentCollectionRenderArguments returnValue2 = this.GetRecordRenderInfoArguments(context, key2, request2, false);
					result = Serializer.Serialize(returnValue2, returnDataPool);
				}
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

		// Token: 0x06004D52 RID: 19794 RVA: 0x002AA934 File Offset: 0x002A8B34
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

		// Token: 0x06004D53 RID: 19795 RVA: 0x002AA97C File Offset: 0x002A8B7C
		public override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (dataId != 0)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification of dataId ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06004D54 RID: 19796 RVA: 0x002AA9F0 File Offset: 0x002A8BF0
		public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (dataId != 0)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of dataId ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06004D55 RID: 19797 RVA: 0x002AAA64 File Offset: 0x002A8C64
		public override bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (dataId != 0)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Not allow to verify modification state of dataId ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06004D56 RID: 19798 RVA: 0x002AAAD8 File Offset: 0x002A8CD8
		public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
		{
			ushort dataId = influence.TargetIndicator.DataId;
			ushort num = dataId;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (num != 0)
			{
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

		// Token: 0x06004D57 RID: 19799 RVA: 0x002AAB6C File Offset: 0x002A8D6C
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
			this.ProcessLifeRecordArchiveResponse(operation, pResult);
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
						DomainManager.Global.CompleteLoading(13);
					}
				}
			}
		}

		// Token: 0x06004D58 RID: 19800 RVA: 0x002AAC40 File Offset: 0x002A8E40
		private void InitializeInternalDataOfCollections()
		{
		}

		// Token: 0x0400155B RID: 5467
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x0400155C RID: 5468
		[DomainData(DomainDataType.SingleValue, false, false, false, false)]
		private int _lifeRecord;

		// Token: 0x0400155D RID: 5469
		private readonly LifeRecordCollection _currLifeRecords = new LifeRecordCollection();

		// Token: 0x0400155E RID: 5470
		private static Action<ReadonlyLifeRecords> _onReceiveCharacterLifeRecords;

		// Token: 0x0400155F RID: 5471
		private readonly List<short> _sourceRecordTemplateIds = new List<short>();

		// Token: 0x04001560 RID: 5472
		private readonly Dictionary<short, string> _templateId2Name = new Dictionary<short, string>();

		// Token: 0x04001561 RID: 5473
		private Type _lifeRecordCollectionType;

		// Token: 0x04001562 RID: 5474
		private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[1][];

		// Token: 0x04001563 RID: 5475
		private Queue<uint> _pendingLoadingOperationIds;
	}
}
