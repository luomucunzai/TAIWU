using System;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.XiangShu
{
	// Token: 0x020002DB RID: 731
	public class XinMiWuJue : CombatSkillEffectBase
	{
		// Token: 0x060032DC RID: 13020 RVA: 0x00221227 File Offset: 0x0021F427
		public XinMiWuJue()
		{
		}

		// Token: 0x060032DD RID: 13021 RVA: 0x00221231 File Offset: 0x0021F431
		public XinMiWuJue(CombatSkillKey skillKey) : base(skillKey, 17093, -1)
		{
		}

		// Token: 0x060032DE RID: 13022 RVA: 0x00221242 File Offset: 0x0021F442
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x060032DF RID: 13023 RVA: 0x0022127B File Offset: 0x0021F47B
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x060032E0 RID: 13024 RVA: 0x002212B4 File Offset: 0x0021F4B4
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = !this.IsSrcSkillPerformed;
			if (flag)
			{
				bool flag2 = charId != base.CharacterId || skillId != base.SkillTemplateId;
				if (!flag2)
				{
					this.IsSrcSkillPerformed = true;
					base.AddMaxEffectCount(true);
				}
			}
			else
			{
				bool flag3 = charId == base.CharacterId && skillId == base.SkillTemplateId;
				if (flag3)
				{
					base.RemoveSelf(context);
				}
				else
				{
					bool flag4 = isAlly != base.CombatChar.IsAlly && Config.CombatSkill.Instance[skillId].EquipType == 1;
					if (flag4)
					{
						DomainManager.Combat.AddGoneMadInjury(context, DomainManager.Combat.GetElement_CombatCharacterDict(charId), skillId, 200);
						base.ShowSpecialEffectTips(0);
					}
				}
			}
		}

		// Token: 0x060032E1 RID: 13025 RVA: 0x00221380 File Offset: 0x0021F580
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = isAlly == base.CombatChar.IsAlly || Config.CombatSkill.Instance[skillId].EquipType != 1;
			if (!flag)
			{
				bool flag2 = power < 100 && !interrupted;
				if (flag2)
				{
					DomainManager.Combat.SilenceSkill(context, DomainManager.Combat.GetElement_CombatCharacterDict(charId), skillId, -1, -1);
					base.ShowSpecialEffectTips(1);
				}
				base.ReduceEffectCount(1);
			}
		}

		// Token: 0x060032E2 RID: 13026 RVA: 0x002213FC File Offset: 0x0021F5FC
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000F0A RID: 3850
		private const short AddGoneMadInjury = 200;
	}
}
