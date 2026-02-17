using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect
{
	// Token: 0x02000254 RID: 596
	public class HeadBreakBase : BreakBodyEffectBase
	{
		// Token: 0x06003020 RID: 12320 RVA: 0x00215FBA File Offset: 0x002141BA
		protected HeadBreakBase()
		{
		}

		// Token: 0x06003021 RID: 12321 RVA: 0x00215FC4 File Offset: 0x002141C4
		protected HeadBreakBase(int charId, int type) : base(charId, type)
		{
			this.AffectBodyParts = new sbyte[]
			{
				2
			};
		}

		// Token: 0x06003022 RID: 12322 RVA: 0x00215FE0 File Offset: 0x002141E0
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 56, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 57, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 58, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 59, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 94, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 95, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 96, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 97, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.RegisterAttackHandler));
			Events.RegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
			this.RegisterAttackHandler(context);
		}

		// Token: 0x06003023 RID: 12323 RVA: 0x00216114 File Offset: 0x00214314
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.RegisterAttackHandler));
			Events.UnRegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
			bool flag = DomainManager.Combat.IsCharInCombat(base.CharacterId, true);
			if (flag)
			{
				Events.UnRegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
				Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			}
		}

		// Token: 0x06003024 RID: 12324 RVA: 0x0021618C File Offset: 0x0021438C
		private void RegisterAttackHandler(DataContext context)
		{
			bool flag = !DomainManager.Combat.IsCharInCombat(base.CharacterId, true);
			if (!flag)
			{
				this._attackAccumulator = 0;
				this._defendAccumulator = 0;
				this._hitAffecting = false;
				this._avoidAffecting = false;
				Events.RegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
				Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			}
		}

		// Token: 0x06003025 RID: 12325 RVA: 0x002161F4 File Offset: 0x002143F4
		private void OnCombatSettlement(DataContext context, sbyte combatStatus)
		{
			bool flag = !DomainManager.Combat.IsCharInCombat(base.CharacterId, false);
			if (!flag)
			{
				Events.UnRegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
				Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			}
		}

		// Token: 0x06003026 RID: 12326 RVA: 0x00216240 File Offset: 0x00214440
		private void OnNormalAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex)
		{
			bool flag = pursueIndex > 0;
			if (!flag)
			{
				bool flag2 = attacker == base.CombatChar;
				if (flag2)
				{
					this._attackAccumulator += 1;
					bool flag3 = this._attackAccumulator >= 2;
					if (flag3)
					{
						this._attackAccumulator -= 2;
						this._hitAffecting = true;
						DomainManager.Combat.ShowSpecialEffectTips(base.CharacterId, this.IsInner ? 1301 : 575, 2);
					}
				}
				else
				{
					bool flag4 = defender == base.CombatChar;
					if (flag4)
					{
						this._defendAccumulator += 1;
						bool flag5 = this._defendAccumulator >= 2;
						if (flag5)
						{
							this._defendAccumulator -= 2;
							this._avoidAffecting = true;
							DomainManager.Combat.ShowSpecialEffectTips(base.CharacterId, this.IsInner ? 1301 : 575, 2);
						}
					}
				}
			}
		}

		// Token: 0x06003027 RID: 12327 RVA: 0x00216338 File Offset: 0x00214538
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = attacker == base.CombatChar;
			if (flag)
			{
				this._hitAffecting = false;
			}
			else
			{
				bool flag2 = defender == base.CombatChar;
				if (flag2)
				{
					this._avoidAffecting = false;
				}
			}
		}

		// Token: 0x06003028 RID: 12328 RVA: 0x00216370 File Offset: 0x00214570
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = this._hitAffecting && (dataKey.FieldId == 56 || dataKey.FieldId == 57 || dataKey.FieldId == 58 || dataKey.FieldId == 59);
				if (flag2)
				{
					result = -60;
				}
				else
				{
					bool flag3 = this._avoidAffecting && (dataKey.FieldId == 94 || dataKey.FieldId == 95 || dataKey.FieldId == 96 || dataKey.FieldId == 97);
					if (flag3)
					{
						result = -60;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04000E4A RID: 3658
		private const sbyte NeedAttackCount = 2;

		// Token: 0x04000E4B RID: 3659
		private const sbyte ReduceHitAvoid = -60;

		// Token: 0x04000E4C RID: 3660
		private sbyte _attackAccumulator;

		// Token: 0x04000E4D RID: 3661
		private sbyte _defendAccumulator;

		// Token: 0x04000E4E RID: 3662
		private bool _hitAffecting;

		// Token: 0x04000E4F RID: 3663
		private bool _avoidAffecting;
	}
}
