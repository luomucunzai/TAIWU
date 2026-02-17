using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Common.Binary;
using GameData.Common.SingleValueCollection;
using GameData.Dependencies;
using GameData.Domains.Character;
using GameData.Domains.Character.Display;
using GameData.Domains.Character.Relation;
using GameData.Domains.Extra;
using GameData.Domains.Global;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Domains.World;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using NLog;

namespace GameData.Domains.Information
{
	// Token: 0x02000679 RID: 1657
	[GameDataDomain(18)]
	public class InformationDomain : BaseGameDataDomain
	{
		// Token: 0x0600532B RID: 21291 RVA: 0x002D0D1F File Offset: 0x002CEF1F
		private void OnInitializedDomainData()
		{
			InformationDomain._StartEnemyRelationItem.Clear();
			InformationDomain._isOfflineUpdate = false;
			this._nextMetaDataId = 1;
		}

		// Token: 0x0600532C RID: 21292 RVA: 0x002D0D3A File Offset: 0x002CEF3A
		private void InitializeOnInitializeGameDataModule()
		{
		}

		// Token: 0x0600532D RID: 21293 RVA: 0x002D0D3D File Offset: 0x002CEF3D
		private void InitializeOnEnterNewWorld()
		{
		}

		// Token: 0x0600532E RID: 21294 RVA: 0x002D0D40 File Offset: 0x002CEF40
		private void OnLoadedArchiveData()
		{
		}

