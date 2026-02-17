using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect
{
	// Token: 0x02000343 RID: 835
	public class AzureMarrowBase : WugEffectBase
	{
		// Token: 0x060034E7 RID: 13543 RVA: 0x0022A864 File Offset: 0x00228A64
		protected AzureMarrowBase()
		{
		}

		// Token: 0x060034E8 RID: 13544 RVA: 0x0022A86E File Offset: 0x00228A6E
		protected AzureMarrowBase(int charId, int type, short wugTemplateId, short effectId) : base(charId, type, wugTemplateId, effectId)
		{
			this.CostWugCount = 28;
		}

		// Token: 0x060034E9 RID: 13545 RVA: 0x0022A888 File Offset: 0x00228A88
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			bool isGrown = base.IsGrown;
			if (isGrown)
			{
				base.CreateAffectedData(268, EDataModifyType.TotalPercent, -1);
				base.CreateAffectedData(299, EDataModifyType.Custom, -1);
			}
			else
			{
				base.CreateAffectedData(293, EDataModifyType.Add, -1);
			}
			bool isGrown2 = base.IsGrown;
			if (isGrown2)
			{
				Events.RegisterHandler_AdvanceMonthFinish(new Events.OnAdvanceMonthFinish(this.OnAdvanceMonthFinish));
			}
			Events.RegisterHandler_MakeLove(new Events.OnMakeLove(this.OnMakeLove));
		}

		// Token: 0x060034EA RID: 13546 RVA: 0x0022A904 File Offset: 0x00228B04
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			bool isGrown = base.IsGrown;
			if (isGrown)
			{
				Events.UnRegisterHandler_AdvanceMonthFinish(new Events.OnAdvanceMonthFinish(this.OnAdvanceMonthFinish));
			}
			Events.UnRegisterHandler_MakeLove(new Events.OnMakeLove(this.OnMakeLove));
		}

		// Token: 0x060034EB RID: 13547 RVA: 0x0022A948 File Offset: 0x00228B48
		protected override void AddAffectDataAndEvent(DataContext context)
		{
			Events.RegisterHandler_AddFatalDamageMark(new Events.OnAddFatalDamageMark(this.OnAddFatalDamageMark));
		}

		// Token: 0x060034EC RID: 13548 RVA: 0x0022A95D File Offset: 0x00228B5D
		protected override void ClearAffectDataAndEvent(DataContext context)
		{
			Events.UnRegisterHandler_AddFatalDamageMark(new Events.OnAddFatalDamageMark(this.OnAddFatalDamageMark));
		}

		// Token: 0x060034ED RID: 13549 RVA: 0x0022A974 File Offset: 0x00228B74
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				if (!true)
				{
				}
				int num;
				if (fieldId != 268)
				{
					if (fieldId != 293)
					{
						num = 0;
					}
					else
					{
						num = ((!base.IsElite) ? 0 : (base.IsGood ? 1 : -1));
					}
				}
				else
				{
					num = 100;
				}
				if (!true)
				{
				}
				result = num;
			}
			return result;
		}

		// Token: 0x060034EE RID: 13550 RVA: 0x0022A9F0 File Offset: 0x00228BF0
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 299 || !base.CanAffect;
			return !flag || dataValue;
		}

		// Token: 0x060034EF RID: 13551 RVA: 0x0022AA34 File Offset: 0x00228C34
		private void OnAdvanceMonthFinish(DataContext context)
		{
			bool flag = !base.CanAffect;
			if (!flag)
			{
				Location location = this.CharObj.GetLocation();
				bool flag2 = !location.IsValid();
				if (!flag2)
				{
					List<MapBlockData> neighborBlocks = ObjectPool<List<MapBlockData>>.Instance.Get();
					DomainManager.Map.GetRealNeighborBlocks(location.AreaId, location.BlockId, neighborBlocks, 1, true);
					IEnumerable<GameData.Domains.Character.Character> neighborChars = (from x in neighborBlocks
					where x.CharacterSet != null
					select x).SelectMany((MapBlockData x) => x.CharacterSet).Select(new Func<int, GameData.Domains.Character.Character>(DomainManager.Character.GetElement_Objects));
					HashSet<int> groupCharIds = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
					bool flag3 = groupCharIds.Contains(this.CharObj.GetId());
					if (flag3)
					{
						neighborChars = neighborChars.Union(groupCharIds.Select(new Func<int, GameData.Domains.Character.Character>(DomainManager.Character.GetElement_Objects)));
					}
					bool flag4 = neighborChars.All((GameData.Domains.Character.Character x) => x.GetGender() == this.CharObj.GetGender());
					if (flag4)
					{
						List<sbyte> poisonTypes = ObjectPool<List<sbyte>>.Instance.Get();
						poisonTypes.Clear();
						for (sbyte i = 0; i < 6; i += 1)
						{
							poisonTypes.Add(i);
						}
						CollectionUtils.Shuffle<sbyte>(context.Random, poisonTypes);
						for (int j = 0; j < 3; j++)
						{
							sbyte poisonType = poisonTypes[j];
							this.CharObj.ChangePoisoned(context, poisonType, 3, 3600);
						}
						ObjectPool<List<sbyte>>.Instance.Return(poisonTypes);
						LifeRecordCollection lifeRecord = DomainManager.LifeRecord.GetLifeRecordCollection();
						base.AddLifeRecord(new WugEffectBase.LifeRecordAddTemplate(lifeRecord.AddWugAzureMarrowAddPoison));
					}
					ObjectPool<List<MapBlockData>>.Instance.Return(neighborBlocks);
				}
			}
		}

		// Token: 0x060034F0 RID: 13552 RVA: 0x0022AC14 File Offset: 0x00228E14
		private unsafe void OnMakeLove(DataContext context, GameData.Domains.Character.Character character, GameData.Domains.Character.Character target, sbyte makeLoveState)
		{
			bool flag = !base.CanAffect || !base.CheckValid();
			if (!flag)
			{
				bool flag2 = character.GetId() != base.CharacterId && target.GetId() != base.CharacterId;
				if (!flag2)
				{
					bool flag3 = makeLoveState == 4;
					if (!flag3)
					{
						bool flag4 = character.GetGender() == target.GetGender();
						if (!flag4)
						{
							GameData.Domains.Character.Character addWugChar = (character.GetId() == base.CharacterId) ? target : character;
							int addWugCharId = addWugChar.GetId();
							EatingItems eatingItems = *addWugChar.GetEatingItems();
							LifeRecordCollection lifeRecord = DomainManager.LifeRecord.GetLifeRecordCollection();
							int existIndex = eatingItems.IndexOfWug(this.WugConfig.WugType, false);
							MedicineItem existConfig = (existIndex < 0) ? null : Config.Medicine.Instance[eatingItems.Get(existIndex).TemplateId];
							bool flag5;
							if (base.IsGrown && base.IsElite)
							{
								sbyte? b = (existConfig != null) ? new sbyte?(existConfig.WugGrowthType) : null;
								int? num = (b != null) ? new int?((int)b.GetValueOrDefault()) : null;
								int num2 = 4;
								flag5 = !(num.GetValueOrDefault() == num2 & num != null);
							}
							else
							{
								flag5 = false;
							}
							bool flag6 = flag5;
							if (flag6)
							{
								short addWugTemplateId = ItemDomain.GetWugTemplateId(this.WugConfig.WugType, 3);
								addWugChar.AddWug(context, addWugTemplateId);
								base.AddLifeRecord(new WugEffectBase.LifeRecordRelatedAddTemplate(lifeRecord.AddWugAzureMarrowAddWug), addWugCharId, addWugTemplateId);
							}
							else
							{
								bool canChangeToGrown = base.CanChangeToGrown;
								if (canChangeToGrown)
								{
									short grownId = ItemDomain.GetWugTemplateId(this.WugConfig.WugType, 4);
									bool flag7 = WugGrowthType.CanChangeToGrown((existConfig != null) ? existConfig.WugGrowthType : -1);
									if (flag7)
									{
										this.CharObj.AddWug(context, grownId);
										base.AddLifeRecord(new WugEffectBase.LifeRecordRelatedAddTemplate(lifeRecord.AddWugAzureMarrowChangeToGrown), addWugCharId, base.CharacterId, grownId);
									}
									else
									{
										this.CharObj.RemoveWug(context, this.WugConfig.TemplateId);
									}
									addWugChar.AddWug(context, grownId);
									base.AddLifeRecord(new WugEffectBase.LifeRecordRelatedAddTemplate(lifeRecord.AddWugAzureMarrowChangeToGrown), addWugChar.GetId(), grownId);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060034F1 RID: 13553 RVA: 0x0022AE58 File Offset: 0x00229058
		private void OnAddFatalDamageMark(DataContext context, CombatCharacter combatChar, int count)
		{
			bool flag = combatChar.GetId() != base.CharacterId || this._affected || !base.CanAffect;
			if (!flag)
			{
				this._affected = true;
				Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnCombatStateMachineUpdateEnd));
			}
		}

		// Token: 0x060034F2 RID: 13554 RVA: 0x0022AEA8 File Offset: 0x002290A8
		private unsafe void OnCombatStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			bool flag = combatChar.GetId() != base.CharacterId;
			if (!flag)
			{
				Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnCombatStateMachineUpdateEnd));
				this._affected = false;
				List<short> prefer = ObjectPool<List<short>>.Instance.Get();
				List<short> normal = ObjectPool<List<short>>.Instance.Get();
				prefer.Clear();
				normal.Clear();
				EatingItems eatingItems = *this.CharObj.GetEatingItems();
				for (sbyte wugType = 0; wugType < 8; wugType += 1)
				{
					bool flag2 = wugType == this.WugConfig.WugType;
					if (!flag2)
					{
						int index = eatingItems.IndexOfWug(wugType, false);
						bool flag3 = index < 0;
						if (!flag3)
						{
							sbyte oldWugGrowthType = Config.Medicine.Instance[eatingItems.Get(index).TemplateId].WugGrowthType;
							sbyte newWugGrowthType = this.GetChangedWugGrowthType(oldWugGrowthType);
							bool flag4 = newWugGrowthType < 0;
							if (!flag4)
							{
								short newWug = ItemDomain.GetWugTemplateId(wugType, newWugGrowthType);
								bool flag5 = WugGrowthType.IsWugGrowthTypeCombatOnly(oldWugGrowthType);
								if (flag5)
								{
									prefer.Add(newWug);
								}
								else
								{
									normal.Add(newWug);
								}
							}
						}
					}
				}
				bool affected = false;
				foreach (short newWug2 in RandomUtils.GetRandomUnrepeated<short>(context.Random, 3, prefer, normal))
				{
					DomainManager.Combat.AddWug(context, combatChar, newWug2, base.CharacterId, EWugReplaceType.All);
					base.ShowEffectTips(context, (base.IsGood && normal.Contains(newWug2)) ? 2 : 1);
					affected = true;
				}
				bool flag6 = affected;
				if (flag6)
				{
					base.CostWugInCombat(context);
				}
				ObjectPool<List<short>>.Instance.Return(prefer);
				ObjectPool<List<short>>.Instance.Return(normal);
			}
		}

		// Token: 0x060034F3 RID: 13555 RVA: 0x0022B07C File Offset: 0x0022927C
		private sbyte GetChangedWugGrowthType(sbyte oldGrowthType)
		{
			bool flag = WugGrowthType.IsWugGrowthTypeCombatOnly(oldGrowthType);
			sbyte result;
			if (flag)
			{
				result = (base.IsGood ? 1 : 3);
			}
			else
			{
				bool flag2 = oldGrowthType == 4;
				if (flag2)
				{
					result = (base.IsGood ? 1 : -1);
				}
				else
				{
					result = (base.IsGood ? -1 : 4);
				}
			}
			return result;
		}

		// Token: 0x04000F92 RID: 3986
		private const int MakeLoveRateAddPercent = 100;

		// Token: 0x04000F93 RID: 3987
		private const int AddPoisonTypeCount = 3;

		// Token: 0x04000F94 RID: 3988
		private const int AddPoisonLevel = 3;

		// Token: 0x04000F95 RID: 3989
		private const int AddPoisonValue = 3600;

		// Token: 0x04000F96 RID: 3990
		private const int ChangeWugCount = 3;

		// Token: 0x04000F97 RID: 3991
		private bool _affected;
	}
}
