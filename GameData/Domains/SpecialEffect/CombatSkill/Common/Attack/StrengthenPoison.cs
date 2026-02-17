using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x020005A7 RID: 1447
	public class StrengthenPoison : CombatSkillEffectBase
	{
		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06004300 RID: 17152 RVA: 0x002695FF File Offset: 0x002677FF
		protected virtual bool Variant1
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004301 RID: 17153 RVA: 0x00269602 File Offset: 0x00267802
		public StrengthenPoison()
		{
		}

		// Token: 0x06004302 RID: 17154 RVA: 0x0026960C File Offset: 0x0026780C
		public StrengthenPoison(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06004303 RID: 17155 RVA: 0x0026961C File Offset: 0x0026781C
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 78, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			}
			else
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 73, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 233, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
			}
			bool flag = !base.IsDirect;
			if (flag)
			{
				Events.RegisterHandler_AddPoison(new Events.OnAddPoison(this.OnAddPoison));
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004304 RID: 17156 RVA: 0x002696E0 File Offset: 0x002678E0
		public override void OnDisable(DataContext context)
		{
			bool flag = !base.IsDirect;
			if (flag)
			{
				Events.UnRegisterHandler_AddPoison(new Events.OnAddPoison(this.OnAddPoison));
			}
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004305 RID: 17157 RVA: 0x00269720 File Offset: 0x00267920
		private void DirectVariant1(DataContext context)
		{
			bool flag = !base.IsDirect || !this.Variant1;
			if (!flag)
			{
				bool flag2 = base.CurrEnemyChar.GetDefeatMarkCollection().PoisonMarkList[(int)this.AffectPoisonType] <= 0;
				if (!flag2)
				{
					base.ShowSpecialEffectTips(1);
					DomainManager.Combat.PoisonAffect(context, base.CurrEnemyChar, this.AffectPoisonType);
				}
			}
		}

		// Token: 0x06004306 RID: 17158 RVA: 0x0026978C File Offset: 0x0026798C
		private unsafe void DirectVariant2(DataContext context)
		{
			bool flag = !base.IsDirect || this.Variant1;
			if (!flag)
			{
				PoisonsAndLevels poisons = base.SkillInstance.GetPoisons();
				sbyte poisonLevel = *(ref poisons.Levels.FixedElementField + this.AffectPoisonType);
				short poisonValue = *(ref poisons.Values.FixedElementField + (IntPtr)this.AffectPoisonType * 2);
				IReadOnlyList<sbyte> poisonTypes = PoisonType.IsInner(this.AffectPoisonType) ? PoisonType.InnerPoison : PoisonType.OuterPoison;
				foreach (sbyte poisonType in poisonTypes)
				{
					bool flag2 = poisonType == this.AffectPoisonType;
					if (!flag2)
					{
						DomainManager.Combat.AddPoison(context, base.CombatChar, base.CurrEnemyChar, poisonType, poisonLevel, (int)poisonValue, base.SkillTemplateId, true, true, default(ItemKey), true, false, false);
					}
				}
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x06004307 RID: 17159 RVA: 0x00269894 File Offset: 0x00267A94
		private int ReverseVariant1()
		{
			bool flag = base.IsDirect || !this.Variant1;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				base.ShowSpecialEffectTips(1);
				result = -50;
			}
			return result;
		}

		// Token: 0x06004308 RID: 17160 RVA: 0x002698CC File Offset: 0x00267ACC
		private void ReverseVariant2(DataContext context)
		{
			StrengthenPoison.<>c__DisplayClass19_0 CS$<>8__locals1 = new StrengthenPoison.<>c__DisplayClass19_0();
			bool flag = base.IsDirect || this.Variant1;
			if (!flag)
			{
				CS$<>8__locals1.enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				CS$<>8__locals1.isInner = PoisonType.IsInner(this.AffectPoisonType);
				foreach (short skillId in CS$<>8__locals1.enemyChar.GetRandomUnrepeatedBanableSkillIds(context.Random, 3, new Func<short, bool>(CS$<>8__locals1.<ReverseVariant2>g__Predicate|0), -1, -1))
				{
					DomainManager.Combat.SilenceSkill(context, CS$<>8__locals1.enemyChar, skillId, 2400, 100);
					base.ShowSpecialEffectTipsOnceInFrame(1);
				}
			}
		}

		// Token: 0x06004309 RID: 17161 RVA: 0x002699A4 File Offset: 0x00267BA4
		private void OnAddPoison(DataContext context, int attackerId, int defenderId, sbyte poisonType, sbyte level, int addValue, short skillId, bool canBounce)
		{
			bool flag = attackerId == base.CharacterId && poisonType == this.AffectPoisonType && skillId == base.SkillTemplateId;
			if (flag)
			{
				this._addedPoisonLevel = level;
				this._addedPoisonValue = addValue;
			}
		}

		// Token: 0x0600430A RID: 17162 RVA: 0x002699E8 File Offset: 0x00267BE8
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = (base.IsDirect || !this.Variant1) && base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					this.DirectVariant1(context);
					this.DirectVariant2(context);
					this.ReverseVariant2(context);
				}
				bool flag3 = !base.IsDirect && this._addedPoisonValue > 0 && context.Random.CheckPercentProb(30);
				if (flag3)
				{
					DomainManager.Combat.AddPoison(context, base.CombatChar, base.CombatChar, this.AffectPoisonType, this._addedPoisonLevel, this._addedPoisonValue * 20 / 100, skillId, true, true, default(ItemKey), false, false, false);
					base.ShowSpecialEffectTips(2);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0600430B RID: 17163 RVA: 0x00269ACC File Offset: 0x00267CCC
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 78 && dataKey.CustomParam0 == (int)this.AffectPoisonType;
				if (flag2)
				{
					base.ShowSpecialEffectTips(0);
					result = true;
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x0600430C RID: 17164 RVA: 0x00269B38 File Offset: 0x00267D38
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId || dataKey.CustomParam0 != (int)this.AffectPoisonType;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 73;
				if (flag2)
				{
					base.ShowSpecialEffectTips(0);
					result = 100;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 233 && dataKey.CustomParam1 > 0;
					if (flag3)
					{
						result = this.ReverseVariant1();
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x040013D7 RID: 5079
		private const sbyte AddPowerValuePercent = 100;

		// Token: 0x040013D8 RID: 5080
		private const sbyte AddPoisonToSelfOdds = 30;

		// Token: 0x040013D9 RID: 5081
		private const sbyte AddPoisonToSelfPercent = 20;

		// Token: 0x040013DA RID: 5082
		private const sbyte ReducePoisonResistPercent = 50;

		// Token: 0x040013DB RID: 5083
		private const int SilenceRequireRatio = 60;

		// Token: 0x040013DC RID: 5084
		private const int SilenceFrame = 2400;

		// Token: 0x040013DD RID: 5085
		private const int SilenceCount = 3;

		// Token: 0x040013DE RID: 5086
		protected sbyte AffectPoisonType;

		// Token: 0x040013DF RID: 5087
		private sbyte _addedPoisonLevel;

		// Token: 0x040013E0 RID: 5088
		private int _addedPoisonValue;
	}
}
