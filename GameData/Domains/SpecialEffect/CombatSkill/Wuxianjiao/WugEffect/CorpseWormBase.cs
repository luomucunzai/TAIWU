using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect
{
	// Token: 0x02000351 RID: 849
	public class CorpseWormBase : WugEffectBase
	{
		// Token: 0x170001CE RID: 462
		// (get) Token: 0x0600351B RID: 13595 RVA: 0x0022B766 File Offset: 0x00229966
		private int HealthPercent
		{
			get
			{
				return CValuePercent.ParseInt((int)this.CharObj.GetHealth(), (int)this.CharObj.GetLeftMaxHealth(false));
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x0600351C RID: 13596 RVA: 0x0022B784 File Offset: 0x00229984
		private CValuePercentBonus FatalDamageBonus
		{
			get
			{
				return (this.HealthPercent < 50) ? 0 : (base.IsGood ? -25 : 25);
			}
		}

		// Token: 0x0600351D RID: 13597 RVA: 0x0022B7A6 File Offset: 0x002299A6
		protected CorpseWormBase()
		{
		}

		// Token: 0x0600351E RID: 13598 RVA: 0x0022B7B0 File Offset: 0x002299B0
		protected CorpseWormBase(int charId, int type, short wugTemplateId, short effectId) : base(charId, type, wugTemplateId, effectId)
		{
			this.CostWugCount = 12;
		}

		// Token: 0x0600351F RID: 13599 RVA: 0x0022B7C8 File Offset: 0x002299C8
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			if (this.AffectDatas == null)
			{
				this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			}
			bool isGrown = base.IsGrown;
			if (isGrown)
			{
				base.CreateAffectedData(263, EDataModifyType.Add, -1);
				base.CreateAffectedData(295, EDataModifyType.Custom, -1);
				Events.RegisterHandler_AdvanceMonthFinish(new Events.OnAdvanceMonthFinish(this.OnAdvanceMonthFinish));
			}
			else
			{
				base.CreateAffectedData(294, EDataModifyType.Custom, -1);
			}
			bool canChangeToGrown = base.CanChangeToGrown;
			if (canChangeToGrown)
			{
				Events.RegisterHandler_PostAdvanceMonthBegin(new Events.OnPostAdvanceMonthBegin(this.OnAdvanceMonthBegin));
			}
		}

		// Token: 0x06003520 RID: 13600 RVA: 0x0022B858 File Offset: 0x00229A58
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			bool isGrown = base.IsGrown;
			if (isGrown)
			{
				Events.UnRegisterHandler_AdvanceMonthFinish(new Events.OnAdvanceMonthFinish(this.OnAdvanceMonthFinish));
			}
			bool canChangeToGrown = base.CanChangeToGrown;
			if (canChangeToGrown)
			{
				Events.UnRegisterHandler_PostAdvanceMonthBegin(new Events.OnPostAdvanceMonthBegin(this.OnAdvanceMonthBegin));
			}
		}

		// Token: 0x06003521 RID: 13601 RVA: 0x0022B8A6 File Offset: 0x00229AA6
		protected override void AddAffectDataAndEvent(DataContext context)
		{
			this._affected = false;
			base.AppendAffectedData(context, base.CharacterId, 102, EDataModifyType.TotalPercent, -1);
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06003522 RID: 13602 RVA: 0x0022B8D4 File Offset: 0x00229AD4
		protected override void ClearAffectDataAndEvent(DataContext context)
		{
			base.RemoveAffectedData(context, base.CharacterId, 102);
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06003523 RID: 13603 RVA: 0x0022B8FC File Offset: 0x00229AFC
		private void OnAdvanceMonthBegin(DataContext context)
		{
			bool flag = !base.CanAffect;
			if (!flag)
			{
				short health = this.CharObj.GetHealth();
				short maxHealth = this.CharObj.GetLeftMaxHealth(false);
				bool flag2 = health > maxHealth * 50 / 100;
				if (!flag2)
				{
					this.ChangeToGrown(context);
				}
			}
		}

		// Token: 0x06003524 RID: 13604 RVA: 0x0022B94C File Offset: 0x00229B4C
		private void OnAdvanceMonthFinish(DataContext context)
		{
			bool flag = !this._affectedOnMonthChange;
			if (!flag)
			{
				this._affectedOnMonthChange = false;
				LifeRecordCollection lifeRecord = DomainManager.LifeRecord.GetLifeRecordCollection();
				base.AddLifeRecord(new WugEffectBase.LifeRecordAddTemplate(lifeRecord.AddWugCorpseWormChangeHealth));
			}
		}

		// Token: 0x06003525 RID: 13605 RVA: 0x0022B990 File Offset: 0x00229B90
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = context.DefenderId != base.CharacterId || index < 3 || !this._affected;
			if (!flag)
			{
				this._affected = false;
				base.ShowEffectTips(context, 1);
				base.CostWugInCombat(context);
			}
		}

		// Token: 0x06003526 RID: 13606 RVA: 0x0022B9E8 File Offset: 0x00229BE8
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
				bool flag2 = dataKey.FieldId == 263;
				if (flag2)
				{
					this._affectedOnMonthChange = true;
					result = -60;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 102 && !dataKey.IsNormalAttack;
					if (flag3)
					{
						this._affected = true;
						result = (base.IsGood ? -40 : 40);
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x06003527 RID: 13607 RVA: 0x0022BA70 File Offset: 0x00229C70
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 295 || !base.IsElite || !base.CanAffect;
			return flag && dataValue;
		}

		// Token: 0x06003528 RID: 13608 RVA: 0x0022BABC File Offset: 0x00229CBC
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 294 || !base.CanAffect || !base.IsElite;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = DomainManager.Combat.CheckHealthImmunity(base.CombatChar.GetDataContext(), base.CombatChar);
				if (flag2)
				{
					result = dataValue;
				}
				else
				{
					int modifiedValue = dataValue * this.FatalDamageBonus;
					int fatalDamageStep = base.CombatChar.GetDamageStepCollection().FatalDamageStep;
					int changeValue = Math.Abs(dataValue - modifiedValue);
					int changeHealth = (int)GlobalConfig.Instance.ReduceHealthPerFatalDamageMark[2] * changeValue / Math.Max(fatalDamageStep, 1);
					bool flag3 = changeValue > 0;
					if (flag3)
					{
						base.ShowEffectTips(DomainManager.Combat.Context, 2);
					}
					bool flag4 = changeHealth > 0;
					if (flag4)
					{
						this.CharObj.ChangeHealth(DomainManager.Combat.Context, -changeHealth);
						base.ShowEffectTips(base.CombatChar.GetDataContext(), 3);
					}
					result = modifiedValue;
				}
			}
			return result;
		}

		// Token: 0x06003529 RID: 13609 RVA: 0x0022BBC8 File Offset: 0x00229DC8
		protected override void ChangeToGrown(DataContext context)
		{
			List<int> charRandomPool = ObjectPool<List<int>>.Instance.Get();
			Location location = this.CharObj.GetLocation();
			bool flag = !location.IsValid();
			if (flag)
			{
				location = this.CharObj.GetValidLocation();
			}
			HashSet<int> blockCharSet = DomainManager.Map.GetBlock(location).CharacterSet;
			charRandomPool.Clear();
			bool flag2 = blockCharSet != null;
			if (flag2)
			{
				charRandomPool.AddRange(blockCharSet);
			}
			charRandomPool.Remove(base.CharacterId);
			bool flag3 = DomainManager.Taiwu.GetTaiwuCharId() == base.CharacterId;
			if (flag3)
			{
				charRandomPool.AddRange(DomainManager.Taiwu.GetGroupCharIds().GetCollection());
				charRandomPool.Remove(base.CharacterId);
			}
			int infectCharCount = Math.Min(5, charRandomPool.Count);
			this.CharObj.SetHealth(0, context);
			this.CharObj.RemoveWug(context, this.WugConfig.TemplateId);
			LifeRecordCollection lifeRecord = DomainManager.LifeRecord.GetLifeRecordCollection();
			for (int i = 0; i < infectCharCount; i++)
			{
				int index = context.Random.Next(0, charRandomPool.Count);
				short grownTemplateId = ItemDomain.GetWugTemplateId(this.WugConfig.WugType, 4);
				DomainManager.Character.GetElement_Objects(charRandomPool[index]).AddWug(context, grownTemplateId);
				base.AddLifeRecord(new WugEffectBase.LifeRecordRelatedAddTemplate(lifeRecord.AddWugCorpseWormChangeToGrown), charRandomPool[index], grownTemplateId);
				charRandomPool.RemoveAt(index);
			}
			ObjectPool<List<int>>.Instance.Return(charRandomPool);
		}

		// Token: 0x04000F9A RID: 3994
		private const int ChangeDamagePercent = 40;

		// Token: 0x04000F9B RID: 3995
		private const int GrowRequireHealthPercent = 50;

		// Token: 0x04000F9C RID: 3996
		private const sbyte GrownInfectCount = 5;

		// Token: 0x04000F9D RID: 3997
		private const int HealthDeltaValue = -60;

		// Token: 0x04000F9E RID: 3998
		private bool _affected;

		// Token: 0x04000F9F RID: 3999
		private bool _affectedOnMonthChange;
	}
}
