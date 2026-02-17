using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect
{
	// Token: 0x02000366 RID: 870
	public class GoldenSilkwormBase : WugEffectBase
	{
		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06003565 RID: 13669 RVA: 0x0022C77E File Offset: 0x0022A97E
		private int ConsummateLevelBonusAddPercent
		{
			get
			{
				return (!base.IsElite) ? 0 : (base.IsGood ? 50 : -50);
			}
		}

		// Token: 0x06003566 RID: 13670 RVA: 0x0022C799 File Offset: 0x0022A999
		private static int CalcPoisonRatio(bool isGrown)
		{
			return isGrown ? 10 : 5;
		}

		// Token: 0x06003567 RID: 13671 RVA: 0x0022C7A3 File Offset: 0x0022A9A3
		protected GoldenSilkwormBase()
		{
		}

		// Token: 0x06003568 RID: 13672 RVA: 0x0022C7AD File Offset: 0x0022A9AD
		protected GoldenSilkwormBase(int charId, int type, short wugTemplateId, short effectId) : base(charId, type, wugTemplateId, effectId)
		{
			this.CostWugCount = 16;
		}

		// Token: 0x06003569 RID: 13673 RVA: 0x0022C7C4 File Offset: 0x0022A9C4
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			bool isGrown = base.IsGrown;
			if (isGrown)
			{
				base.CreateAffectedData(267, EDataModifyType.Custom, -1);
			}
			else
			{
				base.CreateAffectedData(297, EDataModifyType.TotalPercent, -1);
			}
			Events.RegisterHandler_AdvanceMonthBegin(new Events.OnAdvanceMonthBegin(this.OnAdvanceMonthBegin));
		}

		// Token: 0x0600356A RID: 13674 RVA: 0x0022C814 File Offset: 0x0022AA14
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AdvanceMonthBegin(new Events.OnAdvanceMonthBegin(this.OnAdvanceMonthBegin));
			base.OnDisable(context);
		}

		// Token: 0x0600356B RID: 13675 RVA: 0x0022C834 File Offset: 0x0022AA34
		private unsafe void OnAdvanceMonthBegin(DataContext context)
		{
			bool flag = !base.CanAffect;
			if (!flag)
			{
				EatingItems eatingItems = *this.CharObj.GetEatingItems();
				bool flag2 = base.IsGrown && base.IsElite;
				if (flag2)
				{
					List<sbyte> grownWugTypes = ObjectPool<List<sbyte>>.Instance.Get();
					for (sbyte wugType = 0; wugType < 8; wugType += 1)
					{
						int index = eatingItems.IndexOfWug(wugType, false);
						bool flag3 = wugType == this.WugConfig.WugType || index < 0;
						if (!flag3)
						{
							MedicineItem wugConfig = Config.Medicine.Instance[eatingItems.Get(index).TemplateId];
							bool flag4 = wugConfig.WugGrowthType != 4;
							if (!flag4)
							{
								grownWugTypes.Add(wugType);
							}
						}
					}
					bool flag5 = grownWugTypes.Count > 0;
					if (flag5)
					{
						this.EatWug(context, grownWugTypes.GetRandom(context.Random), true);
					}
				}
				else
				{
					bool canChangeToGrown = base.CanChangeToGrown;
					if (canChangeToGrown)
					{
						bool flag6 = eatingItems.ContainsAny();
						if (!flag6)
						{
							this.ChangeToGrown(context);
						}
					}
				}
			}
		}

		// Token: 0x0600356C RID: 13676 RVA: 0x0022C950 File Offset: 0x0022AB50
		public override void OnEffectAdded(DataContext context, short replacedWug)
		{
			bool flag = !base.CanAffect;
			if (!flag)
			{
				bool affected = false;
				for (sbyte wugType = 0; wugType < 8; wugType += 1)
				{
					affected = (this.EatWug(context, wugType, false) || affected);
				}
				bool flag2 = affected;
				if (flag2)
				{
					this.OnAffected(context);
				}
			}
		}

		// Token: 0x0600356D RID: 13677 RVA: 0x0022C99B File Offset: 0x0022AB9B
		protected override void AddAffectDataAndEvent(DataContext context)
		{
			Events.RegisterHandler_AddWug(new Events.OnAddWug(this.OnAddWug));
		}

		// Token: 0x0600356E RID: 13678 RVA: 0x0022C9B0 File Offset: 0x0022ABB0
		protected override void ClearAffectDataAndEvent(DataContext context)
		{
			Events.UnRegisterHandler_AddWug(new Events.OnAddWug(this.OnAddWug));
		}

		// Token: 0x0600356F RID: 13679 RVA: 0x0022C9C8 File Offset: 0x0022ABC8
		private void OnAddWug(DataContext context, int charId, short wugTemplateId, short replacedWug)
		{
			bool flag = charId != base.CharacterId || !base.CanAffect;
			if (!flag)
			{
				MedicineItem wugConfig = Config.Medicine.Instance[wugTemplateId];
				bool flag2 = !this.EatWug(context, wugConfig.WugType, false);
				if (!flag2)
				{
					this.OnAffected(context);
				}
			}
		}

		// Token: 0x06003570 RID: 13680 RVA: 0x0022CA1C File Offset: 0x0022AC1C
		private unsafe bool EatWug(DataContext context, sbyte wugType, bool eatGrown = false)
		{
			bool flag = wugType == this.WugConfig.WugType;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				EatingItems eatingItems = *this.CharObj.GetEatingItems();
				int wugIndex = eatingItems.IndexOfWug(wugType, false);
				bool flag2 = wugIndex < 0;
				if (flag2)
				{
					result = false;
				}
				else
				{
					short wugTemplateId = eatingItems.Get(wugIndex).TemplateId;
					MedicineItem wugConfig = Config.Medicine.Instance[wugTemplateId];
					bool flag3 = !eatGrown && !WugGrowthType.IsWugGrowthTypeCombatOnly(wugConfig.WugGrowthType);
					if (flag3)
					{
						result = false;
					}
					else
					{
						this.CharObj.RemoveWug(context, wugTemplateId);
						bool isGrown = wugConfig.WugGrowthType == 4;
						int poisonRatio = GoldenSilkwormBase.CalcPoisonRatio(isGrown);
						this.EatOtherWugEffect(context, wugConfig.WugType, poisonRatio);
						bool flag4 = isGrown;
						if (flag4)
						{
							LifeRecordCollection lifeRecord = DomainManager.LifeRecord.GetLifeRecordCollection();
							base.AddLifeRecord<sbyte, short>(new WugEffectBase.LifeRecordAddTemplate<sbyte, short>(lifeRecord.AddWugKingGoldenSilkwormEatGrownWug), 8, wugTemplateId);
						}
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06003571 RID: 13681 RVA: 0x0022CB10 File Offset: 0x0022AD10
		private unsafe void EatOtherWugEffect(DataContext context, sbyte foodWugType, int poisonRatio)
		{
			CombatSkillItem skillConfig = CombatSkill.Instance[GoldenSilkwormBase.WugType2SkillId[foodWugType]];
			PoisonsAndLevels skillPoisons = skillConfig.Poisons;
			for (sbyte poisonType = 0; poisonType < 6; poisonType += 1)
			{
				int value = (int)(*(ref skillPoisons.Values.FixedElementField + (IntPtr)poisonType * 2)) * poisonRatio;
				sbyte level = *(ref skillPoisons.Levels.FixedElementField + poisonType);
				bool flag = level <= 0;
				if (!flag)
				{
					bool flag2 = DomainManager.Combat.IsInCombat();
					if (flag2)
					{
						bool isGood = base.IsGood;
						if (isGood)
						{
							DomainManager.Combat.ReducePoison(context, base.CombatChar, poisonType, value, true, false);
						}
						else
						{
							DomainManager.Combat.AddPoison(context, base.CombatChar, base.CombatChar, poisonType, level, value, -1, true, true, default(ItemKey), false, false, false);
						}
					}
					else
					{
						bool isGood2 = base.IsGood;
						if (isGood2)
						{
							this.CharObj.ChangePoisoned(context, poisonType, 3, -value);
						}
						else
						{
							this.CharObj.ChangePoisoned(context, poisonType, 3, value);
						}
					}
				}
			}
		}

		// Token: 0x06003572 RID: 13682 RVA: 0x0022CC24 File Offset: 0x0022AE24
		private void OnAffected(DataContext context)
		{
			base.ShowEffectTips(context, 1);
			base.ShowEffectTips(context, 2);
			base.CostWugInCombat(context);
		}

		// Token: 0x06003573 RID: 13683 RVA: 0x0022CC44 File Offset: 0x0022AE44
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 297 || !base.CanAffect || !base.IsElite;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = this.ConsummateLevelBonusAddPercent;
			}
			return result;
		}

		// Token: 0x06003574 RID: 13684 RVA: 0x0022CC94 File Offset: 0x0022AE94
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 267 || !base.CanAffect;
			return !flag || dataValue;
		}

		// Token: 0x06003575 RID: 13685 RVA: 0x0022CCD8 File Offset: 0x0022AED8
		// Note: this type is marked as 'beforefieldinit'.
		static GoldenSilkwormBase()
		{
			Dictionary<sbyte, short> dictionary = new Dictionary<sbyte, short>();
			dictionary[0] = 445;
			dictionary[1] = 446;
			dictionary[2] = 447;
			dictionary[3] = 448;
			dictionary[4] = 449;
			dictionary[5] = 450;
			dictionary[6] = 451;
			dictionary[7] = 452;
			GoldenSilkwormBase.WugType2SkillId = dictionary;
		}

		// Token: 0x04000FA6 RID: 4006
		private static readonly Dictionary<sbyte, short> WugType2SkillId;
	}
}
