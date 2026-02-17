using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Throw
{
	// Token: 0x020004E0 RID: 1248
	public class WuYingLiuShou : CombatSkillEffectBase
	{
		// Token: 0x06003DC6 RID: 15814 RVA: 0x0025360B File Offset: 0x0025180B
		private bool IsAffectChar(CombatCharacter combatChar)
		{
			return combatChar.IsAlly != base.CombatChar.IsAlly;
		}

		// Token: 0x06003DC7 RID: 15815 RVA: 0x00253623 File Offset: 0x00251823
		public WuYingLiuShou()
		{
		}

		// Token: 0x06003DC8 RID: 15816 RVA: 0x0025362D File Offset: 0x0025182D
		public WuYingLiuShou(CombatSkillKey skillKey) : base(skillKey, 13302, -1)
		{
		}

		// Token: 0x06003DC9 RID: 15817 RVA: 0x00253640 File Offset: 0x00251840
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_AddInjury(new Events.OnAddInjury(this.OnAddInjury));
			Events.RegisterHandler_AddMindMark(new Events.OnAddMindMark(this.OnAddMindMark));
			Events.RegisterHandler_FlawAdded(new Events.OnFlawAdded(this.OnFlawAdded));
			Events.RegisterHandler_AcuPointAdded(new Events.OnAcuPointAdded(this.OnAcuPointAdded));
		}

		// Token: 0x06003DCA RID: 15818 RVA: 0x002536BC File Offset: 0x002518BC
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_AddInjury(new Events.OnAddInjury(this.OnAddInjury));
			Events.UnRegisterHandler_AddMindMark(new Events.OnAddMindMark(this.OnAddMindMark));
			Events.UnRegisterHandler_FlawAdded(new Events.OnFlawAdded(this.OnFlawAdded));
			Events.UnRegisterHandler_AcuPointAdded(new Events.OnAcuPointAdded(this.OnAcuPointAdded));
		}

		// Token: 0x06003DCB RID: 15819 RVA: 0x00253736 File Offset: 0x00251936
		private void OnCombatBegin(DataContext context)
		{
			base.AddMaxEffectCount(true);
		}

		// Token: 0x06003DCC RID: 15820 RVA: 0x00253744 File Offset: 0x00251944
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					base.AddMaxEffectCount(true);
				}
			}
		}

		// Token: 0x06003DCD RID: 15821 RVA: 0x00253788 File Offset: 0x00251988
		private void OnAddInjury(DataContext context, CombatCharacter character, sbyte bodyPart, bool isInner, sbyte value, bool changeToOld)
		{
			bool flag = this.IsAffectChar(character);
			if (flag)
			{
				this.DoAddTrick(context);
			}
		}

		// Token: 0x06003DCE RID: 15822 RVA: 0x002537AC File Offset: 0x002519AC
		private void OnAddMindMark(DataContext context, CombatCharacter character, int count)
		{
			bool flag = this.IsAffectChar(character) && count > 0;
			if (flag)
			{
				this.DoAddTrick(context);
			}
		}

		// Token: 0x06003DCF RID: 15823 RVA: 0x002537D8 File Offset: 0x002519D8
		private void OnFlawAdded(DataContext context, CombatCharacter combatChar, sbyte bodyPart, sbyte level)
		{
			bool flag = this.IsAffectChar(combatChar);
			if (flag)
			{
				this.DoAddTrick(context);
			}
		}

		// Token: 0x06003DD0 RID: 15824 RVA: 0x002537FC File Offset: 0x002519FC
		private void OnAcuPointAdded(DataContext context, CombatCharacter combatChar, sbyte bodyPart, sbyte level)
		{
			bool flag = this.IsAffectChar(combatChar);
			if (flag)
			{
				this.DoAddTrick(context);
			}
		}

		// Token: 0x06003DD1 RID: 15825 RVA: 0x00253820 File Offset: 0x00251A20
		private void DoAddTrick(DataContext context)
		{
			bool flag = base.EffectCount <= 0 || !base.IsCurrent;
			if (!flag)
			{
				base.ReduceEffectCount(1);
				DomainManager.Combat.AddTrick(context, base.IsDirect ? base.CombatChar : base.EnemyChar, 19, base.IsDirect);
				base.ShowSpecialEffectTips(0);
			}
		}
	}
}
