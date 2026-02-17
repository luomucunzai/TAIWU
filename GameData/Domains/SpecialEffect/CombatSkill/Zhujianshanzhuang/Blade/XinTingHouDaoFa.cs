using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Blade
{
	// Token: 0x020001E0 RID: 480
	public class XinTingHouDaoFa : BladeUnlockEffectBase
	{
		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06002DB7 RID: 11703 RVA: 0x0020CD70 File Offset: 0x0020AF70
		private int ReducePowerPercent
		{
			get
			{
				return base.IsDirectOrReverseEffectDoubling ? -50 : -25;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06002DB8 RID: 11704 RVA: 0x0020CD80 File Offset: 0x0020AF80
		protected override IEnumerable<short> RequireWeaponTypes
		{
			get
			{
				yield return 10;
				yield break;
			}
		}

		// Token: 0x06002DB9 RID: 11705 RVA: 0x0020CD9F File Offset: 0x0020AF9F
		public XinTingHouDaoFa()
		{
		}

		// Token: 0x06002DBA RID: 11706 RVA: 0x0020CDA9 File Offset: 0x0020AFA9
		public XinTingHouDaoFa(CombatSkillKey skillKey) : base(skillKey, 9200)
		{
		}

		// Token: 0x06002DBB RID: 11707 RVA: 0x0020CDBC File Offset: 0x0020AFBC
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002DBC RID: 11708 RVA: 0x0020CE08 File Offset: 0x0020B008
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			base.OnDisable(context);
		}

		// Token: 0x06002DBD RID: 11709 RVA: 0x0020CE54 File Offset: 0x0020B054
		protected override bool CanDoAffect()
		{
			return base.CurrEnemyChar.GetAffectingDefendSkillId() >= 0;
		}

		// Token: 0x06002DBE RID: 11710 RVA: 0x0020CE68 File Offset: 0x0020B068
		public override void DoAffectAfterCost(DataContext context, int weaponIndex)
		{
			base.ShowSpecialEffectTips(base.IsDirect, 2, 1);
			short skillId = base.CurrEnemyChar.GetAffectingDefendSkillId();
			DomainManager.Combat.ClearAffectingDefenseSkill(context, base.CurrEnemyChar);
			DomainManager.Combat.AddGoneMadInjury(context, base.CurrEnemyChar, skillId, 0);
			DomainManager.Combat.SilenceSkill(context, base.CurrEnemyChar, skillId, 1800, 100);
		}

		// Token: 0x06002DBF RID: 11711 RVA: 0x0020CED1 File Offset: 0x0020B0D1
		private void OnCombatBegin(DataContext context)
		{
			base.AppendAffectedAllEnemyData(context, 199, EDataModifyType.AddPercent, -1);
		}

		// Token: 0x06002DC0 RID: 11712 RVA: 0x0020CEE4 File Offset: 0x0020B0E4
		private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = !this.SkillKey.IsMatch(charId, skillId) || !base.IsReverseOrUsingDirectWeapon;
			if (!flag)
			{
				this._reducePowerChar = base.CurrEnemyChar;
				base.InvalidateCache(context, this._reducePowerChar.GetId(), 199);
				base.ShowSpecialEffectTips(base.IsDirect, 1, 0);
			}
		}

		// Token: 0x06002DC1 RID: 11713 RVA: 0x0020CF48 File Offset: 0x0020B148
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || this._reducePowerChar == null;
			if (!flag)
			{
				int reducePowerCharId = this._reducePowerChar.GetId();
				this._reducePowerChar = null;
				base.InvalidateCache(context, reducePowerCharId, 199);
			}
		}

		// Token: 0x06002DC2 RID: 11714 RVA: 0x0020CF94 File Offset: 0x0020B194
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = this._reducePowerChar == null || dataKey.CharId != this._reducePowerChar.GetId() || dataKey.FieldId != 199;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = ((this._reducePowerChar.GetAffectingDefendSkillId() != dataKey.CombatSkillId) ? 0 : this.ReducePowerPercent);
			}
			return result;
		}

		// Token: 0x04000DB3 RID: 3507
		private const int SilenceFrame = 1800;

		// Token: 0x04000DB4 RID: 3508
		private CombatCharacter _reducePowerChar;
	}
}
