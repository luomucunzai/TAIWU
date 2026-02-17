using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Sword
{
	// Token: 0x020004E4 RID: 1252
	public class JieQingKuaiJian : CombatSkillEffectBase
	{
		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06003DE6 RID: 15846 RVA: 0x00253DB6 File Offset: 0x00251FB6
		private bool HasExtraOdds
		{
			get
			{
				return (base.IsDirect ? base.CombatChar : base.EnemyChar).GetTrickAtStart() == 19;
			}
		}

		// Token: 0x06003DE7 RID: 15847 RVA: 0x00253DD7 File Offset: 0x00251FD7
		public JieQingKuaiJian()
		{
		}

		// Token: 0x06003DE8 RID: 15848 RVA: 0x00253DE1 File Offset: 0x00251FE1
		public JieQingKuaiJian(CombatSkillKey skillKey) : base(skillKey, 13200, -1)
		{
		}

		// Token: 0x06003DE9 RID: 15849 RVA: 0x00253DF2 File Offset: 0x00251FF2
		public override void OnEnable(DataContext context)
		{
			this._autoCastOdds = 60;
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003DEA RID: 15850 RVA: 0x00253E21 File Offset: 0x00252021
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003DEB RID: 15851 RVA: 0x00253E48 File Offset: 0x00252048
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || this._autoCastOdds >= 60;
			if (!flag)
			{
				DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * 50 / 100);
			}
		}

		// Token: 0x06003DEC RID: 15852 RVA: 0x00253EA4 File Offset: 0x002520A4
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					bool flag3 = base.PowerMatchAffectRequire((int)power, 1) && DomainManager.Combat.CanCastSkill(base.CombatChar, base.SkillTemplateId, true, false) && context.Random.CheckPercentProb(this._autoCastOdds + (this.HasExtraOdds ? 20 : 0));
					if (flag3)
					{
						this._autoCastOdds -= 20;
						DomainManager.Combat.CastSkillFree(context, base.CombatChar, base.SkillTemplateId, ECombatCastFreePriority.Normal);
						base.ShowSpecialEffectTips(1);
					}
					else
					{
						this._autoCastOdds = 60;
					}
					DomainManager.Combat.AddTrick(context, base.IsDirect ? base.CombatChar : DomainManager.Combat.GetCombatCharacter(!isAlly, false), 19, base.IsDirect);
					base.ShowSpecialEffectTips(0);
				}
				else
				{
					this._autoCastOdds = 60;
				}
			}
		}

		// Token: 0x04001241 RID: 4673
		private const sbyte AutoCastBaseOdds = 60;

		// Token: 0x04001242 RID: 4674
		private const sbyte AutoCastExtraOdds = 20;

		// Token: 0x04001243 RID: 4675
		private const sbyte ReduceAutoCastOdds = 20;

		// Token: 0x04001244 RID: 4676
		private const sbyte PrepareProgressPercent = 50;

		// Token: 0x04001245 RID: 4677
		private int _autoCastOdds;
	}
}
