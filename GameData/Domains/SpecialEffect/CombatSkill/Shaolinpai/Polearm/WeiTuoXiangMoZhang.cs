using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Polearm
{
	// Token: 0x02000418 RID: 1048
	public class WeiTuoXiangMoZhang : CombatSkillEffectBase
	{
		// Token: 0x06003938 RID: 14648 RVA: 0x0023DBA5 File Offset: 0x0023BDA5
		public WeiTuoXiangMoZhang()
		{
		}

		// Token: 0x06003939 RID: 14649 RVA: 0x0023DBAF File Offset: 0x0023BDAF
		public WeiTuoXiangMoZhang(CombatSkillKey skillKey) : base(skillKey, 1306, -1)
		{
		}

		// Token: 0x0600393A RID: 14650 RVA: 0x0023DBC0 File Offset: 0x0023BDC0
		public override void OnEnable(DataContext context)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				Events.RegisterHandler_FlawAdded(new Events.OnFlawAdded(this.OnFlawOrAcupointAdded));
			}
			else
			{
				Events.RegisterHandler_AcuPointAdded(new Events.OnAcuPointAdded(this.OnFlawOrAcupointAdded));
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x0600393B RID: 14651 RVA: 0x0023DC24 File Offset: 0x0023BE24
		public override void OnDisable(DataContext context)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				Events.UnRegisterHandler_FlawAdded(new Events.OnFlawAdded(this.OnFlawOrAcupointAdded));
			}
			else
			{
				Events.UnRegisterHandler_AcuPointAdded(new Events.OnAcuPointAdded(this.OnFlawOrAcupointAdded));
			}
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x0600393C RID: 14652 RVA: 0x0023DC88 File Offset: 0x0023BE88
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = !this.IsSrcSkillPerformed;
				if (flag2)
				{
					bool flag3 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag3)
					{
						this.IsSrcSkillPerformed = true;
						base.AddMaxEffectCount(true);
					}
					else
					{
						base.RemoveSelf(context);
					}
				}
				else
				{
					bool flag4 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag4)
					{
						base.RemoveSelf(context);
					}
				}
			}
		}

		// Token: 0x0600393D RID: 14653 RVA: 0x0023DD08 File Offset: 0x0023BF08
		private void OnFlawOrAcupointAdded(DataContext context, CombatCharacter combatChar, sbyte bodyPart, sbyte level)
		{
			bool flag = !this.IsSrcSkillPerformed || combatChar != base.CombatChar;
			if (!flag)
			{
				int addCount = Math.Min((int)(level + 1), base.EffectCount);
				for (int i = 0; i < addCount; i++)
				{
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						DomainManager.Combat.AddFlaw(context, base.CurrEnemyChar, 0, this.SkillKey, -1, 1, false);
					}
					else
					{
						DomainManager.Combat.AddAcupoint(context, base.CurrEnemyChar, 0, this.SkillKey, -1, 1, false);
					}
				}
				base.ReduceEffectCount(addCount);
				base.ShowSpecialEffectTipsOnceInFrame(0);
			}
		}

		// Token: 0x0600393E RID: 14654 RVA: 0x0023DDAC File Offset: 0x0023BFAC
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}
	}
}
