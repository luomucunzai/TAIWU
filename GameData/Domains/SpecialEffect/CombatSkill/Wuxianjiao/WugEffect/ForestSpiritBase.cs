using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect
{
	// Token: 0x0200035F RID: 863
	public class ForestSpiritBase : WugEffectBase
	{
		// Token: 0x0600354E RID: 13646 RVA: 0x0022C254 File Offset: 0x0022A454
		public static bool CanGrown(Character character)
		{
			List<short> featureIds = character.GetFeatureIds();
			return featureIds.Contains(217) || featureIds.Contains(218);
		}

		// Token: 0x0600354F RID: 13647 RVA: 0x0022C288 File Offset: 0x0022A488
		protected ForestSpiritBase()
		{
		}

		// Token: 0x06003550 RID: 13648 RVA: 0x0022C292 File Offset: 0x0022A492
		protected ForestSpiritBase(int charId, int type, short wugTemplateId, short effectId) : base(charId, type, wugTemplateId, effectId)
		{
			this.CostWugCount = 4;
		}

		// Token: 0x06003551 RID: 13649 RVA: 0x0022C2A8 File Offset: 0x0022A4A8
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			bool isGrown = base.IsGrown;
			if (isGrown)
			{
				base.CreateAffectedData(296, EDataModifyType.AddPercent, -1);
			}
			bool canChangeToGrown = base.CanChangeToGrown;
			if (canChangeToGrown)
			{
				Events.RegisterHandler_XiangshuInfectionFeatureChangedEnd(new Events.OnXiangshuInfectionFeatureChangedEnd(this.OnXiangshuInfectionFeatureChangedEnd));
			}
			bool isGrown2 = base.IsGrown;
			if (isGrown2)
			{
				Events.RegisterHandler_AdvanceMonthFinish(new Events.OnAdvanceMonthFinish(this.OnAdvanceMonthFinish));
			}
		}

		// Token: 0x06003552 RID: 13650 RVA: 0x0022C310 File Offset: 0x0022A510
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			bool canChangeToGrown = base.CanChangeToGrown;
			if (canChangeToGrown)
			{
				Events.UnRegisterHandler_XiangshuInfectionFeatureChangedEnd(new Events.OnXiangshuInfectionFeatureChangedEnd(this.OnXiangshuInfectionFeatureChangedEnd));
			}
			bool isGrown = base.IsGrown;
			if (isGrown)
			{
				Events.UnRegisterHandler_AdvanceMonthFinish(new Events.OnAdvanceMonthFinish(this.OnAdvanceMonthFinish));
			}
		}

		// Token: 0x06003553 RID: 13651 RVA: 0x0022C35E File Offset: 0x0022A55E
		protected override void AddAffectDataAndEvent(DataContext context)
		{
			Events.RegisterHandler_PoisonAffected(new Events.OnPoisonAffected(this.OnPoisonAffected));
		}

		// Token: 0x06003554 RID: 13652 RVA: 0x0022C373 File Offset: 0x0022A573
		protected override void ClearAffectDataAndEvent(DataContext context)
		{
			Events.UnRegisterHandler_PoisonAffected(new Events.OnPoisonAffected(this.OnPoisonAffected));
		}

		// Token: 0x06003555 RID: 13653 RVA: 0x0022C388 File Offset: 0x0022A588
		private void OnXiangshuInfectionFeatureChangedEnd(DataContext context, Character character, short featureId)
		{
			bool flag = ForestSpiritBase.CanGrown(this.CharObj) && base.CanAffect;
			if (flag)
			{
				this.ChangeToGrown(context);
			}
		}

		// Token: 0x06003556 RID: 13654 RVA: 0x0022C3B8 File Offset: 0x0022A5B8
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
					List<int> neighborCharIds = ObjectPool<List<int>>.Instance.Get();
					DomainManager.Map.GetRealNeighborBlocks(location.AreaId, location.BlockId, neighborBlocks, 1, true);
					neighborCharIds.Clear();
					neighborCharIds.AddRange((from x in neighborBlocks
					where x.CharacterSet != null
					select x).SelectMany((MapBlockData x) => x.CharacterSet));
					neighborCharIds.Remove(base.CharacterId);
					CollectionUtils.Shuffle<int>(context.Random, neighborCharIds);
					int reduceCount = Math.Min(neighborCharIds.Count, 3);
					bool isTaiwu = this.CharObj.GetId() == DomainManager.Taiwu.GetTaiwuCharId();
					LifeRecordCollection lifeRecord = DomainManager.LifeRecord.GetLifeRecordCollection();
					for (int i = 0; i < reduceCount; i++)
					{
						Character character = DomainManager.Character.GetElement_Objects(neighborCharIds[i]);
						bool flag3 = isTaiwu;
						if (flag3)
						{
							DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, character, this.CharObj, -4000);
						}
						else
						{
							DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, character, this.CharObj, -4000);
						}
						base.AddLifeRecord<int>(new WugEffectBase.LifeRecordAddTemplate<int>(lifeRecord.AddWugForestSpiritReduceFavorability), character.GetId());
						bool isElite = base.IsElite;
						if (isElite)
						{
							Character.ApplyAddRelation_Enemy(context, this.CharObj, character, isTaiwu, 6, new CharacterBecomeEnemyInfo(this.CharObj)
							{
								WugTemplateId = this.WugConfig.TemplateId
							});
						}
					}
					ObjectPool<List<MapBlockData>>.Instance.Return(neighborBlocks);
					ObjectPool<List<int>>.Instance.Return(neighborCharIds);
				}
			}
		}

		// Token: 0x06003557 RID: 13655 RVA: 0x0022C5B0 File Offset: 0x0022A7B0
		private void OnPoisonAffected(DataContext context, int charId, sbyte poisonType)
		{
			bool flag = charId != base.CharacterId || !base.CanAffect;
			if (!flag)
			{
				CombatCharacter affectChar = base.IsGood ? DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false) : base.CombatChar;
				DomainManager.Combat.AppendMindDefeatMark(context, affectChar, (!base.IsGrown && base.IsElite) ? 2 : 1, -1, false);
				base.ShowEffectTips(context, 1);
				bool isElite = base.IsElite;
				if (isElite)
				{
					base.ShowEffectTips(context, 2);
				}
				base.CostWugInCombat(context);
			}
		}

		// Token: 0x06003558 RID: 13656 RVA: 0x0022C64C File Offset: 0x0022A84C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 296 || !base.CanAffect || !base.IsElite;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = 900;
			}
			return result;
		}

		// Token: 0x04000FA3 RID: 4003
		private const int ReduceFavorability = 4000;

		// Token: 0x04000FA4 RID: 4004
		private const int ReduceFavorabilityCharCount = 3;

		// Token: 0x04000FA5 RID: 4005
		private const int TakeRevengeRateAddPercent = 900;
	}
}
