using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.ArchiveData;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Dependencies;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Global;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.PestleEffect;
using GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect;
using GameData.Domains.SpecialEffect.EquipmentEffect;
using GameData.Domains.SpecialEffect.Misc;
using GameData.Domains.Taiwu;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect
{
	// Token: 0x020000E2 RID: 226
	[GameDataDomain(17)]
	public class SpecialEffectDomain : BaseGameDataDomain
	{
		// Token: 0x060028E3 RID: 10467 RVA: 0x001F0AC4 File Offset: 0x001EECC4
		private void OnInitializedDomainData()
		{
		}

		// Token: 0x060028E4 RID: 10468 RVA: 0x001F0AC7 File Offset: 0x001EECC7
		private void InitializeOnInitializeGameDataModule()
		{
		}

		// Token: 0x060028E5 RID: 10469 RVA: 0x001F0ACA File Offset: 0x001EECCA
		private void InitializeOnEnterNewWorld()
		{
			Events.RegisterHandler_AddWug(new Events.OnAddWug(this.OnAddWug));
			SpecialEffectDomain.InvokeResetHandlers();
		}

		// Token: 0x060028E6 RID: 10470 RVA: 0x001F0AE5 File Offset: 0x001EECE5
		private void OnLoadedArchiveData()
		{
			Events.RegisterHandler_AddWug(new Events.OnAddWug(this.OnAddWug));
			SpecialEffectDomain.InvokeResetHandlers();
		}

		// Token: 0x060028E7 RID: 10471 RVA: 0x001F0B00 File Offset: 0x001EED00
		public override void OnCurrWorldArchiveDataReady(DataContext context, bool isNewWorld)
		{
			bool flag = !isNewWorld;
			if (flag)
			{
				this.OnLoadedAllArchiveData(context);
			}
		}

		// Token: 0x060028E8 RID: 10472 RVA: 0x001F0B20 File Offset: 0x001EED20
		private void OnLoadedAllArchiveData(DataContext context)
		{
			List<long> removeList = new List<long>();
			this._updatingErrorEffect = true;
			foreach (KeyValuePair<long, SpecialEffectWrapper> entry in this._effectDict)
			{
				SpecialEffectBase effect = entry.Value.Effect;
				bool flag = this.IsErrorEffectOnLoad(effect);
				if (flag)
				{
					removeList.Add(entry.Key);
				}
				else
				{
					effect.OnEnable(context);
					this.AddDataUid(context, effect);
					effect.OnDataAdded(context);
					FeatureEffectBase featureEffect = effect as FeatureEffectBase;
					bool flag2 = featureEffect != null;
					if (flag2)
					{
						this._featureEffectDict.Add(this.GetFeatureEffectKey(featureEffect.CharacterId, featureEffect.FeatureId), featureEffect.Id);
					}
				}
			}
			foreach (long removeId in removeList)
			{
				this.RemoveElement_EffectDict(removeId, context);
			}
			this._updatingErrorEffect = false;
			this._requestUpdateCombatSkillCharIds.Remove(DomainManager.Taiwu.GetTaiwuCharId());
			foreach (int charId in this._requestUpdateCombatSkillCharIds)
			{
				GameData.Domains.Character.Character character;
				bool flag3 = DomainManager.Character.TryGetElement_Objects(charId, out character);
				if (flag3)
				{
					this.UpdateEquippedSkillEffect(context, character);
				}
			}
			this._requestUpdateCombatSkillCharIds.Clear();
		}

		// Token: 0x060028E9 RID: 10473 RVA: 0x001F0CCC File Offset: 0x001EEECC
		private unsafe bool IsErrorEffect(SpecialEffectBase effect)
		{
			bool flag = effect.Type < 0;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = effect.CharObj == null;
				if (flag2)
				{
					result = true;
				}
				else
				{
					CombatSkillEffectBase skillEffect = effect as CombatSkillEffectBase;
					bool flag3 = skillEffect != null && !skillEffect.IsLegendaryBookEffect;
					if (flag3)
					{
						sbyte activeType = SpecialEffect.Instance[skillEffect.EffectId].EffectActiveType;
						bool flag4 = activeType == 1 || activeType == 0;
						if (flag4)
						{
							return true;
						}
						bool flag5 = activeType == 2 && !effect.CharObj.IsCombatSkillEquipped(skillEffect.SkillKey.SkillTemplateId);
						if (flag5)
						{
							return true;
						}
						GameData.Domains.CombatSkill.CombatSkill combatSkill;
						bool flag6 = !DomainManager.CombatSkill.TryGetElement_CombatSkills(skillEffect.SkillKey, out combatSkill);
						if (flag6)
						{
							return true;
						}
					}
					WugEffectBase wugEffect = effect as WugEffectBase;
					bool flag7 = wugEffect != null && (*effect.CharObj.GetEatingItems()).IndexOfWug(wugEffect.WugTemplateId) < 0;
					result = flag7;
				}
			}
			return result;
		}

		// Token: 0x060028EA RID: 10474 RVA: 0x001F0DD8 File Offset: 0x001EEFD8
		private bool IsErrorEffectOnLoad(SpecialEffectBase effect)
		{
			bool flag = this.IsErrorEffect(effect);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				CombatSkillEffectBase combatSkillEffect = effect as CombatSkillEffectBase;
				bool flag2 = combatSkillEffect != null && !combatSkillEffect.IsLegendaryBookEffect;
				result = (flag2 && combatSkillEffect.SkillInstance.GetSpecialEffectId() != effect.Id);
			}
			return result;
		}

		// Token: 0x060028EB RID: 10475 RVA: 0x001F0E30 File Offset: 0x001EF030
		[DomainMethod]
		public List<CastBoostEffectDisplayData> GetAllCostNeiliEffectData(int charId, short skillId)
		{
			this._costNeiliEffectDisplayDataCache.Clear();
			bool flag = !DomainManager.Combat.IsCharInCombat(charId, true);
			List<CastBoostEffectDisplayData> costNeiliEffectDisplayDataCache;
			if (flag)
			{
				costNeiliEffectDisplayDataCache = this._costNeiliEffectDisplayDataCache;
			}
			else
			{
				bool flag2 = skillId >= 0;
				if (flag2)
				{
					this.ModifyData(charId, skillId, 235, this._costNeiliEffectDisplayDataCache, -1, -1, -1);
				}
				costNeiliEffectDisplayDataCache = this._costNeiliEffectDisplayDataCache;
			}
			return costNeiliEffectDisplayDataCache;
		}

		// Token: 0x060028EC RID: 10476 RVA: 0x001F0E92 File Offset: 0x001EF092
		[DomainMethod]
		public void CostNeiliEffect(DataContext context, int charId, short skillId, short effectId)
		{
			Events.RaiseCombatCostNeiliConfirm(context, charId, skillId, effectId);
		}

		// Token: 0x060028ED RID: 10477 RVA: 0x001F0EA0 File Offset: 0x001EF0A0
		[DomainMethod]
		public bool CanCostTrickDuringPreparingSkill(int charId, short skillId)
		{
			return this.ModifyData(charId, skillId, 324, false, -1, -1, -1);
		}

		// Token: 0x060028EE RID: 10478 RVA: 0x001F0EC4 File Offset: 0x001EF0C4
		[DomainMethod]
		public bool CostTrickDuringPreparingSkill(DataContext context, int charId, int trickIndex)
		{
			bool flag = !DomainManager.Combat.IsCharInCombat(charId, true);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				CombatCharacter combatChar = DomainManager.Combat.GetElement_CombatCharacterDict(charId);
				short preparingSkillId = combatChar.GetPreparingSkillId();
				bool flag2 = preparingSkillId < 0 || !this.CanCostTrickDuringPreparingSkill(charId, preparingSkillId);
				if (flag2)
				{
					result = false;
				}
				else
				{
					IReadOnlyDictionary<int, sbyte> tricks = combatChar.GetTricks().Tricks;
					sbyte trickType;
					bool flag3 = !tricks.TryGetValue(trickIndex, out trickType);
					if (flag3)
					{
						result = false;
					}
					else
					{
						DomainManager.Combat.RemoveTrick(context, combatChar, trickType, 1, true, trickIndex);
						Events.RaiseCostTrickDuringPreparingSkill(context, charId);
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x060028EF RID: 10479 RVA: 0x001F0F60 File Offset: 0x001EF160
		public long Add(DataContext context, SpecialEffectBase effect)
		{
			bool flag = this._effectDict.ContainsKey(this._nextEffectId);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(50, 1);
				defaultInterpolatedStringHandler.AppendLiteral("SpecialEffectSystem: nextEffectId ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(this._nextEffectId);
				defaultInterpolatedStringHandler.AppendLiteral(" already exists.");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			bool flag2 = effect == null;
			if (flag2)
			{
				throw new Exception("SpecialEffectSystem: effect can not be null.");
			}
			effect.Id = this._nextEffectId;
			this.AddElement_EffectDict(effect.Id, new SpecialEffectWrapper
			{
				Effect = effect
			}, context);
			do
			{
				this._nextEffectId += 1L;
				bool flag3 = this._nextEffectId < 0L;
				if (flag3)
				{
					this._nextEffectId = 0L;
				}
			}
			while (this._effectDict.ContainsKey(this._nextEffectId));
			this.SetNextEffectId(this._nextEffectId, context);
			effect.OnEnable(context);
			this.AddDataUid(context, effect);
			effect.OnDataAdded(context);
			return effect.Id;
		}

		// Token: 0x060028F0 RID: 10480 RVA: 0x001F106C File Offset: 0x001EF26C
		public long Add(DataContext context, int charId, string effectName)
		{
			string fullTypeName = "GameData.Domains.SpecialEffect." + effectName;
			Type specialEffectType = Type.GetType(fullTypeName);
			bool flag = specialEffectType == null;
			if (flag)
			{
				throw new Exception("Cannot find type '" + fullTypeName + "'.");
			}
			SpecialEffectBase effect = (SpecialEffectBase)Activator.CreateInstance(specialEffectType, new object[]
			{
				charId
			});
			this.Add(context, effect);
			return effect.Id;
		}

		// Token: 0x060028F1 RID: 10481 RVA: 0x001F10E0 File Offset: 0x001EF2E0
		public void Add(DataContext context, int charId, short skillTemplateId, sbyte effectActiveType, sbyte direction = -1)
		{
			CombatSkillKey skillKey = new CombatSkillKey(charId, skillTemplateId);
			GameData.Domains.CombatSkill.CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(skillKey);
			CombatSkillItem skillConfig = Config.CombatSkill.Instance[skillTemplateId];
			bool flag = direction < 0;
			if (flag)
			{
				direction = skill.GetDirection();
			}
			short effectTemplateId = (short)((direction == 0) ? skillConfig.DirectEffectID : ((direction == 1) ? skillConfig.ReverseEffectID : -1));
			bool flag2 = effectTemplateId < 0;
			if (!flag2)
			{
				SpecialEffectItem effectConfig = SpecialEffect.Instance[effectTemplateId];
				bool flag3 = effectConfig.EffectActiveType != effectActiveType || string.IsNullOrEmpty(effectConfig.ClassName);
				if (!flag3)
				{
					string fullTypeName = "GameData.Domains.SpecialEffect." + effectConfig.ClassName;
					Type specialEffectType = Type.GetType(fullTypeName);
					bool flag4 = specialEffectType == null;
					if (flag4)
					{
						throw new Exception("Cannot find type '" + fullTypeName + "'.");
					}
					SpecialEffectBase effect = (effectActiveType == 3) ? ((SpecialEffectBase)Activator.CreateInstance(specialEffectType, new object[]
					{
						skillKey,
						direction
					})) : ((SpecialEffectBase)Activator.CreateInstance(specialEffectType, new object[]
					{
						skillKey
					}));
					this.Add(context, effect);
					bool flag5 = effectActiveType == 3 || effectActiveType == 2 || effectActiveType == 1;
					if (flag5)
					{
						skill.SetSpecialEffectId(effect.Id, context);
					}
				}
			}
		}

		// Token: 0x060028F2 RID: 10482 RVA: 0x001F123C File Offset: 0x001EF43C
		public long AddCombatStateEffect(DataContext context, int charId, sbyte stateType, short stateId, short power, bool reverse)
		{
			CombatStateEffect effect = new CombatStateEffect(charId, stateType, stateId, power, reverse);
			this.Add(context, effect);
			return effect.Id;
		}

		// Token: 0x060028F3 RID: 10483 RVA: 0x001F126C File Offset: 0x001EF46C
		public void AddEquipmentEffect(DataContext context, int charId, ItemKey equipKey)
		{
			short effectId = DomainManager.Item.GetBaseEquipment(equipKey).GetEquipmentEffectId();
			bool flag = effectId < 0;
			if (!flag)
			{
				this.AddEquipmentEffect(context, charId, equipKey, effectId);
			}
		}

		// Token: 0x060028F4 RID: 10484 RVA: 0x001F12A0 File Offset: 0x001EF4A0
		public long AddEquipmentEffect(DataContext context, int charId, ItemKey equipKey, short effectId)
		{
			string className = EquipmentEffect.Instance[effectId].EffectClassName;
			bool flag = string.IsNullOrEmpty(className);
			long result;
			if (flag)
			{
				result = -1L;
			}
			else
			{
				string fullTypeName = "GameData.Domains.SpecialEffect." + className;
				Type specialEffectType = Type.GetType(fullTypeName);
				bool flag2 = specialEffectType == null;
				if (flag2)
				{
					throw new Exception("Cannot find type '" + fullTypeName + "'.");
				}
				SpecialEffectBase effect = (SpecialEffectBase)Activator.CreateInstance(specialEffectType, new object[]
				{
					charId,
					equipKey
				});
				result = this.Add(context, effect);
			}
			return result;
		}

		// Token: 0x060028F5 RID: 10485 RVA: 0x001F133C File Offset: 0x001EF53C
		public void AddFeatureEffect(DataContext context, int charId, short featureId)
		{
			string className = CharacterFeature.Instance[featureId].AssociatedSpecialEffect;
			bool flag = string.IsNullOrEmpty(className);
			if (!flag)
			{
				long effectKey = this.GetFeatureEffectKey(charId, featureId);
				string fullTypeName = "GameData.Domains.SpecialEffect." + className;
				Type specialEffectType = Type.GetType(fullTypeName);
				bool flag2 = specialEffectType == null;
				if (flag2)
				{
					throw new Exception("Cannot find type '" + fullTypeName + "'.");
				}
				SpecialEffectBase effect = (SpecialEffectBase)Activator.CreateInstance(specialEffectType, new object[]
				{
					charId,
					featureId
				});
				this.Add(context, effect);
				this._featureEffectDict.Add(effectKey, effect.Id);
			}
		}

		// Token: 0x060028F6 RID: 10486 RVA: 0x001F13F0 File Offset: 0x001EF5F0
		public long AddAddPenetrateAndPenetrateResistEffect(DataContext context, int charId, OuterAndInnerInts addPenetrate, OuterAndInnerInts addPenetrateResist)
		{
			SpecialEffectBase effect = new AddPenetrateAndPenetrateResist(charId, addPenetrate, addPenetrateResist);
			this.Add(context, effect);
			return effect.Id;
		}

		// Token: 0x060028F7 RID: 10487 RVA: 0x001F141C File Offset: 0x001EF61C
		public long AddAddMaxHealthEffect(DataContext context, int charId, int addMaxHealth)
		{
			SpecialEffectBase effect = new AddMaxHealth(charId, addMaxHealth);
			this.Add(context, effect);
			return effect.Id;
		}

		// Token: 0x060028F8 RID: 10488 RVA: 0x001F1448 File Offset: 0x001EF648
		public SpecialEffectBase Get(long effectId)
		{
			return this._effectDict.ContainsKey(effectId) ? this._effectDict[effectId].Effect : null;
		}

		// Token: 0x060028F9 RID: 10489 RVA: 0x001F147C File Offset: 0x001EF67C
		public void Remove(DataContext context, long effectId)
		{
			bool flag = !this._effectDict.ContainsKey(effectId);
			if (!flag)
			{
				SpecialEffectBase effect = this._effectDict[effectId].Effect;
				this.RemoveDataUid(context, effect);
				effect.OnDisable(context);
				this.RemoveElement_EffectDict(effectId, context);
			}
		}

		// Token: 0x060028FA RID: 10490 RVA: 0x001F14CC File Offset: 0x001EF6CC
		public void Remove(DataContext context, int charId, short skillTemplateId, sbyte effectActiveType)
		{
			CombatSkillKey skillKey = new CombatSkillKey(charId, skillTemplateId);
			GameData.Domains.CombatSkill.CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(skillKey);
			bool flag = skill.GetSpecialEffectId() >= 0L;
			if (flag)
			{
				CombatSkillItem skillConfig = Config.CombatSkill.Instance[skillTemplateId];
				sbyte direction = skill.GetDirection();
				short effectTemplateId = (short)((direction == 0) ? skillConfig.DirectEffectID : skillConfig.ReverseEffectID);
				bool flag2 = SpecialEffect.Instance[effectTemplateId].EffectActiveType == effectActiveType && (effectActiveType == 3 || effectActiveType == 2);
				if (flag2)
				{
					this.Remove(context, skill.GetSpecialEffectId());
					skill.SetSpecialEffectId(-1L, context);
				}
			}
		}

		// Token: 0x060028FB RID: 10491 RVA: 0x001F1570 File Offset: 0x001EF770
		private void Remove(DataContext context, List<long> removeIdList, bool removeAffectedData = true, bool clearCharObj = false)
		{
			foreach (long effectId in removeIdList)
			{
				SpecialEffectWrapper effectWrapper;
				bool flag = this._effectDict.TryGetValue(effectId, out effectWrapper);
				if (flag)
				{
					SpecialEffectBase effect = effectWrapper.Effect;
					if (clearCharObj)
					{
						effect.CharObj = null;
					}
					effect.OnDisable(context);
					if (removeAffectedData)
					{
						this.RemoveDataUid(context, effect);
					}
					GameData.Domains.CombatSkill.CombatSkill skill;
					bool flag2 = effect is CombatSkillEffectBase && DomainManager.CombatSkill.TryGetElement_CombatSkills(((CombatSkillEffectBase)effect).SkillKey, out skill);
					if (flag2)
					{
						skill.SetSpecialEffectId(-1L, context);
					}
					this.RemoveElement_EffectDict(effectId, context);
				}
			}
		}

		// Token: 0x060028FC RID: 10492 RVA: 0x001F1648 File Offset: 0x001EF848
		public void RemoveAllEffectsInCombat(DataContext context)
		{
			List<long> removeList = ObjectPool<List<long>>.Instance.Get();
			removeList.Clear();
			foreach (KeyValuePair<long, SpecialEffectWrapper> entry in this._effectDict)
			{
				SpecialEffectBase effect = entry.Value.Effect;
				CombatStateEffect stateEffect = effect as CombatStateEffect;
				bool flag = stateEffect != null;
				if (flag)
				{
					stateEffect.OnDisable(context);
					this.RemoveDataUid(context, stateEffect);
					removeList.Add(entry.Key);
				}
				else
				{
					PestleEffectBase pestleEffect = effect as PestleEffectBase;
					bool flag2 = pestleEffect != null;
					if (flag2)
					{
						pestleEffect.OnDisable(context);
						this.RemoveDataUid(context, pestleEffect);
						removeList.Add(entry.Key);
					}
					else
					{
						EquipmentEffectBase equipEffect = effect as EquipmentEffectBase;
						bool flag3 = equipEffect != null && equipEffect.AutoRemoveAfterCombat;
						if (flag3)
						{
							equipEffect.OnDisable(context);
							this.RemoveDataUid(context, equipEffect);
							removeList.Add(entry.Key);
						}
						else
						{
							CombatSkillEffectBase skillEffect = effect as CombatSkillEffectBase;
							bool flag4 = skillEffect != null && !skillEffect.IsLegendaryBookEffect;
							if (flag4)
							{
								sbyte activeType = SpecialEffect.Instance[skillEffect.EffectId].EffectActiveType;
								bool flag5 = activeType == 1 || activeType == 0;
								if (flag5)
								{
									skillEffect.OnDisable(context);
									this.RemoveDataUid(context, skillEffect);
									removeList.Add(entry.Key);
									bool flag6 = activeType == 1;
									if (flag6)
									{
										DomainManager.CombatSkill.GetElement_CombatSkills(skillEffect.SkillKey).SetSpecialEffectId(-1L, context);
									}
								}
							}
						}
					}
				}
			}
			for (int i = 0; i < removeList.Count; i++)
			{
				this.RemoveElement_EffectDict(removeList[i], context);
			}
			ObjectPool<List<long>>.Instance.Return(removeList);
		}

		// Token: 0x060028FD RID: 10493 RVA: 0x001F184C File Offset: 0x001EFA4C
		public void RemoveFeatureEffect(DataContext context, int charId, short featureId)
		{
			long effectKey = this.GetFeatureEffectKey(charId, featureId);
			bool flag = this._featureEffectDict.ContainsKey(effectKey);
			if (flag)
			{
				this.Remove(context, this._featureEffectDict[effectKey]);
				this._featureEffectDict.Remove(effectKey);
			}
		}

		// Token: 0x060028FE RID: 10494 RVA: 0x001F1898 File Offset: 0x001EFA98
		public void RemoveAllEquippedSkillEffects(DataContext context, GameData.Domains.Character.Character character)
		{
			foreach (short skillId in character.GetCombatSkillEquipment())
			{
				DomainManager.SpecialEffect.Remove(context, character.GetId(), skillId, 2);
			}
		}

		// Token: 0x060028FF RID: 10495 RVA: 0x001F18F8 File Offset: 0x001EFAF8
		public void RemoveAllBrokenSkillEffects(DataContext context, GameData.Domains.Character.Character character)
		{
			List<short> learnedCombatSkills = character.GetLearnedCombatSkills();
			foreach (short skillId in learnedCombatSkills)
			{
				bool flag = skillId >= 0;
				if (flag)
				{
					DomainManager.SpecialEffect.Remove(context, character.GetId(), skillId, 3);
				}
			}
		}

		// Token: 0x06002900 RID: 10496 RVA: 0x001F196C File Offset: 0x001EFB6C
		public void AddAllBrokenSkillEffects(DataContext context, GameData.Domains.Character.Character character)
		{
			List<short> learnedCombatSkills = character.GetLearnedCombatSkills();
			foreach (short skillId in learnedCombatSkills)
			{
				bool flag = skillId >= 0;
				if (flag)
				{
					DomainManager.SpecialEffect.Add(context, character.GetId(), skillId, 3, -1);
				}
			}
		}

		// Token: 0x06002901 RID: 10497 RVA: 0x001F19E0 File Offset: 0x001EFBE0
		private long GetFeatureEffectKey(int charId, short featureId)
		{
			return (long)charId * 1000000L + (long)featureId;
		}

		// Token: 0x06002902 RID: 10498 RVA: 0x001F1A00 File Offset: 0x001EFC00
		private void AddDataUid(DataContext context, SpecialEffectBase effect)
		{
			bool flag = effect.AffectDatas != null;
			if (flag)
			{
				foreach (AffectedDataKey dataKey in effect.AffectDatas.Keys)
				{
					this.AppendDataUid(context, effect, dataKey);
				}
			}
		}

		// Token: 0x06002903 RID: 10499 RVA: 0x001F1A70 File Offset: 0x001EFC70
		public void AppendDataUid(DataContext context, SpecialEffectBase effect, AffectedDataKey dataKey)
		{
			bool flag = !this._affectedDatas.ContainsKey(dataKey.CharId);
			if (flag)
			{
				this.AddElement_AffectedDatas(dataKey.CharId, new AffectedData(dataKey.CharId));
			}
			AffectedData affectedDatas = this._affectedDatas[dataKey.CharId];
			SpecialEffectList effectList = affectedDatas.GetEffectList(dataKey.FieldId, true);
			effectList.EffectList.Add(effect);
			affectedDatas.SetEffectList(context, dataKey.FieldId, effectList);
		}

		// Token: 0x06002904 RID: 10500 RVA: 0x001F1AEC File Offset: 0x001EFCEC
		public void RemoveDataUid(DataContext context, SpecialEffectBase effect, AffectedDataKey dataKey)
		{
			bool flag = effect.AffectDatas == null || !effect.AffectDatas.ContainsKey(dataKey);
			if (!flag)
			{
				AffectedData affectedData = this._affectedDatas[dataKey.CharId];
				SpecialEffectList effectList = affectedData.GetEffectList(dataKey.FieldId, false);
				if (effectList != null)
				{
					effectList.EffectList.Remove(effect);
				}
				affectedData.SetEffectList(context, dataKey.FieldId, effectList);
			}
		}

		// Token: 0x06002905 RID: 10501 RVA: 0x001F1B5C File Offset: 0x001EFD5C
		public void RemoveDataUid(DataContext context, SpecialEffectBase effect)
		{
			bool flag = effect.AffectDatas == null;
			if (!flag)
			{
				foreach (AffectedDataKey dataKey in effect.AffectDatas.Keys)
				{
					AffectedData affectedData = this._affectedDatas[dataKey.CharId];
					SpecialEffectList effectList = affectedData.GetEffectList(dataKey.FieldId, false);
					if (effectList != null)
					{
						effectList.EffectList.Remove(effect);
					}
					affectedData.SetEffectList(context, dataKey.FieldId, effectList);
				}
			}
		}

		// Token: 0x06002906 RID: 10502 RVA: 0x001F1C08 File Offset: 0x001EFE08
		public void InvalidateCache(DataContext context, int charId, ushort fieldId)
		{
			bool flag = this._affectedDatas.ContainsKey(charId);
			if (flag)
			{
				AffectedData affectedData = this._affectedDatas[charId];
				affectedData.SetEffectList(context, fieldId, affectedData.GetEffectList(fieldId, false));
			}
			else
			{
				this.AddElement_AffectedDatas(charId, new AffectedData(charId));
				AffectedData affectedDatas = this._affectedDatas[charId];
				affectedDatas.SetEffectList(context, fieldId, null);
				this.RemoveElement_AffectedDatas(charId);
			}
		}

		// Token: 0x06002907 RID: 10503 RVA: 0x001F1C77 File Offset: 0x001EFE77
		public void ChangeAffectedDataUids(DataContext context, SpecialEffectBase effect, Dictionary<AffectedDataKey, EDataModifyType> affectDataUids)
		{
			this.RemoveDataUid(context, effect);
			effect.AffectDatas = affectDataUids;
			this.AddDataUid(context, effect);
		}

		// Token: 0x06002908 RID: 10504 RVA: 0x001F1C94 File Offset: 0x001EFE94
		public int ModifyValue(int charId, ushort fieldId, int value, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1, int extraAdd = 0, int extraAddPercent = 0, int extraTotalPercentAdd = 0, int extraTotalPercentReduce = 0)
		{
			return this.ModifyValue(charId, -1, fieldId, value, customParam0, customParam1, customParam2, extraAdd, extraAddPercent, extraTotalPercentAdd, extraTotalPercentReduce);
		}

		// Token: 0x06002909 RID: 10505 RVA: 0x001F1CC0 File Offset: 0x001EFEC0
		public int ModifyValue(int charId, short skillId, ushort fieldId, int value, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1, int extraAdd = 0, int extraAddPercent = 0, int extraTotalPercentAdd = 0, int extraTotalPercentReduce = 0)
		{
			CValueModify modify = this.GetModify(charId, skillId, fieldId, customParam0, customParam1, customParam2, EDataSumType.All);
			modify += new CValueModify(extraAdd, extraAddPercent, extraTotalPercentAdd, extraTotalPercentReduce);
			return value * modify;
		}

		// Token: 0x0600290A RID: 10506 RVA: 0x001F1D10 File Offset: 0x001EFF10
		public int ModifyValueCustom(int charId, ushort fieldId, int value, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1, int extraAdd = 0, int extraAddPercent = 0, int extraTotalPercentAdd = 0, int extraTotalPercentReduce = 0)
		{
			return this.ModifyValueCustom(charId, -1, fieldId, value, customParam0, customParam1, customParam2, extraAdd, extraAddPercent, extraTotalPercentAdd, extraTotalPercentReduce);
		}

		// Token: 0x0600290B RID: 10507 RVA: 0x001F1D3C File Offset: 0x001EFF3C
		public int ModifyValueCustom(int charId, short skillId, ushort fieldId, int value, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1, int extraAdd = 0, int extraAddPercent = 0, int extraTotalPercentAdd = 0, int extraTotalPercentReduce = 0)
		{
			value = this.ModifyValue(charId, skillId, fieldId, value, customParam0, customParam1, customParam2, extraAdd, extraAddPercent, extraTotalPercentAdd, extraTotalPercentReduce);
			return this.ModifyData(charId, skillId, fieldId, value, customParam0, customParam1, customParam2);
		}

		// Token: 0x0600290C RID: 10508 RVA: 0x001F1D7C File Offset: 0x001EFF7C
		public CValueModify GetModify(int charId, ushort fieldId, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1, EDataSumType valueSumType = EDataSumType.All)
		{
			return this.GetModify(charId, -1, fieldId, customParam0, customParam1, customParam2, valueSumType);
		}

		// Token: 0x0600290D RID: 10509 RVA: 0x001F1DA0 File Offset: 0x001EFFA0
		public CValueModify GetModify(int charId, short combatSkillId, ushort fieldId, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1, EDataSumType valueSumType = EDataSumType.All)
		{
			bool flag = valueSumType == EDataSumType.None;
			CValueModify result;
			if (flag)
			{
				result = CValueModify.Zero;
			}
			else
			{
				int add = this.GetModifyValue(charId, combatSkillId, fieldId, EDataModifyType.Add, customParam0, customParam1, customParam2, valueSumType);
				int addPercent = this.GetModifyValue(charId, combatSkillId, fieldId, EDataModifyType.AddPercent, customParam0, customParam1, customParam2, valueSumType);
				ValueTuple<int, int> totalPercent = this.GetTotalPercentModifyValue(charId, combatSkillId, fieldId, customParam0, customParam1, customParam2);
				int totalPercentAdd = (valueSumType != EDataSumType.OnlyReduce) ? totalPercent.Item1 : 0;
				int totalPercentReduce = (valueSumType != EDataSumType.OnlyAdd) ? totalPercent.Item2 : 0;
				result = new CValueModify(add, addPercent, totalPercentAdd, totalPercentReduce);
			}
			return result;
		}

		// Token: 0x0600290E RID: 10510 RVA: 0x001F1E38 File Offset: 0x001F0038
		public int GetModifyValue(int charId, ushort fieldId, EDataModifyType modifyType, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1, EDataSumType valueSumType = EDataSumType.All)
		{
			return this.GetModifyValue(charId, -1, fieldId, modifyType, customParam0, customParam1, customParam2, valueSumType);
		}

		// Token: 0x0600290F RID: 10511 RVA: 0x001F1E5C File Offset: 0x001F005C
		public int GetModifyValue(int charId, short combatSkillId, ushort fieldId, EDataModifyType modifyType, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1, EDataSumType valueSumType = EDataSumType.All)
		{
			bool flag = modifyType != EDataModifyType.Add && modifyType != EDataModifyType.AddPercent;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid DataModifyType ");
				defaultInterpolatedStringHandler.AppendFormatted<EDataModifyType>(modifyType);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			bool flag2 = !this._affectedDatas.ContainsKey(charId);
			int result;
			if (flag2)
			{
				result = 0;
			}
			else
			{
				SpecialEffectList effectList = this.GetElement_AffectedDatas(charId).GetEffectList(fieldId, false);
				int modifyValue = 0;
				bool flag3 = effectList != null;
				if (flag3)
				{
					AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
					for (int i = 0; i < effectList.EffectList.Count; i++)
					{
						SpecialEffectBase effect = effectList.EffectList[i];
						EDataModifyType type;
						bool flag4 = effect.AffectDatas.TryGetValue(dataKey, out type) && type == modifyType;
						if (flag4)
						{
							int value = effect.GetModifyValue(dataKey, modifyValue);
							modifyValue = valueSumType.Sum(modifyValue, value);
						}
					}
				}
				result = modifyValue;
			}
			return result;
		}

		// Token: 0x06002910 RID: 10512 RVA: 0x001F1F68 File Offset: 0x001F0168
		[return: TupleElementNames(new string[]
		{
			"add",
			"reduce"
		})]
		public ValueTuple<int, int> GetTotalPercentModifyValue(int charId, short combatSkillId, ushort fieldId, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
		{
			ValueTuple<int, int> modifyValue = new ValueTuple<int, int>(0, 0);
			bool flag = !this._affectedDatas.ContainsKey(charId);
			ValueTuple<int, int> result;
			if (flag)
			{
				result = modifyValue;
			}
			else
			{
				SpecialEffectList effectList = this.GetElement_AffectedDatas(charId).GetEffectList(fieldId, false);
				bool flag2 = effectList != null;
				if (flag2)
				{
					AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
					for (int i = 0; i < effectList.EffectList.Count; i++)
					{
						SpecialEffectBase effect = effectList.EffectList[i];
						EDataModifyType type;
						bool flag3 = effect.AffectDatas.TryGetValue(dataKey, out type) && type == EDataModifyType.TotalPercent;
						if (flag3)
						{
							int value = effect.GetModifyValue(dataKey, 0);
							bool flag4 = value > modifyValue.Item1;
							if (flag4)
							{
								modifyValue.Item1 = value;
							}
							else
							{
								bool flag5 = value < modifyValue.Item2;
								if (flag5)
								{
									modifyValue.Item2 = value;
								}
							}
						}
					}
				}
				result = modifyValue;
			}
			return result;
		}

		// Token: 0x06002911 RID: 10513 RVA: 0x001F2064 File Offset: 0x001F0264
		private void CalcCustomModifyEffectList(AffectedDataKey dataKey, List<SpecialEffectBase> customEffectList)
		{
			customEffectList.Clear();
			bool flag = !this._affectedDatas.ContainsKey(dataKey.CharId);
			if (!flag)
			{
				SpecialEffectList effectList = this.GetElement_AffectedDatas(dataKey.CharId).GetEffectList(dataKey.FieldId, false);
				bool flag2 = effectList == null;
				if (!flag2)
				{
					for (int i = 0; i < effectList.EffectList.Count; i++)
					{
						SpecialEffectBase effect = effectList.EffectList[i];
						EDataModifyType type;
						bool flag3 = effect.AffectDatas.TryGetValue(dataKey, out type) && type == EDataModifyType.Custom;
						if (flag3)
						{
							customEffectList.Add(effect);
						}
					}
				}
			}
		}

		// Token: 0x06002912 RID: 10514 RVA: 0x001F210C File Offset: 0x001F030C
		public bool ModifyData(int charId, short combatSkillId, ushort fieldId, bool dataValue, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
		{
			AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
			List<SpecialEffectBase> customEffectList = ObjectPool<List<SpecialEffectBase>>.Instance.Get();
			this.CalcCustomModifyEffectList(dataKey, customEffectList);
			for (int i = 0; i < customEffectList.Count; i++)
			{
				dataValue = customEffectList[i].GetModifiedValue(dataKey, dataValue);
			}
			ObjectPool<List<SpecialEffectBase>>.Instance.Return(customEffectList);
			return dataValue;
		}

		// Token: 0x06002913 RID: 10515 RVA: 0x001F2178 File Offset: 0x001F0378
		public int ModifyData(int charId, short combatSkillId, ushort fieldId, int dataValue, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
		{
			AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
			List<SpecialEffectBase> customEffectList = ObjectPool<List<SpecialEffectBase>>.Instance.Get();
			this.CalcCustomModifyEffectList(dataKey, customEffectList);
			for (int i = 0; i < customEffectList.Count; i++)
			{
				dataValue = customEffectList[i].GetModifiedValue(dataKey, dataValue);
			}
			ObjectPool<List<SpecialEffectBase>>.Instance.Return(customEffectList);
			return dataValue;
		}

		// Token: 0x06002914 RID: 10516 RVA: 0x001F21E4 File Offset: 0x001F03E4
		public long ModifyData(int charId, short combatSkillId, ushort fieldId, long dataValue, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
		{
			AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
			List<SpecialEffectBase> customEffectList = ObjectPool<List<SpecialEffectBase>>.Instance.Get();
			this.CalcCustomModifyEffectList(dataKey, customEffectList);
			for (int i = 0; i < customEffectList.Count; i++)
			{
				dataValue = customEffectList[i].GetModifiedValue(dataKey, dataValue);
			}
			ObjectPool<List<SpecialEffectBase>>.Instance.Return(customEffectList);
			return dataValue;
		}

		// Token: 0x06002915 RID: 10517 RVA: 0x001F2250 File Offset: 0x001F0450
		public HitOrAvoidInts ModifyData(int charId, short combatSkillId, ushort fieldId, HitOrAvoidInts dataValue, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
		{
			AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
			List<SpecialEffectBase> customEffectList = ObjectPool<List<SpecialEffectBase>>.Instance.Get();
			this.CalcCustomModifyEffectList(dataKey, customEffectList);
			for (int i = 0; i < customEffectList.Count; i++)
			{
				dataValue = customEffectList[i].GetModifiedValue(dataKey, dataValue);
			}
			ObjectPool<List<SpecialEffectBase>>.Instance.Return(customEffectList);
			return dataValue;
		}

		// Token: 0x06002916 RID: 10518 RVA: 0x001F22BC File Offset: 0x001F04BC
		public NeiliProportionOfFiveElements ModifyData(int charId, short combatSkillId, ushort fieldId, NeiliProportionOfFiveElements dataValue, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
		{
			AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
			List<SpecialEffectBase> customEffectList = ObjectPool<List<SpecialEffectBase>>.Instance.Get();
			this.CalcCustomModifyEffectList(dataKey, customEffectList);
			for (int i = 0; i < customEffectList.Count; i++)
			{
				dataValue = customEffectList[i].GetModifiedValue(dataKey, dataValue);
			}
			ObjectPool<List<SpecialEffectBase>>.Instance.Return(customEffectList);
			return dataValue;
		}

		// Token: 0x06002917 RID: 10519 RVA: 0x001F2328 File Offset: 0x001F0528
		public OuterAndInnerInts ModifyData(int charId, short combatSkillId, ushort fieldId, OuterAndInnerInts dataValue, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
		{
			AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
			List<SpecialEffectBase> customEffectList = ObjectPool<List<SpecialEffectBase>>.Instance.Get();
			this.CalcCustomModifyEffectList(dataKey, customEffectList);
			for (int i = 0; i < customEffectList.Count; i++)
			{
				dataValue = customEffectList[i].GetModifiedValue(dataKey, dataValue);
			}
			ObjectPool<List<SpecialEffectBase>>.Instance.Return(customEffectList);
			return dataValue;
		}

		// Token: 0x06002918 RID: 10520 RVA: 0x001F2394 File Offset: 0x001F0594
		public List<NeedTrick> ModifyData(int charId, short combatSkillId, ushort fieldId, List<NeedTrick> dataValue, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
		{
			AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
			List<SpecialEffectBase> customEffectList = ObjectPool<List<SpecialEffectBase>>.Instance.Get();
			this.CalcCustomModifyEffectList(dataKey, customEffectList);
			for (int i = 0; i < customEffectList.Count; i++)
			{
				dataValue = customEffectList[i].GetModifiedValue(dataKey, dataValue);
			}
			ObjectPool<List<SpecialEffectBase>>.Instance.Return(customEffectList);
			return dataValue;
		}

		// Token: 0x06002919 RID: 10521 RVA: 0x001F2400 File Offset: 0x001F0600
		public ValueTuple<sbyte, sbyte> ModifyData(int charId, short combatSkillId, ushort fieldId, ValueTuple<sbyte, sbyte> dataValue, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
		{
			AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
			List<SpecialEffectBase> customEffectList = ObjectPool<List<SpecialEffectBase>>.Instance.Get();
			this.CalcCustomModifyEffectList(dataKey, customEffectList);
			for (int i = 0; i < customEffectList.Count; i++)
			{
				dataValue = customEffectList[i].GetModifiedValue(dataKey, dataValue);
			}
			ObjectPool<List<SpecialEffectBase>>.Instance.Return(customEffectList);
			return dataValue;
		}

		// Token: 0x0600291A RID: 10522 RVA: 0x001F246C File Offset: 0x001F066C
		public List<ItemKeyAndCount> ModifyData(int charId, short combatSkillId, ushort fieldId, List<ItemKeyAndCount> dataValue, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
		{
			AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
			List<SpecialEffectBase> customEffectList = ObjectPool<List<SpecialEffectBase>>.Instance.Get();
			this.CalcCustomModifyEffectList(dataKey, customEffectList);
			for (int i = 0; i < customEffectList.Count; i++)
			{
				dataValue = customEffectList[i].GetModifiedValue(dataKey, dataValue);
			}
			ObjectPool<List<SpecialEffectBase>>.Instance.Return(customEffectList);
			return dataValue;
		}

		// Token: 0x0600291B RID: 10523 RVA: 0x001F24D8 File Offset: 0x001F06D8
		public List<CastBoostEffectDisplayData> ModifyData(int charId, short combatSkillId, ushort fieldId, List<CastBoostEffectDisplayData> dataValue, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
		{
			AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
			List<SpecialEffectBase> customEffectList = ObjectPool<List<SpecialEffectBase>>.Instance.Get();
			this.CalcCustomModifyEffectList(dataKey, customEffectList);
			for (int i = 0; i < customEffectList.Count; i++)
			{
				dataValue = customEffectList[i].GetModifiedValue(dataKey, dataValue);
			}
			ObjectPool<List<SpecialEffectBase>>.Instance.Return(customEffectList);
			return dataValue;
		}

		// Token: 0x0600291C RID: 10524 RVA: 0x001F2544 File Offset: 0x001F0744
		public List<CombatSkillEffectData> ModifyData(int charId, short combatSkillId, ushort fieldId, List<CombatSkillEffectData> dataValue, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
		{
			AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
			List<SpecialEffectBase> customEffectList = ObjectPool<List<SpecialEffectBase>>.Instance.Get();
			this.CalcCustomModifyEffectList(dataKey, customEffectList);
			for (int i = 0; i < customEffectList.Count; i++)
			{
				dataValue = customEffectList[i].GetModifiedValue(dataKey, dataValue);
			}
			ObjectPool<List<SpecialEffectBase>>.Instance.Return(customEffectList);
			return dataValue;
		}

		// Token: 0x0600291D RID: 10525 RVA: 0x001F25B0 File Offset: 0x001F07B0
		public BoolArray8 ModifyData(int charId, short combatSkillId, ushort fieldId, BoolArray8 dataValue, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
		{
			AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
			List<SpecialEffectBase> customEffectList = ObjectPool<List<SpecialEffectBase>>.Instance.Get();
			this.CalcCustomModifyEffectList(dataKey, customEffectList);
			for (int i = 0; i < customEffectList.Count; i++)
			{
				dataValue = customEffectList[i].GetModifiedValue(dataKey, dataValue);
			}
			ObjectPool<List<SpecialEffectBase>>.Instance.Return(customEffectList);
			return dataValue;
		}

		// Token: 0x0600291E RID: 10526 RVA: 0x001F261C File Offset: 0x001F081C
		public CombatCharacter ModifyData(int charId, short combatSkillId, ushort fieldId, CombatCharacter dataValue, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
		{
			AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
			List<SpecialEffectBase> customEffectList = ObjectPool<List<SpecialEffectBase>>.Instance.Get();
			this.CalcCustomModifyEffectList(dataKey, customEffectList);
			for (int i = 0; i < customEffectList.Count; i++)
			{
				dataValue = customEffectList[i].GetModifiedValue(dataKey, dataValue);
			}
			ObjectPool<List<SpecialEffectBase>>.Instance.Return(customEffectList);
			return dataValue;
		}

		// Token: 0x0600291F RID: 10527 RVA: 0x001F2688 File Offset: 0x001F0888
		public List<int> ModifyData(int charId, short combatSkillId, ushort fieldId, List<int> dataValue, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
		{
			AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
			List<SpecialEffectBase> customEffectList = ObjectPool<List<SpecialEffectBase>>.Instance.Get();
			for (int i = 0; i < customEffectList.Count; i++)
			{
				dataValue = customEffectList[i].GetModifiedValue(dataKey, dataValue);
			}
			ObjectPool<List<SpecialEffectBase>>.Instance.Return(customEffectList);
			return dataValue;
		}

		// Token: 0x06002920 RID: 10528 RVA: 0x001F26EC File Offset: 0x001F08EC
		public void OnCharacterCreated(DataContext context, GameData.Domains.Character.Character character)
		{
			bool flag = !this._affectedDatas.ContainsKey(character.GetId());
			if (flag)
			{
				this.AddElement_AffectedDatas(character.GetId(), new AffectedData(character.GetId()));
			}
		}

		// Token: 0x06002921 RID: 10529 RVA: 0x001F272C File Offset: 0x001F092C
		public void OnCharacterRemoved(DataContext context, GameData.Domains.Character.Character character)
		{
			List<long> removeIdList = ObjectPool<List<long>>.Instance.Get();
			removeIdList.Clear();
			foreach (KeyValuePair<long, SpecialEffectWrapper> entry in this._effectDict)
			{
				bool flag = entry.Value.Effect.CharacterId == character.GetId();
				if (flag)
				{
					removeIdList.Add(entry.Key);
				}
			}
			this.Remove(context, removeIdList, false, false);
			this.RemoveElement_AffectedDatas(character.GetId());
			ObjectPool<List<long>>.Instance.Return(removeIdList);
		}

		// Token: 0x06002922 RID: 10530 RVA: 0x001F27E0 File Offset: 0x001F09E0
		public void AddCombatSkillSpecialEffects(DataContext context, int charId, short[] combatSkills, sbyte activeType)
		{
			foreach (short skillId in combatSkills)
			{
				bool flag = skillId >= 0;
				if (flag)
				{
					DomainManager.SpecialEffect.Add(context, charId, skillId, activeType, -1);
				}
			}
		}

		// Token: 0x06002923 RID: 10531 RVA: 0x001F2824 File Offset: 0x001F0A24
		public void UpdateEquippedSkillEffect(DataContext context, GameData.Domains.Character.Character character)
		{
			bool updatingErrorEffect = this._updatingErrorEffect;
			if (updatingErrorEffect)
			{
				this._requestUpdateCombatSkillCharIds.Add(character.GetId());
			}
			else
			{
				foreach (short skillId in character.GetLearnedCombatSkills())
				{
					this.Remove(context, character.GetId(), skillId, 2);
				}
				foreach (short skillId2 in character.GetCombatSkillEquipment())
				{
					bool combatSkillCanAffect = character.GetCombatSkillCanAffect(skillId2);
					if (combatSkillCanAffect)
					{
						this.Add(context, character.GetId(), skillId2, 2, -1);
					}
				}
			}
		}

		// Token: 0x06002924 RID: 10532 RVA: 0x001F2900 File Offset: 0x001F0B00
		private void OnAddWug(DataContext context, int charId, short wugTemplateId, short replacedWug)
		{
			long effectId = this.Add(context, charId, Config.Medicine.Instance[wugTemplateId].SpecialEffectClass);
			SpecialEffectWrapper effectWrapper;
			bool flag = this._effectDict.TryGetValue(effectId, out effectWrapper);
			if (flag)
			{
				((WugEffectBase)effectWrapper.Effect).OnEffectAdded(context, replacedWug);
			}
		}

		// Token: 0x06002925 RID: 10533 RVA: 0x001F294D File Offset: 0x001F0B4D
		public void AddBrokenEffectChangedDuringAdvance(long effectId, int charId, short skillId)
		{
			this._brokenEffectsChangedDuringAdvance.Add(new ValueTuple<long, int, short>(effectId, charId, skillId));
		}

		// Token: 0x06002926 RID: 10534 RVA: 0x001F2964 File Offset: 0x001F0B64
		public void ApplyBrokenEffectChangedDuringAdvance(DataContext context)
		{
			foreach (ValueTuple<long, int, short> changedEffect in this._brokenEffectsChangedDuringAdvance)
			{
				GameData.Domains.CombatSkill.CombatSkill skill;
				sbyte direction = DomainManager.CombatSkill.TryGetElement_CombatSkills(new CombatSkillKey(changedEffect.Item2, changedEffect.Item3), out skill) ? skill.GetDirection() : -1;
				bool flag = changedEffect.Item1 >= 0L;
				if (flag)
				{
					DomainManager.SpecialEffect.Remove(context, changedEffect.Item1);
				}
				bool flag2 = direction >= 0;
				if (flag2)
				{
					DomainManager.SpecialEffect.Add(context, changedEffect.Item2, changedEffect.Item3, 3, direction);
				}
			}
			this._brokenEffectsChangedDuringAdvance.Clear();
		}

		// Token: 0x06002927 RID: 10535 RVA: 0x001F2A3C File Offset: 0x001F0C3C
		public void SaveEffect(DataContext context, long effectId)
		{
			this.SetElement_EffectDict(effectId, this._effectDict[effectId], context);
		}

		// Token: 0x06002928 RID: 10536 RVA: 0x001F2A54 File Offset: 0x001F0C54
		public override void PackCrossArchiveGameData(CrossArchiveGameData crossArchiveGameData)
		{
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			List<SpecialEffectWrapper> taiwuEffects = crossArchiveGameData.TaiwuEffects = new List<SpecialEffectWrapper>();
			foreach (SpecialEffectWrapper effectWrapper in this._effectDict.Values)
			{
				bool flag = effectWrapper.Effect == null || effectWrapper.Effect.CharacterId != taiwuCharId;
				if (!flag)
				{
					taiwuEffects.Add(effectWrapper);
				}
			}
		}

		// Token: 0x06002929 RID: 10537 RVA: 0x001F2AF4 File Offset: 0x001F0CF4
		public void UnpackCrossArchiveGameData_Items(DataContext context, CrossArchiveGameData crossArchiveGameData)
		{
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			List<SpecialEffectWrapper> taiwuEffects = crossArchiveGameData.TaiwuEffects;
			bool flag = taiwuEffects == null;
			if (!flag)
			{
				Dictionary<long, sbyte> weaponEffectIds = new Dictionary<long, sbyte>();
				Dictionary<long, sbyte> skillEffectIds = new Dictionary<long, sbyte>();
				for (sbyte i = 0; i < 14; i += 1)
				{
					long weaponEffectId;
					bool flag2 = DomainManager.Extra.TryGetElement_LegendaryBookWeaponEffectId(i, out weaponEffectId);
					if (flag2)
					{
						weaponEffectIds[weaponEffectId] = i;
					}
					LongList skillEffectIdList;
					bool flag3;
					if (DomainManager.Extra.TryGetElement_LegendaryBookSkillEffectId(i, out skillEffectIdList))
					{
						List<long> items = skillEffectIdList.Items;
						flag3 = (items != null && items.Count > 0);
					}
					else
					{
						flag3 = false;
					}
					bool flag4 = flag3;
					if (flag4)
					{
						foreach (long skillEffectId in skillEffectIdList.Items)
						{
							bool flag5 = skillEffectId >= 0L;
							if (flag5)
							{
								skillEffectIds[skillEffectId] = i;
							}
						}
					}
				}
				List<SpecialEffectWrapper> collectedEffects = new List<SpecialEffectWrapper>();
				foreach (SpecialEffectWrapper effectWrapper in taiwuEffects)
				{
					SpecialEffectBase effect = effectWrapper.Effect;
					EquipmentEffectBase equipmentEffect = effect as EquipmentEffectBase;
					bool flag6 = equipmentEffect != null;
					bool anyMissing;
					if (flag6)
					{
						equipmentEffect.EquipItemKey = DomainManager.Item.UnpackCrossArchiveItem(context, crossArchiveGameData, equipmentEffect.EquipItemKey);
						anyMissing = !equipmentEffect.EquipItemKey.IsValid();
					}
					else
					{
						CombatSkillEffectBase combatSkillEffect = effect as CombatSkillEffectBase;
						bool flag7 = combatSkillEffect != null && combatSkillEffect.IsLegendaryBookEffect;
						if (!flag7)
						{
							continue;
						}
						combatSkillEffect.SkillKey.CharId = taiwuCharId;
						GameData.Domains.CombatSkill.CombatSkill combatSkill;
						anyMissing = !DomainManager.CombatSkill.TryGetElement_CombatSkills(combatSkillEffect.SkillKey, out combatSkill);
					}
					effect.OnDreamBack(context);
					effect.CharObj = taiwuChar;
					effect.CharacterId = taiwuCharId;
					collectedEffects.Add(effectWrapper);
					long prevEffectId = effect.Id;
					long currEffectId = (anyMissing || this.IsErrorEffect(effect)) ? -1L : this.Add(context, effect);
					sbyte weaponSkillType;
					bool flag8 = weaponEffectIds.TryGetValue(prevEffectId, out weaponSkillType);
					if (flag8)
					{
						DomainManager.Extra.SetLegendaryBookWeaponEffectId(context, weaponSkillType, currEffectId, prevEffectId);
					}
					sbyte combatSkillType;
					bool flag9 = skillEffectIds.TryGetValue(prevEffectId, out combatSkillType);
					if (flag9)
					{
						DomainManager.Extra.SetLegendaryBookSkillEffectId(context, combatSkillType, currEffectId, prevEffectId);
					}
				}
				foreach (SpecialEffectWrapper collectedEffect in collectedEffects)
				{
					taiwuEffects.Remove(collectedEffect);
				}
			}
		}

		// Token: 0x0600292A RID: 10538 RVA: 0x001F2DE8 File Offset: 0x001F0FE8
		public void UnpackCrossArchiveGameData_CombatSkills(DataContext context, CrossArchiveGameData crossArchiveGameData)
		{
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			List<SpecialEffectWrapper> taiwuEffects = crossArchiveGameData.TaiwuEffects;
			bool flag = taiwuEffects == null;
			if (!flag)
			{
				List<SpecialEffectWrapper> collectedEffects = new List<SpecialEffectWrapper>();
				Dictionary<int, CombatSkillEffectBase> existEffects = new Dictionary<int, CombatSkillEffectBase>();
				foreach (SpecialEffectWrapper effectWrapper in this._effectDict.Values)
				{
					SpecialEffectBase effect = effectWrapper.Effect;
					bool flag2 = effect.CharacterId != taiwuCharId;
					if (!flag2)
					{
						CombatSkillEffectBase combatSkillEffect = effect as CombatSkillEffectBase;
						bool flag3 = combatSkillEffect == null;
						if (!flag3)
						{
							bool isLegendaryBookEffect = combatSkillEffect.IsLegendaryBookEffect;
							if (!isLegendaryBookEffect)
							{
								existEffects.Add(combatSkillEffect.Type, combatSkillEffect);
							}
						}
					}
				}
				foreach (SpecialEffectWrapper effectWrapper2 in taiwuEffects)
				{
					SpecialEffectBase effectBase = effectWrapper2.Effect;
					CombatSkillEffectBase effect2 = effectBase as CombatSkillEffectBase;
					bool flag4 = effect2 == null;
					if (!flag4)
					{
						bool isLegendaryBookEffect2 = effect2.IsLegendaryBookEffect;
						if (!isLegendaryBookEffect2)
						{
							effect2.CharObj = taiwuChar;
							effect2.CharacterId = taiwuCharId;
							effect2.SkillKey.CharId = taiwuCharId;
							collectedEffects.Add(effectWrapper2);
							bool flag5 = this.IsErrorEffect(effect2);
							if (!flag5)
							{
								CombatSkillEffectBase existEffect;
								bool flag6 = existEffects.TryGetValue(effect2.Type, out existEffect);
								if (flag6)
								{
									effect2.Id = existEffect.Id;
									DomainManager.Extra.AddConflictSpecialEffect(context, effectWrapper2);
								}
								else
								{
									effect2.OnDreamBack(context);
									this.Add(context, effect2);
									effect2.SkillInstance.SetSpecialEffectId(effect2.Id, context);
								}
							}
						}
					}
				}
				foreach (SpecialEffectWrapper collectedEffect in collectedEffects)
				{
					taiwuEffects.Remove(collectedEffect);
				}
			}
		}

		// Token: 0x0600292B RID: 10539 RVA: 0x001F3024 File Offset: 0x001F1224
		public void OverwriteSpecialEffectWithConflictCombatSkill(DataContext context, ConflictCombatSkill conflictCombatSkill)
		{
			CombatSkillEffectBase effect;
			bool flag = !DomainManager.Extra.TryGetConflictCombatSkillEffect(conflictCombatSkill.TemplateId, out effect);
			if (!flag)
			{
				effect.CharObj = DomainManager.Taiwu.GetTaiwu();
				effect.CharacterId = DomainManager.Taiwu.GetTaiwuCharId();
				effect.SkillKey.CharId = DomainManager.Taiwu.GetTaiwuCharId();
				bool flag2 = this.IsErrorEffect(effect);
				if (!flag2)
				{
					bool flag3 = !this._effectDict.ContainsKey(effect.Id);
					if (flag3)
					{
						foreach (KeyValuePair<long, SpecialEffectWrapper> keyValuePair in this._effectDict)
						{
							long num;
							SpecialEffectWrapper specialEffectWrapper;
							keyValuePair.Deconstruct(out num, out specialEffectWrapper);
							long effectId = num;
							SpecialEffectWrapper effectWrapper = specialEffectWrapper;
							bool flag4 = effectWrapper.Effect.CharacterId == effect.CharacterId && effectWrapper.Effect.Type == effect.Type;
							if (flag4)
							{
								effect.Id = effectId;
							}
						}
					}
					this.Remove(context, effect.Id);
					effect.OnDreamBack(context);
					this.Add(context, effect);
					effect.SkillInstance.SetSpecialEffectId(effect.Id, context);
				}
			}
		}

		// Token: 0x0600292C RID: 10540 RVA: 0x001F3178 File Offset: 0x001F1378
		public SpecialEffectDomain() : base(3)
		{
			this._effectDict = new Dictionary<long, SpecialEffectWrapper>(0);
			this._nextEffectId = 0L;
			this._affectedDatas = new Dictionary<int, AffectedData>(0);
			this.HelperDataAffectedDatas = new ObjectCollectionHelperData(17, 2, SpecialEffectDomain.CacheInfluencesAffectedDatas, this._dataStatesAffectedDatas, false);
			this.OnInitializedDomainData();
		}

		// Token: 0x0600292D RID: 10541 RVA: 0x001F320C File Offset: 0x001F140C
		private SpecialEffectWrapper GetElement_EffectDict(long elementId)
		{
			return this._effectDict[elementId];
		}

		// Token: 0x0600292E RID: 10542 RVA: 0x001F322C File Offset: 0x001F142C
		private bool TryGetElement_EffectDict(long elementId, out SpecialEffectWrapper value)
		{
			return this._effectDict.TryGetValue(elementId, out value);
		}

		// Token: 0x0600292F RID: 10543 RVA: 0x001F324C File Offset: 0x001F144C
		private unsafe void AddElement_EffectDict(long elementId, SpecialEffectWrapper value, DataContext context)
		{
			this._effectDict.Add(elementId, value);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, this.DataStates, SpecialEffectDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<long>(17, 0, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<long>(17, 0, elementId, 0);
			}
		}

		// Token: 0x06002930 RID: 10544 RVA: 0x001F32B0 File Offset: 0x001F14B0
		private unsafe void SetElement_EffectDict(long elementId, SpecialEffectWrapper value, DataContext context)
		{
			this._effectDict[elementId] = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, this.DataStates, SpecialEffectDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<long>(17, 0, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<long>(17, 0, elementId, 0);
			}
		}

		// Token: 0x06002931 RID: 10545 RVA: 0x001F3312 File Offset: 0x001F1512
		private void RemoveElement_EffectDict(long elementId, DataContext context)
		{
			this._effectDict.Remove(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, this.DataStates, SpecialEffectDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<long>(17, 0, elementId);
		}

		// Token: 0x06002932 RID: 10546 RVA: 0x001F333F File Offset: 0x001F153F
		private void ClearEffectDict(DataContext context)
		{
			this._effectDict.Clear();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, this.DataStates, SpecialEffectDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(17, 0);
		}

		// Token: 0x06002933 RID: 10547 RVA: 0x001F336C File Offset: 0x001F156C
		private long GetNextEffectId()
		{
			return this._nextEffectId;
		}

		// Token: 0x06002934 RID: 10548 RVA: 0x001F3384 File Offset: 0x001F1584
		private unsafe void SetNextEffectId(long value, DataContext context)
		{
			this._nextEffectId = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, this.DataStates, SpecialEffectDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(17, 1, 8);
			*(long*)pData = this._nextEffectId;
			pData += 8;
		}

		// Token: 0x06002935 RID: 10549 RVA: 0x001F33C4 File Offset: 0x001F15C4
		private AffectedData GetElement_AffectedDatas(int objectId)
		{
			return this._affectedDatas[objectId];
		}

		// Token: 0x06002936 RID: 10550 RVA: 0x001F33E4 File Offset: 0x001F15E4
		private bool TryGetElement_AffectedDatas(int objectId, out AffectedData element)
		{
			return this._affectedDatas.TryGetValue(objectId, out element);
		}

		// Token: 0x06002937 RID: 10551 RVA: 0x001F3403 File Offset: 0x001F1603
		private void AddElement_AffectedDatas(int objectId, AffectedData instance)
		{
			instance.CollectionHelperData = this.HelperDataAffectedDatas;
			instance.DataStatesOffset = this._dataStatesAffectedDatas.Create();
			this._affectedDatas.Add(objectId, instance);
		}

		// Token: 0x06002938 RID: 10552 RVA: 0x001F3434 File Offset: 0x001F1634
		private void RemoveElement_AffectedDatas(int objectId)
		{
			AffectedData instance;
			bool flag = !this._affectedDatas.TryGetValue(objectId, out instance);
			if (!flag)
			{
				this._dataStatesAffectedDatas.Remove(instance.DataStatesOffset);
				this._affectedDatas.Remove(objectId);
			}
		}

		// Token: 0x06002939 RID: 10553 RVA: 0x001F3478 File Offset: 0x001F1678
		private void ClearAffectedDatas()
		{
			this._dataStatesAffectedDatas.Clear();
			this._affectedDatas.Clear();
		}

		// Token: 0x0600293A RID: 10554 RVA: 0x001F3494 File Offset: 0x001F1694
		private int GetElementField_AffectedDatas(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
		{
			AffectedData instance;
			bool flag = !this._affectedDatas.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				string tag = "GetElementField_AffectedDatas";
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
					this._dataStatesAffectedDatas.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
				switch (fieldId)
				{
				case 0:
					result = Serializer.Serialize(instance.GetId(), dataPool);
					break;
				case 1:
					result = Serializer.Serialize(instance.GetMaxStrength(), dataPool);
					break;
				case 2:
					result = Serializer.Serialize(instance.GetMaxDexterity(), dataPool);
					break;
				case 3:
					result = Serializer.Serialize(instance.GetMaxConcentration(), dataPool);
					break;
				case 4:
					result = Serializer.Serialize(instance.GetMaxVitality(), dataPool);
					break;
				case 5:
					result = Serializer.Serialize(instance.GetMaxEnergy(), dataPool);
					break;
				case 6:
					result = Serializer.Serialize(instance.GetMaxIntelligence(), dataPool);
					break;
				case 7:
					result = Serializer.Serialize(instance.GetRecoveryOfStance(), dataPool);
					break;
				case 8:
					result = Serializer.Serialize(instance.GetRecoveryOfBreath(), dataPool);
					break;
				case 9:
					result = Serializer.Serialize(instance.GetMoveSpeed(), dataPool);
					break;
				case 10:
					result = Serializer.Serialize(instance.GetRecoveryOfFlaw(), dataPool);
					break;
				case 11:
					result = Serializer.Serialize(instance.GetCastSpeed(), dataPool);
					break;
				case 12:
					result = Serializer.Serialize(instance.GetRecoveryOfBlockedAcupoint(), dataPool);
					break;
				case 13:
					result = Serializer.Serialize(instance.GetWeaponSwitchSpeed(), dataPool);
					break;
				case 14:
					result = Serializer.Serialize(instance.GetAttackSpeed(), dataPool);
					break;
				case 15:
					result = Serializer.Serialize(instance.GetInnerRatio(), dataPool);
					break;
				case 16:
					result = Serializer.Serialize(instance.GetRecoveryOfQiDisorder(), dataPool);
					break;
				case 17:
					result = Serializer.Serialize(instance.GetMinorAttributeFixMaxValue(), dataPool);
					break;
				case 18:
					result = Serializer.Serialize(instance.GetMinorAttributeFixMinValue(), dataPool);
					break;
				case 19:
					result = Serializer.Serialize(instance.GetResistOfHotPoison(), dataPool);
					break;
				case 20:
					result = Serializer.Serialize(instance.GetResistOfGloomyPoison(), dataPool);
					break;
				case 21:
					result = Serializer.Serialize(instance.GetResistOfColdPoison(), dataPool);
					break;
				case 22:
					result = Serializer.Serialize(instance.GetResistOfRedPoison(), dataPool);
					break;
				case 23:
					result = Serializer.Serialize(instance.GetResistOfRottenPoison(), dataPool);
					break;
				case 24:
					result = Serializer.Serialize(instance.GetResistOfIllusoryPoison(), dataPool);
					break;
				case 25:
					result = Serializer.Serialize(instance.GetDisplayAge(), dataPool);
					break;
				case 26:
					result = Serializer.Serialize(instance.GetNeiliProportionOfFiveElements(), dataPool);
					break;
				case 27:
					result = Serializer.Serialize(instance.GetWeaponMaxPower(), dataPool);
					break;
				case 28:
					result = Serializer.Serialize(instance.GetWeaponUseRequirement(), dataPool);
					break;
				case 29:
					result = Serializer.Serialize(instance.GetWeaponAttackRange(), dataPool);
					break;
				case 30:
					result = Serializer.Serialize(instance.GetArmorMaxPower(), dataPool);
					break;
				case 31:
					result = Serializer.Serialize(instance.GetArmorUseRequirement(), dataPool);
					break;
				case 32:
					result = Serializer.Serialize(instance.GetHitStrength(), dataPool);
					break;
				case 33:
					result = Serializer.Serialize(instance.GetHitTechnique(), dataPool);
					break;
				case 34:
					result = Serializer.Serialize(instance.GetHitSpeed(), dataPool);
					break;
				case 35:
					result = Serializer.Serialize(instance.GetHitMind(), dataPool);
					break;
				case 36:
					result = Serializer.Serialize(instance.GetHitCanChange(), dataPool);
					break;
				case 37:
					result = Serializer.Serialize(instance.GetHitChangeEffectPercent(), dataPool);
					break;
				case 38:
					result = Serializer.Serialize(instance.GetAvoidStrength(), dataPool);
					break;
				case 39:
					result = Serializer.Serialize(instance.GetAvoidTechnique(), dataPool);
					break;
				case 40:
					result = Serializer.Serialize(instance.GetAvoidSpeed(), dataPool);
					break;
				case 41:
					result = Serializer.Serialize(instance.GetAvoidMind(), dataPool);
					break;
				case 42:
					result = Serializer.Serialize(instance.GetAvoidCanChange(), dataPool);
					break;
				case 43:
					result = Serializer.Serialize(instance.GetAvoidChangeEffectPercent(), dataPool);
					break;
				case 44:
					result = Serializer.Serialize(instance.GetPenetrateOuter(), dataPool);
					break;
				case 45:
					result = Serializer.Serialize(instance.GetPenetrateInner(), dataPool);
					break;
				case 46:
					result = Serializer.Serialize(instance.GetPenetrateResistOuter(), dataPool);
					break;
				case 47:
					result = Serializer.Serialize(instance.GetPenetrateResistInner(), dataPool);
					break;
				case 48:
					result = Serializer.Serialize(instance.GetNeiliAllocationAttack(), dataPool);
					break;
				case 49:
					result = Serializer.Serialize(instance.GetNeiliAllocationAgile(), dataPool);
					break;
				case 50:
					result = Serializer.Serialize(instance.GetNeiliAllocationDefense(), dataPool);
					break;
				case 51:
					result = Serializer.Serialize(instance.GetNeiliAllocationAssist(), dataPool);
					break;
				case 52:
					result = Serializer.Serialize(instance.GetHappiness(), dataPool);
					break;
				case 53:
					result = Serializer.Serialize(instance.GetMaxHealth(), dataPool);
					break;
				case 54:
					result = Serializer.Serialize(instance.GetHealthCost(), dataPool);
					break;
				case 55:
					result = Serializer.Serialize(instance.GetMoveSpeedCanChange(), dataPool);
					break;
				case 56:
					result = Serializer.Serialize(instance.GetAttackerHitStrength(), dataPool);
					break;
				case 57:
					result = Serializer.Serialize(instance.GetAttackerHitTechnique(), dataPool);
					break;
				case 58:
					result = Serializer.Serialize(instance.GetAttackerHitSpeed(), dataPool);
					break;
				case 59:
					result = Serializer.Serialize(instance.GetAttackerHitMind(), dataPool);
					break;
				case 60:
					result = Serializer.Serialize(instance.GetAttackerAvoidStrength(), dataPool);
					break;
				case 61:
					result = Serializer.Serialize(instance.GetAttackerAvoidTechnique(), dataPool);
					break;
				case 62:
					result = Serializer.Serialize(instance.GetAttackerAvoidSpeed(), dataPool);
					break;
				case 63:
					result = Serializer.Serialize(instance.GetAttackerAvoidMind(), dataPool);
					break;
				case 64:
					result = Serializer.Serialize(instance.GetAttackerPenetrateOuter(), dataPool);
					break;
				case 65:
					result = Serializer.Serialize(instance.GetAttackerPenetrateInner(), dataPool);
					break;
				case 66:
					result = Serializer.Serialize(instance.GetAttackerPenetrateResistOuter(), dataPool);
					break;
				case 67:
					result = Serializer.Serialize(instance.GetAttackerPenetrateResistInner(), dataPool);
					break;
				case 68:
					result = Serializer.Serialize(instance.GetAttackHitType(), dataPool);
					break;
				case 69:
					result = Serializer.Serialize(instance.GetMakeDirectDamage(), dataPool);
					break;
				case 70:
					result = Serializer.Serialize(instance.GetMakeBounceDamage(), dataPool);
					break;
				case 71:
					result = Serializer.Serialize(instance.GetMakeFightBackDamage(), dataPool);
					break;
				case 72:
					result = Serializer.Serialize(instance.GetMakePoisonLevel(), dataPool);
					break;
				case 73:
					result = Serializer.Serialize(instance.GetMakePoisonValue(), dataPool);
					break;
				case 74:
					result = Serializer.Serialize(instance.GetAttackerHitOdds(), dataPool);
					break;
				case 75:
					result = Serializer.Serialize(instance.GetAttackerFightBackHitOdds(), dataPool);
					break;
				case 76:
					result = Serializer.Serialize(instance.GetAttackerPursueOdds(), dataPool);
					break;
				case 77:
					result = Serializer.Serialize(instance.GetMakedInjuryChangeToOld(), dataPool);
					break;
				case 78:
					result = Serializer.Serialize(instance.GetMakedPoisonChangeToOld(), dataPool);
					break;
				case 79:
					result = Serializer.Serialize(instance.GetMakeDamageType(), dataPool);
					break;
				case 80:
					result = Serializer.Serialize(instance.GetCanMakeInjuryToNoInjuryPart(), dataPool);
					break;
				case 81:
					result = Serializer.Serialize(instance.GetMakePoisonType(), dataPool);
					break;
				case 82:
					result = Serializer.Serialize(instance.GetNormalAttackWeapon(), dataPool);
					break;
				case 83:
					result = Serializer.Serialize(instance.GetNormalAttackTrick(), dataPool);
					break;
				case 84:
					result = Serializer.Serialize(instance.GetExtraFlawCount(), dataPool);
					break;
				case 85:
					result = Serializer.Serialize(instance.GetAttackCanBounce(), dataPool);
					break;
				case 86:
					result = Serializer.Serialize(instance.GetAttackCanFightBack(), dataPool);
					break;
				case 87:
					result = Serializer.Serialize(instance.GetMakeFightBackInjuryMark(), dataPool);
					break;
				case 88:
					result = Serializer.Serialize(instance.GetLegSkillUseShoes(), dataPool);
					break;
				case 89:
					result = Serializer.Serialize(instance.GetAttackerFinalDamageValue(), dataPool);
					break;
				case 90:
					result = Serializer.Serialize(instance.GetDefenderHitStrength(), dataPool);
					break;
				case 91:
					result = Serializer.Serialize(instance.GetDefenderHitTechnique(), dataPool);
					break;
				case 92:
					result = Serializer.Serialize(instance.GetDefenderHitSpeed(), dataPool);
					break;
				case 93:
					result = Serializer.Serialize(instance.GetDefenderHitMind(), dataPool);
					break;
				case 94:
					result = Serializer.Serialize(instance.GetDefenderAvoidStrength(), dataPool);
					break;
				case 95:
					result = Serializer.Serialize(instance.GetDefenderAvoidTechnique(), dataPool);
					break;
				case 96:
					result = Serializer.Serialize(instance.GetDefenderAvoidSpeed(), dataPool);
					break;
				case 97:
					result = Serializer.Serialize(instance.GetDefenderAvoidMind(), dataPool);
					break;
				case 98:
					result = Serializer.Serialize(instance.GetDefenderPenetrateOuter(), dataPool);
					break;
				case 99:
					result = Serializer.Serialize(instance.GetDefenderPenetrateInner(), dataPool);
					break;
				case 100:
					result = Serializer.Serialize(instance.GetDefenderPenetrateResistOuter(), dataPool);
					break;
				case 101:
					result = Serializer.Serialize(instance.GetDefenderPenetrateResistInner(), dataPool);
					break;
				case 102:
					result = Serializer.Serialize(instance.GetAcceptDirectDamage(), dataPool);
					break;
				case 103:
					result = Serializer.Serialize(instance.GetAcceptBounceDamage(), dataPool);
					break;
				case 104:
					result = Serializer.Serialize(instance.GetAcceptFightBackDamage(), dataPool);
					break;
				case 105:
					result = Serializer.Serialize(instance.GetAcceptPoisonLevel(), dataPool);
					break;
				case 106:
					result = Serializer.Serialize(instance.GetAcceptPoisonValue(), dataPool);
					break;
				case 107:
					result = Serializer.Serialize(instance.GetDefenderHitOdds(), dataPool);
					break;
				case 108:
					result = Serializer.Serialize(instance.GetDefenderFightBackHitOdds(), dataPool);
					break;
				case 109:
					result = Serializer.Serialize(instance.GetDefenderPursueOdds(), dataPool);
					break;
				case 110:
					result = Serializer.Serialize(instance.GetAcceptMaxInjuryCount(), dataPool);
					break;
				case 111:
					result = Serializer.Serialize(instance.GetBouncePower(), dataPool);
					break;
				case 112:
					result = Serializer.Serialize(instance.GetFightBackPower(), dataPool);
					break;
				case 113:
					result = Serializer.Serialize(instance.GetDirectDamageInnerRatio(), dataPool);
					break;
				case 114:
					result = Serializer.Serialize(instance.GetDefenderFinalDamageValue(), dataPool);
					break;
				case 115:
					result = Serializer.Serialize(instance.GetDirectDamageValue(), dataPool);
					break;
				case 116:
					result = Serializer.Serialize(instance.GetDirectInjuryMark(), dataPool);
					break;
				case 117:
					result = Serializer.Serialize(instance.GetGoneMadInjury(), dataPool);
					break;
				case 118:
					result = Serializer.Serialize(instance.GetHealInjurySpeed(), dataPool);
					break;
				case 119:
					result = Serializer.Serialize(instance.GetHealInjuryBuff(), dataPool);
					break;
				case 120:
					result = Serializer.Serialize(instance.GetHealInjuryDebuff(), dataPool);
					break;
				case 121:
					result = Serializer.Serialize(instance.GetHealPoisonSpeed(), dataPool);
					break;
				case 122:
					result = Serializer.Serialize(instance.GetHealPoisonBuff(), dataPool);
					break;
				case 123:
					result = Serializer.Serialize(instance.GetHealPoisonDebuff(), dataPool);
					break;
				case 124:
					result = Serializer.Serialize(instance.GetFleeSpeed(), dataPool);
					break;
				case 125:
					result = Serializer.Serialize(instance.GetMaxFlawCount(), dataPool);
					break;
				case 126:
					result = Serializer.Serialize(instance.GetCanAddFlaw(), dataPool);
					break;
				case 127:
					result = Serializer.Serialize(instance.GetFlawLevel(), dataPool);
					break;
				case 128:
					result = Serializer.Serialize(instance.GetFlawLevelCanReduce(), dataPool);
					break;
				case 129:
					result = Serializer.Serialize(instance.GetFlawCount(), dataPool);
					break;
				case 130:
					result = Serializer.Serialize(instance.GetMaxAcupointCount(), dataPool);
					break;
				case 131:
					result = Serializer.Serialize(instance.GetCanAddAcupoint(), dataPool);
					break;
				case 132:
					result = Serializer.Serialize(instance.GetAcupointLevel(), dataPool);
					break;
				case 133:
					result = Serializer.Serialize(instance.GetAcupointLevelCanReduce(), dataPool);
					break;
				case 134:
					result = Serializer.Serialize(instance.GetAcupointCount(), dataPool);
					break;
				case 135:
					result = Serializer.Serialize(instance.GetAddNeiliAllocation(), dataPool);
					break;
				case 136:
					result = Serializer.Serialize(instance.GetCostNeiliAllocation(), dataPool);
					break;
				case 137:
					result = Serializer.Serialize(instance.GetCanChangeNeiliAllocation(), dataPool);
					break;
				case 138:
					result = Serializer.Serialize(instance.GetCanGetTrick(), dataPool);
					break;
				case 139:
					result = Serializer.Serialize(instance.GetGetTrickType(), dataPool);
					break;
				case 140:
					result = Serializer.Serialize(instance.GetAttackBodyPart(), dataPool);
					break;
				case 141:
					result = Serializer.Serialize(instance.GetWeaponEquipAttack(), dataPool);
					break;
				case 142:
					result = Serializer.Serialize(instance.GetWeaponEquipDefense(), dataPool);
					break;
				case 143:
					result = Serializer.Serialize(instance.GetArmorEquipAttack(), dataPool);
					break;
				case 144:
					result = Serializer.Serialize(instance.GetArmorEquipDefense(), dataPool);
					break;
				case 145:
					result = Serializer.Serialize(instance.GetAttackRangeForward(), dataPool);
					break;
				case 146:
					result = Serializer.Serialize(instance.GetAttackRangeBackward(), dataPool);
					break;
				case 147:
					result = Serializer.Serialize(instance.GetMoveCanBeStopped(), dataPool);
					break;
				case 148:
					result = Serializer.Serialize(instance.GetCanForcedMove(), dataPool);
					break;
				case 149:
					result = Serializer.Serialize(instance.GetMobilityCanBeRemoved(), dataPool);
					break;
				case 150:
					result = Serializer.Serialize(instance.GetMobilityCostByEffect(), dataPool);
					break;
				case 151:
					result = Serializer.Serialize(instance.GetMoveDistance(), dataPool);
					break;
				case 152:
					result = Serializer.Serialize(instance.GetJumpPrepareFrame(), dataPool);
					break;
				case 153:
					result = Serializer.Serialize(instance.GetBounceInjuryMark(), dataPool);
					break;
				case 154:
					result = Serializer.Serialize(instance.GetSkillHasCost(), dataPool);
					break;
				case 155:
					result = Serializer.Serialize(instance.GetCombatStateEffect(), dataPool);
					break;
				case 156:
					result = Serializer.Serialize(instance.GetChangeNeedUseSkill(), dataPool);
					break;
				case 157:
					result = Serializer.Serialize(instance.GetChangeDistanceIsMove(), dataPool);
					break;
				case 158:
					result = Serializer.Serialize(instance.GetReplaceCharHit(), dataPool);
					break;
				case 159:
					result = Serializer.Serialize(instance.GetCanAddPoison(), dataPool);
					break;
				case 160:
					result = Serializer.Serialize(instance.GetCanReducePoison(), dataPool);
					break;
				case 161:
					result = Serializer.Serialize(instance.GetReducePoisonValue(), dataPool);
					break;
				case 162:
					result = Serializer.Serialize(instance.GetPoisonCanAffect(), dataPool);
					break;
				case 163:
					result = Serializer.Serialize(instance.GetPoisonAffectCount(), dataPool);
					break;
				case 164:
					result = Serializer.Serialize(instance.GetCostTricks(), dataPool);
					break;
				case 165:
					result = Serializer.Serialize(instance.GetJumpMoveDistance(), dataPool);
					break;
				case 166:
					result = Serializer.Serialize(instance.GetCombatStateToAdd(), dataPool);
					break;
				case 167:
					result = Serializer.Serialize(instance.GetCombatStatePower(), dataPool);
					break;
				case 168:
					result = Serializer.Serialize(instance.GetBreakBodyPartInjuryCount(), dataPool);
					break;
				case 169:
					result = Serializer.Serialize(instance.GetBodyPartIsBroken(), dataPool);
					break;
				case 170:
					result = Serializer.Serialize(instance.GetMaxTrickCount(), dataPool);
					break;
				case 171:
					result = Serializer.Serialize(instance.GetMaxBreathPercent(), dataPool);
					break;
				case 172:
					result = Serializer.Serialize(instance.GetMaxStancePercent(), dataPool);
					break;
				case 173:
					result = Serializer.Serialize(instance.GetExtraBreathPercent(), dataPool);
					break;
				case 174:
					result = Serializer.Serialize(instance.GetExtraStancePercent(), dataPool);
					break;
				case 175:
					result = Serializer.Serialize(instance.GetMoveCostMobility(), dataPool);
					break;
				case 176:
					result = Serializer.Serialize(instance.GetDefendSkillKeepTime(), dataPool);
					break;
				case 177:
					result = Serializer.Serialize(instance.GetBounceRange(), dataPool);
					break;
				case 178:
					result = Serializer.Serialize(instance.GetMindMarkKeepTime(), dataPool);
					break;
				case 179:
					result = Serializer.Serialize(instance.GetSkillMobilityCostPerFrame(), dataPool);
					break;
				case 180:
					result = Serializer.Serialize(instance.GetCanAddWug(), dataPool);
					break;
				case 181:
					result = Serializer.Serialize(instance.GetHasGodWeaponBuff(), dataPool);
					break;
				case 182:
					result = Serializer.Serialize(instance.GetHasGodArmorBuff(), dataPool);
					break;
				case 183:
					result = Serializer.Serialize(instance.GetTeammateCmdRequireGenerateValue(), dataPool);
					break;
				case 184:
					result = Serializer.Serialize(instance.GetTeammateCmdEffect(), dataPool);
					break;
				case 185:
					result = Serializer.Serialize(instance.GetFlawRecoverSpeed(), dataPool);
					break;
				case 186:
					result = Serializer.Serialize(instance.GetAcupointRecoverSpeed(), dataPool);
					break;
				case 187:
					result = Serializer.Serialize(instance.GetMindMarkRecoverSpeed(), dataPool);
					break;
				case 188:
					result = Serializer.Serialize(instance.GetInjuryAutoHealSpeed(), dataPool);
					break;
				case 189:
					result = Serializer.Serialize(instance.GetCanRecoverBreath(), dataPool);
					break;
				case 190:
					result = Serializer.Serialize(instance.GetCanRecoverStance(), dataPool);
					break;
				case 191:
					result = Serializer.Serialize(instance.GetFatalDamageValue(), dataPool);
					break;
				case 192:
					result = Serializer.Serialize(instance.GetFatalDamageMarkCount(), dataPool);
					break;
				case 193:
					result = Serializer.Serialize(instance.GetCanFightBackDuringPrepareSkill(), dataPool);
					break;
				case 194:
					result = Serializer.Serialize(instance.GetSkillPrepareSpeed(), dataPool);
					break;
				case 195:
					result = Serializer.Serialize(instance.GetBreathRecoverSpeed(), dataPool);
					break;
				case 196:
					result = Serializer.Serialize(instance.GetStanceRecoverSpeed(), dataPool);
					break;
				case 197:
					result = Serializer.Serialize(instance.GetMobilityRecoverSpeed(), dataPool);
					break;
				case 198:
					result = Serializer.Serialize(instance.GetChangeTrickProgressAddValue(), dataPool);
					break;
				case 199:
					result = Serializer.Serialize(instance.GetPower(), dataPool);
					break;
				case 200:
					result = Serializer.Serialize(instance.GetMaxPower(), dataPool);
					break;
				case 201:
					result = Serializer.Serialize(instance.GetPowerCanReduce(), dataPool);
					break;
				case 202:
					result = Serializer.Serialize(instance.GetUseRequirement(), dataPool);
					break;
				case 203:
					result = Serializer.Serialize(instance.GetCurrInnerRatio(), dataPool);
					break;
				case 204:
					result = Serializer.Serialize(instance.GetCostBreathAndStance(), dataPool);
					break;
				case 205:
					result = Serializer.Serialize(instance.GetCostBreath(), dataPool);
					break;
				case 206:
					result = Serializer.Serialize(instance.GetCostStance(), dataPool);
					break;
				case 207:
					result = Serializer.Serialize(instance.GetCostMobility(), dataPool);
					break;
				case 208:
					result = Serializer.Serialize(instance.GetSkillCostTricks(), dataPool);
					break;
				case 209:
					result = Serializer.Serialize(instance.GetEffectDirection(), dataPool);
					break;
				case 210:
					result = Serializer.Serialize(instance.GetEffectDirectionCanChange(), dataPool);
					break;
				case 211:
					result = Serializer.Serialize(instance.GetGridCost(), dataPool);
					break;
				case 212:
					result = Serializer.Serialize(instance.GetPrepareTotalProgress(), dataPool);
					break;
				case 213:
					result = Serializer.Serialize(instance.GetSpecificGridCount(), dataPool);
					break;
				case 214:
					result = Serializer.Serialize(instance.GetGenericGridCount(), dataPool);
					break;
				case 215:
					result = Serializer.Serialize(instance.GetCanInterrupt(), dataPool);
					break;
				case 216:
					result = Serializer.Serialize(instance.GetInterruptOdds(), dataPool);
					break;
				case 217:
					result = Serializer.Serialize(instance.GetCanSilence(), dataPool);
					break;
				case 218:
					result = Serializer.Serialize(instance.GetSilenceOdds(), dataPool);
					break;
				case 219:
					result = Serializer.Serialize(instance.GetCanCastWithBrokenBodyPart(), dataPool);
					break;
				case 220:
					result = Serializer.Serialize(instance.GetAddPowerCanBeRemoved(), dataPool);
					break;
				case 221:
					result = Serializer.Serialize(instance.GetSkillType(), dataPool);
					break;
				case 222:
					result = Serializer.Serialize(instance.GetEffectCountCanChange(), dataPool);
					break;
				case 223:
					result = Serializer.Serialize(instance.GetCanCastInDefend(), dataPool);
					break;
				case 224:
					result = Serializer.Serialize(instance.GetHitDistribution(), dataPool);
					break;
				case 225:
					result = Serializer.Serialize(instance.GetCanCastOnLackBreath(), dataPool);
					break;
				case 226:
					result = Serializer.Serialize(instance.GetCanCastOnLackStance(), dataPool);
					break;
				case 227:
					result = Serializer.Serialize(instance.GetCostBreathOnCast(), dataPool);
					break;
				case 228:
					result = Serializer.Serialize(instance.GetCostStanceOnCast(), dataPool);
					break;
				case 229:
					result = Serializer.Serialize(instance.GetCanUseMobilityAsBreath(), dataPool);
					break;
				case 230:
					result = Serializer.Serialize(instance.GetCanUseMobilityAsStance(), dataPool);
					break;
				case 231:
					result = Serializer.Serialize(instance.GetCastCostNeiliAllocation(), dataPool);
					break;
				case 232:
					result = Serializer.Serialize(instance.GetAcceptPoisonResist(), dataPool);
					break;
				case 233:
					result = Serializer.Serialize(instance.GetMakePoisonResist(), dataPool);
					break;
				case 234:
					result = Serializer.Serialize(instance.GetCanCriticalHit(), dataPool);
					break;
				case 235:
					result = Serializer.Serialize(instance.GetCanCostNeiliAllocationEffect(), dataPool);
					break;
				case 236:
					result = Serializer.Serialize(instance.GetConsummateLevelRelatedMainAttributesHitValues(), dataPool);
					break;
				case 237:
					result = Serializer.Serialize(instance.GetConsummateLevelRelatedMainAttributesAvoidValues(), dataPool);
					break;
				case 238:
					result = Serializer.Serialize(instance.GetConsummateLevelRelatedMainAttributesPenetrations(), dataPool);
					break;
				case 239:
					result = Serializer.Serialize(instance.GetConsummateLevelRelatedMainAttributesPenetrationResists(), dataPool);
					break;
				case 240:
					result = Serializer.Serialize(instance.GetSkillAlsoAsFiveElements(), dataPool);
					break;
				case 241:
					result = Serializer.Serialize(instance.GetInnerInjuryImmunity(), dataPool);
					break;
				case 242:
					result = Serializer.Serialize(instance.GetOuterInjuryImmunity(), dataPool);
					break;
				case 243:
					result = Serializer.Serialize(instance.GetPoisonAffectThreshold(), dataPool);
					break;
				case 244:
					result = Serializer.Serialize(instance.GetLockDistance(), dataPool);
					break;
				case 245:
					result = Serializer.Serialize(instance.GetResistOfAllPoison(), dataPool);
					break;
				case 246:
					result = Serializer.Serialize(instance.GetMakePoisonTarget(), dataPool);
					break;
				case 247:
					result = Serializer.Serialize(instance.GetAcceptPoisonTarget(), dataPool);
					break;
				case 248:
					result = Serializer.Serialize(instance.GetCertainCriticalHit(), dataPool);
					break;
				case 249:
					result = Serializer.Serialize(instance.GetMindMarkCount(), dataPool);
					break;
				case 250:
					result = Serializer.Serialize(instance.GetCanFightBackWithHit(), dataPool);
					break;
				case 251:
					result = Serializer.Serialize(instance.GetInevitableHit(), dataPool);
					break;
				case 252:
					result = Serializer.Serialize(instance.GetAttackCanPursue(), dataPool);
					break;
				case 253:
					result = Serializer.Serialize(instance.GetCombatSkillDataEffectList(), dataPool);
					break;
				case 254:
					result = Serializer.Serialize(instance.GetCriticalOdds(), dataPool);
					break;
				case 255:
					result = Serializer.Serialize(instance.GetStanceCostByEffect(), dataPool);
					break;
				case 256:
					result = Serializer.Serialize(instance.GetBreathCostByEffect(), dataPool);
					break;
				case 257:
					result = Serializer.Serialize(instance.GetPowerAddRatio(), dataPool);
					break;
				case 258:
					result = Serializer.Serialize(instance.GetPowerReduceRatio(), dataPool);
					break;
				case 259:
					result = Serializer.Serialize(instance.GetPoisonAffectProduceValue(), dataPool);
					break;
				case 260:
					result = Serializer.Serialize(instance.GetCanReadingOnMonthChange(), dataPool);
					break;
				case 261:
					result = Serializer.Serialize(instance.GetMedicineEffect(), dataPool);
					break;
				case 262:
					result = Serializer.Serialize(instance.GetXiangshuInfectionDelta(), dataPool);
					break;
				case 263:
					result = Serializer.Serialize(instance.GetHealthDelta(), dataPool);
					break;
				case 264:
					result = Serializer.Serialize(instance.GetWeaponSilenceFrame(), dataPool);
					break;
				case 265:
					result = Serializer.Serialize(instance.GetSilenceFrame(), dataPool);
					break;
				case 266:
					result = Serializer.Serialize(instance.GetCurrAgeDelta(), dataPool);
					break;
				case 267:
					result = Serializer.Serialize(instance.GetGoneMadInAllBreak(), dataPool);
					break;
				case 268:
					result = Serializer.Serialize(instance.GetMakeLoveRateOnMonthChange(), dataPool);
					break;
				case 269:
					result = Serializer.Serialize(instance.GetCanAutoHealOnMonthChange(), dataPool);
					break;
				case 270:
					result = Serializer.Serialize(instance.GetHappinessDelta(), dataPool);
					break;
				case 271:
					result = Serializer.Serialize(instance.GetTeammateCmdCanUse(), dataPool);
					break;
				case 272:
					result = Serializer.Serialize(instance.GetMixPoisonInfinityAffect(), dataPool);
					break;
				case 273:
					result = Serializer.Serialize(instance.GetAttackRangeMaxAcupoint(), dataPool);
					break;
				case 274:
					result = Serializer.Serialize(instance.GetMaxMobilityPercent(), dataPool);
					break;
				case 275:
					result = Serializer.Serialize(instance.GetMakeMindDamage(), dataPool);
					break;
				case 276:
					result = Serializer.Serialize(instance.GetAcceptMindDamage(), dataPool);
					break;
				case 277:
					result = Serializer.Serialize(instance.GetHitAddByTempValue(), dataPool);
					break;
				case 278:
					result = Serializer.Serialize(instance.GetAvoidAddByTempValue(), dataPool);
					break;
				case 279:
					result = Serializer.Serialize(instance.GetIgnoreEquipmentOverload(), dataPool);
					break;
				case 280:
					result = Serializer.Serialize(instance.GetCanCostEnemyUsableTricks(), dataPool);
					break;
				case 281:
					result = Serializer.Serialize(instance.GetIgnoreArmor(), dataPool);
					break;
				case 282:
					result = Serializer.Serialize(instance.GetUnyieldingFallen(), dataPool);
					break;
				case 283:
					result = Serializer.Serialize(instance.GetNormalAttackPrepareFrame(), dataPool);
					break;
				case 284:
					result = Serializer.Serialize(instance.GetCanCostUselessTricks(), dataPool);
					break;
				case 285:
					result = Serializer.Serialize(instance.GetDefendSkillCanAffect(), dataPool);
					break;
				case 286:
					result = Serializer.Serialize(instance.GetAssistSkillCanAffect(), dataPool);
					break;
				case 287:
					result = Serializer.Serialize(instance.GetAgileSkillCanAffect(), dataPool);
					break;
				case 288:
					result = Serializer.Serialize(instance.GetAllMarkChangeToMind(), dataPool);
					break;
				case 289:
					result = Serializer.Serialize(instance.GetMindMarkChangeToFatal(), dataPool);
					break;
				case 290:
					result = Serializer.Serialize(instance.GetCanCast(), dataPool);
					break;
				case 291:
					result = Serializer.Serialize(instance.GetInevitableAvoid(), dataPool);
					break;
				case 292:
					result = Serializer.Serialize(instance.GetPowerEffectReverse(), dataPool);
					break;
				case 293:
					result = Serializer.Serialize(instance.GetFeatureBonusReverse(), dataPool);
					break;
				case 294:
					result = Serializer.Serialize(instance.GetWugFatalDamageValue(), dataPool);
					break;
				case 295:
					result = Serializer.Serialize(instance.GetCanRecoverHealthOnMonthChange(), dataPool);
					break;
				case 296:
					result = Serializer.Serialize(instance.GetTakeRevengeRateOnMonthChange(), dataPool);
					break;
				case 297:
					result = Serializer.Serialize(instance.GetConsummateLevelBonus(), dataPool);
					break;
				case 298:
					result = Serializer.Serialize(instance.GetNeiliDelta(), dataPool);
					break;
				case 299:
					result = Serializer.Serialize(instance.GetCanMakeLoveSpecialOnMonthChange(), dataPool);
					break;
				case 300:
					result = Serializer.Serialize(instance.GetHealAcupointSpeed(), dataPool);
					break;
				case 301:
					result = Serializer.Serialize(instance.GetMaxChangeTrickCount(), dataPool);
					break;
				case 302:
					result = Serializer.Serialize(instance.GetConvertCostBreathAndStance(), dataPool);
					break;
				case 303:
					result = Serializer.Serialize(instance.GetPersonalitiesAll(), dataPool);
					break;
				case 304:
					result = Serializer.Serialize(instance.GetFinalFatalDamageMarkCount(), dataPool);
					break;
				case 305:
					result = Serializer.Serialize(instance.GetInfinityMindMarkProgress(), dataPool);
					break;
				case 306:
					result = Serializer.Serialize(instance.GetCombatSkillAiScorePower(), dataPool);
					break;
				case 307:
					result = Serializer.Serialize(instance.GetNormalAttackChangeToUnlockAttack(), dataPool);
					break;
				case 308:
					result = Serializer.Serialize(instance.GetAttackBodyPartOdds(), dataPool);
					break;
				case 309:
					result = Serializer.Serialize(instance.GetChangeDurability(), dataPool);
					break;
				case 310:
					result = Serializer.Serialize(instance.GetEquipmentBonus(), dataPool);
					break;
				case 311:
					result = Serializer.Serialize(instance.GetEquipmentWeight(), dataPool);
					break;
				case 312:
					result = Serializer.Serialize(instance.GetRawCreateEffectList(), dataPool);
					break;
				case 313:
					result = Serializer.Serialize(instance.GetJiTrickAsWeaponTrickCount(), dataPool);
					break;
				case 314:
					result = Serializer.Serialize(instance.GetUselessTrickAsJiTrickCount(), dataPool);
					break;
				case 315:
					result = Serializer.Serialize(instance.GetEquipmentPower(), dataPool);
					break;
				case 316:
					result = Serializer.Serialize(instance.GetHealFlawSpeed(), dataPool);
					break;
				case 317:
					result = Serializer.Serialize(instance.GetUnlockSpeed(), dataPool);
					break;
				case 318:
					result = Serializer.Serialize(instance.GetFlawBonusFactor(), dataPool);
					break;
				case 319:
					result = Serializer.Serialize(instance.GetCanCostShaTricks(), dataPool);
					break;
				case 320:
					result = Serializer.Serialize(instance.GetDefenderDirectFinalDamageValue(), dataPool);
					break;
				case 321:
					result = Serializer.Serialize(instance.GetNormalAttackRecoveryFrame(), dataPool);
					break;
				case 322:
					result = Serializer.Serialize(instance.GetFinalGoneMadInjury(), dataPool);
					break;
				case 323:
					result = Serializer.Serialize(instance.GetAttackerDirectFinalDamageValue(), dataPool);
					break;
				case 324:
					result = Serializer.Serialize(instance.GetCanCostTrickDuringPreparingSkill(), dataPool);
					break;
				case 325:
					result = Serializer.Serialize(instance.GetValidItemList(), dataPool);
					break;
				case 326:
					result = Serializer.Serialize(instance.GetAcceptDamageCanAdd(), dataPool);
					break;
				case 327:
					result = Serializer.Serialize(instance.GetMakeDamageCanReduce(), dataPool);
					break;
				case 328:
					result = Serializer.Serialize(instance.GetNormalAttackGetTrickCount(), dataPool);
					break;
				default:
				{
					bool flag2 = fieldId >= 329;
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

		// Token: 0x0600293B RID: 10555 RVA: 0x001F51E4 File Offset: 0x001F33E4
		private void SetElementField_AffectedDatas(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			AffectedData instance;
			bool flag = !this._affectedDatas.TryGetValue(objectId, out instance);
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
				int value = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value);
				instance.SetId(value, context);
				break;
			}
			case 1:
			{
				SpecialEffectList value2 = instance.GetMaxStrength();
				Serializer.Deserialize(dataPool, valueOffset, ref value2);
				instance.SetMaxStrength(value2, context);
				break;
			}
			case 2:
			{
				SpecialEffectList value3 = instance.GetMaxDexterity();
				Serializer.Deserialize(dataPool, valueOffset, ref value3);
				instance.SetMaxDexterity(value3, context);
				break;
			}
			case 3:
			{
				SpecialEffectList value4 = instance.GetMaxConcentration();
				Serializer.Deserialize(dataPool, valueOffset, ref value4);
				instance.SetMaxConcentration(value4, context);
				break;
			}
			case 4:
			{
				SpecialEffectList value5 = instance.GetMaxVitality();
				Serializer.Deserialize(dataPool, valueOffset, ref value5);
				instance.SetMaxVitality(value5, context);
				break;
			}
			case 5:
			{
				SpecialEffectList value6 = instance.GetMaxEnergy();
				Serializer.Deserialize(dataPool, valueOffset, ref value6);
				instance.SetMaxEnergy(value6, context);
				break;
			}
			case 6:
			{
				SpecialEffectList value7 = instance.GetMaxIntelligence();
				Serializer.Deserialize(dataPool, valueOffset, ref value7);
				instance.SetMaxIntelligence(value7, context);
				break;
			}
			case 7:
			{
				SpecialEffectList value8 = instance.GetRecoveryOfStance();
				Serializer.Deserialize(dataPool, valueOffset, ref value8);
				instance.SetRecoveryOfStance(value8, context);
				break;
			}
			case 8:
			{
				SpecialEffectList value9 = instance.GetRecoveryOfBreath();
				Serializer.Deserialize(dataPool, valueOffset, ref value9);
				instance.SetRecoveryOfBreath(value9, context);
				break;
			}
			case 9:
			{
				SpecialEffectList value10 = instance.GetMoveSpeed();
				Serializer.Deserialize(dataPool, valueOffset, ref value10);
				instance.SetMoveSpeed(value10, context);
				break;
			}
			case 10:
			{
				SpecialEffectList value11 = instance.GetRecoveryOfFlaw();
				Serializer.Deserialize(dataPool, valueOffset, ref value11);
				instance.SetRecoveryOfFlaw(value11, context);
				break;
			}
			case 11:
			{
				SpecialEffectList value12 = instance.GetCastSpeed();
				Serializer.Deserialize(dataPool, valueOffset, ref value12);
				instance.SetCastSpeed(value12, context);
				break;
			}
			case 12:
			{
				SpecialEffectList value13 = instance.GetRecoveryOfBlockedAcupoint();
				Serializer.Deserialize(dataPool, valueOffset, ref value13);
				instance.SetRecoveryOfBlockedAcupoint(value13, context);
				break;
			}
			case 13:
			{
				SpecialEffectList value14 = instance.GetWeaponSwitchSpeed();
				Serializer.Deserialize(dataPool, valueOffset, ref value14);
				instance.SetWeaponSwitchSpeed(value14, context);
				break;
			}
			case 14:
			{
				SpecialEffectList value15 = instance.GetAttackSpeed();
				Serializer.Deserialize(dataPool, valueOffset, ref value15);
				instance.SetAttackSpeed(value15, context);
				break;
			}
			case 15:
			{
				SpecialEffectList value16 = instance.GetInnerRatio();
				Serializer.Deserialize(dataPool, valueOffset, ref value16);
				instance.SetInnerRatio(value16, context);
				break;
			}
			case 16:
			{
				SpecialEffectList value17 = instance.GetRecoveryOfQiDisorder();
				Serializer.Deserialize(dataPool, valueOffset, ref value17);
				instance.SetRecoveryOfQiDisorder(value17, context);
				break;
			}
			case 17:
			{
				SpecialEffectList value18 = instance.GetMinorAttributeFixMaxValue();
				Serializer.Deserialize(dataPool, valueOffset, ref value18);
				instance.SetMinorAttributeFixMaxValue(value18, context);
				break;
			}
			case 18:
			{
				SpecialEffectList value19 = instance.GetMinorAttributeFixMinValue();
				Serializer.Deserialize(dataPool, valueOffset, ref value19);
				instance.SetMinorAttributeFixMinValue(value19, context);
				break;
			}
			case 19:
			{
				SpecialEffectList value20 = instance.GetResistOfHotPoison();
				Serializer.Deserialize(dataPool, valueOffset, ref value20);
				instance.SetResistOfHotPoison(value20, context);
				break;
			}
			case 20:
			{
				SpecialEffectList value21 = instance.GetResistOfGloomyPoison();
				Serializer.Deserialize(dataPool, valueOffset, ref value21);
				instance.SetResistOfGloomyPoison(value21, context);
				break;
			}
			case 21:
			{
				SpecialEffectList value22 = instance.GetResistOfColdPoison();
				Serializer.Deserialize(dataPool, valueOffset, ref value22);
				instance.SetResistOfColdPoison(value22, context);
				break;
			}
			case 22:
			{
				SpecialEffectList value23 = instance.GetResistOfRedPoison();
				Serializer.Deserialize(dataPool, valueOffset, ref value23);
				instance.SetResistOfRedPoison(value23, context);
				break;
			}
			case 23:
			{
				SpecialEffectList value24 = instance.GetResistOfRottenPoison();
				Serializer.Deserialize(dataPool, valueOffset, ref value24);
				instance.SetResistOfRottenPoison(value24, context);
				break;
			}
			case 24:
			{
				SpecialEffectList value25 = instance.GetResistOfIllusoryPoison();
				Serializer.Deserialize(dataPool, valueOffset, ref value25);
				instance.SetResistOfIllusoryPoison(value25, context);
				break;
			}
			case 25:
			{
				SpecialEffectList value26 = instance.GetDisplayAge();
				Serializer.Deserialize(dataPool, valueOffset, ref value26);
				instance.SetDisplayAge(value26, context);
				break;
			}
			case 26:
			{
				SpecialEffectList value27 = instance.GetNeiliProportionOfFiveElements();
				Serializer.Deserialize(dataPool, valueOffset, ref value27);
				instance.SetNeiliProportionOfFiveElements(value27, context);
				break;
			}
			case 27:
			{
				SpecialEffectList value28 = instance.GetWeaponMaxPower();
				Serializer.Deserialize(dataPool, valueOffset, ref value28);
				instance.SetWeaponMaxPower(value28, context);
				break;
			}
			case 28:
			{
				SpecialEffectList value29 = instance.GetWeaponUseRequirement();
				Serializer.Deserialize(dataPool, valueOffset, ref value29);
				instance.SetWeaponUseRequirement(value29, context);
				break;
			}
			case 29:
			{
				SpecialEffectList value30 = instance.GetWeaponAttackRange();
				Serializer.Deserialize(dataPool, valueOffset, ref value30);
				instance.SetWeaponAttackRange(value30, context);
				break;
			}
			case 30:
			{
				SpecialEffectList value31 = instance.GetArmorMaxPower();
				Serializer.Deserialize(dataPool, valueOffset, ref value31);
				instance.SetArmorMaxPower(value31, context);
				break;
			}
			case 31:
			{
				SpecialEffectList value32 = instance.GetArmorUseRequirement();
				Serializer.Deserialize(dataPool, valueOffset, ref value32);
				instance.SetArmorUseRequirement(value32, context);
				break;
			}
			case 32:
			{
				SpecialEffectList value33 = instance.GetHitStrength();
				Serializer.Deserialize(dataPool, valueOffset, ref value33);
				instance.SetHitStrength(value33, context);
				break;
			}
			case 33:
			{
				SpecialEffectList value34 = instance.GetHitTechnique();
				Serializer.Deserialize(dataPool, valueOffset, ref value34);
				instance.SetHitTechnique(value34, context);
				break;
			}
			case 34:
			{
				SpecialEffectList value35 = instance.GetHitSpeed();
				Serializer.Deserialize(dataPool, valueOffset, ref value35);
				instance.SetHitSpeed(value35, context);
				break;
			}
			case 35:
			{
				SpecialEffectList value36 = instance.GetHitMind();
				Serializer.Deserialize(dataPool, valueOffset, ref value36);
				instance.SetHitMind(value36, context);
				break;
			}
			case 36:
			{
				SpecialEffectList value37 = instance.GetHitCanChange();
				Serializer.Deserialize(dataPool, valueOffset, ref value37);
				instance.SetHitCanChange(value37, context);
				break;
			}
			case 37:
			{
				SpecialEffectList value38 = instance.GetHitChangeEffectPercent();
				Serializer.Deserialize(dataPool, valueOffset, ref value38);
				instance.SetHitChangeEffectPercent(value38, context);
				break;
			}
			case 38:
			{
				SpecialEffectList value39 = instance.GetAvoidStrength();
				Serializer.Deserialize(dataPool, valueOffset, ref value39);
				instance.SetAvoidStrength(value39, context);
				break;
			}
			case 39:
			{
				SpecialEffectList value40 = instance.GetAvoidTechnique();
				Serializer.Deserialize(dataPool, valueOffset, ref value40);
				instance.SetAvoidTechnique(value40, context);
				break;
			}
			case 40:
			{
				SpecialEffectList value41 = instance.GetAvoidSpeed();
				Serializer.Deserialize(dataPool, valueOffset, ref value41);
				instance.SetAvoidSpeed(value41, context);
				break;
			}
			case 41:
			{
				SpecialEffectList value42 = instance.GetAvoidMind();
				Serializer.Deserialize(dataPool, valueOffset, ref value42);
				instance.SetAvoidMind(value42, context);
				break;
			}
			case 42:
			{
				SpecialEffectList value43 = instance.GetAvoidCanChange();
				Serializer.Deserialize(dataPool, valueOffset, ref value43);
				instance.SetAvoidCanChange(value43, context);
				break;
			}
			case 43:
			{
				SpecialEffectList value44 = instance.GetAvoidChangeEffectPercent();
				Serializer.Deserialize(dataPool, valueOffset, ref value44);
				instance.SetAvoidChangeEffectPercent(value44, context);
				break;
			}
			case 44:
			{
				SpecialEffectList value45 = instance.GetPenetrateOuter();
				Serializer.Deserialize(dataPool, valueOffset, ref value45);
				instance.SetPenetrateOuter(value45, context);
				break;
			}
			case 45:
			{
				SpecialEffectList value46 = instance.GetPenetrateInner();
				Serializer.Deserialize(dataPool, valueOffset, ref value46);
				instance.SetPenetrateInner(value46, context);
				break;
			}
			case 46:
			{
				SpecialEffectList value47 = instance.GetPenetrateResistOuter();
				Serializer.Deserialize(dataPool, valueOffset, ref value47);
				instance.SetPenetrateResistOuter(value47, context);
				break;
			}
			case 47:
			{
				SpecialEffectList value48 = instance.GetPenetrateResistInner();
				Serializer.Deserialize(dataPool, valueOffset, ref value48);
				instance.SetPenetrateResistInner(value48, context);
				break;
			}
			case 48:
			{
				SpecialEffectList value49 = instance.GetNeiliAllocationAttack();
				Serializer.Deserialize(dataPool, valueOffset, ref value49);
				instance.SetNeiliAllocationAttack(value49, context);
				break;
			}
			case 49:
			{
				SpecialEffectList value50 = instance.GetNeiliAllocationAgile();
				Serializer.Deserialize(dataPool, valueOffset, ref value50);
				instance.SetNeiliAllocationAgile(value50, context);
				break;
			}
			case 50:
			{
				SpecialEffectList value51 = instance.GetNeiliAllocationDefense();
				Serializer.Deserialize(dataPool, valueOffset, ref value51);
				instance.SetNeiliAllocationDefense(value51, context);
				break;
			}
			case 51:
			{
				SpecialEffectList value52 = instance.GetNeiliAllocationAssist();
				Serializer.Deserialize(dataPool, valueOffset, ref value52);
				instance.SetNeiliAllocationAssist(value52, context);
				break;
			}
			case 52:
			{
				SpecialEffectList value53 = instance.GetHappiness();
				Serializer.Deserialize(dataPool, valueOffset, ref value53);
				instance.SetHappiness(value53, context);
				break;
			}
			case 53:
			{
				SpecialEffectList value54 = instance.GetMaxHealth();
				Serializer.Deserialize(dataPool, valueOffset, ref value54);
				instance.SetMaxHealth(value54, context);
				break;
			}
			case 54:
			{
				SpecialEffectList value55 = instance.GetHealthCost();
				Serializer.Deserialize(dataPool, valueOffset, ref value55);
				instance.SetHealthCost(value55, context);
				break;
			}
			case 55:
			{
				SpecialEffectList value56 = instance.GetMoveSpeedCanChange();
				Serializer.Deserialize(dataPool, valueOffset, ref value56);
				instance.SetMoveSpeedCanChange(value56, context);
				break;
			}
			case 56:
			{
				SpecialEffectList value57 = instance.GetAttackerHitStrength();
				Serializer.Deserialize(dataPool, valueOffset, ref value57);
				instance.SetAttackerHitStrength(value57, context);
				break;
			}
			case 57:
			{
				SpecialEffectList value58 = instance.GetAttackerHitTechnique();
				Serializer.Deserialize(dataPool, valueOffset, ref value58);
				instance.SetAttackerHitTechnique(value58, context);
				break;
			}
			case 58:
			{
				SpecialEffectList value59 = instance.GetAttackerHitSpeed();
				Serializer.Deserialize(dataPool, valueOffset, ref value59);
				instance.SetAttackerHitSpeed(value59, context);
				break;
			}
			case 59:
			{
				SpecialEffectList value60 = instance.GetAttackerHitMind();
				Serializer.Deserialize(dataPool, valueOffset, ref value60);
				instance.SetAttackerHitMind(value60, context);
				break;
			}
			case 60:
			{
				SpecialEffectList value61 = instance.GetAttackerAvoidStrength();
				Serializer.Deserialize(dataPool, valueOffset, ref value61);
				instance.SetAttackerAvoidStrength(value61, context);
				break;
			}
			case 61:
			{
				SpecialEffectList value62 = instance.GetAttackerAvoidTechnique();
				Serializer.Deserialize(dataPool, valueOffset, ref value62);
				instance.SetAttackerAvoidTechnique(value62, context);
				break;
			}
			case 62:
			{
				SpecialEffectList value63 = instance.GetAttackerAvoidSpeed();
				Serializer.Deserialize(dataPool, valueOffset, ref value63);
				instance.SetAttackerAvoidSpeed(value63, context);
				break;
			}
			case 63:
			{
				SpecialEffectList value64 = instance.GetAttackerAvoidMind();
				Serializer.Deserialize(dataPool, valueOffset, ref value64);
				instance.SetAttackerAvoidMind(value64, context);
				break;
			}
			case 64:
			{
				SpecialEffectList value65 = instance.GetAttackerPenetrateOuter();
				Serializer.Deserialize(dataPool, valueOffset, ref value65);
				instance.SetAttackerPenetrateOuter(value65, context);
				break;
			}
			case 65:
			{
				SpecialEffectList value66 = instance.GetAttackerPenetrateInner();
				Serializer.Deserialize(dataPool, valueOffset, ref value66);
				instance.SetAttackerPenetrateInner(value66, context);
				break;
			}
			case 66:
			{
				SpecialEffectList value67 = instance.GetAttackerPenetrateResistOuter();
				Serializer.Deserialize(dataPool, valueOffset, ref value67);
				instance.SetAttackerPenetrateResistOuter(value67, context);
				break;
			}
			case 67:
			{
				SpecialEffectList value68 = instance.GetAttackerPenetrateResistInner();
				Serializer.Deserialize(dataPool, valueOffset, ref value68);
				instance.SetAttackerPenetrateResistInner(value68, context);
				break;
			}
			case 68:
			{
				SpecialEffectList value69 = instance.GetAttackHitType();
				Serializer.Deserialize(dataPool, valueOffset, ref value69);
				instance.SetAttackHitType(value69, context);
				break;
			}
			case 69:
			{
				SpecialEffectList value70 = instance.GetMakeDirectDamage();
				Serializer.Deserialize(dataPool, valueOffset, ref value70);
				instance.SetMakeDirectDamage(value70, context);
				break;
			}
			case 70:
			{
				SpecialEffectList value71 = instance.GetMakeBounceDamage();
				Serializer.Deserialize(dataPool, valueOffset, ref value71);
				instance.SetMakeBounceDamage(value71, context);
				break;
			}
			case 71:
			{
				SpecialEffectList value72 = instance.GetMakeFightBackDamage();
				Serializer.Deserialize(dataPool, valueOffset, ref value72);
				instance.SetMakeFightBackDamage(value72, context);
				break;
			}
			case 72:
			{
				SpecialEffectList value73 = instance.GetMakePoisonLevel();
				Serializer.Deserialize(dataPool, valueOffset, ref value73);
				instance.SetMakePoisonLevel(value73, context);
				break;
			}
			case 73:
			{
				SpecialEffectList value74 = instance.GetMakePoisonValue();
				Serializer.Deserialize(dataPool, valueOffset, ref value74);
				instance.SetMakePoisonValue(value74, context);
				break;
			}
			case 74:
			{
				SpecialEffectList value75 = instance.GetAttackerHitOdds();
				Serializer.Deserialize(dataPool, valueOffset, ref value75);
				instance.SetAttackerHitOdds(value75, context);
				break;
			}
			case 75:
			{
				SpecialEffectList value76 = instance.GetAttackerFightBackHitOdds();
				Serializer.Deserialize(dataPool, valueOffset, ref value76);
				instance.SetAttackerFightBackHitOdds(value76, context);
				break;
			}
			case 76:
			{
				SpecialEffectList value77 = instance.GetAttackerPursueOdds();
				Serializer.Deserialize(dataPool, valueOffset, ref value77);
				instance.SetAttackerPursueOdds(value77, context);
				break;
			}
			case 77:
			{
				SpecialEffectList value78 = instance.GetMakedInjuryChangeToOld();
				Serializer.Deserialize(dataPool, valueOffset, ref value78);
				instance.SetMakedInjuryChangeToOld(value78, context);
				break;
			}
			case 78:
			{
				SpecialEffectList value79 = instance.GetMakedPoisonChangeToOld();
				Serializer.Deserialize(dataPool, valueOffset, ref value79);
				instance.SetMakedPoisonChangeToOld(value79, context);
				break;
			}
			case 79:
			{
				SpecialEffectList value80 = instance.GetMakeDamageType();
				Serializer.Deserialize(dataPool, valueOffset, ref value80);
				instance.SetMakeDamageType(value80, context);
				break;
			}
			case 80:
			{
				SpecialEffectList value81 = instance.GetCanMakeInjuryToNoInjuryPart();
				Serializer.Deserialize(dataPool, valueOffset, ref value81);
				instance.SetCanMakeInjuryToNoInjuryPart(value81, context);
				break;
			}
			case 81:
			{
				SpecialEffectList value82 = instance.GetMakePoisonType();
				Serializer.Deserialize(dataPool, valueOffset, ref value82);
				instance.SetMakePoisonType(value82, context);
				break;
			}
			case 82:
			{
				SpecialEffectList value83 = instance.GetNormalAttackWeapon();
				Serializer.Deserialize(dataPool, valueOffset, ref value83);
				instance.SetNormalAttackWeapon(value83, context);
				break;
			}
			case 83:
			{
				SpecialEffectList value84 = instance.GetNormalAttackTrick();
				Serializer.Deserialize(dataPool, valueOffset, ref value84);
				instance.SetNormalAttackTrick(value84, context);
				break;
			}
			case 84:
			{
				SpecialEffectList value85 = instance.GetExtraFlawCount();
				Serializer.Deserialize(dataPool, valueOffset, ref value85);
				instance.SetExtraFlawCount(value85, context);
				break;
			}
			case 85:
			{
				SpecialEffectList value86 = instance.GetAttackCanBounce();
				Serializer.Deserialize(dataPool, valueOffset, ref value86);
				instance.SetAttackCanBounce(value86, context);
				break;
			}
			case 86:
			{
				SpecialEffectList value87 = instance.GetAttackCanFightBack();
				Serializer.Deserialize(dataPool, valueOffset, ref value87);
				instance.SetAttackCanFightBack(value87, context);
				break;
			}
			case 87:
			{
				SpecialEffectList value88 = instance.GetMakeFightBackInjuryMark();
				Serializer.Deserialize(dataPool, valueOffset, ref value88);
				instance.SetMakeFightBackInjuryMark(value88, context);
				break;
			}
			case 88:
			{
				SpecialEffectList value89 = instance.GetLegSkillUseShoes();
				Serializer.Deserialize(dataPool, valueOffset, ref value89);
				instance.SetLegSkillUseShoes(value89, context);
				break;
			}
			case 89:
			{
				SpecialEffectList value90 = instance.GetAttackerFinalDamageValue();
				Serializer.Deserialize(dataPool, valueOffset, ref value90);
				instance.SetAttackerFinalDamageValue(value90, context);
				break;
			}
			case 90:
			{
				SpecialEffectList value91 = instance.GetDefenderHitStrength();
				Serializer.Deserialize(dataPool, valueOffset, ref value91);
				instance.SetDefenderHitStrength(value91, context);
				break;
			}
			case 91:
			{
				SpecialEffectList value92 = instance.GetDefenderHitTechnique();
				Serializer.Deserialize(dataPool, valueOffset, ref value92);
				instance.SetDefenderHitTechnique(value92, context);
				break;
			}
			case 92:
			{
				SpecialEffectList value93 = instance.GetDefenderHitSpeed();
				Serializer.Deserialize(dataPool, valueOffset, ref value93);
				instance.SetDefenderHitSpeed(value93, context);
				break;
			}
			case 93:
			{
				SpecialEffectList value94 = instance.GetDefenderHitMind();
				Serializer.Deserialize(dataPool, valueOffset, ref value94);
				instance.SetDefenderHitMind(value94, context);
				break;
			}
			case 94:
			{
				SpecialEffectList value95 = instance.GetDefenderAvoidStrength();
				Serializer.Deserialize(dataPool, valueOffset, ref value95);
				instance.SetDefenderAvoidStrength(value95, context);
				break;
			}
			case 95:
			{
				SpecialEffectList value96 = instance.GetDefenderAvoidTechnique();
				Serializer.Deserialize(dataPool, valueOffset, ref value96);
				instance.SetDefenderAvoidTechnique(value96, context);
				break;
			}
			case 96:
			{
				SpecialEffectList value97 = instance.GetDefenderAvoidSpeed();
				Serializer.Deserialize(dataPool, valueOffset, ref value97);
				instance.SetDefenderAvoidSpeed(value97, context);
				break;
			}
			case 97:
			{
				SpecialEffectList value98 = instance.GetDefenderAvoidMind();
				Serializer.Deserialize(dataPool, valueOffset, ref value98);
				instance.SetDefenderAvoidMind(value98, context);
				break;
			}
			case 98:
			{
				SpecialEffectList value99 = instance.GetDefenderPenetrateOuter();
				Serializer.Deserialize(dataPool, valueOffset, ref value99);
				instance.SetDefenderPenetrateOuter(value99, context);
				break;
			}
			case 99:
			{
				SpecialEffectList value100 = instance.GetDefenderPenetrateInner();
				Serializer.Deserialize(dataPool, valueOffset, ref value100);
				instance.SetDefenderPenetrateInner(value100, context);
				break;
			}
			case 100:
			{
				SpecialEffectList value101 = instance.GetDefenderPenetrateResistOuter();
				Serializer.Deserialize(dataPool, valueOffset, ref value101);
				instance.SetDefenderPenetrateResistOuter(value101, context);
				break;
			}
			case 101:
			{
				SpecialEffectList value102 = instance.GetDefenderPenetrateResistInner();
				Serializer.Deserialize(dataPool, valueOffset, ref value102);
				instance.SetDefenderPenetrateResistInner(value102, context);
				break;
			}
			case 102:
			{
				SpecialEffectList value103 = instance.GetAcceptDirectDamage();
				Serializer.Deserialize(dataPool, valueOffset, ref value103);
				instance.SetAcceptDirectDamage(value103, context);
				break;
			}
			case 103:
			{
				SpecialEffectList value104 = instance.GetAcceptBounceDamage();
				Serializer.Deserialize(dataPool, valueOffset, ref value104);
				instance.SetAcceptBounceDamage(value104, context);
				break;
			}
			case 104:
			{
				SpecialEffectList value105 = instance.GetAcceptFightBackDamage();
				Serializer.Deserialize(dataPool, valueOffset, ref value105);
				instance.SetAcceptFightBackDamage(value105, context);
				break;
			}
			case 105:
			{
				SpecialEffectList value106 = instance.GetAcceptPoisonLevel();
				Serializer.Deserialize(dataPool, valueOffset, ref value106);
				instance.SetAcceptPoisonLevel(value106, context);
				break;
			}
			case 106:
			{
				SpecialEffectList value107 = instance.GetAcceptPoisonValue();
				Serializer.Deserialize(dataPool, valueOffset, ref value107);
				instance.SetAcceptPoisonValue(value107, context);
				break;
			}
			case 107:
			{
				SpecialEffectList value108 = instance.GetDefenderHitOdds();
				Serializer.Deserialize(dataPool, valueOffset, ref value108);
				instance.SetDefenderHitOdds(value108, context);
				break;
			}
			case 108:
			{
				SpecialEffectList value109 = instance.GetDefenderFightBackHitOdds();
				Serializer.Deserialize(dataPool, valueOffset, ref value109);
				instance.SetDefenderFightBackHitOdds(value109, context);
				break;
			}
			case 109:
			{
				SpecialEffectList value110 = instance.GetDefenderPursueOdds();
				Serializer.Deserialize(dataPool, valueOffset, ref value110);
				instance.SetDefenderPursueOdds(value110, context);
				break;
			}
			case 110:
			{
				SpecialEffectList value111 = instance.GetAcceptMaxInjuryCount();
				Serializer.Deserialize(dataPool, valueOffset, ref value111);
				instance.SetAcceptMaxInjuryCount(value111, context);
				break;
			}
			case 111:
			{
				SpecialEffectList value112 = instance.GetBouncePower();
				Serializer.Deserialize(dataPool, valueOffset, ref value112);
				instance.SetBouncePower(value112, context);
				break;
			}
			case 112:
			{
				SpecialEffectList value113 = instance.GetFightBackPower();
				Serializer.Deserialize(dataPool, valueOffset, ref value113);
				instance.SetFightBackPower(value113, context);
				break;
			}
			case 113:
			{
				SpecialEffectList value114 = instance.GetDirectDamageInnerRatio();
				Serializer.Deserialize(dataPool, valueOffset, ref value114);
				instance.SetDirectDamageInnerRatio(value114, context);
				break;
			}
			case 114:
			{
				SpecialEffectList value115 = instance.GetDefenderFinalDamageValue();
				Serializer.Deserialize(dataPool, valueOffset, ref value115);
				instance.SetDefenderFinalDamageValue(value115, context);
				break;
			}
			case 115:
			{
				SpecialEffectList value116 = instance.GetDirectDamageValue();
				Serializer.Deserialize(dataPool, valueOffset, ref value116);
				instance.SetDirectDamageValue(value116, context);
				break;
			}
			case 116:
			{
				SpecialEffectList value117 = instance.GetDirectInjuryMark();
				Serializer.Deserialize(dataPool, valueOffset, ref value117);
				instance.SetDirectInjuryMark(value117, context);
				break;
			}
			case 117:
			{
				SpecialEffectList value118 = instance.GetGoneMadInjury();
				Serializer.Deserialize(dataPool, valueOffset, ref value118);
				instance.SetGoneMadInjury(value118, context);
				break;
			}
			case 118:
			{
				SpecialEffectList value119 = instance.GetHealInjurySpeed();
				Serializer.Deserialize(dataPool, valueOffset, ref value119);
				instance.SetHealInjurySpeed(value119, context);
				break;
			}
			case 119:
			{
				SpecialEffectList value120 = instance.GetHealInjuryBuff();
				Serializer.Deserialize(dataPool, valueOffset, ref value120);
				instance.SetHealInjuryBuff(value120, context);
				break;
			}
			case 120:
			{
				SpecialEffectList value121 = instance.GetHealInjuryDebuff();
				Serializer.Deserialize(dataPool, valueOffset, ref value121);
				instance.SetHealInjuryDebuff(value121, context);
				break;
			}
			case 121:
			{
				SpecialEffectList value122 = instance.GetHealPoisonSpeed();
				Serializer.Deserialize(dataPool, valueOffset, ref value122);
				instance.SetHealPoisonSpeed(value122, context);
				break;
			}
			case 122:
			{
				SpecialEffectList value123 = instance.GetHealPoisonBuff();
				Serializer.Deserialize(dataPool, valueOffset, ref value123);
				instance.SetHealPoisonBuff(value123, context);
				break;
			}
			case 123:
			{
				SpecialEffectList value124 = instance.GetHealPoisonDebuff();
				Serializer.Deserialize(dataPool, valueOffset, ref value124);
				instance.SetHealPoisonDebuff(value124, context);
				break;
			}
			case 124:
			{
				SpecialEffectList value125 = instance.GetFleeSpeed();
				Serializer.Deserialize(dataPool, valueOffset, ref value125);
				instance.SetFleeSpeed(value125, context);
				break;
			}
			case 125:
			{
				SpecialEffectList value126 = instance.GetMaxFlawCount();
				Serializer.Deserialize(dataPool, valueOffset, ref value126);
				instance.SetMaxFlawCount(value126, context);
				break;
			}
			case 126:
			{
				SpecialEffectList value127 = instance.GetCanAddFlaw();
				Serializer.Deserialize(dataPool, valueOffset, ref value127);
				instance.SetCanAddFlaw(value127, context);
				break;
			}
			case 127:
			{
				SpecialEffectList value128 = instance.GetFlawLevel();
				Serializer.Deserialize(dataPool, valueOffset, ref value128);
				instance.SetFlawLevel(value128, context);
				break;
			}
			case 128:
			{
				SpecialEffectList value129 = instance.GetFlawLevelCanReduce();
				Serializer.Deserialize(dataPool, valueOffset, ref value129);
				instance.SetFlawLevelCanReduce(value129, context);
				break;
			}
			case 129:
			{
				SpecialEffectList value130 = instance.GetFlawCount();
				Serializer.Deserialize(dataPool, valueOffset, ref value130);
				instance.SetFlawCount(value130, context);
				break;
			}
			case 130:
			{
				SpecialEffectList value131 = instance.GetMaxAcupointCount();
				Serializer.Deserialize(dataPool, valueOffset, ref value131);
				instance.SetMaxAcupointCount(value131, context);
				break;
			}
			case 131:
			{
				SpecialEffectList value132 = instance.GetCanAddAcupoint();
				Serializer.Deserialize(dataPool, valueOffset, ref value132);
				instance.SetCanAddAcupoint(value132, context);
				break;
			}
			case 132:
			{
				SpecialEffectList value133 = instance.GetAcupointLevel();
				Serializer.Deserialize(dataPool, valueOffset, ref value133);
				instance.SetAcupointLevel(value133, context);
				break;
			}
			case 133:
			{
				SpecialEffectList value134 = instance.GetAcupointLevelCanReduce();
				Serializer.Deserialize(dataPool, valueOffset, ref value134);
				instance.SetAcupointLevelCanReduce(value134, context);
				break;
			}
			case 134:
			{
				SpecialEffectList value135 = instance.GetAcupointCount();
				Serializer.Deserialize(dataPool, valueOffset, ref value135);
				instance.SetAcupointCount(value135, context);
				break;
			}
			case 135:
			{
				SpecialEffectList value136 = instance.GetAddNeiliAllocation();
				Serializer.Deserialize(dataPool, valueOffset, ref value136);
				instance.SetAddNeiliAllocation(value136, context);
				break;
			}
			case 136:
			{
				SpecialEffectList value137 = instance.GetCostNeiliAllocation();
				Serializer.Deserialize(dataPool, valueOffset, ref value137);
				instance.SetCostNeiliAllocation(value137, context);
				break;
			}
			case 137:
			{
				SpecialEffectList value138 = instance.GetCanChangeNeiliAllocation();
				Serializer.Deserialize(dataPool, valueOffset, ref value138);
				instance.SetCanChangeNeiliAllocation(value138, context);
				break;
			}
			case 138:
			{
				SpecialEffectList value139 = instance.GetCanGetTrick();
				Serializer.Deserialize(dataPool, valueOffset, ref value139);
				instance.SetCanGetTrick(value139, context);
				break;
			}
			case 139:
			{
				SpecialEffectList value140 = instance.GetGetTrickType();
				Serializer.Deserialize(dataPool, valueOffset, ref value140);
				instance.SetGetTrickType(value140, context);
				break;
			}
			case 140:
			{
				SpecialEffectList value141 = instance.GetAttackBodyPart();
				Serializer.Deserialize(dataPool, valueOffset, ref value141);
				instance.SetAttackBodyPart(value141, context);
				break;
			}
			case 141:
			{
				SpecialEffectList value142 = instance.GetWeaponEquipAttack();
				Serializer.Deserialize(dataPool, valueOffset, ref value142);
				instance.SetWeaponEquipAttack(value142, context);
				break;
			}
			case 142:
			{
				SpecialEffectList value143 = instance.GetWeaponEquipDefense();
				Serializer.Deserialize(dataPool, valueOffset, ref value143);
				instance.SetWeaponEquipDefense(value143, context);
				break;
			}
			case 143:
			{
				SpecialEffectList value144 = instance.GetArmorEquipAttack();
				Serializer.Deserialize(dataPool, valueOffset, ref value144);
				instance.SetArmorEquipAttack(value144, context);
				break;
			}
			case 144:
			{
				SpecialEffectList value145 = instance.GetArmorEquipDefense();
				Serializer.Deserialize(dataPool, valueOffset, ref value145);
				instance.SetArmorEquipDefense(value145, context);
				break;
			}
			case 145:
			{
				SpecialEffectList value146 = instance.GetAttackRangeForward();
				Serializer.Deserialize(dataPool, valueOffset, ref value146);
				instance.SetAttackRangeForward(value146, context);
				break;
			}
			case 146:
			{
				SpecialEffectList value147 = instance.GetAttackRangeBackward();
				Serializer.Deserialize(dataPool, valueOffset, ref value147);
				instance.SetAttackRangeBackward(value147, context);
				break;
			}
			case 147:
			{
				SpecialEffectList value148 = instance.GetMoveCanBeStopped();
				Serializer.Deserialize(dataPool, valueOffset, ref value148);
				instance.SetMoveCanBeStopped(value148, context);
				break;
			}
			case 148:
			{
				SpecialEffectList value149 = instance.GetCanForcedMove();
				Serializer.Deserialize(dataPool, valueOffset, ref value149);
				instance.SetCanForcedMove(value149, context);
				break;
			}
			case 149:
			{
				SpecialEffectList value150 = instance.GetMobilityCanBeRemoved();
				Serializer.Deserialize(dataPool, valueOffset, ref value150);
				instance.SetMobilityCanBeRemoved(value150, context);
				break;
			}
			case 150:
			{
				SpecialEffectList value151 = instance.GetMobilityCostByEffect();
				Serializer.Deserialize(dataPool, valueOffset, ref value151);
				instance.SetMobilityCostByEffect(value151, context);
				break;
			}
			case 151:
			{
				SpecialEffectList value152 = instance.GetMoveDistance();
				Serializer.Deserialize(dataPool, valueOffset, ref value152);
				instance.SetMoveDistance(value152, context);
				break;
			}
			case 152:
			{
				SpecialEffectList value153 = instance.GetJumpPrepareFrame();
				Serializer.Deserialize(dataPool, valueOffset, ref value153);
				instance.SetJumpPrepareFrame(value153, context);
				break;
			}
			case 153:
			{
				SpecialEffectList value154 = instance.GetBounceInjuryMark();
				Serializer.Deserialize(dataPool, valueOffset, ref value154);
				instance.SetBounceInjuryMark(value154, context);
				break;
			}
			case 154:
			{
				SpecialEffectList value155 = instance.GetSkillHasCost();
				Serializer.Deserialize(dataPool, valueOffset, ref value155);
				instance.SetSkillHasCost(value155, context);
				break;
			}
			case 155:
			{
				SpecialEffectList value156 = instance.GetCombatStateEffect();
				Serializer.Deserialize(dataPool, valueOffset, ref value156);
				instance.SetCombatStateEffect(value156, context);
				break;
			}
			case 156:
			{
				SpecialEffectList value157 = instance.GetChangeNeedUseSkill();
				Serializer.Deserialize(dataPool, valueOffset, ref value157);
				instance.SetChangeNeedUseSkill(value157, context);
				break;
			}
			case 157:
			{
				SpecialEffectList value158 = instance.GetChangeDistanceIsMove();
				Serializer.Deserialize(dataPool, valueOffset, ref value158);
				instance.SetChangeDistanceIsMove(value158, context);
				break;
			}
			case 158:
			{
				SpecialEffectList value159 = instance.GetReplaceCharHit();
				Serializer.Deserialize(dataPool, valueOffset, ref value159);
				instance.SetReplaceCharHit(value159, context);
				break;
			}
			case 159:
			{
				SpecialEffectList value160 = instance.GetCanAddPoison();
				Serializer.Deserialize(dataPool, valueOffset, ref value160);
				instance.SetCanAddPoison(value160, context);
				break;
			}
			case 160:
			{
				SpecialEffectList value161 = instance.GetCanReducePoison();
				Serializer.Deserialize(dataPool, valueOffset, ref value161);
				instance.SetCanReducePoison(value161, context);
				break;
			}
			case 161:
			{
				SpecialEffectList value162 = instance.GetReducePoisonValue();
				Serializer.Deserialize(dataPool, valueOffset, ref value162);
				instance.SetReducePoisonValue(value162, context);
				break;
			}
			case 162:
			{
				SpecialEffectList value163 = instance.GetPoisonCanAffect();
				Serializer.Deserialize(dataPool, valueOffset, ref value163);
				instance.SetPoisonCanAffect(value163, context);
				break;
			}
			case 163:
			{
				SpecialEffectList value164 = instance.GetPoisonAffectCount();
				Serializer.Deserialize(dataPool, valueOffset, ref value164);
				instance.SetPoisonAffectCount(value164, context);
				break;
			}
			case 164:
			{
				SpecialEffectList value165 = instance.GetCostTricks();
				Serializer.Deserialize(dataPool, valueOffset, ref value165);
				instance.SetCostTricks(value165, context);
				break;
			}
			case 165:
			{
				SpecialEffectList value166 = instance.GetJumpMoveDistance();
				Serializer.Deserialize(dataPool, valueOffset, ref value166);
				instance.SetJumpMoveDistance(value166, context);
				break;
			}
			case 166:
			{
				SpecialEffectList value167 = instance.GetCombatStateToAdd();
				Serializer.Deserialize(dataPool, valueOffset, ref value167);
				instance.SetCombatStateToAdd(value167, context);
				break;
			}
			case 167:
			{
				SpecialEffectList value168 = instance.GetCombatStatePower();
				Serializer.Deserialize(dataPool, valueOffset, ref value168);
				instance.SetCombatStatePower(value168, context);
				break;
			}
			case 168:
			{
				SpecialEffectList value169 = instance.GetBreakBodyPartInjuryCount();
				Serializer.Deserialize(dataPool, valueOffset, ref value169);
				instance.SetBreakBodyPartInjuryCount(value169, context);
				break;
			}
			case 169:
			{
				SpecialEffectList value170 = instance.GetBodyPartIsBroken();
				Serializer.Deserialize(dataPool, valueOffset, ref value170);
				instance.SetBodyPartIsBroken(value170, context);
				break;
			}
			case 170:
			{
				SpecialEffectList value171 = instance.GetMaxTrickCount();
				Serializer.Deserialize(dataPool, valueOffset, ref value171);
				instance.SetMaxTrickCount(value171, context);
				break;
			}
			case 171:
			{
				SpecialEffectList value172 = instance.GetMaxBreathPercent();
				Serializer.Deserialize(dataPool, valueOffset, ref value172);
				instance.SetMaxBreathPercent(value172, context);
				break;
			}
			case 172:
			{
				SpecialEffectList value173 = instance.GetMaxStancePercent();
				Serializer.Deserialize(dataPool, valueOffset, ref value173);
				instance.SetMaxStancePercent(value173, context);
				break;
			}
			case 173:
			{
				SpecialEffectList value174 = instance.GetExtraBreathPercent();
				Serializer.Deserialize(dataPool, valueOffset, ref value174);
				instance.SetExtraBreathPercent(value174, context);
				break;
			}
			case 174:
			{
				SpecialEffectList value175 = instance.GetExtraStancePercent();
				Serializer.Deserialize(dataPool, valueOffset, ref value175);
				instance.SetExtraStancePercent(value175, context);
				break;
			}
			case 175:
			{
				SpecialEffectList value176 = instance.GetMoveCostMobility();
				Serializer.Deserialize(dataPool, valueOffset, ref value176);
				instance.SetMoveCostMobility(value176, context);
				break;
			}
			case 176:
			{
				SpecialEffectList value177 = instance.GetDefendSkillKeepTime();
				Serializer.Deserialize(dataPool, valueOffset, ref value177);
				instance.SetDefendSkillKeepTime(value177, context);
				break;
			}
			case 177:
			{
				SpecialEffectList value178 = instance.GetBounceRange();
				Serializer.Deserialize(dataPool, valueOffset, ref value178);
				instance.SetBounceRange(value178, context);
				break;
			}
			case 178:
			{
				SpecialEffectList value179 = instance.GetMindMarkKeepTime();
				Serializer.Deserialize(dataPool, valueOffset, ref value179);
				instance.SetMindMarkKeepTime(value179, context);
				break;
			}
			case 179:
			{
				SpecialEffectList value180 = instance.GetSkillMobilityCostPerFrame();
				Serializer.Deserialize(dataPool, valueOffset, ref value180);
				instance.SetSkillMobilityCostPerFrame(value180, context);
				break;
			}
			case 180:
			{
				SpecialEffectList value181 = instance.GetCanAddWug();
				Serializer.Deserialize(dataPool, valueOffset, ref value181);
				instance.SetCanAddWug(value181, context);
				break;
			}
			case 181:
			{
				SpecialEffectList value182 = instance.GetHasGodWeaponBuff();
				Serializer.Deserialize(dataPool, valueOffset, ref value182);
				instance.SetHasGodWeaponBuff(value182, context);
				break;
			}
			case 182:
			{
				SpecialEffectList value183 = instance.GetHasGodArmorBuff();
				Serializer.Deserialize(dataPool, valueOffset, ref value183);
				instance.SetHasGodArmorBuff(value183, context);
				break;
			}
			case 183:
			{
				SpecialEffectList value184 = instance.GetTeammateCmdRequireGenerateValue();
				Serializer.Deserialize(dataPool, valueOffset, ref value184);
				instance.SetTeammateCmdRequireGenerateValue(value184, context);
				break;
			}
			case 184:
			{
				SpecialEffectList value185 = instance.GetTeammateCmdEffect();
				Serializer.Deserialize(dataPool, valueOffset, ref value185);
				instance.SetTeammateCmdEffect(value185, context);
				break;
			}
			case 185:
			{
				SpecialEffectList value186 = instance.GetFlawRecoverSpeed();
				Serializer.Deserialize(dataPool, valueOffset, ref value186);
				instance.SetFlawRecoverSpeed(value186, context);
				break;
			}
			case 186:
			{
				SpecialEffectList value187 = instance.GetAcupointRecoverSpeed();
				Serializer.Deserialize(dataPool, valueOffset, ref value187);
				instance.SetAcupointRecoverSpeed(value187, context);
				break;
			}
			case 187:
			{
				SpecialEffectList value188 = instance.GetMindMarkRecoverSpeed();
				Serializer.Deserialize(dataPool, valueOffset, ref value188);
				instance.SetMindMarkRecoverSpeed(value188, context);
				break;
			}
			case 188:
			{
				SpecialEffectList value189 = instance.GetInjuryAutoHealSpeed();
				Serializer.Deserialize(dataPool, valueOffset, ref value189);
				instance.SetInjuryAutoHealSpeed(value189, context);
				break;
			}
			case 189:
			{
				SpecialEffectList value190 = instance.GetCanRecoverBreath();
				Serializer.Deserialize(dataPool, valueOffset, ref value190);
				instance.SetCanRecoverBreath(value190, context);
				break;
			}
			case 190:
			{
				SpecialEffectList value191 = instance.GetCanRecoverStance();
				Serializer.Deserialize(dataPool, valueOffset, ref value191);
				instance.SetCanRecoverStance(value191, context);
				break;
			}
			case 191:
			{
				SpecialEffectList value192 = instance.GetFatalDamageValue();
				Serializer.Deserialize(dataPool, valueOffset, ref value192);
				instance.SetFatalDamageValue(value192, context);
				break;
			}
			case 192:
			{
				SpecialEffectList value193 = instance.GetFatalDamageMarkCount();
				Serializer.Deserialize(dataPool, valueOffset, ref value193);
				instance.SetFatalDamageMarkCount(value193, context);
				break;
			}
			case 193:
			{
				SpecialEffectList value194 = instance.GetCanFightBackDuringPrepareSkill();
				Serializer.Deserialize(dataPool, valueOffset, ref value194);
				instance.SetCanFightBackDuringPrepareSkill(value194, context);
				break;
			}
			case 194:
			{
				SpecialEffectList value195 = instance.GetSkillPrepareSpeed();
				Serializer.Deserialize(dataPool, valueOffset, ref value195);
				instance.SetSkillPrepareSpeed(value195, context);
				break;
			}
			case 195:
			{
				SpecialEffectList value196 = instance.GetBreathRecoverSpeed();
				Serializer.Deserialize(dataPool, valueOffset, ref value196);
				instance.SetBreathRecoverSpeed(value196, context);
				break;
			}
			case 196:
			{
				SpecialEffectList value197 = instance.GetStanceRecoverSpeed();
				Serializer.Deserialize(dataPool, valueOffset, ref value197);
				instance.SetStanceRecoverSpeed(value197, context);
				break;
			}
			case 197:
			{
				SpecialEffectList value198 = instance.GetMobilityRecoverSpeed();
				Serializer.Deserialize(dataPool, valueOffset, ref value198);
				instance.SetMobilityRecoverSpeed(value198, context);
				break;
			}
			case 198:
			{
				SpecialEffectList value199 = instance.GetChangeTrickProgressAddValue();
				Serializer.Deserialize(dataPool, valueOffset, ref value199);
				instance.SetChangeTrickProgressAddValue(value199, context);
				break;
			}
			case 199:
			{
				SpecialEffectList value200 = instance.GetPower();
				Serializer.Deserialize(dataPool, valueOffset, ref value200);
				instance.SetPower(value200, context);
				break;
			}
			case 200:
			{
				SpecialEffectList value201 = instance.GetMaxPower();
				Serializer.Deserialize(dataPool, valueOffset, ref value201);
				instance.SetMaxPower(value201, context);
				break;
			}
			case 201:
			{
				SpecialEffectList value202 = instance.GetPowerCanReduce();
				Serializer.Deserialize(dataPool, valueOffset, ref value202);
				instance.SetPowerCanReduce(value202, context);
				break;
			}
			case 202:
			{
				SpecialEffectList value203 = instance.GetUseRequirement();
				Serializer.Deserialize(dataPool, valueOffset, ref value203);
				instance.SetUseRequirement(value203, context);
				break;
			}
			case 203:
			{
				SpecialEffectList value204 = instance.GetCurrInnerRatio();
				Serializer.Deserialize(dataPool, valueOffset, ref value204);
				instance.SetCurrInnerRatio(value204, context);
				break;
			}
			case 204:
			{
				SpecialEffectList value205 = instance.GetCostBreathAndStance();
				Serializer.Deserialize(dataPool, valueOffset, ref value205);
				instance.SetCostBreathAndStance(value205, context);
				break;
			}
			case 205:
			{
				SpecialEffectList value206 = instance.GetCostBreath();
				Serializer.Deserialize(dataPool, valueOffset, ref value206);
				instance.SetCostBreath(value206, context);
				break;
			}
			case 206:
			{
				SpecialEffectList value207 = instance.GetCostStance();
				Serializer.Deserialize(dataPool, valueOffset, ref value207);
				instance.SetCostStance(value207, context);
				break;
			}
			case 207:
			{
				SpecialEffectList value208 = instance.GetCostMobility();
				Serializer.Deserialize(dataPool, valueOffset, ref value208);
				instance.SetCostMobility(value208, context);
				break;
			}
			case 208:
			{
				SpecialEffectList value209 = instance.GetSkillCostTricks();
				Serializer.Deserialize(dataPool, valueOffset, ref value209);
				instance.SetSkillCostTricks(value209, context);
				break;
			}
			case 209:
			{
				SpecialEffectList value210 = instance.GetEffectDirection();
				Serializer.Deserialize(dataPool, valueOffset, ref value210);
				instance.SetEffectDirection(value210, context);
				break;
			}
			case 210:
			{
				SpecialEffectList value211 = instance.GetEffectDirectionCanChange();
				Serializer.Deserialize(dataPool, valueOffset, ref value211);
				instance.SetEffectDirectionCanChange(value211, context);
				break;
			}
			case 211:
			{
				SpecialEffectList value212 = instance.GetGridCost();
				Serializer.Deserialize(dataPool, valueOffset, ref value212);
				instance.SetGridCost(value212, context);
				break;
			}
			case 212:
			{
				SpecialEffectList value213 = instance.GetPrepareTotalProgress();
				Serializer.Deserialize(dataPool, valueOffset, ref value213);
				instance.SetPrepareTotalProgress(value213, context);
				break;
			}
			case 213:
			{
				SpecialEffectList value214 = instance.GetSpecificGridCount();
				Serializer.Deserialize(dataPool, valueOffset, ref value214);
				instance.SetSpecificGridCount(value214, context);
				break;
			}
			case 214:
			{
				SpecialEffectList value215 = instance.GetGenericGridCount();
				Serializer.Deserialize(dataPool, valueOffset, ref value215);
				instance.SetGenericGridCount(value215, context);
				break;
			}
			case 215:
			{
				SpecialEffectList value216 = instance.GetCanInterrupt();
				Serializer.Deserialize(dataPool, valueOffset, ref value216);
				instance.SetCanInterrupt(value216, context);
				break;
			}
			case 216:
			{
				SpecialEffectList value217 = instance.GetInterruptOdds();
				Serializer.Deserialize(dataPool, valueOffset, ref value217);
				instance.SetInterruptOdds(value217, context);
				break;
			}
			case 217:
			{
				SpecialEffectList value218 = instance.GetCanSilence();
				Serializer.Deserialize(dataPool, valueOffset, ref value218);
				instance.SetCanSilence(value218, context);
				break;
			}
			case 218:
			{
				SpecialEffectList value219 = instance.GetSilenceOdds();
				Serializer.Deserialize(dataPool, valueOffset, ref value219);
				instance.SetSilenceOdds(value219, context);
				break;
			}
			case 219:
			{
				SpecialEffectList value220 = instance.GetCanCastWithBrokenBodyPart();
				Serializer.Deserialize(dataPool, valueOffset, ref value220);
				instance.SetCanCastWithBrokenBodyPart(value220, context);
				break;
			}
			case 220:
			{
				SpecialEffectList value221 = instance.GetAddPowerCanBeRemoved();
				Serializer.Deserialize(dataPool, valueOffset, ref value221);
				instance.SetAddPowerCanBeRemoved(value221, context);
				break;
			}
			case 221:
			{
				SpecialEffectList value222 = instance.GetSkillType();
				Serializer.Deserialize(dataPool, valueOffset, ref value222);
				instance.SetSkillType(value222, context);
				break;
			}
			case 222:
			{
				SpecialEffectList value223 = instance.GetEffectCountCanChange();
				Serializer.Deserialize(dataPool, valueOffset, ref value223);
				instance.SetEffectCountCanChange(value223, context);
				break;
			}
			case 223:
			{
				SpecialEffectList value224 = instance.GetCanCastInDefend();
				Serializer.Deserialize(dataPool, valueOffset, ref value224);
				instance.SetCanCastInDefend(value224, context);
				break;
			}
			case 224:
			{
				SpecialEffectList value225 = instance.GetHitDistribution();
				Serializer.Deserialize(dataPool, valueOffset, ref value225);
				instance.SetHitDistribution(value225, context);
				break;
			}
			case 225:
			{
				SpecialEffectList value226 = instance.GetCanCastOnLackBreath();
				Serializer.Deserialize(dataPool, valueOffset, ref value226);
				instance.SetCanCastOnLackBreath(value226, context);
				break;
			}
			case 226:
			{
				SpecialEffectList value227 = instance.GetCanCastOnLackStance();
				Serializer.Deserialize(dataPool, valueOffset, ref value227);
				instance.SetCanCastOnLackStance(value227, context);
				break;
			}
			case 227:
			{
				SpecialEffectList value228 = instance.GetCostBreathOnCast();
				Serializer.Deserialize(dataPool, valueOffset, ref value228);
				instance.SetCostBreathOnCast(value228, context);
				break;
			}
			case 228:
			{
				SpecialEffectList value229 = instance.GetCostStanceOnCast();
				Serializer.Deserialize(dataPool, valueOffset, ref value229);
				instance.SetCostStanceOnCast(value229, context);
				break;
			}
			case 229:
			{
				SpecialEffectList value230 = instance.GetCanUseMobilityAsBreath();
				Serializer.Deserialize(dataPool, valueOffset, ref value230);
				instance.SetCanUseMobilityAsBreath(value230, context);
				break;
			}
			case 230:
			{
				SpecialEffectList value231 = instance.GetCanUseMobilityAsStance();
				Serializer.Deserialize(dataPool, valueOffset, ref value231);
				instance.SetCanUseMobilityAsStance(value231, context);
				break;
			}
			case 231:
			{
				SpecialEffectList value232 = instance.GetCastCostNeiliAllocation();
				Serializer.Deserialize(dataPool, valueOffset, ref value232);
				instance.SetCastCostNeiliAllocation(value232, context);
				break;
			}
			case 232:
			{
				SpecialEffectList value233 = instance.GetAcceptPoisonResist();
				Serializer.Deserialize(dataPool, valueOffset, ref value233);
				instance.SetAcceptPoisonResist(value233, context);
				break;
			}
			case 233:
			{
				SpecialEffectList value234 = instance.GetMakePoisonResist();
				Serializer.Deserialize(dataPool, valueOffset, ref value234);
				instance.SetMakePoisonResist(value234, context);
				break;
			}
			case 234:
			{
				SpecialEffectList value235 = instance.GetCanCriticalHit();
				Serializer.Deserialize(dataPool, valueOffset, ref value235);
				instance.SetCanCriticalHit(value235, context);
				break;
			}
			case 235:
			{
				SpecialEffectList value236 = instance.GetCanCostNeiliAllocationEffect();
				Serializer.Deserialize(dataPool, valueOffset, ref value236);
				instance.SetCanCostNeiliAllocationEffect(value236, context);
				break;
			}
			case 236:
			{
				SpecialEffectList value237 = instance.GetConsummateLevelRelatedMainAttributesHitValues();
				Serializer.Deserialize(dataPool, valueOffset, ref value237);
				instance.SetConsummateLevelRelatedMainAttributesHitValues(value237, context);
				break;
			}
			case 237:
			{
				SpecialEffectList value238 = instance.GetConsummateLevelRelatedMainAttributesAvoidValues();
				Serializer.Deserialize(dataPool, valueOffset, ref value238);
				instance.SetConsummateLevelRelatedMainAttributesAvoidValues(value238, context);
				break;
			}
			case 238:
			{
				SpecialEffectList value239 = instance.GetConsummateLevelRelatedMainAttributesPenetrations();
				Serializer.Deserialize(dataPool, valueOffset, ref value239);
				instance.SetConsummateLevelRelatedMainAttributesPenetrations(value239, context);
				break;
			}
			case 239:
			{
				SpecialEffectList value240 = instance.GetConsummateLevelRelatedMainAttributesPenetrationResists();
				Serializer.Deserialize(dataPool, valueOffset, ref value240);
				instance.SetConsummateLevelRelatedMainAttributesPenetrationResists(value240, context);
				break;
			}
			case 240:
			{
				SpecialEffectList value241 = instance.GetSkillAlsoAsFiveElements();
				Serializer.Deserialize(dataPool, valueOffset, ref value241);
				instance.SetSkillAlsoAsFiveElements(value241, context);
				break;
			}
			case 241:
			{
				SpecialEffectList value242 = instance.GetInnerInjuryImmunity();
				Serializer.Deserialize(dataPool, valueOffset, ref value242);
				instance.SetInnerInjuryImmunity(value242, context);
				break;
			}
			case 242:
			{
				SpecialEffectList value243 = instance.GetOuterInjuryImmunity();
				Serializer.Deserialize(dataPool, valueOffset, ref value243);
				instance.SetOuterInjuryImmunity(value243, context);
				break;
			}
			case 243:
			{
				SpecialEffectList value244 = instance.GetPoisonAffectThreshold();
				Serializer.Deserialize(dataPool, valueOffset, ref value244);
				instance.SetPoisonAffectThreshold(value244, context);
				break;
			}
			case 244:
			{
				SpecialEffectList value245 = instance.GetLockDistance();
				Serializer.Deserialize(dataPool, valueOffset, ref value245);
				instance.SetLockDistance(value245, context);
				break;
			}
			case 245:
			{
				SpecialEffectList value246 = instance.GetResistOfAllPoison();
				Serializer.Deserialize(dataPool, valueOffset, ref value246);
				instance.SetResistOfAllPoison(value246, context);
				break;
			}
			case 246:
			{
				SpecialEffectList value247 = instance.GetMakePoisonTarget();
				Serializer.Deserialize(dataPool, valueOffset, ref value247);
				instance.SetMakePoisonTarget(value247, context);
				break;
			}
			case 247:
			{
				SpecialEffectList value248 = instance.GetAcceptPoisonTarget();
				Serializer.Deserialize(dataPool, valueOffset, ref value248);
				instance.SetAcceptPoisonTarget(value248, context);
				break;
			}
			case 248:
			{
				SpecialEffectList value249 = instance.GetCertainCriticalHit();
				Serializer.Deserialize(dataPool, valueOffset, ref value249);
				instance.SetCertainCriticalHit(value249, context);
				break;
			}
			case 249:
			{
				SpecialEffectList value250 = instance.GetMindMarkCount();
				Serializer.Deserialize(dataPool, valueOffset, ref value250);
				instance.SetMindMarkCount(value250, context);
				break;
			}
			case 250:
			{
				SpecialEffectList value251 = instance.GetCanFightBackWithHit();
				Serializer.Deserialize(dataPool, valueOffset, ref value251);
				instance.SetCanFightBackWithHit(value251, context);
				break;
			}
			case 251:
			{
				SpecialEffectList value252 = instance.GetInevitableHit();
				Serializer.Deserialize(dataPool, valueOffset, ref value252);
				instance.SetInevitableHit(value252, context);
				break;
			}
			case 252:
			{
				SpecialEffectList value253 = instance.GetAttackCanPursue();
				Serializer.Deserialize(dataPool, valueOffset, ref value253);
				instance.SetAttackCanPursue(value253, context);
				break;
			}
			case 253:
			{
				SpecialEffectList value254 = instance.GetCombatSkillDataEffectList();
				Serializer.Deserialize(dataPool, valueOffset, ref value254);
				instance.SetCombatSkillDataEffectList(value254, context);
				break;
			}
			case 254:
			{
				SpecialEffectList value255 = instance.GetCriticalOdds();
				Serializer.Deserialize(dataPool, valueOffset, ref value255);
				instance.SetCriticalOdds(value255, context);
				break;
			}
			case 255:
			{
				SpecialEffectList value256 = instance.GetStanceCostByEffect();
				Serializer.Deserialize(dataPool, valueOffset, ref value256);
				instance.SetStanceCostByEffect(value256, context);
				break;
			}
			case 256:
			{
				SpecialEffectList value257 = instance.GetBreathCostByEffect();
				Serializer.Deserialize(dataPool, valueOffset, ref value257);
				instance.SetBreathCostByEffect(value257, context);
				break;
			}
			case 257:
			{
				SpecialEffectList value258 = instance.GetPowerAddRatio();
				Serializer.Deserialize(dataPool, valueOffset, ref value258);
				instance.SetPowerAddRatio(value258, context);
				break;
			}
			case 258:
			{
				SpecialEffectList value259 = instance.GetPowerReduceRatio();
				Serializer.Deserialize(dataPool, valueOffset, ref value259);
				instance.SetPowerReduceRatio(value259, context);
				break;
			}
			case 259:
			{
				SpecialEffectList value260 = instance.GetPoisonAffectProduceValue();
				Serializer.Deserialize(dataPool, valueOffset, ref value260);
				instance.SetPoisonAffectProduceValue(value260, context);
				break;
			}
			case 260:
			{
				SpecialEffectList value261 = instance.GetCanReadingOnMonthChange();
				Serializer.Deserialize(dataPool, valueOffset, ref value261);
				instance.SetCanReadingOnMonthChange(value261, context);
				break;
			}
			case 261:
			{
				SpecialEffectList value262 = instance.GetMedicineEffect();
				Serializer.Deserialize(dataPool, valueOffset, ref value262);
				instance.SetMedicineEffect(value262, context);
				break;
			}
			case 262:
			{
				SpecialEffectList value263 = instance.GetXiangshuInfectionDelta();
				Serializer.Deserialize(dataPool, valueOffset, ref value263);
				instance.SetXiangshuInfectionDelta(value263, context);
				break;
			}
			case 263:
			{
				SpecialEffectList value264 = instance.GetHealthDelta();
				Serializer.Deserialize(dataPool, valueOffset, ref value264);
				instance.SetHealthDelta(value264, context);
				break;
			}
			case 264:
			{
				SpecialEffectList value265 = instance.GetWeaponSilenceFrame();
				Serializer.Deserialize(dataPool, valueOffset, ref value265);
				instance.SetWeaponSilenceFrame(value265, context);
				break;
			}
			case 265:
			{
				SpecialEffectList value266 = instance.GetSilenceFrame();
				Serializer.Deserialize(dataPool, valueOffset, ref value266);
				instance.SetSilenceFrame(value266, context);
				break;
			}
			case 266:
			{
				SpecialEffectList value267 = instance.GetCurrAgeDelta();
				Serializer.Deserialize(dataPool, valueOffset, ref value267);
				instance.SetCurrAgeDelta(value267, context);
				break;
			}
			case 267:
			{
				SpecialEffectList value268 = instance.GetGoneMadInAllBreak();
				Serializer.Deserialize(dataPool, valueOffset, ref value268);
				instance.SetGoneMadInAllBreak(value268, context);
				break;
			}
			case 268:
			{
				SpecialEffectList value269 = instance.GetMakeLoveRateOnMonthChange();
				Serializer.Deserialize(dataPool, valueOffset, ref value269);
				instance.SetMakeLoveRateOnMonthChange(value269, context);
				break;
			}
			case 269:
			{
				SpecialEffectList value270 = instance.GetCanAutoHealOnMonthChange();
				Serializer.Deserialize(dataPool, valueOffset, ref value270);
				instance.SetCanAutoHealOnMonthChange(value270, context);
				break;
			}
			case 270:
			{
				SpecialEffectList value271 = instance.GetHappinessDelta();
				Serializer.Deserialize(dataPool, valueOffset, ref value271);
				instance.SetHappinessDelta(value271, context);
				break;
			}
			case 271:
			{
				SpecialEffectList value272 = instance.GetTeammateCmdCanUse();
				Serializer.Deserialize(dataPool, valueOffset, ref value272);
				instance.SetTeammateCmdCanUse(value272, context);
				break;
			}
			case 272:
			{
				SpecialEffectList value273 = instance.GetMixPoisonInfinityAffect();
				Serializer.Deserialize(dataPool, valueOffset, ref value273);
				instance.SetMixPoisonInfinityAffect(value273, context);
				break;
			}
			case 273:
			{
				SpecialEffectList value274 = instance.GetAttackRangeMaxAcupoint();
				Serializer.Deserialize(dataPool, valueOffset, ref value274);
				instance.SetAttackRangeMaxAcupoint(value274, context);
				break;
			}
			case 274:
			{
				SpecialEffectList value275 = instance.GetMaxMobilityPercent();
				Serializer.Deserialize(dataPool, valueOffset, ref value275);
				instance.SetMaxMobilityPercent(value275, context);
				break;
			}
			case 275:
			{
				SpecialEffectList value276 = instance.GetMakeMindDamage();
				Serializer.Deserialize(dataPool, valueOffset, ref value276);
				instance.SetMakeMindDamage(value276, context);
				break;
			}
			case 276:
			{
				SpecialEffectList value277 = instance.GetAcceptMindDamage();
				Serializer.Deserialize(dataPool, valueOffset, ref value277);
				instance.SetAcceptMindDamage(value277, context);
				break;
			}
			case 277:
			{
				SpecialEffectList value278 = instance.GetHitAddByTempValue();
				Serializer.Deserialize(dataPool, valueOffset, ref value278);
				instance.SetHitAddByTempValue(value278, context);
				break;
			}
			case 278:
			{
				SpecialEffectList value279 = instance.GetAvoidAddByTempValue();
				Serializer.Deserialize(dataPool, valueOffset, ref value279);
				instance.SetAvoidAddByTempValue(value279, context);
				break;
			}
			case 279:
			{
				SpecialEffectList value280 = instance.GetIgnoreEquipmentOverload();
				Serializer.Deserialize(dataPool, valueOffset, ref value280);
				instance.SetIgnoreEquipmentOverload(value280, context);
				break;
			}
			case 280:
			{
				SpecialEffectList value281 = instance.GetCanCostEnemyUsableTricks();
				Serializer.Deserialize(dataPool, valueOffset, ref value281);
				instance.SetCanCostEnemyUsableTricks(value281, context);
				break;
			}
			case 281:
			{
				SpecialEffectList value282 = instance.GetIgnoreArmor();
				Serializer.Deserialize(dataPool, valueOffset, ref value282);
				instance.SetIgnoreArmor(value282, context);
				break;
			}
			case 282:
			{
				SpecialEffectList value283 = instance.GetUnyieldingFallen();
				Serializer.Deserialize(dataPool, valueOffset, ref value283);
				instance.SetUnyieldingFallen(value283, context);
				break;
			}
			case 283:
			{
				SpecialEffectList value284 = instance.GetNormalAttackPrepareFrame();
				Serializer.Deserialize(dataPool, valueOffset, ref value284);
				instance.SetNormalAttackPrepareFrame(value284, context);
				break;
			}
			case 284:
			{
				SpecialEffectList value285 = instance.GetCanCostUselessTricks();
				Serializer.Deserialize(dataPool, valueOffset, ref value285);
				instance.SetCanCostUselessTricks(value285, context);
				break;
			}
			case 285:
			{
				SpecialEffectList value286 = instance.GetDefendSkillCanAffect();
				Serializer.Deserialize(dataPool, valueOffset, ref value286);
				instance.SetDefendSkillCanAffect(value286, context);
				break;
			}
			case 286:
			{
				SpecialEffectList value287 = instance.GetAssistSkillCanAffect();
				Serializer.Deserialize(dataPool, valueOffset, ref value287);
				instance.SetAssistSkillCanAffect(value287, context);
				break;
			}
			case 287:
			{
				SpecialEffectList value288 = instance.GetAgileSkillCanAffect();
				Serializer.Deserialize(dataPool, valueOffset, ref value288);
				instance.SetAgileSkillCanAffect(value288, context);
				break;
			}
			case 288:
			{
				SpecialEffectList value289 = instance.GetAllMarkChangeToMind();
				Serializer.Deserialize(dataPool, valueOffset, ref value289);
				instance.SetAllMarkChangeToMind(value289, context);
				break;
			}
			case 289:
			{
				SpecialEffectList value290 = instance.GetMindMarkChangeToFatal();
				Serializer.Deserialize(dataPool, valueOffset, ref value290);
				instance.SetMindMarkChangeToFatal(value290, context);
				break;
			}
			case 290:
			{
				SpecialEffectList value291 = instance.GetCanCast();
				Serializer.Deserialize(dataPool, valueOffset, ref value291);
				instance.SetCanCast(value291, context);
				break;
			}
			case 291:
			{
				SpecialEffectList value292 = instance.GetInevitableAvoid();
				Serializer.Deserialize(dataPool, valueOffset, ref value292);
				instance.SetInevitableAvoid(value292, context);
				break;
			}
			case 292:
			{
				SpecialEffectList value293 = instance.GetPowerEffectReverse();
				Serializer.Deserialize(dataPool, valueOffset, ref value293);
				instance.SetPowerEffectReverse(value293, context);
				break;
			}
			case 293:
			{
				SpecialEffectList value294 = instance.GetFeatureBonusReverse();
				Serializer.Deserialize(dataPool, valueOffset, ref value294);
				instance.SetFeatureBonusReverse(value294, context);
				break;
			}
			case 294:
			{
				SpecialEffectList value295 = instance.GetWugFatalDamageValue();
				Serializer.Deserialize(dataPool, valueOffset, ref value295);
				instance.SetWugFatalDamageValue(value295, context);
				break;
			}
			case 295:
			{
				SpecialEffectList value296 = instance.GetCanRecoverHealthOnMonthChange();
				Serializer.Deserialize(dataPool, valueOffset, ref value296);
				instance.SetCanRecoverHealthOnMonthChange(value296, context);
				break;
			}
			case 296:
			{
				SpecialEffectList value297 = instance.GetTakeRevengeRateOnMonthChange();
				Serializer.Deserialize(dataPool, valueOffset, ref value297);
				instance.SetTakeRevengeRateOnMonthChange(value297, context);
				break;
			}
			case 297:
			{
				SpecialEffectList value298 = instance.GetConsummateLevelBonus();
				Serializer.Deserialize(dataPool, valueOffset, ref value298);
				instance.SetConsummateLevelBonus(value298, context);
				break;
			}
			case 298:
			{
				SpecialEffectList value299 = instance.GetNeiliDelta();
				Serializer.Deserialize(dataPool, valueOffset, ref value299);
				instance.SetNeiliDelta(value299, context);
				break;
			}
			case 299:
			{
				SpecialEffectList value300 = instance.GetCanMakeLoveSpecialOnMonthChange();
				Serializer.Deserialize(dataPool, valueOffset, ref value300);
				instance.SetCanMakeLoveSpecialOnMonthChange(value300, context);
				break;
			}
			case 300:
			{
				SpecialEffectList value301 = instance.GetHealAcupointSpeed();
				Serializer.Deserialize(dataPool, valueOffset, ref value301);
				instance.SetHealAcupointSpeed(value301, context);
				break;
			}
			case 301:
			{
				SpecialEffectList value302 = instance.GetMaxChangeTrickCount();
				Serializer.Deserialize(dataPool, valueOffset, ref value302);
				instance.SetMaxChangeTrickCount(value302, context);
				break;
			}
			case 302:
			{
				SpecialEffectList value303 = instance.GetConvertCostBreathAndStance();
				Serializer.Deserialize(dataPool, valueOffset, ref value303);
				instance.SetConvertCostBreathAndStance(value303, context);
				break;
			}
			case 303:
			{
				SpecialEffectList value304 = instance.GetPersonalitiesAll();
				Serializer.Deserialize(dataPool, valueOffset, ref value304);
				instance.SetPersonalitiesAll(value304, context);
				break;
			}
			case 304:
			{
				SpecialEffectList value305 = instance.GetFinalFatalDamageMarkCount();
				Serializer.Deserialize(dataPool, valueOffset, ref value305);
				instance.SetFinalFatalDamageMarkCount(value305, context);
				break;
			}
			case 305:
			{
				SpecialEffectList value306 = instance.GetInfinityMindMarkProgress();
				Serializer.Deserialize(dataPool, valueOffset, ref value306);
				instance.SetInfinityMindMarkProgress(value306, context);
				break;
			}
			case 306:
			{
				SpecialEffectList value307 = instance.GetCombatSkillAiScorePower();
				Serializer.Deserialize(dataPool, valueOffset, ref value307);
				instance.SetCombatSkillAiScorePower(value307, context);
				break;
			}
			case 307:
			{
				SpecialEffectList value308 = instance.GetNormalAttackChangeToUnlockAttack();
				Serializer.Deserialize(dataPool, valueOffset, ref value308);
				instance.SetNormalAttackChangeToUnlockAttack(value308, context);
				break;
			}
			case 308:
			{
				SpecialEffectList value309 = instance.GetAttackBodyPartOdds();
				Serializer.Deserialize(dataPool, valueOffset, ref value309);
				instance.SetAttackBodyPartOdds(value309, context);
				break;
			}
			case 309:
			{
				SpecialEffectList value310 = instance.GetChangeDurability();
				Serializer.Deserialize(dataPool, valueOffset, ref value310);
				instance.SetChangeDurability(value310, context);
				break;
			}
			case 310:
			{
				SpecialEffectList value311 = instance.GetEquipmentBonus();
				Serializer.Deserialize(dataPool, valueOffset, ref value311);
				instance.SetEquipmentBonus(value311, context);
				break;
			}
			case 311:
			{
				SpecialEffectList value312 = instance.GetEquipmentWeight();
				Serializer.Deserialize(dataPool, valueOffset, ref value312);
				instance.SetEquipmentWeight(value312, context);
				break;
			}
			case 312:
			{
				SpecialEffectList value313 = instance.GetRawCreateEffectList();
				Serializer.Deserialize(dataPool, valueOffset, ref value313);
				instance.SetRawCreateEffectList(value313, context);
				break;
			}
			case 313:
			{
				SpecialEffectList value314 = instance.GetJiTrickAsWeaponTrickCount();
				Serializer.Deserialize(dataPool, valueOffset, ref value314);
				instance.SetJiTrickAsWeaponTrickCount(value314, context);
				break;
			}
			case 314:
			{
				SpecialEffectList value315 = instance.GetUselessTrickAsJiTrickCount();
				Serializer.Deserialize(dataPool, valueOffset, ref value315);
				instance.SetUselessTrickAsJiTrickCount(value315, context);
				break;
			}
			case 315:
			{
				SpecialEffectList value316 = instance.GetEquipmentPower();
				Serializer.Deserialize(dataPool, valueOffset, ref value316);
				instance.SetEquipmentPower(value316, context);
				break;
			}
			case 316:
			{
				SpecialEffectList value317 = instance.GetHealFlawSpeed();
				Serializer.Deserialize(dataPool, valueOffset, ref value317);
				instance.SetHealFlawSpeed(value317, context);
				break;
			}
			case 317:
			{
				SpecialEffectList value318 = instance.GetUnlockSpeed();
				Serializer.Deserialize(dataPool, valueOffset, ref value318);
				instance.SetUnlockSpeed(value318, context);
				break;
			}
			case 318:
			{
				SpecialEffectList value319 = instance.GetFlawBonusFactor();
				Serializer.Deserialize(dataPool, valueOffset, ref value319);
				instance.SetFlawBonusFactor(value319, context);
				break;
			}
			case 319:
			{
				SpecialEffectList value320 = instance.GetCanCostShaTricks();
				Serializer.Deserialize(dataPool, valueOffset, ref value320);
				instance.SetCanCostShaTricks(value320, context);
				break;
			}
			case 320:
			{
				SpecialEffectList value321 = instance.GetDefenderDirectFinalDamageValue();
				Serializer.Deserialize(dataPool, valueOffset, ref value321);
				instance.SetDefenderDirectFinalDamageValue(value321, context);
				break;
			}
			case 321:
			{
				SpecialEffectList value322 = instance.GetNormalAttackRecoveryFrame();
				Serializer.Deserialize(dataPool, valueOffset, ref value322);
				instance.SetNormalAttackRecoveryFrame(value322, context);
				break;
			}
			case 322:
			{
				SpecialEffectList value323 = instance.GetFinalGoneMadInjury();
				Serializer.Deserialize(dataPool, valueOffset, ref value323);
				instance.SetFinalGoneMadInjury(value323, context);
				break;
			}
			case 323:
			{
				SpecialEffectList value324 = instance.GetAttackerDirectFinalDamageValue();
				Serializer.Deserialize(dataPool, valueOffset, ref value324);
				instance.SetAttackerDirectFinalDamageValue(value324, context);
				break;
			}
			case 324:
			{
				SpecialEffectList value325 = instance.GetCanCostTrickDuringPreparingSkill();
				Serializer.Deserialize(dataPool, valueOffset, ref value325);
				instance.SetCanCostTrickDuringPreparingSkill(value325, context);
				break;
			}
			case 325:
			{
				SpecialEffectList value326 = instance.GetValidItemList();
				Serializer.Deserialize(dataPool, valueOffset, ref value326);
				instance.SetValidItemList(value326, context);
				break;
			}
			case 326:
			{
				SpecialEffectList value327 = instance.GetAcceptDamageCanAdd();
				Serializer.Deserialize(dataPool, valueOffset, ref value327);
				instance.SetAcceptDamageCanAdd(value327, context);
				break;
			}
			case 327:
			{
				SpecialEffectList value328 = instance.GetMakeDamageCanReduce();
				Serializer.Deserialize(dataPool, valueOffset, ref value328);
				instance.SetMakeDamageCanReduce(value328, context);
				break;
			}
			case 328:
			{
				SpecialEffectList value329 = instance.GetNormalAttackGetTrickCount();
				Serializer.Deserialize(dataPool, valueOffset, ref value329);
				instance.SetNormalAttackGetTrickCount(value329, context);
				break;
			}
			default:
			{
				bool flag2 = fieldId >= 329;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (flag2)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = fieldId >= 329;
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

		// Token: 0x0600293C RID: 10556 RVA: 0x001F8A2C File Offset: 0x001F6C2C
		private int CheckModified_AffectedDatas(int objectId, ushort fieldId, RawDataPool dataPool)
		{
			AffectedData instance;
			bool flag = !this._affectedDatas.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = fieldId >= 329;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesAffectedDatas.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					this._dataStatesAffectedDatas.ResetModified(instance.DataStatesOffset, (int)fieldId);
					switch (fieldId)
					{
					case 0:
						result = Serializer.Serialize(instance.GetId(), dataPool);
						break;
					case 1:
						result = Serializer.Serialize(instance.GetMaxStrength(), dataPool);
						break;
					case 2:
						result = Serializer.Serialize(instance.GetMaxDexterity(), dataPool);
						break;
					case 3:
						result = Serializer.Serialize(instance.GetMaxConcentration(), dataPool);
						break;
					case 4:
						result = Serializer.Serialize(instance.GetMaxVitality(), dataPool);
						break;
					case 5:
						result = Serializer.Serialize(instance.GetMaxEnergy(), dataPool);
						break;
					case 6:
						result = Serializer.Serialize(instance.GetMaxIntelligence(), dataPool);
						break;
					case 7:
						result = Serializer.Serialize(instance.GetRecoveryOfStance(), dataPool);
						break;
					case 8:
						result = Serializer.Serialize(instance.GetRecoveryOfBreath(), dataPool);
						break;
					case 9:
						result = Serializer.Serialize(instance.GetMoveSpeed(), dataPool);
						break;
					case 10:
						result = Serializer.Serialize(instance.GetRecoveryOfFlaw(), dataPool);
						break;
					case 11:
						result = Serializer.Serialize(instance.GetCastSpeed(), dataPool);
						break;
					case 12:
						result = Serializer.Serialize(instance.GetRecoveryOfBlockedAcupoint(), dataPool);
						break;
					case 13:
						result = Serializer.Serialize(instance.GetWeaponSwitchSpeed(), dataPool);
						break;
					case 14:
						result = Serializer.Serialize(instance.GetAttackSpeed(), dataPool);
						break;
					case 15:
						result = Serializer.Serialize(instance.GetInnerRatio(), dataPool);
						break;
					case 16:
						result = Serializer.Serialize(instance.GetRecoveryOfQiDisorder(), dataPool);
						break;
					case 17:
						result = Serializer.Serialize(instance.GetMinorAttributeFixMaxValue(), dataPool);
						break;
					case 18:
						result = Serializer.Serialize(instance.GetMinorAttributeFixMinValue(), dataPool);
						break;
					case 19:
						result = Serializer.Serialize(instance.GetResistOfHotPoison(), dataPool);
						break;
					case 20:
						result = Serializer.Serialize(instance.GetResistOfGloomyPoison(), dataPool);
						break;
					case 21:
						result = Serializer.Serialize(instance.GetResistOfColdPoison(), dataPool);
						break;
					case 22:
						result = Serializer.Serialize(instance.GetResistOfRedPoison(), dataPool);
						break;
					case 23:
						result = Serializer.Serialize(instance.GetResistOfRottenPoison(), dataPool);
						break;
					case 24:
						result = Serializer.Serialize(instance.GetResistOfIllusoryPoison(), dataPool);
						break;
					case 25:
						result = Serializer.Serialize(instance.GetDisplayAge(), dataPool);
						break;
					case 26:
						result = Serializer.Serialize(instance.GetNeiliProportionOfFiveElements(), dataPool);
						break;
					case 27:
						result = Serializer.Serialize(instance.GetWeaponMaxPower(), dataPool);
						break;
					case 28:
						result = Serializer.Serialize(instance.GetWeaponUseRequirement(), dataPool);
						break;
					case 29:
						result = Serializer.Serialize(instance.GetWeaponAttackRange(), dataPool);
						break;
					case 30:
						result = Serializer.Serialize(instance.GetArmorMaxPower(), dataPool);
						break;
					case 31:
						result = Serializer.Serialize(instance.GetArmorUseRequirement(), dataPool);
						break;
					case 32:
						result = Serializer.Serialize(instance.GetHitStrength(), dataPool);
						break;
					case 33:
						result = Serializer.Serialize(instance.GetHitTechnique(), dataPool);
						break;
					case 34:
						result = Serializer.Serialize(instance.GetHitSpeed(), dataPool);
						break;
					case 35:
						result = Serializer.Serialize(instance.GetHitMind(), dataPool);
						break;
					case 36:
						result = Serializer.Serialize(instance.GetHitCanChange(), dataPool);
						break;
					case 37:
						result = Serializer.Serialize(instance.GetHitChangeEffectPercent(), dataPool);
						break;
					case 38:
						result = Serializer.Serialize(instance.GetAvoidStrength(), dataPool);
						break;
					case 39:
						result = Serializer.Serialize(instance.GetAvoidTechnique(), dataPool);
						break;
					case 40:
						result = Serializer.Serialize(instance.GetAvoidSpeed(), dataPool);
						break;
					case 41:
						result = Serializer.Serialize(instance.GetAvoidMind(), dataPool);
						break;
					case 42:
						result = Serializer.Serialize(instance.GetAvoidCanChange(), dataPool);
						break;
					case 43:
						result = Serializer.Serialize(instance.GetAvoidChangeEffectPercent(), dataPool);
						break;
					case 44:
						result = Serializer.Serialize(instance.GetPenetrateOuter(), dataPool);
						break;
					case 45:
						result = Serializer.Serialize(instance.GetPenetrateInner(), dataPool);
						break;
					case 46:
						result = Serializer.Serialize(instance.GetPenetrateResistOuter(), dataPool);
						break;
					case 47:
						result = Serializer.Serialize(instance.GetPenetrateResistInner(), dataPool);
						break;
					case 48:
						result = Serializer.Serialize(instance.GetNeiliAllocationAttack(), dataPool);
						break;
					case 49:
						result = Serializer.Serialize(instance.GetNeiliAllocationAgile(), dataPool);
						break;
					case 50:
						result = Serializer.Serialize(instance.GetNeiliAllocationDefense(), dataPool);
						break;
					case 51:
						result = Serializer.Serialize(instance.GetNeiliAllocationAssist(), dataPool);
						break;
					case 52:
						result = Serializer.Serialize(instance.GetHappiness(), dataPool);
						break;
					case 53:
						result = Serializer.Serialize(instance.GetMaxHealth(), dataPool);
						break;
					case 54:
						result = Serializer.Serialize(instance.GetHealthCost(), dataPool);
						break;
					case 55:
						result = Serializer.Serialize(instance.GetMoveSpeedCanChange(), dataPool);
						break;
					case 56:
						result = Serializer.Serialize(instance.GetAttackerHitStrength(), dataPool);
						break;
					case 57:
						result = Serializer.Serialize(instance.GetAttackerHitTechnique(), dataPool);
						break;
					case 58:
						result = Serializer.Serialize(instance.GetAttackerHitSpeed(), dataPool);
						break;
					case 59:
						result = Serializer.Serialize(instance.GetAttackerHitMind(), dataPool);
						break;
					case 60:
						result = Serializer.Serialize(instance.GetAttackerAvoidStrength(), dataPool);
						break;
					case 61:
						result = Serializer.Serialize(instance.GetAttackerAvoidTechnique(), dataPool);
						break;
					case 62:
						result = Serializer.Serialize(instance.GetAttackerAvoidSpeed(), dataPool);
						break;
					case 63:
						result = Serializer.Serialize(instance.GetAttackerAvoidMind(), dataPool);
						break;
					case 64:
						result = Serializer.Serialize(instance.GetAttackerPenetrateOuter(), dataPool);
						break;
					case 65:
						result = Serializer.Serialize(instance.GetAttackerPenetrateInner(), dataPool);
						break;
					case 66:
						result = Serializer.Serialize(instance.GetAttackerPenetrateResistOuter(), dataPool);
						break;
					case 67:
						result = Serializer.Serialize(instance.GetAttackerPenetrateResistInner(), dataPool);
						break;
					case 68:
						result = Serializer.Serialize(instance.GetAttackHitType(), dataPool);
						break;
					case 69:
						result = Serializer.Serialize(instance.GetMakeDirectDamage(), dataPool);
						break;
					case 70:
						result = Serializer.Serialize(instance.GetMakeBounceDamage(), dataPool);
						break;
					case 71:
						result = Serializer.Serialize(instance.GetMakeFightBackDamage(), dataPool);
						break;
					case 72:
						result = Serializer.Serialize(instance.GetMakePoisonLevel(), dataPool);
						break;
					case 73:
						result = Serializer.Serialize(instance.GetMakePoisonValue(), dataPool);
						break;
					case 74:
						result = Serializer.Serialize(instance.GetAttackerHitOdds(), dataPool);
						break;
					case 75:
						result = Serializer.Serialize(instance.GetAttackerFightBackHitOdds(), dataPool);
						break;
					case 76:
						result = Serializer.Serialize(instance.GetAttackerPursueOdds(), dataPool);
						break;
					case 77:
						result = Serializer.Serialize(instance.GetMakedInjuryChangeToOld(), dataPool);
						break;
					case 78:
						result = Serializer.Serialize(instance.GetMakedPoisonChangeToOld(), dataPool);
						break;
					case 79:
						result = Serializer.Serialize(instance.GetMakeDamageType(), dataPool);
						break;
					case 80:
						result = Serializer.Serialize(instance.GetCanMakeInjuryToNoInjuryPart(), dataPool);
						break;
					case 81:
						result = Serializer.Serialize(instance.GetMakePoisonType(), dataPool);
						break;
					case 82:
						result = Serializer.Serialize(instance.GetNormalAttackWeapon(), dataPool);
						break;
					case 83:
						result = Serializer.Serialize(instance.GetNormalAttackTrick(), dataPool);
						break;
					case 84:
						result = Serializer.Serialize(instance.GetExtraFlawCount(), dataPool);
						break;
					case 85:
						result = Serializer.Serialize(instance.GetAttackCanBounce(), dataPool);
						break;
					case 86:
						result = Serializer.Serialize(instance.GetAttackCanFightBack(), dataPool);
						break;
					case 87:
						result = Serializer.Serialize(instance.GetMakeFightBackInjuryMark(), dataPool);
						break;
					case 88:
						result = Serializer.Serialize(instance.GetLegSkillUseShoes(), dataPool);
						break;
					case 89:
						result = Serializer.Serialize(instance.GetAttackerFinalDamageValue(), dataPool);
						break;
					case 90:
						result = Serializer.Serialize(instance.GetDefenderHitStrength(), dataPool);
						break;
					case 91:
						result = Serializer.Serialize(instance.GetDefenderHitTechnique(), dataPool);
						break;
					case 92:
						result = Serializer.Serialize(instance.GetDefenderHitSpeed(), dataPool);
						break;
					case 93:
						result = Serializer.Serialize(instance.GetDefenderHitMind(), dataPool);
						break;
					case 94:
						result = Serializer.Serialize(instance.GetDefenderAvoidStrength(), dataPool);
						break;
					case 95:
						result = Serializer.Serialize(instance.GetDefenderAvoidTechnique(), dataPool);
						break;
					case 96:
						result = Serializer.Serialize(instance.GetDefenderAvoidSpeed(), dataPool);
						break;
					case 97:
						result = Serializer.Serialize(instance.GetDefenderAvoidMind(), dataPool);
						break;
					case 98:
						result = Serializer.Serialize(instance.GetDefenderPenetrateOuter(), dataPool);
						break;
					case 99:
						result = Serializer.Serialize(instance.GetDefenderPenetrateInner(), dataPool);
						break;
					case 100:
						result = Serializer.Serialize(instance.GetDefenderPenetrateResistOuter(), dataPool);
						break;
					case 101:
						result = Serializer.Serialize(instance.GetDefenderPenetrateResistInner(), dataPool);
						break;
					case 102:
						result = Serializer.Serialize(instance.GetAcceptDirectDamage(), dataPool);
						break;
					case 103:
						result = Serializer.Serialize(instance.GetAcceptBounceDamage(), dataPool);
						break;
					case 104:
						result = Serializer.Serialize(instance.GetAcceptFightBackDamage(), dataPool);
						break;
					case 105:
						result = Serializer.Serialize(instance.GetAcceptPoisonLevel(), dataPool);
						break;
					case 106:
						result = Serializer.Serialize(instance.GetAcceptPoisonValue(), dataPool);
						break;
					case 107:
						result = Serializer.Serialize(instance.GetDefenderHitOdds(), dataPool);
						break;
					case 108:
						result = Serializer.Serialize(instance.GetDefenderFightBackHitOdds(), dataPool);
						break;
					case 109:
						result = Serializer.Serialize(instance.GetDefenderPursueOdds(), dataPool);
						break;
					case 110:
						result = Serializer.Serialize(instance.GetAcceptMaxInjuryCount(), dataPool);
						break;
					case 111:
						result = Serializer.Serialize(instance.GetBouncePower(), dataPool);
						break;
					case 112:
						result = Serializer.Serialize(instance.GetFightBackPower(), dataPool);
						break;
					case 113:
						result = Serializer.Serialize(instance.GetDirectDamageInnerRatio(), dataPool);
						break;
					case 114:
						result = Serializer.Serialize(instance.GetDefenderFinalDamageValue(), dataPool);
						break;
					case 115:
						result = Serializer.Serialize(instance.GetDirectDamageValue(), dataPool);
						break;
					case 116:
						result = Serializer.Serialize(instance.GetDirectInjuryMark(), dataPool);
						break;
					case 117:
						result = Serializer.Serialize(instance.GetGoneMadInjury(), dataPool);
						break;
					case 118:
						result = Serializer.Serialize(instance.GetHealInjurySpeed(), dataPool);
						break;
					case 119:
						result = Serializer.Serialize(instance.GetHealInjuryBuff(), dataPool);
						break;
					case 120:
						result = Serializer.Serialize(instance.GetHealInjuryDebuff(), dataPool);
						break;
					case 121:
						result = Serializer.Serialize(instance.GetHealPoisonSpeed(), dataPool);
						break;
					case 122:
						result = Serializer.Serialize(instance.GetHealPoisonBuff(), dataPool);
						break;
					case 123:
						result = Serializer.Serialize(instance.GetHealPoisonDebuff(), dataPool);
						break;
					case 124:
						result = Serializer.Serialize(instance.GetFleeSpeed(), dataPool);
						break;
					case 125:
						result = Serializer.Serialize(instance.GetMaxFlawCount(), dataPool);
						break;
					case 126:
						result = Serializer.Serialize(instance.GetCanAddFlaw(), dataPool);
						break;
					case 127:
						result = Serializer.Serialize(instance.GetFlawLevel(), dataPool);
						break;
					case 128:
						result = Serializer.Serialize(instance.GetFlawLevelCanReduce(), dataPool);
						break;
					case 129:
						result = Serializer.Serialize(instance.GetFlawCount(), dataPool);
						break;
					case 130:
						result = Serializer.Serialize(instance.GetMaxAcupointCount(), dataPool);
						break;
					case 131:
						result = Serializer.Serialize(instance.GetCanAddAcupoint(), dataPool);
						break;
					case 132:
						result = Serializer.Serialize(instance.GetAcupointLevel(), dataPool);
						break;
					case 133:
						result = Serializer.Serialize(instance.GetAcupointLevelCanReduce(), dataPool);
						break;
					case 134:
						result = Serializer.Serialize(instance.GetAcupointCount(), dataPool);
						break;
					case 135:
						result = Serializer.Serialize(instance.GetAddNeiliAllocation(), dataPool);
						break;
					case 136:
						result = Serializer.Serialize(instance.GetCostNeiliAllocation(), dataPool);
						break;
					case 137:
						result = Serializer.Serialize(instance.GetCanChangeNeiliAllocation(), dataPool);
						break;
					case 138:
						result = Serializer.Serialize(instance.GetCanGetTrick(), dataPool);
						break;
					case 139:
						result = Serializer.Serialize(instance.GetGetTrickType(), dataPool);
						break;
					case 140:
						result = Serializer.Serialize(instance.GetAttackBodyPart(), dataPool);
						break;
					case 141:
						result = Serializer.Serialize(instance.GetWeaponEquipAttack(), dataPool);
						break;
					case 142:
						result = Serializer.Serialize(instance.GetWeaponEquipDefense(), dataPool);
						break;
					case 143:
						result = Serializer.Serialize(instance.GetArmorEquipAttack(), dataPool);
						break;
					case 144:
						result = Serializer.Serialize(instance.GetArmorEquipDefense(), dataPool);
						break;
					case 145:
						result = Serializer.Serialize(instance.GetAttackRangeForward(), dataPool);
						break;
					case 146:
						result = Serializer.Serialize(instance.GetAttackRangeBackward(), dataPool);
						break;
					case 147:
						result = Serializer.Serialize(instance.GetMoveCanBeStopped(), dataPool);
						break;
					case 148:
						result = Serializer.Serialize(instance.GetCanForcedMove(), dataPool);
						break;
					case 149:
						result = Serializer.Serialize(instance.GetMobilityCanBeRemoved(), dataPool);
						break;
					case 150:
						result = Serializer.Serialize(instance.GetMobilityCostByEffect(), dataPool);
						break;
					case 151:
						result = Serializer.Serialize(instance.GetMoveDistance(), dataPool);
						break;
					case 152:
						result = Serializer.Serialize(instance.GetJumpPrepareFrame(), dataPool);
						break;
					case 153:
						result = Serializer.Serialize(instance.GetBounceInjuryMark(), dataPool);
						break;
					case 154:
						result = Serializer.Serialize(instance.GetSkillHasCost(), dataPool);
						break;
					case 155:
						result = Serializer.Serialize(instance.GetCombatStateEffect(), dataPool);
						break;
					case 156:
						result = Serializer.Serialize(instance.GetChangeNeedUseSkill(), dataPool);
						break;
					case 157:
						result = Serializer.Serialize(instance.GetChangeDistanceIsMove(), dataPool);
						break;
					case 158:
						result = Serializer.Serialize(instance.GetReplaceCharHit(), dataPool);
						break;
					case 159:
						result = Serializer.Serialize(instance.GetCanAddPoison(), dataPool);
						break;
					case 160:
						result = Serializer.Serialize(instance.GetCanReducePoison(), dataPool);
						break;
					case 161:
						result = Serializer.Serialize(instance.GetReducePoisonValue(), dataPool);
						break;
					case 162:
						result = Serializer.Serialize(instance.GetPoisonCanAffect(), dataPool);
						break;
					case 163:
						result = Serializer.Serialize(instance.GetPoisonAffectCount(), dataPool);
						break;
					case 164:
						result = Serializer.Serialize(instance.GetCostTricks(), dataPool);
						break;
					case 165:
						result = Serializer.Serialize(instance.GetJumpMoveDistance(), dataPool);
						break;
					case 166:
						result = Serializer.Serialize(instance.GetCombatStateToAdd(), dataPool);
						break;
					case 167:
						result = Serializer.Serialize(instance.GetCombatStatePower(), dataPool);
						break;
					case 168:
						result = Serializer.Serialize(instance.GetBreakBodyPartInjuryCount(), dataPool);
						break;
					case 169:
						result = Serializer.Serialize(instance.GetBodyPartIsBroken(), dataPool);
						break;
					case 170:
						result = Serializer.Serialize(instance.GetMaxTrickCount(), dataPool);
						break;
					case 171:
						result = Serializer.Serialize(instance.GetMaxBreathPercent(), dataPool);
						break;
					case 172:
						result = Serializer.Serialize(instance.GetMaxStancePercent(), dataPool);
						break;
					case 173:
						result = Serializer.Serialize(instance.GetExtraBreathPercent(), dataPool);
						break;
					case 174:
						result = Serializer.Serialize(instance.GetExtraStancePercent(), dataPool);
						break;
					case 175:
						result = Serializer.Serialize(instance.GetMoveCostMobility(), dataPool);
						break;
					case 176:
						result = Serializer.Serialize(instance.GetDefendSkillKeepTime(), dataPool);
						break;
					case 177:
						result = Serializer.Serialize(instance.GetBounceRange(), dataPool);
						break;
					case 178:
						result = Serializer.Serialize(instance.GetMindMarkKeepTime(), dataPool);
						break;
					case 179:
						result = Serializer.Serialize(instance.GetSkillMobilityCostPerFrame(), dataPool);
						break;
					case 180:
						result = Serializer.Serialize(instance.GetCanAddWug(), dataPool);
						break;
					case 181:
						result = Serializer.Serialize(instance.GetHasGodWeaponBuff(), dataPool);
						break;
					case 182:
						result = Serializer.Serialize(instance.GetHasGodArmorBuff(), dataPool);
						break;
					case 183:
						result = Serializer.Serialize(instance.GetTeammateCmdRequireGenerateValue(), dataPool);
						break;
					case 184:
						result = Serializer.Serialize(instance.GetTeammateCmdEffect(), dataPool);
						break;
					case 185:
						result = Serializer.Serialize(instance.GetFlawRecoverSpeed(), dataPool);
						break;
					case 186:
						result = Serializer.Serialize(instance.GetAcupointRecoverSpeed(), dataPool);
						break;
					case 187:
						result = Serializer.Serialize(instance.GetMindMarkRecoverSpeed(), dataPool);
						break;
					case 188:
						result = Serializer.Serialize(instance.GetInjuryAutoHealSpeed(), dataPool);
						break;
					case 189:
						result = Serializer.Serialize(instance.GetCanRecoverBreath(), dataPool);
						break;
					case 190:
						result = Serializer.Serialize(instance.GetCanRecoverStance(), dataPool);
						break;
					case 191:
						result = Serializer.Serialize(instance.GetFatalDamageValue(), dataPool);
						break;
					case 192:
						result = Serializer.Serialize(instance.GetFatalDamageMarkCount(), dataPool);
						break;
					case 193:
						result = Serializer.Serialize(instance.GetCanFightBackDuringPrepareSkill(), dataPool);
						break;
					case 194:
						result = Serializer.Serialize(instance.GetSkillPrepareSpeed(), dataPool);
						break;
					case 195:
						result = Serializer.Serialize(instance.GetBreathRecoverSpeed(), dataPool);
						break;
					case 196:
						result = Serializer.Serialize(instance.GetStanceRecoverSpeed(), dataPool);
						break;
					case 197:
						result = Serializer.Serialize(instance.GetMobilityRecoverSpeed(), dataPool);
						break;
					case 198:
						result = Serializer.Serialize(instance.GetChangeTrickProgressAddValue(), dataPool);
						break;
					case 199:
						result = Serializer.Serialize(instance.GetPower(), dataPool);
						break;
					case 200:
						result = Serializer.Serialize(instance.GetMaxPower(), dataPool);
						break;
					case 201:
						result = Serializer.Serialize(instance.GetPowerCanReduce(), dataPool);
						break;
					case 202:
						result = Serializer.Serialize(instance.GetUseRequirement(), dataPool);
						break;
					case 203:
						result = Serializer.Serialize(instance.GetCurrInnerRatio(), dataPool);
						break;
					case 204:
						result = Serializer.Serialize(instance.GetCostBreathAndStance(), dataPool);
						break;
					case 205:
						result = Serializer.Serialize(instance.GetCostBreath(), dataPool);
						break;
					case 206:
						result = Serializer.Serialize(instance.GetCostStance(), dataPool);
						break;
					case 207:
						result = Serializer.Serialize(instance.GetCostMobility(), dataPool);
						break;
					case 208:
						result = Serializer.Serialize(instance.GetSkillCostTricks(), dataPool);
						break;
					case 209:
						result = Serializer.Serialize(instance.GetEffectDirection(), dataPool);
						break;
					case 210:
						result = Serializer.Serialize(instance.GetEffectDirectionCanChange(), dataPool);
						break;
					case 211:
						result = Serializer.Serialize(instance.GetGridCost(), dataPool);
						break;
					case 212:
						result = Serializer.Serialize(instance.GetPrepareTotalProgress(), dataPool);
						break;
					case 213:
						result = Serializer.Serialize(instance.GetSpecificGridCount(), dataPool);
						break;
					case 214:
						result = Serializer.Serialize(instance.GetGenericGridCount(), dataPool);
						break;
					case 215:
						result = Serializer.Serialize(instance.GetCanInterrupt(), dataPool);
						break;
					case 216:
						result = Serializer.Serialize(instance.GetInterruptOdds(), dataPool);
						break;
					case 217:
						result = Serializer.Serialize(instance.GetCanSilence(), dataPool);
						break;
					case 218:
						result = Serializer.Serialize(instance.GetSilenceOdds(), dataPool);
						break;
					case 219:
						result = Serializer.Serialize(instance.GetCanCastWithBrokenBodyPart(), dataPool);
						break;
					case 220:
						result = Serializer.Serialize(instance.GetAddPowerCanBeRemoved(), dataPool);
						break;
					case 221:
						result = Serializer.Serialize(instance.GetSkillType(), dataPool);
						break;
					case 222:
						result = Serializer.Serialize(instance.GetEffectCountCanChange(), dataPool);
						break;
					case 223:
						result = Serializer.Serialize(instance.GetCanCastInDefend(), dataPool);
						break;
					case 224:
						result = Serializer.Serialize(instance.GetHitDistribution(), dataPool);
						break;
					case 225:
						result = Serializer.Serialize(instance.GetCanCastOnLackBreath(), dataPool);
						break;
					case 226:
						result = Serializer.Serialize(instance.GetCanCastOnLackStance(), dataPool);
						break;
					case 227:
						result = Serializer.Serialize(instance.GetCostBreathOnCast(), dataPool);
						break;
					case 228:
						result = Serializer.Serialize(instance.GetCostStanceOnCast(), dataPool);
						break;
					case 229:
						result = Serializer.Serialize(instance.GetCanUseMobilityAsBreath(), dataPool);
						break;
					case 230:
						result = Serializer.Serialize(instance.GetCanUseMobilityAsStance(), dataPool);
						break;
					case 231:
						result = Serializer.Serialize(instance.GetCastCostNeiliAllocation(), dataPool);
						break;
					case 232:
						result = Serializer.Serialize(instance.GetAcceptPoisonResist(), dataPool);
						break;
					case 233:
						result = Serializer.Serialize(instance.GetMakePoisonResist(), dataPool);
						break;
					case 234:
						result = Serializer.Serialize(instance.GetCanCriticalHit(), dataPool);
						break;
					case 235:
						result = Serializer.Serialize(instance.GetCanCostNeiliAllocationEffect(), dataPool);
						break;
					case 236:
						result = Serializer.Serialize(instance.GetConsummateLevelRelatedMainAttributesHitValues(), dataPool);
						break;
					case 237:
						result = Serializer.Serialize(instance.GetConsummateLevelRelatedMainAttributesAvoidValues(), dataPool);
						break;
					case 238:
						result = Serializer.Serialize(instance.GetConsummateLevelRelatedMainAttributesPenetrations(), dataPool);
						break;
					case 239:
						result = Serializer.Serialize(instance.GetConsummateLevelRelatedMainAttributesPenetrationResists(), dataPool);
						break;
					case 240:
						result = Serializer.Serialize(instance.GetSkillAlsoAsFiveElements(), dataPool);
						break;
					case 241:
						result = Serializer.Serialize(instance.GetInnerInjuryImmunity(), dataPool);
						break;
					case 242:
						result = Serializer.Serialize(instance.GetOuterInjuryImmunity(), dataPool);
						break;
					case 243:
						result = Serializer.Serialize(instance.GetPoisonAffectThreshold(), dataPool);
						break;
					case 244:
						result = Serializer.Serialize(instance.GetLockDistance(), dataPool);
						break;
					case 245:
						result = Serializer.Serialize(instance.GetResistOfAllPoison(), dataPool);
						break;
					case 246:
						result = Serializer.Serialize(instance.GetMakePoisonTarget(), dataPool);
						break;
					case 247:
						result = Serializer.Serialize(instance.GetAcceptPoisonTarget(), dataPool);
						break;
					case 248:
						result = Serializer.Serialize(instance.GetCertainCriticalHit(), dataPool);
						break;
					case 249:
						result = Serializer.Serialize(instance.GetMindMarkCount(), dataPool);
						break;
					case 250:
						result = Serializer.Serialize(instance.GetCanFightBackWithHit(), dataPool);
						break;
					case 251:
						result = Serializer.Serialize(instance.GetInevitableHit(), dataPool);
						break;
					case 252:
						result = Serializer.Serialize(instance.GetAttackCanPursue(), dataPool);
						break;
					case 253:
						result = Serializer.Serialize(instance.GetCombatSkillDataEffectList(), dataPool);
						break;
					case 254:
						result = Serializer.Serialize(instance.GetCriticalOdds(), dataPool);
						break;
					case 255:
						result = Serializer.Serialize(instance.GetStanceCostByEffect(), dataPool);
						break;
					case 256:
						result = Serializer.Serialize(instance.GetBreathCostByEffect(), dataPool);
						break;
					case 257:
						result = Serializer.Serialize(instance.GetPowerAddRatio(), dataPool);
						break;
					case 258:
						result = Serializer.Serialize(instance.GetPowerReduceRatio(), dataPool);
						break;
					case 259:
						result = Serializer.Serialize(instance.GetPoisonAffectProduceValue(), dataPool);
						break;
					case 260:
						result = Serializer.Serialize(instance.GetCanReadingOnMonthChange(), dataPool);
						break;
					case 261:
						result = Serializer.Serialize(instance.GetMedicineEffect(), dataPool);
						break;
					case 262:
						result = Serializer.Serialize(instance.GetXiangshuInfectionDelta(), dataPool);
						break;
					case 263:
						result = Serializer.Serialize(instance.GetHealthDelta(), dataPool);
						break;
					case 264:
						result = Serializer.Serialize(instance.GetWeaponSilenceFrame(), dataPool);
						break;
					case 265:
						result = Serializer.Serialize(instance.GetSilenceFrame(), dataPool);
						break;
					case 266:
						result = Serializer.Serialize(instance.GetCurrAgeDelta(), dataPool);
						break;
					case 267:
						result = Serializer.Serialize(instance.GetGoneMadInAllBreak(), dataPool);
						break;
					case 268:
						result = Serializer.Serialize(instance.GetMakeLoveRateOnMonthChange(), dataPool);
						break;
					case 269:
						result = Serializer.Serialize(instance.GetCanAutoHealOnMonthChange(), dataPool);
						break;
					case 270:
						result = Serializer.Serialize(instance.GetHappinessDelta(), dataPool);
						break;
					case 271:
						result = Serializer.Serialize(instance.GetTeammateCmdCanUse(), dataPool);
						break;
					case 272:
						result = Serializer.Serialize(instance.GetMixPoisonInfinityAffect(), dataPool);
						break;
					case 273:
						result = Serializer.Serialize(instance.GetAttackRangeMaxAcupoint(), dataPool);
						break;
					case 274:
						result = Serializer.Serialize(instance.GetMaxMobilityPercent(), dataPool);
						break;
					case 275:
						result = Serializer.Serialize(instance.GetMakeMindDamage(), dataPool);
						break;
					case 276:
						result = Serializer.Serialize(instance.GetAcceptMindDamage(), dataPool);
						break;
					case 277:
						result = Serializer.Serialize(instance.GetHitAddByTempValue(), dataPool);
						break;
					case 278:
						result = Serializer.Serialize(instance.GetAvoidAddByTempValue(), dataPool);
						break;
					case 279:
						result = Serializer.Serialize(instance.GetIgnoreEquipmentOverload(), dataPool);
						break;
					case 280:
						result = Serializer.Serialize(instance.GetCanCostEnemyUsableTricks(), dataPool);
						break;
					case 281:
						result = Serializer.Serialize(instance.GetIgnoreArmor(), dataPool);
						break;
					case 282:
						result = Serializer.Serialize(instance.GetUnyieldingFallen(), dataPool);
						break;
					case 283:
						result = Serializer.Serialize(instance.GetNormalAttackPrepareFrame(), dataPool);
						break;
					case 284:
						result = Serializer.Serialize(instance.GetCanCostUselessTricks(), dataPool);
						break;
					case 285:
						result = Serializer.Serialize(instance.GetDefendSkillCanAffect(), dataPool);
						break;
					case 286:
						result = Serializer.Serialize(instance.GetAssistSkillCanAffect(), dataPool);
						break;
					case 287:
						result = Serializer.Serialize(instance.GetAgileSkillCanAffect(), dataPool);
						break;
					case 288:
						result = Serializer.Serialize(instance.GetAllMarkChangeToMind(), dataPool);
						break;
					case 289:
						result = Serializer.Serialize(instance.GetMindMarkChangeToFatal(), dataPool);
						break;
					case 290:
						result = Serializer.Serialize(instance.GetCanCast(), dataPool);
						break;
					case 291:
						result = Serializer.Serialize(instance.GetInevitableAvoid(), dataPool);
						break;
					case 292:
						result = Serializer.Serialize(instance.GetPowerEffectReverse(), dataPool);
						break;
					case 293:
						result = Serializer.Serialize(instance.GetFeatureBonusReverse(), dataPool);
						break;
					case 294:
						result = Serializer.Serialize(instance.GetWugFatalDamageValue(), dataPool);
						break;
					case 295:
						result = Serializer.Serialize(instance.GetCanRecoverHealthOnMonthChange(), dataPool);
						break;
					case 296:
						result = Serializer.Serialize(instance.GetTakeRevengeRateOnMonthChange(), dataPool);
						break;
					case 297:
						result = Serializer.Serialize(instance.GetConsummateLevelBonus(), dataPool);
						break;
					case 298:
						result = Serializer.Serialize(instance.GetNeiliDelta(), dataPool);
						break;
					case 299:
						result = Serializer.Serialize(instance.GetCanMakeLoveSpecialOnMonthChange(), dataPool);
						break;
					case 300:
						result = Serializer.Serialize(instance.GetHealAcupointSpeed(), dataPool);
						break;
					case 301:
						result = Serializer.Serialize(instance.GetMaxChangeTrickCount(), dataPool);
						break;
					case 302:
						result = Serializer.Serialize(instance.GetConvertCostBreathAndStance(), dataPool);
						break;
					case 303:
						result = Serializer.Serialize(instance.GetPersonalitiesAll(), dataPool);
						break;
					case 304:
						result = Serializer.Serialize(instance.GetFinalFatalDamageMarkCount(), dataPool);
						break;
					case 305:
						result = Serializer.Serialize(instance.GetInfinityMindMarkProgress(), dataPool);
						break;
					case 306:
						result = Serializer.Serialize(instance.GetCombatSkillAiScorePower(), dataPool);
						break;
					case 307:
						result = Serializer.Serialize(instance.GetNormalAttackChangeToUnlockAttack(), dataPool);
						break;
					case 308:
						result = Serializer.Serialize(instance.GetAttackBodyPartOdds(), dataPool);
						break;
					case 309:
						result = Serializer.Serialize(instance.GetChangeDurability(), dataPool);
						break;
					case 310:
						result = Serializer.Serialize(instance.GetEquipmentBonus(), dataPool);
						break;
					case 311:
						result = Serializer.Serialize(instance.GetEquipmentWeight(), dataPool);
						break;
					case 312:
						result = Serializer.Serialize(instance.GetRawCreateEffectList(), dataPool);
						break;
					case 313:
						result = Serializer.Serialize(instance.GetJiTrickAsWeaponTrickCount(), dataPool);
						break;
					case 314:
						result = Serializer.Serialize(instance.GetUselessTrickAsJiTrickCount(), dataPool);
						break;
					case 315:
						result = Serializer.Serialize(instance.GetEquipmentPower(), dataPool);
						break;
					case 316:
						result = Serializer.Serialize(instance.GetHealFlawSpeed(), dataPool);
						break;
					case 317:
						result = Serializer.Serialize(instance.GetUnlockSpeed(), dataPool);
						break;
					case 318:
						result = Serializer.Serialize(instance.GetFlawBonusFactor(), dataPool);
						break;
					case 319:
						result = Serializer.Serialize(instance.GetCanCostShaTricks(), dataPool);
						break;
					case 320:
						result = Serializer.Serialize(instance.GetDefenderDirectFinalDamageValue(), dataPool);
						break;
					case 321:
						result = Serializer.Serialize(instance.GetNormalAttackRecoveryFrame(), dataPool);
						break;
					case 322:
						result = Serializer.Serialize(instance.GetFinalGoneMadInjury(), dataPool);
						break;
					case 323:
						result = Serializer.Serialize(instance.GetAttackerDirectFinalDamageValue(), dataPool);
						break;
					case 324:
						result = Serializer.Serialize(instance.GetCanCostTrickDuringPreparingSkill(), dataPool);
						break;
					case 325:
						result = Serializer.Serialize(instance.GetValidItemList(), dataPool);
						break;
					case 326:
						result = Serializer.Serialize(instance.GetAcceptDamageCanAdd(), dataPool);
						break;
					case 327:
						result = Serializer.Serialize(instance.GetMakeDamageCanReduce(), dataPool);
						break;
					case 328:
						result = Serializer.Serialize(instance.GetNormalAttackGetTrickCount(), dataPool);
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

		// Token: 0x0600293D RID: 10557 RVA: 0x001FA73C File Offset: 0x001F893C
		private void ResetModifiedWrapper_AffectedDatas(int objectId, ushort fieldId)
		{
			AffectedData instance;
			bool flag = !this._affectedDatas.TryGetValue(objectId, out instance);
			if (!flag)
			{
				bool flag2 = fieldId >= 329;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesAffectedDatas.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (!flag3)
				{
					this._dataStatesAffectedDatas.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
			}
		}

		// Token: 0x0600293E RID: 10558 RVA: 0x001FA7D0 File Offset: 0x001F89D0
		private bool IsModifiedWrapper_AffectedDatas(int objectId, ushort fieldId)
		{
			AffectedData instance;
			bool flag = !this._affectedDatas.TryGetValue(objectId, out instance);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = fieldId >= 329;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = this._dataStatesAffectedDatas.IsModified(instance.DataStatesOffset, (int)fieldId);
			}
			return result;
		}

		// Token: 0x0600293F RID: 10559 RVA: 0x001FA849 File Offset: 0x001F8A49
		public override void OnInitializeGameDataModule()
		{
			this.InitializeOnInitializeGameDataModule();
		}

		// Token: 0x06002940 RID: 10560 RVA: 0x001FA854 File Offset: 0x001F8A54
		public unsafe override void OnEnterNewWorld()
		{
			this.InitializeOnEnterNewWorld();
			this.InitializeInternalDataOfCollections();
			foreach (KeyValuePair<long, SpecialEffectWrapper> entry in this._effectDict)
			{
				long elementId = entry.Key;
				SpecialEffectWrapper value = entry.Value;
				bool flag = value != null;
				if (flag)
				{
					int contentSize = value.GetSerializedSize();
					byte* pData = OperationAdder.DynamicSingleValueCollection_Add<long>(17, 0, elementId, contentSize);
					pData += value.Serialize(pData);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<long>(17, 0, elementId, 0);
				}
			}
			byte* pData2 = OperationAdder.FixedSingleValue_Set(17, 1, 8);
			*(long*)pData2 = this._nextEffectId;
			pData2 += 8;
		}

		// Token: 0x06002941 RID: 10561 RVA: 0x001FA91C File Offset: 0x001F8B1C
		public override void OnLoadWorld()
		{
			this._pendingLoadingOperationIds = new Queue<uint>();
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(17, 0));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(17, 1));
		}

		// Token: 0x06002942 RID: 10562 RVA: 0x001FA954 File Offset: 0x001F8B54
		public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
		{
			switch (dataId)
			{
			case 0:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to get value of dataId: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 1:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to get value of dataId: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
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
		}

		// Token: 0x06002943 RID: 10563 RVA: 0x001FAA30 File Offset: 0x001F8C30
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

		// Token: 0x06002944 RID: 10564 RVA: 0x001FAB0C File Offset: 0x001F8D0C
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
				short skillId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref skillId);
				List<CastBoostEffectDisplayData> returnValue = this.GetAllCostNeiliEffectData(charId, skillId);
				result = Serializer.Serialize(returnValue, returnDataPool);
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
				short skillId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref skillId2);
				short effectId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref effectId);
				this.CostNeiliEffect(context, charId2, skillId2, effectId);
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
				short skillId3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref skillId3);
				bool returnValue2 = this.CanCostTrickDuringPreparingSkill(charId3, skillId3);
				result = Serializer.Serialize(returnValue2, returnDataPool);
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
				int trickIndex = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref trickIndex);
				bool returnValue3 = this.CostTrickDuringPreparingSkill(context, charId4, trickIndex);
				result = Serializer.Serialize(returnValue3, returnDataPool);
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

		// Token: 0x06002945 RID: 10565 RVA: 0x001FAD84 File Offset: 0x001F8F84
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
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06002946 RID: 10566 RVA: 0x001FADE0 File Offset: 0x001F8FE0
		public override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
		{
			switch (dataId)
			{
			case 0:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 1:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
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
		}

		// Token: 0x06002947 RID: 10567 RVA: 0x001FAEBC File Offset: 0x001F90BC
		public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			switch (dataId)
			{
			case 0:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 1:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
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

		// Token: 0x06002948 RID: 10568 RVA: 0x001FAF98 File Offset: 0x001F9198
		public override bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			switch (dataId)
			{
			case 0:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to verify modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 1:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to verify modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
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
		}

		// Token: 0x06002949 RID: 10569 RVA: 0x001FB074 File Offset: 0x001F9274
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
					bool influenceAll = InfluenceChecker.GetScope<int, AffectedData>(context, sourceObject, influence.Scope, this._affectedDatas, influencedObjects);
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
						BaseGameDataDomain.InvalidateAllAndInfluencedCaches(SpecialEffectDomain.CacheInfluencesAffectedDatas, this._dataStatesAffectedDatas, influence, context);
					}
					influencedObjects.Clear();
					InfluenceChecker.InfluencedObjectsPool.Return(influencedObjects);
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(SpecialEffectDomain.CacheInfluencesAffectedDatas, this._dataStatesAffectedDatas, influence, context);
				}
				return;
			}
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

		// Token: 0x0600294A RID: 10570 RVA: 0x001FB210 File Offset: 0x001F9410
		public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
		{
			switch (operation.DataId)
			{
			case 0:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<long, SpecialEffectWrapper>(operation, pResult, this._effectDict);
				break;
			case 1:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single<long>(operation, pResult, ref this._nextEffectId);
				break;
			case 2:
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
						DomainManager.Global.CompleteLoading(17);
					}
				}
			}
		}

		// Token: 0x0600294B RID: 10571 RVA: 0x001FB340 File Offset: 0x001F9540
		private void InitializeInternalDataOfCollections()
		{
			foreach (KeyValuePair<int, AffectedData> entry in this._affectedDatas)
			{
				AffectedData instance = entry.Value;
				instance.CollectionHelperData = this.HelperDataAffectedDatas;
				instance.DataStatesOffset = this._dataStatesAffectedDatas.Create();
			}
		}

		// Token: 0x0600294C RID: 10572 RVA: 0x001FB3B8 File Offset: 0x001F95B8
		public static void RegisterResetHandler(Action action)
		{
			SpecialEffectDomain.ResetOnInitializeGameDataModuleEffects.Add(action);
		}

		// Token: 0x0600294D RID: 10573 RVA: 0x001FB3C8 File Offset: 0x001F95C8
		private static void InvokeResetHandlers()
		{
			foreach (Action effectHandler in SpecialEffectDomain.ResetOnInitializeGameDataModuleEffects)
			{
				effectHandler();
			}
		}

		// Token: 0x0600294E RID: 10574 RVA: 0x001FB420 File Offset: 0x001F9620
		// Note: this type is marked as 'beforefieldinit'.
		static SpecialEffectDomain()
		{
			Dictionary<short, string> dictionary = new Dictionary<short, string>();
			dictionary[246] = "CombatSkill.Xuehoujiao.BreakBodyEffect.HeadHurt";
			dictionary[247] = "CombatSkill.Xuehoujiao.BreakBodyEffect.HeadCrash";
			dictionary[248] = "CombatSkill.Xuehoujiao.BreakBodyEffect.ChestHurt";
			dictionary[249] = "CombatSkill.Xuehoujiao.BreakBodyEffect.ChestCrash";
			dictionary[250] = "CombatSkill.Xuehoujiao.BreakBodyEffect.BellyHurt";
			dictionary[251] = "CombatSkill.Xuehoujiao.BreakBodyEffect.BellyCrash";
			dictionary[252] = "CombatSkill.Xuehoujiao.BreakBodyEffect.HandHurt";
			dictionary[253] = "CombatSkill.Xuehoujiao.BreakBodyEffect.HandCrash";
			dictionary[254] = "CombatSkill.Xuehoujiao.BreakBodyEffect.LegHurt";
			dictionary[255] = "CombatSkill.Xuehoujiao.BreakBodyEffect.LegCrash";
			SpecialEffectDomain.BreakBodyFeatureEffectClassName = dictionary;
			SpecialEffectDomain.CacheInfluences = new DataInfluence[3][];
			SpecialEffectDomain.CacheInfluencesAffectedDatas = new DataInfluence[329][];
			SpecialEffectDomain.ResetOnInitializeGameDataModuleEffects = new List<Action>();
		}

		// Token: 0x0400082D RID: 2093
		[DomainData(DomainDataType.SingleValueCollection, true, false, false, false)]
		private Dictionary<long, SpecialEffectWrapper> _effectDict;

		// Token: 0x0400082E RID: 2094
		[DomainData(DomainDataType.SingleValue, true, false, false, false)]
		private long _nextEffectId;

		// Token: 0x0400082F RID: 2095
		[DomainData(DomainDataType.ObjectCollection, false, false, false, false)]
		private readonly Dictionary<int, AffectedData> _affectedDatas;

		// Token: 0x04000830 RID: 2096
		[TupleElementNames(new string[]
		{
			"effectId",
			"charId",
			"skillId"
		})]
		private readonly ConcurrentBag<ValueTuple<long, int, short>> _brokenEffectsChangedDuringAdvance = new ConcurrentBag<ValueTuple<long, int, short>>();

		// Token: 0x04000831 RID: 2097
		private readonly Dictionary<long, long> _featureEffectDict = new Dictionary<long, long>();

		// Token: 0x04000832 RID: 2098
		private readonly List<CastBoostEffectDisplayData> _costNeiliEffectDisplayDataCache = new List<CastBoostEffectDisplayData>();

		// Token: 0x04000833 RID: 2099
		public static readonly Dictionary<short, string> BreakBodyFeatureEffectClassName;

		// Token: 0x04000834 RID: 2100
		private bool _updatingErrorEffect;

		// Token: 0x04000835 RID: 2101
		private readonly HashSet<int> _requestUpdateCombatSkillCharIds = new HashSet<int>();

		// Token: 0x04000836 RID: 2102
		private static readonly DataInfluence[][] CacheInfluences;

		// Token: 0x04000837 RID: 2103
		private static readonly DataInfluence[][] CacheInfluencesAffectedDatas;

		// Token: 0x04000838 RID: 2104
		private readonly ObjectCollectionDataStates _dataStatesAffectedDatas = new ObjectCollectionDataStates(329, 0);

		// Token: 0x04000839 RID: 2105
		public readonly ObjectCollectionHelperData HelperDataAffectedDatas;

		// Token: 0x0400083A RID: 2106
		private Queue<uint> _pendingLoadingOperationIds;

		// Token: 0x0400083B RID: 2107
		private static readonly List<Action> ResetOnInitializeGameDataModuleEffects;
	}
}
