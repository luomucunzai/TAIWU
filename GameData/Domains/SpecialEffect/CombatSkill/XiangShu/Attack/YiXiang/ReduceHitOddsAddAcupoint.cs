using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.YiXiang
{
	// Token: 0x020002CA RID: 714
	public class ReduceHitOddsAddAcupoint : CombatSkillEffectBase
	{
		// Token: 0x06003286 RID: 12934 RVA: 0x0021FC77 File Offset: 0x0021DE77
		protected ReduceHitOddsAddAcupoint()
		{
		}

		// Token: 0x06003287 RID: 12935 RVA: 0x0021FC81 File Offset: 0x0021DE81
		protected ReduceHitOddsAddAcupoint(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06003288 RID: 12936 RVA: 0x0021FC90 File Offset: 0x0021DE90
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 107, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003289 RID: 12937 RVA: 0x0021FCFD File Offset: 0x0021DEFD
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600328A RID: 12938 RVA: 0x0021FD38 File Offset: 0x0021DF38
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = hit || pursueIndex != 0 || defender != base.CombatChar;
			if (!flag)
			{
				this.AddAcupoint(context);
			}
		}

		// Token: 0x0600328B RID: 12939 RVA: 0x0021FD6C File Offset: 0x0021DF6C
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = hit || index > 2 || context.Defender != base.CombatChar;
			if (!flag)
			{
				this.AddAcupoint(context);
			}
		}

		// Token: 0x0600328C RID: 12940 RVA: 0x0021FDAC File Offset: 0x0021DFAC
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0600328D RID: 12941 RVA: 0x0021FDE4 File Offset: 0x0021DFE4
		private void AddAcupoint(DataContext context)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			for (int i = 0; i < (int)this.AcupointCount; i++)
			{
				DomainManager.Combat.AddAcupoint(context, enemyChar, 2, this.SkillKey, -1, 1, true);
			}
			DomainManager.Combat.AddToCheckFallenSet(enemyChar.GetId());
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x0600328E RID: 12942 RVA: 0x0021FE54 File Offset: 0x0021E054
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
				bool flag2 = dataKey.FieldId == 107;
				if (flag2)
				{
					result = -50;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000EF4 RID: 3828
		private const sbyte ReduceHitOdds = -50;

		// Token: 0x04000EF5 RID: 3829
		private const sbyte AcupointLevel = 2;

		// Token: 0x04000EF6 RID: 3830
		protected sbyte AcupointCount;
	}
}
