using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Neigong.RandomEnemy
{
	// Token: 0x02000296 RID: 662
	public class WenESheng : MinionBase
	{
		// Token: 0x06003162 RID: 12642 RVA: 0x0021AC9B File Offset: 0x00218E9B
		public WenESheng()
		{
		}

		// Token: 0x06003163 RID: 12643 RVA: 0x0021ACA5 File Offset: 0x00218EA5
		public WenESheng(CombatSkillKey skillKey) : base(skillKey, 16000)
		{
		}

		// Token: 0x06003164 RID: 12644 RVA: 0x0021ACB8 File Offset: 0x00218EB8
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			for (sbyte hitType = 0; hitType < 4; hitType += 1)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, (ushort)(32 + hitType), -1, -1, -1, -1), EDataModifyType.TotalPercent);
			}
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
		}

		// Token: 0x06003165 RID: 12645 RVA: 0x0021AD27 File Offset: 0x00218F27
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
		}

		// Token: 0x06003166 RID: 12646 RVA: 0x0021AD4E File Offset: 0x00218F4E
		private void OnCombatBegin(DataContext context)
		{
			this.UpdateAddPercent(context);
		}

		// Token: 0x06003167 RID: 12647 RVA: 0x0021AD5C File Offset: 0x00218F5C
		private void OnCombatCharChanged(DataContext context, bool isAlly)
		{
			bool flag = isAlly == base.CombatChar.IsAlly;
			if (!flag)
			{
				this.UpdateAddPercent(context);
			}
		}

		// Token: 0x06003168 RID: 12648 RVA: 0x0021AD88 File Offset: 0x00218F88
		private void UpdateAddPercent(DataContext context)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			sbyte fameType = FameType.GetFameType(enemyChar.GetCharacter().GetFame());
			this._addPercent = 25 * Math.Max((int)(3 - fameType + 1), 0);
			for (sbyte hitType = 0; hitType < 4; hitType += 1)
			{
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, (ushort)(32 + hitType));
			}
		}

		// Token: 0x06003169 RID: 12649 RVA: 0x0021AE00 File Offset: 0x00219000
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !MinionBase.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = this._addPercent;
			}
			return result;
		}

		// Token: 0x04000EA4 RID: 3748
		private const sbyte AddHitUnit = 25;

		// Token: 0x04000EA5 RID: 3749
		private int _addPercent;
	}
}
