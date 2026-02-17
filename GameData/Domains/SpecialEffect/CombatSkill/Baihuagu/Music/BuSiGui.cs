using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Music
{
	// Token: 0x020005C5 RID: 1477
	public class BuSiGui : CombatSkillEffectBase
	{
		// Token: 0x060043C2 RID: 17346 RVA: 0x0026C779 File Offset: 0x0026A979
		public BuSiGui()
		{
		}

		// Token: 0x060043C3 RID: 17347 RVA: 0x0026C783 File Offset: 0x0026A983
		public BuSiGui(CombatSkillKey skillKey) : base(skillKey, 3301, -1)
		{
		}

		// Token: 0x060043C4 RID: 17348 RVA: 0x0026C794 File Offset: 0x0026A994
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(199, EDataModifyType.AddPercent, base.SkillTemplateId);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.CreateAffectedData(145, EDataModifyType.Add, -1);
				base.CreateAffectedData(146, EDataModifyType.Add, -1);
			}
			else
			{
				base.CreateAffectedAllEnemyData(145, EDataModifyType.Add, -1);
				base.CreateAffectedAllEnemyData(146, EDataModifyType.Add, -1);
			}
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060043C5 RID: 17349 RVA: 0x0026C821 File Offset: 0x0026AA21
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060043C6 RID: 17350 RVA: 0x0026C848 File Offset: 0x0026AA48
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = !this.SkillKey.IsMatch(charId, skillId);
			if (!flag)
			{
				sbyte happinessType = HappinessType.GetHappinessType((base.IsDirect ? base.CombatChar : base.CurrEnemyChar).GetHappiness());
				bool flag2 = happinessType <= 3;
				if (!flag2)
				{
					this._addPower = (int)(5 * (happinessType - 3));
					base.InvalidateCache(context, 199);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x060043C7 RID: 17351 RVA: 0x0026C8BC File Offset: 0x0026AABC
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			this.TryClearChangeDistance(context, charId, isAlly, skillId);
			bool flag = !this.SkillKey.IsMatch(charId, skillId);
			if (!flag)
			{
				this._addPower = 0;
				base.InvalidateCache(context, 199);
				bool flag2 = power > 0;
				if (flag2)
				{
					DomainManager.Combat.AddCombatState(context, base.IsDirect ? base.CombatChar : base.CurrEnemyChar, 0, 36, (int)(power * 2));
				}
				sbyte happinessType = HappinessType.GetHappinessType((base.IsDirect ? base.CombatChar : base.CurrEnemyChar).GetHappiness());
				bool flag3 = !base.PowerMatchAffectRequire((int)power, 0) || happinessType <= 3;
				if (!flag3)
				{
					this._changeDistance = (int)(3 * (happinessType - 3) * (base.IsDirect ? 1 : -1));
					this.InvalidCacheAttackRange(context);
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x060043C8 RID: 17352 RVA: 0x0026C99C File Offset: 0x0026AB9C
		private void TryClearChangeDistance(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = this._changeDistance == 0 || !CombatSkillTemplateHelper.IsAttack(skillId);
			if (!flag)
			{
				bool flag2 = base.IsDirect ? (charId != base.CharacterId) : (isAlly == base.CombatChar.IsAlly);
				if (!flag2)
				{
					this._changeDistance = 0;
					this.InvalidCacheAttackRange(context);
				}
			}
		}

		// Token: 0x060043C9 RID: 17353 RVA: 0x0026CA00 File Offset: 0x0026AC00
		private void InvalidCacheAttackRange(DataContext context)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.InvalidateCache(context, 145);
			}
			else
			{
				base.InvalidateAllEnemyCache(context, 145);
			}
		}

		// Token: 0x060043CA RID: 17354 RVA: 0x0026CA34 File Offset: 0x0026AC34
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.FieldId == 199 && dataKey.CombatSkillId == base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = this._addPower;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId - 145 <= 1;
				bool flag3 = flag2 && !dataKey.IsNormalAttack;
				if (flag3)
				{
					result = this._changeDistance;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x0400141D RID: 5149
		private const sbyte AddPowerUnit = 5;

		// Token: 0x0400141E RID: 5150
		private const sbyte ChangeAttackDistanceUnit = 3;

		// Token: 0x0400141F RID: 5151
		private int _addPower;

		// Token: 0x04001420 RID: 5152
		private int _changeDistance;
	}
}