		// Token: 0x0600532F RID: 21295 RVA: 0x002D0D44 File Offset: 0x002CEF44
		public override void FixAbnormalDomainArchiveData(DataContext context)
		{
			Dictionary<sbyte, int> duplicatedLifeSkillTypes = new Dictionary<sbyte, int>();
			Dictionary<short, sbyte> levelNotExistInformation = new Dictionary<short, sbyte>();
			foreach (KeyValuePair<int, NormalInformationCollection> keyValuePair in this._information)
			{
				int num;
				NormalInformationCollection normalInformationCollection2;
				keyValuePair.Deconstruct(out num, out normalInformationCollection2);
				int charId = num;
				NormalInformationCollection normalInformationCollection = normalInformationCollection2;
				duplicatedLifeSkillTypes.Clear();
				levelNotExistInformation.Clear();
				List<NormalInformation> list = normalInformationCollection.GetList() as List<NormalInformation>;
				bool flag = list != null;
				if (flag)
				{
					for (int i = list.Count - 1; i >= 0; i--)
					{
						NormalInformation normalInformation = list[i];
						InformationItem config = Information.Instance[normalInformation.TemplateId];
						bool flag2 = config.Type == 2;
						if (flag2)
						{
							InformationInfoItem info = InformationInfo.Instance[config.InfoIds[(int)normalInformation.Level]];
							int count;
							bool flag3 = !duplicatedLifeSkillTypes.TryGetValue(info.LifeSkillType, out count);
							if (flag3)
							{
								duplicatedLifeSkillTypes.Add(info.LifeSkillType, count = 0);
							}
							duplicatedLifeSkillTypes[info.LifeSkillType] = count + 1;
						}
						bool flag4 = config.InfoIds[(int)normalInformation.Level] < 0 || levelNotExistInformation.ContainsKey(normalInformation.TemplateId);
						if (flag4)
						{
							bool flag5 = !levelNotExistInformation.ContainsKey(normalInformation.TemplateId);
							if (flag5)
							{
								sbyte correctLevel = 0;
								while ((int)correctLevel < config.InfoIds.Length)
								{
									bool flag6 = config.InfoIds[(int)correctLevel] >= 0;
									if (flag6)
									{
										break;
									}
									correctLevel += 1;
								}
								levelNotExistInformation.Add(normalInformation.TemplateId, correctLevel);
							}
							list.RemoveAt(i);
						}
					}
					bool flag7 = duplicatedLifeSkillTypes.Count > 0;
					if (flag7)
					{
						using (Dictionary<sbyte, int>.KeyCollection.Enumerator enumerator2 = duplicatedLifeSkillTypes.Keys.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								sbyte lifeSkillType = enumerator2.Current;
								bool flag8 = duplicatedLifeSkillTypes[lifeSkillType] > 1;
								if (flag8)
								{
									list.RemoveAll((NormalInformation inf) => InformationInfo.Instance[Information.Instance[inf.TemplateId].InfoIds[(int)inf.Level]].LifeSkillType == lifeSkillType);
								}
							}
						}
						GameData.Domains.Character.Character character;
						bool flag9 = DomainManager.Character.TryGetElement_Objects(charId, out character);
						if (flag9)
						{
							foreach (GameData.Domains.Character.LifeSkillItem lifeSkillItem in character.GetLearnedLifeSkills())
							{
								sbyte lifeSkillType2 = LifeSkill.Instance[lifeSkillItem.SkillTemplateId].Type;
								int count2;
								bool flag10 = lifeSkillItem.IsAllPagesRead() && duplicatedLifeSkillTypes.TryGetValue(lifeSkillType2, out count2) && count2 > 1;
								if (flag10)
								{
									this.GainLifeSkillInformationToCharacter(context, charId, lifeSkillType2);
								}
							}
						}
					}
					bool flag11 = duplicatedLifeSkillTypes.Count > 0 || levelNotExistInformation.Count > 0;
					if (flag11)
					{
						this.SetElement_Information(charId, normalInformationCollection, context);
					}
				}
			}
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			NormalInformationCollection collection = this.EnsureCharacterNormalInformationCollection(context, taiwuCharId);
			sbyte[] xiangshuAvatarIdTasksInOrder = DomainManager.World.GetXiangshuAvatarTasksInOrder();
			int j = 0;
			while (j < 9)
			{
				XiangshuAvatarTaskStatus status = DomainManager.World.GetElement_XiangshuAvatarTaskStatuses(j);
				bool flag12 = status.SwordTombStatus == 2;
				if (flag12)
				{
					bool shouldConsumableVersion = j != (int)xiangshuAvatarIdTasksInOrder[0];
					NormalInformation information = this.CalcSwordTombInformation(EInformationInfoSwordInformationType.SwordTombReal, shouldConsumableVersion, j);
					bool flag13 = shouldConsumableVersion;
					if (flag13)
					{
						bool flag14 = collection.GetList().Any(delegate(NormalInformation u)
						{
							InformationItem config3 = Information.Instance.GetItem(u.TemplateId);
							return config3 != null && config3.TransformId == information.TemplateId;
						});
						if (flag14)
						{
							goto IL_428;
						}
					}
					else
					{
						InformationItem config2 = Information.Instance.GetItem(information.TemplateId);
						foreach (NormalInformation u2 in collection.GetList().ToArray<NormalInformation>())
						{
							bool flag15 = config2.TransformId == u2.TemplateId;
							if (flag15)
							{
								this.DiscardNormalInformation(context, taiwuCharId, u2);
							}
						}
					}
					this.CheckAddNormalInformationToCharacter(context, taiwuCharId, information);
				}
				IL_428:
				j++;
				continue;
				goto IL_428;
			}
			bool isShouldWipe = false;
			try
			{
				this._swSecretInformation = Stopwatch.StartNew();
				Dictionary<int, List<int>> offsetRefMap;
				HashSet<int> multiRefOffsets;
				List<int> danglingOffsetList;
				this.CheckSecretInformationValidState(context, out offsetRefMap, out multiRefOffsets, out danglingOffsetList, true);
				this._swSecretInformation.Stop();
				string tag = "CheckSecretInformationValidState";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 1);
				defaultInterpolatedStringHandler.AppendLiteral("cost ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(this._swSecretInformation.ElapsedMilliseconds);
				defaultInterpolatedStringHandler.AppendLiteral(" ms");
				AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
				foreach (int offset in multiRefOffsets)
				{
					List<int> metaDataIdList;
					bool flag16 = !offsetRefMap.TryGetValue(offset, out metaDataIdList);
					if (!flag16)
					{
						for (int k = metaDataIdList.Count - 1; k >= 1; k--)
						{
							bool flag17 = danglingOffsetList.Count > 0;
							if (flag17)
							{
								int metaDataId = metaDataIdList[k];
								int redirectionIndex = 0;
								int redirectionOffset = danglingOffsetList[redirectionIndex];
								string tag2 = "FixAbnormalDomainArchiveData";
								defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 2);
								defaultInterpolatedStringHandler.AppendLiteral("secretinformation metaData[");
								defaultInterpolatedStringHandler.AppendFormatted<int>(metaDataId);
								defaultInterpolatedStringHandler.AppendLiteral("] redirection to offset[");
								defaultInterpolatedStringHandler.AppendFormatted<int>(redirectionOffset);
								defaultInterpolatedStringHandler.AppendLiteral("]");
								AdaptableLog.TagWarning(tag2, defaultInterpolatedStringHandler.ToStringAndClear(), false);
								this._secretInformationMetaData[metaDataId].SetOffset(redirectionOffset, context);
								danglingOffsetList.RemoveAt(redirectionIndex);
								metaDataIdList.RemoveAt(k);
							}
							else
							{
								int earliestMetaDataId = metaDataIdList[0];
								metaDataIdList.RemoveAt(0);
								foreach (KeyValuePair<int, SecretInformationCharacterDataCollection> charData in this._characterSecretInformation)
								{
									bool flag18 = charData.Value.Collection.ContainsKey(earliestMetaDataId);
									if (flag18)
									{
										this.DiscardSecretInformation(context, charData.Key, earliestMetaDataId);
									}
								}
								this._broadcastSecretInformation.Remove(earliestMetaDataId);
								this.SetBroadcastSecretInformation(this._broadcastSecretInformation, context);
								DomainManager.Extra.RemoveSecretInformationInBroadcastList(context, earliestMetaDataId);
								string tag3 = "FixAbnormalDomainArchiveData";
								defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 1);
								defaultInterpolatedStringHandler.AppendLiteral("secretinformation metaData[");
								defaultInterpolatedStringHandler.AppendFormatted<int>(earliestMetaDataId);
								defaultInterpolatedStringHandler.AppendLiteral("] cleared its all reference");
								AdaptableLog.TagWarning(tag3, defaultInterpolatedStringHandler.ToStringAndClear(), false);
								this.RemoveElement_SecretInformationMetaData(earliestMetaDataId);
								string tag4 = "FixAbnormalDomainArchiveData";
								defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 1);
								defaultInterpolatedStringHandler.AppendLiteral("secretinformation metaData[");
								defaultInterpolatedStringHandler.AppendFormatted<int>(earliestMetaDataId);
								defaultInterpolatedStringHandler.AppendLiteral("] has been removed");
								AdaptableLog.TagWarning(tag4, defaultInterpolatedStringHandler.ToStringAndClear(), false);
							}
						}
					}
				}
				bool flag19 = danglingOffsetList.Count > 100;
				if (flag19)
				{
					isShouldWipe = true;
				}
				else
				{
					bool flag20 = danglingOffsetList.Count > 0;
					if (flag20)
					{
						foreach (int offset2 in danglingOffsetList)
						{
							int nextId = this.GetAndIncreaseNextMetaDataId(context);
							SecretInformationMetaData metaData = new SecretInformationMetaData(nextId, offset2, -1);
							this.AddElement_SecretInformationMetaData(nextId, metaData);
							string tag5 = "FixAbnormalDomainArchiveData";
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(67, 2);
							defaultInterpolatedStringHandler.AppendLiteral("secretinformation metaData[");
							defaultInterpolatedStringHandler.AppendFormatted<int>(nextId);
							defaultInterpolatedStringHandler.AppendLiteral("] has been appended to dangling offset[");
							defaultInterpolatedStringHandler.AppendFormatted<int>(offset2);
							defaultInterpolatedStringHandler.AppendLiteral("]");
							AdaptableLog.TagWarning(tag5, defaultInterpolatedStringHandler.ToStringAndClear(), false);
						}
					}
				}
			}
			catch (NullReferenceException)
			{
				Stopwatch swSecretInformation = this._swSecretInformation;
				if (swSecretInformation != null)
				{
					swSecretInformation.Stop();
				}
				isShouldWipe = true;
			}
			bool flag21 = isShouldWipe;
			if (flag21)
			{
				this.SecretInformationWipeAll(context);
				AdaptableLog.TagWarning("FixAbnormalDomainArchiveData", PredefinedLog.Instance[0].Info, true);
			}
			this.RemoveAllNotExistBroadcastSecretInformation(context);
		}

		// Token: 0x06005330 RID: 21296 RVA: 0x002D1638 File Offset: 0x002CF838
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal NormalInformationCollection EnsureCharacterNormalInformationCollection(DataContext context, int characterId)
		{
			NormalInformationCollection collection;
			bool flag = !this.TryGetElement_Information(characterId, out collection);
			if (flag)
			{
				this.AddElement_Information(characterId, collection = new NormalInformationCollection(), context);
			}
			return collection;
		}

		// Token: 0x06005331 RID: 21297 RVA: 0x002D166B File Offset: 0x002CF86B
		public void RemoveCharacterAllInformation(DataContext context, int characterId)
		{
			this.RemoveElement_Information(characterId, context);
			this.RemoveElement_CharacterSecretInformation(characterId, context);
			DomainManager.Extra.ClearCharacterAllSecretInformationUsedCount(context, characterId);
		}

		// Token: 0x06005332 RID: 21298 RVA: 0x002D1690 File Offset: 0x002CF890
		[DomainMethod]
		public NormalInformationCollection GetCharacterNormalInformation(int characterId)
		{
			NormalInformationCollection collection;
			bool flag = this.TryGetElement_Information(characterId, out collection);
			NormalInformationCollection result;
			if (flag)
			{
				result = collection;
			}
			else
			{
				result = new NormalInformationCollection();
			}
			return result;
		}

		// Token: 0x06005333 RID: 21299 RVA: 0x002D16B8 File Offset: 0x002CF8B8
		[DomainMethod]
		public void AddNormalInformationToCharacter(DataContext context, int characterId, NormalInformation information)
		{
			this.CheckAddNormalInformationToCharacter(context, characterId, information);
		}

		// Token: 0x06005334 RID: 21300 RVA: 0x002D16C4 File Offset: 0x002CF8C4
		public bool CheckAddNormalInformationToCharacter(DataContext context, int characterId, NormalInformation information)
		{
			NormalInformationCollection collection = this.EnsureCharacterNormalInformationCollection(context, characterId);
			IList<NormalInformation> list = collection.GetList();
			foreach (NormalInformation element in list)
			{
				bool flag = element.TemplateId == information.TemplateId && element.Level == information.Level;
				if (flag)
				{
					return false;
				}
			}
			list.Add(information);
			bool flag2 = characterId == DomainManager.Taiwu.GetTaiwuCharId();
			if (flag2)
			{
				this._taiwuReceivedNormalInformationInMonth.Add(information);
				DomainManager.Taiwu.AddLegacyPoint(context, 34, 100);
				InformationItem config = Information.Instance[information.TemplateId];
				sbyte type = config.Type;
				sbyte b = type;
				if (b > 1)
				{
					if (b == 3)
					{
						ProfessionFormulaItem formula = ProfessionFormula.Instance[78];
						int addSeniority = formula.Calculate((int)information.Level);
						DomainManager.Extra.ChangeProfessionSeniority(context, 12, addSeniority, true, false);
					}
				}
				else
				{
					ProfessionFormulaItem formula2 = ProfessionFormula.Instance[77];
					int addSeniority2 = formula2.Calculate((int)information.Level);
					DomainManager.Extra.ChangeProfessionSeniority(context, 12, addSeniority2, true, false);
				}
			}
			this.SetElement_Information(characterId, collection, context);
			return true;
		}

		// Token: 0x06005335 RID: 21301 RVA: 0x002D1830 File Offset: 0x002CFA30
		[DomainMethod]
		public void DeleteTmpInformation(DataContext context)
		{
			this._taiwuReceivedInformation.Clear();
			this._taiwuTmpInformation.Clear();
		}

		// Token: 0x06005336 RID: 21302 RVA: 0x002D184C File Offset: 0x002CFA4C
		public void DiscardNormalInformation(DataContext context, int characterId, NormalInformation information)
		{
			NormalInformationCollection normalInformationCollection;
			bool flag = this.TryGetElement_Information(characterId, out normalInformationCollection);
			if (flag)
			{
				normalInformationCollection.GetList().Remove(information);
				normalInformationCollection.ClearUsedCountData(information);
				this.SetElement_Information(characterId, normalInformationCollection, context);
			}
		}

		// Token: 0x06005337 RID: 21303 RVA: 0x002D1888 File Offset: 0x002CFA88
		[DomainMethod]
		public int GetNormalInformationUsedCount(int characterId, NormalInformation information)
		{
			NormalInformationCollection normalInformationCollection;
			bool flag = this.TryGetElement_Information(characterId, out normalInformationCollection);
			int result;
			if (flag)
			{
				result = (int)normalInformationCollection.GetUsedCount(information);
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06005338 RID: 21304 RVA: 0x002D18B4 File Offset: 0x002CFAB4
		[DomainMethod]
		public IntPair GetNormalInformationUsedCountAndMax(int characterId, NormalInformation information)
		{
			NormalInformationCollection normalInformationCollection;
			bool flag = this.TryGetElement_Information(characterId, out normalInformationCollection);
			IntPair result;
			if (flag)
			{
				result = new IntPair((int)normalInformationCollection.GetUsedCount(information), (int)normalInformationCollection.GetUsedCountMax(information));
			}
			else
			{
				result = new IntPair(0, 0);
			}
			return result;
		}

		// Token: 0x06005339 RID: 21305 RVA: 0x002D18F4 File Offset: 0x002CFAF4
		public void TransformNormalInformation(DataContext context, int charId, NormalInformation normalInformation)
		{
			short informationId = Information.Instance[normalInformation.TemplateId].TransformId;
			NormalInformationCollection collection = this.EnsureCharacterNormalInformationCollection(context, charId);
			sbyte usedCount = collection.GetUsedCount(normalInformation);
			this.DiscardNormalInformation(context, charId, normalInformation);
			bool flag = informationId >= 0;
			if (flag)
			{
				NormalInformation newInformation = new NormalInformation(informationId, normalInformation.Level);
				this.AddNormalInformationToCharacter(context, charId, newInformation);
				collection.SetUsedCount(newInformation, usedCount);
				this.SetElement_Information(charId, collection, context);
				List<int> allCharIds = this._information.Keys.ToList<int>();
				foreach (int targetCharId in allCharIds)
				{
					NormalInformationCollection targetCollection;
					bool flag2 = this.TryGetElement_Information(targetCharId, out targetCollection);
					if (flag2)
					{
						sbyte receivedCount;
						bool flag3 = targetCollection.ReceivedCounts.Remove(normalInformation.TemplateId, out receivedCount);
						if (flag3)
						{
							targetCollection.ReceivedCounts.TryAdd(informationId, receivedCount);
							this.SetElement_Information(targetCharId, targetCollection, context);
						}
					}
				}
			}
		}

		// Token: 0x0600533A RID: 21306 RVA: 0x002D1A10 File Offset: 0x002CFC10
		public NormalInformation CalcSwordTombInformation(EInformationInfoSwordInformationType type, bool isConsume, int xiangshuAvatarId)
		{
			NormalInformation result;
			switch (type)
			{
			case EInformationInfoSwordInformationType.SwordTombFake:
				result = new NormalInformation((short)(xiangshuAvatarId + 80), 4);
				break;
			case EInformationInfoSwordInformationType.SwordTombReal:
				result = new NormalInformation((short)(xiangshuAvatarId + (isConsume ? 89 : 98)), 8);
				break;
			case EInformationInfoSwordInformationType.SwordTombIntell:
				result = new NormalInformation((short)(xiangshuAvatarId + 111), 8);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 1);
				defaultInterpolatedStringHandler.AppendLiteral("unsupported information type: ");
				defaultInterpolatedStringHandler.AppendFormatted<EInformationInfoSwordInformationType>(type);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x0600533B RID: 21307 RVA: 0x002D1A98 File Offset: 0x002CFC98
		public void RegisterInformation(int charId, DataContext context)
		{
			CharacterDomain characterDomain = DomainManager.Character;
			GameData.Domains.Character.Character character = characterDomain.GetElement_Objects(charId);
			foreach (GameData.Domains.Character.LifeSkillItem lifeSkillItem in character.GetLearnedLifeSkills())
			{
				bool flag = lifeSkillItem.IsAllPagesRead();
				if (flag)
				{
					this.GainLifeSkillInformationToCharacter(context, charId, LifeSkill.Instance[lifeSkillItem.SkillTemplateId].Type);
				}
			}
			Location location = character.GetLocation();
			bool flag2 = location.IsValid();
			if (flag2)
			{
				MapBlockData block = DomainManager.Map.GetBlock(location);
				short templateId = block.GetConfig().InformationTemplateId;
				bool flag3 = templateId >= 0;
				if (flag3)
				{
					OrganizationInfo orgInfo = character.GetOrganizationInfo();
					bool flag4 = orgInfo.Grade > 0;
					if (flag4)
					{
						this.AddNormalInformationToCharacter(context, charId, new NormalInformation(templateId, orgInfo.Grade));
					}
				}
			}
			MapDomain mapDomain = DomainManager.Map;
			OrganizationDomain orgDomain = DomainManager.Organization;
			OrganizationInfo orgInfo2 = character.GetOrganizationInfo();
			bool flag5 = orgInfo2.SettlementId >= 0;
			if (flag5)
			{
				Location location2 = orgDomain.GetSettlement(orgInfo2.SettlementId).GetLocation();
				bool flag6 = location2.IsValid();
				if (flag6)
				{
					short templateId2 = mapDomain.GetBlock(location2).GetConfig().InformationTemplateId;
					bool flag7 = templateId2 >= 0;
					if (flag7)
					{
						this.AddNormalInformationToCharacter(context, charId, new NormalInformation(templateId2, orgInfo2.Grade));
					}
				}
			}
			this.AddElement_CharacterSecretInformation(charId, new SecretInformationCharacterDataCollection(), context);
		}

		// Token: 0x0600533C RID: 21308 RVA: 0x002D1C34 File Offset: 0x002CFE34
		public void TransferInformation(int sourceCharId, int targetCharId, DataContext context)
		{
			this.RemoveCharacterAllInformation(context, targetCharId);
			NormalInformationCollection normalInformationCollection;
			bool flag = this.TryGetElement_Information(sourceCharId, out normalInformationCollection);
			if (flag)
			{
				this.AddElement_Information(targetCharId, normalInformationCollection, context);
				this.RemoveElement_Information(sourceCharId, context);
			}
			SecretInformationCharacterDataCollection secretInformationCharacterDataCollection;
			bool flag2 = this.TryGetElement_CharacterSecretInformation(sourceCharId, out secretInformationCharacterDataCollection);
			if (flag2)
			{
				this.AddElement_CharacterSecretInformation(targetCharId, secretInformationCharacterDataCollection, context);
				this.RemoveElement_CharacterSecretInformation(sourceCharId, context);
			}
			DomainManager.Extra.TransferCharacterAllSecretInformationUsedCount(context, sourceCharId, targetCharId);
		}

		// Token: 0x0600533D RID: 21309 RVA: 0x002D1C9D File Offset: 0x002CFE9D
		public void ClearedTaiwuReceivedSecretInformationInMonth()
		{
			this._taiwuReceivedSecretInformationInMonth.Clear();
		}

		// Token: 0x0600533E RID: 21310 RVA: 0x002D1CAC File Offset: 0x002CFEAC
		public void ProcessAdvanceMonth(DataContext context)
		{
			InformationDomain._isOfflineUpdate = true;
			bool flag = DomainManager.World.GetCurrMonthInYear() == 0;
			if (flag)
			{
				DomainManager.Extra.ClearPackReceivedNormalInformationInLastMonth(context);
			}
			DomainManager.Extra.PackReceivedNormalInformationInLastMonth(context, this._taiwuReceivedNormalInformationInMonth);
			this._taiwuReceivedNormalInformationInMonth.Clear();
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			this._swSecretInformation = Stopwatch.StartNew();
			this.MakeSettlementsInformation(context);
			this._swSecretInformation.Stop();
			Logger logger = InformationDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 2);
			defaultInterpolatedStringHandler.AppendFormatted("MakeSettlementsInformation");
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<long>(this._swSecretInformation.ElapsedMilliseconds);
			defaultInterpolatedStringHandler.AppendLiteral(" ms");
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			this._swSecretInformation = Stopwatch.StartNew();
			List<int> characterIds = ObjectPool<List<int>>.Instance.Get();
			characterIds.Clear();
			characterIds.AddRange(this._characterSecretInformation.Keys);
			characterIds.Remove(taiwuCharId);
			foreach (int characterId in characterIds)
			{
				this.PlanDisseminateSecretInformation(context, characterId);
			}
			ObjectPool<List<int>>.Instance.Return(characterIds);
			this._swSecretInformation.Stop();
			Logger logger2 = InformationDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 2);
			defaultInterpolatedStringHandler.AppendFormatted("PlanDisseminateSecretInformation");
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<long>(this._swSecretInformation.ElapsedMilliseconds);
			defaultInterpolatedStringHandler.AppendLiteral(" ms");
			logger2.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			foreach (int charId in DomainManager.Extra.GetSecretInformationShopCharacterKeys())
			{
				SecretInformationShopCharacterData data = DomainManager.Extra.AddOrGetSecretInformationShopCharacterData(context, charId, true);
				data.CollectedSecretInformationIds.Clear();
				DomainManager.Extra.SetSecretInformationShopCharacterData(context, charId, data);
			}
			this.RemoveSenselessSecretInformation(context);
			this.SetTaiwuReceivedNormalInformationInMonth(this._taiwuReceivedNormalInformationInMonth, context);
			this.SetTaiwuReceivedSecretInformationInMonth(this._taiwuReceivedSecretInformationInMonth, context);
			foreach (int index in InformationDomain._offlineIndicesCharacterData)
			{
				SecretInformationCharacterDataCollection element;
				bool flag2 = this.TryGetElement_CharacterSecretInformation(index, out element);
				if (flag2)
				{
					this.SetElement_CharacterSecretInformation(index, element, context);
				}
				else
				{
					this.RemoveElement_CharacterSecretInformation(index, context);
				}
			}
			InformationDomain._offlineIndicesCharacterData.Clear();
			foreach (int index2 in InformationDomain._offlineIndicesMetaDataOffset)
			{
				SecretInformationMetaData element2;
				bool flag3 = this.TryGetElement_SecretInformationMetaData(index2, out element2);
				if (flag3)
				{
					element2.SetOffset(element2.GetOffset(), context);
				}
				else
				{
					this.RemoveElement_SecretInformationMetaData(index2);
				}
			}
			InformationDomain._offlineIndicesMetaDataOffset.Clear();
			foreach (int index3 in InformationDomain._offlineIndicesMetaDataCharacterDisseminationData)
			{
				SecretInformationMetaData element3;
				bool flag4 = this.TryGetElement_SecretInformationMetaData(index3, out element3);
				if (flag4)
				{
					element3.SetDisseminationData(element3.GetDisseminationData(), context);
				}
				else
				{
					this.RemoveElement_SecretInformationMetaData(index3);
				}
			}
			InformationDomain._offlineIndicesMetaDataCharacterDisseminationData.Clear();
			this.SetBroadcastSecretInformation(this.GetBroadcastSecretInformation(), context);
			foreach (InformationDomain.SecretInformationStartEnemyRelationItem item in InformationDomain._StartEnemyRelationItem)
			{
				GameData.Domains.Character.Character character;
				GameData.Domains.Character.Character targetChar;
				bool flag5 = DomainManager.Character.TryGetElement_Objects(item.CharacterId, out character) && DomainManager.Character.TryGetElement_Objects(item.TargetId, out targetChar) && Config.Character.Instance[character.GetTemplateId()].CreatingType == 1 && Config.Character.Instance[targetChar.GetTemplateId()].CreatingType == 1;
				if (flag5)
				{
					GameData.Domains.Character.Character.ApplyAddRelation_Enemy(context, character, targetChar, item.CharacterId == DomainManager.Taiwu.GetTaiwuCharId(), 5, new CharacterBecomeEnemyInfo(character)
					{
						SecretInformationTemplateId = item.SecretInformationTemplateId,
						Location = targetChar.GetValidLocation()
					});
				}
			}
			InformationDomain._StartEnemyRelationItem.Clear();
			InformationDomain._isOfflineUpdate = false;
		}

		// Token: 0x0600533F RID: 21311 RVA: 0x002D2180 File Offset: 0x002D0380
		public override void PackCrossArchiveGameData(CrossArchiveGameData crossArchiveGameData)
		{
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			NormalInformationCollection normalInformationCollection;
			bool flag = this._information.TryGetValue(taiwuCharId, out normalInformationCollection);
			if (flag)
			{
				crossArchiveGameData.NormalInformation = normalInformationCollection;
			}
		}

		// Token: 0x06005340 RID: 21312 RVA: 0x002D21B4 File Offset: 0x002D03B4
		public override void UnpackCrossArchiveGameData(DataContext context, CrossArchiveGameData crossArchiveGameData)
		{
			bool isShouldWipe = false;
			try
			{
				Dictionary<int, List<int>> offsetRefMap;
				HashSet<int> multiRefOffsets;
				List<int> danglingOffsetList;
				this.CheckSecretInformationValidState(context, out offsetRefMap, out multiRefOffsets, out danglingOffsetList, true);
				bool flag = multiRefOffsets.Count > 0 || danglingOffsetList.Count > 0;
				if (flag)
				{
					isShouldWipe = true;
				}
			}
			catch (NullReferenceException)
			{
				isShouldWipe = true;
			}
			bool flag2 = isShouldWipe;
			if (flag2)
			{
				this.SecretInformationWipeAll(context);
				AdaptableLog.TagWarning("UnpackCrossArchiveGameData", PredefinedLog.Instance[0].Info, true);
			}
		}

		// Token: 0x06005341 RID: 21313 RVA: 0x002D2238 File Offset: 0x002D0438
		public void UnpackCrossArchiveGameData_NormalInformation(DataContext context, CrossArchiveGameData crossArchiveGameData)
		{
			bool flag = crossArchiveGameData.NormalInformation == null;
			if (!flag)
			{
				int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
				foreach (NormalInformation information in crossArchiveGameData.NormalInformation.GetList())
				{
					InformationItem config = Information.Instance[information.TemplateId];
					bool flag2 = config.Type == 2;
					if (flag2)
					{
						bool flag3 = config.InfoIds.CheckIndex((int)information.Level);
						if (flag3)
						{
							InformationInfoItem info = InformationInfo.Instance.GetItem(config.InfoIds[(int)information.Level]);
							bool flag4 = info != null;
							if (flag4)
							{
								for (int level = 0; level < (int)(information.Level + 1); level++)
								{
									this.GainLifeSkillInformationToCharacter(context, taiwuCharId, info.LifeSkillType);
								}
							}
						}
					}
					else
					{
						bool flag5 = config.Type == 5;
						if (!flag5)
						{
							this.AddNormalInformationToCharacter(context, taiwuCharId, information);
						}
					}
				}
			}
		}

		// Token: 0x06005342 RID: 21314 RVA: 0x002D2360 File Offset: 0x002D0560
		public void MakeSettlementsInformation(DataContext context)
		{
			InformationDomain.<>c__DisplayClass40_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.context = context;
			MapDomain mapDomain = DomainManager.Map;
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			CS$<>8__locals1.taiwuLocation = taiwu.GetLocation();
			CS$<>8__locals1.taiwuGroups = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
			for (short areaId = 0; areaId < 45; areaId += 1)
			{
				short settlementMainBlockId = mapDomain.GetMainSettlementMainBlockId(areaId);
				bool flag = settlementMainBlockId < 0;
				if (!flag)
				{
					MapBlockData settlementMainBlock = mapDomain.GetBlock(areaId, settlementMainBlockId);
					this.<MakeSettlementsInformation>g__ProcessMapBlock|40_2(settlementMainBlock, ref CS$<>8__locals1);
				}
			}
		}

		// Token: 0x06005343 RID: 21315 RVA: 0x002D2400 File Offset: 0x002D0600
		internal void GiveOrUpgradeWesternRegionInformation(DataContext context, int charId, short westernRegionId)
		{
			NormalInformationCollection collection = this.EnsureCharacterNormalInformationCollection(context, charId);
			IList<NormalInformation> list = collection.GetList();
			for (int i = 0; i < list.Count; i++)
			{
				NormalInformation information = list[i];
				InformationItem config = Information.Instance.GetItem(information.TemplateId);
				InformationInfoItem info = InformationInfo.Instance.GetItem(config.InfoIds[(int)information.Level]);
				sbyte upgradedLevel = information.Level + 1;
				bool flag = info.WesternRegionId == westernRegionId && upgradedLevel <= 8 && config.InfoIds.CheckIndex((int)upgradedLevel) && config.InfoIds[(int)upgradedLevel] >= 0;
				if (flag)
				{
					this.AddNormalInformationToCharacter(context, charId, new NormalInformation(config.TemplateId, upgradedLevel));
					return;
				}
			}
			foreach (InformationItem config2 in ((IEnumerable<InformationItem>)Information.Instance))
			{
				for (sbyte level = 0; level <= 8; level += 1)
				{
					InformationInfoItem info2 = InformationInfo.Instance.GetItem(config2.InfoIds[(int)level]);
					bool flag2 = info2 != null && info2.WesternRegionId == westernRegionId;
					if (flag2)
					{
						this.AddNormalInformationToCharacter(context, charId, new NormalInformation(config2.TemplateId, level));
						return;
					}
				}
			}
		}

		// Token: 0x06005344 RID: 21316 RVA: 0x002D257C File Offset: 0x002D077C
		internal void GiveProfessionInformation(DataContext context, int charId, int professionId)
		{
			NormalInformation information;
			bool flag = this.CalcProfessionInformation(professionId, out information);
			if (flag)
			{
				bool flag2 = !this.CheckAddNormalInformationToCharacter(context, charId, information);
				if (flag2)
				{
					NormalInformationCollection collection = this.EnsureCharacterNormalInformationCollection(context, charId);
					collection.SetRemainUsableCount(information, (sbyte)((int)collection.GetRemainUsableCount(information) + GlobalConfig.Instance.NormalInformationDefaultCostableMaxUseCount));
					DomainManager.Information.SetNormalInformationCharacterDataModified(context, charId);
				}
			}
		}

		// Token: 0x06005345 RID: 21317 RVA: 0x002D25DC File Offset: 0x002D07DC
		internal void FixOldProfessionInformation(DataContext context, ProfessionData professionData, int charId)
		{
			ProfessionItem professionCfg = professionData.GetConfig();
			for (int i = 0; i <= professionCfg.ProfessionSkills.Length; i++)
			{
				bool flag = professionData.IsSkillUnlocked(i);
				if (flag)
				{
					this.GiveProfessionInformation(context, charId, professionData.TemplateId);
				}
			}
		}

		// Token: 0x06005346 RID: 21318 RVA: 0x002D2628 File Offset: 0x002D0828
		internal void GiveRemainUsedCountInformation(DataContext context, int charId, NormalInformation normalInformation)
		{
			InformationItem config = Information.Instance[normalInformation.TemplateId];
			Tester.Assert(!config.UsedCountWithMax, "");
			NormalInformationCollection collection = this.EnsureCharacterNormalInformationCollection(context, charId);
			bool flag = this.CheckAddNormalInformationToCharacter(context, charId, normalInformation);
			if (flag)
			{
				collection.SetRemainUsableCount(normalInformation, 1);
			}
			else
			{
				collection.SetRemainUsableCount(normalInformation, collection.GetRemainUsableCount(normalInformation) + 1);
			}
			DomainManager.Information.SetNormalInformationCharacterDataModified(context, charId);
		}

		// Token: 0x06005347 RID: 21319 RVA: 0x002D26A0 File Offset: 0x002D08A0
		internal void GiveProfessionInformationRemainUsableCount(DataContext context, int charId, int professionId, int oldExtraSeniority, int nowExtraSeniority)
		{
			ProfessionItem professionConfig = Profession.Instance.GetItem(professionId);
			for (int i = 0; i < professionConfig.ProfessionSkills.Length + 1; i++)
			{
				int professionSkillId = professionConfig.ProfessionSkills.CheckIndex(i) ? professionConfig.ProfessionSkills[i] : professionConfig.ExtraProfessionSkill;
				ProfessionSkillItem professionSkillConfig = ProfessionSkill.Instance.GetItem(professionSkillId);
				bool flag = professionSkillConfig != null;
				if (flag)
				{
					int line = (int)GlobalConfig.Instance.GiveProfessionInformationFactorWithExtraSeniority * GameData.Domains.Taiwu.Profession.SharedMethods.GetSkillUnlockSeniority(professionSkillId) / 100;
					bool checkPointNormal = oldExtraSeniority < line && nowExtraSeniority >= line;
					bool checkPointOverline = line == 1500000 && nowExtraSeniority < oldExtraSeniority;
					NormalInformation information;
					bool flag2 = (checkPointNormal || checkPointOverline) && DomainManager.Information.CalcProfessionInformation(professionId, out information);
					if (flag2)
					{
						this.GiveRemainUsedCountInformation(context, charId, information);
					}
				}
			}
		}

		// Token: 0x06005348 RID: 21320 RVA: 0x002D277C File Offset: 0x002D097C
		internal bool CalcProfessionInformation(int professionId, out NormalInformation information)
		{
			foreach (InformationItem config in ((IEnumerable<InformationItem>)Information.Instance))
			{
				for (sbyte level = 0; level <= 8; level += 1)
				{
					InformationInfoItem info = InformationInfo.Instance.GetItem(config.InfoIds[(int)level]);
					bool flag = info != null && (int)info.Profession == professionId;
					if (flag)
					{
						information = new NormalInformation(config.TemplateId, level);
						return true;
					}
				}
			}
			information = default(NormalInformation);
			return false;
		}

		// Token: 0x06005349 RID: 21321 RVA: 0x002D282C File Offset: 0x002D0A2C
		public bool GainRandomSettlementInformationByStateIdToCharacter(DataContext context, sbyte grade, int characterId, sbyte stateId)
		{
			List<short> areaIds = new List<short>();
			DomainManager.Map.GetAllAreaInState(stateId, areaIds);
			List<short> randomPool = new List<short>();
			foreach (short areaId in areaIds)
			{
				MapAreaData area = DomainManager.Map.GetElement_Areas((int)areaId);
				SettlementInfo[] settlementInfos = area.SettlementInfos;
				for (int j = 0; j < settlementInfos.Length; j++)
				{
					SettlementInfo settlementInfo = settlementInfos[j];
					Func<short, bool> <>9__2;
					randomPool.AddRange(from i in Information.Instance.Where(delegate(InformationItem i)
					{
						bool result;
						if (i.IsGeneral)
						{
							IEnumerable<short> infoIds = i.InfoIds;
							Func<short, bool> predicate;
							if ((predicate = <>9__2) == null)
							{
								predicate = (<>9__2 = delegate(short infoId)
								{
									InformationInfoItem informationInfoItem = InformationInfo.Instance[infoId];
									sbyte? b = (informationInfoItem != null) ? new sbyte?(informationInfoItem.Oraganization) : null;
									int? num = (b != null) ? new int?((int)b.GetValueOrDefault()) : null;
									int orgTemplateId = (int)settlementInfo.OrgTemplateId;
									return (num.GetValueOrDefault() == orgTemplateId & num != null) && settlementInfo.OrgTemplateId >= 0;
								});
							}
							result = infoIds.Any(predicate);
						}
						else
						{
							result = false;
						}
						return result;
					})
					select i.TemplateId);
				}
			}
			bool flag = randomPool.Count <= 0;
			return !flag && this.CheckAddNormalInformationToCharacter(context, characterId, new NormalInformation(randomPool.GetRandom(context.Random), grade));
		}

		// Token: 0x0600534A RID: 21322 RVA: 0x002D2954 File Offset: 0x002D0B54
		public void GainLifeSkillInformationToCharacter(DataContext context, int characterId, sbyte lifeSkillType)
		{
			NormalInformationCollection collection = this.EnsureCharacterNormalInformationCollection(context, characterId);
			IList<NormalInformation> list = collection.GetList();
			short informationTemplateId = Config.LifeSkillType.Instance[lifeSkillType].InformationTemplateId;
			bool flag = informationTemplateId < 0;
			if (!flag)
			{
				int i = 0;
				int count = list.Count;
				while (i < count)
				{
					NormalInformation element = list[i];
					bool flag2 = element.TemplateId == informationTemplateId;
					if (flag2)
					{
						element.UpdateLevel(element.Level + 1);
						list[i] = element;
						this.SetElement_Information(characterId, collection, context);
						return;
					}
					i++;
				}
				this.AddNormalInformationToCharacter(context, characterId, new NormalInformation(informationTemplateId, 0));
			}
		}

		// Token: 0x0600534B RID: 21323 RVA: 0x002D2A04 File Offset: 0x002D0C04
		[DomainMethod]
		public int GmCmd_CreateSecretInformationByCharacterIds(DataContext context, string templateDefKeyName, List<int> charIds)
		{
			SecretInformationCollection collection = this.GetSecretInformationCollection();
			Type t = collection.GetType();
			MethodInfo creator = t.GetMethod("Add" + templateDefKeyName);
			bool flag = creator != null;
			if (flag)
			{
				short templateId = (short)typeof(SecretInformation.DefKey).GetField(templateDefKeyName, BindingFlags.Static | BindingFlags.Public).GetValue(null);
				SecretInformationItem config = SecretInformation.Instance[templateId];
				bool flag2 = config.Parameters != null;
				if (flag2)
				{
					object[] args = new object[config.Parameters.Count((string p) => !string.IsNullOrEmpty(p))];
					int i = 0;
					int count = args.Length;
					while (i < count)
					{
						string text = config.Parameters[i];
						string text2 = text;
						uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
						if (num <= 1539345862U)
						{
							if (num != 598792213U)
							{
								if (num != 1347445242U)
								{
									if (num == 1539345862U)
									{
										if (text2 == "Location")
										{
											List<Settlement> settlements = new List<Settlement>();
											DomainManager.Organization.GetAllCivilianSettlements(settlements);
											args[i] = settlements.GetRandom(context.Random).GetLocation();
										}
									}
								}
								else if (text2 == "CombatSkill")
								{
									args[i] = (short)context.Random.Next(CombatSkill.Instance.Count);
								}
							}
							else if (text2 == "Resource")
							{
								args[i] = (sbyte)context.Random.Next(8);
							}
						}
						else if (num <= 3651752933U)
						{
							if (num != 3197690557U)
							{
								if (num == 3651752933U)
								{
									if (text2 == "Integer")
									{
										args[i] = context.Random.Next();
									}
								}
							}
							else if (text2 == "ItemKey")
							{
								args[i] = (ulong)new ItemKey(10, 0, (short)context.Random.Next(Config.SkillBook.Instance.Count), -1);
							}
						}
						else if (num != 3966976176U)
						{
							if (num == 3992405204U)
							{
								if (text2 == "LifeSkill")
								{
									args[i] = (short)context.Random.Next(LifeSkill.Instance.Count);
								}
							}
						}
						else if (text2 == "Character")
						{
							bool flag3 = charIds.Count > 0;
							if (!flag3)
							{
								return -1;
							}
							args[i] = charIds[0];
							charIds.RemoveAt(0);
						}
						i++;
					}
					int dataOffset = (int)creator.Invoke(collection, args);
					return this.AddSecretInformationMetaData(context, dataOffset, false);
				}
			}
			return -1;
		}

		// Token: 0x0600534C RID: 21324 RVA: 0x002D2D38 File Offset: 0x002D0F38
		[DomainMethod]
		public bool GmCmd_MakeCharacterReceiveSecretInformation(DataContext context, int characterId, int metaDataId)
		{
			SecretInformationMetaData secretInformationMetaData;
			bool flag = this.TryGetElement_SecretInformationMetaData(metaDataId, out secretInformationMetaData);
			return flag && this.ReceiveSecretInformation(context, metaDataId, characterId, -1);
		}

		// Token: 0x0600534D RID: 21325 RVA: 0x002D2D66 File Offset: 0x002D0F66
		[DomainMethod]
		public void GmCmd_MakeSecretInformationBroadcast(DataContext context, int metaDataId, int sourceCharId = -1)
		{
			this.MakeSecretInformationBroadcastEffect(context, metaDataId, sourceCharId);
		}

		// Token: 0x0600534E RID: 21326 RVA: 0x002D2D74 File Offset: 0x002D0F74
		[DomainMethod]
		public int GmCmd_DisseminationSecretInformationToRandomCharacters(DataContext context, int secretId, int sourceCharId, int amount)
		{
			CharacterDomain domainCharacter = DomainManager.Character;
			InformationDomain domainInformation = DomainManager.Information;
			List<GameData.Domains.Character.Character> characters = new List<GameData.Domains.Character.Character>();
			int originAmount = amount;
			domainCharacter.FindIntelligentCharacters((GameData.Domains.Character.Character _) => true, characters);
			CollectionUtils.Shuffle<GameData.Domains.Character.Character>(context.Random, characters);
			foreach (GameData.Domains.Character.Character character in characters)
			{
				bool flag = amount <= 0;
				if (flag)
				{
					break;
				}
				bool flag2 = domainInformation.ReceiveSecretInformation(context, secretId, character.GetId(), sourceCharId);
				if (flag2)
				{
					amount--;
				}
			}
			return originAmount - amount;
		}

		// Token: 0x0600534F RID: 21327 RVA: 0x002D2E44 File Offset: 0x002D1044
		[DomainMethod]
		public List<CharacterDisplayDataWithInfo> GetCharacterDisplayDataWithInfoList(List<int> charList)
		{
			this._characterDisplayDataWithInfoList.Clear();
			foreach (int characterId in charList)
			{
				GameData.Domains.Character.Character character;
				bool flag = DomainManager.Character.TryGetElement_Objects(characterId, out character);
				if (flag)
				{
					CharacterInfoCountData characterInfoCountData = this.GetCharacterInfoCountData(characterId);
					bool flag2 = characterInfoCountData != null;
					if (flag2)
					{
						CharacterDisplayDataWithInfo characterDisplayDataWithInfo = new CharacterDisplayDataWithInfo
						{
							CharacterDisplayData = DomainManager.Character.GetCharacterDisplayData(characterId),
							CharacterInfoCountData = characterInfoCountData
						};
						this._characterDisplayDataWithInfoList.Add(characterDisplayDataWithInfo);
					}
				}
			}
			return this._characterDisplayDataWithInfoList;
		}

		// Token: 0x06005350 RID: 21328 RVA: 0x002D2F00 File Offset: 0x002D1100
		private CharacterInfoCountData GetCharacterInfoCountData(int characterId)
		{
			CharacterInfoCountData characterInfoDisplayData = null;
			SecretInformationCharacterDataCollection collection;
			bool flag = this.TryGetElement_CharacterSecretInformation(characterId, out collection);
			if (flag)
			{
				int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
				int holdInfoCount = collection.Collection.Count;
				int holdInfoTaiwuRelatedCount = collection.Collection.Count((KeyValuePair<int, SecretInformationCharacterData> p) => this.CheckSecretIsRelated(p.Key, taiwuCharId));
				bool flag2 = holdInfoCount > 0 || holdInfoTaiwuRelatedCount > 0;
				if (flag2)
				{
					characterInfoDisplayData = new CharacterInfoCountData
					{
						HoldInfoCount = holdInfoCount,
						HoldInfoTaiwuRelatedCount = holdInfoTaiwuRelatedCount
					};
				}
			}
			return characterInfoDisplayData;
		}

		// Token: 0x06005351 RID: 21329 RVA: 0x002D2F94 File Offset: 0x002D1194
		private bool CheckSecretIsRelated(int secretId, int charId)
		{
			SecretInformationMetaData metaData;
			bool flag = this.TryGetElement_SecretInformationMetaData(secretId, out metaData);
			if (flag)
			{
				this._eventArgBox.Clear();
				DomainManager.Information.MakeSecretInformationEventArgBox(metaData, this._eventArgBox);
				int mainCharId = this._eventArgBox.GetInt("arg0");
				int otherCharId = this._eventArgBox.GetInt("arg1");
				bool flag2 = mainCharId == charId || otherCharId == charId;
				if (flag2)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005352 RID: 21330 RVA: 0x002D300E File Offset: 0x002D120E
		[DomainMethod]
		public void PerformProfessionLiteratiSkill2(DataContext ctx, int secretInformationId)
		{
			ProfessionSkillHandle.LiteratiSkill_BroadcastModifiedSecretInformation(ctx, secretInformationId);
		}

		// Token: 0x06005353 RID: 21331 RVA: 0x002D3019 File Offset: 0x002D1219
		[DomainMethod]
		public void PerformProfessionLiteratiSkill3(DataContext ctx, NormalInformation normalInformation)
		{
			ProfessionSkillHandle.LiteratiSkill_AreaBroadcastNormalInformation(ctx, normalInformation);
		}

		// Token: 0x06005354 RID: 21332 RVA: 0x002D3024 File Offset: 0x002D1224
		public unsafe bool TryGetSecretInformationCharacter(int secretId, int index, out int characterId)
		{
			characterId = -1;
			SecretInformationMetaData secretInformationMetaData;
			bool flag = this.TryGetElement_SecretInformationMetaData(secretId, out secretInformationMetaData);
			if (flag)
			{
				SecretInformationCollection collection = this.GetSecretInformationCollection();
				int offset = secretInformationMetaData.GetOffset();
				byte[] array;
				byte* pRawData;
				if ((array = collection.GetRawData()) == null || array.Length == 0)
				{
					pRawData = null;
				}
				else
				{
					pRawData = &array[0];
				}
				byte* pCurrData = pRawData + offset;
				pCurrData++;
				int date = *(int*)pCurrData;
				pCurrData += 4;
				short templateId = *(short*)pCurrData;
				pCurrData += 2;
				SecretInformationItem config = SecretInformation.Instance[templateId];
				bool flag2 = config.Parameters != null;
				if (flag2)
				{
					int i = 0;
					int len = config.Parameters.Length;
					while (i < len)
					{
						string type = config.Parameters[i];
						bool flag3 = string.IsNullOrEmpty(type);
						if (!flag3)
						{
							string text = type;
							string text2 = text;
							uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
							if (num <= 1539345862U)
							{
								if (num != 598792213U)
								{
									if (num != 1347445242U)
									{
										if (num != 1539345862U)
										{
											goto IL_212;
										}
										if (!(text2 == "Location"))
										{
											goto IL_212;
										}
										pCurrData += 2;
										pCurrData += 2;
									}
									else
									{
										if (!(text2 == "CombatSkill"))
										{
											goto IL_212;
										}
										pCurrData += 2;
									}
								}
								else
								{
									if (!(text2 == "Resource"))
									{
										goto IL_212;
									}
									pCurrData++;
								}
							}
							else if (num <= 3651752933U)
							{
								if (num != 3197690557U)
								{
									if (num != 3651752933U)
									{
										goto IL_212;
									}
									if (!(text2 == "Integer"))
									{
										goto IL_212;
									}
									pCurrData += 4;
								}
								else
								{
									if (!(text2 == "ItemKey"))
									{
										goto IL_212;
									}
									pCurrData += 8;
								}
							}
							else if (num != 3966976176U)
							{
								if (num != 3992405204U)
								{
									goto IL_212;
								}
								if (!(text2 == "LifeSkill"))
								{
									goto IL_212;
								}
								pCurrData += 2;
							}
							else
							{
								if (!(text2 == "Character"))
								{
									goto IL_212;
								}
								int charId = *(int*)pCurrData;
								pCurrData += 4;
								bool flag4 = i == index;
								if (flag4)
								{
									characterId = charId;
									return true;
								}
							}
							goto IL_21B;
							IL_212:
							throw new NotImplementedException(type);
						}
						IL_21B:
						i++;
					}
				}
				array = null;
			}
			return false;
		}

		// Token: 0x06005355 RID: 21333 RVA: 0x002D326E File Offset: 0x002D146E
		public void SetSecretInformationCollectionModified(DataContext context)
		{
		}

		// Token: 0x06005356 RID: 21334 RVA: 0x002D3274 File Offset: 0x002D1474
		public IEnumerable<int> GetBroadcastSecretInformationIds()
		{
			return this.GetBroadcastSecretInformation().Concat(DomainManager.Extra.GetSecretInformationInBroadcastList());
		}

		// Token: 0x06005357 RID: 21335 RVA: 0x002D329C File Offset: 0x002D149C
		public bool IsSecretInformationInBroadcast(int metaDataId)
		{
			bool flag = this.GetBroadcastSecretInformation().Contains(metaDataId);
			return flag || DomainManager.Extra.IsSecretInformationInBroadcastList(metaDataId);
		}

		// Token: 0x06005358 RID: 21336 RVA: 0x002D32D0 File Offset: 0x002D14D0
		public short CalcSecretInformationTemplateId(SecretInformationMetaData secretInformationMetaData)
		{
			short result;
			this.CalcSecretInformationRemainingLifeTime(secretInformationMetaData, out result);
			return result;
		}

		// Token: 0x06005359 RID: 21337 RVA: 0x002D32F0 File Offset: 0x002D14F0
		public short CalcSecretInformationTemplateId(int metaDataId)
		{
			SecretInformationMetaData metaData;
			bool flag = this.TryGetElement_SecretInformationMetaData(metaDataId, out metaData);
			short result;
			if (flag)
			{
				result = this.CalcSecretInformationTemplateId(metaData);
			}
			else
			{
				result = -1;
			}
			return result;
		}

		// Token: 0x0600535A RID: 21338 RVA: 0x002D331C File Offset: 0x002D151C
		public SecretInformationItem GetSecretInformationConfig(int metaDataId)
		{
			SecretInformationMetaData metaData = this.GetElement_SecretInformationMetaData(metaDataId);
			short templateId = this.CalcSecretInformationTemplateId(metaData);
			return SecretInformation.Instance[templateId];
		}

		// Token: 0x0600535B RID: 21339 RVA: 0x002D334C File Offset: 0x002D154C
		public bool CharacterHasSecretInformationByTemplateId(int charId, short templateId)
		{
			SecretInformationCharacterDataCollection characterDataCollection;
			bool flag = DomainManager.Information.TryGetElement_CharacterSecretInformation(charId, out characterDataCollection);
			if (flag)
			{
				foreach (int metaDataId in characterDataCollection.Collection.Keys)
				{
					bool flag2 = this.MatchMetaDataIdAndTemplateId(metaDataId, templateId);
					if (flag2)
					{
						return true;
					}
				}
			}
			IEnumerable<int> broadCastList = DomainManager.Extra.GetSecretInformationInBroadcastList();
			foreach (int metaDataId2 in broadCastList)
			{
				bool flag3 = this.MatchMetaDataIdAndTemplateId(metaDataId2, templateId);
				if (flag3)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600535C RID: 21340 RVA: 0x002D3424 File Offset: 0x002D1624
		private bool MatchMetaDataIdAndTemplateId(int metaDataId, short templateId)
		{
			SecretInformationMetaData metaData;
			short infoTemplateId;
			return DomainManager.Information.TryGetElement_SecretInformationMetaData(metaDataId, out metaData) && DomainManager.Information.CalcSecretInformationRemainingLifeTime(metaData, out infoTemplateId) > 0 && infoTemplateId == templateId;
		}

		// Token: 0x0600535D RID: 21341 RVA: 0x002D345C File Offset: 0x002D165C
		public void GetSecretInformationOfCharacter(ICollection<int> result, int charId, bool includeBroadcast = true)
		{
			SecretInformationCharacterDataCollection characterDataCollection;
			bool flag = DomainManager.Information.TryGetElement_CharacterSecretInformation(charId, out characterDataCollection);
			if (flag)
			{
				foreach (int metaDataId in characterDataCollection.Collection.Keys)
				{
					SecretInformationMetaData metaData;
					short num;
					bool flag2 = DomainManager.Information.TryGetElement_SecretInformationMetaData(metaDataId, out metaData) && DomainManager.Information.CalcSecretInformationRemainingLifeTime(metaData, out num) > 0;
					if (flag2)
					{
						result.Add(metaDataId);
					}
				}
			}
			if (includeBroadcast)
			{
				IEnumerable<int> broadCastList = DomainManager.Extra.GetSecretInformationInBroadcastList();
				foreach (int id in broadCastList)
				{
					result.Add(id);
				}
			}
		}

		// Token: 0x0600535E RID: 21342 RVA: 0x002D3548 File Offset: 0x002D1748
		public List<int> GetSecretInformationOfCharacter(int charId, bool includeBroadcast = true)
		{
			List<int> result = new List<int>();
			this.GetSecretInformationOfCharacter(result, charId, includeBroadcast);
			return result;
		}

		// Token: 0x0600535F RID: 21343 RVA: 0x002D356C File Offset: 0x002D176C
		public sbyte CalcSecretInformationDisplaySize(SecretInformationMetaData metaData, IReadOnlyList<sbyte> informationSettings)
		{
			int score = 0;
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			SecretInformationItem config = SecretInformation.Instance[this.CalcSecretInformationTemplateId(metaData)];
			sbyte level = config.BlockSizeArgs[0];
			sbyte factor = config.BlockSizeArgs[(int)(1 + informationSettings[1])];
			score += (int)(level * factor);
			InformationDomain.<>c__DisplayClass76_0 CS$<>8__locals1;
			CS$<>8__locals1.characterIndex = 0;
			bool flag = config.Parameters != null;
			if (flag)
			{
				EventArgBox argBox = this.MakeSecretInformationEventArgBox(metaData, new EventArgBox());
				for (int i = 0; i < config.Parameters.Length; i++)
				{
					bool flag2 = config.Parameters[i].Equals("Character");
					if (flag2)
					{
						EventArgBox eventArgBox = argBox;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
						defaultInterpolatedStringHandler.AppendLiteral("arg");
						defaultInterpolatedStringHandler.AppendFormatted<int>(i);
						int charId = eventArgBox.GetInt(defaultInterpolatedStringHandler.ToStringAndClear());
						bool isSect = false;
						sbyte grade = 0;
						GameData.Domains.Character.Character character;
						bool flag3 = DomainManager.Character.TryGetElement_Objects(charId, out character);
						if (flag3)
						{
							sbyte orgTemplateId = character.GetOrganizationInfo().OrgTemplateId;
							isSect = (orgTemplateId >= 0 && Organization.Instance[orgTemplateId].IsSect);
							grade = character.GetOrganizationInfo().Grade;
						}
						else
						{
							DeadCharacter deadChar = DomainManager.Character.TryGetDeadCharacter(charId);
							bool flag4 = deadChar != null;
							if (flag4)
							{
								sbyte orgTemplateId2 = deadChar.OrganizationInfo.OrgTemplateId;
								isSect = (orgTemplateId2 >= 0 && Organization.Instance[orgTemplateId2].IsSect);
								grade = deadChar.OrganizationInfo.Grade;
							}
						}
						short[] charRelationTypeToValue = InformationDomain.<CalcSecretInformationDisplaySize>g__CharRelationTypeToValue|76_1(isSect, ref CS$<>8__locals1);
						sbyte relationIndex = RelationType.GetTypeId(0);
						bool flag5 = charId == taiwuCharId;
						if (flag5)
						{
							relationIndex = (sbyte)(charRelationTypeToValue.Length - 1);
						}
						else
						{
							RelatedCharacter relation;
							bool flag6 = DomainManager.Character.TryGetRelation(taiwuCharId, charId, out relation) && relation.RelationType != ushort.MaxValue;
							if (flag6)
							{
								for (ushort v = relation.RelationType; v < 17; v += 1)
								{
									ushort r = (ushort)(1 << (int)v);
									bool flag7 = (r & relation.RelationType) > 0;
									if (flag7)
									{
										relationIndex = RelationType.GetTypeId(r);
										break;
									}
								}
							}
						}
						score += (int)(InformationDomain.<CalcSecretInformationDisplaySize>g__CharGradeToValue|76_0(isSect, ref CS$<>8__locals1)[(int)grade] * (short)config.BlockSizeArgs[(int)(4 + informationSettings[2])]);
						score += (int)(charRelationTypeToValue[(int)relationIndex] * (short)config.BlockSizeArgs[(int)(7 + informationSettings[3])]);
						CS$<>8__locals1.characterIndex++;
					}
					else
					{
						bool flag8 = config.Parameters[i].Equals("ItemKey");
						if (flag8)
						{
							EventArgBox eventArgBox2 = argBox;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
							defaultInterpolatedStringHandler.AppendLiteral("arg");
							defaultInterpolatedStringHandler.AppendFormatted<int>(i);
							ItemKey itemKey;
							eventArgBox2.Get<ItemKey>(defaultInterpolatedStringHandler.ToStringAndClear(), out itemKey);
							bool flag9 = itemKey.ItemType >= 0 && itemKey.TemplateId >= 0;
							if (flag9)
							{
								score += (int)(GlobalConfig.SecretInformationDisplay_ItemGradeToValue[(int)ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId)] * (short)config.BlockSizeArgs[(int)(10 + informationSettings[4])]);
							}
						}
					}
				}
			}
			for (int j = 0; j < GlobalConfig.SecretInformationDisplay_SizeThresholds.Length; j++)
			{
				bool flag10 = score < (int)GlobalConfig.SecretInformationDisplay_SizeThresholds[j];
				if (flag10)
				{
					return (sbyte)j;
				}
			}
			return (sbyte)GlobalConfig.SecretInformationDisplay_SizeThresholds.Length;
		}

		// Token: 0x06005360 RID: 21344 RVA: 0x002D38DC File Offset: 0x002D1ADC
		public int CalcSecretInformationShopValue(SecretInformationMetaData metaData)
		{
			SecretInformationItem config = SecretInformation.Instance[this.CalcSecretInformationTemplateId(metaData)];
			int characterLevelSum = 0;
			EventArgBox argBox = this.MakeSecretInformationEventArgBox(metaData, new EventArgBox());
			for (int i = 0; i < config.Parameters.Length; i++)
			{
				bool flag = config.Parameters[i].Equals("Character");
				if (flag)
				{
					EventArgBox eventArgBox = argBox;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
					defaultInterpolatedStringHandler.AppendLiteral("arg");
					defaultInterpolatedStringHandler.AppendFormatted<int>(i);
					int charId = eventArgBox.GetInt(defaultInterpolatedStringHandler.ToStringAndClear());
					int grade = 0;
					GameData.Domains.Character.Character character;
					bool flag2 = DomainManager.Character.TryGetElement_Objects(charId, out character);
					if (flag2)
					{
						grade = (int)(character.GetOrganizationInfo().Grade + 1);
					}
					else
					{
						DeadCharacter deadCharacter;
						bool flag3 = DomainManager.Character.TryGetDeadCharacter(charId, out deadCharacter);
						if (flag3)
						{
							grade = (int)(deadCharacter.OrganizationInfo.Grade + 1);
						}
					}
					characterLevelSum += grade;
					bool flag4 = i == 1 || i == 2;
					if (flag4)
					{
						characterLevelSum += grade;
					}
				}
			}
			return (int)config.SortValue * characterLevelSum * (int)this.CalcSecretInformationDisplaySize(metaData, new sbyte[]
			{
				0,
				1,
				1,
				1,
				1
			}) * 100;
		}

		// Token: 0x06005361 RID: 21345 RVA: 0x002D3A0C File Offset: 0x002D1C0C
		public unsafe int CalcSecretInformationRemainingLifeTime(SecretInformationMetaData secretInformationMetaData, out short templateId)
		{
			SecretInformationCollection collection = this.GetSecretInformationCollection();
			int offset = secretInformationMetaData.GetOffset();
			byte[] array;
			byte* pRawData;
			if ((array = collection.GetRawData()) == null || array.Length == 0)
			{
				pRawData = null;
			}
			else
			{
				pRawData = &array[0];
			}
			byte* pCurrData = pRawData + offset;
			pCurrData++;
			int date = *(int*)pCurrData;
			pCurrData += 4;
			templateId = *(short*)pCurrData;
			pCurrData += 2;
			short duration = SecretInformation.Instance[templateId].Duration;
			bool flag = duration >= 0;
			int result;
			if (flag)
			{
				result = (int)duration - (DomainManager.World.GetCurrDate() - date);
			}
			else
			{
				result = 1;
			}
			array = null;
			return result;
		}

		// Token: 0x06005362 RID: 21346 RVA: 0x002D3AAC File Offset: 0x002D1CAC
		public unsafe EventArgBox MakeSecretInformationEventArgBox(SecretInformationMetaData secretInformationMetaData, EventArgBox target)
		{
			SecretInformationCollection collection = this.GetSecretInformationCollection();
			int offset = secretInformationMetaData.GetOffset();
			byte[] array;
			byte* pRawData;
			if ((array = collection.GetRawData()) == null || array.Length == 0)
			{
				pRawData = null;
			}
			else
			{
				pRawData = &array[0];
			}
			byte* pCurrData = pRawData + offset;
			pCurrData++;
			int date = *(int*)pCurrData;
			pCurrData += 4;
			short templateId = *(short*)pCurrData;
			pCurrData += 2;
			SecretInformationItem config = SecretInformation.Instance[templateId];
			bool flag = config.Parameters != null;
			if (flag)
			{
				int i = 0;
				int len = config.Parameters.Length;
				while (i < len)
				{
					string type = config.Parameters[i];
					bool flag2 = string.IsNullOrEmpty(type);
					if (!flag2)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
						defaultInterpolatedStringHandler.AppendLiteral("arg");
						defaultInterpolatedStringHandler.AppendFormatted<int>(i);
						string name = defaultInterpolatedStringHandler.ToStringAndClear();
						string text = type;
						string text2 = text;
						uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
						if (num <= 1539345862U)
						{
							if (num != 598792213U)
							{
								if (num != 1347445242U)
								{
									if (num != 1539345862U)
									{
										goto IL_2C9;
									}
									if (!(text2 == "Location"))
									{
										goto IL_2C9;
									}
									short areaId = *(short*)pCurrData;
									pCurrData += 2;
									short blockId = *(short*)pCurrData;
									pCurrData += 2;
									target.Set(name, new Location(areaId, blockId));
								}
								else
								{
									if (!(text2 == "CombatSkill"))
									{
										goto IL_2C9;
									}
									short combatSkillTemplateId = *(short*)pCurrData;
									pCurrData += 2;
									target.Set(name, combatSkillTemplateId);
								}
							}
							else
							{
								if (!(text2 == "Resource"))
								{
									goto IL_2C9;
								}
								sbyte resourceType = *(sbyte*)pCurrData;
								pCurrData++;
								target.Set(name, resourceType);
							}
						}
						else if (num <= 3651752933U)
						{
							if (num != 3197690557U)
							{
								if (num != 3651752933U)
								{
									goto IL_2C9;
								}
								if (!(text2 == "Integer"))
								{
									goto IL_2C9;
								}
								target.Set(name, *(int*)pCurrData);
								pCurrData += 4;
							}
							else
							{
								if (!(text2 == "ItemKey"))
								{
									goto IL_2C9;
								}
								ulong itemKeyValue = (ulong)(*(long*)pCurrData);
								ItemKey itemKey = (ItemKey)itemKeyValue;
								pCurrData += 8;
								target.Set(name, new ItemKey(itemKey.ItemType, 0, itemKey.TemplateId, -1));
							}
						}
						else if (num != 3966976176U)
						{
							if (num != 3992405204U)
							{
								goto IL_2C9;
							}
							if (!(text2 == "LifeSkill"))
							{
								goto IL_2C9;
							}
							short lifeSkillTemplateId = *(short*)pCurrData;
							pCurrData += 2;
							target.Set(name, lifeSkillTemplateId);
						}
						else
						{
							if (!(text2 == "Character"))
							{
								goto IL_2C9;
							}
							int charId = *(int*)pCurrData;
							pCurrData += 4;
							target.Set(name, charId);
						}
						goto IL_2D2;
						IL_2C9:
						throw new NotImplementedException(type);
					}
					IL_2D2:
					i++;
				}
			}
			target.Set("templateId", templateId);
			target.Set("metaDataId", secretInformationMetaData.GetId());
			array = null;
			return target;
		}

		// Token: 0x06005363 RID: 21347 RVA: 0x002D3DCC File Offset: 0x002D1FCC
		public unsafe bool SecretInformationIsRelateWithCharacter(int secretInformationMetaDataId, int characterId)
		{
			SecretInformationMetaData secretInformationMetaData;
			bool flag = this.TryGetElement_SecretInformationMetaData(secretInformationMetaDataId, out secretInformationMetaData);
			bool result2;
			if (flag)
			{
				bool result = false;
				SecretInformationCollection collection = this.GetSecretInformationCollection();
				int offset = secretInformationMetaData.GetOffset();
				byte[] array;
				byte* pRawData;
				if ((array = collection.GetRawData()) == null || array.Length == 0)
				{
					pRawData = null;
				}
				else
				{
					pRawData = &array[0];
				}
				byte* pCurrData = pRawData + offset;
				pCurrData++;
				int date = *(int*)pCurrData;
				pCurrData += 4;
				short templateId = *(short*)pCurrData;
				pCurrData += 2;
				SecretInformationItem config = SecretInformation.Instance[templateId];
				bool flag2 = config.Parameters != null;
				if (flag2)
				{
					int i = 0;
					int len = config.Parameters.Length;
					while (i < len)
					{
						string type = config.Parameters[i];
						bool flag3 = string.IsNullOrEmpty(type);
						if (!flag3)
						{
							string text = type;
							string text2 = text;
							uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
							if (num <= 1539345862U)
							{
								if (num != 598792213U)
								{
									if (num != 1347445242U)
									{
										if (num != 1539345862U)
										{
											goto IL_20B;
										}
										if (!(text2 == "Location"))
										{
											goto IL_20B;
										}
										pCurrData += 2;
										pCurrData += 2;
									}
									else
									{
										if (!(text2 == "CombatSkill"))
										{
											goto IL_20B;
										}
										pCurrData += 2;
									}
								}
								else
								{
									if (!(text2 == "Resource"))
									{
										goto IL_20B;
									}
									pCurrData++;
								}
							}
							else if (num <= 3651752933U)
							{
								if (num != 3197690557U)
								{
									if (num != 3651752933U)
									{
										goto IL_20B;
									}
									if (!(text2 == "Integer"))
									{
										goto IL_20B;
									}
									pCurrData += 4;
								}
								else
								{
									if (!(text2 == "ItemKey"))
									{
										goto IL_20B;
									}
									pCurrData += 8;
								}
							}
							else if (num != 3966976176U)
							{
								if (num != 3992405204U)
								{
									goto IL_20B;
								}
								if (!(text2 == "LifeSkill"))
								{
									goto IL_20B;
								}
								pCurrData += 2;
							}
							else
							{
								if (!(text2 == "Character"))
								{
									goto IL_20B;
								}
								int charId = *(int*)pCurrData;
								pCurrData += 4;
								bool flag4 = charId == characterId;
								if (flag4)
								{
									result = true;
								}
							}
							goto IL_214;
							IL_20B:
							throw new NotImplementedException(type);
						}
						IL_214:
						i++;
					}
				}
				array = null;
				result2 = result;
			}
			else
			{
				result2 = false;
			}
			return result2;
		}

		// Token: 0x06005364 RID: 21348 RVA: 0x002D4014 File Offset: 0x002D2214
		public unsafe sbyte CalcSecretInformationDisplaySize(SecretInformationMetaData metaData)
		{
			SecretInformationCollection collection = this.GetSecretInformationCollection();
			int offset = metaData.GetOffset();
			int score = 0;
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			byte[] array;
			byte* pRawData;
			if ((array = collection.GetRawData()) == null || array.Length == 0)
			{
				pRawData = null;
			}
			else
			{
				pRawData = &array[0];
			}
			byte* pCurrData = pRawData + offset;
			pCurrData++;
			int date = *(int*)pCurrData;
			pCurrData += 4;
			SecretInformationItem config = SecretInformation.Instance[*(short*)pCurrData];
			pCurrData += 2;
			sbyte level = config.BlockSizeArgs[0];
			sbyte factor = config.BlockSizeArgs[1];
			score += (int)(level * factor);
			InformationDomain.<>c__DisplayClass81_0 CS$<>8__locals1;
			CS$<>8__locals1.characterIndex = 0;
			bool flag = config.Parameters != null;
			if (flag)
			{
				int i = 0;
				int len = config.Parameters.Length;
				while (i < len)
				{
					string type = config.Parameters[i];
					bool flag2 = string.IsNullOrEmpty(type);
					if (!flag2)
					{
						string text = type;
						string text2 = text;
						uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
						if (num <= 1539345862U)
						{
							if (num != 598792213U)
							{
								if (num != 1347445242U)
								{
									if (num != 1539345862U)
									{
										goto IL_419;
									}
									if (!(text2 == "Location"))
									{
										goto IL_419;
									}
									pCurrData += 2;
									pCurrData += 2;
								}
								else
								{
									if (!(text2 == "CombatSkill"))
									{
										goto IL_419;
									}
									pCurrData += 2;
								}
							}
							else
							{
								if (!(text2 == "Resource"))
								{
									goto IL_419;
								}
								pCurrData++;
							}
						}
						else if (num <= 3651752933U)
						{
							if (num != 3197690557U)
							{
								if (num != 3651752933U)
								{
									goto IL_419;
								}
								if (!(text2 == "Integer"))
								{
									goto IL_419;
								}
								pCurrData += 4;
							}
							else
							{
								if (!(text2 == "ItemKey"))
								{
									goto IL_419;
								}
								ulong itemKeyValue = (ulong)(*(long*)pCurrData);
								ItemKey itemKey = (ItemKey)itemKeyValue;
								pCurrData += 8;
								bool flag3 = itemKey.ItemType >= 0 && itemKey.TemplateId >= 0;
								if (flag3)
								{
									score += (int)(GlobalConfig.SecretInformationDisplay_ItemGradeToValue[(int)ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId)] * (short)config.BlockSizeArgs[4]);
								}
							}
						}
						else if (num != 3966976176U)
						{
							if (num != 3992405204U)
							{
								goto IL_419;
							}
							if (!(text2 == "LifeSkill"))
							{
								goto IL_419;
							}
							pCurrData += 2;
						}
						else
						{
							if (!(text2 == "Character"))
							{
								goto IL_419;
							}
							int charId = *(int*)pCurrData;
							pCurrData += 4;
							bool isSect = false;
							sbyte grade = 0;
							GameData.Domains.Character.Character character;
							bool flag4 = DomainManager.Character.TryGetElement_Objects(charId, out character);
							if (flag4)
							{
								sbyte orgTemplateId = character.GetOrganizationInfo().OrgTemplateId;
								isSect = (orgTemplateId >= 0 && Organization.Instance[orgTemplateId].IsSect);
								grade = character.GetOrganizationInfo().Grade;
							}
							else
							{
								DeadCharacter deadChar = DomainManager.Character.TryGetDeadCharacter(charId);
								bool flag5 = deadChar != null;
								if (flag5)
								{
									sbyte orgTemplateId2 = deadChar.OrganizationInfo.OrgTemplateId;
									isSect = (orgTemplateId2 >= 0 && Organization.Instance[orgTemplateId2].IsSect);
									grade = deadChar.OrganizationInfo.Grade;
								}
							}
							short[] charRelationTypeToValue = InformationDomain.<CalcSecretInformationDisplaySize>g__CharRelationTypeToValue|81_1(isSect, ref CS$<>8__locals1);
							sbyte relationIndex = RelationType.GetTypeId(0);
							bool flag6 = charId == taiwuCharId;
							if (flag6)
							{
								relationIndex = (sbyte)(charRelationTypeToValue.Length - 1);
							}
							else
							{
								RelatedCharacter relation;
								bool flag7 = DomainManager.Character.TryGetRelation(taiwuCharId, charId, out relation) && relation.RelationType != ushort.MaxValue;
								if (flag7)
								{
									for (ushort v = relation.RelationType; v < 17; v += 1)
									{
										ushort r = (ushort)(1 << (int)v);
										bool flag8 = (r & relation.RelationType) > 0;
										if (flag8)
										{
											relationIndex = RelationType.GetTypeId(r);
											break;
										}
									}
								}
							}
							score += (int)(InformationDomain.<CalcSecretInformationDisplaySize>g__CharGradeToValue|81_0(isSect, ref CS$<>8__locals1)[(int)grade] * (short)config.BlockSizeArgs[2]);
							score += (int)(charRelationTypeToValue[(int)relationIndex] * (short)config.BlockSizeArgs[3]);
							CS$<>8__locals1.characterIndex++;
						}
						goto IL_422;
						IL_419:
						throw new NotImplementedException(type);
					}
					IL_422:
					i++;
				}
			}
			array = null;
			for (int j = 0; j < GlobalConfig.SecretInformationDisplay_SizeThresholds.Length; j++)
			{
				bool flag9 = score < (int)GlobalConfig.SecretInformationDisplay_SizeThresholds[j];
				if (flag9)
				{
					return (sbyte)j;
				}
			}
			return (sbyte)GlobalConfig.SecretInformationDisplay_SizeThresholds.Length;
		}

		// Token: 0x06005365 RID: 21349 RVA: 0x002D44A4 File Offset: 0x002D26A4
		public unsafe SecretInformationDisplayData GetSecretInformationDisplayData(int secretInformationMetaDataId, ISet<int> characterSet)
		{
			SecretInformationMetaData secretInformationMetaData;
			bool flag = this.TryGetElement_SecretInformationMetaData(secretInformationMetaDataId, out secretInformationMetaData);
			SecretInformationDisplayData result;
			if (flag)
			{
				bool relatedToTaiwu = false;
				bool relatedToTaiwuFriendly = false;
				bool relatedToTaiwuEnemy = false;
				int taiwuCharacterId = DomainManager.Taiwu.GetTaiwuCharId();
				SecretInformationDisplayData displayData = new SecretInformationDisplayData();
				SecretInformationCollection collection = this.GetSecretInformationCollection();
				int offset = secretInformationMetaData.GetOffset();
				displayData.SourceCharacterId = -1;
				byte[] array;
				byte* pRawData;
				if ((array = collection.GetRawData()) == null || array.Length == 0)
				{
					pRawData = null;
				}
				else
				{
					pRawData = &array[0];
				}
				HashSet<SecretInformationRelationshipType> relationShip = ObjectPool<HashSet<SecretInformationRelationshipType>>.Instance.Get();
				byte* pCurrData = pRawData + offset;
				pCurrData++;
				int date = *(int*)pCurrData;
				pCurrData += 4;
				displayData.SecretInformationTemplateId = *(short*)pCurrData;
				pCurrData += 2;
				SecretInformationItem config = SecretInformation.Instance[displayData.SecretInformationTemplateId];
				bool flag2 = config.Parameters != null;
				if (flag2)
				{
					int i = 0;
					int len = config.Parameters.Length;
					while (i < len)
					{
						string type = config.Parameters[i];
						bool flag3 = string.IsNullOrEmpty(type);
						if (!flag3)
						{
							string text = type;
							string text2 = text;
							uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
							if (num <= 1539345862U)
							{
								if (num != 598792213U)
								{
									if (num != 1347445242U)
									{
										if (num != 1539345862U)
										{
											goto IL_2B6;
										}
										if (!(text2 == "Location"))
										{
											goto IL_2B6;
										}
										pCurrData += 2;
										pCurrData += 2;
									}
									else
									{
										if (!(text2 == "CombatSkill"))
										{
											goto IL_2B6;
										}
										pCurrData += 2;
									}
								}
								else
								{
									if (!(text2 == "Resource"))
									{
										goto IL_2B6;
									}
									pCurrData++;
								}
							}
							else if (num <= 3651752933U)
							{
								if (num != 3197690557U)
								{
									if (num != 3651752933U)
									{
										goto IL_2B6;
									}
									if (!(text2 == "Integer"))
									{
										goto IL_2B6;
									}
									pCurrData += 4;
								}
								else
								{
									if (!(text2 == "ItemKey"))
									{
										goto IL_2B6;
									}
									pCurrData += 8;
								}
							}
							else if (num != 3966976176U)
							{
								if (num != 3992405204U)
								{
									goto IL_2B6;
								}
								if (!(text2 == "LifeSkill"))
								{
									goto IL_2B6;
								}
								pCurrData += 2;
							}
							else
							{
								if (!(text2 == "Character"))
								{
									goto IL_2B6;
								}
								int charId = *(int*)pCurrData;
								pCurrData += 4;
								characterSet.Add(charId);
								bool flag4 = charId == taiwuCharacterId;
								if (flag4)
								{
									relatedToTaiwu = true;
								}
								relationShip.Clear();
								this.CheckSecretInformationRelationship(taiwuCharacterId, -1, charId, -1, relationShip);
								bool flag5 = relationShip.Contains(SecretInformationRelationshipType.Friend) || relationShip.Contains(SecretInformationRelationshipType.Relative);
								if (flag5)
								{
									relatedToTaiwuFriendly = true;
								}
								else
								{
									bool flag6 = relationShip.Contains(SecretInformationRelationshipType.Enemy);
									if (flag6)
									{
										relatedToTaiwuEnemy = true;
									}
								}
							}
							goto IL_2BF;
							IL_2B6:
							throw new NotImplementedException(type);
						}
						IL_2BF:
						i++;
					}
				}
				ObjectPool<HashSet<SecretInformationRelationshipType>>.Instance.Return(relationShip);
				array = null;
				displayData.HolderCount = this.GetSecretInformationHolderCount(secretInformationMetaDataId);
				displayData.UsedCount = 0;
				displayData.AuthorityCostWhenDisseminating = 0;
				displayData.Location = DomainManager.Map.GetBlockFullName(DomainManager.Extra.GetSecretInformationOccurredLocation(secretInformationMetaDataId));
				displayData.SecretInformationMetaDataId = secretInformationMetaDataId;
				SecretInformationCollection rawData = new SecretInformationCollection();
				int size = collection.GetRecordSize(offset);
				rawData.EnsureCapacity(size);
				rawData.Count = 1;
				Array.Copy(collection.RawData, offset, rawData.RawData, 0, size);
				displayData.RawData = rawData;
				bool flag7 = relatedToTaiwu;
				if (flag7)
				{
					SecretInformationDisplayData secretInformationDisplayData = displayData;
					secretInformationDisplayData.FilterMask |= 1;
				}
				else
				{
					SecretInformationDisplayData secretInformationDisplayData2 = displayData;
					secretInformationDisplayData2.FilterMask |= 2;
				}
				bool flag8 = relatedToTaiwuFriendly;
				if (flag8)
				{
					SecretInformationDisplayData secretInformationDisplayData3 = displayData;
					secretInformationDisplayData3.FilterMask |= 4;
				}
				bool flag9 = relatedToTaiwuEnemy;
				if (flag9)
				{
					SecretInformationDisplayData secretInformationDisplayData4 = displayData;
					secretInformationDisplayData4.FilterMask |= 8;
				}
				displayData.DisplaySize = this.CalcSecretInformationDisplaySize(secretInformationMetaData);
				displayData.ShopValue = this.CalcSecretInformationShopValue(secretInformationMetaData);
				result = displayData;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06005366 RID: 21350 RVA: 0x002D48A4 File Offset: 0x002D2AA4
		public int GetSecretInformationHolderCount(int secretInformationMetaDataId)
		{
			return this._characterSecretInformation.Values.Count((SecretInformationCharacterDataCollection v) => v.Collection.ContainsKey(secretInformationMetaDataId));
		}

		// Token: 0x06005367 RID: 21351 RVA: 0x002D48DC File Offset: 0x002D2ADC
		[DomainMethod]
		public SecretInformationDisplayPackage GetSecretInformationDisplayPackage(List<int> secretInformationMetaDataIds)
		{
			SecretInformationDisplayPackage result = new SecretInformationDisplayPackage();
			HashSet<int> characterSet = new HashSet<int>();
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			int taiwuCharId = taiwuChar.GetId();
			SecretInformationCharacterDataCollection taiwuCharacterDataCollection;
			bool taiwuHasCollection = this.TryGetElement_CharacterSecretInformation(taiwuCharId, out taiwuCharacterDataCollection);
			if (secretInformationMetaDataIds == null)
			{
				secretInformationMetaDataIds = new List<int>();
			}
			IEnumerable<int> source = secretInformationMetaDataIds;
			Func<int, SecretInformationDisplayData> <>9__0;
			Func<int, SecretInformationDisplayData> selector;
			if ((selector = <>9__0) == null)
			{
				selector = (<>9__0 = ((int metaDataId) => this.GetSecretInformationDisplayData(metaDataId, characterSet)));
			}
			foreach (SecretInformationDisplayData record in source.Select(selector))
			{
				bool flag = record == null;
				if (!flag)
				{
					record.UsedCount = (int)DomainManager.Extra.GetCharacterSecretInformationUsedCount(taiwuCharId, record.SecretInformationMetaDataId);
					SecretInformationCharacterData characterData;
					record.AuthorityCostWhenDisseminating = this.CalcSecretInformationAuthorityCostWhenDisseminatingByCharacter(record.SecretInformationTemplateId, taiwuChar.GetFameType(), this.GetSecretInformationDisseminatingCountOfBranch(record.SecretInformationMetaDataId, (taiwuHasCollection && taiwuCharacterDataCollection.Collection.TryGetValue(record.SecretInformationMetaDataId, out characterData)) ? characterData.SecretInformationDisseminationBranch : 1));
					result.SecretInformationDisplayDataList.Add(record);
				}
			}
			foreach (int charId in characterSet)
			{
				result.CharacterData.Add(charId, DomainManager.Character.GetCharacterDisplayData(charId));
			}
			return result;
		}

		// Token: 0x06005368 RID: 21352 RVA: 0x002D4A7C File Offset: 0x002D2C7C
		[DomainMethod]
		public SecretInformationDisplayPackage GetSecretInformationDisplayPackageFromCharacter(int characterId)
		{
			SecretInformationDisplayPackage result = new SecretInformationDisplayPackage();
			HashSet<int> characterSet = new HashSet<int>();
			SecretInformationCharacterDataCollection characterDataCollection;
			GameData.Domains.Character.Character character;
			bool flag = this.TryGetElement_CharacterSecretInformation(characterId, out characterDataCollection) && DomainManager.Character.TryGetElement_Objects(characterId, out character);
			if (flag)
			{
				foreach (KeyValuePair<int, SecretInformationCharacterData> pair in characterDataCollection.Collection)
				{
					SecretInformationDisplayData record = this.GetSecretInformationDisplayData(pair.Key, characterSet);
					record.SourceCharacterId = pair.Value.SecretInformationDisseminationBranch;
					record.UsedCount = (int)DomainManager.Extra.GetCharacterSecretInformationUsedCount(characterId, pair.Key);
					record.AuthorityCostWhenDisseminating = this.CalcSecretInformationAuthorityCostWhenDisseminatingByCharacter(record.SecretInformationTemplateId, character.GetFameType(), this.GetSecretInformationDisseminatingCountOfBranch(record.SecretInformationMetaDataId, pair.Value.SecretInformationDisseminationBranch));
					result.SecretInformationDisplayDataList.Add(record);
					bool flag2 = record.SourceCharacterId > 0;
					if (flag2)
					{
						characterSet.Add(record.SourceCharacterId);
					}
				}
			}
			foreach (int charId in characterSet)
			{
				result.CharacterData.Add(charId, DomainManager.Character.GetCharacterDisplayData(charId));
			}
			return result;
		}

		// Token: 0x06005369 RID: 21353 RVA: 0x002D4C00 File Offset: 0x002D2E00
		[DomainMethod]
		public SecretInformationDisplayPackage GetSecretInformationDisplayPackageFromBroadcast(int characterId)
		{
			SecretInformationDisplayPackage result = new SecretInformationDisplayPackage();
			HashSet<int> characterSet = new HashSet<int>();
			foreach (int metaDataId in this.GetBroadcastSecretInformationIds())
			{
				SecretInformationDisplayData record = this.GetSecretInformationDisplayData(metaDataId, characterSet);
				record.UsedCount = (int)DomainManager.Extra.GetCharacterSecretInformationUsedCount(characterId, metaDataId);
				result.SecretInformationDisplayDataList.Add(record);
			}
			foreach (int charId in characterSet)
			{
				result.CharacterData.Add(charId, DomainManager.Character.GetCharacterDisplayData(charId));
			}
			return result;
		}

		// Token: 0x0600536A RID: 21354 RVA: 0x002D4CE0 File Offset: 0x002D2EE0
		[DomainMethod]
		public SecretInformationDisplayPackage GetSecretInformationDisplayPackageForSelections(int characterId)
		{
			SecretInformationDisplayPackage result = new SecretInformationDisplayPackage();
			HashSet<int> characterSet = new HashSet<int>();
			SecretInformationCharacterDataCollection characterDataCollection;
			GameData.Domains.Character.Character character;
			bool flag = this.TryGetElement_CharacterSecretInformation(characterId, out characterDataCollection) && DomainManager.Character.TryGetElement_Objects(characterId, out character);
			if (flag)
			{
				foreach (KeyValuePair<int, SecretInformationCharacterData> pair in characterDataCollection.Collection)
				{
					SecretInformationDisplayData record = this.GetSecretInformationDisplayData(pair.Key, characterSet);
					record.SourceCharacterId = pair.Value.SecretInformationDisseminationBranch;
					record.UsedCount = (int)DomainManager.Extra.GetCharacterSecretInformationUsedCount(characterId, pair.Key);
					record.AuthorityCostWhenDisseminating = this.CalcSecretInformationAuthorityCostWhenDisseminatingByCharacter(record.SecretInformationTemplateId, character.GetFameType(), this.GetSecretInformationDisseminatingCountOfBranch(record.SecretInformationMetaDataId, pair.Value.SecretInformationDisseminationBranch));
					result.SecretInformationDisplayDataList.Add(record);
					bool flag2 = record.SourceCharacterId > 0;
					if (flag2)
					{
						characterSet.Add(record.SourceCharacterId);
					}
				}
			}
			foreach (int metaDataId in this.GetBroadcastSecretInformationIds())
			{
				SecretInformationDisplayData record2 = this.GetSecretInformationDisplayData(metaDataId, characterSet);
				record2.UsedCount = (int)DomainManager.Extra.GetCharacterSecretInformationUsedCount(characterId, metaDataId);
				result.SecretInformationDisplayDataList.Add(record2);
			}
			foreach (int charId in characterSet)
			{
				result.CharacterData.Add(charId, DomainManager.Character.GetCharacterDisplayData(charId));
			}
			return result;
		}

		// Token: 0x0600536B RID: 21355 RVA: 0x002D4ED0 File Offset: 0x002D30D0
		[DomainMethod]
		public bool DisseminateSecretInformation(DataContext context, int metaDataId, int sourceCharId, int targetCharId)
		{
			GameData.Domains.Character.Character sourceChar = DomainManager.Character.GetElement_Objects(sourceCharId);
			int cost = 0;
			SecretInformationCharacterDataCollection characterDataCollection;
			SecretInformationCharacterData characterData;
			bool flag = DomainManager.Information.TryGetElement_CharacterSecretInformation(sourceCharId, out characterDataCollection) && characterDataCollection.Collection.TryGetValue(metaDataId, out characterData);
			if (flag)
			{
				short templateId = DomainManager.Information.CalcSecretInformationTemplateId(metaDataId);
				cost = DomainManager.Information.CalcSecretInformationAuthorityCostWhenDisseminatingByCharacter(templateId, sourceChar.GetFameType(), this.GetSecretInformationDisseminatingCountOfBranch(metaDataId, characterData.SecretInformationDisseminationBranch));
			}
			this.CostSecretInformationUsedCount(context, sourceCharId, metaDataId);
			bool flag2 = this.DistributeSecretInformationToCharacter(context, metaDataId, targetCharId, sourceCharId);
			bool result;
			if (flag2)
			{
				sourceChar.ChangeResource(context, 7, -Math.Min(sourceChar.GetResource(7), cost));
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600536C RID: 21356 RVA: 0x002D4F80 File Offset: 0x002D3180
		public SecretInformationDisplayPackage GetRelateBSecretInformationDisplayPackageForSelections(int charA, int charB)
		{
			SecretInformationDisplayPackage result = new SecretInformationDisplayPackage();
			HashSet<int> characterSet = new HashSet<int>();
			SecretInformationCharacterDataCollection characterDataCollection;
			GameData.Domains.Character.Character character;
			bool flag = this.TryGetElement_CharacterSecretInformation(charA, out characterDataCollection) && DomainManager.Character.TryGetElement_Objects(charA, out character);
			if (flag)
			{
				foreach (KeyValuePair<int, SecretInformationCharacterData> pair in characterDataCollection.Collection)
				{
					bool flag2 = this.SecretInformationIsRelateWithCharacter(pair.Key, charB);
					if (flag2)
					{
						SecretInformationDisplayData record = this.GetSecretInformationDisplayData(pair.Key, characterSet);
						record.SourceCharacterId = pair.Value.SecretInformationDisseminationBranch;
						record.UsedCount = (int)DomainManager.Extra.GetCharacterSecretInformationUsedCount(charA, pair.Key);
						record.AuthorityCostWhenDisseminating = this.CalcSecretInformationAuthorityCostWhenDisseminatingByCharacter(record.SecretInformationTemplateId, character.GetFameType(), this.GetSecretInformationDisseminatingCountOfBranch(record.SecretInformationMetaDataId, pair.Value.SecretInformationDisseminationBranch));
						result.SecretInformationDisplayDataList.Add(record);
						bool flag3 = record.SourceCharacterId > 0;
						if (flag3)
						{
							characterSet.Add(record.SourceCharacterId);
						}
					}
				}
			}
			foreach (int metaDataId in this.GetBroadcastSecretInformationIds())
			{
				bool flag4 = this.SecretInformationIsRelateWithCharacter(metaDataId, charB);
				if (flag4)
				{
					SecretInformationDisplayData record2 = this.GetSecretInformationDisplayData(metaDataId, characterSet);
					record2.UsedCount = (int)DomainManager.Extra.GetCharacterSecretInformationUsedCount(charA, metaDataId);
					result.SecretInformationDisplayDataList.Add(record2);
				}
			}
			foreach (int charId in characterSet)
			{
				result.CharacterData.Add(charId, DomainManager.Character.GetCharacterDisplayData(charId));
			}
			return result;
		}

		// Token: 0x0600536D RID: 21357 RVA: 0x002D5198 File Offset: 0x002D3398
		public void PrepareSecretInformationAdvanceMonth()
		{
		}

		// Token: 0x0600536E RID: 21358 RVA: 0x002D519C File Offset: 0x002D339C
		public void SetNormalInformationCharacterDataModified(DataContext context, int characterId)
		{
			NormalInformationCollection element;
			bool flag = this.TryGetElement_Information(characterId, out element);
			if (flag)
			{
				this.SetElement_Information(characterId, element, context);
			}
		}

		// Token: 0x0600536F RID: 21359 RVA: 0x002D51C4 File Offset: 0x002D33C4
		public void SetSecretInformationCharacterDataModified(DataContext context, int characterId)
		{
			SecretInformationCharacterDataCollection element;
			bool flag = this.TryGetElement_CharacterSecretInformation(characterId, out element);
			if (flag)
			{
				this.SetElement_CharacterSecretInformation(characterId, element, context);
			}
		}

		// Token: 0x06005370 RID: 21360 RVA: 0x002D51EC File Offset: 0x002D33EC
		public int CalcSecretInformationAuthorityCostWhenDisseminating(short secretInformationTemplateId, int disseminationCountOfBranch)
		{
			SecretInformationItem config = SecretInformation.Instance[secretInformationTemplateId];
			return (int)config.CostAuthority * Math.Max(0, 100 - Math.Max(disseminationCountOfBranch - 1, 0) / 10) / 100;
		}

		// Token: 0x06005371 RID: 21361 RVA: 0x002D522C File Offset: 0x002D342C
		public int CalcSecretInformationAuthorityCostWhenDisseminatingByCharacter(short secretInformationTemplateId, sbyte characterFameType, int disseminationCountOfBranch)
		{
			bool flag = characterFameType == -2;
			if (flag)
			{
				characterFameType = 3;
			}
			int baseValue = this.CalcSecretInformationAuthorityCostWhenDisseminating(secretInformationTemplateId, disseminationCountOfBranch);
			SecretInformationItem config = SecretInformation.Instance[secretInformationTemplateId];
			int fameDiff = (int)(config.FameThreshold - characterFameType);
			bool flag2 = fameDiff > 0;
			int result;
			if (flag2)
			{
				result = baseValue * (1 + fameDiff * 2);
			}
			else
			{
				result = baseValue;
			}
			return result;
		}

		// Token: 0x06005372 RID: 21362 RVA: 0x002D5280 File Offset: 0x002D3480
		public int GetSecretInformationDisseminatingCountOfBranch(int metaDataId, int disseminationBranch)
		{
			int disseminatingCount = 0;
			SecretInformationMetaData metaData;
			bool flag = this.TryGetElement_SecretInformationMetaData(metaDataId, out metaData);
			if (flag)
			{
				disseminatingCount = metaData.GetCharacterDisseminationCount(disseminationBranch);
			}
			return disseminatingCount;
		}

		// Token: 0x06005373 RID: 21363 RVA: 0x002D52AC File Offset: 0x002D34AC
		private int GetAndIncreaseNextMetaDataId(DataContext context)
		{
			int nextId = this._nextMetaDataId;
			this._nextMetaDataId++;
			bool flag = this._nextMetaDataId > int.MaxValue;
			if (flag)
			{
				this._nextMetaDataId = 1;
			}
			this.SetNextMetaDataId(this._nextMetaDataId, context);
			return nextId;
		}

		// Token: 0x06005374 RID: 21364 RVA: 0x002D52FA File Offset: 0x002D34FA
		public int AddSecretInformationMetaData(DataContext context, int dataOffset, bool withInitialDistribute = true)
		{
			return this.AddSecretInformationMetaDataWithNecessity(context, dataOffset, withInitialDistribute, false, null);
		}

		// Token: 0x06005375 RID: 21365 RVA: 0x002D5308 File Offset: 0x002D3508
		public unsafe int AddSecretInformationMetaDataWithNecessity(DataContext context, int dataOffset, bool withInitialDistribute, bool necessarily, Action<ICollection<int>> distributeCallback)
		{
			int nextId = this.GetAndIncreaseNextMetaDataId(context);
			bool isTaiwuKeyCharacter = false;
			bool isKeyCharacterRelatedByTaiwu = false;
			SecretInformationMetaData metaData = new SecretInformationMetaData(nextId, dataOffset, -1);
			short templateId;
			this.CalcSecretInformationRemainingLifeTime(metaData, out templateId);
			SecretInformationItem config = SecretInformation.Instance[templateId];
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			this.CommitInsert_SecretInformationCollection(context, dataOffset, this._secretInformationCollection.GetRecordSize(dataOffset));
			this.CommitSetMetadata_SecretInformationCollection(context);
			int discoveryRate = (int)config.DiscoveryRate;
			sbyte resourceType = -1;
			int argGradeSum = 0;
			int argGradeCount = 0;
			int argCharFameSum = 0;
			int argCharCount = 0;
			ObjectPool<HashSet<int>> pool = ObjectPool<HashSet<int>>.Instance;
			EventArgBox argBox = DomainManager.TaiwuEvent.GetEventArgBox();
			HashSet<int> set = pool.Get();
			DomainManager.Information.MakeSecretInformationEventArgBox(metaData, argBox);
			bool flag = config.Parameters != null;
			if (flag)
			{
				for (int index = 0; index < config.Parameters.Length; index++)
				{
					int charId = -1;
					bool flag2 = config.Parameters[index] == "ItemKey";
					if (flag2)
					{
						EventArgBox eventArgBox = argBox;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
						defaultInterpolatedStringHandler.AppendLiteral("arg");
						defaultInterpolatedStringHandler.AppendFormatted<int>(index);
						ItemKey itemKey;
						bool flag3 = eventArgBox.Get<ItemKey>(defaultInterpolatedStringHandler.ToStringAndClear(), out itemKey) && itemKey.IsValid();
						if (flag3)
						{
							argGradeSum += (int)ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId);
							argGradeCount++;
						}
					}
					else
					{
						bool flag4 = config.Parameters[index] == "CombatSkill";
						if (flag4)
						{
							short combatSkillId = -1;
							EventArgBox eventArgBox2 = argBox;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
							defaultInterpolatedStringHandler.AppendLiteral("arg");
							defaultInterpolatedStringHandler.AppendFormatted<int>(index);
							bool flag5 = eventArgBox2.Get(defaultInterpolatedStringHandler.ToStringAndClear(), ref combatSkillId);
							if (flag5)
							{
								argGradeSum += (int)CombatSkill.Instance[combatSkillId].Grade;
								argGradeCount++;
							}
						}
						else
						{
							bool flag6 = config.Parameters[index] == "LifeSkill";
							if (flag6)
							{
								short lifeSkillId = -1;
								EventArgBox eventArgBox3 = argBox;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
								defaultInterpolatedStringHandler.AppendLiteral("arg");
								defaultInterpolatedStringHandler.AppendFormatted<int>(index);
								bool flag7 = eventArgBox3.Get(defaultInterpolatedStringHandler.ToStringAndClear(), ref lifeSkillId);
								if (flag7)
								{
									argGradeSum += (int)LifeSkill.Instance[lifeSkillId].Grade;
									argGradeCount++;
								}
							}
							else
							{
								bool flag8 = config.Parameters[index] == "Resource";
								if (flag8)
								{
									EventArgBox eventArgBox4 = argBox;
									DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
									defaultInterpolatedStringHandler.AppendLiteral("arg");
									defaultInterpolatedStringHandler.AppendFormatted<int>(index);
									eventArgBox4.Get(defaultInterpolatedStringHandler.ToStringAndClear(), ref resourceType);
								}
								else
								{
									bool flag9 = config.Parameters[index] == "Integer";
									if (flag9)
									{
										int value = -1;
										bool flag10;
										if (resourceType >= 0)
										{
											EventArgBox eventArgBox5 = argBox;
											DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
											defaultInterpolatedStringHandler.AppendLiteral("arg");
											defaultInterpolatedStringHandler.AppendFormatted<int>(index);
											flag10 = eventArgBox5.Get(defaultInterpolatedStringHandler.ToStringAndClear(), ref value);
										}
										else
										{
											flag10 = false;
										}
										bool flag11 = flag10;
										if (flag11)
										{
											sbyte resourceGrade = ResourceTypeHelper.ResourceAmountToGrade(resourceType, value);
											bool flag12 = resourceGrade >= 0;
											if (flag12)
											{
												argGradeSum += (int)resourceGrade;
												argGradeCount++;
											}
										}
									}
									else
									{
										bool flag13;
										if (!(config.Parameters[index] != "Character"))
										{
											EventArgBox eventArgBox6 = argBox;
											DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
											defaultInterpolatedStringHandler.AppendLiteral("arg");
											defaultInterpolatedStringHandler.AppendFormatted<int>(index);
											flag13 = !eventArgBox6.Get(defaultInterpolatedStringHandler.ToStringAndClear(), ref charId);
										}
										else
										{
											flag13 = true;
										}
										bool flag14 = flag13;
										if (!flag14)
										{
											int fameTypeForCalc = -1;
											DeadCharacter deadCharacter = DomainManager.Character.TryGetDeadCharacter(charId);
											GameData.Domains.Character.Character chara;
											bool flag15 = DomainManager.Character.TryGetElement_Objects(charId, out chara);
											if (flag15)
											{
												fameTypeForCalc = (int)chara.GetFameType();
												argGradeSum += (int)chara.GetOrganizationInfo().Grade;
											}
											else
											{
												bool flag16 = deadCharacter != null;
												if (flag16)
												{
													fameTypeForCalc = (int)deadCharacter.FameType;
													argGradeSum += (int)deadCharacter.OrganizationInfo.Grade;
												}
											}
											argCharCount++;
											argGradeCount++;
											bool flag17 = fameTypeForCalc == -2;
											if (flag17)
											{
												argCharFameSum = argCharFameSum;
											}
											else
											{
												bool flag18 = InformationDomain.FameTypeForDiscoveryRates.CheckIndex(fameTypeForCalc);
												if (flag18)
												{
													argCharFameSum += Math.Abs(InformationDomain.FameTypeForDiscoveryRates[fameTypeForCalc]);
												}
											}
											bool flag19 = charId == taiwuCharId;
											if (flag19)
											{
												isTaiwuKeyCharacter = true;
											}
											else
											{
												RelatedCharacter relatedCharacter;
												bool flag20 = DomainManager.Character.TryGetRelation(taiwuCharId, charId, out relatedCharacter);
												if (flag20)
												{
													isKeyCharacterRelatedByTaiwu = true;
												}
											}
											GameData.Domains.Character.Character character;
											bool flag21 = DomainManager.Character.TryGetElement_Objects(charId, out character) && index == 0;
											if (flag21)
											{
												DomainManager.Extra.SetSecretInformationOccurredLocation(context, nextId, character.GetLocation());
											}
											bool flag22 = config.RelationshipSnapshotParameterIndices != null && config.RelationshipSnapshotParameterIndices.Contains(index);
											if (flag22)
											{
												RelatedCharacters related = DomainManager.Character.GetRelatedCharacters(charId);
												bool flag23 = related != null;
												if (flag23)
												{
													InformationDomain._bufferForRelationSnapshot.EnsureCapacity(related.GetSerializedSize());
													byte[] array;
													byte* ptr;
													if ((array = InformationDomain._bufferForRelationSnapshot.RawData) == null || array.Length == 0)
													{
														ptr = null;
													}
													else
													{
														ptr = &array[0];
													}
													related.Serialize(ptr);
													related = new RelatedCharacters();
													related.Deserialize(ptr);
													array = null;
													related.General.Clear();
													HashSet<int> relatedAdored = DomainManager.Character.GetReversedRelatedCharIds(charId, 16384).GetCollection();
													bool flag24 = relatedAdored != null;
													if (flag24)
													{
														foreach (int generalCharacterId in relatedAdored)
														{
															bool flag25 = !metaData.CharacterRelationshipSnapshotCollection.Collection.ContainsKey(generalCharacterId);
															if (flag25)
															{
																RelatedCharacters generalRelated = DomainManager.Character.GetRelatedCharacters(generalCharacterId);
																InformationDomain._bufferForRelationSnapshot.EnsureCapacity(generalRelated.GetSerializedSize());
																try
																{
																	byte* ptr2;
																	if ((array = InformationDomain._bufferForRelationSnapshot.RawData) == null || array.Length == 0)
																	{
																		ptr2 = null;
																	}
																	else
																	{
																		ptr2 = &array[0];
																	}
																	generalRelated.Serialize(ptr2);
																	generalRelated = new RelatedCharacters();
																	generalRelated.Deserialize(ptr2);
																}
																finally
																{
																	array = null;
																}
																generalRelated.Friends.Clear();
																generalRelated.General.Clear();
																generalRelated.Mentees.Clear();
																generalRelated.Mentors.Clear();
																generalRelated.AdoptiveChildren.Clear();
																generalRelated.AdoptiveParents.Clear();
																generalRelated.BloodChildren.Clear();
																generalRelated.BloodParents.Clear();
																generalRelated.StepChildren.Clear();
																generalRelated.StepParents.Clear();
																generalRelated.HusbandsAndWives.Clear();
																generalRelated.AdoptiveBrothersAndSisters.Clear();
																generalRelated.BloodBrothersAndSisters.Clear();
																generalRelated.StepBrothersAndSisters.Clear();
																generalRelated.SwornBrothersAndSisters.Clear();
																metaData.CharacterRelationshipSnapshotCollection.Collection[generalCharacterId] = new SecretInformationCharacterRelationshipSnapshot
																{
																	RelatedCharacters = generalRelated
																};
															}
														}
													}
													HashSet<int> relatedEnemy = DomainManager.Character.GetReversedRelatedCharIds(charId, 32768).GetCollection();
													bool flag26 = relatedEnemy != null;
													if (flag26)
													{
														foreach (int generalCharacterId2 in relatedEnemy)
														{
															bool flag27 = !metaData.CharacterRelationshipSnapshotCollection.Collection.ContainsKey(generalCharacterId2);
															if (flag27)
															{
																RelatedCharacters generalRelated2 = DomainManager.Character.GetRelatedCharacters(generalCharacterId2);
																bool flag28 = generalRelated2 == null || InformationDomain._bufferForRelationSnapshot == null;
																if (flag28)
																{
																}
																InformationDomain._bufferForRelationSnapshot.EnsureCapacity(generalRelated2.GetSerializedSize());
																try
																{
																	byte* ptr3;
																	if ((array = InformationDomain._bufferForRelationSnapshot.RawData) == null || array.Length == 0)
																	{
																		ptr3 = null;
																	}
																	else
																	{
																		ptr3 = &array[0];
																	}
																	generalRelated2.Serialize(ptr3);
																	generalRelated2 = new RelatedCharacters();
																	generalRelated2.Deserialize(ptr3);
																}
																finally
																{
																	array = null;
																}
																generalRelated2.Friends.Clear();
																generalRelated2.General.Clear();
																generalRelated2.Mentees.Clear();
																generalRelated2.Mentors.Clear();
																generalRelated2.AdoptiveChildren.Clear();
																generalRelated2.AdoptiveParents.Clear();
																generalRelated2.BloodChildren.Clear();
																generalRelated2.BloodParents.Clear();
																generalRelated2.StepChildren.Clear();
																generalRelated2.StepParents.Clear();
																generalRelated2.HusbandsAndWives.Clear();
																generalRelated2.AdoptiveBrothersAndSisters.Clear();
																generalRelated2.BloodBrothersAndSisters.Clear();
																generalRelated2.StepBrothersAndSisters.Clear();
																generalRelated2.SwornBrothersAndSisters.Clear();
																metaData.CharacterRelationshipSnapshotCollection.Collection[generalCharacterId2] = new SecretInformationCharacterRelationshipSnapshot
																{
																	RelatedCharacters = generalRelated2
																};
															}
														}
													}
													set.Clear();
													related.GetAllRelatedCharIds(set, config.IsGeneralRelationCharactersNeedSnapshot);
													foreach (int relatedCharId in set)
													{
														SecretInformationCharacterExtraInfo extraInfo;
														metaData.CharacterExtraInfoCollection.Collection.TryGetValue(relatedCharId, out extraInfo);
														GameData.Domains.Character.Character character3;
														extraInfo.AliveState = (DomainManager.Character.TryGetElement_Objects(charId, out character3) ? 0 : 1);
														metaData.CharacterExtraInfoCollection.Collection[relatedCharId] = extraInfo;
													}
													metaData.CharacterRelationshipSnapshotCollection.Collection[charId] = new SecretInformationCharacterRelationshipSnapshot
													{
														RelatedCharacters = related
													};
												}
											}
											bool flag29 = config.IsRelationCharactersAliveStateNeedSnapshot || (config.ExtraSnapshotParameterIndices != null && config.ExtraSnapshotParameterIndices.Contains(index));
											if (flag29)
											{
												SecretInformationCharacterExtraInfo extraInfo2;
												metaData.CharacterExtraInfoCollection.Collection.TryGetValue(charId, out extraInfo2);
												bool flag30 = deadCharacter != null;
												if (flag30)
												{
													extraInfo2.FameType = deadCharacter.FameType;
													extraInfo2.MonkType = deadCharacter.MonkType;
													extraInfo2.OrgInfo = deadCharacter.OrganizationInfo;
												}
												else
												{
													bool flag31 = chara != null;
													if (flag31)
													{
														extraInfo2.FameType = chara.GetFameType();
														extraInfo2.MonkType = chara.GetMonkType();
														extraInfo2.OrgInfo = chara.GetOrganizationInfo();
													}
												}
												metaData.CharacterExtraInfoCollection.Collection[charId] = extraInfo2;
											}
										}
									}
								}
							}
						}
					}
				}
			}
			this.AddElement_SecretInformationMetaData(nextId, metaData);
			bool flag32 = isTaiwuKeyCharacter;
			if (flag32)
			{
				discoveryRate = (int)config.DiscoveryRateTaiwu;
			}
			else
			{
				int a = (argCharCount > 0) ? (argCharFameSum / argCharCount) : 0;
				a = Math.Max(a * (int)config.DiscoveryRateFactorA, 0);
				int b = (argGradeCount > 0) ? (argGradeSum / argGradeCount) : 0;
				b = Math.Max(b * (int)config.DiscoveryRateFactorB, 0);
				int c = (int)(isKeyCharacterRelatedByTaiwu ? config.DiscoveryRateFactorC : 0);
				discoveryRate = discoveryRate * Math.Max(100 + a + b, 100) / 100;
				discoveryRate = Math.Max(c, discoveryRate);
			}
			bool flag33 = !context.Random.CheckProb(discoveryRate, 10000) && !necessarily;
			if (!flag33)
			{
				bool autoBroadCast = config.AutoBroadCast;
				if (autoBroadCast)
				{
					this.MakeSecretInformationBroadcastEffect(context, nextId, -1);
				}
				else if (withInitialDistribute)
				{
					foreach (int index2 in config.InitialTargetParameterIndices)
					{
						int charId2 = -1;
						bool flag34;
						if (!(config.Parameters[index2] != "Character"))
						{
							EventArgBox eventArgBox7 = argBox;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
							defaultInterpolatedStringHandler.AppendLiteral("arg");
							defaultInterpolatedStringHandler.AppendFormatted<int>(index2);
							flag34 = !eventArgBox7.Get(defaultInterpolatedStringHandler.ToStringAndClear(), ref charId2);
						}
						else
						{
							flag34 = true;
						}
						bool flag35 = flag34;
						if (!flag35)
						{
							set.Clear();
							GameData.Domains.Character.Character character2;
							bool flag36 = DomainManager.Character.TryGetElement_Objects(charId2, out character2) && character2 != null;
							if (flag36)
							{
								sbyte enthusiastic = *(ref character2.GetPersonalities().Items.FixedElementField + 2);
								bool flag37 = character2.GetLocation().IsValid();
								if (flag37)
								{
									switch (config.InitialTarget)
									{
									case ESecretInformationInitialTarget.Area:
										DomainManager.Character.GetAreaPeople(context.Random, character2, set, (sbyte)Math.Clamp((int)((15 + enthusiastic / 2) / 5), 0, 100));
										break;
									case ESecretInformationInitialTarget.Nearest:
										DomainManager.Character.GetClosePeople(context.Random, character2, set, (sbyte)Math.Clamp((int)((30 + enthusiastic) / 5), 0, 100));
										break;
									case ESecretInformationInitialTarget.Local:
										DomainManager.Character.GetSameBlockPeople(context.Random, character2, set, (sbyte)Math.Clamp((int)((60 + enthusiastic * 2) / 5), 0, 100));
										break;
									}
								}
								else
								{
									set.Add(charId2);
								}
								foreach (int targetId in set)
								{
									this.ReceiveSecretInformation(context, nextId, targetId, -1);
								}
								bool flag38 = distributeCallback != null;
								if (flag38)
								{
									distributeCallback(set);
								}
							}
						}
					}
				}
			}
			DomainManager.TaiwuEvent.ReturnArgBox(argBox);
			pool.Return(set);
			return nextId;
		}

		// Token: 0x06005376 RID: 21366 RVA: 0x002D60EC File Offset: 0x002D42EC
		public bool ReceiveSecretInformation(DataContext context, int metaDataId, int charId, int sourceCharId = -1)
		{
			bool flag = this.IsSecretInformationInBroadcast(metaDataId);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				GameData.Domains.Character.Character character;
				bool flag2 = !DomainManager.Character.TryGetElement_Objects(charId, out character) || character.GetAgeGroup() == 0;
				if (flag2)
				{
					result = false;
				}
				else
				{
					SecretInformationCharacterDataCollection characterDataCollection;
					bool flag3 = !this.TryGetElement_CharacterSecretInformation(charId, out characterDataCollection);
					if (flag3)
					{
						this.AddElement_CharacterSecretInformation(charId, characterDataCollection = new SecretInformationCharacterDataCollection(), context);
					}
					SecretInformationMetaData metaData;
					bool flag4 = !characterDataCollection.Collection.ContainsKey(metaDataId) && this.TryGetElement_SecretInformationMetaData(metaDataId, out metaData);
					if (flag4)
					{
						int disseminationBranch = -1;
						bool flag5 = sourceCharId >= 0;
						if (flag5)
						{
							SecretInformationCharacterDataCollection sourceCharacterDataCollection;
							SecretInformationCharacterData sourceCharacterData;
							bool flag6 = this.TryGetElement_CharacterSecretInformation(sourceCharId, out sourceCharacterDataCollection) && sourceCharacterDataCollection.Collection.TryGetValue(metaDataId, out sourceCharacterData) && sourceCharacterData.SecretInformationDisseminationBranch >= 0;
							if (flag6)
							{
								disseminationBranch = sourceCharacterData.SecretInformationDisseminationBranch;
							}
							else
							{
								disseminationBranch = sourceCharId;
							}
							metaData.IncreaseCharacterDisseminationCount(disseminationBranch);
							GameData.Domains.Character.Character disseminationBranchCharacter;
							bool flag7 = DomainManager.Character.TryGetElement_Objects(disseminationBranch, out disseminationBranchCharacter);
							if (flag7)
							{
								short templateId = this.CalcSecretInformationTemplateId(metaData);
								disseminationBranchCharacter.ChangeResource(context, 7, (int)(SecretInformation.Instance[templateId].CostAuthority / 5));
							}
							bool isOfflineUpdate = InformationDomain._isOfflineUpdate;
							if (isOfflineUpdate)
							{
								InformationDomain._offlineIndicesMetaDataCharacterDisseminationData.Add(metaDataId);
							}
							else
							{
								metaData.SetDisseminationData(metaData.GetDisseminationData(), context);
							}
						}
						bool flag8 = charId == DomainManager.Taiwu.GetTaiwuCharId();
						if (flag8)
						{
							this._taiwuReceivedSecretInformationInMonth.Add(metaDataId);
							DomainManager.Taiwu.AddLegacyPoint(context, 34, 100);
							short templateId2 = this.CalcSecretInformationTemplateId(metaData);
							ProfessionFormulaItem formula = ProfessionFormula.Instance[90];
							int addSeniority = formula.Calculate((int)SecretInformation.Instance[templateId2].SortValue);
							DomainManager.Extra.ChangeProfessionSeniority(context, 14, addSeniority, true, false);
						}
						characterDataCollection.Collection.Add(metaDataId, new SecretInformationCharacterData(metaDataId, disseminationBranch));
						DomainManager.Extra.ClearCharacterSecretInformationUsedCount(context, charId, metaDataId);
						bool isOfflineUpdate2 = InformationDomain._isOfflineUpdate;
						if (isOfflineUpdate2)
						{
							this._characterSecretInformation[charId] = characterDataCollection;
							InformationDomain._offlineIndicesCharacterData.Add(charId);
						}
						else
						{
							this.SetElement_CharacterSecretInformation(charId, characterDataCollection, context);
						}
						result = true;
					}
					else
					{
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x06005377 RID: 21367 RVA: 0x002D6324 File Offset: 0x002D4524
		public bool DistributeSecretInformationToCharacter(DataContext context, int metaDataId, int charId, int sourceCharId = -1)
		{
			bool flag = this.IsSecretInformationInBroadcast(metaDataId);
			return flag || DomainManager.Information.ReceiveSecretInformation(context, metaDataId, charId, sourceCharId);
		}

		// Token: 0x06005378 RID: 21368 RVA: 0x002D6354 File Offset: 0x002D4554
		public void CostSecretInformationUsedCount(DataContext context, int charId, int metaDataId)
		{
			sbyte usedCount = DomainManager.Extra.GetCharacterSecretInformationUsedCount(charId, metaDataId);
			int maxUsedCount = this.IsSecretInformationInBroadcast(metaDataId) ? GlobalConfig.Instance.SecretInformationInBroadcastMaxUseCount : GlobalConfig.Instance.SecretInformationInPrivateMaxUseCount;
			usedCount += 1;
			DomainManager.Extra.SetCharacterSecretInformationUsedCount(context, charId, metaDataId, usedCount);
			bool flag = (int)usedCount > maxUsedCount;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 4);
				defaultInterpolatedStringHandler.AppendLiteral("taiwu(");
				defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
				defaultInterpolatedStringHandler.AppendLiteral(") used secret information ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(metaDataId);
				defaultInterpolatedStringHandler.AppendLiteral(" in ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(usedCount);
				defaultInterpolatedStringHandler.AppendLiteral(" times(max: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(maxUsedCount);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				AdaptableLog.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06005379 RID: 21369 RVA: 0x002D642C File Offset: 0x002D462C
		public void MakeSecretInformationBroadcastEffect(DataContext context, int metaDataId, int sourceCharId)
		{
			List<int> characterIds = ObjectPool<List<int>>.Instance.Get();
			characterIds.Clear();
			characterIds.AddRange(this._characterSecretInformation.Keys);
			foreach (int charId in characterIds)
			{
				SecretInformationCharacterDataCollection data = this._characterSecretInformation[charId];
				bool flag = data.Collection.Remove(metaDataId);
				if (flag)
				{
					bool isOfflineUpdate = InformationDomain._isOfflineUpdate;
					if (isOfflineUpdate)
					{
						this._characterSecretInformation[charId] = data;
						InformationDomain._offlineIndicesCharacterData.Add(charId);
					}
					else
					{
						this.SetElement_CharacterSecretInformation(charId, data, context);
					}
					DomainManager.Extra.ClearCharacterSecretInformationUsedCount(context, charId, metaDataId);
				}
			}
			ObjectPool<List<int>>.Instance.Return(characterIds);
			bool flag2 = !this.IsSecretInformationInBroadcast(metaDataId);
			if (flag2)
			{
				TaiwuEventDomain taiwuEventDomain = DomainManager.TaiwuEvent;
				DomainManager.Extra.AddSecretInformationInBroadcastList(context, metaDataId);
				SecretInformationMetaData secretInformationMetaData;
				bool flag3 = this.TryGetElement_SecretInformationMetaData(metaDataId, out secretInformationMetaData);
				if (flag3)
				{
					BroadcastEffect.OpenEffectEntrance(context, metaDataId, InformationDomain._isOfflineUpdate, sourceCharId, ref InformationDomain._StartEnemyRelationItem);
				}
				else
				{
					StringBuilder sb = new StringBuilder();
					StackTrace stackTrace = new StackTrace();
					StringBuilder stringBuilder = sb;
					StringBuilder stringBuilder2 = stringBuilder;
					StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(57, 1, stringBuilder);
					appendInterpolatedStringHandler.AppendLiteral("secret_information: ");
					appendInterpolatedStringHandler.AppendFormatted<int>(metaDataId);
					appendInterpolatedStringHandler.AppendLiteral(" not found when require for broadcast");
					stringBuilder2.AppendLine(ref appendInterpolatedStringHandler);
					foreach (StackFrame frame in stackTrace.GetFrames())
					{
						stringBuilder = sb;
						StringBuilder stringBuilder3 = stringBuilder;
						appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(14, 3, stringBuilder);
						appendInterpolatedStringHandler.AppendLiteral("at ");
						MethodBase method = frame.GetMethod();
						appendInterpolatedStringHandler.AppendFormatted((method != null) ? method.Name : null);
						appendInterpolatedStringHandler.AppendLiteral(" in ");
						appendInterpolatedStringHandler.AppendFormatted(frame.GetFileName());
						appendInterpolatedStringHandler.AppendLiteral(" Line: ");
						appendInterpolatedStringHandler.AppendFormatted<int>(frame.GetFileLineNumber());
						stringBuilder3.AppendLine(ref appendInterpolatedStringHandler);
					}
					AdaptableLog.Warning(sb.ToString(), false);
				}
			}
		}

		// Token: 0x0600537A RID: 21370 RVA: 0x002D6660 File Offset: 0x002D4860
		[DomainMethod]
		public void DiscardSecretInformation(DataContext context, int charId, int metaDataId)
		{
			SecretInformationCharacterDataCollection characterDataCollection;
			bool flag = this.TryGetElement_CharacterSecretInformation(charId, out characterDataCollection) && characterDataCollection.Collection.ContainsKey(metaDataId);
			if (flag)
			{
				characterDataCollection.Collection.Remove(metaDataId);
				bool isOfflineUpdate = InformationDomain._isOfflineUpdate;
				if (isOfflineUpdate)
				{
					this._characterSecretInformation[charId] = characterDataCollection;
					InformationDomain._offlineIndicesCharacterData.Add(charId);
				}
				else
				{
					this.SetElement_CharacterSecretInformation(charId, characterDataCollection, context);
				}
			}
			DomainManager.Extra.ClearCharacterSecretInformationUsedCount(context, charId, metaDataId);
		}

		// Token: 0x0600537B RID: 21371 RVA: 0x002D66DC File Offset: 0x002D48DC
		[DomainMethod]
		public SecretInformationDisplayData GetSecretInformationDisplayData(int metaDataId)
		{
			return this.GetSecretInformationDisplayData(metaDataId, new HashSet<int>());
		}

		// Token: 0x0600537C RID: 21372 RVA: 0x002D66FC File Offset: 0x002D48FC
		public unsafe void CheckSecretInformationValidState(DataContext context, out Dictionary<int, List<int>> offsetRefMap, out HashSet<int> multiRefOffsets, out List<int> danglingOffsetList, bool checkTemplateId = false)
		{
			offsetRefMap = new Dictionary<int, List<int>>();
			multiRefOffsets = new HashSet<int>();
			foreach (SecretInformationMetaData metaData in this._secretInformationMetaData.Values)
			{
				int offset = metaData.GetOffset();
				List<int> refMetaDataList;
				bool flag = offsetRefMap.TryGetValue(offset, out refMetaDataList);
				if (flag)
				{
					multiRefOffsets.Add(offset);
				}
				else
				{
					refMetaDataList = new List<int>();
				}
				refMetaDataList.Add(metaData.GetId());
				offsetRefMap[offset] = refMetaDataList;
			}
			danglingOffsetList = new List<int>();
			int currentPosition = 0;
			int i = 0;
			int count = this._secretInformationCollection.Count;
			while (i < count)
			{
				bool flag2 = currentPosition >= this._secretInformationCollection.RawData.Length;
				if (flag2)
				{
					string tag = "CheckSecretInformationValidState";
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 2);
					defaultInterpolatedStringHandler.AppendLiteral("wrong secretInformationCollection.Count ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this._secretInformationCollection.Count);
					defaultInterpolatedStringHandler.AppendLiteral(", recount to ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(i);
					AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
					this._secretInformationCollection.Count = i;
					this.CommitSetMetadata_SecretInformationCollection(context);
					break;
				}
				short templateId = -1;
				if (checkTemplateId)
				{
					byte[] array;
					byte* pRawData;
					if ((array = this._secretInformationCollection.GetRawData()) == null || array.Length == 0)
					{
						pRawData = null;
					}
					else
					{
						pRawData = &array[0];
					}
					byte* pCurrData = pRawData + currentPosition;
					pCurrData++;
					int date = *(int*)pCurrData;
					pCurrData += 4;
					templateId = *(short*)pCurrData;
					array = null;
				}
				bool flag3 = !offsetRefMap.ContainsKey(currentPosition);
				if (flag3)
				{
					bool flag4 = checkTemplateId && templateId >= 0;
					if (flag4)
					{
						string tag2 = "CheckSecretInformationValidState";
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 2);
						defaultInterpolatedStringHandler.AppendLiteral("secretinformation data offset ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(currentPosition);
						defaultInterpolatedStringHandler.AppendLiteral("[");
						defaultInterpolatedStringHandler.AppendFormatted(SecretInformation.Instance[templateId].Name);
						defaultInterpolatedStringHandler.AppendLiteral("] is dangling");
						AdaptableLog.TagWarning(tag2, defaultInterpolatedStringHandler.ToStringAndClear(), false);
					}
					else
					{
						string tag3 = "CheckSecretInformationValidState";
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
						defaultInterpolatedStringHandler.AppendLiteral("secretinformation data offset ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(currentPosition);
						defaultInterpolatedStringHandler.AppendLiteral(" is dangling");
						AdaptableLog.TagWarning(tag3, defaultInterpolatedStringHandler.ToStringAndClear(), false);
					}
					danglingOffsetList.Add(currentPosition);
				}
				currentPosition += this._secretInformationCollection.GetRecordSize(currentPosition);
				i++;
			}
		}

		// Token: 0x0600537D RID: 21373 RVA: 0x002D69A8 File Offset: 0x002D4BA8
		public void RemoveSenselessSecretInformation(DataContext context)
		{
			ObjectPool<Dictionary<int, int>> pool = ObjectPool<Dictionary<int, int>>.Instance;
			Dictionary<int, int> refCountMap = pool.Get();
			refCountMap.Clear();
			Dictionary<int, IntPair> sourcePool = ObjectPool<Dictionary<int, IntPair>>.Instance.Get();
			sourcePool.Clear();
			foreach (KeyValuePair<int, SecretInformationMetaData> pair in this._secretInformationMetaData)
			{
				refCountMap.Add(pair.Key, 0);
				sourcePool.Add(pair.Key, this.GetMaxDisseminationCounts(pair.Value.GetDisseminationData().DisseminationCounts));
			}
			this._swSecretInformation.Restart();
			foreach (SecretInformationCharacterDataCollection collection in this._characterSecretInformation.Values)
			{
				foreach (int metaDataId in collection.Collection.Keys)
				{
					int refCount;
					bool flag = refCountMap.TryGetValue(metaDataId, out refCount);
					if (flag)
					{
						refCountMap[metaDataId] = refCount + 1;
					}
				}
			}
			this._swSecretInformation.Stop();
			Logger logger = InformationDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
			defaultInterpolatedStringHandler.AppendFormatted("RemoveSenselessSecretInformation");
			defaultInterpolatedStringHandler.AppendLiteral("(calculate character refCount): ");
			defaultInterpolatedStringHandler.AppendFormatted<long>(this._swSecretInformation.ElapsedMilliseconds);
			defaultInterpolatedStringHandler.AppendLiteral(" ms");
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			this._swSecretInformation.Restart();
			List<int> metaDataIds = ObjectPool<List<int>>.Instance.Get();
			metaDataIds.Clear();
			metaDataIds.AddRange(this._secretInformationMetaData.Keys);
			List<int> characterIds = ObjectPool<List<int>>.Instance.Get();
			characterIds.Clear();
			characterIds.AddRange(this._characterSecretInformation.Keys);
			foreach (int metaDataId2 in metaDataIds)
			{
				IntPair refCount2 = sourcePool[metaDataId2];
				SecretInformationMetaData metaData = this.GetElement_SecretInformationMetaData(metaDataId2);
				short templateId;
				int lifeTime = this.CalcSecretInformationRemainingLifeTime(metaData, out templateId);
				SecretInformationItem config = SecretInformation.Instance[templateId];
				bool flag2 = refCount2.Second >= (int)config.MaxPersonAmount;
				if (flag2)
				{
					this.MakeSecretInformationBroadcastEffect(context, metaDataId2, refCount2.First);
					bool flag3 = this.IsSecretInformationInBroadcast(metaDataId2);
					if (flag3)
					{
						refCountMap[metaDataId2] = 0;
					}
				}
				bool flag4 = lifeTime <= 0;
				if (flag4)
				{
					List<int> broadcast = this.GetBroadcastSecretInformation();
					bool flag5 = broadcast.Remove(metaDataId2);
					if (flag5)
					{
						bool flag6 = !InformationDomain._isOfflineUpdate;
						if (flag6)
						{
							this.SetBroadcastSecretInformation(broadcast, context);
						}
					}
					DomainManager.Extra.RemoveSecretInformationInBroadcastList(context, metaDataId2);
					foreach (int charId in characterIds)
					{
						SecretInformationCharacterDataCollection characterDataCollection;
						bool flag7 = this.TryGetElement_CharacterSecretInformation(charId, out characterDataCollection) && characterDataCollection.Collection.Remove(metaDataId2);
						if (flag7)
						{
							bool flag8 = characterDataCollection.Collection.Count <= 0;
							if (flag8)
							{
								bool isOfflineUpdate = InformationDomain._isOfflineUpdate;
								if (isOfflineUpdate)
								{
									bool flag9 = this._characterSecretInformation.Remove(charId);
									if (flag9)
									{
										InformationDomain._offlineIndicesCharacterData.Add(charId);
									}
								}
								else
								{
									this.RemoveElement_CharacterSecretInformation(charId, context);
								}
							}
							else
							{
								bool isOfflineUpdate2 = InformationDomain._isOfflineUpdate;
								if (isOfflineUpdate2)
								{
									InformationDomain._offlineIndicesCharacterData.Add(charId);
								}
								else
								{
									this.SetElement_CharacterSecretInformation(charId, characterDataCollection, context);
								}
							}
							Dictionary<int, int> dictionary = refCountMap;
							int num = metaDataId2;
							dictionary[num]--;
						}
					}
					bool condition = refCountMap[metaDataId2] == 0;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(64, 2);
					defaultInterpolatedStringHandler.AppendLiteral("SecretInformation ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(metaDataId2);
					defaultInterpolatedStringHandler.AppendLiteral(" refCount(");
					defaultInterpolatedStringHandler.AppendFormatted<int>(refCountMap[metaDataId2]);
					defaultInterpolatedStringHandler.AppendLiteral(") must be 0 when it will be deleted.");
					Tester.Assert(condition, defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			ObjectPool<List<int>>.Instance.Return(metaDataIds);
			ObjectPool<List<int>>.Instance.Return(characterIds);
			this._swSecretInformation.Stop();
			Logger logger2 = InformationDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 2);
			defaultInterpolatedStringHandler.AppendFormatted("RemoveSenselessSecretInformation");
			defaultInterpolatedStringHandler.AppendLiteral("(calculate object status): ");
			defaultInterpolatedStringHandler.AppendFormatted<long>(this._swSecretInformation.ElapsedMilliseconds);
			defaultInterpolatedStringHandler.AppendLiteral(" ms");
			logger2.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			foreach (int metaDataId3 in this.GetBroadcastSecretInformationIds())
			{
				int refCount3;
				bool flag10 = refCountMap.TryGetValue(metaDataId3, out refCount3);
				if (flag10)
				{
					refCountMap[metaDataId3] = refCount3 + 1;
				}
			}
			this._swSecretInformation.Restart();
			SecretInformationCollection collection2 = this.GetSecretInformationCollection();
			this._secretInformationRemovingList.Clear();
			foreach (SecretInformationMetaData metaData2 in this._secretInformationMetaData.Values)
			{
				this._secretInformationRemovingList.Add(metaData2.GetOffset(), metaData2.GetId());
			}
			ObjectPool<List<int>> removedCachePool = ObjectPool<List<int>>.Instance;
			List<int> removedSizeArray = removedCachePool.Get();
			List<int> removedPositionArray = removedCachePool.Get();
			int removedSize = 0;
			int removedCount = 0;
			for (int index = 0; index < this._secretInformationRemovingList.Count; index++)
			{
				int metaDataId4 = this._secretInformationRemovingList.Values[index];
				bool flag11 = refCountMap[metaDataId4] >= 1;
				if (!flag11)
				{
					int position = this.GetElement_SecretInformationMetaData(metaDataId4).GetOffset();
					int size = collection2.GetRecordSize(position);
					this.RemoveElement_SecretInformationMetaData(metaDataId4);
					DomainManager.Extra.RemoveSecretInformationOccurredLocation(context, metaDataId4);
					removedCount++;
					removedSize += size;
					bool flag12;
					if (removedSizeArray.Count > 0)
					{
						List<int> list = removedPositionArray;
						int num2 = list[list.Count - 1];
						List<int> list2 = removedSizeArray;
						flag12 = (num2 + list2[list2.Count - 1] == position);
					}
					else
					{
						flag12 = false;
					}
					bool flag13 = flag12;
					if (flag13)
					{
						List<int> list3 = removedSizeArray;
						int num = list3.Count - 1;
						list3[num] += size;
					}
					else
					{
						removedSizeArray.Add(size);
						removedPositionArray.Add(position);
					}
				}
			}
			int currentPosition = -1;
			for (int i = 0; i < removedPositionArray.Count; i++)
			{
				int size2 = removedSizeArray[i];
				int position2 = removedPositionArray[i];
				bool flag14 = currentPosition < 0;
				if (flag14)
				{
					currentPosition = position2;
				}
				int dst = currentPosition;
				int src = position2 + size2;
				int next = (i + 1 == removedPositionArray.Count) ? collection2.Size : removedPositionArray[i + 1];
				int fill = next - src;
				collection2.Move(src, dst, fill);
				this.CommitWrite_SecretInformationCollection(context, dst, fill);
				currentPosition += fill;
			}
			bool flag15 = currentPosition >= 0;
			if (flag15)
			{
				collection2.Remove(currentPosition, removedSize);
				this.CommitRemove_SecretInformationCollection(context, currentPosition, removedSize);
				collection2.Count -= removedCount;
				this.CommitSetMetadata_SecretInformationCollection(context);
			}
			bool flag16 = removedSizeArray.Count > 0;
			if (flag16)
			{
				int updatedIndex = 0;
				int updatedOffset = removedSizeArray[0];
				for (int relocatableIndex = 0; relocatableIndex < this._secretInformationRemovingList.Count; relocatableIndex++)
				{
					int relocatableMetaDataId = this._secretInformationRemovingList.Values[relocatableIndex];
					SecretInformationMetaData relocatableMetaData;
					bool flag17 = !this.TryGetElement_SecretInformationMetaData(relocatableMetaDataId, out relocatableMetaData);
					if (!flag17)
					{
						int relocatableOffset = relocatableMetaData.GetOffset();
						bool flag18 = relocatableOffset < removedPositionArray[updatedIndex];
						if (!flag18)
						{
							bool flag19 = updatedIndex < removedPositionArray.Count - 1 && relocatableOffset > removedPositionArray[updatedIndex + 1];
							if (flag19)
							{
								updatedIndex++;
								updatedOffset += removedSizeArray[updatedIndex];
							}
							relocatableMetaData.UpdateOffset(-updatedOffset);
							bool isOfflineUpdate3 = InformationDomain._isOfflineUpdate;
							if (isOfflineUpdate3)
							{
								InformationDomain._offlineIndicesMetaDataOffset.Add(relocatableMetaDataId);
							}
							else
							{
								relocatableMetaData.SetOffset(relocatableMetaData.GetOffset(), context);
							}
						}
					}
				}
			}
			this._npcPlanCastCount = 0;
			removedCachePool.Return(removedSizeArray);
			removedCachePool.Return(removedPositionArray);
			this._swSecretInformation.Stop();
			Logger logger3 = InformationDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 3);
			defaultInterpolatedStringHandler.AppendFormatted("RemoveSenselessSecretInformation");
			defaultInterpolatedStringHandler.AppendLiteral("(remove objects ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(removedCount);
			defaultInterpolatedStringHandler.AppendLiteral("): ");
			defaultInterpolatedStringHandler.AppendFormatted<long>(this._swSecretInformation.ElapsedMilliseconds);
			defaultInterpolatedStringHandler.AppendLiteral(" ms");
			logger3.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			pool.Return(refCountMap);
			ObjectPool<Dictionary<int, IntPair>>.Instance.Return(sourcePool);
			Dictionary<int, List<int>> dictionary2;
			HashSet<int> multiRefOffsets;
			List<int> danglingOffsetList;
			this.CheckSecretInformationValidState(context, out dictionary2, out multiRefOffsets, out danglingOffsetList, true);
			Tester.Assert(multiRefOffsets.Count == 0, "multiRefOffsets.Count == 0");
			Tester.Assert(danglingOffsetList.Count == 0, "danglingOffsetList.Count == 0");
		}

		// Token: 0x0600537E RID: 21374 RVA: 0x002D73D4 File Offset: 0x002D55D4
		private void SecretInformationWipeAll(DataContext context)
		{
			SecretInformationCollection collection = this.GetSecretInformationCollection();
			int size = collection.Size;
			collection.Clear();
			collection.Count = 0;
			this.CommitRemove_SecretInformationCollection(context, 0, size);
			this.CommitSetMetadata_SecretInformationCollection(context);
			this.ClearSecretInformationMetaData();
			this.ClearCharacterSecretInformation(context);
			List<int> broadcastSecretInformationLegacy = this.GetBroadcastSecretInformation();
			if (broadcastSecretInformationLegacy != null)
			{
				broadcastSecretInformationLegacy.Clear();
			}
			this.SetBroadcastSecretInformation(broadcastSecretInformationLegacy, context);
			DomainManager.Extra.ClearCharacterAllSecretInformationUsedCount(context);
			DomainManager.Extra.ClearAllSecretInformationOccurredLocation(context);
			DomainManager.Extra.ClearSecretInformationInBroadcastList(context);
			this.ClearedTaiwuReceivedSecretInformationInMonth();
			DomainManager.Extra.ClearSecretInformationBroadcastNotifyList(context);
		}

		// Token: 0x0600537F RID: 21375 RVA: 0x002D7480 File Offset: 0x002D5680
		private IntPair GetMaxDisseminationCounts(IDictionary<int, int> disseminationCounts)
		{
			int maxCount = 0;
			int sourceCharId = -1;
			foreach (KeyValuePair<int, int> pair in disseminationCounts)
			{
				bool flag = pair.Value > maxCount;
				if (flag)
				{
					maxCount = pair.Value;
					sourceCharId = pair.Key;
				}
			}
			return new IntPair(sourceCharId, maxCount);
		}

		// Token: 0x06005380 RID: 21376 RVA: 0x002D74FC File Offset: 0x002D56FC
		public unsafe void PlanDisseminateSecretInformation(DataContext context, int charId)
		{
			InformationDomain.<>c__DisplayClass110_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.charId = charId;
			CharacterDomain characterDomain = DomainManager.Character;
			SecretInformationCharacterDataCollection characterDataCollection;
			GameData.Domains.Character.Character character;
			bool flag = !this.TryGetElement_CharacterSecretInformation(CS$<>8__locals1.charId, out characterDataCollection) || !characterDomain.TryGetElement_Objects(CS$<>8__locals1.charId, out character);
			if (!flag)
			{
				Location location = character.GetLocation();
				bool flag2 = !location.IsValid();
				if (!flag2)
				{
					List<SecretInformationMetaData> sources = ObjectPool<List<SecretInformationMetaData>>.Instance.Get();
					sources.Clear();
					foreach (KeyValuePair<int, SecretInformationCharacterData> pair in characterDataCollection.Collection)
					{
						bool flag3 = this.IsSecretInformationInBroadcast(pair.Key);
						if (!flag3)
						{
							SecretInformationMetaData metaData;
							bool flag4 = !this.TryGetElement_SecretInformationMetaData(pair.Key, out metaData);
							if (!flag4)
							{
								short templateId;
								bool flag5 = this.CalcSecretInformationRemainingLifeTime(metaData, out templateId) <= 0;
								if (!flag5)
								{
									SecretInformationItem config = SecretInformation.Instance.GetItem(templateId);
									bool flag6 = config != null && !config.AutoDissemination;
									if (!flag6)
									{
										bool flag7 = this.CalcSecretInformationAuthorityCostWhenDisseminatingByCharacter(templateId, character.GetFameType(), this.GetSecretInformationDisseminatingCountOfBranch(pair.Key, pair.Value.SecretInformationDisseminationBranch)) > character.GetResource(7);
										if (!flag7)
										{
											sources.Add(metaData);
										}
									}
								}
							}
						}
					}
					bool flag8 = sources.Count > 0;
					if (flag8)
					{
						SecretInformationMetaData metaData2 = sources.GetRandom(context.Random);
						int metaDataId = metaData2.GetId();
						int usedCount = 0;
						int usedCountMax = this.IsSecretInformationInBroadcast(metaDataId) ? GlobalConfig.Instance.SecretInformationInBroadcastMaxUseCount : GlobalConfig.Instance.SecretInformationInPrivateMaxUseCount;
						usedCount = (int)DomainManager.Extra.GetCharacterSecretInformationUsedCount(character.GetId(), metaDataId);
						bool flag9 = usedCount < usedCountMax;
						if (flag9)
						{
							InformationDomain.<>c__DisplayClass110_1 CS$<>8__locals2;
							CS$<>8__locals2.relatives = ObjectPool<Dictionary<int, int>>.Instance.Get();
							CS$<>8__locals2.relatives.Clear();
							SecretInformationCollection collection = this.GetSecretInformationCollection();
							int offset = metaData2.GetOffset();
							byte[] array;
							byte* pRawData;
							if ((array = collection.GetRawData()) == null || array.Length == 0)
							{
								pRawData = null;
							}
							else
							{
								pRawData = &array[0];
							}
							byte* pCurrData = pRawData + offset;
							pCurrData++;
							int date = *(int*)pCurrData;
							pCurrData += 4;
							short templateId2 = *(short*)pCurrData;
							pCurrData += 2;
							SecretInformationItem config2 = SecretInformation.Instance[templateId2];
							bool flag10 = config2.Parameters != null;
							if (flag10)
							{
								int i = 0;
								int len = config2.Parameters.Length;
								while (i < len)
								{
									string type = config2.Parameters[i];
									bool flag11 = string.IsNullOrEmpty(type);
									if (!flag11)
									{
										DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
										defaultInterpolatedStringHandler.AppendLiteral("arg");
										defaultInterpolatedStringHandler.AppendFormatted<int>(i);
										string name = defaultInterpolatedStringHandler.ToStringAndClear();
										string text = type;
										string text2 = text;
										uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
										if (num <= 1539345862U)
										{
											if (num != 598792213U)
											{
												if (num != 1347445242U)
												{
													if (num != 1539345862U)
													{
														goto IL_41D;
													}
													if (!(text2 == "Location"))
													{
														goto IL_41D;
													}
													pCurrData += 2;
													pCurrData += 2;
												}
												else
												{
													if (!(text2 == "CombatSkill"))
													{
														goto IL_41D;
													}
													pCurrData += 2;
												}
											}
											else
											{
												if (!(text2 == "Resource"))
												{
													goto IL_41D;
												}
												pCurrData++;
											}
										}
										else if (num <= 3651752933U)
										{
											if (num != 3197690557U)
											{
												if (num != 3651752933U)
												{
													goto IL_41D;
												}
												if (!(text2 == "Integer"))
												{
													goto IL_41D;
												}
												pCurrData += 4;
											}
											else
											{
												if (!(text2 == "ItemKey"))
												{
													goto IL_41D;
												}
												pCurrData += 8;
											}
										}
										else if (num != 3966976176U)
										{
											if (num != 3992405204U)
											{
												goto IL_41D;
											}
											if (!(text2 == "LifeSkill"))
											{
												goto IL_41D;
											}
											pCurrData += 2;
										}
										else
										{
											if (!(text2 == "Character"))
											{
												goto IL_41D;
											}
											CS$<>8__locals2.relatives.Add(i, *(int*)pCurrData);
											pCurrData += 4;
										}
										goto IL_426;
										IL_41D:
										throw new NotImplementedException(type);
									}
									IL_426:
									i++;
								}
							}
							array = null;
							int authorityCost = this.CalcSecretInformationAuthorityCostWhenDisseminatingByCharacter(config2.TemplateId, character.GetFameType(), this.GetSecretInformationDisseminatingCountOfBranch(metaDataId, characterDataCollection.Collection[metaDataId].SecretInformationDisseminationBranch));
							InformationDomain.<>c__DisplayClass110_2 CS$<>8__locals3;
							CS$<>8__locals3.dissemination = SecretInformationDissemination.Instance[config2.DisseminationId];
							CS$<>8__locals3.effect = SecretInformationEffect.Instance[config2.DefaultEffectId];
							Personalities personalities = character.GetPersonalities();
							CS$<>8__locals3.isSelfIntroduction = CS$<>8__locals2.relatives.ContainsValue(CS$<>8__locals1.charId);
							CS$<>8__locals3.r = new HashSet<SecretInformationRelationshipType>();
							int rateBase = 0;
							bool isSelfIntroduction = CS$<>8__locals3.isSelfIntroduction;
							if (isSelfIntroduction)
							{
								rateBase += (int)CS$<>8__locals3.dissemination.SfBehaviorTypeDiff[(int)character.GetBehaviorType()];
								for (int j = 0; j < 5; j++)
								{
									rateBase += (int)(CS$<>8__locals3.dissemination.SfPersonalityDiff[j] * (short)(*(ref personalities.Items.FixedElementField + j)));
								}
							}
							else
							{
								rateBase += (int)CS$<>8__locals3.dissemination.TfBehaviorTypeDiff[(int)character.GetBehaviorType()];
								for (int k = 0; k < 5; k++)
								{
									rateBase += (int)(CS$<>8__locals3.dissemination.TfPersonalityDiff[k] * (short)(*(ref personalities.Items.FixedElementField + k)));
								}
								int actorId;
								bool flag12 = CS$<>8__locals2.relatives.TryGetValue(CS$<>8__locals3.effect.ActorIndex, out actorId);
								if (flag12)
								{
									CS$<>8__locals3.r.Clear();
									this.CheckSecretInformationRelationship(CS$<>8__locals1.charId, -1, actorId, -1, CS$<>8__locals3.r);
									bool flag13 = CS$<>8__locals3.r.Contains(SecretInformationRelationshipType.Relative) || CS$<>8__locals3.r.Contains(SecretInformationRelationshipType.Friend);
									if (flag13)
									{
										rateBase += (int)CS$<>8__locals3.dissemination.TfRateDiffWhenActFri;
									}
									else
									{
										bool flag14 = CS$<>8__locals3.r.Contains(SecretInformationRelationshipType.Enemy);
										if (flag14)
										{
											rateBase += (int)CS$<>8__locals3.dissemination.TfRateDiffWhenActEnm;
										}
									}
								}
								int reactorId;
								bool flag15 = CS$<>8__locals2.relatives.TryGetValue(CS$<>8__locals3.effect.ActorIndex, out reactorId);
								if (flag15)
								{
									CS$<>8__locals3.r.Clear();
									this.CheckSecretInformationRelationship(CS$<>8__locals1.charId, -1, reactorId, -1, CS$<>8__locals3.r);
									bool flag16 = CS$<>8__locals3.r.Contains(SecretInformationRelationshipType.Relative) || CS$<>8__locals3.r.Contains(SecretInformationRelationshipType.Friend);
									if (flag16)
									{
										rateBase += (int)CS$<>8__locals3.dissemination.TfRateDiffWhenUnaFri;
									}
									else
									{
										bool flag17 = CS$<>8__locals3.r.Contains(SecretInformationRelationshipType.Enemy);
										if (flag17)
										{
											rateBase += (int)CS$<>8__locals3.dissemination.TfRateDiffWhenUnaEnm;
										}
									}
								}
							}
							MapBlockData block = DomainManager.Map.GetBlock(location);
							ByteCoordinate coord = block.GetBlockPos();
							int c = 0;
							bool flag18 = config2.DiffusionRange >= 0;
							if (flag18)
							{
								Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(location.AreaId);
								for (int l = 0; l < areaBlocks.Length; l++)
								{
									MapBlockData targetBlock = *areaBlocks[l];
									bool flag19 = targetBlock.GetManhattanDistanceToPos(coord.X, coord.Y) != (byte)config2.DiffusionRange;
									if (!flag19)
									{
										bool flag20 = targetBlock.CharacterSet != null;
										if (flag20)
										{
											foreach (int targetCharId in targetBlock.CharacterSet)
											{
												int rate = this.<PlanDisseminateSecretInformation>g__GetRateByRelation|110_0(targetCharId, ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3) + rateBase;
												bool flag21 = context.Random.Next(100) < rate;
												if (flag21)
												{
													this.ReceiveSecretInformation(context, metaDataId, targetCharId, CS$<>8__locals1.charId);
													usedCount++;
													c++;
												}
												bool flag22 = c >= (int)config2.DiffusionSpeed || usedCount >= usedCountMax;
												if (flag22)
												{
													break;
												}
											}
										}
										bool flag23 = c >= (int)config2.DiffusionSpeed || usedCount >= usedCountMax;
										if (flag23)
										{
											break;
										}
									}
								}
							}
							else
							{
								int distance = 0;
								while (rateBase > 0)
								{
									Span<MapBlockData> areaBlocks2 = DomainManager.Map.GetAreaBlocks(location.AreaId);
									for (int m = 0; m < areaBlocks2.Length; m++)
									{
										MapBlockData targetBlock2 = *areaBlocks2[m];
										bool flag24 = (int)targetBlock2.GetManhattanDistanceToPos(coord.X, coord.Y) != distance;
										if (!flag24)
										{
											bool flag25 = targetBlock2.CharacterSet != null;
											if (flag25)
											{
												foreach (int targetCharId2 in targetBlock2.CharacterSet)
												{
													int rate2 = this.<PlanDisseminateSecretInformation>g__GetRateByRelation|110_0(targetCharId2, ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3) + rateBase;
													bool flag26 = context.Random.Next(100) < rate2;
													if (flag26)
													{
														this.ReceiveSecretInformation(context, metaDataId, targetCharId2, CS$<>8__locals1.charId);
														usedCount++;
														c++;
													}
													bool flag27 = c >= (int)config2.DiffusionSpeed || usedCount >= usedCountMax;
													if (flag27)
													{
														break;
													}
												}
											}
											bool flag28 = c >= (int)config2.DiffusionSpeed || usedCount >= usedCountMax;
											if (flag28)
											{
												break;
											}
										}
									}
									bool flag29 = c >= (int)config2.DiffusionSpeed || usedCount >= usedCountMax;
									if (flag29)
									{
										break;
									}
									distance++;
									rateBase += (int)(config2.DiffusionRange * 10);
								}
							}
							this._npcPlanCastCount += c;
							authorityCost *= c;
							character.ChangeResource(context, 7, -Math.Min(character.GetResource(7), authorityCost));
							DomainManager.Extra.SetCharacterSecretInformationUsedCount(context, CS$<>8__locals1.charId, metaDataId, (sbyte)Math.Clamp(usedCount, 0, usedCountMax));
							ObjectPool<Dictionary<int, int>>.Instance.Return(CS$<>8__locals2.relatives);
						}
					}
					ObjectPool<List<SecretInformationMetaData>>.Instance.Return(sources);
				}
			}
		}

		// Token: 0x06005381 RID: 21377 RVA: 0x002D7F7C File Offset: 0x002D617C
		private RelatedCharacters GetSecretInformationCharacterRelationSnapshot(int metaDataId, int characterId)
		{
			SecretInformationCharacterRelationshipSnapshot snapshot;
			return this.GetElement_SecretInformationMetaData(metaDataId).CharacterRelationshipSnapshotCollection.Collection.TryGetValue(characterId, out snapshot) ? snapshot.RelatedCharacters : null;
		}

		// Token: 0x06005382 RID: 21378 RVA: 0x002D7FB2 File Offset: 0x002D61B2
		public HashSet<SecretInformationRelationshipType> CheckSecretInformationRelationship(int characterId, int selfMetaDataIdForSnapshot, int targetCharacterId, int targetMetaDataIdForSnapshot)
		{
			return this.CheckSecretInformationRelationship(characterId, selfMetaDataIdForSnapshot, targetCharacterId, targetMetaDataIdForSnapshot, new HashSet<SecretInformationRelationshipType>());
		}

		// Token: 0x06005383 RID: 21379 RVA: 0x002D7FC4 File Offset: 0x002D61C4
		public HashSet<SecretInformationRelationshipType> CheckSecretInformationRelationship(int characterId, int selfMetaDataIdForSnapshot, int targetCharacterId, int targetMetaDataIdForSnapshot, HashSet<SecretInformationRelationshipType> container)
		{
			CharacterDomain characterDomain = DomainManager.Character;
			OrganizationDomain organizationDomain = DomainManager.Organization;
			RelatedCharacters related = (selfMetaDataIdForSnapshot < 0) ? characterDomain.GetRelatedCharacters(characterId) : this.GetSecretInformationCharacterRelationSnapshot(selfMetaDataIdForSnapshot, characterId);
			RelatedCharacters targetRelated = (targetMetaDataIdForSnapshot < 0) ? characterDomain.GetRelatedCharacters(targetCharacterId) : this.GetSecretInformationCharacterRelationSnapshot(targetMetaDataIdForSnapshot, targetCharacterId);
			bool flag = related != null && (related.BloodParents.Contains(targetCharacterId) || related.StepParents.Contains(targetCharacterId) || related.AdoptiveParents.Contains(targetCharacterId) || related.BloodChildren.Contains(targetCharacterId) || related.StepChildren.Contains(targetCharacterId) || related.AdoptiveChildren.Contains(targetCharacterId) || related.BloodBrothersAndSisters.Contains(targetCharacterId) || related.StepBrothersAndSisters.Contains(targetCharacterId) || related.AdoptiveBrothersAndSisters.Contains(targetCharacterId));
			if (flag)
			{
				container.Add(SecretInformationRelationshipType.Relative);
				container.Add(SecretInformationRelationshipType.Allied);
			}
			bool flag2 = (related != null && related.Friends.Contains(targetCharacterId)) || (targetRelated != null && targetRelated.Friends.Contains(characterId));
			if (flag2)
			{
				container.Add(SecretInformationRelationshipType.Friend);
				container.Add(SecretInformationRelationshipType.Allied);
			}
			bool flag3 = (related != null && related.SwornBrothersAndSisters.Contains(targetCharacterId)) || (targetRelated != null && targetRelated.SwornBrothersAndSisters.Contains(characterId));
			if (flag3)
			{
				container.Add(SecretInformationRelationshipType.SwornBrotherOrSister);
				container.Add(SecretInformationRelationshipType.Allied);
			}
			bool flag4 = (related != null && related.HusbandsAndWives.Contains(targetCharacterId)) || (targetRelated != null && targetRelated.HusbandsAndWives.Contains(characterId));
			if (flag4)
			{
				container.Add(SecretInformationRelationshipType.HusbandOrWife);
				container.Add(SecretInformationRelationshipType.Allied);
			}
			bool flag5 = targetRelated != null && targetRelated.Adored.Contains(characterId);
			if (flag5)
			{
				bool flag6 = related != null && related.Adored.Contains(targetCharacterId);
				if (flag6)
				{
					container.Add(SecretInformationRelationshipType.Lover);
				}
				container.Add(SecretInformationRelationshipType.Adorer);
				container.Add(SecretInformationRelationshipType.Allied);
			}
			bool flag7 = targetRelated != null && targetRelated.Enemies.Contains(characterId);
			if (flag7)
			{
				container.Add(SecretInformationRelationshipType.Enemy);
			}
			OrganizationInfo selfOrgInfo = new OrganizationInfo(-1, -1, true, -1);
			OrganizationInfo targetOrgInfo = new OrganizationInfo(-1, -1, true, -1);
			GameData.Domains.Character.Character character;
			bool flag8 = characterDomain.TryGetElement_Objects(characterId, out character);
			if (flag8)
			{
				selfOrgInfo = character.GetOrganizationInfo();
			}
			else
			{
				DeadCharacter deadCharacter;
				bool flag9 = (deadCharacter = characterDomain.TryGetDeadCharacter(characterId)) != null;
				if (flag9)
				{
					selfOrgInfo = deadCharacter.OrganizationInfo;
				}
			}
			bool flag10 = characterDomain.TryGetElement_Objects(targetCharacterId, out character);
			if (flag10)
			{
				targetOrgInfo = character.GetOrganizationInfo();
			}
			else
			{
				DeadCharacter deadCharacter;
				bool flag11 = (deadCharacter = characterDomain.TryGetDeadCharacter(targetCharacterId)) != null;
				if (flag11)
				{
					targetOrgInfo = deadCharacter.OrganizationInfo;
				}
			}
			bool flag12 = selfOrgInfo.OrgTemplateId >= 0 && Organization.Instance[selfOrgInfo.OrgTemplateId].IsSect && selfOrgInfo.OrgTemplateId == targetOrgInfo.OrgTemplateId;
			if (flag12)
			{
				container.Add(SecretInformationRelationshipType.Comrade);
			}
			bool flag13 = (related != null && (related.Mentors.Contains(targetCharacterId) || related.Mentees.Contains(targetCharacterId))) || (targetRelated != null && (targetRelated.Mentors.Contains(characterId) || targetRelated.Mentees.Contains(characterId)));
			if (flag13)
			{
				container.Add(SecretInformationRelationshipType.MentorAndMentee);
				container.Add(SecretInformationRelationshipType.Allied);
			}
			int bloodParent = characterDomain.GetBloodParent(characterId, 1);
			bool flag14 = bloodParent >= 0;
			if (flag14)
			{
				int actualBloodParentId;
				bool flag15 = !characterDomain.TryGetActualBloodParent(bloodParent, characterId, out actualBloodParentId);
				if (flag15)
				{
					actualBloodParentId = bloodParent;
				}
				bool flag16 = actualBloodParentId == targetCharacterId;
				if (flag16)
				{
					container.Add(SecretInformationRelationshipType.ActualBloodFather);
				}
			}
			return container;
		}

		// Token: 0x06005384 RID: 21380 RVA: 0x002D8360 File Offset: 0x002D6560
		internal bool RequestShopSecretInformationIdShouldExclude(int secretId, EventArgBox argBox, int charId)
		{
			bool flag = !SecretInformationDisplayData.IsSecretInformationValid(secretId);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = this.IsSecretInformationInBroadcast(secretId);
				if (flag2)
				{
					result = true;
				}
				else
				{
					bool flag3 = this.IsCharacterIsOwningSecretInformation(DomainManager.Taiwu.GetTaiwuCharId(), secretId);
					if (flag3)
					{
						result = true;
					}
					else
					{
						SecretInformationMetaData metaData;
						bool flag4 = this.TryGetElement_SecretInformationMetaData(secretId, out metaData);
						if (flag4)
						{
							SecretInformationItem config = SecretInformation.Instance[this.CalcSecretInformationTemplateId(metaData)];
							argBox.Clear();
							argBox = this.MakeSecretInformationEventArgBox(metaData, argBox);
							bool flag5 = config.Parameters.Length != 0 && config.Parameters[0] == "Character" && argBox.GetInt("arg0") == charId;
							if (flag5)
							{
								return true;
							}
							bool flag6 = config.Parameters.Length > 1 && config.Parameters[1] == "Character" && argBox.GetInt("arg1") == charId;
							if (flag6)
							{
								return true;
							}
						}
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x06005385 RID: 21381 RVA: 0x002D8468 File Offset: 0x002D6668
		public ICollection<int> RequestShopSecretInformationIdList(DataContext dataContext, int charId)
		{
			HashSet<int> result = new HashSet<int>();
			SecretInformationShopCharacterData shopData = DomainManager.Extra.AddOrGetSecretInformationShopCharacterData(dataContext, charId, true);
			result.UnionWith(shopData.CollectedSecretInformationIds);
			return result;
		}

		// Token: 0x06005386 RID: 21382 RVA: 0x002D849C File Offset: 0x002D669C
		[DomainMethod]
		public void SettleSecretInformationShopTrade(DataContext context, List<IntPair> secretList, int shopCharId)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			GameData.Domains.Character.Character shopChar = DomainManager.Character.GetElement_Objects(shopCharId);
			SecretInformationShopCharacterData shopCharacterData = DomainManager.Extra.AddOrGetSecretInformationShopCharacterData(context, shopCharId, true);
			bool anyBought = false;
			foreach (IntPair secretPair in secretList)
			{
				int secretId = secretPair.First;
				int money = secretPair.Second;
				this.ReceiveSecretInformation(context, secretId, taiwu.GetId(), taiwu.GetId());
				anyBought = true;
				shopCharacterData.CollectedSecretInformationIds.Remove(secretId);
				taiwu.ChangeResource(context, 6, -money);
				shopChar.ChangeResource(context, 6, money);
			}
			bool flag = anyBought;
			if (flag)
			{
				DomainManager.TaiwuEvent.SetListenerEventActionBoolArg("ShopActionComplete", "ConchShip_PresetKey_ShopHasAnyTrade", true);
			}
			bool flag2 = shopCharacterData.CollectedSecretInformationIds.Count == 0;
			if (flag2)
			{
				shopCharacterData.CollectedSecretInformationIds.Add(-1);
			}
			DomainManager.Extra.SetSecretInformationShopCharacterData(context, shopCharId, shopCharacterData);
		}

		// Token: 0x06005387 RID: 21383 RVA: 0x002D85AC File Offset: 0x002D67AC
		internal void ReleaseAllJingangInformation(DataContext ctx)
		{
			foreach (int handlerId in this._characterSecretInformation.Keys.ToArray<int>())
			{
				foreach (int secretId in this.GetSecretInformationOfCharacter(handlerId, false))
				{
					bool flag = EventHelper.JingangSecretInformationIsSolveScripture(secretId);
					if (flag)
					{
						this.DiscardSecretInformation(ctx, handlerId, secretId);
					}
				}
			}
			foreach (int secretId2 in this.GetBroadcastSecretInformationIds().ToArray<int>())
			{
				bool flag2 = EventHelper.JingangSecretInformationIsSolveScripture(secretId2);
				if (flag2)
				{
					DomainManager.Extra.RemoveSecretInformationInBroadcastList(ctx, secretId2);
				}
			}
		}

		// Token: 0x06005388 RID: 21384 RVA: 0x002D8688 File Offset: 0x002D6888
		internal void RemoveAllNotExistBroadcastSecretInformation(DataContext ctx)
		{
			ExtraDomain extraDomain = DomainManager.Extra;
			foreach (int secretId in extraDomain.GetSecretInformationInBroadcastList().ToArray<int>())
			{
				SecretInformationMetaData secretInformationMetaData;
				bool flag = this.TryGetElement_SecretInformationMetaData(secretId, out secretInformationMetaData);
				if (!flag)
				{
					extraDomain.RemoveSecretInformationInBroadcastList(ctx, secretId);
				}
			}
		}

		// Token: 0x06005389 RID: 21385 RVA: 0x002D86D8 File Offset: 0x002D68D8
		internal IReadOnlyCollection<int> SecretInformationAllHandledCharacterIds()
		{
			return this._characterSecretInformation.Keys;
		}

		// Token: 0x0600538A RID: 21386 RVA: 0x002D86F8 File Offset: 0x002D68F8
		internal IReadOnlyCollection<int> QuerySecretInformationIdsHandledByCharacter(int characterId)
		{
			SecretInformationCharacterDataCollection collection;
			bool flag = this.TryGetElement_CharacterSecretInformation(characterId, out collection) && collection.Collection != null;
			IReadOnlyCollection<int> result;
			if (flag)
			{
				result = (collection.Collection.Keys as IReadOnlyCollection<int>);
			}
			else
			{
				result = Array.Empty<int>();
			}
			return result;
		}

		// Token: 0x0600538B RID: 21387 RVA: 0x002D8740 File Offset: 0x002D6940
		internal bool IsCharacterIsOwningSecretInformation(int characterId, int secretId)
		{
			SecretInformationCharacterDataCollection collection;
			bool flag = this.TryGetElement_CharacterSecretInformation(characterId, out collection) && collection.Collection != null;
			return flag && collection.Collection.ContainsKey(secretId);
		}

		// Token: 0x0600538C RID: 21388 RVA: 0x002D8780 File Offset: 0x002D6980
		internal unsafe int QuerySecretInformationOccurrenceDate(int secretId)
		{
			SecretInformationMetaData secretInformationMetaData;
			bool flag = this.TryGetElement_SecretInformationMetaData(secretId, out secretInformationMetaData);
			if (flag)
			{
				SecretInformationCollection collection = this.GetSecretInformationCollection();
				int offset = secretInformationMetaData.GetOffset();
				byte[] array;
				byte* pRawData;
				if ((array = collection.GetRawData()) == null || array.Length == 0)
				{
					pRawData = null;
				}
				else
				{
					pRawData = &array[0];
				}
				byte* pCurrData = pRawData + offset;
				pCurrData++;
				int date = *(int*)pCurrData;
				pCurrData += 4;
				array = null;
			}
			return -1;
		}

		// Token: 0x0600538D RID: 21389 RVA: 0x002D87F0 File Offset: 0x002D69F0
		public InformationDomain() : base(10)
		{
			this._information = new Dictionary<int, NormalInformationCollection>(0);
			this._secretInformationCollection = new SecretInformationCollection();
			this._secretInformationMetaData = new Dictionary<int, SecretInformationMetaData>(0);
			this._nextMetaDataId = 0;
			this._characterSecretInformation = new Dictionary<int, SecretInformationCharacterDataCollection>(0);
			this._broadcastSecretInformation = new List<int>();
			this._taiwuReceivedNormalInformationInMonth = new List<NormalInformation>();
			this._taiwuReceivedSecretInformationInMonth = new List<int>();
			this._taiwuReceivedInformation = new List<int>();
			this._taiwuTmpInformation = new List<NormalInformation>();
			this.HelperDataSecretInformationMetaData = new ObjectCollectionHelperData(18, 2, InformationDomain.CacheInfluencesSecretInformationMetaData, this._dataStatesSecretInformationMetaData, true);
			this.OnInitializedDomainData();
		}

		// Token: 0x0600538E RID: 21390 RVA: 0x002D8900 File Offset: 0x002D6B00
		public NormalInformationCollection GetElement_Information(int elementId)
		{
			return this._information[elementId];
		}

		// Token: 0x0600538F RID: 21391 RVA: 0x002D8920 File Offset: 0x002D6B20
		public bool TryGetElement_Information(int elementId, out NormalInformationCollection value)
		{
			return this._information.TryGetValue(elementId, out value);
		}

		// Token: 0x06005390 RID: 21392 RVA: 0x002D8940 File Offset: 0x002D6B40
		private unsafe void AddElement_Information(int elementId, NormalInformationCollection value, DataContext context)
		{
			this._information.Add(elementId, value);
			this._modificationsInformation.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, this.DataStates, InformationDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<int>(18, 0, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<int>(18, 0, elementId, 0);
			}
		}

		// Token: 0x06005391 RID: 21393 RVA: 0x002D89B0 File Offset: 0x002D6BB0
		private unsafe void SetElement_Information(int elementId, NormalInformationCollection value, DataContext context)
		{
			this._information[elementId] = value;
			this._modificationsInformation.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, this.DataStates, InformationDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<int>(18, 0, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<int>(18, 0, elementId, 0);
			}
		}

		// Token: 0x06005392 RID: 21394 RVA: 0x002D8A1F File Offset: 0x002D6C1F
		private void RemoveElement_Information(int elementId, DataContext context)
		{
			this._information.Remove(elementId);
			this._modificationsInformation.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, this.DataStates, InformationDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<int>(18, 0, elementId);
		}

		// Token: 0x06005393 RID: 21395 RVA: 0x002D8A59 File Offset: 0x002D6C59
		private void ClearInformation(DataContext context)
		{
			this._information.Clear();
			this._modificationsInformation.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, this.DataStates, InformationDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(18, 0);
		}

		// Token: 0x06005394 RID: 21396 RVA: 0x002D8A90 File Offset: 0x002D6C90
		public SecretInformationCollection GetSecretInformationCollection()
		{
			return this._secretInformationCollection;
		}

		// Token: 0x06005395 RID: 21397 RVA: 0x002D8AA8 File Offset: 0x002D6CA8
		private unsafe void CommitInsert_SecretInformationCollection(DataContext context, int offset, int size)
		{
			this._modificationsSecretInformationCollection.RecordInserting(offset, size);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, this.DataStates, InformationDomain.CacheInfluences, context);
			byte* pData = OperationAdder.Binary_Insert(18, 1, offset, size);
			this._secretInformationCollection.CopyTo(offset, size, pData);
		}

		// Token: 0x06005396 RID: 21398 RVA: 0x002D8AF4 File Offset: 0x002D6CF4
		private unsafe void CommitWrite_SecretInformationCollection(DataContext context, int offset, int size)
		{
			this._modificationsSecretInformationCollection.RecordWriting(offset, size);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, this.DataStates, InformationDomain.CacheInfluences, context);
			byte* pData = OperationAdder.Binary_Write(18, 1, offset, size);
			this._secretInformationCollection.CopyTo(offset, size, pData);
		}

		// Token: 0x06005397 RID: 21399 RVA: 0x002D8B3D File Offset: 0x002D6D3D
		private void CommitRemove_SecretInformationCollection(DataContext context, int offset, int size)
		{
			this._modificationsSecretInformationCollection.RecordRemoving(offset, size);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, this.DataStates, InformationDomain.CacheInfluences, context);
			OperationAdder.Binary_Remove(18, 1, offset, size);
		}

		// Token: 0x06005398 RID: 21400 RVA: 0x002D8B6C File Offset: 0x002D6D6C
		private unsafe void CommitSetMetadata_SecretInformationCollection(DataContext context)
		{
			this._modificationsSecretInformationCollection.RecordSettingMetadata();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, this.DataStates, InformationDomain.CacheInfluences, context);
			ushort metadataSize = this._secretInformationCollection.GetSerializedFixedSizeOfMetadata();
			byte* pData = OperationAdder.Binary_SetMetadata(18, 1, metadataSize);
			this._secretInformationCollection.SerializeMetadata(pData);
		}

		// Token: 0x06005399 RID: 21401 RVA: 0x002D8BBC File Offset: 0x002D6DBC
		public SecretInformationMetaData GetElement_SecretInformationMetaData(int objectId)
		{
			return this._secretInformationMetaData[objectId];
		}

		// Token: 0x0600539A RID: 21402 RVA: 0x002D8BDC File Offset: 0x002D6DDC
		public bool TryGetElement_SecretInformationMetaData(int objectId, out SecretInformationMetaData element)
		{
			return this._secretInformationMetaData.TryGetValue(objectId, out element);
		}

		// Token: 0x0600539B RID: 21403 RVA: 0x002D8BFC File Offset: 0x002D6DFC
		private unsafe void AddElement_SecretInformationMetaData(int objectId, SecretInformationMetaData instance)
		{
			instance.CollectionHelperData = this.HelperDataSecretInformationMetaData;
			instance.DataStatesOffset = this._dataStatesSecretInformationMetaData.Create();
			this._secretInformationMetaData.Add(objectId, instance);
			byte* pData = OperationAdder.DynamicObjectCollection_Add<int>(18, 2, objectId, instance.GetSerializedSize());
			instance.Serialize(pData);
		}

		// Token: 0x0600539C RID: 21404 RVA: 0x002D8C50 File Offset: 0x002D6E50
		private void RemoveElement_SecretInformationMetaData(int objectId)
		{
			SecretInformationMetaData instance;
			bool flag = !this._secretInformationMetaData.TryGetValue(objectId, out instance);
			if (!flag)
			{
				this._dataStatesSecretInformationMetaData.Remove(instance.DataStatesOffset);
				this._secretInformationMetaData.Remove(objectId);
				OperationAdder.DynamicObjectCollection_Remove<int>(18, 2, objectId);
			}
		}

		// Token: 0x0600539D RID: 21405 RVA: 0x002D8C9E File Offset: 0x002D6E9E
		private void ClearSecretInformationMetaData()
		{
			this._dataStatesSecretInformationMetaData.Clear();
			this._secretInformationMetaData.Clear();
			OperationAdder.DynamicObjectCollection_Clear(18, 2);
		}

		// Token: 0x0600539E RID: 21406 RVA: 0x002D8CC4 File Offset: 0x002D6EC4
		public int GetElementField_SecretInformationMetaData(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
		{
			SecretInformationMetaData instance;
			bool flag = !this._secretInformationMetaData.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				string tag = "GetElementField_SecretInformationMetaData";
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
					this._dataStatesSecretInformationMetaData.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
				switch (fieldId)
				{
				case 0:
					result = Serializer.Serialize(instance.GetId(), dataPool);
					break;
				case 1:
					result = Serializer.Serialize(instance.GetOffset(), dataPool);
					break;
				case 2:
					result = Serializer.Serialize(instance.GetDisseminationData(), dataPool);
					break;
				case 3:
					result = Serializer.Serialize(instance.GetRelevanceSecretInformationMetaDataId(), dataPool);
					break;
				case 4:
					result = Serializer.Serialize(instance.GetSecretInformationCharacterRelationshipSnapshotCollection(), dataPool);
					break;
				case 5:
					result = Serializer.Serialize(instance.GetSecretInformationCharacterExtraInfoCollection(), dataPool);
					break;
				default:
				{
					bool flag2 = fieldId >= 6;
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

		// Token: 0x0600539F RID: 21407 RVA: 0x002D8E4C File Offset: 0x002D704C
		public void SetElementField_SecretInformationMetaData(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			SecretInformationMetaData instance;
			bool flag = !this._secretInformationMetaData.TryGetValue(objectId, out instance);
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
				int value = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value);
				instance.SetOffset(value, context);
				break;
			}
			case 2:
			{
				SecretInformationDisseminationData value2 = instance.GetDisseminationData();
				Serializer.Deserialize(dataPool, valueOffset, ref value2);
				instance.SetDisseminationData(value2, context);
				break;
			}
			case 3:
			{
				int value3 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value3);
				instance.SetRelevanceSecretInformationMetaDataId(value3, context);
				break;
			}
			case 4:
			{
				SecretInformationCharacterRelationshipSnapshotCollection value4 = instance.GetSecretInformationCharacterRelationshipSnapshotCollection();
				Serializer.Deserialize(dataPool, valueOffset, ref value4);
				instance.SetSecretInformationCharacterRelationshipSnapshotCollection(value4, context);
				break;
			}
			case 5:
			{
				SecretInformationCharacterExtraInfoCollection value5 = instance.GetSecretInformationCharacterExtraInfoCollection();
				Serializer.Deserialize(dataPool, valueOffset, ref value5);
				instance.SetSecretInformationCharacterExtraInfoCollection(value5, context);
				break;
			}
			default:
			{
				bool flag2 = fieldId >= 6;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (flag2)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = fieldId >= 6;
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

		// Token: 0x060053A0 RID: 21408 RVA: 0x002D9054 File Offset: 0x002D7254
		private int CheckModified_SecretInformationMetaData(int objectId, ushort fieldId, RawDataPool dataPool)
		{
			SecretInformationMetaData instance;
			bool flag = !this._secretInformationMetaData.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = fieldId >= 6;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesSecretInformationMetaData.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					this._dataStatesSecretInformationMetaData.ResetModified(instance.DataStatesOffset, (int)fieldId);
					switch (fieldId)
					{
					case 0:
						result = Serializer.Serialize(instance.GetId(), dataPool);
						break;
					case 1:
						result = Serializer.Serialize(instance.GetOffset(), dataPool);
						break;
					case 2:
						result = Serializer.Serialize(instance.GetDisseminationData(), dataPool);
						break;
					case 3:
						result = Serializer.Serialize(instance.GetRelevanceSecretInformationMetaDataId(), dataPool);
						break;
					case 4:
						result = Serializer.Serialize(instance.GetSecretInformationCharacterRelationshipSnapshotCollection(), dataPool);
						break;
					case 5:
						result = Serializer.Serialize(instance.GetSecretInformationCharacterExtraInfoCollection(), dataPool);
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

		// Token: 0x060053A1 RID: 21409 RVA: 0x002D919C File Offset: 0x002D739C
		private void ResetModifiedWrapper_SecretInformationMetaData(int objectId, ushort fieldId)
		{
			SecretInformationMetaData instance;
			bool flag = !this._secretInformationMetaData.TryGetValue(objectId, out instance);
			if (!flag)
			{
				bool flag2 = fieldId >= 6;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesSecretInformationMetaData.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (!flag3)
				{
					this._dataStatesSecretInformationMetaData.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
			}
		}

		// Token: 0x060053A2 RID: 21410 RVA: 0x002D922C File Offset: 0x002D742C
		private bool IsModifiedWrapper_SecretInformationMetaData(int objectId, ushort fieldId)
		{
			SecretInformationMetaData instance;
			bool flag = !this._secretInformationMetaData.TryGetValue(objectId, out instance);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = fieldId >= 6;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = this._dataStatesSecretInformationMetaData.IsModified(instance.DataStatesOffset, (int)fieldId);
			}
			return result;
		}

		// Token: 0x060053A3 RID: 21411 RVA: 0x002D92A4 File Offset: 0x002D74A4
		private int GetNextMetaDataId()
		{
			return this._nextMetaDataId;
		}

		// Token: 0x060053A4 RID: 21412 RVA: 0x002D92BC File Offset: 0x002D74BC
		private unsafe void SetNextMetaDataId(int value, DataContext context)
		{
			this._nextMetaDataId = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, this.DataStates, InformationDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(18, 3, 4);
			*(int*)pData = this._nextMetaDataId;
			pData += 4;
		}

		// Token: 0x060053A5 RID: 21413 RVA: 0x002D92FC File Offset: 0x002D74FC
		public SecretInformationCharacterDataCollection GetElement_CharacterSecretInformation(int elementId)
		{
			return this._characterSecretInformation[elementId];
		}

		// Token: 0x060053A6 RID: 21414 RVA: 0x002D931C File Offset: 0x002D751C
		public bool TryGetElement_CharacterSecretInformation(int elementId, out SecretInformationCharacterDataCollection value)
		{
			return this._characterSecretInformation.TryGetValue(elementId, out value);
		}

		// Token: 0x060053A7 RID: 21415 RVA: 0x002D933C File Offset: 0x002D753C
		private unsafe void AddElement_CharacterSecretInformation(int elementId, SecretInformationCharacterDataCollection value, DataContext context)
		{
			this._characterSecretInformation.Add(elementId, value);
			this._modificationsCharacterSecretInformation.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, this.DataStates, InformationDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<int>(18, 4, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<int>(18, 4, elementId, 0);
			}
		}

		// Token: 0x060053A8 RID: 21416 RVA: 0x002D93AC File Offset: 0x002D75AC
		private unsafe void SetElement_CharacterSecretInformation(int elementId, SecretInformationCharacterDataCollection value, DataContext context)
		{
			this._characterSecretInformation[elementId] = value;
			this._modificationsCharacterSecretInformation.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, this.DataStates, InformationDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<int>(18, 4, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<int>(18, 4, elementId, 0);
			}
		}

		// Token: 0x060053A9 RID: 21417 RVA: 0x002D941B File Offset: 0x002D761B
		private void RemoveElement_CharacterSecretInformation(int elementId, DataContext context)
		{
			this._characterSecretInformation.Remove(elementId);
			this._modificationsCharacterSecretInformation.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, this.DataStates, InformationDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<int>(18, 4, elementId);
		}

		// Token: 0x060053AA RID: 21418 RVA: 0x002D9455 File Offset: 0x002D7655
		private void ClearCharacterSecretInformation(DataContext context)
		{
			this._characterSecretInformation.Clear();
			this._modificationsCharacterSecretInformation.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, this.DataStates, InformationDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(18, 4);
		}

		// Token: 0x060053AB RID: 21419 RVA: 0x002D948C File Offset: 0x002D768C
		public List<int> GetBroadcastSecretInformation()
		{
			return this._broadcastSecretInformation;
		}

		// Token: 0x060053AC RID: 21420 RVA: 0x002D94A4 File Offset: 0x002D76A4
		public unsafe void SetBroadcastSecretInformation(List<int> value, DataContext context)
		{
			this._broadcastSecretInformation = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, this.DataStates, InformationDomain.CacheInfluences, context);
			int elementsCount = this._broadcastSecretInformation.Count;
			int contentSize = 4 * elementsCount;
			int dataSize = 2 + contentSize;
			byte* pData = OperationAdder.DynamicSingleValue_Set(18, 5, dataSize);
			*(short*)pData = (short)((ushort)elementsCount);
			pData += 2;
			for (int i = 0; i < elementsCount; i++)
			{
				*(int*)(pData + (IntPtr)i * 4) = this._broadcastSecretInformation[i];
			}
			pData += contentSize;
		}

		// Token: 0x060053AD RID: 21421 RVA: 0x002D9524 File Offset: 0x002D7724
		public List<NormalInformation> GetTaiwuReceivedNormalInformationInMonth()
		{
			return this._taiwuReceivedNormalInformationInMonth;
		}

		// Token: 0x060053AE RID: 21422 RVA: 0x002D953C File Offset: 0x002D773C
		public unsafe void SetTaiwuReceivedNormalInformationInMonth(List<NormalInformation> value, DataContext context)
		{
			this._taiwuReceivedNormalInformationInMonth = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, this.DataStates, InformationDomain.CacheInfluences, context);
			int elementsCount = this._taiwuReceivedNormalInformationInMonth.Count;
			int contentSize = 3 * elementsCount;
			int dataSize = 2 + contentSize;
			byte* pData = OperationAdder.DynamicSingleValue_Set(18, 6, dataSize);
			*(short*)pData = (short)((ushort)elementsCount);
			pData += 2;
			for (int i = 0; i < elementsCount; i++)
			{
				pData += this._taiwuReceivedNormalInformationInMonth[i].Serialize(pData);
			}
		}

		// Token: 0x060053AF RID: 21423 RVA: 0x002D95BC File Offset: 0x002D77BC
		public List<int> GetTaiwuReceivedSecretInformationInMonth()
		{
			return this._taiwuReceivedSecretInformationInMonth;
		}

		// Token: 0x060053B0 RID: 21424 RVA: 0x002D95D4 File Offset: 0x002D77D4
		public unsafe void SetTaiwuReceivedSecretInformationInMonth(List<int> value, DataContext context)
		{
			this._taiwuReceivedSecretInformationInMonth = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, this.DataStates, InformationDomain.CacheInfluences, context);
			int elementsCount = this._taiwuReceivedSecretInformationInMonth.Count;
			int contentSize = 4 * elementsCount;
			int dataSize = 2 + contentSize;
			byte* pData = OperationAdder.DynamicSingleValue_Set(18, 7, dataSize);
			*(short*)pData = (short)((ushort)elementsCount);
			pData += 2;
			for (int i = 0; i < elementsCount; i++)
			{
				*(int*)(pData + (IntPtr)i * 4) = this._taiwuReceivedSecretInformationInMonth[i];
			}
			pData += contentSize;
		}

		// Token: 0x060053B1 RID: 21425 RVA: 0x002D9654 File Offset: 0x002D7854
		public List<int> GetTaiwuReceivedInformation()
		{
			return this._taiwuReceivedInformation;
		}

		// Token: 0x060053B2 RID: 21426 RVA: 0x002D966C File Offset: 0x002D786C
		public void SetTaiwuReceivedInformation(List<int> value, DataContext context)
		{
			this._taiwuReceivedInformation = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, this.DataStates, InformationDomain.CacheInfluences, context);
		}

		// Token: 0x060053B3 RID: 21427 RVA: 0x002D968C File Offset: 0x002D788C
		public List<NormalInformation> GetTaiwuTmpInformation()
		{
			return this._taiwuTmpInformation;
		}

		// Token: 0x060053B4 RID: 21428 RVA: 0x002D96A4 File Offset: 0x002D78A4
		public void SetTaiwuTmpInformation(List<NormalInformation> value, DataContext context)
		{
			this._taiwuTmpInformation = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(9, this.DataStates, InformationDomain.CacheInfluences, context);
		}

		// Token: 0x060053B5 RID: 21429 RVA: 0x002D96C2 File Offset: 0x002D78C2
		public override void OnInitializeGameDataModule()
		{
			this.InitializeOnInitializeGameDataModule();
		}

		// Token: 0x060053B6 RID: 21430 RVA: 0x002D96CC File Offset: 0x002D78CC
		public unsafe override void OnEnterNewWorld()
		{
			this.InitializeOnEnterNewWorld();
			this.InitializeInternalDataOfCollections();
			foreach (KeyValuePair<int, NormalInformationCollection> entry in this._information)
			{
				int elementId = entry.Key;
				NormalInformationCollection value = entry.Value;
				bool flag = value != null;
				if (flag)
				{
					int contentSize = value.GetSerializedSize();
					byte* pData = OperationAdder.DynamicSingleValueCollection_Add<int>(18, 0, elementId, contentSize);
					pData += value.Serialize(pData);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<int>(18, 0, elementId, 0);
				}
			}
			int dataSize = this._secretInformationCollection.GetSize();
			byte* pData2 = OperationAdder.Binary_Write(18, 1, 0, dataSize);
			this._secretInformationCollection.CopyTo(0, dataSize, pData2);
			ushort metadataSize = this._secretInformationCollection.GetSerializedFixedSizeOfMetadata();
			byte* pMetadata = OperationAdder.Binary_SetMetadata(18, 1, metadataSize);
			this._secretInformationCollection.SerializeMetadata(pMetadata);
			foreach (KeyValuePair<int, SecretInformationMetaData> entry2 in this._secretInformationMetaData)
			{
				int objectId = entry2.Key;
				SecretInformationMetaData instance = entry2.Value;
				byte* pData3 = OperationAdder.DynamicObjectCollection_Add<int>(18, 2, objectId, instance.GetSerializedSize());
				instance.Serialize(pData3);
			}
			byte* pData4 = OperationAdder.FixedSingleValue_Set(18, 3, 4);
			*(int*)pData4 = this._nextMetaDataId;
			pData4 += 4;
			foreach (KeyValuePair<int, SecretInformationCharacterDataCollection> entry3 in this._characterSecretInformation)
			{
				int elementId2 = entry3.Key;
				SecretInformationCharacterDataCollection value2 = entry3.Value;
				bool flag2 = value2 != null;
				if (flag2)
				{
					int contentSize2 = value2.GetSerializedSize();
					byte* pData5 = OperationAdder.DynamicSingleValueCollection_Add<int>(18, 4, elementId2, contentSize2);
					pData5 += value2.Serialize(pData5);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<int>(18, 4, elementId2, 0);
				}
			}
			int elementsCount = this._broadcastSecretInformation.Count;
			int contentSize3 = 4 * elementsCount;
			int dataSize2 = 2 + contentSize3;
			byte* pData6 = OperationAdder.DynamicSingleValue_Set(18, 5, dataSize2);
			*(short*)pData6 = (short)((ushort)elementsCount);
			pData6 += 2;
			for (int i = 0; i < elementsCount; i++)
			{
				*(int*)(pData6 + (IntPtr)i * 4) = this._broadcastSecretInformation[i];
			}
			pData6 += contentSize3;
			int elementsCount2 = this._taiwuReceivedNormalInformationInMonth.Count;
			int contentSize4 = 3 * elementsCount2;
			int dataSize3 = 2 + contentSize4;
			byte* pData7 = OperationAdder.DynamicSingleValue_Set(18, 6, dataSize3);
			*(short*)pData7 = (short)((ushort)elementsCount2);
			pData7 += 2;
			for (int j = 0; j < elementsCount2; j++)
			{
				pData7 += this._taiwuReceivedNormalInformationInMonth[j].Serialize(pData7);
			}
			int elementsCount3 = this._taiwuReceivedSecretInformationInMonth.Count;
			int contentSize5 = 4 * elementsCount3;
			int dataSize4 = 2 + contentSize5;
			byte* pData8 = OperationAdder.DynamicSingleValue_Set(18, 7, dataSize4);
			*(short*)pData8 = (short)((ushort)elementsCount3);
			pData8 += 2;
			for (int k = 0; k < elementsCount3; k++)
			{
				*(int*)(pData8 + (IntPtr)k * 4) = this._taiwuReceivedSecretInformationInMonth[k];
			}
			pData8 += contentSize5;
		}

		// Token: 0x060053B7 RID: 21431 RVA: 0x002D9A20 File Offset: 0x002D7C20
		public override void OnLoadWorld()
		{
			this._pendingLoadingOperationIds = new Queue<uint>();
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(18, 0));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.Binary_Get(18, 1));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicObjectCollection_GetAllObjects(18, 2));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(18, 3));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(18, 4));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValue_Get(18, 5));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValue_Get(18, 6));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValue_Get(18, 7));
		}

		// Token: 0x060053B8 RID: 21432 RVA: 0x002D9ADC File Offset: 0x002D7CDC
		public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
		{
			int result;
			switch (dataId)
			{
			case 0:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 0);
					this._modificationsInformation.Reset();
				}
				result = Serializer.SerializeModifications<int>(this._information, dataPool);
				break;
			case 1:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 1);
					this._modificationsSecretInformationCollection.Reset(this._secretInformationCollection.GetSize());
				}
				result = Serializer.SerializeModifications(this._secretInformationCollection, dataPool);
				break;
			case 2:
				result = this.GetElementField_SecretInformationMetaData((int)subId0, (ushort)subId1, dataPool, resetModified);
				break;
			case 3:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to get value of dataId: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 4:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 4);
					this._modificationsCharacterSecretInformation.Reset();
				}
				result = Serializer.SerializeModifications<int>(this._characterSecretInformation, dataPool);
				break;
			case 5:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 5);
				}
				result = Serializer.Serialize(this._broadcastSecretInformation, dataPool);
				break;
			case 6:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 6);
				}
				result = Serializer.Serialize(this._taiwuReceivedNormalInformationInMonth, dataPool);
				break;
			case 7:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 7);
				}
				result = Serializer.Serialize(this._taiwuReceivedSecretInformationInMonth, dataPool);
				break;
			case 8:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 8);
				}
				result = Serializer.Serialize(this._taiwuReceivedInformation, dataPool);
				break;
			case 9:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 9);
				}
				result = Serializer.Serialize(this._taiwuTmpInformation, dataPool);
				break;
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

		// Token: 0x060053B9 RID: 21433 RVA: 0x002D9CFC File Offset: 0x002D7EFC
		public override void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			switch (dataId)
			{
			case 0:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 1:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
				this.SetElementField_SecretInformationMetaData((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
				break;
			case 3:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 4:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 5:
				Serializer.Deserialize(dataPool, valueOffset, ref this._broadcastSecretInformation);
				this.SetBroadcastSecretInformation(this._broadcastSecretInformation, context);
				break;
			case 6:
				Serializer.Deserialize(dataPool, valueOffset, ref this._taiwuReceivedNormalInformationInMonth);
				this.SetTaiwuReceivedNormalInformationInMonth(this._taiwuReceivedNormalInformationInMonth, context);
				break;
			case 7:
				Serializer.Deserialize(dataPool, valueOffset, ref this._taiwuReceivedSecretInformationInMonth);
				this.SetTaiwuReceivedSecretInformationInMonth(this._taiwuReceivedSecretInformationInMonth, context);
				break;
			case 8:
				Serializer.Deserialize(dataPool, valueOffset, ref this._taiwuReceivedInformation);
				this.SetTaiwuReceivedInformation(this._taiwuReceivedInformation, context);
				break;
			case 9:
				Serializer.Deserialize(dataPool, valueOffset, ref this._taiwuTmpInformation);
				this.SetTaiwuTmpInformation(this._taiwuTmpInformation, context);
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

		// Token: 0x060053BA RID: 21434 RVA: 0x002D9EE8 File Offset: 0x002D80E8
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
				if (num != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int characterId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref characterId);
				NormalInformationCollection returnValue = this.GetCharacterNormalInformation(characterId);
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
				int characterId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref characterId2);
				NormalInformation information = default(NormalInformation);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref information);
				this.AddNormalInformationToCharacter(context, characterId2, information);
				result = -1;
				break;
			}
			case 2:
			{
				int argsCount3 = operation.ArgsCount;
				int num3 = argsCount3;
				if (num3 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.DeleteTmpInformation(context);
				result = -1;
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
				int characterId3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref characterId3);
				NormalInformation information2 = default(NormalInformation);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref information2);
				int returnValue2 = this.GetNormalInformationUsedCount(characterId3, information2);
				result = Serializer.Serialize(returnValue2, returnDataPool);
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
				List<int> secretInformationMetaDataIds = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref secretInformationMetaDataIds);
				SecretInformationDisplayPackage returnValue3 = this.GetSecretInformationDisplayPackage(secretInformationMetaDataIds);
				result = Serializer.Serialize(returnValue3, returnDataPool);
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
				int characterId4 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref characterId4);
				SecretInformationDisplayPackage returnValue4 = this.GetSecretInformationDisplayPackageFromCharacter(characterId4);
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
				int characterId5 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref characterId5);
				SecretInformationDisplayPackage returnValue5 = this.GetSecretInformationDisplayPackageFromBroadcast(characterId5);
				result = Serializer.Serialize(returnValue5, returnDataPool);
				break;
			}
			case 7:
			{
				int argsCount8 = operation.ArgsCount;
				int num8 = argsCount8;
				if (num8 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int characterId6 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref characterId6);
				SecretInformationDisplayPackage returnValue6 = this.GetSecretInformationDisplayPackageForSelections(characterId6);
				result = Serializer.Serialize(returnValue6, returnDataPool);
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
				int charId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId);
				int metaDataId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref metaDataId);
				this.DiscardSecretInformation(context, charId, metaDataId);
				result = -1;
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
				int metaDataId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref metaDataId2);
				SecretInformationDisplayData returnValue7 = this.GetSecretInformationDisplayData(metaDataId2);
				result = Serializer.Serialize(returnValue7, returnDataPool);
				break;
			}
			case 10:
			{
				int argsCount11 = operation.ArgsCount;
				int num11 = argsCount11;
				if (num11 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string templateDefKeyName = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref templateDefKeyName);
				List<int> charIds = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charIds);
				int returnValue8 = this.GmCmd_CreateSecretInformationByCharacterIds(context, templateDefKeyName, charIds);
				result = Serializer.Serialize(returnValue8, returnDataPool);
				break;
			}
			case 11:
			{
				int argsCount12 = operation.ArgsCount;
				int num12 = argsCount12;
				if (num12 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int characterId7 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref characterId7);
				int metaDataId3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref metaDataId3);
				bool returnValue9 = this.GmCmd_MakeCharacterReceiveSecretInformation(context, characterId7, metaDataId3);
				result = Serializer.Serialize(returnValue9, returnDataPool);
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
				int metaDataId4 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref metaDataId4);
				int sourceCharId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref sourceCharId);
				int targetCharId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref targetCharId);
				bool returnValue10 = this.DisseminateSecretInformation(context, metaDataId4, sourceCharId, targetCharId);
				result = Serializer.Serialize(returnValue10, returnDataPool);
				break;
			}
			case 13:
			{
				int argsCount14 = operation.ArgsCount;
				int num14 = argsCount14;
				if (num14 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<int> charList = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charList);
				List<CharacterDisplayDataWithInfo> returnValue11 = this.GetCharacterDisplayDataWithInfoList(charList);
				result = Serializer.Serialize(returnValue11, returnDataPool);
				break;
			}
			case 14:
			{
				int argsCount15 = operation.ArgsCount;
				int num15 = argsCount15;
				if (num15 != 1)
				{
					if (num15 != 2)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					int metaDataId5 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref metaDataId5);
					int sourceCharId2 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref sourceCharId2);
					this.GmCmd_MakeSecretInformationBroadcast(context, metaDataId5, sourceCharId2);
					result = -1;
				}
				else
				{
					int metaDataId6 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref metaDataId6);
					this.GmCmd_MakeSecretInformationBroadcast(context, metaDataId6, -1);
					result = -1;
				}
				break;
			}
			case 15:
			{
				int argsCount16 = operation.ArgsCount;
				int num16 = argsCount16;
				if (num16 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				NormalInformation normalInformation = default(NormalInformation);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref normalInformation);
				this.PerformProfessionLiteratiSkill3(context, normalInformation);
				result = -1;
				break;
			}
			case 16:
			{
				int argsCount17 = operation.ArgsCount;
				int num17 = argsCount17;
				if (num17 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int secretInformationId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref secretInformationId);
				this.PerformProfessionLiteratiSkill2(context, secretInformationId);
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
				int characterId8 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref characterId8);
				NormalInformation information3 = default(NormalInformation);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref information3);
				IntPair returnValue12 = this.GetNormalInformationUsedCountAndMax(characterId8, information3);
				result = Serializer.Serialize(returnValue12, returnDataPool);
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
				List<IntPair> secretList = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref secretList);
				int shopCharId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref shopCharId);
				this.SettleSecretInformationShopTrade(context, secretList, shopCharId);
				result = -1;
				break;
			}
			case 19:
			{
				int argsCount20 = operation.ArgsCount;
				int num20 = argsCount20;
				if (num20 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int secretId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref secretId);
				int sourceCharId3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref sourceCharId3);
				int amount = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref amount);
				int returnValue13 = this.GmCmd_DisseminationSecretInformationToRandomCharacters(context, secretId, sourceCharId3, amount);
				result = Serializer.Serialize(returnValue13, returnDataPool);
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

		// Token: 0x060053BB RID: 21435 RVA: 0x002DA914 File Offset: 0x002D8B14
		public override void OnMonitorData(ushort dataId, ulong subId0, uint subId1, bool monitoring)
		{
			switch (dataId)
			{
			case 0:
				this._modificationsInformation.ChangeRecording(monitoring);
				break;
			case 1:
				this._modificationsSecretInformationCollection.ChangeRecording(monitoring, this._secretInformationCollection.GetSize());
				break;
			case 2:
				break;
			case 3:
				break;
			case 4:
				this._modificationsCharacterSecretInformation.ChangeRecording(monitoring);
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
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x060053BC RID: 21436 RVA: 0x002DA9CC File Offset: 0x002D8BCC
		public override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
		{
			int result;
			switch (dataId)
			{
			case 0:
			{
				bool flag = !BaseGameDataDomain.IsModified(this.DataStates, 0);
				if (flag)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 0);
					int offset = Serializer.SerializeModifications<int>(this._information, dataPool, this._modificationsInformation);
					this._modificationsInformation.Reset();
					result = offset;
				}
				break;
			}
			case 1:
			{
				bool flag2 = !BaseGameDataDomain.IsModified(this.DataStates, 1);
				if (flag2)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 1);
					int offset2 = Serializer.SerializeModifications(this._secretInformationCollection, dataPool, this._modificationsSecretInformationCollection);
					this._modificationsSecretInformationCollection.Reset(this._secretInformationCollection.GetSize());
					result = offset2;
				}
				break;
			}
			case 2:
				result = this.CheckModified_SecretInformationMetaData((int)subId0, (ushort)subId1, dataPool);
				break;
			case 3:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 4:
			{
				bool flag3 = !BaseGameDataDomain.IsModified(this.DataStates, 4);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 4);
					int offset3 = Serializer.SerializeModifications<int>(this._characterSecretInformation, dataPool, this._modificationsCharacterSecretInformation);
					this._modificationsCharacterSecretInformation.Reset();
					result = offset3;
				}
				break;
			}
			case 5:
			{
				bool flag4 = !BaseGameDataDomain.IsModified(this.DataStates, 5);
				if (flag4)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 5);
					result = Serializer.Serialize(this._broadcastSecretInformation, dataPool);
				}
				break;
			}
			case 6:
			{
				bool flag5 = !BaseGameDataDomain.IsModified(this.DataStates, 6);
				if (flag5)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 6);
					result = Serializer.Serialize(this._taiwuReceivedNormalInformationInMonth, dataPool);
				}
				break;
			}
			case 7:
			{
				bool flag6 = !BaseGameDataDomain.IsModified(this.DataStates, 7);
				if (flag6)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 7);
					result = Serializer.Serialize(this._taiwuReceivedSecretInformationInMonth, dataPool);
				}
				break;
			}
			case 8:
			{
				bool flag7 = !BaseGameDataDomain.IsModified(this.DataStates, 8);
				if (flag7)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 8);
					result = Serializer.Serialize(this._taiwuReceivedInformation, dataPool);
				}
				break;
			}
			case 9:
			{
				bool flag8 = !BaseGameDataDomain.IsModified(this.DataStates, 9);
				if (flag8)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 9);
					result = Serializer.Serialize(this._taiwuTmpInformation, dataPool);
				}
				break;
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

		// Token: 0x060053BD RID: 21437 RVA: 0x002DACBC File Offset: 0x002D8EBC
		public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			switch (dataId)
			{
			case 0:
			{
				bool flag = !BaseGameDataDomain.IsModified(this.DataStates, 0);
				if (!flag)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 0);
					this._modificationsInformation.Reset();
				}
				break;
			}
			case 1:
			{
				bool flag2 = !BaseGameDataDomain.IsModified(this.DataStates, 1);
				if (!flag2)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 1);
				}
				break;
			}
			case 2:
				this.ResetModifiedWrapper_SecretInformationMetaData((int)subId0, (ushort)subId1);
				break;
			case 3:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 4:
			{
				bool flag3 = !BaseGameDataDomain.IsModified(this.DataStates, 4);
				if (!flag3)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 4);
					this._modificationsCharacterSecretInformation.Reset();
				}
				break;
			}
			case 5:
			{
				bool flag4 = !BaseGameDataDomain.IsModified(this.DataStates, 5);
				if (!flag4)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 5);
				}
				break;
			}
			case 6:
			{
				bool flag5 = !BaseGameDataDomain.IsModified(this.DataStates, 6);
				if (!flag5)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 6);
				}
				break;
			}
			case 7:
			{
				bool flag6 = !BaseGameDataDomain.IsModified(this.DataStates, 7);
				if (!flag6)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 7);
				}
				break;
			}
			case 8:
			{
				bool flag7 = !BaseGameDataDomain.IsModified(this.DataStates, 8);
				if (!flag7)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 8);
				}
				break;
			}
			case 9:
			{
				bool flag8 = !BaseGameDataDomain.IsModified(this.DataStates, 9);
				if (!flag8)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 9);
				}
				break;
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

		// Token: 0x060053BE RID: 21438 RVA: 0x002DAEDC File Offset: 0x002D90DC
		public override bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			bool result;
			switch (dataId)
			{
			case 0:
				result = BaseGameDataDomain.IsModified(this.DataStates, 0);
				break;
			case 1:
				result = BaseGameDataDomain.IsModified(this.DataStates, 1);
				break;
			case 2:
				result = this.IsModifiedWrapper_SecretInformationMetaData((int)subId0, (ushort)subId1);
				break;
			case 3:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to verify modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 4:
				result = BaseGameDataDomain.IsModified(this.DataStates, 4);
				break;
			case 5:
				result = BaseGameDataDomain.IsModified(this.DataStates, 5);
				break;
			case 6:
				result = BaseGameDataDomain.IsModified(this.DataStates, 6);
				break;
			case 7:
				result = BaseGameDataDomain.IsModified(this.DataStates, 7);
				break;
			case 8:
				result = BaseGameDataDomain.IsModified(this.DataStates, 8);
				break;
			case 9:
				result = BaseGameDataDomain.IsModified(this.DataStates, 9);
				break;
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

		// Token: 0x060053BF RID: 21439 RVA: 0x002DB00C File Offset: 0x002D920C
		public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			switch (influence.TargetIndicator.DataId)
			{
			case 0:
				break;
			case 1:
				break;
			case 2:
			{
				bool flag = !unconditionallyInfluenceAll;
				if (flag)
				{
					List<BaseGameDataObject> influencedObjects = InfluenceChecker.InfluencedObjectsPool.Get();
					bool influenceAll = InfluenceChecker.GetScope<int, SecretInformationMetaData>(context, sourceObject, influence.Scope, this._secretInformationMetaData, influencedObjects);
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
						BaseGameDataDomain.InvalidateAllAndInfluencedCaches(InformationDomain.CacheInfluencesSecretInformationMetaData, this._dataStatesSecretInformationMetaData, influence, context);
					}
					influencedObjects.Clear();
					InfluenceChecker.InfluencedObjectsPool.Return(influencedObjects);
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(InformationDomain.CacheInfluencesSecretInformationMetaData, this._dataStatesSecretInformationMetaData, influence, context);
				}
				return;
			}
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

		// Token: 0x060053C0 RID: 21440 RVA: 0x002DB1D0 File Offset: 0x002D93D0
		public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			switch (operation.DataId)
			{
			case 0:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<int, NormalInformationCollection>(operation, pResult, this._information);
				break;
			case 1:
				ResponseProcessor.ProcessBinary(operation, pResult, this._secretInformationCollection);
				break;
			case 2:
				ResponseProcessor.ProcessDynamicObjectCollection<int, SecretInformationMetaData>(operation, pResult, this._secretInformationMetaData);
				break;
			case 3:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single<int>(operation, pResult, ref this._nextMetaDataId);
				break;
			case 4:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<int, SecretInformationCharacterDataCollection>(operation, pResult, this._characterSecretInformation);
				break;
			case 5:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_List<int>(operation, pResult, this._broadcastSecretInformation);
				break;
			case 6:
				ResponseProcessor.ProcessSingleValue_CustomType_Fixed_Value_List<NormalInformation>(operation, pResult, this._taiwuReceivedNormalInformationInMonth, 3);
				break;
			case 7:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_List<int>(operation, pResult, this._taiwuReceivedSecretInformationInMonth);
				break;
			case 8:
				goto IL_17D;
			case 9:
				goto IL_17D;
			default:
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.DataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
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
						DomainManager.Global.CompleteLoading(18);
					}
				}
			}
			return;
			IL_17D:
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Cannot process archive response of non-archive data ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.DataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060053C1 RID: 21441 RVA: 0x002DB390 File Offset: 0x002D9590
		private void InitializeInternalDataOfCollections()
		{
			foreach (KeyValuePair<int, SecretInformationMetaData> entry in this._secretInformationMetaData)
			{
				SecretInformationMetaData instance = entry.Value;
				instance.CollectionHelperData = this.HelperDataSecretInformationMetaData;
				instance.DataStatesOffset = this._dataStatesSecretInformationMetaData.Create();
			}
		}

		// Token: 0x060053C2 RID: 21442 RVA: 0x002DB408 File Offset: 0x002D9608
		public static bool CheckTuringTest(GameData.Domains.Character.Character character)
		{
			bool flag = character == null;
			return !flag && character.GetCreatingType() == 1 && character.GetAgeGroup() > 0;
		}

		// Token: 0x060053C3 RID: 21443 RVA: 0x002DB43C File Offset: 0x002D963C
		public static bool CheckTuringTest(int charId, out GameData.Domains.Character.Character character)
		{
			return DomainManager.Character.TryGetElement_Objects(charId, out character) && InformationDomain.CheckTuringTest(character);
		}

		// Token: 0x060053C4 RID: 21444 RVA: 0x002DB468 File Offset: 0x002D9668
		public static List<GameData.Domains.Character.Character> GetTuringTestPassedCharacters(IEnumerable collection)
		{
			List<GameData.Domains.Character.Character> result = new List<GameData.Domains.Character.Character>();
			foreach (object obj in collection)
			{
				int charId = (int)obj;
				GameData.Domains.Character.Character character;
				bool flag = DomainManager.Character.TryGetElement_Objects(charId, out character) && InformationDomain.CheckTuringTest(character);
				if (flag)
				{
					result.Add(character);
				}
			}
			return result;
		}

		// Token: 0x060053C6 RID: 21446 RVA: 0x002DB56C File Offset: 0x002D976C
		[CompilerGenerated]
		private void <MakeSettlementsInformation>g__ProcessCharacter|40_0(int characterId, MapBlockItem blockConfig, ref InformationDomain.<>c__DisplayClass40_0 A_3)
		{
			bool flag = blockConfig.InformationTemplateId < 0;
			if (!flag)
			{
				NormalInformationCollection collection = this.EnsureCharacterNormalInformationCollection(A_3.context, characterId);
				IList<NormalInformation> list = collection.GetList();
				InformationItem configData = Information.Instance[blockConfig.InformationTemplateId];
				int i = 0;
				int len = list.Count;
				while (i < len)
				{
					NormalInformation current = list[i];
					bool flag2 = current.TemplateId == configData.TemplateId;
					if (flag2)
					{
						bool flag3 = current.Level < 8 && A_3.context.Random.Next(0, 100) < (int)configData.ExtraGainRate[(int)current.Level];
						if (flag3)
						{
							this.AddNormalInformationToCharacter(A_3.context, characterId, new NormalInformation(current.TemplateId, current.Level));
						}
						break;
					}
					i++;
				}
				int j = 0;
				int len2 = configData.BaseGainRate.Length;
				while (j < len2)
				{
					int gainRate = (int)(configData.BaseGainRate[j] * 2);
					bool flag4 = A_3.context.Random.Next(0, 100) < gainRate;
					if (flag4)
					{
						this.AddNormalInformationToCharacter(A_3.context, characterId, new NormalInformation(configData.TemplateId, (sbyte)j));
						break;
					}
					j++;
				}
			}
		}

		// Token: 0x060053C7 RID: 21447 RVA: 0x002DB6C0 File Offset: 0x002D98C0
		[CompilerGenerated]
		private void <MakeSettlementsInformation>g__ProcessCharacterSet|40_1(HashSet<int> characterSet, MapBlockItem blockConfig, ref InformationDomain.<>c__DisplayClass40_0 A_3)
		{
			bool flag = characterSet == null || blockConfig.InformationTemplateId < 0;
			if (!flag)
			{
				foreach (int characterId in characterSet)
				{
					this.<MakeSettlementsInformation>g__ProcessCharacter|40_0(characterId, blockConfig, ref A_3);
				}
			}
		}

		// Token: 0x060053C8 RID: 21448 RVA: 0x002DB72C File Offset: 0x002D992C
		[CompilerGenerated]
		private void <MakeSettlementsInformation>g__ProcessMapBlock|40_2(MapBlockData block, ref InformationDomain.<>c__DisplayClass40_0 A_2)
		{
			MapBlockItem blockConfig = block.GetConfig();
			bool flag = blockConfig.InformationTemplateId < 0;
			if (!flag)
			{
				this.<MakeSettlementsInformation>g__ProcessCharacterSet|40_1(block.CharacterSet, blockConfig, ref A_2);
				bool flag2 = block.BlockId == A_2.taiwuLocation.BlockId && block.AreaId == A_2.taiwuLocation.AreaId;
				if (flag2)
				{
					this.<MakeSettlementsInformation>g__ProcessCharacterSet|40_1(A_2.taiwuGroups, blockConfig, ref A_2);
				}
				bool flag3 = block.GroupBlockList != null;
				if (flag3)
				{
					foreach (MapBlockData groupBlock in block.GroupBlockList)
					{
						this.<MakeSettlementsInformation>g__ProcessCharacterSet|40_1(groupBlock.CharacterSet, blockConfig, ref A_2);
						bool flag4 = groupBlock.BlockId == A_2.taiwuLocation.BlockId && groupBlock.AreaId == A_2.taiwuLocation.AreaId;
						if (flag4)
						{
							this.<MakeSettlementsInformation>g__ProcessCharacterSet|40_1(A_2.taiwuGroups, blockConfig, ref A_2);
						}
					}
				}
			}
		}

		// Token: 0x060053C9 RID: 21449 RVA: 0x002DB848 File Offset: 0x002D9A48
		[CompilerGenerated]
		internal static short[] <CalcSecretInformationDisplaySize>g__CharGradeToValue|76_0(bool isSect, ref InformationDomain.<>c__DisplayClass76_0 A_1)
		{
			short[] result;
			switch (A_1.characterIndex)
			{
			case 0:
				result = (isSect ? GlobalConfig.SecretInformationDisplay_PosASectCharGradeToValue : GlobalConfig.SecretInformationDisplay_PosANotSectCharGradeToValue);
				break;
			case 1:
				result = (isSect ? GlobalConfig.SecretInformationDisplay_PosBSectCharGradeToValue : GlobalConfig.SecretInformationDisplay_PosBNotSectCharGradeToValue);
				break;
			case 2:
				result = (isSect ? GlobalConfig.SecretInformationDisplay_PosCSectCharGradeToValue : GlobalConfig.SecretInformationDisplay_PosCNotSectCharGradeToValue);
				break;
			default:
				throw new IndexOutOfRangeException();
			}
			return result;
		}

		// Token: 0x060053CA RID: 21450 RVA: 0x002DB8B0 File Offset: 0x002D9AB0
		[CompilerGenerated]
		internal static short[] <CalcSecretInformationDisplaySize>g__CharRelationTypeToValue|76_1(bool isSect, ref InformationDomain.<>c__DisplayClass76_0 A_1)
		{
			short[] result;
			switch (A_1.characterIndex)
			{
			case 0:
				result = (isSect ? GlobalConfig.SecretInformationDisplay_PosASectCharRelationTypeToValue : GlobalConfig.SecretInformationDisplay_PosANotSectCharRelationTypeToValue);
				break;
			case 1:
				result = (isSect ? GlobalConfig.SecretInformationDisplay_PosBSectCharRelationTypeToValue : GlobalConfig.SecretInformationDisplay_PosBNotSectCharRelationTypeToValue);
				break;
			case 2:
				result = (isSect ? GlobalConfig.SecretInformationDisplay_PosCSectCharRelationTypeToValue : GlobalConfig.SecretInformationDisplay_PosCNotSectCharRelationTypeToValue);
				break;
			default:
				throw new IndexOutOfRangeException();
			}
			return result;
		}

		// Token: 0x060053CB RID: 21451 RVA: 0x002DB918 File Offset: 0x002D9B18
		[CompilerGenerated]
		internal static short[] <CalcSecretInformationDisplaySize>g__CharGradeToValue|81_0(bool isSect, ref InformationDomain.<>c__DisplayClass81_0 A_1)
		{
			short[] result;
			switch (A_1.characterIndex)
			{
			case 0:
				result = (isSect ? GlobalConfig.SecretInformationDisplay_PosASectCharGradeToValue : GlobalConfig.SecretInformationDisplay_PosANotSectCharGradeToValue);
				break;
			case 1:
				result = (isSect ? GlobalConfig.SecretInformationDisplay_PosBSectCharGradeToValue : GlobalConfig.SecretInformationDisplay_PosBNotSectCharGradeToValue);
				break;
			case 2:
				result = (isSect ? GlobalConfig.SecretInformationDisplay_PosCSectCharGradeToValue : GlobalConfig.SecretInformationDisplay_PosCNotSectCharGradeToValue);
				break;
			default:
				throw new IndexOutOfRangeException();
			}
			return result;
		}

		// Token: 0x060053CC RID: 21452 RVA: 0x002DB980 File Offset: 0x002D9B80
		[CompilerGenerated]
		internal static short[] <CalcSecretInformationDisplaySize>g__CharRelationTypeToValue|81_1(bool isSect, ref InformationDomain.<>c__DisplayClass81_0 A_1)
		{
			short[] result;
			switch (A_1.characterIndex)
			{
			case 0:
				result = (isSect ? GlobalConfig.SecretInformationDisplay_PosASectCharRelationTypeToValue : GlobalConfig.SecretInformationDisplay_PosANotSectCharRelationTypeToValue);
				break;
			case 1:
				result = (isSect ? GlobalConfig.SecretInformationDisplay_PosBSectCharRelationTypeToValue : GlobalConfig.SecretInformationDisplay_PosBNotSectCharRelationTypeToValue);
				break;
			case 2:
				result = (isSect ? GlobalConfig.SecretInformationDisplay_PosCSectCharRelationTypeToValue : GlobalConfig.SecretInformationDisplay_PosCNotSectCharRelationTypeToValue);
				break;
			default:
				throw new IndexOutOfRangeException();
			}
			return result;
		}

		// Token: 0x060053CD RID: 21453 RVA: 0x002DB9E8 File Offset: 0x002D9BE8
		[CompilerGenerated]
		private int <PlanDisseminateSecretInformation>g__GetRateByRelation|110_0(int targetCharId, ref InformationDomain.<>c__DisplayClass110_0 A_2, ref InformationDomain.<>c__DisplayClass110_1 A_3, ref InformationDomain.<>c__DisplayClass110_2 A_4)
		{
			int rate = 0;
			int actorId;
			int reactorId;
			bool flag = A_3.relatives.TryGetValue(A_4.effect.ActorIndex, out actorId) && A_3.relatives.TryGetValue(A_4.effect.ReactorIndex, out reactorId);
			int result;
			if (flag)
			{
				bool flag2 = !A_4.isSelfIntroduction;
				if (flag2)
				{
					A_4.r.Clear();
					this.CheckSecretInformationRelationship(A_2.charId, -1, targetCharId, -1, A_4.r);
					bool flag3 = A_4.r.Contains(SecretInformationRelationshipType.Relative) || A_4.r.Contains(SecretInformationRelationshipType.Friend);
					if (flag3)
					{
						rate += (int)A_4.dissemination.TfRateItsFri;
					}
					else
					{
						bool flag4 = A_4.r.Contains(SecretInformationRelationshipType.Enemy);
						if (flag4)
						{
							rate += (int)A_4.dissemination.TfRateItsEnm;
						}
					}
				}
				bool flag5 = rate == 0;
				if (flag5)
				{
					A_4.r.Clear();
					this.CheckSecretInformationRelationship(actorId, -1, targetCharId, -1, A_4.r);
					bool flag6 = A_4.r.Contains(SecretInformationRelationshipType.Relative) || A_4.r.Contains(SecretInformationRelationshipType.Friend);
					if (flag6)
					{
						rate += (int)(A_4.isSelfIntroduction ? A_4.dissemination.SfRateActFri : A_4.dissemination.TfRateActFri);
					}
					else
					{
						bool flag7 = A_4.r.Contains(SecretInformationRelationshipType.Enemy);
						if (flag7)
						{
							rate += (int)(A_4.isSelfIntroduction ? A_4.dissemination.SfRateActEnm : A_4.dissemination.TfRateActEnm);
						}
						else
						{
							A_4.r.Clear();
							this.CheckSecretInformationRelationship(reactorId, -1, targetCharId, -1, A_4.r);
							bool flag8 = A_4.r.Contains(SecretInformationRelationshipType.Relative) || A_4.r.Contains(SecretInformationRelationshipType.Friend);
							if (flag8)
							{
								rate += (int)(A_4.isSelfIntroduction ? A_4.dissemination.SfRateUnaFri : A_4.dissemination.TfRateUnaFri);
							}
							else
							{
								bool flag9 = A_4.r.Contains(SecretInformationRelationshipType.Enemy);
								if (flag9)
								{
									rate += (int)(A_4.isSelfIntroduction ? A_4.dissemination.SfRateUnaEnm : A_4.dissemination.TfRateUnaEnm);
								}
								else
								{
									RelatedCharacter relatedCharacter;
									bool flag10 = DomainManager.Character.TryGetRelation(A_2.charId, targetCharId, out relatedCharacter);
									if (flag10)
									{
										rate += (int)(A_4.isSelfIntroduction ? A_4.dissemination.SfRateStr : A_4.dissemination.TfRateStr);
									}
									else
									{
										rate += (int)(A_4.isSelfIntroduction ? A_4.dissemination.SfRateNStr : A_4.dissemination.TfRateNStr);
									}
								}
							}
						}
					}
				}
				result = rate;
			}
			else
			{
				result = -10000;
			}
			return result;
		}

		// Token: 0x04001640 RID: 5696
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, true)]
		private readonly Dictionary<int, NormalInformationCollection> _information;

		// Token: 0x04001641 RID: 5697
		[DomainData(DomainDataType.Binary, true, false, true, false)]
		private SecretInformationCollection _secretInformationCollection;

		// Token: 0x04001642 RID: 5698
		[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
		private readonly Dictionary<int, SecretInformationMetaData> _secretInformationMetaData;

		// Token: 0x04001643 RID: 5699
		[DomainData(DomainDataType.SingleValue, true, false, false, false)]
		private int _nextMetaDataId;

		// Token: 0x04001644 RID: 5700
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, true)]
		private readonly Dictionary<int, SecretInformationCharacterDataCollection> _characterSecretInformation;

		// Token: 0x04001645 RID: 5701
		[DomainData(DomainDataType.SingleValue, true, false, true, true)]
		private List<int> _broadcastSecretInformation;

		// Token: 0x04001646 RID: 5702
		[DomainData(DomainDataType.SingleValue, true, false, true, true)]
		private List<NormalInformation> _taiwuReceivedNormalInformationInMonth;

		// Token: 0x04001647 RID: 5703
		[DomainData(DomainDataType.SingleValue, true, false, true, true)]
		private List<int> _taiwuReceivedSecretInformationInMonth;

		// Token: 0x04001648 RID: 5704
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private List<int> _taiwuReceivedInformation;

		// Token: 0x04001649 RID: 5705
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private List<NormalInformation> _taiwuTmpInformation;

		// Token: 0x0400164A RID: 5706
		private static readonly RawDataBlock _bufferForRelationSnapshot = new RawDataBlock();

		// Token: 0x0400164B RID: 5707
		public static bool _isOfflineUpdate;

		// Token: 0x0400164C RID: 5708
		private static readonly HashSet<int> _offlineIndicesCharacterData = new HashSet<int>();

		// Token: 0x0400164D RID: 5709
		private static readonly HashSet<int> _offlineIndicesMetaDataOffset = new HashSet<int>();

		// Token: 0x0400164E RID: 5710
		private static readonly HashSet<int> _offlineIndicesMetaDataCharacterDisseminationData = new HashSet<int>();

		// Token: 0x0400164F RID: 5711
		private static List<InformationDomain.SecretInformationStartEnemyRelationItem> _StartEnemyRelationItem = new List<InformationDomain.SecretInformationStartEnemyRelationItem>();

		// Token: 0x04001650 RID: 5712
		private readonly List<CharacterDisplayDataWithInfo> _characterDisplayDataWithInfoList = new List<CharacterDisplayDataWithInfo>(128);

		// Token: 0x04001651 RID: 5713
		private EventArgBox _eventArgBox = new EventArgBox();

		// Token: 0x04001652 RID: 5714
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04001653 RID: 5715
		public readonly LocalObjectPool<SecretInformationProcessor> SecretInformationProcessorPool = new LocalObjectPool<SecretInformationProcessor>(2, 16);

		// Token: 0x04001654 RID: 5716
		public static readonly int[] FameTypeForDiscoveryRates = new int[]
		{
			-100,
			-75,
			-25,
			0,
			25,
			75,
			100
		};

		// Token: 0x04001655 RID: 5717
		private Stopwatch _swSecretInformation;

		// Token: 0x04001656 RID: 5718
		private readonly SortedList<int, int> _secretInformationRemovingList = new SortedList<int, int>();

		// Token: 0x04001657 RID: 5719
		private int _npcPlanCastCount = 0;

		// Token: 0x04001658 RID: 5720
		private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[10][];

		// Token: 0x04001659 RID: 5721
		private SingleValueCollectionModificationCollection<int> _modificationsInformation = SingleValueCollectionModificationCollection<int>.Create();

		// Token: 0x0400165A RID: 5722
		private BinaryModificationCollection _modificationsSecretInformationCollection = BinaryModificationCollection.Create();

		// Token: 0x0400165B RID: 5723
		private static readonly DataInfluence[][] CacheInfluencesSecretInformationMetaData = new DataInfluence[6][];

		// Token: 0x0400165C RID: 5724
		private readonly ObjectCollectionDataStates _dataStatesSecretInformationMetaData = new ObjectCollectionDataStates(6, 0);

		// Token: 0x0400165D RID: 5725
		public readonly ObjectCollectionHelperData HelperDataSecretInformationMetaData;

		// Token: 0x0400165E RID: 5726
		private SingleValueCollectionModificationCollection<int> _modificationsCharacterSecretInformation = SingleValueCollectionModificationCollection<int>.Create();

		// Token: 0x0400165F RID: 5727
		private Queue<uint> _pendingLoadingOperationIds;

		// Token: 0x02000AD0 RID: 2768
		public static class NormalInformationUseResultType
		{
			// Token: 0x04002CE6 RID: 11494
			public const sbyte Effective = 0;

			// Token: 0x04002CE7 RID: 11495
			public const sbyte Normal = 1;

			// Token: 0x04002CE8 RID: 11496
			public const sbyte Ineffective = 2;

			// Token: 0x04002CE9 RID: 11497
			public const sbyte SwordTomb = 3;
		}

		// Token: 0x02000AD1 RID: 2769
		public struct SecretInformationFavorChangeItem
		{
			// Token: 0x06008947 RID: 35143 RVA: 0x004EE946 File Offset: 0x004ECB46
			public SecretInformationFavorChangeItem(int characterId, int targetId, int deltaFavor, sbyte priority = 0)
			{
				this.CharacterId = characterId;
				this.TargetId = targetId;
				this.DeltaFavor = deltaFavor;
				this.Priority = priority;
			}

			// Token: 0x04002CEA RID: 11498
			public int CharacterId;

			// Token: 0x04002CEB RID: 11499
			public int TargetId;

			// Token: 0x04002CEC RID: 11500
			public int DeltaFavor;

			// Token: 0x04002CED RID: 11501
			public sbyte Priority;
		}

		// Token: 0x02000AD2 RID: 2770
		public struct SecretInformationHappinessChangeItem
		{
			// Token: 0x06008948 RID: 35144 RVA: 0x004EE966 File Offset: 0x004ECB66
			public SecretInformationHappinessChangeItem(int characterId, int deltaHappiness, sbyte priority = 0)
			{
				this.CharacterId = characterId;
				this.DeltaHappiness = deltaHappiness;
				this.Priority = priority;
			}

			// Token: 0x04002CEE RID: 11502
			public int CharacterId;

			// Token: 0x04002CEF RID: 11503
			public int DeltaHappiness;

			// Token: 0x04002CF0 RID: 11504
			public sbyte Priority;
		}

		// Token: 0x02000AD3 RID: 2771
		public struct SecretInformationStartEnemyRelationItem
		{
			// Token: 0x06008949 RID: 35145 RVA: 0x004EE97E File Offset: 0x004ECB7E
			public SecretInformationStartEnemyRelationItem(short secretInformationTemplateId, int characterId, int targetId, byte odds)
			{
				this.SecretInformationTemplateId = secretInformationTemplateId;
				this.CharacterId = characterId;
				this.TargetId = targetId;
				this.Odds = odds;
			}

			// Token: 0x04002CF1 RID: 11505
			public short SecretInformationTemplateId;

			// Token: 0x04002CF2 RID: 11506
			public int CharacterId;

			// Token: 0x04002CF3 RID: 11507
			public int TargetId;

			// Token: 0x04002CF4 RID: 11508
			public byte Odds;
		}
	}
}
