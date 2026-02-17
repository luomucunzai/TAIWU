using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.FistAndPalm
{
	// Token: 0x0200051C RID: 1308
	public class FengMoZuiQuan : CombatSkillEffectBase
	{
		// Token: 0x06003F06 RID: 16134 RVA: 0x00257FE1 File Offset: 0x002561E1
		public FengMoZuiQuan()
		{
		}

		// Token: 0x06003F07 RID: 16135 RVA: 0x00257FEB File Offset: 0x002561EB
		public FengMoZuiQuan(CombatSkillKey skillKey) : base(skillKey, 14103, -1)
		{
		}

		// Token: 0x06003F08 RID: 16136 RVA: 0x00257FFC File Offset: 0x002561FC
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003F09 RID: 16137 RVA: 0x00258057 File Offset: 0x00256257
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003F0A RID: 16138 RVA: 0x00258080 File Offset: 0x00256280
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || !this._autoCasting;
			if (!flag)
			{
				this._addDamage = (int)(base.CombatChar.GetCharacter().GetDisorderOfQi() / 100);
				DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * 50 / 100);
			}
		}

		// Token: 0x06003F0B RID: 16139 RVA: 0x002580F0 File Offset: 0x002562F0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0) && !interrupted;
				if (flag2)
				{
					this.TryAutoCast(context);
				}
				else
				{
					this._addDamage = 0;
					this._autoCasting = false;
				}
			}
		}

		// Token: 0x06003F0C RID: 16140 RVA: 0x00258150 File Offset: 0x00256350
		private unsafe void TryAutoCast(DataContext context)
		{
			bool autoCasting = this._autoCasting;
			if (autoCasting)
			{
				this._autoCastOdds -= 15;
				bool flag = context.Random.CheckPercentProb(this._autoCastOdds) && DomainManager.Combat.CanCastSkill(base.CombatChar, base.SkillTemplateId, true, false);
				if (flag)
				{
					DomainManager.Combat.CastSkillFree(context, base.CombatChar, base.SkillTemplateId, ECombatCastFreePriority.Normal);
					base.ShowSpecialEffectTips(0);
				}
				else
				{
					this._addDamage = 0;
					this._autoCasting = false;
				}
			}
			else
			{
				this._autoCastOdds = 30 + ((*this.CharObj.GetEatingItems()).ContainsWine() ? 60 : 0);
				bool flag2 = context.Random.CheckPercentProb(this._autoCastOdds) && DomainManager.Combat.CanCastSkill(base.CombatChar, base.SkillTemplateId, true, false);
				if (flag2)
				{
					DomainManager.Combat.CastSkillFree(context, base.CombatChar, base.SkillTemplateId, ECombatCastFreePriority.Normal);
					this._autoCasting = true;
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06003F0D RID: 16141 RVA: 0x00258268 File Offset: 0x00256468
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 69 && dataKey.CustomParam0 == (base.IsDirect ? 0 : 1);
				if (flag2)
				{
					result = this._addDamage;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04001290 RID: 4752
		private const sbyte AutoCastBaseOdds = 30;

		// Token: 0x04001291 RID: 4753
		private const sbyte DrunkAddAutoCastOdds = 60;

		// Token: 0x04001292 RID: 4754
		private const sbyte AutoCastOddsReduce = 15;

		// Token: 0x04001293 RID: 4755
		private const sbyte PrepareProgressPercent = 50;

		// Token: 0x04001294 RID: 4756
		private int _autoCastOdds;

		// Token: 0x04001295 RID: 4757
		private int _addDamage;

		// Token: 0x04001296 RID: 4758
		private bool _autoCasting;
	}
}
